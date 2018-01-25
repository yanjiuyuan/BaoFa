using Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebZhongZhi.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.needLogin = false;
            return View();
        }


        /// <summary>
        /// 登入接口
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <returns></returns>
        public bool CheckLogin(string strUserName, string strPassword)
        {
            LoginServer loginServer = new LoginServer();
            return loginServer.ChekLogin(strUserName, strPassword);
        }
    }
}