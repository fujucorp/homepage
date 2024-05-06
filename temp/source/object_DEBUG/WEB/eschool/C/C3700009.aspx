<%@ Page Title="土地銀行 - 代收學雜費服務網 - 查詢歷史(繳費)資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3700009.aspx.cs" Inherits="eSchoolWeb.C.C3700009" %>

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

<table class="result" summary="查詢條件" width="100%">
<tr>
    <td width="50%">
        <div align="left"><%=this.GetLocalized("部別") %>
            <asp:DropDownList ID="ddlDept" runat="server">
            </asp:DropDownList>
        </div>
    </td>
    <td width="50%">
        <div align="left"><%=this.GetLocalized("院別") %>
            <asp:DropDownList ID="ddlCollege" runat="server">
            </asp:DropDownList>
        </div>
    </td>
</tr>
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
    <td width="50%">
        <div align="left"><%=this.GetLocalized("班別") %>
            <asp:DropDownList ID="ddlClass" runat="server">
            </asp:DropDownList>
        </div>
    </td>
    <td>
        <div align="left"><%=this.GetLocalized("批號") %>
            <asp:DropDownList ID="ddlUpNo" runat="server">
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left" id="divCancelStatus"><%=this.GetLocalized("銷帳狀態") %>
            <asp:DropDownList ID="ddlCancelStatus" runat="server">
                <asp:ListItem Value="0">未繳款</asp:ListItem>
                <asp:ListItem Value="1">已繳款未入帳</asp:ListItem>
                <asp:ListItem Value="2">已入帳</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr class="trReceiveWay">
    <td colspan="2">
        <div align="left"><%=this.GetLocalized("繳款方式") %>
            <asp:DropDownList ID="ddlReceiveWay" runat="server">
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr id="trQueryDateRange">
    <td colspan="2">
        <div align="left">
            <span id="spnNoQueryDateRang"><asp:RadioButton ID="rbtNoQueryDate" runat="server" GroupName="rbtDateType" Checked="true" /><%=this.GetLocalized("不指定日期區間") %></span>
            <span id="spnReceiveDateRange"><asp:RadioButton ID="rbtReceiveDate" runat="server" GroupName="rbtDateType" /><%=this.GetLocalized("代收日區間") %></span>
            <span id="spnAccountDateRange"><asp:RadioButton ID="rbtAccountDate" runat="server" GroupName="rbtDateType" /><%=this.GetLocalized("入帳日區間") %></span>
            <asp:TextBox ID="tbxQuerySDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
            <asp:TextBox ID="tbxQueryEDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
        </div>
    </td>
</tr>
<tr>
    <td colspan="2">
        <div align="left">
            <asp:RadioButtonList ID="rdoSearchField" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                <asp:ListItem Value="StuId" Selected="True" >學號</asp:ListItem>
                <asp:ListItem Value="CancelNo">虛擬帳號</asp:ListItem>
                <asp:ListItem Value="StuName">姓名</asp:ListItem>
            </asp:RadioButtonList>
            <asp:TextBox ID="tbxSearchValue" MaxLength="16" runat="server"></asp:TextBox>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyLinkButton ID="ccbtnExport" runat="server" LocationText="匯出XLS" CommandArgument="XLS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnExportODS" runat="server" LocationText="匯出ODS" CommandArgument="ODS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
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
			<cc:MyBoundField DataField="ReceiveAmount" LocationHeaderText="應繳金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CancelStatusText" LocationHeaderText="銷帳狀態">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="明細">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:LinkButton ID="lbtnDetail" runat="server" CommandName="Detail" CssClass="btn">明細</asp:LinkButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

<script type="text/javascript">
    $(function () {
        $('#divCancelStatus > select').change(function () {
            var typ = $(this).val();
            if (typ == "0") {
                $('#trReceiveWay, #trQueryDateRange').hide();
            } else if (typ == "1") {
                $('#trReceiveWay, #trQueryDateRange, #spnNoQueryDateRang, #spnReceiveDateRange').show();
                $('#spnAccountDateRange').hide();
                if ($('#spnAccountDateRange input:radio').is(":checked")) {
                    $('#spnNoQueryDateRang input:radio').prop("checked", true);
                }
            } else {
                $('#trReceiveWay, #trQueryDateRange, #spnNoQueryDateRang, #spnReceiveDateRange, #spnAccountDateRange').show();
            }
        });

        $('#divCancelStatus > select').trigger("change");
    });
</script>
</asp:Content>
