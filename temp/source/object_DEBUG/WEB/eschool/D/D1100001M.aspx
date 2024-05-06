<%@ Page Title="土地銀行 - 代收學雜費服務網 - 學期代碼" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1100001M.aspx.cs" Inherits="eSchoolWeb.D.D1100001M" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" TermVisible="false" AutoGetDataBound="false" />
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th style="width:30%"><%= GetLocalized("學期代碼") %>：</th>
	<td>
		<asp:TextBox ID="tbxTermId" runat="server" MaxLength="1" Width="100px"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><%= GetLocalized("學期名稱")%>：</th>
	<td>
		<asp:TextBox ID="tbxTermName" runat="server" MaxLength="40" Width="80%"></asp:TextBox>
	</td>
</tr>
<asp:PlaceHolder ID="phdTermEName" runat="server" Visible="false">
<tr>
	<th><%= GetLocalized("學期英文名稱")%>：</th>
	<td>
		<asp:TextBox ID="tbxTermEName" runat="server" MaxLength="140" Width="80%"></asp:TextBox>
	</td>
</tr>
</asp:PlaceHolder>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
