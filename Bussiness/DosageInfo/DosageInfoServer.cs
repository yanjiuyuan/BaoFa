using Bussiness.Time;
using Common.DbHelper;
using Common.JsonHelper;
 
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.LogHelper;
namespace Bussiness.DosageInfo
{
    public class DosageInfoServer
    {
        private static Logger logger = Logger.CreateLogger(typeof(DosageInfoServer));
        public string GetDosageInfo(string  startdate,string enddate, int lineid)
        {
            //string strSearchSql = "SELECT WeiTiaoConsumption,HuChiConsumption,BiaoQianConsumption,DaDiConsumption FROM huabao.`Usage` " +
            //    " WHERE orderid =@Orderid AND childid =@Childid  ORDER BY CT DESC   LIMIT 0,1";
            //string strSearchSql = "SELECT WeiTiaoConsumption,HuChiConsumption,BiaoQianConsumption,DaDiConsumption FROM huabao.`Usage` " +
            //   " WHERE OldN =@OldN  ORDER BY CT DESC   LIMIT 0,1";
            //MySqlParameter[] parameter = new MySqlParameter[] {
            //   // new MySqlParameter("@Orderid",MySqlDbType.VarChar),
            //   // new MySqlParameter ("@Childid",MySqlDbType.VarChar
            //    new MySqlParameter ("@OldN",MySqlDbType.String),
            //};
            string strJsonString = string.Empty;
            try
            {
                string strSearchSql = string.Format("SELECT   sum(WeiTiaoConsumption) as WeiTiaoConsumption,  sum(HuChiConsumption) as HuChiConsumption, sum(BiaoQianConsumption) as BiaoQianConsumption,  sum(DaDiConsumption) as DaDiConsumption FROM huabao.`Usage` WHERE CT >='{0}' AND CT <='{1}' and ProductLineId={2}  ", startdate, enddate, lineid);
                //string strSearchSql = "SELECT * FROM huabao.`Usage`";
                DataTable db = Common.DbHelper.MySqlHelper.ExecuteQuery(strSearchSql);
                strJsonString = JsonConvert.SerializeObject(db);
                //string strJsonString = JsonHelper.DataTableToJson(db);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }

            return strJsonString;
             
        }


        public DataTable GetCurrentProduction(long startDataTime, long endDataTime, int Count,int lineid)
        {
            DataTable tb = new DataTable();
            try {
            string strSql = string.Format("SELECT  ID_RealTimeUsage,AllN,NowN,OldN,ChildN FROM huabao.`realtimeusage` WHERE ID_RealTimeUsage>{0} and ID_RealTimeUsage <{1} and ProductLineId ={2} " +
                " order by ID_RealTimeUsage ", startDataTime, endDataTime, lineid);
             tb = Common.DbHelper.MySqlHelper.ExecuteQuery(strSql);
            
            }
            catch (Exception  ex)
            {
                logger.Error(ex.Message);
                
            }
            return ChangeTable(tb, Count);
        }

        /// <summary>
        /// 取整点及头尾有效数据
        /// </summary>
        /// <param name="tbNew"></param>
        /// <returns></returns>
        public DataTable ChangeTable(DataTable tbNew, int Count)
        {
            
            DataTable tbOld = new DataTable();
            tbOld = tbNew.Clone();
            tbOld.PrimaryKey = null;
            tbOld.Columns["ID_RealTimeUsage"].DataType = typeof(string);//指定Age为Int类型
            try
            {
                if (tbNew.Rows.Count > 0)
            {
                if (tbNew.Rows.Count > Count)
                {
                    int j = tbNew.Rows.Count / Count + 1;
                    for (int i = 0; i < tbNew.Rows.Count; i++)
                    {
                        //取收尾两
                        if (i == 0 || i == tbNew.Rows.Count - 1)
                        { 
                            //加入首尾两行
                            tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                        }
                        else
                        {
                            if (i * j < tbNew.Rows.Count)
                            {
                                tbOld.Rows.Add(tbNew.Rows[i * j].ItemArray);
                            }
                        }
                    }
                }
                else
                {

                    //数据条数低于Count ，则去除间隔低于1分钟的数据
                    for (int i = 0; i < tbNew.Rows.Count; i++)
                    {
                        if (i == 0)
                            tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                        else if (i == tbNew.Rows.Count - 1 && tbNew.Rows.Count > 2)
                        {
                            tbOld.Rows.Add(tbNew.Rows[i].ItemArray);

                            continue;
                        }

                        else if (Convert.ToInt64(tbNew.Rows[i][0]) >= Convert.ToInt64(tbNew.Rows[i - 1][0]) + (long)60*1000)
                        {
                            tbOld.Rows.Add(tbNew.Rows[i].ItemArray);

                            continue;
                        }
                    }

                   
                }
            }
            
            for (int i = 0; i < tbOld.Rows.Count; i++)
            {
                //转换时间格式
                tbOld.Rows[i][0] = TimeHelper.GetStringToDateTime((tbOld.Rows[i][0].ToString()));

            }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return tbOld;
        }



        public DataTable GetYieldFluctuation(long strDataTime, long strDataTimeend, int dura_min,int lineid)
        {
            DataTable tb = new DataTable();
            try { 
            string strSql = string.Format("SELECT  ID_RealTimeUsage,NowN, NowN as CurrN FROM huabao.`realtimeusage` WHERE ID_RealTimeUsage>{0} and ID_RealTimeUsage<{1} and ProductLineId={2} order by ID_RealTimeUsage", strDataTime, strDataTimeend, lineid);
                tb = Common.DbHelper.MySqlHelper.ExecuteQuery(strSql);
            
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                
            }
            return ChangeData(tb, dura_min);
        }

        /// <summary>
        /// 取整点及头尾有效数据
        /// </summary>
        /// <param name="tbNew"></param>
        /// <returns></returns>
        public DataTable ChangeData(DataTable tbNew, int dura_min)
        {
            int scale = 1;
            if (dura_min > 60) dura_min = 60;
            if (dura_min < 60) dura_min = 10;
            
                scale = 60 / dura_min;


             DataTable tbOld = new DataTable();
            try
            {
                tbOld = tbNew.Clone();
            tbOld.PrimaryKey = null;
            tbOld.Columns["ID_RealTimeUsage"].DataType = typeof(string);//指定Age为Int类型

            if (tbNew.Rows.Count > 0)
            {
                 
                for (int i = 0; i < tbNew.Rows.Count; i++)
                {
                    if (i == 0)
                        {
                        tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                      
                        }
                        else if (i == tbNew.Rows.Count - 1 && tbNew.Rows.Count > 2)
                    {
                        tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                        int count = tbOld.Rows.Count-1;
                       
                        tbOld.Rows[count][2] = scale*(Convert.ToInt32(tbOld.Rows[count][1]) - Convert.ToInt32(tbOld.Rows[count - 1][1]));
                        continue;
                    }

                  else  if (Convert.ToInt64(tbNew.Rows[i][0]) >= Convert.ToInt64(tbOld.Rows[tbOld.Rows.Count - 1][0]) + (long)dura_min * 60 * 1000)
                    {
                        tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                        int count = tbOld.Rows.Count-1;
                        tbOld.Rows[count][2] = scale * (Convert.ToInt32(tbOld.Rows[count][1]) - Convert.ToInt32(tbOld.Rows[count - 1][1]));
                        
                        continue;
                    }
                }
            }

            for (int i = 0; i < tbOld.Rows.Count; i++)
            {
                //转换时间格式
                tbOld.Rows[i][0] = TimeHelper.GetStringToDateTime((tbOld.Rows[i][0].ToString()));
                tbOld.Rows[i][2] = Convert.ToInt32(tbOld.Rows[i][2]) < 0 ? 0 : Convert.ToInt32(tbOld.Rows[i][2]);
            }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            return tbOld;
        }

        public string GetMonProduct(string StartDate, string EndDate, bool MultiMon = true)
        {

            try
            {
                //同一个月
                if (!MultiMon)
                {


                    string strSql = " SELECT datestr, round(sum( b.`NowN`) ) as AllN" +
                                    
                                    "  FROM   ( select left(a.CT, 10) as datestr,a.* from `usage`  a  ";



                    StringBuilder sb = new StringBuilder();
                    sb.Append(strSql);

                    if (StartDate != null || EndDate != null)
                    {
                        sb.Append(string.Format(" WHERE ct BETWEEN '{0}' AND  '{1}'     ", StartDate, EndDate));
                    }

                    sb.Append(" )b group by datestr order by datestr ");
                    DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
                    string strJsonString = string.Empty;
                    if (tb.Rows.Count > 0)
                        strJsonString = JsonHelper.DataTableToJson(tb);

                    else
                    {
                        return Global.RETURN_EMPTY;
                    }
                    return strJsonString;
                }
                else
                {


                    string strSql = " SELECT datestr, round(sum( b.`NowN`) ) as AllN" +

                                    "  FROM   ( select left(a.CT,7) as datestr,a.* from `usage`  a  ";
                   


                    StringBuilder sb = new StringBuilder();
                    sb.Append(strSql);

                    if (StartDate != null || EndDate != null)
                    {
                        sb.Append(string.Format(" WHERE ct BETWEEN '{0}' AND  '{1}'     ", StartDate, EndDate));
                    }

                    sb.Append(" ) b group by datestr  order by datestr ");
                    DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
                    string strJsonString = string.Empty;
                    if (tb.Rows.Count > 0)
                        strJsonString = JsonHelper.DataTableToJson(tb);

                    else
                    {
                        return Global.RETURN_EMPTY;
                    }
                    return strJsonString;



                }
            }

            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }


    }

}
