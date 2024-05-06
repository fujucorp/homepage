namespace FolderClear
{
    partial class FrmSettingEdit
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
            this.gbxMemo = new System.Windows.Forms.GroupBox();
            this.labMemo = new System.Windows.Forms.Label();
            this.btnFolderPathBrowse = new System.Windows.Forms.Button();
            this.tbxFolderPath = new System.Windows.Forms.TextBox();
            this.labFolderPath = new System.Windows.Forms.Label();
            this.labFileFilter = new System.Windows.Forms.Label();
            this.labSubFolderFilter = new System.Windows.Forms.Label();
            this.labSubFileFilter = new System.Windows.Forms.Label();
            this.labKeepDays = new System.Windows.Forms.Label();
            this.tbxFileFilter = new System.Windows.Forms.TextBox();
            this.tbxSubFolderFilter = new System.Windows.Forms.TextBox();
            this.tbxSubFileFilter = new System.Windows.Forms.TextBox();
            this.mudKeepDays = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.cbxIsRegex = new System.Windows.Forms.CheckBox();
            this.gbxMemo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mudKeepDays)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxMemo
            // 
            this.gbxMemo.Controls.Add(this.labMemo);
            this.gbxMemo.Location = new System.Drawing.Point(22, 12);
            this.gbxMemo.Name = "gbxMemo";
            this.gbxMemo.Size = new System.Drawing.Size(612, 119);
            this.gbxMemo.TabIndex = 0;
            this.gbxMemo.TabStop = false;
            this.gbxMemo.Text = "說明：";
            // 
            // labMemo
            // 
            this.labMemo.AutoSize = true;
            this.labMemo.Location = new System.Drawing.Point(19, 30);
            this.labMemo.Name = "labMemo";
            this.labMemo.Size = new System.Drawing.Size(562, 75);
            this.labMemo.TabIndex = 1;
            this.labMemo.Text = "【資料夾路徑】為必填參數，不分大小寫，必須是絕對路徑。\r\n【檔名過濾條件】為必填參數，不分大小寫。與 Dir 的 Dos 指令參數相同。\r\n無子資料夾時【子資料夾" +
    "過濾條件】與【子資料夾下檔名過濾條件】必須保留空白。\r\n【子資料夾過濾條件】除一般的過濾條件，亦可指定正規表示式的 Pattern。\r\n【保留檔案天數】為必填參" +
    "數，限設定 1 ~ 3650 的整數值。";
            // 
            // btnFolderPathBrowse
            // 
            this.btnFolderPathBrowse.Location = new System.Drawing.Point(559, 148);
            this.btnFolderPathBrowse.Name = "btnFolderPathBrowse";
            this.btnFolderPathBrowse.Size = new System.Drawing.Size(75, 26);
            this.btnFolderPathBrowse.TabIndex = 4;
            this.btnFolderPathBrowse.Text = "瀏覽..";
            this.btnFolderPathBrowse.UseVisualStyleBackColor = true;
            this.btnFolderPathBrowse.Click += new System.EventHandler(this.btnFolderPathBrowse_Click);
            // 
            // tbxFolderPath
            // 
            this.tbxFolderPath.Location = new System.Drawing.Point(137, 149);
            this.tbxFolderPath.Name = "tbxFolderPath";
            this.tbxFolderPath.Size = new System.Drawing.Size(394, 25);
            this.tbxFolderPath.TabIndex = 3;
            // 
            // labFolderPath
            // 
            this.labFolderPath.AutoSize = true;
            this.labFolderPath.Location = new System.Drawing.Point(34, 154);
            this.labFolderPath.Name = "labFolderPath";
            this.labFolderPath.Size = new System.Drawing.Size(97, 15);
            this.labFolderPath.TabIndex = 2;
            this.labFolderPath.Text = "資料夾路徑：";
            // 
            // labFileFilter
            // 
            this.labFileFilter.AutoSize = true;
            this.labFileFilter.Location = new System.Drawing.Point(19, 193);
            this.labFileFilter.Name = "labFileFilter";
            this.labFileFilter.Size = new System.Drawing.Size(112, 15);
            this.labFileFilter.TabIndex = 5;
            this.labFileFilter.Text = "檔名過濾條件：";
            // 
            // labSubFolderFilter
            // 
            this.labSubFolderFilter.AutoSize = true;
            this.labSubFolderFilter.Location = new System.Drawing.Point(64, 239);
            this.labSubFolderFilter.Name = "labSubFolderFilter";
            this.labSubFolderFilter.Size = new System.Drawing.Size(142, 15);
            this.labSubFolderFilter.TabIndex = 9;
            this.labSubFolderFilter.Text = "子資料夾過濾條件：";
            // 
            // labSubFileFilter
            // 
            this.labSubFileFilter.AutoSize = true;
            this.labSubFileFilter.Location = new System.Drawing.Point(19, 277);
            this.labSubFileFilter.Name = "labSubFileFilter";
            this.labSubFileFilter.Size = new System.Drawing.Size(187, 15);
            this.labSubFileFilter.TabIndex = 12;
            this.labSubFileFilter.Text = "子資料夾下檔名過濾條件：";
            // 
            // labKeepDays
            // 
            this.labKeepDays.AutoSize = true;
            this.labKeepDays.Location = new System.Drawing.Point(341, 193);
            this.labKeepDays.Name = "labKeepDays";
            this.labKeepDays.Size = new System.Drawing.Size(112, 15);
            this.labKeepDays.TabIndex = 7;
            this.labKeepDays.Text = "保留檔案天數：";
            // 
            // tbxFileFilter
            // 
            this.tbxFileFilter.Location = new System.Drawing.Point(137, 189);
            this.tbxFileFilter.Name = "tbxFileFilter";
            this.tbxFileFilter.Size = new System.Drawing.Size(97, 25);
            this.tbxFileFilter.TabIndex = 6;
            this.tbxFileFilter.Text = "*.*";
            // 
            // tbxSubFolderFilter
            // 
            this.tbxSubFolderFilter.Location = new System.Drawing.Point(344, 236);
            this.tbxSubFolderFilter.Name = "tbxSubFolderFilter";
            this.tbxSubFolderFilter.Size = new System.Drawing.Size(290, 25);
            this.tbxSubFolderFilter.TabIndex = 11;
            // 
            // tbxSubFileFilter
            // 
            this.tbxSubFileFilter.Location = new System.Drawing.Point(209, 274);
            this.tbxSubFileFilter.Name = "tbxSubFileFilter";
            this.tbxSubFileFilter.Size = new System.Drawing.Size(92, 25);
            this.tbxSubFileFilter.TabIndex = 13;
            // 
            // mudKeepDays
            // 
            this.mudKeepDays.Location = new System.Drawing.Point(459, 189);
            this.mudKeepDays.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.mudKeepDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mudKeepDays.Name = "mudKeepDays";
            this.mudKeepDays.Size = new System.Drawing.Size(72, 25);
            this.mudKeepDays.TabIndex = 8;
            this.mudKeepDays.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(559, 309);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(456, 309);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 26);
            this.btnOk.TabIndex = 14;
            this.btnOk.Text = "確定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cbxIsRegex
            // 
            this.cbxIsRegex.AutoSize = true;
            this.cbxIsRegex.Location = new System.Drawing.Point(209, 238);
            this.cbxIsRegex.Name = "cbxIsRegex";
            this.cbxIsRegex.Size = new System.Drawing.Size(134, 19);
            this.cbxIsRegex.TabIndex = 10;
            this.cbxIsRegex.Text = "使用正規表示式";
            this.cbxIsRegex.UseVisualStyleBackColor = true;
            // 
            // FrmSettingEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 357);
            this.Controls.Add(this.cbxIsRegex);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.mudKeepDays);
            this.Controls.Add(this.tbxSubFileFilter);
            this.Controls.Add(this.tbxSubFolderFilter);
            this.Controls.Add(this.tbxFileFilter);
            this.Controls.Add(this.labKeepDays);
            this.Controls.Add(this.labSubFileFilter);
            this.Controls.Add(this.labSubFolderFilter);
            this.Controls.Add(this.labFileFilter);
            this.Controls.Add(this.btnFolderPathBrowse);
            this.Controls.Add(this.tbxFolderPath);
            this.Controls.Add(this.labFolderPath);
            this.Controls.Add(this.gbxMemo);
            this.Name = "FrmSettingEdit";
            this.Text = "暫存檔清除參數項設定";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSettingEdit_FormClosed);
            this.Load += new System.EventHandler(this.FrmSettingEdit_Load);
            this.gbxMemo.ResumeLayout(false);
            this.gbxMemo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mudKeepDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxMemo;
        private System.Windows.Forms.Label labMemo;
        private System.Windows.Forms.Button btnFolderPathBrowse;
        private System.Windows.Forms.TextBox tbxFolderPath;
        private System.Windows.Forms.Label labFolderPath;
        private System.Windows.Forms.Label labFileFilter;
        private System.Windows.Forms.Label labSubFolderFilter;
        private System.Windows.Forms.Label labSubFileFilter;
        private System.Windows.Forms.Label labKeepDays;
        private System.Windows.Forms.TextBox tbxFileFilter;
        private System.Windows.Forms.TextBox tbxSubFolderFilter;
        private System.Windows.Forms.TextBox tbxSubFileFilter;
        private System.Windows.Forms.NumericUpDown mudKeepDays;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox cbxIsRegex;
    }
}