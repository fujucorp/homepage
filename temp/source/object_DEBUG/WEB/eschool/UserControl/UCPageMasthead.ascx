<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPageMasthead.ascx.cs" Inherits="eSchoolWeb.UCPageMasthead" %>

<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
    if (self === top) {
        document.documentElement.style.display = 'block';
    }
</script>

<!--//loginID、查詢日期、歷史路徑----------->        
<div class="history" id="divMenuHistory" runat="server">基本資料 ／ 維護代碼檔</div>
<div class="loginID" id="divLogonUser" runat="server"><span>國立台灣藝術學院</span>／登入帳號：7777777</div>
<div class="date" id="divNow" runat="server">查詢日期：2010/11/24</div>
                
                
<!--begin of masthead 刊頭------------------>
<div class='masthead' id="divMastHead" runat="server">    		
  <p><asp:Label ID="labMenuName" runat="server" Text=""></asp:Label></p>
  <div class="function"><a class="printer" href="javascript:void(0)" onclick="window.open('<%= this.GetResolveUrl("~/PrintBox.aspx") %>?P=.pbox', 'pbox'); return false;" title="友善列印"><span></span></a></div>
</div>
<!--end of masthead 刊頭-->