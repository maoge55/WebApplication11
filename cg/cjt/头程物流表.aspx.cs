using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WebApplication11.cg.cjt
{
    public partial class 头程物流表 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (!access_sql.yzdlcg())
            {
                Response.Redirect("/cg/clogin.aspx");
            }
            else
            {
                u = HttpContext.Current.Request.Cookies["cu"].Value;
                p = HttpContext.Current.Request.Cookies["cp"].Value;
                uid = HttpContext.Current.Request.Cookies["cuid"].Value;
                if (uid != "6" && uid != "12" && uid != "9" && uid!="22")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
                if (!IsPostBack)
                {

                }
            }
        }
        public string u = "";
        public string p = "";
        public string uid = "";

        public void bindzhy(string yybm = "")
        {

       
            // 获取海外仓入库单号（从会话获取首次填写值）
            string haiwaicangRukuCode = Session["FirstFill_HaiwaicangRukuCode"] != null ? Session["FirstFill_HaiwaicangRukuCode"].ToString() : "";
            string yunyinbianma = yybm.Trim();

            // 转义特殊字符防止SQL注入
            string safeYybm = yunyinbianma.Replace("'", "''");
            string safeHaiwaicang = haiwaicangRukuCode.Replace("'", "''");

            // 构建查询SQL（关联yunyinbianma和haiwaicangrukudanhao字段）
            string sql = @"SELECT DISTINCT haiwaicangxitongbianma, putianfachushuliang, haiwaicangrukudanhao "
                          + "FROM touchenwuliu "
                          + "WHERE yunyinbianma = '" + safeYybm + "' "
                          + "AND haiwaicangrukudanhao = '" + safeHaiwaicang + "'";

            // 执行查询并绑定数据
            DataSet ds = access_sql.GreatDs(sql);
            DataTable dt = access_sql.yzTable(ds) ? ds.Tables[0] : new DataTable();

            // 初始化表结构（如果无数据或列缺失） 
            if (dt.Rows.Count == 0)
            {
                // 仅当列不存在时添加 
                if (!dt.Columns.Contains("haiwaicangxitongbianma")) dt.Columns.Add("haiwaicangxitongbianma", typeof(string));
                if (!dt.Columns.Contains("putianfachushuliang")) dt.Columns.Add("putianfachushuliang", typeof(string));
                if (!dt.Columns.Contains("beizhu")) dt.Columns.Add("beizhu", typeof(string));
                // 确保添加50行空数据 
                for (int i = dt.Rows.Count; i < 50; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }

            rplb.DataSource = dt;
            rplb.DataBind();

            // 保持会话初始化逻辑
            Session["txtCommonPtsj"] = null;
            Session["txtCommonHaiwaicangRuku"] = null;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            bindzhy(txtsjbm.Text.Trim());

        }

        protected void rplb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "qr")
            {
                TextBox txtY_haiwaicangxitongbianma = e.Item.FindControl("txtY_haiwaicangxitongbianma") as TextBox;
                TextBox txtY_ptfachu = e.Item.FindControl("txtY_ptfachu") as TextBox;  // 仅保留前端存在的控件

                // 直接使用公共字段值（前端存在的公共控件）
                string ptsj = txtCommonPtsj.Text.Trim();

                string haiwaicangRukuCode = txtCommonHaiwaicangRuku.Text.Trim();

                string yunyinbianma = txtsjbm.Text.Trim();


                if (Session["FirstFill_PtTime"] == null)
                {
                    Session["FirstFill_PtTime"] = txtCommonPtsj.Text.Trim();


                    Session["FirstFill_HaiwaicangRukuCode"] = txtCommonHaiwaicangRuku.Text.Trim();
                }


                string[] columns = {
                    "haiwaicangxitongbianma",
                    "yunyinbianma",
                    "putianfachushuliang",
                    "putianfachushijian",

                    "haiwaicangrukudanhao"
                };
                object[] values = {
                        txtY_haiwaicangxitongbianma.Text.Trim(),
                        yunyinbianma,
                        txtY_ptfachu.Text.Trim(),
                        ptsj,  // 使用公共字段值
                        haiwaicangRukuCode  // 使用公共字段值
                    };

                if (access_sql.T_Insert_ExecSqls(columns, values, "touchenwuliu"))
                {
                    lits.Text = "保存成功";
                    bindzhy(yunyinbianma);
                }
                else
                {
                    lits.Text = "保存失败，请检查数据格式";
                }
            }
        }
        protected void btnPutian_Click(object sender, EventArgs e)
        {
            string yybm = txtsjbm.Text.Trim();
            if (string.IsNullOrEmpty(yybm))
            {
                lits.Text = "请先输入运营编码";
                return;
            }

            // 修改为字符串拼接方式
            string yunyinbianma = "";
            if (!string.IsNullOrEmpty(yybm))
            {
                yunyinbianma = " AND yunyinbianma = '" + yybm + "'";
            }
            string sql = @"
                WITH cte AS (
                    SELECT *, 
                           ROW_NUMBER() OVER (PARTITION BY haiwaicangrukudanhao ORDER BY putianfachushijian DESC, haiwaicangxitongbianma DESC, putianfachushuliang DESC) AS rn
                    FROM touchenwuliu 
                    WHERE 1=1" + yunyinbianma + @"
                      AND (ISNULL(putianfachushijian, '') <> '' AND ISNULL(putianfachukuaididanghao, '') = '')
                )
                SELECT haiwaicangxitongbianma, haiwaicangrukudanhao,putianfachushijian, putianfachukuaididanghao, kuaidifei,beizhu ,putianfachushuliang
                FROM cte 
                WHERE rn = 1
                ORDER BY haiwaicangrukudanhao";

            DataSet ds = access_sql.GreatDs(sql);





            if (access_sql.yzTable(ds))
            {
                rptPutianPending.DataSource = ds.Tables[0];
                rptPutianPending.DataBind();
                divPutianModal.Style["display"] = "block";
            }
            else
            {
                lits.Text = "没有需要补全的发出信息";
                divPutianModal.Style["display"] = "none";
            }

        }

        protected void rptPutianPending_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "savePutian")
            {
                string haiwaicangrukudanhao = e.CommandArgument.ToString();
                TextBox txtKddh = e.Item.FindControl("txtKddh") as TextBox;
                TextBox txtPrice = e.Item.FindControl("txtPrice") as TextBox;
                TextBox txtPtBeizhu = e.Item.FindControl("txtPtBeizhu") as TextBox;
                if (txtKddh == null || txtPrice == null)
                {
                    lits.Text = "获取控件失败";
                    return;
                }

                string safeKddh = txtKddh.Text.Trim().Replace("'", "''");
                string safePrice = txtPrice.Text.Trim().Replace("'", "''");
                string safeBeizhu = txtPtBeizhu.Text.Trim().Replace("'", "''");
                string safehaiwaicangrukudanhao = haiwaicangrukudanhao.Replace("'", "''");


                string updateSql = string.Format(@"
                    UPDATE touchenwuliu 
                    SET putianfachukuaididanghao = '{0}', kuaidifei = '{1}' ,beizhu='{2}'
                    WHERE haiwaicangrukudanhao = '{3}'", safeKddh, safePrice, safeBeizhu, safehaiwaicangrukudanhao);

                if (access_sql.ExecSql(updateSql))
                {
                    lits.Text = "保存成功";
                    btnPutian_Click(source, e); // 刷新模态框数据
                }
                else
                {
                    lits.Text = "保存失败";
                }
            }
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            divPutianModal.Style["display"] = "none";
        }

        public void clzy()
        {

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            clzy();
        }

        // 新增全部保存按钮点击事件
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            string yunyinbianma = txtsjbm.Text.Trim();
            if (string.IsNullOrEmpty(yunyinbianma))
            {
                lits.Text = "请先输入运营编码";
                return;
            }

            // 获取公共字段值
            string ptsj = txtCommonPtsj.Text.Trim();

            string haiwaicangRukuCode = txtCommonHaiwaicangRuku.Text.Trim();

            if (string.IsNullOrEmpty(haiwaicangRukuCode))
            {
                lits.Text = "请填写完整的公共字段信息";
                return;
            }

            bool allSuccess = true;
            foreach (RepeaterItem item in rplb.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtY_haiwaicangxitongbianma = item.FindControl("txtY_haiwaicangxitongbianma") as TextBox;
                    TextBox txtY_ptfachu = item.FindControl("txtY_ptfachu") as TextBox;

                    if (txtY_haiwaicangxitongbianma == null || txtY_ptfachu == null)
                    {
                        allSuccess = false;
                        continue;
                    }

                    string haiwaicangxitongbianma = txtY_haiwaicangxitongbianma.Text.Trim();
                    string fachu = txtY_ptfachu.Text.Trim();

                    if (string.IsNullOrEmpty(haiwaicangxitongbianma) || string.IsNullOrEmpty(fachu))
                    {
                        allSuccess = false;
                        continue;
                    }

                    string[] columns = {
                        "haiwaicangxitongbianma",
                        "yunyinbianma",
                        "putianfachushuliang",
                        "putianfachushijian",

                        "haiwaicangrukudanhao"
                    };
                    object[] values = {
                        haiwaicangxitongbianma,
                        yunyinbianma,
                        fachu,
                        ptsj,

                        haiwaicangRukuCode
                    };

                    if (!access_sql.T_Insert_ExecSqls(columns, values, "touchenwuliu"))
                    {
                        allSuccess = false;
                    }
                }
            }

            if (allSuccess)
            {
                lits.Text = "全部保存成功";
                bindzhy(yunyinbianma);
            }
            else
            {
                lits.Text = "输入框还有未填写行";
            }
        }

        // 新增广东发出信息查询方法
        protected void btnGuangdong_Click(object sender, EventArgs e)
        {
            string yybm = txtsjbm.Text.Trim();
            string haiwaiCangRuKu = txtHaiwaiCangRuKu.Text.Trim();
            if (string.IsNullOrEmpty(yybm))
            {
                lits.Text = "请先输入运营编码";
                return;
            }

            string safeYybm = yybm.Replace("'", "''");
            string safehaiwaiCangRuKu = haiwaiCangRuKu.Replace("'", "''");

            string warehouseCondition = "";
            if (!string.IsNullOrEmpty(safehaiwaiCangRuKu))
            {
                warehouseCondition = string.Format(" AND haiwaicangrukudanhao = '{0}'", safehaiwaiCangRuKu);
            }

            // 保持原始SQL结构
            string sql = string.Format(@"
        WITH cte AS ( 
            SELECT *,  
            ROW_NUMBER() OVER (PARTITION BY haiwaicangrukudanhao ORDER BY putianfachushijian DESC) AS rn 
            FROM touchenwuliu 
            WHERE yunyinbianma = '{0}'
                AND ISNULL(putianfachukuaididanghao, '') <> '' 
                AND ISNULL(guandongfachuyundanhao, '') = ''
                {1}  
        )
        SELECT 
            haiwaicangrukudanhao, 
            guandongfachuyundanhao, 
            touchenwuliufei,
            beizhu, 
            touchenwuliushan,
            putianfachushijian
        FROM cte 
        WHERE rn = 1 
        ORDER BY haiwaicangrukudanhao, putianfachushijian",
                safeYybm,
                warehouseCondition);

            DataSet ds = access_sql.GreatDs(sql);

            if (access_sql.yzTable(ds))
            {
                rptGuangdongPending.DataSource = ds.Tables[0];
                rptGuangdongPending.DataBind();
                divGuangdongModal.Style["display"] = "block";
            }
            else
            {
                lits.Text = "没有需要补全的广东发出信息";
                divGuangdongModal.Style["display"] = "none";
            }
        }

        // 新增广东发出信息保存方法
        protected void rptGuangdongPending_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "saveGuangdong")
            {
                string haiwaicangrukudanhao = e.CommandArgument.ToString();
                TextBox txtGdYundanhao = e.Item.FindControl("txtGdYundanhao") as TextBox;
                TextBox txtTlFei = e.Item.FindControl("txtTlFei") as TextBox;
                DropDownList touchenwuliushan = e.Item.FindControl("touchenwuliushan") as DropDownList;
                TextBox txtGdBeizhu = e.Item.FindControl("txtGdBeizhu") as TextBox;
                if (txtGdYundanhao == null || txtTlFei == null)
                {
                    lits.Text = "获取控件失败";
                    return;
                }


                string safeGdYundanhao = txtGdYundanhao.Text.Trim().Replace("'", "''");
                string safeTlFei = txtTlFei.Text.Trim().Replace("'", "''");
                string safehaiwaicangrukudanhao = haiwaicangrukudanhao.Replace("'", "''");
                string safetouchenwuliushan = touchenwuliushan.SelectedValue;
                string safeBeizhu = txtGdBeizhu.Text.Trim().Replace("'", "''");
                string updateSql = string.Format(@"
                    UPDATE touchenwuliu 
                    SET guandongfachuyundanhao = '{0}', 
                        touchenwuliufei = '{1}' ,
                        touchenwuliushan='{2}',
                        beizhu='{3}'
                    WHERE haiwaicangrukudanhao = '{4}'
                     ", safeGdYundanhao, safeTlFei, safetouchenwuliushan, safeBeizhu, safehaiwaicangrukudanhao);

                if (access_sql.ExecSql(updateSql))
                {
                    lits.Text = "保存成功";
                    btnGuangdong_Click(source, e);
                }
                else
                {
                    lits.Text = "保存失败";
                }


            }
            if (e.CommandName == "upimg")
            {
                lits.Text = "";
                string haiwaicangrukudanhao = e.CommandArgument.ToString();
                FileUpload ff = e.Item.FindControl("FileUpload2") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList2") as DataList;
                string imgs = access_sql.GetOneValue("select top 1 guangdongtupian from touchenwuliu where haiwaicangrukudanhao='" + haiwaicangrukudanhao + "'") + "|";
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
                    access_sql.T_Update_ExecSql(new string[] { "guangdongtupian" }, new object[] { imgs }, "touchenwuliu", "haiwaicangrukudanhao='" + haiwaicangrukudanhao + "'");
                    bindimgs(liimgxs, imgs, haiwaicangrukudanhao);
                }

            }
        }

        protected void btnCloseGdModal_Click(object sender, EventArgs e)
        {
            divGuangdongModal.Style["display"] = "none";
        }

        // 新增海外已发出按钮点击事件
        protected void btnHaiwai_Click(object sender, EventArgs e)
        {
            string yybm = txtsjbm.Text.Trim();
            if (string.IsNullOrEmpty(yybm))
            {
                lits.Text = "请先输入运营编码";
                return;
            }

            string safeYybm = yybm.Replace("'", "''");


            string sql = string.Format(@"WITH cte AS (SELECT *, 
ROW_NUMBER() OVER (PARTITION BY haiwaicangrukudanhao ORDER BY putianfachushijian DESC) AS rn
FROM touchenwuliu 
WHERE yunyinbianma = '{0}' 
AND ISNULL(guandongfachuyundanhao, '') <> '' 
AND (fahuozhuangtai != '发货完成'  OR ISNULL(fahuozhuangtai, '') = '')  )
SELECT haiwaicangrukudanhao,fahuozhuangtai ,putianfachushijian,guandongfachuyundanhao,touchenwuliushan,touchenwuliufei
FROM cte  WHERE rn = 1
ORDER BY haiwaicangrukudanhao, putianfachushijian,guandongfachuyundanhao,touchenwuliushan,touchenwuliufei", safeYybm);

            DataSet ds = access_sql.GreatDs(sql);
            if (access_sql.yzTable(ds))
            {
                rptHaiwaiPending.DataSource = ds.Tables[0];
                rptHaiwaiPending.DataBind();
                divHaiwaiModal.Style["display"] = "block";
            }
            else
            {
                lits.Text = "没有需要补全的海外仓信息";
                divHaiwaiModal.Style["display"] = "none";
            }
        }

        protected void rptHaiwaiPending_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "saveHaiwai")
            {
                string haiwaicangrukudanhao = e.CommandArgument.ToString();

                TextBox txtShoudao = e.Item.FindControl("txtShoudao") as TextBox;
                TextBox txtDiuShi = e.Item.FindControl("txtDiuShi") as TextBox;
                TextBox txtHaiwaiBeizhu = e.Item.FindControl("txtHaiwaiBeizhu") as TextBox;
                DropDownList fahuozhuangtai = e.Item.FindControl("fahuozhuangtai") as DropDownList;

                TextBox touchenwuliufei = e.Item.FindControl("touchenwuliufei") as TextBox;
                string safeShoudao = txtShoudao.Text.Trim().Replace("'", "''");
                string safeDiuShi = txtDiuShi.Text.Trim().Replace("'", "''");
                string safehaiwaicangrukudanhao = haiwaicangrukudanhao.Replace("'", "''");
                string safeHaiwaiBeizhu = txtHaiwaiBeizhu.Text.Trim().Replace("'", "''");
                string safefahuozhuangtai = fahuozhuangtai.SelectedValue;
                string safetouchenwuliufei = touchenwuliufei.Text.Trim().Replace("'", "''");
                string updateSql = string.Format(@"
                    UPDATE touchenwuliu 
                    SET 
                        haiwaicangshoudaoshuliang = '{0}', 
                        diushishuliang = '{1}',
                        beizhu='{2}',
                          fahuozhuangtai='{3}',
                        touchenwuliufei='{4}'
                    WHERE haiwaicangrukudanhao = '{5}'", safeShoudao, safeDiuShi, safeHaiwaiBeizhu, safefahuozhuangtai, safetouchenwuliufei, safehaiwaicangrukudanhao);

                if (access_sql.ExecSql(updateSql))
                {
                    lits.Text = "保存成功";
                    btnHaiwai_Click(source, e); // 刷新模态框数据
                }
                else
                {
                    lits.Text = "保存失败";
                }
            }

            if (e.CommandName == "upimg")
            {
                lits.Text = "";
                string haiwaicangrukudanhao = (e.CommandArgument).ToString();
                FileUpload ff = e.Item.FindControl("FileUpload1") as FileUpload;
                DataList liimgxs = e.Item.FindControl("DataList1") as DataList;
                string imgs = access_sql.GetOneValue("select top 1 shangchuangtupian from touchenwuliu where haiwaicangrukudanhao='" + haiwaicangrukudanhao + "'") + "|";
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
                    access_sql.T_Update_ExecSql(new string[] { "shangchuangtupian" }, new object[] { imgs }, "touchenwuliu", "haiwaicangrukudanhao='" + haiwaicangrukudanhao + "'");
                    bindimgs(liimgxs, imgs, haiwaicangrukudanhao);
                }

            }

        }

        // 新增莆田全部保存按钮事件
        protected void btnSaveAllPutian_Click(object sender, EventArgs e)
        {
            bool allSuccess = true;
            foreach (RepeaterItem item in rptPutianPending.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtKddh = item.FindControl("txtKddh") as TextBox;
                    TextBox txtPrice = item.FindControl("txtPrice") as TextBox;
                    TextBox txtPtBeizhu = item.FindControl("txtPtBeizhu") as TextBox;
                    if (txtKddh == null || txtPrice == null) continue;

                    string safeKddh = txtKddh.Text.Trim().Replace("'", "''");
                    string safePrice = txtPrice.Text.Trim().Replace("'", "''");
                    string safeBeizhu = txtPtBeizhu.Text.Trim().Replace("'", "''");
                    string haiwaicangrukudanhao = (item.FindControl("haiwaicangrukudanhao") as Literal).Text.Trim().Replace("'", "''");

                    string updateSql = string.Format(@"
                        UPDATE touchenwuliu 
                        SET putianfachukuaididanghao = '{0}', kuaidifei = '{1}',beizhu='{2}' 
                        WHERE haiwaicangrukudanhao = '{3}'", safeKddh, safePrice, safeBeizhu, haiwaicangrukudanhao);

                    if (!access_sql.ExecSql(updateSql)) allSuccess = false;
                }
            }
            lits.Text = allSuccess ? "莆田全部保存成功" : "莆田部分保存失败";
            btnPutian_Click(sender, e); // 刷新模态框
        }

        // 新增广东全部保存按钮事件
        protected void btnSaveAllGuangdong_Click(object sender, EventArgs e)
        {
            bool allSuccess = true;
            foreach (RepeaterItem item in rptGuangdongPending.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtGdYundanhao = item.FindControl("txtGdYundanhao") as TextBox;
                    TextBox txtTlFei = item.FindControl("txtTlFei") as TextBox;
                    DropDownList touchenwuliushan = item.FindControl("touchenwuliushan") as DropDownList;
                    TextBox txtGdBeizhu = item.FindControl("txtGdBeizhu") as TextBox;
                    if (txtGdYundanhao == null || txtTlFei == null) continue;

                    string safeGdYundanhao = txtGdYundanhao.Text.Trim().Replace("'", "''");
                    string safeTlFei = txtTlFei.Text.Trim().Replace("'", "''");
                    string safetouchenwuliushan = touchenwuliushan.SelectedValue;
                    string haiwaicangrukudanhao = (item.FindControl("haiwaicangrukudanhao") as Literal).Text.Trim().Replace("'", "''");
                    string safeBeizhu = txtGdBeizhu.Text.Trim().Replace("'", "''");
                    string updateSql = string.Format(@"
                        UPDATE touchenwuliu 
                        SET guandongfachuyundanhao = '{0}', touchenwuliufei = '{1}',touchenwuliushan='{2}',beizhu='{3}' 
                        WHERE haiwaicangrukudanhao = '{4}'", safeGdYundanhao, safeTlFei, safetouchenwuliushan, safeBeizhu, haiwaicangrukudanhao);

                    if (!access_sql.ExecSql(updateSql)) allSuccess = false;
                }
            }
            lits.Text = allSuccess ? "广东全部保存成功" : "广东部分保存失败";
            btnGuangdong_Click(sender, e); // 刷新模态框
        }

        // 新增莆田准备发出按钮点击事件
        protected void btnPutianLeft_Click(object sender, EventArgs e)
        {
            string haiwaiCangRuKu = txtHaiwaiCangRuKu.Text.Trim();
            if (string.IsNullOrEmpty(haiwaiCangRuKu))
            {
                lits.Text = "<script>alert('请输入海外仓入库单号！');</script>";
                return;
            }


            string safeHaiWaiCangRuKu = haiwaiCangRuKu.Replace("'", "''");
            string sql = string.Format("SELECT haiwaicangxitongbianma, putianfachushuliang, beizhu FROM touchenwuliu WHERE haiwaicangrukudanhao = '{0}' AND (putianfachushijian IS NULL OR putianfachushijian = '')", safeHaiWaiCangRuKu);

            {
                DataSet ds = access_sql.GreatDs(sql);

                if (access_sql.yzTable(ds))
                {
                    DataTable dt = ds.Tables[0];

                    // 检查并添加必要的列
                    if (!dt.Columns.Contains("haiwaicangxitongbianma")) dt.Columns.Add("haiwaicangxitongbianma", typeof(string));
                    if (!dt.Columns.Contains("putianfachushuliang")) dt.Columns.Add("putianfachushuliang", typeof(string));
                    if (!dt.Columns.Contains("beizhu")) dt.Columns.Add("beizhu", typeof(string));

                    // 添加50行空数据
                    for (int i = dt.Rows.Count; i < 50; i++)
                    {
                        dt.Rows.Add(dt.NewRow());
                    }

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
        }

        // 新增海外全部保存按钮事件
        protected void btnSaveAllHaiwai_Click(object sender, EventArgs e)
        {
            bool allSuccess = true;
            foreach (RepeaterItem item in rptHaiwaiPending.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {


                    TextBox txtShoudao = item.FindControl("txtShoudao") as TextBox;
                    TextBox txtDiuShi = item.FindControl("txtDiuShi") as TextBox;
                    TextBox txtHaiwaiBeizhu = item.FindControl("txtHaiwaiBeizhu") as TextBox; 
                    TextBox touchenwuliufei = item.FindControl("touchenwuliufei") as TextBox;
                    DropDownList fahuozhuangtai = item.FindControl("fahuozhuangtai") as DropDownList;
                    if (txtShoudao == null || txtDiuShi == null) continue;

                    string safeShoudao = txtShoudao.Text.Trim().Replace("'", "''");
                    string safeDiuShi = txtDiuShi.Text.Trim().Replace("'", "''");
                    string haiwaicangrukudanhao = (item.FindControl("haiwaicangrukudanhao") as Literal).Text.Trim().Replace("'", "''");
                    string safeHaiwaiBeizhu = txtHaiwaiBeizhu.Text.Trim().Replace("'", "''");

                    string safetouchenwuliufei = touchenwuliufei.Text.Trim().Replace("'", "''");
                    string safefahuozhuangtai = fahuozhuangtai.SelectedValue;

                    string updateSql = string.Format(@"
                    UPDATE touchenwuliu 
                    SET 
                        haiwaicangshoudaoshuliang = '{0}', 
                        diushishuliang = '{1}',
                        beizhu='{2}',
                          fahuozhuangtai='{3}',
                           touchenwuliufei='{4}'
                    WHERE haiwaicangrukudanhao = '{5}'", safeShoudao, safeDiuShi, safeHaiwaiBeizhu, safefahuozhuangtai, safetouchenwuliufei, haiwaicangrukudanhao);

                    if (!access_sql.ExecSql(updateSql)) allSuccess = false;
                }
            }
            lits.Text = allSuccess ? "海外全部保存成功" : "海外部分保存失败";
            btnHaiwai_Click(sender, e); // 刷新模态框
        }

        // 新增模态框关闭事件
        protected void btnCloseHaiwaiModal_Click(object sender, EventArgs e)
        {
            divHaiwaiModal.Style["display"] = "none";
        }


        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal haiwaicangrukudanhao = e.Item.FindControl("haiwaicangrukudanhao") as Literal;
                string ddimg = access_sql.GetOneValue("select shangchuangtupian from touchenwuliu where haiwaicangrukudanhao='" + haiwaicangrukudanhao.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "tupianwangzhi" }, new object[] { ddimg }, "touchenwuliu", "haiwaicangrukudanhao='" + haiwaicangrukudanhao.Text + "'");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList1") as DataList;
                bindimgs(liimgxs, ddimg, haiwaicangrukudanhao.Text);
            }

        }
        protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string filename = e.CommandArgument.ToString();
                Literal haiwaicangrukudanhao = e.Item.FindControl("haiwaicangrukudanhao") as Literal;
                string ddimg = access_sql.GetOneValue("select guangdongtupian from touchenwuliu where haiwaicangrukudanhao='" + haiwaicangrukudanhao.Text + "'");
                ddimg = ddimg.Replace(filename, "").Replace("||", "|");
                access_sql.T_Update_ExecSql(new string[] { "guangdongtupian" }, new object[] { ddimg }, "touchenwuliu", "haiwaicangrukudanhao='" + haiwaicangrukudanhao.Text + "'");
                RepeaterItem item = (RepeaterItem)((DataList)source).NamingContainer;
                DataList liimgxs = item.FindControl("DataList2") as DataList;
                bindimgs(liimgxs, ddimg, haiwaicangrukudanhao.Text);
            }

        }
        public void bindimgs(DataList liimgxs, string imgs, string cid)
        {
            string[] iiii = imgs.Split('|');
            DataTable dtimgs = new DataTable();
            dtimgs.Columns.Add("haiwaicangrukudanhao");
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
        //excel表格操作

        // 在类级别添加自定义数据结构
        public class ImportItem
        {
            public string SystemCode { get; set; }
            public string Quantity { get; set; }
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!fuExcel.HasFile)
            {
                lits.Text = "请选择要上传的Excel文件";
                return;
            }

            try
            {
                List<ImportItem> importData = new List<ImportItem>();
                IWorkbook workbook;

                // 读取Excel文件
                using (Stream stream = fuExcel.FileContent)
                {
                    string fileExt = System.IO.Path.GetExtension(fuExcel.FileName).ToLower();
                    if (fileExt == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(stream);
                    }
                    else if (fileExt == ".xls")
                    {

                        workbook = new XSSFWorkbook(stream);

                    }
                    else
                    {
                        workbook = new HSSFWorkbook(stream);
                    }
                }

                ISheet sheet = workbook.GetSheetAt(0);
                const int systemCodeCol = 7;  // H列（0-based索引）
                const int quantityCol = 11;    // L列
                const int startRow = 5;        // 数据起始行（0-based）

                // 检测列标题行
                bool headerFound = false;
                for (int rowIdx = startRow; rowIdx <= sheet.LastRowNum; rowIdx++)
                {
                    IRow row = sheet.GetRow(rowIdx);
                    if (row == null) continue;

                    // 自动检测列标题行
                    if (!headerFound)
                    {
                        ICell firstCell = row.GetCell(0);
                        string firstCellValue = GetCellValue(firstCell) ?? "";
                        if (firstCellValue.Trim().Equals("海外仓系统编码", StringComparison.OrdinalIgnoreCase) ||
                            firstCellValue.Trim().Equals("系统编码", StringComparison.OrdinalIgnoreCase))
                        {
                            headerFound = true;
                            continue; // 跳过标题行
                        }
                    }

                    // 处理数据行
                    ICell codeCell = row.GetCell(systemCodeCol);
                    ICell qtyCell = row.GetCell(quantityCol);

                    string systemCode = GetCellValue(codeCell);
                    string quantity = GetCellValue(qtyCell);

                    if (!string.IsNullOrWhiteSpace(systemCode) && !string.IsNullOrWhiteSpace(quantity))
                    {
                        importData.Add(new ImportItem
                        {
                            SystemCode = systemCode.Trim(),
                            Quantity = quantity.Trim()
                        });
                    }
                }

                // 合并重复项
                bool hasDuplicates = false;
                for (int i = 0; i < importData.Count; i++)
                {
                    for (int j = i + 1; j < importData.Count;)
                    {
                        if (string.Equals(importData[i].SystemCode, importData[j].SystemCode, StringComparison.OrdinalIgnoreCase))
                        {
                            // 合并数量
                            int qtyI, qtyJ;
                            if (int.TryParse(importData[i].Quantity, out qtyI) &&
                                int.TryParse(importData[j].Quantity, out qtyJ))
                            {
                                importData[i].Quantity = (qtyI + qtyJ).ToString();
                            }
                            else
                            {
                                importData[i].Quantity += "+" + importData[j].Quantity;
                            }

                            importData.RemoveAt(j);
                            hasDuplicates = true;
                        }
                        else
                        {
                            j++;
                        }
                    }
                }

                // 创建数据表
                DataTable dt = new DataTable();
                dt.Columns.Add("haiwaicangxitongbianma", typeof(string));
                dt.Columns.Add("putianfachushuliang", typeof(string));
                dt.Columns.Add("beizhu", typeof(string));

                foreach (ImportItem item in importData)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["haiwaicangxitongbianma"] = item.SystemCode;
                    newRow["putianfachushuliang"] = item.Quantity;
                    newRow["beizhu"] = string.Empty;
                    dt.Rows.Add(newRow);
                }

                // 补足50行
                while (dt.Rows.Count < 50)
                {
                    dt.Rows.Add(dt.NewRow());
                }

                // 绑定数据
                rplb.DataSource = dt;
                rplb.DataBind();

                // 显示结果
                lits.Text = string.Format("成功导入 {0} 条数据{1}",
                    importData.Count,
                    hasDuplicates ? "（已合并重复项）" : "");
            }
            catch (Exception ex)
            {
                lits.Text = "导入失败：" + ex.Message;
            }
        }


        private int FindColumnIndex(IRow headerRow, string[] possibleNames)
        {
            if (headerRow == null) return -1;

            for (int col = 0; col < headerRow.LastCellNum; col++)
            {
                ICell cell = headerRow.GetCell(col);
                if (cell == null) continue;

                string cellValue = GetCellValue(cell);
                foreach (string name in possibleNames)
                {
                    if (cellValue.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return col;
                    }
                }
            }
            return -1;
        }

        private string GetCellValue(ICell cell)
        {
            if (cell == null) return "";

            // 处理公式单元格
            if (cell.CellType == CellType.Formula)
            {
                return EvaluateFormulaCell(cell);
            }

            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue.Trim();
                case CellType.Numeric:
                    return DateUtil.IsCellDateFormatted(cell)
                        ? cell.DateCellValue.ToString("yyyy-MM-dd")
                        : cell.NumericCellValue.ToString("0.##");
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                default:
                    return cell.ToString().Trim();
            }
        }

        private string EvaluateFormulaCell(ICell cell)
        {
            try
            {
                IFormulaEvaluator evaluator = cell.Sheet.Workbook is HSSFWorkbook
                    ? (IFormulaEvaluator)new HSSFFormulaEvaluator(cell.Sheet.Workbook)
                    : new XSSFFormulaEvaluator(cell.Sheet.Workbook);

                evaluator.EvaluateFormulaCell(cell);
                return GetCellValue(cell);
            }
            catch
            {
                return cell.ToString();
            }
        }
    }
}