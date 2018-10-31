using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Common.LogHelper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace Common.RabbitMQ
{
    public static class RabbitMQHelper
    {
        private static Logger logger = Logger.CreateLogger(typeof(RabbitMQHelper));
        private static readonly ConnectionFactory rabbitMqFactory;

        //用于保存本身服务于redis的长链接，一个产线一条连接
        public static Dictionary<int, KeyValuePair<IConnection, IModel>> dic  = new Dictionary<int, KeyValuePair<IConnection, IModel>>();

        //用于保存实时页面打开时与redis的长连接，一个页面打开或刷新一条连接
        public static Dictionary<string, KeyValuePair<IConnection, IModel>> dicnostatic = new Dictionary<string, KeyValuePair<IConnection, IModel>>();



        static RabbitMQHelper()
        {
            logger.Info("RabbitMQHelper init   ");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "bin\\Common.config");
            logger.Info("xml = " + xmlDoc.OuterXml);
            XmlNode node = xmlDoc.SelectSingleNode("configuration/RabbitMQ");
            string host = node.SelectSingleNode("HostName").InnerText;
            string user = node.SelectSingleNode("UserName").InnerText;
            string pwd = node.SelectSingleNode("Password").InnerText;
            int port = int.Parse(node.SelectSingleNode("Port").InnerText);
            string vhost = node.SelectSingleNode("VirtualHost").InnerText;
            rabbitMqFactory = new ConnectionFactory()
            {
                HostName = host,
                UserName = user,
                Password = pwd,


                VirtualHost = vhost,
                Port = port

            };

        }
        /// </summary>

        /// <summary>
        /// 路由名称
        /// </summary>


        public static void ExchangeSendMsg(string msg, int lineid, string ExchangeName = "E_Line2Ser")
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

                    var msgBody = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(exchange: ExchangeName, routingKey: lineid.ToString(), basicProperties: props, body: msgBody);
                    Console.WriteLine(string.Format("***发送时间:{0}，发送完成，输入exit退出消息发送",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));


                }
            }
        }
        public static void CreateMq(string QueueName, int lineid, string ExchangeName = "E_Line2Ser")
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {

                    IDictionary<string, object> idc = new Dictionary<string, object>();

                    idc.Add("x-max-length", 50);
                    idc.Add("x-overflow", "drop-head");

                    channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: idc);
                    channel.QueueBind(QueueName, ExchangeName, routingKey: lineid.ToString());

                    logger.Info(QueueName + "create成功");

                }
            }
        }




        public static void GetMqMsg(string QueueName, int lineid, string id, Action<string> ret, string ExchangeName = "E_Line2Ser")
        {
            try
            {
                IConnection conn; IModel channel;
                conn = rabbitMqFactory.CreateConnection();
                channel = conn.CreateModel();

                IDictionary<string, object> idc = new Dictionary<string, object>();

                idc.Add("x-max-length", 50);
                idc.Add("x-overflow", "drop-head");
                channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: idc);
                channel.QueueBind(QueueName, ExchangeName, routingKey: lineid.ToString());
                var consumer = new EventingBasicConsumer(channel);


                consumer.Received += (model, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body);
                    ret(msgBody);
                    logger.Info(string.Format(id+"*******************接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                };

                channel.BasicConsume(QueueName, true, consumer: consumer);
                logger.Info("consumer启动成功");

                dicnostatic.Add(id, new KeyValuePair<IConnection, IModel>(conn, channel));
            }
            catch (Exception e)
            {
                logger.Info(e.Message);

            }
         
        }

        public static void ListenMqMsg(string QueueName, int lineid, Action<int, string> act, string ExchangeName = "E_Line2Ser")
        {
            try
            {
                IConnection conn; IModel channel;
               conn = rabbitMqFactory.CreateConnection();
               channel = conn.CreateModel();
  
                IDictionary<string, object> idc = new Dictionary<string, object>();

                idc.Add("x-max-length", 100);
                idc.Add("x-overflow", "drop-head");
                channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: idc);
                channel.QueueBind(QueueName, ExchangeName, routingKey: lineid.ToString());
                var consumer = new EventingBasicConsumer(channel);
        

                consumer.Received += (model, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body);
                    act(lineid, msgBody);
                    logger.Info(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                };
                 
                channel.BasicConsume(QueueName, true, consumer: consumer);
                logger.Info("consumer启动成功");
                dic.Remove(lineid);
                dic.Add(lineid, new KeyValuePair<IConnection, IModel>(conn, channel));
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                
            }
        }
        public static bool CheckMqStat(int lineid)
        {
            if(dic[lineid].Key.IsOpen==false)
            {
                dic[lineid].Key.Dispose();
                dic[lineid].Value.Dispose(); 
                return false;
            }
            else
            {

                return true;
            }
        }


        public static void DeleteMq(string QueueName,string id )
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                        channel.QueueDelete(QueueName );
                   

                }
            }
            if(dicnostatic.ContainsKey(id))
            {
                if (dicnostatic[id].Key != null)
                    dicnostatic[id].Key.Dispose();
                if (dicnostatic[id].Value != null)
                    dicnostatic[id].Value.Dispose();
                dicnostatic.Remove(id);
            }
        }
        public   static string RcvDefaultMq(string QueueName, int lineid)
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {

                    //BasicGetResult msgResponse;
                    //while ((msgResponse=channel.BasicGet("Q_Ser2Line_2", autoAck: true)) !=null)
                    //{ 
                    //    var msgBody = Encoding.UTF8.GetString(msgResponse.Body);
                    //    Console.WriteLine(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                    //}
                    //channel.ExchangeDeclare(ExchangeName, "direct", durable: true, autoDelete: false, arguments: null);
                    IDictionary<string, object> idc = new Dictionary<string, object>();
                     
                    idc.Add("x-max-length", 100);
                    idc.Add("x-overflow", "drop-head");

                    channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: idc);
                    var message=  channel.BasicGet(QueueName, true);
                    //var consumer = new  QueueingBasicConsumer(channel);

                    //channel.BasicConsume(QueueName, true, consumer: consumer);
                    //var eventArgs =  consumer.d.DequeueNoWait( new BasicDeliverEventArgs());
                     string str = "";

                    if (message.Body!=null)
                        str = Encoding.UTF8.GetString(message.Body);
                    return str;
                }
            }
        }



    }
}
