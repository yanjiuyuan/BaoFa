using Bussiness;
using Bussiness.DosageInfo;
using Bussiness.ProductionLines;
using Bussiness.Quality;
using Bussiness.Report;
using Bussiness.UsageInfo;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bussiness.Chart;

namespace DingTalk.Controllers
{

    //电子看板

    //一次http查询，获取所有看板数据
    public class DataViewController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }



        /// 测试数据：DataView/GetData?lineid=1
        public string GetData(int lineid)
        {
            
            return  Bussiness.DataViewData.lineViewData[lineid];
             
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



        /// 测试数据：Reports/GetMonProduct?startYear=2018&startMonth=5&endYear=2018&endMonth=5
        public string GetMonProduct(string startYear, string startMonth, string endYear, string endMonth)
        {
            DosageInfoServer dosServer = new DosageInfoServer();
            string startdatestr = startYear + "-" + startMonth + "-" + "01";
            DateTime starttime = Convert.ToDateTime(startdatestr);
            string StartDate = starttime.ToString("yyyy-MM-dd HH:mm:ss");

            string enddatestr = endYear + "-" + endMonth + "-" + "01";
            DateTime endtime = Convert.ToDateTime(enddatestr);
            string EndDate = endtime.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");
            if (startMonth.Equals(endMonth) && startYear.Equals(endYear))
                return dosServer.GetMonProduct(StartDate, EndDate, false);
            else
                return dosServer.GetMonProduct(StartDate, EndDate);
             

        }



        /// </summary>
        /// 测试数据： Reports/GetProductDailyReport?DataTime=2018-05-11&lineid=1
        public FileStreamResult GetProductDailyReport(string DataTime, int lineid=1)
        {
            //模板路径
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "ExcelFiles\\华宝硫化鞋生产线产量日报表模板.xls");
            //服务器生成路径
            string strServerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "ExcelFiles\\日报表\\" + "华宝硫化鞋生产线产量日报表" + DataTime) + ".xls";
            ReportServer reportServer = new ReportServer();
            DateTime starttime = Convert.ToDateTime(DataTime);
            string StartDate = starttime.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime endtime = Convert.ToDateTime(DataTime).AddDays(1);
            string EndDate = endtime.ToString("yyyy-MM-dd HH:mm:ss");

            reportServer.GetProductDailyReport(strPath, strServerPath, StartDate, EndDate, lineid);
            return File(new FileStream(strServerPath, FileMode.Open), "application/octet-stream", Server.UrlEncode("华宝硫化鞋生产线产量日报表" + "ExcelFiles\\日报表\\" + "华宝硫化鞋生产线产量日报表" + DataTime) + ".xls");
    
        }
        /// </summary>
        /// 测试数据： Reports/GetProductDailyData?DataTime=2018-05-11&lineid=1
        public string GetProductDailyData(string DataTime, int lineid = 1)
        {
             
            ReportServer reportServer = new ReportServer();
            DateTime starttime = Convert.ToDateTime(DataTime);
            string StartDate = starttime.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime endtime = Convert.ToDateTime(DataTime).AddDays(1);
            string EndDate = endtime.ToString("yyyy-MM-dd HH:mm:ss");

           return reportServer.GetProductDailyData( StartDate, EndDate, lineid);
           
        }


        /// </summary>
        /// 测试数据： Reports/GetQualityDailyReport?DataTime=2018-05-11&lineid=1
        public FileStreamResult GetQualityDailyReport(string DataTime, int lineid = 1)
        {
            //模板路径
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "ExcelFiles\\华宝硫化鞋生产线质量日报表模板.xls");
            //服务器生成路径
            string strServerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "ExcelFiles\\日报表\\" + "华宝硫化鞋生产线质量日报表" + DataTime) + ".xls";
            ReportServer reportServer = new ReportServer();
            DateTime starttime = Convert.ToDateTime(DataTime);
            string StartDate = starttime.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime endtime = Convert.ToDateTime(DataTime).AddDays(1);
            string EndDate = endtime.ToString("yyyy-MM-dd HH:mm:ss");

            reportServer.GetQualityDailyReport(strPath, strServerPath, StartDate, EndDate, lineid);
            return File(new FileStream(strServerPath, FileMode.Open), "application/octet-stream", Server.UrlEncode("华宝硫化鞋生产线质量日报表" + "ExcelFiles\\日报表\\" + "华宝硫化鞋生产线质量日报表" + DataTime) + ".xls");

        }

        /// </summary>
        /// 测试数据： Reports/GetQualityDailyData?DataTime=2018-05-11&lineid=1
        public string GetQualityDailyData(string DataTime, int lineid = 1)
        {

            ReportServer reportServer = new ReportServer();
            DateTime starttime = Convert.ToDateTime(DataTime);
            string StartDate = starttime.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime endtime = Convert.ToDateTime(DataTime).AddDays(1);
            string EndDate = endtime.ToString("yyyy-MM-dd HH:mm:ss");

             return   reportServer.GetQualityDailyData(StartDate, EndDate, lineid);
         
        }



        /// </summary>
        /// 测试数据： Reports/GetQualityDailyDetail?DataTime=2018-05-11&lineid=1

        public string GetQualityDailyDetail(string DataTime, int lineid = 1)
        {

            ReportServer reportServer = new ReportServer();
            DateTime starttime = Convert.ToDateTime(DataTime);
            string StartDate = starttime.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime endtime = Convert.ToDateTime(DataTime).AddDays(1);
            string EndDate = endtime.ToString("yyyy-MM-dd HH:mm:ss");

            return reportServer.GetQualityDailyDetail(StartDate, EndDate, lineid);

        }






        /// <summary>
        /// /Reports/LineDailyRpt
        /// </summary>
        /// <param name="begintime"> 查询日期(*)</param>
        /// <param name="groupid">集团id</param>
        /// <param name="companyid">公司id</param>
        /// <param name="foundryid">车间id</param>
        /// <param name="ProductLineId">产线id</param>
        /// <returns></returns>
        public string  LineDailyRpt(string begintime,   int? groupid, int? companyid, int? foundryid, int? ProductLineId)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid,  companyid,foundryid, ProductLineId);

            ReportServer rps = new ReportServer();
            return rps.LineDailyRpt(begintime, lineids);


        }
    /// <param name=""begintime"">开始日期(*)
    /// <param name=""endtime""> 结束日期(*)
    /// <param name=""groupid"">集团id
    /// <param name=""companyid"">公司id
    /// <param name=""foundryid"">车间id
    /// <param name=""ProductLineId"">产线id"	
        public string LinePhaseRpt(string begintime,string endtime, int? groupid, int? companyid, int? foundryid, int? ProductLineId)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);

            ReportServer rps = new ReportServer();
            return rps.LinePhaseRpt(begintime, endtime, lineids);


        }

        /// <param name=""begintime"">查询月份(*)
        /// <param name=""endtime""> 结束日期(*)
        /// <param name=""groupid"">集团id
        /// <param name=""companyid"">公司id
        /// <param name=""foundryid"">车间id
        /// <param name=""ProductLineId"">产线id"	

        public string LineMonthRpt(string begintime, int? groupid, int? companyid, int? foundryid, int? ProductLineId)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);
            ReportServer rps = new ReportServer();
            return rps.LineMonthRpt(begintime, lineids);

        }


        /// <param name=""begintime"">查询年份(*)
        /// <param name=""endtime""> 结束日期(*)
        /// <param name=""groupid"">集团id
        /// <param name=""companyid"">公司id
        /// <param name=""foundryid"">车间id
        /// <param name=""ProductLineId"">产线id"
        public string LineYearRpt(string begintime, int? groupid, int? companyid, int? foundryid, int? ProductLineId)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);
            ReportServer rps = new ReportServer();
            return rps.LineYearRpt(begintime, lineids);
        }

        public string DeviceDailyRpt(string begintime, int? groupid, int? companyid, int? foundryid, int? ProductLineId ,string  devicemodel)
        {

            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);

            ReportServer rps = new ReportServer();
            return rps.DeviceDailyRpt(begintime, lineids,devicemodel);


        }
        /// <param name=""begintime"">开始日期(*)
        /// <param name=""endtime""> 结束日期(*)
        /// <param name=""groupid"">集团id
        /// <param name=""companyid"">公司id
        /// <param name=""foundryid"">车间id
        /// <param name=""ProductLineId"">产线id"	
        public string DevicePhaseRpt(string begintime, string endtime, int? groupid, int? companyid, int? foundryid, int? ProductLineId, string devicemodel)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);

            ReportServer rps = new ReportServer();
            return rps.DevicePhaseRpt(begintime, endtime, lineids,devicemodel);


        }

        /// <param name=""begintime"">查询月份(*)
        /// <param name=""endtime""> 结束日期(*)
        /// <param name=""groupid"">集团id
        /// <param name=""companyid"">公司id
        /// <param name=""foundryid"">车间id
        /// <param name=""ProductLineId"">产线id"	

        public string DeviceMonthRpt(string begintime, int? groupid, int? companyid, int? foundryid, int? ProductLineId, string devicemodel)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);
            ReportServer rps = new ReportServer();
            return rps.DeviceMonthRpt(begintime, lineids, devicemodel);

        }


        /// <param name=""begintime"">查询年份(*)
        /// <param name=""endtime""> 结束日期(*)
        /// <param name=""groupid"">集团id
        /// <param name=""companyid"">公司id
        /// <param name=""foundryid"">车间id
        /// <param name=""ProductLineId"">产线id"
        public string DeviceYearRpt(string begintime, int? groupid, int? companyid, int? foundryid, int? ProductLineId, string devicemodel)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);
            ReportServer rps = new ReportServer();
            return rps.DeviceYearRpt(begintime, lineids, devicemodel);
        }

        public string DeviceErrDailyRpt(string begintime, int? groupid, int? companyid, int? foundryid, int? ProductLineId, string devicemodel, int islisterr = 1)
        {

            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);

            ReportServer rps = new ReportServer();
            return rps.DeviceErrDailyRpt(begintime, lineids, devicemodel,islisterr);


        }
        /// <param name=""begintime"">开始日期(*)
        /// <param name=""endtime""> 结束日期(*)
        /// <param name=""groupid"">集团id
        /// <param name=""companyid"">公司id
        /// <param name=""foundryid"">车间id
        /// <param name=""ProductLineId"">产线id"	
        public string DeviceErrPhaseRpt(string begintime, string endtime, int? groupid, int? companyid, int? foundryid, int? ProductLineId, string devicemodel,int islisterr = 1)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);

            ReportServer rps = new ReportServer();
            return rps.DeviceErrPhaseRpt(begintime, endtime, lineids, devicemodel,islisterr);


        }

        /// <param name=""begintime"">查询月份(*)
        /// <param name=""endtime""> 结束日期(*)
        /// <param name=""groupid"">集团id
        /// <param name=""companyid"">公司id
        /// <param name=""foundryid"">车间id
        /// <param name=""ProductLineId"">产线id"	

        public string DeviceErrMonthRpt(string begintime, int? groupid, int? companyid, int? foundryid, int? ProductLineId, string devicemodel, int islisterr = 1)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);
            ReportServer rps = new ReportServer();
            return rps.DeviceErrMonthRpt(begintime, lineids, devicemodel, islisterr);

        }


        /// <param name=""begintime"">查询年份(*)
        /// <param name=""endtime""> 结束日期(*)
        /// <param name=""groupid"">集团id
        /// <param name=""companyid"">公司id
        /// <param name=""foundryid"">车间id
        /// <param name=""ProductLineId"">产线id"
        public string DeviceErrYearRpt(string begintime, int? groupid, int? companyid, int? foundryid, int? ProductLineId, string devicemodel, int islisterr = 1)
        {
            if (groupid == null && companyid == null && foundryid == null && ProductLineId == null)
                return Global.RETURN_ERROR("部门编号不能全部为空");
            ProductionLinesServer pls = new ProductionLinesServer();
            string lineids = Global.GetLinesStr(groupid, companyid, foundryid, ProductLineId);
            ReportServer rps = new ReportServer();
            return rps.DeviceErrYearRpt(begintime, lineids, devicemodel, islisterr);
        }
    }
}