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
    public partial class b : System.Web.UI.Page
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

                    bind();

                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";
        public string did = "";
        public string dname = "";
        public string dfile = "";
        public string dsearchtxt = "";
        public DataTable dtyjz = null;
        public DataTable dtouuuu = null;

        public void bind()
        {
            if (Request.QueryString["did"] != null)
            {
                did = Request.QueryString["did"];
                lidid.Text = did;


                dpyjz = works.getyjz(dpyjz, uid);
                if (Request.QueryString["yid"] != null)
                {
                    dpyjz.SelectedValue = Request.QueryString["yid"];
                    DataSet dsyjz = access_sql.GreatDs("select * from ydata where jyid=" + Request.QueryString["yid"]);
                    if (access_sql.yzTable(dsyjz))
                    {
                        dtyjz = dsyjz.Tables[0];
                    }

                }
                DataSet ds = access_sql.GreatDs("select * from mb where did=" + did + " and duid=" + uid + "  order by did");
                if (access_sql.yzTable(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    dname = dr["dname"].ToString();
                    dfile = dr["dfile"].ToString();
                    dsearchtxt = dr["dsearchtxt"].ToString();
                    int drow = int.Parse(dr["drow"].ToString());
                    string tablename = dr["dtablename"].ToString();
                    string did = lidid.Text.Trim();
                    DataSet dsour = access_sql.GreatDs("SELECT lid, lname, lsmt, lqt, lgd, ldid, lrow,  lbt FROM  lb where ldid = " + did + " order by lid");
                    if (access_sql.yzTable(dsour))
                    {
                        dtouuuu = dsour.Tables[0];
                        rplb.DataSource = dtouuuu;
                        rplb.DataBind();
                    }
                    else
                    {
                        if (dfile != "")
                        {
                            dfile = dfile.Split('\\')[dfile.Split('\\').Length - 1];

                            if (File.Exists(dr["dfile"].ToString()))
                            {
                                DataTable dt = works.ReadExcelData_(dr["dfile"].ToString(), tablename, drow);


                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    rplb.DataSource = dt;
                                    rplb.DataBind();

                                }
                            }
                            else
                            {
                                Response.Write("<script>alert(\"文件已经被删除\")</script>");
                            }
                        }

                    }
                }
            }
            else
            {
                Response.Redirect("/mb.aspx");
            }
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {


            DropDownList dpxl = e.Item.FindControl("dpxl") as DropDownList;
            dpxl = works.getxl(dpxl, uid);


            DropDownList dpother = e.Item.FindControl("dpother") as DropDownList;
            dpother = works.getother(dpother, uid);


            TextBox txtlbname = e.Item.FindControl("txtlbname") as TextBox;
            TextBox txtlindex = e.Item.FindControl("txtlindex") as TextBox;
            TextBox txtzdy = e.Item.FindControl("txtzdy") as TextBox;
            Literal libt = e.Item.FindControl("libt") as Literal;

            DataRow[] aaaa = null;
            DataRow[] bbb = null;


            if (dtouuuu != null)
            {
                aaaa = dtouuuu.Select("lname='" + txtlbname.Text.Trim().Replace("'", "''") + "' and lrow=" + txtlindex.Text.Trim() + "");
            }
            if (dtyjz != null)
            {
                bbb = dtyjz.Select("jname='" + txtlbname.Text.Trim().Replace("'", "''") + "'");
            }
            try
            {


                if (aaaa != null && aaaa.Length > 0)
                {


                    dpxl.SelectedValue = aaaa[0]["lsmt"].ToString().Trim();
                    dpother.SelectedValue = aaaa[0]["lqt"].ToString().Trim();
                    txtzdy.Text = aaaa[0]["lgd"].ToString().Trim();

                    if (aaaa[0]["lsmt"].ToString() != "无")
                    {

                        dpxl.BackColor = Color.Pink;
                    }
                    if (aaaa[0]["lqt"].ToString() != "无")
                    {

                        dpother.BackColor = Color.Pink;
                    }
                    if (aaaa[0]["lgd"].ToString() != "")
                    {

                        txtzdy.BackColor = Color.Pink;
                    }

                    if (aaaa[0]["lbt"].ToString() == "1")
                    {
                        libt.Visible = true;
                    }
                    else
                    {
                        libt.Visible = false;
                    }
                }
                if (bbb != null && bbb.Length > 0)
                {


                    dpxl.SelectedValue = bbb[0]["jsmt"].ToString().Trim();
                    dpother.SelectedValue = bbb[0]["jqt"].ToString().Trim();
                    txtzdy.Text = bbb[0]["jgd"].ToString().Trim();

                    if (bbb[0]["jsmt"].ToString() != "无")
                    {

                        dpxl.BackColor = Color.Pink;
                    }
                    if (bbb[0]["jqt"].ToString() != "无")
                    {

                        dpother.BackColor = Color.Pink;
                    }
                    if (bbb[0]["jgd"].ToString() != "")
                    {

                        txtzdy.BackColor = Color.Pink;
                    }
                }

            }
            catch
            {


            }

        }

        protected void btnadddata_Click(object sender, EventArgs e)
        {
            string did = lidid.Text.Trim();
            DataSet ds = access_sql.GreatDs("select * from lb where ldid=" + did);
            bool up = false;
            if (access_sql.yzTable(ds))
            {
                up = true;
            }
            for (int i = 0; i < rplb.Items.Count; i++)
            {
                TextBox txtlbname = rplb.Items[i].FindControl("txtlbname") as TextBox;
                DropDownList dpxl = rplb.Items[i].FindControl("dpxl") as DropDownList;
                DropDownList dpother = rplb.Items[i].FindControl("dpother") as DropDownList;
                TextBox txtzdy = rplb.Items[i].FindControl("txtzdy") as TextBox;
                TextBox txtlindex = rplb.Items[i].FindControl("txtlindex") as TextBox;

                TextBox txtbt = rplb.Items[i].FindControl("txtbt") as TextBox;
                int bt = 0;
                if (txtbt.Text.Trim() == "1")
                {
                    bt = 1;
                }

                if (up && ds.Tables[0].Select("lname='" + txtlbname.Text.Trim().Replace("'", "''") + "'").Length > 0)
                {
                    //更新
                    access_sql.T_Update_ExecSql(new string[] { "lsmt", "lqt", "lgd", "lbt" }, new object[] { dpxl.SelectedValue, dpother.SelectedValue, txtzdy.Text.Trim(), bt }, "lb", "lname='" + txtlbname.Text.Trim().Replace("'", "''") + "' and ldid=" + did + "");
                }
                else
                {
                    //新增
                    access_sql.T_Insert_ExecSql(new string[] { "lname", "lsmt", "lqt", "lgd", "ldid", "lrow", "lbt" }, new object[] { txtlbname.Text.Trim(), dpxl.SelectedValue, dpother.SelectedValue, txtzdy.Text.Trim(), did, txtlindex.Text.Trim(), bt }, "lb");
                }
            }
            Response.Write("<script>alert(\"保存成功\")</script>");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (dpyjz.SelectedValue == "无")
            {
                Response.Write("<script>alert(\"请选择\")</script>");
            }
            else
            {
                Response.Redirect("b.aspx?did=" + Request.QueryString["did"] + "&yid=" + dpyjz.SelectedValue);
            }
        }
    }
}