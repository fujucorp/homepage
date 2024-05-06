using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
//using System.Reflection;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// 將指定的16進位字串轉成 Byte 陣列
    /// </summary>
    public sealed class Encryption
    {
        #region Member
        /// <summary>
        /// 16進位字串規則運算式
        /// </summary>
        public static readonly Regex HexStringRegex = new Regex("^([0-9A-F]{2})+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 TBB 的加解密共用方法類別
        /// </summary>
        public Encryption()
        {

        }
        #endregion

        #region Method
        /// <summary>
        /// 將指定的16進位字串轉成 Byte 陣列
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public byte[] HexStringToBytes(string hex)
        {
            if (hex == null || !HexStringRegex.IsMatch(hex))
            {
                return null;
            }

            int charCount = hex.Length;
            byte[] bytes = new byte[charCount / 2];
            for (int idx = 0; idx < charCount; idx += 2)
            {
                bytes[idx / 2] = Convert.ToByte(hex.Substring(idx, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 將指定的 Byte 陣列轉成16進位字串
        /// </summary>
        /// <param name="bytes">指定的 Byte 陣列</param>
        /// <param name="toUpper">指定使否將英文字母轉成大寫</param>
        /// <returns>成功則傳回16進位字串，否則傳回 null</returns>
        public string Bytes2HexString(byte[] bytes, bool toUpper = true)
        {
            if (bytes == null)
            {
                return null;
            }

            string hexPattern = toUpper ? "{0:X2}" : "{0:x2}";
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte byteValue in bytes)
            {
                hex.AppendFormat(hexPattern, byteValue);
            }
            return hex.ToString();
        }

        /// <summary>
        /// 取得 EAI 的 Hex 字串資料的 DES 加密後字串 (iv 為 { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })
        /// </summary>
        /// <param name="hexData">指定要加密的 Hex 字串</param>
        /// <param name="hexIKey">指定加密的 Hex 金鑰</param>
        /// <returns>傳回加密後的 Hex 字串</returns>
        public string GetDESEncrypt(string hexData, string hexIKey)
        {
            byte[] dataBytes = this.HexStringToBytes(hexData);
            byte[] iKeyBytes = this.HexStringToBytes(hexIKey);
            byte[] ivBytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = iKeyBytes;
            des.IV = ivBytes;
            ICryptoTransform desEncrypt = des.CreateEncryptor();
            byte[] resultBytes = desEncrypt.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
            string resultHex = this.Bytes2HexString(resultBytes);
            return resultHex.Substring(0, resultHex.Length / 2);
        }

        /// <summary>
        /// 取得 EAI 的 Hex 資料的 DES 解密後的 Hex 字串資料 (iv 為 { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })
        /// </summary>
        /// <param name="hexData">指定要解密的 Hex 字串</param>
        /// <param name="hexIKey">指定解密的 Hex 金鑰</param>
        /// <returns>傳回解密後的 Hex 字串</returns>
        public string GedDESDecrypt(string hexData, string hexIKey)
        {
            hexData = hexData + "0000000000000000";
            byte[] dataBytes = this.HexStringToBytes(hexData);
            byte[] iKeyBytes = this.HexStringToBytes(hexIKey);
            byte[] ivBytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            ICryptoTransform desDecrypt = des.CreateDecryptor(iKeyBytes, ivBytes);
            byte[] resultBytes = new byte[15];
            desDecrypt.TransformBlock(dataBytes, 0, dataBytes.Length, resultBytes, 0);
            return this.Bytes2HexString(resultBytes);
        }

        /// <summary>
        /// 將指定的 input_file 檔做 DES 加密，並存成指定的 out_file 檔
        /// </summary>
        /// <param name="input_file">指定要加密的來源檔</param>
        /// <param name="out_file">指定加密後的輸出檔</param>
        /// <param name="strKey">指定加密的金鑰</param>
        /// <param name="msg">傳回錯誤訊息或空字串</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool DESEncryptFile(string input_file, string out_file, string strKey, out string msg)
        {
            bool rc = false;
            msg = "";
            try
            {
                byte[] key = Encoding.ASCII.GetBytes(strKey);

                #region [Old]
                //string cryptFile = out_file;
                //FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                //RijndaelManaged RMCrypto = new RijndaelManaged();

                //CryptoStream cs = new CryptoStream(fsCrypt,
                //    RMCrypto.CreateEncryptor(key, key),
                //    CryptoStreamMode.Write);

                //FileStream fsIn = new FileStream(input_file, FileMode.Open);

                //int data;
                //while ((data = fsIn.ReadByte()) != -1)
                //    cs.WriteByte((byte)data);


                //fsIn.Close();
                //cs.Close();
                //fsCrypt.Close();
                #endregion

                string cryptFile = out_file;
                using (FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create))
                {
                    using (RijndaelManaged RMCrypto = new RijndaelManaged())
                    {
                        using (CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write))
                        {
                            using (FileStream fsIn = new FileStream(input_file, FileMode.Open))
                            {
                                int data;
                                while ((data = fsIn.ReadByte()) != -1)
                                {
                                    cs.WriteByte((byte)data);
                                }
                                fsIn.Close();
                            }
                            cs.Close();
                        }
                    }
                    fsCrypt.Close();
                }

                rc = true;
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return rc;
        }

        /// <summary>
        /// 將指定的 input_file 檔做 DES 解密，並存成指定的 out_file 檔
        /// </summary>
        /// <param name="input_file">指定要解密的來源檔</param>
        /// <param name="out_file">指定解密後的輸出檔</param>
        /// <param name="strKey">指定解密的金鑰</param>
        /// <param name="msg">傳回錯誤訊息或空字串</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool DESDecryptFile(string input_file, string out_file, string strKey, out string msg)
        {
            bool rc = false;
            msg = "";

            try
            {
                byte[] key = Encoding.ASCII.GetBytes(strKey);

                #region [MDY:20220530] Checkmarx 調整
                #region [OLD]
                //FileStream fsCrypt = new FileStream(input_file, FileMode.Open);

                //RijndaelManaged RMCrypto = new RijndaelManaged();

                //CryptoStream cs = new CryptoStream(fsCrypt,
                //    RMCrypto.CreateDecryptor(key, key),
                //    CryptoStreamMode.Read);

                //FileStream fsOut = new FileStream(out_file, FileMode.Create);

                //int data;
                //while ((data = cs.ReadByte()) != -1)
                //    fsOut.WriteByte((byte)data);

                //fsOut.Close();
                //cs.Close();
                //fsCrypt.Close();
                #endregion

                using (FileStream fsCrypt = new FileStream(input_file, FileMode.Open))
                {
                    using (RijndaelManaged RMCrypto = new RijndaelManaged())
                    {
                        using (CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read))
                        {
                            using (FileStream fsOut = new FileStream(out_file, FileMode.Create))
                            {
                                int data;
                                while ((data = cs.ReadByte()) != -1)
                                {
                                    fsOut.WriteByte((byte)data);
                                }
                                fsOut.Close();
                            }
                            cs.Close();
                        }
                    }
                    fsCrypt.Close();
                }
                #endregion

                rc = true;
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return rc;
        }


        #region [MDY:20160921] 字串加解密
        /// <summary>
        /// 取得指定文字的 DES 加密後 Hex 字串
        /// </summary>
        /// <param name="text">指定要加密的文字</param>
        /// <param name="key">指定加密的金鑰</param>
        /// <param name="encoding">指定編碼，未指定則使用 Encoding.Default</param>
        /// <returns>傳回加密後 Hex 字串或 null</returns>
        public string GetTextDESEncode(string text, string key, Encoding encoding = null)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(key))
            {
                return null;
            }
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            byte[] dataBytes = encoding.GetBytes(text);
            if (dataBytes.Length % 8 != 0)
            {
                int length = ((dataBytes.Length / 8) + 1) * 8;
                byte[] tempBytes = new byte[length];
                Array.Copy(dataBytes, tempBytes, dataBytes.Length);
                dataBytes = tempBytes;
            }
            byte[] iKeyBytes = encoding.GetBytes(key);
            byte[] ivBytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            ICryptoTransform desEncrypt = des.CreateEncryptor(iKeyBytes, ivBytes);
            byte[] resultBytes = desEncrypt.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
            string resultHex = this.Bytes2HexString(resultBytes);
            return resultHex;
        }

        /// <summary>
        /// 取得指定加密後 Hex 文字的 DES 解密後字串
        /// </summary>
        /// <param name="hexText">指定加密後 Hex 文字</param>
        /// <param name="key">指定解密的金鑰</param>
        /// <param name="encoding">指定編碼，未指定則使用 Encoding.Default</param>
        /// <returns>傳回解密後 Hex 字串或 null</returns>
        public string GetTextDESDecode(string hexText, string key, Encoding encoding = null)
        {
            if (String.IsNullOrEmpty(hexText) || String.IsNullOrEmpty(key))
            {
                return null;
            }
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            byte[] dataBytes = this.HexStringToBytes(hexText);
            byte[] iKeyBytes = encoding.GetBytes(key);
            byte[] ivBytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            ICryptoTransform desDecrypt = des.CreateDecryptor(iKeyBytes, ivBytes);
            byte[] resultBytes = new byte[1024];
            int count = desDecrypt.TransformBlock(dataBytes, 0, dataBytes.Length, resultBytes, 0);
            return encoding.GetString(resultBytes, 0, count);
        }
        #endregion

        #endregion
    }
}
