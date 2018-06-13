using Bussiness.Time;
using Common.Code;
using Common.DbHelper;
using Common.JsonHelper;
using Common.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        public string ChartBeatQuery(  string DataTime,int lineid=1 )
        {
            string strJsonString = string.Empty;

            try
            {
                



                logger.Info("查询生产节拍");
           
               
                long begintime = 0;
                long endtime = 0;
                string datestr =   DateTime.Now.ToLongDateString();
                if (DataTime == null)
                {
                   
                    begintime = TimeHelper.ConvertDateTimeToInt(Convert.ToDateTime(datestr));
                    endtime = TimeHelper.ConvertDateTimeToInt(Convert.ToDateTime(datestr).AddDays(1));
                }
                else
                {
                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

                    dtFormat.ShortDatePattern = "yyyy-MM-dd";
                    datestr = DataTime;
                    begintime = TimeHelper.ConvertDateTimeToInt(Convert.ToDateTime(datestr, dtFormat));
                    endtime = TimeHelper.ConvertDateTimeToInt(Convert.ToDateTime(datestr, dtFormat).AddDays(1));

                }
              
                //获取生产线的最新id_usage
            
           //string dt = TimeHelper.GetDateTimeFrom1970Ticks(1527044472080/1000).ToString();
            string strSql = "   select a1.*, if (stationstate is null ,'停止',stationstate) as stationstate , if (currtime is null ,0,currtime) as currtime  from ( select  t2.JobType,t2.LocationSeq,t1.stationNAME, round(run_t /if (run_c > 0,run_c,1))as run,round(free_t /if (free_c > 0,free_c,1)) as free," +
            "round(warn_t /if (warn_c > 0,warn_c,1))as warn from(" +
           " select   sum( if (stationstate = '运行', endtime - startTime, 0))/1000 AS run_t," +
           "sum( if (stationstate = '运行',1, 0))  AS run_c," +
            " sum( if (stationstate = '空闲', endtime - startTime, 0))/1000 AS free_t," +
              " sum( if (stationstate = '空闲',1, 0))  AS free_c," +
             " sum( if (stationstate = '报警', endtime - startTime, 0))/1000 AS warn_t," +
              "  sum( if (stationstate = '报警',1, 0))  AS warn_c , stationNAME" +
             " from huabao.LocationState where   starttime > " + begintime + " and  starttime < " + endtime + " and ProductLineId = " + lineid + " group by stationNAME ) as t1 left join" +
             "  (SELECT *  from huabao.locationcfg where ProductLineId = "+lineid+"  )  t2  on t1.stationNAME=t2.StationName ) a1" +
             " left join ( select  stationNAME,stationstate , (TIMESTAMPDIFF(SECOND, '1970-1-1 08:00:00', NOW())- round(starttime/1000)) as currtime    from huabao.LocationStatecache where " +
             "   ProductLineId = " + lineid + "  )b1" +
             " on a1.stationNAME  =b1.stationNAME order by a1.JobType, a1.LocationSeq";
          
          
          
                DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
               if(newTb.Rows.Count>0)
                {
                    for (int i = 0; i < newTb.Rows.Count; i++)
                    { 
                        if (Convert.ToUInt32(newTb.Rows[i][3]) > 299)
                            newTb.Rows[i][3] = 299;
                    if (Convert.ToUInt32(newTb.Rows[i][4]) > 299)
                        newTb.Rows[i][4] = 299;
                    if (Convert.ToUInt32(newTb.Rows[i][5]) > 299)
                        newTb.Rows[i][5] = 299;
                    if (Convert.ToUInt32(newTb.Rows[i][7]) > 299)
                        newTb.Rows[i][7] = 299;
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
