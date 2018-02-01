var SPRAY = {
    '雾化压': 'AtomizationPressure',
    '物料压': 'MaterialPressure',
    '胶重': 'GlueWeight',
    '流量计': 'FlowMeter',
    '烤箱实时温度': 'OvenTemperatureNow',
    '机器人状态号': 'StateN',
    '机器人报警号': 'ErrorN'
}
var ROSTER = {
    '烤箱温度':'OvenTemperatureNow'
}

var DEVICES = [
    {
        num: 1,
        name: '视觉1号站',
        tooltip: {
            disabled: true,
            placement: 'top'
        },
        position: {
            top: '93.02%',
            left: '0.56%'
        }
    }, {
        num: 2,
        name: '鞋面喷胶站',
        RFIDN: 1,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'right'
        },
        tips: {
            '参数1': '参数1',
            '参数2': '参数2'
        },
        position: {
            top: '63.95%',
            left: '0.56%'
        }
    }, {
        num: 3,
        name: '鞋面烤箱',
        RFIDN: 1,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'top'
        },
        position: {
            top: '58.14%',
            left: '0.56%'
        }
    }, {
        num: 4,
        name: '大底烤箱',
        RFIDN: 6,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'top'
        },
        position: {
            top: '45.35%',
            left: '0.56%'
        }
    }, {
        num: 5,
        name: '压底机',
        status: 0,
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '37.67%',
            left: '15.56%'
        }
    }, {
        num: 6,
        name: '视觉2号站',
        status: 2,
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '31.16%',
            left: '15.56%'
        }
    }, {
        num: 7,
        name: '围条一胶站',
        status: 3,
        RFIDN: 2,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '24.42%',
            left: '15.56%'
        }
    }, {
        num: 8,
        name: '围条二胶站',
        RFIDN: 3,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '17.67%',
            left: '15.56%'
        }
    }, {
        num: 9,
        name: '围一烤箱',
        RFIDN: 2,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '14.80%',
            left: '30.56%'
        }
    }, {
        num: 10,
        name: '围二烤箱',
        RFIDN: 3,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '8.84%',
            left: '30.56%'
        }
    }, {
        num: 11,
        name: '围条三胶站',
        RFIDN: 4,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '2.60%',
            left: '30.56%'
        }
    }, {
        num: 12,
        name: '围三烤箱',
        RFIDN: 4,
        detail: _cloneObj(ROSTER),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '2.79%',
            left: '84.88%'
        }
    }, {
        num: 13,
        name: '视觉3号站',
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '9.30%',
            left: '84.88%'
        }
    }, {
        num: 14,
        name: '围条贴附1号站',
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '46.51%',
            left: '84.88%'
        }
    }, {
        num: 15,
        name: '围条贴附2号站',
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '53.49%',
            left: '84.88%'
        }
    }, {
        num: 16,
        name: '护齿喷胶站',
        RFIDN: 5,
        detail: _cloneObj(SPRAY),
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '62.79%',
            left: '70%'
        }
    }, {
        num: 17,
        name: '十字压站',
        tooltip: {
            disabled: true,
            placement: 'left'
        },
        position: {
            top: '73.49%',
            left: '70%'
        }
    }
]