using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 虾皮菲律宾广告产品找货源 : System.Web.UI.Page
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


                if (uid != "6" && uid != "9" && uid != "19")
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

            string where = "Quantity<>'0' and (Y_1688url is null or Y_1688url='') and (yunyingbianma<>'NONE')";


            DataSet ds = access_sql.GreatDs("SELECT TOP " + txtcount.Text.Trim() + " iii.id,iii.itemid,iii.pname, sss.skuid,iii.image,iii.url,sss.sku1,sss.sku2 FROM ShopeePHADPro AS iii OUTER APPLY (SELECT TOP 1 * FROM ShopeePHADProSKU WHERE itemid = iii.itemid  ORDER BY id ASC) AS sss WHERE   (iii.y_1688url IS NULL OR iii.y_1688url = '') AND iii.yuse = " + uid + "");
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    string count = access_sql.GetOneValue("select count(1) from ShopeePHADPro where  (y_1688url IS NULL OR y_1688url = '') AND yuse = "+ uid + "");
                    DataTable dt = ds.Tables[0];
                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载数据" + dt.Rows.Count + "条，共有" + count + "条</span>";

                }

            }
            else
            {
                lits.Text = "无数据"; ;
            }
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
            if (e.CommandName == "xd")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);
                if (access_sql.T_Update_ExecSql(new string[] { "Y_1688url" }, new object[] { "0" }, "ShopeePHADPro", "id=" + id + "") > 0)
                {
                    bindzhy();
                    lits.Text = "ID:" + id + "改成找不到";
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
                    string url = txtY_1688url.Text.Trim().Replace("'", "''");
                    string alid = "";
                    if (url.IndexOf("offer/") != -1)
                    {
                        string temp = url.Replace("offer/", "|").Split('|')[1];
                        if (temp.IndexOf(".") != -1)
                        {
                            alid = temp.Split('.')[0];
                        }
                    }
                    bool ky = true;
                    if (alid != "")
                    {
                        if (access_sql.GetOneValue("select count(1) from black1688 where url like '%" + alid + "%'") != "0")
                        {
                            ky = false;
                        }
                    }
                    if (ky)
                    {
                        if (access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''") }, "ShopeePHADPro", "id=" + id + "") > 0)
                        {
                            bindzhy();
                            lits.Text = "ID:" + id + "更新成功";
                        }
                    }
                    else
                    {
                        lits.Text = "该1688链接商家无货，请更换1688链接";
                        Response.Write("<script>alert('" + lits.Text + "')</script>");
                    }
                }
                else
                {
                    lits.Text = "1688url不能为空和1688采购价不能为空";
                }
            }
        }

        public void clzy()
        {
            int cg = 0;
            bool ky = true;

            for (int i = 0; i < rplb.Items.Count; i++)
            {

                TextBox txtY_1688url = rplb.Items[i].FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688price = rplb.Items[i].FindControl("txtY_1688price") as TextBox;


                if (txtY_1688url.Text == "" || txtY_1688price.Text == "")
                {
                    ky = false;
                    lits.Text = "第" + (i + 1) + "行数据，1688url不能为空,1688采购价不能为空";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");
                    break;
                }
                string url = txtY_1688url.Text.Trim().Replace("'", "''");
                string alid = "";
                if (url.IndexOf("offer/") != -1)
                {
                    string temp = url.Replace("offer/", "|").Split('|')[1];
                    if (temp.IndexOf(".") != -1)
                    {
                        alid = temp.Split('.')[0];
                    }
                }
                if (alid != "")
                {
                    if (access_sql.GetOneValue("select count(1) from black1688 where url like '%" + alid + "%'") != "0")
                    {
                        ky = false;
                        lits.Text = "第" + (i + 1) + "行数据，该1688链接商家无货，请更换1688链接";
                        Response.Write("<script>alert('" + lits.Text + "');</script>");
                    }
                }
            }



            if (ky)
            {
                for (int i = 0; i < rplb.Items.Count; i++)
                {
                    Literal id = (Literal)rplb.Items[i].FindControl("liid");
                    TextBox txtY_1688url = rplb.Items[i].FindControl("txtY_1688url") as TextBox;
                    TextBox txtY_1688sku1 = rplb.Items[i].FindControl("txtY_1688sku1") as TextBox;
                    TextBox txtY_1688sku2 = rplb.Items[i].FindControl("txtY_1688sku2") as TextBox;
                    TextBox txtY_1688sku3 = rplb.Items[i].FindControl("txtY_1688sku3") as TextBox;
                    TextBox txtY_1688price = rplb.Items[i].FindControl("txtY_1688price") as TextBox;
                    cg += access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''") }, "ShopeePHADPro", "id=" + id + "");
                }
                bindzhy();
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
    }
}