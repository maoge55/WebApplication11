using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class pinpai : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void Button1_Click1(object sender, EventArgs e)
        {
            DataSet ds = access_sql.GreatDs("SELECT a.pbrand, COUNT(*) as count FROM ALLproduct  as a   GROUP BY a.pbrand order by count desc");
            DataSet dsw = access_sql.GreatDs("SELECT * from PPWhite");
            string a = "";
            for (int i = 0; i < dsw.Tables[0].Rows.Count; i++)
            {
                a += "*" + dsw.Tables[0].Rows[i]["ppname"].ToString().ToLower() + "*";
            }
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                lits.Text = "所有" + dt.Rows.Count + "个品牌";
                Literal1.Visible = true;
                string sss = "<table class='ttttt'>";
                int star = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string 品牌 = "<span style='color:颜色'>" + dr["pbrand"] + "</span>";


                    if (dr["pbrand"].ToString() == "" || dr["pbrand"].ToString() == "no brand" || dr["pbrand"].ToString() == "other" || dr["pbrand"].ToString() == "bez marki")
                    {
                        品牌 = 品牌.Replace("颜色", "green");
                    }
                    else
                    {
                        if (a.Contains("*" + dr["pbrand"].ToString().ToLower() + "*"))
                        {
                            品牌 = 品牌.Replace("颜色", "green");
                        }
                        else
                        {
                            品牌 = 品牌.Replace("颜色", "red");
                        }

                    }

                    if (i % 5 == 0)
                    {
                        star = i;
                        sss += "<tr>";
                    }
                    sss += "<td style='width:25%'>" + 品牌 + "(" + dr["count"] + ")" + "</td>";
                    if (i > star + 5)
                    {
                        sss += "</tr>";
                    }
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
            Response.Write("<script>window.open('/pinpaiwhite.aspx', '_blank');</script>");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            DataSet dspw = access_sql.GreatDs("select * from PPWhite order by ppid");
            access_sql.DoSql("update ALLproduct set pw=1 where pbrand is  null or pbrand='no brand' or pbrand ='other' or pbrand ='bez marki'");
            if (access_sql.yzTable(dspw))
            {
                foreach (DataRow item in dspw.Tables[0].Rows)
                {
                    access_sql.DoSql("update ALLproduct set pw=1 where pbrand='" + item["ppname"].ToString().Replace("'", "''") + "'");
                }
                lits.Text = "修改完成";
            }
            else
            {
                lits.Text = "白名单表为空";
            }
        }
    }
}