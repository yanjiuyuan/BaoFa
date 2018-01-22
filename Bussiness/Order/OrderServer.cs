﻿using Common.DbHelper;
using Common.JsonHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Order
{
    public class OrderServer
    {

        public string GetOrderMessage()
        {
            string strSql = "SELECT * FROM huabao.order";
            DataSet dataSet = MySqlHelper.GetDataSet(strSql);

            DataSet newDataSet = new DataSet();
            /// Material  1：色织布，2：帆布，3：尼龙，4：皮革，5：反毛皮，6：其他
            /// Color 1：黑色，2：白色，3：红色，4：黄，5：绿，6：紫，7：其他
            foreach (DataTable tb in dataSet.Tables)
            {
                DataTable newTB=UpdateDataTable(tb);
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
                foreach (DataRow  dr in newTB.Rows)
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

                    newDataSet.Tables.Add(newTB);
                }
            }

            string strJsonString = string.Empty;
            strJsonString = JsonHelper.DataTableToJsonWithJsonNet(newDataSet);
            return strJsonString;
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
            return newDt;
        }
    }
}
