<%@ Page Title="土地銀行 - 代收學雜費服務網 - 連動製單帳號管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600014M.aspx.cs" Inherits="eSchoolWeb.S.S5600014M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="修改" width="100%">
    <tr>
        <th width="30%"><%= GetLocalized("系統代碼") %>：</th>
        <td>
            <asp:TextBox ID="tbxSysId" runat="server" MaxLength="32" Width="80%"></asp:TextBox>
            <br />(6 ~ 32 碼的數字、英文、或英數字混合，可含 Dash 符號，但第一個字不可以是 Dash)
        </td>
    </tr>
    <tr>
        <th width="30%"><%= GetLocalized("系統名稱") %>：</th>
        <td>
            <asp:TextBox ID="tbxSysName" runat="server" MaxLength="32" Width="80%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th width="30%"><%= GetLocalized("系統驗證碼") %>：</th>
        <td>
            <asp:TextBox ID="tbxSysPXX" runat="server" MaxLength="32" Width="80%"></asp:TextBox>
            <br />(8 ~ 32 碼的數字、英文、或英數字混合)
        </td>
    </tr>
    <tr>
        <th width="30%"><%= GetLocalized("授權呼叫端IP清單") %>：</th>
        <td>
            <asp:TextBox ID="tbxClientIp" runat="server" MaxLength="120" Width="98%"></asp:TextBox>
            <br />(多個IP時，請以逗號區隔)
        </td>
    </tr>
    <tr id="trReceiveType" runat="server" >
        <th><%= GetLocalized("申請的學校") %>：</th>
        <td class="tdReceiveType">
            <asp:DropDownList ID="ddlSchIdenty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchIdenty_SelectedIndexChanged" >
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("授權的商家代號") %>：</th>
        <td>
            <asp:CheckBoxList ID="cblReceiveType" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("學校接收端 Url") %>：</th>
        <td>
            <asp:TextBox ID="tbxSchReceiveUrl" runat="server" MaxLength="100" Width="98%"></asp:TextBox>
            <br />(不設定則不會發送【即時入金資料回饋】)
        </td>
    </tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
