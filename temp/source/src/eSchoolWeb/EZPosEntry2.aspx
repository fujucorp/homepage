<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EZPosEntry2.aspx.cs" Inherits="eSchoolWeb.EZPosEntry2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=big5"/>
    <title></title>
    <style type="text/css">
        #webTitle a, .title p, .title span, .masthead p, .masthead_s p, #channel a, #menu a, #logOut a, .button {
            font-family: 蘋果儷中黑, 微軟正黑體, 新細明體, Arial, Helvetica, sans-serif;
        }

        table.result input[type="text"] {
            border-width: 0px;
        }

        table.result td, table.result th {
            padding: 6px 5px;
            text-align: right;
        }

        input {
            font-size : 12pt;
        }

        input[type="text"]:disabled {
            background: #ffffff;
            color: #000000;
        }

        html {
            display: none;
        }
    </style>
    <script type="text/javascript" >
        //if (top != self) {
        //    top.location = self.location
        //}
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = '/EZPosEntry2.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divInfo" runat="server" visible="false">
            <table class="result">
                <tr>
                    <th><%= GetLocalized("學校名稱") %>： </th>
                    <td><asp:TextBox ID="tbxSchoolName" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%= GetLocalized("學生姓名") %>： </th>
                    <td><asp:TextBox ID="tbxStudentName" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%= GetLocalized("學年") %>： </th>
                    <td><asp:TextBox ID="tbxYearId" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%= GetLocalized("學期") %>： </th>
                    <td><asp:TextBox ID="tbxTermName" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%= GetLocalized("費用別") %>： </th>
                    <td><asp:TextBox ID="tbxReceiveName" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%= GetLocalized("虛擬帳號") %>： </th>
                    <td><asp:TextBox ID="tbxCancelNo" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%= GetLocalized("交易編號") %>： </th>
                    <td><asp:TextBox ID="tbxTxnId" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%= GetLocalized("交易金額") %>： </th>
                    <td><asp:TextBox ID="tbxReceiveAmount" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <div id="divMessage" runat="server" EnableViewState="false"></div>
    </form>
    <div id="divForm" runat="server" visible="false" >

    </div>
</body>
</html>
