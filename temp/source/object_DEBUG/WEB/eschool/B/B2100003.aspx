<%@ Page Title="土地銀行 - 代收學雜費服務網 - 下載學生繳費資料媒體檔" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2100003.aspx.cs" Inherits="eSchoolWeb.B.B2100003" %>

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
<table class="result" summary="查詢條件" width="100%">
<tr>
	<td colspan="2"><div align="left"><%= GetLocalized("設定查詢條件") %></div></td>
</tr>
<tr>
    <td width="50%">
        <div align="left"><%=this.GetLocalized("批號") %>
            <asp:DropDownList ID="ddlUpNo" runat="server"></asp:DropDownList>
        </div>
    </td>
    <td width="50%">
        <div align="left" id="divCancelStatus"><%=this.GetLocalized("銷帳狀態") %>
            <asp:DropDownList ID="ddlCancelStatus" runat="server">
                <asp:ListItem Value="0">未繳款</asp:ListItem>
                <asp:ListItem Value="1">已繳款未入帳</asp:ListItem>
                <asp:ListItem Value="2">已入帳</asp:ListItem>
            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr id="trReceiveWay">
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
            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rdoSearchField" runat="server">
                <asp:ListItem Value="StuId">學號</asp:ListItem>
                <asp:ListItem Value="CancelNo">虛擬帳號</asp:ListItem>
                <asp:ListItem Value="IdNumber">身分證字號</asp:ListItem>
            </asp:RadioButtonList>
            <asp:TextBox ID="tbxSearchValue" MaxLength="16" runat="server"></asp:TextBox>
        </div>
    </td>
</tr>
</table>

<table class="condition" summary="下載資料項目" width="100%">
<tr>
	<th colspan="4"><div align="left"><%= GetLocalized("勾選下載資料項目") %></div></th>
</tr>
<tr>
	<td colspan="4">
		<%= GetLocalized("載入資料項目設定") %>：
		<asp:DropDownList ID="ddlLoadExportConfig" runat="server">
		</asp:DropDownList>&nbsp;&nbsp;
		<asp:LinkButton ID="lbtnLoadExportConfig" runat="server" CssClass="btn" OnClick="lbtnLoadExportConfig_Click"><%= GetLocalized("載入") %></asp:LinkButton>
	</td>
</tr>
<tr>
	<td colspan="4" style="border-bottom:0px hidden #e6e6e6">
		<table width="100%" class="#" id="tabCheckboxs" runat="server">
		<tr>
			<td colspan="12"><input type="checkbox" id="cbxCheckAll" onchange="checkAll(this);" /><%= GetLocalized("全選") %></td>
		</tr>
		<!-- 學年學期 -->
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkYearId" runat="server" Text="<%$ Resources:Localized, 學年代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkYearName" runat="server" Text="<%$ Resources:Localized, 學年名稱%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkTermId" runat="server" Text="<%$ Resources:Localized, 學期代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkTermName" runat="server" Text="<%$ Resources:Localized, 學期名稱%>" /></td>
		</tr>
		<!-- 學生基本資料 -->
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkStu_Id" runat="server" Text="<%$ Resources:Localized, 學號%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkStu_Name" runat="server" Text="<%$ Resources:Localized, 姓名%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkId_Number" runat="server" Text="<%$ Resources:Localized, 身分證字號%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkStu_Birthday" runat="server" Text="<%$ Resources:Localized, 生日%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkStu_Tel" runat="server" Text="<%$ Resources:Localized, 電話%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkStu_Addcode" runat="server" Text="<%$ Resources:Localized, 郵遞區號%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkStu_Add" runat="server" Text="<%$ Resources:Localized, 住址%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkEmail" runat="server" Text="<%$ Resources:Localized, 電子郵件%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkStuParent" runat="server" Text="<%$ Resources:Localized, 家長名稱%>" /></td>
			<td colspan="9">&nbsp;</td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkOld_Seq" runat="server" Text="<%$ Resources:Localized, 序號%>" /></td>
			<td colspan="9"><asp:CheckBox ID="chkPay_Due_Date" runat="server" Text="<%$ Resources:Localized, 繳款期限%>" /> <span style="color:red; font-size:11pt;">(<%= GetLocalized("學生繳費資料之繳款期限若為空白，則顯示代收費用檔中的繳款期限") %>)</span></td>
		</tr>
		<!-- 學籍資料 -->
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkStu_Grade" runat="server" Text="<%$ Resources:Localized, 年級%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkStu_Hid" runat="server" Text="<%$ Resources:Localized, 座號%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkClass_Id" runat="server" Text="<%$ Resources:Localized, 班別代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkClass_Name" runat="server" Text="<%$ Resources:Localized, 班別名稱%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkDept_Id" runat="server" Text="<%$ Resources:Localized, 部別代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkDept_Name" runat="server" Text="<%$ Resources:Localized, 部別名稱%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkCollege_Id" runat="server" Text="<%$ Resources:Localized, 院別代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkCollege_Name" runat="server" Text="<%$ Resources:Localized, 院別名稱%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkMajor_Id" runat="server" Text="<%$ Resources:Localized, 科系代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMajor_Name" runat="server" Text="<%$ Resources:Localized, 科系名稱%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkReduce_Id" runat="server" Text="<%$ Resources:Localized, 減免代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkReduce_Name" runat="server" Text="<%$ Resources:Localized, 減免名稱%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkLoan_Id" runat="server" Text="<%$ Resources:Localized, 就貸代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkLoan_Name" runat="server" Text="<%$ Resources:Localized, 就貸名稱%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkDorm_Id" runat="server" Text="<%$ Resources:Localized, 住宿代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkDorm_Name" runat="server" Text="<%$ Resources:Localized, 住宿名稱%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Id1" runat="server" Text="<%$ Resources:Localized, 身分註記01代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Name1" runat="server" Text="<%$ Resources:Localized, 身分註記01名稱%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Id2" runat="server" Text="<%$ Resources:Localized, 身分註記02代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Name2" runat="server" Text="<%$ Resources:Localized, 身分註記02名稱%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Id3" runat="server" Text="<%$ Resources:Localized, 身分註記03代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Name3" runat="server" Text="<%$ Resources:Localized, 身分註記03名稱%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Id4" runat="server" Text="<%$ Resources:Localized, 身分註記04代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Name4" runat="server" Text="<%$ Resources:Localized, 身分註記04名稱%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Id5" runat="server" Text="<%$ Resources:Localized, 身分註記05代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Name5" runat="server" Text="<%$ Resources:Localized, 身分註記05名稱%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Id6" runat="server" Text="<%$ Resources:Localized, 身分註記06代碼%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkIdentify_Name6" runat="server" Text="<%$ Resources:Localized, 身分註記06名稱%>" /></td>
		</tr>
		<!-- 繳費資料 -->
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkStu_Hour" runat="server" Text="<%$ Resources:Localized, 上課時數%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkStu_Credit" runat="server" Text="<%$ Resources:Localized, 總學分數%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkLoan_Amount" runat="server" Text="<%$ Resources:Localized, 就學貸款可貸金額%>" /></td>
			<td colspan="3">&nbsp;</td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkReceive" runat="server" Text="<%$ Resources:Localized, 收入科目金額%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkReceiveSum" runat="server" Text="<%$ Resources:Localized, 小計金額%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkZBarcode" runat="server" Text="<%$ Resources:Localized, 超商條碼%>" /></td>
			<td colspan="3">&nbsp;</td>
		</tr>
		<tr>
			<td colspan="6"><asp:CheckBox ID="chkSerior_No" runat="server" Text="<%$ Resources:Localized, 流水號 (客戶編號中的流水號)%>" /></td>
			<td colspan="9">&nbsp;</td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkReceive_Amount" runat="server" Text="<%$ Resources:Localized, 繳費金額%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkCancel_No" runat="server" Text="<%$ Resources:Localized, 虛擬帳號%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkReceive_SMAmount" runat="server" Text="<%$ Resources:Localized, 超商繳費金額%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkCancel_SMNo" runat="server" Text="<%$ Resources:Localized, 超商虛擬帳號%>" /></td>
		</tr>
		<!-- 轉帳資料 -->
		<tr>
			<td colspan="3" style="font-size:10pt; text-align:left;"><asp:CheckBox ID="chkDeduct_BankID" runat="server" Text="<%$ Resources:Localized, 學生扣款轉帳銀行代碼%>" /></td>
			<td colspan="3" style="font-size:10pt; text-align:left;"><asp:CheckBox ID="chkDeduct_AccountNo" runat="server" Text="<%$ Resources:Localized, 學生扣款轉帳銀行帳號%>" /></td>
			<td colspan="3" style="font-size:10pt; text-align:left;"><asp:CheckBox ID="chkDeduct_AccountName" runat="server" Text="<%$ Resources:Localized, 學生扣款轉帳銀行戶名%>" /></td>
			<td colspan="3" style="font-size:10pt; text-align:left;"><asp:CheckBox ID="chkDeduct_AccountId" runat="server" Text="<%$ Resources:Localized, 學生扣款轉帳銀行ＩＤ%>" /></td>
		</tr>
		<!-- 備註資料 -->
		<tr id="trMemoRow1" runat="server">
			<td colspan="3"><asp:CheckBox ID="chkMemo01" runat="server" Text="備註01" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo02" runat="server" Text="備註02" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo03" runat="server" Text="備註03" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo04" runat="server" Text="備註04" /></td>
		</tr>
		<tr id="trMemoRow2" runat="server">
			<td colspan="3"><asp:CheckBox ID="chkMemo05" runat="server" Text="備註05" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo06" runat="server" Text="備註06" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo07" runat="server" Text="備註07" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo08" runat="server" Text="備註08" /></td>
		</tr>
		<tr id="trMemoRow3" runat="server">
			<td colspan="3"><asp:CheckBox ID="chkMemo09" runat="server" Text="備註09" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo10" runat="server" Text="備註10" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo11" runat="server" Text="備註11" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo12" runat="server" Text="備註12" /></td>
		</tr>
		<tr id="trMemoRow4" runat="server">
			<td colspan="3"><asp:CheckBox ID="chkMemo13" runat="server" Text="備註13" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo14" runat="server" Text="備註14" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo15" runat="server" Text="備註15" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo16" runat="server" Text="備註16" /></td>
		</tr>
		<tr id="trMemoRow5" runat="server">
			<td colspan="3"><asp:CheckBox ID="chkMemo17" runat="server" Text="備註17" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo18" runat="server" Text="備註18" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo19" runat="server" Text="備註19" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo20" runat="server" Text="備註20" /></td>
		</tr>
		<tr id="trMemoRow6" runat="server">
			<td colspan="3"><asp:CheckBox ID="chkMemo21" runat="server" Text="備註21" /></td>
			<td colspan="9">&nbsp;</td>
		</tr>
		<!-- 繳款狀態 -->
		<tr id="tr1" runat="server">
			<td colspan="3"><asp:CheckBox ID="chkReceive_Date" runat="server" Text="<%$ Resources:Localized, 代收日期%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkAccount_Date" runat="server" Text="<%$ Resources:Localized, 入帳日期%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkReceive_Way" runat="server" Text="<%$ Resources:Localized, 繳款管道代號%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkReceive_Way_Name" runat="server" Text="<%$ Resources:Localized, 繳款管道名稱%>" /></td>
		</tr>
		</table>
	</td>
</tr>
<tr>
	<td id="tdSaveExportConfig" style="padding: 2px 0px;">
		<div style="margin-bottom:5px;"><%= GetLocalized("存入資料項目設定") %>：
			<asp:DropDownList ID="ddlSaveExportConfig" runat="server">
			</asp:DropDownList>
		</div>
		<div style="margin-top:5px;"><%= GetLocalized("名稱") %>：
			<asp:TextBox ID="tbxSaveExportConfigName" runat="server" MaxLength="50" Width="80%"></asp:TextBox>&nbsp;&nbsp;
			<asp:LinkButton ID="lbtnSaveExportConfig" runat="server" CssClass="btn" OnClick="lbtnSaveExportConfig_Click" Text="<%$ Resources:Localized, 儲存%>"></asp:LinkButton>
		</div>
	</td>
</tr>
</table>

<div class="button">
    <cc:MyLinkButton ID="ccbtnDownload" runat="server" LocationText="下載XLS" CommandArgument="XLS" OnClick="ccbtnDownload_Click"></cc:MyLinkButton>
    <cc:MyLinkButton ID="ccbtnDownloadODS" runat="server" LocationText="下載ODS" CommandArgument="ODS" OnClick="ccbtnDownload_Click"></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript">
    function checkAll(obj) {
        var checked = obj.checked;
        $('input:checkbox:enabled').each(function (idx) {
            var cbx = $(this)[0];
            if (cbx.id != obj.id) {
                cbx.checked = checked;
            }
        });
    }

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

        $('#tdSaveExportConfig select').change(function () {
            var name = $('#tdSaveExportConfig input[type=text]').val().prune();
            if (name.length == 0) {
                var id = $('#tdSaveExportConfig select option:selected').val();
                name = $('#tdSaveExportConfig select option:selected').text().substr(5);
                $('#tdSaveExportConfig input[type=text]').val(name);
            }
        });
    });
</script>

</asp:Content>
