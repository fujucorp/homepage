using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

using ICSharpCode.SharpZipLib.Zip;

namespace eSchoolWeb
{
    /// <summary>
    /// 操作模式常數定義抽象類別
    /// </summary>
    public abstract class ActionMode
    {
        public const string Query = "Q";
        /// <summary>
        /// 新增資料 : A
        /// </summary>
        public const string Insert = "A";

        /// <summary>
        /// 修改資料 : M
        /// </summary>
        public const string Modify = "M";

        /// <summary>
        /// 刪除資料 : D
        /// </summary>
        public const string Delete = "D";

        /// <summary>
        /// 檢視資料 : V
        /// </summary>
        public const string View = "V";

        ///// <summary>
        ///// 審核資料 : C
        ///// </summary>
        //public const string Approve = "C";

        /// <summary>
        /// 取得指定模式字串是否為定義的操作模式
        /// </summary>
        /// <param name="mode">指定模式字串</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string mode)
        {
            switch (mode)
            {
                case Query:
                case Insert:
                case Modify:
                case Delete:
                case View:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 取得指定模式字串是否為維護類(需要維護權限)的操作模式
        /// </summary>
        /// <param name="mode">指定模式字串</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsMaintinaMode(string mode)
        {
            switch (mode)
            {
                case Insert:
                case Modify:
                case Delete:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 取得指定模式字串是否為PKey可編輯的操作模式
        /// </summary>
        /// <param name="mode">指定模式字串</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsPKeyEditableMode(string mode)
        {
            switch (mode)
            {
                case Insert:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 取得指定模式字串是否為資料可編輯的操作模式
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool IsDataEditableMode(string mode)
        {
            switch (mode)
            {
                case Insert:
                case Modify:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 取得指定模式字串的 Localized
        /// </summary>
        /// <param name="mode">指定模式字串</param>
        /// <returns>傳回 Localized 或空字串</returns>
        public static string GetActionLocalized(string mode)
        {
            switch (mode)
            {
                case Query:
                    return WebHelper.GetLocalized("Action_Query", "查詢資料");
                case Insert:
                    return WebHelper.GetLocalized("Action_Insert", "新增資料");
                case Modify:
                    return WebHelper.GetLocalized("Action_Modify", "修改資料");
                case Delete:
                    return WebHelper.GetLocalized("Action_Delete", "刪除資料");
                case View:
                    return WebHelper.GetLocalized("Action_View", "檢視資料");
            }
            return String.Empty;
        }
    }

    /// <summary>
    /// (編輯用)對照檔項目設定承載類別
    /// </summary>
    [Serializable]
    public sealed class MappingItem
    {
        #region Property
        private string _Key = String.Empty;
        /// <summary>
        /// 項目的 Key
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Name = null;
        /// <summary>
        /// 項目的名稱
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value == null ? null : value.Trim();
            }
        }

        private string _FieldName = null;
        /// <summary>
        /// 項目的對照資料表欄位名稱
        /// </summary>
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 是否勾選
        /// </summary>
        public bool IsChecked
        {
            get;
            set;
        }

        private string _FileType = null;
        /// <summary>
        /// 檔案格式代碼
        /// </summary>
        public string FileType
        {
            get
            {
                return _FileType;
            }
            set
            {
                _FileType = value == null ? null : value.Trim().ToLower();
            }
        }

        private string _XlsName = null;
        /// <summary>
        /// Xls 格式的對照欄位名稱
        /// </summary>
        public string XlsName
        {
            get
            {
                return _XlsName;
            }
            set
            {
                _XlsName = value == null ? null : value.Trim();
            }
        }

        private int _XlsNameSize = 10;
        public int XlsNameSize
        {
            get
            {
                return _XlsNameSize;
            }
            set
            {
                _XlsNameSize = value < 10 ? 10 : value;
            }
        }

        private string _TxtStart = null;
        /// <summary>
        /// Txt 格式的字元起始位置 (1 開始)
        /// </summary>
        public string TxtStart
        {
            get
            {
                return _TxtStart;
            }
            set
            {
                _TxtStart = value == null ? null : value.Trim();
            }
        }

        private string _TxtLength = null;
        /// <summary>
        /// Txt 格式的字元長度
        /// </summary>
        public string TxtLength
        {
            get
            {
                return _TxtLength;
            }
            set
            {
                _TxtLength = value == null ? null : value.Trim();
            }
        }

        private int _MaxItemCount = 0;
        /// <summary>
        /// 最大項目的項數 (最小 2，否則為 0)
        /// </summary>
        public int MaxItemCount
        {
            get
            {
                return _MaxItemCount;
            }
            set
            {
                _MaxItemCount = value < 2 ? 0 : value;
            }
        }

        private string _ItemCount = null;
        /// <summary>
        /// 項目的項數
        /// </summary>
        public string ItemCount
        {
            get
            {
                return _ItemCount;
            }
            set
            {
                _ItemCount = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 英文資料欄位、英文資料對照欄位相關
        private string _EngFieldName = null;
        /// <summary>
        /// 英文資料的欄位名稱
        /// </summary>
        public string EngFieldName
        {
            get
            {
                return _EngFieldName;
            }
            set
            {
                _EngFieldName = value == null ? null : value.Trim();
            }
        }

        private int _EngFieldSize = 0;
        /// <summary>
        /// 英文資料的欄位長度
        /// </summary>
        public int EngFieldSize
        {
            get
            {
                return _EngFieldSize;
            }
            set
            {
                _EngFieldSize = value < 0 ? 0 : value;
            }
        }

        private string _EngFieldValue = null;
        /// <summary>
        /// 英文資料的欄位內容
        /// </summary>
        public string EngFieldValue
        {
            get
            {
                return _EngFieldValue;
            }
            set
            {
                _EngFieldValue = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// 建構(編輯用)對照檔項目設定承載類別
        /// </summary>
        private MappingItem()
        {

        }

        /// <summary>
        /// 建構(編輯用)對照檔項目設定承載類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="isChecked"></param>
        /// <param name="xlsName"></param>
        /// <param name="txtStart"></param>
        /// <param name="txtLength"></param>
        private MappingItem(string key, string name, string fieldName, bool isChecked, string fileType, string xlsName, string txtStart, string txtLength)
        {
            this.Key = key;
            this.Name = name;
            this.FieldName = fieldName;
            this.IsChecked = isChecked;
            this.FileType = fileType;
            this.XlsName = xlsName;
            this.TxtStart = txtStart;
            this.TxtLength = txtLength;

            this.MaxItemCount = 0;
            this.ItemCount = null;
        }

        /// <summary>
        /// 建構(編輯用)對照檔項目設定承載類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="isChecked"></param>
        /// <param name="itemCount"></param>
        private MappingItem(string key, string name, bool isChecked, int maxItemCount, string itemCount)
        {
            this.Key = key;
            this.Name = name;
            this.IsChecked = isChecked;
            this.MaxItemCount = maxItemCount;
            this.ItemCount = itemCount;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得 Txt 格式的字元起始位置的 int? 型別
        /// </summary>
        /// <returns>成功則傳回 int 型別，否則傳回 null</returns>
        public int? GetTxtStart()
        {
            int value = 0;
            if (Int32.TryParse(this.TxtStart, out value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// 取得 Txt 格式的字元長度的 int? 型別
        /// </summary>
        /// <returns>成功則傳回 int 型別，否則傳回 null</returns>
        public int? GetTxtLength()
        {
            int value = 0;
            if (Int32.TryParse(this.TxtLength, out value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// 取得項目數量的 int? 型別
        /// </summary>
        /// <returns>成功則傳回 int 型別，否則傳回 null</returns>
        public int? GetItemCount()
        {
            int value = 0;
            if (Int32.TryParse(this.ItemCount, out value))
            {
                return value;
            }
            return null;
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 建立 Xls 格式的(編輯用)對照檔項目設定承載類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="xlsName"></param>
        /// <returns></returns>
        public static MappingItem CreateByXls(string key, string name, string fieldName, string xlsName)
        {
            #region [MDY:2018xxxx] 收入科目金額的Excel欄位名稱允許空值，表示略過該項目 (null 才算未設定)
            #region [OLD]
            //bool isChecked = !String.IsNullOrWhiteSpace(xlsName);
            #endregion

            bool isChecked = xlsName != null;
            #endregion

            return new MappingItem(key, name, fieldName, isChecked, "xls", xlsName, null, null);
        }

        /// <summary>
        /// 建立 Txt 格式的(編輯用)對照檔項目設定承載類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="txtStart"></param>
        /// <param name="txtLength"></param>
        /// <returns></returns>
        public static MappingItem CreateByTxt(string key, string name, string fieldName, string txtStart, string txtLength)
        {
            bool isChecked = !String.IsNullOrWhiteSpace(txtStart);
            return new MappingItem(key, name, fieldName, isChecked, "txt", null, txtStart, txtLength);
        }

        /// <summary>
        /// 建立 Txt 格式的(編輯用)對照檔項目設定承載類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="txtStart"></param>
        /// <param name="txtLength"></param>
        /// <returns></returns>
        public static MappingItem CreateByTxt(string key, string name, string fieldName, int? txtStart, int? txtLength)
        {
            bool isChecked = txtStart != null;
            return new MappingItem(key, name, fieldName, isChecked, "txt", null, (txtStart == null ? null : txtStart.Value.ToString()), (txtLength == null ? null : txtLength.Value.ToString()));
        }

        /// <summary>
        /// 建立項目項數的(編輯用)對照檔項目設定承載類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="maxItemCount"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public static MappingItem CreateByItemCount(string key, string name, int maxItemCount, string itemCount)
        {
            bool isChecked = !String.IsNullOrWhiteSpace(itemCount);
            return new MappingItem(key, name, isChecked, maxItemCount, itemCount);
        }

        #region [MDY:20220808] 2022擴充案 英文資料欄位、英文資料對照欄位相關
        public static MappingItem CreateByXls(string key, string name, string fieldName, string xlsName, string engFieldName, string engFieldValue)
        {
            bool isChecked = xlsName != null;
            MappingItem item = new MappingItem(key, name, fieldName, isChecked, "data", xlsName, null, null);
            item.EngFieldName = engFieldName;
            item.EngFieldValue = engFieldValue;
            return item;
        }
        #endregion
        #endregion
    }

    public sealed class DownloadFileMethodCode
    {

        /// <summary>
        /// 繳費單模板管理代碼
        /// </summary>
        public const string BILLFORM = "BillForm";
        
        #region Constructor
        /// <summary>
        /// 建構類別
        /// </summary>
        private DownloadFileMethodCode()
        {

        }
        #endregion

    }

    /// <summary>
    /// Cache 處理工具類別
    /// </summary>
    public sealed class CacheHelper
    {
        #region 廣告相關
        /// <summary>
        /// 取得廣告圖檔的網址
        /// </summary>
        /// <param name="adId"></param>
        /// <returns></returns>
        public static string GetADImgUrl(string adId)
        {
            string imgUrl = null;
            CodeText data = AdCodeTexts.GetCodeText(adId);
            if (data != null)
            {
                Cache cache = HttpContext.Current.Cache;
                string imgUrlKey = String.Concat(data.Code, "_ImgUrl");
                imgUrl = cache.Get(imgUrlKey) as string;
                if (imgUrl == null)
                {
                    byte[] imgContent = null;
                    string linkUrl = null;
                    ReNewADCache(data, out imgUrl, out imgContent, out linkUrl);
                }
            }
            return imgUrl;
        }

        /// <summary>
        /// 取得廣告圖檔的內容
        /// </summary>
        /// <param name="adId"></param>
        /// <returns></returns>
        public static byte[] GetADImgContent(string adId)
        {
            byte[] imgContent = null;
            CodeText data = AdCodeTexts.GetCodeText(adId);
            if (data != null)
            {
                Cache cache = HttpContext.Current.Cache;
                string imgContentKey = String.Concat(data.Code, "_ImgContent");
                imgContent = cache.Get(imgContentKey) as byte[];
                if (imgContent == null)
                {
                    string imgUrl = null;
                    string linkUrl = null;
                    ReNewADCache(data, out imgUrl, out imgContent, out linkUrl);
                }
            }
            return imgContent;
        }

        /// <summary>
        /// 取得廣告連結的網址
        /// </summary>
        /// <param name="asId"></param>
        /// <returns></returns>
        public static string GetADLinkUrl(string adId)
        {
            string linkUrl = null;
            CodeText data = AdCodeTexts.GetCodeText(adId);
            if (data != null)
            {
                Cache cache = HttpContext.Current.Cache;
                string linkKey = String.Concat(data.Code, "_LinkUrl");
                linkUrl = cache.Get(linkKey) as string;
                if (linkUrl == null)
                {
                    string imgUrl = null;
                    byte[] imgContent = null;
                    ReNewADCache(data, out imgUrl, out imgContent, out linkUrl);
                }
            }
            return linkUrl;
        }

        /// <summary>
        /// 取得廣告資料
        /// </summary>
        /// <param name="adId"></param>
        /// <param name="imgUrl"></param>
        /// <param name="linkUrl"></param>
        public static void GetADData(string adId, out string imgUrl, out string linkUrl)
        {
            imgUrl = null;
            linkUrl = null;
            CodeText data = AdCodeTexts.GetCodeText(adId);
            if (data != null)
            {
                Cache cache = HttpContext.Current.Cache;
                string imgUrlKey = String.Concat(data.Code, "_ImgUrl");
                string linkUrlKey = String.Concat(data.Code, "_LinkUrl");
                imgUrl = cache.Get(imgUrlKey) as string;
                if (imgUrl == null)
                {
                    byte[] imgContent = null;
                    ReNewADCache(data, out imgUrl, out imgContent, out linkUrl);
                }
            }
        }

        private static void ReNewADCache(CodeText data, out string imgUrl, out byte[] imgContent, out string linkUrl)
        {
            imgUrl = null;
            imgContent = null;
            linkUrl = null;

            if (data != null)
            {
                AdEntity ad = null;
                Expression where = new Expression(AdEntity.Field.Id, data.Code);
                DataProxy.Current.SelectFirst<AdEntity>(null, where, null, out ad);
                if (ad != null)
                {
                    Cache cache = HttpContext.Current.Cache;
                    string imgUrlKey = String.Concat(data.Code, "_ImgUrl");
                    string imgContentKey = String.Concat(data.Code, "_ImgContent");
                    string linkUrlKey = String.Concat(data.Code, "_LinkUrl");
                    TimeSpan slidingExpiration = new TimeSpan(0, 10, 0);

                    if (ad.Kind == AdKindCodeTexts.URL)
                    {
                        imgUrl = ad.ImgUrl == null ? null : ad.ImgUrl.Trim();
                        if (!String.IsNullOrEmpty(imgUrl))
                        {
                            cache.Add(imgUrlKey, imgUrl, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                            cache.Remove(imgContentKey);
                        }
                    }
                    else
                    {
                        imgUrl = "~/api/AD.ashx?id=" + ad.Id;
                        imgContent = ad.ImgContent;
                        if (imgContent != null && imgContent.Length > 0)
                        {
                            cache.Add(imgUrlKey, imgUrl, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                            cache.Add(imgContentKey, imgContent, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                        }
                    }

                    linkUrl = ad.LinkUrl == null ? null : ad.LinkUrl.Trim();
                    if (String.IsNullOrEmpty(linkUrl))
                    {
                        linkUrl = "javascript:void(0);";
                    }
                    cache.Remove(linkUrlKey);
                    cache.Add(linkUrlKey, linkUrl, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }
            }
        }
        #endregion

        #region 學校選項相關
        #region [Old]
        ///// <summary>
        ///// 取得所有學校的選項資料
        ///// </summary>
        ///// <returns></returns>
        //public static CodeText[] GetSchoolItems(bool renew = false)
        //{
        //    Cache cache = HttpContext.Current.Cache;
        //    string cacheKey = "SCHOOL_ITEMS";
        //    CodeText[] datas = cache.Get(cacheKey) as CodeText[];
        //    if (datas == null || renew)
        //    {
        //        Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
        //        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
        //        //orderbys.Add(SchoolRTypeEntity.Field.CorpType, OrderByEnum.Asc);
        //        orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);
        //        //orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);

        //        string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
        //        string codeCombineFormat = null;
        //        string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
        //        string textCombineFormat = null;

        //        XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(null, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
        //        if (xmlResult.IsSuccess)
        //        {
        //            TimeSpan slidingExpiration = new TimeSpan(0, 10, 0);
        //            cache.Add(cacheKey, datas, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        //        }
        //    }
        //    return datas;
        //}
        #endregion

        /// <summary>
        /// 取得所有學校的 ConfigView 選項資料
        /// </summary>
        /// <param name="renew"></param>
        /// <returns></returns>
        public static SchoolConfigView[] GetSchoolConfigs(bool renew = false)
        {
            Cache cache = HttpContext.Current.Cache;
            string cacheKey = "SCHOOL_CONFIGS";
            SchoolConfigView[] datas = cache.Get(cacheKey) as SchoolConfigView[];
            if (datas == null || renew)
            {
                datas = null;
                Expression where = new Expression();
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(SchoolConfigView.Field.CorpType, OrderByEnum.Asc);
                orderbys.Add(SchoolConfigView.Field.SchIdenty, OrderByEnum.Asc);
                orderbys.Add(SchoolConfigView.Field.ReceiveType, OrderByEnum.Asc);

                SchoolConfigView[] some = null;
                XmlResult xmlResult = DataProxy.Current.SelectAll<SchoolConfigView>(null, where, orderbys, out some);
                if (xmlResult.IsSuccess)
                {
                    List<SchoolConfigView> list = null;
                    if (some != null && some.Length > 0)
                    {
                        list = new List<SchoolConfigView>(some.Length);
                        foreach (SchoolConfigView one in some)
                        {
                            if (list.Find(x => x.SchIdenty == one.SchIdenty) == null)
                            {
                                list.Add(one);
                            }
                        }
                        datas = list.ToArray();
                    }
                    TimeSpan slidingExpiration = new TimeSpan(0, 10, 0);
                    cache.Add(cacheKey, datas, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }
            }
            return datas;
        }
        #endregion
    }
}