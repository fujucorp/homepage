<%@ Page Title="土地銀行 - 代收學雜費服務網 - 系統參數設定" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600016.aspx.cs" Inherits="eSchoolWeb.S.S5600016" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="result" class="result" summary="查詢結果" width="100%">
	<tr>
		<th width="220px"><%=this.GetLocalized("中國信託特殊商家代號") %>：</th>
		<td>
			<asp:TextBox ID="tbxCTCBSpecial" runat="server" Width="95%" MaxLength="500"></asp:TextBox>
			<br />(指定學校檔資料不使用費用別名稱的商家代號，每個商家代號以逗號隔開)
		</td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("FileService 網址") %>：</th>
		<td>
			<asp:TextBox ID="tbxFileServiceUrl" runat="server" Width="95%" MaxLength="100"></asp:TextBox>
			<br />(指定FTP外送檔案的WEB接收服務網址，必須以 http:// 或 https:// 開頭)
		</td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("學校銷帳檔3合1的商家代號") %>：</th>
		<td>
			<asp:TextBox ID="tbxSC31ReceiveType" runat="server" Width="97%" TextMode="MultiLine" Rows="3" Wrap="false"></asp:TextBox>
			<br />(指定使用 SC31-學校銷帳檔3合1 的商家代號，每個商家代號以逗號隔開)
		</td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("學生姓名要遮罩的商家代號") %>：</th>
		<td>
			<asp:TextBox ID="tbxMaskReceiveType" runat="server" Width="97%" TextMode="MultiLine" Rows="3" Wrap="false"></asp:TextBox>
			<br />(指定學生專區的網頁與PDF中學生姓名要遮罩的商家代號，多筆以逗號隔開)
		</td>
	</tr>
	<tr>
		<th><%=this.GetLocalized("顯示使用者密碼的學校代碼") %>：</th>
		<td>
			<asp:TextBox ID="tbxFixVerifySchoolId" runat="server" Width="97%" TextMode="MultiLine" Rows="3" Wrap="false"></asp:TextBox>
			<div style="color:red">
				注意 1：設定值必須是學校代碼，不是商家代號，多筆以逗號隔開<br />
				注意 2：這些學校不管是使用身份證號或生日做驗證欄位，在學生專區登入頁面中，驗證欄位名稱會顯示成「使用者密碼」
			</div>
		</td>
	</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>
</asp:Content>
