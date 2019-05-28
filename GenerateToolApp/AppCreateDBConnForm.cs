using GenerateToolApp.Common;
using GenerateToolApp.Model;
using GenerateToolApp.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateToolApp
{
    public partial class AppCreateDBConnForm : Form
    {

        public AppCreateDBConnForm()
        {
            InitializeComponent();
        }
        public static DatabaseModel dbModel = null;
        public static bool IsRight = false;
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            IsRight = false;
            if (dbModel == null) {
                dbModel = new DatabaseModel();
            }
            dbModel.DBName = txtDBName.Text;
            dbModel.IP = txtDBIP.Text;
            dbModel.Port = txtDBPort.Text;
            dbModel.UserName = txtDBUserName.Text;
            dbModel.Password = txtDBPassword.Text;
            var flag=new MySQLTester().TestConnection(dbModel);
            if (flag) {
                IsRight = true;
                MessageBox.Show("连接成功！");
            }
            else {
                MessageBox.Show("连接失败！");
                return;
            }
        }

        private void btnAddConfig_Click(object sender, EventArgs e)
        {
            if (!IsRight) {
                MessageBox.Show("请点击连接测试！");
                return;
            }
            if (dbModel == null)
            {
                dbModel = new DatabaseModel();
            }
            dbModel.DBName = txtDBName.Text;
            dbModel.IP = txtDBIP.Text;
            dbModel.Port = txtDBPort.Text;
            dbModel.UserName = txtDBUserName.Text;
            dbModel.Password = txtDBPassword.Text;
            ConfigSettingHelper.UpdateExeConnStriConfig(dbModel.DBName, DatabaseConfigUtil.GenerateDBConnectionString(dbModel));
            this.DialogResult = DialogResult.OK;
            MessageBox.Show("添加成功！");
            this.Close();
        }

        private void txtDBName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtDBUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDBIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDBPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDBPort_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
