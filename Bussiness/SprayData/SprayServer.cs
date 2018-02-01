using Common.DbHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.SprayData
{
   public  class SprayServer
    {
        public string  GetSprayData(string OrderID, string SprayID, string
            ProductLineId, string GlueType, string SprayDistance, string GlueLinePosition,
            string AtomizationAdjustment, string MaterialAdjustment,
            string BakingTemperature, string BakingTime, string Keyword
            , int? PageIndex = 0, int? PageSize = 5)
        {
            int startRow = PageIndex.Value * PageSize.Value;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM huabao.`Spray` ");
            if (OrderID != null || SprayID != null || ProductLineId != null || GlueType != null || SprayDistance != null
                || GlueLinePosition != null || AtomizationAdjustment != null || MaterialAdjustment != null || BakingTemperature != null
                || BakingTime != null || Keyword != null)
            {
                sb.Append(" where 1=1 ");
            }
            if (Keyword != null)
            {
                string strWhereKeyWord = string.Format("and  OrderID like '%{0}%'  " +
                    "or GlueType like  '%{0}%' or SprayID like  '%{0}%'  or ProductLineId   like  '%{0}%'  ", Keyword);
                sb.Append(strWhereKeyWord);
            }
            if (GlueType != null)
            {
                sb.Append(string.Format(" and GlueType='{0}'", GlueType));
            }
            if (OrderID != null)
            {
                sb.Append(string.Format(" and OrderID='{0}'", OrderID));
            }

            string strWhereLimit = string.Format(" LIMIT {0},{1}", startRow, PageSize.Value);
            sb.Append(strWhereLimit);
            DataTable db=MySqlHelper.ExecuteQuery(sb.ToString());
            Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
            dic.Add("Spray", db);
            return JsonConvert.SerializeObject(dic);
        }
    }
}
