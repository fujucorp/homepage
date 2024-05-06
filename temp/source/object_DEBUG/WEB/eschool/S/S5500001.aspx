<%@ Page Title="土地銀行 - 代收學雜費服務網 - 學校設定檔管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5500001.aspx.cs" Inherits="eSchoolWeb.S.S5500001" %>
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

<table id="information" class="information" summary="查詢結果" width="100%">  
<tr>
    <th>
        <uc:Filter1 ID="ucFilter1" runat="server" YearVisible="false" TermVisible="false" UIMode="Option" OnItemSelectedIndexChanged="ucFilter1_ItemSelectedIndexChanged" />
    </th>
</tr>
</table>

<table id="result" class="result" summary="查詢結果" width="100%">
	<tr>
		<th><%=this.GetLocalized("目前學年") %>：</th>
		<td><asp:DropDownList ID="ddlYearId" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearId_SelectedIndexChanged"></asp:DropDownList></td>
		<th><%=this.GetLocalized("目前學期") %>：</th>
		<td><asp:DropDownList ID="ddlTermId" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("是否開放學生專區") %>：</th>
		<td colspan="3">
			<asp:RadioButtonList ID="rdoOpenStudentArea" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
				<asp:ListItem Value="Y">開放</asp:ListItem>
				<asp:ListItem Value="N">不開放</asp:ListItem>
			</asp:RadioButtonList>
			&nbsp;&nbsp;&nbsp;&nbsp;(請注意：此設定同時作用在該學校的所有商家代號)
		</td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("學生登入依據") %>：</th>
		<td colspan="3">
			<asp:RadioButtonList ID="rdoLoginType" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
				<asp:ListItem Value="0">身分證字號</asp:ListItem>
				<asp:ListItem Value="1">生日</asp:ListItem>
			</asp:RadioButtonList>
			&nbsp;&nbsp;&nbsp;&nbsp;(請注意：此設定同時作用在該學校的所有商家代號)
		</td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("學校使用者審核階層") %>：</th>
		<td colspan="3">
			<asp:DropDownList ID="ddlFlowKind" runat="server"></asp:DropDownList>
			&nbsp;&nbsp;&nbsp;&nbsp;(請注意：此設定同時作用在該學校的所有商家代號)
		</td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("裁決章抬頭1") %>：</th>
		<td><asp:TextBox ID="tbxGiree11" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
		<th><%=this.GetLocalized("裁決章姓名1") %>：</th>
		<td><asp:TextBox ID="tbxGiree12" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
	</tr>
	<tr>
	<th><%=this.GetLocalized("裁決章抬頭2") %>：</th>
		<td><asp:TextBox ID="tbxGiree21" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
		<th><%=this.GetLocalized("裁決章姓名2") %>：</th>
		<td><asp:TextBox ID="tbxGiree22" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("裁決章抬頭3") %>：</th>
		<td><asp:TextBox ID="tbxGiree31" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
		<th><%=this.GetLocalized("裁決章姓名3") %>：</th>
		<td><asp:TextBox ID="tbxGiree32" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("裁決章抬頭4") %>：</th>
		<td><asp:TextBox ID="tbxGiree41" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
		<th><%=this.GetLocalized("裁決章姓名4") %>：</th>
		<td><asp:TextBox ID="tbxGiree42" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("裁決章抬頭5") %>：</th>
		<td><asp:TextBox ID="tbxGiree51" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
		<th><%=this.GetLocalized("裁決章姓名5") %>：</th>
		<td><asp:TextBox ID="tbxGiree52" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("裁決章抬頭6") %>：</th>
		<td><asp:TextBox ID="tbxGiree61" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
		<th><%=this.GetLocalized("裁決章姓名6") %>：</th>
		<td><asp:TextBox ID="tbxGiree62" runat="server" Width="40%" MaxLength="20"></asp:TextBox></td>
	</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

</asp:Content>
