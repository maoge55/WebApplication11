using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 重新采购无货订单 : System.Web.UI.Page
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
                if (uid != "6" && uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";


        public void bindcccg()
        {
            string sjbm = txtsjbm.Text.Trim().Replace("'", "''");
            string url = txt1688url.Text.Trim();
            string offer = url.Replace("/offer/", "|").Split('|')[1];
            offer= url.Replace("/offer/", "|").Split('|')[1];
            DataSet ds = access_sql.GreatDs("SELECT [code],yunyingbianma,phitemid,Quantity, [id],[purl],[Title],[itemID],[SkuID],[sku1],[sku2],[Y_1688url],[Y_1688sku1],[Y_1688sku2] ,[Y_1688sku3],[Y_1688price], Quantity*2  AS 发仓数量_自动,[shifoucaigouchenggong],[caigoubeizhu] FROM [YNBigData] as y where  (rucanglianjie is not null and rucanglianjie<>'' and rucanglianjie<>'0') and (shifoucaigouchenggong=0 or shifoucaigouchenggong='' or shifoucaigouchenggong is null)  and shangjiabianma='" + sjbm + "' and (yunyingbianma='goodslink' or yunyingbianma='haicang' or yunyingbianma='yacang' ) and code in(SELECT [code] FROM [YNBigData] where shangjiabianma='" + sjbm + "' and (yunyingbianma='goodslink' or yunyingbianma='haicang' or yunyingbianma='yacang' ) group by code  HAVING SUM(Quantity) >=2) order by code ");
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("img");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        string phitemid = dt.Rows[i]["phitemid"].ToString();
                        string sku1 = dt.Rows[i]["sku1"].ToString();
                        string sku2 = dt.Rows[i]["sku2"].ToString();
                        string img = "无图片";



                        if (phitemid != "")
                        {
                            DataSet dsimgs = access_sql.GreatDs("select top 1 image,skuimg from ProShopeePh where itemID='" + phitemid + "' and sku1='" + sku1 + "' and sku2='" + sku2 + "'  ");
                            if (access_sql.yzTable(dsimgs))
                            {
                                DataRow drimg = dsimgs.Tables[0].Rows[0];
                                string image = drimg["image"].ToString();
                                string skuimg = drimg["skuimg"].ToString();
                                if (skuimg != "")
                                {
                                    img = "<img src=\"" + skuimg + "\" style=\"width:300px\" />";
                                }
                                else
                                {
                                    img = "<img src=\"" + image + "\" style=\"width:300px\" />";
                                }
                            }
                            else
                            {
                                img = access_sql.GetOneValue("select image from ProShopeePh where itemID='" + phitemid + "'");
                                if (img != "")
                                {
                                    img = "<img src=\"" + img + "\" style=\"width:300px\" />";
                                }
                            }

                        }



                        dt.Rows[i]["img"] = img;

                    }
                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载数据" + dt.Rows.Count + "条</span>";

                }

            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }





        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtsjbm.Text.Trim() != "" && txt1688url.Text.Trim() != "")
            {
                string url = txt1688url.Text.Trim();
                if (url.IndexOf("1688.com") != -1 && url.IndexOf("offer") != -1 && url.IndexOf(".html") != -1)
                {


                    bindcccg();
                }
                else
                {
                    lits.Text = "1688url格式不正确";
                }
            }
            else
            {
                lits.Text = "请输入商家编码或者1688url";
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
    }
}