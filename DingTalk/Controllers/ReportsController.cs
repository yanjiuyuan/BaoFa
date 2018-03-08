using Bussiness.Report;
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
                "ExcelFiles\\日报表\\"+ "华宝硫化鞋生产线日报表" + DateTime.Now.ToShortDateString())+ ".xls";
            ReportServer reportServer = new ReportServer();
            reportServer.GetDailyReport(strPath, strServerPath);
            return File(new FileStream(strServerPath, FileMode.Open), "application/octet-stream", Server.UrlEncode("华宝硫化鞋生产线日报表"+DateTime.Now.ToShortDateString()+".xls"));
        }
    }
}