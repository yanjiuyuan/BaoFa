using Bussiness.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DingTalk.Controllers
{
    public class CodeController : Controller
    {
        // GET: Code
        /// <summary>
        /// 验证码接口
        /// </summary>
        /// <param name="codeLength">验证码长度</param>
        /// <returns></returns>
        public ActionResult Index(int? codeLength=4)
        {
            CodeServer codeServer = new CodeServer();
            string strCode=codeServer.GetStrCode(codeLength);
            Session["Code"] = strCode;
            return File(codeServer.GetCode(strCode).ToArray(), "image/jpeg");
        }
    }
}