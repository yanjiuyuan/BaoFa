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
        public DataTable GetSingQuality(string RFID,int sprayid)
        {
             
            string strSql = string.Format(" SELECT * FROM  sprayrecd  WHERE  rfidn='{0}' and    sprayid={1} ORDER BY  id",   RFID, sprayid );
            DataTable tb = MySqlHelper.ExecuteQuery(strSql);
            DataTable OldTb = ChangeTableBySecond(tb);
            //OldTb = CalculateTable(OldTb);
            return OldTb;
        }
        public DataTable GetSingQualityLineUsage(string RFID)
        {

            string strSql = string.Format(" SELECT * FROM  lineusagerecd  WHERE  rfidn='{0}'   ORDER BY  lineusageid", RFID);
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
            tbOld.Columns[0].DataType = typeof(string);
            if (tbNew.Rows.Count > 0)
            {
              
                if (tbNew.Rows.Count > 20)
                {
                    int j = tbNew.Rows.Count / 20 + 1;
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
                     

                    //数据条数低于20 ，则去除间隔低于1秒的数据
                    for (int i = 0; i < tbNew.Rows.Count; i++)
                    {
                        if (i == 0)
                            tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                       else if (i == tbNew.Rows.Count - 1 && tbNew.Rows.Count > 2)
                        {
                            tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                            
                            continue;
                        }

                      else  if (Convert.ToInt64(tbNew.Rows[i][0]) >= Convert.ToInt64(tbNew.Rows[i-1][0]) + (long) 1000)
                        {
                            tbOld.Rows.Add(tbNew.Rows[i].ItemArray);
                         
                            continue;
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
                tbOld.Rows[i][0] = TimeHelper.GetStringToDateTime((tbOld.Rows[i][0].ToString()));
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
