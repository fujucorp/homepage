<%@ Page Title="土地銀行 - 代收學雜費服務網 - KP3預設值維護" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S6200001.aspx.cs" Inherits="eSchoolWeb.S.S6200001" %>

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
<div>
    <table class="result" summary="編輯資料" width="100%">
    <tr>
        <th width="150"><%=this.GetLocalized("報送單位代號") %>：</th>
        <td><asp:TextBox ID="tbxUnit" runat="server" MaxLength="3" Width="80"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("管理者清單") %>：</th>
        <td>
            <asp:TextBox ID="tbxManagers" runat="server" MaxLength="35" Width="90%"></asp:TextBox>
            <div style="font-size:12px;">(請輸入員工編號並以逗號隔開，作多 5 筆)</div>
        </td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("FTP 網址") %>：</th>
        <td>
            <asp:TextBox ID="tbxFTPUrl" runat="server" MaxLength="100" Width="90%"></asp:TextBox>
            <div style="font-size:12px;">(請輸入 ftp/ftps/sftp 開頭的網址</div>
        </td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("FTP 帳號") %>：</th>
        <td><asp:TextBox ID="tbxFTPAcct" runat="server" MaxLength="20" Width="200"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("FTP 密碼") %>：</th>
        <td>
            <asp:TextBox ID="tbxFTPPXX" runat="server" MaxLength="20" Width="200"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("資訊格式代號") %>：</th>
        <td><asp:TextBox ID="tbxHeadItem01" runat="server" MaxLength="18" Width="200"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("聯絡電話") %>：</th>
        <td>
            <asp:TextBox ID="tbxHeadItem07" runat="server" MaxLength="16" Width="200"></asp:TextBox>
            <div style="font-size:12px;">(格式：區域碼-電話號碼#分機號碼)</div>
        </td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("聯絡人資訊或訊息") %>：</th>
        <td><asp:TextBox ID="tbxHeadItem08" runat="server" MaxLength="80" Width="90%"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("特約機構屬性") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem05" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("特約機構類型") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem09" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("營業型態") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem23" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("業務行為") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem26" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("是否受理電子支付帳戶或儲值卡服務") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem27" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("是否受理信用卡服務") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem29" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("是否有銷售遞延性商品或服務") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem33" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("是否安裝端末設備") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem34" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("是否安裝錄影設備") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem35" runat="server" /></td>
    </tr>
    <tr>
        <th width="150"><%=this.GetLocalized("連鎖店加盟或直營") %>：</th>
        <td><asp:DropDownList ID="ddlDataItem36" runat="server" /></td>
    </tr>
    </table>
    <div class="button">
        <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
    </div>
</div>
</asp:Content>
