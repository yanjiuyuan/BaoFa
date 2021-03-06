﻿using Common.DbHelper;
using Common.Excel;
using Common.Files;
using Common.JsonHelper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.LogHelper;
using Bussiness;

namespace Bussiness.Report
{


    public class ReportServer
    {
        private static Logger logger = Logger.CreateLogger(typeof(ReportServer));
        //产量日报
        public void GetDailyReport(string strPath, string strServerPath)
        {

            try
            {
                //复制Excel
                if (File.Exists(strPath))
                {
                    

                    File.Copy(strPath, strServerPath, true);

                    string[] strListTest = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };

                    string strSql = "SELECT a.orderid,a.childid,b.ordern,a.childn,'华宝有限公司' AS factoryName," +
                        "productlineid,ordtime,DeliveryTime,ProductionT,Customer,ExpCountries,KRXTM,XTDH,Material," +
                        "color,XingTiN,BaoTouL,WeiTiaoW,HuChiW,ProductionBeat,NowN,OldN,ALlN,WeiTiaoConsumption," +
                        "HuChiConsumption,BiaoQianConsumption,DaDiConsumption FROM `Usage` a " +
                        "LEFT JOIN `Order` b ON a.orderid=b.orderid   " +
                        " ORDER BY  CreateTime DESC LIMIT 0,1";
                    DataTable db = MySqlHelper.ExecuteQuery(strSql);
                    List<string> list = new List<string>();
                    List<string> listTwo = new List<string>();
                    List<string> listTh = new List<string>();
                    List<string> listFour = new List<string>();
                    List<string> listFive = new List<string>();
                    List<string> listSix = new List<string>();
                    List<string> listSeven = new List<string>();
                    for (int i = 0; i < 20; i++)
                    {
                        //初始化
                        list.Add("");
                        listTwo.Add("");
                        listTh.Add("");
                        listFour.Add("");
                        listFive.Add("");
                        listSix.Add("");
                        listSeven.Add("");
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == 0)
                        {
                            list[i] = db.Rows[0][0].ToString();
                        }
                        if (i == 1)
                        {
                            list[i] = db.Rows[0][1].ToString();
                        }
                        if (i == 4)
                        {
                            list[i] = db.Rows[0][2].ToString();
                        }
                        if (i == 7)
                        {
                            list[i] = db.Rows[0][3].ToString();
                        }
                        if (i == 9)
                        {
                            list[i] = db.Rows[0][4].ToString();
                        }
                        if (i == 13)
                        {
                            list[i] = db.Rows[0][5].ToString();
                        }
                        if (i == 15)
                        {
                            list[i] = db.Rows[0][6].ToString();
                        }
                        if (i == 17)
                        {
                            list[i] = db.Rows[0][7].ToString();
                        }
                        if (i == 19)
                        {
                            list[i] = db.Rows[0][8].ToString();
                        }
                    }

                    for (int i = 0; i < listTwo.Count; i++)
                    {
                        if (i == 0)
                        {
                            listTwo[i] = db.Rows[0][9].ToString();
                        }
                        if (i == 1)
                        {
                            listTwo[i] = db.Rows[0][10].ToString();
                        }
                        if (i == 4)
                        {
                            listTwo[i] = db.Rows[0][11].ToString();
                        }
                        if (i == 7)
                        {
                            listTwo[i] = db.Rows[0][12].ToString();
                        }
                    }


                    for (int i = 0; i < listTh.Count; i++)
                    {
                        if (i == 0)
                        {
                            listTh[i] = db.Rows[0][13].ToString();
                        }
                        if (i == 1)
                        {
                            listTh[i] = db.Rows[0][14].ToString();
                        }
                        if (i == 4)
                        {
                            listTh[i] = db.Rows[0][15].ToString();
                        }
                        if (i == 7)
                        {
                            listTh[i] = db.Rows[0][16].ToString();
                        }
                        if (i == 9)
                        {
                            listTh[i] = db.Rows[0][17].ToString();
                        }
                        if (i == 13)
                        {
                            listTh[i] = db.Rows[0][18].ToString();
                        }
                    }

                    for (int i = 0; i < listFour.Count; i++)
                    {
                        if (i == 0)
                        {
                            listFour[i] = db.Rows[0][19].ToString();
                        }
                        if (i == 1)
                        {
                            listFour[i] = db.Rows[0][20].ToString();
                        }
                    }

                    for (int i = 0; i < listFive.Count; i++)
                    {
                        if (i == 0)
                        {
                            listFive[i] = db.Rows[0][21].ToString();
                        }
                        if (i == 1)
                        {
                            listFive[i] = db.Rows[0][22].ToString();
                        }
                    }

                    for (int i = 0; i < listSix.Count; i++)
                    {
                        if (i == 0)
                        {
                            listSix[i] = db.Rows[0][23].ToString();
                        }
                        if (i == 1)
                        {
                            listSix[i] = db.Rows[0][24].ToString();
                        }
                    }

                    for (int i = 0; i < listSeven.Count; i++)
                    {
                        if (i == 0)
                        {
                            listSeven[i] = db.Rows[0][23].ToString();
                        }
                        if (i == 1)
                        {
                            listSeven[i] = db.Rows[0][24].ToString();
                        }
                    }


                    string[] strListOne = list.ToArray();
                    string[] strListTwo = listTwo.ToArray();
                    string[] strListTh = listTh.ToArray();

                    string[] strListFour = listFour.ToArray();
                    string[] strListFive = listFive.ToArray();
                    string[] strListSix = listSix.ToArray();
                    string[] strListSeven = listSeven.ToArray();
                    //更新3 5 7 行
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListOne, 0, 3, true);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListTwo, 0, 5, true);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListTh, 0, 7, true);
                    //更新2 4 6 8 列
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListFour, 1, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListFive, 7, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListSix, 13, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListSeven, 17, 9, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return ;
            }


        }

        //产量日报数据
        public string GetProductDailyData(string startTime, string endTime, int lineid)
        {
        try
        {
                string comname = Global.GetCompanyNameByLineID(lineid);


            string[] strListTest = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };

            string strSql = "select * from (SELECT a.orderid,a.childid,b.ordern,a.childn,'" + comname + "' AS factoryName," +
                "productlineid,ordtime,DeliveryTime,ProductionT,Customer,ExpCountries,KRXTM,XTDH,Material," +
                "color,XingTiN,BaoTouL,WeiTiaoW,HuChiW,ProductionBeat,NowN,OldN,ALlN FROM `Usage` a " +
                "LEFT JOIN `Order` b ON a.orderid=b.orderid  where a.CT >='" + startTime + "' and a.CT<='" + endTime + "' and a.productlineid=" + lineid +
                " ORDER BY  a.CreateTime DESC  LIMIT 0,1)  T1 LEFT JOIN  (SELECT MAX(productlineid) AS productlineid , sum(NowN) as NowAN , sum(WeiTiaoConsumption) as WeiTiaoConsumption ," +
                "sum(HuChiConsumption) as HuChiConsumption ,sum(BiaoQianConsumption) as BiaoQianConsumption , sum(DaDiConsumption) as DaDiConsumption  FROM `Usage` a   where a.CT >='" + startTime + "' and a.CT<='" + endTime + "'" +
                " and a.productlineid=" + lineid + " ) T2 ON T1.productlineid=T2.productlineid";

                
                DataTable db = MySqlHelper.ExecuteQuery(strSql);
                 
                return JsonHelper.DataTableToJson(db);
             
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            return Global.RETURN_ERROR(ex.Message);
        }
        }



                //产量日报
     public void GetProductDailyReport(string strPath, string strServerPath,string startTime, string endTime, int lineid)
        {

        //复制Excel

        try {
            if (File.Exists(strPath))
            {
                    string comname = Global.GetCompanyNameByLineID(lineid);

                    File.Copy(strPath, strServerPath, true);

                string[] strListTest = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };

                    string strSql = "select * from (SELECT a.orderid,a.childid,b.ordern,a.childn,'"+ comname+"' AS factoryName," +
                     "productlineid,ordtime,DeliveryTime,ProductionT,Customer,ExpCountries,KRXTM,XTDH,Material," +
                     "color,XingTiN,BaoTouL,WeiTiaoW,HuChiW,ProductionBeat,NowN,OldN,ALlN FROM `Usage` a " +
                     "LEFT JOIN `Order` b ON a.orderid=b.orderid  where a.CT >='" + startTime + "' and a.CT<='" + endTime + "' and a.productlineid=" + lineid +
                     " ORDER BY  a.CreateTime DESC  LIMIT 0,1)  T1 LEFT JOIN  (SELECT MAX(productlineid) AS productlineid , sum(NowN) as NowAN , sum(WeiTiaoConsumption) as WeiTiaoConsumption ," +
                     "sum(HuChiConsumption) as HuChiConsumption ,sum(BiaoQianConsumption) as BiaoQianConsumption , sum(DaDiConsumption) as DaDiConsumption  FROM `Usage` a   where a.CT >='" + startTime + "' and a.CT<='" + endTime + "'" +
                     " and a.productlineid=" + lineid + " ) T2 ON T1.productlineid=T2.productlineid";
                    DataTable db = MySqlHelper.ExecuteQuery(strSql);



                if (db.Rows.Count > 0)
                {
                        //当班总产量
                        string strSql1 = "SELECT  sum(NowN) as NowAN  FROM `Usage` a   where a.CT >='" + startTime + "' and a.CT<='" + endTime + "' and a.productlineid=" + lineid;
                    
                        DataTable DB_ALL_DAY = MySqlHelper.ExecuteQuery(strSql1);

                        List<string> list = new List<string>();
                    List<string> listTwo = new List<string>();
                    List<string> listTh = new List<string>();
                    List<string> listFour = new List<string>();
                    List<string> listFive = new List<string>();
                    List<string> listSix = new List<string>();
                    List<string> listSeven = new List<string>();
                    for (int i = 0; i < 20; i++)
                    {
                        //初始化
                        list.Add("");
                        listTwo.Add("");
                        listTh.Add("");

                    }
                    for (int i = 0; i < 2; i++)
                    {
                        listFour.Add("");
                        listFive.Add("");
                        listSix.Add("");
                        listSeven.Add("");

                    }

                    //LIST
                            list[0] = db.Rows[0][0].ToString();
                        
                            list[1] = db.Rows[0][1].ToString();
                        
                            list[4] = db.Rows[0][2].ToString();
                        
                            list[7] = db.Rows[0][3].ToString();
                        
                            list[9] = db.Rows[0][4].ToString();
                        
                            list[13] = db.Rows[0][5].ToString();
                        
                            list[15] = db.Rows[0][6].ToString();
                        
                            list[17] = db.Rows[0][7].ToString();
                        
                            list[19] = db.Rows[0][8].ToString();
                         
                     
                            listTwo[0] = db.Rows[0][9].ToString();
                       
                            listTwo[1] = db.Rows[0][10].ToString();
                         
                            listTwo[4] = db.Rows[0][11].ToString();
                        
                            listTwo[7] = db.Rows[0][12].ToString();
                    
                            listTh[0] = db.Rows[0][13].ToString();
                        
                            listTh[1] = db.Rows[0][14].ToString();
                        
                            listTh[4] = db.Rows[0][15].ToString();
                         
                            listTh[7] = db.Rows[0][16].ToString();
                       
                            listTh[9] = db.Rows[0][17].ToString();
                        
                            listTh[13] = db.Rows[0][18].ToString();
                         
                            //生产节拍 当班 当日 总产量
                            listFour[0] = db.Rows[0][19].ToString();
                            listFive[0] = db.Rows[0][20].ToString();
                            listSix[0] = db.Rows[0][24].ToString();
                            listSeven[0] = db.Rows[0][22].ToString();

                        //4用量
                        listFour[1] = db.Rows[0][25].ToString();
                        listFive[1] = db.Rows[0][26].ToString();
                        listSix[1] = db.Rows[0][27].ToString();
                        listSeven[1] = db.Rows[0][28].ToString();

                        
                    string[] strListOne = list.ToArray();
                    string[] strListTwo = listTwo.ToArray();
                    string[] strListTh = listTh.ToArray();

                    string[] strListFour = listFour.ToArray();
                    string[] strListFive = listFive.ToArray();
                    string[] strListSix = listSix.ToArray();
                    string[] strListSeven = listSeven.ToArray();
                    //更新3 5 7 行
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListOne, 0, 3, true);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListTwo, 0, 5, true);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListTh, 0, 7, true);
                    //更新2 4 6 8 列
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListFour, 1, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListFive, 7, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListSix, 13, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListSeven, 17, 9, false);
                }
                //添加产量增长图
                String sql2 = "SELECT picstype, pics FROM getpics where getpicstime >='" + startTime + "' and getpicstime <='" + endTime + "'  limit 5 ";

                MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(sql2);

                byte[] buffer = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if ("产品增量图" == reader.GetString(0))
                        {
                            long len = reader.GetBytes(1, 0, null, 0, 0);//1是picture  
                            buffer = new byte[len];

                            len = reader.GetBytes(1, 0, buffer, 0, (int)len);

                            ExcelHelperByNPOI.UpdateImgToExcel(strServerPath, "日报表", buffer, 0, 11, 10, 25);
                        }
                        if ("用量统计图" == reader.GetString(0))
                        {
                            long len = reader.GetBytes(1, 0, null, 0, 0);//1是picture  
                            buffer = new byte[len];

                            len = reader.GetBytes(1, 0, buffer, 0, (int)len);

                            ExcelHelperByNPOI.UpdateImgToExcel(strServerPath, "日报表", buffer, 10, 11, 21, 25);
                        }
                    }

                }

               }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            return ;
        }

    }

        //产量日报数据
        public string GetQualityDailyData (  string startTime, string endTime, int lineid)
        {
         try
             {
                string comname = Global.GetCompanyNameByLineID(lineid);

                string strSql = "select * from (SELECT a.orderid,a.childid,b.ordern,a.childn,'" + comname + "' AS factoryName," +
                           "productlineid,ordtime,DeliveryTime,ProductionT,Customer,ExpCountries,KRXTM,XTDH,Material," +
                           "color,XingTiN,BaoTouL,WeiTiaoW,HuChiW,ProductionBeat,NowN,OldN,ALlN FROM `Usage` a " +
                           "LEFT JOIN `Order` b ON a.orderid=b.orderid  where a.CT >='" + startTime + "' and a.CT<='" + endTime + "' and a.productlineid=" + lineid +
                           " ORDER BY  a.CreateTime DESC  LIMIT 0,1)  T1 LEFT JOIN  (SELECT MAX(productlineid) AS productlineid , sum(NowN) as NowAN , sum(WeiTiaoConsumption) as WeiTiaoConsumption ," +
                           "sum(HuChiConsumption) as HuChiConsumption ,sum(BiaoQianConsumption) as BiaoQianConsumption , sum(DaDiConsumption) as DaDiConsumption  FROM `Usage` a   where a.CT >='" + startTime + "' and a.CT<='" + endTime + "'" +
                           " and a.productlineid=" + lineid + " ) T2 ON T1.productlineid=T2.productlineid";
                DataTable db = MySqlHelper.ExecuteQuery(strSql);



                //获取质量统计

                string strSqlqa = " SELECT  round(sum(t.`AppearanceQualified`) /sum(1) ) as AppearanceQualified, " +
                        "   round(sum(t.`AppearanceAfterQualified`) / sum(1)) as AppearanceAfterQualified," +
                        "   round(sum(t.`VampPullQualified`*100) / sum(1)) as VampPullQualified," +
                        "    round(sum(t.`DaDiPullQualified`*100) / sum(1)) as DaDiPullQualified," +
                        "    round(sum(t.`ZheWangQualified`) / sum(1)) as ZheWangQualified ," +
                        "         round(sum(t.`Qualified`) / sum(1)) as Qualified , group_concat( distinct TestT) as TestT , group_concat(distinct   tester) as tester" +
                        "    FROM(select a.* from Quality a left join `usage` b on a.id_usage = b.id_usage" +
                       "      where  b.CT >= '" + startTime + "' and b.CT <='" + endTime + "' and b.productlineid = " + lineid + ") as t";
        DataTable dbqa = MySqlHelper.ExecuteQuery(strSqlqa);
 
         Hashtable hasht = new Hashtable();

        if (db.Rows.Count > 0)
            hasht.Add("order", JsonHelper.DataRowToDic(db.Columns, db.Rows[0]));
        if (dbqa.Rows.Count > 0)
            hasht.Add("quality", JsonHelper.DataRowToDic(dbqa.Columns, dbqa.Rows[0]));
         
        
                    return JsonConvert.SerializeObject(hasht);
       
         }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
        return Global.RETURN_ERROR(ex.Message);
    }

}

        public string GetQualityDailyDetail(string startTime, string endTime, int lineid)
        {
        try { 
            string strSqlqadetail = " SELECT RFIDN,round( AppearanceQualified,1) as AppearanceQualified, VampPullMinimum, VampPullAverage,round( VampPullQualified*100,1) as  VampPullQualified, DaDiPullMinimum, " +
                                          "   DaDiPullAverage, round(DaDiPullQualified*100,1) as DaDiPullQualified, round(ZheWangQualified,1) as ZheWangQualified , round(AppearanceAfterQualified,1) as AppearanceAfterQualified  " +
                                          "    FROM  Quality a left join `usage` b on a.id_usage = b.id_usage " +
                                          "      where  b.CT >= '" + startTime + "' and b.CT <= '" + endTime + "' and b.productlineid = " + lineid + " order by RFIDN";
            DataTable qadetail = MySqlHelper.ExecuteQuery(strSqlqadetail);


             
                return JsonHelper.DataTableToJson(qadetail);
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            return Global.RETURN_ERROR(ex.Message);
        }
    }


        //质量日报xls
        public void GetQualityDailyReport(string strPath, string strServerPath, string startTime, string endTime, int lineid)
        {
        try
        {
            int imgRowstart = 19;
            int imgRowend = 33;
            //复制Excel
            if (File.Exists(strPath))
            {
                File.Copy(strPath, strServerPath, true);

                    string comname = Global.GetCompanyNameByLineID(lineid);

                    string[] strListTest = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };
                    //获取订单信息
                    string strSql = "select * from (SELECT a.orderid,a.childid,b.ordern,a.childn,'" + comname + "' AS factoryName," +
                              "productlineid,ordtime,DeliveryTime,ProductionT,Customer,ExpCountries,KRXTM,XTDH,Material," +
                              "color,XingTiN,BaoTouL,WeiTiaoW,HuChiW,ProductionBeat,NowN,OldN,ALlN FROM `Usage` a " +
                              "LEFT JOIN `Order` b ON a.orderid=b.orderid  where a.CT >='" + startTime + "' and a.CT<='" + endTime + "' and a.productlineid=" + lineid +
                              " ORDER BY  a.CreateTime DESC  LIMIT 0,1)  T1 LEFT JOIN  (SELECT MAX(productlineid) AS productlineid , sum(NowN) as NowAN , sum(WeiTiaoConsumption) as WeiTiaoConsumption ," +
                              "sum(HuChiConsumption) as HuChiConsumption ,sum(BiaoQianConsumption) as BiaoQianConsumption , sum(DaDiConsumption) as DaDiConsumption  FROM `Usage` a   where a.CT >='" + startTime + "' and a.CT<='" + endTime + "'" +
                              " and a.productlineid=" + lineid + " ) T2 ON T1.productlineid=T2.productlineid";
                    DataTable db = MySqlHelper.ExecuteQuery(strSql);


                    //获取质量统计

                    string strSqlqa = " SELECT  round(sum(t.`AppearanceQualified`) /sum(1) ) as AppearanceQualified, " +
                                "   round(sum(t.`AppearanceAfterQualified`) / sum(1)) as AppearanceAfterQualified," +
                                "   round(sum(t.`VampPullQualified`*100) / sum(1)) as VampPullQualified," +
                                "    round(sum(t.`DaDiPullQualified`*100) / sum(1)) as DaDiPullQualified," +
                                "    round(sum(t.`ZheWangQualified`) / sum(1)) as ZheWangQualified ," +
                                "         round(sum(t.`Qualified`) / sum(1)) as Qualified , group_concat( distinct TestT) as TestT , group_concat(distinct   tester) as tester" +
                                "    FROM(select a.* from Quality a left join `usage` b on a.id_usage = b.id_usage" +
                               "      where  b.CT >= '" + startTime + "' and b.CT <='" + endTime + "' and b.productlineid = " + lineid + ") as t";
                DataTable dbqa = MySqlHelper.ExecuteQuery(strSqlqa);
                if (db.Rows.Count > 0)
                {

                    List<string> list = new List<string>();
                    List<string> listTwo = new List<string>();
                    List<string> listTh = new List<string>();
                    List<string> listFour = new List<string>();
                    List<string> listFive = new List<string>();
                    List<string> listSix = new List<string>();
                    List<string> listSeven = new List<string>();
                    for (int i = 0; i < 20; i++)
                    {
                        //初始化
                        list.Add("");
                        listTwo.Add("");
                        listTh.Add("");

                    }
                    for (int i = 0; i < 2; i++)
                    {
                        listFour.Add("");
                        listFive.Add("");
                        listSix.Add("");
                        listSeven.Add("");

                    }
                        if (db.Rows.Count > 0)
                        {

                            //LIST
                            list[0] = db.Rows[0][0].ToString();

                            list[1] = db.Rows[0][1].ToString();

                            list[4] = db.Rows[0][2].ToString();

                            list[7] = db.Rows[0][3].ToString();

                            list[9] = db.Rows[0][4].ToString();

                            list[13] = db.Rows[0][5].ToString();

                            list[15] = db.Rows[0][6].ToString();

                            list[17] = db.Rows[0][7].ToString();

                            list[19] = db.Rows[0][8].ToString();


                            listTwo[0] = db.Rows[0][9].ToString();

                            listTwo[1] = db.Rows[0][10].ToString();

                            listTwo[4] = db.Rows[0][11].ToString();

                            listTwo[7] = db.Rows[0][12].ToString();

                            listTh[0] = db.Rows[0][13].ToString();

                            listTh[1] = db.Rows[0][14].ToString();

                            listTh[4] = db.Rows[0][15].ToString();

                            listTh[7] = db.Rows[0][16].ToString();

                            listTh[9] = db.Rows[0][17].ToString();

                            listTh[13] = db.Rows[0][18].ToString();
                        }
                        if (dbqa.Rows.Count > 0)
                    {

                        listFour[0] = dbqa.Rows[0][0].ToString();
                        listFour[1] = dbqa.Rows[0][2].ToString();
                        listFive[0] = dbqa.Rows[0][1].ToString();
                        listFive[1] = dbqa.Rows[0][3].ToString();
                        listSix[0] = dbqa.Rows[0][4].ToString();
                        listSix[1] = dbqa.Rows[0][5].ToString();
                        listSeven[0] = dbqa.Rows[0][6].ToString();
                        listSeven[1] = dbqa.Rows[0][7].ToString();
                    }


                    string[] strListOne = list.ToArray();
                    string[] strListTwo = listTwo.ToArray();
                    string[] strListTh = listTh.ToArray();

                    string[] strListFour = listFour.ToArray();
                    string[] strListFive = listFive.ToArray();
                    string[] strListSix = listSix.ToArray();
                    string[] strListSeven = listSeven.ToArray();
                    //更新3 5 7 行
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListOne, 0, 3, true);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListTwo, 0, 5, true);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListTh, 0, 7, true);
                    //更新2 4 6 8 列
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListFour, 1, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListFive, 7, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListSix, 13, 9, false);
                    ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", strListSeven, 17, 9, false);
                }

                //获取质量统计

                string strSqlqadetail = " SELECT RFIDN,round( AppearanceQualified,1), VampPullMinimum, VampPullAverage,round( VampPullQualified*100,1), DaDiPullMinimum, " +
                                "   DaDiPullAverage, round(DaDiPullQualified*100,1), round(ZheWangQualified,1), round(AppearanceAfterQualified,1)  " +
                                "    FROM  Quality a left join `usage` b on a.id_usage = b.id_usage " +
                                "      where  b.CT >= '" + startTime + "' and b.CT <= '" + endTime + "' and b.productlineid = " + lineid;
                DataTable qadetail = MySqlHelper.ExecuteQuery(strSqlqadetail);

                if (qadetail.Rows.Count > 0)
                {
                    ExcelHelperByNPOI.InsertRow(strServerPath, "日报表", 17, (qadetail.Rows.Count - 1) * 5);
                    imgRowstart = imgRowstart + (qadetail.Rows.Count - 1) * 5;
                    imgRowend = imgRowend + (qadetail.Rows.Count - 1) * 5;


                    for (int num = 0; num < qadetail.Rows.Count; num++)
                    {
                        List<string> listdetail1 = new List<string>();
                        List<string> listdetail2 = new List<string>();
                        List<string> listdetail3 = new List<string>();
                        List<string> listdetail4 = new List<string>();
                        List<string> listdetail5 = new List<string>();
                        for (int i = 0; i < 20; i++)
                        {
                            listdetail1.Add(""); listdetail2.Add(""); listdetail3.Add(""); listdetail4.Add(""); listdetail5.Add("");

                        }
                        listdetail1[0] = qadetail.Rows[num][0].ToString();
                        listdetail1[1] = "硫化前外观合格率";
                        listdetail1[7] = "";
                        listdetail1[9] = "";
                        listdetail1[13] = qadetail.Rows[num][1].ToString();
                        listdetail1[15] = double.Parse(qadetail.Rows[num][1].ToString()) >= 100 ? "√" : "×";
                        listdetail2[0] = qadetail.Rows[num][0].ToString();
                        listdetail2[1] = "鞋面拉力合格点";
                        listdetail2[7] = qadetail.Rows[num][2].ToString();
                        listdetail2[9] = qadetail.Rows[num][3].ToString();
                        listdetail2[13] = qadetail.Rows[num][4].ToString();
                        listdetail2[15] = double.Parse(qadetail.Rows[num][4].ToString()) >= 100 ? "√" : "×";

                        listdetail3[0] = qadetail.Rows[num][0].ToString();
                        listdetail3[1] = "大底拉力合格点";
                        listdetail3[7] = qadetail.Rows[num][5].ToString();
                        listdetail3[9] = qadetail.Rows[num][6].ToString();
                        listdetail3[13] = qadetail.Rows[num][7].ToString();
                        listdetail3[15] = double.Parse(qadetail.Rows[num][7].ToString()) >= 100 ? "√" : "×";

                        listdetail4[0] = qadetail.Rows[num][0].ToString();
                        listdetail4[1] = "折弯疲劳合格率";
                        listdetail4[7] = "";
                        listdetail4[9] = "";
                        listdetail4[13] = qadetail.Rows[num][8].ToString();
                        listdetail4[15] = double.Parse(qadetail.Rows[num][8].ToString()) >= 100 ? "√" : "×";

                        listdetail5[0] = qadetail.Rows[num][0].ToString();

                        listdetail5[1] = "硫化前后观合格率";
                        listdetail5[7] = "";
                        listdetail5[9] = "";
                        listdetail5[13] = qadetail.Rows[num][9].ToString();
                        listdetail5[15] = double.Parse(qadetail.Rows[num][9].ToString()) >= 100 ? "√" : "×";


                        ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", listdetail1.ToArray(), 0, 13 + num * 5, true);
                        ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", listdetail2.ToArray(), 0, 14 + num * 5, true);
                        ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", listdetail3.ToArray(), 0, 15 + num * 5, true);
                        ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", listdetail4.ToArray(), 0, 16 + num * 5, true);
                        ExcelHelperByNPOI.UpdateExcel(strServerPath, "日报表", listdetail5.ToArray(), 0, 17 + num * 5, true);

                    }

                }



                    //添加质量分析图
                    String sql2 = "SELECT picstype, pics FROM getpics where getpicstime >='" + startTime + "' and getpicstime <='" + endTime + "'   limit 5 ";

                MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(sql2);

                byte[] buffer = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if ("质量分析图" == reader.GetString(0))
                        {
                            long len = reader.GetBytes(1, 0, null, 0, 0);//1是picture  
                            buffer = new byte[len];

                            len = reader.GetBytes(1, 0, buffer, 0, (int)len);

                            ExcelHelperByNPOI.UpdateImgToExcel(strServerPath, "日报表", buffer, 6, imgRowstart, 17, imgRowend);
                        }

                    }
                }




            }
        }  
    catch (Exception ex)
    {
        logger.Error(ex.Message);
        return ;
    }

        }


        public string LineDailyRpt(string begintime, string lineids )
        {
            
            string RSTSTR = string.Empty;
            try
            {
                string querysql = "select 1 as Days, t.* ,round(runt*100/if(allt>0,allt,1),2) as runrate, " +
                  "  round(runt, 2) as RunT, round(stopt, 2) as StopT,round(warnt, 2) as WarnT,round(offlinet, 2) as OffLineT,"+
                   "round(stopt * 100 /if (allt > 0,allt,1),2) as stoprate,  " +
                   "round(warnt * 100 /if (allt > 0,allt,1),2)  as warnrate, " +
                   " round(offlinet * 100 /if (allt > 0,allt,1),2) as offlinerate, " +
                  " round((WorkLoad * planpoweronT)*100 / (planworkload  * (runt + warnt)),2) as OEE ,"+
                   " round((runt) * 100 /if (planpoweronT > 0,planpoweronT,1),2) as  ACT, " +
                   " round((runt+warnt) * 100 /if (planpoweronT > 0,planpoweronT,1),2) as  DACT ," +
                       " round((runt ) * 100 /if ((runt+warnt) > 0,(runt+warnt),1),2) as  TACT  " +
                   "  from( select  a.* , a.runt + a.stopt + a.warnt + offlinet as allt, b.productlinename from rptproductday a left " +
                   "  join productlineinfo b on a.productlineid = b.productlineid  where a.productionT='" + begintime + "' and a.productlineid in (" + lineids+ ")) t";

                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }

        public string LineMonthRpt(string begintime, string lineids)
        {

            string RSTSTR = string.Empty;
            try
            {
                string querysql = "select t.* , "+
                " round(runt , 2) as RunT,round(stopt , 2) as StopT,round(warnt, 2) as WarnT,round(offlinet , 2) as OffLineT, " +
                "  round(runt * 100 /if (allt > 0,allt,1),2)  as runrate, " +
                "  round(stopt * 100 /if (allt > 0,allt,1),2) as stoprate, " +
                "  round(warnt * 100 /if (allt > 0,allt,1),2)  as warnrate, " +
                "  round(offlinet * 100 /if (allt > 0,allt,1),2)  as offlinerate, " +
                   " round((WorkLoad * planpoweronT)*100 / (planworkload  * (runt + warnt)),2) as OEE ," +
            " round((runt) * 100 /if (planpoweronT > 0,planpoweronT,1),2) as  ACT, " +
                   " round((runt+warnt) * 100 /if (planpoweronT > 0,planpoweronT,1),2) as  DACT ," +
                       " round((runt ) * 100 /if ((runt+warnt) > 0,(runt+warnt),1),2) as  TACT  " +

                "  from( select  a.*, a.runt + a.stopt + a.warnt + offlinet as allt, b.productlinename from rptproductmonth a left  join " +
                " productlineinfo b on a.productlineid = b.productlineid where a.productionT='" + begintime + "' and a.productlineid in (" + lineids + ")) t ";

                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }

        public string LinePhaseRpt(string begintime, string endtime, string lineids)
        {
            string tm = begintime + "-" + endtime;
            string RSTSTR = string.Empty;
            try
            {
                string querysql = "   select t.* ,  '"+tm+ "' as ProductionT,  " +
               "   round(runt , 2) as RunT,round(stopt, 2) as StopT,round(warnt , 2) as WarnT,round(offlinet , 2) as OffLineT,  " +
               "    round(runt * 100 /if (allt > 0,allt,1),2)  as runrate,   " +
               "    round(stopt * 100 /if (allt > 0,allt,1),2)  as stoprate,  " +
               "    round(warnt * 100 /if (allt > 0,allt,1),2)  as warnrate,   " +
               "    round(offlinet * 100 /if (allt > 0,allt,1),2)  as offlinerate,  " +
                  " round((WorkLoad * planpoweronT)*100 / (planworkload  * (runt + warnt)),2) as OEE ," +
             " round((runt) * 100 /if (planpoweronT > 0,planpoweronT,1),2) as  ACT, " +
                   " round((runt+warnt) * 100 /if (planpoweronT > 0,planpoweronT,1),2) as  DACT ," +
                       " round((runt ) * 100 /if ((runt+warnt) > 0,(runt+warnt),1),2) as  TACT  " +

               "   from(select count(id) as Days, sum(runt) as RunT, sum(RunC) as RunC, sum(StopT) as StopT, sum(StopC) as StopC, sum(WarnT) as WarnT, sum(WarnC) as WarnC, sum(OffLineT) as OffLineT, sum(OffLineC) as OffLineC,  " +
               "   sum(PlanPowerOnT) as PlanPowerOnT, sum(PowerOnT) as PowerOnT, sum(PowerOffT) as PowerOffT, sum(IsPlanPowerOn) as IsPlanPowerOn, sum(PlanShifts) as PlanShifts, sum(Shifts) as Shifts, sum(PlanWorkLoad) as PlanWorkLoad, sum(WorkLoad) as WorkLoad  " +
               "   ,a.ProductLineId, productlinename, sum(a.runt + a.stopt + a.warnt + offlinet) as allt from rptproductday a left    join  " +
               "  productlineinfo b on a.productlineid = b.productlineid where a.productionT >= '"+begintime+ "' and productionT <= '" + endtime + "' and  a.productlineid in (" + lineids + ") group by a.ProductLineId, productlinename) t ";



               DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }
        public string LineYearRpt(string begintime, string lineids)
        {

            string RSTSTR = string.Empty;
            try
            {
                string querysql = "select t.* , " +
                " round(runt , 2) as RunT,round(stopt , 2) as StopT,round(warnt , 2) as WarnT,round(offlinet, 2) as OffLineT, " +
                "  round(runt * 100 /if (allt > 0,allt,1),2) as runrate, " +
                "  round(stopt * 100 /if (allt > 0,allt,1),2) as stoprate, " +
                "  round(warnt * 100 /if (allt > 0,allt,1),2) as warnrate, " +
                "  round(offlinet * 100 /if (allt > 0,allt,1),2) as offlinerate, " +
                   " round((WorkLoad * planpoweronT)*100 / (planworkload  * (runt + warnt)),2) as OEE ," +
               " round((runt) * 100 /if (planpoweronT > 0,planpoweronT,1),2) as  ACT, " +
                   " round((runt+warnt) * 100 /if (planpoweronT > 0,planpoweronT,1),2) as  DACT ," +
                       " round((runt ) * 100 /if ((runt+warnt) > 0,(runt+warnt),1),2) as  TACT  " +

                "  from( select  a.*, a.runt + a.stopt + a.warnt + offlinet as allt, b.productlinename from rptproductyear a left  join " +
                " productlineinfo b on a.productlineid = b.productlineid  where a.productionT='" + begintime + "' and a.productlineid in (" + lineids + ")) t";

                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }

        public string DeviceDailyRpt(string begintime, string lineids,string devicemodel)
        {

            string RSTSTR = string.Empty;
            try
            {
                string querysql =
                    " select 1 as Days, tt1.DeviceId, tt1.DeviceName,tt1.DeviceModel,tt1.ProductionT,tt1.PlanPowerOnT, tt1.RunT,tt1.FreeT,Round(tt1.RunT * 3600 / tt1.RunC) as AvgRunT,  " +
           " tt1.WarnT,tt1.WarnC, tt2.AvgWarnT,tt2.AvgWarnInter,tt2.FirstWarnInter  " +
          " from(select a.*, b.productlinename, c.devicename, c.devicemodel, (a.runt + a.freet + a.warnt) as allt from rptdeviceday a  " +
          "  left join productlineinfo b on a.productlineid = b.productlineid  left   join deviceinfo c on a.deviceid = c.deviceid  " +
             "     where a.productionT = '"+begintime+"' and a.productlineid in ('" + lineids + "') ) tt1  left join  "+
             "        rptdeviceerrday tt2 on tt1.productionT = tt2.productionT and tt1.DeviceId = tt2.DeviceId  ";
                  if (devicemodel != null)
                    querysql += " and tt1.devicemodel = '" + devicemodel + "'";

                querysql += "  order by tt1.DeviceId";
                   DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }


        public string DevicePhaseRpt(string begintime, string endtime, string lineids, string devicemodel)
        {
            string tm = begintime + "-" + endtime;
            string RSTSTR = string.Empty;
            try
            {
                string querysql =
                       "  select DeviceId,DeviceName,DeviceModel ,'"+ tm + "' as ProductionT, sum(PlanPowerOnT) as PlanPowerOnT,sum(RunT) as RunT,sum(FreeT) as FreeT,  "+
                     "    Round(sum(RunT) * 3600 / sum(RunC)) as AvgRunT,sum(WarnT) as WarnT,sum(WarnC) as WarnC,  " +
                       "   Round(sum(WarnT) * 3600 / sum(WarnC)) as AvgWarnT, round(sum(AvgWarnInter) / sum(Days)) as AvgWarnInter, " +
                       "   round(sum(FirstWarnInter) / sum(Days)) as FirstWarnInter , " +
                       "  sum(Days) as Days from( " +
                       "   select 1 as Days, tt1.DeviceId, tt1.DeviceName, tt1.DeviceModel, tt1.ProductionT, tt1.PlanPowerOnT, tt1.RunT, tt1.FreeT, tt1.RunC, " +
                        "     tt1.WarnT, tt1.WarnC, tt2.AvgWarnT, tt2.AvgWarnInter, tt2.FirstWarnInter   from(select a.*, b.productlinename, " +
                        "      c.devicename, c.devicemodel, (a.runt + a.freet + a.warnt) as allt from rptdeviceday a    left " +
                        "  join productlineinfo b on a.productlineid = b.productlineid  left " +
                    "     join deviceinfo c on a.deviceid = c.deviceid       where a.productionT >= '" + begintime + "' and a.productionT <= '" + endtime + "'and a.productlineid in ('" + lineids+"')) tt1  left join " +
                    "  rptdeviceerrday tt2 on tt1.productionT = tt2.productionT and tt1.DeviceId = tt2.DeviceId    order by tt1.DeviceId " +
                       "  ) t ";
              if (devicemodel != null)
                    querysql += "  where devicemodel = '" + devicemodel + "'";

                querysql += " group by DeviceId, DeviceName, DeviceModel  order by DeviceId";
                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }

        public string DeviceMonthRpt(string begintime, string lineids, string devicemodel)
        {

            string RSTSTR = string.Empty;
            try
            {
                string querysql =
                  " select tt1.Days, tt1.DeviceId, tt1.DeviceName,tt1.DeviceModel,tt1.ProductionT,tt1.PlanPowerOnT, tt1.RunT,tt1.FreeT,Round(tt1.RunT * 3600 / tt1.RunC) as AvgRunT,  " +
         " tt1.WarnT,tt1.WarnC, tt2.AvgWarnT,tt2.AvgWarnInter,tt2.FirstWarnInter  " +
        " from(select a.*, b.productlinename, c.devicename, c.devicemodel, (a.runt + a.freet + a.warnt) as allt from rptdevicemonth a  " +
        "  left join productlineinfo b on a.productlineid = b.productlineid  left   join deviceinfo c on a.deviceid = c.deviceid  " +
           "     where a.productionT = '" + begintime + "' and a.productlineid in ('" + lineids + "') ) tt1  left join  " +
           "        rptdeviceerrmonth tt2 on tt1.productionT = tt2.productionT and tt1.DeviceId = tt2.DeviceId  ";
                if (devicemodel != null)
                    querysql += " and tt1.devicemodel = '" + devicemodel + "'";

                querysql += "  order by tt1.DeviceId";
                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }
        public string DeviceYearRpt(string begintime, string lineids, string devicemodel)
        {

            string RSTSTR = string.Empty;
            try
            {
                string querysql =
                   " select tt1.Days, tt1.DeviceId, tt1.DeviceName,tt1.DeviceModel,tt1.ProductionT,tt1.PlanPowerOnT, tt1.RunT,tt1.FreeT,Round(tt1.RunT * 3600 / tt1.RunC) as AvgRunT,  " +
          " tt1.WarnT,tt1.WarnC, tt2.AvgWarnT,tt2.AvgWarnInter,tt2.FirstWarnInter  " +
         " from(select a.*, b.productlinename, c.devicename, c.devicemodel, (a.runt + a.freet + a.warnt) as allt from rptdeviceyear a  " +
         "  left join productlineinfo b on a.productlineid = b.productlineid  left   join deviceinfo c on a.deviceid = c.deviceid  " +
            "     where a.productionT = '" + begintime + "' and a.productlineid in ('" + lineids + "') ) tt1  left join  " +
            "        rptdeviceerryear tt2 on tt1.productionT = tt2.productionT and tt1.DeviceId = tt2.DeviceId  ";
                if (devicemodel != null)
                    querysql += " and tt1.devicemodel = '" + devicemodel + "'";

                querysql += "  order by tt1.DeviceId";
                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }


        public string DeviceErrDailyRpt(string begintime, string lineids, string devicemodel,int   islisterr= 1)
        {

            string RSTSTR = string.Empty;
            try
            {
                string querysql = string.Empty;
                if(islisterr==1)
                { 
                  querysql = "select a.*, b.productlinename ,c.devicename,c.devicemodel,c.stationName,d.paraname as errname  from rptdeviceerrday a left  join productlineinfo b on a.productlineid = b.productlineid  " +
                 " left join(select t.deviceid, t.devicename, t.devicemodel, t1.stationName from  deviceinfo t left join  " +
                 "locationcfg t1 on t.productlineid= t1.productlineid and t.locationid= t1.locationid)  c on a.deviceid = c.deviceid "+
                 " left join parainfo d on   a.errorn = d.paraid and d.paratype='ERRORN' " +
                 " where a.productionT ='" + begintime + "' and a.productlineid in ('" + lineids + "')";
                if (devicemodel != null)
                    querysql += " and c.devicemodel = '" + devicemodel + "'";

                querysql += "  order by productlineid,deviceid,errorn";
                }
                else
                {
                      querysql = "select a.*, b.productlinename ,c.devicename,c.devicemodel,c.stationName ,'全部' as errname from(  " +
                    "  select deviceid, productiont, productlineid,'ALL' as ERRORN ,sum(warnt) as warnt,sum(warnC) as warnC   from rptdeviceerrday   " +
                    "  where productionT = '" + begintime + "' and productlineid in ('" + lineids + "') group by deviceid , productionT,productlineid) " +
                   "  a left  join productlineinfo b on a.productlineid = b.productlineid  " +
                   "  left join(select t.deviceid, t.devicename, t.devicemodel, t1.stationName from  deviceinfo t left join  locationcfg t1 on" +
                   " t.productlineid= t1.productlineid and t.locationid= t1.locationid)  c on a.deviceid = c.deviceid ";
                     if (devicemodel != null)
                        querysql += " where c.devicemodel = '" + devicemodel + "'";

                    querysql += "    order by productlineid,deviceid";



                }
                   


                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }
        public string DeviceErrPhaseRpt(string begintime, string endtime, string lineids, string devicemodel, int islisterr = 1)
        {

            string RSTSTR = string.Empty;
            try
            {
                string querysql = string.Empty;
                if (islisterr == 1)
                {
                      querysql = " select '"+begintime+"-"+ endtime +"' as ProductionT, round(warnt/days,2) as  Davgwarnt,round(warnc/days,2) as  Davgwarnc,a.*, b.productlinename ,c.devicename,c.devicemodel,c.stationName from  " +
                         " (select deviceid, productlineid, ERRORN, sum(warnt) as warnt, sum(warnC) as warnC, count(id) as days  from rptdeviceerrday    where productionT = '2018-06-25' and  productlineid in ('1') group by deviceid," +
                        " productlineid, ERRORN) a left  join productlineinfo b on a.productlineid = b.productlineid "+
                   "    left join(select t.deviceid, t.devicename, t.devicemodel, t1.stationName from  deviceinfo t left join  locationcfg t1 on t.productlineid= t1.productlineid and t.locationid= t1.locationid)  c on a.deviceid = c.deviceid " +
                   "   left join parainfo d on a.errorn = d.paraid and d.paratype = 'ERRORN'  ";
                     if (devicemodel != null)
                        querysql += " where c.devicemodel = '" + devicemodel + "'";

                    querysql += "   order by productlineid,deviceid,errorn";
                }
                else
                {
                      querysql = "select  '" + begintime + "-" + endtime + "' as ProductionT, round(warnt/days,2) as  Davgwarnt,round(warnc/days,2) as  Davgwarnc,a.*, b.productlinename ,c.devicename,c.devicemodel,c.stationName ,'全部' as errname from(  " +
                    "  select deviceid,  productlineid,'ALL' as ERRORN ,sum(warnt) as warnt,sum(warnC) as warnC ,count(id) as days  from rptdeviceerrday   " +
                    "  where productionT = '" + begintime + "' and productlineid in ('" + lineids + "') group by deviceid ,productlineid) " +
                   "  a left  join productlineinfo b on a.productlineid = b.productlineid  " +
                   "  left join(select t.deviceid, t.devicename, t.devicemodel, t1.stationName from  deviceinfo t left join  locationcfg t1 on" +
                   " t.productlineid= t1.productlineid and t.locationid= t1.locationid)  c on a.deviceid = c.deviceid ";
                    if (devicemodel != null)
                        querysql += " and c.devicemodel = '" + devicemodel + "'";

                    querysql += "   order by productlineid,deviceid";



                }



                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }
        public string DeviceErrMonthRpt(string begintime, string lineids, string devicemodel, int islisterr = 1)
        {

            string RSTSTR = string.Empty;
            try
            {
                 string querysql = string.Empty;
                if (islisterr == 1)
                {
                      querysql = "select   round(warnt/days,2) as  Davgwarnt,round(warnc/days,2) as  Davgwarnc, a.*, b.productlinename ,c.devicename,c.devicemodel,c.stationName,d.paraname as errname  from rptdeviceerrmonth a left  join productlineinfo b on a.productlineid = b.productlineid  " +
                     " left join(select t.deviceid, t.devicename, t.devicemodel, t1.stationName from  deviceinfo t left join  " +
                     "locationcfg t1 on t.productlineid= t1.productlineid and t.locationid= t1.locationid)  c on a.deviceid = c.deviceid " +
                     " left join parainfo d on   a.errorn = d.paraid and d.paratype='ERRORN' " +
                     " where a.productionT ='" + begintime + "' and a.productlineid in ('" + lineids + "')";
                    if (devicemodel != null)
                        querysql += " and c.devicemodel = '" + devicemodel + "'";

                    querysql += "  order by productlineid,deviceid,errorn";
                }
                else
                {
                      querysql = "select   round(warnt/days,2) as  Davgwarnt,round(warnc/days,2) as  Davgwarnc, a.*, b.productlinename ,c.devicename,c.devicemodel,c.stationName ,'全部' as errname from(  " +
                    "  select deviceid, productiont, productlineid,'ALL' as ERRORN ,sum(warnt) as warnt,sum(warnC) as warnC ,max(days) as days  from rptdeviceerrmonth   " +
                    "  where productionT = '" + begintime + "' and productlineid in ('" + lineids + "') group by deviceid , productionT,productlineid) " +
                   "  a left  join productlineinfo b on a.productlineid = b.productlineid  " +
                   "  left join(select t.deviceid, t.devicename, t.devicemodel, t1.stationName from  deviceinfo t left join  locationcfg t1 on" +
                   " t.productlineid= t1.productlineid and t.locationid= t1.locationid)  c on a.deviceid = c.deviceid ";
                    if (devicemodel != null)
                        querysql += " where c.devicemodel = '" + devicemodel + "'";

                    querysql += "   order by productlineid,deviceid";



                }



                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }
        public string DeviceErrYearRpt(string begintime, string lineids, string devicemodel, int islisterr = 1)
        {

            string RSTSTR = string.Empty;
            try
            {
                string querysql = string.Empty;
                if (islisterr == 1)
                {
                      querysql = "select   round(warnt/days,2) as  Davgwarnt,round(warnc/days,2) as  Davgwarnc, a.*, b.productlinename ,c.devicename,c.devicemodel,c.stationName,d.paraname as errname  from rptdeviceerryear a left  join productlineinfo b on a.productlineid = b.productlineid  " +
                     " left join(select t.deviceid, t.devicename, t.devicemodel, t1.stationName from  deviceinfo t left join  " +
                     "locationcfg t1 on t.productlineid= t1.productlineid and t.locationid= t1.locationid)  c on a.deviceid = c.deviceid " +
                     " left join parainfo d on   a.errorn = d.paraid and d.paratype='ERRORN' " +
                     " where a.productionT ='" + begintime + "' and a.productlineid in ('" + lineids + "')";
                    if (devicemodel != null)
                        querysql += " and c.devicemodel = '" + devicemodel + "'";

                    querysql += "  order by productlineid,deviceid,errorn";
                }
                else
                {
                      querysql = "select   round(warnt/days,2) as  Davgwarnt,round(warnc/days,2) as  Davgwarnc, a.*, b.productlinename ,c.devicename,c.devicemodel,c.stationName ,'全部' as errname from(  " +
                    "  select deviceid, productiont, productlineid,'ALL' as ERRORN ,sum(warnt) as warnt,sum(warnC) as warnC,max(days) as days,max(months) as months   from rptdeviceerryear   " +
                    "  where productionT = '" + begintime + "' and productlineid in ('" + lineids + "') group by deviceid , productionT,productlineid) " +
                   "  a left  join productlineinfo b on a.productlineid = b.productlineid  " +
                   "  left join(select t.deviceid, t.devicename, t.devicemodel, t1.stationName from  deviceinfo t left join  locationcfg t1 on" +
                   " t.productlineid= t1.productlineid and t.locationid= t1.locationid)  c on a.deviceid = c.deviceid ";
                    if (devicemodel != null)
                        querysql += " where c.devicemodel = '" + devicemodel + "'";

                    querysql += "   order by productlineid,deviceid";



                }



                DataTable dt = MySqlHelper.ExecuteQuery(querysql);

                RSTSTR = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                return Global.RETURN_ERROR(ex.Message);
            }
            return RSTSTR;
        }
    }
}
