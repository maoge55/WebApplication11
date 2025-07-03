using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class uplb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdl())
            {
                Response.Redirect("/login.aspx");
            }
            else
            {
                u = HttpContext.Current.Request.Cookies["u"].Value;
                p = HttpContext.Current.Request.Cookies["p"].Value;
                uid = HttpContext.Current.Request.Cookies["uid"].Value;
                if (HttpContext.Current.Request.Cookies["eee"] != null)
                {
                    lg.Visible = false;
                    zc.Visible = true;
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtlbm.Text.Trim() != "")
            {
                string count = access_sql.GetOneValue("select count(*) from lb where lname='" + txtlbm.Text.Trim().Replace("'", "''") + "' and ldid in (select did from mb where duid=" + uid + ")");
                lits.Text = "可修改数据列为" + count;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (txtlbm.Text.Trim() != "")
            {
                string upstr = "";
                string name = txtlbm.Text.Trim().Replace("'", "''");
                string value = txtvalue.Text.Trim().Replace("'", "''");
                string lgd = "";
                string lqt = "无";
                string lsmt = "无";
                switch (DropDownList1.SelectedValue)
                {
                    case "lgd":
                        lgd = value;
                        upstr = "lgd='" + value + "' ,lqt='无',lsmt='无'";
                        break;
                    case "lqt":
                        lqt = value;
                        upstr = "lqt='" + value + "' ,lgd='',lsmt='无'";
                        break;
                    case "lsmt":
                        lsmt = value;
                        upstr = "lsmt='" + value + "' ,lqt='无',lgd=''";
                        break;
                }

                access_sql.DoSql("update  lb set " + upstr + " where lname='" + name + "' and ldid in (select did from mb where duid=" + uid + ")");
                DataSet dsss = access_sql.GreatDs("select yid from ymb where yuid = " + uid + "");
                if (access_sql.yzTable(dsss))
                {
                    if (access_sql.GetOneValue("select count(*) from ydata where jname='" + txtlbm.Text.Trim().Replace("'", "''") + "' and jyid=" + dsss.Tables[0].Rows[0]["yid"].ToString() + "") != "0")
                    {
                        access_sql.T_Update_ExecSql(new string[] { "jsmt", "jqt", "jgd", "jyid" }, new object[] { lsmt, lqt, lgd, dsss.Tables[0].Rows[0]["yid"].ToString() }, "ydata", "jname='" + name + "'");
                    }
                    else
                    {
                        access_sql.T_Insert_ExecSql(new string[] { "jname", "jsmt", "jqt", "jgd", "jyid" }, new object[] { name, lsmt, lqt, lgd, dsss.Tables[0].Rows[0]["yid"].ToString() }, "ydata");
                    }
                }
                lits.Text = "完成";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (txtpwd.Text.Trim() == "cai-8897")
            {
                zc.Visible = true;
                lg.Visible = false;

                HttpCookie eee = null;

                if (HttpContext.Current.Request.Cookies["eee"] != null)
                {
                    eee = HttpContext.Current.Request.Cookies["eee"];
                    eee.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(eee);

                }
                else
                {
                    eee = new HttpCookie("eee");

                }
                eee.Value = txtpwd.Text.Trim();
                eee.Expires = DateTime.Now.AddDays(10);
                HttpContext.Current.Response.Cookies.Add(eee);
                if (Request.QueryString["r"] != null)
                {
                    Response.Redirect("/" + Request.QueryString["r"]);
                }
            }
            else
            {
                Response.Write("<script>alert(\"密码错误\")</script>");
            }
        }
    }
}