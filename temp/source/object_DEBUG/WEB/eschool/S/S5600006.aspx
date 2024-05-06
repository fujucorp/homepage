<%@ Page Title="土地銀行 - 代收學雜費服務網 - 廣告管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600006.aspx.cs" Inherits="eSchoolWeb.S.S5600006" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
.nowrap {
	text-wrap:none;
	white-space: nowrap;
	}
.break-word {
	word-wrap: break-word;
}
</style>
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
    <th nowrap="nowrap" style="width:120px; text-align:center;">廣告版位</th>
    <th nowrap="nowrap" style="width:60px; text-align:center;">廣告種類</th>
    <th nowrap="nowrap" style="text-align:center;"">廣告連結</th>
    <th nowrap="nowrap" style="width:170px; text-align:center;">廣告預覽</th>
    <th nowrap="nowrap" style="width:40px; text-align:center;">設定</th>
</tr>
<tr>
    <td><asp:Label ID="labAD001Name" runat="server" Text="" CssClass="nowrap" ToolTip="500 x 300"></asp:Label></td>
    <td><asp:Label ID="labAD001Kind" runat="server" Text="" CssClass="nowrap"></asp:Label></td>
    <td><asp:Label ID="labAD001Url" runat="server" Text="" CssClass="break-word"></asp:Label></td>
    <td class="C">
        <asp:Image ID="ImgAD001" runat="server" Width="166" Height="100" />
    </td>
    <td class="nowrap C">
        <cc:MyLinkButton ID="ccbtnAD001Set" runat="server" CssClass="btn" LocationText="設定" CommandArgument="AD001" OnClick="ccbtnSet_Click"></cc:MyLinkButton>
    </td>
</tr>
<tr>
    <td><asp:Label ID="labAD002Name" runat="server" Text="" CssClass="nowrap" ToolTip="120 x 60"></asp:Label></td>
    <td><asp:Label ID="labAD002Kind" runat="server" Text="" CssClass="nowrap"></asp:Label></td>
    <td><asp:Label ID="labAD002Url" runat="server" Text=""  CssClass="break-word"></asp:Label></td>
    <td class="C">
        <asp:Image ID="ImgAD002" runat="server" Width="120" Height="60" />
    </td>
    <td class="nowrap C">
        <cc:MyLinkButton ID="ccbtnAD002Set" runat="server" CssClass="btn" LocationText="設定" CommandArgument="AD002" OnClick="ccbtnSet_Click"></cc:MyLinkButton>
    </td>
</tr>
<tr>
    <td><asp:Label ID="labAD003Name" runat="server" Text="" CssClass="nowrap" ToolTip="120 x 60"></asp:Label></td>
    <td><asp:Label ID="labAD003Kind" runat="server" Text="" CssClass="nowrap"></asp:Label></td>
    <td><asp:Label ID="labAD003Url" runat="server" Text=""  CssClass="break-word"></asp:Label></td>
    <td class="C">
        <asp:Image ID="ImgAD003" runat="server" Width="120" Height="60" />
    </td>
    <td class="nowrap C">
        <cc:MyLinkButton ID="ccbtnAD003Set" runat="server" CssClass="btn" LocationText="設定" CommandArgument="AD003" OnClick="ccbtnSet_Click"></cc:MyLinkButton>
    </td>
</tr>
<tr>
    <td><asp:Label ID="labAD004Name" runat="server" Text="" CssClass="nowrap" ToolTip="120 x 60"></asp:Label></td>
    <td><asp:Label ID="labAD004Kind" runat="server" Text="" CssClass="nowrap"></asp:Label></td>
    <td><asp:Label ID="labAD004Url" runat="server" Text=""  CssClass="break-word"></asp:Label></td>
    <td class="C">
        <asp:Image ID="ImgAD004" runat="server" Width="120" Height="60" />
    </td>
    <td class="nowrap C">
        <cc:MyLinkButton ID="ccbtnAD004Set" runat="server" CssClass="btn" LocationText="設定" CommandArgument="AD004" OnClick="ccbtnSet_Click"></cc:MyLinkButton>
    </td>
</tr>
<tr>
    <td><asp:Label ID="labAD005Name" runat="server" Text="" CssClass="nowrap" ToolTip="120 x 60"></asp:Label></td>
    <td><asp:Label ID="labAD005Kind" runat="server" Text="" CssClass="nowrap"></asp:Label></td>
    <td><asp:Label ID="labAD005Url" runat="server" Text=""  CssClass="break-word"></asp:Label></td>
    <td class="C">
        <asp:Image ID="ImgAD005" runat="server" Width="120" Height="60" />
    </td>
    <td class="nowrap C">
        <cc:MyLinkButton ID="ccbtnAD005Set" runat="server" CssClass="btn" LocationText="設定" CommandArgument="AD005" OnClick="ccbtnSet_Click"></cc:MyLinkButton>
    </td>
</tr>
</table>

<div class="button">
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
