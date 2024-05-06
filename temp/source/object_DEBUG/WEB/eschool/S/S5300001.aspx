<%@ Page Title="土地銀行 - 代收學雜費服務網 - 使用者管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5300001.aspx.cs" Inherits="eSchoolWeb.S.S5300001" %>

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
<table class="information" id="condition" width="100%">
<tr align="left">
	<td>
		<%= GetLocalized("群組")%>：<asp:DropDownList ID="ddlGroup" runat="server"></asp:DropDownList>
	</td>
</tr>
<tr align="left" id="trBank" runat="server">
	<td>
		<%= GetLocalized("分行")%>：<asp:DropDownList ID="ddlBank" runat="server"></asp:DropDownList> &nbsp;&nbsp;(限查行員帳號)
	</td>
</tr>
<tr align="left" id="trSchool" runat="server">
	<td>
		<%= GetLocalized("學校")%>：<asp:DropDownList ID="ddlSchool" runat="server"></asp:DropDownList> &nbsp;&nbsp;(限查學校帳號)
	</td>
</tr>
<tr align="left" id="trReceiveType" runat="server">
	<td>
		<%= GetLocalized("商家代號")%>：<asp:DropDownList ID="ddlReceiveType" runat="server"></asp:DropDownList>
	</td>
</tr>
<tr align="left">
	<td>
		<asp:RadioButtonList ID="rbtnlQDataType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList> &nbsp;
		<asp:TextBox ID="tbxQDataValue" runat="server" MaxLength="10"></asp:TextBox>
	</td>
</tr>
<tr>
	<td class="button">
		<cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click" ></cc:MyQueryButton>
	</td>
</tr>
</table>

<br/>
<table class="information">
<tr align="right">
	<td>
	<cc:MyInsertButton ID="ccbtnInsert" runat="server" OnClick="ccbtnInsert_Click" CssClass="btn"></cc:MyInsertButton>
	</td>
</tr>
</table>

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
			<cc:MyBoundField DataField="URt" LocationHeaderText="商家代號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="UBank" LocationHeaderText="單位代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="UId" LocationHeaderText="使用者帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="UName" LocationHeaderText="使用者姓名">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="FlowStatusText" LocationHeaderText="審核狀態">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" Wrap="false" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" Wrap="false" />
				<ItemTemplate>
					<cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn"></cc:MyDeleteButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

</asp:Content>
