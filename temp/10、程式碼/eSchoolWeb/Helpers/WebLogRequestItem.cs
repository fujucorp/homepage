using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Fuju;
using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 網站日誌 Request 項目
    /// </summary>
    public sealed class WebLogRequestItem
    {
        #region Property
        /// <summary>
        /// Request 代碼 (功能代碼)
        /// </summary>
        public string RequestId
        {
            get;
            private set;
        }

        /// <summary>
        /// Request 名稱 (功能名稱)
        /// </summary>
        public string RequestName
        {
            get;
            private set;
        }

        /// <summary>
        /// Request 類別陣列
        /// </summary>
        public CodeText[] RequestKinds
        {
            get;
            private set;
        }

        /// <summary>
        /// 網頁參數類別陣列
        /// </summary>
        public string[] ArgumentKinds
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        public WebLogRequestItem(string requestId, string requestName, string[] requestKindCodes, string[] argumentKinds)
        {
            this.RequestId = requestId.Trim();
            this.RequestName = requestName.Trim();

            if (requestKindCodes == null || requestKindCodes.Length == 0)
            {
                this.RequestKinds = new CodeText[0];
            }
            else
            {
                WebLogRequestKindCodeTexts requestKinds = new WebLogRequestKindCodeTexts();
                List<CodeText> list = new List<CodeText>(requestKinds.Count);
                foreach (string code in requestKindCodes)
                {
                    if (!String.IsNullOrWhiteSpace(code))
                    {
                        CodeText item = requestKinds.Find(x => x.Code == code);
                        list.Add(item);
                    }
                }
                this.RequestKinds = list.ToArray();
            }

            this.ArgumentKinds = argumentKinds ?? new string[0];
        }
        #endregion
    }

    /// <summary>
    /// 網站日誌 Request 項目 定義清單集合類別
    /// </summary>
    public sealed class WebLogRequestList
    {
        #region Member
        private List<WebLogRequestItem> _All = null;
        #endregion

        #region Property
        /// <summary>
        /// 取得所有 網站日誌 Request 項目 陣列
        /// </summary>
        public WebLogRequestItem[] Items
        {
            get
            {
                return _All.ToArray();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 網站日誌 Request 項目 定義清單集合類別
        /// </summary>
        public WebLogRequestList()
        {
            _All = new List<WebLogRequestItem>(1);
            _All.Add(new WebLogRequestItem("check_bill_status", "查詢繳費狀態", new string[] { WebLogRequestKindCodeTexts.QUERY_CODE }, new string[] { "商家代號", "虛擬帳號" }));

            #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 增加 行動版超商條碼 項目
            _All.Add(new WebLogRequestItem("SMMBarcode", "行動版超商條碼", new string[] { WebLogRequestKindCodeTexts.VIEW_CODE }, new string[] { "商家代號", "虛擬帳號" }));
            #endregion
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得所有 Request 代碼名稱對照集合
        /// </summary>
        /// <returns></returns>
        public List<CodeText> GetAllIdNames()
        {
            List<CodeText> list = new List<CodeText>(_All.Count);
            foreach (WebLogRequestItem item in _All)
            {
                list.Add(new CodeText(item.RequestId, item.RequestName));
            }
            return list;
        }

        /// <summary>
        /// 找出指定 Request 代碼的網站日誌 Request 項目
        /// </summary>
        /// <param name="requestId">指定 Request 代碼</param>
        /// <returns>找到則傳回網站日誌 Request 項目，否則傳回 null</returns>
        public WebLogRequestItem Find(string requestId)
        {
            WebLogRequestItem item = null;
            if (!String.IsNullOrWhiteSpace(requestId))
            {
                item = _All.Find(x => x.RequestId == requestId);
            }
            return item;
        }
        #endregion
    }
}