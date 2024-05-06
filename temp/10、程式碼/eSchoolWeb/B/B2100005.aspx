<%@ Page Title="土地銀行 - 代收學雜費服務網 - 產生委扣媒體" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2100005.aspx.cs" Inherits="eSchoolWeb.B.B2100005" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" />

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%"style="display:none;">
<tr>
    <td>
        <div align="left"><%=this.GetLocalized("委扣銀行") %>
            <asp:DropDownList ID="ddlBankKind" runat="server">
                <asp:ListItem Value="1">自行</asp:ListItem>
                <asp:ListItem Value="2">他行</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

    <!--
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server" Visible="false"></uc:Paging>
	</div>
        -->
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<asp:TemplateField ItemStyle-Width="40px">
				<HeaderTemplate>
					<asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />全選
				</HeaderTemplate>
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:CheckBox ID="chkSelected" runat="server" />
					<asp:HiddenField ID="hidKey" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<cc:MyBoundField DataField="StuId" LocationHeaderText="學號" HeaderStyle-Wrap="false">
			</cc:MyBoundField>
			<cc:MyBoundField DataField="StuName" LocationHeaderText="姓名" HeaderStyle-Wrap="false">
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號" HeaderStyle-Wrap="false">
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="應繳金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
			</cc:MyBoundField>
			<cc:MyBoundField DataField="DeductBankId" LocationHeaderText="委扣銀行代碼" HeaderStyle-Wrap="false">
			</cc:MyBoundField>
			<cc:MyBoundField DataField="DeductAccountNo" LocationHeaderText="委扣銀行帳號" HeaderStyle-Wrap="false">
			</cc:MyBoundField>
		</Columns>
	</asp:GridView>
    <!--
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server" Visible="false"></uc:Paging>
	</div>
    -->

    <div id="divResult2" runat="server" >
        <br />
        <div class="button">
            <cc:MyLinkButton ID="ccbtnGenTTB" runat="server" LocationText="產生自行委扣媒體" OnClick="ccbtnExportFile_Click" CommandName="ExportBANK"></cc:MyLinkButton>&nbsp;&nbsp;
            <cc:MyLinkButton ID="ccbtnGenCNB" runat="server" LocationText="產生全國繳委扣媒體" OnClick="ccbtnExportFile_Click" CommandName="ExportCNB" Visible="false"></cc:MyLinkButton>&nbsp;&nbsp;
            <cc:MyLinkButton ID="ccbtnGenACH" runat="server" LocationText="產生ACH委扣媒體" OnClick="ccbtnExportFile_Click" CommandName="ExportACH" Visible="false"></cc:MyLinkButton>
        </div>
    </div>

<script type="text/javascript" language="javascript">
    function checkAll(obj) {
        var checked = obj.checked;
        $('input:checkbox:enabled').each(function (idx) {
            var cbx = $(this)[0];
            if (cbx.id != obj.id) {
                cbx.checked = checked;
            }
        });
    }
</script>
</asp:Content>
