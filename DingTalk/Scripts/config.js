
//监控界面配置参数
var SPRAY = {
    '雾化压': 'AtomizationPressure',
    '物料压': 'MaterialPressure',
    '胶重': 'GlueWeight',
    '流量计': 'FlowMeter',
    //'烤箱实时温度': 'OvenTemperatureNow',
    '机器人状态号': 'StateN',
    '机器人报警号': 'ErrorN'
}
var ROSTER = {
    '烤箱温度':'OvenTemperatureNow'
}
var PRESS = {
    '十字压': 'BOilPressure',
    '左十字压': 'LOilPressure',
    '右十字压': 'ROilPressure',
    '左右脚': 'LRFoot',
    '鞋长': 'ShoeL'
}
var DETAILRANGE = {
    AtomizationPressure: {
        min: 0.1,
        max: 2.0
    },//雾化压力
    MaterialPressure: {
        min: 0.1,
        max: 2.0
    },//物料压力
    GlueWeight: {
        min: 15,
        max: 40
    },//胶重
    FlowMeter: {
        min: 0,
        max: 3.0
    },//流量计
    StateN: {
        min: 0,
        max: 1
    },//机器人状态号
    ErrorN: {
        min: 0,
        max: 0
    },//机器人报警号
    OvenTemperatureNow: {
        min: 0,
        max: 150
    }//OvenTemperatureNow
}
var DEVICES = [
    {
        num: 1,
        name: '视觉1号站',
        status: 0,
        tooltip: {
            disabled: true,
            placement: 'top'
        },
        tips:{},
        position: {
            top: '82.02%',
            left: '0.56%'
        }
    }, {
        num: 2,
        name: '鞋面喷胶站',
        SprayID: 1,
        status: 0,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'right'
        },
        //tips: {
        //    '1234': '4321',
        //    '666':'888'
        //},
        tips: {
            
        },
        position: {
            top: '63.95%',
            left: '0.56%'
        }
    }, {
        num: 3,
        name: '鞋面烤箱',
        SprayID: 1,
        status: 0,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'top'
        },
        tips: {},
        position: {
            top: '58.14%',
            left: '0.56%'
        }
    }, {
        num: 4,
        name: '大底烤箱',
        SprayID: 6,
        status: 0,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'right'
        },
        tips: {},
        position: {
            top: '49.55%',
            left: '0.56%'
        }
    }, {
        num: 5,
        name: '大底喷胶',
        status: 0,
        tooltip: {
            disabled: true,
            placement: 'right'
        },
        tips: {},
        position: {
            top: '43.27%',
            left: '0.56%'
        }
    }, {
        num: 6,
        name: '压底机',
        status: 0,
        tooltip: {
            disabled: true,
            placement: 'right'
        },
        tips: {},
        position: {
            top: '36.67%',
            left: '0.56%'
        }
    }, {
        num: 7,
        name: '视觉2号站',
        status: 0,
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '31.16%',
            left: '15.56%'
        }
    }, {
        num: 8,
        name: '围条一胶站',
        status: 0,
        SprayID: 2,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '24.42%',
            left: '15.56%'
        }
    }, {
        num: 9,
        name: '围条二胶站',
        SprayID: 3,
        status: 0,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '14.80%',
            left: '30.56%'
        }
    }, {
        num: 10,
        name: '围一烤箱',
        SprayID: 2,
        status: 0,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '17.67%',
            left: '15.56%'
        }
    }, {
        num: 11,
        name: '围二烤箱',
        SprayID: 3,
        status: 0,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '8.84%',
            left: '30.56%'
        }
    }, {
        num: 12,
        name: '围条三胶站',
        SprayID: 4,
        status: 0,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '9.30%',
            left: '84.88%'
        }
    }, {
        num: 13,
        name: '围三烤箱',
        SprayID: 4,
        status: 0,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '2.79%',
            left: '84.88%'
        }
    }, {
        num: 14,
        name: '视觉3号站',
        status: 0,
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '23.30%',
            left: '84.88%'
        }
    }, {
        num: 15,
        name: '围条贴附1站',
        status: 0,
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '46.51%',
            left: '84.88%'
        }
    }, {
        num: 16,
        name: '围条贴附2站',
        status: 0,
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '53.49%',
            left: '84.88%'
        }
    }, {
        num: 17,
        name: '护齿喷胶站',
        SprayID: 5,
        status: 0,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '62.79%',
            left: '70%'
        }
    }, {
        num: 18,
        name: '十字压站',
        status: 0,
        detail: _cloneObj(PRESS),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        tips: {},
        position: {
            top: '73.49%',
            left: '70%'
        }
    }
]

//批次质量配置参数
var QUALITY_RADAR = {
    'AppearanceQualified': '硫化外观合格率',
    'AppearanceAfterQualified': '硫化后外观合格率',
    'VampPullQualified': '鞋面拉力合格点占比',
    'DaDiPullQualified': '大底拉力合格点占比',
    'ZheWangQualified': '折弯疲劳合格率'
}
var QUALITY_PIE = {
    'Goods': 'A等',
    'Bads': 'B等',
    'Inferior': 'C等'
}

//工艺人员界面配置参数
var WORKERS = [
    {
        ArtificialConfig_ID: 1,
        Jobs: '套楦',
        position: {
            top: '74%',
            left: '4%'
        },
        img: ''
    },
    {
        ArtificialConfig_ID: 4,
        Jobs: '帖大底',
        position: {
            top: '0%',
            left: '22%'
        },
        img: ''
    },
    {
        ArtificialConfig_ID:5,
        Jobs: '敲平',
        position: {
            top: '0%',
            left: '42%'
        },
        img: ''
    },
    {
        ArtificialConfig_ID: 8,
        Jobs: '贴围条',
        position: {
            top: '20%',
            left: '92%'
        },
        img: ''
    },
    {
        ArtificialConfig_ID: 9,
        Jobs: '滚压',
        position: {
            top: '74%',
            left: '60%'
        },
        img: ''
    },
    {
        ArtificialConfig_ID: 11,
        Jobs: '帖护齿',
        position: {
            top: '74%',
            left: '40%'
        },
        img: ''
    },
    {
        ArtificialConfig_ID: 13,
        Jobs: '品检',
        position: {
            top: '74%',
            left: '22%'
        },
        img: ''
    },
    {
        ArtificialConfig_ID: 14,
        Jobs: '工艺组组长',
        position: {
            top: '30%',
            left: '36%'
        },
        img: ''
    },
    {
        ArtificialConfig_ID: 15,
        Jobs: '喷胶调试员',
        position: {
            top: '30%',
            left: '50%'
        },
        img: ''
    }
]