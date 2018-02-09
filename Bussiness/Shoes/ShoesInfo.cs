using Common.DbHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Shoes
{
    public class ShoesInfo
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="RFID"></param>
        /// <param name="tableNameList"></param>
        /// <returns></returns>
        public string GetShoesInfo(string RFID, List<string> tableNameList)
        {
            foreach (var item in tableNameList)
            {

            }
            return "";
        }


        /// <summary>
        /// 时时表数据检索
        /// </summary>
        /// <param name="RFID"></param>
        /// <param name="strTableNames">表明</param>
        /// <returns></returns>
        public DataTable SearchInfo(string RFID, string strTableNames)
        {
            string strSql = string.Format("SELECT * FROM `{1}`  WHERE RFIdn={0}", RFID, strTableNames);
            DataTable db = MySqlHelper.ExecuteQuery(strSql);
            return DataHanding(db);
        }


        /// <summary>
        /// 数据逻辑处理
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public DataTable DataHanding(DataTable db)
        {
            foreach (DataRow row in db.Rows)
            {
                //foreach (var items in row[""])
                //{

                //}
            }
            return db;
        }

    }
}
