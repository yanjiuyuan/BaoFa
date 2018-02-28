using Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bussiness.Code
{

    public class CodeServer
    {
        YzmClass yzm = new YzmClass();
        public byte[] GetCode(string strCode)
        {
            byte[] CreateImage = yzm.CreateImage(strCode);
            return CreateImage;
        }


        public string GetStrCode(int? Length = 4)
        {
           return  yzm.CreateRandomCode(Length);
        }
    }
}
