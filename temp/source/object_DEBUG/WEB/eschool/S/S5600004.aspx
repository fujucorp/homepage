<%@ Page Title="土地銀行 - 代收學雜費服務網 - 代收管道管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600004.aspx.cs" Inherits="eSchoolWeb.S.S5600004" %>
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
    
<table class="information">
<tr align="right">
	<td>
		<cc:MyInsertButton ID="ccbtnInsert" runat="server" CssClass="btn" OnClick="ccbtnInsert_Click"></cc:MyInsertButton>&nbsp;
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
			<cc:MyBoundField DataField="ChannelName" LocationHeaderText="管道名稱">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ChannelId" LocationHeaderText="管道代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="繳款方式">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
                    <asp:LinkButton ID="lbtnChannelWay" runat="server" CommandName="ChannelWay" CssClass="btn">查看繳款方式</asp:LinkButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyDeleteButton ID="ccbtnDelete" UseDefaultJSConfirm="false" runat="server" CssClass="btn"></cc:MyDeleteButton>
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
</asp:Content>
