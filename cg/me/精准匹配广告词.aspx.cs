using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 精准匹配广告词 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                    dtru.Columns.Add("Clicks");
                    dtru.Columns.Add("IMPRESSION");
                    dtru.Columns.Add("CTR");
                    dtru.Columns.Add("path");
                    dtru.Columns.Add("User_Name");
                    dtru.Columns.Add("Shop_Name");
                    dtru.Columns.Add("Shop_ID");
                    dtru.Columns.Add("Product_Name_Ad_Name");
                    dtru.Columns.Add("DpName");
                    dtru.Columns.Add("GroupName");


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
                        DataSet dsxc = access_sql.GreatDs("select [Search_Query] from YN_AD_CSV where BrowserID='" + bid + "'");
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

                                    getjg(dtcsv, csvpath, pid, bid, dsxc);//处理csv，找出符合条件的数据

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




                            lits.Text = "店铺：" + dtdp.Rows.Count + "个，文件数：" + wjs + "，读取去重复后有pid和关键词" + dtru.Rows.Count + "个";

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
                    string cccstr = "";
                    for (int i = 0; i < dtru.Rows.Count; i++)
                    {
                        DataRow dr = dtru.Rows[i];
                        string pid = dr["Product_ID"].ToString();
                        string BrowserID = dr["BrowserID"].ToString();
                        string Search_Query = dr["Search_Query"].ToString().Replace("'", "''");
                        string Clicks = dr["Clicks"].ToString();
                        string CTR = dr["CTR"].ToString();
                        string IMPRESSION = dr["IMPRESSION"].ToString();
                        string User_Name = dr["User_Name"].ToString();
                        string Shop_Name = dr["Shop_Name"].ToString();
                        string Shop_ID = dr["Shop_ID"].ToString();
                        string Product_Name_Ad_Name = dr["Product_Name_Ad_Name"].ToString();
                        string path = dr["path"].ToString();
                        string DpName = dr["DpName"].ToString();
                        string GroupName = dr["GroupName"].ToString();
                        string uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");

                        if (CTR != "∞")
                        {
                            //cccstr += pid + "|" + CTR + "</br>";
                            if (access_sql.GetOneValue("select count(*) from YN_AD_CSV where Product_ID='" + pid + "' and Search_Query='" + Search_Query + "'") == "0")
                            {

                                string[] strs = new string[] { "BrowserID", "DpName", "GroupName", "User_Name", "Shop_Name", "Shop_ID", "Product_Name_Ad_Name", "Product_ID", "Search_Query", "Clicks", "IMPRESSION", "CTR", "uploadTime", "path" };
                                string[] objs = new string[] { BrowserID, DpName, GroupName, User_Name, Shop_Name, Shop_ID, Product_Name_Ad_Name, pid, Search_Query, Clicks, IMPRESSION, CTR, uploadTime, path };
                                if (access_sql.T_Insert_ExecSql(strs, objs, "YN_AD_CSV") > 0)
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
                                cccstr += pid + Search_Query;
                                cf++;
                            }
                        }

                    }
                    Response.Write(cccstr);
                    lits.Text = "成功" + cg + "个，重复" + cf + ",失败" + sb + ",文本" + cccstr;
                }
            }
        }
        public void getjg(DataTable dtcsv, string path, string pid, string bid, DataSet dsxc)
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
                    bool ky = false;
                    if (access_sql.yzTable(dsxc))
                    {
                        if (dsxc.Tables[0].Select("[Search_Query]='" + sq.Replace("'", "''") + "'").Length == 0)
                        {
                            ky = true;

                        }
                    }
                    else
                    {
                        ky = true;
                    }
                    if (ky)
                    {
                        DataRow[] drsbysq = dtcsv.Select("[Search Query]='" + sq.Replace("'", "''") + "'");
                        float Clicks = 0;
                        float IMPRESSION = 0;
                        float CTR = 0;
                        foreach (DataRow drsq in drsbysq)
                        {
                            Clicks += float.Parse(drsq["Clicks"].ToString());
                            IMPRESSION += float.Parse(drsq["IMPRESSION"].ToString());

                        }


                        CTR = Clicks / IMPRESSION;

                        if (Clicks >= 3 && CTR >= 0.05)
                        {
                            dtru.Rows.Add(new object[] { pid, bid, sq, Clicks, IMPRESSION, CTR, path, User_Name, Shop_Name, Shop_ID, Product_Name_Ad_Name });
                        }
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
        public DataTable RemoveDuplicatesByColumn(DataTable dataTable, string pid, string bid)
        {
            DataTable dtru = new DataTable();
            dtru.Columns.Add("pid");
            dtru.Columns.Add("bid");
            dtru.Columns.Add("Search Query");
            dtru.Columns.Add("Clicks");
            dtru.Columns.Add("CTR");
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string sq = dataTable.Rows[i]["Search Query"].ToString();
                if (dtru.Select("[Search Query]='" + sq.Replace("'", "''") + "'").Length == 0)
                {
                    dtru.Rows.Add(new object[] { pid, bid, sq, "", "" });
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
                string code = e.CommandArgument.ToString();

                TextBox txtsjbm = e.Item.FindControl("txtsjbm") as TextBox;


                if (txtsjbm.Text.Trim() != "")
                {


                    if (access_sql.T_Update_ExecSql(new string[] { "shangjiabianma" }, new object[] { txtsjbm.Text.Trim().Replace("'", "''") }, "YNBigData", "code='" + code + "'") > 0)
                    {

                        lits.Text = "code:" + code + "更新成功";
                    }
                }
                else
                {
                    lits.Text = "商家编码不能为空";
                    Response.Write("<script>alert('商家编码不能为空');</script>");
                }
            }
        }

        public void clzy()
        {
            int cg = 0;
            bool ky = true;

            for (int i = 0; i < rplb.Items.Count; i++)
            {

                TextBox txtsjbm = rplb.Items[i].FindControl("txtsjbm") as TextBox;
                if (txtsjbm.Text == "")
                {
                    ky = false;
                    lits.Text = "第" + (i + 1) + "商家编码不能为空";
                    Response.Write("<script>alert('第" + (i + 1) + "商家编码不能为空');</script>");
                    break;
                }



            }
            if (ky)
            {
                for (int i = 0; i < rplb.Items.Count; i++)
                {
                    Literal licode = (Literal)rplb.Items[i].FindControl("licode");
                    string code = licode.Text;
                    TextBox txtsjbm = rplb.Items[i].FindControl("txtsjbm") as TextBox;
                    cg += access_sql.T_Update_ExecSql(new string[] { "shangjiabianma" }, new object[] { txtsjbm.Text.Trim().Replace("'", "''") }, "YNBigData", "code='" + code + "'");
                }
                lits.Text = "更新成功" + cg + "个";
            }



        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            clzy();
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
            rplb.DataSource = null;
            rplb.DataBind();
            string where = "";
            string order = "";
            Literal1.Text = "";
            lits.Text = "";
            if (drtype.SelectedValue == "0")
            {
                where = " is_del=0 ";
                lits.Text += "在用精准关键词,";
            }
            else
            {
                where = " is_del=1 ";
                lits.Text += "停用精准关键词,";
            }

            if (drporder.SelectedValue == "0")
            {
                order = "  order by CTR desc ";
                lits.Text += "点击率高-低";
            }
            else
            {
                order = " order by CTR ";
                lits.Text += "点击率低-高";
            }
            DataSet dsall = access_sql.GreatDs("select * from YN_AD_CSV where" + where + order);
            if (access_sql.yzTable(dsall))
            {
                rplb.DataSource = dsall.Tables[0];
                rplb.DataBind();
                Literal1.Text = "加载到数据" + dsall.Tables[0].Rows.Count + "条";
            }
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            lits.Text = "";
            Literal2.Text = "";
            if (txtProduct_ID.Text.Trim() != "" && ckall.Checked)
            {
                lits.Text = "不能同时选择两种模式";
            }
            else
            {
                if (txtProduct_ID.Text.Trim() != "" || ckall.Checked)
                {
                    string where = "";
                    if (!ckall.Checked)
                    {
                        where = " (ProductID='" + txtProduct_ID.Text.Trim().Replace("'", "''") + "') and ({0}) and SearchCount>" + txtSearchCount.Text.Trim().Replace("'", "''") + " and isadd=0";
                    }
                    else
                    {
                        where = "  1=1 and ({0}) and SearchCount>" + txtSearchCount.Text.Trim().Replace("'", "''") + " and isadd=0";
                    }

                    string ckkk = "";

                    if (ck3.Checked)
                    {
                        ckkk += "or ( WordNum=3) ";
                    }
                    if (ck4.Checked)
                    {
                        ckkk += "or ( WordNum=4) ";
                    }
                    if (ck5.Checked)
                    {
                        ckkk += "or ( WordNum=5) ";
                    }
                    if (ck6.Checked)
                    {
                        ckkk += "or ( WordNum=6) ";
                    }
                    if (ck7.Checked)
                    {
                        ckkk += "or ( WordNum=7) ";
                    }
                    if (ckkk != "")
                    {
                        ckkk = ckkk.Substring(2, ckkk.Length - 2);
                        where = "where " + where.Replace("{0}", ckkk);
                    }
                    else
                    {
                        where = "where " + where.Replace("and ({0})", "");
                    }

                    string sql = "select id,ProductID,KW,WordNum,SearchCount from ADTitKW " + where;
                    DataSet dsall = access_sql.GreatDs(sql);
                    if (access_sql.yzTable(dsall))
                    {

                        if (!ckall.Checked)
                        {
                            Session["dttemp"] = dsall.Tables[0];
                            Literal2.Text = "加载到符合条件的数据" + dsall.Tables[0].Rows.Count + "条";
                        }
                        else
                        {
                            string sqltjcount = "select ProductID from ADTitKW " + where + " group by ProductID";
                            DataSet Dstjcount = access_sql.GreatDs(sqltjcount);
                            if (access_sql.yzTable(Dstjcount))
                            {
                                Session["dttemp"] = dsall.Tables[0];
                                Session["dttjcount"] = Dstjcount.Tables[0];
                                Literal2.Text = "加载到Product_ID：(" + Dstjcount.Tables[0].Rows.Count + ")个，数据:(" + dsall.Tables[0].Rows.Count + "条)";
                            }
                        }
                    }
                    else
                    {
                        Literal2.Text = "无数据";
                    }
                }

                else
                {
                    lits.Text = "请输入Product_ID，或者选择所有Product_ID";
                }
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            DataTable dt = access_sql.GreatDs("select Product_ID from YN_AD_CSV where DpName is null group by Product_ID").Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string Product_ID = dt.Rows[i]["Product_ID"].ToString();
                DataSet ds = access_sql.GreatDs("select bid,bname from campaign where product_id='" + Product_ID + "'");
                if (access_sql.yzTable(ds))
                {
                    string BrowserID = ds.Tables[0].Rows[0]["bid"].ToString();
                    string DpName = ds.Tables[0].Rows[0]["bname"].ToString();
                    access_sql.T_Update_ExecSql(new string[] { "BrowserID", "DpName" }, new object[] { BrowserID, DpName }, "YN_AD_CSV", "Product_ID='" + Product_ID + "' and DpName is null ");

                }
            }
            if (!ckall.Checked)
            {
                if (Session["dttemp"] != null)
                {
                    DataTable dttemp = (DataTable)Session["dttemp"];
                    int cg = 0;
                    int cf = 0;
                    for (int i = 0; i < dttemp.Rows.Count; i++)
                    {
                        DataRow dr = dttemp.Rows[i];
                        string id = dr["id"].ToString();
                        string ProductID = dr["ProductID"].ToString();
                        string KW = dr["KW"].ToString().Replace("'", "''");
                        string WordNum = dr["WordNum"].ToString();
                        string SearchCount = dr["SearchCount"].ToString();

                        try
                        {


                            if (access_sql.T_Insert_ExecSql(new string[] { "Product_ID", "Search_Query", "Search_Volume", "WordNum" }, new object[] { ProductID, KW, SearchCount, WordNum }, "YN_AD_CSV") != 0)
                            {
                                cg++;
                                access_sql.DoSql("update ADTitKW set isadd=1 where id=" + id + "");
                            }
                        }
                        catch
                        {

                            cf++;
                        }

                    }
                    Literal2.Text = "添加成功" + cg + "条，重复" + cf + "条";
                }
            }
            else
            {
                if (Session["dttemp"] != null && Session["dttjcount"] != null)
                {
                    List<CCCCC> ccc = new List<CCCCC>();
                    DataTable dttemp = (DataTable)Session["dttemp"];
                    DataTable dttjcount = (DataTable)Session["dttjcount"];
                    int cg = 0;
                    int cf = 0;
                    int cgpid = 0;
                    for (int i = 0; i < dttjcount.Rows.Count; i++)
                    {
                        string ProductID = dttjcount.Rows[i]["ProductID"].ToString();
                        DataRow[] drsbypid = dttemp.Select("ProductID='" + ProductID + "'");

                        string BrowserID = "";
                        string DpName = "";
                        DataSet ds = access_sql.GreatDs("select bid,bname from campaign where product_id='" + ProductID + "'");
                        if (access_sql.yzTable(ds))
                        {
                            BrowserID = ds.Tables[0].Rows[0]["bid"].ToString();
                            DpName = ds.Tables[0].Rows[0]["bname"].ToString();

                        }

                        for (int q = 0; q < drsbypid.Length; q++)
                        {
                            DataRow dr = drsbypid[q];
                            string id = dr["id"].ToString();
                            string KW = dr["KW"].ToString().Replace("'", "''");
                            string WordNum = dr["WordNum"].ToString();
                            string SearchCount = dr["SearchCount"].ToString();



                            CCCCC c = new CCCCC();
                            c.Product_ID = ProductID;
                            c.Search_Query = KW;
                            c.Search_Volume = int.Parse(SearchCount);
                            c.WordNum = WordNum;
                            c.BrowserID = BrowserID;
                            c.DpName = DpName;
                            c.id = int.Parse(id);
                            ccc.Add(c);


                        }

                    }
                    int insertCount = 0;
                    int SBCount = 0;
                    using (SqlConnection connection = new SqlConnection(connstring))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {


                            try
                            {
                                string sql = "IF NOT EXISTS (SELECT 1 FROM YN_AD_CSV WHERE Product_ID=@Product_ID AND Search_Query=@Search_Query) " + "INSERT INTO YN_AD_CSV (Product_ID,Search_Query,Search_Volume,WordNum,BrowserID,DpName) VALUES (@Product_ID,@Search_Query,@Search_Volume,@WordNum,@BrowserID,@DpName)";

                                List<int> idList = new List<int>(); // 假设id是整数类型
                                List<int> idList_sb = new List<int>(); // 假设id是整数类型
                                using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                                {
                                    // 添加参数
                                    command.Parameters.Add("@Product_ID", SqlDbType.NVarChar);
                                    command.Parameters.Add("@Search_Query", SqlDbType.NVarChar);
                                    command.Parameters.Add("@Search_Volume", SqlDbType.Int);
                                    command.Parameters.Add("@WordNum", SqlDbType.NVarChar);
                                    command.Parameters.Add("@BrowserID", SqlDbType.NVarChar);
                                    command.Parameters.Add("@DpName", SqlDbType.NVarChar);

                                    // 循环插入数据
                                    foreach (var c in ccc)
                                    {
                                        command.Parameters["@Product_ID"].Value = c.Product_ID;
                                        command.Parameters["@Search_Query"].Value = c.Search_Query.Replace("'", "''");
                                        command.Parameters["@Search_Volume"].Value = c.Search_Volume;
                                        command.Parameters["@WordNum"].Value = c.WordNum;
                                        command.Parameters["@BrowserID"].Value = c.BrowserID;
                                        command.Parameters["@DpName"].Value = c.DpName.Replace("'", "''");

                                        int rowsAffected = command.ExecuteNonQuery(); // 执行插入
                                        if (rowsAffected > 0)
                                        {
                                            insertCount++;          // 仅当插入成功时计数
                                            idList.Add(c.id);
                                        }
                                        else
                                        {
                                            SBCount++;
                                            idList_sb.Add(c.id);
                                        }

                                    }
                                }

                                if (idList.Count > 0)
                                {
                                    var sqlUpdate = "UPDATE ADTitKW SET isadd = 1 WHERE id IN ({0})";
                                    var parameters = idList.Select((id, index) => $"@id{index}").ToArray();
                                    sqlUpdate = string.Format(sqlUpdate, string.Join(",", parameters));

                                    using (SqlCommand updateCmd = new SqlCommand(sqlUpdate, connection, transaction))
                                    {
                                        for (int i = 0; i < parameters.Length; i++)
                                        {
                                            updateCmd.Parameters.AddWithValue(parameters[i], idList[i]);
                                        }
                                        updateCmd.ExecuteNonQuery();
                                    }
                                }
                                if (idList_sb.Count > 0)
                                {
                                    var sqlUpdate = "UPDATE ADTitKW SET isadd = 1 WHERE id IN ({0})";
                                    var parameters = idList_sb.Select((id, index) => $"@id{index}").ToArray();
                                    sqlUpdate = string.Format(sqlUpdate, string.Join(",", parameters));

                                    using (SqlCommand updateCmd = new SqlCommand(sqlUpdate, connection, transaction))
                                    {
                                        for (int i = 0; i < parameters.Length; i++)
                                        {
                                            updateCmd.Parameters.AddWithValue(parameters[i], idList_sb[i]);
                                        }
                                        updateCmd.ExecuteNonQuery();
                                    }
                                }
                                transaction.Commit(); // 提交事务

                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    transaction.Rollback();
                                    lits.Text = "操作失败，已回滚事务。" + ex.ToString();
                                }
                                catch (Exception rollbackEx)
                                {
                                    lits.Text = "回滚失败。" + rollbackEx.ToString();
                                }
                            }

                        }
                    }

                    Literal2.Text = "完成，新增数据" + insertCount + "条，重复数据" + SBCount + "条";

                }
            }
        }
        public static readonly string connstring = ConfigurationSettings.AppSettings["SQLConnectionString"];
        protected void Button6_Click(object sender, EventArgs e)
        {


        }
    }
    public class CCCCC
    {
        public string Product_ID { get; set; }
        public string Search_Query { get; set; }
        public int Search_Volume { get; set; }
        public string WordNum { get; set; }
        public string BrowserID { get; set; }
        public string DpName { get; set; }
        public int id { get; set; }
    }
}