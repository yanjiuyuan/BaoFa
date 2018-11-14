using Bussiness.Time;
using Common.Code;
using Common.DbHelper;
using Common.JsonHelper;
using Common.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bussiness.AutoTask
{

    public class SubTaskServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(SubTaskServer));

//        private static string errorcollect = "select stationNAME, errortype, sum(warn_t) as warn_t, sum(warn_c) as warn_c from(select stationNAME, sum( if (errorn & 1 = 1, warn_t ,0)) as warn_t,'EN0' as errortype,  " +
//" sum( if (errorn & 1 = 1, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +

//" union all select stationNAME, sum( if (errorn & 2 = 2, warn_t, 0)) as warn_t,  " +
//" 'EN1' as errortype,sum( if (errorn & 2 = 2, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +

//" union all select stationNAME, sum( if (errorn & 4 = 4, warn_t, 0)) as warn_t,'EN2' as errortype,  " +
//" sum( if (errorn & 4 = 4, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
//" union all select stationNAME, sum( if (errorn & 8 = 8, warn_t, 0)) as warn_t,'EN3' as errortype,  " +
//" sum( if (errorn & 8 = 8, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
//" union all select stationNAME, sum( if (errorn & 16 = 16, warn_t, 0)) as warn_t,'EN4' as errortype,  " +
//" sum( if (errorn & 16 = 16, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
//" union all select stationNAME, sum( if (errorn & 32 = 32, warn_t, 0)) as warn_t,'EN5' as errortype,  " +
//" sum( if (errorn & 32 = 32, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +

//" union all select stationNAME, sum( if (errorn & 64 = 64, warn_t, 0)) as warn_t,'EN6' as errortype,  " +
//" sum( if (errorn & 64 = 64, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
//" union all  select stationNAME, sum( if (errorn & 128 = 128, warn_t, 0)) as warn_t,'EN7' as errortype,  " +
//" sum( if (errorn & 128 = 128, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
//" union all select stationNAME, sum( if (errorn & 256 = 256, warn_t, 0)) as warn_t,'EN8' as errortype,  " +
//" sum( if (errorn & 256 = 256, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
//" union all select stationNAME, sum( if (errorn & 512 = 512, warn_t, 0)) as warn_t,'EN9' as errortype,  " +
//" sum( if (errorn & 512 = 512, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +

//" union all select stationNAME, sum( if (errorn & 1024 = 1024, warn_t, 0)) as warn_t,'EN10' as errortype,  " +
//" sum( if (errorn & 1024 = 1024, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
//" union all select stationNAME, sum( if (errorn & 2048 = 2048, warn_t, 0)) as warn_t,'EN11' as errortype,  " +
//" sum( if (errorn & 2048 = 2048, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
//" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  )t1 group by stationNAME, errortype";
////" union all select stationNAME, sum( if (errorn & 4096 = 4096, warn_t, 0)) as warn_t,'EN12' as errortype,  " +
////" sum( if (errorn & 4096 = 4096, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
////" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +

////" union all select stationNAME, sum( if (errorn & 8192 = 8192, warn_t, 0)) as warn_t,'EN13' as errortype,  " +
////" sum( if (errorn & 8192 = 8192, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
////" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
////" union all select stationNAME, sum( if (errorn & 16384 = 16384, warn_t, 0)) as warn_t,'EN14' as errortype,  " +
////" sum( if (errorn & 16384 = 16384, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
////" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME  " +
////" union all select stationNAME, sum( if (errorn & 32768 = 32768, warn_t, 0)) as warn_t,'EN15' as errortype,  " +
////" sum( if (errorn & 32768 = 32768, 1 ,0)) as warn_c from(select(endtime - startTime) / 1000 AS warn_t, if (ERRORN is null ,0,ERRORN) as  " +
////" ERRORN, stationNAME from huabao.LocationState where id_usage in ('{0}') and StationState = '报警' ) t group by stationNAME)t1 group by stationNAME, errortype";

        //获取各工位的末次生产时间
        public void taskInit(string datestr)
        {
            try
            {

                string strSql = "update runctrl set staticsdate=  '" + datestr + "', " +
                    " stat=1 ,begintime=  date_format(  now() ,'%Y-%m-%d %H:%m:%s') where TaskType='AutoTask' and TaskID='0000' and stat!=1";

                int updateC = MySqlHelper.ExecuteSql(strSql);
                if (updateC != 1)
                {
                    logger.Info("登记初始化任务开始失败：更新条数：" + updateC);
                    return;
                }

                updateC = 0;
                strSql = "update runctrl set staticsdate= '" + datestr + "' , " +
                  " stat=0    where TaskType='AutoTask' and TaskID !='0000'  ";
                updateC = MySqlHelper.ExecuteSql(strSql);
                if (updateC < 1)
                {
                    logger.Info("初始化执行任务失败：更新条数：" + updateC);
                    return;
                }

                strSql = "update runctrl  set " +
                 " stat=2 ,endtime=  date_format(  now() ,'%Y-%m-%d %H:%m:%s') where TaskType='AutoTask' and TaskID='0000' and stat=1";

                updateC = MySqlHelper.ExecuteSql(strSql);
                if (updateC != 1)
                {
                    logger.Info("登记初始化任务完成失败：更新条数：" + updateC);
                    return;
                }
                MySqlHelper.ExecuteSql("update queuestat set num=0 ,CT=now() where DataType=0 ");


            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        //



        public void RunTask(string taskid,  string msg,string datestr)
        {
            try
            {
                ////    1.Load(命名空间名称)，GetType(命名空间.类名)  
               

                ////    2.GetMethod(需要调用的方法名称)  
                MethodInfo method = this.GetType().GetMethod(msg);

                ////    3.调用的实例化方法（非静态方法）需要创建类型的一个实例  
                object obj = Activator.CreateInstance(this.GetType());

                ////    4.方法需要传入的参数  
                object[] parameters = new object[] {  datestr };

                ////    5.调用方法，如果调用的是一个静态方法，就不需要第3步（创建类型的实例）  
                ////      相应地调用静态方法时，Invoke的第一个参数为null  
                method.Invoke(obj, parameters);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                TaskFail(taskid);
            }







            //switch (taskid)
            //{
            //    case "1000":
            //        DbPartCreate();
            //        break;
            //    case "1001":
            //        DataTransfer(datestr);
            //        break;
            //    case "2002":
            //        SprayDataClean(datestr);
            //        break;
            //    case "2003":
            //        LineDataClean(datestr);
            //        break;
            //    case "3004":
            //        LineDataRpt(datestr);
            //        break;
            //    case "3005":
            //        DbPartCreate5();
            //        break;
            //    case "3006":
            //        DbPartCreate6();
            //        break;
            //    case "4007":
            //        DbPartCreate7();
            //        break;
            //    case "5008":
            //        DbPartCreate8();
            //        break;
            //}

        }

        public void TaskFail(string taskid)
        {
            int updateC = 0;
            string strSql = "update runctrl set  " +
                " stat=3    where TaskType='AutoTask' and TaskID ='" + taskid + "'";
            updateC = MySqlHelper.ExecuteSql(strSql);
            if (updateC < 1)
            {
                logger.Info("执行任务" + taskid + "失败：更新条数：" + updateC);
                return;
            }
            logger.Info(" 执行任务失败：" + taskid);



        }






        /// <summary>
        /// 表分区创建
        /// </summary>
        public void DbPartCreate(string datestr)
        {

            try {
                string strsql = "select max(id_usage) from `usage`";

                object obj = MySqlHelper.GetSingle(strsql);
                int current_id_usage = Convert.ToInt32(obj == null ? "0" : obj);

                strsql = "select max(CONVERT(PARTITION_DESCRIPTION, SIGNED))  from `INFORMATION_SCHEMA`.`PARTITIONS` where `TABLE_NAME`= 'sprayrecd' " +
                 " and PARTITION_DESCRIPTION != 'maxvalue'";
                obj = MySqlHelper.GetSingle(strsql);
                int current_max_usage = Convert.ToInt32(obj == null ? "0" : obj);

                int max_id_usage = 10 * (current_id_usage / 10) + 100;
                List<int> idlist = new List<int>();
                logger.Info("current_id_usage" + current_id_usage + ",current_max_usage" + current_max_usage + ",max_id_usage" + max_id_usage);


                while (true)
                {
                    current_max_usage = current_max_usage + 5;
                    if (current_max_usage <= max_id_usage)
                        idlist.Add(current_max_usage);
                    else
                        break;
                }
                if (idlist.Count == 0)
                {
                    logger.Info("无需创建分区");
                    return;
                }

                var file = File.Open(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\dbpart.config", FileMode.Open);
                string s = string.Empty;
                using (var stream = new StreamReader(file, System.Text.Encoding.GetEncoding("gb2312")))
                {
                    while (!stream.EndOfStream)
                    {
                        s = stream.ReadToEnd();
                    }
                }
                file.Close();
                string[] sqls = s.Split(';');
                if (sqls.Length == 0)
                {
                    logger.Info("读取文件内容为空");
                    return;
                }
                for (int i = 0; i < sqls.Length; i++)
                {

                    string sql = sqls[i].Replace('\r', ' ').Replace('\n', ' ');
                    if (sql.Length < 10)
                        continue;
                    for (int j = 0; j < idlist.Count; j++)
                    {

                        string execsql = String.Format(sql, idlist[j]);
                        int execrst = MySqlHelper.ExecuteSql(execsql);

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

                TaskFail("1000");


            }
        }



        /// <summary>
        /// 数据迁移
        /// </summary>
        public void DataTransfer(string datestr)
        {
            logger.Info("当日数据迁移");
        }
        /// <summary>
        /// 胶站数据清洗
        /// </summary>
        public void SprayDataClean(string datestr)
        {
            logger.Info("胶站数据清洗");
        }


        public DataTable ProductDataDefine()
        {
            DataTable newdt = new DataTable();
            // ProductionT, id_usage, TM, ChildID, OrderID, ChildN, ProductionBeat, 
            // NowN, NowS, AllN, WeiTiaoConsumption, HuChiConsumption,
            //BiaoQianConsumption, DaDiConsumption, 
            // WTConsumeS, HCConsumeS, BQConsumeS, DDConsumeS, ProductLineId
            DataColumn d1 = new DataColumn("ProductionT", Type.GetType("System.String"));
            DataColumn d2 = new DataColumn("id_usage", Type.GetType("System.Int32"));
            DataColumn d3 = new DataColumn("TM", Type.GetType("System.String"));
            DataColumn d4 = new DataColumn("ChildID", Type.GetType("System.String"));
            DataColumn d5 = new DataColumn("OrderID", Type.GetType("System.String"));
            DataColumn d6 = new DataColumn("ChildN", Type.GetType("System.Int32"));
            DataColumn d7 = new DataColumn("ProductionBeat", Type.GetType("System.Int32"));
            DataColumn d8 = new DataColumn("NowN", Type.GetType("System.Int32"));
            DataColumn d9 = new DataColumn("NowS", Type.GetType("System.Int32"));
            DataColumn d10 = new DataColumn("AllN", Type.GetType("System.Int32"));
            DataColumn d11 = new DataColumn("WeiTiaoConsumption", Type.GetType("System.Int32"));
            DataColumn d12 = new DataColumn("HuChiConsumption", Type.GetType("System.Int32"));
            DataColumn d13 = new DataColumn("BiaoQianConsumption", Type.GetType("System.Int32"));
            DataColumn d14 = new DataColumn("DaDiConsumption", Type.GetType("System.Int32"));
            DataColumn d15 = new DataColumn("WTConsumeS", Type.GetType("System.Int32"));
            DataColumn d16 = new DataColumn("HCConsumeS", Type.GetType("System.Int32"));
            DataColumn d17 = new DataColumn("BQConsumeS", Type.GetType("System.Int32"));
            DataColumn d18 = new DataColumn("DDConsumeS", Type.GetType("System.Int32"));
            DataColumn d19 = new DataColumn("ProductLineId", Type.GetType("System.Int32"));

            newdt.Columns.Add(d1);
            newdt.Columns.Add(d2);
            newdt.Columns.Add(d3);
            newdt.Columns.Add(d4);
            newdt.Columns.Add(d5);
            newdt.Columns.Add(d6);
            newdt.Columns.Add(d7);
            newdt.Columns.Add(d8);
            newdt.Columns.Add(d9);
            newdt.Columns.Add(d10);
            newdt.Columns.Add(d11);
            newdt.Columns.Add(d12);
            newdt.Columns.Add(d13);
            newdt.Columns.Add(d14);
            newdt.Columns.Add(d15);
            newdt.Columns.Add(d16);
            newdt.Columns.Add(d17);
            newdt.Columns.Add(d18);
            newdt.Columns.Add(d19);
            return newdt;

        }
        public object[] DataRowColCopy(DataColumnCollection cols, DataRow dr)
        {
            DataTable newdt = ProductDataDefine();
            DataRow dr1 = newdt.NewRow();
            int colCount = cols.Count;
            foreach (DataColumn col in newdt.Columns)
            {

                for (int i = 0; i < colCount; i++)
                    if ((col.ColumnName == cols[i].ColumnName))
                        dr1[col] = dr[i];
            }
            dr1["TM"] = TimeHelper.GetStringToDateTime((dr[0].ToString()));

            return dr1.ItemArray;
        }




        /// <summary>
        /// 产线数据清洗
        /// </summary>
        /// <param name="datestr"></param>
        public void LineDataClean(string datestr)
        {
            //查看前日所有班次
            try
            {
                //删除旧数据

                string delsql = "delete from  productdata where productionT ='" + datestr + "'";
                int delC=MySqlHelper.ExecuteSql(delsql);
                if (delC > 0)
                    logger.Info("productdata删除现有旧数据，数量：" + delC);



                DataTable dts = new DataTable();
                string strsql = "select id_usage,orderid,childid,childn,productionbeat,productlineid  from `usage` where createtime like '" + datestr + "%'";
                dts = MySqlHelper.ExecuteQuery(strsql);
                if (dts.Rows.Count == 0)
                {
                    logger.Info("当日无生产班次！");
                    return;
                }
                int current_usage = 0;
                string orderid = string.Empty;
                string childid = string.Empty;

                int childn = 0;
                int productionbeat = 0;
                int productlineid = 0;

                int scale = 6;
                int dura_min = 10;

                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    current_usage = Convert.ToInt32(dts.Rows[i]["id_usage"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["id_usage"]);
                    childn = Convert.ToInt32(dts.Rows[i]["childn"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["childn"]);
                    productionbeat = Convert.ToInt32(dts.Rows[i]["productionbeat"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["productionbeat"]);
                    productlineid = Convert.ToInt32(dts.Rows[i]["productlineid"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["productlineid"]);
                    orderid = Convert.ToString(dts.Rows[i]["orderid"]);
                    childid = Convert.ToString(dts.Rows[i]["childid"]);
                    strsql = "select * from realtimeusage  where orderid='" + orderid + "' and childid='" + childid + "' order by id_realtimeusage";
                    DataTable dt = MySqlHelper.ExecuteQuery(strsql);
                    if (dt.Rows.Count == 0) continue;
                    DataTable newdt = ProductDataDefine();
                    long listid = 0;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (j == 0)
                        {
                            newdt.Rows.Add(DataRowColCopy(dt.Columns, dt.Rows[j]));
                            listid = Convert.ToInt64(dt.Rows[j][0]);
                            int nowS = Convert.ToInt32(dt.Rows[0]["NowN"]);
                            newdt.Rows[0]["nowS"] = nowS < 0 ? 0 : nowS;
                            int WTConsumeS = Convert.ToInt32(dt.Rows[0]["WeiTiaoConsumption"]);
                            newdt.Rows[0]["WTConsumeS"] = WTConsumeS < 0 ? 0 : WTConsumeS;
                            int HCConsumeS = Convert.ToInt32(dt.Rows[0]["HuChiConsumption"]);
                            newdt.Rows[0]["HCConsumeS"] = HCConsumeS < 0 ? 0 : HCConsumeS;
                            int BQConsumeS = Convert.ToInt32(dt.Rows[0]["BiaoQianConsumption"]);
                            newdt.Rows[0]["BQConsumeS"] = BQConsumeS < 0 ? 0 : BQConsumeS;
                            int DDConsumeS = Convert.ToInt32(dt.Rows[0]["DaDiConsumption"]);
                            newdt.Rows[0]["DDConsumeS"] = DDConsumeS < 0 ? 0 : DDConsumeS;
                        }
                        else if (j == newdt.Rows.Count - 1 && newdt.Rows.Count > 2)
                        {
                            newdt.Rows.Add(DataRowColCopy(dt.Columns, dt.Rows[j]));
                            listid = Convert.ToInt64(dt.Rows[j][0]);
                            int count = newdt.Rows.Count - 1;
                            int nowS = scale * (Convert.ToInt32(newdt.Rows[count]["NowN"]) - Convert.ToInt32(newdt.Rows[count - 1]["NowN"]));
                            newdt.Rows[count]["nowS"] = nowS < 0 ? 0 : nowS;
                            int WTConsumeS = scale * (Convert.ToInt32(newdt.Rows[count]["WeiTiaoConsumption"]) - Convert.ToInt32(newdt.Rows[count - 1]["WeiTiaoConsumption"]));
                            newdt.Rows[count]["WTConsumeS"] = WTConsumeS < 0 ? 0 : WTConsumeS;
                            int HCConsumeS = scale * (Convert.ToInt32(newdt.Rows[count]["HuChiConsumption"]) - Convert.ToInt32(newdt.Rows[count - 1]["HuChiConsumption"]));
                            newdt.Rows[count]["HCConsumeS"] = HCConsumeS < 0 ? 0 : HCConsumeS;
                            int BQConsumeS = scale * (Convert.ToInt32(newdt.Rows[count]["BiaoQianConsumption"]) - Convert.ToInt32(newdt.Rows[count - 1]["BiaoQianConsumption"]));
                            newdt.Rows[count]["BQConsumeS"] = BQConsumeS < 0 ? 0 : BQConsumeS;
                            int DDConsumeS = scale * (Convert.ToInt32(newdt.Rows[count]["DaDiConsumption"]) - Convert.ToInt32(newdt.Rows[count - 1]["DaDiConsumption"]));
                            newdt.Rows[count]["DDConsumeS"] = DDConsumeS < 0 ? 0 : DDConsumeS;

                            continue;
                        }

                        else if (Convert.ToInt64(dt.Rows[j][0]) >= Convert.ToInt64(listid + (long)dura_min * 60 * 1000))
                        {

                            newdt.Rows.Add(DataRowColCopy(dt.Columns, dt.Rows[j]));
                            listid = Convert.ToInt64(dt.Rows[j][0]);
                            int count = newdt.Rows.Count - 1;
                            int nowS = scale * (Convert.ToInt32(newdt.Rows[count]["NowN"]) - Convert.ToInt32(newdt.Rows[count - 1]["NowN"]));
                            newdt.Rows[count]["nowS"] = nowS < 0 ? 0 : nowS;
                            int WTConsumeS = scale * (Convert.ToInt32(newdt.Rows[count]["WeiTiaoConsumption"]) - Convert.ToInt32(newdt.Rows[count - 1]["WeiTiaoConsumption"]));
                            newdt.Rows[count]["WTConsumeS"] = WTConsumeS < 0 ? 0 : WTConsumeS;
                            int HCConsumeS = scale * (Convert.ToInt32(newdt.Rows[count]["HuChiConsumption"]) - Convert.ToInt32(newdt.Rows[count - 1]["HuChiConsumption"]));
                            newdt.Rows[count]["HCConsumeS"] = HCConsumeS < 0 ? 0 : HCConsumeS;
                            int BQConsumeS = scale * (Convert.ToInt32(newdt.Rows[count]["BiaoQianConsumption"]) - Convert.ToInt32(newdt.Rows[count - 1]["BiaoQianConsumption"]));
                            newdt.Rows[count]["BQConsumeS"] = BQConsumeS < 0 ? 0 : BQConsumeS;
                            int DDConsumeS = scale * (Convert.ToInt32(newdt.Rows[count]["DaDiConsumption"]) - Convert.ToInt32(newdt.Rows[count - 1]["DaDiConsumption"]));
                            newdt.Rows[count]["DDConsumeS"] = DDConsumeS < 0 ? 0 : DDConsumeS;

                            continue;

                        }
                    }


                    for (int z = 0; z < newdt.Rows.Count; z++)
                    {
                        //转换时间格式
                        newdt.Rows[z]["ProductionT"] = datestr;
                        newdt.Rows[z]["id_usage"] = current_usage;
                        newdt.Rows[z]["ChildID"] = childid;
                        newdt.Rows[z]["OrderID"] = orderid;
                        newdt.Rows[z]["ChildN"] = childn;
                        newdt.Rows[z]["ProductionBeat"] = productionbeat;
                        newdt.Rows[z]["ProductLineId"] = productlineid;

                    }
                    MySqlHelper.BatchInsert(newdt, "productdata");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                TaskFail("2003");
            }



        }
        public void LineDataRptDay(string datestr)
        {
            //查看前日所有班次

            try
            {
                string delsql = "delete from  RptProductDay where productionT ='" + datestr + "'";
                int delC = MySqlHelper.ExecuteSql(delsql);
                if (delC > 0)
                    logger.Info("RptProductDay删除现有旧数据，数量：" + delC);



                DataTable dts = new DataTable();
                string strsql = "select group_concat(id_usage) as id_usage,sum(nown) as nowan, sum(childn) as childn,productlineid  from `usage` where  createtime like '" + datestr + "%'   group by productlineid   order by productlineid ";
                dts = MySqlHelper.ExecuteQuery(strsql);
                if (dts.Rows.Count == 0)
                {
                    logger.Info("当日无生产班次！");
                    return;
                }


                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    string id_usages = string.Empty;
                    int PlanWorkLoad = 0;
                    int productlineid = 0;
                    int isplanpoweron = 0;
                    int PlanPowerOnT = 0;
                    int PlanShifts = 1;
                    int WorkLoad = 0;
                    int Shifts = 1;
                    double RunT = 0, StopT = 0, WarnT = 0, OffLineT = 0;
                    int RunC = 0, StopC = 0, WarnC = 0, OffLineC = 0;
                    id_usages = Convert.ToString(dts.Rows[i]["id_usage"]);
                    PlanWorkLoad = Convert.ToInt32(dts.Rows[i]["childn"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["childn"]);
                    productlineid = Convert.ToInt32(dts.Rows[i]["productlineid"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["productlineid"]);
                    WorkLoad = Convert.ToInt32(dts.Rows[i]["nowan"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["nowan"]);

                    //查询当日是否计划开机及计划开机时间

                    strsql = "select `ProductionDays` ,ProductionShifts,ProductionTimes from productlineinfo where productlineid=" + productlineid;
                    dts = MySqlHelper.ExecuteQuery(strsql);
                    if (dts.Rows.Count == 0)
                    {
                        logger.Info("查询产线参数设置为空！");
                        return;
                    }
                    string ProductionDays = Convert.ToString(dts.Rows[i]["ProductionDays"]);
                    PlanPowerOnT = Convert.ToInt32(dts.Rows[i]["ProductionTimes"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["ProductionTimes"]);
                    PlanShifts = Convert.ToInt32(dts.Rows[i]["ProductionShifts"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["ProductionShifts"]);
                    isplanpoweron = Global.IsWeekInList(datestr, ProductionDays) == true ? 1 : 0;
                    Shifts = id_usages.Split(',').Length;

                    //查询各事件次数及事件时间
                    strsql = " select    if (run_c is null ,0,run_c)as run_c,  if (run_t is null ,0,run_t)as run_t, if (stop_t is null ,0,stop_t)as stop_t  ,if (stop_c is null ,0,stop_c)as stop_c " +
               "  ,if (warn_t is null ,0,warn_t)as warn_t, if (warn_c is null ,0,warn_c)as warn_c, if (offline_t is null ,0,offline_t)as offline_t, if (offline_c is null ,0,offline_c)as offline_c " +
               "    from(select   sum( if (stationstate = '运行', endtime - startTime, 0))/ 1000 AS run_t, sum( if (stationstate = '运行',1, 0))  AS run_c, sum( if " +
               "(stationstate = '停止', endtime - startTime, 0))/ 1000 AS stop_t, sum( if (stationstate = '停止',1, 0))  AS stop_c, " +
               "sum( if (stationstate = '报警', endtime - startTime, 0))/ 1000 AS warn_t, sum( if (stationstate = '报警',1, 0))  " +
               "AS warn_c, sum( if (stationstate = '离线', endtime - startTime, 0))/ 1000 AS offline_t, sum( if (stationstate = '离线',1, 0)) " +
               "AS offline_c, stationNAME from huabao.LocationState where id_usage in (" + id_usages + ") and stationNAME = '生产线') t ";
                    dts = MySqlHelper.ExecuteQuery(strsql);
                    if (dts.Rows.Count == 0)
                    {
                        logger.Info("查询产线事件为空！");
                        return;
                    }
                    RunT = Math.Round(Convert.ToDouble(dts.Rows[i]["run_t"]) / 3600, 2);
                    RunC = Convert.ToInt32(dts.Rows[i]["run_c"]);
                    StopT = Math.Round(Convert.ToDouble(dts.Rows[i]["stop_t"]) / 3600, 2);
                    StopC = Convert.ToInt32(dts.Rows[i]["stop_c"]);
                    WarnT = Math.Round(Convert.ToDouble(dts.Rows[i]["warn_t"]) / 3600, 2);
                    WarnC = Convert.ToInt32(dts.Rows[i]["warn_c"]);
                    OffLineT = Math.Round(Convert.ToDouble(dts.Rows[i]["offline_t"]) / 3600, 2);
                    OffLineC = Convert.ToInt32(dts.Rows[i]["offline_c"]);


                    //数据插入
                    strsql = string.Format("insert into RptProductDay set " +
                        " ProductLineId={0},ProductionT='{1}',RunT={2},RunC={3},StopT={4},StopC={5},WarnT={6},WarnC={7},OffLineT={8},OffLineC={9}, " +
                        " PlanPowerOnT={10} ,PowerOnT={11} ,PowerOffT={12} ,IsPlanPowerOn={13},PlanShifts={14},Shifts={15},PlanWorkLoad={16},WorkLoad={17} "
                        , productlineid, datestr, RunT, RunC, StopT, StopC, WarnT, WarnC, OffLineT, OffLineC, PlanPowerOnT, RunT + WarnT, StopT + OffLineT, isplanpoweron, PlanShifts
                        , Shifts, PlanWorkLoad, WorkLoad);
                    int insertC = MySqlHelper.ExecuteSql(strsql);
                    logger.Info(productlineid + "产线生产统计数据插入数量：" + insertC);
                    

                }
            }
            catch(Exception  ex)
            {
                logger.Error(ex.Message);
                TaskFail("3004");
            }

    }
        public void DeviceRptDay(string datestr)
        {
            //查看前日所有班次

            try {
                string delsql = "delete from  RptDeviceDay where productionT ='" + datestr + "'";
                int delC = MySqlHelper.ExecuteSql(delsql);
                if (delC > 0)
                    logger.Info("RptDeviceDay删除现有旧数据，数量：" + delC);



                DataTable dts = new DataTable();
                string strsql = "select group_concat(id_usage) as id_usage,sum(nown) as nowan, sum(childn) as childn,productlineid  from `usage` where  createtime like '" + datestr + "%'   group by productlineid   order by productlineid ";
                dts = MySqlHelper.ExecuteQuery(strsql);
                if (dts.Rows.Count == 0)
                {
                    logger.Info("当日无生产班次！");
                    return;
                }


                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    string id_usages = string.Empty;
                    int PlanWorkLoad = 0;
                    int productlineid = 0;
                    int isplanpoweron = 0;
                    int PlanPowerOnT = 0;
                    int WorkLoad = 0;
                    
                    id_usages = Convert.ToString(dts.Rows[i]["id_usage"]);
                    PlanWorkLoad = Convert.ToInt32(dts.Rows[i]["childn"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["childn"]);
                    productlineid = Convert.ToInt32(dts.Rows[i]["productlineid"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["productlineid"]);
                   // WorkLoad = Convert.ToInt32(dts.Rows[i]["nowan"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["nowan"]);

                    //查询当日是否计划开机及计划开机时间

                    strsql = "select `ProductionDays` ,ProductionShifts,ProductionTimes from productlineinfo where productlineid=" + productlineid;
                    dts = MySqlHelper.ExecuteQuery(strsql);
                    if (dts.Rows.Count == 0)
                    {
                        logger.Info("查询产线参数设置为空！");
                        return;
                    }
                    string ProductionDays = Convert.ToString(dts.Rows[i]["ProductionDays"]);
                    PlanPowerOnT = Convert.ToInt32(dts.Rows[i]["ProductionTimes"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["ProductionTimes"]);
                    isplanpoweron = Global.IsWeekInList(datestr, ProductionDays) == true ? 1 : 0;
                    //查询各事件次数及事件时间
                    strsql =  string.Format(
                       "  insert into RptDeviceDay  " +
                        " select null as id, t.Deviceid,'{0}' as ProductionT ,t.productlineid ,t2.run_t as runt,t2.run_c as runc,t2.free_t as freet,t2.free_c as freec," +
                        " t2.warn_t as warnt,t2.warn_c as warnc,{1} as PlanPowerOnT" +
                        " ,t2.run_t+t2.free_t+t2.warn_t as PowerOnT,{1}-(t2.run_t+t2.free_t+t2.warn_t) as PowerOffT, {2} as isplanpoweron,  {3} as PlanWorkLoad ,{4} as WorkLoad   from" +
                        
                        " (select a.ProductLineId, a.DeviceId, b.StationName from deviceinfo a left join " +
                    " locationcfg b on a.LocationId = b.LocationId and  a.ProductLineId = b.ProductLineId " +
                    " where a.ProductLineId ={5} ) t left join " +
                    " (select stationNAME, if (run_c is null ,0,run_c)as run_c, round( if (run_t is null ,0,run_t) /3600 ,2) as run_t, round(if (free_t is null ,0,free_t) /3600 ,2) as free_t  , " +
                    " if (free_c is null ,0,free_c)as free_c , round(if (warn_t is null ,0,warn_t) /3600 ,2) as warn_t, if (warn_c is null ,0,warn_c)as warn_c " +
                    "  from(select   sum( if (stationstate = '运行', if( (endtime - startTime)<300000,endtime - startTime,300000), 0))/ 1000 AS run_t, sum( if (stationstate = '运行',1, 0))  AS run_c, sum( if " +
                    " (stationstate = '空闲', endtime - startTime, 0))/ 1000 AS free_t, sum( if (stationstate = '空闲',1, 0))  AS free_c, " +
                    " sum( if (stationstate = '报警', endtime - startTime, 0))/ 1000 AS warn_t, sum( if (stationstate = '报警',1, 0))   " +
                    "  AS warn_c, stationNAME from huabao.LocationState where id_usage in ({6}) group by stationNAME ) t1)t2 on t.StationName = t2.StationName where t2.StationName is not null "
                    ,datestr , PlanPowerOnT, isplanpoweron, PlanWorkLoad,WorkLoad,productlineid,id_usages);
                    int insertC = MySqlHelper.ExecuteSql(strsql);
                   logger.Info(productlineid+"产线机器生产统计数据插入数量：" + insertC);
                   
 
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                TaskFail("3005");
            }
        }


        public void LineErrRptDay(string datestr)
        {
            //查看前日所有班次
            try
            {

                string delsql = "delete from  RptLineErrDay where productionT ='" + datestr + "'";
                int delC = MySqlHelper.ExecuteSql(delsql);
                if (delC > 0)
                    logger.Info("RptDeviceErrDay删除现有旧数据，数量：" + delC);

                DataTable dts = new DataTable();
                string strsql = "select group_concat(id_usage) as id_usage,sum(nown) as nowan, sum(childn) as childn,productlineid  from `usage` where  createtime like '" + datestr + "%'   group by productlineid   order by productlineid ";
                dts = MySqlHelper.ExecuteQuery(strsql);
                if (dts.Rows.Count == 0)
                {
                    logger.Info("当日无生产班次！");
                    return;
                }
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    string id_usages = string.Empty;
                    double warnt = 0;
                    int warnc = 0;
                    double firstwarninter = 0f;
                    double avgwarnt = 0f;
                    double avgwarninter = 0f;

                    id_usages = Convert.ToString(dts.Rows[i]["id_usage"]);
                    int productlineid = Convert.ToInt32(dts.Rows[i]["productlineid"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["productlineid"]);

                    string[] ids = id_usages.Split(',');

                    for (int x = 0; x < ids.Length; x++)
                    {
                        //每一班
                        int idusage = int.Parse(ids[x]);
                        string sql = "   select tt2.*,tt1.warn_t ,tt1.warn_c from " +
            "   (select '生产线' as stationname,     sum( if (stationstate = '报警', endtime - startTime, 0))/1000 AS warn_t,  sum( if (stationstate = '报警',1, 0)) " +
           " AS warn_c   from huabao.LocationState where ID_USAGE = " + idusage + " and stationname = '生产线'    " +
            "             ) tt1 left join(select '生产线' as stationname, round((beginwarn - beginwork) / 1000) as firstwarninter, round((endwork - endwarn) / 1000) as lstwarninter, round((endwarn - beginwarn) / 1000) as warninter from( " +
           "   select(select min( if (StartTime is null,0,StartTime))  from locationstate where id_usage =  " + idusage + " and stationname = '生产线' and stationstate = '报警') as beginwarn " +
          "   , (select min(if (StartTime is null,0,StartTime)) from locationstate where id_usage =  " + idusage + " and stationname = '生产线') as beginwork, " +
         "    (select max(if (endtime is null,0,endtime)) from locationstate where id_usage =  " + idusage + " and stationname = '生产线') as endwork, " +
         "     (select max(if (endtime is null,0,endtime))  from locationstate where id_usage =  " + idusage + " and stationname = '生产线' and stationstate = '报警') as endwarn) tt )tt2 on " +
         "     tt1.stationname = tt2.stationname  ";
                        DataTable warnlinedt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);
                        if (warnlinedt.Rows.Count > 0)
                        {
                             warnc = Convert.ToInt32(warnlinedt.Rows[0]["warn_c"].ToString());
                            
                            if (warnc > 0)
                            {
                                 firstwarninter = Convert.ToInt32(warnlinedt.Rows[0]["firstwarninter"].ToString());
                                int lstwarninter = Convert.ToInt32(warnlinedt.Rows[0]["lstwarninter"].ToString());
                                int warninter = Convert.ToInt32(warnlinedt.Rows[0]["warninter"].ToString());

                                 warnt = Convert.ToDouble(warnlinedt.Rows[0]["warn_t"].ToString());
                                 avgwarnt = warnt / warnc > 0 ? warnc : 1;
                                 avgwarninter = (warninter - warnt) / warnc > 1 ? warnc - 1 : 1;

                            
                            }
                        }
                       

                        strsql = string.Format("  insert into RptlineErrDay  " +
                                      "  select null as id, '{0}' as ProductionT, {1}  as ProductLineId, {7} as IDUsage,   {2}  as   WarnT, {3} as   WarnC,  {4}  as AvgWarnT , {5}  as AvgWarnInter" +
                                      " ,{6} as FirstWarnInter   ",
                               datestr, productlineid, Math.Round(warnt, 2)  , warnc, Math.Round(avgwarnt, 2)  , Math.Round(avgwarninter, 2), Math.Round(firstwarninter, 2) , idusage);
                        int insertC = MySqlHelper.ExecuteSql(strsql);
                        logger.Info(strsql);
                        logger.Info(productlineid + "产线报警统计数据插入数量：" + insertC);


                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                TaskFail("3006");
            }
        }


        public void DeviceErrRptDay(string datestr)
        {
            //查看前日所有班次
            try
            {

                string delsql = "delete from  RptDeviceErrDay where productionT ='" + datestr + "'";
                int delC = MySqlHelper.ExecuteSql(delsql);
                if (delC > 0)
                    logger.Info("RptDeviceErrDay删除现有旧数据，数量：" + delC);

                DataTable dts = new DataTable();
                string strsql = "select group_concat(id_usage) as id_usage,sum(nown) as nowan, sum(childn) as childn,productlineid  from `usage` where  createtime like '" + datestr + "%'   group by productlineid   order by productlineid ";
                dts = MySqlHelper.ExecuteQuery(strsql);
                if (dts.Rows.Count == 0)
                {
                    logger.Info("当日无生产班次！");
                    return;
                }
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    string id_usages = string.Empty;


                    id_usages = Convert.ToString(dts.Rows[i]["id_usage"]);
                    int productlineid = Convert.ToInt32(dts.Rows[i]["productlineid"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["productlineid"]);

                    string[] ids = id_usages.Split(',');
                    List<Dictionary<string, object>> devwarnlist = new List<Dictionary<string, object>>();

                    for (int x = 0; x < ids.Length; x++)
                    {
                        //每一班
                        int idusage = int.Parse(ids[x]);

                        string sql = "   select tt6.deviceid,tt1.* ,round((tt2.beginwarn-tt3.beginwork)/1000) as firstwarninter, round((tt4.endwork-tt5.endwarn)/1000) as lstwarninter,round((tt5.endwarn-tt2.beginwarn)/1000) as warninter " +
     "  from(  select  stationname,  sum( if (stationstate = '报警', endtime - startTime, 0))/1000 AS warn_t,  sum( if (stationstate = '报警',1, 0))  AS warn_c    from huabao.LocationState where " +
  "  ID_USAGE = " + idusage + "     group by stationname)tt1 left join " +
  " (select a.deviceid ,stationname from deviceinfo a left join locationcfg b on a.locationid=b.locationid and a.productlineid=b.productlineid where a.productlineid=" + productlineid + " ) tt6 on tt1.stationname=tt6.stationname " +
   "left join " +
  "    (select stationname, min( if (StartTime is null,0,StartTime)) as beginwarn " +
  "    from locationstate where id_usage = " + idusage + "    and stationstate = '报警' group by stationname )tt2 on tt1.stationname = tt2.stationname " +
  "    left join(  select stationname, min( if (StartTime is null,0,StartTime)) as beginwork " +
  "    from locationstate where id_usage = " + idusage + "  group by stationname)tt3 on tt1.stationname = tt3.stationname " +
  "      left join(  select stationname, max( if (endtime is null,0,endtime)) as endwork " +
  "    from locationstate where id_usage = " + idusage + "  group by stationname)tt4 on tt1.stationname = tt4.stationname " +
  "        left join(  select stationname, max( if (endtime is null,0,endtime)) as endwarn " +
  "    from locationstate where id_usage = " + idusage + "  and stationstate = '报警'   group by stationname)tt5 on tt1.stationname = tt5.stationname where tt1.warn_c>0 ";

                        DataTable devwarndt = Common.DbHelper.MySqlHelper.ExecuteQuery(sql);

                        if (devwarndt.Rows.Count > 0)
                        {
                            for (int tt = 0; tt < devwarndt.Rows.Count; tt++)
                            {
                                string stationname = devwarndt.Rows[tt]["stationname"].ToString();
                                string deviceid = devwarndt.Rows[tt]["deviceid"].ToString();
                                int warn_c = Convert.ToInt32(devwarndt.Rows[tt]["warn_c"].ToString());
                                double firstwarninter = Convert.ToInt32(devwarndt.Rows[tt]["firstwarninter"].ToString());
                                int lstwarninter = Convert.ToInt32(devwarndt.Rows[tt]["lstwarninter"].ToString());
                                int warninter = Convert.ToInt32(devwarndt.Rows[tt]["warninter"].ToString());

                                double warnt = Convert.ToDouble(devwarndt.Rows[tt]["warn_t"].ToString());
                                double avgwarntm = warnt / (warn_c > 0 ? warn_c : 1);
                                double avgwarninter = (warninter - warnt) /( warn_c > 1 ? warn_c - 1 : 1);

                                strsql = string.Format("  insert into RptDeviceErrDay  " +
                                              "  select null as id,  '{0}' as DeviceId, '{1}' as ProductionT, {2} as ProductLineId,{8} as IDUsage,   {3} as   WarnT, {4} as   WarnC,  {5} as AvgWarnT ,{6} as AvgWarnInter" +
                                              " ,{7} as FirstWarnInter  ",
                                      deviceid, datestr, productlineid, Math.Round(warnt, 2) , warn_c, Math.Round(avgwarntm, 2) , Math.Round(avgwarninter, 2) , Math.Round(firstwarninter, 2), idusage);
                                int insertC = MySqlHelper.ExecuteSql(strsql);
                                logger.Info(strsql);
                                logger.Info(productlineid + "设备报警统计数据插入数量：" + insertC);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                TaskFail("3007");
            }
        }


        public void LocationRptDay(string datestr)
        {
            //查看前日所有班次
            try
            {

                string delsql = "delete from  RptLocationDay where productionT ='" + datestr + "'";
                int delC = MySqlHelper.ExecuteSql(delsql);
                if (delC > 0)
                    logger.Info("RptLocationDay删除现有旧数据，数量：" + delC);

                DataTable dts = new DataTable();
                string strsql = "select group_concat(id_usage) as id_usage,sum(nown) as nowan, sum(childn) as childn,productlineid  from `usage` where  createtime like '" + datestr + "%'   group by productlineid   order by productlineid ";
                dts = MySqlHelper.ExecuteQuery(strsql);
                if (dts.Rows.Count == 0)
                {
                    logger.Info("当日无生产班次！");
                    return;
                }
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    string id_usages = string.Empty; 
                    id_usages = Convert.ToString(dts.Rows[i]["id_usage"]);
                    int productlineid = Convert.ToInt32(dts.Rows[i]["productlineid"].Equals(DBNull.Value) ? 0 : dts.Rows[i]["productlineid"]);
                  string  inserttsql = " insert into rptlocationday " +
             " select null   ,t2.locationid ,'"+datestr +"' ,"+productlineid+",t1.run_t,t1.run_C,t1.free_t,t1.free_c,t1.warn_t,t1.warn_c from  " +
             " (select sum( if (stationstate = '运行', endtime - startTime, 0))/ 1000 AS " +
             " run_t,sum( if (stationstate = '运行',1, 0))  AS run_c, sum( if (stationstate = '空闲', endtime - startTime, 0))/ 1000 AS free_t, " +
             " sum( if (stationstate = '空闲',1, 0))  AS free_c, sum( if (stationstate = '报警', endtime - startTime, 0))/ 1000 AS warn_t, " +
             " sum( if (stationstate = '报警',1, 0))  AS warn_c, stationNAME from huabao.LocationState where id_usage in ("+id_usages+") " +
             " group by stationNAME ) as t1 left join   huabao.locationcfg t2  on t1.stationNAME = t2.StationName where locationid is not null ";

                    int insertC = MySqlHelper.ExecuteSql(inserttsql);
                    if (insertC > 0)
                        logger.Info("rptlocationday插入报表数据，数量：" + insertC);
                     
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                TaskFail("3008");
            }
        }
        public void LineDataRptMonth(string datestr)
        {
            //判断是否月末
            if(!Global.IsMonthLast(datestr ))
            {
                logger.Info("未到月末");
                return;
            }
            string monthstr = Convert.ToDateTime(datestr).ToString("yyyy-MM");

            string delsql = "delete from  rptproductmonth where productionT ='" + monthstr + "'";
            int delC = MySqlHelper.ExecuteSql(delsql);
            if (delC > 0)
                logger.Info("rptproductmonth删除现有旧数据，数量：" + delC);


            string strsql = string.Format(
                  "  insert into rptproductmonth  " +
                  "  SELECT null, ProductLineId, '{0}' as ProductionT, sum(RunT) as RunT, SUM(RunC) AS RunC, sum(StopT) as StopT, sum(StopC) StopT, sum(WarnT) as WarnT, " +
                  "  sum(WarnC) as WarnC, sum(OffLineT) as OffLineT, sum(OffLineC) as OffLineC, sum(PlanPowerOnT) as PlanPowerOnT, sum(PowerOnT) asPowerOnT, sum(PowerOffT) as PowerOffT, " +
                  "  sum(IsPlanPowerOn) as PlanPowerOns, sum(PlanShifts) as PlanShifts, sum(Shifts) as Shi, sum(PlanWorkLoad) as PlanWorkLoad, sum(WorkLoad) as WorkLoad ,count(id) as Days FROM rptproductday where ProductionT like '{0}%' group by ProductLineId"
                  , monthstr);
            int insertC = MySqlHelper.ExecuteSql(strsql);
            logger.Info("产线生产统计月报数据插入数量：" + insertC);

        }
        public void DeviceRptMonth(string datestr)
        {
            //判断是否月末
            if (!Global.IsMonthLast(datestr))
            {
                logger.Info("未到月末");
                return;
            }
            string monthstr = Convert.ToDateTime(datestr).ToString("yyyy-MM");
            string delsql = "delete from  rptdevicemonth where productionT ='" + monthstr + "'";
            int delC = MySqlHelper.ExecuteSql(delsql);
            if (delC > 0)
                logger.Info("rptdevicemonth删除现有旧数据，数量：" + delC);


            string strsql = string.Format(
                  "  insert into rptdevicemonth  " +
                  "  SELECT null as id, DeviceId, '{0}' as ProductionT, ProductLineId, sum(RunT) as RunT, SUM(RunC) AS RunC, sum(FreeT) as  freeT," +
                  " sum(FreeC) as freeC, sum(WarnT) as WarnT, sum(WarnC) as WarnC," +
                  " sum(PlanPowerOnT) as PlanPowerOnT, sum(PowerOnT) asPowerOnT, sum(PowerOffT) as PowerOffT," +
                   " sum(IsPlanPowerOn) as PlanPowerOns, sum(PlanWorkLoad) as PlanWorkLoad, sum(WorkLoad) as WorkLoad ,count(id) as Days FROM rptdeviceday where ProductionT like '{0}%'  group by DeviceId, ProductLineId"
                    , monthstr);
            int insertC = MySqlHelper.ExecuteSql(strsql);
            logger.Info("机器运行统计月报数据插入数量：" + insertC);
        }
        public void LineErrMonth(string datestr)
        {
            //判断是否月末
            if (!Global.IsMonthLast(datestr))
            {
                logger.Info("未到月末");
                return;
            }
            string monthstr = Convert.ToDateTime(datestr).ToString("yyyy-MM");

  
            string delsql = "delete from  rptLineerrMonth where productionT ='" + monthstr + "'";
            int delC = MySqlHelper.ExecuteSql(delsql);
            if (delC > 0)
                logger.Info("rptLineerrMonth删除现有旧数据，数量：" + delC);

            string strsql = string.Format(
                  "  insert into rptLineerrMonth  " +
                  " SELECT null as  id,  '{0}' as ProductionT, ProductLineId,  sum(WarnT) as warnT, sum(WarnC) as WarnC ," +
                  " round(sum(WarnT)/sum(WarnC),2) as AvgWarnT,round(sum(AvgWarnInter)/count(*),2) as AvgWarnInter,round( sum(FirstWarnInter)/count(*),2) as FirstWarnInter ,count(distinct ProductionT) as Days  " +
                  " FROM rptlineerrday where  ProductionT like '{0}%' group by ProductLineId "
                    , monthstr);
            int insertC = MySqlHelper.ExecuteSql(strsql);
            logger.Info("产线报警统计月报数据插入数量：" + insertC);
        }

        public void DeviceErrMonth(string datestr)
        {
            //判断是否月末
            if (!Global.IsMonthLast(datestr))
            {
                logger.Info("未到月末");
                return;
            }
            string monthstr = Convert.ToDateTime(datestr).ToString("yyyy-MM");


            string delsql = "delete from  rptdeviceerrMonth where productionT ='" + monthstr + "'";
            int delC = MySqlHelper.ExecuteSql(delsql);
            if (delC > 0)
                logger.Info("rptdeviceerrMonth删除现有旧数据，数量：" + delC);

            string strsql = string.Format(
                  "  insert into rptdeviceerrMonth  " +
                  " SELECT null as  id, DeviceId, '{0}' as ProductionT, ProductLineId, sum(WarnT) as warnT, sum(WarnC) as WarnC , " +
                  " round(sum(WarnT)/sum(WarnC),2) as AvgWarnT,round(sum(AvgWarnInter)/count(*),2) as AvgWarnInter, round(sum(FirstWarnInter)/count(*),2) as FirstWarnInter ,count(distinct ProductionT) as Days " +
                  " FROM rptdeviceerrday where  ProductionT like '{0}%' group by DeviceId,ProductLineId "
                    , monthstr);
            int insertC = MySqlHelper.ExecuteSql(strsql);
            logger.Info("机器报警统计月报数据插入数量：" + insertC);
        }

        public void LineDataRptYear(string datestr)
        {
            //判断是否月末
            if (!Global.IsYearLast(datestr))
            {
                logger.Info("未到年末");
                return;
            }
            string monthstr = Convert.ToDateTime(datestr).ToString("yyyy");


            string delsql = "delete from  rptproductyear where productionT ='" + monthstr + "'";
            int delC = MySqlHelper.ExecuteSql(delsql);
            if (delC > 0)
                logger.Info("rptproductyear删除现有旧数据，数量：" + delC);

            string strsql = string.Format(
                  "  insert into rptproductyear  " +
                  "  SELECT null, ProductLineId, '{0}' as ProductionT, sum(RunT) as RunT, SUM(RunC) AS RunC, sum(StopT) as StopT, sum(StopC) StopT, sum(WarnT) as WarnT, " +
                  "  sum(WarnC) as WarnC, sum(OffLineT) as OffLineT, sum(OffLineC) as OffLineC, sum(PlanPowerOnT) as PlanPowerOnT, sum(PowerOnT) asPowerOnT, sum(PowerOffT) as PowerOffT, " +
                  "  sum(PlanPowerOns) as PlanPowerOns, sum(PlanShifts) as PlanShifts, sum(Shifts) as Shi, sum(PlanWorkLoad) as PlanWorkLoad, sum(WorkLoad) as WorkLoad ,sum(days) as days,count(id) as months FROM rptproductMonth where ProductionT like '{0}%' group by ProductLineId"
                  , monthstr);
            int insertC = MySqlHelper.ExecuteSql(strsql);
            logger.Info("产线生产统计年报数据插入数量：" + insertC);

        }
        public void DeviceRptYear(string datestr)
        {
            //判断是否月末
            if (!Global.IsYearLast(datestr))
            {
                logger.Info("未到年末");
                return;
            }
            string monthstr = Convert.ToDateTime(datestr).ToString("yyyy");

            string delsql = "delete from  rptdeviceyear where productionT ='" + monthstr + "'";
            int delC = MySqlHelper.ExecuteSql(delsql);
            if (delC > 0)
                logger.Info("rptdeviceyear删除现有旧数据，数量：" + delC);

            string strsql = string.Format(
                  "  insert into rptdeviceyear  " +
                  "  SELECT null as id, DeviceId, '{0}' as ProductionT, ProductLineId, sum(RunT) as RunT, SUM(RunC) AS RunC, sum(FreeT) as  freeT," +
                  " sum(FreeC) as freeC, sum(WarnT) as WarnT, sum(WarnC) as WarnC," +
                  " sum(PlanPowerOnT) as PlanPowerOnT, sum(PowerOnT) asPowerOnT, sum(PowerOffT) as PowerOffT," +
                   " sum(PlanPowerOns) as PlanPowerOns, sum(PlanWorkLoad) as PlanWorkLoad, sum(WorkLoad) as WorkLoad ,sum(days) as days,count(id) as months  FROM rptdevicemonth where ProductionT like '{0}%'  group by DeviceId, ProductLineId"
                    , monthstr);
            int insertC = MySqlHelper.ExecuteSql(strsql);
            logger.Info("机器运行统计年报数据插入数量：" + insertC);
        }
        public void LineErrYear(string datestr)
        {
            //判断是否月末
            if (!Global.IsYearLast(datestr))
            {
                logger.Info("未到月末");
                return;
            }
            string monthstr = Convert.ToDateTime(datestr).ToString("yyyy");

            string delsql = "delete from  rptLineerrYear where productionT ='" + monthstr + "'";
            int delC = MySqlHelper.ExecuteSql(delsql);
            if (delC > 0)
                logger.Info("rptLineerrYear删除现有旧数据，数量：" + delC);

            string strsql = string.Format(
                  "  insert into rptLineerrYear " +
                  " SELECT null as  id, '{0}' as ProductionT, ProductLineId,   sum(WarnT) as warnT, sum(WarnC) as WarnC ," +
                  " round(sum(WarnT)/sum(WarnC),2) as AvgWarnT,round(sum(AvgWarnInter)/count(*),2) as AvgWarnInter, round(sum(FirstWarnInter)/count(*),2) as FirstWarnInter,sum(days) as days,count(id) as months  FROM rptlineerrMonth where  ProductionT like '{0}%' group by ProductLineId "
                    , monthstr);
            int insertC = MySqlHelper.ExecuteSql(strsql);
            logger.Info("产线报警统计年报数据插入数量：" + insertC);
        }
        public void DeviceErrYear(string datestr)
        {
            //判断是否月末
            if (!Global.IsYearLast(datestr))
            {
                logger.Info("未到月末");
                return;
            }
            string monthstr = Convert.ToDateTime(datestr).ToString("yyyy");

            string delsql = "delete from  rptdeviceerrYear where productionT ='" + monthstr + "'";
            int delC = MySqlHelper.ExecuteSql(delsql);
            if (delC > 0)
                logger.Info("rptdeviceerrYear删除现有旧数据，数量：" + delC);

            string strsql = string.Format(
                  "  insert into rptdeviceerrYear " +
                  " SELECT null as  id, DeviceId, '{0}' as ProductionT, ProductLineId,   sum(WarnT) as warnT, sum(WarnC) as WarnC , " +
                  " round(sum(WarnT)/sum(WarnC),2) as AvgWarnT,round(sum(AvgWarnInter)/count(*),2) as AvgWarnInter, round(sum(FirstWarnInter)/count(*),2) as FirstWarnInter,sum(days) as days,count(id) as months  FROM rptdeviceerrMonth where  ProductionT like '{0}%' group by DeviceId,ProductLineId "
                    , monthstr);
            int insertC = MySqlHelper.ExecuteSql(strsql);
            logger.Info("机器报警统计年报数据插入数量：" + insertC);
        }
        public void HisDataTransfer(string datestr)
        {

            logger.Info("历史数据迁移");
        }
        public void AutoTaskFinish(string datestr)
        {

            logger.Info("日终完成");
        }

    }

}
