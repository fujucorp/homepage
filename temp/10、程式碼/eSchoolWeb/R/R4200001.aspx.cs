using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.R
{
    public partial class R4200001 : BasePage
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
        /// 儲存查詢的StudentReturn
        /// </summary>
        private StudentReturnView[] ResultStudentReturns
        {
            get
            {
                return ViewState["ResultStudentReturns"] as StudentReturnView[];
            }
            set
            {
                ViewState["ResultStudentReturns"] = value == null ? null : value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                this.GetAndBindQueryData();
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            #region 處理五個下拉選項
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string receiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out receiveID)
                || String.IsNullOrEmpty(receiveType)
                || String.IsNullOrEmpty(yearID)
                || String.IsNullOrEmpty(termID))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowJsAlert(msg);
                return false;
            }

            //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
            //因為 ucFilter1 有指定 Filter2ControlID 為 ucFilter2，所以 ucFilter2 頁會被自動結繫資料
            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearID, termID, depID, receiveID);
            if (xmlResult.IsSuccess)
            {
                //depID = ucFilter2.SelectedDepID;
                depID = "";
                receiveID = ucFilter2.SelectedReceiveID;
            }

            //一定要用這個方法將業務別碼、學年、學期、部別、代收費用別參數傳給下一頁
            //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
            WebHelper.SetFilterArguments(receiveType, yearID, termID, depID, receiveID);
            #endregion

            this.QueryReceiveType = receiveType;
            this.QueryYearId = yearID;
            this.QueryTermId = termID;
            this.QueryDepId = depID;
            this.QueryReceiveId = receiveID;

            ccbtnGenExcel.Visible = false;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            this.ccbtnGenCalc.Visible = this.ccbtnGenExcel.Visible;
            #endregion
            return true;
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetAndBindQueryData()
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(this.QueryReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                return false;
            }
            #endregion

            #region 查詢條件
            Expression where = this.GetWhere();
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReturnView.Field.StuId, OrderByEnum.Asc);
            #endregion

            StudentReturnView[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<StudentReturnView>(this, where, orderbys, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            else if (datas == null || datas.Length == 0)
            {
                this.ShowErrorMessage(ErrorCode.D_QUERY_NO_DATA, "查無資料");
            }

            ccbtnGenExcel.Visible = true;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            this.ccbtnGenCalc.Visible = this.ccbtnGenExcel.Visible;
            #endregion

            this.ResultStudentReturns = datas;
            this.gvResult.DataSource = datas;
            this.gvResult.DataBind();
            return xmlResult.IsSuccess;
        }

        private Expression GetWhere()
        {
            Expression where = new Expression();

            if (!String.IsNullOrEmpty(this.QueryReceiveType))
            {
                where.And(StudentReturnView.Field.ReceiveType, this.QueryReceiveType);
            }

            if (!String.IsNullOrEmpty(this.QueryYearId))
            {
                where.And(StudentReturnView.Field.YearId, this.QueryYearId);
            }
            if (!String.IsNullOrEmpty(this.QueryTermId))
            {
                where.And(StudentReturnView.Field.TermId, this.QueryTermId);
            }
            if (!String.IsNullOrEmpty(this.QueryDepId))
            {
                where.And(StudentReturnView.Field.DepId, this.QueryDepId);
            }
            if (!String.IsNullOrEmpty(this.QueryReceiveId))
            {
                where.And(StudentReturnView.Field.ReceiveId, this.QueryReceiveId);
            }

            where.And(StudentReturnView.Field.SrNo, string.Empty);

            return where;
        }

        /// <summary>
        /// 取下一個批號
        /// </summary>
        /// <returns></returns>
        private string GetNextSRNo()
        {
            StudentReturnEntity[] datas = null;

            Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, this.QueryReceiveType)
                .And(StudentReturnEntity.Field.YearId, this.QueryYearId)
                .And(StudentReturnEntity.Field.TermId, this.QueryTermId)
                .And(StudentReturnEntity.Field.DepId, this.QueryDepId)
                .And(StudentReturnEntity.Field.ReceiveId, this.QueryReceiveId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReturnEntity.Field.DataNo, OrderByEnum.Desc);

            XmlResult result = DataProxy.Current.SelectAll<StudentReturnEntity>(this, where, orderbys, out datas);
            if (!result.IsSuccess)
            {
                this.ShowSystemMessage(string.Format("讀取退費檔錯誤，") + result.Message);
                return null;
            }

            if (datas == null || datas.Length == 0)
            {
                return "1";
            }

            int maxReSeq = 0;
            foreach (StudentReturnEntity data in datas)
            {
                int reseq = 0;
                if (int.TryParse(data.SrNo, out reseq))
                {
                    if (reseq > maxReSeq)
                    {
                        maxReSeq = reseq;
                    }
                }
            }

            maxReSeq++;

            return maxReSeq.ToString();
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            this.QueryDepId = "";   //土銀沒有使用原部別
            this.QueryReceiveId = ucFilter2.SelectedReceiveID;

            this.GetAndBindQueryData();
            labSRNo.Text = GetNextSRNo();
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void ccbtnGenExcel_Click(object sender, EventArgs e)
        {
            if (!CheckEditData())
            {
                return;
            }

            KeyValueList<string> studentReturnDatas = new KeyValueList<string>();

            StudentReturnView[] datas = this.ResultStudentReturns;
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            DataTable dt = MakeDataTable();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentReturnView data = this.ResultStudentReturns[row.RowIndex];

                CheckBox chk = (CheckBox)row.FindControl("chkSelected");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        #region [MDY:20210805] 原碼修正 (這個 DataTable 是轉成 XLS 檔沒有 XSS 的問題。Checkmarx 誤判。沒轍)
                        #region [OLD]
                        //string cell1 = row.Cells[1].Text == "&nbsp;" ? string.Empty : row.Cells[1].Text;
                        //string cell2 = row.Cells[2].Text == "&nbsp;" ? string.Empty : row.Cells[2].Text;
                        //string cell3 = row.Cells[3].Text == "&nbsp;" ? string.Empty : row.Cells[3].Text;
                        //string cell4 = row.Cells[4].Text == "&nbsp;" ? string.Empty : row.Cells[4].Text;
                        #endregion

                        string cell1 = row.Cells[1].Text == "&nbsp;" ? string.Empty : HttpUtility.HtmlEncode(row.Cells[1].Text);
                        string cell2 = row.Cells[2].Text == "&nbsp;" ? string.Empty : HttpUtility.HtmlEncode(row.Cells[2].Text);
                        string cell3 = row.Cells[3].Text == "&nbsp;" ? string.Empty : HttpUtility.HtmlEncode(row.Cells[3].Text);
                        string cell4 = row.Cells[4].Text == "&nbsp;" ? string.Empty : HttpUtility.HtmlEncode(row.Cells[4].Text);
                        #endregion

                        dt.Rows.Add(cell1, cell2, cell3, cell4);

                        string argument = string.Format("{0}, {1}", data.DataNo, labSRNo.Text.Trim());
                        studentReturnDatas.Add("args", argument);
                    }
                }
            }

            #region update SrNO

            //執行多筆銷帳
            if (studentReturnDatas.Count > 0)
            {
                object returnData = null;
                XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.UpdateStudentReturnDatas, studentReturnDatas, out returnData);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowSystemMessage(this.GetLocalized("資料更新失敗") + "，" + xmlResult.Message);
                    return;
                }
            }
            #endregion

            #region [Old] 因為 Web 端參考 NPOI V2.0，所以改用 ExcelHelper.dll 的 ConvertFileHelper
            //ConvertFileHelper helper = new ConvertFileHelper();

            //byte[] content = helper.Dt2Xls(dt);
            #endregion

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式 & ConvertFileHelper 已改用 NPOI V2.0
            {
                byte[] fileContent = null;
                string fileName = null;

                LinkButton control = sender as LinkButton;
                if (control.CommandArgument == "ODS")
                {
                    #region 產生 ods 的檔案內容
                    ODSHelper helper = new ODSHelper();
                    fileContent = helper.DataTable2ODS(dt, "sheet1");
                    fileName = string.Format("{0}.ods", tbxFileName.Text.Trim());
                    #endregion
                }
                else
                {
                    #region 產生 xls 的檔案內容
                    //ExcelHelper.ConvertFileHelper helper = new ExcelHelper.ConvertFileHelper();
                    ConvertFileHelper helper = new ConvertFileHelper();
                    fileContent = helper.DataTable2Xls(dt, "sheet1");
                    fileName = string.Format("{0}.xls", tbxFileName.Text.Trim());
                    #endregion
                }

                if (fileContent == null)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("產生檔案失敗");
                    this.ShowSystemMessage(msg);
                    return;
                }
                else
                {
                    #region [MDY:20210401] 原碼修正
                    this.ResponseFile(HttpUtility.UrlEncode(fileName), fileContent);
                    #endregion
                }
            }
            #endregion
        }

        private DataTable MakeDataTable()
        {
            GridViewRow headerRow = gvResult.HeaderRow;

            DataTable dt = new DataTable();

            #region [MDY:20210805] 原碼修正 (這個 DataTable 是轉成 XLS 檔沒有 XSS 的問題。Checkmarx 誤判。沒轍)
            #region [OLD]
            //dt.Columns.AddRange(new DataColumn[4]{
            //        new DataColumn(headerRow.Cells[1].Text, typeof(string)),
            //        new DataColumn(headerRow.Cells[2].Text, typeof(string)),
            //        new DataColumn(headerRow.Cells[3].Text, typeof(string)),
            //        new DataColumn(headerRow.Cells[4].Text, typeof(string)) });
            #endregion

            dt.Columns.AddRange(new DataColumn[4]{
                    new DataColumn("姓名", typeof(string)),
                    new DataColumn("學號", typeof(string)),
                    new DataColumn("虛擬帳號", typeof(string)),
                    new DataColumn("退費金額", typeof(string)) });
            #endregion

            return dt;

        }


        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            #region  檔名
            {
                if (tbxFileName.Text.Trim() == "")
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("請輸入「檔名」");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            #region 不須填寫副檔名
            {
                if (tbxFileName.Text.IndexOf('.') > 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("不須填寫副檔名");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            #region 判斷是否有勾選欲產生的資料
            {
                foreach (GridViewRow row in this.gvResult.Rows)
                {
                    StudentReturnView data = this.ResultStudentReturns[row.RowIndex];

                    CheckBox chk = (CheckBox)row.FindControl("chkSelected");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            return true;
                        }
                    }
                }

                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("至少選擇一筆");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion
        }

        protected void ccbtnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentReturnView data = this.ResultStudentReturns[row.RowIndex];

                CheckBox chk = (CheckBox)row.FindControl("chkSelected");
                if (chk != null)
                {
                    chk.Checked = true;
                }
            }
        }
    }
}