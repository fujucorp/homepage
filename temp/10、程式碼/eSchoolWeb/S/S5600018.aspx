<%@ Page Title="土地銀行 - 代收學雜費服務網 - 資料保留年限設定" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600018.aspx.cs" Inherits="eSchoolWeb.S.S5600018" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="information" class="information" summary="查詢結果" width="100%">  
<tr>
	<th>
		<uc:Filter1 ID="ucFilter1" runat="server" YearVisible="false" TermVisible="false" UIMode="Option" OnItemSelectedIndexChanged="ucFilter1_ItemSelectedIndexChanged" />
	</th>
</tr>
</table>

<div style="text-align:left; padding-top: 5px;">
    [注意]：<br />
    <ul style="list-style:disc; padding-left: 15px;">
    <li>【線上資料保留學年數】與【歷史資料保留學年數】預設值為「不限學年數」，<span style="color:red;">無須區分線上與歷史資料時，請勿設定</span>。</li>
    <li>【歷史資料保留學年數】的學年數必須比【線上資料保留學年數】的學年數大或指定為「不限學年數」。</li>
    <li>超過指定保留學年數的線上資料將移至歷史資料庫，<span style="color:red;">歷史資料只能使用『查詢歷史(繳費)資料』功能查詢</span>。</li>
    <li>超過指定保留學年數的歷史資料將從資料庫中移除。<span style="color:red;">移除的資料將永遠從系統中刪除，無任何功能可查詢</span>。</li>
    <li><span style="color:red;">系統不提供將歷史資料庫的資料移回線上資料庫。從資料庫中刪除的資料亦無法還原，請小心使用。</span></li>
    <li><span style="color:red;">改變設定後不會即時處理線上與歷史資料，而是由『線上資料搬移與歷史資料刪除』排程來處理。</span></li>
    </ul>
</div>

<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th><%=this.GetLocalized("線上資料保留學年數") %>：</th>
	<td>
		<asp:DropDownList ID="ddlKeepDataYear" runat="server"></asp:DropDownList><br />
		(EX：選擇 01個學年，則 <%=DateTime.Today.Year - 1911 - 1 %> 學年以前的線上資料會被移至歷史資料)
	</td>
</tr>
<tr>
	<th><%=this.GetLocalized("歷史資料保留學年數") %>：</th>
	<td>
		<asp:DropDownList ID="ddlKeepHistoryYear" runat="server"></asp:DropDownList><br />
		(EX：選擇 02個學年，則 <%=DateTime.Today.Year - 1911 - 2 %> 學年以前的歷史資料會從資料庫中刪除)<br />
	</td>
</tr>
<tr>
	<th><%=this.GetLocalized("套用保留學年數設定") %>：</th>
	<td>
		<asp:DropDownList ID="ddlCopyTo" runat="server"></asp:DropDownList>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

</asp:Content>
