namespace GenerateToolApp
{
    partial class AppCreateDBConnForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnAddConfig = new System.Windows.Forms.Button();
            this.txtDBUserName = new System.Windows.Forms.TextBox();
            this.txtDBIP = new System.Windows.Forms.TextBox();
            this.txtDBPassword = new System.Windows.Forms.TextBox();
            this.txtDBPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库名称：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(115, 30);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(236, 21);
            this.txtDBName.TabIndex = 1;
            this.txtDBName.Text = "TCSURPRISEGAMELOG";
            this.txtDBName.TextChanged += new System.EventHandler(this.txtDBName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "主机：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "端口：";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "用户名：";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "密码：";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(62, 226);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "测试连接";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnAddConfig
            // 
            this.btnAddConfig.Location = new System.Drawing.Point(180, 226);
            this.btnAddConfig.Name = "btnAddConfig";
            this.btnAddConfig.Size = new System.Drawing.Size(75, 23);
            this.btnAddConfig.TabIndex = 6;
            this.btnAddConfig.Text = "添加连接";
            this.btnAddConfig.UseVisualStyleBackColor = true;
            this.btnAddConfig.Click += new System.EventHandler(this.btnAddConfig_Click);
            // 
            // txtDBUserName
            // 
            this.txtDBUserName.Location = new System.Drawing.Point(115, 136);
            this.txtDBUserName.Name = "txtDBUserName";
            this.txtDBUserName.Size = new System.Drawing.Size(236, 21);
            this.txtDBUserName.TabIndex = 7;
            this.txtDBUserName.Text = "TCSURPRISEGAMELOG_TEST";
            this.txtDBUserName.TextChanged += new System.EventHandler(this.txtDBUserName_TextChanged);
            // 
            // txtDBIP
            // 
            this.txtDBIP.Location = new System.Drawing.Point(115, 63);
            this.txtDBIP.Name = "txtDBIP";
            this.txtDBIP.Size = new System.Drawing.Size(236, 21);
            this.txtDBIP.TabIndex = 7;
            this.txtDBIP.Text = "10.100.41.4";
            this.txtDBIP.TextChanged += new System.EventHandler(this.txtDBIP_TextChanged);
            // 
            // txtDBPassword
            // 
            this.txtDBPassword.Location = new System.Drawing.Point(115, 171);
            this.txtDBPassword.Name = "txtDBPassword";
            this.txtDBPassword.Size = new System.Drawing.Size(236, 21);
            this.txtDBPassword.TabIndex = 7;
            this.txtDBPassword.Text = "uGT3lRPC9SR0ABc3Y2a8qrE";
            this.txtDBPassword.TextChanged += new System.EventHandler(this.txtDBPassword_TextChanged);
            // 
            // txtDBPort
            // 
            this.txtDBPort.Location = new System.Drawing.Point(115, 97);
            this.txtDBPort.Name = "txtDBPort";
            this.txtDBPort.Size = new System.Drawing.Size(77, 21);
            this.txtDBPort.TabIndex = 7;
            this.txtDBPort.Text = "3818";
            this.txtDBPort.TextChanged += new System.EventHandler(this.txtDBPort_TextChanged);
            // 
            // AppCreateDBConnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 281);
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
            this.Name = "AppCreateDBConnForm";
            this.Text = "创建数据库连接";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnAddConfig;
        private System.Windows.Forms.TextBox txtDBUserName;
        private System.Windows.Forms.TextBox txtDBIP;
        private System.Windows.Forms.TextBox txtDBPassword;
        private System.Windows.Forms.TextBox txtDBPort;
    }
}