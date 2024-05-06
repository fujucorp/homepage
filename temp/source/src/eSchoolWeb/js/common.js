if (!String.prototype.trim) {
    /*---------------------------------------
    * 清除字串兩端空格，包含換行符、製表符
    *---------------------------------------*/
    String.prototype.trim = function () {
        return this.replace(/^\s*|\s*$/g, "");
    }
}
/*---------------------------------------
* Checkmarx 見不得 trim 字眼所以增加這個方法
* 清除字串兩端空格，包含換行符、製表符
*---------------------------------------*/
String.prototype.prune = function () {
    return this.replace(/^\s*|\s*$/g, "");
}