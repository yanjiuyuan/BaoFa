using Common.DbHelper;
using Common.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
   public static class Global
    {
        private static Logger logger = Logger.CreateLogger(typeof(Global));
        public static bool busy_statics = true;
        public static string robotRun = "0";
        public static string robotStop = "1";
        public static Dictionary<string, RobotState> rootstatelist = new Dictionary<string, RobotState>();
        public static Dictionary<string, List<string>> AccessList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> MenuList = new Dictionary<string, List<string>>();
        static Global()
         {
            AccessListInit();
         
         }
        public static string GetLocalIP()
          {
             try
             {
                 string HostName = Dns.GetHostName(); //得到主机名
                 IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                 for (int i = 0; i<IpEntry.AddressList.Length; i++)
                 {
                     //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                     //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                     if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                     {
                         return IpEntry.AddressList[i].ToString();
                     }
                 }
                 return "";
             }
             catch (Exception ex)
             {
                 return ex.Message;
             }
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
        {   {"压底","BOilPressure"},
                {"左十字压","LOilPressure"}, {"右十字压","ROilPressure"} 

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
        
        //根据角色代码，获取该登录人员的机构级别
        public static int GetDepLevelByRole(string role)
        {
            int level = 0;
            switch (role)
             {
                case "01"://管理员
                    level = 0;
                    break;
                case "02"://集团用户
                    level = 1;
                    break;
                case "03"://公司用户
                    level = 2;
                    break;
                case "04"://车间用户
                    level = 3;
                    break;
                default:
                    level = 3;
                    break;

            }

            return level;

        }
        public static   string  GetBrnoDepthByDepartID(int deplevel, string brno)
        {
            string BrnoDepth = string.Empty;
            DataTable dt = new DataTable();
            if (deplevel ==0)
                BrnoDepth = "00,";
            else
            {
                //查询归属的集团的BrnoDepth
               string strsql = " select BrnoDepth from branchinfo where  Level=" + deplevel + " and  Brno = '" + brno + "'";
                dt = MySqlHelper.ExecuteQuery(strsql);
                if (dt.Rows.Count >= 1)
                {
                    //存在
                    string depth = dt.Rows[0]["BrnoDepth"].ToString();
                    string[] depths = depth.Split(',');
                    if (depths.Length >= 2)
                        BrnoDepth = depths[0] + "," + depths[1];
                    //得到机构层级，接下来查询所有该机构及下属机构的机构关系 

                }
            }
            return BrnoDepth;
        }

        public static  int  GetCurrUsageId(int lineid)
        {
            string datestr = Global.GetDbDate();
            string strSql = string.Empty;
            strSql = " select max(id_usage)  from `usage` a where a.productlineid = " + lineid + " and  CT like '" + datestr + "%' group by null";
           DataTable newTb = MySqlHelper.ExecuteQuery(strSql);

            if (newTb.Rows.Count == 0)
                return 0;
            else
                return int.Parse(Convert.ToString(newTb.Rows[0][0]));

        }
        public static int GetLstUsageId(int lineid)
        {
             string strSql = string.Empty;
            strSql = " select max(id_usage)  from `usage` a where a.productlineid = " + lineid  ;
            DataTable newTb = MySqlHelper.ExecuteQuery(strSql);

            if (newTb.Rows.Count == 0)
                return 0;
            else
                return int.Parse(Convert.ToString(newTb.Rows[0][0]));

        }

        //获取胶站外的其他设备信息
        public static Dictionary <string,string> GetMachineStationsWithoutSpary(int lineid)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string strSql = string.Empty;

            strSql = "  select a.StationName,b.deviceid from `locationcfg` a " +
        " left join(select deviceid, LocationId from deviceinfo where productlineid = "+ lineid+") b on a.LocationId = b.LocationId " +
        " where a.productlineid =  " + lineid + " and a.JobType = '机器'  and a.SprayId is    null ";
 
            DataTable newTb = MySqlHelper.ExecuteQuery(strSql);

            if (newTb.Rows.Count == 0)
                return dic;
            else
            {
                for(int i=0;i< newTb.Rows.Count;i++)
                {
                    dic.Add(Convert.ToString(newTb.Rows[i][0]), Convert.ToString(newTb.Rows[i][1]));
                }
            }
            return dic;
        }


       public  static   List<int>  GetLineList(int? ProductLineId, int? CompanyId, int? GroupId, int? FoundryId)
        {

            List<int> list = new List<int>();
            string strSql = string.Empty;
            DataTable newTb = new DataTable();
            if (ProductLineId != null)
                list.Add((int)ProductLineId);
            else if(FoundryId!=null)
            {
               
                strSql = " select ProductLineId  from `productlineinfo` a where a.brno = " + FoundryId ;
                  newTb = MySqlHelper.ExecuteQuery(strSql);
                if(newTb.Rows.Count>0)
                {
                    foreach (DataRow dr in newTb.Rows)
                        list.Add(int.Parse(dr[0].ToString())); 
                } 
            }
            else if (CompanyId != null)
            {
                strSql = " select ProductLineId  from `productlineinfo` a where a.brno in (select Brno  from branchinfo where upbrno ='"+ CompanyId + "')  ";
                newTb = MySqlHelper.ExecuteQuery(strSql);
                if (newTb.Rows.Count > 0)
                {
                    foreach (DataRow dr in newTb.Rows)
                        list.Add(int.Parse(dr[0].ToString()));
                }

            }
            else if (GroupId !=null)
            {
                strSql = " select ProductLineId  from `productlineinfo` a  where Brno in (select Brno  from branchinfo where where  BrnoDept like '00," + GroupId + ",%' and Level=3)  ";
                newTb = MySqlHelper.ExecuteQuery(strSql);
                if (newTb.Rows.Count > 0)
                {
                    foreach (DataRow dr in newTb.Rows)
                        list.Add(int.Parse(dr[0].ToString()));
                }
            }

            else
            {
                strSql = " select ProductLineId  from `productlineinfo` a  ";
                newTb = MySqlHelper.ExecuteQuery(strSql);
                if (newTb.Rows.Count > 0)
                {
                    foreach (DataRow dr in newTb.Rows)
                        list.Add(int.Parse(dr[0].ToString()));
                }

            }
             
            return list;

        }
    }
}
