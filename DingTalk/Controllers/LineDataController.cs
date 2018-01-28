using Bussiness.LineData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace DingTalk.Controllers
{
    
    [RoutePrefix("api/dt")]
    public class LineDataController : ApiController
    {

        [Route("Get")]
        public HttpResponseMessage Get()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessWSChat);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        [Route("ProcessWSChat")]
        private async Task ProcessWSChat(AspNetWebSocketContext arg)
        {
            WebSocket socket = arg.WebSocket;
            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {
                    string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    //string returnMessage = "You send :" + message + ". at" + DateTime.Now.ToLongTimeString();

                    //string returnMessage = GetSpray();

                    
                    //string returnSpray = GetSpray();
                    string returnVamp = GetTableByTableName("Vamp");
                    string returnWaio = GetTableByTableName("Waio");
                    string returnWaiT = GetTableByTableName("WaiT");
                    string returnWaiS = GetTableByTableName("WaiS");
                    string returnOutsole = GetTableByTableName("Outsole");
                    Dictionary<string, string> dString = new Dictionary<string, string>();
                    //dString.Add("Spray", returnSpray);
                    dString.Add("Vamp", returnVamp);
                    dString.Add("Waio", returnWaio);
                    dString.Add("WaiT", returnWaiT);
                    dString.Add("WaiS", returnWaiS);
                    dString.Add("Outsole", returnOutsole);
                    
                    string returnMessage = JsonConvert.SerializeObject(dString);
                    //returnMessage = returnMessage.Replace(@"\", "");
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMessage));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }


        /// <summary>
        /// 1号胶站信息
        /// </summary>
        /// <returns></returns>
        [Route("GetSpray")]
        public  string GetSpray()
        {
            LineDataServer lineDataServer = new LineDataServer();
            var result = lineDataServer.GetSprayMessage();
            return result;
        }


        [Route("GetTable")]

        public string GetTableByTableName(string strTableName)
        {
            LineDataServer lineDataServer = new LineDataServer();
            var result = lineDataServer.GetTableMessage(strTableName);
            return result;
        }


    }
}
