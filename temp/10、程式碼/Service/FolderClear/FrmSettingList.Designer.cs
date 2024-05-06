namespace FolderClear
{
    partial class FrmSettingList
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
            this.labLogPath = new System.Windows.Forms.Label();
            this.tbxLogPath = new System.Windows.Forms.TextBox();
            this.btnLogPathBrowse = new System.Windows.Forms.Button();
            this.gbxMemo = new System.Windows.Forms.GroupBox();
            this.labMemo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSettingList = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.gbxMemo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettingList)).BeginInit();
            this.SuspendLayout();
            // 
            // labLogPath
            // 
            this.labLogPath.AutoSize = true;
            this.labLogPath.Location = new System.Drawing.Point(22, 122);
            this.labLogPath.Name = "labLogPath";
            this.labLogPath.Size = new System.Drawing.Size(97, 15);
            this.labLogPath.TabIndex = 2;
            this.labLogPath.Text = "日誌檔路徑：";
            // 
            // tbxLogPath
            // 
            this.tbxLogPath.Location = new System.Drawing.Point(125, 119);
            this.tbxLogPath.Name = "tbxLogPath";
            this.tbxLogPath.Size = new System.Drawing.Size(613, 25);
            this.tbxLogPath.TabIndex = 3;
            this.tbxLogPath.TextChanged += new System.EventHandler(this.tbxLogPath_TextChanged);
            // 
            // btnLogPathBrowse
            // 
            this.btnLogPathBrowse.Location = new System.Drawing.Point(756, 119);
            this.btnLogPathBrowse.Name = "btnLogPathBrowse";
            this.btnLogPathBrowse.Size = new System.Drawing.Size(75, 26);
            this.btnLogPathBrowse.TabIndex = 4;
            this.btnLogPathBrowse.Text = "瀏覽..";
            this.btnLogPathBrowse.UseVisualStyleBackColor = true;
            this.btnLogPathBrowse.Click += new System.EventHandler(this.btnLogPathBrowse_Click);
            // 
            // gbxMemo
            // 
            this.gbxMemo.Controls.Add(this.labMemo);
            this.gbxMemo.Location = new System.Drawing.Point(25, 13);
            this.gbxMemo.Name = "gbxMemo";
            this.gbxMemo.Size = new System.Drawing.Size(806, 85);
            this.gbxMemo.TabIndex = 0;
            this.gbxMemo.TabStop = false;
            this.gbxMemo.Text = "說明：";
            // 
            // labMemo
            // 
            this.labMemo.AutoSize = true;
            this.labMemo.Location = new System.Drawing.Point(11, 26);
            this.labMemo.Name = "labMemo";
            this.labMemo.Size = new System.Drawing.Size(772, 45);
            this.labMemo.TabIndex = 1;
            this.labMemo.Text = "此介面用於設定『暫存檔清除工具』的參數，與執行無關。參數分成【日誌檔路徑】與【清除暫存檔設定】兩部份。\r\n【日誌檔路徑】用於指定『暫存檔清除工具』執行時日誌檔存放" +
    "路徑。\r\n【清除暫存檔設定】用於指定各暫存檔的路徑、過濾條件、保留天數等設定。";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "清除暫存檔設定：";
            // 
            // dgvSettingList
            // 
            this.dgvSettingList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSettingList.Location = new System.Drawing.Point(25, 196);
            this.dgvSettingList.Name = "dgvSettingList";
            this.dgvSettingList.RowTemplate.Height = 27;
            this.dgvSettingList.Size = new System.Drawing.Size(806, 374);
            this.dgvSettingList.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(651, 589);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(756, 589);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 26);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "結束";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Location = new System.Drawing.Point(481, 162);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(75, 26);
            this.btnAppend.TabIndex = 7;
            this.btnAppend.Text = "新增";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(663, 162);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 26);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "刪除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(571, 162);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 26);
            this.btnModify.TabIndex = 8;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // FrmSettingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 624);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAppend);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvSettingList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbxMemo);
            this.Controls.Add(this.btnLogPathBrowse);
            this.Controls.Add(this.tbxLogPath);
            this.Controls.Add(this.labLogPath);
            this.Name = "FrmSettingList";
            this.Text = "暫存檔清除工具參數設定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSettingList_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSettingList_FormClosed);
            this.Load += new System.EventHandler(this.FrmSettingList_Load);
            this.Shown += new System.EventHandler(this.FrmSettingList_Shown);
            this.gbxMemo.ResumeLayout(false);
            this.gbxMemo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettingList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labLogPath;
        private System.Windows.Forms.TextBox tbxLogPath;
        private System.Windows.Forms.Button btnLogPathBrowse;
        private System.Windows.Forms.GroupBox gbxMemo;
        private System.Windows.Forms.Label labMemo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvSettingList;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
    }
}