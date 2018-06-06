using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageUploader
{
    class Program
    {
        private const string URL = "https://img.lee.io/upload?expires=604800";

        [STAThread]
        static void Main(string[] args)
        {
            if (!Clipboard.ContainsImage())
            {
                Console.WriteLine("No image on clipboard");
                Environment.Exit(1);
            }

            try
            {
                Console.WriteLine(UploadFile(Clipboard.GetImage()));
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload: {ex.Message}");
                Environment.Exit(2);
            }
        }

        private static string UploadFile(Image image)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                byte[] response = wc.UploadData(new Uri(URL), "POST", System.Text.Encoding.UTF8.GetBytes("data=" + System.Web.HttpUtility.UrlEncode(ImageToBase64String(image))));

                return System.Text.Encoding.ASCII.GetString(response);
            }
        }

        private static string ImageToBase64String(Image image)
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);

                return $"data:image/png;base64,{Convert.ToBase64String(stream.ToArray())}";
            }
        }
    }
}
