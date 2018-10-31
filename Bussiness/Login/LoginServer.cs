using Common.DbHelper;
using Common.Encrypt;
using Common.LogHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class LoginServer
    {
        private static Logger logger = Logger.CreateLogger(typeof(LoginServer));
        public  string ChekLogin(string strUserName, string strPassword)
        {
            string retstr = string.Empty;
            Dictionary<string, string> loginrst = new Dictionary<string, string>();
            int iResult = 0;
            try
            {
                strPassword = MD5Encrypt.Encrypt(strPassword);
                string strSql = string.Format("select username,password ,status,Brno,companyname,role from huabao.userInfo where username='{0}' ",
                    strUserName);
                //int iResult= MySqlHelper.ExecuteSql(strSql);
                DataTable dt = MySqlHelper.ExecuteQuery(strSql);
                if (dt.Rows.Count == 0)
                { 
                    loginrst.Add("Success", "false");
                    loginrst.Add("msg", "用户不存在!");
                }
                else
                {
                   string  dbpsd = dt.Rows[0][1].ToString();
                    string status = dt.Rows[0][2].ToString();
                    if(!"1" .Equals(status))
                    {
                        loginrst.Add("Success", "false");
                        loginrst.Add("msg", "用户已停用!");

                    }
                    else if(!dbpsd.Equals(strPassword))
                    {
                        loginrst.Add("Success", "false");
                        loginrst.Add("msg", "输入密码有误!");

                    }
                    else
                    {
                       
                        string updatesql = "update huabao.userInfo set lastLoginTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where   username='" + strUserName + "'";
                        iResult = MySqlHelper.ExecuteSql(updatesql);
                        if(iResult==1)
                        {
                            loginrst.Add("Success", "true");
                            loginrst.Add("msg", "登录成功!");
                            loginrst.Add("LoginTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            loginrst.Add("departid", dt.Rows[0][3].ToString());
                            loginrst.Add("departname", dt.Rows[0][4].ToString());
                            loginrst.Add("role", dt.Rows[0][5].ToString());
                            loginrst.Add("username", dt.Rows[0][0].ToString());
                        }
                        else
                        {
                            loginrst.Add("Success", "false");
                            loginrst.Add("msg", "登记末次登录时间失败!");
                               
                        }


                    }

                }

           
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
               
            }
            return JsonConvert.SerializeObject(loginrst);
            
           
        }


        /// <summary>
        /// role:用户角色
        //用户角色影响用户权限，用户权限大概有以下几种
        //1）浏览平台基础信息，平台要展示给普通用户的一般信息。如产品介绍信息（普通用户）
        //2）浏览所有设备的运行状态，查看报表。（厂管理）
        //3）查看与操作所管理设备（设备管理）-》需要添加字段映射用户和设备
        //4）查看与操作所有设备（管理员）
        /// </summary>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public  int GetRole(string strUserName)
        {
          
            int iRole = 0;
            try
            {
                string strSql = string.Format("select role from huabao.userInfo where username='{0}'", strUserName);
            DataSet dataset = MySqlHelper.GetDataSet(strSql);
            iRole= Int32.TryParse(dataset.Tables[0].Rows[0]["role"].ToString(),out iRole) ?iRole:0;
         
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                
            }
            return iRole;
        }

        /// <summary>
        /// 返回CompanyId，多公司职位以逗号隔开
        /// </summary>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public string GetCompanyId(string strUserName)
        {
            try { 
            string GetCompanyId = string.Empty;
            string strSql = string.Format("select CompanyId from huabao.userInfo where username='{0}'", strUserName);
            DataSet dataset = MySqlHelper.GetDataSet(strSql);
            GetCompanyId = dataset.Tables[0].Rows[0]["CompanyId"].ToString();
            return GetCompanyId;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }
    }
}
