<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護學生繳費資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2100002.aspx.cs" Inherits="eSchoolWeb.B.B2100002" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    .max-w180px {
        max-width: 180px;
    }
</style>
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
<uc:Filter2 ID="ucFilter2" runat="server" ReceiveDefaultKind="All" ReceiveDefaultMode="ByKind" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<br/>

<!--//表格_修改----------------------------------------------------------------->
<table id="tabQuery1" runat="server" class="result" summary="查詢結果" width="100%">
    <tr>
        <td>
            <div align="left"><%=this.GetLocalized("科系") %>
                <asp:DropDownList ID="ddlMajor" runat="server" CssClass="max-w180px">
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
        <td>
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
                    <asp:ListItem Value="1">超商</asp:ListItem>
                    <asp:ListItem Value="2">ATM</asp:ListItem>
                    <asp:ListItem Value="3">臨櫃</asp:ListItem>
                </asp:DropDownList>
            </div>
        </td>
    </tr>
    <tr class="trNoQueryDate">
        <td colspan="2">
            <div align="left">
                <asp:RadioButton ID="rbtNoQueryDate" runat="server" GroupName="rbtDateType" Checked="true" /><%=this.GetLocalized("不指定日期區間") %>
            </div>
        </td>
    </tr>
    <tr class="trReceiveDateRange">
        <td colspan="2">
            <div align="left">
                <asp:RadioButton ID="rbtReceiveDate" runat="server" GroupName="rbtDateType" /><%=this.GetLocalized("代收日區間") %>
                <asp:TextBox ID="tbxReceiveDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
                <asp:TextBox ID="tbxReceiveDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
            </div>
        </td>
    </tr>
    <tr class="trAccountDateRange">
        <td colspan="2">
            <div align="left">
                <asp:RadioButton ID="rbtAccountDate" runat="server" GroupName="rbtDateType" /><%=this.GetLocalized("入帳日區間") %>
                <asp:TextBox ID="tbxAccountDateS" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
                <asp:TextBox ID="tbxAccountDateE" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
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

<table id="tabQuery2" runat="server" visible="false" class="result" summary="查詢條件" width="100%">
<tr>
    <td>
        <div align="left"><%=this.GetLocalized("學號") %>
            <asp:TextBox ID="tbxStuId" MaxLength="16" runat="server"></asp:TextBox>
        </div>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<table id="tabInsert" runat="server" visible="false" class="information" >
<tr>
	<td><%=this.GetLocalized("學生繳費資料所屬費用別") %>：
		<asp:DropDownList ID="ddlReceiveId" runat="server"></asp:DropDownList> &nbsp;&nbsp;&nbsp;&nbsp;
	</td>
	<td align="right">
		<cc:MyInsertButton ID="ccbtnInsert" runat="server" CssClass="btn" OnClick="ccbtnInsert_Click"></cc:MyInsertButton>
	</td>
</tr>
</table>

<div id="divResult" runat="server" >
	<table id="tabSummary" runat="server" class="result" summary="查詢結果" width="100%">
	<tr>
		<td><%=this.GetLocalized("總筆數") %>：<asp:Label ID="labDataCount" runat="server" Text=""></asp:Label></td>
		<td><%=this.GetLocalized("總金額") %>：<asp:Label ID="labSumAmount" runat="server" Text=""></asp:Label></td>
	</tr>
	</table>

	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="ReceiveName" LocationHeaderText="費用別">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="StuId" LocationHeaderText="學號">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="Name" LocationHeaderText="姓名">
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
			<cc:MyBoundField DataField="" LocationHeaderText="銷帳狀態">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn"></cc:MyDeleteButton>
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
                $('.trReceiveWay, .trNoQueryDate, .trReceiveDateRange, .trAccountDateRange').hide();
            } else if (typ == "1") {
                $('.trReceiveWay, .trNoQueryDate, .trReceiveDateRange').show();
                $('.trAccountDateRange').hide();
                if ($('.trAccountDateRange input:radio').is(":checked")) {
                    $('.trNoQueryDate input:radio').prop("checked", true);
                }
            } else {
                $('.trReceiveWay, .trNoQueryDate, .trReceiveDateRange, .trAccountDateRange').show();
            }
        });

        $('#divCancelStatus > select').trigger("change");
    });
</script>

</asp:Content>
