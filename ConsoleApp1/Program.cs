using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            
                Bussiness.ChartBeatServer ChartBeatServer1 = new Bussiness.ChartBeatServer();
               string  s= ChartBeatServer1.ChartBeatQuery();
               Console.WriteLine(s);
               Console.ReadKey();
 
        }
    }
}
