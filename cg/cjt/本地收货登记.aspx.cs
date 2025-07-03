using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace WebApplication11.cg.cjt
{
    public partial class 本地收货登记 : System.Web.UI.Page
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
                if (uid != "6" && uid != "12" && uid != "9" )
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }

        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(String yybm = "", string status = "", string yundanhao = "", string huopinbiaoti = "", string dingdanbianhao = "")
        {
            ViewState["yybm"] = yybm;
            ViewState["status"] = status;
            ViewState["yundanhao"] = yundanhao;
            ViewState["huopinbiaoti"] = huopinbiaoti;
            ViewState["dingdanbianhao"] = dingdanbianhao;
            string safeyybm = !string.IsNullOrEmpty(yybm) ? yybm.Replace("'", "''") : "";
        
            string safestatus = !string.IsNullOrEmpty(status) && status != "-1" ? status.Replace("'", "''") : "";
            string safeyundanhao = !string.IsNullOrEmpty(yundanhao) ? yundanhao.Replace("'", "''") : "";
            string safehuopinbiaoti = !string.IsNullOrEmpty(huopinbiaoti) ? huopinbiaoti.Replace("'", "''") : "";
            string safedingdanbianhao = !string.IsNullOrEmpty(dingdanbianhao) ? dingdanbianhao.Replace("'", "''") : "";

            string whereCondition = "";
            if (!string.IsNullOrEmpty(safeyybm))
                whereCondition += " AND so.YYBM= '" + safeyybm + "'";
            if (!string.IsNullOrEmpty(safestatus))
                whereCondition += " AND so.dingdanzhuangtai= '" + safestatus + "'";
            if (!string.IsNullOrEmpty(safeyundanhao))
                whereCondition += " AND so.YunDanHao= '" + safeyundanhao + "'";
            if (!string.IsNullOrEmpty(safehuopinbiaoti))
                whereCondition += " AND so.HuoPinBiaoTi like  '%" + safehuopinbiaoti + "%'";
            if (!string.IsNullOrEmpty(safedingdanbianhao))
                whereCondition += " AND so.DingDanBianHao= '" + safedingdanbianhao + "'";
            string sql = @" 
            Select 
            so.DingDanBianHao as [订单号],
            so.WuLiuGongSi as [物流公司],
            so.YunDanHao as [运单号],
            so.HuoPinBiaoTi as [标题],
            so.Skuid as [SKU_ID],
            so.sku_img as [SKU图片],
            so.ShuLiang as [数量],
            so.DanJia as [单价],
            so.YYBM as [运营编码],
            so.ShiJiShouHuoCount as [实际收货数量],
            so.CanCiPinCount as [残次品数量],
            so.DingDanBeiZhu as [订单备注],
            so.sjtype as [数据类型],
            so.pimage as [图片网址],
            so.video as [视频网址],
            so.Offerid as OfferID,
            so.dingdanzhuangtai as [订单状态],
            so.DanWei as [单位]
            from S1688Order so
            WHERE 1=1 " + whereCondition+ @";";

            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                DataTable dt = ds.Tables[0];

                rplb.DataSource = dt;
                rplb.DataBind();
            }
            else
            {
                rplb.DataSource = null;
                rplb.DataBind();
                lits.Text = "无数据";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string yybm = txtsjbm.Text.Trim();
            string status = ddlStatus.SelectedValue;
            string yundanhao = txtYundanhao.Text.Trim();    // 快递单号
            string huopinbiaoti = txtHuopinbiaoti.Text.Trim();  // 标题搜索词
            string dingdanbianhao = txtDingdanbianhao.Text.Trim();  // 1688单号

            bindzhy(
                yybm: yybm,
                status: status,
                yundanhao: yundanhao,
                huopinbiaoti: huopinbiaoti,
                dingdanbianhao: dingdanbianhao
            );
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "qr")
            {
                lits.Text = "";
                ulong SKU_ID = Convert.ToUInt64(e.CommandArgument);
                TextBox txtY_true = e.Item.FindControl("txtY_true") as TextBox;

                TextBox txtY_pool = e.Item.FindControl("txtY_pool") as TextBox;
                TextBox txtY_ding = e.Item.FindControl("txtY_ding") as TextBox;
                DropDownList dingdanzhuangtai = e.Item.FindControl("dingdanzhuangtai") as DropDownList;
                DropDownList shujuleixing = e.Item.FindControl("shujuleixing") as DropDownList;


                Literal dingdanbianhaoLit = e.Item.FindControl("订单号") as Literal;
   
                string dingdanbianhao = dingdanbianhaoLit.Text ;


           if( access_sql.T_Update_ExecSql(new string[] { "ShiJiShouHuoCount", "CanCiPinCount", "DingDanBeiZhu", "sjtype", "dingdanzhuangtai" },
             new object[] { txtY_true.Text.Trim().Replace("'", "''"), txtY_pool.Text.Trim().Replace("'", "''"), txtY_ding.Text.Trim().Replace("'", "''"), shujuleixing.SelectedValue, dingdanzhuangtai.SelectedValue },
             "S1688Order",
            "Skuid='" + SKU_ID + "' AND DingDanBianHao='" + dingdanbianhao.Replace("'", "''") + "'") > 0)
                {
                    bindzhy(ViewState["yybm"] as string,
                    ViewState["status"] as string,
                    ViewState["yundanhao"] as string,
                    ViewState["huopinbiaoti"] as string,
                    ViewState["dingdanbianhao"] as  string);
                    lits.Text = "Skuid:" + SKU_ID + "更新成功";
                }
                //插到残次品表
                if (!string.IsNullOrEmpty(txtY_pool.Text))
                {
                    if (Convert.ToInt32(txtY_pool.Text) > 0)
                    {
                        int cancipinShu;
                        if (!int.TryParse(txtY_pool.Text.Trim(), out cancipinShu))
                        {
                            lits.Text += " | 残次品数量必须为整数";
                            return;
                        }

                        // 获取单价
                        string danjiaStr = access_sql.GetOneValue("SELECT DanJia FROM S1688Order WHERE Skuid='" + SKU_ID + "'");
                        float danjia;
                        if (!float.TryParse(danjiaStr, out danjia))
                        {
                            lits.Text += " | 单价数据格式错误";
                            return;
                        }
                        string insertSql = string.Format(@"
        IF NOT EXISTS (
            SELECT 1 
            FROM cancipin 
            WHERE SKU_ID = '{0}' 
            AND cancipinshuliang = {1}
        )
        BEGIN
            INSERT INTO cancipin (
                dingdanbianhao,
                wuliugongsi_1688,
                yundanhao_1688,
                huopinbiaoti,
                SKU_ID,
                SKU_Picture,
                danwei,
                cancipinshuliang,
                danjia,
                tuikuanjiner,
                tuihuan_date,
                shangjiabianma
            )
            SELECT 
                so.DingDanBianHao,
                so.WuLiuGongSi,
                so.YunDanHao,
                so.HuoPinBiaoTi,
                so.Skuid,
                so.sku_img,
                so.DanWei,
                {1},
                so.DanJia,
               so.DanJia * {1},
                GETDATE(),
                so.YYBM
            FROM S1688Order so

            WHERE so.Skuid = '{0}'
        END",
                            SKU_ID, // {0}
                            cancipinShu // {1}
                        );

                        // 执行SQL
                        if (access_sql.ExecSql(insertSql))
                        {
                            lits.Text += " | 残次品记录已添加";
                        }
                        else
                        {
                            lits.Text += " | 残次品记录添加失败";
                        }
                    }
                }
            }
            if (e.CommandName == "upimg")
            {
                lits.Text = "";
                string SKU_ID = (e.CommandArgument).ToString();
                FileUpload ff = e.Item.FindControl("FileUpload1") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList1") as DataList;
                string imgs = access_sql.GetOneValue("select top 1 pimage from S1688Order where Skuid='" + SKU_ID + "'") + "|";
                for (int i = 0; i < ff.PostedFiles.Count; i++)
                {
                    string type = ff.PostedFiles[i].FileName.Split('.')[ff.PostedFiles[i].FileName.Split('.').Length - 1];
                    string imgname = DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + type;
                    string savename = Server.MapPath("~/Uploads/") + imgname;
                    ff.PostedFiles[i].SaveAs(savename);
                    imgs += imgname + "|";
                }
                if (imgs != "")
                {
                    imgs = imgs.Replace("||", "|");
                    access_sql.T_Update_ExecSql(new string[] { "pimage" }, new object[] { imgs }, "S1688Order", "Skuid='" + SKU_ID + "'");
               
                 
                    bindimgs(liimgxs, imgs, SKU_ID);
                    lits.Text = "图片上传成功";
                }
            }
            if (e.CommandName == "upsp")
            {
                lits.Text = "";
                string SKU_ID = (e.CommandArgument).ToString();
                FileUpload ff = e.Item.FindControl("FileUpload2") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList2") as DataList;
                string imgs = access_sql.GetOneValue("select top 1 video from S1688Order where Skuid='" + SKU_ID + "'") + "|";
                for (int i = 0; i < ff.PostedFiles.Count; i++)
                {
                    string type = ff.PostedFiles[i].FileName.Split('.')[ff.PostedFiles[i].FileName.Split('.').Length - 1];
                    string imgname = DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + type;
                    string savename = Server.MapPath("~/Uploads/") + imgname;
                    ff.PostedFiles[i].SaveAs(savename);
                    imgs += imgname + "|";
                }
                if (imgs != "")
                {
                    imgs = imgs.Replace("||", "|");
                    access_sql.T_Update_ExecSql(new string[] { "video" }, new object[] { imgs }, "S1688Order", "Skuid='" + SKU_ID + "'");
                    // 仅刷新当前项的视频预览（无需全量bindzhy）
          
                    bindimgs(liimgxs, imgs, SKU_ID);
                    lits.Text = "视频上传成功";
                }
            }
            if (e.CommandName == "upskuimg")
            {
                lits.Text = "";
                string SKU_ID = e.CommandArgument.ToString();
                FileUpload ff = e.Item.FindControl("FileUpload3") as FileUpload;
                string imgPaths = "";

                for (int i = 0; i < ff.PostedFiles.Count; i++)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + i + ".jpg";
                    string savePath = Server.MapPath("~/Uploads/") + fileName;
                    ff.PostedFiles[i].SaveAs(savePath);
                    imgPaths += "~/Uploads/" + fileName + "|";
                }

                if (!string.IsNullOrEmpty(imgPaths))
                {
                    imgPaths = imgPaths.TrimEnd('|');
                    access_sql.T_Update_ExecSql(
                        new string[] { "sku_img" },
                        new object[] { imgPaths },
                        "S1688Order",
                        "Skuid='" + SKU_ID + "'"
                    );
                    bindzhy(ViewState["yybm"] as string,
   ViewState["status"] as string,
   ViewState["yundanhao"] as string,
   ViewState["huopinbiaoti"] as string,
   ViewState["dingdanbianhao"] as string);
                    lits.Text = "SKU图片上传成功";
                }
            }

        }

        protected void rplb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Literal litSKU_ID = e.Item.FindControl("SKU_ID") as Literal;
                Literal litTupian = e.Item.FindControl("litTupianwangzhi") as Literal;
                DataList dlImages = e.Item.FindControl("DataList1") as DataList;

                Literal litShipin = e.Item.FindControl("litShipinwangzhi") as Literal;
                DataList dlVideos = e.Item.FindControl("DataList2") as DataList;

                // 绑定图片
                if (litTupian != null && !string.IsNullOrEmpty(litTupian.Text))
                {
                    bindimgs(dlImages, litTupian.Text, litSKU_ID.Text);
                }

                // 绑定视频
                if (litShipin != null && !string.IsNullOrEmpty(litShipin.Text))
                {
                    bindimgs(dlVideos, litShipin.Text, litSKU_ID.Text);
                }


            }
        }

        private void bindSKUImages(DataList dl, string images)
        {
            string[] imgArr = images.Split('|');
            DataTable dt = new DataTable();
            dt.Columns.Add("imgname");

            foreach (string img in imgArr)
            {
                if (!string.IsNullOrEmpty(img))
                    dt.Rows.Add(img);
            }

            dl.RepeatColumns = dt.Rows.Count;
            dl.DataSource = dt;
            dl.DataBind();
        }

        public void clzy()
        {
            int cg = 0;


            for (int i = 0; i < rplb.Items.Count; i++)
            {
                Literal SKU_ID = (Literal)rplb.Items[i].FindControl("SKU_ID");

                String id = SKU_ID.Text;
                TextBox txtY_true = rplb.Items[i].FindControl("txtY_true") as TextBox;

                TextBox txtY_pool = rplb.Items[i].FindControl("txtY_pool") as TextBox;
                TextBox txtY_ding = rplb.Items[i].FindControl("txtY_ding") as TextBox;
                DropDownList dingdanzhuangtai = rplb.Items[i].FindControl("dingdanzhuangtai") as DropDownList;
                DropDownList shujuleixing = rplb.Items[i].FindControl("shujuleixing") as DropDownList;


                Literal dingdanbianhaoLit = (Literal)rplb.Items[i].FindControl("订单号") as Literal;

                string dingdanbianhao = dingdanbianhaoLit.Text;


                cg += access_sql.T_Update_ExecSql(new string[] { "ShiJiShouHuoCount", "CanCiPinCount", "DingDanBeiZhu", "sjtype", "dingdanzhuangtai" },
                  new object[] { txtY_true.Text.Trim().Replace("'", "''"), txtY_pool.Text.Trim().Replace("'", "''"), txtY_ding.Text.Trim().Replace("'", "''"), shujuleixing.SelectedValue, dingdanzhuangtai.SelectedValue },
                  "S1688Order",
                 "Skuid='" + id + "' AND DingDanBianHao='" + dingdanbianhao.Replace("'", "''") + "'");
                 
         
            }
            bindzhy(ViewState["yybm"] as string,
   ViewState["status"] as string,
   ViewState["yundanhao"] as string,
   ViewState["huopinbiaoti"] as string,
   ViewState["dingdanbianhao"] as string);
            lits.Text = "更新成功" + cg + "个";

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal liimgSKU_ID = e.Item.FindControl("liimgSKU_ID") as Literal;
                string ddimg = access_sql.GetOneValue("select pimage from S1688Order where Skuid='" + liimgSKU_ID.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "pimage" }, new object[] { ddimg }, "S1688Order", "Skuid='" + liimgSKU_ID.Text + "'");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList1") as DataList;
                bindimgs(liimgxs, ddimg, liimgSKU_ID.Text);
            }

        }
        protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal liimgSKU_ID = e.Item.FindControl("liimgSKU_ID") as Literal;
                string ddimg = access_sql.GetOneValue("select video from S1688Order where Skuid='" + liimgSKU_ID.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "video" }, new object[] { ddimg }, "S1688Order", "Skuid='" + liimgSKU_ID.Text + "'");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList2") as DataList;
                bindimgs(liimgxs, ddimg, liimgSKU_ID.Text);
            }

        }
        public void bindimgs(DataList liimgxs, string imgs, string cid)
        {
            string[] iiii = imgs.Split('|');
            DataTable dtimgs = new DataTable();
            dtimgs.Columns.Add("SKU_ID");
            dtimgs.Columns.Add("imgname");

            for (int i = 0; i < iiii.Length; i++)
            {
                if (iiii[i] != "")
                {
                    dtimgs.Rows.Add(new object[] { cid, iiii[i] });
                }
            }
            if (dtimgs.Rows.Count > 0)
            {
                liimgxs.RepeatColumns = dtimgs.Rows.Count;
                liimgxs.DataSource = dtimgs;
                liimgxs.DataBind();
            }
            else
            {
                liimgxs.DataSource = null;
                liimgxs.DataBind();
            }
        }



    }
}