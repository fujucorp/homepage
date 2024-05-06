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

namespace eSchoolWeb.R
{
    public partial class R4300001 : BasePage
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
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

            ccbtnOK.Visible = true;
            SetddlSrNoOptions();
            return true;
        }

        /// <summary>
        /// 設定身分註記代碼下接選單
        /// </summary>
        protected void SetddlSrNoOptions()
        {
            this.ddlSrNo.Items.Clear();

            StudentReturnEntity[] datas = null;

            Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, this.QueryReceiveType)
                .And(StudentReturnEntity.Field.YearId, this.QueryYearId)
                .And(StudentReturnEntity.Field.TermId, this.QueryTermId)
                .And(StudentReturnEntity.Field.DepId, this.QueryDepId)
                .And(StudentReturnEntity.Field.ReceiveId, this.QueryReceiveId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReturnEntity.Field.SrNo, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<StudentReturnEntity>(this, where, orderbys, out datas);
            if (!result.IsSuccess)
            {
                this.ShowSystemMessage(string.Format("讀取退費檔錯誤，") + result.Message);
                return;
            }

            if (datas == null || datas.Length == 0)
            {
                this.ShowSystemMessage(string.Format("無退費檔資料") + result.Message);
                return;
            }

            List<ListItem> list = new List<ListItem>();
            string srno = string.Empty;
            foreach (StudentReturnEntity data in datas)
            {
                if (srno != data.SrNo.Trim())
                {
                    srno = data.SrNo.Trim();

                    list.Add(new ListItem(srno, srno));
                }
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlSrNo.Items.AddRange(items);
            }
        }

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            this.QueryDepId = "";   //土銀沒有使用原部別
            this.QueryReceiveId = ucFilter2.SelectedReceiveID;

            this.SetddlSrNoOptions();
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!CheckEditData())
            {
                return;
            }

            StringBuilder txtContent = new StringBuilder();
            DateTime dt = DateTime.Now;
            DateTime.TryParse(tbxDate.Text, out dt);
            string OutputDate = dt.ToString("yyyyMMdd");

            StudentReturnView[] datas = GetDatas();
            foreach (StudentReturnView data in datas)
            {
                //姓名, 學號, 銷帳編號, 退費金額
                txtContent.AppendFormat("{0}, {1}, {2}, {3}, {4}", data.StuName, data.StuId, data.CancelNo, DataFormat.GetAmountText(data.ReturnAmount), OutputDate).AppendLine();
            }

            #region [MDY:20210401] 原碼修正
            string fileName = string.Format("{0}.txt", HttpUtility.UrlEncode(tbxFileName.Text.Trim()));
            #endregion

            #region [MDY:20160413] 改用 ResponseFile 回傳檔案
            #region Old
            //string contentType = GetContentType("TXT");
            //Byte[] fileContent = Encoding.Default.GetBytes(txtContent.ToString());

            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = contentType;
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            //Response.BinaryWrite(fileContent);
            //Response.Flush();
            //Response.End();
            #endregion

            Byte[] fileContent = Encoding.Default.GetBytes(txtContent.ToString());
            this.ResponseFile(fileName, fileContent);
            #endregion
        }

        protected StudentReturnView[] GetDatas()
        {
            StudentReturnView[] datas = null;

            Expression where = new Expression(StudentReturnView.Field.ReceiveType, this.QueryReceiveType)
                .And(StudentReturnView.Field.YearId, this.QueryYearId)
                .And(StudentReturnView.Field.TermId, this.QueryTermId)
                .And(StudentReturnView.Field.DepId, this.QueryDepId)
                .And(StudentReturnView.Field.ReceiveId, this.QueryReceiveId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(StudentReturnView.Field.SrNo, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<StudentReturnView>(this, where, orderbys, out datas);
            if (!result.IsSuccess)
            {
                this.ShowSystemMessage(string.Format("讀取退費檔錯誤，") + result.Message);
                return null;
            }

            return datas;
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

            #region 匯款日期
            {
                if (tbxDate.Text.Trim() == "")
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("請輸入匯款日期");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            return true;
        }
    }
}