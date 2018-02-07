using Bussiness.LineData;
using Newtonsoft.Json;
using System;
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
using System.Web.WebSockets;

namespace DingTalk.Controllers
{
    [RoutePrefix("api/dt")]
    public class LineDataController : ApiController
    {
        //定义缓存锁类型
        public static object _oLock = new object();

        [Route("Get")]
        public HttpResponseMessage Get()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessWSChat);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }


        /// <summary>
        /// WebSocket接口
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
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
                    string strMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    //string returnMessage = "You send :" + message + ". at" + DateTime.Now.ToLongTimeString();
                    //string returnMessage = GetSpray();


                    ////string returnSpray = GetSpray();
                    //string returnVamp = GetTableByTableName("Vamp");
                    //string returnWaio = GetTableByTableName("Waio");
                    //string returnWaiT = GetTableByTableName("WaiT");
                    //string returnWaiS = GetTableByTableName("WaiS");
                    //string returnOutsole = GetTableByTableName("Outsole");
                    //Dictionary<string, string> dString = new Dictionary<string, string>();
                    ////dString.Add("Spray", returnSpray);
                    //dString.Add("Vamp", returnVamp);
                    //dString.Add("Waio", returnWaio);
                    //dString.Add("WaiT", returnWaiT);
                    //dString.Add("WaiS", returnWaiS);
                    //dString.Add("Outsole", returnOutsole);
                    string strJsonString = string.Empty;
                    
                    if (strMessage == "GetAllTable")
                    {
                        string[] strList = new string[7] { "Vamp", "Waio", "WaiT", "WaiS", "Outsole", "Mouthguards", "LineUsage" };
                        strJsonString = RunAllTask(strList);
                    }
                    if (strMessage.Contains("GetTable"))   //GetTable:Vamp,Waio,WaiT...  
                    {
                        string[] strList = strMessage.Split(':');
                        if (strList.Length > 1)
                        {
                            Dictionary<string, DataTable> dString = new Dictionary<string, DataTable>();
                            string strTable = strList[1].ToString();
                            string[] strTablesList = strTable.Split(',');
                            foreach (var item in strTablesList)
                            {
                                dString.Add(item, GetTableByTableName(item));
                            }
                            strJsonString = JsonConvert.SerializeObject(dString);
                        }
                        else
                        {
                            strJsonString = "未输入表名!";
                        }

                    }
                    string returnMessage = JsonConvert.SerializeObject(strJsonString);
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
        public string GetSpray()
        {
            LineDataServer lineDataServer = new LineDataServer();
            var result = lineDataServer.GetSprayMessage();
            return result;
        }


        /// <summary>
        /// Post时时数据接口
        /// </summary>
        /// <param name="strMessage">获取所有表或者单独表</param>
        /// 所有表 GetAllTable   单独表 GetTable:Vamp,Waio,WaiT... 
        /// <returns></returns>
        /// 测试数据 /api/dt/GetAllTable?strMessage=GetAllTable
        [Route("GetAllTable")]
        public string GetAllTable(string strMessage)
        {
            string strJsonString = string.Empty;
            if (strMessage == "GetAllTable")
            {
                string[] strList = new string[7] { "Vamp", "Waio", "WaiT", "WaiS", "Outsole", "Mouthguards", "LineUsage" };
                strJsonString = RunAllTask(strList);
            }
            if (strMessage.Contains("GetTable"))   //GetTable:Vamp,Waio,WaiT...  
            {
                string[] strList = strMessage.Split(':');
                if (strList.Length > 1)
                {
                    Dictionary<string, DataTable> dString = new Dictionary<string, DataTable>();
                    string strTable = strList[1].ToString();
                    string[] strTablesList = strTable.Split(',');
                    foreach (var item in strTablesList)
                    {
                        dString.Add(item, GetTableByTableName(item));
                    }
                    strJsonString = JsonConvert.SerializeObject(dString);
                }
                else
                {
                    strJsonString = "未输入表名!";
                }

            }
            string returnMessage = JsonConvert.SerializeObject(strJsonString);
            return returnMessage;
        }


        [Route("GetTable")]

        public static DataTable GetTableByTableName(string strTableName)
        {
            LineDataServer lineDataServer = new LineDataServer();
            var result = lineDataServer.GetTableMessage(strTableName);
            return result;
        }


        /// <summary>
        /// 多线程同时查N张表
        /// </summary>
        /// <param name="strTableNames">表名数组</param>
        /// <returns>返回Json格式数组</returns>
        public static string RunAllTask(string[] strTableNames)
        {
            int iCount = strTableNames.Length;
            string strJsonString = string.Empty;
            Dictionary<string, DataTable> dString = new Dictionary<string, DataTable>();
   
            try
            {
                if (iCount >= 1)
                {
                    Task[] tasks = new Task[iCount];
                    for (int i = 0; i < strTableNames.Length; i++)
                    {
                        lock (_oLock)
                        {
                            tasks[i] = Task.Factory.StartNew(() =>
                        {
                        });
                            dString.Add(strTableNames[i], GetTableByTableName(strTableNames[i]));
                        }
                    }
                    Task.WaitAll(tasks);
                    strJsonString = JsonConvert.SerializeObject(dString);
                }
                else
                {
                    strJsonString = "strTableNames数组为空";
                }
            }
            catch (Exception ex)
            {
                strJsonString = ex.Message;
                throw;
            }

            return strJsonString;
        }

    }
}
