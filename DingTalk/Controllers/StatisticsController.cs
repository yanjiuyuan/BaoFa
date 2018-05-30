using Bussiness.Chart;
using Bussiness.DosageInfo;
using Bussiness.Time;
using Common.JsonHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 时时用量统计
        /// </summary>
        /// <param name="OrderId">总订单编号</param>
        /// <param name="ChildId">子订单编号</param>
        /// <returns>返回围条用量 护齿用量 标签用量 大底用量</returns>
        /// 测试数据：/Statistics/GetDosageInfo?OrderId=18-35-12-1&ChildId=362-0102-1
        public string GetDosageInfo(string OrderId, string ChildId)
        {
            DosageInfoServer dServer = new DosageInfoServer();
            return dServer.GetDosageInfo(OrderId, ChildId);
        }



        /// <summary>
        /// 时时产量读取接口
        /// </summary>
        /// <param name="DataTime">查询日期(格式为yyyy-MM-dd)</param>
        /// <returns>返回头尾以及整点数据</returns>
        /// 测试数据 Statistics/GetCurrentProduction
        /// 测试数据 /Statistics/GetCurrentProduction?DataTime=2018-03-01&Count=100
        public string GetCurrentProduction(string DataTime,int Count)
        {
            string strResult = string.Empty;
            if (DataTime != null)
            {
                strResult = ChangeTime(DataTime,Count);
            }
            else
            {
                strResult = ChangeTime(DateTime.Now.ToString("yyyy-MM-dd"), Count);
            }
            return strResult;
        }

        public string ChangeTime(string DataTime,int Count)
        {
            DateTime time = Convert.ToDateTime(DataTime);
            long lTime = TimeHelper.ConvertDateTimeToInt(time);
            DosageInfoServer dServer = new DosageInfoServer();
            DataTable tb = dServer.GetCurrentProduction(lTime, Count);
            return JsonHelper.DataTableToJson(tb);
        }

        /// <summary>
        /// 时时生产速度读取接口
        /// </summary>
        /// <param name="DataTime">查询日期(格式为yyyy-MM-dd)</param>
        /// <returns>返回头尾以及整点数据</returns>
        /// 测试数据 Statistics/GetYieldFluct
        /// 测试数据 /Statistics/GetYieldFluct?DataTime=2018-05-13&dura=30
        public string GetYieldFluct(string DataTime, int dura=30)
        {
            string strResult = string.Empty;
            if (DataTime != null)
            {
                strResult = FluctChangeTime(DataTime, dura);
            }
            else
            {
                strResult = FluctChangeTime(DateTime.Now.ToString("yyyy-MM-dd"), dura);
            }
            return strResult;
        }

        public string FluctChangeTime(string DataTime, int dura)
        {
            DateTime time = Convert.ToDateTime(DataTime);
            long lTime = TimeHelper.ConvertDateTimeToInt(time);
            DateTime timeend = Convert.ToDateTime(DataTime).AddDays(1);
            long lTimeend = TimeHelper.ConvertDateTimeToInt(timeend);
            DosageInfoServer dServer = new DosageInfoServer();
            DataTable tb = dServer.GetYieldFluctuation(lTime, lTimeend, dura);
            return JsonHelper.DataTableToJson(tb);
        }

        /// 测试数据 /Statistics/ChartBeatQuery?mins=60
        public string ChartBeatQuery(int mins= 30)
        {
             ChartBeatServer chartBeatServer = new  ChartBeatServer();
             return chartBeatServer.ChartBeatQuery(mins);
        }

    }
}