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
using System.Collections;

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
            strSql = "select a.* , if ( b.stationstate is null,'停止',  b.stationstate) as stationstate from "
           + " (select  *  , '"+jzName+ "' as stationName from  `" + strTableName + "`    where id_usage = (select max(id_usage) from `usage` a where a.productlineid = " + lineid+"   )  order by "+ strTableName+"id desc limit 1) a"
           + " left join(select  stationName, stationstate from LocationStateCache  where productlineid = " + lineid + "  "
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

        public Dictionary<string, string> getVstate(int lineid)
        {
            DataTable tb2 = new DataTable();
            DataTable tbline = new DataTable();
            DataTable tblineusage = new DataTable();
            Hashtable htable = new Hashtable();
            Dictionary<string, string> retdic = new Dictionary<string, string>();
            try
            {
              
 

                //获取压底和十字压参数
                string strlineusageSql = "select t1.*,t2.VisualOneState,t2.VisualOneError,t2.VisualTwoState,t2.VisualTwoError,t2.VisualThreeState,t2.VisualThreeError "+
                     " from(select *  from  `LineUsage`  where id_usage = (select max(id_usage) from `usage` a where a.productlineid = " + lineid + " )order by lineusageid desc limit 1)t1 left join  `Line` t2 on t1.id_usage =t2.id_usage ";
                tblineusage = MySqlHelper.ExecuteQuery(strlineusageSql);
                if(tblineusage.Rows.Count>0)
                { 
                retdic = JsonHelper.DataRowToDic(tblineusage.Columns, tblineusage.Rows[0]);

             int num = 0;
            StringBuilder sb = new StringBuilder();
            foreach( KeyValuePair< string, string>  x in Global.jq)
            {
                num++;
                sb.Append("'");
                sb.Append(x.Value);
                sb.Append("'"); 
                if(num!= Global.jq.Count)
                    sb.Append(",");
            }
               
                string strSqlstate = "select a.stationName,if(b.stationstate is null ,'停止', b.stationstate ) as stationstate  from  LocationCfg a left join " +
                    "locationstatecache b on a.stationName=b.stationName and a.ProductLineId=b.ProductLineId  where "
                + "a.ProductLineId = " + lineid + "  and "
                 + "a.stationName in ( "+ sb.ToString()+") ";
                tb2 = MySqlHelper.ExecuteQuery(strSqlstate);
                 for(int i=0;i< tb2.Rows.Count;i++)
                {
                    Dictionary<string, string> dic = JsonHelper.DataRowToDic(tb2.Columns, tb2.Rows[i]);
                    htable.Add(dic["stationName"], dic); 
                }
                    //压底
                    foreach (KeyValuePair<string, string> x in Global.jq)
                    {
                        if (htable.ContainsKey(x.Value))
                        {
                            KeyValuePair<string, string> kp = new KeyValuePair<string, string>(x.Key, ((Dictionary<string, string>)htable[x.Value])["stationstate"]);
                            retdic.Add(kp.Key, kp.Value);
                        }


                    }

                    }

                }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return retdic;
        }


    }
}
