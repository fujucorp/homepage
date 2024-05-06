<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生繳費收據" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2400001.aspx.cs" Inherits="eSchoolWeb.B.B2400001" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtRange0" runat="server" GroupName="Range" /><%= GetLocalized("產生所有繳費收據") %>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtRange1" runat="server" GroupName="Range" /><%= GetLocalized("自訂產生繳費收據流水號") %> &nbsp;&nbsp;
            <asp:TextBox ID="tbxSeriorNoStart" runat="server" MaxLength="11"></asp:TextBox>～<asp:TextBox ID="tbxSeriorNoEnd" runat="server" MaxLength="11"></asp:TextBox>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtRange2" runat="server" GroupName="Range" /><%= GetLocalized("依批號產生，批號") %> &nbsp;&nbsp;
            <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtRange3" runat="server" GroupName="Range" /><%= GetLocalized("依學號產生，學號") %>： &nbsp;&nbsp;
            <asp:TextBox ID="tbxStudentId" runat="server" MaxLength="16"></asp:TextBox>
        </div>
    </th>
</tr>
<tr id="trLang" runat="server" visible="false">
    <th>
        <div style="text-align:left">
            <asp:RadioButtonList ID="rblLang" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
    </th>
</tr>
</table>

<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
