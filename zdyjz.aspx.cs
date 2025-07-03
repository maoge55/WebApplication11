using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class zdyjz : System.Web.UI.Page
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
                if (!IsPostBack)
                {
                    if (Request.QueryString["yid"] != null)
                    {
                        DataSet ds = access_sql.GreatDs("select * from ymb where yid=" + Request.QueryString["yid"] + " and yuid=" + uid + "");
                        if (access_sql.yzTable(ds))
                        {
                            lits.Text = ds.Tables[0].Rows[0]["yname"].ToString();
                            name = ds.Tables[0].Rows[0]["yname"].ToString();
                            liyid.Text = ds.Tables[0].Rows[0]["yid"].ToString();
                            yid = ds.Tables[0].Rows[0]["yid"].ToString();

                            DataSet dsour = access_sql.GreatDs("select * from ydata where jyid=" + Request.QueryString["yid"]);
                            if (access_sql.yzTable(dsour))
                            {
                                dtouuuu = dsour.Tables[0];
                                rplb.DataSource = dsour;
                                rplb.DataBind();
                            }
                        }
                    }

                }
            }

        }
        public string name = "";
        public string yid = "";
        public string u = "";
        public string p = "";
        public string uid = "";
        public string did = "";
        public string dname = "";
        public string dfile = "";
        public string dsearchtxt = "";
        public DataTable dtouuuu = null;



        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            TextBox txtjid = e.Item.FindControl("txtjid") as TextBox;

            DropDownList dpxl = e.Item.FindControl("dpxl") as DropDownList;
            dpxl = works.getxl(dpxl, uid);


            DropDownList dpother = e.Item.FindControl("dpother") as DropDownList;
            dpother = works.getother(dpother, uid);


            TextBox txtlbname = e.Item.FindControl("txtlbname") as TextBox;

            TextBox txtzdy = e.Item.FindControl("txtzdy") as TextBox;
            Literal libt = e.Item.FindControl("libt") as Literal;

            string jid = txtjid.Text.Trim();
            if (dtouuuu != null && dtouuuu.Rows.Count > 0)
            {
                DataRow[] aaaa = dtouuuu.Select("jid=" + jid + "");
                try
                {


                    if (aaaa.Length > 0)
                    {
                        if (aaaa[0]["jsmt"].ToString() != "无"&& aaaa[0]["jsmt"].ToString() != "")
                        {

                            dpxl.BackColor = Color.Pink;
                        }
                        if (aaaa[0]["jqt"].ToString() != "无" && aaaa[0]["jqt"].ToString() != "")
                        {

                            dpother.BackColor = Color.Pink;
                        }
                        if (aaaa[0]["jgd"].ToString() != "")
                        {

                            txtzdy.BackColor = Color.Pink;
                        }
                        dpxl.SelectedValue = aaaa[0]["jsmt"].ToString();

                        dpother.SelectedValue = aaaa[0]["jqt"].ToString();
                        txtzdy.Text = aaaa[0]["jgd"].ToString();
                    }
                }
                catch
                {
                    Response.Write(txtlbname.Text);

                }
            }
        }

        protected void btnadddata_Click(object sender, EventArgs e)
        {
            string yid = liyid.Text.Trim();

            for (int i = 0; i < rplb.Items.Count; i++)
            {
                string jid = (rplb.Items[i].FindControl("txtjid") as TextBox).Text.Trim();

                DropDownList dpxl = rplb.Items[i].FindControl("dpxl") as DropDownList;
                DropDownList dpother = rplb.Items[i].FindControl("dpother") as DropDownList;
                TextBox txtzdy = rplb.Items[i].FindControl("txtzdy") as TextBox;
                access_sql.T_Update_ExecSql(new string[] { "jsmt", "jqt", "jgd" }, new object[] { dpxl.SelectedValue, dpother.SelectedValue, txtzdy.Text.Trim() }, "ydata", "jid=" + jid);

            }
            Response.Write("<script>alert(\"保存成功\")</script>");
        }
    }
}