<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護系統訊息公告(最新消息)" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600007M.aspx.cs" Inherits="eSchoolWeb.S.S5600007M" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th width="30%"><%= GetLocalized("公告主旨") %>：</th>
    <td>
        <asp:TextBox ID="tbxBoardSubject" runat="server" MaxLength="300" Width="90%"></asp:TextBox>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("公告日期") %>：</th>
    <td>
        <asp:TextBox ID="tbxStartDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("有效日期") %>：</th>
    <td>
        <asp:TextBox ID="tbxEndDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("公告內容型態") %>：</th>
    <td>
        <asp:DropDownList ID="ddlBoardType" runat="server"></asp:DropDownList>
    </td>
</tr>
<tr>
    <th><%= GetLocalized("公告位置") %>：</th>
    <td>
        <asp:DropDownList ID="ddlSchId" runat="server"></asp:DropDownList>
    </td>
</tr>
<%--[MDY:2018xxxx] 新增公告對象--%>
<tr id="trTarget" style="display:none;">
    <th><%= GetLocalized("公告對象") %>：</th>
    <td>
        <asp:DropDownList ID="ddlTarget" runat="server"></asp:DropDownList>
    </td>
</tr>
<%--[MDY:2018xxxx] 新增社群分享--%>
<tr>
    <th><%= GetLocalized("社群分享") %>：</th>
    <td>
        <asp:DropDownList ID="ddlShareFlag" runat="server"></asp:DropDownList>
    </td>
</tr>
<tr>
    <th width="30%"><%= GetLocalized("公告內容") %>：</th>
    <td>
        <asp:TextBox ID="tbxBoardContent" runat="server" CssClass="tbxBoardContent" TextMode="MultiLine" Height="500px" Width="90%"></asp:TextBox>
    </td>
</tr>
</table>

<div class="button">
    <input type="hidden" id="hidHtmlContent" runat="server" clientidmode="Static" />
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
        var content = $('.tbxBoardContent').val().trim();
        $('#hidHtmlContent').val(htmlEncode(content));
        $('.tbxBoardContent').attr("disabled", true);
        return true;
    }

    function goBack() {
        $('.tbxBoardContent').attr("disabled", true);
        return true;
    }

    function changeTarget() {
        var val = $("#<%= this.ddlSchId.ClientID %>").val();
        if (val == "<%= Entities.BoardTypeCodeTexts.STUDENT %>" || val == "<%= Entities.BoardTypeCodeTexts.SCHOOL %>") {
            $("#trTarget").show();
        } else {
            $("#trTarget").hide();
        }
    }

    $("#<%= this.ddlSchId.ClientID %>").change(function () {
        changeTarget();
    })

    changeTarget();
</script>
</asp:Content>
