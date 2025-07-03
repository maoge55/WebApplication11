using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗订单查货源 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                if (uid != "12" && uid != "9" && uid != "13" && uid != "10" && uid != "15" && uid != "19" && uid != "20")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
                bindbyurl();
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(int type)
        {
            string sql = "";
            if (type == 1)
            {
                sql = "select top 10 pid,visits,added,sold,purl,biaoti,ZhuTuWangZhi,Y_1688url,Y_1688sku1,Y_1688sku2,Y_1688sku3,Y_1688price,shipping_cost from ALLGoodPro where ky=0 and (Y_1688url is null or Y_1688url='' ) and (sold>=1 or added>=1 or  visits>=10) order by sold desc ,added desc,visits desc";
            }
            else
            {
                sql = "select top 10 pid,visits,added,sold,purl,biaoti,ZhuTuWangZhi,Y_1688url,Y_1688sku1,Y_1688sku2,Y_1688sku3,Y_1688price,shipping_cost from ALLGoodPro where ky=1 and (shipping_cost is null or shipping_cost= '')  order by sold desc ,added desc,visits desc";
            }


            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {

                    DataTable dt = ds.Tables[0];
                    Session["mdt"] = dt;
                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + (type == 2 ? "无运费" : "") + "数据" + dt.Rows.Count + "条，共有符合条件数据" + (type == 2 ? access_sql.GetOneValue("select count(*) from ALLGoodPro where ky=1 and (shipping_cost is null or shipping_cost= '')") : access_sql.GetOneValue("select count(*) from ALLGoodPro where ky=0 and (Y_1688url is null or Y_1688url='' or Y_1688url='') and (sold>=1 or added>=1 or  visits>=10)")) + "条 </span>";

                }

            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }

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
            bindzhy(1);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
        public void bindbyurl()
        {
            if (Request.QueryString["url"] != null && txtinoffid.Text.Trim() == "")
            {
                string url = Request.QueryString["url"];
                if (url != "")
                {
                    if (url.IndexOf("?") != -1)
                    {
                        url = url.Substring(0, url.IndexOf("?"));
                    }
                    txtinoffid.Text = url;
                    DataTable dtls = new DataTable();
                    dtls.Columns.Add("offer_id");
                    dtls.Columns.Add("pean");
                    dtls.Columns.Add("jg");

                    string offids = "";


                    string offer_id = "";
                    if (url.IndexOf("-") != -1)
                    {
                        offer_id = (url.Split('-')[url.Split('-').Length - 1]).Trim();
                    }
                    else if (url.IndexOf("oferta") != -1)
                    {
                        offer_id = (url.Split('/')[url.Split('/').Length - 1]).Trim();
                    }


                    string sql = "SELECT ALLGoodPro.yuse,ALLGoodPro.pid,ALLGoodPro.ZhuTuWangZhi,ALLGoodPro.pean,ALLGoodPro.Y_1688url, ALLGoodPro.Y_1688sku1,ALLGoodPro.Y_1688sku2,ALLGoodPro.Y_1688sku3,ALLGoodPro.Y_1688price,ALLGoodPro.shipping_cost FROM ALLGoodPro where (ALLGoodPro.Y_1688url is not null and ALLGoodPro.Y_1688url<>'' ) and ALLGoodPro.pean in ( select pean from AllGM where SelfOfferID in (" + offer_id + ") group by pean)";
                    DataSet dsss = access_sql.GreatDs(sql);
                    if (access_sql.yzTable(dsss))
                    {
                        DataTable dt = dsss.Tables[0];

                        rplb.DataSource = dt;
                        rplb.DataBind();
                        lits.Text = "查找到【查找已有货源】数据" + dt.Rows.Count + "条";
                    }
                    else
                    {
                        lits.Text = "未找到数据";

                    }
                }
                else
                {
                    lits.Text = "请输入搜索链接";
                }
            }
        }
        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "up")
            {
                lits.Text = "";
                string pean = e.CommandArgument.ToString();
                TextBox txtY_1688url = e.Item.FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688sku1 = e.Item.FindControl("txtY_1688sku1") as TextBox;
                TextBox txtY_1688sku2 = e.Item.FindControl("txtY_1688sku2") as TextBox;
                TextBox txtY_1688sku3 = e.Item.FindControl("txtY_1688sku3") as TextBox;
                TextBox txtY_1688price = e.Item.FindControl("txtY_1688price") as TextBox;
                TextBox txt_yf = e.Item.FindControl("txt_yf") as TextBox;

                if (txtY_1688url.Text.Trim() != "" && txtY_1688price.Text.Trim() != "" && txt_yf.Text.Trim() != "")
                {




                    if (access_sql.T_Update_ExecSql(new string[] { "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "ky", "shipping_cost" }, new object[] { txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), 1, txt_yf.Text.Trim().Replace("'", "''") }, "Allgoodpro", "pean='" + pean + "'") > 0)
                    {
                        access_sql.DoSql("update ALLGoodPro set NewSalePrice=ROUND(((Y_1688price+shipping_cost)*2/1.7),2),isUpdatePrice=1 where pean='" + pean + "'");

                        lits.Text = "pean:" + pean + "更新成功";
                    }
                    else
                    {
                        lits.Text = "pean:" + pean + "更新失败";
                    }




                }
                else
                {
                    lits.Text = "1688url不能为空,1688采购价不能为空,运费不能为空";
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
                TextBox txt_yf = rplb.Items[i].FindControl("txt_yf") as TextBox;
                if (txtY_1688url.Text == "" || txtY_1688price.Text == "" || txt_yf.Text == "")
                {
                    ky = false;
                    lits.Text = "1688url不能为空,1688采购价不能为空,运费不能为空";
                    Response.Write("<script>alert('第" + (i + 1) + "需先填写1688货源信息');</script>");
                    break;
                }



            }
            if (ky)
            {
                for (int i = 0; i < rplb.Items.Count; i++)
                {
                    Literal lipean = (Literal)rplb.Items[i].FindControl("lipean");
                    string pean = lipean.Text;
                    TextBox txtY_1688url = rplb.Items[i].FindControl("txtY_1688url") as TextBox;
                    TextBox txtY_1688sku1 = rplb.Items[i].FindControl("txtY_1688sku1") as TextBox;
                    TextBox txtY_1688sku2 = rplb.Items[i].FindControl("txtY_1688sku2") as TextBox;
                    TextBox txtY_1688sku3 = rplb.Items[i].FindControl("txtY_1688sku3") as TextBox;
                    TextBox txtY_1688price = rplb.Items[i].FindControl("txtY_1688price") as TextBox;
                    TextBox txt_yf = rplb.Items[i].FindControl("txt_yf") as TextBox;




                    cg += access_sql.T_Update_ExecSql(new string[] { "ky", "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "shipping_cost" }, new object[] { 1, txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), txt_yf.Text.Trim().Replace("'", "''") }, "Allgoodpro", "pean='" + pean + "'");
                    access_sql.DoSql("update ALLGoodPro set NewSalePrice=ROUND(((Y_1688price+shipping_cost)*2/1.7),2),,isUpdatePrice=1 where pean='" + pean + "'");






                }
            }


            lits.Text = "更新成功" + cg + "个";
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
            bindzhy(2);
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            if (txtinoffid.Text.Trim() != "")
            {
                string[] urls = txtinoffid.Text.Trim().Replace("\r\n", "|").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                DataTable dtls = new DataTable();
                dtls.Columns.Add("offer_id");
                dtls.Columns.Add("pean");
                dtls.Columns.Add("jg");

                string offids = "";

                for (int i = 0; i < urls.Length; i++)
                {
                    string url = urls[i];
                    string offer_id = "";
                    if (url.IndexOf("-") != -1)
                    {
                        offer_id = (url.Split('-')[url.Split('-').Length - 1]).Trim();
                    }
                    else if (url.IndexOf("oferta") != -1)
                    {
                        offer_id = (url.Split('/')[url.Split('/').Length - 1]).Trim();
                    }
                    if (offer_id != "")
                    {
                        offids += "'" + offer_id + "',";
                    }
                }
                if (offids != "")
                {
                    offids = offids.Substring(0, offids.Length - 1);
                }
                string sql = "SELECT ALLGoodPro.yuse,ALLGoodPro.pid,ALLGoodPro.ZhuTuWangZhi,ALLGoodPro.pean,ALLGoodPro.Y_1688url, ALLGoodPro.Y_1688sku1,ALLGoodPro.Y_1688sku2,ALLGoodPro.Y_1688sku3,ALLGoodPro.Y_1688price,ALLGoodPro.shipping_cost FROM ALLGoodPro where (ALLGoodPro.Y_1688url is not null and ALLGoodPro.Y_1688url<>'' ) and ALLGoodPro.pean in ( select pean from AllGM where SelfOfferID in (" + offids + ") group by pean)";
                DataSet dsss = access_sql.GreatDs(sql);
                if (access_sql.yzTable(dsss))
                {
                    DataTable dt = dsss.Tables[0];

                    rplb.DataSource = dt;
                    rplb.DataBind();
                    lits.Text = "查找到【查找已有货源】数据" + dt.Rows.Count + "条";
                }
                else
                {
                    lits.Text = "未找到数据";

                }
            }
            else
            {
                lits.Text = "请输入搜索链接";
            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            if (txtinoffid.Text.Trim() != "")
            {
                string[] urls = txtinoffid.Text.Trim().Replace("\r\n", "|").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                DataTable dtls = new DataTable();
                dtls.Columns.Add("offer_id");
                dtls.Columns.Add("pean");
                dtls.Columns.Add("jg");

                string offids = "";

                for (int i = 0; i < urls.Length; i++)
                {
                    string url = urls[i];
                    string offer_id = "";
                    if (url.IndexOf("-") != -1)
                    {
                        offer_id = (url.Split('-')[url.Split('-').Length - 1]).Trim();
                    }
                    else if (url.IndexOf("oferta") != -1)
                    {
                        offer_id = (url.Split('/')[url.Split('/').Length - 1]).Trim();
                    }
                    if (offer_id != "")
                    {
                        offids += "'" + offer_id + "',";
                    }
                }
                if (offids != "")
                {
                    offids = offids.Substring(0, offids.Length - 1);
                }
                string sql = "SELECT ALLGoodPro.yuse,ALLGoodPro.pid,ALLGoodPro.ZhuTuWangZhi,ALLGoodPro.pean,ALLGoodPro.Y_1688url, ALLGoodPro.Y_1688sku1,ALLGoodPro.Y_1688sku2,ALLGoodPro.Y_1688sku3,ALLGoodPro.Y_1688price,ALLGoodPro.shipping_cost FROM ALLGoodPro where  ALLGoodPro.pean in ( select pean from AllGM where SelfOfferID in (" + offids + ") group by pean)";
                DataSet dsss = access_sql.GreatDs(sql);
                if (access_sql.yzTable(dsss))
                {
                    DataTable dt = dsss.Tables[0];

                    rplb.DataSource = dt;
                    rplb.DataBind();
                    lits.Text = "查找到【补充订单货源】数据" + dt.Rows.Count + "条";
                }
                else
                {
                    lits.Text = "未找到数据";

                }
            }
            else
            {
                lits.Text = "请输入搜索链接";
            }
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            string[] urls = txtinoffid.Text.Trim().Replace("\r\n", "镍").Split(new char[] { '镍' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < urls.Length; i++)
            {
                string[] aaaaa = urls[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string pid = aaaaa[0];
                string shipping_cost = aaaaa[1];
                string Y_1688price = aaaaa[2];
                access_sql.T_Update_ExecSql(new string[] { "shipping_cost", "Y_1688price" }, new object[] { shipping_cost, Y_1688price }, "ALLGoodPro", "pid=" + pid + "");
            }
            lits.Text = "123";
        }
    }

}