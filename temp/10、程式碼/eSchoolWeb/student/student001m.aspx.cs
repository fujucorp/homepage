using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

using System.Data;

namespace eSchoolWeb.student
{
    public partial class student001m : LocalizedPage
    {
        #region Override IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "student001m";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "學生專區(行動版)";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region [MDY:20200807] M202008_01 Keep 學生姓名是否要遮罩頁面參數
        /// <summary>
        /// 學生姓名是否要遮罩
        /// </summary>
        private bool KeepIsMaskReceiveType
        {
            get
            {
                object value = ViewState["KeepIsMaskReceiveType"];
                if (value is bool)
                {
                    return (bool)value;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["KeepIsMaskReceiveType"] = value;
            }
        }
        #endregion

        /// <summary>
        /// 結繫登入者(學生)資訊
        /// </summary>
        /// <param name="user">登入者(學生)</param>
        private void BindUserInfo(LogonUser user)
        {
            if (user == null)
            {
                this.labSchoolName.Text = String.Empty;
                this.labStudentID.Text = String.Empty;
                this.labStudentName.Text = String.Empty;
            }
            else
            {
                #region [MDY:202203XX] 2022擴充案 學校英文名稱
                if (this.isEngUI() && !String.IsNullOrEmpty(user.ReceiveTypeEName))
                {
                    this.labSchoolName.Text = user.ReceiveTypeEName;
                }
                else
                {
                    this.labSchoolName.Text = user.ReceiveTypeName;
                }
                #endregion

                this.labStudentID.Text = user.UserId;

                #region [MDY:20200807] M202008_01 依據學生姓名要遮罩的商家代號系統參數決定是否要遮罩 (2020806_01)
                #region [Old]
                //#region [MDY:20200705] 姓名改要遮罩
                //#region 姓名遮罩
                //this.labStudentName.Text = DataFormat.MaskText(user.UserName, DataFormat.MaskDataType.Name);
                //#endregion

                //#region [Old] 姓名不遮罩
                ////this.labStudentName.Text = user.UserName;
                //#endregion
                //#endregion
                #endregion

                if (this.KeepIsMaskReceiveType)
                {
                    this.labStudentName.Text = DataFormat.MaskText(user.UserName, DataFormat.MaskDataType.Name);
                }
                else
                {
                    this.labStudentName.Text = user.UserName;
                }
                #endregion
            }
        }

        /// <summary>
        /// 取得並結繫登入者(學生)的繳費資料
        /// </summary>
        /// <param name="user">登入者(學生)</param>
        private void GetAndBindReceiveData(LogonUser user)
        {
            StudentReceiveView[] datas = null;

            #region [MDY:202203XX] 2022擴充案 改用 GetStudentReceiveViews() 取資料
            #region [OLD]
            //Expression where = new Expression(StudentReceiveView.Field.StuId, user.UserId);
            ////因為學生是使用學校統編登入，同一學號可能跨商家代號
            //if (user.MyReceiveTypes == null || user.MyReceiveTypes.Length <= 1)
            //{
            //    where.And(StudentReceiveView.Field.ReceiveType, user.ReceiveType);
            //}
            //else
            //{
            //    where.And(StudentReceiveView.Field.ReceiveType, user.MyReceiveTypes);
            //}

            //#region [MDY:2018xxxx] 不取未到「開放列印日」的資料
            //where.And(StudentReceiveView.Field.BillOpenDate, RelationEnum.LessEqual, Common.GetDate8(DateTime.Today));
            //#endregion

            //KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(5);
            //orderbys.Add(StudentReceiveView.Field.YearId, OrderByEnum.Desc);
            //orderbys.Add(StudentReceiveView.Field.TermId, OrderByEnum.Desc);
            //orderbys.Add(StudentReceiveView.Field.ReceiveType, OrderByEnum.Asc);
            //orderbys.Add(StudentReceiveView.Field.CreateDate, OrderByEnum.Desc);
            //orderbys.Add(StudentReceiveView.Field.OldSeq, OrderByEnum.Desc);

            //XmlResult xmlResult = DataProxy.Current.SelectAll<StudentReceiveView>(this, where, orderbys, out datas);
            //if (!xmlResult.IsSuccess)
            //{
            //    this.ShowSystemMessage(xmlResult.Code, xmlResult.Message);
            //}
            #endregion

            XmlResult xmlResult = null;
            if (user.MyReceiveTypes == null || user.MyReceiveTypes.Length <= 1)
            {
                xmlResult = DataProxy.Current.GetStudentReceiveViews(this, user.UserId, this.isEngUI(), out datas, user.ReceiveType);
            }
            else
            {
                xmlResult = DataProxy.Current.GetStudentReceiveViews(this, user.UserId, this.isEngUI(), out datas, user.MyReceiveTypes);
            }
            if (!xmlResult.IsSuccess)
            {
                this.ShowSystemMessage(xmlResult.Code, xmlResult.Message);
            }
            #endregion

            this.gvResult.DataSource = datas;
            this.gvResult.DataBind();
        }

        #region [MDY:20200807] M202008_01 檢查指定商家代號是否學生姓名要遮罩 (2020806_01)
        /// <summary>
        /// 檢查指定商家代號是否學生姓名要遮罩
        /// </summary>
        /// <param name="receiveType">指定商家代號</param>
        /// <returns>是則傳回 true，否則傳回 false，檢查失敗也傳回 false</returns>
        private bool IsMaskReceiveType(string receiveType)
        {
            if (!String.IsNullOrWhiteSpace(receiveType) && receiveType.Length == 4)
            {
                #region 讀取學生姓名要遮罩的商家代號系統參數
                ConfigEntity data = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.MASK_RECEIVETYPE);
                KeyValueList<OrderByEnum> orderbys = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, orderbys, out data);
                #endregion

                if (xmlResult.IsSuccess && data != null)
                {
                    if (("," + data.ConfigValue + ",").IndexOf("," + receiveType + ",") > -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 取得登入者資料
                LogonUser user = WebHelper.GetLogonUser();
                if (user == null)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無法取得登入者資訊");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                #region [MDY:20200807] M202008_01 取得指定商家代號是否學生姓名要遮罩 (2020806_01)
                this.KeepIsMaskReceiveType = this.IsMaskReceiveType(user.ReceiveType);
                #endregion

                this.BindUserInfo(user);
                this.GetAndBindReceiveData(user);
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            StudentReceiveView[] datas = this.gvResult.DataSource as StudentReceiveView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            #region [MDY:20190906] (2019擴充案) 多語系
            string paymentLocalized = this.GetLocalized("繳款");
            string queryLocalized = this.GetLocalized("查詢");
            string genBillLocalized = this.GetLocalized("列印繳費單");
            string genReceiptLocalized = this.GetLocalized("列印收據");
            #endregion

            DateTime today = DateTime.Today;
            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentReceiveView data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3},{4},{5},{6}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.StuId, data.OldSeq);

                row.Cells[9].Text = this.GetLocalized(CancelStatusCodeTexts.GetText(data.GetCancelStatus()));

                bool isNonPay = (data.GetCancelStatus() == CancelStatusCodeTexts.NONPAY);

                #region 判斷列印日期
                //DateTime? payDueDate = DataFormat.ConvertDateText(data.PayDueDate);
                DateTime? openDate = DataFormat.ConvertDateText(data.BillOpenDate);
                DateTime? closeDate = DataFormat.ConvertDateText(data.BillCloseDate);
                bool isOpened = ((openDate == null || openDate.Value <= today) && (closeDate == null || closeDate.Value > today));
                #endregion

                //bool hasAmount = data.ReceiveAmount != null;
                //bool isVisible = (data.ReceiveAmount != null && ((data.ReceiveAmount > 0 && !String.IsNullOrEmpty(data.CancelNo)) || data.ReceiveAmount <= 0));
                //bool isVisible = (!String.IsNullOrEmpty(data.CancelNo) && data.ReceiveAmount != null);
                //bool isPayable = (payDueDate != null && payDueDate.Value >= today);
                bool isBillable = (data.ReceiveAmount != null && ((data.ReceiveAmount > 0 && !String.IsNullOrEmpty(data.CancelNo)) || data.ReceiveAmount <= 0));    //有效帳單

                LinkButton lbtnDetail = row.FindControl("lbtnDetail") as LinkButton;
                if (lbtnDetail != null)
                {
                    #region [MDY:20190906] (2019擴充案) 多語系
                    lbtnDetail.Text = isNonPay ? paymentLocalized : queryLocalized;
                    #endregion

                    lbtnDetail.CommandArgument = argument;
                    lbtnDetail.Visible = isOpened;
                }
                Label labMsg = row.FindControl("labMsg") as Label;
                if (labMsg != null)
                {
                    labMsg.Visible = !isOpened;
                }

                LinkButton lbtnGenBill = row.FindControl("lbtnGenBill") as LinkButton;
                if (lbtnGenBill != null)
                {
                    #region [MDY:20190906] (2019擴充案) 多語系
                    lbtnGenBill.Text = genBillLocalized;
                    #endregion

                    lbtnGenBill.CommandArgument = argument;
                    lbtnGenBill.Visible = isNonPay && isOpened && isBillable; // && isVisible && isPayable && isVisible;
                }

                LinkButton lbtnGenReceipt = row.FindControl("lbtnGenReceipt") as LinkButton;
                if (lbtnGenReceipt != null)
                {
                    #region [MDY:20190906] (2019擴充案) 多語系
                    lbtnGenReceipt.Text = genReceiptLocalized;
                    #endregion

                    lbtnGenReceipt.CommandArgument = argument;

                    #region [MDY:2018xxxx] 判斷 列印收據關閉日 與 是否繳款 決定是否顯示 列印收據按鈕
                    #region [OLD]
                    //lbtnGenReceipt.Visible = !isNonPay;
                    #endregion

                    DateTime? invoiceCloseDate = DataFormat.ConvertDateText(data.InvoiceCloseDate);
                    lbtnGenReceipt.Visible = !isNonPay && (!invoiceCloseDate.HasValue || invoiceCloseDate.Value.Date > today);
                    #endregion
                }
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 處理資料參數
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            string[] args = argument.Split(new char[] { ',' }, StringSplitOptions.None);
            if (args.Length != 7)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            string receiveType = args[0];
            string yearId = args[1];
            string termId = args[2];
            string depId = args[3];
            string receiveId = args[4];
            string stuId = args[5];
            string oldSeq = args[6];
            #endregion

            string editUrl = "student002m.aspx";
            switch (e.CommandName)
            {
                case "Detail":
                    #region 明細
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.View);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("YearId", yearId);
                        QueryString.Add("TermId", termId);
                        QueryString.Add("DepId", depId);
                        QueryString.Add("ReceiveId", receiveId);
                        QueryString.Add("StuId", stuId);
                        QueryString.Add("OldSeq", oldSeq);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
                case "GenBill":
                    #region 列印繳費單
                    {
                        byte[] pdfContent = null;

                        #region [MDY:20200807] M202008_01 依據學生姓名要遮罩的商家代號系統參數決定是否要遮罩 (2020806_01)
                        XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, "GENBILL"
                            , receiveType, yearId, termId, depId, receiveId, stuId, Int32.Parse(oldSeq)
                            , this.KeepIsMaskReceiveType, this.isEngUI(), out pdfContent);
                        #endregion

                        if (xmlResult.IsSuccess)
                        {
                            #region [MDY:20190906] (2019擴充案) 多語系
                            #region [MDY:20210401] 原碼修正
                            string fileName = String.Concat(HttpUtility.UrlEncode(stuId), this.GetLocalized("繳費單"), ".PDF");
                            #endregion
                            #endregion

                            this.ResponseFile(fileName, pdfContent);
                        }
                        else
                        {
                            this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
                case "GenReceipt":
                    #region 列印收據
                    {
                        byte[] pdfContent = null;

                        #region [MDY:20200807] M202008_01 依據學生姓名要遮罩的商家代號系統參數決定是否要遮罩 (2020806_01)
                        XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, "GENRECEIPT"
                            , receiveType, yearId, termId, depId, receiveId, stuId, Int32.Parse(oldSeq)
                            , this.KeepIsMaskReceiveType, this.isEngUI(), out pdfContent);
                        #endregion

                        if (xmlResult.IsSuccess)
                        {
                            #region [MDY:20190906] (2019擴充案) 多語系
                            #region [MDY:20210401] 原碼修正
                            string fileName = String.Concat(HttpUtility.UrlEncode(stuId), this.GetLocalized("收據"), ".PDF");
                            #endregion
                            #endregion

                            this.ResponseFile(fileName, pdfContent);
                        }
                        else
                        {
                            this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
            }
        }
    }
}