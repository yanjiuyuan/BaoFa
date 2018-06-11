using Bussiness.ProductionLines;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class ProductionLinesController : Controller
    {
        // GET: ProductionLines
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 产线信息总表接口
        /// </summary>
        /// <param name="ProductLineId">产线Id</param>
        /// <param name="OrderID">产线名</param>
        /// <param name="CompanyId">公司Id</param>
        /// <param name="telephone">固定电话</param>
        /// <param name="registertime">注册时间</param>
        /// <param name="role">角色</param>
        /// <param name="status">状态</param>
        /// <param name="IsEnable">是否活跃</param>
        /// <param name="KeyWord">查询关键字</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="PageSize">每页条数</param>
        /// <returns>Json数组</returns>
        /// 测试数据：/ProductionLines/ProductionLinesData?CompanyId=1&keyword=1号生产线

        public string ProductionLinesData(string ProductLineId, string OrderID
            , string CompanyId, string telephone, string registertime,
            string role, string status, string GroupId,
            string IsEnable, string KeyWord
            , int? PageIndex = 0, int? PageSize = 5)
        {
            //if (Session["Role"].ToString() != "1")
            //{
            //    return "没有权限访问！";
            //}
            ProductionLinesServer pServer = new ProductionLinesServer();

            return pServer.GetProductionLinesData(ProductLineId, OrderID
            , CompanyId, telephone, registertime, role,
            status, GroupId, IsEnable, KeyWord, PageIndex, PageSize);
        }

        public string GetGroupList()
        {
            ProductionLinesServer pLinesServer = new ProductionLinesServer();
            return pLinesServer.GetGroupList();
        }


        //获取生产线列表

        /// 测试数据 /ProductionLines/GetLineList
        public string GetLineList()
        {
            ProductionLinesServer pLinesServer = new ProductionLinesServer();
            return JsonConvert.SerializeObject(pLinesServer.GetLinesList());
        }


    }
}