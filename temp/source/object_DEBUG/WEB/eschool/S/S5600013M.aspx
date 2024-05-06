<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護Q&A" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600013M.aspx.cs" Inherits="eSchoolWeb.S.S5600013M" %>
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
        <th width="30%"><%= GetLocalized("問題類型") %>：</th>
        <td>
            
            <asp:DropDownList ID="ddlType" runat="server">
                <asp:ListItem Text="登入問題"   Value="1"></asp:ListItem>
                <asp:ListItem Text="產生繳費單" Value="2"></asp:ListItem>
                <asp:ListItem Text="信用卡繳費" Value="3"></asp:ListItem>
                <asp:ListItem Text="其他問題" Value="4"></asp:ListItem>
            </asp:DropDownList>
	    </td>
    </tr>
    <tr>
        <th><%= GetLocalized("排序") %>：</th>
        <td>
		    <asp:TextBox ID="tbxSort" runat="server" MaxLength="5" Width="80%"></asp:TextBox>
	    </td>
    </tr>
    <tr>
        <th><%= GetLocalized("問題") %>：</th>
        <td>
            <asp:TextBox ID="tbxQ" TextMode="MultiLine" CssClass="tbxQ" Width="500px" MaxLength="1000" Rows="5" runat="server"></asp:TextBox>
	    </td>
    </tr>
    <tr>
        <th><%= GetLocalized("答案") %>：</th>
        <td>
            <asp:TextBox ID="tbxA" TextMode="MultiLine" CssClass="tbxA" Width="500px" MaxLength="1000" Rows="5" runat="server"></asp:TextBox>
	    </td>
    </tr>
</table>

<div class="button">
    <input type="hidden" id="hidHtmlContentQ" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidHtmlContentA" runat="server" clientidmode="Static" />
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click" OnClientClick="return check();"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server" OnClientClick="return goBack()"></cc:MyGoBackButton>
</div>
    
<script type="text/javascript">
    //取得 html encode 後的字串
    function htmlEncode(value) {
        if (value) {
            var lines = value.split(/\r\n|\r|\n/);
            for (var i = 0; i < lines.length; i++) {
                lines[i] = $('<div/>').text(lines[i]).html();
            }
            return lines.join('\r\n');
        }
        return '';
    }

    function check() {
        var contentQ = $('.tbxQ').val().prune();
        var contentA = $('.tbxA').val().prune();
        $('#hidHtmlContentQ').val(htmlEncode(contentQ));
        $('#hidHtmlContentA').val(htmlEncode(contentA));
        $('.tbxQ').attr("disabled", true);
        $('.tbxA').attr("disabled", true);
        return true;
    }

    function goBack() {
        $('.tbxQ').attr("disabled", true);
        $('.tbxA').attr("disabled", true);
        return true;
    }
</script>
</asp:Content>
