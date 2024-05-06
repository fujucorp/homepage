<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPageHeader.ascx.cs" Inherits="eSchoolWeb.UCPageHeader" %>
<style type="text/css">
html { display: none; }
</style>
<script type="text/javascript" language="javascript">
    if (self === top) {
        document.documentElement.style.display = 'block';
    }
</script>
<div id="header">
    <div id="logo"><a href="javascript:void(0)"><asp:Image ID="imgLogo" runat="server" ImageUrl="/img/logo.gif"  ToolTip="logo" /></a></div>
    <div id="webTitle"><a href="javascript:void(0)"><cc:MyLiteral ID="litWebTitle" runat="server" ResourceKey="WebTitle" Text="代收學雜費服務網" /></a></div>
    <div id='logOut'><a href="javascript:void(0)" onclick="javascript:logout();"><%=this.GetLocalized("登出") %></a></div>
    <div class='countDown'><a href="javascript:void(0)" onclick="reCountDown();"><asp:Label ID="Label1" runat="server" Text="重新計時"></asp:Label></a><%=this.GetLocalized("剩餘") %><span id="spnMinutes">25</span><%=this.GetLocalized("分") %><span id="spnSeconds">7</span>><%=this.GetLocalized("秒") %></div>
    <!-- 
    <div id="channel">
        <ul>
            <li>您的操作時間尚有 <span id="spnMinutes">19</span> 分 <span id="spnSeconds">48</span> 秒。</li>
            <li>&nbsp;<a href="javascript:void(0)" onclick="reCountDown();"><asp:Label ID="labReCountDown" runat="server" Text="重新計時"></asp:Label></a>&nbsp;</li>
            <li>&nbsp;<a href="javascript:void(0)" onclick="logout();"><asp:Label ID="labLogout" runat="server" Text="登出"></asp:Label></a>&nbsp;</li>
        </ul>
    </div>
    -->
</div>
<script type="text/javascript" language="javascript">
    function countDownTimer(ts, as) {
        this.timeoutSeconds = ts;       //操作逾時的倒數秒數
        this.remindSeconds = as;        //顯示提醒的剩餘秒數

        this.startSeconds = 0;
        this.timeoutId = null;
        this.reminded = false;

        //顯示倒數訊息的方法
        this.showCountDownMessage = function (seconds) {
            if (seconds <= 0) {
                var divCountDown = document.getElementById("divCountDown");
                if (divCountDown) {
                    divCountDown.innerHTML = "操作逾期";
                }
            }
            else {
                var d = new Date(seconds * 1000);
                var spnMinutes = document.getElementById("spnMinutes");
                var spnSeconds = document.getElementById("spnSeconds");
                if (spnMinutes) {
                    var min = "0" + d.getMinutes();
                    spnMinutes.innerHTML = min.substr(min.length - 2);
                }
                if (spnSeconds) {
                    var sec = "0" + d.getSeconds();
                    spnSeconds.innerHTML = sec.substr(sec.length - 2);
                }
            }
        };

        //顯示提醒訊息的方法
        this.showRemindMessage = function () {
            var d = new Date(this.remindSeconds * 1000);
            var min = "0" + d.getMinutes();
            var sec = "0" + d.getSeconds();
            alert("您還有" + min.substr(min.length - 2) + "分" + sec.substr(sec.length - 2) + "秒 操作時間。");
        }

        //倒數逾時的處理方法
        this.runTimeoutHandler = function () {
            logout(true);
            //alert("操作逾期");
        }

        this.countDown = function (fgStart) {
            clearTimeout(this.timeoutId);
            var nowSeconds = (new Date() / 1000);
            var seconds = this.timeoutSeconds - (nowSeconds - this.startSeconds);
            this.showCountDownMessage(seconds);
            if (seconds > 0) {
                if (seconds <= this.remindSeconds) {
                    if (!this.reminded && this.timeoutSeconds > this.remindSeconds) {
                        this.reminded = true;
                        this.showRemindMessage();
                    }
                }
                var ts = fgStart ? 900 : 1000;
                var me = this;
                this.timeoutId = setTimeout(function () { me.countDown() }, ts);
            }
            else {
                this.runTimeoutHandler();
            }
        };

        this.start = function () {
            clearTimeout(this.timeoutId);
            if (this.timeoutSeconds > 0) {
                this.reminded = false;
                this.startSeconds = (new Date() / 1000);
                this.countDown(true);
            }
            else {
                var divCountDown = document.getElementById("divCountDown");
                if (divCountDown) {
                    divCountDown.style.display = "none";
                }
            }
        };

        this.pause = function () {
            clearTimeout(this.timeoutId);
        }

        this.proceed = function () {
            this.countDown();
        }
    }

    var timeoutSeconds = parseInt("<%= this.GetPageOperatingTimeoutSeconds() %>");
    var remindSeconds = parseInt("<%= this.GetPageOperatingRemindSeconds() %>");
    var timer = new countDownTimer(timeoutSeconds, remindSeconds);
    timer.start();

    function reCountDown() {
        timer.pause();
        $.ajax({
            url: '<%= this.GetCheckLogonApiUrl() %>',
            type: 'POST',
            datatype: 'json',
            cache: false,
            error: function (xhr) {
                timer.proceed();
                alert('檢查登入者狀態失敗');
            },
            success: function (data) {
                if (data.status == 'online') {
                    timer.start();
                    //alert('已重新計時');
                }
                else {
                    alert('已被系統登出');
                    top.window.location.replace('<%= this.GetLogoutUrl() %>');
                }
            }
        });
    }

    function logout(timeout) {
        if (timeout) {
            top.window.location.replace('<%= this.GetLogoutUrl() %>' + '?ec=1');
        }
        else {
            top.window.location.replace('<%= this.GetLogoutUrl() %>');
        }
        //top.window.location.replace('<%= this.GetLogoutUrl() %>');
        //window.location.replace('<! % = LogoutPage % >');
    }
</script>
