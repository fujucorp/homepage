using System;
using System.IO;

using Fuju;
using Fuju.DB.Data;
using Fuju.Web;
using Fuju.Web.Services;

using Entities;

namespace Helpers
{
    /// <summary>
    /// 資料處理服務執行器類別
    /// </summary>
    public class DataServiceExecutor : ServiceExecutor
    {
        #region Property
        private string _TempPath = null;
        /// <summary>
        /// 設定或取得檔案暫存路徑
        /// </summary>
        public string TempPath
        {
            get
            {
                return _TempPath;
            }
            set
            {
                _TempPath = value == null ? null : value.Trim();
                if (String.IsNullOrEmpty(_TempPath))
                {
                    _TempPath = Path.GetTempPath();
                }
                else
                {
                    try
                    {
                        if (!Directory.Exists(_TempPath))
                        {
                            Directory.CreateDirectory(_TempPath);
                        }
                    }
                    catch (Exception)
                    {
                        _TempPath = Path.GetTempPath();
                    }
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構資料處理服務類別物件，並使用預設的資料庫連線設定
        /// </summary>
        public DataServiceExecutor()
        {
            _TempPath = Path.GetTempPath();
        }
        #endregion

        #region Implement ServiceExecutor's Abstract Method
        /// <summary>
        /// 取得是否為定義的服務命令名稱 (繼承者如有新的服務命令，必須 Override 此方法)
        /// </summary>
        /// <param name="commandName">指定要檢查的服務命令名稱。</param>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public override bool IsDefinedCommandName(string commandName)
        {
            //[TODO] 提供新的 Command 時，要在這裡加一個命令名稱判斷
            if (InsertCommand.COMMAND_NAME == commandName
                || InsertAllCommand.COMMAND_NAME == commandName
                || UpdateCommand.COMMAND_NAME == commandName
                || UpdateFieldsCommand.COMMAND_NAME == commandName
                || DeleteCommand.COMMAND_NAME == commandName
                || DeleteAllCommand.COMMAND_NAME == commandName
                || SelectCommand.COMMAND_NAME == commandName
                || SelectFirstCommand.COMMAND_NAME == commandName
                || SelectAllCommand.COMMAND_NAME == commandName
                || SelectCountCommand.COMMAND_NAME == commandName

                || BankLogonCommand.COMMAND_NAME == commandName
                || UserLogonCommand.COMMAND_NAME == commandName
                || StudentLogonCommand.COMMAND_NAME == commandName

                #region [Old] 行員 SSO 登入 (土銀沒有)
                //|| SSOLogonCommand.COMMAND_NAME == commandName
                #endregion

                || CheckLogonCommand.COMMAND_NAME == commandName
                || UserLogoutCommand.COMMAND_NAME == commandName

                || FilterOptionCommand.COMMAND_NAME == commandName
                || EntityOptionsCommand.COMMAND_NAME == commandName

                || CallMethodCommand.COMMAND_NAME == commandName

                || Entities.BankService.BankServiceCommand.COMMAND_NAME == commandName

                #region [MDY:20160305] 連動製單服務 相關
                || SchoolServiceCommand.COMMAND_NAME == commandName
                #endregion
                )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 執行資料處理服務命令 (繼承者如有新的服務命令，必須 Override 此方法)
        /// </summary>
        /// <param name="command">指定要執行的資料處理服務命令。</param>
        /// <param name="asker">指定執行命令的服務請求者。</param>
        /// <param name="data">傳回命令執行結果的回傳資料。</param>
        /// <param name="log">傳回日誌資料。(預設 null，繼承者自行處理)</param>
        /// <returns>傳回處理結果。</returns>
        protected override Result ExecuteCommand(IServiceCommand command, ServiceAsker asker, out object data, out string log)
        {
            data = null;
            log = null;

            #region 檢查服務命令參數
            if (command == null || command.IsEmpty() || !command.IsReady())
            {
                return new Result("缺少或無效的資料處理服務命令參數", ServiceStatusCode.P_LOST_COMMAND);
            }
            #endregion

            #region 新增資料
            if (InsertCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                int count = 0;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.Insert(command, out commandAsker, out count);
                data = count;
                return result;
            }
            #endregion

            #region 新增多筆資料
            if (InsertAllCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                int count = 0;
                int[] failIndexs = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.InsertAll(command, out commandAsker, out count, out failIndexs);
                data = new object[] { count, failIndexs };
                return result;
            }
            #endregion

            #region 更新資料
            if (UpdateCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                int count = 0;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.Update(command, out commandAsker, out count);
                data = count;
                return result;
            }
            #endregion

            #region 更新指定欄位值
            if (UpdateFieldsCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                int count = 0;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.UpdateFields(command, out commandAsker, out count);
                data = count;
                return result;
            }
            #endregion

            #region 刪除資料
            if (DeleteCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                int count = 0;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.Delete(command, out commandAsker, out count);
                data = count;
                return result;
            }
            #endregion

            #region 刪除多筆資料
            if (DeleteAllCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                int count = 0;
                int[] failIndexs = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.DeleteAll(command, out commandAsker, out count, out failIndexs);
                data = new object[] { count, failIndexs };
                return result;
            }
            #endregion

            #region 查詢資料
            if (SelectCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                Entity[] instances = null;
                int tiotalCount = 0;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.Select(command, out commandAsker, out instances, out tiotalCount);
                object[] data2 = new object[2];
                data2[0] = instances;
                data2[1] = tiotalCount;
                data = data2;
                return result;
            }
            #endregion

            #region 查詢首筆資料
            if (SelectFirstCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                Entity instance = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.SelectFirst(command, out commandAsker, out instance);
                data = instance;
                return result;
            }
            #endregion

            #region 查詢所有資料
            if (SelectAllCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                Entity[] instances = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.SelectAll(command, out commandAsker, out instances);
                data = instances;
                return result;
            }
            #endregion

            #region 查詢資料筆數
            if (SelectCountCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                int count = 0;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.SelectCount(command, out commandAsker, out count);
                data = count;
                return result;
            }
            #endregion

            #region 行員登入
            if (BankLogonCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                LogonUser logonUser = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.BankLogon(command, out commandAsker, out logonUser);
                data = logonUser;
                return result;
            }
            #endregion

            #region 學校登入
            if (UserLogonCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                LogonUser logonUser = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.SchoolLogon(command, out commandAsker, out logonUser);
                data = logonUser;
                return result;
            }
            #endregion

            #region 學生登入
            if (StudentLogonCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                LogonUser logonUser = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.StudentLogon(command, out commandAsker, out logonUser);
                data = logonUser;
                return result;
            }
            #endregion

            #region [Old] 行員 SSO 登入 (土銀沒有)
            //if (SSOLogonCommand.COMMAND_NAME == command.CommandName)
            //{
            //    CommandAsker commandAsker = null;
            //    LogonUser logonUser = null;
            //    DataServiceHelper helper = new DataServiceHelper();
            //    Result result = helper.SSOLogon(command, out commandAsker, out logonUser);
            //    data = logonUser;
            //    return result;
            //}
            #endregion

            #region 檢查登入與功能狀態
            if (CheckLogonCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                string resultCode = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.CheckLogon(command, out commandAsker, out resultCode);
                data = resultCode;
                return result;
            }
            #endregion

            #region 使用者登出
            if (UserLogoutCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.UserLogout(command, out commandAsker);
                return result;
            }
            #endregion

            #region 取得主要條件選項資料
            if (FilterOptionCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                FilterOption option = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.GetFilterOption(command, out commandAsker, out option);
                data = option;
                return result;
            }
            #endregion

            #region 取得資料選項陣列
            if (EntityOptionsCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                CodeText[] datas = null;
                DataServiceHelper helper = new DataServiceHelper();
                Result result = helper.GetEntityOptions(command, out commandAsker, out datas);
                data = datas;
                return result;
            }
            #endregion

            #region 呼叫後台服務方法
            if (CallMethodCommand.COMMAND_NAME == command.CommandName)
            {
                CommandAsker commandAsker = null;
                object returnData = null;
                DataServiceHelper helper = new DataServiceHelper(this.TempPath);
                Result result = helper.CallMethod(command, out commandAsker, out returnData);
                data = returnData;
                return result;
            }
            #endregion

            #region 呼叫後台即查繳服務方法
            if (Entities.BankService.BankServiceCommand.COMMAND_NAME == command.CommandName)
            {
                string rsXml = null;
                Entities.BankService.BankServiceHelper helper = new Entities.BankService.BankServiceHelper();
                Result result = helper.ExecuteCommand(command, out rsXml);
                data = rsXml;
                return result;
            }
            #endregion

            #region [MDY:20160305] 呼叫後台連動製單服務方法
            if (SchoolServiceCommand.COMMAND_NAME == command.CommandName)
            {
                Entities.SchoolService.ReturnData returnData = null;
                Entities.SchoolService.SchoolServiceHelper helper = new Entities.SchoolService.SchoolServiceHelper();
                helper.ExecuteCommand(command, out returnData);
                data = returnData;
                return new Result(true);
            }
            #endregion

            //[TODO] 提供新的 Command 時，要在這裡加一個該命令要執行的方法或程式碼

            return new Result("未實作該資料處理服務命令", ServiceStatusCode.P_LOST_COMMAND);
        }
        #endregion

        #region Override ServiceExecutor's Method
        /// <summary>
        /// 嘗試取得 指定服務命令序列化 Xml 的 服務命令物件，並檢查是否為已定義的命令名稱
        /// </summary>
        /// <param name="commandXml">指定要反序列的服務命令序列化 Xml 字串。</param>
        /// <param name="command">成功則服務命令物件，否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        public override XmlResult TryGetCommand(string commandXml, out IServiceCommand command)
        {
            command = null;
            if (Common.IsNullOrSpace(commandXml))
            {
                return new XmlResult(false, "缺少服務命令參數", ServiceStatusCode.P_LOST_COMMAND);
            }

            //為了保證資料處理命令可以正確反序列化，以 ServiceCommand 作為傳遞命令的類別
            command = ServiceCommand.CreateByCommandXml(commandXml);
            if (command == null)
            {
                return new XmlResult(false, "無法反序列化服務命令參數", ServiceStatusCode.S_SERVICE_DESERIALIZED_FAILURE);
            }

            if (!this.IsDefinedCommandName(command.CommandName))
            {
                return new XmlResult(false, "無效的服務命令參數", ServiceStatusCode.P_LOST_COMMAND);
            }

            return new XmlResult(true);
        }
        #endregion
    }
}
