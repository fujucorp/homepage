<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EZPosResponse2.aspx.cs" Inherits="eSchoolWeb.EZPosResponse2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=big5"/>
    <title></title>
    <link href="/css/whole.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #webTitle a, .title p, .masthead p, .button {
            font-family: ī�G�פ���, �L�n������, �s�ө���, Arial, Helvetica, sans-serif;
        }

        #page {
            width: 500px;
            height:300px;
            position: relative;
            margin: 0 auto;
            padding:0;
            border: 0px solid #000000;
            background: transparent url(../images/bg/page.jpg) no-repeat top left;
            overflow: auto;
        }

        table.result input[type="text"] {
            border-width: 0px;
        }

        table.result td, table.result th {
            padding: 6px 5px;
            text-align: right;
        }

        input[type="text"]:disabled {
            background: #ffffff;
            color: #000000;
        }

        html {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        //if (top != self) {
        //    top.location = self.location
        //}
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = '/EZPosResponse2.aspx';
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
<div id="container">

    <!--begin of header-------------------------------------------------------------------->
    <div id="header">
        <uc:UCEntryPageHeader ID="ucEntryPageHeader" runat="server" />
    </div>
    <!--end of header-->

    <!--begin of content------------------------------------------------------------------->
    <div id="content">

        <!--begin of main------------------------------------------------------------------>
        <div  style="height:500px; width:100%; margin-top:50px;">
            <!--�D�n���e����----------------------------------->

            <div style="margin-top: 150px;">
                <div style="padding-bottom:5px;">
                    <asp:Label ID="labResult" runat="server" EnableViewState="false" ></asp:Label>
                </div>
                <div id="divResult" runat="server" visible="false">
                    <table class="result">
                    <tr>
                        <th><%= GetLocalized("����s��") %>�G </th>
                        <td><asp:TextBox ID="tbxTxnId" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%= GetLocalized("�����b��") %>�G </th>
                        <td><asp:TextBox ID="tbxLidm" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%= GetLocalized("���v���B") %>�G </th>
                        <td><asp:TextBox ID="tbxAuthAmt" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%= GetLocalized("���O") %>�G </th>
                        <td><asp:TextBox ID="tbxCurrency" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%= GetLocalized("���v���A") %>�G </th>
                        <td><asp:TextBox ID="tbxStatus" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%= GetLocalized("������v�X") %>�G </th>
                        <td><asp:TextBox ID="tbxAuthCode" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%= GetLocalized("���v����Ǹ�") %>�G </th>
                        <td><asp:TextBox ID="tbxXid" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%= GetLocalized("���d�H�H�Υd���|�X") %>�G </th>
                        <td><asp:TextBox ID="tbxLastPan4" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr id="thErrCode" runat="server">
                        <th><%= GetLocalized("���~�N�X") %>�G </th>
                        <td><asp:TextBox ID="tbxErrCode" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    <tr id="thErrDesc" runat="server">
                        <th><%= GetLocalized("���v���ѭ�]") %>�G </th>
                        <td><asp:TextBox ID="tbxErrDesc" runat="server" ReadOnly="True" Enabled="false" EnableViewState="false" ></asp:TextBox></td>
                    </tr>
                    </table>
                </div>
                <div style="padding-bottom:5px;">
                    <asp:Label ID="labMemo" runat="server" EnableViewState="false" ></asp:Label>
                </div>
                <div style="padding-bottom:5px;">
                    <input type="button" name="button" value="<%= GetLocalized("��������") %>" onclick="window.close();" />
                </div>
            </div>

        </div><!--end of main-->

        <!--begin of footer---------------------------------------------------------------->
        <div id="footer">
            <uc:UCFooter ID="ucFooter" runat="server" />
        </div><!--end of footer-->

    </div><!--end of content-->
</div>
    </form>
</body>
</html>