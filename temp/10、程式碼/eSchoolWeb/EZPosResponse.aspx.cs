using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.Configuration;
using Fuju.DB;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    public partial class EZPosResponse : LocalizedPage
    {
        #region Log 相關
        private const string _MethodName = "EZPosResponse";
        private string _LogPath = null;

        /// <summary>
        /// 取得 Log 檔完整路徑檔名
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName()
        {
            if (_LogPath == null)
            {
                _LogPath = ConfigurationManager.AppSettings.Get("log_path");
                if (_LogPath == null)
                {
                    _LogPath = String.Empty;
                }
                else
                {
                    _LogPath = _LogPath.Trim();
                }
                if (!String.IsNullOrEmpty(_LogPath))
                {
                    try
                    {
                        if (!Directory.Exists(_LogPath))
                        {
                            Directory.CreateDirectory(_LogPath);
                        }
                    }
                    catch (Exception)
                    {
                        _LogPath = String.Empty;
                    }
                }
            }

            if (String.IsNullOrEmpty(_LogPath))
            {
                return null;
            }
            else
            {
                return Path.Combine(_LogPath, String.Format("{0}_{1:yyyyMMdd}.log", _MethodName, DateTime.Today));
            }
        }

        /// <summary>
        /// 寫 Log
        /// </summary>
        /// <param name="methodName">方法名稱</param>
        /// <param name="msg">訊息</param>
        private void WriteLog(string msg)
        {
            if (String.IsNullOrEmpty(msg))
            {
                return;
            }
            string logFileName = this.GetLogFileName();
            if (String.IsNullOrEmpty(logFileName))
            {
                return;
            }

            StringBuilder log = new StringBuilder();
            log
                .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, _MethodName).AppendLine()
                .AppendLine(msg)
                .AppendLine();

            this.WriteLogFile(logFileName, log.ToString());
        }

        /// <summary>
        /// 寫入 Log 檔
        /// </summary>
        /// <param name="fileName">Log 檔名</param>
        /// <param name="log">Log 內容</param>
        private void WriteLogFile(string fileName, string log)
        {
            try
            {
                File.AppendAllText(fileName, log, Encoding.Default);
            }
            catch (Exception)
            {
                //_logPath = String.Empty;
            }
        }
        #endregion

        #region Override IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "EZPos";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "信用卡繳費-財金";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        //private System.Collections.Specialized.NameValueCollection GetEncodedForm(System.IO.Stream stream, Encoding encoding)
        //{
        //    System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.ASCII);
        //    return GetEncodedForm(reader.ReadToEnd(), encoding);
        //}

        //private System.Collections.Specialized.NameValueCollection GetEncodedForm(string urlEncoded, Encoding encoding)
        //{
        //    System.Collections.Specialized.NameValueCollection form = new System.Collections.Specialized.NameValueCollection();
        //    string[] pairs = urlEncoded.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //    foreach (string pair in pairs)
        //    {
        //        string[] pairItems = pair.Split("=".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries);
        //        string name = HttpUtility.UrlDecode(pairItems[0], encoding);
        //        string value = (pairItems.Length > 1) ? HttpUtility.UrlDecode(pairItems[1], encoding) : null;
        //        form.Add(name, value);
        //    }
        //    return form;
        //}

        //protected override void OnPreInit(EventArgs e)
        //{
        //    Encoding big5 = Encoding.GetEncoding("big5");
        //    this.Request.ContentEncoding = big5;
        //    Session.CodePage = 950;
        //    base.OnPreInit(e);
        //}

        //protected override void OnInit(EventArgs e)
        //{
        //    Encoding big5 = Encoding.GetEncoding("big5");
        //    this.Request.ContentEncoding = big5;
        //    base.OnInit(e);
        //    this.Request.ContentEncoding = big5;
        //    this.Request.ContentType = "application/x-www-form-urlencoded;charset=BIG5";
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Encoding big5 = Encoding.GetEncoding("big5");
                this.Request.ContentEncoding = big5;
                //this.Response.ContentEncoding = big5;
                //this.Page.Response.Charset = "BIG5";
                //this.Page.Request.ContentType = "application/x-www-form-urlencoded;charset=BIG5";

                //System.Collections.Specialized.NameValueCollection request = HttpUtility.ParseQueryString(Request.Url.Query, Encoding.GetEncoding("BIG5"));

                #region [MDY:20210401] 原碼修正
                string x = HttpUtility.UrlDecode(Request.Form["errDesc"], big5);
                #endregion

                //System.Collections.Specialized.NameValueCollection kvs = GetEncodedForm(Request.InputStream, big5);

                #region 回應參數
                #region [MDY:20191214] (2019擴充案) 改用 Form 取參數並增加 authRespTime 參數
                string status = Request.Form["status"];              //授權狀態
                string errcode = Request.Form["errcode"];            //錯誤代碼
                string authCode = Request.Form["authCode"];          //交易授權碼
                string authAmt = Request.Form["authAmt"];            //授權金額
                string lidm = Request.Form["lidm"];                  //銷帳編號
                string xid = Request.Form["xid"];                    //授權交易序號
                string currency = Request.Form["currency"];          //幣值代號
                string amtExp = Request.Form["amtExp"];              //幣值指數
                string merID = Request.Form["merID"];                //特店編號
                string errDesc = Request.Form["errDesc"];            //授權失敗原因
                string lastPan4 = Request.Form["lastPan4"];          //持卡人信用卡末四碼
                string authRespTime = Request.Form["authRespTime"];  //授權處理回應時間 (YYYYMMDDHHMMSS)
                DateTime? dAuthRespTime = null;
                if (Common.IsNumber(authRespTime) && authRespTime.Length == 14)
                {
                    DateTime dateTime;
                    if (DateTime.TryParseExact(authRespTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime))
                    {
                        dAuthRespTime = dateTime;
                    }
                }
                #endregion

                #region [MDY:20210401] 原碼修正
                string txnId = Request.Form["key"];            //信用卡交易編號(行內)
                #endregion
                #endregion

                string myStatus = null;
                string txnResult = null;
                string txnRemark = null;

                StringBuilder log = new StringBuilder();

                #region 紀錄回應參數
                #region [MDY:20191214] (2019擴充案) 增加 authRespTime 參數
                log
                    .AppendFormat("回應參數： txnId={0}; status={1}; errcode={2}; authCode={3}; authAmt={4}; lidm={5}; xid={6}; currency={7}; amtExp={8}; merID={9}; errDesc={10}, lastPan4={11}, lastPan4={12}"
                        , txnId, status, errcode, authCode, authAmt, lidm, xid, currency, amtExp, merID, errDesc, lastPan4, authRespTime).AppendLine();
                #endregion
                #endregion

                status = status == null ? String.Empty : status.Trim();
                string cancelNo = lidm == null ? String.Empty : lidm.Trim();
                txnId = txnId == null ? String.Empty : txnId.Trim();

                StringBuilder html = new System.Text.StringBuilder();
                if (status == "0" && !String.IsNullOrEmpty(authCode) && !String.IsNullOrEmpty(xid))
                {
                    myStatus = "2";
                    txnResult = lastPan4;
                    txnRemark = String.Empty;
                    html
                        .AppendLine("交易成功，相關訊息說明如下：<br/>")
                        .AppendFormat("交易編號：{0}", txnId).AppendLine("<br/>")
                        .AppendFormat("交易授權碼：{0}", authCode).AppendLine("<br/>")
                        .AppendFormat("虛擬帳號：{0}", lidm).AppendLine("<br/>")
                        .AppendFormat("授權金額：{0}", authAmt).AppendLine("<br/>")
                        .AppendFormat("授權交易序號：{0}", xid).AppendLine("<br/>")
                        .AppendFormat("持卡人信用卡末四碼：{0}", lastPan4).AppendLine("<br/>");
                }
                else
                {
                    myStatus = "3";
                    txnResult = String.Concat(status, ":", errcode);
                    txnRemark = errDesc;
                    html
                        .AppendLine("交易失敗，相關訊息說明如下：<br/>")
                        .AppendFormat("交易編號：{0}", txnId).AppendLine("<br/>")
                        .AppendFormat("交易狀態：{0}", status).AppendLine("<br/>")
                        .AppendFormat("錯誤代碼：{0}", errcode).AppendLine("<br/>")
                        .AppendFormat("授權失敗原因：{0}", errDesc).AppendLine("<br/>")
                        .AppendFormat("虛擬帳號：{0}", lidm).AppendLine("<br/>");
                }

                this.labResult.Text = html.ToString();

                #region 更新 EZPos 交易資料
                CCardTxnDtlEntity txn = null;
                {
                    DataProxy proxy = DataProxy.Current;
                    Expression where = new Expression(CCardTxnDtlEntity.Field.TxnId, txnId);
                    XmlResult xmlResult = proxy.SelectFirst<CCardTxnDtlEntity>(this.Page, where, null, out txn);
                    if (xmlResult.IsSuccess)
                    {
                        if (txn != null && txn.Rid == cancelNo && txn.Status == "1")
                        {
                            where
                                .And(CCardTxnDtlEntity.Field.Rid, cancelNo)
                                .And(CCardTxnDtlEntity.Field.Status, "1");

                            #region [MDY:20191214] (2019擴充案) 增加 authRespTime 參數
                            KeyValue[] fieldValues = null;
                            if (dAuthRespTime.HasValue)
                            {
                                fieldValues = new KeyValue[] {
                                    new KeyValue(CCardTxnDtlEntity.Field.Xid, xid),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnResult, txnResult),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnAuthCode, authCode),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnAuthDate, dAuthRespTime.Value),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnRemark, txnRemark),
                                    new KeyValue(CCardTxnDtlEntity.Field.UpdateDate, DateTime.Now),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnStatus, status),
                                    new KeyValue(CCardTxnDtlEntity.Field.Status, myStatus)
                                };
                            }
                            else
                            {
                                fieldValues = new KeyValue[] {
                                    new KeyValue(CCardTxnDtlEntity.Field.Xid, xid),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnResult, txnResult),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnAuthCode, authCode),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnRemark, txnRemark),
                                    new KeyValue(CCardTxnDtlEntity.Field.UpdateDate, DateTime.Now),
                                    new KeyValue(CCardTxnDtlEntity.Field.TxnStatus, status),
                                    new KeyValue(CCardTxnDtlEntity.Field.Status, myStatus)
                                };
                            }
                            #endregion

                            int count = 0;
                            xmlResult = proxy.UpdateFields<CCardTxnDtlEntity>(this.Page, where, fieldValues, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count > 0)
                                {
                                    log.AppendFormat("更新 TxnId={0} 的 EZPos 交易資料 (CCardTxnDtlEntity) 成功", txnId).AppendLine();
                                }
                                else
                                {
                                    log.AppendFormat("更新 TxnId={0} 的 EZPos 交易資料 (CCardTxnDtlEntity) 失敗，無任何資料被更新", txnId).AppendLine();
                                }
                            }
                            else
                            {
                                log.AppendFormat("更新 TxnId={0} 的 EZPos 交易資料 (CCardTxnDtlEntity) 發生錯誤，錯誤訊息：{1}", txnId, xmlResult.Message).AppendLine();
                            }
                        }
                        else
                        {
                            log.AppendFormat("查無 TxnId={0} 的 EZPos 交易資料 (CCardTxnDtlEntity)，或該資料的虛擬帳號或狀態不和", txnId).AppendLine();
                        }
                    }
                    else
                    {
                        log.AppendFormat("查詢 TxnId={0} 的 EZPos 交易資料 (CCardTxnDtlEntity) 發生錯誤，錯誤訊息：{1}", txnId, xmlResult.Message).AppendLine();
                    }
                }
                #endregion

                #region 更新
                //如果授權成功，更新學生資料繳費明細檔(STUDENT_RECEIVE)的繳款方式 + 代收日
                //更新條件，代收類別 + (由銷帳編號擷取) + 銷帳編號 + 未繳 (+ 金額) (+ 學號)
                //如果更新失敗，不要影響目前的流程
                if (myStatus == "2")
                {
                    string receiveType = cancelNo.Substring(0, 4);

                    Expression w1 = new Expression(StudentReceiveEntity.Field.ReceiveDate, null)
                        .Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty);
                    Expression w2 = new Expression(StudentReceiveEntity.Field.ReceiveWay, null)
                        .Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty);
                    Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                        .And(StudentReceiveEntity.Field.CancelNo, cancelNo)
                        .And(w1)
                        .And(w2);
                    if (txn != null)
                    {
                        if (txn.Amount != null)
                        {
                            where.And(StudentReceiveEntity.Field.ReceiveAmount, txn.Amount.Value);
                        }
                        if (!String.IsNullOrEmpty(txn.StudentNo))
                        {
                            where.And(StudentReceiveEntity.Field.StuId, txn.StudentNo);
                        }
                    }

                    #region [MDY:20191214] (2019擴充案) ReceiveDate 改用 AuthRespTime 或 txn.CreateDate
                    DateTime receiveDateTime = dAuthRespTime.HasValue ? dAuthRespTime.Value : txn.CreateDate;

                    KeyValue[] fieldValues = new KeyValue[] {
                        new KeyValue(StudentReceiveEntity.Field.ReceiveWay, ChannelHelper.FISC),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveDate, Common.GetTWDate7(receiveDateTime)),
                        new KeyValue(StudentReceiveEntity.Field.ReceiveTime, String.Empty)
                    };
                    #endregion

                    int count = 0;
                    XmlResult xmlResult = DataProxy.Current.UpdateFields<StudentReceiveEntity>(this.Page, where, fieldValues, out count);
                    if (xmlResult.IsSuccess)
                    {
                        log.AppendFormat("更新 學生繳費資料 (StudentReceiveEntity) 成功 ({0}筆)", count).AppendLine();
                    }
                    else
                    {
                        log.AppendFormat("更新 學生繳費資料 (StudentReceiveEntity) 發生錯誤，錯誤訊息：{0}", xmlResult.Message).AppendLine();
                    }
                }
                #endregion

                this.WriteLog(log.ToString());
            }
        }
    }
}