using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗群控任务表 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["wenben"] != null)
            {
                if (Request.QueryString["wenben"] != "" && Request.QueryString["wenben"] != "no")
                {
                    wenben = Request.QueryString["wenben"];
                }
            }
            if (Request.QueryString["renwu"] != null)
            {
                if (Request.QueryString["renwu"] != "" && Request.QueryString["renwu"] != "no")
                {
                    renwu = Request.QueryString["renwu"];
                }
            }
            if (!IsPostBack)
            {
                if (!access_sql.yzdlcg())
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
                else
                {
                    Session.Timeout = 640;
                    u = HttpContext.Current.Request.Cookies["cu"].Value;
                    p = HttpContext.Current.Request.Cookies["cp"].Value;
                    uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                    if (uid != "9")
                    {

                        Response.Redirect("/cg/clogin.aspx");
                    }


                    if (Request.QueryString["Country"] != null && Request.QueryString["Country"] != "")
                    {
                        rplb.DataSource = null;
                        rplb.DataBind();

                        lits.Text = "";
                        bindzhy();

                    }
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public int count = 0;
        public int pg = 0;



        public string wenben = "";
        public string renwu = "";
        public void bindzhy()
        {
            string sql = "";
            rplb.DataSource = null;
            rplb.DataBind();
            string where = " is_close=0  ";


            if (Request.QueryString["sjbm"] != null && Request.QueryString["sjbm"] != "")
            {
                where += " and SJBM='" + Request.QueryString["sjbm"].Trim().Replace("'", "''") + "'";
            }
            if (Request.QueryString["Country"] != null && Request.QueryString["Country"] != "")
            {
                if (Request.QueryString["Country"] != "all")
                {
                    string cc = "";
                    if (Request.QueryString["Country"] == "pl")
                    {
                        cc = "波兰";
                    }
                    where += "and Country='" + cc + "'";
                }
            }

            if (Request.QueryString["PingTai"] != null && Request.QueryString["PingTai"] != "")
            {
                if (Request.QueryString["PingTai"] != "all")
                {
                    where += " and PingTai='" + Request.QueryString["PingTai"].Trim().Replace("'", "''") + "'";
                }
            }

            if (Request.QueryString["wenben"] != null && Request.QueryString["wenben"] != "" && Request.QueryString["wenben"] != "no")
            {
                if (Request.QueryString["wenben"] != null && Request.QueryString["wenben"] != "")
                {

                    wenben = Request.QueryString["wenben"];
                }

            }

            if (Request.QueryString["renwu"] != null && Request.QueryString["renwu"] != "" && Request.QueryString["renwu"] != "no")
            {
                if (Request.QueryString["renwu"] != null && Request.QueryString["renwu"] != "")
                {

                    renwu = Request.QueryString["renwu"];
                }
                if (Request.QueryString["renwu_zt"] != null && Request.QueryString["renwu_zt"] != "")
                {

                    if (Request.QueryString["renwu_zt"].Trim() == "1")
                    {
                        where += " and " + renwu + "=1";
                    }
                    else
                    {
                        where += " and (" + renwu + "=0 or " + renwu + " is null)";
                    }

                }
            }






            string sqler = "select *,   [0][1] from HouTai_alg where " + where;
            if (renwu != "")
            {
                sqler = sqler.Replace("[0]", renwu + " as nowrw,");
            }
            else
            {
                sqler = sqler.Replace("[0]", "-1 as nowrw,");
            }
            if (wenben != "")
            {
                sqler = sqler.Replace("[1]", wenben + " as wb");
            }
            else
            {
                sqler = sqler.Replace("[1]", "'镍' as wb");
            }

            DataSet dser = access_sql.GreatDs(sqler);
            if (access_sql.yzTable(dser))
            {
                Literal1.Text = "加载数据" + dser.Tables[0].Rows.Count + "条";
                rplb.DataSource = dser.Tables[0];
                rplb.DataBind();


            }
            else
            {
                lits.Text = "无数据"; ;
            }
        }
        public string lasttime = "";
        protected void Button1_Click(object sender, EventArgs e)
        {

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
                int id = Convert.ToInt32(e.CommandArgument);

                TextBox txtPlatform = e.Item.FindControl("txtPlatform") as TextBox;
                TextBox txtCountry = e.Item.FindControl("txtCountry") as TextBox;
                TextBox txtPingTai = e.Item.FindControl("txtPingTai") as TextBox;
                TextBox txtUserName = e.Item.FindControl("txtUserName") as TextBox;
                TextBox txtPassword = e.Item.FindControl("txtPassword") as TextBox;
                TextBox txtSJBM = e.Item.FindControl("txtSJBM") as TextBox;
                TextBox txtYYBM = e.Item.FindControl("txtYYBM") as TextBox;

                RadioButtonList rdjg = (RadioButtonList)e.Item.FindControl("rdjg");

                TextBox txtwb = e.Item.FindControl("txtwb") as TextBox;
                string renwu = Request.QueryString["renwu"];
                string wenben = Request.QueryString["wenben"];

                List<string> nnn = new List<string>() { "Platform", "Country", "PingTai", "UserName", "Password", "SJBM", "YYBM" };
                List<object> ooo = new List<object>() { txtPlatform.Text.Trim().Replace("'", "''"), txtCountry.Text.Trim().Replace("'", "''"), txtPingTai.Text.Trim().Replace("'", "''"), txtUserName.Text.Trim().Replace("'", "''"), txtPassword.Text.Trim().Replace("'", "''"), txtSJBM.Text.Trim().Replace("'", "''"), txtYYBM.Text.Trim().Replace("'", "''") };
                if (renwu != "" && renwu != "no")
                {
                    nnn.Add(renwu);
                    ooo.Add(rdjg.SelectedValue);
                }
                if (wenben != "" && wenben != "no")
                {
                    nnn.Add(wenben);
                    ooo.Add(txtwb.Text.Trim().Replace("'", "''"));
                }



                string[] names = nnn.ToArray();

                object[] objs = ooo.ToArray();
                if (access_sql.T_Update_ExecSql(names, objs, "HouTai_alg", "id=" + id + "") > 0)
                {
                    bindzhy();
                    lits.Text = "ID:" + id + "修改成功";
                }
                else
                {
                    bindzhy();
                    lits.Text = "ID:" + id + "修改失败";
                }


            }

            if (e.CommandName == "bf")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string[] names = new string[] { "is_close" };
                object[] objs = new object[] { 1 };
                if (access_sql.T_Update_ExecSql(names, objs, "HouTai_ALG", "id=" + id + "") > 0)
                {
                    Session.Clear();
                    bindzhy();
                    lits.Text = "ID:" + id + "修改封店成功";
                }
            }
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RadioButtonList rdjg = e.Item.FindControl("rdjg") as RadioButtonList;


            RadioButton RadioButton0 = e.Item.FindControl("RadioButton0") as RadioButton;
            RadioButton RadioButton1 = e.Item.FindControl("RadioButton1") as RadioButton;
            Literal linowrw = e.Item.FindControl("linowrw") as Literal;
            if (linowrw.Text == "0")
            {
                rdjg.SelectedIndex = 0;
            }
            else if (linowrw.Text == "1")
            {
                rdjg.SelectedIndex = 1;
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 1;

            }
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");
                rdjg.SelectedIndex = 0;

            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            int cg = 0;
            for (int i = 0; i < rplb.Items.Count; i++)
            {


                lits.Text = "";
                Literal liid = rplb.Items[i].FindControl("liid") as Literal;

                TextBox txtPlatform = rplb.Items[i].FindControl("txtPlatform") as TextBox;
                TextBox txtCountry = rplb.Items[i].FindControl("txtCountry") as TextBox;
                TextBox txtPingTai = rplb.Items[i].FindControl("txtPingTai") as TextBox;
                TextBox txtUserName = rplb.Items[i].FindControl("txtUserName") as TextBox;
                TextBox txtPassword = rplb.Items[i].FindControl("txtPassword") as TextBox;
                TextBox txtSJBM = rplb.Items[i].FindControl("txtSJBM") as TextBox;
                TextBox txtYYBM = rplb.Items[i].FindControl("txtYYBM") as TextBox;

                RadioButtonList rdjg = (RadioButtonList)rplb.Items[i].FindControl("rdjg");

                TextBox txtwb = rplb.Items[i].FindControl("txtwb") as TextBox;
                string renwu = Request.QueryString["renwu"];
                string wenben = Request.QueryString["wenben"];

                List<string> nnn = new List<string>() { "Platform", "Country", "PingTai", "UserName", "Password", "SJBM", "YYBM" };
                List<object> ooo = new List<object>() { txtPlatform.Text.Trim().Replace("'", "''"), txtCountry.Text.Trim().Replace("'", "''"), txtPingTai.Text.Trim().Replace("'", "''"), txtUserName.Text.Trim().Replace("'", "''"), txtPassword.Text.Trim().Replace("'", "''"), txtSJBM.Text.Trim().Replace("'", "''"), txtYYBM.Text.Trim().Replace("'", "''") };
                if (renwu != "" && renwu != "no")
                {
                    nnn.Add(renwu);
                    ooo.Add(rdjg.SelectedValue);
                }
                if (wenben != "" && wenben != "no")
                {
                    nnn.Add(wenben);
                    ooo.Add(txtwb.Text.Trim().Replace("'", "''"));
                }



                string[] names = nnn.ToArray();

                object[] objs = ooo.ToArray();
                if (access_sql.T_Update_ExecSql(names, objs, "HouTai_alg", "id=" + liid.Text + "") > 0)
                {

                    cg++;
                }
                

            }

            bindzhy();
            lits.Text = "成功修改:" + cg + "个";
        }
    }
}