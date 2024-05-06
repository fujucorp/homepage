/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/04/17 16:54:10
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 銷帳(入帳)資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class CancelDebtsEntity : Entity
    {
        public const string TABLE_NAME = "Cancel_Debts";

        #region Field Name Const Class
        /// <summary>
        /// CancelDebtsEntity 欄位名稱定義抽象類別
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
            #region 繳款資料
            /// <summary>
            /// 商家代號
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 銷帳編號
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// 代收日期，異業代收則為行員確認日 (民國年日期7碼)
            /// </summary>
            public const string ReceiveDate = "Receive_Date";

            /// <summary>
            /// 代收時間
            /// </summary>
            public const string ReceiveTime = "Receive_Time";

            /// <summary>
            /// 入賬日期 (民國年日期7碼)
            /// </summary>
            public const string AccountDate = "Account_Date";

            /// <summary>
            /// 代收銀行 (土銀使用6碼)
            /// </summary>
            public const string ReceiveBank = "Receive_Bank";

            /// <summary>
            /// 代收管道
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 代收金額
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";
            #endregion

            /// <summary>
            /// 繳款期限 (注意 SchoolRid 的繳款期限是民國年7碼)
            /// </summary>
            public const string PayDueDate = "Pay_Due_Date";

            /// <summary>
            /// 土銀用來記錄被匯出成銷帳資料的日期 (格式：YYYYMMDD)
            /// </summary>
            public const string Reserve1 = "Reserve1";

            /// <summary>
            /// 土銀用來記錄更正記號 (0=正常交易; 1=更正交易;，請參考 D00I70ECMarkCodeTexts 定義)
            /// </summary>
            public const string Reserve2 = "Reserve2";

            /// <summary>
            /// Remark (土銀沒用到，固定空字串)
            /// </summary>
            public const string Remark = "Remark";

            #region 資料來源
            /// <summary>
            /// 資料來源 (資料來源的檔案名稱 或 電文代碼 D00I70 + 商店代號 APPNO + 交易日期 TXDAY 或 D338)
            /// </summary>
            public const string FileName = "File_Name";

            /// <summary>
            /// 資料來源的原始字串 或 D00I70 電文的 D00I70Rec 節點 Xml 或 D338 資料節點 Xml (舊資料轉置則為 NULL)
            /// </summary>
            public const string SourceData = "Source_Data";

            /// <summary>
            /// 資料來源的編號
            /// </summary>
            public const string SourceSeq = "Source_Seq";
            #endregion

            /// <summary>
            /// 最後修改日期時間
            /// </summary>
            public const string ModifyDate = "Modify_Date";

            /// <summary>
            /// 被更正的日期時間
            /// </summary>
            public const string RollbackDate = "Rollback_Date";

            /// <summary>
            /// 銷帳日期時間
            /// </summary>
            public const string CancelDate = "Cancel_Date";

            /// <summary>
            /// 狀態 (0=已銷帳; 1=待處理; 2=處理中; 3=銷帳失敗; 4=免銷帳; 5=已更正; 6=被更正)
            /// </summary>
            public const string Status = "Status";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// CancelDebtsEntity 類別建構式
        /// </summary>
        public CancelDebtsEntity()
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
        #region 繳款資料
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

        private string _AccountDate = null;
        /// <summary>
        /// 入賬日期 (民國年日期7碼)
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

        /// <summary>
        /// 代收銀行 (土銀使用6碼)
        /// </summary>
        [FieldSpec(Field.ReceiveBank, false, FieldTypeEnum.VarChar, 7, false)]
        public string ReceiveBank
        {
            get;
            set;
        }

        private string _ReceiveWay = null;
        /// <summary>
        /// 代收管道
        /// </summary>
        [FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 4, false)]
        public string ReceiveWay
        {
            get
            {
                return _ReceiveWay;
            }
            set
            {
                _ReceiveWay = value == null ? null : value.Trim();
            }
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
        #endregion

        private string _PayDueDate = String.Empty;
        /// <summary>
        /// 繳款期限 (西元年日期8碼) (注意 SchoolRid 的繳款期限是民國年7碼)
        /// </summary>
        [FieldSpec(Field.PayDueDate, false, FieldTypeEnum.Char, 8, false)]
        public string PayDueDate
        {
            get
            {
                return _PayDueDate;
            }
            set
            {
                _PayDueDate = value == null ? String.Empty : value.Trim();
            }
        }

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

        private string _Remark = String.Empty;
        /// <summary>
        /// Remark (土銀沒用到，固定空字串)
        /// </summary>
        [FieldSpec(Field.Remark, false, FieldTypeEnum.VarChar, 40, false)]
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                _Remark = value == null ? String.Empty : value.Trim();
            }
        }

        #region 資料來源
        private string _FileName = String.Empty;
        /// <summary>
        /// 資料來源註記 (資料來源的檔案名稱 或 電文代碼 D00I70 + 商店代號 APPNO + 交易日期 TXDAY 或 D338)
        /// </summary>
        [FieldSpec(Field.FileName, false, FieldTypeEnum.VarChar, 50, false)]
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 資料來源的原始字串 或 D00I70 電文的 D00I70Rec 節點 Xml 或 D338 資料節點 Xml (舊資料轉置則為 NULL)
        /// </summary>
        [FieldSpec(Field.SourceData, false, FieldTypeEnum.NVarChar, 500, true)]
        public string SourceData
        {
            get;
            set;
        }

        /// <summary>
        /// 資料來源的編號
        /// </summary>
        [FieldSpec(Field.SourceSeq, false, FieldTypeEnum.Integer, true)]
        public int? SourceSeq
        {
            get;
            set;
        }
        #endregion


        /// <summary>
        /// 最後修改日期時間
        /// </summary>
        [FieldSpec(Field.ModifyDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime ModifyDate
        {
            get;
            set;
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

        /// <summary>
        /// 銷帳日期時間
        /// </summary>
        [FieldSpec(Field.CancelDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? CancelDate
        {
            get;
            set;
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
        #endregion

        #region [MDY:20160607] 增加更正記號相關屬性
        /// <summary>
        /// 取得更正記號的文字說明
        /// </summary>
        public string Reserve2Text
        {
            get
            {
                return GetReserve2Text(this.Reserve2, this.Status);
            }
        }
        #endregion

        #region [MDY:20160607] 增加更正記號相關方法
        /// <summary>
        /// 取得資料註記
        /// </summary>
        /// <param name="code">指定更正註記</param>
        /// <param name="fgEmpty">指定如果失敗是否傳回空字串，預設 false</param>
        /// <returns>成功則傳回更正註記的文字說明，否則傳回 code 參數或空字串</returns>
        public static string GetReserve2Text(string reserve2, string status)
        {
            if (reserve2 == D00I70ECMarkCodeTexts.RECTIFY_CODE)
            {
                return D00I70ECMarkCodeTexts.RECTIFY_TEXT;
            }
            else if (reserve2 == D00I70ECMarkCodeTexts.NORMAL_CODE)
            {
                if (status == CancelDebtsStatusCodeTexts.BE_RECTIFIED_CODE)
                {
                    return CancelDebtsStatusCodeTexts.BE_RECTIFIED_TEXT;
                }
                else
                {
                    return D00I70ECMarkCodeTexts.NORMAL_TEXT;
                }
            }
            else
            {
                return reserve2;
            }
        }
        #endregion
    }
}
