using Bussiness;
using Bussiness.ProductionLines;
using Common.Cookie;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebZhongZhi.Controllers
{

    public class RobotController : Controller
    {
        // GET: Login
        public ActionResult RobotShow(int lineid ,string deviceID,string model)
        {
            //System.Web.HttpContext.Current.Session["UserName"] = "abcd";

            //取model相关参数xml转为base64

            RobotServer rbs = new RobotServer();
           string devicelist =   rbs.DeviceList(lineid);
            ViewBag.devicelist = devicelist;
            ViewBag.deviceid = deviceID;
            return View(); 
        }

        
    }
}