<%@ Page Title="土地銀行 - 代收學雜費服務網 - 自收多筆銷帳" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3200001.aspx.cs" Inherits="eSchoolWeb.C.C3200001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--//表格_修改----------------------------------------------------------------->
<table id="modify" class="modify" summary="表格_修改" width="100%">
<tr>
    <th>虛擬帳號：</th>
    <td><asp:TextBox ID="tbxCancelNo" onkeypress="return EnterEvent(event, 1)" runat="server" MaxLength="16" Width="200px"></asp:TextBox></td>
</tr>
<tr>
    <th>應繳金額：</th>
    <td><asp:TextBox ID="tbxReceiveAmount" onkeypress="return EnterEvent(event, 2)" runat="server" MaxLength="9" Width="100px"></asp:TextBox></td>
</tr>
<tr>
    <th>代收日：</th>
    <td>
        <asp:TextBox ID="tbxReceiveDate" onkeypress="return EnterEvent(event, 3)" runat="server" MaxLength="7" Width="100px"></asp:TextBox>&nbsp;
        <span style="font-size:11px">(請輸入3碼的民國年+2碼月+2碼日。例如 2015/01/02 請輸入 1040102)</span>
    </td>
</tr>
<tr>
    <th>入帳日：</th>
    <td>
        <asp:TextBox ID="tbxAccountDate" runat="server" MaxLength="7" Width="100px"></asp:TextBox>&nbsp;
        <span style="font-size:11px">(請輸入3碼的民國年+2碼月+2碼日。例如 2015/01/02 請輸入 1040102)</span>
    </td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<asp:GridView ID="gvResult" runat="server" CssClass="modify"
	AutoGenerateColumns="false" AllowPaging="false"
	RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
	OnPreRender="gvResult_PreRender">
	<Columns>
		<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="應繳金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveDate" LocationHeaderText="代收日">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="AccountDate" LocationHeaderText="入帳日">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="Result" LocationHeaderText="處理結果">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
	</Columns>
</asp:GridView>

<div class="button">
	<asp:LinkButton ID="lbtnCancelBill" runat="server" OnClick="lbtnCancelBill_Click">銷帳</asp:LinkButton>
</div>

<script type="text/javascript" language="javascript" >
   function EnterEvent(e, order) {
       if (e.keyCode == 13) {
           switch (order) {
               case 1:
                   $('#<%= tbxReceiveAmount.ClientID %>').focus();
                   break;
               case 2:
                   $('#<%= tbxReceiveDate.ClientID %>').focus();
                   break;
               case 3:
                   $('#<%= tbxAccountDate.ClientID %>').focus();
                   break;
           }
        }
   }
</script>

</asp:Content>
