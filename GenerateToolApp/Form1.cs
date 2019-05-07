using Dapper;
using GenerateToolApp.Common;
using GenerateToolApp.Template;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateToolApp
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> _dbConfig;
        string basePath => Environment.CurrentDirectory.Replace("\\temp", ""); // 向上进一级

        private List<dynamic> _tableList;

        public Form1()
        {
            InitializeComponent();

            InitData();
            // 初始化一些控件属性
            cboxVersion.SelectedIndex = 0;
            lboxMessage.Visible = false;
            cboxDatabase.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboxDatabase.AutoCompleteSource = AutoCompleteSource.ListItems;
            chklboxTable.CheckOnClick = true;
            chklboxTable.MultiColumn = true;
            chklboxTable.IntegralHeight = true;
            chklboxTable.ColumnWidth = 249;
            chklboxTable.Visible = true;
            rbFilePath.Checked = true;
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
        private void mySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppCreateDBConnForm form = new AppCreateDBConnForm();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                this.InitData();//重新绑定 
            }
        }


        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (rbFilePath.Checked && string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBoxEx.Show("请选择保存地址");
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
        /// 打印消息到消息框
        /// </summary>
        /// <param name="msg"></param>
        private void ShowLog(string msg)
        {
            var visibleItems = lboxMessage.ClientSize.Height / lboxMessage.ItemHeight;
            lboxMessage.TopIndex = Math.Max(lboxMessage.Items.Count - visibleItems + 1, 0);
            lboxMessage.Items.Add(msg);
        }


        private void btnSetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            txtPath.Text = path.SelectedPath;
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

        #region 代码生成器
        void WriteCode(string dbName, string tableName)
        {
            //var sln = Directory.GetFiles(basePath).FirstOrDefault(x => x.EndsWith(".sln"));
            //if (sln == null)
            //{
            //    MessageBoxEx.Show("ERROR：没有找到解决方案");
            //}

            //var arrBasePath = sln.Replace("/", "\\").Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var solution = "WebDemo";//arrBasePath.Skip(arrBasePath.Count - 1).FirstOrDefault().Split('.')[0];

            var version = cboxVersion.SelectedIndex;
            var path = txtPath.Text;
            ShowLog("项目路径:" + basePath);
            try
            {
                var allFolder = new DirectoryInfo(basePath).GetDirectories();

                string columnsSql = MySqlDBA.AllColumnsSql();
                var connStr = _dbConfig[dbName];

                using (DbConnection conn = new MySqlConnection(connStr))
                {
                    var tables = conn.Query<dynamic>(columnsSql, new
                    {
                        dbName,
                        tableName
                    });

                    var tableDescribe = string.Empty;

                    #region web
                    if (chkWeb.Checked)
                    {
                        //var webPath = allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("webmvc"))
                        //              ?? allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("web"));
                        var webPath = txtPath.Text;
                        if (webPath != null)
                        {
                            foreach (var item in chklboxTable.Items)
                            {
                                if (item.ToString().Split('（')[0] == tableName)
                                {
                                    tableDescribe = item.ToString().Split('（')[1].Replace("）", "");
                                }
                            }
                            // 列表页
                            var listContent = PageTemplate.GetListPageString(tableName, tables);
                            WriteToFile(path, $"Views/{dbName}/{tableName}/{tableName}List.cshtml", listContent);

                            // 编辑页
                            var editContent = PageTemplate.GetEditPageString(tableName, tables);
                            WriteToFile(path, $"Views/{dbName}/{tableName}/{tableName}Edit.cshtml", editContent);

                            // controller
                            var controllerContent = PageTemplate.GetControllerString(tableName, solution, dbName);
                            WriteToFile(path, $"Controllers/{dbName}/{tableName}Controller.cs", controllerContent);
                            WriteExtend(path, $"Controllers/{dbName}/{tableName}Controller_Extend.cs", controllerContent);
                        }
                        else
                        {
                            ShowLog("错误，未能找到web层");
                            goto OVER;
                        }
                    }
                    #endregion

                    #region model
                    if (chkModel.Checked)
                    {
                        //var modelPath = allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("model"));
                        var modelPath = txtPath.Text+"/Model";
                        if (modelPath != null)
                        {
                            var modelContent = ModelTemplate.GetViewModelString(tableName, tables, solution);
                            WriteToFile(path, $"{dbName}/{tableName}Model.cs", modelContent);
                        }
                        else
                        {
                            ShowLog("错误，未能找到model层");
                            goto OVER;
                        }
                    }
                    #endregion

                    #region entity 
                    if (chkEntity.Checked)
                    {
                        //var entityPath = allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("entity"));
                        var entityPath = txtPath.Text+"/Entity";
                        if (entityPath != null)
                        {
                            var entityContent = EntityTemplate.GetTemplateString(dbName, tableName, tables, solution);
                            WriteToFile(entityPath, $"{dbName}/{tableName}Entity.cs", entityContent);
                        }
                        else
                        {
                            ShowLog("错误，未能找到entity层");
                            goto OVER;
                        }
                    }
                    #endregion

                    #region dal
                    if (chkDAL.Checked)
                    {
                        //var dalPath = allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("dal"));
                        var dalPath = txtPath.Text+"/DAL";
                        if (dalPath != null)
                        {
                            var dalContent = PageTemplate.GetDalString(dbName, tableName, tables, solution);
                            WriteToFile(
                                dalPath,
                                $"{dbName}/{tableName}DAL.cs",
                                dalContent);
                            WriteExtend(
                                dalPath,
                                $"{dbName}/{tableName}DAL_Extend.cs",
                                dalContent);
                        }
                        else
                        {
                            ShowLog("错误，未能找到dal层");
                            goto OVER;
                        }
                    }
                    #endregion

                    #region service
                    if (chkService.Checked)
                    {
                        //var servicePath = allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("services")) ?? allFolder.FirstOrDefault(x => x.Name.ToLower().EndsWith("service")));
                        var servicePath = txtPath.Text+"/Service";
                        if (servicePath != null)
                        {
                            var serviceContent = PageTemplate.GetServiceString(tableName, tables, solution);
                            WriteToFile(
                                servicePath,
                                $"{dbName}/{tableName}Service.cs",
                                serviceContent);
                            WriteExtend(
                                servicePath,
                                $"{dbName}/{tableName}Service_Extend.cs",
                                serviceContent);
                        }
                        else
                        {
                            ShowLog("错误，未能找到service层");
                        }
                    }
                    #endregion
                }

            OVER:
                ShowLog("OVER");
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        void WriteExtend(string path, string file, string code)
        {
            var resultCode = new StringBuilder();
            if (File.Exists(path + "/" + file))
            {
                return;
            }

            var arr = code.Split(new[] { "\r\n" }, StringSplitOptions.None);
            foreach (var line in arr)
            {
                if (line.TrimStart().StartsWith("namespace "))
                {
                    resultCode.AppendLine(line);
                    resultCode.AppendLine("{");
                }

                else if (line.TrimStart().StartsWith("public partial class "))
                {
                    resultCode.AppendLine("    " + line.Split(':')[0].Trim());
                    resultCode.AppendLine("    {");
                    resultCode.AppendLine("    }");
                }
            }
            resultCode.AppendLine("}");
            WriteToFile(path, file, resultCode.ToString());
        }

        void WriteToFile(string thatPath, string filePath, string content)
        {
            var arr = filePath.Split('/').ToList();
            var directory = string.Join("/", arr.Where(x => arr.IndexOf(x) != arr.Count - 1)); // 删除掉最后的文件路径
            var path = $"{thatPath}/{directory}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText($"{thatPath}/{filePath}", content, Encoding.UTF8);
            var arrPath = thatPath.Split('/');
            //var csproj = arrPath.Skip(arrPath.Length - 1).FirstOrDefault();
            //ShowLog($"写入{filePath.Replace("/", "\\")}到{thatPath}/{csproj}.csproj文件");
            //CsprojHelper.AddProject($"{thatPath}/{csproj}.csproj", filePath.Replace("/", "\\"));
        }


        #endregion

    }
}
