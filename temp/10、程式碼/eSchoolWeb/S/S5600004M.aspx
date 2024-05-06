<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護代收管道管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600004M.aspx.cs" Inherits="eSchoolWeb.S.S5600004M" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
	<tr>
        <th><%= GetLocalized("管道代碼") %>：</th>
        <td><asp:TextBox ID="tbxChannelId" runat="server" MaxLength="4" Width="30%"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%= GetLocalized("管道名稱") %>：</th>
        <td><asp:TextBox ID="tbxChannelName" runat="server" MaxLength="50" Width="80%"></asp:TextBox></td>
    </tr>
	<tr>
        <th><%= GetLocalized("啟用狀態") %>：</th>
        <td><asp:CheckBox ID="chkIsUsing" Checked="true" runat="server" />是</td>
    </tr>
    <tr>
        <th><%= GetLocalized("管道收款上限金額") %>：</th>
        <td><asp:TextBox ID="tbxMaxMoney" runat="server" MaxLength="5" Width="80%"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%= GetLocalized("此管道是否為學雜費預設管道") %>：</th>
        <td>
            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoDefaultFlag" runat="server">
                <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                <asp:ListItem Value="1">是</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("此管道是否要處理手續費") %>：</th>
        <td>
            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoProcessFee" runat="server">
                <asp:ListItem Selected="True" Value="N">否</asp:ListItem>
                <asp:ListItem Value="Y">是</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th><%= GetLocalized("此管道的彙總管道") %>：</th>
        <td>
            <asp:DropDownList ID="ddlCategoryId" runat="server"></asp:DropDownList>
        </td>
    </tr>
</table>
          
<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>      
</asp:Content>
