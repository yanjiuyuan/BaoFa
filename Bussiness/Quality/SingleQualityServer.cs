using Bussiness.DosageInfo;
using Bussiness.Time;
using Common.DbHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Quality
{
    public class SingleQualityServer
    {
        public DataTable GetSingQuality(string RFID, string strTableName)
        {
            string strSql = string.Format(" SELECT * FROM `{0}`  WHERE rfidn='{1}'  ORDER BY {2}id", strTableName, RFID, strTableName);
            DataTable tb = MySqlHelper.ExecuteQuery(strSql);
            DataTable OldTb = ChangeTableBySecond(tb);
            //OldTb = CalculateTable(OldTb);
            return OldTb;
        }

        /// <summary>
        /// 取整点及头尾有效数据
        /// </summary>
        /// <param name="tbNew"></param>
        /// <returns></returns>
        public DataTable ChangeTableBySecond(DataTable tbNew)
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
            if (tbNew.Rows.Count > 0)
            {
                string strBeginTime = tbNew.Rows[0][0].ToString();
                string strEndTime = tbNew.Rows[tbNew.Rows.Count - 1][0].ToString();
                float x = (float.Parse(strEndTime) - float.Parse(strBeginTime));
                int z = (int)(x / 1000); //一秒约有z个点
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
            }
           

            //string strCompareTime = string.Empty;
            for (int i = 0; i < tbOld.Rows.Count; i++)
            {
                //转换时间格式
                //tbOld.Rows[i][0] = TimeHelper.ConvertStringToDateTime(tbOld.Rows[i][0].ToString());
                //tbOld.Rows[i][0] = tbOld.Rows[i][0].ToString().Substring(tbOld.Rows[i][0].ToString().Length - 8, 5);
            }
            return tbOld;
        }

        public DataTable CalculateTable(DataTable tbNew)
        {
            int i = 0;
            string strValue = string.Empty;
            foreach (DataRow row in tbNew.Rows)
            {
                if (i == 0)
                {
                    strValue = row[0].ToString();
                    row[0] = 0;
                    i++;
                }
                else
                {
                    row[0] = long.Parse(row[0].ToString()) - long.Parse(strValue);
                }
            }

            return tbNew;
        }
    }
}
