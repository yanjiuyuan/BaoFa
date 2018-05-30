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

        //获取工位信息
        /// 测试数据 SingleQuality/GetSingleQualityStation 
        public string GetSingleQualityStation()
        {



            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            string[] strList = new string[7] { "Vamp", "Waio", "WaiT", "WaiS", "Outsole", "Mouthguards", "LineUsage" };
            string[] descList = new string[7] { "鞋面喷胶站", "围条一胶站", "围条二胶站", "围条三胶站", "护齿喷胶站", "大底喷胶站", "生产线参数" };

            for (int i = 0; i < strList.Length; i++)
            {
                KeyValuePair <string ,string> keyp = new KeyValuePair<string, string>(strList[i], descList[i]);
                list.Add(keyp);
            }
            
            return JsonConvert.SerializeObject(list);
        }



        //根据工位获取追溯鞋子时时数据
        /// 测试数据 SingleQuality/GetSingleQualityByRFID?RFID=5&Spray=Vamp
        public string GetSingleQualityByRFIDandSpray(string RFID,string Spray)
        {
            
                SingleQualityServer sServer = new SingleQualityServer();
                DataTable db = sServer.GetSingQuality(RFID, Spray);
                 
             
            if (db.Rows.Count == 0)
            {
                return "{\"ErrorType\":0,\"ErrorMessage\":\"暂无数据!\"}";
            }
             
            return JsonConvert.SerializeObject(db);
        }

    }
}