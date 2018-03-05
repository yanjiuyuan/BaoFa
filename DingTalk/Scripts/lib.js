﻿
function getLocalObj(name) {
    return JSON.parse(localStorage.getItem(name))
}

function setLocalObj(name, obj) {
    localStorage.setItem(name,JSON.stringify(obj))
}

function logout() {
    localStorage.clear()
    _delCookie('UserName')
    location.reload()
}

function loadPage(url) {
    $("#tempPage").load(url)
}

function loadHtml(parentId,childId) {
    $("#" + parentId).html('')
    $("#" + parentId).append($("#" + childId))
}

function _cloneObj(obj) {
    var newObj = {}
    for (var o in obj) {
        newObj[o]=obj[o]
    }
    return newObj
}

function _cloneArr(arr) {
    var newArr = []
    for (var a of arr) {
        if (typeof (a) == 'object') {
            newArr.push($.extend(true, {}, a))
        }
        else newArr.push(a)
    }
    return newArr
}

function _formatQueryStr(obj) {
    let queryStr = '?'
    for (let o in obj) {
        queryStr = queryStr+o+'='+obj[o]+'&'
    }
    return queryStr.substring(0, queryStr.length-1)
}

//cookie 操作
function _getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}
function _delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = _getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}

function _dateToString(date,split) {
    let d = new Date(date)
    let year = d.getFullYear()
    let month = d.getMonth()+1
    let day = d.getDate()
    if (month < 10) month = '0' + month
    if (day < 10) day = '0' + day
    return year+split+month+split+day
}

function isArray(o) {
    return Object.prototype.toString.call(o) == '[object Array]';
}
//时间选择器插件参数
var pickerOptions = {
    shortcuts: [{
        text: '最近一周',
        onClick(picker) {
            const end = new Date();
            const start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
            picker.$emit('pick', [start, end]);
        }
    }, {
        text: '最近一个月',
        onClick(picker) {
            const end = new Date();
            const start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
            picker.$emit('pick', [start, end]);
        }
    }, {
        text: '最近三个月',
        onClick(picker) {
            const end = new Date();
            const start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
            picker.$emit('pick', [start, end]);
        }
    }]
}

//实例总参数
var intervalId = 0
var CURDATE = new Date()
var mixin = {
    data: {
        user: {},
        pickerOptions: pickerOptions,
        curYear: CURDATE.getFullYear(),
        curMonth: CURDATE.getMonth()+1
    },
    methods: {
        
    },
    created: function () {
        window.clearInterval(intervalId)
    }
}

