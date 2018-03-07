using Bussiness.WorkSataions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class WorkStationsController : Controller
    {
        // GET: WorkStations
        public ActionResult Index()
        {
            return View();
        }

        

        /// <summary>
        /// 工位读取接口
        /// </summary>
        /// <returns></returns>
        /// 测试数据  WorkStations/GetWorkSataionsInfo
        public string GetWorkSataionsInfo()
        {
            WorkSataionsServer wServer = new WorkSataionsServer();
            return JsonConvert.SerializeObject(wServer.GetWorkStationInfo());
        }
    }
}