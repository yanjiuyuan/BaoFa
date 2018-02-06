using Bussiness.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebZhongZhi.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            ViewBag.needLogin = false;
            return View();
        }


        /// <summary>
        /// 注册接口（必填字段：验证码，用户名，密码，所属公司Id，所属公司名称）
        /// </summary>
        /// <param name="strCode">验证码(必填)</param>
        /// <param name="username">用户名(必填)</param>
        /// <param name="strRealName">真实姓名</param>
        /// <param name="strphoneNumber"></param>
        /// <param name="strPassword">密码(必填)</param>
        /// <param name="strRegisterTime">注册时间</param>
        /// <param name="strLastLoginTime">最后登入时间</param>
        /// <param name="iStatus">状态</param>
        /// <param name="strAddress">地址</param>
        /// <param name="iRole">用户角色
        //用户角色影响用户权限，用户权限大概有以下几种
        //默认权限 1 浏览平台基础信息，平台要展示给普通用户的一般信息。如产品介绍信息（普通用户）
        //2 浏览所有设备的运行状态，查看报表。（厂管理）
        //3 查看与操作所管理设备（设备管理）-》需要添加字段映射用户和设备
        //4 查看与操作所有设备（管理员）
        /// <param name="strProvince">省</param>
        /// <param name="strCity">市</param>
        /// <param name="strTelephone">电话</param>
        /// <param name="strOtherContact">其他概要</param>
        /// <param name="strFax">传真</param>
        /// <param name="iIsActive">是否活跃</param>
        /// <param name="strLastLoginIp">最后一次登入Ip</param>
        /// <param name="strEmail">邮箱</param>
        /// <param name="strCompanyId">公司Id</param>
        /// <param name="strCompanyName">公司名</param>
        /// 测试数据：Register/CheckRegister?username=sa&strPassword=123&strCompanyId=1&strCompanyName=华宝有限公司
        /// <returns></returns>
        public string CheckRegister(string strCode,string username, string strRealName,
            string strphoneNumber, string strPassword, string strRegisterTime, string strLastLoginTime,
           string strAddress, string strProvince, string strCity,
            string strTelephone, string strOtherContact, string strFax,
            string strLastLoginIp, string strEmail,string strCompanyId,string strCompanyName,int? iStatus = 1,int? iRole=1, int IsActive=1)
        {
            if (Session["Code"].ToString() != strCode)
            {
                return "{\"ErrorType\":1,\"ErrorMessage\":\"验证码有误!\"}";
            }
            if (username == null || strPassword == null || strCompanyId == null || strCompanyName==null)
            {
                return "{\"ErrorType\":2,\"ErrorMessage\":\"信息不完整!\"}";
            }
            if (RegisterServer.CheckUserName(username))
            {
                if (RegisterServer.Register(username, strRealName,
             strphoneNumber, strPassword, strRegisterTime, strLastLoginTime,
              strAddress, strProvince, strCity,
             strTelephone, strOtherContact, strFax,
             strLastLoginIp, strEmail,strCompanyId, strCompanyName,iStatus, iRole,IsActive))
                {
                    return "{\"ErrorType\":0,\"ErrorMessage\":\"注册成功!\"}";
                }
                else
                {
                    return "{\"ErrorType\":3,\"ErrorMessage\":\"注册失败,请联系管理员!\"}";
                }
            }
            else
            {
                return "{\"ErrorType\":4,\"ErrorMessage\":\"用户名已存在!\"}"; 
            }
        }
    }
}