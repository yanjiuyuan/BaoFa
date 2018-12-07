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
    public static class RobotData
    {

        private static Dictionary<int, Dictionary<int, int>> linestage = new Dictionary<int, Dictionary<int, int>>();

        private static Logger logger = Logger.CreateLogger(typeof(RobotData));

        static public List<string> datakeys = new List<string>();
        static public Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
        static RobotData()
        {

            DataTable dt = MySqlHelper.ExecuteQuery("select deviceid from deviceinfo");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data.Add(dt.Rows[i]["deviceid"].ToString(), new Dictionary<string, string>());
                Global.rootstatelist.Add(dt.Rows[i]["deviceid"].ToString(), new RobotState());
                datakeys.Add(dt.Rows[i]["deviceid"].ToString());
            }

            Task.Run(() => Query());


        }

        public static void init()
        {

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
        static async  void Query()
        {
            while(true)
            {
                try
                {
                    Thread.Sleep(200);
                    foreach (string key in datakeys)
                    {
                        try
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            RedisHelper redis = new RedisHelper();
                            dic = redis.GetHashAllKV(key);
                            data[key] = dic;
                            // DealData(key, dic); 
                        }
                        catch (Exception e)
                        {
                            logger.Info(e.Message);
                        }
                    }
                }
                catch(Exception e)
                {
                    logger.Info(e.Message);
                }
        }
    }
}}
