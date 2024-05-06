<%@ Page Title="土地銀行 - 代收學雜費服務網 - 整批刪除學生資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2100004.aspx.cs" Inherits="eSchoolWeb.B.B2100004" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<br/>
<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
    <td width="30%">
        <div align="right"><%=this.GetLocalized("虛擬帳號") %>:</div>
    </td>
    <td><asp:TextBox ID="tbxCancelNo" MaxLength="16" runat="server"></asp:TextBox></td>
</tr>
<tr>
    <td>
        <div align="right"><%=this.GetLocalized("學號") %>:</div>
    </td>
    <td><asp:TextBox ID="tbxStuId" MaxLength="20" runat="server"></asp:TextBox></td>
</tr>
<tr>
    <td>
        <div align="right"><%=this.GetLocalized("批號") %>:</div>
    </td>
    <td><asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList></td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyLinkButton ID="lbtnBacthDelete" runat="server" LocationText="整批刪除" OnClick="lbtnBatcthDelete_Click" OnClientClick="return confirm('確定整批刪除學生繳費資料?');"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<div style="text-align:left">
    [操作說明]：<br />
    1.查詢結果僅含未繳的資料 (包含已有虛擬帳號的資料)。<br />
    2.查詢結果有資料，才會顯示【刪除】按鈕。<br />
    3.點選【刪除】按鈕僅刪除查詢結果中當前頁面上勾選的資料。<br />
    4.點選【整批刪除】按鈕會直接依據指定條件，刪除未繳資料，無須先查詢。<br />
    5.資料刪除後無法還原，請小心使用。<br />
</div>

<div id="divResult" runat="server" >
	<table class="result" summary="查詢結果" width="100%">
	<tr>
		<td><%=this.GetLocalized("總筆數") %>：<asp:Label ID="labDataCount" runat="server" Text=""></asp:Label></td>
	</tr>
	</table>

	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server" NoPageSizeVisible="false"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<asp:TemplateField ItemStyle-Width="40px">
				<HeaderTemplate>
					<asp:CheckBox ID="chkAllEdit" Width="100px" runat="server" Text="<%$ Resources:Localized, 編緝(全選) %>" CssClass="cbxAllEdit" />
				</HeaderTemplate>
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:CheckBox ID="chkSelected" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Name" LocationHeaderText="姓名">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="應繳金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="" LocationHeaderText="銷帳狀態">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server" NoPageSizeVisible="false"></uc:Paging>
	</div>
</div>

<div class="button">
    <cc:MyDeleteButton ID="ccbtnDelete" runat="server" OnClick="ccbtnDelete_Click"></cc:MyDeleteButton>
</div>

<script type="text/javascript" language="javascript">
    function checkAllEdit(cbx) {
        var grid = document.getElementById("<%= gvResult.ClientID %>");
        var cell;
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[0];
                var chk = cell.getElementsByTagName("input");
                if (!chk[0].disabled) {
                    chk[0].checked = cbx.checked;
                }
            }
        }
    }

    $(function () {
        $('.cbxAllEdit > input:checkbox').click(function () {
            checkAllEdit($(this)[0]);
        });
    });
</script>
</asp:Content>
