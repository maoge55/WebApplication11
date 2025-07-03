using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class pm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void Button1_Click1(object sender, EventArgs e)
        {
            DataSet ds = access_sql.GreatDs("SELECT a.pmbid, COUNT(*) as count,b.dname FROM ALLproduct as a left join mb as b on a.pmbid=b.did  where a.pean is not null and a.pmbid<>-1 and a.pmbid<>0 and a.pgm=0 and a.pw<>0 and b.dstate<>-1 and a.isuse=0 GROUP BY a.pmbid,b.dname order by count desc");
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                lits.Text = "有" + dt.Rows.Count + "个模板有产品数据";
                Literal1.Visible = true;
                int ccc = 0;
                string sss = "<table class='ttttt'>";
                sss += "<tr>";
                sss += "<td>模板ID</td>";
                sss += "<td>产品数量</td>";
                sss += "<td>模板名称</td>";
                sss += "</tr>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    sss += "<tr>";
                    sss += "<td>" + dr["pmbid"] + "</td>";
                    sss += "<td>" + dr["count"] + "</td>";
                    sss += "<td>" + dr["dname"] + "</td>";
                    sss += "</tr>";
                    ccc += int.Parse(dr["count"].ToString());
                }
                sss += "</table>";
                Literal1.Text = sss;
                lits.Text += "，共有产品" + ccc + "个";
            }
            else

            {
                lits.Text = "无数据";
            }
        }
    }
}