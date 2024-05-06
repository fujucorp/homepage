using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fuju;
using Fuju.DB;
using System.Configuration;
using Entities;
using System.IO;
using System.Data;
using System.Data.SqlClient;
namespace GenTaxData
{
    class Program
    {
        #region Member
        //private string _HostName = System.Environment.MachineName.Trim();
        #endregion

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

        private static Int32 LengthByChar(string data)
        {
            return System.Text.Encoding.Default.GetBytes(data).Length;
        }

        private static string cutString(string data, Int32 len)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(data);
            return System.Text.Encoding.Default.GetString(buff, 0, len);
        }

        private static string fixString(string data, Int32 len, string left_right, char fill)
        {
            if(LengthByChar(data)>=len)
            {
                return cutString(data, len);
            }
            else
            {
                if(left_right=="left")
                {
                    return cutString(data.PadLeft(len, fill), len);
                }
                else
                {
                    return cutString(data.PadRight(len, fill), len);
                }
            }
        }

        private static string makeTAXRecord(string sch_id, string stu_no, string id, string pay_date, string stu_name, string year_term, string sch_level, string degree, string edu_amt, string stay_amt, string remark)
        {
            return fixString(sch_id, 6, "left", '0') + fixString(stu_no, 12, "right", ' ') + fixString(id, 10, "left", ' ') + fixString(pay_date, 7, "left", '0') + fixString(stu_name, 12, "right", ' ') + fixString(year_term, 4, "left", '0') + fixString(sch_level, 2, "right", ' ') + fixString(degree, 2, "left", '0') + fixString(edu_amt, 10, "left", '0') + fixString(stay_amt, 10, "left", '0') + fixString(remark, 30, "right", ' ');
        }

        private static DataTable buildTB1()
        {
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn("receive_type", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("school_id", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("school_name", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("school_level", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            return dt;
        }

        private static DataTable buildtbTAX()
        {
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn("school_id", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("stu_id", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("id", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("receive_date", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("stu_name", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("year_term", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("school_level", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("degree", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("edu_amt", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("stay_amt", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            col = new DataColumn("remark", System.Type.GetType("System.String"));
            dt.Columns.Add(col);
            return dt;
        }

        private static string[] splitFlag(string data)
        {
            string[] flag={"N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N","N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N"};
            if(data==null)
            {
                return flag;
            }
            if(data.Trim().Length!=30)
            {
                return flag;
            }
            for(int i=0;i<30;i++)
            {
                flag[i]=data.Substring(i,1);
            }
            return flag;
        }

        private static bool checkID(string personal_id)
        {
            return Fuju.Common.IsPersonalID2(personal_id);
        }

        private static bool checkLEVEL(string  school_level , string stu_grade )
        {
            if(school_level=="5")
            {
                if(Convert.ToInt16(stu_grade)<=3)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true; 
            }
        }

        private static DataSet getStudentReceive(string school_level,string receive_date_start, string receive_date_end, string receive_type_list
            , out string msg, out bool isOK)
        {
            msg = "";
            isOK = true;
            DataSet datas = new DataSet();
            DataTable tax_data = buildtbTAX();
            tax_data.TableName = "tax_success_data";
            DataRow tax_rowdata = null;
            DataTable tax_fail_data = buildtbTAX();
            tax_fail_data.TableName = "tax_fail_data";
            string log = "";
            StringBuilder logs = new StringBuilder();

            #region 組sql指令
            string sql = @"
select (a.receive_01+isnull(e.loan_01,0)) as 'receive_01' 
     , (a.receive_02+isnull(e.loan_02,0)) as 'receive_02' 
     , (a.receive_03+isnull(e.loan_03,0)) as 'receive_03' 
     , (a.receive_04+isnull(e.loan_04,0)) as 'receive_04' 
     , (a.receive_05+isnull(e.loan_05,0)) as 'receive_05' 
     , (a.receive_06+isnull(e.loan_06,0)) as 'receive_06' 
     , (a.receive_07+isnull(e.loan_07,0)) as 'receive_07' 
     , (a.receive_08+isnull(e.loan_08,0)) as 'receive_08' 
     , (a.receive_09+isnull(e.loan_09,0)) as 'receive_09' 
     , (a.receive_10+isnull(e.loan_10,0)) as 'receive_10' 
     , (a.receive_11+isnull(e.loan_11,0)) as 'receive_11' 
     , (a.receive_12+isnull(e.loan_12,0)) as 'receive_12' 
     , (a.receive_13+isnull(e.loan_13,0)) as 'receive_13' 
     , (a.receive_14+isnull(e.loan_14,0)) as 'receive_14' 
     , (a.receive_15+isnull(e.loan_15,0)) as 'receive_15' 
     , (a.receive_16+isnull(e.loan_16,0)) as 'receive_16' 
     , (a.receive_17+isnull(e.loan_17,0)) as 'receive_17' 
     , (a.receive_18+isnull(e.loan_18,0)) as 'receive_18' 
     , (a.receive_19+isnull(e.loan_19,0)) as 'receive_19' 
     , (a.receive_20+isnull(e.loan_20,0)) as 'receive_20' 
     , (a.receive_21+isnull(e.loan_21,0)) as 'receive_21' 
     , (a.receive_22+isnull(e.loan_22,0)) as 'receive_22' 
     , (a.receive_23+isnull(e.loan_23,0)) as 'receive_23' 
     , (a.receive_24+isnull(e.loan_24,0)) as 'receive_24' 
     , (a.receive_25+isnull(e.loan_25,0)) as 'receive_25' 
     , (a.receive_26+isnull(e.loan_26,0)) as 'receive_26' 
     , (a.receive_27+isnull(e.loan_27,0)) as 'receive_27' 
     , (a.receive_28+isnull(e.loan_28,0)) as 'receive_28' 
     , (a.receive_29+isnull(e.loan_29,0)) as 'receive_29' 
     , (a.receive_30+isnull(e.loan_30,0)) as 'receive_30' 
     , a.year_id, a.stu_grade, a.stu_id, a.receive_date, a.term_id, a.receive_type, a.dep_id, a.receive_id 
     , b.edu_tax, b.stay_tax, b.sch_level, c.id_number, c.stu_name, d.sch_id, d.sch_name 
  from student_receive a 
 inner join school_rid b on b.receive_type=a.receive_type and b.year_id=a.year_id and b.term_id=a.term_id and b.dep_id=a.dep_id and b.receive_id=a.receive_id 
 inner join student_master c on c.receive_type=a.receive_type and c.dep_id=a.dep_id and c.stu_id=a.stu_id 
 inner join school_rtype d on d.receive_type=a.receive_type 
  left outer join student_loan e on e.receive_type=a.receive_type and e.year_id=a.year_id and e.term_id=a.term_id and e.dep_id=a.dep_id and e.receive_id=a.receive_id and e.stu_id=a.stu_id and e.loan_id=a.loan_id 
 where 1=1 
   --and b.enabled_tax='Y' and b.sch_level<>'' 
   and b.enabled_tax='Y' 
   and c.id_number <> '' and c.id_number is not null 
   and a.receive_date >= @SDATE and a.receive_date <= @DATE 
   and a.receive_type in (" + receive_type_list + @") ";
            #endregion

            DataTable data = new DataTable();
            KeyValue[] fieldValues = new KeyValue[] {
                new KeyValue("@SDATE", receive_date_start),
                new KeyValue("@DATE", receive_date_end)
            };
            Result result = null;
            using (Fuju.DB.Data.EntityFactory factory = new Fuju.DB.Data.EntityFactory())
            {
                result = factory.GetDataTable(sql, fieldValues, 0, 0, out data);
            }
            if (result.IsSuccess)
            {
                if (data != null && data.Rows.Count > 0)
                {
                    #region 有值
                    double edu_amount=0;
                    double stay_amount=0;
                    Int32 dup_count=0;
                    Int32 fail_count = 0;
                    Int32 success_count = 0;
                    bool correctID = false;
                    bool correctLevel = false;
                    string[] receive_item_list={"receive_01", "receive_02", "receive_03", "receive_04", "receive_05", "receive_06", "receive_07", "receive_08", "receive_09", "receive_10", "receive_11", "receive_12", "receive_13", "receive_14", "receive_15", "receive_16","receive_17", "receive_18", "receive_19", "receive_20", "receive_21", "receive_22", "receive_23", "receive_24", "receive_25", "receive_26", "receive_27", "receive_28", "receive_29", "receive_30"};

                    foreach (DataRow row in data.Rows)
                    {
                        try
                        {
                            #region 逐筆處理
                            edu_amount = 0;
                            stay_amount = 0;

                            #region 判斷該收入科目是否是屬於可扣抵學雜費
                            string[] flag = splitFlag(Convert.ToString(row["edu_tax"]));
                            for (int j = 0; j < 30; j++)
                            {
                                if (flag[j] == "Y")
                                {
                                    edu_amount = row.IsNull(receive_item_list[j]) ? 0 : Convert.ToDouble(row[receive_item_list[j]]);
                                }
                            }
                            #endregion

                            #region 判斷該收入科目是否是屬於可扣抵住宿費
                            flag = splitFlag(Convert.ToString(row["stay_tax"]));
                            for (int j = 0; j < 30; j++)
                            {
                                if (flag[j] == "Y")
                                {
                                    stay_amount = row.IsNull(receive_item_list[j]) ? 0 : Convert.ToDouble(row[receive_item_list[j]]);
                                }
                            }
                            #endregion

                            #region [MDY:20210401] 原碼修正
                            string selectstring = String.Format("school_id='{0}' and stu_id='{1}' and id='{2}' and receive_date='{3}'",
                                                                  Convert.ToString(row["sch_id"]).Replace("'", ""),
                                                                  Convert.ToString(row["stu_id"]).Replace("'", ""),
                                                                  Convert.ToString(row["id_number"]).Replace("'", ""),
                                                                  Convert.ToString(row["receive_date"]).Replace("'", ""));
                            #endregion

                            DataRow[] dup_rows = tax_data.Select(selectstring);
                            if (dup_rows != null && dup_rows.Length > 0)
                            {
                                #region 要合併資料
                                double amt=0;
                                amt=Convert.ToDouble(dup_rows[0]["edu_amt"])+edu_amount;
                                dup_rows[0]["edu_amt"]=amt.ToString("0");

                                amt = 0;
                                amt=Convert.ToDouble(dup_rows[0]["stay_amt"])+stay_amount;
                                dup_rows[0]["stay_amt"]=amt.ToString("0");
                                dup_count++;
                                #endregion
                            }
                            else
                            {
                                #region
                                correctID = checkID(Convert.ToString(row["id_number"]));
                                if(!correctID)
                                {
                                    log = String.Format("學號：{0}身分證字號有誤，身分證字號：{1}", Convert.ToString(row["stu_id"]), Convert.ToString(row["id_number"]));
                                    logs.AppendLine(log);
                                }
                                correctLevel = checkLEVEL(school_level, Convert.ToString(row["stu_grade"]));
                                if(!correctLevel)
                                {
                                    log = String.Format("學號：{0}所屬學制有誤，年級：{1}", Convert.ToString(row["stu_id"]), Convert.ToString(row["stu_grade"]));
                                    logs.AppendLine(log);
                                }

                                if(correctID && correctLevel)
                                {
                                    if(edu_amount==0 && stay_amount==0)
                                    {
                                        log = String.Format("學號：{0}申報金額為0，無需申報", Convert.ToString(row["stu_id"]));
                                        logs.AppendLine(log);

                                        #region 金額為0不須申報
                                        tax_rowdata = tax_fail_data.NewRow();
                                        tax_rowdata["school_id"] = Convert.ToString(row["sch_id"]);
                                        tax_rowdata["stu_id"] = Convert.ToString(row["stu_id"]);
                                        tax_rowdata["id"] = Convert.ToString(row["id_number"]);
                                        tax_rowdata["receive_date"] = Convert.ToString(row["receive_date"]);
                                        tax_rowdata["stu_name"] = Convert.ToString(row["stu_name"]);
                                        tax_rowdata["year_term"] = Convert.ToString(row["year_id"]).Trim().PadLeft(3, '0') + Convert.ToString(row["term_id"]).Trim();
                                        tax_rowdata["school_level"] = Convert.ToString(row["sch_level"]);
                                        tax_rowdata["degree"] = Convert.ToString(row["stu_grade"]);
                                        tax_rowdata["edu_amt"] = edu_amount.ToString();
                                        tax_rowdata["stay_amt"] = stay_amount.ToString();
                                        tax_rowdata["remark"] = "";
                                        tax_fail_data.Rows.Add(tax_rowdata);
                                        fail_count += 1;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 資料正確
                                        tax_rowdata = tax_data.NewRow();
                                        tax_rowdata["school_id"] = Convert.ToString(row["sch_id"]);
                                        tax_rowdata["stu_id"] = Convert.ToString(row["stu_id"]);
                                        tax_rowdata["id"] = Convert.ToString(row["id_number"]);
                                        tax_rowdata["receive_date"] = Convert.ToString(row["receive_date"]);
                                        tax_rowdata["stu_name"] = Convert.ToString(row["stu_name"]);
                                        tax_rowdata["year_term"] = Convert.ToString(row["year_id"]).Trim().PadLeft(3, '0') + Convert.ToString(row["term_id"]).Trim();
                                        tax_rowdata["school_level"] = Convert.ToString(row["sch_level"]);
                                        tax_rowdata["degree"] = Convert.ToString(row["stu_grade"]);
                                        tax_rowdata["edu_amt"] = edu_amount.ToString();
                                        tax_rowdata["stay_amt"] = stay_amount.ToString();
                                        tax_rowdata["remark"] = "";
                                        tax_data.Rows.Add(tax_rowdata);
                                        success_count += 1;
                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region 檢核錯誤不需申報
                                    tax_rowdata = tax_fail_data.NewRow();
                                    tax_rowdata["school_id"] = Convert.ToString(row["sch_id"]);
                                    tax_rowdata["stu_id"] = Convert.ToString(row["stu_id"]);
                                    tax_rowdata["id"] = Convert.ToString(row["id_number"]);
                                    tax_rowdata["receive_date"] = Convert.ToString(row["receive_date"]);
                                    tax_rowdata["stu_name"] = Convert.ToString(row["stu_name"]);
                                    tax_rowdata["year_term"] = Convert.ToString(row["year_id"]).Trim().PadLeft(3, '0') + Convert.ToString(row["term_id"]).Trim();
                                    tax_rowdata["school_level"] = Convert.ToString(row["sch_level"]);
                                    tax_rowdata["degree"] = Convert.ToString(row["stu_grade"]);
                                    tax_rowdata["edu_amt"] = edu_amount.ToString();
                                    tax_rowdata["stay_amt"] = stay_amount.ToString();
                                    tax_rowdata["remark"] = "";
                                    tax_fail_data.Rows.Add(tax_rowdata);
                                    fail_count += 1;
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        }
                        catch(Exception ex)
                        {
                            isOK = false;
                            log = String.Format("處理學生繳費單資料發生錯誤，錯誤訊息：{0}", ex.Message);
                            logs.AppendLine(log);
                        }
                    }
                    //寫log
                    #endregion
                }
                else
                {
                    log = String.Format("查無符合的學生繳費單資料，sql指令：{0}", sql);
                    logs.AppendLine(log);
                }
            }
            else
            {
                isOK = false;
                log = String.Format("讀取學生繳費單資料發生錯誤，sql指令：{0}，錯誤訊息：{1}", sql, result.Message);
                logs.AppendLine(log);
            }

            datas.Tables.Add(tax_data);
            datas.Tables.Add(tax_fail_data);
            msg = log.ToString();
            return datas;
        }

        static void Main(string[] args)
        {
            #region Initial
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            FileLoger fileLog = new FileLoger(appName);
            string tempPath = null;
            {
                tempPath = ConfigurationManager.AppSettings.Get("TEMP_PATH");
                if (!String.IsNullOrWhiteSpace(tempPath))
                {
                    tempPath = tempPath.Trim();
                    try
                    {
                        DirectoryInfo info = new DirectoryInfo(tempPath);
                        if (!info.Exists)
                        {
                            info.Create();
                        }
                    }
                    catch (Exception)
                    {
                        tempPath = null;
                    }
                }
                if (String.IsNullOrEmpty(tempPath))
                {
                    tempPath = Path.GetTempPath();
                }
            }
            int exitCode = 0;
            #endregion

            #region 處理參數
            //無參數
            #endregion

            #region 處理 SGG(報稅) 待處理序列資料
            string jobcubeType = JobCubeTypeCodeTexts.SGG;
            //string stamp = Guid.NewGuid().ToString("N");
            string stamp = null;
            int jobNo = 0;
            DateTime endTime = DateTime.Now;
            string errmsg = null;
            StringBuilder log = new StringBuilder();
            try
            {
                #region 取得一筆待處理的 job
                JobcubeEntity job = null;
                {
                    Result result = null;
                    using (JobCubeHelper jobHelper = new JobCubeHelper())
                    {
                        result = jobHelper.GetWaitJobToProcess(jobcubeType, out job);
                    }
                    if (result.IsSuccess)
                    {
                        if (job == null)
                        {
                            exitCode = -1;
                            fileLog.WriteDebugLog("查無待處理的 {0} 批次處理佇列資料", jobcubeType);
                            System.Environment.Exit(exitCode);
                            return;
                        }
                    }
                    else
                    {
                        exitCode = -2;
                        errmsg = String.Format("查詢待處理的 {0} 批次處理佇列資料發生錯誤，錯誤訊息：{1}", jobcubeType, result.Message);
                        fileLog.WriteLog(errmsg);
                        System.Environment.Exit(exitCode);
                        return;
                    }
                }
                #endregion

                #region 轉出資料
                if (job != null)
                {
                    jobNo = job.Jno;
                    stamp = job.Memo;
                    //jparam=sch_pid,sch_id,y,receive_types,sch_level,proc_dat。   receive_types=receive_type;receive_type;...
                    string[] para = job.Jparam.Split(',');
                    if (para == null || para.Length != 6)
                    {
                        exitCode = -3;
                        errmsg = String.Format("處理待產生報稅資料發生錯誤，jno={0}，jparam={1}，錯誤訊息：{1}", jobNo, job.Jparam, "jparam錯誤");
                        log.AppendLine(errmsg);
                        fileLog.WriteLog(errmsg);
                        //System.Environment.Exit(exitCode);
                        return;
                    }

                    string sch_pid = para[0].Trim();//學校統編
                    string sch_id = para[1].Trim();//學校代碼
                    string[] receive_types = para[3].Replace(" ", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);  //哪幾個代收類別
                    string school_level = para[4].Trim();
                    string proc_date = para[5].Trim();
                    if (receive_types == null || receive_types.Length == 0)
                    {
                        exitCode = -4;
                        errmsg = String.Format("處理待產生報稅資料發生錯誤，jno={0}，receive_types={1}，錯誤訊息：{1}", jobNo, para[3], "未選擇任何業務別碼");
                        log.AppendLine(errmsg);
                        fileLog.WriteLog(errmsg);
                        //System.Environment.Exit(exitCode);
                        return;
                    }
                    string receive_type_list = String.Format("'{0}'", String.Join("','", receive_types));
                    //foreach(string receive_type in receive_types)
                    //{
                    //    if (!String.IsNullOrWhiteSpace(receive_type))
                    //    {
                    //        receive_type_list += (receive_type_list == "" ? "" : ",") + "'" + receive_type + "'";
                    //    }
                    //}
                    string pay_year = para[2].Trim();//會計年度，民國年3碼
                    string pay_date_start = string.Format("{0}0101",pay_year);
                    string pay_date_end = string.Format("{0}1231", pay_year);

                    #region 取得繳款資料
                    DataSet datas = new DataSet();
                    string logmsg = null;
                    bool isGetDataOK = false;
                    datas = getStudentReceive(school_level, pay_date_start, pay_date_end, receive_type_list, out logmsg, out isGetDataOK);
                    fileLog.WriteLog(logmsg);
                    if (!isGetDataOK)
                    {
                        log.AppendLine(logmsg);
                    }
                    if (datas == null)
                    {
                        exitCode = -5;
                        errmsg = String.Format("取得繳費單資料發生錯誤");
                        fileLog.WriteLog(errmsg);
                        //System.Environment.Exit(exitCode);
                        return;
                    }
                    DataTable tax_data = datas.Tables["tax_success_data"];
                    DataTable tax_fail_data = datas.Tables["tax_fail_data"];
                    string line = "";
                    StringBuilder detail = new StringBuilder();
                    StringBuilder fail_detail = new StringBuilder();
                    foreach (DataRow row in tax_data.Rows)
                    {
                        line = makeTAXRecord(sch_id,
                                             Convert.ToString(row["stu_id"]).Trim(),
                                             Convert.ToString(row["id"]).Trim(),
                                             Convert.ToString(row["receive_date"]).Trim(),
                                             Convert.ToString(row["stu_name"]).Trim(),
                                             Convert.ToString(row["year_term"]).Trim(),
                                             Convert.ToString(row["school_level"]).Trim(),
                                             Convert.ToString(row["degree"]).Trim(),
                                             Convert.ToString(row["edu_amt"]).Trim(),
                                             Convert.ToString(row["stay_amt"]).Trim(),
                                             Convert.ToString(row["remark"]).Trim());
                        detail.AppendLine(line);
                    }
                    foreach(DataRow row in tax_fail_data.Rows)
                    {
                        line = makeTAXRecord(sch_id,
                                             Convert.ToString(row["stu_id"]).Trim(),
                                             Convert.ToString(row["id"]).Trim(),
                                             Convert.ToString(row["receive_date"]).Trim(),
                                             Convert.ToString(row["stu_name"]).Trim(),
                                             Convert.ToString(row["year_term"]).Trim(),
                                             Convert.ToString(row["school_level"]).Trim(),
                                             Convert.ToString(row["degree"]).Trim(),
                                             Convert.ToString(row["edu_amt"]).Trim(),
                                             Convert.ToString(row["stay_amt"]).Trim(),
                                             Convert.ToString(row["remark"]).Trim());
                        fail_detail.AppendLine(line);
                    }

                    #region 儲存報稅檔案
                    string file_name = string.Format("{0}.13.{1}", pay_year, sch_pid);
                    string full_file_name = string.Format("{0}{1}", (tempPath.EndsWith(@"\") ? tempPath : tempPath + @"\"), file_name);
                    string okFileFullName = full_file_name;
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(full_file_name, false, Encoding.Default))
                        {
                            sw.Write(detail.ToString());
                        }
                    }
                    catch(Exception ex1)
                    {
                        exitCode = -6;
                        errmsg = String.Format("儲存報稅檔案時發生錯誤，jno={0}，file name={1}，錯誤訊息：{1}", jobNo, full_file_name, ex1.Message);
                        log.AppendLine(errmsg);
                        fileLog.WriteLog(errmsg);
                        //System.Environment.Exit(exitCode);
                        return;
                    }
                    #endregion

                    #region 儲存報稅錯誤檔案
                    file_name = string.Format("{0}.13.{1}.fail", pay_year, sch_pid);
                    full_file_name = string.Format("{0}{1}", (tempPath.EndsWith(@"\") ? tempPath : tempPath + @"\"), file_name);
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(full_file_name, false, Encoding.Default))
                        {
                            sw.Write(detail.ToString());
                        }
                    }
                    catch (Exception ex1)
                    {
                        exitCode = -7;
                        errmsg = String.Format("儲存報稅錯誤檔案時發生錯誤，jno={0}，file name={1}，錯誤訊息：{1}", jobNo, full_file_name, ex1.Message);
                        log.AppendLine(errmsg);
                        fileLog.WriteLog(errmsg);
                        //System.Environment.Exit(exitCode);
                        return;
                    }
                    #endregion

                    #endregion

                    #region 檔案寫到bankPM等候下載
                    Result result = null;
                    try
                    {
                        int count = 0;
                        BankpmEntity bankpm = new BankpmEntity();
                        bankpm.ReceiveType = sch_pid;
                        bankpm.Filename = string.Format("{0}.13.{1}", pay_year, sch_pid);
                        bankpm.Filedetail = string.Format("{0},{1},{2},{3}", "SGH", sch_pid, pay_year, proc_date);
                        bankpm.Status = "0";
                        bankpm.Cdate = DateTime.Now.ToString("yyyy/MM/dd");
                        bankpm.Udate = DateTime.Now.ToString("yyyy/MM/dd");
                        bankpm.Tempfile = File.ReadAllBytes(okFileFullName);
                        if (bankpm.Tempfile == null || bankpm.Tempfile.Length == 0)
                        {
                            bankpm.Status = "E";
                        }
                        using (Fuju.DB.Data.EntityFactory factory = new Fuju.DB.Data.EntityFactory())
                        {
                            result = factory.Insert(bankpm, out count);
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new Result(false, ex.Message, CoreStatusCode.UNKNOWN_EXCEPTION, ex);
                    }
                    if(!result.IsSuccess)
                    {
                        exitCode = -7;
                        errmsg = String.Format("寫入bankpm時發生錯誤，錯誤訊息：{0}", result.Message);
                        log.AppendLine(errmsg);
                        fileLog.WriteLog(errmsg);
                        //System.Environment.Exit(exitCode);
                        return;
                    }
                    #endregion

                }
                #endregion
            }
            catch (Exception ex)
            {
                exitCode = -8;
                errmsg = String.Format("執行處理程序發生例外，錯誤訊息：{0}", ex.Message);
                log.AppendLine("執行處理程序發生例外");
                fileLog.WriteLog(errmsg);
            }
            finally
            {
                #region [Old]
                //#region 更新 job 處理結果 (避免資料更新臨時失敗，造成狀態未更新，試 3 次 每次睡 10 秒)
                //if (jobNo > 0)
                //{
                //    int sleepTimes = 3;
                //    int sleepSecond = 1000 * 10;
                //    StringBuilder updateError = new StringBuilder();
                //    using (JobCubeHelper jobHelper = new JobCubeHelper())
                //    {
                //        Result result = null;
                //        for (int timesNo = 1; timesNo <= sleepTimes; timesNo++)
                //        {
                //            if (String.IsNullOrEmpty(errmsg))
                //            {
                //                result = jobHelper.UpdateProcessJobToFinsh(jobNo, stamp, JobCubeResultCodeTexts.SUCCESS, JobCubeResultCodeTexts.SUCCESS_TEXT, log.ToString());
                //            }
                //            else
                //            {
                //                result = jobHelper.UpdateProcessJobToFinsh(jobNo, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, log.ToString());
                //            }
                //            if (result.IsSuccess)
                //            {
                //                break;
                //            }
                //            else
                //            {
                //                updateError.AppendFormat("第 {0} 次更新編號 {1} 的處理中批次處理佇列資料為已結束發生錯誤，錯誤訊息：{2}", timesNo, jobNo, result.Message).AppendLine();
                //                if (result.Code == ErrorCode.D_DATA_NOT_FOUND || result.Code == ErrorCode.S_INVALID_FACTORY)
                //                {
                //                    break;
                //                }
                //            }
                //            System.Threading.Thread.Sleep(sleepSecond);
                //        }
                //    }
                //    if (updateError.Length > 0)
                //    {
                //        fileLog.WriteLog(updateError.ToString());
                //    }
                //}
                //#endregion
                #endregion

                #region 更新 Job
                if (jobNo > 0)
                {
                    using (JobCubeHelper jobHelper = new JobCubeHelper())
                    {
                        Result result = null;
                        if (String.IsNullOrEmpty(errmsg))
                        {
                            result = jobHelper.UpdateProcessJobToFinsh(jobNo, stamp, JobCubeResultCodeTexts.SUCCESS, JobCubeResultCodeTexts.SUCCESS_TEXT, log.ToString());
                        }
                        else
                        {
                            result = jobHelper.UpdateProcessJobToFinsh(jobNo, stamp, JobCubeResultCodeTexts.FAILURE, errmsg, log.ToString());
                        }
                        if (!result.IsSuccess)
                        {
                            log.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 更新批次處理佇列為已完成失敗，{1}", DateTime.Now, result.Message).AppendLine();
                        }
                    }
                }
                #endregion
            }
            #endregion

            System.Environment.Exit(exitCode);
            return;
        }
    }
}
