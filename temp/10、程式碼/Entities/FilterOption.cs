using System;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 主要條件選項資料承載類別
    /// </summary>
    [Serializable]
    public class FilterOption
    {
        #region Property
        private string _SelectedReceiveType = String.Empty;
        /// <summary>
        /// 選擇的代收類別代碼
        /// </summary>
        public string SelectedReceiveType
        {
            get
            {
                return _SelectedReceiveType;
            }
            set
            {
                _SelectedReceiveType = value == null ? String.Empty : value.Trim();
            }
        }

        private string _SelectedReceiveTypeCFlag = String.Empty;
        /// <summary>
        /// 選擇代收類別的 c_flag
        /// </summary>
        public string SelectedReceiveTypeCFlag
        {
            get
            {
                return _SelectedReceiveTypeCFlag;
            }
            set
            {
                _SelectedReceiveTypeCFlag = value == null ? String.Empty : value.Trim();
            }
        }

        private string _SelectedYearID = String.Empty;
        /// <summary>
        /// 選擇的學年代碼
        /// </summary>
        public string SelectedYearID
        {
            get
            {
                return _SelectedYearID;
            }
            set
            {
                _SelectedYearID = value == null ? String.Empty : value.Trim();
            }
        }

        private string _SelectedTermID = String.Empty;
        /// <summary>
        /// 選擇的學期代碼
        /// </summary>
        public string SelectedTermID
        {
            get
            {
                return _SelectedTermID;
            }
            set
            {
                _SelectedTermID = value == null ? String.Empty : value.Trim();
            }
        }

        private string _SelectedDepID = String.Empty;
        /// <summary>
        /// 選擇的部別代碼
        /// </summary>
        public string SelectedDepID
        {
            get
            {
                return _SelectedDepID;
            }
            set
            {
                _SelectedDepID = value == null ? String.Empty : value.Trim();
            }
        }

        private string _SelectedReceiveID = String.Empty;
        /// <summary>
        /// 選擇的代收費用別代碼
        /// </summary>
        public string SelectedReceiveID
        {
            get
            {
                return _SelectedReceiveID;
            }
            set
            {
                _SelectedReceiveID = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 代收類別選項
        /// </summary>
        public CodeText[] ReceiveTypeDatas
        {
            get;
            set;
        }

        /// <summary>
        /// 學年選項
        /// </summary>
        public CodeText[] YearDatas
        {
            get;
            set;
        }

        /// <summary>
        /// 學期選項
        /// </summary>
        public CodeText[] TermDatas
        {
            get;
            set;
        }

        /// <summary>
        /// 部別選項
        /// </summary>
        public CodeText[] DepDatas
        {
            get;
            set;
        }

        /// <summary>
        /// 代收費用別
        /// </summary>
        public CodeText[] ReceiveDatas
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構主要條件選項資料承載類別
        /// </summary>
        public FilterOption()
        {
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得選擇的代收類別名稱
        /// </summary>
        /// <returns>找到則傳回代收類別名稱，否則空字串</returns>
        public string GetSelectedReceiveTypeName()
        {
            if (!String.IsNullOrEmpty(this.SelectedReceiveType)
                && (this.ReceiveTypeDatas != null && this.ReceiveTypeDatas.Length > 0))
            {
                foreach(CodeText data in this.ReceiveTypeDatas)
                {
                    if (data.Code == this.SelectedReceiveType)
                    {
                        return data.Text;
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 取得選擇的學年名稱
        /// </summary>
        /// <returns>找到則傳回學年名稱，否則空字串</returns>
        public string GetSelectedYearName()
        {
            if (!String.IsNullOrEmpty(this.SelectedYearID)
                && (this.YearDatas != null && this.YearDatas.Length > 0))
            {
                foreach (CodeText data in this.YearDatas)
                {
                    if (data.Code == this.SelectedYearID)
                    {
                        return data.Text;
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 取得選擇的學期名稱
        /// </summary>
        /// <returns>找到則傳回學期名稱，否則空字串</returns>
        public string GetSelectedTermName()
        {
            if (!String.IsNullOrEmpty(this.SelectedTermID)
                && (this.TermDatas != null && this.TermDatas.Length > 0))
            {
                foreach (CodeText data in this.TermDatas)
                {
                    if (data.Code == this.SelectedTermID)
                    {
                        return data.Text;
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 取得選擇的部別名稱
        /// </summary>
        /// <returns>找到則傳回部別名稱，否則空字串</returns>
        public string GetSelectedDepName()
        {
            if (!String.IsNullOrEmpty(this.SelectedDepID)
                && (this.DepDatas != null && this.DepDatas.Length > 0))
            {
                foreach (CodeText data in this.DepDatas)
                {
                    if (data.Code == this.SelectedDepID)
                    {
                        return data.Text;
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 取得選擇的代收費用別名稱
        /// </summary>
        /// <returns>找到則傳回代收費用別名稱，否則空字串</returns>
        public string GetSelectedReceiveName()
        {
            if (!String.IsNullOrEmpty(this.SelectedReceiveID)
                && (this.ReceiveDatas != null && this.ReceiveDatas.Length > 0))
            {
                foreach (CodeText data in this.ReceiveDatas)
                {
                    if (data.Code == this.SelectedReceiveID)
                    {
                        return data.Text;
                    }
                }
            }
            return String.Empty;
        }
        #endregion
    }
}
