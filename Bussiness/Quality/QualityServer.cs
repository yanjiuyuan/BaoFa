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
        private static Logger logger = Logger.CreateLogger(typeof(QualityServer));
        public string GetQualityCounts(string OrderId, string ChildId, string StartTime, string EndTime)
        {
            try
            {
                if (StartTime == null)
                    StartTime = DateTime.Now.ToString("yyyy-MM-dd");
                if (EndTime == null)
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                StartTime = StartTime + " 00:00:00";
                EndTime = EndTime + " 23:59:59";

                string dbsql = "  SELECT  sum( if(b.`QualityType`=1,1,0)) as Goods,  sum( if (b.`QualityType`= 2,1,0)) as Bads, sum( if (b.`QualityType`= 3,1,0)) as Inferior " +
                    "FROM `usage` a INNER JOIN `Quality` b ON a.`ID_Usage`= b.`ID_Usage` ";
                StringBuilder sb = new StringBuilder();
                 sb.Append(dbsql);
                sb.Append(string.Format(" where  ct BETWEEN '{0}' AND  '{1}'", StartTime, EndTime));
                if (OrderId != null)
                {
                    sb.Append(string.Format(" and a.`OrderID`='{0}'", OrderId));
                }
                if (ChildId != null)
                {
                    sb.Append(string.Format(" and a.`ChildID`='{0}'", ChildId));
                }
                DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
                int iGoods=0, iBads=0, iInferior=0;
                if (tb.Rows.Count>0)
                {
                    object obj = tb.Rows[0]["Goods"];

                    iGoods = int.Parse(tb.Rows[0].IsNull("Goods")? "0": tb.Rows[0]["Goods"].ToString());
                    iBads = int.Parse(tb.Rows[0].IsNull("Bads") ? "0": tb.Rows[0]["Bads"].ToString());
                    iInferior = int.Parse(tb.Rows[0].IsNull("Inferior") ? "0": tb.Rows[0]["Inferior"].ToString());
                }
                Dictionary<string, int> dic = new Dictionary<string, int>();
             
                
                dic.Add("Goods", iGoods);
                dic.Add("Bads", iBads);
                dic.Add("Inferior", iInferior);
                string strJsonString = JsonConvert.SerializeObject(dic);
                return strJsonString;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }
 

        public string GetBatchQuality(string OrderId, string ChildId, string StartTime, string EndTime)
        {
            try {

                if (StartTime == null)
                    StartTime = DateTime.Now.ToString("yyyy-MM-dd");
                if (EndTime == null)
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                StartTime = StartTime + " 00:00:00";
                EndTime = EndTime + " 23:59:59";


                string strSql = " SELECT  round(sum( b.`AppearanceQualified`) /sum(1) ) as AppearanceQualified," +
                                " round(sum(b.`AppearanceAfterQualified`) / sum(1)) as AppearanceAfterQualified," +
                                " round(sum(b.`VampPullQualified`*100) / sum(1) ) as VampPullQualified," +
                                 " round(sum(b.`DaDiPullQualified`*100) / sum(1) )as DaDiPullQualified," +
                                 " round(sum(b.`ZheWangQualified`) / sum(1)) as ZheWangQualified" +
                                "  FROM    `usage` a INNER JOIN `Quality` b ON a.`ID_Usage`= b.`ID_Usage` ";



                StringBuilder sb = new StringBuilder();
                sb.Append(strSql);
               sb.Append(string.Format(" WHERE ct BETWEEN '{0}' AND  '{1}'     ", StartTime, EndTime));
               
                
                if (OrderId != null)
                {
                    sb.Append(string.Format(" and a.`OrderID`='{0}'", OrderId));
                }
                if (ChildId != null)
                {
                    sb.Append(string.Format(" and a.`ChildID`='{0}'", ChildId));
                }
                 
                DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
                string strJsonString = string.Empty;
                if (tb.Rows.Count > 0)
                {
                     
                    tb.TableName = "Quality";
                    strJsonString = JsonHelper.DataTableToJson(tb);
 

                }
                else
                {
                    return Global.RETURN_EMPTY;
                }
                return strJsonString;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
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



        public string GetMonQuality(string StartDate, string EndDate, bool MultiMon = true)
        {

            try
            {
                //同一个月
                if (!MultiMon)
                {


                    string strSql = " SELECT datestr, round(sum( b.`AppearanceQualified`) /sum(1) ) as AppearanceQualified," +
                                    " round(sum(b.`AppearanceAfterQualified`) / sum(1)) as AppearanceAfterQualified," +
                                    " round(sum(b.`VampPullQualified`*100) / sum(1) ) as VampPullQualified," +
                                     " round(sum(b.`DaDiPullQualified`*100) / sum(1) )as DaDiPullQualified," +
                                     " round(sum(b.`ZheWangQualified`) / sum(1)) as ZheWangQualified" +
                                    "  FROM   ( select left(a.CT, 10) as datestr,t.* from `usage` a INNER JOIN `Quality` t ON a.`ID_Usage`= t.`ID_Usage` ";



                    StringBuilder sb = new StringBuilder();
                    sb.Append(strSql);

                    if (StartDate != null || EndDate != null)
                    {
                        sb.Append(string.Format(" WHERE ct BETWEEN '{0}' AND  '{1}'     ", StartDate, EndDate));
                    }

                    sb.Append(" )b group by datestr order by datestr ");
                    DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
                    string strJsonString = string.Empty;
                    if (tb.Rows.Count > 0)
                        strJsonString = JsonHelper.DataTableToJson(tb);

                    else
                    {
                        return Global.RETURN_EMPTY;
                    }
                    return strJsonString;
                }
                else
                {

                    string strSql = " SELECT datestr, round(sum( b.`AppearanceQualified`) /sum(1) ) as AppearanceQualified," +
                                    " round(sum(b.`AppearanceAfterQualified`) / sum(1)) as AppearanceAfterQualified," +
                                    " round(sum(b.`VampPullQualified`*100) / sum(1) ) as VampPullQualified," +
                                     " round(sum(b.`DaDiPullQualified`*100) / sum(1) )as DaDiPullQualified," +
                                     " round(sum(b.`ZheWangQualified`) / sum(1)) as ZheWangQualified" +
                                    "  FROM   ( select left(a.CT, 7) as datestr,t.* from `usage` a INNER JOIN `Quality` t ON a.`ID_Usage`= t.`ID_Usage` ";



                    StringBuilder sb = new StringBuilder();
                    sb.Append(strSql);

                    if (StartDate != null || EndDate != null)
                    {
                        sb.Append(string.Format(" WHERE ct BETWEEN '{0}' AND  '{1}'     ", StartDate, EndDate));
                    }

                    sb.Append(" ) b group by datestr  order by datestr ");
                    DataTable tb = MySqlHelper.ExecuteQuery(sb.ToString());
                    string strJsonString = string.Empty;
                    if (tb.Rows.Count > 0)
                        strJsonString = JsonHelper.DataTableToJson(tb);

                    else
                    {
                        return Global.RETURN_EMPTY;
                    }
                    return strJsonString;



                }
            }
            
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
     }



    }
}
