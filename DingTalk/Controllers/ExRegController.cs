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
    public class ExRegController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }



        /// 测试数据：DataView/GetData?lineid=1
        public string GetWarnTypeList( string type)
        {
            string retstr = string.Empty;
            DataTable dt = (new WarnServer()).WarnTypeList(type);
            retstr= JsonConvert.SerializeObject(dt);

            return retstr;
        }
        public string GetProductLineList( )
        {
            string retstr = string.Empty;
            DataTable dt = (new WarnServer()).GetLineList();
            retstr = JsonConvert.SerializeObject(dt);

            return retstr;
        }

        public string GetLocationList(int lineid)
        {
            string retstr = string.Empty;
            DataTable dt = (new WarnServer()).GetLocationList(lineid);
            retstr = JsonConvert.SerializeObject(dt);

            return retstr;
        }


        public string GetWarnList(int lineid,string datestr)
        {
            string retstr = string.Empty;
            DataTable dt = (new WarnServer()).GetWarnList(lineid,datestr);
            retstr = JsonConvert.SerializeObject(dt);

            return retstr;
        }

        public string  WarnReg(int lineid, string productiont, int? locationid, string warntype
            , string warnphe, string treatment, string warntime, string opr,int ? warndura)
        {
            string retstr = string.Empty;
           int bResult = (new WarnServer()).WarnReg(lineid, productiont, locationid, warntype
            , warnphe, treatment, warntime, opr, warndura);
        
            if (bResult == 1)
                return Global.RETURN_SUCESS;
            else
                return Global.RETURN_ERROR("操作失败");
       
        }
    }
}