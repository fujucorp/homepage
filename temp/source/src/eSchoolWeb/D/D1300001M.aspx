<%@ Page Title="土地銀行 - 代收學雜費服務網 - 一般收費標準檔" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300001M.aspx.cs" Inherits="eSchoolWeb.D.D1300001M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    .max-w180px {
        max-width: 180px;
    }
</style>
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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" AutoGetDataBound="false" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" AutoGetDataBound="false" />

<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
<tr>
	<th colspan="4">
		<div align="left">
			<cc:MyLabel ID="MyLabel1" runat="server" LocationText="院別"></cc:MyLabel>：<asp:DropDownList ID="ddlCollegeId" runat="server"></asp:DropDownList>&nbsp;&nbsp;
			<cc:MyLabel ID="MyLabel2" runat="server" LocationText="科系"></cc:MyLabel>：<asp:DropDownList ID="ddlMajorId" runat="server" CssClass="max-w180px"></asp:DropDownList>&nbsp;&nbsp;<br />
			<cc:MyLabel ID="MyLabel3" runat="server" LocationText="年級"></cc:MyLabel>：<asp:DropDownList ID="ddlStuGrade" runat="server"></asp:DropDownList>&nbsp;&nbsp;
			<cc:MyLabel ID="MyLabel4" runat="server" LocationText="班別"></cc:MyLabel>：<asp:DropDownList ID="ddlClassId" runat="server"></asp:DropDownList>
		</div>
	</th>
</tr>
<tr>
	<th colspan="4">
		<div align="left">
			<cc:MyLabel ID="MyLabel5" runat="server" LocationText="計算順序"></cc:MyLabel>：
			<asp:TextBox ID="tbxOrder" Text="0" runat="server" MaxLength="5"></asp:TextBox>
		</div>
	</th>
</tr>
<tr>
	<td colspan="4" align="left">
		<table class="#" width="100%">
		    <tr>
			    <th>
				    <div align="center"><cc:MyLabel ID="MyLabel6" runat="server" LocationText="收入科目"></cc:MyLabel></div>
			    </th>
			    <th>
				    <div align="center"><cc:MyLabel ID="MyLabel7" runat="server" LocationText="繳費金額"></cc:MyLabel></div>
			    </th>
		    </tr>
		    <asp:Literal ID="litHtml" runat="server"></asp:Literal>
		</table>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
