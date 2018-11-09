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
    public class SingleQualityController : BaseController
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
            string [] strList = new string[7] { "Vamp", "Waio", "WaiT", "WaiS", "Outsole", "Mouthguards", "LineUsage" };
            Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
            for(int i=0;i< strList.Length-1; i++)  
            {
                SingleQualityServer sServer = new SingleQualityServer();
                DataTable db = sServer.GetSingQuality(RFID, i);
                dic.Add(strList[i], db);
            }
            SingleQualityServer sServer1 = new SingleQualityServer();
            DataTable db1 = sServer1.GetSingQualityLineUsage(RFID );
            dic.Add(strList[6], db1);

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
            string[] strList = new string[7] { "1", "2", "3", "4", "5", "6", "7" };
            string[] descList = new string[7] { "一次喷胶", "喷处理剂", "二次喷胶", "三次喷胶", "护齿喷胶", "大底喷胶", "压合参数" };

            for (int i = 0; i < strList.Length; i++)
            {
                KeyValuePair <string ,string> keyp = new KeyValuePair<string, string>(strList[i], descList[i]);
                list.Add(keyp);
            }
            
            return JsonConvert.SerializeObject(list);
        }



        //根据工位获取追溯鞋子时时数据
        /// 测试数据 SingleQuality/GetSingleQualityByRFIDandSpray?RFID=5&SprayId=Vamp
        public string GetSingleQualityByRFIDandSpray(string RFID,int Spray)
        {
            DataTable db = new DataTable();
                SingleQualityServer sServer = new SingleQualityServer();
                if (Spray != 7)
                      db = sServer.GetSingQuality(RFID, Spray);
                else
            db = sServer.GetSingQualityLineUsage(RFID);
         
            if (db.Rows.Count == 0)
            {
                return "{\"ErrorType\":0,\"ErrorMessage\":\"暂无数据!\"}";
            }
             
            return JsonConvert.SerializeObject(db);
        }

    }
}