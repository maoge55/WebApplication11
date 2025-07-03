using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗找货源 : System.Web.UI.Page
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
                if (uid != "12" && uid != "9" && uid != "13" && uid != "10" && uid != "6" && uid != "15" && uid != "19" && uid != "8" && uid != "20")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public string getallurl(string aaa)
        {
            string ru = "";
            string[] qq = aaa.Split('|');
            for (int i = 0; i < qq.Length; i++)
            {
                if (qq[i] != "")
                {
                    ru += " <a href='" + qq[i] + "' target='_blank'>打开网址" + (i + 1) + "</a>&nbsp;&nbsp;";
                }
            }
            return ru;
        }
        public void bindzhy(int type)
        {

            rplb.DataSource = null;
            rplb.DataBind();
            lits.Text = "";
            Literal1.Text = "";
            string sql = "";
            string leixing = "";
            string zl = "";
            string ssss = "SELECT  top 1000 leimu, COUNT(1) AS leimu_count  FROM ALLGoodPro where pw=1  GROUP BY leimu  ORDER BY leimu_count DESC";
            string jb = " ap.tg=0 and ap.ky=0 and ap.shenhetiji=1 and ap.pw=1  and ap.visits>=0";
            if (type == 1)
            {

                leixing = "正常";
                zl = access_sql.GetOneValue("select count(*) from ALLGoodPro where tg=0 and pw=1 and ky=0 and shenhetiji=1 and pw=1 and visits>=0  and (Y_1688url is null or Y_1688url='' or Y_1688url='')  and yuse=" + uid + "");
                sql = "select top 10 ap.pean,ap.pid,ap.zhongliang,ap.cptype,ap.visits,ap.added,ap.sold,ap.purl,ap.biaoti,ap.ZhuTuWangZhi,ap.Y_1688url,ap.Y_1688sku1,ap.Y_1688sku2,ap.Y_1688sku3,ap.Y_1688price,ap.shipping_cost,mb_counts.*  from ALLGoodPro as ap JOIN (" + ssss + ") mb_counts on ap.leimu = mb_counts.leimu where " + jb + " and (ap.Y_1688url is null or ap.Y_1688url='' ) and ap.yuse=" + uid + " order by mb_counts.leimu_count DESC,ap.visits desc";
            }
            else if (type == 2)
            {
                leixing = "无重量";
                zl = access_sql.GetOneValue("select count(*) from ALLGoodPro where ky=1 and shenhetiji=1 and pw=1  and visits>=0 and (zhongliang is null or zhongliang= '') and(cptype is not null  and cptype <>'')  and yuse=" + uid + "");
                sql = "select top 10 ap.pean,ap.pid,ap.zhongliang,ap.cptype,ap.visits,ap.added,ap.sold,ap.purl,ap.biaoti,ap.ZhuTuWangZhi,ap.Y_1688url,ap.Y_1688sku1,ap.Y_1688sku2,ap.Y_1688sku3,ap.Y_1688price,ap.shipping_cost,mb_counts.*  from ALLGoodPro as ap JOIN (" + ssss + ") mb_counts on ap.leimu = mb_counts.leimu where  ap.ky=1 and ap.shenhetiji=1 and ap.pw=1 and ap.visits>=0  and (zhongliang is null or zhongliang= '') and(cptype is not null and cptype <>'') and ap.yuse=" + uid + " order by mb_counts.leimu_count DESC,ap.visits desc";
            }
            else if (type == 3)
            {
                leixing = "跳过";
                zl = access_sql.GetOneValue("select count(*) from ALLGoodPro where tg=1 and visits>=0 and yuse=" + uid + "");
                sql = "select top 10 '-'as leimu_count,leimu,pean,pid,zhongliang,cptype,visits,added,sold,purl,biaoti,ZhuTuWangZhi,Y_1688url,Y_1688sku1,Y_1688sku2,Y_1688sku3,Y_1688price,shipping_cost from ALLGoodPro where " + jb.Replace("ap.", "").Replace("tg=0", "tg=1") + "   and yuse=" + uid + "";
            }

            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                if (access_sql.yzTable(ds))
                {

                    DataTable dt = ds.Tables[0];
                    //dt.Columns.Add("pic");
                    //dt.Columns.Add("myurl");
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    DataSet dsallgm = access_sql.GreatDs("select  SelfOfferID,pic from AllGM where pean='" + dt.Rows[i]["pean"] + "' order by update_time desc");
                    //    string pic = "";
                    //    string myurl = "";
                    //    if (access_sql.yzTable(dsallgm))
                    //    {
                    //        for (int q = 0; q < dsallgm.Tables[0].Rows.Count; q++)
                    //        {
                    //            if (pic == "")
                    //            {
                    //                pic = dsallgm.Tables[0].Rows[q]["pic"].ToString();
                    //            }
                    //            myurl += "https://allegro.pl/oferta/" + dsallgm.Tables[0].Rows[q]["SelfOfferID"].ToString() + "|";
                    //        }
                    //        dt.Rows[i]["pic"] = pic;
                    //        dt.Rows[i]["myurl"] = myurl;
                    //    }
                    //    else
                    //    {
                    //        dt.Rows[i]["pic"] = dt.Rows[i]["ZhuTuWangZhi"];
                    //        dt.Rows[i]["myurl"] = dt.Rows[i]["purl"];
                    //    }
                    //}

                    Session["mdt"] = dt;
                    rplb.DataSource = dt;
                    rplb.DataBind();
                    Literal1.Text = "<span style='color:red'>加载" + leixing + "数据" + dt.Rows.Count + "条，共有" + leixing + "数据" + zl + "条";

                }

            }
            else
            {
                lits.Text = leixing + "无数据"; ;
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

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "tg")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);


                if (access_sql.T_Update_ExecSql(new string[] { "tg" }, new object[] { 1 }, "Allgoodpro", "pid=" + id + "") > 0)
                {
                    deleteyt(id);
                    lits.Text = "ID:" + id + "跳过成功";
                }

            }
            if (e.CommandName == "xd")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);


                if (access_sql.T_Update_ExecSql(new string[] { "ky", "tg" }, new object[] { "-1", 0 }, "Allgoodpro", "pid=" + id + "") > 0)
                {
                    deleteyt(id);
                    lits.Text = "ID:" + id + "改成找不到";
                }

            }
            if (e.CommandName == "bc")
            {
                lits.Text = "";
                int id = Convert.ToInt32(e.CommandArgument);
                TextBox txtY_1688url = e.Item.FindControl("txtY_1688url") as TextBox;
                TextBox txtY_1688price = e.Item.FindControl("txtY_1688price") as TextBox;
                if (txtY_1688url.Text.Trim() != "" && txtY_1688price.Text.Trim() != "")
                {
                    if (access_sql.T_Update_ExecSql(new string[] { "ky", "tg" }, new object[] { "-2", 0 }, "Allgoodpro", "pid=" + id + "") > 0)
                    {
                        deleteyt(id);
                        lits.Text = "ID:" + id + "拉黑成功";
                    }
                }
                else
                {
                    lits.Text = "需先填写1688货源信息";
                    Response.Write("<script>alert('需先填写1688货源信息');</script>");
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
                TextBox txt_yf = e.Item.FindControl("txt_yf") as TextBox;

                TextBox txt_zhongliang = e.Item.FindControl("txt_zhongliang") as TextBox;
                RadioButtonList rbcptype = (RadioButtonList)e.Item.FindControl("rbcptype");


                if (txtY_1688url.Text.Trim() != "" && txtY_1688price.Text.Trim() != "" && rbcptype.SelectedIndex != -1)
                {
                    string yf = "";
                    string cptype = rbcptype.SelectedIndex == 0 ? "普货" : "带电";
                    if (txt_yf.Text.Trim() != "")
                    {
                        yf = txt_yf.Text.Trim();
                    }
                    else if (txt_zhongliang.Text.Trim() != "")
                    {
                        yf = ((int)Math.Round((float.Parse(txt_zhongliang.Text.Trim()) * (rbcptype.SelectedIndex == 0 ? 86 : 102) + 13))).ToString();
                    }

                    if (yf != "")
                    {
                        if (access_sql.T_Update_ExecSql(new string[] { "zhongliang", "cptype", "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "ky", "shipping_cost", "tg" }, new object[] { txt_zhongliang.Text.Trim(), cptype, txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), 1, yf, 0 }, "Allgoodpro", "pid=" + id + "") > 0)
                        {
                            access_sql.DoSql("update ALLGoodPro set NewSalePrice=ROUND(((Y_1688price+shipping_cost)*2/1.7),2),isUpdatePrice=1 where pid=" + id + "");
                            deleteyt(id);
                            lits.Text = "ID:" + id + "更新成功";
                        }
                        else
                        {
                            lits.Text = "ID:" + id + "更新失败";
                        }
                    }
                    else
                    {
                        if (access_sql.T_Update_ExecSql(new string[] { "cptype", "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "ky", "tg" }, new object[] { cptype, txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), 1, 0 }, "Allgoodpro", "pid=" + id + "") > 0)
                        {
                            deleteyt(id);
                            lits.Text = "ID:" + id + "更新成功";
                        }
                        else
                        {
                            lits.Text = "ID:" + id + "更新失败";
                        }
                    }
                }
                else
                {
                    lits.Text = "1688url不能为空,1688采购价不能为空,产品类型不能为空";
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

                RadioButtonList rbcptype = (RadioButtonList)rplb.Items[i].FindControl("rbcptype");

                if (txtY_1688url.Text == "" || txtY_1688price.Text == "" || rbcptype.SelectedIndex == -1)
                {
                    ky = false;
                    lits.Text = "1688url不能为空,1688采购价不能为空,产品类型不能为空";
                    Response.Write("<script>alert('需先填写1688货源信息');</script>");
                    break;
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
                    TextBox txt_yf = rplb.Items[i].FindControl("txt_yf") as TextBox;



                    TextBox txt_zhongliang = rplb.Items[i].FindControl("txt_zhongliang") as TextBox;
                    RadioButtonList rbcptype = (RadioButtonList)rplb.Items[i].FindControl("rbcptype");
                    string cptype = rbcptype.SelectedIndex == 0 ? "普货" : "带电";
                    string yf = "";
                    if (txt_yf.Text.Trim() != "")
                    {
                        yf = txt_yf.Text.Trim();
                    }
                    else if (txt_zhongliang.Text.Trim() != "")
                    {
                        yf = (float.Parse(txt_zhongliang.Text.Trim()) * (rbcptype.SelectedIndex == 0 ? 70 : 91) + 13).ToString("0.00");
                    }

                    if (yf != "")
                    {
                        cg += access_sql.T_Update_ExecSql(new string[] { "zhongliang", "cptype", "ky", "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "shipping_cost", "tg" }, new object[] { txt_zhongliang.Text.Trim(), cptype, 1, txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), yf, 0 }, "Allgoodpro", "pid=" + id + "");
                        access_sql.DoSql("update ALLGoodPro set NewSalePrice=ROUND(((Y_1688price+shipping_cost)*2/1.7),2),isUpdatePrice=1 where pid=" + id + "");
                    }
                    else
                    {
                        cg += access_sql.T_Update_ExecSql(new string[] { "cptype", "ky", "Y_1688url", "Y_1688sku1", "Y_1688sku2", "Y_1688sku3", "Y_1688price", "tg" }, new object[] { cptype, 1, txtY_1688url.Text.Trim().Replace("'", "''"), txtY_1688sku1.Text.Trim().Replace("'", "''"), txtY_1688sku2.Text.Trim().Replace("'", "''"), txtY_1688sku3.Text.Trim().Replace("'", "''"), txtY_1688price.Text.Trim().Replace("'", "''"), 0 }, "Allgoodpro", "pid=" + id + "");
                    }







                }
            }

            bindzhy(1);
            lits.Text = "更新成功" + cg + "个,为你加载其他数据";
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

        protected void Button5_Click(object sender, EventArgs e)
        {
            bindzhy(3);
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal licptype = (Literal)e.Item.FindControl("licptype");
            RadioButtonList rbcptype = (RadioButtonList)e.Item.FindControl("rbcptype");
            if (licptype.Text != "")
            {
                if (licptype.Text == "普货")
                {
                    rbcptype.SelectedIndex = 0;
                }
                else
                {
                    rbcptype.SelectedIndex = 1;
                }
            }

        }
    }
}