using Bussiness.LineData;
using Bussiness.ProductionLines;
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

                        //获取所有生产线
                        ProductionLinesServer pServer = new ProductionLinesServer();

                         DataTable  linedt=  pServer.GetLinesList();
                     
                        if(linedt.Rows.Count>0)
                        { 
                            string[] strList = new string[7] { "Vamp", "WaiO", "WaiT", "WaiS", "Outsole", "Mouthguards", "LineUsage" };
                            strJsonString = RunAllTask(strList, linedt);
                             
                        }


                  
                    }
                    if (strMessage.Contains("GetTable"))   //GetTable:Vamp,Waio,WaiT...  
                    {




                        string[] strList = strMessage.Split(':');
                        if (strList.Length >1)
                        {
                            Dictionary<string, DataTable> dString = new Dictionary<string, DataTable>();
                            string strTable = strList[1].ToString();
                            string[] strTablesList = strTable.Split(',');
                            foreach (var item in strTablesList)
                            {
                                dString.Add(item, GetTableByTableName(item,1));
                            }
                            strJsonString = JsonConvert.SerializeObject(dString);
                        }
                        else
                        {
                            strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"参数有误!\"}";
                        }

                    }
                    string returnMessage = strJsonString;
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
        /// 测试数据 /api/dt/GetAllTable?strMessage=GetTable:Vamp,Waio
        /// 测试数据 /api/dt/GetAllTable?strMessage=GetTable:Usage
        [Route("GetAllTable")]
        public string GetAllTable(string strMessage)
        {
            string strJsonString = string.Empty;
            if (strMessage == "GetAllTable")
            {
                ProductionLinesServer pServer = new ProductionLinesServer();

                DataTable linedt = pServer.GetLinesList();
                string[] strList = new string[7] { "Vamp", "WaiO", "WaiT", "WaiS", "Outsole", "Mouthguards", "LineUsage" };
                strJsonString = RunAllTask(strList,linedt);
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
                        dString.Add(item, GetTableByTableName(item,1));
                    }
                    strJsonString = JsonConvert.SerializeObject(dString);
                }
                else
                {
                    strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"未输入表名!\"}";  
                }

            }
            string returnMessage = strJsonString;
            return returnMessage;
        }


        [Route("GetTable")]

        public static DataTable GetTableByTableName(string strTableName,int lineid)
        {
            LineDataServer lineDataServer = new LineDataServer();
            var result = lineDataServer.GetTableMessage(strTableName,lineid);
            return result;
        }

        /// <summary>
        /// 多线程同时查N张表
        /// </summary>
        /// <param name="strTableNames">表名数组</param>
        /// <returns>返回Json格式数组</returns>
        public static string RunAllTask(string[] strTableNames,DataTable  linedt)
        {
            int iCount = strTableNames.Length;
            string strJsonString = string.Empty;
           List<Hashtable> list = new List<Hashtable>();
            try
            {
            
            if(linedt.Rows.Count<1)
            return strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"生产线数量为0!\"}";

            for(int x=0;x< linedt.Rows.Count-1;x++)
            {
                    Hashtable dic = new Hashtable();
                    int lineid = 0;
                    string linename= string.Empty; ;
                    int.TryParse(linedt.Rows[x][0].ToString(), out lineid);
                    linename = linedt.Rows[x][1].ToString();

                dic.Add("ProductLineId", lineid.ToString());
                dic.Add("ProductLineName", linename);
                Dictionary<string, DataTable> dString = new Dictionary<string, DataTable>();

            
                Task[] tasks = new Task[iCount];
                    for (int i = 0; i < strTableNames.Length; i++)
                    {
                        lock (_oLock)
                        {
                            tasks[i] = Task.Factory.StartNew(() =>
                        {
                        });
                            dString.Add(strTableNames[i], GetTableByTableName(strTableNames[i],lineid));
                        }
                    }
                    Task.WaitAll(tasks);
                   dic.Add("Data", dString);
                    list.Add(dic);
                 }
                
            }
            catch (Exception ex)
            {
                return strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"交易处理异常\"}";
                throw;
            }

            return JsonConvert.SerializeObject(list);
        }


        /// <summary>
        /// 读取最新产线工位状态接口
        /// </summary>
        /// <returns></returns>
        /// 测试数据  /api/dt/GetLocationState
        [Route("GetLocationState")]
        public string GetLocationState()
        {
            LineDataServer lineDataServer = new LineDataServer();
            return JsonConvert.SerializeObject(lineDataServer.GetLocationState());
        }

    }
}
