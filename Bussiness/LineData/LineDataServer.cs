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

        public Dictionary<string,   string> GetTableMessage(int sprayid,string sprayname, int lineid,string deviceid)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DataTable newTb = new DataTable();
            try
            {
                //获取当日最新id_usage ，如果没有，则表示当日产线无数据，不继续进行数据获取

                 string strSql = string.Empty;
                 

                int usageid = Global.GetCurrUsageId(lineid);
                
            strSql = "select a.* , if ( b.stationstate is null,'停止',  b.stationstate) as stationstate from "
           + " (select  *  , '"+ sprayname + "' as stationName from  `sprayrecd`   where ID_Usage = "+usageid+" and SprayId="+ sprayid + "  order by  ID desc limit 1) a"
           + " left join(select  stationName, stationstate from LocationStateCache  where productlineid = " + lineid + "  "
           + "  and stationName = '" + sprayname + "' order by starttime desc limit 1) b on a.stationName = b.stationName" ;
 
                newTb = MySqlHelper.ExecuteQuery(strSql);
            
            }
            catch (Exception ex)
            {
                logger.Error( ex.Message);
               
            }
            if(newTb.Rows.Count>0)
                dic= JsonHelper.DataRowToDic(newTb.Columns,newTb.Rows[0]);

            dic.Add("DeviceId", deviceid);
            return dic;
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

        public Dictionary<string, Dictionary<string, string>> getVstate(int lineid)
        {
            DataTable tb2 = new DataTable();
            DataTable tbline = new DataTable();
            DataTable tblineusage = new DataTable();
            Hashtable htable = new Hashtable();
            Dictionary<string, Dictionary<string, string>> retdic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> paradic = new Dictionary<string, string>();
            Dictionary<string, string> stationdic = new Dictionary<string, string>();
            try
            {
                int usageid = Global.GetCurrUsageId(lineid);


                //获取压底和十字压参数
                string strlineusageSql = "select t1.*,t2.VisualOneState,t2.VisualOneError,t2.VisualTwoState,t2.VisualTwoError,t2.VisualThreeState,t2.VisualThreeError "+
                     " from(select *  from  `LineUsagerecd`  where id_usage = " +usageid+" order by lineusageid desc limit 1)t1 left join  `Line` t2 on t1.id_usage =t2.id_usage ";
                tblineusage = MySqlHelper.ExecuteQuery(strlineusageSql);
                if (tblineusage.Rows.Count > 0)
                {
                    paradic = JsonHelper.DataRowToDic(tblineusage.Columns, tblineusage.Rows[0]);
                }
                    int num = 0;
                    StringBuilder sb = new StringBuilder();
                    Dictionary<string, string> dic = Global.GetMachineStationsWithoutSpary(lineid);


                    foreach (KeyValuePair<string, string> x in dic)
                    {
                        num++;
                        sb.Append("'");
                        sb.Append(x.Key);
                        sb.Append("'");
                        if (num != dic.Count)
                            sb.Append(",");
                    }

                    string strSqlstate = "select a.stationName,if(b.stationstate is null ,'停止', b.stationstate ) as stationstate  from  LocationCfg a left join " +
                        "locationstatecache b on a.stationName=b.stationName and a.ProductLineId=b.ProductLineId  where "
                    + "a.ProductLineId = " + lineid + "  and "
                     + "a.stationName in ( " + sb.ToString() + ") ";
                    tb2 = MySqlHelper.ExecuteQuery(strSqlstate);
                    for (int i = 0; i < tb2.Rows.Count; i++)
                    {
                        Dictionary<string, string> dic2 = JsonHelper.DataRowToDic(tb2.Columns, tb2.Rows[i]);
                        stationdic.Add(dic2["stationName"], dic2["stationstate"]);

                    }

                    foreach (KeyValuePair<string, string> kv in stationdic)
                    {
                        string stanm = kv.Key;
                        Dictionary<string, string> subdic = new Dictionary<string, string>();
                        subdic.Add("stationstate", kv.Value);
                        subdic.Add("DeviceId", dic[kv.Key]);

                        if (Global.jz.ContainsKey(stanm))
                        {
                            if(paradic.ContainsKey(Global.jz[stanm]))
                            subdic.Add(Global.jz[stanm], paradic[Global.jz[stanm]]);
                        }
                        retdic.Add(stanm, subdic);
                     
                }

                }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return retdic;
        }

        public SortedList<int, KeyValuePair<string, string>> GetLineSprayList(int lineid)
        {
            SortedList<int, KeyValuePair<string, string>> dic = new SortedList<int, KeyValuePair<string, string>>();
            try
            {

                string STRSQL =
                  "  select  a.StationName,a.SprayId,b.deviceid from locationcfg a "+
            " left join(select deviceid, LocationId from deviceinfo where productlineid=" + lineid + ") b on a.LocationId = b.LocationId " +
            " where ProductLineId = " + lineid + " and SprayId  is not null  ";


                 DataTable  tb2 = MySqlHelper.ExecuteQuery(STRSQL);
                for (int i = 0; i < tb2.Rows.Count; i++)
                {
                    int sprayid = 0;
                    string stationname = string.Empty;
                    string deviceid = string.Empty;
                    int.TryParse(Convert.ToString( tb2.Rows[i]["SprayId"]), out sprayid);
                    stationname = Convert.ToString(tb2.Rows[i]["StationName"]);
                    deviceid= tb2.Rows[i]["deviceid"] == null?"": Convert.ToString(tb2.Rows[i]["deviceid"]);
                    dic.Add(sprayid,  new KeyValuePair<string ,string >(  stationname, deviceid));
                }


            }

            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return dic;
        }
         
        }
}
