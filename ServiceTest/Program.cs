using Bussiness.ProductionLines;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductionLinesServer pd = new ProductionLinesServer();


           string s= pd.GetLineTreeList();
            Console.Write(s);
            Console.ReadKey();














           string connstr = @"Host=47.96.172.122;UserName=huabao;Password=huabao2025;Database=huabao;Port=3306;CharSet=utf8;Allow Zero Datetime=true";

            using (MySqlConnection connection = new MySqlConnection(connstr))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string sql = "update getpics set getpicstime=@getpicstime where id=260";
                MySqlCommand cmd = new MySqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@getpicstime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }

        }
    }
}
