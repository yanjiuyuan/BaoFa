using Common.DbHelper;
using Common.JsonHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.LogHelper;
using Newtonsoft.Json;
using System.Collections;

namespace Bussiness.ProductionLines
{
    public class ProductionLinesServer
    {

        private static Logger logger = Logger.CreateLogger(typeof(ProductionLinesServer));
        public string GetProductionLinesData(int? ProductLineId, string ProductLineName
            , int? CompanyId, string telephone, string registertime,
            string role, string status, int? GroupId, int? FoundryId,
            string IsEnable, string KeyWord
            , int? PageIndex = 0, int? PageSize = 5)
        {

            try
            {
                int startRow = PageIndex.Value * PageSize.Value;
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from (SELECT a.ProductLineId,a.ProductLineName,a.role,c.telephone,c.registertime,if(a.status=1,'启用','禁用') as  status " +
                      ",d.groupid, d.GroupName,b.FoundryName,b.Foundryid,c.CompanyId,c.CompanyName  FROM huabao.`productlineinfo` a  left join huabao.`foundryinfo` b  on a.FoundryId=b.FoundryId left join huabao.`Companyinfo` c on b.companyid=c.companyid" +
                    " left join groupinfo d on c.groupid=d.groupid) t1  ");
                if (ProductLineId != null || ProductLineName != null || CompanyId != null || telephone != null || registertime != null
                    || role != null || status != null || IsEnable != null || KeyWord != null || FoundryId != null || GroupId != null)
                {
                    sb.Append(" where 1=1 ");
                }
                if (KeyWord != null)
                {
                    string strWhereKeyWord = string.Format(
                        " and ( ProductLineId like  '%{0}%' " +
                        "   or  ProductLineName like  '%{0}%' " +
                          "   or  CompanyName like  '%{0}%' " +
                          "   or  GroupName like  '%{0}%' " +
                           "   or FoundryName like  '%{0}%' " +
                           "   or  telephone like  '%{0}%' " +
                        "or role like  '%{0}%' ) ", KeyWord);
                    sb.Append(strWhereKeyWord);
                }
                if (ProductLineName != null)
                {
                    sb.Append(string.Format(" and ProductLineName='{0}' ", ProductLineName));
                }
                if (ProductLineId != null)
                {
                    sb.Append(string.Format(" and ProductLineId='{0}'", ProductLineId));
                }
                if (CompanyId != null)
                {
                    sb.Append(string.Format(" and CompanyId='{0}'", CompanyId));
                }
                if (telephone != null)
                {
                    sb.Append(string.Format(" and telephone='{0}'", telephone));
                }
                if (FoundryId != null)
                {
                    sb.Append(string.Format(" and FoundryId='{0}'", FoundryId));
                }
                if (GroupId != null)
                {
                    sb.Append(string.Format(" and GroupId='{0}'", GroupId));
                }
                if (status != null)
                {
                    sb.Append(string.Format(" and status='{0}'", status));
                }
                int iRows = MySqlHelper.ExecuteQuery(sb.ToString()).Rows.Count;
                string strWhereLimit = string.Format(" LIMIT {0},{1}", startRow, PageSize.Value);

                sb.Append(strWhereLimit);
                DataTable db = MySqlHelper.ExecuteQuery(sb.ToString());
                //Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
                //dic.Add("Spray", db);
                string strJsonString = JsonHelper.DataTableToJson(db, iRows);
                return strJsonString;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Global.RETURN_ERROR(ex.Message);
            }
        }



        public string GetGroupList()
        {
            DataTable dt = new DataTable();
            try
            {
                string strsql = "select * from groupinfo";


                dt = MySqlHelper.ExecuteQuery(strsql);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }

            return JsonConvert.SerializeObject(dt);

        }


        public DataTable GetLinesList(string role="01", int departid=1)
        {
            DataTable dt = new DataTable();
            string strsql = string.Empty;
            try
            {

        strsql = " select t1.* ,d.groupname from (select t.*, c.companyname ,c.groupid  from (select a.ProductLineId, a.ProductLineName, b.foundryid, b.foundryname ,b.companyid  " +
              " from productlineinfo a left join foundryinfo b  on a.foundryid = b.foundryid  where a.status =1 ) t  " +
             " left join companyinfo c   on t.companyid = c.companyid ) t1 " +
          "left join groupinfo d on t1.groupid = d.groupid " +
            "  where 1=1  ";
                    if ("02".Equals(role))
                        strsql += " and  t1.groupid=" + departid;
                    else if ("03".Equals(role))
                     
                        strsql += " and  t1.companyid=" + departid;
                    else if ("04".Equals(role))
                    
                        strsql += " and  t1.foundryid=" + departid;

                strsql +=" order by groupid,companyid,foundryid,ProductLineId";

                dt = MySqlHelper.ExecuteQuery(strsql);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }

            return dt;

        }
        
        public string GetLineTreeList(string role = "01", int departid = 1)
        {
            DataTable dt = new DataTable();
            List<Hashtable> list = new List<Hashtable>();
            try
            {

            string     strsql = " select t1.* ,d.groupname from (select t.*, c.companyname ,c.groupid  from (select a.ProductLineId, a.ProductLineName, b.foundryid, b.foundryname ,b.companyid  " +
                    " from productlineinfo a left join foundryinfo b  on a.foundryid = b.foundryid ) t  " +
                   " left join companyinfo c   on t.companyid = c.companyid ) t1 " +
                "left join groupinfo d on t1.groupid = d.groupid " +
                  "  where 1=1 ";
                if ("02".Equals(role))
                    strsql += " and    t1.groupid=" + departid;
                else if ("03".Equals(role))

                    strsql += " and    t1.companyid=" + departid;
                else if ("04".Equals(role))

                    strsql += " and    t1.foundryid=" + departid;

                strsql += "  order by groupid,companyid,foundryid,ProductLineId";

                dt = MySqlHelper.ExecuteQuery(strsql);
                string lstgroupid = string.Empty;
                string lstfoundryid = string.Empty;
                string lstcompanyid = string.Empty;
                string lstlineid = string.Empty;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string groupid = dt.Rows[i]["groupid"].ToString();
                    string foundryid = dt.Rows[i]["foundryid"].ToString();
                    string companyid = dt.Rows[i]["companyid"].ToString();
                    string lineid = dt.Rows[i]["ProductLineId"].ToString();
                    string groupnm = dt.Rows[i]["groupname"].ToString();
                    string foundrynm = dt.Rows[i]["foundryname"].ToString();
                    string companynm = dt.Rows[i]["companyname"].ToString();
                    string linenm = dt.Rows[i]["ProductLineName"].ToString();

                    if (!groupid.Equals(lstgroupid))
                    {
                        Hashtable t = new Hashtable();
                        t.Add("groupid", groupid);
                        t.Add("groupnm", groupnm);
                        t.Add("list", new List<Hashtable>());
                        list.Add(t);
                        lstgroupid = groupid;
                    }

                    if (!companyid.Equals(lstcompanyid))
                    {
                        List<Hashtable> com_list = (List<Hashtable>)(list.Find(x => (x["groupid"].ToString() == lstgroupid)))["list"];
                        Hashtable t = new Hashtable();
                        t.Add("companyid", companyid);
                        t.Add("companynm", companynm);
                        t.Add("list", new List<Hashtable>());

                        com_list.Add(t);
                        lstcompanyid = companyid;

                    }
                    if (!foundryid.Equals(lstfoundryid))
                    {
                        List<Hashtable> com_list = (List<Hashtable>)(list.Find(x => (x["groupid"].ToString() == lstgroupid)))["list"];
                        List<Hashtable> fdy_list = (List<Hashtable>)(com_list.Find(x => (x["companyid"].ToString() == lstcompanyid)))["list"];

                        Hashtable t = new Hashtable();
                        t.Add("foundryid", foundryid);
                        t.Add("foundrynm", foundrynm);
                        t.Add("list", new List<Hashtable>());
                        fdy_list.Add(t);
                        lstfoundryid = foundryid;

                    }
                    if (!lineid.Equals(lstlineid))
                    {
                        List<Hashtable> com_list = (List<Hashtable>)(list.Find(x => (x["groupid"].ToString() == lstgroupid)))["list"];
                        List<Hashtable> fdy_list = (List<Hashtable>)(com_list.Find(x => (x["companyid"].ToString() == lstcompanyid)))["list"];
                        List<Hashtable> line_list = (List<Hashtable>)(fdy_list.Find(x => (x["foundryid"].ToString() == lstfoundryid)))["list"];

                        Hashtable t = new Hashtable();
                        t.Add("lineid", lineid);
                        t.Add("linename", linenm);
                        line_list.Add(t);
                        lstlineid = lineid;

                    }


                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            return JsonConvert.SerializeObject(list);



        }

        public string ProductionCalSet(int ProductLineId ,int ProductionBeat,int ProductionShifts,
            string ProductionDays, string ProductionTimes, string oper)
        {
            string retstr = string.Empty;
           
            try
            {

                string strsql = string.Format("update  productlineinfo set  ProductionBeat={0} , " +
                    " ProductionShifts={1} ," + " ProductionTimes={2} ," +
                    " ProductionDays='{3}'  " +
                    " where ProductLineId= {4}", ProductionBeat, ProductionShifts, ProductionTimes, ProductionDays, ProductLineId);
                 
                int anum = MySqlHelper.ExecuteSql(strsql);

                if (anum <= 1)
                    retstr = Global.RETURN_ERROR("操作失败!");
                retstr = Global.RETURN_SUCESS;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                retstr =Global.RETURN_ERROR(ex.Message);
            }
            return retstr;



        }
    }
}
