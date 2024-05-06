using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    public sealed class HistoryHelper
    {
        /// <summary>
        /// 代收費用型態，土銀不使用，固定設為 1
        /// </summary>
        private string _ReceiveStatus = "1";

        private string _LogPath = null;

        #region Constructor
        public HistoryHelper(string logPath)
        {
            _LogPath = logPath == null ? null : logPath.Trim();
        }
        #endregion

        #region Method
        /// <summary>
        /// 移動線上資料到歷史資料表
        /// </summary>
        /// <param name="checkDate"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        public string MoveOnlineData(DateTime checkDate, out string resultMsg)
        {
            #region 取各學校商家代號線上資料保留學年數
            KeyValueList<int> schoolDataYears = null;
            {
                #region [MDY:2018xxxx] 配合新的保留學年數設定介面 (可指定單一商家代號) 修改 SQL
                #region [OLD]
//                string sql = @"SELECT Receive_Type, Keep_Data_Year FROM
//(
//SELECT Sch_Identy, Receive_Type
//     , (SELECT MAX(Keep_Data_Year) FROM School_Rtype AS B WHERE B.Sch_Identy = A.Sch_Identy) Keep_Data_Year
//  FROM School_Rtype AS A
//) AS T
// WHERE Keep_Data_Year IS NOT NULL AND Keep_Data_Year > 0
// ORDER BY Sch_Identy, Receive_Type";
                #endregion

                string sql = @"
SELECT Sch_Identy, Receive_Type, Keep_Data_Year
  FROM School_Rtype
 WHERE Keep_Data_Year IS NOT NULL AND Keep_Data_Year > 0
   AND status = '" + DataStatusCodeTexts.NORMAL + @"' 
 ORDER BY Sch_Identy, Receive_Type";
                #endregion

                KeyValue[] parameters = null;
                DataTable dt = null;
                Result result = null;
                using (EntityFactory factroy = new EntityFactory())
                {
                    result = factroy.GetDataTable(sql, parameters, 0, 0, out dt);
                }
                if (result.IsSuccess)
                {
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        resultMsg = "查無任何有效線的線上資料保留學年數設定資料";
                        return null;
                    }
                    else
                    {
                        schoolDataYears = new KeyValueList<int>(dt.Rows.Count);
                        foreach (DataRow row in dt.Rows)
                        {
                            schoolDataYears.Add(row["Receive_Type"].ToString().Trim(), Int32.Parse(row["Keep_Data_Year"].ToString()));
                        }
                    }
                }
                else
                {
                    resultMsg = String.Format("查詢學校的線上資料保留學年數設定資料失敗，錯誤訊息：{0}", result.Message);
                    return result.Message;
                }
            }
            #endregion

            StringBuilder msg = new StringBuilder();
            if (schoolDataYears != null && schoolDataYears.Count > 0)
            {
                foreach(KeyValue<int> schoolDataYear in schoolDataYears)
                {
                    string receiveType = schoolDataYear.Key;
                    int keepDataYear = schoolDataYear.Value;
                    string keepMinYearId = (checkDate.Year - 1911 - keepDataYear).ToString("000");  //要轉民國年
                    string logFile = String.IsNullOrEmpty(_LogPath) ? null : Path.Combine(_LogPath, String.Format("{0}_{1:yyyyMMddHHmmss}.log", receiveType, DateTime.Now));
                    Int32 totlaCount = 0;
                    Int32 okCount = 0;
                    Int32 failCount = 0;
                    string errmsg = this.MoveOnlineDataByReceiveType(receiveType, keepMinYearId, logFile, out totlaCount, out okCount, out failCount);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        msg.AppendFormat("處理商家代號 {0} 小於 {1} 學年的線上資料完成，共 {2} 筆資料，成功 {3} 筆資料，失敗 {4} 筆資料", receiveType, keepMinYearId, totlaCount, okCount, failCount).AppendLine();
                    }
                    else
                    {
                        msg.AppendFormat("處理商家代號 {0} 小於 {1} 學年的線上資料失敗，錯誤訊息：{2}", receiveType, keepMinYearId, errmsg).AppendLine();
                    }
                }
            }

            resultMsg = msg.ToString();
            return null;
        }

        /// <summary>
        /// 移動線上資料到歷史資料表
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="keepMinYearId"></param>
        /// <param name="logFile"></param>
        /// <param name="totalCount"></param>
        /// <param name="okCount"></param>
        /// <param name="failCount"></param>
        /// <returns></returns>
        public string MoveOnlineDataByReceiveType(string receiveType, string keepMinYearId, string logFile, out Int32 totalCount, out Int32 okCount, out Int32 failCount)
        {
            totalCount = 0;
            okCount = 0;
            failCount = 0;

            #region 檢查參數
            {
                if (String.IsNullOrEmpty(receiveType))
                {
                    return "未指定處理的商家代號";
                }
                int year = 0;
                if (String.IsNullOrEmpty(keepMinYearId) || keepMinYearId.Length != 3 || !int.TryParse(keepMinYearId, out year) || year >= DateTime.Now.Year)
                {
                    return "未指定保留的最小學年代碼或該參數無效";
                }
            }
            #endregion

            string errmsg = null;
            LogWriter loger = new LogWriter(logFile);

            try
            {
                StudentReceiveView[] datas = null;
                KeyValueList<string> channels = null;
                KeyValueList<string> banks = null;
                bool overWrite = true;
                using (EntityFactory factory = new EntityFactory())
                {
                    #region 檢查資料存取物件
                    if (!factory.IsReady())
                    {
                        errmsg = "無法取得資料存取物件";
                        loger.Write(errmsg);
                        return errmsg;
                    }
                    #endregion

                    #region 收集管道代碼名稱
                    errmsg = this.GetChannels(factory, out channels);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        loger.Write(errmsg);
                        return errmsg;
                    }
                    #endregion

                    #region 收集銀行代碼名稱
                    errmsg = this.GetBanks(factory, out banks);
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        loger.Write(errmsg);
                        return errmsg;
                    }
                    #endregion

                    Result result = null;

                    #region 商家代號(學校)資料
                    SchoolRTypeEntity school = null;
                    {
                        Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                        result = factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                        if (!result.IsSuccess)
                        {
                            errmsg = String.Format("讀取 ({0}) 商家代號(學校)資料失敗，錯誤訊息：{1}", receiveType, result.Message);
                            loger.Write(errmsg);
                            return errmsg;
                        }
                        else if (school == null)
                        {
                            errmsg = String.Format("查無 ({0}) 商家代號(學校)資料", receiveType);
                            loger.Write(errmsg);
                            return errmsg;
                        }
                    }
                    string schoolXml = this.GetSchoolXml(school);
                    #endregion

                    #region 預估要處理的資料筆數
                    int preTotalRecords = 0;
                    {
                        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                            .And(StudentReceiveEntity.Field.YearId, RelationEnum.Less, keepMinYearId);
                        result = factory.SelectCount<StudentReceiveEntity>(where, out preTotalRecords);
                        if (!result.IsSuccess)
                        {
                            errmsg = String.Format("讀取要處理的學生繳費資料筆數失敗，錯誤訊息：{0}", result.Message);
                            loger.Write(errmsg);
                            return errmsg;
                        }
                        else
                        {
                            loger.Write(String.Format("預估要處理的學生繳費資料共 {0} 筆", preTotalRecords));
                        }
                    }
                    #endregion

                    #region 取得要處理的 StudentReceive (為了避免系統資料被用光，每次處理 100 筆)
                    int maxRecords = 100;
                    int skipRecords = 0;
                    bool hasData = preTotalRecords > 0;
                    List<string> dataKeys = new List<string>(preTotalRecords);
                    string deleteSql = String.Format("DELETE [{0}] WHERE [{1}] = @ReceiveType AND [{2}] = @YearId AND [{3}] = @TermId AND [{4}] = @DepId AND [{5}] = @ReceiveId AND [{6}] = @StuId AND [{7}] = @OldSeq"
                        , StudentReceiveEntity.TABLE_NAME, StudentReceiveEntity.Field.ReceiveType, StudentReceiveEntity.Field.YearId
                        , StudentReceiveEntity.Field.TermId, StudentReceiveEntity.Field.DepId, StudentReceiveEntity.Field.ReceiveId
                        , StudentReceiveEntity.Field.StuId, StudentReceiveEntity.Field.OldSeq);

                    while (hasData)
                    {
                        #region 取資料
                        {
                            Expression where = new Expression(StudentReceiveView.Field.ReceiveType, receiveType)
                                .And(StudentReceiveView.Field.YearId, RelationEnum.Less, keepMinYearId);

                            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                            orderbys.Add(StudentReceiveView.Field.YearId, OrderByEnum.Asc);
                            orderbys.Add(StudentReceiveView.Field.TermId, OrderByEnum.Asc);
                            orderbys.Add(StudentReceiveView.Field.DepId, OrderByEnum.Asc);
                            orderbys.Add(StudentReceiveView.Field.ReceiveId, OrderByEnum.Asc);
                            orderbys.Add(StudentReceiveView.Field.UpNo, OrderByEnum.Asc);
                            orderbys.Add(StudentReceiveView.Field.UpOrder, OrderByEnum.Asc);
                            orderbys.Add(StudentReceiveView.Field.CreateDate, OrderByEnum.Asc);

                            result = factory.Select<StudentReceiveView>(where, orderbys, skipRecords, maxRecords, out datas);    //因為原資料會被刪除，所以永遠從 0 開始
                            if (!result.IsSuccess)
                            {
                                errmsg = String.Format("讀取要處理的學生繳費資料失敗，錯誤訊息：{0}", result.Message);
                                loger.Write(errmsg);
                                return errmsg;
                            }
                        }
                        #endregion

                        if (datas == null || datas.Length == 0)
                        {
                            hasData = false;
                            loger.Write("查無須要處理的學生繳費資料");
                            return null;
                        }
                        else
                        {
                            Int64 startNo = totalCount + 1;
                            totalCount += datas.Length;
                            loger.Write("開始逐筆處理資料 ({0} ~ {1})", startNo, startNo + datas.Length - 1);

                            #region 逐筆處理
                            KeyValueList<StudentMasterEntity> studentList = new KeyValueList<StudentMasterEntity>();
                            KeyValueList<SchoolRidView3> schoolRidList = new KeyValueList<SchoolRidView3>();

                            foreach (StudentReceiveView studentReceive in datas)
                            {
                                //學生繳費資料 PKey
                                string dataKey = String.Format("{0},{1},{2},{3},{4},{5},{6}", studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.DepId, studentReceive.ReceiveId, studentReceive.StuId, studentReceive.OldSeq);

                                #region 檢查 DataKey 是否出現過，如果出現過表示資料重複取的
                                if (dataKeys.Contains(dataKey))
                                {
                                    //資料重複取的，就中斷以避免無窮迴圈
                                    errmsg = String.Format("資料 Key ({0}) 重複出現，強迫中斷此商家代號處理", dataKey);
                                    loger.Write(errmsg);
                                    return errmsg;
                                }
                                else
                                {
                                    dataKeys.Add(dataKey);
                                }
                                #endregion

                                #region 檢查歷史資料
                                HistoryEntity oldHistory = null;
                                {
                                    Expression where = new Expression(HistoryEntity.Field.ReceiveType, studentReceive.ReceiveType)
                                        .And(HistoryEntity.Field.YearId, studentReceive.YearId)
                                        .And(HistoryEntity.Field.TermId, studentReceive.TermId)
                                        .And(HistoryEntity.Field.DepId, studentReceive.DepId)
                                        .And(HistoryEntity.Field.ReceiveId, studentReceive.ReceiveId)
                                        .And(HistoryEntity.Field.StuId, studentReceive.StuId)
                                        .And(HistoryEntity.Field.OldSeq, studentReceive.OldSeq);
                                    result = factory.SelectFirst<HistoryEntity>(where, null, out oldHistory);
                                    if (!result.IsSuccess)
                                    {
                                        skipRecords++;
                                        loger.Write("查詢 ({0}) 的學生繳費歷史資料失敗，此資料略過不處理。錯誤訊息：{1}", dataKey, result.Message);
                                        continue;
                                    }
                                    else if (oldHistory != null && !overWrite)
                                    {
                                        skipRecords++;
                                        loger.Write("({0}) 的學生繳費歷史資料已存在，此資料略過不處理", dataKey);
                                        continue;
                                    }
                                }
                                #endregion

                                #region 學生基本資料
                                string studentKey = String.Format("{0},{1},{2}", studentReceive.ReceiveType, studentReceive.DepId, studentReceive.StuId);
                                StudentMasterEntity student = studentList.TryGetValue(studentKey, null);
                                if (student == null)
                                {
                                    Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, studentReceive.ReceiveType)
                                        .And(StudentMasterEntity.Field.DepId, studentReceive.DepId)
                                        .And(StudentMasterEntity.Field.Id, studentReceive.StuId);
                                    result = factory.SelectFirst<StudentMasterEntity>(where, null, out student);
                                    if (!result.IsSuccess)
                                    {
                                        skipRecords++;
                                        loger.Write("查詢 ({0}) 的學生基本資料失敗，({1}) 資料略過不處理。錯誤訊息：{2}", studentKey, dataKey, result.Message);
                                        continue;
                                    }
                                    studentList.Add(studentKey, student);
                                }
                                if (student == null)
                                {
                                    skipRecords++;
                                    loger.Write("查無 ({0}) 的學生基本資料，({1}) 資料略過不處理。", studentKey, dataKey);
                                    continue;
                                }
                                #endregion

                                #region 費用別資料
                                string schoolRidKey = String.Format("{0},{1},{2},{3},{4},{5}", studentReceive.ReceiveType, _ReceiveStatus, studentReceive.YearId, studentReceive.TermId, studentReceive.DepId, studentReceive.ReceiveId);
                                SchoolRidView3 schoolRid = schoolRidList.TryGetValue(schoolRidKey, null);
                                {
                                    Expression where = new Expression(SchoolRidView3.Field.ReceiveType, studentReceive.ReceiveType)
                                        .And(SchoolRidView3.Field.ReceiveStatus, _ReceiveStatus)
                                        .And(SchoolRidView3.Field.YearId, studentReceive.YearId)
                                        .And(SchoolRidView3.Field.TermId, studentReceive.TermId)
                                        .And(SchoolRidView3.Field.DepId, studentReceive.DepId)
                                        .And(SchoolRidView3.Field.ReceiveId, studentReceive.ReceiveId);
                                    result = factory.SelectFirst<SchoolRidView3>(where, null, out schoolRid);
                                    if (!result.IsSuccess)
                                    {
                                        skipRecords++;
                                        loger.Write("查詢 ({0}) 的代收費用設定資料失敗，({1}) 資料略過不處理。錯誤訊息：{2}", schoolRidKey, dataKey, result.Message);
                                        continue;
                                    }
                                    schoolRidList.Add(schoolRidKey, schoolRid);
                                }
                                #endregion

                                #region 小計
                                string receiveSumData = null;
                                {
                                    ReceiveSumEntity[] receiveSums = null;
                                    string errmsg2 = this.GetReceiveSums(factory, studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.DepId, studentReceive.ReceiveId, out receiveSums);
                                    if (!String.IsNullOrEmpty(errmsg2))
                                    {
                                        skipRecords++;
                                        loger.Write("查詢 ({0}) 的小計設定資料失敗，({1}) 資料略過不處理。錯誤訊息：{2}", schoolRidKey, dataKey, errmsg2);
                                        continue;
                                    }

                                    if (receiveSums != null && receiveSums.Length > 0)
                                    {
                                        List<SubTotalAmount> subTotalDatas = new List<SubTotalAmount>(receiveSums.Length);
                                        foreach (ReceiveSumEntity receiveSum in receiveSums)
                                        {
                                            #region [MDY:202203XX] 2022擴充案 歷史資料的合計項目固定使用中文名稱
                                            SubTotalAmount subTotal = SubTotalAmount.Create(studentReceive, receiveSum, true, false, out errmsg2);
                                            #endregion

                                            if (String.IsNullOrEmpty(errmsg2))
                                            {
                                                subTotalDatas.Add(subTotal);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        if (!String.IsNullOrEmpty(errmsg2))
                                        {
                                            skipRecords++;
                                            loger.Write("計算 ({0}) 的小計資料失敗，({1}) 資料略過不處理。錯誤訊息：{2}", schoolRidKey, dataKey, errmsg2);
                                            continue;
                                        }
                                        else
                                        {
                                            receiveSumData = this.GetSubTotalAmountString(subTotalDatas);
                                        }
                                    }
                                }
                                #endregion

                                #region 代碼名稱
                                string termName = studentReceive.TermName;
                                string receiveName = studentReceive.ReceiveName;
                                string deptName = studentReceive.DeptName;
                                string collegeName = studentReceive.CollegeName;
                                string majorName = studentReceive.MajorName;
                                string className = studentReceive.ClassName;
                                string receiveWayName = studentReceive.ReceiveWayName;

                                #region [Old]
                                //{
                                //    string errmsg2 = this.GetAllCodeNames(factory, studentReceive, out termName, out receiveName, out deptName, out collegeName, out majorName, out className);
                                //    if (!String.IsNullOrEmpty(errmsg2))
                                //    {
                                //        skipRecords++;
                                //        string nameKey = String.Format("{0},{1}", studentReceive.ReceiveType, studentReceive.YearId);
                                //        loger.Write("查詢 ({0}) 代碼名稱資料失敗，({1}) 資料略過不處理。錯誤訊息：{2}", nameKey, dataKey, errmsg2);
                                //        continue;
                                //    }

                                //    if (channels != null && channels.Count > 0)
                                //    {
                                //        receiveWayName = channels.TryGetValue(studentReceive.ReceiveWay, null);
                                //    }
                                //}
                                #endregion
                                #endregion


                                #region 組歷史資料
                                HistoryEntity newHistory = new HistoryEntity();

                                #region StudentReceive PKey
                                newHistory.ReceiveType = studentReceive.ReceiveType;
                                newHistory.YearId = studentReceive.YearId;
                                newHistory.TermId = studentReceive.TermId;
                                newHistory.DepId = studentReceive.DepId;
                                newHistory.ReceiveId = studentReceive.ReceiveId;
                                newHistory.StuId = studentReceive.StuId;
                                newHistory.OldSeq = studentReceive.OldSeq;
                                #endregion

                                #region Query Key
                                newHistory.UpNo = studentReceive.UpNo;
                                newHistory.UpOrder = studentReceive.UpOrder;
                                newHistory.DeptId = studentReceive.DeptId;
                                newHistory.CollegeId = studentReceive.CollegeId;
                                newHistory.MajorId = studentReceive.MajorId;
                                newHistory.StuGrade = studentReceive.StuGrade;
                                newHistory.ClassId = studentReceive.ClassId;
                                newHistory.CancelNo = studentReceive.CancelNo;
                                newHistory.ReceiveAmount = studentReceive.ReceiveAmount;
                                newHistory.ReceiveDate = studentReceive.ReceiveDate;
                                newHistory.AccountDate = studentReceive.AccountDate;
                                newHistory.ReceiveWay = studentReceive.ReceiveWay;
                                newHistory.SchIdenty = school.SchIdenty;
                                newHistory.SchBankId = school.BankId;
                                #endregion

                                #region 代碼名稱
                                newHistory.TermName = termName;
                                newHistory.ReceiveName = receiveName;
                                newHistory.StuName = student == null ? null : student.Name;

                                newHistory.DeptName = deptName;
                                newHistory.CollegeName = collegeName;
                                newHistory.MajorName = majorName;
                                newHistory.ClassName = className;
                                newHistory.ReceiveWayName = receiveWayName;
                                #endregion

                                newHistory.SchoolXml = schoolXml;
                                newHistory.StudentXml = this.GetStudentXml(student);
                                newHistory.SchoolRidXml = this.GetSchoolRidXml(schoolRid);
                                newHistory.StudentReceiveXml = this.GetStudentReceiveXml(studentReceive);

                                newHistory.ReceiveSumData = receiveSumData ?? String.Empty;
                                newHistory.StudentReceiveMdyDate = studentReceive.UpdateDate;

                                newHistory.CrtDate = DateTime.Now;
                                #endregion

                                #region 寫入歷史資料庫
                                bool isBackupOK = false;
                                if (oldHistory != null)
                                {
                                    newHistory.SN = oldHistory.SN;
                                    int count = 0;
                                    result = factory.Update(newHistory, out count);
                                    if (!result.IsSuccess)
                                    {
                                        loger.Write("覆寫 {0} 歷史資料 (SN={1}) 更新失敗：{2}", dataKey, newHistory.SN, result.Message);
                                        failCount++;
                                    }
                                    else
                                    {
                                        isBackupOK = true;
                                        loger.Write("覆寫 {0} 歷史資料 (SN={1}) 成功", dataKey, newHistory.SN);
                                        okCount++;
                                    }
                                }
                                else
                                {
                                    int count = 0;
                                    result = factory.Insert(newHistory, out count);
                                    if (!result.IsSuccess)
                                    {
                                        loger.Write("新增 {0} 歷史資料失敗：{1}", dataKey, result.Message);
                                        failCount++;
                                    }
                                    else
                                    {
                                        isBackupOK = true;
                                        loger.Write("新增 {0} 歷史資料成功", dataKey);
                                        okCount++;
                                    }
                                }
                                #endregion

                                #region 刪除線上資料
                                if (isBackupOK)
                                {
                                    int count = 0;
                                    KeyValueList parameters = new KeyValueList(7);
                                    parameters.Add("@ReceiveType", studentReceive.ReceiveType);
                                    parameters.Add("@YearId", studentReceive.YearId);
                                    parameters.Add("@TermId", studentReceive.TermId);
                                    parameters.Add("@DepId", studentReceive.DepId);
                                    parameters.Add("@ReceiveId", studentReceive.ReceiveId);
                                    parameters.Add("@StuId", studentReceive.StuId);
                                    parameters.Add("@OldSeq", studentReceive.OldSeq);
                                    result = factory.ExecuteNonQuery(deleteSql, parameters, out count);
                                    if (!result.IsSuccess)
                                    {
                                        skipRecords++;
                                        loger.Write("刪除 {0} 線上資料失敗：{1}", dataKey, result.Message);
                                    }
                                    else
                                    {
                                        loger.Write("刪除 {0} 線上資料成功", dataKey);
                                    }
                                }
                                else
                                {
                                    skipRecords++;
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理線上資料移動到歷史資料表發生例外，錯誤訊息：{0}", ex.Message);
            }

            return errmsg;
        }

        /// <summary>
        /// 清除刪除歷史資料
        /// </summary>
        /// <param name="checkDate"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        public string ClearHistoryData(DateTime checkDate, out string resultMsg)
        {
            #region 取各學校商家代號線上資料保留學年數
            KeyValueList<int> schoolHistoryYears = null;
            {
                #region [MDY:2018xxxx] 配合新的保留學年數設定介面 (可指定單一商家代號) 修改 SQL
                #region [OLD]
//                string sql = @"SELECT Receive_Type, Keep_History_Year FROM
//(
//SELECT Sch_Identy, Receive_Type
//     , (SELECT MAX(Keep_History_Year) FROM School_Rtype AS B WHERE B.Sch_Identy = A.Sch_Identy) Keep_History_Year
//  FROM School_Rtype AS A
//) AS T
// WHERE Keep_History_Year IS NOT NULL AND Keep_History_Year > 0
// ORDER BY Sch_Identy, Receive_Type";
                #endregion

                string sql = @"
SELECT Sch_Identy, Receive_Type, Keep_History_Year
  FROM School_Rtype
 WHERE Keep_History_Year IS NOT NULL AND Keep_History_Year > 0
   AND status = '" + DataStatusCodeTexts.NORMAL + @"' 
 ORDER BY Sch_Identy, Receive_Type";
                #endregion

                KeyValue[] parameters = null;
                DataTable dt = null;
                Result result = null;
                using (EntityFactory factroy = new EntityFactory())
                {
                    result = factroy.GetDataTable(sql, parameters, 0, 0, out dt);
                }
                if (result.IsSuccess)
                {
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        resultMsg = "查無任何有效線的線上資料保留學年數設定資料";
                        return null;
                    }
                    else
                    {
                        schoolHistoryYears = new KeyValueList<int>(dt.Rows.Count);
                        foreach (DataRow row in dt.Rows)
                        {
                            schoolHistoryYears.Add(row["Receive_Type"].ToString().Trim(), Int32.Parse(row["Keep_History_Year"].ToString()));
                        }
                    }
                }
                else
                {
                    resultMsg = String.Format("查詢學校的線上資料保留學年數設定資料失敗，錯誤訊息：{0}", result.Message);
                    return result.Message;
                }
            }
            #endregion

            StringBuilder msg = new StringBuilder();
            if (schoolHistoryYears != null && schoolHistoryYears.Count > 0)
            {
                foreach (KeyValue<int> schoolHistoryYear in schoolHistoryYears)
                {
                    string receiveType = schoolHistoryYear.Key;
                    int keepHistoryYear = schoolHistoryYear.Value;
                    string keepMinYearId = (checkDate.Year - 1911 - keepHistoryYear).ToString("000");  //要轉民國年
                    string logFile = String.IsNullOrEmpty(_LogPath) ? null : Path.Combine(_LogPath, String.Format("{0}_{1:yyyyMMddHHmmss}.log", receiveType, DateTime.Now));
                    Int32 totlaCount = 0;
                    string errmsg = this.ClearHistoryDataByReceiveType(receiveType, keepMinYearId, logFile, out totlaCount);
                    if (String.IsNullOrEmpty(errmsg))
                    {
                        msg.AppendFormat("處理商家代號 {0} 小於 {1} 學年的歷史資料完成，共 {2} 筆資料", receiveType, keepMinYearId, totlaCount).AppendLine();
                    }
                    else
                    {
                        msg.AppendFormat("處理商家代號 {0} 小於 {1} 學年的歷史資料失敗，錯誤訊息：{2}", receiveType, keepMinYearId, errmsg).AppendLine();
                    }
                }
            }

            resultMsg = msg.ToString();
            return null;
        }

        /// <summary>
        /// 清除刪除歷史資料
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="keepMinYearId"></param>
        /// <param name="logFile"></param>
        /// <param name="totalCount"></param>
        /// <param name="okCount"></param>
        /// <param name="failCount"></param>
        /// <returns></returns>
        public string ClearHistoryDataByReceiveType(string receiveType, string keepMinYearId, string logFile, out int totalCount)
        {
            totalCount = 0;

            #region 檢查參數
            {
                if (String.IsNullOrEmpty(receiveType))
                {
                    return "未指定處理的商家代號";
                }
                int year = 0;
                if (String.IsNullOrEmpty(keepMinYearId) || keepMinYearId.Length != 3 || !int.TryParse(keepMinYearId, out year) || year >= DateTime.Now.Year)
                {
                    return "未指定保留的最小學年代碼或該參數無效";
                }
            }
            #endregion

            string errmsg = null;
            LogWriter loger = new LogWriter(logFile);

            try
            {
                using (EntityFactory factory = new EntityFactory())
                {
                    #region 檢查資料存取物件
                    if (!factory.IsReady())
                    {
                        errmsg = "無法取得資料存取物件";
                        loger.Write(errmsg);
                        return errmsg;
                    }
                    #endregion

                    Result result = null;

                    #region 預估要處理的資料筆數
                    int preTotalRecords = 0;
                    {
                        string sql = String.Format("SELECT COUNT(1) FROM [{0}] WHERE [{1}] = @ReceiveType AND [{2}] < @KeepMinYearId", HistoryEntity.TABLE_NAME, HistoryEntity.Field.ReceiveType, HistoryEntity.Field.YearId);
                        KeyValue[] parameters = new KeyValue[2] {
                            new KeyValue("@ReceiveType", receiveType),
                            new KeyValue("@KeepMinYearId", keepMinYearId)
                        };
                        object value = null;
                        result = factory.ExecuteScalar(sql, parameters, out value);
                        if (!result.IsSuccess)
                        {
                            errmsg = String.Format("讀取要處理的歷史資料筆數失敗，錯誤訊息：{0}", result.Message);
                            loger.Write(errmsg);
                            return errmsg;
                        }
                        else
                        {
                            preTotalRecords = Int32.Parse(value.ToString());
                            loger.Write(String.Format("預估要處理的歷史資料共 {0} 筆", preTotalRecords));
                        }
                    }
                    #endregion

                    #region 刪除歷史資料
                    if (preTotalRecords > 0)
                    {
                        string dataKey = String.Format("(ReceiveType = {0} AND YearId < {1})", receiveType, keepMinYearId);
                        string sql = String.Format("DELETE [{0}] WHERE [{1}] = @ReceiveType AND [{2}] < @KeepMinYearId", HistoryEntity.TABLE_NAME, HistoryEntity.Field.ReceiveType, HistoryEntity.Field.YearId);
                        KeyValue[] parameters = new KeyValue[2] {
                            new KeyValue("@ReceiveType", receiveType),
                            new KeyValue("@KeepMinYearId", keepMinYearId)
                        };
                        result = factory.ExecuteNonQuery(sql, parameters, out totalCount);
                        if (result.IsSuccess)
                        {
                            loger.Write("刪除 {0} 歷史資料成功，共 {1} 筆資料", dataKey, totalCount);
                        }
                        else
                        {
                            loger.Write("刪除 {0} 歷史資料失敗：{1}", dataKey, result.Message);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理清除刪除歷史資料發生例外，錯誤訊息：{0}", ex.Message);
            }

            return errmsg;
        }

        #region [Old]
        //public string Backup(string receiveType, DateTime? sDate, DateTime eDate, bool overWrite, string logFile, out Int64 totalCount, out Int64 okCount, out Int64 failCount)
        //{
        //    totalCount = 0;
        //    okCount = 0;
        //    failCount = 0;

        //    #region 檢查參數
        //    if (String.IsNullOrEmpty(receiveType))
        //    {
        //        return "未指定備份的商家代號";
        //    }
        //    #endregion

        //    string errmsg = null;
        //    LogWriter loger = new LogWriter(logFile);
        //    EntityFactory factory = null;

        //    try
        //    {
        //        StudentReceiveEntity[] datas = null;
        //        KeyValueList<string> channels = null;
        //        using (factory = new EntityFactory())
        //        {
        //            #region 檢查資料存取物件
        //            if (!factory.IsReady())
        //            {
        //                errmsg = "無法取得資料存取物件";
        //                loger.Write(errmsg);
        //                return errmsg;
        //            }
        //            #endregion

        //            Result result = null;

        //            #region 取得要備份的 StudentReceive
        //            {
        //                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
        //                    .And(StudentReceiveEntity.Field.CreateDate, RelationEnum.Less, eDate.Date.AddDays(1));
        //                if (sDate != null)
        //                {
        //                    where.And(StudentReceiveEntity.Field.CreateDate, RelationEnum.GreaterEqual, sDate.Value.Date);
        //                }

        //                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
        //                orderbys.Add(StudentReceiveEntity.Field.YearId, OrderByEnum.Asc);
        //                orderbys.Add(StudentReceiveEntity.Field.TermId, OrderByEnum.Asc);
        //                orderbys.Add(StudentReceiveEntity.Field.DepId, OrderByEnum.Asc);
        //                orderbys.Add(StudentReceiveEntity.Field.ReceiveId, OrderByEnum.Asc);
        //                orderbys.Add(StudentReceiveEntity.Field.UpNo, OrderByEnum.Asc);
        //                orderbys.Add(StudentReceiveEntity.Field.UpOrder, OrderByEnum.Asc);

        //                result = factory.SelectAll<StudentReceiveEntity>(where, orderbys, out datas);
        //                if (!result.IsSuccess)
        //                {
        //                    errmsg = String.Format("讀取要備份的學生繳費資料失敗，錯誤訊息：{0}", result.Message);
        //                    loger.Write(errmsg);
        //                    return errmsg;
        //                }

        //                if (datas == null || datas.Length == 0)
        //                {
        //                    loger.Write("查無須要備份的學生繳費資料");
        //                    return null;
        //                }
        //            }
        //            totalCount = datas.Length;
        //            #endregion

        //            #region 收集管道代碼名稱
        //            errmsg = this.GetChannels(factory, out channels);
        //            if (!String.IsNullOrEmpty(errmsg))
        //            {
        //                loger.Write(errmsg);
        //                return errmsg;
        //            }
        //            #endregion

        //            #region 收集銀行代碼名稱
        //            errmsg = this.GetBanks(factory, out channels);
        //            if (!String.IsNullOrEmpty(errmsg))
        //            {
        //                loger.Write(errmsg);
        //                return errmsg;
        //            }
        //            #endregion

        //            #region 商家代號(學校)資料
        //            SchoolRTypeEntity school = null;
        //            {
        //                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
        //                result = factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
        //                if (!result.IsSuccess)
        //                {
        //                    errmsg = String.Format("讀取 ({0}) 商家代號(學校)資料失敗，錯誤訊息：{1}", receiveType, result.Message);
        //                    loger.Write(errmsg);
        //                    return errmsg;
        //                }
        //                else if (school == null)
        //                {
        //                    errmsg = String.Format("查無 ({0}) 商家代號(學校)資料", receiveType);
        //                    loger.Write(errmsg);
        //                    return errmsg;
        //                }
        //            }
        //            string schoolXml = this.GetSchoolXml(school);
        //            #endregion

        //            loger.Write("開始逐筆處理");

        //            #region 逐筆處理
        //            KeyValueList<StudentMasterEntity> studentList = new KeyValueList<StudentMasterEntity>();
        //            KeyValueList<SchoolRidView3> schoolRidList = new KeyValueList<SchoolRidView3>();

        //            foreach (StudentReceiveEntity studentReceive in datas)
        //            {
        //                //學生繳費資料 PKey
        //                string dataKey = String.Format("{0},{1},{2},{3},{4},{5},{6}", studentReceive.ReceiveType, studentReceive.YearId, studentReceive.TermId, studentReceive.DepId, studentReceive.ReceiveId, studentReceive.StuId, studentReceive.OldSeq);

        //                #region 檢查歷史資料
        //                HistoryEntity oldHistory = null;
        //                {
        //                    Expression where = new Expression(HistoryEntity.Field.ReceiveType, studentReceive.ReceiveType)
        //                        .And(HistoryEntity.Field.YearId, studentReceive.YearId)
        //                        .And(HistoryEntity.Field.TermId, studentReceive.TermId)
        //                        .And(HistoryEntity.Field.DepId, studentReceive.DepId)
        //                        .And(HistoryEntity.Field.ReceiveId, studentReceive.ReceiveId)
        //                        .And(HistoryEntity.Field.StuId, studentReceive.StuId)
        //                        .And(HistoryEntity.Field.OldSeq, studentReceive.OldSeq);
        //                    result = factory.SelectFirst<HistoryEntity>(where, null, out oldHistory);
        //                    if (!result.IsSuccess)
        //                    {
        //                        loger.Write("查詢 ({0}) 的學生繳費歷史資料失敗，此資料略過不處理。錯誤訊息：{1}", dataKey, result.Message);
        //                        continue;
        //                    }
        //                    else if (oldHistory != null && !overWrite)
        //                    {
        //                        loger.Write("({0}) 的學生繳費歷史資料已存在，此資料略過不處理", dataKey);
        //                        continue;
        //                    }
        //                }
        //                #endregion

        //                #region 學生基本資料
        //                string studentKey = String.Format("{0},{1},{2}", studentReceive.ReceiveType, studentReceive.DepId, studentReceive.StuId);
        //                StudentMasterEntity student = studentList.TryGetValue(studentKey, null);
        //                if (student == null)
        //                {
        //                    Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, studentReceive.ReceiveType)
        //                        .And(StudentMasterEntity.Field.DepId, studentReceive.DepId)
        //                        .And(StudentMasterEntity.Field.Id, studentReceive.StuId);
        //                    result = factory.SelectFirst<StudentMasterEntity>(where, null, out student);
        //                    if (!result.IsSuccess)
        //                    {
        //                        loger.Write("查詢 ({0}) 的學生基本資料失敗，({1}) 資料略過不處理。錯誤訊息：{2}", studentKey, dataKey, result.Message);
        //                        continue;
        //                    }
        //                    if (student == null)
        //                    {
        //                        student = new StudentMasterEntity();
        //                    }
        //                    studentList.Add(studentKey, student);
        //                }
        //                if (String.IsNullOrEmpty(student.Id))
        //                {
        //                    loger.Write("查無 ({0}) 的學生基本資料，({1}) 資料略過不處理。", studentKey, dataKey);
        //                    continue;
        //                }
        //                #endregion

        //                #region 費用別資料
        //                string schoolRidKey = String.Format("{0},{1},{2},{3},{4},{5}", studentReceive.ReceiveType, _ReceiveStatus, studentReceive.YearId, studentReceive.TermId, studentReceive.DepId, studentReceive.ReceiveId);
        //                SchoolRidView3 schoolRid = schoolRidList.TryGetValue(schoolRidKey, null);
        //                {
        //                    Expression where = new Expression(SchoolRidView3.Field.ReceiveType, studentReceive.ReceiveType)
        //                        .And(SchoolRidView3.Field.ReceiveStatus, _ReceiveStatus)
        //                        .And(SchoolRidView3.Field.YearId, studentReceive.YearId)
        //                        .And(SchoolRidView3.Field.TermId, studentReceive.TermId)
        //                        .And(SchoolRidView3.Field.DepId, studentReceive.DepId)
        //                        .And(SchoolRidView3.Field.ReceiveId, studentReceive.ReceiveId);
        //                    result = factory.SelectFirst<SchoolRidView3>(where, null, out schoolRid);
        //                    if (!result.IsSuccess)
        //                    {
        //                        loger.Write("查詢 ({0}) 的代收費用設定資料失敗，({1}) 資料略過不處理。錯誤訊息：{2}", schoolRidKey, dataKey, result.Message);
        //                        continue;
        //                    }
        //                    if (schoolRid == null)
        //                    {
        //                        schoolRid = new SchoolRidView3();
        //                    }
        //                    schoolRidList.Add(schoolRidKey, schoolRid);
        //                }
        //                #endregion

        //                #region 小計
        //                #endregion

        //                #region 名稱
        //                #endregion


        //                #region 組歷史資料
        //                //HistoryEntity newHistory = new HistoryEntity();
        //                //newHistory.VID = bill.VID;
        //                //newHistory.YearID = bill.YearID;
        //                //newHistory.ExpenseID = bill.ExpenseID;
        //                //newHistory.TermID = bill.TermID;
        //                //newHistory.BillTypeID = bill.BillTypeID;
        //                //newHistory.BillSN = bill.SN;

        //                //newHistory.PayerID = bill.PayerID;
        //                //newHistory.CancelNO = bill.CancelNO;
        //                //newHistory.AccountDate = bill.AccountDate;
        //                //newHistory.ReceiveDate = bill.ReceiveDate;
        //                //newHistory.CancelFlag = bill.CancelFlag;
        //                //newHistory.BillAmount = bill.BillAmount;

        //                //newHistory.CoName = companyVID.CoName == null ? company.CoName : companyVID.CoName;
        //                //newHistory.YearName = yearName;
        //                //newHistory.ExpenseName = expenseName;
        //                //newHistory.TermName = termName;
        //                //newHistory.BillTypeName = billTypeName;
        //                //newHistory.PayerName = payer == null ? String.Empty : payer.PayerName;

        //                //newHistory.SetBill(bill);
        //                //newHistory.SetPayer(payer);
        //                //newHistory.SetCompany(company);
        //                //newHistory.SetBillTypeReceiveItem(receiveItem);

        //                //newHistory.ReceiveMemo = receiveMemo;
        //                //newHistory.ReceiveWayName = channelNames[bill.ReceiveWay];
        //                //newHistory.ReceiveBankName = bankNames[bill.ReceiveBankID];

        //                //newHistory.BillCreatedTime = bill.CreatedTime;
        //                //newHistory.CreatedTime = DateTime.Now;
        //                #endregion

        //                #region 寫入資料庫
        //                //if (oldHistory != null)
        //                //{
        //                //    newHistory.SN = oldHistory.SN;
        //                //    expResult = factory.Update(newHistory);
        //                //    if (!expResult.IsSuccess)
        //                //    {
        //                //        loger.Write("繳費單 {0} 歷史資料更新失敗：{1}", billKey, expResult.MessageText);
        //                //    }
        //                //    else
        //                //    {
        //                //        loger.Write("繳費單 {0} 歷史資料更新成功", billKey);
        //                //        count++;
        //                //    }
        //                //}
        //                //else
        //                //{
        //                //    expResult = factory.Insert(newHistory);
        //                //    if (!expResult.IsSuccess)
        //                //    {
        //                //        loger.Write("繳費單 {0} 歷史資料新增失敗：{1}", billKey, expResult.MessageText);
        //                //    }
        //                //    else
        //                //    {
        //                //        loger.Write("繳費單 {0} 歷史資料新增成功", billKey);
        //                //        count++;
        //                //    }
        //                //}
        //                #endregion
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errmsg = String.Format("處理歷史資料備份發生例外，錯誤訊息：{0}", ex.Message);
        //    }

        //    return errmsg;
        //}
        #endregion

        #region 匯出歷史(繳費)資料查詢結果 (Excel 檔)
        #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
        /// <summary>
        /// 匯出歷史(繳費)資料查詢結果
        /// </summary>
        /// <param name="where">指定查詢條件。</param>
        /// <param name="outFileContent"></param>
        /// <param name="isUseODS">指定是否匯出成 ODS 檔</param>
        /// <returns></returns>
        public Result ExportC3700009QueryResult(Expression where, out byte[] outFileContent, bool isUseODS = false)
        {
            outFileContent = null;
            if (where == null || where.IsEmpty())
            {
                return new Result(false, "缺少查詢條件", CoreStatusCode.INVALID_PARAMETER, null);
            }

            Result result = null;
            try
            {
                int dataCount = 0;
                HistoryEntity[] datas = null;
                CodeTextList yearNames = new CodeTextList();
                using (EntityFactory factory = new EntityFactory())
                {
                    result = factory.SelectCount<HistoryEntity>(where, out dataCount);
                    if (result.IsSuccess && dataCount > 0)
                    {
                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                        orderbys.Add(HistoryEntity.Field.StuId, OrderByEnum.Asc);
                        result = factory.SelectAll<HistoryEntity>(where, orderbys, out datas);
                        if (result.IsSuccess && datas != null && datas.Length > 0)
                        {
                            #region 取得學年代碼名稱
                            {
                                YearListEntity[] yearDatas = null;
                                Expression w2 = new Expression();
                                result = factory.SelectAll<YearListEntity>(w2, (KeyValueList<OrderByEnum>)null, out yearDatas);
                                if (result.IsSuccess && yearDatas != null && yearDatas.Length > 0)
                                {
                                    yearNames = new CodeTextList(yearDatas.Length);
                                    foreach (YearListEntity yearData in yearDatas)
                                    {
                                        yearNames.Add(yearData.YearId, yearData.YearName);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                if (result.IsSuccess)
                {
                    if (dataCount == 0 || datas == null || datas.Length == 0)
                    {
                        outFileContent = new byte[0];
                    }
                    else if (!isUseODS && dataCount > 65535)
                    {
                        result = new Result(false, "匯出資料超過 Excel (XLS) 的筆數限制 (65535 筆)", CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    else if (isUseODS && dataCount > Fuju.ODS.ODSSheet.MAX_ROW_COUNT)
                    {
                        result = new Result(false, String.Format("匯出資料超過 Calc (ODS) 的筆數限制 ({0} 筆)", Fuju.ODS.ODSSheet.MAX_ROW_COUNT), CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    else
                    {
                        #region 空的 DataTable
                        DataTable dt = new DataTable();
                        DataColumnCollection dColumns = dt.Columns;
                        List<string> historyFields = new List<string>();
                        List<string> studentFields = new List<string>();
                        List<string> receiveFields = new List<string>();
                        CodeTextList receiveAmountFields = new CodeTextList(40);
                        CodeTextList subTotalFields = new CodeTextList(21);
                        CodeTextList memoFields = new CodeTextList(21);

                        #region 學年代碼、學年名稱、學期代碼、學期名稱
                        {
                            dColumns.Add(new DataColumn(HistoryEntity.Field.YearId)
                            {
                                Caption = "學年代碼"
                            });
                            historyFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn("YearName")
                            {
                                Caption = "學年名稱"
                            });
                            historyFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(HistoryEntity.Field.TermId)
                            {
                                Caption = "學期代碼"
                            });
                            historyFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(HistoryEntity.Field.TermName)
                            {
                                Caption = "學期名稱"
                            });
                            historyFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                        }
                        #endregion

                        #region 學生基本資料 (StudentMasterEntity)
                        {
                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.Id)
                            {
                                Caption = "學號"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.Name)
                            {
                                Caption = "姓名"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.IdNumber)
                            {
                                Caption = "身分證字號"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.Birthday)
                            {
                                Caption = "生日"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.Tel)
                            {
                                Caption = "電話"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.ZipCode)
                            {
                                Caption = "郵遞區號"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.Address)
                            {
                                Caption = "住址"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.Email)
                            {
                                Caption = "電子郵件"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentMasterEntity.Field.StuParent)
                            {
                                Caption = "家長名稱"
                            });
                            studentFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                        }
                        #endregion

                        #region 學生繳費資料 (StudentReceiveView)
                        {
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.OldSeq)
                            {
                                Caption = "序號"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.PayDueDate)
                            {
                                Caption = "繳款期限"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            #region 學籍資料 (StudentReceiveView)
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.StuGrade)
                            {
                                Caption = "年級"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.StuHid)
                            {
                                Caption = "座號"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            #region 班別
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ClassId)
                            {
                                Caption = "班別代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ClassName)
                            {
                                Caption = "班別名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 部別
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.DeptId)
                            {
                                Caption = "部別代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.DeptName)
                            {
                                Caption = "部別名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 院別
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.CollegeId)
                            {
                                Caption = "院別代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.CollegeName)
                            {
                                Caption = "院別名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 科系
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.MajorId)
                            {
                                Caption = "科系代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.MajorName)
                            {
                                Caption = "科系名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 減免
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ReduceId)
                            {
                                Caption = "減免代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ReduceName)
                            {
                                Caption = "減免名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 就貸
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.LoanId)
                            {
                                Caption = "就貸代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.LoanName)
                            {
                                Caption = "就貸名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 住宿
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.DormId)
                            {
                                Caption = "住宿代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.DormName)
                            {
                                Caption = "住宿名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 身份註
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyId01)
                            {
                                Caption = "身份註記01代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyName01)
                            {
                                Caption = "身份註記01名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyId02)
                            {
                                Caption = "身份註記02代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyName02)
                            {
                                Caption = "身份註記02名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyId03)
                            {
                                Caption = "身份註記03代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyName03)
                            {
                                Caption = "身份註記03名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyId04)
                            {
                                Caption = "身份註記04代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyName04)
                            {
                                Caption = "身份註記04名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyId05)
                            {
                                Caption = "身份註記05代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyName05)
                            {
                                Caption = "身份註記05名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyId06)
                            {
                                Caption = "身份註記06代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.IdentifyName06)
                            {
                                Caption = "身份註記06名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion
                            #endregion

                            #region 繳費資料 (StudentReceiveView)
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.StuHour)
                            {
                                Caption = "上課時數"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.StuCredit)
                            {
                                Caption = "總學分數"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.LoanAmount)
                            {
                                Caption = "就學貸款可貸金額"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.SeriorNo)
                            {
                                Caption = "流水號"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ReceiveAmount)
                            {
                                Caption = "繳費金額"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.CancelNo)
                            {
                                Caption = "虛擬帳號"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ReceiveSmamount)
                            {
                                Caption = "超商繳費金額"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.CancelSmno)
                            {
                                Caption = "超商虛擬帳號"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            //dColumns.Add(new DataColumn("ZBarcode")
                            //{
                            //    Caption = "超商條碼"
                            //});
                            //receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 收入科目金額
                            {
                                string[] fields = new string[] {
                                    StudentReceiveView.Field.Receive01, StudentReceiveView.Field.Receive02, StudentReceiveView.Field.Receive03, StudentReceiveView.Field.Receive04, StudentReceiveView.Field.Receive05,
                                    StudentReceiveView.Field.Receive06, StudentReceiveView.Field.Receive07, StudentReceiveView.Field.Receive08, StudentReceiveView.Field.Receive09, StudentReceiveView.Field.Receive10,
                                    StudentReceiveView.Field.Receive11, StudentReceiveView.Field.Receive12, StudentReceiveView.Field.Receive13, StudentReceiveView.Field.Receive14, StudentReceiveView.Field.Receive15,
                                    StudentReceiveView.Field.Receive16, StudentReceiveView.Field.Receive17, StudentReceiveView.Field.Receive18, StudentReceiveView.Field.Receive19, StudentReceiveView.Field.Receive20,
                                    StudentReceiveView.Field.Receive21, StudentReceiveView.Field.Receive22, StudentReceiveView.Field.Receive23, StudentReceiveView.Field.Receive24, StudentReceiveView.Field.Receive25,
                                    StudentReceiveView.Field.Receive26, StudentReceiveView.Field.Receive27, StudentReceiveView.Field.Receive28, StudentReceiveView.Field.Receive29, StudentReceiveView.Field.Receive30,
                                    StudentReceiveView.Field.Receive31, StudentReceiveView.Field.Receive32, StudentReceiveView.Field.Receive33, StudentReceiveView.Field.Receive34, StudentReceiveView.Field.Receive35,
                                    StudentReceiveView.Field.Receive36, StudentReceiveView.Field.Receive37, StudentReceiveView.Field.Receive38, StudentReceiveView.Field.Receive39, StudentReceiveView.Field.Receive40
                                };
                                for (int idx = 0; idx < fields.Length; idx++)
                                {
                                    string field = fields[idx];
                                    dColumns.Add(new DataColumn(field));
                                    receiveAmountFields.Add(field, String.Format("收入科目金額{0:00}", idx + 1));
                                }
                            }
                            #endregion

                            #region 小計金額
                            {
                                for (int no = 1; no <= 21; no++)
                                {
                                    string field = String.Format("Other_{0:00}", no);
                                    dColumns.Add(new DataColumn(field));
                                    subTotalFields.Add(field, String.Format("小計金額{0:00}", no));
                                }
                            }
                            #endregion

                            #region 轉帳資料
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.DeductBankid)
                            {
                                Caption = "學生扣款轉帳銀行代碼"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.DeductAccountno)
                            {
                                Caption = "學生扣款轉帳銀行帳號"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.DeductAccountname)
                            {
                                Caption = "學生扣款轉帳銀行戶名"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.DeductAccountid)
                            {
                                Caption = "學生扣款轉帳銀行ＩＤ"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion

                            #region 備註資料
                            {
                                string[] fields = new string[] {
                                    StudentReceiveView.Field.Memo01, StudentReceiveView.Field.Memo02, StudentReceiveView.Field.Memo03, StudentReceiveView.Field.Memo04, StudentReceiveView.Field.Memo05,
                                    StudentReceiveView.Field.Memo06, StudentReceiveView.Field.Memo07, StudentReceiveView.Field.Memo08, StudentReceiveView.Field.Memo09, StudentReceiveView.Field.Memo10,
                                    StudentReceiveView.Field.Memo11, StudentReceiveView.Field.Memo12, StudentReceiveView.Field.Memo13, StudentReceiveView.Field.Memo14, StudentReceiveView.Field.Memo15,
                                    StudentReceiveView.Field.Memo16, StudentReceiveView.Field.Memo17, StudentReceiveView.Field.Memo18, StudentReceiveView.Field.Memo19, StudentReceiveView.Field.Memo20,
                                    StudentReceiveView.Field.Memo21
                                };
                                for (int idx = 0; idx < fields.Length; idx++)
                                {
                                    string field = fields[idx];
                                    dColumns.Add(new DataColumn(field));
                                    memoFields.Add(field, String.Format("備註{0:00}", idx + 1));
                                }
                            }
                            #endregion

                            #region 繳款狀態 (StudentReceiveView)
                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ReceiveDate)
                            {
                                Caption = "代收日期"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.AccountDate)
                            {
                                Caption = "入帳日期"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ReceiveWay)
                            {
                                Caption = "繳款管道代號"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);

                            dColumns.Add(new DataColumn(StudentReceiveView.Field.ReceiveWayName)
                            {
                                Caption = "繳款管道名稱"
                            });
                            receiveFields.Add(dColumns[dColumns.Count - 1].ColumnName);
                            #endregion
                        }
                        #endregion
                        #endregion

                        #region 收集收入科目名稱、備註名稱、小計名稱
                        {
                            HistoryEntity data = datas[0];
                            SchoolRidView3 schoolRid = this.DeSchoolRidXml(data.SchoolRidXml);

                            #region [MDY:202203XX] 2022擴充案 收入科目 改寫，改用 GetAllReceiveItemChts()
                            #region [OLD]
                            //string[] receiveItems = schoolRid.GetAllReceiveItems();
                            #endregion

                            string[] receiveItems = schoolRid.GetAllReceiveItemChts();
                            #endregion

                            for (int idx = 0; idx < receiveItems.Length; idx++)
                            {
                                if (idx < receiveAmountFields.Count)
                                {
                                    receiveAmountFields[idx].Text = receiveItems[idx];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            //for (int idx = receiveAmountFields.Count - 1; idx >= 0; idx--)
                            //{
                            //    if (String.IsNullOrWhiteSpace(receiveAmountFields[idx].Text))
                            //    {
                            //        receiveAmountFields.RemoveAt(idx);
                            //    }
                            //}

                            #region [MDY:202203XX] 2022擴充案 備註項目 改寫，改用 GetAllMemoTitleChts()
                            #region [OLD]
                            //string[] memoTitles = schoolRid.GetAllMemoTitles();
                            #endregion

                            string[] memoTitles = schoolRid.GetAllMemoTitleChts();
                            #endregion

                            for (int idx = 0; idx < memoTitles.Length; idx++)
                            {
                                if (idx < memoFields.Count)
                                {
                                    memoFields[idx].Text = memoTitles[idx];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            //for (int idx = memoFields.Count - 1; idx >= 0; idx--)
                            //{
                            //    if (String.IsNullOrWhiteSpace(memoFields[idx].Text))
                            //    {
                            //        memoFields.RemoveAt(idx);
                            //    }
                            //}

                            SubTotalAmount[] subTotalAmounts = this.DeSubTotalAmountString(data.ReceiveSumData);
                            if (subTotalAmounts == null || subTotalAmounts.Length == 0)
                            {
                                for (int idx = 0; idx < subTotalFields.Count; idx++)
                                {
                                    subTotalFields[idx].Text = String.Empty;
                                }
                            }
                            else
                            {
                                for (int idx = 0; idx < subTotalFields.Count; idx++)
                                {
                                    string field = subTotalFields[idx].Code;
                                    string text = String.Empty;
                                    foreach (SubTotalAmount subTotalAmount in subTotalAmounts)
                                    {
                                        if (subTotalAmount.Id == field)
                                        {
                                            text = subTotalAmount.Name;
                                            break;
                                        }
                                    }
                                    subTotalFields[idx].Text = text;
                                }
                            }
                            //for (int idx = subTotalFields.Count - 1; idx >= 0; idx--)
                            //{
                            //    if (String.IsNullOrWhiteSpace(subTotalFields[idx].Text))
                            //    {
                            //        subTotalFields.RemoveAt(idx);
                            //    }
                            //}
                        }
                        #endregion

                        #region 轉成 DataTable
                        foreach (HistoryEntity data in datas)
                        {
                            StudentMasterEntity student = this.DeStudentXml(data.StudentXml);
                            SchoolRidView3 schoolRid = this.DeSchoolRidXml(data.SchoolRidXml);
                            StudentReceiveView receive = this.DeStudentReceiveXml(data.StudentReceiveXml);
                            SubTotalAmount[] subTotalAmounts = this.DeSubTotalAmountString(data.ReceiveSumData);

                            DataRow dRow = dt.NewRow();

                            #region 學年代碼、學年名稱、學期代碼、學期名稱
                            foreach (string field in historyFields)
                            {
                                if (field == "YearName")
                                {
                                    int idx = yearNames.CodeIndexOf(data.YearId);
                                    if (idx > -1)
                                    {
                                        dRow[field] = yearNames[idx].Text;
                                    }
                                }
                                else
                                {
                                    object value = data.GetValue(field, out result);
                                    if (result.IsSuccess)
                                    {
                                        dRow[field] = value == null ? String.Empty : value.ToString();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                #region [Old]
                                //row[HistoryEntity.Field.YearId] = data.YearId;
                                //row["YearName"] = data.YearId;
                                //row[HistoryEntity.Field.TermId] = data.TermId;
                                //row[HistoryEntity.Field.TermName] = data.TermName;
                                #endregion
                            }
                            #endregion

                            #region 學生基本資料 (StudentMasterEntity)
                            if (result.IsSuccess)
                            {
                                foreach (string field in studentFields)
                                {
                                    object value = student == null ? String.Empty : student.GetValue(field, out result);
                                    if (result.IsSuccess)
                                    {
                                        dRow[field] = value == null ? String.Empty : value.ToString();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                #region [Old]
                                //row[StudentMasterEntity.Field.Id] = (student == null ? String.Empty : student.Id);
                                //row[StudentMasterEntity.Field.Name] = (student == null ? String.Empty : student.Name);
                                //row[StudentMasterEntity.Field.IdNumber] = (student == null ? String.Empty : student.IdNumber);
                                //row[StudentMasterEntity.Field.Birthday] = (student == null ? String.Empty : student.Birthday);
                                //row[StudentMasterEntity.Field.Tel] = (student == null ? String.Empty : student.Tel);
                                //row[StudentMasterEntity.Field.ZipCode] = (student == null ? String.Empty : student.ZipCode);
                                //row[StudentMasterEntity.Field.Address] = (student == null ? String.Empty : student.Address);
                                //row[StudentMasterEntity.Field.Email] = (student == null ? String.Empty : student.Email);
                                //row[StudentMasterEntity.Field.StuParent] = (student == null ? String.Empty : student.StuParent);
                                #endregion
                            }
                            #endregion

                            #region 學生繳費資料 (StudentReceiveView)
                            if (result.IsSuccess)
                            {
                                foreach (string field in receiveFields)
                                {
                                    object value = receive == null ? String.Empty : receive.GetValue(field, out result);
                                    if (result.IsSuccess)
                                    {
                                        if (value is decimal?)
                                        {
                                            decimal? amount = (decimal?)value;
                                            dRow[field] = amount == null ? String.Empty : amount.Value.ToString("#,#");
                                        }
                                        else
                                        {
                                            dRow[field] = value == null ? String.Empty : value.ToString();
                                        }

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                #region [Old]
                                //row[StudentReceiveView.Field.OldSeq] = (receive == null ? String.Empty : receive.OldSeq.ToString());
                                //row[StudentReceiveView.Field.PayDueDate] = (receive == null ? String.Empty : receive.PayDueDate);

                                //row[StudentReceiveView.Field.StuGrade] = (receive == null ? String.Empty : receive.StuGrade);
                                //row[StudentReceiveView.Field.StuHid] = (receive == null ? String.Empty : receive.StuHid);

                                //row[StudentReceiveView.Field.ClassId] = (receive == null ? String.Empty : receive.ClassId);
                                //row[StudentReceiveView.Field.ClassName] = (receive == null ? String.Empty : receive.ClassName);
                                //row[StudentReceiveView.Field.DeptId] = (receive == null ? String.Empty : receive.DeptId);
                                //row[StudentReceiveView.Field.DeptName] = (receive == null ? String.Empty : receive.DeptName);
                                //row[StudentReceiveView.Field.CollegeId] = (receive == null ? String.Empty : receive.CollegeId);
                                //row[StudentReceiveView.Field.CollegeName] = (receive == null ? String.Empty : receive.CollegeName);
                                //row[StudentReceiveView.Field.MajorId] = (receive == null ? String.Empty : receive.MajorId);
                                //row[StudentReceiveView.Field.MajorName] = (receive == null ? String.Empty : receive.MajorName);
                                //row[StudentReceiveView.Field.ReduceId] = (receive == null ? String.Empty : receive.ReduceId);
                                //row[StudentReceiveView.Field.ReduceName] = (receive == null ? String.Empty : receive.ReduceName);
                                //row[StudentReceiveView.Field.LoanId] = (receive == null ? String.Empty : receive.LoanId);
                                //row[StudentReceiveView.Field.LoanName] = (receive == null ? String.Empty : receive.LoanName);
                                //row[StudentReceiveView.Field.DormId] = (receive == null ? String.Empty : receive.DormId);
                                //row[StudentReceiveView.Field.DormName] = (receive == null ? String.Empty : receive.DormName);

                                //row[StudentReceiveView.Field.IdentifyId01] = (receive == null ? String.Empty : receive.IdentifyId01);
                                //row[StudentReceiveView.Field.IdentifyName01] = (receive == null ? String.Empty : receive.IdentifyName01);
                                //row[StudentReceiveView.Field.IdentifyId02] = (receive == null ? String.Empty : receive.IdentifyId02);
                                //row[StudentReceiveView.Field.IdentifyName02] = (receive == null ? String.Empty : receive.IdentifyName02);
                                //row[StudentReceiveView.Field.IdentifyId03] = (receive == null ? String.Empty : receive.IdentifyId03);
                                //row[StudentReceiveView.Field.IdentifyName03] = (receive == null ? String.Empty : receive.IdentifyName03);
                                //row[StudentReceiveView.Field.IdentifyId04] = (receive == null ? String.Empty : receive.IdentifyId04);
                                //row[StudentReceiveView.Field.IdentifyName04] = (receive == null ? String.Empty : receive.IdentifyName04);
                                //row[StudentReceiveView.Field.IdentifyId05] = (receive == null ? String.Empty : receive.IdentifyId05);
                                //row[StudentReceiveView.Field.IdentifyName05] = (receive == null ? String.Empty : receive.IdentifyName05);
                                //row[StudentReceiveView.Field.IdentifyId06] = (receive == null ? String.Empty : receive.IdentifyId06);
                                //row[StudentReceiveView.Field.IdentifyName06] = (receive == null ? String.Empty : receive.IdentifyName06);

                                //row[StudentReceiveView.Field.StuHour] = (receive == null || receive.StuHour == null ? String.Empty : receive.StuHour.Value.ToString("0"));
                                //row[StudentReceiveView.Field.StuCredit] = (receive == null || receive.StuCredit == null ? String.Empty : receive.StuCredit.Value.ToString("0"));
                                //row[StudentReceiveView.Field.LoanAmount] = (receive == null || receive.LoanAmount == null ? String.Empty : receive.LoanAmount.Value.ToString("#,0"));

                                //row[StudentReceiveView.Field.SeriorNo] = (receive == null ? String.Empty : receive.SeriorNo);
                                //row[StudentReceiveView.Field.ReceiveAmount] = (receive == null || receive.ReceiveAmount == null ? String.Empty : receive.ReceiveAmount.Value.ToString("#,0"));
                                //row[StudentReceiveView.Field.CancelNo] = (receive == null ? String.Empty : receive.CancelNo);
                                //row[StudentReceiveView.Field.ReceiveSmamount] = (receive == null || receive.ReceiveSmamount == null ? String.Empty : receive.ReceiveSmamount.Value.ToString("#,0"));
                                //row[StudentReceiveView.Field.CancelSmno] = (receive == null ? String.Empty : receive.CancelSmno);

                                ////row["ZBarcode", null);
                                #endregion
                            }
                            #endregion

                            #region 收入科目金額
                            if (result.IsSuccess)
                            {
                                Decimal?[] values = null;
                                if (receive == null)
                                {
                                    values = new Decimal?[40];
                                    for (int idx = 0; idx < values.Length; idx++)
                                    {
                                        values[idx] = null;
                                    }
                                }
                                values = new Decimal?[40] {
                                    receive.Receive01, receive.Receive02, receive.Receive03, receive.Receive04, receive.Receive05, receive.Receive06, receive.Receive07, receive.Receive08, receive.Receive09, receive.Receive10,
                                    receive.Receive11, receive.Receive12, receive.Receive13, receive.Receive14, receive.Receive15, receive.Receive16, receive.Receive17, receive.Receive18, receive.Receive19, receive.Receive20,
                                    receive.Receive21, receive.Receive22, receive.Receive23, receive.Receive24, receive.Receive25, receive.Receive26, receive.Receive27, receive.Receive28, receive.Receive29, receive.Receive30,
                                    receive.Receive31, receive.Receive32, receive.Receive33, receive.Receive34, receive.Receive35, receive.Receive36, receive.Receive37, receive.Receive38, receive.Receive39, receive.Receive40
                                };

                                for (int idx = 0; idx < receiveAmountFields.Count; idx++)
                                {
                                    string field = receiveAmountFields[idx].Code;
                                    decimal? amount = values[idx];
                                    dRow[field] = amount == null ? String.Empty : amount.Value.ToString("#,#");
                                }

                                #region [Old]
                                //row[StudentReceiveView.Field.Receive01, "收入科目金額01");
                                //row[StudentReceiveView.Field.Receive02, "收入科目金額02");
                                //row[StudentReceiveView.Field.Receive03, "收入科目金額03");
                                //row[StudentReceiveView.Field.Receive04, "收入科目金額04");
                                //row[StudentReceiveView.Field.Receive05, "收入科目金額05");
                                //row[StudentReceiveView.Field.Receive06, "收入科目金額06");
                                //row[StudentReceiveView.Field.Receive07, "收入科目金額07");
                                //row[StudentReceiveView.Field.Receive08, "收入科目金額08");
                                //row[StudentReceiveView.Field.Receive09, "收入科目金額09");
                                //row[StudentReceiveView.Field.Receive10, "收入科目金額10");

                                //row[StudentReceiveView.Field.Receive11, "收入科目金額11");
                                //row[StudentReceiveView.Field.Receive12, "收入科目金額12");
                                //row[StudentReceiveView.Field.Receive13, "收入科目金額13");
                                //row[StudentReceiveView.Field.Receive14, "收入科目金額14");
                                //row[StudentReceiveView.Field.Receive15, "收入科目金額15");
                                //row[StudentReceiveView.Field.Receive16, "收入科目金額16");
                                //row[StudentReceiveView.Field.Receive17, "收入科目金額17");
                                //row[StudentReceiveView.Field.Receive18, "收入科目金額18");
                                //row[StudentReceiveView.Field.Receive19, "收入科目金額19");
                                //row[StudentReceiveView.Field.Receive20, "收入科目金額20");

                                //row[StudentReceiveView.Field.Receive21, "收入科目金額21");
                                //row[StudentReceiveView.Field.Receive22, "收入科目金額22");
                                //row[StudentReceiveView.Field.Receive23, "收入科目金額23");
                                //row[StudentReceiveView.Field.Receive24, "收入科目金額24");
                                //row[StudentReceiveView.Field.Receive25, "收入科目金額25");
                                //row[StudentReceiveView.Field.Receive26, "收入科目金額26");
                                //row[StudentReceiveView.Field.Receive27, "收入科目金額27");
                                //row[StudentReceiveView.Field.Receive28, "收入科目金額28");
                                //row[StudentReceiveView.Field.Receive29, "收入科目金額29");
                                //row[StudentReceiveView.Field.Receive30, "收入科目金額30");

                                //row[StudentReceiveView.Field.Receive31, "收入科目金額31");
                                //row[StudentReceiveView.Field.Receive32, "收入科目金額32");
                                //row[StudentReceiveView.Field.Receive33, "收入科目金額33");
                                //row[StudentReceiveView.Field.Receive34, "收入科目金額34");
                                //row[StudentReceiveView.Field.Receive35, "收入科目金額35");
                                //row[StudentReceiveView.Field.Receive36, "收入科目金額36");
                                //row[StudentReceiveView.Field.Receive37, "收入科目金額37");
                                //row[StudentReceiveView.Field.Receive38, "收入科目金額38");
                                //row[StudentReceiveView.Field.Receive39, "收入科目金額39");
                                //row[StudentReceiveView.Field.Receive40, "收入科目金額40");
                                #endregion
                            }
                            #endregion

                            #region 備註資料
                            if (result.IsSuccess)
                            {
                                string[] values = new string[] {
                                    receive.Memo01, receive.Memo02, receive.Memo03, receive.Memo04, receive.Memo05,
                                    receive.Memo06, receive.Memo07, receive.Memo08, receive.Memo08, receive.Memo10,
                                    receive.Memo11, receive.Memo12, receive.Memo13, receive.Memo14, receive.Memo15,
                                    receive.Memo16, receive.Memo17, receive.Memo18, receive.Memo18, receive.Memo20,
                                    receive.Memo21
                                };
                                for (int idx = 0; idx < memoFields.Count; idx++)
                                {
                                    string field = memoFields[idx].Code;
                                    dRow[field] = values[idx];
                                }
                            }
                            #endregion

                            #region 小計金額
                            if (result.IsSuccess)
                            {
                                foreach (SubTotalAmount subTotalAmount in subTotalAmounts)
                                {
                                    string field = subTotalAmount.Id;
                                    decimal? amount = subTotalAmount.Amount;
                                    dRow[field] = amount == null ? String.Empty : amount.Value.ToString("#,#");
                                }
                            }
                            #endregion

                            if (result.IsSuccess)
                            {
                                dt.Rows.Add(dRow);
                            }
                            else
                            {
                                break;
                            }
                        }
                        #endregion

                        #region 轉換 DataTable 前先將 ColumnName 換成中文
                        {
                            foreach (CodeText one in receiveAmountFields)
                            {
                                string field = one.Code;
                                if (!String.IsNullOrWhiteSpace(one.Text))
                                {
                                    dColumns[field].Caption = one.Text;
                                }
                                else
                                {
                                    dColumns.Remove(field);
                                }
                            }

                            foreach (CodeText one in subTotalFields)
                            {
                                string field = one.Code;
                                if (!String.IsNullOrWhiteSpace(one.Text))
                                {
                                    dColumns[field].Caption = one.Text;
                                }
                                else
                                {
                                    dColumns.Remove(field);
                                }
                            }

                            foreach (CodeText one in memoFields)
                            {
                                string field = one.Code;
                                if (!String.IsNullOrWhiteSpace(one.Text))
                                {
                                    dColumns[field].Caption = one.Text;
                                }
                                else
                                {
                                    dColumns.Remove(field);
                                }
                            }

                            foreach (DataColumn dColumn in dColumns)
                            {
                                dColumn.ColumnName = dColumn.Caption;
                            }
                        }
                        #endregion

                        #region 匯出成檔案
                        if (result.IsSuccess)
                        {
                            if (dt != null)
                            {
                                if (isUseODS)
                                {
                                    #region DataTable 轉 ODS
                                    ODSHelper helper = new ODSHelper();
                                    outFileContent = helper.DataTable2ODS(dt, isDecimalTruncate: true);
                                    #endregion
                                }
                                else
                                {
                                    #region DataTable 轉 Xls
                                    ConvertFileHelper helper = new ConvertFileHelper();
                                    outFileContent = helper.Dt2Xls(dt);
                                    #endregion
                                }

                                dt.Clear();
                                dt.Dispose();
                                dt = null;

                                if (outFileContent == null)
                                {
                                    if (isUseODS)
                                    {
                                        result = new Result(false, "將匯出資料存成 ODS 檔失敗", CoreStatusCode.UNKNOWN_ERROR, null);
                                    }
                                    else
                                    {
                                        result = new Result(false, "將匯出資料存成 XLS 檔失敗", CoreStatusCode.UNKNOWN_ERROR, null);
                                    }
                                }

                                #region 大於 20M 則壓縮成 ZIP，避免序列化失敗 (目前沒有傳附檔名回去，所以暫時 Mark)
                                //if (result.IsSuccess && outFileContent.Length > 1024 * 1024 * 20)
                                //{
                                //    try
                                //    {
                                //        string xlsFileName = Path.Combine(Path.GetTempPath(), String.Format("{0}_{1:yyyyMMddHHmmss}", receiveType, DateTime.Now));
                                //        string zipFileName = Path.GetTempFileName();
                                //        File.WriteAllBytes(xlsFileName, outFileContent);
                                //        ZIPHelper.ZipFile(xlsFileName, zipFileName);
                                //        outFileContent = File.ReadAllBytes(zipFileName);
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        return new Result(false, "將匯出資料壓縮成 ZIP 檔失敗，", CoreStatusCode.UNKNOWN_EXCEPTION, ex);
                                //    }
                                //}
                                #endregion

                            }
                            else
                            {
                                outFileContent = new byte[0];
                                //return "無查資料";
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                string errmsg = String.Format("匯出歷史(繳費)資料查詢結果發生例外，錯誤訊息：{0}", ex.Message);
                result = new Result(false, errmsg, CoreStatusCode.UNKNOWN_EXCEPTION, ex);
            }
            return result;
        }
        #endregion
        #endregion
        #endregion

        #region 序列化與反序列化
        #region SchoolRTypeEntity
        private Type _SchoolDataType = typeof(SchoolRTypeEntity);

        /// <summary>
        /// 取得商家代號(學校)資料的序列化 XML
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetSchoolXml(SchoolRTypeEntity data)
        {
            string xml = null;
            if (data != null && Common.TryToXmlExplicitly2(data, _SchoolDataType, out xml))
            {
                return xml;
            }
            return null;
        }

        /// <summary>
        /// 取得商家代號(學校) XML 反序列化資料
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public SchoolRTypeEntity DeSchoolXml(string xml)
        {
            object data = null;
            if (!String.IsNullOrWhiteSpace(xml) && Common.TryDeXmlExplicitly2(xml, _SchoolDataType, out data))
            {
                return data as SchoolRTypeEntity;
            }
            return null;
        }
        #endregion

        #region StudentMasterEntity
        private Type _StudentDataType = typeof(StudentMasterEntity);

        /// <summary>
        /// 取得學生資料的序列化 XML
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        public string GetStudentXml(StudentMasterEntity data)
        {
            string xml = null;
            if (data != null && Common.TryToXmlExplicitly2(data, _StudentDataType, out xml))
            {
                return xml;
            }
            return null;
        }

        /// <summary>
        /// 取得學生 XML 反序列化資料
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public StudentMasterEntity DeStudentXml(string xml)
        {
            object data = null;
            if (!String.IsNullOrWhiteSpace(xml) && Common.TryDeXmlExplicitly2(xml, _StudentDataType, out data))
            {
                return data as StudentMasterEntity;
            }
            return null;
        }
        #endregion

        #region SchoolRidView3
        private Type _SchoolRidDataType = typeof(SchoolRidView3);

        /// <summary>
        /// 取得費用別資料的序列化 XML
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        public string GetSchoolRidXml(SchoolRidView3 schoolRid)
        {
            string xml = null;
            if (schoolRid != null && Common.TryToXmlExplicitly2(schoolRid, _SchoolRidDataType, out xml))
            {
                return xml;
            }
            return null;
        }

        /// <summary>
        /// 取得費用別 XML 反序列化資料
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public SchoolRidView3 DeSchoolRidXml(string xml)
        {
            object schoolRid = null;
            if (!String.IsNullOrWhiteSpace(xml) && Common.TryDeXmlExplicitly2(xml, _SchoolRidDataType, out schoolRid))
            {
                return schoolRid as SchoolRidView3;
            }
            return null;
        }
        #endregion

        #region StudentReceiveView
        private Type _StudentReceiveDataType = typeof(StudentReceiveView);

        /// <summary>
        /// 取得學生繳費資料的序列化 XML
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetStudentReceiveXml(StudentReceiveView data)
        {
            string xml = null;
            if (data != null && Common.TryToXmlExplicitly2(data, _StudentReceiveDataType, out xml))
            {
                return xml;
            }
            return null;
        }

        /// <summary>
        /// 取得費用別 XML 反序列化資料
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public StudentReceiveView DeStudentReceiveXml(string xml)
        {
            object data = null;
            if (!String.IsNullOrWhiteSpace(xml) && Common.TryDeXmlExplicitly2(xml, _StudentReceiveDataType, out data))
            {
                return data as StudentReceiveView;
            }
            return null;
        }
        #endregion

        public string GetSubTotalAmountString(ICollection<SubTotalAmount> datas)
        {
            StringBuilder sb = new StringBuilder();
            if (datas != null && datas.Count > 0)
            {
                foreach (SubTotalAmount data in datas)
                {
                    sb.AppendFormat("{0},{1},{2};", this.ConvertSplitChar(data.Id), this.ConvertSplitChar(data.Name), data.Amount);
                }
            }
            return sb.ToString();
        }

        public SubTotalAmount[] DeSubTotalAmountString(string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                char[] comma = new char[] { ',' };
                string[] values = text.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                List<SubTotalAmount> datas = new List<SubTotalAmount>(values.Length);
                foreach (string value in values)
                {
                    if (!String.IsNullOrWhiteSpace(value))
                    {
                        decimal amount = 0;
                        string[] txts = value.Trim().Split(comma);
                        if (txts.Length == 3 && Decimal.TryParse(txts[2], out amount))
                        {
                            SubTotalAmount data = new SubTotalAmount();
                            data.Id = this.RestoreSplitChar(txts[0]);
                            data.Name = this.RestoreSplitChar(txts[1]);
                            data.Amount = amount;
                            datas.Add(data);
                        }
                    }
                }
                return datas.ToArray();
            }
            else
            {
                return new SubTotalAmount[0];
            }
        }

        private string ConvertSplitChar(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            else
            {
                return text.Replace(",", "&#44").Replace(";", "&#59");
            }
        }

        private string RestoreSplitChar(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            else
            {
                return text.Replace("&#44", ",").Replace("&#59", ";");
            }
        }
        #endregion

        #region 快取資料相關
        /// <summary>
        /// 管道代碼名稱快取
        /// </summary>
        KeyValueList<string> _CacheChannels = null;
        /// <summary>
        /// 取得管道代碼名稱
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        private string GetChannels(EntityFactory factory, out KeyValueList<string> channels)
        {
            string errmsg = null;
            if (_CacheChannels == null)
            {
                Expression where = new Expression();
                ChannelSetEntity[] datas = null;
                Result result = factory.SelectAll<ChannelSetEntity>(where, null, out datas);
                if (result.IsSuccess)
                {
                    if (datas != null && datas.Length > 0)
                    {
                        _CacheChannels = new KeyValueList<string>(datas.Length);
                        foreach(ChannelSetEntity data in datas)
                        {
                            _CacheChannels.Add(data.ChannelId, data.ChannelName);
                        }
                    }
                    else
                    {
                        _CacheChannels = new KeyValueList<string>(0);
                    }
                }
                else
                {
                    errmsg = String.Format("讀取代收管道資料失敗，錯誤訊息：{0}", result.Message);
                }
            }
            channels = _CacheChannels == null ? new KeyValueList<string>(0) : new KeyValueList<string>(_CacheChannels);
            return errmsg;
        }

        /// <summary>
        /// 分行代碼名稱快取
        /// </summary>
        KeyValueList<string> _CacheBanks = null;
        /// <summary>
        /// 取得分行代碼名稱
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        private string GetBanks(EntityFactory factory, out KeyValueList<string> banks)
        {
            string errmsg = null;
            if (_CacheBanks == null)
            {
                Expression where = new Expression();
                BankEntity[] datas = null;
                Result result = factory.SelectAll<BankEntity>(where, null, out datas);
                if (result.IsSuccess)
                {
                    if (datas != null && datas.Length > 0)
                    {
                        _CacheBanks = new KeyValueList<string>(datas.Length);
                        foreach (BankEntity data in datas)
                        {
                            _CacheBanks.Add(data.BankNo, data.BankSName);
                        }
                    }
                    else
                    {
                        _CacheBanks = new KeyValueList<string>(0);
                    }
                }
                else
                {
                    errmsg = String.Format("讀取代收管道資料失敗，錯誤訊息：{0}", result.Message);
                }
            }
            banks = _CacheBanks == null ? new KeyValueList<string>(0) : new KeyValueList<string>(_CacheBanks);
            return errmsg;
        }

        string _CacheReceiveSumKey = null;
        ReceiveSumEntity[] _CacheReceiveSums = null;
        /// <summary>
        /// 取得小計設定資料
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="receiveSums"></param>
        /// <returns></returns>
        private string GetReceiveSums(EntityFactory factory, string receiveType, string yearId, string termId, string depId, string receiveId, out ReceiveSumEntity[] receiveSums)
        {
            receiveSums = null;
            if (factory == null || !factory.IsReady())
            {
                return "缺少或無效的資料存取物件參數";
            }
            if (String.IsNullOrWhiteSpace(receiveType) || String.IsNullOrWhiteSpace(yearId) || String.IsNullOrWhiteSpace(termId) || depId == null || String.IsNullOrWhiteSpace(receiveId))
            {
                //沒有參數一定沒有資料
                return null;
            }
            receiveType = receiveType.Trim();
            yearId = yearId.Trim();
            termId = termId.Trim();
            depId = depId.Trim();
            receiveId = receiveId.Trim();

            string errmsg = null;
            string key = String.Format("{0},{1},{2},{3},{4}", receiveType, yearId, termId, depId, receiveId);
            if (_CacheReceiveSumKey != key)
            {
                Expression where = new Expression(ReceiveSumEntity.Field.ReceiveType, receiveType)
                    .And(ReceiveSumEntity.Field.YearId, yearId)
                    .And(ReceiveSumEntity.Field.TermId, termId)
                    .And(ReceiveSumEntity.Field.DepId, depId)
                    .And(ReceiveSumEntity.Field.ReceiveId, receiveId);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(ReceiveSumEntity.Field.SumId, OrderByEnum.Asc);
                ReceiveSumEntity[] datas = null;
                Result result = factory.SelectAll<ReceiveSumEntity>(where, orderbys, out datas);
                if (result.IsSuccess)
                {
                    _CacheReceiveSumKey = key;
                    _CacheReceiveSums = datas;
                }
                else
                {
                    errmsg = result.Message;
                }
            }
            receiveSums = _CacheReceiveSums;
            return errmsg;
        }

        string _CacheNameKey = null;
        KeyValueList<string> _CacheTermCodeNames = null;
        KeyValueList<string> _CacheReceiveCodeNames = null;
        KeyValueList<string> _CacheDeptCodeNames = null;
        KeyValueList<string> _CacheCollegeCodeNames = null;
        KeyValueList<string> _CacheMajorCodeNames = null;
        KeyValueList<string> _CacheClassCodeNames = null;
        private string GetAllCacheNames(EntityFactory factory, string receiveType, string yearId)
        {
            receiveType = receiveType == null ? String.Empty : receiveType.Trim();
            yearId = yearId == null ? String.Empty : yearId.Trim();
            string nameKey = String.Format("{0},{1}", receiveType, yearId);
            if (_CacheNameKey != nameKey)
            {
                #region Clear
                _CacheNameKey = null;
                if (_CacheTermCodeNames != null)
                {
                    _CacheTermCodeNames.Clear();
                }
                if (_CacheReceiveCodeNames != null)
                {
                    _CacheReceiveCodeNames.Clear();
                }
                if (_CacheDeptCodeNames != null)
                {
                    _CacheDeptCodeNames.Clear();
                }
                if (_CacheCollegeCodeNames != null)
                {
                    _CacheCollegeCodeNames.Clear();
                }
                if (_CacheMajorCodeNames != null)
                {
                    _CacheMajorCodeNames.Clear();
                }
                #endregion

                if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId))
                {
                    Result result = null;

                    #region TermListEntity
                    {
                        TermListEntity[] datas = null;
                        Expression where = new Expression(TermListEntity.Field.ReceiveType, receiveType)
                            .And(TermListEntity.Field.YearId, yearId);
                        result = factory.SelectAll<TermListEntity>(where, null, out datas);
                        if (result.IsSuccess)
                        {
                            if (datas != null && datas.Length > 0)
                            {
                                _CacheTermCodeNames = new KeyValueList<string>(datas.Length);
                                foreach (TermListEntity data in datas)
                                {
                                    string key = String.Format("{0}_{1}_{2}", data.ReceiveType, data.YearId, data.TermId);
                                    _CacheTermCodeNames.Add(key, data.TermName);
                                }
                            }
                        }
                        else
                        {
                            return result.Message;
                        }
                    }
                    #endregion

                    #region ReceiveListEntity
                    {
                        ReceiveListEntity[] datas = null;
                        Expression where = new Expression(ReceiveListEntity.Field.ReceiveType, receiveType)
                            .And(ReceiveListEntity.Field.YearId, yearId);
                        result = factory.SelectAll<ReceiveListEntity>(where, null, out datas);
                        if (result.IsSuccess)
                        {
                            if (datas != null && datas.Length > 0)
                            {
                                _CacheReceiveCodeNames = new KeyValueList<string>(datas.Length);
                                foreach (ReceiveListEntity data in datas)
                                {
                                    string key = String.Format("{0}_{1}_{2}_{3}_{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);
                                    _CacheReceiveCodeNames.Add(key, data.ReceiveName);
                                }
                            }
                        }
                        else
                        {
                            return result.Message;
                        }
                    }
                    #endregion

                    #region DeptListEntity
                    {
                        DeptListEntity[] datas = null;
                        Expression where = new Expression(DeptListEntity.Field.ReceiveType, receiveType)
                            .And(DeptListEntity.Field.YearId, yearId);
                        result = factory.SelectAll<DeptListEntity>(where, null, out datas);
                        if (result.IsSuccess)
                        {
                            if (datas != null && datas.Length > 0)
                            {
                                _CacheDeptCodeNames = new KeyValueList<string>(datas.Length);
                                foreach (DeptListEntity data in datas)
                                {
                                    string key = String.Format("{0}_{1}_{2}_{3}", data.ReceiveType, data.YearId, data.TermId, data.DeptId);
                                    _CacheDeptCodeNames.Add(key, data.DeptName);
                                }
                            }
                        }
                        else
                        {
                            return result.Message;
                        }
                    }
                    #endregion

                    #region CollegeListEntity
                    {
                        CollegeListEntity[] datas = null;
                        Expression where = new Expression(CollegeListEntity.Field.ReceiveType, receiveType)
                            .And(CollegeListEntity.Field.YearId, yearId);
                        result = factory.SelectAll<CollegeListEntity>(where, null, out datas);
                        if (result.IsSuccess)
                        {
                            if (datas != null && datas.Length > 0)
                            {
                                _CacheCollegeCodeNames = new KeyValueList<string>(datas.Length);
                                foreach (CollegeListEntity data in datas)
                                {
                                    string key = String.Format("{0}_{1}_{2}_{3}_{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.CollegeId);
                                    _CacheCollegeCodeNames.Add(key, data.CollegeName);
                                }
                            }
                        }
                        else
                        {
                            return result.Message;
                        }
                    }
                    #endregion

                    #region MajorListEntity
                    {
                        MajorListEntity[] datas = null;
                        Expression where = new Expression(MajorListEntity.Field.ReceiveType, receiveType)
                            .And(MajorListEntity.Field.YearId, yearId);
                        result = factory.SelectAll<MajorListEntity>(where, null, out datas);
                        if (result.IsSuccess)
                        {
                            if (datas != null && datas.Length > 0)
                            {
                                _CacheMajorCodeNames = new KeyValueList<string>(datas.Length);
                                foreach (MajorListEntity data in datas)
                                {
                                    string key = String.Format("{0}_{1}_{2}_{3}_{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.MajorId);
                                    _CacheMajorCodeNames.Add(key, data.MajorName);
                                }
                            }
                        }
                        else
                        {
                            return result.Message;
                        }
                    }
                    #endregion

                    #region ClassListEntity
                    {
                        ClassListEntity[] datas = null;
                        Expression where = new Expression(ClassListEntity.Field.ReceiveType, receiveType)
                            .And(ClassListEntity.Field.YearId, yearId);
                        result = factory.SelectAll<ClassListEntity>(where, null, out datas);
                        if (result.IsSuccess)
                        {
                            if (datas != null && datas.Length > 0)
                            {
                                _CacheClassCodeNames = new KeyValueList<string>(datas.Length);
                                foreach (ClassListEntity data in datas)
                                {
                                    string key = String.Format("{0}_{1}_{2}_{3}_{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ClassId);
                                    _CacheClassCodeNames.Add(key, data.ClassName);
                                }
                            }
                        }
                        else
                        {
                            return result.Message;
                        }
                    }
                    #endregion

                    _CacheNameKey = nameKey;
                }
            }
            return null;
        }

        #region [Old]
        //private string GetAllCodeNames(EntityFactory factory, StudentReceiveView data, out string termName, out string receiveName
        //    , out string deptName, out string collegeName, out string majorName, out string className)
        //{
        //    termName = null;
        //    receiveName = null;
        //    deptName = null;
        //    collegeName = null;
        //    majorName = null;
        //    className = null;
        //    string errmsg = this.GetAllCacheNames(factory, data.ReceiveType, data.YearId);
        //    if (!String.IsNullOrEmpty(errmsg))
        //    {
        //        return errmsg;
        //    }

        //    #region termName
        //    if (_CacheTermCodeNames != null && _CacheTermCodeNames.Count > 0)
        //    {
        //        string key = String.Format("{0}_{1}_{2}", data.ReceiveType, data.YearId, data.TermId);
        //        termName = _CacheTermCodeNames.TryGetValue(key, null);
        //    }
        //    #endregion

        //    #region receiveName
        //    if (_CacheReceiveCodeNames != null && _CacheReceiveCodeNames.Count > 0)
        //    {
        //        string key = String.Format("{0}_{1}_{2}_{3}_{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);
        //        receiveName = _CacheReceiveCodeNames.TryGetValue(key, null);
        //    }
        //    #endregion

        //    #region deptName
        //    if (_CacheDeptCodeNames != null && _CacheDeptCodeNames.Count > 0)
        //    {
        //        string key = String.Format("{0}_{1}_{2}_{3}", data.ReceiveType, data.YearId, data.TermId, data.DeptId);
        //        deptName = _CacheDeptCodeNames.TryGetValue(key, null);
        //    }
        //    #endregion

        //    #region collegeName
        //    if (_CacheCollegeCodeNames != null && _CacheCollegeCodeNames.Count > 0)
        //    {
        //        string key = String.Format("{0}_{1}_{2}_{3}_{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);
        //        collegeName = _CacheReceiveCodeNames.TryGetValue(key, null);
        //    }
        //    #endregion

        //    #region majorName
        //    if (_CacheMajorCodeNames != null && _CacheMajorCodeNames.Count > 0)
        //    {
        //        string key = String.Format("{0}_{1}_{2}_{3}_{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.MajorId);
        //        majorName = _CacheMajorCodeNames.TryGetValue(key, null);
        //    }
        //    #endregion

        //    #region className
        //    if (_CacheClassCodeNames != null && _CacheClassCodeNames.Count > 0)
        //    {
        //        string key = String.Format("{0}_{1}_{2}_{3}_{4}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.MajorId);
        //        className = _CacheClassCodeNames.TryGetValue(key, null);
        //    }
        //    #endregion

        //    return null;
        //}
        #endregion
        #endregion

        #region Inner Class
        private class LogWriter
        {
            private string _LogFile = String.Empty;
            private int failCount = 0;

            private bool _IsReady = false;
            public bool IsReady
            {
                get
                {
                    return _IsReady;
                }
            }

            public LogWriter(string logFile)
            {
                _LogFile = logFile == null ? String.Empty : logFile.Trim();
                if (_LogFile.Length > 0)
                {
                    try
                    {
                        if (Path.IsPathRooted(_LogFile))
                        {
                            string logPath = System.IO.Path.GetDirectoryName(_LogFile);
                            if (!System.IO.Directory.Exists(logPath))
                            {
                                System.IO.Directory.CreateDirectory(logPath);
                            }
                        }
                        else
                        {
                            _LogFile = String.Empty;
                        }
                    }
                    catch
                    {
                        _LogFile = String.Empty;
                    }
                }
                _IsReady = _LogFile.Length > 0;
            }

            public void Write(string format, params object[] args)
            {
                if (!this.IsReady)
                {
                    return;
                }

                if (args != null && args.Length > 0)
                {
                    StringBuilder log = new StringBuilder();
                    log
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now)
                        .AppendFormat(format, args);
                    WriteToFile(log.ToString());
                }
            }

            public void Write(string format, params string[] msgs)
            {
                if (!this.IsReady)
                {
                    return;
                }

                if (_IsReady && msgs != null && msgs.Length > 0)
                {
                    StringBuilder log = new StringBuilder();
                    log
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now)
                        .AppendFormat(format, msgs);
                    WriteToFile(log.ToString());
                }
            }

            public void Write(string msg)
            {
                if (!this.IsReady)
                {
                    return;
                }

                if (msg != null && msg.Length > 0)
                {
                    string log = String.Format("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, msg);
                    WriteToFile(log);
                }
                else
                {
                    WriteToFile(String.Empty);
                }
            }

            List<string> _Msgs = new List<string>();
            public void PushMsg(string msg)
            {
                if (!this.IsReady)
                {
                    return;
                }

                if (msg != null && msg.Length > 0)
                {
                    _Msgs.Add(String.Format("[{0:yyyy/MM/dd HH:mm:ss}] {1}", DateTime.Now, msg));
                }
                else
                {
                    _Msgs.Add(String.Empty);
                }
            }

            public void PushMsg(string format, params object[] args)
            {
                if (!this.IsReady)
                {
                    return;
                }

                if (args != null && args.Length > 0)
                {
                    StringBuilder log = new StringBuilder();
                    log
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] ", DateTime.Now)
                        .AppendFormat(format, args);
                    _Msgs.Add(log.ToString());
                }
            }

            public void PushWrite()
            {
                if (_Msgs.Count > 0)
                {
                    WriteToFile(String.Join("\r\n", _Msgs.ToArray()));
                    _Msgs.Clear();
                }
            }

            private void WriteToFile(string log)
            {
                this.WriteToFile(new string[] { log });
            }

            private void WriteToFile(string[] logs)
            {
                if (_IsReady && logs != null && logs.Length > 0)
                {
                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(_LogFile, true, System.Text.Encoding.Default))
                        {
                            foreach (string log in logs)
                            {
                                sw.WriteLine(log ?? String.Empty);
                            }
                        }
                    }
                    catch
                    {
                        failCount++;
                        if (failCount > 10)
                        {
                            _IsReady = false;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
