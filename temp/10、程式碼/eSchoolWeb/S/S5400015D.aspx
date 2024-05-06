<%@ Page Title="土地銀行 - 代收學雜費服務網 - 首頁功能日誌查詢(明細)" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400015D.aspx.cs" Inherits="eSchoolWeb.S.S5400015D" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    td.pre {
        display: block;
        max-height:80px;
        overflow: auto;
        white-space:pre;
    }
    th {
        width: 100px;
        min-width: 80px;
        max-width: 100px;
        white-space : nowrap;
    }
    td {
        width: 530px;
    }
    th span, td span {
        padding: 0px !important;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="condition" class="condition" summary="表格_修改" width="100%">
<tr>
	<th><%= GetLocalized("任務編號") %>：</th>
	<td><asp:Label ID="labTaskNo" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("功能") %>：</th>
	<td><asp:Label ID="labRequestId" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("操作") %>：</th>
	<td><asp:Label ID="labRequestKind" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("功能說明") %>：</th>
	<td><asp:Label ID="labRequestDesc" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("請求時間") %>：</th>
	<td><asp:Label ID="labRequestTime" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("網頁參數") %>：</th>
	<td class="pre"><asp:Literal ID="labRequestArgs" runat="server"></asp:Literal></td>
</tr>
<tr>
	<th><%= GetLocalized("網站主機") %>：</th>
	<td><asp:Label ID="labWebMachine" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("Session ID") %>：</th>
	<td><asp:Label ID="labSessionId" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("用戶 IP") %>：</th>
	<td><asp:Label ID="labClientIp" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("使用者單位類別") %>：</th>
	<td><asp:Label ID="labUserUnitKind" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("使用者單位代碼") %>：</th>
	<td><asp:Label ID="labUserUnitId" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("使用者登入帳號") %>：</th>
	<td><asp:Label ID="labUserLoginId" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("回應時間") %>：</th>
	<td><asp:Label ID="labResponseTime" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("回應資料") %>：</th>
	<td class="pre"><asp:Literal ID="litResponseData" runat="server"></asp:Literal></td>
</tr>
<tr>
	<th><%= GetLocalized("狀態代碼") %>：</th>
	<td><asp:Label ID="labStatusCode" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("狀態訊息") %>：</th>
	<td><asp:Label ID="labStatusMessage" runat="server"></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("日誌時間") %>：</th>
	<td><asp:Label ID="labLogTime" runat="server"></asp:Label></td>
</tr>
</table>

<div class="button">
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
