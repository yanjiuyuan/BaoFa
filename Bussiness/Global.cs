using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
   public static class Global
    {

        public  static string  RETURN_EMPTY =  "{\"ErrorType\":1,\"ErrorMessage\":\"暂无数据!\"}";
        public static string RETURN_ERROR(string msg)
        {
            string err_Str = "{\"ErrorType\":1,\"ErrorMessage\":\""+msg+"!\"}";
            return err_Str;
        }

    }
}
