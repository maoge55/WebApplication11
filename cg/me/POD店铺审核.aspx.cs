using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class POD店铺审核 : System.Web.UI.Page
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
                if (uid != "11" && uid != "9" && uid != "6" && uid != "16")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy()
        {
            string where = "";
            if (dptype.SelectedValue != "all")
            {
                where = " and source_word='" + dptype.SelectedValue + "'";
            }
            DataSet dsdp = access_sql.GreatDs("select top 10 shopid from ProShopeePh where datatype=2 and ispoddp=0 and ispodcp<>-1 " + where + " group by shopid");
            if (access_sql.yzTable(dsdp))
            {
                DataTable dtdp = dsdp.Tables[0];
                string qq = "";
                foreach (DataRow item in dtdp.Rows)
                {
                    qq += "'" + item[0].ToString() + "'" + ",";
                }
                qq = qq.Substring(0, qq.Length - 1);
                DataSet ds = access_sql.GreatDs("WITH RankedData AS (SELECT top 10000000000000 image,shopid,datatype,itemid,ROW_NUMBER() OVER (PARTITION BY shopid ORDER BY shopid) AS rn FROM ProShopeePh  where datatype=2 and shopid in (" + qq + ") and ispodcp<>-1 ORDER BY RAND())SELECT itemid,image,shopid,datatype FROM RankedData WHERE rn<6 ORDER BY shopid");

                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];
                    dtdp.Columns.Add("imgs");
                    dtdp.Columns.Add("datatype");
                    dtdp.Columns.Add("itemid");
                    for (int i = 0; i < dtdp.Rows.Count; i++)
                    {
                        string shopid = dtdp.Rows[i]["shopid"].ToString();
                        string datatype = "";
                        string itemid = "";
                        string strimgs = "";
                        DataRow[] drimgs = dt.Select("shopid='" + shopid + "'");
                        if (drimgs.Length > 0)
                        {
                            foreach (DataRow item in drimgs)
                            {
                                strimgs += "<img  src='" + item["image"].ToString() + "'>";
                                itemid += item["itemid"].ToString() + "|";
                                datatype += item["datatype"].ToString() + "|";
                            }
                        }
                        dtdp.Rows[i]["imgs"] = strimgs;
                        dtdp.Rows[i]["itemid"] = itemid;
                        dtdp.Rows[i]["datatype"] = datatype;

                    }
                    rplb.DataSource = dtdp;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载数据" + dtdp.Rows.Count + "条</span>";

                }



            }
            else
            {
                lits.Text = "无数据"; ;
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

                Literal lishopid = (Literal)rplb.Items[i].FindControl("lishopid");
                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                if (rdjg.SelectedIndex == -1)
                {
                    ky = false;
                    Response.Write("<script>alert('还有未审核的数据，请检查');</script>");
                    break;
                }
                else
                {
                    if (rdjg.SelectedIndex == 0)
                    {
                        yes_dp += "'" + lishopid.Text + "',";
                    }
                    else if (rdjg.SelectedIndex == 1)
                    {
                        or_dp += "'" + lishopid.Text + "',";
                    }
                    else if (rdjg.SelectedIndex == 2)
                    {
                        no_dp += "'" + lishopid.Text + "',";
                    }
                }
            }
            if (ky)
            {
                int cg = 0;
                if (yes_dp != "")
                {
                    yes_dp = yes_dp.Substring(0, yes_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "ispoddp" }, new object[] { 1 }, "ProShopeePh", "datatype=2 and shopid in (" + yes_dp + ")");
                    string[] dps = yes_dp.Split(',');
                    for (int i = 0; i < dps.Length; i++)
                    {
                        if (dps[i] != "")
                        {

                            if (!IsNumeric(dps[i]))
                            {
                                try
                                {
                                    access_sql.T_Insert_ExecSql(new string[] { "ShopID" }, new object[] { dps[i].ToString().Replace("'", "") }, "AmazonShop");
                                }
                                catch
                                {


                                }
                            }

                        }
                    }

                }
                if (or_dp != "")
                {
                    or_dp = or_dp.Substring(0, or_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "ispoddp" }, new object[] { 2 }, "ProShopeePh", "datatype=2 and shopid in (" + or_dp + ")");
                }
                if (no_dp != "")
                {
                    no_dp = no_dp.Substring(0, no_dp.Length - 1);
                    cg += access_sql.T_Update_ExecSql(new string[] { "ispoddp" }, new object[] { -1 }, "ProShopeePh", "datatype=2 and shopid in (" + no_dp + ")");
                }
                lits.Text = "成功更新" + cg + "个店铺";
                rplb.DataSource = null;
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>请点击按钮，加载新数据</span>";
            }
        }
        public bool IsNumeric(string str)
        {
            Regex regex = new Regex(@"^\d+(\.\d+)?$");
            return regex.IsMatch(str);
        }
    }
}