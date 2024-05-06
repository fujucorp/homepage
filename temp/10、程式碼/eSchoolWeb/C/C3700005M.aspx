<%@ Page Title="土地銀行 - 代收學雜費服務網 - 還原銷帳註記" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="C3700005M.aspx.cs" Inherits="eSchoolWeb.C.C3700005M" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" AutoGetDataBound="false" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" AutoGetDataBound="false" />

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("學生基本資料") %></div></th>
</tr>
<tr>
	<th><%=this.GetLocalized("學號") %>：</th>
	<td><asp:Label ID="labStuId" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("姓名") %>：</th>
	<td><asp:Label ID="labName" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("身分證字號") %>：</th>
	<td><asp:Label ID="labIdNumber" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("電話") %>：</th>
	<td><asp:Label ID="labTel" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("生日") %>：</th>
	<td><asp:Label ID="labBirthday" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("郵遞區號") %>：</th>
	<td><asp:Label ID="labZipCode" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("住址") %>：</th>
	<td><asp:Label ID="labAddress" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("電子郵件") %>：</th>
	<td><asp:Label ID="labEmail" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("家長名稱") %>：</th>
	<td><asp:Label ID="labStuParent" runat="server" Text=""></asp:Label></td>
	<th>&nbsp;</th>
	<td>&nbsp;</td>
</tr>
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("繳費資料") %></div></th>
</tr>
<tr>
	<th><%=this.GetLocalized("批號、舊資料序號") %>：</th>
	<td><asp:Label ID="labUpNo" runat="server" ></asp:Label></td>
	<th><%=this.GetLocalized("部別") %>：</th>
	<td><asp:Label ID="labDeptName" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("院別") %>：</th>
	<td><asp:Label ID="labCollegeName" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("科系") %>：</th>
	<td><asp:Label ID="labMajorName" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("年級") %>：</th>
	<td><asp:Label ID="labStuGrade" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("班別") %>：</th>
	<td><asp:Label ID="labClassName" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("座號") %>：</th>
	<td><asp:Label ID="labStuHid" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("住宿") %>：</th>
	<td><asp:Label ID="labDormName" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("減免") %>：</th>
	<td><asp:Label ID="labReduceName" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("上傳就學貸款金額") %>：</th>
	<td><asp:Label ID="labLoan" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("就貸") %>：</th>
	<td><asp:Label ID="labLoanName" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("原就學貸款金額") %>：</th>
	<td><asp:Label ID="labRealLoan" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("身份註記一") %>：</th>
	<td><asp:Label ID="labIdentifyId01Name" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("身份註記二") %>：</th>
	<td><asp:Label ID="labIdentifyId02Name" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("身份註記三") %>：</th>
	<td><asp:Label ID="labIdentifyId03Name" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("身份註記四") %>：</th>
	<td><asp:Label ID="labIdentifyId04Name" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("身份註記五") %>：</th>
	<td><asp:Label ID="labIdentifyId05Name" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("身份註記六") %>：</th>
	<td><asp:Label ID="labIdentifyId06Name" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("學分數") %>：</th>
	<td><asp:Label ID="labStuCredit" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("上課時數") %>：</th>
	<td><asp:Label ID="labStuHour" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("補單註記") %>：</th>
	<td colspan="3"><asp:Label ID="labReissueFlag" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("收入明細") %></div></th>
</tr>
<tr>
	<td colspan="4">
		<table width="100%">
			<tr><td width="60%"><%=this.GetLocalized("收入科目") %></td><td><%=this.GetLocalized("金額") %></td></tr>
			<asp:Literal ID="litReceiveItemHtml" runat="server"></asp:Literal>
		</table>
	</td>
</tr>
<tr id="trMemoRow00" runat="server">
	<th colspan="4"><div align="center"><%=this.GetLocalized("備註") %></div></th>
</tr>
<tr id="trMemoRow01" runat="server">
	<th><asp:Label ID="labMemoTitle01" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo01" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle02" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo02" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow02" runat="server">
	<th><asp:Label ID="labMemoTitle03" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo03" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle04" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo04" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow03" runat="server">
	<th><asp:Label ID="labMemoTitle05" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo05" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle06" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo06" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow04" runat="server">
	<th><asp:Label ID="labMemoTitle07" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo07" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle08" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo08" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow05" runat="server">
	<th><asp:Label ID="labMemoTitle09" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo09" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle10" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo10" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow06" runat="server">
	<th><asp:Label ID="labMemoTitle11" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo11" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle12" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo12" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow07" runat="server">
	<th><asp:Label ID="labMemoTitle13" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo13" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle14" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo14" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow08" runat="server">
	<th><asp:Label ID="labMemoTitle15" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo15" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle16" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo16" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow09" runat="server">
	<th><asp:Label ID="labMemoTitle17" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo17" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle18" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo18" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow10" runat="server">
	<th><asp:Label ID="labMemoTitle19" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo19" runat="server" ></asp:Label></td>
	<th><asp:Label ID="labMemoTitle20" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo20" runat="server" ></asp:Label></td>
</tr>
<tr id="trMemoRow11" runat="server">
	<th><asp:Label ID="labMemoTitle21" runat="server" ></asp:Label></th>
	<td><asp:Label ID="labMemo21" runat="server" ></asp:Label></td>
	<th>&nbsp;</th>
	<td>&nbsp;</td>
</tr>
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("繳費/銷帳資料") %></div></th>
</tr>
<tr>
	<th><%=this.GetLocalized("繳費金額合計") %>：</th>
	<td><asp:Label ID="labReceiveAmount" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("虛擬帳號") %>：</th>
	<td><asp:Label ID="labCancelNo" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("臨櫃金額合計") %>：</th>
	<td><asp:Label ID="labReceiveATMAmount" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("臨櫃虛擬帳號") %>：</th>
	<td><asp:Label ID="labCancelATMNo" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("超商繳費金額") %>：</th>
	<td><asp:Label ID="labReceiveSMAmount" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("超商虛擬帳號") %>：</th>
	<td><asp:Label ID="labCancelSMNo" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("銷帳狀態") %>：</th>
	<td><asp:Label ID="labCancelStatus" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("繳費方式") %>：</th>
	<td><asp:Label ID="labReceiveWay" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("代收銀行/分行") %>：</th>
	<td><asp:Label ID="labReceiveBankId" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("代收日") %>：</th>
	<td><asp:Label ID="labReceiveDate" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("入帳日") %>：</th>
	<td colspan="3"><asp:Label ID="labAccountDate" runat="server" Text=""></asp:Label></td>
</tr>
</table>

<div class="button">
    <cc:MyLinkButton ID="ccbtnOK" runat="server" LocationText="還原銷帳註記" OnClick="ccbtnOK_Click"></cc:MyLinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
