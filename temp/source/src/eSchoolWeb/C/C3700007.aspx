<%@ Page Title="土地銀行 - 代收學雜費服務網 - 下載銷帳資料(固定格式)" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3700007.aspx.cs" Inherits="eSchoolWeb.C.C3700007" %>

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
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<br/>
<!--//表格_修改----------------------------------------------------------------->
<table class="result" summary="查詢條件" width="100%">
<tr>
    <td width="50%">
        <div align="left"><%=this.GetLocalized("批號") %>
            <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
        </div>
    </td>
</tr>
<tr id="trReceiveWay">
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("繳款方式") %>
            <asp:DropDownList ID="ddlReceiveWay" runat="server">
                <asp:ListItem Value="1">超商</asp:ListItem>
                <asp:ListItem Value="2">ATM</asp:ListItem>
                <asp:ListItem Value="3">臨櫃</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr id="trReceiveDateRange">
    <td colspan="2">
        <div align="left">
            <asp:RadioButton ID="rbtReceiveDate" runat="server" GroupName="rbtDateType" /><%=this.GetLocalized("代收日區間") %>
            <asp:TextBox ID="tbxSReceiveDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
            <asp:TextBox ID="tbxEReceiveDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        </div>
    </td>
</tr>
<tr id="trAccountDateRange">
    <td colspan="2">
        <div align="left">
            <asp:RadioButton ID="rbtAccountDate" runat="server" GroupName="rbtDateType" /><%=this.GetLocalized("入帳日區間") %>
            <asp:TextBox ID="tbxSAccountDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
            <asp:TextBox ID="tbxEAccountDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left">
            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoSearchField" runat="server">
                <asp:ListItem Value="StuId">學號</asp:ListItem>
                <asp:ListItem Value="CancelNo">虛擬帳號</asp:ListItem>
                <asp:ListItem Value="IdNumber">身分證字號</asp:ListItem>
            </asp:RadioButtonList>
            <asp:TextBox ID="tbxSearchValue" MaxLength="16" runat="server"></asp:TextBox>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyLinkButton ID="ccbtnDownload" runat="server" LocationText="下載XLS" CommandArgument="XLS" OnClick="ccbtnDownload_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnDownloadODS" runat="server" LocationText="下載ODS" CommandArgument="ODS" OnClick="ccbtnDownload_Click"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
