<%@ Page Title="土地銀行 - 代收學雜費服務網 - 退費處理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="R4100001.aspx.cs" Inherits="eSchoolWeb.R.R4100001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<uc:Filter1 ID="ucFilter1" runat="server" ReceiveTypeDefaultMode="First" UIMode="Option" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" AutoPostBack="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

<table id="modify" class="modify" summary="表格_修改" width="100%">
    <tr>
        <th colspan="3"><%=this.GetLocalized("新增查詢學生退費資料") %></th>
    </tr>
    <tr>
        <td>
            <%=this.GetLocalized("請選擇") %>&nbsp;&nbsp;
            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoSearchType" runat="server">
                <asp:ListItem Value="StuId">學號</asp:ListItem>
                <asp:ListItem Value="CancelNo">虛擬帳號</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td>
            <%=this.GetLocalized("請輸入") %>&nbsp;&nbsp;<asp:TextBox ID="tbxSearchString" MaxLength="16" runat="server"></asp:TextBox>
        </td>
        <td style="width:90px">
            <asp:LinkButton ID="lbtnQuery" runat="server" CssClass="btn" OnClick="lbtnQuery_Click">查詢</asp:LinkButton>
            <asp:LinkButton ID="lbtnInsert" runat="server" CssClass="btn" OnClick="lbtnInsert_Click">新增</asp:LinkButton>
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
			<cc:MyBoundField DataField="StuName" LocationHeaderText="姓名">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReturnDate" LocationHeaderText="退費日期">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReSeq" LocationHeaderText="退費次序">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" Width="55px" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyDeleteButton UseDefaultJSConfirm="false" ID="ccbtnDelete" runat="server" CssClass="btn"></cc:MyDeleteButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

</asp:Content>
