using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 流水號取號工具類別
    /// </summary>
    public sealed class SNoHelper
    {
        #region Static Readonly
        /// <summary>
        /// 定義最大的流水號值
        /// </summary>
        public static readonly Int64 MaxValue = 99999999999L;
        #endregion

        #region Constructor
        /// <summary>
        /// 建構流水號取號工具類別
        /// </summary>
        public SNoHelper()
        {

        }
        #endregion

        #region Method
        /// <summary>
        /// 取得指定流水號代碼的下一個流水號
        /// </summary>
        /// <param name="factory">指定資料存取物件</param>
        /// <param name="snoKey">指定流水號代碼</param>
        /// <param name="maxValue">指定流水號最大值，最大 99999999999，超過取 99999999999</param>
        /// <param name="isRecycle">指定流水號是否可循環</param>
        /// <returns>成功則傳回大於 0 的值，否則傳回 0</returns>
        public Int64 GetNextSNo(EntityFactory factory, string snoKey, Int64 maxValue, bool isRecycle)
        {
            if (factory != null && factory.IsReady() && !String.IsNullOrWhiteSpace(snoKey))
            {
                string sql = @"DECLARE @NextSNo bigint;
EXEC @NextSNo = usp_GetNextSNo @Q_SN_KEY, @P_MAX_VALUE, @P_RECYCLE_FLAG;
SELECT @NextSNo";
                KeyValue[] parameters = new KeyValue[3];
                parameters[0] = new KeyValue("@Q_SN_KEY", snoKey);
                parameters[1] = new KeyValue("@P_MAX_VALUE", maxValue > MaxValue ? MaxValue : maxValue);
                parameters[2] = new KeyValue("@P_RECYCLE_FLAG", isRecycle ? "Y" : "N");

                object value = null;
                Result result = factory.ExecuteScalar(sql, parameters, out value);
                if (result.IsSuccess && value != null)
                {
                    Int64 nextSNo = 0;
                    if (Int64.TryParse(value.ToString(), out nextSNo))
                    {
                        return nextSNo;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 取得指定流水號代碼的下一個流水號，最大 999999999 且不循環
        /// </summary>
        /// <param name="factory">指定資料存取物件</param>
        /// <param name="snoKey">指定流水號代碼</param>
        /// <returns>成功則傳回大於 0 的值，否則傳回 0</returns>
        public Int64 GetNextSNo(EntityFactory factory, string snoKey)
        {
            return this.GetNextSNo(factory, snoKey, MaxValue, false);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 取得 StudentReceive 的 SeriorNo 欄位用的流水號 Key
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <returns></returns>
        public string GetKeyForStudentReceiveSeriorNo(string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            if (String.IsNullOrWhiteSpace(receiveType) 
                || String.IsNullOrWhiteSpace(yearId)
                || String.IsNullOrWhiteSpace(termId)
                //|| String.IsNullOrWhiteSpace(depId)
                || String.IsNullOrWhiteSpace(receiveId))
            {
                return null;
            }
            return String.Format("StudentReceive.SeriorNo:{0}_{1}_{2}_{3}_{4}", receiveType.Trim(), yearId.Trim(), termId.Trim(), depId.Trim(), receiveId.Trim());
        }
        #endregion
    }
}
