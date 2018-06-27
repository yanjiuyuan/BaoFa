using Bussiness;
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
    public class DeviceInfoController : BaseController
    {
        // GET:DeviceInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查询设备列表
        /// </summary>
        /// <param name="ProductLineId">产线编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="GroupId">集团编号</param>
        /// <param name="FoundryId">车间编号</param>
        /// <param name="status">设备状态</param>
        /// <param name="KeyWord">关键词</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">页条数</param>
        /// <returns>json数组</returns>
        //  /DeviceInfo/GetDeviceList?ProductLineId=1
        public string  GetDeviceList(int? ProductLineId, int? CompanyId, int? GroupId, int? FoundryId,
            int? status, string KeyWord, int? PageIndex = 0, int? PageSize = 10)
        {
              DevicesServer dev = new DevicesServer();
            return dev.GetDeviceList(ProductLineId, CompanyId, GroupId, FoundryId, status, KeyWord, PageIndex, PageSize);

        }

        /// <summary>
        /// 设备添加
        /// </summary>
        /// <param name="DeviceId">机器编号</param>
        /// <param name="DeviceName">机器名</param>
        /// <param name="DeviceType">机器类型 1：胶站 2：视觉站 3：压机 0: 其他</param>
        /// <param name="DeviceModel">设备型号</param>
        /// <param name="ComAddress">通信地址</param>
        /// <param name="ProductLineId">产线编号</param>
        /// <param name="LocationId">工位编号</param>
        /// <param name="OnlineDate">上线日期</param>
        /// <param name="OfflineDate">下线日期</param>
        /// <param name="DeviceStat">设备状态 0:未启用/停用1:已启用2:维护中3:报废</param>
        /// <returns>json</returns>
        /// /DeviceInfo/DeviceAdd?DeviceId=111&DeviceName=222&DeviceType=1&DeviceModel=de33s3&ComAddress=0.0&ProductLineId=1&LocationId=23&DeviceStat=1


        public string DeviceAdd(string DeviceId,string DeviceName,string DeviceType,string DeviceModel,string ComAddress
            , int? ProductLineId,int? LocationId,string OnlineDate,string OfflineDate,int? DeviceStat )
        {
            DevicesServer dev = new DevicesServer();
           
            string oper = string.Empty;
                if (HttpContext.Session["user"] != null)
            oper = (HttpContext.Session["user"] as SessionUser).username;
            return dev.DeviceAdd(DeviceId, DeviceName, DeviceType, DeviceModel, ComAddress, ProductLineId, LocationId, OnlineDate, OfflineDate, DeviceStat,oper );


        }
        /// <summary>
        /// 设备修改E:\BF4\DingTalk\Controllers\AccountController.cs
        /// </summary>
        /// <param name="DeviceId">机器编号</param>
        /// <param name="DeviceName">机器名</param>
        /// <param name="DeviceType">机器类型 1：胶站 2：视觉站 3：压机 0: 其他</param>
        /// <param name="DeviceModel">设备型号</param>
        /// <param name="ComAddress">通信地址</param>
        /// <param name="ProductLineId">产线编号</param>
        /// <param name="LocationId">工位编号</param>
        /// <param name="OnlineDate">上线日期</param>
        /// <param name="OfflineDate">下线日期</param>
        /// <param name="DeviceStat">设备状态 0:未启用/停用1:已启用2:维护中3:报废</param>
        /// <returns>json</returns>
        /// /DeviceInfo/DeviceSave?DeviceId=111&DeviceName=GG222&DeviceType=1&DeviceModel=de33s3&ComAddress=0.0&ProductLineId=1&LocationId=23&DeviceStat=1


        public string DeviceSave(string DeviceId, string DeviceName, string DeviceType, string DeviceModel, string ComAddress
          , int?  ProductLineId, int?  LocationId, string OnlineDate, string OfflineDate, int ?DeviceStat)
        {
            DevicesServer dev = new DevicesServer();

            string oper = string.Empty;
            if (HttpContext.Session["user"] != null)
                oper = (HttpContext.Session["user"] as SessionUser).username;
            return dev.DeviceSave(DeviceId, DeviceName, DeviceType, DeviceModel, ComAddress, ProductLineId, LocationId, OnlineDate, OfflineDate, DeviceStat, oper);


        }

        /// <summary>
        /// 设备删除
        /// </summary>
        /// <param name="DeviceId">机器编号</param>
        /// <returns></returns>
        /// /DeviceInfo/DeviceDel?DeviceId=111
        public string DeviceDel(string DeviceId )
        {
            DevicesServer dev = new DevicesServer();
            
            return dev.DeviceDel(DeviceId);


        }

    }
}