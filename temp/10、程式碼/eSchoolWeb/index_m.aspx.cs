using System;
using System.Collections.Generic;
using System.Text;
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
    public partial class index_m : LocalizedPage
    {
        #region Override IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "index_m";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "代收學雜費服務網(行動)";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region [Old]
        //private string GenMenuItem()
        //{
        //    string[] menu_url = { "#", "#", "#" };
        //    string[] menu_label = { "學生專區", "繳費狀態查詢", "信用卡繳款" };

        //    string html = "";
        //    html += "<ul>";
        //    for(int i=0;i<menu_url.Length;i++)
        //    {
        //        html += string.Format("<li><a href=\"{0}\">{1}</a></li>", menu_url[i], menu_label[i]);
        //    }
        //    html += "</ul>";
        //    return html;
        //}
        #endregion

        /// <summary>
        /// 取得最新消息資料
        /// </summary>
        protected void GetDataAndBind()
        {
            this.litNews.Text = string.Empty;

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
            if (datas != null && datas.Length > 0)
            {
                foreach (BoardEntity data in datas)
                {
                    html.AppendLine("<ul>");
                    if (data.StartDate != null)
                    {
                        html.AppendFormat("<li class=\"date\">{0}</li>", ((DateTime)data.StartDate).ToString("yyyy/MM/dd")).AppendLine();
                    }
                    html.AppendFormat("<li>{0}</li>", data.BoardContent).AppendLine();
                    html.AppendLine("</ul>");
                }
            }
            //else
            //{
            //    html.AppendLine("<div style='padding: 1%; width: 98%; text-align: center;'><img src='img/ad/ad001.jpg' alt='代收學雜費服務網(行動)'></div>");
            //}
            this.litNews.Text = html.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.GetDataAndBind();
            }
        }
    }
}