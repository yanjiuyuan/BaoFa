﻿using Bussiness.Quality;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class QualityController : BaseController
    {
        // GET: Quality
        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 批次质量数据接口
        /// </summary>
        /// <param name="OrderId">总订单编号</param>
        /// <param name="ChildId">子订单编号</param>
        /// <param name="StartTime">子订单开始时间</param>
        /// <param name="EndTime">子订单结束时间</param>
        /// 测试数据：Quality/GetBatchQuality?OrderId=18-35-12-1&ChildId=362-0102-1&StartTime=2018-01-26&EndTime=2018-01-27
        public string GetBatchQuality(string OrderId, string ChildId, string StartTime, string EndTime)
        {

            QualityServer qualityServer = new QualityServer();
            return qualityServer.GetBatchQuality(OrderId, ChildId, StartTime, EndTime);
        }


        /// <summary>
        /// 质量数读取接口
        /// </summary>
        /// <param name="OrderId">总订单编号</param>
        /// <param name="ChildId">子订单编号</param>
        /// <param name="StartTime">子订单开始时间</param>
        /// <param name="EndTime">子订单结束时间</param>
        /// 测试数据：Quality/GetQualityCounts?OrderId=18-35-12-1&ChildId=362-0102-1&StartTime=2018-01-26&EndTime=2018-01-27
        /// 返回数据 {"Goods":1,"Bads":2,"Inferior":1}  Goods正品，Bads废品，Inferior次品
        public string GetQualityCounts(string OrderId, string ChildId, string StartTime, string EndTime)
        {
            QualityServer qualityServer = new QualityServer();

            return qualityServer.GetQualityCounts(OrderId, ChildId, StartTime, EndTime);
        }
    }
}