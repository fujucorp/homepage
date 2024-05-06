using System;
using System.Collections.Generic;

using Fuju;

namespace Entities
{
    /// <summary>
    /// 銀行的 AD 群組 (預設的銀行群組：總行、分行主控、會計主管、分行主管、分行經辦、業務部主管、業務部經辦)
    /// </summary>
    public class BankADGroupCodeTexts : CodeTextList
    {
        #region Const Code
        /// <summary>
        /// 總行 : AD0
        /// </summary>
        public const string AD0 = "AD0";

        /// <summary>
        /// 分行主控 : AD1
        /// </summary>
        public const string AD1 = "AD1";

        /// <summary>
        /// 會計主管 : AD2
        /// </summary>
        public const string AD2 = "AD2";

        /// <summary>
        /// 分行主管 : AD3
        /// </summary>
        public const string AD3 = "AD3";

        /// <summary>
        /// 分行經辦 : AD4
        /// </summary>
        public const string AD4 = "AD4";

        /// <summary>
        /// 業務部主管 : AD5
        /// </summary>
        public const string AD5 = "AD5";

        /// <summary>
        /// 業務部經辦 : AD6
        /// </summary>
        public const string AD6 = "AD6";
        #endregion

        #region Const Text
        /// <summary>
        /// 總行 : AD0
        /// </summary>
        public const string AD0_TEXT = "總行";

        /// <summary>
        /// 分行主控 : AD1
        /// </summary>
        public const string AD1_TEXT = "分行主控";

        /// <summary>
        /// 會計主管 : AD2
        /// </summary>
        public const string AD2_TEXT = "會計主管";

        /// <summary>
        /// 分行主管 : AD3
        /// </summary>
        public const string AD3_TEXT = "分行主管";

        /// <summary>
        /// 分行經辦 : AD4
        /// </summary>
        public const string AD4_TEXT = "分行經辦";

        /// <summary>
        /// 業務部主管 : AD5
        /// </summary>
        public const string AD5_TEXT = "業務部主管";

        /// <summary>
        /// 業務部經辦 : AD6
        /// </summary>
        public const string AD6_TEXT = "業務部經辦";
        #endregion

        #region Constructor
        /// <summary>
        /// 建構檔案庫的狀態代碼文字定義清單類別
        /// </summary>
        public BankADGroupCodeTexts()
        {
            base.Add(AD0, AD0_TEXT);
            base.Add(AD1, AD1_TEXT);
            base.Add(AD2, AD2_TEXT);

            base.Add(AD3, AD3_TEXT);
            base.Add(AD4, AD4_TEXT);

            base.Add(AD5, AD5_TEXT);
            base.Add(AD6, AD6_TEXT);
        }
        #endregion

        #region Static Method
        #region [MDY:20161010] 依據【土銀學雜使用者帳號管理說明.doc】文件重新整理程式碼與判斷邏輯
        /// <summary>
        /// 所有銀行 AD 群組集合 靜態 Member
        /// </summary>
        private static readonly BankADGroupCodeTexts _All = new BankADGroupCodeTexts();

        /// <summary>
        /// 取得指定銀行 AD 群組代碼的文字
        /// </summary>
        /// <param name="code">指定銀行 AD 群組代碼</param>
        /// <returns>傳回對應的文字，無對應則傳回空字串</returns>
        public static string GetText(string code)
        {
            CodeText one = GetCodeText(code);
            if (one == null)
            {
                return String.Empty;
            }
            else
            {
                return one.Text;
            }
        }

        /// <summary>
        /// 取得指定銀行 AD 群組代碼的代碼與文字對照類別
        /// </summary>
        /// <param name="code">指定銀行 AD 群組代碼</param>
        /// <returns>傳回對應的代碼名稱對照類別，無對應則傳回 null</returns>
        public static CodeText GetCodeText(string code)
        {
            foreach (CodeText one in _All)
            {
                if (one.Code == code)
                {
                    return one.Copy();
                }
            }
            return null;
        }

        /// <summary>
        /// 取得指定的代碼是否為定義的銀行 AD 群組代碼
        /// </summary>
        /// <param name="code">指定的代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsDefine(string code)
        {
            return GetCodeText(code) == null ? false : true;
        }

        #region 歸屬 AD (即這些群組的帳號存在 AD 中，不可建立帳號) 群組相關
        /// <summary>
        /// 所有歸屬 AD (即這些群組的帳號存在 AD 中，不可建立帳號) 的群組代碼集合 靜態 Member
        /// </summary>
        private static readonly List<string> _InADCodes = new List<string>(new string[] { AD0, AD1, AD2 });

        /// <summary>
        /// 取得指定銀行群組代碼是否為歸屬 AD (即這些群組的帳號存在 AD 中，不可建立帳號)
        /// </summary>
        /// <param name="code">指定銀行群組代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsInAD(string code)
        {
            return _InADCodes.Contains(code);
        }

        /// <summary>
        /// 取得所有歸屬 AD (即這些群組的帳號存在 AD 中，不可建立帳號) 的銀行群組代碼陣列
        /// </summary>
        /// <returns>傳回歸屬 AD (即這些群組的帳號存在 AD 中，不可建立帳號) 的群組代碼陣列</returns>
        public static string[] GetInADCodes()
        {
            return _InADCodes.ToArray();
        }
        #endregion

        #region 總行 (可跨分行、學校) 群組相關
        /// <summary>
        /// 屬於總行 (可跨分行、學校) 的群組代碼集合 靜態 Member
        /// </summary>
        private static readonly List<string> _HeadOfficeCodes = new List<string>(new string[] { AD0, AD5, AD6 });

        /// <summary>
        /// 取得指定銀行群組代碼是否屬於總行 (可跨分行、學校)
        /// </summary>
        /// <param name="code">指定銀行群組代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsHeadOffice(string code)
        {
            return _HeadOfficeCodes.Contains(code);
        }

        /// <summary>
        /// 取得所有屬於總行 (可跨分行、學校) 的群組代碼陣列
        /// </summary>
        /// <returns>傳回屬於總行 (可跨分行、學校) 的群組代碼陣列</returns>
        public static string[] GetHeadOfficeCodes()
        {
            return _HeadOfficeCodes.ToArray();
        }
        #endregion

        #region 分行 群組相關
        /// <summary>
        /// 屬於分行的群組代碼集合 靜態 Member
        /// </summary>
        private static readonly List<string> _BranchCodes = new List<string>(new string[] { AD1, AD2, AD3, AD4 });

        /// <summary>
        /// 取得指定銀行群組代碼是否屬於分行
        /// </summary>
        /// <param name="code">指定銀行群組代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsBranch(string code)
        {
            return _BranchCodes.Contains(code);
        }

        /// <summary>
        /// 取得所有屬於分行的群組代碼陣列
        /// </summary>
        /// <returns>傳回屬於分行的群組代碼陣列</returns>
        public static string[] GetBranchCodes()
        {
            return _BranchCodes.ToArray();
        }
        #endregion

        #region [沒用到 暫時 Mark]
        //#region 主管 (可審核) 群組相關
        ///// <summary>
        ///// 屬於主管 (可審核) 的群組代碼集合 靜態 Member
        ///// </summary>
        //private static readonly List<string> _ChiefCodes = new List<string>(new string[] { AD0, AD1, AD2, AD3, AD5 });

        ///// <summary>
        ///// 取得指定銀行群組代碼是否屬於主管 (可審核)
        ///// </summary>
        ///// <param name="code">指定銀行群組代碼</param>
        ///// <returns>是則傳回 true，否則傳回 false</returns>
        //public static bool IsChief(string code)
        //{
        //    return _ChiefCodes.Contains(code);
        //}

        ///// <summary>
        ///// 取得所有屬於主管 (可審核) 的群組代碼陣列
        ///// </summary>
        ///// <returns>傳回屬於主管 (可審核) 的群組代碼陣列</returns>
        //public static string[] GetChiefCodes()
        //{
        //    return _ChiefCodes.ToArray();
        //}
        //#endregion

        //#region 經辦 群組相關
        ///// <summary>
        ///// 屬於經辦的群組代碼集合 靜態 Member
        ///// </summary>
        //private static readonly List<string> _StafferCodes = new List<string>(new string[] { AD4, AD6 });

        ///// <summary>
        ///// 取得指定銀行群組代碼是否屬於經辦
        ///// </summary>
        ///// <param name="code">指定銀行群組代碼</param>
        ///// <returns>是則傳回 true，否則傳回 false</returns>
        //public static bool IsStaffer(string code)
        //{
        //    return _StafferCodes.Contains(code);
        //}

        ///// <summary>
        ///// 取得所有屬於經辦的群組代碼陣列
        ///// </summary>
        ///// <returns>傳回屬於經辦的群組代碼陣列</returns>
        //public static string[] GetStafferCodes()
        //{
        //    return _StafferCodes.ToArray();
        //}
        //#endregion
        #endregion
        #endregion
        #endregion
    }
}
