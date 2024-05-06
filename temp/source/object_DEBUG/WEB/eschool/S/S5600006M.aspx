<%@ Page Title="土地銀行 - 代收學雜費服務網 - 廣告管理" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600006M.aspx.cs" Inherits="eSchoolWeb.S.S5600006M" %>
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
<table id="result" class="result" summary="查詢結果" width="100%">
<tr>
	<th width="30%"><cc:MyLabel ID="cclabAdId" runat="server" LocationText="廣告版位"></cc:MyLabel>：</th>
	<td>
		<asp:Label ID="labAdId" runat="server" Text=""></asp:Label>
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabKinde" runat="server" LocationText="廣告種類"></cc:MyLabel>：</th>
	<td id="tdKind">
		<asp:DropDownList ID="ddlKind" runat="server"></asp:DropDownList>
	</td>
</tr>
<tr id="trImgUrl">
	<th><cc:MyLabel ID="cclabImgUrl" runat="server" LocationText="圖檔網址"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxImgUrl" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
	</td>
</tr>
<tr id="trImgContent">
	<th><cc:MyLabel ID="cclabImgContent" runat="server" LocationText="上傳圖檔"></cc:MyLabel>：</th>
	<td>
		<asp:FileUpload ID="fupImgContent" runat="server" Width="80%" />
	</td>
</tr>
<tr>
	<th><cc:MyLabel ID="cclabLinkUrl" runat="server" LocationText="連結網址"></cc:MyLabel>：</th>
	<td>
		<asp:TextBox ID="tbxLinkUrl" runat="server" MaxLength="100" Width="90%"></asp:TextBox>
	</td>
</tr>
</table>

<div class="button">
	<cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
	<cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript">
    $(function () {
        $('#tdKind > select').change(function () {
            var kind = $(this).val();
            if (kind == "C") {
                $('#trImgContent').show();
                $('#trImgUrl').hide();
            } else {
                $('#trImgUrl').show();
                $('#trImgContent').hide();
                var control = $('#trImgContent');
                control.replaceWith(control = control.clone(true));
            }
        });

        $('#tdKind > select').trigger("change");
    });
</script>
</asp:Content>
