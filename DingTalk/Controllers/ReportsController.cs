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

        /// 测试数据：Reports/GetMonQuality?Year=2018&Month=5
        public string GetMonQuality(string Year,string Month)
        {
            QualityServer quaServer = new QualityServer();
            string datestr = Year + "-" + Month + "-" + "01";
            DateTime time = Convert.ToDateTime(datestr);
           string StartDate = time.ToString("yyyy-MM-dd HH:mm:ss");
            string EndDate=time.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");
            return quaServer.GetMonQuality(  StartDate,   EndDate) ;
              
        }

        /// 测试数据：Reports/GetMonQuality?Year=2018&Month=5
        public string GetMonUasge(string Year, string Month)
        {
            UsageInfo usageServer = new UsageInfo();
            string datestr = Year + "-" + Month + "-" + "01";
            DateTime time = Convert.ToDateTime(datestr);
            string StartDate = time.ToString("yyyy-MM-dd HH:mm:ss");
            string EndDate = time.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");
            return usageServer.GetMonthUsage(StartDate, EndDate);

        }


    }
}