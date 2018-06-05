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
namespace Bussiness.Order
{
    public class OrderServer
    {
        private static Logger logger = Logger.CreateLogger(typeof(OrderServer));
        public string GetAllOrderMessage(int PageIndex, int PageSize)
        {
            string strSql = string.Format("SELECT * FROM huabao.order LIMIT {0},{1}", PageIndex, PageSize);
            return GetOrderMessage(strSql);
        }

        public  string  GetAllOrderMessageWithParameter(string Color,
            string Material, string StartOrderTime, string EndOrderTime,
            string StartDeliveryTime, string EndDeliveryTime, string Customer, string ExpCountries, string KeyWord, int? PageIndex = 0, int? PageSize = 5)
        {
            // strMaterial = strMaterial == "" || strMaterial == null ? "1=1" : "Material='" + strMaterial + "'";
            // strColor = strColor == "" || strColor == null ? "1=1" : "color='" + strColor + "'";
            // strStartOrderTime = strStartOrderTime == "" || strStartOrderTime == null ? "1=1" : "ordtime BETWEEN '" + strStartOrderTime + "'";
            // strEndOrderTime = strEndOrderTime == "" || strEndOrderTime == null ? "1=1" : strEndOrderTime;
            // strStartDeliveryTime = strStartDeliveryTime == "" || strStartDeliveryTime == null ? "1=1" : "DeliveryTime BETWEEN '" + strStartDeliveryTime + "'";
            // strEndDeliveryTime = strEndDeliveryTime == "" || strEndDeliveryTime == null ? "1=1" : strEndDeliveryTime;
            // strCustomer = strCustomer == "" || strCustomer == null ? "1=1" : " Customer LIKE '%" + strCustomer + "%'";
            // strExpCountries = strExpCountries == "" || strExpCountries == null ? "1=1" : " ExpCountries LIKE '%" + strExpCountries + "%'";
            // int startRow = PageIndex * PageSize;
            // string strSql = string.Format("SELECT * FROM ( SELECT * FROM huabao.`order` " +
            //     "WHERE  {0} AND {1} " +
            //     "AND {2}  AND {3}" +
            //     " AND {4}  AND {5}" +
            //     " AND {6}  AND {7}" +
            //     " LIMIT {8},{9} ) a,( SELECT COUNT(*) AS Counts FROM huabao.`order` ) b", strMaterial, strColor, strStartOrderTime
            //     , strEndOrderTime, strStartDeliveryTime, strEndDeliveryTime, strCustomer, strExpCountries
            //     , startRow, PageSize);


            //return GetOrderMessage(strSql);

            try
            {
                int startRow = PageIndex.Value * PageSize.Value;
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM huabao.`order` ");
                if (Color != null || Material != null || StartOrderTime != null || EndOrderTime != null || StartDeliveryTime != null
                    || EndDeliveryTime != null || Customer != null || ExpCountries != null || KeyWord != null)
                {
                    sb.Append(" where 1=1 ");
                }
                if (KeyWord != null)
                {
                    string strWhereKeyWord = string.Format("and ( Color like '%{0}%'  " +
                        "or Material like  '%{0}%' " +
                        "or  ExpCountries  like  '%{0}%' " +
                        " or Customer like  '%{0}%'  )", KeyWord);
                    sb.Append(strWhereKeyWord);
                }
                if (Material != null)
                {
                    sb.Append(string.Format(" and Material='{0}'", Material));
                }
                if (Color != null)
                {
                    sb.Append(string.Format(" and Color='{0}'", Color));
                }
                if (StartOrderTime != null || EndOrderTime != null)
                {
                    sb.Append(string.Format(" and ordtime between '{0}' and '{1}' ", StartOrderTime, EndOrderTime));
                }
                if (StartDeliveryTime != null || EndDeliveryTime != null)
                {
                    sb.Append(string.Format(" and DeliveryTime between '{0}' and '{1}' ", StartDeliveryTime, EndDeliveryTime));
                }
                if (Customer != null)
                {
                    sb.Append(string.Format(" and Customer='{0}'", Customer));
                }
                if (ExpCountries != null)
                {
                    sb.Append(string.Format(" and ExpCountries='{0}'", ExpCountries));
                }
                int iRows = MySqlHelper.ExecuteQuery(sb.ToString()).Rows.Count;
                string strWhereLimit = string.Format(" LIMIT {0},{1}", startRow, PageSize.Value);
                sb.Append(strWhereLimit);
                Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
                DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());

                //DataTable dbCounts = new DataTable();
                //dbCounts.Columns.Add("Counts", Type.GetType("System.String"));
                //DataRow newRow = dbCounts.NewRow();
                //newRow["Counts"] = iRows.ToString();
                //dbCounts.Rows.Add(newRow);

                //dic.Add("Counts", dbCounts);
                dic.Add("OrderTable", tb);


                //string strJsonString = JsonConvert.SerializeObject(dic);
                string strJsonString = JsonHelper.DataTableToJson(tb, iRows);
                return strJsonString;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }

        /// <summary>
        /// 执行查询语句转换参数
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public string GetOrderMessage(string strSql)
        {

            try { 
            DataSet dataSet = MySqlHelper.GetDataSet(strSql);

            DataSet newDataSet = new DataSet();
            /// Material  1：色织布，2：帆布，3：尼龙，4：皮革，5：反毛皮，6：其他
            /// Color 1：黑色，2：白色，3：红色，4：黄，5：绿，6：紫，7：其他
            foreach (DataTable tb in dataSet.Tables)
            {
                DataTable newTB = UpdateDataTable(tb);
                //foreach (DataColumn cols in tb.Columns)
                //{
                //    if (cols.ColumnName == "Color")
                //    {
                //        //修改列类型
                //        cols.DataType = typeof(String);
                //    }
                //    if (cols.ColumnName == "Material")
                //    {
                //        //修改列类型
                //        cols.DataType = typeof(String);
                //    }
                //}
                foreach (DataRow dr in newTB.Rows)
                {
                    switch (dr["Color"].ToString())
                    {
                        case "1":
                            dr["Color"] = "黑色";
                            break;
                        case "2":
                            dr["Color"] = "白色";
                            break;
                        case "3":
                            dr["Color"] = "红色";
                            break;
                        case "4":
                            dr["Color"] = "黄";
                            break;
                        case "5":
                            dr["Color"] = "绿";
                            break;
                        case "6":
                            dr["Color"] = "紫";
                            break;
                        case "7":
                            dr["Color"] = "其他";
                            break;
                    }

                    switch (dr["Material"].ToString())
                    {
                        case "1":
                            dr["Material"] = "色织布";
                            break;
                        case "2":
                            dr["Material"] = "帆布";
                            break;
                        case "3":
                            dr["Material"] = "尼龙";
                            break;
                        case "4":
                            dr["Material"] = "皮革";
                            break;
                        case "5":
                            dr["Material"] = "反毛皮";
                            break;
                        case "6":
                            dr["Material"] = "其他";
                            break;
                    }
                }
                newDataSet.Tables.Add(newTB);
            }

            string strJsonString = string.Empty;
            strJsonString = JsonHelper.DatasetToJson(newDataSet);
            return strJsonString;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }

        /// <summary>
        /// 修改数据表DataTable某一列的类型和记录值(正确步骤：1.克隆表结构，2.修改列类型，3.修改记录值，4.返回希望的结果)
        /// </summary>
        /// <param name="argDataTable">数据表DataTable</param>
        /// <returns>数据表DataTable</returns>  

        private DataTable UpdateDataTable(DataTable argDataTable)
        {
            //新表  
     
            DataTable newDt = new DataTable();
            try
            {
                List<string> listColums = new List<string>();
            //复制表结够  
            newDt = argDataTable.Clone();

            //新表中的列数据类型为Decmail的改为string  
            foreach (DataColumn col in newDt.Columns)
            {
                listColums.Add(col.ColumnName);
                if (col.DataType.FullName == "System.Decimal")
                {
                    col.DataType = Type.GetType("System.String");
                }
                if (col.DataType.FullName == "System.SByte")
                {
                    col.DataType = Type.GetType("System.String");
                }
            }

            foreach (DataRow row in argDataTable.Rows)
            {
                DataRow newDtRow = newDt.NewRow();
                foreach (DataColumn column in argDataTable.Columns)
                {
                    newDtRow[column.ColumnName] = row[column.ColumnName];
                }
                newDt.Rows.Add(newDtRow);
            }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
               
            }
            return newDt;
           
        }
    }
}
