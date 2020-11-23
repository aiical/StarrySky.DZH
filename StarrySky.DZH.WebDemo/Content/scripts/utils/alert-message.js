

Vue.prototype.alert = {
    Info: function (message, duration) {
        let dur = duration == undefined || duration < 0 ? 2000 : duration;
        Vue.prototype.$message({ message: message, duration: dur});
    },
    Warning: function (message, duration) {
        let dur = duration == undefined || duration < 0 ? 2000 : duration;
        Vue.prototype.$message({
            message: message,
            type: 'warning',
            duration: dur
        });
    },
    Success: function (message, duration) {
        let dur = duration == undefined || duration < 0 ? 2000 : duration;
        Vue.prototype.$message({
            message: message,
            type: 'success'
            , duration: dur
        });
    },

    Error: function (message, duration) {
        let dur = duration == undefined || duration < 0 ? 2000 : duration;
        Vue.prototype.$message({
            message: message,
            type: 'error',
            duration: dur
        });
    }
};

CheckString = function (str) {
    if (str === undefined || str === null || str.trim() === "") {
        return false;
    }
    return true;
};

CheckList = function (list) {
    if (list === undefined || list === null) {
        return false;
    }
    if (list.length <= 0) {
        return false;
    }
    return true;
};

