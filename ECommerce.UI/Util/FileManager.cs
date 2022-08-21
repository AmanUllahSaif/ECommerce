using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ECommerce.UI.Util
{
    public class FileManager
    {
        static string filePath = "/uploadimages";
        public static string SaveImage(HttpPostedFileBase file)
        {
            string imageurl = string.Empty;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(filePath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(filePath));
            }
            if (file.ContentType.ToLower().Contains("image"))
            {
                string uniqeName = GetUniqueName(file.FileName);
                file.SaveAs(HttpContext.Current.Server.MapPath(filePath + "/" + uniqeName));
                imageurl = filePath + "/" + uniqeName;
            }
            return imageurl;
        }

        public static string SaveBase64Img(string base64String)
        {
            //Convert Base64 Encoded string to Byte Array.
            byte[] imageBytes = Convert.FromBase64String(base64String.Split(',')[1]);
            string uniqeName = GetUniqueName("customimage.png");
            //Save the Byte Array as Image File.
            File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath + "/" + uniqeName), imageBytes);
            string imageUrl = filePath + "/" + uniqeName;
            return imageUrl;
        }

        public static string GetUniqueName(string name)
        {
            var datenow = DateTime.UtcNow;
            return datenow.Year + datenow.Month + datenow.Day + datenow.Hour + datenow.Minute + datenow.Second + datenow.Ticks + name;
        }
    }
}