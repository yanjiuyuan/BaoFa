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
        public  static Dictionary<int, Dictionary<string, double>> LocationBusyState = new Dictionary<int, Dictionary<string, double>>();

        private static Logger logger = Logger.CreateLogger(typeof(RobotData));

        static public List<int> datakeys = new List<int>();
        static public Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
        static LocationBusyData()
        {

            DataTable dt = MySqlHelper.ExecuteQuery("select productlineid from productlineinfo where status=1 order by productlineid");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                datakeys.Add(int.Parse(dt.Rows[i]["productlineid"].ToString()));
                LocationBusyState.Add(int.Parse(dt.Rows[i]["productlineid"].ToString()), new Dictionary<string, double>());

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
                    DateTime dtm = DateTime.Now.AddMinutes(-10);
                    long begintime = TimeHelper.ConvertDateTimeToInt(dtm);

                    foreach (int key in datakeys)
                    {
                         
                            string sql =
                                " select tt1.stationname, round(tt1.runt/if(tt1.freet=0,1,tt1.freet),2) as runrate from  ( " +
                         " select t.stationname, sum(if (t.stationstate = '运行',tm ,0))  as runt, sum(if (t.stationstate = '空闲',tm ,0))  as freet from  " +
                        "  (  select stationname, stationstate, (TIMESTAMPDIFF(SECOND, '1970-1-1 08:00:00', NOW()) * 1000 - starttime) as tm  " +
               "   from locationstate where starttime > " + begintime + " and productlineid = " + key +
              "   union  select stationname, stationstate, (TIMESTAMPDIFF(SECOND, '1970-1-1 08:00:00', NOW()) * 1000 - starttime) as tm  " +
              "    from locationstatecache where productlineid = " + key + "  ) t where stationname in " +
             "   (select stationname from locationcfg where productlineid = " + key + " and state = 1)  group by stationname ) tt1 ";


                            Dictionary<string, double> dic = new Dictionary<string, double>();
                            DataTable dt = MySqlHelper.ExecuteQuery(sql);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string stationname = dt.Rows[i]["stationname"].ToString();
                                double runrate = Convert.ToDouble(dt.Rows[i]["runrate"].ToString());
                                dic.Add(stationname, runrate);


                            }
                            LocationBusyState[key] = dic;

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

 