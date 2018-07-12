using Bussiness;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebZhongZhi.Controllers
{
    public class MainController : Controller
    {

        public ActionResult Index()
        {
            if (HttpContext.Session["user"] == null)
            {
                HttpContext.Response.Redirect("~/Login/Index", true);
                return View();
            }

            return View();
        }
        public ActionResult DeviceMonitor()
        {
            return View();
        }
        public ActionResult DeviceDetail()
        {
            return View();
        }

        public ActionResult ParamOrder()
        {
            return View();
        }

        public ActionResult ParamStand()
        {
            return View();
        }

        public ActionResult ParamLine()
        {
            return View();
        }

        public ActionResult ParamWorker()
        {
            return View();
        }
        public ActionResult ChartBeat()
        {
            return View();
        }
        public ActionResult ChartProduce()
        {
            return View();
        }
        public ActionResult ChartOrderQuality()
        {
            return View();
        }
        public ActionResult ChartProductQuality()
        {
            return View();
        }
        public ActionResult ChartMultiply()
        {
            return View();
        }
        public ActionResult TableQuality()
        {
            return View();
        }
        public ActionResult ChartRunStatus()
        {
            return View();
        }
        public ActionResult ChartDeviceErr()
        {
            return View();
        }
        public ActionResult ChartDay()
        {
            return View();
        }
        public ActionResult VideoMonitor()
        {
            return View();
        }
        public ActionResult VideoSimulation()
        {
            return View();
        }
        public ActionResult ManagerUser()
        {
            return View();
        }
        public ActionResult ManagerDevice()
        {
            return View();
        }
        public ActionResult NewPage()
        {
            return View();
        }
        public ActionResult BlankPage()
        {
            return View();
        }

        public string MenuList(string roleid)
        {
            if (roleid == null)
                return Global.RETURN_ERROR("roleid参数不能为空！");
             return  JsonConvert.SerializeObject (Global.MenuList[roleid]);
        }
        public string AccessList(string roleid)
        {
            if (roleid == null)
                return Global.RETURN_ERROR("roleid参数不能为空！");
            return JsonConvert.SerializeObject(Global.AccessList[roleid]);
        }
    }
}