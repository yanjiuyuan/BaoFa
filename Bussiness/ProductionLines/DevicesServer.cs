using Common.DbHelper;
using Common.JsonHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.LogHelper;
using Newtonsoft.Json;
using System.Collections;

namespace Bussiness.ProductionLines
{
    public class DevicesServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(DevicesServer));
        public string GetDeviceList(int? ProductLineId, int? CompanyId,   int? GroupId, int? FoundryId,
            int? devicestat, string KeyWord  , int? PageIndex = 0, int? PageSize = 10)
        {

            try
            {
                int startRow = PageIndex.Value * PageSize.Value;

                //设备最后是归属产线，所以，这个查询首先获取产线列表

                List<int> linelist = Global.GetLineList(ProductLineId, CompanyId, GroupId, FoundryId);
                StringBuilder lines = new StringBuilder();
                int num = 0;
                foreach (int s in linelist)
                {
                   
                    lines.Append(s);
                    num++;
                    if (num!= linelist.Count)
                    lines.Append(",");

                }

                StringBuilder sb = new StringBuilder();


                sb.Append(" select t.*,t1.ProductLineName,t2.stationname from deviceinfo t" +
                    " left join productlineinfo t1 on  t.ProductLineId=t1.ProductLineId " +
                    " left join locationcfg t2 on t.LocationId=t2.LocationId and t.ProductLineId=t2.ProductLineId " +
                    "  where t.ProductLineId in("+ lines.ToString()+") ");


           
                if(devicestat!=null)
                    sb.Append(string.Format(" and t.DeviceStat={0}", devicestat));
                if (KeyWord != null)
                {
                    string strWhereKeyWord = string.Format(
                        " and ( DeviceId like  '%{0}%' " +
                        "   or  DeviceName like  '%{0}%' " +
                          "   or  DeviceModel like  '%{0}%' " + 
                             "   or  ProductLineName like  '%{0}%' ) ", KeyWord);
                    sb.Append(strWhereKeyWord);
                }
                 
                sb.Append("order by t.ProductLineId, t.LocationId");
                int iRows = MySqlHelper.ExecuteQuery(sb.ToString()).Rows.Count;
                string strWhereLimit = string.Format(" LIMIT {0},{1}", startRow, PageSize.Value);

                sb.Append(strWhereLimit);
                DataTable db = MySqlHelper.ExecuteQuery(sb.ToString());
                //Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
                //dic.Add("Spray", db);
                string strJsonString = JsonHelper.DataTableToJson(db, iRows);
                return strJsonString;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }

        public string DeviceAdd(string DeviceId, string DeviceName, string DeviceType, string DeviceModel, string ComAddress
           , int? ProductLineId, int? LocationId, string OnlineDate, string OfflineDate, int? DeviceStat,string oper)

        {
            string retstr = string.Empty;

            try
            {
                string strTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //进行MD5 加密

                string sql = "insert into  deviceinfo set " +
                    "DeviceId='{0}',DeviceName='{1}',DeviceType='{2}',DeviceModel='{3}'," +
                    "ComAddress='{4}',OnlineDate='{5}'," +
                    "OfflineDate='{6}',CT='{7}',Operator='{8}'";
                if (ProductLineId != null)
                    sql += " ,ProductLineId={9}";
                if (LocationId != null)
                    sql += " ,LocationId={10}";
                if (DeviceStat != null)
                    sql += " ,DeviceStat={11}";
                string strSql=string.Format(sql ,
                      DeviceId,   DeviceName,   DeviceType,   DeviceModel,   ComAddress
               ,   OnlineDate,   OfflineDate,   strTime, oper, ProductLineId==null?0:(int)ProductLineId,
                    LocationId == null ? 0 : (int)LocationId,
                    DeviceStat == null ? 0 : (int)DeviceStat );


                int anum = MySqlHelper.ExecuteSql(strSql);

                if (anum <= 1)
                    retstr = Global.RETURN_ERROR("操作失败!");
                retstr = Global.RETURN_SUCESS;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                retstr = Global.RETURN_ERROR(ex.Message);
            }
            return retstr;

        }


        public string DeviceSave(string DeviceId, string DeviceName, string DeviceType, string DeviceModel, string ComAddress
           , int? ProductLineId, int? LocationId, string OnlineDate, string OfflineDate, int? DeviceStat, string oper)

        {
            string retstr = string.Empty;

            try
            {
                string strTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //进行MD5 加密
                string sql = "update  deviceinfo set" +
                  " DeviceName='{1}',DeviceType='{2}',DeviceModel='{3}'," +
                  " ComAddress='{4}',OnlineDate='{5}'," +
                  " OfflineDate='{6}',CT='{7}',Operator='{8}'";
                if (ProductLineId != null)
                    sql += " ,ProductLineId={9}";
                if (LocationId != null)
                    sql += " ,LocationId={10}";
                if (DeviceStat != null)
                    sql += " ,DeviceStat={11}";

                sql += " where DeviceId='{0}'";
                string strSql = string.Format(sql,
                      DeviceId, DeviceName, DeviceType, DeviceModel, ComAddress
               , OnlineDate, OfflineDate, strTime, oper, ProductLineId == null ? 0 : (int)ProductLineId,
                    LocationId == null ? 0 : (int)LocationId,
                    DeviceStat == null ? 0 : (int)DeviceStat);
                 


                int anum = MySqlHelper.ExecuteSql(strSql);

                if (anum <= 1)
                    retstr = Global.RETURN_ERROR("操作失败!");
                retstr = Global.RETURN_SUCESS;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                retstr = Global.RETURN_ERROR(ex.Message);
            }
            return retstr;

        }
        public string DeviceDel(string DeviceId )

        {
            string retstr = string.Empty;

            try
            {
              
                string strSql = string.Format("delete from   deviceinfo   where DeviceId='{0}'",
                      DeviceId );


                int anum = MySqlHelper.ExecuteSql(strSql);

                if (anum <= 1)
                    retstr = Global.RETURN_ERROR("操作失败!");
                retstr = Global.RETURN_SUCESS;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                retstr = Global.RETURN_ERROR(ex.Message);
            }
            return retstr;

        }
        public string DeviceModelList( )

        {
            string retstr = string.Empty;
     
            try
            {

                string strSql = string.Format(" select distinct(devicemodel) as devicemodel from    deviceinfo  " );

                DataTable dt = MySqlHelper.ExecuteQuery(strSql);
                 retstr=JsonConvert.SerializeObject(dt);
 

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                retstr = Global.RETURN_ERROR(ex.Message);
            }
            return retstr;

        }

        //根据设备编号ID查询机器人坐标
        public string GetRobotCoordinate(string id)
        {
            string retstr = string.Empty; ;
            try
            {   if(RobotData.data.ContainsKey(id))
                {
                    retstr = JsonConvert.SerializeObject( RobotData.data[id]);
                     
                }

                else
                {
                    retstr = Global.RETURN_ERROR("设备查询为空");
                }
                 

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
               
            }

            return retstr;
        }

    }
}
