<%@ Page Title="土地銀行 - 代收學雜費服務網 - 每日銷帳結果查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3400001.aspx.cs" Inherits="eSchoolWeb.C.C3400001" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Option" Filter2ControlID="ucFilter2" YearDefaultKind="All" TermDefaultKind="All" YearDefaultMode="ByKind" TermDefaultMode="ByKind" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" AutoPostBack="false" ReceiveDefaultKind="All" ReceiveDefaultMode="ByKind" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th width="30%">入帳日期區間：</th>
    <td >
        <asp:TextBox ID="tbxSAccountDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
        <asp:TextBox ID="tbxEAccountDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
</tr>
</table>

<asp:Literal ID="litResult" runat="server"></asp:Literal>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyLinkButton ID="ccbtnExport" runat="server" LocationText="匯出XLS" CommandArgument="XLS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnExportODS" runat="server" LocationText="匯出ODS" CommandArgument="ODS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
</div>


</asp:Content>
