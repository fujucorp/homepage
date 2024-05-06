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

namespace eSchoolWeb.C
{
    /// <summary>
    /// 人工銷帳
    /// </summary>
    public partial class C3300001M : BasePage
    {
        #region Property
        /// <summary>
        /// 儲存業務別碼代碼的查詢條件
        /// </summary>
        private string QueryProblemCancelNo
        {
            get
            {
                return ViewState["QueryProblemCancelNo"] as string;
            }
            set
            {
                ViewState["QueryProblemCancelNo"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存ProblemList資料
        /// </summary>
        private ProblemListEntity EditProblemList
        {
            get
            {
                return ViewState["EditProblemList"] as ProblemListEntity;
            }
            set
            {
                ViewState["EditProblemList"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 儲存學生資料
        /// </summary>
        private StudentView EditStudentView
        {
            get
            {
                return ViewState["EditStudentView"] as StudentView;
            }
            set
            {
                ViewState["EditStudentView"] = value == null ? null : value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.QueryProblemCancelNo = QueryString.TryGetValue("ProblemCancelNo", String.Empty);
                if (String.IsNullOrEmpty(this.QueryProblemCancelNo))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                #region 取得資料
                ProblemListEntity data = null;
                {
                    string action = this.GetLocalized("查詢明細資料");

                    #region 查詢條件
                    Expression where = new Expression(ProblemListEntity.Field.CancelNo, this.QueryProblemCancelNo);
                    #endregion

                    #region 查詢資料
                    XmlResult xmlResult = DataProxy.Current.SelectFirst<ProblemListEntity>(this, where, null, out data);
                    if (!xmlResult.IsSuccess)
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        return;
                    }
                    if (data == null)
                    {
                        string msg = this.GetLocalized("資料不存在");
                        this.ShowJsAlertAndGoUrl(msg, "C3300001.aspx");
                    }
                    #endregion

                    #region [MDY:20210721] FIX BUG 檢查資料的身家代號授權
                    if (data != null && !this.GetLogonUser().IsAuthReceiveTypes(data.ReceiveType))
                    {
                        string msg = this.GetLocalized("該虛擬帳號無權限");
                        this.ShowJsAlertAndGoUrl(msg, "C3300001.aspx");
                    }
                    #endregion
                }
                #endregion

                this.EditProblemList = data;
                this.BindProblemData(data);
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.labProblemCancelNo.Text = string.Empty;
            this.labPayAmount.Text = string.Empty;
            this.labReceiveWay.Text = string.Empty;
            this.labReceiveDate.Text = string.Empty;
            this.labAccountDate.Text = string.Empty;
            this.labProblemRemark.Text = string.Empty;

            //divGoNext
            this.tbxCancelNo.Text = string.Empty;

            //divNextPage
            this.labCancelNo.Text = string.Empty;
            this.labStuId.Text = string.Empty;
            this.labName.Text = string.Empty;
            this.labReceiveAmount.Text = string.Empty;

            divGoNext.Visible = true;
            divQuery.Visible = true;
            divNextPage.Visible = false;
        }

        /// <summary>
        /// 繫結銷帳問題檔資料
        /// </summary>
        /// <param name="data">銷帳問題檔資料</param>
        private void BindProblemData(ProblemListEntity data)
        {
            if (data == null)
            {
                this.labProblemCancelNo.Text = string.Empty;
                this.labPayAmount.Text = string.Empty;
                this.labReceiveWay.Text = string.Empty;
                this.labReceiveDate.Text = string.Empty;
                this.labAccountDate.Text = string.Empty;
                this.labProblemRemark.Text = string.Empty;
            }
            else
            {
                this.labProblemCancelNo.Text = data.CancelNo;
                this.labPayAmount.Text = DataFormat.GetAmountText(data.PayAmount);
                this.labReceiveWay.Text = GetChannelName(data.ReceiveWay);
                this.labReceiveDate.Text = data.ReceiveDate == null ? string.Empty : data.ReceiveDate.Value.ToString("yyyy/MM/dd");
                this.labAccountDate.Text = data.AccountDate == null ? string.Empty : data.AccountDate.Value.ToString("yyyy/MM/dd");
                this.labProblemRemark.Text = data.ProblemRemark;
            }
        }

        private string GetChannelName(string receiveWay)
        {
            ChannelSetEntity entity = null;

            Expression where = new Expression();
            where.And(ChannelSetEntity.Field.ChannelId, receiveWay);

            XmlResult result = DataProxy.Current.SelectFirst<ChannelSetEntity>(this, where, null, out entity);
            if (!result.IsSuccess || entity == null)
            {
                return string.Empty;
            }
            else
            {
                return entity.ChannelName;
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string action = this.GetLocalized("查詢明細資料");
            string backUrl = "C3300001.aspx";

            #region Step 1. update student_receive cancel_flag='8'

            StudentView data = this.EditStudentView;
            ProblemListEntity problemData = this.EditProblemList;


            if (data == null)
            {
                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                return;
            }

            #region 更新條件
            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, data.ReceiveType)
                .And(StudentReceiveEntity.Field.YearId, data.YearId)
                .And(StudentReceiveEntity.Field.TermId, data.TermId)
                .And(StudentReceiveEntity.Field.DepId, data.DepId)
                .And(StudentReceiveEntity.Field.ReceiveId, data.ReceiveId)
                .And(StudentReceiveEntity.Field.StuId, data.StuId)
                .And(StudentReceiveEntity.Field.OldSeq, data.OldSeq);
            #endregion

            string receiveDate = problemData.ReceiveDate == null ? string.Empty : Common.GetTWDate7(((DateTime)problemData.ReceiveDate));
            string accountDate = problemData.AccountDate == null ? string.Empty : Common.GetTWDate7(((DateTime)problemData.AccountDate));
            string receiveTime = DateTime.Now.ToString("hhmmss");
            string cancelDate = DateTime.Today.ToString("yyyyMMdd");
            string receiveWay = string.Empty;
            string receiveBankId = string.Empty;

            #region [Old] 土銀沒有 C 管道，所以不用區分
            //if (GetLogonUser().UserQual == UserQualCodeTexts.BANK)
            //{
            //    receiveWay = "S";  //(如果登入者是行員就是S不然就是C)
            //    receiveBankId = GetLogonUser().BankId;
            //}
            //else
            //{
            //    receiveWay = "C";  //(如果登入者是行員就是S不然就是C)
            //    receiveBankId = string.Empty;
            //}
            #endregion

            receiveWay = ChannelHelper.SELF;
            if (GetLogonUser().UserQual == UserQualCodeTexts.BANK)
            {
                receiveBankId = GetLogonUser().BankId;
            }
            else
            {
                receiveBankId = string.Empty;
            }

            #region 更新欄位
            KeyValueList fieldValues = new KeyValueList();
            fieldValues.Add(StudentReceiveEntity.Field.CancelFlag, "8");
            fieldValues.Add(StudentReceiveEntity.Field.ReceiveDate, receiveDate);
            fieldValues.Add(StudentReceiveEntity.Field.AccountDate, accountDate);
            fieldValues.Add(StudentReceiveEntity.Field.ReceiveTime, receiveTime);
            fieldValues.Add(StudentReceiveEntity.Field.CancelDate, cancelDate);
            fieldValues.Add(StudentReceiveEntity.Field.ReceiveWay, receiveWay);
            fieldValues.Add(StudentReceiveEntity.Field.ReceivebankId, receiveBankId);
            #endregion

            int count = 0;
            XmlResult xmlResult = DataProxy.Current.UpdateFields<StudentReceiveEntity>(this, where, fieldValues, out count);
            if (!xmlResult.IsSuccess)
            {

                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }

            if (count < 1)
            {
                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                return;
            }
            #endregion

            #region Step 2. 刪除 ProblemList
            count = 0;
            xmlResult = DataProxy.Current.Delete<ProblemListEntity>(this, problemData, out count);
            if (xmlResult.IsSuccess)
            {
                if (count < 1)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                    return;
                }
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }
            #endregion

            string msg = this.GetLocalized("銷帳成功");
            this.ShowJsAlertAndGoUrl(msg, backUrl);

        }

        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            if (!CheckEditData())
            {
                return;
            }

            this.BindStudentReceiveData();

        }

        /// <summary>
        /// 繫結學生繳費檔資料
        /// </summary>
        /// <param name="data">學生繳費檔資料</param>
        private void BindStudentReceiveData()
        {
            #region 取得資料
            StudentView data = null;
            {
                string action = this.GetLocalized("查詢明細資料");

                #region 查詢條件
                Expression where = new Expression(StudentView.Field.CancelNo, this.tbxCancelNo.Text.Trim());

                #region [MDY:20181203] 排除已繳的資料 (20181201_06)
                where.And(new Expression(StudentView.Field.ReceiveWay, null).Or(StudentView.Field.ReceiveWay, String.Empty))
                    .And(new Expression(StudentView.Field.ReceiveDate, null).Or(StudentView.Field.ReceiveDate, String.Empty))
                    .And(new Expression(StudentView.Field.AccountDate, null).Or(StudentView.Field.AccountDate, String.Empty));
                #endregion
                #endregion

                #region 查詢資料
                XmlResult xmlResult = DataProxy.Current.SelectFirst<StudentView>(this, where, null, out data);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return;
                }
                if (data == null)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                    return;
                }
                #endregion
            }
            #endregion

            if (data == null)
            {
                this.labCancelNo.Text = string.Empty;
                this.labStuId.Text = string.Empty;
                this.labName.Text = string.Empty;
                this.labReceiveAmount.Text = string.Empty;
            }
            else
            {
                divQuery.Visible = false;
                divGoNext.Visible = false;
                divNextPage.Visible = true;

                this.EditStudentView = data;

                this.labCancelNo.Text = data.CancelNo;
                this.labStuId.Text = data.StuId;
                this.labName.Text = data.Name;
                this.labReceiveAmount.Text = DataFormat.GetAmountText(data.ReceiveAmount);
            }
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            #region [MDY:20170721] FIX BUG
            {
                string cancelNo = tbxCancelNo.Text.Trim();
                if (String.IsNullOrEmpty(cancelNo))
                {
                    this.ShowMustInputAlert("繳費資料的虛擬帳號");
                    return false;
                }

                if (!Common.IsNumber(cancelNo) || (cancelNo.Length != 14 && cancelNo.Length != 16))
                {
                    this.ShowSystemMessage(this.GetLocalized("繳費資料的虛擬帳號不合法"));
                    return false;
                }

                string receiveType = cancelNo.Substring(0, 4);
                if (this.EditProblemList == null || this.EditProblemList.ReceiveType != receiveType)
                {
                    this.ShowSystemMessage(this.GetLocalized("繳費資料的虛擬帳號與銷帳問題檔的虛擬帳號，必須是同一個商家代號"));
                    return false;
                }

                if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "該繳費資料的虛擬帳號無權限");
                    return false;
                }
            }
            #endregion

            return true;
        }
    }
}