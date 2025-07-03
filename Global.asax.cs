using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApplication11
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Cookies["cu"] != null)
            {
                string u = HttpContext.Current.Request.Cookies["cu"].Value;
                string p = HttpContext.Current.Request.Cookies["cp"].Value;
                string uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "9")
                {




                    string sr_sjbm = "";
                    if (Request.QueryString != null)
                    {

                        string[] keys = Request.QueryString.AllKeys;

                        for (int i = 0; i < keys.Length; i++)
                        {
                            string name = keys[i];


                            if (name == "txtsjbm")
                            {
                                sr_sjbm = Request.QueryString[name];
                            }
                        }
                    }
                    if (Request.Form.Count > 0)
                    {
                        string[] keys = Request.Form.AllKeys;
                        for (int i = 0; i < keys.Length; i++)
                        {
                            string name = keys[i];


                            if (name == "txtsjbm")
                            {
                                sr_sjbm = Request.Form[name];

                            }
                        }

                    }
                    if (sr_sjbm != "")
                    {
                        string sjbm = access_sql.GetOneValue("select sjbm from YN_user where id=" + uid + "");
                        if (sjbm != "")
                        {
                            bool ky = false;
                            string[] qqq = sjbm.Split('|');
                            for (int i = 0; i < qqq.Length; i++)
                            {
                                string q = qqq[i].Trim().ToLower();
                                if (q == sr_sjbm.ToLower().Trim())
                                {
                                    ky = true;
                                    break;
                                }
                            }
                            if (!ky)
                            {
                                Response.ContentType = "text/html; charset=utf-8";
                                Response.Write("非法商家编码,请后退重新输入");
                                Response.End();
                            }
                        }
                    }
                }
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}