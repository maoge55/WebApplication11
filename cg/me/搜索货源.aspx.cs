using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 搜索货源 : System.Web.UI.Page
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
                if (uid != "6" && uid != "9" && uid != "10" && uid != "8" && uid != "12" && uid != "21")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindd()
        {


            rplb.DataSource = null;
            rplb.DataBind();
            lits.Text = "";
            Literal1.Text = "";
            DataSet ds = access_sql.GreatDs("select id,MainImage,purl,Title,itemID,SkuID,code,Y_1688url,Y_1688sku1,Y_1688sku2,Y_1688sku3,Y_1688price from YNBigData where title like '%" + txtkjc.Text.Trim().Replace("'", "''") + "%' and Y_1688url like '%1688%' order by itemid");
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string MainImage = dt.Rows[i]["MainImage"].ToString();
                    string code = dt.Rows[i]["code"].ToString();
                    if (MainImage == "" && code != "")
                    {
                        MainImage = access_sql.GetOneValue("select  top 1 image from ProShopeePh where itemid =(select top 1 PHItemid from YNBigData where code='" + code + "')");
                        dt.Rows[i]["MainImage"] = MainImage;
                    }
                }


                rplb.DataSource = dt;
                rplb.DataBind();
                lits.Text = "";
                Literal1.Text = "<span style='color:red'>加载数据" + dt.Rows.Count + "条</span>";
            }
            else
            {
                lits.Text = "无数据";
            }

        }





        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtkjc.Text.Trim() != "")
            {
                bindd();
            }
            else
            {

                lits.Text = "请输入关键词";

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
            if (e.CommandName == "lhurl")
            {
                lits.Text = "";
                string url = e.CommandArgument.ToString();
                string alid = "";
                if (url.IndexOf("offer/") != -1)
                {
                    string temp = url.Replace("offer/", "|").Split('|')[1];
                    if (temp.IndexOf(".") != -1)
                    {
                        alid = temp.Split('.')[0];
                    }
                }
                if (access_sql.GetOneValue("select count(1) from black1688 where url like '%" + alid + "%'") == "0")
                {
                    if (access_sql.T_Insert_ExecSql(new string[] { "url" }, new object[] { url }, "black1688") > 0)
                    {

                        lits.Text = url + "拉黑成功";
                        Response.Write("<script>alert('" + lits.Text + "')</script>");
                    }
                }
                else
                {
                    lits.Text = url + "已经在黑名单中了";
                    Response.Write("<script>alert('" + lits.Text + "')</script>");
                }

            }
            if (e.CommandName == "upcz")
            {
                lits.Text = "";
                int yid = Convert.ToInt32(e.CommandArgument);
                Literal licid = e.Item.FindControl("licid") as Literal;
                TextBox txt_caizhi = e.Item.FindControl("txt_caizhi") as TextBox;

                if (licid.Text != "")
                {
                    if (access_sql.T_Update_ExecSql(new string[] { "caizhi" }, new object[] { txt_caizhi.Text.Trim().Replace("'", "''") }, "caiwu", "cid=" + licid.Text + "") > 0)
                    {
                        bindd();
                        Response.Write("<script>alert('ID:" + yid + "修改材质成功')</script>");

                    }
                }
                else
                {
                    Response.Write("<script>alert('改id还没匹配skuid')</script>");
                }


            }
            if (e.CommandName == "xd")
            {
                lits.Text = "";
                int yid = Convert.ToInt32(e.CommandArgument);
                if (access_sql.T_Update_ExecSql(new string[] { "lastbuhuotime", "yxd" }, new object[] { DateTime.Now.ToString("yyyy/MM/dd"), 1 }, "yacangbuhuo", "yid=" + yid + "") > 0)
                {
                    bindd();
                    lits.Text = "ID:" + yid + "改成已下单成";
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
                        bindd();
                        lits.Text = "ID:" + id + "更新成功";
                    }
                }
                else
                {
                    lits.Text = "1688url不能为空和1688采购价不能为空";
                }
            }
        }
    }
}