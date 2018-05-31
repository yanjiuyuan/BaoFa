using Bussiness.Quality;
using Bussiness.Report;
using Bussiness.UsageInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 生成日报表接口(最新一天)
        /// </summary>
        /// 测试数据： Reports/GetDailyReport
        public FileStreamResult GetDailyReport()
        {
            //模板路径
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "ExcelFiles\\华宝硫化鞋生产线日报表模板.xls");
            //服务器生成路径
            string strServerPath= Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "ExcelFiles\\日报表\\"+ "华宝硫化鞋生产线日报表" + DateTime.Now.ToString("yyyy-MM-dd"))+ ".xls";
            ReportServer reportServer = new ReportServer();
            reportServer.GetDailyReport(strPath, strServerPath);
            return File(new FileStream(strServerPath, FileMode.Open), "application/octet-stream", Server.UrlEncode("华宝硫化鞋生产线日报表"+DateTime.Now.ToString("yyyy-MM-dd") + ".xls"));
        }

        /// 测试数据：Reports/GetMonQuality?startYear=2018&startMonth=5&endYear=2018&endMonth=5
        public string GetMonQuality(string startYear,string startMonth, string endYear, string endMonth)
        {
            QualityServer quaServer = new QualityServer();
            string startdatestr = startYear + "-" + startMonth + "-" + "01";
            DateTime starttime = Convert.ToDateTime(startdatestr);
           string StartDate = starttime.ToString("yyyy-MM-dd HH:mm:ss");

            string enddatestr = endYear + "-" + endMonth + "-" + "01";
            DateTime endtime = Convert.ToDateTime(enddatestr);
            string EndDate = endtime.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");
            if(!startMonth.Equals(endMonth))
            return quaServer.GetMonQuality(  StartDate,   EndDate) ;
            else
            return quaServer.GetMonQuality(StartDate, EndDate,false);
        }

        /// 测试数据：Reports/GetMonQuality?startYear=2018&startMonth=5&endYear=2018&endMonth=5
        public string GetMonUsage(string startYear, string startMonth, string endYear, string endMonth)
        {
            UsageInfo usageServer = new UsageInfo();
            string startdatestr = startYear + "-" + startMonth + "-" + "01";
            DateTime starttime = Convert.ToDateTime(startdatestr);
            string StartDate = starttime.ToString("yyyy-MM-dd HH:mm:ss");

            string enddatestr = endYear + "-" + endMonth + "-" + "01";
            DateTime endtime = Convert.ToDateTime(enddatestr);
            string EndDate = endtime.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");
            return usageServer.GetMonthUsage(StartDate, EndDate);

        }


    }
}