using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class DT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void Button1_Click1(object sender, EventArgs e)
        {
            DataSet ds = access_sql.GreatDs("SELECT gsq,COUNT(*) as count FROM ALLproduct where gsq<>'' GROUP BY gsq order by count desc");
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                lits.Text = "有" + dt.Rows.Count + "授权码有导出记录";
                Literal1.Visible = true;
                int ccc = 0;
                string sss = "<table class='ttttt'>";
                sss += "<tr>";
                sss += "<td>授权码</td>";
                sss += "<td>导出数量</td>";
              
                sss += "</tr>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string sj = access_sql.GetOneValue("select top 1 gmdp from ALLproduct where gsq='" + dr["gsq"].ToString().Replace("'", "''") + "'");
                    if (sj.Split('镍').Length > 0)
                    {
                        sj = sj.Split('镍')[0];
                    }
                    sss += "<tr>";
                    sss += "<td>" + sj + "</td>";
                    sss += "<td>" + dr["count"] + "</td>";
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