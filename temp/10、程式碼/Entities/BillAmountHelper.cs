using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
namespace Entities
{
    public sealed class BillAmountHelper
    {
        #region private property
        private string _err_msg="";
        public string err_mgs
        {
            get { return _err_msg.Trim(); }
        }
        #endregion

        #region enum
        public enum CalculateType:int
        {
            byAmount=1,//依金額
            byStandard=2//依標準
        }

        public enum RoundType : int
        {
            RoundUp,//無條件進位
            RoundDown,//無條件捨去
            RoundOff//四捨五入
        }
        #endregion

        #region private method

        /// <summary>
        /// 讀取費用別設定
        /// </summary>
        /// <param name="receive_type">代收類別</param>
        /// <param name="year_id">學年</param>
        /// <param name="term_id">學期</param>
        /// <param name="dep_id">部別</param>
        /// <param name="receive_id">費用別</param>
        /// <param name="school_rid">回傳費用別設定</param>
        /// <returns>成功或失敗</returns>
        private bool getSchoolRid(string receive_type,string year_id,string term_id,string dep_id,string receive_id,out SchoolRidEntity school_rid)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4}", receive_type, year_id, term_id, dep_id, receive_id);

            school_rid = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(SchoolRidEntity.Field.ReceiveType, receive_type);
            where.And(SchoolRidEntity.Field.YearId, year_id);
            where.And(SchoolRidEntity.Field.TermId, term_id);
            where.And(SchoolRidEntity.Field.DepId, dep_id);
            where.And(SchoolRidEntity.Field.ReceiveId, receive_id);
            Result result = factroy.SelectFirst<SchoolRidEntity>(where, orderbys, out school_rid);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getSchoolRid] 讀取School_RID發生錯誤，錯誤訊息={0}，key={1}",result.Message,key);
            }
            else
            {
                if (school_rid == null)
                {
                    _err_msg = string.Format("[getSchoolRid] 讀取School_RID發生錯誤，錯誤訊息={0}，key={1}", "找不到費用別設定",key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        /// <summary>
        /// 取得費用別共有幾個收入科目
        /// </summary>
        /// <param name="school_rid">費用別設定檔</param>
        /// <returns>收入科目數</returns>
        private Int32 countReceiveItems(SchoolRidEntity school_rid)
        {
            _err_msg = "";
            //bool rc = false;
            //string log = "";
            StringBuilder logs = new StringBuilder();
            int count = 0;
            if (school_rid == null)
            {
                return count;
            }

            if ((school_rid.ReceiveItem01 == null ? "" : school_rid.ReceiveItem01.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem02 == null ? "" : school_rid.ReceiveItem02.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem03 == null ? "" : school_rid.ReceiveItem03.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem04 == null ? "" : school_rid.ReceiveItem04.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem05 == null ? "" : school_rid.ReceiveItem05.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem06 == null ? "" : school_rid.ReceiveItem06.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem07 == null ? "" : school_rid.ReceiveItem07.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem08 == null ? "" : school_rid.ReceiveItem08.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem09 == null ? "" : school_rid.ReceiveItem09.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem10 == null ? "" : school_rid.ReceiveItem10.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem11 == null ? "" : school_rid.ReceiveItem11.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem12 == null ? "" : school_rid.ReceiveItem12.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem13 == null ? "" : school_rid.ReceiveItem13.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem14 == null ? "" : school_rid.ReceiveItem14.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem15 == null ? "" : school_rid.ReceiveItem15.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem16 == null ? "" : school_rid.ReceiveItem16.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem17 == null ? "" : school_rid.ReceiveItem17.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem18 == null ? "" : school_rid.ReceiveItem18.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem19 == null ? "" : school_rid.ReceiveItem19.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem20 == null ? "" : school_rid.ReceiveItem20.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem21 == null ? "" : school_rid.ReceiveItem21.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem22 == null ? "" : school_rid.ReceiveItem22.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem23 == null ? "" : school_rid.ReceiveItem23.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem24 == null ? "" : school_rid.ReceiveItem24.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem25 == null ? "" : school_rid.ReceiveItem25.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem26 == null ? "" : school_rid.ReceiveItem26.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem27 == null ? "" : school_rid.ReceiveItem27.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem28 == null ? "" : school_rid.ReceiveItem28.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem29 == null ? "" : school_rid.ReceiveItem29.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem30 == null ? "" : school_rid.ReceiveItem30.Trim()) != "")
            {
                count++;
            }

            if ((school_rid.ReceiveItem31 == null ? "" : school_rid.ReceiveItem31.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem32 == null ? "" : school_rid.ReceiveItem32.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem33 == null ? "" : school_rid.ReceiveItem33.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem34 == null ? "" : school_rid.ReceiveItem34.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem35 == null ? "" : school_rid.ReceiveItem35.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem36 == null ? "" : school_rid.ReceiveItem36.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem37 == null ? "" : school_rid.ReceiveItem37.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem38 == null ? "" : school_rid.ReceiveItem38.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem39 == null ? "" : school_rid.ReceiveItem39.Trim()) != "")
            {
                count++;
            }
            if ((school_rid.ReceiveItem40 == null ? "" : school_rid.ReceiveItem40.Trim()) != "")
            {
                count++;
            }
            return count;
        }

        /// <summary>
        /// 計算總金額
        /// </summary>
        /// <param name="student_receive">要計算的學生繳費資料</param>
        /// <returns>每個收入科目的加總金額</returns>
        private decimal sumReceiveItemAmount(StudentReceiveEntity student_receive)
        {
            decimal total_amount = 0;

            total_amount += (decimal)(student_receive.Receive01 == null ? 0 : student_receive.Receive01);
            total_amount += (decimal)(student_receive.Receive02 == null ? 0 : student_receive.Receive02);
            total_amount += (decimal)(student_receive.Receive03 == null ? 0 : student_receive.Receive03);
            total_amount += (decimal)(student_receive.Receive04 == null ? 0 : student_receive.Receive04);
            total_amount += (decimal)(student_receive.Receive05 == null ? 0 : student_receive.Receive05);
            total_amount += (decimal)(student_receive.Receive06 == null ? 0 : student_receive.Receive06);
            total_amount += (decimal)(student_receive.Receive07 == null ? 0 : student_receive.Receive07);
            total_amount += (decimal)(student_receive.Receive08 == null ? 0 : student_receive.Receive08);
            total_amount += (decimal)(student_receive.Receive09 == null ? 0 : student_receive.Receive09);
            total_amount += (decimal)(student_receive.Receive10 == null ? 0 : student_receive.Receive10);
            total_amount += (decimal)(student_receive.Receive11 == null ? 0 : student_receive.Receive11);
            total_amount += (decimal)(student_receive.Receive12 == null ? 0 : student_receive.Receive12);
            total_amount += (decimal)(student_receive.Receive13 == null ? 0 : student_receive.Receive13);
            total_amount += (decimal)(student_receive.Receive14 == null ? 0 : student_receive.Receive14);
            total_amount += (decimal)(student_receive.Receive15 == null ? 0 : student_receive.Receive15);
            total_amount += (decimal)(student_receive.Receive16 == null ? 0 : student_receive.Receive16);
            total_amount += (decimal)(student_receive.Receive17 == null ? 0 : student_receive.Receive17);
            total_amount += (decimal)(student_receive.Receive18 == null ? 0 : student_receive.Receive18);
            total_amount += (decimal)(student_receive.Receive19 == null ? 0 : student_receive.Receive19);
            total_amount += (decimal)(student_receive.Receive20 == null ? 0 : student_receive.Receive20);
            total_amount += (decimal)(student_receive.Receive21 == null ? 0 : student_receive.Receive21);
            total_amount += (decimal)(student_receive.Receive22 == null ? 0 : student_receive.Receive22);
            total_amount += (decimal)(student_receive.Receive23 == null ? 0 : student_receive.Receive23);
            total_amount += (decimal)(student_receive.Receive24 == null ? 0 : student_receive.Receive24);
            total_amount += (decimal)(student_receive.Receive25 == null ? 0 : student_receive.Receive25);
            total_amount += (decimal)(student_receive.Receive26 == null ? 0 : student_receive.Receive26);
            total_amount += (decimal)(student_receive.Receive27 == null ? 0 : student_receive.Receive27);
            total_amount += (decimal)(student_receive.Receive28 == null ? 0 : student_receive.Receive28);
            total_amount += (decimal)(student_receive.Receive29 == null ? 0 : student_receive.Receive29);
            total_amount += (decimal)(student_receive.Receive30 == null ? 0 : student_receive.Receive30);

            total_amount += (decimal)(student_receive.Receive31 == null ? 0 : student_receive.Receive31);
            total_amount += (decimal)(student_receive.Receive32 == null ? 0 : student_receive.Receive32);
            total_amount += (decimal)(student_receive.Receive33 == null ? 0 : student_receive.Receive33);
            total_amount += (decimal)(student_receive.Receive34 == null ? 0 : student_receive.Receive34);
            total_amount += (decimal)(student_receive.Receive35 == null ? 0 : student_receive.Receive35);
            total_amount += (decimal)(student_receive.Receive36 == null ? 0 : student_receive.Receive36);
            total_amount += (decimal)(student_receive.Receive37 == null ? 0 : student_receive.Receive37);
            total_amount += (decimal)(student_receive.Receive38 == null ? 0 : student_receive.Receive38);
            total_amount += (decimal)(student_receive.Receive39 == null ? 0 : student_receive.Receive39);
            total_amount += (decimal)(student_receive.Receive40 == null ? 0 : student_receive.Receive40);

            #region [Old]
            //#region 判斷是否有student_loan 20150906
            //if (existStudentLoan(student_receive.ReceiveType,student_receive.YearId,student_receive.TermId,student_receive.DepId,student_receive.ReceiveId,student_receive.StuId,student_receive.LoanId,student_receive.OldSeq))
            //{
            //    //[MEMO] 這裡以上傳的就貸總額為準，因為目前學校一定會上傳就貸總額 (可以考慮用 RealLoan)
            //    total_amount = total_amount - student_receive.Loan;
            //}
            //#endregion
            #endregion

            #region [New] 有上傳就學貸款金額 (土銀說上傳的就貸金額項目不用去異動 Student_Receive 的收入科目金額，且可貸金額為必傳資料，所以要特別扣除減免金額)
            if (student_receive.Loan > 0)
            {
                total_amount = total_amount - student_receive.Loan;
            }
            #endregion

            #region [MDY:20190906] (2019擴充案) 修正：有上傳減免金額 StudentReduceEntity (土銀說上傳的減免項目金額不用去異動 Student_Receive 的收入科目金額，所以要特別扣除減免金額)
            if (student_receive.FeePayable.HasValue)
            {
                total_amount = total_amount - student_receive.FeePayable.Value;
            }
            #endregion

            return total_amount;
        }

        private EntityFactory _Factory = null;
        private EntityFactory GetEntityFactory()
        {
            if (_Factory == null)
            {
                _Factory = new EntityFactory();
            }
            return _Factory;
        }

        #region [OLD]
        //private bool HasStudentLoanAmount(string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq, string loadId)
        //{
        //    StudentLoanEntity data = null;
        //    Expression where = new Expression(StudentLoanEntity.Field.ReceiveType, receiveType)
        //        .And(StudentLoanEntity.Field.YearId, yearId)
        //        .And(StudentLoanEntity.Field.TermId, termId)
        //        .And(StudentLoanEntity.Field.DepId, depId)
        //        .And(StudentLoanEntity.Field.ReceiveId, receiveId)
        //        .And(StudentLoanEntity.Field.StuId, stuId)
        //        .And(StudentLoanEntity.Field.LoanId, loadId)
        //        .And(StudentLoanEntity.Field.OldSeq, oldSeq);
        //    EntityFactory factory = this.GetEntityFactory();
        //    Result result = factory.SelectFirst<StudentLoanEntity>(where, null, out data);
        //    if (data != null && data.LoanAmount != null && data.LoanAmount.Value > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //private bool existStudentLoan(string receive_type,string year_id,string term_id,string dep_id,string receive_id,string stu_id,string loan_id,int old_seq)
        //{
        //    bool rc = false;
        //    StudentLoanEntity student_loan=new StudentLoanEntity();
        //    Expression where = new Expression(StudentLoanEntity.Field.ReceiveType, receive_type);
        //    where.And(StudentLoanEntity.Field.YearId, year_id);
        //    where.And(StudentLoanEntity.Field.TermId, term_id);
        //    where.And(StudentLoanEntity.Field.DepId, dep_id);
        //    where.And(StudentLoanEntity.Field.ReceiveId, receive_id);
        //    where.And(StudentLoanEntity.Field.StuId, stu_id);
        //    where.And(StudentLoanEntity.Field.LoanId, loan_id);
        //    where.And(StudentLoanEntity.Field.OldSeq, old_seq);
        //    KeyValueList<OrderByEnum> orderbys = null;
        //    EntityFactory factory = new EntityFactory();
        //    Result result = factory.SelectFirst<StudentLoanEntity>(where, orderbys, out student_loan);
        //    if (result.IsSuccess && student_loan!=null)
        //    {
        //        rc = true;
        //    }
        //    else
        //    {
        //        rc = false;
        //    }
        //    return rc;
        //}
        #endregion

        /// <summary>
        /// 取得學分費收費標準
        /// </summary>
        /// <param name="receive_type">代收類別</param>
        /// <param name="year_id">學年</param>
        /// <param name="term_id">學期</param>
        /// <param name="dep_id">部別</param>
        /// <param name="receive_id">費用別</param>
        /// <param name="college_id">院別</param>
        /// <param name="credit_standard">回傳</param>
        /// <returns></returns>
        private bool getCreditStandard(string receive_type, string year_id, string term_id, string dep_id, string receive_id,string college_id,out CreditStandardEntity credit_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},college_id={5}", receive_type, year_id, term_id, dep_id, receive_id, college_id);

            credit_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(CreditStandardEntity.Field.ReceiveType, receive_type);
            where.And(CreditStandardEntity.Field.YearId, year_id);
            where.And(CreditStandardEntity.Field.TermId, term_id);
            where.And(CreditStandardEntity.Field.DepId, dep_id);
            where.And(CreditStandardEntity.Field.ReceiveId, receive_id);
            where.And(CreditStandardEntity.Field.CollegeId, college_id);
            Result result = factroy.SelectFirst<CreditStandardEntity>(where, orderbys, out credit_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getCreditStandard] 讀取credit_standard發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (credit_standard == null)
                {
                    where = new Expression(CreditStandardEntity.Field.ReceiveType, receive_type);
                    where.And(CreditStandardEntity.Field.YearId, year_id);
                    where.And(CreditStandardEntity.Field.TermId, term_id);
                    where.And(CreditStandardEntity.Field.DepId, dep_id);
                    where.And(CreditStandardEntity.Field.ReceiveId, receive_id);
                    where.And(CreditStandardEntity.Field.CollegeId, "all");
                    result = factroy.SelectFirst<CreditStandardEntity>(where, orderbys, out credit_standard);
                    if (!result.IsSuccess)
                    {
                        _err_msg = string.Format("[getCreditStandard] 讀取credit_standard發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
                    }
                    else
                    {
                        if(credit_standard==null)
                        {
                            rc = true;
                            _err_msg = string.Format("[getCreditStandard] 讀取credit_standard發生錯誤，錯誤訊息={0}，key={1}", "找不到學分費收費標準", key);
                        }
                        else
                        {
                            rc = true;
                        }
                    }
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        /// <summary>
        /// 課程收費標準
        /// </summary>
        /// <param name="receive_type">代收類別</param>
        /// <param name="year_id">學年</param>
        /// <param name="term_id">學期</param>
        /// <param name="course_id">課程代碼</param>
        /// <param name="class_standard">回傳</param>
        /// <returns></returns>
        private bool getClassStandard(string receive_type, string year_id, string term_id,out ClassStandardEntity[] class_standards)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2}", receive_type, year_id, term_id);

            class_standards = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(ClassStandardEntity.Field.ReceiveType, receive_type);
            where.And(ClassStandardEntity.Field.YearId, year_id);
            where.And(ClassStandardEntity.Field.TermId, term_id);
            Result result = factroy.Select<ClassStandardEntity>(where, orderbys,0,0, out class_standards);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getClassStandard] 讀取class_standard發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (class_standards == null || class_standards.Length <= 0)
                {
                    rc = true;
                    _err_msg = string.Format("[getClassStandard] 讀取class_standard發生錯誤，錯誤訊息={0}，key={1}", "找不到課程標準檔設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        private bool getCreditbStandard(string receive_type, string year_id, string term_id, string dep_id, string receive_id,out CreditbStandardEntity[] creditb_standards)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4}", receive_type, year_id, term_id, dep_id,receive_id);

            creditb_standards = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(CreditbStandardEntity.Field.ReceiveType, receive_type);
            where.And(CreditbStandardEntity.Field.YearId, year_id);
            where.And(CreditbStandardEntity.Field.TermId, term_id);
            where.And(CreditbStandardEntity.Field.DepId, dep_id);
            where.And(CreditbStandardEntity.Field.ReceiveId, receive_id);
            Result result = factroy.Select<CreditbStandardEntity>(where, orderbys,0,0, out creditb_standards);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getCreditbStandard] 讀取class_standard發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (creditb_standards == null || creditb_standards.Length<=0)
                {
                    rc = true;
                    _err_msg = string.Format("[getCreditbStandard] 讀取class_standard發生錯誤，錯誤訊息={0}，key={1}", "找不到學分基準標準檔設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        /// <summary>
        /// 小於學分費收費標準
        /// </summary>
        /// <param name="receive_type">代收類別</param>
        /// <param name="year_id">學年</param>
        /// <param name="term_id">學期</param>
        /// <param name="dep_id">部別</param>
        /// <param name="receive_id">費用別</param>
        /// <param name="credit2_standard">回傳</param>
        /// <returns></returns>
        private bool getCredit2Standard(string receive_type, string year_id, string term_id, string dep_id, string receive_id,out Credit2StandardEntity credit2_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4}", receive_type, year_id, term_id, dep_id, receive_id);

            credit2_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(Credit2StandardEntity.Field.ReceiveType, receive_type);
            where.And(Credit2StandardEntity.Field.YearId, year_id);
            where.And(Credit2StandardEntity.Field.TermId, term_id);
            where.And(Credit2StandardEntity.Field.DepId, dep_id);
            where.And(Credit2StandardEntity.Field.ReceiveId, receive_id);
            Result result = factroy.SelectFirst<Credit2StandardEntity>(where, orderbys, out credit2_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getCredit2Standard] 讀取credit2_standard發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (credit2_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getCredit2Standard] 讀取credit2_standard發生錯誤，錯誤訊息={0}，key={1}", "找不到小於基準其他收費標準", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        /// <summary>
        /// 讀取學生課程設定
        /// </summary>
        /// <param name="receive_type">代收類別</param>
        /// <param name="year_id">學年</param>
        /// <param name="term_id">學期</param>
        /// <param name="dep_id">部別</param>
        /// <param name="receive_id">費用別</param>
        /// <param name="stu_id">學號</param>
        /// <param name="oldSeq">舊資料序號</param>
        /// <param name="student_course">回傳</param>
        /// <returns></returns>
        private bool getStudentCourse(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string stu_id, int oldSeq, out StudentCourseEntity student_course)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5}", receive_type, year_id, term_id, dep_id, receive_id,stu_id);

            student_course = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(StudentCourseEntity.Field.ReceiveType, receive_type);
            where.And(StudentCourseEntity.Field.YearId, year_id);
            where.And(StudentCourseEntity.Field.TermId, term_id);
            where.And(StudentCourseEntity.Field.DepId, dep_id);
            where.And(StudentCourseEntity.Field.ReceiveId, receive_id);
            where.And(StudentCourseEntity.Field.StuId, stu_id);
            where.And(StudentCourseEntity.Field.OldSeq, oldSeq);
            Result result = factroy.SelectFirst<StudentCourseEntity>(where, orderbys, out student_course);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getStudentCourse] 讀取student_course發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (student_course == null)
                {
                    rc = true;//沒有也沒關係
                    _err_msg = string.Format("[getStudentCourse] 讀取student_course發生錯誤，錯誤訊息={0}，key={1}", "找不到學生課程設定設定", key);
                }
                else
                {
                    rc = true;
                }
            }
            return rc;
        }

        private bool getDormStandard(string receive_type, string year_id, string term_id, string dep_id, string receive_id,string dorm_id, out DormStandardEntity dorm_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},dorm_id={5}", receive_type, year_id, term_id, dep_id, receive_id, dorm_id);//20160306 jj

            dorm_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(DormStandardEntity.Field.ReceiveType, receive_type);
            where.And(DormStandardEntity.Field.YearId, year_id);
            where.And(DormStandardEntity.Field.TermId, term_id);
            where.And(DormStandardEntity.Field.DepId, dep_id);
            where.And(DormStandardEntity.Field.ReceiveId, receive_id);
            where.And(DormStandardEntity.Field.DormId, dorm_id);
            Result result = factroy.SelectFirst<DormStandardEntity>(where, orderbys, out dorm_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getDormStandard] 讀取Dorm_Standard發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (dorm_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getDormStandard] 讀取Dorm_Standard發生錯誤，錯誤訊息={0}，key={1}", "找不到住宿標準設定", key);//20160306 jj
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        //20160306 jj
        private bool getReduceStandard(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string reduce_id, out ReduceStandardEntity reduce_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},reduce_id={5}", receive_type, year_id, term_id, dep_id, receive_id, reduce_id);

            reduce_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(ReduceStandardEntity.Field.ReceiveType, receive_type);
            where.And(ReduceStandardEntity.Field.YearId, year_id);
            where.And(ReduceStandardEntity.Field.TermId, term_id);
            where.And(ReduceStandardEntity.Field.DepId, dep_id);
            where.And(ReduceStandardEntity.Field.ReceiveId, receive_id);
            where.And(ReduceStandardEntity.Field.ReduceId, reduce_id);
            Result result = factroy.SelectFirst<ReduceStandardEntity>(where, orderbys, out reduce_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getReduceStandard] 讀取Reduce_Standard發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (reduce_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getReduceStandard] 讀取Reduce_Standard發生錯誤，錯誤訊息={0}，key={1}", "找不到減免標準設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        private bool getIdentifyStandard1(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string identify_id, out IdentifyStandard1Entity identify_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},identify_id={5}", receive_type, year_id, term_id, dep_id, receive_id, identify_id);

            identify_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(IdentifyStandard1Entity.Field.ReceiveType, receive_type);
            where.And(IdentifyStandard1Entity.Field.YearId, year_id);
            where.And(IdentifyStandard1Entity.Field.TermId, term_id);
            where.And(IdentifyStandard1Entity.Field.DepId, dep_id);
            where.And(IdentifyStandard1Entity.Field.ReceiveId, receive_id);
            where.And(IdentifyStandard1Entity.Field.IdentifyId, identify_id);
            Result result = factroy.SelectFirst<IdentifyStandard1Entity>(where, orderbys, out identify_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getIdentifyStandard1] 讀取Identify_Standard1發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (identify_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getIdentifyStandard1] 讀取Identify_Standard1發生錯誤，錯誤訊息={0}，key={1}", "找不到身份註記標準設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        private bool getIdentifyStandard2(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string identify_id, out IdentifyStandard2Entity identify_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},identify_id={5}", receive_type, year_id, term_id, dep_id, receive_id, identify_id);

            identify_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(IdentifyStandard2Entity.Field.ReceiveType, receive_type);
            where.And(IdentifyStandard2Entity.Field.YearId, year_id);
            where.And(IdentifyStandard2Entity.Field.TermId, term_id);
            where.And(IdentifyStandard2Entity.Field.DepId, dep_id);
            where.And(IdentifyStandard2Entity.Field.ReceiveId, receive_id);
            where.And(IdentifyStandard2Entity.Field.IdentifyId, identify_id);
            Result result = factroy.SelectFirst<IdentifyStandard2Entity>(where, orderbys, out identify_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getIdentifyStandard2] 讀取Identify_Standard2發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (identify_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getIdentifyStandard2] 讀取Identify_Standard2發生錯誤，錯誤訊息={0}，key={1}", "找不到身份註記標準設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        private bool getIdentifyStandard3(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string identify_id, out IdentifyStandard3Entity identify_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},identify_id={5}", receive_type, year_id, term_id, dep_id, receive_id, identify_id);

            identify_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(IdentifyStandard3Entity.Field.ReceiveType, receive_type);
            where.And(IdentifyStandard3Entity.Field.YearId, year_id);
            where.And(IdentifyStandard3Entity.Field.TermId, term_id);
            where.And(IdentifyStandard3Entity.Field.DepId, dep_id);
            where.And(IdentifyStandard3Entity.Field.ReceiveId, receive_id);
            where.And(IdentifyStandard3Entity.Field.IdentifyId, identify_id);
            Result result = factroy.SelectFirst<IdentifyStandard3Entity>(where, orderbys, out identify_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getIdentifyStandard3] 讀取Identify_Standard3發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (identify_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getIdentifyStandard3] 讀取Identify_Standard3發生錯誤，錯誤訊息={0}，key={1}", "找不到身份註記標準設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        private bool getIdentifyStandard4(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string identify_id, out IdentifyStandard4Entity identify_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},identify_id={5}", receive_type, year_id, term_id, dep_id, receive_id, identify_id);

            identify_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(IdentifyStandard4Entity.Field.ReceiveType, receive_type);
            where.And(IdentifyStandard4Entity.Field.YearId, year_id);
            where.And(IdentifyStandard4Entity.Field.TermId, term_id);
            where.And(IdentifyStandard4Entity.Field.DepId, dep_id);
            where.And(IdentifyStandard4Entity.Field.ReceiveId, receive_id);
            where.And(IdentifyStandard4Entity.Field.IdentifyId, identify_id);
            Result result = factroy.SelectFirst<IdentifyStandard4Entity>(where, orderbys, out identify_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getIdentifyStandard4] 讀取Identify_Standard4發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (identify_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getIdentifyStandard4] 讀取Identify_Standard4發生錯誤，錯誤訊息={0}，key={1}", "找不到身份註記標準設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        private bool getIdentifyStandard5(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string identify_id, out IdentifyStandard5Entity identify_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},identify_id={5}", receive_type, year_id, term_id, dep_id, receive_id, identify_id);

            identify_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(IdentifyStandard5Entity.Field.ReceiveType, receive_type);
            where.And(IdentifyStandard5Entity.Field.YearId, year_id);
            where.And(IdentifyStandard5Entity.Field.TermId, term_id);
            where.And(IdentifyStandard5Entity.Field.DepId, dep_id);
            where.And(IdentifyStandard5Entity.Field.ReceiveId, receive_id);
            where.And(IdentifyStandard5Entity.Field.IdentifyId, identify_id);
            Result result = factroy.SelectFirst<IdentifyStandard5Entity>(where, orderbys, out identify_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getIdentifyStandard5] 讀取Identify_Standard5發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (identify_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getIdentifyStandard5] 讀取Identify_Standard5發生錯誤，錯誤訊息={0}，key={1}", "找不到身份註記標準設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        private bool getIdentifyStandard6(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string identify_id, out IdentifyStandard6Entity identify_standard)
        {
            _err_msg = "";
            bool rc = false;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},identify_id={5}", receive_type, year_id, term_id, dep_id, receive_id, identify_id);

            identify_standard = null;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            factroy = new EntityFactory();
            where = new Expression(IdentifyStandard6Entity.Field.ReceiveType, receive_type);
            where.And(IdentifyStandard6Entity.Field.YearId, year_id);
            where.And(IdentifyStandard6Entity.Field.TermId, term_id);
            where.And(IdentifyStandard6Entity.Field.DepId, dep_id);
            where.And(IdentifyStandard6Entity.Field.ReceiveId, receive_id);
            where.And(IdentifyStandard6Entity.Field.IdentifyId, identify_id);
            Result result = factroy.SelectFirst<IdentifyStandard6Entity>(where, orderbys, out identify_standard);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[getIdentifyStandard6] 讀取Identify_Standard6發生錯誤，錯誤訊息={0}，key={1}", result.Message, key);
            }
            else
            {
                if (identify_standard == null)
                {
                    rc = true;
                    _err_msg = string.Format("[getIdentifyStandard6] 讀取Identify_Standard6發生錯誤，錯誤訊息={0}，key={1}", "找不到身份註記標準設定", key);
                }
                else
                {
                    rc = true;
                }
            }

            return rc;
        }

        /// <summary>
        /// 取得一般標準設定，並依照一般收費標準，計算每個收入課目金額
        /// </summary>
        /// <param name="student_receive">學生繳費資料</param>
        /// <param name="school_rid">費用別檔</param>
        /// <param name="receive_item_count">收入科目數</param>
        /// <param name="receive_items_amount">回傳收入科目金額</param>
        /// <returns>成功或失敗</returns>
        private bool calcByGeneralStandard(StudentReceiveEntity student_receive, SchoolRidEntity school_rid, Int32 receive_item_count, ref decimal[] receive_items_amount)
        {
            //receive_items_amount不要init，因為要串整個流程
            _err_msg = "";
            bool rc = false;

            //建立資料庫連線
            EntityFactory factroy = null;
            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;

            where = new Expression();
            where.And(GeneralStandardEntity.Field.ReceiveType, student_receive.ReceiveType);
            where.And(GeneralStandardEntity.Field.YearId, student_receive.YearId);
            where.And(GeneralStandardEntity.Field.TermId, student_receive.TermId);
            where.And(GeneralStandardEntity.Field.DepId, student_receive.DepId);
            where.And(GeneralStandardEntity.Field.ReceiveId, student_receive.ReceiveId);

            Expression where1 = new Expression(GeneralStandardEntity.Field.CollegeId, student_receive.CollegeId);
            where1.Or(GeneralStandardEntity.Field.CollegeId, "all");
            where.And(where1);
            where1 = new Expression(GeneralStandardEntity.Field.MajorId, student_receive.MajorId);
            where1.Or(GeneralStandardEntity.Field.MajorId, "all");
            where.And(where1);
            where1 = new Expression(GeneralStandardEntity.Field.StuGrade, student_receive.StuGrade);
            where1.Or(GeneralStandardEntity.Field.StuGrade, "0");
            where.And(where1);
            where1 = new Expression(GeneralStandardEntity.Field.ClassId, student_receive.ClassId);
            where1.Or(GeneralStandardEntity.Field.ClassId, "all");
            where.And(where1);

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(GeneralStandardEntity.Field.OrderId, OrderByEnum.Desc);

            GeneralStandardEntity[] general_standards = null;
            factroy = new EntityFactory();
            Result result = factroy.Select<GeneralStandardEntity>(where, orderbys, 0, 0, out general_standards);
            if (!result.IsSuccess)
            {
                _err_msg = string.Format("[calcReceiveItemAmount] 計算一般收費標準發生錯誤，錯誤訊息={0}", result.Message);
                return rc;
            }
            else
            {
                if (general_standards == null || general_standards.Length <= 0)
                {
                    _err_msg = string.Format("[calcReceiveItemAmount] 沒有一般收費標準設定");
                    return rc;
                }
                else
                {
                    for (int i = 0; i < receive_item_count; i++)
                    {
                        for (int j = 0; j < general_standards.Length; j++)
                        {
                            GeneralStandardEntity general_standard = general_standards[j];
                            #region 如果找到收入課目的設定就跳下一個
                            if (i==0 && general_standard.General01 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General01;
                                break;
                            }
                            if (i==1 && general_standard.General02 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General02;
                                break;
                            }
                            if (i==2 && general_standard.General03 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General03;
                                break;
                            }
                            if (i==3 && general_standard.General04 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General04;
                                break;
                            }
                            if (i==4 && general_standard.General05 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General05;
                                break;
                            }
                            if (i==5 && general_standard.General06 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General06;
                                break;
                            }
                            if (i==6 && general_standard.General07 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General07;
                                break;
                            }
                            if (i==7 && general_standard.General08 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General08;
                                break;
                            }
                            if (i==8 && general_standard.General09 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General09;
                                break;
                            }
                            if (i==9 && general_standard.General10 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General10;
                                break;
                            }
                            if (i==10 && general_standard.General11 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General11;
                                break;
                            }
                            if (i==11 && general_standard.General12 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General12;
                                break;
                            }
                            if (i==12 && general_standard.General13 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General13;
                                break;
                            }
                            if (i==13 && general_standard.General14 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General14;
                                break;
                            }
                            if (i==14 && general_standard.General15 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General15;
                                break;
                            }
                            if (i==15 && general_standard.General16 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General16;
                                break;
                            }
                            if (i==16 && general_standard.General17 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General17;
                                break;
                            }
                            if (i==17 && general_standard.General18 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General18;
                                break;
                            }
                            if (i==18 && general_standard.General19 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General19;
                                break;
                            }
                            if (i==19 && general_standard.General20 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General20;
                                break;
                            }
                            if (i==20 && general_standard.General21 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General21;
                                break;
                            }
                            if (i==21 && general_standard.General22 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General22;
                                break;
                            }
                            if (i==22 && general_standard.General23 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General23;
                                break;
                            }
                            if (i==23 && general_standard.General24 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General24;
                                break;
                            }
                            if (i==24 && general_standard.General25 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General25;
                                break;
                            }
                            if (i==25 && general_standard.General26 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General26;
                                break;
                            }
                            if (i==26 && general_standard.General27 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General27;
                                break;
                            }
                            if (i==27 && general_standard.General28 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General28;
                                break;
                            }
                            if (i==28 && general_standard.General29 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General29;
                                break;
                            }
                            if (i==29 && general_standard.General30 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General30;
                                break;
                            }

                            if (i == 30 && general_standard.General31 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General31;
                                break;
                            }
                            if (i == 31 && general_standard.General32 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General32;
                                break;
                            }
                            if (i == 32 && general_standard.General33 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General33;
                                break;
                            }
                            if (i == 33 && general_standard.General34 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General34;
                                break;
                            }
                            if (i == 34 && general_standard.General35 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General35;
                                break;
                            }
                            if (i == 35 && general_standard.General36 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General36;
                                break;
                            }
                            if (i == 36 && general_standard.General37 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General37;
                                break;
                            }
                            if (i == 37 && general_standard.General38 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General38;
                                break;
                            }
                            if (i == 38 && general_standard.General39 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General39;
                                break;
                            }
                            if (i == 39 && general_standard.General40 != null)
                            {
                                receive_items_amount[i] = (decimal)general_standard.General40;
                                break;
                            }
                            #endregion
                        }
                    }
                    rc = true;
                }
            }

            return rc;
        }

        private bool calcByCreditStand(StudentReceiveEntity student_receive, SchoolRidEntity school_rid,CreditStandardEntity credit_standard,Credit2StandardEntity credit2_standard,StudentCourseEntity student_course,ClassStandardEntity[] class_standards, CreditbStandardEntity[] creditb_standards, Int32 receive_item_count, ref decimal[] receive_items_amount)
        {
            //receive_items_amount不要init，因為要串整個流程
            _err_msg = "";
            bool rc = false;

            decimal receive_item_amount = 0;
            switch (school_rid.StudentType)
            {
                case "1"://無學分費之收入科目
                    for (Int32 i = 0; i < receive_items_amount.Length; i++)
                    {
                        if(!String.IsNullOrWhiteSpace(school_rid.CreditItem))
                        {
                            if (calcItemAmountByCourse(student_course, class_standards, creditb_standards, i, out receive_item_amount))
                            {
                                receive_items_amount[i] += receive_item_amount;
                            }
                        }
                    }
                    rc = true;
                    break;
                case "2"://以學分數計算
                    if(credit_standard !=null)
                    {
                        if (!String.IsNullOrWhiteSpace(credit_standard.CreditItem))
                        {
                            receive_items_amount[Convert.ToInt32(credit_standard.CreditItem.Trim()) - 1] += (decimal)student_receive.StuCredit * (decimal)credit_standard.CreditPrice;
                        }
                    }
                    for (Int32 i = 0; i < receive_items_amount.Length; i++)
                    {
                        if (calcItemAmountByCourse(student_course, class_standards, creditb_standards, i, out receive_item_amount))
                        {
                            receive_items_amount[i] += receive_item_amount;
                        }
                    }
                    rc = true;
                    break;
                case "3"://以上課時數計算
                    if (credit_standard != null)
                    {
                        if (!String.IsNullOrWhiteSpace(credit_standard.CreditItem))
                        {
                            receive_items_amount[Convert.ToInt32(credit_standard.CreditItem.Trim())-1] += (decimal)student_receive.StuHour * (decimal)credit_standard.CreditPrice;
                        }
                    }
                    for (Int32 i = 0; i < receive_items_amount.Length; i++)
                    {
                        if (calcItemAmountByCourse(student_course, class_standards, creditb_standards, i, out receive_item_amount))
                        {
                            receive_items_amount[i] += receive_item_amount;
                        }
                    }
                    rc = true;
                    break;
                case "4"://以學分數少於某一數目來計算
                    if (Convert.ToInt32(school_rid.CreditBasic) > Convert.ToInt32(student_receive.StuCredit))
                    {
                        if (credit_standard != null)
                        {
                            if (!String.IsNullOrWhiteSpace(credit_standard.CreditItem))
                            {
                                receive_items_amount[Convert.ToInt32(credit_standard.CreditItem.Trim())] += (decimal)student_receive.StuCredit * (decimal)credit_standard.CreditPrice;
                            }
                        }
                    }
                    for (Int32 i = 0; i < receive_items_amount.Length; i++)
                    {
                        if (calcItemAmountByCourse(student_course, class_standards, creditb_standards, i, out receive_item_amount))
                        {
                            receive_items_amount[i] += receive_item_amount;
                        }
                    }
                    rc = true;
                    break;
                case "5"://以上課時數某一時數來計算
                    if (Convert.ToInt32(school_rid.CreditBasic) > Convert.ToInt32(student_receive.StuHour))
                    {
                        if (credit_standard != null)
                        {
                            if (!String.IsNullOrWhiteSpace(credit_standard.CreditItem))
                            {
                                receive_items_amount[Convert.ToInt32(credit_standard.CreditItem.Trim())] += (decimal)student_receive.StuHour * (decimal)credit_standard.CreditPrice;
                            }
                        }
                    }
                    for (Int32 i = 0; i < receive_items_amount.Length; i++)
                    {
                        if (calcItemAmountByCourse(student_course, class_standards, creditb_standards, i, out receive_item_amount))
                        {
                            receive_items_amount[i] += receive_item_amount;
                        }
                    }
                    rc = true;
                    break;
                case "6"://以學分數少於某一數目來計算、但以上課時數某一時數來計算
                    if (Convert.ToInt32(school_rid.CreditBasic) > Convert.ToInt32(student_receive.StuCredit))
                    {
                        if (credit_standard != null)
                        {
                            if (!String.IsNullOrWhiteSpace(credit_standard.CreditItem))
                            {
                                receive_items_amount[Convert.ToInt32(credit_standard.CreditItem.Trim())] += (decimal)student_receive.StuHour * (decimal)credit_standard.CreditPrice;
                            }
                        }
                    }
                    for (Int32 i = 0; i < receive_items_amount.Length; i++)
                    {
                        if (calcItemAmountByCourse(student_course, class_standards, creditb_standards, i, out receive_item_amount))
                        {
                            receive_items_amount[i] += receive_item_amount;
                        }
                    }
                    if (credit_standard != null)
                    {
                        if(school_rid.CreditBasic <=student_receive.StuCredit)
                        {
                            receive_items_amount[Convert.ToInt32(credit_standard.CreditItem.Trim())] = 0;
                        }
                        else
                        {
                            if (credit2_standard != null)
                            {
                                for(int i=0;i<receive_items_amount.Length;i++)
                                {
                                    if(Convert.ToInt32(credit_standard.CreditItem.Trim())!=i)
                                    {
                                        receive_items_amount[i] = 0;
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                    rc = true;
                    break;
            }
            return rc;
        }

        private bool calcItemAmountByCourse(StudentCourseEntity student_course, ClassStandardEntity[] class_standards, CreditbStandardEntity[] creditb_standards, Int32 receive_item_no, out decimal receive_item_amount)
        {
            //receive_items_amount不要init，因為要串整個流程
            _err_msg = "";
            bool rc = false;

            string credit_id = "";
            string course_id = "";
            decimal credit=0;

            receive_item_amount = 0;
            if (student_course != null)
            {
                Int32 i = receive_item_no + 1;
                #region 取得學生課程資料
                if (i == 1)
                {
                    credit_id = student_course.CreditId1.Trim();
                    course_id = student_course.CourseId1.Trim();
                    credit = (decimal)student_course.Credit1;
                }
                if (i == 2)
                {
                    credit_id = student_course.CreditId2.Trim();
                    course_id = student_course.CourseId2.Trim();
                    credit = (decimal)student_course.Credit2;
                }
                if (i == 3)
                {
                    credit_id = student_course.CreditId3.Trim();
                    course_id = student_course.CourseId3.Trim();
                    credit = (decimal)student_course.Credit3;
                }
                if (i == 4)
                {
                    credit_id = student_course.CreditId4.Trim();
                    course_id = student_course.CourseId4.Trim();
                    credit = (decimal)student_course.Credit4;
                }
                if (i == 5)
                {
                    credit_id = student_course.CreditId5.Trim();
                    course_id = student_course.CourseId5.Trim();
                    credit = (decimal)student_course.Credit5;
                }
                if (i == 6)
                {
                    credit_id = student_course.CreditId6.Trim();
                    course_id = student_course.CourseId6.Trim();
                    credit = (decimal)student_course.Credit6;
                }
                if (i == 7)
                {
                    credit_id = student_course.CreditId7.Trim();
                    course_id = student_course.CourseId7.Trim();
                    credit = (decimal)student_course.Credit7;
                }
                if (i == 8)
                {
                    credit_id = student_course.CreditId8.Trim();
                    course_id = student_course.CourseId8.Trim();
                    credit = (decimal)student_course.Credit8;
                }
                if (i == 9)
                {
                    credit_id = student_course.CreditId9.Trim();
                    course_id = student_course.CourseId9.Trim();
                    credit = (decimal)student_course.Credit9;
                }
                if (i == 10)
                {
                    credit_id = student_course.CreditId10.Trim();
                    course_id = student_course.CourseId10.Trim();
                    credit = (decimal)student_course.Credit10;
                }
                if (i == 11)
                {
                    credit_id = student_course.CreditId11.Trim();
                    course_id = student_course.CourseId11.Trim();
                    credit = (decimal)student_course.Credit11;
                }
                if (i == 12)
                {
                    credit_id = student_course.CreditId12.Trim();
                    course_id = student_course.CourseId12.Trim();
                    credit = (decimal)student_course.Credit12;
                }
                if (i == 13)
                {
                    credit_id = student_course.CreditId13.Trim();
                    course_id = student_course.CourseId13.Trim();
                    credit = (decimal)student_course.Credit13;
                }
                if (i == 14)
                {
                    credit_id = student_course.CreditId14.Trim();
                    course_id = student_course.CourseId14.Trim();
                    credit = (decimal)student_course.Credit14;
                }
                if (i == 15)
                {
                    credit_id = student_course.CreditId15.Trim();
                    course_id = student_course.CourseId15.Trim();
                    credit = (decimal)student_course.Credit15;
                }
                if (i == 16)
                {
                    credit_id = student_course.CreditId16.Trim();
                    course_id = student_course.CourseId16.Trim();
                    credit = (decimal)student_course.Credit16;
                }
                if (i == 17)
                {
                    credit_id = student_course.CreditId17.Trim();
                    course_id = student_course.CourseId17.Trim();
                    credit = (decimal)student_course.Credit17;
                }
                if (i == 18)
                {
                    credit_id = student_course.CreditId18.Trim();
                    course_id = student_course.CourseId18.Trim();
                    credit = (decimal)student_course.Credit18;
                }
                if (i == 19)
                {
                    credit_id = student_course.CreditId19.Trim();
                    course_id = student_course.CourseId19.Trim();
                    credit = (decimal)student_course.Credit19;
                }
                if (i == 20)
                {
                    credit_id = student_course.CreditId20.Trim();
                    course_id = student_course.CourseId20.Trim();
                    credit = (decimal)student_course.Credit20;
                }
                if (i == 21)
                {
                    credit_id = student_course.CreditId21.Trim();
                    course_id = student_course.CourseId21.Trim();
                    credit = (decimal)student_course.Credit21;
                }
                if (i == 22)
                {
                    credit_id = student_course.CreditId22.Trim();
                    course_id = student_course.CourseId22.Trim();
                    credit = (decimal)student_course.Credit22;
                }
                if (i == 23)
                {
                    credit_id = student_course.CreditId23.Trim();
                    course_id = student_course.CourseId23.Trim();
                    credit = (decimal)student_course.Credit23;
                }
                if (i == 24)
                {
                    credit_id = student_course.CreditId24.Trim();
                    course_id = student_course.CourseId24.Trim();
                    credit = (decimal)student_course.Credit24;
                }
                if (i == 25)
                {
                    credit_id = student_course.CreditId25.Trim();
                    course_id = student_course.CourseId25.Trim();
                    credit = (decimal)student_course.Credit25;
                }
                if (i == 26)
                {
                    credit_id = student_course.CreditId26.Trim();
                    course_id = student_course.CourseId26.Trim();
                    credit = (decimal)student_course.Credit26;
                }
                if (i == 27)
                {
                    credit_id = student_course.CreditId27.Trim();
                    course_id = student_course.CourseId27.Trim();
                    credit = (decimal)student_course.Credit27;
                }
                if (i == 28)
                {
                    credit_id = student_course.CreditId28.Trim();
                    course_id = student_course.CourseId28.Trim();
                    credit = (decimal)student_course.Credit28;
                }
                if (i == 29)
                {
                    credit_id = student_course.CreditId29.Trim();
                    course_id = student_course.CourseId29.Trim();
                    credit = (decimal)student_course.Credit29;
                }
                if (i == 30)
                {
                    credit_id = student_course.CreditId30.Trim();
                    course_id = student_course.CourseId30.Trim();
                    credit = (decimal)student_course.Credit30;
                }

                if (i == 31)
                {
                    credit_id = student_course.CreditId31.Trim();
                    course_id = student_course.CourseId31.Trim();
                    credit = (decimal)student_course.Credit31;
                }
                if (i == 32)
                {
                    credit_id = student_course.CreditId32.Trim();
                    course_id = student_course.CourseId32.Trim();
                    credit = (decimal)student_course.Credit32;
                }
                if (i == 33)
                {
                    credit_id = student_course.CreditId33.Trim();
                    course_id = student_course.CourseId33.Trim();
                    credit = (decimal)student_course.Credit33;
                }
                if (i == 34)
                {
                    credit_id = student_course.CreditId34.Trim();
                    course_id = student_course.CourseId34.Trim();
                    credit = (decimal)student_course.Credit34;
                }
                if (i == 35)
                {
                    credit_id = student_course.CreditId35.Trim();
                    course_id = student_course.CourseId35.Trim();
                    credit = (decimal)student_course.Credit35;
                }
                if (i == 36)
                {
                    credit_id = student_course.CreditId36.Trim();
                    course_id = student_course.CourseId36.Trim();
                    credit = (decimal)student_course.Credit36;
                }
                if (i == 37)
                {
                    credit_id = student_course.CreditId37.Trim();
                    course_id = student_course.CourseId37.Trim();
                    credit = (decimal)student_course.Credit37;
                }
                if (i == 38)
                {
                    credit_id = student_course.CreditId38.Trim();
                    course_id = student_course.CourseId38.Trim();
                    credit = (decimal)student_course.Credit38;
                }
                if (i == 39)
                {
                    credit_id = student_course.CreditId39.Trim();
                    course_id = student_course.CourseId39.Trim();
                    credit = (decimal)student_course.Credit39;
                }
                if (i == 40)
                {
                    credit_id = student_course.CreditId40.Trim();
                    course_id = student_course.CourseId40.Trim();
                    credit = (decimal)student_course.Credit40;
                }
                #endregion
            }
            else
            {
                rc = true;
            }

            if (credit_id != "")
            {
                #region 用學分計算
                foreach(CreditbStandardEntity creditb_standard in creditb_standards)
                {
                    if(creditb_standard.CreditbId==credit_id)
                    {
                        receive_item_amount = credit * creditb_standard.CreditbCprice;
                    }
                }
                #endregion
            }
            else
            {
                #region 用課程計算
                if (class_standards != null && class_standards.Length > 0)
                {
                    foreach (ClassStandardEntity class_standard in class_standards)
                    {
                        if (class_standard.CourseId == course_id)
                        {
                            receive_item_amount = (decimal)class_standard.CourseCredit * (decimal)class_standard.CourseCprice;
                            break;
                        }
                    }
                }
                #endregion
            }

            rc = true;
            return rc;
        }

        /// <summary>
        /// 依標準計算每個收入科目的金額
        /// </summary>
        /// <param name="student_receive">要計算的學生繳費資料，計算後會回存該欄位</param>
        /// <returns>成功/失敗。失敗可以查err_msg</returns>
        private bool calcReceiveItemAmount(ref StudentReceiveEntity student_receive)
        {
            _err_msg = "";
            //string log = "";
            StringBuilder logs = new StringBuilder();
            bool rc = false;

            #region 讀取student_rid
            SchoolRidEntity school_rid = new SchoolRidEntity();
            if (!this.getSchoolRid(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, out school_rid))
            {
                _err_msg = string.Format("[calcReceiveItemAmount] {0}", _err_msg);
                return rc;
            }
            #endregion

            #region 初始一個收入明細的array，用來暫存收入科目金額
            Int32 receive_item_count = 40;// this.countReceiveItems(school_rid);
            decimal[] receive_items_amount = new decimal[receive_item_count];
            for (int i = 0; i < receive_items_amount.Length; i++)
            {
                receive_items_amount[i] = 0;
            }
            #endregion

            #region 讀取標準檔
            //標準檔可以不存在，所以null不能當失敗
            CreditStandardEntity credit_standard = new CreditStandardEntity();
            if(!getCreditStandard(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId,student_receive.CollegeId,out credit_standard))
            {
                _err_msg = string.Format("[calcReceiveItemAmount] {0}", _err_msg);
                return rc;
            }
            ClassStandardEntity[] class_standards = null;
            if (!getClassStandard(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, out class_standards))
            {
                _err_msg = string.Format("[calcReceiveItemAmount] {0}", _err_msg);
                return rc;
            }
            CreditbStandardEntity[] creditb_standards = null;
            if(!getCreditbStandard(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId,out creditb_standards))
            {
                _err_msg = string.Format("[calcReceiveItemAmount] {0}", _err_msg);
                return rc;
            }
            DormStandardEntity dorm_standard = new DormStandardEntity();
            ReduceStandardEntity reduce_standard = new ReduceStandardEntity();
            StudentCourseEntity student_course = new StudentCourseEntity();
            if (!getStudentCourse(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.StuId, student_receive.OldSeq, out student_course))
            {
                _err_msg = string.Format("[calcReceiveItemAmount] {0}", _err_msg);
                return rc;
            }
            Credit2StandardEntity credit2_standard = new Credit2StandardEntity();
            #endregion

            #region 計算一般收費標準
            if (!this.calcByGeneralStandard(student_receive,school_rid,receive_item_count,ref receive_items_amount))
            {
                _err_msg = string.Format("[calcReceiveItemAmount] {0}", _err_msg);
                return rc;
            }
            #endregion

            #region 計算學分費
            if(!calcByCreditStand(student_receive,school_rid,credit_standard,credit2_standard,student_course,class_standards,creditb_standards,receive_item_count,ref receive_items_amount))
            {
                _err_msg = string.Format("[calcReceiveItemAmount] {0}", _err_msg);
                return rc;
            }
            #endregion

            #region 計算住宿費
            if (this.getDormStandard(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId,student_receive.DormId, out dorm_standard))
            {
                #region 把住宿費塞到正確的收入科目
                if (dorm_standard!=null)
                {
                    if (dorm_standard.DormItem == "01")
                    {
                        receive_items_amount[0] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "02")
                    {
                        receive_items_amount[1] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "03")
                    {
                        receive_items_amount[2] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "04")
                    {
                        receive_items_amount[3] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "05")
                    {
                        receive_items_amount[4] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "06")
                    {
                        receive_items_amount[5] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "07")
                    {
                        receive_items_amount[6] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "08")
                    {
                        receive_items_amount[7] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "09")
                    {
                        receive_items_amount[8] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "10")
                    {
                        receive_items_amount[9] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "11")
                    {
                        receive_items_amount[10] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "12")
                    {
                        receive_items_amount[11] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "13")
                    {
                        receive_items_amount[12] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "14")
                    {
                        receive_items_amount[13] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "15")
                    {
                        receive_items_amount[14] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "16")
                    {
                        receive_items_amount[15] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "17")
                    {
                        receive_items_amount[16] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "18")
                    {
                        receive_items_amount[17] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "19")
                    {
                        receive_items_amount[18] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "20")
                    {
                        receive_items_amount[19] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "21")
                    {
                        receive_items_amount[20] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "22")
                    {
                        receive_items_amount[21] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "23")
                    {
                        receive_items_amount[22] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "24")
                    {
                        receive_items_amount[23] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "25")
                    {
                        receive_items_amount[24] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "26")
                    {
                        receive_items_amount[25] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "27")
                    {
                        receive_items_amount[26] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "28")
                    {
                        receive_items_amount[27] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "29")
                    {
                        receive_items_amount[28] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "30")
                    {
                        receive_items_amount[29] = (decimal)dorm_standard.DormAmount;
                    }

                    if (dorm_standard.DormItem == "31")
                    {
                        receive_items_amount[30] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "32")
                    {
                        receive_items_amount[31] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "33")
                    {
                        receive_items_amount[32] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "34")
                    {
                        receive_items_amount[33] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "35")
                    {
                        receive_items_amount[34] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "36")
                    {
                        receive_items_amount[35] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "37")
                    {
                        receive_items_amount[36] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "38")
                    {
                        receive_items_amount[37] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "39")
                    {
                        receive_items_amount[38] = (decimal)dorm_standard.DormAmount;
                    }
                    if (dorm_standard.DormItem == "40")
                    {
                        receive_items_amount[39] = (decimal)dorm_standard.DormAmount;
                    }
                }
                #endregion
            }
            #endregion

            #region 計算代辦費用(沒有代辦費)

            #endregion

            #region 計算身份註記1-6項
            IdentifyStandard1Entity identify_standard = null;
            #region 身份註記1
            if (this.getIdentifyStandard1(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.IdentifyId01, out identify_standard))
            {
                if (identify_standard != null)
                {
                    for (Int32 i = 1; i < 41; i++)
                    {
                        decimal reduce_amount = 0;
                        #region 計算扣除金額
                        if (identify_standard.IdWay == "1")
                        {
                            #region 依百分比
                            decimal Denominator = identify_standard.GetDenominator(i);
                            decimal Molecular = identify_standard.GetMolecular(i);
                            if (Denominator == 0 || Molecular == 0)
                            {
                                reduce_amount = 0;
                            }
                            else
                            {
                                reduce_amount = receive_items_amount[i - 1] * (Molecular / Denominator);
                                reduce_amount = Round(reduce_amount, RoundType.RoundOff);
                            }
                            #endregion
                        }
                        else if (identify_standard.IdWay == "2")
                        {
                            #region 依金額
                            reduce_amount = identify_standard.GetReduceAmount(i);
                            #endregion
                        }
                        #endregion

                        #region 限額
                        decimal reduce_amount_limit = 0;
                        if (reduce_amount_limit > 0)
                        {
                            if (reduce_amount > reduce_amount_limit)
                            {
                                reduce_amount = reduce_amount_limit;
                            }
                        }
                        #endregion

                        if (String.IsNullOrEmpty(identify_standard.IdItem.Trim()))
                        {
                            #region 沒有減項科目就是直接扣除
                            receive_items_amount[i - 1] = receive_items_amount[i - 1] - reduce_amount;
                            if (receive_items_amount[i - 1] < 0)
                            {
                                receive_items_amount[i - 1] = 0;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 扣除到減項科目
                            if (receive_items_amount[i - 1] < reduce_amount)
                            {
                                reduce_amount = receive_items_amount[i - 1];
                            }
                            Int32 reduce_item = Convert.ToInt32(identify_standard.IdItem.Trim());
                            receive_items_amount[reduce_item - 1] = receive_items_amount[reduce_item - 1] - reduce_amount;
                            #endregion
                        }
                    }
                }
            }
            #endregion

            IdentifyStandard2Entity identify_standard2 = null;
            #region 身份註記2
            if (this.getIdentifyStandard2(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.IdentifyId02, out identify_standard2))
            {
                if (identify_standard2 != null)
                {
                    for (Int32 i = 1; i < 41; i++)
                    {
                        decimal reduce_amount = 0;
                        #region 計算扣除金額
                        if (identify_standard2.IdWay == "1")
                        {
                            #region 依百分比
                            decimal Denominator = identify_standard2.GetDenominator(i);
                            decimal Molecular = identify_standard2.GetMolecular(i);
                            if (Denominator == 0 || Molecular == 0)
                            {
                                reduce_amount = 0;
                            }
                            else
                            {
                                reduce_amount = receive_items_amount[i - 1] * (Molecular / Denominator);
                                reduce_amount = Round(reduce_amount, RoundType.RoundOff);
                            }
                            #endregion
                        }
                        else if (identify_standard2.IdWay == "2")
                        {
                            #region 依金額
                            reduce_amount = identify_standard2.GetReduceAmount(i);
                            #endregion
                        }
                        #endregion

                        #region 限額
                        decimal reduce_amount_limit = 0;
                        if (reduce_amount_limit > 0)
                        {
                            if (reduce_amount > reduce_amount_limit)
                            {
                                reduce_amount = reduce_amount_limit;
                            }
                        }
                        #endregion

                        if (String.IsNullOrEmpty(identify_standard2.IdItem.Trim()))
                        {
                            #region 沒有減項科目就是直接扣除
                            receive_items_amount[i - 1] = receive_items_amount[i - 1] - reduce_amount;
                            if (receive_items_amount[i - 1] < 0)
                            {
                                receive_items_amount[i - 1] = 0;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 扣除到減項科目
                            if (receive_items_amount[i - 1] < reduce_amount)
                            {
                                reduce_amount = receive_items_amount[i - 1];
                            }
                            Int32 reduce_item = Convert.ToInt32(identify_standard2.IdItem.Trim());
                            receive_items_amount[reduce_item - 1] = receive_items_amount[reduce_item - 1] - reduce_amount;
                            #endregion
                        }
                    }
                }
            }
            #endregion

            IdentifyStandard3Entity identify_standard3 = null;
            #region 身份註記3
            if (this.getIdentifyStandard3(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.IdentifyId03, out identify_standard3))
            {
                if (identify_standard3 != null)
                {
                    for (Int32 i = 1; i < 41; i++)
                    {
                        decimal reduce_amount = 0;
                        #region 計算扣除金額
                        if (identify_standard3.IdWay == "1")
                        {
                            #region 依百分比
                            decimal Denominator = identify_standard3.GetDenominator(i);
                            decimal Molecular = identify_standard3.GetMolecular(i);
                            if (Denominator == 0 || Molecular == 0)
                            {
                                reduce_amount = 0;
                            }
                            else
                            {
                                reduce_amount = receive_items_amount[i - 1] * (Molecular / Denominator);
                                reduce_amount = Round(reduce_amount, RoundType.RoundOff);
                            }
                            #endregion
                        }
                        else if (identify_standard3.IdWay == "2")
                        {
                            #region 依金額
                            reduce_amount = identify_standard3.GetReduceAmount(i);
                            #endregion
                        }
                        #endregion

                        #region 限額
                        decimal reduce_amount_limit = 0;
                        if (reduce_amount_limit > 0)
                        {
                            if (reduce_amount > reduce_amount_limit)
                            {
                                reduce_amount = reduce_amount_limit;
                            }
                        }
                        #endregion

                        if (String.IsNullOrEmpty(identify_standard3.IdItem.Trim()))
                        {
                            #region 沒有減項科目就是直接扣除
                            receive_items_amount[i - 1] = receive_items_amount[i - 1] - reduce_amount;
                            if (receive_items_amount[i - 1] < 0)
                            {
                                receive_items_amount[i - 1] = 0;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 扣除到減項科目
                            if (receive_items_amount[i - 1] < reduce_amount)
                            {
                                reduce_amount = receive_items_amount[i - 1];
                            }
                            Int32 reduce_item = Convert.ToInt32(identify_standard3.IdItem.Trim());
                            receive_items_amount[reduce_item - 1] = receive_items_amount[reduce_item - 1] - reduce_amount;
                            #endregion
                        }
                    }
                }
            }
            #endregion

            IdentifyStandard4Entity identify_standard4 = null;
            #region 身份註記4
            if (this.getIdentifyStandard4(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.IdentifyId04, out identify_standard4))
            {
                if (identify_standard4 != null)
                {
                    for (Int32 i = 1; i < 41; i++)
                    {
                        decimal reduce_amount = 0;
                        #region 計算扣除金額
                        if (identify_standard4.IdWay == "1")
                        {
                            #region 依百分比
                            decimal Denominator = identify_standard4.GetDenominator(i);
                            decimal Molecular = identify_standard4.GetMolecular(i);
                            if (Denominator == 0 || Molecular == 0)
                            {
                                reduce_amount = 0;
                            }
                            else
                            {
                                reduce_amount = receive_items_amount[i - 1] * (Molecular / Denominator);
                                reduce_amount = Round(reduce_amount, RoundType.RoundOff);
                            }
                            #endregion
                        }
                        else if (identify_standard4.IdWay == "2")
                        {
                            #region 依金額
                            reduce_amount = identify_standard4.GetReduceAmount(i);
                            #endregion
                        }
                        #endregion

                        #region 限額
                        decimal reduce_amount_limit = 0;
                        if (reduce_amount_limit > 0)
                        {
                            if (reduce_amount > reduce_amount_limit)
                            {
                                reduce_amount = reduce_amount_limit;
                            }
                        }
                        #endregion

                        if (String.IsNullOrEmpty(identify_standard4.IdItem.Trim()))
                        {
                            #region 沒有減項科目就是直接扣除
                            receive_items_amount[i - 1] = receive_items_amount[i - 1] - reduce_amount;
                            if (receive_items_amount[i - 1] < 0)
                            {
                                receive_items_amount[i - 1] = 0;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 扣除到減項科目
                            if (receive_items_amount[i - 1] < reduce_amount)
                            {
                                reduce_amount = receive_items_amount[i - 1];
                            }
                            Int32 reduce_item = Convert.ToInt32(identify_standard4.IdItem.Trim());
                            receive_items_amount[reduce_item - 1] = receive_items_amount[reduce_item - 1] - reduce_amount;
                            #endregion
                        }
                    }
                }
            }
            #endregion

            IdentifyStandard5Entity identify_standard5 = null;
            #region 身份註記5
            if (this.getIdentifyStandard5(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.IdentifyId05, out identify_standard5))
            {
                if (identify_standard5 != null)
                {
                    for (Int32 i = 1; i < 41; i++)
                    {
                        decimal reduce_amount = 0;
                        #region 計算扣除金額
                        if (identify_standard5.IdWay == "1")
                        {
                            #region 依百分比
                            decimal Denominator = identify_standard5.GetDenominator(i);
                            decimal Molecular = identify_standard5.GetMolecular(i);
                            if (Denominator == 0 || Molecular == 0)
                            {
                                reduce_amount = 0;
                            }
                            else
                            {
                                reduce_amount = receive_items_amount[i - 1] * (Molecular / Denominator);
                                reduce_amount = Round(reduce_amount, RoundType.RoundOff);
                            }
                            #endregion
                        }
                        else if (identify_standard5.IdWay == "2")
                        {
                            #region 依金額
                            reduce_amount = identify_standard5.GetReduceAmount(i);
                            #endregion
                        }
                        #endregion

                        #region 限額
                        decimal reduce_amount_limit = 0;
                        if (reduce_amount_limit > 0)
                        {
                            if (reduce_amount > reduce_amount_limit)
                            {
                                reduce_amount = reduce_amount_limit;
                            }
                        }
                        #endregion

                        if (String.IsNullOrEmpty(identify_standard5.IdItem.Trim()))
                        {
                            #region 沒有減項科目就是直接扣除
                            receive_items_amount[i - 1] = receive_items_amount[i - 1] - reduce_amount;
                            if (receive_items_amount[i - 1] < 0)
                            {
                                receive_items_amount[i - 1] = 0;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 扣除到減項科目
                            if (receive_items_amount[i - 1] < reduce_amount)
                            {
                                reduce_amount = receive_items_amount[i - 1];
                            }
                            Int32 reduce_item = Convert.ToInt32(identify_standard5.IdItem.Trim());
                            receive_items_amount[reduce_item - 1] = receive_items_amount[reduce_item - 1] - reduce_amount;
                            #endregion
                        }
                    }
                }
            }
            #endregion

            IdentifyStandard6Entity identify_standard6 = null;
            #region 身份註記6
            if (this.getIdentifyStandard6(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.IdentifyId06, out identify_standard6))
            {
                if (identify_standard6 != null)
                {
                    for (Int32 i = 1; i < 41; i++)
                    {
                        decimal reduce_amount = 0;
                        #region 計算扣除金額
                        if (identify_standard6.IdWay == "1")
                        {
                            #region 依百分比
                            decimal Denominator = identify_standard6.GetDenominator(i);
                            decimal Molecular = identify_standard6.GetMolecular(i);
                            if (Denominator == 0 || Molecular == 0)
                            {
                                reduce_amount = 0;
                            }
                            else
                            {
                                reduce_amount = receive_items_amount[i - 1] * (Molecular / Denominator);
                                reduce_amount = Round(reduce_amount, RoundType.RoundOff);
                            }
                            #endregion
                        }
                        else if (identify_standard6.IdWay == "2")
                        {
                            #region 依金額
                            reduce_amount = identify_standard6.GetReduceAmount(i);
                            #endregion
                        }
                        #endregion

                        #region 限額
                        decimal reduce_amount_limit = 0;
                        if (reduce_amount_limit > 0)
                        {
                            if (reduce_amount > reduce_amount_limit)
                            {
                                reduce_amount = reduce_amount_limit;
                            }
                        }
                        #endregion

                        if (String.IsNullOrEmpty(identify_standard6.IdItem.Trim()))
                        {
                            #region 沒有減項科目就是直接扣除
                            receive_items_amount[i - 1] = receive_items_amount[i - 1] - reduce_amount;
                            if (receive_items_amount[i - 1] < 0)
                            {
                                receive_items_amount[i - 1] = 0;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 扣除到減項科目
                            if (receive_items_amount[i - 1] < reduce_amount)
                            {
                                reduce_amount = receive_items_amount[i - 1];
                            }
                            Int32 reduce_item = Convert.ToInt32(identify_standard6.IdItem.Trim());
                            receive_items_amount[reduce_item - 1] = receive_items_amount[reduce_item - 1] - reduce_amount;
                            #endregion
                        }
                    }
                }
            }
            #endregion
            #endregion

            #region 減免就帶先後

            #endregion

            #region 計算可貸總額

            #endregion

            #region 計算小於學分基準或小時數

            #endregion

            #region 減免
            if (this.getReduceStandard(student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.ReduceId, out reduce_standard))
            {
                if (reduce_standard != null)
                {
                    if (reduce_standard.ReduceWay == "1" || reduce_standard.ReduceWay == "2")
                    {
                        for (Int32 i = 1; i < 41; i++)
                        {
                            #region 逐一收入科目
                            decimal reduce_amount = 0;
                            if (reduce_standard.ReduceWay == "1")
                            {
                                #region 依百分比
                                decimal Denominator = reduce_standard.GetDenominator(i);
                                decimal Molecular = reduce_standard.GetMolecular(i);
                                if (Denominator == 0 || Molecular == 0)
                                {
                                    reduce_amount = 0;
                                }
                                else
                                {
                                    reduce_amount = receive_items_amount[i - 1] * (Molecular / Denominator);
                                    reduce_amount = Round(reduce_amount, RoundType.RoundOff);
                                }
                                #endregion
                            }
                            else
                            {
                                #region 依金額
                                reduce_amount = reduce_standard.GetReduceAmount(i);
                                #endregion
                            }

                            #region 限額
                            decimal reduce_amount_limit = reduce_standard.GetLimit(i);
                            if (reduce_amount_limit > 0)
                            {
                                if (reduce_amount > reduce_amount_limit)
                                {
                                    reduce_amount = reduce_amount_limit;
                                }
                            }
                            #endregion

                            if (String.IsNullOrEmpty(reduce_standard.ReduceItem.Trim()))
                            {
                                #region 沒有減免科目就是直接扣除
                                receive_items_amount[i - 1] = receive_items_amount[i - 1] - reduce_amount;
                                if (receive_items_amount[i - 1] < 0)
                                {
                                    receive_items_amount[i - 1] = 0;
                                }
                                #endregion
                            }
                            else
                            {
                                #region 扣除到減免科目
                                if (receive_items_amount[i - 1] < reduce_amount)
                                {
                                    reduce_amount = receive_items_amount[i - 1];
                                }
                                Int32 reduce_item = Convert.ToInt32(reduce_standard.ReduceItem.Trim());
                                receive_items_amount[reduce_item - 1] = receive_items_amount[reduce_item - 1] - reduce_amount;
                                #endregion
                            }
                            #endregion
                        }
                    }
                    else if (reduce_standard.ReduceWay == "3")
                    {
                        #region 依次減免
                        decimal reduce_total = reduce_standard.ReduceTotal.Value;
                        for (Int32 i = 1; i < 41; i++)
                        {
                            Int32 index = reduce_standard.GetReduceItemByOrder(i);
                            if (index != 0)
                            {
                                if (receive_items_amount[index - 1] >= reduce_total)
                                {
                                    receive_items_amount[index - 1] = receive_items_amount[index - 1] - reduce_total;
                                    reduce_total = 0;
                                }
                                else
                                {
                                    receive_items_amount[index - 1] = 0;
                                    reduce_total = reduce_total - receive_items_amount[index - 1];
                                }
                            }
                            if (reduce_total <= 0)
                            {
                                break;
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #region 就貸

            #endregion

            #region 可貸
            string[] loan_items = school_rid.GetAllLoanItems();
            decimal loan_amount = 0;
            for (Int32 i = 0; i < loan_items.Length; i++)
            {
                if (loan_items[i].Trim().ToUpper() == "Y")
                {
                    loan_amount += receive_items_amount[i];
                }
            }

            #region [MDY:20161215] 修正 school_rid.LoanFee 為 null 時會發生例外的問題
            if (school_rid.LoanFee != null)
            {
                loan_amount += school_rid.LoanFee.Value;
            }
            #endregion

            student_receive.LoanAmount = loan_amount;
            #endregion

            #region 教育部補助
            /*
            if (school_rid.Issubsidy01.ToUpper()=="Y")
            {
                receive_items_amount[0] = 0;
            }
            if (school_rid.Issubsidy02.ToUpper() == "Y")
            {
                receive_items_amount[1] = 0;
            }
            if (school_rid.Issubsidy03.ToUpper() == "Y")
            {
                receive_items_amount[2] = 0;
            }
            if (school_rid.Issubsidy04.ToUpper() == "Y")
            {
                receive_items_amount[3] = 0;
            }
            if (school_rid.Issubsidy05.ToUpper() == "Y")
            {
                receive_items_amount[4] = 0;
            }
            if (school_rid.Issubsidy06.ToUpper() == "Y")
            {
                receive_items_amount[5] = 0;
            }
            if (school_rid.Issubsidy07.ToUpper() == "Y")
            {
                receive_items_amount[6] = 0;
            }
            if (school_rid.Issubsidy08.ToUpper() == "Y")
            {
                receive_items_amount[07] = 0;
            }
            if (school_rid.Issubsidy09.ToUpper() == "Y")
            {
                receive_items_amount[8] = 0;
            }
            if (school_rid.Issubsidy10.ToUpper() == "Y")
            {
                receive_items_amount[9] = 0;
            }
            if (school_rid.Issubsidy11.ToUpper() == "Y")
            {
                receive_items_amount[10] = 0;
            }
            if (school_rid.Issubsidy12.ToUpper() == "Y")
            {
                receive_items_amount[11] = 0;
            }
            if (school_rid.Issubsidy13.ToUpper() == "Y")
            {
                receive_items_amount[12] = 0;
            }
            if (school_rid.Issubsidy14.ToUpper() == "Y")
            {
                receive_items_amount[13] = 0;
            }
            if (school_rid.Issubsidy15.ToUpper() == "Y")
            {
                receive_items_amount[14] = 0;
            }
            if (school_rid.Issubsidy16.ToUpper() == "Y")
            {
                receive_items_amount[15] = 0;
            }
            
            if (school_rid.IsSubsidy17.ToUpper() == "Y")
            {
                receive_items_amount[16] = 0;
            }
            if (school_rid.IsSubsidy18.ToUpper() == "Y")
            {
                receive_items_amount[17] = 0;
            }
            if (school_rid.IsSubsidy19.ToUpper() == "Y")
            {
                receive_items_amount[18] = 0;
            }
            if (school_rid.IsSubsidy20.ToUpper() == "Y")
            {
                receive_items_amount[19] = 0;
            }
            if (school_rid.IsSubsidy21.ToUpper() == "Y")
            {
                receive_items_amount[20] = 0;
            }
            if (school_rid.IsSubsidy22.ToUpper() == "Y")
            {
                receive_items_amount[21] = 0;
            }
            if (school_rid.IsSubsidy23.ToUpper() == "Y")
            {
                receive_items_amount[22] = 0;
            }
            if (school_rid.IsSubsidy24.ToUpper() == "Y")
            {
                receive_items_amount[23] = 0;
            }
            if (school_rid.IsSubsidy25.ToUpper() == "Y")
            {
                receive_items_amount[24] = 0;
            }
            if (school_rid.IsSubsidy26.ToUpper() == "Y")
            {
                receive_items_amount[25] = 0;
            }
            if (school_rid.IsSubsidy27.ToUpper() == "Y")
            {
                receive_items_amount[26] = 0;
            }
            if (school_rid.IsSubsidy28.ToUpper() == "Y")
            {
                receive_items_amount[27] = 0;
            }
            if (school_rid.IsSubsidy29.ToUpper() == "Y")
            {
                receive_items_amount[28] = 0;
            }
            if (school_rid.IsSubsidy30.ToUpper() == "Y")
            {
                receive_items_amount[29] = 0;
            }

            if (school_rid.IsSubsidy31.ToUpper() == "Y")
            {
                receive_items_amount[30] = 0;
            }
            if (school_rid.IsSubsidy32.ToUpper() == "Y")
            {
                receive_items_amount[31] = 0;
            }
            if (school_rid.IsSubsidy33.ToUpper() == "Y")
            {
                receive_items_amount[32] = 0;
            }
            if (school_rid.IsSubsidy34.ToUpper() == "Y")
            {
                receive_items_amount[33] = 0;
            }
            if (school_rid.IsSubsidy35.ToUpper() == "Y")
            {
                receive_items_amount[34] = 0;
            }
            if (school_rid.IsSubsidy36.ToUpper() == "Y")
            {
                receive_items_amount[35] = 0;
            }
            if (school_rid.IsSubsidy37.ToUpper() == "Y")
            {
                receive_items_amount[36] = 0;
            }
            if (school_rid.IsSubsidy38.ToUpper() == "Y")
            {
                receive_items_amount[37] = 0;
            }
            if (school_rid.IsSubsidy39.ToUpper() == "Y")
            {
                receive_items_amount[38] = 0;
            }
            if (school_rid.IsSubsidy40.ToUpper() == "Y")
            {
                receive_items_amount[39] = 0;
            }
            */
            #endregion

            #region 把計算好的收入科目金額，回傳到學生繳費資料
            Int32 count = this.countReceiveItems(school_rid);
            if (count > 0)
            {
                student_receive.Receive01 = receive_items_amount[0];
                count--;
            }
            else
            {
                student_receive.Receive01 = null;
            }

            if (count > 0)
            {
                student_receive.Receive02 = receive_items_amount[1];
                count--;
            }
            else
            {
                student_receive.Receive02 = null;
            }
            if (count > 0)
            {
                student_receive.Receive03 = receive_items_amount[2];
                count--;
            }
            else
            {
                student_receive.Receive03 = null;
            }
            if (count > 0)
            {
                student_receive.Receive04 = receive_items_amount[3];
                count--;
            }
            else
            {
                student_receive.Receive04 = null;
            }
            if (count > 0)
            {
                student_receive.Receive05 = receive_items_amount[4];
                count--;
            }
            else
            {
                student_receive.Receive05 = null;
            }
            if (count > 0)
            {
                student_receive.Receive06 = receive_items_amount[5];
                count--;
            }
            else
            {
                student_receive.Receive06 = null;
            }
            if (count > 0)
            {
                student_receive.Receive07 = receive_items_amount[6];
                count--;
            }
            else
            {
                student_receive.Receive07 = null;
            }
            if (count > 0)
            {
                student_receive.Receive08 = receive_items_amount[7];
                count--;
            }
            else
            {
                student_receive.Receive08 = null;
            }
            if (count > 0)
            {
                student_receive.Receive09 = receive_items_amount[8];
                count--;
            }
            else
            {
                student_receive.Receive09 = null;
            }
            if (count > 0)
            {
                student_receive.Receive10 = receive_items_amount[9];
                count--;
            }
            else
            {
                student_receive.Receive10 = null;
            }
            if (count > 0)
            {
                student_receive.Receive11 = receive_items_amount[10];
                count--;
            }
            else
            {
                student_receive.Receive11 = null;
            }
            if (count > 0)
            {
                student_receive.Receive12 = receive_items_amount[11];
                count--;
            }
            else
            {
                student_receive.Receive12 = null;
            }
            if (count > 0)
            {
                student_receive.Receive13 = receive_items_amount[12];
                count--;
            }
            else
            {
                student_receive.Receive13 = null;
            }
            if (count > 0)
            {
                student_receive.Receive14 = receive_items_amount[13];
                count--;
            }
            else
            {
                student_receive.Receive14 = null;
            }
            if (count > 0)
            {
                student_receive.Receive15 = receive_items_amount[14];
                count--;
            }
            else
            {
                student_receive.Receive15 = null;
            }
            if (count > 0)
            {
                student_receive.Receive16 = receive_items_amount[15];
                count--;
            }
            else
            {
                student_receive.Receive16 = null;
            }
            if (count > 0)
            {
                student_receive.Receive17 = receive_items_amount[16];
                count--;
            }
            else
            {
                student_receive.Receive17 = null;
            }
            if (count > 0)
            {
                student_receive.Receive18 = receive_items_amount[17];
                count--;
            }
            else
            {
                student_receive.Receive18 = null;
            }
            if (count > 0)
            {
                student_receive.Receive19 = receive_items_amount[18];
                count--;
            }
            else
            {
                student_receive.Receive19 = null;
            }
            if (count > 0)
            {
                student_receive.Receive20 = receive_items_amount[19];
                count--;
            }
            else
            {
                student_receive.Receive20 = null;
            }
            if (count > 0)
            {
                student_receive.Receive21 = receive_items_amount[20];
                count--;
            }
            else
            {
                student_receive.Receive21 = null;
            }
            if (count > 0)
            {
                student_receive.Receive22 = receive_items_amount[21];
                count--;
            }
            else
            {
                student_receive.Receive22 = null;
            }
            if (count > 0)
            {
                student_receive.Receive23 = receive_items_amount[22];
                count--;
            }
            else
            {
                student_receive.Receive23 = null;
            }
            if (count > 0)
            {
                student_receive.Receive24 = receive_items_amount[23];
                count--;
            }
            else
            {
                student_receive.Receive24 = null;
            }
            if (count > 0)
            {
                student_receive.Receive25 = receive_items_amount[24];
                count--;
            }
            else
            {
                student_receive.Receive25 = null;
            }
            if (count > 0)
            {
                student_receive.Receive26 = receive_items_amount[25];
                count--;
            }
            else
            {
                student_receive.Receive26 = null;
            }
            if (count > 0)
            {
                student_receive.Receive27 = receive_items_amount[26];
                count--;
            }
            else
            {
                student_receive.Receive27 = null;
            }
            if (count > 0)
            {
                student_receive.Receive28 = receive_items_amount[27];
                count--;
            }
            else
            {
                student_receive.Receive28 = null;
            }
            if (count > 0)
            {
                student_receive.Receive29 = receive_items_amount[28];
                count--;
            }
            else
            {
                student_receive.Receive29 = null;
            }
            if (count > 0)
            {
                student_receive.Receive30 = receive_items_amount[29];
                count--;
            }
            else
            {
                student_receive.Receive30 = null;
            }

            if (count > 0)
            {
                student_receive.Receive31 = receive_items_amount[30];
                count--;
            }
            else
            {
                student_receive.Receive31 = null;
            }
            if (count > 0)
            {
                student_receive.Receive32 = receive_items_amount[31];
                count--;
            }
            else
            {
                student_receive.Receive32 = null;
            }
            if (count > 0)
            {
                student_receive.Receive33 = receive_items_amount[32];
                count--;
            }
            else
            {
                student_receive.Receive33 = null;
            }
            if (count > 0)
            {
                student_receive.Receive34 = receive_items_amount[33];
                count--;
            }
            else
            {
                student_receive.Receive34 = null;
            }
            if (count > 0)
            {
                student_receive.Receive35 = receive_items_amount[34];
                count--;
            }
            else
            {
                student_receive.Receive35 = null;
            }
            if (count > 0)
            {
                student_receive.Receive36 = receive_items_amount[35];
                count--;
            }
            else
            {
                student_receive.Receive36 = null;
            }
            if (count > 0)
            {
                student_receive.Receive37 = receive_items_amount[36];
                count--;
            }
            else
            {
                student_receive.Receive37 = null;
            }
            if (count > 0)
            {
                student_receive.Receive38 = receive_items_amount[37];
                count--;
            }
            else
            {
                student_receive.Receive38 = null;
            }
            if (count > 0)
            {
                student_receive.Receive39 = receive_items_amount[38];
                count--;
            }
            else
            {
                student_receive.Receive39 = null;
            }
            if (count > 0)
            {
                student_receive.Receive40 = receive_items_amount[39];
                count--;
            }
            else
            {
                student_receive.Receive40 = null;
            }
            #endregion

            rc = true;
            return rc;
        }

        #endregion

        public BillAmountHelper()
        {

        }

        /// <summary>
        /// 金額欄位作取位處理
        /// </summary>
        /// <param name="amount">原始金額</param>
        /// <param name="round_type">取位方法。無條件捨去/無條件進位/四捨五入</param>
        /// <returns>取位後金額</returns>
        public Int32 Round(decimal amount ,RoundType round_type)
        {
            if (round_type == RoundType.RoundUp)
            {
                return Convert.ToInt32(Math.Ceiling(amount));
            }
            else if (round_type == RoundType.RoundDown)
            {
                return Convert.ToInt32(Math.Floor(amount));
            }
            else
            {
                return Convert.ToInt32(Math.Round((amount), 0, MidpointRounding.AwayFromZero));
            }
        }

        /// <summary>
        /// 檢查明細金額加總與總金額是否相符
        /// </summary>
        /// <param name="bills">學生繳費資料</param>
        /// <returns>成功/失敗。失敗可以查err_msg</returns>
        public bool CheckBillAmount(StudentReceiveEntity bill)
        {
            _err_msg = "";
            bool rc = false;

            if(bill!=null)
            {
                decimal total_amount = sumReceiveItemAmount(bill);
                decimal bill_amount = (decimal)bill.ReceiveAmount;
                if(total_amount==bill_amount )
                {
                    rc = true;
                }
                else
                {
                    _err_msg = string.Format("[CheckBillAmount] 金額不符，明細加總金額={0}，總金額={1}",total_amount,bill_amount);
                }
            }
            else
            {
                _err_msg = string.Format("[CheckBillAmount] 傳入的學生繳費資料為null");
            }
            return rc;
        }

        /// <summary>
        /// 單筆計算金額(不做更新，只回傳)
        /// </summary>
        /// <param name="bill">要計算的學生繳費資料，計算後會回填欄位</param>
        /// <param name="calculate_type">計算方式，依金額/依標準</param>
        /// <returns>成功或失敗，失敗可以查err_msg</returns>
        public bool CalcBillAmount(ref StudentReceiveEntity bill, CalculateType calculate_type)
        {
            _err_msg = "";
            bool rc = false;

            if(bill==null)
            {
                _err_msg = string.Format("[CalcBillAmount] 傳入的學生繳費資料為null");
                return rc;
            }

            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5}", bill.ReceiveType, bill.YearId, bill.TermId, bill.DepId, bill.ReceiveId, bill.StuId);

            //依標準計算先計算每個收入科目的金額
            if (calculate_type == CalculateType.byStandard)
            {
                if (!calcReceiveItemAmount(ref bill))
                {
                    _err_msg = _err_msg + "," + key;
                    return rc;
                }
                #region 明細先做四捨五入
                bill.Receive01 = bill.Receive01 == null ? 0 : Round((decimal)bill.Receive01, RoundType.RoundOff);
                bill.Receive02 = bill.Receive02 == null ? 0 : Round((decimal)bill.Receive02, RoundType.RoundOff);
                bill.Receive03 = bill.Receive03 == null ? 0 : Round((decimal)bill.Receive03, RoundType.RoundOff);
                bill.Receive04 = bill.Receive04 == null ? 0 : Round((decimal)bill.Receive04, RoundType.RoundOff);
                bill.Receive05 = bill.Receive05 == null ? 0 : Round((decimal)bill.Receive05, RoundType.RoundOff);
                bill.Receive06 = bill.Receive06 == null ? 0 : Round((decimal)bill.Receive06, RoundType.RoundOff);
                bill.Receive07 = bill.Receive07 == null ? 0 : Round((decimal)bill.Receive07, RoundType.RoundOff);
                bill.Receive08 = bill.Receive08 == null ? 0 : Round((decimal)bill.Receive08, RoundType.RoundOff);
                bill.Receive09 = bill.Receive09 == null ? 0 : Round((decimal)bill.Receive09, RoundType.RoundOff);
                bill.Receive10 = bill.Receive10 == null ? 0 : Round((decimal)bill.Receive10, RoundType.RoundOff);
                bill.Receive11 = bill.Receive11 == null ? 0 : Round((decimal)bill.Receive11, RoundType.RoundOff);
                bill.Receive12 = bill.Receive12 == null ? 0 : Round((decimal)bill.Receive12, RoundType.RoundOff);
                bill.Receive13 = bill.Receive13 == null ? 0 : Round((decimal)bill.Receive13, RoundType.RoundOff);
                bill.Receive14 = bill.Receive14 == null ? 0 : Round((decimal)bill.Receive14, RoundType.RoundOff);
                bill.Receive15 = bill.Receive15 == null ? 0 : Round((decimal)bill.Receive15, RoundType.RoundOff);
                bill.Receive16 = bill.Receive16 == null ? 0 : Round((decimal)bill.Receive16, RoundType.RoundOff);
                bill.Receive17 = bill.Receive17 == null ? 0 : Round((decimal)bill.Receive17, RoundType.RoundOff);
                bill.Receive18 = bill.Receive18 == null ? 0 : Round((decimal)bill.Receive18, RoundType.RoundOff);
                bill.Receive19 = bill.Receive19 == null ? 0 : Round((decimal)bill.Receive19, RoundType.RoundOff);
                bill.Receive20 = bill.Receive20 == null ? 0 : Round((decimal)bill.Receive20, RoundType.RoundOff);
                bill.Receive21 = bill.Receive21 == null ? 0 : Round((decimal)bill.Receive21, RoundType.RoundOff);
                bill.Receive22 = bill.Receive22 == null ? 0 : Round((decimal)bill.Receive22, RoundType.RoundOff);
                bill.Receive23 = bill.Receive23 == null ? 0 : Round((decimal)bill.Receive23, RoundType.RoundOff);
                bill.Receive24 = bill.Receive24 == null ? 0 : Round((decimal)bill.Receive24, RoundType.RoundOff);
                bill.Receive25 = bill.Receive25 == null ? 0 : Round((decimal)bill.Receive25, RoundType.RoundOff);
                bill.Receive26 = bill.Receive26 == null ? 0 : Round((decimal)bill.Receive26, RoundType.RoundOff);
                bill.Receive27 = bill.Receive27 == null ? 0 : Round((decimal)bill.Receive27, RoundType.RoundOff);
                bill.Receive28 = bill.Receive28 == null ? 0 : Round((decimal)bill.Receive28, RoundType.RoundOff);
                bill.Receive29 = bill.Receive29 == null ? 0 : Round((decimal)bill.Receive29, RoundType.RoundOff);
                bill.Receive30 = bill.Receive30 == null ? 0 : Round((decimal)bill.Receive30, RoundType.RoundOff);

                bill.Receive31 = bill.Receive31 == null ? 0 : Round((decimal)bill.Receive31, RoundType.RoundOff);
                bill.Receive32 = bill.Receive32 == null ? 0 : Round((decimal)bill.Receive32, RoundType.RoundOff);
                bill.Receive33 = bill.Receive33 == null ? 0 : Round((decimal)bill.Receive33, RoundType.RoundOff);
                bill.Receive34 = bill.Receive34 == null ? 0 : Round((decimal)bill.Receive34, RoundType.RoundOff);
                bill.Receive35 = bill.Receive35 == null ? 0 : Round((decimal)bill.Receive35, RoundType.RoundOff);
                bill.Receive36 = bill.Receive36 == null ? 0 : Round((decimal)bill.Receive36, RoundType.RoundOff);
                bill.Receive37 = bill.Receive37 == null ? 0 : Round((decimal)bill.Receive37, RoundType.RoundOff);
                bill.Receive38 = bill.Receive38 == null ? 0 : Round((decimal)bill.Receive38, RoundType.RoundOff);
                bill.Receive39 = bill.Receive39 == null ? 0 : Round((decimal)bill.Receive39, RoundType.RoundOff);
                bill.Receive40 = bill.Receive40 == null ? 0 : Round((decimal)bill.Receive40, RoundType.RoundOff);
                #endregion
            }

            #region 教育部補助 20150906
            SchoolRidEntity school_rid=new SchoolRidEntity();
            if(getSchoolRid(bill.ReceiveType,bill.YearId,bill.TermId,bill.DepId,bill.ReceiveId,out school_rid))
            {
                #region 有選教育部補助就把該收入科目金額設為0
                if (school_rid.Issubsidy01.ToUpper()=="Y")
                {
                    bill.Receive01 = 0;
                }
                if (school_rid.Issubsidy02.ToUpper() == "Y")
                {
                    bill.Receive02 = 0;
                }
                if (school_rid.Issubsidy03.ToUpper() == "Y")
                {
                    bill.Receive03 = 0;
                }
                if (school_rid.Issubsidy04.ToUpper() == "Y")
                {
                    bill.Receive04 = 0;
                }
                if (school_rid.Issubsidy05.ToUpper() == "Y")
                {
                    bill.Receive05 = 0;
                }
                if (school_rid.Issubsidy06.ToUpper() == "Y")
                {
                    bill.Receive06 = 0;
                }
                if (school_rid.Issubsidy07.ToUpper() == "Y")
                {
                    bill.Receive07 = 0;
                }
                if (school_rid.Issubsidy08.ToUpper() == "Y")
                {
                    bill.Receive08 = 0;
                }
                if (school_rid.Issubsidy09.ToUpper() == "Y")
                {
                    bill.Receive09 = 0;
                }
                if (school_rid.Issubsidy10.ToUpper() == "Y")
                {
                    bill.Receive10 = 0;
                }
                if (school_rid.Issubsidy11.ToUpper() == "Y")
                {
                    bill.Receive11 = 0;
                }
                if (school_rid.Issubsidy12.ToUpper() == "Y")
                {
                    bill.Receive12 = 0;
                }
                if (school_rid.Issubsidy13.ToUpper() == "Y")
                {
                    bill.Receive13 = 0;
                }
                if (school_rid.Issubsidy14.ToUpper() == "Y")
                {
                    bill.Receive14 = 0;
                }
                if (school_rid.Issubsidy15.ToUpper() == "Y")
                {
                    bill.Receive15 = 0;
                }
                if (school_rid.Issubsidy16.ToUpper() == "Y")
                {
                    bill.Receive16 = 0;
                }
                if (school_rid.Issubsidy17.ToUpper() == "Y")
                {
                    bill.Receive17 = 0;
                }
                if (school_rid.Issubsidy18.ToUpper() == "Y")
                {
                    bill.Receive18 = 0;
                }
                if (school_rid.Issubsidy19.ToUpper() == "Y")
                {
                    bill.Receive19 = 0;
                }
                if (school_rid.Issubsidy20.ToUpper() == "Y")
                {
                    bill.Receive20 = 0;
                }
                if (school_rid.Issubsidy21.ToUpper() == "Y")
                {
                    bill.Receive21 = 0;
                }
                if (school_rid.Issubsidy22.ToUpper() == "Y")
                {
                    bill.Receive22 = 0;
                }
                if (school_rid.Issubsidy23.ToUpper() == "Y")
                {
                    bill.Receive23 = 0;
                }
                if (school_rid.Issubsidy24.ToUpper() == "Y")
                {
                    bill.Receive24 = 0;
                }
                if (school_rid.Issubsidy25.ToUpper() == "Y")
                {
                    bill.Receive25 = 0;
                }
                if (school_rid.Issubsidy26.ToUpper() == "Y")
                {
                    bill.Receive26 = 0;
                }
                if (school_rid.Issubsidy27.ToUpper() == "Y")
                {
                    bill.Receive27 = 0;
                }
                if (school_rid.Issubsidy28.ToUpper() == "Y")
                {
                    bill.Receive28 = 0;
                }
                if (school_rid.Issubsidy29.ToUpper() == "Y")
                {
                    bill.Receive29 = 0;
                }
                if (school_rid.Issubsidy30.ToUpper() == "Y")
                {
                    bill.Receive30 = 0;
                }
                if (school_rid.Issubsidy31.ToUpper() == "Y")
                {
                    bill.Receive31 = 0;
                }
                if (school_rid.Issubsidy32.ToUpper() == "Y")
                {
                    bill.Receive32 = 0;
                }
                if (school_rid.Issubsidy33.ToUpper() == "Y")
                {
                    bill.Receive33 = 0;
                }
                if (school_rid.Issubsidy34.ToUpper() == "Y")
                {
                    bill.Receive34 = 0;
                }
                if (school_rid.Issubsidy35.ToUpper() == "Y")
                {
                    bill.Receive35 = 0;
                }
                if (school_rid.Issubsidy36.ToUpper() == "Y")
                {
                    bill.Receive36 = 0;
                }
                if (school_rid.Issubsidy37.ToUpper() == "Y")
                {
                    bill.Receive37 = 0;
                }
                if (school_rid.Issubsidy38.ToUpper() == "Y")
                {
                    bill.Receive38 = 0;
                }
                if (school_rid.Issubsidy39.ToUpper() == "Y")
                {
                    bill.Receive39 = 0;
                }
                if (school_rid.Issubsidy40.ToUpper() == "Y")
                {
                    bill.Receive40 = 0;
                }
                #endregion
            }
            #endregion

            //再計算總金額
            decimal total_amount = this.sumReceiveItemAmount(bill);
            bill.ReceiveAmount = total_amount;

            //再去計算超商金額及臨櫃金額
            #region 土銀的的收續費，內含就是企業負擔，外加就是繳款人負擔，所以各管道的金額都一樣，不用算只要判斷是否有超商與臨櫃管道
            #region [Old]
            //ChannelHelper helper = new ChannelHelper();

            //decimal sm_amount = 0;
            //string sm_code = "";
            //decimal po_amount = 0;
            //decimal cash_amount = 0;
            //if(!helper.GetChannelFee(bill.ReceiveType, (decimal)bill.ReceiveAmount, out sm_amount, out sm_code, out po_amount, out cash_amount, out log))
            //{
            //    _err_msg = log + "," + key;
            //    return rc;
            //}

            //bill.ReceiveSMAmount = sm_amount;
            //bill.ReceiveATMAmount = cash_amount;
            #endregion

            bool hasSMChannel = false;
            bool hasCashChannel = false;
            ChannelHelper helper = new ChannelHelper();
            string errmsg = helper.CheckReceiveChannel(bill.ReceiveType, out hasSMChannel, out hasCashChannel);
            if (String.IsNullOrEmpty(errmsg))
            {
                bill.ReceiveSmamount = hasSMChannel ? bill.ReceiveAmount : 0;
                bill.ReceiveAtmamount = hasCashChannel ? bill.ReceiveAmount : 0;
            }
            else
            {
                _err_msg = errmsg + "," + key;
                return rc;
            }
            #endregion

            rc = true;
            return rc;
        }

        /// <summary>
        /// 單筆計算金額(不做更新，只回傳)
        /// </summary>
        /// <param name="bill">要計算的學生繳費資料，計算後會回填欄位</param>
        /// <param name="calculate_type">計算方式，依金額/依標準<</param>
        /// <param name="hasCashChannel">是否有臨櫃管道</param>
        /// <param name="hasSMChannel">是否有超商管道</param>
        /// <returns></returns>
        public bool CalcBillAmount(ref StudentReceiveEntity bill, CalculateType calculate_type, bool hasCashChannel, bool hasSMChannel)
        {
            _err_msg = "";
            bool rc = false;

            //string log = "";

            if (bill == null)
            {
                _err_msg = string.Format("[CalcBillAmount] 傳入的學生繳費資料為null");
                return rc;
            }

            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5},old_seq={6}", bill.ReceiveType, bill.YearId, bill.TermId, bill.DepId, bill.ReceiveId, bill.StuId, bill.OldSeq);

            //依標準計算先計算每個收入科目的金額
            if (calculate_type == CalculateType.byStandard)
            {
                if (!calcReceiveItemAmount(ref bill))
                {
                    _err_msg = _err_msg + "," + key;
                    return rc;
                }
                #region 明細先做四捨五入
                bill.Receive01 = bill.Receive01 == null ? 0 : Round((decimal)bill.Receive01, RoundType.RoundOff);
                bill.Receive02 = bill.Receive02 == null ? 0 : Round((decimal)bill.Receive02, RoundType.RoundOff);
                bill.Receive03 = bill.Receive03 == null ? 0 : Round((decimal)bill.Receive03, RoundType.RoundOff);
                bill.Receive04 = bill.Receive04 == null ? 0 : Round((decimal)bill.Receive04, RoundType.RoundOff);
                bill.Receive05 = bill.Receive05 == null ? 0 : Round((decimal)bill.Receive05, RoundType.RoundOff);
                bill.Receive06 = bill.Receive06 == null ? 0 : Round((decimal)bill.Receive06, RoundType.RoundOff);
                bill.Receive07 = bill.Receive07 == null ? 0 : Round((decimal)bill.Receive07, RoundType.RoundOff);
                bill.Receive08 = bill.Receive08 == null ? 0 : Round((decimal)bill.Receive08, RoundType.RoundOff);
                bill.Receive09 = bill.Receive09 == null ? 0 : Round((decimal)bill.Receive09, RoundType.RoundOff);
                bill.Receive10 = bill.Receive10 == null ? 0 : Round((decimal)bill.Receive10, RoundType.RoundOff);
                bill.Receive11 = bill.Receive11 == null ? 0 : Round((decimal)bill.Receive11, RoundType.RoundOff);
                bill.Receive12 = bill.Receive12 == null ? 0 : Round((decimal)bill.Receive12, RoundType.RoundOff);
                bill.Receive13 = bill.Receive13 == null ? 0 : Round((decimal)bill.Receive13, RoundType.RoundOff);
                bill.Receive14 = bill.Receive14 == null ? 0 : Round((decimal)bill.Receive14, RoundType.RoundOff);
                bill.Receive15 = bill.Receive15 == null ? 0 : Round((decimal)bill.Receive15, RoundType.RoundOff);
                bill.Receive16 = bill.Receive16 == null ? 0 : Round((decimal)bill.Receive16, RoundType.RoundOff);
                bill.Receive17 = bill.Receive17 == null ? 0 : Round((decimal)bill.Receive17, RoundType.RoundOff);
                bill.Receive18 = bill.Receive18 == null ? 0 : Round((decimal)bill.Receive18, RoundType.RoundOff);
                bill.Receive19 = bill.Receive19 == null ? 0 : Round((decimal)bill.Receive19, RoundType.RoundOff);
                bill.Receive20 = bill.Receive20 == null ? 0 : Round((decimal)bill.Receive20, RoundType.RoundOff);
                bill.Receive21 = bill.Receive21 == null ? 0 : Round((decimal)bill.Receive21, RoundType.RoundOff);
                bill.Receive22 = bill.Receive22 == null ? 0 : Round((decimal)bill.Receive22, RoundType.RoundOff);
                bill.Receive23 = bill.Receive23 == null ? 0 : Round((decimal)bill.Receive23, RoundType.RoundOff);
                bill.Receive24 = bill.Receive24 == null ? 0 : Round((decimal)bill.Receive24, RoundType.RoundOff);
                bill.Receive25 = bill.Receive25 == null ? 0 : Round((decimal)bill.Receive25, RoundType.RoundOff);
                bill.Receive26 = bill.Receive26 == null ? 0 : Round((decimal)bill.Receive26, RoundType.RoundOff);
                bill.Receive27 = bill.Receive27 == null ? 0 : Round((decimal)bill.Receive27, RoundType.RoundOff);
                bill.Receive28 = bill.Receive28 == null ? 0 : Round((decimal)bill.Receive28, RoundType.RoundOff);
                bill.Receive29 = bill.Receive29 == null ? 0 : Round((decimal)bill.Receive29, RoundType.RoundOff);
                bill.Receive30 = bill.Receive30 == null ? 0 : Round((decimal)bill.Receive30, RoundType.RoundOff);

                bill.Receive31 = bill.Receive21 == null ? 0 : Round((decimal)bill.Receive31, RoundType.RoundOff);
                bill.Receive32 = bill.Receive32 == null ? 0 : Round((decimal)bill.Receive32, RoundType.RoundOff);
                bill.Receive33 = bill.Receive33 == null ? 0 : Round((decimal)bill.Receive33, RoundType.RoundOff);
                bill.Receive34 = bill.Receive34 == null ? 0 : Round((decimal)bill.Receive34, RoundType.RoundOff);
                bill.Receive35 = bill.Receive35 == null ? 0 : Round((decimal)bill.Receive35, RoundType.RoundOff);
                bill.Receive36 = bill.Receive36 == null ? 0 : Round((decimal)bill.Receive36, RoundType.RoundOff);
                bill.Receive37 = bill.Receive37 == null ? 0 : Round((decimal)bill.Receive37, RoundType.RoundOff);
                bill.Receive38 = bill.Receive38 == null ? 0 : Round((decimal)bill.Receive38, RoundType.RoundOff);
                bill.Receive39 = bill.Receive39 == null ? 0 : Round((decimal)bill.Receive39, RoundType.RoundOff);
                bill.Receive40 = bill.Receive40 == null ? 0 : Round((decimal)bill.Receive40, RoundType.RoundOff);
                #endregion
            }

            #region 教育部補助 20150906
            SchoolRidEntity school_rid = new SchoolRidEntity();
            if (getSchoolRid(bill.ReceiveType, bill.YearId, bill.TermId, bill.DepId, bill.ReceiveId, out school_rid))
            {
                #region 有選教育部補助就把該收入科目金額設為0
                if (school_rid.Issubsidy01.ToUpper() == "Y")
                {
                    bill.Receive01 = 0;
                }
                if (school_rid.Issubsidy02.ToUpper() == "Y")
                {
                    bill.Receive02 = 0;
                }
                if (school_rid.Issubsidy03.ToUpper() == "Y")
                {
                    bill.Receive03 = 0;
                }
                if (school_rid.Issubsidy04.ToUpper() == "Y")
                {
                    bill.Receive04 = 0;
                }
                if (school_rid.Issubsidy05.ToUpper() == "Y")
                {
                    bill.Receive05 = 0;
                }
                if (school_rid.Issubsidy06.ToUpper() == "Y")
                {
                    bill.Receive06 = 0;
                }
                if (school_rid.Issubsidy07.ToUpper() == "Y")
                {
                    bill.Receive07 = 0;
                }
                if (school_rid.Issubsidy08.ToUpper() == "Y")
                {
                    bill.Receive08 = 0;
                }
                if (school_rid.Issubsidy09.ToUpper() == "Y")
                {
                    bill.Receive09 = 0;
                }
                if (school_rid.Issubsidy10.ToUpper() == "Y")
                {
                    bill.Receive10 = 0;
                }
                if (school_rid.Issubsidy11.ToUpper() == "Y")
                {
                    bill.Receive11 = 0;
                }
                if (school_rid.Issubsidy12.ToUpper() == "Y")
                {
                    bill.Receive12 = 0;
                }
                if (school_rid.Issubsidy13.ToUpper() == "Y")
                {
                    bill.Receive13 = 0;
                }
                if (school_rid.Issubsidy14.ToUpper() == "Y")
                {
                    bill.Receive14 = 0;
                }
                if (school_rid.Issubsidy15.ToUpper() == "Y")
                {
                    bill.Receive15 = 0;
                }
                if (school_rid.Issubsidy16.ToUpper() == "Y")
                {
                    bill.Receive16 = 0;
                }
                if (school_rid.Issubsidy17.ToUpper() == "Y")
                {
                    bill.Receive17 = 0;
                }
                if (school_rid.Issubsidy18.ToUpper() == "Y")
                {
                    bill.Receive18 = 0;
                }
                if (school_rid.Issubsidy19.ToUpper() == "Y")
                {
                    bill.Receive19 = 0;
                }
                if (school_rid.Issubsidy20.ToUpper() == "Y")
                {
                    bill.Receive20 = 0;
                }
                if (school_rid.Issubsidy21.ToUpper() == "Y")
                {
                    bill.Receive21 = 0;
                }
                if (school_rid.Issubsidy22.ToUpper() == "Y")
                {
                    bill.Receive22 = 0;
                }
                if (school_rid.Issubsidy23.ToUpper() == "Y")
                {
                    bill.Receive23 = 0;
                }
                if (school_rid.Issubsidy24.ToUpper() == "Y")
                {
                    bill.Receive24 = 0;
                }
                if (school_rid.Issubsidy25.ToUpper() == "Y")
                {
                    bill.Receive25 = 0;
                }
                if (school_rid.Issubsidy26.ToUpper() == "Y")
                {
                    bill.Receive26 = 0;
                }
                if (school_rid.Issubsidy27.ToUpper() == "Y")
                {
                    bill.Receive27 = 0;
                }
                if (school_rid.Issubsidy28.ToUpper() == "Y")
                {
                    bill.Receive28 = 0;
                }
                if (school_rid.Issubsidy29.ToUpper() == "Y")
                {
                    bill.Receive29 = 0;
                }
                if (school_rid.Issubsidy30.ToUpper() == "Y")
                {
                    bill.Receive30 = 0;
                }
                if (school_rid.Issubsidy31.ToUpper() == "Y")
                {
                    bill.Receive31 = 0;
                }
                if (school_rid.Issubsidy32.ToUpper() == "Y")
                {
                    bill.Receive32 = 0;
                }
                if (school_rid.Issubsidy33.ToUpper() == "Y")
                {
                    bill.Receive33 = 0;
                }
                if (school_rid.Issubsidy34.ToUpper() == "Y")
                {
                    bill.Receive34 = 0;
                }
                if (school_rid.Issubsidy35.ToUpper() == "Y")
                {
                    bill.Receive35 = 0;
                }
                if (school_rid.Issubsidy36.ToUpper() == "Y")
                {
                    bill.Receive36 = 0;
                }
                if (school_rid.Issubsidy37.ToUpper() == "Y")
                {
                    bill.Receive37 = 0;
                }
                if (school_rid.Issubsidy38.ToUpper() == "Y")
                {
                    bill.Receive38 = 0;
                }
                if (school_rid.Issubsidy39.ToUpper() == "Y")
                {
                    bill.Receive39 = 0;
                }
                if (school_rid.Issubsidy40.ToUpper() == "Y")
                {
                    bill.Receive40 = 0;
                }
                #endregion
            }
            #endregion

            //再計算總金額
            decimal total_amount = this.sumReceiveItemAmount(bill);
            bill.ReceiveAmount = total_amount;

            //再去計算超商金額及臨櫃金額
            #region 土銀的的收續費，內含就是企業負擔，外加就是繳款人負擔，所以各管道的金額都一樣，不用算只要判斷是否有超商與臨櫃管道
            #region [Old]
            //ChannelHelper helper = new ChannelHelper();

            //decimal sm_amount = 0;
            //string sm_code = "";
            //decimal po_amount = 0;
            //decimal cash_amount = 0;
            //if(!helper.GetChannelFee(bill.ReceiveType, (decimal)bill.ReceiveAmount, out sm_amount, out sm_code, out po_amount, out cash_amount, out log))
            //{
            //    _err_msg = log + "," + key;
            //    return rc;
            //}

            //bill.ReceiveSMAmount = sm_amount;
            //bill.ReceiveATMAmount = cash_amount;
            #endregion

            bill.ReceiveSmamount = hasSMChannel ? bill.ReceiveAmount : 0;
            bill.ReceiveAtmamount = hasCashChannel ? bill.ReceiveAmount : 0;
            #endregion

            rc = true;
            return rc;
        }

        /// <summary>
        /// 單筆計算金額(不做更新，只回傳)
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="calculate_type"></param>
        /// <param name="schoolRid"></param>
        /// <param name="hasCashChannel"></param>
        /// <param name="hasSMChannel"></param>
        /// <returns></returns>
        public bool CalcBillAmount(ref StudentReceiveEntity bill, CalculateType calculate_type, SchoolRidEntity schoolRid, bool hasCashChannel, bool hasSMChannel)
        {
            _err_msg = "";
            bool rc = false;

            if (bill == null)
            {
                _err_msg = string.Format("[CalcBillAmount] 傳入的學生繳費資料為null");
                return rc;
            }
            if (schoolRid == null)
            {
                _err_msg = string.Format("[CalcBillAmount] 傳入的費用別設定資料為null");
                return rc;
            }

            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5},old_seq={6}", bill.ReceiveType, bill.YearId, bill.TermId, bill.DepId, bill.ReceiveId, bill.StuId, bill.OldSeq);

            //依標準計算先計算每個收入科目的金額
            if (calculate_type == CalculateType.byStandard)
            {
                if (!calcReceiveItemAmount(ref bill))
                {
                    _err_msg = _err_msg + "," + key;
                    return rc;
                }
                #region 明細先做四捨五入
                bill.Receive01 = bill.Receive01 == null ? 0 : Round((decimal)bill.Receive01, RoundType.RoundOff);
                bill.Receive02 = bill.Receive02 == null ? 0 : Round((decimal)bill.Receive02, RoundType.RoundOff);
                bill.Receive03 = bill.Receive03 == null ? 0 : Round((decimal)bill.Receive03, RoundType.RoundOff);
                bill.Receive04 = bill.Receive04 == null ? 0 : Round((decimal)bill.Receive04, RoundType.RoundOff);
                bill.Receive05 = bill.Receive05 == null ? 0 : Round((decimal)bill.Receive05, RoundType.RoundOff);
                bill.Receive06 = bill.Receive06 == null ? 0 : Round((decimal)bill.Receive06, RoundType.RoundOff);
                bill.Receive07 = bill.Receive07 == null ? 0 : Round((decimal)bill.Receive07, RoundType.RoundOff);
                bill.Receive08 = bill.Receive08 == null ? 0 : Round((decimal)bill.Receive08, RoundType.RoundOff);
                bill.Receive09 = bill.Receive09 == null ? 0 : Round((decimal)bill.Receive09, RoundType.RoundOff);
                bill.Receive10 = bill.Receive10 == null ? 0 : Round((decimal)bill.Receive10, RoundType.RoundOff);
                bill.Receive11 = bill.Receive11 == null ? 0 : Round((decimal)bill.Receive11, RoundType.RoundOff);
                bill.Receive12 = bill.Receive12 == null ? 0 : Round((decimal)bill.Receive12, RoundType.RoundOff);
                bill.Receive13 = bill.Receive13 == null ? 0 : Round((decimal)bill.Receive13, RoundType.RoundOff);
                bill.Receive14 = bill.Receive14 == null ? 0 : Round((decimal)bill.Receive14, RoundType.RoundOff);
                bill.Receive15 = bill.Receive15 == null ? 0 : Round((decimal)bill.Receive15, RoundType.RoundOff);
                bill.Receive16 = bill.Receive16 == null ? 0 : Round((decimal)bill.Receive16, RoundType.RoundOff);
                bill.Receive17 = bill.Receive17 == null ? 0 : Round((decimal)bill.Receive17, RoundType.RoundOff);
                bill.Receive18 = bill.Receive18 == null ? 0 : Round((decimal)bill.Receive18, RoundType.RoundOff);
                bill.Receive19 = bill.Receive19 == null ? 0 : Round((decimal)bill.Receive19, RoundType.RoundOff);
                bill.Receive20 = bill.Receive20 == null ? 0 : Round((decimal)bill.Receive20, RoundType.RoundOff);
                bill.Receive21 = bill.Receive21 == null ? 0 : Round((decimal)bill.Receive21, RoundType.RoundOff);
                bill.Receive22 = bill.Receive22 == null ? 0 : Round((decimal)bill.Receive22, RoundType.RoundOff);
                bill.Receive23 = bill.Receive23 == null ? 0 : Round((decimal)bill.Receive23, RoundType.RoundOff);
                bill.Receive24 = bill.Receive24 == null ? 0 : Round((decimal)bill.Receive24, RoundType.RoundOff);
                bill.Receive25 = bill.Receive25 == null ? 0 : Round((decimal)bill.Receive25, RoundType.RoundOff);
                bill.Receive26 = bill.Receive26 == null ? 0 : Round((decimal)bill.Receive26, RoundType.RoundOff);
                bill.Receive27 = bill.Receive27 == null ? 0 : Round((decimal)bill.Receive27, RoundType.RoundOff);
                bill.Receive28 = bill.Receive28 == null ? 0 : Round((decimal)bill.Receive28, RoundType.RoundOff);
                bill.Receive29 = bill.Receive29 == null ? 0 : Round((decimal)bill.Receive29, RoundType.RoundOff);
                bill.Receive30 = bill.Receive30 == null ? 0 : Round((decimal)bill.Receive30, RoundType.RoundOff);

                bill.Receive31 = bill.Receive21 == null ? 0 : Round((decimal)bill.Receive31, RoundType.RoundOff);
                bill.Receive32 = bill.Receive32 == null ? 0 : Round((decimal)bill.Receive32, RoundType.RoundOff);
                bill.Receive33 = bill.Receive33 == null ? 0 : Round((decimal)bill.Receive33, RoundType.RoundOff);
                bill.Receive34 = bill.Receive34 == null ? 0 : Round((decimal)bill.Receive34, RoundType.RoundOff);
                bill.Receive35 = bill.Receive35 == null ? 0 : Round((decimal)bill.Receive35, RoundType.RoundOff);
                bill.Receive36 = bill.Receive36 == null ? 0 : Round((decimal)bill.Receive36, RoundType.RoundOff);
                bill.Receive37 = bill.Receive37 == null ? 0 : Round((decimal)bill.Receive37, RoundType.RoundOff);
                bill.Receive38 = bill.Receive38 == null ? 0 : Round((decimal)bill.Receive38, RoundType.RoundOff);
                bill.Receive39 = bill.Receive39 == null ? 0 : Round((decimal)bill.Receive39, RoundType.RoundOff);
                bill.Receive40 = bill.Receive40 == null ? 0 : Round((decimal)bill.Receive40, RoundType.RoundOff);
                #endregion
            }

            #region 教育部補助 20150906
            {
                #region 有選教育部補助就把該收入科目金額設為0
                if (schoolRid.Issubsidy01.ToUpper() == "Y")
                {
                    bill.Receive01 = 0;
                }
                if (schoolRid.Issubsidy02.ToUpper() == "Y")
                {
                    bill.Receive02 = 0;
                }
                if (schoolRid.Issubsidy03.ToUpper() == "Y")
                {
                    bill.Receive03 = 0;
                }
                if (schoolRid.Issubsidy04.ToUpper() == "Y")
                {
                    bill.Receive04 = 0;
                }
                if (schoolRid.Issubsidy05.ToUpper() == "Y")
                {
                    bill.Receive05 = 0;
                }
                if (schoolRid.Issubsidy06.ToUpper() == "Y")
                {
                    bill.Receive06 = 0;
                }
                if (schoolRid.Issubsidy07.ToUpper() == "Y")
                {
                    bill.Receive07 = 0;
                }
                if (schoolRid.Issubsidy08.ToUpper() == "Y")
                {
                    bill.Receive08 = 0;
                }
                if (schoolRid.Issubsidy09.ToUpper() == "Y")
                {
                    bill.Receive09 = 0;
                }
                if (schoolRid.Issubsidy10.ToUpper() == "Y")
                {
                    bill.Receive10 = 0;
                }
                if (schoolRid.Issubsidy11.ToUpper() == "Y")
                {
                    bill.Receive11 = 0;
                }
                if (schoolRid.Issubsidy12.ToUpper() == "Y")
                {
                    bill.Receive12 = 0;
                }
                if (schoolRid.Issubsidy13.ToUpper() == "Y")
                {
                    bill.Receive13 = 0;
                }
                if (schoolRid.Issubsidy14.ToUpper() == "Y")
                {
                    bill.Receive14 = 0;
                }
                if (schoolRid.Issubsidy15.ToUpper() == "Y")
                {
                    bill.Receive15 = 0;
                }
                if (schoolRid.Issubsidy16.ToUpper() == "Y")
                {
                    bill.Receive16 = 0;
                }
                if (schoolRid.Issubsidy17.ToUpper() == "Y")
                {
                    bill.Receive17 = 0;
                }
                if (schoolRid.Issubsidy18.ToUpper() == "Y")
                {
                    bill.Receive18 = 0;
                }
                if (schoolRid.Issubsidy19.ToUpper() == "Y")
                {
                    bill.Receive19 = 0;
                }
                if (schoolRid.Issubsidy20.ToUpper() == "Y")
                {
                    bill.Receive20 = 0;
                }
                if (schoolRid.Issubsidy21.ToUpper() == "Y")
                {
                    bill.Receive21 = 0;
                }
                if (schoolRid.Issubsidy22.ToUpper() == "Y")
                {
                    bill.Receive22 = 0;
                }
                if (schoolRid.Issubsidy23.ToUpper() == "Y")
                {
                    bill.Receive23 = 0;
                }
                if (schoolRid.Issubsidy24.ToUpper() == "Y")
                {
                    bill.Receive24 = 0;
                }
                if (schoolRid.Issubsidy25.ToUpper() == "Y")
                {
                    bill.Receive25 = 0;
                }
                if (schoolRid.Issubsidy26.ToUpper() == "Y")
                {
                    bill.Receive26 = 0;
                }
                if (schoolRid.Issubsidy27.ToUpper() == "Y")
                {
                    bill.Receive27 = 0;
                }
                if (schoolRid.Issubsidy28.ToUpper() == "Y")
                {
                    bill.Receive28 = 0;
                }
                if (schoolRid.Issubsidy29.ToUpper() == "Y")
                {
                    bill.Receive29 = 0;
                }
                if (schoolRid.Issubsidy30.ToUpper() == "Y")
                {
                    bill.Receive30 = 0;
                }
                if (schoolRid.Issubsidy31.ToUpper() == "Y")
                {
                    bill.Receive31 = 0;
                }
                if (schoolRid.Issubsidy32.ToUpper() == "Y")
                {
                    bill.Receive32 = 0;
                }
                if (schoolRid.Issubsidy33.ToUpper() == "Y")
                {
                    bill.Receive33 = 0;
                }
                if (schoolRid.Issubsidy34.ToUpper() == "Y")
                {
                    bill.Receive34 = 0;
                }
                if (schoolRid.Issubsidy35.ToUpper() == "Y")
                {
                    bill.Receive35 = 0;
                }
                if (schoolRid.Issubsidy36.ToUpper() == "Y")
                {
                    bill.Receive36 = 0;
                }
                if (schoolRid.Issubsidy37.ToUpper() == "Y")
                {
                    bill.Receive37 = 0;
                }
                if (schoolRid.Issubsidy38.ToUpper() == "Y")
                {
                    bill.Receive38 = 0;
                }
                if (schoolRid.Issubsidy39.ToUpper() == "Y")
                {
                    bill.Receive39 = 0;
                }
                if (schoolRid.Issubsidy40.ToUpper() == "Y")
                {
                    bill.Receive40 = 0;
                }
                #endregion
            }
            #endregion

            //再計算總金額
            decimal total_amount = this.sumReceiveItemAmount(bill);
            bill.ReceiveAmount = total_amount;

            //再去計算超商金額及臨櫃金額
            #region 土銀的的收續費，內含就是企業負擔，外加就是繳款人負擔，所以各管道的金額都一樣，不用算只要判斷是否有超商與臨櫃管道
            #region [Old]
            //ChannelHelper helper = new ChannelHelper();

            //decimal sm_amount = 0;
            //string sm_code = "";
            //decimal po_amount = 0;
            //decimal cash_amount = 0;
            //if(!helper.GetChannelFee(bill.ReceiveType, (decimal)bill.ReceiveAmount, out sm_amount, out sm_code, out po_amount, out cash_amount, out log))
            //{
            //    _err_msg = log + "," + key;
            //    return rc;
            //}

            //bill.ReceiveSMAmount = sm_amount;
            //bill.ReceiveATMAmount = cash_amount;
            #endregion

            bill.ReceiveSmamount = hasSMChannel ? bill.ReceiveAmount : 0;
            bill.ReceiveAtmamount = hasCashChannel ? bill.ReceiveAmount : 0;
            #endregion

            rc = true;
            return rc;
        }

        /// <summary>
        /// 單筆計算金額(會做更新)
        /// </summary>
        /// <param name="bill">要計算的學生繳費資料，計算後會回填欄位</param>
        /// <param name="calculate_type">計算方式，依金額/依標準</param>
        /// <returns>成功或失敗，失敗可以查err_msg</returns>
        public bool CalcBillAmount1(StudentReceiveEntity student_receive, CalculateType calculate_type)
        {
            _err_msg = "";
            bool rc = false;

            string log = "";

            if (student_receive == null)
            {
                _err_msg = string.Format("[CalcBillAmount] 傳入的學生繳費資料為null");
                return rc;
            }

            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5},old_seq={6}", student_receive.ReceiveType, student_receive.YearId, student_receive.TermId, student_receive.DepId, student_receive.ReceiveId, student_receive.StuId, student_receive.OldSeq);

            StudentReceiveEntity bill = new StudentReceiveEntity();
            bill = student_receive;
            //依標準計算先計算每個收入科目的金額
            if (calculate_type == CalculateType.byStandard)
            {
                if (!calcReceiveItemAmount(ref bill))
                {
                    _err_msg = _err_msg + "," + key;
                    return rc;
                }


                //
                #region 明細先做四捨五入
                bill.Receive01 = bill.Receive01 == null ? 0 : Round((decimal)bill.Receive01, RoundType.RoundOff);
                bill.Receive02 = bill.Receive02 == null ? 0 : Round((decimal)bill.Receive02, RoundType.RoundOff);
                bill.Receive03 = bill.Receive03 == null ? 0 : Round((decimal)bill.Receive03, RoundType.RoundOff);
                bill.Receive04 = bill.Receive04 == null ? 0 : Round((decimal)bill.Receive04, RoundType.RoundOff);
                bill.Receive05 = bill.Receive05 == null ? 0 : Round((decimal)bill.Receive05, RoundType.RoundOff);
                bill.Receive06 = bill.Receive06 == null ? 0 : Round((decimal)bill.Receive06, RoundType.RoundOff);
                bill.Receive07 = bill.Receive07 == null ? 0 : Round((decimal)bill.Receive07, RoundType.RoundOff);
                bill.Receive08 = bill.Receive08 == null ? 0 : Round((decimal)bill.Receive08, RoundType.RoundOff);
                bill.Receive09 = bill.Receive09 == null ? 0 : Round((decimal)bill.Receive09, RoundType.RoundOff);
                bill.Receive10 = bill.Receive10 == null ? 0 : Round((decimal)bill.Receive10, RoundType.RoundOff);
                bill.Receive11 = bill.Receive11 == null ? 0 : Round((decimal)bill.Receive11, RoundType.RoundOff);
                bill.Receive12 = bill.Receive12 == null ? 0 : Round((decimal)bill.Receive12, RoundType.RoundOff);
                bill.Receive13 = bill.Receive13 == null ? 0 : Round((decimal)bill.Receive13, RoundType.RoundOff);
                bill.Receive14 = bill.Receive14 == null ? 0 : Round((decimal)bill.Receive14, RoundType.RoundOff);
                bill.Receive15 = bill.Receive15 == null ? 0 : Round((decimal)bill.Receive15, RoundType.RoundOff);
                bill.Receive16 = bill.Receive16 == null ? 0 : Round((decimal)bill.Receive16, RoundType.RoundOff);
                bill.Receive17 = bill.Receive17 == null ? 0 : Round((decimal)bill.Receive17, RoundType.RoundOff);
                bill.Receive18 = bill.Receive18 == null ? 0 : Round((decimal)bill.Receive18, RoundType.RoundOff);
                bill.Receive19 = bill.Receive19 == null ? 0 : Round((decimal)bill.Receive19, RoundType.RoundOff);
                bill.Receive20 = bill.Receive20 == null ? 0 : Round((decimal)bill.Receive20, RoundType.RoundOff);
                bill.Receive21 = bill.Receive21 == null ? 0 : Round((decimal)bill.Receive21, RoundType.RoundOff);
                bill.Receive22 = bill.Receive22 == null ? 0 : Round((decimal)bill.Receive22, RoundType.RoundOff);
                bill.Receive23 = bill.Receive23 == null ? 0 : Round((decimal)bill.Receive23, RoundType.RoundOff);
                bill.Receive24 = bill.Receive24 == null ? 0 : Round((decimal)bill.Receive24, RoundType.RoundOff);
                bill.Receive25 = bill.Receive25 == null ? 0 : Round((decimal)bill.Receive25, RoundType.RoundOff);
                bill.Receive26 = bill.Receive26 == null ? 0 : Round((decimal)bill.Receive26, RoundType.RoundOff);
                bill.Receive27 = bill.Receive27 == null ? 0 : Round((decimal)bill.Receive27, RoundType.RoundOff);
                bill.Receive28 = bill.Receive28 == null ? 0 : Round((decimal)bill.Receive28, RoundType.RoundOff);
                bill.Receive29 = bill.Receive29 == null ? 0 : Round((decimal)bill.Receive29, RoundType.RoundOff);
                bill.Receive30 = bill.Receive30 == null ? 0 : Round((decimal)bill.Receive30, RoundType.RoundOff);

                bill.Receive31 = bill.Receive31 == null ? 0 : Round((decimal)bill.Receive31, RoundType.RoundOff);
                bill.Receive32 = bill.Receive32 == null ? 0 : Round((decimal)bill.Receive32, RoundType.RoundOff);
                bill.Receive33 = bill.Receive33 == null ? 0 : Round((decimal)bill.Receive33, RoundType.RoundOff);
                bill.Receive34 = bill.Receive34 == null ? 0 : Round((decimal)bill.Receive34, RoundType.RoundOff);
                bill.Receive35 = bill.Receive35 == null ? 0 : Round((decimal)bill.Receive35, RoundType.RoundOff);
                bill.Receive36 = bill.Receive36 == null ? 0 : Round((decimal)bill.Receive36, RoundType.RoundOff);
                bill.Receive37 = bill.Receive37 == null ? 0 : Round((decimal)bill.Receive37, RoundType.RoundOff);
                bill.Receive38 = bill.Receive38 == null ? 0 : Round((decimal)bill.Receive38, RoundType.RoundOff);
                bill.Receive39 = bill.Receive39 == null ? 0 : Round((decimal)bill.Receive39, RoundType.RoundOff);
                bill.Receive40 = bill.Receive40 == null ? 0 : Round((decimal)bill.Receive40, RoundType.RoundOff);
                #endregion
            }


            //再計算總金額
            decimal total_amount = this.sumReceiveItemAmount(bill);
            bill.ReceiveAmount = total_amount;

            //再去計算超商金額及臨櫃金額
            #region 土銀的的收續費，內含就是企業負擔，外加就是繳款人負擔，所以各管道的金額都一樣，不用算只要判斷是否有超商與臨櫃管道
            #region [Old]
            //ChannelHelper helper = new ChannelHelper();

            //decimal sm_amount = 0;
            //string sm_code = "";
            //decimal po_amount = 0;
            //decimal cash_amount = 0;
            //if (!helper.GetChannelFee(bill.ReceiveType, (decimal)bill.ReceiveAmount, out sm_amount, out sm_code, out po_amount, out cash_amount, out log))
            //{
            //    _err_msg = log + "," + key;
            //    return rc;
            //}

            //bill.ReceiveSMAmount = sm_amount;
            //bill.ReceiveATMAmount = cash_amount;
            #endregion

            bool hasSMChannel = false;
            bool hasCashChannel = false;
            ChannelHelper helper = new ChannelHelper();
            log = helper.CheckReceiveChannel(bill.ReceiveType, out hasSMChannel, out hasCashChannel);
            if (String.IsNullOrEmpty(_err_msg))
            {
                bill.ReceiveSmamount = hasSMChannel ? bill.ReceiveAmount : 0;
                bill.ReceiveAtmamount = hasCashChannel ? bill.ReceiveAmount : 0;
            }
            else
            {
                _err_msg = log + "," + key;
                return rc;
            }
            #endregion

            EntityFactory factory = new EntityFactory();
            int count=0;
            //TODO: Update ? UpdateFields ?
            Result result = factory.Update(bill, out count);
            if(result.IsSuccess)
            {
                rc = true;
            }
            else
            {
                _err_msg = string.Format("更新student_receive失敗，錯誤訊息={0},key={1}", result.Message, key);
                rc = false;
            }
            return rc;
        }

        /// <summary>
        /// 整批計算金額
        /// </summary>
        /// <param name="bills">要計算的學生繳費資料，計算後會回填欄位</param>
        /// <param name="calculate_type">計算方式，依金額/依標準</param>
        /// <returns>成功或失敗，失敗可以查err_msg</returns>
        public bool CalcBillsAmount(ref StudentReceiveEntity[] bills, CalculateType calculate_type)
        {
            _err_msg = "";
            bool rc = false;
            //string log="";
            StringBuilder logs = new StringBuilder();

            Int32 success = 0;
            Int32 fail = 0;
            Int32 count = bills.Length;

            if(bills==null || bills.Length<=0)
            {
                _err_msg = string.Format("[CalcBillsAmount] 傳入的學生繳費資料為null");
                return rc;
            }

            DateTime now = DateTime.Now;
            for (Int32 i = 0; i < count; i++)
            {
                //log = "";
                StudentReceiveEntity bill = bills[i];
                Decimal? orgReceiveAmount = bill.ReceiveAmount;
                if(!CalcBillAmount(ref bill,calculate_type))
                {
                    fail++;
                    logs.AppendLine(_err_msg);
                }
                else
                {
                    #region 應繳金額有異動則中信資料發送旗標清為 0，並更新 UpdateDate
                    if (orgReceiveAmount != bill.ReceiveAmount)
                    {
                        bill.CFlag = "0";

                        bill.UpdateDate = now;
                    }
                    #endregion

                    success++;
                }
            }

            logs.AppendLine(string.Format("總筆數={0},成功筆數={1},失敗筆數{2}", count, success, fail));
            _err_msg = logs.ToString().Trim();
            
            rc = true;
            return rc;
        }
    }

    #region [NEW:20151229] 合計項目小計資料承載類別
    /// <summary>
    /// 合計項目小計資料承載類別
    /// </summary>
    public sealed class SubTotalAmount
    {
        #region Property
        private string _Id = null;
        /// <summary>
        /// 小計代碼 (存放 ReceiveSumEntity.SumId)
        /// </summary>
        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value == null ? null : value.Trim().Replace(" ", "");
            }
        }

        private string _Name = null;
        /// <summary>
        /// 小計名稱 (存放 ReceiveSumEntity.SumName)
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value == null ? null : value.Trim();
            }
        }

        private decimal? _Amount = null;
        /// <summary>
        /// 小計金額
        /// </summary>
        public decimal? Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }
        #endregion

        #region Constructor
        public SubTotalAmount()
        {

        }

        private SubTotalAmount(string id, string name, decimal? amount)
        {
            this.Id = id;
            this.Name = name;
            this.Amount = amount;
        }
        #endregion

        #region Method
        /// <summary>
        /// 計算合計
        /// </summary>
        /// <param name="studentReceive"></param>
        /// <param name="receiveSum"></param>
        /// <param name="skipNullAmount"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public decimal? Calc(StudentReceiveEntity studentReceive, ReceiveSumEntity receiveSum, bool skipNullAmount, out string errmsg)
        {
            #region 檢查參數
            if (studentReceive == null)
            {
                errmsg = "無學生繳費單資料";
                return null;
            }
            int[] sumItemNos = receiveSum == null ? null : receiveSum.GetSumReceiveItemNos();
            if (sumItemNos == null || sumItemNos.Length == 0)
            {
                errmsg = "無合計項目設定資料";
                return null;
            }

            decimal?[] itemAmounts = studentReceive.GetAllReceiveItemAmounts();
            if (sumItemNos.Length > itemAmounts.Length)
            {
                errmsg = "合計項目超過收入科目數量";
                return null;
            }
            #endregion

            #region 計算項目合計
            errmsg = null;
            decimal sumAmount = 0;
            foreach (int itemNo in sumItemNos)
            {
                decimal? itemAmount = itemAmounts[itemNo - 1];
                if (itemAmount != null)
                {
                    sumAmount += itemAmount.Value;
                }
                else if (!skipNullAmount)
                {
                    errmsg = String.Format("收入科目{0}無金額", itemNo);
                    return null;
                }
            }
            return sumAmount;
            #endregion
        }
        #endregion

        #region Static Method
        #region [MDY:202203XX] 2022擴充案 合計項目中文/英文名稱
        /// <summary>
        /// 依據建立
        /// </summary>
        /// <param name="studentReceive"></param>
        /// <param name="receiveSum"></param>
        /// <param name="skipNullAmount"></param>
        /// <param name="useEngDataUI"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static SubTotalAmount Create(StudentReceiveEntity studentReceive, ReceiveSumEntity receiveSum, bool skipNullAmount, bool useEngDataUI, out string errmsg)
        {
            errmsg = null;
            SubTotalAmount data = null;
            if (useEngDataUI && !String.IsNullOrEmpty(receiveSum.SumEName))
            {
                data = new SubTotalAmount(receiveSum.SumId, receiveSum.SumEName, null);
            }
            else
            {
                data = new SubTotalAmount(receiveSum.SumId, receiveSum.SumName, null);
            }
            data.Amount = data.Calc(studentReceive, receiveSum, skipNullAmount, out errmsg);
            return data;
        }
        #endregion
        #endregion
    }
    #endregion
}
