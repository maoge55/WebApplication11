using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 出单词 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                Session.Timeout = 240;
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public int dpok = 0;
        public static DataTable dtdp = null;
        public static DataTable dtru = null;

        public string jjj = "";
        public string yyy = "";
        public void bindzhy()
        {
            jjj = "";
            yyy = "";
            dtdp = new DataTable();
            rplb.DataSource = null;
            rplb.DataBind();
            string tdpath = Server.MapPath("/ShopeeAD/详情报表_") + DateTime.Now.ToString("yyyy_MM_dd");
            //for (int i = -30; i < -1; i++)
            //{
            //    string depath = Server.MapPath("/ShopeeAD/详情报表_") + DateTime.Now.AddDays(i).ToString("yyyy_MM_dd");
            //    if (Directory.Exists(depath))
            //    {
            //        Directory.Delete(depath, true);
            //    }
            //}


            if (Directory.Exists(tdpath))   //如果当日文件夹存在
            {
                DataTable dtczdp = access_sql.GreatDs("select BrowserID from houtai where isBBAdInfo=1").Tables[0];
                dtczdp.Columns.Add("jj");
                dtczdp.Columns.Add("yy");
                dtdp.Clear();
                dtdp.Columns.Add("bid");
                dtdp.Columns.Add("path");

                string[] alldp = Directory.GetDirectories(tdpath);    //提取所有店铺文件夹路径

                for (int i = 0; i < alldp.Length; i++)    //判断每个店铺是否采集完成，是否有个1.txt
                {
                    string path = alldp[i];
                    string bid = path.Split('\\')[path.Split('\\').Length - 1];
                    dtdp.Rows.Add(new object[] { bid, path });
                    DataRow[] drrrrr = dtczdp.Select("BrowserID='" + bid + "'");
                    if (drrrrr.Length != 0)
                    {
                        drrrrr[0]["jj"] = 1;
                    }
                }



                if (dtdp != null && dtdp.Rows.Count > 0)
                {



                    rplb.DataSource = null;
                    rplb.DataBind();



                    dtru = new DataTable();
                    dtru.Clear();
                    dtru.Columns.Add("Product_ID");
                    dtru.Columns.Add("BrowserID");
                    dtru.Columns.Add("Search_Query");
                    dtru.Columns.Add("Conversions");
                    dtru.Columns.Add("ROAS");
                    dtru.Columns.Add("path");
                    dtru.Columns.Add("User_Name");
                    dtru.Columns.Add("Shop_Name");
                    dtru.Columns.Add("Shop_ID");
                    dtru.Columns.Add("Product_Name_Ad_Name");
                    dtru.Columns.Add("DpName");
                    dtru.Columns.Add("GroupName");
                    dtru.Columns.Add("search_count");
                    dtru.Columns.Add("mid");
                    dtru.Columns.Add("leimuwangzhi");
                    dtru.Columns.Add("leimuxuanpinriqi");
                    dtru.Columns.Add("sumccc");



                    int wjs = 0;


                    for (int i = 0; i < dtdp.Rows.Count; i++)       //循环所有已经完成的店铺，提取店铺的所有pid文件
                    {
                        string path = dtdp.Rows[i][1].ToString();
                        string bid = dtdp.Rows[i][0].ToString();
                        if (File.Exists(path + "\\1.txt"))
                        {
                            DataRow[] drrrrr = dtczdp.Select("BrowserID='" + bid + "'");
                            if (drrrrr.Length != 0)
                            {
                                drrrrr[0]["yy"] = 1;
                            }
                        }
                       // DataSet dsxc = access_sql.GreatDs("select [Search_Query] from YN_AD_CSV where BrowserID='" + bid + "'");
                        string[] allcsv = Directory.GetFiles(path, "*.csv");
                        wjs += allcsv.Length;
                        for (int c = 0; c < allcsv.Length; c++)   //循环所有csv
                        {
                            string csvpath = allcsv[c];

                            string pid = csvpath.Split('\\')[csvpath.Split('\\').Length - 1];
                            pid = pid.Split('.')[0];

                            DataTable dtcsv = getdatbycsv(csvpath);   //从csv里读取所有数据


                            if (dtcsv != null)
                            {
                                if (dtcsv.Rows.Count > 0)
                                {

                                    getjg(dtcsv, csvpath, pid, bid);//处理csv，找出符合条件的数据

                                }
                            }


                        }

                    }

                    for (int i = 0; i < dtczdp.Rows.Count; i++)
                    {
                        DataRow dr = dtczdp.Rows[i];
                        if (dr["jj"].ToString() != "1")
                        {
                            jjj += "【" + dr[0].ToString() + "】，";
                        }
                        if (dr["yy"].ToString() != "1")
                        {
                            yyy += "【" + dr[0].ToString() + "】，";
                        }
                    }
                    if (dtru != null)
                    {
                        if (dtru.Rows.Count > 0)
                        {
                            DataTable dtddbid = RemoveDuplicatesByColumn(dtru, "BrowserID");
                            DataSet dsdpxx = access_sql.GreatDs("select * from houtai");
                            for (int i = 0; i < dtddbid.Rows.Count; i++)
                            {
                                string DpName = "";
                                string GroupName = "";
                                string BrowserID = dtddbid.Rows[i]["BrowserID"].ToString();

                                DataRow[] drdddddd = dsdpxx.Tables[0].Select("BrowserID='" + BrowserID + "'");
                                if (drdddddd.Length > 0)
                                {
                                    DpName = drdddddd[0]["DpName"].ToString();
                                    GroupName = drdddddd[0]["GroupName"].ToString();
                                    DataRow[] drs = dtru.Select("BrowserID='" + BrowserID + "'");
                                    foreach (DataRow item in drs)
                                    {
                                        item["DpName"] = DpName;
                                        item["GroupName"] = GroupName;
                                    }
                                }

                            }




                            lits.Text = "店铺：" + dtdp.Rows.Count + "个，文件数：" + wjs + "，符合条件关键词" + dtru.Rows.Count + "个";

                            rplb.DataSource = dtru;
                            rplb.DataBind();
                        }
                        else
                        {
                            lits.Text = "店铺：" + dtdp.Rows.Count + "个，文件数：" + wjs + "，无新数据";
                        }

                    }
                    else
                    {
                        lits.Text = "店铺：" + dtdp.Rows.Count + "个，文件数：" + wjs + "，无新数据";
                    }

                }
                else
                {
                    lits.Text = "无店铺可用";
                }
            }
            else
            {
                lits.Text = "今日还没数据";
            }
            if (jjj != "")
            {
                lits.Text += "<br>未找到文档的店铺有" + jjj;
            }
            if (yyy != "")
            {
                lits.Text += "<br>没全部下载成功的店铺有" + yyy;
            }
            if (jjj != "" || yyy != "")
            {
                access_sql.DoSql("update Task set [state]=1 where id=17");
            }

        }
        protected void Button2_Click2(object sender, EventArgs e)
        {

            if (dtru != null)
            {
                if (dtru.Rows.Count > 0)
                {
                    int cg = 0;
                    int cf = 0;
                    int sb = 0;
                    int up = 0;
                    int noup = 0;
                    string cccstr = "";
                    for (int i = 0; i < dtru.Rows.Count; i++)
                    {
                        DataRow dr = dtru.Rows[i];
                        string pid = dr["Product_ID"].ToString();
                        string BrowserID = dr["BrowserID"].ToString();
                        string Search_Query = dr["Search_Query"].ToString().Replace("'", "''");
                        string Conversions = dr["Conversions"].ToString();
                        string Roas = dr["Roas"].ToString();
                        string User_Name = dr["User_Name"].ToString();
                        string Shop_Name = dr["Shop_Name"].ToString();
                        string Shop_ID = dr["Shop_ID"].ToString();
                        string Product_Name_Ad_Name = dr["Product_Name_Ad_Name"].ToString();
                        string path = dr["path"].ToString();
                        string DpName = dr["DpName"].ToString();
                        string GroupName = dr["GroupName"].ToString();
                        string uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");

                        DataSet dsold = access_sql.GreatDs("select * from YN_AD_CDC where Product_ID='" + pid + "' and Search_Query='" + Search_Query + "'");
                        if (!access_sql.yzTable(dsold))
                        {

                            string[] strs = new string[] { "BrowserID", "DpName", "GroupName", "User_Name", "Shop_Name", "Shop_ID", "Product_Name_Ad_Name", "Product_ID", "Search_Query", "Conversions", "Roas", "uploadTime", "path" };
                            string[] objs = new string[] { BrowserID, DpName, GroupName, User_Name, Shop_Name, Shop_ID, Product_Name_Ad_Name, pid, Search_Query, Conversions, Roas, uploadTime, path };
                            if (access_sql.T_Insert_ExecSql(strs, objs, "YN_AD_CDC") > 0)
                            {
                                cg++;
                            }
                            else
                            {
                                sb++;
                            }

                        }
                        else
                        {
                            if (Conversions != dsold.Tables[0].Rows[0]["Conversions"].ToString() || Roas != dsold.Tables[0].Rows[0]["Roas"].ToString())
                            {
                                string id = dsold.Tables[0].Rows[0]["id"].ToString();
                                if (access_sql.T_Update_ExecSql(new string[] { "Conversions", "Roas", "updateTime" }, new object[] { Conversions, Roas, uploadTime }, "YN_AD_CDC", "id=" + id) > 0)
                                {
                                    up++;
                                }
                            }
                            else
                            {
                                noup++;
                            }

                        }

                    }
                    lits.Text = "新增" + cg + "个，更新" + up + ",失败" + sb + ",没有变化" + noup;
                }
            }
        }
        public void getjg(DataTable dtcsv, string path, string pid, string bid)
        {
            DataTable dttemp = RemoveDuplicatesByColumn(dtcsv, "Search Query");

            if (dttemp != null && dttemp.Rows.Count > 0)
            {
                string User_Name = "";
                string Shop_Name = "";
                string Shop_ID = "";
                string Product_Name_Ad_Name = "";
                string[] xx = getXXbycsv(path).Split(new char[] { '镍' }, StringSplitOptions.RemoveEmptyEntries);
                if (xx.Length == 4)
                {
                    User_Name = xx[0].Trim();
                    Shop_Name = xx[1].Trim();
                    Shop_ID = xx[2].Trim();
                    Product_Name_Ad_Name = xx[3].Trim();
                }
                foreach (DataRow item in dttemp.Rows)
                {
                    string sq = item["Search Query"].ToString().Trim();

                    DataRow[] drsbysq = dtcsv.Select("[Search Query]='" + sq.Replace("'", "''") + "'");

                    int Conversions = 0;
                    float GMV = 0;
                    float Expense = 0;
                    foreach (DataRow drsq in drsbysq)
                    {

                        Conversions += int.Parse(drsq["Conversions"].ToString());
                        GMV += int.Parse(drsq["GMV"].ToString());
                        Expense += int.Parse(drsq["Expense"].ToString());
                    }
                    if (Conversions > 0)
                    {
                        string ROAS = "0";
                        if (Expense != 0)
                        {
                            ROAS = access_sql.getnum22222((GMV / Expense).ToString()).ToString();
                        }
                        dtru.Rows.Add(new object[] { pid, bid, sq, Conversions, ROAS, path, User_Name, Shop_Name, Shop_ID, Product_Name_Ad_Name });
                    }

                }
            }
        }
        public string getXXbycsv(string path)
        {
            string ru = "";
            List<string[]> liru = new CSVReader().Reader(path);
            int cc = 0;
            for (int i = 0; i < liru.Count; i++)
            {
                string[] aaa = liru[i];
                if (aaa[0] == "User Name")
                {
                    ru += aaa[1].Trim() + "镍";
                    cc++;
                }
                if (aaa[0] == "Shop Name")
                {
                    ru += aaa[1].Trim() + "镍";
                    cc++;
                }
                if (aaa[0] == "Shop ID")
                {
                    ru += aaa[1].Trim() + "镍";
                    cc++;
                }
                if (aaa[0] == "Product Name/Ad Name")
                {
                    ru += aaa[1].Trim() + "镍";
                    cc++;
                }
                if (cc == 4)
                {
                    break;
                }
            }

            return ru;
        }

        public DataTable getdatbycsv(string path)
        {
            DataTable dtcsv = new DataTable();
            bool ishead = false;
            bool isnr = false;

            List<string[]> liru = new CSVReader().Reader(path);
            for (int i = 0; i < liru.Count; i++)
            {
                if (isnr)
                {
                    if (liru[i][0] != "" && liru[i].Length == dtcsv.Columns.Count)
                    {
                        dtcsv.Rows.Add(liru[i].ToArray());
                    }
                    else
                    {
                        isnr = false;
                        break;
                    }
                }
                if (liru[i][0] == "Sequence")
                {
                    ishead = true;
                }
                if (ishead)
                {
                    foreach (var vv in liru[i])
                    {
                        dtcsv.Columns.Add(vv);
                    }
                    ishead = false;
                    isnr = true;
                }
            }
            if (dtcsv.Rows.Count > 0)
            {
                dtcsv.Rows.RemoveAt(0);
            }

            return dtcsv;
        }

        public DataTable RemoveDuplicatesByColumn(DataTable dataTable, string columnName)
        {
            var distinctRows = dataTable.AsEnumerable()
                                         .GroupBy(row => row.Field<object>(columnName))
                                         .Select(group => group.First())
                                         .CopyToDataTable();

            return distinctRows;
        }
        public DataTable RemoveDuplicatesByColumn(DataTable dataTable)
        {
            DataTable dtru = new DataTable();

            dtru.Columns.Add("Search Query");

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string sq = dataTable.Rows[i]["Search Query"].ToString();
                if (dtru.Select("[Search Query]='" + sq.Replace("'", "''") + "'").Length == 0)
                {
                    dtru.Rows.Add(new object[] { sq });
                }
            }
            return dtru;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            bindzhy();
        }


        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "up")
            {
                lits.Text = "";
                string Product_ID = e.CommandArgument.ToString();

                TextBox txtmbid = e.Item.FindControl("txtmbid") as TextBox;
                TextBox txtleimuwangzhi = e.Item.FindControl("txtleimuwangzhi") as TextBox;
                TextBox txtleimuxuanpinriqi = e.Item.FindControl("txtleimuxuanpinriqi") as TextBox;

                //if (txtmbid.Text.Trim() != "")
                //{

                int cg = access_sql.T_Update_ExecSql(new string[] { "mid", "leimuwangzhi", "leimuxuanpinriqi" }, new object[] { txtmbid.Text.Trim().Replace("'", "''"), txtleimuwangzhi.Text.Trim().Replace("'", "''"), txtleimuxuanpinriqi.Text.Trim().Replace("'", "''") }, "YN_AD_CDC", "Product_ID='" + Product_ID + "'");
                if (cg > 0)
                {

                    lits.Text = cg + "个更新成功";
                }
                //}
                //else
                //{
                //    lits.Text = "模板id不能为空";
                //    Response.Write("<script>alert('" + lits.Text + "');</script>");
                //}
            }
        }




        protected void Button4_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click2(object sender, EventArgs e)
        {
            List<string[]> ru = new CSVReader().Reader("E:\\repos\\WebApplication11\\WebApplication11\\ShopeeAD\\详情报表_2025_01_18\\kmf6ahe\\29101183584.csv");
        }

        protected void Button3_Click3(object sender, EventArgs e)
        {
            bind();
        }
        public void bind()
        {
            rplb.DataSource = null;
            rplb.DataBind();
            string where = "";
            string order = "";
            string sql = "";
            if (ckymbid.Checked)
            {
                where = " where (mid is null or mid='')";
            }
            if (dporder.SelectedValue != "onepid")
            {
                order = " order by " + dporder.SelectedValue + " desc";
                sql = "select *,'-' as sumccc from YN_AD_CDC" + where + order;
            }
            else
            {
                sql = "select * from YN_AD_CDC as yy  JOIN(select top 10000000  Product_ID,SUM(Conversions) as sumccc from YN_AD_CDC group by Product_ID order by sumccc desc) op on yy.Product_ID=op.Product_ID " + where + " order by op.sumccc DESC";
            }




            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                rplb.DataSource = ds.Tables[0];
                rplb.DataBind();
                lits.Text = "加载出单词" + ds.Tables[0].Rows.Count + "个";
            }
            else
            {
                lits.Text = "无数据";
            }
        }
        protected void Button4_Click1(object sender, EventArgs e)
        {
            int xz = 0;
            int up = 0;

            DataTable dt1 = access_sql.GreatDs("select id,Product_ID,Search_Query,Conversions from YN_AD_CDC").Tables[0];
            DataTable dt2 = access_sql.GreatDs("select id,Product_ID,Search_Query,Conversions from ttttt").Tables[0];

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                string Search_Query = dt2.Rows[i]["Search_Query"].ToString().Replace("'", "''").Trim();
                string Product_ID = dt2.Rows[i]["Product_ID"].ToString();
                int Conversions = int.Parse(dt2.Rows[i]["Conversions"].ToString());




                DataRow[] dt1sssss = dt1.Select("Product_ID='" + Product_ID + "' and Search_Query='" + Search_Query + "'");
                if (dt1sssss.Length == 0)
                {
                    access_sql.T_Insert_ExecSql(new string[] { "Product_ID", "Search_Query", "Conversions" }, new object[] { Product_ID, Search_Query, Conversions }, "YN_AD_CDC");
                }
                else
                {
                    int oldccc = int.Parse(dt1sssss[0]["Conversions"].ToString());
                    if (Conversions > oldccc)
                    {
                        access_sql.T_Update_ExecSql(new string[] { "Conversions" }, new object[] { Conversions }, "YN_AD_CDC", "id=" + dt1sssss[0]["id"]);
                    }
                }


            }





        }

        protected void Button4_Click2(object sender, EventArgs e)
        {
            int cg = 0;
            for (int i = 0; i < rplb.Items.Count; i++)
            {

                TextBox txtmbid = rplb.Items[i].FindControl("txtmbid") as TextBox;
                Literal lipid = rplb.Items[i].FindControl("lipid") as Literal;
                if (txtmbid.Text != "")
                {
                    cg += access_sql.T_Update_ExecSql(new string[] { "mid" }, new object[] { txtmbid.Text.Trim().Replace("'", "''") }, "YN_AD_CDC", "Product_ID='" + lipid.Text + "'");

                }



            }
            if (cg > 0)
            {

                lits.Text = cg + "个更新成功";
                bind();
            }
            else
            {
                lits.Text = "模板id不能为空";
                Response.Write("<script>alert('" + lits.Text + "');</script>");
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            DataTable dt = access_sql.GreatDs("SELECT Product_ID FROM YN_AD_CDC where Product_Name_Ad_Name is null group by Product_ID").Tables[0];


            string pidsssss = "";


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string Product_ID = dt.Rows[i][0].ToString().Trim();

                pidsssss += "'" + Product_ID + "'" + ",";


            }
            if (pidsssss != "")
            {
                pidsssss = pidsssss.Substring(0, pidsssss.Length - 1);
            }

            DataTable name1 = access_sql.GreatDs("select Product_ID,min(Product_Name_Ad_Name) from YN_AD_Info where Product_ID in (" + pidsssss + ") group by Product_ID").Tables[0];
            DataTable name2 = access_sql.GreatDs("select Product_ID,min(Product_Name_Ad_Name) from YN_AD_Info2 where Product_ID in (" + pidsssss + ") group by Product_ID").Tables[0];



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name = "";
                string Product_ID = dt.Rows[i][0].ToString().Trim();
                DataRow[] drn1 = name1.Select("Product_ID='" + Product_ID + "'");
                DataRow[] drn2 = name2.Select("Product_ID='" + Product_ID + "'");
                if (drn2.Length > 0)
                {
                    name = drn2[0][1].ToString();
                }
                else if (drn1.Length > 0 && name == "")
                {
                    name = drn1[0][1].ToString();
                }
                if (name != "")
                {
                    access_sql.T_Update_ExecSql(new string[] { "Product_Name_Ad_Name" }, new object[] { name.Replace("'", "''") }, "YN_AD_CDC", "Product_ID='" + Product_ID + "'");
                }

            }





            string q = "";
        }
    }
}