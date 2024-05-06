<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生email繳費通知" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2300003.aspx.cs" Inherits="eSchoolWeb.B.B2300003" %>

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
            <asp:RadioButton ID="rbtRangeType0" runat="server" GroupName="RangeType" /><%= GetLocalized("產生所有繳費通知") %>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtRangeType1" runat="server" GroupName="RangeType" /><%= GetLocalized("自訂產生繳費單通知流水號") %> &nbsp;&nbsp;
            <asp:TextBox ID="tbxSeriorNoStart" runat="server" MaxLength="11"></asp:TextBox>～<asp:TextBox ID="tbxSeriorNoEnd" runat="server" MaxLength="11"></asp:TextBox>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtRangeType2" runat="server" GroupName="RangeType" /><%= GetLocalized("依批號產生，批號") %> &nbsp;&nbsp;
            <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtRangeType3" runat="server" GroupName="RangeType" /><%= GetLocalized("依學號產生，學號") %>： &nbsp;&nbsp;
            <asp:TextBox ID="tbxStudentId" runat="server" MaxLength="16"></asp:TextBox>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            *產生繳費通知功能說明：<br />
            1.執行此功能前請務必再次確認『學生資料』、『繳費金額』、『繳款期限』是否正確，以避免學生收到不正確的繳費訊息。<br />
            2.當您確定產生EMail繳費通知後，系統會在當日夜間整批寄發繳費通知至學生基本資料中設定的Email信箱。<br />
            3.若單筆學生資料有誤或須重新寄送，可利用單筆學生資料修改後重新執行『產生Email繳費通知』。<br />
            4.產生Email通知的條件:<br />
            &nbsp;&nbsp;˙該學生必須有Email資料<br />
            &nbsp;&nbsp;˙必須是未繳費的資料<br />
        </div>
    </th>
</tr>
</table>

<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
