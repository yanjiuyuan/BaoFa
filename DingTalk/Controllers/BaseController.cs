using Bussiness;
using Bussiness.Code;
using Bussiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class BaseController : Controller
    {

        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ////这一步是验证是否登陆  

            if (filterContext.HttpContext.Session["user"] == null)
            {
                filterContext.HttpContext.Response.Redirect("~/Login/Index", true);
                return;
            }
            //string roleid = (filterContext.HttpContext.Session["user"] as SessionUser).roleid;

            ////判断角色是否有该URL的访问权限

            //string accessURL = filterContext.HttpContext.Request.CurrentExecutionFilePath;
            // if(Global.AccessList[roleid] !=null)
            //{
            //    //不允许
            //    if(!Global.AccessList[roleid].Contains(accessURL))
            //    {
            //        //
            //        ErrorRedirect(filterContext);
            //    }
                 
            //}
              
            //ContentResult content = new ContentResult();
            //content.Content = "{\"ErrorType\":1,\"ErrorMessage\":\"暂无数据!\"}";
            //filterContext.Result = content;
            // 错误处理方法  

        }
        private void ErrorRedirect(ActionExecutingContext filterContext)
        {
            //注：由于前端页面用的Iframe框架，用下面的方法跳转的时候会跳转到iframe里面去，所以不能用注释掉的方法  
            // filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Home", action = "Index" }));  
            // filterContext.Result = new RedirectResult("/Home/Index");  
            ContentResult content = new ContentResult();
            content.Content = "<script language='javascript'> top.location.href = '/Home/Index';</script>";
            filterContext.Result = content;
        } // end ErrorRedirect 
    }
}