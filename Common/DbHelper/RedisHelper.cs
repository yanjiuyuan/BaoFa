using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Common.DbHelper
{

    public sealed class RedisConfig : ConfigurationSection
    {

        public static string WriteServerConStr
        {
            get
            {
                return string.Format("{0}", "192.168.1.138:6379");
            }
        }
        public static string ReadServerConStr
        {
            get
            {
                return string.Format("{0}", "192.168.1.138:6379");
            }
        }
        public static int MaxWritePoolSize
        {
            get
            {
                return 50;
            }
        }
        public static int MaxReadPoolSize
        {
            get
            {
                return 200;
            }
        }
        public static bool AutoStart
        {
            get
            {
                return true;
            }
        }

    }
}
