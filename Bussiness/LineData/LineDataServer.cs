using Common.DbHelper;
using Common.JsonHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.LineData
{
    public class LineDataServer
    {
        public  string GetSprayMessage()
        {

            string strSql = "SELECT * FROM huabao.`spray` WHERE orderid=(SELECT orderid  FROM huabao.`usage` ORDER BY CT DESC LIMIT 0,1);";
            
            DataTable newTb=  MySqlHelper.ExecuteQuery(strSql);
            string strJsonString = string.Empty;
            strJsonString =  JsonHelper.DataTableToJson(newTb);

            return strJsonString;
        }

        public DataTable GetTableMessage(string strTableName)
        {
            string strSql = string.Format("SELECT  * FROM huabao.`{0}` ORDER  BY {1}id DESC LIMIT 0,1;", strTableName,strTableName);

            DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
            //string strJsonString = string.Empty;
            //strJsonString = JsonHelper.DataTableToJson(newTb);
            //strJsonString = JsonConvert.SerializeObject(newTb, new DataTableConverter());
            return newTb;
        }
    }
}
