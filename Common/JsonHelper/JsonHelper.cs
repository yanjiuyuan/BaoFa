using Common.LogHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.JsonHelper
{
  public static class JsonHelper
    {
        private static JsonSerializerSettings _jsonSettings;

        static JsonHelper()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            _jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);
        }
        
        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            try
            {
                if (null == obj)
                    return null;

                return JsonConvert.SerializeObject(obj, Formatting.None, _jsonSettings);
            }
            catch (Exception ex)
            {
                //Logger.LogManager.Error(new Logging.ExceptionLogInfo()
                //{
                //    ExceptionClassName = "YY.SZYD.Shop.Common.Utils.JsonHelper",
                //    ExceptionMethod = "ToJson",
                //    ExceptionNote = "Json序列化出错",
                //    RequestInfo = obj.GetType().FullName,
                //},
                //ex);

                return null;
            }
        }

        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                //Logging.LogManager.Error(new Logging.ExceptionLogInfo()
                //{
                //    ExceptionClassName = "YY.SZYD.Shop.Common.Utils.JsonHelper",
                //    ExceptionMethod = "ToJson",
                //    ExceptionNote = "Json序列化出错",
                //    RequestInfo = json,
                //},
                //ex);
                return default(T);
            }
        }

        /// <summary>
        /// 返回JsonString
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <returns></returns>
        public static string DataTableToJsonWithJsonNet<T>(T tValue)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(tValue);
            return jsonString;
        }

        /// <summary>  
        /// table转json  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"Name\":\"" + dt.TableName + "\",\"Rows");
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }


        /// <summary>  
        /// table转json  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <param name="iCounts">查询行数</param>  
        /// <returns></returns>  
        public static string DataTableToJson(DataTable dt, int iCounts)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"Name\":\"" + dt.TableName +"\""+","+"\"Counts\":"+"\""+iCounts.ToString()+"\"" + ",\"Rows");
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            if (iCounts > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>  
        /// dataset转Json  
        /// </summary>  
        /// <param name="ds"></param>  
        /// <returns></returns>  
        public static string DatasetToJson(System.Data.DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"Tables\":");
            json.Append("[");
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                json.Append(DataTableToJson(dt));
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            json.Append("}");
            return json.ToString();
        }
    }
}
