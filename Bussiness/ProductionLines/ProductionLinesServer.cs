using Common.DbHelper;
using Common.JsonHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.ProductionLines
{
    public class ProductionLinesServer
    {
        public string GetProductionLinesData(string ProductLineId, string ProductLineName
            , string CompanyId, string telephone, string registertime,
            string role, string status,
            string IsEnable, string KeyWord
            , int? PageIndex = 0, int? PageSize = 5)
        {
            int startRow = PageIndex.Value * PageSize.Value;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM huabao.`productlineinfo` ");
            if (ProductLineId != null || ProductLineName != null || CompanyId != null || telephone != null || registertime != null
                || role != null || status != null || IsEnable != null || KeyWord != null )
            {
                sb.Append(" where 1=1 ");
            }
            if (KeyWord != null)
            {
                string strWhereKeyWord = string.Format(
                    " and ( ProductLineId like  '%{0}%' " +
                    "   or  ProductLineName like  '%{0}%' " +
                      "   or  CompanyId like  '%{0}%' " +
                       "   or  telephone like  '%{0}%' " +
                    "or role like  '%{0}%' ) ", KeyWord);
                sb.Append(strWhereKeyWord);
            }
            if (ProductLineName != null)
            {
                sb.Append(string.Format(" and ProductLineName='{0}' ", ProductLineName));
            }
            if (ProductLineId != null)
            {
                sb.Append(string.Format(" and ProductLineId='{0}'", ProductLineId));
            }
            if (CompanyId != null)
            {
                sb.Append(string.Format(" and CompanyId='{0}'", CompanyId));
            }
            if (telephone != null)
            {
                sb.Append(string.Format(" and telephone='{0}'", telephone));
            }
            int iRows = MySqlHelper.ExecuteQuery(sb.ToString()).Rows.Count;
            string strWhereLimit = string.Format(" LIMIT {0},{1}", startRow, PageSize.Value);

            sb.Append(strWhereLimit);
            DataTable db = MySqlHelper.ExecuteQuery(sb.ToString());
            //Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
            //dic.Add("Spray", db);
            string strJsonString = JsonHelper.DataTableToJson(db, iRows);
            return strJsonString;
        }



        public DataTable GetLinesList()
        {

            string strsql = "select a.ProductLineId ,a.ProductLineName,b.CompanyId,b.CompanyName from productlineinfo a left join  companyinfo b on a.CompanyId =b.CompanyId  where a.status =1";


            DataTable dt = MySqlHelper.ExecuteQuery(strsql);
       
            return dt;

        }
    }
}
