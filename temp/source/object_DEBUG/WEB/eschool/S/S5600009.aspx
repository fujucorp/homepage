<%@ Page Title="土地銀行 - 代收學雜費服務網 - 排程作業管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600009.aspx.cs" Inherits="eSchoolWeb.S.S5600009" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    .max-w250px {
        max-width: 250px;
    }
    span.scroll {
        display: block;
        width:100%;
        max-width: 230px;
        width:230px\9;  /* IE 8 */
        /*height: 60px;*/
        /*min-height: 100%;*/
        /*max-height: 150px;*/
        overflow: auto;
        white-space: nowrap;
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
<table class="information">
<tr align="right">
	<td>
		<cc:MyInsertButton ID="ccbtnInsert" runat="server" CssClass="btn" OnClick="ccbtnInsert_Click"></cc:MyInsertButton>
	</td>
</tr>
</table>

<div id="divResult" runat="server">
    <%--[MDY:2018xxxx] 改結繫 ServiceConfig2 集合物件--%>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
		<Columns>
			<cc:MyBoundField LocationHeaderText="服務名稱" ItemStyle-CssClass="max-w250px">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="週期設定">
				<ItemTemplate>
					<span class="scroll"  style="height:60px;"><asp:Literal ID="litCycle" runat="server"></asp:Literal></span>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="應用程式設定">
				<ItemTemplate>
					<span class="scroll"><asp:Literal ID="litApp" runat="server"></asp:Literal></span>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyBoundField LocationHeaderText="狀態">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="操作">
				<ItemStyle HorizontalAlign="Center" Wrap="false" />
				<ItemTemplate>
					<div style="padding:4px 0px;"><cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton></div>
					<div style="padding:4px 0px;""><cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn" UseDefaultJSConfirm="true"></cc:MyDeleteButton></div>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
</div>

<div class="button">
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
