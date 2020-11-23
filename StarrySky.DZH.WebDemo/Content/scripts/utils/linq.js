//js版本的linq，用法和c#一样

Array.prototype.where = function (func) {
    let sw = [];
    for (var i = 0; i < this.length; i++) {
        if (func(this[i]) === true) {
            sw.push(this[i]);
        }
    }
    return sw;
}
Array.prototype.exist = function (func) {
    for (var i = 0; i < this.length; i++) {
        if (func(this[i]) === true) {
            return true;
        }
    }
    return false;
}
Array.prototype.contains = function (model) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === model) {
            return true;
        }
    }
    return false;
}
Array.prototype.first = function (func) {
    for (var i = 0; i < this.length; i++) {
        if (func(this[i]) === true) {
            return this[i];
        }
    }
    return null;
}
Array.prototype.select = function (func) {
    let sw = [];
    for (var i = 0; i < this.length; i++) {
        sw.push(func(this[i]));
    }
    return sw;
}
Array.prototype.groupBy = function (func) {
    let sw = {};
    for (var i = 0; i < this.length; i++) {
        if (sw[func(this[i])] && sw[func(this[i])].length > 0) {
            sw[func(this[i])].push(this[i]);
        }
        else {
            sw[func(this[i])] = [this[i]];
        }
    }
    return sw;
}
Array.prototype.min = function (func) {
    if (this.length <= 0) return null;
    let sw = this[0];
    for (var i = 0; i < this.length; i++) {
        if (func != undefined) {
            if (func(this[i]) < sw) {
                sw = func(this[i]);
            }
        }
        else if (this[i] < sw) {
            sw = this[i];
        }
    }
    return sw;
}
Array.prototype.max = function (func) {
    if (this.length <= 0) return null;
    let sw = this[0];
    for (var i = 0; i < this.length; i++) {
        if (func != undefined) {
            if (func(this[i]) > func(sw)) {
                sw = this[i];
            }
        }
        else if (this[i] > sw) {
            sw = this[i];
        }
    }
    return sw;
}
Array.prototype.orderBy = function (func) {
    if (this.length <= 0) return null;
    let sw = [];
    for (var i = 0; i < this.length; i++) {
        sw.push(this[i]);
    }
    if (func) {
        sw.sort((a, b) => func(a) - func(b));
    }
    else {
        sw.sort();
    }
    return sw;
}
Array.prototype.orderByDesc = function (func) {
    if (this.length <= 0) return null;
    let sw = [];
    for (var i = 0; i < this.length; i++) {
        sw.push(this[i]);
    }
    if (func != undefined) {
        sw.sort((a, b) => func(b) - func(a));
    }
    else {
        sw.sort((a, b) => b - a);
    }
    return sw;
}
Array.prototype.remove = function (model, func, models) {
    if (this.length <= 0) return null;
    let sw = [];
    for (var i = 0; i < this.length; i++) {
        if (func != undefined&&func(this[i])!==true) {
            sw.push(this[i]);
        } else if (model != undefined && model !== this[i]) {
            sw.push(this[i]);
        } else if (models != undefined&&!(models.contains(this[i]))) {
            sw.push(this[i]);
        }
    }
    return sw;
}

/**
 * 集合取交集 Array.intersect(a,b)
 */
Array.intersect = function () {
    var result = new Array();
    var obj = {};
    for (var i = 0; i < arguments.length; i++) {
        for (var j = 0; j < arguments[i].length; j++) {
            var str = arguments[i][j];
            if (!obj[str]) {
                obj[str] = 1;
            } else {
                obj[str]++;
                if (obj[str] == arguments.length) {
                    result.push(str);
                }
            } //end else
        } //end for j
    } //end for i
    return result;
}

/** 
* 得到一个数组不重复的元素集合<br/> 
* 唯一化一个数组 
* @returns {Array} 由不重复元素构成的数组 
*/
Array.prototype.uniquelize = function () {
    var ra = new Array();
    for (var i = 0; i < this.length; i++) {
        if (!ra.contains(this[i])) {
            ra.push(this[i]);
        }
    }
    return ra;
}; 

//并集 Array.union(a,b)
Array.union = function () {
    var arr = new Array();
    var obj = {};
    for (var i = 0; i < arguments.length; i++) {
        for (var j = 0; j < arguments[i].length; j++) {
            var str = arguments[i][j];
            if (!obj[str]) {
                obj[str] = 1;
                arr.push(str);
            }
        } //end for j
    } //end for i
    return arr;
}

//2个集合的差集 在arr不存在 a.minus(b)
Array.prototype.minus = function (arr) {
    var result = new Array();
    var obj = {};
    for (var i = 0; i < arr.length; i++) {
        obj[arr[i]] = 1;
    }
    for (var j = 0; j < this.length; j++) {
        if (!obj[this[j]]) {
            obj[this[j]] = 1;
            result.push(this[j]);
        }
    }
    return result;
};