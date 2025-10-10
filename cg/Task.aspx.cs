using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication11.cg
{
    public partial class Task : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            DataSet ds = access_sql.GreatDs("SELECT * FROM dbo.Task ORDER BY id DESC");
            repTask.DataSource = ds;
            repTask.DataBind();
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

            BindData();
        }
    }
}
