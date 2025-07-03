using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 阿里狗拉黑链接 : System.Web.UI.Page
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
                if (uid != "12" && uid != "9" && uid != "13" && uid != "10" && uid != "15" && uid != "13" && uid != "19" && uid != "20")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            if (txtinoffid.Text.Trim() != "")
            {
                string[] urls = txtinoffid.Text.Trim().Replace("\r\n", "|").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                DataTable dtls = new DataTable();
                dtls.Columns.Add("offer_id");
                dtls.Columns.Add("pean");
                dtls.Columns.Add("jg");

                string offids = "";

                for (int i = 0; i < urls.Length; i++)
                {
                    string url = urls[i];
                    string offer_id = "";
                    if (url.IndexOf("-") != -1)
                    {
                        offer_id = (url.Split('-')[url.Split('-').Length - 1]).Trim();
                    }
                    else if (url.IndexOf("oferta") != -1)
                    {
                        offer_id = (url.Split('/')[url.Split('/').Length - 1]).Trim();
                    }
                    if (offer_id != "")
                    {
                        offids += "'" + offer_id + "',";
                    }
                }
                if (offids != "")
                {
                    offids = offids.Substring(0, offids.Length - 1);
                }
                string sql = "SELECT ALLGoodPro.pean,AllGM.SelfOfferID FROM ALLGoodPro left join  AllGM on ALLGoodPro.pean= AllGM.pean  where AllGM.SelfOfferID in(" + offids + ")";
                DataSet dsss = access_sql.GreatDs(sql);
                if (access_sql.yzTable(dsss))
                {
                    DataTable dt = dsss.Tables[0];
                    dt.Columns.Add("jg");
                    foreach (DataRow item in dt.Rows)
                    {
                        if (access_sql.T_Update_ExecSql(new string[] { "pw" }, new object[] { -108 }, "ALLGoodPro", "pean='" + item["pean"] + "'") != 0)
                        {
                            access_sql.T_Insert_ExecSql(new string[] { "pean", "des" }, new object[] { item["pean"], "主动拉黑" }, "qqEAN");
                            item["jg"] = "拉黑成功";
                        }
                        else
                        {
                            item["jg"] = "拉黑失败";
                        }
                    }
                    Repeater1.DataSource = dt;
                    Repeater1.DataBind();
                    lits.Text = "处理成功，请看结果";
                }
                else
                {
                    lits.Text = "未找到数据";

                }
            }
            else
            {
                lits.Text = "请输入搜索链接";
            }
        }
    }
}