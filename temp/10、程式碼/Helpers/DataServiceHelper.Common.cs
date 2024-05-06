using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web.Services;

using Entities;

namespace Helpers
{
    public partial class DataServiceHelper
    {
        /// <summary>
        /// 取得服務命令請求者的使用者資料
        /// </summary>
        /// <param name="dbLogger">指定資料庫日誌記錄器。</param>
        /// <param name="asker">指定服務命令請求者物件。</param>
        /// <param name="user">找到則傳回 UsersEntity 否則傳回 null。</param>
        /// <returns>傳回處理結果。</returns>
        private Result GetUserByCommandAsker(DBLogger dbLogger, CommandAsker asker, out UsersEntity user)
        {
            user = null;
            if (asker == null || !UserQualCodeTexts.IsDefine(asker.UserQual) || UserQualCodeTexts.IsStudentUser(asker.UserQual))
            {
                return new Result(false, "無效的請求者", ErrorCode.S_NO_AUTHORIZE, null);
            }

            //TODO
            if (asker.IsBankUser && BankADGroupCodeTexts.IsInAD(asker.GroupId))
            {
                user = new UsersEntity();
                user.UId = asker.UserId;
                user.UGrp = asker.GroupId;
                user.URt = String.Empty;
                user.UBank = asker.UnitId;
                return new Result(true);
            }

            //[MEMO] 雖然資料表的 PK 是 UserId + GroupId + ReceiveType + BankId，但 ReceiveType, BankId 同時只有一個有值
            Expression where = new Expression(UsersEntity.Field.UId, asker.UserId)
                .And(UsersEntity.Field.UGrp, asker.GroupId);
            switch (asker.UserQual)
            {
                case UserQualCodeTexts.BANK:
                    where.And(UsersEntity.Field.UBank, asker.UnitId);
                    break;
                case UserQualCodeTexts.SCHOOL:
                    where.And(UsersEntity.Field.UBank, asker.UnitId);
                    break;
            }

            Result result = _Factory.SelectFirst<UsersEntity>(where, null, out user);
            if (result.IsSuccess && user == null)
            {
                result = new Result(false, "查無請求者", ErrorCode.D_DATA_NOT_FOUND, null);
            }

            #region 新增日誌資料
            if (dbLogger != null)
            {
                string notation = null;
                if (result.IsSuccess)
                {
                    notation = String.Format("[{0}] {1}請求者的使用者資料成功 (userId={2}, groupId={3}, receiveType={4}, bankId={5})", asker.FuncName, LogTypeCodeTexts.SELECT_TEXT, user.URt, user.UGrp, user.URt, user.UBank);
                }
                else
                {
                    notation = String.Format("[{0}] {1}請求者的使用者資料失敗 (錯誤訊息：{2})", asker.FuncName, LogTypeCodeTexts.SELECT_TEXT, result.Message);
                }
                dbLogger.AppendLog(asker.UserQual, asker.UnitId, asker.FuncId, LogTypeCodeTexts.SELECT, asker.UserId, notation);
            }
            #endregion

            return result;
        }


        #region Log 相關
        private string _logPath = null;
        /// <summary>
        /// 取得 Log 檔完整路徑檔名
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string GetLogFileName(string methodName)
        {
            if (_logPath == null)
            {
                _logPath = ConfigurationManager.AppSettings.Get("log_path");
                if (_logPath == null)
                {
                    _logPath = String.Empty;
                }
                else
                {
                    _logPath = _logPath.Trim();
                }
                if (!String.IsNullOrEmpty(_logPath))
                {
                    try
                    {
                        if (!Directory.Exists(_logPath))
                        {
                            Directory.CreateDirectory(_logPath);
                        }
                    }
                    catch (Exception)
                    {
                        _logPath = String.Empty;
                    }
                }
            }

            if (String.IsNullOrEmpty(_logPath))
            {
                return null;
            }
            else
            {
                return Path.Combine(_logPath, String.Format("{0}_{1:yyyyMMdd}.log", methodName, DateTime.Today));
            }
        }

        /// <summary>
        /// 寫 Log
        /// </summary>
        /// <param name="methodName">方法名稱</param>
        /// <param name="msg">訊息</param>
        private void WriteLog(string methodName, string msg)
        {
            if (String.IsNullOrEmpty(methodName) || String.IsNullOrEmpty(msg))
            {
                return;
            }
            string logFileName = this.GetLogFileName(methodName);
            if (String.IsNullOrEmpty(logFileName))
            {
                return;
            }

            StringBuilder log = new StringBuilder();
            log
                .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, methodName).AppendLine()
                .AppendLine(msg)
                .AppendLine();

            this.WriteLogFile(logFileName, log.ToString());
        }

        private void WriteLog(string methodName, string format, params object[] args)
        {
            if (String.IsNullOrEmpty(methodName) || String.IsNullOrEmpty(format))
            {
                return;
            }
            string logFileName = this.GetLogFileName(methodName);
            if (String.IsNullOrEmpty(logFileName))
            {
                return;
            }

            StringBuilder log = new StringBuilder();
            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, methodName).AppendLine();
            try
            {
                if (args == null || args.Length == 0)
                {
                    log.AppendLine(format);
                }
                else
                {
                    log.AppendFormat(format, args).AppendLine();
                }
            }
            catch (Exception)
            {

            }
            log.AppendLine();

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
            }
        }
        #endregion
    }
}
