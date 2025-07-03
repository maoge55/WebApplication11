using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class YGL : System.Web.UI.Page
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
                if (HttpContext.Current.Request.Cookies["eee"] == null)
                {
                    Response.Redirect("/other.aspx?r=ygl.aspx");
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
            DataSet ds = access_sql.GreatDs("select * from ymb where yuid=" + uid + "   order by yid");
            if (access_sql.yzTable(ds))
            {

                rplb.DataSource = ds;
                rplb.DataBind();
            }
        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int yid = Convert.ToInt32(e.CommandArgument);
                access_sql.DoSql("delete from ymb where yid=" + yid);
                access_sql.DoSql("delete from ydata where jyid=" + yid);
                showmewss("删除成功");
                bind();
            }
            if (e.CommandName == "upsj")
            {
                int yid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("/zdyjz.aspx?yid=" + yid);
            }
            if (e.CommandName== "uplm")
            {
                int yid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("/addyjz.aspx?yid=" + yid);
            }
            if (e.CommandName == "update")
            {
                int yid = Convert.ToInt32(e.CommandArgument);
                Literal liyname = e.Item.FindControl("liyname") as Literal;
                liyname.Visible = false;
                LinkButton btnup = e.Item.FindControl("btnup") as LinkButton;
                btnup.Visible = false;
                TextBox txtyname = e.Item.FindControl("txtyname") as TextBox;
                txtyname.Visible = true;
                LinkButton btnsave = e.Item.FindControl("btnsave") as LinkButton;
                btnsave.Visible = true;
                showmewss(yid.ToString());
            }

            if (e.CommandName == "save")
            {
                int yid = Convert.ToInt32(e.CommandArgument);


                Literal liyname = e.Item.FindControl("liyname") as Literal;
                TextBox txtyname = e.Item.FindControl("txtyname") as TextBox;
                LinkButton btnsave = e.Item.FindControl("btnsave") as LinkButton;
                LinkButton btnup = e.Item.FindControl("btnup") as LinkButton;
                string name = txtyname.Text.Trim();
                if (name != "")
                {
                    if (access_sql.GetOneValue("select count(*) from ymb where yname='" + name + "' and yuid=" + uid + "") == "0")
                    {
                        if (access_sql.T_Update_ExecSql(new string[] { "yname" }, new object[] { name }, "ymb", "yid=" + yid + "") != 0)
                        {
                            liyname.Visible = true;
                            btnup.Visible = true;
                            txtyname.Visible = false;
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
            //DropDownList dptype = e.Item.FindControl("dptype") as DropDownList;
            //Literal liotype = e.Item.FindControl("liotype") as Literal;
            //dptype.SelectedValue = liotype.Text;
        }
        public void showmewss(string mess)
        {
            lits.Text = mess;

        }

    }
}