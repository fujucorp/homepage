<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QandA.aspx.cs" Inherits="eSchoolWeb.QandA" %>

<%@ Register Src="~/UserControl/UCPageNews.ascx" TagPrefix="uc" TagName="UCPageNews" %>
<%@ Register Src="~/UserControl/UCQA.ascx" TagPrefix="uc" TagName="UCQA" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/whole.css" rel="stylesheet" type="text/css" />
    <link href="/css/index.css" rel="stylesheet" type="text/css" />
    <link href="/css/content.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        #webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }

        div.form_item {
            display: table;
            width: 100%;
            line-height: 30px;
        }
        label.form_item_title {
            display: table-cell;
            width: 15%;
            min-width: 155px;
            text-align: right;
            vertical-align: top;
            background-color: rgba(132,117,78,.1);
            padding: 5px 5px;
        }
        label.form_item_title span, span.form_item_title {
            color: #e03127 ;
            font-size: 1.375em;
        }
        div.form_item_input {
            display: table-cell;
            width: 85%;
            padding: 5px 5px 0 5px;
        }
        div.form_item_input input[type="text"] {
            width: 330px;
            max-width: 330px;
            border: 1px solid #a9a9a9;
            padding: 2px;
        }
        div.form_item_input textarea {
            width: 530px;
            height: 130px;
            margin-bottom: 5px;
            border: 1px solid #a9a9a9;
            padding: 2px;
        }

        div.form_item_input input:focus {
            outline: 2px solid #66afe9!important;
        }

        abbr[title] {
            cursor: help;
            font-weight: bold;
            border-bottom: none;
            text-decoration: none;
            vertical-align: middle;
            padding-right: 2px;
        }

        input[type="image"] {
            vertical-align: bottom;
            padding: 0px 5px;
        }

        span.errmsg {
            color: red;
            margin-left: 8px;
        }

        :-ms-input-placeholder { /* Internet Explorer 10-11 */
           color: #a9a9a9;
        }
        ::-ms-input-placeholder { /* Microsoft Edge */
            color: #a9a9a9;
        }
        *::placeholder { /* Chrome, Firefox, Opera, Safari 10.1+ */
            color: #a9a9a9;
            opacity: 1; /* Firefox */
        }
        ::-webkit-input-placeholder { /* Chrome/Opera/Safari */
            color: #a9a9a9;
        }
        ::-moz-placeholder { /* Firefox 19+ */
            color: #a9a9a9;
        }
        :-moz-placeholder { /* Firefox 18- */
            color: #a9a9a9;
        }
        html {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript" src="/js/jquery-3.5.1.min.js"></script>
    <script type="text/javascript" language="javascript" src="/js/jquery.blockUI.js"></script>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = decodeURIComponent(self.location);
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">

        <div id="container">

            <!--begin of header--------------------------------------------------------------------------------------------------------------------------------------------->
            <div id="header">
                <uc:UCEntryPageHeader runat="server" ID="UCEntryPageHeader" />
            </div>
            <!--end of header-->

            <!--begin of content-------------------------------------------------------------------------------------------------------------------------------------------->
            <div id="bg">
                <div id="content">

                    <!--begin of menu------------------------------------------------------------------>
                    <div id="menu">
                        <uc:UCEntryPageMenu runat="server" ID="UCEntryPageMenu" />
                    </div>
                    <!--end of menu-->

                    <!--begin of main------------------------------------------------------------------>
                    <div id="main">
                        <div class="title">
                            <p></p>
                            <span>Ｑ＆Ａ</span>
                        </div>

                        <uc:UCQA runat="server" id="UCQA" />

                        <div id="divOpinion" style="padding:10px; width:100%; line-height: 24px; letter-spacing: 2px;">
                            <div style="margin:5px 0px 0px 0px;">
                                <h2 style="border-left:5px solid #1a8601; padding-left:15px; font-size:1.375em;">顧客意見信箱</h2>
                            </div>
                            <div style="margin:15px 0px 0px 0px;">
                                您若對本行有任何建議或諮詢事項歡迎來信至本行將竭誠為您解答<br />
                                (如未書寫姓名，電話，電子郵箱，恕難處理)<br />
                                請以電子郵件傳送到：<a href="mailto:lbot@landbank.com.tw">lbot@landbank.com.tw</a><br />
                            </div>
                            <div style="margin:20px 0px 0px 0px;">
                                意見信箱處理程序：本行於營業時間內隨時下載意見信箱，承辦單位於限辦日期(5個日曆天)內處理完妥答覆客戶。
                            </div>
                        </div>
                    </div>
                    <!--end of main-->

                </div>
                <!--end of content-->
            </div>
            <!--end of bg-->

            <uc:UCFooter runat="server" ID="UCFooter" />

        </div>
    </form>
</body>

</html>
