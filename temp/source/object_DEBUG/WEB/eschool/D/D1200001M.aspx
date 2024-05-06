<%@ Page Title="土地銀行 - 代收學雜費服務網 - 代收費用檔" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="D1200001M.aspx.cs" Inherits="eSchoolWeb.D.D1200001M" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style type="text/css">
    .hide {
        display: none;
    }
</style>
<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
    if (self === top) {
        document.documentElement.style.display = 'block';
    }
</script>
<asp:PlaceHolder ID="phdStyle" runat="server" Visible="false">
<style type="text/css">
    span.spnCht {
        display: initial;
    }
</style>
</asp:PlaceHolder>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc:Filter1 ID="ucFilter1" runat="server" UIMode="Label" AutoGetDataBound="false" Filter2ControlID="ucFilter2" />
<uc:Filter2 ID="ucFilter2" runat="server" UIMode="Label" AutoGetDataBound="false" />
<table id="condition" class="condition" summary="表格_修改" width="100%">
<tr>
    <th style="width:30%"><cc:MyLabel ID="cclabBillFormId" runat="server" LocationText="繳費單模板"></cc:MyLabel>：</th>
    <td colspan="3">
        <div><span class="spnCht hide">中：</span><asp:DropDownList ID="ddlBillFormChtId" runat="server"></asp:DropDownList></div>
        <asp:PlaceHolder ID="phdBillFormEId" runat="server" Visible="false">
        <div><span>英：</span><asp:DropDownList ID="ddlBillFormEngId" runat="server"></asp:DropDownList></div>
        </asp:PlaceHolder>
    </td>
</tr>
<tr>
    <th><cc:MyLabel ID="cclabInvoiceFormId" runat="server" LocationText="收據模板"></cc:MyLabel>：</th>
    <td colspan="3">
        <div><span class="spnCht hide">中：</span><asp:DropDownList ID="ddlInvoiceFormChtId" runat="server"></asp:DropDownList></div>
        <asp:PlaceHolder ID="phdInvoiceFormEId" runat="server" Visible="false">
        <div><span>英：</span><asp:DropDownList ID="ddlInvoiceFormEngId" runat="server"></asp:DropDownList></div>
        </asp:PlaceHolder>
    </td>
</tr>
<tr>
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("字軌") %>：</th>
    <td colspan="3">
        <asp:TextBox ID="tbxSchWord" runat="server" MaxLength="40" Width="80%"></asp:TextBox>
    </td>
</tr>
<tr>
    <th ><%=this.GetHtmlEncodeLocalized("繳款期限") %>：</th>
    <td colspan="3">
        <asp:TextBox ID="tbxPayDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>&nbsp;&nbsp;
        <%=this.GetHtmlEncodeLocalized("超商可延遲日") %>：<asp:DropDownList ID="ddlExtraDays" runat="server"></asp:DropDownList>
        <br/><%=this.GetHtmlEncodeLocalized("繳費後銷帳時間需3-5營業日,請謹慎設定!") %>
    </td>
</tr>
<tr>
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("中信繳費平台繳費期限") %>：</th>
    <td style="width:20%">
        <asp:TextBox ID="tbxPayDueDate2" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("財金繳費期限") %>：</th>
    <td style="width:20%">
        <asp:TextBox ID="tbxPayDueDate3" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
</tr>
<tr>
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("開放列印日期") %>：</th>
    <td style="width:20%"><asp:TextBox ID="tbxBillOpenDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox></td>
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("關閉列印日期") %>：</th>
    <td style="width:20%" id="tdBillCloseDate">
        <asp:CheckBox ID="cbxBillCloseDate" runat="server" />啟用<br/>
        <asp:TextBox ID="tbxBillCloseDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
</tr>
<tr>
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("列印收據關閉日") %>：</th>
    <td colspan="3">
        <asp:TextBox ID="tbxInvoiceCloseDate" runat="server" CssClass="datepicker" MaxLength="10" Width="100px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td colspan="4">
        <table class="#" width="100%">
        <tr align="center">
            <td><%=this.GetHtmlEncodeLocalized("機關長官") %></td>
            <td><%=this.GetHtmlEncodeLocalized("姓名") %></td>
            <td><%=this.GetHtmlEncodeLocalized("主辦會計") %></td>
            <td><%=this.GetHtmlEncodeLocalized("姓名") %></td>
            <td><%=this.GetHtmlEncodeLocalized("主辦出納") %></td>
            <td><%=this.GetHtmlEncodeLocalized("姓名") %></td>
        </tr>
        <tr align="center">
            <td><asp:TextBox ID="tbxATitle1" runat="server" MaxLength="6" Width="80px"></asp:TextBox></td>
            <td><asp:TextBox ID="tbxAName1" runat="server" MaxLength="6" Width="80px"></asp:TextBox></td>
            <td><asp:TextBox ID="tbxATitle2" runat="server" MaxLength="6" Width="80px"></asp:TextBox></td>
            <td><asp:TextBox ID="tbxAName2" runat="server" MaxLength="6" Width="80px"></asp:TextBox></td>
            <td><asp:TextBox ID="tbxATitle3" runat="server" MaxLength="6" Width="80px"></asp:TextBox></td>
            <td><asp:TextBox ID="tbxAName3" runat="server" MaxLength="6" Width="80px"></asp:TextBox></td>
        </tr>
        </table>
    </td>
</tr>
<tr style="display:none;">
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("代收費用型態") %>：</th>
    <td colspan="3">
        <asp:DropDownList ID="ddlReceiveStatus" runat="server" Enabled="false">
            <asp:ListItem Text ="已有繳費者資料之收費" Value="1" Selected="True" />
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("學生學分費計算方式") %>：</th>
    <td colspan="3">
        <asp:DropDownList ID="ddlStudentType" runat="server">
            <asp:ListItem Text ="此業務別碼無學分費之收入科目" Value="1" />
            <asp:ListItem Text ="以學分數計算" Value="2" />
            <asp:ListItem Text ="以上課時數計算" Value="3" />
            <asp:ListItem Text ="以小於某學分數才收學分費" Value="4" />
            <asp:ListItem Text ="以小於某上課時數才以上課時數收學分費" Value="5" />
            <asp:ListItem Text ="以小於某學分數才以上課時數收學分費" Value="6" />
        </asp:DropDownList>
    </td>
</tr>
<tr >
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("學分費比較基準") %>：</th>
    <td colspan="3"><asp:TextBox ID="tbxCreditBasic" runat="server" MaxLength="3" Width="80px"></asp:TextBox></td>
</tr>
<tr>
    <th style="width:30%"><%=this.GetHtmlEncodeLocalized("啟用大專院校") %><br/><%=this.GetHtmlEncodeLocalized("學雜費電子化申報") %>：</th>
    <td colspan="3">
        <asp:CheckBox ID="cbxEnabledTax" runat="server" /><%=this.GetHtmlEncodeLocalized("是") %>
    </td>
</tr>
<tr>
    <td colspan="4">
        <table width="100%" class="#">
        <tr>
            <th colspan="5"><div align="center" style="color:red">注意：勾選【教育部補助】的收入科目，計算金額時該科目的金額會清為 0</div></th>
        </tr>
        <tr>
            <td style="text-align:center">No.</td>
            <td style="text-align:center"><%=this.GetHtmlEncodeLocalized("收入科目名稱") %></td>
            <td style="text-align:center"><%=this.GetHtmlEncodeLocalized("就貸") %></td>
            <td style="text-align:center"><%=this.GetHtmlEncodeLocalized("教育部補助") %></td>
            <td style="text-align:center"><%=this.GetHtmlEncodeLocalized("申報學雜費") %></td>
            <td style="text-align:center"><%=this.GetHtmlEncodeLocalized("申報住宿費") %></td>
        </tr>
        <tr>
            <td>01</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht01" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng01" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng01" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem01" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy01" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax01" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax01" runat="server" /></td>
        </tr>
        <tr>
            <td>02</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht02" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng02" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng02" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem02" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy02" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax02" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax02" runat="server" /></td>
        </tr>
        <tr>
            <td>03</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht03" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng03" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng03" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem03" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy03" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax03" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax03" runat="server" /></td>
        </tr>
        <tr>
            <td>04</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht04" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng04" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng04" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem04" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy04" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax04" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax04" runat="server" /></td>
        </tr>
        <tr>
            <td>05</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht05" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng05" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng05" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem05" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy05" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax05" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax05" runat="server" /></td>
        </tr>
        <tr>
            <td>06</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht06" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng06" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng06" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem06" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy06" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax06" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax06" runat="server" /></td>
        </tr>
        <tr>
            <td>07</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht07" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng07" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng07" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem07" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy07" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax07" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax07" runat="server" /></td>
        </tr>
        <tr>
            <td>08</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht08" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng08" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng08" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem08" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy08" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax08" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax08" runat="server" /></td>
        </tr>
        <tr>
            <td>09</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht09" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng09" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng09" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem09" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy09" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax09" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax09" runat="server" /></td>
        </tr>
        <tr>
            <td>10</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht10" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng10" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng10" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem10" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy10" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax10" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax10" runat="server" /></td>
        </tr>
        <tr>
            <td>11</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht11" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng11" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng11" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem11" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy11" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax11" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax11" runat="server" /></td>
        </tr>
        <tr>
            <td>12</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht12" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng12" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng12" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem12" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy12" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax12" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax12" runat="server" /></td>
        </tr>
        <tr>
            <td>13</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht13" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng13" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng13" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem13" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy13" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax13" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax13" runat="server" /></td>
        </tr>
        <tr>
            <td>14</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht14" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng14" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng14" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem14" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy14" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax14" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax14" runat="server" /></td>
        </tr>
        <tr>
            <td>15</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht15" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng15" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng15" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem15" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy15" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax15" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax15" runat="server" /></td>
        </tr>
        <tr>
            <td>16</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht16" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng16" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng16" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem16" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy16" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax16" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax16" runat="server" /></td>
        </tr>
        <tr>
            <td>17</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht17" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng17" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng17" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem17" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy17" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax17" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax17" runat="server" /></td>
        </tr>
        <tr>
            <td>18</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht18" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng18" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng18" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem18" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy18" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax18" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax18" runat="server" /></td>
        </tr>
        <tr>
            <td>19</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht19" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng19" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng19" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem19" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy19" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax19" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax19" runat="server" /></td>
        </tr>
        <tr>
            <td>20</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht20" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng20" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng20" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem20" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy20" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax20" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax20" runat="server" /></td>
        </tr>
        <tr>
            <td>21</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht21" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng21" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng21" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem21" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy21" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax21" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax21" runat="server" /></td>
        </tr>
        <tr>
            <td>22</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht22" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng22" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng22" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem22" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy22" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax22" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax22" runat="server" /></td>
        </tr>
        <tr>
            <td>23</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht23" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng23" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng23" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem23" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy23" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax23" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax23" runat="server" /></td>
        </tr>
        <tr>
            <td>24</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht24" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng24" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng24" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem24" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy24" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax24" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax24" runat="server" /></td>
        </tr>
        <tr>
            <td>25</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht25" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng25" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng25" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem25" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy25" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax25" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax25" runat="server" /></td>
        </tr>
        <tr>
            <td>26</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht26" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng26" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng26" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem26" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy26" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax26" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax26" runat="server" /></td>
        </tr>
        <tr>
            <td>27</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht27" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng27" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng27" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem27" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy27" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax27" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax27" runat="server" /></td>
        </tr>
        <tr>
            <td>28</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht28" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng28" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng28" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem28" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy28" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax28" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax28" runat="server" /></td>
        </tr>
        <tr>
            <td>29</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht29" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng29" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng29" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem29" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy29" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax29" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax29" runat="server" /></td>
        </tr>
        <tr>
            <td>30</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht30" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng30" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng30" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem30" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy30" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax30" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax30" runat="server" /></td>
        </tr>
        <tr>
            <td>31</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht31" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng31" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng31" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem31" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy31" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax31" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax31" runat="server" /></td>
        </tr>
        <tr>
            <td>32</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht32" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng32" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng32" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem32" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy32" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax32" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax32" runat="server" /></td>
        </tr>
        <tr>
            <td>33</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht33" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng33" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng33" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem33" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy33" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax33" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax33" runat="server" /></td>
        </tr>
        <tr>
            <td>34</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht34" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng34" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng34" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem34" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy34" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax34" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax34" runat="server" /></td>
        </tr>
        <tr>
            <td>35</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht35" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng35" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng35" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem35" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy35" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax35" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax35" runat="server" /></td>
        </tr>
        <tr>
            <td>36</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht36" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng36" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng36" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem36" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy36" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax36" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax36" runat="server" /></td>
        </tr>
        <tr>
            <td>37</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht37" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng37" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng37" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem37" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy37" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax37" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax37" runat="server" /></td>
        </tr>
        <tr>
            <td>38</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht38" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng38" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng38" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem38" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy38" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax38" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax38" runat="server" /></td>
        </tr>
        <tr>
            <td>39</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht39" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng39" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng39" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem39" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy39" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax39" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax39" runat="server" /></td>
        </tr>
        <tr>
            <td>40</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxReceiveItemCht40" runat="server" MaxLength="40" Width="260px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdReceiveItemEng40" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxReceiveItemEng40" runat="server" MaxLength="140" Width="260px" ></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:center"><asp:CheckBox ID="cbxLoanItem40" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxIsSubsidy40" runat="server" CssClass="cbxSubsidy" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxEduTax40" runat="server" /></td>
            <td style="text-align:center"><asp:CheckBox ID="cbxStayTax40" runat="server" /></td>
        </tr>
        </table>
    </td>
</tr>
<tr>
    <td colspan="4">
        <table width="100%" class="#">
        <tr>
            <td style="text-align:center; width:10%"><%=this.GetHtmlEncodeLocalized("備註項目") %></td>
            <td style="text-align:center; width:40%"><%=this.GetHtmlEncodeLocalized("備註標題") %></td>
            <td style="text-align:center; width:10%"><%=this.GetHtmlEncodeLocalized("備註項目") %></td>
            <td style="text-align:center; width:40%"><%=this.GetHtmlEncodeLocalized("備註標題") %></td>
        </tr>
        <tr>
            <td style="text-align:right;">01</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht01" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng01" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng01" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">02</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht02" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng02" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng02" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">03</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht03" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng03" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng03" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">04</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht04" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng04" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng04" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">05</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht05" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng05" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng05" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">06</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht06" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng06" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng06" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">07</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht07" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng07" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng07" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">08</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht08" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng08" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng08" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">09</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht09" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng09" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng09" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">10</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht10" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng10" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng10" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">11</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht11" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng11" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng11" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">12</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht12" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng12" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng12" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">13</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht13" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng13" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng13" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">14</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht14" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng14" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng14" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">15</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht15" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng15" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng15" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">16</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht16" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng16" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng16" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">17</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht17" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng17" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng17" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">18</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht18" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng18" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng18" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">19</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht19" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng19" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng19" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td style="text-align:right;">20</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht20" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng20" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng20" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">21</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxMemoTitleCht21" runat="server" MaxLength="40" Width="200px"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdMemoTitleEng21" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxMemoTitleEng21" runat="server" MaxLength="140" Width="200px"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        </table>
    </td>
</tr>
<tr>
    <th colspan="4">
        <div align="center"><%=this.GetHtmlEncodeLocalized("課程或學分基準所屬收入科目") %>：<asp:TextBox ID="tbxCreditItem" runat="server" MaxLength="2" Width="100px"></asp:TextBox></div>
    </th>
</tr>
<tr>
    <th colspan="4">
        <div align="center"><%=this.GetHtmlEncodeLocalized("整批退費所屬收入科目") %>：<asp:TextBox ID="tbxReturnItem" runat="server" MaxLength="2" Width="100px"></asp:TextBox></div>
    </th>
</tr>
<tr>
    <th colspan="4">
        <div align="center"><%=this.GetHtmlEncodeLocalized("除收入科目外就學貸款可貸之額外固定金額") %>：<asp:TextBox ID="tbxLoanFee" runat="server" MaxLength="8" Width="100px"></asp:TextBox></div>
    </th>
</tr>
<tr>
    <th colspan="4">
        <div align="center"><%=this.GetHtmlEncodeLocalized("依收費標準計算時，就貸可貸金額計算依據") %> <asp:RadioButton ID="rbtnFlagRL0" runat="server" GroupName="FlagRL" /><%=this.GetHtmlEncodeLocalized("減免前") %>&nbsp;<%=this.GetHtmlEncodeLocalized("或") %>&nbsp;<asp:RadioButton ID="rbtnFlagRL1" runat="server" GroupName="FlagRL" Checked="True" /><%=this.GetHtmlEncodeLocalized("減免後") %>&nbsp;<%=this.GetHtmlEncodeLocalized("繳費金額計算") %></div>
    </th>
</tr>
<tr>
    <th colspan="4">
        <div align="center"><%=this.GetHtmlEncodeLocalized("繳費單上就學貸款可貸金額列印欄位") %> <asp:RadioButton ID="rbtnLoanQual1" runat="server" GroupName="LoanQual" Checked="True" /><%=this.GetHtmlEncodeLocalized("可貸金額") %>&nbsp;<%=this.GetHtmlEncodeLocalized("或") %>&nbsp;<asp:RadioButton ID="rbtnLoanQual2" runat="server" GroupName="LoanQual" /><%=this.GetHtmlEncodeLocalized("就貸金額") %></div>
    </th>
</tr>
<tr>
    <th>
        備註：<br/><br />
        <div align="left">
        ＊備註欄內資料（文數字）請打全形。<br/>
        </div>
    </th>
    <td colspan="3">
        <asp:TextBox ID="tbxReceiveMemo" runat="server" TextMode="MultiLine" Rows="9" Columns="60" Wrap="false" ></asp:TextBox>
        <div style="padding-top:5px;"><label for="<%=this.cbxReceiveMemoNoWrap.ClientID %>"><asp:CheckBox ID="cbxReceiveMemoNoWrap" runat="server" /> 備註不自動換行</label></div>
    </td>
</tr>
<tr>
    <th colspan="4"><div align="center"><%=this.GetHtmlEncodeLocalized("繳費單(通訊聯)注意事項設定") %>：</div></th>
</tr>
<tr>
    <td colspan="4">
        <table width="100%" class="#">
        <tr>
            <td style="text-align:right; width:25%"><%=this.GetHtmlEncodeLocalized("注意事項編號") %></td>
            <td style="text-align:center; width:75%"><%=this.GetHtmlEncodeLocalized("注意事項內容") %></td>
        </tr>
        <tr>
            <td style="text-align:right;">01</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxBriefCht1" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdBriefEng1" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxBriefEng1" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">02</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxBriefCht2" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdBriefEng2" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxBriefEng2" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">03</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxBriefCht3" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdBriefEng3" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxBriefEng3" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">04</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxBriefCht4" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdBriefEng4" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxBriefEng4" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">05</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxBriefCht5" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdBriefEng5" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxBriefEng5" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">06</td>
            <td>
                <div><span class="spnCht hide">中：</span><asp:TextBox ID="tbxBriefCht6" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                <asp:PlaceHolder ID="phdBriefEng6" runat="server" Visible="false">
                <div><span>英：</span><asp:TextBox ID="tbxBriefEng6" runat="server" MaxLength="500" width="80%"></asp:TextBox></div>
                </asp:PlaceHolder>
            </td>
        </tr>
        </table>
    </td>
</tr>
</table>

<div class="button">
    <cc:MyLinkButton ID="ccbtnPreview" runat="server" LocationText="預覽" OnClick="ccbtnPreview_Click" Visible="false"></cc:MyLinkButton>
    <cc:MyOKButton ID="ccbtnOK" runat="server" OnClick="ccbtnOK_Click" OnClientClick="checkInputData();"></cc:MyOKButton>&nbsp;&nbsp;&nbsp;&nbsp;
    <cc:MyGoBackButton ID="ccbtnGoBack" runat="server"></cc:MyGoBackButton>
</div>

<script type="text/javascript" language="javascript">
    $(function () {
        if ($('#tdBillCloseDate').find('input:checkbox').prop("checked")) {
            $('#tdBillCloseDate').find('input:text').show();
        }
        else {
            $('#tdBillCloseDate').find('input:text').hide();
        }

        $('#tdBillCloseDate').find('input:checkbox').click(function() {
            if ($(this).prop("checked")) {
                $('#tdBillCloseDate').find('input:text').show();
            } else {
                $('#tdBillCloseDate').find('input:text').hide();
            }
        });
    });

    function checkInputData() {
        if ($('.cbxSubsidy > input[type=checkbox]:checked').length > 0) {
            alert("注意：\r勾選【教育部補助】的收入科目，\r計算金額時該科目的金額會清為 0");
        }
        return true;
    }
</script>
</asp:Content>
