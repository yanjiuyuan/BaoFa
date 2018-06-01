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
        public string GetSprayMessage()
        {

            string strSql = "SELECT * FROM huabao.`spray` WHERE orderid=(SELECT orderid  FROM huabao.`usage` ORDER BY CT DESC LIMIT 0,1);";

            DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
            string strJsonString = string.Empty;
            strJsonString = JsonHelper.DataTableToJson(newTb);

            return strJsonString;
        }

        public DataTable GetTableMessage(string strTableName ,int lineid)
        {
            string strSql = string.Empty;
            //if (strTableName == "Usage")
            //{
            //    //strsql = string.format("select  * from huabao.`{0}` order  by  ct desc limit 0,1;", strtablename);
            //    strSql = "select  *  from  `usage`  ";
            //}
            //else
            //{
            strSql = string.Format("  select a.* from   `{0}` a  left join `usage` b on  a.ID_Usage =b.ID_Usage where a.ProductLineId={1}  ORDER  BY {2}id  DESC LIMIT 0,1 ", strTableName, lineid, strTableName);
            //}
            DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
            return newTb;
        }

        public DataTable GetLocationState()
        {
            string strSql = string.Format("SELECT * FROM `LocationState` WHERE endtime IS NULL");
            DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
            return newTb;
        }
    }
}
