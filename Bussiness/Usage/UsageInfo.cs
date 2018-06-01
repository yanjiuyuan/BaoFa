using Bussiness.ProductionLines;
using Bussiness.Time;
using Common.DbHelper;
using Common.JsonHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;

namespace Bussiness.UsageInfo
{
    public class UsageInfo
    {
        public string GetUsage( )
        {
            string strJsonString = string.Empty;
            ProductionLinesServer pServer = new ProductionLinesServer();

            DataTable linedt = pServer.GetLinesList();

            if (linedt.Rows.Count < 0)
            {
                return strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"生产线数量为0!\"}";
            }
            List<Hashtable> list = new List<Hashtable> ();
            try
            {
                for (int x = 0; x < linedt.Rows.Count - 1; x++)
                {
                    Hashtable dic = new Hashtable();
                    int lineid = 0;
                    string linename = string.Empty; ;
                    int.TryParse(linedt.Rows[x][0].ToString(), out lineid);
                    linename = linedt.Rows[x][1].ToString();

                    dic.Add("ProductLineId", lineid.ToString());
                    dic.Add("ProductLineName", linename);
                    Dictionary<string, DataTable> dString = new Dictionary<string, DataTable>();

                    DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    long lTime = TimeHelper.ConvertDateTimeToInt(time);
                    //string strSql = string.Format("SELECT  *  FROM  `usage` a  LEFT JOIN  `line` b ON a.id_usage=b.id_usage  WHERE UpData_Time>{0}  ORDER BY  ct DESC LIMIT 0,1", lTime);
                    string strSql = string.Format("SELECT  *  FROM  `usage` a  LEFT JOIN  `line` b ON a.id_usage=b.id_usage  where a.ProductLineId={0}    ORDER BY  ct DESC LIMIT 0,1", lineid);
                    DataTable tb = MySqlHelper.ExecuteQuery(strSql);
                    if(tb.Rows.Count>0)
                    dic.Add("Data", JsonHelper.DataRowToDic(tb.Columns,tb.Rows[0]));
                    else
                    dic.Add("Data", "");


                    list.Add(dic);
                }
            }
            catch (Exception e)
            {
                return strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"交易处理异常\"}";
            }
            return JsonConvert.SerializeObject(list);

        }


            public String GetMonthUsage(string StartDate, string EndDate )
        {
            
              string strSql = " SELECT   sum(WeiTiaoConsumption) as WeiTiaoConsumption," +
                            " sum(HuChiConsumption) as HuChiConsumption," +
                            " sum(BiaoQianConsumption) as BiaoQianConsumption ," +
                          
                             " sum(DaDiConsumption) as DaDiConsumption" +
                            "  FROM `usage`   ";
            StringBuilder sb = new StringBuilder();
            sb.Append(strSql);

            if (StartDate != null || EndDate != null)
            {
                sb.Append(string.Format("  WHERE  ct BETWEEN '{0}' AND  '{1}'", StartDate, EndDate));
            }
            DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
            string strJsonString = string.Empty;
            if (tb.Rows.Count > 0)
                strJsonString = JsonHelper.DataTableToJson(tb);

            else
            {
                strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"暂无数据!\"}";
            }
            return strJsonString;
        }
    }
}
