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

        //修改完成  20180930

        private static Logger logger = Logger.CreateLogger(typeof(ProductionLinesServer));
        public string GetProductionLinesData(int? ProductLineId 
            , string CompanyId, string telephone, string registertime,
            string role, string status, string GroupId, string FoundryId,
            string IsEnable, string KeyWord
            , int? PageIndex = 0, int? PageSize = 5)
        {
            DataTable dt = new DataTable();
            string strsql = string.Empty;
           
           
            //获取当前用户归属结构的机构层级关系
            try
            {  //查询归属的集团的BrnoDepth
                  int startRow = PageIndex.Value * PageSize.Value;
                StringBuilder sb = new StringBuilder();
                sb.Append(" select a.* from  productlineinfo a left join   (select Brno from branchinfo where  Level=3  )b on a.Brno=b.Brno  ");
                if (ProductLineId != null ||    CompanyId != null || telephone != null || registertime != null
                    || role != null || status != null || IsEnable != null || KeyWord != null || FoundryId != null || GroupId != null)
                {
                    sb.Append(" where 1=1 ");
                }
                if (KeyWord != null)
                {
                    string strWhereKeyWord = string.Format(" and  a.BrName like  '%{0}%'  ", KeyWord);
                    sb.Append(strWhereKeyWord);
                }
                
                if (ProductLineId != null)
                {
                    sb.Append(string.Format(" and a.ProductLineId='{0}'", ProductLineId));
                }
                if (CompanyId != null)
                {
                    sb.Append(string.Format(" and b.UpBrno='{0}'", CompanyId));
                }
                 
                if (FoundryId != null)
                {
                    sb.Append(string.Format(" and  b.Brno='{0}'", FoundryId));
                }
                if (GroupId != null)
                {
                    sb.Append(string.Format(" and BrnoDepth like '00,{0}%'", GroupId));
                }
                if (status != null)
                {
                    sb.Append(string.Format(" and status='{0}'", status));
                }
                sb.Append(" Order by a.ProductLineID ");
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
                string strsql = "select * from branchinfo where Level=1";


                dt = MySqlHelper.ExecuteQuery(strsql);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }

            return JsonConvert.SerializeObject(dt);

        }


        public DataTable GetLinesList(string role="01", string brno = "01")
        {
            DataTable dt = new DataTable();
            string strsql = string.Empty;
            int deplevel = Global.GetDepLevelByRole(role);
            string BrnoDepth = Global.GetBrnoDepthByDepartID(deplevel, brno);
            //获取当前用户归属结构的机构层级关系
            try
            {  //查询归属的集团的BrnoDepth
                    strsql = "select a.* from  productlineinfo a left join   (select Brno from branchinfo where  Level=3 and  BrnoDepth like '" + BrnoDepth+"%' )b on a.Brno=b.Brno  Order by a.Brno,a.ProductLineID";
                    dt = MySqlHelper.ExecuteQuery(strsql);
                     
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }

            return dt;

        }
        
        public string GetLineTreeList(string role = "01", string brno="01")
        {
            List<Hashtable> list = new List<Hashtable>();
            int deplevel = Global.GetDepLevelByRole(role);
            string BrnoDepth = string.Empty;
            string strsql= string.Empty;
            //获取当前用户归属结构的机构层级关系
            try
            {
                DataTable dt = new DataTable();
                BrnoDepth = Global.GetBrnoDepthByDepartID(deplevel, brno);

                strsql = " select Brno,BrName,UpBrno,Level from branchinfo where  BrnoDepth like '" + BrnoDepth + "%' order by BrnoDepth";

                dt = MySqlHelper.ExecuteQuery(strsql);

                string listgroupid = string.Empty;
                string listcompanyid = string.Empty;


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Level"].ToString() == "1")//集团
                    {
                        Hashtable t2 = new Hashtable();
                        t2.Add("groupid", dt.Rows[i]["Brno"]);
                        t2.Add("groupnm", dt.Rows[i]["BrName"]);
                        t2.Add("list", new List<Hashtable>());
                        list.Add(t2);
                        listgroupid = dt.Rows[i]["Brno"].ToString();
                    }
                    if (dt.Rows[i]["Level"].ToString() == "2")//公司
                    {
                        //归属集团brno
                        string upbrno = dt.Rows[i]["UpBrno"].ToString();
                        Hashtable upT = list.Find(x => (x["groupid"].ToString() == listgroupid));

                        Hashtable t1 = new Hashtable();
                        t1.Add("companyid", dt.Rows[i]["Brno"]);
                        t1.Add("companynm", dt.Rows[i]["BrName"]);
                        t1.Add("list", new List<Hashtable>());
                        List<Hashtable> com_list1 = (List<Hashtable>)(upT)["list"];
                        com_list1.Add(t1);
                        listcompanyid = dt.Rows[i]["Brno"].ToString();
                    }

                    if (dt.Rows[i]["Level"].ToString() == "3")//产线
                    {
                        //上级公司机构
                        string upbrno = dt.Rows[i]["UpBrno"].ToString();
                        List<Hashtable> com_list = (List<Hashtable>)(list.Find(x => (x["groupid"].ToString() == listgroupid)))["list"];
                        List<Hashtable> fdy_list = (List<Hashtable>)(com_list.Find(x => (x["companyid"].ToString() == upbrno)))["list"];
                        Hashtable t1 = new Hashtable();
                        t1.Add("foundryid", dt.Rows[i]["Brno"]);
                        t1.Add("foundrynm", dt.Rows[i]["BrName"]);
                        t1.Add("list", new List<Hashtable>());

                        //查询产线加入列表

                          strsql = "select ProductLineId,ProductLineName from  productlineinfo where Brno='" + dt.Rows[i]["Brno"].ToString() + "'";
                        DataTable dt2 = MySqlHelper.ExecuteQuery(strsql);
                        for (int linenum = 0; linenum < dt2.Rows.Count; linenum++)
                        {
                            Hashtable t3 = new Hashtable();
                            t3.Add("lineid", dt2.Rows[linenum]["ProductLineId"]);
                            t3.Add("linename", dt2.Rows[linenum]["ProductLineName"]);
                            ((List<Hashtable>)(t1)["list"]).Add(t3);
                        }
                        fdy_list.Add(t1);
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
