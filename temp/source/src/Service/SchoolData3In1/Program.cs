using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

using Entities;

namespace SchoolData3In1
{
    /// <summary>
    /// 學校銷帳檔3合1 (SC31) (合併D70下載與產生、發送學校銷帳檔)
    /// </summary>
    class Program
    {
        #region FileLoger 日誌檔類別
        /// <summary>
        /// 日誌檔類別
        /// </summary>
        private class FileLoger
        {
            #region Member
            /// <summary>
            /// 日誌內容暫存大小 (512 * 1024)
            /// </summary>
            private const int _BufferSize = 64 * 1024;

            /// <summary>
            /// 日誌內容暫存
            /// </summary>
            private StringBuilder _Buffer = new StringBuilder(_BufferSize + 1024);
            #endregion

            #region Property
            private string _LogName = null;
            /// <summary>
            /// 日誌名稱
            /// </summary>
            public string LogName
            {
                get
                {
                    return _LogName;
                }
                private set
                {
                    _LogName = value == null ? null : value.Trim();
                }
            }

            private string _LogPath = null;
            /// <summary>
            /// 日誌檔路徑
            /// </summary>
            public string LogPath
            {
                get
                {
                    return _LogPath;
                }
                private set
                {
                    _LogPath = value == null ? String.Empty : value.Trim();
                }
            }

            /// <summary>
            /// 是否啟用除錯模式
            /// </summary>
            public bool IsDebug
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌檔完整路徑檔名
            /// </summary>
            public string LogFileName
            {
                get;
                private set;
            }

            /// <summary>
            /// 日誌序號
            /// </summary>
            public string LogSN
            {
                get;
                private set;
            }

            /// <summary>
            /// 取得日誌檔物件是否準備好
            /// </summary>
            public bool IsReady
            {
                get;
                private set;
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 日誌檔類別 物件，日誌檔路徑與是否啟用除錯模式取自 Config 的 appSettings 的 LOG_PATH 與 LOG_MDOE 參數設定
            /// </summary>
            /// <param name="logName">日誌名稱 (通常使用應用程式執行檔名稱)</param>
            public FileLoger(string logName)
            {
                this.LogName = logName;
                this.LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
                if (String.IsNullOrWhiteSpace(logMode))
                {
                    this.IsDebug = false;
                }
                else
                {
                    this.IsDebug = (logMode.Trim().Equals("DEBUG", StringComparison.CurrentCultureIgnoreCase));
                }
                this.Initial();
            }

            /// <summary>
            /// 建構 日誌檔類別 物件
            /// </summary>
            /// <param name="logName">日誌名稱 (通常使用應用程式執行檔名稱)</param>
            /// <param name="logPath">日誌檔路徑</param>
            /// <param name="isDebug">是否啟用除錯模式</param>
            public FileLoger(string logName, string logPath, bool isDebug)
            {
                this.LogName = logName;
                this.LogPath = logPath;
                this.IsDebug = isDebug;
                this.Initial();
            }
            #endregion

            #region Private Method
            /// <summary>
            /// 日誌檔物件初始化
            /// </summary>
            /// <returns></returns>
            private string Initial()
            {
                this.LogSN = String.Concat("SN", DateTime.Now.Ticks.ToString("X16"));

                string errmsg = null;
                if (String.IsNullOrEmpty(this.LogPath))
                {
                    errmsg = "未指定日誌檔路徑";
                }
                else
                {
                    try
                    {
                        DirectoryInfo info = new DirectoryInfo(this.LogPath);
                        if (!info.Exists)
                        {
                            info.Create();
                        }
                        if (String.IsNullOrEmpty(this.LogName))
                        {
                            string fileName = String.Format("{0:yyyyMMdd}.log", DateTime.Today);
                            this.LogFileName = Path.Combine(info.FullName, fileName);
                        }
                        else
                        {
                            string fileName = String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today);
                            this.LogFileName = Path.Combine(info.FullName, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        errmsg = ex.Message;
                    }
                }
                this.IsReady = String.IsNullOrEmpty(errmsg);
                return errmsg;
            }

            /// <summary>
            /// 刷新日誌內容暫存
            /// </summary>
            private void RefreshBuffer()
            {
                if (_Buffer.Length > 0)
                {
                    this.WriteLogBuffer();
                    _Buffer.Clear();
                }
            }

            /// <summary>
            /// 將日誌內容暫存寫入日誌檔
            /// </summary>
            private void WriteLogBuffer()
            {
                if (this.IsReady && _Buffer != null && _Buffer.Length > 0)
                {
                    try
                    {
                        #region [MDY:20220618] Checkmarx 調整 (Information Exposure Through an Error Message)
                        #region [OLD] 土銀建議改用 File.AppendAllText()，都是寫檔有什麼差別，很奇怪
                        //using (StreamWriter sw = new StreamWriter(this.LogFileName, true, Encoding.Default))
                        //{
                        //    sw.WriteLine(String.Format(@"[日誌序號：{0}] ------\\", this.LogSN));
                        //    sw.WriteLine(_Buffer);
                        //    sw.WriteLine(String.Format(@"[日誌序號：{0}] ------//", this.LogSN));
                        //    sw.WriteLine();
                        //    sw.Flush();
                        //}
                        #endregion

                        string content = $@"
[日誌序號：{this.LogSN}] ------\\
{_Buffer.ToString()}
[日誌序號：{this.LogSN}] ------//
";
                        File.AppendAllText(this.LogFileName, content, Encoding.Default);
                        #endregion
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            /// <summary>
            /// 新增日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="logTime">是否包含日誌時間</param>
            /// <param name="msg">日誌資訊</param>
            /// <returns></returns>
            private FileLoger AppendLogLine(bool logTime, string msg)
            {
                if (this.IsReady)
                {
                    if (logTime)
                    {
                        _Buffer.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                    }
                    _Buffer.AppendLine(msg);
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.RefreshBuffer();
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增格式化的日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="logTime">是否包含日誌時間</param>
            /// <param name="format">格式化字串</param>
            /// <param name="args">日誌資訊陣列</param>
            /// <returns></returns>
            private FileLoger AppendLogFormatLine(bool logTime, string format, params object[] args)
            {
                if (this.IsReady)
                {
                    if (logTime)
                    {
                        _Buffer.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                    }
                    _Buffer.AppendFormat(format, args).AppendLine();
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.RefreshBuffer();
                    }
                }
                return this;
            }
            #endregion

            #region Public Method
            /// <summary>
            /// 新增日誌資訊至日誌檔
            /// </summary>
            /// <param name="msg">日誌資訊</param>
            public FileLoger AppendLog(string msg)
            {
                if (this.IsReady)
                {
                    _Buffer.Append(msg);
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.RefreshBuffer();
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增包含日誌時間的日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="msg">日誌資訊</param>
            /// <returns></returns>
            public FileLoger AppendLogStartLine(string msg)
            {
                #region [Old]
                //if (this.IsReady)
                //{
                //    _Buffer.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now);
                //    _Buffer.AppendLine(msg);
                //    if (_Buffer.Length > _BufferSize)
                //    {
                //        this.RefreshBuffer();
                //    }
                //}
                //return this;
                #endregion

                return this.AppendLogLine(true, msg);
            }

            /// <summary>
            /// 新增日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="msg">日誌資訊</param>
            public FileLoger AppendLogLine(string msg)
            {
                #region [Old]
                //if (this.IsReady)
                //{
                //    _Buffer.AppendLine(msg);
                //    if (_Buffer.Length > _BufferSize)
                //    {
                //        this.RefreshBuffer();
                //    }
                //}
                //return this;
                #endregion

                return this.AppendLogLine(false, msg);
            }

            /// <summary>
            /// 新增格式化的日誌資訊至日誌檔
            /// </summary>
            /// <param name="format">格式化字串</param>
            /// <param name="args">日誌資訊陣列</param>
            public FileLoger AppendLogFormat(string format, params object[] args)
            {
                if (this.IsReady)
                {
                    _Buffer.AppendFormat(format, args);
                    if (_Buffer.Length > _BufferSize)
                    {
                        this.RefreshBuffer();
                    }
                }
                return this;
            }

            /// <summary>
            /// 新增格式化的包含日誌時間的日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="format"></param>
            /// <param name="args"></param>
            /// <returns></returns>
            public FileLoger AppendLogFormatStartLine(string format, params object[] args)
            {
                #region [Old]
                //if (this.IsReady)
                //{
                //    _Buffer.AppendFormat(format, args).AppendLine();
                //    if (_Buffer.Length > _BufferSize)
                //    {
                //        this.RefreshBuffer();
                //    }
                //}
                //return this;
                #endregion

                return this.AppendLogFormatLine(true, format, args);
            }

            /// <summary>
            /// 新增格式化的日誌資訊至日誌檔並換行
            /// </summary>
            /// <param name="format">格式化字串</param>
            /// <param name="args">日誌資訊陣列</param>
            public FileLoger AppendLogFormatLine(string format, params object[] args)
            {
                #region [Old]
                //if (this.IsReady)
                //{
                //    _Buffer.AppendFormat(format, args).AppendLine();
                //    if (_Buffer.Length > _BufferSize)
                //    {
                //        this.RefreshBuffer();
                //    }
                //}
                //return this;
                #endregion

                return this.AppendLogFormatLine(false, format, args);
            }

            /// <summary>
            /// 新增除錯資訊至日誌檔並換行
            /// </summary>
            /// <param name="msg">除錯資訊</param>
            public FileLoger AppendDebugLine(string msg)
            {
                if (this.IsReady && this.IsDebug)
                {
                    this.AppendLogLine("[DEBUG] " + msg);
                }
                return this;
            }

            /// <summary>
            /// 新增格式化的除錯資訊至日誌檔並換行
            /// </summary>
            /// <param name="format">格式化字串</param>
            /// <param name="args">除錯資訊陣列</param>
            public FileLoger AppendDebugFormatLine(string format, params object[] args)
            {
                if (this.IsReady && this.IsDebug)
                {
                    this.AppendLogFormatLine("[DEBUG] " + format, args);
                }
                return this;
            }

            /// <summary>
            /// 將日誌內容暫存寫入日誌檔並清空暫存
            /// </summary>
            /// <returns></returns>
            public FileLoger Flush()
            {
                if (this.IsReady)
                {
                    this.RefreshBuffer();
                }
                return this;
            }
            #endregion
        }
        #endregion

        [Flags]
        private enum ActionEnum : uint
        {
            /// <summary>
            /// 未指定
            /// </summary>
            NONE = 0x00,
            /// <summary>
            /// 只執行『D70下載』作業
            /// </summary>
            D70  = 0x01,
            /// <summary>
            /// 只執行『產生學校銷帳檔』作業
            /// </summary>
            GEN  = 0x02,
            /// <summary>
            /// 只執行『發送學校銷帳檔』作業
            /// </summary>
            SEND = 0x04,
            /// <summary>
            /// 只執行GEN與SEND作業
            /// </summary>
            GENSEND = (GEN | SEND),
            /// <summary>
            /// 執行排程作業（即執行D70、GEN與SEND作業）
            /// </summary>
            SCHEDULE = (0x08 | D70 | GEN | SEND)
        }

        #region MyHelper 處理工具類別
        /// <summary>
        /// MyHelper 處理工具類別
        /// </summary>
        private class MyHelper : IDisposable
        {
            #region Const
            /// <summary>
            /// 作業類別代碼 = SC31 (學校銷帳檔3合1)
            /// </summary>
            private const string _JobTypeId = JobCubeTypeCodeTexts.SC31;
            /// <summary>
            /// 檔案類別代碼 =  CANCELED_DATA (學校銷帳檔類別)
            /// </summary>
            private const string _FileKind = "CANCELED_DATA";
            #endregion

            #region Member
            /// <summary>
            /// 儲存 資料存取物件 的變數
            /// </summary>
            private EntityFactory _Factory = null;
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 MyHelper 處理工具類別
            /// </summary>
            public MyHelper()
            {

            }
            #endregion

            #region Destructor
            /// <summary>
            /// 解構 MyHelper 處理工具類別
            /// </summary>
            ~MyHelper()
            {
                this.Dispose(false);
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
                this.Dispose(true);
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

            #region IO 相關 Method
            /// <summary>
            /// 清空指定資料夾
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public string ClearFolder(string path)
            {
                if (String.IsNullOrWhiteSpace(path))
                {
                    return "未指定資料夾路徑";
                }

                DirectoryInfo dInfo = null;
                try
                {
                    dInfo = new DirectoryInfo(path);
                }
                catch (Exception ex)
                {
                    return String.Format("資料夾路徑不正確，{0}", ex.Message);
                }

                if (dInfo.Exists)
                {
                    return this.ClearFolder(dInfo);
                }
                else
                {
                    return "資料夾不存在";
                }
            }

            /// <summary>
            /// 清空指定資料夾
            /// </summary>
            /// <param name="dInfo"></param>
            /// <returns></returns>
            public string ClearFolder(DirectoryInfo dInfo)
            {
                if (dInfo == null || !dInfo.Exists)
                {
                    return "未指定資料夾或該資料夾不存在";
                }

                try
                {
                    FileInfo[] fInfos = dInfo.GetFiles();
                    foreach (FileInfo finfo in fInfos)
                    {
                        finfo.Delete();
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return null;
            }

            /// <summary>
            /// 檢查指定資料夾路徑是否存在，不存在則建立該資料夾
            /// </summary>
            /// <param name="path">指定的資料夾路徑</param>
            /// <param name="fgClear">指定是否清空資料夾，預設為否</param>
            /// <returns>傳回錯誤訊息</returns>
            public string CheckThenCreateFolder(string path, bool fgClear = false)
            {
                if (String.IsNullOrWhiteSpace(path))
                {
                    return "未指定資料夾路徑";
                }

                DirectoryInfo dInfo = null;
                try
                {
                    dInfo = new DirectoryInfo(path);
                }
                catch (Exception ex)
                {
                    return String.Format("資料夾路徑不正確，{0}", ex.Message);
                }

                if (dInfo.Exists)
                {
                    #region 資料夾已存在，判斷是否要清空資料夾
                    if (fgClear)
                    {
                        string errmsg = this.ClearFolder(dInfo);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            return String.Format("清空資料夾失敗，{0}", errmsg);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 資料夾不存，建立該資料夾
                    try
                    {
                        dInfo.Create();
                    }
                    catch (Exception ex)
                    {
                        return String.Format("建立資料夾失敗，{0}", ex.Message);
                    }
                    #endregion
                }
                return null;
            }
            #endregion

            #region DB 相關 Method
            private EntityFactory GetFactory()
            {
                if (_Factory == null)
                {
                    _Factory = new EntityFactory();
                }
                return _Factory;
            }

            /// <summary>
            /// 取得 SC31ReceiveType 系統參數指定的商家代號
            /// </summary>
            /// <param name="schools">傳回取得的商家代號資料物件陣列</param>
            /// <param name="receiveType">指定 SC31ReceiveType 中的某商家代號</param>
            /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
            public string GetSC31ReceiveTypes(out SchoolRTypeEntity[] schools, string receiveType = null)
            {
                schools = null;
                string errmsg = null;
                try
                {
                    ConfigEntity config = null;
                    Result result = null;
                    using (EntityFactory factory = new EntityFactory())
                    {
                        #region 取 SC31ReceiveType 參數
                        {
                            Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SC31_RECEIVETYPE);
                            result = factory.SelectFirst<ConfigEntity>(where, null, out config);
                        }
                        #endregion

                        if (result.IsSuccess)
                        {
                            if (config == null || String.IsNullOrWhiteSpace(config.ConfigValue))
                            {
                                errmsg = "未設定 " + ConfigKeyCodeTexts.SC31_RECEIVETYPE_TEXT + " 參數值";
                            }
                            else
                            {
                                string[] values = config.ConfigValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (values.Length == 0)
                                {
                                    errmsg = ConfigKeyCodeTexts.SC31_RECEIVETYPE_TEXT + " 參數值無效";
                                }
                                else
                                {
                                    #region 處理 SC31ReceiveType 系統參數值，轉成商家代號陣列
                                    List<string> list = new List<string>(values.Length);
                                    for (int idx = 0; idx < values.Length; idx++)
                                    {
                                        string value = values[idx].Trim();
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            if (!list.Contains(value))
                                            {
                                                list.Add(value);
                                            }
                                        }
                                    }

                                    string[] receiveTypes = null;
                                    if (!String.IsNullOrWhiteSpace(receiveType))
                                    {
                                        receiveType = receiveType.Trim();
                                        if (list.IndexOf(receiveType) > -1)
                                        {
                                            receiveTypes = new string[] { receiveType };
                                        }
                                        else
                                        {
                                            errmsg = String.Format(ConfigKeyCodeTexts.SC31_RECEIVETYPE_TEXT + " 不包含 {0} 商家代號", receiveType);
                                        }
                                    }
                                    else
                                    {
                                        receiveTypes = list.ToArray();
                                    }
                                    #endregion

                                    #region 取得商家代號陣列中有效且有FTP設定的 SchoolRTypeEntity 資料陣列
                                    if (String.IsNullOrEmpty(errmsg))
                                    {
                                        Expression where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL)
                                            .And(SchoolRTypeEntity.Field.FtpAccount, RelationEnum.NotEqual, String.Empty)
                                            .And(SchoolRTypeEntity.Field.FtpAccount, RelationEnum.NotEqual, null)
                                            .And(SchoolRTypeEntity.Field.FtpLocation, RelationEnum.NotEqual, String.Empty)
                                            .And(SchoolRTypeEntity.Field.FtpLocation, RelationEnum.NotEqual, null)
                                            .And(SchoolRTypeEntity.Field.ReceiveType, RelationEnum.In, receiveTypes);
                                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                        orderbys.Add(SchoolRTypeEntity.Field.CorpType, OrderByEnum.Asc);
                                        orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);
                                        orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
                                        result = factory.SelectAll<SchoolRTypeEntity>(where, orderbys, out schools);
                                        if (result.IsSuccess)
                                        {
                                            if (schools == null || schools.Length == 0)
                                            {
                                                errmsg = ConfigKeyCodeTexts.SC31_RECEIVETYPE_TEXT + " 的商家代號皆無效或無FTP設定";
                                            }
                                        }
                                        else
                                        {
                                            errmsg = result.Message;
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            errmsg = result.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
                return errmsg;
            }

            /// <summary>
            /// 取得指定商家代號與交易日期 (TWDate8) 的下一個 EAI 交易序號
            /// </summary>
            /// <param name="receiveType">指定商家代號</param>
            /// <param name="txDay">指定交易日期 (TWDate8)</param>
            /// <param name="nextTxSeq">成功則傳回下一個 EAI 交易序號，否則傳回 0</param>
            /// <returns>成功則傳回 null，否則傳回錯誤訊息</returns>
            public string GetEAINextTxSeq(string receiveType, string txDay, out int nextTxSeq)
            {
                nextTxSeq = 0;
                string errmsg = null;
                try
                {
                    EntityFactory factory = this.GetFactory();

                    string sql = @"SELECT ISNULL(MAX([Source_Seq]), 0) + 1 FROM [Cancel_Debts] WHERE [Receive_Type] = @RECEIVETYPE AND [File_Name] = 'D00I70_' + [Receive_Type] + '_' + @TXDAY";
                    KeyValue[] parameters = new KeyValue[] { new KeyValue("@RECEIVETYPE", receiveType), new KeyValue("@TXDAY", txDay) };

                    object value = null;
                    Result result = factory.ExecuteScalar(sql, parameters, out value);
                    if (result.IsSuccess)
                    {
                        nextTxSeq = Convert.ToInt32(value);
                        if (nextTxSeq < 1)
                        {
                            errmsg = "SourceSeq 資料不正確";
                        }
                    }
                    else
                    {
                        errmsg = result.Message;
                    }
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
                return errmsg;
            }
            #endregion

            #region SendFile 相關 Method
            #region [MDY:2018xxxx] 服務改呼叫 FTPUpload() 方法
            #region [OLD]
            //public string SendFile(string full_file_name, string ap_id, string ap_pword)
            //{
            //    string log = "";
            //    //string ap_id = ConfigurationManager.AppSettings.Get("ap_id");

            //    //#region [MDY:20170717] 為了避過源碼掃描敏感字詞，所有密碼相關變數名稱改用 ap_pword
            //    //string ap_pword = ConfigurationManager.AppSettings.Get("ap_pword");
            //    //#endregion

            //    string file_kind = "CANCELED_DATA";

            //    string config_file_name = ConfigurationManager.AppSettings.Get("config_file_name");
            //    byte[] contents = File.ReadAllBytes(full_file_name);

            //    #region [MDY:20170416] 配合系統參數設定機制，FileService 網址改取自 FileServiceUrl 系統參數值
            //    #region [Old]
            //    //string endpointConfigurationName = ConfigHelper.Current.GetFileServiceSoapEndpointConfigurationName();
            //    //FileService.FileServiceSoapClient client = new FileService.FileServiceSoapClient(endpointConfigurationName);
            //    #endregion

            //    string errmsg = null;
            //    FileService.FileServiceSoapClient client = GetFileServiceClient(out errmsg);
            //    if (!String.IsNullOrEmpty(errmsg))
            //    {
            //        return "取得 FileService 服務物件失敗，" + errmsg;
            //    }
            //    #endregion

            //    try
            //    {
            //        #region {MDY:20170717] 忽略不信任的憑證
            //        if (client.Endpoint.Address.Uri.Scheme == Uri.UriSchemeHttps)
            //        {
            //            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            //        }
            //        #endregion

            //        log = client.ReceiveFile(ap_id, ap_pword, file_kind, config_file_name, contents);
            //    }
            //    catch (Exception ex)
            //    {
            //        log = ex.Message;
            //    }
            //    return log;
            //}
            #endregion

            /// <summary>
            /// 傳送檔案
            /// </summary>
            /// <param name="jobNo">作業代碼</param>
            /// <param name="fileFullName">發送檔完整路徑檔名</param>
            /// <param name="apId">Web 端檔案服務連線帳號</param>
            /// <param name="apCode">Web 端檔案服務連線代碼</param>
            /// <returns></returns>
            public string SendFile(int jobNo, string fileFullName, string apId, string apCode)
            {
                string procName = null;  //處理程序名稱
                try
                {
                    #region 檢查參數
                    procName = "檢查參數";

                    if (jobNo < 0)
                    {
                        return "作業代碼參數不正確";
                    }
                    if (String.IsNullOrWhiteSpace(fileFullName))
                    {
                        return "缺少發送檔參數或參數值不正確";
                    }
                    FileInfo fileInfo = new FileInfo(fileFullName);
                    if (!fileInfo.Exists)
                    {
                        return "發送檔不存在";
                    }
                    #endregion

                    #region 產生服務執行參數
                    procName = "產生服務執行參數";

                    string apData = Fuju.Common.GetBase64Encode(String.Format("{0}_{1}_{2}_{3}_{4}", apId, jobNo, _JobTypeId, _FileKind, apCode));
                    byte[] fileContents = File.ReadAllBytes(fileFullName);
                    #endregion

                    #region 呼叫服務
                    procName = "呼叫服務";

                    string errmsg = null;
                    FileService.FileServiceSoapClient client = GetFileServiceClient(out errmsg);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        return String.Concat("取得 FileService 服務物件失敗，", errmsg);
                    }

                    #region 忽略不信任的憑證
                    if (client.Endpoint.Address.Uri.Scheme == Uri.UriSchemeHttps)
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                    }
                    #endregion

                    string result = client.FTPUpload(apData, jobNo, _JobTypeId, _FileKind, fileContents);
                    if ("OK".Equals(result, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return null;
                    }
                    else
                    {
                        return String.Concat("服務回傳訊息：", result);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return String.Format("傳送檔案發生例外 [{0}]：{1}", procName, ex.Message);
                }
            }
            #endregion

            #region 配合系統參數設定機制，FileService 網址改取自 FileServiceUrl 系統參數值
            /// <summary>
            /// FileService 網址系統參 KEY
            /// </summary>
            private string FileServiceUrl_ConfigKey = "FileServiceUrl";

            /// <summary>
            /// 取得 FileServiceSoapClient
            /// </summary>
            /// <returns></returns>
            private FileService.FileServiceSoapClient GetFileServiceClient(out string errmsg)
            {
                errmsg = null;
                FileService.FileServiceSoapClient client = null;

                try
                {
                    #region 取得FileServiceUrl 系統參數值
                    string fileServiceUrl = null;
                    using (EntityFactory factory = new EntityFactory())
                    {
                        ConfigEntity data = null;
                        Expression where = new Expression(ConfigEntity.Field.ConfigKey, FileServiceUrl_ConfigKey);
                        Result result = factory.SelectFirst<ConfigEntity>(where, null, out data);
                        if (result.IsSuccess)
                        {
                            if (data != null && data.ConfigValue != null)
                            {
                                fileServiceUrl = data.ConfigValue.Trim();
                            }
                            if (String.IsNullOrEmpty(fileServiceUrl))
                            {
                                errmsg = String.Format("未設定 {0} 系統參數值", FileServiceUrl_ConfigKey);
                            }
                        }
                        else
                        {
                            errmsg = result.Message;
                        }
                    }
                    #endregion

                    if (String.IsNullOrEmpty(errmsg))
                    {
                        if (fileServiceUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                        {
                            client = new FileService.FileServiceSoapClient("FileServiceSoap_HTTPS");
                            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(fileServiceUrl);
                        }
                        else if (fileServiceUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
                        {
                            client = new FileService.FileServiceSoapClient("FileServiceSoap_HTTP");
                            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(fileServiceUrl);
                        }
                        else
                        {
                            errmsg = String.Format("{0} 系統參數值不合法", FileServiceUrl_ConfigKey);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }

                return client;
            }
            #endregion

            #region 忽略不信任的憑證
            /// <summary>
            /// 處理不信任的憑證 (忽略)
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="certificate"></param>
            /// <param name="chain"></param>
            /// <param name="sslPolicyErrors"></param>
            /// <returns></returns>
            public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }
            #endregion

            #endregion
        }
        #endregion


        /// <summary>
        /// 學校銷帳檔3合1，作業類別代碼固定為 SC31，不提供 retry 機制，同一時間 (分鐘) 排程僅允許一個作業，
        /// </summary>
        /// <param name="args">
        /// 必要參數
        /// do = 指定執行動作 (SCHEDULE：排程、ALL：D70+GEN+SEND、D70：D00I70資料下載、GEN：產生學校銷帳資料檔、SEND=發送學校銷帳資料檔)
        /// 選擇參數
        /// complement_time = 指定執行補齊昨天資料的時間 (HHmm)，執行動作為SCHEDULE時的必要參數
        /// receive_type = 指定處理的商家代號 (必須為SC31ReceiveType系統參數中的商家代號)，執行動作不為SCHEDULE時的選擇參數
        /// txdate = 指定資料日期，這裡是指 D00I70資料下載的交易日期、處理資料的代收日期 (TODAY, YESTERDAY 或 yyyyMMdd)，執行動作不為SCHEDULE時的必要參數
        /// </param>
        /// <remarks>
        /// 1. 不提供 retry 機制是因為每天會執行多次，且每次傳送當下的所有資料，所以沒必要 retry，且會破壞即時性
        /// 2. 不同的 do 參數需配合使用不同的選擇參數
        /// </remarks>
        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

            FileLoger fileLog = new FileLoger(appName);
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = JobCubeTypeCodeTexts.SC31;                   //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);   //作業類別名稱

            int exitCode = 0;
            string exitMsg = null;
            #endregion

            DateTime startTime = DateTime.Now;
            string argsLine = (args == null || args.Length == 0) ? String.Empty : String.Join(" ", args);

            MyHelper myHelper = new MyHelper();
            JobCubeHelper jobHelper = new JobCubeHelper();

            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobLog = new StringBuilder();     //job 日誌紀錄

            try
            {
                fileLog.AppendLogFormatStartLine("{0} 開始", appName);

                #region 處理命令參數
                ActionEnum action = ActionEnum.NONE;  //指定執行動作 (SCHEDULE、ALL、D70、GEN、SEND)
                string receiveType = null;  //指定處理的商家代號
                DateTime? txDate = null;    //指定資料日期 (D00I70資料下載的交易日期、處理資料的代收日期)
                if (exitCode == 0)
                {
                    fileLog.AppendLogFormatLine("命令參數：{0}", argsLine);

                    string errmsg = null;
                    int? complementTime = null; //執行補齊昨天資料的時間 (分鐘)
                    Regex regHHMM = new Regex("^([01][0-9]|2[0-3]):{0,1}([0-5][0-9])$", RegexOptions.Compiled);

                    if (args != null && args.Length > 0)
                    {
                        #region 拆解命令參數
                        foreach (string arg in args)
                        {
                            string[] kvs = arg.Split('=');
                            if (kvs.Length == 2)
                            {
                                string key = kvs[0].Trim().ToLower();
                                string value = kvs[1].Trim();
                                switch (key)
                                {
                                    case "do":
                                        #region action (指定執行動作)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            value = value.ToUpper();
                                            switch (value)
                                            {
                                                case "SCHEDULE":
                                                    action = ActionEnum.SCHEDULE;
                                                    break;
                                                case "GEN+SEND":
                                                    action = ActionEnum.GENSEND;
                                                    break;
                                                case "D70":
                                                    action = ActionEnum.D70;
                                                    break;
                                                case "GEN":
                                                    action = ActionEnum.GEN;
                                                    break;
                                                case "SEND":
                                                    action = ActionEnum.SEND;
                                                    break;
                                                default:
                                                    action = ActionEnum.NONE;
                                                    errmsg = "do 命令參數值無效 (僅允許 SCHEDULE, GEN+SEND, D70, GEN 或 SEND)";
                                                    break;
                                            }
                                        }
                                        break;
                                        #endregion
                                    case "complement_time":
                                        #region complementTime (指定執行補齊昨天資料的時間)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            Match match = regHHMM.Match(value);
                                            if (match.Success)
                                            {
                                                //轉成分鐘
                                                complementTime = Int32.Parse(match.Groups[1].Value) * 60 + Int32.Parse(match.Groups[2].Value);
                                            }
                                            else
                                            {
                                                errmsg = "complement_time 命令參數值不是有效的時間格式 (HHmm 格式，HH:2碼的時、mm:2碼的分)";
                                            }
                                        }
                                        break;
                                        #endregion
                                    case "receive_type":
                                        #region receiveType (指定處理的商家代號)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            receiveType = value.Trim();
                                        }
                                        break;
                                        #endregion
                                    case "txdate":
                                        #region txDate (指定資料日期)
                                        if (!String.IsNullOrEmpty(value))
                                        {
                                            switch (value.ToUpper())
                                            {
                                                case "TODAY":
                                                    txDate = startTime.Date;
                                                    break;
                                                case "YESTERDAY":
                                                    txDate = startTime.Date.AddDays(-1);
                                                    break;
                                                default:
                                                    txDate = DataFormat.ConvertDateText(value);
                                                    if (txDate == null)
                                                    {
                                                        errmsg = "txdate 命令參數值不是有效的日期格式 (TODAY, YESTERDAY 或 yyyyMMdd 格式)";
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                        #endregion

                                    default:
                                        errmsg = String.Format("不支援 {0} 命令參數", arg);
                                        break;
                                }
                            }
                            else
                            {
                                errmsg = String.Format("不支援 {0} 命令參數", arg);
                            }
                            if (!String.IsNullOrEmpty(errmsg))
                            {
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        errmsg = "未指定命令參數";
                    }

                    #region 檢查必要命令參數
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        switch (action)
                        {
                            case ActionEnum.SCHEDULE:   //執行排程作業
                                #region 執行排程作業
                                {
                                    receiveType = null;
                                    if (complementTime == null)
                                    {
                                        errmsg = "缺少 complement_time 命令參數";
                                    }
                                    else
                                    {
                                        int nowTime = startTime.Hour * 60 + startTime.Minute;
                                        if (complementTime == nowTime)
                                        {
                                            txDate = startTime.Date.AddDays(-1);  //前一天
                                        }
                                        else
                                        {
                                            txDate = startTime.Date;    //當天
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case ActionEnum.D70:        //執行D70下載
                            case ActionEnum.GEN:        //執行產生學校銷帳檔
                            case ActionEnum.GENSEND:    //執行GEN與SEND作業
                                #region 執行D70下載、產生學校銷帳檔、GEN與SEND作業
                                {
                                    if (txDate == null)
                                    {
                                        errmsg = "缺少 txdate 命令參數";
                                    }
                                }
                                #endregion
                                break;
                            case ActionEnum.SEND:       //發送學校銷帳檔
                                #region 執行發送學校銷帳檔
                                {
                                    receiveType = null;
                                    if (txDate == null)
                                    {
                                        errmsg = "缺少 txdate 命令參數";
                                    }
                                }
                                #endregion
                                break;
                            default:
                                errmsg = "缺少 do 命令參數";
                                break;
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -1;
                        exitMsg = String.Format("命令參數錯誤，錯誤訊息：{0}", errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);

                        if (args == null || args.Length == 0)
                        {
                            fileLog.AppendLogLine("命令參數語法：do = 指定執行動作 (SCHEDULE、D70、GEN、SEND、GEN+SEND) [complement_time=指定執行補齊昨天資料的時間(HHmm)] [receive_type=指定處理的商家代號(必須為" + ConfigKeyCodeTexts.SC31_RECEIVETYPE_TEXT + "中的商家代號)] [txdate=指定資料日期(TODAY, YESTERDAY 或 yyyyMMdd)]");
                        }
                    }
                }
                #endregion

                #region 取得 SC31ReceiveType 系統參數指定的商家代號
                SchoolRTypeEntity[] schools = null;  //實際要處理的商家代號資料陣列
                if (exitCode == 0)
                {
                    string errmsg = myHelper.GetSC31ReceiveTypes(out schools, receiveType);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -3;
                        exitMsg = errmsg;
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion

                #region 處理 Config 參數
                #region 暫存檔路徑 (tempPath)
                string tempPath = null;
                if (exitCode == 0)
                {
                    tempPath = ConfigurationManager.AppSettings.Get("TEMP_PATH");
                    if (String.IsNullOrWhiteSpace(tempPath))
                    {
                        exitCode = -2;
                        exitMsg = "Config 參數錯誤，未設定暫存檔路徑 (TEMP_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                    else
                    {
                        tempPath = tempPath.Trim();
                        string errmsg = myHelper.CheckThenCreateFolder(tempPath);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            exitCode = -2;
                            exitMsg = String.Format("處理 Config 的暫存檔路徑 (TEMP_PATH) 參數 ({0}) 失敗，{1}", tempPath, errmsg);
                            jobLog.AppendLine(exitMsg);
                            fileLog.AppendLogLine(exitMsg);
                        }
                    }
                }
                #endregion

                #region 資料檔路徑 (dataPath)
                string dataPath = null;
                if (exitCode == 0)
                {
                    dataPath = ConfigurationManager.AppSettings.Get("DATA_PATH");
                    if (String.IsNullOrWhiteSpace(dataPath))
                    {
                        exitCode = -2;
                        exitMsg = "Config 參數錯誤，未設定資料檔路徑 (DATA_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                    else
                    {
                        dataPath = dataPath.Trim();
                        string errmsg = myHelper.CheckThenCreateFolder(dataPath);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            exitCode = -2;
                            exitMsg = String.Format("處理 Config 的資料檔路徑 (DATA_PATH) 參數 ({0}) 失敗，{1}", dataPath, errmsg);
                            jobLog.AppendLine(exitMsg);
                            fileLog.AppendLogLine(exitMsg);
                        }
                    }
                }
                #endregion

                #region 備份檔路徑 (bakPath)
                string bakPath = null;
                if (exitCode == 0)
                {
                    bakPath = ConfigurationManager.AppSettings.Get("BAK_PATH");
                    if (String.IsNullOrWhiteSpace(bakPath))
                    {
                        exitCode = -2;
                        exitMsg = "Config 參數錯誤，未設定備份檔路徑 (BAK_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                    else
                    {
                        bakPath = bakPath.Trim();
                        string errmsg = myHelper.CheckThenCreateFolder(bakPath);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            exitCode = -2;
                            exitMsg = String.Format("處理 Config 的備份檔路徑 (BAK_PATH) 參數 ({0}) 失敗，{1}", bakPath, errmsg);
                            jobLog.AppendLine(exitMsg);
                            fileLog.AppendLogLine(exitMsg);
                        }
                    }
                }
                #endregion

                #region Web 端檔案服務的指示檔名稱
                string configFileName = null;
                if (exitCode == 0)
                {
                    configFileName = ConfigurationManager.AppSettings.Get("config_file_name");
                    if (String.IsNullOrWhiteSpace(configFileName))
                    {
                        exitCode = -2;
                        exitMsg = "Config 參數錯誤，未設定 Web 端檔案服務的指示檔名稱 (config_file_name) 參數";
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion

                #region Web 端檔案服務的系統 ID
                string apId = null;
                if (exitCode == 0)
                {
                    apId = ConfigurationManager.AppSettings.Get("ap_id");
                    if (String.IsNullOrWhiteSpace(apId))
                    {
                        exitCode = -2;
                        exitMsg = "Config 參數錯誤，未設定 Web 端檔案服務連線帳號參數";
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion

                #region Web 端檔案服務的系統 CODE
                string apCode = null;
                if (exitCode == 0)
                {
                    apCode = ConfigurationManager.AppSettings.Get("ap_code");
                    if (String.IsNullOrWhiteSpace(apCode))
                    {
                        exitCode = -2;
                        exitMsg = "Config 參數錯誤，未設定 Web 端檔案服務連線代碼參數";
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion
                #endregion

                #region 本次產檔工作路徑 (workPath)
                string workPath = null;
                if (exitCode == 0)
                {
                    workPath = Path.Combine(tempPath, startTime.ToString("yyyyMMddHHmm"));
                    string errmsg = myHelper.CheckThenCreateFolder(workPath, true);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -2;
                        exitMsg = String.Format("處理本次產檔工作路徑 ({0}) 失敗，{1}", workPath, errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion

                #region 本次發送資料檔路徑 (sendPath)
                string sendPath = null;
                if (exitCode == 0)
                {
                    sendPath = Path.Combine(dataPath, startTime.ToString("yyyyMMddHHmm"));
                    string errmsg = myHelper.CheckThenCreateFolder(sendPath, true);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -2;
                        exitMsg = String.Format("處理本次發送資料檔路徑 ({0}) 失敗，{1}", sendPath, errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion

                #region 本次原始資料檔路徑 (srcPath)
                string srcPath = null;
                if (exitCode == 0)
                {
                    srcPath = Path.Combine(bakPath, txDate.Value.ToString("yyyyMMdd"));
                    string errmsg = myHelper.CheckThenCreateFolder(srcPath);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -2;
                        exitMsg = String.Format("處理本次原始資料檔路徑 ({0}) 失敗，{1}", srcPath, errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion

                #region 已發送ZIP檔備份路徑
                string zipBakPath = null;
                if (exitCode == 0)
                {
                    zipBakPath = Path.Combine(bakPath, "ZIP");
                    string errmsg = myHelper.CheckThenCreateFolder(zipBakPath);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        exitCode = -2;
                        exitMsg = String.Format("處理已發送ZIP檔備份路徑 ({0}) 失敗，{1}", zipBakPath, errmsg);
                        jobLog.AppendLine(exitMsg);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion

                #region 新增處理中的 Job
                if (exitCode == 0)
                {
                    JobcubeEntity job = new JobcubeEntity();
                    job.Jtypeid = jobTypeId;
                    job.Jparam = argsLine;
                    Result result = jobHelper.InsertProcessJob(ref job, jobCheckMode);
                    if (result.IsSuccess)
                    {
                        jobNo = job.Jno;
                        jobStamp = job.Memo;
                    }
                    else
                    {
                        exitCode = -4;
                        exitMsg = String.Format("新增處理中的 Job 失敗，錯誤訊息：{0}", result.Message);
                        fileLog.AppendLogLine(exitMsg);
                    }
                }
                #endregion

                #region 處理資料
                if (exitCode == 0)
                {
                    string mutexName = mutexName = "Global\\" + txDate.Value.ToString("yyyyMMdd") + "_" + appGuid;
                    using (Mutex m = new Mutex(false, mutexName))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            if (exitCode == 0)
                            {
                                #region D00I70資料下載
                                string d70Result = null;
                                if (action.HasFlag(ActionEnum.D70))
                                {
                                    string actionName = "D00I70資料下載";
                                    fileLog.AppendLogFormatStartLine("{0} 開始", actionName);

                                    try
                                    {
                                        string txDay = Common.GetTWDate8(txDate.Value);

                                        #region [MDY:20181007] 使用新的 EAIHelper (一律取自專案組態的參數)
                                        #region [OLD]
                                        //EAIHelper eaiHelper = new EAIHelper(true);  //土銀改成共用 config 後，EAI 參數一定要取自 Project 的設定
                                        #endregion

                                        EAIHelper eaiHelper = new EAIHelper();
                                        #endregion

                                        if (!eaiHelper.IsEAIArgsReady())
                                        {
                                            d70Result = String.Format("{0} 失敗，請查看日誌", actionName);
                                            exitCode = -6;
                                            exitMsg = "EAI 的執行參數未設定好";
                                            jobLog.AppendLine(exitMsg);
                                            fileLog.AppendLogLine(exitMsg);
                                        }

                                        if (exitCode == 0)
                                        {
                                            CancelHelper cancelHelper = new CancelHelper();
                                            int failSchoolCount = 0;
                                            foreach (SchoolRTypeEntity school in schools)
                                            {
                                                #region 分批取回資料，跑迴圈直到無資料或發生錯誤
                                                bool isSchoolOK = true;
                                                bool isBreak = false;
                                                int count = 0;
                                                do
                                                {
                                                    count++;
                                                    int nextTxSeq = 0;
                                                    string errmsg = myHelper.GetEAINextTxSeq(school.ReceiveType, txDay, out nextTxSeq);
                                                    if (String.IsNullOrEmpty(errmsg))
                                                    {
                                                        #region 發送電文
                                                        {
                                                            CancelDebtsEntity[] datas = null;
                                                            string appNo = school.ReceiveType;
                                                            int sTxSeq = nextTxSeq;
                                                            int eTxSeq = sTxSeq + 500;  //一次最多處理500筆，避免電文太大或處理時間過長，導致下一次啟動時重複抓資料
                                                            bool isOK = eaiHelper.SendD00I70(appNo, txDay, sTxSeq, eTxSeq, out datas);
                                                            fileLog.AppendLogFormatLine("SendD00I70 Log : {0}", eaiHelper.Log);
                                                            if (isOK)
                                                            {
                                                                #region 寫入 CancelDebtsEntity
                                                                if (datas == null || datas.Length == 0)
                                                                {
                                                                    isBreak = true;
                                                                    string msg = String.Format("發送 D00I70 (appNo={0}; txDay={1}; sTxSeq={2}; eTxSeq={3}) 電文成功，但無資料", appNo, txDay, sTxSeq, eTxSeq);
                                                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, msg).AppendLine();
                                                                    fileLog.AppendLogStartLine(msg);
                                                                }
                                                                else
                                                                {
                                                                    int insertCount = 0;
                                                                    using (EntityFactory factory = new EntityFactory())
                                                                    {
                                                                        Result result = null;
                                                                        foreach (CancelDebtsEntity data in datas)
                                                                        {
                                                                            result = cancelHelper.CheckThenInsertCancelDebts(factory, data);
                                                                            if (result.IsSuccess)
                                                                            {
                                                                                insertCount++;
                                                                            }
                                                                            else
                                                                            {
                                                                                isSchoolOK = false;
                                                                                fileLog.AppendLogLine(result.Message);
                                                                            }
                                                                        }
                                                                    }
                                                                    string msg = String.Format("發送 D00I70 (appNo={0}; txDay={1}; sTxSeq={2}; eTxSeq={3}) 電文成功，取得 {4} 筆資料，新增 {5} 筆資料", appNo, txDay, sTxSeq, eTxSeq, datas.Length, insertCount);

                                                                    #region [MDY:20180414] 發送成功訊息也要寫入job日誌
                                                                    jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, msg).AppendLine();
                                                                    #endregion

                                                                    fileLog.AppendLogFormatLine(msg);
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                isSchoolOK = false;
                                                                isBreak = true;
                                                                errmsg = String.Format("發送 D00I70 (appNo={0}; txDay={1}; sTxSeq={2}; eTxSeq={3}) 電文失敗，錯誤訊息：{4}", appNo, txDay, sTxSeq, eTxSeq, eaiHelper.ErrMsg);
                                                                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, errmsg).AppendLine();
                                                                fileLog.AppendLogStartLine(errmsg);
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        isSchoolOK = false;
                                                        isBreak = true;
                                                        errmsg = String.Format("查詢 {0} 在 {1} 的下一個交易序號失敗，{2}", school.ReceiveType, txDay, errmsg);
                                                        jobLog.AppendLine(errmsg);
                                                        fileLog.AppendLogLine(errmsg);
                                                    }
                                                }
                                                while (!isBreak && count < 2000);  //每次500筆所以不可能超過2000次

                                                if (!isSchoolOK)
                                                {
                                                    failSchoolCount++;
                                                }
                                                #endregion
                                            }
                                            d70Result = String.Format("{0}：共 {1} 個商家代號，失敗 {2} 個", actionName, schools.Length, failSchoolCount);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        d70Result = String.Format("{0} 發生例外，請查看日誌", actionName);
                                        string errmsg = String.Format("{0} 發生例外，{1}", actionName, ex.Message);
                                        jobLog.AppendLine(errmsg);
                                        fileLog.AppendLogLine(errmsg);
                                    }

                                    fileLog.AppendLogFormatStartLine("{0} 結束", actionName);
                                }
                                #endregion

                                #region 產生學校銷帳資料檔
                                string genResult = null;
                                FileInfo genZipFile = null;  //紀錄本次產生的壓縮檔，以便 SEND 發送
                                if (action.HasFlag(ActionEnum.GEN))
                                {
                                    string actionName = "產生學校銷帳資料檔";
                                    fileLog.AppendLogFormatStartLine("{0} 開始", actionName);

                                    try
                                    {
                                        bool isGenOK = false;

                                        #region 產生所有商家代號的銷帳資料檔
                                        StringBuilder allConfigFileContent = new StringBuilder();  //所有上傳學校銷帳資料檔寫成一個指示檔
                                        List<string> allFileFullNames = new List<string>();
                                        {
                                            CancelHelper cancelHelper = new CancelHelper();
                                            DateTime chkTime = DateTime.Now;
                                            foreach (SchoolRTypeEntity school in schools)
                                            {
                                                //讀取資料時應該就濾掉了，這裡只是 double check 避免全空白的資料
                                                if (String.IsNullOrWhiteSpace(school.FtpAccount) || String.IsNullOrWhiteSpace(school.FtpLocation))
                                                {
                                                    fileLog.AppendDebugFormatLine("{0} 的 FTP 伺服器或帳號設定為全空白字串，忽略不處理", school.ReceiveType);
                                                    continue;
                                                }

                                                chkTime = DateTime.Now;

                                                #region 每一個商家代號一個檔，對應一個 key 檔
                                                int dataCount = 0;
                                                string keyFileFullName = null;
                                                string txtFileFullName = null;
                                                string configFileContent = null;
                                                string warning = null;
                                                string errmsg = cancelHelper.GenSchoolCancelData(school, txDate.Value, workPath, srcPath, out dataCount, out keyFileFullName, out txtFileFullName, out configFileContent, out warning);
                                                if (String.IsNullOrEmpty(errmsg))
                                                {
                                                    allConfigFileContent.Append(configFileContent);
                                                    allFileFullNames.Add(keyFileFullName);
                                                    allFileFullNames.Add(txtFileFullName);
                                                    if (!String.IsNullOrEmpty(warning))
                                                    {
                                                        fileLog.AppendDebugFormatLine("{0} 產生學校銷帳檔相關檔案成功，但 {1}", school.ReceiveType, warning);
                                                    }

                                                    #region Debug Info
                                                    if (dataCount == 0)
                                                    {
                                                        fileLog.AppendDebugFormatLine("{0} 無銷帳資料", school.ReceiveType);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    errmsg = String.Format("產生 {0} 學校銷帳資料檔失敗，{1}", school.ReceiveType, errmsg);
                                                    jobLog.AppendLine(errmsg);
                                                    fileLog.AppendLogLine(errmsg);
                                                }
                                                #endregion

                                                #region 迴圈間隔小於1秒鐘，就睡1秒鐘，以避免檔名重複
                                                if ((DateTime.Now - chkTime).TotalMilliseconds < 1000)
                                                {
                                                    Thread.Sleep(1000 * 1);
                                                }
                                                #endregion
                                            }

                                            if (allFileFullNames.Count == 0)
                                            {
                                                isGenOK = false;
                                                string errmsg = "所有商家代號的學校銷帳資料檔皆產生失敗 (指示檔無資料)";
                                                jobLog.AppendLine(errmsg);
                                                fileLog.AppendLogLine(errmsg);
                                            }
                                            else
                                            {
                                                isGenOK = true;
                                            }
                                        }
                                        #endregion

                                        #region 產生指示檔 (所有上傳學校銷帳資料檔寫成一個指示檔)
                                        if (isGenOK)
                                        {
                                            string configFileFullName = Path.Combine(workPath, configFileName);
                                            allFileFullNames.Add(configFileFullName);
                                            try
                                            {
                                                File.WriteAllText(configFileFullName, allConfigFileContent.ToString(), Encoding.Default);
                                            }
                                            catch (Exception ex)
                                            {
                                                isGenOK = false;
                                                string errmsg = String.Format("產生指示檔 ({0}) 發生例外，錯誤訊息：{1}", configFileFullName, ex.Message);
                                                jobLog.AppendLine(errmsg);
                                                fileLog.AppendLogLine(errmsg);
                                            }
                                        }
                                        #endregion

                                        #region 將檔案移至發送檔路徑
                                        if (isGenOK)
                                        {
                                            string sourceFileFullName = null;
                                            string destFileFullName = null;
                                            try
                                            {
                                                for (int idx = 0; idx < allFileFullNames.Count; idx++)
                                                {
                                                    sourceFileFullName = allFileFullNames[idx];
                                                    destFileFullName = Path.Combine(sendPath, Path.GetFileName(sourceFileFullName));
                                                    File.Move(sourceFileFullName, destFileFullName);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                isGenOK = false;
                                                string errmsg = String.Format("移動檔案 ({0}) 至 {1} 發生例外，錯誤訊息：{2}", sourceFileFullName, destFileFullName, ex.Message);
                                                jobLog.AppendLine(errmsg);
                                                fileLog.AppendLogLine(errmsg);
                                            }
                                        }
                                        #endregion

                                        #region 刪除搬空的本次產檔工作路徑 (資料夾)
                                        if (isGenOK)
                                        {
                                            try
                                            {
                                                string[] files = Directory.GetFiles(workPath, "*.*", SearchOption.TopDirectoryOnly);
                                                if (files == null || files.Length == 0)
                                                {
                                                    Directory.Delete(workPath);
                                                }
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                        #endregion

                                        #region 將發送檔資料夾壓縮成 ZIP
                                        if (isGenOK)
                                        {
                                            string zipFileName = string.Format("canceldata.{0:yyyyMMddHHmmss}.zip", startTime);
                                            string zipFileFullName = Path.Combine(dataPath, zipFileName);
                                            try
                                            {
                                                ZIPHelper.ZipDir(sendPath, zipFileFullName);
                                                genZipFile = new FileInfo(zipFileFullName);
                                            }
                                            catch (Exception ex)
                                            {
                                                isGenOK = false;
                                                string errmsg = String.Format("壓縮資料夾 ({0}) 產生壓縮檔 ({1}) 發生例外，錯誤訊息：{2}", sendPath, zipFileFullName, ex.Message);
                                                jobLog.AppendLine(errmsg);
                                                fileLog.AppendLogLine(errmsg);
                                            }
                                        }
                                        #endregion

                                        #region 刪除搬空的本次發送資料檔路徑 (資料夾)
                                        if (isGenOK)
                                        {
                                            try
                                            {
                                                string[] files = Directory.GetFiles(sendPath, "*.*", SearchOption.TopDirectoryOnly);
                                                if (files == null || files.Length == 0)
                                                {
                                                    Directory.Delete(sendPath);
                                                }
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                        #endregion

                                        if (isGenOK)
                                        {
                                            genResult = String.Format("{0} 成功", actionName);
                                        }
                                        else
                                        {
                                            genResult = String.Format("{0} 失敗，請查看日誌", actionName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        genResult = String.Format("{0} 發生例外，請查看日誌", genResult);
                                        string errmsg = String.Format("{0} 發生例外，{1}", actionName, ex.Message);
                                        jobLog.AppendLine(errmsg);
                                        fileLog.AppendLogLine(errmsg);
                                    }

                                    fileLog.AppendLogFormatStartLine("{0} 結束", actionName);
                                }
                                #endregion

                                #region 發送學校銷帳資料檔
                                string sendResult = null;
                                if (action.HasFlag(ActionEnum.SEND))
                                {
                                    string actionName = "發送學校銷帳資料檔";
                                    fileLog.AppendLogFormatStartLine("{0} 開始", actionName);

                                    try
                                    {
                                        bool isSendOK = true;
                                        List<FileInfo> sendFiles = null;
                                        if (action.HasFlag(ActionEnum.GEN))
                                        {
                                            if (genZipFile == null)
                                            {
                                                isSendOK = false;
                                                string msg = "產生學校銷帳資料檔失敗，本次發送學校銷帳資料檔不處理";
                                                jobLog.AppendLine(msg);
                                                fileLog.AppendLogLine(msg);
                                            }
                                            else
                                            {
                                                sendFiles = new List<FileInfo>(1);
                                                sendFiles.Add(genZipFile);
                                            }
                                        }
                                        else
                                        {
                                            string searchFile = String.Format("canceldata.{0:yyyyMMdd}*.zip", startTime);
                                            DirectoryInfo dInfo = new DirectoryInfo(dataPath);
                                            FileInfo[] some = dInfo.GetFiles(searchFile, SearchOption.TopDirectoryOnly);
                                            if (some != null && some.Length > 0)
                                            {
                                                sendFiles = new List<FileInfo>(some);
                                            }
                                            else
                                            {
                                                isSendOK = false;
                                                string msg = String.Format("找不到 {0} 相符的檔案，無檔案需要發送", searchFile);
                                                jobLog.AppendLine(msg);
                                                fileLog.AppendLogLine(msg);
                                            }
                                        }

                                        #region 傳送
                                        if (sendFiles != null && sendFiles.Count > 0)
                                        {
                                            foreach (FileInfo one in sendFiles)
                                            {
                                                string fileFullName = one.FullName;

                                                #region [MDY:2018xxxx] 服務改呼叫 FTPUpload() 方法
                                                string errmsg = myHelper.SendFile(jobNo, fileFullName, apId, apCode);
                                                if (String.IsNullOrWhiteSpace(errmsg))
                                                {
                                                    string msg = String.Format("傳送檔案 ({0}) 至 Web 端成功", fileFullName);
                                                    jobLog.AppendLine(msg);
                                                    fileLog.AppendLogLine(msg);

                                                    #region 傳送成功搬到 ZIP BAK 裡
                                                    {
                                                        string newFileFullName = Path.Combine(zipBakPath, Path.GetFileName(fileFullName));
                                                        File.Move(fileFullName, newFileFullName);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    isSendOK = false;
                                                    string msg = String.Format("傳送檔案 ({0}) 至 Web 端失敗。{1}", fileFullName, errmsg);
                                                    jobLog.AppendLine(msg);
                                                    fileLog.AppendLogLine(msg);
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                        if (isSendOK)
                                        {
                                            sendResult = String.Format("{0} 成功", actionName);
                                        }
                                        else
                                        {
                                            sendResult = String.Format("{0} 失敗，請查看日誌", actionName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sendResult = String.Format("{0} 發生例外，請查看日誌", actionName);
                                        string errmsg = String.Format("{0} 發生例外，{1}", actionName, ex.Message);
                                        jobLog.AppendLine(errmsg);
                                        fileLog.AppendLogLine(errmsg);
                                    }

                                    fileLog.AppendLogFormatStartLine("{0} 結束", actionName);
                                }
                                #endregion

                                if (exitCode == 0)
                                {
                                    exitMsg = String.Format("{0}；{1}；{2}；", d70Result, genResult, sendResult);
                                }
                            }

                            #region [MDY:20210401] 原碼修正
                            m.ReleaseMutex();
                            #endregion
                        }
                        else
                        {
                            exitCode = -5;
                            exitMsg = String.Format("執行緒中已存在 {0}，不重複執行", mutexName);
                            jobLog.AppendLine(exitMsg);
                            fileLog.AppendLogLine(exitMsg);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                fileLog.AppendLogFormatStartLine("發生例外，命令參數：{0}，錯誤訊息：{1}", argsLine, ex.Message);
            }
            finally
            {
                #region 更新 Job
                if (jobNo > 0)
                {
                    string jobResultId = null;
                    if (exitCode == 0)
                    {
                        jobResultId = JobCubeResultCodeTexts.SUCCESS;
                    }
                    else
                    {
                        jobResultId = JobCubeResultCodeTexts.FAILURE;
                    }

                    Result result = jobHelper.UpdateProcessJobToFinsh(jobNo, jobStamp, jobResultId, exitMsg, jobLog.ToString());
                    if (!result.IsSuccess)
                    {
                        fileLog.AppendLogFormatStartLine("更新批次處理佇列為已完成失敗，{0}", result.Message);
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                fileLog.AppendLogFormatStartLine("{0} 結束", appName);
                fileLog.Flush();

                System.Environment.Exit(exitCode);
            }
        }
    }
}
