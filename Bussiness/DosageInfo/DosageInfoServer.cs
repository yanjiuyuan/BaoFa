using Common.DbHelper;
using Common.JsonHelper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.DosageInfo
{
    public class DosageInfoServer
    {
        public string GetDosageInfo(string OrderId, string ChildId)
        {
            //string strSearchSql = "SELECT WeiTiaoConsumption,HuChiConsumption,BiaoQianConsumption,DaDiConsumption FROM huabao.`Usage` " +
            //    " WHERE orderid =@Orderid AND childid =@Childid  ORDER BY CT DESC   LIMIT 0,1";
            //string strSearchSql = "SELECT WeiTiaoConsumption,HuChiConsumption,BiaoQianConsumption,DaDiConsumption FROM huabao.`Usage` " +
            //   " WHERE OldN =@OldN  ORDER BY CT DESC   LIMIT 0,1";
            //MySqlParameter[] parameter = new MySqlParameter[] {
            //   // new MySqlParameter("@Orderid",MySqlDbType.VarChar),
            //   // new MySqlParameter ("@Childid",MySqlDbType.VarChar
            //    new MySqlParameter ("@OldN",MySqlDbType.String),
            //};
            
            string strSearchSql = string.Format("SELECT WeiTiaoConsumption,HuChiConsumption,BiaoQianConsumption,DaDiConsumption FROM huabao.`Usage` WHERE orderid ='{0}' AND childid ='{1}'  ORDER BY CT DESC   LIMIT 0,1", OrderId, ChildId);
            //string strSearchSql = "SELECT * FROM huabao.`Usage`";
            DataTable db = Common.DbHelper.MySqlHelper.ExecuteQuery(strSearchSql);
            string strJsonString = JsonConvert.SerializeObject(db);
            //string strJsonString = JsonHelper.DataTableToJson(db);
            
            return strJsonString;
        }

    }
}
