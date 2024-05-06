using System;

namespace Entities
{
    /// <summary>
    /// 授權代碼(位元旗標)列舉值 (可做位元運算)
    /// </summary>
    [FlagsAttribute]
    public enum AuthCodeEnum
    {
        /// <summary>
        /// 未指定 (未授權) : 0
        /// </summary>
        None = 0,

        #region [Old] 維護、查詢
        ///// <summary>
        ///// 維護授權代碼 : 1
        ///// </summary>
        //Maintain = 1,

        ///// <summary>
        ///// 查詢授權代碼 : 2
        ///// </summary>
        //Query = 2,

        ///// <summary>
        ///// 維護 + 查詢授權代碼 : 3
        ///// </summary>
        //All = Maintain | Query
        #endregion

        #region [New] 新增、修改、刪除、查詢、列印
        /// <summary>
        /// 新增授權代碼 : 0x01
        /// </summary>
        Insert = 0x01,

        /// <summary>
        /// 修改授權代碼 : 0x02
        /// </summary>
        Update = 0x02,

        /// <summary>
        /// 刪除授權代碼 : 0x04
        /// </summary>
        Delete = 0x04,

        /// <summary>
        /// 查詢授權代碼 : 0x08
        /// </summary>
        Select = 0x08,

        /// <summary>
        /// 列印授權代碼 : 0x10
        /// </summary>
        Print = 0x10,

        /// <summary>
        /// 維護授權代碼 : 0x01 | 0x02 | 0x04
        /// </summary>
        Maintain = Insert | Update | Delete,

        /// <summary>
        /// 查詢授權代碼 : 0x08 | 0x10
        /// </summary>
        Query = Select | Print,

        /// <summary>
        /// 全部授權代碼 : Maintain | Query
        /// </summary>
        All = Maintain | Query
        #endregion
    }

    /// <summary>
    /// 授權代碼(位元旗標)列舉的工具類別
    /// </summary>
    public sealed class AuthCodeHelper
    {
        #region Constructor
        private AuthCodeHelper()
        {
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 轉換指定數值為授權代碼
        /// </summary>
        /// <param name="value">指定數值</param>
        /// <returns>傳回授權代碼，無效的數值則傳回 AuthCodeEnum.None</returns>
        public static AuthCodeEnum Convert(int value)
        {
            value &= (int)AuthCodeEnum.All;
            return (AuthCodeEnum)value;
        }

        /// <summary>
        /// 取得是否有新增授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool HasInsert(AuthCodeEnum auth)
        {
            return ((auth & AuthCodeEnum.Insert) == AuthCodeEnum.Insert);
        }

        /// <summary>
        /// 取得是否有修改授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool HasUpdate(AuthCodeEnum auth)
        {
            return ((auth & AuthCodeEnum.Update) == AuthCodeEnum.Update);
        }

        /// <summary>
        /// 取得是否有刪除授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool HasDelete(AuthCodeEnum auth)
        {
            return ((auth & AuthCodeEnum.Delete) == AuthCodeEnum.Delete);
        }

        /// <summary>
        /// 取得是否有查詢授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool HasSelect(AuthCodeEnum auth)
        {
            return ((auth & AuthCodeEnum.Select) == AuthCodeEnum.Select);
        }

        /// <summary>
        /// 取得是否有列印授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool HasPrint(AuthCodeEnum auth)
        {
            return ((auth & AuthCodeEnum.Print) == AuthCodeEnum.Print);
        }

        /// <summary>
        /// 取得是否有維護授權 (有新增、修改、刪除任何一種授權就算有)
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool HasMaintain(AuthCodeEnum auth)
        {
            return ((auth & AuthCodeEnum.Maintain) != AuthCodeEnum.None);
        }

        /// <summary>
        /// 取得是否有查詢授權 (有查詢、列印任何一種授權就算有)
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool HasQuery(AuthCodeEnum auth)
        {
            return ((auth & AuthCodeEnum.Query) != AuthCodeEnum.None);
        }

        /// <summary>
        /// 取得是否有任何授權
        /// </summary>
        /// <param name="auth">授權代碼列舉值</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool HasAnyone(AuthCodeEnum auth)
        {
            return ((auth & AuthCodeEnum.All) != AuthCodeEnum.None);
        }

        /// <summary>
        /// 取得指定授權代碼(列舉值)的權限代碼(字串)
        /// </summary>
        /// <param name="authCode">授權代碼列舉值</param>
        /// <returns>傳回權限代碼字串</returns>
        public static string ToRightCode(AuthCodeEnum authCode)
        {
            AuthCodeEnum[] aCodes = new AuthCodeEnum[] {
                AuthCodeEnum.Insert, AuthCodeEnum.Update, AuthCodeEnum.Delete, AuthCodeEnum.Select, AuthCodeEnum.Print 
            };
            System.Text.StringBuilder rCode = new System.Text.StringBuilder();
            foreach (AuthCodeEnum aCode in aCodes)
            {
                rCode.Append(((authCode & aCode) == aCode) ? "Y" : "N");
            }
            return rCode.ToString();
        }

        /// <summary>
        /// 取得指定權限代碼(字串)的授權代碼(列舉值)
        /// </summary>
        /// <param name="rightCode"></param>
        /// <returns>傳回授權代碼列舉值</returns>
        public static AuthCodeEnum ToAuthCode(string rightCode)
        {
            AuthCodeEnum aCode = AuthCodeEnum.None;
            if (rightCode != null && rightCode.Length <= 5)
            {
                AuthCodeEnum[] aCodes = new AuthCodeEnum[] {
                AuthCodeEnum.Insert, AuthCodeEnum.Update, AuthCodeEnum.Delete, AuthCodeEnum.Select, AuthCodeEnum.Print 
            };
                char[] rCodes = rightCode.ToUpper().PadRight(5, 'N').ToCharArray();
                for (int idx = 0; idx < 5; idx++)
                {
                    if (rCodes[idx].Equals('Y'))
                    {
                        aCode |= aCodes[idx];
                    }
                }
            }
            return aCode;
        }
        #endregion
    }
}
