var rootPath = '';
window.BaseURL = rootPath;
var ajaxCall = {
    PostForListPage: function (that, url, request) {
        that.loading = true;
        $.ajax({
            type: 'post',
            url: rootPath + url,
            data: request,
            success: function (resultData) {
                console.log(resultData);
                that.loading = false;
                if (resultData == undefined) {
                    that.alert.Error("服务器无响应");
                    return;
                }
                if (resultData.ResultCode == 500) {
                    that.alert.Error("服务器开小差了...");
                    return;
                }
                if (resultData.ResultCode != 1) {
                    let msg = resultData.ResultMsg;
                    msg = msg < "" ? "服务器发生未知错误,代码：" + resultData.ResultCode : msg;
                    that.alert.Error(msg);
                    return;
                }
                if (resultData.Body == undefined) {
                    that.totalRecord = 0;
                    that.gridData = [];
                    return;
                }
                that.totalCount = resultData.Body.TotalCount;
                that.tableData = resultData.Body.List;
            }
        });
    },
    Get: function (that, url, data, callBack, failedCallBack, obj) {
        that.loading = true;
        $.ajax({
            type: 'Get',
            url: rootPath + url,
            data: data,
            success: function (resultData) {
                that.loading = false;
                if (resultData == undefined) {
                    that.alert.Error("服务器无响应");
                    if (typeof (failedCallBack) == "function") {
                        failedCallBack(that, obj);
                    }
                    return;
                }
                if (resultData.ResultCode == 500 || resultData.ResultCode == 500) {
                    that.alert.Error("服务器开小差了。。。");
                    if (typeof (failedCallBack) == "function") {
                        failedCallBack(that, obj);
                    }
                    return;
                }
                if (typeof (callBack) == "function") {
                    callBack(that, resultData, obj);
                }
            }
        });
    },
    Post: function (that, url, data, callBack, failedCallBack) {

        $.ajax({
            type: 'post',
            url: rootPath + url,
            data: data,
            success: function (resultData) {
                if (resultData == undefined) {
                    that.alert.Error("服务器无响应");
                    if (typeof (failedCallBack) == "function") {
                        failedCallBack(that);
                    }
                    return;
                }
                if (resultData.ResultCode == 500 || resultData.ResultCode == 500) {
                    that.alert.Error("服务器开小差了...");
                    if (typeof (failedCallBack) == "function") {
                        failedCallBack(that);
                    }
                    return;
                }
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    }
    ,
    GetDicData: function (that, pid, callBack) {
        var url = rootPath + "/Common/GetDicData?pid=" + pid;
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetPlayData: function (that, callBack) {
        var url = rootPath + "/ChallengeThreeCfg/GetPlayInfos";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    }, 
    GetBrandDatas: function (that, callBack) {
        var url = rootPath + "/GoodsBrand/GetBrandData";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetShoper: function (that, callBack) {
        var url = rootPath + "/UMUserLoginInfo/GetShoper";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetSupplierDatas: function (that, callBack) {
        var url = rootPath + "/SupplierInfo/GetSupplierDatas";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetUserDatas: function (that, callBack) {
        var url = rootPath + "/AuditRoleUserConfig/GetUserDatas";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetModuleDatas: function (that, callBack) {
        var url = rootPath + "/AuditModuleProcess/GetModuleDatas";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetRoleDatas: function (that, callBack) {
        var url = rootPath + "/AuditRoleConfig/GetRoleDatas";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetBankDatas: function (that, callBack) {
        var url = rootPath + "/NewAuditTaskBankCfg/GetBankData";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetSourceDatas: function (that, callBack) {
        var url = rootPath + "/NewAuditTaskSource/GetSourceDatas";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetKefuDatas: function (that, callBack) {

        var url = rootPath + "/WorkOrderBaseLog/GetKeFuList";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetBBCGoodsBrands: function (that, callBack) {
        var url = rootPath + "/BBCGoodsSycRecord/GetBBCGoodsBrandList";
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    GetMoneyNumberDic: function (that, gameTypeId, callBack) {
        var url = rootPath + "/Common/GetMoneyNumberDic?gameTypeId=" + gameTypeId;
        $.ajax({
            type: 'get',
            url: url,
            success: function (resultData) {
                if (typeof (callBack) == "function") {
                    callBack(that, resultData);
                }
            }
        });
    },
    Upload: function (that, url, data, callBack, failedCallBack, obj) {

        $.ajax({
            type: 'post',
            url: rootPath + url,
            data: data,
            contentType: false,
            processData: false,
            success: function (resultData) {
                if (resultData == undefined) {
                    that.alert.Error("服务器无响应");
                    if (typeof (failedCallBack) == "function") {
                        failedCallBack(that, resultData, obj);
                    }
                    return;
                }
                if (resultData.ResultCode == 500 || resultData.ResultCode == 500) {
                    that.alert.Error("服务器开小差了...");
                    if (typeof (failedCallBack) == "function") {
                        failedCallBack(that, resultData, obj);
                    }
                    return;
                }
                if (typeof (callBack) == "function") {
                    callBack(that, resultData, obj);
                }
            },
            error: function (e) {
                debugger;
            }
        });
    },
};


function dateToString(date) {
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString();
    var day = (date.getDate()).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    if (day.length == 1) {
        day = "0" + day;
    }
    var dateTime = year + "/" + month + "/" + day + ' ' + date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
    return dateTime;
};


