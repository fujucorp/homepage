<%@ Page Title="土地銀行 - 代收學雜費服務網 - 退費標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300010M.aspx.cs" Inherits="eSchoolWeb.D.D1300010M" %>

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
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" />
    
<!--//表格_修改----------------------------------------------------------------->
<table id="condition" class="condition" summary="表格_修改" width="100%">
    <tr>
	    <th width="30%"><cc:MyLabel ID="MyLabel8" runat="server" LocationText="退費名稱" />：</th>
        <td><asp:DropDownList ID="ddlReturnId" runat="server" AutoPostBack="true" ></asp:DropDownList></td>
    </tr>
	<tr>
	<td colspan="2">
		<table width="100%" class="#">
		    <tr>
                <th><div align="center"><cc:MyLabel ID="MyLabel2" runat="server" LocationText="收入科目" /></div></th>
                <th><div align="center"><cc:MyLabel ID="MyLabel1" runat="server" LocationText="退費分子" /></div></th>
                <th><div align="center"><cc:MyLabel ID="MyLabel7" runat="server" LocationText="退費分母" /></div></th>
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
