<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEntryPageMenu.ascx.cs" Inherits="eSchoolWeb.UCEntryPageMenu" %>
<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
    if (self === top) {
        document.documentElement.style.display = 'block';
    }
</script>
<ul>
  <li><a href="javascript:void(0);" class="school" onclick="window.location.replace('school_login.aspx')"><span><%= GetLocalized("學校專區") %><!--學校專區--></span></a></li>
  <li><a href="javascript:void(0);" class="student" onclick="window.location.replace('student_login.aspx')"><span><%= GetLocalized("學生專區") %><!--學生專區--></span></a></li>
  <li><a href="javascript:void(0);" class="bank" onclick="window.location.replace('bank_login.aspx')"><span><%= GetLocalized("銀行專區") %><!--銀行專區--></span></a></li>
  <li><a href="javascript:void(0);" class="creditCard" onclick="window.location.replace('creditcard.aspx')"><span><%= GetLocalized("信用卡繳費") %><!--信用卡繳費--></span></a></li>
  <li><a href="javascript:void(0);" class="unionPay" onclick="window.open('https://www.i-payment.com.tw', '_unionPay')"><span><%= GetLocalized("銀聯卡繳費") %><!--銀聯卡繳費--></span></a></li>
  <li id="liAlipay" runat="server" ><a href="javascript:void(0);" class="alipay" onclick="window.location.replace('alipay.aspx')"><span><%= GetLocalized("支付寶繳費") %><!--支付寶繳費--></span></a></li>
  <li><a href="javascript:void(0);" class="creditCard" onclick="window.location.replace('creditcard2.aspx')"><span><%= GetLocalized("國際信用卡繳費") %><!--國際信用卡繳費--></span></a></li>
  <li><a href="javascript:void(0);" class="checkPayment" onclick="window.location.replace('check_bill_status.aspx')"><span><%= GetLocalized("查詢繳費狀態") %><!--查詢繳費狀態--></span></a></li>
  <li><a href="javascript:void(0);" class="checkReceipt" onclick="window.location.replace('print_bill.aspx')"><span><%= GetLocalized("查詢列印繳費單") %><!--查詢列印繳費單--></span></a></li>
  <li><a href="javascript:void(0);" class="receipt" onclick="window.location.replace('print_receipt.aspx')"><span><%= GetLocalized("列印收據") %><!--列印收據--></span></a></li>
  <li><a href="javascript:void(0);" class="download" onclick="window.location.replace('file_download.aspx')"><span><%= GetLocalized("檔案下載") %><!--檔案下載--></span></a></li>
  <li><a href="javascript:void(0);" class="contact" onclick="window.location.replace('contact.aspx')"><span><%= GetLocalized("聯絡我們") %><!--聯絡我們--></span></a></li>
  <li><a href="javascript:void(0);" class="qa" onclick="window.location.replace('QandA.aspx')"><span><%= GetLocalized("Ｑ＆Ａ") %><!--Ｑ＆Ａ--></span></a></li>
</ul>
