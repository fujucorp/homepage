using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    public sealed class CancelNoHelper
    {
        #region Enum
        /// <summary>
        /// 年天數資料種類列舉
        /// </summary>
        public enum YearDayKind
        {
            /// <summary>
            /// 未定義
            /// </summary>
            Empty = 0,

            /// <summary>
            /// 公元年個位數 1 碼 + 該年第幾天 3 碼
            /// </summary>
            ADYearDay4 = 1,

            /// <summary>
            /// 民國年個位數 1 碼 + 該年第幾天 3 碼
            /// </summary>
            TWYearDay4 = 2
        }
        #endregion

        #region [MDY:20160303] 增加 M12C
        #region Const 14位虛擬帳號模組代號
        /// <summary>
        /// 12位虛擬帳號 無檢碼 模組代號 : M12C
        /// </summary>
        public const string Module12C = "M12C";
        #endregion
        #endregion

        #region Const 14位虛擬帳號模組代號
        /// <summary>
        /// 14位虛擬帳號 土銀檢碼 模組代號 : M14A
        /// </summary>
        public const string Module14A = "M14A";

        /// <summary>
        /// 14位虛擬帳號 廠商檢碼 模組代號 : M14B
        /// </summary>
        public const string Module14B = "M14B";

        /// <summary>
        /// 14位虛擬帳號 無檢碼 模組代號 : M14C
        /// </summary>
        public const string Module14C = "M14C";

        /// <summary>
        /// 14位虛擬帳號 檢核中心應繳資料 模組代號 : M14J
        /// </summary>
        public const string Module14J = "M14J";
        #endregion

        #region Const 16位虛擬帳號模組代號
        /// <summary>
        /// 16位虛擬帳號 土銀檢碼 模組代號 : M16A
        /// </summary>
        public const string Module16A = "M16A";

        /// <summary>
        /// 16位虛擬帳號 廠商檢碼 模組代號 : M16B
        /// </summary>
        public const string Module16B = "M16B";

        /// <summary>
        /// 16位虛擬帳號 無檢碼 模組代號 : M16C
        /// </summary>
        public const string Module16C = "M16C";

        #region 取消 Module16D、Module16E、Module16G
        ///// <summary>
        ///// 16位虛擬帳號 身分證號 模組代號 : M16D
        ///// </summary>
        //public const string Module16D = "M16D";

        ///// <summary>
        ///// 16位虛擬帳號 營利事業統一編號 模組代號 : M16E
        ///// </summary>
        //public const string Module16E = "M16E";

        ///// <summary>
        ///// 16位虛擬帳號 日期檢碼 模組代號 : M16G
        ///// </summary>
        //public const string Module16G = "M16G";
        #endregion

        /// <summary>
        /// 16位虛擬帳號 檢核中心應繳資料 模組代號 : M16J
        /// </summary>
        public const string Module16J = "M16J";
        #endregion

        #region Static Property
        /// <summary>
        /// 需要做 D38 處理的模組代碼清單
        /// </summary>
        public static readonly string[] D38ModuleIds = new string[] { Module14J, Module16J };

        #region [MDY:20190218] 虛擬帳號的客戶編號無規則的模組代碼清單
        /// <summary>
        /// 無規則且無檢碼的模組代碼清單
        /// </summary>
        public static readonly string[] UnrulyModuleIds = new string[] { Module14C, Module14J, Module16C, Module16J };
        #endregion
        #endregion

        #region Inner Class
        /// <summary>
        /// 虛擬帳號模組資訊類別
        /// </summary>
        public sealed class Module
        {
            #region [MDY:20160303] 增加 M12C
            #region Static Readonly Property 12位虛擬帳號模組
            /// <summary>
            /// 12位虛擬帳號 無檢碼 模組資訊 M12C
            /// </summary>
            public static readonly Module M12C = new Module(Module12C, "2 - 12碼無檢碼", 12, 5, 0, false);
            #endregion
            #endregion

            #region Static Readonly Property 14位虛擬帳號模組
            /// <summary>
            /// 14位虛擬帳號 土銀檢碼 模組資訊 M14A
            /// </summary>
            public static readonly Module M14A = new Module(Module14A, "0 - 14碼土銀規則", 14, 6, 1, false);
            /// <summary>
            /// 14位虛擬帳號 廠商檢碼 模組資訊 M14B
            /// </summary>
            public static readonly Module M14B = new Module(Module14B, "1 - 14碼廠商規則", 14, 6, 1, true);
            /// <summary>
            /// 14位虛擬帳號 無檢碼 模組資訊 M14C
            /// </summary>
            public static readonly Module M14C = new Module(Module14C, "2 - 14碼無檢碼", 14, 7, 0, false);

            #region 取消 Module14G
            ///// <summary>
            ///// 14位虛擬帳號 日期檢碼 模組資訊 M14G
            ///// </summary>
            //public static readonly Module M14G = new Module(Module14G, "6 - 14碼檢查日期", 14, 4, 2, true, YearDayKind.ADYearDay4);
            #endregion

            /// <summary>
            /// 位虛擬帳號 檢核中心應繳資料 模組代號 : M14J
            /// </summary>
            public static readonly Module M14J = new Module(Module14J, "9 - 14碼檢核中心", 14, 7, 0, true);
            #endregion

            #region Static Readonly Property 16位虛擬帳號模組
            /// <summary>
            /// 16位虛擬帳號 土銀檢碼 模組資訊 M16A
            /// </summary>
            public static readonly Module M16A = new Module(Module16A, "0 - 16碼土銀規則", 16, 8, 1, false);
            /// <summary>
            /// 16位虛擬帳號 廠商檢碼 模組資訊 M16B
            /// </summary>
            public static readonly Module M16B = new Module(Module16B, "1 - 16碼廠商規則", 16, 8, 1, true);
            /// <summary>
            /// 16位虛擬帳號 無檢碼 模組資訊 M16C
            /// </summary>
            public static readonly Module M16C = new Module(Module16C, "2 - 16碼無檢碼", 16, 9, 0, false);

            #region 取消 Module16D、Module16E、Module16G
            ///// <summary>
            ///// 16位虛擬帳號 身分證號 模組資訊 M16D
            ///// </summary>
            //public static readonly Module M16D = new Module(Module16D, "3 - 16碼檢查身分證字號", 16, 0, 0, false, YearDayKind.Empty, true);
            ///// <summary>
            ///// 16位虛擬帳號 營利事業統一編號 模組資訊 M16E
            ///// </summary>
            //public static readonly Module M16E = new Module(Module16E, "4 - 16碼檢查統一編號", 16, 0, 0, false, YearDayKind.Empty, false, true);

            ///// <summary>
            ///// 16位虛擬帳號 日期檢碼 模組資訊 M16G
            ///// </summary>
            //public static readonly Module M16G = new Module(Module16G, "6 - 16碼檢查日期", 16, 6, 2, true, YearDayKind.TWYearDay4);
            #endregion

            /// <summary>
            /// 位虛擬帳號 檢核中心應繳資料 模組代號 : M16J
            /// </summary>
            public static readonly Module M16J = new Module(Module16J, "9 - 16碼檢核中心", 16, 9, 0, true);
            #endregion

            #region Static Property
            /// <summary>
            /// 所有虛擬帳號模組資訊的快取靜態變數
            /// </summary>
            private static Module[] _CacheModules = null;
            #endregion

            #region Property
            private string _Id = null;
            /// <summary>
            /// 取得模組代號
            /// </summary>
            public string Id
            {
                get
                {
                    return _Id;
                }
                private set
                {
                    if (!String.IsNullOrWhiteSpace(value))
                    {
                        _Id = value.Trim();
                        _IsD38Kind = CancelNoHelper.IsD38ModuleId(this.Id);
                        return;
                    }
                    throw new NullReferenceException("Id can't be null Reference or empty");
                }
            }

            private string _Name = String.Empty;
            /// <summary>
            /// 取得模組名稱
            /// </summary>
            public string Name
            {
                get
                {
                    return _Name;
                }
                private set
                {
                    _Name = value == null ? String.Empty : value.Trim();
                }
            }

            /// <summary>
            /// 取得商家代號 Size
            /// </summary>
            public int ReceiveTypeSize
            {
                get
                {
                    return 4;
                }
            }

            private int _CancelNoSize = 0;
            /// <summary>
            /// 取得虛擬帳號 Size
            /// </summary>
            public int CancelNoSize
            {
                get
                {
                    return _CancelNoSize;
                }
                private set
                {
                    #region [MDY:20160303] 增加 12碼虛擬帳號
                    if (value == 14 || value == 16 || value == 12)
                    {
                        _CancelNoSize = value;
                        return;
                    }
                    throw new ArgumentOutOfRangeException("CancelNoSize must is 14 or 16 or 12");
                    #endregion
                }
            }

            private int _SeriorNoSize = 0;
            /// <summary>
            /// 取得流水號 Size (允許自由編號的長度)
            /// </summary>
            public int SeriorNoSize
            {
                get
                {
                    return _SeriorNoSize;
                }
                private set
                {
                    if (value >= 0 && value <= 9)
                    {
                        _SeriorNoSize = value;
                        return;
                    }
                    throw new ArgumentOutOfRangeException("SeriorNoSize must is 0 ~ 9");
                }
            }

            private int _ChecksumSize = 0;
            /// <summary>
            /// 取得檢查碼 Size
            /// </summary>
            public int ChecksumSize
            {
                get
                {
                    return _ChecksumSize;
                }
                private set
                {
                    if (value >= 0 && value <= 2)
                    {
                        _ChecksumSize = value;
                        return;
                    }
                    throw new ArgumentOutOfRangeException("ChecksumSize must is 0 ~ 2");
                }
            }

            private int? _CustomNoSize = null;
            /// <summary>
            /// 取得客戶編號 Size (去掉商家代號、檢查碼後的長度)
            /// </summary>
            public int CustomNoSize
            {
                get
                {
                    if (_CustomNoSize == null)
                    {
                        _CustomNoSize = this.CancelNoSize - this.ReceiveTypeSize - this.ChecksumSize;
                    }
                    return _CustomNoSize.Value;
                }
            }

            /// <summary>
            /// 取得是否有檢查碼
            /// </summary>
            public bool HasChecksum
            {
                get
                {
                    return _ChecksumSize > 0;
                }
            }

            /// <summary>
            /// 取得是否有流水號
            /// </summary>
            public bool HasSeriorNo
            {
                get
                {
                    return _SeriorNoSize > 0;
                }
            }

            private bool _IsNeedAmount = false;
            /// <summary>
            /// 取得是否需要金額資料
            /// </summary>
            public bool IsNeedAmount
            {
                get
                {
                    return _IsNeedAmount;
                }
                private set
                {
                    _IsNeedAmount = value;
                }
            }

            private YearDayKind _PayDueDateKind = YearDayKind.Empty;
            /// <summary>
            /// 取得繳款期限資料種類 (0=不含繳款期限，1=西元年1碼 + 該年第幾日3碼，2=民國年1碼 + 該年第幾日3碼)
            /// </summary>
            public YearDayKind PayDueDateKind
            {
                get
                {
                    return _PayDueDateKind;
                }
                private set
                {
                    _PayDueDateKind = value;
                }
            }

            private bool _IsHasPersonalId = false;
            /// <summary>
            /// 取得是否含身分證字號
            /// </summary>
            public bool IsHasPersonalId
            {
                get
                {
                    return _IsHasPersonalId;
                }
                private set
                {
                    _IsHasPersonalId = value;
                }
            }

            private bool _IsHasCorpId = false;
            /// <summary>
            /// 取得是否含營利事業統一編號
            /// </summary>
            public bool IsHasCorpId
            {
                get
                {
                    return _IsHasCorpId;
                }
                private set
                {
                    _IsHasCorpId = value;
                }
            }

            private bool? _IsD38Kind = null;
            /// <summary>
            /// 取得是否為需要做 D38 處理的模組
            /// </summary>
            /// <returns>是則傳回 true，否則傳回 false</returns>
            public bool IsD38Kind
            {
                get
                {
                    return _IsD38Kind ?? false;
                }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構虛擬帳號模組資訊類別
            /// </summary>
            /// <param name="id">模組代號</param>
            /// <param name="name">模組名稱</param>
            /// <param name="cancelNoSize">虛擬帳號 Size</param>
            /// <param name="seriorNoSize">流水號 Size</param>
            /// <param name="checksumSize">檢查碼 Size</param>
            /// <param name="isNeedAmount">是否需要金額資料</param>
            /// <param name="isHasPayDueDate">是否含繳款期限資料</param>
            /// <param name="isHasPersonalId">是否含身分證字號</param>
            /// <param name="isHasCorpId">是否含營利事業統一編號</param>
            private Module(string id, string name, int cancelNoSize, int seriorNoSize, int checksumSize, bool isNeedAmount
                , YearDayKind payDueDateKind = YearDayKind.Empty, bool isHasPersonalId = false, bool isHasCorpId = false)
            {
                this.Id = id;
                this.Name = name;
                this.CancelNoSize = cancelNoSize;
                this.SeriorNoSize = seriorNoSize;
                this.ChecksumSize = checksumSize;
                this.IsNeedAmount = isNeedAmount;
                this.PayDueDateKind = payDueDateKind;
                this.IsHasPersonalId = isHasPersonalId;
                this.IsHasCorpId = isHasCorpId;

                if (!this.IsReady())
                {
                    throw new ArgumentOutOfRangeException("設定值不合理");
                }
            }
            #endregion

            #region Method
            /// <summary>
            /// 取得指定流水號組成的客戶編號 (會檢查此模組是否可使用此方法取客戶編號)
            /// </summary>
            /// <param name="seriorNo">指定流水號</param>
            /// <returns>成功則傳回客戶編號，否則傳回 null</returns>
            internal string GenCustomNo(Int64 seriorNo)
            {
                int size = this.SeriorNoSize;
                if (size > 0 && seriorNo > 0 && this.PayDueDateKind == YearDayKind.Empty && !this.IsHasPersonalId && !this.IsHasCorpId)
                {
                    string txt = seriorNo.ToString();
                    if (txt.Length < size)
                    {
                        return txt.PadLeft(size, '0');
                    }
                    else if (txt.Length == size)
                    {
                        return txt;
                    }
                }
                return null;
            }

            /// <summary>
            /// 取得指定流水號組成的客戶編號 (會檢查此模組是否可使用此方法取客戶編號)
            /// </summary>
            /// <param name="seriorNo">指定流水號</param>
            /// <returns>成功則傳回客戶編號，否則傳回 null</returns>
            internal string GenCustomNo(string seriorNo)
            {
                if (seriorNo != null)
                {
                    Int64 sno = 0;
                    if (Int64.TryParse(seriorNo.Trim(), out sno))
                    {
                        return this.GenCustomNo(sno);
                    }
                }
                return null;
            }

            /// <summary>
            /// 取得指定學年代碼、學期代碼、收費用別代碼、流水號組成的客戶編號 (會檢查此模組是否可使用此方法取客戶編號)
            /// </summary>
            /// <param name="yearId">指定學年代碼</param>
            /// <param name="termId">指定學期代碼</param>
            /// <param name="receiveId">指定代收費用別代碼</param>
            /// <param name="seriorNo">指定流水號</param>
            /// <param name="isBigReceiveId">指定是否使用兩碼費用別代碼</param>
            /// <returns>成功則傳回客戶編號，否則傳回 null</returns>
            internal string GenCustomNo(string yearId, string termId, string receiveId, Int64 seriorNo, bool isBigReceiveId)
            {
                int size = this.SeriorNoSize;
                if (size > 0 && seriorNo > 0
                    && yearId != null && yearId.Length > 0
                    && termId != null && termId.Length > 0
                    && receiveId != null && receiveId.Length > 0
                    && this.PayDueDateKind == YearDayKind.Empty && !this.IsHasPersonalId && !this.IsHasCorpId)
                {
                    string txt = seriorNo.ToString();

                    #region [Old] 如果費用別代碼使用兩碼 (不足兩碼則前補 0)，第一碼放在流水號的第一碼
                    //if (txt.Length < size)
                    //{
                    //    return String.Concat(yearId.Substring(yearId.Length - 1), termId.Substring(termId.Length - 1), receiveId.Substring(receiveId.Length - 1), txt.PadLeft(size, '0'));
                    //}
                    //else if (txt.Length == size)
                    //{
                    //    return String.Concat(yearId.Substring(yearId.Length - 1), termId.Substring(termId.Length - 1), receiveId.Substring(receiveId.Length - 1), txt);
                    //}
                    #endregion

                    #region [New] 如果費用別代碼使用兩碼 (不足兩碼則前補 0)，第一碼放在流水號的第一碼
                    if (isBigReceiveId)
                    {
                        if (receiveId.Length == 1)
                        {
                            receiveId = "0" + receiveId;
                        }
                        size -= 1;
                        if (txt.Length < size)
                        {
                            return String.Concat(yearId.Substring(yearId.Length - 1), termId.Substring(termId.Length - 1), receiveId.Substring(receiveId.Length - 1), receiveId.Substring(receiveId.Length - 2, 1), txt.PadLeft(size, '0'));
                        }
                        else if (txt.Length == size)
                        {
                            return String.Concat(yearId.Substring(yearId.Length - 1), termId.Substring(termId.Length - 1), receiveId.Substring(receiveId.Length - 1), receiveId.Substring(receiveId.Length - 2, 1), txt);
                        }
                    }
                    else
                    {
                        if (txt.Length < size)
                        {
                            return String.Concat(yearId.Substring(yearId.Length - 1), termId.Substring(termId.Length - 1), receiveId.Substring(receiveId.Length - 1), txt.PadLeft(size, '0'));
                        }
                        else if (txt.Length == size)
                        {
                            return String.Concat(yearId.Substring(yearId.Length - 1), termId.Substring(termId.Length - 1), receiveId.Substring(receiveId.Length - 1), txt);
                        }
                    }
                    #endregion
                }
                return null;
            }

            /// <summary>
            /// 取得指定學年代碼、學期代碼、收費用別代碼、流水號組成的客戶編號 (會檢查此模組是否可使用此方法取客戶編號)
            /// </summary>
            /// <param name="yearId">指定學年代碼</param>
            /// <param name="termId">指定學期代碼</param>
            /// <param name="receiveId">指定代收費用別代碼</param>
            /// <param name="seriorNo">指定流水號</param>
            /// <param name="isBigReceiveId">指定是否使用兩碼費用別代碼</param>
            /// <returns>成功則傳回客戶編號，否則傳回 null</returns>
            internal string GenCustomNo(string yearId, string termId, string receiveId, string seriorNo, bool isBigReceiveId)
            {
                if (seriorNo != null)
                {
                    Int64 sno = 0;
                    if (Int64.TryParse(seriorNo.Trim(), out sno))
                    {
                        return this.GenCustomNo(yearId, termId, receiveId, sno, isBigReceiveId);
                    }
                }
                return null;
            }

            #region [Old] 取消含繳款期限、身份證、統編的模組
            ///// <summary>
            ///// 取得指定流水號與繳款期限組成的客戶編號 (會檢查此模組是否可使用此方法取客戶編號)
            ///// </summary>
            ///// <param name="seriorNo">指定流水號</param>
            ///// <param name="payDueDate">指定繳款期限</param>
            ///// <returns>成功則傳回客戶編號，否則傳回 null</returns>
            //internal string GenCustomNo(Int64 seriorNo, DateTime payDueDate)
            //{
            //    string payDueDay = GetYearDay(payDueDate, this.PayDueDateKind);

            //    int size = this.SeriorNoSize;
            //    if (size > 0 && seriorNo > 0 && payDueDay != null && !this.IsHasPersonalId && !this.IsHasCorpId)
            //    {
            //        string txt = seriorNo.ToString();
            //        if (txt.Length < size)
            //        {
            //            return String.Concat(payDueDay, txt.PadLeft(size, '0'));
            //        }
            //        else if (txt.Length == size)
            //        {
            //            return String.Concat(payDueDay, txt);
            //        }
            //    }
            //    return null;
            //}

            ///// <summary>
            ///// 取得指定流水號與繳款期限組成的客戶編號 (會檢查此模組是否可使用此方法取客戶編號)
            ///// </summary>
            ///// <param name="seriorNo">指定流水號</param>
            ///// <param name="payDueDate">指定繳款期限 (僅允許7碼民國年月日或8碼西元年月日的日期格式字串)</param>
            ///// <returns>成功則傳回客戶編號，否則傳回 null</returns>
            //internal string GenCustomNo(string seriorNo, string payDueDate)
            //{
            //    Int64 sno = 0;
            //    DateTime? date = null;
            //    if (Int64.TryParse(seriorNo, out sno) && (date = DataFormat.ConvertDateText(payDueDate)) != null)
            //    {
            //        return this.GenCustomNo(sno, date.Value);
            //    }
            //    return null;
            //}

            ///// <summary>
            ///// 取得身份證字號轉成客戶編號 (會檢查此模組是否可使用此方法取客戶編號)
            ///// </summary>
            ///// <param name="pId">指定身份證字號</param>
            ///// <returns>成功則傳回客戶編號，否則傳回 null</returns>
            //internal string GenCustomNoByPId(string pId)
            //{
            //    if (this.IsHasPersonalId && String.IsNullOrWhiteSpace(pId) && this.SeriorNoSize == 0 && this.PayDueDateKind == YearDayKind.Empty && !this.IsHasCorpId)
            //    {
            //        pId = pId.Trim().ToUpper();
            //        if (Common.IsPersonalID(pId))
            //        {
            //            string engChars = "ABCDEFGHIJKLMNOPQRSUTVWXYZ";
            //            List<string> codes = new List<string>(11);
            //            foreach (char chr in pId.ToCharArray())
            //            {
            //                int idx = engChars.IndexOf(chr);
            //                if (idx == -1)
            //                {
            //                    codes.Add(chr.ToString());
            //                }
            //                else
            //                {
            //                    codes.Add((idx + 1).ToString("00"));
            //                }
            //            }
            //            string customNo = String.Join("", codes.ToArray());
            //            int customNoSize = this.CustomNoSize;
            //            if (customNo.Length < customNoSize)
            //            {
            //                return customNo.PadLeft(customNoSize, '0');
            //            }
            //            else if (customNo.Length == customNoSize)
            //            {
            //                return customNo;
            //            }
            //        }
            //    }
            //    return null;
            //}

            ///// <summary>
            ///// 取得營利事業統一編號轉成客戶編號 (會檢查此模組是否可使用此方法取客戶編號)
            ///// </summary>
            ///// <param name="corpId">指定營利事業統一編號</param>
            ///// <returns>成功則傳回客戶編號，否則傳回 null</returns>
            //internal string GenCustomNoByCorpId(string corpId)
            //{
            //    if (this.IsHasCorpId && String.IsNullOrWhiteSpace(corpId) && this.SeriorNoSize == 0 && this.PayDueDateKind == YearDayKind.Empty && !this.IsHasPersonalId)
            //    {
            //        corpId = corpId.Trim();
            //        if (Common.IsNumber(corpId, 8))
            //        {
            //            int customNoSize = this.CustomNoSize;
            //            if (corpId.Length < customNoSize)
            //            {
            //                return corpId.PadLeft(customNoSize, '0');
            //            }
            //            else if (corpId.Length == customNoSize)
            //            {
            //                return corpId;
            //            }
            //        }
            //    }
            //    return null;
            //}
            #endregion

            #region [Old] 取消含繳款期限、身份證、統編的模組
            ///// <summary>
            ///// 嘗試拆解指定虛擬帳號的組成 (不驗證檢查碼)
            ///// </summary>
            ///// <param name="cancelNo">指定虛擬帳號</param>
            ///// <param name="receiveType">成功則傳回商家代號，否則傳回 null</param>
            ///// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
            ///// <param name="checksum">成功則傳回檢查碼，否則傳回 null，沒有檢查碼則傳回空字串</param>
            ///// <param name="seriorNo">傳回流水號，無流水號則傳回 null</param>
            ///// <param name="payDueDay">傳回繳款期限 (YearDay 格式)，無繳款期限則傳回 null</param>
            ///// <param name="pcId">傳回身分證字號或營利事業統一編號，無則傳回 null</param>
            ///// <returns>成功則傳回 true，否則傳回 false</returns>
            //public bool TryParseCancelNo(string cancelNo, out string receiveType, out string customNo, out string checksum, out string seriorNo, out string payDueDay, out string pcId)
            //{
            //    cancelNo = cancelNo == null ? String.Empty : cancelNo.Trim();
            //    if (Common.IsNumber(cancelNo, this.CancelNoSize))
            //    {
            //        receiveType = cancelNo.Substring(0, this.ReceiveTypeSize);

            //        if (this.ChecksumSize > 0)
            //        {
            //            customNo = cancelNo.Substring(this.ReceiveTypeSize, this.CancelNoSize - this.ReceiveTypeSize - this.ChecksumSize);
            //            checksum = cancelNo.Substring(this.CancelNoSize - this.ChecksumSize);
            //        }
            //        else
            //        {
            //            customNo = cancelNo.Substring(this.ReceiveTypeSize);
            //            checksum = String.Empty;
            //        }

            //        return this.TryParseCustomNo(customNo, out seriorNo, out payDueDay, out pcId);
            //    }

            //    receiveType = null;
            //    customNo = null;
            //    checksum = null;
            //    seriorNo = null;
            //    payDueDay = null;
            //    pcId = null;
            //    return false;
            //}

            ///// <summary>
            ///// 嘗試拆解指定客戶編號的組成
            ///// </summary>
            ///// <param name="checksum">成功則傳回檢查碼，否則傳回 null，沒有檢查碼則傳回空字串</param>
            ///// <param name="seriorNo">傳回流水號，無流水號則傳回 null</param>
            ///// <param name="payDueDay">傳回繳款期限 (YearDay 格式)，無繳款期限則傳回 null</param>
            ///// <param name="pcId">傳回身分證字號或營利事業統一編號，無則傳回 null</param>
            ///// <returns>成功則傳回 true，否則傳回 false</returns>
            //public bool TryParseCustomNo(string customNo, out string seriorNo, out string payDueDay, out string pcId)
            //{
            //    customNo = customNo == null ? String.Empty : customNo.Trim();
            //    if (Common.IsNumber(customNo, this.CustomNoSize))
            //    {
            //        if (this.IsHasPersonalId && customNo.Length >= 11)
            //        {
            //            pcId = customNo.Substring(customNo.Length - 11);
            //            seriorNo = null;
            //            payDueDay = null;
            //        }
            //        else if (this.IsHasCorpId && customNo.Length >= 8)
            //        {
            //            pcId = customNo.Substring(customNo.Length - 8);
            //            seriorNo = null;
            //            payDueDay = null;
            //        }
            //        else if (this.PayDueDateKind == YearDayKind.ADYearDay4 && customNo.Length > 4)
            //        {
            //            pcId = null;
            //            payDueDay = customNo.Substring(0, customNo.Length - 4);
            //            seriorNo = customNo.Substring(customNo.Length - 4);
            //        }
            //        else if (this.PayDueDateKind == YearDayKind.TWYearDay4 && customNo.Length > 4)
            //        {
            //            pcId = null;
            //            payDueDay = customNo.Substring(0, 4);
            //            seriorNo = customNo.Substring(4);
            //        }
            //        else
            //        {
            //            pcId = null;
            //            payDueDay = null;
            //            seriorNo = customNo;
            //        }
            //        return true;
            //    }

            //    seriorNo = null;
            //    payDueDay = null;
            //    pcId = null;
            //    return false;
            //}
            #endregion

            /// <summary>
            /// 嘗試拆解指定虛擬帳號的組成 (不驗證檢查碼)
            /// </summary>
            /// <param name="cancelNo">指定虛擬帳號</param>
            /// <param name="isBigReceiveId">指定是否使用兩碼費用別代碼</param>
            /// <param name="receiveType">成功則傳回商家代號，否則傳回 null</param>
            /// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
            /// <param name="checksum">成功則傳回檢查碼，否則傳回 null，沒有檢查碼則傳回空字串</param>
            /// <param name="yearId">成功則傳回學年代碼，否則傳回 null</param>
            /// <param name="termId">成功則傳回學期代碼，否則傳回 null</param>
            /// <param name="receiveId">成功則傳回代收費用別代碼，否則傳回 null</param>
            /// <param name="seriorNo">成功則傳回流水號，無流水號則傳回 null</param>
            /// <returns>成功則傳回 true，否則傳回 false</returns>
            public bool TryParseCancelNo(string cancelNo, bool isBigReceiveId, out string receiveType, out string customNo, out string checksum, out string yearId, out string termId, out string receiveId, out string seriorNo)
            {
                cancelNo = cancelNo == null ? String.Empty : cancelNo.Trim();
                if (Common.IsNumber(cancelNo, this.CancelNoSize))
                {
                    receiveType = cancelNo.Substring(0, this.ReceiveTypeSize);

                    if (this.ChecksumSize > 0)
                    {
                        customNo = cancelNo.Substring(this.ReceiveTypeSize, this.CancelNoSize - this.ReceiveTypeSize - this.ChecksumSize);
                        checksum = cancelNo.Substring(this.CancelNoSize - this.ChecksumSize);
                    }
                    else
                    {
                        customNo = cancelNo.Substring(this.ReceiveTypeSize);
                        checksum = String.Empty;
                    }

                    #region [Old] 改用 TryParseCustomNo 處理
                    //yearId = customNo.Substring(0, 1);
                    //termId = customNo.Substring(1, 1);
                    //receiveId = customNo.Substring(2, 1);
                    //seriorNo = customNo.Substring(3);

                    //return true;
                    #endregion

                    return this.TryParseCustomNo(customNo, isBigReceiveId, out yearId, out termId, out receiveId, out seriorNo);
                }

                receiveType = null;
                customNo = null;
                checksum = null;
                yearId = null;
                termId = null;
                receiveId = null;
                seriorNo = null;
                return false;
            }

            /// <summary>
            /// 嘗試拆解指定虛擬帳號的組成 (不驗證檢查碼)
            /// </summary>
            /// <param name="cancelNo">指定虛擬帳號</param>
            /// <param name="receiveType">成功則傳回商家代號，否則傳回 null</param>
            /// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
            /// <param name="checksum">成功則傳回檢查碼，否則傳回 null，沒有檢查碼則傳回空字串</param>
            /// <returns>成功則傳回 true，否則傳回 false</returns>
            public bool TryParseCancelNo(string cancelNo, out string receiveType, out string customNo, out string checksum)
            {
                cancelNo = cancelNo == null ? String.Empty : cancelNo.Trim();
                if (Common.IsNumber(cancelNo, this.CancelNoSize))
                {
                    receiveType = cancelNo.Substring(0, this.ReceiveTypeSize);

                    if (this.ChecksumSize > 0)
                    {
                        customNo = cancelNo.Substring(this.ReceiveTypeSize, this.CancelNoSize - this.ReceiveTypeSize - this.ChecksumSize);
                        checksum = cancelNo.Substring(this.CancelNoSize - this.ChecksumSize);
                    }
                    else
                    {
                        customNo = cancelNo.Substring(this.ReceiveTypeSize);
                        checksum = String.Empty;
                    }
                    return true;
                }

                receiveType = null;
                customNo = null;
                checksum = null;
                return false;
            }


            /// <summary>
            /// 嘗試拆解指定客戶編號的組成
            /// </summary>
            /// <param name="customNo">指定客戶編號</param>
            /// <param name="isBigReceiveId">指定是否使用兩碼費用別代碼</param>
            /// <param name="yearId">成功則傳回學年代碼，否則傳回 null</param>
            /// <param name="termId">成功則傳回學期代碼，否則傳回 null</param>
            /// <param name="receiveId">成功則傳回費用別代碼，否敗則傳回 null</param>
            /// <param name="seriorNo">成功則傳回流水號，否則傳回 null</param>
            /// <returns>成功則傳回 true，否則傳回 false</returns>
            public bool TryParseCustomNo(string customNo, bool isBigReceiveId, out string yearId, out string termId, out string receiveId, out string seriorNo)
            {
                customNo = customNo == null ? String.Empty : customNo.Trim();
                if (Common.IsNumber(customNo, this.CustomNoSize))
                {
                    yearId = customNo.Substring(0, 1);
                    termId = customNo.Substring(1, 1);

                    #region [Old] 如果費用別代碼使用兩碼，第一碼放在流水號的第一碼
                    //receiveId = customNo.Substring(2, 1);
                    //seriorNo = customNo.Substring(3);
                    #endregion

                    #region [New] 如果費用別代碼使用兩碼，第一碼放在流水號的第一碼
                    if (isBigReceiveId)
                    {
                        //使用兩碼的費用別代碼，表示流水號的第一碼為費用別代碼的第一碼
                        receiveId = customNo.Substring(3, 1) + customNo.Substring(2, 1);
                        seriorNo = customNo.Substring(4);
                    }
                    else
                    {
                        receiveId = customNo.Substring(2, 1);
                        seriorNo = customNo.Substring(3);
                    }
                    #endregion

                    return true;
                }

                yearId = null;
                termId = null;
                receiveId = null;
                seriorNo = null;
                return false;
            }

            private Int64? _MaxSeriorNo = null;
            /// <summary>
            /// 取得最大流水號
            /// </summary>
            /// <param name="isBigReceiveId"></param>
            /// <returns></returns>
            public Int64 GetMaxSeriorNo(bool isBigReceiveId)
            {
                if (this.SeriorNoSize < 1 || this.SeriorNoSize > 9)
                {
                    return 0;
                }
                if (_MaxSeriorNo == null)
                {
                    int size = isBigReceiveId ? this.SeriorNoSize - 1 : this.SeriorNoSize;
                    _MaxSeriorNo = Int64.Parse("999999999".Substring(0, size));
                }
                return _MaxSeriorNo.Value;
            }

            /// <summary>
            /// 取得設定值是否 Ready
            /// </summary>
            /// <returns></returns>
            private bool IsReady()
            {
                if (!String.IsNullOrEmpty(_Id) && this.CancelNoSize > 0)
                {
                    if (this.IsHasPersonalId)
                    {
                        return (!this.HasSeriorNo && !this.HasChecksum && this.PayDueDateKind == YearDayKind.Empty  && !this.IsHasCorpId);
                    }
                    if (this.IsHasCorpId)
                    {
                        return (!this.HasSeriorNo && !this.HasChecksum && this.PayDueDateKind == YearDayKind.Empty && !this.IsHasPersonalId);
                    }
                    else
                    {
                        if (this.HasSeriorNo)
                        {
                            if (this.PayDueDateKind == YearDayKind.Empty)
                            {
                                return (this.CustomNoSize == this.SeriorNoSize + 3);
                            }
                            else
                            {
                                return (this.CustomNoSize == this.SeriorNoSize + 4 + 3);
                            }
                        }
                    }
                }
                return false;
            }
            #endregion

            #region Static Method
            /// <summary>
            /// 取得所有虛擬帳號模組資訊的陣列
            /// </summary>
            /// <returns>傳回所有虛擬帳號模組資訊的陣列</returns>
            public static Module[] GetModules()
            {
                if (_CacheModules == null)
                {
                    #region 取消 Module14G & Module16D & Module16E & Module16G
                    //_CacheModules = new Module[] {
                    //    M14A, M14B, M14C, M14G,
                    //    M16A, M16B, M16C, M16D, M16E, M16G
                    //};
                    #endregion

                    #region [MDY:20160303] 增加 M12C
                    #region [Old]
                    ////新增 Module14J 與 Module16J
                    //_CacheModules = new Module[] {
                    //    M14A, M14B, M14C, M14J,
                    //    M16A, M16B, M16C, M16J
                    //};
                    #endregion

                    _CacheModules = new Module[] {
                        M14A, M14B, M14C, M14J,
                        M16A, M16B, M16C, M16J,
                        M12C
                    };
                    #endregion
                }
                return _CacheModules;
            }

            /// <summary>
            /// 取得指定模組代碼的虛擬帳號模組資訊
            /// </summary>
            /// <param name="id">指定模組代碼</param>
            /// <returns>找到則傳回虛擬帳號模組資訊，否則傳回 null</returns>
            public static Module GetById(string id)
            {
                if (!String.IsNullOrWhiteSpace(id))
                {
                    id = id.Trim();
                    Module[] modules = GetModules();
                    foreach (Module module in modules)
                    {
                        if (module.Id == id)
                        {
                            return module;
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// 取得所有虛擬帳號模組資訊的代碼名稱陣列
            /// </summary>
            /// <returns>傳回所有虛擬帳號模組資訊的代碼名稱陣列</returns>
            public static CodeText[] GetIdNames()
            {
                Module[] modules = GetModules();
                if (modules == null || modules.Length == 0)
                {
                    return new CodeText[0];
                }
                CodeText[] datas = new  CodeText[modules.Length];
                for(int idx = 0; idx < modules.Length; idx++)
                {
                    Module module = modules[idx];
                    datas[idx] = new CodeText(module.Id, module.Name);
                }
                return datas;
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 取得指定日期的指定年天數資料種類的字串
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <param name="kind">指定年天數資料種類</param>
        /// <returns></returns>
        public static string GetYearDay(DateTime date, YearDayKind kind)
        {
            switch (kind)
            {
                case YearDayKind.ADYearDay4:
                    return String.Format("{0}{1:000}", date.Year % 10, date.DayOfYear);
                case YearDayKind.TWYearDay4:
                    return String.Format("{0}{1:000}", (date.Year - 1911) % 10, date.DayOfYear);
            }
            return null;
        }

        #region 取得客戶編號 相關方法
        #region [Old] 取消 G 類
        ///// <summary>
        ///// 嘗試由指定的虛擬帳號模組將商家代號、流水號、繳款期限、身分證字號組成客戶編號
        ///// </summary>
        ///// <param name="module">指定虛擬帳號模組</param>
        ///// <param name="checkReceiveType">指定是否檢查商家代號</param>
        ///// <param name="receiveType">指定商家代號</param>
        ///// <param name="seriorNo">指定流水號</param>
        ///// <param name="payDueDate">指定繳款期限</param>
        ///// <param name="personalId">指定身分證字號</param>
        ///// <param name="corpId">指定營利事業統一編號</param>
        ///// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
        ///// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        //public string TryGetCustomNo(Module module, string receiveType, Int64? seriorNo, DateTime? payDueDate, string personalId, string corpId, out string customNo)
        //{
        //    customNo = null;

        //    if (module == null)
        //    {
        //        return "缺少虛擬帳號模組參數";
        //    }

        //    if (module.IsHasPersonalId)
        //    {
        //        #region 須含身分證字號資料
        //        personalId = personalId == null ? String.Empty : personalId.Trim();
        //        if (personalId.Length != 10)
        //        {
        //            return "缺少身分證字號或資料不正確";
        //        }
        //        customNo = module.GenCustomNoByPId(personalId);
        //        if (String.IsNullOrEmpty(customNo))
        //        {
        //            return "缺少身分證字號或資料不正確";
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //        #endregion
        //    }

        //    if (module.IsHasCorpId)
        //    {
        //        #region 須含營利事業統一編號資料
        //        corpId = corpId == null ? String.Empty : corpId.Trim();
        //        if (corpId.Length != 8)
        //        {
        //            return "缺少營利事業統一編號或資料不正確";
        //        }
        //        customNo = module.GenCustomNoByCorpId(corpId);
        //        if (String.IsNullOrEmpty(customNo))
        //        {
        //            return "缺少營利事業統一編號或資料不正確";
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //        #endregion
        //    }

        //    if (module.SeriorNoSize > 0)
        //    {
        //        #region 須含流水號資料
        //        if (seriorNo == null || seriorNo.Value < 1)
        //        {
        //            return "缺少流水號或資料不正確";
        //        }

        //        switch (module.PayDueDateKind)
        //        {
        //            case YearDayKind.Empty:
        //                #region 不含繳款期限資料
        //                customNo = module.GenCustomNo(seriorNo.Value);
        //                if (String.IsNullOrEmpty(customNo))
        //                {
        //                    return "缺少流水號或資料不正確";
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //                #endregion
        //            case YearDayKind.ADYearDay4:
        //            case YearDayKind.TWYearDay4:
        //                #region 須含繳款期限資料
        //                if (payDueDate == null)
        //                {
        //                    return "缺少繳款期限";
        //                }
        //                customNo = module.GenCustomNo(seriorNo.Value, payDueDate.Value);
        //                if (String.IsNullOrEmpty(customNo))
        //                {
        //                    return "缺少流水號、繳款期限或資料不正確";
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //                #endregion
        //        }
        //        #endregion
        //    }

        //    return "虛擬帳號模組參數不正確";
        //}

        ///// <summary>
        ///// 嘗試由指定的虛擬帳號模組將商家代號、流水號、繳款期限、身分證字號組成客戶編號
        ///// </summary>
        ///// <param name="module">指定虛擬帳號模組</param>
        ///// <param name="checkReceiveType">指定是否檢查商家代號</param>
        ///// <param name="receiveType">指定商家代號</param>
        ///// <param name="seriorNo">指定流水號</param>
        ///// <param name="payDueDate">指定繳款期限</param>
        ///// <param name="personalId">指定身分證字號</param>
        ///// <param name="corpId">指定營利事業統一編號</param>
        ///// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
        ///// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        //public string TryGetCustomNo(Module module, string receiveType, string seriorNo, DateTime? payDueDate, string personalId, string corpId, out string customNo)
        //{
        //    Int64 sno = 0;
        //    if (!String.IsNullOrWhiteSpace(seriorNo) && !Int64.TryParse(seriorNo.Trim(), out sno))
        //    {
        //        customNo = null;
        //        return "缺少流水號或資料不正確";
        //    }

        //    return this.TryGetCustomNo(module, receiveType, sno, payDueDate, personalId, corpId, out customNo);
        //}

        ///// <summary>
        ///// 嘗試由指定的虛擬帳號模組將商家代號、流水號、繳款期限、身分證字號組成客戶編號
        ///// </summary>
        ///// <param name="module">指定虛擬帳號模組</param>
        ///// <param name="checkReceiveType">指定是否檢查商家代號</param>
        ///// <param name="receiveType">指定商家代號</param>
        ///// <param name="seriorNo">指定流水號</param>
        ///// <param name="payDueDate">指定繳款期限</param>
        ///// <param name="personalId">指定身分證字號</param>
        ///// <param name="corpId">指定營利事業統一編號</param>
        ///// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
        ///// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        //public string TryGetCustomNo(Module module, string receiveType, string seriorNo, string payDueDate, string personalId, string corpId, out string customNo)
        //{
        //    DateTime? myPayDueDate = null;
        //    payDueDate = payDueDate == null ? null : payDueDate.Trim();
        //    if (!String.IsNullOrEmpty(payDueDate))
        //    {
        //        myPayDueDate = DataFormat.ConvertDateText(payDueDate);
        //        if (myPayDueDate == null)
        //        {
        //            customNo = null;
        //            return "繳款期限資料不正確";
        //        }
        //    }
        //    return this.TryGetCustomNo(module, receiveType, seriorNo, myPayDueDate, personalId, corpId, out customNo);
        //}
        #endregion

        /// <summary>
        /// 嘗試由指定的虛擬帳號模組將學年代碼、學期代碼、收費用別代碼、流水號組成客戶編號
        /// </summary>
        /// <param name="module">指定虛擬帳號模組</param>
        /// <param name="yearId">指定學年代號</param>
        /// <param name="termId">指定學期代碼</param>
        /// <param name="receiveId">指定代收費用別代碼</param>
        /// <param name="seriorNo">指定流水號</param>
        /// <param name="isBigReceiveId">指定是否使用兩碼費用別代碼</param>
        /// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string TryGetCustomNo(Module module, string yearId, string termId, string receiveId, Int64? seriorNo, bool isBigReceiveId, out string customNo)
        {
            customNo = null;

            if (module == null)
            {
                return "缺少虛擬帳號模組參數";
            }

            if (module.IsHasPersonalId || module.IsHasCorpId || module.PayDueDateKind != YearDayKind.Empty)
            {
                return "此模組不適用此方法產生客戶編號";
            }

            if (module.SeriorNoSize > 0)
            {
                #region 須含流水號資料
                if (seriorNo == null || seriorNo.Value < 1)
                {
                    return "缺少流水號或資料不正確";
                }

                customNo = module.GenCustomNo(yearId, termId, receiveId, seriorNo.Value, isBigReceiveId);
                if (String.IsNullOrEmpty(customNo))
                {
                    return "缺少學年、學期、代收費用別、流水號或資料不正確";
                }
                return null;
                #endregion
            }

            return "虛擬帳號模組參數不正確";
        }

        /// <summary>
        /// 嘗試由指定的虛擬帳號模組將商家代號、學年代碼、學期代碼、收費用別代碼、流水號組成客戶編號
        /// </summary>
        /// <param name="module">指定虛擬帳號模組</param>
        /// <param name="yearId">指定學年代碼</param>
        /// <param name="termId">指定學期代碼</param>
        /// <param name="receiveId">指定代收費用別代碼</param>
        /// <param name="seriorNo">指定流水號</param>
        /// <param name="isBigReceiveId">指定是否使用兩碼費用別代碼</param>
        /// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
        /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
        public string TryGetCustomNo(Module module, string yearId, string termId, string receiveId, string seriorNo, bool isBigReceiveId, out string customNo)
        {
            Int64 sno = 0;
            if (!String.IsNullOrWhiteSpace(seriorNo) && !Int64.TryParse(seriorNo.Trim(), out sno))
            {
                customNo = null;
                return "缺少流水號或資料不正確";
            }

            return this.TryGetCustomNo(module, yearId, termId, receiveId, sno, isBigReceiveId, out customNo);
        }
        #endregion

        #region 取得 Module 相關方法
        /// <summary>
        /// 取得指定虛擬帳號對應的虛擬帳號模組資訊與業務別碼
        /// </summary>
        /// <param name="cancelNo">指定虛擬帳號</param>
        /// <param name="cancelNo">成功則傳回業務別碼否則傳回 null</param>
        /// <returns>成功則傳回虛擬帳號模組資訊，否則傳回 null</returns>
        public Module GetModuleById(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                id = id.Trim();
                Module[] modules = Module.GetModules();
                foreach (Module module in modules)
                {
                    if (module.Id == id)
                    {
                        return module;
                    }
                }
            }
            return null;
        }

        public Module GetModuleByReceiveType(string receiveType)
        {
            SchoolRTypeEntity instance = null;
            Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
            Result result = null;
            using (EntityFactory factory = new EntityFactory())
            {
                result = factory.SelectFirst<SchoolRTypeEntity>(where, null, out instance);
            }
            if (instance != null && instance.CancelNoRule != null)
            {
                return this.GetModuleById(instance.CancelNoRule);
            }
            return null;
        }
        #endregion

        #region 虛擬帳號 相關方法
        /// <summary>
        /// 嘗試由指定的虛擬帳號模組將指定的商家代號、學年、學期、代收費用別、流水號、金額組成虛擬帳號
        /// </summary>
        /// <param name="module">指定虛擬帳號模組</param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="yearId">指定學年代碼</param>
        /// <param name="termId">指定學期代碼</param>
        /// <param name="receiveId">指定代收費用別代碼</param>
        /// <param name="seriorNo">指定流水號</param>
        /// <param name="isBigReceiveId">指定是否使用兩碼費用別代碼</param>
        /// <param name="amount">指定金額</param>
        /// <param name="cancelNo">成功則傳回虛擬帳號，否則傳回 null</param>
        /// <param name="customNo">成功則傳回客戶編號，否則傳回 null</param>
        /// <param name="checksum">成功則傳回檢查碼，否則傳回 null</param>
        /// <returns>成功則傳回 null 或空字串，否則傳回錯誤訊息</returns>
        public string TryGenCancelNo(Module module, string receiveType, string yearId, string termId, string receiveId, string seriorNo, bool isBigReceiveId, decimal amount
            , out string cancelNo, out string customNo, out string checksum)
        {
            cancelNo = null;
            customNo = null;
            checksum = null;

            string errmsg = this.TryGetCustomNo(module, yearId, termId, receiveId, seriorNo, isBigReceiveId, out customNo);
            if (String.IsNullOrEmpty(errmsg))
            {
                if (module.HasChecksum)
                {
                    if (!this.TryGenChecksum(module, receiveType, customNo, amount, out checksum))
                    {
                        checksum = null;
                        return "無法產生檢查碼";
                    }
                }
                else
                {
                    checksum = String.Empty;
                }
                cancelNo = String.Concat(receiveType, customNo, checksum);
                return null;
            }
            return errmsg;
        }

        /// <summary>
        /// 嘗試由指定的虛擬帳號模組將指定的商家代號、客戶編號、金額組成虛擬帳號
        /// </summary>
        /// <param name="module">指定虛擬帳號模組</param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="customNo">指定客戶編號</param>
        /// <param name="amount">指定金額</param>
        /// <param name="cancelNo">成功則傳回虛擬帳號，否則傳回 null</param>
        /// <param name="checksum">成功則傳回檢查碼，否則傳回 null</param>
        /// <returns>成功則傳回 null 或空字串，否則傳回錯誤訊息</returns>
        public string TryGenCancelNo(Module module, string receiveType, string customNo, decimal amount, out string cancelNo, out string checksum)
        {
            cancelNo = null;
            checksum = null;
            if (module == null)
            {
                return "缺少虛擬帳號模組參數";
            }
            receiveType = receiveType == null ? String.Empty : receiveType.Trim();
            if (!Common.IsNumber(receiveType, module.ReceiveTypeSize))
            {
                return "缺少或不正確的商家代號參數";
            }
            customNo = customNo == null ? String.Empty : customNo.Trim();
            if (!Common.IsNumber(customNo, module.CustomNoSize))
            {
                return "缺少或不正確的客戶編號參數";
            }

            if (module.HasChecksum)
            {
                if (!this.TryGenChecksum(module, receiveType, customNo, amount, out checksum))
                {
                    checksum = null;
                    return "無法產生檢查碼";
                }
            }
            else
            {
                checksum = String.Empty;
            }
            cancelNo = String.Concat(receiveType, customNo, checksum);

            return null;
        }

        /// <summary>
        /// 嘗試以指定的虛擬帳號模組計算指定商家代號、客戶編號、金額的檢查碼
        /// </summary>
        /// <param name="module">指定虛擬帳號模組資訊</param>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="customNo">指定客戶編號，須補期位數</param>
        /// <param name="amount">指定金額</param>
        /// <param name="checksum">成功則傳回檢查碼 (無檢查時傳回空字串)，否則傳回 null</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool TryGenChecksum(Module module, string receiveType, string customNo, decimal amount, out string checksum)
        {
            checksum = null;
            if (module == null)
            {
                return false;
            }
            int customNoSize = module.CustomNoSize;
            if (!Common.IsNumber(customNo, customNoSize))
            {
                return false;
            }
            if (!module.HasChecksum)
            {
                checksum = String.Empty;
                return true;
            }

            switch (module.Id)
            {
                case Module14A:
                    #region Module14A
                    {
                        #region 邏輯 M14A
                        // customNo
                        //   n13  n12  n11  n10  n09  n08  n07  n06  n05  n04  n03  n02  n01  C
                        // X   8    7    6    5    4    3    2    7    6    5    4    3    2
                        //=====================================================================
                        //   s13  s12  s11  s10  s09  s08  s07  s06  s05  s04  s03  s02  s01
                        // sum = SUM(s13, s01)
                        // C = MOD(10-MOD(sum,11),10)
                        #endregion

                        char[] chars = String.Concat(receiveType, customNo).ToCharArray();
                        int[] factors = new int[13] {
                            8, 7, 6, 5, 4, 3, 2,
                            7, 6, 5, 4, 3, 2
                        };
                        int sum = 0;
                        for (int idx = 0; idx < chars.Length; idx++)
                        {
                            sum += Convert.ToInt32(chars[idx].ToString()) * factors[idx];
                        }

                        checksum = ((10 - (sum % 11)) % 10).ToString();
                        return true;
                    }
                    #endregion
                case Module14B:
                    #region Module14B
                    {
                        #region 邏輯 M14B
                        // customNo
                        //   n13  n12  n11  n10  n09  n08  n07  n06  n05  n04  n03  n02  n01  C
                        // X   8    7    6    5    4    3    2    7    6    5    4    3    2
                        //=====================================================================
                        //   s13  s12  s11  s10  s09  s08  s07  s06  s05  s04  s03  s02  s01
                        // sum1 = SUM(s13, s01)
                        //
                        // amount
                        //   a11  a10  a09  a08  a07  a06  a05  a04  a03  a02  a01
                        // X   6    5    4    3    2    7    6    5    4    3    2
                        //=====================================================================
                        //   t11  t10  t09  t08  t07  t06  t05  t04  t03  t02  t01
                        // sum2 = SUM(t11, t01)
                        //
                        // C = MOD(11-MOD((sum1+sum2),11),10)
                        #endregion

                        char[] nChars = String.Concat(receiveType, customNo).ToCharArray();
                        int[] nFactors = new int[13] {
                            8, 7, 6, 5, 4, 3, 2,
                            7, 6, 5, 4, 3, 2
                        };
                        int sum1 = 0;
                        for (int idx = 0; idx < nChars.Length; idx++)
                        {
                            sum1 += Convert.ToInt32(nChars[idx].ToString()) * nFactors[idx];
                        }

                        char[] aChars = DataFormat.GetDecimalString(amount, 11).ToCharArray();
                        int[] aFactors = new int[11] {
                            6, 5, 4, 3, 2,
                            7, 6, 5, 4, 3, 2
                        };
                        int sum2 = 0;
                        for (int idx = 0; idx < aChars.Length; idx++)
                        {
                            sum2 += Convert.ToInt32(aChars[idx].ToString()) * aFactors[idx];
                        }

                        checksum = ((11 - ((sum1 + sum2) % 11)) % 10).ToString();
                        return true;
                    }
                    #endregion

                #region 取消 Module14G
                //case Module14G:
                //    #region Module14G
                //    {
                //        #region 邏輯 M14G
                //        // customNo
                //        //   n12  n11  n10  n09  n08  n07  n06  n05  n04  n03  n02  n01   C1  C2
                //        // X   8    7    6    5    4    3    2    7    6    5    4    3    2
                //        //=====================================================================
                //        //   s13  s12  s11  s10  s09  s08  s07  s06  s05  s04  s03  s02  s01
                //        // sumC2 = SUM(s13, s01)
                //        //
                //        // amount
                //        //   a11  a10  a09  a08  a07  a06  a05  a04  a03  a02  a01
                //        // X   2    3    4    5    6    2    3    4    5    6    7
                //        //=====================================================================
                //        //   B11  B10  B09  B08  B07  B06  B05  B04  B03  B02  B01
                //        // sumC1 = SUM(B11, B10)
                //        // C1 = MOD(11-MOD(sumC1,11),10)
                //        //
                //        //   a11  a10  a09  a08  a07  a06  a05  a04  a03  a02  a01
                //        // X   6    5    4    3    2    7    6    5    4    3    2
                //        //=====================================================================
                //        //   A11  A10  A09  A08  A07  A06  A05  A04  A03  A02  A01
                //        // sumA = SUM(t11, t01)
                //        //
                //        // C2 = MOD(11-MOD((sumC2+sumA),11),10)
                //        #endregion

                //        char[] aChars = DataFormat.GetDecimalString(amount, 11).ToCharArray();
                //        int[] BFactors = new int[11] {
                //            2, 3, 4, 5, 6,
                //            2, 3, 4, 5, 6, 7
                //        };
                //        int sumC1 = 0;
                //        for (int idx = 0; idx < aChars.Length; idx++)
                //        {
                //            sumC1 += Convert.ToInt32(aChars[idx].ToString()) * BFactors[idx];
                //        }
                //        int c1 = (11 - (sumC1 % 11)) % 10;

                //        int[] AFactors = new int[11] {
                //            6, 5, 4, 3, 2,
                //            7, 6, 5, 4, 3, 2
                //        };
                //        int sumA = 0;
                //        for (int idx = 0; idx < aChars.Length; idx++)
                //        {
                //            sumA += Convert.ToInt32(aChars[idx].ToString()) * AFactors[idx];
                //        }

                //        char[] nChars = String.Concat(receiveType, customNo, c1.ToString()).ToCharArray();
                //        int[] nFactors = new int[13] {
                //            8, 7, 6, 5, 4, 3, 2,
                //            7, 6, 5, 4, 3, 2
                //        };
                //        int sumC2 = 0;
                //        for (int idx = 0; idx < nChars.Length; idx++)
                //        {
                //            sumC2 += Convert.ToInt32(nChars[idx].ToString()) * nFactors[idx];
                //        }
                //        int c2 = (11 - ((sumC2 + sumA) % 11)) % 10;

                //        checksum = String.Format("{0}{1}", c1, c2);
                //        return true;
                //    }
                //    #endregion
                #endregion

                case Module16A:
                    #region Module16A
                    {
                        #region 邏輯 M16A
                        // customNo
                        //   n15  n14  n13  n12  n11  n10  n09  n08  n07  n06  n05  n04  n03  n02  n01  C
                        // X   8    7    6    5    4    3    2    9    8    7    6    5    4    3    2
                        //===============================================================================
                        //   s15  s14  s13  s12  s11  s10  s09  s08  s07  s06  s05  s04  s03  s02  s01
                        // sum = SUM(s15, s01)
                        // C = MOD(10-MOD(sum,11),10)
                        #endregion

                        char[] chars = String.Concat(receiveType, customNo).ToCharArray();
                        int[] factors = new int[15] {
                            8, 7, 6, 5, 4, 3, 2,
                            9, 8, 7, 6, 5, 4, 3, 2
                        };
                        int sum = 0;
                        for (int idx = 0; idx < chars.Length; idx++)
                        {
                            sum += Convert.ToInt32(chars[idx].ToString()) * factors[idx];
                        }

                        checksum = ((10 - (sum % 11)) % 10).ToString();
                        return true;
                    }
                    #endregion
                case Module16B:
                    #region Module16B
                    {
                        #region 邏輯 M16B
                        // customNo
                        //   n15  n14  n13  n12  n11  n10  n09  n08  n07  n06  n05  n04  n03  n02  n01  C
                        // X   8    7    6    5    4    3    2    9    8    7    6    5    4    3    2
                        //===============================================================================
                        //   s15  s14  s13  s12  s11  s10  s09  s08  s07  s06  s05  s04  s03  s02  s01
                        // sum1 = SUM(s15, s01)
                        //
                        // amount
                        //   a11  a10  a09  a08  a07  a06  a05  a04  a03  a02  a01
                        // X   6    5    4    3    2    7    6    5    4    3    2
                        //=====================================================================
                        //   t11  t10  t09  t08  t07  t06  t05  t04  t03  t02  t01
                        // sum2 = SUM(t11, t01)
                        //
                        // C = MOD(11-MOD((sum1+sum2),11),10)
                        #endregion

                        char[] nChars = String.Concat(receiveType, customNo).ToCharArray();
                        int[] nFactors = new int[13] {
                            8, 7, 6, 5, 4, 3, 2,
                            7, 6, 5, 4, 3, 2
                        };
                        int sum1 = 0;
                        for (int idx = 0; idx < nChars.Length; idx++)
                        {
                            sum1 += Convert.ToInt32(nChars[idx].ToString()) * nFactors[idx];
                        }

                        char[] aChars = DataFormat.GetDecimalString(amount, 11).ToCharArray();
                        int[] aFactors = new int[11] {
                            6, 5, 4, 3, 2,
                            7, 6, 5, 4, 3, 2
                        };
                        int sum2 = 0;
                        for (int idx = 0; idx < aChars.Length; idx++)
                        {
                            sum2 += Convert.ToInt32(aChars[idx].ToString()) * aFactors[idx];
                        }

                        checksum = ((11 - ((sum1 + sum2) % 11)) % 10).ToString();
                        return true;
                    }
                    #endregion

                #region 取消 Module16G
                //case Module16G:
                //    #region Module16G
                //    {
                //        #region 邏輯 M16G
                //        // customNo
                //        //   n14  n13  n12  n11  n10  n09  n08  n07  n06  n05  n04  n03  n02  n01   C1  C2
                //        // X   9    8    7    6    5    4    3    2    8    7    6    5    4    3
                //        //================================================================================
                //        //   s14  s13  s12  s11  s10  s09  s08  s07  s06  s05  s04  s03  s02  s01
                //        // sumC2 = SUM(s13, s01)
                //        // C2 = MOD(10-MOD((sumC2,11),10)
                //        //
                //        // amount
                //        //   a11  a10  a09  a08  a07  a06  a05  a04  a03  a02  a01
                //        // X   2    3    4    5    6    2    3    4    5    6    7
                //        //=====================================================================
                //        //   B11  B10  B09  B08  B07  B06  B05  B04  B03  B02  B01
                //        // sumC1 = SUM(B11, B10)
                //        // C1 = MOD(10-MOD(sumC1,11),10)
                //        #endregion

                //        char[] aChars = DataFormat.GetDecimalString(amount, 11).ToCharArray();
                //        int[] aFactors = new int[11] {
                //            2, 3, 4, 5, 6,
                //            2, 3, 4, 5, 6, 7
                //        };
                //        int sumC1 = 0;
                //        for (int idx = 0; idx < aChars.Length; idx++)
                //        {
                //            sumC1 += Convert.ToInt32(aChars[idx].ToString()) * aFactors[idx];
                //        }
                //        int c1 = (10 - (sumC1 % 11)) % 10;

                //        char[] nChars = String.Concat(receiveType, customNo, c1.ToString()).ToCharArray();
                //        int[] nFactors = new int[14] {
                //            9, 8, 7, 6, 5, 4, 3, 2,
                //            8, 7, 6, 5, 4, 3
                //        };
                //        int sumC2 = 0;
                //        for (int idx = 0; idx < nChars.Length; idx++)
                //        {
                //            sumC2 += Convert.ToInt32(nChars[idx].ToString()) * nFactors[idx];
                //        }
                //        int c2 = (11 - (sumC2 % 11)) % 10;

                //        checksum = String.Format("{0}{1}", c1, c2);
                //        return true;
                //    }
                //    #endregion
                #endregion

                default:
                    return false;
            }
        }

        /// <summary>
        /// 產生 StudentReceive 的流水號
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="module"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="isBigReceiveId"></param>
        /// <param name="seriorNo"></param>
        /// <returns></returns>
        public bool GenStudentReceiveSeriorNo(EntityFactory factory, Module module, string receiveType, string yearId, string termId, string depId, string receiveId, bool isBigReceiveId, out string seriorNo)
        {
            Int64 maxSeriorNo = module.GetMaxSeriorNo(isBigReceiveId);
            if (maxSeriorNo == 0)
            {
                seriorNo = String.Empty;
                return false;
            }

            SNoHelper helper = new SNoHelper();
            string snKey = helper.GetKeyForStudentReceiveSeriorNo(receiveType, yearId, termId, depId, receiveId);
            Int64 sno = helper.GetNextSNo(factory, snKey, maxSeriorNo, true);
            if (isBigReceiveId)
            {
                seriorNo = sno.ToString().PadLeft(module.SeriorNoSize - 1, '0');
            }
            else
            {
                seriorNo = sno.ToString().PadLeft(module.SeriorNoSize, '0');
            }
            return true;
        }

        /// <summary>
        /// 清除虛擬帳號
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="byKind"></param>
        /// <param name="byValue"></param>
        /// <param name="count"></param>
        /// <param name="sqlLog"></param>
        /// <returns></returns>
        public Result ClearCancelNo(EntityFactory factory, string receiveType, string yearId, string termId, string depId, string receiveId, string byKind, string byValue, out int count, out string sqlLog)
        {
            count = 0;
            sqlLog = null;
            if (factory == null || !factory.IsReady())
            {
                return new Result(false, "缺少或無效的資料庫存取物件參數", ErrorCode.S_INVALID_FACTORY, null);
            }

            #region #region [Old] 土銀不使用原有部別 DepList，改用專用部別 DeptList
            //if (String.IsNullOrEmpty(receiveType)           //必要參數且不可為空字串
            //    || String.IsNullOrEmpty(yearId)             //必要參數且不可為空字串
            //    || String.IsNullOrEmpty(termId)             //必要參數且不可為空字串
            //    || String.IsNullOrEmpty(depId)              //必要參數且不可為空字串
            //    || String.IsNullOrEmpty(receiveId)          //必要參數且不可為空字串
            //    || String.IsNullOrEmpty(byKind)             //必要參數且不可為空字串
            //    || String.IsNullOrEmpty(byValue))
            //{
            //    return new Result(false, "缺少或無效的資料條件參數", CoreStatusCode.INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType)           //必要參數且不可為空字串
                || String.IsNullOrEmpty(yearId)             //必要參數且不可為空字串
                || String.IsNullOrEmpty(termId)             //必要參數且不可為空字串
                || depId == null                            //必要參數
                || String.IsNullOrEmpty(receiveId)          //必要參數且不可為空字串
                || String.IsNullOrEmpty(byKind)             //必要參數且不可為空字串
                || String.IsNullOrEmpty(byValue))           //必要參數且不可為空字串
            {
                return new Result(false, "缺少或無效的資料條件參數", CoreStatusCode.INVALID_PARAMETER, null);
            }

            string sql = @"UPDATE [Student_Receive] SET [Cancel_No] = '', [Cancel_ATMNo] = '', [Cancel_SMNo] = '', [Cancel_PONo] = '', [c_flag] = '0'
     , [Uploadflag] = (CASE WHEN [Uploadflag] = '2' THEN '0'
                            WHEN [Uploadflag] = '3' THEN '1'
                            WHEN [Uploadflag] = '6' THEN '4'
                            WHEN [Uploadflag] = '7' THEN '5'
                            ELSE [Uploadflag]
                            END)
 WHERE [Receive_Type] = @ReceiveType AND [Year_Id] = @YearId AND [Term_Id] = @TermId AND [Dep_Id] = @DepId AND [Receive_Id] = @ReceiveId
   AND [Cancel_No] != ''
   AND ([Receive_Way] = '' OR [Receive_Way] IS NULL)
   AND ([Receive_Date] = '' OR [Receive_Date] IS NULL)
   AND ([Account_Date] = '' OR [Account_Date] IS NULL)
   AND ([Cancel_Date] = '' OR [Cancel_Date] IS NULL)
";
            KeyValueList parameters = new KeyValueList();
            parameters.Add("@ReceiveType", receiveType);
            parameters.Add("@YearId", yearId);
            parameters.Add("@TermId", termId);
            parameters.Add("@DepId", depId);
            parameters.Add("@ReceiveId", receiveId);

            switch (byKind)
            {
                case "BYUPNO":
                    int upNo = 0;
                    if (Int32.TryParse(byValue, out upNo))
                    {
                        sql += "   AND [Up_No] = @UpNo";
                        parameters.Add("@UpNo", upNo.ToString());
                    }
                    else
                    {
                        return new Result(false, "無效的批號參數", CoreStatusCode.INVALID_PARAMETER, null);
                    }
                    break;
                case "BYSTUID":
                    #region [MDY:2018xxxx] 改提供多筆學號，以逗號隔開
                    #region [OLD]
                    //sql += "   AND [Stu_Id] = @StuId";
                    //parameters.Add("@StuId", byValue);
                    #endregion
                    {
                        string[] tmps = byValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tmps.Length > 100)
                        {
                            return new Result(false, "學號參數最多只能100筆", CoreStatusCode.INVALID_PARAMETER, null);
                        }
                        parameters.Capacity = parameters.Count + tmps.Length;
                        List<string> pNames = new List<string>(tmps.Length);
                        for(int idx = 0; idx < tmps.Length; idx++)
                        {
                            string pValue = tmps[idx].Trim();
                            if (!String.IsNullOrEmpty(pValue))
                            {
                                string pName = String.Format("@StuId{0}", pNames.Count);
                                pNames.Add(pName);
                                parameters.Add(pName, pValue);
                            }
                        }
                        if (pNames.Count == 0)
                        {
                            return new Result(false, "無效的學號參數", CoreStatusCode.INVALID_PARAMETER, null);
                        }
                        else
                        {
                            sql += String.Format("   AND [Stu_Id] IN ({0})", String.Join(",", pNames.ToArray()));
                        }
                    }
                    #endregion
                    break;
                case "BYCANCELNO":
                    sql += "   AND [Cancel_No] = @CancelNo";
                    parameters.Add("@CancelNo", byValue);
                    break;
                default:
                    return new Result(false, "缺少或無效的資料條件參數", CoreStatusCode.INVALID_PARAMETER, null);
            }

            Result result = factory.ExecuteNonQuery(sql, parameters, out count);
            if (result.IsSuccess)
            {
                IDBLogger dbLogger = factory.DBLogger;
                sqlLog = String.Format("sql：{0}; \r\nparameters：{1}\r\ncount：{2}\r\n執行成功。", sql, parameters.ToString(), count);
            }
            else
            {
                sqlLog = String.Format("sql：{0}; \r\nparameters：{1}\r\ncount：{2}\r\n執行失敗，錯誤訊息：{3}", sql, parameters.ToString(), count, result.Message);
            }
            return result;
        }

        /// <summary>
        /// 嘗試產生虛擬帳號
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="module"></param>
        /// <param name="studentReceive"></param>
        /// <param name="isBigReceiveId"></param>
        /// <param name="changeChecksum">如果虛擬帳號為使用者上傳的且檢碼不正確，是否更正檢碼</param>
        /// <param name="cancelNo"></param>
        /// <param name="seriorNo"></param>
        /// <param name="checksum"></param>
        /// <param name="atmCancelNo"></param>
        /// <param name="smCancelNo"></param>
        /// <returns></returns>
        public string TryGenCancelNo(EntityFactory factory, Module module, StudentReceiveEntity studentReceive, bool isBigReceiveId
            , out string cancelNo, out string seriorNo, out string checksum, out string atmCancelNo, out string smCancelNo
            , bool changeChecksum = false)
        {
            cancelNo = null;
            seriorNo = null;
            checksum = null;
            atmCancelNo = null;
            smCancelNo = null;

            #region 檢查參數
            if (module == null)
            {
                return "無檢碼規則資料";
            }
            if (studentReceive == null || studentReceive.ReceiveAmount == null)
            {
                return "無學生繳費資料或應繳金額未計算";
            }
            #endregion

            #region 是否產生虛擬帳號邏輯
            //1. 檢碼規則 9 的學校如有上傳虛擬帳號，不論金額是否大於 0，皆以上傳資料為準
            //2. 其他檢碼規則的學校不論是否有上傳虛擬帳號，金額大於 0 才產生，否則清為空字串
            #endregion

            #region 產生流水號與各種虛擬帳號
            SNoHelper snoHelper = new SNoHelper();

            //bool isNeedSaveSeriorNo = true;
            //bool isNeedSaveCancelNo = true;
            bool hasUploadSeriorNo = studentReceive.HasUploadSeriorNo();
            bool hasUploadCancelNo = studentReceive.HasUploadCancelNo();
            if (hasUploadSeriorNo && hasUploadCancelNo)
            {
                //[MEMO] 目前介面不允與同時上傳流水號與虛擬帳號，這裡先寫
                #region 上傳資料含流水號與虛擬帳號
                if (module.IsD38Kind && !String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                {
                    //isNeedSaveSeriorNo = false;
                    //isNeedSaveCancelNo = false;

                    #region 特別處理 module.IsD38Kind 類，直接使用 CanceNo
                    {
                        cancelNo = studentReceive.CancelNo;
                        seriorNo = studentReceive.SeriorNo;
                        checksum = String.Empty;

                        //D38 沒有檢碼也沒有組成邏輯，所以直接用 CancelNo
                        #region 臨櫃虛擬帳號
                        if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                        {
                            atmCancelNo = cancelNo;
                        }
                        else
                        {
                            atmCancelNo = String.Empty;
                        }
                        #endregion

                        #region 超商虛擬帳號
                        if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                        {
                            smCancelNo = cancelNo;
                        }
                        else
                        {
                            smCancelNo = String.Empty;
                        }
                        #endregion

                        return null;
                    }
                    #endregion
                }
                else
                {
                    if (studentReceive.ReceiveAmount > 0)
                    {
                        //isNeedSaveSeriorNo = false;
                        //isNeedSaveCancelNo = false;

                        //金額大於 0 才要產生虛擬帳號
                        #region 檢查流水號與虛擬帳號
                        if (String.IsNullOrWhiteSpace(studentReceive.SeriorNo))
                        {
                            return "上傳繳費資料缺少流水號";
                        }
                        if (String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                        {
                            return "上傳繳費資料缺少虛擬帳號";
                        }
                        #endregion

                        cancelNo = studentReceive.CancelNo;
                        seriorNo = studentReceive.SeriorNo;

                        #region 檢查虛擬帳號 (checksum)
                        {
                            string myReceiveType = null;
                            string myCustomNo = null;
                            string myChecksum = null;
                            string myYearId = null;
                            string myTermId = null;
                            string myReceiveId = null;
                            string mySeriorNo = null;
                            //string customNo = null;
                            if (module.TryParseCancelNo(studentReceive.CancelNo, isBigReceiveId, out myReceiveType, out myCustomNo, out myChecksum, out myYearId, out myTermId, out myReceiveId, out mySeriorNo))
                            {
                                if (myReceiveType != studentReceive.ReceiveType)
                                {
                                    return "上傳虛擬帳號與商家代號不合";
                                }

                                #region [Old] 上傳的虛擬帳號必須完全符合土銀規則 (含學年、學期、費用別)
                                //if (studentReceive.SeriorNo != mySeriorNo)
                                //{
                                //    return "上傳虛擬帳號與上傳流水號不合";
                                //}
                                //else
                                //{
                                //    string errmsg = TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value, out cancelNo, out customNo, out checksum);
                                //    if (!String.IsNullOrEmpty(errmsg))
                                //    {
                                //        return errmsg;
                                //    }
                                //    if (cancelNo != myCustomNo || checksum != myChecksum)
                                //    {
                                //        return String.Format("上傳虛擬帳號與系統產生的虛擬帳號不同", studentReceive.StuId);
                                //    }
                                //}
                                #endregion

                                #region [New] 只檢查檢碼是否正確 (因為土銀允許上傳的虛擬帳號可以不用含學年、學期、費用別)
                                if (module.HasChecksum)
                                {
                                    if (!TryGenChecksum(module, myReceiveType, myCustomNo, studentReceive.ReceiveAmount.Value, out checksum))
                                    {
                                        return "無法計算上傳虛擬帳號的檢查碼";
                                    }
                                    else if (checksum != myChecksum)
                                    {
                                        if (changeChecksum)
                                        {
                                            //重新計算虛擬帳號
                                            string errmsg = TryGenCancelNo(module, myReceiveType, myCustomNo, studentReceive.ReceiveAmount.Value, out cancelNo, out checksum);
                                            if (!String.IsNullOrEmpty(errmsg))
                                            {
                                                return String.Format("上傳虛擬帳號的檢查碼不正確且無法重新計算檢碼，錯誤訊息：{0}", errmsg);
                                            }
                                        }
                                        else
                                        {
                                            return "上傳虛擬帳號的檢查碼不正確";
                                        }
                                    }
                                }
                                else
                                {
                                    checksum = String.Empty;
                                }
                                #endregion
                            }
                            else
                            {
                                return "上傳虛擬帳號不正確無法拆解";
                            }
                        }
                        #endregion

                        //因為土銀的收續費都外加或學校負擔，臨櫃與超商的金額與應繳金額會相同，所以虛擬帳號也會相同
                        #region 臨櫃虛擬帳號
                        if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                        {
                            atmCancelNo = cancelNo;
                        }
                        else
                        {
                            atmCancelNo = String.Empty;
                        }
                        #endregion

                        #region 超商虛擬帳號
                        if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                        {
                            smCancelNo = cancelNo;
                        }
                        else
                        {
                            smCancelNo = String.Empty;
                        }
                        #endregion

                        return null;
                    }
                    else
                    {
                        //isNeedSaveSeriorNo = false;
                        //isNeedSaveCancelNo = true;

                        //否則清除虛擬帳號 (但不清除流水號)
                        cancelNo = String.Empty;
                        seriorNo = studentReceive.SeriorNo;
                        checksum = String.Empty;
                        smCancelNo = String.Empty;
                        atmCancelNo = String.Empty;
                        return null;
                    }
                }
                #endregion
            }
            else if (hasUploadSeriorNo)
            {
                #region 上傳資料含流水號
                //isNeedSaveSeriorNo = false;
                //isNeedSaveCancelNo = true;

                if (studentReceive.ReceiveAmount > 0)
                {
                    //金額大於 0 才要產生虛擬帳號
                    #region 檢查流水號
                    if (String.IsNullOrWhiteSpace(studentReceive.SeriorNo))
                    {
                        return "上傳繳費資料缺少流水號";
                    }
                    #endregion

                    seriorNo = studentReceive.SeriorNo;

                    #region 產生虛擬帳號
                    {
                        string customNo = null;
                        string errmsg = TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value, out cancelNo, out customNo, out checksum);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            return String.Format("無法使用上傳的流水號產生虛擬帳號，錯誤訊息：{0}", errmsg);
                        }

                        //因為土銀的收續費都外加或學校負擔，臨櫃與超商的金額與應繳金額會相同，所以虛擬帳號也會相同
                        #region 臨櫃虛擬帳號
                        if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                        {
                            atmCancelNo = cancelNo;
                        }
                        else
                        {
                            atmCancelNo = String.Empty;
                        }
                        #endregion

                        #region 超商虛擬帳號
                        if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                        {
                            smCancelNo = cancelNo;
                        }
                        else
                        {
                            smCancelNo = String.Empty;
                        }
                        #endregion
                    }
                    #endregion

                    return null;
                }
                else
                {
                    //否則清除虛擬帳號 (但不清除流水號)
                    cancelNo = String.Empty;
                    seriorNo = studentReceive.SeriorNo;
                    checksum = String.Empty;
                    smCancelNo = String.Empty;
                    atmCancelNo = String.Empty;
                    return null;
                }
                #endregion
            }
            else if (hasUploadCancelNo)
            {
                #region 上傳資料含虛擬帳號
                if (module.IsD38Kind && !String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                {
                    //isNeedSaveSeriorNo = false;
                    //isNeedSaveCancelNo = false;

                    #region 特別處理 module.IsD38Kind 類，直接使用 CanceNo
                    {
                        cancelNo = studentReceive.CancelNo;
                        seriorNo = studentReceive.SeriorNo; //保持原來的值 (不管有沒有值)
                        checksum = String.Empty;

                        //D38 沒有檢碼也沒有組成邏輯，所以直接用 CancelNo
                        #region 臨櫃虛擬帳號
                        if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                        {
                            atmCancelNo = cancelNo;
                        }
                        else
                        {
                            atmCancelNo = String.Empty;
                        }
                        #endregion

                        #region 超商虛擬帳號
                        if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                        {
                            smCancelNo = cancelNo;
                        }
                        else
                        {
                            smCancelNo = String.Empty;
                        }
                        #endregion

                        return null;
                    }
                    #endregion
                }
                else
                {
                    if (studentReceive.ReceiveAmount > 0)
                    {
                        //isNeedSaveSeriorNo = false;
                        //isNeedSaveCancelNo = false;

                        //金額大於 0 才要產生虛擬帳號
                        #region 檢查虛擬帳號
                        if (String.IsNullOrWhiteSpace(studentReceive.CancelNo))
                        {
                            return "上傳繳費資料缺少虛擬帳號";
                        }
                        #endregion

                        cancelNo = studentReceive.CancelNo;
                        seriorNo = studentReceive.SeriorNo; //保持原來的值 (不管有沒有值)

                        #region 檢查虛擬帳號 (checksum)
                        {
                            string myReceiveType = null;
                            string myCustomNo = null;
                            string myChecksum = null;
                            string myYearId = null;
                            string myTermId = null;
                            string myReceiveId = null;
                            string mySeriorNo = null;
                            //string customNo = null;
                            if (module.TryParseCancelNo(studentReceive.CancelNo, isBigReceiveId, out myReceiveType, out myCustomNo, out myChecksum, out myYearId, out myTermId, out myReceiveId, out mySeriorNo))
                            {
                                if (myReceiveType != studentReceive.ReceiveType)
                                {
                                    return "上傳虛擬帳號與商家代號不合";
                                }

                                #region [Old] 上傳的虛擬帳號必須完全符合土銀規則 (含學年、學期、費用別)
                                //seriorNo = mySeriorNo;
                                //string errmsg = TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, seriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value, out cancelNo, out customNo, out checksum);
                                //if (!String.IsNullOrEmpty(errmsg))
                                //{
                                //    return errmsg;
                                //}
                                //if (cancelNo != myCustomNo || checksum != myChecksum)
                                //{
                                //    return String.Format("上傳虛擬帳號與系統產生的虛擬帳號不同", studentReceive.StuId);
                                //}
                                #endregion

                                #region [New] 只檢查檢碼是否正確 (因為土銀允許上傳的虛擬帳號可以不用含學年、學期、費用別)
                                if (module.HasChecksum)
                                {
                                    if (!TryGenChecksum(module, myReceiveType, myCustomNo, studentReceive.ReceiveAmount.Value, out checksum))
                                    {
                                        return "無法計算上傳虛擬帳號的檢查碼";
                                    }
                                    else if (checksum != myChecksum)
                                    {
                                        if (changeChecksum)
                                        {
                                            //重新計算虛擬帳號
                                            string errmsg = TryGenCancelNo(module, myReceiveType, myCustomNo, studentReceive.ReceiveAmount.Value, out cancelNo, out checksum);
                                            if (!String.IsNullOrEmpty(errmsg))
                                            {
                                                return String.Format("上傳虛擬帳號的檢查碼不正確且無法重新計算檢碼，錯誤訊息：{0}", errmsg);
                                            }
                                        }
                                        else
                                        {
                                            return "上傳虛擬帳號的檢查碼不正確";
                                        }
                                    }
                                }
                                else
                                {
                                    checksum = String.Empty;
                                }
                                #endregion
                            }
                            else
                            {
                                return "上傳虛擬帳號不正確無法拆解";
                            }
                        }
                        #endregion

                        //因為土銀的收續費都外加或學校負擔，臨櫃與超商的金額與應繳金額會相同，所以虛擬帳號也會相同
                        #region 臨櫃虛擬帳號
                        if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                        {
                            atmCancelNo = cancelNo;
                        }
                        else
                        {
                            atmCancelNo = String.Empty;
                        }
                        #endregion

                        #region 超商虛擬帳號
                        if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                        {
                            smCancelNo = cancelNo;
                        }
                        else
                        {
                            smCancelNo = String.Empty;
                        }
                        #endregion

                        return null;
                    }
                    else
                    {
                        //isNeedSaveSeriorNo = false;
                        //isNeedSaveCancelNo = true;

                        //否則清除虛擬帳號 (但不清除流水號)
                        cancelNo = String.Empty;
                        seriorNo = studentReceive.SeriorNo;
                        checksum = String.Empty;
                        smCancelNo = String.Empty;
                        atmCancelNo = String.Empty;
                        return null;
                    }
                }
                #endregion
            }
            else
            {
                #region 須取流水號
                if (studentReceive.ReceiveAmount > 0)
                {
                    //isNeedSaveSeriorNo = true;
                    //isNeedSaveCancelNo = true;

                    //有舊的流水號就用舊的流水號，為了避免舊流水號位數不同，一律轉成數值後重新轉成字串，並一律更新
                    Int64 oldSeriroNo = 0;
                    if (!String.IsNullOrEmpty(studentReceive.SeriorNo) && Int64.TryParse(studentReceive.SeriorNo, out oldSeriroNo))
                    {
                        seriorNo = oldSeriroNo.ToString().PadLeft(module.SeriorNoSize, '0');
                        //studentReceive.SeriorNo = seriorNo;
                    }
                    else
                    {
                        #region 取得流水號
                        Int64 maxSeriorNo = module.GetMaxSeriorNo(isBigReceiveId);    // Convert.ToInt64(Math.Pow(10, module.SeriorNoSize));
                        string snoKey = snoHelper.GetKeyForStudentReceiveSeriorNo(studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.DepId, studentReceive.ReceiveId);
                        Int64 nextSNo = snoHelper.GetNextSNo(factory, snoKey, maxSeriorNo, false);
                        if (nextSNo > maxSeriorNo)
                        {
                            return String.Format("流水號已超過此虛擬帳號模組允許的最大值 {0} 的限制", maxSeriorNo);
                        }
                        else if (nextSNo == 0)
                        {
                            return String.Format("無法取得 {0} 的下一個流水號", snoKey);
                        }
                        else
                        {
                            seriorNo = nextSNo.ToString().PadLeft(module.SeriorNoSize, '0');
                            studentReceive.SeriorNo = seriorNo;
                        }
                        #endregion
                    }

                    #region 產生虛擬帳號
                    {
                        string customNo = null;
                        string errmsg = TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, isBigReceiveId, studentReceive.ReceiveAmount.Value, out cancelNo, out customNo, out checksum);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            return errmsg;
                        }

                        //因為土銀的收續費都外加或學校負擔，臨櫃與超商的金額與應繳金額會相同，所以虛擬帳號也會相同
                        #region 臨櫃虛擬帳號
                        if (studentReceive.ReceiveAtmamount != null && studentReceive.ReceiveAtmamount.Value > 0)
                        {
                            atmCancelNo = cancelNo;
                        }
                        else
                        {
                            atmCancelNo = String.Empty;
                        }
                        #endregion

                        #region 超商虛擬帳號
                        if (studentReceive.ReceiveSmamount != null && studentReceive.ReceiveSmamount.Value > 0)
                        {
                            smCancelNo = cancelNo;
                        }
                        else
                        {
                            smCancelNo = String.Empty;
                        }
                        #endregion
                    }
                    #endregion

                    return null;
                }
                else
                {
                    //isNeedSaveSeriorNo = false;
                    //isNeedSaveCancelNo = true;

                    //否則清除虛擬帳號 (但不清除流水號)
                    cancelNo = String.Empty;
                    seriorNo = studentReceive.SeriorNo;
                    checksum = String.Empty;
                    smCancelNo = String.Empty;
                    atmCancelNo = String.Empty;
                    return null;
                }
                #endregion
            }
            #endregion
        }


        /// <summary>
        /// 檢查自訂虛擬帳號是否正確
        /// </summary>
        /// <param name="customCancelNo"></param>
        /// <param name="module"></param>
        /// <param name="oldCancelNo"></param>
        /// <param name="receiveAmount"></param>
        /// <returns></returns>
        public string CheckCustomCancelNo(string customCancelNo, Module module, string oldCancelNo, decimal receiveAmount)
        {
            #region 處理邏輯
            //a. 新增的資料要檢查檢碼是否正確
            //b. 修改的資料只有檢查碼可以異動
            //   1. 無檢查碼或檢查碼計算無須金額，表示自訂虛擬帳號必須與舊虛擬帳號完全相同
            //   2. 使用舊虛擬帳號的CustomNo計算出來的虛擬帳號要與自訂虛擬帳號相同
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(customCancelNo))
            {
                return "未指定要檢查的虛擬帳號";
            }
            if (module == null)
            {
                return "未指定虛擬帳號模組資訊";
            }
            if (receiveAmount < 0 && module.IsNeedAmount)
            {
                return "應繳金額小於0無法計算檢查碼";
            }
            if (!Common.IsNumber(customCancelNo, module.CancelNoSize))
            {
                return "指定的虛擬帳號長度與指定虛擬帳號模組不和";
            }
            #endregion

            #region 無舊的虛擬帳號視為新增
            if (String.IsNullOrWhiteSpace(oldCancelNo))
            {
                if (module.HasChecksum)
                {
                    string receiveType = null;
                    string customNo = null;
                    string orgChecksum = null;
                    if (module.TryParseCancelNo(customCancelNo, out receiveType, out customNo, out orgChecksum))
                    {
                        string checksum = null;
                        if (this.TryGenChecksum(module, receiveType, customNo, receiveAmount, out checksum))
                        {
                            if (checksum != orgChecksum)
                            {
                                return "檢查碼不正確";
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return "無法產生檢查碼";
                        }
                    }
                    else
                    {
                        return "指定的虛擬帳號與指定虛擬帳號模組不和";
                    }
                }
                else
                {
                    return null;
                }
            }
            #endregion

            if (!module.HasChecksum)
            {
                //無檢查碼或檢碼計算無須金額，虛擬帳號要完全相同
                if (customCancelNo != oldCancelNo)
                {
                    return "自訂的虛擬帳號不可異動";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string newReceiveType = null;
                string newCustomNo = null;
                string newChecksum = null;
                if (module.TryParseCancelNo(customCancelNo, out newReceiveType, out newCustomNo, out newChecksum))
                {
                    string oldReceiveType = null;
                    string oldCustomNo = null;
                    string oldChecksum = null;
                    if (module.TryParseCancelNo(oldCancelNo, out oldReceiveType, out oldCustomNo, out oldChecksum))
                    {
                        if (newReceiveType != oldReceiveType || newCustomNo != oldCustomNo)
                        {
                            return "自訂的虛擬帳號與舊虛擬帳號不和";
                        }
                        else
                        {
                            string checksum = null;
                            if (this.TryGenChecksum(module, newReceiveType, newCustomNo, receiveAmount, out checksum))
                            {
                                if (checksum != newChecksum)
                                {
                                    return "檢查碼不正確";
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                return "無法產生檢查碼";
                            }
                        }
                    }
                    else
                    {
                        return "自訂的虛擬帳號與舊虛擬帳號不和";
                    }
                }
                else
                {
                    return "自訂的虛擬帳號與指定虛擬帳號模組不和";
                }
            }
        }

        //public string TryGenCancelNo(EntityFactory factory, Module module, StudentReceiveEntity studentReceive, bool isBigReceiveId
        //    , string customCancelNo, string customSeriorNo
        //    , out string newCancelNo, out string newSeriorNo, out string checksum, out string atmCancelNo, out string smCancelNo)
        //{
        //    #region 處理邏輯
        //    //1. 無虛擬帳號且無流水號
        //    //                      虛擬帳號                   | 流水號
        //    //   a. 有自訂虛擬帳號：使用自訂的虛擬帳號         | 自訂流水號 或 null
        //    //   b. 有自訂流水號  ：使用自訂流水號產生虛擬帳號 | 自訂流水號
        //    //   b. 皆無          ：取系統流水號來產生虛擬帳號 | 系統流水號
        //    //2. 無虛擬帳號但有流水號
        //    //                      虛擬帳號                   | 流水號
        //    //   a. 有自訂虛擬帳號：使用自訂的虛擬帳號         | 自訂流水號 或 null
        //    //   b. 有自訂流水號  ：使用自訂流水號產生虛擬帳號 | 自訂流水號
        //    //   b. 皆無          ：使用原流水號來產生虛擬帳號 | 原流水號
        //    //3. 有系統虛擬帳號
        //    //                      虛擬帳號                   | 流水號
        //    //   a. 有自訂虛擬帳號：檢查正確則使用自訂虛擬帳號 | 自訂流水號 或 原流水號
        //    //                      否則丟出錯誤
        //    //   b. 有自訂流水號  ：使用自訂流水號產生虛擬帳號 | 自訂流水號
        //    //   b. 皆無          ：使用原流水號來產生虛擬帳號 | 原流水號
        //    //4. 有自訂虛擬帳號
        //    //                      虛擬帳號                   | 流水號
        //    //   a. 有自訂虛擬帳號：檢查正確則使用自訂虛擬帳號 | 自訂流水號 或 原流水號
        //    //   b. 有自訂流水號  ：丟出錯誤
        //    //   b. 皆無          ：丟出錯誤
        //    #endregion
        //}
        #endregion

        #region Static Method
        /// <summary>
        /// 取得指定模組代碼是否為需要做 D38 處理的模組代碼
        /// </summary>
        /// <param name="id">指定模組代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsD38ModuleId(string id)
        {
            return D38ModuleIds.Contains(id);
        }

        #region [MDY:20190218] 取得指定模組代碼是否為無規則且無檢碼的模組代碼
        /// <summary>
        /// 取得指定模組代碼是否為無規則且無檢碼的模組代碼
        /// </summary>
        /// <param name="id">指定模組代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsUnrulyModuleId(string id)
        {
            return UnrulyModuleIds.Contains(id);
        }

        /// <summary>
        /// 取得指定模組是否為無規則且無檢碼的模組
        /// </summary>
        /// <param name="module">指定模組</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsUnrulyModule(Module module)
        {
            return UnrulyModuleIds.Contains(module.Id);
        }
        #endregion
        #endregion
    }
}
