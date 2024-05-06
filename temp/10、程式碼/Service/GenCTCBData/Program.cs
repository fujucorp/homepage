using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

using Entities;

namespace GenCTCBData
{
    /// <summary>
    /// 產生中國信託學校檔和學生檔 (CTC1)
    /// </summary>
    class Program
    {
        #region FileLog 相關
        private class FileLoger
        {
            private string _LogName = null;
            public string LogName
            {
                get
                {
                    return _LogName;
                }
                private set
                {
                    _LogName = value == null ? null : value.Trim();
                }
            }

            private string _LogPath = null;
            public string LogPath
            {
                get
                {
                    return _LogPath;
                }
                private set
                {
                    _LogPath = value == null ? String.Empty : value.Trim();
                }
            }

            public bool IsDebug
            {
                get;
                private set;
            }

            private string _LogFileName = null;

            public FileLoger(string logName)
            {
                this.LogName = logName;
                this.LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
                if (String.IsNullOrEmpty(logMode))
                {
                    this.IsDebug = false;
                }
                else
                {
                    this.IsDebug = (logMode.Trim().Equals("DEBUG", StringComparison.CurrentCultureIgnoreCase));
                }
                this.Initial();
            }

            public FileLoger(string logName, string path, bool isDebug)
            {
                this.LogName = logName;
                this.LogPath = path;
                this.IsDebug = isDebug;
                this.Initial();
            }

            public string Initial()
            {
                if (!String.IsNullOrEmpty(this.LogPath))
                {
                    try
                    {
                        DirectoryInfo info = new DirectoryInfo(this.LogPath);
                        if (!info.Exists)
                        {
                            info.Create();
                        }
                        if (String.IsNullOrEmpty(this.LogName))
                        {
                            string fileName = String.Format("{0:yyyyMMdd}.log", DateTime.Today);
                            _LogFileName = Path.Combine(info.FullName, fileName);
                        }
                        else
                        {
                            string fileName = String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today);
                            _LogFileName = Path.Combine(info.FullName, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                return null;
            }

            public void WriteLog(string msg)
            {
                if (!String.IsNullOrEmpty(_LogFileName) && msg != null)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
                        {
                            if (String.IsNullOrEmpty(msg))
                            {
                                sw.WriteLine(String.Empty);
                            }
                            else
                            {
                                sw.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, msg);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public void WriteLog(string format, params object[] args)
            {
                if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
                {
                    try
                    {
                        this.WriteLog(String.Format(format, args));
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public void WriteLog(StringBuilder msg)
            {
                if (!String.IsNullOrEmpty(_LogFileName) && msg != null && msg.Length > 0)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
                        {
                            sw.WriteLine(msg);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public void WriteDebugLog(string msg)
            {
                if (this.IsDebug)
                {
                    this.WriteLog(msg);
                }
            }

            public void WriteDebugLog(string format, params object[] args)
            {
                if (this.IsDebug)
                {
                    this.WriteLog(format, args);
                }
            }
        }
        #endregion

        /// <summary>
        /// 檢查或建立指定路徑資料夾
        /// </summary>
        /// <param name="path">指定的資料夾路徑</param>
        /// <param name="fgClear">指定是否清空資料夾，預設否</param>
        /// <returns>傳回錯誤訊息</returns>
        private static string CheckOrCreateFolder(string path, bool fgClear = false)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return "未指定資料夾路徑";
            }
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(path);
                if (dinfo.Exists)
                {
                    #region 資料夾存在
                    if (fgClear)
                    {
                        try
                        {
                            FileInfo[] finfos = dinfo.GetFiles();
                            foreach (FileInfo finfo in finfos)
                            {
                                finfo.Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            return String.Format("無法清空 {0} 資料夾，錯誤訊息：{1}", path, ex.Message);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 資料夾不存在就建立
                    try
                    {
                        dinfo.Create();
                    }
                    catch (Exception ex)
                    {
                        return String.Format("無法建立 {0} 資料夾，錯誤訊息：{1}", path, ex.Message);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return String.Format("不正確的資料夾路徑 ({0})，錯誤訊息：{1}", path, ex.Message);
            }
            return null;
        }

        #region [MDY:20160106] 因為自訂銷帳編號第7碼不一定與費用別相同，所以改用銷編第7碼來組學校檔
        #region [Old]
        //private static DataTable getSchoolData(DataTable student_receive)
        //{
        //    DataTable data = new DataTable();
        //    #region 建schema
        //    DataColumn[] keys = new DataColumn[5];
        //    DataColumn col = new DataColumn("receive_type", System.Type.GetType("System.String"));
        //    data.Columns.Add(col);
        //    keys[0] = col;

        //    col = new DataColumn("sch_name", System.Type.GetType("System.String"));
        //    data.Columns.Add(col);

        //    col = new DataColumn("year_id", System.Type.GetType("System.String"));
        //    data.Columns.Add(col);
        //    keys[1] = col;

        //    col = new DataColumn("term_id", System.Type.GetType("System.String"));
        //    data.Columns.Add(col);
        //    keys[2] = col;

        //    col = new DataColumn("dep_id", System.Type.GetType("System.String"));
        //    data.Columns.Add(col);
        //    keys[3] = col;

        //    col = new DataColumn("receive_id", System.Type.GetType("System.String"));
        //    data.Columns.Add(col);
        //    keys[4] = col;

        //    col = new DataColumn("pay_date", System.Type.GetType("System.String"));
        //    data.Columns.Add(col);

        //    col = new DataColumn("count", System.Type.GetType("System.Int32"));
        //    data.Columns.Add(col);

        //    data.PrimaryKey = keys;
        //    #endregion

        //    if(student_receive!=null && student_receive.Rows.Count>0)
        //    {
        //        foreach (DataRow row in student_receive.Rows)
        //        {
        //            string receive_type = Convert.ToString(row["receive_type"]).Trim();
        //            string sch_name = Convert.ToString(row["sch_name"]).Trim() + Convert.ToString(row["receive_name"]).Trim();
        //            string year_id = Convert.ToString(row["year_id"]).Trim();
        //            string term_id = Convert.ToString(row["term_id"]).Trim();
        //            string dep_id = Convert.ToString(row["dep_id"]).Trim();
        //            string receive_id = Convert.ToString(row["receive_id"]).Trim();
        //            string pay_date = Convert.ToString(row["pay_date"]).Trim();

        //            #region 累計費用別向下有幾筆
        //            string expression = string.Format("receive_type='{0}' and year_id='{1}' and term_id='{2}' and dep_id='{3}' and receive_id='{4}'", receive_type, year_id, term_id, dep_id, receive_id);
        //            DataRow[] rows = data.Select(expression);
        //            if (rows != null && rows.Length > 0)
        //            {
        //                rows[0]["count"] = Convert.ToInt32(rows[0]["count"]) + 1;
        //            }
        //            else
        //            {
        //                DataRow newRow = data.NewRow();
        //                newRow["receive_type"] = receive_type;
        //                newRow["sch_name"] = sch_name;
        //                newRow["year_id"] = year_id;
        //                newRow["term_id"] = term_id;
        //                newRow["dep_id"] = dep_id;
        //                newRow["receive_id"] = receive_id;
        //                newRow["pay_date"] = pay_date;
        //                newRow["count"] = 1;
        //                data.Rows.Add(newRow);
        //            }
        //            #endregion
        //        }
        //    }

        //    return data;
        //}
        #endregion

        private static DataTable getSchoolData(DataTable student_receive, string sepcialValue)
        {
            DataTable data = new DataTable();
            #region 建schema
            DataColumn[] keys = new DataColumn[6];
            DataColumn col = new DataColumn("receive_type", System.Type.GetType("System.String"));
            data.Columns.Add(col);
            keys[0] = col;

            col = new DataColumn("sch_name", System.Type.GetType("System.String"));
            data.Columns.Add(col);

            col = new DataColumn("year_id", System.Type.GetType("System.String"));
            data.Columns.Add(col);
            keys[1] = col;

            col = new DataColumn("term_id", System.Type.GetType("System.String"));
            data.Columns.Add(col);
            keys[2] = col;

            col = new DataColumn("dep_id", System.Type.GetType("System.String"));
            data.Columns.Add(col);
            keys[3] = col;

            col = new DataColumn("receive_id", System.Type.GetType("System.String"));
            data.Columns.Add(col);
            keys[4] = col;

            col = new DataColumn("pay_date", System.Type.GetType("System.String"));
            data.Columns.Add(col);
            keys[5] = col;

            col = new DataColumn("count", System.Type.GetType("System.Int32"));
            data.Columns.Add(col);

            data.PrimaryKey = keys;
            #endregion

            if (student_receive != null && student_receive.Rows.Count > 0)
            {
                foreach (DataRow row in student_receive.Rows)
                {
                    #region [MDY:20170708] Row[] 取值後 Replace 單引號 (For Data Filter Injection 修改)
                    string receive_type = Convert.ToString(row["receive_type"]).Trim().Replace("'", "");
                    #endregion

                    #region [MDY:20170305] 依據中國信託學校檔的學校名稱不串費用別名稱的商家代號清單字串，決定是否不串費用別名稱
                    #region [Old]
                    //string sch_name = Convert.ToString(row["sch_name"]).Trim() + Convert.ToString(row["receive_name"]).Trim();
                    #endregion

                    string sch_name = null;
                    if (sepcialValue.IndexOf("," + receive_type + ",") > -1)
                    {
                        sch_name = Convert.ToString(row["sch_name"]).Trim();
                    }
                    else
                    {
                        sch_name = Convert.ToString(row["sch_name"]).Trim() + Convert.ToString(row["receive_name"]).Trim();
                    }
                    #endregion

                    #region [MDY:20170708] Row[] 取值後 Replace 單引號 (For Data Filter Injection 修改)
                    string year_id = Convert.ToString(row["year_id"]).Trim().Replace("'", "");
                    string term_id = Convert.ToString(row["term_id"]).Trim().Replace("'", "");
                    string dep_id = Convert.ToString(row["dep_id"]).Trim().Replace("'", "");
                    //string receive_id = Convert.ToString(row["receive_id"]).Trim();
                    string pay_date = Convert.ToString(row["pay_date"]).Trim().Replace("'", "");
                    string cancel_no = Convert.ToString(row["cancel_no"]).Trim().Replace("'", "");
                    string receive_id = cancel_no.Length > 7 ? cancel_no.Substring(6,1) : " ";
                    #endregion

                    #region 累計費用別向下有幾筆
                    string expression = string.Format("receive_type='{0}' and year_id='{1}' and term_id='{2}' and dep_id='{3}' and receive_id='{4}' and pay_date='{5}'", receive_type, year_id, term_id, dep_id, receive_id, pay_date);
                    DataRow[] rows = data.Select(expression);
                    if (rows != null && rows.Length > 0)
                    {
                        rows[0]["count"] = Convert.ToInt32(rows[0]["count"]) + 1;
                    }
                    else
                    {
                        DataRow newRow = data.NewRow();
                        newRow["receive_type"] = receive_type;
                        newRow["sch_name"] = sch_name;
                        newRow["year_id"] = year_id;
                        newRow["term_id"] = term_id;
                        newRow["dep_id"] = dep_id;
                        newRow["receive_id"] = receive_id;
                        newRow["pay_date"] = pay_date;
                        newRow["count"] = 1;
                        data.Rows.Add(newRow);
                    }
                    #endregion
                }
            }

            return data;
        }
        #endregion

        private static string getStudentDataSQL()
        {
            #region [MDY:20170506] 修正中信管道常數為 CTCB
            #region [Old]
//            string sql = @"
//SELECT * 
//FROM (   
//select sr.receive_type receive_type
//     , ISNULL((select school_rtype.sch_name from school_rtype where school_rtype.Receive_Type=sr.Receive_Type),'') sch_name
//     , sr.year_id year_id
//     , sr.term_id term_id
//     , sr.dep_id dep_id
//     , sr.receive_id receive_id
//     , sr.Cancel_No cancel_no
//     , Convert(numeric(8),sr.Receive_Amount) receive_amount
//     , sr.stu_id stu_id
//     , ISNULL((select student_master.stu_name from student_master where student_master.receive_type=sr.receive_type and student_master.dep_id=sr.dep_id and student_master.stu_id=sr.stu_id),'') stu_name
//     , (SELECT 19110000 + CASE WHEN ISNUMERIC(School_Rid.Pay_Due_Date2) = 1 THEN Convert(numeric(8), School_Rid.Pay_Due_Date2) ELSE 0 END  /* 信用卡繳費期限 */
//          FROM School_Rid 
//         WHERE School_Rid.Receive_Type = sr.Receive_Type
//           AND School_Rid.Year_Id = sr.Year_Id
//           AND School_Rid.Term_Id = sr.Term_Id
//           AND School_Rid.Dep_Id = sr.Dep_Id
//           AND School_Rid.Receive_Id = sr.Receive_Id
//       ) pay_date
//     , ISNULL((SELECT Receive_Name FROM Receive_List rl WHERE rl.Receive_Type=sr.Receive_Type AND rl.Year_Id=sr.Year_Id AND rl.Term_Id=sr.Term_Id AND rl.Dep_Id=sr.Dep_Id AND rl.Receive_Id=sr.Receive_Id), '') AS receive_name
//     , ISNULL(sr.UploadFlag, '') UploadFlag
//  FROM Student_Receive sr
// WHERE (sr.Cancel_No != '' AND sr.Cancel_No IS NOT NULL)
//   AND (sr.Receive_Amount > 0 AND sr.Receive_Amount IS NOT NULL)
//   AND (sr.Receive_Way = '' OR sr.Receive_Way IS NULL)
//   AND (sr.c_flag = '0' OR sr.c_flag IS NULL)
//   AND sr.Receive_Type IN (SELECT DISTINCT rc_type FROM receive_channel WHERE rc_channel = '" + ChannelHelper.CTCD + @"')
//) AS T
// where pay_date > CONVERT(numeric(8),Convert(char(8),getdate(),112))
// order by receive_type,year_id,term_id,dep_id,receive_id,stu_id
//";
            #endregion

            string sql = @"
SELECT *
  FROM (
SELECT sr.receive_type receive_type
     , ISNULL((select school_rtype.sch_name from school_rtype where school_rtype.Receive_Type=sr.Receive_Type),'') sch_name
     , sr.year_id year_id
     , sr.term_id term_id
     , sr.dep_id dep_id
     , sr.receive_id receive_id
     , sr.Cancel_No cancel_no
     , Convert(numeric(8),sr.Receive_Amount) receive_amount
     , sr.stu_id stu_id
     , ISNULL((select student_master.stu_name from student_master where student_master.receive_type=sr.receive_type and student_master.dep_id=sr.dep_id and student_master.stu_id=sr.stu_id),'') stu_name
     , (SELECT 19110000 + CASE WHEN ISNUMERIC(School_Rid.Pay_Due_Date2) = 1 THEN Convert(numeric(8), School_Rid.Pay_Due_Date2) ELSE 0 END  /* 信用卡繳費期限 */
          FROM School_Rid 
         WHERE School_Rid.Receive_Type = sr.Receive_Type
           AND School_Rid.Year_Id = sr.Year_Id
           AND School_Rid.Term_Id = sr.Term_Id
           AND School_Rid.Dep_Id = sr.Dep_Id
           AND School_Rid.Receive_Id = sr.Receive_Id
       ) pay_date
     , ISNULL((SELECT Receive_Name FROM Receive_List rl WHERE rl.Receive_Type=sr.Receive_Type AND rl.Year_Id=sr.Year_Id AND rl.Term_Id=sr.Term_Id AND rl.Dep_Id=sr.Dep_Id AND rl.Receive_Id=sr.Receive_Id), '') AS receive_name
     , ISNULL(sr.UploadFlag, '') UploadFlag
  FROM Student_Receive sr
 WHERE (sr.Cancel_No != '' AND sr.Cancel_No IS NOT NULL)
   AND (sr.Receive_Amount > 0 AND sr.Receive_Amount IS NOT NULL)
   AND (sr.Receive_Way = '' OR sr.Receive_Way IS NULL)
   AND (sr.c_flag = '0' OR sr.c_flag IS NULL)
   AND sr.Receive_Type IN (SELECT DISTINCT rc_type FROM receive_channel WHERE rc_channel = '" + ChannelHelper.CTCB + @"')
) AS T
 WHERE pay_date > CONVERT(numeric(8),Convert(char(8),getdate(),112))
 ORDER BY receive_type,year_id,term_id,dep_id,receive_id,stu_id
";
            #endregion

            return sql;
        }

        /// <summary>
        /// 取得 QueueCTCB 要發送的資料
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="sendStamp"></param>
        /// <param name="stuTable">學生檔資料</param>
        /// <param name="schTable">學校檔資料</param>
        /// <returns></returns>
        private static Result GetQueueCTCB(EntityFactory factory, out string sendStamp, out DataTable stuTable, out DataTable schTable)
        {
            stuTable = null;
            schTable = null;
            Result result = null;

            #region 註記戳記
            sendStamp = System.Guid.NewGuid().ToString("N");
            {
                string sql = @"UPDATE [Queue_CTCB] SET [Send_Stamp] = @SendStamp 
  WHERE [Send_Stamp] IS NULL AND [Send_Date] IS NULL
    AND (19110000 + CASE WHEN ISNUMERIC([Pay_Due_Date]) = 1 THEN Convert(numeric(8), [Pay_Due_Date]) ELSE 0 END) >= CONVERT(numeric(8),Convert(char(8),GETDATE(),112))";
                KeyValue[] parameters = new KeyValue[] { new KeyValue("@SendStamp", sendStamp) };

                int count = 0;
                result = factory.ExecuteNonQuery(sql, parameters, out count);
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            #endregion

            #region 取回戳記資料
            {
                string sql = @"
SELECT [Stu_Id], [Stu_Name], [Cancel_No], Convert(numeric(8), [Receive_Amount]) AS [Receive_Amount]
        , [Receive_Type]
        , ISNULL((SELECT [Sch_Name] FROM [School_Rtype] AS SR WHERE SR.[Receive_Type] = A.[Receive_Type]), '') AS [Sch_Name]
        , (19110000 + CASE WHEN ISNUMERIC([Pay_Due_Date]) = 1 THEN Convert(numeric(8), [Pay_Due_Date]) ELSE 0 END) AS [Pay_Due_Date]
  FROM [Queue_CTCB] AS A
 WHERE [Send_Stamp] = @SendStamp AND [Send_Date] IS NULL AND [Cancel_No] IS NOT NULL AND [Cancel_No] != ''";
                KeyValue[] parameters = new KeyValue[] { new KeyValue("@SendStamp", sendStamp) };

                result = factory.GetDataTable(sql, parameters, 0, 0, out stuTable);
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            #endregion

            #region 產生學校檔的資料
            schTable = new DataTable();

            #region [MDY:20160106] 改用銷編第7碼來組學校檔
            #region [Old]
            //#region 建schema
            //DataColumn[] keys = new DataColumn[2];
            //DataColumn col = new DataColumn("Receive_Type", System.Type.GetType("System.String"));
            //schTable.Columns.Add(col);
            //keys[0] = col;

            //col = new DataColumn("Sch_Name", System.Type.GetType("System.String"));
            //schTable.Columns.Add(col);

            //col = new DataColumn("Pay_Due_Date", System.Type.GetType("System.String"));
            //schTable.Columns.Add(col);
            //keys[1] = col;

            //col = new DataColumn("Count", System.Type.GetType("System.Int32"));
            //schTable.Columns.Add(col);

            //schTable.PrimaryKey = keys;
            //#endregion

            //if (stuTable != null && stuTable.Rows.Count > 0)
            //{
            //    foreach (DataRow row in stuTable.Rows)
            //    {
            //        string receiveType = Convert.ToString(row["Receive_Type"]).Trim();
            //        string schName = Convert.ToString(row["Sch_Name"]).Trim();
            //        string payDueDate = Convert.ToString(row["Pay_Due_Date"]).Trim();

            //        #region 累計商家代號+繳款期限有幾筆
            //        string expression = string.Format("Receive_Type='{0}' AND Pay_Due_Date='{1}' ", receiveType, payDueDate);
            //        DataRow[] rows = schTable.Select(expression);
            //        if (rows != null && rows.Length > 0)
            //        {
            //            rows[0]["Count"] = Convert.ToInt32(rows[0]["count"]) + 1;
            //        }
            //        else
            //        {
            //            DataRow newRow = schTable.NewRow();
            //            newRow["Receive_Type"] = receiveType;
            //            newRow["Sch_Name"] = schName;
            //            newRow["Pay_Due_Date"] = payDueDate;
            //            newRow["Count"] = 1;
            //            schTable.Rows.Add(newRow);
            //        }
            //        #endregion
            //    }
            //}
            #endregion

            #region 建schema
            DataColumn[] keys = new DataColumn[3];
            DataColumn col = new DataColumn("Receive_Type", System.Type.GetType("System.String"));
            schTable.Columns.Add(col);
            keys[0] = col;

            col = new DataColumn("Sch_Name", System.Type.GetType("System.String"));
            schTable.Columns.Add(col);

            col = new DataColumn("Receive_Id", System.Type.GetType("System.String"));
            schTable.Columns.Add(col);
            keys[1] = col;

            col = new DataColumn("Pay_Due_Date", System.Type.GetType("System.String"));
            schTable.Columns.Add(col);
            keys[2] = col;

            col = new DataColumn("Count", System.Type.GetType("System.Int32"));
            schTable.Columns.Add(col);

            schTable.PrimaryKey = keys;
            #endregion

            if (stuTable != null && stuTable.Rows.Count > 0)
            {
                foreach (DataRow row in stuTable.Rows)
                {
                    #region [MDY:20170708] Row[] 取值後 Replace 單引號 (For Data Filter Injection 修改)
                    string receiveType = Convert.ToString(row["Receive_Type"]).Trim().Replace("'", "");
                    string schName = Convert.ToString(row["Sch_Name"]).Trim().Replace("'", "");
                    string payDueDate = Convert.ToString(row["Pay_Due_Date"]).Trim().Replace("'", "");

                    string cancelNo = Convert.ToString(row["Cancel_No"]).Trim().Replace("'", "");
                    string receiveId = cancelNo.Length > 7 ? cancelNo.Substring(6, 1) : " ";
                    #endregion

                    #region 累計商家代號+繳款期限有幾筆
                    string expression = string.Format("Receive_Type='{0}' AND Receive_Id='{1}' AND Pay_Due_Date='{2}' ", receiveType, receiveId, payDueDate);
                    DataRow[] rows = schTable.Select(expression);
                    if (rows != null && rows.Length > 0)
                    {
                        rows[0]["Count"] = Convert.ToInt32(rows[0]["count"]) + 1;
                    }
                    else
                    {
                        DataRow newRow = schTable.NewRow();
                        newRow["Receive_Type"] = receiveType;
                        newRow["Sch_Name"] = schName;
                        newRow["Receive_Id"] = receiveId;
                        newRow["Pay_Due_Date"] = payDueDate;
                        newRow["Count"] = 1;
                        schTable.Rows.Add(newRow);
                    }
                    #endregion
                }
            }
            #endregion
            #endregion

            return result;
        }

        private static Result SetQueueCTCBSendDate(EntityFactory factory, string sendStamp)
        {
            string sql = @"UPDATE [Queue_CTCB] SET [Send_Date] = GETDATE() WHERE [Send_Stamp] = @SendStamp AND [Send_Date] IS NULL";
            KeyValue[] parameters = new KeyValue[] { new KeyValue("@SendStamp", sendStamp) };

            int count = 0;
            Result result = factory.ExecuteNonQuery(sql, parameters, out count);
            return result;
        }

        #region [MDY:20170305] 取得中國信託學校檔的學校名稱不串費用別名稱的商家代號清單字串
        /// <summary>
        /// 取得中國信託學校檔的學校名稱不串費用別名稱的商家代號清單字串 (以逗號隔開每筆商家代號，且頭尾都有逗號)
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="specialReceiveTypes">傳回商家代號清單字串或空字串</param>
        /// <returns></returns>
        private static Result GetCTCBSpecialConfigValue(EntityFactory factory, out string configValue)
        {
            configValue = String.Empty;
            ConfigEntity instance = null;
            Expression where = new Expression(ConfigEntity.Field.ConfigKey, "CTCBSpecial");
            Result result = factory.SelectFirst<ConfigEntity>(where, null, out instance);
            if (result.IsSuccess)
            {
                if (instance != null && !String.IsNullOrWhiteSpace(instance.ConfigValue))
                {
                    configValue = ("," + instance.ConfigValue.Replace(" ", "") + ",").Replace(",,", ",");
                }
            }
            return result;
        }
        #endregion

        ///// <summary>
        ///// 最大失敗重試次數 : 8
        ///// </summary>
        //private static int _MaxRetryTimes = 8;
        ///// <summary>
        ///// 最大失敗重試間隔(單位分鐘) : 60
        ///// </summary>
        //private static int _MaxRetrySleep = 60;

        /// <summary>
        /// 產生中信語音檔
        /// 中信語音檔包含學校檔與學生檔，zip壓縮後傳送
        /// 處理原則為
        /// 1.要有銷帳編號
        /// 2.要有金額
        /// 3.未過繳費期限
        /// 4.未處理過(student_receive.c_flag判斷)
        /// </summary>
        /// <param name="args">
        /// 參數：無
        /// </param>
        static void Main(string[] args)
        {
            #region Initial
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string appGuid = myAssembly.GetCustomAttribute<GuidAttribute>().Value;
            string appName = Path.GetFileNameWithoutExtension(myAssembly.Location);

            FileLoger fileLog = new FileLoger(appName);
            JobCubeCheckMode jobCheckMode = JobCubeCheckMode.ByTime;
            string jobTypeId = JobCubeTypeCodeTexts.CTC1;                   //作業類別代碼
            string jobTypeName = JobCubeTypeCodeTexts.GetText(jobTypeId);   //作業類別名稱
            int exitCode = 0;
            string exitMsg = null;
            #endregion

            StringBuilder log = new StringBuilder();

            //暫時不提供 retry 
            //int retryTimes = 5;     //re-try 次數 (預設為5次)
            //int retrySleep = 5;     //re-try 間隔，單位分鐘 (預設為5分鐘)

            DateTime startTime = DateTime.Now;

            JobCubeHelper jobHelper = new JobCubeHelper();
            int jobNo = 0;
            string jobStamp = null;
            StringBuilder jobLog = new StringBuilder();

            try
            {
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 開始", DateTime.Now, appName).AppendLine();

                #region 處理參數
                //無參數
                #endregion

                #region 處理 Config 參數
                #region 產出檔案路徑
                string dataPath = null;
                string tempPath = null;
                string bakPath = null;
                if (exitCode == 0)
                {
                    dataPath = ConfigurationManager.AppSettings.Get("DATA_PATH");
                    if (String.IsNullOrWhiteSpace(dataPath))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定產出檔案路徑 (DATA_PATH) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        dataPath = dataPath.Trim();
                        tempPath = Path.Combine(dataPath, "tmp");
                        bakPath = Path.Combine(dataPath, "bak");

                        string errmsg = CheckOrCreateFolder(dataPath);
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            errmsg = CheckOrCreateFolder(bakPath);
                        }
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            exitCode = -1;
                            exitMsg = String.Format("Config (DATA_PATH)參數錯誤，{0}", errmsg);
                            jobLog.AppendLine(exitMsg);
                            log.AppendLine(exitMsg);
                        }
                    }
                }
                #endregion

                #region 學校檔檔案名稱
                string schoolFileName = null;
                if (exitCode == 0)
                {
                    schoolFileName = ConfigurationManager.AppSettings.Get("SCHOOL_FILE_NAME");
                    if (String.IsNullOrWhiteSpace(schoolFileName))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定學校檔檔案名稱 (SCHOOL_FILE_NAME) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                }
                #endregion

                #region 學生檔檔案名稱
                string studentFileName = null;
                if (exitCode == 0)
                {
                    studentFileName = ConfigurationManager.AppSettings.Get("STUDENT_FILE_NAME");
                    if (String.IsNullOrWhiteSpace(studentFileName))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定學生檔檔案名稱 (STUDENT_FILE_NAME) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                }
                #endregion

                #region 壓縮檔設定 (是否啟用與密碼)
                #region [MDY:20210401] 原碼修正
                string zipFileName = String.Format("LB{0:yyyyMMdd}.ZIP", startTime);
                bool isUseZip = true;
                string zipPWord = null;
                if (exitCode == 0)
                {
                    string zipEnable = ConfigurationManager.AppSettings.Get("ZIP_ENABLE");
                    if (String.IsNullOrWhiteSpace(zipEnable))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定是否啟用壓縮檔 (ZIP_ENABLE) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                    else
                    {
                        zipEnable = zipEnable.Trim().ToLower();
                        switch (zipEnable)
                        {
                            case "true":
                                isUseZip = true;
                                break;
                            case "false":
                                isUseZip = false;
                                break;
                            default:
                                exitCode = -1;
                                exitMsg = "Config 參數錯誤，是否啟用壓縮檔 (ZIP_ENABLE) 參數不正確 (true=啟用/false=不啟用)";
                                jobLog.AppendLine(exitMsg);
                                log.AppendLine(exitMsg);
                                break;
                        }
                    }

                    zipPWord = ConfigurationManager.AppSettings.Get("ZIP_PWORD");
                    if (exitCode == 0 && isUseZip && String.IsNullOrWhiteSpace(zipPWord))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定壓縮檔密碼 (ZIP_PWORD) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                }
                #endregion
                #endregion

                #region CTCB 的 FTP 設定
                #region [MDY:20210401] 原碼修正
                string ftpHost = null;
                string ftpPort = null;
                string ftpUid = null;
                string ftpPWord = null;
                string ftpPath = null;
                if (exitCode == 0)
                {
                    ftpHost = ConfigurationManager.AppSettings.Get("CTCB_FTP_HOST");
                    ftpPort = ConfigurationManager.AppSettings.Get("CTCB_FTP_PORT");
                    ftpUid = ConfigurationManager.AppSettings.Get("CTCB_FTP_UID");
                    ftpPWord = ConfigurationManager.AppSettings.Get("CTCB_FTP_PXX");
                    ftpPath = ConfigurationManager.AppSettings.Get("CTCB_FTP_PATH");
                    List<string> lost = new List<string>(5);
                    if (String.IsNullOrWhiteSpace(ftpHost))
                    {
                        lost.Add("CTCB_FTP_HOST");
                    }
                    else
                    {
                        ftpHost = ftpHost.Trim();
                    }
                    if (String.IsNullOrWhiteSpace(ftpPort) || !Common.IsNumber(ftpPort))
                    {
                        lost.Add("CTCB_FTP_PORT");
                    }
                    else
                    {
                        ftpPort = ftpPort.Trim();
                    }
                    if (String.IsNullOrWhiteSpace(ftpUid))
                    {
                        lost.Add("CTCB_FTP_UID");
                    }
                    else
                    {
                        ftpUid = ftpUid.Trim();
                    }
                    if (String.IsNullOrWhiteSpace(ftpPWord))
                    {
                        lost.Add("CTCB_FTP_PXX");
                    }
                    else
                    {
                        ftpPWord = ftpPWord.Trim();
                    }
                    if (String.IsNullOrWhiteSpace(ftpPath))
                    {
                        lost.Add("CTCB_FTP_PATH");
                    }
                    else
                    {
                        ftpPath = ftpPath.Trim();
                        if (ftpPath.EndsWith(@"/"))
                        {
                            ftpPath += @"/";
                        }
                    }

                    if (lost.Count > 0)
                    {
                        exitCode = -1;
                        exitMsg = String.Format("Config 參數錯誤，未設定 CTCB 的 FTP 相關 ({0}) 參數", lost .ToArray());
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                }
                #endregion
                #endregion

                #region 指示檔名稱
                string configFileName = null;
                if (exitCode == 0)
                {
                    configFileName = ConfigurationManager.AppSettings.Get("config_file_name");
                    if (String.IsNullOrEmpty(configFileName))
                    {
                        exitCode = -1;
                        exitMsg = "Config 參數錯誤，未設定指示檔名稱 (config_file_name) 參數";
                        jobLog.AppendLine(exitMsg);
                        log.AppendLine(exitMsg);
                    }
                }
                #endregion
                #endregion

                #region 新增處理中的 Job
                if (exitCode == 0)
                {
                    JobcubeEntity job = new JobcubeEntity();
                    job.Jtypeid = jobTypeId;
                    job.Jparam = String.Join(" ", args);
                    Result result = jobHelper.InsertProcessJob(ref job, jobCheckMode);
                    if (result.IsSuccess)
                    {
                        jobNo = job.Jno;
                        jobStamp = job.Memo;
                    }
                    else
                    {
                        exitCode = -2;
                        log.AppendLine(result.Message);
                    }
                }
                #endregion

                #region 處理資料
                if(exitCode == 0)
                {
                    string mutexName = "Global\\" + appGuid;
                    using (Mutex m = new Mutex(false, mutexName))    //全域不可重複執行
                    {
                        //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                        if (m.WaitOne(0, false))
                        {
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 開始處理資料", DateTime.Now).AppendLine();

                            #region 處理資料夾
                            {
                                string errmsg = CheckOrCreateFolder(tempPath, true);    //檢查並清空暫存檔資料夾
                                if (!String.IsNullOrEmpty(errmsg))
                                {
                                    exitCode = -1;
                                    exitMsg = String.Format("Config (BAK_PATH)參數錯誤，{0}", errmsg);
                                    jobLog.AppendLine(exitMsg);
                                    log.AppendLine(exitMsg);
                                }
                            }
                            #endregion

                            #region 產生中信語音檔
                            if (exitCode == 0)
                            {
                                string msg = null;
                                EntityFactory factory = new EntityFactory();
                                KeyValueList para = null;
                                DataTable school_data = new DataTable();
                                DataTable student_data = new DataTable();

                                #region 取得資料
                                string sql = "";
                                Result result;

                                #region 中國信託學校檔的學校名稱不串費用別名稱的商家代號清單字串
                                string specialValue = null;
                                result = GetCTCBSpecialConfigValue(factory, out specialValue);
                                if (!result.IsSuccess)
                                {
                                    msg = string.Format("讀取中國信託學校檔的學校名稱不串費用別名稱的商家代號清單字串發生錯誤，錯誤訊息={0}", result.Message);
                                    jobLog.AppendLine(msg);
                                    fileLog.WriteLog(msg);
                                    exitCode = -1;
                                    System.Environment.Exit(exitCode);
                                    return;
                                }
                                #endregion

                                #region 學生資料
                                sql = getStudentDataSQL();
                                result = factory.GetDataTable(sql, para, 0, 0, out student_data);
                                if (result.IsSuccess)
                                {
                                    if (student_data.Rows.Count > 0)
                                    {

                                    }
                                    else
                                    {
                                        msg = string.Format("讀取學生資料發生錯誤，sql={0}，錯誤訊息={1}", sql, "沒有符合的資料");
                                        jobLog.AppendLine(msg);
                                        fileLog.WriteLog(msg);
                                    }
                                }
                                else
                                {
                                    msg = string.Format("讀取學生資料發生錯誤，sql={0}，錯誤訊息={1}", sql, result.Message);
                                    jobLog.AppendLine(msg);
                                    fileLog.WriteLog(msg);
                                    exitCode = -1;
                                    System.Environment.Exit(exitCode);
                                    return;
                                }
                                #endregion

                                #region 學校資料
                                school_data = getSchoolData(student_data, specialValue);
                                #endregion

                                #endregion

                                #region 取 Queue_CTCB 資料
                                string sendStamp = null;
                                DataTable stuTable = null;
                                DataTable schTable = null;
                                result = GetQueueCTCB(factory, out sendStamp, out stuTable, out schTable);
                                if (!result.IsSuccess)
                                {
                                    msg = string.Format("讀取 Queue_CTCB 資料發生錯誤，錯誤訊息={0}", result.Message);
                                    jobLog.AppendLine(msg);
                                    fileLog.WriteLog(msg);
                                    exitCode = -1;
                                    System.Environment.Exit(exitCode);
                                    return;
                                }
                                #endregion

                                #region 組資料
                                string line = "";
                                Int32 count = 0;
                                Encoding encoding = Encoding.Default;

                                #region 學生檔
                                /*
                                 * 學生檔的內容
                                 */
                                #region 學生檔欄位說明 (59 bytes)(中文編碼：ANSI)
                                //序號  欄位名稱         欄位名稱    欄位型態    備註
                                //1     stu_id           學號        char(10)    文數字，左靠右補空白 (可以為空白)
                                //2     stu_name         姓名        char(20)    文字，左靠右補空白 (可以為空白)
                                //3     cancel_no        銷帳編號    char(20)    文數字，左靠右補空白(虛擬帳號)
                                //4     receive_amount   金額        char(7)     數字，右靠左補0
                                //5     CR               換行字元    char(2)     Carriage reuren & line feed (10,13)
                                #endregion

                                count = 0;

                                #region [MDY:20160106] 因為自訂銷帳編號第7碼不一定與費用別相同，所以改用銷編第7碼來組學校檔 (getSchoolData() 已處理所以這裡不用處理)
                                //List<string> customKeys = new List<string>(); //紀錄有自行傳銷帳編號的 Key (receive_type + year_id + term_id + dep_id + receive_id)
                                #endregion

                                string full_student_file_name = Path.Combine(tempPath, studentFileName);
                                try
                                {
                                    using (StreamWriter sw = new StreamWriter(full_student_file_name, false, Encoding.Default))
                                    {
                                        #region Student_Receive 的學生檔
                                        if (student_data != null && student_data.Rows.Count > 0)
                                        {
                                            foreach (DataRow row in student_data.Rows)
                                            {
                                                count++;
                                                string stu_id = Common.GetCutPadString(encoding, Convert.ToString(row["stu_id"]).Trim(), 10, Common.AlignCutPadEnum.Left, ' ');
                                                string stu_name = Common.GetCutPadString(encoding, Convert.ToString(row["stu_name"]).Trim(), 20, Common.AlignCutPadEnum.Left, ' ');
                                                string cancel_no = Common.GetCutPadString(encoding, Convert.ToString(row["cancel_no"]).Trim(), 20, Common.AlignCutPadEnum.Left, ' ');
                                                string receive_amount = Common.GetCutPadString(encoding, Convert.ToString(row["receive_amount"]).Trim(), 7, Common.AlignCutPadEnum.Right, '0');
                                                line = string.Concat(stu_id, stu_name, cancel_no, receive_amount);
                                                sw.WriteLine(line);

                                                #region [MDY:20160106] 因為自訂銷帳編號第7碼不一定與費用別相同，所以改用銷編第7碼來組學校檔 (getSchoolData() 已處理所以這裡不用處理)
                                                //#region 檢查是為自行上傳銷帳編號且第7碼與業務代碼(費用別最後一碼)不同的資料
                                                //string uploadFlag = row["UploadFlag"].ToString().Trim();
                                                //if (uploadFlag == "2" || uploadFlag == "3" || uploadFlag == "6" || uploadFlag == "7")
                                                //{
                                                //    string receiveId = row["receive_id"].ToString().Trim();

                                                //    #region 因為費用別擴增為兩碼，所以業務代碼只取費用別最後一碼
                                                //    if (receiveId.Length > 1)
                                                //    {
                                                //        receiveId = receiveId.Substring(receiveId.Length - 1);
                                                //    }
                                                //    #endregion

                                                //    if (cancel_no.Substring(6, 1) != receiveId)
                                                //    {
                                                //        string receiveType = row["receive_id"].ToString().Trim();
                                                //        string yearId = row["year_id"].ToString().Trim();
                                                //        string termId = row["term_id"].ToString().Trim();
                                                //        string depId = row["dep_id"].ToString().Trim();
                                                //        string key = String.Format("{0}_{1}_{2}_{3}_{4}", receiveType, yearId, termId, depId, receiveId);
                                                //        if (!customKeys.Contains(key))
                                                //        {
                                                //            customKeys.Add(key);
                                                //        }
                                                //    }
                                                //}
                                                //#endregion
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            count = 0;
                                        }
                                        #endregion

                                        #region Queue_CTCB 的學生檔
                                        if (stuTable != null && stuTable.Rows.Count > 0)
                                        {
                                            foreach (DataRow row in stuTable.Rows)
                                            {
                                                count++;
                                                string stu_id = Common.GetCutPadString(encoding, Convert.ToString(row["Stu_Id"]).Trim(), 10, Common.AlignCutPadEnum.Left, ' ');
                                                string stu_name = Common.GetCutPadString(encoding, Convert.ToString(row["Stu_Name"]).Trim(), 20, Common.AlignCutPadEnum.Left, ' ');
                                                string cancel_no = Common.GetCutPadString(encoding, Convert.ToString(row["Cancel_No"]).Trim(), 20, Common.AlignCutPadEnum.Left, ' ');
                                                string receive_amount = Common.GetCutPadString(encoding, Convert.ToString(row["Receive_Amount"]).Trim(), 7, Common.AlignCutPadEnum.Right, '0');
                                                line = string.Concat(stu_id, stu_name, cancel_no, receive_amount);
                                                sw.WriteLine(line);
                                            }
                                        }
                                        #endregion
                                    }
                                    msg = string.Format("學生檔共{0}筆資料", count);
                                    jobLog.AppendLine(msg);
                                }
                                catch (Exception ex)
                                {
                                    msg = string.Format("建立學生檔發生錯誤，檔案名稱={0}，錯誤訊息={1}", full_student_file_name, ex.Message);
                                    jobLog.AppendLine(msg);
                                    fileLog.WriteLog(msg);
                                }
                                #endregion

                                #region 學校檔
                                /*
                                 * 學校檔的內容
                                 */
                                #region 欄位說明 (96 bytes)(中文編碼：ANSI)
                                //序號   欄位名稱       欄位名稱             欄位型態    備註
                                //1      receive_type   商家代碼             char(4)     文數字，左靠右補空白(虛擬帳號前4碼)
                                //2      sch_name       學校名稱(+收費名稱)  char(54)    文字，左靠右補空白必須全部為全型字
                                //3      pay_date       繳款迄日             char(8)     YYYYMMDD
                                //4      receive_id     業務代碼             char(1)     數字
                                //5      Receive_Name   費用名稱             char(20)    目前請填空白
                                //6      Counter        繳費單總筆數         char(7)     數字，右靠左補空白 目前請填空白
                                //7      CR             換行字元             char(2)     Carriage reuren & line feed (10,13)
                                #endregion

                                #region 隱藏規則
                                //中信會檢查學校檔的業務代碼(receive_id)與學生檔銷帳編號的第7碼是否符合，
                                //因為自行上傳虛擬帳號者不一定會遵循銷帳編號規則，所以如果學生檔有自行上傳
                                //虛擬帳號的資料時，學校檔的業務代碼填空白
                                #endregion

                                count = 0;
                                string full_school_file_name = Path.Combine(tempPath, schoolFileName);
                                try
                                {
                                    using (StreamWriter sw = new StreamWriter(full_school_file_name, false, Encoding.Default))
                                    {
                                        #region Student_Receive 的學校檔
                                        if (school_data != null && school_data.Rows.Count > 0)
                                        {
                                            foreach (DataRow row in school_data.Rows)
                                            {
                                                count++;
                                                string receive_type = Common.GetCutPadString(encoding, Convert.ToString(row["receive_type"]).Trim(), 4, Common.AlignCutPadEnum.Left, ' ');
                                                string sch_name = Common.GetCutPadString(encoding, Convert.ToString(row["sch_name"]).Trim(), 54, Common.AlignCutPadEnum.Left, ' ');
                                                string pay_date = Common.GetCutPadString(encoding, Convert.ToString(row["pay_date"]).Trim(), 8, Common.AlignCutPadEnum.Left, ' ');
                                                string receive_id = Common.GetCutPadString(encoding, Convert.ToString(row["receive_id"]).Trim(), 1, Common.AlignCutPadEnum.Left, ' ');

                                                #region [MDY:20160106] 因為自訂銷帳編號第7碼不一定與費用別相同，所以改用銷編第7碼來組學校檔 (getSchoolData() 已處理所以這裡不用處理)
                                                //#region 因為費用別擴增為兩碼，所以業務代碼只取費用別最後一碼
                                                //if (receive_id.Length > 1)
                                                //{
                                                //    receive_id = receive_id.Substring(receive_id.Length - 1);
                                                //}
                                                //#endregion

                                                //#region 檢查是否為有自行傳銷帳編號的 Key (receive_type + year_id + term_id + dep_id + receive_id)
                                                //string receiveType = row["receive_id"].ToString().Trim();
                                                //string yearId = row["year_id"].ToString().Trim();
                                                //string termId = row["term_id"].ToString().Trim();
                                                //string depId = row["dep_id"].ToString().Trim();
                                                //string key = String.Format("{0}_{1}_{2}_{3}_{4}", receiveType, yearId, termId, depId, receive_id);
                                                //if (customKeys.Contains(key))
                                                //{
                                                //    //是則改為空白
                                                //    receive_id = " ";
                                                //}
                                                //#endregion
                                                #endregion

                                                string Receive_Name = "".PadLeft(20, ' ');
                                                string Counter = "".PadLeft(7, ' ');    //因為中信不處理所以用空白
                                                line = string.Concat(receive_type, sch_name, pay_date, receive_id, Receive_Name, Counter);
                                                sw.WriteLine(line);
                                            }
                                        }
                                        else
                                        {
                                            count = 0;
                                        }
                                        #endregion

                                        #region Queue_CTCB 的學校檔
                                        if (schTable != null && schTable.Rows.Count > 0)
                                        {
                                            #region [MDY:20160106] 改用銷編第7碼來組學校檔 (GetQueueCTCB() 已處理)
                                            //string receive_id = String.Empty;   //Queue_CTCB 沒有代收費用別
                                            #endregion

                                            string Receive_Name = "".PadLeft(20, ' ');
                                            foreach (DataRow row in schTable.Rows)
                                            {
                                                count++;

                                                #region [MDY:20160106] 改用銷編第7碼來組學校檔 (GetQueueCTCB() 已處理)
                                                string receive_id = Common.GetCutPadString(encoding, Convert.ToString(row["Receive_Id"]).Trim(), 1, Common.AlignCutPadEnum.Left, ' ');
                                                #endregion

                                                string receive_type = Common.GetCutPadString(encoding, Convert.ToString(row["Receive_Type"]).Trim(), 4, Common.AlignCutPadEnum.Left, ' ');
                                                string sch_name = Common.GetCutPadString(encoding, Convert.ToString(row["Sch_Name"]).Trim(), 54, Common.AlignCutPadEnum.Left, ' ');
                                                string pay_date = Common.GetCutPadString(encoding, Convert.ToString(row["Pay_Due_Date"]).Trim(), 8, Common.AlignCutPadEnum.Left, ' ');
                                                string Counter = "".PadLeft(7, ' ');    //因為中信不處理所以用空白
                                                line = string.Concat(receive_type, sch_name, pay_date, receive_id, Receive_Name, Counter);
                                                sw.WriteLine(line);
                                            }
                                        }
                                        #endregion
                                    }
                                    msg = string.Format("學校檔共{0}筆資料", count);
                                    jobLog.AppendLine(msg);
                                }
                                catch (Exception ex)
                                {
                                    msg = string.Format("建立學校檔發生錯誤，檔案名稱={0}，錯誤訊息={1}", full_school_file_name, ex.Message);
                                    jobLog.AppendLine(msg);
                                    fileLog.WriteLog(msg);
                                }

                                #endregion

                                #endregion

                                if (isUseZip)
                                {
                                    #region 傳壓縮檔
                                    #region 壓縮檔案
                                    string full_zip_file_name = Path.Combine(dataPath, zipFileName);
                                    #region 如果檔案存在先刪除
                                    if (System.IO.File.Exists(full_zip_file_name))
                                    {
                                        try
                                        {
                                            System.IO.File.Delete(full_zip_file_name);
                                        }
                                        catch (Exception ex)
                                        {
                                            msg = string.Format("刪除檔案{0}發生錯誤，錯誤訊息={1}", full_zip_file_name, ex.Message);
                                            jobLog.AppendLine(msg);
                                            fileLog.WriteLog(msg);
                                        }
                                    }
                                    #endregion
                                    string[] files = { "", "" };
                                    files[0] = full_student_file_name;
                                    files[1] = full_school_file_name;
                                    try
                                    {
                                        #region [MDY:20210401] 原碼修正
                                        Entities.ZIPHelper.ZipFiles(files, full_zip_file_name, zipPWord);
                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = string.Format("壓縮檔案{0}、{1}發生錯誤，錯誤訊息={2}", full_student_file_name, full_school_file_name, ex.Message);
                                        jobLog.AppendLine(msg);
                                        fileLog.WriteLog(msg);
                                    }

                                    string bak_full_zip_file_name = Path.Combine(bakPath, string.Format("{0}.{1}", zipFileName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                                    try
                                    {
                                        System.IO.File.Copy(full_zip_file_name, bak_full_zip_file_name);
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = string.Format("備份壓縮檔案{0}發生錯誤，錯誤訊息={1}", full_zip_file_name, ex.Message);
                                        jobLog.AppendLine(msg);
                                        fileLog.WriteLog(msg);
                                    }
                                    #endregion

                                    #region 指示檔
                                    #region [MDY:20210401] 原碼修正
                                    string ftp_host = ConfigurationManager.AppSettings.Get("CTCB_FTP_HOST");
                                    string ftp_port = ConfigurationManager.AppSettings.Get("CTCB_FTP_PORT");
                                    string ftp_uid = ConfigurationManager.AppSettings.Get("CTCB_FTP_UID");
                                    string ftp_pword = ConfigurationManager.AppSettings.Get("CTCB_FTP_PXX");
                                    string remote_path = ConfigurationManager.AppSettings.Get("CTCB_FTP_PATH");

                                    #region [MDY:20211001] M202110_01 支援 FTP/FTPS/SFTP (2021擴充案先做)
                                    string ftp_kind = ConfigurationManager.AppSettings.Get("CTCB_FTP_KIND");
                                    string cmd = string.Format("protocol={0} host={1} port={2} uid={3} pwd={4} remote_path={5} remote_file={6} local_file={7}",
                                        ftp_kind, ftp_host, ftp_port, ftp_uid, ftp_pword, remote_path, zipFileName, zipFileName);
                                    #endregion

                                    string config_file_name = ConfigurationManager.AppSettings.Get("config_file_name");
                                    string full_config_file_name = Path.Combine(dataPath, config_file_name);
                                    try
                                    {
                                        using (StreamWriter sw = new StreamWriter(full_config_file_name, false, Encoding.Default))
                                        {
                                            sw.Write(cmd);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = string.Format("建立指示檔發生錯誤，檔案名稱={0}，錯誤訊息={1}", full_config_file_name, ex.Message);
                                        jobLog.AppendLine(msg);
                                        fileLog.WriteLog(msg);
                                    }
                                    #endregion
                                    #endregion
                                    #endregion
                                }
                                else
                                {
                                    #region 傳原始資料檔
                                    #region 複製資料與備份
                                    {
                                        string copyFileName = null;
                                        try
                                        {
                                            string data_full_student_file_name = Path.Combine(dataPath, studentFileName);
                                            copyFileName = data_full_student_file_name;
                                            System.IO.File.Copy(full_student_file_name, data_full_student_file_name);

                                            string data_full_school_file_name = Path.Combine(dataPath, schoolFileName);
                                            copyFileName = data_full_school_file_name;
                                            System.IO.File.Copy(full_school_file_name, data_full_school_file_name);

                                            string bak_full_student_file_name = Path.Combine(bakPath, studentFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
                                            copyFileName = bak_full_student_file_name;
                                            System.IO.File.Copy(full_student_file_name, bak_full_student_file_name);

                                            string bak_full_school_file_name = Path.Combine(bakPath, schoolFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
                                            copyFileName = bak_full_school_file_name;
                                            System.IO.File.Copy(full_school_file_name, bak_full_school_file_name);
                                        }
                                        catch (Exception ex)
                                        {
                                            msg = string.Format("備份資料檔案{0}發生錯誤，錯誤訊息={1}", copyFileName, ex.Message);
                                            jobLog.AppendLine(msg);
                                            fileLog.WriteLog(msg);
                                        }
                                    }
                                    #endregion

                                    #region 指示檔
                                    {
                                        #region [MDY:20210401] 原碼修正
                                        string cmd1 = string.Format(" host={0} port={1} uid={2} pwd={3} remote_path={4} remote_file={5} local_file={6}", ftpHost, ftpPort, ftpUid, ftpPWord, ftpPath, studentFileName, studentFileName);
                                        string cmd2 = string.Format(" host={0} port={1} uid={2} pwd={3} remote_path={4} remote_file={5} local_file={6}", ftpHost, ftpPort, ftpUid, ftpPWord, ftpPath, schoolFileName, schoolFileName);
                                        #endregion

                                        string config_file_name = ConfigurationManager.AppSettings.Get("config_file_name");
                                        string full_config_file_name = Path.Combine(dataPath, config_file_name);
                                        try
                                        {
                                            using (StreamWriter sw = new StreamWriter(full_config_file_name, false, Encoding.Default))
                                            {
                                                sw.WriteLine(cmd1);
                                                sw.WriteLine(cmd2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            msg = string.Format("建立指示檔發生錯誤，檔案名稱={0}，錯誤訊息={1}", full_config_file_name, ex.Message);
                                            jobLog.AppendLine(msg);
                                            fileLog.WriteLog(msg);
                                        }
                                    }
                                    #endregion
                                    #endregion
                                }

                                #region ok檔
                                /*
                                 * ok檔用來判斷處理完畢
                                 */
                                string full_ok_file_name = Path.Combine(dataPath, "ok");
                                try
                                {
                                    using (StreamWriter sw = new StreamWriter(full_ok_file_name, false, Encoding.Default))
                                    {

                                    }
                                }
                                catch (Exception ex)
                                {
                                    msg = string.Format("寫ok檔發生錯誤，檔案名稱={0}，錯誤訊息={1}", full_ok_file_name, ex.Message);
                                    jobLog.AppendLine(msg);
                                    fileLog.WriteLog(msg);
                                }
                                #endregion

                                #region 更新狀態
                                foreach (DataRow row in student_data.Rows)
                                {
                                    string receive_type = Convert.ToString(row["receive_type"]).Trim();
                                    string year_id = Convert.ToString(row["year_id"]).Trim();
                                    string term_id = Convert.ToString(row["term_id"]).Trim();
                                    string dep_id = Convert.ToString(row["dep_id"]).Trim();
                                    string receive_id = Convert.ToString(row["receive_id"]).Trim();
                                    string stu_id = Convert.ToString(row["stu_id"]).Trim();
                                    string where = string.Format("receive_type='{0}' and year_id='{1}' and term_id='{2}' and dep_id='{3}' and receive_id='{4}'", receive_type, year_id, term_id, dep_id, receive_id);
                                    sql = string.Format("update student_receive set c_flag='1' where {0}", where);
                                    para = null;
                                    result = factory.ExecuteNonQuery(sql, para, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count > 0)
                                        {

                                        }
                                        else
                                        {
                                            msg = string.Format("更新student_receive失敗，key={0}，錯誤訊息={1}", where, "沒有資料被更新");
                                            jobLog.AppendLine(msg);
                                        }
                                    }
                                    else
                                    {
                                        msg = string.Format("更新student_receive發生錯誤，key={0}，錯誤訊息={1}", where, result.Message);
                                        jobLog.AppendLine(msg);
                                        fileLog.WriteLog(msg);
                                    }
                                }

                                result = SetQueueCTCBSendDate(factory, sendStamp);
                                if (!result.IsSuccess)
                                {
                                    msg = string.Format("更新 Queue_CTCB 上船中信日期發生錯誤，Send_Stamp={0}，錯誤訊息={1}", sendStamp, result.Message);
                                    jobLog.AppendLine(msg);
                                    fileLog.WriteLog(msg);
                                }
                                #endregion
                            }
                            #endregion

                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 結束處理資料", DateTime.Now).AppendLine();

                            #region [MDY:20210401] 原碼修正
                            m.ReleaseMutex();
                            #endregion
                        }
                        else
                        {
                            #region [MDY:20210401] 不重複執行也要有日誌資訊
                            exitCode = -5;  //不重複執行回傳代碼
                            exitMsg = String.Format("執行緒中已存在 {0}，不重複執行", mutexName);
                            jobLog.AppendLine(exitMsg);
                            log.AppendLine(exitMsg);
                            #endregion
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -9;
                exitMsg = String.Format("{0} 處理失敗，錯誤訊息：{1}；", jobTypeName, ex.Message);
                jobLog.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理 {1} 作業發生例外，錯誤訊息：{2}", DateTime.Now, jobTypeName, ex.Message).AppendLine();
                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 發生例外，參數：{1}，錯誤訊息：{2}", DateTime.Now, String.Join(" ", args), ex.Message).AppendLine();
            }
            finally
            {
                #region 更新 Job
                if (jobNo > 0)
                {
                    string jobResultId = null;
                    if (exitCode == 0)
                    {
                        jobResultId = JobCubeResultCodeTexts.SUCCESS;
                    }
                    else
                    {
                        jobResultId = JobCubeResultCodeTexts.FAILURE;
                    }

                    Result result = jobHelper.UpdateProcessJobToFinsh(jobNo, jobStamp, jobResultId, exitMsg, jobLog.ToString());
                    if (!result.IsSuccess)
                    {
                        log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 更新批次處理佇列為已完成失敗，{1}", DateTime.Now, result.Message).AppendLine();
                    }
                }
                jobHelper.Dispose();
                jobHelper = null;
                #endregion

                log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] {1} 結束", DateTime.Now, appName).AppendLine();

                fileLog.WriteLog(log);
            }

            System.Environment.Exit(exitCode);
        }
    }
}
