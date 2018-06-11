using Common.DbHelper;
using Common.JsonHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.LogHelper;
using Newtonsoft.Json;

namespace Bussiness.ProductionLines
{
    public class ProductionLinesServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(ProductionLinesServer));
        public string GetProductionLinesData(string ProductLineId, string ProductLineName
            , string CompanyId, string telephone, string registertime,
            string role, string status, string GroupId,
            string IsEnable, string KeyWord
            , int? PageIndex = 0, int? PageSize = 5)
        {

            try
            {
                int startRow = PageIndex.Value * PageSize.Value;
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from (SELECT a.*,c.GroupName  FROM huabao.`productlineinfo` a  left join huabao.`Companyinfo` b on a.companyid=b.companyid" +
                    " left join groupinfo c on b.groupid=c.groupid) t1  ");
                if (ProductLineId != null || ProductLineName != null || CompanyId != null || telephone != null || registertime != null
                    || role != null || status != null || IsEnable != null || KeyWord != null)
                {
                    sb.Append(" where 1=1 ");
                }
                if (KeyWord != null)
                {
                    string strWhereKeyWord = string.Format(
                        " and ( ProductLineId like  '%{0}%' " +
                        "   or  ProductLineName like  '%{0}%' " +
                          "   or  CompanyName like  '%{0}%' " +
                          "   or  GroupName like  '%{0}%' " +
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
               
                if (GroupId != null)
                {
                    sb.Append(string.Format(" and GroupId='{0}'", GroupId));
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
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }



        public string GetGroupList()
        {
            DataTable dt = new DataTable();
            try
            {
                string strsql = "select * from groupinfo";


                dt = MySqlHelper.ExecuteQuery(strsql);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
               
            }

            return JsonConvert.SerializeObject(dt);

        }


        public DataTable GetLinesList()
        {
            DataTable dt = new DataTable();
            try
            {
                string strsql = "select a.ProductLineId ,a.ProductLineName,b.CompanyId,b.CompanyName from productlineinfo a left join  companyinfo b on a.CompanyId =b.CompanyId  where a.status =1";


                dt = MySqlHelper.ExecuteQuery(strsql);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }

            return  dt;

        }
    }
}
