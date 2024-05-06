<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mobile.Master" AutoEventWireup="true" CodeBehind="student001m.aspx.cs" Inherits="eSchoolWeb.student.student001m" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="information" class="information" summary="表格_修改" width="100%">
	<tr>
		<td><%=this.GetLocalized("學校") %>：<asp:Label ID="labSchoolName" runat="server" Text=""></asp:Label></td>
	</tr>
	<tr>
		<td><%=this.GetLocalized("學號") %>：<asp:Label ID="labStudentID" runat="server" Text=""></asp:Label></td>
	</tr>
	<tr>
		<td><%=this.GetLocalized("姓名") %>：<asp:Label ID="labStudentName" runat="server" Text=""></asp:Label></td>
	</tr>
</table>
<br />

<asp:GridView ID="gvResult" runat="server" CssClass="modify"
	AutoGenerateColumns="false" AllowPaging="false"
	RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
	OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender">
	<Columns>
		<cc:MyBoundField DataField="ReceiveType" LocationHeaderText="商家代號" />
		<cc:MyBoundField DataField="YearName" LocationHeaderText="學年" />
		<cc:MyBoundField DataField="TermName" LocationHeaderText="學期" />
		<cc:MyBoundField DataField="ReceiveName" LocationHeaderText="費用別" />
		<cc:MyBoundField DataField="DeptName" LocationHeaderText="部別" />
		<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號" />
		<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="應繳金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" />
		<cc:MyBoundField DataField="ReceiveWayName" LocationHeaderText="繳費管道" Visible="false" />
		<cc:MyBoundField DataField="LoanName" LocationHeaderText="助貸" />
		<cc:MyBoundField DataField="" LocationHeaderText="狀態" />
		<cc:MyTemplateField LocationHeaderText="明細">
			<ItemStyle HorizontalAlign="Center" Width="50px" />
			<ItemTemplate>
				<asp:LinkButton ID="lbtnDetail" runat="server" CommandName="Detail" CssClass="btn">明細</asp:LinkButton>
				<asp:Label ID="labMsg" runat="server" Text="<%$ Resources:Localized, 未開放 %>" Visible="false"></asp:Label>
			</ItemTemplate>
		</cc:MyTemplateField>
		<cc:MyTemplateField LocationHeaderText="列印繳費單">
			<ItemStyle HorizontalAlign="Center" Width="90px" />
			<ItemTemplate>
				<asp:LinkButton ID="lbtnGenBill" runat="server" CommandName="GenBill" CssClass="btn">列印繳費單</asp:LinkButton>
			</ItemTemplate>
		</cc:MyTemplateField>
		<cc:MyTemplateField LocationHeaderText="列印收據">
			<ItemStyle HorizontalAlign="Center" Width="90px" />
			<ItemTemplate>
				<asp:LinkButton ID="lbtnGenReceipt" runat="server" CommandName="GenReceipt" CssClass="btn">列印收據</asp:LinkButton>
			</ItemTemplate>
		</cc:MyTemplateField>
	</Columns>
</asp:GridView>
</asp:Content>
