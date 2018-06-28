using Common.DbHelper;
using Common.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
   public static class Global
    {
        private static Logger logger = Logger.CreateLogger(typeof(Global));
        public static Dictionary<string, List<string>> AccessList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> MenuList = new Dictionary<string, List<string>>();
        static Global()
         {
            AccessListInit();
         
         }

        private static void AccessListInit()
        {
            DataTable roledt = new DataTable();
            DataTable accessdt = new DataTable();

            try
            {
                //获取角色列表
                string strsql = " select roleid  from roleinfo where rolestat =1";


                roledt = MySqlHelper.ExecuteQuery(strsql);

                for (int i = 0; i < roledt.Rows.Count; i++)
                {
                    string roleid = roledt.Rows[i]["roleid"].ToString();
                    AccessList.Add(roleid, new List<string>());
                    MenuList.Add(roleid, new List<string>());
                    string strsqlR = " select accessid,menuid ,url from accessinfo where rolectl like  '%" + roleid + "%'";


                    accessdt = MySqlHelper.ExecuteQuery(strsqlR);
                    for (int j = 0; j < accessdt.Rows.Count; j++)
                    {
                        string urls = accessdt.Rows[j]["url"].ToString();
                        string menuid = accessdt.Rows[j]["menuid"].ToString();
                        foreach (string s in urls.Split(','))
                        {
                            if (!AccessList[roleid].Contains(s))
                                AccessList[roleid].Add(s                                                                                          );

                        }
                       
                        if (!MenuList[roleid].Contains(menuid))
                            MenuList[roleid].Add(menuid);
                    }


                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);

            }

            
        }


        public static string   RETURN_SUCESS = "{\"Success\":true,\"Msg\":\"操作成功!\"}";
        public  static string  RETURN_EMPTY = "{\"Success\":false,\"Msg\":\"暂无数据!\"}";
        public static string RETURN_ERROR(string msg)
        {
            string err_Str = "{\"Success\":false,\"Msg\":\"" + msg+"!\"}";
            return err_Str;
        }

        //胶站列表
        public static Dictionary<string, string> jz = new Dictionary<string, string>
        {   {"Vamp","鞋面喷胶"},
                {"WaiO","围条一胶"}, {"WaiT","围条二胶"}, {"WaiS","围条三胶"}, {"Outsole","大底喷胶"},
            { "Mouthguards","护齿喷胶"}

        };

        //其余机器列表
        public static Dictionary<string, string> jq = new Dictionary<string, string>
        {
            { "V1","视觉1号站"},{"SOLEP","压底"},{"V2","视觉2号站"},{"V3","视觉3号站"},{"TENP","十字压"} 

        };
        

        public static string GetCompanyNameByLineID(int lineid)
        {
            DataTable dt = new DataTable();
            string retstr = string.Empty;
            try
            {

                string strsql = " select  c.companyName  from productlineinfo a left join foundryinfo b  on a.foundryid = b.foundryid " +
                 " left join companyinfo c   on b.companyid = c.companyid  where a.ProductLineId=" + lineid;


                dt = MySqlHelper.ExecuteQuery(strsql);
                if (dt.Rows.Count > 0)
                {
                    retstr = dt.Rows[0]["companyName"].ToString();
                }


            }
            catch (Exception ex)
            {

                logger.Error(ex.Message);
            }
            return retstr;

        }
        public static string GetDbDate()
        {
            DataTable dt = new DataTable();
            string retstr = string.Empty;
            try
            {

                string strSql = "select   date_format(  curdate() ,'%Y-%m-%d') as nowday  ";


                dt = MySqlHelper.ExecuteQuery(strSql);
                if (dt.Rows.Count > 0)
                {
                    retstr = dt.Rows[0]["nowday"].ToString();
                }


            }
            catch (Exception ex)
            {

                logger.Error(ex.Message);
            }
            return retstr;

        }
        public static bool IsWeekInList(string datestr,string days)
        {
            DateTime dt = Convert.ToDateTime(datestr);
            int y = dt.Year;
            int m = dt.Month;
            int d = dt.Day;
            int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;
            week = week + 1;//上面计算的，星期一是0
            if (days.Contains(week.ToString()))
                return true;
            return false;

        }
        public static bool IsMonthLast(string datestr)
        {
            DateTime dt = Convert.ToDateTime(datestr);
            return ( dt== dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1));
             
        }

        public static bool IsYearLast(string datestr)
        {
            DateTime dt = Convert.ToDateTime(datestr);
            return (dt.Month==12 && dt == dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1));

        }
    }
}
