<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護金融機構代碼" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600002M.aspx.cs" Inherits="eSchoolWeb.S.S5600002M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th width="30%"><%= GetLocalized("分行代碼") %>：</th>
        <td>
            <asp:TextBox ID="tbxBankNo" runat="server" MaxLength="6" Width="80%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("分行名稱") %>：</th>
        <td>
            <asp:TextBox ID="tbxBankFName" runat="server" MaxLength="34" Width="80%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("分行電話") %>：</th>
        <td>
            <asp:TextBox ID="tbxTel" runat="server" MaxLength="20" Width="80%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("銀行金融代號") %>：</th>
        <td>
            <asp:TextBox ID="tbxFullCode" runat="server" MaxLength="7" Width="80%"></asp:TextBox>
        </td>
    </tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
