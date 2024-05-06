<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護退費處理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="R4100001M.aspx.cs" Inherits="eSchoolWeb.R.R4100001M" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
    if (self === top) {
        document.documentElement.style.display = 'block';
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" />

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th colspan="2" style="text-align:center">學生基本資料</th>
    </tr>
    <tr>
        <td width="50%">學號： <asp:Label ID="labStuId" runat="server" Text=""></asp:Label></td>
        <td width="50%">姓名： <asp:Label ID="labStuName" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td>身份證字號： <asp:Label ID="labIdNumber" runat="server" Text=""></asp:Label></td>
        <td>電話：  <asp:Label ID="labTel" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td>生日：<asp:Label ID="labBirthday" runat="server" Text=""></asp:Label></td>
        <td>郵遞區號：<asp:Label ID="labZipCode" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td>電子郵件：<asp:Label ID="labEmail" runat="server" Text=""></asp:Label></td>
        <td>帳號：<asp:Label ID="labAccount" runat="server" Text=""></asp:Label></td>
    </tr>

    <tr>
        <th colspan="2" style="text-align:center">退費記錄</th>
    </tr>
    <tr>
        <td>上次退費日期： <asp:Label ID="labReturnDate" runat="server" Text=""></asp:Label></td>
        <td>退費方式/標準： <asp:Label ID="labReturnId" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td>已產生媒體註記： <asp:Label ID="labReturnRemark" runat="server" Text=""></asp:Label></td>
        <td>退費方式： <asp:Label ID="labReturnWay" runat="server" Text=""></asp:Label></td>
    </tr>

    <tr>
        <th colspan="2" style="text-align:center">退費處理</th>
    </tr>
    <tr>
        <td>虛擬帳號： <asp:Label ID="labCancelNo" runat="server" Text=""></asp:Label></td>
        <td>退費方式： 
            <asp:DropDownList ID="ddlReturnWay" runat="server">
                <asp:ListItem Value="1">現金</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>已產生媒體註記：
            <asp:RadioButtonList ID="rdoReturnRemark" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
                <asp:ListItem Value="y">是</asp:ListItem>
                <asp:ListItem Selected="True" Value="n">否</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td>退費次序：<asp:Label ID="labReSeq" runat="server" Text=""></asp:Label></td>
    </tr>

    <tr>
        <th colspan="2" style="text-align:center">退費科目金額</th>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%">
                <tr>
                    <td width="33%">收入科目</td><td width="33%">目前可退金額</td><td width="34%" id="tdReturnMoneyTitle">退費金額</td>
                </tr>
                <asp:Literal ID="litReceiveItemHtml" runat="server"></asp:Literal>
            </table>
        </td>
    </tr>
    <tr>
        <td>累計退費金額合計：<asp:TextBox ID="labReturnAmount" Enabled="false" runat="server"></asp:TextBox></td>
        <td>退費方式/標準：
            <asp:DropDownList ID="ddlReturnId" runat="server">
                <asp:ListItem Value="0">依輸入金額</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trReturnWay" style="display:none">
        <td>領取支票方式：<asp:Label ID="Label1" runat="server" Text=""></asp:Label></td>
        <td>匯款資料：
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> --- 
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>

<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click" ></cc:MyOKButton>&nbsp;&nbsp;&nbsp;
    <cc:MyLinkButton ID="lbtnBack" runat="server" Text="離開" OnClick="lbtnBack_Click"></cc:MyLinkButton>
</div>

<script type="text/javascript">
    function addmon() {
        var total = 0;
        var val = 0;

        var fieldName = "tbxReturnMoney";
        for (i = 1; i < 31; i++) {
            var str = "";
            if (i < 10)
            { str = fieldName + "0" + i.toString(); }
            else
            { str = fieldName + i.toString(); }

            if (document.getElementById(str) != null) {
                if (document.getElementById(str).value != "") {
                    val = parseFloat(document.getElementById(str).value);
                    //alert(val);
                    total += val;
                }
            }
        }
        document.getElementById('<%=labReturnAmount.ClientID%>').value = total;
    }
</script>

</asp:Content>
