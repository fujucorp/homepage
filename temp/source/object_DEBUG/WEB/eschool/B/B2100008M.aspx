<%@ Page Title="土地銀行 - 代收學雜費服務網 - 維護就貸資料" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="B2100008M.aspx.cs" Inherits="eSchoolWeb.B.B2100008M" %>

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
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" Filter2ControlID="ucFilter2" AutoGetDataBound="false" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" AutoGetDataBound="false" />

<br/>

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th><%=this.GetLocalized("學號") %>：</th>
	<td><asp:Label ID="labStuId" runat="server" ></asp:Label></td>
	<th><%=this.GetLocalized("姓名") %>：</th>
	<td><asp:Label ID="labStuName" runat="server" ></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("批號") %>：</th>
	<td><asp:Label ID="labUpNo" runat="server" ></asp:Label></td>
	<th><%=this.GetLocalized("序號") %>：</th>
	<td><asp:Label ID="labOldSeq" runat="server" ></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("科系") %>：</th>
	<td><asp:Label ID="labMajorName" runat="server" ></asp:Label></td>
	<th><%=this.GetLocalized("年級") %>：</th>
	<td><asp:Label ID="labStuGrade" runat="server" ></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("虛擬帳號") %>：</th>
	<td><asp:Label ID="labCancelNo" runat="server" Text=""></asp:Label></td>
	<th><%=this.GetLocalized("繳費金額") %>：</th>
	<td><asp:Label ID="labReceiveAmount" runat="server" Text=""></asp:Label></td>
</tr>
<tr>
	<th><%=this.GetLocalized("就貸代碼") %>：</th>
	<td><asp:DropDownList ID="ddlLoanId" runat="server"></asp:DropDownList></td>
	<th><%=this.GetLocalized("就貸總金額") %>：</th>
	<td><asp:TextBox ID="tbxLoanFixAmount" runat="server" MaxLength="9"></asp:TextBox></td>
</tr>
<tr>
	<th colspan="4"><div align="center"><%=this.GetLocalized("就貸明細") %></div></th>
</tr>
<tr>
	<td colspan="4">
		<table width="100%">
			<tr>
				<th width="50%" style="text-align:left;"><%=this.GetLocalized("收入科目") %></th>
				<th width="20%" style="text-align:left;"><%=this.GetLocalized("金額") %></th>
				<th width="30%" style="text-align:left;"><%=this.GetLocalized("就貸金額") %></th>
			</tr>
			<tr id="trItemRow01" runat="server" visible="false">
				<td><asp:Label ID="labItemName01" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount01" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan01" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow02" runat="server" visible="false">
				<td><asp:Label ID="labItemName02" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount02" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan02" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow03" runat="server" visible="false">
				<td><asp:Label ID="labItemName03" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount03" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan03" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow04" runat="server" visible="false">
				<td><asp:Label ID="labItemName04" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount04" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan04" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow05" runat="server" visible="false">
				<td><asp:Label ID="labItemName05" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount05" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan05" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow06" runat="server" visible="false">
				<td><asp:Label ID="labItemName06" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount06" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan06" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow07" runat="server" visible="false">
				<td><asp:Label ID="labItemName07" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount07" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan07" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow08" runat="server" visible="false">
				<td><asp:Label ID="labItemName08" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount08" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan08" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow09" runat="server" visible="false">
				<td><asp:Label ID="labItemName09" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount09" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan09" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow10" runat="server" visible="false">
				<td><asp:Label ID="labItemName10" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount10" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan10" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>

			<tr id="trItemRow11" runat="server" visible="false">
				<td><asp:Label ID="labItemName11" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount11" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan11" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow12" runat="server" visible="false">
				<td><asp:Label ID="labItemName12" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount12" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan12" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow13" runat="server" visible="false">
				<td><asp:Label ID="labItemName13" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount13" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan13" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow14" runat="server" visible="false">
				<td><asp:Label ID="labItemName14" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount14" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan14" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow15" runat="server" visible="false">
				<td><asp:Label ID="labItemName15" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount15" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan15" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow16" runat="server" visible="false">
				<td><asp:Label ID="labItemName16" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount16" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan16" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow17" runat="server" visible="false">
				<td><asp:Label ID="labItemName17" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount17" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan17" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow18" runat="server" visible="false">
				<td><asp:Label ID="labItemName18" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount18" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan18" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow19" runat="server" visible="false">
				<td><asp:Label ID="labItemName19" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount19" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan19" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow20" runat="server" visible="false">
				<td><asp:Label ID="labItemName20" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount20" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan20" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>

			<tr id="trItemRow21" runat="server" visible="false">
				<td><asp:Label ID="labItemName21" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount21" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan21" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow22" runat="server" visible="false">
				<td><asp:Label ID="labItemName22" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount22" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan22" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow23" runat="server" visible="false">
				<td><asp:Label ID="labItemName23" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount23" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan23" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow24" runat="server" visible="false">
				<td><asp:Label ID="labItemName24" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount24" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan24" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow25" runat="server" visible="false">
				<td><asp:Label ID="labItemName25" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount25" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan25" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow26" runat="server" visible="false">
				<td><asp:Label ID="labItemName26" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount26" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan26" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow27" runat="server" visible="false">
				<td><asp:Label ID="labItemName27" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount27" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan27" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow28" runat="server" visible="false">
				<td><asp:Label ID="labItemName28" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount28" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan28" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow29" runat="server" visible="false">
				<td><asp:Label ID="labItemName29" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount29" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan29" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow30" runat="server" visible="false">
				<td><asp:Label ID="labItemName30" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount30" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan30" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>

			<tr id="trItemRow31" runat="server" visible="false">
				<td><asp:Label ID="labItemName31" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount31" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan31" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow32" runat="server" visible="false">
				<td><asp:Label ID="labItemName32" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount32" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan32" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow33" runat="server" visible="false">
				<td><asp:Label ID="labItemName33" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount33" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan33" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow34" runat="server" visible="false">
				<td><asp:Label ID="labItemName34" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount34" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan34" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow35" runat="server" visible="false">
				<td><asp:Label ID="labItemName35" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount35" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan35" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow36" runat="server" visible="false">
				<td><asp:Label ID="labItemName36" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount36" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan36" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow37" runat="server" visible="false">
				<td><asp:Label ID="labItemName37" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount37" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan37" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow38" runat="server" visible="false">
				<td><asp:Label ID="labItemName38" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount38" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan38" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow39" runat="server" visible="false">
				<td><asp:Label ID="labItemName39" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount39" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan39" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
			<tr id="trItemRow40" runat="server" visible="false">
				<td><asp:Label ID="labItemName40" runat="server" ></asp:Label></td>
				<td><asp:Label ID="labItemAmount40" runat="server" ></asp:Label></td>
				<td><asp:TextBox ID="tbxItemLoan40" runat="server" MaxLength="9"></asp:TextBox></td>
			</tr>
		</table>
	</td>
</tr>
</table>

<div style="text-align:left">
    [操作說明]：<br />
    1.就貸代碼與就貸總金額為必填項目。<br />
    2.就貸明細可以不設定。<br />
    3.<span style="color:red;">如有修改就貸總金額或刪除就貸資料，請務必重新計算金額與虛擬帳號。</span><br />
</div>

<div class="button">
    <cc:MyLinkButton ID="ccbtnOK" runat="server" LocationText="存檔" OnClick="ccbtnOK_Click" ></cc:MyLinkButton>
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
