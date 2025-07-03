using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 菲律宾虾皮类目链接 : System.Web.UI.Page
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
                if (uid != "9" && uid != "6")
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
            Literal1.Text = "";
            Literal2.Text = "";
            lits.Text = "";
            string a = "";
            bool ky = true;
            string qqq = "";
            int q = 0;
            if (txtQuantity.Text.Trim().Replace("'", "''") != "")
            {
                q = int.Parse(txtQuantity.Text.Trim().Replace("'", "''"));
            }
            string where = " Quantity>" + q + " ";
            if (dpzt.SelectedValue == "未添加类目链接")
            {
                where += " and (lmurl is null or lmurl='0')";

            }
            else if (dpzt.SelectedValue == "已添加类目链接")
            {
                where += " and (lmurl is not null  and lmurl<>'0')";

            }


            if (dpisadpd.SelectedValue != "全部")
            {
                where += " and (isadpd=" + dpisadpd.SelectedValue + ")";

            }



            if (dpisfc.SelectedValue != "全部")
            {
                where += " and (isfc=" + dpisfc.SelectedValue + ")";

            }

            if (dpishy.SelectedValue != "全部")
            {
                where += " and (ishy=" + dpishy.SelectedValue + ")";

            }
            if (txtyybm.Text.Trim() != "")
            {
                where += " and (yybm='" + txtyybm.Text.Trim().Replace("'", "''") + "')";
            }




            if (dpyybm.SelectedValue == "-1")
            {
                where += " and (yybm is null or yybm='')";

            }
            else if (dpyybm.SelectedValue == "1")
            {
                where += " and (yybm is not null  and yybm<>'')";

            }



            if (ky)
            {

                string sqlsx = "select top "+txttop.Text.Trim()+" * from phlmurl where " + where + " order by " + dporder.SelectedValue;
                DataSet ds = access_sql.GreatDs(sqlsx);


                if (access_sql.yzTable(ds))
                {
                    rplb.DataSource = ds.Tables[0];
                    rplb.DataBind();
                    Literal1.Text = "找到数据" + access_sql.GetOneValue("select count(1) from phlmurl where " + where) + "条";
                }
                else
                {
                    Literal1.Text = "无数据";
                }
            }
            else
            {
                Literal1.Text = "输入链接未能找到数据";
            }

        }



        public string getcnjg(string payments_0_paid_amount)
        {
            string ru = "";
            if (payments_0_paid_amount != "")
            {
                ru = (double.Parse(payments_0_paid_amount) * 1.83).ToString("0.00");
            }
            return ru;
        }



        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "up")
            {
                lits.Text = "";
                string code = e.CommandArgument.ToString();

                TextBox txtlmurl = e.Item.FindControl("txtlmurl") as TextBox;
                RadioButtonList rbisadpd = e.Item.FindControl("rbisadpd") as RadioButtonList;
                RadioButtonList rbisfc = e.Item.FindControl("rbisfc") as RadioButtonList;

                RadioButtonList rbishy = e.Item.FindControl("rbishy") as RadioButtonList;

                TextBox txtlmcjtime = e.Item.FindControl("txtlmcjtime") as TextBox;


                TextBox txtysdpurl = e.Item.FindControl("txtysdpurl") as TextBox;
                TextBox txtdpcjtime = e.Item.FindControl("txtdpcjtime") as TextBox;
                TextBox txtyybm = e.Item.FindControl("txtyybm") as TextBox;
                TextBox txtY_1688url = e.Item.FindControl("txtY_1688url") as TextBox;
                if (access_sql.T_Update_ExecSql(new string[] { "lmurl", "lmcjtime", "isadpd", "isfc", "ysdpurl", "dpcjtime", "ishy", "yybm", "Y_1688url" }, new object[] { txtlmurl.Text.Trim().Replace("'", "''"), txtlmcjtime.Text.Trim().Replace("'", "''"), rbisadpd.SelectedValue, rbisfc.SelectedValue, txtysdpurl.Text.Trim().Replace("'", "''"), txtdpcjtime.Text.Trim().Replace("'", "''"), rbishy.SelectedValue, txtyybm.Text.Trim().Replace("'", "''"), txtY_1688url.Text.Trim().Replace("'", "''") }, "phlmurl", "code='" + code + "'") > 0)
                {
                    bindd();
                    lits.Text = "code:" + code + "更新成功";
                }

            }
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal liisadpd = e.Item.FindControl("liisadpd") as Literal;
            RadioButtonList rbisadpd = e.Item.FindControl("rbisadpd") as RadioButtonList;
            rbisadpd.SelectedValue = liisadpd.Text;



            Literal liisfc = e.Item.FindControl("liisfc") as Literal;
            RadioButtonList rbisfc = e.Item.FindControl("rbisfc") as RadioButtonList;
            rbisfc.SelectedValue = liisfc.Text;


            Literal liishy = e.Item.FindControl("liishy") as Literal;
            RadioButtonList rbishy = e.Item.FindControl("rbishy") as RadioButtonList;
            rbishy.SelectedValue = liishy.Text;
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            lits.Text = "";
            DataSet dsadd = access_sql.GreatDs("SELECT y.code,SUM(y.Quantity) AS cc,MIN(y.title) AS title,max(y.Y_1688url) AS Y_1688url,max(p.image) as MainImage,max(y.PHItemid) as PHItemid,max(p.url) as phurl FROM YNBigData as y left join ProShopeePh as p on p.itemid=y.PHItemid where y.dcleimu=0 GROUP BY y.itemID,y.code HAVING SUM(y.Quantity) IS NOT NULL ORDER BY SUM(y.Quantity) DESC");
            if (access_sql.yzTable(dsadd))
            {
                int cg = 0;
                int cz = 0;
                for (int i = 0; i < dsadd.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsadd.Tables[0].Rows[i];

                    string cc = dr["cc"].ToString();

                    string code = dr["code"].ToString();
                    string title = dr["title"].ToString();
                    string Y_1688url = dr["Y_1688url"].ToString();
                    string MainImage = dr["MainImage"].ToString();
                    string PHItemid = dr["PHItemid"].ToString();
                    string phurl = dr["phurl"].ToString();
                    int isadpd = -1;
                    int isfc = -1;
                    if (PHItemid != "")
                    {
                        if (access_sql.GetOneValue("select count(*) from mapd where yuanshilianjie like '%." + PHItemid + "%'") != "0")
                        {
                            isadpd = 1;
                        }
                    }
                    if (access_sql.GetOneValue("select count(1) from phlmurl where code='" + code + "'") == "0")
                    {

                        if (access_sql.T_Insert_ExecSql(new string[] { "code", "Quantity", "title", "Y_1688url", "MainImage", "PHItemid", "phurl", "isadpd", "isfc" }, new object[] { code, cc, title, Y_1688url, MainImage, PHItemid, phurl, isadpd, isfc }, "phlmurl") > 0)
                        {
                            access_sql.DoSql("update YNBigData set dcleimu=1 where code='" + code + "'");
                            cg++;
                        }
                    }
                    else
                    {
                        access_sql.DoSql("update YNBigData set dcleimu=1 where code='" + code + "'");
                        cz++;
                    }

                }
                lits.Text = "新增[" + cg + "]个,已经存在[" + cz + "]个";
            }
            else
            {
                lits.Text = "无数据";
            }



        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            if (txtyybm.Text != "" || uid == "9")
            {
                if (txtQuantity.Text.Trim() != "")
                {
                    bindd();
                }
                else
                {
                    lits.Text = "请输入订单数量";
                }
            }
            else
            {
                lits.Text = "请输入运营编码";
            }
        }

        protected void Button3_Click1(object sender, EventArgs e)
        {
            int cg = 0;
            bool ky = true;

            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal licode = (Literal)rplb.Items[i].FindControl("licode");
                TextBox txtlmurl = rplb.Items[i].FindControl("txtlmurl") as TextBox;

                RadioButtonList rbisadpd = rplb.Items[i].FindControl("rbisadpd") as RadioButtonList;
                RadioButtonList rbisfc = rplb.Items[i].FindControl("rbisfc") as RadioButtonList;
                RadioButtonList rbishy = rplb.Items[i].FindControl("rbishy") as RadioButtonList;
                TextBox txtlmcjtime = rplb.Items[i].FindControl("txtlmcjtime") as TextBox;


                TextBox txtysdpurl = rplb.Items[i].FindControl("txtysdpurl") as TextBox;
                TextBox txtdpcjtime = rplb.Items[i].FindControl("txtdpcjtime") as TextBox;
                TextBox txtyybm = rplb.Items[i].FindControl("txtyybm") as TextBox;
                TextBox txtY_1688url = rplb.Items[i].FindControl("txtY_1688url") as TextBox;

                if (access_sql.T_Update_ExecSql(new string[] { "lmurl", "lmcjtime", "isadpd", "isfc", "ysdpurl", "dpcjtime", "ishy", "yybm", "Y_1688url" }, new object[] { txtlmurl.Text.Trim().Replace("'", "''"), txtlmcjtime.Text.Trim().Replace("'", "''"), rbisadpd.SelectedValue, rbisfc.SelectedValue, txtysdpurl.Text.Trim().Replace("'", "''"), txtdpcjtime.Text.Trim().Replace("'", "''"), rbishy.SelectedValue, txtyybm.Text.Trim().Replace("'", "''"), txtY_1688url.Text.Trim().Replace("'", "''") }, "phlmurl", "code='" + licode.Text + "'") > 0)
                {
                    cg++;
                }

            }
            bindd();
            lits.Text = "更新成功" + cg + "个";

        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            DataTable dt = access_sql.GreatDs("SELECT DISTINCT y.code,y.PHItemid, p.url FROM YNBigData AS y LEFT JOIN ProShopeePh AS p ON p.itemid = y.PHItemid WHERE y.code IN (SELECT code FROM phlmurl where PHItemid is null);").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string code = dt.Rows[i]["code"].ToString();
                string PHItemid = dt.Rows[i]["PHItemid"].ToString();
                string phurl = dt.Rows[i]["url"].ToString();
                int isadpd = -1;
                int isfc = -1;
                if (PHItemid != "")
                {
                    if (access_sql.GetOneValue("select count(*) from mapd where yuanshilianjie like '%." + PHItemid + "%'") != "0")
                    {
                        isadpd = 1;
                    }
                }
                access_sql.T_Update_ExecSql(new string[] { "PHItemid", "phurl", "isadpd", "isfc" }, new object[] { PHItemid, phurl, isadpd, isfc }, "phlmurl", "code='" + code + "'");
            }

        }
    }
}