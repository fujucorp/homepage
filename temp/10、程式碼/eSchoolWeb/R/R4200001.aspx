<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生退費清冊" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="R4200001.aspx.cs" Inherits="eSchoolWeb.R.R4200001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" ReceiveDefaultMode="First" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<table id="modify" class="modify" summary="表格_修改" width="100%">
	<tr>
	    <td>
		    <%=this.GetLocalized("欲產生之退費清單批號") %>：<asp:Label ID="labSRNo" runat="server" Text="1"></asp:Label>
	    </td>
	    <td>
		    <%=this.GetLocalized("檔名") %>：<asp:TextBox ID="tbxFileName" runat="server"></asp:TextBox>
            <span style="color:red">*<%=this.GetLocalized("不須填寫副檔名") %>。</span>
	    </td>
	</tr>
</table>
        
<asp:GridView ID="gvResult" runat="server" CssClass="modify"
	AutoGenerateColumns="false" AllowPaging="false"
	RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
	OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
	<Columns>
		<cc:MyTemplateField LocationHeaderText="選擇">
			<ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
                <asp:CheckBox ID="chkSelected" runat="server" />
			</ItemTemplate>
		</cc:MyTemplateField>
		<cc:MyBoundField DataField="StuName" LocationHeaderText="姓名">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReturnAmount"  LocationHeaderText="退費金額" DataFormatString="{0:N0}" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
	</Columns>
</asp:GridView>

<div class="button">
    <cc:MyLinkButton ID="ccbtnCheckAll" runat="server" LocationText="全選" OnClick="ccbtnCheckAll_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnGenExcel" runat="server" LocationText="產生清冊XLS" CommandArgument="XLS" OnClick="ccbtnGenExcel_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnGenCalc" runat="server" LocationText="產生清冊ODS" CommandArgument="ODS" OnClick="ccbtnGenExcel_Click"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
    
</asp:Content>
