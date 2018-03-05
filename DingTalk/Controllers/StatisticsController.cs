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
        /// 测试数据 Statistics/GetCurrentProduction?DataTime=2018-03-01
        public string GetCurrentProduction(string DataTime)
        {
            string strResult = string.Empty;
            if (DataTime != null)
            {
                strResult = ChangeTime(DataTime);
            }
            else
            {
                strResult = ChangeTime(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            return strResult;
        }

        public string ChangeTime(string DataTime)
        {
            DateTime time = Convert.ToDateTime(DataTime);
            long lTime = TimeHelper.ConvertDateTimeToInt(time);
            DosageInfoServer dServer = new DosageInfoServer();
            DataTable tb = dServer.GetCurrentProduction(lTime);
            return JsonHelper.DataTableToJson(tb);
        }

    }
}