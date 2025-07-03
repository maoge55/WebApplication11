using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 管理广告测试产品 : System.Web.UI.Page
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
                    Session.Timeout = 640;
                    u = HttpContext.Current.Request.Cookies["cu"].Value;
                    p = HttpContext.Current.Request.Cookies["cp"].Value;
                    uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                    if (uid != "12" && uid != "9" && uid != "13" && uid != "10"&& uid != "6")
                    {
                        Response.Redirect("/cg/clogin.aspx");
                    }
                    if (Request.QueryString["txtsjbm"] != null && Request.QueryString["txtsjbm"] != "")
                    {
                        bindzhy();
                        DataTable dtbd = (DataTable)Session["dtall"];
                        count = dtbd.Rows.Count;
                        pg = count / 10;
                        if (pg == 0) { pg = 1; }
                        if (count % pg > 0) { pg = pg + 1; }



                        int star = 1;
                        int end = 10;
                        int pageindex = 1;
                        if (Request.QueryString["page"] != null && Request.QueryString["page"] != "")
                        {
                            pageindex = int.Parse(Request.QueryString["page"]);

                        }

                        lify.Text = "当前<span style='color:red'>" + pageindex + "</span>共" + pg + "页    分页：";
                        for (int i = 1; i <= pg; i++)
                        {
                            lify.Text += "<a href='?txtsjbm=" + Request.QueryString["txtsjbm"] + "&rdno=" + Request.QueryString["rdno"] + "&page=" + i + "'>" + i + "</a>&nbsp;&nbsp;";
                        }
                        lify2.Text = lify.Text;
                        lits.Text = "加载数据" + count + "共有" + pg + "页。当前页面" + pageindex + ",商家编码" + Request.QueryString["txtsjbm"] + "，" + (Request.QueryString["rdno"] == "no" ? "无入仓链接" : "有入仓链接");
                        star = (pageindex - 1) * 10 + 1;
                        end = 10 * pageindex;

                        DataTable dtls = dtbd.Clone();
                        dtls.Clear();
                        for (int i = star - 1; i <= end - 1; i++)
                        {
                            if (dtbd.Rows.Count > i)
                            {
                                dtls.Rows.Add(dtbd.Rows[i].ItemArray);
                            }
                        }
                        rplb.DataSource = dtls;
                        rplb.DataBind();
                    }
                    if (Request.QueryString["txtcode"] != null && Request.QueryString["txtcode"] != "")
                    {
                        bindzhy_code();
                        lits.Text = "搜索随机码：" + Request.QueryString["txtcode"];
                    }
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";


        public void bindzhy_code()
        {
            string sql = "";
            rplb.DataSource = null;
            rplb.DataBind();
            string where = "";
            bool kycx = true;
            if (Request.QueryString["txtcode"] != null && Request.QueryString["txtcode"] != "")
            {
                where += " code='" + Request.QueryString["txtcode"].Trim().Replace("'", "''") + "'";


            }





            string sqler = "select shangjiabianma,id,Purl,NewPrice_shopeeid,rucanglianjie,dianpubeizhu,itemid,MainImage,code,Title,Y_1688url,ercipuhuoURL,kefadehaiyun,ROW_NUMBER() OVER(PARTITION BY code ORDER BY code) AS rn from YNBigData where " + where + " order by code";


            DataSet dser = access_sql.GreatDs(sqler);
            if (access_sql.yzTable(dser))
            {
                DataTable dter = dser.Tables[0];
                DataRow[] codeList = dter.Select("rn=1");
                DataTable dtbd = new DataTable();

                dtbd.Columns.Add("MainImage");
                dtbd.Columns.Add("code");
                dtbd.Columns.Add("Title");
                dtbd.Columns.Add("Y_1688url");
                dtbd.Columns.Add("ercipuhuoURL");
                dtbd.Columns.Add("kefadehaiyun");
                dtbd.Columns.Add("Purl");
                dtbd.Columns.Add("NewPrice_shopeeid");
                dtbd.Columns.Add("rucanglianjie");
                dtbd.Columns.Add("dianpubeizhu");

                dtbd.Columns.Add("shangjiabianma");


                for (int i = 0; i < codeList.Length; i++)
                {
                    string code = codeList[i]["code"].ToString();
                    string MainImage = "";
                    string Title = "";
                    string Y_1688url = "";

                    string ercipuhuoURL = "";
                    string kefadehaiyun = "";
                    string Purl = "";
                    string NewPrice_shopeeid = "";
                    string rucanglianjie = "";
                    string dianpubeizhu = "";
                    string shangjiabianma = "";

                    DataRow[] allrowsbycode = dter.Select("code='" + code + "'");

                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["ercipuhuoURL"].ToString() != "" && dr["ercipuhuoURL"].ToString() != "0" && dr["ercipuhuoURL"].ToString().ToLower() != "null")
                        {
                            ercipuhuoURL = dr["ercipuhuoURL"].ToString();
                            break;
                        }
                    }
                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["Y_1688url"].ToString().IndexOf("1688") != -1)
                        {
                            Y_1688url = dr["Y_1688url"].ToString();
                            break;
                        }
                    }
                    if (Y_1688url != "")
                    {
                        foreach (DataRow dr in allrowsbycode)
                        {
                            if (dr["kefadehaiyun"].ToString() != "" && dr["kefadehaiyun"].ToString() != "0" && dr["kefadehaiyun"].ToString().ToLower() != "null")
                            {
                                kefadehaiyun = dr["kefadehaiyun"].ToString();
                                break;
                            }
                        }
                    }

                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["MainImage"].ToString() != "" && dr["MainImage"].ToString() != "0" && dr["MainImage"].ToString().ToLower() != "null")
                        {
                            MainImage = dr["MainImage"].ToString();
                            break;
                        }
                    }
                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["Title"].ToString() != "" && dr["Title"].ToString() != "0" && dr["Title"].ToString().ToLower() != "null")
                        {
                            Title = dr["Title"].ToString();
                            break;
                        }
                    }
                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["Purl"].ToString() != "" && dr["Purl"].ToString() != "0" && dr["Purl"].ToString().ToLower() != "null")
                        {
                            Purl = dr["Purl"].ToString();
                            break;
                        }
                    }
                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["NewPrice_shopeeid"].ToString() != "" && dr["NewPrice_shopeeid"].ToString() != "0" && dr["NewPrice_shopeeid"].ToString().ToLower() != "null")
                        {
                            NewPrice_shopeeid = dr["NewPrice_shopeeid"].ToString();
                            break;
                        }
                    }
                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["rucanglianjie"].ToString() != "" && dr["rucanglianjie"].ToString() != "0" && dr["rucanglianjie"].ToString().ToLower() != "null")
                        {
                            rucanglianjie = dr["rucanglianjie"].ToString();
                            shangjiabianma = dr["shangjiabianma"].ToString();
                            break;
                        }
                    }
                    if (shangjiabianma == "")
                    {
                        foreach (DataRow dr in allrowsbycode)
                        {
                            if (dr["shangjiabianma"].ToString() != "" && dr["shangjiabianma"].ToString() != "0" && dr["shangjiabianma"].ToString().ToLower() != "null")
                            {
                                shangjiabianma = dr["shangjiabianma"].ToString() + "|";
                            }
                        }
                    }
                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["dianpubeizhu"].ToString() != "" && dr["dianpubeizhu"].ToString() != "0" && dr["dianpubeizhu"].ToString().ToLower() != "null")
                        {
                            dianpubeizhu = dr["dianpubeizhu"].ToString();
                            break;
                        }
                    }


                    code = allrowsbycode[0]["code"].ToString();
                    if (MainImage == "" && code != "")
                    {
                        MainImage = access_sql.GetOneValue("select  top 1 image from ProShopeePh where itemid =(select top 1 PHItemid from YNBigData where code='" + code + "')");
                    }


                    dtbd.Rows.Add(new object[] { MainImage, code, Title, Y_1688url, ercipuhuoURL, kefadehaiyun, Purl, NewPrice_shopeeid, rucanglianjie, dianpubeizhu, shangjiabianma });



                }

                if (dtbd != null && dtbd.Rows.Count > 0)
                {
                    rplb.DataSource = dtbd;
                    rplb.DataBind();
                }
                else
                {
                    lits.Text = "无数据";
                }

            }


            else
            {
                lits.Text = "无数据"; ;
            }
        }
        public void bindzhy()
        {
            string sql = "";
            rplb.DataSource = null;
            rplb.DataBind();
            string where = "";
            bool kycx = true;
            if (Request.QueryString["txtsjbm"] != null && Request.QueryString["txtsjbm"] != "")
            {
                where += " shangjiabianma='" + Request.QueryString["txtsjbm"].Trim().Replace("'", "''") + "'";


            }
            if (Request.QueryString["rdno"] != null && Request.QueryString["rdno"] != "")
            {
                if (Request.QueryString["rdno"] == "no")
                {
                    where += " and (rucanglianjie is null or rucanglianjie='' or rucanglianjie='0') ";

                }
                else if (Request.QueryString["rdno"] == "yes")
                {
                    where += " and (rucanglianjie is not null and rucanglianjie<>'' and rucanglianjie<>'0') ";

                }
            }




            string sqler = "select shangjiabianma,id,Purl,NewPrice_shopeeid,rucanglianjie,dianpubeizhu,itemid,MainImage,code,Title,Y_1688url,ercipuhuoURL,kefadehaiyun,ROW_NUMBER() OVER(PARTITION BY code ORDER BY code) AS rn from YNBigData where " + where + " order by code";
            string oldslq = "";
            if (Session["sqler"] != null)
            {
                oldslq = Session["sqler"].ToString();
            }
            Session["sqler"] = sqler;
            if (oldslq == sqler && Session["dtall"] != null)
            {
                kycx = false;
            }
            if (kycx)
            {
                DataSet dser = access_sql.GreatDs(sqler);
                if (access_sql.yzTable(dser))
                {
                    DataTable dter = dser.Tables[0];
                    DataRow[] codeList = dter.Select("rn=1");
                    DataTable dtbd = new DataTable();

                    dtbd.Columns.Add("MainImage");
                    dtbd.Columns.Add("code");
                    dtbd.Columns.Add("Title");
                    dtbd.Columns.Add("Y_1688url");
                    dtbd.Columns.Add("ercipuhuoURL");
                    dtbd.Columns.Add("kefadehaiyun");
                    dtbd.Columns.Add("Purl");
                    dtbd.Columns.Add("NewPrice_shopeeid");
                    dtbd.Columns.Add("rucanglianjie");
                    dtbd.Columns.Add("dianpubeizhu");

                    dtbd.Columns.Add("shangjiabianma");


                    for (int i = 0; i < codeList.Length; i++)
                    {
                        string code = codeList[i]["code"].ToString();
                        string MainImage = "";
                        string Title = "";
                        string Y_1688url = "";

                        string ercipuhuoURL = "";
                        string kefadehaiyun = "";
                        string Purl = "";
                        string NewPrice_shopeeid = "";
                        string rucanglianjie = "";
                        string dianpubeizhu = "";
                        string shangjiabianma = "";

                        DataRow[] allrowsbycode = dter.Select("code='" + code + "'");
                        bool jx = true;
                        foreach (DataRow dr in allrowsbycode)
                        {
                            if (dr["ercipuhuoURL"].ToString() != "" && dr["ercipuhuoURL"].ToString() != "0" && dr["ercipuhuoURL"].ToString().ToLower() != "null")
                            {
                                jx = false;
                                break;
                            }
                        }
                        if (jx)
                        {
                            foreach (DataRow dr in allrowsbycode)
                            {
                                if (dr["Y_1688url"].ToString().IndexOf("1688") != -1)
                                {
                                    Y_1688url = dr["Y_1688url"].ToString();
                                    break;
                                }
                            }
                            if (Y_1688url != "")
                            {
                                foreach (DataRow dr in allrowsbycode)
                                {
                                    if (dr["kefadehaiyun"].ToString() != "" && dr["kefadehaiyun"].ToString() != "0" && dr["kefadehaiyun"].ToString().ToLower() != "null")
                                    {
                                        kefadehaiyun = dr["kefadehaiyun"].ToString();
                                        break;
                                    }
                                }
                            }
                            if (kefadehaiyun != "")
                            {
                                foreach (DataRow dr in allrowsbycode)
                                {
                                    if (dr["MainImage"].ToString() != "" && dr["MainImage"].ToString() != "0" && dr["MainImage"].ToString().ToLower() != "null")
                                    {
                                        MainImage = dr["MainImage"].ToString();
                                        break;
                                    }
                                }
                                foreach (DataRow dr in allrowsbycode)
                                {
                                    if (dr["Title"].ToString() != "" && dr["Title"].ToString() != "0" && dr["Title"].ToString().ToLower() != "null")
                                    {
                                        Title = dr["Title"].ToString();
                                        break;
                                    }
                                }
                                foreach (DataRow dr in allrowsbycode)
                                {
                                    if (dr["Purl"].ToString() != "" && dr["Purl"].ToString() != "0" && dr["Purl"].ToString().ToLower() != "null")
                                    {
                                        Purl = dr["Purl"].ToString();
                                        break;
                                    }
                                }
                                foreach (DataRow dr in allrowsbycode)
                                {
                                    if (dr["NewPrice_shopeeid"].ToString() != "" && dr["NewPrice_shopeeid"].ToString() != "0" && dr["NewPrice_shopeeid"].ToString().ToLower() != "null")
                                    {
                                        NewPrice_shopeeid = dr["NewPrice_shopeeid"].ToString();
                                        break;
                                    }
                                }
                                foreach (DataRow dr in allrowsbycode)
                                {
                                    if (dr["rucanglianjie"].ToString() != "" && dr["rucanglianjie"].ToString() != "0" && dr["rucanglianjie"].ToString().ToLower() != "null")
                                    {
                                        rucanglianjie = dr["rucanglianjie"].ToString();
                                        shangjiabianma = dr["shangjiabianma"].ToString();
                                        break;
                                    }
                                }
                                if (shangjiabianma == "")
                                {
                                    foreach (DataRow dr in allrowsbycode)
                                    {
                                        if (dr["shangjiabianma"].ToString() != "" && dr["shangjiabianma"].ToString() != "0" && dr["shangjiabianma"].ToString().ToLower() != "null")
                                        {
                                            shangjiabianma = dr["shangjiabianma"].ToString() + "|";
                                        }
                                    }
                                }
                                foreach (DataRow dr in allrowsbycode)
                                {
                                    if (dr["dianpubeizhu"].ToString() != "" && dr["dianpubeizhu"].ToString() != "0" && dr["dianpubeizhu"].ToString().ToLower() != "null")
                                    {
                                        dianpubeizhu = dr["dianpubeizhu"].ToString();
                                        break;
                                    }
                                }


                                code = allrowsbycode[0]["code"].ToString();
                                if (MainImage == "" && code != "")
                                {
                                    MainImage = access_sql.GetOneValue("select  top 1 image from ProShopeePh where itemid =(select top 1 PHItemid from YNBigData where code='" + code + "')");
                                }
                            }
                            if (Y_1688url != "" && kefadehaiyun != "")
                            {
                                dtbd.Rows.Add(new object[] { MainImage, code, Title, Y_1688url, ercipuhuoURL, kefadehaiyun, Purl, NewPrice_shopeeid, rucanglianjie, dianpubeizhu, shangjiabianma });
                            }

                        }
                    }

                    if (dtbd != null && dtbd.Rows.Count > 0)
                    {
                        Session["dtall"] = dtbd;
                    }
                    else
                    {
                        lits.Text = "无数据";
                    }

                }

            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }
        public int count = 0;
        public int pg = 0;
        public void deleteyt(int pid)
        {

            DataTable dt = (DataTable)Session["mdt"];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["pid"].ToString() == pid.ToString())
                    {
                        dt.Rows.Remove(dt.Rows[i]);
                        break;
                    }
                }


                rplb.DataSource = dt;
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>加载数据" + dt.Rows.Count + "条</span>";
            }
            else
            {
                lits.Text = "会话状态过期，请关闭浏览器重新打开";
            }



        }


        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "rucang")
            {
                lits.Text = "";
                string code = e.CommandArgument.ToString();


                TextBox txtrucanglianjie = e.Item.FindControl("txtrucanglianjie") as TextBox;


             
                if (txtrucanglianjie.Text.Trim() != "")
                {


                    if (access_sql.T_Update_ExecSql(new string[] { "rucanglianjie" }, new object[] { txtrucanglianjie.Text.Trim().Replace("'", "''") }, "YNBigData", "code='" + code + "'") > 0)
                    {

                        lits.Text = "code:" + code + "入仓链接更新成功";
                    }
                }
                else
                {
                    lits.Text = "入仓链接不能为空";
                    Response.Write("<script>alert('入仓链接不能为空');</script>");
                }
            }


            if (e.CommandName == "bz")
            {
                lits.Text = "";
                string code = e.CommandArgument.ToString();

                TextBox txtdianpubeizhu = e.Item.FindControl("txtdianpubeizhu") as TextBox;


                if (txtdianpubeizhu.Text.Trim() != "")
                {


                    if (access_sql.T_Update_ExecSql(new string[] { "dianpubeizhu" }, new object[] { txtdianpubeizhu.Text.Trim().Replace("'", "''") }, "YNBigData", "code='" + code + "'") > 0)
                    {

                        lits.Text = "code:" + code + "印尼入仓店铺备注更新成功";
                    }
                }
                else
                {
                    lits.Text = "印尼入仓店铺备注不能为空";
                    Response.Write("<script>alert('印尼入仓店铺备注不能为空');</script>");
                }
            }


            if (e.CommandName == "erci")
            {
                lits.Text = "";
                string code = e.CommandArgument.ToString();

                TextBox txtercipuhuoURL = e.Item.FindControl("txtercipuhuoURL") as TextBox;


                if (txtercipuhuoURL.Text.Trim() != "")
                {


                    if (access_sql.T_Update_ExecSql(new string[] { "ercipuhuoURL" }, new object[] { txtercipuhuoURL.Text.Trim().Replace("'", "''") }, "YNBigData", "code='" + code + "'") > 0)
                    {

                        lits.Text = "code:" + code + "二次铺货链接更新成功";
                    }
                }
                else
                {
                    lits.Text = "二次铺货链接不能为空";
                    Response.Write("<script>alert('二次铺货链接不能为空');</script>");
                }
            }
        }





        protected void Button4_Click(object sender, EventArgs e)
        {

        }
    }
}