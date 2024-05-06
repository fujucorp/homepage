using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
namespace Entities
{
    public class ZIPHelper
    {
        public ZIPHelper()
        {
            
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 壓縮整個資料夾
        /// </summary>
        private static void ZipDir(string SourceDir, string TargetFile, string pxx, bool BackupOldFile)
        {
            FastZip oZipDir = new FastZip();
            try
            {
                if (!Directory.Exists(SourceDir))
                {
                    throw new Exception("資料夾不存在!");
                }

                if (BackupOldFile == true)
                {
                    //判斷要產生的ZIP檔案是否存在
                    if (File.Exists(TargetFile) == true)
                    {
                        //原本的檔案存在，把他ReName
                        File.Copy(TargetFile, TargetFile + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".back");
                        File.Delete(TargetFile);
                    }
                }

                if (string.IsNullOrEmpty(pxx))
                    oZipDir.Password = pxx;

                oZipDir.CreateZip(TargetFile, SourceDir, true, "");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 壓縮整個資料夾
        /// </summary>
        public static void ZipDir(string SourceDir, string TargetFile, string pxx)
        {
            ZipDir(SourceDir, TargetFile, pxx, true);
        }
        #endregion

        /// <summary>
        /// 壓縮整個資料夾
        /// </summary>
        public static void ZipDir(string SourceDir, string TargetFile)
        {
            ZipDir(SourceDir, TargetFile, "", true);
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 壓縮檔案
        /// </summary>
        private static void ZipFiles(string[] SourceFiles, string TargetFile, string pxx, bool BackupOldFile)
        {
            try
            {
                if (SourceFiles == null || SourceFiles.Length <= 0)
                {
                    throw new Exception("並未傳入檔案完整路徑");
                }

                for (int i = 0; i < SourceFiles.Length; i++)
                {
                    if (File.Exists(SourceFiles[i]) == false)
                    {
                        throw new Exception("要壓縮的檔案【" + SourceFiles[i] + "】不存在");
                    }
                }

                if (BackupOldFile == true)
                {
                    //判斷要產生的ZIP檔案是否存在
                    if (File.Exists(TargetFile) == true)
                    {
                        //原本的檔案存在，把他ReName
                        File.Copy(TargetFile, TargetFile + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".back");
                        File.Delete(TargetFile);
                    }
                }

                ZipOutputStream zs = new ZipOutputStream(File.Create(TargetFile));
                zs.SetLevel(9); //壓縮比
                if (pxx != "")
                {
                    zs.Password = pxx;
                }

                for (int i = 0; i < SourceFiles.Length; i++)
                {
                    FileStream s = File.OpenRead(SourceFiles[i]);
                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);
                    ZipEntry Entry = new ZipEntry(Path.GetFileName(SourceFiles[i]));
                    Entry.DateTime = DateTime.Now;
                    Entry.Size = s.Length;
                    s.Close();
                    zs.PutNextEntry(Entry);
                    zs.Write(buffer, 0, buffer.Length);
                }
                zs.Finish();
                zs.Close();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 壓縮檔案
        /// </summary>
        public static void ZipFiles(string[] SourceFiles, string TargetFile, string pxx)
        {
            ZipFiles(SourceFiles, TargetFile, pxx, true);
        }
        #endregion

        /// <summary>
        /// 壓縮檔案
        /// </summary>
        public static void ZipFiles(string[] SourceFiles, string TargetFile)
        {
            ZipFiles(SourceFiles, TargetFile, "", true);
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 壓縮單一檔案
        /// </summary>
        public static void ZipFile(string SourceFile, string TargetFile, string pxx, bool BackupOldFile)
        {
            ZipFiles(new string[] { SourceFile }, TargetFile, pxx, BackupOldFile);
        }

        /// <summary>
        /// 壓縮單一檔案
        /// </summary>
        public static void ZipFile(string SourceFile, string TargetFile, string pxx)
        {
            ZipFile(SourceFile, TargetFile, pxx, true);
        }
        #endregion

        /// <summary>
        /// 壓縮單一檔案
        /// </summary>
        /// <returns>
        public static void ZipFile(string SourceFile, string TargetFile)
        {
            ZipFile(SourceFile, TargetFile, "", true);
        }

        #region [MDY:20220530] Checkmarx 調整
        /// <summary>
        /// 解壓縮
        /// </summary>
        private static void ExtractZip(string SourceFile, string TargetDir, string pxx)
        {
            FastZip oZip = new FastZip();
            try
            {
                //判斷ZIP檔案是否存在
                if (File.Exists(SourceFile) == false)
                {
                    throw new Exception("要解壓縮的檔案【" + SourceFile + "】不存在，無法執行");
                }
                if (pxx != "")
                {
                    oZip.Password = pxx;
                }
                oZip.ExtractZip(SourceFile, TargetDir, "");
            }
            catch
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 解壓縮
        /// </summary>
        public static void ExtractZip(string SourceFile, string TargetDir)
        {
            ExtractZip(SourceFile, TargetDir, "");
        }

    }
}
