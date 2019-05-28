using Dapper;
using DZHCodeBuilder.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using StarrySky.DZH.Util.Extensions;
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

namespace DZHCodeBuilder
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> _dbConfig;
        private static string savePath;
        private List<dynamic> _tableList;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化数据库选择框的数据源
        /// </summary>
        public void InitDBConfig()
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
                MessageBox.Show(e.Message);
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
                this.InitDBConfig();//重新绑定 
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
            if (rbFilePath.Checked && txtPath.Text.IsNullOrWhiteSpace())
            {
                MessageBox.Show("请选择保存地址");
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
                                                                                                                   //WriteCode(cboxDatabase.SelectedItem.ToString(), tableName);
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

            string sqlStr = MySqlDBA.AllTablesSql(connStr);

            using (DbConnection conn = new MySqlConnection(connStr))
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

        /// <summary>
        /// 设置路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            txtPath.Text = path.SelectedPath;
        }
    }
}
