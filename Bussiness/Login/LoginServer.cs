﻿using Common.DbHelper;
using Common.Encrypt;
using Common.LogHelper;
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
        public  bool ChekLogin(string strUserName, string strPassword)
        {
            int iResult = 0;
            try
            {
                strPassword = MD5Encrypt.Encrypt(strPassword);
                string strSql = string.Format("select username,password from huabao.userInfo where username='{0}' and password = '{1}' and  status='1'",
                    strUserName, strPassword);
                //int iResult= MySqlHelper.ExecuteSql(strSql);
                DataSet dataset = MySqlHelper.GetDataSet(strSql);
                iResult = dataset.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
               
            }
            if (iResult == 1)
                {
                    return true;
                }
             return false;
            
           
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
