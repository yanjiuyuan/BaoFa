using Common.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness 
{
    public class RabbitServer
    {

        public void ExchangeSendMsg(string msg, int lineid, string ExchangeName = "E_Ser2Line")
        {
            RabbitMQHelper.ExchangeSendMsg(msg, lineid, ExchangeName);
        }
        public string RcvDefaultMq(string QueueName, int lineid)
        {
          return  RabbitMQHelper.RcvDefaultMq(QueueName,lineid );
        }
        public void CreateMq(string QueueName, int lineid )
        {
              RabbitMQHelper.CreateMq(QueueName, lineid);
        }

        public void  GetMqMsg(string QueueName, int lineid,string id,Action<string> act)
        {
              RabbitMQHelper.GetMqMsg(QueueName, lineid,id,act);
        }

        public void DeleteMq(string QueueName,string id)
        {
            RabbitMQHelper.DeleteMq(QueueName,id);
        }
    }
}
