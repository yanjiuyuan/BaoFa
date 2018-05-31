using Bussiness.Time;
using Common.DbHelper;
using Common.JsonHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.UsageInfo
{
    public class UsageInfo
    {
        public DataTable GetUsage()
        {
            DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            long lTime = TimeHelper.ConvertDateTimeToInt(time);
            //string strSql = string.Format("SELECT  *  FROM  `usage` a  LEFT JOIN  `line` b ON a.id_usage=b.id_usage  WHERE UpData_Time>{0}  ORDER BY  ct DESC LIMIT 0,1", lTime);
            string strSql = string.Format("SELECT  *  FROM  `usage` a  LEFT JOIN  `line` b ON a.id_usage=b.id_usage    ORDER BY  ct DESC LIMIT 0,1", lTime);
            DataTable tb = MySqlHelper.ExecuteQuery(strSql);
            return tb;
        }





        public String GetMonthUsage(string StartDate, string EndDate )
        {
            
              string strSql = " SELECT   sum(WeiTiaoConsumption) as WeiTiaoConsumption," +
                            " sum(HuChiConsumption) as HuChiConsumption," +
                            " sum(BiaoQianConsumption) as BiaoQianConsumption ," +
                             " sum(DaDiConsumption) as DaDiConsumption," +
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
