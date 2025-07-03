using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class POD产品审核数量查看与分配 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                Session.Timeout = 240;
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "9")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";



        protected void Button1_Click(object sender, EventArgs e)
        {
            bind();
        }
        public string html = "";
        public void bind()
        {
            string where = "";
            if (dptype.SelectedValue != "all")
            {
                where = " and source_word='" + dptype.SelectedValue + "'";
            }
            string 无人 = access_sql.GetOneValue("select count(*) as 无人 from ProShopeePh where shuse=0 and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='')" + where);
            string 郑雨蝶 = access_sql.GetOneValue("select count(*) as 郑雨蝶 from ProShopeePh where shuse=6 and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='')" + where);
            string 徐震雄 = access_sql.GetOneValue("select count(*) as 徐震雄 from ProShopeePh where shuse=8 and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='')" + where);
            string 陈高乐 = access_sql.GetOneValue("select count(*) as 陈高乐 from ProShopeePh where shuse=11 and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='')" + where);
            string 夏鸿飞 = access_sql.GetOneValue("select count(*) as 夏鸿飞 from ProShopeePh where shuse=12 and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='')" + where);
            string 胡伟国 = access_sql.GetOneValue("select count(*) as 胡伟国 from ProShopeePh where shuse=13 and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='')" + where);
            string 张志伟POD = access_sql.GetOneValue("select count(*) as 张志伟POD from ProShopeePh where shuse=14 and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='')" + where);
            string 林嘉华 = access_sql.GetOneValue("select count(*) as 林嘉华 from ProShopeePh where shuse=16 and datatype=2  and daochu<>-5 and (ispodcp is null or ispodcp='')" + where);


            html += "<tr><td>无人</td><td  class='ccc1'>" + 无人 + "</td></tr>";
            html += "<tr><td>郑雨蝶</td><td class='ccc1'>" + 郑雨蝶 + "</td></tr>";
            html += "<tr><td>徐震雄</td><td  class='ccc1'>" + 徐震雄 + "</td></tr>";
            html += "<tr><td>陈高乐</td><td  class='ccc1'>" + 陈高乐 + "</td></tr>";
            html += "<tr><td>夏鸿飞</td><td  class='ccc1'>" + 夏鸿飞 + "</td></tr>";
            html += "<tr><td>胡伟国</td><td  class='ccc1'>" + 胡伟国 + "</td></tr>";
            html += "<tr><td>张志伟POD</td><td  class='ccc1'>" + 张志伟POD + "</td></tr>";
            html += "<tr><td>林嘉华</td><td  class='ccc1'>" + 林嘉华 + "</td></tr>";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (txtcount.Text.Trim() != "")
            {
                if (int.Parse(txtcount.Text.Trim()) <= 1000)
                {
                    string where = "";
                    if (dptype.SelectedValue != "all")
                    {
                        where = " and source_word='" + dptype.SelectedValue + "'";
                    }
                    string fyuser = drf.SelectedValue;
                    string tyuser = drin.SelectedValue;
                    string sql = "update ProShopeePh set shuse=" + tyuser + " where itemid in(select top " + txtcount.Text.Trim() + " itemid from ProShopeePh where shuse=" + fyuser + " and datatype=2 and daochu>=0 and (ispodcp is null or ispodcp='') " + where + ")";
                    access_sql.DoSql(sql);
                    bind();
                    txtcount.Text = "";

                    lits.Text = "修改完成，已经重新加载统计";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");
                }
                else
                {
                    lits.Text = "数量不能大于1000";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");
                }
            }
            else
            {
                lits.Text = "请输入数量";
                Response.Write("<script>alert('" + lits.Text + "');</script>");

            }
        }
    }
}