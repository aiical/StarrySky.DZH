namespace GenerateToolApp
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cboxDatabase = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mySQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboxVersion = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkEntity = new System.Windows.Forms.CheckBox();
            this.lboxMessage = new System.Windows.Forms.ListBox();
            this.chkDAL = new System.Windows.Forms.CheckBox();
            this.chkService = new System.Windows.Forms.CheckBox();
            this.chkWeb = new System.Windows.Forms.CheckBox();
            this.chklboxTable = new System.Windows.Forms.CheckedListBox();
            this.btnBuilder = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSetPath = new System.Windows.Forms.Button();
            this.chkModel = new System.Windows.Forms.CheckBox();
            this.rbFilePath = new System.Windows.Forms.RadioButton();
            this.rbProject = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTableSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboxDatabase
            // 
            this.cboxDatabase.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxDatabase.FormattingEnabled = true;
            this.cboxDatabase.Location = new System.Drawing.Point(150, 44);
            this.cboxDatabase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboxDatabase.Name = "cboxDatabase";
            this.cboxDatabase.Size = new System.Drawing.Size(511, 37);
            this.cboxDatabase.TabIndex = 27;
            this.cboxDatabase.SelectedIndexChanged += new System.EventHandler(this.cboxDatabase_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 26;
            this.label1.Text = "数据库：";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1406, 34);
            this.menuStrip1.TabIndex = 35;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接ToolStripMenuItem});
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(62, 28);
            this.新建ToolStripMenuItem.Text = "新建";
            // 
            // 连接ToolStripMenuItem
            // 
            this.连接ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mySQLToolStripMenuItem,
            this.sQLServerToolStripMenuItem});
            this.连接ToolStripMenuItem.Name = "连接ToolStripMenuItem";
            this.连接ToolStripMenuItem.Size = new System.Drawing.Size(182, 34);
            this.连接ToolStripMenuItem.Text = "新建连接";
            // 
            // mySQLToolStripMenuItem
            // 
            this.mySQLToolStripMenuItem.Name = "mySQLToolStripMenuItem";
            this.mySQLToolStripMenuItem.Size = new System.Drawing.Size(202, 34);
            this.mySQLToolStripMenuItem.Text = "MySQL";
            this.mySQLToolStripMenuItem.Click += new System.EventHandler(this.mySQLToolStripMenuItem_Click);
            // 
            // sQLServerToolStripMenuItem
            // 
            this.sQLServerToolStripMenuItem.Name = "sQLServerToolStripMenuItem";
            this.sQLServerToolStripMenuItem.Size = new System.Drawing.Size(202, 34);
            this.sQLServerToolStripMenuItem.Text = "SQL Server";
            // 
            // cboxVersion
            // 
            this.cboxVersion.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxVersion.FormattingEnabled = true;
            this.cboxVersion.Items.AddRange(new object[] {
            "V1"});
            this.cboxVersion.Location = new System.Drawing.Point(765, 42);
            this.cboxVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboxVersion.Name = "cboxVersion";
            this.cboxVersion.Size = new System.Drawing.Size(128, 37);
            this.cboxVersion.TabIndex = 55;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(694, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 18);
            this.label4.TabIndex = 54;
            this.label4.Text = "版本：";
            // 
            // chkEntity
            // 
            this.chkEntity.AutoSize = true;
            this.chkEntity.Location = new System.Drawing.Point(753, 160);
            this.chkEntity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkEntity.Name = "chkEntity";
            this.chkEntity.Size = new System.Drawing.Size(106, 22);
            this.chkEntity.TabIndex = 63;
            this.chkEntity.Text = "entity层";
            this.chkEntity.UseVisualStyleBackColor = true;
            // 
            // lboxMessage
            // 
            this.lboxMessage.FormattingEnabled = true;
            this.lboxMessage.ItemHeight = 18;
            this.lboxMessage.Location = new System.Drawing.Point(88, 219);
            this.lboxMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lboxMessage.Name = "lboxMessage";
            this.lboxMessage.Size = new System.Drawing.Size(1240, 670);
            this.lboxMessage.TabIndex = 58;
            this.lboxMessage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lboxMessage_MouseDoubleClick);
            // 
            // chkDAL
            // 
            this.chkDAL.AutoSize = true;
            this.chkDAL.Location = new System.Drawing.Point(504, 162);
            this.chkDAL.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkDAL.Name = "chkDAL";
            this.chkDAL.Size = new System.Drawing.Size(79, 22);
            this.chkDAL.TabIndex = 61;
            this.chkDAL.Text = "DAL层";
            this.chkDAL.UseVisualStyleBackColor = true;
            // 
            // chkService
            // 
            this.chkService.AutoSize = true;
            this.chkService.Location = new System.Drawing.Point(354, 160);
            this.chkService.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkService.Name = "chkService";
            this.chkService.Size = new System.Drawing.Size(115, 22);
            this.chkService.TabIndex = 60;
            this.chkService.Text = "Service层";
            this.chkService.UseVisualStyleBackColor = true;
            // 
            // chkWeb
            // 
            this.chkWeb.AutoSize = true;
            this.chkWeb.Location = new System.Drawing.Point(237, 162);
            this.chkWeb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkWeb.Name = "chkWeb";
            this.chkWeb.Size = new System.Drawing.Size(79, 22);
            this.chkWeb.TabIndex = 59;
            this.chkWeb.Text = "Web层";
            this.chkWeb.UseVisualStyleBackColor = true;
            // 
            // chklboxTable
            // 
            this.chklboxTable.FormattingEnabled = true;
            this.chklboxTable.Location = new System.Drawing.Point(68, 195);
            this.chklboxTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chklboxTable.Name = "chklboxTable";
            this.chklboxTable.Size = new System.Drawing.Size(1249, 654);
            this.chklboxTable.TabIndex = 57;
            this.chklboxTable.SelectedIndexChanged += new System.EventHandler(this.chklboxTable_SelectedIndexChanged);
            // 
            // btnBuilder
            // 
            this.btnBuilder.Location = new System.Drawing.Point(478, 918);
            this.btnBuilder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBuilder.Name = "btnBuilder";
            this.btnBuilder.Size = new System.Drawing.Size(410, 56);
            this.btnBuilder.TabIndex = 56;
            this.btnBuilder.Text = "生成";
            this.btnBuilder.UseVisualStyleBackColor = true;
            this.btnBuilder.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 114);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 54;
            this.label2.Text = "保存路径：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(150, 105);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(446, 28);
            this.txtPath.TabIndex = 64;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnSetPath
            // 
            this.btnSetPath.Location = new System.Drawing.Point(594, 105);
            this.btnSetPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSetPath.Name = "btnSetPath";
            this.btnSetPath.Size = new System.Drawing.Size(56, 36);
            this.btnSetPath.TabIndex = 65;
            this.btnSetPath.Text = "...";
            this.btnSetPath.UseVisualStyleBackColor = true;
            this.btnSetPath.Click += new System.EventHandler(this.btnSetPath_Click);
            // 
            // chkModel
            // 
            this.chkModel.AutoSize = true;
            this.chkModel.Location = new System.Drawing.Point(608, 160);
            this.chkModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkModel.Name = "chkModel";
            this.chkModel.Size = new System.Drawing.Size(97, 22);
            this.chkModel.TabIndex = 62;
            this.chkModel.Text = "model层";
            this.chkModel.UseVisualStyleBackColor = true;
            // 
            // rbFilePath
            // 
            this.rbFilePath.AutoSize = true;
            this.rbFilePath.Location = new System.Drawing.Point(144, 12);
            this.rbFilePath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbFilePath.Name = "rbFilePath";
            this.rbFilePath.Size = new System.Drawing.Size(105, 22);
            this.rbFilePath.TabIndex = 0;
            this.rbFilePath.TabStop = true;
            this.rbFilePath.Text = "指定路径";
            this.rbFilePath.UseVisualStyleBackColor = true;
            // 
            // rbProject
            // 
            this.rbProject.AutoSize = true;
            this.rbProject.Location = new System.Drawing.Point(28, 12);
            this.rbProject.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbProject.Name = "rbProject";
            this.rbProject.Size = new System.Drawing.Size(105, 22);
            this.rbProject.TabIndex = 0;
            this.rbProject.TabStop = true;
            this.rbProject.Text = "特定项目";
            this.rbProject.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(928, 56);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 26;
            this.label3.Text = "储存方式：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbProject);
            this.panel1.Controls.Add(this.rbFilePath);
            this.panel1.Location = new System.Drawing.Point(1054, 42);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 48);
            this.panel1.TabIndex = 66;
            // 
            // txtTableSearch
            // 
            this.txtTableSearch.Location = new System.Drawing.Point(788, 110);
            this.txtTableSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTableSearch.Name = "txtTableSearch";
            this.txtTableSearch.Size = new System.Drawing.Size(367, 28);
            this.txtTableSearch.TabIndex = 67;
            this.txtTableSearch.TextChanged += new System.EventHandler(this.txtTableSearch_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(694, 114);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 18);
            this.label5.TabIndex = 54;
            this.label5.Text = "搜索表名：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1406, 992);
            this.Controls.Add(this.txtTableSearch);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSetPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.chkEntity);
            this.Controls.Add(this.lboxMessage);
            this.Controls.Add(this.chkModel);
            this.Controls.Add(this.chkDAL);
            this.Controls.Add(this.chkService);
            this.Controls.Add(this.chkWeb);
            this.Controls.Add(this.chklboxTable);
            this.Controls.Add(this.btnBuilder);
            this.Controls.Add(this.cboxVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboxDatabase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cboxDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mySQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQLServerToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboxVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkEntity;
        private System.Windows.Forms.ListBox lboxMessage;
        private System.Windows.Forms.CheckBox chkDAL;
        private System.Windows.Forms.CheckBox chkService;
        private System.Windows.Forms.CheckBox chkWeb;
        private System.Windows.Forms.CheckedListBox chklboxTable;
        private System.Windows.Forms.Button btnBuilder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSetPath;
        private System.Windows.Forms.CheckBox chkModel;
        private System.Windows.Forms.RadioButton rbProject;
        private System.Windows.Forms.RadioButton rbFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtTableSearch;
        private System.Windows.Forms.Label label5;
    }
}

