<%@ Page Title="土地銀行 - 代收學雜費服務網 - 排程作業管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600009M.aspx.cs" Inherits="eSchoolWeb.S.S5600009M" %>

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
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th style="width:30%"><%= GetLocalized("服務名稱") %>：</th>
	<td>
		<asp:DropDownList ID="ddlJobCubeType" runat="server"></asp:DropDownList>
	</td>
</tr>
<%--[MDY:2018xxxx] 改用 ServiceConfig2 物件，所以調整週期相關輸入項目--%>
<tr>
	<th><%= GetLocalized("啟動週期間隔") %>：</th>
	<td id="tdCycle">
		<asp:TextBox ID="tbxStartDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox> ~
		<asp:TextBox ID="tbxEndDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox> &nbsp;
		每 <asp:TextBox ID="tbxDaysInterval" runat="server" MaxLength="3" Width="100px"></asp:TextBox> &nbsp; 日
	</td>
</tr>
<tr>
	<th><%= GetLocalized("啟動時間間隔")%>：</th>
	<td>
		<asp:TextBox ID="tbxStartTime" runat="server" MaxLength="5" Width="100px"></asp:TextBox> ~
		<asp:TextBox ID="tbxEndTime" runat="server" MaxLength="5" Width="100px"></asp:TextBox> &nbsp;
		每 <asp:TextBox ID="tbxTimeInterval" runat="server" MaxLength="4" Width="100px"></asp:TextBox> 分鐘
	</td>
</tr>
<tr>
	<th><%= GetLocalized("應用程式路徑檔名")%>：</th>
	<td>
		<asp:TextBox ID="tbxFileName" runat="server" MaxLength="256" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><%= GetLocalized("應用程式命令參數")%>：</th>
	<td>
		<asp:TextBox ID="tbxArguments" runat="server" MaxLength="256" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr>
	<th><%= GetLocalized("狀態")%>：</th>
	<td>
		<asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript">
    function changeEndTimeUI() {
        var unit = $('#tdCycle > select option:selected').val();
        if (unit == "Day") {
            $('#spnEndTime').hide();
        } else {
            $('#spnEndTime').show();
        }
    }
    $(function () {
        $('#tdCycle > select').change(function () {
            changeEndTimeUI();
            //var unit = $(this).val();
            //if (unit == "Day") {
            //    $('#spnEndTime').hide();
            //} else {
            //    $('#spnEndTime').show();
            //}
        });

        changeEndTimeUI();
    });
</script>
</asp:Content>
