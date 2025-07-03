using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class 物流审核 : System.Web.UI.Page
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
                if (uid != "9" && uid != "6")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy()
        {



        }



        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }






        protected void Button2_Click1(object sender, EventArgs e)
        {

        }

        protected void Button3_Click1(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DataSet ds = access_sql.GreatDs("select code,kefadehaiyun,ROW_NUMBER() OVER(PARTITION BY code ORDER BY code) AS rn from YNBigData where code is not null and code<>'' order by code");
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                DataRow[] codeList = dt.Select("rn=1");
                int cg = 0;
                int cgcode = 0;
                for (int i = 0; i < codeList.Length; i++)
                {
                    string code = codeList[i]["code"].ToString();
                    DataRow[] allrowsbycode = dt.Select("code='" + code + "'");
                    if (allrowsbycode.Length > 1)
                    {
                        string kefadehaiyun = "";
                        foreach (DataRow dr in allrowsbycode)
                        {
                            if (dr["kefadehaiyun"].ToString() != "" && dr["kefadehaiyun"].ToString() != "0" && dr["kefadehaiyun"].ToString().ToLower() != "null")
                            {
                                kefadehaiyun = dr["kefadehaiyun"].ToString();
                                break;
                            }
                        }
                        if (kefadehaiyun != "")
                        {
                            cgcode++;
                            cg += access_sql.T_Update_ExecSql(new string[] { "kefadehaiyun" }, new object[] { kefadehaiyun }, "YNBigData", "code='" + code + "'");
                        }

                    }

                }
                lits.Text = "成功同步" + cg + "个产品,随机码" + cgcode + "个";
            }

        }
        public DataTable dtcode = new DataTable();
        protected void Button2_Click2(object sender, EventArgs e)
        {

        }

        protected void Button2_Click3(object sender, EventArgs e)
        {
            DataSet ds = access_sql.GreatDs("select id,skuid,itemid,code,Quantity,kefadehaiyun,ROW_NUMBER() OVER(PARTITION BY code ORDER BY code) AS rn from [YNBigData] where code in(SELECT code FROM [YNBigData] group by code  HAVING SUM(Quantity)>=2) order by code");
            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];
                DataRow[] codeList = dt.Select("rn=1");
                int cg = 0;
               
                for (int i = 0; i < codeList.Length; i++)
                {
                    string code = codeList[i]["code"].ToString();
                    DataRow[] allrowsbycode = dt.Select("code='" + code + "'");

                    string kefadehaiyun = "";
                    foreach (DataRow dr in allrowsbycode)
                    {
                        if (dr["kefadehaiyun"].ToString() != "" && dr["kefadehaiyun"].ToString() != "0" && dr["kefadehaiyun"].ToString().ToLower() != "null")
                        {
                            kefadehaiyun = dr["kefadehaiyun"].ToString();
                            break;
                        }
                    }
                    if (kefadehaiyun == "")
                    {
                        cg++;
                    }


                }
                lits.Text = "查询到需审核的数量为" + cg + "个";
            }
        }
    }
}