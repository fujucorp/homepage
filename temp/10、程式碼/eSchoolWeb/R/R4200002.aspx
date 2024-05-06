<%@ Page Title="土地銀行 - 代收學雜費服務網 - 下載退費清冊" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="R4200002.aspx.cs" Inherits="eSchoolWeb.R.R4200002" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" ReceiveDefaultMode="First" />

<table id="modify" class="modify" summary="表格_修改" width="100%">
	<tr>
	    <td>
            <%=this.GetLocalized("檔案名稱") %>：<asp:DropDownList ID="ddlFileName" runat="server"></asp:DropDownList>
	    </td>
	</tr>
</table>
        
<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
