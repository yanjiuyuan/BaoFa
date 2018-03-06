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


        public DataTable GetCurrentProduction(long strDataTime)
        {
            string strSql = string.Format("SELECT  ID_RealTimeUsage,AllN,NowN,OldN FROM huabao.`realtimeusage` WHERE ID_RealTimeUsage>{0} order by ID_RealTimeUsage", strDataTime);
            DataTable tb = Common.DbHelper.MySqlHelper.ExecuteQuery(strSql);
            return ChangeTable(tb);
        }

        /// <summary>
        /// 取整点及头尾有效数据
        /// </summary>
        /// <param name="tbNew"></param>
        /// <returns></returns>
        public DataTable ChangeTable(DataTable tbNew)
        {
            DataTable tbOld = new DataTable();
            tbOld = tbNew.Clone();
            tbOld.PrimaryKey = null;
            foreach (DataColumn col in tbOld.Columns)
            {
                if (col.ColumnName == "ID_RealTimeUsage" ||
                    col.ColumnName == "VampID" ||
                    col.ColumnName == "WaiOID" ||
                        col.ColumnName == "WaiTID" ||
                    col.ColumnName == "WaiSID" ||
                    col.ColumnName == "OutsoleID" ||
                     col.ColumnName == "MouthguardsID" ||
                      col.ColumnName == "LineUsageID")
                {
                    col.DataType = typeof(string); //改变第一列属性值
                }
                
            }
            string strBeginTime = tbNew.Rows[0][0].ToString();
            string strEndTime = tbNew.Rows[tbNew.Rows.Count - 1][0].ToString();
            float x = (float.Parse(strEndTime) - float.Parse(strBeginTime)) / tbNew.Rows.Count;
            int z = (int)(360 * (10000 / x)); //一小时约有z个点
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
                    if (z != 0)
                    {
                        if (i % z == 0)
                        {
                            tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                        }
                    }

                }
            }

            //string strCompareTime = string.Empty;
            for (int i = 0; i < tbOld.Rows.Count; i++)
            {
                //转换时间格式
                tbOld.Rows[i][0] = TimeHelper.ConvertStringToDateTime(tbOld.Rows[i][0].ToString());
                //截取时间
                //if (i == 0)
                //{
                //    strCompareTime = tbOld.Rows[i][0].ToString().Substring(tbOld.Rows[i][0].ToString().Length - 8, 5);
                //    //插入数据
                //    tbOld.Rows[i][0] = tbOld.Rows[i][0].ToString().Substring(tbOld.Rows[i][0].ToString().Length - 8, 5);
                //}
                //else
                //{
                //    //两次数据相同不插入
                //    strCompareTime = tbOld.Rows[i][0].ToString().Substring(tbOld.Rows[i][0].ToString().Length - 8, 5);
                //}

                //tbOld.Rows[i][0] = tbOld.Rows[i][0].ToString().Substring();
                tbOld.Rows[i][0] = tbOld.Rows[i][0].ToString().Substring(tbOld.Rows[i][0].ToString().Length - 8, 5);
            }
            return tbOld;
        }
    }

}
