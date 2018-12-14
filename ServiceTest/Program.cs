using Bussiness.AutoTask;
using Bussiness.Chart;
using Bussiness.ProductionLines;
using Bussiness.Report;
using Bussiness.WorkSataions;
using Common.DbHelper;
 
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceTest
{
    public class WState
    {
        public WState(int val1 )
        {
            val = val1;
            
            timestamp = System.DateTime.Now;
        }
        public int val = 0;
        public bool locked = false;
        public DateTime timestamp;
    };
    class Program
    {
        //将List转换为TXT文件
        public   static void WriteListToTextFile(List<string> list, string txtFile)
        {
            //创建一个文件流，用以写入或者创建一个StreamWriter 
            FileStream fs = new FileStream(txtFile, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            // 使用StreamWriter来往文件中写入内容 
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < list.Count; i++) sw.WriteLine(list[i]);
            //关闭此文件t 
            sw.Flush();
            sw.Close();
            fs.Close();
        }


        /// </summary>
        private static readonly ConnectionFactory rabbitMqFactory = new ConnectionFactory()
        {
            HostName = "47.96.172.122",
            UserName = "admin",
            Password = "admin",
            Port = 5672,
            VirtualHost = "/"
        };
        /// <summary>
        /// 路由名称
        /// </summary>
        const string ExchangeName = "E_Line2Ser";

        //队列名称
        const string QueueName = "";


      static   Dictionary<string, int> dic = new Dictionary<string, int>()
        {{"I0",0},{"I1",0},{"O2",0},{"O4",0},{"I11",0},{"O7",0},{"I15",0},{"O10",0},
            {"I19",0},{"O12",0},{"I23",0},{"O14",0},{"I27",0},{"O17",0},{"I31",0},
            {"O23",0},{"O24",0},{"I43",0} ,{"O28",0},{"O26",0},{"O25",0},{"O29",0},{"I48",0},{"I49",0}

        };

        static Dictionary<string, int[]> dic3 = new Dictionary<string, int[]>()
        {{"I0", new int[]{1} },
            {"I1", new int[]{1} },
            { "O2",new int[]{64,128}},
            {"O4",new int[]{2, 1024, 4,1,2048 } },          {"I11",new int[]{1, 2048, 2,1024,4 } },
            { "O7",new int[]{2, 1024, 4, 1, 2048 } },  {"I15",new int[]{1, 2048, 2,   1024, 8 } },
          
            { "O10",new int[]{2, 1, 4,1024,2048 }}, {"I19",new int[]{ 1, 1024, 2, 2048, 32 } },
            { "O12",new int[]{4, 1, 8 ,1024,2048}}, {"I23",new int[]{ 1024,2048,2, 1, 8 } },
            {"O14",new int[]{2, 1, 4,1024,2048 }}, {"I27",new int[]{ 1024,2048,2, 1, 8 } },

            { "O17",new int[]{2, 1, 4,1024,2048 }},{"I31",new int[]{ 1024,2048,2, 1, 8 } },
            { "O23",new int[]{1,2,4,16,8}},
            { "O24",new int[]{2, 1, 4,1024,2048 }},{"I43",new int[]{ 1024,2048,4, 1, 16 } },

            { "O28",new int[]{1,2 }},
             { "O26",new int[]{128,1,2,4,8 }},
              { "O25",new int[]{1,64,2,4,8 }},
               { "O29",new int[]{2,4,32,8,16 }},
                { "I48",new int[]{64 }} ,
                 { "I49",new int[]{3,2,1 }}         };



        static  Dictionary<string, WState> dic2 = new Dictionary<string, WState>()  ;



        static void Main(string[] args)
        {
            //foreach(var key in dic.Keys)
            //{
            //    dic2.Add(key, new WState(0)); 
            //}

            //try {
            //    while (true)
            //    {
            //        foreach (var key in dic2.Keys)
            //        {
            //            if (dic2[key].val == 0)
            //            { DateTime dt = System.DateTime.Now;

            //                TimeSpan ts = dt.Subtract(dic2[key].timestamp).Duration();
            //                //超过三秒，进行下一次赋值
            //                if (ts.Seconds >= 3)
            //                {
            //                    int[] list = dic3[key];
            //                    dic2[key].val = list[0];
            //                    dic2[key].timestamp = System.DateTime.Now;
            //                }

            //            }
            //            if (dic2[key].val != 0)
            //            {
            //                DateTime dt = System.DateTime.Now;

            //                TimeSpan ts = dt.Subtract(dic2[key].timestamp).Duration();
            //                //超过三秒，进行下一次赋值
            //                if (ts.Seconds >= 3)
            //                {
            //                    int[] list1 = dic3[key];

            //                    int c = new List<int>(list1).IndexOf(dic2[key].val);


            //                    if (list1.Length == c + 1)
            //                    {
            //                        dic2[key].val = 0;
            //                        dic2[key].timestamp = System.DateTime.Now;
            //                    }
            //                    else if (list1.Length > c + 1)
            //                    {
            //                        dic2[key].val = list1[c + 1];
            //                        dic2[key].timestamp = System.DateTime.Now;
            //                    }

            //                }

            //            }


            //            dic[key] = dic2[key].val;
            //        }

            //        string s = JsonConvert.SerializeObject(dic);
            //        Console.WriteLine(s);
            //        Thread.Sleep(100);
            //        ExchangeSendMsg(s);
            //    }


            //}
            //catch ( Exception ex)
            //{

            //    Console.WriteLine(ex.Message);
            ////}
            AutoTaskServer ats = new AutoTaskServer();
            ats.TaskExec("2018-12-06",1);
            //DataViewServer dv = new DataViewServer();
            //dv.GetData(1);
            //SubTaskServer sub = new SubTaskServer();
            //sub.DbPartCreate("2018-11-24");

            //ChartBeatServer cbt = new ChartBeatServer();
            //cbt.LocationBeatQuery("2018-11-27", "视觉1号", 1);
            //// TopicExchangeSendMsg();
            Console.WriteLine("按任意值，退出程序");
            Console.ReadKey();
        }


       static  void  Twrite()
        {
            while(true)
            {
            Thread.Sleep(10);

             string key = "HSR-DT801B";
              Dictionary<string, string> CoorDic = new Dictionary<string, string>();
            int X = (new Random()).Next(-200, 200);
            int Y = (new Random()).Next(-200, 0);
            int Z= (new Random()).Next(-200, 200);
            RedisHash.SetEntryInHash(key,"P1",X+","+Y+","+Z);
            }
            // WriteListToTextFile(list, "111.txt");
        }
        /// <summary>
        ///  单点精确路由模式
        /// </summary>
        public static void  ExchangeSendMsg(string s )
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                   
                    var props = channel.CreateBasicProperties();
                    props.Persistent = true;
                    IDictionary<string, object> idc = new Dictionary<string, object>();
                    idc.Add("LineId", 1);

                    props.Headers = idc;
                    
                        var msgBody = Encoding.UTF8.GetBytes(s);
                        channel.BasicPublish(exchange: ExchangeName, routingKey: "1", basicProperties: props, body: msgBody);
                        Console.WriteLine(string.Format("***发送时间:{0}，发送完成，输入exit退出消息发送",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                       
                    
                }
            }
        }
        static void readMq()
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    IDictionary<string, object> idc = new Dictionary<string, object>();

                    idc.Add("x-max-length", 10);
                    idc.Add("x-overflow", "drop-head");

                    //BasicGetResult msgResponse;
                    //while ((msgResponse=channel.BasicGet("Q_Ser2Line_2", autoAck: true)) !=null)
                    //{ 
                    //    var msgBody = Encoding.UTF8.GetString(msgResponse.Body);
                    //    Console.WriteLine(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                    //}
                    //channel.ExchangeDeclare(ExchangeName, "direct", durable: true, autoDelete: false, arguments: null);
                    channel.QueueDeclare("Q_Line2Ser_Auto_1", durable: true, autoDelete: false, exclusive: false, arguments: idc);
                    channel.QueueBind(QueueName, "E_Line2Ser", routingKey: "1");
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var msgBody = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                    };
                    channel.BasicConsume("Q_Line2Ser_Auto_1",   true, consumer: consumer);

                    //已过时用EventingBasicConsumer代替
                    //var consumer2 = new QueueingBasicConsumer(channel);
                    //channel.BasicConsume(QueueName, noAck: true, consumer: consumer);
                    //var msgResponse = consumer2.Queue.Dequeue(); //blocking
                    //var msgBody2 = Encoding.UTF8.GetString(msgResponse.Body);

                    Console.WriteLine("consumer启动成功");
                    Console.ReadKey();

                     
                   


                }
            }




        }
        static void Main1(string[] args)
        {

            try
            {
                // string key = "设备1";
                // Dictionary<string, string> CoorDic = new Dictionary<string, string>();
                //List<String> list= RedisList.Get ("robotCoordinate");
                // WriteListToTextFile(list, "111.txt");

              





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //AutoTaskServer  at= new AutoTaskServer();
            //// Console.WriteLine(r.DeviceErrDailyRpt("2018-06-25", "1, 2" ,null));
            //at.TaskExec("2018-12-31");


             Console.ReadKey();

            ReportServer r = new ReportServer();
            // Console.WriteLine(r.DeviceErrDailyRpt("2018-06-25", "1, 2" ,null));
            // Console.WriteLine(r.DeviceErrDailyRpt("2018-06-25", "1, 2", "HNC-808A",0));
            //Console.WriteLine(r.DeviceErrPhaseRpt("2018-06-25", "2018-06-26","1, 2", "HNC-808A", 0));
            Console.WriteLine(r.DeviceErrYearRpt("2018", "1, 2", "HNC-808A", 0));
            Console.WriteLine(r.DeviceErrYearRpt("2018", "1, 2", "HNC-808A", 1));

            Console.ReadKey();

            ReportServer ats = new ReportServer();
            WorkSataionsServer ws = new WorkSataionsServer();
            Console.WriteLine(ats.DeviceDailyRpt("2018-06-25","1,2", "HNC-808A"));
            Console.WriteLine(ats.DevicePhaseRpt("2018-06-25", "2018-06-25", "1,2", "HNC-808A"));
            Console.WriteLine(ats.DeviceMonthRpt("2018-06", "1,2", "HNC-808A"));
            Console.WriteLine(ats.DeviceYearRpt("2018", "1,2", "HNC-808A"));


            DevicesServer ds = new DevicesServer();
            Console.WriteLine(ds.DeviceModelList());
            Console.ReadKey();














           string connstr = @"Host=47.96.172.122;UserName=huabao;Password=huabao2025;Database=huabao;Port=3306;CharSet=utf8;Allow Zero Datetime=true";

            using (MySqlConnection connection = new MySqlConnection(connstr))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string sql = "update getpics set getpicstime=@getpicstime where id=260";
                MySqlCommand cmd = new MySqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@getpicstime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }

        }
    }
}
