﻿using Bussiness;
using Common.Cookie;
using Newtonsoft.Json.Linq;
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
        public string  CheckLogin()
        {
            var strUserName = Request.Form["UserName"].ToString();
            var strPassword = Request.Form["Password"].ToString();
            LoginServer loginServer = new LoginServer();
            bool IsSuccess = false;
            string  rst = loginServer.ChekLogin(strUserName, strPassword);
            JObject json = JObject.Parse(rst);
            //下面我们读取JSON字符串的第一级里的任何值，如下
           if(json !=null && "true".Equals( json["Success"].ToString()))
            {
                //((System.Web.Mvc.Controller)(this)).Session["UserName"] = strUserName;
                //System.Web.HttpContext.Current.Session["UserName"] = strUserName;
                //System.Web.HttpContext.Current.Session["Role"] = loginServer.GetRole(strUserName);
                //System.Web.HttpContext.Current.Session["CompanyId"] = loginServer.GetCompanyId(strUserName);
                Bussiness.Model.SessionUser user = new Bussiness.Model.SessionUser
                {
                    username = json["username"].ToString(),
                    departid= Convert.ToInt32(json["departid"].ToString() ),
                    departname = json["departname"].ToString(),
                    roleid = json["role"].ToString(),
                    lstlogintime = json["LoginTime"].ToString()

                };


                System.Web.HttpContext.Current.Session["user"] = user;
                //var loginName = strUserName;
                //loginer.AuthToken = GetToken(loginName);
                //var data = JsonConvert.SerializeObject(loginer);
                CookieHelper.SetCookie("UserName", strUserName);
                CookieHelper.SetCookie("Role", loginServer.GetRole(strUserName).ToString());
                CookieHelper.SetCookie("CompanyId", loginServer.GetCompanyId(strUserName));
            }
            return rst;
        }
    }
}