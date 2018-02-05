using Common.DbHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Company
{
  public  class CompanyServer
    {
        public string GetAllCompanyInfo()
        {
            string strSql = "SELECT * FROM companyinfo";
            DataTable db = MySqlHelper.ExecuteQuery(strSql);
            string strJsonString = JsonConvert.SerializeObject(db);
            return strJsonString;
        }
    }
}
