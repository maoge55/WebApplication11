using System;
using System.Data;
using System.Web;

namespace WebApplication11
{
    public partial class clogin : System.Web.UI.Page
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
                DataSet ds = access_sql.GreatDs("select * from yn_user where us='" + txtname.Text.Trim() + "' and pass='" + pwd + "'");
                
                if (access_sql.yzTable(ds))
                {
                    HttpCookie u = null;

                    if (HttpContext.Current.Request.Cookies["cu"] != null)
                    {
                        u = HttpContext.Current.Request.Cookies["cu"];
                        u.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(u);

                    }
                    else
                    {
                        u = new HttpCookie("cu");

                    }
                    u.Value = ds.Tables[0].Rows[0]["us"].ToString();
                    u.Expires = DateTime.Now.AddDays(10);
                    HttpContext.Current.Response.Cookies.Add(u);


                    HttpCookie p = null;

                    if (HttpContext.Current.Request.Cookies["cp"] != null)
                    {
                        p = HttpContext.Current.Request.Cookies["cp"];
                        p.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(p);

                    }
                    else
                    {
                        p = new HttpCookie("cp");
                    }
                    p.Value = ds.Tables[0].Rows[0]["pass"].ToString();
                    p.Expires = DateTime.Now.AddDays(10);
                    HttpContext.Current.Response.Cookies.Add(p);





                    HttpCookie uid = null;

                    if (HttpContext.Current.Request.Cookies["cuid"] != null)
                    {
                        uid = HttpContext.Current.Request.Cookies["cuid"];
                        uid.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(uid);

                    }
                    else
                    {
                        uid = new HttpCookie("cuid");

                    }
                    uid.Value = ds.Tables[0].Rows[0]["id"].ToString();
                    uid.Expires = DateTime.Now.AddDays(10);
                    HttpContext.Current.Response.Cookies.Add(uid);


                    if (ds.Tables[0].Rows[0]["id"].ToString() == "6" || ds.Tables[0].Rows[0]["id"].ToString() == "8" || ds.Tables[0].Rows[0]["id"].ToString() == "9" || ds.Tables[0].Rows[0]["id"].ToString() == "10" || ds.Tables[0].Rows[0]["id"].ToString() == "11" || ds.Tables[0].Rows[0]["id"].ToString() == "12" || ds.Tables[0].Rows[0]["id"].ToString() == "13" || ds.Tables[0].Rows[0]["id"].ToString() == "14" || ds.Tables[0].Rows[0]["id"].ToString() == "15" || ds.Tables[0].Rows[0]["id"].ToString() == "16" || ds.Tables[0].Rows[0]["id"].ToString() == "17" || ds.Tables[0].Rows[0]["id"].ToString() == "18" || ds.Tables[0].Rows[0]["id"].ToString() == "19" || ds.Tables[0].Rows[0]["id"].ToString() == "22" || ds.Tables[0].Rows[0]["id"].ToString() == "21" )
                    {
                        Response.Redirect("/cg/main.aspx");
                    }
                    else
                    {
                        Response.Redirect("/cg/see.aspx");
                    }

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