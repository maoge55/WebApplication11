using System;
using System.Data;
using System.Web;

namespace WebApplication11
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"] == "out")
                    {
                        if (HttpContext.Current.Request.Cookies["u"] != null)
                        {
                            HttpCookie u = HttpContext.Current.Request.Cookies["u"];
                            u.Expires = DateTime.Now.AddDays(-1);
                            HttpContext.Current.Response.Cookies.Add(u);

                        }
                        if (HttpContext.Current.Request.Cookies["p"] != null)
                        {
                            HttpCookie p = HttpContext.Current.Request.Cookies["p"];
                            p.Expires = DateTime.Now.AddDays(-1);
                            HttpContext.Current.Response.Cookies.Add(p);

                        }

                        if (HttpContext.Current.Request.Cookies["uid"] != null)
                        {
                            HttpCookie uid = HttpContext.Current.Request.Cookies["uid"];
                            uid.Expires = DateTime.Now.AddDays(-1);
                            HttpContext.Current.Response.Cookies.Add(uid);

                        }
                        if (HttpContext.Current.Request.Cookies["e"] != null)
                        {
                            HttpCookie uid = HttpContext.Current.Request.Cookies["e"];
                            uid.Expires = DateTime.Now.AddDays(-1);
                            HttpContext.Current.Response.Cookies.Add(uid);
                        }
                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtname.Text != "" && txtpwd.Text != "")
            {
                string pwd = access_sql.GetMD5_32(txtpwd.Text.Trim());
                DataSet ds = access_sql.GreatDs("select * from users where uname='" + txtname.Text.Trim() + "' and upwd='" + pwd + "' and uzt=0 ");
                if (access_sql.yzTable(ds))
                {
                    HttpCookie u = null;

                    if (HttpContext.Current.Request.Cookies["u"] != null)
                    {
                        u = HttpContext.Current.Request.Cookies["u"];
                        u.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(u);

                    }
                    else
                    {
                        u = new HttpCookie("u");

                    }
                    u.Value = ds.Tables[0].Rows[0]["uname"].ToString();
                    u.Expires = DateTime.Now.AddDays(10);
                    HttpContext.Current.Response.Cookies.Add(u);


                    HttpCookie p = null;

                    if (HttpContext.Current.Request.Cookies["p"] != null)
                    {
                        p = HttpContext.Current.Request.Cookies["p"];
                        p.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(p);

                    }
                    else
                    {
                        p = new HttpCookie("p");
                    }
                    p.Value = ds.Tables[0].Rows[0]["upwd"].ToString();
                    p.Expires = DateTime.Now.AddDays(10);
                    HttpContext.Current.Response.Cookies.Add(p);





                    HttpCookie uid = null;

                    if (HttpContext.Current.Request.Cookies["uid"] != null)
                    {
                        uid = HttpContext.Current.Request.Cookies["uid"];
                        uid.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(uid);

                    }
                    else
                    {
                        uid = new HttpCookie("uid");

                    }
                    uid.Value = ds.Tables[0].Rows[0]["uid"].ToString();
                    uid.Expires = DateTime.Now.AddDays(10);
                    HttpContext.Current.Response.Cookies.Add(uid);




                    Response.Redirect("/mb.aspx");

                }
                else
                {
                    lits.Text = "登录失败";
                }
            }
            else
            {
                lits.Text = "不能为空";
            }
        }
    }
}