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
        public string ChartBeatQuery(int lineid=1,int mins=30)
        {
            string strJsonString = string.Empty;

            try
            {

                logger.Info("查询生产节拍");
            int qmins = mins * -1;
            
            //获取指定时间点,若时间点小于当天则获取当天
            long begintime = TimeHelper.ConvertDateTimeToInt(DateTime.Now.AddMinutes(qmins));
             string datestr = DateTime.Now.ToLongDateString();
            long todaybegintime = TimeHelper.ConvertDateTimeToInt(Convert.ToDateTime(datestr));
                //if (begintime < todaybegintime)
                //    begintime = todaybegintime;
                //获取生产线的最新id_usage
            string strSql1 = " select  max(id_usage) from `usage` where ProductLineId = " + lineid + " limit 1";
            int usage_id = 0;
            object obj= MySqlHelper.GetSingle(strSql1);
            if (obj != null)
                usage_id = Convert.ToInt32(obj);
           //string dt = TimeHelper.GetDateTimeFrom1970Ticks(1527044472080/1000).ToString();
            string strSql = "   select a1.*, if (stationstate is null ,'停止',stationstate) as stationstate , if (currtime is null ,0,currtime) as currtime  from ( select  t2.JobType,t2.LocationSeq,t1.stationNAME, round(run_t /if (run_c > 0,run_c,1))as run,round(free_t /if (free_c > 0,free_c,1)) as free," +
            "round(warn_t /if (warn_c > 0,warn_c,1))as warn from(" +
           " select   sum( if (stationstate = '运行', endtime - startTime, 0))/1000 AS run_t," +
           "sum( if (stationstate = '运行',1, 0))  AS run_c," +
            " sum( if (stationstate = '空闲', endtime - startTime, 0))/1000 AS free_t," +
              " sum( if (stationstate = '空闲',1, 0))  AS free_c," +
             " sum( if (stationstate = '报警', endtime - startTime, 0))/1000 AS warn_t," +
              "  sum( if (stationstate = '报警',1, 0))  AS warn_c , stationNAME" +
             " from huabao.LocationState where   starttime > " + begintime + " and id_usage = " + usage_id + " group by stationNAME ) as t1 left join" +
             "   huabao.locationcfg t2  on t1.stationNAME=t2.StationName ) a1" +
             " left join ( select  stationNAME,stationstate , (TIMESTAMPDIFF(SECOND, '1970-1-1', NOW())- round(starttime/1000)) as currtime    from huabao.LocationStatecache where " +
             "   id_usage = " + usage_id + "  )b1" +
             " on a1.stationNAME  =b1.stationNAME order by a1.JobType, a1.LocationSeq";
          
          
          
                DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
               if(newTb.Rows.Count>0)
                {
                    for (int i = 0; i < newTb.Rows.Count; i++)
                    { 
                        if (Convert.ToUInt32(newTb.Rows[i][3]) > 300)
                            newTb.Rows[i][3] = 300;
                    if (Convert.ToUInt32(newTb.Rows[i][4]) > 300)
                        newTb.Rows[i][4] = 300;
                    if (Convert.ToUInt32(newTb.Rows[i][5]) > 300)
                        newTb.Rows[i][5] = 300;
                    if (Convert.ToUInt32(newTb.Rows[i][7]) > 300)
                        newTb.Rows[i][7] = 300;
                    }

                }

                strJsonString = JsonHelper.DataTableToJson(newTb);
             
            }
            catch (Exception ex)
            {
                logger.Error("查询生产节拍失败" + ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
            logger.Info("查询生产节拍结束");
            return strJsonString;
        }



    }
}
