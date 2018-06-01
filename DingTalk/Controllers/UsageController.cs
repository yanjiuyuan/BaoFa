using Bussiness.UsageInfo;
using Common.JsonHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class UsageController : Controller
    {
        // GET: Usage
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// usage时时数据读取
        /// </summary>
        /// <returns></returns>
        /// 测试数据 usage/getusage
        public string GetUsage()
        {
           UsageInfo usageInfo = new UsageInfo();
           return  usageInfo.GetUsage();
        }
    }
}