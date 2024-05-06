<%@ Page Title="土地銀行 - 代收學雜費服務網 - 繳費資料查詢" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5400004.aspx.cs" Inherits="eSchoolWeb.S.S5400004" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Option" YearVisible="false" TermVisible="false" />

<br/>

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <td>
        <div align="left" id="divCancelStatus"><%=this.GetLocalized("繳費狀態") %>
            <asp:DropDownList ID="ddlCancelStatus" runat="server">
                <asp:ListItem Value="0">未繳款</asp:ListItem>
                <asp:ListItem Value="1">已繳款未入帳</asp:ListItem>
                <asp:ListItem Value="2">已入帳</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr id="trReceiveWay">
    <td>
        <div align="left"><%=this.GetLocalized("繳款方式") %>
            <asp:DropDownList ID="ddlReceiveWay" runat="server">
                <asp:ListItem Value="1">超商</asp:ListItem>
                <asp:ListItem Value="2">ATM</asp:ListItem>
                <asp:ListItem Value="3">臨櫃</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr id="trAccountDateRange">
    <td>
        <div align="left"><%=this.GetLocalized("入帳日區間") %>
            <asp:TextBox ID="tbxAccountDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
            <asp:TextBox ID="tbxAccountDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        (最多 1 個月區間) 
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
</div>

<div id="divResult" runat="server" >
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="ReceiveWayName" LocationHeaderText="代收管道">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="AccountDate" LocationHeaderText="入帳日期">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="代收金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Remark" LocationHeaderText="收款單位自收備註" Visible="false">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="" LocationHeaderText="銷帳狀態">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="StuName" LocationHeaderText="繳款人編號<br/>與姓名">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
		<cc:MyTemplateField LocationHeaderText="功能">
			<ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
				<asp:LinkButton ID="lbtnDetail" runat="server" CommandName="Detail" CssClass="btn">明細</asp:LinkButton>
			</ItemTemplate>
		</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

<div class="button">
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript">
    $(function () {
        $('#divCancelStatus > select').change(function () {
            var typ = $(this).val();
            if (typ == "0") {
                $('#trReceiveWay, #trAccountDateRange').hide();
            } else if (typ == "1") {
                $('#trReceiveWay').show();
                $('#trAccountDateRange').hide();
            } else {
                $('#trReceiveWay, #trAccountDateRange').show();
            }
        });

        $('#divCancelStatus > select').trigger("change");
    });
</script>

</asp:Content>
