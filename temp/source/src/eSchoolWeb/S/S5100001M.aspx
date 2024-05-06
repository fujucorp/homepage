<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="S5100001M.aspx.cs" Inherits="eSchoolWeb.S.S5100001M" MasterPageFile="~/MasterPage/Main.Master" %>

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

<!--//表格_修改----------------------------------------------------------------->
<table id="result" class="result" summary="查詢結果" width="100%">
    <tr>
        <th width="30%">商家代號：</th>
        <td width="20%"><asp:TextBox MaxLength="4" ID="tbxReceiveType" runat="server"></asp:TextBox></td>
        <!--
        <th width="30%">是否檢查繳費期限：</th>
        <td width="20%"><asp:CheckBox ID="chkPayOver" Text="是" runat="server" /></td>
        -->
        <th width="30%">停用：</th>
        <td><asp:CheckBox ID="chkStatus" runat="server" /></td>
    </tr>
    <tr>
        <th width="30%">檢碼規則：</th>
        <td colspan="3" width="70%">
            <asp:DropDownList ID="ddlCancelNoRule" runat="server"></asp:DropDownList>
            &nbsp;&nbsp;<font color="red">(＊請勿隨意異動此項設定，以免影響銷帳編號產生)</font>
        </td>
    </tr>
    <tr>
        <th width="30%">代收種類：</th>
        <td colspan="3" id="tdReceiveKind">
            <asp:RadioButtonList ID="rblReceiveKind" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ></asp:RadioButtonList>
            &nbsp;&nbsp;<font color="red">(＊請勿隨意異動此項設定，以免影響銷帳處理)</font>
        </td>
    </tr>
    <tr>
        <th width="30%">英文資料啟用：</th>
        <td colspan="3" id="tdEngEnabled">
            <asp:RadioButtonList ID="rblEngEnabled" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ></asp:RadioButtonList>
            &nbsp;&nbsp;<font color="red">(＊請勿隨意異動此項設定，以免影響資料維護)</font>
        </td>
    </tr>
    <tr>
        <th width="30%">學校代碼：</th>
        <td width="20%">
            <asp:TextBox ID="txtSchIdenty" runat="server"></asp:TextBox>
        </td>
        <th width="30%">客戶委託代號：</th>
        <td width="20%">
            <asp:TextBox ID="txtDeductId" MaxLength="10" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th width="30%">學校中文名稱：</th>
        <td><asp:TextBox MaxLength="54" ID="tbxSchName" runat="server"></asp:TextBox></td>
        <th width="30%">學校英文名稱：</th>
        <td><asp:TextBox MaxLength="140" ID="tbxSchEName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">校長姓名：</th>
        <td ><asp:TextBox MaxLength="10" ID="tbxSchPrincipal" runat="server"></asp:TextBox></td>
        <th width="30%">啟用兩碼費用別：</th>
        <td><asp:CheckBox ID="chkBigReceiveId" runat="server" /> <font color="red">(*啟用後無法取消)</font></td>
    </tr>
    <tr>
        <th width="30%">學校郵遞區號：</th>
        <td width="20%"><asp:TextBox MaxLength="5" ID="tbxSchPostal" runat="server"></asp:TextBox></td>
        <th width="30%">學校地址：</th>
        <td width="20%"><asp:TextBox MaxLength="100" ID="tbxSchAddress" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">學校網址：</th>
        <td width="20%"><asp:TextBox MaxLength="40" ID="tbxUrl" runat="server"></asp:TextBox></td>
        <th width="30%">學校電子郵件：</th>
        <td width="20%"><asp:TextBox MaxLength="500" ID="tbxSchMail" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">學校FTP種類：</th>
        <td width="20%">
            <asp:DropDownList ID="ddlFtpKind" runat="server">
                <asp:ListItem Value="">忽略</asp:ListItem>
                <asp:ListItem Value="FTP">FTP</asp:ListItem>
                <asp:ListItem Value="FTPS">FTPS</asp:ListItem>
                <asp:ListItem Value="SFTP">SFTP</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td width="50%"  colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <th width="30%">學校FTP伺服器：</th>
        <td width="20%"><asp:TextBox MaxLength="40" ID="tbxFtpLocation" runat="server"></asp:TextBox></td>
        <th width="30%">學校FTP埠：</th>
        <td width="20%"><asp:TextBox MaxLength="5" ID="tbxFtpPort" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">學校FTP帳號：</th>
        <td width="20%"><asp:TextBox MaxLength="15" ID="tbxFtpAccount" runat="server"></asp:TextBox></td>
        <th width="30%">學校FTP密碼：</th>
        <td width="20%"><asp:TextBox MaxLength="16" ID="tbxFtpPXX" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">主要聯絡人：</th>
        <td width="20%"><asp:TextBox MaxLength="10" ID="tbxSchContract" runat="server"></asp:TextBox></td>
        <th width="30%">連絡電話：</th>
        <td width="20%"><asp:TextBox MaxLength="20" ID="tbxSchConTel" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">次要聯絡人：</th>
        <td width="20%"><asp:TextBox MaxLength="10" ID="tbxSchContract1" runat="server"></asp:TextBox></td>
        <th width="30%">連絡電話：</th>
        <td width="20%"><asp:TextBox MaxLength="20" ID="tbxSchConTel1" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">主辦分行：</th>
        <td width="20%">
            <asp:DropDownList ID="ddlMainBank" runat="server">
            </asp:DropDownList>
        </td>
        <th width="30%">銀行電子郵件帳號：</th>
        <td width="20%"><asp:TextBox MaxLength="500" ID="tbxBankMail" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">戶名：</th>
        <td width="20%"><asp:TextBox MaxLength="54" ID="tbxAccountName" runat="server"></asp:TextBox></td>
        <th width="30%">帳號：</th>
        <td width="20%"><asp:TextBox MaxLength="21" ID="tbxSchAccount" runat="server"></asp:TextBox></td>
    </tr>
    <tr style="display:none">
        <th>優先順序：</th>
        <td>
            <asp:DropDownList ID="ddlPrioity" runat="server">
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="5">5</asp:ListItem>
            </asp:DropDownList>
        </td>
        <th width="30%">預估處理量：</th>
        <td width="20%"><asp:TextBox MaxLength="5" ID="tbxSchHAmount" runat="server"></asp:TextBox></td>
    </tr>
    <tr style="display:none">
        <th width="30%">代理銀行：</th>
        <td colspan="3">
            <asp:DropDownList ID="ddlBank" runat="server"></asp:DropDownList></td>
    </tr>
    <tr style="display:none">
        <th width="30%">是否參加其他管道：</th>
        <td colspan="3">
            <asp:DropDownList ID="ddlEFlag" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr style="display:none">
        <th width="30%">銷帳完帳和產生虛擬帳號後、再上傳學生資料：</th>
        <td colspan="3">
            <asp:DropDownList ID="ddlAFlag" runat="server">
                <asp:ListItem Value="N" Text="否"></asp:ListItem>
                <asp:ListItem Value="Y" Text="是"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr style="display:none">
        <th width="30%">分行控管才可新增代收費用別：</th>
        <td colspan="3">
            <asp:DropDownList ID="ddlCFlag" runat="server">
                <asp:ListItem Value="0" Text="否" Selected="True"></asp:ListItem>
                <asp:ListItem Value="1" Text="是"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr style="display:none">
        <th width="30%">繳費單格式：</th>
        <td colspan="3">
            <asp:DropDownList ID="ddlBillFormType" runat="server">
                <asp:ListItem Value="0" Text="二聯式繳費單"></asp:ListItem>
                <asp:ListItem Value="1" Text="三聯式繳費單"></asp:ListItem>
                <asp:ListItem Value="9" Text="專屬繳費單"></asp:ListItem>
            </asp:DropDownList>
            <!--
            <br />
            <asp:CheckBox ID="chkDiviFlag" runat="server" />提供分項繳費單，分
            <asp:TextBox ID="tbx" Width="100px" runat="server"></asp:TextBox>項(可輸入1~30)</td>
            -->
        </td>
    </tr>
    <tr style="display:none">
        <th width="30%">超商4~6萬使用的識別碼(40001~60000)：</th>
        <td colspan="3"></td>
    </tr>
    <tr>
        <th width="30%">學制：</th>
        <td colspan="3">
            <asp:DropDownList ID="ddlCorpType" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr style="display:none">
        <th width="30%">學校申貸名冊上傳後重新計算金額：</th>
        <td colspan="3"><asp:CheckBox ID="chkCalcSchoolLoan" runat="server" />是</td>
    </tr>
    <tr style="display:none">
        <th width="30%">郵局手續費(內含)：</th>
        <td colspan="3">
            <asp:CheckBox ID="chkPostFeeInclude" runat="server" />是。手續費金額2萬(含)以下
            <asp:TextBox MaxLength="10" ID="tbxPostFee1" Width="100px" runat="server"></asp:TextBox>。2萬以上
            <asp:TextBox MaxLength="10" ID="tbxPostFee2" Width="100px" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr style="display:none">
        <th width="30%">使用20碼的學號：</th>
        <td colspan="3"><asp:CheckBox ID="chkUseStuId20" runat="server" />是</td>
    </tr>
    <tr style="display:none">
        <th width="30%">使用「教育部補助」：</th>
        <td colspan="3">
            <asp:CheckBox ID="chkUseEduSubsidy" runat="server" />是。字樣
            <asp:TextBox MaxLength="40" ID="tbxEduSubsidyLabel" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th width="30%">財金特店代碼：</th>
        <td width="20%"><asp:TextBox ID="tbxMerchantId" runat="server" MaxLength="15" ></asp:TextBox></td>
        <th width="30%">財金端末機代號：</th>
        <td width="20%"><asp:TextBox ID="tbxTerminalId" runat="server" MaxLength="8" ></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">財金特店編號參數：</th>
        <td width="20%"><asp:TextBox ID="tbxMerId" runat="server" MaxLength="10"></asp:TextBox></td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <th colspan="4" style="border-bottom:1px solid #926002; text-align:center;">國際信用卡 - 財金特店參數</th>
    </tr>
    <tr>
        <th width="30%">特店代碼：</th>
        <td width="20%"><asp:TextBox ID="tbxMerchantId2" runat="server" MaxLength="15" ></asp:TextBox></td>
        <th width="30%">端末機代號：</th>
        <td width="20%"><asp:TextBox ID="tbxTerminalId2" runat="server" MaxLength="8" ></asp:TextBox></td>
    </tr>
    <tr>
        <th width="30%">特店編號參數：</th>
        <td width="20%"><asp:TextBox ID="tbxMerId2" runat="server" MaxLength="10"></asp:TextBox></td>
        <th width="30%">手續費率：</th>
        <td width="20%"><asp:TextBox ID="tbxHandlingFeeRate" runat="server" MaxLength="8"></asp:TextBox></td>
    </tr>
</table>

<div class="button">
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

    <script type="text/javascript">
        $(function () {
            var rkfg = false;
            $('#tdReceiveKind input[type=radio]').change(function () {
                if (!rkfg) {
                    rkfg = true;
                    alert('請注意，【代收種類】會影響銷帳處理，儲存資料前請確認');
                }
            });
        });
    </script>

</asp:Content>
