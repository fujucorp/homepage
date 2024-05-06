<%@ Page Title="土地銀行 - 代收學雜費服務網 - 學分費收費標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300002M.aspx.cs" Inherits="eSchoolWeb.D.D1300002M" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="true" TermVisible="true" AutoGetDataBound="true" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" AutoGetDataBound="true" />

<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
		<th><cc:MyLabel ID="MyLabel1" runat="server" LocationText="院別"></cc:MyLabel>：</th>
        <td><asp:DropDownList ID="ddlCollegeId" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
		<th><cc:MyLabel ID="MyLabel2" runat="server" LocationText="學分費單價"></cc:MyLabel>：</th>
        <td><asp:TextBox ID="tbxCreditPrice" Text="0" runat="server"></asp:TextBox></td>
    </tr>
	<tr>
		<th><cc:MyLabel ID="MyLabel3" runat="server" LocationText="所屬收入科目"></cc:MyLabel>：</th>
        <td><asp:DropDownList ID="ddlReceiveItem" runat="server"></asp:DropDownList></td>
    </tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
