<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護上傳繳費資料對照表" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1400001M.aspx.cs" Inherits="eSchoolWeb.D.D1400001M" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="false" TermVisible="false" AutoGetDataBound="false" />

<div id="divStep1" runat="server">
<table id="step1" class="condition" summary="step1" width="100%">
<tr>
	<th colspan="4"><div align="left"><%= GetLocalized("勾選上傳資料對照表項目") %></div></th>
</tr>
<tr>
	<th><%= GetLocalized("上傳檔案格式") %>：</th>
	<td>
		<asp:DropDownList ID="ddlFileType" runat="server">
			<asp:ListItem Selected="True" Value="xls">試算表(xls/xlsx/ods)</asp:ListItem>
			<asp:ListItem Value="txt">純文字(txt)</asp:ListItem>
		</asp:DropDownList>
	</td>
	<th><%= GetLocalized("對照表名稱") %>：</th>
	<td><asp:TextBox ID="tbxMappingName" runat="server"></asp:TextBox></td>
</tr>
<tr>
	<td colspan="4" style="border-bottom:0px hidden #e6e6e6">
		<table width="100%" class="#">
		<tr>
			<td colspan="12"><input type="checkbox" id="cbxCheckAll" onchange="checkAll(this);" />全選</td>
		</tr>
		<!-- 學生基本資料 -->
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkStu_Id" runat="server" Text="<%$ Resources:Localized, 學號%>" Enabled="false" Checked="true" /></td>
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
			<td colspan="3"><asp:CheckBox ID="chkStu_Parent" runat="server" Text="<%$ Resources:Localized, 家長名稱%>" /></td>
			<td colspan="9">&nbsp;</td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkOld_Seq" runat="server" Text="<%$ Resources:Localized, 序號%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkPay_Due_Date" runat="server" Text="<%$ Resources:Localized, 繳款期限%>" /></td>
			<td colspan="6"><asp:CheckBox ID="chkNCCardFlag" runat="server" Text="<%$ Resources:Localized, 是否啟用國際信用卡繳費%>" /></td>
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
			<td colspan="3">
				<asp:CheckBox ID="chkCredit" runat="server" Text="<%$ Resources:Localized, 學分基準或課程學分數%>" />&nbsp;
			</td>
			<td colspan="3">
				<asp:CheckBox ID="chkCredit_Id" runat="server" Text="<%$ Resources:Localized, 學分基準%>" />&nbsp;
				<asp:TextBox ID="tbxCreditIdCount" Width="30px" runat="server" MaxLength="2"></asp:TextBox><%= GetLocalized("項") %>
			</td>
			<td colspan="3">
				<asp:CheckBox ID="chkCourse_Id" runat="server" Text="<%$ Resources:Localized, 課程代碼%>" />&nbsp;
				<asp:TextBox ID="tbxCourseIdCount" Width="30px" runat="server" MaxLength="2"></asp:TextBox><%= GetLocalized("項") %>
			</td>
			<td colspan="3">
				<asp:CheckBox ID="chkReceive" runat="server" Text="<%$ Resources:Localized, 收入科目金額%>" />
				<asp:TextBox ID="tbxReceiveCount" Width="30px" runat="server" MaxLength="2"></asp:TextBox><%= GetLocalized("項") %>
			</td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkReceive_Amount" runat="server" Text="<%$ Resources:Localized, 繳費金額%>" /></td>
			<td colspan="6"><asp:CheckBox ID="chkSerior_No" runat="server" Text="<%$ Resources:Localized, 流水號 (客戶編號中的流水號)%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkCancel_No" runat="server" Text="<%$ Resources:Localized, 虛擬帳號%>" /></td>
		</tr>
		<!-- 轉帳資料 -->
		<tr>
			<td colspan="3" style="font-size:10pt; text-align:left;"><asp:CheckBox ID="chkDeduct_BankID" runat="server" Text="<%$ Resources:Localized, 學生扣款轉帳銀行代碼%>" /></td>
			<td colspan="3" style="font-size:10pt; text-align:left;"><asp:CheckBox ID="chkDeduct_AccountNo" runat="server" Text="<%$ Resources:Localized, 學生扣款轉帳銀行帳號%>" /></td>
			<td colspan="3" style="font-size:10pt; text-align:left;"><asp:CheckBox ID="chkDeduct_AccountName" runat="server" Text="<%$ Resources:Localized, 學生扣款轉帳銀行戶名%>" /></td>
			<td colspan="3" style="font-size:10pt; text-align:left;"><asp:CheckBox ID="chkDeduct_AccountId" runat="server" Text="<%$ Resources:Localized, 學生扣款轉帳銀行ＩＤ%>" /></td>
		</tr>
		<!-- 備註資料 -->
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkMemo01" runat="server" Text="<%$ Resources:Localized, 備註01%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo02" runat="server" Text="<%$ Resources:Localized, 備註02%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo03" runat="server" Text="<%$ Resources:Localized, 備註03%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo04" runat="server" Text="<%$ Resources:Localized, 備註04%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkMemo05" runat="server" Text="<%$ Resources:Localized, 備註05%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo06" runat="server" Text="<%$ Resources:Localized, 備註06%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo07" runat="server" Text="<%$ Resources:Localized, 備註07%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo08" runat="server" Text="<%$ Resources:Localized, 備註08%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkMemo09" runat="server" Text="<%$ Resources:Localized, 備註09%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo10" runat="server" Text="<%$ Resources:Localized, 備註10%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo11" runat="server" Text="<%$ Resources:Localized, 備註11%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo12" runat="server" Text="<%$ Resources:Localized, 備註12%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkMemo13" runat="server" Text="<%$ Resources:Localized, 備註13%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo14" runat="server" Text="<%$ Resources:Localized, 備註14%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo15" runat="server" Text="<%$ Resources:Localized, 備註15%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo16" runat="server" Text="<%$ Resources:Localized, 備註16%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkMemo17" runat="server" Text="<%$ Resources:Localized, 備註17%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo18" runat="server" Text="<%$ Resources:Localized, 備註18%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo19" runat="server" Text="<%$ Resources:Localized, 備註19%>" /></td>
			<td colspan="3"><asp:CheckBox ID="chkMemo20" runat="server" Text="<%$ Resources:Localized, 備註20%>" /></td>
		</tr>
		<tr>
			<td colspan="3"><asp:CheckBox ID="chkMemo21" runat="server" Text="<%$ Resources:Localized, 備註21%>" /></td>
			<td colspan="9">&nbsp;</td>
		</tr>
		</table>
	</td>
</tr>
</table>

<div class="button">
	<asp:LinkButton ID="lbtnGoStep2" runat="server" OnClick="lbtnGoStep2_Click"><%= GetLocalized("下一步") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
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
</script>
</div>

<div id="divStep2" runat="server" visible="false">
<table id="step2" class="condition" summary="step2" width="100%">
<tr>
	<th style="width:50%"><div align="left"><%= GetLocalized("上傳檔案格式") %>：<asp:Label ID="labFileType" runat="server" Text=""></asp:Label></div></th>
	<th style="width:50%"><div align="left"><%= GetLocalized("對照表名稱") %>：<asp:Label ID="labMappingName" runat="server" Text=""></asp:Label></div></th>
</tr>
<tr>
	<td colspan="2" style="border-bottom:0px hidden #e6e6e6">
		<asp:Literal ID="litFieldsHtml" runat="server"></asp:Literal>
	</td>
</tr>
</table>

<div class="button">
	<asp:LinkButton ID="lbtnGoStep1" runat="server"  OnClick="lbtnGoStep1_Click"><%= GetLocalized("上一步") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack2" runat="server"></cc:MyGoBackButton>
</div>
</div>
</asp:Content>
