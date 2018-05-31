using Common.DbHelper;
using Common.JsonHelper;
using Common.LogHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Quality
{
    public class QualityServer
    {
        public string GetQualityCounts(string OrderId, string ChildId, string StartTime, string EndTime)
        {
            try
            {
                Dictionary<string, int> dic = new Dictionary<string, int>();
                int iGoods, iBads, iInferior;
                iGoods = Search(OrderId, ChildId, StartTime, EndTime, 1);
                iBads = Search(OrderId, ChildId, StartTime, EndTime, 2);
                iInferior = Search(OrderId, ChildId, StartTime, EndTime, 3);
                dic.Add("Goods", iGoods);
                dic.Add("Bads", iBads);
                dic.Add("Inferior", iInferior);
                string strJsonString = JsonConvert.SerializeObject(dic);
                return strJsonString;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Search(string OrderId, string ChildId, string StartTime, string EndTime,int iValue)
        {
            string strSql = "SELECT  COUNT(b.`QualityType`) as Counts   FROM `usage` a INNER JOIN `Quality` b ON a.`ID_Usage`= b.`ID_Usage`  ";
            StringBuilder sb = new StringBuilder();
            sb.Append(strSql);
            if (OrderId != null)
            {
                sb.Append(string.Format(" and a.`OrderID`='{0}'", OrderId));
            }
            if (ChildId != null)
            {
                sb.Append(string.Format(" and a.`ChildID`='{0}'", ChildId));
            }
            if (StartTime != null || EndTime != null)
            {
                sb.Append(string.Format(" and ct BETWEEN '{0}' AND  '{1}'", StartTime, EndTime));
            }
            sb.Append(string.Format(" and QualityType = {0}", iValue));
            DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
            int iResult;
            iResult = Int32.TryParse(tb.Rows[0][0].ToString(),out iResult)?iResult:0;
            return iResult;
        }

        public string GetBatchQuality(string OrderId, string ChildId, string StartTime, string EndTime)
        {
            string strSql = "SELECT a.`OrderID`,a.`ChildID`,b.`AppearanceQualified`,b.`AppearanceAfterQualified`,b.`VampPullQualified`,b.`DaDiPullQualified`,b.`ZheWangQualified`,ct FROM `usage` a INNER JOIN `Quality` b ON a.`ID_Usage`= b.`ID_Usage`  ";
            StringBuilder sb = new StringBuilder();
            sb.Append(strSql);
            if (OrderId != null)
            {
                sb.Append(string.Format(" and a.`OrderID`='{0}'", OrderId));
            }
            if (ChildId != null)
            {
                sb.Append(string.Format(" and a.`ChildID`='{0}'", ChildId));
            }
            if (StartTime != null || EndTime != null)
            {
                sb.Append(string.Format(" and ct BETWEEN '{0}' AND  '{1}'", StartTime, EndTime));
            }
            DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
            string strJsonString = string.Empty;
            if (tb.Rows.Count > 0)
            {
                int iCount = tb.Rows.Count;
                float faveVampPullQualified = 0;
                float faveDaDiPullQualified = 0;
                float faveAppearanceQualified = 0;
                float faveAppearanceAfterQualified = 0;
                float faveZheWangQualified = 0;
                foreach (DataRow Row in tb.Rows)
                {
                    float fVampPullQualified = 0; float fDaDiPullQualified = 0;
                    Row["VampPullQualified"] = float.TryParse(Row["VampPullQualified"].ToString(), out fVampPullQualified) ? fVampPullQualified * 100 : 0;
                    Row["DaDiPullQualified"] = float.TryParse(Row["DaDiPullQualified"].ToString(), out fDaDiPullQualified) ? fDaDiPullQualified * 100 : 0;
                    faveVampPullQualified += (float)Row["VampPullQualified"];
                    faveDaDiPullQualified += (float)Row["DaDiPullQualified"];
                    faveAppearanceQualified += (float)Row["AppearanceQualified"];
                    faveAppearanceAfterQualified += (float)Row["AppearanceAfterQualified"];
                    faveZheWangQualified += (float)Row["ZheWangQualified"];
                }
                faveVampPullQualified = faveVampPullQualified / iCount;
                faveDaDiPullQualified = faveDaDiPullQualified / iCount;
                faveAppearanceQualified = faveAppearanceQualified / iCount;
                faveAppearanceAfterQualified = faveAppearanceAfterQualified / iCount;
                faveZheWangQualified = faveZheWangQualified / iCount;

                DataRow newRow; newRow = tb.NewRow();
                newRow["VampPullQualified"] = faveVampPullQualified;
                newRow["DaDiPullQualified"] = faveDaDiPullQualified;
                newRow["AppearanceQualified"] = faveAppearanceQualified;
                newRow["AppearanceAfterQualified"] = faveAppearanceAfterQualified;
                newRow["ZheWangQualified"] = faveZheWangQualified;
                tb.Rows.Add(newRow);
                tb.TableName = "Quality";
                strJsonString = JsonHelper.DataTableToJson(tb);

                //JObject jObject = (JObject)JsonConvert.SerializeObject(strJsonString);
                //strJsonString= JsonConvert.SerializeObject(strJsonString);

                //strJsonString = JsonConvert.SerializeObject(tb);

                //var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
                //strJsonString = JsonConvert.SerializeObject(tb, settings);

            }
            else
            {
                strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"暂无数据!\"}";
            }
            return strJsonString;
        }

        public class NullToEmptyStringResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                return type.GetProperties()
                        .Select(p =>
                        {
                            var jp = base.CreateProperty(p, memberSerialization);
                            jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                            return jp;
                        }).ToList();
            }
        }

        public class NullToEmptyStringValueProvider : IValueProvider
        {
            PropertyInfo _MemberInfo;
            public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
            {
                _MemberInfo = memberInfo;
            }

            public object GetValue(object target)
            {
                object result = _MemberInfo.GetValue(target, null);
                if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
                return result;

            }

            public void SetValue(object target, object value)
            {
                _MemberInfo.SetValue(target, value, null);
            }
        }



        public string GetMonQuality(string StartDate, string EndDate)
        {
            string strSql = " SELECT  round(sum( b.`AppearanceQualified`) /sum(1) ) as AppearanceQualified,"+
                            " round(sum(b.`AppearanceAfterQualified`) / sum(1)) as AppearanceAfterQualified," +
                            " round(sum(b.`VampPullQualified`*100) / sum(1) ) as VampPullQualified," +
                             " round(sum(b.`DaDiPullQualified`*100) / sum(1) )as DaDiPullQualified," +
                             " round(sum(b.`ZheWangQualified`) / sum(1)) as ZheWangQualified" +
                            "  FROM `usage` a INNER JOIN `Quality` b ON a.`ID_Usage`= b.`ID_Usage` ";
            StringBuilder sb = new StringBuilder();
            sb.Append(strSql);
            
            if (StartDate != null || EndDate != null)
            {
                sb.Append(string.Format(" WHERE ct BETWEEN '{0}' AND  '{1}'", StartDate, EndDate));
            }
            DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
            string strJsonString = string.Empty;
            if(tb.Rows.Count>0)
                strJsonString = JsonHelper.DataTableToJson(tb);
             
            else
            {
                strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"暂无数据!\"}";
            }
            return strJsonString;
        }



    }
}
