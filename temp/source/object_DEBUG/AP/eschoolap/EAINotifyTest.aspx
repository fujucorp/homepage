<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EAINotifyTest.aspx.cs" Inherits="WebAP.EAINotifyTest" ValidateRequest="False" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>資料查詢維護工具</title>
    <style type="text/css">
        html {
            display: none;
        }
    </style>
    <script  type="text/javascript">
        if (self === top) {
            document.documentElement.style.display = 'initial';
        }
        else {
            top.location = '/EAINotifyTest.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:100%; min-width:600px;">
        <div style="width:100%; text-align:center; border-bottom:dotted 2px #808080; margin-bottom:4px; padding-bottom:4px;">資料查詢維護工具</div>
        <div style="width:100%; min-height:200px; height:20%; border-bottom:dashed 2px #808080; margin-bottom:4px; padding-bottom:4px;">
            <div style="width:100%; padding:4px">執行 Sql 指令： (如要改變此輸入列數，請在網址加入 SqlRows 參數，參數值 10 ~ 30)</div>
            <div style="width:100%;">
                <asp:TextBox ID="tbxSql" runat="server" Width="100%" Height="100%" TextMode="MultiLine" Wrap="false" Rows="10" ></asp:TextBox>
            </div>
            <div style="width:100%; padding:4px">
                <asp:RadioButton ID="rbtIsSelectSql" runat="server" GroupName="CmdType" Checked="true" />執行查詢指令&nbsp;&nbsp;
                起始索引：<asp:TextBox ID="tbxStartIndex" runat="server" value="0" Width="80px"></asp:TextBox> &nbsp;&nbsp;&nbsp;&nbsp;
                最大筆數：<asp:TextBox ID="tbxMaxRecords" runat="server" value="100" Width="60px" MaxLength="4"></asp:TextBox> &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="rbtIsExecSql" runat="server" GroupName="CmdType" />
                執行增修刪指令 (INSERT/UPDATE/DELETE)&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnLogon" runat="server" Text="執行" OnClick="btnExec_Click" />
            </div>
        </div>
        <div style="width:100%; height:100%; min-height:300px; margin-bottom:4px; padding-bottom:4px;">
            <div style="width:100%; padding:4px">執行結果：</div>
            <div id="divResult" runat="server" style="width:auto; overflow:auto; border:solid 2px #808080; max-height:500px; min-height:300px; padding:5px"></div>
        </div>
    </div>

<%--    <div style="display:none">
        網址：<asp:TextBox ID="TextBox2" runat="server" Width="442px">http://localhost/eSchoolAP/EAINotify.aspx</asp:TextBox><br/><br/>
        <asp:TextBox ID="TextBox1" runat="server" Height="276px" TextMode="MultiLine" Width="647px"></asp:TextBox><br/>
        <asp:Button ID="Button1" runat="server" Text="發送" OnClick="Button1_Click" /><br/><br/>
        接收：<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
    </div>
    <div style="display:none">
        <asp:Button ID="btnEncodeInitKey" runat="server" Text="InitKey 加密" OnClick="btnEncodeInitKey_Click" />
        <asp:Label ID="labEncodeResult" runat="server"></asp:Label>
    </div>
    <div style="display:none;">
        商家代號：<asp:TextBox ID="tbxReceiveType" runat="server" MaxLength="4" Text="7202" ></asp:TextBox> <br />
        學年：<asp:TextBox ID="tbxYearId" runat="server" MaxLength="3" Text="104" ></asp:TextBox> <br />
        學期：<asp:TextBox ID="tbxTermId" runat="server" MaxLength="1" Text="1" ></asp:TextBox> <br />
        費用別：<asp:TextBox ID="tbxReceiveId" runat="server" MaxLength="1" Text="1" ></asp:TextBox> <br />
        資料批號：<asp:TextBox ID="tbxUpNo" runat="server" MaxLength="3" Text="7" ></asp:TextBox> <br />
        起始資料序號：<asp:TextBox ID="tbxSUpOrder" runat="server" MaxLength="6" Text="000601" ></asp:TextBox> <br />
        起始虛擬帳號流水號：<asp:TextBox ID="tbxSSeriroNo" runat="server" MaxLength="6" Text="002365" ></asp:TextBox> <br />
        結束資料序號：<asp:TextBox ID="tbxEUpOrder" runat="server" MaxLength="6" Text="000610" ></asp:TextBox> <br />
        <asp:Button ID="btnUpdateSeriorNoAndCancelNo" runat="server" Text="更新虛擬帳號相關資料" OnClick="btnUpdateSeriorNoAndCancelNo_Click" /><br />
        <asp:Label ID="labUpdateSeriorNoAndCancelNoResult" runat="server"></asp:Label>
    </div>--%>
    </form>
</body>
</html>
