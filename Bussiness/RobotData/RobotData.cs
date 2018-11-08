using Common.DbHelper;
using Common.LogHelper;
using Newtonsoft.Json;
using RedisHelp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bussiness
{
    public static class RobotData
    {

        private static Dictionary<int, Dictionary<int, int>> linestage = new Dictionary<int, Dictionary<int, int>>();
         
        private static Logger logger = Logger.CreateLogger(typeof(RobotData));
        static Timer time1;
        static public List<string > datakeys = new List<string> ();

        static public Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
        static public Dictionary<string, List<string>> codeData = new Dictionary<string, List<string>>();
        static RobotData()
        {
            //读取code

            RedisHelper redis = new RedisHelper();


            //Dictionary<string, string> dic = redis.GetHashAllKV("currFileContent");
             

            //foreach (var x in dic.Keys)
            //{
            //    List<string> s = new List<string>(dic[x].Split('\n'));
            //    for (int i = 0; i < s.Count; i++)
            //        s[i] = i.ToString() + " " + s[i];

            //    codeData.Add(x, s);
            //}

            
            DataTable dt = MySqlHelper.ExecuteQuery("select deviceid from deviceinfo");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data.Add(dt.Rows[i]["deviceid"].ToString(), new Dictionary<string, string>());
                Global.rootstatelist.Add(dt.Rows[i]["deviceid"].ToString(), new RobotState());
                datakeys.Add(dt.Rows[i]["deviceid"].ToString());
            }
            time1 = new Timer(new TimerCallback(TimeTaskManager.Tick), null, 1000, 100);
            foreach(string s in datakeys)
            TimeTaskManager.AddTimer(1, 500, Query, s);
        
            //初始化产线气缸列表数据
            List<int> lineids = Global.GetLineList(null, null, null, null);
            for (int id = 0; id < lineids.Count; id++)
            {
                Dictionary<int, int> localdic = new Dictionary<int, int>();
                 for (int i = -3; i < 50; i++)
                {
                     localdic.Add(i, 0);

                }
                linestage.Add(lineids[id], localdic);
                 
            }
            string ip = Global.GetLocalIP();
            if ( ip.StartsWith("172"))
            { 
             
            MySqlHelper.ExecuteSql("update queuestat set num=0 ,CT=now() where DataType=0 ");


            for (int id = 0; id < lineids.Count; id++)
                {
                int lineid = lineids[id];
                string queuename = "Q_Line2Ser_" + lineid;
                string exchangenm = "E_Line2Ser";

                Common.RabbitMQ.RabbitMQHelper.ListenMqMsg(queuename, lineid, ProcessQG,exchangenm);
                TimeTaskManager.AddTimer(1, 10000, CheckMQ, lineid);
                }

            }


        }


        private  static void CheckMQ(object obj)
        {
            int lineid = int.Parse(obj.ToString());
            if(!Common.RabbitMQ.RabbitMQHelper.CheckMqStat(lineid))
          {
           
            string queuename = "Q_Line2Ser_" + lineid;
            string exchangenm = "E_Line2Ser";

            Common.RabbitMQ.RabbitMQHelper.ListenMqMsg(queuename, lineid, ProcessQG, exchangenm);
            }
        }
        private static void QueueAdd(int lineid, string localid )
      {
            MySqlHelper.ExecuteSql("update queuestat set num=num+1 ,CT=now() where DataType=0  and   ProductLineId=" + lineid +" and LocationName='" + localid+"'");

       }
       private static void QueueMinus(int lineid, string localid)
      {
            MySqlHelper.ExecuteSql("update queuestat set num= if(num-1>0,num-1,0) ,CT=now() where DataType=0  and   ProductLineId=" + lineid + "  and LocationName='" + localid+"'");
      }
        private static void ProcessQG(int lineid,string s)
    {
            //根据不同的气缸信号，对queuestat 这个表进行更新
             var dict = JsonConvert.DeserializeObject<IDictionary>(s);
            
            Dictionary<int, int> nowstatdic = linestage[lineid];
            //第一组套楦及贴腹
            //第一组套楦及贴腹
            
            //第1#图像站
            if (dict.Contains("I11"))
            {
                //Debug.Log("O4" + dict["O4"]);
                int val = int.Parse(dict["I11"].ToString());
                //1#阻挡气缸

                //1#工位完成
                if ((val & 2) >0) nowstatdic[1] =1; //到位
                if (nowstatdic[1] == 1 && (val & 2) <1) //到位完成
                {
                    nowstatdic[1] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "一次喷胶");
             
                }
                if ((val & 4) >0) nowstatdic[2] = 1;
                if (nowstatdic[2] == 1 && (val & 4) <1)
                {
                    nowstatdic[2] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "一次喷胶");
           
                }
            }

            //第四组鞋面喷胶站
            if (dict.Contains("I15"))
            {
                //Debug.Log("O6" + dict["O6"]);
                //Debug.Log("O8" + dict["O8"]);
                int val = int.Parse(dict["I15"].ToString());

                if ((val & 2)>0) nowstatdic[4] = 1;
                if (nowstatdic[4] == 1 && (val & 2) <1)
                {
                    nowstatdic[4] = 0; 
                    QueueMinus(lineid, "一次喷胶");
                    QueueAdd(lineid, "贴腹/压底");
                }
                if ((val & 8) > 0) nowstatdic[5] = 1;
                if (nowstatdic[5] == 1 && (val & 8) <1)
                {
                    nowstatdic[5] = 0; 
                    QueueMinus(lineid, "一次喷胶");
                    QueueAdd(lineid, "贴腹/压底");
                }  
            }
            
            
            //第二组压合
            if (dict.Contains("O2"))
            {
                //Debug.Log("O2" + dict["O2"]);
                int val = int.Parse(dict["O2"].ToString());
                //压底
                if ((val & 128) >1) nowstatdic[11] = 1; 
                if (nowstatdic[11] == 1 && (val & 128) <1)
                {
                    nowstatdic[11] = 0;
                    //敲平 +1
                    QueueAdd(lineid, "敲平/视觉2站");
                    QueueMinus(lineid, "贴腹/压底");
                } 
            }
           

           
 
            //2#图像站
            if (dict.Contains("I19"))
            {
                //Debug.Log("O10" + dict["O10"]);
                int val = int.Parse(dict["I19"].ToString());
                 
                if ((val & 2) >0) nowstatdic[14] = 1;
                if (nowstatdic[14] == 1 && (val & 2) <1)
                {
                    nowstatdic[14] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "喷处理剂");
                    QueueMinus(lineid, "敲平/视觉2站");
                }
                if ((val & 32) >0) nowstatdic[15] = 1;
                if (nowstatdic[15] == 1 && (val & 32) <1)
                {
                    nowstatdic[15] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "喷处理剂");
                    QueueMinus(lineid, "敲平/视觉2站");
                }
           

            }
            //围条一胶站
            if (dict.Contains("I23"))
            {
                // Debug.Log("O12" + dict["O12"]);
                //Debug.Log("O10" + dict["O10"]);
                int val = int.Parse(dict["I23"].ToString());

                if ((val & 2) >0) nowstatdic[17] = 1;
                if (nowstatdic[17] == 1 && (val & 2) <1)
                {
                    nowstatdic[17] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "二次喷胶");
                    QueueMinus(lineid, "喷处理剂");
                }
                if ((val & 8) >0) nowstatdic[18] = 1;
                if (nowstatdic[18] == 1 && (val & 8) <1)
                {
                    nowstatdic[18] =0;
                    //视觉2 +1
                    QueueAdd(lineid, "二次喷胶");
                    QueueMinus(lineid, "喷处理剂");
                }

              

            }

            //围条二胶站
            if (dict.Contains("I27"))
            {
                //Debug.Log("O14" + dict["O14"]);
                int val = int.Parse(dict["I27"].ToString());

                if ((val & 2) >0) nowstatdic[23] = 1;
                if (nowstatdic[23] == 1 && (val & 2) <1)
                {
                    nowstatdic[23] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "三次喷胶");
                    QueueMinus(lineid, "二次喷胶");
                }
                if ((val & 8) >0) nowstatdic[24] = 1;
                if (nowstatdic[24] == 1 && (val & 8) <1)
                {
                    nowstatdic[24] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "三次喷胶");
                    QueueMinus(lineid, "二次喷胶");
                }
                 

            }

            //围条三胶站
            if (dict.Contains("I31") )
            {
                //Debug.Log("O16" + dict["O16"]);
                //Debug.Log("O18" + dict["O18"]);
                int val = int.Parse(dict["I31"].ToString());
               
                if ((val & 2) >0) nowstatdic[27] = 1;
                if (nowstatdic[27] == 1 && (val & 2) <1)
                {
                    nowstatdic[27] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "滚压/护齿喷胶");
                    QueueMinus(lineid, "三次喷胶");
                }
                if ((val & 8) >1) nowstatdic[28] = 1;
                if (nowstatdic[28] == 1 && (val &8 )<1)
                {
                    nowstatdic[28] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "滚压/护齿喷胶");
                    QueueMinus(lineid, "三次喷胶");
                }
                 
            }
             
            //护齿喷胶站
            if (dict.Contains("I43"))
            {
                //Debug.Log("O24" + dict["O24"]);
                int val = int.Parse(dict["I43"].ToString());
                if ((val & 4) >0) nowstatdic[37] = 1;
                if (nowstatdic[37] == 1 && (val & 4) <1)
                {
                    nowstatdic[37] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "贴护齿/十字压");
                    QueueMinus(lineid, "滚压/护齿喷胶");
                }
                if ((val & 16) >0) nowstatdic[38] = 1;
                if (nowstatdic[38] == 1 && (val & 16) <1)
                {
                    nowstatdic[38] = 0;
                    //视觉2 +1
                    QueueAdd(lineid, "贴护齿/十字压");
                    QueueMinus(lineid, "滚压/护齿喷胶");
                }
                 
            }



            //左十字压
            if (dict.Contains("O25"))
            {

                //Debug.Log("O26" + dict["O26"]);
                int val = int.Parse(dict["O25"].ToString());
                if ((val & 64) >0) nowstatdic[42] = 1;
                if (nowstatdic[42] == 1 && (val & 64) <1)
                {
                    nowstatdic[42] = 0;
                    //视觉2 +1
                    QueueMinus(lineid, "贴护齿/十字压");
                }


            }

            //右十字压
            if (dict.Contains("O29"))
            {
                //Debug.Log("O28" + dict["O28"]);
                int val = int.Parse(dict["O29"].ToString());
                if ((val & 16) >0) nowstatdic[44] = 1;
                if (nowstatdic[44] == 1 && (val & 16)<1)
                {
                    nowstatdic[44] = 0;
                    //视觉2 +1
                    QueueMinus(lineid, "贴护齿/十字压");
                }

            }








        }
        public static void init()
            {

            }
        public static string  DataToStr<T>(Dictionary<string,T> DIC)
        {
            StringBuilder str = new StringBuilder();
            str.Append("{");
            List<string > keys = data.Keys.ToList<string>() ;
            for (int i=0;i< keys.Count;i++ )
            {
                str.Append("\""+ keys[i]+ "\":");
                 T item =  DIC[keys[i]];
                str.Append(DicToStr(item as Dictionary<string, string>));
                if (i != keys.Count - 1)
                    str.Append(",");
            }
             
            str.Append("}");
            return str.ToString();
        }
        static string DicToStr(Dictionary<string, string> DIC)
        {
            StringBuilder str = new StringBuilder();
            str.Append("{");
            List<string> keys = DIC.Keys.ToList<string>();
            for (int i = 0; i < keys.Count; i++)
            {
                str.Append("\"" + keys[i] + "\":");
                str.Append("\"" + DIC[keys[i]] + "\"");
                if(i!= keys.Count-1)
                 str.Append(",");
            }
                str.Append("}");
            return str.ToString();
        }
        static void Query(object obj)
        {
    
            string key = (string)obj; logger.Info(key);
            try {
            
             
              
                Dictionary<string, string> dic = new Dictionary<string, string>();
                    RedisHelper redis = new RedisHelper();
                    dic = redis.GetHashAllKV(key);
                   
                    logger.Info(key +(dic.ContainsKey("deviceId")? dic["deviceId"].ToString():"none"));
                  data[key] = dic;
                   
                
               // DealData(key, dic);

 
            }
            catch(Exception e)
            {
                logger.Info(e.Message);
            }


        }
        //private async static void DealData(string key, Dictionary<string, string> dic)
        //{
        //    //var t = AsyncBus(key,dic);
   
        //}
        static async Task<int>  AsyncBus(string key, Dictionary<string, string> dic)
        {
            return await Task.Run(() =>
            {
                //处理当前运行日志
                //获取当前运行状态
                //if(dic.ContainsKey("workStatus"))
                //{
                //    int runstat = 0;
                //    string runstate = dic["workStatus"].ToString();
                //    if (runstate == Global.robotRun)
                //        runstat = 0;
                //    else if (runstate == Global.robotStop)
                //        runstat = 1;
                //    else
                //        return 0;
                //    string begintm = "";
                //    string endtm = "";

                //    if (dic.ContainsKey("begintm"))
                //    {
                //        begintm = dic["begintm"].ToString();
                //    }
                //        if (begintm == "")
                //            begintm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                 
                //if (dic.ContainsKey("endtm"))
                //{
                //    endtm = dic["endtm"].ToString();
                //}
                //if (endtm == "")
                //    endtm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
             
                //   //检测到运行
                //    if(runstat!= Global.rootstatelist[key].runstat )
                //    { 
                //        string sql = "select * from runinfo where deviceid='" + key + "'";
                //        DataTable dt = MySqlHelper.ExecuteQuery(sql);
                //        if(dt.Rows.Count==0)
                //        {

                //            string sqlinsert = " insert into runinfo set ct='{0}', rundate='{1}'" +
                //       " ,deviceid='{2}',platid='{3}',runstat={4},begintime='{5}'   ";
                //            string sqlinsert1 = string.Format(sqlinsert, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                //                , DateTime.Now.ToString("yyyy-MM-dd"), key, key.Substring(0,2),
                //                runstat, begintm);
                //            int rnt = MySqlHelper.ExecuteSql(sqlinsert1);

                //            Global.rootstatelist[key].runstat = runstat;
                //        }
                //        else
                //        {

                     
                //           string sql1 = "update runinfo set endtime='"+ endtm+"' where deviceid='" + key + "'";
                //            int i = MySqlHelper.ExecuteSql(sql1);

                            
                //            string sql2 = "insert into runinfohist (ct,rundate,platid,runstat,begintime,endtime,deviceid) " +
 
                //            " select ct,rundate,platid,runstat,begintime,endtime,deviceid from runinfo   where deviceid='" + key + "'";
                //            int i1 = MySqlHelper.ExecuteSql(sql2);
                         

                          

                //            string sql4 = " update   runinfo set ct='{0}', rundate='{1}'" +
                //            " ,platid='{3}',runstat={4},begintime='{5}',endtime=null where deviceid='{2}'"; 
                //            string sql5 = string.Format(sql4, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                //                , DateTime.Now.ToString("yyyy-MM-dd"), key, key.Substring(0,2),
                //                runstat, begintm);
                //            int i3 = MySqlHelper.ExecuteSql(sql5);
                //            Global.rootstatelist[key].runstat = runstat;

                //        }

                //    }
                //}





                //if (dic.ContainsKey("alarmInfo"))
                //{
                //    int atype = 0;
                //    string alarmInfo = dic["alarmInfo"].ToString();
                //    string alarmtm =""; string alarmType = "";
                //    if (dic.ContainsKey("alarmtm"))
                //   {
                //        alarmtm = dic["alarmtm"].ToString();
                      
                //    }
                //    if (alarmtm == "")
                //        alarmtm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //    if (dic.ContainsKey("alarmType"))
                //    {
                //        alarmType = dic["alarmType"].ToString();
                //        }
                //    if (alarmType != "")
                //        atype = int.Parse(alarmType);
                
                //    //警报为空
                //    if(alarmInfo== "" && Global.rootstatelist[key].alarminfo != alarmInfo)
                //    {
                //        DataTable dt =MySqlHelper.ExecuteQuery("select max(id) as id from warnlog where deviceid = '" + key + "'");
                //        if (dt.Rows.Count == 0)
                //            return 0;

                //        int id = 0;
                //       int.TryParse(dt.Rows[0]["id"].ToString(), out id);
                //        string sql = "update warnlog t1 set t1.endtime='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"'  where  "+
                //        "    t1.id ="+ id;

                //        MySqlHelper.ExecuteSql(sql);
                //        Global.rootstatelist[key].alarm = 0;
                //        Global.rootstatelist[key].alarminfo = "";
                //    }
                   
                //    else if (alarmInfo != "" && Global.rootstatelist[key].alarminfo != alarmInfo)
                //    {

                //        DataTable dt = MySqlHelper.ExecuteQuery("select max(id) as id from warnlog where deviceid = '" + key + "'");
                //        if (dt.Rows.Count == 0)
                //            return 0;

                //        int id = 0;
                //        int.TryParse(dt.Rows[0]["id"].ToString(), out id);
                //        string sql = "update warnlog t1 set t1.endtime='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'  where  " +
                //        "    t1.id =" + id;

                //        MySqlHelper.ExecuteSql(sql);

                //        string sql22 = "insert into warnlog set  ct='{0}' ," +
                //        "warndate='{1}',deviceid='{2}',platid='{3}',warntype={4},warnmsg='{5}'" +
                //        ",begintime='{6}'";
                //        string sql222 = string.Format(sql22, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                //            , DateTime.Now.ToString("yyyy-MM-dd"), key, key.Substring(0, 2),
                //            atype, alarmInfo, alarmtm);
                //        MySqlHelper.ExecuteSql(sql222);
                //        Global.rootstatelist[key].alarminfo = alarmInfo;
                //        Global.rootstatelist[key].alarm = 1;
                //    }



                //}







                //异步执行一些任务
                return 1;                               //异步执行完成标记
            });
        }

    }
}
