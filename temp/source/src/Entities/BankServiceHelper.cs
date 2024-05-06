using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

using Fuju.Configuration;
using Fuju.Web;
using Fuju.Web.Services;

using Entities;

namespace Entities.BankService
{
    /// <summary>
    /// 訊息代碼文字定義清單類別
    /// </summary>
    public class ErrorList : CodeTextList
    {
        #region 訊息代碼
        //0000：交易成功
        //S999：傳入參數錯誤
        //C001：IP與申請IP不符
        //C002：機關代碼錯誤
        //C003：密碼錯誤
        //1001：查詢條件錯誤
        //1002：查無應繳資料
        //1003：查無符合條件的資料
        //2001：銷帳編號錯誤或是空白
        //2002：該銷帳編號已繳款
        //2003：查無該銷帳編號資料
        //2004：繳款期限錯誤
        //2005：無需繳款
        //2006：已過繳款期限
        //2007：委託單位尚未向本行申請信用卡繳款管道
        //3001：銷帳編號錯誤或是空白
        //3002：繳款金額空白
        //3003：繳款日期空白
        //3004：繳款時間空白
        //3005：授權碼空白
        //3006：該銷帳編號已繳款
        //3007：該銷帳編號已過繳費期限
        //3008：該銷帳編號應繳金額不符
        //3009：無該銷帳編號資料
        //3010：交易序號為空白
        //3011：繳款日期格式錯誤
        //3012：繳款時間錯誤
        //3013：繳款期限錯誤
        //3014：MAC不符
        #endregion

        #region Const Code
        /// <summary>
        /// 交易成功 : 0000
        /// </summary>
        public const string NORMAL = "0000";

        #region S 類
        /// <summary>
        /// 傳入參數錯誤 : S999
        /// </summary>
        public const string S999 = "S999";

        /// <summary>
        /// 系統處理發生錯誤 : S990
        /// </summary>
        public const string S990 = "S990";
        #endregion

        #region C 類
        /// <summary>
        /// IP與申請IP不符 : C001
        /// </summary>
        public const string C001 = "C001";

        /// <summary>
        /// 機關代碼錯誤 : C002
        /// </summary>
        public const string C002 = "C002";

        /// <summary>
        /// 密碼錯誤 : C003
        /// </summary>
        public const string C003 = "C003";
        #endregion

        #region D 類
        /// <summary>
        /// 查詢條件錯誤 : 1001
        /// </summary>
        public const string D1001 = "1001";

        /// <summary>
        /// 查無應繳資料 : 1002
        /// </summary>
        public const string D1002 = "1002";

        /// <summary>
        /// 查無符合條件的資料 : 1003
        /// </summary>
        public const string D1003 = "1003";

        /// <summary>
        /// 銷帳編號錯誤或是空白 : 2001
        /// </summary>
        public const string D2001 = "2001";

        /// <summary>
        /// 該銷帳編號已繳款 : 2002
        /// </summary>
        public const string D2002 = "2002";

        /// <summary>
        /// 查無該銷帳編號資料 : 2003
        /// </summary>
        public const string D2003 = "2003";

        /// <summary>
        /// 繳款期限錯誤 : 2004
        /// </summary>
        public const string D2004 = "2004";

        /// <summary>
        /// 無需繳款 : 2005
        /// </summary>
        public const string D2005 = "2005";

        /// <summary>
        /// 已過繳款期限 : 2006
        /// </summary>
        public const string D2006 = "2006";

        /// <summary>
        /// 委託單位尚未向本行申請信用卡繳款管道 : 2007
        /// </summary>
        public const string D2007 = "2007";

        /// <summary>
        /// 銷帳編號錯誤或是空白 : 3001
        /// </summary>
        public const string D3001 = "3001";

        /// <summary>
        /// 繳款金額空白 : 3002
        /// </summary>
        public const string D3002 = "3002";

        /// <summary>
        /// 繳款日期空白 : 3003
        /// </summary>
        public const string D3003 = "3003";

        /// <summary>
        /// 繳款時間空白 : 3004
        /// </summary>
        public const string D3004 = "3004";

        /// <summary>
        /// 授權碼空白 : 3005
        /// </summary>
        public const string D3005 = "3005";

        /// <summary>
        /// 該銷帳編號已繳款 : 3006
        /// </summary>
        public const string D3006 = "3006";

        /// <summary>
        /// 該銷帳編號已過繳費期限 : 3007
        /// </summary>
        public const string D3007 = "3007";

        /// <summary>
        /// 該銷帳編號應繳金額不符 : 3008
        /// </summary>
        public const string D3008 = "3008";

        /// <summary>
        /// 無該銷帳編號資料 : 3009
        /// </summary>
        public const string D3009 = "3009";

        /// <summary>
        /// 交易序號為空白 : 3010
        /// </summary>
        public const string D3010 = "3010";

        /// <summary>
        /// 繳款日期格式錯誤 : 3011
        /// </summary>
        public const string D3011 = "3011";

        /// <summary>
        /// 繳款時間錯誤 : 3012
        /// </summary>
        public const string D3012 = "3012";

        /// <summary>
        /// 繳款期限錯誤 : 3013
        /// </summary>
        public const string D3013 = "3013";

        /// <summary>
        /// MAC不符 : 3014
        /// </summary>
        public const string D3014 = "3014";
        #endregion
        #endregion

        #region Const Text
        /// <summary>
        /// 交易成功 : 0000
        /// </summary>
        public const string NORMAL_TEXT = "交易成功";

        #region S 類
        /// <summary>
        /// 傳入參數錯誤 : S999
        /// </summary>
        public const string S999_TEXT = "傳入參數錯誤";
        #endregion

        #region C 類
        /// <summary>
        /// IP與申請IP不符 : C001
        /// </summary>
        public const string C001_TEXT = "IP與申請IP不符";

        /// <summary>
        /// 機關代碼錯誤 : C002
        /// </summary>
        public const string C002_TEXT = "機關代碼錯誤";

        /// <summary>
        /// 密碼錯誤 : C003
        /// </summary>
        public const string C003_TEXT = "密碼錯誤";
        #endregion

        #region D 類
        /// <summary>
        /// 查詢條件錯誤 : 1001
        /// </summary>
        public const string D1001_TEXT = "查詢條件錯誤";

        /// <summary>
        /// 查無應繳資料 : 1002
        /// </summary>
        public const string D1002_TEXT = "查無應繳資料";

        /// <summary>
        /// 查無符合條件的資料 : 1003
        /// </summary>
        public const string D1003_TEXT = "查無符合條件的資料";

        /// <summary>
        /// 銷帳編號錯誤或是空白 : 2001
        /// </summary>
        public const string D2001_TEXT = "銷帳編號錯誤或是空白";

        /// <summary>
        /// 該銷帳編號已繳款 : 2002
        /// </summary>
        public const string D2002_TEXT = "該銷帳編號已繳款";

        /// <summary>
        /// 查無該銷帳編號資料 : 2003
        /// </summary>
        public const string D2003_TEXT = "查無該銷帳編號資料";

        /// <summary>
        /// 繳款期限錯誤 : 2004
        /// </summary>
        public const string D2004_TEXT = "繳款期限錯誤";

        /// <summary>
        /// 無需繳款 : 2005
        /// </summary>
        public const string D2005_TEXT = "無需繳款";

        /// <summary>
        /// 已過繳款期限 : 2006
        /// </summary>
        public const string D2006_TEXT = "已過繳款期限";

        /// <summary>
        /// 委託單位尚未向本行申請信用卡繳款管道 : 2007
        /// </summary>
        public const string D2007_TEXT = "委託單位尚未向本行申請信用卡繳款管道";

        /// <summary>
        /// 銷帳編號錯誤或是空白 : 3001
        /// </summary>
        public const string D3001_TEXT = "銷帳編號錯誤或是空白";

        /// <summary>
        /// 繳款金額空白 : 3002
        /// </summary>
        public const string D3002_TEXT = "繳款金額空白";

        /// <summary>
        /// 繳款日期空白 : 3003
        /// </summary>
        public const string D3003_TEXT = "繳款日期空白";

        /// <summary>
        /// 繳款時間空白 : 3004
        /// </summary>
        public const string D3004_TEXT = "繳款時間空白";

        /// <summary>
        /// 授權碼空白 : 3005
        /// </summary>
        public const string D3005_TEXT = "授權碼空白";

        /// <summary>
        /// 該銷帳編號已繳款 : 3006
        /// </summary>
        public const string D3006_TEXT = "該銷帳編號已繳款";

        /// <summary>
        /// 該銷帳編號已過繳費期限 : 3007
        /// </summary>
        public const string D3007_TEXT = "該銷帳編號已過繳費期限";

        /// <summary>
        /// 該銷帳編號應繳金額不符 : 3008
        /// </summary>
        public const string D3008_TEXT = "該銷帳編號應繳金額不符";

        /// <summary>
        /// 無該銷帳編號資料 : 3009
        /// </summary>
        public const string D3009_TEXT = "無該銷帳編號資料";

        /// <summary>
        /// 交易序號為空白 : 3010
        /// </summary>
        public const string D3010_TEXT = "交易序號為空白";

        /// <summary>
        /// 繳款日期格式錯誤 : 3011
        /// </summary>
        public const string D3011_TEXT = "繳款日期格式錯誤";

        /// <summary>
        /// 繳款時間錯誤 : 3012
        /// </summary>
        public const string D3012_TEXT = "繳款時間錯誤";

        /// <summary>
        /// 繳款期限錯誤 : 3013
        /// </summary>
        public const string D3013_TEXT = "繳款期限錯誤";

        /// <summary>
        /// MAC不符 : 3014
        /// </summary>
        public const string D3014_TEXT = "MAC不符";
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
            base.Add(S999, S999_TEXT);
            #endregion

            #region C 類
            base.Add(C001, C001_TEXT);
            base.Add(C002, C002_TEXT);
            #endregion

            #region D 類
            base.Add(D1001, D1001_TEXT);
            base.Add(D1002, D1002_TEXT);
            base.Add(D1003, D1003_TEXT);

            base.Add(D2001, D2001_TEXT);
            base.Add(D2002, D2002_TEXT);
            base.Add(D2003, D2003_TEXT);
            base.Add(D2004, D2004_TEXT);
            base.Add(D2005, D2005_TEXT);
            base.Add(D2006, D2006_TEXT);
            base.Add(D2007, D2007_TEXT);

            base.Add(D3001, D3001_TEXT);
            base.Add(D3002, D3002_TEXT);
            base.Add(D3003, D3003_TEXT);
            base.Add(D3004, D3004_TEXT);
            base.Add(D3005, D3005_TEXT);
            base.Add(D3006, D3006_TEXT);
            base.Add(D3007, D3007_TEXT);
            base.Add(D3008, D3008_TEXT);
            base.Add(D3009, D3009_TEXT);
            base.Add(D3010, D3010_TEXT);
            base.Add(D3011, D3011_TEXT);
            base.Add(D3012, D3012_TEXT);
            base.Add(D3013, D3013_TEXT);
            base.Add(D3014, D3014_TEXT);
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
        #endregion
    }

    /// <summary>
    /// 交易參數承載基底抽象類別
    /// </summary>
    public abstract class BaseTxnData
    {
        #region Const
        public const string RootNodeName = "Bot";
        #endregion

        #region Property
        private string _Op = null;
        /// <summary>
        /// 交易代碼 (op)
        /// </summary>
        public virtual string Op
        {
            get
            {
                return _Op;
            }
            set
            {
                _Op = value == null ? null : value.Trim();
            }
        }

        private string _OrgId = null;
        /// <summary>
        /// 機關代號 (orgid)
        /// </summary>
        public virtual string OrgId
        {
            get
            {
                return _OrgId;
            }
            set
            {
                _OrgId = value == null ? null : value.Trim();
            }
        }

        #region [MDY:20220530] Checkmarx 調整
        #region [MDY:20210401] 原碼修正
        private string _PXX = null;
        /// <summary>
        /// 使用密碼 (PXX)
        /// </summary>
        public virtual string PXX
        {
            get
            {
                return _PXX;
            }
            set
            {
                _PXX = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 BaseTxnData 類別
        /// </summary>
        protected BaseTxnData()
        {

        }

        #region [MDY:20210401] 原碼修正
        /// <summary>
        /// 建構 BaseTxnData 類別
        /// </summary>
        /// <param name="op"></param>
        /// <param name="orgId"></param>
        /// <param name="pxx"></param>
        protected BaseTxnData(string op, string orgId, string pxx)
        {
            this.Op = op;
            this.OrgId = orgId;
            this.PXX = pxx;
        }
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 檢查交易參數是否準備好
        /// </summary>
        /// <returns>傳回錯誤代碼 (請參考 Entities.BankService.ErrorList)</returns>
        public virtual string IsReady()
        {
            #region [MDY:20210401] 原碼修正
            if (String.IsNullOrEmpty(this.Op) || String.IsNullOrEmpty(this.OrgId) || String.IsNullOrEmpty(this.PXX))
            {
                return ErrorList.S999;
            }
            else
            {
                return ErrorList.NORMAL;
            }
            #endregion
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initial()
        {
            this.Op = null;
            this.OrgId = null;

            #region [MDY:20210401] 原碼修正
            this.PXX = null;
            #endregion
        }

        /// <summary>
        /// 載入交易參數 xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="errmsg"></param>
        /// <returns>傳回錯誤代碼 (請參考 Entities.BankService.ErrorList)</returns>
        public string LoadXml(string xml, out string errmsg)
        {
            this.Initial();
            errmsg = null;
            try
            {
                XmlDocument xDoc = new XmlDocument();

                #region [MDY:20160329] 修正 FORTIFY 弱點
                #region [Old]
                //xDoc.LoadXml(xml);
                #endregion

                using (MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(xml)))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Prohibit;
                    settings.XmlResolver = null;
                    using (XmlReader reader = XmlReader.Create(stream, settings))
                    {
                        xDoc.Load(reader);
                    }
                }
                #endregion

                XmlNode xRoot = xDoc.DocumentElement;

                if (xRoot.Name == RootNodeName)
                {
                    StringBuilder msg = new StringBuilder();
                    XmlNode xNode = null;

                    #region 交易代碼 (op)
                    {
                        xNode = xRoot.SelectSingleNode("op");
                        if (xNode != null)
                        {
                            this.Op = xNode.InnerText;
                        }
                        else
                        {
                            msg.AppendLine("缺少交易代碼 (op) 節點");
                        }
                    }
                    #endregion

                    #region 機關代號 (orgid)
                    {
                        xNode = xRoot.SelectSingleNode("orgid");
                        if (xNode != null)
                        {
                            this.OrgId = xNode.InnerText;
                        }
                        else
                        {
                            msg.AppendLine("缺少機關代號 (orgid) 節點");
                        }
                    }
                    #endregion

                    #region [MDY:20220530] Checkmarx 調整
                    #region [MDY:20210401] 原碼修正
                    #region 機關驗證碼 (PXX)
                    {
                        xNode = xRoot.SelectSingleNode("pwd");
                        if (xNode != null)
                        {
                            this.PXX = xNode.InnerText;
                        }
                        else
                        {
                            msg.AppendLine("缺少機關驗證碼 (pwd) 節點");
                        }
                    }
                    #endregion
                    #endregion
                    #endregion

                    #region 繼承者的資料節點
                    {
                        string errmsg2 = null;
                        if (!this.LoadMyXmlNode(xRoot, out errmsg2))
                        {
                            msg.Append(errmsg2);
                        }
                    }
                    #endregion

                    errmsg = msg.ToString();
                }
                else
                {
                    errmsg = String.Format("缺少或不正確的根節點 ({0})", RootNodeName);
                }

                if (String.IsNullOrEmpty(errmsg))
                {
                    return ErrorList.NORMAL;
                }
                else
                {
                    return ErrorList.S999;
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("執行 BaseTxnData.LoadXml() 方法失敗，錯誤訊息：{0}", ex.Message);
                return ErrorList.S999;
            }
        }

        /// <summary>
        /// 載入繼承者所屬節點的資料
        /// </summary>
        /// <param name="xRoot"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        protected abstract bool LoadMyXmlNode(XmlNode xRoot, out string errmsg);
        #endregion
    }

    /// <summary>
    /// 以銷帳編號查詢繳費單資訊的查詢參數承載類別
    /// </summary>
    public sealed class Q0002TxnData : BaseTxnData
    {
        #region Const
        public const string MyOP = "Q0002";
        #endregion

        #region Property
        private string _Rid = null;
        /// <summary>
        /// 銷帳編號 (rid)
        /// </summary>
        public string Rid
        {
            get
            {
                return _Rid;
            }
            set
            {
                _Rid = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 Q0002TxnData 類別
        /// </summary>
        public Q0002TxnData()
            : base()
        {
            this.Op = MyOP;
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 建構 Q0002TxnData 類別
        /// </summary>
        /// <param name="op"></param>
        /// <param name="orgId"></param>
        /// <param name="pxx"></param>
        /// <param name="rid"></param>
        private Q0002TxnData(string op, string orgId, string pxx, string rid)
            : base(op, orgId, pxx)
        {
            this.Rid = rid;
        }
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 檢查交易參數
        /// </summary>
        /// <returns>傳回錯誤代碼 (請參考 Entities.BankService.ErrorList)</returns>
        public override string IsReady()
        {
            string errCode = base.IsReady();
            if (errCode != ErrorList.NORMAL)
            {
                return errCode;
            }
            if (this.Op != MyOP)
            {
                return ErrorList.S999;
            }
            if (String.IsNullOrEmpty(this.Rid))
            {
                return ErrorList.D1001;
            }
            return ErrorList.NORMAL;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initial()
        {
            base.Initial();
            this.Rid = null;
        }

        /// <summary>
        /// 載入所屬節點的資料
        /// </summary>
        /// <param name="xRoot"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        protected override bool LoadMyXmlNode(XmlNode xRoot, out string errmsg)
        {
            if (xRoot != null && xRoot.Name == RootNodeName)
            {
                errmsg = null;
                StringBuilder msg = new StringBuilder();
                XmlNode xNode = null;

                #region 銷帳編號 (rid)
                {
                    xNode = xRoot.SelectSingleNode("rid");
                    if (xNode != null)
                    {
                        this.Rid = xNode.InnerText;
                    }
                    else
                    {
                        msg.AppendLine("缺少銷帳編號 (rid) 節點");
                    }
                }
                #endregion

                errmsg = msg.ToString();

                return String.IsNullOrEmpty(errmsg);
            }
            else
            {
                errmsg = String.Format("缺少或不正確的根節點 ({0})", RootNodeName);
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 信用卡繳費成功通知的通知參數承載類別
    /// </summary>
    public sealed class T0001TxnData : BaseTxnData
    {
        #region Const
        public const string MyOP = "T0001";
        #endregion

        #region Property
        private string _SeqNo = null;
        /// <summary>
        /// 交易序號 (seqno)
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

        private string _Rid = null;
        /// <summary>
        /// 銷帳編號 (rid)
        /// </summary>
        public string Rid
        {
            get
            {
                return _Rid;
            }
            set
            {
                _Rid = value == null ? null : value.Trim();
            }
        }

        private string _Amount = null;
        /// <summary>
        /// 繳款金額 (amount)
        /// </summary>
        public string Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value == null ? null : value.Trim();
            }
        }

        private string _PayDate = null;
        /// <summary>
        /// 代收日期 (paydate) (日期格式: yyyymmdd)
        /// </summary>
        public string PayDate
        {
            get
            {
                return _PayDate;
            }
            set
            {
                _PayDate = value == null ? null : value.Trim();
            }
        }

        private string _PayTime = null;
        /// <summary>
        /// 代收時間 (paytime) (時間格式: hhmmss)
        /// </summary>
        public string PayTime
        {
            get
            {
                return _PayTime;
            }
            set
            {
                _PayTime = value == null ? null : value.Trim();
            }
        }

        private string _AuthCode = null;
        /// <summary>
        /// 交易授權碼 (authcode)
        /// </summary>
        public string AuthCode
        {
            get
            {
                return _AuthCode;
            }
            set
            {
                _AuthCode = value == null ? null : value.Trim();
            }
        }

        private string _MAC = null;
        /// <summary>
        /// 訊息認證押碼 (MAC)
        /// </summary>
        public string MAC
        {
            get
            {
                return _MAC;
            }
            set
            {
                _MAC = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 T0001TxnData 類別
        /// </summary>
        public T0001TxnData()
            : base()
        {
            this.Op = MyOP;
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 建構 T0001TxnData 類別
        /// </summary>
        /// <param name="op"></param>
        /// <param name="orgId"></param>
        /// <param name="pxx"></param>
        private T0001TxnData(string op, string orgId, string pxx)
            : base(op, orgId, pxx)
        {
        }
        #endregion
        #endregion

        public Decimal? GetAmount()
        {
            Decimal amount;
            if (!String.IsNullOrEmpty(this.Amount) && Decimal.TryParse(this.Amount, out amount))
            {
                return amount;
            }
            else
            {
                return null;
            }
        }

        public DateTime? GetPayDate()
        {
            DateTime date;
            if (!String.IsNullOrEmpty(this.PayDate) && Common.TryConvertDate8(this.PayDate, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

        public TimeSpan? GetPayTime()
        {
            TimeSpan value;
            string payTime = this.PayTime ?? String.Empty;
            if (payTime.Length == 6 && TimeSpan.TryParse(payTime.Insert(4, ":").Insert(2, ":"), out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        #region [MDY:20220910] Checkmarx - Use Of Broken Or Risky Cryptographic Algorithm 誤判調整
        #region [OLD] Checkmarx 不允許用 MD5CryptoServiceProvider
        //public static string GetMAC(string seqno, string orgid, string rid, string amount, string paydate)
        //{
        //    try
        //    {
        //        seqno = seqno == null ? String.Empty : seqno.Trim();
        //        if (seqno.Length < 16)
        //        {
        //            seqno = seqno.PadRight(16, ' ');
        //        }
        //        orgid = orgid == null ? String.Empty : orgid.Trim();
        //        if (orgid.Length < 16)
        //        {
        //            orgid = orgid.PadRight(16, ' ');
        //        }
        //        rid = rid == null ? String.Empty : rid.Trim();
        //        if (rid.Length < 20)
        //        {
        //            rid = rid.PadRight(20, ' ');
        //        }
        //        amount = amount == null ? String.Empty : amount.Trim();
        //        if (amount.Length < 9)
        //        {
        //            amount = amount.PadRight(9, ' ');
        //        }
        //        paydate = paydate == null ? String.Empty : paydate.Trim();
        //        if (paydate.Length < 8)
        //        {
        //            paydate = paydate.PadRight(8, ' ');
        //        }

        //        string value = String.Concat(seqno, orgid, rid, amount, paydate);
        //        byte[] data = null;
        //        using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
        //        {
        //            data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
        //        }

        //        if (data != null && data.Length > 0)
        //        {
        //            StringBuilder sBuilder = new StringBuilder();
        //            for (int i = 0; i < data.Length; i++)
        //            {
        //                sBuilder.Append(data[i].ToString("x2"));
        //            }
        //            return sBuilder.ToString();
        //        }
        //        else
        //        {
        //            return String.Empty;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return String.Empty;
        //    }
        //}
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 檢查交易參數
        /// </summary>
        /// <returns>傳回錯誤代碼 (請參考 Entities.BankService.ErrorList)</returns>
        public override string IsReady()
        {
            string errCode = base.IsReady();
            if (errCode != ErrorList.NORMAL)
            {
                return errCode;
            }
            if (this.Op != MyOP)
            {
                return ErrorList.S999;
            }
            if (String.IsNullOrEmpty(this.SeqNo))
            {
                return ErrorList.D3010;
            }
            if (this.SeqNo.Length > 16)
            {
                return ErrorList.S999;
            }
            if (String.IsNullOrEmpty(this.Rid))
            {
                return ErrorList.D3001;
            }
            if (String.IsNullOrEmpty(this.Amount))
            {
                return ErrorList.D3002;
            }
            if (this.GetAmount() == null)
            {
                return ErrorList.S999;
            }
            if (String.IsNullOrEmpty(this.PayDate))
            {
                return ErrorList.D3003;
            }
            if (this.GetPayDate() == null)
            {
                return ErrorList.D3011;
            }
            if (String.IsNullOrEmpty(this.PayTime))
            {
                return ErrorList.D3004;
            }
            if (this.GetPayTime() == null)
            {
                return ErrorList.D3012;
            }
            if (String.IsNullOrEmpty(this.AuthCode))
            {
                return ErrorList.D3005;
            }

            #region [MDY:20220910] Checkmarx - Use Of Broken Or Risky Cryptographic Algorithm 誤判調整
            if (String.IsNullOrEmpty(this.MAC) || this.MAC != Fuju.Help.LBHelper.GetBankServiceMAC(this.SeqNo, this.OrgId, this.Rid, this.Amount, this.PayDate))
            {
                return ErrorList.D3014;
            }
            #endregion

            return ErrorList.NORMAL;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initial()
        {
            base.Initial();
            this.SeqNo = null;
            this.Rid = null;
            this.Amount = null;
            this.PayDate = null;
            this.PayTime = null;
            this.AuthCode = null;
            this.MAC = null;
        }

        /// <summary>
        /// 載入所屬節點的資料
        /// </summary>
        /// <param name="xRoot">指定載入的根節點</param>
        /// <param name="errmsg">傳回錯誤訊息</param>
        /// <returns></returns>
        protected override bool LoadMyXmlNode(XmlNode xRoot, out string errmsg)
        {
            if (xRoot != null && xRoot.Name == RootNodeName)
            {
                errmsg = null;
                StringBuilder msg = new StringBuilder();
                XmlNode xNode = null;

                #region 交易序號 (seqno)
                {
                    xNode = xRoot.SelectSingleNode("seqno");
                    if (xNode != null)
                    {
                        this.SeqNo = xNode.InnerText;
                    }
                    else
                    {
                        errmsg = "缺少交易序號 (seqno) 節點";
                    }
                }
                #endregion

                #region 銷帳編號 (rid)
                {
                    xNode = xRoot.SelectSingleNode("rid");
                    if (xNode != null)
                    {
                        this.Rid = xNode.InnerText;
                    }
                    else
                    {
                        msg.AppendLine("缺少銷帳編號 (rid) 節點");
                    }
                }
                #endregion

                #region 繳款金額 (amount)
                {
                    xNode = xRoot.SelectSingleNode("amount");
                    if (xNode != null)
                    {
                        this.Amount = xNode.InnerText;
                    }
                    else
                    {
                        msg.AppendLine("缺少繳款金額 (amount) 節點");
                    }
                }
                #endregion

                #region 代收日期 (paydate)
                {
                    xNode = xRoot.SelectSingleNode("paydate");
                    if (xNode != null)
                    {
                        this.PayDate = xNode.InnerText;
                    }
                    else
                    {
                        msg.AppendLine("缺少代收日期 (paydate) 節點");
                    }
                }
                #endregion

                #region 代收時間 (paytime)
                {
                    xNode = xRoot.SelectSingleNode("paytime");
                    if (xNode != null)
                    {
                        this.PayTime = xNode.InnerText;
                    }
                    else
                    {
                        msg.AppendLine("缺少代收時間 (paytime) 節點");
                    }
                }
                #endregion

                #region 交易授權碼 (authcode)
                {
                    xNode = xRoot.SelectSingleNode("authcode");
                    if (xNode != null)
                    {
                        this.AuthCode = xNode.InnerText;
                    }
                    else
                    {
                        msg.AppendLine("缺少交易授權碼 (authcode) 節點");
                    }
                }
                #endregion

                #region 訊息認證押碼 (MAC)
                {
                    xNode = xRoot.SelectSingleNode("MAC");
                    if (xNode != null)
                    {
                        this.MAC = xNode.InnerText;
                    }
                    else
                    {
                        msg.AppendLine("缺少訊息認證押碼 (MAC) 節點");
                    }
                }
                #endregion

                errmsg = msg.ToString();

                return String.IsNullOrEmpty(errmsg);
            }
            else
            {
                errmsg = String.Format("缺少或不正確的根節點 ({0})", RootNodeName);
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 回傳資料承載基底抽象類別
    /// </summary>
    public abstract class BaseRtnData
    {
        #region Const
        public const string RootNodeName = "Bot";
        #endregion

        #region Property
        private string _TxnNo = null;
        /// <summary>
        /// 交易編號 (txnno)
        /// </summary>
        public virtual string TxnNo
        {
            get
            {
                return _TxnNo;
            }
            set
            {
                _TxnNo = value == null ? null : value.Trim();
            }
        }

        private string _ErrCode = null;
        /// <summary>
        /// 訊息代碼 (errcode)
        /// </summary>
        public virtual string ErrCode
        {
            get
            {
                return _ErrCode;
            }
            set
            {
                _ErrCode = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 BaseRtnData 類別
        /// </summary>
        protected BaseRtnData()
        {

        }

        /// <summary>
        /// 建構 BaseRtnData 類別
        /// </summary>
        /// <param name="txnNo"></param>
        /// <param name="errCode"></param>
        protected BaseRtnData(string txnNo, string errCode)
        {
            this.TxnNo = txnNo;
            this.ErrCode = errCode;
        }
        #endregion

        #region Method
        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initial()
        {
            this.TxnNo = null;
            this.ErrCode = null;
        }

        /// <summary>
        /// 將資料轉成 xml 字串
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            string xml = null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.CheckCharacters = false;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;
                    settings.OmitXmlDeclaration = true;

                    using (XmlWriter xw = XmlWriter.Create(ms, settings))
                    {
                        #region 根節點開始
                        xw.WriteStartElement(RootNodeName);
                        #endregion

                        #region 交易編號 (txnno)
                        xw.WriteElementString("txnno", this.TxnNo ?? String.Empty);
                        #endregion

                        #region 訊息代碼 (errcode)
                        xw.WriteElementString("errcode", this.ErrCode ?? String.Empty);
                        #endregion

                        #region 繼承者的其他資料
                        this.WriteMyXmlNode(xw);
                        #endregion

                        #region 根節點結束
                        xw.WriteEndElement();
                        #endregion

                        xw.Flush();
                    }

                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        xml = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                xml = null;
            }
            return xml;
        }

        /// <summary>
        /// 將所屬資料寫入 XmlWriter 節點
        /// </summary>
        /// <param name="xw"></param>
        /// <returns></returns>
        protected abstract bool WriteMyXmlNode(XmlWriter xw);
        #endregion
    }

    /// <summary>
    /// 銷帳編號查詢繳費單資訊的回傳資料承載類別
    /// </summary>
    public sealed class Q0002RtnData : BaseRtnData
    {
        #region Const
        public const string MyTxnNo = "Q0002";
        #endregion

        #region Property
        private string _StudentName = null;
        /// <summary>
        /// 學生姓名 (studentname)
        /// </summary>
        public string StudentName
        {
            get
            {
                return _StudentName;
            }
            set
            {
                _StudentName = value == null ? null : value.Trim();
            }
        }

        private string _RidNo = null;
        /// <summary>
        /// 虛擬帳號 (ridno)
        /// </summary>
        public string RidNo
        {
            get
            {
                return _RidNo;
            }
            set
            {
                _RidNo = value;
            }
        }

        private Decimal? _Amount = null;
        /// <summary>
        /// 應繳金額 (amount)
        /// </summary>
        public Decimal? Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }

        private DateTime? _PayDueDate = null;
        /// <summary>
        /// 繳款期限 (payduedate)
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

        #region Constructor
        /// <summary>
        /// 建構 Q0002RtnData 類別
        /// </summary>
        public Q0002RtnData()
            : base()
        {
            this.TxnNo = MyTxnNo;
        }

        /// <summary>
        /// 建構 Q0002RtnData 類別
        /// </summary>
        /// <param name="studentName"></param>
        /// <param name="amount"></param>
        /// <param name="payDueDate"></param>
        public Q0002RtnData(string studentName, string ridNo, Decimal? amount, DateTime? payDueDate)
            : base(MyTxnNo, ErrorList.NORMAL)
        {
            this.StudentName = studentName;
            this.RidNo = ridNo;
            this.Amount = amount;
            this.PayDueDate = payDueDate;
        }

        /// <summary>
        /// 建構 Q0002RtnData 類別
        /// </summary>
        /// <param name="errCode"></param>
        public Q0002RtnData(string errCode)
            : base(MyTxnNo, errCode)
        {
        }
        #endregion

        #region Method
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initial()
        {
            this.TxnNo = null;
            this.ErrCode = null;
            this.StudentName = null;
            this.RidNo = null;
            this.Amount = null;
            this.PayDueDate = null;
        }

        /// <summary>
        /// 將所屬資料寫入 XmlWriter 節點
        /// </summary>
        /// <param name="xw"></param>
        /// <returns></returns>
        protected override bool WriteMyXmlNode(XmlWriter xw)
        {
            if (xw != null)
            {
                #region 學生姓名 (studentname)
                xw.WriteElementString("studentname", this.StudentName ?? String.Empty);
                #endregion

                #region 虛擬帳號 (ridno)
                xw.WriteElementString("ridno", this.RidNo ?? String.Empty);
                #endregion

                #region 應繳金額 (amount)
                xw.WriteElementString("amount", this.Amount == null ? String.Empty : this.Amount.Value.ToString("0"));
                #endregion

                #region 繳款期限 (payduedate)
                //日期格式: yyyymmdd
                xw.WriteElementString("payduedate", this.PayDueDate == null ? String.Empty : this.PayDueDate.Value.ToString("yyyyMMdd"));
                #endregion

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 信用卡繳費成功通知的回傳資料承載類別
    /// </summary>
    public sealed class T0001RtnData : BaseRtnData
    {
        #region Const
        public const string MyTxnNo = "T0001";
        #endregion

        #region Property
        private string _MAC = null;
        /// <summary>
        /// 訊息認證押碼 (MAC)
        /// </summary>
        public string MAC
        {
            get
            {
                return _MAC;
            }
            private set
            {
                _MAC = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Constructor
        public T0001RtnData()
            : base()
        {
            this.TxnNo = MyTxnNo;
        }

        public T0001RtnData(string errCode, string mac)
            : base(MyTxnNo, errCode)
        {
            this.MAC = mac;
        }
        #endregion

        #region Method
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initial()
        {
            this.TxnNo = null;
            this.ErrCode = null;
            this.MAC = null;
        }

        /// <summary>
        /// 將所屬資料寫入 XmlWriter 節點
        /// </summary>
        /// <param name="xw"></param>
        /// <returns></returns>
        protected override bool WriteMyXmlNode(XmlWriter xw)
        {
            if (xw != null)
            {
                #region 訊息認證押碼 (MAC)
                xw.WriteElementString("MAC", this.MAC ?? String.Empty);
                #endregion

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 即查繳服務處理工具類別
    /// </summary>
    public class BankServiceHelper : IDisposable
    {
        #region Member
        /// <summary>
        /// 儲存 資料存取物件 的變數
        /// </summary>
        private EntityFactory _Factory = null;
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 BankServiceHelper 類別
        /// </summary>
        public BankServiceHelper()
        {
            _Factory = new EntityFactory();
        }
        #endregion

        #region Destructor
        /// <summary>
        /// 解構 BankServiceHelper 類別
        /// </summary>
        ~BankServiceHelper()
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
        /// <summary>
        /// 取得此物件是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public bool IsReady()
        {
            return (_Factory != null && _Factory.IsReady());
        }

        private void AppendLog(BankServiceLogEntity log)
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
        /// <param name="orgId"></param>
        /// <param name="orgPXX"></param>
        /// <param name="clientIP"></param>
        /// <param name="methodName"></param>
        /// <param name="methodArguments"></param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回空字串</returns>
        private string ParseCommand(IServiceCommand command
            , out string orgId, out string orgPXX, out string clientIP, out string methodName, out ICollection<KeyValue<string>> methodArguments)
        {
            orgId = null;
            orgPXX = null;
            clientIP = null;
            methodName = null;
            methodArguments = null;

            #region 檢查參數
            BankServiceCommand myCommand = command is BankServiceCommand ? (BankServiceCommand)command : BankServiceCommand.Create(command);
            if (myCommand == null)
            {
                return "缺少或無效的資料處理服務命令參數"; ;
            }
            #endregion

            #region 處理服務命令參數
            StringBuilder errmsg = new StringBuilder();

            #region ORG_ID
            #region [MDY:20220530] Checkmarx 調整
            if (!myCommand.GetOrgId(out orgId)
                || String.IsNullOrWhiteSpace(orgId)     //不允許無此參數或空值
                || Encoding.Default.GetByteCount(orgId) > 32)
            {
                errmsg.AppendFormat("缺少或無效的 {0} 命令參數 ({1})", BankServiceCommand.ORG_ID, orgId);
            }
            else
            {
                orgId = orgId.Trim();
            }
            #endregion
            #endregion

            #region [MDY:20220530] Checkmarx 調整
            #region ORG_PXX
            if (!myCommand.GetOrgPXX(out orgPXX)
                || String.IsNullOrWhiteSpace(orgPXX)        //不允許無此參數或空值
                || Encoding.Default.GetByteCount(orgPXX) > 32)
            {
                errmsg.AppendFormat("缺少或無效的 {0} 命令參數 ({1})", BankServiceCommand.ORG_PXX, orgPXX);
            }
            else
            {
                orgPXX = orgPXX.Trim();
            }
            #endregion
            #endregion

            #region CLIENT_IP
            if (!myCommand.GetClientIP(out clientIP)
                || String.IsNullOrWhiteSpace(clientIP)      //不允許無此參數或空值
                || Encoding.Default.GetByteCount(clientIP) > 40)
            {
                errmsg.AppendFormat("缺少或無效的 {0} 命令參數 ({1})", BankServiceCommand.CLIENT_IP, clientIP);
            }
            else
            {
                clientIP = clientIP.Trim();
            }
            #endregion

            #region METHOD_NAME
            if (!myCommand.GetMethodName(out methodName)
                || !BankServiceMethodName.IsDefine(methodName))
            {
                errmsg.AppendFormat("缺少或無效的 {0} 命令參數 ({1})", BankServiceCommand.METHOD_NAME, methodName);
            }
            #endregion

            #region METHOD_ARGUMENTS
            if (!myCommand.GetMethodArguments(out methodArguments))    //允許無此參數
            {
                errmsg.AppendFormat("無效的 {0} 命令參數", BankServiceCommand.METHOD_NAME);
            }
            #endregion

            return errmsg.ToString();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="returnMsg"></param>
        /// <param name="data"></param>
        public Result ExecuteCommand(IServiceCommand command, out string rsXml)
        {
            rsXml = null;

            #region 檢查並處理服務命令參數
            string orgId = null;
            string orgPXX = null;
            string clientIP = null;
            string methodName = null;
            ICollection<KeyValue<string>> methodArguments = null;
            string errmsg = this.ParseCommand(command, out orgId, out orgPXX, out clientIP, out methodName, out methodArguments);

            BankServiceLogEntity log = new BankServiceLogEntity();
            log.CrtDate = DateTime.Now;
            log.OrgId = orgId ?? String.Empty;
            log.OrgPXX = orgPXX ?? String.Empty;
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
                log.ErrorMsg = errmsg;
                log.ReturnMsg = rsXml;
                this.AppendLog(log);
                return new Result(false, errmsg, ErrorList.S999, null);
            }
            else
            {
                log.ErrorMsg = String.Empty;
                log.ReturnMsg = String.Empty;
            }
            #endregion

            #region 檢查資料存取物件
            if (!this.IsReady())
            {
                errmsg = "缺少或無效的資料存取物件";
                log.ErrorMsg = errmsg;
                log.ReturnMsg = rsXml;
                this.AppendLog(log);
                return new Result(false, errmsg, ErrorList.S990, null);
            }
            #endregion

            #region 驗證並取得即查繳服務帳號資料
            BankServiceAccountEntity account = null;
            {
                string errCode = null;
                errmsg = this.CheckBankServiceAccount(orgId, orgPXX, clientIP, out errCode, out account);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    log.ErrorMsg = errmsg;
                    log.ReturnMsg = String.Format("ErrorCode={0}", errCode);
                    this.AppendLog(log);
                    return new Result(false, errmsg, errCode, null);
                }
            }
            #endregion

            #region 呼叫方法
            {
                Result result = new Result(true);

                switch (methodName)
                {
                    case BankServiceMethodName.Q0002:
                        #region 以銷帳編號查詢繳費單資訊
                        {
                            #region 取得方法參數
                            string requestXml = null;
                            foreach (KeyValue<string> argument in methodArguments)
                            {
                                string argName = argument.Key.ToUpper();
                                switch (argName)
                                {
                                    case "REQUEST_XML":
                                        requestXml = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                }
                            }
                            #endregion

                            Q0002TxnData txnData = null;
                            Q0002RtnData rtnData = this.RunQ0002(account, requestXml, out txnData, out errmsg);
                            rsXml = rtnData.ToXml();
                            log.ErrorMsg = errmsg;
                            log.ReturnMsg = rsXml;
                            log.CallRid = txnData.Rid;
                            log.CallSeqNo = null;
                        }
                        #endregion
                        break;
                    case BankServiceMethodName.T0001:
                        #region 信用卡繳費成功通知
                        {
                            #region 取得方法參數
                            string requestXml = null;
                            foreach (KeyValue<string> argument in methodArguments)
                            {
                                string argName = argument.Key.ToUpper();
                                switch (argName)
                                {
                                    case "REQUEST_XML":
                                        requestXml = argument.Value == null ? null : argument.Value.Trim();
                                        break;
                                }
                            }
                            #endregion

                            T0001TxnData txnData = null;
                            T0001RtnData rtnData = this.RunT0001(account, requestXml, out txnData, out errmsg);
                            rsXml = rtnData.ToXml();
                            log.ErrorMsg = errmsg;
                            log.ReturnMsg = rsXml;
                            log.CallRid = txnData.Rid;
                            log.CallSeqNo = txnData.SeqNo;
                        }
                        #endregion
                        break;
                }

                this.AppendLog(log);
                return new Result(true);
            }
            #endregion
        }
        #endregion
        #endregion

        /// <summary>
        /// 取得代收種類
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="receiveKind"></param>
        /// <returns></returns>
        private string GetReceiveKind(string receiveType, out string receiveKind)
        {
            receiveKind = null;
            string sql = String.Format("SELECT [{0}] FROM [{1}] WHERE [{2}] = @ReceiveType"
                    , SchoolRTypeEntity.Field.ReceiveKind
                    , SchoolRTypeEntity.TABLE_NAME
                    , SchoolRTypeEntity.Field.ReceiveType);
            KeyValue[] parameters = new KeyValue[] {
                new KeyValue("@ReceiveType", receiveType)
            };
            DataTable dt = null;
            Result result = _Factory.GetDataTable(sql, parameters, 0, 1, out dt);
            if (result.IsSuccess)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    receiveKind = row.IsNull(0) ? null : row[0].ToString();
                }
                return null;
            }
            else
            {
                return result.Message;
            }
        }

        /// <summary>
        /// 取得指定商家代號是否有中信管道
        /// </summary>
        /// <param name="receiveType">指定商家代號</param>
        /// <param name="hasCTCB">有中信管道則傳回 true，否則傳回 false</param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        private string HasCTCBReceiveWay(string receiveType, out bool hasCTCB)
        {
            hasCTCB = false;
            int count = 0;
            Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType)
                .And(ReceiveChannelEntity.Field.ChannelId, ChannelHelper.CTCB);
            Result result = _Factory.SelectCount<ReceiveChannelEntity>(where, out count);
            if (result.IsSuccess)
            {
                hasCTCB = (count > 0);
                return null;
            }
            else
            {
                return result.Message;
            }
        }

        private Q0002RtnData RunQ0002(BankServiceAccountEntity account, string rqXml, out Q0002TxnData txnData, out string errmsg)
        {
            errmsg = null;
            string errCode = null;

            txnData = new Q0002TxnData();
            errCode = txnData.LoadXml(rqXml, out errmsg);
            if (errCode != ErrorList.NORMAL)
            {
                Q0002RtnData rtnData = new Q0002RtnData(errCode);
                return rtnData;
            }

            errCode = txnData.IsReady();
            if (errCode != ErrorList.NORMAL)
            {
                errmsg = "查詢參數不正確";
                Q0002RtnData rtnData = new Q0002RtnData(errCode);
                return rtnData;
            }

            if (account == null)
            {
                errmsg = "缺少機關帳號資料";
                errCode = ErrorList.S990;
                Q0002RtnData rtnData = new Q0002RtnData(errCode);
                return rtnData;
            }

            string qCancelNo = txnData.Rid;
            string qReceiveType = qCancelNo.Substring(0, 4);

            #region 先判斷是哪一種代收種類
            string receiveKind = null;
            {
                errmsg = this.GetReceiveKind(qReceiveType, out receiveKind);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    errCode = ErrorList.S990;
                    Q0002RtnData rtnData = new Q0002RtnData(errCode);
                    return rtnData;
                }
                if (!ReceiveKindCodeTexts.IsDefine(receiveKind))
                {
                    errmsg = "查無該虛擬帳號資料";
                    errCode = ErrorList.D1003;
                    Q0002RtnData rtnData = new Q0002RtnData(errCode);
                    return rtnData;
                }
            }
            #endregion

            #region [MDY:20180520] 增加交易的商家代號是否有中信管道的檢查
            if (receiveKind == ReceiveKindCodeTexts.SCHOOL)
            {
                bool hasCTCB = false;
                errmsg = this.HasCTCBReceiveWay(qReceiveType, out hasCTCB);
                if (String.IsNullOrEmpty(errmsg))
                {
                    if (!hasCTCB)
                    {
                        errmsg = "該商家代號未設定中信該收管道";
                        errCode = ErrorList.D2007;
                        Q0002RtnData rtnData = new Q0002RtnData(errCode);
                        return rtnData;
                    }
                }
                else
                {
                    errCode = ErrorList.S990;
                    Q0002RtnData rtnData = new Q0002RtnData(errCode);
                    return rtnData;
                }
            }
            #endregion

            #region 取資料
            {
                if (receiveKind == ReceiveKindCodeTexts.SCHOOL)
                {
                    #region 學雜費
                    int count = 0;
                    Expression where = new Expression(StudentReceiveView3.Field.ReceiveType, qReceiveType)
                        .And(StudentReceiveView3.Field.CancelNo, qCancelNo);
                    Result result = _Factory.SelectCount<StudentReceiveView3>(where, out count);
                    if (result.IsSuccess)
                    {
                        if (count == 0)
                        {
                            errmsg = "查無該虛擬帳號資料";
                            errCode = ErrorList.D1003;
                            Q0002RtnData rtnData = new Q0002RtnData(errCode);
                            return rtnData;
                        }

                        where
                            .And(new Expression(StudentReceiveView3.Field.ReceiveWay, null).Or(StudentReceiveView3.Field.ReceiveWay, String.Empty))
                            .And(new Expression(StudentReceiveView3.Field.ReceiveDate, null).Or(StudentReceiveView3.Field.ReceiveDate, String.Empty))
                            .And(StudentReceiveView3.Field.PayDueDate2, RelationEnum.NotEqual, String.Empty)
                            .And(StudentReceiveView3.Field.PayDueDate2, RelationEnum.GreaterEqual, Common.GetTWDate7());
                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(2);
                        orderbys.Add(StudentReceiveView3.Field.YearId, OrderByEnum.Desc);
                        orderbys.Add(StudentReceiveView3.Field.TermId, OrderByEnum.Desc);

                        #region
                        //                    string sql = @"
                        //SELECT SR.[Receive_Type], SR.[Year_Id], SR.[Term_Id], SR.[Dep_Id], SR.[Receive_Id], SR.[Stu_Id], SR.[Old_Seq]
                        //     , ISNULL((SELECT TOP 1 [Stu_Name] FROM Student_Master AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
                        //     , SR.[Cancel_No] AS [Cancel_No], SR.[Receive_Amount]
                        //     , ISNULL(RI.[Bill_Valid_Date], '') AS [Bill_Valid_Date], ISNULL(RI.[Bill_Close_Date], '') AS [Bill_Close_Date]
                        //     , ISNULL(RI.[Pay_Date], '') AS [Pay_Due_Date], ISNULL(RI.[Pay_Due_Date2], '') AS [Pay_Due_Date2]
                        //  FROM Student_Receive AS SR
                        //  JOIN School_Rid AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]
                        // WHERE SR.[Receive_Amount] > 0
                        //   AND SR.[Cancel_No] = '51003110078104'
                        //   AND (SR.[Receive_Way] IS NULL OR SR.[Receive_Way] = '')
                        //   AND (SR.[Receive_Date] IS NULL OR SR.[Receive_Date] = '')
                        //   AND (SR.[Account_Date] IS NULL OR SR.[Account_Date] = '')
                        //   AND (RI.[Bill_Valid_Date] IS NULL OR RI.[Bill_Valid_Date] = '' OR (ISNUMERIC(RI.[Bill_Valid_Date]) = 1 AND CAST(RI.[Bill_Valid_Date] AS int) + 19110000 > 19110101))
                        // ORDER BY SR.[Receive_Type], SR.[Year_Id] desc, SR.[Term_Id] desc, SR.[Old_Seq] desc, SR.[create_date] desc";
                        #endregion

                        StudentReceiveView3 data = null;
                        result = _Factory.SelectFirst<StudentReceiveView3>(where, orderbys, out data);
                        if (result.IsSuccess)
                        {
                            if (data == null)
                            {
                                errmsg = "查無應繳資料";
                                errCode = ErrorList.D1002;
                                Q0002RtnData rtnData = new Q0002RtnData(errCode);
                                return rtnData;
                            }
                            else
                            {
                                Q0002RtnData rtnData = new Q0002RtnData(data.StuName, data.CancelNo, data.ReceiveAmount, DataFormat.ConvertDateText(data.PayDueDate2));
                                return rtnData;
                            }
                        }
                    }

                    {
                        errmsg = result.Message;
                        errCode = ErrorList.S990;
                        Q0002RtnData rtnData = new Q0002RtnData(errCode);
                        return rtnData;
                    }
                    #endregion
                }
                else
                {
                    #region 代收各項費用
                    int count = 0;
                    Expression where = new Expression(QueueCTCBEntity.Field.ReceiveType, qReceiveType)
                        .And(QueueCTCBEntity.Field.CancelNo, qCancelNo)
                        .And(QueueCTCBEntity.Field.SendDate, RelationEnum.LessEqual, DateTime.Today.AddDays(1));
                    Result result = _Factory.SelectCount<QueueCTCBEntity>(where, out count);
                    if (result.IsSuccess)
                    {
                        if (count == 0)
                        {
                            errmsg = "查無該虛擬帳號資料";
                            errCode = ErrorList.D1003;
                            Q0002RtnData rtnData = new Q0002RtnData(errCode);
                            return rtnData;
                        }

                        where
                            .And(new Expression(QueueCTCBEntity.Field.CancelFlag, RelationEnum.NotEqual, 'Y').Or(QueueCTCBEntity.Field.CancelFlag, null))
                            .And(QueueCTCBEntity.Field.PayDueDate, RelationEnum.NotEqual, String.Empty)
                            .And(QueueCTCBEntity.Field.PayDueDate, RelationEnum.GreaterEqual, Common.GetTWDate7());
                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                        orderbys.Add(QueueCTCBEntity.Field.CrtDate, OrderByEnum.Desc);

                        QueueCTCBEntity data = null;
                        result = _Factory.SelectFirst<QueueCTCBEntity>(where, orderbys, out data);
                        if (result.IsSuccess)
                        {
                            if (data == null)
                            {
                                errmsg = "查無應繳資料";
                                errCode = ErrorList.D1002;
                                Q0002RtnData rtnData = new Q0002RtnData(errCode);
                                return rtnData;
                            }
                            else
                            {
                                Q0002RtnData rtnData = new Q0002RtnData(data.StuName, data.CancelNo, data.ReceiveAmount, DataFormat.ConvertDateText(data.PayDueDate));
                                return rtnData;
                            }
                        }
                    }

                    {
                        errmsg = result.Message;
                        errCode = ErrorList.S990;
                        Q0002RtnData rtnData = new Q0002RtnData(errCode);
                        return rtnData;
                    }
                    #endregion
                }
            }
            #endregion
        }

        private T0001RtnData RunT0001(BankServiceAccountEntity account, string rqXml, out T0001TxnData txnData, out string errmsg)
        {
            errmsg = null;
            string errCode = null;

            string mac = null;
            txnData = new T0001TxnData();
            errCode = txnData.LoadXml(rqXml, out errmsg);
            if (errCode != ErrorList.NORMAL)
            {
                if (txnData != null)
                {
                    mac = txnData.MAC;
                }
                T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                return rtnData;
            }

            errCode = txnData.IsReady();
            if (errCode != ErrorList.NORMAL)
            {
                errmsg = "查詢參數不正確";
                T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                return rtnData;
            }

            if (account == null)
            {
                errmsg = "缺少機關帳號資料";
                errCode = ErrorList.S990;
                T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                return rtnData;
            }

            string seqNo = txnData.SeqNo;
            string cancelNo = txnData.Rid;
            decimal amount = txnData.GetAmount().Value;
            string receiveDate = Common.GetTWDate7(txnData.GetPayDate().Value);
            string receiveTime = txnData.GetPayTime().Value.ToString("hhmmss");
            string receiveType = cancelNo.Substring(0, 4);

            #region 先判斷是哪一種代收種類
            string receiveKind = null;
            {
                errmsg = this.GetReceiveKind(receiveType, out receiveKind);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    errCode = ErrorList.S990;
                    T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                    return rtnData;
                }
                if (!ReceiveKindCodeTexts.IsDefine(receiveKind))
                {
                    errmsg = "查無該虛擬帳號資料";
                    errCode = ErrorList.D3009;
                    T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                    return rtnData;
                }
            }
            #endregion

            #region [MDY:20180520] 增加交易的商家代號是否有中信管道的檢查
            if (receiveKind == ReceiveKindCodeTexts.SCHOOL)
            {
                bool hasCTCB = false;
                errmsg = this.HasCTCBReceiveWay(receiveType, out hasCTCB);
                if (String.IsNullOrEmpty(errmsg))
                {
                    if (!hasCTCB)
                    {
                        errmsg = "該商家代號未設定中信該收管道";
                        errCode = ErrorList.D2007;
                        T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                        return rtnData;
                    }
                }
                else
                {
                    errCode = ErrorList.S990;
                    T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                    return rtnData;
                }
            }
            #endregion

            #region 處理資料
            {
                if (receiveKind == ReceiveKindCodeTexts.SCHOOL)
                {
                    #region 學雜費
                    StudentReceiveView3[] datas = null;
                    Expression where = new Expression(StudentReceiveView3.Field.ReceiveType, receiveType)
                        .And(StudentReceiveView3.Field.CancelNo, cancelNo);
                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(2);
                    orderbys.Add(StudentReceiveView3.Field.YearId, OrderByEnum.Desc);
                    orderbys.Add(StudentReceiveView3.Field.TermId, OrderByEnum.Desc);
                    Result result = _Factory.SelectAll<StudentReceiveView3>(where, orderbys, out datas);
                    if (result.IsSuccess)
                    {
                        StudentReceiveView3 myData = null;
                        bool fgCanceled = false;    //已繳款旗標
                        bool fgAmount = false;      //金額不符旗標
                        bool fgPayDueDate = false;  //已過繳費期限旗標
                        if (datas != null && datas.Length > 0)
                        {
                            foreach (StudentReceiveView3 data in datas)
                            {
                                if (String.IsNullOrWhiteSpace(data.ReceiveWay) && String.IsNullOrWhiteSpace(data.ReceiveDate))
                                {
                                    if (data.ReceiveAmount == amount)
                                    {
                                        DateTime? payDueDate2 = data.GetPayDueDate2();
                                        if (payDueDate2 != null && payDueDate2.Value >= DateTime.Today)
                                        {
                                            myData = data;
                                        }
                                        else
                                        {
                                            fgPayDueDate = true;
                                        }
                                    }
                                    else
                                    {
                                        fgAmount = true;
                                    }
                                }
                                else
                                {
                                    fgCanceled = true;
                                }
                            }
                        }

                        if (myData == null)
                        {
                            if (fgPayDueDate)
                            {
                                errmsg = "已繳款";
                                errCode = ErrorList.D3007;
                            }
                            else if (fgAmount)
                            {
                                errmsg = "金額不符";
                                errCode = ErrorList.D3008;
                            }
                            else if (fgCanceled)
                            {
                                errmsg = "已過繳費期限";
                                errCode = ErrorList.D3006;
                            }
                            else
                            {
                                errmsg = "查無該虛擬帳號資料";
                                errCode = ErrorList.D3009;
                            }
                            T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                            return rtnData;
                        }
                        else
                        {
                            Expression where2 = new Expression(StudentReceiveEntity.Field.ReceiveType, myData.ReceiveType)
                                .And(StudentReceiveEntity.Field.YearId, myData.YearId)
                                .And(StudentReceiveEntity.Field.TermId, myData.TermId)
                                .And(StudentReceiveEntity.Field.DepId, myData.DepId)
                                .And(StudentReceiveEntity.Field.ReceiveId, myData.ReceiveId)
                                .And(StudentReceiveEntity.Field.StuId, myData.StuId)
                                .And(StudentReceiveEntity.Field.OldSeq, myData.OldSeq)
                                .And(StudentReceiveEntity.Field.CancelNo, cancelNo)
                                .And(StudentReceiveEntity.Field.ReceiveAmount, amount)
                                .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty))
                                .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));

                            KeyValueList fieldValues = new KeyValueList();
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveWay, account.ReceiveWay);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveDate, receiveDate);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveTime, receiveTime);

                            int count = 0;
                            result = _Factory.UpdateFields<StudentReceiveEntity>(fieldValues, where2, out count);
                            if (result.IsSuccess)
                            {
                                if (count == 0)
                                {
                                    result = new Result(false, "無資料被更新", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                                }
                                else
                                {
                                    T0001RtnData rtnData = new T0001RtnData(ErrorList.NORMAL, mac);
                                    return rtnData;
                                }
                            }
                        }
                    }

                    {
                        errmsg = result.Message;
                        errCode = ErrorList.S990;
                        T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                        return rtnData;
                    }
                    #endregion
                }
                else
                {
                    #region 代收各項費用
                    DateTime today = DateTime.Today;
                    QueueCTCBEntity[] datas = null;
                    Expression where = new Expression(QueueCTCBEntity.Field.ReceiveType, receiveType)
                        .And(QueueCTCBEntity.Field.CancelNo, cancelNo)
                        .And(QueueCTCBEntity.Field.SendDate, RelationEnum.LessEqual, today.AddDays(1));
                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(2);
                    orderbys.Add(QueueCTCBEntity.Field.CrtDate, OrderByEnum.Desc);
                    Result result = _Factory.SelectAll<QueueCTCBEntity>(where, orderbys, out datas);
                    if (result.IsSuccess)
                    {
                        QueueCTCBEntity myData = null;
                        bool fgCanceled = false;    //已繳款旗標
                        bool fgAmount = false;      //金額不符旗標
                        bool fgPayDueDate = false;  //已過繳費期限旗標
                        if (datas != null && datas.Length > 0)
                        {
                            foreach (QueueCTCBEntity data in datas)
                            {
                                if (String.IsNullOrWhiteSpace(data.CancelFlag) || data.CancelFlag != "Y")
                                {
                                    if (data.ReceiveAmount == amount)
                                    {
                                        DateTime? payDueDate = DataFormat.ConvertDateText(data.PayDueDate);
                                        if (payDueDate != null && payDueDate.Value >= today)
                                        {
                                            myData = data;
                                        }
                                        else
                                        {
                                            fgPayDueDate = true;
                                        }
                                    }
                                    else
                                    {
                                        fgAmount = true;
                                    }
                                }
                                else
                                {
                                    fgCanceled = true;
                                }
                            }
                        }

                        if (myData == null)
                        {
                            if (fgPayDueDate)
                            {
                                errmsg = "已繳款";
                                errCode = ErrorList.D3007;
                            }
                            else if (fgAmount)
                            {
                                errmsg = "金額不符";
                                errCode = ErrorList.D3008;
                            }
                            else if (fgCanceled)
                            {
                                errmsg = "已過繳費期限";
                                errCode = ErrorList.D3006;
                            }
                            else
                            {
                                errmsg = "查無該虛擬帳號資料";
                                errCode = ErrorList.D3009;
                            }
                            T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                            return rtnData;
                        }
                        else
                        {
                            Expression where2 = new Expression(QueueCTCBEntity.Field.SN, myData.SN)
                                .And(QueueCTCBEntity.Field.ReceiveType, myData.ReceiveType)
                                .And(QueueCTCBEntity.Field.StuId, myData.StuId)
                                .And(QueueCTCBEntity.Field.CancelNo, cancelNo)
                                .And(QueueCTCBEntity.Field.ReceiveAmount, amount)
                                .And(new Expression(QueueCTCBEntity.Field.CancelFlag, RelationEnum.NotEqual, 'Y').Or(QueueCTCBEntity.Field.CancelFlag, null));

                            KeyValueList fieldValues = new KeyValueList();
                            fieldValues.Add(QueueCTCBEntity.Field.CancelFlag, "Y");

                            int count = 0;
                            result = _Factory.UpdateFields<QueueCTCBEntity>(fieldValues, where2, out count);
                            if (result.IsSuccess)
                            {
                                if (count == 0)
                                {
                                    result = new Result(false, "無資料被更新", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                                }
                                else
                                {
                                    T0001RtnData rtnData = new T0001RtnData(ErrorList.NORMAL, mac);
                                    return rtnData;
                                }
                            }
                        }
                    }

                    {
                        errmsg = result.Message;
                        errCode = ErrorList.S990;
                        T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                        return rtnData;
                    }
                    #endregion
                }
            }
            #endregion
        }

        #region [MDY:20220530] Checkmarx 調整
        #region [MDY:20210401] 原碼修正
        /// <summary>
        /// 驗證並取得即查繳服務帳號資料
        /// </summary>
        /// <param name="orgId">機關代碼</param>
        /// <param name="orgPXX">使用密碼</param>
        /// <param name="clientIP">呼叫端 IP</param>
        /// <param name="errCode">傳回錯誤代碼</param>
        /// <param name="account">傳回帳號資料</param>
        /// <returns>失敗則傳回錯誤訊息，否則傳回 null</returns>
        public string CheckBankServiceAccount(string orgId, string orgPXX, string clientIP, out string errCode, out BankServiceAccountEntity account)
        {
            errCode = null;
            account = null;

            if (String.IsNullOrWhiteSpace(orgId) || String.IsNullOrWhiteSpace(orgPXX))
            {
                errCode = ErrorList.S990;
                return "未指定機關代碼或使用密碼";
            }
            if (String.IsNullOrWhiteSpace(clientIP))
            {
                errCode = ErrorList.C001;
                return "未指定呼叫端 IP";
            }
            if (!this.IsReady())
            {
                errCode = ErrorList.S990;
                return "資料存取物件未準備好";
            }

            Expression where = new Expression(BankServiceAccountEntity.Field.OrgId, orgId)
                .And(BankServiceAccountEntity.Field.Status, DataStatusCodeTexts.NORMAL);
            Result result = _Factory.SelectFirst<BankServiceAccountEntity>(where, null, out account);
            if (!result.IsSuccess)
            {
                errCode = ErrorList.S990;
                return String.Format("查詢機關代碼 {0} 資料失敗，錯誤訊息：({1}) {2}", orgId, result.Code, result.Message);
            }

            if (account == null)
            {
                errCode = ErrorList.C002;
                return "機關代碼不正確或該機管停用";
            }

            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            if (account.OrgPXX != orgPXX)
            {
                errCode = ErrorList.C003;
                return "使用密碼不正確";
            }
            #endregion
            #endregion

            if (!account.IsMyClientIP(clientIP))
            {
                errCode = ErrorList.C001;
                return "未授權的 IP";
            }

            return null;
        }
        #endregion
        #endregion
        #endregion
    }
}
