using Bussiness;
using Common.Cookie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebZhongZhi.Controllers
{

    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            //System.Web.HttpContext.Current.Session["UserName"] = "abcd";
            ViewBag.needLogin = false;
            return View();
        }


        /// <summary>
        /// 登入接口
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <returns></returns>
        /// 测试数据： Login/CheckLogin?strUserName=123&strPassword=123
        [HttpPost]
        public bool CheckLogin()
        {
            var strUserName = Request.Form["UserName"].ToString();
            var strPassword = Request.Form["Password"].ToString();
            LoginServer loginServer = new LoginServer();
            bool IsSuccess = false;
            IsSuccess = loginServer.ChekLogin(strUserName, strPassword);
            if (IsSuccess)
            {
                //((System.Web.Mvc.Controller)(this)).Session["UserName"] = strUserName;
                //System.Web.HttpContext.Current.Session["UserName"] = strUserName;
                //System.Web.HttpContext.Current.Session["Role"] = loginServer.GetRole(strUserName);
                //System.Web.HttpContext.Current.Session["CompanyId"] = loginServer.GetCompanyId(strUserName);

                //var loginName = strUserName;
                //loginer.AuthToken = GetToken(loginName);
                //var data = JsonConvert.SerializeObject(loginer);
                CookieHelper.SetCookie("UserName", strUserName);
                CookieHelper.SetCookie("Role", loginServer.GetRole(strUserName).ToString());
                CookieHelper.SetCookie("CompanyId", loginServer.GetCompanyId(strUserName));
            }
            return IsSuccess;
        }
    }
}