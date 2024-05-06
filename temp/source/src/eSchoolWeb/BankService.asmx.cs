using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

using Fuju;
using Fuju.DB;
using Fuju.Configuration;
using Fuju.Web;
using Fuju.Web.Proxy;
using Fuju.Web.Services;

using Entities;
using Entities.BankService;
using Helpers;

namespace eSchoolWeb
{
    /// <summary>
    ///即查繳服務
    /// </summary>
    [WebService(Namespace = "http://eschool.landbank.com.tw/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class BankService : System.Web.Services.WebService, IMenuPage
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public virtual string MenuID
        {
            get
            {
                return "BankService";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public virtual string MenuName
        {
            get
            {
                return "即查繳服務";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public virtual bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public virtual bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public virtual bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Log 相關
        private const string _LogName = "BankService";
        private string _LogPath = null;

        /// <summary>
        /// 取得 Log 檔完整路徑檔名
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName()
        {
            if (_LogPath == null)
            {
                _LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                if (String.IsNullOrWhiteSpace(_LogPath))
                {
                    _LogPath = String.Empty;
                }
                else
                {
                    _LogPath = _LogPath.Trim();
                    try
                    {
                        DirectoryInfo info = new DirectoryInfo(_LogPath);
                        if (!info.Exists)
                        {
                            info.Create();
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
                return Path.Combine(_LogPath, String.Format("{0}_{1:yyyyMMdd}.log", _LogName, DateTime.Today));
            }
        }

        /// <summary>
        /// 寫 Log
        /// </summary>
        /// <param name="msg">訊息</param>
        /// <param name="methodName">方法名稱</param>
        private void WriteLog(string msg, string methodName = "")
        {
            if (!String.IsNullOrEmpty(msg))
            {
                string logFileName = this.GetLogFileName();
                if (!String.IsNullOrEmpty(logFileName))
                {
                    StringBuilder log = new StringBuilder();
                    log
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, methodName).AppendLine()
                        .AppendLine(msg)
                        .AppendLine();

                    this.WriteLogFile(logFileName, log.ToString());
                }
            }
        }

        /// <summary>
        /// 寫 Log
        /// </summary>
        /// <param name="format"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        private void WriteLog(string format, string methodName, params object[] args)
        {
            if (String.IsNullOrWhiteSpace(format) || args == null || args.Length == 0)
            {
                return;
            }

            try
            {
                this.WriteLog(String.Format(format, args), methodName);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 寫入 Log 檔
        /// </summary>
        /// <param name="logFileName">Log 檔名</param>
        /// <param name="logContent">Log 內容</param>
        private void WriteLogFile(string logFileName, string logContent)
        {
            try
            {
                File.AppendAllText(logFileName, logContent, Encoding.Default);
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region 以銷帳編號查詢繳費單資訊
        /// <summary>
        /// 以銷帳編號查詢繳費單資訊
        /// </summary>
        /// <param name="rqXml">參數 xml</param>
        /// <returns>回傳 xml</returns>
        [WebMethod]
        public string Q0002(string rqXml)
        {
            #region XML Sample
            ///查詢 xml 範例
            /// <Bot>
            ///   <op>Q0002</op>
            ///   <orgid></orgid>
            ///   <pwd></pwd>
            ///   <rid></rid>
            /// </Bot>
            /// 回傳 xml 範例
            /// <Bot>
            ///   <txnno>Q0002</txnno>
            ///   <errcode></errcode>
            ///   <studentname></studentname>
            ///   <ridno></ridno> 
            ///   <amount></amount>   <payduedate></payduedate>
            /// </Bot>
            #endregion

            string rsXml = null;

            #region Log
            this.WriteLog("rqXml = {0}", "Q0002", rqXml);
            #endregion

            #region 檢查參數
            Q0002TxnData txnData = null;
            {
                if (String.IsNullOrWhiteSpace(rqXml))
                {
                    Q0002RtnData rtnData = new Q0002RtnData(ErrorList.S999);
                    rsXml = rtnData.ToXml();

                    #region Log
                    this.WriteLog("rsXml = {0}", "Q0002", rsXml);
                    #endregion

                    return rsXml;
                }
                else
                {
                    string errmsg = null;
                    txnData = new Q0002TxnData();
                    string errCode = txnData.LoadXml(rqXml, out errmsg);
                    if (errCode != ErrorList.NORMAL)
                    {
                        Q0002RtnData rtnData = new Q0002RtnData(errCode);
                        rsXml = rtnData.ToXml();

                        #region Log
                        this.WriteLog("rsXml = {0}", "Q0002", rsXml);
                        #endregion

                        return rsXml;
                    }
                }
            }
            #endregion

            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            XmlResult xmlResult = DataProxy.Current.CallBankServiceQ0002(txnData.OrgId, txnData.PXX, rqXml, out rsXml);
            #endregion
            #endregion

            if (xmlResult.IsSuccess)
            {
                #region Log
                this.WriteLog("rsXml = {0}", "Q0002", rsXml);
                #endregion

                return rsXml;
            }
            else
            {
                Q0002RtnData rtnData = null;
                string errCode = xmlResult.Code;
                if (ErrorList.IsDefine(errCode))
                {
                    rtnData = new Q0002RtnData(errCode);
                }
                else
                {
                    rtnData = new Q0002RtnData(ErrorList.S990);
                }
                rsXml = rtnData.ToXml();

                #region Log
                this.WriteLog("rsXml = {0}\r\n交易失敗，錯誤訊息：{1}", "Q0002", rsXml, xmlResult.Message);
                #endregion

                return rsXml;
            }
        }
        #endregion

        #region 信用卡繳費成功通知
        /// <summary>
        /// 信用卡繳費成功通知
        /// </summary>
        /// <param name="rqXml">參數 xml</param>
        /// <returns>回傳 xml</returns>
        [WebMethod]
        public string T0001(string rqXml)
        {
            #region XML Sample
            ///查詢 xml 範例
            /// <Bot>
            ///   <op>T0001</op>
            ///   <orgid></orgid>
            ///   <pwd></pwd>
            ///   <seqno></seqno>
            ///   <rid></rid>
            ///   <amount></amount>
            ///   <paydate></paydate>
            ///   <paytime></paytime>
            ///   <authcode></authcode>
            ///   <MAC></MAC>
            /// </Bot>
            /// 回傳 xml 範例
            /// <Bot>
            ///   <txnno> T0001</txnno>
            ///   <errcode></errcode>
            ///   <MAC></MAC>
            /// </Bot>
            #endregion

            string rsXml = null;

            #region Log
            this.WriteLog("rqXml = {0}", "T0001", rqXml);
            #endregion

            #region 檢查參數
            T0001TxnData txnData = null;
            string mac = null;
            {
                if (String.IsNullOrWhiteSpace(rqXml))
                {
                    T0001RtnData rtnData = new T0001RtnData(ErrorList.S999, mac);
                    rsXml = rtnData.ToXml();

                    #region Log
                    this.WriteLog("rsXml = {0}", "T0001", rsXml);
                    #endregion

                    return rsXml;
                }
                else
                {
                    string errmsg = null;
                    txnData = new T0001TxnData();
                    string errCode = txnData.LoadXml(rqXml, out errmsg);
                    if (errCode != ErrorList.NORMAL)
                    {
                        if (txnData != null)
                        {
                            mac = txnData.MAC;
                        }
                        T0001RtnData rtnData = new T0001RtnData(errCode, mac);
                        rsXml = rtnData.ToXml();

                        #region Log
                        this.WriteLog("rsXml = {0}", "T0001", rsXml);
                        #endregion

                        return rsXml;
                    }
                }
            }
            #endregion

            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            XmlResult xmlResult = DataProxy.Current.CallBankServiceT0001(txnData.OrgId, txnData.PXX, rqXml, out rsXml);
            #endregion
            #endregion

            if (xmlResult.IsSuccess)
            {
                #region Log
                this.WriteLog("rsXml = {0}", "T0001", rsXml);
                #endregion

                return rsXml;
            }
            else
            {
                T0001RtnData rtnData = null;
                string errCode = xmlResult.Code;
                if (ErrorList.IsDefine(errCode))
                {
                    rtnData = new T0001RtnData(errCode, mac);
                }
                else
                {
                    rtnData = new T0001RtnData(ErrorList.S990, mac);
                }
                rsXml = rtnData.ToXml();

                #region Log
                this.WriteLog("rsXml = {0}\r\n交易失敗，錯誤訊息：{1}", "T0001", rsXml, xmlResult.Message);
                #endregion

                return rsXml;
            }
        }
        #endregion
    }
}
