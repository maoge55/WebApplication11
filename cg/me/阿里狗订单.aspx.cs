using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗订单 : System.Web.UI.Page
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
                if (uid != "12" && uid != "9" && uid != "10" && uid != "13" && uid != "15" && uid != "19" && uid != "20")
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
            if (dpzt.SelectedValue == "待处理")
            {
                a = " and status='NEW' and summary_paymentStatus='PAID' ";
            }
            string aa = " 1=1 ";
            if (txtsjbm.Text.Trim() != "")
            {
                aa = "sjbm='" + txtsjbm.Text.Trim().Replace("'", "''")+"'";
            }
            DataSet ds = access_sql.GreatDs("select * from ALorder where " + aa + " and zt='" + dpzt.SelectedValue + "' " + a + "  order by orderDate desc");
            DataSet dsdp = access_sql.GreatDs("select * from HouTai_ALG where " + aa + " and isGetOrder=1 and order_updated=0 and is_task_baned=0");
            if (access_sql.yzTable(ds))
            {
                rplb.DataSource = ds.Tables[0];
                rplb.DataBind();
                Literal1.Text = "加载到数据" + ds.Tables[0].Rows.Count + "条";
            }
            else
            {
                Literal1.Text = "无数据";
            }
            if (access_sql.yzTable(dsdp))
            {
                Literal2.Text = "该时间点未采集的店铺有：<br>";
                for (int i = 0; i < dsdp.Tables[0].Rows.Count; i++)
                {
                    Literal2.Text += dsdp.Tables[0].Rows[i]["BrowserID"] + " | " + dsdp.Tables[0].Rows[i]["DpName"] + " | " + dsdp.Tables[0].Rows[i]["GroupName"] + "<br>";
                }
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (uid != "9")
            {
                if (txtsjbm.Text.Trim() != "")
                {
                    bindd();
                }
                else
                {
                    lits.Text = "请输入商家编码";
                }
            }
            else
            {
                bindd();

            }
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
                DropDownList dpzt2 = e.Item.FindControl("dpzt2") as DropDownList;
                TextBox txtbz = e.Item.FindControl("txtbz") as TextBox;


                if (access_sql.T_Update_ExecSql(new string[] { "zt", "beizhu" }, new object[] { dpzt2.SelectedValue, txtbz.Text.Trim().Replace("'", "''") }, "ALOrder", "id=" + id + "") > 0)
                {
                    bindd();
                    lits.Text = "ID:" + id + "更新成功";
                }

            }
        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList dpzt2 = e.Item.FindControl("dpzt2") as DropDownList;
            Literal lizt = e.Item.FindControl("lizt") as Literal;
            dpzt2.SelectedValue = lizt.Text;
        }
    }
}