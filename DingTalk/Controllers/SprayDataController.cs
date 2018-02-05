using Bussiness.SprayData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class SprayDataController : Controller
    {
        // GET: SprayData
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 胶站数据读取接口
        /// </summary>
        /// <param name="OrderID">总订单编号</param>
        /// <param name="SprayID">胶站编号  围条一胶</param>
        /// <param name="ProductLineId">所属产线Id</param>
        /// <param name="GlueType">胶水类型</param>
        /// <param name="SprayDistance">喷距</param>
        /// <param name="GlueLinePosition">胶线位置</param>
        /// <param name="AtomizationAdjustment">雾化调节</param>
        /// <param name="MaterialAdjustment">物料调节</param>
        /// <param name="BakingTemperature">烘烤温度</param>
        /// <param name="BakingTime">烘烤时间</param>
        /// <param name="Keyword">关键字查询(目前支持：SprayID GlueType)</param>
        /// <param name="PageIndex">每数(默认0)</param>
        /// <param name="PageSize">每页条数(默认5)</param>
        /// <returns>返回Json数组</returns>
        /// 测试数据：/SprayData/GetSprayData?Keyword=白色
        public string GetSprayData(string OrderID,string SprayID,string 
            ProductLineId,string GlueType,string SprayDistance,string GlueLinePosition,
            string AtomizationAdjustment,string MaterialAdjustment,
            string BakingTemperature,string BakingTime,string KeyWord
            , int? PageIndex = 0, int? PageSize = 5)
        {
            SprayServer sprayServer = new SprayServer();
            return sprayServer.GetSprayData( OrderID,  SprayID, 
            ProductLineId,  GlueType,  SprayDistance,  GlueLinePosition,
             AtomizationAdjustment,  MaterialAdjustment,
             BakingTemperature,  BakingTime,  KeyWord
            , PageIndex,PageSize);
        }
    }
}