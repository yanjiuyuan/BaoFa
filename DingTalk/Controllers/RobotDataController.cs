using Bussiness;
using Bussiness.LineData;
using Bussiness.Model;
using Bussiness.ProductionLines;
 
using Common.LogHelper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.SessionState;
using System.Web.WebSockets;

namespace DingTalk.Controllers
{
    //同样是用来做websocket连接
   

    [RoutePrefix("api/RobotData")]
    public class RobotDataController : ApiController,IRequiresSessionState
    {
        private static Logger logger = Logger.CreateLogger(typeof(RobotDataController));

        //定义缓存锁类型
        public static object _oLock = new object();


       
        
        //获取机器人数据
        [Route("GetRobot")]
        public HttpResponseMessage GetRobot()
        {
             if (HttpContext.Current.IsWebSocketRequest)
            {
                var user = (SessionUser)HttpContext.Current.Session["user"];
                var ooo = new Func<AspNetWebSocketContext, Task>((x) => ProcessRobot(x, user));
                HttpContext.Current.AcceptWebSocketRequest(ooo);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }


        /// <summary>
        /// WebSocket接口
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Route("ProcessRobot")]
        private async Task ProcessRobot(AspNetWebSocketContext arg, SessionUser user)
        {
            
            WebSocket socket = arg.WebSocket; 
            try
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {

                    string lineid = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    if (lineid == null)
                        return;

                   while (socket.State == WebSocketState.Open)
                    {
                        string strJsonString = string.Empty;

                        Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
                         int lineint = int.Parse(lineid);
                        string LINESTR = lineint.ToString("D2") + "-";
                          for (int i = 0; i < Bussiness.RobotData.datakeys.Count; i++)
                                {
                                    if (Bussiness.RobotData.datakeys[i].StartsWith(LINESTR))
                                    {

                                        dic.Add(Bussiness.RobotData.datakeys[i], Bussiness.RobotData.data[Bussiness.RobotData.datakeys[i]]);
                                    }
                                } 
                        string returnMessage = JsonConvert.SerializeObject(dic); 
                        buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMessage));
                        await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        Thread.Sleep(200);
                    }
                }
                
            }

            catch(Exception ex)
            {

                logger.Info(ex.Message);
            }
        }



        //获取产线气缸数据
        [Route("GetQG")]
        public HttpResponseMessage GetQG()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                var user = (SessionUser)HttpContext.Current.Session["user"];
                var ooo = new Func<AspNetWebSocketContext, Task>((x) => ProcessQG(x, user));
                HttpContext.Current.AcceptWebSocketRequest(ooo);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }


        /// <summary>
        /// WebSocket接口
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Route("ProcessQG")]
        private async Task ProcessQG(AspNetWebSocketContext arg, SessionUser user)
        {
            WebSocket socket = arg.WebSocket;
            String id = System.Guid.NewGuid().ToString("N");
            RabbitServer rbs = new RabbitServer();
            try
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {

                    string lineid = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    Action<string> act = new Action<string>((s) =>
                    {
                        try { 
                        ArraySegment<byte> buffer1 = new ArraySegment<byte>(new byte[1024]);
                        buffer1 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(s));
                        socket.SendAsync(buffer1, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        catch(Exception ex)
                        {
                             
                        }
                    });


                    rbs.CreateMq("Q_Line2Ser_Auto_" + id, int.Parse(lineid));



                    if (socket.State == WebSocketState.Open)
                    {
                        rbs.GetMqMsg("Q_Line2Ser_Auto_" + id, int.Parse(lineid), id, act);
                    }
                    while(socket.State == WebSocketState.Open)
                   {
                        Thread.Sleep(1);
                    }
                     
                }
                }
                    catch (Exception ex)
                    {
                        logger.Info(ex.Message);
                    }
            finally
            {
                rbs.DeleteMq("Q_Line2Ser_Auto_" + id,id);


            }

        }


        [AcceptVerbs("GET","POST")]
        [Route("Robot")]
        public string Robot([FromUri]int? lineid=0 )
        {
            string strJsonString = string.Empty;
            if(lineid!=0)
            {

                Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
                try
                {
                        string LINESTR = ((int)lineid).ToString("D2") + "-";


                        for (int i = 0; i < Bussiness.RobotData.datakeys.Count; i++)
                        {
                            if (Bussiness.RobotData.datakeys[i].StartsWith(LINESTR))
                            {

                                dic.Add(Bussiness.RobotData.datakeys[i], Bussiness.RobotData.data[Bussiness.RobotData.datakeys[i]]);
                            }


                        }

                   

                    strJsonString = JsonConvert.SerializeObject(dic);
                }
                catch (Exception ex)
                {
                    logger.Info(ex.Message);
                }

            }
              
            string returnMessage = strJsonString;
            return returnMessage;
        }

        [AcceptVerbs("GET", "POST")]
        [Route("QG")]
        public string QG([FromUri]int? lineid = 0)
        {
            string strJsonString = string.Empty;
            try { 
                 if (lineid != 0)
                {

                 RabbitServer rbs = new RabbitServer();
                strJsonString= rbs.RcvDefaultMq("Q_Line2Ser_"+ lineid.ToString(),(int )lineid );

                
                }
            }
            catch (Exception ex)
                {
                    logger.Info(ex.Message);
                }
 
 
            return strJsonString;
        }




    }
}
