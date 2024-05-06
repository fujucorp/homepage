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

namespace eSchoolWeb.B
{
    /// <summary>
    /// 產生委扣媒體
    /// </summary>
    public partial class B2100005 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存查詢的業務別碼代碼
        /// </summary>
        private string QueryReceiveType
        {
            get
            {
                return ViewState["QueryReceiveType"] as string;
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學年參數
        /// </summary>
        private string QueryYearId
        {
            get
            {
                return ViewState["QueryYearId"] as string;
            }
            set
            {
                ViewState["QueryYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學期參數
        /// </summary>
        private string QueryTermId
        {
            get
            {
                return ViewState["QueryTermId"] as string;
            }
            set
            {
                ViewState["QueryTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的部別參數
        /// </summary>
        private string QueryDepId
        {
            get
            {
                return ViewState["QueryDepId"] as string;
            }
            set
            {
                ViewState["QueryDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的代收費用別參數
        /// </summary>
        private string QueryReceiveId
        {
            get
            {
                return ViewState["QueryReceiveId"] as string;
            }
            set
            {
                ViewState["QueryReceiveId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的委扣銀行參數
        /// </summary>
        private string QueryBankKind
        {
            get
            {
                return ViewState["QueryCancelType"] as string;
            }
            set
            {
                ViewState["QueryCancelType"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 實作 PagingBasePage's 抽象方法
        /// <summary>
        /// 取得頁面中的分頁控制項陣列
        /// </summary>
        /// <returns>傳回分頁控制項陣列或 null</returns>
        protected override Paging[] GetPagingControls()
        {
            return new Paging[] { this.ucPaging1, this.ucPaging2 };
        }

        /// <summary>
        /// 取得查詢條件與排序方法
        /// </summary>
        /// <param name="where">成功則傳回查詢條件，否則傳回 null</param>
        /// <param name="orderbys">成功則傳回查詢條件，否則傳回 null</param>
        /// <returns>傳回處理結果</returns>
        protected override XmlResult GetWhereAndOrderBys(out Expression where, out KeyValueList<OrderByEnum> orderbys)
        {
            where = null;
            orderbys = null;

            where = new Expression(StudentReceiveView2.Field.ReceiveType, this.QueryReceiveType);
            where.And(StudentReceiveView2.Field.YearId, this.QueryYearId);
            where.And(StudentReceiveView2.Field.TermId, this.QueryTermId);
            where.And(StudentReceiveView2.Field.DepId, this.QueryDepId);
            where.And(StudentReceiveView2.Field.ReceiveId, this.QueryReceiveId);

            #region 委扣銀行 條件
            switch(this.QueryBankKind)
            {
                case "1":   //自行
                    where.And(StudentReceiveView2.Field.DeductBankId, RelationEnum.Like, DataFormat.MyBankID + "%");
                    break;
                case "2":   //其他
                    where.And(StudentReceiveView2.Field.DeductBankId, RelationEnum.NotLike, DataFormat.MyBankID + "%");
                    break;
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReceiveView2.Field.StuId, OrderByEnum.Asc);
            return new XmlResult(true);
        }

        /// <summary>
        /// 呼叫 QueryDataAndBind 方法
        /// </summary>
        /// <param name="pagingInfo">指定分頁訊息</param>
        /// <returns>傳回處理結果</returns>
        protected override XmlResult CallQueryDataAndBind(PagingInfo pagingInfo)
        {
            #region 先不分頁
            //Paging[] ucPagings = this.GetPagingControls();
            //XmlResult xmlResult = base.QueryDataAndBind<StudentReceiveView2>(pagingInfo, ucPagings, this.gvResult);
            //if (!xmlResult.IsSuccess)
            //{
            //    string action = ActionMode.GetActionLocalized(ActionMode.Query);
            //    this.ShowSystemMessage(xmlResult.Code + "，" + xmlResult.Message);
            //}

            ////因為有新增與回上一頁的按鈕，所以無資料時不適合把 gvResult 隱藏
            //this.divResult.Visible = this.gvResult.Rows.Count > 0;
            ////bool showPaging = this.gvResult.Rows.Count > 0;
            ////foreach (Paging ucPaging in ucPagings)
            ////{
            ////    ucPaging.Visible = showPaging;
            ////}
            #endregion

            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;
            XmlResult xmlResult = this.GetWhereAndOrderBys(out where, out orderbys);
            if (xmlResult.IsSuccess)
            {
                StudentReceiveView2[] datas = null;
                xmlResult = DataProxy.Current.SelectAll<StudentReceiveView2>(this.Page, where, orderbys, out datas);
                this.gvResult.DataSource = datas;
                this.gvResult.DataBind();
                this.divResult2.Visible = this.gvResult.Rows.Count > 0;
                this.gvResult.Visible = true;
            }



            return xmlResult;
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.divResult2.Visible = false;
            this.gvResult.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }


        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            #region 5 Key
            this.QueryReceiveType = this.ucFilter1.SelectedReceiveType;
            this.QueryYearId = this.ucFilter1.SelectedYearID;
            this.QueryTermId = this.ucFilter1.SelectedTermID;

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //this.QueryDepId = this.ucFilter2.SelectedDepID;
            #endregion

            this.QueryDepId = "";

            this.QueryReceiveId = this.ucFilter2.SelectedReceiveID;
            if (String.IsNullOrEmpty(this.QueryReceiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return false;
            }
            if (String.IsNullOrEmpty(this.QueryYearId))
            {
                this.ShowMustInputAlert("學年");
                return false;
            }
            if (String.IsNullOrEmpty(this.QueryTermId))
            {
                this.ShowMustInputAlert("學期");
                return false;
            }

            #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(this.QueryDepId))
            //{
            //    this.ShowMustInputAlert("部別");
            //    return false;
            //}
            #endregion

            if (String.IsNullOrEmpty(this.QueryReceiveId))
            {
                this.ShowMustInputAlert("代收費用別");
                return false;
            }
            #endregion

            #region [Old] 土銀只有自行委扣
            //this.QueryBankKind = this.ddlBankKind.SelectedValue;
            #endregion

            this.QueryBankKind = "1";

            return true;
        }

        protected void ccbtnQuery_Click(object sender, EventArgs e)
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

            if (this.GetAndKeepQueryCondition())
            {
                PagingInfo pagingInfo = new PagingInfo();
                XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);

                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }
            }
        }

        protected void ccbtnExportFile_Click(object sender, EventArgs e)
        {
            MyLinkButton ccbtn = sender as MyLinkButton;

            #region [Old] 土銀只有自行委扣
            //if (this.QueryBankKind != "1" && ccbtn.CommandName == "ExportBANK")
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    string msg = this.GetLocalized("委扣銀行非自行時，不可產生自行委扣媒體");
            //    this.ShowSystemMessage(msg);
            //    return;
            //}
            #endregion

            string commandName = "ExportBANK";

            string receiveType = this.ucFilter1.SelectedReceiveType;

            #region 轉帳日期 (土銀的委扣匯出只做前半段，轉帳日是後半段指定，這裡改成匯出當天日期
            DateTime deductDate = DateTime.Today;
            #endregion

            #region 勾選資料
            List<BillKey> keys = new List<BillKey>(gvResult.Rows.Count);
            foreach (GridViewRow row in gvResult.Rows)
            {
                HiddenField hidKey = row.FindControl("hidKey") as HiddenField;
                CheckBox chkSelected = row.FindControl("chkSelected") as CheckBox;
                if (hidKey != null && chkSelected != null && chkSelected.Checked)
                {
                    BillKey key = BillKey.ParseKeyText(hidKey.Value);
                    if (key != null)
                    {
                        keys.Add(key);
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("無法取得勾選的資料");
                        this.ShowJsAlert(msg);
                        return;
                    }
                }
            }
            if (keys.Count == 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("請先勾選要匯出的資料");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            string action = this.ccbtnGenTTB.Text;
            Byte[] fileContent = null;
            XmlResult xmlResult = DataProxy.Current.ExportB2100005File(this.Page, commandName, receiveType, keys.ToArray(), deductDate, out fileContent);
            if (xmlResult.IsSuccess)
            {
                if (fileContent == null || fileContent.Length == 0)
                {
                    this.ShowActionFailureMessage(action, "無資料被匯出");
                }
                else
                {
                    this.ResponseFile(Common.GetTWDate7(deductDate), fileContent, "TXT");
                }
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            StudentReceiveView2[] datas = this.gvResult.DataSource as StudentReceiveView2[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in gvResult.Rows)
            {
                StudentReceiveView2 data = datas[row.RowIndex];
                string key = data.ToKeyText();

                HiddenField hidKey = row.FindControl("hidKey") as HiddenField;
                if (hidKey != null)
                {
                    hidKey.Value = key;
                }
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

    }
}