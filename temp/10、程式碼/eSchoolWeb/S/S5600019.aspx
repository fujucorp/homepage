<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600019.aspx.cs" Inherits="eSchoolWeb.S.S5600019" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="condition" class="condition" summary="查詢條件" width="100%">
<tr>
    <th width="20%" ><%= GetLocalized("商家代號") %>：</th>
    <td width="30%" >
        <asp:TextBox ID="tbxReceiveType" runat="server" MaxLength="4" Width="80"></asp:TextBox>
    </td>
    <th width="20%" ><%= GetLocalized("超商代收代號") %>：</th>
    <td width="30%" >
        <asp:DropDownList ID="ddlSMBarcodeId" runat="server" OnSelectedIndexChanged="ddlSMBarcodeId_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    </td>
</tr>
<tr>
    <th width="20%" style="font-size:14px;"><%= GetLocalized("產生張數") %>：</th>
    <td width="80%" colspan="3">
        <asp:TextBox ID="tbxDataCount" runat="server" MaxLength="2" Width="80px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td colspan="4">
        <asp:Label ID="labChannelInfo" runat="server" Text=""></asp:Label>
    </td>
</tr>
</table>

<div class="button">
	<cc:MyLinkButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click" LocationText="產生繳費單"></cc:MyLinkButton>
</div>
</asp:Content>
