using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility
{
    public class AvatarGenerator
    {
        private List<string> _BackgroundColours;

        public AvatarGenerator()
        {
            _BackgroundColours = new List<string> { "#1abc9c", "#2ecc71", "#3498db", "#9b59b6", "#34495e", "#16a085", "#27ae60", "#2980b9", "#8e44ad", "#2c3e50",
                "#f1c40f", "#e67e22", "#e74c3c", "#ecf0f1", "#95a5a6", "#f39c12", "#d35400", "#c0392b", "#bdc3c7", "#7f8c8d",
"#8bbf61",

"#DC143C",
"#CD6889",
"#8B8386",
"#800080",
"#9932CC",
"#009ACD",
"#00CED1",
"#03A89E",

"#00C78C",
"#00CD66",
"#66CD00",
"#EEB422",
"#FF8C00",
"#EE4000",

"#388E8E",
"#8E8E38",
"#7171C6" };
        }

        public String Generate(string firstName)
        {
            var avatarString = string.Format("{0}", firstName[0]).ToUpper();

            var charIndex = (avatarString[0] == '?' ? 72 : firstName[0]) - 64;
            var colourIndex = charIndex % 20;

          //  context.fillStyle = colours[colourIndex - 1];
            var randomIndex = new Random().Next(0, _BackgroundColours.Count - 1);
            var bgColour = _BackgroundColours[colourIndex - 1];

            var bmp = new Bitmap(194, 194);
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            var font = new Font("Arial", 77, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(bmp);

            graphics.Clear((Color)new ColorConverter().ConvertFromString(bgColour));
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(avatarString, font, new SolidBrush(Color.WhiteSmoke), new RectangleF(0, 0, 192, 192), sf);
            graphics.Flush();

            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            var base64 = Convert.ToBase64String(ms.GetBuffer());
            return base64;
        }
    }
}
