//实例总参数
var UrlObj = {} //url参数对象

if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/(^[\s\n\t]+|[\s\n\t]+$)/g, "");
    }
}

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
    var param = url.split('?')[1]
    if (param) {
        var paramArr = param.split('&')
        for (let p of paramArr) {
            UrlObj[p.split('=')[0]] = p.split('=')[1]
        }
    }
    $("#tempPage").load(url)
}

function loadHtml(parentId,childId) {
    $("#" + parentId).html('')
    $("#" + parentId).append($("#" + childId))
}

function _cloneObj(obj) {
    var newObj = {}
    for (let o in obj) {
        newObj[o]=obj[o]
    }
    return newObj
}

function _cloneArr(arr) {
    var newArr = []
    for (var a = 0; a < arr.length;a++) {
        if (typeof (arr[a]) == 'object') {
            newArr.push($.extend(true, {}, arr[a]))
        }
        else newArr.push(arr[a])
    }
    return newArr
}

function _formatQueryStr(obj) {
    var queryStr = '?'
    for (var o in obj) {
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

function _dateToString(date, split) {
    if (!split) split = '-' 
    var d = new Date(date)
    var year = d.getFullYear()
    var month = d.getMonth()+1
    var day = d.getDate()
    if (month < 10) month = '0' + month
    if (day < 10) day = '0' + day
    return year+split+month+split+day
}

function _getTime() {
    var split = "-"
    var d = new Date()
    var year = d.getFullYear()
    var month = d.getMonth() + 1
    var day = d.getDate()
    var hour = d.getHours()
    var minute = d.getMinutes()
    if (month < 10) month = '0' + month
    if (day < 10) day = '0' + day
    return year + split + month + split + day + ' ' + hour + ':' + minute
}

function isArray(o) {
    return Object.prototype.toString.call(o) == '[object Array]';
}
//时间选择器插件参数
var pickerOptions = {
    shortcuts: [
        {
        text: '最近一周',
        onClick:function(picker) {
            var end = new Date();
            var start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
            picker.$emit('pick', [start, end]);
        }
    }, {
        text: '最近一个月',
        onClick: function(picker) {
            var end = new Date();
            var start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
            picker.$emit('pick', [start, end]);
        }
    }, {
        text: '最近三个月',
        onClick: function(picker) {
            var end = new Date();
            var start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
            picker.$emit('pick', [start, end]);
        }
    }
    ]
}

//实例总参数
var intervalId = 0
var CURDATE = new Date()
var DATASTR = CURDATE.getFullYear() + '-' + (CURDATE.getMonth() + 1) + '-' + CURDATE.getDate()
var mixin = {
    data: {
        user: {},
        pickerOptions: pickerOptions,
        curYear: CURDATE.getFullYear(),
        curMonth: CURDATE.getMonth() + 1,
        rules: {
            qrCode: [
                { required: true, message: '二维码不能为空', trigger: 'blur' },
                { min: 1, max: 15, message: '长度在 1 到 15 个字符', trigger: 'blur' }
            ],
            stationId: [
                { required: true, message: '工位不能为空', trigger: 'blur' }
            ],
            startTime: [
                { required: true, message: '开始时间不能为空', trigger: 'blur' }
            ],
            endTime: [
                { required: true, message: '结束时间不能为空', trigger: 'blur' }
            ],
            userName: [
                { required: true, message: '用户名不能为空', trigger: 'blur' }
            ],
            iRole: [
                { required: true, message: '用户角色不能为空', trigger: 'blur' }
            ],
            strCompanyId: [
                { required: true, message: '部门Id不能为空', trigger: 'blur' }
            ],
            strCompanyName: [
                { required: true, message: '部门名称不能为空', trigger: 'blur' }
            ],
            DeviceId: [
                { required: true, message: '设备Id不能为空', trigger: 'blur' }
            ],
            DeviceName: [
                { required: true, message: '设备名称不能为空', trigger: 'blur' }
            ],
            DeviceType: [
                { required: true, message: '设备类型不能为空', trigger: 'blur' }
            ],
            DeviceModel: [
                { required: true, message: '设备型号不能为空', trigger: 'blur' }
            ],
            DeviceStat: [
                { required: true, message: '设备状态不能为空', trigger: 'blur' }
            ]
        },
        currentPage: 1,
        totalRows: 0,
        pageSize: 5
    },
    methods: {
        //翻頁相關事件
        getData() {
            var start = this.pageSize * (this.currentPage - 1)
            this.tableData = this.data.slice(start, start + this.pageSize)
        },
        handleSizeChange: function (val) {
            this.currentPage = 1
            this.pageSize = val
            this.getData()
        },
        handleCurrentChange: function (val) {
            this.currentPage = val
            this.getData()
        }
        
    },
    created: function () {
        window.clearInterval(intervalId)
    }
}

