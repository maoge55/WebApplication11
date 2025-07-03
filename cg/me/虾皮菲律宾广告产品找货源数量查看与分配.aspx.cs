using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 虾皮菲律宾广告产品找货源数量查看与分配 : System.Web.UI.Page
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
          
            string 王志敏 = "";
           
            string sql = "select *  from ShopeePHADPro where month_sold>30 and (price>500 and price<1200) and (y_1688url is null or y_1688url='') and is_cj=1";
            DataSet all = access_sql.GreatDs(sql.Replace("{0}", ""));

            if (access_sql.yzTable(all))
            {
                DataTable dtall = all.Tables[0];
                Session["dtall"] = dtall;
                无人 = dtall.Select("yuse=0").Length.ToString();
                郑雨蝶 = dtall.Select("yuse=6").Length.ToString();
              
                王志敏 = dtall.Select("yuse=19").Length.ToString();
             
            }


            html += "<tr><td>无人</td><td  class='ccc1'>" + 无人 + "</td></tr>";
            html += "<tr><td>郑雨蝶</td><td class='ccc1'>" + 郑雨蝶 + "</td></tr>";
          
            html += "<tr><td>王志敏</td><td  class='ccc1'>" + 王志敏 + "</td></tr>";
          
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
                                pidsssss += drfyuser[i]["id"] + ",";
                            }
                        }
                        if (pidsssss != "")
                        {
                            pidsssss = pidsssss.Substring(0, pidsssss.Length - 1);
                        }



                        access_sql.DoSql("update ShopeePHADPro set yuse=" + tyuser + " where id in(" + pidsssss + ")");
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

                pidsssss += drfyuser[i]["id"] + ",";

            }
            if (pidsssss != "")
            {
                pidsssss = pidsssss.Substring(0, pidsssss.Length - 1);
            }



            access_sql.DoSql("update ShopeePHADPro set yuse=0 where id in(" + pidsssss + ")");
        }
    }
}