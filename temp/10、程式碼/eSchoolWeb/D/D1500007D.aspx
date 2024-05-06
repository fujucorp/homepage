<%@ Page Title="土地銀行 - 代收學雜費服務網 - 查閱檔案上傳結果(明細)" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1500007D.aspx.cs" Inherits="eSchoolWeb.D.D1500007D" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" />

<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
<tr>
	<th width="20%"><%= GetLocalized("業務別碼") %>：</th>
	<td><asp:Label ID="labReceiveType" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("學年") %>：</th>
	<td><asp:Label ID="labYearId" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("學期") %>：</th>
	<td><asp:Label ID="labTermId" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("功能名稱") %>：</th>
	<td><asp:Label ID="labJtypeid" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("使用者帳號") %>：</th>
	<td><asp:Label ID="labOwner" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("上傳日期時間") %>：</th>
	<td><asp:Label ID="labC_Date" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("費用別") %>：</th>
	<td><asp:Label ID="labReceiveId" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("批號") %>：</th>
	<td><asp:Label ID="labSerior_No" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("上傳檔案") %>：</th>
	<td><asp:Label ID="labFileName" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("狀態") %>：</th>
	<td><asp:Label ID="labStatus" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%= GetLocalized("說明") %>：</th>
	<td><asp:Label ID="labLog" runat="server" Text=""></asp:Label></td>
</tr>
</table>

<div class="button">
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
