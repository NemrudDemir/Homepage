using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Homepage.Pages
{
    public class Favicon : PageModel
    {
        public ActionResult OnGet()
        {
            var bitmap = new Bitmap(32, 32);
            var g = Graphics.FromImage(bitmap);
            //TODO
            //g.FillRectangle(new SolidBrush(Color.Blue), 0, 0, 32, 32);
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return File(ms.ToArray(), "image/x-icon");
            }
        }
    }
}
