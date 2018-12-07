using Bussiness.Chart;
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
    public static class  DataViewData
    {
       public  static void init()
        {

        }
        public  static Dictionary<int, string> lineViewData = new Dictionary<int, string>();
         
        private static Logger logger = Logger.CreateLogger(typeof(RobotData));
        static Timer time1;
        static public List<int > datakeys = new List<int> ();

        static DataViewData()
        {
            //读取code

            DataTable dt = MySqlHelper.ExecuteQuery("select productlineid from productlineinfo where status=1 order by productlineid");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                datakeys.Add(int.Parse(dt.Rows[i]["productlineid"].ToString()));
                lineViewData.Add(int.Parse(dt.Rows[i]["productlineid"].ToString()), "");

            }
            Task.Run(() => Query());
        }
        static async void Query()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(5000);
                   
                    foreach (int key in datakeys)
                    {
                         DataViewServer dv = new DataViewServer();
                         string viewstr= dv.GetData(key);

                         lineViewData[key] = viewstr;

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
