using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.IO;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 產生XML for PDF
    /// </summary>
    public class XmlHelper
    {
        #region [MDY:20160521] 增加是否精簡 Xml 產生屬性
        /// <summary>
        /// 是否精簡 Xml 產生 (true 則無值的節點不產生)
        /// </summary>
        public bool IsSimplify
        {
            get;
            set;
        }
        #endregion

        #region [MDY:20160521] 增加 Constructor
        /// <summary>
        /// 建構 產生XML for PDF 類別物件
        /// </summary>
        public XmlHelper()
        {

        }

        /// <summary>
        /// 建構 產生XML for PDF 類別物件
        /// </summary>
        /// <param name="isSimplify"></param>
        public XmlHelper(bool isSimplify)
        {
            this.IsSimplify = isSimplify;
        }
        #endregion

        #region [MDY:202203XX] 2022擴充案 是否英文介面
        /// <summary>
        /// 產生繳費單的 XML DATA
        /// </summary>
        /// <param name="xmlWriter"></param>
        /// <param name="PageNo">頁號</param>
        /// <param name="UserId">使用者ID</param>
        /// <param name="DepName">部別名稱</param>
        /// <param name="MajorName">科別名稱</param>
        /// <param name="GradeName">年級名稱</param>
        /// <param name="CollegeName">院別名稱</param>
        /// <param name="ClassName">班別名稱</param>
        /// <param name="ReduceName">減免名稱</param>
        /// <param name="DormName">住宿名稱</param>
        /// <param name="LoanName">就貸名稱</param>
        /// <param name="YearName">學年名稱</param>
        /// <param name="TermName">學期名稱</param>
        /// <param name="ReceiveName">費用別名稱</param>
        /// <param name="IdentifyName">身分註記名稱</param>
        /// <param name="BankName">主辦分行名稱</param>
        /// <param name="BankTel">主辦分行電話</param>
        /// <param name="IdentifyName01">身分註記名稱01</param>
        /// <param name="IdentifyName02">身分註記名稱02</param>
        /// <param name="IdentifyName03">身分註記名稱03</param>
        /// <param name="IdentifyName04">身分註記名稱04</param>
        /// <param name="IdentifyName05">身分註記名稱05</param>
        /// <param name="IdentifyName06">身分註記名稱06</param>
        /// <param name="SchoolRid">代收費用設定檔</param>
        /// <param name="StudentMaster">學生資料主檔</param>
        /// <param name="SchoolRType">代收類別資料檔</param>
        /// <param name="StudentReceive">學生繳費資料檔</param>
        /// <param name="smChannelWay">超商手續費資料</param>
        /// <param name="cashChannelWay">銀行(臨櫃)手續費資料</param>
        /// <param name="receiveSums">合計項目設定資料</param>
        /// <param name="qrcodeConfig">財金 QRCode 支付設定資料</param>
        /// <param name="fgMark">指定是否遮罩個資欄位</param>
        /// <param name="isEngUI">指定是否英文介面</param>
        /// <returns></returns>
        public bool GenXmlData(XmlWriter xmlWriter, string PageNo, string UserId
            , string DepName, string MajorName, string GradeName, string CollegeName
            , string ClassName, string ReduceName, string DormName, string LoanName
            , string YearName, string TermName, string ReceiveName
            , string IdentifyName, string BankName, string BankTel
            , string IdentifyName01, string IdentifyName02, string IdentifyName03, string IdentifyName04, string IdentifyName05, string IdentifyName06
            , SchoolRidEntity SchoolRid, StudentMasterEntity StudentMaster
            , SchoolRTypeEntity SchoolRType, StudentReceiveEntity StudentReceive
            , ReceiveChannelEntity smChannelWay, ReceiveChannelEntity cashChannelWay, ReceiveSumEntity[] receiveSums
            , FiscQRCodeConfig qrcodeConfig
            , bool fgMark, bool isEngUI)
        {
            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            bool useEngDataUI = isEngUI && SchoolRType.IsEngEnabled();
            #endregion

            // <Fields Page='1'>
            xmlWriter.WriteStartElement("Fields");
            xmlWriter.WriteAttributeString("Page", PageNo);

            // <Year>2014</Year>
            xmlWriter.WriteStartElement("Year");
            xmlWriter.WriteString(DateTime.Now.Year.ToString());
            xmlWriter.WriteEndElement();

            // <CYear>103</CYear>
            xmlWriter.WriteStartElement("CYear");
            xmlWriter.WriteString(Common.GetTWDate7(DateTime.Now).Substring(0, 3));
            xmlWriter.WriteEndElement();

            // <Month>05</Month>
            xmlWriter.WriteStartElement("Month");
            xmlWriter.WriteString(DateTime.Now.Month.ToString());
            xmlWriter.WriteEndElement();

            // <Day>15</Day>
            xmlWriter.WriteStartElement("Day");
            xmlWriter.WriteString(DateTime.Now.Day.ToString());
            xmlWriter.WriteEndElement();

            // <PrintDate>2014/05/15</PrintDate>
            xmlWriter.WriteStartElement("PrintDate");
            xmlWriter.WriteString(DateTime.Now.ToString("yyyy/MM/dd"));
            xmlWriter.WriteEndElement();

            //<UserID>BOT001</UserID>
            xmlWriter.WriteStartElement("UserID");
            xmlWriter.WriteString(UserId);
            xmlWriter.WriteEndElement();

            //開放列印日期
            DateTime? OpenDate = DataFormat.ConvertDateText(SchoolRid.BillValidDate);
            if (OpenDate != null)
            {
                xmlWriter.WriteElementString("OpenDate", String.Format("{0:yyyy/MM/dd}", OpenDate.Value));   //開放列印 (4碼西元年 yyyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
                xmlWriter.WriteElementString("COpenDate", String.Format("{0:000}/{1:MM/dd}", OpenDate.Value.Year - 1911, OpenDate.Value));   //開放列印 (3碼民國年 yyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)

                xmlWriter.WriteElementString("OpenDatey", String.Format("{0}", OpenDate.Value.Year));
                xmlWriter.WriteElementString("OpenDatecy", String.Format("{0:000}", OpenDate.Value.Year - 1911));
                xmlWriter.WriteElementString("OpenDatem", String.Format("{0:MM}", OpenDate.Value));
                xmlWriter.WriteElementString("OpenDated", String.Format("{0:dd}", OpenDate.Value));
            }
            //else
            //{
            //    xmlWriter.WriteElementString("OpenDate", SchoolRid.PayDate); //開放列印日期
            //    xmlWriter.WriteElementString("COpenDate", SchoolRid.PayDate);   //開放列印日期

            //    try
            //    {
            //        xmlWriter.WriteElementString("OpenDatey", String.Format("{0}", SchoolRid.PayDate.Substring(0, 4)));
            //        xmlWriter.WriteElementString("OpenDatecy", String.Format("{0:000}", Convert.ToInt32(SchoolRid.PayDate.Substring(0, 4)) - 1911));
            //        xmlWriter.WriteElementString("OpenDatem", String.Format("{0:MM}", SchoolRid.PayDate.Substring(4, 2)));
            //        xmlWriter.WriteElementString("OpenDated", String.Format("{0:dd}", SchoolRid.PayDate.Substring(6, 2)));
            //    }
            //    catch (Exception ex)
            //    {
            //        xmlWriter.WriteElementString("OpenDatey", "");
            //        xmlWriter.WriteElementString("OpenDatecy", "");
            //        xmlWriter.WriteElementString("OpenDatem", "");
            //        xmlWriter.WriteElementString("OpenDated", "");
            //    }
            //}

            KeyValueList<string> XmlElements = new KeyValueList<string>();

            #region [MDY:20160131] 因為 StudentReceive 增加繳款期限 (PayDueDate)，所以先取 StudentReceive.PayDueDate，無值採改取 SchoolRid.PayDate
            #region [Old]
            //繳款期限
            //DateTime? payDueDate = DataFormat.ConvertDateText(SchoolRid.PayDate);
            #endregion

            string txtPayDueDate = null;
            DateTime? payDueDate = null;
            if (String.IsNullOrWhiteSpace(StudentReceive.PayDueDate))
            {
                txtPayDueDate = SchoolRid.PayDate;
                payDueDate = DataFormat.ConvertDateText(SchoolRid.PayDate);
            }
            else
            {
                txtPayDueDate = StudentReceive.PayDueDate;
                payDueDate = DataFormat.ConvertDateText(StudentReceive.PayDueDate);
            }
            #endregion

            if (payDueDate != null)
            {
                XmlElements.Add("PayDueDate", String.Format("{0:yyyy/MM/dd}", payDueDate.Value));   //繳款期限 (4碼西元年 yyyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
                xmlWriter.WriteElementString("PayDueDate", String.Format("{0:yyyy/MM/dd}", payDueDate.Value));   //繳款期限 (4碼西元年 yyyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
                XmlElements.Add("PayDueDateY", String.Format("{0:yyyy}", payDueDate.Value));
                xmlWriter.WriteElementString("PayDueDateY", String.Format("{0:yyyy}", payDueDate.Value));
                XmlElements.Add("PayDueDateCY", String.Format("{0}", payDueDate.Value.Year - 1911));
                xmlWriter.WriteElementString("PayDueDateCY", String.Format("{0}", payDueDate.Value.Year - 1911));
                XmlElements.Add("PayDueDateM", String.Format("{0:MM}", payDueDate.Value));
                xmlWriter.WriteElementString("PayDueDateM", String.Format("{0:MM}", payDueDate.Value));
                XmlElements.Add("PayDueDateD", String.Format("{0:dd}", payDueDate.Value));
                xmlWriter.WriteElementString("PayDueDateD", String.Format("{0:dd}", payDueDate.Value));
                XmlElements.Add("CPayDueDate", String.Format("{0:000}/{1:MM/dd}", payDueDate.Value.Year - 1911, payDueDate.Value));
                xmlWriter.WriteElementString("CPayDueDate", String.Format("{0:000}/{1:MM/dd}", payDueDate.Value.Year - 1911, payDueDate.Value));   //繳款期限 (3碼民國年 yyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)

                XmlElements.Add("TaPayDueDate", String.Format("{0:yyyy年MM月dd日}", payDueDate.Value));    //中文繳款期限 (4碼西元年 yyyy + "年" + 2碼月 MM + "月" + 2碼日期 dd + "日")
                xmlWriter.WriteElementString("TaPayDueDate", String.Format("{0:yyyy年MM月dd日}", payDueDate.Value));   //中文繳款期限 (4碼西元年 yyyy + "年" + 2碼月 MM + "月" + 2碼日期 dd + "日")
                XmlElements.Add("TaCPayDueDate", String.Format("{0:000}年{1:MM月dd日}", payDueDate.Value.Year - 1911, payDueDate.Value));  //中文繳款期限 (3碼民國年 yyy + "年" + 2碼月 MM + "月" + 2碼日期 dd+ "日")
                xmlWriter.WriteElementString("TaCPayDueDate", String.Format("{0:000}年{1:MM月dd日}", payDueDate.Value.Year - 1911, payDueDate.Value));   //中文繳款期限 (3碼民國年 yyy + "年" + 2碼月 MM + "月" + 2碼日期 dd+ "日")
            }
            else
            {
                XmlElements.Add("PayDueDate", txtPayDueDate);               //繳款期限
                xmlWriter.WriteElementString("PayDueDate", txtPayDueDate);  //繳款期限
                XmlElements.Add("CPayDueDate", txtPayDueDate);                  //繳款期限
                xmlWriter.WriteElementString("CPayDueDate", txtPayDueDate);     //繳款期限
            }

            XmlElements.Add("RePrint", "");                           //補印註記(18號字)
            XmlElements.Add("SchoolZipCode", SchoolRType.SchPostal);  //學校郵遞區號(12號字)
            XmlElements.Add("SchoolAddress", SchoolRType.SchAddress); //學校地址(12號字)
            XmlElements.Add("StudentZipCode", StudentMaster.ZipCode); //學生郵遞區號(12號字)
            XmlElements.Add("SchoolTel1", SchoolRType.SchConTel);     //學校聯絡人1電話(12號字)
            XmlElements.Add("SchoolTel2", SchoolRType.SchConTel1);    //學校聯絡人2電話(12號字)

            #region [MDY:202203XX] 2022擴充案 學校中文、英文名稱
            XmlElements.Add("SchoolName", (useEngDataUI ? SchoolRType.SchEName : SchoolRType.SchName));       //學校名稱
            #endregion

            //xmlWriter.WriteElementString("Sch_Name", SchoolRid.PayDate);//學校名稱

            #region 學生身分證字號 & 學生姓名 & 學生地址 & 學生生日 & 家長名稱
            if (fgMark)
            {
                XmlElements.Add("id", DataFormat.MaskText(StudentMaster.IdNumber, DataFormat.MaskDataType.ID));         //學生身分證字號

                #region [MDY:20200705] 姓名改要遮罩
                #region 姓名遮罩
                XmlElements.Add("StudentName", DataFormat.MaskText(StudentMaster.Name, DataFormat.MaskDataType.Name));  //學生姓名
                #endregion

                #region [Old] 姓名不遮罩
                //XmlElements.Add("StudentName", StudentMaster.Name);  //學生姓名
                #endregion
                #endregion

                XmlElements.Add("StudentAddress", DataFormat.MaskText(StudentMaster.Address, DataFormat.MaskDataType.Address)); //學生地址(12號字)
                XmlElements.Add("birthday", DataFormat.MaskText(StudentMaster.Birthday, DataFormat.MaskDataType.Birthday));      //學生生日
                xmlWriter.WriteElementString("Stu_Birthday", DataFormat.MaskText(StudentMaster.Birthday, DataFormat.MaskDataType.Birthday));
                //XmlElements.Add("StuParent", StudentMaster.StuParent);  //家長名稱
                if (!String.IsNullOrWhiteSpace(StudentMaster.StuParent))
                {
                    XmlElements.Add("StuParent", StudentMaster.StuParent);  //家長名稱
                }
            }
            else
            {
                XmlElements.Add("id", StudentMaster.IdNumber);            //學生身分證字號
                XmlElements.Add("StudentName", StudentMaster.Name);       //學生姓名
                XmlElements.Add("StudentAddress", StudentMaster.Address); //學生地址(12號字)
                XmlElements.Add("birthday", StudentMaster.Birthday);      //學生生日
                xmlWriter.WriteElementString("Stu_Birthday", StudentMaster.Birthday);
                if (!String.IsNullOrWhiteSpace(StudentMaster.StuParent))
                {
                    XmlElements.Add("StuParent", StudentMaster.StuParent);  //家長名稱
                }
            }
            #endregion

            #region [Old] 臺銀用的
            //xmlWriter.WriteElementString("Id_Number", StudentMaster.IdNumber);//學生身分證字號
            //xmlWriter.WriteElementString("Stu_Name", StudentMaster.Name);//學生姓名
            #endregion

            XmlElements.Add("email", StudentMaster.Email);            //學生電子郵件

            #region [MDY:202203XX] 2022擴充案 注意事項中文、英文內容
            string[] briefs = SchoolRid.GetAllBrief(useEngDataUI);
            XmlElements.Add("brief1", briefs[0]);              //注意事項1
            XmlElements.Add("brief2", briefs[1]);              //注意事項2
            XmlElements.Add("brief3", briefs[2]);              //注意事項3
            XmlElements.Add("brief4", briefs[3]);              //注意事項4
            XmlElements.Add("brief5", briefs[4]);              //注意事項5
            XmlElements.Add("brief6", briefs[5]);              //注意事項6
            #endregion

            XmlElements.Add("CancelNo", StudentReceive.CancelNo);     //銷帳編號
            xmlWriter.WriteElementString("Cancel_No", StudentReceive.CancelNo);
            XmlElements.Add("SchoolWordSN", SchoolRid.SchWord);       //字軌
            xmlWriter.WriteElementString("SchoolWordSN", SchoolRid.SchWord);

            #region 學號
            {
                string stuId = StudentMaster.Id.Trim();
                XmlElements.Add("StudentID", stuId);
                xmlWriter.WriteElementString("Stu_Id", stuId);
                //學號條碼，條碼只能大寫英文或數字
                if (Common.IsEnglishNumber(stuId))
                {
                    GenBarcodeElement(xmlWriter, "StudentIDBarcode", stuId.ToUpper());
                }
            }
            #endregion

            #region [MDY:20160213] 新增資料序號
            XmlElements.Add("Old_Seq", StudentReceive.OldSeq.ToString());   //資料序號
            #endregion

            XmlElements.Add("DepName", DepName);                      //部別名稱

            XmlElements.Add("MajorId", StudentReceive.MajorId);         //科系代碼
            XmlElements.Add("MajorName", MajorName);                    //科系名稱
            xmlWriter.WriteElementString("Major_Name", MajorName);

            XmlElements.Add("GradeName", GradeName);                  //年級名稱
            xmlWriter.WriteElementString("Stu_Grade", GradeName);
            xmlWriter.WriteElementString("Grade_Name", GradeName);

            XmlElements.Add("CollegeId", StudentReceive.CollegeId);     //院別代碼
            XmlElements.Add("CollegeName", CollegeName);                //院別名稱
            xmlWriter.WriteElementString("College_Name", CollegeName);

            XmlElements.Add("ClassId", StudentReceive.ClassId);         //班級代碼
            XmlElements.Add("ClassName", ClassName);                    //班級名稱
            xmlWriter.WriteElementString("Class_Name", ClassName);

            XmlElements.Add("StudentHid", StudentReceive.StuHid);     //座號
            xmlWriter.WriteElementString("Stu_hid", StudentReceive.StuHid);

            #region [Old] 沒有收入科目名稱就不產生收入科目金額
            //#region 收入科目名稱 1 ~ 40
            //string idx_list = "";//為了判斷有收入科目名稱的才出現金額
            //Result rt = new Result();
            //string itemName = string.Empty;
            //string itemValue = string.Empty;
            //for (int i = 1; i < 41; i++)
            //{
            //    itemName = "ReceiveItem" + i.ToString("00");
            //    object ob = SchoolRid.GetValue(itemName, out rt);
            //    if (rt.IsSuccess && ob != null)
            //    {

            //        itemValue = ob.ToString();
            //        if(itemValue.Trim()!="")
            //        {
            //            idx_list += (idx_list == "" ? i.ToString() : "," + i.ToString());
            //        }
            //        XmlElements.Add("ReceiveItem" + i.ToString(), itemValue);
            //    }
            //}
            //#endregion

            //#region 收入科目金額 1 ~ 40
            //for (int i = 1; i < 41; i++)
            //{
            //    itemName = "Receive" + i.ToString("00");
            //    object ob = StudentReceive.GetValue(itemName, out rt);
            //    decimal value;
            //    if (rt.IsSuccess && ob != null && Decimal.TryParse(ob.ToString(), out value))
            //    {
            //        #region old
            //        //itemValue = value.ToString("###,###,###,##0");
            //        //XmlElements.Add("ReceiveAmount" + i.ToString(), itemValue);
            //        #endregion

            //        if (idx_list.IndexOf(i.ToString())>=0)
            //        {
            //            itemValue = value.ToString("###,###,###,##0");
            //            XmlElements.Add("ReceiveAmount" + i.ToString(), itemValue);
            //        }

            //    }
            //}
            //#endregion
            #endregion

            #region [MDY:202203XX] 2022擴充案 收入科目中文/英文名稱、金額 (40項)
            #region [OLD]
            //#region [New] 沒有收入科目名稱就不產生收入科目金額 (40項)
            //{
            //    //string idx_list = "";//為了判斷有收入科目名稱的才出現金額
            //    Result rt = new Result();
            //    //string itemName = string.Empty;
            //    //string itemValue = string.Empty;
            //    for (int no = 1; no <= 40; no++)
            //    {
            //        #region 收入科目名稱
            //        string receiveItemName = null;
            //        {
            //            string fieldName = String.Format("ReceiveItem{0:00}", no);
            //            object fieldValue = SchoolRid.GetValue(fieldName, out rt);
            //            if (rt.IsSuccess && fieldValue != null)
            //            {
            //                receiveItemName = fieldValue.ToString();
            //                XmlElements.Add(String.Format("ReceiveItem{0}", no), receiveItemName);
            //            }
            //        }
            //        #endregion

            //        #region 收入科目金額
            //        if (!String.IsNullOrWhiteSpace(receiveItemName))
            //        {
            //            string fieldName = String.Format("Receive{0:00}", no);
            //            object fieldValue = StudentReceive.GetValue(fieldName, out rt);
            //            decimal value;
            //            if (rt.IsSuccess && fieldValue != null && Decimal.TryParse(fieldValue.ToString(), out value))
            //            {
            //                XmlElements.Add(String.Format("ReceiveAmount{0}", no), value.ToString("###,###,###,##0"));
            //            }
            //        }
            //        #endregion
            //    }
            //}
            //#endregion
            #endregion

            {
                string[] receiveItemNames = SchoolRid.GetAllReceiveItems(useEngDataUI);
                decimal?[] receiveItemAmounts = StudentReceive.GetAllReceiveItemAmounts();
                for (int idx = 0; idx < receiveItemNames.Length; idx++)
                {
                    string receiveItemName = receiveItemNames[idx];
                    if (!String.IsNullOrWhiteSpace(receiveItemName))
                    {
                        decimal? receiveItemAmount = receiveItemAmounts[idx];
                        int no = idx + 1;
                        XmlElements.Add($"ReceiveItem{no}", receiveItemName);
                        if (receiveItemAmount.HasValue)
                        {
                            XmlElements.Add($"ReceiveAmount{no}", $"{receiveItemAmount.Value:###,###,###,##0}");
                        }
                    }
                }
            }
            #endregion

            #region [NEW:20151229] 合計項目
            if (receiveSums != null && receiveSums.Length > 0)
            {
                foreach (ReceiveSumEntity receiveSum in receiveSums)
                {
                    string errmsg = null;

                    #region [MDY:202203XX] 2022擴充案 合計項目中文/英文名稱
                    SubTotalAmount subTotal = SubTotalAmount.Create(StudentReceive, receiveSum, true, useEngDataUI, out errmsg);
                    #endregion

                    if (String.IsNullOrEmpty(errmsg))
                    {
                        #region 合計項目名稱
                        XmlElements.Add(String.Format("S_{0}_Name", subTotal.Id), subTotal.Name);
                        #endregion

                        #region 合計項目金額
                        string amountText = subTotal.Amount == null ? "0" : subTotal.Amount.Value.ToString("###,###,###,##0");
                        XmlElements.Add(String.Format("S_{0}_Amount", subTotal.Id), amountText);
                        #endregion
                    }
                    else
                    {
                        //錯誤就忽略
                    }
                }
            }
            #endregion

            #region 中文金額
            //Ta1			    應繳金額萬元的國字
            //Ta2			    應繳金額千元的國字
            //Ta3			    應繳金額百元的國字
            //Ta4			    應繳金額十元的國字
            //Ta5			    應繳金額個元的國字
            //TaAmount		    應繳金額的國字

            decimal amount = 0;
            if (StudentReceive.ReceiveAmount != null)
            {
                amount = (decimal)StudentReceive.ReceiveAmount;
            }

            long billAmount = Convert.ToInt64(amount);
            string[] CMoney = this.GetChineseArray(billAmount);
            string[] tags = new string[] { "Ta5", "Ta4", "Ta3", "Ta2", "Ta1" };
            string CTaAmount = null;
            if (billAmount < 0)
            {
                CTaAmount = "負" + Common.GetChineseMoney(billAmount * -1);
            }
            else if (billAmount == 0)
            {
                CTaAmount = "零";
            }
            else
            {
                CTaAmount = Common.GetChineseMoney(billAmount);
            }
            for (int idx = 0; idx < tags.Length; idx++)
            {
                xmlWriter.WriteStartElement(tags[idx]);
                xmlWriter.WriteString(CMoney[idx]);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteElementString("TaAmount", CTaAmount);       //應繳金額的國字
            XmlElements.Add("TaAmount", CTaAmount);
            #endregion

            #region [Old] 備註改用 Memo01 ~ Memo10 欄位
            //XmlElements.Add("stu_remark_01", StudentReceive.Remark);       //學生個人備註1
            //XmlElements.Add("stu_remark_02", "");                          //學生個人備註2
            #endregion

            #region StudentReceive 的備註
            XmlElements.Add("SR_Memo01", StudentReceive.Memo01);
            XmlElements.Add("SR_Memo02", StudentReceive.Memo02);
            XmlElements.Add("SR_Memo03", StudentReceive.Memo03);
            XmlElements.Add("SR_Memo04", StudentReceive.Memo04);
            XmlElements.Add("SR_Memo05", StudentReceive.Memo05);
            XmlElements.Add("SR_Memo06", StudentReceive.Memo06);
            XmlElements.Add("SR_Memo07", StudentReceive.Memo07);
            XmlElements.Add("SR_Memo08", StudentReceive.Memo08);
            XmlElements.Add("SR_Memo09", StudentReceive.Memo09);
            XmlElements.Add("SR_Memo10", StudentReceive.Memo10);

            XmlElements.Add("SR_Memo11", StudentReceive.Memo11);
            XmlElements.Add("SR_Memo12", StudentReceive.Memo12);
            XmlElements.Add("SR_Memo13", StudentReceive.Memo13);
            XmlElements.Add("SR_Memo14", StudentReceive.Memo14);
            XmlElements.Add("SR_Memo15", StudentReceive.Memo15);
            XmlElements.Add("SR_Memo16", StudentReceive.Memo16);
            XmlElements.Add("SR_Memo17", StudentReceive.Memo17);
            XmlElements.Add("SR_Memo18", StudentReceive.Memo18);
            XmlElements.Add("SR_Memo19", StudentReceive.Memo19);
            XmlElements.Add("SR_Memo20", StudentReceive.Memo20);

            XmlElements.Add("SR_Memo21", StudentReceive.Memo21);
            #endregion

            XmlElements.Add("YearId", StudentReceive.YearId);              //學年代碼
            xmlWriter.WriteElementString("Year_Id", StudentReceive.YearId); //學年代碼
            XmlElements.Add("YearName", YearName);                         //學年名稱
            xmlWriter.WriteElementString("Year_Name", YearName);
            XmlElements.Add("TermId", StudentReceive.TermId);              //學期代碼
            XmlElements.Add("TermName", TermName);                         //學期名稱
            xmlWriter.WriteElementString("Term_Name", TermName);
            XmlElements.Add("ReceiveID", SchoolRid.ReceiveId);             //費用別代碼
            XmlElements.Add("ReceiveName", ReceiveName);                   //費用別名稱
            xmlWriter.WriteElementString("Receive_Name", ReceiveName);
            XmlElements.Add("IdentifyName", IdentifyName);                 //身分註記名稱
            xmlWriter.WriteElementString("Identify_Name", IdentifyName);
            XmlElements.Add("IdentifyName1", IdentifyName01); //身分註記名稱1
            XmlElements.Add("IdentifyName2", IdentifyName02); //身分註記名稱2
            XmlElements.Add("IdentifyName3", IdentifyName03); //身分註記名稱3
            XmlElements.Add("IdentifyName4", IdentifyName04); //身分註記名稱4
            XmlElements.Add("IdentifyName5", IdentifyName05); //身分註記名稱5
            XmlElements.Add("IdentifyName6", IdentifyName06); //身分註記名稱6

            #region 就貸金額相關
            XmlElements.Add("Loan", StudentReceive.Loan.ToString("###,###,###,##0"));           //上傳就學貸款金額 (BUD上傳的就貸金額)
            XmlElements.Add("RealLoan", StudentReceive.RealLoan.ToString("###,###,###,##0"));   //實際貸款金額 (BUD上傳的就貸金額或由BUD上傳的就貸明細總額)

            #region [MDY:20160412] 可貸金額增加 SchoolRid.LoanQual 判斷
            #region [Old]
            //if (StudentReceive.LoanAmount != null)
            //{
            //    XmlElements.Add("LoanFee", Convert.ToDecimal(StudentReceive.LoanAmount).ToString("###,###,###,##0")); //可貸金額 (BUA或頁面上輸入的預估就貸金額)
            //}
            #endregion

            if (SchoolRid.LoanQual == "1" && StudentReceive.LoanAmount != null)
            {
                XmlElements.Add("LoanFee", Convert.ToDecimal(StudentReceive.LoanAmount).ToString("###,###,###,##0")); //可貸金額 (BUA或頁面上輸入的預估就貸金額)
            }
            else if (SchoolRid.LoanQual == "2")
            {
                XmlElements.Add("LoanFee", StudentReceive.RealLoan.ToString("###,###,###,##0"));   //實際貸款金額 (BUD上傳的就貸金額或由BUD上傳的就貸明細總額)
            }
            #endregion
            #endregion

            #region [MDY:20160412] 就貸名稱
            if (!String.IsNullOrWhiteSpace(LoanName))
            {
                XmlElements.Add("LoanName", LoanName);                         //就貸名稱
            }
            #endregion

            XmlElements.Add("ReduceName", ReduceName);                     //減免名稱
            XmlElements.Add("DormName", DormName);                         //住宿名稱
            XmlElements.Add("ReceiveType", StudentReceive.ReceiveType);    //代收類別
            XmlElements.Add("ReceiveAmount", StudentReceive.ReceiveAmount != null ? ((decimal)StudentReceive.ReceiveAmount).ToString("###,###,###,##0") : "0"); //繳費金額合計
            XmlElements.Add("Principal", string.Format("{0} {1}", SchoolRid.AName1, SchoolRid.ATitle1)); // 機關長官姓名 + 機關長官職稱
            XmlElements.Add("Principal_Name", SchoolRid.AName1);           //機關長官姓名
            XmlElements.Add("Principal_Title", SchoolRid.ATitle1);         //機關長官職稱
            XmlElements.Add("Accounting", string.Format("{0} {1}", SchoolRid.AName2, SchoolRid.ATitle2)); //主辦會計姓名 + 主辦會計職稱
            XmlElements.Add("Accounting_Name", SchoolRid.AName2);          //主辦會計姓名 
            XmlElements.Add("Accounting_Title", SchoolRid.ATitle2);        //主辦會計職稱
            XmlElements.Add("Cashier", string.Format("{0} {1}", SchoolRid.AName3, SchoolRid.ATitle3));   //主辦出納姓名 + 主辦出納職稱
            XmlElements.Add("Cashier_Name", SchoolRid.AName3);             //主辦出納姓名
            XmlElements.Add("Cashier_Title", SchoolRid.ATitle3);           //主辦出納職稱

            #region [New] 20151013 新增校長名稱變數，因為衛理女子高中的繳費單有
            XmlElements.Add("Sch_Principal", SchoolRType.SchPrincipal);           //主辦出納職稱
            #endregion

            #region 代收費用別備註
            #region [Old]
            //XmlElements.Add("Memo1", SchoolRid.ReceiveMemoa );              //代收費用別備註1
            //XmlElements.Add("Memo2", SchoolRid.ReceiveMemob);              //代收費用別備註2
            //XmlElements.Add("Memo3", "");                                  //代收費用別備註3
            //XmlElements.Add("Memo4", "");                                  //代收費用別備註4
            //XmlElements.Add("Memo5", "");                                  //代收費用別備註5
            //XmlElements.Add("Memo6", "");                                  //代收費用別備註6
            //XmlElements.Add("Memo7", "");                                  //代收費用別備註7
            //XmlElements.Add("Memo8", "");                                  //代收費用別備註8
            //XmlElements.Add("Memo9", "");                                  //代收費用別備註9
            #endregion

            if (!String.IsNullOrWhiteSpace(SchoolRid.ReceiveMemo))
            {
                #region [MDY:20160519] 依據模板編號決定備註每行的長度
                #region 邏輯
                // 1. 公版 1 ： 三聯式繳費單，每行 20 個字
                // 2. 公版 3 ： 四聯式繳費單，每行 24 個字
                // 3. 公版 4 ： 四聯式繳費單，每行 24 個字
                // 4. 其他   ： 專屬或其他公版，每行 22 個字
                #endregion

                #region [Old]
                //string[] memos = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

                //List<string> buffs = new List<string>();
                //string[] lines = SchoolRid.ReceiveMemo.Trim().Split(new string[] { "\n" }, StringSplitOptions.None);
                //for (int i = 0; i < lines.Length; i++)
                //{
                //    string[] tmps = split22(lines[i]);
                //    //string[] tmps = split22(lines[i]);
                //    foreach (string tmp in tmps)
                //    {
                //        if (!String.IsNullOrWhiteSpace(tmp) && tmp != "\r\n" && tmp != "\r" && tmp != "\n")
                //        {
                //            buffs.Add(tmp);
                //        }
                //    }
                //    //buffs.AddRange(tmps);
                //}

                //for (int i = 0; i < buffs.Count; i++)
                //{
                //    if (i >= memos.Length)
                //    {
                //        break;
                //    }
                //    memos[i] = buffs[i];
                //}

                //XmlElements.Add("Memo1", memos[0]);        //代收費用別備註1
                //XmlElements.Add("Memo2", memos[1]);        //代收費用別備註2
                //XmlElements.Add("Memo3", memos[2]);        //代收費用別備註3
                //XmlElements.Add("Memo4", memos[3]);        //代收費用別備註4
                //XmlElements.Add("Memo5", memos[4]);        //代收費用別備註5
                //XmlElements.Add("Memo6", memos[5]);        //代收費用別備註6
                //XmlElements.Add("Memo7", memos[6]);        //代收費用別備註7
                //XmlElements.Add("Memo8", memos[7]);        //代收費用別備註8
                //XmlElements.Add("Memo9", memos[8]);        //代收費用別備註9
                //XmlElements.Add("Memo10", memos[9]);        //代收費用別備註9
                //XmlElements.Add("Memo11", memos[10]);        //代收費用別備註9
                //XmlElements.Add("Memo12", memos[11]);        //代收費用別備註9
                //XmlElements.Add("Memo13", memos[12]);        //代收費用別備註9
                //XmlElements.Add("Memo14", memos[13]);        //代收費用別備註9
                //XmlElements.Add("Memo15", memos[14]);        //代收費用別備註9
                //XmlElements.Add("Memo16", memos[15]);        //代收費用別備註9
                //XmlElements.Add("Memo17", memos[16]);        //代收費用別備註9
                //XmlElements.Add("Memo18", memos[17]);        //代收費用別備註9
                //XmlElements.Add("Memo19", memos[18]);        //代收費用別備註9
                //XmlElements.Add("Memo20", memos[19]);        //代收費用別備註9
                #endregion

                int maxLineCount = 20;
                int maxLineLength = 22;
                switch (SchoolRid.BillformId)
                {
                    case "1":
                        maxLineLength = 20;
                        break;
                    case "3":
                    case "4":
                        maxLineLength = 24;
                        break;
                    default:
                        maxLineLength = 22;
                        break;
                }

                List<string> lines = new List<string>(maxLineCount);
                string[] temps = SchoolRid.ReceiveMemo.Trim().Split(new string[] { "\n" }, StringSplitOptions.None);

                #region [MDY:2018xxxx] SchoolRid.ReceiveMemoNoWrap == "Y" 則不自動換行
                #region [OLD]
                //foreach (string temp in temps)
                //{
                //    if (temp.Length > 0)
                //    {
                //        if (temp.Length > maxLineLength)
                //        {
                //            string[] tmps = this.SplitByLength(temp, maxLineLength);
                //            foreach (string tmp in tmps)
                //            {
                //                if (!String.IsNullOrWhiteSpace(tmp) && tmp != "\r\n" && tmp != "\r" && tmp != "\n")
                //                {
                //                    lines.Add(tmp);
                //                }
                //            }
                //        }
                //        else
                //        {
                //            lines.Add(temp);
                //        }
                //    }
                //    if (lines.Count > maxLineCount)
                //    {
                //        break;
                //    }
                //}
                #endregion

                bool isNoWrap = "Y".Equals(SchoolRid.ReceiveMemoNoWrap, StringComparison.CurrentCultureIgnoreCase);
                foreach (string temp in temps)
                {
                    if (temp.Length > 0)
                    {
                        if (isNoWrap || temp.Length <= maxLineLength)
                        {
                            lines.Add(temp);
                        }
                        else
                        {
                            string[] tmps = this.SplitByLength(temp, maxLineLength);
                            foreach (string tmp in tmps)
                            {
                                if (!String.IsNullOrWhiteSpace(tmp) && tmp != "\r\n" && tmp != "\r" && tmp != "\n")
                                {
                                    lines.Add(tmp);
                                }
                            }
                        }
                    }
                    if (lines.Count > maxLineCount)
                    {
                        break;
                    }
                }
                #endregion

                int no = 1;
                foreach (string line in lines)
                {
                    string key = String.Format("Memo{0}", no);
                    XmlElements.Add(key, line);     //代收費用別備註no
                    no++;
                }
                #endregion
            }
            #endregion

            //Memo10			"*全家、統一及萊爾富超商繳費，需自付手續費?元，繳費期限?。"
            //Memo11			"*郵局繳費無需付手續費。繳費期限?。"
            //Memo12            "*本單據至台銀臨櫃繳費，需自付手續費?元，繳費期限: ?。"
            //Memo14			"利用各行庫自動櫃員機轉帳,轉入行請點選：臺灣銀行(代號004)、輸入轉入帳號：?、轉入金額：?。"
            //Ad1			    繳費單廣告管理1
            //Ad2			    繳費單廣告管理2
            //Ad3			    繳費單廣告管理3
            //Ad4			    繳費單廣告管理4
            //Ad5			    繳費單廣告管理5
            //Ad6			    繳費單廣告管理6

            XmlElements.Add("BankName", BankName);           //主辦分行名稱
            XmlElements.Add("BankTel", BankTel);             //主辦分行電話

            #region SMFee : 超商自付手續費
            {
                int smFee = 0;
                if (smChannelWay != null && smChannelWay.IncludePay == "0" && smChannelWay.ChannelCharge != null)
                {
                    //有超商管道 且 不包含手續費(外加) 且有手續費金額
                    smFee = Convert.ToInt32(smChannelWay.ChannelCharge.Value);
                }
                xmlWriter.WriteElementString("SMFee", smFee.ToString());     //超商繳款須自付 處理費
            }
            #endregion

            #region BankFee : 銀行(臨櫃)自付手續費
            {
                int bankFee = 0;
                if (cashChannelWay != null && cashChannelWay.IncludePay == "0" && cashChannelWay.ChannelCharge != null)
                {
                    //有臨櫃管道 且 不包含手續費(外加) 且有手續費金額
                    bankFee = Convert.ToInt32(smChannelWay.ChannelCharge.Value);
                }
                xmlWriter.WriteElementString("BankFee", bankFee.ToString());   //臨櫃繳款須自付 處理費
            }
            #endregion

            //XmlElements.Add("CancelNoLen", StudentReceive.CancelNo.Length.ToString());           //銷帳編號幾碼
            xmlWriter.WriteElementString("CancelNoLen", StudentReceive.CancelNo == null ? "0" : StudentReceive.CancelNo.Length.ToString()); //銷帳編號幾碼

            GenMultiElement(xmlWriter, XmlElements);

            #region 銀行(臨櫃)條碼 [銷編16碼(14碼則後補2碼空白)+金額]
            if (!String.IsNullOrWhiteSpace(StudentReceive.CancelAtmno) && StudentReceive.ReceiveAmount != null && StudentReceive.ReceiveAmount.Value > 0)
            {
                #region TAG 說明
                // BankBarcode : 銀行(臨櫃)條碼
                // BankBarcode_text : 銀行(臨櫃)條碼文字
                #endregion

                string bbarcode = String.Format("{0}{1:0}", StudentReceive.CancelNo.PadRight(16, ' '), StudentReceive.ReceiveAmount.Value);
                GenBarcodeElement(xmlWriter, "BankBarcode", bbarcode);
                GenBarcodeTextElement(xmlWriter, "BankBarcode_text", bbarcode);

                string cancelNoBarcode = StudentReceive.CancelNo;
                GenBarcodeElement(xmlWriter, "BankBarcode1", cancelNoBarcode);
                GenBarcodeTextElement(xmlWriter, "BankBarcode1_text", cancelNoBarcode);

                string amountBarcode = StudentReceive.ReceiveAmount.Value.ToString("0");
                GenBarcodeElement(xmlWriter, "ReceiveAmountBarcode", amountBarcode);
                GenBarcodeTextElement(xmlWriter, "ReceiveAmountBarcode_text", amountBarcode);
            }
            #endregion

            #region 超商三段條碼
            if (payDueDate != null && smChannelWay != null && !String.IsNullOrWhiteSpace(StudentReceive.CancelSmno) && StudentReceive.ReceiveSmamount != null && StudentReceive.ReceiveSmamount.Value > 0)
            {
                #region TAG 說明
                // SMBarcode1 : 超商第一段條碼
                // SMBarcode2 : 超商第二段條碼
                // SMBarcode3 : 超商第三段條碼
                // SMNumber1  : 超商第一段條碼文字
                // SMNumber2  : 超商第二段條碼文字
                // SMNumber3  : 超商第三段條碼文字
                #endregion

                ChannelHelper helper = new ChannelHelper();

                //超商繳款期限
                DateTime smPayDueDate = payDueDate.Value;
                if (SchoolRid.ExtraDays > 0)
                {
                    smPayDueDate = smPayDueDate.AddDays(SchoolRid.ExtraDays);
                }

                string zbarcode1 = helper.GenSMBarcode1(smPayDueDate, smChannelWay.BarcodeId);
                string zbarcode2 = helper.GenSMBarcode2(StudentReceive.CancelSmno);
                string zbarcode3 = helper.GenSMBarcode3(zbarcode1, zbarcode2, payDueDate.Value, StudentReceive.ReceiveSmamount.Value);

                GenBarcodeElement(xmlWriter, "SMBarcode1", zbarcode1);
                GenBarcodeElement(xmlWriter, "SMBarcode2", zbarcode2);
                GenBarcodeElement(xmlWriter, "SMBarcode3", zbarcode3);

                string zbarcode1Text = zbarcode1;
                string zbarcode2Text = zbarcode2;
                string zbarcode3Text = zbarcode3;
                GenBarcodeTextElement(xmlWriter, "SMNumber1", zbarcode1Text);
                GenBarcodeTextElement(xmlWriter, "SMNumber2", zbarcode2Text);
                GenBarcodeTextElement(xmlWriter, "SMNumber3", zbarcode3Text);
            }
            #endregion

            #region 郵局三段條碼
            //#region TAG 說明
            //// POBarcode1 : 郵局第一段條碼
            //// POBarcode2 : 郵局第二段條碼
            //// POBarcode3 : 郵局第三段條碼
            //// PONumber1 : 郵局第一段條碼文字
            //// PONumber2 : 郵局第二段條碼文字
            //// PONumber3 : 郵局第三段條碼文字
            //#endregion

            ////郵局三段條碼規則
            ////barcode1:劃撥帳號
            ////barcode2:繳費期限(yyyMMdd)虛擬帳號
            ////barcode3:金額

            //string pbarcode1 = StudentReceive.CancelPONo;
            //string pbarcode2 = SchoolRid.PayDate + StudentReceive.CancelPONo;
            //string pbarcode3 = String.Format("{0:0}", 0);  //ToDo 沒有郵局的應收金額(StudentReceive.ReceivePOAmount)

            //GenBarcodeElement(xmlWriter, "POBarcode1", pbarcode1);
            //GenBarcodeElement(xmlWriter, "POBarcode2", pbarcode2);
            //GenBarcodeElement(xmlWriter, "POBarcode3", pbarcode3);

            //string pbarcode1Text = pbarcode1;
            //string pbarcode2Text = pbarcode2;
            //string pbarcode3Text = String.Format("{0:#,##0}", 0);  //ToDo 沒有郵局的應收金額(StudentReceive.ReceivePOAmount)

            //GenBarcodeTextElement(xmlWriter, "PONumber1", pbarcode1Text);
            //GenBarcodeTextElement(xmlWriter, "PONumber2", pbarcode2Text);
            //GenBarcodeTextElement(xmlWriter, "PONumber3", pbarcode3Text);
            #endregion

            #region [MDY:20171127] 財金 QRCode 支付 (20170831_01)
            if (payDueDate != null
                && !String.IsNullOrWhiteSpace(StudentReceive.CancelNo)
                && (StudentReceive.ReceiveAmount != null && StudentReceive.ReceiveAmount > 0)
                && (qrcodeConfig != null && qrcodeConfig.IsReady()))
            {
                xmlWriter.WriteStartElement("QRCode");
                xmlWriter.WriteAttributeString("FontName", "QRCODE");
                xmlWriter.WriteAttributeString("Align", "center");
                string qrcode = qrcodeConfig.GetQRCode(StudentReceive.ReceiveAmount.Value, StudentReceive.CancelNo, payDueDate.Value, ReceiveName);

                #region [MDY:20181209] 增加一個繳費網址 (20181207_01)
                if (String.IsNullOrWhiteSpace(qrcodeConfig.PayUrl))
                {
                    xmlWriter.WriteString(System.Net.WebUtility.UrlEncode(qrcode));
                }
                else
                {
                    xmlWriter.WriteString(String.Concat(qrcodeConfig.PayUrl, "\r\n",System.Net.WebUtility.UrlEncode(qrcode)));
                }
                #endregion

                xmlWriter.WriteEndElement();
            }
            #endregion

            //</Fields> 
            xmlWriter.WriteEndElement();

            //Write the XML to file and close the xmlWriter.
            xmlWriter.Flush();

            return true;

        }

        /// <summary>
        /// 產生收據的 XML DATA
        /// </summary>
        /// <param name="xmlWriter"></param>
        /// <param name="PageNo"></param>
        /// <param name="UserId"></param>
        /// <param name="DepName"></param>
        /// <param name="MajorName"></param>
        /// <param name="GradeName"></param>
        /// <param name="CollegeName"></param>
        /// <param name="ClassName"></param>
        /// <param name="ReduceName"></param>
        /// <param name="DormName"></param>
        /// <param name="LoanName"></param>
        /// <param name="YearName"></param>
        /// <param name="TermName"></param>
        /// <param name="ReceiveName"></param>
        /// <param name="IdentifyName"></param>
        /// <param name="IdentifyName01"></param>
        /// <param name="IdentifyName02"></param>
        /// <param name="IdentifyName03"></param>
        /// <param name="IdentifyName04"></param>
        /// <param name="IdentifyName05"></param>
        /// <param name="IdentifyName06"></param>
        /// <param name="SchoolRid"></param>
        /// <param name="StudentMaster"></param>
        /// <param name="SchoolRType"></param>
        /// <param name="StudentReceive"></param>
        /// <param name="fgMark"></param>
        /// <param name="isEngUI">指定是否英文介面</param>
        /// <returns></returns>
        public bool GenReceiptXmlData(XmlWriter xmlWriter, int PageNo, string UserId
            , string DepName, string MajorName, string GradeName, string CollegeName
            , string ClassName, string ReduceName, string DormName, string LoanName
            , string YearName, string TermName, string ReceiveName
            , string IdentifyName, string IdentifyName01, string IdentifyName02, string IdentifyName03, string IdentifyName04, string IdentifyName05, string IdentifyName06
            , SchoolRidEntity SchoolRid, StudentMasterEntity StudentMaster
            , SchoolRTypeEntity SchoolRType, StudentReceiveEntity StudentReceive
            , bool fgMark, bool isEngUI)
        {
            #region [MDY:202203XX] 2022擴充案 是否使用英文資料介面
            bool useEngDataUI = isEngUI && SchoolRType.IsEngEnabled();
            #endregion

            // <Fields Page='1'>
            xmlWriter.WriteStartElement("Fields");
            xmlWriter.WriteAttributeString("Page", PageNo.ToString());

            // <Year>2014</Year>
            xmlWriter.WriteStartElement("Year");
            xmlWriter.WriteString(DateTime.Now.Year.ToString());
            xmlWriter.WriteEndElement();

            // <CYear>103</CYear>
            xmlWriter.WriteStartElement("CYear");
            xmlWriter.WriteString(Common.GetTWDate7(DateTime.Now).Substring(0, 3));
            xmlWriter.WriteEndElement();

            // <Month>05</Month>
            xmlWriter.WriteStartElement("Month");
            xmlWriter.WriteString(DateTime.Now.Month.ToString());
            xmlWriter.WriteEndElement();

            // <Day>15</Day>
            xmlWriter.WriteStartElement("Day");
            xmlWriter.WriteString(DateTime.Now.Day.ToString());
            xmlWriter.WriteEndElement();

            // <PrintDate>2014/05/15</PrintDate>
            xmlWriter.WriteStartElement("PrintDate");
            xmlWriter.WriteString(DateTime.Now.ToString("yyyy/MM/dd"));
            xmlWriter.WriteEndElement();

            // <PrintDate>2014/05/15</PrintDate>
            xmlWriter.WriteStartElement("CPrintDate");
            xmlWriter.WriteString(Common.GetTWDate7(DateTime.Now).Substring(0, 3) + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日");
            xmlWriter.WriteEndElement();

            //<UserID>BOT001</UserID>
            xmlWriter.WriteStartElement("UserID");
            xmlWriter.WriteString(UserId);
            xmlWriter.WriteEndElement();

            #region [Old] 土銀沒有 QRCode 與 系統別
            ////QRCODE Elements value=系統別,學校名稱,學生姓名,銷帳編號,金額
            ////系統別：學雜費放CPB1，企業放CBP2
            //string strAmount = "0";
            //if (StudentReceive.ReceiveAmount != null)
            //{
            //    strAmount = ((decimal)StudentReceive.ReceiveAmount).ToString("###,###,###,##0");
            //}
            //xmlWriter.WriteStartElement("qrcode");
            //xmlWriter.WriteAttributeString("FontName", "QRCODE");
            //xmlWriter.WriteString(string.Format("{0},{1},{2},{3},{4}", "CPB1",
            //    SchoolRType.SchName, StudentMaster.Name, StudentReceive.CancelNo, strAmount));
            //xmlWriter.WriteEndElement();

            ////系統別
            //xmlWriter.WriteElementString("SysType", "01");   //因為這是學雜費專用的所以固定為 01
            #endregion

            #region [MDY:20160131] 因為 StudentReceive 增加繳款期限 (PayDueDate)，所以先取 StudentReceive.PayDueDate，無值採改取 SchoolRid.PayDate
            #region [Old]
            //繳款期限
            //DateTime? payDuDate = DataFormat.ConvertDateText(SchoolRid.PayDate);
            #endregion

            string txtPayDueDate = null;
            DateTime? payDueDate = null;
            if (String.IsNullOrWhiteSpace(StudentReceive.PayDueDate))
            {
                txtPayDueDate = SchoolRid.PayDate;
                payDueDate = DataFormat.ConvertDateText(SchoolRid.PayDate);
            }
            else
            {
                txtPayDueDate = StudentReceive.PayDueDate;
                payDueDate = DataFormat.ConvertDateText(StudentReceive.PayDueDate);
            }
            #endregion

            if (payDueDate != null)
            {
                xmlWriter.WriteElementString("PayDueDate", String.Format("{0:yyyy/MM/dd}", payDueDate.Value));   //繳款期限 (4碼西元年 yyyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
                xmlWriter.WriteElementString("CPayDueDate", String.Format("{0:000}/{1:MM/dd}", payDueDate.Value.Year - 1911, payDueDate.Value));   //繳款期限 (3碼民國年 yyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
            }
            else
            {
                xmlWriter.WriteElementString("PayDueDate", txtPayDueDate);      //繳款期限
                xmlWriter.WriteElementString("CPayDueDate", txtPayDueDate);     //繳款期限
            }

            KeyValueList<string> XmlElements = new KeyValueList<string>();
            XmlElements.Add("RePrint", "");                           //補印註記(18號字)
            XmlElements.Add("SchoolZipCode", SchoolRType.SchPostal);  //學校郵遞區號(12號字)
            XmlElements.Add("SchoolAddress", SchoolRType.SchAddress); //學校地址(12號字)
            XmlElements.Add("StudentZipCode", StudentMaster.ZipCode); //學生郵遞區號(12號字)
            XmlElements.Add("SchoolTel1", SchoolRType.SchConTel);     //學校聯絡人1電話(12號字)
            XmlElements.Add("SchoolTel2", SchoolRType.SchConTel1);    //學校聯絡人2電話(12號字)

            #region [MDY:202203XX] 2022擴充案 學校中文、英文名稱
            XmlElements.Add("SchoolName", (useEngDataUI ? SchoolRType.SchEName : SchoolRType.SchName));       //學校名稱
            #endregion

            #region 學生身分證字號 & 學生姓名 & 學生地址 & 學生生日
            if (fgMark)
            {
                XmlElements.Add("id", DataFormat.MaskText(StudentMaster.IdNumber, DataFormat.MaskDataType.ID));         //學生身分證字號

                #region [MDY:20200705] 姓名改要遮罩
                #region 姓名遮罩
                XmlElements.Add("StudentName", DataFormat.MaskText(StudentMaster.Name, DataFormat.MaskDataType.Name));  //學生姓名
                #endregion

                #region [Old] 姓名不遮罩
                //XmlElements.Add("StudentName", StudentMaster.Name);  //學生姓名
                #endregion
                #endregion

                XmlElements.Add("StudentAddress", DataFormat.MaskText(StudentMaster.Address, DataFormat.MaskDataType.Address)); //學生地址(12號字)
                XmlElements.Add("birthday", DataFormat.MaskText(StudentMaster.Birthday, DataFormat.MaskDataType.Birthday));      //學生生日
            }
            else
            {
                XmlElements.Add("id", StudentMaster.IdNumber);            //學生身分證字號
                XmlElements.Add("StudentName", StudentMaster.Name);       //學生姓名
                XmlElements.Add("StudentAddress", StudentMaster.Address); //學生地址(12號字)
                XmlElements.Add("birthday", StudentMaster.Birthday);      //學生生日
            }
            #endregion

            XmlElements.Add("email", StudentMaster.Email);            //學生電子郵件

            #region [MDY:202203XX] 2022擴充案 注意事項中文、英文內容
            string[] briefs = SchoolRid.GetAllBrief(useEngDataUI);
            XmlElements.Add("brief1", briefs[0]);              //注意事項1
            XmlElements.Add("brief2", briefs[1]);              //注意事項2
            XmlElements.Add("brief3", briefs[2]);              //注意事項3
            XmlElements.Add("brief4", briefs[3]);              //注意事項4
            XmlElements.Add("brief5", briefs[4]);              //注意事項5
            XmlElements.Add("brief6", briefs[5]);              //注意事項6
            #endregion

            XmlElements.Add("CancelNo", StudentReceive.CancelNo);     //銷帳編號
            XmlElements.Add("SchoolWordSN", SchoolRid.SchWord);       //字軌
            XmlElements.Add("StudentID", StudentMaster.Id);           //學號
            XmlElements.Add("DepName", DepName);                      //部別名稱
            XmlElements.Add("MajorName", MajorName);                  //科系名稱
            XmlElements.Add("GradeName", GradeName);                  //年級名稱
            XmlElements.Add("CollegeName", CollegeName);              //院別名稱
            XmlElements.Add("ClassName", ClassName);                  //班級名稱
            XmlElements.Add("StudentHid", StudentReceive.StuHid);     //學號

            #region [Old] 沒有收入科目名稱就不產生收入科目金額
            //#region 收入科目名稱 1 ~ 30
            //Result rt = new Result();
            //string itemName = string.Empty;
            //string itemValue = string.Empty;
            //for (int i = 1; i < 31; i++)
            //{
            //    itemName = "ReceiveItem" + i.ToString("00");
            //    object ob = SchoolRid.GetValue(itemName, out rt);
            //    if (rt.IsSuccess && ob != null)
            //    {
            //        itemValue = ob.ToString();
            //        XmlElements.Add("ReceiveItem" + i.ToString(), itemValue);
            //    }
            //}
            //#endregion

            //#region 收入科目金額 1 ~ 30
            //for (int i = 1; i < 31; i++)
            //{
            //    itemName = "Receive" + i.ToString("00");
            //    object ob = StudentReceive.GetValue(itemName, out rt);
            //    decimal value;
            //    if (rt.IsSuccess && ob != null && Decimal.TryParse(ob.ToString(), out value))
            //    {
            //        itemValue = value.ToString("###,###,###,##0");
            //        XmlElements.Add("ReceiveAmount" + i.ToString(), itemValue);
            //    }
            //}
            //#endregion
            #endregion

            #region [MDY:202203XX] 2022擴充案 收入科目中文/英文名稱、金額 (40項)
            #region [OLD]
            //#region [New] 沒有收入科目名稱就不產生收入科目金額 (40項)
            //{
            //    Result rt = null;
            //    for (int no = 1; no <= 40; no++)
            //    {
            //        #region 收入科目名稱
            //        string receiveItemName = null;
            //        {
            //            string fieldName = String.Format("ReceiveItem{0:00}", no);
            //            object fieldValue = SchoolRid.GetValue(fieldName, out rt);
            //            if (rt.IsSuccess && fieldValue != null)
            //            {
            //                receiveItemName = fieldValue.ToString();
            //                XmlElements.Add(String.Format("ReceiveItem{0}", no), receiveItemName);
            //            }
            //        }
            //        #endregion

            //        #region 收入科目金額
            //        if (!String.IsNullOrWhiteSpace(receiveItemName))
            //        {
            //            string fieldName = String.Format("Receive{0:00}", no);
            //            object fieldValue = StudentReceive.GetValue(fieldName, out rt);
            //            decimal value;
            //            if (rt.IsSuccess && fieldValue != null && Decimal.TryParse(fieldValue.ToString(), out value))
            //            {
            //                XmlElements.Add(String.Format("ReceiveAmount{0}", no), value.ToString("###,###,###,##0"));
            //            }
            //        }
            //        #endregion
            //    }
            //}
            //#endregion
            #endregion

            {
                string[] receiveItemNames = SchoolRid.GetAllReceiveItems(useEngDataUI);
                decimal?[] receiveItemAmounts = StudentReceive.GetAllReceiveItemAmounts();
                for (int idx = 0; idx < receiveItemNames.Length; idx++)
                {
                    string receiveItemName = receiveItemNames[idx];
                    if (!String.IsNullOrWhiteSpace(receiveItemName))
                    {
                        decimal? receiveItemAmount = receiveItemAmounts[idx];
                        int no = idx + 1;
                        XmlElements.Add($"ReceiveItem{no}", receiveItemName);
                        if (receiveItemAmount.HasValue)
                        {
                            XmlElements.Add($"ReceiveAmount{no}", $"{receiveItemAmount.Value:###,###,###,##0}");
                        }
                    }
                }
            }
            #endregion

            #region 中文金額
            //Ta1			    應繳金額萬元的國字
            //Ta2			    應繳金額千元的國字
            //Ta3			    應繳金額百元的國字
            //Ta4			    應繳金額十元的國字
            //Ta5			    應繳金額個元的國字
            //TaAmount		    應繳金額的國字

            decimal amount = 0;
            if (StudentReceive.ReceiveAmount != null)
            {
                amount = (decimal)StudentReceive.ReceiveAmount;
            }

            string[] CMoney = this.GetChineseArray((long)amount);
            string[] tags = new string[] { "Ta5", "Ta4", "Ta3", "Ta2", "Ta1" };
            string CTaAmount = Common.GetChineseMoney((long)amount);
            for (int idx = 0; idx < tags.Length; idx++)
            {
                xmlWriter.WriteStartElement(tags[idx]);
                xmlWriter.WriteString(CMoney[idx]);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteElementString("TaAmount", CTaAmount);       //應繳金額的國字
            #endregion

            #region [Old] 備註改用 Memo01 ~ Memo10 欄位
            //XmlElements.Add("stu_remark_01", StudentReceive.Remark);       //學生個人備註1
            //XmlElements.Add("stu_remark_02", "");                          //學生個人備註2
            #endregion

            #region StudentReceive 的備註
            XmlElements.Add("SR_Memo01", StudentReceive.Memo01);
            XmlElements.Add("SR_Memo02", StudentReceive.Memo02);
            XmlElements.Add("SR_Memo03", StudentReceive.Memo03);
            XmlElements.Add("SR_Memo04", StudentReceive.Memo04);
            XmlElements.Add("SR_Memo05", StudentReceive.Memo05);
            XmlElements.Add("SR_Memo06", StudentReceive.Memo06);
            XmlElements.Add("SR_Memo07", StudentReceive.Memo07);
            XmlElements.Add("SR_Memo08", StudentReceive.Memo08);
            XmlElements.Add("SR_Memo09", StudentReceive.Memo09);
            XmlElements.Add("SR_Memo10", StudentReceive.Memo10);

            XmlElements.Add("SR_Memo11", StudentReceive.Memo11);
            XmlElements.Add("SR_Memo12", StudentReceive.Memo12);
            XmlElements.Add("SR_Memo13", StudentReceive.Memo13);
            XmlElements.Add("SR_Memo14", StudentReceive.Memo14);
            XmlElements.Add("SR_Memo15", StudentReceive.Memo15);
            XmlElements.Add("SR_Memo16", StudentReceive.Memo16);
            XmlElements.Add("SR_Memo17", StudentReceive.Memo17);
            XmlElements.Add("SR_Memo18", StudentReceive.Memo18);
            XmlElements.Add("SR_Memo19", StudentReceive.Memo19);
            XmlElements.Add("SR_Memo20", StudentReceive.Memo20);

            XmlElements.Add("SR_Memo21", StudentReceive.Memo21);
            #endregion

            XmlElements.Add("YearName", YearName);              //學年名稱
            XmlElements.Add("YearId", StudentReceive.YearId);   //學年代碼
            XmlElements.Add("TermName", TermName);              //學期名稱
            XmlElements.Add("TermId", StudentReceive.TermId);   //學期代碼
            XmlElements.Add("ReceiveID", SchoolRid.ReceiveId);  //費用別代碼
            XmlElements.Add("ReceiveName", ReceiveName);        //費用別名稱
            XmlElements.Add("IdentifyName", IdentifyName);                 //身分註記名稱
            XmlElements.Add("IdentifyName1", IdentifyName01);   //身分註記名稱1
            XmlElements.Add("IdentifyName2", IdentifyName02);   //身分註記名稱2
            XmlElements.Add("IdentifyName3", IdentifyName03);   //身分註記名稱3
            XmlElements.Add("IdentifyName4", IdentifyName04);   //身分註記名稱4
            XmlElements.Add("IdentifyName5", IdentifyName05);   //身分註記名稱5
            XmlElements.Add("IdentifyName6", IdentifyName06);   //身分註記名稱6

            #region 就貸金額相關
            XmlElements.Add("Loan", StudentReceive.Loan.ToString("###,###,###,##0"));           //上傳就學貸款金額 (BUD上傳的就貸金額)
            XmlElements.Add("RealLoan", StudentReceive.RealLoan.ToString("###,###,###,##0"));   //實際貸款金額 (BUD上傳的就貸金額或由BUD上傳的就貸明細總額)

            #region [MDY:20160412] 可貸金額增加 SchoolRid.LoanQual 判斷
            #region [Old]
            //if (StudentReceive.LoanAmount != null)
            //{
            //    XmlElements.Add("LoanFee", Convert.ToDecimal(StudentReceive.LoanAmount).ToString("###,###,###,##0")); //可貸金額 (BUA或頁面上輸入的預估就貸金額)
            //}
            #endregion

            if (SchoolRid.LoanQual == "1" && StudentReceive.LoanAmount != null)
            {
                XmlElements.Add("LoanFee", Convert.ToDecimal(StudentReceive.LoanAmount).ToString("###,###,###,##0")); //可貸金額 (BUA或頁面上輸入的預估就貸金額)
            }
            else if (SchoolRid.LoanQual == "2")
            {
                XmlElements.Add("LoanFee", StudentReceive.RealLoan.ToString("###,###,###,##0"));   //實際貸款金額 (BUD上傳的就貸金額或由BUD上傳的就貸明細總額)
            }
            #endregion
            #endregion

            #region [MDY:20160412] 就貸名稱
            if (!String.IsNullOrWhiteSpace(LoanName))
            {
                XmlElements.Add("LoanName", LoanName);                         //就貸名稱
            }
            #endregion

            XmlElements.Add("ReduceName", ReduceName);                     //減免名稱
            XmlElements.Add("DormName", DormName);                         //住宿名稱
            XmlElements.Add("ReceiveType", StudentReceive.ReceiveType);    //代收類別
            XmlElements.Add("ReceiveAmount", StudentReceive.ReceiveAmount != null ? ((decimal)StudentReceive.ReceiveAmount).ToString("###,###,###,##0") : "0"); //繳費金額合計

            //XmlElements.Add("ReceiveDate", StudentReceive.ReceiveDate);    //代收日
            DateTime? receiveDate = DataFormat.ConvertDateText(StudentReceive.ReceiveDate);
            if (receiveDate != null)
            {
                XmlElements.Add("ReceiveDate", String.Format("{0:yyyy/MM/dd}", receiveDate.Value));   //代收日 (4碼西元年 yyyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
                xmlWriter.WriteElementString("ReceiveDateY", String.Format("{0:yyyy}", receiveDate.Value));
                xmlWriter.WriteElementString("ReceiveDateCY", String.Format("{0}", receiveDate.Value.Year - 1911));
                xmlWriter.WriteElementString("ReceiveDateM", String.Format("{0:MM}", receiveDate.Value));
                xmlWriter.WriteElementString("ReceiveDateD", String.Format("{0:dd}", receiveDate.Value));
                XmlElements.Add("CReceiveDate", String.Format("{0:000}/{1:MM/dd}", receiveDate.Value.Year - 1911, receiveDate.Value));   //代收日 (3碼民國年 yyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
            }

            XmlElements.Add("ReceiveWay", StudentReceive.ReceiveWay);     //代收管道
            XmlElements.Add("ReceiveWay_Name", ChannelHelper.GetChannelName(StudentReceive.ReceiveWay));     //代收管道名稱
            XmlElements.Add("Principal", string.Format("{0} {1}", SchoolRid.AName1, SchoolRid.ATitle1)); // 機關長官姓名 + 機關長官職稱
            XmlElements.Add("Principal_Name", SchoolRid.AName1);           //機關長官姓名
            XmlElements.Add("Principal_Title", SchoolRid.ATitle1);         //機關長官職稱
            XmlElements.Add("Accounting", string.Format("{0} {1}", SchoolRid.AName2, SchoolRid.ATitle2)); //主辦會計姓名 + 主辦會計職稱
            XmlElements.Add("Accounting_Name", SchoolRid.AName2);          //主辦會計姓名 
            XmlElements.Add("Accounting_Title", SchoolRid.ATitle2);        //主辦會計職稱
            XmlElements.Add("Cashier", string.Format("{0} {1}", SchoolRid.AName3, SchoolRid.ATitle3));   //主辦出納姓名 + 主辦出納職稱
            XmlElements.Add("Cashier_Name", SchoolRid.AName3);             //主辦出納姓名
            XmlElements.Add("Cashier_Title", SchoolRid.ATitle3);           //主辦出納職稱

            #region 代收費用別備註
            #region [Old]
            //XmlElements.Add("Memo1", SchoolRid.ReceiveMemoa);              //代收費用別備註1
            //XmlElements.Add("Memo2", SchoolRid.ReceiveMemob);              //代收費用別備註2
            //XmlElements.Add("Memo3", "");                                  //代收費用別備註3
            //XmlElements.Add("Memo4", "");                                  //代收費用別備註4
            //XmlElements.Add("Memo5", "");                                  //代收費用別備註5
            //XmlElements.Add("Memo6", "");                                  //代收費用別備註6
            //XmlElements.Add("Memo7", "");                                  //代收費用別備註7
            //XmlElements.Add("Memo8", "");                                  //代收費用別備註8
            //XmlElements.Add("Memo9", "");                                  //代收費用別備註9
            #endregion

            if (!String.IsNullOrWhiteSpace(SchoolRid.ReceiveMemo))
            {
                int memoIndex = 0;
                string[] lines = SchoolRid.ReceiveMemo.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int idx = 0; idx < lines.Length; idx++)
                {
                    string line = lines[idx].Trim();
                    if (line.Length > 24)
                    {
                        #region 大於 24 個字
                        int startIndex = 0;
                        while (line.Length > startIndex + 24)
                        {
                            int length = line.Length - startIndex;
                            if (length > 24)
                            {
                                memoIndex++;
                                if (memoIndex <= 9)
                                {
                                    string xmlValue = line.Substring(startIndex, 24);
                                    string xmlTag = String.Format("Memo{0}", memoIndex);
                                    XmlElements.Add(xmlTag, xmlValue);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                memoIndex++;
                                if (memoIndex <= 9)
                                {
                                    string xmlValue = line.Substring(startIndex, length);
                                    string xmlTag = String.Format("Memo{0}", memoIndex);
                                    XmlElements.Add(xmlTag, xmlValue);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            startIndex += 24;
                        }

                        memoIndex++;
                        if (memoIndex <= 9)
                        {
                            string xmlValue = line.Substring(startIndex);
                            string xmlTag = String.Format("Memo{0}", memoIndex);
                            XmlElements.Add(xmlTag, xmlValue);
                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                    else
                    {
                        memoIndex++;
                        if (memoIndex <= 9)
                        {
                            string xmlValue = line;
                            string xmlTag = String.Format("Memo{0}", memoIndex);
                            XmlElements.Add(xmlTag, xmlValue);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            #endregion

            //Memo10			"*全家、統一及萊爾富超商繳費，需自付手續費?元，繳費期限?。"
            //Memo11			"*郵局繳費無需付手續費。繳費期限?。"
            //Memo12            "*本單據至台銀臨櫃繳費，需自付手續費?元，繳費期限: ?。"
            //Memo14			"利用各行庫自動櫃員機轉帳,轉入行請點選：臺灣銀行(代號004)、輸入轉入帳號：?、轉入金額：?。"
            //Ad1			    繳費單廣告管理1
            //Ad2			    繳費單廣告管理2
            //Ad3			    繳費單廣告管理3
            //Ad4			    繳費單廣告管理4
            //Ad5			    繳費單廣告管理5
            //Ad6			    繳費單廣告管理6

            XmlElements.Add("BankName", string.Empty);           //主辦分行名稱
            XmlElements.Add("BankTel", string.Empty);             //主辦分行電話

            #region SMFee : 超商自付手續費
            {
                int smFee = 0;
                xmlWriter.WriteElementString("SMFee", smFee.ToString());     //超商繳款須自付 處理費
            }
            #endregion

            #region BankFee : 銀行(臨櫃)自付手續費
            {
                int bankFee = 0;
                xmlWriter.WriteElementString("BankFee", bankFee.ToString());   //臨櫃繳款須自付 處理費
            }
            #endregion


            XmlElements.Add("CancelNoLen", StudentReceive.CancelNo.Length.ToString());           //銷帳編號幾碼
            GenMultiElement(xmlWriter, XmlElements);

            #region 銀行(臨櫃)條碼 [銷編16碼(14碼則後補2碼空白)+金額]
            if (!String.IsNullOrWhiteSpace(StudentReceive.CancelAtmno) && StudentReceive.ReceiveAmount != null)
            {
                #region TAG 說明
                // BankBarcode : 銀行(臨櫃)條碼
                // BankBarcode_text : 銀行(臨櫃)條碼文字
                #endregion

                string bbarcode = String.Format("{0}{1:0}", StudentReceive.CancelNo.PadRight(16, ' '), StudentReceive.ReceiveAmount.Value);
                GenBarcodeElement(xmlWriter, "BankBarcode", bbarcode);
                GenBarcodeTextElement(xmlWriter, "BankBarcode_text", bbarcode);

                string cancelNoBarcode = StudentReceive.CancelNo;
                GenBarcodeElement(xmlWriter, "BankBarcode1", cancelNoBarcode);
                GenBarcodeTextElement(xmlWriter, "BankBarcode1_text", cancelNoBarcode);

                string amountBarcode = StudentReceive.ReceiveAmount.Value.ToString("0");
                GenBarcodeElement(xmlWriter, "ReceiveAmountBarcode", amountBarcode);
                GenBarcodeTextElement(xmlWriter, "ReceiveAmountBarcode_text", amountBarcode);
            }
            #endregion

            #region 超商三段條碼
            //if (smChannelWay != null && !String.IsNullOrWhiteSpace(StudentReceive.CancelSMNo) && StudentReceive.ReceiveSMAmount != null)
            //{
            //    #region TAG 說明
            //    // SMBarcode1 : 超商第一段條碼
            //    // SMBarcode2 : 超商第二段條碼
            //    // SMBarcode3 : 超商第三段條碼
            //    // SMNumber1  : 超商第一段條碼文字
            //    // SMNumber2  : 超商第二段條碼文字
            //    // SMNumber3  : 超商第三段條碼文字
            //    #endregion

            //    ChannelHelper helper = new ChannelHelper();

            //    if (payDuDate != null && SchoolRid.ExtraDays > 0)
            //    {
            //        payDuDate = payDuDate.Value.AddDays(SchoolRid.ExtraDays);
            //    }

            //    string zbarcode1 = null ? String.Empty : helper.GenSMBarcode1(payDuDate.Value, smChannelWay.BarcodeId);
            //    string zbarcode2 = helper.GenSMBarcode2(StudentReceive.CancelSMNo);
            //    string zbarcode3 = helper.GenSMBarcode3(zbarcode1, zbarcode2, StudentReceive.ReceiveSMAmount.Value);

            //    GenBarcodeElement(xmlWriter, "SMBarcode1", zbarcode1);
            //    GenBarcodeElement(xmlWriter, "SMBarcode2", zbarcode2);
            //    GenBarcodeElement(xmlWriter, "SMBarcode3", zbarcode3);

            //    string zbarcode1Text = zbarcode1;
            //    string zbarcode2Text = zbarcode2;
            //    string zbarcode3Text = zbarcode3;
            //    GenBarcodeTextElement(xmlWriter, "SMNumber1", zbarcode1Text);
            //    GenBarcodeTextElement(xmlWriter, "SMNumber2", zbarcode2Text);
            //    GenBarcodeTextElement(xmlWriter, "SMNumber3", zbarcode3Text);
            //}
            #endregion

            #region 郵局三段條碼
            //#region TAG 說明
            //// POBarcode1 : 郵局第一段條碼
            //// POBarcode2 : 郵局第二段條碼
            //// POBarcode3 : 郵局第三段條碼
            //// PONumber1 : 郵局第一段條碼文字
            //// PONumber2 : 郵局第二段條碼文字
            //// PONumber3 : 郵局第三段條碼文字
            //#endregion

            ////郵局三段條碼規則
            ////barcode1:劃撥帳號
            ////barcode2:繳費期限(yyyMMdd)虛擬帳號
            ////barcode3:金額

            //string pbarcode1 = StudentReceive.CancelPONo;
            //string pbarcode2 = SchoolRid.PayDate + StudentReceive.CancelPONo;
            //string pbarcode3 = String.Format("{0:0}", 0);  //ToDo 沒有郵局的應收金額(StudentReceive.ReceivePOAmount)

            //GenBarcodeElement(xmlWriter, "POBarcode1", pbarcode1);
            //GenBarcodeElement(xmlWriter, "POBarcode2", pbarcode2);
            //GenBarcodeElement(xmlWriter, "POBarcode3", pbarcode3);

            //string pbarcode1Text = pbarcode1;
            //string pbarcode2Text = pbarcode2;
            //string pbarcode3Text = String.Format("{0:#,##0}", 0);  //ToDo 沒有郵局的應收金額(StudentReceive.ReceivePOAmount)

            //GenBarcodeTextElement(xmlWriter, "PONumber1", pbarcode1Text);
            //GenBarcodeTextElement(xmlWriter, "PONumber2", pbarcode2Text);
            //GenBarcodeTextElement(xmlWriter, "PONumber3", pbarcode3Text);
            #endregion

            //</Fields> 
            xmlWriter.WriteEndElement();

            //Write the XML to file and close the xmlWriter.
            xmlWriter.Flush();

            return true;

        }
        #endregion

        #region [MDY:20160519] 依據指定長度參數拆成字串陣列
        #region [Old]
        //public string[] split24(string data)
        //{
        //    List<string> buffs = new List<string>();
        //    Int32 idx = 0;
        //    int i = 1;
        //    while (true)
        //    {
        //        try
        //        {
        //            if ((data.Length - 24 * i) >= 0)
        //            {
        //                buffs.Add(data.Substring(idx, 24));
        //            }
        //            else
        //            {
        //                buffs.Add(data.Substring(idx, data.Length - 24 * (i - 1)));
        //                break;
        //            }
        //            idx = idx + 24;
        //            i++;
        //        }
        //        catch (Exception)
        //        {
        //            break;
        //        }
        //    }

        //    return buffs.ToArray();
        //}

        //private string[] split22(string data)
        //{
        //    List<string> buffs = new List<string>();
        //    Int32 idx = 0;
        //    int i = 1;
        //    while (true)
        //    {
        //        try
        //        {
        //            if ((data.Length - 22 * i) >= 0)
        //            {
        //                buffs.Add(data.Substring(idx, 22));
        //            }
        //            else
        //            {
        //                buffs.Add(data.Substring(idx, data.Length - 22 * (i - 1)));
        //                break;
        //            }
        //            idx = idx + 22;
        //            i++;
        //        }
        //        catch (Exception)
        //        {
        //            break;
        //        }
        //    }

        //    return buffs.ToArray();
        //}
        #endregion

        /// <summary>
        /// 依據指定長度參數拆成字串陣列
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string[] SplitByLength(string txt, int length)
        {
            List<string> lines = new List<string>();
            int txtLength = txt.Length;
            int startIndex = 0;
            while (startIndex < txtLength)
            {
                if ((txtLength - startIndex) >= length)
                {
                    lines.Add(txt.Substring(startIndex, length));
                }
                else
                {
                    lines.Add(txt.Substring(startIndex, txtLength - startIndex));
                    break;
                }
                startIndex += length;
            }

            return lines.ToArray();
        }
        #endregion

        /// <summary>
        /// 產生Xml節點
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="xmlEmenents">要產生的節點陣列</param>
        private void GenMultiElement(XmlWriter writer, KeyValueList<string> xmlEmenents)
        {
            foreach (KeyValue<string> element in xmlEmenents)
            {
                #region [MDY:20160521] 判斷精簡 Xml 產生且節點無值時，不產生節點
                if (this.IsSimplify && String.IsNullOrWhiteSpace(element.Value))
                {
                    continue;
                }
                #endregion

                GenXmlElement(writer, element.Key, element.Value);
            }
        }

        /// <summary>
        /// 產生相關節點
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="fieldValue">欄位值</param>
        public void GenXmlElement(XmlWriter writer, string fieldName, string fieldValue)
        {
            #region [MDY:20160521] 判斷精簡 Xml 產生且節點無值時，不產生節點
            if (this.IsSimplify && String.IsNullOrWhiteSpace(fieldValue))
            {
                return;
            }
            #endregion

            string fontSizeName = "FontSize";
            string alignName = "Align";
            string alignValue = string.Empty;

            List<string> listFontSize = new List<string>();
            listFontSize.Add("7");
            listFontSize.Add("8");
            listFontSize.Add("9");
            listFontSize.Add("10");
            listFontSize.Add("12");
            //listFontSize.Add("14");
            //listFontSize.Add("16");

            List<string> listAlign = new List<string>();
            listAlign.Add("l");

            #region [MDY:20160521] 土銀沒有置中
            //listAlign.Add("c");
            #endregion

            listAlign.Add("r");

            string newFieldName = string.Empty;
            foreach (string fontSize in listFontSize)
            {
                foreach (string align in listAlign)
                {
                    newFieldName = fieldName + "_" + fontSize + "_" + align;
                    switch (align)
                    {
                        case "l":
                            alignValue = "left";
                            break;
                        case "c":
                            alignValue = "center";
                            break;
                        case "r":
                            alignValue = "right";
                            break;
                    }
                    GenElement(writer, newFieldName, fieldValue, fontSizeName, fontSize, alignName, alignValue);
                }
            }
        }

        /// <summary>
        /// 產生條碼節點
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="fieldValue">欄位值</param>
        public void GenBarcodeElement(XmlWriter writer, string fieldName, string fieldValue)
        {
            #region [MDY:20160521] 判斷精簡 Xml 產生且節點無值時，不產生節點
            if (this.IsSimplify && String.IsNullOrWhiteSpace(fieldValue))
            {
                return;
            }
            #endregion

            string fontName = "FontName";
            string fontValue = "code39";
            string alignName = "Align";
            string alignValue = "center";

            writer.WriteStartElement(fieldName);
            writer.WriteAttributeString(fontName, fontValue);
            writer.WriteAttributeString(alignName, alignValue);

            #region [MDY:20160521] 條碼只能是大寫英文或數字
            writer.WriteString(fieldValue == null ? String.Empty : fieldValue.ToUpper());
            #endregion

            writer.WriteEndElement();
        }

        /// <summary>
        /// 產生條碼說明節點
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="fieldValue">欄位值</param>
        public void GenBarcodeTextElement(XmlWriter writer, string fieldName, string fieldValue)
        {
            #region [MDY:20160521] 判斷精簡 Xml 產生且節點無值時，不產生節點
            if (this.IsSimplify && String.IsNullOrWhiteSpace(fieldValue))
            {
                return;
            }
            #endregion

            string alignName = "Align";
            string alignValue = "center";

            writer.WriteStartElement(fieldName);
            writer.WriteAttributeString(alignName, alignValue);
            writer.WriteString(fieldValue);
            writer.WriteEndElement();
        }

        /// <summary>
        /// 產生 XML Element
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="fieldValue">欄位值</param>
        /// <param name="fontSizeName">FontSize名稱</param>
        /// <param name="fontSizeValue">FontSize值</param>
        /// <param name="alignName">Align名稱</param>
        /// <param name="alignValue">Align值</param>
        public void GenElement(XmlWriter writer, string fieldName, string fieldValue, string fontSizeName, string fontSizeValue, string alignName, string alignValue)
        {
            #region [MDY:20160521] 判斷精簡 Xml 產生且節點無值時，不產生節點
            if (this.IsSimplify && String.IsNullOrWhiteSpace(fieldValue))
            {
                return;
            }
            #endregion

            writer.WriteStartElement(fieldName);
            writer.WriteAttributeString(fontSizeName, fontSizeValue);
            writer.WriteAttributeString(alignName, alignValue);
            writer.WriteString(fieldValue);
            writer.WriteEndElement();
        }
        
		/// <summary>
		/// 取得金額的每位數中文大寫陣列
		/// </summary>
		/// <param name="money">要轉換的金額</param>
		/// <returns>傳回金額的每位數中文陣列</returns>
		public string[] GetChineseArray(long money)
        {
            if (money < 0)
            {
                money = money * -1;
            }

            string[] chineseNumber = { "零", "壹", "貳", "叁", "肆", "伍", "陸", "柒", "捌", "玖" };
            //string inputNumber = money.ToString("000000000");
            string inputNumber = money.ToString("00000");
            string[] ret = new string[inputNumber.Length];
            int idx = 0;
            foreach (char c in inputNumber)
            {
                ret[idx] = chineseNumber[(int)(c - '0')];
                idx++;
            }
            return ret;
        }

        #region [MDY:20191014] M201910_01 (2019擴充案+小修正) 產生測試繳款單
        /// <summary>
        /// 產生測試繳款單 Xml 資料
        /// </summary>
        /// <param name="xmlWriter"></param>
        /// <param name="printDate"></param>
        /// <param name="pageNo"></param>
        /// <param name="cancelNo"></param>
        /// <param name="receiveAmount"></param>
        /// <param name="payDueDate"></param>
        /// <param name="channelData"></param>
        public void GenSMBarcodePDFXmlData(XmlWriter xmlWriter, DateTime printDate, int pageNo, string cancelNo, decimal receiveAmount, DateTime payDueDate, ChannelWayEntity channelData)
        {
            #region <Fields Page='1'>
            xmlWriter.WriteStartElement("Fields");
            xmlWriter.WriteAttributeString("Page", pageNo.ToString());
            #endregion

            #region <Year>2019</Year>
            xmlWriter.WriteStartElement("Year");
            xmlWriter.WriteString(printDate.Year.ToString());
            xmlWriter.WriteEndElement();
            #endregion

            #region <CYear>108</CYear>
            xmlWriter.WriteStartElement("CYear");
            xmlWriter.WriteString(Common.GetTWDate7(printDate).Substring(0, 3));
            xmlWriter.WriteEndElement();
            #endregion

            #region <Month>09</Month>
            xmlWriter.WriteStartElement("Month");
            xmlWriter.WriteString(printDate.Month.ToString());
            xmlWriter.WriteEndElement();
            #endregion

            #region <Day>29</Day>
            xmlWriter.WriteStartElement("Day");
            xmlWriter.WriteString(printDate.Day.ToString());
            xmlWriter.WriteEndElement();
            #endregion

            #region <PrintDate>2014/05/15</PrintDate>
            xmlWriter.WriteStartElement("PrintDate");
            xmlWriter.WriteString(printDate.ToString("yyyy/MM/dd"));
            xmlWriter.WriteEndElement();
            #endregion

            #region MultiElement
            KeyValueList<string> XmlElements = new KeyValueList<string>();

            #region 繳款期限
            XmlElements.Add("PayDueDate", String.Format("{0:yyyy/MM/dd}", payDueDate));   //繳款期限 (4碼西元年 yyyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
            xmlWriter.WriteElementString("PayDueDate", String.Format("{0:yyyy/MM/dd}", payDueDate));   //繳款期限 (4碼西元年 yyyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)
            XmlElements.Add("PayDueDateY", String.Format("{0:yyyy}", payDueDate));
            xmlWriter.WriteElementString("PayDueDateY", String.Format("{0:yyyy}", payDueDate));
            XmlElements.Add("PayDueDateCY", String.Format("{0}", payDueDate.Year - 1911));
            xmlWriter.WriteElementString("PayDueDateCY", String.Format("{0}", payDueDate.Year - 1911));
            XmlElements.Add("PayDueDateM", String.Format("{0:MM}", payDueDate));
            xmlWriter.WriteElementString("PayDueDateM", String.Format("{0:MM}", payDueDate));
            XmlElements.Add("PayDueDateD", String.Format("{0:dd}", payDueDate));
            xmlWriter.WriteElementString("PayDueDateD", String.Format("{0:dd}", payDueDate));
            XmlElements.Add("CPayDueDate", String.Format("{0:000}/{1:MM/dd}", payDueDate.Year - 1911, payDueDate));
            xmlWriter.WriteElementString("CPayDueDate", String.Format("{0:000}/{1:MM/dd}", payDueDate.Year - 1911, payDueDate));   //繳款期限 (3碼民國年 yyy + "/" + 2碼月 MM + "/" + 2碼日期 dd)

            XmlElements.Add("TaPayDueDate", String.Format("{0:yyyy年MM月dd日}", payDueDate));    //中文繳款期限 (4碼西元年 yyyy + "年" + 2碼月 MM + "月" + 2碼日期 dd + "日")
            xmlWriter.WriteElementString("TaPayDueDate", String.Format("{0:yyyy年MM月dd日}", payDueDate));   //中文繳款期限 (4碼西元年 yyyy + "年" + 2碼月 MM + "月" + 2碼日期 dd + "日")
            XmlElements.Add("TaCPayDueDate", String.Format("{0:000}年{1:MM月dd日}", payDueDate.Year - 1911, payDueDate));  //中文繳款期限 (3碼民國年 yyy + "年" + 2碼月 MM + "月" + 2碼日期 dd+ "日")
            xmlWriter.WriteElementString("TaCPayDueDate", String.Format("{0:000}年{1:MM月dd日}", payDueDate.Year - 1911, payDueDate));   //中文繳款期限 (3碼民國年 yyy + "年" + 2碼月 MM + "月" + 2碼日期 dd+ "日")
            #endregion

            #region 學生姓名
            XmlElements.Add("StudentName", String.Format("測試{0:00}", pageNo));
            #endregion

            #region 中文金額
            //Ta1			    應繳金額萬元的國字
            //Ta2			    應繳金額千元的國字
            //Ta3			    應繳金額百元的國字
            //Ta4			    應繳金額十元的國字
            //Ta5			    應繳金額個元的國字
            //TaAmount		    應繳金額的國字

            long billAmount = Convert.ToInt64(receiveAmount);
            string[] CMoney = this.GetChineseArray(billAmount);
            string[] tags = new string[] { "Ta5", "Ta4", "Ta3", "Ta2", "Ta1" };
            string CTaAmount = null;
            if (billAmount < 0)
            {
                CTaAmount = "負" + Common.GetChineseMoney(billAmount * -1);
            }
            else if (billAmount == 0)
            {
                CTaAmount = "零";
            }
            else
            {
                CTaAmount = Common.GetChineseMoney(billAmount);
            }
            for (int idx = 0; idx < tags.Length; idx++)
            {
                xmlWriter.WriteStartElement(tags[idx]);
                xmlWriter.WriteString(CMoney[idx]);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteElementString("TaAmount", CTaAmount);       //應繳金額的國字
            XmlElements.Add("TaAmount", CTaAmount);
            #endregion

            #region 銷帳編號
            XmlElements.Add("CancelNo", cancelNo);
            xmlWriter.WriteElementString("Cancel_No", cancelNo);
            #endregion

            #region 繳費金額合計
            XmlElements.Add("ReceiveAmount", receiveAmount.ToString("###,###,###,##0"));
            #endregion

            GenMultiElement(xmlWriter, XmlElements);
            #endregion

            #region 銀行(臨櫃)條碼 [銷編16碼(14碼則後補2碼空白)+金額]
            {
                #region TAG 說明
                // BankBarcode : 銀行(臨櫃)條碼
                // BankBarcode_text : 銀行(臨櫃)條碼文字
                #endregion

                string bbarcode = String.Format("{0}{1:0}", cancelNo.PadRight(16, ' '), receiveAmount);
                GenBarcodeElement(xmlWriter, "BankBarcode", bbarcode);
                GenBarcodeTextElement(xmlWriter, "BankBarcode_text", bbarcode);

                string cancelNoBarcode = cancelNo;
                GenBarcodeElement(xmlWriter, "BankBarcode1", cancelNoBarcode);
                GenBarcodeTextElement(xmlWriter, "BankBarcode1_text", cancelNoBarcode);

                string amountBarcode = receiveAmount.ToString("0");
                GenBarcodeElement(xmlWriter, "ReceiveAmountBarcode", amountBarcode);
                GenBarcodeTextElement(xmlWriter, "ReceiveAmountBarcode_text", amountBarcode);
            }
            #endregion

            #region SMFee : 超商自付手續費
            {
                int smFee = 0;
                if (channelData != null && channelData.IncludePay == "0" && channelData.ChannelCharge != null)
                {
                    //有超商管道 且 不包含手續費(外加) 且有手續費金額
                    smFee = Convert.ToInt32(channelData.ChannelCharge.Value);
                }
                xmlWriter.WriteElementString("SMFee", smFee.ToString());     //超商繳款須自付 處理費
            }
            #endregion

            #region 超商三段條碼
            {
                #region TAG 說明
                // SMBarcode1 : 超商第一段條碼
                // SMBarcode2 : 超商第二段條碼
                // SMBarcode3 : 超商第三段條碼
                // SMNumber1  : 超商第一段條碼文字
                // SMNumber2  : 超商第二段條碼文字
                // SMNumber3  : 超商第三段條碼文字
                #endregion

                ChannelHelper helper = new ChannelHelper();

                //超商繳款期限
                DateTime smPayDueDate = payDueDate;

                string zbarcode1 = helper.GenSMBarcode1(smPayDueDate, channelData.BarcodeId);
                string zbarcode2 = helper.GenSMBarcode2(cancelNo);
                string zbarcode3 = helper.GenSMBarcode3(zbarcode1, zbarcode2, payDueDate, receiveAmount);

                this.GenBarcodeElement(xmlWriter, "SMBarcode1", zbarcode1);
                this.GenBarcodeElement(xmlWriter, "SMBarcode2", zbarcode2);
                this.GenBarcodeElement(xmlWriter, "SMBarcode3", zbarcode3);

                string zbarcode1Text = zbarcode1;
                string zbarcode2Text = zbarcode2;
                string zbarcode3Text = zbarcode3;
                this.GenBarcodeTextElement(xmlWriter, "SMNumber1", zbarcode1Text);
                this.GenBarcodeTextElement(xmlWriter, "SMNumber2", zbarcode2Text);
                this.GenBarcodeTextElement(xmlWriter, "SMNumber3", zbarcode3Text);
            }
            #endregion

            #region </Fields>
            xmlWriter.WriteEndElement();
            #endregion

            //Write the XML to file and close the xmlWriter.
            xmlWriter.Flush();
        }
        #endregion
    }
}