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
using Entities.SchoolService;
using Helpers;

namespace eSchoolWeb
{
    /// <summary>
    /// 連動製單服務
    /// </summary>
    [WebService(Namespace = "http://eschool.landbank.com.tw/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class SchoolService : System.Web.Services.WebService, IMenuPage
    {
        #region Implement IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public virtual string MenuID
        {
            get
            {
                return "SchoolService";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public virtual string MenuName
        {
            get
            {
                return "連動製單服務";
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
        private const string _LogName = "SchoolService";
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
        /// <param name="methodName">方法名稱</param>
        /// <param name="msg">訊息</param>
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
                //_logPath = String.Empty;
            }
        }
        #endregion

        #region Private Method
        //private string CheckSchoolServiceAccount(string sysId, string sysPwd, out SchoolServiceAccountEntity account)
        //{
        //    account = null;

        //    if (String.IsNullOrWhiteSpace(sysId) || String.IsNullOrWhiteSpace(sysPwd))
        //    {
        //        return "S0001系統代碼或系統驗證碼錯誤";
        //    }

        //    string ip = WebHelper.GetClientIP();
        //    if (String.IsNullOrWhiteSpace(ip))
        //    {
        //        return "S0002無法判斷呼叫端IP";
        //    }

        //    Expression where = new Expression(SchoolServiceAccountEntity.Field.SysId, sysId)
        //        .And(SchoolServiceAccountEntity.Field.SysPwd, sysPwd)
        //        .And(SchoolServiceAccountEntity.Field.Status, DataStatusCodeTexts.NORMAL);
        //    XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolServiceAccountEntity>(null, where, null, out account);
        //    if (!xmlResult.IsSuccess)
        //    {
        //        this.WriteLog("查詢帳號 {0} 密碼 {1} 資料失敗，錯誤訊息：({2}) {4}", "CheckSchoolServiceAccount", sysId, sysPwd, xmlResult.Code, xmlResult.Message);
        //        return "S9999系統錯誤";
        //    }

        //    if (account == null)
        //    {
        //        return "S0001系統代碼或系統驗證碼錯誤";
        //    }
        //    if (account.SysPwd != sysPwd)
        //    {
        //        return "S0001系統代碼或系統驗證碼錯誤";
        //    }
        //    if (!account.IsMyClientIP(ip))
        //    {
        //        return "S0002IP錯誤";
        //    }

        //    return null;
        //}
        #endregion

        #region 單筆新增、修改、刪除學生繳費資料相關
        /// <summary>
        /// 單筆新增、修改、刪除學生繳費資料
        /// </summary>
        /// <param name="sys_id">系統代碼 X(32) 必需</param>
        /// <param name="sys_pwd">系統驗證碼 X(32) 必需</param>
        /// <param name="op">操作 X(1) 必需 (I：新增、U：修改、D：刪除)</param>
        /// <param name="user">學校操作人員 X(32) 必需</param>
        /// <param name="mdy_date">學校操作日期 9(8) 必需 (格式：yyyymmdd)</param>
        /// <param name="mdy_time">學校操作時間 9(6) 必需 (格式：hhmmss)</param>
        /// <param name="txn_data">學生繳費單資料 X(2000) 必需 (參考學生繳費單資料格式說明。內容為XML)</param>
        /// <returns>傳回錯誤訊息 X(200) ([err_code][err_msg] 前5碼為錯誤代碼，後面跟著錯誤訊息)</returns>
        [WebMethod]
        public string bill(string sys_id, string sys_pwd, string op, string user, string mdy_date, string mdy_time, string txn_data)
        {
            if (String.IsNullOrWhiteSpace(sys_id) || String.IsNullOrWhiteSpace(sys_pwd))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0001);
            }
            if (!BillOpList.IsDefine(op))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0003);
            }
            if (String.IsNullOrWhiteSpace(user))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0004);
            }
            DateTime mdyDateTime;
            if (String.IsNullOrWhiteSpace(mdy_date) || mdy_date.Length != 8 || String.IsNullOrWhiteSpace(mdy_time) || mdy_time.Length != 6
                || !DateTime.TryParse(mdy_date.Insert(6, "/").Insert(4, "/") + " " +mdy_time.Insert(4, ":").Insert(2, ":"), out mdyDateTime))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0005);
            }
            if (String.IsNullOrWhiteSpace(txn_data))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0006);
            }

            string errmsg = DataProxy.Current.CallSchoolServiceForBill(sys_id, sys_pwd, op, user, mdy_date, mdy_time, txn_data);
            return errmsg;
        }
        #endregion

        #region 學生繳費資料查詢
        /// <summary>
        /// 學生繳費資料查詢
        /// </summary>
        /// <param name="sys_id">系統代碼 X(32) 必需</param>
        /// <param name="sys_pwd">系統驗證碼 X(32) 必需</param>
        /// <param name="receive_type">商家代號 9(4) 必需</param>
        /// <param name="year_id">學年代號 9(3) 必需</param>
        /// <param name="term_id">學期代號 9(1) 必需</param>
        /// <param name="stu_id_start">起始學號 X(20) 擇一</param>
        /// <param name="stu_id_end">結束學號 X(20) 擇一</param>
        /// <param name="seq_no">學校端惟一序號 X(32) 擇一</param>
        /// <param name="records">傳回筆數</param>
        /// <param name="txn_file">傳回學生繳費資料檔 (zip 檔密碼為 sys_pwd)</param>
        /// <returns>傳回錯誤訊息 X(200) ([err_code][err_msg] 前5碼為錯誤代碼，後面跟著錯誤訊息)</returns>
        [WebMethod]
        public string bill_query(string sys_id, string sys_pwd, string receive_type, string year_id, string term_id, string stu_id_start, string stu_id_end, string seq_no
            , out int records, out byte[] txn_file)
        {
            records = 0;
            txn_file = null;
            if (String.IsNullOrWhiteSpace(sys_id) || String.IsNullOrWhiteSpace(sys_pwd))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0001);
            }
            if (String.IsNullOrWhiteSpace(receive_type) || String.IsNullOrWhiteSpace(year_id) || String.IsNullOrWhiteSpace(term_id))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "缺少商家代號、學年代號或學期代號的查詢參數");
            }
            if (String.IsNullOrWhiteSpace(stu_id_start) && String.IsNullOrWhiteSpace(stu_id_end) && String.IsNullOrWhiteSpace(seq_no))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "起始學號、結束學號或學校端惟一序號的查詢參數至少要指定一項");
            }

            string errmsg = DataProxy.Current.CallSchoolServiceForBillQuery(sys_id, sys_pwd, receive_type, year_id, term_id, stu_id_start, stu_id_end, seq_no
                , out records, out txn_file);
            return errmsg;
        }
        #endregion

        #region 入金資料查詢
        /// <summary>
        /// 入金資料查詢
        /// </summary>
        /// <param name="sys_id">系統代碼 X(32) 必需</param>
        /// <param name="sys_pwd">系統驗證碼 X(32) 必需</param>
        /// <param name="receive_type">商家代號 9(4) 必需</param>
        /// <param name="cancel_no">虛擬帳號 擇一</param>
        /// <param name="receive_way">代收管道 9(16) 擇一</param>
        /// <param name="account_date">入帳日 9(8) (格式：yyyymmdd) 擇一</param>
        /// <param name="records">傳回筆數</param>
        /// <param name="txn_file">傳回學生繳費資料檔 (zip 檔密碼為 sys_pwd)</param>
        /// <returns>傳回錯誤訊息 X(200) ([err_code][err_msg] 前5碼為錯誤代碼，後面跟著錯誤訊息)</returns>
        [WebMethod]
        public string pay_query(string sys_id, string sys_pwd, string receive_type, string cancel_no, string receive_way, string account_date
            , out int records, out byte[] txn_file)
        {
            records = 0;
            txn_file = null;
            if (String.IsNullOrWhiteSpace(sys_id) || String.IsNullOrWhiteSpace(sys_pwd))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0001);
            }
            if (String.IsNullOrWhiteSpace(receive_type))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "缺少商家代號的查詢參數");
            }
            if (String.IsNullOrWhiteSpace(cancel_no) && String.IsNullOrWhiteSpace(receive_way) && String.IsNullOrWhiteSpace(account_date))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "虛擬帳號、代收管道或入帳日的查詢參數至少要指定一項");
            }
            if (String.IsNullOrWhiteSpace(cancel_no) && !String.IsNullOrWhiteSpace(receive_way) && String.IsNullOrWhiteSpace(account_date))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "指定代收管道查詢參數時必須同時指定入帳日的查詢參數");
            }

            string errmsg = DataProxy.Current.CallSchoolServiceForPayQuery(sys_id, sys_pwd, receive_type, cancel_no, receive_way, account_date
                , out records, out txn_file);
            return errmsg;
        }
        #endregion

        #region [MDY:20191219] 新增繳費資訊查詢
        /// <summary>
        /// 繳費資訊查詢
        /// </summary>
        /// <param name="sys_id">系統代碼 X(32) 必需</param>
        /// <param name="sys_pwd">系統驗證碼 X(32) 必需</param>
        /// <param name="data_kind">資料類別 9(4) 必需（1=帳務中心資料; 2=繳費單已繳代銷資料; 2=繳費單已銷資料;）</param>
        /// <param name="receive_type">商家代號 9(4) 必需</param>
        /// <param name="cancel_nos">虛擬帳號清單 （以逗號隔開）</param>
        /// <param name="seq_nos">學校端唯一序號清單（以逗號隔開，限data_kind=2 或 3有效）</param>
        /// <param name="recevie_date">代收日期 9(8)</param>
        /// <param name="start_record">起始資料位置 9(8)</param>
        /// <param name="record_count">傳回符合條件的總筆數</param>
        /// <param name="txn_file">傳回繳費資訊檔 (zip 檔密碼為 sys_pwd)</param>
        /// <returns>傳回錯誤訊息 X(200) ([err_code][err_msg] 前5碼為錯誤代碼，後面跟著錯誤訊息)</returns>
        [WebMethod]
        public string query_data(string sys_id, string sys_pwd, string data_kind, string receive_type
            , string cancel_nos, string seq_nos, string recevie_date, int start_record
            , out int record_count, out byte[] txn_file)
        {
            record_count = 0;
            txn_file = null;

            #region 檢查參數
            if (String.IsNullOrWhiteSpace(sys_id) || String.IsNullOrWhiteSpace(sys_pwd))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0001);
            }
            if (String.IsNullOrWhiteSpace(data_kind))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "缺少資料類別的查詢參數");
            }
            if (String.IsNullOrWhiteSpace(receive_type))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "缺少商家代號的查詢參數");
            }
            if (String.IsNullOrWhiteSpace(cancel_nos) && String.IsNullOrWhiteSpace(seq_nos) && String.IsNullOrWhiteSpace(recevie_date))
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "虛擬帳號清單、學校端唯一序號清單或代收日期的查詢參數至少要指定一項");
            }
            if (start_record < 1)
            {
                return ErrorList.GetErrorMessage(ErrorList.S0007, "起始資料位置必須大於 0");
            }
            if (!String.IsNullOrWhiteSpace(recevie_date))
            {
                DateTime? receiveDate = DataFormat.ConvertDateText(recevie_date);
                if (!receiveDate.HasValue)
                {
                    return ErrorList.GetErrorMessage(ErrorList.S0007, "代收日期參數不是有效的日期");
                }
            }
            #endregion

            string errmsg = DataProxy.Current.CallSchoolServiceForQueryData(sys_id, sys_pwd, data_kind, receive_type, cancel_nos, seq_nos, recevie_date, start_record
                , out record_count, out txn_file);
            return errmsg;
        }
        #endregion
    }
}
