<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alipay.aspx.cs" Inherits="eSchoolWeb.alipay" %>

<%@ Register Src="~/UserControl/UCPageNews.ascx" TagPrefix="uc" TagName="UCPageNews" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/whole.css" rel="stylesheet" type="text/css" />
    <link href="/css/index.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }
        .uppercase {
            text-transform: uppercase;
        }
        html {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript" src="/js/jquery-3.5.1.min.js"></script>
    <script type="text/javascript" language="javascript" src="/js/jquery.blockUI.js"></script>
    <script type="text/javascript" language="javascript" src="/js/common.js"></script>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = decodeURIComponent(self.location);
        }
    </script>
    <script  type="text/javascript">
        function checkInput(event) {
            if (event.keyCode == 13) {
                var oInput1 = $("#<%=txtCancelNo.ClientID%>");
                if (oInput1.val().trim() == "") {
                    oInput1.focus();
                    return false;
                }
                var oInput2 = $("#<%=txtValidateCode.ClientID%>");
                if (oInput2.val().trim() == "") {
                    oInput2.focus();
                    return false;
                }

                $("#<%=ccbtnOK.ClientID%>")[0].click();
                return false;
            }
        }
        function reloadCaptcha() {
            var src = "ValidatePicPage.aspx?" + (new Date()).getTime();
            $("#imgCaptcha").attr("src", src);
        }
    </script>
</head>

<body>
    <form id="form1" runat="server" onkeypress="javascript:return checkInput(event);">
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
                            <span><%= GetLocalized("支付寶繳費") %></span>
                        </div>

                        <div id="loginBox">
                            <table>
                                <tr>
                                    <th><%= GetLocalized("虛擬帳號") %>：</th>
                                    <td><asp:TextBox ID="txtCancelNo" runat="server" MaxLength="16" autocomplete="off"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("圖型驗證碼") %>：</th>
                                    <td>
                                        <img id="imgCaptcha" src="ValidatePicPage.aspx?<%= DateTime.Now.Ticks %>" style="vertical-align:bottom" alt="圖形驗證" onclick="reloadCaptcha()" />
                                        <asp:TextBox ID="txtValidateCode" runat="server" MaxLength="6" Width="90px" CssClass="uppercase" EnableViewState="false"></asp:TextBox><br />
                                        <span style="font-size:10pt;">(<%= GetLocalized("點上圖可更新驗證碼") %>)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="2" style="background-color:#DFDFDF;">
                                        提醒您，<br />
                                        <ul style="list-style-type:decimal; padding-left:20px;">
                                            <li>
                                                本交易係為跨境電子支付服務，依據支付寶作業，繳費款項入帳時間為繳款日隔週週四，屆時本行及貴屬學校始能查得您的繳費資料。<br />
                                                繳費前請確認款項入帳時間未逾您的繳款單繳費期限以維護您的權益。
                                            </li>
                                            <li>
                                                使用本繳款服務每筆交易須支付繳費金額之1.7%手續費，完成繳款後，相關退款作業請逕洽各學校辦理，本繳款交易已收取之手續費將不退還。
                                            </li>
                                        </ul>
                                    </th>
                                </tr>
                                <tr>
                                    <th>&nbsp;</th>
                                    <td>
                                        <div class="button">
                                            <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
                                            <a href="javascript:form1.reset();"><%= GetLocalized("重填") %></a>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <uc:UCPageNews runat="server" ID="UCPageNews" />
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
