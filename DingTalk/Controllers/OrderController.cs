using Bussiness.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 返回订单总数据
        /// </summary>
        /// <returns></returns>
        /// [{"ID_Order":2,"OrderID":"18-35-12-1","OrderN":1847,"Size":42,"Material":4,"BaoTouL":1.2,"WeiTiaoW":1.3,"HuChiW":1.4,"XingTiN":15,"Color":3,"Customer":1,"ExpCountries":1,"KRXTM":1,"XTDH":1}]
        /// Material  1：色织布，2：帆布，3：尼龙，4：皮革，5：反毛皮，6：其他
        /// Color 1：黑色，2：白色，3：红色，4：黄，5：绿，6：紫，7：其他
        public string GetOrder()
        {

            OrderServer oServer = new OrderServer();
            return oServer.GetOrderMessage();
        }
    }
}