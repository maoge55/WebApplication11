using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class Task : System.Web.UI.Page
    {
        private DataSet dsAll; // 保存全量数据

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData(bool onlyPositionNotMinusOne = false)
        {
            DataSet ds = access_sql.GreatDs("SELECT * FROM dbo.Task ORDER BY id");

            dsAll = ds; // 保存全量数据到成员变量
            ViewState["dsAll"] = dsAll;

            if (onlyPositionNotMinusOne)
            {
                DataTable dtFiltered = ds.Tables[0].AsEnumerable()
                    .Where(row => Convert.ToInt32(row["position"]) != -1)
                    .CopyToDataTable();
                repTask.DataSource = dtFiltered;
            }
            else
            {
                repTask.DataSource = ds.Tables[0];
            }

            repTask.DataBind();
        }

        // 点击过滤按钮
        protected void btnFilterPosition_Click(object sender, EventArgs e)
        {
            BindData(true);
        }

        // 点击显示全部按钮
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindData(false);
        }

        protected void repTask_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            string sql = "";

            if (e.CommandName == "start")
                sql = $"UPDATE Task SET state=1 WHERE id={id}";
            else if (e.CommandName == "continue")
                sql = $"UPDATE Task SET state=1, LastHouTaiID=999 WHERE id={id}";
            else if (e.CommandName == "close")
                sql = $"UPDATE Task SET state=-1 WHERE id={id}";
            else if (e.CommandName == "save")
            {
                string[] cols = { "tName", "ShopClass", "tcount", "pt", "logPath", "LastRunTime", "LastEndTime", "LastHouTaiID", "iszd", "FuncName", "isMulti", "extraParam", "timing", "gtName", "position", "mutilTiming" };
                object[] vals = {
                    ((TextBox)e.Item.FindControl("txttName")).Text,
                    ((TextBox)e.Item.FindControl("txtShopClass")).Text,
                    ((TextBox)e.Item.FindControl("txttcount")).Text,
                    ((TextBox)e.Item.FindControl("txtpt")).Text,
                    ((TextBox)e.Item.FindControl("txtlogPath")).Text,
                    ((TextBox)e.Item.FindControl("txtLastRunTime")).Text,
                    ((TextBox)e.Item.FindControl("txtLastEndTime")).Text,
                    ((TextBox)e.Item.FindControl("txtLastHouTaiID")).Text,
                    ((TextBox)e.Item.FindControl("txtiszd")).Text,
                    ((TextBox)e.Item.FindControl("txtFuncName")).Text,
                    ((TextBox)e.Item.FindControl("txtisMulti")).Text,
                    ((TextBox)e.Item.FindControl("txtextraParam")).Text,
                    ((TextBox)e.Item.FindControl("txttiming")).Text,
                    ((TextBox)e.Item.FindControl("txtgtName")).Text,
                    ((DropDownList)e.Item.FindControl("ddlPosition")).SelectedValue,
                    ((TextBox)e.Item.FindControl("txtmutilTiming")).Text
                };

                // mutilTiming 更新时，同步 timing 最小值
                string mutilTiming = vals[15]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(mutilTiming) && mutilTiming.Contains("|"))
                {
                    var parts = mutilTiming.Split('|')
                                           .Select(s => { int n; return int.TryParse(s, out n) ? n : int.MaxValue; });
                    vals[12] = parts.Min(); // timing 列更新为最小值
                }

                access_sql.T_Update_ExecSql(cols, vals, "Task", $"id={id}");
            }

            if (!string.IsNullOrEmpty(sql))
                access_sql.ExecSql(sql);

            // 重新绑定当前过滤状态
            bool onlyFiltered = ViewState["dsAllFiltered"] != null && (bool)ViewState["dsAllFiltered"];
            BindData(onlyFiltered);
        }
    }
}
