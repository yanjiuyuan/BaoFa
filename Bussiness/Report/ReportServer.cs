using Common.DbHelper;
using Common.Excel;
using Common.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Report
{
    public class ReportServer
    {
        public void GetDailyReport(string strPath, string strServerPath)
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
                    "LEFT JOIN `Order` b ON a.orderid=b.orderid    LEFT JOIN  " +
                    " `Spray` c ON a.orderid = c.orderid ORDER BY  CreateTime DESC LIMIT 0,1";
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
    }
}
