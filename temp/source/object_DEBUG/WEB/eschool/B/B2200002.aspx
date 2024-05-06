<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生虛擬帳號" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2200002.aspx.cs" Inherits="eSchoolWeb.B.B2200002" %>

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
    <td>
        <div style="text-align:center">
            <%= GetLocalized("請選擇您上傳的批號") %>：
            <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
        </div>
    </td>
</tr>
<!--
<tr>
    <th><div style="text-align:center"><%= GetLocalized("流水號起始位置") %></div></th>
</tr>
<tr>
    <td>
        <div style="text-align:left">
            <asp:RadioButton ID="rbtSeriorType1" runat="server" GroupName="SeriorNoType" Checked="true" /><%= GetLocalized("使用目前流水號位置") %><br />
            <asp:RadioButton ID="rbtSeriorType2" runat="server" GroupName="SeriorNoType" /><%= GetLocalized("自定流水號位置") %> &nbsp;&nbsp;<asp:TextBox ID="tbxSeriorNoStart" runat="server" MaxLength="11"></asp:TextBox>
        </div>
    </td>
</tr>
<tr>
    <th>
        <div style="text-align:center"><%= GetLocalized("代收管道") %></div>
    </th>
</tr>
<tr>
    <td>
        <div style="text-align:left">
            <asp:CheckBox ID="cbxChannel0" runat="server" Enabled="false" Checked="true" /><%= GetLocalized("臨櫃") %>&nbsp;
            <asp:CheckBox ID="cbxChannel2" runat="server" Enabled="false" /><%= GetLocalized("超商") %>&nbsp;
        </div>
    </td>
</tr>
-->
<tr>
	<th><div style="text-align:center"><%= GetLocalized("流水號排序原則") %></div></th>
</tr>
<tr>
    <td>
        <div align="left">
            <asp:RadioButton ID="rbtSeriorSortType1" runat="server" GroupName="SeriorSortType" /><%= GetLocalized("照下列順序編排流水號(依標準計算時請選此項)") %><br />
            <div style="padding-left:10px;">
                1.&nbsp;<asp:DropDownList ID="ddlSeriorSortField1" runat="server"></asp:DropDownList>&nbsp;
                2.&nbsp;<asp:DropDownList ID="ddlSeriorSortField2" runat="server"></asp:DropDownList>&nbsp;
                3.&nbsp;<asp:DropDownList ID="ddlSeriorSortField3" runat="server"></asp:DropDownList>&nbsp;
                4.&nbsp;<asp:DropDownList ID="ddlSeriorSortField4" runat="server"></asp:DropDownList>&nbsp;
                5.&nbsp;<asp:DropDownList ID="ddlSeriorSortField5" runat="server"></asp:DropDownList>&nbsp;
            </div>
            <asp:RadioButton ID="rbtSeriorSortType2" runat="server" GroupName="SeriorSortType" Checked="true" /><%= GetLocalized("照原接收資料順序編排流水號") %>
        </div>
    </td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
