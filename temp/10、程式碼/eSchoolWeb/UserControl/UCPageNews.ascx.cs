using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;

namespace eSchoolWeb.UserControl
{
    public partial class UCPageNews : BaseUserControl
    {
        #region Propery
        private string _PageId = string.Empty;
        /// <summary>
        /// 取得顯示的頁面名稱
        /// </summary>
        public string PageId
        {
            get
            {
                return _PageId;
            }
            set
            {
                _PageId = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!String.IsNullOrEmpty(_PageId))
                {
                    GetDataAndBind(_PageId);
                }
            }
        }

        /// <summary>
        /// 取得最新消息資料
        /// </summary>
        protected void GetDataAndBind(string _PageId)
        {
            litNews.Text = string.Empty;

            Expression where = new Expression();
            where.And(BoardEntity.Field.SchId, _PageId);
            where.And(BoardEntity.Field.StartDate, RelationEnum.LessEqual, DateTime.Today);
            where.And(BoardEntity.Field.EndDate, RelationEnum.GreaterEqual, DateTime.Today);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();

            #region [MDY:2018xxxx] 公告對象
            orderbys.Add(BoardEntity.Field.ReceiveType, OrderByEnum.Asc);
            #endregion

            orderbys.Add(BoardEntity.Field.BoardId, OrderByEnum.Asc);

            BoardEntity[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<BoardEntity>(this.Page, where, orderbys, out datas);
            if (xmlResult.IsSuccess)
            {
                #region [MDY:2018xxxx] 新增社群分享
                string url = Server.UrlEncode(this.Page.Request.Url.AbsoluteUri);
                string fbHtml = "<span style=\"margin:2px 5px;\"><a href=\"http://www.facebook.com/sharer.php?u={0}&t={1}&locale=zh_TW\" target=\"_blank\"><img style=\"width:26px;\" src=\"/img/button/fb-share-40.png\"></a></span>";
                string lineHtml = "<span style=\"margin:2px 5px;\"><a href=\"http://line.naver.jp/R/msg/text/?{0}\" target=\"_blank\"><img style=\"width:26px;\" src=\"/img/button/line-share-40.png\"></a></span>";
                #endregion

                StringBuilder html = new StringBuilder();
                foreach (BoardEntity data in datas)
                {
                    #region [MDY:2018xxxx] 區分公告對象 & 社群分享
                    #region [Old]
                    //html.AppendFormat("<li style=\"list-style:disc;\">{0}</li>", data.BoardContent).AppendLine();
                    #endregion

                    string shareHtml = null;
                    if (data.ShareFlag == "Y")
                    {
                        shareHtml = String.Concat("<br/>", String.Format(fbHtml, url, Server.UrlEncode(data.BoardSubject)), String.Format(lineHtml, url));
                    }

                    if (String.IsNullOrEmpty(data.ReceiveType))
                    {
                        html.AppendFormat("<li style=\"list-style:disc;\">{0}{1}</li>", data.BoardContent, shareHtml).AppendLine();
                    }
                    else
                    {
                        html.AppendFormat("<li style=\"list-style:disc; display:none;\" name=\"{1}\">{0}{2}</li>", data.BoardContent, data.ReceiveType, shareHtml).AppendLine();
                    }
                    #endregion
                }

                litNews.Text = html.ToString();
            }
        }
    }
}