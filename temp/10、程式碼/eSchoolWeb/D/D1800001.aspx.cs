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

namespace eSchoolWeb.D
{
    /// <summary>
    /// 刪除代碼檔與標準檔
    /// </summary>
    public partial class D1800001 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.Request.QueryString["t"] != null)
                {
                    string queryType = this.Request.QueryString["t"].ToString();
                    switch (queryType)
                    {
                        case "1":
                            panList.Visible = true;
                            panStandard.Visible = false;
                            break;
                        case "3":
                            panList.Visible = false;
                            panStandard.Visible = true;
                            break;
                        default:
                            panList.Visible = true;
                            panStandard.Visible = true;
                            break;

                    }
                }
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region [Old]
            //string receiveType = this.ucFilter1.SelectedReceiveType;
            //string yearID = this.ucFilter1.SelectedYearID;
            //string termID = this.ucFilter1.SelectedTermID;
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearID) || String.IsNullOrEmpty(termID))
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    string mesg = this.GetLocalized("請選擇欲刪除資料的商家代號、學年、學期");
            //    this.ShowJsAlert(mesg);
            //    return;
            //}

            //KeyValueList<string> arguments = new KeyValueList<string>(3);
            //arguments.Add("receiveType", receiveType);
            //arguments.Add("yearId", yearID);
            //arguments.Add("termId", termID);
            //object returnData = null;

            //StringBuilder sb = new StringBuilder();

            //#region 刪除部別代碼 chkDepId
            //if (chkDepId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelDep, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除部別代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除部別代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除院別代碼 chkCollegeId
            //if (chkCollegeId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelCollege, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除院別代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除院別代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除科系代碼 chkMajorId
            //if (chkMajorId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelMajor, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除科系代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除科系代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除班別代碼 chkClassId
            //if (chkClassId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelClass, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除班別代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除班別代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除減免類別代碼 chkReduceId
            //if (chkReduceId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelReduce, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除減免類別代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除減免類別代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除住宿代碼 chkDormId
            //if (chkDormId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelDorm, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除住宿代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除住宿代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除身分註記代碼 chkIdentify
            //if (chkIdentify.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelIdentify, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除身分註記代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除身分註記代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除就貸代碼 chkLoanId
            //if (chkLoanId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelLoan, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除就貸代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除就貸代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除退費代碼 chkReturnId
            //if (chkReturnId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelReturn, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除退費代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除退費代碼失敗");
            //    }
            //}
            //#endregion

            //#region 刪除代收費用別 chkReceiveId
            //if (chkReceiveId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelReceive, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除代收費用別成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除代收費用別失敗");
            //    }
            //}
            //#endregion

            ////商家代號費用

            //#region 刪除一般收費標準 chkGeneralStandard
            //if (chkGeneralStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelGeneralStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除一般收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除一般收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除小於基數其他收費標準 chkCredit2Standard
            //if (chkCredit2Standard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelCredit2Standard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除小於基數其他收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除小於基數其他收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除學分費收費標準 chkCreditStandard
            //if (chkCreditStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelCreditStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除學分費收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除學分費收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除住宿費收費標準 chkDormStandard
            //if (chkDormStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelDormStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除住宿費收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除住宿費收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除學分費基準收費標準 chkCreditbStandard
            //if (chkCreditbStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelCreditbStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除學分費基準收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除學分費基準收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除身分註記收費標準 chkIdentifyStandard
            //if (chkIdentifyStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelIdentifyStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除身分註記收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除身分註記收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除課程收費標準 chkClassStandard
            //if (chkClassStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelClassStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除課程收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除課程收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除減免收費標準 chkReduceStandard
            //if (chkReduceStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelReduceStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除減免收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除減免收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除就貸收費標準 chkLoanStandard
            //if (chkLoanStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelLoanStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除就貸收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除就貸收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 刪除退費收費標準 chkReturnStandard
            //if (chkReturnStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.DelReturnStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("刪除退費收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("刪除退費收費標準失敗");
            //    }
            //}
            //#endregion

            //string msg = sb.ToString();
            //this.ShowSystemMessage(msg);
            #endregion

            #region 檢查參數
            string receiveType = this.ucFilter1.SelectedReceiveType;
            string yearId = this.ucFilter1.SelectedYearID;
            string termId = this.ucFilter1.SelectedTermID;
            if (String.IsNullOrWhiteSpace(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }
            if (String.IsNullOrWhiteSpace(yearId))
            {
                this.ShowMustInputAlert("學年");
                return;
            }
            if (String.IsNullOrWhiteSpace(termId))
            {
                this.ShowMustInputAlert("學期");
                return;
            }

            CheckBox[] entityCbxs = new CheckBox[] {
                this.chkDeptList, this.chkCollegeList, this.chkMajorList, this.chkClassList, this.chkReduceList,
                this.chkDormList, this.chkLoanList, this.chkReturnList, this.chkIdentifyList, this.chkReceiveList,
                this.chkSchoolRid,
                this.chkGeneralStandard, this.chkCredit2Standard, this.chkCreditStandard, this.chkDormStandard, this.chkCreditbStandard,
                this.chkIdentifyStandard, this.chkClassStandard, this.chkReduceStandard, this.chkLoanStandard, this.chkReturnStandard
            };
            string[] entityNames = new string[] {
                DeptListEntity.TABLE_NAME, CollegeListEntity.TABLE_NAME, MajorListEntity.TABLE_NAME, ClassListEntity.TABLE_NAME, ReduceListEntity.TABLE_NAME, 
                DormListEntity.TABLE_NAME, LoanListEntity.TABLE_NAME, ReturnListEntity.TABLE_NAME, IdentifyList1Entity.TABLE_NAME, ReceiveListEntity.TABLE_NAME, 
                SchoolRidEntity.TABLE_NAME,
                GeneralStandardEntity.TABLE_NAME, Credit2StandardEntity.TABLE_NAME, CreditStandardEntity.TABLE_NAME, DormStandardEntity.TABLE_NAME, CreditbStandardEntity.TABLE_NAME,
                IdentifyStandard1Entity.TABLE_NAME, ClassStandardEntity.TABLE_NAME, ReduceStandardEntity.TABLE_NAME, LoanStandardEntity.TABLE_NAME, ReturnStandardEntity.TABLE_NAME
            };
            List<string> chkEntityNames = new List<string>(entityCbxs.Length);
            for (int idx = 0; idx < entityCbxs.Length; idx++)
            {
                if (entityCbxs[idx].Checked)
                {
                    chkEntityNames.Add(entityNames[idx]);
                }
            }
            #endregion

            string msg = null;
            XmlResult xmlResult = DataProxy.Current.DeleteBaseData(this.Page, receiveType, yearId, termId, chkEntityNames, out msg);
            if (xmlResult.IsSuccess)
            {
                this.labResultMsg.Text = msg.Replace("\n", "<br/>");
                this.ShowJsAlert("刪除代碼檔與標準檔處理完成");
            }
            else
            {
                this.labResultMsg.Text = String.Empty;
                this.ShowActionFailureMessage("刪除代碼檔與標準檔", xmlResult.Message);
            }
        }

    }
}