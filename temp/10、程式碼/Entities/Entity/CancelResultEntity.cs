/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/04/20 18:04:46
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
    /// 每日銷帳結果資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class CancelResultEntity : Entity
    {
        public const string TABLE_NAME = "Cancel_Result";

        #region Field Name Const Class
        /// <summary>
        /// CancelResultEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 代收類別
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 學年代碼
            /// </summary>
            public const string YearId = "Year_Id";

            /// <summary>
            /// 學期代碼
            /// </summary>
            public const string TermId = "Term_Id";

            /// <summary>
            /// 部別代碼
            /// </summary>
            public const string DepId = "Dep_Id";

            /// <summary>
            /// 代收費用別代碼
            /// </summary>
            public const string ReceiveId = "Receive_Id";

            /// <summary>
            /// 入賬日期 (民國年日期7碼)
            /// </summary>
            public const string AccountDate = "Account_Date";
            #endregion

            #region Data
            /// <summary>
            /// 學年名稱
            /// </summary>
            public const string YearName = "Year_Name";

            /// <summary>
            /// 學期名稱
            /// </summary>
            public const string TermName = "Term_Name";

            /// <summary>
            /// 部別名稱 (土銀不使用原有部別 DepList，固定為空字串)
            /// </summary>
            public const string DepName = "Dep_Name";

            /// <summary>
            /// 代收費用別名稱
            /// </summary>
            public const string ReceiveName = "Receive_Name";

            /// <summary>
            /// 收款單位自收筆數
            /// </summary>
            public const string SoCount = "SO_Count";

            /// <summary>
            /// 收款單位自收金額
            /// </summary>
            public const string SoAmount = "SO_Amount";

            /// <summary>
            /// EDI代收筆數
            /// </summary>
            public const string YCount = "Y_Count";

            /// <summary>
            /// EDI代收金額
            /// </summary>
            public const string YAmount = "Y_Amount";

            /// <summary>
            /// 郵局代收筆數 (土銀不使用，固定為 0)
            /// </summary>
            public const string PCount = "P_Count";

            /// <summary>
            /// 郵局代收金額 (土銀不使用，固定為 0)
            /// </summary>
            public const string PAmount = "P_Amount";

            /// <summary>
            /// 統一代收筆數
            /// </summary>
            public const string GCount = "G_Count";

            /// <summary>
            /// 統一代收金額
            /// </summary>
            public const string GAmount = "G_Amount";

            /// <summary>
            /// 全家代收筆數
            /// </summary>
            public const string DCount = "D_Count";

            /// <summary>
            /// 全家代收金額
            /// </summary>
            public const string DAmount = "D_Amount";

            /// <summary>
            /// 萊爾富代收筆數
            /// </summary>
            public const string NCount = "N_Count";

            /// <summary>
            /// 萊爾富代收金額
            /// </summary>
            public const string NAmount = "N_Amount";

            /// <summary>
            /// ＯＫ代收筆數
            /// </summary>
            public const string JCount = "J_Count";

            /// <summary>
            /// ＯＫ代收金額
            /// </summary>
            public const string JAmount = "J_Amount";

            /// <summary>
            /// 語音銀行代收筆數
            /// </summary>
            public const string BCount = "B_Count";

            /// <summary>
            /// 語音銀行代收金額
            /// </summary>
            public const string BAmount = "B_Amount";

            /// <summary>
            /// 網路信用卡財金代收筆數
            /// </summary>
            public const string KCount = "K_Count";

            /// <summary>
            /// 網路信用卡財金代收金額
            /// </summary>
            public const string KAmount = "K_Amount";

            /// <summary>
            /// 網路信用卡中國信託代收筆數
            /// </summary>
            public const string WCount = "W_Count";

            /// <summary>
            /// 網路信用卡中國信託代收金額
            /// </summary>
            public const string WAmount = "W_Amount";

            /// <summary>
            /// ＡＴＭ代收筆數
            /// </summary>
            public const string ACount = "A_Count";

            /// <summary>
            /// ＡＴＭ代收金額
            /// </summary>
            public const string AAmount = "A_Amount";

            /// <summary>
            /// 網路銀行代收筆數
            /// </summary>
            public const string ICount = "I_Count";

            /// <summary>
            /// 網路銀行代收金額
            /// </summary>
            public const string IAmount = "I_Amount";

            /// <summary>
            /// 臨櫃代收筆數
            /// </summary>
            public const string CmfCount = "CMF_Count";

            /// <summary>
            /// 臨櫃代收金額
            /// </summary>
            public const string CmfAmount = "CMF_Amount";

            /// <summary>
            /// 匯款代收筆數
            /// </summary>
            public const string HCount = "H_Count";

            /// <summary>
            /// 匯款代收金額
            /// </summary>
            public const string HAmount = "H_Amount";

            #region [MDY:20170506] 增加支付寶
            /// <summary>
            /// 支付寶代收筆數
            /// </summary>
            public const string C09Count = "C09_Count";

            /// <summary>
            /// 支付寶代收金額
            /// </summary>
            public const string C09Amount = "C09_Amount";
            #endregion

            #region [MDY:20171127] 增加08-全國繳費網、10-台灣Pay (20170831_01)
            /// <summary>
            /// 全國繳代收筆數
            /// </summary>
            public const string C08Count = "C08_Count";

            /// <summary>
            /// 全國繳代收金額
            /// </summary>
            public const string C08Amount = "C08_Amount";

            /// <summary>
            /// 台灣Pay筆數
            /// </summary>
            public const string C10Count = "C10_Count";

            /// <summary>
            /// 台灣Pay金額
            /// </summary>
            public const string C10Amount = "C10_Amount";
            #endregion

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - NC 代收筆數、代收金額
            /// <summary>
            /// 國際信用卡代收筆數
            /// </summary>
            public const string NCCount = "NC_Count";

            /// <summary>
            /// 國際信用卡代收金額
            /// </summary>
            public const string NCAmount = "NC_Amount";
            #endregion

            /// <summary>
            /// 超商筆數
            /// </summary>
            public const string MarketCount = "Market_Count";

            /// <summary>
            /// 超商手續費金額
            /// </summary>
            public const string MarketFee = "Market_Fee";

            /// <summary>
            /// 小計筆數
            /// </summary>
            public const string SubCount = "Sub_Count";

            /// <summary>
            /// 小計金額
            /// </summary>
            public const string SubAmount = "Sub_Amount";

            /// <summary>
            /// 總筆數
            /// </summary>
            public const string TotalCount = "Total_Count";

            /// <summary>
            /// 總金額
            /// </summary>
            public const string TotalAmount = "Total_Amount";

            /// <summary>
            /// 資料建立日期
            /// </summary>
            public const string CreateDate = "Create_Date";

            /// <summary>
            /// 郵局手續費金額 (土銀不使用，固定為 0)
            /// </summary>
            public const string PostFee = "Post_Fee";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// CancelResultEntity 類別建構式
        /// </summary>
        public CancelResultEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _ReceiveType = null;
        /// <summary>
        /// 代收類別
        /// </summary>
        [FieldSpec(Field.ReceiveType, true, FieldTypeEnum.VarChar, 6, false)]
        public string ReceiveType
        {
            get
            {
                return _ReceiveType;
            }
            set
            {
                _ReceiveType = value == null ? null : value.Trim();
            }
        }

        private string _YearId = null;
        /// <summary>
        /// 學年代碼
        /// </summary>
        [FieldSpec(Field.YearId, true, FieldTypeEnum.VarChar, 3, false)]
        public string YearId
        {
            get
            {
                return _YearId;
            }
            set
            {
                _YearId = value == null ? null : value.Trim();
            }
        }

        private string _TermId = null;
        /// <summary>
        /// 學期代碼
        /// </summary>
        [FieldSpec(Field.TermId, true, FieldTypeEnum.Char, 1, false)]
        public string TermId
        {
            get
            {
                return _TermId;
            }
            set
            {
                _TermId = value == null ? null : value.Trim();
            }
        }

        private string _DepId = null;
        /// <summary>
        /// 部別代碼
        /// </summary>
        [FieldSpec(Field.DepId, true, FieldTypeEnum.Char, 1, false)]
        public string DepId
        {
            get
            {
                return _DepId;
            }
            set
            {
                _DepId = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveId = null;
        /// <summary>
        /// 代收費用別代碼
        /// </summary>
        [FieldSpec(Field.ReceiveId, true, FieldTypeEnum.VarChar, 2, false)]
        public string ReceiveId
        {
            get
            {
                return _ReceiveId;
            }
            set
            {
                _ReceiveId = value == null ? null : value.Trim();
            }
        }

        private string _AccountDate = null;
        /// <summary>
        /// 入賬日期 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.AccountDate, true, FieldTypeEnum.VarChar, 8, false)]
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
        #endregion

        #region Data
        /// <summary>
        /// 學年名稱
        /// </summary>
        [FieldSpec(Field.YearName, false, FieldTypeEnum.VarChar, 8, false)]
        public string YearName
        {
            get;
            set;
        }

        /// <summary>
        /// 學期名稱
        /// </summary>
        [FieldSpec(Field.TermName, false, FieldTypeEnum.VarChar, 20, false)]
        public string TermName
        {
            get;
            set;
        }

        /// <summary>
        /// 部別名稱 (土銀不使用原有部別 DepList，固定為空字串)
        /// </summary>
        [FieldSpec(Field.DepName, false, FieldTypeEnum.VarChar, 20, false)]
        public string DepName
        {
            get;
            set;
        }

        /// <summary>
        /// 代收費用別名稱
        /// </summary>
        [FieldSpec(Field.ReceiveName, false, FieldTypeEnum.VarChar, 40, false)]
        public string ReceiveName
        {
            get;
            set;
        }

        /// <summary>
        /// 收款單位自收筆數
        /// </summary>
        [FieldSpec(Field.SoCount, false, FieldTypeEnum.Decimal, false)]
        public decimal SoCount
        {
            get;
            set;
        }

        /// <summary>
        /// 收款單位自收金額
        /// </summary>
        [FieldSpec(Field.SoAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal SoAmount
        {
            get;
            set;
        }

        /// <summary>
        /// EDI代收筆數
        /// </summary>
        [FieldSpec(Field.YCount, false, FieldTypeEnum.Decimal, false)]
        public decimal YCount
        {
            get;
            set;
        }

        /// <summary>
        /// EDI代收金額
        /// </summary>
        [FieldSpec(Field.YAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal YAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 郵局代收筆數 (土銀不使用，固定為 0)
        /// </summary>
        [FieldSpec(Field.PCount, false, FieldTypeEnum.Decimal, false)]
        public decimal PCount
        {
            get;
            set;
        }

        /// <summary>
        /// 郵局代收金額 (土銀不使用，固定為 0)
        /// </summary>
        [FieldSpec(Field.PAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal PAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 統一代收筆數
        /// </summary>
        [FieldSpec(Field.GCount, false, FieldTypeEnum.Decimal, false)]
        public decimal GCount
        {
            get;
            set;
        }

        /// <summary>
        /// 統一代收金額
        /// </summary>
        [FieldSpec(Field.GAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal GAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 全家代收筆數
        /// </summary>
        [FieldSpec(Field.DCount, false, FieldTypeEnum.Decimal, false)]
        public decimal DCount
        {
            get;
            set;
        }

        /// <summary>
        /// 全家代收金額
        /// </summary>
        [FieldSpec(Field.DAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal DAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 萊爾富代收筆數
        /// </summary>
        [FieldSpec(Field.NCount, false, FieldTypeEnum.Decimal, false)]
        public decimal NCount
        {
            get;
            set;
        }

        /// <summary>
        /// 萊爾富代收金額
        /// </summary>
        [FieldSpec(Field.NAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal NAmount
        {
            get;
            set;
        }

        /// <summary>
        /// ＯＫ代收筆數
        /// </summary>
        [FieldSpec(Field.JCount, false, FieldTypeEnum.Decimal, false)]
        public decimal JCount
        {
            get;
            set;
        }

        /// <summary>
        /// ＯＫ代收金額
        /// </summary>
        [FieldSpec(Field.JAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal JAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 語音銀行代收筆數
        /// </summary>
        [FieldSpec(Field.BCount, false, FieldTypeEnum.Decimal, false)]
        public decimal BCount
        {
            get;
            set;
        }

        /// <summary>
        /// 語音銀行代收金額
        /// </summary>
        [FieldSpec(Field.BAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal BAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 網路信用卡財金代收筆數
        /// </summary>
        [FieldSpec(Field.KCount, false, FieldTypeEnum.Decimal, false)]
        public decimal KCount
        {
            get;
            set;
        }

        /// <summary>
        /// 網路信用卡財金代收金額
        /// </summary>
        [FieldSpec(Field.KAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal KAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 網路信用卡中國信託代收筆數
        /// </summary>
        [FieldSpec(Field.WCount, false, FieldTypeEnum.Decimal, false)]
        public decimal WCount
        {
            get;
            set;
        }

        /// <summary>
        /// 網路信用卡中國信託代收金額
        /// </summary>
        [FieldSpec(Field.WAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal WAmount
        {
            get;
            set;
        }

        /// <summary>
        /// ＡＴＭ代收筆數
        /// </summary>
        [FieldSpec(Field.ACount, false, FieldTypeEnum.Decimal, false)]
        public decimal ACount
        {
            get;
            set;
        }

        /// <summary>
        /// ＡＴＭ代收金額
        /// </summary>
        [FieldSpec(Field.AAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal AAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 網路銀行代收筆數
        /// </summary>
        [FieldSpec(Field.ICount, false, FieldTypeEnum.Decimal, false)]
        public decimal ICount
        {
            get;
            set;
        }

        /// <summary>
        /// 網路銀行代收金額
        /// </summary>
        [FieldSpec(Field.IAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal IAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 臨櫃代收筆數
        /// </summary>
        [FieldSpec(Field.CmfCount, false, FieldTypeEnum.Decimal, false)]
        public decimal CmfCount
        {
            get;
            set;
        }

        /// <summary>
        /// 臨櫃代收金額
        /// </summary>
        [FieldSpec(Field.CmfAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal CmfAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 匯款代收筆數
        /// </summary>
        [FieldSpec(Field.HCount, false, FieldTypeEnum.Decimal, false)]
        public decimal HCount
        {
            get;
            set;
        }

        /// <summary>
        /// 匯款代收金額
        /// </summary>
        [FieldSpec(Field.HAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal HAmount
        {
            get;
            set;
        }

        #region [MDY:20170506] 增加支付寶
        /// <summary>
        /// 支付寶代收筆數
        /// </summary>
        [FieldSpec(Field.C09Count, false, FieldTypeEnum.Decimal, false)]
        public decimal C09Count
        {
            get;
            set;
        }

        /// <summary>
        /// 支付寶代收金額
        /// </summary>
        [FieldSpec(Field.C09Amount, false, FieldTypeEnum.Decimal, false)]
        public decimal C09Amount
        {
            get;
            set;
        }
        #endregion

        #region [MDY:20171127] 增加08-全國繳費網、10-台灣Pay (20170831_01)
        /// <summary>
        /// 全國繳代收筆數
        /// </summary>
        [FieldSpec(Field.C08Count, false, FieldTypeEnum.Decimal, false)]
        public decimal C08Count
        {
            get;
            set;
        }

        /// <summary>
        /// 全國繳代收金額
        /// </summary>
        [FieldSpec(Field.C08Amount, false, FieldTypeEnum.Decimal, false)]
        public decimal C08Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 台灣Pay筆數
        /// </summary>
        [FieldSpec(Field.C10Count, false, FieldTypeEnum.Decimal, false)]
        public decimal C10Count
        {
            get;
            set;
        }

        /// <summary>
        /// 台灣Pay金額
        /// </summary>
        [FieldSpec(Field.C10Amount, false, FieldTypeEnum.Decimal, false)]
        public decimal C10Amount
        {
            get;
            set;
        }
        #endregion

        #region [MDY:20191214] (2019擴充案) 國際信用卡 - NC 代收筆數、代收金額
        /// <summary>
        /// 國際信用卡代收筆數
        /// </summary>
        [FieldSpec(Field.NCCount, false, FieldTypeEnum.Decimal, false)]
        public decimal NCCount
        {
            get;
            set;
        }

        /// <summary>
        /// 國際信用卡代收金額
        /// </summary>
        [FieldSpec(Field.NCAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal NCAmount
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 超商筆數
        /// </summary>
        [FieldSpec(Field.MarketCount, false, FieldTypeEnum.Decimal, false)]
        public decimal MarketCount
        {
            get;
            set;
        }

        /// <summary>
        /// 超商手續費金額
        /// </summary>
        [FieldSpec(Field.MarketFee, false, FieldTypeEnum.Decimal, false)]
        public decimal MarketFee
        {
            get;
            set;
        }

        /// <summary>
        /// 小計筆數
        /// </summary>
        [FieldSpec(Field.SubCount, false, FieldTypeEnum.Decimal, false)]
        public decimal SubCount
        {
            get;
            set;
        }

        /// <summary>
        /// 小計金額
        /// </summary>
        [FieldSpec(Field.SubAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal SubAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 總筆數
        /// </summary>
        [FieldSpec(Field.TotalCount, false, FieldTypeEnum.Decimal, false)]
        public decimal TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 總金額
        /// </summary>
        [FieldSpec(Field.TotalAmount, false, FieldTypeEnum.Decimal, false)]
        public decimal TotalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 資料建立日期
        /// </summary>
        [FieldSpec(Field.CreateDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 郵局手續費金額 (土銀不使用，固定為 0)
        /// </summary>
        [FieldSpec(Field.PostFee, false, FieldTypeEnum.Decimal, false)]
        public decimal PostFee
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
