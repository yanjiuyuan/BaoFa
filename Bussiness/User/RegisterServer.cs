﻿using Common.DbHelper;
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
            try
            {
                string strSql = string.Format("select a.*,b.BrName from  huabao.userinfo a  left join branchinfo b on a.Brno=b.Brno where username='{0}'", strUserName);
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
        public static bool Register(string strUserName, string strRealName,
            string strphoneNumber,
             string strAddress, string strProvince, string strCity,
            string strTelephone, string strOtherContact, string strFax,
              string strEmail, string Brno, string iRole)
        {
            bool bResult = false;
            try
            {
                string strRegisterTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //进行MD5 加密
                string strPassword = MD5Encrypt.Encrypt("123456");
                string strSql = string.Format("insert into huabao.userinfo set " +
                    "userName='{0}',realName='{1}',phoneNumber='{2}',password='{3}'," +
                    "registertime='{4}',lastLoginTime='{5}',status='{6}',address='{7}'," +
                    "role='{8}',province='{9}',city='{10}',telephone='{11}',otherContact='{12}'," +
                    "fax='{13}',isActive='{14}',lastLoginIp='{15}',email='{16}',Brno='{17}' ",
                    strUserName, strRealName,
                 strphoneNumber, strPassword, strRegisterTime, "",
                 "1", strAddress, iRole, strProvince, strCity,
                 strTelephone, strOtherContact, strFax, "1",
                 "", strEmail, Brno );
                bResult = MySqlHelper.ExecuteSql(strSql) == 1 ? true : false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return bResult;

        }
        public static bool UserModify(string strUserName, string strRealName,
            string strphoneNumber, string strAddress, string strProvince, string strCity,
            string strTelephone, string strOtherContact, string strFax,
              string strEmail, string Brno,  string iRole, int? status)

        {
            bool bResult = false;
            try
            {

                string strSql = "update huabao.userinfo set " +
                    " role ='" + iRole + "' " +
                     " ,Brno ='" + Brno + "' "  ;
                if (strRealName != null)
                    strSql += " ,realName ='" + strRealName + "' ";
                if (strRealName != null)
                    strSql += " ,realName ='" + strRealName + "' ";
                if (strphoneNumber != null)
                    strSql += " ,phoneNumber ='" + strphoneNumber + "' ";
                if (strAddress != null)
                    strSql += " ,address ='" + strAddress + "' ";
                if (strProvince != null)
                    strSql += " ,province ='" + strProvince + "' ";
                if (strCity != null)
                    strSql += " ,city ='" + strCity + "' ";
                if (strTelephone != null)
                    strSql += " ,telephone ='" + strTelephone + "' ";
                if (strOtherContact != null)
                    strSql += " ,otherContact ='" + strOtherContact + "' ";
                if (strFax != null)
                    strSql += " ,fax ='" + strFax + "' ";
                if (strEmail != null)
                    strSql += " ,email ='" + strEmail + "' ";
                if (strEmail != null)
                    strSql += " ,email ='" + strEmail + "' ";
                if (status != null)
                    strSql += " ,status =" + status + " ";
                strSql += " where username='" + strUserName + "'";


                bResult = MySqlHelper.ExecuteSql(strSql) == 1 ? true : false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return bResult;

        }
        //修改角色
        public static string RoleChange(string strUserName, string Brno , string iRole = "01")
        {
            int bResult = 0;
            try
            {
                string strRegisterTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //进行MD5 加密


                string strSql = string.Format("update huabao.userinfo set " +
                 "  role='{0}' ,Brno='{1}'  where  userName='{2}'", iRole, Brno, strUserName);
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
        public static string GetUserList(string keyword, int? GroupId, int? CompanyId, int? FoundryId)
        {

            string querysql = string.Empty;
            if (FoundryId != null)
                querysql = "     select a.* from userinfo a where  a.Brno in (select Brno  from branchinfo where BrnoDepth like  CONCAT( (SELECT `BrnoDepth` FROM branchinfo WHERE Brno='" + FoundryId + "'), '%')) ";
            else if (CompanyId != null)
            {
                querysql = "     select a.*  from userinfo a where  a.Brno in (select Brno  from branchinfo where BrnoDepth like  CONCAT( (SELECT `BrnoDepth` FROM branchinfo WHERE Brno='" + CompanyId +"'), '%')) "  ;

            }

            else if (GroupId != null)
            {
                querysql = "     select a.*  from userinfo a where  a.Brno in (select Brno  from branchinfo where BrnoDepth like  CONCAT( (SELECT `BrnoDepth` FROM branchinfo WHERE Brno='" + GroupId + "'), '%')) ";


            }
            else
            {
                querysql = "select  *  from userinfo   ";


            }
            if (keyword != null)
            {
                querysql = "select tt1.*,tt2.BrName from (" + querysql + ") as tt1 left join branchinfo tt2 on tt1.Brno=tt2.Brno where username like '%" + keyword + "%' or BrName like '%" + keyword + "%' order by role,Brno";

            }
            else
            {
                querysql = "select tt1.*,tt2.BrName from (" + querysql + ") as tt1 left join branchinfo tt2 on tt1.Brno=tt2.Brno order by role,Brno";
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
                if (dt.Rows.Count < 1)
                    return Global.RETURN_EMPTY;
                if (!stroldpsw.Equals(dt.Rows[0]["password"].ToString()))
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
        public static string DleteUser(string username)
        {
            int bResult = 0;
            try { 

          string    strSql1 = "delete from userinfo where username='" + username + "'";
            
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



        public static string GetRoleList()
        {
            string querysql = string.Empty;

            querysql = "select roleid,rolename  from roleinfo where rolestat=1";

            DataTable dt = MySqlHelper.ExecuteQuery(querysql);

            return JsonConvert.SerializeObject(dt);
        }



        public static string GetDepartList(string role = "01", string brno = "01")
        {
            int deplevel = Global.GetDepLevelByRole(role);
            string BrnoDepth = string.Empty;
            string strsql = string.Empty;
            List<KeyValuePair<string, string>> DepartList = new List<KeyValuePair<string, string>>();

            //获取当前用户归属结构的机构层级关系
            try
            {
                DataTable dt = new DataTable();
                BrnoDepth = Global.GetBrnoDepthByDepartID(deplevel, brno);

                strsql = " select Brno,BrName,UpBrno,Level from branchinfo where  BrnoDepth like '" + BrnoDepth + "%' order by BrnoDepth";

                dt = MySqlHelper.ExecuteQuery(strsql);



                string lstgroupid = string.Empty;
                string lstfoundryid = string.Empty;
                string lstcompanyid = string.Empty;


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Level"].ToString() == "1")//集团
                    {
                        string groupid = dt.Rows[i]["Brno"].ToString();
                        string groupnm = dt.Rows[i]["BrName"].ToString();
                        DepartList.Add(new KeyValuePair<string, string>(groupid, groupnm));

                    }

                    if (dt.Rows[i]["Level"].ToString() == "2")//com
                    {
                        string companyid = dt.Rows[i]["Brno"].ToString();
                        string companynm = dt.Rows[i]["BrName"].ToString();
                        DepartList.Add(new KeyValuePair<string, string>(companyid, "&nbsp;&nbsp;" + companynm));


                    }
                    if (dt.Rows[i]["Level"].ToString() == "3")//com
                    {
                        string foundryid = dt.Rows[i]["Brno"].ToString();
                        string foundrynm = dt.Rows[i]["BrName"].ToString();
                        DepartList.Add(new KeyValuePair<string, string>(foundryid, "&nbsp;&nbsp;&nbsp;&nbsp;" + foundrynm));


                    }


                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            return JsonConvert.SerializeObject(DepartList );



        }
    }
}
