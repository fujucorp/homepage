<%@ Page Title="土地銀行 - 代收學雜費服務網 - 學生繳費名冊" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3500002.aspx.cs" Inherits="eSchoolWeb.C.C3500002" %>

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
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" AutoPostBack="true" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<br/>
<!--//表格_修改----------------------------------------------------------------->
<table class="result" summary="查詢條件" width="100%">
<tr>
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("報表名稱") %>：
            <asp:TextBox ID="tbxReportName" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
        </div>
    </td>
</tr>
<tr>
    <td width="50%">
        <div align="left"><%=this.GetLocalized("批號") %>：
            <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
        </div>
    </td>
    <td width="50%">
        <div align="left" id="divReceiveStatus"><%=this.GetLocalized("繳費狀態") %>：
            <asp:DropDownList ID="ddlReceiveStatus" runat="server">
                <asp:ListItem Value="0">未繳</asp:ListItem>
                <asp:ListItem Value="1">已繳</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("群組明細程度") %>：
            <asp:RadioButtonList ID="rblGroupKind" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("說明項目") %>：
            <asp:CheckBoxList ID="cblOtherItems" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="4"></asp:CheckBoxList>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("收入科目") %>：<br />
            <asp:RadioButtonList ID="rblReceiveItems" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="4"></asp:RadioButtonList>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyLinkButton ID="ccbtnGenRpeortC" runat="server" LocationText="下載XLS" CommandArgument="XLS" OnClick="ccbtnGenRpeortC_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnGenRpeortCODS" runat="server" LocationText="下載ODS" CommandArgument="ODS" OnClick="ccbtnGenRpeortC_Click"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
