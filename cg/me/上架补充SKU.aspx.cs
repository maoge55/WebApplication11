using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 上架补充SKU : System.Web.UI.Page
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
                if (uid != "8" && uid != "9" && uid != "10")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public DataTable dt = null;

        public string getimg(string itemid)
        {
            string ru = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("itemid='" + itemid + "'");
                if (dr.Length > 0)
                {
                    ru = "<img src='" + dr[0]["img"].ToString() + "' style='width:200px;height:200px''>";
                }
            }
            return ru;
        }
        public string gettitle(string itemid)
        {
            string ru = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("itemid='" + itemid + "'");
                if (dr.Length > 0)
                {
                    ru = dr[0]["title"].ToString().Replace(txtsearch.Text.Trim(), "<span style='color:red'>" + txtsearch.Text.Trim() + "</span>").Replace(txtsearch.Text.Trim().ToLower(),"<span style='color:red'>" + txtsearch.Text.Trim().ToLower() + "</span>").Replace(txtsearch.Text.Trim().ToUpper(), "<span style='color:red'>" + txtsearch.Text.Trim().ToUpper() + "</span>").Replace(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtsearch.Text.Trim().ToLower()), "<span style='color:red'>" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtsearch.Text.Trim().ToLower()) + "</span>");
                    
                }
            }
            return ru;
        }
        public void bindcccg()
        {
            string searchtxt = txtsearch.Text.Trim().Replace("'", "''");

            DataSet dswyitemid = access_sql.GreatDs("SELECT itemid FROM [YNBigData] where code='" + searchtxt + "' or Title like '%" + searchtxt + "%' group by itemID");
            DataSet dsPPPPHHH = access_sql.GreatDs(" select sku1,sku2,skuimg,[image],itemid from ProShopeePh where itemid in(SELECT PHItemid FROM [YNBigData] where code='" + searchtxt + "' or Title like '%" + searchtxt + "%' group by  PHItemid)");
            DataSet dscdb = access_sql.GreatDs("SELECT id,sku1,sku2,newprice_shopeeid,code,itemid,PHItemid,title FROM [YNBigData] where code='" + searchtxt + "' or Title like '%" + searchtxt + "%' ");
            if (access_sql.yzTable(dscdb))
            {



                dt = dscdb.Tables[0];
                DataTable dtPPPPHHH = dsPPPPHHH.Tables[0];
                dt.Columns.Add("img");
                dt.Columns.Add("skuimg");
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string phitemid = dt.Rows[i]["phitemid"].ToString();
                    string sku1 = dt.Rows[i]["sku1"].ToString().Replace("'", "''");
                    string sku2 = dt.Rows[i]["sku2"].ToString().Replace("'", "''");

                    DataRow[] drsimg = dtPPPPHHH.Select("itemid='" + phitemid + "'");
                    if (drsimg.Length > 0)
                    {
                        dt.Rows[i]["img"] = drsimg[0]["image"].ToString();
                    }

                    DataRow[] drs = dtPPPPHHH.Select("sku1='" + sku1 + "' and sku2='" + sku2 + "' and itemid='" + phitemid + "'");
                    if (drs.Length > 0)
                    {
                        dt.Rows[i]["skuimg"] = drs[0]["skuimg"].ToString();
                    }
                }



                rplb.DataSource = dswyitemid.Tables[0];
                rplb.DataBind();
                Literal1.Text = "<span style='color:red'>加载数据" + dswyitemid.Tables[0].Rows.Count + "条</span>";



            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }





        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtsearch.Text.Trim() != "")
            {
                bindcccg();
            }
            else
            {
                lits.Text = "请输入搜索字符";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "no")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);
                if (access_sql.T_Update_ExecSql(new string[] { "shifoucaigouchenggong" }, new object[] { -1 }, "YNBigData", "id=" + id + "") > 0)
                {
                    bindcccg();
                    lits.Text = "ID:" + id + "改成不能采购";
                }

            }
            if (e.CommandName == "yes")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);
                if (access_sql.T_Update_ExecSql(new string[] { "shifoucaigouchenggong" }, new object[] { 1 }, "YNBigData", "id=" + id + "") > 0)
                {
                    bindcccg();
                    lits.Text = "ID:" + id + "改成已下单成";
                }

            }
            if (e.CommandName == "up")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);
                TextBox txtY_1688url = e.Item.FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688sku1 = e.Item.FindControl("txtY_1688sku1") as TextBox;
                TextBox txtY_1688sku2 = e.Item.FindControl("txtY_1688sku2") as TextBox;
                TextBox txtY_1688sku3 = e.Item.FindControl("txtY_1688sku3") as TextBox;
                TextBox txtY_1688price = e.Item.FindControl("txtY_1688price") as TextBox;
                if (txtY_1688url.Text.Trim() != "" && txtY_1688price.Text.Trim() != "")
                {
                    if (access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''") }, "YNBigData", "id=" + id + "") > 0)
                    {
                        bindcccg();
                        lits.Text = "ID:" + id + "更新成功";
                    }
                }
                else
                {
                    lits.Text = "1688url不能为空和1688采购价不能为空";
                }
            }
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpej = (Repeater)e.Item.FindControl("rpej");
            Literal liitemid = (Literal)e.Item.FindControl("liitemid");
            if (dt != null && dt.Rows.Count > 0)
            {
                string itemid = liitemid.Text;
                DataTable dtls = new DataTable();
                dtls = dt.Clone();
                dtls.Clear();
                DataRow[] drs = dt.Select("itemid='" + itemid + "'");
                if (drs.Length > 0)
                {
                    foreach (DataRow item in drs)
                    {
                        dtls.Rows.Add(item.ItemArray);
                    }
                    rpej.DataSource = dtls;
                    rpej.DataBind();
                }
            }
        }
    }
}