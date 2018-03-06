using Bussiness.Quality;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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


        /// <summary>
        /// 追溯鞋子时时数据
        /// </summary>
        /// <param name="RFID">RFID</param>
        /// <returns></returns>
        /// 测试数据 SingleQuality/GetSingleQualityByRFID?RFID=5
        public string GetSingleQualityByRFID(string RFID)
        {
            string[] strList = new string[7] { "Vamp", "Waio", "WaiT", "WaiS", "Outsole", "Mouthguards", "LineUsage" };
            Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
            foreach (var item in strList)
            {
                SingleQualityServer sServer = new SingleQualityServer();
                DataTable db = sServer.GetSingQuality(RFID, item);
                dic.Add(item, db);
            }
            if (dic == null)
            {
                return "{\"ErrorType\":0,\"ErrorMessage\":\"暂无数据!\"}";
            }
            return JsonConvert.SerializeObject(dic);
        }
    }
}