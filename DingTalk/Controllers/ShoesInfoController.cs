using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class ShoesInfoController : Controller
    {
        // GET: ShoesInfo
        public ActionResult Index()
        {
            return View();
        }


        public string GetShoesInfo(string RFID)
        {

            return "";
        }
    }
}