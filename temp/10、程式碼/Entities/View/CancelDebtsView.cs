using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 銷帳(入帳)資料 + 分行名稱 + 代收管道名稱 View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class CancelDebtsView : Entity
    {
        #region [MDY:20160607] 增加更正相關欄位
        #region [Old]
        //        public const string VIEWSQL = @"SELECT [SNo], [Receive_Type], [Cancel_No], [Receive_Amount], [Receive_Date], [Receive_Time], [Receive_Way], [Receive_Bank]
//     , [Account_Date], [Status]
//     , ISNULL((SELECT [BANKSNAME] FROM [" + BankEntity.TABLE_NAME + @"] AS B WHERE B.[BANKNO] = CD.[Receive_Bank]), CD.[Receive_Bank]) AS [Receive_Bank_Name]
//     , ISNULL((SELECT [Channel_Name] FROM [" + ChannelSetEntity.TABLE_NAME + @"] AS C WHERE C.[Channel_Id] = CD.[Receive_Way]), '') AS [Receive_Way_Name]
//  FROM [" + CancelDebtsEntity.TABLE_NAME  + @"] AS CD";
        #endregion

        public const string VIEWSQL = @"SELECT [SNo], [Receive_Type], [Cancel_No], [Receive_Amount], [Receive_Date], [Receive_Time], [Receive_Way], [Receive_Bank]
     , [Account_Date], [Status], [Reserve1], [Reserve2], [Rollback_Date]
     , ISNULL((SELECT [BANKSNAME] FROM [" + BankEntity.TABLE_NAME + @"] AS B WHERE B.[BANKNO] = CD.[Receive_Bank]), CD.[Receive_Bank]) AS [Receive_Bank_Name]
     , ISNULL((SELECT [Channel_Name] FROM [" + ChannelSetEntity.TABLE_NAME + @"] AS C WHERE C.[Channel_Id] = CD.[Receive_Way]), '') AS [Receive_Way_Name]
  FROM [" + CancelDebtsEntity.TABLE_NAME + @"] AS CD";
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// 代收費用設定資料 + 學年、學期、部別、代收費用別名稱 View 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 流水號 (PKey) (Identity)
            /// </summary>
            public const string SNo = "SNo";
            #endregion

            #region Data
            /// <summary>
            /// 商家代號
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 銷帳編號
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// 代收金額
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";

            /// <summary>
            /// 代收日期，異業代收則為行員確認日 (民國年日期7碼)
            /// </summary>
            public const string ReceiveDate = "Receive_Date";

            /// <summary>
            /// 代收時間
            /// </summary>
            public const string ReceiveTime = "Receive_Time";

            /// <summary>
            /// 代收管道
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 代收銀行 (土銀使用6碼)
            /// </summary>
            public const string ReceiveBank = "Receive_Bank";

            /// <summary>
            /// 入帳日期 (民國年日期7碼)
            /// </summary>
            public const string AccountDate = "Account_Date";

            /// <summary>
            /// 狀態 (0=已銷帳; 1=待處理; 2=處理中; 3=銷帳失敗; 4=免銷帳; 5=已更正; 6=被更正)
            /// </summary>
            public const string Status = "Status";

            #region [MDY:20160607] 增加更正相關欄位
            /// <summary>
            /// 土銀用來記錄被匯出成銷帳資料的日期 (格式：YYYYMMDD)
            /// </summary>
            public const string Reserve1 = "Reserve1";

            /// <summary>
            /// 土銀用來記錄更正記號 (0=正常交易; 1=更正交易;，請參考 D00I70ECMarkCodeTexts 定義)
            /// </summary>
            public const string Reserve2 = "Reserve2";

            /// <summary>
            /// 被更正的日期時間
            /// </summary>
            public const string RollbackDate = "Rollback_Date";
            #endregion
            #endregion

            #region 代碼名稱
            /// <summary>
            /// 代收銀行名稱
            /// </summary>
            public const string ReceiveBankName = "Receive_Bank_Name";

            /// <summary>
            /// 代收管道名稱
            /// </summary>
            public const string ReceiveWayName = "Receive_Way_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// CancelDebtsView 類別建構式
        /// </summary>
        public CancelDebtsView()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        /// <summary>
        /// 流水號 (PKey) (Identity)
        /// </summary>
        [FieldSpec(Field.SNo, true, FieldTypeEnum.Identity, false)]
        public Int64 SNo
        {
            get;
            set;
        }
        #endregion

        #region Data
        /// <summary>
        /// 商家代號
        /// </summary>
        [FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, false)]
        public string ReceiveType
        {
            get;
            set;
        }

        /// <summary>
        /// 銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelNo, false, FieldTypeEnum.VarChar, 16, false)]
        public string CancelNo
        {
            get;
            set;
        }

        /// <summary>
        /// 代收金額
        /// </summary>
        [FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal ReceiveAmount
        {
            get;
            set;
        }

        private string _ReceiveDate = null;
        /// <summary>
        /// 代收日期，異業代收則為行員確認日 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.ReceiveDate, false, FieldTypeEnum.Char, 8, false)]
        public string ReceiveDate
        {
            get
            {
                return _ReceiveDate;
            }
            set
            {
                _ReceiveDate = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 代收時間
        /// </summary>
        [FieldSpec(Field.ReceiveTime, false, FieldTypeEnum.VarChar, 6, false)]
        public string ReceiveTime
        {
            get;
            set;
        }

        /// <summary>
        /// 代收管道
        /// </summary>
        [FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 4, false)]
        public string ReceiveWay
        {
            get;
            set;
        }

        /// <summary>
        /// 代收銀行 (土銀使用6碼)
        /// </summary>
        [FieldSpec(Field.ReceiveBank, false, FieldTypeEnum.VarChar, 7, false)]
        public string ReceiveBank
        {
            get;
            set;
        }

        private string _AccountDate = null;
        /// <summary>
        /// 入帳日期 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.AccountDate, false, FieldTypeEnum.Char, 8, false)]
        public string AccountDate
        {
            get
            {
                return _AccountDate;
            }
            set
            {
                _AccountDate = value == null ? null : value.Trim();
            }
        }

        private string _Status = null;
        /// <summary>
        /// 狀態 (0=已銷帳; 1=待處理; 2=處理中; 3=銷帳失敗; 4=免銷帳; 5=已更正; 6=被更正)
        /// </summary>
        [FieldSpec(Field.Status, false, FieldTypeEnum.Char, 1, false)]
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region [MDY:20160607] 增加更正相關欄位
        private string _Reserve1 = String.Empty;
        /// <summary>
        /// 土銀用來記錄被匯出成銷帳資料的日期 (格式：YYYYMMDD)
        /// </summary>
        [FieldSpec(Field.Reserve1, false, FieldTypeEnum.VarChar, 8, false)]
        public string Reserve1
        {
            get
            {
                return _Reserve1;
            }
            set
            {
                _Reserve1 = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Reserve2 = String.Empty;
        /// <summary>
        /// 土銀用來記錄更正記號 (0=正常交易; 1=更正交易;，請參考 D00I70ECMarkCodeTexts 定義)
        /// </summary>
        [FieldSpec(Field.Reserve2, false, FieldTypeEnum.VarChar, 10, false)]
        public string Reserve2
        {
            get
            {
                return _Reserve2;
            }
            set
            {
                _Reserve2 = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 被更正的日期時間
        /// </summary>
        [FieldSpec(Field.RollbackDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? RollbackDate
        {
            get;
            set;
        }
        #endregion

        #region 代碼名稱
        /// <summary>
        /// 代收銀行名稱
        /// </summary>
        [FieldSpec(Field.ReceiveBankName, false, FieldTypeEnum.NVarChar, 10, false)]
        public string ReceiveBankName
        {
            get;
            set;
        }

        /// <summary>
        /// 代收管道名稱
        /// </summary>
        [FieldSpec(Field.ReceiveWayName, false, FieldTypeEnum.NVarChar, 50, false)]
        public string ReceiveWayName
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 取得格式化後的代收日期 (yyyy/MM/dd)
        /// </summary>
        public string ReceiveDateFormat
        {
            get
            {
                DateTime? date = DataFormat.ConvertDateText(this.ReceiveDate);
                if (date != null)
                {
                    return date.Value.ToString("yyyy/MM/dd");
                }
                return this.ReceiveDate ?? String.Empty;
            }
        }

        #region [MDY:20160607] 增加更正註記相關屬性
        /// <summary>
        /// 取得更正註記的文字說明
        /// </summary>
        public string Reserve2Text
        {
            get
            {
                return CancelDebtsEntity.GetReserve2Text(this.Reserve2, this.Status);
            }
        }
        #endregion
        #endregion
    }
}
