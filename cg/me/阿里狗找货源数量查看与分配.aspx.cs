using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗找货源数量查看与分配 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
            string 无人 = "";
            string 郑雨蝶 = "";
            string 张志伟 = "";
            string 夏鸿飞 = "";
            string 胡伟国 = "";
            string 杨庆烟 = "";
            string 王志敏 = "";
            string 徐震雄 = "";
            string 陈琳 = "";
            string ssss = "SELECT  top 1000 leimu, COUNT(1) AS leimu_count  FROM ALLGoodPro where pw=1  GROUP BY leimu  ORDER BY leimu_count DESC";
            string sql = "select ap.yuse,ap.pid,ap.leimu,ap.shenhetiji,ap.pw,ap.Y_1688url,mb_counts.* from ALLGoodPro  as ap JOIN (" + ssss + ") mb_counts on ap.leimu = mb_counts.leimu where  ap.tg=0 and ap.ky=0 and ap.shenhetiji=1 and ap.pw=1 and ap.visits>=0 and (Y_1688url is null or Y_1688url='' or Y_1688url='') {0} order by mb_counts.leimu_count DESC, ap.visits desc";
            DataSet all = access_sql.GreatDs(sql.Replace("{0}", ""));

            if (access_sql.yzTable(all))
            {
                DataTable dtall = all.Tables[0];
                Session["dtall"] = dtall;
                无人 = dtall.Select("yuse=0").Length.ToString();
                郑雨蝶 = dtall.Select("yuse=6").Length.ToString();
                张志伟 = dtall.Select("yuse=10").Length.ToString();
                夏鸿飞 = dtall.Select("yuse=12").Length.ToString();
                胡伟国 = dtall.Select("yuse=13").Length.ToString();
                杨庆烟 = dtall.Select("yuse=15").Length.ToString();
                王志敏 = dtall.Select("yuse=19").Length.ToString();
                徐震雄 = dtall.Select("yuse=8").Length.ToString();
                陈琳 = dtall.Select("yuse=20").Length.ToString();
            }






            html += "<tr><td>无人</td><td  class='ccc1'>" + 无人 + "</td></tr>";
            html += "<tr><td>郑雨蝶</td><td class='ccc1'>" + 郑雨蝶 + "</td></tr>";
            html += "<tr><td>张志伟</td><td  class='ccc1'>" + 张志伟 + "</td></tr>";
            html += "<tr><td>夏鸿飞</td><td  class='ccc1'>" + 夏鸿飞 + "</td></tr>";
            html += "<tr><td>胡伟国</td><td  class='ccc1'>" + 胡伟国 + "</td></tr>";
            html += "<tr><td>杨庆烟</td><td  class='ccc1'>" + 杨庆烟 + "</td></tr>";
            html += "<tr><td>王志敏</td><td  class='ccc1'>" + 王志敏 + "</td></tr>";
            html += "<tr><td>徐震雄</td><td  class='ccc1'>" + 徐震雄 + "</td></tr>";
            html += "<tr><td>陈琳</td><td  class='ccc1'>" + 陈琳 + "</td></tr>";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (Session["dtall"] != null)
            {
                DataTable dtall = (DataTable)Session["dtall"];
                if (txtcount.Text.Trim() != "" && dtall.Rows.Count > 0)
                {
                    if (int.Parse(txtcount.Text.Trim()) <= 500)
                    {
                        string fyuser = drf.SelectedValue;
                        string tyuser = drin.SelectedValue;
                        DataRow[] drfyuser = dtall.Select("yuse=" + fyuser);
                        string pidsssss = "";
                        for (int i = 0; i < drfyuser.Length; i++)
                        {
                            if (i == int.Parse(txtcount.Text.Trim()))
                            {
                                break;
                            }
                            else
                            {
                                pidsssss += drfyuser[i]["pid"] + ",";
                            }
                        }
                        if (pidsssss != "")
                        {
                            pidsssss = pidsssss.Substring(0, pidsssss.Length - 1);
                        }



                        access_sql.DoSql("update ALLGoodPro set yuse=" + tyuser + " where pid in(" + pidsssss + ")");
                        bind();
                        txtcount.Text = "";

                        lits.Text = "修改完成，已经重新加载统计";
                        Response.Write("<script>alert('" + lits.Text + "');</script>");
                    }
                    else
                    {
                        lits.Text = "数量不能大于500";
                        Response.Write("<script>alert('" + lits.Text + "');</script>");
                    }
                }
                else
                {
                    lits.Text = "请输入数量";
                    Response.Write("<script>alert('" + lits.Text + "');</script>");

                }
            }
            else
            {
                lits.Text = "请先加载数据";
                Response.Write("<script>alert('" + lits.Text + "');</script>");

            }

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            DataTable dtall = (DataTable)Session["dtall"];
            DataRow[] drfyuser = dtall.Select("yuse<>0");
            string pidsssss = "";
            for (int i = 0; i < drfyuser.Length; i++)
            {

                pidsssss += drfyuser[i]["pid"] + ",";

            }
            if (pidsssss != "")
            {
                pidsssss = pidsssss.Substring(0, pidsssss.Length - 1);
            }



            access_sql.DoSql("update ALLGoodPro set yuse=0 where pid in(" + pidsssss + ")");
        }
    }
}