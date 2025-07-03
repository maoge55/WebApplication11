using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 长期入仓采购 : System.Web.UI.Page
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

        public void bindd()
        {
            if (access_sql.GetOneValue("select count(*) from yacangbuhuo where (Y_1688url is  null or Y_1688url='' or Y_1688url='0')") == "0")
            {
                int cc = 0;
                DataSet ds = access_sql.GreatDs("select y.*,c.rucangSKUID,c.cid,c.caizhi from yacangbuhuo as y left join (SELECT *, ROW_NUMBER() OVER(PARTITION BY rucangSKUID ORDER BY rucangSKUID) AS rn FROM caiwu) as c on(y.SkuID=c.rucangSKUID and c.rn=1) where (y.Y_1688url is not null and y.Y_1688url<>'' and y.Y_1688url<>'0') and y.yxd=0  order by y.yid");
                if (access_sql.yzTable(ds))
                {
                    DataTable dtout = new DataTable();
                    dtout.Columns.Add("yid");
                    dtout.Columns.Add("purl");
                    dtout.Columns.Add("zhongwenbiaoti");
                    dtout.Columns.Add("guige");
                    dtout.Columns.Add("img");

                    dtout.Columns.Add("Y_1688url");
                    dtout.Columns.Add("Y_1688sku1");
                    dtout.Columns.Add("Y_1688sku2");
                    dtout.Columns.Add("Y_1688sku3");
                    dtout.Columns.Add("Y_1688price");
                    dtout.Columns.Add("shuliang");
                    dtout.Columns.Add("shouhuoren");
                    dtout.Columns.Add("lastbuhuotime");

                    dtout.Columns.Add("chanpinID");
                    dtout.Columns.Add("skuid");
                    dtout.Columns.Add("cid");
                    dtout.Columns.Add("rucangSKUID");
                    dtout.Columns.Add("caizhi");
                    dtout.Columns.Add("code");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        DataRow dr = ds.Tables[0].Rows[i];
                        string img = "无图片";
                        string chanpinID = dr["chanpinID"].ToString();
                        string SkuID = dr["SkuID"].ToString();
                        if (chanpinID != "0")
                        {
                            string title = dr["zhongwenbiaoti"].ToString();
                           
                            string purl = "";
                            string code = "";
                            DataSet dssss = access_sql.GreatDs("select purl,code from YNBigData where itemID='" + chanpinID + "' and skuid='" + SkuID + "' ");
                            if (access_sql.yzTable(dssss))
                            {
                                purl = dssss.Tables[0].Rows[0]["purl"].ToString();
                                code = dssss.Tables[0].Rows[0]["code"].ToString();
                                if (code.Length == 8)
                                {
                                    string ProShopeePh_itemid = access_sql.GetOneValue("select itemid from RandomCodes where RandomCode='" + code + "'");
                                    if (ProShopeePh_itemid != "")
                                    {
                                        img = access_sql.GetOneValue("select image from ProShopeePh where itemID='" + ProShopeePh_itemid + "'");
                                        if (img != "")
                                        {
                                            img = "<img src=\"" + img + "\" style=\"width:300px\" />";
                                        }
                                    }
                                }
                            }

                            int 补货数量 = 0;
                            int 第一周销量 = (dr["diyizhouxiaoliang"] != null && dr["diyizhouxiaoliang"].ToString() != "") ? int.Parse(dr["diyizhouxiaoliang"].ToString()) : 0;
                            int 最近一周销量 = (dr["zuijinyizhouxiaoliang"] != null && dr["zuijinyizhouxiaoliang"].ToString() != "") ? int.Parse(dr["zuijinyizhouxiaoliang"].ToString()) : 0;
                            int 实际可用库存量 = (dr["shijikeyongkucunliang"] != null && dr["shijikeyongkucunliang"].ToString() != "") ? int.Parse(dr["shijikeyongkucunliang"].ToString()) : 0;
                            if (第一周销量 == 0 && 最近一周销量 == 0)
                            {

                            }
                            else if (第一周销量 > 0 && 最近一周销量 == 0)
                            {
                                补货数量 = (第一周销量 * 5) - 实际可用库存量;
                                if (补货数量 < 5)
                                {
                                    补货数量 = 0;
                                }
                                else
                                {
                                    if (补货数量 < 10)
                                    {
                                        补货数量 = 10;
                                    }
                                }
                            }
                            else if (最近一周销量 > 0)
                            {

                                if (实际可用库存量 < 5)
                                {
                                    补货数量 = 最近一周销量;
                                }
                                else if (实际可用库存量 >= 5 && 最近一周销量 >= 5)
                                {
                                    补货数量 = 最近一周销量;
                                }
                                if (补货数量 > 0 && 补货数量 < 10)
                                {
                                    补货数量 = 10;
                                }
                            }
                            if (补货数量 != 0)
                            {

                                string time = dr["lastbuhuotime"].ToString();
                                if (time != "")
                                {
                                    time = DateTime.Parse(time).ToString("yyyy/MM/dd");
                                    time += "（距离现在" + (DateTime.Now - DateTime.Parse(time)).Days + "天)";
                                }
                                else
                                {
                                    time = "无";
                                }
                                dtout.Rows.Add(new object[] { dr["yid"], purl, dr["zhongwenbiaoti"], dr["guige"], img, dr["Y_1688url"], dr["Y_1688sku1"], dr["Y_1688sku2"], dr["Y_1688sku3"], dr["Y_1688price"], 补货数量, "王先生-雅仓", time, chanpinID, SkuID, dr["cid"], dr["rucangSKUID"], dr["caizhi"], code });

                            }
                        }

                    }
                    if (dtout != null && dtout.Rows.Count > 0)
                    {
                        rplb.DataSource = dtout;
                        rplb.DataBind();
                        lits.Text = "";
                        Literal1.Text = "<span style='color:red'>加载数据" + dtout.Rows.Count + "条</span>";
                    }
                    else
                    {
                        lits.Text = "无数据";
                    }

                }

            }
            else
            {
                lits.Text = "还存在没有1688链接的数据，请先补充完整信息，再导出";
            }
        }





        protected void Button1_Click(object sender, EventArgs e)
        {
            bindd();
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
                int yid = Convert.ToInt32(e.CommandArgument);
                TextBox txtY_1688url = e.Item.FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688sku1 = e.Item.FindControl("txtY_1688sku1") as TextBox;
                TextBox txtY_1688sku2 = e.Item.FindControl("txtY_1688sku2") as TextBox;
                TextBox txtY_1688sku3 = e.Item.FindControl("txtY_1688sku3") as TextBox;
                TextBox txtY_1688price = e.Item.FindControl("txtY_1688price") as TextBox;
                if (txtY_1688url.Text.Trim() != "" && txtY_1688price.Text.Trim() != "")
                {
                    if (access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "Y_1688_SKU_ID" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), "" }, "yacangbuhuo", "yid=" + yid + "") > 0)
                    {
                        bindd();
                        lits.Text = "ID:" + yid + "更新成功";
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