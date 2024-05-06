<%@ Page Title="土地銀行 - 代收學雜費服務網 - 自收單筆銷帳" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3100001.aspx.cs" Inherits="eSchoolWeb.C.C3100001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th><%= GetLocalized("虛擬帳號") %>：</th>
    <td><asp:TextBox ID="tbxCancelNo" runat="server" MaxLength="16" Width="200px"></asp:TextBox></td>
</tr>
<tr>
    <th><%= GetLocalized("應繳金額") %>：</th>
    <td><asp:TextBox ID="tbxReceiveAmount" runat="server" MaxLength="9" Width="100px"></asp:TextBox></td>
</tr>
<tr>
    <th><%= GetLocalized("代收日") %>：</th>
    <td>
        <asp:TextBox ID="tbxReceiveDate" runat="server" MaxLength="7" Width="100px"></asp:TextBox>&nbsp;
        <span style="font-size:11px">(請輸入3碼的民國年+2碼月+2碼日。例如 2015/01/02 請輸入 1040102)</span>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("入帳日") %>：</th>
    <td>
        <asp:TextBox ID="tbxAccountDate" runat="server" MaxLength="7" Width="100px"></asp:TextBox>&nbsp;
        <span style="font-size:11px">(請輸入3碼的民國年+2碼月+2碼日。例如 2015/01/02 請輸入 1040102)</span>
    </td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
</div>
</asp:Content>
