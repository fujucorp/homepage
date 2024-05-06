<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2300001D.aspx.cs" Inherits="eSchoolWeb.B.B2300001D" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <th>
        <div style="text-align:center">
        <%= GetLocalized("編號") %>：<asp:Label ID="labStamp" runat="server" ></asp:Label>
        </div>
    </th>
</tr>
<tr>
    <th>
        <div style="text-align:center" id="divResult">
            <asp:Label ID="labList" runat="server"></asp:Label>
            <asp:LinkButton ID="lbtnQuery" runat="server" CssClass="btn" OnClick="lbtnQuery_Click" >檢查處理狀態</asp:LinkButton>
        </div>
    </th>
</tr>
<tr>
    <td>
        <table class="#" width="100%">
        <tr><td><div align="center">此作業較耗時，請耐心等候。</div></td></tr>
        <tr style="display:none"><td>&nbsp;</td></tr>
        <tr style="display:none"><td>&nbsp;</td></tr>
        <tr style="display:none"><td><div align="center">系統每一分鐘會自動檢查處理狀態，請勿離開此頁面</div></td></tr>
        </table>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
<script type="text/javascript">
    $(function () {
        var lbtn = $('#divResult > a');
        if (lbtn.length > 0) {
            setTimeout(function () {
                $('#divResult > a')[0].click();
            }, 1000 * 60);
        }
    });
</script>
</asp:Content>
