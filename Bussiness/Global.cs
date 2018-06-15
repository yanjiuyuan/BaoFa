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
        public  static string  RETURN_EMPTY =  "{\"ErrorType\":1,\"ErrorMessage\":\"暂无数据!\"}";
        public static string RETURN_ERROR(string msg)
        {
            string err_Str = "{\"ErrorType\":1,\"ErrorMessage\":\""+msg+"!\"}";
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


    }
}
