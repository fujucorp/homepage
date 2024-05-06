<%@ Page Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="S5600015.aspx.cs" Inherits="eSchoolWeb.S.S5600015" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="result" class="result" summary="查詢結果" width="100%">
                <tr>
                    <th colspan="2" style="background-color:#996633; color:#FFFFFF; text-align:center;"><%=this.GetLocalized("支付寶系統參數設定") %></th>
                </tr>
                <tr>
                    <th width="20%">
                        <cc:MyLabel ID="cclabInboundURL" runat="server" LocationText="財金網址"></cc:MyLabel>：
                    </th>
                    <td>
                        <asp:TextBox ID="txtInboundURL" runat="server"  Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <cc:MyLabel ID="cclabmerchantId" runat="server" LocationText="特店代號"></cc:MyLabel>：
                    </th>
                    <td>
                        <asp:TextBox ID="txtmerchantId" runat="server"  Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <cc:MyLabel ID="cclabterminalId" runat="server" LocationText="端末代號"></cc:MyLabel>：
                    </th>
                    <td>
                        <asp:TextBox ID="txtterminalId" runat="server"  Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <cc:MyLabel ID="cclabInitKey" runat="server" LocationText="初始key"></cc:MyLabel>：
                    </th>
                    <td>
                        <asp:TextBox ID="txtInitKey" runat="server"  Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <cc:MyLabel ID="cclabKey" runat="server" LocationText="目前的Key"></cc:MyLabel>：
                    </th>
                    <td>
                        <asp:TextBox ID="txtKey" runat="server"  Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <cc:MyLabel ID="cclabCharge" runat="server" LocationText="手續費率"></cc:MyLabel>：
                    </th>
                    <td>
                        <asp:TextBox ID="txtCharge" runat="server"  Width="50px"></asp:TextBox>％
                    </td>
                </tr>
                <tr>
                    <th>
                        <cc:MyLabel ID="cclabAuthResURL" runat="server" LocationText="回傳網址"></cc:MyLabel>：
                    </th>
                    <td>
                        <asp:TextBox ID="txtAuthResURL" runat="server" Width="90%" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th colspan="2" style="background-color:#996633; color:#FFFFFF; text-align:center;"><%=this.GetLocalized("授權商家代號設定") %></th>
                </tr>
                <tr>
                    <th>
                        <cc:MyLabel ID="cclabAuthReceiveType" runat="server" LocationText="商家代號"></cc:MyLabel>：</th>
                    <td>
                        <div>
                            授權商家代號清單：<br />
                            <asp:TextBox ID="txtAuthReceiveType" runat="server" Width="90%" TextMode="MultiLine" Rows="7" Wrap="false"></asp:TextBox>
                        </div>
                        <div>
                            特殊商家代號清單 (開放可繳4000元以下繳費單)：<br />
                            <asp:TextBox ID="txtSpecialReceiveType" runat="server" Width="90%" TextMode="MultiLine" Rows="7" Wrap="false"></asp:TextBox>
                        </div>
                        <div style="padding-top:5px;">
                        注意：<br />
                        1. 系統會自動去除重複商家代號、無意義空白字元。<br />
                        2. 系統會自動重新由小到大排序，且每行10筆資料。<br />
                        3. 【特殊商家代號清單】的商家代號必須同時是【授權商家代號清單】的商家代號。<br />
                        4. 系統不會檢查設定的商家代號是否存在，請自行確認。<br />
                        5. 如果不指定【授權商家代號清單】則首頁的『支付寶繳費』功能項不會顯示。<br />
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="button">
        <cc:MyLinkButton ID="ccbtnOK1" runat="server" LocationText="儲存支付寶系統參數設定" OnClick="ccbtnOK1_Click"></cc:MyLinkButton>
        <cc:MyLinkButton ID="ccbtnOK2" runat="server" LocationText="儲存授權商家代號設定" OnClick="ccbtnOK2_Click"></cc:MyLinkButton>
        <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
    </div>
</asp:Content>
