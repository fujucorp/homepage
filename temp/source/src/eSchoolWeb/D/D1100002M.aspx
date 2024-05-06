<%@ Page Title="土地銀行 - 代收學雜費服務網 - 部別代碼" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1100002M.aspx.cs" Inherits="eSchoolWeb.D.D1100002M" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" AutoGetDataBound="false" />
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th style="width:30%"><cc:MyLabel ID="cclabDepId" runat="server" LocationText="部別代碼"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxDeptId" runat="server" MaxLength="20" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabDeptName" runat="server" LocationText="部別名稱"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxDeptName" runat="server" MaxLength="40" Width="80%"></asp:TextBox>
	</td>
</tr>
<asp:PlaceHolder ID="phdDeptEName" runat="server" Visible="false">
<tr>
	<th><cc:MyLabel ID="cclabDeptEName" runat="server" LocationText="部別英文名稱"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxDeptEName" runat="server" MaxLength="140" Width="80%"></asp:TextBox>
	</td>
</tr>
</asp:PlaceHolder>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
