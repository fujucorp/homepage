using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderClear
{
    public partial class FrmSettingEdit : Form
    {
        #region Constructor
        public FrmSettingEdit()
        {
            InitializeComponent();
        }
        #endregion

        #region Member
        private FolderBrowserDialog _FolderBrowserDialog = null;
        #endregion

        #region Property
        /// <summary>
        /// 編輯的設定資料
        /// </summary>
        public Setting SettingData
        {
            get;
            set;
        }
        #endregion

        #region Method
        private void BindData(Setting data)
        {
            this.tbxFolderPath.Text = data.FolderPath;
            this.tbxFileFilter.Text = data.FileFilter;

            Regex subFolderRegex = data.SubFolderRegex;
            if (subFolderRegex != null)
            {
                this.cbxIsRegex.Checked = true;
                this.tbxSubFolderFilter.Text = subFolderRegex.ToString();
            }
            else
            {
                this.cbxIsRegex.Checked = false;
                this.tbxSubFolderFilter.Text = data.SubFolderFilter;
            }

            this.tbxSubFileFilter.Text = data.SubFileFilter;
            this.mudKeepDays.Text = data.KeepDays.ToString();
        }

        /// <summary>
        /// 取得 FolderBrowserDialog 物件
        /// </summary>
        /// <param name="description"></param>
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
        /// 檢查 資料夾路徑 是否合法
        /// </summary>
        /// <returns></returns>
        private bool CheckFolderPath()
        {
            string folderPath = this.tbxFolderPath.Text.Trim();

            if (String.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("未指定資料夾路徑", "資料夾路徑");
                return false;
            }

            #region 檢查路徑
            try
            {
                if (!Path.IsPathRooted(folderPath))
                {
                    MessageBox.Show("資料夾路徑不合法，必須為絕對路徑", "資料夾路徑");
                    return false;
                }

                //DirectoryInfo dinfo = new DirectoryInfo(folderPath);
                //if (dinfo.FullName != folderPath)
                //{
                //    MessageBox.Show("資料夾路徑不合法，必須為絕對路徑", "資料夾路徑");
                //    return false;
                //}

                #region [MDY:20210401] 原碼修正
                {
                    bool isOK = false;
                    string[] rootList = new string[] { @"D:\WEB\DATA\", @"D:\WEB\LOG\", @"D:\WEB\TEMP\", @"D:\AP\DATA\", @"D:\AP\LOG\", @"D:\AP\TEMP\" };
                    if (!folderPath.EndsWith(@"\"))
                    {
                        folderPath += @"\";
                    }
                    foreach (string root in rootList)
                    {
                        if (folderPath.StartsWith(root))
                        {
                            isOK = true;
                            break;
                        }
                    }
                    if (!isOK)
                    {
                        MessageBox.Show("資料夾根目錄不合法", "資料夾路徑");
                        return false;
                    }
                }
                #endregion

                #region [MARK] 暫時不檢查
                //if (!Directory.Exists(folderPath))
                //{
                //    MessageBox.Show("資料夾路徑不存在", "資料夾路徑");
                //    return false;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("資料夾路徑不合法，" + ex.Message, "資料夾路徑");
                return false;
            }
            return true;
            #endregion
        }
        #endregion

        #region Form Event Hander
        private void FrmSettingEdit_Load(object sender, EventArgs e)
        {
            if (this.SettingData == null)
            {
                Setting data = new Setting();
                data.FileFilter = "*.*";
                data.KeepDays = 30;
                this.SettingData = data;
            }

            this.BindData(this.SettingData);
        }

        private void FrmSettingEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_FolderBrowserDialog != null)
            {
                _FolderBrowserDialog.Dispose();
                _FolderBrowserDialog = null;
            }
        }
        #endregion

        #region 資料夾路徑 相關事件
        private void btnFolderPathBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = this.GetFolderBrowserDialog("請選擇資料夾路徑", this.tbxFolderPath.Text.Trim());
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbxFolderPath.Text = dialog.SelectedPath;
                this.CheckFolderPath();
            }
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.CheckFolderPath())
            {
                Setting data = this.SettingData;

                #region 資料夾路徑
                data.FolderPath = this.tbxFolderPath.Text;
                #endregion

                #region 檔名過濾條件
                data.FileFilter = this.tbxFileFilter.Text;
                if (String.IsNullOrEmpty(data.FileFilter))
                {
                    MessageBox.Show("請指定檔名過濾條件");
                    return;
                }
                #endregion

                #region 子資料夾過濾條件 + 子資料夾下檔名過濾條件
                data.SubFolderFilter = this.tbxSubFolderFilter.Text;
                data.SubFileFilter = this.tbxSubFileFilter.Text;
                if (String.IsNullOrEmpty(data.SubFolderFilter))
                {
                    if (cbxIsRegex.Checked)
                    {
                        MessageBox.Show("請指定子資料夾過濾條件的正規表示式 Pattern");
                        return;
                    }
                    if (!String.IsNullOrEmpty(data.SubFileFilter))
                    {
                        MessageBox.Show("未指定子資料夾過濾條件時，不可指定子資料夾下檔名過濾條件");
                        return;
                    }
                }
                else
                {
                    if (cbxIsRegex.Checked)
                    {
                        data.SubFolderFilter = String.Format("{0}{1}{2}", Setting.RegexPrefix, data.SubFolderFilter, Setting.RegexSuffix);
                        string errmsg = data.GenSubFolderRegex();
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            MessageBox.Show("子資料夾過濾條件設定值不正確，" + errmsg);
                            return;
                        }
                    }
                }
                #endregion

                #region 保留檔案天數
                if (this.mudKeepDays.Value > Setting.MaxKeepDays || this.mudKeepDays.Value < Setting.MinKeepDays)
                {
                    MessageBox.Show(String.Format("保留檔案天數必須為 {0} ~ {1} 的整數值", Setting.MinKeepDays, Setting.MaxKeepDays));
                    return;
                }
                data.KeepDays = Convert.ToInt32(this.mudKeepDays.Value);
                #endregion

                if (!data.IsReady())
                {
                    MessageBox.Show("資料未準備好");
                    return;
                }

                this.SettingData = data;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
