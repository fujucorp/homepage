<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護學年代碼" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600001M.aspx.cs" Inherits="eSchoolWeb.S.S5600001M" %>
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
    
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
    <th width="30%"><%= GetLocalized("學年代碼") %>：</th>
    <td>
        <asp:TextBox ID="tbxYearId" runat="server" MaxLength="3" Width="80%"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <th><%= GetLocalized("學年名稱") %>：</th>
    <td>
        <asp:TextBox ID="tbxYearName" runat="server" MaxLength="20" Width="80%"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <th><%= GetLocalized("學年英文名稱") %>：</th>
    <td>
        <asp:TextBox ID="tbxYearEName" runat="server" MaxLength="140" Width="80%"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <th><%= GetLocalized("資料啟用") %>：</th>
    <td>
        <asp:DropDownList ID="ddlEnabled" runat="server"></asp:DropDownList>
        &nbsp;&nbsp;<font color="red">(＊請勿隨意異動此項設定，以免影響資料正確性)</font>
    </td>
    </tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
