using MovitCodeTools.Common;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MovitCodeTools.Template;
using System.IO;

namespace MovitCodeTools
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> _dbConfig;
        private static string savePath;
        string basePath => Environment.CurrentDirectory.Replace("\\temp", ""); // 向上进一级

        private List<dynamic> _tableList;

        public Form1()
        {
            InitializeComponent();
            InitData();
            // 初始化一些控件属性
            
            lboxMessage.Visible = false;
            cboxDatabase.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboxDatabase.AutoCompleteSource = AutoCompleteSource.ListItems;
            chklboxTable.CheckOnClick = true;
            chklboxTable.MultiColumn = true;
            chklboxTable.IntegralHeight = true;
            chklboxTable.ColumnWidth = 249;
            chklboxTable.Visible = true;
            
        }


        public void InitData()
        {
            //初始化数据
            // 获取数据库的配置
            //var allFolder = new DirectoryInfo(basePath).GetDirectories();
            //var webPath = allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("webmvc")) ?? allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("web"));
            try
            {
                _dbConfig = null;
                _dbConfig = ConfigSettingHelper.GetExeConnectionStrDictionary();
                if (_dbConfig != null && _dbConfig.ContainsKey("LocalSqlServer"))
                {
                    _dbConfig.Remove("LocalSqlServer");
                }
                cboxDatabase.Items.Clear();
                foreach (var item in _dbConfig)
                {
                    cboxDatabase.Items.Add(item.Key);
                }
            }
            catch (Exception e)
            {
                MessageBoxEx.Show(e.Message);
                //MessageBoxEx.Show("ERROR：请确保web层下的App_Data目录有配置appSettings.config。且配置正确无重复配置");
                //MessageBoxEx.Show("ERROR：请确保web层下的App_Data目录有配置dbconfig.json。且配置正确无重复配置");
            }

        }
        /// <summary>
        /// 新建MySql数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBConnConfigForm form = new DBConnConfigForm();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                this.InitData();//重新绑定 
            }
        }

        private void txtTableSearch_TextChanged(object sender, EventArgs e)
        {
            chklboxTable.Items.Clear();
            if (string.IsNullOrWhiteSpace(txtTableSearch.Text.Trim(' ')))
            {
                if (_tableList != null && _tableList.Any())
                {
                    foreach (var item in _tableList)
                    {
                        chklboxTable.Items.Add($"{item.tableName}（{item.tableComment}）");
                    }
                }
                return;
            }
            if (_tableList != null && _tableList.Any())
            {
                foreach (var item in _tableList)
                {
                    if (item.tableName.ToLower().Contains(txtTableSearch.Text.ToLower()))
                    {
                        chklboxTable.Items.Add($"{item.tableName}（{item.tableComment}）");
                    }
                }
            }
        }

        private void btnBuilder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();

            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                savePath = fd.SelectedPath;
            }
            else {
                lboxMessage.Items.Add("=======请选择路径=======");
                return;
            }
            chklboxTable.Visible = false;
            lboxMessage.Visible = true;
            lboxMessage.Items.Clear();
            for (int i = 0; i < chklboxTable.Items.Count; i++)
            {
                if (chklboxTable.GetItemCheckState(i) == CheckState.Checked)
                {
                    lboxMessage.Items.Add($"=======生成{chklboxTable.Items[i]}=======");
                    var item = chklboxTable.Items[i];
                    var tableName = item.ToString().Split(new[] { "（" }, StringSplitOptions.RemoveEmptyEntries)[0];//去掉表名注释
                    WriteCode(cboxDatabase.SelectedItem.ToString(), tableName);
                }
            }
            lboxMessage.Items.Add("=======双击关闭关闭当前视图=======");
        }
        /// <summary>
        /// 重置表选择框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chklboxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chklboxTable.SelectedItem.ToString() != "全选" && chklboxTable.SelectedItem.ToString() != "全不选")
                return;

            var clone = JsonConvert.DeserializeObject<List<object>>(JsonConvert.SerializeObject(chklboxTable.Items));
            clone.RemoveAll(x => x.ToString() == "全选" || x.ToString() == "全不选");

            if (chklboxTable.SelectedItem.ToString() == "全选")
            {
                chklboxTable.Items.Clear();
                chklboxTable.Items.Add("全不选");
                foreach (var item in clone)
                {
                    chklboxTable.Items.Add(item, true);
                }
            }
            else if (chklboxTable.SelectedItem.ToString() == "全不选")
            {
                chklboxTable.Items.Clear();
                chklboxTable.Items.Add("全选");
                foreach (var item in clone)
                {
                    chklboxTable.Items.Add(item, false);
                }
            }
        }
        /// <summary>
        /// 数据库选择后刷新表名列表框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxDatabase.SelectedItem == null)
            {
                return;
            }
            var connStr = _dbConfig[cboxDatabase.SelectedItem.ToString()];

            string sqlStr = SqlServerDBA.AllTablesSql();

            using (DbConnection conn = new SqlConnection(connStr))
            {
                _tableList = conn.Query<dynamic>(sqlStr).ToList();//nuget Dapper
                chklboxTable.Items.Clear();
                chklboxTable.Items.Add("全选");
                foreach (var item in _tableList)
                {
                    chklboxTable.Items.Add($"{item.tableName}（{item.tableComment}）");
                }
            }
        }

        private void lboxMessage_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            chklboxTable.Visible = true;
            lboxMessage.Visible = false;
        }

        private void sQLServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBConnConfigForm form = new DBConnConfigForm();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                this.InitData();//重新绑定 
            }
        }



        #region 代码生成器
        void WriteCode(string dbName, string tableName)
        {
            var solution = "WebDemo";//arrBasePath.Skip(arrBasePath.Count - 1).FirstOrDefault().Split('.')[0];
            var path = savePath;
            ShowLog("保存路径:" + path);
            try
            {
                //var allFolder = new DirectoryInfo(basePath).GetDirectories();
                string columnsSql = SqlServerDBA.AllColumnsSql();
                var connStr = _dbConfig[dbName];

                using (DbConnection conn = new SqlConnection(connStr))
                {
                    var tables = conn.Query<TableDetailsInfo>(columnsSql, new
                    {
                        dbName,
                        tableName
                    }).ToList();

                    var tableDescribe = string.Empty;

                    #region model

                    var modelContent = EntityTemplate.GetTemplateString(tableName, tables, solution);
                    WriteToFile(path, $"{tableName.Replace("_", "")}.cs", modelContent);
                    //ShowLog(modelContent);
                    #endregion

                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        void WriteToFile(string thatPath, string filePath, string content)
        {
            var arr = filePath.Split('/').ToList();
            var directory = string.Join("/", arr.Where(x => arr.IndexOf(x) != arr.Count - 1)); // 删除掉最后的文件路径
            var path = $"{thatPath}/{directory}";
            
            File.WriteAllText($"{thatPath}/{filePath}", content, Encoding.UTF8);
            var arrPath = thatPath.Split('/');
            //var csproj = arrPath.Skip(arrPath.Length - 1).FirstOrDefault();
            //ShowLog($"写入{filePath.Replace("/", "\\")}到{thatPath}/{csproj}.csproj文件");
            //CsprojHelper.AddProject($"{thatPath}/{csproj}.csproj", filePath.Replace("/", "\\"));
        }

        /// <summary>
        /// 打印消息到消息框
        /// </summary>
        /// <param name="msg"></param>
        private void ShowLog(string msg)
        {
            var visibleItems = lboxMessage.ClientSize.Height / lboxMessage.ItemHeight;
            lboxMessage.TopIndex = Math.Max(lboxMessage.Items.Count - visibleItems + 1, 0);
            lboxMessage.Items.Add(msg);
        }
        #endregion

        private void btnSetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            //txtPath.Text = path.SelectedPath;
        }
    }
}
