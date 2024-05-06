<%@ Page Title="土地銀行 - 代收學雜費服務網 - 查詢問題檔" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3600001.aspx.cs" Inherits="eSchoolWeb.C.C3600001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    .hidden {
        display:none;
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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Option" YearVisible="false" TermVisible="false" />

<br/>

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th style="width:30%"><%=this.GetLocalized("虛擬帳號") %>：</th>
        <td><asp:TextBox ID="tbxCancelNo" runat="server" MaxLength="16"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("繳款金額") %>：</th>
        <td><asp:TextBox ID="tbxPayAmount" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("管道") %>：</th>
        <td>
            <asp:DropDownList ID="ddlReceiveWay" runat="server">
                <asp:ListItem Value="1">超商</asp:ListItem>
                <asp:ListItem Value="2">ATM</asp:ListItem>
                <asp:ListItem Value="3">臨櫃</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>
            <%=this.GetLocalized("日期區間") %>：
        </th>
        <td>
            <div align="left">
                <asp:DropDownList ID="ddlQueryDateType" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox ID="tbxQuerySDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>～
                <asp:TextBox ID="tbxQueryEDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
            </div>
        </td>
    </tr>
</table>

<div class="button">
    <cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyLinkButton ID="ccbtnExport" runat="server" LocationText="匯出XLS" CommandArgument="XLS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnExportODS" runat="server" LocationText="匯出ODS" CommandArgument="ODS" OnClick="ccbtnExport_Click"></cc:MyLinkButton>
</div>

<asp:GridView ID="gvResult" runat="server" CssClass="modify"
	AutoGenerateColumns="false" AllowPaging="false"
	RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
	OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
	EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
	<Columns>
		<cc:MyBoundField DataField="id" LocationHeaderText="編號">
			<HeaderStyle CssClass="hidden" ></HeaderStyle>
			<ItemStyle CssClass="hidden" ></ItemStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="CancelNo" LocationHeaderText="虛擬帳號">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="PayAmount" LocationHeaderText="繳款金額" HeaderStyle-Wrap="false" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ReceiveWayName" LocationHeaderText="繳款方式">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="ProblemFlag" LocationHeaderText="問題註記">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField LocationHeaderText="代收日期/時間">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField DataField="AccountDate" LocationHeaderText="入帳日期" DataFormatString="{0:yyyy/MM/dd}">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField LocationHeaderText="學號/姓名">
			<HeaderStyle Wrap="False"></HeaderStyle>
		</cc:MyBoundField>
		<cc:MyBoundField LocationHeaderText="備註">
			<HeaderStyle Wrap="False" Width="120px"></HeaderStyle>
		</cc:MyBoundField>
	</Columns>
</asp:GridView>

<div class="button">
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
    <span style="display:none"></span>
</div>

<div id="dialog-form" title="請輸入備註" style="text-align:center;">
    <p>&nbsp;</p>
    <fieldset>
        <textarea id="txtProblemRemark" rows="10" cols="30"></textarea>
    </fieldset>
</div>

<div class="hidden" id="divHidden">
    <input type="hidden" id="hidRowIndex" name="hidRowIndex" value="" runat="server" class="hidRowIndex" />
    <input type="hidden" id="hidProblemRemark" name="hidProblemRemark" value="" runat="server" class="hidProblemRemark" />
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
</div>

<script type="text/javascript">
    var dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 300,
        width: 280,
        modal: true,
        buttons: {
            "確定": submitOK,
            "取消": function () {
                dialog.dialog("close");
            }
        },
        close: function () {
            $('#txtProblemRemark').val('');
        }
    });
    function openDialog(idx, remark) {
        $('.hidRowIndex').val(idx);
        $('#txtProblemRemark').val(remark);
        dialog.dialog("open");
    }
    function submitOK() {
        $('.hidProblemRemark').val($('#txtProblemRemark').val());
        $('#divHidden > a')[0].click();
    }
</script>
</asp:Content>
