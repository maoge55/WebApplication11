using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication11.cg.cjt
{
    public partial class ImageProxy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string imageUrl = Request.QueryString["url"];
            if (string.IsNullOrWhiteSpace(imageUrl) || !Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Response.StatusCode = 400;
                Response.Write("Invalid image URL");
                return;
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    // 伪造 Referer，模拟从 1688 页面访问
                    client.Headers.Add("Referer", "https://detail.1688.com/");

                    byte[] imageData = client.DownloadData(imageUrl);

                    // 简单判断图片类型
                    string contentType = GetContentType(imageUrl);
                    Response.ContentType = contentType;
                    Response.Cache.SetCacheability(HttpCacheability.Public); // 可缓存
                    Response.BinaryWrite(imageData);
                    Response.End();
                }
            }
            catch (WebException ex)
            {
                Response.StatusCode = 500;
                Response.Write("图片获取失败: " + ex.Message);
            }
        }

        private string GetContentType(string imageUrl)
        {
            string ext = imageUrl.ToLower();
            if (ext.EndsWith(".png")) return "image/png";
            if (ext.EndsWith(".jpg") || ext.EndsWith(".jpeg")) return "image/jpeg";
            if (ext.EndsWith(".gif")) return "image/gif";
            return "application/octet-stream";
        }
    }
}