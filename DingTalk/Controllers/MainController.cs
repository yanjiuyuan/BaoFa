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
            return View();
        }
        public ActionResult DeviceMonitor()
        {
            return View();
        }

        public ActionResult TableDevice()
        {
            return View();
        }

        public ActionResult TableOrder()
        {
            return View();
        }

        public ActionResult TableOther()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult TableProduct()
        {
            return View();
        }

        public ActionResult TableQuality()
        {
            return View();
        }
        public ActionResult Main6()
        {
            return View();
        }
        public ActionResult Main7()
        {
            return View();
        }
        public ActionResult Main8()
        {
            return View();
        }
    }
}