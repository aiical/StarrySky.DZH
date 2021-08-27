using MovitCodeTools.Common;
using MovitCodeTools.Model;
using MovitCodeTools.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovitCodeTools
{
    public partial class DBConnConfigForm : Form
    {
        public DBConnConfigForm()
        {
            InitializeComponent();
        }

        public static DBConfigModel dbModel = null;

        public static bool IsRight = false;

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            IsRight = false;
            //if (!string.IsNullOrEmpty(rtxtConnStr.Text)) {
                //var flag1 = new SqlServerTester().TestConnection(rtxtConnStr.Text);
                //if (flag1)
                //{
                //    IsRight = true;
                //    MessageBox.Show("连接成功！");
                //}
                //else
                //{
                //    MessageBox.Show("连接失败！");
                //    return;
                //}
            //}
            if (dbModel == null)
            {
                dbModel = new DBConfigModel();
            }
            dbModel.DBName = txtDBName.Text;
            dbModel.IP = txtDBIP.Text;
            dbModel.Port = txtDBPort.Text;
            dbModel.UserName = txtDBUserName.Text;
            dbModel.Password = txtDBPassword.Text;
            var flag = new SqlServerTester().TestConnection(dbModel);
            if (flag)
            {
                IsRight = true;
                MessageBox.Show("连接成功！");
            }
            else
            {
                MessageBox.Show("连接失败！");
                return;
            }
        }

        private void btnAddConfig_Click(object sender, EventArgs e)
        {
            if (!IsRight || dbModel == null)
            {
                MessageBox.Show("请点击连接测试！");
                return;
            }
            ConfigSettingHelper.UpdateExeConnStriConfig(dbModel.DBName, DBConfigUtil.GenerateSqlServerDBConnectionString(dbModel));
            this.DialogResult = DialogResult.OK;
            MessageBox.Show("添加成功！");
            this.Close();
        }
    }
}
