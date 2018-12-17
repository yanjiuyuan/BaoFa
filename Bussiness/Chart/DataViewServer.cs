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

    public class DataViewServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(ChartBeatServer));
        //获取各工位的末次生产时间
        public string GetData(int lineid = 1)
        {
             
            Dictionary<string, Object> dv = new Dictionary<string, Object>();
            //关键字订单
            //产线当前订单
            // 总订单号  已完成产品数   当日完成产品数    当日子订单编号                    
            int idusage = Global.GetLstUsageId(lineid);

            //订单 当班产量
            string sql = "select OrderID	,ChildID,NowN,AllN from `usage` where ID_usage= " + idusage;
            DataTable usagedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            string OrderID = string.Empty;
            string ChildID = string.Empty;
            string NowN = string.Empty;
            string AllN = string.Empty;

            if (usagedt.Rows.Count >= 0)
            {
                OrderID = usagedt.Rows[0]["OrderID"].ToString();
                ChildID = usagedt.Rows[0]["ChildID"].ToString();
                NowN = usagedt.Rows[0]["NowN"].ToString();
                AllN = usagedt.Rows[0]["AllN"].ToString();
            }
            sql = "select OrderN,Customer,OrdTime,DeliveryTime from `order` where OrderID='" + OrderID + "'";

            //订单情况
            DataTable orderdt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            string OrderN = string.Empty;
            string Customer = string.Empty;
            string OrdTime = string.Empty;
            string DeliveryTime = string.Empty;
            if (orderdt.Rows.Count >= 0)
            {
                Customer = orderdt.Rows[0]["Customer"].ToString();
                OrdTime = orderdt.Rows[0]["OrdTime"].ToString();
                DeliveryTime = orderdt.Rows[0]["DeliveryTime"].ToString();
                OrderN = orderdt.Rows[0]["OrderN"].ToString();
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("OrderID", OrderID);
            dic.Add("ChildID", OrderID);
            dic.Add("OrderN", OrderN);
            dic.Add("AllN", AllN);
            dic.Add("NowN", NowN);
            dic.Add("Customer", Customer);
            dic.Add("OrdTime", OrdTime);
            dic.Add("DeliveryTime", DeliveryTime);
            dv.Add("order", dic);

            //末班产线状态
            sql = "select linestatus  from  `line` where id_usage=" + idusage;


            DataTable Linedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            string Linestatus = string.Empty;
            if (Linedt.Rows.Count >= 0)
            {
                Linestatus = Linedt.Rows[0]["linestatus"].ToString();
            }

            int plan_t = 0;
            sql = "select ProductionTimes  from  productlineinfo where ProductLineId=" + lineid;
            DataTable pttb = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            if (pttb.Rows.Count >= 0)
            {
                plan_t = 3600 * (Convert.ToInt32(pttb.Rows[0]["ProductionTimes"].ToString()));
            }

            

            double run_t = 0;
            double warn_t = 0;
            double stop_t = 0;
            sql = " select stationstate, round((TIMESTAMPDIFF(SECOND, '1970-1-1 08:00:00', NOW())- starttime/1000),2)  as currtime from locationstatecache  where stationname ='生产线' ";
            DataTable Linetoday = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            if(Linetoday.Rows.Count>0)
            {
                string sstate = Linetoday.Rows[0]["stationstate"].ToString();
                double todaytm = Convert.ToDouble( Linetoday.Rows[0]["currtime"].ToString());
                if ("运行".Equals(sstate))
                    run_t = todaytm;
                else if("报警".Equals(sstate))
                    warn_t = todaytm;
                 else if ("停止".Equals(sstate))
                    stop_t = todaytm;

            }
             

            sql = " select   sum( if (stationstate = '运行', endtime - startTime, 0))/1000 AS run_t , sum( if   " +
       " (stationstate = '停止', endtime - startTime, 0))/ 1000 AS stop_t,    " +
       " sum( if (stationstate = '报警', endtime - startTime, 0))/ 1000 AS warn_t,     " +
       "  stationNAME from huabao.LocationState where id_usage = " + idusage + " and ProductLineId = " + lineid + " and stationName = '生产线'   " +
       " group by stationNAME ";

            DataTable Linetimedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);

            if (Linetimedt.Rows.Count > 0)
            {
                run_t += Convert.ToDouble(Linetimedt.Rows[0]["run_t"].ToString());
                stop_t += Convert.ToDouble(Linetimedt.Rows[0]["stop_t"].ToString());
                warn_t += Convert.ToDouble(Linetimedt.Rows[0]["warn_t"].ToString());

            }
            //产线利用率  run_t+free_t+warn_t /plan_t
            //计划利用率  run_t+free_t+warn_t /run_t+free_t+warn_t+启动时间
            //时间稼动率   run_t+free_t /run_t+free_t+warn_t
            if (run_t == 0 && warn_t == 0) warn_t = 1;
            //时间开动率
            double time_activation =Math.Round(100 * (run_t) / (run_t + warn_t),2) ;
            //设备利用率
            double plan_activation = Math.Round(100 * (run_t + warn_t) / plan_t, 2)  ;
            //产线稼动率
            double activation = Math.Round(100 * run_t / plan_t,2) ;


         


            //当日  七日平均
            //  设备综合效率 ：产线设备 OEE  运行时间/（运行时间+报警时间） * 加工件数* 理论运行周期 /运行时间  *合格率*100%
            //  加工件数 *理论运行周期 /（运行时间+报警时间） *合格率
            // 产能利用率：TEEP = 运行时间+报警时间 / 计划开机时间 * oee

            //产线综合效率曲线 过去一个月的日统计曲线

            string lstmonstr = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");

            sql = " select tt1.* ,tt2.RunT,tt2.WarnT,tt2.StopT,tt2.PlanPowerOnT,tt2.WorkLoad,tt3.CircleTime,tt2.PlanWorkLoad  from  " +
            " (select b.productlineid, left(CreateTime, 10) as ProductionT, sum(if (QualityType != null,1,0)) as all_num, sum(if (QualityType = 1,1,0)) as pass_num   from `usage` b left join  " +
             "  `quality` a on a.id_usage = b.id_usage   where b.productlineid = "+lineid+" and b.id_usage > (select min(id_usage) from  `usage`  where left(CreateTime,10)> '"+lstmonstr+ "') group by left(CreateTime, 10)) tt1 left join rptproductday tt2 on tt1.ProductionT = tt2.ProductionT  " +
             " left join productlineinfo tt3 on tt1.productlineid = tt3.productlineid where RunT  is not null  order by  tt1.ProductionT";
            DataTable efectlinedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            List<Dictionary<string, object>> efectlinelist = new List<Dictionary<string, object>>();
            if (efectlinedt.Rows.Count > 0)
            {
                for (int i = 0; i < efectlinedt.Rows.Count; i++)
                {
                    Dictionary<string, object> efectlinedic = new Dictionary<string, object>();
                    string ProductionT = efectlinedt.Rows[i]["ProductionT"].ToString();
                    int QA_all_num = 1; int QA_pass_num = 1;
                    if (efectlinedt.Rows[i]["all_num"] !=  System.DBNull.Value)
                    {
                        QA_all_num = Convert.ToInt32(efectlinedt.Rows[i]["all_num"].ToString());
                        QA_pass_num = Convert.ToInt32(efectlinedt.Rows[i]["pass_num"].ToString());
                    }
                    if(QA_all_num==0 )
                    {
                        QA_all_num = 1;
                        QA_pass_num = 1;
                    }
                    
                    int CircleTime = Convert.ToInt32(efectlinedt.Rows[i]["CircleTime"].ToString());
                    int WorkLoad = Convert.ToInt32(efectlinedt.Rows[i]["WorkLoad"].ToString());
                    double QA_RunT = Convert.ToDouble(efectlinedt.Rows[i]["RunT"].ToString());
                    double QA_WarnT = Convert.ToDouble(efectlinedt.Rows[i]["WarnT"].ToString());
                    double QA_PlanPowerOnT = Convert.ToDouble(efectlinedt.Rows[i]["PlanPowerOnT"].ToString());
                    double QA_PlanWorkLoad = Convert.ToDouble(efectlinedt.Rows[i]["PlanWorkLoad"].ToString());
                    //时间开动率
                    double TACT  = Math.Round(100 * (QA_RunT) / (QA_RunT + QA_WarnT), 2)  ;
                    //设备利用率
                    double DACT  = Math.Round(100 * (QA_RunT + QA_WarnT) / QA_PlanPowerOnT, 2)  ;
                    //产线稼动率
                    double ACT  = Math.Round(100 * QA_RunT / QA_PlanPowerOnT, 2)  ;


                    double OEE = 100 * (WorkLoad * QA_PlanPowerOnT / ((QA_RunT + QA_WarnT)* QA_PlanWorkLoad)) * (QA_pass_num / QA_all_num);
                    double TEEP = OEE * ((QA_RunT + QA_WarnT) / QA_PlanPowerOnT);
                    efectlinedic.Add("ProductionT", ProductionT);
                    efectlinedic.Add("WorkLoad", WorkLoad);
                    efectlinedic.Add("TACT", TACT);
                    efectlinedic.Add("DACT", DACT);
                    efectlinedic.Add("ACT", ACT);
                    efectlinedic.Add("OEE", Math.Round(OEE,2));
                    efectlinedic.Add("TEEP", Math.Round(TEEP,2));
                    efectlinelist.Add(efectlinedic);
                }



            }
            dv.Add("line_oee", efectlinelist);

            //产线当日警报情况
            sql = "   select tt2.*,tt1.warn_t ,tt1.warn_c from " +
      "   (select '生产线' as stationname,     sum( if (stationstate = '报警', endtime - startTime, 0))/1000 AS warn_t,  sum( if (stationstate = '报警',1, 0)) " +
     " AS warn_c   from huabao.LocationState where ID_USAGE = " + idusage + " and stationname = '生产线'   " +
      "             ) tt1 left join(select '生产线' as stationname, round((beginwarn - beginwork) / 1000) as firstwarninter, round((endwork - endwarn) / 1000) as lstwarninter, round((endwarn - beginwarn) / 1000) as warninter from( " +
     "   select(select min( if (StartTime is null,0,StartTime))  from locationstate where id_usage =  " + idusage + " and stationname = '生产线' and stationstate = '报警') as beginwarn " +
    "   , (select min(if (StartTime is null,0,StartTime)) from locationstate where id_usage =  " + idusage + " and stationname = '生产线') as beginwork, " +
   "    (select max(if (endtime is null,0,endtime)) from locationstate where id_usage =  " + idusage + " and stationname = '生产线') as endwork, " +
   "     (select max(if (endtime is null,0,endtime))  from locationstate where id_usage =  " + idusage + " and stationname = '生产线' and stationstate = '报警') as endwarn) tt )tt2 on " +
   "     tt1.stationname = tt2.stationname  ";
            DataTable warnlinedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            Dictionary<string, object> warnlinedic = new Dictionary<string, object>();
            if (warnlinedt.Rows.Count > 0)
            {
                int warnc = Convert.ToInt32(warnlinedt.Rows[0]["warn_c"]==System.DBNull.Value?"0": warnlinedt.Rows[0]["warn_c"].ToString());
                warnlinedic.Add("warn_c", warnc);

                double warnt = 0;
                double avgwarntm = 0;
                double avgwarninter = 0;
           
                int firstwarninter = 0;
                if (warnc > 0)
                {
                    firstwarninter = Convert.ToInt32(warnlinedt.Rows[0]["firstwarninter"].ToString());
                    int lstwarninter = Convert.ToInt32(warnlinedt.Rows[0]["lstwarninter"].ToString());
                    int warninter = Convert.ToInt32(warnlinedt.Rows[0]["warninter"].ToString());

                    warnt = Convert.ToDouble(warnlinedt.Rows[0]["warn_t"].ToString());
                    avgwarntm = warnt / warnc > 0 ? warnc : 1;
                    avgwarninter = (warninter - warnt) / warnc > 1 ? warnc - 1 : 1;
                }
                    warnlinedic.Add("firstwarninter", firstwarninter);
                    warnlinedic.Add("avgwarnt", Math.Round(avgwarntm,2));
                    warnlinedic.Add("avgwarninter", Math.Round(avgwarninter,2));

                 
            }
            dv.Add("line_warn_stat", warnlinedic);

            //产线1个月警报曲线
            sql = "select ProductionT,  WarnT,WarnC,FirstWarnInter,avgwarnt,avgwarninter from rptlineerrday where  productLineId=" + lineid
                + " and ProductionT > '" + lstmonstr + "' order by ProductionT";
            DataTable warnlinedt1 = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            if (warnlinedt1.Rows.Count > 0)
            {
                dv.Add("line_warn_curve",warnlinedt1);
            }
            string datestr7 = "0";
            sql = "  select min(productionT)  as productionT  from (select  distinct productionT  as productionT  from rptlocationday where productlineid = " + lineid + " and productionT >= '" + lstmonstr + "' order by productionT desc  limit 7) tt";
            DataTable min7datedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            if (min7datedt.Rows.Count > 0)
                datestr7 = min7datedt.Rows[0]["productionT"].ToString();
            //工位信息
            //实时工位状态  末次班次平均加工周期   七日平均加工周期



            sql = "  select if (tt1.run is null ,0,tt1.run) run,tt2.* from( "+
           "  select t21.*, t2.JobType, t2.LocationSeq,t2.state, t2.stationNAME from  " +
       "   (select Locationid, round(sum(RunT) /if (sum(RunC) > 0,sum(RunC),1)) as run7 from(select * from rptlocationday where productlineid = " + lineid + "  and productionT >='" + datestr7 + "') " +
       "   t group by Locationid ) t21 left join huabao.locationcfg t2   on t21.Locationid = t2.Locationid where t2.productlineid = "+ lineid + "   ) tt2 " +
        "  left join(select round(run_t /if (run_c > 0,run_c,1))as run ,stationNAME " +
            "  from(select   sum( if (stationstate = '运行', if ((endtime - startTime) < 300000,endtime - startTime,300000), 0))/ 1000 AS run_t, " +
             "    sum( if (stationstate = '运行',1, 0))  AS run_c, stationNAME   from huabao.LocationState where id_usage = " + idusage + " group by stationNAME ) as t1 " +
         "   )  tt1  on tt1.stationNAME = tt2.stationNAME  where tt2.state=1 order by tt2.LocationSeq ";

 

            DataTable stationstatedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            
            dv.Add("location_stat",  stationstatedt);
            //实时工序繁忙状态 +当日工序繁忙曲线 


            dv.Add("location_busy1", LocationBusyData.LocationBusyState1[lineid]);
            dv.Add("location_busy2", LocationBusyData.LocationBusyState2[lineid]);

            // 产线当日品检 
            // 末次品检数      当日品检合格率  次品率   废品率
            // 七日平均品检数  七日平均品检率

            double all_num = 0.0;
            double pass_rate = 0.0;
            double inferior_rate = 0.0;
            double waste_rate = 0.0;
            sql = "select all_num,round(100*pass_num/all_num) as pass_rate,round(100*inferior_num/all_num) as inferior_rate ,round(100*waste_num/all_num ) as  waste_rate from  " +

                    " (select sum(1) as all_num, sum(if (QualityType = 1,1,0)) as pass_num,sum(if (QualityType = 2,1,0)) as inferior_num ,sum(if (QualityType = 3,1,0)) as waste_num " +

                    " from quality where id_usage = " + idusage + ") t where all_num is not null";
            DataTable quadt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            Dictionary<string, double> quadic = new Dictionary<string, double>();

            if (quadt.Rows.Count > 0)
            {
                all_num = Convert.ToDouble(quadt.Rows[0]["all_num"].ToString());
                pass_rate = Convert.ToDouble(quadt.Rows[0]["pass_rate"].ToString());
                inferior_rate = Convert.ToDouble(quadt.Rows[0]["inferior_rate"].ToString());
                waste_rate = Convert.ToDouble(quadt.Rows[0]["waste_rate"].ToString());

            }
            quadic.Add("all_num", all_num);
            quadic.Add("pass_rate", Math.Round(pass_rate,2));
            quadic.Add("inferior_rate", Math.Round(inferior_rate,2));
            quadic.Add("waste_rate", Math.Round(waste_rate,2));

            double all_num7 = 0.0;
            double pass_rate7 = 0.0;
            double inferior_rate7 = 0.0;
            double waste_rate7 = 0.0;


            sql = "select distinct  `usage`.id_usage " +

       " from quality left join `usage` on quality.id_usage = `usage`.id_usage where  ProductLineId = 1 order by quality.id_usage desc limit 1 offset 7 ";
            DataTable quadt1 = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            int minusage = 0;
            if (quadt1.Rows.Count > 0)
            {
                minusage = Convert.ToInt32(quadt1.Rows[0]["id_usage"].ToString());
            }
            sql = "select all_num as all_num7,round(100*pass_num/all_num) as pass_rate7,round(100*inferior_num/all_num) as inferior_rate7 ,round(100*waste_num/all_num ) as  waste_rate7 from  " +

                     " (select sum(1) as all_num, sum(if (QualityType = 1,1,0)) as pass_num,sum(if (QualityType = 2,1,0)) as inferior_num ,sum(if (QualityType = 3,1,0)) as waste_num " +

                     " from quality left join `usage` on quality.id_usage = `usage`.id_usage  where quality.id_usage >= " + minusage + "  and    `usage`.ProductLineId= " + lineid + " ) t";

            DataTable quadt7 = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            Dictionary<string, double> quadicdt7 = new Dictionary<string, double>();

            if (quadt7.Rows.Count > 0)
            {
                all_num7 = Convert.ToDouble(quadt7.Rows[0]["all_num7"].ToString());
                pass_rate7 = Convert.ToDouble(quadt7.Rows[0]["pass_rate7"].ToString());
                inferior_rate7 = Convert.ToDouble(quadt7.Rows[0]["inferior_rate7"].ToString());
                waste_rate7 = Convert.ToDouble(quadt7.Rows[0]["waste_rate7"].ToString());

            }
            quadic.Add("all_num7", all_num7);
            quadic.Add("pass_rate7", Math.Round(pass_rate7,2));
            quadic.Add("inferior_rate7", Math.Round(inferior_rate7,2));
            quadic.Add("waste_rate7", Math.Round(waste_rate7,2));

            dv.Add("quality", quadic);
            //设备效率指标
            //产线->多少设备->工位
            //当日设备稼动率

            sql = " select tt2.locationid,  tt2.devicemodel,tt2.stationname,if (tt1.run_t > 0,tt1.run_t,0) as run_t  ,if (tt1.free_t > 0,tt1.free_t,0) as free_t  ,if (tt1.warn_t > 0,tt1.warn_t,0) as warn_t   from  " +
            " (select   sum( if (stationstate = '运行', endtime - startTime, 0))/ 1000 AS run_t, sum( if " +
           "  (stationstate = '空闲', endtime - startTime, 0))/ 1000 AS free_t, " +
            "  sum( if (stationstate = '报警', endtime - startTime, 0))/ 1000 AS warn_t, stationNAME from huabao.LocationState where ID_USAGE = " + idusage + " and ProductLineId = " + lineid +
          " group by stationNAME) tt1 right join( " +
             "  select a.devicemodel, b.stationname ,b.locationid from deviceinfo a left join locationcfg b on a.locationid = b.locationid and a.ProductLineId = b.ProductLineId where a.ProductLineId = " + lineid +
             "  ) tt2 on tt1.stationname = tt2.stationname order by tt2.locationid ";

            DataTable devdt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            List<Dictionary<string, object>> devlist = new List<Dictionary<string, object>>();
            if (devdt.Rows.Count > 0)
            {
                for (int i = 0; i < devdt.Rows.Count; i++)
                {
                    Dictionary<string, object> devdic = new Dictionary<string, object>();
                    //获取
                    string stationname = devdt.Rows[i]["stationname"].ToString();
                    string devicemodel = devdt.Rows[i]["devicemodel"].ToString();
                    double dev_run_t = Convert.ToDouble(devdt.Rows[i]["run_t"].ToString());
                    double dev_free_t = Convert.ToDouble(devdt.Rows[i]["free_t"].ToString());
                    double dev_warn_t = Convert.ToDouble(devdt.Rows[i]["warn_t"].ToString());
                    devdic.Add("stationname", stationname);
                    devdic.Add("devicemodel", devicemodel);
                    devdic.Add("dev_activation",Math.Round(100* (dev_run_t + dev_free_t) / (plan_t*3600),2));
                    devdic.Add("dev_plan_activation", Math.Round(100 * (dev_run_t + dev_free_t + dev_warn_t) / (plan_t * 3600), 2));
                    devdic.Add("dev_time_activation", Math.Round(100 * (dev_run_t + dev_free_t) / (dev_run_t + dev_free_t + dev_warn_t > 0 ? (dev_run_t + dev_free_t + dev_warn_t) : 1),2));
                    devlist.Add(devdic);
                }
            }
            dv.Add("dev_activation",  devlist);

            //7日平均设备稼动率
            sql = "select distinct ProductionT as ProductionT from rptdeviceday where ProductLineId = 1 order by  ProductionT desc  limit 1 offset 7";
            DataTable begindatedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            string begindate = "0";
            if (begindatedt.Rows.Count > 0)
            {
                begindate = begindatedt.Rows[0]["ProductionT"].ToString();
            }

            //获取七日平均稼动率
            sql = "  select sum(RunT) as RunT, sum(WarnT) as WarnT,sum(PlanPowerOnT) as PlanPowerOnT " +
                "  from rptproductday where ProductLineId = 1 order by ProductionT desc limit 7 ";

            double RunT = 0.0;
            double WarnT = 0.0;
            double PlanPowerOnT = 0.0;
            DataTable Lineavgdt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            if (Lineavgdt.Rows.Count > 0)
            {
                RunT = Convert.ToDouble(Lineavgdt.Rows[0]["RunT"].ToString());
                WarnT = Convert.ToDouble(Lineavgdt.Rows[0]["WarnT"].ToString());
                PlanPowerOnT = Convert.ToDouble(Lineavgdt.Rows[0]["PlanPowerOnT"].ToString());

            }
            if (run_t == 0 && warn_t == 0) warn_t = 1;
            //时间开动率
            double time_activation_7 = Math.Round(100 * (RunT) / (RunT + WarnT), 2);
            //设备利用率
            double plan_activation_7 = Math.Round(100 * (RunT + WarnT) / PlanPowerOnT, 2);
            //产线稼动率
            double activation_7 = Math.Round(100 * RunT / PlanPowerOnT, 2);

            Dictionary<string, object> dicline = new Dictionary<string, object>();
            dicline.Add("stationname", "生产线");
            dicline.Add("devicemodel", "");
            dicline.Add("dev_activation7", activation_7.ToString());
            dicline.Add("dev_plan_activation7", plan_activation_7.ToString());
            dicline.Add("dev_time_activation7", time_activation_7.ToString());
            



            sql = "  select tt1.locationid,tt1.stationname,tt1.devicemodel ,tt2.* from " +
       " (select a.deviceid, a.devicemodel, b.stationname, b.locationid from deviceinfo a left join locationcfg b on a.locationid = b.locationid and a.ProductLineId = b.ProductLineId where a.ProductLineId =" + lineid +
        " )tt1  right join (select deviceid, SUM(RunT) as RunT, SUM(FreeT) as FreeT, SUM(WarnT) as WarnT, SUM(PlanPowerOnT) as PlanPowerOnT  " +
         " from rptdeviceday where ProductLineId = " + lineid + " and ProductionT >= '" + begindate + "' group by deviceid  ) tt2 on tt1.deviceid = tt2.deviceid  ";
            DataTable devdt7 = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            List<Dictionary<string, object>> devlist7 = new List<Dictionary<string, object>>();

            devlist7.Add(dicline);

            if (devdt7.Rows.Count > 0)
            {
                for (int i = 0; i < devdt7.Rows.Count; i++)
                {
                    Dictionary<string, object> devdic7 = new Dictionary<string, object>();
                    //获取
                    string stationname = devdt7.Rows[i]["stationname"].ToString();
                    string devicemodel = devdt7.Rows[i]["devicemodel"].ToString();
                    double dev_run_t = Convert.ToDouble(devdt7.Rows[i]["RunT"].ToString());
                    double dev_free_t = Convert.ToDouble(devdt7.Rows[i]["FreeT"].ToString());
                    double dev_warn_t = Convert.ToDouble(devdt7.Rows[i]["WarnT"].ToString());
                    double dev_PlanPowerOnT = Convert.ToDouble(devdt7.Rows[i]["PlanPowerOnT"].ToString());
                    devdic7.Add("stationname", stationname);
                    devdic7.Add("devicemodel", devicemodel);
                    devdic7.Add("dev_activation7", Math.Round( (dev_run_t + dev_free_t + +dev_warn_t) *100 / dev_PlanPowerOnT, 2));
                    devdic7.Add("dev_plan_activation7", Math.Round(100*(dev_run_t + dev_free_t) / dev_PlanPowerOnT, 2));
                    devdic7.Add("dev_time_activation7", Math.Round(100*(dev_run_t + dev_free_t) / (dev_run_t + dev_free_t + dev_warn_t > 0 ? (dev_run_t + dev_free_t + dev_warn_t) : 1),2));
                    devlist7.Add(devdic7);
                }
            }
            dv.Add("dev_activation7", devlist7);
            ////时间开动率
            //double dev_time_activation = (RunT + FreeT) / (RunT + FreeT+WarnT);
            ////设备利用率
            //double dev_plan_activation = (RunT + FreeT + WarnT)  / PlanPowerOnT;
            ////设备稼动率
            //double dev_activation = (RunT+FreeT) / PlanPowerOnT;
            //设备稼动率  run_t+free_t+warn_t /plan_t
            //设备利用率  run_t+free_t+warn_t /run_t+free_t+warn_t+启动时间
            //时间稼动率   run_t+free_t /run_t+free_t+warn_t
            //产线机器列表
            //当日产线稼动时间      负荷时间            产线稼动率            
            // 七日平均稼动时间  七日平均负荷时间    七日平均产线稼动率

            //当日设备报警情况
            //根据stationname 获取月平均故障统计值
            List<Dictionary<string, object>> devwarnlist = new List<Dictionary<string, object>>();

            double hist_warn_c = 0; double hist_firstwarninter = 0; double hist_avgwarnt = 0;
            double hist_warninter = 0;
            sql = "select c.locationseq, c.stationname, sum(WarnC)/count(*) as WarnC ,sum(WarnT)/count(*) as WarnT,sum(AvgWarnT)/count(*) as AvgWarnT,  " +
     " sum(AvgWarnInter) / count(*) as AvgWarnInter,sum(FirstWarnInter) / count(*) as FirstWarnInter " +
      " from rptdeviceerrday a  left  join deviceinfo b  on a.DeviceId = b.DeviceId left " +
   " join locationcfg c  on b.locationid = c.locationid and b.ProductLineId = c.ProductLineId  " +
   " where a.ProductLineId = " + lineid + "  and ProductionT >= '" + lstmonstr + "' group by  c.stationname ,c.locationseq order by c.locationseq ";
            DataTable devwarnhisdt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
            if (devwarnhisdt.Rows.Count > 0)
            {
                for (int i = 0; i < devwarnhisdt.Rows.Count; i++)
                {
                    Dictionary<string, object> devwarndic = new Dictionary<string, object>();

                    string stationname = devwarnhisdt.Rows[i]["stationname"].ToString();

                 hist_warn_c = Convert.ToDouble(devwarnhisdt.Rows[i]["WarnC"].ToString());
                hist_firstwarninter = Convert.ToDouble(devwarnhisdt.Rows[i]["FirstWarnInter"].ToString());
                hist_avgwarnt = Convert.ToDouble(devwarnhisdt.Rows[i]["AvgWarnT"].ToString());
                hist_warninter = Convert.ToDouble(devwarnhisdt.Rows[i]["AvgWarnInter"].ToString());
             
               
             devwarndic.Add("stationname", stationname);

           devwarndic.Add("h_avg_warn_c", Math.Round(hist_warn_c, 2));
            devwarndic.Add("h_firstwarninter", Math.Round(hist_firstwarninter, 2));
            devwarndic.Add("h_avgwarnt", Math.Round(hist_avgwarnt, 2));
            devwarndic.Add("h_avgwarninter", Math.Round(hist_avgwarnt, 2));
            sql = "   select tt1.* ,round((tt2.beginwarn-tt3.beginwork)/1000) as firstwarninter, round((tt4.endwork-tt5.endwarn)/1000) as lstwarninter,round((tt5.endwarn-tt2.beginwarn)/1000) as warninter " +
         "  from(  select  stationname,sum( if (stationstate = '报警', endtime - startTime, 0))/1000 AS warn_t,  sum( if (stationstate = '报警',1, 0))  AS warn_c    from huabao.LocationState where " +
     "  ID_USAGE = " + idusage + "    and stationname='" + stationname + "')tt1 left join " +
      "    (select stationname, min( if (StartTime is null,0,StartTime)) as beginwarn " +
      "    from locationstate where id_usage = " + idusage + "    and stationstate = '报警' and stationname='"+ stationname+"'  )tt2 on tt1.stationname = tt2.stationname " +
      "    left join(  select stationname, min( if (StartTime is null,0,StartTime)) as beginwork " +
      "    from locationstate where id_usage = " + idusage + "  and stationname='" + stationname + "' )tt3 on tt1.stationname = tt3.stationname " +
     "      left join(  select stationname, max( if (endtime is null,0,endtime)) as endwork " +
      "    from locationstate where id_usage = " + idusage + " and stationname='" + stationname + "' )tt4 on tt1.stationname = tt4.stationname " +
    "        left join(  select stationname, max( if (endtime is null,0,endtime)) as endwarn " +
      "    from locationstate where id_usage = " + idusage + "  and stationstate = '报警'   and stationname='" + stationname + "' )tt5 on tt1.stationname = tt5.stationname where tt1.warn_c>0 ";
                    int warn_c = 0;
                    int firstwarninter = 0;
                    double avgwarntm = 0.0;
                    double avgwarninter = 0.0;
                    DataTable devwarndt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
                   if (devwarndt.Rows.Count > 0)
                    {

                          warn_c = Convert.ToInt32(devwarndt.Rows[0]["warn_c"].ToString());
                          firstwarninter = Convert.ToInt32(devwarndt.Rows[0]["firstwarninter"].ToString());
                        int lstwarninter = Convert.ToInt32(devwarndt.Rows[0]["lstwarninter"].ToString());
                        int warninter = Convert.ToInt32(devwarndt.Rows[0]["warninter"].ToString());

                        double warnt = Convert.ToDouble(devwarndt.Rows[0]["warn_t"].ToString());
                          avgwarntm = warnt / warn_c > 0 ? warn_c : 1;
                          avgwarninter = (warninter - warnt) / warn_c > 1 ? warn_c - 1 : 1;
                       

                    }
                    devwarndic.Add("warn_c", warn_c);
                    devwarndic.Add("firstwarninter", firstwarninter);
                    devwarndic.Add("avgwarnt", Math.Round(avgwarntm, 2));
                    devwarndic.Add("avgwarninter", Math.Round(avgwarninter, 2));

                    devwarnlist.Add(devwarndic);
                }
            }
            dv.Add("dev_warn_stat",  devwarnlist );
            string res= JsonConvert.SerializeObject(dv);
            return res;

        }

    }
}
