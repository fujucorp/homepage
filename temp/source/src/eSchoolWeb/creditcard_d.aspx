<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="creditcard_d.aspx.cs" Inherits="eSchoolWeb.creditcard_d" %>

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
            top.location = '/creditcard_d.aspx';
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
                            <span><%= GetLocalized("信用卡繳費") %></span>
                        </div>

                        <div id="loginBox">
                            <table>
                                <tr>
                                    <th><%= GetLocalized("學校名稱") %>： </th>
                                    <td><asp:TextBox ID="txtSchool" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("學生姓名") %>： </th>
                                    <td><asp:TextBox ID="txtStudent" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("虛擬帳號") %>： </th>
                                    <td><asp:TextBox ID="txtCancelNo" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("應繳金額") %>： </th>
                                    <td><asp:TextBox ID="txtAmount" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("發卡銀行") %>：</th>
                                    <td><asp:TextBox ID="txtBank" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <th><%= GetLocalized("持卡人身分證字號") %>： </th>
                                    <td><asp:TextBox ID="txtPayerId" runat="server" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="button">
                                            <cc:MyLinkButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click" LocationText="繳費"></cc:MyLinkButton>&nbsp;&nbsp;
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
