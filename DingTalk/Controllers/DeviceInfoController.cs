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
    public class DeviceInfoController : BaseController
    {
        // GET: ProductionLines
        public ActionResult Index()
        {
            return View();
        }

         
    }
}