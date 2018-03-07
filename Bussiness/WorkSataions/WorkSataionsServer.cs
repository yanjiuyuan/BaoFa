using Common.DbHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.WorkSataions
{
    public class WorkSataionsServer
    {
        public DataTable GetWorkStationInfo()
        {
            string strSql = " SELECT  * FROM  `ArtificialConfig` a LEFT JOIN  `artificialinformation` b ON  a.Preferredid=b.ID_ArtificialInformation  WHERE endtime IS NULL  AND starttime IS NOT NULL ";
            DataTable db=MySqlHelper.ExecuteQuery(strSql);
            return db;
        }
    }
}
