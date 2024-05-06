<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sitemap.aspx.cs" Inherits="eSchoolWeb.Sitemap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>土地銀行 - 代收學雜費服務網 - 網站導覽</title>
    <link href="/css/whole.css" rel="stylesheet" type="text/css" />
    <link href="/css/index.css" rel="stylesheet" type="text/css" />
    <link href="/css/content.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }

        #sitemap ul {
            margin-left: 100px;
            line-height: 32px;
        }
        #sitemap li a {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }
        #sitemap li a:link, li a:visited {
            color: black;
        }
        #sitemap li a:hover {
            color: #0066cc;
        }
        html {
            display: none;
        }
    </style>

    <script type="text/javascript" language="javascript" src="/js/jquery-3.5.1.min.js"></script>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = '/Sitemap.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="container">

            <!--begin of header-------------------------------------------------------------------->
            <div id="header">
                <uc:UCEntryPageHeader runat="server" ID="UCEntryPageHeader" />
            </div>
            <!--end of header---------------------------------------------------------------------->

            <!--begin of content------------------------------------------------------------------->
            <div id="bg">
                <div id="content">
                    <!--begin of main------------------------------------------------------------------>
                    <div id="main" style="height:100%;">
                        <div class="title">
                            <p></p>
                            <span>網站導覽</span>
                        </div>

                        <div id="sitemap">
                            <ul>
                                <li><a href="javascript:void(0);" class="school" onclick="window.location.replace('school_login.aspx')"><span><%= GetLocalized("學校專區") %><!--學校專區--></span></a></li>
                                <li><a href="javascript:void(0);" class="student" onclick="window.location.replace('student_login.aspx')"><span><%= GetLocalized("學生專區") %><!--學生專區--></span></a></li>
                                <li><a href="javascript:void(0);" class="bank" onclick="window.location.replace('bank_login.aspx')"><span><%= GetLocalized("銀行專區") %><!--銀行專區--></span></a></li>
                                <li><a href="javascript:void(0);" class="creditCard" onclick="window.location.replace('creditcard.aspx')"><span><%= GetLocalized("信用卡繳費") %><!--信用卡繳費--></span></a></li>
                                <li><a href="javascript:void(0);" class="unionPay" onclick="window.open('https://www.i-payment.com.tw', '_unionPay')"><span><%= GetLocalized("銀聯卡繳費") %><!--銀聯卡繳費--></span></a></li>
                                <li id="liAlipay" runat="server" ><a href="javascript:void(0);" class="alipay" onclick="window.location.replace('alipay.aspx')"><span><%= GetLocalized("支付寶繳費") %><!--支付寶繳費--></span></a></li>
                                <li><a href="javascript:void(0);" class="checkPayment" onclick="window.location.replace('check_bill_status.aspx')"><span><%= GetLocalized("查詢繳費狀態") %><!--查詢繳費狀態--></span></a></li>
                                <li><a href="javascript:void(0);" class="checkReceipt" onclick="window.location.replace('print_bill.aspx')"><span><%= GetLocalized("查詢列印繳費單") %><!--查詢列印繳費單--></span></a></li>
                                <li><a href="javascript:void(0);" class="receipt" onclick="window.location.replace('print_receipt.aspx')"><span><%= GetLocalized("列印收據") %><!--列印收據--></span></a></li>
                                <li><a href="javascript:void(0);" class="download" onclick="window.location.replace('file_download.aspx')"><span><%= GetLocalized("檔案下載") %><!--檔案下載--></span></a></li>
                                <li><a href="javascript:void(0);" class="contact" onclick="window.location.replace('contact.aspx')"><span><%= GetLocalized("聯絡我們") %><!--聯絡我們--></span></a></li>
                                <li><a href="javascript:void(0);" class="qa" onclick="window.location.replace('QandA.aspx')"><span><%= GetLocalized("Ｑ＆Ａ") %><!--Ｑ＆Ａ--></span></a></li>
                            </ul>
                        </div>
                    </div>
                    <!--end of main-->

                </div>
            </div>
            <!--end of content--------------------------------------------------------------------->

            <uc:UCFooter runat="server" ID="UCFooter" />
        </div>
    </form>
</body>
</html>
