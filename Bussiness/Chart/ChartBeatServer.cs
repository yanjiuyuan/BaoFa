using Common.DbHelper;
using Common.Encrypt;
using Common.JsonHelper;
using Common.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class ChartBeatServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(ChartBeatServer));
        //获取各工位的末次生产时间
        public  string ChartBeatQuery( )
        {
            logger.Info("查询生产节拍");
            string strSql = "select t.* from ( "+
                   " select max(endtime) -max(startTime) as duratm,stationNAME from huabao.LocationState where stationstate  = '运行' " +
               " group by stationNAME ) as    t order by t.stationNAME";
            string strJsonString = string.Empty;
            try { 
            DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
         
            strJsonString = JsonHelper.DataTableToJson(newTb);
            }
            catch(Exception ex)
            {
                logger.Error("查询生产节拍失败"+ex.Message);
                return strJsonString;
            }
            logger.Info("查询生产节拍结束");
            return strJsonString;
        }

        

    }
}
