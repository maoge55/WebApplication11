using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11
{
    public partial class fc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string purl = "";
        public string skuAttr = "";
        public string pid = "";
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtname.Text.Trim() != "" && txtsku.Text != "")
            {

                string name = txtname.Text.Trim().Replace("'", "''");
                string chanpinid = "";



                DataSet ds = access_sql.GreatDs("select * from product where biaoti like N'%" + name + "%'");
                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        string skuprice = dr["skuprice"].ToString();
                        JArray objskuprice = JsonConvert.DeserializeObject<JArray>(skuprice);
                        if (objskuprice != null && objskuprice.Count > 0)
                        {
                            foreach (var item in objskuprice)   //循环生成多个产品
                            {
                                if (item["skuId"] != null && item["skuAttr"] != null)
                                {
                                    if (item["skuId"].ToString() == txtsku.Text.Trim().Replace("A", ""))
                                    {
                                        chanpinid = dr["ChanPinID"].ToString();
                                        pid = dr["pid"].ToString();
                                        skuAttr = item["skuAttr"].ToString();
                                        break;

                                    }
                                }
                            }
                        }
                        if(chanpinid!="")
                        {
                            break;
                        }

                    }
                }


                if (chanpinid != "")
                {
                    jg.Visible = true;
                    lits.Text = "";
                    purl = "https://pl.aliexpress.com/item/" + chanpinid + ".html";
                    skuAttr = skuAttr.Replace(";", "<br>");    //拆分多个属性

                }
                else
                {
                    lits.Text = "无数据，请缩短标题字符再试";
                }
            }
            else
            {
                lits.Text = "标题和skuid不能为空";
            }
        }
    }
}