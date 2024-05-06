using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Fuju.DB.Data;
using Entities;
using System.Collections;
namespace eSchoolWeb.UserControl
{
    public partial class UCContact : System.Web.UI.UserControl
    {
        private bool getContactata(out ContactView[] contacts, out string msg)
        {
            bool rc = false;
            contacts = null;
            msg = "";

            Expression where = new Expression();
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ContactView.Field.CorpType, OrderByEnum.Asc);
            orderbys.Add(ContactView.Field.sch_identy, OrderByEnum.Asc);

            XmlResult xmlResult = DataProxy.Current.SelectAll<ContactView>(this.Page, where, orderbys, out contacts);
            if (!xmlResult.IsSuccess)
            {
                msg = string.Format("[getSchoolData] 取得各主辦分行資料發生錯誤，錯誤訊息={0}", xmlResult.Message);
            }
            else
            {
                rc = true;
            }

            return rc;
        }

        private string genPage(string[] tab_titles, ArrayList datas)
        {
            //本頁面用Tabs來呈現

            string html_string = "";
            //
            html_string += string.Format("<div class=\"abgne_tab\">");

            string tmp1 = "";
            #region 頁籤的部分
            tmp1 += string.Format("<ul class=\"tabs\">");
            for (int i = 0; i < tab_titles.Length; i++)
            {
                tmp1 += string.Format("<li><a href=\"#tab{0}\">{1}</a></li>", i + 1, tab_titles[i]);
            }
            tmp1 += string.Format("</ul>");
            #endregion
            html_string += tmp1;

            string tmp2 = "";
            #region 內容的部分
            tmp2 += string.Format("<div class=\"tab_container\">");

            for (int i = 0; i < tab_titles.Length; i++)
            {
                string tmp3 = "";
                #region 每一個頁籤的內容
                tmp3 += string.Format("<div id=\"tab{0}\" class=\"tab_content\">", i + 1);

                string tmp4 = "";
                ContactView[] contacts = null;
                if (i < datas.Count)
                {
                    contacts = (ContactView[])datas[i];
                }

                #region 呈現
                Int32 idx = 0;
                tmp4 += string.Format("<table id=\"table1\" class=\"normal\" summary=\"表格_修改\" width=\"100%\">");
                if (contacts == null || contacts.Length <= 0)
                {
                    tmp4 += string.Format("<tr><th>查無資料</th></tr>");
                }
                else
                {
                    #region 每一筆資訊
                    foreach (ContactView contact in contacts)
                    {
                        idx++;
                        string css_class = "";
                        if((idx % 2)==0)
                        {
                            css_class = "light";
                        }
                        else
                        {
                            css_class = "dark";
                        }
                        string sch_name = contact.sch_name.Trim();
                        string bank_name = contact.BANKSNAME.Trim();
                        string tel = contact.Tel.Trim();
                        tmp4 += string.Format("<tr class='{3}'><td>{0}</td><td>{1}</td><td>{2}</td></tr>", sch_name, bank_name,tel,css_class);
                    }
                    #endregion
                }
                tmp4 += string.Format("</table>");
                #endregion
                tmp3 += tmp4;
                tmp3 += string.Format("</div>");
                #endregion
                tmp2 += tmp3;
            }

            tmp2 += string.Format("</div>");
            #endregion
            html_string += tmp2;

            html_string += string.Format("</div>");
            return html_string;
        }

        private string genEmptyPage()
        {
            return string.Format("<div class=\"abgne_tab\" id=\"abgne_tab\" style=\"display:none;overflow-x:scroll;overflow-y:scroll; height:350px; width:700px\">{0}</div>", "查無資料");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ContactView[] contacts = null;
                string msg = "";

                string html_string = "";
                if (getContactata(out contacts, out msg))
                {
                    #region 資料分類
                    List<string> tabs_title = new List<string>();
                    //tabs_title.Add(GetLocalized("大專院校"));
                    //tabs_title.Add(GetLocalized("高中職"));
                    //tabs_title.Add(GetLocalized("國中小"));
                    //tabs_title.Add(GetLocalized("幼兒園"));
                    tabs_title.Add("大專院校");
                    tabs_title.Add("高中職");
                    tabs_title.Add("國中小");
                    tabs_title.Add("幼兒園");

                    ArrayList contents = new ArrayList();
                    List<ContactView> tmp = new List<ContactView>();
                    string last_type = "";
                    for (Int32 i = 0; i < contacts.Length; i++)
                    {
                        ContactView contact = contacts[i];
                        if (contact.CorpType != last_type)
                        {
                            if (last_type == "")
                            {
                                tmp = new List<ContactView>();
                                tmp.Add(contact);
                                last_type = contact.CorpType;
                            }
                            else
                            {
                                contents.Add(tmp.ToArray<ContactView>());

                                tmp = new List<ContactView>();
                                tmp.Add(contact);
                                last_type = contact.CorpType;
                            }
                        }
                        else
                        {
                            //tmp = new List<ContactView>();
                            tmp.Add(contact);
                        }
                    }
                    contents.Add(tmp.ToArray<ContactView>());
                    #endregion
                    html_string = genPage(tabs_title.ToArray(), contents);
                }
                else
                {
                    html_string = genEmptyPage();
                }

                this.Literal1.Text = html_string;
            }
        }
    }
}