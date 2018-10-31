namespace DingTalk
{
    using Bussiness;
    using Bussiness.AutoTask;
    using Common.LogHelper;
 
    using System;
    using System.Threading;
    using System.Timers;

    public static class AutoTask  
    {

        private static Logger logger = Logger.CreateLogger(typeof(AutoTask));
        private static  string statics_date =string.Empty;

       public static void Initialize()
        {
            
            System.Timers.Timer myTimer = new System.Timers.Timer(5000);
            //TaskAction.SetContent 表示要调用的方法  
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler( SetContent);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;



        }

        //12点之后判断是否可以进行日终处理。若允许日终则直接日终处理
        //否则最迟3点进行日终处理
        private static void SetContent(object source, ElapsedEventArgs e)
        {
            string s = DateTime.Now.ToString("HH:mm:ss");
            if (String.Compare(s,"01:00:01")>0  && String.Compare(s, "04:00:00") < 0)
            {
                logger.Info("日终任务调起，执行数据清理工作");
                if (AutoTask.statics_date.Equals(Global.GetDbDate()))
                {
                    logger.Info("已启动日终执行！");
                    return;
                }
                if (String.Compare(s, "01:00:01") > 0 && String.Compare(s, "03:00:00") < 0)
                {
                    AutoTaskServer ats = new AutoTaskServer();
                int finished = ats.isProductFinish();
                if (finished != 0)
                {
                    logger.Info("生产未完成！");
                    return;
                }
                }
               
                TaskExec();

                 

            }
            else
            {

                logger.Info("未到日终时间!");
                return;
            }
        }

        //轮训未执行的任务，一个个执行。
        private static void TaskExec( )
        {

            AutoTask.statics_date = Global.GetDbDate();
            AutoTaskServer ats = new AutoTaskServer();
              ats.TaskExec(AutoTask.statics_date);
        }

    }
}
