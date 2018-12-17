using Bussiness.Time;
using Common.DbHelper;
using Common.LogHelper;
using Newtonsoft.Json;
using RedisHelp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bussiness
{
    //机器人数据缓存
    public static class LocationBusyData
    {
        public static void init()
        {

        }
        public  static Dictionary<int, Dictionary<string, int>> LocationBusyState1 = new Dictionary<int, Dictionary<string, int>>();
        public static Dictionary<int, Dictionary<string, int>> LocationBusyState2 = new Dictionary<int, Dictionary<string, int>>();

        private static Logger logger = Logger.CreateLogger(typeof(RobotData));

        static public List<int> datakeys = new List<int>();
        static public Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
        static LocationBusyData()
        {

            DataTable dt = MySqlHelper.ExecuteQuery("select productlineid from productlineinfo where status=1 order by productlineid");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                datakeys.Add(int.Parse(dt.Rows[i]["productlineid"].ToString()));
                LocationBusyState1.Add(int.Parse(dt.Rows[i]["productlineid"].ToString()), new Dictionary<string, int>());
                LocationBusyState2.Add(int.Parse(dt.Rows[i]["productlineid"].ToString()), new Dictionary<string, int>());

            }

            Task.Run(() => Query());


        }



        public static string DataToStr<T>(Dictionary<string, T> DIC)
        {
            StringBuilder str = new StringBuilder();
            str.Append("{");
            List<string> keys = data.Keys.ToList<string>();
            for (int i = 0; i < keys.Count; i++)
            {
                str.Append("\"" + keys[i] + "\":");
                T item = DIC[keys[i]];
                str.Append(DicToStr(item as Dictionary<string, string>));
                if (i != keys.Count - 1)
                    str.Append(",");
            }

            str.Append("}");
            return str.ToString();
        }
        static string DicToStr(Dictionary<string, string> DIC)
        {
            StringBuilder str = new StringBuilder();
            str.Append("{");
            List<string> keys = DIC.Keys.ToList<string>();
            for (int i = 0; i < keys.Count; i++)
            {
                str.Append("\"" + keys[i] + "\":");
                str.Append("\"" + DIC[keys[i]] + "\"");
                if (i != keys.Count - 1)
                    str.Append(",");
            }
            str.Append("}");
            return str.ToString();
        }
        static async void Query()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(10000);
                    Decimal  begintime = 0;
                    string qrysql = "  select (TIMESTAMPDIFF(SECOND, '1970-1-1 08:00:00', NOW())-300) * 1000  as begintime";
                    DataTable datedt = MySqlHelper.ExecuteQuery(qrysql);
                    if (datedt.Rows.Count > 0)
                        begintime = Convert.ToDecimal( datedt.Rows[0]["begintime"].ToString());
                    else
                    {
                     DateTime dtm = DateTime.Now.AddMinutes(-5);
                        begintime = TimeHelper.ConvertDateTimeToInt(dtm);
                    }
                    foreach (int key in datakeys)
                    {

                        string sql =
                   "select tt2.locationid, tt2.stationname, if( runc>0,runc,0) as runc from locationcfg tt2  left join (  select t.productlineid,  "+
            " t.stationname, sum(if (t.stationstate = '运行',1 ,0))  as runc  from(select stationname, stationstate, starttime, productlineid  " +
            "  from locationstate where starttime >  " + begintime + "  and productlineid = " + key+"   union  select stationname, stationstate, starttime, productlineid  " +
            " from locationstatecache where productlineid = 1  ) t where stationname in    (select stationname from locationcfg where productlineid =   "+ key+" and state = 1)  " +
            "  group by productlineid ,stationname ) tt1  " +
            " on tt1.productlineid = tt2.productlineid and tt1.stationname = tt2.stationname  where tt2.state = 1  order by tt2.locationid";
                        Dictionary<string, int> dic2 = new Dictionary<string, int>();
                        Dictionary<string, int> dic = new Dictionary<string, int>();
                            DataTable dt = MySqlHelper.ExecuteQuery(sql);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int  locationid = Convert.ToInt32(dt.Rows[i]["locationid"].ToString());
                            string stationname = dt.Rows[i]["stationname"].ToString();
                            int runc = Convert.ToInt32(dt.Rows[i]["runc"].ToString());
                            if (locationid<=11)
                                dic.Add(stationname, runc);


                            }
                        for (int i = dt.Rows.Count-1; i >0; i--)
                        {
                            int locationid = Convert.ToInt32(dt.Rows[i]["locationid"].ToString());
                            string stationname = dt.Rows[i]["stationname"].ToString();
                            int runc = Convert.ToInt32(dt.Rows[i]["runc"].ToString());
                            if (locationid > 11)
                                dic2.Add(stationname, runc);


                        }
                        LocationBusyState1[key] = dic;
                        LocationBusyState2[key] = dic2;

                        // DealData(key, dic); 
                    }

                }
                catch (Exception e)
                {
                    logger.Info(e.Message);
                }
            }

          }
        }
    }

 