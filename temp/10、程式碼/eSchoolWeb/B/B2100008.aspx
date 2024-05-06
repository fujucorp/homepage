<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護就貸資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2100008.aspx.cs" Inherits="eSchoolWeb.B.B2100008" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<br/>

<table class="result" summary="" width="100%">
<tr>
    <td>
        <div align="left"><%=this.GetLocalized("科系") %>
            <asp:DropDownList ID="ddlMajor" runat="server">
            </asp:DropDownList>
        </div>
    </td>
    <td>
        <div align="left"><%=this.GetLocalized("年級") %>
            <asp:DropDownList ID="ddlGrade" runat="server">
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr>
    <td>
        <div align="left"><%=this.GetLocalized("批號") %>
            <asp:DropDownList ID="ddlUpNo" runat="server">
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left">
            <asp:RadioButtonList ID="rdoSearchField" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                <asp:ListItem Value="StuId">學號</asp:ListItem>
                <asp:ListItem Value="CancelNo">虛擬帳號</asp:ListItem>
                <asp:ListItem Value="IdNumber">身分證字號</asp:ListItem>
                <asp:ListItem Value="StuName">姓名</asp:ListItem>
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
			<cc:MyBoundField DataField="StuName" LocationHeaderText="姓名">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="OldSeq" LocationHeaderText="序號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="繳費金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="LoanId" LocationHeaderText="就貸代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="loan" LocationHeaderText="實貸金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="新增">
				<ItemStyle HorizontalAlign="Center" Width="50" />
				<ItemTemplate>
					<cc:MyInsertButton ID="ccbtnInsert" runat="server" CssClass="btn" Visible="false"></cc:MyInsertButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" Width="50" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn" Visible="false"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" Width="50" />
				<ItemTemplate>
					<cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn" Visible="false"></cc:MyDeleteButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

</asp:Content>
