﻿<%@ Page Title="土地銀行 - 代收學雜費服務網 - 就貸收費標準" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1300005.aspx.cs" Inherits="eSchoolWeb.D.D1300005" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" ReceiveTypeDefaultMode="First"  UIMode="Label" YearVisible="true" TermVisible="true" AutoGetDataBound="true" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" DepDefaultMode="First" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" ReceiveDefaultMode="First" UIMode="Option" />

<br/>
<table class="information">
<tr align="right">
	<td>
		<cc:MyInsertButton ID="ccbtnInsert" runat="server" OnClick="ccbtnInsert_Click" CssClass="btn" ></cc:MyInsertButton>
	</td>
</tr>
</table>

<br />
<div id="divResult" runat="server">
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
		<Columns>
			<cc:MyBoundField DataField="LoanId" LocationHeaderText="就貸代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="" LocationHeaderText="就貸名稱">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="LoanAmount" LocationHeaderText="就貸代碼金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" Width="80px" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" Width="80px" />
				<ItemTemplate>
					<cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn" UseDefaultJSConfirm="true"></cc:MyDeleteButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>
<br />

<div class="button">
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
