using Bussiness.Time;
using Common.DbHelper;
using Common.JsonHelper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.DosageInfo
{
    public class DosageInfoServer
    {
        public string GetDosageInfo(string OrderId, string ChildId)
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

            string strSearchSql = string.Format("SELECT WeiTiaoConsumption,HuChiConsumption,BiaoQianConsumption,DaDiConsumption FROM huabao.`Usage` WHERE orderid ='{0}' AND childid ='{1}'  ORDER BY CT DESC   LIMIT 0,1", OrderId, ChildId);
            //string strSearchSql = "SELECT * FROM huabao.`Usage`";
            DataTable db = Common.DbHelper.MySqlHelper.ExecuteQuery(strSearchSql);
            string strJsonString = JsonConvert.SerializeObject(db);
            //string strJsonString = JsonHelper.DataTableToJson(db);

            return strJsonString;
        }


        public DataTable GetCurrentProduction(long strDataTime, int Count)
        {
            string strSql = string.Format("SELECT  ID_RealTimeUsage,AllN,NowN,OldN,ChildN FROM huabao.`realtimeusage` WHERE ID_RealTimeUsage>{0} order by ID_RealTimeUsage", strDataTime);
            DataTable tb = Common.DbHelper.MySqlHelper.ExecuteQuery(strSql);
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
                    string strBeginTime = tbNew.Rows[0][0].ToString();
                    string strEndTime = tbNew.Rows[tbNew.Rows.Count - 1][0].ToString();
                    //float x = (float.Parse(strEndTime) - float.Parse(strBeginTime)) / tbNew.Rows.Count;
                    //int z = (int)(360 * (10000 / x)); //一小时约有z个点
                    float x = float.Parse(strEndTime) - float.Parse(strBeginTime);
                    int z = (int)(x / (60 * 1000));
                    int k = (int)(tbNew.Rows.Count / z);
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
                            if (k != 0)
                            {
                                if (i % k == 0)
                                {
                                    tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                                }
                            }
                        }
                    }
                }
            }
            
            for (int i = 0; i < tbOld.Rows.Count; i++)
            {
                //转换时间格式
                tbOld.Rows[i][0] = TimeHelper.GetStringToDateTime((tbOld.Rows[i][0].ToString()));

            }
            return tbOld;
        }
    }

}
