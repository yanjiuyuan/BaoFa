using Bussiness;
using Bussiness.Model;
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
        /// 注册接口（必填字段： 用户名 ，所属公司Id，所属公司名称）
        /// </summary>

        /// <param name="username">用户名(必填)</param>
        /// <param name="strRealName">真实姓名</param>
        /// <param name="strphoneNumber"></param>
        /// <param name="strAddress">地址</param>
        /// <param name="iRole">用户角色
        /// <param name="strProvince">省</param>
        /// <param name="strCity">市</param>
        /// <param name="strTelephone">电话</param>
        /// <param name="strOtherContact">其他概要</param>
        /// <param name="strFax">传真</param>
        /// <param name="strEmail">邮箱</param>
        /// <param name="strCompanyId">部门Id（产线id/公司id/集团id）</param>
        /// <param name="strCompanyName">部门名</param>
        /// 测试数据：Register/CheckRegister?username=sa&strCompanyId=1&strCompanyName=华宝有限公司
        /// <returns></returns>
        public string CheckRegister(string username, string strRealName,
            string strphoneNumber,  
           string strAddress, string strProvince, string strCity,
            string strTelephone, string strOtherContact, string strFax,
            string strEmail,string strCompanyId,string strCompanyName,string iRole )
        {
             
            if (username == null || iRole == null || strCompanyId == null || strCompanyName==null)
            {
                return  Global.RETURN_ERROR("信息不完整!");
            }
            if (RegisterServer.CheckUserName(username))
            {
                if (RegisterServer.Register(username, strRealName,
             strphoneNumber,  
              strAddress, strProvince, strCity,
             strTelephone, strOtherContact, strFax,
              strEmail,strCompanyId, strCompanyName ,iRole))
                {
                    return Global.RETURN_SUCESS;
                  
                }
                else
                {
                    return Global.RETURN_ERROR("添加失败!");
                   }
            }
            else
            {
                return Global.RETURN_ERROR("用户名已存在!"); 
            }
        }
        public string UserModify(string username, string strRealName,
            string strphoneNumber,  
           string strAddress, string strProvince, string strCity,
            string strTelephone, string strOtherContact, string strFax,
              string strEmail, string strCompanyId, string strCompanyName,   string iRole , int? status)
        {

            if (username == null || iRole == null || strCompanyId == null || strCompanyName == null)
            {
                return Global.RETURN_ERROR("信息不完整!");
            }
            if (RegisterServer.CheckUserName(username))
            {
                return Global.RETURN_ERROR("用户名不存在!");
            }
                if (RegisterServer.UserModify(username, strRealName,
             strphoneNumber, 
              strAddress, strProvince, strCity,
             strTelephone, strOtherContact, strFax,
              strEmail, strCompanyId, strCompanyName,  iRole , status))
                {
                    return Global.RETURN_SUCESS;

                }
                else
                {
                    return Global.RETURN_ERROR("添加失败!");
                }
             
            
        }
        //角色修改
        //测试 /Register/RoleChange?username=2222&strCompanyId=1&strCompanyName=第二车间&iRole=04
        public string RoleChange(string username,string strCompanyId, string strCompanyName, string iRole )
        {
             
            if (username == null ||   strCompanyId == null || strCompanyName == null || iRole == null)
            {
                return  Global.RETURN_ERROR("输入参数不完整");
            }
            if (RegisterServer.CheckUserName(username))
            {
                return Global.RETURN_ERROR("用户名不存在");
            }

            string rst = RegisterServer.RoleChange(username, strCompanyId, strCompanyName, iRole);
             
                    return rst;
                
        }


        //角色修改
        //测试 /Register/GetUserList?LineId=1&keyword=04
        public string GetUserList(string keyword, int? GroupId, int? CompanyId, int? FoundryId )
        {

             
            string rst = RegisterServer.GetUserList(keyword, GroupId, CompanyId, FoundryId );

            return rst;

        }

        //角色修改
        //测试 /Register/GetRoleList 
        public string GetRoleList()
        {


            string rst = RegisterServer.GetRoleList();

            return rst;

        }

        //密码修改
        //测试 /Register/RoleChange?username=2222&strCompanyId=1&strCompanyName=第二车间&iRole=04
        public string PwdChange(string username, string oldpwd, string newpwd )
        {
            if (username == null || oldpwd == null || newpwd == null  )
            {
                return Global.RETURN_ERROR("输入参数不完整");
            }
            if (RegisterServer.CheckUserName(username))
            {
                return Global.RETURN_ERROR("用户名不存在");
            }

            string rst = RegisterServer.PwdChange(username, oldpwd, newpwd);

            return rst;

        }

        //角色修改
        //测试 /Register/GetDepartList?LineId=1&keyword=04
        public string GetDepartList()
        {
            string role = "01";
            int departid = 0;
            if (HttpContext.Session["user"] != null)
            {
                role = (HttpContext.Session["user"] as SessionUser).roleid;
                departid = (HttpContext.Session["user"] as SessionUser).departid;
            }


            string rst = RegisterServer.GetDepartList(role, departid );

            return rst;

        }
    }
}