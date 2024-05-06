<%@ Page Title="土地銀行 - 代收學雜費服務網 - 報稅資料下載" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5500006.aspx.cs" Inherits="eSchoolWeb.S.S5500006" %>

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
<table id="information" class="information" summary="查詢結果" width="100%">  
<tr>
    <th>
        <%=this.GetLocalized("學校") %>：<asp:DropDownList ID="ddlSchool" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchool_SelectedIndexChanged"></asp:DropDownList>
    </th>
</tr>
</table>

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
	<tr>
		<th width="30%"><%=this.GetLocalized("請選擇要下載的檔案") %>：</th>
		<td>
			<asp:DropDownList ID="ddlFileName" runat="server"></asp:DropDownList>
		</td>
	</tr>
</table>
    
<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

</asp:Content>
