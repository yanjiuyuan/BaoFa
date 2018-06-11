using Bussiness.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 公司信息读取接口
        /// </summary>
        /// <returns></returns>
        /// 测试数据：/Company/GetCompanyInfo
        public string GetCompanyInfo(string groupid )
        {
            CompanyServer companyServer = new CompanyServer();
            return companyServer.GetAllCompanyInfo(groupid);
        }
    }
}