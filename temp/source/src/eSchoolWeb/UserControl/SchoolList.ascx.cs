using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Entities;

namespace eSchoolWeb.UserControl
{
    public partial class SchoolList : System.Web.UI.UserControl
    {
        private bool _IsForStudent = false;
        /// <summary>
        /// 取得或指定是否在學生專區使用 (預設 false)
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">將此屬性設定為不屬於 UIModeEnum 列舉型別值</exception>
        [DefaultValue(false)]
        [Themeable(false)]
        public bool IsForStudent
        {
            get
            {
                return _IsForStudent;
            }
            set
            {
                _IsForStudent = value;
            }
        }

        private bool genTab(out string html_string, out string msg)
        {
            bool rc = false;
            html_string = "";
            msg = "";

            string[] tab_list = { "大專院校", "高中職", "國中小", "幼兒園" };
            ArrayList datas = new ArrayList();
            List<SchoolConfigView> tab1 = new List<SchoolConfigView>();
            List<SchoolConfigView> tab2 = new List<SchoolConfigView>();
            List<SchoolConfigView> tab3 = new List<SchoolConfigView>();
            List<SchoolConfigView> tab4 = new List<SchoolConfigView>();

            SchoolConfigView[] school_rtypes = null;
            if (!getSchoolData(out school_rtypes, out msg))
            {
                
            }
            else
            {
                #region 轉換成分類array
                foreach (SchoolConfigView school_rtype in school_rtypes)
                {
                    if (school_rtype.CorpType == "1")//大專院校
                    {
                        tab1.Add(school_rtype);
                    }
                    if (school_rtype.CorpType == "2")//高中職
                    {
                        tab2.Add(school_rtype);
                    }
                    if (school_rtype.CorpType == "3")//國中小
                    {
                        tab3.Add(school_rtype);
                    }
                    if (school_rtype.CorpType == "4")//幼兒園
                    {
                        tab4.Add(school_rtype);
                    }
                }
                datas.Add(tab1.ToArray());
                datas.Add(tab2.ToArray());
                datas.Add(tab3.ToArray());
                datas.Add(tab4.ToArray());
                #endregion
            }

            html_string = genTabTitle(tab_list);

            html_string += genContent(datas);

            rc = true;
            return rc;
        }

        private string genTabTitle(string[] tab_list)
        {
            if(tab_list==null || tab_list.Length<=0)
            {
                return "";
            }
            int tabs = tab_list.Length;

            string css_class = "tabs";
            string html = "";
            html += string.Format("<ul class=\"{0}\">", css_class);
            for (int i = 0; i < tabs;i++)
            {
                html += string.Format("<li><a href=\"#tab{0}\">{1}</a></li>", i + 1, tab_list[i]);
            }
            html += string.Format("</ul>");
            return html;
        }

        private string genContent(ArrayList data)
        {
            #region [MDY:20200815] M202008_02 取得「顯示使用者密碼的學校代碼」參數設定值 (2020819_01)
            #region [OLD]
            //#region [MDY:20160517] 取得學生專區登入顯示密碼字眼的學校代號
            //List<string> schoolIds = null;
            //{
            //    string value = System.Configuration.ConfigurationManager.AppSettings.Get("PWORD_SCHOOL_ID");
            //    if (!String.IsNullOrWhiteSpace(value))
            //    {
            //        string[] ids = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //        schoolIds = new List<string>(ids.Length);
            //        foreach (string id in ids)
            //        {
            //            schoolIds.Add(id.Trim());
            //        }
            //    }
            //}
            //#endregion
            #endregion

            List<string> schoolIds = null;
            {
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.FIXVERIFY_SCHOOLID);
                KeyValueList<OrderByEnum> orderbys = null;
                ConfigEntity config = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, orderbys, out config);
                if (xmlResult.IsSuccess && config != null && !String.IsNullOrWhiteSpace(config.ConfigValue))
                {
                    string[] values = config.ConfigValue.Trim().Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    schoolIds = new List<string>(values.Length);
                    foreach (string value in values)
                    {
                        schoolIds.Add(value.Trim());
                    }
                }
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 取得是否使用英文介面
            bool isEngUI = "en-US".Equals(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(), StringComparison.CurrentCultureIgnoreCase);
            #endregion

            string html = "";
            //int recordsofline = 3;
            int tabs = data.Count;
            string tab_contenter_css = "tab_container";
            string tab_content_css = "tab_content";
            html += string.Format("<div class=\"{0}\">", tab_contenter_css);
            for (int i = 0; i < tabs; i++)
            {
                #region 每一個tab的內容
                if (data[i] == null)
                {
                    html += string.Format("<div id=\"tab{0}\" class=\"{1}\">", i + 1, tab_content_css);
                    html += string.Format("<table>");
                    html += string.Format("<tr>");
                    html += string.Format("<td colspan=\"3\">查無資料</td>");
                    html += string.Format("</tr>");
                    html += string.Format("</table>");
                    html += string.Format("</div>");
                    continue;
                }

                #region 直向
                #region 先過濾重覆的學校
                SchoolConfigView[] schools = (SchoolConfigView[])data[i];
                string last_sch_identy = "";
                List<SchoolConfigView> listSchools = new List<SchoolConfigView>();
                foreach (SchoolConfigView school in schools)
                {
                    if (last_sch_identy != school.SchIdenty)
                    {
                        last_sch_identy = school.SchIdenty;
                        listSchools.Add(school);
                    }
                }
                #endregion

                html += string.Format("<div id=\"tab{0}\" class=\"{1}\">", i + 1, tab_content_css);
                html += string.Format("<table id=\"table1\" class=\"normal\" summary=\"表格_修改\" width=\"100%\">");

                Int32 idxSchool = 0;
                Int32 lines = 0;
                Int32 tdCount = 3;      //每一行有幾個 td
                Int32 trCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(listSchools.Count) / Convert.ToDouble(tdCount)));

                for (int trIdx = 0; trIdx < trCount; trIdx++)
                {
                    lines++;
                    string css_class = lines % 2 == 0 ? "light" : "dark";
                    html += string.Format("<tr class='{0}'>", css_class);
                    for (int tdIdx = 0; tdIdx < tdCount; tdIdx++)
                    {
                        idxSchool = (lines + (tdIdx * trCount)) - 1;

                        if (idxSchool < listSchools.Count)
                        {
                            #region [MDY:20200815] M202008_02 依據驗證欄位種類、顯示使用者密碼的學校代碼參數產生 chooseSchool() 的參數 (2020806_01)(2020819_01)
                            #region [OLD]
                            //#region [MDY:20160517] 判斷是否為顯示密碼字眼的學校代號
                            //string schIdenty = listSchools[idxSchool].SchIdenty.Trim();
                            //if (schoolIds != null && schoolIds.Contains(schIdenty))
                            //{
                            //    html += string.Format("<td><a href=\"javascript:void(0);\" onclick=\"javascript:chooseSchool('{0}','{1}','{2}');\">{1}</a></td>", schIdenty, listSchools[idxSchool].SchName.Trim(), "P");
                            //}
                            //else
                            //{
                            //    html += string.Format("<td><a href=\"javascript:void(0);\" onclick=\"javascript:chooseSchool('{0}','{1}','{2}');\">{1}</a></td>", schIdenty, listSchools[idxSchool].SchName.Trim(), listSchools[idxSchool].LoginKeyType);
                            //}
                            //#endregion
                            #endregion

                            SchoolConfigView school = listSchools[idxSchool];
                            string schIdenty = school.SchIdenty.Trim();
                            string loginKeyKind = "Y".Equals(school.LoginKeyType, StringComparison.CurrentCultureIgnoreCase) ? "B" : "I";
                            if (schoolIds != null && schoolIds.Contains(schIdenty))
                            {
                                loginKeyKind += "2P";
                            }

                            #region [MDY:202203XX] 2022擴充案 改用 GetSchName() 取得學校名稱
                            html += string.Format("<td><a href=\"javascript:void(0);\" onclick=\"javascript:chooseSchool('{0}','{1}','{2}');\">{1}</a></td>", schIdenty, school.GetSchName(isEngUI), loginKeyKind);
                            #endregion

                            #endregion
                        }
                        else
                        {
                            html += string.Format("<td>&nbsp;</td>");
                        }
                    }
                    html += string.Format("</tr>");
                }
                html += string.Format("</table>");
                html += string.Format("</div>");
                #endregion

                #region [OLD] 橫向
                //SchoolConfigView[] schools = (SchoolConfigView[])data[i];
                //html += string.Format("<div id=\"tab{0}\" class=\"{1}\">", i + 1, tab_content_css);
                //html += string.Format("<table id=\"table1\" class=\"normal\" summary=\"表格_修改\" width=\"100%\">");
                //int count = 0;
                //string last_sch_identy = "";
                //Int32 idx = 0;
                //Int32 lines = 0;
                //bool new_line = true;
                //foreach (SchoolConfigView school in schools)
                //{
                //    if (last_sch_identy != school.SchIdenty)
                //    {
                //        if (count == 0)
                //        {
                //            lines++;
                //            string css_class = "";
                //            if ((lines % 2) == 0)
                //            {
                //                css_class = "light";
                //            }
                //            else
                //            {
                //                css_class = "dark";
                //            }
                //            html += string.Format("<tr class='{0}'>", css_class);
                //        }

                //        html += string.Format("<td><a href=\"javascript:void(0);\" onclick=\"javascript:chooseSchool('{0}','{1}','{2}');\">{1}</a></td>", school.SchIdenty.Trim(), school.SchName.Trim(), listSchools[idxSchool].LoginKeyType);
                //        count++;

                //        last_sch_identy = school.SchIdenty;

                //        //滿足一列就加上/tr
                //        if (count == recordsofline)
                //        {
                //            html += string.Format("</tr>");
                //            count = 0;
                //        }
                //    }
                //}
                ////補不足的td
                //if (count != recordsofline)
                //{
                //    while (count != recordsofline)
                //    {
                //        html += string.Format("<td><a href=\"javascript:void(0);\" >{0}</a></td>", "&nbsp");
                //        count++;
                //    }
                //    html += string.Format("</tr>");
                //}
                //html += string.Format("</table>");
                //html += string.Format("</div>");
                #endregion
                #endregion
            }
            html += string.Format("</div>");
            return html;
        }

        private bool getSchoolData(out SchoolConfigView[] school_rtypes, out string msg)
        {
            bool rc = false;
            school_rtypes = null;
            msg = "";

            Expression where = null;
            if (IsForStudent)
            {
                //不特別指定為 N 的都視為 Y
                where = new Expression(SchoolConfigView.Field.OpenStudentArea, RelationEnum.NotEqual, 'N');
            }
            else
            {
                where = new Expression();
            }


            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(SchoolConfigView.Field.CorpType, OrderByEnum.Asc);
            orderbys.Add(SchoolConfigView.Field.SchIdenty, OrderByEnum.Asc);
            orderbys.Add(SchoolConfigView.Field.ReceiveType, OrderByEnum.Asc);

            XmlResult xmlResult = DataProxy.Current.SelectAll<SchoolConfigView>(this.Page, where, orderbys, out school_rtypes);
            if (!xmlResult.IsSuccess)
            {
                msg = string.Format("[getSchoolData] 取得學校資料發生錯誤，錯誤訊息={0}", xmlResult.Message);
            }
            else
            {
                rc = true;
            }

            return rc;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string html_string = "";
                string msg = "";
                if(!genTab(out html_string,out msg))
                {
                    this.Literal1.Text = msg;
                }
                else
                {
                    html_string = string.Format("<div class=\"abgne_tab\" id=\"abgne_tab\" style=\"display:none;overflow-x:scroll;overflow-y:scroll; height:350px; width:700px\"><div style=\"width:100%; height:25px; text-align:center;\"><a href=\"javascript:void(0);\" onclick=\"$.unblockUI();\">關閉</a></div>{0}</div>", html_string);
                    this.Literal1.Text = html_string;
                }
            }
        }
    }
}