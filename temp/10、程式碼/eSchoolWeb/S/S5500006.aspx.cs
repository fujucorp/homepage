using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 報稅資料下載
    /// </summary>
    public partial class S5500006 : BasePage
    {
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            bool isOK = true;
            LogonUser logonUser = this.GetLogonUser();

            #region 學校統編下拉選項
            if (logonUser.IsBankUser)
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                if (!logonUser.IsBankManager)
                {
                    where.And(SchoolRTypeEntity.Field.BankId, logonUser.UnitId);
                }
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
                string textCombineFormat = null;

                CodeText[] datas = null;
                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("讀取學校資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    isOK = false;
                }
                WebHelper.SetDropDownListItems(this.ddlSchool, DefaultItem.Kind.Select, false, datas, true, false, 0, null);
            }
            else
            {
                this.ddlSchool.Items.Clear();
                this.ddlSchool.Items.Add(new ListItem(logonUser.UnitName, logonUser.UnitId));
            }
            #endregion

            return isOK;
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <param name="schIdenty"></param>
        /// <returns>成功則傳回 true</returns>
        private bool GetDataAndBind(string schIdenty)
        {
            #region 查詢條件
            Expression where = new Expression(BankpmEntity.Field.ReceiveType, schIdenty)
                .And(BankpmEntity.Field.Filedetail, RelationEnum.Like, "SGH%");
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(BankpmEntity.Field.Cancel, OrderByEnum.Desc);
            #endregion

            string[] codeFieldNames = new string[] { BankpmEntity.Field.Cancel };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { BankpmEntity.Field.Filename };
            string textCombineFormat = null;

            CodeText[] datas = null;
            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<BankpmEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("讀取檔案資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            WebHelper.SetDropDownListItems(this.ddlFileName, DefaultItem.Kind.Select, false, datas, false, false, 0, null);

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnOK.Visible = false;
                if (this.InitialUI())
                {
                    ccbtnOK.Visible = this.GetDataAndBind(null);
                }
            }
        }

        protected void ddlSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            string schIdenty = this.ddlSchool.SelectedValue;

            this.GetDataAndBind(schIdenty);
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string cancel = ddlFileName.SelectedValue;
            if (String.IsNullOrEmpty(cancel))
            {
                string msg = this.GetLocalized("請選擇要下載的檔案");
                this.ShowSystemMessage(msg);
                return;
            }

            #region 取上傳檔案
            Byte[] fileContent = null;
            {
                BankpmEntity data = null;
                Expression where = new Expression(BankpmEntity.Field.Cancel, cancel);

                XmlResult xmlResult = DataProxy.Current.SelectFirst<BankpmEntity>(this, where, null, out data);
                if (!xmlResult.IsSuccess)
                {
                    string msg = this.GetLocalized("讀取上傳檔案資料失敗");
                    this.ShowSystemMessage(msg);
                    return;
                }
                if (data == null)
                {
                    string msg = this.GetLocalized("查無上傳檔案資料");
                    this.ShowSystemMessage(msg);
                    return;
                }

                fileContent = data.Tempfile;
                if (fileContent != null && fileContent.Length > 0)
                {
                    string fileName = data.Filename.Replace(" ", "").Replace(",", "_");
                    string contentType = GetContentType("TXT");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = contentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.BinaryWrite(fileContent);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    string msg = this.GetLocalized("該檔案無資料");
                    this.ShowSystemMessage(msg);
                    return;
                }
            }
            #endregion
        }
    }
}