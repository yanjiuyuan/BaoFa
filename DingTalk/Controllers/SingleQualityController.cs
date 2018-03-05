using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class SingleQualityController : Controller
    {
        // GET: SingleQuality
        public ActionResult Index()
        {
            return View();
        }



        public string GetSingleQualityByRFID(string RFID)
        {
            return null;
        }
    }
}