﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb
{
    public partial class index : LocalizedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (isMobileDevice())
                {
                    #region [MDY:20210521] 原碼修正
                    Server.Transfer(WebHelper.GenRNUrl("index_m.aspx"));
                    #endregion

                    return;
                }

                string ec = this.Request.QueryString["ec"];
                if (!String.IsNullOrWhiteSpace(ec))
                {
                    switch(ec.Trim())
                    {
                        case "1":
                            this.ShowJsAlert("頁面操作逾時，系統已自動登出");
                            break;
                    }
                }
                GetDataAndBind();
            }
        }

        /// <summary>
        /// 取得最新消息資料
        /// </summary>
        protected void GetDataAndBind()
        {
            litNews.Text = string.Empty;

            Expression where = new Expression();
            where.And(BoardEntity.Field.SchId, BoardTypeCodeTexts.INDEX);
            where.And(BoardEntity.Field.StartDate, RelationEnum.LessEqual, DateTime.Today);
            where.And(BoardEntity.Field.EndDate, RelationEnum.GreaterEqual, DateTime.Today);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(BoardEntity.Field.BoardId, OrderByEnum.Asc);

            BoardEntity[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<BoardEntity>(this, where, orderbys, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            
            StringBuilder html = new StringBuilder();
            if(datas!=null && datas.Length>0)
            {
                #region [MDY:2018xxxx] 新增社群分享
                string url = Server.UrlEncode(this.Page.Request.Url.AbsoluteUri);
                string fbHtml = "<span style=\"margin:2px 5px;\"><a href=\"http://www.facebook.com/sharer.php?u={0}&t={1}&locale=zh_TW\" target=\"_blank\"><img style=\"width:26px;\" src=\"/img/button/fb-share-40.png\"></a></span>";
                string lineHtml = "<span style=\"margin:2px 5px;\"><a href=\"http://line.naver.jp/R/msg/text/?{0}\" target=\"_blank\"><img style=\"width:26px;\" src=\"/img/button/line-share-40.png\"></a></span>";
                #endregion

                foreach (BoardEntity data in datas)
                {
                    html.AppendLine("<ul>");
                    if (data.StartDate != null)
                    {
                        html.AppendFormat("<li class=\"date\">{0}</li>", ((DateTime)data.StartDate).ToString("yyyy/MM/dd")).AppendLine();
                    }

                    #region [MDY:2018xxxx] 社群分享
                    #region [Old]
                    //html.AppendFormat("<li>{0}</li>", data.BoardContent).AppendLine();
                    #endregion

                    string shareHtml = null;
                    if (data.ShareFlag == "Y")
                    {
                        shareHtml = String.Concat("<br/>", String.Format(fbHtml, url, Server.UrlEncode(data.BoardSubject)), String.Format(lineHtml, url));
                    }
                    html.AppendFormat("<li>{0}{1}</li>", data.BoardContent, shareHtml).AppendLine();
                    #endregion

                    html.AppendLine("</ul>");
                }
            }
            litNews.Text = html.ToString();
        }


        private bool isMobileDevice()
        {
            bool rc = false;
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                rc = true;
            }
            else
            {
                rc = false;
            }
            return rc;
        }
    }
}