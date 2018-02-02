using Bussiness.DosageInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 时时用量统计
        /// </summary>
        /// <param name="OrderId">总订单编号</param>
        /// <param name="ChildId">子订单编号</param>
        /// <returns>返回围条用量 护齿用量 标签用量 大底用量</returns>
        /// 测试数据：/Statistics/GetDosageInfo?OrderId=18-35-12-1&ChildId=362-0102-1
        public string GetDosageInfo(string OrderId, string ChildId)
        {
            DosageInfoServer dServer = new DosageInfoServer();
            return dServer.GetDosageInfo(OrderId, ChildId);
        }

    }
}