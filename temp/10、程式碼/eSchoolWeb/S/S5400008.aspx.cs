using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 手續費查詢
    /// </summary>
    public partial class S5400008 : BasePage
    {
        private bool InitialUI()
        {
            LogonUser logonUser = this.GetLogonUser();

            #region [Old] 土銀沒說行員專用，所以 Mark
            //if (!logonUser.IsBankUser)
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    string msg = this.GetLocalized("此功能行員專用");
            //    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE, msg);
            //    return false;
            //}
            #endregion

            this.ddlBank.Items.Clear();
            this.ddlSchIdenty.Items.Clear();
            this.ddlReceiveType.Items.Clear();
            if (logonUser.IsBankManager)
            {
                this.trBank.Visible = true;
                this.trSchIdenty.Visible = true;
                this.GetAndBindBankOption(logonUser, null);
            }
            else if (logonUser.IsBankUser)
            {
                this.trBank.Visible = false;
                this.trSchIdenty.Visible = true;
                string bankId = logonUser.BankId;
                this.GetAndBindSchIdentyOption(this.GetLogonUser(), bankId, null);
            }
            else
            {
                this.trBank.Visible = false;
                this.trSchIdenty.Visible = false;
                string schIdenty = logonUser.UnitId;
                this.GetAndBindReceiveTypeOptions(this.GetLogonUser(), null, schIdenty, null);
            }

            return true;
        }

        private void GetAndBindBankOption(LogonUser logonUser, string selectedValue)
        {
            Expression where = null;
            if (logonUser.IsBankManager)
            {
                where = new Expression();
            }
            else if (logonUser.IsBankUser)
            {
                where = new Expression(BankEntity.Field.BankNo, logonUser.BankId);
            }

            CodeText[] datas = null;
            if (where != null)
            {
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(BankEntity.Field.BankNo, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { BankEntity.Field.BankNo };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { BankEntity.Field.BankSName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<BankEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, "無法取得發卡銀行選項資料");
                }
            }
            WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.Omit, false, datas, true, false, 0, selectedValue);

            string bankId = this.ddlBank.SelectedValue;
            this.GetAndBindSchIdentyOption(logonUser, selectedValue, bankId);
        }

        private void GetAndBindSchIdentyOption(LogonUser logonUser, string bankId, string selectedValue)
        {
            Expression where = null;

            if (logonUser.IsBankManager)
            {
                //銀行管理者可以查指定分行的學校
                if (!String.IsNullOrEmpty(bankId))
                {
                    if (bankId.Length == 7)
                    {
                        bankId = bankId.Substring(0, 6);
                    }
                    else if (bankId.Length == 3)
                    {
                        bankId = DataFormat.MyBankID + bankId;
                    }
                    where = new Expression(SchoolRTypeEntity.Field.BankId, bankId);
                }
                else
                {
                    where = new Expression();
                }
            }
            else if (logonUser.IsBankUser)
            {
                bankId = logonUser.BankId;
                if (bankId.Length == 7)
                {
                    bankId = bankId.Substring(0, 6);
                }
                else if (bankId.Length == 3)
                {
                    bankId = DataFormat.MyBankID + bankId;
                }
                //非銀行管理者只查自己分行的學校
                where = new Expression(SchoolRTypeEntity.Field.BankId, bankId);
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校只能查自己的學校
                where = new Expression(SchoolRTypeEntity.Field.SchIdenty, logonUser.UnitId);
            }

            CodeText[] datas = null;
            if (where != null)
            {
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, "無法取得學校選項資料");
                }
            }

            if (datas != null && datas.Length == 1 && String.IsNullOrEmpty(selectedValue))
            {
                selectedValue = datas[0].Code;
            }
            DefaultItem.Kind kind = String.IsNullOrEmpty(bankId) ? DefaultItem.Kind.Select : DefaultItem.Kind.All;
            WebHelper.SetDropDownListItems(this.ddlSchIdenty, kind, false, datas, true, false, 0, selectedValue);

            string schIdenty = this.ddlSchIdenty.SelectedValue;
            this.GetAndBindReceiveTypeOptions(logonUser, bankId, schIdenty, null);
        }

        private void GetAndBindReceiveTypeOptions(LogonUser logonUser, string bankId, string schIdenty, string selectedValue)
        {
            Expression where = null;

            if (logonUser.IsBankManager)
            {
                //銀行管理者可以查指定分行的學校
                if (!String.IsNullOrEmpty(bankId))
                {
                    if (bankId.Length == 7)
                    {
                        bankId = bankId.Substring(0, 6);
                    }
                    else if (bankId.Length == 3)
                    {
                        bankId = DataFormat.MyBankID + bankId;
                    }
                    where = new Expression(SchoolRTypeEntity.Field.BankId, bankId);
                    if (!String.IsNullOrEmpty(schIdenty))
                    {
                        where.And(SchoolRTypeEntity.Field.SchIdenty, schIdenty);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(schIdenty))
                    {
                        where = new Expression(SchoolRTypeEntity.Field.SchIdenty, schIdenty);
                    }
                    else
                    {
                        where = new Expression();
                    }
                }
            }
            else if (logonUser.IsBankUser)
            {
                bankId = logonUser.BankId;
                if (bankId.Length == 7)
                {
                    bankId = bankId.Substring(0, 6);
                }
                else if (bankId.Length == 3)
                {
                    bankId = DataFormat.MyBankID + bankId;
                }
                //非銀行管理者只查自己分行的學校
                where = new Expression(SchoolRTypeEntity.Field.BankId, bankId);
                if (!String.IsNullOrEmpty(schIdenty))
                {
                    where.And(SchoolRTypeEntity.Field.SchIdenty, schIdenty);
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校只能查自己的學校
                where = new Expression(SchoolRTypeEntity.Field.SchIdenty, logonUser.UnitId);
            }

            CodeText[] datas = null;
            if (where != null)
            {
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.ReceiveType };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, "無法取得學校統一編號選項資料");
                }
            }

            if (datas != null && datas.Length == 1 && String.IsNullOrEmpty(selectedValue))
            {
                selectedValue = datas[0].Code;
            }
            DefaultItem.Kind kind = String.IsNullOrEmpty(bankId) && String.IsNullOrEmpty(schIdenty) ? DefaultItem.Kind.Select : DefaultItem.Kind.All;
            WebHelper.SetDropDownListItems(this.ddlReceiveType, kind, false, datas, true, false, 0, selectedValue);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ccbtnReport.Visible = this.InitialUI();
            }
        }

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            string bankId = this.ddlBank.SelectedValue;
            this.GetAndBindSchIdentyOption(this.GetLogonUser(), bankId, null);
        }

        protected void ddlSchIdenty_SelectedIndexChanged(object sender, EventArgs e)
        {
            string bankId = this.ddlBank.SelectedValue;
            string schIdenty = this.ddlSchIdenty.SelectedValue;
            this.GetAndBindReceiveTypeOptions(this.GetLogonUser(), bankId, schIdenty, null);
        }

        protected void ccbtnReport_Click(object sender, EventArgs e)
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            string extName = "XLS";
            {
                LinkButton control = sender as LinkButton;
                if (control.CommandArgument == "ODS")
                {
                    extName = "ODS";
                }
            }
            #endregion

            LogonUser logonUser = this.GetLogonUser();

            string bankId = this.ddlBank.SelectedValue.Trim();
            string schIdenty = this.ddlSchIdenty.SelectedValue.Trim();
            string receiveType = this.ddlReceiveType.SelectedValue.Trim();
            if (logonUser.IsBankManager)
            {
                if (String.IsNullOrEmpty(bankId) && String.IsNullOrEmpty(schIdenty) && String.IsNullOrEmpty(receiveType))
                {
                    this.ShowSystemMessage("分行、學校或商家代號至少需要設定一個條件");
                    return;
                }
            }
            else if (logonUser.IsBankUser)
            {
                bankId = logonUser.BankId;
                if (String.IsNullOrEmpty(schIdenty) && String.IsNullOrEmpty(receiveType))
                {
                    this.ShowSystemMessage("學校或商家代號至少需要設定一個條件");
                    return;
                }
            }
            else
            {
                bankId = String.Empty;
                schIdenty = logonUser.UnitId;
            }

            #region 入帳日期區間
            string acctSDate = this.tbxAccountDateS.Text.Trim();
            string acctEDate = this.tbxAccountDateE.Text.Trim();
            {
                if (String.IsNullOrEmpty(acctSDate))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowMustInputAlert("入帳日期區間的啟日");
                    return;
                }
                DateTime sDate;
                if (!DateTime.TryParse(acctSDate, out sDate) || sDate.Year < 1911)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「入帳日期區間的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                    this.ShowJsAlert(msg);
                    return;
                }
                if (String.IsNullOrEmpty(acctEDate))
                {
                    this.ShowMustInputAlert("入帳日期區間的迄日");
                    return;
                }
                DateTime eDate;
                if (!DateTime.TryParse(acctEDate, out eDate) || eDate.Year < 1911)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「入帳日期區間的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                    this.ShowJsAlert(msg);
                    return;
                }
                if (sDate > eDate)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「入帳日期區間」的起日不可大於迄日");
                    this.ShowJsAlert(msg);
                    return;
                }
            }
            #endregion


            string action = this.GetLocalized("手續費統計報表");
            byte[] fileContent = null;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            XmlResult result = DataProxy.Current.ExportReportD(this.Page, bankId, schIdenty, receiveType, acctSDate, acctEDate, out fileContent, (extName == "ODS"));
            #endregion

            if (result.IsSuccess && fileContent != null && fileContent.Length > 0)
            {
                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                string fileName = String.Format("手續費統計報表.{0}", extName);
                this.ResponseFile(fileName, fileContent, extName);
                #endregion
            }
            else
            {
                if (result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, "無資料被匯出");
                }
                else
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                }
            }
        }
    }
}