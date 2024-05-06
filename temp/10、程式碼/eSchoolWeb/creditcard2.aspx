<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="creditcard2.aspx.cs" Inherits="eSchoolWeb.creditcard2" %>

<%@ Register Src="~/UserControl/UCPageNews.ascx" TagPrefix="uc" TagName="UCPageNews" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>土地銀行 - 代收學雜費服務網 - 國際信用卡繳費</title>
    <link href="/css/whole.css" rel="stylesheet" type="text/css" />
    <link href="/css/index.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }

        .uppercase {
            text-transform: uppercase;
        }

        table.result td, table.result th {
            padding: 6px 5px;
            text-align: left;
            font-size: 12pt;
            line-height: 18px;
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
            var oInput1 = $("#<%=tbxQCancelNo.ClientID%>");
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
                            <span><%= GetLocalized("國際信用卡繳費") %></span>
                        </div>

                        <div id="divQuery" runat="server">
                            <table class="result">
                                <tr>
                                    <th><%= GetLocalized("虛擬帳號") %>：</th>
                                    <td>
                                        <asp:TextBox ID="tbxQCancelNo" runat="server" MaxLength="16" autocomplete="off"></asp:TextBox></td>
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
                                    <th>&nbsp;</th>
                                    <td>
                                        <div class="button">
                                            <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;
                                            <a href="javascript:form1.reset();"><%= GetLocalized("重填") %><!--重填--></a>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <uc:UCPageNews runat="server" ID="UCPageNews" />

                        <div id="divResult" runat="server">
                            <table class="result">
                                <tr>
                                    <th><%= GetLocalized("學校名稱") %>： </th>
                                    <td><asp:TextBox ID="tbxRSchoolName" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("學生姓名") %>： </th>
                                    <td><asp:TextBox ID="tbxRStudentName" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("學年") %>： </th>
                                    <td><asp:TextBox ID="tbxRYearId" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("學期") %>： </th>
                                    <td><asp:TextBox ID="tbxRTermName" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("費用別") %>： </th>
                                    <td><asp:TextBox ID="tbxRReceiveName" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("虛擬帳號") %>： </th>
                                    <td><asp:TextBox ID="tbxRCancelNo" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("應繳金額") %>： </th>
                                    <td><asp:TextBox ID="tbxRAmount" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="button">
                                            <cc:MyLinkButton ID="ccbtnPay" runat="server" OnClick="ccbtnPay_Click" LocationText="繳費"></cc:MyLinkButton>&nbsp;&nbsp;
                                            <a href="javascript:window.history.back();"><%= GetLocalized("回上一頁") %></a>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label ID="labErrMsg" runat="server" Text="" Font-Size="16px"></asp:Label></td>
                                </tr>
                            </table>
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
