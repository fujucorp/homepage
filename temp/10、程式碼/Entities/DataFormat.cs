using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using Fuju;

namespace Entities
{
    public abstract class DataFormat
    {
        #region Const
        /// <summary>
        /// 土銀代碼 : 005
        /// </summary>
        public const string MyBankID = "005";
        #endregion

        #region [MDY:20160814] 分行代碼驗證
        private static Regex _BankCodeReg = null;

        /// <summary>
        /// 取得是否為土銀的分行代碼 (6碼)
        /// </summary>
        /// <param name="bankCode">分行代碼 (6碼)</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsMyBankCode(string bankCode)
        {
            if (_BankCodeReg == null)
            {
                _BankCodeReg = new Regex(String.Format(@"^{0}\d{{2}}[0-9A-Z]$", MyBankID), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            return _BankCodeReg.IsMatch(bankCode);
        }

        private static Regex _BankFullCodeReg = null;
        /// <summary>
        /// 取得是否為土銀的分行代碼 (7碼)
        /// </summary>
        /// <param name="bankCode">分行代碼 (7碼)</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsMyBankFullCode(string bankCode)
        {
            if (_BankFullCodeReg == null)
            {
                _BankFullCodeReg = new Regex(String.Format(@"^{0}\d{{2}}[0-9A-Z]{{2}}$", MyBankID), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            return _BankFullCodeReg.IsMatch(bankCode);
        }
        #endregion

        #region
        /// <summary>
        /// 取得指定分行代碼的 6碼的分行代碼格式
        /// </summary>
        /// <param name="bank">指定分行代碼</param>
        /// <returns>成功則傳回6碼的分行代碼，否則傳回原字串</returns>
        public static string GetMyBankCodeFormat(string bank)
        {
            if (!String.IsNullOrWhiteSpace(bank))
            {
                string code = bank.Trim().ToUpper();
                if (code.Length == 3)
                {
                    code = MyBankID + code;
                }
                else if (bank.Length == 4)
                {
                    code = MyBankID + code.Substring(0, 3);
                }
                else if (code.Length == 7)
                {
                    code = code.Substring(0, 6);
                }
                if (IsMyBankCode(code))
                {
                    return code;
                }
            }
            return bank;
        }
        #endregion

        #region 驗證字串相關
        /// <summary>
        /// 檢查是否為所有字元都完全重複的字元
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool IsSameChars(string txt)
        {
            if (String.IsNullOrEmpty(txt))
            {
                return false;
            }
            char chr = txt[0];
            for (int idx = 1; idx < txt.Length; idx++)
            {
                if (txt[idx] != chr)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 檢查是否為連續的英文或連續的數字
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool IsSerialEnglisOrNumber(string txt)
        {
            if (String.IsNullOrEmpty(txt))
            {
                return false;
            }
            string eng1 = "abcdefghijklmnopqrstuvwxyz";
            string eng2 = "zyxwvutsrqponmlkjihgfedcba";
            string num1 = "0123456789";
            string num2 = "9876543210";
            string ptn1 = null;
            string ptn2 = null;
            int count = 0;

            count = Convert.ToInt32(txt.Length / eng1.Length) + 2;
            ptn1 = Common.Repeat(eng1, count);
            ptn2 = Common.Repeat(eng2, count);
            if (ptn1.IndexOf(txt) > -1 || ptn2.IndexOf(txt) > -1)
            {
                return true;
            }

            count = Convert.ToInt32(txt.Length / num1.Length) + 2;
            ptn1 = Common.Repeat(num1, count);
            ptn2 = Common.Repeat(num2, count);
            if (ptn1.IndexOf(txt) > -1 || ptn2.IndexOf(txt) > -1)
            {
                return true;
            }

            return false;
        }

        private static Regex _OnlyNumberEnglishReg = null;
        /// <summary>
        /// 取得指定字串是否僅由數字或(且)與英文字元組成
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool OnlyNumberEnglish(string txt)
        {
            if (_OnlyNumberEnglishReg == null)
            {
                _OnlyNumberEnglishReg = new Regex("^[0-9a-zA-Z]{1,}$", RegexOptions.Compiled);
            }
            return _OnlyNumberEnglishReg.IsMatch(txt);
        }

        private static Regex _OnlyNumberEnglishSymbolReg = null;
        /// <summary>
        /// 取得指定字串是否僅由數字或(且)英文或(且)符號字元組成
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool OnlyNumberEnglishSymbol(string txt)
        {
            if (_OnlyNumberEnglishSymbolReg == null)
            {
                _OnlyNumberEnglishSymbolReg = new Regex(@"^[0-9a-zA-Z\x21-\x2f\x3a-\x40\x5b-\x60\x7b-\x7e]{1,}$", RegexOptions.Compiled);
            }
            return _OnlyNumberEnglishSymbolReg.IsMatch(txt);
        }

        private static Regex _HasEnglishReg = null;
        /// <summary>
        /// 取得指定字串是否包含英文字元
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool HasEnglish(string txt)
        {
            if (_HasEnglishReg == null)
            {
                _HasEnglishReg = new Regex(@"[a-zA-Z]{1,}", RegexOptions.Compiled);
            }
            return _HasEnglishReg.IsMatch(txt);
        }

        private static Regex _HasNumericReg = null;
        /// <summary>
        /// 取得指定字串是否包含英文字元
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool HasNumeric(string txt)
        {
            if (_HasNumericReg == null)
            {
                _HasNumericReg = new Regex(@"[0-9]{1,}", RegexOptions.Compiled);
            }
            return _HasNumericReg.IsMatch(txt);
        }
        #endregion

        #region 數值格式化相關
        /// <summary>
        /// 取得指定 Decimal 值的指定整數長度位數的字串 (不含正負號，長度不足則右靠左補0，長度超過則去左取右)
        /// </summary>
        /// <param name="value">指定 Decimal 值</param>
        /// <param name="intSize">指定整數長度位數</param>
        /// <returns>傳回字串</returns>
        public static string GetDecimalString(decimal value, int intSize)
        {
            if (intSize > 0)
            {
                value = Math.Abs(value);
                string intTxt = value.ToString("0");
                if (intTxt.Length > intSize)
                {
                    return intTxt.Substring(intTxt.Length - intSize);
                }
                else if (intTxt.Length < intSize)
                {
                    return intTxt.PadLeft(intSize, '0');
                }
                else
                {
                    return intTxt;
                }
            }
            return String.Empty;
        }
        #endregion

        #region 使用者帳號與密碼格式相關
        /// <summary>
        /// 使用者帳號最小長度 6
        /// </summary>
        public static int UserIdMinSize = 6;
        /// <summary>
        /// 使用者帳號最大長度 16
        /// </summary>
        public static int UserIdMaxSize = 16;

        /// <summary>
        /// 2個以上的英文字母規則運算式
        /// </summary>
        private static Regex _Eng2MoreReg = new Regex("[a-zA-Z].*[a-zA-Z]", RegexOptions.Compiled);

        /// <summary>
        /// 取得使用者帳號註解
        /// </summary>
        /// <returns></returns>
        public static string GetUserIdComment()
        {
            #region [MDY:20181206] 增加 不可連續3個(以上)相同或連號的英數字元 (20181201_03)
            #region [OLD]
            //return String.Format("{0} ~ {1} 碼英數字，且英文字母至少2碼", DataFormat.UserIdMinSize, DataFormat.UserIdMaxSize);
            #endregion

            return String.Format("{0} ~ {1} 碼英文或英數字混合，英文字母至少2碼，且不可含連續3個(或以上)相同或連號的英文或數字", DataFormat.UserIdMinSize, DataFormat.UserIdMaxSize);
            #endregion
        }

        /// <summary>
        /// 檢查使用者帳號是否符合格式要求 (格式為：6 ~ 16 碼英數字 + 英文至少2位 + 不可連續3個(以上)相同或連號的英數字元) 
        /// </summary>
        /// <param name="userId">指定要檢查的使用者帳號。</param>
        /// <returns>符合則傳回 true，否則傳回 false。</returns>
        public static bool CheckUserIDFormat(string userId)
        {
            #region [MDY:20181206] 增加 不可連續3個(以上)相同或連號的英數字元 (20181201_03)
            #region [OLD]
            //return Common.IsEnglishNumber(userId, UserIdMinSize, UserIdMaxSize) && _Eng2MoreReg.IsMatch(userId);

            //#region [Old]
            //////直接判斷必須英文與數字混合
            ////if (String.IsNullOrEmpty(userId) || userId.Length < UserIdMinSize || userId.Length > UserIdMaxSize)
            ////{
            ////    return false;
            ////}
            ////return OnlyNumberEnglish(userId) && Common.HasEnglishAndNumber(userId);
            //#endregion
            #endregion

            if (Common.IsEnglishNumber(userId, UserIdMinSize, UserIdMaxSize) && _Eng2MoreReg.IsMatch(userId))
            {
                if (!_HasSome3EnglishNumberRegex.IsMatch(userId)
                    && !_HasSerial3NumberRegex.IsMatch(userId)
                    && !_HasSerial3EnglishUpperRegex.IsMatch(userId)
                    && !_HasSerial3EnglishLowerRegex.IsMatch(userId))
                {
                    return true;
                }
            }
            return false;
            #endregion
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 使用者密碼最小長度 8
        /// </summary>
        public static int UserPXXMinSize = 8;
        /// <summary>
        /// 使用者密碼最大長度 20
        /// </summary>
        public static int UserPXXMaxSize = 20;
        #endregion

        /// <summary>
        /// 同時包含英文與數字 的規則運算式
        /// </summary>
        private static Regex _HasEnglishAndNumber = new Regex("[0-9]{1,}[a-zA-Z]{1,}|[a-zA-Z]{1,}[0-9]{1,}", RegexOptions.Compiled);
        /// <summary>
        /// 只含英文、數字或符號組成 的規則運算式
        /// </summary>
        private static Regex _IsEnglishAndNumberAndSymbol = new Regex(@"^[0-9a-zA-z\x21-\x2f\x3a-\x40\x5b-\x60\x7b-\x7e]{1,}$", RegexOptions.Compiled);

        #region [MDY:20181206] 改為 不可連續3個(以上)相同或連號的英數字元 (20181201_03)
        #region [OLD]
        ////6個以上(連續)相同的英數字元
        //private static Regex _HasSomeEnglishNumberRegex = new Regex(@"([\w])\1{5,}", RegexOptions.Compiled);
        ////6個以上連號的數字字元
        //private static Regex _HasSerialNumberRegex = new Regex(@"(?:0(?=1)|1(?=2)|2(?=3)|3(?=4)|4(?=5)|5(?=6)|6(?=7)|7(?=8)|8(?=9)|9(?=0)|0(?=9)|9(?=8)|8(?=7)|7(?=6)|6(?=5)|5(?=4)|4(?=3)|3(?=2)|2(?=1)|1(?=0)){5}\d", RegexOptions.Compiled);
        ////6個以上連號的大寫英文字元
        //private static Regex _HasSerialEnglishUpperRegex = new Regex(@"(?:A(?=B)|B(?=C)|C(?=D)|D(?=E)|E(?=F)|F(?=G)|G(?=H)|H(?=I)|I(?=J)|J(?=K)|K(?=L)|L(?=M)|M(?=N)|N(?=O)|O(?=P)|P(?=Q)|Q(?=R)|R(?=S)|S(?=T)|T(?=U)|U(?=V)|V(?=W)|W(?=X)|X(?=Y)|Y(?=Z)|Z(?=A)|A(?=Z)|Z(?=Y)|Y(?=X)|X(?=W)|W(?=V)|V(?=U)|U(?=T)|T(?=S)|S(?=R)|R(?=Q)|Q(?=P)|P(?=O)|O(?=N)|N(?=M)|M(?=L)|L(?=K)|K(?=J)|J(?=I)|I(?=H)|H(?=G)|G(?=F)|F(?=E)|E(?=D)|D(?=C)|C(?=B)|B(?=A)){5}\w", RegexOptions.Compiled);
        ////6個以上連號的小寫英文字元
        //private static Regex _HasSerialEnglishLowerRegex = new Regex(@"(?:a(?=b)|b(?=c)|c(?=d)|d(?=e)|e(?=f)|f(?=g)|g(?=h)|h(?=i)|i(?=j)|j(?=k)|k(?=l)|l(?=m)|m(?=n)|n(?=o)|o(?=p)|p(?=q)|q(?=r)|r(?=s)|s(?=t)|t(?=u)|u(?=v)|v(?=w)|w(?=x)|x(?=y)|y(?=z)|z(?=a)|a(?=z)|z(?=y)|y(?=x)|x(?=w)|w(?=v)|v(?=u)|u(?=t)|t(?=s)|s(?=r)|r(?=q)|q(?=p)|p(?=o)|o(?=n)|n(?=m)|m(?=l)|l(?=k)|k(?=j)|j(?=i)|i(?=h)|h(?=g)|g(?=f)|f(?=e)|e(?=d)|d(?=c)|c(?=b)|b(?=a)){5}\w", RegexOptions.Compiled);
        #endregion

        //3個以上(連續)相同的英數字元
        private static Regex _HasSome3EnglishNumberRegex = new Regex(@"([\w])\1{2,}", RegexOptions.Compiled);

        #region [MDY:20200402] 連續數字、英文字的判斷不包含循環
        #region [OLD]
        ////3個以上連號的數字字元
        //private static Regex _HasSerial3NumberRegex = new Regex(@"(?:0(?=1)|1(?=2)|2(?=3)|3(?=4)|4(?=5)|5(?=6)|6(?=7)|7(?=8)|8(?=9)|9(?=0)|0(?=9)|9(?=8)|8(?=7)|7(?=6)|6(?=5)|5(?=4)|4(?=3)|3(?=2)|2(?=1)|1(?=0)){2}\d", RegexOptions.Compiled);
        ////3個以上連號的大寫英文字元
        //private static Regex _HasSerial3EnglishUpperRegex = new Regex(@"(?:A(?=B)|B(?=C)|C(?=D)|D(?=E)|E(?=F)|F(?=G)|G(?=H)|H(?=I)|I(?=J)|J(?=K)|K(?=L)|L(?=M)|M(?=N)|N(?=O)|O(?=P)|P(?=Q)|Q(?=R)|R(?=S)|S(?=T)|T(?=U)|U(?=V)|V(?=W)|W(?=X)|X(?=Y)|Y(?=Z)|Z(?=A)|A(?=Z)|Z(?=Y)|Y(?=X)|X(?=W)|W(?=V)|V(?=U)|U(?=T)|T(?=S)|S(?=R)|R(?=Q)|Q(?=P)|P(?=O)|O(?=N)|N(?=M)|M(?=L)|L(?=K)|K(?=J)|J(?=I)|I(?=H)|H(?=G)|G(?=F)|F(?=E)|E(?=D)|D(?=C)|C(?=B)|B(?=A)){2}\w", RegexOptions.Compiled);
        ////3個以上連號的小寫英文字元
        //private static Regex _HasSerial3EnglishLowerRegex = new Regex(@"(?:a(?=b)|b(?=c)|c(?=d)|d(?=e)|e(?=f)|f(?=g)|g(?=h)|h(?=i)|i(?=j)|j(?=k)|k(?=l)|l(?=m)|m(?=n)|n(?=o)|o(?=p)|p(?=q)|q(?=r)|r(?=s)|s(?=t)|t(?=u)|u(?=v)|v(?=w)|w(?=x)|x(?=y)|y(?=z)|z(?=a)|a(?=z)|z(?=y)|y(?=x)|x(?=w)|w(?=v)|v(?=u)|u(?=t)|t(?=s)|s(?=r)|r(?=q)|q(?=p)|p(?=o)|o(?=n)|n(?=m)|m(?=l)|l(?=k)|k(?=j)|j(?=i)|i(?=h)|h(?=g)|g(?=f)|f(?=e)|e(?=d)|d(?=c)|c(?=b)|b(?=a)){2}\w", RegexOptions.Compiled);
        #endregion

        #region [OLD]
        ////3個以上連號的數字字元
        //private static Regex _HasSerial3NumberRegex = new Regex(@"(?:0(?=1)|1(?=2)|2(?=3)|3(?=4)|4(?=5)|5(?=6)|6(?=7)|7(?=8)|8(?=9)|9(?=8)|8(?=7)|7(?=6)|6(?=5)|5(?=4)|4(?=3)|3(?=2)|2(?=1)|1(?=0)){2}\d", RegexOptions.Compiled);
        ////3個以上連號的大寫英文字元
        //private static Regex _HasSerial3EnglishUpperRegex = new Regex(@"(?:A(?=B)|B(?=C)|C(?=D)|D(?=E)|E(?=F)|F(?=G)|G(?=H)|H(?=I)|I(?=J)|J(?=K)|K(?=L)|L(?=M)|M(?=N)|N(?=O)|O(?=P)|P(?=Q)|Q(?=R)|R(?=S)|S(?=T)|T(?=U)|U(?=V)|V(?=W)|W(?=X)|X(?=Y)|Y(?=Z)|Z(?=Y)|Y(?=X)|X(?=W)|W(?=V)|V(?=U)|U(?=T)|T(?=S)|S(?=R)|R(?=Q)|Q(?=P)|P(?=O)|O(?=N)|N(?=M)|M(?=L)|L(?=K)|K(?=J)|J(?=I)|I(?=H)|H(?=G)|G(?=F)|F(?=E)|E(?=D)|D(?=C)|C(?=B)|B(?=A)){2}\w", RegexOptions.Compiled);
        ////3個以上連號的小寫英文字元
        //private static Regex _HasSerial3EnglishLowerRegex = new Regex(@"(?:a(?=b)|b(?=c)|c(?=d)|d(?=e)|e(?=f)|f(?=g)|g(?=h)|h(?=i)|i(?=j)|j(?=k)|k(?=l)|l(?=m)|m(?=n)|n(?=o)|o(?=p)|p(?=q)|q(?=r)|r(?=s)|s(?=t)|t(?=u)|u(?=v)|v(?=w)|w(?=x)|x(?=y)|y(?=z)|z(?=y)|y(?=x)|x(?=w)|w(?=v)|v(?=u)|u(?=t)|t(?=s)|s(?=r)|r(?=q)|q(?=p)|p(?=o)|o(?=n)|n(?=m)|m(?=l)|l(?=k)|k(?=j)|j(?=i)|i(?=h)|h(?=g)|g(?=f)|f(?=e)|e(?=d)|d(?=c)|c(?=b)|b(?=a)){2}\w", RegexOptions.Compiled);
        #endregion

        //3個以上連號的數字字元
        private static Regex _HasSerial3NumberRegex = new Regex(@"(?:0(?=1)|1(?=2)|2(?=3)|3(?=4)|4(?=5)|5(?=6)|6(?=7)|7(?=8)|8(?=9)){2}\d|(?:9(?=8)|8(?=7)|7(?=6)|6(?=5)|5(?=4)|4(?=3)|3(?=2)|2(?=1)|1(?=0)){2}\d", RegexOptions.Compiled);
        //3個以上連號的大寫英文字元
        private static Regex _HasSerial3EnglishUpperRegex = new Regex(@"(?:A(?=B)|B(?=C)|C(?=D)|D(?=E)|E(?=F)|F(?=G)|G(?=H)|H(?=I)|I(?=J)|J(?=K)|K(?=L)|L(?=M)|M(?=N)|N(?=O)|O(?=P)|P(?=Q)|Q(?=R)|R(?=S)|S(?=T)|T(?=U)|U(?=V)|V(?=W)|W(?=X)|X(?=Y)|Y(?=Z)){2}\w|(?:Z(?=Y)|Y(?=X)|X(?=W)|W(?=V)|V(?=U)|U(?=T)|T(?=S)|S(?=R)|R(?=Q)|Q(?=P)|P(?=O)|O(?=N)|N(?=M)|M(?=L)|L(?=K)|K(?=J)|J(?=I)|I(?=H)|H(?=G)|G(?=F)|F(?=E)|E(?=D)|D(?=C)|C(?=B)|B(?=A)){2}\w", RegexOptions.Compiled);
        //3個以上連號的小寫英文字元
        private static Regex _HasSerial3EnglishLowerRegex = new Regex(@"(?:a(?=b)|b(?=c)|c(?=d)|d(?=e)|e(?=f)|f(?=g)|g(?=h)|h(?=i)|i(?=j)|j(?=k)|k(?=l)|l(?=m)|m(?=n)|n(?=o)|o(?=p)|p(?=q)|q(?=r)|r(?=s)|s(?=t)|t(?=u)|u(?=v)|v(?=w)|w(?=x)|x(?=y)|y(?=z)){2}\w|(?:z(?=y)|y(?=x)|x(?=w)|w(?=v)|v(?=u)|u(?=t)|t(?=s)|s(?=r)|r(?=q)|q(?=p)|p(?=o)|o(?=n)|n(?=m)|m(?=l)|l(?=k)|k(?=j)|j(?=i)|i(?=h)|h(?=g)|g(?=f)|f(?=e)|e(?=d)|d(?=c)|c(?=b)|b(?=a)){2}\w", RegexOptions.Compiled);
        #endregion

        #endregion

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 檢查使用者密碼是否符合格式要求 (格式為：8 ~ 20 碼英數字 + 不可連續3個(以上)相同或連號的英數字元)
        /// </summary>
        /// <param name="userPXX">指定要檢查的使用者密碼。</param>
        /// <returns>符合則傳回 true，否則傳回 false。</returns>
        public static bool CheckUserPXXFormat(string userPXX)
        {
            #region [MDY:20181206] 改為 不可連續3個(以上)相同或連號的英數字元 (20181201_03)
            #region [OLD]
            //if (userPwd != null && userPwd.Length >= UserPwdMinSize && userPwd.Length <= UserPwdMaxSize && userPwd.IndexOf(' ') == -1
            //    && _IsEnglishAndNumberAndSymbol.IsMatch(userPwd) && _HasEnglishAndNumber.IsMatch(userPwd))
            //{
            //    if (!_HasSomeEnglishNumberRegex.IsMatch(userPwd)
            //        && !_HasSerialNumberRegex.IsMatch(userPwd)
            //        && !_HasSerialEnglishUpperRegex.IsMatch(userPwd)
            //        && !_HasSerialEnglishLowerRegex.IsMatch(userPwd))
            //    {
            //        return true;
            //    }
            //}
            //return false;

            //#region [Old]
            //////直接判斷必須是英數字或符號混雜
            ////if (String.IsNullOrEmpty(userPwd) || userPwd.Length < UserPwdMinSize || userPwd.Length > UserPwdMaxSize)
            ////{
            ////    return false;
            ////}
            ////if (OnlyNumberEnglishSymbol(userPwd))
            ////{
            ////    return ((Common.HasSymbol(userPwd) ? 1 : 0) + (HasNumeric(userPwd) ? 1 : 0) + (HasEnglish(userPwd) ? 1 : 0)) > 1;
            ////}
            ////return false;
            //#endregion
            #endregion

            if (userPXX != null && userPXX.Length >= UserPXXMinSize && userPXX.Length <= UserPXXMaxSize && userPXX.IndexOf(' ') == -1
                && _IsEnglishAndNumberAndSymbol.IsMatch(userPXX) && _HasEnglishAndNumber.IsMatch(userPXX))
            {
                if (!_HasSome3EnglishNumberRegex.IsMatch(userPXX)
                    && !_HasSerial3NumberRegex.IsMatch(userPXX)
                    && !_HasSerial3EnglishUpperRegex.IsMatch(userPXX)
                    && !_HasSerial3EnglishLowerRegex.IsMatch(userPXX))
                {
                    return true;
                }
            }
            return false;
            #endregion
        }
        #endregion

        #region [MDY:20210401] 原碼修正
        //private static char[] _RandomPWorChars = null;
        ///// <summary>
        ///// 產生隨機密碼
        ///// </summary>
        ///// <param name="length">指定隨機密碼長度，小於 1 時以 8 處理。</param>
        ///// <returns>傳回隨機密碼。</returns>
        //public static string GenRandomPWord(int length)
        //{
        //    if (length < 1)
        //    {
        //        length = 8;
        //    }
        //    if (_RandomPWorChars == null)
        //    {
        //        _RandomPWorChars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
        //    }
        //    Random rnd = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

        //    #region [MDY:20210401] 原碼修正
        //    char[] pwords = new char[length];
        //    for (int idx = 0; idx < length; idx++)
        //    {
        //        pwords[idx] = _RandomPWorChars[rnd.Next(0, _RandomPWorChars.Length * 3) % _RandomPWorChars.Length];
        //    }
        //    return String.Join<char>("", pwords);
        //    #endregion
        //}
        #endregion

        #region [MDY:20160921] 使用者密碼的加解密相關
        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 使用者密碼的加解密金鑰
        /// </summary>
        private static readonly string UserPXXKey = "LandBank";

        /// <summary>
        /// 取得使用者密碼的加密後 Hex 字串
        /// </summary>
        /// <param name="value">指定要加密的使用者密碼</param>
        /// <returns>傳回加密後的 Hex 字串或 null</returns>
        public static string GetUserPXXEncode(string value)
        {
            try
            {
                Encryption encryption = new Encryption();
                return encryption.GetTextDESEncode(value, UserPXXKey);
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// 取得使用者密碼的解密後字串
        /// </summary>
        /// <param name="hexData">指定要解密的 Hex 字串</param>
        /// <returns>傳回解密後的字串或 null</returns>
        public static string GetUserPXXDecode(string hexData)
        {
            try
            {
                Encryption encryption = new Encryption();
                return encryption.GetTextDESDecode(hexData, UserPXXKey);
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion
        #endregion
        #endregion

        #region 行員帳號格式相關
        /// <summary>
        /// 取得行員帳號註解
        /// </summary>
        /// <returns></returns>
        public static string GetBankUserIdComment()
        {
            return "6碼數字的員工編號";
        }

        /// <summary>
        /// 檢查使用者帳號是否符合格式要求 (格式為：6 碼數字)
        /// </summary>
        /// <param name="userId">指定要檢查的使用者帳號。</param>
        /// <returns>符合則傳回 true，否則傳回 false。</returns>
        public static bool CheckBankUserIDFormat(string userId)
        {
            return Common.IsNumber(userId, 6);
        }
        #endregion

        #region 遮罩相關

        #region 2015/07/23 朱小姐給的遮罩新邏輯
        //姓名：某些頁面不遮罩，某些頁面要遮 (除前1字元,  後1字元不遮罩，其餘以特殊字元（＊）遮罩)
        //住址：留前6個字
        //出生年月日：遮年
        //聯絡方式(電話)：固定遮後四碼
        #endregion

        /// <summary>
        /// 遮罩資料類型列舉
        /// </summary>
        public enum MaskDataType
        {
            #region 身分證字號/護照號碼
            /// <summary>
            /// 身分證字號/護照號碼
            /// </summary>
            ID = 1,
            #endregion

            #region 姓名
            /// <summary>
            /// 姓名
            /// </summary>
            Name = 2,
            #endregion

            #region 出生年月日
            /// <summary>
            /// 出生年月日
            /// </summary>
            Birthday = 3,
            #endregion

            #region 住址
            /// <summary>
            /// 住址
            /// </summary>
            Address = 4,
            #endregion

            #region 聯絡方式(電話)
            /// <summary>
            /// 聯絡方式(電話)
            /// </summary>
            Tel = 5,
            #endregion

            #region 信用卡號 (不遮了)
            ///// <summary>
            ///// 信用卡號
            ///// </summary>
            //CreditCard = 6,
            #endregion

            #region 帳號 (不遮了)
            ///// <summary>
            ///// 帳號(含信託帳號)
            ///// </summary>
            //AccountID = 7,
            #endregion

            #region 其他 (不遮了)
            ///// <summary>
            ///// 其他
            ///// </summary>
            //Other = 9,
            #endregion

            #region 不遮罩
            /// <summary>
            /// 不遮罩
            /// </summary>
            None = 0
            #endregion
        }

        /// <summary>
        /// 遮罩符號
        /// </summary>
        public const string MaskSymbol = "*";

        /// <summary>
        /// 取得遮罩後的字串
        /// </summary>
        /// <param name="text">要遮罩的字串</param>
        /// <param name="dataType">遮罩資料類型</param>
        /// <returns>傳回罩後的字串</returns>
        public static string MaskText(string text, MaskDataType dataType)
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            switch (dataType)
            {
                case MaskDataType.ID:
                    #region 身分證字號/護照號碼 - 除前3個字元,  後3個字元不遮罩外，其餘以特殊字元（＊）遮罩
                    {
                        if (text.Length > 6)
                        {
                            return String.Concat(text.Substring(0, 3), Common.Repeat(MaskSymbol, text.Length - 6), text.Substring(text.Length - 3, 3));
                        }
                        else
                        {
                            return text;
                        }
                    }
                    #endregion

                case MaskDataType.Name:
                    #region 姓名 - 除前1字元,  後1字元不遮罩，其餘以特殊字元（＊）遮罩
                    {
                        if (text.Length > 2)
                        {
                            #region [MDY:20200705] 姓名遮罩改用全形 O
                            #region [OLD]
                            //return String.Concat(text.Substring(0, 1), Common.Repeat(MaskSymbol, text.Length - 2), text.Substring(text.Length - 1, 1));
                            #endregion

                            return String.Concat(text.Substring(0, 1), Common.Repeat("Ｏ", text.Length - 2), text.Substring(text.Length - 1, 1));
                            #endregion
                        }
                        else
                        {
                            return text;
                        }
                    }
                    #endregion

                case MaskDataType.Birthday:
                    #region 出生年月日 - 遮年
                    {
                        if (Common.IsDate(text))
                        {
                            int yearLength = text.IndexOfAny(new char[] { '/', '-', '.' });
                            return String.Concat(Common.Repeat(MaskSymbol, yearLength), text.Substring(yearLength));
                        }
                        else if (Common.IsNumber(text, 6, 8))
                        {
                            int yearLength = text.Length - 4;
                            return String.Concat(Common.Repeat(MaskSymbol, yearLength), text.Substring(yearLength));
                        }
                        else
                        {
                            return text;
                        }
                    }
                    #endregion

                case MaskDataType.Address:
                    #region 住址 - 留前6個字
                    {
                        if (text.Length > 6)
                        {
                            return String.Concat(text.Substring(0, 6), Common.Repeat(MaskSymbol, text.Length - 6));
                        }
                        else
                        {
                            return text;
                        }
                    }
                    #endregion

                case MaskDataType.Tel:
                    #region 聯絡方式(電話) - 固定遮後四碼
                    {
                        if (text.Length > 4)
                        {
                            return String.Concat(text.Substring(0, text.Length - 4), Common.Repeat(MaskSymbol, 4));
                        }
                        else
                        {
                            return text;
                        }
                    }
                    #endregion

                #region 信用卡號 - 不遮了
                //case MaskDataType.CreditCard:
                //    #region 信用卡號 - 遮第14及第15碼
                //    {
                //        if (text.Length > 15)
                //        {
                //            return String.Concat(text.Substring(0, 13), MaskSymbol, MaskSymbol, text.Substring(15));
                //        }
                //        else if (text.Length == 15)
                //        {
                //            return String.Concat(text.Substring(0, 13), MaskSymbol, MaskSymbol);
                //        }
                //        else if (text.Length == 14)
                //        {
                //            return String.Concat(text.Substring(0, 13), MaskSymbol);
                //        }
                //        else
                //        {
                //            return text;
                //        }
                //    }
                //    #endregion
                #endregion

                #region 帳號(含信託帳號) - 不遮了
                //case MaskDataType.AccountID:
                //    #region 帳號(含信託帳號) - 遮7、8、9碼
                //    {
                //        if (text.Length > 9)
                //        {
                //            return String.Concat(text.Substring(0, 6), MaskSymbol, MaskSymbol, MaskSymbol, text.Substring(9));
                //        }
                //        else if (text.Length == 9)
                //        {
                //            return String.Concat(text.Substring(0, 6), MaskSymbol, MaskSymbol, MaskSymbol);
                //        }
                //        else if (text.Length == 8)
                //        {
                //            return String.Concat(text.Substring(0, 6), MaskSymbol, MaskSymbol);
                //        }
                //        else if (text.Length == 7)
                //        {
                //            return String.Concat(text.Substring(0, 6), MaskSymbol);
                //        }
                //        else
                //        {
                //            return text;
                //        }
                //    }
                //    #endregion
                #endregion

                #region 其他 - 不遮了
                //case MaskDataType.Other:
                //default:
                //    #region 其他 - 二個字遮第二個字，超過二個字保留頭尾，中間遮掉
                //    {
                //        if (text.Length > 2)
                //        {
                //            return String.Concat(text.Substring(0, 1), Common.Repeat(MaskSymbol, text.Length - 2), text.Substring(text.Length - 1, 1));
                //        }
                //        else if (text.Length == 2)
                //        {
                //            return String.Concat(text.Substring(0, 1), MaskSymbol);
                //        }
                //        else
                //        {
                //            return text;
                //        }
                //    }
                //    #endregion
                #endregion
            }
            return text;
        }
        #endregion

        #region 日期文字格式化相關
        /// <summary>
        /// 轉換指定日期字串成 DateTime? 型別
        /// </summary>
        /// <param name="dateText">指定日期字串 (僅允許民國年2、3碼 +月2碼 + 日2碼 或 西元年4碼 + 月2碼 + 日2碼 否則以 DateTime.TryPase() 處理)</param>
        /// <returns>成功則傳回日期型別，否則傳回 null;</returns>
        public static DateTime? ConvertDateText(string dateText)
        {
            if (dateText != null)
            {
                #region [Old]
                ////dateText = dateText.Trim().PadLeft(7, '0');
                //dateText = dateText.Trim().Replace("/","").Replace("-","").PadLeft(7, '0');
                //DateTime date;
                //if (dateText.Length == 7)
                //{
                //    //7碼一律視為民國年 3 碼 + 月2碼 + 日碼
                //    if (Common.TryConvertTWDate7(dateText, out date))
                //    {
                //        return date;
                //    }
                //}
                //else if (dateText.Length == 8)
                //{
                //    //8碼一律視為西元年 4 碼 + 月2碼 + 日碼
                //    if (Common.TryConvertDate8(dateText, out date))
                //    {
                //        return date;
                //    }
                //}
                //else
                //{
                //    if (DateTime.TryParse(dateText, out date))
                //    {
                //        return date;
                //    }
                //}
                #endregion

                dateText = dateText.Trim();
                DateTime date;
                if (Common.IsNumber(dateText, 6, 8))
                {
                    //6碼一律視為民國年 2 碼 + 月2碼 + 日碼
                    if (dateText.Length == 6 && Common.TryConvertTWDate7("0" + dateText, out date))
                    {
                        return date;
                    }
                    //7碼一律視為民國年 3 碼 + 月2碼 + 日碼
                    if (dateText.Length == 7 && Common.TryConvertTWDate7(dateText, out date))
                    {
                        return date;
                    }
                    //8碼一律視為西元年 4 碼 + 月2碼 + 日碼
                    if (dateText.Length == 8 && Common.TryConvertDate8(dateText, out date))
                    {
                        return date;
                    }
                }
                else if (DateTime.TryParse(dateText, out date))
                {
                    return date;
                }
            }
            return null;
        }

        #region [MDY:20190226] 民國年日期字串轉成 DateTime? 型別
        /// <summary>
        /// 轉換指定民國年日期字串成 DateTime? 型別
        /// </summary>
        /// <param name="dateText">指定民國年日期字串 (允許民國年2~4碼 + 月2碼 + 日2碼)</param>
        /// <returns>成功則傳回日期型別，否則傳回 null;</returns>
        public static DateTime? ConvertTWDateText(string dateText)
        {
            if (!String.IsNullOrWhiteSpace(dateText))
            {
                dateText = dateText.Trim();
                if (Common.IsNumber(dateText, 6, 8))
                {
                    dateText = dateText.PadLeft(8, '0');  //補齊8碼 yyyyMMdd

                    DateTime date;
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("zh-TW");
                    culture.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
                    if (DateTime.TryParseExact(dateText, "yyyyMMdd", culture, System.Globalization.DateTimeStyles.None, out date))
                    {
                        return date;
                    }
                }
            }
            return null;
        }
        #endregion
        #endregion

        #region [MDY:20190226] 時間文字格式化相關
        /// <summary>
        /// 時間文字格式 1 (HHmmss / HHmm)
        /// </summary>
        private static Regex _Time1Reg = null;

        /// <summary>
        /// 時間文字格式 2 ([H]H:[m]m:[s]s / HH:mm)
        /// </summary>
        private static Regex _Time2Reg = null;

        /// <summary>
        /// 轉換指定時間字串成 TimeSpan? 型別
        /// </summary>
        /// <param name="timeText">指定時間字串 (支援格式：HHmmss、HH:mm:ss、HHmm、HH:mm)</param>
        /// <returns></returns>
        public static TimeSpan? ConvertTimeText(string timeText)
        {
            if (!String.IsNullOrWhiteSpace(timeText))
            {
                int hour = 0, minute = 0, second = 0;

                #region 時間文字格式 1 ([H]H[m]m[[s]s])
                {
                    if (_Time1Reg == null)
                    {
                        //格式：HHmmss / HHmm
                        _Time1Reg = new Regex("^([0-1][0-9]|[2][0-3])([0-5][0-9])([0-5][0-9])?$", RegexOptions.Compiled);
                    }
                    Match m = _Time1Reg.Match(timeText);
                    if (m.Success)
                    {
                        hour = Int32.Parse(m.Groups[1].Value);
                        minute = Int32.Parse(m.Groups[2].Value);
                        if (m.Groups.Count == 4)
                        {
                            second = Int32.Parse(m.Groups[3].Value);
                        }
                        return new TimeSpan(hour, minute, second);
                    }
                }
                #endregion

                #region 時間文字格式 2 ([H]H:[m]m[:[s]s])
                {
                    if (_Time2Reg == null)
                    {
                        //格式：HH:mm:ss / HH:mm / H:m:s / H:m
                        _Time2Reg = new Regex("^([0-1]?[0-9]|[2][0-3]):([0-5]?[0-9])(:[0-5]?[0-9])?$", RegexOptions.Compiled);
                    }
                    Match m = _Time2Reg.Match(timeText);
                    if (m.Success)
                    {
                        hour = Int32.Parse(m.Groups[1].Value);
                        minute = Int32.Parse(m.Groups[2].Value);
                        if (m.Groups.Count == 4)
                        {
                            second = Int32.Parse(m.Groups[3].Value.Substring(1));
                        }
                        return new TimeSpan(hour, minute, second);
                    }
                }
                #endregion
            }
            return null;
        }

        /// <summary>
        /// 取得指定時間的 8 碼格式的時間字串 (HHmmss)
        /// </summary>
        /// <param name="time">指定時間</param>
        /// <returns>成功則傳回時間字串，否則傳回空串</returns>
        public static string GetTime8Text(TimeSpan? time)
        {
            if (time.HasValue)
            {
                TimeSpan value = time.Value;
                return String.Format("{0:00}{1:00}{2:00}", value.Hours, value.Minutes, value.Seconds);
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion

        #region [MDY:20160925] 連動製單的系統驗證碼加解密 相關
        /// <summary>
        /// 系統驗證碼加解密的金鑰
        /// </summary>
        private static readonly string SysCWordKey = "LANDBANK";
        /// <summary>
        /// 取得系統驗證碼的加密後 Hex 字串
        /// </summary>
        /// <param name="value">指定要加密的使用者密碼</param>
        /// <returns>傳回加密後的 Hex 字串或 null</returns>
        public static string GetSysCWordEncode(string value)
        {
            try
            {
                Encryption encryption = new Encryption();
                return encryption.GetTextDESEncode(value, SysCWordKey);
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// 取得系統驗證碼的解密後字串
        /// </summary>
        /// <param name="hexData">指定要解密的 Hex 字串</param>
        /// <returns>傳回解密後的字串或 null</returns>
        public static string GetSysCWordDecode(string hexData)
        {
            try
            {
                Encryption encryption = new Encryption();
                return encryption.GetTextDESDecode(hexData, SysCWordKey);
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion

        #region [MDY:20161124] 檢查次應用代碼相關
        /// <summary>
        /// 需輸入次應用代碼 (SUBAPPNO) 的最小應用代碼 (APPNO)
        /// </summary>
        public const int MinAppNoForHasSub = 7300;
        /// <summary>
        /// 需輸入次應用代碼 (SUBAPPNO) 的最大應用代碼 (APPNO)
        /// </summary>
        public const int MaxAppNoForHasSub = 7599;

        /// <summary>
        /// 檢查指定應用代碼是否需輸入次應用代碼
        /// </summary>
        /// <param name="appNo">指定應用代碼</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool CheckHasSubAppNo(string appNo)
        {
            if (Common.IsNumber(appNo, 4))
            {
                int value = Convert.ToInt32(appNo);
                if (value >= MinAppNoForHasSub && value <= MaxAppNoForHasSub)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region [MDY:20180224] 支付寶服務 (FiscService) 驗證碼 相關
        /// <summary>
        /// 支付寶服務 (FiscService) Checksum 對照碼清單
        /// </summary>
        private static readonly string[] _FiscService_ChecksumCodes = new string[] { "0A", "1B", "2C", "3D", "4E", "5F", "6G", "7X", "8Y", "9Z", "A0", "B1", "C2", "D3", "E4", "F5" };

        /// <summary>
        /// 產生指定數值的支付寶服務的檢查碼
        /// </summary>
        /// <param name="value">指定數值</param>
        /// <param name="key">驗證碼</param>
        /// <returns>傳回驗證碼</returns>
        public static string GenFiscServiceCheckCode(int value, string key)
        {
            int count = _FiscService_ChecksumCodes.Length;
            string codeA1 = Common.GetBase64Encode(value.ToString("X"));
            string codeA2 = _FiscService_ChecksumCodes[(value / count) % count];
            string codeA3 = _FiscService_ChecksumCodes[(value % count)];

            int bSize = key.Length / 2;
            string codeB1 = key.Substring(0, bSize);
            string codeB2 = key.Substring(bSize, key.Length - bSize);

            string codeText = String.Concat(codeB2, codeA1, codeA2, codeA3, codeB1);
            byte[] codeBytes = Encoding.UTF8.GetBytes(codeText);
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = hashMD5.ComputeHash(codeBytes);

            StringBuilder hex = new StringBuilder();
            foreach (byte hashByte in hashBytes)
            {
                hex.AppendFormat("{0:X2}", hashByte);
            }
            return hex.ToString();
        }
        #endregion

        #region [MDY:20210401] 亂數相關
        /// <summary>
        /// 取得正整數亂數
        /// </summary>
        /// <returns>傳回亂數值</returns>
        public static int GetRandomValue()
        {
            return RNG.GetValue();
        }

        /// <summary>
        /// 取得小於指定最大值的正整數亂數
        /// </summary>
        /// <param name="maxValue">指定最大值</param>
        /// <returns>傳回亂數值</returns>
        public static int GetRandomValue(int maxValue)
        {
            return RNG.GetValue(maxValue);
        }

        /// <summary>
        /// 取得指定最小值至最大值的正整數亂數
        /// </summary>
        /// <param name="minValue">指定最小值</param>
        /// <param name="maxValue">指定最大值</param>
        /// <returns>傳回亂數值</returns>
        public static int GetRandomValue(int minValue, int maxValue)
        {
            return RNG.GetValue(minValue, maxValue);
        }
        #endregion
    }

    #region [MDY:20210401] 使用 RNGCryptoServiceProvider 產生由密碼編譯服務供應者 (CSP) 提供的亂數產生器。
    /// <summary>
    /// 使用 RNGCryptoServiceProvider 產生由密碼編譯服務供應者 (CSP) 提供的亂數產生器。
    /// </summary>
    static class RNG
    {
        private static RNGCryptoServiceProvider _RNGSCP = new RNGCryptoServiceProvider();
        private static byte[] _Buffer = new byte[4];

        /// <summary>
        /// 產生一個非負數的亂數
        /// </summary>
        public static int GetValue()
        {
            _RNGSCP.GetBytes(_Buffer);
            int value = BitConverter.ToInt32(_Buffer, 0);
            return value < 0 ? -value : value;
        }

        /// <summary>
        /// 產生一個非負數且最大值 max 以下的亂數
        /// </summary>
        /// <param name="max">最大值</param>
        public static int GetValue(int max)
        {
            _RNGSCP.GetBytes(_Buffer);
            int value = BitConverter.ToInt32(_Buffer, 0);
            value = value % (max + 1);
            return value < 0 ? -value : value;
        }
        /// <summary>
        /// 產生一個非負數且最小值在 min 以上最大值在 max 以下的亂數
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static int GetValue(int min, int max)
        {
            int value = GetValue(max - min) + min;
            return value;
        }
    }
    #endregion
}
