<%@ Page Title="土地銀行 - 代收學雜費服務網 - 單筆新增學生繳費資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2100001.aspx.cs" Inherits="eSchoolWeb.B.B2100001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    .max-w180px {
        max-width: 180px;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" OnItemSelectedIndexChanged="ucFilter2_ItemSelectedIndexChanged" />

<br/>

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("學生基本資料") %></div></th>
</tr>
<tr>
	<th><%=this.GetLocalized("學號") %>：</th>
	<td><asp:TextBox ID="tbxStuId" runat="server" MaxLength="20" OnTextChanged="tbxStuId_TextChanged" AutoPostBack="true"></asp:TextBox></td>
	<th><%=this.GetLocalized("姓名") %>：</th>
	<td><asp:TextBox ID="tbxName" runat="server" MaxLength="60"></asp:TextBox></td>
</tr>
<tr>
	<th><%=this.GetLocalized("身分證字號") %>：</th>
	<td><asp:TextBox ID="tbxIdNumber" runat="server" MaxLength="10"></asp:TextBox></td>
	<th><%=this.GetLocalized("電話") %>：</th>
	<td><asp:TextBox ID="tbxTel" runat="server" MaxLength="14"></asp:TextBox></td>
</tr>
<tr>
	<th><%=this.GetLocalized("生日") %>：</th>
	<td><asp:TextBox ID="tbxBirthday" runat="server" CssClass="birthday" MaxLength="10" Width="100px"></asp:TextBox></td>
	<th><%=this.GetLocalized("郵遞區號") %>：</th>
	<td><asp:TextBox ID="tbxZipCode" runat="server" MaxLength="5" Width="100px"></asp:TextBox></td>
</tr>
<tr>
	<th><%=this.GetLocalized("住址") %>：</th>
	<td><asp:TextBox ID="tbxAddress" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><%=this.GetLocalized("電子郵件") %>：</th>
	<td><asp:TextBox ID="tbxEmail" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr>
	<th><%=this.GetLocalized("家長名稱") %>：</th>
	<td><asp:TextBox ID="tbxStuParent" runat="server" MaxLength="60"></asp:TextBox></td>
	<th>&nbsp;</th>
	<td>&nbsp;</td>
</tr>
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("繳費資料") %></div></th>
</tr>
<tr>
	<th><%=this.GetLocalized("批號、序號") %>：</th>
	<td><asp:Label ID="labUpNo" runat="server" ></asp:Label></td>
	<th><%=this.GetLocalized("繳款期限") %>：</th>
	<td><asp:TextBox ID="tbxPayDueDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox></td>
</tr>
<tr>
	<th><%=this.GetLocalized("部別") %>：</th>
	<td><asp:DropDownList ID="ddlDeptId" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><asp:Label ID="labNCCardFlag" runat="server" Text="國際信用卡繳費" ></asp:Label></th>
	<td><asp:DropDownList ID="ddlNCCardFlag" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
</tr>
<tr>
	<th><%=this.GetLocalized("院別") %>：</th>
	<td><asp:DropDownList ID="ddlCollegeId" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><%=this.GetLocalized("科系") %>：</th>
	<td><asp:DropDownList ID="ddlMajorId" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
</tr>
<tr>
	<th><%=this.GetLocalized("年級") %>：</th>
	<td><asp:DropDownList ID="ddlStuGrade" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><%=this.GetLocalized("班別") %>：</th>
	<td><asp:DropDownList ID="ddlClassId" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
</tr>
<tr>
	<th><%=this.GetLocalized("座號") %>：</th>
	<td><asp:TextBox ID="tbxStuHid" runat="server" MaxLength="10"></asp:TextBox></td>
	<th><%=this.GetLocalized("住宿") %>：</th>
	<td><asp:DropDownList ID="ddlDormId" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
</tr>
<tr>
	<th><%=this.GetLocalized("減免") %>：</th>
	<td><asp:DropDownList ID="ddlReduceId" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><%=this.GetLocalized("上傳就學貸款金額") %>：</th>
	<td><asp:TextBox ID="tbxLoan" runat="server" MaxLength="9"></asp:TextBox></td>
</tr>
<tr>
	<th><%=this.GetLocalized("就貸") %>：</th>
	<td><asp:DropDownList ID="ddlLoanId" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><%=this.GetLocalized("可貸金額") %>：</th>
	<td><asp:TextBox ID="tbxLoanAmount" runat="server" MaxLength="9"></asp:TextBox></td>
</tr>
<tr>
	<th><%=this.GetLocalized("身份註記一") %>：</th>
	<td><asp:DropDownList ID="ddlIdentifyId01" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><%=this.GetLocalized("身份註記二") %>：</th>
	<td><asp:DropDownList ID="ddlIdentifyId02" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
</tr>
<tr>
	<th><%=this.GetLocalized("身份註記三") %>：</th>
	<td><asp:DropDownList ID="ddlIdentifyId03" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><%=this.GetLocalized("身份註記四") %>：</th>
	<td><asp:DropDownList ID="ddlIdentifyId04" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
</tr>
<tr>
	<th><%=this.GetLocalized("身份註記五") %>：</th>
	<td><asp:DropDownList ID="ddlIdentifyId05" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><%=this.GetLocalized("身份註記六") %>：</th>
	<td><asp:DropDownList ID="ddlIdentifyId06" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
</tr>
<tr>
	<th><%=this.GetLocalized("學分數") %>：</th>
	<td><asp:TextBox ID="tbxStuCredit" runat="server" MaxLength="3"></asp:TextBox></td>
	<th><%=this.GetLocalized("上課時數") %>：</th>
	<td><asp:TextBox ID="tbxStuHour" runat="server" MaxLength="3"></asp:TextBox></td>
</tr>
<tr>
	<th><%=this.GetLocalized("補單註記") %>：</th>
	<td><asp:DropDownList ID="ddlReissueFlag" runat="server" CssClass="max-w180px"></asp:DropDownList></td>
	<th><%=this.GetLocalized("計算方式") %>：</th>
	<td>
		<asp:DropDownList ID="ddlBillingType" runat="server" CssClass="max-w180px">
			<asp:ListItem Value="2" Text="依金額計算"></asp:ListItem>
			<asp:ListItem Value="1" Text="依標準計算"></asp:ListItem>
		</asp:DropDownList>
	</td>
</tr>
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("收入明細") %></div></th>
</tr>
<tr>
	<td colspan="4">
		<table id="tblReceiveItems" width="100%">
		<tr>
			<td width="60%"><%=this.GetLocalized("收入科目") %></td>
			<td><%=this.GetLocalized("金額") %></td>
		</tr>
		<tr id="trItemRow01" runat="server">
			<td width="60%">01：<asp:Label ID="labItemName01" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue01" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow02" runat="server">
			<td width="60%">02：<asp:Label ID="labItemName02" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue02" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow03" runat="server">
			<td width="60%">03：<asp:Label ID="labItemName03" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue03" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow04" runat="server">
			<td width="60%">04：<asp:Label ID="labItemName04" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue04" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow05" runat="server">
			<td width="60%">05：<asp:Label ID="labItemName05" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue05" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow06" runat="server">
			<td width="60%">06：<asp:Label ID="labItemName06" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue06" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow07" runat="server">
			<td width="60%">07：<asp:Label ID="labItemName07" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue07" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow08" runat="server">
			<td width="60%">08：<asp:Label ID="labItemName08" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue08" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow09" runat="server">
			<td width="60%">09：<asp:Label ID="labItemName09" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue09" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow10" runat="server">
			<td width="60%">10：<asp:Label ID="labItemName10" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue10" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow11" runat="server">
			<td width="60%">11：<asp:Label ID="labItemName11" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue11" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow12" runat="server">
			<td width="60%">12：<asp:Label ID="labItemName12" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue12" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow13" runat="server">
			<td width="60%">13：<asp:Label ID="labItemName13" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue13" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow14" runat="server">
			<td width="60%">14：<asp:Label ID="labItemName14" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue14" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow15" runat="server">
			<td width="60%">15：<asp:Label ID="labItemName15" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue15" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow16" runat="server">
			<td width="60%">16：<asp:Label ID="labItemName16" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue16" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow17" runat="server">
			<td width="60%">17：<asp:Label ID="labItemName17" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue17" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow18" runat="server">
			<td width="60%">18：<asp:Label ID="labItemName18" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue18" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow19" runat="server">
			<td width="60%">19：<asp:Label ID="labItemName19" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue19" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow20" runat="server">
			<td width="60%">20：<asp:Label ID="labItemName20" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue20" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow21" runat="server">
			<td width="60%">21：<asp:Label ID="labItemName21" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue21" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow22" runat="server">
			<td width="60%">22：<asp:Label ID="labItemName22" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue22" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow23" runat="server">
			<td width="60%">23：<asp:Label ID="labItemName23" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue23" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow24" runat="server">
			<td width="60%">24：<asp:Label ID="labItemName24" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue24" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow25" runat="server">
			<td width="60%">25：<asp:Label ID="labItemName25" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue25" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow26" runat="server">
			<td width="60%">26：<asp:Label ID="labItemName26" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue26" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow27" runat="server">
			<td width="60%">27：<asp:Label ID="labItemName27" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue27" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow28" runat="server">
			<td width="60%">28：<asp:Label ID="labItemName28" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue28" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow29" runat="server">
			<td width="60%">29：<asp:Label ID="labItemName29" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue29" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow30" runat="server">
			<td width="60%">30：<asp:Label ID="labItemName30" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue30" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow31" runat="server">
			<td width="60%">31：<asp:Label ID="labItemName31" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue31" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow32" runat="server">
			<td width="60%">32：<asp:Label ID="labItemName32" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue32" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow33" runat="server">
			<td width="60%">33：<asp:Label ID="labItemName33" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue33" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow34" runat="server">
			<td width="60%">34：<asp:Label ID="labItemName34" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue34" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow35" runat="server">
			<td width="60%">35：<asp:Label ID="labItemName35" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue35" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow36" runat="server">
			<td width="60%">36：<asp:Label ID="labItemName36" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue36" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow37" runat="server">
			<td width="60%">37：<asp:Label ID="labItemName37" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue37" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow38" runat="server">
			<td width="60%">38：<asp:Label ID="labItemName38" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue38" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow39" runat="server">
			<td width="60%">39：<asp:Label ID="labItemName39" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue39" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		<tr id="trItemRow40" runat="server">
			<td width="60%">40：<asp:Label ID="labItemName40" runat="server" ></asp:Label></td>
			<td><asp:TextBox ID="tbxItemValue40" runat="server" MaxLength="9" ></asp:TextBox></td>
		</tr>
		</table>
		<asp:Literal ID="litReceiveSumHtml" runat="server"></asp:Literal>
	</td>
</tr>
<tr id="trMemoRow00" runat="server">
	<th colspan="4"><div align="center"><%=this.GetLocalized("備註") %></div></th>
</tr>
<tr id="trMemoRow01" runat="server">
	<th><asp:Label ID="labMemoTitle01" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue01" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle02" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue02" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow02" runat="server">
	<th><asp:Label ID="labMemoTitle03" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue03" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle04" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue04" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow03" runat="server">
	<th><asp:Label ID="labMemoTitle05" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue05" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle06" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue06" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow04" runat="server">
	<th><asp:Label ID="labMemoTitle07" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue07" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle08" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue08" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow05" runat="server">
	<th><asp:Label ID="labMemoTitle09" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue09" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle10" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue10" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow06" runat="server">
	<th><asp:Label ID="labMemoTitle11" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue11" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle12" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue12" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow07" runat="server">
	<th><asp:Label ID="labMemoTitle13" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue13" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle14" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue14" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow08" runat="server">
	<th><asp:Label ID="labMemoTitle15" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue15" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle16" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue16" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow09" runat="server">
	<th><asp:Label ID="labMemoTitle17" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue17" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle18" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue18" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow10" runat="server">
	<th><asp:Label ID="labMemoTitle19" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue19" runat="server" MaxLength="50"></asp:TextBox></td>
	<th><asp:Label ID="labMemoTitle20" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue20" runat="server" MaxLength="50"></asp:TextBox></td>
</tr>
<tr id="trMemoRow11" runat="server">
	<th><asp:Label ID="labMemoTitle21" runat="server" ></asp:Label></th>
	<td><asp:TextBox ID="tbxMemoValue21" runat="server" MaxLength="50"></asp:TextBox></td>
	<th>&nbsp;</th>
	<td>&nbsp;</td>
</tr>
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("學生扣款轉帳資料") %></div></th>
</tr>
<tr>
	<th style="padding:8px 0px;"><%=this.GetLocalized("扣款轉帳銀行代碼") %>：</th>
	<td><asp:TextBox ID="tbxDeductBankId" runat="server" MaxLength="7" Width="100px"></asp:TextBox></td>
	<th style="padding:8px 0px;"><%=this.GetLocalized("扣款轉帳銀行帳號") %>：</th>
	<td><asp:TextBox ID="tbxDeductAccountNo" runat="server" MaxLength="16" ></asp:TextBox></td>
</tr>
<tr>
	<th style="padding:8px 0px;"><%=this.GetLocalized("扣款轉帳銀行帳號戶名") %>：</th>
	<td><asp:TextBox ID="tbxDeductAccountName" runat="server" MaxLength="60" ></asp:TextBox></td>
	<th style="padding:8px 0px;"><%=this.GetLocalized("扣款轉帳銀行帳戶ＩＤ") %>：</th>
	<td><asp:TextBox ID="tbxDeductAccountId" runat="server" MaxLength="10" Width="120px"></asp:TextBox></td>
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
</table>

<div class="button">
    <cc:MyLinkButton ID="ccbtnSave" runat="server" LocationText="儲存資料" OnClick="ccbtnSave_Click"></cc:MyLinkButton><br /><br />
    <cc:MyLinkButton ID="ccbtnCalc" runat="server" LocationText="計算金額" OnClick="ccbtnCalc_Click"></cc:MyLinkButton><br /><br />
    <cc:MyLinkButton ID="ccbtnGenBill" runat="server" LocationText="產生PDF繳費單" OnClick="ccbtnGenBill_Click"></cc:MyLinkButton><br />&nbsp;<br />
    <cc:MyLinkButton ID="ccbtnGenEngBill" runat="server" LocationText="產生英文PDF繳費單" OnClick="ccbtnGenEngBill_Click" ></cc:MyLinkButton><br /><br />
    <cc:MyLinkButton ID="ccbtnNewData" runat="server" LocationText="新增下一筆" OnClick="ccbtnNewData_Click"></cc:MyLinkButton>&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server" OnClick="ccbtnGoBack_Click"></cc:MyGoBackButton>
</div>

</asp:Content>
