<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600011M.aspx.cs" Inherits="eSchoolWeb.S.S5600011M" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th width="30%"><cc:MyLabel ID="cclabFuncId" runat="server" LocationText="代收類別"></cc:MyLabel>：</th>
        <td>
            <asp:Label ID="labReceiveTypeName" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <th><cc:MyLabel ID="cclabFuncName" runat="server" LocationText="繳費管道"></cc:MyLabel>：</th>
        <td>
            <asp:DropDownList ID="ddlChannelId" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelId_SelectedIndexChanged" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th><cc:MyLabel ID="MyLabel1" runat="server" LocationText="繳款方式"></cc:MyLabel>：</th>
        <td>
            <asp:DropDownList ID="ddlBarcodeId" AutoPostBack="true" OnSelectedIndexChanged="ddlBarcodeId_SelectedIndexChanged" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th><cc:MyLabel ID="MyLabel2" runat="server" LocationText="繳費下限 (含)"></cc:MyLabel>：</th>
        <td>
            <asp:TextBox ID="tbxMinMoney" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><cc:MyLabel ID="MyLabel3" runat="server" LocationText="繳費上限 (含)"></cc:MyLabel>：</th>
        <td>
            <asp:TextBox ID="tbxMaxMoney" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><cc:MyLabel ID="MyLabel4" runat="server" LocationText="應付手續費"></cc:MyLabel>：</th>
        <td>
            <asp:TextBox ID="tbxChannelCharge" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><cc:MyLabel ID="MyLabel5" runat="server" LocationText="銀行或學校負擔"></cc:MyLabel>：</th>
        <td>
            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoRCFlag" runat="server">
                <asp:ListItem Selected="True" Value="3">繳款人負擔</asp:ListItem>
                <asp:ListItem Value="2">學校</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th><cc:MyLabel ID="MyLabel6" runat="server" LocationText="繳款人手續"></cc:MyLabel>：</th>
        <td>
            <asp:TextBox ID="tbxRCSPay" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><cc:MyLabel ID="MyLabel7" runat="server" LocationText="學校負擔手續"></cc:MyLabel>：</th>
        <td>
            <asp:TextBox ID="tbxRCBPay" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
        </td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
