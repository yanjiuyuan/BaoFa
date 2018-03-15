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
            string strSql = "SELECT  *  FROM  `usage` a  LEFT JOIN  `line` b ON a.id_usage=b.id_usage   ORDER BY  ct DESC LIMIT 0,1";
            DataTable tb = MySqlHelper.ExecuteQuery(strSql);
            return tb;
        }
    }
}
