if (!String.prototype.trim) {
    /*---------------------------------------
    * 清除字串兩端空格，包含換行符、製表符
    *---------------------------------------*/
    String.prototype.trim = function () {
        return this.replace(/(^[\s\n\t] |[\s\n\t] $)/g, "");
    }
}