﻿using Bussiness;
using Bussiness.Model;
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
    public class ProductionLinesController : BaseController
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

        public string ProductionLinesData(int? LineId, string OrderID
            , string CompanyId, string telephone, string registertime,
            string role, string status, string GroupId, string FoundryId,
            string IsEnable, string KeyWord
            , int? PageIndex = 0, int? PageSize = 5)
        {
            //if (Session["Role"].ToString() != "1")
            //{
            //    return "没有权限访问！";
            //}

            role = "01";
            string departid = "";
            if (HttpContext.Session["user"] != null)
            {

                role = (HttpContext.Session["user"] as SessionUser).roleid;
                departid = (HttpContext.Session["user"] as SessionUser).departid;
                if ("02".Equals(role))
                    GroupId = departid;
                if ("03".Equals(role))
                    CompanyId = departid;
                if ("04".Equals(role))
                    FoundryId = departid;


            }
            ProductionLinesServer pServer = new ProductionLinesServer();

            return pServer.GetProductionLinesData( LineId 
            , CompanyId, telephone, registertime, role,
            status, GroupId, FoundryId, IsEnable, KeyWord, PageIndex, PageSize);
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
            string role = "01";
            string departid = "";
            if (HttpContext.Session["user"] != null)
            {

                role = (HttpContext.Session["user"] as SessionUser).roleid;
                departid = (HttpContext.Session["user"] as SessionUser).departid;
            }

            ProductionLinesServer pLinesServer = new ProductionLinesServer();
            return JsonConvert.SerializeObject(pLinesServer.GetLinesList(role, departid));
        }
        /// 测试数据 /ProductionLines/GetLineTreeList
        public string GetLineTreeList()
        {
            string role = "01";
            string departid = "";
            if (HttpContext.Session["user"] != null)
            {
             
               role = (HttpContext.Session["user"] as SessionUser).roleid;
                departid = (HttpContext.Session["user"] as SessionUser).departid;
            }


            ProductionLinesServer pLinesServer = new ProductionLinesServer();
            return pLinesServer.GetLineTreeList(role, departid);
        }

        //产线生产计划设置
        // 测试数据 /ProductionLines/ProductionCalSet?ProductLineId=1&ProductionDays=1,2,3,4,5,6&ProductionBeat=600&ProductionShifts=1

        public string ProductionCalSet()
        {

            if (Request["ProductLineId"] == null || Request["ProductionBeat"] == null || Request["ProductionShifts"] == null || Request["ProductionDays"] == null || Request["ProductionTimes"] == null)
                return Global.RETURN_ERROR("参数不完整!");

            int ProductLineId = Convert.ToInt32(Request["ProductLineId"]);
            int ProductionBeat = Convert.ToInt32(Request["ProductionBeat"]);
            int ProductionShifts = Convert.ToInt32(Request["ProductionShifts"]);
            int ProductionTimes = Convert.ToInt32(Request["ProductionTimes"]);
            string ProductionDays = Convert.ToString( Request["ProductionDays"]);
            string oper= Convert.ToString(HttpContext.Session["user"]==null?"": (HttpContext.Session["user"] as SessionUser).username);
             

            ProductionLinesServer pLinesServer = new ProductionLinesServer();
            return  pLinesServer.ProductionCalSet(ProductLineId, ProductionBeat, ProductionShifts, ProductionDays, ProductionDays, oper);
        }

    }
}