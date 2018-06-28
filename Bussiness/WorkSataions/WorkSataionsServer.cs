using Common.DbHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.LogHelper;
using Newtonsoft.Json;

namespace Bussiness.WorkSataions
{
    public class WorkSataionsServer
    {
        private static Logger logger = Logger.CreateLogger(typeof(WorkSataionsServer));
        public DataTable GetWorkStationInfo()
        {
            string strSql = " SELECT  * FROM  `ArtificialConfig` a LEFT JOIN  `artificialinformation` b ON  a.Preferredid=b.ID_ArtificialInformation  WHERE endtime IS NULL  AND starttime IS NOT NULL ";
            DataTable db=MySqlHelper.ExecuteQuery(strSql);
            return db;
        }

        //按订单和生产线获取工位列表及状态
        public string GetWorkStationList(string orderid,int lineid=1)
        {
            string retstr = string.Empty;
            try
            {
                string strSql = " select  a.* , if ( b.stationstate is null,'停止',  b.stationstate) as stationstate ,if ( b.starttime is null,'',  b.starttime) as lsttime from artificialconfig a "
                + " left join LocationStatecache  b on a.Jobs = b.StationName and  a.ProductLineId=b.ProductLineId WHERE a.OrderID ='" + orderid + "' and a.ProductLineId="+lineid  ;
                DataTable db = MySqlHelper.ExecuteQuery(strSql);
                return  JsonConvert.SerializeObject(db);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);

            }
            
        }

        //人员iD 获取人员信息
        public string GetArtificialInfo( int artificialid  )
        {
            string retstr = string.Empty;
            try
            {
                string strSql = " select * from artificialinformation  WHERE ID_ArtificialInformation =" + artificialid ;
                DataTable db = MySqlHelper.ExecuteQuery(strSql);
                return JsonConvert.SerializeObject(db);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);

            }
           
        }
        public string GetMachineLocationList(int lineid)
        {
            string retstr = string.Empty;
            try
            {
                string strSql = " select locationid,stationname from locationcfg  WHERE productlineid =" + lineid +" and jobtype='机器' and state=1 order by locationseq ";
                DataTable db = MySqlHelper.ExecuteQuery(strSql);
                return JsonConvert.SerializeObject(db);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);

            }

        }

    }
}
