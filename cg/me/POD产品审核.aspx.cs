using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class POD产品审核 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "11" && uid != "9" && uid != "14" && uid != "6" && uid != "16" && uid != "8" && uid != "12" && uid != "13")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public DataTable getdt()
        {
            DataTable ru = new DataTable();
            ru.Columns.Add("image");

            ru.Columns.Add("itemid");
            ru.Columns.Add("yntitle");
            ru.Columns.Add("shopid");
            DataSet dscp = access_sql.GreatDs("WITH RankedData AS (SELECT shopid,image,itemid,yntitle,ROW_NUMBER() OVER (PARTITION BY itemid ORDER BY itemid) AS rn FROM ProShopeePh  where datatype=2  and daochu<>-5 and itemid in (select top 18 itemid from ProShopeePh where shuse=" + uid + " and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='') group by itemid))SELECT image,itemid,yntitle,shopid FROM RankedData where rn = 1");
            DataSet ds = access_sql.GreatDs("select * from qqc where qtype='印尼语POD' order by qid");
            if (access_sql.yzTable(dscp))
            {
                DataTable dtcp = dscp.Tables[0];
                for (int i = 0; i < dtcp.Rows.Count; i++)
                {
                    string title = dtcp.Rows[i]["yntitle"].ToString().ToLower().Trim();
                    string itemid = dtcp.Rows[i]["itemid"].ToString();
                    bool ky = true;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        string ci = item["qcontent"].ToString().ToLower().Trim();
                        if (title.IndexOf(ci) != -1)
                        {
                            ky = false;
                        }
                    }
                    if (ky)
                    {
                        ru.Rows.Add(dtcp.Rows[i].ItemArray);
                    }
                    else
                    {
                        access_sql.DoSql("update ProShopeePh set daochu=-5 where itemid='" + itemid + "'");
                    }

                }
            }
            else
            {
                ru = null;
            }
            return ru;
        }
        public void bindzhy()
        {
            rplb.DataSource = null;
            rplb.DataBind();
            DataSet dscp = access_sql.GreatDs("WITH RankedData AS (SELECT shopid,image,itemid,yntitle,ROW_NUMBER() OVER (PARTITION BY itemid ORDER BY itemid) AS rn FROM ProShopeePh  where itemid in (select top 18 itemid from ProShopeePh where shuse=" + uid + " and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='') group by itemid))SELECT image,itemid,yntitle,shopid FROM RankedData where rn = 1");
            if (access_sql.yzTable(dscp))
            {
                rplb.DataSource = dscp.Tables[0];
                rplb.DataBind();
                string zs = access_sql.GetOneValue("WITH RankedData AS (select itemid from ProShopeePh where shuse=" + uid + " and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='') group by itemid)select count(1) from RankedData");
                Literal1.Text = "<span style='color:red'>加载数据" + dscp.Tables[0].Rows.Count + "条，共有" + zs + "条数据</span>";
            }


        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            lits.Text = "";

            bindzhy();

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }







        protected void Button2_Click2(object sender, EventArgs e)
        {
            cl();
        }

        protected void Button3_Click2(object sender, EventArgs e)
        {
            cl();
        }
        public void cl()
        {
            lits.Text = "";
            string yes_dp = "";
            string or_dp = "";
            string no_dp = "";
            bool ky = true;
            for (int i = 0; i < rplb.Items.Count; i++)
            {

                Literal liitemid = (Literal)rplb.Items[i].FindControl("liitemid");
                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                if (rdjg.SelectedIndex == -1)
                {

                    ky = false;
                    Response.Write("<script>alert('第" + (i + 1) + "个还未审核，请检查');</script>");
                    break;
                }
                else
                {
                    if (rdjg.SelectedIndex == 0)
                    {
                        yes_dp += "'" + liitemid.Text + "',";
                    }

                    else if (rdjg.SelectedIndex == 1)
                    {
                        no_dp += "'" + liitemid.Text + "',";
                    }
                }
            }
            if (ky)
            {
                int cg = 0;
                if (yes_dp != "")
                {
                    yes_dp = yes_dp.Substring(0, yes_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "ispodcp" }, new object[] { 1 }, "ProShopeePh", "datatype=2 and itemid in (" + yes_dp + ")");

                }

                if (no_dp != "")
                {
                    no_dp = no_dp.Substring(0, no_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "ispodcp" }, new object[] { -1 }, "ProShopeePh", "datatype=2 and itemid in (" + no_dp + ")");
                }
                lits.Text = "成功更新" + cg + "个产品";
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>请点击按钮，加载新数据</span>";
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 0;

            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 1;

            }
        }
    }
}