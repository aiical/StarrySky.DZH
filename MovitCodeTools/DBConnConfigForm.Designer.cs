namespace MovitCodeTools
{
    partial class DBConnConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDBPort = new System.Windows.Forms.TextBox();
            this.txtDBPassword = new System.Windows.Forms.TextBox();
            this.txtDBIP = new System.Windows.Forms.TextBox();
            this.txtDBUserName = new System.Windows.Forms.TextBox();
            this.btnAddConfig = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtxtConnStr = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtDBPort
            // 
            this.txtDBPort.Location = new System.Drawing.Point(202, 153);
            this.txtDBPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtDBPort.Name = "txtDBPort";
            this.txtDBPort.Size = new System.Drawing.Size(114, 28);
            this.txtDBPort.TabIndex = 16;
            this.txtDBPort.Text = "port";
            // 
            // txtDBPassword
            // 
            this.txtDBPassword.Location = new System.Drawing.Point(202, 264);
            this.txtDBPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtDBPassword.Name = "txtDBPassword";
            this.txtDBPassword.Size = new System.Drawing.Size(352, 28);
            this.txtDBPassword.TabIndex = 17;
            // 
            // txtDBIP
            // 
            this.txtDBIP.Location = new System.Drawing.Point(202, 102);
            this.txtDBIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtDBIP.Name = "txtDBIP";
            this.txtDBIP.Size = new System.Drawing.Size(352, 28);
            this.txtDBIP.TabIndex = 18;
            this.txtDBIP.Text = "ip";
            // 
            // txtDBUserName
            // 
            this.txtDBUserName.Location = new System.Drawing.Point(202, 212);
            this.txtDBUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtDBUserName.Name = "txtDBUserName";
            this.txtDBUserName.Size = new System.Drawing.Size(352, 28);
            this.txtDBUserName.TabIndex = 19;
            // 
            // btnAddConfig
            // 
            this.btnAddConfig.Location = new System.Drawing.Point(293, 394);
            this.btnAddConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddConfig.Name = "btnAddConfig";
            this.btnAddConfig.Size = new System.Drawing.Size(112, 34);
            this.btnAddConfig.TabIndex = 14;
            this.btnAddConfig.Text = "添加连接";
            this.btnAddConfig.UseVisualStyleBackColor = true;
            this.btnAddConfig.Click += new System.EventHandler(this.btnAddConfig_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(117, 394);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(112, 34);
            this.btnTest.TabIndex = 15;
            this.btnTest.Text = "测试连接";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(132, 268);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "密码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(114, 216);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "用户名：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 158);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "端口：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "主机：";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(202, 52);
            this.txtDBName.Margin = new System.Windows.Forms.Padding(4);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(352, 28);
            this.txtDBName.TabIndex = 9;
            this.txtDBName.Text = "dbname";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 8;
            this.label1.Text = "数据库名称：";
            // 
            // rtxtConnStr
            // 
            this.rtxtConnStr.Location = new System.Drawing.Point(202, 319);
            this.rtxtConnStr.Name = "rtxtConnStr";
            this.rtxtConnStr.Size = new System.Drawing.Size(352, 68);
            this.rtxtConnStr.TabIndex = 20;
            this.rtxtConnStr.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(96, 332);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 18);
            this.label6.TabIndex = 21;
            this.label6.Text = "连接字串：";
            // 
            // DBConnConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 465);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.rtxtConnStr);
            this.Controls.Add(this.txtDBPort);
            this.Controls.Add(this.txtDBPassword);
            this.Controls.Add(this.txtDBIP);
            this.Controls.Add(this.txtDBUserName);
            this.Controls.Add(this.btnAddConfig);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DBConnConfigForm";
            this.Text = "DBConnConfigForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDBPort;
        private System.Windows.Forms.TextBox txtDBPassword;
        private System.Windows.Forms.TextBox txtDBIP;
        private System.Windows.Forms.TextBox txtDBUserName;
        private System.Windows.Forms.Button btnAddConfig;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtxtConnStr;
        private System.Windows.Forms.Label label6;
    }
}