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
    /// 複製代碼檔與標準檔
    /// </summary>
    public partial class D1700001 : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 編輯的業務別碼參數
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return ViewState["EditReceiveType"] as string;
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學年1參數
        /// </summary>
        private string EditYearId1
        {
            get
            {
                return ViewState["EditYearId1"] as string;
            }
            set
            {
                ViewState["EditYearId1"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學年2參數
        /// </summary>
        private string EditYearId2
        {
            get
            {
                return ViewState["EditYearId2"] as string;
            }
            set
            {
                ViewState["EditYearId2"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學期1參數
        /// </summary>
        private string EditTermId1
        {
            get
            {
                return ViewState["EditTermId1"] as string;
            }
            set
            {
                ViewState["EditTermId1"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學期2參數
        /// </summary>
        private string EditTermId2
        {
            get
            {
                return ViewState["EditTermId2"] as string;
            }
            set
            {
                ViewState["EditTermId2"] = value == null ? null : value.Trim();
            }
        }
        #endregion

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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        /// <summary>
        /// 介面初始話
        /// </summary>
        private bool InitialUI()
        {
            this.EditReceiveType = ucFilter1.SelectedReceiveType;

            BindYearIdOptions();

            this.EditYearId1 = ddlYearId1.SelectedValue;
            this.EditYearId2 = ddlYearId2.SelectedValue;
            BindTermId1Options();
            BindTermId2Options();
            this.EditTermId1 = ddlTermId1.SelectedValue;
            this.EditTermId2 = ddlTermId2.SelectedValue;

            return true;
        }

        /// <summary>
        /// 繫結 學年 下拉選單
        /// </summary>
        private void BindYearIdOptions()
        {
            List<ListItem> list = new List<ListItem>();

            Expression where = new Expression();

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(YearListEntity.Field.YearId, OrderByEnum.Asc);

            YearListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<YearListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (YearListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.YearName, data.YearId);
                    list.Add(new ListItem(text, data.YearId));
                }
            }

            this.ddlYearId1.Items.Clear();
            this.ddlYearId2.Items.Clear();
            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlYearId1.Items.AddRange(items);

                this.ddlYearId2.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 繫結 學期1 下拉選單
        /// </summary>
        private void BindTermId1Options()
        {
            List<ListItem> list = new List<ListItem>();

            Expression where = new Expression();
            where.And(TermListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(TermListEntity.Field.YearId, this.EditYearId1);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(TermListEntity.Field.TermId, OrderByEnum.Asc);

            TermListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<TermListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (TermListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.TermName, data.TermId);
                    list.Add(new ListItem(text, data.TermId));
                }
            }

            this.ddlTermId1.Items.Clear();
            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlTermId1.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 繫結 學期2 下拉選單
        /// </summary>
        private void BindTermId2Options()
        {
            List<ListItem> list = new List<ListItem>();

            Expression where = new Expression();
            where.And(TermListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(TermListEntity.Field.YearId, this.EditYearId2);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(TermListEntity.Field.TermId, OrderByEnum.Asc);

            TermListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<TermListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (TermListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.TermName, data.TermId);
                    list.Add(new ListItem(text, data.TermId));
                }
            }

            this.ddlTermId2.Items.Clear();
            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlTermId2.Items.AddRange(items);
            }
        }

        protected void ddlYearId1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EditYearId1 = ddlYearId1.SelectedValue;
            BindTermId1Options();
        }
        
        protected void ddlYearId2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EditYearId2 = ddlYearId2.SelectedValue;
            BindTermId2Options();
        }

        protected void ucFilter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EditReceiveType = ucFilter1.SelectedReceiveType;
            //BindYearIdOptions();
            BindTermId1Options();
            BindTermId2Options();
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region [Old]
            //this.EditTermId1 = ddlTermId1.SelectedValue;
            //this.EditTermId2 = ddlTermId2.SelectedValue;

            //if (String.IsNullOrEmpty(this.EditReceiveType)
            //    || String.IsNullOrEmpty(this.EditYearId1) || String.IsNullOrEmpty(this.EditYearId2)
            //    || String.IsNullOrEmpty(this.EditTermId1) || String.IsNullOrEmpty(this.EditTermId2))
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    string mesg = this.GetLocalized("請選擇複製來源及複製目的的商家代號、學年、學期");
            //    this.ShowJsAlert(mesg);
            //    return;
            //}

            //string receiveType = this.EditReceiveType;
            //string yearId1 = this.EditYearId1;
            //string termId1 = this.EditTermId1;
            //string yearId2 = this.EditYearId2;
            //string termId2 = this.EditTermId2;
            //KeyValueList<string> arguments = new KeyValueList<string>(5);
            //arguments.Add("receiveType", receiveType);
            //arguments.Add("yearId1", yearId1);
            //arguments.Add("yearId2", yearId2);
            //arguments.Add("termId1", termId1);
            //arguments.Add("termId2", termId2);
            //object returnData = null;
            
            //StringBuilder sb = new StringBuilder();

            //#region 複製部別代碼 chkDepId
            //if (chkDepId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyDep, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製部別代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製部別代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製院別代碼 chkCollegeId
            //if (chkCollegeId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyCollege, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製院別代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製院別代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製科系代碼 chkMajorId
            //if (chkMajorId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyMajor, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製科系代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製科系代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製班別代碼 chkClassId
            //if (chkClassId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyClass, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製班別代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製班別代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製減免類別代碼 chkReduceId
            //if (chkReduceId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyReduce, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製減免類別代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製減免類別代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製住宿代碼 chkDormId
            //if (chkDormId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyDorm, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製住宿代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製住宿代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製身分註記代碼 chkIdentify
            //if (chkIdentify.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyIdentify, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製身分註記代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製身分註記代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製就貸代碼 chkLoanId
            //if (chkLoanId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyLoan, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製就貸代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製就貸代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製退費代碼 chkReturnId
            //if (chkReturnId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyReturn, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製退費代碼成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製退費代碼失敗");
            //    }
            //}
            //#endregion

            //#region 複製代收費用別 chkReceiveId
            //if (chkReceiveId.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyReceive, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製代收費用別成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製代收費用別失敗");
            //    }
            //}
            //#endregion

            //#region 複製商家代號費用 (費用別設定) chkSchoolRid
            //if (chkSchoolRid.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyReceive, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製代收費用別成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製代收費用別失敗");
            //    }
            //}
            //#endregion

            //#region 一般收費標準 chkGeneralStandard
            //if (chkGeneralStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyGeneralStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製一般收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製一般收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 小於基數其他收費標準 chkCredit2Standard
            //if (chkCredit2Standard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyCredit2Standard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製小於基數其他收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製小於基數其他收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 學分費收費標準 chkCreditStandard
            //if (chkCreditStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyCreditStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製學分費收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製學分費收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 住宿費收費標準 chkDormStandard
            //if (chkDormStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyDormStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製住宿費收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製住宿費收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 學分費基準收費標準 chkCreditbStandard
            //if (chkCreditbStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyCreditbStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製學分費基準收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製學分費基準收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 身分註記收費標準 chkIdentifyStandard
            //if (chkIdentifyStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyIdentifyStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製身分註記收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製身分註記收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 課程收費標準 chkClassStandard
            //if (chkClassStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyClassStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製課程收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製課程收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 減免收費標準 chkReduceStandard
            //if (chkReduceStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyReduceStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製減免收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製減免收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 就貸收費標準 chkLoanStandard
            //if (chkLoanStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyLoanStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製就貸收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製就貸收費標準失敗");
            //    }
            //}
            //#endregion

            //#region 退費收費標準 chkReturnStandard
            //if (chkReturnStandard.Checked)
            //{
            //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyReturnStandard, arguments, out returnData);
            //    if (xmlResult.IsSuccess)
            //    {
            //        sb.AppendLine("複製退費收費標準成功");
            //    }
            //    else
            //    {
            //        sb.AppendLine("複製退費收費標準失敗");
            //    }
            //}
            //#endregion

            //string msg = sb.ToString();
            //this.ShowSystemMessage(msg);
            #endregion

            #region 檢查參數
            string receiveType = this.ucFilter1.SelectedReceiveType;
            string yearId1 = this.ddlYearId1.SelectedValue;
            string termId1 = this.ddlTermId1.SelectedValue;
            string yearId2 = this.ddlYearId2.SelectedValue;
            string termId2 = this.ddlTermId2.SelectedValue;
            if (String.IsNullOrWhiteSpace(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }
            if (String.IsNullOrWhiteSpace(yearId1) || String.IsNullOrWhiteSpace(termId1))
            {
                this.ShowMustInputAlert("複製來源的學年、學期");
                return;
            }
            if (String.IsNullOrWhiteSpace(yearId2) || String.IsNullOrWhiteSpace(termId2))
            {
                this.ShowMustInputAlert("複製目的的學年、學期");
                return;
            }
            if (yearId1 == yearId2 && termId1 == termId2)
            {
                this.ShowSystemMessage("複製來源與複製目的的學年、學期不可完全相同");
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
            XmlResult xmlResult = DataProxy.Current.CopyBaseData(this.Page, receiveType, yearId1, termId1, yearId2, termId2, chkEntityNames, out msg);
            if (xmlResult.IsSuccess)
            {
                this.labResultMsg.Text = msg.Replace("\n", "<br/>");
                this.ShowJsAlert("複製代碼檔與標準檔處理完成");
            }
            else
            {
                this.labResultMsg.Text = String.Empty;
                this.ShowActionFailureMessage("複製代碼檔與標準檔", xmlResult.Message);
            }
        }
    }
}