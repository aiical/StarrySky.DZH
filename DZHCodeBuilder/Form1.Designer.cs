namespace DZHCodeBuilder
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mySQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtTableSearch = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbProject = new System.Windows.Forms.RadioButton();
            this.rbFilePath = new System.Windows.Forms.RadioButton();
            this.chkEntity = new System.Windows.Forms.CheckBox();
            this.chkModel = new System.Windows.Forms.CheckBox();
            this.chkDAL = new System.Windows.Forms.CheckBox();
            this.chkService = new System.Windows.Forms.CheckBox();
            this.chkWeb = new System.Windows.Forms.CheckBox();
            this.cboxVersion = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboxDatabase = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lboxMessage = new System.Windows.Forms.ListBox();
            this.chklboxTable = new System.Windows.Forms.CheckedListBox();
            this.btnBuilder = new System.Windows.Forms.Button();
            this.btnSetPath = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.lblSetPath = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(927, 25);
            this.menuStrip1.TabIndex = 36;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接ToolStripMenuItem});
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.新建ToolStripMenuItem.Text = "新建";
            // 
            // 连接ToolStripMenuItem
            // 
            this.连接ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mySQLToolStripMenuItem,
            this.sQLServerToolStripMenuItem});
            this.连接ToolStripMenuItem.Name = "连接ToolStripMenuItem";
            this.连接ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.连接ToolStripMenuItem.Text = "新建连接";
            // 
            // mySQLToolStripMenuItem
            // 
            this.mySQLToolStripMenuItem.Name = "mySQLToolStripMenuItem";
            this.mySQLToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.mySQLToolStripMenuItem.Text = "MySQL";
            this.mySQLToolStripMenuItem.Click += new System.EventHandler(this.mySQLToolStripMenuItem_Click);
            // 
            // sQLServerToolStripMenuItem
            // 
            this.sQLServerToolStripMenuItem.Name = "sQLServerToolStripMenuItem";
            this.sQLServerToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.sQLServerToolStripMenuItem.Text = "SQL Server";
            // 
            // txtTableSearch
            // 
            this.txtTableSearch.Location = new System.Drawing.Point(90, 91);
            this.txtTableSearch.Name = "txtTableSearch";
            this.txtTableSearch.Size = new System.Drawing.Size(313, 21);
            this.txtTableSearch.TabIndex = 83;
            this.txtTableSearch.TextChanged += new System.EventHandler(this.txtTableSearch_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbProject);
            this.panel1.Controls.Add(this.rbFilePath);
            this.panel1.Location = new System.Drawing.Point(693, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(176, 32);
            this.panel1.TabIndex = 82;
            // 
            // rbProject
            // 
            this.rbProject.AutoSize = true;
            this.rbProject.Location = new System.Drawing.Point(19, 8);
            this.rbProject.Name = "rbProject";
            this.rbProject.Size = new System.Drawing.Size(71, 16);
            this.rbProject.TabIndex = 0;
            this.rbProject.TabStop = true;
            this.rbProject.Text = "当前项目";
            this.rbProject.UseVisualStyleBackColor = true;
            // 
            // rbFilePath
            // 
            this.rbFilePath.AutoSize = true;
            this.rbFilePath.Location = new System.Drawing.Point(96, 8);
            this.rbFilePath.Name = "rbFilePath";
            this.rbFilePath.Size = new System.Drawing.Size(71, 16);
            this.rbFilePath.TabIndex = 0;
            this.rbFilePath.TabStop = true;
            this.rbFilePath.Text = "指定路径";
            this.rbFilePath.UseVisualStyleBackColor = true;
            // 
            // chkEntity
            // 
            this.chkEntity.AutoSize = true;
            this.chkEntity.Location = new System.Drawing.Point(434, 134);
            this.chkEntity.Name = "chkEntity";
            this.chkEntity.Size = new System.Drawing.Size(72, 16);
            this.chkEntity.TabIndex = 79;
            this.chkEntity.Text = "entity层";
            this.chkEntity.UseVisualStyleBackColor = true;
            // 
            // chkModel
            // 
            this.chkModel.AutoSize = true;
            this.chkModel.Location = new System.Drawing.Point(337, 134);
            this.chkModel.Name = "chkModel";
            this.chkModel.Size = new System.Drawing.Size(66, 16);
            this.chkModel.TabIndex = 78;
            this.chkModel.Text = "model层";
            this.chkModel.UseVisualStyleBackColor = true;
            // 
            // chkDAL
            // 
            this.chkDAL.AutoSize = true;
            this.chkDAL.Location = new System.Drawing.Point(268, 135);
            this.chkDAL.Name = "chkDAL";
            this.chkDAL.Size = new System.Drawing.Size(54, 16);
            this.chkDAL.TabIndex = 77;
            this.chkDAL.Text = "DAL层";
            this.chkDAL.UseVisualStyleBackColor = true;
            // 
            // chkService
            // 
            this.chkService.AutoSize = true;
            this.chkService.Location = new System.Drawing.Point(168, 134);
            this.chkService.Name = "chkService";
            this.chkService.Size = new System.Drawing.Size(78, 16);
            this.chkService.TabIndex = 76;
            this.chkService.Text = "Service层";
            this.chkService.UseVisualStyleBackColor = true;
            // 
            // chkWeb
            // 
            this.chkWeb.AutoSize = true;
            this.chkWeb.Location = new System.Drawing.Point(90, 135);
            this.chkWeb.Name = "chkWeb";
            this.chkWeb.Size = new System.Drawing.Size(54, 16);
            this.chkWeb.TabIndex = 75;
            this.chkWeb.Text = "Web层";
            this.chkWeb.UseVisualStyleBackColor = true;
            // 
            // cboxVersion
            // 
            this.cboxVersion.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxVersion.FormattingEnabled = true;
            this.cboxVersion.Items.AddRange(new object[] {
            "V1"});
            this.cboxVersion.Location = new System.Drawing.Point(500, 40);
            this.cboxVersion.Name = "cboxVersion";
            this.cboxVersion.Size = new System.Drawing.Size(71, 27);
            this.cboxVersion.TabIndex = 74;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 72;
            this.label5.Text = "搜索表名：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(453, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 73;
            this.label4.Text = "版本：";
            // 
            // cboxDatabase
            // 
            this.cboxDatabase.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxDatabase.FormattingEnabled = true;
            this.cboxDatabase.Location = new System.Drawing.Point(90, 41);
            this.cboxDatabase.Name = "cboxDatabase";
            this.cboxDatabase.Size = new System.Drawing.Size(313, 27);
            this.cboxDatabase.TabIndex = 70;
            this.cboxDatabase.SelectedIndexChanged += new System.EventHandler(this.cboxDatabase_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(609, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 68;
            this.label3.Text = "储存方式：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 69;
            this.label1.Text = "数据库：";
            // 
            // lboxMessage
            // 
            this.lboxMessage.FormattingEnabled = true;
            this.lboxMessage.ItemHeight = 12;
            this.lboxMessage.Location = new System.Drawing.Point(54, 171);
            this.lboxMessage.Name = "lboxMessage";
            this.lboxMessage.Size = new System.Drawing.Size(828, 448);
            this.lboxMessage.TabIndex = 86;
            this.lboxMessage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lboxMessage_MouseDoubleClick);
            // 
            // chklboxTable
            // 
            this.chklboxTable.FormattingEnabled = true;
            this.chklboxTable.Location = new System.Drawing.Point(35, 157);
            this.chklboxTable.Name = "chklboxTable";
            this.chklboxTable.Size = new System.Drawing.Size(834, 452);
            this.chklboxTable.TabIndex = 85;
            this.chklboxTable.SelectedIndexChanged += new System.EventHandler(this.chklboxTable_SelectedIndexChanged);
            // 
            // btnBuilder
            // 
            this.btnBuilder.Location = new System.Drawing.Point(309, 639);
            this.btnBuilder.Name = "btnBuilder";
            this.btnBuilder.Size = new System.Drawing.Size(273, 37);
            this.btnBuilder.TabIndex = 84;
            this.btnBuilder.Text = "生成";
            this.btnBuilder.UseVisualStyleBackColor = true;
            this.btnBuilder.Click += new System.EventHandler(this.btnBuilder_Click);
            // 
            // btnSetPath
            // 
            this.btnSetPath.Location = new System.Drawing.Point(807, 91);
            this.btnSetPath.Name = "btnSetPath";
            this.btnSetPath.Size = new System.Drawing.Size(37, 24);
            this.btnSetPath.TabIndex = 89;
            this.btnSetPath.Text = "...";
            this.btnSetPath.UseVisualStyleBackColor = true;
            this.btnSetPath.Click += new System.EventHandler(this.btnSetPath_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(511, 91);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(299, 21);
            this.txtPath.TabIndex = 88;
            // 
            // lblSetPath
            // 
            this.lblSetPath.AutoSize = true;
            this.lblSetPath.Location = new System.Drawing.Point(440, 97);
            this.lblSetPath.Name = "lblSetPath";
            this.lblSetPath.Size = new System.Drawing.Size(65, 12);
            this.lblSetPath.TabIndex = 87;
            this.lblSetPath.Text = "保存路径：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 688);
            this.Controls.Add(this.btnSetPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblSetPath);
            this.Controls.Add(this.lboxMessage);
            this.Controls.Add(this.chklboxTable);
            this.Controls.Add(this.btnBuilder);
            this.Controls.Add(this.txtTableSearch);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkEntity);
            this.Controls.Add(this.chkModel);
            this.Controls.Add(this.chkDAL);
            this.Controls.Add(this.chkService);
            this.Controls.Add(this.chkWeb);
            this.Controls.Add(this.cboxVersion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboxDatabase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "代码生成器";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mySQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQLServerToolStripMenuItem;
        private System.Windows.Forms.TextBox txtTableSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbProject;
        private System.Windows.Forms.RadioButton rbFilePath;
        private System.Windows.Forms.CheckBox chkEntity;
        private System.Windows.Forms.CheckBox chkModel;
        private System.Windows.Forms.CheckBox chkDAL;
        private System.Windows.Forms.CheckBox chkService;
        private System.Windows.Forms.CheckBox chkWeb;
        private System.Windows.Forms.ComboBox cboxVersion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboxDatabase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lboxMessage;
        private System.Windows.Forms.CheckedListBox chklboxTable;
        private System.Windows.Forms.Button btnBuilder;
        private System.Windows.Forms.Button btnSetPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label lblSetPath;
    }
}

