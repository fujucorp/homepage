<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護代收管道管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600004M2.aspx.cs" Inherits="eSchoolWeb.S.S5600004M2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th><%= GetLocalized("繳款方式") %>：</th>
        <td><asp:TextBox ID="tbxBarcodeId" runat="server" MaxLength="3" Width="30%"></asp:TextBox></td>
    </tr>
	<tr>
        <th><%= GetLocalized("預設手續費") %>：</th>
        <td><asp:TextBox ID="tbxChannelCharge" runat="server" MaxLength="10" Width="30%"></asp:TextBox></td>
    </tr>
	<tr>
        <th><%= GetLocalized("繳費單是否包含手續費") %>：</th>
        <td>
            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoIncludePay" runat="server">
                <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                <asp:ListItem Value="1">是</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
	<tr>
        <th><%= GetLocalized("繳款人或學校負擔") %>：</th>
        <td>
            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoRCFlag" runat="server">
                <asp:ListItem Selected="True" Value="3">繳款人負擔</asp:ListItem>
                <asp:ListItem Value="2">學校負擔</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
	<tr>
        <th><%= GetLocalized("預設繳款人/學校手續費") %>：</th>
        <td><asp:TextBox ID="tbxRCSPay" runat="server" MaxLength="10" Width="30%"></asp:TextBox></td>
    </tr>
	<tr>
        <th><%= GetLocalized("預設繳費下限") %>：</th>
        <td><asp:TextBox ID="tbxMinMoney" runat="server" MaxLength="5" Width="30%"></asp:TextBox></td>
    </tr>
	<tr>
        <th><%= GetLocalized("預設繳費上限") %>：</th>
        <td><asp:TextBox ID="tbxMaxMoney" runat="server" MaxLength="5" Width="30%"></asp:TextBox></td>
    </tr>
</table>
          
<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:LinkButton ID="lbtnBack" runat="server" OnClick="lbtnBack_Click"><%= GetLocalized("回上一頁") %></asp:LinkButton>
</div>      
</asp:Content>
