using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FolderClear
{
    public partial class FrmSettingList : Form
    {
        #region Member
        private FolderBrowserDialog _FolderBrowserDialog = null;
        private FrmSettingEdit _SettingEditDialog = null;
        private bool _IsDataChanged = false;
        private List<Setting> _Settings = new List<Setting>();
        #endregion

        #region Private Method
        /// <summary>
        /// 初始化
        /// </summary>
        private void Initial()
        {
            #region dgvSettingList 初始化
            #region 基本屬性設定
            this.dgvSettingList.AutoGenerateColumns = false;
            //this.dgvSettingList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvSettingList.AllowUserToAddRows = false;
            this.dgvSettingList.AllowUserToDeleteRows = false;
            this.dgvSettingList.AllowUserToOrderColumns = false;
            this.dgvSettingList.AllowUserToResizeColumns = true;
            this.dgvSettingList.AllowUserToResizeRows = true;
            this.dgvSettingList.ScrollBars = ScrollBars.Both;
            //this.dgvSettingList.DefaultCellStyle.NullValue = "NULL";
            this.dgvSettingList.ReadOnly = true;
            //this.dgvSettingList.AutoSize = true;
            this.dgvSettingList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //this.dgvSettingList.SortCompare += new DataGridViewSortCompareEventHandler(this.FileSizeSortCompare);
            //this.dgvSettingList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSetting_CellFormatting);
            #endregion

            #region 欄位屬性設定
            this.dgvSettingList.Columns.Clear();

            #region 設定資料 (Setting)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Name = "SettingIndex";
                column.HeaderText = "設定資料";
                column.Visible = false;
                column.ReadOnly = true;
                this.dgvSettingList.Columns.Add(column);
            }
            #endregion

            #region 資料夾路徑 (FolderPath)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                this.dgvSettingList.Columns.Add(column);
                column.Name = "FolderPath";
                column.HeaderText = "資料夾路徑";
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
                //column.DataPropertyName = "FolderPath";
                column.Visible = true;
                column.ReadOnly = true;
                column.Frozen = true;
            }
            #endregion

            #region 檔名過濾 (FileFilter)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                this.dgvSettingList.Columns.Add(column);
                column.Name = "FileFilter";
                column.HeaderText = "檔名過濾條件";
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
                //column.DataPropertyName = "FileFilter";
                column.Visible = true;
                column.ReadOnly = true;
            }
            #endregion

            #region 子資料夾過濾 (SubFolderFilter)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                this.dgvSettingList.Columns.Add(column);
                column.Name = "SubFolderFilter";
                column.HeaderText = "子資料夾過濾條件";
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
                //column.DataPropertyName = "SubFolderFilter";
                column.Visible = true;
                column.ReadOnly = true;
            }
            #endregion

            #region 子資料夾下檔名過濾 (SubFileFilter)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                this.dgvSettingList.Columns.Add(column);
                column.Name = "SubFileFilter";
                column.HeaderText = "子資料夾下檔名過濾條件";
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
                //column.DataPropertyName = "SubFolderFilter";
                column.Visible = true;
                column.ReadOnly = true;
            }
            #endregion

            #region 保留檔案天數 (KeepDays)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                this.dgvSettingList.Columns.Add(column);
                column.Name = "KeepDays";
                column.HeaderText = "保留檔案天數";
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
                //column.DataPropertyName = "KeepDays";
                column.Visible = true;
                column.ReadOnly = true;
            }
            #endregion

            #region 前面先將 Column 的 Header 設為不換行，然後執行 AutoResizeColumns() 這樣，這樣 Header 就不會縮短
            this.dgvSettingList.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            foreach (DataGridViewColumn column in this.dgvSettingList.Columns)
            {
                if (column.Visible)
                {
                    column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                }
            }
            #endregion
            #endregion
            #endregion
        }

        /// <summary>
        /// 結繫資料
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="settingDatas"></param>
        private void BindData(string logPath, List<Setting> settingDatas)
        {
            #region 日誌檔路徑
            this.tbxLogPath.Text = logPath;
            #endregion

            #region 清除暫存檔設定
            this.dgvSettingList.Rows.Clear();
            if (settingDatas != null && settingDatas.Count > 0)
            {
                this.dgvSettingList.SuspendLayout();

                #region [MDY:20220905] Checkmarx - Stored XSS 誤判調整
                int settingIndex = 0;
                foreach (Setting data in settingDatas)
                {
                    this.BindAppendData(settingIndex, data);
                    settingIndex++;
                }
                #endregion

                this.dgvSettingList.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                this.dgvSettingList.ResumeLayout();
                this.dgvSettingList.Refresh();
            }
            #endregion

            #region [MDY:20220905] Checkmarx - Stored XSS 誤判調整
            _Settings.Clear();
            if (settingDatas != null && settingDatas.Count > 0)
            {
                _Settings.AddRange(settingDatas);
            }
            #endregion

            _IsDataChanged = false;
        }

        #region [MDY:20220905] Checkmarx - Stored XSS 誤判調整
        /// <summary>
        /// Append 一筆資料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="suspendFlag"></param>
        private void BindAppendData(int settingIndex, Setting data, bool suspendFlag = false)
        {
            //if (suspendFlag)
            //{
            //    this.dgvSettingList.SuspendLayout();
            //}

            int index = this.dgvSettingList.Rows.Add();
            DataGridViewRow row = this.dgvSettingList.Rows[index];


            this.BindRowData(row, settingIndex, data, suspendFlag);

            //if (suspendFlag)
            //{
            //    this.dgvSettingList.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //    this.dgvSettingList.ResumeLayout();
            //    this.dgvSettingList.Refresh();
            //}
        }

        private void BindRowData(DataGridViewRow row, int settingIndex, Setting data, bool suspendFlag = false)
        {
            if (suspendFlag)
            {
                this.dgvSettingList.SuspendLayout();
            }

            row.Cells["SettingIndex"].Value = settingIndex;
            row.Cells["FolderPath"].Value = System.Web.HttpUtility.HtmlEncode(data.FolderPath);
            row.Cells["FileFilter"].Value = System.Web.HttpUtility.HtmlEncode(data.FileFilter);
            row.Cells["SubFolderFilter"].Value = System.Web.HttpUtility.HtmlEncode(data.SubFolderFilter);
            row.Cells["SubFileFilter"].Value = System.Web.HttpUtility.HtmlEncode(data.SubFileFilter);

            row.Cells["KeepDays"].Value = data.KeepDays;

            if (suspendFlag)
            {
                this.dgvSettingList.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                this.dgvSettingList.ResumeLayout();
                this.dgvSettingList.Refresh();
            }
        }
        #endregion

        /// <summary>
        /// 取得 FolderBrowserDialog 物件
        /// </summary>
        /// <param name="description"></param>
        /// <param name="selectedPath"></param>
        /// <returns></returns>
        private FolderBrowserDialog GetFolderBrowserDialog(string description, string selectedPath)
        {
            if (_FolderBrowserDialog == null)
            {
                _FolderBrowserDialog = new FolderBrowserDialog();
            }
            _FolderBrowserDialog.Description = description;
            _FolderBrowserDialog.SelectedPath = selectedPath;
            return _FolderBrowserDialog;
        }

        /// <summary>
        /// 取得 FrmSettingEdit 物件
        /// </summary>
        /// <returns></returns>
        private FrmSettingEdit GetSettingEditDialog()
        {
            if (_SettingEditDialog == null)
            {
                _SettingEditDialog = new FrmSettingEdit();
                _SettingEditDialog.SettingData = null;
            }
            return _SettingEditDialog;
        }

        /// <summary>
        /// 檢查 日誌檔路徑 是否合法
        /// </summary>
        /// <param name="showEmptyMemo"></param>
        /// <returns></returns>
        private bool CheckLogPath(bool showEmptyMemo = true)
        {
            string logPath = this.tbxLogPath.Text.Trim();

            if (String.IsNullOrEmpty(logPath))
            {
                if (showEmptyMemo)
                {
                    MessageBox.Show("未指定日誌檔路徑，『暫存檔清除工具』將不產生日誌檔", "日誌檔路徑", MessageBoxButtons.OK);
                }
                return true;
            }

            #region 檢查路徑
            try
            {
                if (!Path.IsPathRooted(logPath))
                {
                    MessageBox.Show("日誌檔路徑不合法，必須為絕對路徑", "日誌檔路徑");
                    return false;
                }

                #region [MARK] 不用檢查，因為清檔程式執行時會自動建日誌檔路徑
                //if (!Directory.Exists(logPath))
                //{
                //    MessageBox.Show("日誌檔路徑不存在", "日誌檔路徑");
                //    return false;
                //}
                #endregion
            }
            catch(Exception ex)
            {
                MessageBox.Show("日誌檔路徑不合法，" + ex.Message, "日誌檔路徑");
                return false;
            }
            return true;
            #endregion
        }
        #endregion

        #region Form Event Hander
        private void FrmSettingList_Load(object sender, EventArgs e)
        {
            this.Initial();
        }

        private void FrmSettingList_Shown(object sender, EventArgs e)
        {
            string errmsg = null;
            string logPath = null;
            List<Setting> settingDatas = Setting.LoadBySettingFile(out logPath, out errmsg);
            if (String.IsNullOrEmpty(errmsg))
            {
                this.BindData(logPath, settingDatas);
            }
            else
            {
                MessageBox.Show("讀取設定檔失敗，" + errmsg, "讀取設定檔");
                return;
            }
        }

        private void FrmSettingList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_IsDataChanged)
            {
                DialogResult result = MessageBox.Show("資料已修改但未儲存，是否結束設定不儲存", "結束確認", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void FrmSettingList_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_FolderBrowserDialog != null)
            {
                _FolderBrowserDialog.Dispose();
                _FolderBrowserDialog = null;
            }
        }
        #endregion

        #region Constructor
        public FrmSettingList()
        {
            InitializeComponent();
        }
        #endregion

        #region 日誌檔路徑 相關事件
        private void tbxLogPath_TextChanged(object sender, EventArgs e)
        {
            _IsDataChanged = true;
        }

        private void btnLogPathBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = this.GetFolderBrowserDialog("請選擇日誌檔路徑", this.tbxLogPath.Text.Trim());
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _IsDataChanged = true;
                this.tbxLogPath.Text = dialog.SelectedPath;
                this.CheckLogPath();
            }
        }
        #endregion

        #region 清除暫存檔設定 相關事件
        private void btnAppend_Click(object sender, EventArgs e)
        {
            FrmSettingEdit dialog = this.GetSettingEditDialog();
            dialog.SettingData = null;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _IsDataChanged = true;
                Setting data = dialog.SettingData;

                #region [MDY:20220905] Checkmarx - Stored XSS 誤判調整
                int settingIndex = _Settings.Count;
                _Settings.Add(data);
                #endregion

                this.BindAppendData(settingIndex, data, true);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = this.dgvSettingList.SelectedRows;
            if (rows.Count == 0)
            {
                MessageBox.Show("修改時請先選擇一筆資料");
                return;
            }
            if (rows.Count > 1)
            {
                MessageBox.Show("修改時只能選擇一筆資料");
                return;
            }

            DataGridViewRow row = rows[0];
            FrmSettingEdit dialog = this.GetSettingEditDialog();

            #region [MDY:20220905] Checkmarx - Stored XSS 誤判調整
            int settingIndex = (int)row.Cells["SettingIndex"].Value;
            dialog.SettingData = settingIndex < _Settings.Count ? _Settings[settingIndex] : null;
            #endregion

            if (dialog.SettingData != null)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _IsDataChanged = true;
                    Setting data = dialog.SettingData;
                    this.BindRowData(row, settingIndex, data, true);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = this.dgvSettingList.SelectedRows;
            if (rows.Count == 0)
            {
                MessageBox.Show("刪除時至少須選擇一筆資料");
                return;
            }

            this.dgvSettingList.SuspendLayout();
            foreach (DataGridViewRow row in rows)
            {
                this.dgvSettingList.Rows.Remove(row);
            }
            this.dgvSettingList.ResumeLayout();
            this.dgvSettingList.Refresh();
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.CheckLogPath(false))
            {
                string errmsg = null;
                string logPath = this.tbxLogPath.Text.Trim();
                List<Setting> datas = null;
                if (this.dgvSettingList.Rows.Count > 0)
                {
                    datas = new List<Setting>(this.dgvSettingList.Rows.Count);
                    foreach (DataGridViewRow row in this.dgvSettingList.Rows)
                    {
                        #region [MDY:20220905] Checkmarx - Stored XSS 誤判調整
                        int settingIndex = (int)row.Cells["SettingIndex"].Value;
                        Setting data = settingIndex < _Settings.Count ? _Settings[settingIndex] : null;
                        #endregion

                        if (data != null)
                        {
                            datas.Add(data);
                        }
                    }
                }

                errmsg = Setting.WriteToSettingFile(logPath, datas);
                if (String.IsNullOrEmpty(errmsg))
                {
                    _IsDataChanged = false;
                    MessageBox.Show("儲存資料成功");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("儲存資料失敗，" + errmsg, "儲存資料");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
