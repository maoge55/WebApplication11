using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class other : System.Web.UI.Page
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
            if (!IsPostBack)
            {

                bind();
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public void bind()
        {
            DataSet ds = access_sql.GreatDs("select * from otmb where ouid=" + uid + "   order by oid");
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                dt.Columns.Add("count");
                foreach (DataRow item in dt.Rows)
                {
                    item["count"] = access_sql.GetOneValue("select count(*) from otdata where odmid=" + item["oid"]);
                }
                rplb.DataSource = dt;
                rplb.DataBind();
            }
        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int oid = Convert.ToInt32(e.CommandArgument);
                access_sql.DoSql("delete from otmb where oid=" + oid);
                access_sql.DoSql("delete from otdata where odmid=" + oid);
                bind();
            }
            if (e.CommandName == "add")
            {
                int oid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("/addother.aspx?oid=" + oid);
            }
            if (e.CommandName == "update")
            {
                int oid = Convert.ToInt32(e.CommandArgument);
                Literal lioname = e.Item.FindControl("lioname") as Literal;
                lioname.Visible = false;
                Literal liotype = e.Item.FindControl("liotype") as Literal;
                liotype.Visible = false;
                LinkButton btnup = e.Item.FindControl("btnup") as LinkButton;
                btnup.Visible = false;


                TextBox txtoname = e.Item.FindControl("txtoname") as TextBox;
                txtoname.Visible = true;
                DropDownList dptype = e.Item.FindControl("dptype") as DropDownList;
                dptype.Visible = true;


                LinkButton btnsave = e.Item.FindControl("btnsave") as LinkButton;
                btnsave.Visible = true;
                showmewss(oid.ToString());
            }

            if (e.CommandName == "save")
            {
                int oid = Convert.ToInt32(e.CommandArgument);


                Literal lioname = e.Item.FindControl("lioname") as Literal;
                DropDownList dptype = e.Item.FindControl("dptype") as DropDownList;
                TextBox txtoname = e.Item.FindControl("txtoname") as TextBox;
                Literal liotype = e.Item.FindControl("liotype") as Literal;
                LinkButton btnsave = e.Item.FindControl("btnsave") as LinkButton;
                LinkButton btnup = e.Item.FindControl("btnup") as LinkButton;
                string name = txtoname.Text.Trim();
                if (name != "")
                {
                    if (access_sql.GetOneValue("select count(*) from otmb where oname='" + name + "' and  ouid=" + uid + "") == "0" || liotype.Text != dptype.SelectedValue)
                    {
                        if (access_sql.T_Update_ExecSql(new string[] { "oname", "otype" }, new object[] { name, dptype.SelectedValue }, "otmb", "oid=" + oid + "") != 0)
                        {
                            lioname.Visible = true;

                            liotype.Visible = true;

                            btnup.Visible = true;



                            txtoname.Visible = false;

                            dptype.Visible = false;



                            btnsave.Visible = false;
                            bind();
                            showmewss("修改成功");
                        }
                        else
                        {
                            showmewss("修改失败");
                        }
                    }
                    else
                    {
                        showmewss("名字已经存在");
                    }
                }
                else
                {
                    showmewss("不能为空");
                }

            }
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList dptype = e.Item.FindControl("dptype") as DropDownList;
            Literal liotype = e.Item.FindControl("liotype") as Literal;
            dptype.SelectedValue = liotype.Text;
        }
        public void showmewss(string mess)
        {
            lits.Text = mess;

        }

        protected void Button1_Click(object sender, EventArgs e)
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