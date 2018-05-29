using Bussiness.Time;
using Common.Code;
using Common.DbHelper;
using Common.JsonHelper;
using Common.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bussiness.Chart
{

    public class ChartBeatServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(ChartBeatServer));
        //获取各工位的末次生产时间
        public string ChartBeatQuery(int mins=60)
        {
            logger.Info("查询生产节拍");
            int qmins = mins * -1;

            //获取前一个小时时间点
            long begintime = TimeHelper.ConvertDateTimeToInt(DateTime.Now.AddMinutes(qmins));
            //string dt = TimeHelper.GetDateTimeFrom1970Ticks(1527044472080/1000).ToString();
            string strSql = "    select stationNAME, round(run_t /if (run_c > 0,run_c,1))as run,round(free_t /if (free_c > 0,free_c,1)) as free," +
            "round(warn_t /if (warn_c > 0,warn_c,1))as warn from(" +
           " select   sum( if (stationstate = '运行', endtime - startTime, 0)) AS run_t," +
           "sum( if (stationstate = '运行',1, 0))  AS run_c," +
            " sum( if (stationstate = '空闲', endtime - startTime, 0)) AS free_t," +
              " sum( if (stationstate = '空闲',1, 0))  AS free_c," +
             " sum( if (stationstate = '报警', endtime - startTime, 0)) AS warn_t," +
              "  sum( if (stationstate = '报警',1, 0))  AS warn_c , stationNAME" +
             " from huabao.LocationState where starttime > " + begintime + " group by stationNAME ) as t1 order by stationNAME";
          
            string strJsonString = string.Empty;
            try
            {
                DataTable newTb = MySqlHelper.ExecuteQuery(strSql);

                strJsonString = JsonHelper.DataTableToJson(newTb);
            }
            catch (Exception ex)
            {
                logger.Error("查询生产节拍失败" + ex.Message);
                return strJsonString;
            }
            logger.Info("查询生产节拍结束");
            return strJsonString;
        }



    }
}
