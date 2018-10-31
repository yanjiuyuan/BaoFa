using Bussiness;
using Bussiness.Model;
using Bussiness.ProductionLines;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class RobotInfoController : BaseController
    {
        // GET:DeviceInfo
        public ActionResult Index()
        {
            return View();
        }

       
 

        public string QueryRobotInfo(string model)
        {
            RobotServer dev = new RobotServer();
           
           
            return dev.QueryRobotInfo(model);


        }
       
    }
}