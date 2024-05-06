<%@ Page Title="土地銀行 - 代收學雜費服務網 - 清除虛擬帳號" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2200004.aspx.cs" Inherits="eSchoolWeb.B.B2200004" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <td>
        <asp:RadioButton ID="rbtByUpNo" runat="server" GroupName="ByKind" /><%= GetLocalized("指定批號") %>：
    </td>
    <td>
        <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
    </td>
</tr>
<tr>
    <td>
        <asp:RadioButton ID="rbtByStuId" runat="server" GroupName="ByKind" /><%= GetLocalized("指定學號") %>：
    </td>
    <td>
        <asp:TextBox ID="tbxStuId" runat="server" TextMode="MultiLine" Columns="80" Rows="10"></asp:TextBox><br />
        (多筆資料以逗號或換行隔開，最多100筆)
    </td>
</tr>
<tr>
    <td>
        <asp:RadioButton ID="rbtByCancelNo" runat="server" GroupName="ByKind" /><%= GetLocalized("指定虛擬帳號") %>：
    </td>
    <td>
        <asp:TextBox ID="tbxCancelNo" MaxLength="16" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div style="text-align:left">
            請注意：<br />
            1. 已繳費的資料無法清除虛擬帳號。<br />
            2. 指定清除虛擬帳號的資料中，如包含已繳費的資料，系統會略過這些資料。<br />
            3. 如此商家代號有設定中國信託代收管道，會同時重置通知中國信託註記。<br />
            4. 自訂虛擬帳號的資料也會被清除，請小心使用。<br />
        </div>
    </td>
</tr>
</table>

<div class="modify">
    <asp:Label ID="labLog" runat="server" EnableViewState="false"></asp:Label>
</div>

<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

</asp:Content>
