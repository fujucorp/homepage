using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.B
{
    /// <summary>
    /// 匯入委扣回復資料
    /// </summary>
    public partial class B2100007 : BasePage
    {
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxResult.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    this.lbtnUpload.Visible = false;
                    return;
                }
                #endregion
            }
        }

        protected void lbtnUpload_Click(object sender, EventArgs e)
        {
            #region 5 Key
            string receiveType = this.ucFilter1.SelectedReceiveType;
            string yearId = this.ucFilter1.SelectedYearID;
            string termId = this.ucFilter1.SelectedTermID;
            string depId = "";
            string receiveId = this.ucFilter2.SelectedReceiveID;

            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }
            if (String.IsNullOrEmpty(yearId))
            {
                this.ShowMustInputAlert("學年");
                return;
            }
            if (String.IsNullOrEmpty(termId))
            {
                this.ShowMustInputAlert("學期");
                return;
            }
            if (String.IsNullOrEmpty(receiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return;
            }
            #endregion

            #region 上傳委扣回復媒體檔案
            if (!this.fileUpload.HasFile || this.fileUpload.FileBytes == null || this.fileUpload.FileBytes.Length == 0)
            {
                this.ShowMustInputAlert("上傳委扣回復媒體檔案");
                return;
            }
            #endregion

            #region [MDY:20201227] 檢查副檔名限制 .TXT 或無副檔名
            {
                string extName = Path.GetExtension(this.fileUpload.FileName).ToLower();
                if (!String.IsNullOrEmpty(extName) && extName != ".txt")
                {
                    string msg = this.GetLocalized("僅支援 文字檔 的 txt 或 無副檔名");
                    this.ShowJsAlert(msg);
                    return;
                }
            }
            #endregion

            #region 取 SchoolRTypeEntity
            string deductId = null;
            {
                SchoolRTypeEntity data = null;
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this.Page, where, null, out data);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowSystemMessage(String.Format("讀取學校資料失敗，錯誤訊息：{0}", receiveType, xmlResult.Message));
                    return;
                }
                if (data == null)
                {
                    this.ShowSystemMessage("查無該學校資料");
                    return;
                }
                deductId = data.DeductId;
                if (String.IsNullOrEmpty(deductId))
                {
                    this.ShowSystemMessage("該學校無客戶委託代號資料");
                    return;
                }
            }
            #endregion

            #region 初步檢查檔案內容
            StringBuilder content = new StringBuilder();
            using (StreamReader reader = new StreamReader(this.fileUpload.FileContent))
            {
                int lineNo = 0;

                #region [MDY:20220410] Checkmarx 調整
                try
                {
                    #region 首錄
                    {
                        string line = reader.ReadLine();
                        lineNo = 1;

                        #region 長度
                        if (line == null || line.Length != 200)
                        {
                            this.ShowSystemMessage(String.Format("第{0}行資料長度不正確", lineNo));
                            return;
                        }
                        #endregion

                        #region 錄別 (首筆為1)
                        if (line.Substring(0, 1) != "1")
                        {
                            this.ShowSystemMessage(String.Format("第{0}行資料的錄別不正確", lineNo));
                            return;
                        }
                        #endregion

                        #region 客戶委託編號
                        if (deductId != line.Substring(1, 8).Trim())
                        {
                            this.ShowSystemMessage(String.Format("第{0}行資料的客戶委託編號不正確", lineNo));
                            return;
                        }
                        #endregion

                        #region 資料性質別 (2:轉帳結果(處理後退回))
                        if (line.Substring(30, 1) != "2")
                        {
                            this.ShowSystemMessage(String.Format("第{0}行資料的資料性質別不正確", lineNo));
                            return;
                        }
                        #endregion

                        content.AppendLine(line);
                    }
                    #endregion

                    #region 明細 + 尾錄
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            lineNo++;

                            #region 長度
                            if (line.Length != 200)
                            {
                                this.ShowSystemMessage(String.Format("第{0}行資料長度不正確", lineNo));
                                return;
                            }
                            #endregion

                            #region 錄別 (明細每筆為2，尾錄為3)
                            string lineType = line.Substring(0, 1);
                            if (lineType == "2")
                            {

                            }
                            else if (lineType == "3")
                            {
                            }
                            else
                            {
                                this.ShowSystemMessage(String.Format("第{0}行資料的錄別不正確", lineNo));
                                return;
                            }
                            #endregion

                            #region 客戶委託編號
                            if (deductId != line.Substring(1, 8).Trim())
                            {
                                this.ShowSystemMessage(String.Format("第{0}行資料的客戶委託編號不正確", lineNo));
                                return;
                            }
                            #endregion

                            #region 銷帳編號
                            //if (line.Substring(30, 1) != "2")
                            //{
                            //    this.ShowSystemMessage(String.Format("第{0}行資料的客戶委託編號不正確", lineNo));
                            //    return;
                            //}
                            #endregion

                            content.AppendLine(line);
                            line = reader.ReadLine();
                        }
                    }
                    #endregion
                }
                catch (Exception)
                {
                    this.ShowJsAlert("初步檢查檔案內容失敗。");
                    return;
                }
                #endregion
            }
            #endregion

            #region
            {
                string action = this.GetLocalized("匯入委扣回復資料");
                string resultText = null;
                XmlResult xmlResult = DataProxy.Current.ImportB2100007File(this.Page, receiveType, yearId, termId, depId, receiveId, content.ToString(), out resultText);
                if (xmlResult.IsSuccess)
                {
                    this.tbxResult.Text = resultText;
                    this.tbxResult.Visible = true;
                }
                else
                {
                    this.tbxResult.Text = String.Empty;
                    this.tbxResult.Visible = false;
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion
        }
    }
}