using Bussiness.Time;
using Common.Code;
using Common.DbHelper;
using Common.JsonHelper;
using Common.LogHelper;
using Newtonsoft.Json;
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
                string strSql = " select b1.JobType,b1.StationName,b1.LocationSeq, if (stationstate is null ,'停止',stationstate) as stationstate , if (currtime is null || currtime < 1,1,currtime) as currtime ," +
                    " if (run is null  ,0,run) as run ,if (free is null ,0,free) as free ,if (warn is null ,0,warn) as warn  from(select t3.*, t2.LocationSeq, t2.JobType  from(select  stationNAME, stationstate, (TIMESTAMPDIFF(SECOND, '1970-1-1 08:00:00', NOW()) - round(starttime / 1000)) as currtime " +
                    " from huabao.LocationStatecache where ProductLineId =  " + lineid + ") t3 " +
            " left join(SELECT * from huabao.locationcfg where ProductLineId = " + lineid + ")  t2 " +
                   " on t3.stationNAME = t2.StationName and t2.state=1) as b1   left join(select stationNAME, " +
        "  round( run_t  /if (run_c > 0,run_c,1))as run,round(free_t /if (free_c > 0,free_c,1)) as free,round(warn_t /if (warn_c > 0,warn_c,1))as warn " +
     " from(select   sum( if (stationstate = '运行',if( (endtime - startTime)<300000,endtime - startTime,300000), 0))/ 1000 AS run_t, sum( if (stationstate = '运行',1, 0))  AS run_c, sum( if " +
       "  (stationstate = '空闲', endtime - startTime, 0))/ 1000 AS free_t, sum( if (stationstate = '空闲',1, 0))  AS free_c, " +
      " sum( if (stationstate = '报警', endtime - startTime, 0))/ 1000 AS warn_t, sum( if (stationstate = '报警',1, 0))  " +
        " AS warn_c, stationNAME from huabao.LocationState where starttime > " + begintime + " and starttime< " + endtime + " and ProductLineId =  " + lineid + " " +
       " group by stationNAME ) a2 )a1 on a1.stationNAME = b1.stationNAME where JobType is not null  order by   b1.LocationSeq";

                 
                DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
               if(newTb.Rows.Count>0)
                {
                    for (int i = 0; i < newTb.Rows.Count; i++)
                    { 
                        if (Convert.ToUInt32(newTb.Rows[i][6]) > 299)
                            newTb.Rows[i][6] = 299;
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


        public string LocationBeatQuery(string DataTime,string stationname, int lineid = 1)
        {
            string strJsonString = string.Empty;

            try
            {
 
                long begintime = 0;
                long endtime = 0;
                string datestr = DateTime.Now.ToLongDateString();
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
                double avgrun = 0.0;
                double avgfree = 0.0;
                double avgwarn = 0.0;
                 
                //获取七日平均
                string strSql = " select round( sum(runt)  /sum(if (runc > 0,runc,1)))as run,round(sum(freet)/ sum(if (freec > 0,freec,1))) as free,round(sum(warnt) /sum(if (warnc > 0,warnc,1)))as warn from rptlocationday a left join locationcfg b on a.ProductLineId=b.ProductLineId and a.locationid=" +
                    "b.locationid where a.ProductLineId=" + lineid + " and b.StationName='" + stationname + "' limit 7";
                 DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
                if(newTb.Rows.Count>0)
                {
                    avgrun = Convert.ToDouble(newTb.Rows[0]["run"].ToString());
                    avgfree = Convert.ToDouble(newTb.Rows[0]["free"].ToString());
                    avgwarn = Convert.ToDouble(newTb.Rows[0]["warn"].ToString());

                }
                
                

                string strSql1="  select stationstate, FROM_UNIXTIME(starttime / 1000, '%H:%i:%s' ) as tm    , if (currtime > 299,299,currtime) as timelen from(  "+
                         "  select  stationstate, starttime, (TIMESTAMPDIFF(SECOND, '1970-1-1 08:00:00', NOW()) - round(starttime / 1000)) as currtime     from locationstatecache where ProductLineId ="+lineid+" and StationName = '"+stationname+"' "+
                     "  union all  select  stationstate, starttime, round((endtime - starttime) / 1000) as currtime from locationstate  where ProductLineId = "+lineid+ " and StationName = '" + stationname + "'  and starttime >  '"+ begintime+ "' and stationstate!='报警' order by starttime desc limit 200 " +
                " ) a order by starttime asc  ";
                DataTable  Tb = MySqlHelper.ExecuteQuery(strSql1);
                string recds = JsonConvert.SerializeObject(Tb);


                string strjson = "{ \"avgrun\":" + avgrun + ", \"avgfree\":" + avgfree + ", \"avgwarn \": " + avgwarn + " ,\"records\":" + recds + " }";
                return strjson;

            }
            catch (Exception ex)
            {
                logger.Error("查询生产节拍失败" + ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
             
        }
    }
}
