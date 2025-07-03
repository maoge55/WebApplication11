using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 初次入仓采购 : System.Web.UI.Page
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
                if (uid != "6" && uid != "9" && uid != "10")
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
            DataSet ds = access_sql.GreatDs("SELECT y.[rucanglianjie],y.[code],y.yunyingbianma,y.phitemid,y.Quantity, y.[ercipuhuoURL],y.[id],y.[purl],y.[Title],y.[itemID],y.[SkuID],y.[sku1],y.[sku2],y.[Y_1688url],y.[Y_1688sku1],y.[Y_1688sku2],y.[Y_1688sku3],y.[Y_1688price], y.Quantity*2  AS 发仓数量_自动,y.Quantity,y.[shifoucaigouchenggong],y.[caigoubeizhu] ,c.rucangSKUID,c.cid,c.caizhi FROM [YNBigData] as y left join (SELECT *, ROW_NUMBER() OVER(PARTITION BY rucangSKUID ORDER BY rucangSKUID) AS rn FROM caiwu) as c on(y.SkuID=c.rucangSKUID and c.rn=1) where  (y.rucanglianjie is not null and y.rucanglianjie<>'' and y.rucanglianjie<>'0') and (y.shifoucaigouchenggong=0 or y.shifoucaigouchenggong='' or y.shifoucaigouchenggong is null) and y.shangjiabianma='" + sjbm + "' and (y.yunyingbianma='goodslink' or y.yunyingbianma='haicang' or y.yunyingbianma='yacang' or y.yunyingbianma='zzw888'  ) and y.code in(SELECT [code] FROM [YNBigData] group by code  HAVING SUM(Quantity) >=3) order by y.code ");
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

                    DataTable dtcode = new DataTable();
                    dtcode.Columns.Add("code");
                    dtcode.Columns.Add("zl");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string c = dt.Rows[i]["code"].ToString();
                        if (dtcode.Select("code='" + c + "'").Length == 0)
                        {
                            DataRow[] drs = dt.Select("code='" + c + "'");
                            int zl = 0;
                            foreach (DataRow dr in drs)
                            {
                                if (dr["Quantity"] != null && dr["Quantity"].ToString() != "")
                                {
                                    zl += int.Parse(dr["Quantity"].ToString());
                                }
                            }
                            dtcode.Rows.Add(new object[] { c, zl });
                        }
                    }
                    DataView view = new DataView(dtcode);
                    view.Sort = "zl desc";
                    dtcode = view.ToTable();

                    DataTable dt2 = new DataTable();
                    dt2 = dt.Copy();
                    dt2.Clear();
                    for (int i = 0; i < dtcode.Rows.Count; i++)
                    {
                        DataRow dr = dtcode.Rows[i];
                        DataRow[] drs = dt.Select("code='" + dr["code"] + "'", "Quantity desc");
                        if (drs.Length > 0)
                        {
                            foreach (DataRow item in drs)
                            {
                                dt2.Rows.Add(item.ItemArray);
                            }
                        }
                    }

                    rplb.DataSource = dt2;
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
            if (txtsjbm.Text.Trim() != "")
            {
                bindcccg();
            }
            else
            {
                lits.Text = "请输入商家编码";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
        private void RegisterStartupScript(string r)
        {
            string rowNumber = (int.Parse(r) - 1).ToString();
            string script = @"<script type='text/javascript'>window.onload = function() {var row = document.getElementById('row_" + rowNumber + "');if (row){row.scrollIntoView();}};</script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "ScrollToRow", script);
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
                string id = e.CommandArgument.ToString().Split('-')[0];
                string md = e.CommandArgument.ToString().Split('-')[1];
                Literal licid = e.Item.FindControl("licid") as Literal;
                TextBox txt_caizhi = e.Item.FindControl("txt_caizhi") as TextBox;

                if (licid.Text != "")
                {
                    if (access_sql.T_Update_ExecSql(new string[] { "caizhi" }, new object[] { txt_caizhi.Text.Trim().Replace("'", "''") }, "caiwu", "cid=" + licid.Text + "") > 0)
                    {
                        bindcccg();
                        RegisterStartupScript((int.Parse(md) + 1).ToString());
                        Response.Write("<script>alert('ID:" + id + "修改材质成功')</script>");

                    }
                }
                else
                {
                    Response.Write("<script>alert('改id还没匹配skuid')</script>");
                }


            }
            if (e.CommandName == "no")
            {
                lits.Text = "";
                string id = e.CommandArgument.ToString().Split('-')[0];
                string md = e.CommandArgument.ToString().Split('-')[1];


                if (access_sql.T_Update_ExecSql(new string[] { "shifoucaigouchenggong" }, new object[] { -1 }, "YNBigData", "id=" + id + "") > 0)
                {
                    bindcccg();
                    RegisterStartupScript(md);
                    lits.Text = "ID:" + id + "改成不能采购";
                }

            }
            if (e.CommandName == "yes")
            {
                lits.Text = "";
                string id = e.CommandArgument.ToString().Split('-')[0];
                string md = e.CommandArgument.ToString().Split('-')[1];

                if (access_sql.T_Update_ExecSql(new string[] { "shifoucaigouchenggong" }, new object[] { 1 }, "YNBigData", "id=" + id + "") > 0)
                {
                    bindcccg();
                    RegisterStartupScript(md);
                    lits.Text = "ID:" + id + "改成已下单成";
                }

            }
            if (e.CommandName == "up")
            {
                lits.Text = "";
                string id = e.CommandArgument.ToString().Split('-')[0];
                string md = e.CommandArgument.ToString().Split('-')[1];
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
                        RegisterStartupScript(md);
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