using Bussiness.WorkSataions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class WorkStationsController : BaseController
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

        //test:  /WorkStations/GetWorkStationList?orderid=99-1-18-1&lineid=1

        public string  GetWorkStationList(string orderid, int lineid = 1)
        {
            WorkSataionsServer wServer = new WorkSataionsServer();
           return wServer.GetWorkStationList(orderid, lineid);


        }


        //test: /WorkStations/GetArtificialInfo?artificialid=24
        public string GetArtificialInfo(int artificialid)
        {
            WorkSataionsServer wServer = new WorkSataionsServer();
            return wServer.GetArtificialInfo(artificialid);

        }


        public string GetMachineLocationList(  int lineid = 1)
        {
            WorkSataionsServer wServer = new WorkSataionsServer();
            return wServer.GetMachineLocationList( lineid);


        }
        public string GetLocationList(int lineid = 1)
        {
            WorkSataionsServer wServer = new WorkSataionsServer();
            return wServer.GetLocationList(lineid);


        }

    }
}