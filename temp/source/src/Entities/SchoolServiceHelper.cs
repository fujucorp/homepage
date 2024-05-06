using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Fuju;
using Fuju.Configuration;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web;
using Fuju.Web.Services;

using Entities;

namespace Entities.SchoolService
{
    /// <summary>
    /// 單筆新增、修改、刪除學生繳費單資料錯誤代碼文字定義清單類別
    /// </summary>
    public class ErrorList : CodeTextList
    {
        #region 錯誤代碼表
        //S0001：系統代碼或系統驗證碼錯誤
        //S0002：IP錯誤
        //S0003：op值錯誤
        //S0004：學校操作人員為空白
        //S0005：學校操作日期錯誤
        //S0006：學生繳費單資料格式錯誤
        //S9999：系統錯誤
        //D0001：序號重複
        //D0002：繳費單資料有誤
        //D0003：資料不存在
        //D0004：資料已存在
        //D0005：資料已繳費
        //D0006：產生金額發生錯誤
        //D0007：產生銷帳編號發生錯誤
        #endregion

        #region Const Code
        /// <summary>
        /// 成功 : 00000
        /// </summary>
        public const string NORMAL = "00000";

        #region S 類
        /// <summary>
        /// 系統代碼或系統驗證碼錯誤 : S0001
        /// </summary>
        public const string S0001 = "S0001";

        /// <summary>
        /// IP錯誤 : S0002
        /// </summary>
        public const string S0002 = "S0002";

        /// <summary>
        /// op操作參數值錯誤 : S0003
        /// </summary>
        public const string S0003 = "S0003";

        /// <summary>
        /// 學校操作人員參數值錯誤 : S0004
        /// </summary>
        public const string S0004 = "S0004";

        /// <summary>
        /// 學校操作日期或時間參數值錯誤 : S0005
        /// </summary>
        public const string S0005 = "S0005";

        /// <summary>
        /// 學生繳費單資料參數格式錯誤 : S0006
        /// </summary>
        public const string S0006 = "S0006";

        /// <summary>
        /// 查詢資料參數錯誤 : S0007
        /// </summary>
        public const string S0007 = "S0007";

        /// <summary>
        /// 系統錯誤 : S9999
        /// </summary>
        public const string S9999 = "S9999";
        #endregion

        #region D 類
        /// <summary>
        /// 繳費單資料序號重複 : D0001
        /// </summary>
        public const string D0001 = "D0001";

        /// <summary>
        /// 繳費單資料有錯誤 : D0002
        /// </summary>
        public const string D0002 = "D0002";

        /// <summary>
        /// 資料不存在 : D0003
        /// </summary>
        public const string D0003 = "D0003";

        /// <summary>
        /// 資料已存在 : D0004
        /// </summary>
        public const string D0004 = "D0004";

        /// <summary>
        /// 資料已繳費 : D0005
        /// </summary>
        public const string D0005 = "D0005";

        /// <summary>
        /// 產生金額發生錯誤 : D0006
        /// </summary>
        public const string D0006 = "D0006";

        /// <summary>
        /// 產生虛擬帳號發生錯誤 : D0007
        /// </summary>
        public const string D0007 = "D0007";
        #endregion
        #endregion

        #region Const Text
        /// <summary>
        /// 成功 : 00000
        /// </summary>
        public const string NORMAL_TEXT = "00000";

        #region S 類
        /// <summary>
        /// 系統代碼或系統驗證碼錯誤 : S0001
        /// </summary>
        public const string S0001_TEXT = "系統代碼或系統驗證碼錯誤";

        /// <summary>
        /// IP錯誤 : S0002
        /// </summary>
        public const string S0002_TEXT = "IP錯誤";

        /// <summary>
        /// op操作參數值錯誤 : S0003
        /// </summary>
        public const string S0003_TEXT = "op操作參數值錯誤";

        /// <summary>
        /// 學校操作人員參數值錯誤 : S0004
        /// </summary>
        public const string S0004_TEXT = "學校操作人員參數值錯誤";

        /// <summary>
        /// 學校操作日期或時間參數值錯誤 : S0005
        /// </summary>
        public const string S0005_TEXT = "學校操作日期或時間參數值錯誤";

        /// <summary>
        /// 學生繳費單資料參數格式錯誤 : S0006
        /// </summary>
        public const string S0006_TEXT = "學生繳費單資料參數格式錯誤";

        /// <summary>
        /// 查詢學生繳費資料參數錯誤 : S0007
        /// </summary>
        public const string S0007_TEXT = "查詢資料參數錯誤";

        /// <summary>
        /// 系統錯誤 : S9999
        /// </summary>
        public const string S9999_TEXT = "系統錯誤";
        #endregion

        #region D 類
        /// <summary>
        /// 繳費單資料序號重複 : D0001
        /// </summary>
        public const string D0001_TEXT = "繳費單資料序號重複";

        /// <summary>
        /// 繳費單資料有錯誤 : D0002
        /// </summary>
        public const string D0002_TEXT = "繳費單資料有錯誤";

        /// <summary>
        /// 資料不存在 : D0003
        /// </summary>
        public const string D0003_TEXT = "資料不存在";

        /// <summary>
        /// 資料已存在 : D0004
        /// </summary>
        public const string D0004_TEXT = "資料已存在";

        /// <summary>
        /// 資料已繳費 : D0005
        /// </summary>
        public const string D0005_TEXT = "資料已繳費";

        /// <summary>
        /// 產生金額發生錯誤 : D0006
        /// </summary>
        public const string D0006_TEXT = "產生金額發生錯誤";

        /// <summary>
        /// 產生虛擬帳號發生錯誤 : D0007
        /// </summary>
        public const string D0007_TEXT = "產生虛擬帳號發生錯誤";
        #endregion
        #endregion

        #region Static Readonly
        private static readonly ErrorList _ErrorList = new ErrorList();
        #endregion

        #region Constructor
        /// <summary>
        /// 建構單筆新增、修改、刪除學生繳費單資料錯誤代碼文字定義清單類別
        /// </summary>
        private ErrorList()
        {
            base.Add(NORMAL, NORMAL_TEXT);

            #region S 類
            base.Add(S0001, S0001_TEXT);
            base.Add(S0002, S0002_TEXT);
            base.Add(S0003, S0003_TEXT);
            base.Add(S0004, S0004_TEXT);
            base.Add(S0005, S0005_TEXT);
            base.Add(S0006, S0006_TEXT);
            base.Add(S0007, S0007_TEXT);
            base.Add(S9999, S9999_TEXT);
            #endregion

            #region D 類
            base.Add(D0001, D0001_TEXT);
            base.Add(D0002, D0002_TEXT);
            base.Add(D0003, D0003_TEXT);
            base.Add(D0004, D0004_TEXT);
            base.Add(D0005, D0005_TEXT);
            base.Add(D0006, D0006_TEXT);
            base.Add(D0007, D0007_TEXT);
            #endregion
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得單筆新增、修改、刪除學生繳費單資料錯誤代碼對應的文字
        /// </summary>
        /// <param name="code">錯誤代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            #region [Old]
            //switch (code)
            //{
            //    #region S 類
            //    case S0001:
            //        return S0001_TEXT;
            //    case S0002:
            //        return S0002_TEXT;
            //    case S0003:
            //        return S0003_TEXT;
            //    case S0004:
            //        return S0004_TEXT;
            //    case S0005:
            //        return S0005_TEXT;
            //    case S0006:
            //        return S0006_TEXT;
            //    case S0007:
            //        return S0007_TEXT;
            //    case S9999:
            //        return S9999_TEXT;
            //    #endregion

            //    #region D 類
            //    case D0001:
            //        return D0001_TEXT;
            //    case D0002:
            //        return D0002_TEXT;
            //    case D0003:
            //        return D0003_TEXT;
            //    case D0004:
            //        return D0004_TEXT;
            //    case D0005:
            //        return D0005_TEXT;
            //    case D0006:
            //        return D0006_TEXT;
            //    case D0007:
            //        return D0007_TEXT;
            //    #endregion
            //}
            //return string.Empty;
            #endregion

            CodeText data = _ErrorList.Find(x => x.Code == code);
            return data != null ? data.Text : string.Empty;
        }

        /// <summary>
        /// 取得單筆新增、修改、刪除學生繳費單資料錯誤代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">錯誤代碼</param>
        /// <returns>傳回對應的代碼與文字對照類別，無對應則傳回 null</returns>
        public static CodeText GetCodeText(string code)
        {
            #region [Old]
            //string text = GetText(code);
            //if (string.IsNullOrEmpty(text))
            //{
            //    return null;
            //}
            //else
            //{
            //    return new CodeText(code, text);
            //}
            #endregion

            CodeText data = _ErrorList.Find(x => x.Code == code);
            return data != null ? data.Copy() : null;
        }

        /// <summary>
        /// 取得單筆新增、修改、刪除學生繳費單資料錯誤代碼是否為定義值
        /// </summary>
        /// <param name="code">錯誤代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return (_ErrorList.FindIndex(x => x.Code == code) > -1);
        }

        /// <summary>
        /// 取得錯誤訊息 (格式：[err_code][err_msg] 前5碼為錯誤代碼，後面跟著錯誤訊息)
        /// </summary>
        /// <param name="code">錯誤代碼</param>
        /// <param name="message">錯誤訊息，不指定則使用錯誤代碼的預設錯誤訊息</param>
        public static string GetErrorMessage(string code, string message = null)
        {
            if (code == NORMAL)
            {
                return NORMAL;
            }
            if (String.IsNullOrEmpty(message))
            {
                message = GetText(code);
                if (String.IsNullOrEmpty(message))
                {
                    message = "未知的錯誤訊息";
                }
            }
            return String.Concat(code, message);
        }
        #endregion
    }

    #region 單筆新增、修改、刪除學生繳費單資料相關類別
    /// <summary>
    /// 單筆新增、修改、刪除學生繳費單資料 op 操作代碼文字定義清單類別
    /// </summary>
    public class BillOpList : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 新增 : I
        /// </summary>
        public const string INSERT = "I";

        /// <summary>
        /// 修改 : U
        /// </summary>
        public const string UPDATE = "U";

        /// <summary>
        /// 刪除 : D
        /// </summary>
        public const string DELETE = "D";
        #endregion

        #region Const Text
        /// <summary>
        /// 新增 : I
        /// </summary>
        public const string INSERT_TEXT = "新增";

        /// <summary>
        /// 修改 : U
        /// </summary>
        public const string UPDATE_TEXT = "修改";

        /// <summary>
        /// 刪除 : D
        /// </summary>
        public const string DELETE_TEXT = "刪除";
        #endregion

        #region Static Readonly
        private static readonly BillOpList _BillOpList = new BillOpList();
        #endregion

        #region Constructor
        /// <summary>
        /// 建構單筆新增、修改、刪除學生繳費單資料 op 操作代碼文字定義清單類別
        /// </summary>
        private BillOpList()
        {
            base.Add(INSERT, INSERT_TEXT);
            base.Add(UPDATE, UPDATE_TEXT);
            base.Add(DELETE, DELETE_TEXT);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得單筆新增、修改、刪除學生繳費單資料 op 操作代碼對應的文字
        /// </summary>
        /// <param name="code">op 操作代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            CodeText data = _BillOpList.Find(x => x.Code == code);
            return data != null ? data.Text : string.Empty;
        }

        /// <summary>
        /// 取得單筆新增、修改、刪除學生繳費單資料 op 操作代碼對應的代碼與文字對照類別
        /// </summary>
        /// <param name="code">op 操作代碼</param>
        /// <returns>傳回對應的代碼與文字對照類別，無對應則傳回 null</returns>
        public static CodeText GetCodeText(string code)
        {
            CodeText data = _BillOpList.Find(x => x.Code == code);
            return data != null ? data.Copy() : null;
        }

        /// <summary>
        /// 取得單筆新增、修改、刪除學生繳費單資料錯誤代碼是否為定義值
        /// </summary>
        /// <param name="code">op 操作代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return (_BillOpList.FindIndex(x => x.Code == code) > -1);
        }
        #endregion
    }

    /// <summary>
    /// 學生繳費單資料參數承載類別
    /// </summary>
    public class BillTxnData
    {
        #region Const
        public const string RootNodeName = "landbank";
        #endregion

        #region Property
        public BillTxnHeader Header
        {
            get;
            set;
        }

        public BillTxnDetail Detail
        {
            get;
            set;
        }

        public BillTxnInfo Info
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public BillTxnData()
        {

        }
        #endregion

        #region Method
        private void Initial()
        {
            this.Header = null;
            this.Detail = null;
            this.Info = null;
        }

        public string LoadXml(string xml, bool fgInitial = false)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xml);
                return this.Load(xDoc.DocumentElement, fgInitial);
            }
            catch (Exception)
            {
                return "解析 Xml 失敗";
            }
        }

        public string Load(XmlNode xRoot, bool fgInitial = false)
        {
            if (fgInitial)
            {
                this.Initial();
            }
            if (xRoot == null || xRoot.Name != RootNodeName)
            {
                return String.Concat("非", RootNodeName, "節點");
            }

            List<string> errmsgs = new List<string>();
            XmlNode xNode = null;

            #region 處理子節點
            #region BillTxnHeader
            xNode = xRoot.SelectSingleNode(BillTxnHeader.RootNodeName);
            if (xNode != null)
            {
                BillTxnHeader header = new BillTxnHeader();
                string errmsg = header.Load(xNode);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    errmsgs.Add(errmsg);
                }
                this.Header = header;
            }
            else
            {
                errmsgs.Add(String.Format("缺少或不正確的子節點 ({0})", BillTxnHeader.RootNodeName));
            }
            #endregion

            #region BillTxnDetail
            xNode = xRoot.SelectSingleNode(BillTxnDetail.RootNodeName);
            if (xNode != null)
            {
                BillTxnDetail detail = new BillTxnDetail();
                string errmsg = detail.Load(xNode);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    errmsgs.Add(errmsg);
                }
                this.Detail = detail;
            }
            else
            {
                errmsgs.Add(String.Format("缺少或不正確的子節點 ({0})", BillTxnDetail.RootNodeName));
            }
            #endregion

            #region BillTxnInfo
            xNode = xRoot.SelectSingleNode(BillTxnInfo.RootNodeName);
            if (xNode != null)
            {
                BillTxnInfo info = new BillTxnInfo();
                string errmsg = info.Load(xNode);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    errmsgs.Add(errmsg);
                }
                this.Info = info;
            }
            else
            {
                errmsgs.Add(String.Format("缺少或不正確的子節點 ({0})", BillTxnInfo.RootNodeName));
            }
            #endregion
            #endregion

            if (errmsgs.Count > 0)
            {
                return String.Join(";", errmsgs.ToArray());
            }
            else
            {
                return null;
            }
        }
        #endregion
    }

    public class BillTxnHeader
    {
        #region Xml Sample
        //<bill_header>
        //    <seq_no>學校端惟一序號</seq_no>
        //    <receive_type>商家代號</receive_type>
        //    <year_id>學年</year_id>
        //    <term_id>學期</term_id>
        //    <term_name>學期名稱</term_name>
        //    <term_ename>學期英文名稱 (備註1 X(40))</term_ename>
        //    <receive_id>費用別</receive_id>
        //    <receive_name>費用別名稱</receive_name>
        //    <receive_ename>費用別英文名稱 (備註1 X(40))</receive_ename>
        //    <old_seq>分期/分筆代號(非必要 0~99)</old_seq>
        //    <cancel_no>虛擬帳號</cancel_no>
        //    <receive_amount>應繳總金額</receive_amount>
        //    <loan_amount>可貸金額</loan_amount>
        //    <pay_due_date>繳款期限(yyyymmdd)</pay_due_date>
        //    <!-- sm_channel>是否可使用超商管道(Y/N)</sm_channel -->
        //    <stu_id>學生學號</stu_id>
        //    <stu_name>學生姓名</stu_name>
        //    <stu_pid>學生身分證字號(非必要)</stu_pid>
        //    <stu_birthday>學生生日(yyyymmdd)</stu_birthday >
        //    <stu_email>學生電子信箱</stu_email>
        //    <stu_addcode>學生郵遞區號</stu_addcode>
        //    <stu_address>學生地址</stu_address>
        //    <stu_tel>學生電話</stu_tel>
        //</bill_header>
        #endregion

        #region Const
        public const string RootNodeName = "bill_header";
        #endregion

        #region Property
        private string _SeqNo = null;
        /// <summary>
        /// 學校端惟一序號
        /// </summary>
        public string SeqNo
        {
            get
            {
                return _SeqNo;
            }
            set
            {
                _SeqNo = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveType = null;
        /// <summary>
        /// 商家代號
        /// </summary>
        public string ReceiveType
        {
            get
            {
                return _ReceiveType;
            }
            set
            {
                _ReceiveType = value == null ? null : value.Trim();
            }
        }

        private string _YearId = null;
        /// <summary>
        /// 學年
        /// </summary>
        public string YearId
        {
            get
            {
                return _YearId;
            }
            set
            {
                _YearId = value == null ? null : value.Trim();
            }
        }

        private string _TermId = null;
        /// <summary>
        /// 學期
        /// </summary>
        public string TermId
        {
            get
            {
                return _TermId;
            }
            set
            {
                _TermId = value == null ? null : value.Trim();
            }
        }

        private string _TermName = null;
        /// <summary>
        /// 學期名稱
        /// </summary>
        public string TermName
        {
            get
            {
                return _TermName;
            }
            set
            {
                _TermName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加學期英文名稱節點
        private string _TermEName = null;
        /// <summary>
        /// 學期英文名稱
        /// </summary>
        public string TermEName
        {
            get
            {
                return _TermEName;
            }
            set
            {
                _TermEName = value?.Trim();
            }
        }
        #endregion

        private string _ReceiveId = null;
        /// <summary>
        /// 費用別
        /// </summary>
        public string ReceiveId
        {
            get
            {
                return _ReceiveId;
            }
            set
            {
                _ReceiveId = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveName = null;
        /// <summary>
        /// 費用別名稱
        /// </summary>
        public string ReceiveName
        {
            get
            {
                return _ReceiveName;
            }
            set
            {
                _ReceiveName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加費用別英文名稱節點
        private string _ReceiveEName = null;
        /// <summary>
        /// 費用別英文名稱
        /// </summary>
        public string ReceiveEName
        {
            get
            {
                return _ReceiveEName;
            }
            set
            {
                _ReceiveEName = value?.Trim();
            }
        }
        #endregion

        #region [MDY:20211001] M202110_01 新增 分期/分筆代碼 OldSeq (2021擴充案先做)
        private int _OldSeq = 0;
        /// <summary>
        /// 分期/分筆代碼
        /// </summary>
        public int OldSeq
        {
            get
            {
                return _OldSeq;
            }
            set
            {
                _OldSeq = value;
            }
        }
        #endregion

        #region [MDY:20191211] 新增虛擬帳號
        private string _CancelNo = null;
        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public string CancelNo
        {
            get
            {
                return _CancelNo;
            }
            set
            {
                _CancelNo = value == null ? null : value.Trim();
            }
        }
        #endregion

        private Decimal? _ReceiveAmount = null;
        /// <summary>
        /// 應繳總金額
        /// </summary>
        public Decimal? ReceiveAmount
        {
            get
            {
                return _ReceiveAmount;
            }
            set
            {
                _ReceiveAmount = value;
            }
        }

        private Decimal? _LoanAmount = null;
        /// <summary>
        /// 可貸金額
        /// </summary>
        public Decimal? LoanAmount
        {
            get
            {
                return _LoanAmount;
            }
            set
            {
                _LoanAmount = value;
            }
        }

        private DateTime? _PayDueDate = null;
        /// <summary>
        /// (費用別)繳款期限
        /// </summary>
        public DateTime? PayDueDate
        {
            get
            {
                return _PayDueDate;
            }
            set
            {
                _PayDueDate = value;
            }
        }

        #region [OLD]
        //private Boolean? _HasSMChannel = null;
        ///// <summary>
        ///// 是否可使用超商管道
        ///// </summary>
        //public Boolean? HasSMChannel
        //{
        //    get
        //    {
        //        return _HasSMChannel;
        //    }
        //    set
        //    {
        //        _HasSMChannel = value;
        //    }
        //}
        #endregion

        #region 學生基本資料
        private string _StuId = null;
        /// <summary>
        /// 學生學號
        /// </summary>
        public string StuId
        {
            get
            {
                return _StuId;
            }
            set
            {
                _StuId = value == null ? null : value.Trim();
            }
        }

        private string _StuName = null;
        /// <summary>
        /// 學生姓名
        /// </summary>
        public string StuName
        {
            get
            {
                return _StuName;
            }
            set
            {
                _StuName = value == null ? null : value.Trim();
            }
        }

        private string _StuPId = null;
        /// <summary>
        /// 學生身分證字號(非必要)
        /// </summary>
        public string StuPId
        {
            get
            {
                return _StuPId;
            }
            set
            {
                _StuPId = value == null ? null : value.Trim();
            }
        }

        private DateTime? _StuBirthday = null;
        /// <summary>
        /// 學生生日
        /// </summary>
        public DateTime? StuBirthday
        {
            get
            {
                return _StuBirthday;
            }
            set
            {
                _StuBirthday = value;
            }
        }

        private string _StuEmail = null;
        /// <summary>
        /// 學生電子信箱
        /// </summary>
        public string StuEmail
        {
            get
            {
                return _StuEmail;
            }
            set
            {
                _StuEmail = value == null ? null : value.Trim();
            }
        }

        private string _StuAddCode = null;
        /// <summary>
        /// 學生郵遞區號
        /// </summary>
        public string StuAddCode
        {
            get
            {
                return _StuAddCode;
            }
            set
            {
                _StuAddCode = value == null ? null : value.Trim();
            }
        }

        private string _StuAddress = null;
        /// <summary>
        /// 學生地址
        /// </summary>
        public string StuAddress
        {
            get
            {
                return _StuAddress;
            }
            set
            {
                _StuAddress = value == null ? null : value.Trim();
            }
        }

        private string _StuTel = null;
        /// <summary>
        /// 學生電話
        /// </summary>
        public string StuTel
        {
            get
            {
                return _StuTel;
            }
            set
            {
                _StuTel = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region Constructor
        public BillTxnHeader()
        {

        }
        #endregion

        #region Method
        private void Initial()
        {
            _SeqNo = null;
            _ReceiveType = null;
            _YearId = null;
            _TermId = null;
            _TermName = null;

            #region [MDY:20220808] 2022擴充案 增加學期英文名稱節點
            _TermEName = null;
            #endregion

            _ReceiveId = null;
            _ReceiveName = null;

            #region [MDY:20220808] 2022擴充案 增加費用別英文名稱節點
            _ReceiveEName = null;
            #endregion

            #region [MDY:20211001] M202110_01 新增 分期/分筆代碼 OldSeq (2021擴充案先做)
            _OldSeq = 0;
            #endregion

            #region [MDY:20191211] 新增虛擬帳號
            _CancelNo = null;
            #endregion

            _ReceiveAmount = null;
            _LoanAmount = null;
            _PayDueDate = null;

            #region [OLD]
            //_HasSMChannel = null;
            #endregion

            #region 學生基本資料
            _StuId = null;
            _StuName = null;
            _StuPId = null;
            _StuBirthday = null;
            _StuEmail = null;
            _StuAddCode = null;
            _StuAddress = null;
            _StuTel = null;
            #endregion
        }

        public string Load(XmlNode xRoot, bool fgInitial = false)
        {
            if (fgInitial)
            {
                this.Initial();
            }
            if (xRoot == null || xRoot.Name != RootNodeName)
            {
                return String.Concat("非", RootNodeName, "節點");
            }

            List<string> errmsgs = new List<string>();
            XmlNode xNode = null;

            #region 處理子節點
            #region 學校端惟一序號
            xNode = xRoot.SelectSingleNode("seq_no");
            if (xNode != null)
            {
                this.SeqNo = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.SeqNo, 1, 32))
                {
                    errmsgs.Add("學校端惟一序號不合法");
                }
            }
            else
            {
                errmsgs.Add("缺少學校端惟一序號");
            }
            #endregion

            #region 商家代號
            xNode = xRoot.SelectSingleNode("receive_type");
            if (xNode != null)
            {
                this.ReceiveType = xNode.InnerText;
                if (!Common.IsNumber(this.ReceiveType, 4))
                {
                    errmsgs.Add("商家代號不合法");
                }
            }
            else
            {
                errmsgs.Add("缺少商家代號");
            }
            #endregion

            #region 學年
            xNode = xRoot.SelectSingleNode("year_id");
            if (xNode != null)
            {
                this.YearId = xNode.InnerText;
                if (!Common.IsNumber(this.YearId, 3))
                {
                    errmsgs.Add("學年代碼不合法");
                }
            }
            else
            {
                errmsgs.Add("缺少學年代碼");
            }
            #endregion

            #region 學期 & 學期名稱 & 學期英文名稱
            xNode = xRoot.SelectSingleNode("term_id");
            if (xNode != null)
            {
                this.TermId = xNode.InnerText;
                if (!Common.IsNumber(this.TermId, 1))
                {
                    errmsgs.Add("學期代碼不合法");
                }
            }
            else
            {
                errmsgs.Add("缺少學期代碼");
            }

            xNode = xRoot.SelectSingleNode("term_name");
            if (xNode != null)
            {
                this.TermName = xNode.InnerText;
                if (String.IsNullOrEmpty(this.TermName))
                {
                    errmsgs.Add("缺少學期名稱");
                }
                else if (this.TermName.Length > 20)
                {
                    errmsgs.Add("學期名稱不可超過20個字");
                }
            }
            else
            {
                errmsgs.Add("缺少學期名稱");
            }

            //學期英文名稱是否必須值在外面檢查
            xNode = xRoot.SelectSingleNode("term_ename");
            if (xNode != null)
            {
                this.TermEName = xNode.InnerText;
                if (this.TermEName.Length > 140)
                {
                    errmsgs.Add("學期英文名稱不可超過140個字");
                }
            }
            #endregion

            #region 費用別 & 費用別名稱 &  費用別英文名稱
            xNode = xRoot.SelectSingleNode("receive_id");
            if (xNode != null)
            {
                this.ReceiveId = xNode.InnerText;
                if (!Common.IsNumber(this.ReceiveId, 1))
                {
                    errmsgs.Add("費用別代碼不合法");
                }
            }
            else
            {
                errmsgs.Add("缺少費用別代碼");
            }

            xNode = xRoot.SelectSingleNode("receive_name");
            if (xNode != null)
            {
                this.ReceiveName = xNode.InnerText;
                if (String.IsNullOrEmpty(this.ReceiveName))
                {
                    errmsgs.Add("缺少費用別名稱");
                }
                else if (this.ReceiveName.Length > 20)
                {
                    errmsgs.Add("費用別名稱不可超過20個字");
                }
            }
            else
            {
                errmsgs.Add("缺少費用別名稱");
            }

            //費用別英文名稱是否必須有值在外面檢查
            xNode = xRoot.SelectSingleNode("receive_ename");
            if (xNode != null)
            {
                this.ReceiveEName = xNode.InnerText;
                if (this.ReceiveEName.Length > 140)
                {
                    errmsgs.Add("費用別英文名稱不可超過140個字");
                }
            }
            #endregion

            #region [MDY:20211001] M202110_01 新增 分期/分筆代碼 OldSeq (2021擴充案先做)
            xNode = xRoot.SelectSingleNode("old_seq");
            if (xNode != null)
            {
                int oldSeq = 0;
                string txt = xNode.InnerText.Trim();
                if (!String.IsNullOrEmpty(txt) && Int32.TryParse(txt, out oldSeq) && oldSeq >= 0 && oldSeq <= 99)
                {
                    this.OldSeq = oldSeq;
                }
                else
                {
                    errmsgs.Add("分期/分筆代碼必須為 0 ~ 99 的整數");
                }
            }
            #endregion

            #region [MDY:20191211] 新增銷編流水號、虛擬帳號
            xNode = xRoot.SelectSingleNode("cancel_no");
            if (xNode != null)
            {
                this.CancelNo = xNode.InnerText;
                if (!Common.IsNumber(this.CancelNo) || !this.CancelNo .StartsWith(this.ReceiveType))
                {
                    errmsgs.Add("虛擬帳號不合法");
                }
            }
            #endregion

            #region 應繳總金額
            xNode = xRoot.SelectSingleNode("receive_amount");
            if (xNode != null)
            {
                decimal amount = 0M;
                string txt = xNode.InnerText.Trim();
                if (String.IsNullOrEmpty(txt))
                {
                    errmsgs.Add("缺少應繳總金額");
                }
                else
                {
                    if (Decimal.TryParse(txt, out amount))
                    {
                        this.ReceiveAmount = amount;
                    }
                    else
                    {
                        errmsgs.Add("應繳總金額不合法");
                    }
                }
            }
            else
            {
                errmsgs.Add("缺少應繳總金額");
            }
            #endregion

            #region 可貸金額
            xNode = xRoot.SelectSingleNode("loan_amount");
            if (xNode != null)
            {
                decimal amount = 0M;
                string txt = xNode.InnerText.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    if (Decimal.TryParse(xNode.InnerText.Trim(), out amount))
                    {
                        this.LoanAmount = amount;
                    }
                    else
                    {
                        errmsgs.Add("可貸金額不合法");
                    }
                }
            }
            else
            {
                errmsgs.Add("缺少可貸金額");
            }
            #endregion

            #region (費用別)繳款期限
            xNode = xRoot.SelectSingleNode("pay_due_date");
            if (xNode != null)
            {
                DateTime date;
                string txt = xNode.InnerText.Trim();
                if (String.IsNullOrEmpty(txt))
                {
                    errmsgs.Add("缺少(費用別)繳款期限");
                }
                else
                {
                    if (Common.TryConvertDate8(txt, out date))
                    {
                        this.PayDueDate = date;
                    }
                    else
                    {
                        errmsgs.Add("(費用別)繳款期限不合法");
                    }
                }
            }
            else
            {
                errmsgs.Add("缺少(費用別)繳款期限");
            }
            #endregion

            #region [OLD]
            //#region 是否可使用超商管道
            //xNode = xRoot.SelectSingleNode("sm_channel");
            //if (xNode != null)
            //{
            //    string txt = xNode.InnerText.Trim();
            //    if (txt != "Y" && txt != "N")
            //    {
            //        errmsgs.Add("是否可使用超商管道不合法");
            //    }
            //    else
            //    {
            //        this.HasSMChannel = (txt == "Y");
            //    }
            //}
            //else
            //{
            //    errmsgs.Add("缺少是否可使用超商管道");
            //}
            //#endregion
            #endregion

            #region 學生基本資料
            #region 學生學號
            xNode = xRoot.SelectSingleNode("stu_id");
            if (xNode != null)
            {
                this.StuId = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.StuId, 1, 20))
                {
                    errmsgs.Add("學生學號必須為1 ~ 20碼的英文、數字、或英數字混合");
                }
            }
            else
            {
                errmsgs.Add("缺少學生學號");
            }
            #endregion

            #region 學生姓名
            xNode = xRoot.SelectSingleNode("stu_name");
            if (xNode != null)
            {
                this.StuName = xNode.InnerText ?? String.Empty;

                #region {MDY:20160910] 土銀要求可以沒有學生姓名，因為有些學校會先建資料，等學生報到在更新姓名
                if (this.StuName.Length > 60)
                {
                    errmsgs.Add("學生姓名不可超過60個字");
                }
                #endregion
            }
            else
            {
                errmsgs.Add("缺少學生姓名");
            }
            #endregion

            #region 學生身分證字號
            xNode = xRoot.SelectSingleNode("stu_pid");
            if (xNode != null)
            {
                this.StuPId = xNode.InnerText;
                if (this.StuPId.Length > 10)
                {
                    errmsgs.Add("學生身分證字號不可超過10個字");
                }
            }
            else
            {
                errmsgs.Add("缺少學生身分證字號");
            }
            #endregion

            #region 學生生日
            xNode = xRoot.SelectSingleNode("stu_birthday");
            if (xNode != null)
            {
                DateTime date;
                string txt = xNode.InnerText.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    if (Common.TryConvertDate8(txt, out date))
                    {
                        this.StuBirthday = date;
                    }
                    else
                    {
                        errmsgs.Add("學生生日不合法");
                    }
                }
            }
            else
            {
                errmsgs.Add("缺少學生生日");
            }
            #endregion

            #region 學生電子信箱
            xNode = xRoot.SelectSingleNode("stu_email");
            if (xNode != null)
            {
                this.StuEmail = xNode.InnerText;
                if (Encoding.Default.GetByteCount(this.StuEmail) > 50)
                {
                    errmsgs.Add("學生電子信箱不可超過50個字元");
                }
            }
            else
            {
                errmsgs.Add("缺少學生電子信箱");
            }
            #endregion

            #region 學生郵遞區號
            xNode = xRoot.SelectSingleNode("stu_addcode");
            if (xNode != null)
            {
                this.StuAddCode = xNode.InnerText;
                if (!Common.IsNumber(this.StuAddCode, 0, 5))
                {
                    errmsgs.Add("學生郵遞區號必須為1 ~ 5碼的數字");
                }
            }
            else
            {
                errmsgs.Add("缺少學生郵遞區號");
            }
            #endregion

            #region 學生地址
            xNode = xRoot.SelectSingleNode("stu_address");
            if (xNode != null)
            {
                this.StuAddress = xNode.InnerText;
                if (this.StuAddress.Length > 50)
                {
                    errmsgs.Add("學生地址不可超過50個字");
                }
            }
            else
            {
                errmsgs.Add("缺少學生地址");
            }
            #endregion

            #region 學生電話
            xNode = xRoot.SelectSingleNode("stu_tel");
            if (xNode != null)
            {
                this.StuTel = xNode.InnerText;
                if (Encoding.Default.GetByteCount(this.StuTel) > 14)
                {
                    errmsgs.Add("學生電話不可超過14個字元");
                }
            }
            #endregion
            #endregion
            #endregion

            if (errmsgs.Count > 0)
            {
                return String.Join(";", errmsgs.ToArray());
            }
            else
            {
                return null;
            }
        }
        #endregion
    }

    #region [MDY:20220808] 2022擴充案 新增收入科目英文名稱，改用 BillTxnReceiveItem 承載收入科目相關資料
    #region [OLD]
    //public class BillTxnDetail
    //{
    //    #region Xml Sample
    //    //<bill_detail>
    //    //    <!-- 不一定要全部填 -->
    //    //    <receive_item_01_name>收入科目1名稱</receive_item_01_name>
    //    //    <receive_item_01_amount>收入科目1金額</receive_item_01_amount>
    //    //    <receive_item_02_name>收入科目2名稱</receive_item_02_name>
    //    //    <receive_item_02_amount>收入科目2金額</receive_item_02_amount>
    //    //    .....
    //    //    <receive_item_40_name>收入科目40名稱</receive_item_40_name>
    //    //    <receive_item_40_amount>收入科目40金額</receive_item_40_amount>
    //    //    <pay_due_date>繳費單繳款期限(yyyymmdd)</pay_due_date>
    //    //    <nccard_flag>國際信用卡繳費（非必要，Y=啟用; N=停用）</nccard_flag>
    //    //</bill_detail>
    //    #endregion

    //    #region Const
    //    public const string RootNodeName = "bill_detail";

    //    public const int MaxItemCount = 40;
    //    #endregion

    //    #region Member
    //    /// <summary>
    //    /// 收入科目陣列
    //    /// </summary>
    //    private KeyValue<Decimal?>[] _ReceiveItems = null;
    //    #endregion

    //    #region Property
    //    public KeyValue<Decimal?>[] ReceiveItems
    //    {
    //        get
    //        {
    //            return _ReceiveItems;
    //        }
    //    }

    //    #region [MDY:20160305] 繳費單繳款期限
    //    private DateTime? _PayDueDate = null;
    //    /// <summary>
    //    /// 繳費單繳款期限
    //    /// </summary>
    //    public DateTime? PayDueDate
    //    {
    //        get
    //        {
    //            return _PayDueDate;
    //        }
    //        set
    //        {
    //            _PayDueDate = value;
    //        }
    //    }
    //    #endregion

    //    #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
    //    private string _NCCardFlag = null;
    //    /// <summary>
    //    /// 國際信用卡繳費 (Y/N)
    //    /// </summary>
    //    public string NCCardFlag
    //    {
    //        get
    //        {
    //            return _NCCardFlag;
    //        }
    //        set
    //        {
    //            _NCCardFlag = value;
    //        }
    //    }
    //    #endregion
    //    #endregion

    //    #region Constructor
    //    public BillTxnDetail()
    //    {
    //        this.Initial();
    //    }
    //    #endregion

    //    #region Method
    //    private void Initial()
    //    {
    //        _ReceiveItems = new KeyValue<Decimal?>[MaxItemCount];

    //        #region [MDY:20160305] 繳費單繳款期限
    //        _PayDueDate = null;
    //        #endregion

    //        #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
    //        _NCCardFlag = null;
    //        #endregion
    //    }

    //    private bool SetReceiveItem(int index, string name, Decimal? amount)
    //    {
    //        if (index >= 0 && index < MaxItemCount)
    //        {
    //            name = name == null ? String.Empty : name.Trim();
    //            KeyValue<Decimal?> receiveItem = _ReceiveItems[index];
    //            if (receiveItem == null)
    //            {
    //                receiveItem = new KeyValue<decimal?>(name, amount);
    //                _ReceiveItems[index] = receiveItem;
    //            }
    //            else
    //            {
    //                receiveItem.Key = name;
    //                receiveItem.Value = amount;
    //            }
    //            return true;
    //        }
    //        return false;
    //    }

    //    private bool ClearReceiveItem(int index)
    //    {
    //        if (index >= 0 && index < MaxItemCount)
    //        {
    //            _ReceiveItems[index] = null;
    //            return true;
    //        }
    //        return false;
    //    }

    //    public KeyValue<Decimal?> GetReceiveItem(int index)
    //    {
    //        if (index >= 0 && index < MaxItemCount)
    //        {
    //            return _ReceiveItems[index];
    //        }
    //        return null;
    //    }

    //    public KeyValue<Decimal?> GetReceiveItemByNo(string no)
    //    {
    //        int index = 0;
    //        if (int.TryParse(no, out index) && index >= 0 && index < MaxItemCount)
    //        {
    //            return _ReceiveItems[index];
    //        }
    //        return null;
    //    }

    //    public string[] GetAllReceiveItemNames()
    //    {
    //        string[] names = new string[MaxItemCount];
    //        for (int idx = 0; idx < MaxItemCount; idx++)
    //        {
    //            KeyValue<Decimal?> receiveItem = _ReceiveItems[idx];
    //            names[idx] = receiveItem == null ? null : receiveItem.Key;
    //        }
    //        return names;
    //    }

    //    public Decimal?[] GetReceiveItemAmounts()
    //    {
    //        Decimal?[] amounts = new Decimal?[MaxItemCount];
    //        for (int idx = 0; idx < MaxItemCount; idx++)
    //        {
    //            KeyValue<Decimal?> receiveItem = _ReceiveItems[idx];
    //            amounts[idx] = receiveItem == null ? null : receiveItem.Value;
    //        }
    //        return amounts;
    //    }

    //    public string Load(XmlNode xRoot, bool fgInitial = false)
    //    {
    //        if (fgInitial)
    //        {
    //            this.Initial();
    //        }
    //        if (xRoot == null || xRoot.Name != RootNodeName)
    //        {
    //            return String.Concat("非", RootNodeName, "節點");
    //        }

    //        List<string> errmsgs = new List<string>();
    //        XmlNode xNode = null;

    //        #region 處理子節點
    //        #region 收入科目
    //        {
    //            int count = 0;
    //            for (int no = 1; no <= MaxItemCount; no++)
    //            {
    //                bool isReady = true;

    //                #region 收入科目名稱
    //                string receiveItemName = null;
    //                xNode = xRoot.SelectSingleNode(String.Format("receive_item_{0:00}_name", no));
    //                if (xNode != null)
    //                {
    //                    receiveItemName = xNode.InnerText;
    //                    if (receiveItemName.Length > 40)
    //                    {
    //                        isReady = false;
    //                        errmsgs.Add(String.Format("收入科目{0:00}名稱不合法", no));
    //                    }
    //                }
    //                #endregion

    //                #region 收入科目金額
    //                Decimal? receiveItemAmount = null;
    //                xNode = xRoot.SelectSingleNode(String.Format("receive_item_{0:00}_amount", no));
    //                if (xNode != null)
    //                {
    //                    decimal amount;
    //                    string txt = xNode.InnerText.Trim();
    //                    if (!String.IsNullOrEmpty(txt) && Decimal.TryParse(txt, out amount))
    //                    {
    //                        receiveItemAmount = amount;
    //                    }
    //                    else
    //                    {
    //                        isReady = false;
    //                        errmsgs.Add(String.Format("收入科目{0:00}金額不合法", no));
    //                    }
    //                }
    //                #endregion

    //                if (!String.IsNullOrEmpty(receiveItemName) && receiveItemAmount == null)
    //                {
    //                    isReady = false;
    //                    errmsgs.Add(String.Format("指定收入科目{0:00}的名稱時，必須同時指定金額", no));
    //                }
    //                else if (receiveItemAmount != null && String.IsNullOrEmpty(receiveItemName))
    //                {
    //                    isReady = false;
    //                    errmsgs.Add(String.Format("指定收入科目{0:00}的金額時，必須同時指定名稱", no));
    //                }

    //                if (isReady && !String.IsNullOrEmpty(receiveItemName) && receiveItemAmount != null)
    //                {
    //                    count++;
    //                    this.SetReceiveItem(no - 1, receiveItemName, receiveItemAmount);
    //                }
    //            }
    //            if (count == 0)
    //            {
    //                errmsgs.Add("無任何有效的收入科目資料");
    //            }
    //        }
    //        #endregion

    //        #region [MDY:20160305] 繳費單繳款期限
    //        xNode = xRoot.SelectSingleNode("pay_due_date");
    //        if (xNode != null)
    //        {
    //            DateTime date;
    //            string txt = xNode.InnerText.Trim();
    //            if (String.IsNullOrEmpty(txt))
    //            {
    //                errmsgs.Add("缺少繳費單繳款期限");
    //            }
    //            else
    //            {
    //                if (Common.TryConvertDate8(txt, out date))
    //                {
    //                    this.PayDueDate = date;
    //                }
    //                else
    //                {
    //                    errmsgs.Add("繳費單繳款期限不合法");
    //                }
    //            }
    //        }
    //        #endregion

    //        #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
    //        xNode = xRoot.SelectSingleNode("nccard_flag");
    //        if (xNode != null)
    //        {
    //            this.NCCardFlag = xNode.InnerText.Trim();
    //            if (this.NCCardFlag != "Y" && this.NCCardFlag != "N")
    //            {
    //                errmsgs.Add("國際信用卡繳費不合法");
    //            }
    //        }
    //        #endregion
    //        #endregion

    //        if (errmsgs.Count > 0)
    //        {
    //            return String.Join(";", errmsgs.ToArray());
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }
    //    #endregion
    //}
    #endregion

    public class BillTxnDetail
    {
        #region Xml Sample
        //<bill_detail>
        //    <!-- 不一定要全部填 -->
        //    <receive_item_01_name>收入科目1名稱</receive_item_01_name>
        //    <receive_item_01_ename>收入科目01英文名稱 (備註1 X(40))</receive_item_01_ename>
        //    <receive_item_01_amount>收入科目1金額</receive_item_01_amount>
        //    <receive_item_02_name>收入科目2名稱</receive_item_02_name>
        //    <receive_item_02_ename>收入科目02英文名稱 (備註1 X(40))</receive_item_02_ename>
        //    <receive_item_02_amount>收入科目2金額</receive_item_02_amount>
        //    .....
        //    <receive_item_40_name>收入科目40名稱</receive_item_40_name>
        //    <receive_item_40_ename>收入科目40英文名稱 (備註1 X(40))</receive_item_40_ename>
        //    <receive_item_40_amount>收入科目40金額</receive_item_40_amount>
        //    <pay_due_date>繳費單繳款期限(yyyymmdd)</pay_due_date>
        //    <nccard_flag>國際信用卡繳費（非必要，Y=啟用; N=停用）</nccard_flag>
        //</bill_detail>
        #endregion

        #region Const
        public const string RootNodeName = "bill_detail";

        public const int MaxItemCount = 40;
        #endregion

        #region Member
        /// <summary>
        /// 收入科目陣列
        /// </summary>
        private List<BillTxnReceiveItem> _ReceiveItems = null;
        #endregion

        #region Property
        #region [MDY:20160305] 繳費單繳款期限
        private DateTime? _PayDueDate = null;
        /// <summary>
        /// 繳費單繳款期限
        /// </summary>
        public DateTime? PayDueDate
        {
            get
            {
                return _PayDueDate;
            }
            set
            {
                _PayDueDate = value;
            }
        }
        #endregion

        #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
        private string _NCCardFlag = null;
        /// <summary>
        /// 國際信用卡繳費 (Y/N)
        /// </summary>
        public string NCCardFlag
        {
            get
            {
                return _NCCardFlag;
            }
            set
            {
                _NCCardFlag = value;
            }
        }
        #endregion
        #endregion

        #region Constructor
        public BillTxnDetail()
        {
            this.Initial();
        }
        #endregion

        #region Method
        private void Initial()
        {
            if (_ReceiveItems == null)
            {
                _ReceiveItems = new List<BillTxnReceiveItem>(MaxItemCount);
            }
            else
            {
                _ReceiveItems.Clear();
            }

            #region [MDY:20160305] 繳費單繳款期限
            _PayDueDate = null;
            #endregion

            #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
            _NCCardFlag = null;
            #endregion
        }

        public BillTxnReceiveItem GetReceiveItem(int no)
        {
            if (no >= 1 && no <= MaxItemCount)
            {
                return _ReceiveItems.Find(x => x.No == no);
            }
            return null;
        }

        public BillTxnReceiveItem[] GetAllReceiveItems()
        {
            return _ReceiveItems.ToArray();
        }

        #region [OLD]
        //public string[] GetAllReceiveItemNames()
        //{
        //    string[] names = new string[MaxItemCount];
        //    for (int idx = 0; idx < MaxItemCount; idx++)
        //    {
        //        KeyValue<Decimal?> receiveItem = _ReceiveItems[idx];
        //        names[idx] = receiveItem == null ? null : receiveItem.Key;
        //    }
        //    return names;
        //}

        //public Decimal?[] GetReceiveItemAmounts()
        //{
        //    Decimal?[] amounts = new Decimal?[MaxItemCount];
        //    for (int idx = 0; idx < MaxItemCount; idx++)
        //    {
        //        KeyValue<Decimal?> receiveItem = _ReceiveItems[idx];
        //        amounts[idx] = receiveItem == null ? null : receiveItem.Value;
        //    }
        //    return amounts;
        //}
        #endregion

        public string Load(XmlNode xRoot, bool fgInitial = false)
        {
            if (fgInitial)
            {
                this.Initial();
            }
            if (xRoot == null || xRoot.Name != RootNodeName)
            {
                return String.Concat("非", RootNodeName, "節點");
            }

            List<string> errmsgs = new List<string>();
            XmlNode xNode = null;

            #region 處理子節點
            #region 收入科目
            {
                int count = 0;
                for (int no = 1; no <= MaxItemCount; no++)
                {
                    bool isReady = true;

                    #region 收入科目中文名稱
                    string chtName = null;
                    xNode = xRoot.SelectSingleNode($"receive_item_{no:00}_name");
                    if (xNode != null)
                    {
                        chtName = xNode.InnerText;
                        if (chtName.Length > 40)
                        {
                            isReady = false;
                            errmsgs.Add($"收入科目{no:00}名稱不可超過40個字");
                        }
                    }
                    #endregion

                    #region 收入科目英文名稱
                    string engName = null;
                    xNode = xRoot.SelectSingleNode($"receive_item_{no:00}_ename");
                    if (xNode != null)
                    {
                        engName = xNode.InnerText;
                        if (engName.Length > 140)
                        {
                            isReady = false;
                            errmsgs.Add($"收入科目{no:00}英文名稱不可超過140個字");
                        }
                    }
                    #endregion

                    #region 收入科目金額
                    Decimal? amount = null;
                    xNode = xRoot.SelectSingleNode($"receive_item_{no:00}_amount");
                    if (xNode != null)
                    {
                        decimal value;
                        string txt = xNode.InnerText.Trim();
                        if (!String.IsNullOrEmpty(txt) && Decimal.TryParse(txt, out value))
                        {
                            amount = value;
                        }
                        else
                        {
                            isReady = false;
                            errmsgs.Add($"收入科目{no:00}金額不合法");
                        }
                    }
                    #endregion

                    //收入科目英文名稱是否必須有值在外面檢查
                    if (!String.IsNullOrWhiteSpace(chtName) && !amount.HasValue)
                    {
                        isReady = false;
                        errmsgs.Add($"指定收入科目{no:00}的名稱時，必須同時指定金額");
                    }
                    else if (amount.HasValue && String.IsNullOrWhiteSpace(chtName))
                    {
                        isReady = false;
                        errmsgs.Add($"指定收入科目{no:00}的金額時，必須同時指定名稱");
                    }

                    if (isReady && !String.IsNullOrWhiteSpace(chtName) && amount.HasValue)
                    {
                        count++;
                        _ReceiveItems.Add(new BillTxnReceiveItem(no, chtName, engName, amount));
                    }
                }
                if (count == 0)
                {
                    errmsgs.Add("無任何有效的收入科目資料");
                }
            }
            #endregion

            #region [MDY:20160305] 繳費單繳款期限
            xNode = xRoot.SelectSingleNode("pay_due_date");
            if (xNode != null)
            {
                DateTime date;
                string txt = xNode.InnerText.Trim();
                if (String.IsNullOrEmpty(txt))
                {
                    errmsgs.Add("缺少繳費單繳款期限");
                }
                else
                {
                    if (Common.TryConvertDate8(txt, out date))
                    {
                        this.PayDueDate = date;
                    }
                    else
                    {
                        errmsgs.Add("繳費單繳款期限不合法");
                    }
                }
            }
            #endregion

            #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
            xNode = xRoot.SelectSingleNode("nccard_flag");
            if (xNode != null)
            {
                this.NCCardFlag = xNode.InnerText.Trim();
                if (this.NCCardFlag != "Y" && this.NCCardFlag != "N")
                {
                    errmsgs.Add("國際信用卡繳費不合法");
                }
            }
            #endregion
            #endregion

            if (errmsgs.Count > 0)
            {
                return String.Join(";", errmsgs.ToArray());
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
    #endregion

    public class BillTxnInfo
    {
        #region Xml Sample
        //<bill_info>
        //    <!-- 不一定要全部填 -->
        //    <dep_id>部別代碼 </dep_id>
        //    <dep_name>部別名稱</dep_name>
        //    <dep_ename>部別英文名稱 (備註1 X(40))</dep_ename>
        //    <college_id>院別代碼</college_id>
        //    <college_name>院別名稱</college_name>
        //    <college_ename>院別英文名稱 (備註1 X(40))</college_ename>
        //    <major_id>科系代碼</major_id>
        //    <major_name>科系名稱</major_name>
        //    <major_ename>科系英文名稱 (備註1 X(40))</major_ename>
        //    <grade>年級</grade>
        //    <class_id>班級代碼</class_id>
        //    <class_name>班級名稱</class_name>
        //    <class_ename>班級英文名稱 (備註1 X(40))</class_ename>
        //    <dorm_id>住宿代碼</dorm_id>
        //    <dorm_name>住宿名稱</dorm_name>
        //    <dorm_ename>住宿英文名稱 (備註1 X(40))</dorm_ename>
        //    <reduce_id>減免代碼</reduce_id>
        //    <reduce_name>減免名稱</reduce_name>
        //    <reduce_ename>減免英文名稱 (備註1 X(40))</reduce_ename>
        //    <identify_id>身分註記代碼</identify_id>
        //    <identify_name>身分註記名稱</identify_name>
        //    <identify_ename>身分註記英文名稱 (備註1 X(40))</identify_ename>
        //</bill_info>
        #endregion

        #region Const
        public const string RootNodeName = "bill_info";
        #endregion

        #region Property
        private string _DeptId = null;
        /// <summary>
        /// 部別代碼
        /// </summary>
        public string DeptId
        {
            get
            {
                return _DeptId;
            }
            set
            {
                _DeptId = value == null ? null : value.Trim();
            }
        }

        private string _DeptName = null;
        /// <summary>
        /// 部別名稱
        /// </summary>
        public string DeptName
        {
            get
            {
                return _DeptName;
            }
            set
            {
                _DeptName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加部別英文名稱節點
        private string _DeptEName = null;
        /// <summary>
        /// 部別英文名稱
        /// </summary>
        public string DeptEName
        {
            get
            {
                return _DeptEName;
            }
            set
            {
                _DeptEName = value?.Trim();
            }
        }
        #endregion

        private string _CollegeId = null;
        /// <summary>
        /// 院別代碼
        /// </summary>
        public string CollegeId
        {
            get
            {
                return _CollegeId;
            }
            set
            {
                _CollegeId = value == null ? null : value.Trim();
            }
        }

        private string _CollegeName = null;
        /// <summary>
        /// 院別名稱
        /// </summary>
        public string CollegeName
        {
            get
            {
                return _CollegeName;
            }
            set
            {
                _CollegeName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加院別英文名稱節點
        private string _CollegeEName = null;
        /// <summary>
        /// 院別英文名稱
        /// </summary>
        public string CollegeEName
        {
            get
            {
                return _CollegeEName;
            }
            set
            {
                _CollegeEName = value?.Trim();
            }
        }
        #endregion

        private string _MajorId = null;
        /// <summary>
        /// 科系代碼
        /// </summary>
        public string MajorId
        {
            get
            {
                return _MajorId;
            }
            set
            {
                _MajorId = value == null ? null : value.Trim();
            }
        }

        private string _MajorName = null;
        /// <summary>
        /// 科系名稱
        /// </summary>
        public string MajorName
        {
            get
            {
                return _MajorName;
            }
            set
            {
                _MajorName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加科系英文名稱節點
        private string _MajorEName = null;
        /// <summary>
        /// 科系英文名稱
        /// </summary>
        public string MajorEName
        {
            get
            {
                return _MajorEName;
            }
            set
            {
                _MajorEName = value?.Trim();
            }
        }
        #endregion

        private string _Grade = null;
        /// <summary>
        /// 年級
        /// </summary>
        public string Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                _Grade = value == null ? null : value.Trim();
            }
        }

        private string _ClassId = null;
        /// <summary>
        /// 班級代碼
        /// </summary>
        public string ClassId
        {
            get
            {
                return _ClassId;
            }
            set
            {
                _ClassId = value == null ? null : value.Trim();
            }
        }

        private string _ClassName = null;
        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName
        {
            get
            {
                return _ClassName;
            }
            set
            {
                _ClassName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加班級英文名稱節點
        private string _ClassEName = null;
        /// <summary>
        /// 班級英文名稱
        /// </summary>
        public string ClassEName
        {
            get
            {
                return _ClassEName;
            }
            set
            {
                _ClassEName = value?.Trim();
            }
        }
        #endregion

        private string _DormId = null;
        /// <summary>
        /// 住宿代碼
        /// </summary>
        public string DormId
        {
            get
            {
                return _DormId;
            }
            set
            {
                _DormId = value == null ? null : value.Trim();
            }
        }

        private string _DormName = null;
        /// <summary>
        /// 住宿名稱
        /// </summary>
        public string DormName
        {
            get
            {
                return _DormName;
            }
            set
            {
                _DormName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加住宿英文名稱節點
        private string _DormEName = null;
        /// <summary>
        /// 住宿英文名稱
        /// </summary>
        public string DormEName
        {
            get
            {
                return _DormEName;
            }
            set
            {
                _DormEName = value?.Trim();
            }
        }
        #endregion

        private string _ReduceId = null;
        /// <summary>
        /// 減免代碼
        /// </summary>
        public string ReduceId
        {
            get
            {
                return _ReduceId;
            }
            set
            {
                _ReduceId = value == null ? null : value.Trim();
            }
        }

        private string _ReduceName = null;
        /// <summary>
        /// 減免名稱
        /// </summary>
        public string ReduceName
        {
            get
            {
                return _ReduceName;
            }
            set
            {
                _ReduceName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加減免英文名稱節點
        private string _ReduceEName = null;
        /// <summary>
        /// 減免英文名稱
        /// </summary>
        public string ReduceEName
        {
            get
            {
                return _ReduceEName;
            }
            set
            {
                _ReduceEName = value?.Trim();
            }
        }
        #endregion

        private string _IdentifyId = null;
        /// <summary>
        /// 身分註記代碼
        /// </summary>
        public string IdentifyId
        {
            get
            {
                return _IdentifyId;
            }
            set
            {
                _IdentifyId = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyName = null;
        /// <summary>
        /// 身分註記名稱
        /// </summary>
        public string IdentifyName
        {
            get
            {
                return _IdentifyName;
            }
            set
            {
                _IdentifyName = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220808] 2022擴充案 增加身分註記英文名稱節點
        private string _IdentifyEName = null;
        /// <summary>
        /// 身分註記英文名稱
        /// </summary>
        public string IdentifyEName
        {
            get
            {
                return _IdentifyEName;
            }
            set
            {
                _IdentifyEName = value?.Trim();
            }
        }
        #endregion
        #endregion

        #region Constructor
        public BillTxnInfo()
        {
            this.Initial();
        }
        #endregion

        #region Method
        private void Initial()
        {
            _DeptId = null;
            _DeptName = null;

            #region [MDY:20220808] 2022擴充案 增加部別英文名稱節點
            _DeptEName = null;
            #endregion

            _CollegeId = null;
            _CollegeName = null;

            #region [MDY:20220808] 2022擴充案 增加院別英文名稱節點
            _CollegeEName = null;
            #endregion

            _MajorId = null;
            _MajorName = null;

            #region [MDY:20220808] 2022擴充案 增加科系英文名稱節點
            _MajorEName = null;
            #endregion

            _Grade = null;
            _ClassId = null;
            _ClassName = null;

            #region [MDY:20220808] 2022擴充案 增加班級英文名稱節點
            _ClassEName = null;
            #endregion

            _DormId = null;
            _DormName = null;

            #region [MDY:20220808] 2022擴充案 增加住宿英文名稱節點
            _DormEName = null;
            #endregion

            _ReduceId = null;
            _ReduceName = null;

            #region [MDY:20220808] 2022擴充案 增加減免英文名稱節點
            _ReduceEName = null;
            #endregion

            _IdentifyId = null;
            _IdentifyName = null;

            #region [MDY:20220808] 2022擴充案 增加身分註記英文名稱節點
            _IdentifyEName = null;
            #endregion
        }

        public string Load(XmlNode xRoot, bool fgInitial = false)
        {
            if (fgInitial)
            {
                this.Initial();
            }
            if (xRoot == null || xRoot.Name != RootNodeName)
            {
                return String.Concat("非", RootNodeName, "節點");
            }

            List<string> errmsgs = new List<string>();
            XmlNode xNode = null;

            #region 處理子節點
            #region 部別代碼 & 部別名稱 & 部別英文名稱
            xNode = xRoot.SelectSingleNode("dep_id");
            if (xNode != null)
            {
                this.DeptId = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.DeptId, 0, 20))
                {
                    errmsgs.Add("部別代碼不合法");
                }
            }
            xNode = xRoot.SelectSingleNode("dep_name");
            if (xNode != null)
            {
                this.DeptName = xNode.InnerText;
                if (this.DeptName.Length > 40)
                {
                    errmsgs.Add("部別名稱不可超過40個字");
                }
            }
            if (!String.IsNullOrEmpty(this.DeptName) && String.IsNullOrEmpty(this.DeptId))
            {
                errmsgs.Add("指定部別名稱時必須同時指定部別代碼");
            }

            //部別英文名稱是否必須有值在外面檢查
            xNode = xRoot.SelectSingleNode("dep_ename");
            if (xNode != null)
            {
                this.DeptEName = xNode.InnerText;
                if (this.DeptEName.Length > 140)
                {
                    errmsgs.Add("部別英文名稱不可超過140個字");
                }
            }
            #endregion

            #region 院別代碼 & 院別名稱 & 院別英文名稱
            xNode = xRoot.SelectSingleNode("college_id");
            if (xNode != null)
            {
                this.CollegeId = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.CollegeId, 0, 20))
                {
                    errmsgs.Add("院別代碼不合法");
                }
            }
            xNode = xRoot.SelectSingleNode("college_name");
            if (xNode != null)
            {
                this.CollegeName = xNode.InnerText;
                if (this.CollegeName.Length > 40)
                {
                    errmsgs.Add("院別名稱不可超過40個字");
                }
            }
            if (!String.IsNullOrEmpty(this.CollegeName) && String.IsNullOrEmpty(this.CollegeId))
            {
                errmsgs.Add("指定院別名稱時必須同時指定院別代碼");
            }

            //院別英文名稱是否必須有值在外面檢查
            xNode = xRoot.SelectSingleNode("college_ename");
            if (xNode != null)
            {
                this.CollegeEName = xNode.InnerText;
                if (this.CollegeEName.Length > 140)
                {
                    errmsgs.Add("院別英文名稱不可超過140個字");
                }
            }
            #endregion

            #region 科系代碼 & 科系名稱 & 科系英文名稱
            xNode = xRoot.SelectSingleNode("major_id");
            if (xNode != null)
            {
                this.MajorId = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.MajorId, 0, 20))
                {
                    errmsgs.Add("科系代碼多多20碼的英文、數字或英數字混合");
                }
            }

            #region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
            xNode = xRoot.SelectSingleNode("major_name");
            if (xNode != null)
            {
                this.MajorName = xNode.InnerText;
                if (this.MajorName.Length > 40)
                {
                    errmsgs.Add("科系名稱不可超過40個字");
                }
            }
            if (!String.IsNullOrEmpty(this.MajorName) && String.IsNullOrEmpty(this.MajorId))
            {
                errmsgs.Add("指定科系名稱時必須同時指定科系代碼");
            }
            #endregion

            //科系英文名稱是否必須有值在外面檢查
            xNode = xRoot.SelectSingleNode("major_ename");
            if (xNode != null)
            {
                this.MajorEName = xNode.InnerText;
                if (this.MajorEName.Length > 140)
                {
                    errmsgs.Add("科系英文名稱不可超過140個字");
                }
            }
            #endregion

            #region 年級
            xNode = xRoot.SelectSingleNode("grade");
            if (xNode != null)
            {
                this.Grade = xNode.InnerText;
                if (GradeCodeTexts.GetCodeText(this.Grade) == null)
                {
                    errmsgs.Add("年級代碼不合法");
                }
            }
            #endregion

            #region 班級代碼 & 班級名稱 & 班級英文名稱
            xNode = xRoot.SelectSingleNode("class_id");
            if (xNode != null)
            {
                this.ClassId = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.ClassId, 0, 20))
                {
                    errmsgs.Add("班級代碼不合法");
                }
            }
            xNode = xRoot.SelectSingleNode("class_name");
            if (xNode != null)
            {
                this.ClassName = xNode.InnerText;
                if (this.ClassName.Length > 40)
                {
                    errmsgs.Add("班級名稱不可超過40個字");
                }
            }
            if (!String.IsNullOrEmpty(this.ClassName) && String.IsNullOrEmpty(this.ClassId))
            {
                errmsgs.Add("指定班級名稱時必須同時指定班級代碼");
            }

            //班級英文名稱是否必須有值在外面檢查
            xNode = xRoot.SelectSingleNode("class_ename");
            if (xNode != null)
            {
                this.ClassEName = xNode.InnerText;
                if (this.ClassEName.Length > 140)
                {
                    errmsgs.Add("班級英文名稱不可超過140個字");
                }
            }
            #endregion

            #region 住宿代碼 & 住宿名稱 & 住宿英文名稱
            xNode = xRoot.SelectSingleNode("dorm_id");
            if (xNode != null)
            {
                this.DormId = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.DormId, 0, 20))
                {
                    errmsgs.Add("住宿代碼不合法");
                }
            }
            xNode = xRoot.SelectSingleNode("dorm_name");
            if (xNode != null)
            {
                this.DormName = xNode.InnerText;
                if (this.DormName.Length > 40)
                {
                    errmsgs.Add("住宿名稱不可超過40個字");
                }
            }
            if (!String.IsNullOrEmpty(this.DormName) && String.IsNullOrEmpty(this.DormId))
            {
                errmsgs.Add("指定住宿名稱時必須同時指定住宿代碼");
            }

            //住宿英文名稱是否必須有值在外面檢查
            xNode = xRoot.SelectSingleNode("dorm_ename");
            if (xNode != null)
            {
                this.DormEName = xNode.InnerText;
                if (this.DormEName.Length > 140)
                {
                    errmsgs.Add("住宿英文名稱不可超過140個字");
                }
            }
            #endregion

            #region 減免代碼 & 減免名稱 & 減免英文名稱
            xNode = xRoot.SelectSingleNode("reduce_id");
            if (xNode != null)
            {
                this.ReduceId = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.ReduceId, 0, 20))
                {
                    errmsgs.Add("減免代碼不合法");
                }
            }
            xNode = xRoot.SelectSingleNode("reduce_name");
            if (xNode != null)
            {
                this.ReduceName = xNode.InnerText;
                if (this.ReduceName.Length > 40)
                {
                    errmsgs.Add("減免名稱不可超過40個字");
                }
            }
            if (!String.IsNullOrEmpty(this.ReduceName) && String.IsNullOrEmpty(this.ReduceId))
            {
                errmsgs.Add("指定減免名稱時必須同時指定減免代碼");
            }

            //減免英文名稱是否必須有值在外面檢查
            xNode = xRoot.SelectSingleNode("reduce_ename");
            if (xNode != null)
            {
                this.ReduceEName = xNode.InnerText;
                if (this.ReduceEName.Length > 140)
                {
                    errmsgs.Add("減免英文名稱不可超過140個字");
                }
            }
            #endregion

            #region 身分註記代碼 & 身分註記名稱 & 身分註記英文名稱
            xNode = xRoot.SelectSingleNode("identify_id");
            if (xNode != null)
            {
                this.IdentifyId = xNode.InnerText;
                if (!Common.IsEnglishNumber(this.IdentifyId, 0, 20))
                {
                    errmsgs.Add("身分註記代碼不合法");
                }
            }
            xNode = xRoot.SelectSingleNode("identify_name");
            if (xNode != null)
            {
                this.IdentifyName = xNode.InnerText;
                if (this.IdentifyName.Length > 40)
                {
                    errmsgs.Add("身分註記名稱不可超過40個字");
                }
            }
            if (!String.IsNullOrEmpty(this.IdentifyName) && String.IsNullOrEmpty(this.IdentifyId))
            {
                errmsgs.Add("指定身分註記名稱時必須同時指定身分註記代碼");
            }

            //身分註記英文名稱是否必須有值在外面檢查
            xNode = xRoot.SelectSingleNode("identify_ename");
            if (xNode != null)
            {
                this.IdentifyEName = xNode.InnerText;
                if (this.IdentifyEName.Length > 140)
                {
                    errmsgs.Add("身分註記英文名稱不可超過140個字");
                }
            }
            #endregion
            #endregion

            if (errmsgs.Count > 0)
            {
                return String.Join(";", errmsgs.ToArray());
            }
            else
            {
                return null;
            }
        }
        #endregion
    }

    #region [MDY:20220808] 2022擴充案 BillTxnDetail 的 收入科目 承載類別
    /// <summary>
    /// 收入科目 中文名稱、英文名稱、金額 承載類別
    /// </summary>
    public class BillTxnReceiveItem
    {
        #region Property
        /// <summary>
        /// 編號 (01~40)
        /// </summary>
        public Int32 No
        {
            get;
            private set;
        }

        private string _ChtName = null;
        /// <summary>
        /// 中文名稱
        /// </summary>
        public string ChtName
        {
            get
            {
                return _ChtName;
            }
            set
            {
                _ChtName = value?.Trim();
            }
        }

        private string _EngName = null;
        /// <summary>
        /// 英文名稱
        /// </summary>
        public string EngName
        {
            get
            {
                return _EngName;
            }
            set
            {
                _EngName = value?.Trim();
            }
        }

        /// <summary>
        /// 金額
        /// </summary>
        public Decimal? Amount
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public BillTxnReceiveItem(int no)
        {
            if (no < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(no));
            }
        }

        public BillTxnReceiveItem(int no, string chtName, string engName, Decimal? amount)
        {
            if (no < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(no));
            }
            this.No = no;
            this.ChtName = chtName;
            this.EngName = engName;
            this.Amount = amount;
        }
        #endregion
    }
    #endregion
    #endregion

    /// <summary>
    /// 連動製單服務回傳資料類別
    /// </summary>
    [Serializable]
    public class ReturnData
    {
        #region Property
        private string _ErrMsg = null;
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrMsg
        {
            get
            {
                return _ErrMsg;
            }
            set
            {
                _ErrMsg = value == null ? null : value.Trim();
            }
        }

        private int _Records = 0;
        /// <summary>
        /// 資料筆數
        /// </summary>
        public int Records
        {
            get
            {
                return _Records;
            }
            set
            {
                _Records = value < 0 ? 0 : value;
            }
        }

        private byte[] _TxnFile = null;
        /// <summary>
        /// 資料檔
        /// </summary>
        public byte[] TxnFile
        {
            get
            {
                return _TxnFile;
            }
            set
            {
                _TxnFile = value;
            }
        }
        #endregion

        #region Constructor
        public ReturnData()
        {

        }

        public ReturnData(string errCode, string errMsg = null)
        {
            this.ErrMsg = ErrorList.GetErrorMessage(errCode, errMsg);
        }
        #endregion
    }

    /// <summary>
    /// 連動製單服務處理工具類別
    /// </summary>
    public class SchoolServiceHelper : IDisposable
    {
        #region Member
        /// <summary>
        /// 儲存 資料存取物件 的變數
        /// </summary>
        private EntityFactory _Factory = null;
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 SchoolServiceHelper 類別
        /// </summary>
        public SchoolServiceHelper()
        {
            _Factory = new EntityFactory();
        }
        #endregion

        #region Destructor
        /// <summary>
        /// 解構 SchoolServiceHelper 類別
        /// </summary>
        ~SchoolServiceHelper()
        {
            Dispose(false);
        }
        #endregion

        #region Implement IDisposable
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool _Disposed = false;

        /// <summary>
        /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing">指定是否釋放資源。</param>
        private void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    if (_Factory != null)
                    {
                        _Factory.Dispose();
                        _Factory = null;
                    }
                }
                _Disposed = true;
            }
        }
        #endregion

        #region Method
        private void AppendLog(SchoolServiceLogEntity log)
        {
            if (this.IsReady())
            {
                int count;
                _Factory.Insert(log, out count);
            }
        }

        #region [MDY:20220530] Checkmarx 調整
        #region [MDY:20210401] 原碼修正
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sysId"></param>
        /// <param name="sysPXX"></param>
        /// <param name="clientIP"></param>
        /// <param name="methodName"></param>
        /// <param name="methodArguments"></param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回空字串</returns>
        private string ParseCommand(IServiceCommand command
            , out string sysId, out string sysPXX, out string clientIP, out string methodName, out ICollection<KeyValue<string>> methodArguments)
        {
            sysId = null;
            sysPXX = null;
            clientIP = null;
            methodName = null;
            methodArguments = null;

            #region 檢查參數
            SchoolServiceCommand myCommand = command is SchoolServiceCommand ? (SchoolServiceCommand)command : SchoolServiceCommand.Create(command);
            if (myCommand == null)
            {
                return "缺少或無效的資料處理服務命令參數"; ;
            }
            #endregion

            #region 處理服務命令參數
            StringBuilder errmsg = new StringBuilder();

            #region SYS_ID
            if (!myCommand.GetSysId(out sysId)
                || String.IsNullOrWhiteSpace(sysId)     //不允許無此參數或空值
                || Encoding.Default.GetByteCount(sysId) > 32)
            {
                errmsg.AppendFormat("缺少或無效的 {0} 命令參數 ({1})", SchoolServiceCommand.SYS_ID, sysId);
            }
            else
            {
                sysId = sysId.Trim();
            }
            #endregion

            #region SYS_PXX
            if (!myCommand.GetSysPXX(out sysPXX)
                || String.IsNullOrWhiteSpace(sysPXX)        //不允許無此參數或空值
                || Encoding.Default.GetByteCount(sysPXX) > 32)
            {
                errmsg.AppendFormat("缺少或無效的 {0} 命令參數 ({1})", SchoolServiceCommand.SYS_PXX, sysPXX);
            }
            else
            {
                sysPXX = sysPXX.Trim();
            }
            #endregion

            #region CLIENT_IP
            if (!myCommand.GetClientIP(out clientIP)
                || String.IsNullOrWhiteSpace(clientIP)      //不允許無此參數或空值
                || Encoding.Default.GetByteCount(clientIP) > 40)
            {
                errmsg.AppendFormat("缺少或無效的 {0} 命令參數 ({1})", SchoolServiceCommand.CLIENT_IP, clientIP);
            }
            else
            {
                clientIP = clientIP.Trim();
            }
            #endregion

            #region METHOD_NAME
            if (!myCommand.GetMethodName(out methodName)
                || !SchoolServiceMethodName.IsDefine(methodName))
            {
                errmsg.AppendFormat("缺少或無效的 {0} 命令參數 ({1})", SchoolServiceCommand.METHOD_NAME, methodName);
            }
            #endregion

            #region METHOD_ARGUMENTS
            if (!myCommand.GetMethodArguments(out methodArguments))    //允許無此參數
            {
                errmsg.AppendFormat("無效的 {0} 命令參數", SchoolServiceCommand.METHOD_NAME);
            }
            #endregion

            return errmsg.ToString();
            #endregion
        }
        #endregion
        #endregion

        /// <summary>
        /// 取得此物件是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public bool IsReady()
        {
            return (_Factory != null && _Factory.IsReady());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="returnMsg"></param>
        /// <param name="data"></param>
        public void ExecuteCommand(IServiceCommand command, out ReturnData returnData)
        {
            returnData = null;

            #region 檢查並處理服務命令參數
            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            string sysId = null;
            string sysPXX = null;
            string clientIP = null;
            string methodName = null;
            ICollection<KeyValue<string>> methodArguments = null;
            string errmsg = this.ParseCommand(command, out sysId, out sysPXX, out clientIP, out methodName, out methodArguments);

            SchoolServiceLogEntity log = new SchoolServiceLogEntity();
            log.CrtDate = DateTime.Now;
            log.SysId = sysId ?? String.Empty;
            log.SysPXX = sysPXX ?? String.Empty;

            log.ClientIp = clientIP ?? String.Empty;
            log.MethodName = methodName ?? String.Empty;

            #region MethodArgs
            if (methodArguments != null && methodArguments.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValue<string> arg in methodArguments)
                {
                    sb.AppendFormat("{0}={1};", arg.Key, arg.Value);
                }
                log.MethodArgs = sb.ToString();
            }
            else
            {
                log.MethodArgs = String.Empty;
            }
            #endregion

            if (!String.IsNullOrEmpty(errmsg))
            {
                returnData = new ReturnData(ErrorList.S9999);
                log.ErrorMsg = errmsg;
                log.ReturnMsg = returnData.ErrMsg;
                this.AppendLog(log);
                return;
            }
            else
            {
                log.ErrorMsg = String.Empty;
                log.ReturnMsg = String.Empty;
            }
            #endregion
            #endregion

            #region [Old]
            #region 檢查參數
            //SchoolServiceCommand myCommand = command is SchoolServiceCommand ? (SchoolServiceCommand)command : SchoolServiceCommand.Create(command);
            //if (myCommand == null || !myCommand.IsReady())
            //{
            //    errmsg = ErrorList.GetErrorMessage(ErrorList.S9999);
            //    return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_PARAMETER);
            //}
            #endregion

            #region 處理服務命令參數
            //#region SYS_ID
            //string sysId = null;
            //if (!myCommand.GetSysId(out sysId)
            //    || String.IsNullOrWhiteSpace(sysId))    //不允許無此參數或空值
            //{
            //    errmsg = ErrorList.GetErrorMessage(ErrorList.S9999);
            //    return new Result(String.Format("缺少或無效的 {0} 命令參數", SchoolServiceCommand.SYS_ID), ServiceStatusCode.P_LOST_PARAMETER);
            //}
            //#endregion

            //#region SYS_PWD
            //string sysPwd = null;
            //if (!myCommand.GetSysPwd(out sysPwd)
            //    || String.IsNullOrWhiteSpace(sysPwd))    //不允許無此參數或空值
            //{
            //    errmsg = ErrorList.GetErrorMessage(ErrorList.S9999);
            //    return new Result(String.Format("缺少或無效的 {0} 命令參數", SchoolServiceCommand.SYS_PWD), ServiceStatusCode.P_LOST_PARAMETER);
            //}
            //#endregion

            //#region CLIENT_IP
            //string clientIP = null;
            //if (!myCommand.GetClientIP(out clientIP)
            //    || String.IsNullOrWhiteSpace(clientIP))    //不允許無此參數或空值
            //{
            //    errmsg = ErrorList.GetErrorMessage(ErrorList.S9999);
            //    return new Result(String.Format("缺少或無效的 {0} 命令參數", SchoolServiceCommand.CLIENT_IP), ServiceStatusCode.P_LOST_PARAMETER);
            //}
            //#endregion

            //#region METHOD_NAME
            //string methodName = null;
            //if (!myCommand.GetMethodName(out methodName))
            //{
            //    errmsg = ErrorList.GetErrorMessage(ErrorList.S9999);
            //    return new Result(String.Format("缺少或無效的 {0} 命令參數", CallMethodCommand.METHOD_NAME), ServiceStatusCode.P_LOST_PARAMETER);
            //}
            //#endregion

            //#region METHOD_ARGUMENTS
            //ICollection<KeyValue<string>> methodArguments = null;
            //if (!myCommand.GetMethodArguments(out methodArguments))    //允許無此參數
            //{
            //    errmsg = ErrorList.GetErrorMessage(ErrorList.S9999);
            //    return new Result(String.Format("無效的 {0} 命令參數", CallMethodCommand.METHOD_ARGUMENTS), ServiceStatusCode.P_LOST_PARAMETER);
            //}
            //#endregion
            #endregion
            #endregion

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                returnData = new ReturnData(ErrorList.S9999);
                log.ErrorMsg = "缺少或無效的資料存取物件";
                log.ReturnMsg = returnData.ErrMsg;
                this.AppendLog(log);
                return;
            }
            #endregion

            #region 驗證並取得連動製單服務帳號資料
            SchoolServiceAccountEntity account = null;
            {
                string returnMsg = null;
                errmsg = this.CheckSchoolServiceAccount(sysId, sysPXX, clientIP, out returnMsg, out account);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    returnData = new ReturnData()
                    {
                        ErrMsg = returnMsg
                    };
                    log.ErrorMsg = errmsg;
                    log.ReturnMsg = returnData.ErrMsg;
                    this.AppendLog(log);
                    return;
                }
            }
            #endregion

            #region 呼叫方法
            {
                Result result = new Result(true);

                switch (methodName)
                {
                    case SchoolServiceMethodName.Bill:
                        #region 單筆新增、修改、刪除學生繳費單資料
                        {
                            #region 取得方法參數
                            string op = null;
                            string user = null;
                            string mdyDate = null;
                            string mdyTime = null;
                            string txnData = null;
                            foreach (KeyValue<string> argument in methodArguments)
                            {
                                string argName = argument.Key.ToUpper();
                                switch (argName)
                                {
                                    case "OP":
                                        op = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "USER":
                                        user = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "MDYDATE":
                                        mdyDate = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "MDYTIME":
                                        mdyTime = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "TXNDATA":
                                        txnData = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                }
                            }
                            #endregion

                            string returnMsg = null;
                            errmsg = this.Bill(account, op, user, mdyDate, mdyTime, txnData, out returnMsg);
                            returnData = new ReturnData()
                            {
                                ErrMsg = returnMsg
                            };
                            log.ErrorMsg = errmsg;
                            log.ReturnMsg = returnMsg;
                        }
                        #endregion
                        break;
                    case SchoolServiceMethodName.BillQuery:
                        #region 學生繳費資料查詢
                        {
                            #region 取得方法參數
                            string qReceiveType = null;
                            string qYearId = null;
                            string qTermId = null;
                            string qSStuId = null;
                            string qEStuId = null;
                            string qSeqNo = null;
                            foreach (KeyValue<string> argument in methodArguments)
                            {
                                string argName = argument.Key.ToUpper();
                                switch (argName)
                                {
                                    case "RECEIVETYPE":
                                        qReceiveType = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "YEARID":
                                        qYearId = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "TERMID":
                                        qTermId = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "STUIDSTART":
                                        qSStuId = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "STUIDEND":
                                        qEStuId = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "SEQNO":
                                        qSeqNo = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                }
                            }
                            #endregion

                            errmsg = this.BillQuery(account, qReceiveType, qYearId, qTermId, qSStuId, qEStuId, qSeqNo, out returnData);
                            log.ErrorMsg = errmsg;
                            log.ReturnMsg = returnData != null ? returnData.ErrMsg : null;
                        }
                        #endregion
                        break;
                    case SchoolServiceMethodName.PayQuery:
                        #region 入金資料查詢
                        {
                            #region 取得方法參數
                            string qReceiveType = null;
                            string qCancelNo = null;
                            string qReceiveWay = null;
                            string qAccountDate = null;
                            foreach (KeyValue<string> argument in methodArguments)
                            {
                                string argName = argument.Key.ToUpper();
                                switch (argName)
                                {
                                    case "RECEIVETYPE":
                                        qReceiveType = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "CANCELNO":
                                        qCancelNo = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "RECEIVEWAY":
                                        qReceiveWay = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "ACCOUNTDATE":
                                        qAccountDate = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                }
                            }
                            #endregion

                            errmsg = this.PayQuery(account, qReceiveType, qCancelNo, qReceiveWay, qAccountDate, out returnData);
                            log.ErrorMsg = errmsg;
                            log.ReturnMsg = returnData != null ? returnData.ErrMsg : null;
                        }
                        #endregion
                        break;

                    #region [MDY:20191219] 新增繳費資訊查詢
                    case SchoolServiceMethodName.QueryData:
                        #region 繳費資訊查詢
                        {
                            #region 取得方法參數
                            string qDataKind = null;
                            string qReceiveType = null;
                            string qCancelNos = null;
                            string qSeqNos = null;
                            string qReceiveDate = null;
                            int startRecord = 0;
                            foreach (KeyValue<string> argument in methodArguments)
                            {
                                string argName = argument.Key.ToUpper();
                                switch (argName)
                                {
                                    case "DATA_KIND":
                                        qDataKind = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "RECEIVE_TYPE":
                                        qReceiveType = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "CANCEL_NOS":
                                        qCancelNos = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "SEQ_NOS":
                                        qSeqNos = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "RECEVIE_DATE":
                                        qReceiveDate = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                    case "START_RECORD":
                                        startRecord = Common.IsNumber(argument.Value) ? Int32.Parse(argument.Value) : 0;
                                        break;
                                }
                            }
                            #endregion

                            errmsg = this.QueryData(account, qDataKind, qReceiveType, qCancelNos, qSeqNos, qReceiveDate, startRecord, out returnData);
                            log.ErrorMsg = errmsg;
                            log.ReturnMsg = returnData != null ? returnData.ErrMsg : null;
                        }
                        #endregion
                        break;
                    #endregion
                }

                this.AppendLog(log);
                return;
            }
            #endregion
            #endregion
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 驗證並取得連動製單服務帳號資料
        /// </summary>
        /// <param name="sysId">系統代碼</param>
        /// <param name="sysPXX">系統驗證碼</param>
        /// <param name="clientIP">呼叫端 IP</param>
        /// <param name="clientIP">呼叫端 IP</param>
        /// <param name="returnMsg">傳回回傳訊息</param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        public string CheckSchoolServiceAccount(string sysId, string sysPXX, string clientIP, out string returnMsg, out SchoolServiceAccountEntity account)
        {
            returnMsg = null;
            account = null;

            if (String.IsNullOrWhiteSpace(sysId) || String.IsNullOrWhiteSpace(sysPXX))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0001);
                return "未指定系統代碼或系統驗證碼";
            }

            #region [MDY:20160925] 系統驗證碼加密處理
            sysPXX = DataFormat.GetSysCWordEncode(sysPXX);
            if (String.IsNullOrEmpty(sysPXX))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0001);
                return "系統驗證碼加解密處理失敗";
            }
            #endregion

            if (String.IsNullOrWhiteSpace(clientIP))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0002);
                return "未指定呼叫端 IP";
            }
            if (!this.IsReady())
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999, "資料存取物件未準備好");
                return "資料存取物件未準備好";
            }

            #region [MDY:20210401] 原碼修正
            Expression where = new Expression(SchoolServiceAccountEntity.Field.SysId, sysId)
                .And(SchoolServiceAccountEntity.Field.SysPXX, sysPXX)
                .And(SchoolServiceAccountEntity.Field.Status, DataStatusCodeTexts.NORMAL);
            #endregion

            Result result = _Factory.SelectFirst<SchoolServiceAccountEntity>(where, null, out account);
            if (!result.IsSuccess)
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999, "資料存取失敗");
                return String.Format("查詢帳號 {0} 密碼 {1} 資料失敗，錯誤訊息：({2}) {3}", sysId, sysPXX, result.Code, result.Message);
            }

            #region [MDY:20210401] 原碼修正
            if (account == null || account.SysPXX != sysPXX)
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0001);
                return "帳號不存在或系統驗證碼不正確";
            }
            #endregion

            if (!account.IsMyClientIP(clientIP))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0002);
                return "呼叫端 IP 不正確";
            }

            return null;
        }
        #endregion

        /// <summary>
        /// 單筆新增、修改、刪除學生繳費資料
        /// </summary>
        /// <param name="account"></param>
        /// <param name="op">操作 (I：新增; U：修改; D：刪除。參考 BillOpList)</param>
        /// <param name="schUser">學校端操作人員</param>
        /// <param name="schMdyDate">學校端操作日期</param>
        /// <param name="schMdyTime">學校端操作時間</param>
        /// <param name="txnData">學生繳費單資料</param>
        /// <param name="returnMsg">傳回要回傳學校端的訊息 (格式：[err_code][err_msg] 前5碼為錯誤代碼，後面跟著錯誤訊息，err_code = 00000 表示成功)</param>
        /// <returns>傳回錯誤訊息</returns>
        public string Bill(SchoolServiceAccountEntity account, string op, string schUser, string schMdyDate, string schMdyTime, string txnData, out string returnMsg)
        {
            returnMsg = null;

            #region 檢查參數
            if (!BillOpList.IsDefine(op))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0003);
                return String.Format("缺少或不正確的操作參數 ({0})", op);
            }
            if (String.IsNullOrWhiteSpace(schUser) || Encoding.Default.GetByteCount(schUser) > 20)
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0004);
                return String.Format("缺少或不正確的學校操作人員參數 ({0})", schUser);
            }
            DateTime mdyDateTime;
            if (String.IsNullOrWhiteSpace(schMdyDate) || schMdyDate.Length != 8 || String.IsNullOrWhiteSpace(schMdyTime) || schMdyTime.Length != 6
                || !DateTime.TryParse(schMdyDate.Insert(6, "/").Insert(4, "/") + " " + schMdyTime.Insert(4, ":").Insert(2, ":"), out mdyDateTime))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0005);
                return String.Format("缺少或不正確的改日期、時間參數 ({0}, {1})", schMdyDate, schMdyTime);
            }
            if (String.IsNullOrWhiteSpace(txnData))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006);
                return "缺少 txnData 參數資料";
            }

            BillTxnData txn = new BillTxnData();
            string xmlError = txn.LoadXml(txnData);
            if (!String.IsNullOrEmpty(xmlError))
            {
                #region [MDY:20160910] 土銀要求這類錯誤直接將錯誤訊息傳回，免得還要查 log，但是制定的規範回傳資料作多200個字，所以要做超長截斷的處理 (Log 也只開 200 個字)
                if (xmlError.Length > 190)
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006, xmlError.Substring(0, 190) + "...");
                }
                else
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006, xmlError);
                }
                #endregion

                return String.Format("不正確的 txnData 參數資料：{0}", xmlError);
            }

            BillTxnHeader header = txn.Header;
            BillTxnDetail detail = txn.Detail;
            BillTxnInfo info = txn.Info;
            if (!account.IsMyReceiveType(header.ReceiveType))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.D0002, "商家代號不正確");
                return String.Format("不正確的商家代號 ({0})", header.ReceiveType);
            }
            #endregion

            #region 取得商家代號資料
            SchoolRTypeEntity schoolRType = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, header.ReceiveType)
                        .And(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out schoolRType);
                if (result.IsSuccess)
                {
                    if (schoolRType == null)
                    {
                        returnMsg = ErrorList.GetErrorMessage(ErrorList.D0002, "商家代號不正確或已停用");
                        return String.Format("查無商家代號 ({0}) 的資料或已停用", header.ReceiveType);
                    }
                }
                else
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999);
                    return String.Format("查巡無商家代號 ({0}) 的資料失敗，錯誤訊息：{1}", header.ReceiveType, result.Message);
                }
            }
            #endregion

            #region [MDY:20220808] 2022擴充案 檢查是否可有英文名稱相關資料
            bool isEngEnabled = schoolRType.IsEngEnabled();
            BillTxnReceiveItem[] receiveItems = detail.GetAllReceiveItems();
            if (!isEngEnabled)
            {
                if (!String.IsNullOrEmpty(header.TermEName) || !String.IsNullOrEmpty(header.ReceiveEName))
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006, "該商家代號未啟用英文資料，不可指定學期、費用別的英文名稱欄位");
                    return $"商家代號 {header.ReceiveType} 未啟用英文資料，不可指定學期、費用別的英文名稱欄位";
                }

                foreach (BillTxnReceiveItem receiveItem in receiveItems)
                {
                    if (!String.IsNullOrEmpty(receiveItem.EngName))
                    {
                        returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006, "該商家代號未啟用英文資料，不可指定收入科目英文名稱相關欄位");
                        return $"商家代號 {header.ReceiveType} 未啟用英文資料，不可指定英文名稱相關欄位";
                    }
                }

                if (!String.IsNullOrEmpty(info.DeptEName)
                    || !String.IsNullOrEmpty(info.CollegeEName)
                    || !String.IsNullOrEmpty(info.MajorEName)
                    || !String.IsNullOrEmpty(info.ClassEName)
                    || !String.IsNullOrEmpty(info.DormEName)
                    || !String.IsNullOrEmpty(info.ReduceEName)
                    || !String.IsNullOrEmpty(info.IdentifyEName))
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006, "該商家代號未啟用英文資料，不可指定部別、院別、科系、班級、住宿、減免、身分註記英文名稱相關欄位");
                    return $"商家代號 {header.ReceiveType} 未啟用英文資料，不可指定部別、院別、科系、班級、住宿、減免、身分註記英文名稱相關欄位";
                }
            }
            else
            {
                if (String.IsNullOrEmpty(header.TermEName) || String.IsNullOrEmpty(header.ReceiveEName))
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006, "該商家代號已啟用英文資料，必須指定學期、費用別的英文名稱");
                    return $"商家代號 {header.ReceiveType} 已啟用英文資料，必須指定學期、費用別的英文名稱";
                }

                foreach (BillTxnReceiveItem receiveItem in receiveItems)
                {
                    if (String.IsNullOrEmpty(receiveItem.EngName))
                    {
                        returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006, "該商家代號已啟用英文資料，必須指定收入科目英文名稱相關欄位");
                        return $"商家代號 {header.ReceiveType} 已啟用英文資料，必須指定收入科目英文名稱相關欄位";
                    }
                    if ((!String.IsNullOrEmpty(info.DeptName) && String.IsNullOrEmpty(info.DeptEName))
                        || (!String.IsNullOrEmpty(info.CollegeName) && String.IsNullOrEmpty(info.CollegeEName))
                        || (!String.IsNullOrEmpty(info.MajorName) && String.IsNullOrEmpty(info.MajorEName))
                        || (!String.IsNullOrEmpty(info.ClassName) && String.IsNullOrEmpty(info.ClassEName))
                        || (!String.IsNullOrEmpty(info.DormName) && String.IsNullOrEmpty(info.DormEName))
                        || (!String.IsNullOrEmpty(info.ReduceName) && String.IsNullOrEmpty(info.ReduceEName))
                        || (!String.IsNullOrEmpty(info.IdentifyName) && String.IsNullOrEmpty(info.IdentifyEName)))
                    {
                        returnMsg = ErrorList.GetErrorMessage(ErrorList.S0006, "該商家代號已啟用英文資料，指定部別、院別、科系、班級、住宿、減免、身分註記中文名稱時必須同時指定英文名稱相關欄位");
                        return $"商家代號 {header.ReceiveType} 已啟用英文資料，指定部別、院別、科系、班級、住宿、減免、身分註記中文名稱時必須同時指定英文名稱相關欄位";
                    }
                }
            }
            #endregion

            #region [MDY:20200815] M202008_02 學生身分證字號不可與學號相同 (2020806_01)
            if (!String.IsNullOrEmpty(header.StuPId) && header.StuPId.Equals(header.StuId))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.D0002, "學生身分證字號不可與學號相同");
                return "學生身分證字號不可與學號相同";
            }
            #endregion

            #region [MDY:20200815] 虛擬帳號必須以商家代號開頭
            if (!String.IsNullOrEmpty(header.CancelNo) && !header.CancelNo.StartsWith(header.ReceiveType))
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.D0002, "虛擬帳號不合法，必須以商家代號開頭");
                return "虛擬帳號不合法，必須以商家代號開頭";
            }
            #endregion

            #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
            if (detail.NCCardFlag == "Y")
            {
                if (String.IsNullOrEmpty(schoolRType.MerchantId2) || String.IsNullOrEmpty(schoolRType.TerminalId2) || String.IsNullOrEmpty(schoolRType.MerId2))
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.D0002, "無國際信用卡管道不允許啟用國際信用卡繳費");
                    return "無國際信用卡管道不允許啟用國際信用卡繳費";
                }
            }
            #endregion

            StudentReceiveEntity oldStudentReceive = null;  //要修改或刪除的學生繳費資料
            switch (op)
            {
                case BillOpList.INSERT:
                    #region 新增要檢查資料的學校端惟一序號是否重複
                    {
                        int count = 0;
                        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, header.ReceiveType)
                            .And(StudentReceiveEntity.Field.ServiceSeqNo, header.SeqNo);
                        Result result = _Factory.SelectCount<StudentReceiveEntity>(where, out count);
                        if (result.IsSuccess)
                        {
                            if (count > 0)
                            {
                                returnMsg = ErrorList.GetErrorMessage(ErrorList.D0001);
                                return String.Format("商家代號 {0} 的學校端惟一序號 {1} 的資料已存在", header.ReceiveType, header.SeqNo);
                            }
                        }
                        else
                        {
                            returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999);
                            return String.Format("查詢學生繳費資料的學校端惟一序號資料失敗，錯誤訊息：{0}", result.Message);
                        }

                        #region [MDY:20160327] 檢查資料的 PKey 是否重複
                        #region [MDY:20211001] M202110_01 新增 分期/分筆代碼 OldSeq (2021擴充案先做)
                        where = new Expression(StudentReceiveEntity.Field.ReceiveType, header.ReceiveType)
                            .And(StudentReceiveEntity.Field.YearId, header.YearId)
                            .And(StudentReceiveEntity.Field.TermId, header.TermId)
                            .And(StudentReceiveEntity.Field.DepId, String.Empty)    //土銀沒有使用原部別，所以固定為空字串
                            .And(StudentReceiveEntity.Field.ReceiveId, header.ReceiveId)
                            .And(StudentReceiveEntity.Field.StuId, header.StuId)
                            .And(StudentReceiveEntity.Field.OldSeq, header.OldSeq);
                        #endregion
                        result = _Factory.SelectCount<StudentReceiveEntity>(where, out count);
                        if (result.IsSuccess)
                        {
                            if (count > 0)
                            {
                                returnMsg = ErrorList.GetErrorMessage(ErrorList.D0004);
                                return "該學生繳費資料已存在";
                            }
                        }
                        else
                        {
                            returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999);
                            return String.Format("查詢學生繳費資料是否重複失敗，錯誤訊息：{0}", result.Message);
                        }
                        #endregion
                    }
                    #endregion
                    break;
                case BillOpList.UPDATE:
                case BillOpList.DELETE:
                    #region 取舊資料
                    {
                        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, header.ReceiveType)
                            .And(StudentReceiveEntity.Field.ServiceSeqNo, header.SeqNo);
                        Result result = _Factory.SelectFirst<StudentReceiveEntity>(where, null, out oldStudentReceive);
                        if (result.IsSuccess)
                        {
                            if (oldStudentReceive == null)
                            {
                                returnMsg = ErrorList.GetErrorMessage(ErrorList.D0003);
                                return String.Format("商家代號 {0} 的學校端惟一序號 {1} 的資料不存在", header.ReceiveType, header.SeqNo);
                            }
                            if (oldStudentReceive.StuId != header.StuId)
                            {
                                returnMsg = ErrorList.GetErrorMessage(ErrorList.D0003);
                                return String.Format("商家代號 {0} 學校端惟一序號 {1} 的學號 ({2}) 與上傳資料({3})不同", oldStudentReceive.ReceiveType, oldStudentReceive.ServiceSeqNo, oldStudentReceive.StuId, header.StuId);
                            }

                            #region [MDY:20211001] M202110_01 新增 分期/分筆代碼 OldSeq (2021擴充案先做)
                            if (oldStudentReceive.YearId != header.YearId || oldStudentReceive.TermId != header.TermId
                                || oldStudentReceive.ReceiveId != header.ReceiveId || oldStudentReceive.OldSeq != header.OldSeq)
                            {
                                returnMsg = ErrorList.GetErrorMessage(ErrorList.D0003);
                                return String.Format("商家代號 {0} 學校端惟一序號 {1} 的學年、學期、費用別、分期 ({2}、{3}、{4}、{5}) 與上傳資料({6}、{7}、{8}、{9})不同"
                                    , oldStudentReceive.ReceiveType, oldStudentReceive.ServiceSeqNo
                                    , oldStudentReceive.YearId, oldStudentReceive.TermId, oldStudentReceive.ReceiveId, oldStudentReceive.OldSeq
                                    , header.YearId, header.TermId, header.ReceiveId, header.OldSeq);
                            }
                            #endregion

                            if (!String.IsNullOrWhiteSpace(oldStudentReceive.ReceiveDate) || !String.IsNullOrWhiteSpace(oldStudentReceive.AccountDate) || !String.IsNullOrWhiteSpace(oldStudentReceive.ReceiveWay))
                            {
                                if (op == BillOpList.DELETE)
                                {
                                    returnMsg = ErrorList.GetErrorMessage(ErrorList.D0005, "資料已繳款，不可刪除");
                                }
                                else
                                {
                                    returnMsg = ErrorList.GetErrorMessage(ErrorList.D0005, "資料已繳款，不可修改");
                                }
                                return String.Format("商家代號 {0} 的學校端惟一序號 {1} 的資料已繳費", header.ReceiveType, header.SeqNo);
                            }
                        }
                        else
                        {
                            returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999);
                            return String.Format("查詢學生繳費資料的學校端惟一序號資料失敗，錯誤訊息：{0}", result.Message);
                        }
                    }
                    #endregion

                    #region 如果是刪除資料就直接刪
                    if (op == BillOpList.DELETE)
                    {
                        int count = 0;
                        Result result = _Factory.Delete(oldStudentReceive, out count);
                        if (result.IsSuccess)
                        {
                            returnMsg = ErrorList.NORMAL;
                            return null;
                        }
                        else
                        {
                            returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999);
                            return String.Format("商家代號 {0} 的學校端惟一序號 {1} 的資料刪除失敗，錯誤訊息：{3}", header.ReceiveType, header.SeqNo, result.Message);
                        }
                    }
                    #endregion
                    break;
            }

            #region 取得虛擬帳號模組資料
            CancelNoHelper cnoHelper = new CancelNoHelper();
            CancelNoHelper.Module module = cnoHelper.GetModuleById(schoolRType.CancelNoRule);
            if (module == null)
            {
                returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999, "無法取得虛擬帳號模組資料");
                return String.Format("查詢商家代號 {0} 的虛擬帳號模組資料失敗", header.ReceiveType);
            }
            #endregion

            string depId = String.Empty;    //土銀不使用原有的部別欄位，改用 DeptId

            #region 取得原費用別設定
            SchoolRidEntity oldSchoolRid = null;
            {
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, header.ReceiveType)
                    .And(SchoolRidEntity.Field.YearId, header.YearId)
                    .And(SchoolRidEntity.Field.TermId, header.TermId)
                    .And(SchoolRidEntity.Field.DepId, depId)
                    .And(SchoolRidEntity.Field.ReceiveId, header.ReceiveId);
                Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out oldSchoolRid);
                if (!result.IsSuccess)
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999);
                    return String.Format("查詢費用別設定資料失敗，錯誤訊息：{0}", result.Message);
                }
            }
            #endregion

            DateTime now = DateTime.Now;

            #region 產生各種 Entity
            #region 學生資料 (StudentMasterEntity)
            StudentMasterEntity student = new StudentMasterEntity();
            student.ReceiveType = header.ReceiveType;
            student.DepId = depId;
            student.Id = header.StuId;
            student.Name = header.StuName ?? String.Empty;
            student.Birthday = header.StuBirthday == null ? String.Empty : Common.GetTWDate7(header.StuBirthday.Value);
            student.IdNumber = header.StuPId ?? String.Empty;
            student.Tel = header.StuTel ?? String.Empty;
            student.ZipCode = header.StuAddCode ?? String.Empty;
            student.Address = header.StuAddress ?? String.Empty;
            student.Email = header.StuEmail ?? String.Empty;
            student.Account = String.Empty;
            student.CrtDate = now;
            student.MdyDate = null;
            #endregion

            StudentReceiveEntity studentReceive = new StudentReceiveEntity();
            studentReceive.ReceiveType = header.ReceiveType;
            studentReceive.YearId = header.YearId;
            studentReceive.TermId = header.TermId;
            studentReceive.DepId = depId;
            studentReceive.ReceiveId = header.ReceiveId;
            studentReceive.StuId = header.StuId;

            #region [MDY:20211001] M202110_01 新增 分期/分筆代碼 OldSeq (2021擴充案先做)
            studentReceive.OldSeq = header.OldSeq;
            #endregion

            #region 連動製單服務 相關欄位
            studentReceive.ServiceSeqNo = header.SeqNo;
            studentReceive.ServiceSchUser = schUser;
            studentReceive.ServiceSchMDate = schMdyDate;
            studentReceive.ServiceSchMTime = schMdyTime;
            #endregion

            #region 學期 (TermListEntity)
            TermListEntity term = new TermListEntity();
            term.ReceiveType = header.ReceiveType;
            term.YearId = header.YearId;
            term.TermId = header.TermId;
            term.TermName = header.TermName ?? String.Empty;

            #region [MDY:20220808] 2022擴充案 學期英文名稱
            term.TermEName = isEngEnabled ? header.TermEName ?? String.Empty : null;
            #endregion

            term.Status = DataStatusCodeTexts.NORMAL;
            term.CrtDate = now;
            term.CrtUser = schUser;
            term.MdyDate = null;
            term.MdyUser = null;
            #endregion

            #region 費用別代碼 (ReceiveListEntity)
            ReceiveListEntity receive = new ReceiveListEntity();
            receive.ReceiveType = header.ReceiveType;
            receive.YearId = header.YearId;
            receive.TermId = header.TermId;
            receive.DepId = depId;
            receive.ReceiveId = header.ReceiveId;
            receive.ReceiveName = header.ReceiveName ?? String.Empty;

            #region [MDY:20220808] 2022擴充案 費用別英文名稱
            receive.ReceiveEName = isEngEnabled ? header.ReceiveEName ?? String.Empty : null;
            #endregion

            receive.Status = DataStatusCodeTexts.NORMAL;
            receive.CrtDate = now;
            receive.CrtUser = schUser;
            receive.MdyDate = null;
            receive.MdyUser = null;
            #endregion

            #region 學籍資料 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
            ClassListEntity classEntity = null;
            DeptListEntity dept = null;
            CollegeListEntity college = null;
            MajorListEntity major = null;
            {
                studentReceive.StuGrade = info.Grade ?? String.Empty;
                studentReceive.StuHid = String.Empty;

                #region 班別 ClassListEntity
                if (!String.IsNullOrEmpty(info.ClassId))
                {
                    studentReceive.ClassId = info.ClassId;

                    classEntity = new ClassListEntity();
                    classEntity.ReceiveType = header.ReceiveType;
                    classEntity.YearId = header.YearId;
                    classEntity.TermId = header.TermId;
                    classEntity.DepId = depId;
                    classEntity.ClassId = studentReceive.ClassId;
                    classEntity.ClassName = info.ClassName ?? studentReceive.ClassId;

                    #region [MDY:20220808] 2022擴充案 費用別英文名稱
                    classEntity.ClassEName = isEngEnabled ? info.ClassEName ?? classEntity.ClassName : null;
                    #endregion

                    classEntity.Status = DataStatusCodeTexts.NORMAL;
                    classEntity.CrtDate = now;
                    classEntity.CrtUser = schUser;
                    classEntity.MdyDate = null;
                    classEntity.MdyUser = null;
                }
                else
                {
                    studentReceive.ClassId = String.Empty;
                }
                #endregion

                #region 土銀專用的部別資料 DeptListEntity
                if (!String.IsNullOrEmpty(info.DeptId))
                {
                    studentReceive.DeptId = info.DeptId;

                    dept = new DeptListEntity();
                    dept.ReceiveType = header.ReceiveType;
                    dept.YearId = header.YearId;
                    dept.TermId = header.TermId;
                    dept.DeptId = studentReceive.DeptId;
                    dept.DeptName = info.DeptName ?? studentReceive.DeptId;

                    #region [MDY:20220808] 2022擴充案 部別英文名稱
                    dept.DeptEName = isEngEnabled ? info.DeptEName ?? dept.DeptName : null;
                    #endregion

                    dept.Status = DataStatusCodeTexts.NORMAL;
                    dept.CrtDate = now;
                    dept.CrtUser = schUser;
                    dept.MdyDate = null;
                    dept.MdyUser = null;
                }
                else
                {
                    studentReceive.DepId = String.Empty;
                }
                #endregion

                #region 院別資料 CollegeListEntity
                if (!String.IsNullOrEmpty(info.CollegeId))
                {
                    studentReceive.CollegeId = info.CollegeId;

                    college = new CollegeListEntity();
                    college.ReceiveType = header.ReceiveType;
                    college.YearId = header.YearId;
                    college.TermId = header.TermId;
                    college.DepId = depId;
                    college.CollegeId = studentReceive.CollegeId;
                    college.CollegeName = info.CollegeName ?? studentReceive.CollegeId;

                    #region [MDY:20220808] 2022擴充案 院別英文名稱
                    college.CollegeEName = isEngEnabled ? info.CollegeEName ?? college.CollegeName : null;
                    #endregion

                    college.Status = DataStatusCodeTexts.NORMAL;
                    college.CrtDate = now;
                    college.CrtUser = schUser;
                    college.MdyDate = null;
                    college.MdyUser = null;
                }
                else
                {
                    studentReceive.CollegeId = String.Empty;
                }
                #endregion

                #region 系別資料 MajorListEntity
                if (!String.IsNullOrEmpty(info.MajorId))
                {
                    studentReceive.MajorId = info.MajorId;

                    major = new MajorListEntity();
                    major.ReceiveType = header.ReceiveType;
                    major.YearId = header.YearId;
                    major.TermId = header.TermId;
                    major.DepId = depId;
                    major.MajorId = studentReceive.MajorId;
                    major.MajorName = info.MajorName ?? major.MajorId;

                    #region [MDY:20220808] 2022擴充案 系別英文名稱
                    major.MajorEName = isEngEnabled ? info.MajorEName ?? major.MajorName : null;
                    #endregion

                    major.Status = DataStatusCodeTexts.NORMAL;
                    major.CrtDate = now;
                    major.CrtUser = schUser;
                    major.MdyDate = null;
                    major.MdyUser = null;
                }
                else
                {
                    studentReceive.MajorId = String.Empty;
                }
                #endregion
            }
            #endregion

            #region 減免、就貸、住宿 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
            ReduceListEntity reduce = null;
            LoanListEntity loan = null;
            DormListEntity dorm = null;
            {
                #region 減免
                if (!String.IsNullOrEmpty(info.ReduceId))
                {
                    studentReceive.ReduceId = info.ReduceId;

                    reduce = new ReduceListEntity();
                    reduce.ReceiveType = header.ReceiveType;
                    reduce.YearId = header.YearId;
                    reduce.TermId = header.TermId;
                    reduce.DepId = depId;
                    reduce.ReduceId = studentReceive.ReduceId;
                    reduce.ReduceName = info.ReduceName ?? reduce.ReduceId;

                    #region [MDY:20220808] 2022擴充案 減免英文名稱
                    reduce.ReduceEName = isEngEnabled ? info.ReduceEName ?? reduce.ReduceName : null;
                    #endregion

                    reduce.Status = DataStatusCodeTexts.NORMAL;
                    reduce.CrtDate = now;
                    reduce.CrtUser = schUser;
                    reduce.MdyDate = null;
                    reduce.MdyUser = null;
                }
                else
                {
                    studentReceive.ReduceId = String.Empty;
                }
                #endregion

                #region 就貸
                //if (!String.IsNullOrEmpty(info.LoanId))
                //{
                //    studentReceive.LoanId = info.LoanId;

                //    loan = new LoanListEntity();
                //    loan.ReceiveType = header.ReceiveType;
                //    loan.YearId = header.YearId;
                //    loan.TermId = header.TermId;
                //    loan.DepId = depId;
                //    loan.LoanId = studentReceive.LoanId;
                //    loan.LoanName = info.LoanName) ?? loan.LoanId;

                //    loan.Status = DataStatusCodeTexts.NORMAL;
                //    loan.CrtDate = now;
                //    loan.CrtUser = user;
                //    loan.MdyDate = null;
                //    loan.MdyUser = null;
                //}
                //else
                //{
                //    studentReceive.LoanId = String.Empty;
                //}

                studentReceive.LoanId = String.Empty;
                #endregion

                #region 住宿
                if (!String.IsNullOrEmpty(info.DormId))
                {
                    studentReceive.DormId = info.DormId;

                    dorm = new DormListEntity();
                    dorm.ReceiveType = header.ReceiveType;
                    dorm.YearId = header.YearId;
                    dorm.TermId = header.TermId;
                    dorm.DepId = depId;
                    dorm.DormId = studentReceive.DormId;
                    dorm.DormName = info.DormName ?? studentReceive.DormId;

                    #region [MDY:20220808] 2022擴充案 住宿英文名稱
                    dorm.DormEName = isEngEnabled ? info.DormEName ?? dorm.DormName : null;
                    #endregion

                    dorm.Status = DataStatusCodeTexts.NORMAL;
                    dorm.CrtDate = now;
                    dorm.CrtUser = schUser;
                    dorm.MdyDate = null;
                    dorm.MdyUser = null;
                }
                else
                {
                    studentReceive.DormId = String.Empty;
                }
                #endregion
            }
            #endregion

            #region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
            IdentifyList1Entity identify1 = null;
            //IdentifyList2Entity identify2 = null; //連動製單目前只用一個身分註記
            //IdentifyList3Entity identify3 = null;
            //IdentifyList4Entity identify4 = null;
            //IdentifyList5Entity identify5 = null;
            //IdentifyList6Entity identify6 = null;
            {
                if (!String.IsNullOrEmpty(info.IdentifyId))
                {
                    studentReceive.IdentifyId01 = info.IdentifyId;

                    identify1 = new IdentifyList1Entity();
                    identify1.ReceiveType = header.ReceiveType;
                    identify1.YearId = header.YearId;
                    identify1.TermId = header.TermId;
                    identify1.DepId = depId;
                    identify1.IdentifyId = studentReceive.IdentifyId01;
                    identify1.IdentifyName = info.IdentifyName ?? identify1.IdentifyId;

                    #region [MDY:20220808] 2022擴充案 身分註記英文名稱
                    identify1.IdentifyEName = isEngEnabled ? info.IdentifyEName ?? identify1.IdentifyName : null;
                    #endregion

                    identify1.Status = DataStatusCodeTexts.NORMAL;
                    identify1.CrtDate = now;
                    identify1.CrtUser = schUser;
                    identify1.MdyDate = null;
                    identify1.MdyUser = null;
                }
                else
                {
                    studentReceive.IdentifyId01 = String.Empty;
                }

                studentReceive.IdentifyId02 = String.Empty;

                studentReceive.IdentifyId03 = String.Empty;

                studentReceive.IdentifyId04 = String.Empty;

                studentReceive.IdentifyId05 = String.Empty;

                studentReceive.IdentifyId06 = String.Empty;
            }
            #endregion

            #region 取得原費用別設定
            SchoolRidEntity schoolRid = new SchoolRidEntity();
            {
                schoolRid.ReceiveType = header.ReceiveType;
                schoolRid.YearId = header.YearId;
                schoolRid.TermId = header.TermId;
                schoolRid.DepId = depId;
                schoolRid.ReceiveId = header.ReceiveId;
                schoolRid.ReceiveStatus = String.Empty;

                schoolRid.PayDate = header.PayDueDate == null ? null : Common.GetTWDate7(header.PayDueDate.Value);

                schoolRid.BillValidDate = String.Empty;
                schoolRid.BillCloseDate = String.Empty;

                schoolRid.BillingType = "1";

                schoolRid.FlagRl = "1";
                schoolRid.LoanQual = "1";

                schoolRid.Brief1 = String.Empty;
                schoolRid.Brief2 = String.Empty;
                schoolRid.Brief3 = String.Empty;
                schoolRid.Brief4 = String.Empty;
                schoolRid.Brief5 = String.Empty;
                schoolRid.Brief6 = String.Empty;
                schoolRid.Hide = String.Empty;
                schoolRid.SchLevel = String.Empty;
                schoolRid.EnabledTax = "N";
                schoolRid.EduTax = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";
                schoolRid.StayTax = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";

                schoolRid.Usestamp = "N";
                schoolRid.Usewatermark = "N";
                schoolRid.Usepostdueday = String.Empty;

                #region [MDY:20220808] 2022擴充案 收入科目中文、英文名稱資料改用 BillTxnReceiveItem 承載
                #region [OLD]
                //#region 收入科目名稱
                //string[] receiveItemNames = detail.GetAllReceiveItemNames();

                //schoolRid.ReceiveItem01 = receiveItemNames[00];
                //schoolRid.ReceiveItem02 = receiveItemNames[01];
                //schoolRid.ReceiveItem03 = receiveItemNames[02];
                //schoolRid.ReceiveItem04 = receiveItemNames[03];
                //schoolRid.ReceiveItem05 = receiveItemNames[04];
                //schoolRid.ReceiveItem06 = receiveItemNames[05];
                //schoolRid.ReceiveItem07 = receiveItemNames[06];
                //schoolRid.ReceiveItem08 = receiveItemNames[07];
                //schoolRid.ReceiveItem09 = receiveItemNames[08];
                //schoolRid.ReceiveItem10 = receiveItemNames[09];

                //schoolRid.ReceiveItem11 = receiveItemNames[10];
                //schoolRid.ReceiveItem12 = receiveItemNames[11];
                //schoolRid.ReceiveItem13 = receiveItemNames[12];
                //schoolRid.ReceiveItem14 = receiveItemNames[13];
                //schoolRid.ReceiveItem15 = receiveItemNames[14];
                //schoolRid.ReceiveItem16 = receiveItemNames[15];
                //schoolRid.ReceiveItem17 = receiveItemNames[16];
                //schoolRid.ReceiveItem18 = receiveItemNames[17];
                //schoolRid.ReceiveItem19 = receiveItemNames[18];
                //schoolRid.ReceiveItem20 = receiveItemNames[19];

                //schoolRid.ReceiveItem21 = receiveItemNames[20];
                //schoolRid.ReceiveItem22 = receiveItemNames[21];
                //schoolRid.ReceiveItem23 = receiveItemNames[22];
                //schoolRid.ReceiveItem24 = receiveItemNames[23];
                //schoolRid.ReceiveItem25 = receiveItemNames[24];
                //schoolRid.ReceiveItem26 = receiveItemNames[25];
                //schoolRid.ReceiveItem27 = receiveItemNames[26];
                //schoolRid.ReceiveItem28 = receiveItemNames[27];
                //schoolRid.ReceiveItem29 = receiveItemNames[28];
                //schoolRid.ReceiveItem30 = receiveItemNames[29];

                //schoolRid.ReceiveItem31 = receiveItemNames[30];
                //schoolRid.ReceiveItem32 = receiveItemNames[31];
                //schoolRid.ReceiveItem33 = receiveItemNames[32];
                //schoolRid.ReceiveItem34 = receiveItemNames[33];
                //schoolRid.ReceiveItem35 = receiveItemNames[34];
                //schoolRid.ReceiveItem36 = receiveItemNames[35];
                //schoolRid.ReceiveItem37 = receiveItemNames[36];
                //schoolRid.ReceiveItem38 = receiveItemNames[37];
                //schoolRid.ReceiveItem39 = receiveItemNames[38];
                //schoolRid.ReceiveItem40 = receiveItemNames[39];
                //#endregion
                #endregion

                //[MEMO] 因為 schoolRid 是 new 出來的 ReceiveItem01～ReceiveItem40 本來就是 null，所以不用逐項設定
                foreach (BillTxnReceiveItem receiveItem in receiveItems)
                {
                    if (isEngEnabled)
                    {
                        schoolRid.SetReceiveItemByNo(receiveItem.No, receiveItem.ChtName, receiveItem.EngName);
                    }
                    else
                    {
                        schoolRid.SetReceiveItemChtByNo(receiveItem.No, receiveItem.ChtName);
                    }
                }
                #endregion

                #region 收入科目是否助貸旗標
                schoolRid.LoanItem01 = "N";
                schoolRid.LoanItem02 = "N";
                schoolRid.LoanItem03 = "N";
                schoolRid.LoanItem04 = "N";
                schoolRid.LoanItem05 = "N";
                schoolRid.LoanItem06 = "N";
                schoolRid.LoanItem07 = "N";
                schoolRid.LoanItem08 = "N";
                schoolRid.LoanItem09 = "N";
                schoolRid.LoanItem10 = "N";

                schoolRid.LoanItem11 = "N";
                schoolRid.LoanItem12 = "N";
                schoolRid.LoanItem13 = "N";
                schoolRid.LoanItem14 = "N";
                schoolRid.LoanItem15 = "N";
                schoolRid.LoanItem16 = "N";
                schoolRid.LoanItem17 = "N";
                schoolRid.LoanItem18 = "N";
                schoolRid.LoanItem19 = "N";
                schoolRid.LoanItem20 = "N";

                schoolRid.LoanItem21 = "N";
                schoolRid.LoanItem22 = "N";
                schoolRid.LoanItem23 = "N";
                schoolRid.LoanItem24 = "N";
                schoolRid.LoanItem25 = "N";
                schoolRid.LoanItem26 = "N";
                schoolRid.LoanItem27 = "N";
                schoolRid.LoanItem28 = "N";
                schoolRid.LoanItem29 = "N";
                schoolRid.LoanItem30 = "N";

                schoolRid.LoanItem31 = "N";
                schoolRid.LoanItem32 = "N";
                schoolRid.LoanItem33 = "N";
                schoolRid.LoanItem34 = "N";
                schoolRid.LoanItem35 = "N";
                schoolRid.LoanItem36 = "N";
                schoolRid.LoanItem37 = "N";
                schoolRid.LoanItem38 = "N";
                schoolRid.LoanItem39 = "N";
                schoolRid.LoanItem40 = "N";

                #region [MDY:20220808] 2022擴充案 對應欄位已擴充，無須此對應旗標欄位 (不再使用，固定設為 空白)
                #region [OLD]
                //schoolRid.LoanItemOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                #endregion

                schoolRid.LoanItemOthers = String.Empty;
                #endregion

                #endregion

                #region 收入科目是否代辦旗標
                schoolRid.AgencyItem01 = "N";
                schoolRid.AgencyItem02 = "N";
                schoolRid.AgencyItem03 = "N";
                schoolRid.AgencyItem04 = "N";
                schoolRid.AgencyItem05 = "N";
                schoolRid.AgencyItem06 = "N";
                schoolRid.AgencyItem07 = "N";
                schoolRid.AgencyItem08 = "N";
                schoolRid.AgencyItem09 = "N";
                schoolRid.AgencyItem10 = "N";

                schoolRid.AgencyItem11 = "N";
                schoolRid.AgencyItem12 = "N";
                schoolRid.AgencyItem13 = "N";
                schoolRid.AgencyItem14 = "N";
                schoolRid.AgencyItem15 = "N";
                schoolRid.AgencyItem16 = "N";
                schoolRid.AgencyItem17 = "N";
                schoolRid.AgencyItem18 = "N";
                schoolRid.AgencyItem19 = "N";
                schoolRid.AgencyItem20 = "N";

                schoolRid.AgencyItem21 = "N";
                schoolRid.AgencyItem22 = "N";
                schoolRid.AgencyItem23 = "N";
                schoolRid.AgencyItem24 = "N";
                schoolRid.AgencyItem25 = "N";
                schoolRid.AgencyItem26 = "N";
                schoolRid.AgencyItem27 = "N";
                schoolRid.AgencyItem28 = "N";
                schoolRid.AgencyItem29 = "N";
                schoolRid.AgencyItem30 = "N";

                schoolRid.AgencyItem31 = "N";
                schoolRid.AgencyItem32 = "N";
                schoolRid.AgencyItem33 = "N";
                schoolRid.AgencyItem34 = "N";
                schoolRid.AgencyItem35 = "N";
                schoolRid.AgencyItem36 = "N";
                schoolRid.AgencyItem37 = "N";
                schoolRid.AgencyItem38 = "N";
                schoolRid.AgencyItem39 = "N";
                schoolRid.AgencyItem40 = "N";

                #region [MDY:20220808] 2022擴充案 對應欄位已擴充，無須此對應旗標欄位 (不再使用，固定設為 空白)
                #region [OLD]
                //schoolRid.AgencyItemOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                #endregion

                schoolRid.AgencyItemOthers = String.Empty;
                #endregion
                #endregion

                #region 收入科目是否可更改旗標
                schoolRid.AgencyCheck01 = "N";
                schoolRid.AgencyCheck02 = "N";
                schoolRid.AgencyCheck03 = "N";
                schoolRid.AgencyCheck04 = "N";
                schoolRid.AgencyCheck05 = "N";
                schoolRid.AgencyCheck06 = "N";
                schoolRid.AgencyCheck07 = "N";
                schoolRid.AgencyCheck08 = "N";
                schoolRid.AgencyCheck09 = "N";
                schoolRid.AgencyCheck10 = "N";

                schoolRid.AgencyCheck11 = "N";
                schoolRid.AgencyCheck12 = "N";
                schoolRid.AgencyCheck13 = "N";
                schoolRid.AgencyCheck14 = "N";
                schoolRid.AgencyCheck15 = "N";
                schoolRid.AgencyCheck16 = "N";
                schoolRid.AgencyCheck17 = "N";
                schoolRid.AgencyCheck18 = "N";
                schoolRid.AgencyCheck19 = "N";
                schoolRid.AgencyCheck20 = "N";

                schoolRid.AgencyCheck21 = "N";
                schoolRid.AgencyCheck22 = "N";
                schoolRid.AgencyCheck23 = "N";
                schoolRid.AgencyCheck24 = "N";
                schoolRid.AgencyCheck25 = "N";
                schoolRid.AgencyCheck26 = "N";
                schoolRid.AgencyCheck27 = "N";
                schoolRid.AgencyCheck28 = "N";
                schoolRid.AgencyCheck29 = "N";
                schoolRid.AgencyCheck30 = "N";

                schoolRid.AgencyCheck31 = "N";
                schoolRid.AgencyCheck32 = "N";
                schoolRid.AgencyCheck33 = "N";
                schoolRid.AgencyCheck34 = "N";
                schoolRid.AgencyCheck35 = "N";
                schoolRid.AgencyCheck36 = "N";
                schoolRid.AgencyCheck37 = "N";
                schoolRid.AgencyCheck38 = "N";
                schoolRid.AgencyCheck39 = "N";
                schoolRid.AgencyCheck40 = "N";

                #region [MDY:20220808] 2022擴充案 對應欄位已擴充，無須此對應旗標欄位 (不再使用，固定設為 空白)
                #region [OLD]
                //schoolRid.AgencyCheckOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                #endregion

                schoolRid.AgencyCheckOthers = String.Empty;
                #endregion
                #endregion

                #region 收入科目是否教育部補助旗標
                schoolRid.Issubsidy01 = "N";
                schoolRid.Issubsidy02 = "N";
                schoolRid.Issubsidy03 = "N";
                schoolRid.Issubsidy04 = "N";
                schoolRid.Issubsidy05 = "N";
                schoolRid.Issubsidy06 = "N";
                schoolRid.Issubsidy07 = "N";
                schoolRid.Issubsidy08 = "N";
                schoolRid.Issubsidy09 = "N";
                schoolRid.Issubsidy10 = "N";

                schoolRid.Issubsidy11 = "N";
                schoolRid.Issubsidy12 = "N";
                schoolRid.Issubsidy13 = "N";
                schoolRid.Issubsidy14 = "N";
                schoolRid.Issubsidy15 = "N";
                schoolRid.Issubsidy16 = "N";
                schoolRid.Issubsidy17 = "N";
                schoolRid.Issubsidy18 = "N";
                schoolRid.Issubsidy19 = "N";
                schoolRid.Issubsidy20 = "N";

                schoolRid.Issubsidy21 = "N";
                schoolRid.Issubsidy22 = "N";
                schoolRid.Issubsidy23 = "N";
                schoolRid.Issubsidy24 = "N";
                schoolRid.Issubsidy25 = "N";
                schoolRid.Issubsidy26 = "N";
                schoolRid.Issubsidy27 = "N";
                schoolRid.Issubsidy28 = "N";
                schoolRid.Issubsidy29 = "N";
                schoolRid.Issubsidy30 = "N";

                schoolRid.Issubsidy31 = "N";
                schoolRid.Issubsidy32 = "N";
                schoolRid.Issubsidy33 = "N";
                schoolRid.Issubsidy34 = "N";
                schoolRid.Issubsidy35 = "N";
                schoolRid.Issubsidy36 = "N";
                schoolRid.Issubsidy37 = "N";
                schoolRid.Issubsidy38 = "N";
                schoolRid.Issubsidy39 = "N";
                schoolRid.Issubsidy40 = "N";

                #region [MDY:20220808] 2022擴充案 對應欄位已擴充，無須此對應旗標欄位 (不再使用，固定設為 空白)
                #region [OLD]
                //schoolRid.IssubsidyOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                #endregion

                schoolRid.IssubsidyOthers = String.Empty;
                #endregion
                #endregion
            }
            #endregion

            #region 收入科目金額 (StudentReceiveEntity)
            {
                #region [MDY:20220808] 2022擴充案 收入科目金額資料改用 BillTxnReceiveItem 承載
                #region [OLD]
                //Decimal?[] values = detail.GetReceiveItemAmounts();

                //studentReceive.Receive01 = values[0];
                //studentReceive.Receive02 = values[1];
                //studentReceive.Receive03 = values[2];
                //studentReceive.Receive04 = values[3];
                //studentReceive.Receive05 = values[4];
                //studentReceive.Receive06 = values[5];
                //studentReceive.Receive07 = values[6];
                //studentReceive.Receive08 = values[7];
                //studentReceive.Receive09 = values[8];
                //studentReceive.Receive10 = values[9];

                //studentReceive.Receive11 = values[10];
                //studentReceive.Receive12 = values[11];
                //studentReceive.Receive13 = values[12];
                //studentReceive.Receive14 = values[13];
                //studentReceive.Receive15 = values[14];
                //studentReceive.Receive16 = values[15];
                //studentReceive.Receive17 = values[16];
                //studentReceive.Receive18 = values[17];
                //studentReceive.Receive19 = values[18];
                //studentReceive.Receive20 = values[19];

                //studentReceive.Receive21 = values[20];
                //studentReceive.Receive22 = values[21];
                //studentReceive.Receive23 = values[22];
                //studentReceive.Receive24 = values[23];
                //studentReceive.Receive25 = values[24];
                //studentReceive.Receive26 = values[25];
                //studentReceive.Receive27 = values[26];
                //studentReceive.Receive28 = values[27];
                //studentReceive.Receive29 = values[28];
                //studentReceive.Receive30 = values[29];

                //studentReceive.Receive31 = values[30];
                //studentReceive.Receive32 = values[31];
                //studentReceive.Receive33 = values[32];
                //studentReceive.Receive34 = values[33];
                //studentReceive.Receive35 = values[34];
                //studentReceive.Receive36 = values[35];
                //studentReceive.Receive37 = values[36];
                //studentReceive.Receive38 = values[37];
                //studentReceive.Receive39 = values[38];
                //studentReceive.Receive40 = values[39];
                #endregion

                //[MEMO] 因為 studentReceive 是 new 出來的 Receive01～Receive40 本來就是 null，所以不用逐項設定
                foreach (BillTxnReceiveItem receiveItem in receiveItems)
                {
                    studentReceive.SetReceiveItemAmount(receiveItem.No, receiveItem.Amount);
                }
                #endregion
            }
            #endregion

            #region [MDY:20191211] 銷編流水號
            studentReceive.SeriorNo = null;
            #endregion

            #region [MDY:20191211] 新增虛擬帳號
            studentReceive.CancelNo = header.CancelNo;
            #endregion

            #region 計算應繳金額
            {
                BillAmountHelper amountHelper = new BillAmountHelper();
                if (!amountHelper.CalcBillAmount(ref studentReceive, BillAmountHelper.CalculateType.byAmount))
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.D0006);
                    return returnMsg;
                }
                if (header.ReceiveAmount != null && studentReceive.ReceiveAmount.Value != header.ReceiveAmount.Value)
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.D0006, "計算後金額與指定應繳總金額不和");
                    return returnMsg;
                }
                studentReceive.ReceiveAmount = header.ReceiveAmount;
            }
            #endregion

            #region [MDY:20191211] 上傳資料註記
            bool hasUploadAmount = studentReceive.ReceiveAmount.HasValue;             //上傳資料是否包含應繳總金額
            bool hasUploadSeriorNo = !String.IsNullOrEmpty(studentReceive.SeriorNo);  //上傳資料是否包含流水號
            bool hasUploadCancelNo = !String.IsNullOrEmpty(studentReceive.CancelNo);  //上傳資料是否包含虛擬帳號
            {
                int uploadFlag = header.ReceiveAmount.HasValue ? StudentReceiveEntity.UploadAmountFlag : 0;
                if (hasUploadSeriorNo)
                {
                    uploadFlag |= StudentReceiveEntity.UploadSeriorNoFlag;
                }
                if (hasUploadCancelNo)
                {
                    uploadFlag |= StudentReceiveEntity.UploadCancelNoFlag;
                }
                studentReceive.Uploadflag = uploadFlag.ToString();
            }
            #endregion

            #region [MDY:20160305] 繳費單繳款期限 (StudentReceiveEntity)
            studentReceive.PayDueDate = detail.PayDueDate == null ? null : Common.GetTWDate7(detail.PayDueDate.Value);
            #endregion

            #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
            studentReceive.NCCardFlag = detail.NCCardFlag;
            #endregion

            #region 其他資料 (StudentReceiveEntity)
            {
                #region 總學分數
                studentReceive.StuCredit = null;
                #endregion

                #region 上課時數
                studentReceive.StuHour = null;
                #endregion

                #region 就貸可貸金額
                studentReceive.LoanAmount = header.LoanAmount;
                #endregion
            }
            #endregion

            #region 學分基準、課程、學分數對照欄位 (StudentCourseEntity)
            StudentCourseEntity course = null;
            {
                #region 目前沒有定義上傳欄位
                //course = new StudentCourseEntity();
                //course.ReceiveType = studentReceive.ReceiveType;
                //course.YearId = studentReceive.YearId;
                //course.TermId = studentReceive.TermId;
                //course.DepId = studentReceive.DepId;
                //course.ReceiveId = studentReceive.ReceiveId;
                //course.StuId = studentReceive.StuId;

                //course.CreditId1 = creditIdValues[0];
                //course.CreditId2 = creditIdValues[1];
                //course.CreditId3 = creditIdValues[2];
                //course.CreditId4 = creditIdValues[3];
                //course.CreditId5 = creditIdValues[4];
                //course.CreditId6 = creditIdValues[5];
                //course.CreditId7 = creditIdValues[6];
                //course.CreditId8 = creditIdValues[7];
                //course.CreditId9 = creditIdValues[8];
                //course.CreditId10 = creditIdValues[9];

                //course.CreditId11 = creditIdValues[10];
                //course.CreditId12 = creditIdValues[11];
                //course.CreditId13 = creditIdValues[12];
                //course.CreditId14 = creditIdValues[13];
                //course.CreditId15 = creditIdValues[14];
                //course.CreditId16 = creditIdValues[15];
                //course.CreditId17 = creditIdValues[16];
                //course.CreditId18 = creditIdValues[17];
                //course.CreditId19 = creditIdValues[18];
                //course.CreditId20 = creditIdValues[19];

                //course.CreditId21 = creditIdValues[20];
                //course.CreditId22 = creditIdValues[21];
                //course.CreditId23 = creditIdValues[22];
                //course.CreditId24 = creditIdValues[23];
                //course.CreditId25 = creditIdValues[24];
                //course.CreditId26 = creditIdValues[25];
                //course.CreditId27 = creditIdValues[26];
                //course.CreditId28 = creditIdValues[27];
                //course.CreditId29 = creditIdValues[28];
                //course.CreditId30 = creditIdValues[29];

                //course.CreditId31 = creditIdValues[30];
                //course.CreditId32 = creditIdValues[31];
                //course.CreditId33 = creditIdValues[32];
                //course.CreditId34 = creditIdValues[33];
                //course.CreditId35 = creditIdValues[34];
                //course.CreditId36 = creditIdValues[35];
                //course.CreditId37 = creditIdValues[36];
                //course.CreditId38 = creditIdValues[37];
                //course.CreditId39 = creditIdValues[38];
                //course.CreditId40 = creditIdValues[39];

                //course.CourseId1 = courseIdValues[0];
                //course.CourseId2 = courseIdValues[1];
                //course.CourseId3 = courseIdValues[2];
                //course.CourseId4 = courseIdValues[3];
                //course.CourseId5 = courseIdValues[4];
                //course.CourseId6 = courseIdValues[5];
                //course.CourseId7 = courseIdValues[6];
                //course.CourseId8 = courseIdValues[7];
                //course.CourseId9 = courseIdValues[8];
                //course.CourseId10 = courseIdValues[9];

                //course.CourseId11 = courseIdValues[10];
                //course.CourseId12 = courseIdValues[11];
                //course.CourseId13 = courseIdValues[12];
                //course.CourseId14 = courseIdValues[13];
                //course.CourseId15 = courseIdValues[14];
                //course.CourseId16 = courseIdValues[15];
                //course.CourseId17 = courseIdValues[16];
                //course.CourseId18 = courseIdValues[17];
                //course.CourseId19 = courseIdValues[18];
                //course.CourseId20 = courseIdValues[19];

                //course.CourseId21 = courseIdValues[20];
                //course.CourseId22 = courseIdValues[21];
                //course.CourseId23 = courseIdValues[22];
                //course.CourseId24 = courseIdValues[23];
                //course.CourseId25 = courseIdValues[24];
                //course.CourseId26 = courseIdValues[25];
                //course.CourseId27 = courseIdValues[26];
                //course.CourseId28 = courseIdValues[27];
                //course.CourseId29 = courseIdValues[28];
                //course.CourseId30 = courseIdValues[29];

                //course.CourseId31 = courseIdValues[30];
                //course.CourseId32 = courseIdValues[31];
                //course.CourseId33 = courseIdValues[32];
                //course.CourseId34 = courseIdValues[33];
                //course.CourseId35 = courseIdValues[34];
                //course.CourseId36 = courseIdValues[35];
                //course.CourseId37 = courseIdValues[36];
                //course.CourseId38 = courseIdValues[37];
                //course.CourseId39 = courseIdValues[38];
                //course.CourseId40 = courseIdValues[39];

                //course.Credit1 = creditValues[0] ?? 0M;
                //course.Credit2 = creditValues[1] ?? 0M;
                //course.Credit3 = creditValues[2] ?? 0M;
                //course.Credit4 = creditValues[3] ?? 0M;
                //course.Credit5 = creditValues[4] ?? 0M;
                //course.Credit6 = creditValues[5] ?? 0M;
                //course.Credit7 = creditValues[6] ?? 0M;
                //course.Credit8 = creditValues[7] ?? 0M;
                //course.Credit9 = creditValues[8] ?? 0M;
                //course.Credit10 = creditValues[9] ?? 0M;

                //course.Credit11 = creditValues[10] ?? 0M;
                //course.Credit12 = creditValues[11] ?? 0M;
                //course.Credit13 = creditValues[12] ?? 0M;
                //course.Credit14 = creditValues[13] ?? 0M;
                //course.Credit15 = creditValues[14] ?? 0M;
                //course.Credit16 = creditValues[15] ?? 0M;
                //course.Credit17 = creditValues[16] ?? 0M;
                //course.Credit18 = creditValues[17] ?? 0M;
                //course.Credit19 = creditValues[18] ?? 0M;
                //course.Credit20 = creditValues[19] ?? 0M;

                //course.Credit21 = creditValues[20] ?? 0M;
                //course.Credit22 = creditValues[21] ?? 0M;
                //course.Credit23 = creditValues[22] ?? 0M;
                //course.Credit24 = creditValues[23] ?? 0M;
                //course.Credit25 = creditValues[24] ?? 0M;
                //course.Credit26 = creditValues[25] ?? 0M;
                //course.Credit27 = creditValues[26] ?? 0M;
                //course.Credit28 = creditValues[27] ?? 0M;
                //course.Credit29 = creditValues[28] ?? 0M;
                //course.Credit30 = creditValues[29] ?? 0M;

                //course.Credit31 = creditValues[30] ?? 0M;
                //course.Credit32 = creditValues[31] ?? 0M;
                //course.Credit33 = creditValues[32] ?? 0M;
                //course.Credit34 = creditValues[33] ?? 0M;
                //course.Credit35 = creditValues[34] ?? 0M;
                //course.Credit36 = creditValues[35] ?? 0M;
                //course.Credit37 = creditValues[36] ?? 0M;
                //course.Credit38 = creditValues[37] ?? 0M;
                //course.Credit39 = creditValues[38] ?? 0M;
                //course.Credit40 = creditValues[39] ?? 0M;
                #endregion
            }
            #endregion

            #region Remark (StudentReceiveEntity)
            {
                studentReceive.Remark = String.Empty;
            }
            #endregion

            #region 扣款資料 (StudentReceiveEntity)
            {
                studentReceive.DeductBankid = String.Empty;
                studentReceive.DeductAccountno = String.Empty;
                studentReceive.DeductAccountname = String.Empty;
                studentReceive.DeductAccountid = String.Empty;
            }
            #endregion

            #region 備註 (StudentReceiveEntity)
            {
                studentReceive.Memo01 = String.Empty;
                studentReceive.Memo02 = String.Empty;
                studentReceive.Memo03 = String.Empty;
                studentReceive.Memo04 = String.Empty;
                studentReceive.Memo05 = String.Empty;
                studentReceive.Memo06 = String.Empty;
                studentReceive.Memo07 = String.Empty;
                studentReceive.Memo08 = String.Empty;
                studentReceive.Memo09 = String.Empty;
                studentReceive.Memo10 = String.Empty;
            }
            #endregion

            #region [OLD:20191211]
            //bool hasUploadAmount = header.ReceiveAmount != null;    //上傳資料是否包含應繳總金額
            //bool hasUploadSeriorNo = false;     //上傳資料是否包含流水號
            //bool hasUploadCancelNo = false;     //上傳資料是否包含虛擬帳號
            #endregion

            #region 虛擬帳號相關 (StudentReceiveEntity)
            {
                #region [MDY:20191211] 新增虛擬帳號
                #region [OLD]
                //#region [MEMO] 處理邏輯
                ////目前服務沒有上傳虛擬帳號，但資料可能是透過整批上傳的方式寫入
                ////規格又沒註明已有虛擬帳號時是否可更新，D38處理規則
                ////這裡先定義以下的處理邏輯
                ////1. 新增時自動產生虛擬帳號
                ////2. 修改有虛擬帳號的資料(未繳款)時，因為服務不能上傳虛擬帳號，所以修改資料時使用原流水號與虛擬帳號處理
                ////   a. 屬 D38 則，使用舊虛擬帳號不更新
                ////   b. 非 D38 且虛擬帳號由使用者上傳則，更新減碼
                ////   c. 非 D38 且虛擬帳號由系統產生則，重新產生虛擬帳號
                ////3. 修改無虛擬帳號的資料(未繳款)時，自動更新虛擬帳號
                //#endregion

                //#region 產生銷帳編號
                //#region [Old]
                ////if (studentReceive.ReceiveAmount != null && studentReceive.ReceiveAmount > 0)
                ////{
                ////    //金額大於 0 才要產生虛擬帳號
                ////    if (!String.IsNullOrEmpty(oldStudentReceive.SeriorNo))
                ////    {
                ////        //有流水號
                ////        studentReceive.SeriorNo = oldStudentReceive.SeriorNo;
                ////    }
                ////    else
                ////    {
                ////        #region 取得流水號
                ////        SNoHelper snohelper = new SNoHelper();
                ////        Int64 maxSeriorNo = module.GetMaxSeriorNo(schoolRType.IsBigReceiveId());
                ////        string snoKey = snohelper.GetKeyForStudentReceiveSeriorNo(header.ReceiveType, header.YearId, header.TermId, depId, header.ReceiveId);
                ////        Int64 nextSNo = snohelper.GetNextSNo(_Factory, snoKey, maxSeriorNo, false);
                ////        if (nextSNo > maxSeriorNo)
                ////        {
                ////            returnMsg = ErrorList.GetErrorMessage(ErrorList.D0007, String.Format("流水號已超過此業務別的最大值 {0} 的限制", maxSeriorNo));
                ////            return returnMsg;
                ////        }
                ////        else if (nextSNo == 0)
                ////        {
                ////            returnMsg = ErrorList.GetErrorMessage(ErrorList.D0007, String.Format("無法取得 {0} 的下一個流水號", snoKey));
                ////            return returnMsg;
                ////        }
                ////        else
                ////        {
                ////            studentReceive.SeriorNo = nextSNo.ToString().PadLeft(module.SeriorNoSize, '0');
                ////        }
                ////        #endregion
                ////    }

                ////    string cancelNo = null;
                ////    string customNo = null;
                ////    string checksum = null;
                ////    string errmsg = cnoHelper.TryGenCancelNo(module, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.ReceiveId, studentReceive.SeriorNo, schoolRType.IsBigReceiveId(), studentReceive.ReceiveSmamount.Value, out cancelNo, out customNo, out checksum);
                ////    if (!String.IsNullOrEmpty(errmsg))
                ////    {
                ////        returnMsg = ErrorList.GetErrorMessage(ErrorList.D0007, errmsg);
                ////        return returnMsg;
                ////    }
                ////    studentReceive.CancelNo = cancelNo;
                ////    //if (oldStudentReceive != null && oldStudentReceive.CancelNo != studentReceive.CancelNo)
                ////    //{
                ////    //    studentReceive.CFlag = "0";
                ////    //}
                ////}
                //#endregion

                //#region [New]
                //bool changeChecksum = true;
                //string cancelNo = null;
                //string checksum = null;
                //string seriorNo = null;
                //string atmCancelNo = null;
                //string smCancelNo = null;
                //if (oldStudentReceive == null)
                //{
                //    //新增
                //    studentReceive.SeriorNo = null;
                //    studentReceive.CancelNo = null;
                //    string errmsg = cnoHelper.TryGenCancelNo(_Factory, module, studentReceive, schoolRType.IsBigReceiveId(), out cancelNo, out seriorNo, out checksum, out atmCancelNo, out smCancelNo, changeChecksum);
                //    if (String.IsNullOrEmpty(errmsg))
                //    {
                //        studentReceive.SeriorNo = seriorNo;
                //        studentReceive.CancelNo = cancelNo;
                //        studentReceive.CancelAtmno = atmCancelNo;
                //        studentReceive.CancelSmno = smCancelNo;
                //    }
                //    else
                //    {
                //        returnMsg = ErrorList.GetErrorMessage(ErrorList.D0007, "無法產生虛擬帳號");
                //        return String.Format("無法產生虛擬帳號 (學校端惟一序號={0})，錯誤訊息：{1}", header.SeqNo, errmsg);
                //    }

                //}
                //else
                //{
                //    //修改
                //    hasUploadSeriorNo = studentReceive.HasUploadSeriorNo();
                //    hasUploadCancelNo = studentReceive.HasUploadCancelNo();

                //    studentReceive.SeriorNo = oldStudentReceive.SeriorNo;
                //    studentReceive.CancelNo = oldStudentReceive.CancelNo;
                //    studentReceive.Uploadflag = oldStudentReceive.Uploadflag;   //這裡要先將舊資料的Uploadflag指定到新資料
                //    string errmsg = cnoHelper.TryGenCancelNo(_Factory, module, studentReceive, schoolRType.IsBigReceiveId(), out cancelNo, out seriorNo, out checksum, out atmCancelNo, out smCancelNo, changeChecksum);
                //    if (String.IsNullOrEmpty(errmsg))
                //    {
                //        studentReceive.SeriorNo = seriorNo;
                //        studentReceive.CancelNo = cancelNo;
                //        studentReceive.CancelAtmno = atmCancelNo;
                //        studentReceive.CancelSmno = smCancelNo;

                //        //[MEMO] 這裡比照就貸，更新使用者上傳的虛擬帳號的檢碼仍視為使用者上傳，所以 Mark
                //        //if (hasUploadSeriorNo && studentReceive.SeriorNo != oldStudentReceive.SeriorNo)
                //        //{
                //        //    hasUploadSeriorNo = false;
                //        //}
                //        //if (hasUploadCancelNo && studentReceive.CancelNo != oldStudentReceive.CancelNo)
                //        //{
                //        //    hasUploadCancelNo = false;
                //        //}
                //    }
                //    else
                //    {
                //        returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999, "無法產生虛擬帳號");
                //        return String.Format("無法產生虛擬帳號 (學校端惟一序號={0})，錯誤訊息：{1}", header.SeqNo, errmsg);
                //    }
                //}
                //#endregion

                //#endregion
                #endregion

                #region [MEMO] 處理邏輯
                //新增上傳虛擬帳號，另外虛擬帳號可能透過整批上傳的方式寫入
                //規格沒註明已有虛擬帳號時是否可更新與D38處理規則
                //定義以下的處理邏輯
                //A. 未上傳虛擬帳號
                //  A1 新增時由系統自動取流水號，自動產生虛擬帳號
                //  A2 修改有虛擬帳號的資料(未繳款)時，因未上傳虛擬帳號，所以使用原流水號與虛擬帳號處理
                //    a. 屬 D38 則，使用舊虛擬帳號不更新
                //    b. 非 D38 且虛擬帳號由使用者上傳則，更新檢查碼
                //    c. 非 D38 且虛擬帳號由系統產生則，重新產生虛擬帳號
                //  A3 修改無虛擬帳號的資料(未繳款)時，自動更新虛擬帳號
                //B. 有上傳虛擬帳號
                //  B0 檢核上傳虛擬帳號是否正確
                //  B1 新增時依據上傳虛擬帳號，直接用上傳虛擬帳號，產生臨櫃、超商的虛擬帳號
                //  B2 修改有虛擬帳號的資料(未繳款)時，因有上傳虛擬帳號，所以使用上傳虛擬帳號處理
                //    a. 屬 D38 則，已上傳 D38 則不允許虛擬帳號異動，否則直接用上傳虛擬帳號更新
                //    b. 非 D38 則，直接用上傳虛擬帳號更新
                //  B3 修改無虛擬帳號的資料(未繳款)時，直接用上傳虛擬帳號更新
                #endregion

                if (hasUploadCancelNo)
                {
                    #region B. 有上傳虛擬帳號
                    #region B0 檢核上傳虛擬帳號是否正確
                    {
                        #region [MDY:20200810] M202008_02 Fix
                        #region [OLD]
                        //string errmsg = cnoHelper.CheckCustomCancelNo(studentReceive.CancelNo, module, studentReceive.CancelNo, studentReceive.ReceiveAmount.Value);
                        #endregion

                        string errmsg = cnoHelper.CheckCustomCancelNo(studentReceive.CancelNo, module, (oldStudentReceive == null ? null : oldStudentReceive.CancelNo), studentReceive.ReceiveAmount.Value);
                        #endregion

                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            returnMsg = ErrorList.GetErrorMessage(ErrorList.D0007, String.Format("上傳虛擬帳號不正確，{0}", errmsg));
                            return returnMsg;
                        }
                    }
                    #endregion

                    string cancelNo = null;
                    if (op == BillOpList.INSERT)
                    {
                        #region B1 新增（直接用上傳虛擬帳號，產生臨櫃、超商的虛擬帳號）
                        cancelNo = studentReceive.CancelNo;
                        if (studentReceive.ReceiveAtmamount.HasValue && studentReceive.ReceiveAtmamount.Value > 0)
                        {
                            studentReceive.CancelAtmno = cancelNo;
                        }
                        else
                        {
                            studentReceive.CancelAtmno = String.Empty;
                        }
                        if (studentReceive.ReceiveSmamount.HasValue && studentReceive.ReceiveSmamount.Value > 0)
                        {
                            studentReceive.CancelSmno = cancelNo;
                        }
                        else
                        {
                            studentReceive.CancelSmno = String.Empty;
                        }
                        #endregion
                    }
                    else
                    {
                        #region B2/B3 修改 （使用上傳虛擬帳號處理）
                        if (!String.IsNullOrEmpty(oldStudentReceive.CancelNo)
                            && module.IsD38Kind
                            && oldStudentReceive.Exportreceivedata == "Y"
                            && oldStudentReceive.CancelNo != studentReceive.CancelNo)
                        {
                            returnMsg = ErrorList.GetErrorMessage(ErrorList.D0007, "無法更新虛擬帳號，該虛擬帳號以上傳至中心");
                            return returnMsg;
                        }

                        studentReceive.SeriorNo = oldStudentReceive.SeriorNo;  //保留原流水號？？

                        cancelNo = studentReceive.CancelNo;
                        if (studentReceive.ReceiveAtmamount.HasValue && studentReceive.ReceiveAtmamount.Value > 0)
                        {
                            studentReceive.CancelAtmno = cancelNo;
                        }
                        else
                        {
                            studentReceive.CancelAtmno = String.Empty;
                        }
                        if (studentReceive.ReceiveSmamount.HasValue && studentReceive.ReceiveSmamount.Value > 0)
                        {
                            studentReceive.CancelSmno = cancelNo;
                        }
                        else
                        {
                            studentReceive.CancelSmno = String.Empty;
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region A. 未上傳虛擬帳號
                    bool changeChecksum = true;
                    string cancelNo = null;
                    string checksum = null;
                    string seriorNo = null;
                    string atmCancelNo = null;
                    string smCancelNo = null;
                    if (op == BillOpList.INSERT)
                    {
                        #region A1 新增 （由系統自動取流水號，自動產生虛擬帳號）
                        string errmsg = cnoHelper.TryGenCancelNo(_Factory, module, studentReceive, schoolRType.IsBigReceiveId(), out cancelNo, out seriorNo, out checksum, out atmCancelNo, out smCancelNo, changeChecksum);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            studentReceive.SeriorNo = seriorNo;
                            studentReceive.CancelNo = cancelNo;
                            studentReceive.CancelAtmno = atmCancelNo;
                            studentReceive.CancelSmno = smCancelNo;
                        }
                        else
                        {
                            returnMsg = ErrorList.GetErrorMessage(ErrorList.D0007, String.Format("無法產生虛擬帳號，{0}", errmsg));
                            return returnMsg;
                        }
                        #endregion
                    }
                    else
                    {
                        #region A2/A3 修改 （使用原流水號與虛擬帳號處理）
                        studentReceive.SeriorNo = oldStudentReceive.SeriorNo;
                        studentReceive.CancelNo = oldStudentReceive.CancelNo;
                        studentReceive.Uploadflag = oldStudentReceive.Uploadflag;   //這裡要先將舊資料的Uploadflag指定到新資料
                        string errmsg = cnoHelper.TryGenCancelNo(_Factory, module, studentReceive, schoolRType.IsBigReceiveId(), out cancelNo, out seriorNo, out checksum, out atmCancelNo, out smCancelNo, changeChecksum);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            studentReceive.SeriorNo = seriorNo;
                            studentReceive.CancelNo = cancelNo;
                            studentReceive.CancelAtmno = atmCancelNo;
                            studentReceive.CancelSmno = smCancelNo;

                            //[MEMO] 這裡比照就貸，更新使用者上傳的虛擬帳號的檢碼仍視為使用者上傳，所以 Mark
                            //if (hasUploadSeriorNo && studentReceive.SeriorNo != oldStudentReceive.SeriorNo)
                            //{
                            //    hasUploadSeriorNo = false;
                            //}
                            //if (hasUploadCancelNo && studentReceive.CancelNo != oldStudentReceive.CancelNo)
                            //{
                            //    hasUploadCancelNo = false;
                            //}
                        }
                        else
                        {
                            returnMsg = ErrorList.GetErrorMessage(ErrorList.D0007, String.Format("無法產生虛擬帳號，{0}", errmsg));
                            return returnMsg;
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

            #region [OLD:20191211] 上傳資料註記
            {
                int uploadFlag = hasUploadAmount ? StudentReceiveEntity.UploadAmountFlag : 0;
                if (hasUploadSeriorNo)
                {
                    uploadFlag |= StudentReceiveEntity.UploadSeriorNoFlag;
                }
                if (hasUploadCancelNo)
                {
                    uploadFlag |= StudentReceiveEntity.UploadCancelNoFlag;
                }
                studentReceive.Uploadflag = uploadFlag.ToString();
            }
            #endregion

            studentReceive.UpNo = "0";  //因為服務沒有批號，所以使用 0
            studentReceive.UpOrder = String.Empty;
            studentReceive.MappingId = String.Empty;
            studentReceive.MappingType = String.Empty;
            studentReceive.Exportreceivedata = "N";

            //[TODO]
            studentReceive.BillingType = "2";

            studentReceive.CreateDate = now;
            //receive.UpdateDate = now;
            #endregion

            #region 寫入資料庫
            {
                int count = 0;
                Result result = new Result(true);

                switch (op)
                {
                    case BillOpList.INSERT:
                    case BillOpList.UPDATE:
                        {
                            using (EntityFactory tsFactory = _Factory.IsUseTransaction ? _Factory : _Factory.CloneForTransaction())
                            {
                                #region StudentReceiveEntity
                                if (oldStudentReceive == null)
                                {
                                    #region Insert StudentReceiveEntity
                                    result = tsFactory.Insert(studentReceive, out count);
                                    #endregion
                                }
                                else
                                {
                                    #region Update StudentReceiveEntity
                                    Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, oldStudentReceive.ReceiveType)
                                        .And(StudentReceiveEntity.Field.YearId, oldStudentReceive.YearId)
                                        .And(StudentReceiveEntity.Field.TermId, oldStudentReceive.TermId)
                                        .And(StudentReceiveEntity.Field.DepId, oldStudentReceive.DepId)
                                        .And(StudentReceiveEntity.Field.ReceiveId, oldStudentReceive.ReceiveId)
                                        .And(StudentReceiveEntity.Field.StuId, oldStudentReceive.StuId)
                                        .And(StudentReceiveEntity.Field.OldSeq, oldStudentReceive.OldSeq)
                                        .And(StudentReceiveEntity.Field.ServiceSeqNo, header.SeqNo);

                                    KeyValueList fieldValues = new KeyValueList();

                                    #region 連動製單服務 相關欄位
                                    fieldValues.Add(StudentReceiveEntity.Field.ServiceSchUser, studentReceive.ServiceSchUser);
                                    fieldValues.Add(StudentReceiveEntity.Field.ServiceSchMDate, studentReceive.ServiceSchMDate);
                                    fieldValues.Add(StudentReceiveEntity.Field.ServiceSchMTime, studentReceive.ServiceSchMTime);
                                    #endregion

                                    #region 學籍資料
                                    fieldValues.Add(StudentReceiveEntity.Field.StuGrade, studentReceive.StuGrade);
                                    fieldValues.Add(StudentReceiveEntity.Field.StuHid, studentReceive.StuHid);

                                    fieldValues.Add(StudentReceiveEntity.Field.ClassId, studentReceive.ClassId);
                                    fieldValues.Add(StudentReceiveEntity.Field.DeptId, studentReceive.DeptId);
                                    fieldValues.Add(StudentReceiveEntity.Field.CollegeId, studentReceive.CollegeId);
                                    fieldValues.Add(StudentReceiveEntity.Field.MajorId, studentReceive.MajorId);
                                    #endregion

                                    #region 減免、就貸、住宿
                                    fieldValues.Add(StudentReceiveEntity.Field.ReduceId, studentReceive.ReduceId);
                                    fieldValues.Add(StudentReceiveEntity.Field.LoanId, studentReceive.LoanId);
                                    fieldValues.Add(StudentReceiveEntity.Field.DormId, studentReceive.DormId);
                                    #endregion

                                    #region 身分註記
                                    fieldValues.Add(StudentReceiveEntity.Field.IdentifyId01, studentReceive.IdentifyId01);
                                    fieldValues.Add(StudentReceiveEntity.Field.IdentifyId02, studentReceive.IdentifyId02);
                                    fieldValues.Add(StudentReceiveEntity.Field.IdentifyId03, studentReceive.IdentifyId03);
                                    fieldValues.Add(StudentReceiveEntity.Field.IdentifyId04, studentReceive.IdentifyId04);
                                    fieldValues.Add(StudentReceiveEntity.Field.IdentifyId05, studentReceive.IdentifyId05);
                                    fieldValues.Add(StudentReceiveEntity.Field.IdentifyId06, studentReceive.IdentifyId06);
                                    #endregion

                                    #region 收入科目金額
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive01, studentReceive.Receive01);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive02, studentReceive.Receive02);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive03, studentReceive.Receive03);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive04, studentReceive.Receive04);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive05, studentReceive.Receive05);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive06, studentReceive.Receive06);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive07, studentReceive.Receive07);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive08, studentReceive.Receive08);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive09, studentReceive.Receive09);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive10, studentReceive.Receive10);

                                    fieldValues.Add(StudentReceiveEntity.Field.Receive11, studentReceive.Receive11);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive12, studentReceive.Receive12);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive13, studentReceive.Receive13);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive14, studentReceive.Receive14);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive15, studentReceive.Receive15);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive16, studentReceive.Receive16);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive17, studentReceive.Receive17);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive18, studentReceive.Receive18);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive19, studentReceive.Receive19);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive20, studentReceive.Receive20);

                                    fieldValues.Add(StudentReceiveEntity.Field.Receive21, studentReceive.Receive21);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive22, studentReceive.Receive22);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive23, studentReceive.Receive23);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive24, studentReceive.Receive24);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive25, studentReceive.Receive25);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive26, studentReceive.Receive26);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive27, studentReceive.Receive27);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive28, studentReceive.Receive28);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive29, studentReceive.Receive29);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive30, studentReceive.Receive30);

                                    fieldValues.Add(StudentReceiveEntity.Field.Receive31, studentReceive.Receive31);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive32, studentReceive.Receive32);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive33, studentReceive.Receive33);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive34, studentReceive.Receive34);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive35, studentReceive.Receive35);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive36, studentReceive.Receive36);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive37, studentReceive.Receive37);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive38, studentReceive.Receive38);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive39, studentReceive.Receive39);
                                    fieldValues.Add(StudentReceiveEntity.Field.Receive40, studentReceive.Receive40);
                                    #endregion

                                    #region 應繳金額相關
                                    fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, studentReceive.ReceiveAmount);

                                    fieldValues.Add(StudentReceiveEntity.Field.ReceiveAtmamount, studentReceive.ReceiveAtmamount);
                                    fieldValues.Add(StudentReceiveEntity.Field.ReceiveSmamount, studentReceive.ReceiveSmamount);
                                    #endregion

                                    #region [MDY:20160305] 繳費單繳款期限 (StudentReceiveEntity)
                                    fieldValues.Add(StudentReceiveEntity.Field.PayDueDate, studentReceive.PayDueDate);
                                    #endregion

                                    #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
                                    fieldValues.Add(StudentReceiveEntity.Field.NCCardFlag, studentReceive.NCCardFlag);
                                    #endregion

                                    #region 其他資料
                                    fieldValues.Add(StudentReceiveEntity.Field.StuCredit, studentReceive.StuCredit);
                                    fieldValues.Add(StudentReceiveEntity.Field.StuHour, studentReceive.StuHour);
                                    fieldValues.Add(StudentReceiveEntity.Field.LoanAmount, studentReceive.LoanAmount);
                                    #endregion

                                    fieldValues.Add(StudentReceiveEntity.Field.Remark, studentReceive.Remark);

                                    #region 扣款資料
                                    fieldValues.Add(StudentReceiveEntity.Field.DeductBankid, studentReceive.DeductBankid);
                                    fieldValues.Add(StudentReceiveEntity.Field.DeductAccountno, studentReceive.DeductAccountno);
                                    fieldValues.Add(StudentReceiveEntity.Field.DeductAccountname, studentReceive.DeductAccountname);
                                    fieldValues.Add(StudentReceiveEntity.Field.DeductAccountid, studentReceive.DeductAccountid);
                                    #endregion

                                    #region 備註
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo01, studentReceive.Memo01);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo02, studentReceive.Memo02);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo03, studentReceive.Memo03);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo04, studentReceive.Memo04);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo05, studentReceive.Memo05);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo06, studentReceive.Memo06);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo07, studentReceive.Memo07);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo08, studentReceive.Memo08);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo09, studentReceive.Memo09);
                                    fieldValues.Add(StudentReceiveEntity.Field.Memo10, studentReceive.Memo10);
                                    #endregion

                                    #region 虛擬帳號相關
                                    fieldValues.Add(StudentReceiveEntity.Field.SeriorNo, studentReceive.SeriorNo);
                                    fieldValues.Add(StudentReceiveEntity.Field.CancelNo, studentReceive.CancelNo);

                                    fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, studentReceive.CancelAtmno);
                                    fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, studentReceive.CancelSmno);
                                    #endregion

                                    #region 上傳資料註記
                                    fieldValues.Add(StudentReceiveEntity.Field.Uploadflag, studentReceive.Uploadflag);
                                    #endregion

                                    #region 有異動金額或虛擬帳號，中信資料發送旗標清為 0
                                    if (oldStudentReceive != null && oldStudentReceive.CFlag != "0"
                                        && (studentReceive.CancelNo != oldStudentReceive.CancelNo || studentReceive.ReceiveAmount != oldStudentReceive.ReceiveAmount))
                                    {
                                        studentReceive.CFlag = "0";
                                        fieldValues.Add(StudentReceiveEntity.Field.CFlag, studentReceive.CFlag);
                                    }
                                    #endregion

                                    fieldValues.Add(StudentReceiveEntity.Field.MappingId, studentReceive.MappingId);
                                    fieldValues.Add(StudentReceiveEntity.Field.MappingType, studentReceive.MappingType);

                                    #region D38資料發送旗標 (統一更新資料時不覆蓋此欄位，因為沒有制定邏輯)
                                    //[MEMO] 這些都會造成不知道怎麼處理
                                    //1. D38資料似乎是以虛擬帳號為 Key，所以修改、刪除都需要原虛擬帳號
                                    //2. 虛擬帳號可能因為修改金額、修改就貸、修改教育部補助、重新上傳虛擬帳號等改變
                                    //3. 影響虛擬帳號的動作可能不會一次做完
                                    //4. 那些資料修改後要重傳

                                    //fieldValues.Add(StudentReceiveEntity.Field.Exportreceivedata, receive.Exportreceivedata);
                                    #endregion

                                    fieldValues.Add(StudentReceiveEntity.Field.UpdateDate, now);

                                    result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                                    if (result.IsSuccess && count == 0)
                                    {
                                        result = new Result(false, "學生繳費資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                    }
                                    #endregion
                                }
                                #endregion

                                #region StudentMasterEntity
                                if (result.IsSuccess)
                                {
                                    Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, student.ReceiveType)
                                        .And(StudentMasterEntity.Field.DepId, student.DepId)
                                        .And(StudentMasterEntity.Field.Id, student.Id);
                                    result = tsFactory.SelectCount<StudentMasterEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(student, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學生資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(StudentMasterEntity.Field.Name, student.Name);
                                            fieldValues.Add(StudentMasterEntity.Field.Birthday, student.Birthday);
                                            fieldValues.Add(StudentMasterEntity.Field.IdNumber, student.IdNumber);
                                            fieldValues.Add(StudentMasterEntity.Field.Tel, student.Tel);
                                            fieldValues.Add(StudentMasterEntity.Field.ZipCode, student.ZipCode);
                                            fieldValues.Add(StudentMasterEntity.Field.Address, student.Address);
                                            fieldValues.Add(StudentMasterEntity.Field.Email, student.Email);
                                            fieldValues.Add(StudentMasterEntity.Field.Account, student.Account);

                                            fieldValues.Add(StudentMasterEntity.Field.MdyDate, now);

                                            result = tsFactory.UpdateFields<StudentMasterEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學生資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                #region TermListEntity
                                if (result.IsSuccess)
                                {
                                    TermListEntity oldTerm = null;
                                    Expression where = new Expression(TermListEntity.Field.ReceiveType, term.ReceiveType)
                                        .And(TermListEntity.Field.YearId, term.YearId)
                                        .And(TermListEntity.Field.TermId, term.TermId);
                                    result = tsFactory.SelectFirst<TermListEntity>(where, null, out oldTerm);
                                    if (result.IsSuccess)
                                    {
                                        if (oldTerm == null)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(term, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學期代碼資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            if (oldTerm.TermName != term.TermName
                                                || oldTerm.TermEName != term.TermEName)
                                            {
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(TermListEntity.Field.TermName, term.TermName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(TermListEntity.Field.TermEName, term.TermEName);
                                                #endregion

                                                fieldValues.Add(TermListEntity.Field.MdyDate, term.CrtDate);
                                                fieldValues.Add(TermListEntity.Field.MdyUser, term.CrtUser);

                                                result = tsFactory.UpdateFields<TermListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "學期代碼資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                #region ReceiveListEntity
                                if (result.IsSuccess)
                                {
                                    ReceiveListEntity oldReceive = null;
                                    Expression where = new Expression(ReceiveListEntity.Field.ReceiveType, receive.ReceiveType)
                                        .And(ReceiveListEntity.Field.YearId, receive.YearId)
                                        .And(ReceiveListEntity.Field.TermId, receive.TermId)
                                        .And(ReceiveListEntity.Field.DepId, receive.DepId)
                                        .And(ReceiveListEntity.Field.ReceiveId, receive.ReceiveId);
                                    result = tsFactory.SelectFirst<ReceiveListEntity>(where, null, out oldReceive);
                                    if (result.IsSuccess)
                                    {
                                        if (oldReceive == null)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(receive, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "費用別代碼資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            if (oldReceive.ReceiveName != receive.ReceiveName
                                                || oldReceive.ReceiveEName != receive.ReceiveEName)
                                            {
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(ReceiveListEntity.Field.ReceiveName, receive.ReceiveName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(ReceiveListEntity.Field.ReceiveEName, receive.ReceiveEName);
                                                #endregion

                                                fieldValues.Add(ReceiveListEntity.Field.MdyDate, receive.CrtDate);
                                                fieldValues.Add(ReceiveListEntity.Field.MdyUser, receive.CrtUser);

                                                result = tsFactory.UpdateFields<ReceiveListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "費用別代碼資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                #region SchoolRidEntity
                                if (result.IsSuccess)
                                {
                                    if (oldSchoolRid == null)
                                    {
                                        #region Insert
                                        result = tsFactory.Insert(schoolRid, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "費用別設定資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Update
                                        Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, oldSchoolRid.ReceiveType)
                                            .And(SchoolRidEntity.Field.YearId, oldSchoolRid.YearId)
                                            .And(SchoolRidEntity.Field.TermId, oldSchoolRid.TermId)
                                            .And(SchoolRidEntity.Field.DepId, oldSchoolRid.DepId)
                                            .And(SchoolRidEntity.Field.ReceiveId, oldSchoolRid.ReceiveId);

                                        KeyValueList fieldValues = new KeyValueList();

                                        #region 繳款期限
                                        if (oldSchoolRid.PayDate != schoolRid.PayDate)
                                        {
                                            fieldValues.Add(SchoolRidEntity.Field.PayDate, schoolRid.PayDate);
                                        }
                                        #endregion

                                        #region 收入科目名稱
                                        {
                                            string[] fields = new string[] {
                                                SchoolRidEntity.Field.ReceiveItem01, SchoolRidEntity.Field.ReceiveItem02, SchoolRidEntity.Field.ReceiveItem03, SchoolRidEntity.Field.ReceiveItem04, SchoolRidEntity.Field.ReceiveItem05,
                                                SchoolRidEntity.Field.ReceiveItem06, SchoolRidEntity.Field.ReceiveItem07, SchoolRidEntity.Field.ReceiveItem08, SchoolRidEntity.Field.ReceiveItem09, SchoolRidEntity.Field.ReceiveItem10,
                                                SchoolRidEntity.Field.ReceiveItem11, SchoolRidEntity.Field.ReceiveItem12, SchoolRidEntity.Field.ReceiveItem13, SchoolRidEntity.Field.ReceiveItem14, SchoolRidEntity.Field.ReceiveItem15,
                                                SchoolRidEntity.Field.ReceiveItem16, SchoolRidEntity.Field.ReceiveItem17, SchoolRidEntity.Field.ReceiveItem18, SchoolRidEntity.Field.ReceiveItem19, SchoolRidEntity.Field.ReceiveItem20,
                                                SchoolRidEntity.Field.ReceiveItem21, SchoolRidEntity.Field.ReceiveItem22, SchoolRidEntity.Field.ReceiveItem23, SchoolRidEntity.Field.ReceiveItem24, SchoolRidEntity.Field.ReceiveItem25,
                                                SchoolRidEntity.Field.ReceiveItem26, SchoolRidEntity.Field.ReceiveItem27, SchoolRidEntity.Field.ReceiveItem28, SchoolRidEntity.Field.ReceiveItem29, SchoolRidEntity.Field.ReceiveItem30,
                                                SchoolRidEntity.Field.ReceiveItem31, SchoolRidEntity.Field.ReceiveItem32, SchoolRidEntity.Field.ReceiveItem33, SchoolRidEntity.Field.ReceiveItem34, SchoolRidEntity.Field.ReceiveItem35,
                                                SchoolRidEntity.Field.ReceiveItem36, SchoolRidEntity.Field.ReceiveItem37, SchoolRidEntity.Field.ReceiveItem38, SchoolRidEntity.Field.ReceiveItem39, SchoolRidEntity.Field.ReceiveItem40
                                            };

                                            #region [MDY:20220808] 2022擴充案 收入科目 改寫，改用 GetAllReceiveItemChts()
                                            #region [OLD]
                                            //string[] oldItemNames = oldSchoolRid.GetAllReceiveItems();
                                            //string[] newItemNames = schoolRid.GetAllReceiveItems();
                                            #endregion

                                            string[] oldItemNames = oldSchoolRid.GetAllReceiveItemChts();
                                            string[] newItemNames = schoolRid.GetAllReceiveItemChts();
                                            #endregion

                                            for (int idx = 0; idx < 40; idx++)
                                            {
                                                if (oldItemNames[idx] != newItemNames[idx])
                                                {
                                                    fieldValues.Add(fields[idx], newItemNames[idx]);
                                                }
                                            }

                                        }
                                        #endregion

                                        #region [MDY:20220808] 2022擴充案 收入科目英文名稱
                                        {
                                            string[] fields = new string[] {
                                                SchoolRidEntity.Field.ReceiveItemE01, SchoolRidEntity.Field.ReceiveItemE02, SchoolRidEntity.Field.ReceiveItemE03, SchoolRidEntity.Field.ReceiveItemE04, SchoolRidEntity.Field.ReceiveItemE05,
                                                SchoolRidEntity.Field.ReceiveItemE06, SchoolRidEntity.Field.ReceiveItemE07, SchoolRidEntity.Field.ReceiveItemE08, SchoolRidEntity.Field.ReceiveItemE09, SchoolRidEntity.Field.ReceiveItemE10,
                                                SchoolRidEntity.Field.ReceiveItemE11, SchoolRidEntity.Field.ReceiveItemE12, SchoolRidEntity.Field.ReceiveItemE13, SchoolRidEntity.Field.ReceiveItemE14, SchoolRidEntity.Field.ReceiveItemE15,
                                                SchoolRidEntity.Field.ReceiveItemE16, SchoolRidEntity.Field.ReceiveItemE17, SchoolRidEntity.Field.ReceiveItemE18, SchoolRidEntity.Field.ReceiveItemE19, SchoolRidEntity.Field.ReceiveItemE20,
                                                SchoolRidEntity.Field.ReceiveItemE21, SchoolRidEntity.Field.ReceiveItemE22, SchoolRidEntity.Field.ReceiveItemE23, SchoolRidEntity.Field.ReceiveItemE24, SchoolRidEntity.Field.ReceiveItemE25,
                                                SchoolRidEntity.Field.ReceiveItemE26, SchoolRidEntity.Field.ReceiveItemE27, SchoolRidEntity.Field.ReceiveItemE28, SchoolRidEntity.Field.ReceiveItemE29, SchoolRidEntity.Field.ReceiveItemE30,
                                                SchoolRidEntity.Field.ReceiveItemE31, SchoolRidEntity.Field.ReceiveItemE32, SchoolRidEntity.Field.ReceiveItemE33, SchoolRidEntity.Field.ReceiveItemE34, SchoolRidEntity.Field.ReceiveItemE35,
                                                SchoolRidEntity.Field.ReceiveItemE36, SchoolRidEntity.Field.ReceiveItemE37, SchoolRidEntity.Field.ReceiveItemE38, SchoolRidEntity.Field.ReceiveItemE39, SchoolRidEntity.Field.ReceiveItemE40
                                            };

                                            string[] oldItemNames = oldSchoolRid.GetAllReceiveItemEngs();
                                            string[] newItemNames = schoolRid.GetAllReceiveItemEngs();

                                            for (int idx = 0; idx < 40; idx++)
                                            {
                                                if (oldItemNames[idx] != newItemNames[idx])
                                                {
                                                    fieldValues.Add(fields[idx], newItemNames[idx]);
                                                }
                                            }

                                        }
                                        #endregion

                                        if (fieldValues.Count > 0)
                                        {
                                            result = tsFactory.UpdateFields<SchoolRidEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "費用別設定資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #endregion

                                #region StudentCourseEntity
                                if (result.IsSuccess && course != null)
                                {
                                    Expression where = new Expression(StudentCourseEntity.Field.ReceiveType, course.ReceiveType)
                                        .And(StudentCourseEntity.Field.YearId, course.YearId)
                                        .And(StudentCourseEntity.Field.TermId, course.TermId)
                                        .And(StudentCourseEntity.Field.DepId, course.DepId)
                                        .And(StudentCourseEntity.Field.ReceiveId, course.ReceiveId)
                                        .And(StudentCourseEntity.Field.StuId, course.StuId);
                                    result = tsFactory.SelectCount<StudentCourseEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(course, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學生課程資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            result = tsFactory.Update(course, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學生課程資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                #region StudentLoanEntity
                                if (result.IsSuccess)
                                {
                                    Expression where = new Expression(StudentLoanEntity.Field.ReceiveType, receive.ReceiveType)
                                        .And(StudentLoanEntity.Field.YearId, studentReceive.YearId)
                                        .And(StudentLoanEntity.Field.TermId, studentReceive.TermId)
                                        .And(StudentLoanEntity.Field.DepId, studentReceive.DepId)
                                        .And(StudentLoanEntity.Field.ReceiveId, receive.ReceiveId)
                                        .And(StudentLoanEntity.Field.StuId, studentReceive.StuId)
                                        .And(StudentLoanEntity.Field.LoanId, studentReceive.LoanId);
                                    result = tsFactory.SelectCount<StudentLoanEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            StudentLoanEntity studentLoan = null;
                                            studentLoan = new StudentLoanEntity();
                                            studentLoan.ReceiveType = studentReceive.ReceiveType;
                                            studentLoan.YearId = studentReceive.YearId;
                                            studentLoan.TermId = studentReceive.TermId;
                                            studentLoan.DepId = studentReceive.DepId;
                                            studentLoan.ReceiveId = studentReceive.ReceiveId;
                                            studentLoan.StuId = studentReceive.StuId;
                                            studentLoan.LoanId = studentReceive.LoanId;
                                            studentLoan.LoanAmount = studentReceive.LoanAmount ?? 0M;
                                            result = tsFactory.Insert(studentLoan, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學生就貸資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(StudentLoanEntity.Field.LoanAmount, studentReceive.LoanAmount);

                                            result = tsFactory.UpdateFields<StudentLoanEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學生就貸資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                #region ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity, ReduceListEntity, LoanListEntity, DormListEntity
                                {
                                    #region ClassListEntity
                                    if (result.IsSuccess && classEntity != null)
                                    {
                                        Expression where = new Expression(ClassListEntity.Field.ReceiveType, classEntity.ReceiveType)
                                            .And(ClassListEntity.Field.YearId, classEntity.YearId)
                                            .And(ClassListEntity.Field.TermId, classEntity.TermId)
                                            .And(ClassListEntity.Field.DepId, classEntity.DepId)
                                            .And(ClassListEntity.Field.ClassId, classEntity.ClassId);
                                        result = tsFactory.SelectCount<ClassListEntity>(where, out count);
                                        if (result.IsSuccess)
                                        {
                                            if (count == 0)
                                            {
                                                #region Insert
                                                result = tsFactory.Insert(classEntity, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "班別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Update
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(ClassListEntity.Field.ClassName, classEntity.ClassName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(ClassListEntity.Field.ClassEName, classEntity.ClassEName);
                                                #endregion

                                                fieldValues.Add(ClassListEntity.Field.MdyDate, classEntity.CrtDate);
                                                fieldValues.Add(ClassListEntity.Field.MdyUser, classEntity.CrtUser);

                                                result = tsFactory.UpdateFields<ClassListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "班別資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion

                                    #region DeptListEntity
                                    if (result.IsSuccess && dept != null)
                                    {
                                        Expression where = new Expression(DeptListEntity.Field.ReceiveType, dept.ReceiveType)
                                            .And(DeptListEntity.Field.YearId, dept.YearId)
                                            .And(DeptListEntity.Field.TermId, dept.TermId)
                                            .And(DeptListEntity.Field.DeptId, dept.DeptId);
                                        result = tsFactory.SelectCount<DeptListEntity>(where, out count);
                                        if (result.IsSuccess)
                                        {
                                            if (count == 0)
                                            {
                                                #region Insert
                                                result = tsFactory.Insert(dept, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "部別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Update
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(DeptListEntity.Field.DeptName, dept.DeptName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(DeptListEntity.Field.DeptEName, dept.DeptEName);
                                                #endregion

                                                fieldValues.Add(DeptListEntity.Field.MdyDate, dept.CrtDate);
                                                fieldValues.Add(DeptListEntity.Field.MdyUser, dept.CrtUser);

                                                result = tsFactory.UpdateFields<DeptListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "部別資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion

                                    #region CollegeListEntity
                                    if (result.IsSuccess && college != null)
                                    {
                                        Expression where = new Expression(CollegeListEntity.Field.ReceiveType, college.ReceiveType)
                                            .And(CollegeListEntity.Field.YearId, college.YearId)
                                            .And(CollegeListEntity.Field.TermId, college.TermId)
                                            .And(CollegeListEntity.Field.DepId, college.DepId)
                                            .And(CollegeListEntity.Field.CollegeId, college.CollegeId);
                                        result = tsFactory.SelectCount<CollegeListEntity>(where, out count);
                                        if (result.IsSuccess)
                                        {
                                            if (count == 0)
                                            {
                                                #region Insert
                                                result = tsFactory.Insert(college, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "院別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Update
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(CollegeListEntity.Field.CollegeName, college.CollegeName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(CollegeListEntity.Field.CollegeEName, college.CollegeEName);
                                                #endregion

                                                fieldValues.Add(CollegeListEntity.Field.MdyDate, college.CrtDate);
                                                fieldValues.Add(CollegeListEntity.Field.MdyUser, college.CrtUser);

                                                result = tsFactory.UpdateFields<CollegeListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "院別資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion

                                    #region MajorListEntity
                                    if (result.IsSuccess && major != null)
                                    {
                                        Expression where = new Expression(MajorListEntity.Field.ReceiveType, major.ReceiveType)
                                            .And(MajorListEntity.Field.YearId, major.YearId)
                                            .And(MajorListEntity.Field.TermId, major.TermId)
                                            .And(MajorListEntity.Field.DepId, major.DepId)
                                            .And(MajorListEntity.Field.MajorId, major.MajorId);
                                        result = tsFactory.SelectCount<MajorListEntity>(where, out count);
                                        if (result.IsSuccess)
                                        {
                                            if (count == 0)
                                            {
                                                #region Insert
                                                result = tsFactory.Insert(major, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "系所資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Update
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(MajorListEntity.Field.MajorName, major.MajorName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(MajorListEntity.Field.MajorEName, major.MajorEName);
                                                #endregion

                                                fieldValues.Add(MajorListEntity.Field.MdyDate, major.CrtDate);
                                                fieldValues.Add(MajorListEntity.Field.MdyUser, major.CrtUser);

                                                result = tsFactory.UpdateFields<MajorListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "系所資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion

                                    #region ReduceListEntity
                                    if (result.IsSuccess && reduce != null)
                                    {
                                        Expression where = new Expression(ReduceListEntity.Field.ReceiveType, reduce.ReceiveType)
                                            .And(ReduceListEntity.Field.YearId, reduce.YearId)
                                            .And(ReduceListEntity.Field.TermId, reduce.TermId)
                                            .And(ReduceListEntity.Field.DepId, reduce.DepId)
                                            .And(ReduceListEntity.Field.ReduceId, reduce.ReduceId);
                                        result = tsFactory.SelectCount<ReduceListEntity>(where, out count);
                                        if (result.IsSuccess)
                                        {
                                            if (count == 0)
                                            {
                                                #region Insert
                                                result = tsFactory.Insert(reduce, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "減免類別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Update
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(ReduceListEntity.Field.ReduceName, reduce.ReduceName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(ReduceListEntity.Field.ReduceEName, reduce.ReduceEName);
                                                #endregion

                                                fieldValues.Add(ReduceListEntity.Field.MdyDate, reduce.CrtDate);
                                                fieldValues.Add(ReduceListEntity.Field.MdyUser, reduce.CrtUser);

                                                result = tsFactory.UpdateFields<ReduceListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "減免類別資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion

                                    #region LoanListEntity
                                    if (result.IsSuccess && loan != null)
                                    {
                                        Expression where = new Expression(LoanListEntity.Field.ReceiveType, loan.ReceiveType)
                                            .And(LoanListEntity.Field.YearId, loan.YearId)
                                            .And(LoanListEntity.Field.TermId, loan.TermId)
                                            .And(LoanListEntity.Field.DepId, loan.DepId)
                                            .And(LoanListEntity.Field.LoanId, loan.LoanId);
                                        result = tsFactory.SelectCount<LoanListEntity>(where, out count);
                                        if (result.IsSuccess)
                                        {
                                            if (count == 0)
                                            {
                                                #region Insert
                                                result = tsFactory.Insert(loan, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "就貸項目資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Update
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(LoanListEntity.Field.LoanName, loan.LoanName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(LoanListEntity.Field.LoanEName, loan.LoanEName);
                                                #endregion

                                                fieldValues.Add(LoanListEntity.Field.MdyDate, loan.CrtDate);
                                                fieldValues.Add(LoanListEntity.Field.MdyUser, loan.CrtUser);

                                                result = tsFactory.UpdateFields<LoanListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "就貸項目資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion

                                    #region DormListEntity
                                    if (result.IsSuccess && dorm != null)
                                    {
                                        Expression where = new Expression(DormListEntity.Field.ReceiveType, dorm.ReceiveType)
                                            .And(DormListEntity.Field.YearId, dorm.YearId)
                                            .And(DormListEntity.Field.TermId, dorm.TermId)
                                            .And(DormListEntity.Field.DepId, dorm.DepId)
                                            .And(DormListEntity.Field.DormId, dorm.DormId);
                                        result = tsFactory.SelectCount<DormListEntity>(where, out count);
                                        if (result.IsSuccess)
                                        {
                                            if (count == 0)
                                            {
                                                #region Insert
                                                result = tsFactory.Insert(dorm, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "住宿項目資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Update
                                                KeyValueList fieldValues = new KeyValueList();
                                                fieldValues.Add(DormListEntity.Field.DormName, dorm.DormName);

                                                #region [MDY:20220808] 2022擴充案 英文名稱
                                                fieldValues.Add(DormListEntity.Field.DormEName, dorm.DormEName);
                                                #endregion

                                                fieldValues.Add(DormListEntity.Field.MdyDate, dorm.CrtDate);
                                                fieldValues.Add(DormListEntity.Field.MdyUser, dorm.CrtUser);

                                                result = tsFactory.UpdateFields<DormListEntity>(fieldValues, where, out count);
                                                if (result.IsSuccess && count == 0)
                                                {
                                                    result = new Result(false, "住宿項目資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region IdentifyList1Entity
                                if (result.IsSuccess && identify1 != null)
                                {
                                    Expression where = new Expression(IdentifyList1Entity.Field.ReceiveType, identify1.ReceiveType)
                                        .And(IdentifyList1Entity.Field.YearId, identify1.YearId)
                                        .And(IdentifyList1Entity.Field.TermId, identify1.TermId)
                                        .And(IdentifyList1Entity.Field.DepId, identify1.DepId)
                                        .And(IdentifyList1Entity.Field.IdentifyId, identify1.IdentifyId);
                                    result = tsFactory.SelectCount<IdentifyList1Entity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(identify1, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "身分註記一項目資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(IdentifyList1Entity.Field.IdentifyName, identify1.IdentifyName);

                                            #region [MDY:20220808] 2022擴充案 英文名稱
                                            fieldValues.Add(IdentifyList1Entity.Field.IdentifyEName, identify1.IdentifyEName);
                                            #endregion

                                            fieldValues.Add(IdentifyList1Entity.Field.MdyDate, identify1.CrtDate);
                                            fieldValues.Add(IdentifyList1Entity.Field.MdyUser, identify1.CrtUser);

                                            result = tsFactory.UpdateFields<IdentifyList1Entity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "身分註記一項目資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                if (result.IsSuccess)
                                {
                                    tsFactory.Commit();
                                }
                                else
                                {
                                    tsFactory.Rollback();
                                }
                            }
                        }
                        break;
                    case BillOpList.DELETE:
                        break;
                }

                if (result.IsSuccess)
                {
                    returnMsg = ErrorList.NORMAL;
                    return null;
                }
                else
                {
                    returnMsg = ErrorList.GetErrorMessage(ErrorList.S9999, "寫入資料庫失敗");
                    return String.Format("寫入資料庫失敗，錯誤訊息：{0}", result.Message);
                }
            }
            #endregion
        }

        /// <summary>
        /// 學生繳費資料查詢
        /// </summary>
        /// <param name="account"></param>
        /// <param name="qReceiveType"></param>
        /// <param name="qYearId"></param>
        /// <param name="qTermId"></param>
        /// <param name="qSStuId"></param>
        /// <param name="qEStuId"></param>
        /// <param name="qSeqNo"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public string BillQuery(SchoolServiceAccountEntity account, string qReceiveType, string qYearId, string qTermId, string qSStuId, string qEStuId, string qSeqNo, out ReturnData returnData)
        {
            returnData = null;

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(qReceiveType))
            {
                string errmsg = "缺少商家代號查詢參數";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            qReceiveType = qReceiveType.Trim();
            if (String.IsNullOrWhiteSpace(qYearId))
            {
                string errmsg = "缺少學年代碼查詢參數";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            qYearId = qYearId.Trim();
            if (String.IsNullOrWhiteSpace(qTermId))
            {
                string errmsg = "缺少學期代碼查詢參數";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            qTermId = qTermId.Trim();

            if (qSStuId != null)
            {
                qSStuId = qSStuId.Trim();
            }
            if (qEStuId != null)
            {
                qEStuId = qEStuId.Trim();
            }
            if (qSeqNo != null)
            {
                qSeqNo = qSeqNo.Trim();
            }
            if (String.IsNullOrEmpty(qSStuId) && String.IsNullOrEmpty(qEStuId) && String.IsNullOrEmpty(qSeqNo))
            {
                string errmsg = "起始學號、結束學號或學校端惟一序號的查詢參數至少要指定一項";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }

            if (!account.IsMyReceiveType(qReceiveType))
            {
                string errmsg = String.Format("查詢的商家代號 ({0}) 不在服務帳號授權裡", qReceiveType);
                returnData = new ReturnData(ErrorList.S0007, "商家代號未授權");
                return errmsg;
            }
            #endregion

            #region #region [MDY:202203XX] 2022擴充案 取得是否啟用英文資料
            bool isEngEnabled = false;
            {
                string sql = $"SELECT {SchoolRTypeEntity.Field.EngEnabled} FROM {SchoolRTypeEntity.TABLE_NAME} WHERE {SchoolRTypeEntity.Field.ReceiveType} = @ReceiveType";
                KeyValue[] parameters = new KeyValue[] { new KeyValue("@ReceiveType", qReceiveType) };
                object value = null;
                Result result = _Factory.ExecuteScalar(sql, parameters, out value);
                if (result.IsSuccess)
                {
                    isEngEnabled = "Y".Equals(value?.ToString());
                }
                else
                {
                    returnData = new ReturnData(ErrorList.S9999, "查詢商家代號資料失敗");
                    return $"查詢商家代號 ({qReceiveType}) 資料失敗，錯誤訊息：{result.Message}";
                }
            }
            #endregion

            KeyValueList qParameters = new KeyValueList();
            string andSql = String.Empty;

            #region 檢查學號範圍資料筆數
            if (!String.IsNullOrEmpty(qSStuId) && !String.IsNullOrEmpty(qEStuId))
            {
                string sql = String.Format("SELECT COUNT(1) FROM [{0}] WHERE [{1}] = @ReceiveType AND [{2}] >= @SStuId AND [{2}] <= @EStuId"
                    , StudentMasterEntity.TABLE_NAME, StudentMasterEntity.Field.ReceiveType, StudentMasterEntity.Field.Id);
                KeyValue[] parameters = new KeyValue[3] {
                    new KeyValue("@ReceiveType", qReceiveType),
                    new KeyValue("@SStuId", qSStuId),
                    new KeyValue("@EStuId", qEStuId)
                };
                object value = null;
                Result result = _Factory.ExecuteScalar(sql, parameters, out value);
                if (result.IsSuccess)
                {
                    int maxDataCount = 15000;
                    int dataCount = 0;
                    if (value != null && Int32.TryParse(value.ToString(), out dataCount) && dataCount > maxDataCount)
                    {
                        returnData = new ReturnData(ErrorList.S0007, String.Format("查詢學號範圍超過 {0} 筆學生，請縮小查詢範圍", maxDataCount));
                        return String.Format("查詢學號範圍 ({0} ~ {1}) 包含 {2} 筆學生，超過 {3} 筆的限制", qSStuId, qEStuId, dataCount, maxDataCount);
                    }
                }
                else
                {
                    returnData = new ReturnData(ErrorList.S9999);
                    return String.Format("檢查學號範圍資料筆數失敗，錯誤訊息：{0}", result.Message);
                }

                andSql += " AND SR.[Stu_Id] >= @SStuId AND SR.[Stu_Id] <= @EStuId";
                qParameters.Add("@SStuId", qSStuId);
                qParameters.Add("@EStuId", qEStuId);
            }
            else if (!String.IsNullOrEmpty(qSStuId))
            {
                andSql += " AND SR.[Stu_Id] = @SStuId";
                qParameters.Add("@SStuId", qSStuId);
            }
            else if (!String.IsNullOrEmpty(qEStuId))
            {
                andSql += " AND SR.[Stu_Id] = @EStuId";
                qParameters.Add("@EStuId", qEStuId);
            }
            #endregion

            #region qSeqNo
            if (!String.IsNullOrEmpty(qSeqNo))
            {
                andSql += " AND SR.[Service_Seq_No] = @SeqNo";
                qParameters.Add("@SeqNo", qSeqNo);
            }
            #endregion

            DataTable dt = null;

            #region 取得查詢資料
            {
                #region [MDY:202204] 2022擴充案 依據 是否啟用英文資料 決定是否取 學期、費用別、收入科目、部別、院別、科系、班級、住宿、減免、身分註記 英文名稱欄位
                #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
                #region [MDY:20211001] M202110_01 新增 分期/分筆代碼 OldSeq (2021擴充案先做)
                string sql = null;
                if (isEngEnabled)
                {
                    sql = $@"
SELECT SR.[Service_Seq_No] AS [seq_no], SR.[Receive_Type], SR.[Year_Id]
     , SR.[Term_Id], ISNULL(TL.[Term_Name], '') AS [Term_Name], ISNULL(TL.[Term_EName], '') AS [Term_EName]
     , SR.[Receive_Id], ISNULL(RL.[Receive_Name], '') AS [Receive_Name], ISNULL(RL.[Receive_EName], '') AS [Receive_EName]
     , SR.[Old_Seq]
     , SR.[Cancel_No], SR.[Receive_Amount], SR.[Loan_Amount], SR2.[Pay_Date] AS [pay_due_date]
     , CASE WHEN ISNULL(SR.Cancel_SMNo, '') = '' THEN 'N' ELSE 'Y' END AS [sm_channel]
     --bill_header (學生基本資料)
     , SR.[Stu_Id], SM.[Stu_Name], SM.[Stu_Birthday], SM.[Id_Number] AS [stu_pid], SM.[Stu_Tel], SM.[Stu_Addcode], SM.[Stu_Address], SM.[Stu_Email]
     --bill_detail (收入科目名稱)
     , SR2.[Receive_Item01] AS [receive_item_01_name], SR2.[Receive_ItemE01] AS [receive_item_01_ename]
     , SR2.[Receive_Item02] AS [receive_item_02_name], SR2.[Receive_ItemE02] AS [receive_item_02_ename]
     , SR2.[Receive_Item03] AS [receive_item_03_name], SR2.[Receive_ItemE03] AS [receive_item_03_ename]
     , SR2.[Receive_Item04] AS [receive_item_04_name], SR2.[Receive_ItemE04] AS [receive_item_04_ename]
     , SR2.[Receive_Item05] AS [receive_item_05_name], SR2.[Receive_ItemE05] AS [receive_item_05_ename]
     , SR2.[Receive_Item06] AS [receive_item_06_name], SR2.[Receive_ItemE06] AS [receive_item_06_ename]
     , SR2.[Receive_Item07] AS [receive_item_07_name], SR2.[Receive_ItemE07] AS [receive_item_07_ename]
     , SR2.[Receive_Item08] AS [receive_item_08_name], SR2.[Receive_ItemE08] AS [receive_item_08_ename]
     , SR2.[Receive_Item09] AS [receive_item_09_name], SR2.[Receive_ItemE09] AS [receive_item_09_ename]
     , SR2.[Receive_Item10] AS [receive_item_10_name], SR2.[Receive_ItemE10] AS [receive_item_10_ename]
     , SR2.[Receive_Item11] AS [receive_item_11_name], SR2.[Receive_ItemE11] AS [receive_item_11_ename]
     , SR2.[Receive_Item12] AS [receive_item_12_name], SR2.[Receive_ItemE12] AS [receive_item_12_ename]
     , SR2.[Receive_Item13] AS [receive_item_13_name], SR2.[Receive_ItemE13] AS [receive_item_13_ename]
     , SR2.[Receive_Item14] AS [receive_item_14_name], SR2.[Receive_ItemE14] AS [receive_item_14_ename]
     , SR2.[Receive_Item15] AS [receive_item_15_name], SR2.[Receive_ItemE15] AS [receive_item_15_ename]
     , SR2.[Receive_Item16] AS [receive_item_16_name], SR2.[Receive_ItemE16] AS [receive_item_16_ename]
     , SR2.[Receive_Item17] AS [receive_item_17_name], SR2.[Receive_ItemE17] AS [receive_item_17_ename]
     , SR2.[Receive_Item18] AS [receive_item_18_name], SR2.[Receive_ItemE18] AS [receive_item_18_ename]
     , SR2.[Receive_Item19] AS [receive_item_19_name], SR2.[Receive_ItemE19] AS [receive_item_19_ename]
     , SR2.[Receive_Item20] AS [receive_item_20_name], SR2.[Receive_ItemE20] AS [receive_item_20_ename]
     , SR2.[Receive_Item21] AS [receive_item_21_name], SR2.[Receive_ItemE21] AS [receive_item_21_ename]
     , SR2.[Receive_Item22] AS [receive_item_22_name], SR2.[Receive_ItemE22] AS [receive_item_22_ename]
     , SR2.[Receive_Item23] AS [receive_item_23_name], SR2.[Receive_ItemE23] AS [receive_item_23_ename]
     , SR2.[Receive_Item24] AS [receive_item_24_name], SR2.[Receive_ItemE24] AS [receive_item_24_ename]
     , SR2.[Receive_Item25] AS [receive_item_25_name], SR2.[Receive_ItemE25] AS [receive_item_25_ename]
     , SR2.[Receive_Item26] AS [receive_item_26_name], SR2.[Receive_ItemE26] AS [receive_item_26_ename]
     , SR2.[Receive_Item27] AS [receive_item_27_name], SR2.[Receive_ItemE27] AS [receive_item_27_ename]
     , SR2.[Receive_Item28] AS [receive_item_28_name], SR2.[Receive_ItemE28] AS [receive_item_28_ename]
     , SR2.[Receive_Item29] AS [receive_item_29_name], SR2.[Receive_ItemE29] AS [receive_item_29_ename]
     , SR2.[Receive_Item30] AS [receive_item_30_name], SR2.[Receive_ItemE30] AS [receive_item_30_ename]
     , SR2.[Receive_Item31] AS [receive_item_31_name], SR2.[Receive_ItemE31] AS [receive_item_31_ename]
     , SR2.[Receive_Item32] AS [receive_item_32_name], SR2.[Receive_ItemE32] AS [receive_item_32_ename]
     , SR2.[Receive_Item33] AS [receive_item_33_name], SR2.[Receive_ItemE33] AS [receive_item_33_ename]
     , SR2.[Receive_Item34] AS [receive_item_34_name], SR2.[Receive_ItemE34] AS [receive_item_34_ename]
     , SR2.[Receive_Item35] AS [receive_item_35_name], SR2.[Receive_ItemE35] AS [receive_item_35_ename]
     , SR2.[Receive_Item36] AS [receive_item_36_name], SR2.[Receive_ItemE36] AS [receive_item_36_ename]
     , SR2.[Receive_Item37] AS [receive_item_37_name], SR2.[Receive_ItemE37] AS [receive_item_37_ename]
     , SR2.[Receive_Item38] AS [receive_item_38_name], SR2.[Receive_ItemE38] AS [receive_item_38_ename]
     , SR2.[Receive_Item39] AS [receive_item_39_name], SR2.[Receive_ItemE39] AS [receive_item_39_ename]
     , SR2.[Receive_Item40] AS [receive_item_40_name], SR2.[Receive_ItemE40] AS [receive_item_40_ename]
     --bill_detail (收入科目金額)
     , SR.[Receive_01] AS [receive_item_01_amount], SR.[Receive_02] AS [receive_item_02_amount]
     , SR.[Receive_03] AS [receive_item_03_amount], SR.[Receive_04] AS [receive_item_04_amount]
     , SR.[Receive_05] AS [receive_item_05_amount], SR.[Receive_06] AS [receive_item_06_amount]
     , SR.[Receive_07] AS [receive_item_07_amount], SR.[Receive_08] AS [receive_item_08_amount]
     , SR.[Receive_09] AS [receive_item_09_amount], SR.[Receive_10] AS [receive_item_10_amount]
     , SR.[Receive_11] AS [receive_item_11_amount], SR.[Receive_12] AS [receive_item_12_amount]
     , SR.[Receive_13] AS [receive_item_13_amount], SR.[Receive_14] AS [receive_item_14_amount]
     , SR.[Receive_15] AS [receive_item_15_amount], SR.[Receive_16] AS [receive_item_16_amount]
     , SR.[Receive_17] AS [receive_item_17_amount], SR.[Receive_18] AS [receive_item_18_amount]
     , SR.[Receive_19] AS [receive_item_19_amount], SR.[Receive_20] AS [receive_item_20_amount]
     , SR.[Receive_21] AS [receive_item_21_amount], SR.[Receive_22] AS [receive_item_22_amount]
     , SR.[Receive_23] AS [receive_item_23_amount], SR.[Receive_24] AS [receive_item_24_amount]
     , SR.[Receive_25] AS [receive_item_25_amount], SR.[Receive_26] AS [receive_item_26_amount]
     , SR.[Receive_27] AS [receive_item_27_amount], SR.[Receive_28] AS [receive_item_28_amount]
     , SR.[Receive_29] AS [receive_item_29_amount], SR.[Receive_30] AS [receive_item_30_amount]
     , SR.[Receive_31] AS [receive_item_31_amount], SR.[Receive_32] AS [receive_item_32_amount]
     , SR.[Receive_33] AS [receive_item_33_amount], SR.[Receive_34] AS [receive_item_34_amount]
     , SR.[Receive_35] AS [receive_item_35_amount], SR.[Receive_36] AS [receive_item_36_amount]
     , SR.[Receive_37] AS [receive_item_37_amount], SR.[Receive_38] AS [receive_item_38_amount]
     , SR.[Receive_39] AS [receive_item_39_amount], SR.[Receive_40] AS [receive_item_40_amount]
     , SR.[Pay_Due_Date] AS [bill_pay_due_date], SR.[NCCardFlag] AS [nccard_flag]
     -- bill_info
     , SR.[Dept_Id] AS [dep_id], ISNULL(DTL.[Dept_Name], '') AS [dep_name], ISNULL(DTL.[Dept_EName], '') AS [dep_ename]
     , SR.[College_Id], ISNULL(CL.[College_Name], '') AS [College_Name], ISNULL(CL.[College_EName], '') AS [College_EName]
     , SR.[Major_Id], ISNULL(ML.[Major_Name], '') AS [Major_Name], ISNULL(ML.[Major_EName], '') AS [Major_EName]
     , SR.[Class_Id], ISNULL(CL2.[Class_Name], '') AS [Class_Name], ISNULL(CL2.[Class_EName], '') AS [Class_EName]
     , SR.[Reduce_Id], ISNULL(RL2.[Reduce_Name], '') AS [Reduce_Name], ISNULL(RL2.[Reduce_EName], '') AS [Reduce_EName]
     , SR.[Dorm_Id], ISNULL(DL.[Dorm_Name], '') AS [Dorm_Name], ISNULL(DL.[Dorm_EName], '') AS [Dorm_EName]
     , SR.[Loan_Id], ISNULL(LL.[Loan_Name], '') AS [Loan_Name], ISNULL(LL.[Loan_EName], '') AS [Loan_EName]
     , SR.[Identify_Id01] AS [identify_id], ISNULL(IL1.[Identify_Name], '') AS [identify_name], ISNULL(IL1.[Identify_EName], '') AS [identify_ename]
     , SR.[Receive_Way], ISNULL(CS.[Channel_Name], '') AS [Channel_Name]
     , SR.[Stu_Grade] AS [grade]
  FROM Student_Receive AS SR
  JOIN Student_Master AS SM ON SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id
  LEFT JOIN [Term_List]      AS TL  ON TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]
  LEFT JOIN [Receive_List]   AS RL  ON RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id] = SR.[Receive_Id]
  LEFT JOIN [Dept_List]      AS DTL ON DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]
  LEFT JOIN [College_List]   AS CL  ON CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]
  LEFT JOIN [Major_List]     AS ML  ON ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]
  LEFT JOIN [Class_List]     AS CL2 ON CL2.[Receive_Type] = SR.[Receive_Type] AND CL2.[Year_Id] = SR.[Year_Id] AND CL2.[Term_Id] = SR.[Term_Id] AND CL2.[Dep_Id]  = SR.[Dep_Id] AND CL2.[Class_Id]    = SR.[Class_Id]
  LEFT JOIN [Reduce_List]    AS RL2 ON RL2.[Receive_Type] = SR.[Receive_Type] AND RL2.[Year_Id] = SR.[Year_Id] AND RL2.[Term_Id] = SR.[Term_Id] AND RL2.[Dep_Id]  = SR.[Dep_Id] AND RL2.[Reduce_Id]   = SR.[Reduce_Id]
  LEFT JOIN [Dorm_List]      AS DL  ON DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]
  LEFT JOIN [Loan_List]      AS LL  ON LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]
  LEFT JOIN [Identify_List1] AS IL1 ON IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]
  LEFT JOIN [School_Rid]     AS SR2 ON SR2.[Receive_Type] = SR.[Receive_Type] AND SR2.[Year_Id] = SR.[Year_Id] AND SR2.[Term_Id] = SR.[Term_Id] AND SR2.[Dep_Id]  = SR.[Dep_Id] AND SR2.[Receive_Id]  = SR.[Receive_Id]
  LEFT JOIN [Channel_Set]    AS CS  ON CS.[Channel_Id] = SR.[Receive_Way]
 WHERE SR.Receive_Type = @Receive_Type AND SR.Year_Id = @Year_Id AND SR.Term_Id = @Term_Id
   {andSql}".Trim();
                }
                else
                {
                    sql = $@"
SELECT SR.[Service_Seq_No] AS [seq_no], SR.[Receive_Type], SR.[Year_Id]
     , SR.[Term_Id], ISNULL(TL.[Term_Name], '') AS [Term_Name]
     , SR.[Receive_Id], ISNULL(RL.[Receive_Name], '') AS [Receive_Name]
     , SR.[Old_Seq]
     , SR.[Cancel_No], SR.[Receive_Amount], SR.[Loan_Amount], SR2.[Pay_Date] AS [pay_due_date]
     , CASE WHEN ISNULL(SR.Cancel_SMNo, '') = '' THEN 'N' ELSE 'Y' END AS [sm_channel]
     --bill_header (學生基本資料)
     , SR.[Stu_Id], SM.[Stu_Name], SM.[Stu_Birthday], SM.[Id_Number] AS [stu_pid], SM.[Stu_Tel], SM.[Stu_Addcode], SM.[Stu_Address], SM.[Stu_Email]
     --bill_detail (收入科目名稱)
     , SR2.[Receive_Item01] AS [receive_item_01_name]
     , SR2.[Receive_Item02] AS [receive_item_02_name]
     , SR2.[Receive_Item03] AS [receive_item_03_name]
     , SR2.[Receive_Item04] AS [receive_item_04_name]
     , SR2.[Receive_Item05] AS [receive_item_05_name]
     , SR2.[Receive_Item06] AS [receive_item_06_name]
     , SR2.[Receive_Item07] AS [receive_item_07_name]
     , SR2.[Receive_Item08] AS [receive_item_08_name]
     , SR2.[Receive_Item09] AS [receive_item_09_name]
     , SR2.[Receive_Item10] AS [receive_item_10_name]
     , SR2.[Receive_Item11] AS [receive_item_11_name]
     , SR2.[Receive_Item12] AS [receive_item_12_name]
     , SR2.[Receive_Item13] AS [receive_item_13_name]
     , SR2.[Receive_Item14] AS [receive_item_14_name]
     , SR2.[Receive_Item15] AS [receive_item_15_name]
     , SR2.[Receive_Item16] AS [receive_item_16_name]
     , SR2.[Receive_Item17] AS [receive_item_17_name]
     , SR2.[Receive_Item18] AS [receive_item_18_name]
     , SR2.[Receive_Item19] AS [receive_item_19_name]
     , SR2.[Receive_Item20] AS [receive_item_20_name]
     , SR2.[Receive_Item21] AS [receive_item_21_name]
     , SR2.[Receive_Item22] AS [receive_item_22_name]
     , SR2.[Receive_Item23] AS [receive_item_23_name]
     , SR2.[Receive_Item24] AS [receive_item_24_name]
     , SR2.[Receive_Item25] AS [receive_item_25_name]
     , SR2.[Receive_Item26] AS [receive_item_26_name]
     , SR2.[Receive_Item27] AS [receive_item_27_name]
     , SR2.[Receive_Item28] AS [receive_item_28_name]
     , SR2.[Receive_Item29] AS [receive_item_29_name]
     , SR2.[Receive_Item30] AS [receive_item_30_name]
     , SR2.[Receive_Item31] AS [receive_item_31_name]
     , SR2.[Receive_Item32] AS [receive_item_32_name]
     , SR2.[Receive_Item33] AS [receive_item_33_name]
     , SR2.[Receive_Item34] AS [receive_item_34_name]
     , SR2.[Receive_Item35] AS [receive_item_35_name]
     , SR2.[Receive_Item36] AS [receive_item_36_name]
     , SR2.[Receive_Item37] AS [receive_item_37_name]
     , SR2.[Receive_Item38] AS [receive_item_38_name]
     , SR2.[Receive_Item39] AS [receive_item_39_name]
     , SR2.[Receive_Item40] AS [receive_item_40_name]
     --bill_detail (收入科目金額)
     , SR.[Receive_01] AS [receive_item_01_amount], SR.[Receive_02] AS [receive_item_02_amount]
     , SR.[Receive_03] AS [receive_item_03_amount], SR.[Receive_04] AS [receive_item_04_amount]
     , SR.[Receive_05] AS [receive_item_05_amount], SR.[Receive_06] AS [receive_item_06_amount]
     , SR.[Receive_07] AS [receive_item_07_amount], SR.[Receive_08] AS [receive_item_08_amount]
     , SR.[Receive_09] AS [receive_item_09_amount], SR.[Receive_10] AS [receive_item_10_amount]
     , SR.[Receive_11] AS [receive_item_11_amount], SR.[Receive_12] AS [receive_item_12_amount]
     , SR.[Receive_13] AS [receive_item_13_amount], SR.[Receive_14] AS [receive_item_14_amount]
     , SR.[Receive_15] AS [receive_item_15_amount], SR.[Receive_16] AS [receive_item_16_amount]
     , SR.[Receive_17] AS [receive_item_17_amount], SR.[Receive_18] AS [receive_item_18_amount]
     , SR.[Receive_19] AS [receive_item_19_amount], SR.[Receive_20] AS [receive_item_20_amount]
     , SR.[Receive_21] AS [receive_item_21_amount], SR.[Receive_22] AS [receive_item_22_amount]
     , SR.[Receive_23] AS [receive_item_23_amount], SR.[Receive_24] AS [receive_item_24_amount]
     , SR.[Receive_25] AS [receive_item_25_amount], SR.[Receive_26] AS [receive_item_26_amount]
     , SR.[Receive_27] AS [receive_item_27_amount], SR.[Receive_28] AS [receive_item_28_amount]
     , SR.[Receive_29] AS [receive_item_29_amount], SR.[Receive_30] AS [receive_item_30_amount]
     , SR.[Receive_31] AS [receive_item_31_amount], SR.[Receive_32] AS [receive_item_32_amount]
     , SR.[Receive_33] AS [receive_item_33_amount], SR.[Receive_34] AS [receive_item_34_amount]
     , SR.[Receive_35] AS [receive_item_35_amount], SR.[Receive_36] AS [receive_item_36_amount]
     , SR.[Receive_37] AS [receive_item_37_amount], SR.[Receive_38] AS [receive_item_38_amount]
     , SR.[Receive_39] AS [receive_item_39_amount], SR.[Receive_40] AS [receive_item_40_amount]
     , SR.[Pay_Due_Date] AS [bill_pay_due_date], SR.[NCCardFlag] AS [nccard_flag]
     -- bill_info
     , SR.[Dept_Id] AS [dep_id], ISNULL(DTL.[Dept_Name], '') AS [dep_name]
     , SR.[College_Id], ISNULL(CL.[College_Name], '') AS [College_Name]
     , SR.[Major_Id], ISNULL(ML.[Major_Name], '') AS [Major_Name]
     , SR.[Class_Id], ISNULL(CL2.[Class_Name], '') AS [Class_Name]
     , SR.[Reduce_Id], ISNULL(RL2.[Reduce_Name], '') AS [Reduce_Name]
     , SR.[Dorm_Id], ISNULL(DL.[Dorm_Name], '') AS [Dorm_Name]
     , SR.[Loan_Id], ISNULL(LL.[Loan_Name], '') AS [Loan_Name]
     , SR.[Identify_Id01] AS [identify_id], ISNULL(IL1.[Identify_Name], '') AS [identify_name]
     , SR.[Receive_Way], ISNULL(CS.[Channel_Name], '') AS [Channel_Name]
     , SR.[Stu_Grade] AS [grade]
  FROM Student_Receive AS SR
  JOIN Student_Master AS SM ON SM.Receive_Type = SR.Receive_Type AND SM.Dep_Id = SR.Dep_Id AND SM.Stu_Id = SR.Stu_Id
  LEFT JOIN [Term_List]      AS TL  ON TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]
  LEFT JOIN [Receive_List]   AS RL  ON RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id] = SR.[Receive_Id]
  LEFT JOIN [Dept_List]      AS DTL ON DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]
  LEFT JOIN [College_List]   AS CL  ON CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]
  LEFT JOIN [Major_List]     AS ML  ON ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]
  LEFT JOIN [Class_List]     AS CL2 ON CL2.[Receive_Type] = SR.[Receive_Type] AND CL2.[Year_Id] = SR.[Year_Id] AND CL2.[Term_Id] = SR.[Term_Id] AND CL2.[Dep_Id]  = SR.[Dep_Id] AND CL2.[Class_Id]    = SR.[Class_Id]
  LEFT JOIN [Reduce_List]    AS RL2 ON RL2.[Receive_Type] = SR.[Receive_Type] AND RL2.[Year_Id] = SR.[Year_Id] AND RL2.[Term_Id] = SR.[Term_Id] AND RL2.[Dep_Id]  = SR.[Dep_Id] AND RL2.[Reduce_Id]   = SR.[Reduce_Id]
  LEFT JOIN [Dorm_List]      AS DL  ON DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]
  LEFT JOIN [Loan_List]      AS LL  ON LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]
  LEFT JOIN [Identify_List1] AS IL1 ON IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]
  LEFT JOIN [School_Rid]     AS SR2 ON SR2.[Receive_Type] = SR.[Receive_Type] AND SR2.[Year_Id] = SR.[Year_Id] AND SR2.[Term_Id] = SR.[Term_Id] AND SR2.[Dep_Id]  = SR.[Dep_Id] AND SR2.[Receive_Id]  = SR.[Receive_Id]
  LEFT JOIN [Channel_Set]    AS CS  ON CS.[Channel_Id] = SR.[Receive_Way]
 WHERE SR.Receive_Type = @Receive_Type AND SR.Year_Id = @Year_Id AND SR.Term_Id = @Term_Id
   {andSql}".Trim();
                }
                #endregion
                #endregion
                #endregion

                qParameters.Add("@Receive_Type", qReceiveType);
                qParameters.Add("@Year_Id", qYearId);
                qParameters.Add("@Term_Id", qTermId);

                Result result = _Factory.GetDataTable(sql, qParameters, 0, 0, out dt);
                if (!result.IsSuccess)
                {
                    returnData = new ReturnData(ErrorList.S9999, "查詢學生繳費資料失敗");
                    return String.Format("查詢學生繳費資料失敗，錯誤訊息：{0}", result.Message);
                }
            }
            #endregion

            #region 將查詢結果轉成 XML 檔
            if (dt == null || dt.Rows.Count == 0)
            {
                returnData = new ReturnData(ErrorList.NORMAL);
                returnData.Records = 0;
                returnData.TxnFile = null;
                return "查無資料";
            }
            else
            {
                try
                {
                    string tempPath = ConfigManager.Current.GetSystemConfigValue("DATA_SERVICE", "TEMP_PATH");
                    if (String.IsNullOrEmpty(tempPath))
                    {
                        tempPath = Path.GetTempPath();
                    }
                    DateTime runTime = DateTime.Now;
                    string xmlFile = Path.Combine(tempPath, String.Format("{0}_BILL{1:yyyyMMddHHmmssfffff}.xml", account.SysId, runTime));
                    if (File.Exists(xmlFile))
                    {
                        File.Delete(xmlFile);
                    }

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.CheckCharacters = false;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;
                    settings.OmitXmlDeclaration = true;

                    //bill_header 欄位清單
                    #region [MDY:202204] 2022擴充案 依據 是否啟用英文資料 決定是否取 學期、費用別 英文名稱欄位
                    #region [MDY:20211001] M202110_01 新增 分期/分筆代碼 OldSeq (2021擴充案先做)
                    string[] headerColumns = null;
                    if (isEngEnabled)
                    {
                        headerColumns = new string[] {
                            "seq_no", "receive_type", "year_id", "term_id", "term_name", "term_ename", "receive_id", "receive_name", "receive_ename",
                            "old_seq", "cancel_no", "receive_amount", "loan_amount", "pay_due_date", "sm_channel",
                            "stu_id", "stu_name", "stu_pid", "stu_birthday", "stu_email", "stu_addcode", "stu_address", "stu_tel"
                        };
                    }
                    else
                    {
                        headerColumns = new string[] {
                            "seq_no", "receive_type", "year_id", "term_id", "term_name", "receive_id", "receive_name",
                            "old_seq", "cancel_no", "receive_amount", "loan_amount", "pay_due_date", "sm_channel",
                            "stu_id", "stu_name", "stu_pid", "stu_birthday", "stu_email", "stu_addcode", "stu_address", "stu_tel"
                        };
                    }
                    #endregion
                    #endregion

                    // bill_info 欄位清單
                    #region [MDY:202204] 2022擴充案 依據 是否啟用英文資料 決定是否取 部別、院別、科系、班級、住宿、減免、身分註記 英文名稱欄位
                    string[] infoColumns = null;
                    if (isEngEnabled)
                    {
                        infoColumns = new string[] {
                            "dep_id", "dep_name", "dep_ename", "college_id", "college_name", "college_ename",
                            "major_id", "major_name", "major_ename",
                            "grade",
                            "class_id", "class_name", "class_ename", "dorm_id", "dorm_name", "dorm_ename",
                            "reduce_id", "reduce_name", "reduce_ename", "identify_id", "identify_name", "identify_ename"
                        };
                    }
                    else
                    {
                        infoColumns = new string[] {
                            "dep_id", "dep_name", "college_id", "college_name", 
                            "major_id", "major_name",
                            "grade", 
                            "class_id", "class_name", "dorm_id", "dorm_name", 
                            "reduce_id", "reduce_name", "identify_id", "identify_name"
                        };
                    }
                    #endregion

                    using (XmlWriter xw = XmlWriter.Create(xmlFile, settings))
                    {
                        #region 根節點(landbank)開始
                        xw.WriteStartElement("landbank");
                        #endregion

                        foreach (DataRow drow in dt.Rows)
                        {
                            #region 資料根節點(bill)開始
                            xw.WriteStartElement("bill");
                            #endregion

                            #region bill_header 節點
                            {
                                xw.WriteStartElement("bill_header");
                                foreach (string column in headerColumns)
                                {
                                    string value = drow.IsNull(column) ? String.Empty : drow[column].ToString();
                                    if (column == "pay_due_date" || column == "stu_birthday")
                                    {
                                        DateTime? dateTime = DataFormat.ConvertDateText(value);
                                        if (dateTime != null)
                                        {
                                            value = dateTime.Value.ToString("yyyyMMdd");
                                        }
                                    }
                                    xw.WriteElementString(column, value);
                                }
                                xw.WriteEndElement();
                            }
                            #endregion

                            #region bill_detail 節點
                            {
                                xw.WriteStartElement("bill_detail");
                                for (int no = 1; no <= 40; no++)
                                {
                                    #region 收入科目名稱
                                    string receiveItemNameColumn = String.Format("receive_item_{0:00}_name", no);
                                    string receiveItemName = drow.IsNull(receiveItemNameColumn) ? null : drow[receiveItemNameColumn].ToString();
                                    #endregion

                                    #region 收入科目金額
                                    string receiveItemAmountColumn = String.Format("receive_item_{0:00}_amount", no);
                                    string receiveItemAmount = drow.IsNull(receiveItemAmountColumn) ? null : Convert.ToDecimal(drow[receiveItemAmountColumn]).ToString("0");
                                    #endregion

                                    if (!String.IsNullOrEmpty(receiveItemName))
                                    {
                                        xw.WriteElementString(receiveItemNameColumn, receiveItemName);

                                        #region [MDY:202204] 2022擴充案 依據 是否啟用英文資料 決定是否取 收入科目英文名稱
                                        if (isEngEnabled)
                                        {
                                            string receiveItemENameColumn = $"receive_item_{no:00}_ename";
                                            string receiveItemEName = drow.IsNull(receiveItemENameColumn) ? null : drow[receiveItemENameColumn].ToString();
                                            xw.WriteElementString(receiveItemENameColumn, receiveItemEName);
                                        }
                                        #endregion

                                        xw.WriteElementString(receiveItemAmountColumn, receiveItemAmount);
                                    }
                                }

                                #region [MDY:20211017] M202110_04 新增 國際信用卡繳費 NCCardFlag (2021擴充案先做)
                                {
                                    #region 繳費單繳款期限
                                    if (!drow.IsNull("bill_pay_due_date"))
                                    {
                                        DateTime? dateTime = DataFormat.ConvertDateText(drow["bill_pay_due_date"].ToString());
                                        if (dateTime.HasValue)
                                        {
                                            xw.WriteElementString("pay_due_date", dateTime.Value.ToString("yyyyMMdd"));
                                        }
                                    }
                                    #endregion

                                    #region 國際信用卡繳費
                                    if (!drow.IsNull("nccard_flag"))
                                    {
                                        string value = drow["nccard_flag"].ToString().Trim();
                                        if (value == "Y" || value == "N")
                                        {
                                            xw.WriteElementString("nccard_flag", value);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                xw.WriteEndElement();
                            }
                            #endregion

                            #region bill_info 節點
                            {
                                xw.WriteStartElement("bill_info");
                                foreach (string column in infoColumns)
                                {
                                    string value = drow.IsNull(column) ? String.Empty : drow[column].ToString();
                                    xw.WriteElementString(column, value);
                                }
                                xw.WriteEndElement();
                            }
                            #endregion

                            #region 資料根節點(bill)結束
                            xw.WriteEndElement();
                            #endregion

                            xw.Flush();
                        }

                        #region 根節點(landbank)結束
                        xw.WriteEndElement();
                        #endregion

                        xw.Flush();
                    }

                    #region 壓縮檔案
                    {
                        string zipFile = Path.Combine(tempPath, String.Format("{0}_BILL{1:yyyyMMddHHmmss}.zip", account.SysId, runTime));
                        if (File.Exists(zipFile))
                        {
                            File.Delete(zipFile);
                        }

                        #region [MDY:20210401] 原碼修正
                        ZIPHelper.ZipFile(xmlFile, zipFile, account.SysPXX);
                        #endregion

                        returnData = new ReturnData(ErrorList.NORMAL);
                        returnData.Records = dt.Rows.Count;
                        returnData.TxnFile = File.ReadAllBytes(zipFile);
                        return String.Format("Records={0}; TxnFile={1} ({2})", returnData.Records, zipFile, returnData.TxnFile == null ? 0 : returnData.TxnFile.Length);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    returnData = new ReturnData(ErrorList.S9999, "產生檔案發生例外");
                    return String.Format("產生檔案發生例外，錯誤訊息：{0}", ex.Message);
                }
            }
            #endregion
        }

        /// <summary>
        /// 入金資料查詢
        /// </summary>
        /// <param name="account"></param>
        /// <param name="qReceiveType"></param>
        /// <param name="cancel_no"></param>
        /// <param name="receive_way"></param>
        /// <param name="account_date"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public string PayQuery(SchoolServiceAccountEntity account, string qReceiveType, string qCancelNo, string qReceiveWay, string qAccountDate, out ReturnData returnData)
        {
            returnData = null;

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(qReceiveType))
            {
                string errmsg = "缺少商家代號查詢參數";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            qReceiveType = qReceiveType.Trim();

            if (qCancelNo != null)
            {
                qCancelNo = qCancelNo.Trim();
            }
            if (qReceiveWay != null)
            {
                qReceiveWay = qReceiveWay.Trim();
            }
            if (qAccountDate != null)
            {
                qAccountDate = qAccountDate.Trim();
                if (!String.IsNullOrEmpty(qAccountDate))
                {
                    DateTime date;
                    if (Common.TryConvertDate8(qAccountDate, out date))
                    {
                        qAccountDate = Common.GetTWDate7(date);
                    }
                    else
                    {
                        string errmsg = "入帳日查詢參數不正確";
                        returnData = new ReturnData(ErrorList.S0007, errmsg);
                        return errmsg;
                    }
                }
            }

            if (String.IsNullOrEmpty(qCancelNo) && String.IsNullOrEmpty(qReceiveWay) && String.IsNullOrEmpty(qAccountDate))
            {
                string errmsg = "虛擬帳號、代收管道或入帳日的查詢參數至少要指定一項";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            if (String.IsNullOrWhiteSpace(qCancelNo) && !String.IsNullOrWhiteSpace(qReceiveWay) && String.IsNullOrWhiteSpace(qAccountDate))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "指定代收管道查詢參數時必須同時指定入帳日的查詢參數");
            }

            if (!account.IsMyReceiveType(qReceiveType))
            {
                string errmsg = String.Format("查詢的商家代號 ({0}) 不在服務帳號授權裡", qReceiveType);
                returnData = new ReturnData(ErrorList.S0007, "商家代號未授權");
                return errmsg;
            }
            #endregion

            KeyValueList qParameters = new KeyValueList();
            string andSql = String.Empty;

            #region qCancelNo
            if (!String.IsNullOrEmpty(qCancelNo))
            {
                andSql += " AND [Cancel_No] = @CancelNo";
                qParameters.Add("@CancelNo", qCancelNo);
            }
            #endregion

            #region qReceiveWay
            if (!String.IsNullOrEmpty(qReceiveWay))
            {
                andSql += " AND [Receive_Way] = @ReceiveWay";
                qParameters.Add("@ReceiveWay", qReceiveWay);
            }
            #endregion

            #region qAccountDate
            if (!String.IsNullOrEmpty(qAccountDate))
            {
                andSql += " AND [Account_Date] = @AccountDate";
                qParameters.Add("@AccountDate", qAccountDate);
            }
            #endregion

            DataTable dt = null;

            #region 取得查詢資料
            {
                #region [MDY:20160325] 調整成與文件相同
                #region [Old]
//                string sql = @"SELECT [Service_Seq_No] AS [seq_no], [Receive_Type], [Year_Id], [Term_Id], [Receive_Id]
//     , [Cancel_No], [Receive_Amount] AS [amount], [Receive_Way], [Receive_Amount], [Account_Date]
//  FROM [Student_Receive]
// WHERE [Receive_Type] = @Receive_Type AND [Account_Date] IS NOT NULL AND [Account_Date] != ''
//  " + andSql;
                #endregion

                string sql = @"SELECT [Service_Seq_No] AS [seq_no], [Receive_Type], [Year_Id], [Term_Id], [Receive_Id]
     , [Cancel_No], [Receive_Amount], [Receive_Way], [Account_Date]
  FROM [Student_Receive]
 WHERE [Receive_Type] = @Receive_Type AND [Account_Date] IS NOT NULL AND [Account_Date] != ''
  " + andSql;
                #endregion

                qParameters.Add("@Receive_Type", qReceiveType);

                Result result = _Factory.GetDataTable(sql, qParameters, 0, 0, out dt);
                if (!result.IsSuccess)
                {
                    returnData = new ReturnData(ErrorList.S9999, "查詢學生繳費資料失敗");
                    return String.Format("查詢學生繳費資料失敗，錯誤訊息：{0}", result.Message);
                }
            }
            #endregion

            #region 將查詢結果轉成 XML 檔
            if (dt == null || dt.Rows.Count == 0)
            {
                returnData = new ReturnData(ErrorList.NORMAL);
                returnData.Records = 0;
                returnData.TxnFile = null;
                return "查無資料";
            }
            else
            {
                try
                {
                    string tempPath = ConfigManager.Current.GetSystemConfigValue("DATA_SERVICE", "TEMP_PATH");
                    if (String.IsNullOrEmpty(tempPath))
                    {
                        tempPath = Path.GetTempPath();
                    }
                    DateTime runTime = DateTime.Now;
                    string xmlFile = Path.Combine(tempPath, String.Format("{0}_PAY{1:yyyyMMddHHmmssfffff}.xml", account.SysId, runTime));
                    if (File.Exists(xmlFile))
                    {
                        File.Delete(xmlFile);
                    }

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.CheckCharacters = false;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;
                    settings.OmitXmlDeclaration = true;

                    string[] columns = new string[] {
                        "receive_type", "year_id", "term_id", "receive_id", "cancel_no", "receive_amount", "receive_way", "account_date"
                    };

                    using (XmlWriter xw = XmlWriter.Create(xmlFile, settings))
                    {
                        #region 根節點(landbank)開始
                        xw.WriteStartElement("landbank");
                        #endregion

                        foreach (DataRow drow in dt.Rows)
                        {
                            #region 資料根節點(pay_data)開始
                            xw.WriteStartElement("pay_data");
                            #endregion

                            foreach (string column in columns)
                            {
                                string value = drow.IsNull(column) ? String.Empty : drow[column].ToString();
                                if (column == "account_date")
                                {
                                    DateTime? dateTime = DataFormat.ConvertDateText(value);
                                    if (dateTime != null)
                                    {
                                        value = dateTime.Value.ToString("yyyyMMdd");
                                    }
                                }
                                xw.WriteElementString(column, value);
                            }

                            #region 資料根節點(pay_data)結束
                            xw.WriteEndElement();
                            #endregion

                            xw.Flush();
                        }

                        #region 根節點(landbank)結束
                        xw.WriteEndElement();
                        #endregion

                        xw.Flush();
                    }

                    #region 壓縮檔案
                    {
                        #region [MDY:20220530] Checkmarx 調整
                        #region [MDY:20210401] 原碼修正
                        #region [MDY:20161225] 系統驗證碼解密處理
                        string sysPXX = DataFormat.GetSysCWordDecode(account.SysPXX);
                        if (String.IsNullOrEmpty(sysPXX))
                        {
                            returnData = new ReturnData(ErrorList.S9999, "系統驗證碼解密處理失敗");
                            return "系統驗證碼解密處理失敗";
                        }
                        #endregion

                        string zipFile = Path.Combine(tempPath, String.Format("{0}_PAY{1:yyyyMMddHHmmss}.zip", account.SysId, runTime));
                        if (File.Exists(zipFile))
                        {
                            File.Delete(zipFile);
                        }
                        ZIPHelper.ZipFile(xmlFile, zipFile, sysPXX);
                        returnData = new ReturnData(ErrorList.NORMAL);
                        returnData.Records = dt.Rows.Count;
                        returnData.TxnFile = File.ReadAllBytes(zipFile);
                        return String.Format("Records={0}; TxnFile={1} ({2})", returnData.Records, zipFile, returnData.TxnFile == null ? 0 : returnData.TxnFile.Length);
                        #endregion
                        #endregion
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    returnData = new ReturnData(ErrorList.S9999, "產生檔案發生例外");
                    return String.Format("產生檔案發生例外，錯誤訊息：{0}", ex.Message);
                }
            }
            #endregion
        }

        #region [MDY:20191219] 新增繳費資訊查詢
        /// <summary>
        /// 繳費資訊查詢
        /// </summary>
        /// <param name="account"></param>
        /// <param name="qDataKind"></param>
        /// <param name="qReceiveType"></param>
        /// <param name="qCancelNos"></param>
        /// <param name="qSeqNos"></param>
        /// <param name="qReceiveDate"></param>
        /// <param name="startRecord"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        public string QueryData(SchoolServiceAccountEntity account, string qDataKind, string qReceiveType, string qCancelNos, string qSeqNos, string qReceiveDate, int startRecord, out ReturnData returnData)
        {
            returnData = null;

            #region 檢查參數
            #region 資料類別
            if (String.IsNullOrWhiteSpace(qDataKind))
            {
                string errmsg = "缺少資料類別查詢參數";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }

            #region [MDY:20211003] M202110_02 增加 C80、C81、C82、C83、C85、C87 的查詢異業代收的指定代收日最近15天繳費單已繳代銷資料
            bool isCDataKind = (qDataKind == "C80" || qDataKind == "C81" || qDataKind == "C82" || qDataKind == "C83" || qDataKind == "C85" || qDataKind == "C87");  //是否為查詢異業代收的指定代收日最近15天繳費單已繳代銷資料
            if (qDataKind != "1" && qDataKind != "2" && qDataKind != "3" && !isCDataKind)
            {
                string errmsg = "資料類別參數不正確";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            #endregion
            #endregion

            #region 商家代號
            if (String.IsNullOrWhiteSpace(qReceiveType))
            {
                string errmsg = "缺少商家代號查詢參數";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            qReceiveType = qReceiveType.Trim();
            #endregion

            string[] separator = new string[] { "," };

            #region 虛擬帳號清單
            List<string> qCancelNoList = new List<string>(20);
            if (!String.IsNullOrWhiteSpace(qCancelNos))
            {
                string[] cancelNos = qCancelNos.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string cancelNo in cancelNos)
                {
                    if (!String.IsNullOrWhiteSpace(cancelNo))
                    {
                        qCancelNoList.Add(cancelNo);
                        if (qCancelNoList.Count >= 20)
                        {
                            break;
                        }
                    }
                }
            }
            #endregion

            #region 學校端唯一序號清單
            List<string> qSeqNoList = new List<string>(20);
            if (!String.IsNullOrWhiteSpace(qSeqNos))
            {
                string[] seqNos = qSeqNos.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string seqNo in seqNos)
                {
                    if (!String.IsNullOrWhiteSpace(seqNo))
                    {
                        qSeqNoList.Add(seqNo);
                        if (qSeqNoList.Count >= 20)
                        {
                            break;
                        }
                    }
                }
            }
            if (qSeqNoList.Count > 0 && qDataKind == "1")
            {
                string errmsg = "資料類別參數為 1 時不支援學校端唯一序號清單參數";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            #endregion

            #region 代收日期
            #region [MDY:20211003] M202110_02 qDataKind 為 C80、C81、C82、C83、C85、C87 時，代收日期必須有值，且轉成最近15天的日期
            string[] qReceiveDates = null;  //最近15天代收日期
            if (!String.IsNullOrWhiteSpace(qReceiveDate))
            {
                DateTime date;
                if (Common.TryConvertDate8(qReceiveDate, out date))
                {
                    qReceiveDate = Common.GetTWDate7(date);

                    if (isCDataKind)
                    {
                        qReceiveDates = new string[15];
                        for (int idx = 0; idx < qReceiveDates.Length; idx++)
                        {
                            qReceiveDates[idx] = Common.GetTWDate7(date);
                            date = date.AddDays(-1);
                        }
                    }
                }
                else
                {
                    string errmsg = "代收日期查詢參數不正確";
                    returnData = new ReturnData(ErrorList.S0007, errmsg);
                    return errmsg;
                }
            }
            else if (isCDataKind)
            {
                string errmsg = "查詢異業代收的指定代收日最近15天繳費單已繳代銷資料時，代收日期必須有值";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            #endregion
            #endregion

            #region 起始資料位置
            if (startRecord < 1)
            {
                string errmsg = "起始資料位置查詢參數不正確";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }
            int startIndex = startRecord - 1;
            int maxRecords = 20;
            #endregion

            if (qCancelNoList.Count == 0 && qSeqNoList.Count == 0 && String.IsNullOrEmpty(qReceiveDate))
            {
                string errmsg = "虛擬帳號清單、學校端唯一序號清單或代收日期的查詢參數至少要指定一項";
                returnData = new ReturnData(ErrorList.S0007, errmsg);
                return errmsg;
            }

            #region [MDY:20211003] M202110_02 qDataKind 為 C80、C81、C82、C83、C85、C87 時 qCancelNoList、qSeqNoList 必須為空的
            if (isCDataKind)
            {
                if (qCancelNoList.Count != 0)
                {
                    string errmsg = "查詢異業代收的指定代收日最近15天繳費單已繳代銷資料時，虛擬帳號清單必須為空的";
                    returnData = new ReturnData(ErrorList.S0007, errmsg);
                    return errmsg;
                }
                if (qSeqNoList.Count != 0)
                {
                    string errmsg = "查詢異業代收的指定代收日最近15天繳費單已繳代銷資料時，學校端唯一序號清單必須為空的";
                    returnData = new ReturnData(ErrorList.S0007, errmsg);
                    return errmsg;
                }
            }
            #endregion

            if (account == null)
            {
                string errmsg = "缺少服務帳號";
                returnData = new ReturnData(ErrorList.S0001, errmsg);
                return errmsg;
            }
            if (!account.IsMyReceiveType(qReceiveType))
            {
                string errmsg = String.Format("查詢的商家代號 ({0}) 不在服務帳號授權裡", qReceiveType);
                returnData = new ReturnData(ErrorList.S0007, "商家代號未授權");
                return errmsg;
            }
            #endregion

            KeyValueList qParameters = new KeyValueList();
            StringBuilder andSql = new StringBuilder();

            #region qCancelNoList
            if (qCancelNoList.Count > 0)
            {
                string[] pNames = new string[qCancelNoList.Count];
                for (int idx = 0; idx < qCancelNoList.Count; idx++)
                {
                    pNames[idx] = String.Format("@CNO_{0:00}", idx + 1);
                    qParameters.Add(pNames[idx], qCancelNoList[idx]);
                }
                andSql.AppendFormat(" AND [Cancel_No] IN ({0})", String.Join(", ", pNames)).AppendLine();
            }
            #endregion

            #region qSeqNoList
            if (qSeqNoList.Count > 0)
            {
                string[] pNames = new string[qSeqNoList.Count];
                for (int idx = 0; idx < qSeqNoList.Count; idx++)
                {
                    pNames[idx] = String.Format("@SNO_{0:00}", idx + 1);
                    qParameters.Add(pNames[idx], qSeqNoList[idx]);
                }
                andSql.AppendFormat(" AND [Service_Seq_No] IN ({0})", String.Join(", ", pNames)).AppendLine();
            }
            #endregion

            #region qReceiveDate
            #region [MDY:20211003] M202110_02 qDataKind 為 C80、C81、C82、C83、C85、C87 時，代收管道與15天代收日期查詢條件
            if (isCDataKind)
            {
                qParameters.Add("@ReceiveWay", qDataKind.Substring(1, 2));
                andSql.AppendLine(" AND [Receive_Way] = @ReceiveWay");

                string[] parameterNames = new string[qReceiveDates.Length];
                for (int idx = 0; idx < qReceiveDates.Length; idx++)
                {
                    parameterNames[idx] = String.Format("@ReceiveDate{0}", idx + 1);
                    qParameters.Add(parameterNames[idx], qReceiveDates[idx]);
                }
                andSql.AppendFormat(" AND [Receive_Date] IN ({0})", String.Join(",", parameterNames)).AppendLine();
            }
            else if (!String.IsNullOrEmpty(qReceiveDate))
            {
                qParameters.Add("@ReceiveDate", qReceiveDate);
                andSql.AppendLine(" AND [Receive_Date] = @ReceiveDate");
            }
            #endregion
            #endregion

            #region 處理資料
            Int32 dataCount = 0;
            DataTable dt = null;
            {
                string sqlCount = null;
                string sqlData = null;
                switch (qDataKind)
                {
                    case "1":
                        #region 帳務中心資料
                        {
                            sqlCount = String.Format(@"
SELECT COUNT(1)
  FROM [Cancel_Debts]
 WHERE [Receive_Type] = @ReceiveType
{0} ", andSql).Trim();
                            sqlData = String.Format(@"
SELECT '' AS [seq_no], [Cancel_No], [Receive_Amount], [Receive_Way], [Receive_Date], [Receive_Time], [Receive_Bank] AS [Bank_Id], [Reserve2] AS [Flag], [Account_Date]
  FROM [Cancel_Debts]
 WHERE [Receive_Type] = @ReceiveType
{0}
 ORDER BY [SNo] ", andSql).Trim();
                        }
                        #endregion
                        break;
                    case "2":
                        #region 繳費單已繳代銷資料
                        {
                            sqlCount = String.Format(@"
SELECT COUNT(1)
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType
   AND ([Receive_Way] IS NOT NULL AND [Receive_Way] != '')
   AND ([Receive_Date] IS NOT NULL AND [Receive_Date] != '')
   AND ([Account_Date] IS NULL OR [Account_Date] = '')
{0} ", andSql).Trim();

                            sqlData = String.Format(@"
SELECT [Service_Seq_No] AS [seq_no], [Cancel_No], [Receive_Amount], [Receive_Way], [Receive_Date], [Receive_Time], [Receivebank_Id] AS [Bank_Id], '' AS [Flag], [Account_Date]
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType
   AND ([Receive_Way] IS NOT NULL AND [Receive_Way] != '')
   AND ([Receive_Date] IS NOT NULL AND [Receive_Date] != '')
   AND ([Account_Date] IS NULL OR [Account_Date] = '')
{0}
 ORDER BY [Receive_Date], [Receive_Time] ", andSql).Trim();
                        }
                        #endregion
                        break;
                    case "3":
                        #region 繳費單已銷資料
                        {
                            sqlCount = String.Format(@"
SELECT COUNT(1)
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType
   AND ([Receive_Way] IS NOT NULL AND [Receive_Way] != '')
   AND ([Receive_Date] IS NOT NULL AND [Receive_Date] != '')
   AND ([Account_Date] IS NOT NULL AND [Account_Date] != '')
{0} ", andSql).Trim();

                            sqlData = String.Format(@"
SELECT [Service_Seq_No] AS [seq_no], [Cancel_No], [Receive_Amount], [Receive_Way], [Receive_Date], [Receive_Time], [Receivebank_Id] AS [Bank_Id], '' AS [Flag], [Account_Date]
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType
   AND ([Receive_Way] IS NOT NULL AND [Receive_Way] != '')
   AND ([Receive_Date] IS NOT NULL AND [Receive_Date] != '')
   AND ([Account_Date] IS NOT NULL AND [Account_Date] != '')
{0}
 ORDER BY [Receive_Date], [Receive_Time] ", andSql).Trim();
                        }
                        #endregion
                        break;

                    #region [MDY:20211003] M202110_02 增加 C80、C81、C82、C83、C85、C87 的查詢異業代收的指定代收日最近15天繳費單已繳代銷資料
                    case "C80":
                    case "C81":
                    case "C82":
                    case "C83":
                    case "C85":
                    case "C87":
                        #region 異業代收的指定代收日最近15天繳費單已繳代銷資料
                        {
                            sqlCount = String.Format(@"
SELECT COUNT(1)
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType
   AND ([Receive_Way] IS NOT NULL AND [Receive_Way] != '')
   AND ([Receive_Date] IS NOT NULL AND [Receive_Date] != '')
   AND ([Account_Date] IS NULL OR [Account_Date] = '')
{0} ", andSql).Trim();

                            sqlData = String.Format(@"
SELECT [Service_Seq_No] AS [seq_no], [Cancel_No], [Receive_Amount], [Receive_Way], [Receive_Date], [Receive_Time], [Receivebank_Id] AS [Bank_Id], '' AS [Flag], [Account_Date]
  FROM [Student_Receive]
 WHERE [Receive_Type] = @ReceiveType
   AND ([Receive_Way] IS NOT NULL AND [Receive_Way] != '')
   AND ([Receive_Date] IS NOT NULL AND [Receive_Date] != '')
   AND ([Account_Date] IS NULL OR [Account_Date] = '')
{0}
 ORDER BY [Receive_Date], [Receive_Time], [Cancel_No] ", andSql).Trim();
                        }
                        #endregion
                        break;
                    #endregion
                }

                qParameters.Add("@ReceiveType", qReceiveType);

                object value = null;
                Result result = _Factory.ExecuteScalar(sqlCount, qParameters, out value);
                if (!result.IsSuccess)
                {
                    returnData = new ReturnData(ErrorList.S9999, "查詢資料筆數失敗");
                    return String.Format("查詢資料筆數失敗，錯誤訊息：{0}", result.Message);
                }
                dataCount = Convert.ToInt32(value);
                if (dataCount > 0)
                {
                    if (dataCount < startRecord)
                    {
                        returnData = new ReturnData(ErrorList.S0007, "起始資料位置參數超過資料筆數");
                        return String.Format("起始資料位置參數超過資料筆數 ({0})", dataCount);
                    }

                    result = _Factory.GetDataTable(sqlData, qParameters, startIndex, maxRecords, out dt);
                    if (!result.IsSuccess)
                    {
                        returnData = new ReturnData(ErrorList.S9999, "查詢學生繳費資料失敗");
                        return String.Format("查詢學生繳費資料失敗，錯誤訊息：{0}", result.Message);
                    }
                }
            }
            #endregion

            #region 將查詢結果轉成 XML 檔
            if (dt == null || dt.Rows.Count == 0)
            {
                returnData = new ReturnData(ErrorList.NORMAL);
                returnData.Records = dataCount;
                returnData.TxnFile = null;
                return "查無資料";
            }
            else
            {
                try
                {
                    string tempPath = ConfigManager.Current.GetSystemConfigValue("DATA_SERVICE", "TEMP_PATH");
                    if (String.IsNullOrEmpty(tempPath))
                    {
                        tempPath = Path.GetTempPath();
                    }
                    DateTime runTime = DateTime.Now;
                    string xmlFile = Path.Combine(tempPath, String.Format("{0}_DATA{1:yyyyMMddHHmmssfffff}.xml", account.SysId, runTime));
                    if (File.Exists(xmlFile))
                    {
                        File.Delete(xmlFile);
                    }

                    #region 產生 Xml 檔
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.CheckCharacters = false;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;
                    settings.OmitXmlDeclaration = true;

                    string[] columns = new string[] {
                        "seq_no", "cancel_no", "receive_amount", "receive_way", "receive_date", "receive_time", "bank_id", "flag", "account_date"
                    };

                    using (XmlWriter xw = XmlWriter.Create(xmlFile, settings))
                    {
                        #region 根節點(landbank)開始
                        xw.WriteStartElement("landbank");
                        #endregion

                        foreach (DataRow drow in dt.Rows)
                        {
                            #region 資料根節點(data)開始
                            xw.WriteStartElement("data");
                            #endregion

                            #region 資料欄節點
                            foreach (string column in columns)
                            {
                                string value = drow.IsNull(column) ? String.Empty : drow[column].ToString();
                                if (column == "receive_date" || column == "account_date")
                                {
                                    DateTime? dateTime = DataFormat.ConvertDateText(value);
                                    if (dateTime != null)
                                    {
                                        value = dateTime.Value.ToString("yyyyMMdd");
                                    }
                                }
                                xw.WriteElementString(column, value);
                            }
                            #endregion

                            #region 資料根節點(data)結束
                            xw.WriteEndElement();
                            #endregion

                            xw.Flush();
                        }

                        #region 根節點(landbank)結束
                        xw.WriteEndElement();
                        #endregion

                        xw.Flush();
                    }
                    #endregion

                    #region 壓縮檔案
                    {
                        #region [MDY:20220530] Checkmarx 調整
                        #region [MDY:20210401] 原碼修正
                        #region 系統驗證碼解密處理
                        string sysPXX = DataFormat.GetSysCWordDecode(account.SysPXX);
                        if (String.IsNullOrEmpty(sysPXX))
                        {
                            returnData = new ReturnData(ErrorList.S9999, "系統驗證碼解密處理失敗");
                            return "系統驗證碼解密處理失敗";
                        }
                        #endregion

                        string zipFile = Path.Combine(tempPath, String.Format("{0}_DATA{1:yyyyMMddHHmmss}.zip", account.SysId, runTime));
                        if (File.Exists(zipFile))
                        {
                            File.Delete(zipFile);
                        }
                        ZIPHelper.ZipFile(xmlFile, zipFile, sysPXX);
                        returnData = new ReturnData(ErrorList.NORMAL);
                        returnData.Records = dataCount;
                        returnData.TxnFile = File.ReadAllBytes(zipFile);
                        return String.Format("Records={0}; TxnFile={1} ({2})", returnData.Records, zipFile, returnData.TxnFile == null ? 0 : returnData.TxnFile.Length);
                        #endregion
                        #endregion
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    returnData = new ReturnData(ErrorList.S9999, "產生檔案發生例外");
                    return String.Format("產生檔案發生例外，錯誤訊息：{0}", ex.Message);
                }
            }
            #endregion
        }
        #endregion
        #endregion
    }
}
