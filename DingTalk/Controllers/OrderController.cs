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
        /// 返回订单所有数据
        /// </summary>
        /// <returns></returns>
        /// [{"ID_Order":2,"OrderID":"18-35-12-1","OrderN":1847,"Size":42,"Material":4,"BaoTouL":1.2,"WeiTiaoW":1.3,"HuChiW":1.4,"XingTiN":15,"Color":3,"Customer":1,"ExpCountries":1,"KRXTM":1,"XTDH":1}]
        /// Material  1：色织布，2：帆布，3：尼龙，4：皮革，5：反毛皮，6：其他
        /// Color 1：黑色，2：白色，3：红色，4：黄，5：绿，6：紫，7：其他
        ///  /// 测试数据：/order/GetOrder
        public string GetOrder()
        {
            OrderServer oServer = new OrderServer();
            return oServer.GetAllOrderMessage();
        }



        /// <summary>
        /// 订单条件查询
        /// </summary>
        /// <param name="PageIndex">页数(0开始为第一页)</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="strColor">颜色</param>
        /// <param name="strMaterial">材质</param>
        /// <param name="strOrderTime">订单下单时间</param>
        /// <param name="strDeliveryTime">交货时间</param>
        /// [{"ID_Order":2,"OrderID":"18-35-12-1","OrderN":1847,"Size":42,"Material":4,"BaoTouL":1.2,"WeiTiaoW":1.3,"HuChiW":1.4,"XingTiN":15,"Color":3,"Customer":1,"ExpCountries":1,"KRXTM":1,"XTDH":1}]
        /// Material  1：色织布，2：帆布，3：尼龙，4：皮革，5：反毛皮，6：其他
        /// Color 1：黑色，2：白色，3：红色，4：黄，5：绿，6：紫，7：其他
        /// <returns></returns>
        /// 测试数据：/order/GetOrderByPara?PageIndex=0&PageSize=1&strColor=1&strMaterial=2&strStartOrderTime=2010-01-01&strEndOrderTime=2018-03-01&strStartDeliveryTime=2010-01-01&strEndDeliveryTime=2018-03-01
        public string GetOrderByPara(int PageIndex, int PageSize, string strColor,
            string strMaterial, string strStartOrderTime, string strEndOrderTime,
            string strStartDeliveryTime, string strEndDeliveryTime)
        {
            OrderServer oServer = new OrderServer();
            return oServer.GetAllOrderMessageWithParameter(PageIndex, PageSize, strColor,
             strMaterial, strStartOrderTime, strEndOrderTime,
             strStartDeliveryTime, strEndDeliveryTime);
        }


    }
}