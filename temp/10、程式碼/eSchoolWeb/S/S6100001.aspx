<%@ Page Title="土地銀行 - 代收學雜費服務網 - KP3資料登錄" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S6100001.aspx.cs" Inherits="eSchoolWeb.S.S6100001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    table.result td span {
        padding: 0;
    }

    table.result td select {
        max-width: 200px;
        padding: 5px;
    }

    span label {
        margin-right: 10px;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="divQuery" runat="server">
    <table class="information" summary="查詢條件" width="100%">
    <tr>
        <td width="120"><%= GetLocalized("特約機構代號") %>：</td>
        <td><asp:TextBox ID="tbxQItem07" runat="server" MaxLength="20" Width="60%"></asp:TextBox></td>
        <td style="width:120px;" class="button">
            <cc:MyLinkButton ID="ccbtnEdit" runat="server" OnClick="ccbtnEdit_Click" Text="登錄"></cc:MyLinkButton>
        </td>
    </tr>
    </table>
</div>

<div id="divEdit" runat="server" visible="false">
    <table class="result" summary="編輯資料" width="100%">
    <tr>
        <th width="150"><%=this.GetLocalized("資料別") %>：</th>
        <td><asp:Label ID="lblItem01" runat="server" ></asp:Label></td>
        <th width="150"><%=this.GetLocalized("報送代碼") %>：</th>
        <td>
            <asp:Label ID="lblItem02" runat="server" >A - 新增</asp:Label>
            <asp:DropDownList ID="ddlItem02" runat="server" />
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("電子支付機構代號") %>：</th>
        <td><asp:Label ID="lblItem03" runat="server" ></asp:Label></td>
        <th><%=this.GetLocalized("特約機構屬性") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem05" runat="server" Enabled="false" />
        </td>
    </tr>
    <tr>
        <th style="font-size:11pt;"><%=this.GetLocalized("特約機構 BAN/IDN") %>：</th>
        <td><asp:TextBox ID="tbxItem06" runat="server" MaxLength="10" Width="50%"></asp:TextBox></td>
        <th><%=this.GetLocalized("特約機構代號") %>：</th>
        <td><asp:TextBox ID="tbxItem07" runat="server" MaxLength="20" Width="50%" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("特約機構類型") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem09" runat="server" Enabled="false" />
        </td>
        <th><%=this.GetLocalized("負責人/代表人 IDN") %>：</th>
        <td><asp:TextBox ID="tbxItem10" runat="server" MaxLength="10" Width="60%"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("簽約日期") %>：</th>
        <td>
            <asp:TextBox ID="tbxItem11" runat="server" MaxLength="7" Width="80"></asp:TextBox>
            <div style="font-size:10px;">(格式：民國年 YYYMMDD)</div>
        </td>
        <th><%=this.GetLocalized("終止契約日期") %>：</th>
        <td>
            <asp:TextBox ID="tbxItem14" runat="server" MaxLength="7" Width="80" AutoPostBack="true" OnTextChanged="tbxItem14_TextChanged"></asp:TextBox>
            <div style="font-size:10px;">(格式：民國年 YYYMMDD)</div>
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("終止契約種類代號") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem12" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlItem12_SelectedIndexChanged" />
        </td>
        <th><%=this.GetLocalized("終止契約原因代號") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem13" runat="server" />
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("終止契約後有應收未收取款項") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem15" runat="server" Enabled="false" />
        </td>
        <th><%=this.GetLocalized("終止契約後應收未收取款項金額") %>：</th>
        <td><asp:TextBox ID="tbxItem16" runat="server" MaxLength="10" Width="60%" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("登記名稱") %>：</th>
        <td colspan="3">
            <asp:TextBox ID="tbxItem17" runat="server" MaxLength="30" Width="75%"></asp:TextBox>
            <span style="font-size:10px;">(英文須全形)</span>
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("登記地址") %>：</th>
        <td colspan="3">
            <asp:TextBox ID="tbxItem18" runat="server" MaxLength="60" Width="75%"></asp:TextBox>
            <span style="font-size:10px;">(英文須全形)</span>
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("招牌名稱") %>：</th>
        <td colspan="3">
            <asp:TextBox ID="tbxItem19" runat="server" MaxLength="30" Width="75%"></asp:TextBox>
            <span style="font-size:10px;">(英文須全形)</span>
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("營業地址") %>：</th>
        <td colspan="3">
            <asp:TextBox ID="tbxItem20" runat="server" MaxLength="60" Width="75%"></asp:TextBox>
            <span style="font-size:10px;">(英文須全形)</span>
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("英文名稱") %>：</th>
        <td colspan="3">
            <asp:TextBox ID="tbxItem21" runat="server" MaxLength="30" Width="75%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("營業型態") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem23" runat="server" Enabled="false" />
        </td>
        <th><%=this.GetLocalized("資本額") %>：</th>
        <td><asp:TextBox ID="tbxItem24" runat="server" MaxLength="15" Width="50%"></asp:TextBox></td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("設立日期") %>：</th>
        <td>
            <asp:TextBox ID="tbxItem25" runat="server" MaxLength="7" Width="80"></asp:TextBox>
            <div style="font-size:10px;">(格式：民國年 YYYMMDD)</div>
        </td>
        <th><%=this.GetLocalized("業務行為") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem26" runat="server" Enabled="false" />
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("是否受理電子支付帳戶或儲值卡服務") %>：</th>
        <td colspan="3">
            <asp:CheckBoxList ID="cblItem27" runat="server" RepeatColumns="2" RepeatLayout="Flow" RepeatDirection="Horizontal" Enabled="false" />
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("是否受理信用卡服務") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem29" runat="server" Enabled="false" />
        </td>
        <th><%=this.GetLocalized("營業性質") %>：</th>
        <td>
            <asp:TextBox ID="tbxItem30" runat="server" MaxLength="4" Width="80" Enabled="false"></asp:TextBox>
            <div style="font-size:10px;">(有受理信用卡服務時請填報 MCC 碼)</div>
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("受理信用卡別名稱") %>：</th>
        <td colspan="3">
            <asp:CheckBoxList ID="cblItem31" runat="server" RepeatColumns="2" RepeatLayout="Flow" RepeatDirection="Horizontal" Enabled="false" />
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("是否有銷售遞延性商品或服務") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem33" runat="server" Enabled="false" />
        </td>
        <th><%=this.GetLocalized("是否安裝端末設備") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem34" runat="server" Enabled="false" />
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("是否安裝錄影設備") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem35" runat="server" Enabled="false" />
        </td>
        <th><%=this.GetLocalized("連鎖店加盟或直營") %>：</th>
        <td>
            <asp:DropDownList ID="ddlItem36" runat="server" Enabled="false" />
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("保證人1 IDN/BAN") %>：</th>
        <td>
            <asp:TextBox ID="tbxItem37" runat="server" MaxLength="10" Width="50%"></asp:TextBox>
        </td>
        <th><%=this.GetLocalized("保證人2 IDN/BAN") %>：</th>
        <td>
            <asp:TextBox ID="tbxItem38" runat="server" MaxLength="10" Width="50%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th><%=this.GetLocalized("資料更新日期") %>：</th>
        <td>
            <asp:Label ID="lblItem40" runat="server" ></asp:Label>
            <span style="font-size:10px;">(格式：民國年 YYYMMDD)</span>
        </td>
        <th><%=this.GetLocalized("狀態") %>：</th>
        <td>
            <asp:Label ID="lblStatus" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr id="trFeedback" runat="server">
        <th><%=this.GetLocalized("上次回饋結果") %>：</th>
        <td colspan="3">
            <asp:Label ID="lblFeedback" runat="server" ></asp:Label>
        </td>
    </tr>
    </table>
    <div class="button">
        <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click"></cc:MyOKButton>
        <cc:MyGoBackButton ID="ccbtnGoBack" runat="server" OnClick="ccbtnGoBack_Click"></cc:MyGoBackButton>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $("#<%=this.tbxItem17.ClientID%>").change(function () {
            debugger;
            var item17 = $(this).val().trim();
            var jqItem19 = $("#<%=this.tbxItem19.ClientID%>");
            var item19 = jqItem19.val().trim();
            if (item19 == "" && item17 != "") {
                jqItem19.val(item17);
            }
        });

        $("#<%=this.tbxItem18.ClientID%>").change(function () {
            debugger;
            var item18 = $(this).val().trim();
            var jqItem20 = $("#<%=this.tbxItem20.ClientID%>");
            var item20 = jqItem20.val().trim();
            if (item20 == "" && item18 != "") {
                jqItem20.val(item18);
            }
        });
    });
</script>
</asp:Content>
