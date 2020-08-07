namespace MovitCodeTools
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
            this.lboxMessage = new System.Windows.Forms.ListBox();
            this.chklboxTable = new System.Windows.Forms.CheckedListBox();
            this.btnBuilder = new System.Windows.Forms.Button();
            this.txtTableSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboxDatabase = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mySQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lboxMessage
            // 
            this.lboxMessage.FormattingEnabled = true;
            this.lboxMessage.ItemHeight = 18;
            this.lboxMessage.Location = new System.Drawing.Point(42, 118);
            this.lboxMessage.Margin = new System.Windows.Forms.Padding(4);
            this.lboxMessage.Name = "lboxMessage";
            this.lboxMessage.Size = new System.Drawing.Size(1013, 418);
            this.lboxMessage.TabIndex = 106;
            this.lboxMessage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lboxMessage_MouseDoubleClick);
            // 
            // chklboxTable
            // 
            this.chklboxTable.FormattingEnabled = true;
            this.chklboxTable.Location = new System.Drawing.Point(33, 118);
            this.chklboxTable.Margin = new System.Windows.Forms.Padding(4);
            this.chklboxTable.Name = "chklboxTable";
            this.chklboxTable.Size = new System.Drawing.Size(1053, 429);
            this.chklboxTable.TabIndex = 105;
            this.chklboxTable.SelectedIndexChanged += new System.EventHandler(this.chklboxTable_SelectedIndexChanged);
            // 
            // btnBuilder
            // 
            this.btnBuilder.Location = new System.Drawing.Point(347, 592);
            this.btnBuilder.Margin = new System.Windows.Forms.Padding(4);
            this.btnBuilder.Name = "btnBuilder";
            this.btnBuilder.Size = new System.Drawing.Size(410, 56);
            this.btnBuilder.TabIndex = 104;
            this.btnBuilder.Text = "生成";
            this.btnBuilder.UseVisualStyleBackColor = true;
            this.btnBuilder.Click += new System.EventHandler(this.btnBuilder_Click);
            // 
            // txtTableSearch
            // 
            this.txtTableSearch.Location = new System.Drawing.Point(766, 58);
            this.txtTableSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtTableSearch.Name = "txtTableSearch";
            this.txtTableSearch.Size = new System.Drawing.Size(468, 28);
            this.txtTableSearch.TabIndex = 103;
            this.txtTableSearch.TextChanged += new System.EventHandler(this.txtTableSearch_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(659, 63);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 18);
            this.label5.TabIndex = 94;
            this.label5.Text = "搜索表名：";
            // 
            // cboxDatabase
            // 
            this.cboxDatabase.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxDatabase.FormattingEnabled = true;
            this.cboxDatabase.Location = new System.Drawing.Point(128, 50);
            this.cboxDatabase.Margin = new System.Windows.Forms.Padding(4);
            this.cboxDatabase.Name = "cboxDatabase";
            this.cboxDatabase.Size = new System.Drawing.Size(468, 37);
            this.cboxDatabase.TabIndex = 93;
            this.cboxDatabase.SelectedIndexChanged += new System.EventHandler(this.cboxDatabase_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 92;
            this.label1.Text = "数据库：";
            // 
            // sQLServerToolStripMenuItem
            // 
            this.sQLServerToolStripMenuItem.Name = "sQLServerToolStripMenuItem";
            this.sQLServerToolStripMenuItem.Size = new System.Drawing.Size(202, 34);
            this.sQLServerToolStripMenuItem.Text = "SQL Server";
            this.sQLServerToolStripMenuItem.Click += new System.EventHandler(this.sQLServerToolStripMenuItem_Click);
            // 
            // mySQLToolStripMenuItem
            // 
            this.mySQLToolStripMenuItem.Name = "mySQLToolStripMenuItem";
            this.mySQLToolStripMenuItem.Size = new System.Drawing.Size(202, 34);
            this.mySQLToolStripMenuItem.Text = "MySQL";
            this.mySQLToolStripMenuItem.Click += new System.EventHandler(this.mySQLToolStripMenuItem_Click);
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
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接ToolStripMenuItem});
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(62, 28);
            this.新建ToolStripMenuItem.Text = "新建";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1122, 32);
            this.menuStrip1.TabIndex = 90;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 695);
            this.Controls.Add(this.lboxMessage);
            this.Controls.Add(this.chklboxTable);
            this.Controls.Add(this.btnBuilder);
            this.Controls.Add(this.txtTableSearch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboxDatabase);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lboxMessage;
        private System.Windows.Forms.CheckedListBox chklboxTable;
        private System.Windows.Forms.Button btnBuilder;
        private System.Windows.Forms.TextBox txtTableSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboxDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem sQLServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mySQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}

