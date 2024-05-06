<%@ Page Title="土地銀行 - 代收學雜費服務網 - 取消銷帳註記" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3700002.aspx.cs" Inherits="eSchoolWeb.C.C3700002" %>

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

<br/>

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <td>
            <div align="left" id="divCancelStatus"><%=this.GetLocalized("銷帳狀態") %>
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
    <tr id="trNoQueryDate">
        <td colspan="2">
            <div align="left">
                <asp:RadioButton ID="rbtNoQueryDate" runat="server" GroupName="rbtDateType" Checked="true" /><%=this.GetLocalized("不指定日期區間") %>
            </div>
        </td>
    </tr>
    <tr id="trReceiveDateRange">
        <td>
            <div align="left">
                <asp:RadioButton ID="rbtReceiveDate" runat="server" GroupName="rbtDateType" /><%=this.GetLocalized("代收日區間") %>
                <asp:TextBox ID="tbxReceiveDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
                <asp:TextBox ID="tbxReceiveDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
            </div>
        </td>
    </tr>
    <tr id="trAccountDateRange">
        <td>
            <div align="left">
                <asp:RadioButton ID="rbtAccountDate" runat="server" GroupName="rbtDateType" /><%=this.GetLocalized("入帳日區間") %>
                <asp:TextBox ID="tbxAccountDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
                <asp:TextBox ID="tbxAccountDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div align="left">
                <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoSearchField" runat="server">
                    <asp:ListItem Value="StuId">學號</asp:ListItem>
                    <asp:ListItem Value="CancelNo">虛擬帳號</asp:ListItem>
                    <asp:ListItem Value="IdNumber">身分證字號</asp:ListItem>
                </asp:RadioButtonList>
                <asp:TextBox ID="tbxSearchValue" MaxLength="16" runat="server"></asp:TextBox>
            </div>
        </td>
    </tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
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
			<cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Name" LocationHeaderText="姓名">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="應繳金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="" LocationHeaderText="銷帳狀態">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="調整">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyLinkButton ID="ccbtnModify" runat="server" LocationText="調整" CommandName="ModifyData" CssClass="btn"></cc:MyLinkButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

<script type="text/javascript">
    $(function () {
        $('#divCancelStatus > select').change(function () {
            var typ = $(this).val();
            if (typ == "0") {
                $('#trReceiveWay, #trNoQueryDate, #trReceiveDateRange, #trAccountDateRange').hide();
            } else if (typ == "1") {
                $('#trReceiveWay, #trNoQueryDate, #trReceiveDateRange').show();
                $('#trAccountDateRange').hide();
                if ($('#trAccountDateRange input:radio').is(":checked")) {
                    $('#trNoQueryDate input:radio').prop("checked", true);
                }
            } else {
                $('#trReceiveWay, #trNoQueryDate, #trReceiveDateRange, #trAccountDateRange').show();
            }
        });

        $('#divCancelStatus > select').trigger("change");
    });
</script>

</asp:Content>
