using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using Fuju;
using Fuju.DB;
using Fuju.Web;
using Entities;
namespace eSchoolWeb.UserControl
{
    public partial class UCQA : System.Web.UI.UserControl
    {
        private bool getQNAData(out QnaEntity[] QNAs, out string msg)
        {
            bool rc = false;
            QNAs = null;
            msg = "";

            Expression where = new Expression(QnaEntity.Field.Status, DataStatusCodeTexts.NORMAL);
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(QnaEntity.Field.Type, OrderByEnum.Asc);
            orderbys.Add(QnaEntity.Field.Sort, OrderByEnum.Asc);

            XmlResult xmlResult = DataProxy.Current.SelectAll<QnaEntity>(this.Page, where, orderbys, out QNAs);
            if (!xmlResult.IsSuccess)
            {
                msg = string.Format("[getSchoolData] 取得Q&A資料發生錯誤，錯誤訊息={0}", xmlResult.Message);
            }
            else
            {
                rc = true;
            }

            return rc;
        }

        private string genPage(CodeTextList tabs, ArrayList datas)
        {
            //本頁面用Tabs來呈現

            string html_string = "";
            //
            html_string += string.Format("<div class=\"abgne_tab\">");

            string tmp1 = "";
            #region 頁籤的部分
            tmp1 += string.Format("<ul class=\"tabs\">");

            #region [MDY:20160516] 修正分類 Key
            #region [Old]
            //for (int i = 0; i < tab_titles.Length;i++)
            //{
            //    tmp1 += string.Format("<li><a href=\"#tab{0}\">{1}</a></li>", i + 1, tab_titles[i]);
            //}
            #endregion

            foreach (CodeText tab in tabs)
            {
                tmp1 += string.Format("<li><a href=\"#tab{0}\">{1}</a></li>", tab.Code, tab.Text);
            }
            #endregion

            tmp1 += string.Format("</ul>");
            #endregion
            html_string += tmp1;

            string tmp2 = "";
            #region 內容的部分
            tmp2 += string.Format("<div class=\"tab_container\">");

            //[MDY:20160516] 修正分類 Key
            for (int i = 0; i < tabs.Count; i++)
            {
                string tmp3 = "";
                #region 每一個頁籤的內容
                tmp3 += string.Format("<div id=\"tab{0}\" class=\"tab_content\">",i+1);

                string tmp4 = "";
                QnaEntity[] qnas = null;
                if (i < datas.Count)
                {
                    qnas = (QnaEntity[])datas[i];
                }

                #region 呈現
                Int32 idx = 0;
                tmp4 += string.Format("<table id=\"table1\" class=\"normal\" summary=\"表格_修改\" width=\"100%\">");
                if (qnas == null || qnas.Length <= 0)
                {
                    tmp4 += string.Format("<tr><th>查無資料</th></tr>");
                }
                else
                {
                    #region 每一筆資訊
                    foreach (QnaEntity qna in qnas)
                    {
                        idx++;
                        string q = qna.Q.Trim();
                        string a = qna.A.Trim();
                        tmp4 += string.Format("<tr><th>Q{0}：{1}</th></tr>", idx, q);
                        tmp4 += string.Format("<tr><td>　　　{0}</td></tr>", a);
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
                QnaEntity[] datas=null;
                string msg="";

                string html_string = "";
                if (getQNAData(out datas, out msg))
                {
                    #region 資料分類

                    #region [MDY:20160516] 修正分類 Key
                    #region [Old]
                    //List<string> tabs_title = new List<string>();
                    //tabs_title.Add("登入問題");
                    //tabs_title.Add("產生繳費單問題");
                    //tabs_title.Add("繳費問題");
                    //tabs_title.Add("信用卡繳費");
                    #endregion

                    CodeTextList tabs = new CodeTextList(4);
                    tabs.Add("1", "登入問題");
                    tabs.Add("2", "產生繳費單問題");
                    tabs.Add("3", "信用卡繳費");
                    tabs.Add("4", "其他問題");
                    #endregion

                    ArrayList contents = new ArrayList();
                    List<QnaEntity> tmp = new List<QnaEntity>();
                    string last_type = "";
                    if (datas != null && datas.Length > 0)
                    {
                        for (Int32 i = 0; i < datas.Length; i++)
                        {
                            QnaEntity qna = datas[i];
                            if (qna.Type != last_type)
                            {
                                if (last_type == "")
                                {
                                    tmp = new List<QnaEntity>();
                                    tmp.Add(qna);
                                    last_type = qna.Type;
                                }
                                else
                                {
                                    contents.Add(tmp.ToArray<QnaEntity>());

                                    tmp = new List<QnaEntity>();
                                    tmp.Add(qna);
                                    last_type = qna.Type;
                                }
                            }
                            else
                            {
                                //tmp = new List<QnaEntity>();
                                tmp.Add(qna);
                            }
                        }
                        contents.Add(tmp.ToArray<QnaEntity>());
                    }
                    #endregion
                    html_string = genPage(tabs, contents);
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