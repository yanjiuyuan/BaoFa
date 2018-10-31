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
   
    [RoutePrefix("api/dt")]
    public class LineDataController : ApiController,IRequiresSessionState
    {
        private static Logger logger = Logger.CreateLogger(typeof(LineDataController));

        //定义缓存锁类型
        public static object _oLock = new object();


       
        

        [Route("Get")]
        public HttpResponseMessage Get()
        {
             if (HttpContext.Current.IsWebSocketRequest)
            {
                var user = (SessionUser)HttpContext.Current.Session["user"];
                var ooo = new Func<AspNetWebSocketContext, Task>((x) => ProcessWSChat(x, user));
                HttpContext.Current.AcceptWebSocketRequest(ooo);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }


        /// <summary>
        /// WebSocket接口
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Route("ProcessWSChat")]
        private async Task ProcessWSChat(AspNetWebSocketContext arg, SessionUser user)
        {
            WebSocket socket = arg.WebSocket;
            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {
                    string lineid = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
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

                 
                        DataTable linedt = new DataTable();
                        try {
                            //获取所有生产线
                            linedt =  GetLineDt(user);
                            for (int x = 0; x < linedt.Rows.Count; x++)
                            {
                                int tmpid = 0;
                                int.TryParse(linedt.Rows[x][0].ToString(), out tmpid);
                                if (lineid != null && tmpid != int.Parse(lineid))
                                {
                                    linedt.Rows.RemoveAt(x);
                                    x--;
                                }
                            }

                            strJsonString = RunAllTask(linedt);
                        }
                        catch(Exception ex)
                        {
                            logger.Info(ex.Message);
                        }
                        string  returnMessage = strJsonString;
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
        public DataTable GetLineDt(SessionUser user)
        {
            string role = string.Empty;
            string departid = "";
           
            if (user != null)
            {
                role = user.roleid;
                departid = user.departid;
              

            }
            

            ProductionLinesServer pServer = new ProductionLinesServer();

           return pServer.GetLinesList(role, departid);

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
        public string GetAllTable([FromUri]int? lineid=0 )
        {
            string strJsonString = string.Empty;

           
            
              
                    DataTable linedt = GetLineDt((SessionUser)HttpContext.Current.Session["user"]);

                for (int x = 0; x < linedt.Rows.Count; x++)
                {
                    int tmpid = 0;
                    int.TryParse(linedt.Rows[x][0].ToString(), out tmpid);
                    if (lineid != 0 && tmpid !=  lineid)
                    {
                        linedt.Rows.RemoveAt(x);
                    x--;
                } 
                }
  
                strJsonString = RunAllTask(linedt);
             
             
            string returnMessage = strJsonString;
            return returnMessage;
        }


        [Route("GetTable")]

        public static Dictionary<string, string> GetTableByTableName(int sprayid,string sprayname,int lineid,string deviceid)
        {
            LineDataServer lineDataServer = new LineDataServer();
        Dictionary<string, string> result = lineDataServer.GetTableMessage(sprayid, sprayname, lineid, deviceid);
            return result;
        }

        /// <summary>
        /// 多线程同时查N张表
        /// </summary>
        /// <param name="strTableNames">表名数组</param>
        /// <returns>返回Json格式数组</returns>
        public static string RunAllTask( DataTable  linedt)
        {
         
            string strJsonString = string.Empty;
           List<Hashtable> list = new List<Hashtable>();
            try
            {
            
            if(linedt.Rows.Count<1)
            return strJsonString = "{\"ErrorType\":1,\"ErrorMessage\":\"生产线数量为0!\"}";
             

            for (int x=0;x< linedt.Rows.Count;x++)
            {
                    Hashtable dic = new Hashtable();
                    int lineid = 0;
                    string linename= string.Empty; ;
                    int.TryParse(linedt.Rows[x][0].ToString(), out lineid);


                  

                linename = linedt.Rows[x][1].ToString();

                dic.Add("ProductLineId", lineid.ToString());
                dic.Add("ProductLineName", linename);
                    //获取产线的所有胶站工位名称及胶站编号
                    Dictionary<string, Dictionary<string, string>> dString = new Dictionary<string, Dictionary<string, string>>();

                    LineDataServer lds = new LineDataServer();

                    Dictionary<string, Dictionary<string, string>> vdt = lds.getVstate(lineid);
                    foreach(var x1 in vdt)
                    dString.Add(x1.Key, x1.Value);


                    SortedList<int, KeyValuePair<string,string>> spraylist = new LineDataServer().GetLineSprayList(lineid) ;
                    
                    Task[] tasks = new Task[spraylist.Count];
                    for (int i = 0; i < spraylist.Count; i++)
                    {
                        int sprayid = spraylist.Keys[i];
                        string  sprayname = spraylist.Values[i].Key;
                        string deviceid = spraylist.Values[i].Value;
                        lock (_oLock)
                        {
                            tasks[i] = Task.Factory.StartNew(() =>
                        {
                        });
                            dString.Add(sprayname, GetTableByTableName(sprayid, sprayname, lineid, deviceid));
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
