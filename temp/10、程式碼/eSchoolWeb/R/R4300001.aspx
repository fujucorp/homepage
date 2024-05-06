<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生退費媒體檔" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="R4300001.aspx.cs" Inherits="eSchoolWeb.R.R4300001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" ReceiveDefaultMode="First" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<table id="modify" class="modify" summary="表格_修改" width="100%">
	<tr>
	    <td width="90%">
		    <%=this.GetLocalized("媒體檔名稱") %>：
            <asp:TextBox ID="tbxFileName" runat="server"></asp:TextBox>(<%=this.GetLocalized("不含副檔名") %>)
	    </td>
	</tr>
	<tr>
	    <td>
		    <%=this.GetLocalized("選擇清冊批號") %>：            
            <asp:DropDownList ID="ddlSrNo" runat="server">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
	    <td>
		    <%=this.GetLocalized("輸入匯款日期") %>：
            <asp:TextBox ID="tbxDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
	    </td>
	</tr>
</table>
    
<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click" ></cc:MyOKButton>&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
    
</asp:Content>
