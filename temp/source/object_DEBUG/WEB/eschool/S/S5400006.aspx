<%@ Page Title="土地銀行 - 代收學雜費服務網 - D38資料上傳" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400006.aspx.cs" Inherits="eSchoolWeb.S.S5400006" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" Filter2ControlID="ucFilter2" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />
<uc:Filter2 ID="ucFilter2" runat="server" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtQType1" runat="server" GroupName="QType" /><%= GetLocalized("產生所有繳費單") %>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtQType2" runat="server" GroupName="QType" /><%= GetLocalized("自訂產生繳費單流水號") %> &nbsp;&nbsp;
            <asp:TextBox ID="tbxSSeriorNo" runat="server" MaxLength="9"></asp:TextBox>～<asp:TextBox ID="tbxESeriorNo" runat="server" MaxLength="9"></asp:TextBox>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtQType3" runat="server" GroupName="QType" /><%= GetLocalized("依批號產生，批號") %> &nbsp;&nbsp;
            <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtQType4" runat="server" GroupName="QType" /><%= GetLocalized("依學號產生，學號") %>： &nbsp;&nbsp;
            <asp:TextBox ID="tbxStudentId" runat="server" MaxLength="16"></asp:TextBox>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:left">
            <%= GetLocalized("處理類型") %>：<asp:RadioButtonList ID="rblUpdKind" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:RadioButtonList>
            &nbsp;(選擇【刪除已上傳資料】時，僅處理註記上傳過的資料。)
        </div>
    </th>
</tr>
</table>

<table class="result" width="100%" id="tabResult" runat="server">
<tr>
    <th style="text-align:center;">
        此作業需要花費較多的時間處理，請耐心等候。<br /><br />
        <!-- 請先記下此次作業的序號 (<asp:Label ID="labJobNo" runat="server"></asp:Label>)，稍後可使用「D38資料查詢」查詢詳細的上傳處理結果。<br /><br /> -->
    </th>
</tr>
</table>

<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

</asp:Content>
