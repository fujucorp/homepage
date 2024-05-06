<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1500009.aspx.cs" Inherits="eSchoolWeb.D.D1500009" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" YearVisible="true" TermVisible="true" AutoGetDataBound="true" Filter2ControlID="ucFilter2" />
    <uc:Filter2 ID="ucFilter2" runat="server" UIMode="Option" />
    
    <!--//表格_修改----------------------------------------------------------------->
    <table id="modify" class="modify" summary="表格_修改" width="100%">
        <tr>
            <th><%= GetLocalized("輸入工作表名稱") %>：</th>
            <td>
                <asp:TextBox ID="tbxSheetName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="light">
            <th><%= GetLocalized("選擇上傳檔案所在位置") %>：</th>
            <td>
                <asp:FileUpload ID="fileUpload" runat="server" />
			    <br />
			    *<%= GetLocalized("注意") %>：不得為中文檔名或 檔名含有減號(-)及其他不允許的符號(如#,&,(,$,@,),!,*,^,+,~...等),以免造成上傳匯入失敗
		    </td>
        </tr>
        <tr class="dark">
            <th><%= GetLocalized("上傳批號") %>：</th>
            <td><asp:Label ID="labSeriorNo" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr class="light">
        <td colspan="2">
			上傳資料須包含︰
			<br />
			[工作表名稱說明]<br />
			開啟Excel後,左下方的名稱即為工作表名稱(如圖所示)<br />
			<img src="../img/SheetName.JPG">
		</td>
        </tr>
        <tr id="trWaitingMsg" style="display:none">
            <td colspan="2" align="center">
                <%= GetLocalized("上傳檔案匯入相當耗時，請耐心等待") %>
            </td>
        </tr>
    </table>                  
    <div class="button">
        <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click" OnClientClick="showWaitingMsg();"><%= GetLocalized("上傳") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
        <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
    </div>
<script type="text/javascript" language="javascript">
    function showWaitingMsg() {
        $('.trWaitingMsg').show();
        return true;
    }
</script>
</asp:Content>
