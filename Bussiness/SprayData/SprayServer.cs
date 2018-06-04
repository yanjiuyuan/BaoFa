using Common.DbHelper;
using Common.JsonHelper;
using Common.LogHelper;
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

        private static Logger logger = Logger.CreateLogger(typeof(SprayServer));
        public string  GetSprayData(string OrderID, string SprayID, string
            ProductLineId, string GlueType, string SprayDistance, string GlueLinePosition,
            string AtomizationAdjustment, string MaterialAdjustment,
            string BakingTemperature, string BakingTime, string Keyword
            , int? PageIndex = 0, int? PageSize = 5)
        {
            try
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
                    string strWhereKeyWord = string.Format(
                        " and ( GlueType like  '%{0}%' or SprayID like  '%{0}%' ) ", Keyword);
                    sb.Append(strWhereKeyWord);
                }
                if (GlueType != null)
                {
                    sb.Append(string.Format(" and GlueType='{0}' ", GlueType));
                }
                if (OrderID != null)
                {
                    sb.Append(string.Format(" and OrderID='{0}'", OrderID));
                }
                if (BakingTime != null)
                {
                    sb.Append(string.Format(" and BakingTime='{0}'", BakingTime));
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

        public string GetSprayCoordinate(string  id)
        {
            try { 
            string key = "设备1";
            Dictionary<string, string> CoorDic = new Dictionary<string, string>();
            List<string> list=  RedisHash.GetHashKeys(key);

           for(int i=0;i<list.Count-1;i++)
            {
                string val = RedisHash.GetValueFromHash(key, list[i]);
                CoorDic.Add(list[i], val);
            }


            return JsonConvert.SerializeObject(CoorDic);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }


        }



    }
}
