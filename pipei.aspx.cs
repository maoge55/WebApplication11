using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class pipei : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string pcount = access_sql.GetOneValue("select count(*) from ALLproduct where pmbid = 0  and pean is not null and LeiMu is not null");
            lits.Text = "需要匹配模板id的产品有" + pcount + "个";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            int cg = 0;
            int sb = 0;

            DataSet dsp = access_sql.GreatDs("select leimu,count(*) as count from ALLproduct where pmbid = 0  and pean is not null  and LeiMu is not null GROUP BY leimu");
            if (access_sql.yzTable(dsp))
            {
                DataTable dtp = dsp.Tables[0];
                for (int i = 0; i < dtp.Rows.Count; i++)
                {
                    DataRow dr = dtp.Rows[i];
                    string leimu = dr["leimu"].ToString();
                    if (leimu != "")
                    {
                       
                        DataSet dsm = access_sql.GreatDs("select * from mb where dname like '%(" + leimu + ")%'");
                        if (access_sql.yzTable(dsm))
                        {
                            string pmid = dsm.Tables[0].Rows[0]["did"].ToString();
                            access_sql.DoSql("update ALLproduct set pmbid=" + pmid + " where leimu=" + leimu + "");
                            cg = cg + int.Parse(dr["count"].ToString());
                        }
                        else
                        {
                            access_sql.DoSql("update ALLproduct set pmbid=-1  where leimu=" + leimu + "");
                            sb = sb + int.Parse(dr["count"].ToString());
                        }
                    }
                }
                lits.Text = "成功匹配" + cg + "，无匹配到模板" + sb;
            }
        }
    }
}