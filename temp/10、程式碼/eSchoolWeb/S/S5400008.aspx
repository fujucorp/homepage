<%@ Page Title="土地銀行 - 代收學雜費服務網 - 手續費統計報表" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400008.aspx.cs" Inherits="eSchoolWeb.S.S5400008" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
<tr id="trBank" runat="server">
    <th><%=this.GetLocalized("分行") %>：</th>
    <td>
        <asp:DropDownList ID="ddlBank" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged"></asp:DropDownList>
    </td>
</tr>
<tr id="trSchIdenty" runat="server">
    <th><%=this.GetLocalized("學校") %>：</th>
    <td>
        <asp:DropDownList ID="ddlSchIdenty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchIdenty_SelectedIndexChanged"></asp:DropDownList>
    </td>
</tr>
<tr>
    <th><%=this.GetLocalized("商家代號") %>：</th>
    <td>
        <asp:DropDownList ID="ddlReceiveType" runat="server"></asp:DropDownList>
    </td>
</tr>
<tr id="trAccountDateRange">
    <th><%=this.GetLocalized("入帳日期區間") %>：</th>
    <td>
        <div align="left">
            <asp:TextBox ID="tbxAccountDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox> ～
            <asp:TextBox ID="tbxAccountDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyLinkButton ID="ccbtnReport" runat="server" LocationText="報表XLS" CommandArgument="XLS" OnClick="ccbtnReport_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnReportODS" runat="server" LocationText="報表ODS" CommandArgument="ODS" OnClick="ccbtnReport_Click"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

</asp:Content>
