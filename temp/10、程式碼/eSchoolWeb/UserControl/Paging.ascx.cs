using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    #region PagingInfo Class
    /// <summary>
    /// 分頁訊息類別
    /// </summary>
    [Serializable]
    public class PagingInfo
    {
        #region Const
        /// <summary>
        /// 不分頁的每頁筆數: 0
        /// </summary>
        public const int NoPageSize = 0;

        /// <summary>
        /// 預設的每頁筆數 : 10
        /// </summary>
        public const int DefaultPageSize = 10;
        #endregion

        #region Property
        private int _PageSize = DefaultPageSize;
        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value < 0 ? DefaultPageSize : value;
            }
        }

        private int _PageNo = 0;
        /// <summary>
        /// 頁號 (第幾頁)
        /// </summary>
        public int PageNo
        {
            get
            {
                return _PageNo;
            }
            set
            {
                _PageNo = value < 0 ? 0 : value;
            }
        }

        private int _MaxPageNo = 0;
        /// <summary>
        /// 最大頁號 (總頁數)
        /// </summary>
        public int MaxPageNo
        {
            get
            {
                return _MaxPageNo;
            }
            set
            {
                _MaxPageNo = value < 0 ? 0 : value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構分頁訊息類別
        /// </summary>
        public PagingInfo()
        {
        }

        /// <summary>
        /// 建構分頁訊息類別
        /// </summary>
        /// <param name="pageSize">指定每頁筆數</param>
        /// <param name="pageNo">指定頁號 (第幾頁)</param>
        /// <param name="maxPageNo">指定最大頁號 (總頁數)</param>
        public PagingInfo(int pageSize, int pageNo, int maxPageNo)
        {
            this.PageSize = pageSize;
            this.PageNo = pageNo;
            this.MaxPageNo = maxPageNo;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得讀取資料的起始索引值
        /// </summary>
        /// <returns>傳回起始索引值</returns>
        public int GetStartIndex()
        {
            if (this.MaxPageNo > 1 && this.PageNo > 1)
            {
                return (this.PageNo - 1) * this.PageSize;
            }
            return 0;
        }

        /// <summary>
        /// 取得是否有第一頁
        /// </summary>
        /// <returns>有則傳回 true，否則傳回 false</returns>
        public bool HasFirstPage()
        {
            return (this.PageSize != NoPageSize && this.MaxPageNo > 1 && this.PageNo > 1);
        }

        /// <summary>
        /// 取得是否有上一頁
        /// </summary>
        /// <returns>有則傳回 true，否則傳回 false</returns>
        public bool HasPreviousPage()
        {
            return (this.PageSize != NoPageSize && this.MaxPageNo > 1 && this.PageNo > 1);
        }

        /// <summary>
        /// 取得是否有下一頁
        /// </summary>
        /// <returns>有則傳回 true，否則傳回 false</returns>
        public bool HasNextPage()
        {
            return (this.PageSize != NoPageSize && this.MaxPageNo > 1 && this.PageNo < this.MaxPageNo);
        }

        /// <summary>
        /// 取得是否有最後頁
        /// </summary>
        /// <returns>有則傳回 true，否則傳回 false</returns>
        public bool HasLastPage()
        {
            return (this.PageSize != NoPageSize && this.MaxPageNo > 1 && this.PageNo < this.MaxPageNo);
        }

        public PagingInfo Calculate(string commandName, int commandArgument)
        {
            int totalRecord = this.MaxPageNo * this.PageSize;
            switch (commandName)
            {
                case PagingCommandName.ChangePageSize:
                    return Calculate(commandArgument, this.PageNo, totalRecord);
                case PagingCommandName.GoPageNo:
                case PagingCommandName.GoFirstPage:
                case PagingCommandName.GoPreviousPage:
                case PagingCommandName.GoNextPage:
                case PagingCommandName.GoLastPage:
                    return Calculate(this.PageSize, commandArgument, totalRecord);
                default:
                    return Calculate(this.PageSize, commandArgument, totalRecord);
            }
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 產生計算後的分頁訊息
        /// </summary>
        /// <param name="pageSize">指定每頁筆數</param>
        /// <param name="pageNo">指定目前頁號 (第幾頁)</param>
        /// <param name="totalRecord">指定要計算的總筆數</param>
        /// <returns>傳回分頁訊息</returns>
        public static PagingInfo Calculate(int pageSize, int pageNo, int totalRecord)
        {
            if (pageSize < 0)
            {
                pageSize = DefaultPageSize;
            }
            if (totalRecord < 1)
            {
                return new PagingInfo(pageSize, 0, 0);
            }
            if (pageNo < 1)
            {
                pageNo = 1;
            }

            if (pageSize == 0)
            {
                return new PagingInfo(pageSize, 1, 1);
            }

            int maxPageNo = Convert.ToInt32(Math.Ceiling((decimal)totalRecord / (decimal)pageSize));
            if (pageNo > maxPageNo)
            {
                return new PagingInfo(pageSize, maxPageNo, maxPageNo);
            }
            else
            {
                return new PagingInfo(pageSize, pageNo, maxPageNo);
            }
        }
        #endregion
    }
    #endregion

    #region Paging 的 Command Name 定義類別
    /// <summary>
    /// Paging 使用者控制項的命令名稱常數定義抽象類別
    /// </summary>
    public abstract class PagingCommandName
    {
        /// <summary>
        /// 改變每頁筆數
        /// </summary>
        public const string ChangePageSize = "ChangePageSize";

        /// <summary>
        /// 前往某頁面
        /// </summary>
        public const string GoPageNo = "GoPageNo";

        /// <summary>
        /// 前往第一頁
        /// </summary>
        public const string GoFirstPage = "GoFirstPage";

        /// <summary>
        /// 前往上一頁
        /// </summary>
        public const string GoPreviousPage = "GoPreviousPage";

        /// <summary>
        /// 前往最後頁
        /// </summary>
        public const string GoLastPage = "GoLastPage";

        /// <summary>
        /// 前往下一頁
        /// </summary>
        public const string GoNextPage = "GoNextPage";
    }
    #endregion

    #region Paging 的 EventArgs
    /// <summary>
    /// Paging 使用者控制項的事件資料類別
    /// </summary>
    public class PagingEventArgs : EventArgs
    {
        #region Property
        private string _CommandName = null;
        /// <summary>
        /// 命令名稱
        /// </summary>
        public string CommandName
        {
            get
            {
                return _CommandName;
            }
            set
            {
                _CommandName = value == null ? String.Empty : value.Trim();
            }
        }

        private int _CommandArgument = 0;
        /// <summary>
        /// 命令參數 (命令名稱為 ChangePageSize 時此參數為新的每頁筆數，否則為新的頁號)
        /// </summary>
        public int CommandArgument
        {
            get
            {
                return _CommandArgument;
            }
            set
            {
                _CommandArgument = value < 0 ? 0 : value;
            }
        }

        private PagingInfo _PagingInfo = null;
        /// <summary>
        /// 原始的分頁訊息
        /// </summary>
        public PagingInfo PagingInfo
        {
            get
            {
                return _PagingInfo;
            }
            set
            {
                _PagingInfo = value == null ? null : value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 Paging 使用者控制項的事件資料類別
        /// </summary>
        public PagingEventArgs()
        {
        }

        /// <summary>
        /// 建構 Paging 使用者控制項的事件資料類別
        /// </summary>
        /// <param name="menuID">選單(功能)代碼</param>
        /// <param name="menuName">選單(功能)名稱</param>
        /// <param name="menuUrl">選單(功能)網址</param>
        public PagingEventArgs(string commandName, int commandArgument, PagingInfo pagingInfo)
        {
            this.CommandName = commandName;
            this.CommandArgument = commandArgument;
            this.PagingInfo = pagingInfo;
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// 分頁資訊與翻頁按鈕的使用者控制項
    /// </summary>
    [DefaultProperty("PageNo")]
    [DefaultEvent("PagingChanged")]
    public partial class Paging : BaseUserControl
    {
        #region Const
        /// <summary>
        /// 儲存所有每頁筆數陣列的靜態唯讀變數
        /// </summary>
        public static readonly int[] AllPageSizes = new int[] { PagingInfo.DefaultPageSize, 20, 50, 100, PagingInfo.NoPageSize };
        #endregion

        #region Property
        /// <summary>
        /// 分頁訊息
        /// </summary>
        [Themeable(false)]
        [ReadOnly(true)]
        [Browsable(false)]
        public PagingInfo PagingInfo
        {
            get
            {
                return this.GetPagingInfo();
            }
        }

        private bool _NoPageSizeVisible = true;
        /// <summary>
        /// 取得或指定不分頁選項控制項是否顯示
        /// </summary>
        [DefaultValue(false)]
        [Themeable(false)]
        [Browsable(true)]
        public bool NoPageSizeVisible
        {
            get
            {
                return _NoPageSizeVisible;
            }
            set
            {
                _NoPageSizeVisible = value;
            }
        }
        #endregion

        #region 事件與處理常式
        /// <summary>
        /// 發生於改變分頁訊息相關動作時
        /// </summary>
        public event EventHandler<PagingEventArgs> PagingChanged = null;

        /// <summary>
        /// 引發改變分頁訊息的事件
        /// </summary>
        /// <param name="e">Change 事件資料</param>
        protected virtual void OnPagingChanged(PagingEventArgs e)
        {
            if (this.PagingChanged != null)
            {
                this.PagingChanged(this, e);
            }
        }
        #endregion

        #region Override BaseUserControl's Method
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.IsPostBack || !this.IsViewStateEnabled)
            {
                this.InitialUI();
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region 第幾頁、共幾頁、到第幾頁
            string ptnPageNo = this.GetLocalized("CTL_PageNo", "第<span>{0}</span>頁");
            this.labPageNo.Text = String.Format(ptnPageNo, 0);

            string ptnMaxPageNo = this.GetLocalized("CTL_MaxPageNo", "共<span>{0}</span>頁");
            this.labMaxPageNo.Text = String.Format(ptnMaxPageNo, 0);

            this.ddlGoPageNo.Items.Clear();
            this.ddlGoPageNo.Enabled = false;
            #endregion

            #region 每頁筆數
            this.ddlPageSize.Items.Clear();
            foreach (int pageSize in AllPageSizes)
            {
                ListItem item = null;
                string value = pageSize.ToString();
                if (pageSize == PagingInfo.NoPageSize)
                {
                    if (this.NoPageSizeVisible)
                    {
                        string text = this.GetLocalized("CTL_NoPageSize", "不分頁");
                        item = new ListItem(text, value);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    item = new ListItem(value, value);
                    item.Selected = (pageSize == PagingInfo.DefaultPageSize);
                }
                this.ddlPageSize.Items.Add(item);
            }
            this.ddlPageSize.Enabled = false;
            #endregion

            #region 最前頁、上一頁、下一頁、最後頁
            this.lbtnGoFirstPage.Text = this.GetLocalized("CTL_BTN_GoFirstPage", "最前頁");
            this.lbtnGoFirstPage.Enabled = false;

            this.lbtnGoPreviousPage.Text = this.GetLocalized("CTL_BTN_GoPreviousPage", "上一頁");
            this.lbtnGoPreviousPage.Enabled = false;

            this.lbtnGoNextPage.Text = this.GetLocalized("CTL_BTN_GoNextPage", "下一頁");
            this.lbtnGoNextPage.Enabled = false;

            this.lbtnGoLastPage.Text = this.GetLocalized("CTL_BTN_GoLastPage", "最後頁");
            this.lbtnGoLastPage.Enabled = false;
            #endregion
        }

        /// <summary>
        /// 取得分頁訊息
        /// </summary>
        /// <returns>傳回分頁訊息</returns>
        public PagingInfo GetPagingInfo()
        {
            object value = ViewState["PagingInfo"];
            if (value is PagingInfo)
            {
                return (PagingInfo)value;
            }
            return new PagingInfo();
        }

        /// <summary>
        /// 儲存分頁訊息
        /// </summary>
        /// <param name="pagingInfo">指定分頁訊息</param>
        protected void SetPagingInfo(PagingInfo pagingInfo)
        {
            ViewState["PagingInfo"] = pagingInfo;
        }

        /// <summary>
        /// 結繫分頁訊息資料
        /// </summary>
        /// <param name="pagingInfo"></param>
        public void BindData(PagingInfo pagingInfo)
        {
            if (pagingInfo == null)
            {
                pagingInfo = new PagingInfo(PagingInfo.DefaultPageSize, 0, 0);
            }
            this.SetPagingInfo(pagingInfo);

            #region Bind 第幾頁、共幾頁、到第幾頁
            {
                string ptnPageNo = this.GetLocalized("CTL_PageNo", "第<span>{0}</span>頁");
                this.labPageNo.Text = String.Format(ptnPageNo, pagingInfo.PageNo);

                string ptnMaxPageNo = this.GetLocalized("CTL_MaxPageNo", "共<span>{0}</span>頁");
                this.labMaxPageNo.Text = String.Format(ptnMaxPageNo, pagingInfo.MaxPageNo);

                this.ddlGoPageNo.Items.Clear();
                if (pagingInfo.MaxPageNo > 0)
                {
                    for (int no = 1; no <= pagingInfo.MaxPageNo; no++)
                    {
                        string txt = no.ToString();
                        ListItem item = new ListItem(txt, txt);
                        if (no == pagingInfo.PageNo)
                        {
                            item.Selected = true;
                        }
                        this.ddlGoPageNo.Items.Add(item);
                    }
                    this.ddlGoPageNo.Enabled = true;
                }
                else
                {
                    this.ddlGoPageNo.Enabled = false;
                }
            }
            #endregion

            #region Bind 每頁筆數
            {
                ListItem item = this.ddlPageSize.Items.FindByValue(pagingInfo.PageSize.ToString());
                if (item != null && item != this.ddlPageSize.SelectedItem)
                {
                    this.ddlPageSize.SelectedIndex = -1;
                    item.Selected = true;
                }
                this.ddlPageSize.Enabled = (pagingInfo.MaxPageNo > 0);
            }
            #endregion

            #region Bind 最前頁、上一頁、下一頁、最後頁
            if (pagingInfo.HasFirstPage())
            {
                this.lbtnGoFirstPage.CommandArgument = "1";
                this.lbtnGoFirstPage.Enabled = true;
            }
            else
            {
                this.lbtnGoFirstPage.CommandArgument = "0";
                this.lbtnGoFirstPage.Enabled = false;
            }

            if (pagingInfo.HasPreviousPage())
            {
                this.lbtnGoPreviousPage.CommandArgument = (pagingInfo.PageNo - 1).ToString();
                this.lbtnGoPreviousPage.Enabled = true;
            }
            else
            {
                this.lbtnGoPreviousPage.CommandArgument = "0";
                this.lbtnGoPreviousPage.Enabled = false;
            }

            if (pagingInfo.HasNextPage())
            {
                this.lbtnGoNextPage.CommandArgument = (pagingInfo.PageNo + 1).ToString();
                this.lbtnGoNextPage.Enabled = true;
            }
            else
            {
                this.lbtnGoNextPage.CommandArgument = "0";
                this.lbtnGoNextPage.Enabled = false;
            }

            if (pagingInfo.HasLastPage())
            {
                this.lbtnGoLastPage.CommandArgument = pagingInfo.MaxPageNo.ToString();
                this.lbtnGoLastPage.Enabled = true;
            }
            else
            {
                this.lbtnGoLastPage.CommandArgument = "0";
                this.lbtnGoLastPage.Enabled = false;
            }
            #endregion
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PagingInfo pagingInfo = this.GetPagingInfo();
            int pageSize = 0;
            if (this.ddlPageSize.SelectedIndex > -1 && Int32.TryParse(this.ddlPageSize.SelectedValue, out pageSize))
            {
                if (!Array.Exists(AllPageSizes, x => x == pageSize)
                    || (pageSize == PagingInfo.NoPageSize && !this.NoPageSizeVisible))
                {
                    pageSize = PagingInfo.DefaultPageSize;
                }
            }
            else
            {
                pageSize = PagingInfo.DefaultPageSize;
            }
            //if (this.ddlPageSize.SelectedIndex == -1
            //    || !Int32.TryParse(this.ddlPageSize.SelectedValue, out pageSize)
            //    || !Array.Exists(AllPageSizes, x => x == pageSize)
            //    || (pageSize == 0 && !this.NoPageSizeVisible))
            //{
            //    //[TODO] 要用 0 處理還是 ShowAlert ??
            //    pageSize = PagingInfo.DefaultPageSize;
            //}
            PagingEventArgs eArgs = new PagingEventArgs(PagingCommandName.ChangePageSize, pageSize, pagingInfo);
            this.OnPagingChanged(eArgs);
        }

        protected void ddlGoPageNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PagingInfo pagingInfo = this.GetPagingInfo();
            int pageNo = 0;
            if (this.ddlGoPageNo.SelectedIndex == -1
                || !Int32.TryParse(this.ddlGoPageNo.SelectedValue, out pageNo)
                || pageNo < 0)
            {
                //[TODO] 要用 0 處理還是 ShowAlert ??
                pageNo = 0;
            }
            else if (pageNo > pagingInfo.MaxPageNo)
            {
                pageNo = pagingInfo.MaxPageNo;
            }
            PagingEventArgs eArgs = new PagingEventArgs(PagingCommandName.GoPageNo, pageNo, pagingInfo);
            this.OnPagingChanged(eArgs);
        }

        protected void lbtnGoFirstPage_Click(object sender, EventArgs e)
        {
            PagingInfo pagingInfo = this.GetPagingInfo();
            if (pagingInfo.HasFirstPage())
            {
                int pageNo = 1;
                PagingEventArgs eArgs = new PagingEventArgs(PagingCommandName.GoFirstPage, pageNo, pagingInfo);
                this.OnPagingChanged(eArgs);
            }
            //[TODO] 要不要 ShowAlert ??
        }

        protected void lbtnGoPreviousPage_Click(object sender, EventArgs e)
        {
            PagingInfo pagingInfo = this.GetPagingInfo();
            if (pagingInfo.HasPreviousPage())
            {
                int pageNo = pagingInfo.PageNo - 1;
                PagingEventArgs eArgs = new PagingEventArgs(PagingCommandName.GoPreviousPage, pageNo, pagingInfo);
                this.OnPagingChanged(eArgs);
            }
            //[TODO] 要不要 ShowAlert ??
        }

        protected void lbtnGoNextPage_Click(object sender, EventArgs e)
        {
            PagingInfo pagingInfo = this.GetPagingInfo();
            if (pagingInfo.HasNextPage())
            {
                int pageNo = pagingInfo.PageNo + 1;
                PagingEventArgs eArgs = new PagingEventArgs(PagingCommandName.GoNextPage, pageNo, pagingInfo);
                this.OnPagingChanged(eArgs);
            }
            //[TODO] 要不要 ShowAlert ??
        }

        protected void lbtnGoLastPage_Click(object sender, EventArgs e)
        {
            PagingInfo pagingInfo = this.GetPagingInfo();
            if (pagingInfo.HasLastPage())
            {
                int pageNo = pagingInfo.MaxPageNo;
                PagingEventArgs eArgs = new PagingEventArgs(PagingCommandName.GoLastPage, pageNo, pagingInfo);
                this.OnPagingChanged(eArgs);
            }
            //[TODO] 要不要 ShowAlert ??
        }
    }
}