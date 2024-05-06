<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="S5100001.aspx.cs" Inherits="eSchoolWeb.S.S5100001" MasterPageFile="~/MasterPage/Main.Master" %>

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

<!--//查詢條件----------------------------------------------------------------->
<table id="condition" class="condition" summary="查詢條件" width="100%">
    <tr>
        <th width="30%">商家代號：</th>
        <td width="20%">
            <asp:DropDownList ID="ddlReceiveType" runat="server"></asp:DropDownList>
        </td>
        <th width="30%">學校代碼：</th>
        <td width="20%">
            <asp:TextBox ID="tbxSchIdenty" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th width="30%">學校名稱：</th>
        <td width="20%"><asp:TextBox ID="tbxSchName" runat="server"></asp:TextBox></td>
	    <th width="30%">學制：</th>
        <td width="20%">
            <asp:DropDownList ID="ddlCorpType" runat="server">
            </asp:DropDownList>
	    </td>
    </tr>
    <!--
    <tr>
	    <th width="30%">主辦分行：</th>
        <td width="20%">
            <asp:DropDownList ID="DropDownList1" runat="server">
            </asp:DropDownList>
	    </td>
        <th width="30%">縣市：</th>
        <td width="20%">
            <asp:DropDownList ID="DropDownList2" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    -->
    <tr>
	    <th width="30%">主辦分行：</th>
        <td width="20%">
            <asp:DropDownList ID="ddlBank" runat="server">
            </asp:DropDownList>
	    </td>
        <th width="30%">&nbsp;</th>
        <td width="20%">
            &nbsp;
        </td>
    </tr>
</table>
    
<div class="button">
	<cc:MyQueryButton ID="ccbtnQuery" runat="server" OnClick="ccbtnQuery_Click"></cc:MyQueryButton>
    <cc:MyInsertButton ID="ccbtnInsert" runat="server" OnClick="ccbtnInsert_Click"></cc:MyInsertButton>
</div>	

<div id="divResult" runat="server" >
	<div class='pageControl'>
		<uc:Paging ID="ucPaging1" runat="server"></uc:Paging>
	</div>
	<asp:GridView ID="gvResult" runat="server" CssClass="modify"
		AutoGenerateColumns="false" AllowPaging="false"
		RowStyle-CssClass="light" AlternatingRowStyle-CssClass="dark"
		OnRowCommand="gvResult_RowCommand" OnPreRender="gvResult_PreRender"
		EmptyDataText="<%$ Resources:Localized, 查無資料 %>" >
		<Columns>
			<cc:MyBoundField DataField="SchName" LocationHeaderText="學校名稱">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="SchIdenty" LocationHeaderText="學校代碼">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="CorpType" LocationHeaderText="學制">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="新增">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyInsertButton ID="ccbtnAdd" runat="server" CssClass="btn"></cc:MyInsertButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyBoundField DataField="ReceiveType" LocationHeaderText="商家代號" Visible="false">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="商家代號">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:LinkButton ID="lbtnViewD0071" runat="server" CssClass="btn" CommandName="ViewD0071"></asp:LinkButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyBoundField DataField="SchContract" LocationHeaderText="聯絡人<br />連絡電話">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyBoundField DataField="BankId" LocationHeaderText="主辦分行">
				<HeaderStyle Wrap="False"></HeaderStyle>
			</cc:MyBoundField>
			<cc:MyTemplateField LocationHeaderText="修改">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
					<cc:MyModifyButton ID="ccbtnModify" runat="server" CssClass="btn"></cc:MyModifyButton>
				</ItemTemplate>
			</cc:MyTemplateField>
			<cc:MyTemplateField LocationHeaderText="刪除">
				<ItemStyle HorizontalAlign="Center" />
				<ItemTemplate>
				<cc:MyDeleteButton ID="ccbtnDelete" runat="server" CssClass="btn" UseDefaultJSConfirm="false"></cc:MyDeleteButton>
				</ItemTemplate>
			</cc:MyTemplateField>
		</Columns>
	</asp:GridView>
	<div class='pageControl'>
		<uc:Paging ID="ucPaging2" runat="server"></uc:Paging>
	</div>
</div>

</asp:Content>
