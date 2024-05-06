<%@ Page Title="土地銀行 - 代收學雜費服務網 - 待辦事項" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5200004.aspx.cs" Inherits="eSchoolWeb.S.S5200004" %>

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
    <td>
        <div align="left"><%=this.GetLocalized("待辦事項") %>
            <asp:DropDownList ID="ddlFormId" runat="server">
            </asp:DropDownList>
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
			<cc:MyBoundField DataField="FormName" LocationHeaderText="待辦事項">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ApplyKindName" LocationHeaderText="申請種類">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ApplyDate" LocationHeaderText="申請日期" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd <br/> HH:mm:ss}">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ApplyUserName" LocationHeaderText="申請人">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="FormDesc" LocationHeaderText="描述">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="放行">
				<HeaderStyle Wrap="False" Width="50px" ></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" Wrap="False" />
				<ItemTemplate>
					<cc:MyLinkButton ID="lbtnApprove" runat="server" CommandName="Approve" CssClass="btn">放行</cc:MyLinkButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="駁回">
				<HeaderStyle Wrap="False" Width="50px" ></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" Wrap="False" />
				<ItemTemplate>
					<cc:MyLinkButton ID="lbtnReject" runat="server" CommandName="Reject" CssClass="btn">駁回</cc:MyLinkButton>
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
