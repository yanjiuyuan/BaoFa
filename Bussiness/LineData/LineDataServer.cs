using Common.DbHelper;
using Common.JsonHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.LogHelper;
namespace Bussiness.LineData
{
    public class LineDataServer
    {
        private static Logger logger = Logger.CreateLogger(typeof(LineDataServer));
        public string GetSprayMessage()
        {
            string strJsonString = string.Empty;

            try
            {
                string strSql = "SELECT * FROM huabao.`spray` WHERE orderid=(SELECT orderid  FROM huabao.`usage` ORDER BY CT DESC LIMIT 0,1);";

                DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
                
                strJsonString = JsonHelper.DataTableToJson(newTb);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
            return strJsonString;
        }

        public DataTable GetTableMessage(string strTableName ,int lineid)
        {
            DataTable newTb = new DataTable();
            try
            {
                string strSql = string.Empty;
                string jzName = Global.jz[strTableName];
            strSql = "select a.* ,b.stationstate from "
           +" (select*  , '"+jzName+"' as stationName from  `vamp`    where id_usage = (select max(id_usage) from `usage` a where a.productlineid = "+lineid+"   )  order by vampid desc limit 1) a"
           + " left join(select  stationName, stationstate from LocationState  where id_usage = (select max(id_usage) from `usage` a where a.productlineid =  " + lineid + " ) "
           + "  and stationName = '" + jzName+"' order by starttime desc limit 1) b on a.stationName = b.stationName"

                     ;

                //}
                newTb = MySqlHelper.ExecuteQuery(strSql);
            
            }
            catch (Exception ex)
            {
                logger.Error( ex.Message);
               
            }
            return newTb;
        }

        public DataTable GetLocationState()
        {
            DataTable newTb = new DataTable();
            try
            {
                string strSql = string.Format("SELECT * FROM `LocationState` WHERE endtime IS NULL");
                newTb = MySqlHelper.ExecuteQuery(strSql);
          
            }
            catch (Exception ex)
            {
                logger.Error( ex.Message);
               
            }
            return newTb;
        }

        public  DataTable getVstate(int lineid)
        {
            DataTable tb2= new DataTable ();
            try
            {
                string strSqlstate = "select a.stationName,a.stationstate from LocationState a inner join( "
          + " select stationName, max(starttime) as starttime from LocationState  where "
         + "ProductLineId = " + lineid + " and id_usage = (select max(id_usage) from `usage` where ProductLineId = " + lineid + ") and "
          + " stationName like  '视觉%' group by stationName) t on  a.starttime = t.starttime and a.stationName = t.stationName   ";
                tb2 = MySqlHelper.ExecuteQuery(strSqlstate);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return tb2;
        }


    }
}
