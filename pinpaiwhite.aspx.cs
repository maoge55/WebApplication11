using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class pinpaiwhite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                bind();
            }
        }



        protected void Button1_Click1(object sender, EventArgs e)
        {
            bind();
        }
        public void bind()
        {
            DataSet ds = access_sql.GreatDs("SELECT * from PPWhite order by ppid");
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                lits.Text = "有" + dt.Rows.Count + "个白名单品牌";
                Literal1.Visible = true;
                string sss = "<table class='ttttt'>";
                sss += "<tr>";
                sss += "<td>品牌</td>";

                sss += "</tr>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    sss += "<tr>";
                    sss += "<td style='width:30%'>" + dr["ppname"] + "</td>";

                    sss += "</tr>";
                }
                sss += "</table>";
                Literal1.Text = sss;

            }
            else

            {
                lits.Text = "无数据";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string aaa = TextBox1.Text.Trim().Replace("\r\n", "镍");
            string[] qqq = aaa.Split(new char[] { '镍' }, StringSplitOptions.RemoveEmptyEntries);
            int wc = 0;
            for (int i = 0; i < qqq.Length; i++)
            {
                string ppname = qqq[i].Trim().Replace("'", "''");
                if (access_sql.GetOneValue("select count(*) from ppwhite where ppname='" + ppname + "'") == "0")
                {
                    access_sql.T_Insert_ExecSql(new string[] { "ppname" }, new object[] { ppname }, "PPWhite");
                    access_sql.DoSql("update ALLproduct set pw=1 where pbrand='" + ppname + "'");
                    wc++;
                }
            }
            lits.Text = "成功加入" + wc + "个";
            bind();

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string aaa = TextBox2.Text.Trim().Replace("\r\n", "镍");
            string[] qqq = aaa.Split(new char[] { '镍' }, StringSplitOptions.RemoveEmptyEntries);
            int wc = 0;
            for (int i = 0; i < qqq.Length; i++)
            {
                string ppname = qqq[i].Trim().Replace("'", "''");
                access_sql.DoSql("delete from ppwhite where ppname='" + ppname + "'");
                access_sql.DoSql("update ALLproduct set pw=0 where pbrand='" + ppname + "'");
            }
           
            bind();
        }
    }
}