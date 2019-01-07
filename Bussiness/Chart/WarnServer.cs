using Bussiness.Time;
using Common.Code;
using Common.DbHelper;
using Common.JsonHelper;
using Common.LogHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bussiness.Chart
{

    public class WarnServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(WarnServer));
        //获取各工位的末次生产时间
        public DataTable  WarnTypeList(string type)
        {
            string qursql = string.Empty;
             if (type == "A")
                qursql = "select paraid ,paraname from  parainfo where paratype='WARNTYPE_A' ORDER BY paraid";
            else if (type == "B")
                qursql = "select paraid ,paraname from  parainfo where paratype='WARNTYPE_B' ORDER BY paraid";
            else if (type == "C")
                qursql = "select paraid ,paraname from  parainfo where paratype='WARNTYPE_C' ORDER BY paraid";
            else
                  qursql = "select paraid ,paraname from  parainfo where paratype like 'WARNTYPE_%' ORDER BY paraid";

            DataTable dt = MySqlHelper.ExecuteQuery(qursql);

            return dt;

        }

        public DataTable GetWarnList(int  lineid ,string datestr)
        {
            string qursql = string.Empty;
            qursql = "select a.productiont ,a.warntype,a.warnphe,a.treatment,a.warntime,a.operator ,a.warndura,b.stationname,c.paraname as warntypedesc from  warnlog a left join" +
                " locationcfg b on  a.productlineid =b.productlineid and a.locationid=b.locationid " +
                " left join  parainfo  c on a.warntype=c.paraid " +
                " where a.productlineid=" + lineid + " and productiont='" + datestr + "'";
                
                  
            DataTable dt = MySqlHelper.ExecuteQuery(qursql);

            return dt;

        }
        public DataTable GetLocationList(int lineid)
        {
            string qursql = string.Empty;
             qursql = "select locationid ,stationname  from locationcfg    where productlineid=" + lineid  ;
            DataTable dt = MySqlHelper.ExecuteQuery(qursql); 
            return dt; 
        }
        public DataTable GetLineList()
        {
            string qursql = string.Empty;
           
            qursql = "select productlineid ,productlinename from productlineinfo";
            DataTable dt = MySqlHelper.ExecuteQuery(qursql);
            return dt;
        }

        public int WarnReg(int lineid, string productiont,int ? locationid,string warntype
            ,string warnphe, string treatment,string warntime,string opr,int ? warndura)
        {
            string  sql = string.Empty;
            sql =    string.Format("insert into warnlog (productlineid,productiont,locationid,warntype,warnphe,treatment,warntime,ct,operator) " +
               "values({0},'{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}',{9})",
               lineid, productiont, locationid, warntype, warnphe, treatment, warntime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), opr, warndura);
                

            int num= MySqlHelper.ExecuteSql(sql);

            return num;

        }
    }
}
