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
using System.Collections;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Bussiness.ProductionLines
{
    public class RobotServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(RobotServer));
        public string QueryRobotInfo( string model )
        {

            try
            {
               string key = model;
                Dictionary<string, string> CoorDic = new Dictionary<string, string>();
                CoorDic = RedisHash.GetAllEntriesFromHash(key);



                string strJsonString = JsonConvert.SerializeObject(CoorDic);
                return strJsonString;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }
        public string DeviceList(int lineid  )
        {
            List<Hashtable> lists = new List<Hashtable>();
            try { 
            string s=" select deviceid,devicemodel from deviceinfo t where ProductLineId=" + lineid;
            DataTable db = MySqlHelper.ExecuteQuery(s);
            if(db.Rows.Count>0)
            {
                foreach (DataRow dr in db.Rows)
                { 
                     Hashtable ht = new Hashtable();
                    ht.Add("deviceid", dr["deviceid"]);
                    ht.Add("model", dr["devicemodel"]);
                    ht.Add("configxml", QueryRobotConfig(dr["devicemodel"].ToString()));
                    lists.Add(ht);


                }

            }
            }
            catch(Exception  ex)
            {
                logger.Error(ex.Message);
            }
            return JsonConvert.SerializeObject(lists);

        }
        public string QueryRobotConfig(string model)
        {
            string retstr = string.Empty;
            try
            {

                Assembly assembly = Assembly.Load("Bussiness");
                Stream stream = assembly.GetManifestResourceStream("Bussiness.RobotConfig.xml");



                   XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);
                  XmlNodeList nodes = xmlDoc.SelectSingleNode("Root").ChildNodes;

                    foreach (XmlNode node in nodes)
                    {
                        if (node.SelectSingleNode("Model").InnerText.Equals(model))
                        {
                              retstr = node.OuterXml;
                        }



                    }
                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }

            byte[] b = System.Text.Encoding.Default.GetBytes(retstr);
            //转成 Base64 形式的 System.String  
            return Convert.ToBase64String(b);
            

        }
    }
}
