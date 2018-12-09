using System.Drawing;
using System.Web;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites
{
    public class ImageProcessor : IHttpHandler
    {
        public bool IsReusable { get { return false; } } 

        public void ProcessRequest(HttpContext context)
        {
            string imagePath = context.Request["imageid"];
            var maxWidth = int.Parse(context.Request["width"]);
            var maxHeight = int.Parse(context.Request["height"]);
            string fullPath = context.Request.ApplicationPath + Utilities.Base64Decode(imagePath);
            var resize = new ResizePhoto();
            resize.Resize(fullPath, maxWidth, maxHeight);
            context.Response.Clear();
            context.Response.ContentType = "Image/jpeg";
            context.Response.OutputStream.Write(resize.ImageBuffer, 0, resize.ImageBuffer.Length);
            context.Response.End();
        }
    }
}