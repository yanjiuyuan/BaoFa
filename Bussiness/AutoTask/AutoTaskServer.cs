using Bussiness.Time;
using Common.Code;
using Common.DbHelper;
using Common.JsonHelper;
using Common.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bussiness.AutoTask
{

    public class AutoTaskServer
    {

        private static Logger logger =  Logger.CreateLogger(typeof(AutoTaskServer));
        //获取各工位的末次生产时间
        public int isProductFinish()
        {
            int finished = 0;
            string statics_date = string.Empty;

            try
            {
                logger.Info("查询生产是否完成");
                logger.Info("查询昨日生产日期");
                string strSql = "select   date_format( date_add(now(), interval -1 day),'%y-%m-%d')   as yestoday  ";
                DataTable newTb = MySqlHelper.ExecuteQuery(strSql);
                if (newTb.Rows.Count > 0)
                {
                    statics_date = newTb.Rows[0][0].ToString();
                }
                else
                {
                    logger.Error("获取昨日生产日期失败！");
                    return 9;
                    // 获取日终
                }
                logger.Info("查询昨日生产日期完成：" + statics_date);

                finished = 0;
                logger.Info("查询昨日生产是否完成");
                string strSql1 = "select a.id_usage,productlineId,b.linestatus from " +
                     "(select * from `usage` where ProductionT = '18-06-21') a left join line b on a.id_usage = b.id_usage ";
                DataTable newTb1 = MySqlHelper.ExecuteQuery(strSql1);

                if (newTb1.Rows.Count > 0)
                {
                    for (int i = 0; i < newTb1.Rows.Count; i++)
                    {
                        int id_usage = Convert.ToInt32(newTb1.Rows[i]["id_usage"]);
                        int lineid = Convert.ToInt32(newTb1.Rows[i]["productlineId"]);
                        string linestat = Convert.ToString(newTb1.Rows[i]["linestatus"]);
                        logger.Info("班次" + id_usage + "，产线 " + lineid + "，产线状态 " + linestat);
                        if ("运行".Equals(linestat))
                        {
                            logger.Info("查询昨日生产日期完成：" + statics_date);
                            finished = 1;
                            break;
                        }

                    }
                    statics_date = newTb.Rows[0][0].ToString();
                }
                else
                {
                    logger.Error("获取昨日生产是否完成失败！");
                    return 9;
                    // 获取日终
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }


            logger.Info("查询结束:"+(finished==0?"生产完成" :"生产未完成"));
            return finished;
            }


        //



        public void TaskExec(string datestr)
        {
            while(true)
            {

                //增加当前时间判断 大于6点则停止日终任务
                string s = DateTime.Now.ToString("HH:mm:ss");
                if (String.Compare(s, "06:00:01") > 0)
                {
                    logger.Info("超出日终时间，跳出循环");
                    break;
                }

                    string current_taskid = string.Empty;
                string current_taskname = string.Empty;
                string pre_task = string.Empty;
                string msg = string.Empty;
                // 初始化
                string strSql = "select   staticsdate,stat from runctrl where taskid='0000' ";
                DataTable Tb = MySqlHelper.ExecuteQuery(strSql);
                if(Tb.Rows.Count>0)
                {
                    string lststaticsdate = Convert.ToString(Tb.Rows[0]["staticsdate"]);
                    int stat = Convert.ToInt32(Tb.Rows[0]["stat"].Equals(DBNull.Value)?0: Tb.Rows[0]["stat"]);
                    if(String.Compare(lststaticsdate, datestr)<0  )
                    {
                        SubTaskServer sts1 = new SubTaskServer();
                        sts1.taskInit(datestr); 
                    }
                    else if (stat==0)
                    {
                        SubTaskServer sts1 = new SubTaskServer();
                        sts1.taskInit(datestr);
                    }
                }
                //查询待执行步骤
                Tb = new DataTable();
                strSql = "select   taskid ,taskName,pretaskid,msg from runctrl where taskid !='0000' and  stat=0 order by taskid limit 1 ";
                Tb = MySqlHelper.ExecuteQuery(strSql);
                if (Tb.Rows.Count == 0)
                {
                    logger.Info("不存在待执行任务，跳出循环");
                    break;
                }
                current_taskid = Convert.ToString(Tb.Rows[0]["taskid"]);
                msg = Convert.ToString(Tb.Rows[0]["msg"]);
                current_taskname = Convert.ToString(Tb.Rows[0]["taskname"]);
                pre_task = Convert.ToString(Tb.Rows[0]["pretaskid"]) ;
                pre_task = "'" + pre_task.Replace(",", "','")+"'";
              
                //查询待执行步骤的前置步骤是否完成
                strSql = "select  count(*) as uncomnum from runctrl where taskid !='0000' and stat !=2 and taskid in( "+ pre_task + ")";
                Tb = MySqlHelper.ExecuteQuery(strSql);
                if(Convert.ToInt32(Tb.Rows[0]["uncomnum"])>0)
                {
                    logger.Info(" 存在未完成的前置任务！");
                    Thread.Sleep(5000);
                    continue;
                }

                //执行登记
                logger.Info("登记任务" + current_taskid + " ： " +current_taskname + DateTime.Now.ToString());
                strSql = "update runctrl  set " +
                 " stat=1 ,begintime=  date_format(  now() ,'%Y-%m-%d %H:%m:%s') where TaskType='AutoTask' and TaskID='" + current_taskid+"' and stat=0";

                int updateC = MySqlHelper.ExecuteSql(strSql);
                if (updateC != 1)
                {
                    logger.Error("登记"+ current_taskid+"任务开始失败：更新条数：" + updateC);
                    Thread.Sleep(5000);
                    continue;
                }
                //执行待执行步骤
                logger.Info("执行" + current_taskid + "任务开始 ： " + current_taskname + DateTime.Now.ToString());
                SubTaskServer sts = new SubTaskServer();
                sts.RunTask(current_taskid, msg, datestr);
                logger.Info("执行" + current_taskid + "任务完成 ： " + current_taskname + DateTime.Now.ToString());
                //完成登记

                strSql = "update runctrl  set " +
                " stat=2 ,endtime=  date_format(  now() ,'%Y-%m-%d %H:%m:%s') where TaskType='AutoTask' and stat=1 and TaskID='" + current_taskid + "'";

                  updateC = MySqlHelper.ExecuteSql(strSql);
                if (updateC != 1)
                {
                    logger.Error("登记" + current_taskid + "任务完成失败：更新条数：" + updateC);
                    Thread.Sleep(5000);
                    continue;
                }
                logger.Info("完成任务" + current_taskid + " ： " + current_taskname + DateTime.Now.ToString());
                Thread.Sleep(5000);
            }



        }
    }
}
