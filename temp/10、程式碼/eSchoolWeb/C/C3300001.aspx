<%@ Page Title="土地銀行 - 代收學雜費服務網 - 人工銷帳" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3300001.aspx.cs" Inherits="eSchoolWeb.C.C3300001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
	<tr>
        <td colspan="2" style="text-align:center"><%= GetLocalized("銷帳問題檔") %></td>
    </tr>
    <tr>
        <th width="30%"><%= GetLocalized("有問題虛擬帳號") %>：</th>
        <td><asp:TextBox ID="tbxProblemCancelNo" runat="server" MaxLength="16" Width="80%"></asp:TextBox></td>
    </tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
</div>

</asp:Content>
