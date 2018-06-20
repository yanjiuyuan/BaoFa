using Common.DbHelper;
using Common.Encrypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.LogHelper;
using Newtonsoft.Json;

namespace Bussiness.Register
{
    public class RegisterServer
    {
        private static Logger logger = Logger.CreateLogger(typeof(RegisterServer));
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <returns></returns>
        public static bool CheckUserName(string strUserName)
        {
            int iResult = 0;
            try { 
            string strSql=string.Format("select * from  huabao.userinfo where username='{0}'", strUserName);
            DataSet dataset = MySqlHelper.GetDataSet(strSql);
              iResult = dataset.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            if (iResult > 0) 
            {
                return false;  //用户名存在
            }
            else
            {
                return true;
            }
            
         
        }


        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strRealName"></param>
        /// <param name="strphoneNumber"></param>
        /// <param name="strPassword"></param>
        /// <param name="strRegisterTime"></param>
        /// <param name="iStatus"></param>
        /// <param name="strAddress"></param>
        /// <param name="iRole"></param>
        /// <param name="strProvince"></param>
        /// <param name="strCity"></param>
        /// <param name="strTelephone"></param>
        /// <param name="strOtherContact"></param>
        /// <param name="strFax"></param>
        /// <param name="iIsActive"></param>
        /// <param name="strLastLoginIp"></param>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static bool  Register(string strUserName, string strRealName,
            string strphoneNumber, string strPassword, string strRegisterTime,string strLastLoginTime,
             string strAddress, string strProvince, string strCity,
            string strTelephone, string strOtherContact, string strFax,
            string strLastLoginIp, string strEmail, string strCompanyId, string strCompanyName, int? iStatus=1, string iRole="01",int? IsActive=1)
        {
            bool bResult = false;
            try
            {
                strRegisterTime = DateTime.Now.ToString();
                //进行MD5 加密
                strPassword = MD5Encrypt.Encrypt(strPassword);
                string strSql = string.Format("insert into huabao.userinfo set " +
                    "userName='{0}',realName='{1}',phoneNumber='{2}',password='{3}'," +
                    "registertime='{4}',lastLoginTime='{5}',status='{6}',address='{7}'," +
                    "role='{8}',province='{9}',city='{10}',telephone='{11}',otherContact='{12}'," +
                    "fax='fax',isActive='{14}',lastLoginIp='{15}',email='{16}',CompanyId='{17}',CompanyName='{18}'", strUserName, strRealName,
                 strphoneNumber, strPassword, strRegisterTime, strLastLoginTime,
                 iStatus, strAddress, iRole, strProvince, strCity,
                 strTelephone, strOtherContact, strFax, IsActive,
                 strLastLoginIp, strEmail, strCompanyId, strCompanyName);
                 bResult = MySqlHelper.ExecuteSql(strSql) == 1 ? true : false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return bResult;
            
}

        //修改角色
        public static string RoleChange(string strUserName,  string strCompanyId, string strCompanyName,  string iRole = "01" )
        {
            int bResult = 0;
            try
            {
                string strRegisterTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //进行MD5 加密
             

                string strSql = string.Format("update huabao.userinfo set " +
                 "  role='{0}' ,CompanyId='{1}',CompanyName='{2}'  where  userName='{3}'",   iRole, strCompanyId, strCompanyName,strUserName);
                bResult = MySqlHelper.ExecuteSql(strSql);
                if (bResult == 1)
                    return Global.RETURN_SUCESS;
                else
                    return Global.RETURN_EMPTY;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);

            }
          

        }


        //获取用户列表
        public static string GetUserList(string keyword, int? GroupId , int? CompanyId , int? FoundryId , int? LineId )
        {

            string querysql = string.Empty;
            if (LineId != null)
                querysql = "select * from userinfo where role='04' and Companyid=" + LineId;
            else if (FoundryId != null)
            {
                querysql = "select * from userinfo where role='03' and Companyid=" + FoundryId
                    + " union select * from userinfo where role='04' and Companyid in (select ProductLineId" +
                    " from productlineinfo where foundryid="+ FoundryId+")";
                 
            }
            else if (CompanyId != null)
            {
                querysql = "select * from userinfo where role='02' and Companyid=" + CompanyId
                    + " union select * from userinfo where role='03' and Companyid in (select FoundryId" 
                    + " from foundryinfo where companyid=" + CompanyId + ") "
                    +" union select * from userinfo where role='04' and Companyid in (select ProductLineId"  
                    + " from productlineinfo where foundryid in (select foundryid from foundryinfo where companyid=" + CompanyId + ")) ";

            }
            else if (GroupId != null)
            {
                querysql = "select * from userinfo where role='01' and Companyid=" + GroupId
                    + " union select * from userinfo where role='02' and Companyid in (select companyid"
                    + " from companyinfo where groupid=" + GroupId + ") "
                    + " union select * from userinfo where role='03' and Companyid in (select foundryid"
                    + " from foundryinfo where companyid in (select companyid from companyinfo where GroupId=" + GroupId + ")) "
                     + " union select * from userinfo where role='04' and Companyid in (select ProductLineId"
                    + " from productlineinfo where foundryid in (select foundryid from foundryinfo where companyid in"
                    + " (select   companyid from companyinfo where GroupId=" + GroupId + ")) ) ";
                 
            }
            else
            {
                querysql = "select * from userinfo ";


            }
            if(keyword !=null)
            {
                querysql = "select * from (" + querysql + ") as a where username like '%" + keyword + "%' or companyname like '%" + keyword + "%'";

            }

            DataTable dt = MySqlHelper.ExecuteQuery(querysql);

            return JsonConvert.SerializeObject(dt);
        }


        public static string PwdChange(string username, string oldpwd, string newpwd)
        {
            int bResult = 0;
            try
            {
                string strRegisterTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //进行MD5 加密

               string stroldpsw = MD5Encrypt.Encrypt(oldpwd);
                string strSql = string.Format("select  password   from huabao.userInfo where username='{0}' ",
                    username);

                //int iResult= MySqlHelper.ExecuteSql(strSql);
                DataTable dt = MySqlHelper.ExecuteQuery(strSql);
                if(dt.Rows.Count<1)
                    return Global.RETURN_EMPTY;
                if(!stroldpsw.Equals(dt.Rows[0]["password"].ToString()))
                    return Global.RETURN_ERROR("旧密码校验失败");
                string strnewpsw = MD5Encrypt.Encrypt(newpwd);


                string strSql1 = string.Format("update huabao.userinfo set " +
                 "  password='{0}'  where  userName='{1}'", strnewpsw, username);
                bResult = MySqlHelper.ExecuteSql(strSql1);
                if (bResult == 1)
                    return Global.RETURN_SUCESS;
                else
                    return Global.RETURN_EMPTY;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);

            }


        }
    }
}
