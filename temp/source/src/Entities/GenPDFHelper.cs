using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    public sealed class GenPDFHelper
    {
        #region Const
        /// <summary>
        /// PDF 每個檔案最大筆數
        /// </summary>
        public const int PDF_FILE_MAX_RECORD = 4000;
        #endregion

        #region Emun
        /// <summary>
        /// PDF 種類
        /// </summary>
        public enum PDFType
        {
            /// <summary>
            /// 未指定
            /// </summary>
            None = 0,
            /// <summary>
            /// 繳費單
            /// </summary>
            Bill = 1,
            /// <summary>
            /// 收據
            /// </summary>
            Receipt = 2,
        }
        #endregion

        #region Constructor
        public GenPDFHelper()
        {
        }
        #endregion

        public Result GenPDFFiles(string data_file, string template_file, string pdf_file)
        {
            string[] xmlFileFullNames = new string[] { data_file };
            string[] templetFileFullNames = new string[] { template_file };
            string[] pdfFileFullNames = new string[] { pdf_file };
            string[] outPdfFiles = null;
            DateTime startTime;
            DateTime endTime;
            Result result = this.CallPDFService(xmlFileFullNames, templetFileFullNames, pdfFileFullNames, true
                , out startTime, out endTime, out outPdfFiles);
            return result;
        }

        #region [MDY:202203XX] 2022擴充案 是否英文介面
        /// <summary>
        /// 取得符合指定條件的 StudentReceiveView 集合
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="qType"></param>
        /// <param name="qValue"></param>
        /// <param name="allAmount"></param>
        /// <param name="pdfType"></param>
        /// <param name="module"></param>
        /// <param name="isEngEnabled"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        private Result GetStudentReceiveViews(EntityFactory factory
            , string receiveType, string yearId, string termId, string depId, string receiveId
            , string qType, string qValue, bool allAmount, GenPDFHelper.PDFType pdfType
            , CancelNoHelper.Module module, bool isEngEnabled, out StudentReceiveView[] datas)
        {
            datas = null;

            #region 組 SQL
            KeyValueList parameters = new KeyValueList(6);

            parameters.Add("@RECEIVE_TYPE", receiveType);
            parameters.Add("@YEAR_ID", yearId);
            parameters.Add("@TERM_ID", termId);
            parameters.Add("@DEP_ID", depId);
            parameters.Add("@RECEIVE_ID", receiveId);

            string orderbySql = null;
            List<string> andSqls = new List<string>(5);

            string errmsg = null;
            switch (qType)
            {
                case "1":
                    #region 產生所有繳費單
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.UpNo}, {StudentReceiveView.Field.UpOrder}";
                    }
                    #endregion
                    break;
                case "2":
                    #region 自訂產生繳費單流水號
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.SeriorNo}";

                        if (String.IsNullOrWhiteSpace(qValue))
                        {
                            errmsg = "批次處理序列缺少條件值的參數或參數值不正確";
                            return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                        }
                        string[] sNoRanges = qValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sNoRanges.Length != 2)
                        {
                            errmsg = "批次處理序列缺少條件值的參數或參數值不正確";
                            return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                        }
                        andSqls.Add($"   AND (@SERIORNO_S <= {StudentReceiveView.Field.SeriorNo} AND {StudentReceiveView.Field.SeriorNo} <= @SERIORNO_E)");
                        parameters.Add("@SERIORNO_S", sNoRanges[0].PadLeft(module.SeriorNoSize, '0'));
                        parameters.Add("@SERIORNO_E", sNoRanges[1].PadLeft(module.SeriorNoSize, '0'));
                    }
                    #endregion
                    break;
                case "3":
                    #region 依批號產生
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.UpOrder}";

                        andSqls.Add($"   AND {StudentReceiveView.Field.UpNo} = @UP_NO");
                        parameters.Add("@UP_NO", qValue);
                    }
                    #endregion
                    break;
                case "4":
                    #region 依學號產生
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.OldSeq}";

                        andSqls.Add($"   AND {StudentReceiveView.Field.StuId} = @STU_ID");
                        parameters.Add("@STU_ID", qValue);
                    }
                    #endregion
                    break;
                case "5":
                    #region 依科系產生
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.UpNo}, {StudentReceiveView.Field.UpOrder}";

                        andSqls.Add($"   AND {StudentReceiveView.Field.MajorId} = @MAJOR_ID");
                        parameters.Add("@MAJOR_ID", qValue);
                    }
                    #endregion
                    break;
                case "6":
                    #region 依年級產生
                    {
                        orderbySql = $" ORDER BY {StudentReceiveView.Field.UpNo}, {StudentReceiveView.Field.UpOrder}";

                        andSqls.Add($"   AND {StudentReceiveView.Field.StuGrade} = @STU_GRADE");
                        parameters.Add("@STU_GRADE", qValue);
                    }
                    #endregion
                    break;
                default:
                    errmsg = "批次處理序列缺少上條件類別參數或參數值不正確";
                    return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            //只取應繳金額大於 0 的資料
            if (!allAmount)
            {
                andSqls.Add($"   AND {StudentReceiveView.Field.ReceiveAmount} > 0");
            }

            if (pdfType == GenPDFHelper.PDFType.Bill)
            {
                //繳費單：只取未繳費
                andSqls.Add($"   AND ({StudentReceiveView.Field.ReceiveDate} IS NULL OR {StudentReceiveView.Field.ReceiveDate} = '')");
            }
            else
            {
                //收據：只取已銷
                andSqls.Add($"   AND ({StudentReceiveView.Field.AccountDate} IS NOT NULL AND {StudentReceiveView.Field.AccountDate} != '')");
            }

            andSqls.Add(orderbySql);

            string sql = null;
            if (isEngEnabled)
            {
                sql = $@"
SELECT SR.*
     , ISNULL(RI.[Bill_Valid_Date], '')    AS [Bill_Valid_Date]
     , ISNULL(RI.[Bill_Close_Date], '')    AS [Bill_Close_Date]
     , ISNULL(RI.[Invoice_Close_Date], '') AS [Invoice_Close_Date]
     , ISNULL(RI.[Pay_Date], '')           AS [Pay_Due_Date1]
     , ISNULL(RI.[Pay_Due_Date2], '')      AS [Pay_Due_Date2]
     , ISNULL(RI.[Pay_Due_Date3], '')      AS [Pay_Due_Date3]
     , ISNULL((SELECT [Stu_Name] FROM [{StudentMasterEntity.TABLE_NAME}] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Year_EName],     [Year_Name])     ELSE [Year_Name]     END) FROM [Year_List]      AS YL  WHERE YL.[Year_Id]       = SR.[Year_Id]), '')                                                                                                                                                   AS [Year_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Term_EName],     [Term_Name])     ELSE [Term_Name]     END) FROM [Term_List]      AS TL  WHERE TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]), '')                                                                            AS [Term_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Dept_EName],     [Dept_Name])     ELSE [Dept_Name]     END) FROM [Dept_List]      AS DTL WHERE DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]), '')                                           AS [Dept_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Receive_EName],  [Receive_Name])  ELSE [Receive_Name]  END) FROM [Receive_List]   AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id]   = SR.[Receive_Id]), '')    AS [Receive_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([College_EName],  [College_Name])  ELSE [College_Name]  END) FROM [College_List]   AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]), '')    AS [College_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Major_EName],    [Major_Name])    ELSE [Major_Name]    END) FROM [Major_List]     AS ML  WHERE ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]), '')      AS [Major_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Class_EName],    [Class_Name])    ELSE [Class_Name]    END) FROM [Class_List]     AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[Class_Id]     = SR.[Class_Id]), '')      AS [Class_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Reduce_EName],   [Reduce_Name])   ELSE [Reduce_Name]   END) FROM [Reduce_List]    AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Reduce_Id]    = SR.[Reduce_Id]), '')     AS [Reduce_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Dorm_EName],     [Dorm_Name])     ELSE [Dorm_Name]     END) FROM [Dorm_List]      AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]), '')       AS [Dorm_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Loan_EName],     [Loan_Name])     ELSE [Loan_Name]     END) FROM [Loan_List]      AS LL  WHERE LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]), '')       AS [Loan_Name]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List1] AS IL1 WHERE IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]), '') AS [Identify_Name01]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List2] AS IL2 WHERE IL2.[Receive_Type] = SR.[Receive_Type] AND IL2.[Year_Id] = SR.[Year_Id] AND IL2.[Term_Id] = SR.[Term_Id] AND IL2.[Dep_Id]  = SR.[Dep_Id] AND IL2.[Identify_Id] = SR.[Identify_Id02]), '') AS [Identify_Name02]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List3] AS IL3 WHERE IL3.[Receive_Type] = SR.[Receive_Type] AND IL3.[Year_Id] = SR.[Year_Id] AND IL3.[Term_Id] = SR.[Term_Id] AND IL3.[Dep_Id]  = SR.[Dep_Id] AND IL3.[Identify_Id] = SR.[Identify_Id03]), '') AS [Identify_Name03]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List4] AS IL4 WHERE IL4.[Receive_Type] = SR.[Receive_Type] AND IL4.[Year_Id] = SR.[Year_Id] AND IL4.[Term_Id] = SR.[Term_Id] AND IL4.[Dep_Id]  = SR.[Dep_Id] AND IL4.[Identify_Id] = SR.[Identify_Id04]), '') AS [Identify_Name04]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List5] AS IL5 WHERE IL5.[Receive_Type] = SR.[Receive_Type] AND IL5.[Year_Id] = SR.[Year_Id] AND IL5.[Term_Id] = SR.[Term_Id] AND IL5.[Dep_Id]  = SR.[Dep_Id] AND IL5.[Identify_Id] = SR.[Identify_Id05]), '') AS [Identify_Name05]
     , ISNULL((SELECT (CASE WHEN SC.Eng_Enabled = 'Y' THEN ISNULL([Identify_EName], [Identify_Name]) ELSE [Identify_Name] END) FROM [Identify_List6] AS IL6 WHERE IL6.[Receive_Type] = SR.[Receive_Type] AND IL6.[Year_Id] = SR.[Year_Id] AND IL6.[Term_Id] = SR.[Term_Id] AND IL6.[Dep_Id]  = SR.[Dep_Id] AND IL6.[Identify_Id] = SR.[Identify_Id06]), '') AS [Identify_Name06]
     , ISNULL((SELECT [Channel_Name] FROM [{ChannelSetEntity.TABLE_NAME}] AS CS WHERE CS.[Channel_Id] = SR.[Receive_Way]), '') AS [Receive_Way_Name]
  FROM [{StudentReceiveEntity.TABLE_NAME}] AS SR
  LEFT JOIN [{SchoolRidEntity.TABLE_NAME}] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]
  LEFT JOIN [{SchoolRTypeEntity.TABLE_NAME}] AS SC ON SC.[Receive_Type] = SR.[Receive_Type]
 WHERE SR.[Receive_Type] = @RECEIVE_TYPE AND SR.[Year_Id] = @YEAR_ID AND SR.[Term_Id] = @TERM_ID AND SR.[Dep_Id] = @DEP_ID AND SR.[Receive_Id] = @RECEIVE_ID
{(andSqls.Count == 0 ? null : String.Join(Environment.NewLine, andSqls))}
".Trim();
            }
            else
            {
                sql = $@"
SELECT SR.*
     , ISNULL(RI.[Bill_Valid_Date], '')    AS [Bill_Valid_Date]
     , ISNULL(RI.[Bill_Close_Date], '')    AS [Bill_Close_Date]
     , ISNULL(RI.[Invoice_Close_Date], '') AS [Invoice_Close_Date]
     , ISNULL(RI.[Pay_Date], '')           AS [Pay_Due_Date1]
     , ISNULL(RI.[Pay_Due_Date2], '')      AS [Pay_Due_Date2]
     , ISNULL(RI.[Pay_Due_Date3], '')      AS [Pay_Due_Date3]
     , ISNULL((SELECT [Stu_Name] FROM [{StudentMasterEntity.TABLE_NAME}] AS SM WHERE SM.[Receive_Type] = SR.[Receive_Type] AND SM.[Dep_Id] = SR.[Dep_Id] AND SM.[Stu_Id] = SR.[Stu_Id]), '') AS [Stu_Name]
     , ISNULL((SELECT [Year_Name]     FROM [Year_List]      AS YL  WHERE YL.[Year_Id]       = SR.[Year_Id]), '')                                                                                                                                                   AS [Year_Name]
     , ISNULL((SELECT [Term_Name]     FROM [Term_List]      AS TL  WHERE TL.[Receive_Type]  = SR.[Receive_Type] AND TL.[Year_Id]  = SR.[Year_Id] AND TL.[Term_Id]  = SR.[Term_Id]), '')                                                                            AS [Term_Name]
     , ISNULL((SELECT [Dept_Name]     FROM [Dept_List]      AS DTL WHERE DTL.[Receive_Type] = SR.[Receive_Type] AND DTL.[Year_Id] = SR.[Year_Id] AND DTL.[Term_Id] = SR.[Term_Id] AND DTL.[Dept_Id] = SR.[Dept_Id]), '')                                           AS [Dept_Name]
     , ISNULL((SELECT [Receive_Name]  FROM [Receive_List]   AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Receive_Id]   = SR.[Receive_Id]), '')    AS [Receive_Name]
     , ISNULL((SELECT [College_Name]  FROM [College_List]   AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[College_Id]   = SR.[College_Id]), '')    AS [College_Name]
     , ISNULL((SELECT [Major_Name]    FROM [Major_List]     AS ML  WHERE ML.[Receive_Type]  = SR.[Receive_Type] AND ML.[Year_Id]  = SR.[Year_Id] AND ML.[Term_Id]  = SR.[Term_Id] AND ML.[Dep_Id]   = SR.[Dep_Id] AND ML.[Major_Id]     = SR.[Major_Id]), '')      AS [Major_Name]
     , ISNULL((SELECT [Class_Name]    FROM [Class_List]     AS CL  WHERE CL.[Receive_Type]  = SR.[Receive_Type] AND CL.[Year_Id]  = SR.[Year_Id] AND CL.[Term_Id]  = SR.[Term_Id] AND CL.[Dep_Id]   = SR.[Dep_Id] AND CL.[Class_Id]     = SR.[Class_Id]), '')      AS [Class_Name]
     , ISNULL((SELECT [Reduce_Name]   FROM [Reduce_List]    AS RL  WHERE RL.[Receive_Type]  = SR.[Receive_Type] AND RL.[Year_Id]  = SR.[Year_Id] AND RL.[Term_Id]  = SR.[Term_Id] AND RL.[Dep_Id]   = SR.[Dep_Id] AND RL.[Reduce_Id]    = SR.[Reduce_Id]), '')     AS [Reduce_Name]
     , ISNULL((SELECT [Dorm_Name]     FROM [Dorm_List]      AS DL  WHERE DL.[Receive_Type]  = SR.[Receive_Type] AND DL.[Year_Id]  = SR.[Year_Id] AND DL.[Term_Id]  = SR.[Term_Id] AND DL.[Dep_Id]   = SR.[Dep_Id] AND DL.[Dorm_Id]      = SR.[Dorm_Id]), '')       AS [Dorm_Name]
     , ISNULL((SELECT [Loan_Name]     FROM [Loan_List]      AS LL  WHERE LL.[Receive_Type]  = SR.[Receive_Type] AND LL.[Year_Id]  = SR.[Year_Id] AND LL.[Term_Id]  = SR.[Term_Id] AND LL.[Dep_Id]   = SR.[Dep_Id] AND LL.[Loan_Id]      = SR.[Loan_Id]), '')       AS [Loan_Name]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List1] AS IL1 WHERE IL1.[Receive_Type] = SR.[Receive_Type] AND IL1.[Year_Id] = SR.[Year_Id] AND IL1.[Term_Id] = SR.[Term_Id] AND IL1.[Dep_Id]  = SR.[Dep_Id] AND IL1.[Identify_Id] = SR.[Identify_Id01]), '') AS [Identify_Name01]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List2] AS IL2 WHERE IL2.[Receive_Type] = SR.[Receive_Type] AND IL2.[Year_Id] = SR.[Year_Id] AND IL2.[Term_Id] = SR.[Term_Id] AND IL2.[Dep_Id]  = SR.[Dep_Id] AND IL2.[Identify_Id] = SR.[Identify_Id02]), '') AS [Identify_Name02]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List3] AS IL3 WHERE IL3.[Receive_Type] = SR.[Receive_Type] AND IL3.[Year_Id] = SR.[Year_Id] AND IL3.[Term_Id] = SR.[Term_Id] AND IL3.[Dep_Id]  = SR.[Dep_Id] AND IL3.[Identify_Id] = SR.[Identify_Id03]), '') AS [Identify_Name03]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List4] AS IL4 WHERE IL4.[Receive_Type] = SR.[Receive_Type] AND IL4.[Year_Id] = SR.[Year_Id] AND IL4.[Term_Id] = SR.[Term_Id] AND IL4.[Dep_Id]  = SR.[Dep_Id] AND IL4.[Identify_Id] = SR.[Identify_Id04]), '') AS [Identify_Name04]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List5] AS IL5 WHERE IL5.[Receive_Type] = SR.[Receive_Type] AND IL5.[Year_Id] = SR.[Year_Id] AND IL5.[Term_Id] = SR.[Term_Id] AND IL5.[Dep_Id]  = SR.[Dep_Id] AND IL5.[Identify_Id] = SR.[Identify_Id05]), '') AS [Identify_Name05]
     , ISNULL((SELECT [Identify_Name] FROM [Identify_List6] AS IL6 WHERE IL6.[Receive_Type] = SR.[Receive_Type] AND IL6.[Year_Id] = SR.[Year_Id] AND IL6.[Term_Id] = SR.[Term_Id] AND IL6.[Dep_Id]  = SR.[Dep_Id] AND IL6.[Identify_Id] = SR.[Identify_Id06]), '') AS [Identify_Name06]
     , ISNULL((SELECT [Channel_Name] FROM [{ChannelSetEntity.TABLE_NAME}] AS CS WHERE CS.[Channel_Id] = SR.[Receive_Way]), '') AS [Receive_Way_Name]
  FROM [{StudentReceiveEntity.TABLE_NAME}] AS SR
  LEFT JOIN [{SchoolRidEntity.TABLE_NAME}] AS RI ON RI.[Receive_Type] = SR.[Receive_Type] AND RI.[Year_Id] = SR.[Year_Id] AND RI.[Term_Id] = SR.[Term_Id] AND RI.[Dep_Id] = SR.[Dep_Id] AND RI.[Receive_Id] = SR.[Receive_Id]
 WHERE SR.[Receive_Type] = @RECEIVE_TYPE AND SR.[Year_Id] = @YEAR_ID AND SR.[Term_Id] = @TERM_ID AND SR.[Dep_Id] = @DEP_ID AND SR.[Receive_Id] = @RECEIVE_ID
{(andSqls.Count == 0 ? null : String.Join(Environment.NewLine, andSqls))}
".Trim();
            }
            #endregion

            Result result = factory.SelectSql<StudentReceiveView>(sql, parameters, 0, 1, out datas);
            return result;
        }

        public Result GenPDFFiles(SchoolRTypeEntity school, SchoolRidEntity schoolRid
            , ReceiveChannelEntity[] receiveChannels, StudentReceiveView[] receives, StudentMasterEntity[] students
            , BankEntity bank, ReceiveSumEntity[] receiveSums, FiscQRCodeConfig qrcodeConfig, bool fgMark, bool isEngUI
            , PDFType pdfType, byte[] templetContent, string outPath, string userID
            , out DateTime startTime, out DateTime endTime, out string pdfFile)
        {
            startTime = DateTime.Now;
            endTime = startTime;
            pdfFile = null;

            #region 檢查參數
            if (school == null || schoolRid == null)
            {
                return new Result(false, "缺少業務別資料或代收費用別資料參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            if (school.ReceiveType != schoolRid.ReceiveType)
            {
                return new Result(false, "業務別資料與代收費用別資料不符合", CoreStatusCode.INVALID_PARAMETER, null);
            }

            if (receiveChannels == null || receiveChannels.Length == 0)
            {
                return new Result(false, "缺少業務別管道手續費資料參數", CoreStatusCode.INVALID_PARAMETER, null);
            }

            //if (receiveChannels == null)
            //{
            //    receiveChannels = new ReceiveChannelEntity[0];
            //}

            if (receives == null || receives.Length == 0)
            {
                return new Result(false, "缺少學生繳費資料參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            if (students == null || students.Length == 0)
            {
                return new Result(false, "缺少學生基本資料參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            if (pdfType == PDFType.None)
            {
                return new Result(false, "缺少或無效的PDF種類參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            if (templetContent == null || templetContent.Length == 0)
            {
                return new Result(false, "缺少模板資料參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            if (String.IsNullOrWhiteSpace(outPath))
            {
                return new Result(false, "缺少產出檔案目錄參數", CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 檢查目錄
            try
            {
                outPath = outPath.Trim();
                if (!Directory.Exists(outPath))
                {
                    Directory.CreateDirectory(outPath);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, "產出檔案目錄不存在，且無法建立，錯誤訊息：" + ex.Message, CoreStatusCode.UNKNOWN_EXCEPTION, ex);
            }
            #endregion

            #region 檢查 PKey
            {
                StudentReceiveView receive = receives[0];
                if (receive.ReceiveType != schoolRid.ReceiveType
                    || receive.YearId != schoolRid.YearId
                    || receive.TermId != schoolRid.TermId
                    || receive.DepId != schoolRid.DepId
                    || receive.ReceiveId != schoolRid.ReceiveId)
                {
                    return new Result(false, "學生繳費資料與代收費用別不符合", CoreStatusCode.INVALID_PARAMETER, null);
                }
                foreach (StudentReceiveView one in receives)
                {
                    if (receive.ReceiveType != one.ReceiveType
                        || receive.YearId != one.YearId
                        || receive.TermId != one.TermId
                        || receive.DepId != one.DepId
                        || receive.ReceiveId != one.ReceiveId)
                    {
                        return new Result(false, "學生繳費資料不一致", CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }

                foreach (ReceiveChannelEntity receiveChannel in receiveChannels)
                {
                    if (receiveChannel.ReceiveType != schoolRid.ReceiveType)
                    {
                        return new Result(false, "代收管道手續費資料與代收費用別不符合", CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }

                foreach (StudentMasterEntity student in students)
                {
                    if (student.ReceiveType != schoolRid.ReceiveType
                        || student.DepId != schoolRid.DepId)
                    {
                        return new Result(false, "學生基本資料與代收費用別不符合", CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }
            }
            #endregion

            #region 產生資料 Xml 與各檔案路徑
            string tmpBaseName = String.Format("{0}_{1:yyyyMMddHHmmssfff}", school.ReceiveType, DateTime.Now);
            string templetFileFullName = Path.Combine(outPath, tmpBaseName + ".Templet.PDF");
            string xmlFileFullName = Path.Combine(outPath, tmpBaseName + ".1.XML");
            Result result = null;
            switch (pdfType)
            {
                case PDFType.Bill:
                    #region [MDY:20171127] 財金 QRCode 支付 (20170831_01)
                    #region [Old]
                    //result = this.GenBillPDFXml(xmlFileFullName, school, schoolRid, receiveChannels, receives, students, bank, receiveSums, fgMark, userID);
                    #endregion

                    #region [MDY:202203XX] 2022擴充案 是否英文介面
                    result = this.GenBillPDFXml(xmlFileFullName, school, schoolRid, receiveChannels, receives, students, bank, receiveSums, qrcodeConfig, fgMark, isEngUI, userID);
                    #endregion
                    #endregion
                    break;
                case PDFType.Receipt:
                    #region [MDY:202203XX] 2022擴充案 是否英文介面
                    result = this.GenReceiptPDFXml(xmlFileFullName, school, schoolRid, receiveChannels, receives, students, fgMark, isEngUI, userID);
                    #endregion
                    break;
            }
            if (result.IsSuccess)
            {
                pdfFile = Path.Combine(outPath, tmpBaseName + ".PDF");
            }
            #endregion

            #region 處理各檔案
            if (result.IsSuccess)
            {
                try
                {
                    #region 模板
                    if (File.Exists(templetFileFullName))
                    {
                        File.Delete(templetFileFullName);
                    }
                    File.WriteAllBytes(templetFileFullName, templetContent);
                    #endregion

                    #region PDF 檔
                    if (File.Exists(pdfFile))
                    {
                        File.Delete(pdfFile);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    result = new Result(false, "處理檔案失敗：" + ex.Message, CoreStatusCode.UNKNOWN_EXCEPTION, ex);
                }
            }
            if (!result.IsSuccess)
            {
                return result;
            }
            #endregion

            #region 產生 PDF
            if (result.IsSuccess)
            {
                string[] xmlFileFullNames = new string[] { xmlFileFullName };
                string[] templetFileFullNames = new string[] { templetFileFullName };
                string[] pdfFileFullNames = new string[] { pdfFile };
                string[] outPdfFiles = null;
                result = this.CallPDFService(xmlFileFullNames, templetFileFullNames, pdfFileFullNames, true
                    , out startTime, out endTime, out outPdfFiles);
            }
            return result;
            #endregion
        }

        /// <summary>
        /// 分檔產生 PDF 檔
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="qType"></param>
        /// <param name="qValue"></param>
        /// <param name="allAmount"></param>
        /// <param name="fgMark"></param>
        /// <param name="pdfType"></param>
        /// <param name="outPath"></param>
        /// <param name="userId"></param>
        /// <param name="totalCount"></param>
        /// <param name="outPDFFiles"></param>
        /// <returns></returns>
        public Result GenPDFFiles(EntityFactory factory
            , string receiveType, string yearId, string termId, string depId, string receiveId
            , string qType, string qValue, bool allAmount, bool fgMark, bool isEngUI
            , PDFType pdfType, string outPath, string userId
            , out int totalCount, out string[] outPDFFiles)
        {
            totalCount = 0;
            outPDFFiles = null;
            string errmsg = null;

            #region 取得虛擬帳號模組資料
            //CancelNoHelper.Module module = CancelNoHelper.Module.GetByReceiveType(receiveType);
            CancelNoHelper cancelNoHelper = new Entities.CancelNoHelper();
            CancelNoHelper.Module module = cancelNoHelper.GetModuleByReceiveType(receiveType);
            if (module == null)
            {
                errmsg = String.Format("無法取得業務別碼 {0} 的虛擬帳號模組資訊", receiveType);
                return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region StudentReceiveView
            StudentReceiveView[] studentReceives = null;
            {
                #region [MDY:20220820] 2022擴充案 改用 GetStudentReceiveViews() 取英文資料
                #region [OLD]
                //KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(2);

                //Expression where = new Expression(StudentReceiveView.Field.ReceiveType, receiveType)
                //    .And(StudentReceiveView.Field.YearId, yearId)
                //    .And(StudentReceiveView.Field.TermId, termId)
                //    .And(StudentReceiveView.Field.DepId, depId)
                //    .And(StudentReceiveView.Field.ReceiveId, receiveId);
                //switch (qType)
                //{
                //    case "1":   //產生所有繳費單
                //        orderbys.Add(StudentReceiveView.Field.UpNo, OrderByEnum.Asc);
                //        orderbys.Add(StudentReceiveView.Field.UpOrder, OrderByEnum.Asc);
                //        //orderbys.Add(StudentReceiveView.Field.StuId, OrderByEnum.Asc);
                //        break;
                //    case "2":   //自訂產生繳費單流水號
                //        #region
                //        {
                //            orderbys.Add(StudentReceiveView.Field.SeriorNo, OrderByEnum.Asc);
                //            if (String.IsNullOrWhiteSpace(qValue))
                //            {
                //                errmsg = "批次處理序列缺少條件值的參數或參數值不正確";
                //                return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                //            }
                //            string[] sNoRanges = qValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //            if (sNoRanges.Length != 2)
                //            {
                //                errmsg = "批次處理序列缺少條件值的參數或參數值不正確";
                //                return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                //            }
                //            where
                //                .And(StudentReceiveView.Field.SeriorNo, RelationEnum.GreaterEqual, sNoRanges[0].PadLeft(module.SeriorNoSize, '0'))
                //                .And(StudentReceiveView.Field.SeriorNo, RelationEnum.LessEqual, sNoRanges[1].PadLeft(module.SeriorNoSize, '0'));
                //        }
                //        #endregion
                //        break;
                //    case "3":   //依批號產生
                //        orderbys.Add(StudentReceiveView.Field.UpOrder, OrderByEnum.Asc);
                //        where.And(StudentReceiveView.Field.UpNo, qValue);
                //        break;
                //    case "4":   //依學號產生
                //        where.And(StudentReceiveView.Field.StuId, qValue);
                //        break;
                //    case "5":   //依科系產生
                //        orderbys.Add(StudentReceiveView.Field.UpNo, OrderByEnum.Asc);
                //        orderbys.Add(StudentReceiveView.Field.UpOrder, OrderByEnum.Asc);
                //        where.And(StudentReceiveView.Field.MajorId, qValue);
                //        break;
                //    case "6":   //依年級產生
                //        orderbys.Add(StudentReceiveView.Field.UpNo, OrderByEnum.Asc);
                //        orderbys.Add(StudentReceiveView.Field.UpOrder, OrderByEnum.Asc);
                //        where.And(StudentReceiveView.Field.StuGrade, qValue);
                //        break;
                //    default:
                //        errmsg = "批次處理序列缺少上條件類別參數或參數值不正確";
                //        return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                //}

                ////只取應繳金額大於 0 的資料
                //if (!allAmount)
                //{
                //    where.And(StudentReceiveView.Field.ReceiveAmount, RelationEnum.Greater, 0);
                //}

                //if (pdfType == GenPDFHelper.PDFType.Bill)   //繳費單
                //{
                //    //未繳費
                //    where.And(new Expression(StudentReceiveView.Field.ReceiveDate, null).Or(StudentReceiveView.Field.ReceiveDate, String.Empty));
                //}
                //else
                //{
                //    //已銷
                //    where
                //        .And(StudentReceiveView.Field.AccountDate, RelationEnum.NotEqual, null)
                //        .And(StudentReceiveView.Field.AccountDate, RelationEnum.NotEqual, String.Empty);
                //}

                //Result result = factory.SelectAll<StudentReceiveView>(where, orderbys, out studentReceives);
                #endregion

                Result result = this.GetStudentReceiveViews(factory, receiveType, yearId, termId, depId, receiveId
                    , qType, qValue, allAmount, pdfType, module, isEngUI, out studentReceives);
                #endregion

                if (!result.IsSuccess)
                {
                    errmsg = "讀取繳費單資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (studentReceives == null || studentReceives.Length == 0)
                {
                    errmsg = "查無繳費單資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            totalCount = studentReceives == null ? 0 : studentReceives.Length;
            #endregion

            #region SchoolRTypeEntity
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取業務別資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    errmsg = "查無業務別資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 取消 SchoolRidView2 改用 SchoolRidEntity 就夠
            SchoolRidEntity schoolRid = null;
            {
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
                    .And(SchoolRidEntity.Field.YearId, yearId)
                    .And(SchoolRidEntity.Field.TermId, termId)
                    .And(SchoolRidEntity.Field.DepId, depId)
                    .And(SchoolRidEntity.Field.ReceiveId, receiveId);
                Result result = factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取代收費用別資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (schoolRid == null)
                {
                    errmsg = "查無代收費用別資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region ReceiveChannelEntity
            ReceiveChannelEntity[] receiveChannels = null;
            {
                Expression where = new Expression(ReceiveChannelEntity.Field.ReceiveType, receiveType);
                Result result = factory.SelectAll<ReceiveChannelEntity>(where, null, out receiveChannels);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取代收管道手續費資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (receiveChannels == null || receiveChannels.Length == 0)
                {
                    errmsg = "查無代收管道手續費資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region BankEntity
            BankEntity bank = null;
            {
                Expression where = null;
                string bankId = school.BankId == null ? String.Empty : school.BankId.Trim();
                if (bankId.Length == 7)
                {
                    where = new Expression(BankEntity.Field.FullCode, bankId);
                }
                else if (bankId.Length == 6)
                {
                    where = new Expression(BankEntity.Field.BankNo, bankId);
                }
                else if (bankId.Length == 3)
                {
                    where = new Expression(BankEntity.Field.BankNo, DataFormat.MyBankID + bankId);
                }
                else
                {
                    where = new Expression(BankEntity.Field.BankNo, RelationEnum.Like, DataFormat.MyBankID + bankId + "%");
                }
                Result result = factory.SelectFirst<BankEntity>(where, null, out bank);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取主辦行資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (bank == null)
                {
                    errmsg = "查無主辦行資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region [NEW:20151229] 合計項目 ReceiveSumEntity
            ReceiveSumEntity[] receiveSums = null;
            {
                Expression where = new Expression(ReceiveSumEntity.Field.ReceiveType, schoolRid.ReceiveType)
                    .And(ReceiveSumEntity.Field.YearId, schoolRid.YearId)
                    .And(ReceiveSumEntity.Field.TermId, schoolRid.TermId)
                    .And(ReceiveSumEntity.Field.DepId, schoolRid.DepId)
                    .And(ReceiveSumEntity.Field.ReceiveId, schoolRid.ReceiveId)
                    .And(ReceiveSumEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(ReceiveSumEntity.Field.SumId, OrderByEnum.Asc);

                Result result = factory.SelectAll<ReceiveSumEntity>(where, orderbys, out receiveSums);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取合計項目設定資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region templetContent
            byte[] templetContent = null;
            {
                #region [MDY:202203XX] 2022擴充案 中英文模板
                #region [OLD]
                //string sql = String.Format(@"SELECT [{0}] FROM [{1}] WHERE  [{2}] = @BillFormType AND [{3}] = @BillFormId"
                //    , BillFormEntity.Field.BillFormImage
                //    , BillFormEntity.TABLE_NAME
                //    , BillFormEntity.Field.BillFormType
                //    , BillFormEntity.Field.BillFormId);      //10
                //KeyValueList parameters = new KeyValueList(2);
                //if (pdfType == GenPDFHelper.PDFType.Bill)
                //{
                //    parameters.Add("@BillFormType", BillFormTypeCodeTexts.BILLING);
                //    parameters.Add("@BillFormId", schoolRid.BillformId);
                //}
                //else
                //{
                //    parameters.Add("@BillFormType", BillFormTypeCodeTexts.RECEIPT);
                //    parameters.Add("@BillFormId", schoolRid.InvoiceformId);
                //}
                #endregion

                bool useEngDataUI = isEngUI && school.IsEngEnabled();

                string sql = $@"
SELECT [{BillFormEntity.Field.BillFormImage}]
  FROM [{BillFormEntity.TABLE_NAME}]
 WHERE [{BillFormEntity.Field.BillFormType}] = @BillFormType AND[{BillFormEntity.Field.BillFormId}] = @BillFormId";

                KeyValueList parameters = new KeyValueList(2);
                if (pdfType == GenPDFHelper.PDFType.Bill)
                {
                    parameters.Add("@BillFormType", BillFormTypeCodeTexts.BILLING);
                    if (useEngDataUI)
                    {
                        parameters.Add("@BillFormId", schoolRid.BillFormEId);
                    }
                    else
                    {
                        parameters.Add("@BillFormId", schoolRid.BillformId);
                    }
                }
                else
                {
                    parameters.Add("@BillFormType", BillFormTypeCodeTexts.RECEIPT);
                    if (useEngDataUI)
                    {
                        parameters.Add("@BillFormId", schoolRid.InvoiceFormEId);
                    }
                    else
                    {
                        parameters.Add("@BillFormId", schoolRid.InvoiceformId);
                    }
                }
                #endregion

                object value = null;
                Result result = factory.ExecuteScalar(sql, parameters, out value);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取繳費單模板資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                templetContent = value as byte[];
                if (templetContent == null || templetContent.Length == 0)
                {
                    errmsg = "查無繳費單模板資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region StudentMasterEntity
            StudentMasterEntity[] students = null;
            {
                Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, receiveType)
                    .And(StudentMasterEntity.Field.DepId, depId);
                Result result = factory.SelectAll<StudentMasterEntity>(where, null, out students);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取學生資料失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (students == null || students.Length == 0)
                {
                    errmsg = "查無學生資料";
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
            }
            #endregion

            #region [MDY:20171127] 取財金 QRCode 支付參數設定 (20170831_01)
            FiscQRCodeConfig qrcodeConfig = null;
            {
                ConfigEntity configData = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, FiscQRCodeConfig.ConfigKey);
                Result result = factory.SelectFirst<ConfigEntity>(where, null, out configData);
                if (!result.IsSuccess)
                {
                    errmsg = "讀取財金 QRCode 支付參數失敗，錯誤訊息：" + result.Message;
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                //無資料或參數設定錯誤 (無法解析) 都不要中斷處理，避免使用無 QRCode 的模板也無法產生 PDF
                if (configData != null)
                {
                    qrcodeConfig = FiscQRCodeConfig.Parse(configData.ConfigValue);
                }
            }
            #endregion

            int maxCount = 2000;
            if (totalCount <= maxCount)
            {
                DateTime startTime;
                DateTime endTime;
                string pdfFile = null;

                #region [MDY:20171127] 財金 QRCode 支付 (20170831_01)
                #region [Old]
                //Result result = this.GenPDFFiles(school, schoolRid, receiveChannels, studentReceives, students, bank, receiveSums, fgMark
                //    , pdfType, templetContent, outPath, userId, out startTime, out endTime, out pdfFile);
                //if (result.IsSuccess)
                //{
                //    outPDFFiles = new string[] { pdfFile };
                //}
                #endregion

                #region [MDY:202203XX] 2022擴充案 是否英文介面
                Result result = this.GenPDFFiles(school, schoolRid, receiveChannels, studentReceives, students
                    , bank, receiveSums, qrcodeConfig, fgMark, isEngUI
                    , pdfType, templetContent, outPath, userId, out startTime, out endTime, out pdfFile);
                #endregion

                if (result.IsSuccess)
                {
                    outPDFFiles = new string[] { pdfFile };
                }
                #endregion
                return result;
            }
            else
            {
                List<string> pdfFiles = new List<string>();
                for (int idx = 0; idx < studentReceives.Length; idx += maxCount)
                {
                    int count = studentReceives.Length - idx;
                    count = count > maxCount ? maxCount : count;
                    StudentReceiveView[] datas = new StudentReceiveView[count];
                    Array.Copy(studentReceives, idx, datas, 0, count);
                    DateTime startTime;
                    DateTime endTime;
                    string pdfFile = null;

                    #region [MDY:20171127] 財金 QRCode 支付 (20170831_01)
                    #region [Old]
                    //Result result = this.GenPDFFiles(school, schoolRid, receiveChannels, datas, students, bank, receiveSums, fgMark
                    //    , pdfType, templetContent, outPath, userId, out startTime, out endTime, out pdfFile);
                    #endregion

                    #region [MDY:202203XX] 2022擴充案 是否英文介面
                    Result result = this.GenPDFFiles(school, schoolRid, receiveChannels, datas, students
                        , bank, receiveSums, qrcodeConfig, fgMark, isEngUI
                        , pdfType, templetContent, outPath, userId, out startTime, out endTime, out pdfFile);
                    #endregion

                    #endregion

                    if (result.IsSuccess)
                    {
                        pdfFiles.Add(pdfFile);
                    }
                    else
                    {
                        return result;
                    }
                }
                outPDFFiles = pdfFiles.ToArray();
                return new Result(true);
            }
        }

        private Result GenBillPDFXml(string xmlFileName, SchoolRTypeEntity school, SchoolRidEntity schoolRid
            , ReceiveChannelEntity[] receiveChannels, StudentReceiveView[] receives, StudentMasterEntity[] students
            , BankEntity bank, ReceiveSumEntity[] receiveSums, FiscQRCodeConfig qrcodeConfig, bool fgMark, bool isEngUI
            , string userid)
        {
            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            bool useEngDataUI = isEngUI && school.IsEngEnabled();
            #endregion

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = false;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = true;

            XmlHelper xmlHelper = new XmlHelper();

            #region [MDY:20160521] 如果資料超過300筆使用精簡 Xml 產生
            xmlHelper.IsSimplify = (receives != null && receives.Length > 300);
            #endregion

            ChannelHelper channelHelper = new ChannelHelper();
            using (FileStream fs = File.Open(xmlFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (XmlWriter writer = XmlWriter.Create(fs, settings))
                {
                    writer.WriteStartElement("invoices");

                    int pageNo = 1;
                    foreach(StudentReceiveView receive in receives)
                    {
                        StudentMasterEntity student = null;
                        foreach(StudentMasterEntity one in students)
                        {
                            #region [MDY:20170227] 學號中的英文可能大小寫會不一致，所以比對時忽略大小寫
                            #region [Old]
                            //if (one.ReceiveType == receive.ReceiveType
                            //    && one.DepId == receive.DepId
                            //    && one.Id == receive.StuId)
                            //{
                            //    student = one;
                            //    break;
                            //}
                            #endregion

                            if (one.ReceiveType == receive.ReceiveType
                                && one.DepId == receive.DepId
                                && one.Id.Equals(receive.StuId, StringComparison.CurrentCultureIgnoreCase))
                            {
                                student = one;
                                break;
                            }
                            #endregion
                        }
                        if (student == null)
                        {
                            return new Result(false, String.Format("查無學號 {0} 的學生資料", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        }

                        if (receive.ReceiveAmount == null)
                        {
                            return new Result(false, String.Format("學號 {0} 的繳費資料未計算金額", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        }

                        #region [Old] 土銀特別允許金額小於或等於 0 的資料 (不會產生虛擬帳號) 也可以列印
                        //if (String.IsNullOrWhiteSpace(receive.CancelNo))
                        //{
                        //    return new Result(false, String.Format("學號 {0} 的繳費資料未產生虛擬帳號", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        //}
                        #endregion

                        #region [New] 土銀特別允許金額小於或等於 0 的資料 (不會產生虛擬帳號) 也可以列印
                        if (receive.ReceiveAmount > 0 && String.IsNullOrEmpty(receive.CancelNo))
                        {
                            return new Result(false, String.Format("學號 {0} 的繳費資料未產生虛擬帳號", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        #endregion

                        #region 土銀的邏輯是手續費內含就是企業負擔，外加就是繳款人負擔，所以各管道的金額都一樣，不用算金額只要判斷適用的超商與臨櫃管道
                        #region [Old]
                        //decimal smAmount = 0M;
                        //ReceiveChannelEntity smChannel = null;
                        //decimal poAmount = 0M;
                        //ReceiveChannelEntity poChannel = null;
                        //decimal cashAmount = 0M;
                        //ReceiveChannelEntity cashChannel = null;
                        //if (!channelHelper.GetChannelFee(receive.ReceiveAmount.Value, receiveChannels
                        //    , out smAmount, out smChannel, out poAmount, out poChannel, out cashAmount, out cashChannel))
                        //{
                        //    return new Result(false, String.Format("學號 {0} 的繳費資料無法取各管道的手續費資料", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        //}
                        #endregion

                        ReceiveChannelEntity smChannel = null;
                        ReceiveChannelEntity cashChannel = null;
                        channelHelper.CheckReceiveChannel(receive.ReceiveAmount.Value, receiveChannels, out smChannel, out cashChannel);
                        if (smChannel == null || cashChannel == null)
                        {
                            //沒有管道不算錯誤
                        }
                        #endregion

                        string bankName = bank == null ? String.Empty : bank.BankFName;
                        string bankTel = bank == null ? String.Empty : bank.Tel;

                        #region [Old] 土銀不使用原有的部別 DepListEntity，改用專用的部別 DeptListEntity
                        //if (!xmlHelper.GenXmlData(writer, pageNo.ToString(), userid, schoolRid.DepName, receive.MajorName, receive.StuGradeName, receive.CollegeName, receive.ClassName
                        //    , receive.ReduceName, receive.DormName, schoolRid.YearName, schoolRid.TermName, schoolRid.ReceiveName, "", bankName, bankTel, schoolRid, student, school, receive
                        //    , smChannel, cashChannel))
                        //{
                        //    return new Result(false, String.Format("處理學號 {0} 的繳費單資料失敗", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        //}
                        #endregion

                        #region [MDY:20171127] 財金 QRCode 支付 (20170831_01)
                        #region [Old]
                        //if (!xmlHelper.GenXmlData(writer, pageNo.ToString(), userid
                        //    , receive.DeptName, receive.MajorName, receive.StuGradeName, receive.CollegeName
                        //    , receive.ClassName, receive.ReduceName, receive.DormName, receive.LoanName
                        //    , schoolRid.YearName, schoolRid.TermName, schoolRid.ReceiveName
                        //    , "", bankName, bankTel
                        //    , receive.IdentifyName01, receive.IdentifyName02, receive.IdentifyName03, receive.IdentifyName04, receive.IdentifyName05, receive.IdentifyName06
                        //    , schoolRid, student, school, receive
                        //    , smChannel, cashChannel, receiveSums, fgMark))
                        //{
                        //    return new Result(false, String.Format("處理學號 {0} 的繳費單資料失敗", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        //}
                        #endregion

                        #region [MDY:202203XX] 2022擴充案 英文介面、班級英文名稱
                        string stuGradeName = GradeCodeTexts.GetText(receive.StuGrade, useEngDataUI);
                        if (!xmlHelper.GenXmlData(writer, pageNo.ToString(), userid
                            , receive.DeptName, receive.MajorName, stuGradeName, receive.CollegeName
                            , receive.ClassName, receive.ReduceName, receive.DormName, receive.LoanName
                            , receive.YearName, receive.TermName, receive.ReceiveName
                            , "", bankName, bankTel
                            , receive.IdentifyName01, receive.IdentifyName02, receive.IdentifyName03, receive.IdentifyName04, receive.IdentifyName05, receive.IdentifyName06
                            , schoolRid, student, school, receive
                            , smChannel, cashChannel, receiveSums, qrcodeConfig, fgMark, isEngUI))
                        {
                            return new Result(false, String.Format("處理學號 {0} 的繳費單資料失敗", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        #endregion
                        #endregion

                        pageNo++;
                    }

                    writer.WriteEndElement();
                }
            }
            return new Result(true);
        }

        private Result GenReceiptPDFXml(string xmlFileName, SchoolRTypeEntity school, SchoolRidEntity schoolRid
            , ReceiveChannelEntity[] receiveChannels, StudentReceiveView[] receives, StudentMasterEntity[] students
            , bool fgMark, bool isEngUI, string userid)
        {
            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            bool useEngDataUI = isEngUI && school.IsEngEnabled();
            #endregion

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = false;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = true;

            XmlHelper xmlHelper = new XmlHelper();

            #region [MDY:20160521] 如果資料超過300筆使用精簡 Xml 產生
            xmlHelper.IsSimplify = (receives != null && receives.Length > 300);
            #endregion

            ChannelHelper channelHelper = new ChannelHelper();
            using (FileStream fs = File.Open(xmlFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (XmlWriter writer = XmlWriter.Create(fs, settings))
                {
                    writer.WriteStartElement("invoices");

                    int pageNo = 1;
                    foreach (StudentReceiveView receive in receives)
                    {
                        StudentMasterEntity student = null;
                        foreach (StudentMasterEntity one in students)
                        {
                            #region [MDY:20170227] 學號中的英文可能大小寫會不一致，所以比對時忽略大小寫
                            #region [Old]
                            //if (one.ReceiveType == receive.ReceiveType
                            //    && one.DepId == receive.DepId
                            //    && one.Id == receive.StuId)
                            //{
                            //    student = one;
                            //    break;
                            //}
                            #endregion

                            if (one.ReceiveType == receive.ReceiveType
                                && one.DepId == receive.DepId
                                && one.Id.Equals(receive.StuId, StringComparison.CurrentCultureIgnoreCase))
                            {
                                student = one;
                                break;
                            }
                            #endregion
                        }
                        if (student == null)
                        {
                            return new Result(false, String.Format("查無學號 {0} 的學生資料", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        if (receive.ReceiveAmount == null)
                        {
                            return new Result(false, String.Format("學號 {0} 的繳費資料未計算金額", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        if (String.IsNullOrWhiteSpace(receive.CancelNo))
                        {
                            return new Result(false, String.Format("學號 {0} 的繳費資料未產生虛擬帳號", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        }

                        #region 土銀的邏輯是手續費內含就是企業負擔，外加就是繳款人負擔，所以各管道的金額都一樣，不用算金額只要判斷適用的超商與臨櫃管道
                        #region [Old]
                        //decimal smAmount = 0M;
                        //ReceiveChannelEntity smChannel = null;
                        //decimal poAmount = 0M;
                        //ReceiveChannelEntity poChannel = null;
                        //decimal cashAmount = 0M;
                        //ReceiveChannelEntity cashChannel = null;
                        //if (!channelHelper.GetChannelFee(receive.ReceiveAmount.Value, receiveChannels
                        //    , out smAmount, out smChannel, out poAmount, out poChannel, out cashAmount, out cashChannel))
                        //{
                        //    return new Result(false, String.Format("學號 {0} 的繳費資料無法取各管道的手續費資料", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        //}
                        #endregion

                        ReceiveChannelEntity smChannel = null;
                        ReceiveChannelEntity cashChannel = null;
                        channelHelper.CheckReceiveChannel(receive.ReceiveAmount.Value, receiveChannels, out smChannel, out cashChannel);
                        if (smChannel == null || cashChannel == null)
                        {
                            //沒有管道不算錯誤
                        }
                        #endregion

                        #region [Old] 土銀不使用原有的部別 DepListEntity，改用專用的部別 DeptListEntity
                        //if (!xmlHelper.GenReceiptXmlData(writer, pageNo, userid, schoolRid.DepName, receive.MajorName, receive.StuGradeName, receive.CollegeName, receive.ClassName
                        //    , receive.ReduceName, receive.DormName, schoolRid.YearName, schoolRid.TermName, schoolRid.ReceiveName, "", schoolRid, student, school, receive))
                        //{
                        //    return new Result(false, String.Format("處理學號 {0} 的收據資料失敗", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        //}
                        #endregion

                        #region [MDY:202203XX] 2022擴充案 英文介面、班級英文名稱
                        string stuGradeName = GradeCodeTexts.GetText(receive.StuGrade, useEngDataUI);
                        if (!xmlHelper.GenReceiptXmlData(writer, pageNo, userid
                            , receive.DeptName, receive.MajorName, stuGradeName, receive.CollegeName
                            , receive.ClassName, receive.ReduceName, receive.DormName, receive.LoanName
                            , receive.YearName, receive.TermName, receive.ReceiveName
                            , "", receive.IdentifyName01, receive.IdentifyName02, receive.IdentifyName03, receive.IdentifyName04, receive.IdentifyName05, receive.IdentifyName06
                            , schoolRid, student, school, receive, fgMark, isEngUI))
                        {
                            return new Result(false, String.Format("處理學號 {0} 的收據資料失敗", receive.StuId), ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        #endregion

                        pageNo++;
                    }

                    writer.WriteEndElement();
                }
            }
            return new Result(true);
        }
        #endregion

        private Result CallPDFService(string[] xmlFileFullNames, string[] templetFileFullNames, string[] pdfFileFullNames, bool fgOverWrite
            , out DateTime startTime, out DateTime endTime, out string[] pdfFiles)
        {
            startTime = DateTime.Now;
            endTime = startTime;
            pdfFiles = null;

            #region Check
            if (xmlFileFullNames == null || templetFileFullNames == null || pdfFileFullNames == null
                || xmlFileFullNames.Length == 0
                || xmlFileFullNames.Length != templetFileFullNames.Length
                || xmlFileFullNames.Length != pdfFileFullNames.Length)
            {
                return new Result(false, "缺少參數或參數不合法", CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            string errmsg = String.Empty;
            List<string> okPDFs = new List<string>(xmlFileFullNames.Length);

            #region 逐檔處理
            startTime = DateTime.Now;
            try
            {
                for (int idx = 0; idx < xmlFileFullNames.Length; idx++)
                {
                    string xmlFileFullName = xmlFileFullNames[idx];
                    string templetFileFullName = templetFileFullNames[idx];
                    string pdfFileFullName = pdfFileFullNames[idx];
                    xmlFileFullName = xmlFileFullName == null ? String.Empty : xmlFileFullName.Trim();
                    templetFileFullName = templetFileFullName == null ? String.Empty : templetFileFullName.Trim();
                    pdfFileFullName = pdfFileFullName == null ? String.Empty : pdfFileFullName.Trim();

                    #region 檢查檔案
                    if (xmlFileFullName.Length == 0 || !Path.IsPathRooted(xmlFileFullName))
                    {
                        errmsg = "未指定資料檔的檔名與路徑";
                        break;
                    }
                    else if (!File.Exists(xmlFileFullName))
                    {
                        errmsg = String.Format("資料檔 {0} 不存在", xmlFileFullName);
                        break;
                    }
                    else if (templetFileFullName.Length == 0 || !Path.IsPathRooted(templetFileFullName))
                    {
                        errmsg = "未指定模板檔的檔名與路徑";
                        break;
                    }
                    else if (!File.Exists(templetFileFullName))
                    {
                        errmsg = String.Format("模板檔 {0} 不存在", xmlFileFullName);
                        break;
                    }
                    else if (pdfFileFullName.Length == 0 || !Path.IsPathRooted(pdfFileFullName))
                    {
                        errmsg = "未指定 PDF 的檔名與路徑";
                        break;
                    }
                    else
                    {
                        string fileName = Path.GetFileName(pdfFileFullName);
                        string path = pdfFileFullName.Replace(fileName, String.Empty);
                        try
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                        }
                        catch (Exception exp)
                        {
                            errmsg = String.Format("建立 PDF 目錄失敗：{0}", exp.Message);
                            break;
                        }

                        if (errmsg.Length == 0 && File.Exists(pdfFileFullName))
                        {
                            if (fgOverWrite)
                            {
                                try
                                {
                                    File.Delete(pdfFileFullName);
                                }
                                catch (Exception exp)
                                {
                                    errmsg = String.Format("刪除已存在的 PDF 檔失敗：{0}", exp.Message);
                                    break;
                                }
                            }
                            else
                            {
                                errmsg = "PDF 檔已存在";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region 執行 Job 服務
                    if (errmsg.Length == 0)
                    {
                        string jobID = String.Empty;
                        errmsg = this.InsertPDFJob(xmlFileFullName, templetFileFullName, pdfFileFullName, out jobID);
                        if (errmsg.Length == 0 && jobID.Length > 0)
                        {
                            #region 查詢 Job
                            bool doneFlag = false;
                            int requeryTimes = 0;
                            while (!doneFlag
                                && errmsg.Length == 0
                                && requeryTimes <= _MaxReQueryTimes)
                            {
                                requeryTimes++;
                                string jobStatus = String.Empty;
                                string jobMsg = String.Empty;
                                errmsg = this.QueryJob(jobID, out jobStatus, out jobMsg);
                                if (errmsg.Length == 0)
                                {
                                    switch (jobStatus)
                                    {
                                        case "PROCESSING":
                                        case "QUEUED":
                                            Thread.Sleep(_ReQuerySecond * 1000);
                                            break;
                                        case "DONE":
                                            okPDFs.Add(pdfFileFullName);
                                            doneFlag = true;
                                            break;
                                        case "ERROR":
                                            errmsg = jobMsg == null || jobMsg.Length == 0 ? "未知的 job 錯誤" : jobMsg;
                                            break;
                                        default:
                                            errmsg = String.Format("未知的 job 處理狀態 ({0})", jobStatus);
                                            break;
                                    }
                                }
                            }
                            if (errmsg.Length == 0 && (!doneFlag || requeryTimes > _MaxReQueryTimes))
                            {
                                errmsg = "單次產生 PDF 執行 Timeout";
                            }
                            #endregion
                        }
                        if (errmsg.Length > 0)
                        {
                            errmsg = String.Format("產生 PDF 失敗：{0}", errmsg);
                            break;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception exp)
            {
                errmsg = String.Concat("執行 PDF Job 服務失敗：", exp.Message);
            }
            endTime = DateTime.Now;
            #endregion

            pdfFiles = okPDFs.ToArray();
            if (errmsg.Length > 0)
            {
                return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
            }
            return new Result(true);
        }

        #region Call PDF Service
        private string _ServiceIP = "127.0.0.1";
        private int _ServicePort = 8888;
        private int _ReQuerySecond = 2;
        private int _MaxReQueryTimes = 60 * 3;

        /// <summary>
        /// 呼叫 PDF 工作服務
        /// </summary>
        /// <param name="command">呼叫服務命令</param>
        /// <param name="jobID"傳回>工作代碼</param>
        /// <param name="jobStatus"></param>
        /// <param name="jobMsg"></param>
        /// <returns></returns>
        private string CallPDFJobService(string command, out string jobID, out string jobStatus, out string jobMsg)
        {
            jobID = String.Empty;
            jobStatus = String.Empty;
            jobMsg = String.Empty;

            string errmsg = String.Empty;
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                #region 發送 request
                byte[] requestData = Encoding.Default.GetBytes(command);
                client = new TcpClient(_ServiceIP, _ServicePort);
                stream = client.GetStream();
                stream.Write(requestData, 0, requestData.Length);
                #endregion

                #region 接收 response
                byte[] responseData = new byte[256];
                int responseDataSize = stream.Read(responseData, 0, responseData.Length);
                string result = Encoding.Default.GetString(responseData, 0, responseDataSize);
                string[] resultValues = result.Split(' ');
                jobID = resultValues[1].Trim();
                jobStatus = resultValues[5].Trim().ToUpper();
                jobMsg = resultValues[6].Trim();
                #endregion
            }
            catch (Exception exp)
            {
                errmsg = exp.Message;
            }
            finally
            {
                #region Close & Dispose
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                    stream = null;
                }
                if (client != null)
                {
                    if (client.Connected)
                    {
                        client.Close();
                    }
                    client = null;
                }
                #endregion
            }
            return errmsg;
        }

        /// <summary>
        /// 新增 PDF 服務工作
        /// </summary>
        /// <param name="xmlFileFullName">資料的路徑檔名</param>
        /// <param name="templetFileFullName">模板的路徑檔名</param>
        /// <param name="pdfFileFullName">產生PDF的路徑檔名</param>
        /// <param name="jobID">傳回工作代碼</param>
        /// <returns></returns>
        private string InsertPDFJob(string xmlFileFullName, string templetFileFullName, string pdfFileFullName, out string jobID)
        {
            jobID = String.Empty;

            string command = string.Format("EXE JobGeneratePDF {0} {1} {2}", templetFileFullName, pdfFileFullName, xmlFileFullName);
            string jobStatus = String.Empty;
            string jobMsg = String.Empty;
            string errmsg = this.CallPDFJobService(command, out jobID, out jobStatus, out jobMsg);
            if (errmsg.Length == 0)
            {
                if (jobStatus == "ERROR")
                {
                    jobID = String.Empty;
                    errmsg = jobMsg;
                }
                else if (jobID.Length == 0)
                {
                    errmsg = "無法取的 jobID";
                }
            }
            return errmsg;
        }

        /// <summary>
        /// 查詢 PDF 服務工作
        /// </summary>
        /// <param name="jobID">要查詢的工作代碼</param>
        /// <param name="jobStatus"></param>
        /// <param name="jobMsg"></param>
        /// <returns></returns>
        private string QueryJob(string jobID, out string jobStatus, out string jobMsg)
        {
            jobStatus = String.Empty;
            jobMsg = String.Empty;

            string command = "QRY " + jobID;
            string jobid = String.Empty;
            string errmsg = this.CallPDFJobService(command, out jobid, out jobStatus, out jobMsg);
            //if (errmsg.Length == 0)
            //{
            //    if (jobStatus == "ERROR")
            //    {
            //        errmsg = jobMsg;
            //    }
            //}
            return errmsg;
        }
        #endregion

        #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 產生測試繳款單
        /// <summary>
        /// 產生測試繳款單
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="receiveType"></param>
        /// <param name="channelId"></param>
        /// <param name="barcodeId"></param>
        /// <param name="dataCount"></param>
        /// <param name="billFormId"></param>
        /// <param name="payDueDate"></param>
        /// <param name="outPath"></param>
        /// <param name="outPDFContent"></param>
        /// <returns></returns>
        public Result GenSMBarcodePDF(EntityFactory factory, string receiveType, string channelId, string barcodeId, Int32 dataCount, int billFormId, DateTime payDueDate
            , string outPath, out byte[] outPDFContent)
        {
            outPDFContent = null;

            string tmpBaseName = String.Format("{0}_{1:yyyyMMddHHmmssfff}", receiveType, DateTime.Now);
            string templetFileFullName = Path.Combine(outPath, tmpBaseName + ".Templet.PDF");
            string xmlFileFullName = Path.Combine(outPath, tmpBaseName + ".XML");
            string outPDFFile = Path.Combine(outPath, tmpBaseName + ".PDF");

            #region 取得商家代號資料
            //SchoolRTypeEntity schoolData = null;
            //{
            //    Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
            //    Result result = factory.SelectFirst<SchoolRTypeEntity>(where, null, out schoolData);
            //    if (result.IsSuccess && schoolData == null)
            //    {
            //        result = new Result(false, "查無商家代號資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
            //    }
            //    if (!result.IsSuccess)
            //    {
            //        return result;
            //    }
            //}
            #endregion

            #region 取得代收管道手續費級距資料
            ChannelWayEntity channelData = null;
            {
                Expression where = new Expression(ChannelWayEntity.Field.ChannelId, channelId)
                    .And(ChannelWayEntity.Field.BarcodeId, barcodeId);
                Result result = factory.SelectFirst<ChannelWayEntity>(where, null, out channelData);
                if (result.IsSuccess && channelData == null)
                {
                    result = new Result(false, "查無代收管道手續費級距資料", CoreStatusCode.S_SELECT_DATA_FAILURE, null);
                }
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            #endregion

            #region 取得模板檔
            byte[] templetContent = null;
            {
                string sql = String.Format(@"
SELECT TOP 1 [{0}] 
FROM ( 
SELECT TOP 1 [{0}], 1 AS ORDER_NO FROM [{1}] WHERE [{2}] = @BillFormType AND [{3}] = @BillFormEdition AND [{4}] = @BillFormName ORDER BY [{5}] 
UNION 
SELECT TOP 1 [{0}], 2 AS ORDER_NO FROM [{1}] WHERE [{2}] = @BillFormType AND [{3}] = @BillFormEdition AND [{5}] = @BillFormId ORDER BY [{5}] 
UNION 
SELECT TOP 1 [{0}], 3 AS ORDER_NO FROM [{1}] WHERE [{2}] = @BillFormType AND [{3}] = @BillFormEdition ORDER BY [{5}] 
) V 
ORDER BY ORDER_NO"
                    , BillFormEntity.Field.BillFormImage
                    , BillFormEntity.TABLE_NAME
                    , BillFormEntity.Field.BillFormType
                    , BillFormEntity.Field.BillFormEdition
                    , BillFormEntity.Field.BillFormName
                    , BillFormEntity.Field.BillFormId).Trim();
                KeyValueList parameters = new KeyValueList(3);
                parameters.Add("@BillFormType", BillFormTypeCodeTexts.BILLING);
                parameters.Add("@BillFormEdition", BillFormEditionCodeTexts.PUBLIC);
                parameters.Add("@BillFormName", "產生測試繳款單用");
                parameters.Add("@BillFormId", billFormId);

                object value = null;
                Result result = factory.ExecuteScalar(sql, parameters, out value);
                if (result.IsSuccess)
                {
                    templetContent = value as byte[];
                    if (templetContent == null || templetContent.Length == 0)
                    {
                        result = new Result(false, "查無繳費單模板資料", CoreStatusCode.S_SELECT_DATA_FAILURE, result.Exception);
                    }
                }
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            #endregion

            #region 產生 XmlFile
            {
                decimal minAmount = channelData.MinMoney;
                decimal maxAmount = channelData.MaxMoney > channelData.MinMoney ? channelData.MaxMoney : minAmount + 20000;
                decimal intervalAmount = maxAmount - minAmount;
                if (dataCount > 1)
                {
                    intervalAmount = Decimal.Truncate(intervalAmount / dataCount);
                }

                DateTime printDate = DateTime.Now;

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.CheckCharacters = false;
                settings.Encoding = Encoding.UTF8;
                settings.Indent = false;
                settings.NewLineOnAttributes = false;
                settings.OmitXmlDeclaration = true;

                XmlHelper xmlHelper = new XmlHelper();
                xmlHelper.IsSimplify = true;

                using (FileStream fs = File.Open(xmlFileFullName, FileMode.Create, FileAccess.Write, FileShare.None))
                using (XmlWriter xmlWriter = XmlWriter.Create(fs, settings))
                {
                    xmlWriter.WriteStartElement("invoices");

                    decimal receiveAmount = minAmount;
                    for (int pageNo = 1; pageNo <= dataCount; pageNo++)
                    {
                        string cancelNo = String.Format("{0}{1:0000000000}", receiveType, pageNo);
                        if (pageNo == dataCount)
                        {
                            receiveAmount = maxAmount;
                        }

                        xmlHelper.GenSMBarcodePDFXmlData(xmlWriter, printDate, pageNo, cancelNo, receiveAmount, payDueDate, channelData);

                        receiveAmount += intervalAmount;
                    }
                }
            }
            #endregion

            #region 產生 PDF
            {
                Result result = null;
                try
                {
                    #region 模板
                    if (File.Exists(templetFileFullName))
                    {
                        File.Delete(templetFileFullName);
                    }
                    File.WriteAllBytes(templetFileFullName, templetContent);
                    #endregion

                    #region PDF 檔
                    if (File.Exists(outPDFFile))
                    {
                        File.Delete(outPDFFile);
                    }

                    string[] xmlFileFullNames = new string[] { xmlFileFullName };
                    string[] templetFileFullNames = new string[] { templetFileFullName };
                    string[] pdfFileFullNames = new string[] { outPDFFile };
                    string[] outPdfFiles = null;
                    DateTime startTime, endTime;
                    result = this.CallPDFService(xmlFileFullNames, templetFileFullNames, pdfFileFullNames, true, out startTime, out endTime, out outPdfFiles);
                    if (result.IsSuccess)
                    {
                        outPDFContent = System.IO.File.ReadAllBytes(outPDFFile);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    result = new Result(false, "處理檔案失敗：" + ex.Message, CoreStatusCode.UNKNOWN_EXCEPTION, ex);
                }
                return result;
            }
            #endregion
        }
        #endregion
    }
}
