using Bussiness.Time;
using Common.DbHelper;
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
    }
}
