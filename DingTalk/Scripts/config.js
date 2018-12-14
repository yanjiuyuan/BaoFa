
//监控界面配置参数-旧-华宝
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
var TENP = {
    '状态': 'TENP',
    '左十字压': 'LOilPressure',
    '右十字压': 'ROilPressure',
    '左右脚': 'LRFoot',
    '鞋长': 'ShoeL'
}
var SOLEP = {
    '总油压': 'BOilPressure'
}
var VIEWONE = {
    '状态':'V1',
    '状态号': 'VisualOneState',
    '报警号': 'VisualOneError'
}
var VIEWTWO = {
    '状态': 'V2',
    '状态号': 'VisualTwoState',
    '报警号': 'VisualTwoError'
}
var EMPY = {}
//监控界面配置参数-新-华昂
var TENP_L = {
    '左十字压': 'LOilPressure'
}
var TENP_R = {
    '右十字压': 'ROilPressure'
}

var DETAILRANGE = {
    //AtomizationPressure: {
    //    min: 0.1,
    //    max: 2.0
    //},//雾化压力
    //MaterialPressure: {
    //    min: 0.1,
    //    max: 2.0
    //},//物料压力
    //GlueWeight: {
    //    min: 15,
    //    max: 40
    //},//胶重
    //FlowMeter: {
    //    min: 0,
    //    max: 3.0
    //},//流量计
    //StateN: {
    //    min: 0,
    //    max: 1
    //},//机器人状态号
    OvenTemperatureNow: {
        min: 0,
        max: 150
    }//烤箱温度
}
var DEVICES = [
    [
        {
            num: 1,
            name: '鞋楦信息',
            stationstate: 0,
            detail: _cloneObj(EMPY),
            tooltip: {
                disabled: true,
                placement: 'top'
            },
            tips: {},
            position: {
                top: '44.3%',
                left: '84.6%'
            }
        },
        {
            num: 2,
            name: '视觉1号',
            stationstate: 0,
            detail: _cloneObj(EMPY),
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
                top: '53.0%',
                left: '84.6%'
            }
        },
        {
            num: 3,
            name: '一次喷胶',
            stationstate: 0,
            detail: _cloneObj(SPRAY),
            tooltip: {
                disabled: true,
                placement: 'top'
            },
            tips: {},
            position: {
                top: '61.2%',
                left: '84.6%'
            }
        },
        {
            num: 4,
            name: '压底',
            stationstate: 0,
            detail: _cloneObj(SOLEP),
            tooltip: {
                disabled: true,
                placement: 'right'
            },
            tips: {},
            position: {
                top: '67.2%',
                left: '70%'
            }
        },
        {
            num: 5,
            name: '视觉2号',
            stationstate: 0,
            detail: _cloneObj(EMPY),
            tooltip: {
                disabled: true,
                placement: 'right'
            },
            tips: {},
            position: {
                top: '76.2%',
                left: '70%'
            }
        },
        {
            num: 6,
            name: '喷处理剂',
            stationstate: 0,
            detail: _cloneObj(SPRAY),
            tooltip: {
                disabled: true,
                placement: 'right'
            },
            tips: {},
            position: {
                top: '80.2%',
                left: '0%'
            }
        },
        {
            num: 7,
            name: '二次喷胶',
            stationstate: 0,
            detail: _cloneObj(SPRAY),
            tooltip: {
                disabled: true,
                placement: 'left'
            },
            tips: {},
            position: {
                top: '49%',
                left: '0%'
            }
        },
        {
            num: 8,
            name: '三次喷胶',
            stationstate: 0,
            detail: _cloneObj(SPRAY),
            tooltip: {
                disabled: true,
                placement: 'left'
            },
            tips: {},
            position: {
                top: '40.2%',
                left: '0%'
            }
        },
        {
            num: 9,
            name: '贴围条1',
            stationstate: 0,
            detail: _cloneObj(EMPY),
            tooltip: {
                disabled: true,
                placement: 'left'
            },
            tips: {},
            position: {
                top: '34.67%',
                left: '15.56%'
            }
        },
        {
            num: 10,
            name: '贴围条2',
            stationstate: 0,
            detail: _cloneObj(EMPY),
            tooltip: {
                disabled: true,
                placement: 'left'
            },
            tips: {},
            position: {
                top: '25.67%',
                left: '15.56%'
            }
        },
        {
            num: 11,
            name: '护齿喷胶',
            stationstate: 0,
            detail: _cloneObj(SPRAY),
            tooltip: {
                disabled: true,
                placement: 'left'
            },
            tips: {},
            position: {
                top: '21%',
                left: '30.56%'
            }
        },
        {
            num: 12,
            name: '左十字压',
            stationstate: 0,
            detail: _cloneObj(TENP_L),
            tooltip: {
                disabled: true,
                placement: 'left'
            },
            tips: {},
            position: {
                top: '1.2%',
                left: '84.6%'
            }
        },
        {
            num: 13,
            name: '右十字压',
            stationstate: 0,
            detail: _cloneObj(TENP_R),
            tooltip: {
                disabled: true,
                placement: 'left'
            },
            tips: {},
            position: {
                top: '9.3%',
                left: '84.6%'
            }
        }
    ],
    [
    {
            num: 1,
            name: '视觉1号站',
            SprayID: 7,
            status: 0,
            detail: _cloneObj(VIEWONE),
            tooltip: {
                disabled: true,
                placement: 'top'
            },
            tips:{},
            position: {
                top: '82.02%',
                left: '0.56%'
            }
        },
        {
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
        },
        {
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
        },
        {
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
            },
            {
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
        },
        {
            num: 6,
            name: '压底机',
            SprayID: 7,
            status: 0,
            detail: _cloneObj(SOLEP),
            tooltip: {
                disabled: true,
                placement: 'right'
            },
            tips: {},
            position: {
                top: '36.67%',
                left: '0.56%'
            }
        },
        {
            num: 7,
            name: '视觉2号站',
            SprayID: 7,
            status: 0,
            detail: _cloneObj(VIEWTWO),
            tooltip: {
                disabled: true,
                placement: 'left'
            },
            tips: {},
            position: {
                top: '31.16%',
                left: '15.56%'
            }
        },
        {
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
            },
            {
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
        },
        {
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
            },
            {
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
        },
        {
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
        },
        {
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
    },
    //{
    //    num: 14,
    //    name: '视觉3号站',
    //    status: 0,
    //    tooltip: {
    //        disabled: true,
    //        placement: 'left'
    //    },
    //    tips: {},
    //    position: {
    //        top: '23.30%',
    //        left: '84.88%'
    //    }
    //},
    {
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
        },
        {
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
        },
        {
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
        },
        {
            num: 18,
            name: '十字压站',
            SprayID: 7,
            status: 0,
            detail: _cloneObj(TENP),
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
    ],
    []
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
        Mark: 1,
        Jobs: '蒸湿套楦',
        position: {
            top: '66%',
            left: '86%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 1,
        Mark: 0,
        Jobs: '蒸湿套楦',
        position: {
            top: '66%',
            left: '86%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 2,
        Mark: 1,
        Jobs: '贴腹',
        position: {
            top: '66%',
            left: '52%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 2,
        Mark: 0,
        Jobs: '贴腹',
        position: {
            top: '66%',
            left: '52%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 3,
        Mark: 1,
        Jobs: '贴大底',
        position: {
            top: '66%',
            left: '44%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 3,
        Mark: 0,
        Jobs: '贴大底',
        position: {
            top: '66%',
            left: '44%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 4,
        Mark: 1,
        Jobs: '敲平',
        position: {
            top: '66%',
            left: '36%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 4,
        Mark: 0,
        Jobs: '敲平',
        position: {
            top: '66%',
            left: '36%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 6,
        Mark: 1,
        Jobs: '滚压',
        position: {
            top: '2%',
            left: '59%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 6,
        Mark: 0,
        Jobs: '滚压',
        position: {
            top: '2%',
            left: '59%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 7,
        Mark: 1,
        Jobs: '贴护齿',
        position: {
            top: '2%',
            left: '69%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 7,
        Mark: 0,
        Jobs: '贴护齿',
        position: {
            top: '2%',
            left: '69%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 8,
        Mark: 1,
        Jobs: '贴标',
        position: {
            top: '2%',
            left: '76%'
        },
        stationstate: '',
        img: ''
    },
    //{
    //    ArtificialConfig_ID: 8,
    //    Mark: 0,
    //    Jobs: '贴标',
    //    position: {
    //        top: '2%',
    //        left: '76%'
    //    },
    //    stationstate: '',
    //    img: ''
    //},
    {
        ArtificialConfig_ID: 9,
        Mark: 1,
        Jobs: '品检',
        position: {
            top: '2%',
            left: '92%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 9,
        Mark: 0,
        Jobs: '品检',
        position: {
            top: '2%',
            left: '92%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 10,
        Mark: 1,
        Jobs: '码垛',
        position: {
            top: '46%',
            left: '92%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 10,
        Mark: 0,
        Jobs: '码垛',
        position: {
            top: '46%',
            left: '92%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 11,
        Mark: 1,
        Jobs: '放围条',
        position: {
            top: '2%',
            left: '49%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 11,
        Mark: 0,
        Jobs: '放围条',
        position: {
            top: '2%',
            left: '49%'
        },
        stationstate: '',
        img: ''
    },
    {
        ArtificialConfig_ID: 12,
        Mark: 1,
        Jobs: '组长',
        position: {
            top: '30%',
            left: '36%'
        },
        stationstate: '',
        img: ''
    }
]