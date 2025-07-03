using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace WebApplication11.cg.tb
{
    public partial class 广告测品出单数据分析_管理员 : System.Web.UI.Page
    {
        // 排序规则
        private static readonly Dictionary<string, int> SalesRangeOrder = new Dictionary<string, int>
        {
            {"未知", 0},
            {"0 - 500", 1},
            {"501 - 1000", 2},
            {"1001 - 2000", 3},
            {"2000+", 4}
        };

        private static readonly Dictionary<string, int> bookRangeOrder = new Dictionary<string, int>
        {
            {"未知", 0},
            {"0 - 500", 1},
            {"501 - 1000", 2},
            {"1001 - 2000", 3},
            {"2000+", 4}
        };


        private static readonly Dictionary<string, int> RatingRangeOrder = new Dictionary<string, int>
        {
            {"未知", 0},
            {"1分 - 3分", 1},
            {"3.5分", 2},
            {"4分", 3},
            {"4.5分", 4},
            {"5分", 5},
            {"其他", 6}
        };

        private static readonly Dictionary<string, int> PriceRangeOrder = new Dictionary<string, int>
        {
            {"未知", 0},
            {"1 - 20", 1},
            {"21 - 40", 2},
            {"41 - 60", 3},
            {"60+", 4}
        };

        private static readonly Dictionary<string, int> ShopTypeOrder = new Dictionary<string, int>
        {
            {"研究所", 1},
            {"商行", 2},
            {"公司", 3},
            {"厂", 4},
            {"店", 5},
            {"其他", 6}
        };

        private static readonly Dictionary<string, int> ReturnRateOrder = new Dictionary<string, int>
        {
            {"未知", 0},
            {"0 - 0.2", 1},
            {"0.21 - 0.4", 2},
            {"0.41 - 0.6", 3},
            {"0.6+", 4}
        };

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
                if (uid != "8" && uid != "9" && uid != "18" && uid != "19" && uid != "12" && uid != "6")
                {
                    Response.Redirect("/cg/clogin.aspx");
                }
            }
            if (!IsPostBack)
            {
                // 初始化时隐藏数据区域和无数据提示
                pnlDataArea.Visible = false;
                pnlNoData.Visible = false;
            }
        }

        private string u = "";
        private string p = "";
        private string uid = "";

        private void BindChartData()
        {
            try
            {
                string[] merchantCodes = txtOperationCodes.Text.Trim()
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()) // 去除每行的首尾空格
                    .Where(x => !string.IsNullOrWhiteSpace(x)) // 过滤掉空行
                    .ToArray();

                if (merchantCodes.Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('请输入有效的商家编码！');", true);
                    ShowNoData();
                    return;
                }

                string whereCondition = " AND so.SJBM IN ('" + string.Join("','", merchantCodes.Select(x => x.Replace("'", "''"))) + "')";

                string sql = @"
                WITH DistinctOrders AS (
                    SELECT DISTINCT ItemID, buyer_id, random_code
                    FROM ShopeeOrder so
                    WHERE random_code IS NOT NULL" + whereCondition + @"
                ),
                BaseData AS (
                    SELECT 
                        CASE 
                            WHEN s.historical_sold IS NULL OR s.historical_sold < 0 THEN '未知'
                            WHEN s.historical_sold BETWEEN 0 AND 500 THEN '0 - 500'
                            WHEN s.historical_sold BETWEEN 501 AND 1000 THEN '501 - 1000'
                            WHEN s.historical_sold BETWEEN 1001 AND 2000 THEN '1001 - 2000'
                            ELSE '2000+'
                        END AS sales_range,
                        CASE 
                            WHEN s.rating_star IS NULL OR s.rating_star < 1 THEN '未知'
                            WHEN s.rating_star <= 3 THEN '1分 - 3分'
                            WHEN s.rating_star = 3.5 THEN '3.5分'
                            WHEN s.rating_star = 4 THEN '4分'
                            WHEN s.rating_star = 4.5 THEN '4.5分'
                            WHEN s.rating_star = 5 THEN '5分'
                            ELSE '其他'
                        END AS rating_range,
                        CASE 
                            WHEN s.bookedCount IS NULL OR s.bookedCount < 0 THEN '未知'
                            WHEN s.bookedCount BETWEEN 0 AND 500 THEN '0 - 500'
                            WHEN s.bookedCount BETWEEN 501 AND 1000 THEN '501 - 1000'
                            WHEN s.bookedCount BETWEEN 1001 AND 2000 THEN '1001 - 2000'
                            ELSE '2000+'
                        END AS booking_range,
                        CASE 
                            WHEN s.price IS NULL OR s.price < 1 THEN '未知'
                            WHEN s.price BETWEEN 1 AND 20 THEN '1 - 20'
                            WHEN s.price BETWEEN 21 AND 40 THEN '21 - 40'
                            WHEN s.price BETWEEN 41 AND 60 THEN '41 - 60'
                            ELSE '60+'
                        END AS price_range,
                        CASE 
                            WHEN RIGHT(s.shop_name, 3) LIKE '%研究所' THEN '研究所'
                            WHEN RIGHT(s.shop_name, 3) LIKE '%商行' THEN '商行'
                            WHEN RIGHT(s.shop_name, 3) LIKE '%公司' THEN '公司'
                            WHEN RIGHT(s.shop_name, 3) LIKE '%厂' THEN '厂'
                            WHEN RIGHT(s.shop_name, 3) LIKE '%店' THEN '店'
                            ELSE '其他'
                        END AS shop_type,
                        CASE 
                            WHEN s.return_rate BETWEEN 0 AND 0.2 THEN '0 - 0.2'
                            WHEN s.return_rate BETWEEN 0.21 AND 0.4 THEN '0.21 - 0.4'
                            WHEN s.return_rate BETWEEN 0.41 AND 0.6 THEN '0.41 - 0.6'
                            WHEN s.return_rate > 0.6 THEN '0.6+'
                            ELSE '未知'
                        END AS return_rate_range,
                        o.buyer_id
                    FROM DistinctOrders o
                    INNER JOIN S1688pro s ON o.random_code = s.random_code
                ),
                Stats AS (
                    SELECT 'sales' AS Type, sales_range AS Range, COUNT(DISTINCT buyer_id) AS Count
                    FROM BaseData
                    GROUP BY sales_range
                    UNION ALL
                    SELECT 'rating', rating_range, COUNT(DISTINCT buyer_id)
                    FROM BaseData
                    GROUP BY rating_range
                    UNION ALL
                    SELECT 'booking', booking_range, COUNT(DISTINCT buyer_id)
                    FROM BaseData
                    GROUP BY booking_range
                    UNION ALL
                    SELECT 'price', price_range, COUNT(DISTINCT buyer_id)
                    FROM BaseData
                    GROUP BY price_range
                    UNION ALL
                    SELECT 'shop', shop_type, COUNT(DISTINCT buyer_id)
                    FROM BaseData
                    GROUP BY shop_type
                    UNION ALL
                    SELECT 'return', return_rate_range, COUNT(DISTINCT buyer_id)
                    FROM BaseData
                    GROUP BY return_rate_range
                )
                SELECT Type, Range, Count
                FROM Stats";

                DataSet ds = access_sql.GreatDs(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // 使用相同的排序规则
                    BindRepeaterData(ds.Tables[0], "sales", SalesRangeOrder, rptSalesData);
                    BindRepeaterData(ds.Tables[0], "rating", RatingRangeOrder, rptRatingData);
                    BindRepeaterData(ds.Tables[0], "booking", bookRangeOrder, rptBookingData); 
                    BindRepeaterData(ds.Tables[0], "price", PriceRangeOrder, rptPriceData);
                    BindRepeaterData(ds.Tables[0], "shop", ShopTypeOrder, rptShopData);
                    BindRepeaterData(ds.Tables[0], "return", ReturnRateOrder, rptReturnRateData);

                    // 显示数据区域，隐藏无数据提示
                    pnlDataArea.Visible = true;
                    pnlNoData.Visible = false;
                }
                else
                {
                    ShowNoData();
                }
            }
            catch (Exception ex)
            {
                // 记录错误
                System.Diagnostics.Debug.WriteLine("数据绑定错误: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "error", "alert('数据处理出错，请稍后重试！');", true);
                ShowNoData();
            }
        }

        private void ShowNoData()
        {
            // 显示无数据提示，隐藏数据区域
            pnlDataArea.Visible = false;
            pnlNoData.Visible = true;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOperationCodes.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('请输入商家编码！');", true);
                ShowNoData();
                return;
            }

            // 检查是否存在空行
            var lines = txtOperationCodes.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Any(line => string.IsNullOrWhiteSpace(line)))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('商家编码不能包含空行！');", true);
                ShowNoData();
                return;
            }

            BindChartData();
        }

        private void BindRepeaterData(DataTable sourceTable, string type, Dictionary<string, int> orderDict, Repeater repeater)
        {
            var data = sourceTable.AsEnumerable()
                .Where(r => r.Field<string>("Type") == type)
                .Select(r => new
                {
                    Range = r.Field<string>("Range"),
                    Count = r.Field<int>("Count")
                })
                .ToList();

            // 计算总数和百分比
            int total = data.Sum(x => x.Count);

            var result = data.Select(x => new
            {
                Range = x.Range,
                Count = x.Count,
                Percentage = total > 0 ? $"{(x.Count * 100.0 / total):F2}%" : "0%"
            })
            .OrderBy(x => orderDict[x.Range])
            .ToList();

            repeater.DataSource = result;
            repeater.DataBind();
        }
    }
} 