<%@ Page Title="土地銀行 - 代收學雜費服務網 - 匯入委扣回復資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2100007.aspx.cs" Inherits="eSchoolWeb.B.B2100007" %>
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
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" />

<!--//表格_修改----------------------------------------------------------------->
<table id="modify" class="modify" summary="表格_修改" width="100%">
<tr>
    <th><%= GetLocalized("上傳委扣回復媒體檔案") %>：</th>
    <td>
        <asp:FileUpload ID="fileUpload" runat="server" Width="90%" />
    </td>
</tr>
</table>

<div class="modify">
    <asp:TextBox ID="tbxResult" runat="server" TextMode="MultiLine" Rows="25" Columns="100" ViewStateMode="Disabled" ReadOnly="true" Wrap="false"></asp:TextBox>
</div>

<div class="button">
    <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click"><%= GetLocalized("上傳") %></asp:LinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
