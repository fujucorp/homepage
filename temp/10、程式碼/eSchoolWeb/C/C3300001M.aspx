<%@ Page Title="土地銀行 - 代收學雜費服務網 - 人工銷帳" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3300001M.aspx.cs" Inherits="eSchoolWeb.C.C3300001M" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<!--//表格_修改----------------------------------------------------------------->
<table id="result1" class="result" summary="查詢結果" width="100%">
	<tr>
        <td colspan="2" style="text-align:center"><%= GetLocalized("銷帳問題檔") %></td>
    </tr>
    <tr>
        <th width="30%"><%= GetLocalized("虛擬帳號") %>：</th>
        <td>
            <asp:Label ID="labProblemCancelNo" runat="server" Text=""></asp:Label>
        </td>
    </tr>
	<tr>
        <th width="30%"><%= GetLocalized("實繳金額") %>：</th>
        <td><asp:Label ID="labPayAmount" runat="server" Text=""></asp:Label></td>
    </tr>
	<tr>
        <th width="30%"><%= GetLocalized("繳款方式") %>：</th>
        <td><asp:Label ID="labReceiveWay" runat="server" Text=""></asp:Label></td>
    </tr>
	<tr>
        <th width="30%"><%= GetLocalized("代收日") %>：</th>
        <td><asp:Label ID="labReceiveDate" runat="server" Text=""></asp:Label></td>
    </tr>
	<tr>
        <th width="30%"><%= GetLocalized("入帳日") %>：</th>
        <td><asp:Label ID="labAccountDate" runat="server" Text=""></asp:Label></td>
    </tr>
	<tr>
        <th width="30%"><%= GetLocalized("問題註記") %>：</th>
        <td><asp:Label ID="labProblemRemark" runat="server" Text=""></asp:Label></td>
    </tr>
    <div id="divQuery" runat="server">
	<tr>
        <th colspan="2">&nbsp;</th>
    </tr>
	<tr>
        <td colspan="2" style="text-align:center"><%= GetLocalized("繳費資料") %></td>
    </tr>
    <tr>
        <th width="30%"><%= GetLocalized("虛擬帳號") %>：</th>
        <td><asp:TextBox ID="tbxCancelNo" runat="server" MaxLength="16" Width="80%"></asp:TextBox></td>
    </tr>
    </div>
</table>
    
<div id="divGoNext" runat="server">
    <div class="button">
	    <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click"><%= GetLocalized("下一步") %></asp:LinkButton>
    </div>
</div>

<div id="divNextPage" runat="server">
    <table id="result2" class="result" summary="查詢結果" width="100%">
         <tr>
            <td colspan="2" style="text-align:center"><%= GetLocalized("繳費資料") %></td>
        </tr>
        <tr>
            <th width="30%"><%= GetLocalized("虛擬帳號") %>：</th>
            <td><asp:Label ID="labCancelNo" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <th width="30%"><%= GetLocalized("學號") %>：</th>
            <td><asp:Label ID="labStuId" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <th width="30%"><%= GetLocalized("姓名") %>：</th>
            <td><asp:Label ID="labName" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <th width="30%"><%= GetLocalized("應繳金額") %>：</th>
            <td><asp:Label ID="labReceiveAmount" runat="server" Text=""></asp:Label></td>
        </tr>    
    </table>
    
    <div class="button">
	    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
    </div>
</div>
</asp:Content>
