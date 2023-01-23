using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Synergy.App.WebUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace CMS.UI.Web.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IBreMasterMetadataBusiness _breMasterMetadataBusiness;
        private readonly IPageBusiness _pageBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ITableMetadataBusiness _tableDataBusiness;
        private readonly IColumnMetadataBusiness _columnDataBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IFormTemplateBusiness _formTemplateBusiness;
        private readonly INoteTemplateBusiness _noteTemplateBusiness;
        private readonly ITaskTemplateBusiness _taskTemplateBusiness;
        private readonly IServiceTemplateBusiness _serviceTemplateBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IServiceProvider _sp;
        private readonly ILogger _logger;
        private readonly IQRCodeBusiness _qrCodeBusiness;
        private readonly IFileBusiness _fileBusiness;
        private List<string> Styles { get; set; }

        public HomeController(IBreMasterMetadataBusiness breMasterMetadataBusiness
            , IPageBusiness pageBusiness
            , IPortalBusiness portalBusiness, IRazorViewEngine razorViewEngine
            , IHttpContextAccessor contextAccessor
            , ITempDataProvider tempDataProvider
            , ITemplateBusiness templateBusiness
            , ITableMetadataBusiness tableDataBusiness
            , IColumnMetadataBusiness columnDataBusiness
            , ICmsBusiness cmsBusiness
            , IFormTemplateBusiness formTemplateBusiness,
            INoteTemplateBusiness noteTemplateBusiness, ITaskTemplateBusiness taskTemplateBusiness,
            IServiceTemplateBusiness serviceTemplateBusiness
            , ITableMetadataBusiness tableMetadataBusiness
            , IUserBusiness userBusiness
            , IStringLocalizer<HomeController> localizer
            , IServiceProvider sp
            , ILogger<ApplicationError> logger
            , IQRCodeBusiness qrCodeBusiness
            , IFileBusiness fileBusiness)
        {
            _breMasterMetadataBusiness = breMasterMetadataBusiness;
            _pageBusiness = pageBusiness;
            _portalBusiness = portalBusiness;
            _razorViewEngine = razorViewEngine;
            _contextAccessor = contextAccessor;
            _tempDataProvider = tempDataProvider;
            _templateBusiness = templateBusiness;
            _tableDataBusiness = tableDataBusiness;
            _columnDataBusiness = columnDataBusiness;
            _cmsBusiness = cmsBusiness;
            _formTemplateBusiness = formTemplateBusiness;
            _noteTemplateBusiness = noteTemplateBusiness;
            _taskTemplateBusiness = taskTemplateBusiness;
            _serviceTemplateBusiness = serviceTemplateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _userBusiness = userBusiness;
            _localizer = localizer;
            _sp = sp;
            _logger = logger;
            _qrCodeBusiness = qrCodeBusiness;
            _fileBusiness = fileBusiness;
        }
        string ReportsFolder;

        public async Task<IActionResult> Index([FromRoute] string portalName, [FromRoute] string pageName, string mode, string source, string id, string pageUrl)
        {
            var runningMode = RunningModeEnum.Preview;

            if (mode != null && mode.ToLower() == "preview")
            {
                runningMode = RunningModeEnum.Preview;
            }
            var requestSource = RequestSourceEnum.Main;
            if (source.IsNotNullAndNotEmpty())
            {
                requestSource = source.ToEnum<RequestSourceEnum>();
            }

            return RedirectToAction("index", "content", new { @area = "cms" });
        }

        private readonly Assembly[] DefaultReferences =
{
            typeof(TaskTemplateViewModel).Assembly,
            typeof(TaskTemplate).Assembly,
            typeof(TaskTemplateBusiness).Assembly,
            typeof(IRepositoryBase<,>).Assembly,
            typeof(Enumerable).Assembly,
            typeof(List<string>).Assembly,
        };
        public async Task<IActionResult> TestDate()
        {

            return View();

        }
        public async Task<IActionResult> GenerateBarCodeImage(string data, QRCodeDataTypeEnum dataType = QRCodeDataTypeEnum.Text, QRCodeTypeEnum type = QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum? referenceType = null, string referenceId = null)
        {
            var result = await _fileBusiness.GenerateBarCodeFile(data, dataType, type, ReferenceTypeEnum.NTS_Service);
            return File(result.Item3.ToArray(), "image/jpeg");
        }
        public async Task<IActionResult> GenerateCaptcha(string referenceId, string referenceType)
        {
            var model = new GenerateCaptchaViewModel();
            var random = new Random();
            Styles = new List<string>();
            model.Images = new List<CaptchaImages>();
            var allImages = new List<string>();
            var allNumbers = new List<int>();
            var displayImages = new List<string>();
            var actualImages = new List<string>();
            var dummyImages = new List<string>();
            var actualNumber = random.Next(100, 1000);
            allNumbers.Add(actualNumber);
            var count = random.Next(3, 7);
            for (int i = 0; i < count; i++)
            {
                var id = GenerateRandomString(random);

                while (allImages.Contains(id))
                {
                    id = GenerateRandomString(random);
                }

                model.Images.Add(new CaptchaImages
                {
                    Id = id,
                    Number = actualNumber,
                    Base64 = GenerateImage(random, 150, 80, Convert.ToString(actualNumber))
                });
                displayImages.Add(id);
                actualImages.Add(id);
                allImages.Add(id);
            }
            var randomNum = 101;
            while (displayImages.Count() < 9)
            {
                randomNum = random.Next(100, 1000);
                while (randomNum == actualNumber)
                {
                    randomNum = random.Next(100, 1000);
                }
                allNumbers.Add(randomNum);
                var id = GenerateRandomString(random);
                while (allImages.Contains(id))
                {
                    id = GenerateRandomString(random);
                }
                model.Images.Add(new CaptchaImages
                {
                    Id = id,
                    Number = randomNum,
                    Base64 = GenerateImage(random, 150, 80, Convert.ToString(randomNum))
                });
                displayImages.Add(id);
                allImages.Add(id);
            }


            TempData["DisplayImages"] = string.Join(",", displayImages);
            TempData["ActualImages"] = string.Join(",", actualImages);
            var allNumbersCount = allNumbers.Count();
            var numberLimit = allNumbers.Count();
            var numberIndex = 0;
            while (allImages.Count() < 36)
            {
                var lmt = random.Next(1, 6);
                numberIndex = random.Next(0, numberLimit);
                randomNum = allNumbers[numberIndex];
                allNumbers.Add(randomNum);
                for (int i = 0; i < lmt; i++)
                {
                    if (allImages.Count() >= 36)
                    {
                        break;
                    }
                    var id = GenerateRandomString(random);
                    while (allImages.Contains(id))
                    {
                        id = GenerateRandomString(random);
                    }
                    allImages.Add(id);
                    dummyImages.Add(id);

                    model.Images.Add(new CaptchaImages
                    {
                        Id = id,
                        Number = randomNum,
                        Base64 = GenerateImage(random, 150, 80, Convert.ToString(randomNum))
                    });
                }
            }


            while (allImages.Count() < 45)
            {
                var lmt = random.Next(1, 6);
                randomNum = random.Next(100, 1000);
                allNumbers.Add(randomNum);
                for (int i = 0; i < lmt; i++)
                {
                    if (allImages.Count() >= 45)
                    {
                        break;
                    }
                    var id = GenerateRandomString(random);
                    while (allImages.Contains(id))
                    {
                        id = GenerateRandomString(random);
                    }
                    allImages.Add(id);
                    dummyImages.Add(id);

                    model.Images.Add(new CaptchaImages
                    {
                        Id = id,
                        Number = randomNum,
                        Base64 = GenerateImage(random, 150, 80, Convert.ToString(randomNum))
                    });
                }
            }

            GenerateDummyStyles(random);

            var allfns = GenerateScripts(random, allImages, displayImages, dummyImages);
            Shuffle<string>(random, allfns.Item1);
            Shuffle<string>(random, allfns.Item2);
            model.Script = $"$(function(){{{string.Join("();", allfns.Item1)}();}});{string.Join("", allfns.Item2)}";

            var bg = GenerateBGColors(random);
            model.BgColor = bg.Item1;
            var labels = GenerateLabels(random, actualNumber, allNumbers, bg.Item2);
            Shuffle<string>(random, labels.Item2);
            model.Labels = string.Join("", labels.Item2);
            model.LabelId = labels.Item1;
            Shuffle<CaptchaImages>(random, model.Images);
            Shuffle<string>(random, Styles);
            model.Style = string.Join("", Styles);
            return View(model);
        }


        private Tuple<string, List<string>> GenerateBGColors(Random random)
        {
            var clrs = new List<Tuple<string, string>>();
            clrs.Add(new Tuple<string, string>("AliceBlue", "#F0F8FF"));
            clrs.Add(new Tuple<string, string>("Azure", "#F0FFFF"));
            clrs.Add(new Tuple<string, string>("FloralWhite", "#FFFAF0"));
            clrs.Add(new Tuple<string, string>("GhostWhite", "#F8F8FF"));
            clrs.Add(new Tuple<string, string>("HoneyDew", "#F0FFF0"));
            clrs.Add(new Tuple<string, string>("Ivory", "#FFFFF0"));
            clrs.Add(new Tuple<string, string>("LightYellow", "#FFFFE0"));
            clrs.Add(new Tuple<string, string>("MintCream", "#F5FFFA"));
            clrs.Add(new Tuple<string, string>("Snow", "#FFFAFA"));
            clrs.Add(new Tuple<string, string>("WhiteSmoke", "#F5F5F5"));
            clrs.Add(new Tuple<string, string>("White", "#FFFFFF"));
            var numb = random.Next(0, 11);
            var actualColor = clrs[numb];
            numb = random.Next(6, 11);
            var bgClass = GenerateRandomString(random, numb);
            Styles.Add($".{bgClass}{{background-color:{actualColor.Item1};}}");
            for (int i = 0; i < 50; i++)
            {
                Styles.Add($".{GenerateRandomString(random, random.Next(6, 11))}{{background-color:{actualColor.Item1};}}");
            }

            numb = random.Next(6, 11);
            var labelClass = "";
            var labelClasses = new List<string>();
            for (int i = 0; i < 50; i++)
            {
                labelClass = GenerateRandomString(random, numb);
                labelClasses.Add(labelClass);
                Styles.Add($".{labelClass}{{color:{actualColor.Item2};}}");
            }
            return new Tuple<string, List<string>>($"{GenerateRandomString(random)} {bgClass} {GenerateRandomString(random)}", labelClasses);
        }

        private Tuple<string, List<string>> GenerateLabels(Random random, int actualNumber, List<int> allNumbers, List<string> bg)
        {

            var labels = new List<string>();
            var labelId = GenerateRandomString(random);
            var lblClass = GenerateRandomString(random);
            var actualZindex = random.Next(900, 1000);
            Styles.Add($".{lblClass}{{z-index:{actualZindex};}}");
            labels.Add($"<div class='col-12 box-label {lblClass} {GenerateRandomString(random)} {GenerateRandomString(random)}' id='{labelId}'>Please select all boxes with number {actualNumber}</div>");
            var randomId = "";

            var zindex = 500;
            var numberCount = allNumbers.Count();
            var bgCount = bg.Count();
            var bgNext = 0;
            foreach (var item in allNumbers.Distinct().ToList())
            {
                zindex = random.Next(500, actualZindex);
                bgNext = random.Next(0, bgCount);
                lblClass = GenerateRandomString(random);
                randomId = GenerateRandomString(random);
                Styles.Add($".{lblClass}{{z-index:{zindex};}}");
                labels.Add($"<div class='col-12 box-label {lblClass} {GenerateRandomString(random)} {bg[bgNext]}' id='{randomId}'>Please select all boxes with number {item}</div>");
            }
            var next = random.Next(10, 30);
            for (int i = 0; i < next; i++)
            {
                var item = random.Next(100, 1000);
                zindex = random.Next(500, actualZindex);
                bgNext = random.Next(0, bgCount);
                lblClass = GenerateRandomString(random);
                randomId = GenerateRandomString(random);
                Styles.Add($".{lblClass}{{z-index:{zindex};}}");
                labels.Add($"<div class='col-12 box-label {lblClass} {GenerateRandomString(random)} {bg[bgNext]}' id='{randomId}'>Please select all boxes with number {item}</div>");
            }

            for (int i = 0; i < 30; i++)
            {
                zindex = random.Next(1000, 1500);
                lblClass = GenerateRandomString(random);
                Styles.Add($".{lblClass}{{z-index:{zindex};}}");
            }
            return new Tuple<string, List<string>>(labelId, labels);
        }

        private Tuple<List<string>, List<string>> GenerateScripts(Random random, List<string> allImages, List<string> displayImages, List<string> dummyImages)
        {
            var fns = new List<string>();
            var fnContent = new List<string>();
            var next = random.Next(15, 30);
            for (int i = 0; i < next; i++)
            {
                var fn = GenerateReturnScript(random, allImages);
                fns.Add(fn.Item1);
                fnContent.Add(fn.Item2);
            }
            next = random.Next(15, 30);
            for (int i = 0; i < next; i++)
            {
                var fn = GenerateTryCatchScript(random, allImages, displayImages);
                fns.Add(fn.Item1);
                fnContent.Add(fn.Item2);
            }
            for (int i = 0; i < 45; i++)
            {
                GenerateActualScript(random, allImages, displayImages, dummyImages, i, fns, fnContent);
            }


            return new Tuple<List<string>, List<string>>(fns, fnContent);
        }

        private void GenerateActualScript(Random random, List<string> allImages, List<string> displayImages, List<string> dummyImages, int index, List<string> fns, List<string> fnContent)
        {
            var img = allImages[index];
            if (displayImages.Contains(img))
            {
                var fn = GenerateShowActtualScript(random, allImages, displayImages, dummyImages, img);
                fns.Add(fn.Item1);
                fnContent.Add(fn.Item2);
            }
            else
            {
                var fn = GenerateHideDummyScript(random, allImages, displayImages, dummyImages, img);
                fns.Add(fn.Item1);
                fnContent.Add(fn.Item2);
            }
        }

        private Tuple<string, string> GenerateShowActtualScript(Random random, List<string> allImages, List<string> displayImages, List<string> dummyImages, string img)
        {

            var dCount = dummyImages.Count();
            dCount = 10;
            var fnName = GenerateRandomString(random);
            var str = $"function {fnName}(){{try{{";
            var next = random.Next(0, dCount);
            for (int i = 0; i < next; i++)
            {
                str = string.Concat(str, $"$('#'+document.getElementById('", dummyImages[i], "').id).hide();");
            }
            str = string.Concat(str, $"$('#'+document.getElementById('", img, "').id).show();");
            next = random.Next(5, 15);
            for (int i = 0; i < next; i++)
            {
                var dummyName = GenerateRandomString(random);
                while (allImages.Contains(dummyName))
                {
                    dummyName = GenerateRandomString(random);
                }
                var swtch = random.Next(1, 3);
                if (swtch == 1)
                {
                    str = string.Concat(str, $"$('#'+document.getElementById('", dummyName, "').id).show();");

                }
                else
                {
                    str = string.Concat(str, $"$('#'+document.getElementById('", dummyName, "').id).hide();");

                }
            }
            str = string.Concat(str, "}catch(er){console.log(er);}}");
            return new Tuple<string, string>(fnName, str);

        }
        private Tuple<string, string> GenerateHideDummyScript(Random random, List<string> allImages, List<string> displayImages, List<string> dummyImages, string img)
        {
            var dCount = dummyImages.Count();
            dCount = 10;
            var fnName = GenerateRandomString(random);
            var str = $"function {fnName}(){{try{{";
            var next = random.Next(0, dCount);
            for (int i = 0; i < next; i++)
            {
                str = string.Concat(str, $"$('#'+document.getElementById('", dummyImages[i], "').id).hide();");
            }
            str = string.Concat(str, $"$('#'+document.getElementById('", img, "').id).hide();");
            next = random.Next(5, 15);
            for (int i = 0; i < next; i++)
            {
                var dummyName = GenerateRandomString(random);
                while (allImages.Contains(dummyName))
                {
                    dummyName = GenerateRandomString(random);
                }
                var swtch = random.Next(1, 3);
                if (swtch == 1)
                {
                    str = string.Concat(str, $"$('#'+document.getElementById('", dummyName, "').id).show();");

                }
                else
                {
                    str = string.Concat(str, $"$('#'+document.getElementById('", dummyName, "').id).hide();");

                }
            }
            str = string.Concat(str, "}catch(er){console.log(er);}}");
            return new Tuple<string, string>(fnName, str);
        }

        private Tuple<string, string> GenerateTryCatchScript(Random random, List<string> allImages, List<string> displayImages)
        {
            var fnName = GenerateRandomString(random);
            var dummyName = GenerateRandomString(random);
            while (allImages.Contains(dummyName))
            {
                dummyName = GenerateRandomString(random);
            }
            var str = $"function {fnName}(){{try{{";
            var next = random.Next(5, allImages.Count());
            var lngth = allImages.Count();
            var swtch = random.Next(1, 3);
            if (swtch == 1)
            {
                str = string.Concat(str, $"$('#'+document.getElementById('", dummyName, "').id).show();");
                for (int i = 0; i < next; i++)
                {
                    var k = random.Next(0, lngth);
                    str = string.Concat(str, $"$('#'+document.getElementById('", allImages[k], "').id).show();");

                }
            }
            else
            {
                str = string.Concat(str, $"$('#'+document.getElementById('", dummyName, "').id).hide();");
                for (int i = 0; i < next; i++)
                {
                    var k = random.Next(0, lngth);
                    str = string.Concat(str, $"$('#'+document.getElementById('", allImages[k], "').id).hide();");
                }
            }

            str = string.Concat(str, "}catch(er){console.log(er);}}");
            return new Tuple<string, string>(fnName, str);
        }

        private Tuple<string, string> GenerateReturnScript(Random random, List<string> allImages)
        {
            var fnName = GenerateRandomString(random);
            var str = $"function {fnName}(){{try{{return false;";
            var next = random.Next(5, 15);
            var lngth = allImages.Count();
            var swtch = random.Next(1, 3);
            if (swtch == 1)
            {
                for (int i = 0; i < next; i++)
                {
                    var k = random.Next(0, lngth);
                    str = string.Concat(str, $"$('#'+document.getElementById('", allImages[k], "').id).show();");

                }
            }
            else
            {
                for (int i = 0; i < next; i++)
                {
                    var k = random.Next(0, lngth);
                    str = string.Concat(str, $"$('#'+document.getElementById('", allImages[k], "').id).hide();");
                }
            }

            str = string.Concat(str, "}catch(er){console.log(er);}}");
            return new Tuple<string, string>(fnName, str);
        }

        private string GetRandomCssClass(List<string> dummyStyles, Random random)
        {
            Shuffle<string>(random, dummyStyles);
            var next = random.Next(15, dummyStyles.Count());
            var className = "";
            for (int i = next; i > next - 10; i--)
            {
                className = string.Concat(className, " ", dummyStyles[i]);
            }
            return className;
        }

        private Tuple<string, string> GetHiddenStyle(Random random)
        {
            var cName = GenerateRandomString(random);
            return new Tuple<string, string>(cName, string.Concat(".", cName, "{display:none;}"));
        }

        private Tuple<string, string> GetDisplayStyle(Random random)
        {
            var cName = GenerateRandomString(random);
            return new Tuple<string, string>(cName, string.Concat(".", cName, "{display:inline;}"));
        }

        private void GenerateDummyStyles(Random random)
        {
            var cName = "";
            for (int i = 0; i < 50; i++)
            {
                cName = GenerateRandomString(random);
                var rd = random.Next(1, 3);
                if (rd == 1)
                {
                    Styles.Add(string.Concat(".", cName, "{display:inline;}"));
                }
                else
                {
                    Styles.Add(string.Concat(".", cName, "{display:none;}"));
                }
            }
        }

        private void Shuffle<T>(Random rng, List<T> array)
        {
            int n = array.Count();
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
        private string GenerateRandomString(Random random, int length = 0)
        {
            if (length == 0)
            {
                length = random.Next(5, 10);
            }

            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private string GenerateImage(Random random, int width, int height, string text)
        {

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new System.Drawing.Rectangle(0, 0, width, height);
            var color = GetColorCombination(random);
            var hatchBrush = color.Item1;
            g.FillRectangle(color.Item1, rect);
            System.Drawing.SizeF size;
            float fontSize = rect.Height + 1;
            Font font;

            do
            {
                fontSize--;
                var ffs = FontFamily.Families;
                var ffc = ffs.Count();
                var r = random.Next(0, ffc - 1);
                font = new Font(ffs[r], fontSize, FontStyle.Bold);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);
            var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            GraphicsPath path = new GraphicsPath();
            //path.AddString(this.text, font.FontFamily, (int) font.Style, 
            //    font.Size, rect, format);
            path.AddString(text, font.FontFamily, (int)font.Style, 75, rect, format);
            float v = 4F;
            System.Drawing.PointF[] points =
            {
                new System.Drawing.PointF(random.Next(rect.Width) / v, random.Next(
                   rect.Height) / v),
                new System.Drawing.PointF(rect.Width - random.Next(rect.Width) / v,
                    random.Next(rect.Height) / v),
                new System.Drawing.PointF(random.Next(rect.Width) / v,
                    rect.Height - random.Next(rect.Height) / v),
                new System.Drawing.PointF(rect.Width - random.Next(rect.Width) / v,
                    rect.Height - random.Next(rect.Height) / v)
          };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            hatchBrush = color.Item2;
            g.FillPath(hatchBrush, path);
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            var base64 = "";
            using (var ms = new MemoryStream())
            {
                using (bitmap)
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    base64 = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                }
            }
            return base64;
        }

        private Tuple<HatchBrush, HatchBrush> GetColorCombination(Random random)
        {
            var number = random.Next(1, 3);
            System.Drawing.Color c1;
            System.Drawing.Color c2;
            var r = 1;
            var g = 1;
            var b = 1;
            switch (number)
            {
                case 1:
                    r = random.Next(245, 255);
                    g = random.Next(245, 255);
                    b = random.Next(245, 255);
                    c1 = System.Drawing.Color.FromArgb(r, g, b);
                    r = random.Next(160, 245);
                    g = random.Next(160, 245);
                    b = random.Next(160, 245);
                    c2 = System.Drawing.Color.FromArgb(r, g, b);
                    break;
                case 2:
                default:
                    r = random.Next(160, 245);
                    g = random.Next(160, 245);
                    b = random.Next(160, 245);
                    c1 = System.Drawing.Color.FromArgb(r, g, b);
                    r = random.Next(245, 255);
                    g = random.Next(245, 255);
                    b = random.Next(245, 255);
                    c2 = System.Drawing.Color.FromArgb(r, g, b);
                    break;
            }
            var hatchBrush1 = new HatchBrush(HatchStyle.SmallConfetti, c1, c2);
            r = random.Next(1, 250);
            g = random.Next(1, 250);
            b = random.Next(1, 250);
            c1 = System.Drawing.Color.FromArgb(r, g, b);
            r = random.Next(1, 250);
            g = random.Next(1, 250);
            b = random.Next(1, 250);
            c2 = System.Drawing.Color.FromArgb(r, g, b);
            var hatchBrush2 = new HatchBrush(HatchStyle.Percent10, c1, c2);
            return new Tuple<HatchBrush, HatchBrush>(hatchBrush1, hatchBrush2);
        }

        public async Task<IActionResult> GenerateBarCodeId(string data, QRCodeDataTypeEnum dataType = QRCodeDataTypeEnum.Url, QRCodeTypeEnum type = QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum? referenceType = null, string referenceId = null, bool isPopup = false)
        {
            var result = await GenerateBarCode(data, dataType, type, referenceType, referenceId, isPopup);
            return Content(result.Item2);

        }
        public async Task<IActionResult> GenerateBarCodeImageId(string data, QRCodeDataTypeEnum dataType = QRCodeDataTypeEnum.Url, QRCodeTypeEnum type = QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum? referenceType = null, string referenceId = null, bool isPopup = false)
        {
            var result = await GenerateBarCode(data, dataType, type, referenceType, referenceId, isPopup);
            return Content(result.Item1);
        }
        private async Task<Tuple<string, string, MemoryStream>> GenerateBarCode(string data, QRCodeDataTypeEnum dataType = QRCodeDataTypeEnum.Url, QRCodeTypeEnum codeType = QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum? referenceType = null, string referenceId = null, bool isPopup = false)
        {
            var width = 250;
            var height = 250;
            var margin = 0;
            var barcodeFormat = (BarcodeFormat)codeType;
            if (barcodeFormat != BarcodeFormat.QR_CODE)
            {
                height = 100;
            }
            var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = barcodeFormat,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var _configuration = _sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
            var qrCodeId = Guid.NewGuid().ToString();
            var qrCodeUrl = @$"{ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration)}home/loadqrcode?id={qrCodeId}";
            using (var image = barcodeWriter.Write(qrCodeUrl))
            {
                var format = SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance;
                var ms = new MemoryStream();
                image.Save(ms, format);
                var bytes = ms.ToArray();
                var file = await _fileBusiness.Create(new FileViewModel
                {
                    ContentByte = bytes,
                    ContentType = "image/jpeg",
                    ContentLength = bytes.Length,
                    FileName = "barcode.jpg",
                    FileExtension = ".jpg"
                });


                var qrCode = await _fileBusiness.Create<QRCodeDataViewModel, QRCodeData>(new QRCodeDataViewModel
                {
                    QRCodeDataType = dataType,
                    Data = data,
                    QRCodeType = codeType,
                    QrCodeUrl = qrCodeUrl,
                    ReferenceType = referenceType,
                    ReferenceTypeId = referenceId,
                    IsPopup = isPopup,
                    Id = qrCodeId
                });
                return new Tuple<string, string, MemoryStream>(file.Item.Id, qrCode.Item.Id, ms);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ReadQrCode(IList<IFormFile> file)
        {
            try
            {
                var ms = new MemoryStream();
                file[0].OpenReadStream().CopyTo(ms);
                var bitMap = (System.DrawingCore.Bitmap)System.DrawingCore.Bitmap.FromStream(ms);
                var reader = new ZXing.ZKWeb.BarcodeReader();
                var result = Convert.ToString(reader.Decode(bitMap));
                if (result.IsNullOrEmpty())
                {
                    return Json(new { success = false, data = "No data found!" });
                }
                else
                {
                    return Json(new { success = true, data = result });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "Error! Could not scan the image" });
            }
        }
        public async Task<IActionResult> LoadQrCode(string id)
        {
            var qrCode = await _cmsBusiness.GetSingleById<QRCodeDataViewModel, QRCodeData>(id);
            if (qrCode != null)
            {
                if (qrCode.QRCodeDataType == QRCodeDataTypeEnum.Url)
                {
                    return Redirect(qrCode.Data);
                }
                else
                {
                    return View(new IdNameViewModel { Name = qrCode.Data });
                }
            }
            else
            {
                return View(new IdNameViewModel { Name = "Invalid QR Code" });
            }

        }
        public async Task<IActionResult> GetQRCodeData(string qrcodeId)
        {
            var qrCodeData = await _cmsBusiness.GetSingleById<QRCodeDataViewModel, QRCodeData>(qrcodeId);
            if (qrCodeData.IsNotNull())
            {
                return Json(new { success = true, data = qrCodeData });
            }
            else
            {
                return Json(new { success = false, data = "No data found!" });
            }
        }
        [Route("core/home/test")]
        public async Task<IActionResult> Test()
        {
            var json = "";
            var k = Helper.ReplaceJsonProperty(json, "columnMetadataId");
            return View();

        }
        public async Task<IActionResult> ProjectProposal()
        {
            return View();

        }
        public async Task<IActionResult> TestReport()
        {
            ViewBag.Orders = "61e629812b03a5f3715b7101";
            return View();

        }
        public async Task<ActionResult> GetSwitchToUserVirtualData(string page, string pageSize, dynamic filter, string filters)
        {

            var data = await _cmsBusiness.GetList<TeamViewModel, Team>();
            //if (request.Filters != null)
            //{
            //    request.Filters.Clear();
            //}
            return Json(new { Data = data.Take(40).Select(x => new { Id = x.Id, Name = x.Name }), Total = 78 });
        }
        public async Task<ActionResult> GetGridData()
        {

            var data = await _cmsBusiness.GetList<TeamViewModel, Team>();
            //if (request.Filters != null)
            //{
            //    request.Filters.Clear();
            //}
            //return Json(new { Data = data.Take(40).Select(x => new { Id = x.Id, Name = x.Name }), Total = 78 });
            return Json(data);
        }
        public async Task<ActionResult> GetSwitchUserValueMapper(string value)
        {
            var dataItemIndex = -1;
            var data = await _cmsBusiness.GetList<TeamViewModel, Team>();
            if (value != null)
            {
                var item = data.FirstOrDefault(x => x.Id == value);
                dataItemIndex = data.IndexOf(item);
            }
            return Json(dataItemIndex);
        }
        //public async Task<IActionResult> Test()
        //{

        //    var globals = new Params();
        //    globals.input = new TaskViewModel { Id = "123" };

        //    string script = @$"select c.*,t.""Name"" as ""TableName"" from public.""ColumnMetadata"" c
        //    join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""Schema""='log' 
        //    and c.""ForeignKeyConstraintName"" is not null";
        //    var b = _sp.GetService<IRepositoryQueryBase<ColumnMetadataViewModel>>();
        //    var data = await b.ExecuteQueryList<ColumnMetadataViewModel>(script, null);
        //    foreach (var item in data)
        //    {
        //        item.ForeignKeyConstraintName = item.ForeignKeyConstraintName.Replace(item.TableName.Replace("Log", ""), item.TableName);
        //        await _pageBusiness.Edit<ColumnMetadataViewModel, ColumnMetadata>(item);

        //    }
        //    return await Task.FromResult(View("UpdateTableMetadata", new IdNameViewModel()));
        //    var result = await CSharpScript.EvaluateAsync<TaskViewModel>(script, options:
        //       ScriptOptions.Default // This adds a reference, assuming MyType is a type defined within the MyCompany.MyNamespace
        //       .WithImports("Synergy.App.ViewModel")
        //       .WithReferences(typeof(TaskViewModel).Assembly), globals: globals);



        //    return await Task.FromResult(View("UpdateTableMetadata", new IdNameViewModel()));
        //}
        private string GenerateScript(string methodName, string dynamicScript)
        {

            return string.Concat(@"
            using System; 
namespace CMS.UI.Web.Controllers
{
            public class Script : IScript
            {
                ", methodName, @"
                {
                ", dynamicScript
                , @"
                }
            }
}");
        }
        private string GenerateMethod(string script)
        {

            return string.Concat(@" public bool DecisionScript(TaskTemplateViewModel taskViewModel, ServiceTemplateViewModel serviceViewModel, NoteTemplateViewModel noteViewModel, Dictionary<string, object> udf)
                {
                      ", script, @"
                }");
        }
        public async Task<IActionResult> UpdateTableMetadata()
        {
            return await Task.FromResult(View("UpdateTableMetadata", new IdNameViewModel()));
        }
        public async Task<IActionResult> UpdateTemplates()
        {

            var ntb = _sp.GetService<INoteTemplateBusiness>();
            var notes = await ntb.GetList();
            foreach (var note in notes.OrderByDescending(x => x.LastUpdatedDate))
            {
                try
                {
                    note.DataAction = DataActionEnum.Edit;
                    var template = await ntb.GetSingleById<TemplateViewModel, Template>(note.TemplateId);
                    if (template != null)
                    {
                        note.Json = template.Json;
                        await _noteTemplateBusiness.Edit(note);
                    }

                }
                catch (Exception ex)
                {

                    var error = ex.ToString();
                }

            }

            return await Task.FromResult(View("UpdateTableMetadata", new IdNameViewModel()));
        }


        public async Task<IActionResult> EncryptPassword()
        {
            var nts = _sp.GetService<INtsBusiness>();
            await nts.UpdateOverdueNts(DateTime.Now);
            var userList = await _userBusiness.GetList();
            foreach (var user in userList)
            {
                if (user.Password.IsNotNullAndNotEmpty())
                {
                    try
                    {
                        user.Password = Helper.Encrypt(user.Password);
                        user.ConfirmPassword = user.Password;
                        var result = await _userBusiness.Edit(user);
                        if (result.IsSuccess)
                        {
                            //var dec = Helper.Decrypt(user.Password);
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                        //try
                        //{
                        //    user.Password = Helper.Encrypt(user.Password);
                        //}
                        //catch (Exception ex2)
                        //{

                        //    throw;
                        //}

                    }
                }



            }
            return Content("Success");
        }
        [HttpPost]
        public async Task<IActionResult> SubmitUpdateTableMetadata(IdNameViewModel model)
        {
            await _tableMetadataBusiness.UpdateStaticTables(model.Name);
            return await Task.FromResult(View("UpdateTableMetadata", "Done"));
        }

        //[Route("portal/{portal?}/{page?}/{id?}")]
        //public async Task<IActionResult> Portal(string portal, string page, string id, string pageUrl)
        //{

        //    var runningMode = RunningModeEnum.Preview;
        //    var requestSource = RequestSourceEnum.Main;

        //    //if (mode != null && mode.ToLower() == "preview")
        //    //{
        //    //    runningMode = RunningModeEnum.Preview;
        //    //}

        //    //if (source.IsNotNullAndNotEmpty())
        //    //{
        //    //    requestSource = source.ToEnum<RequestSourceEnum>();
        //    //}
        //    var result = await LoadCms(portal, page, runningMode, requestSource, id, pageUrl);
        //    if (result != null)
        //    {
        //        return result;
        //    }
        //    return RedirectToAction("index", "content", new { @area = "cms" });
        //}

        //public async Task<IActionResult> LoadFormIndexPageGrid([DataSourceRequest] DataSourceRequest request, string indexPageTemplateId)
        //{
        //    var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, request);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        public async Task<IActionResult> LoadFormIndexPageGrid(string indexPageTemplateId)
        {
            var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, null);
            return Json(dt);
        }
        public async Task<IActionResult> LoadNoteIndexPageGrid([DataSourceRequest] DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType)
        {
            var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, request);
            return Json(dt);
            // return Json(dt.ToDataSourceResult(request));
        }

        public async Task<IActionResult> Page(string id, string pageType, string source, string dataAction, string recordId)
        {
            var page = await _pageBusiness.GetPageForExecution(id);
            if (page == null)
            {
                return Json(new { view = "", uiJson = "", dataJson = "" });
            }
            if (source.IsNullOrEmpty())
            {
                page.RequestSource = RequestSourceEnum.Main;
            }
            else
            {
                page.RequestSource = source.ToEnum<RequestSourceEnum>();
            }
            if (dataAction.IsNullOrEmpty())
            {
                page.DataAction = DataActionEnum.None;
            }
            else
            {
                page.DataAction = dataAction.ToEnum<DataActionEnum>();
            }
            page.RecordId = recordId;
            var viewName = page.PageType;
            var viewModel = await GetViewModelForPage(page, viewName);
            var enableIndex = (bool)viewModel.EnableIndexPage;
            if (page.RequestSource == RequestSourceEnum.Main)
            {
                switch (page.PageType)
                {
                    case TemplateTypeEnum.FormIndexPage:
                        break;
                    case TemplateTypeEnum.Page:
                        break;
                    case TemplateTypeEnum.Form:
                        if (enableIndex)
                        {
                            viewName = TemplateTypeEnum.FormIndexPage;
                            viewModel = await GetViewModelForPage(page, viewName);
                        }
                        break;
                    case TemplateTypeEnum.Note:
                        if (enableIndex)
                        {
                            viewName = TemplateTypeEnum.NoteIndexPage;
                            viewModel = await GetViewModelForPage(page, viewName);
                        }
                        break;
                    case TemplateTypeEnum.Task:
                        if (enableIndex)
                        {
                            viewName = TemplateTypeEnum.TaskIndexPage;
                            viewModel = await GetViewModelForPage(page, viewName);
                        }
                        break;
                    case TemplateTypeEnum.Service:
                        if (enableIndex)
                        {
                            viewName = TemplateTypeEnum.FormIndexPage;
                            viewModel = await GetViewModelForPage(page, viewName);
                        }
                        break;
                    case TemplateTypeEnum.Custom:
                        break;
                    default:
                        break;
                }
            }



            var data = await GetModelForPage(viewName, page, recordId);

            var viewStr = await RenderViewToStringAsync(viewName.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
            return Json(new { view = viewStr, uiJson = page.Template.Json, dataJson = data });
        }
        private async Task<string> GetModelForPage(TemplateTypeEnum viewName, PageViewModel page, string recordId)
        {
            if (page.DataAction != DataActionEnum.Edit)
            {
                return "{}";
            }
            switch (viewName)
            {
                case TemplateTypeEnum.Form:
                case TemplateTypeEnum.Note:
                case TemplateTypeEnum.Task:
                case TemplateTypeEnum.Service:
                    var dr = await _cmsBusiness.GetDataById(viewName, page, recordId);
                    if (dr != null)
                    {
                        return dr.ToJson();
                    }
                    return "{}";
                case TemplateTypeEnum.Custom:
                    break;
                case TemplateTypeEnum.FormIndexPage:
                case TemplateTypeEnum.Page:
                default:
                    return null;
            }
            return null;
        }

        private async Task<dynamic> GetViewModelForPage(PageViewModel page, TemplateTypeEnum viewName)
        {
            return viewName switch
            {
                TemplateTypeEnum.FormIndexPage => await GetIndexPageViewModel(page),
                TemplateTypeEnum.Form => await GetFormViewModel(page),
                TemplateTypeEnum.Note => await GetNoteViewModel(page),
                TemplateTypeEnum.Task => await GetTaskViewModel(page),
                TemplateTypeEnum.Service => await GetServiceViewModel(page),
                TemplateTypeEnum.Custom => await GetCustomViewModel(page),
                _ => await PageView(page),
            };
        }

        //public async Task<IActionResult> GetPageContent(string id)
        //{
        //    var page = await _pageBusiness.GetSingleById(id);
        //    return Json(page);
        //}

        //private async Task<IActionResult> GetPage(PageViewModel page)
        //{
        //    if (page == null)
        //    {
        //        return NotFound();
        //    }

        //    return page.PageType switch
        //    {
        //        TemplateTypeEnum.IndexPage => await IndexPageView(page),
        //        TemplateTypeEnum.Form => await FormView(page),
        //        TemplateTypeEnum.Note => await NoteView(page),
        //        TemplateTypeEnum.Task => await TaskView(page),
        //        TemplateTypeEnum.Service => await ServiceView(page),
        //        TemplateTypeEnum.Custom => await ServiceView(page),
        //        _ => await PageView(page),
        //    };
        //}

        private async Task<FormIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page)
        {
            var model = await _cmsBusiness.GetFormIndexPageViewModel(page);
            model.Page = page;
            model.PageId = page.Id;
            return model;
        }
        public async Task<FormTemplateViewModel> GetFormViewModel(PageViewModel page)
        {
            var model = await _formTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);
            model.Page = page;
            model.PageId = page.Id;
            model.DataAction = page.DataAction;
            model.RecordId = page.RecordId;
            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;
        }

        //private async Task<IActionResult> LoadCms(string portalName, string pageName, RunningModeEnum runningMode, RequestSourceEnum requestSource, string id, string pageUrl)
        //{

        //    PortalViewModel portal = null;
        //    var domain = string.Concat(Request.IsHttps ? "https://" : "http://", Request.Host.Value).ToLower();
        //    if (portalName.IsNullOrEmpty())
        //    {
        //        portal = await _portalBusiness.GetSingleGlobal(x => x.DomainName == domain);
        //        if (portal == null)
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        portal = await _portalBusiness.GetSingleGlobal(x => x.Name == portalName);
        //        if (portal == null)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    if (!Request.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Login", "Account", new { portalId = portal.Id, returnUrl = Request.Path });
        //    }
        //    portal.DomainName = domain;
        //    var page = new PageViewModel();
        //    if (pageName.IsNullOrEmpty())
        //    {
        //        page = await _pageBusiness.GetDefaultPageDataByPortal(portal, runningMode);
        //    }
        //    else
        //    {
        //        page = await _pageBusiness.GetPageDataForExecution(portal.Id, pageName, runningMode);
        //        if (page == null)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    if (page == null)
        //    {
        //        return NotFound();
        //    }
        //    var pagePermission = await _pageBusiness.GetUserPagePermission(portal.Id, page.Id);
        //    if (pagePermission == null)
        //    {
        //        var view = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/NotAuthorize.cshtml";
        //        return View(view, new LoginViewModel { PortalId = portal.Id });
        //    }
        //    var menus = await _portalBusiness.GetMenuItems(portal,null,null);
        //    if (menus != null && menus.Count > 0)
        //    {
        //        var landingPage = menus.FirstOrDefault(x => x.IsRoot == true);
        //        if (landingPage != null)
        //        {
        //            portal.LandingPage = string.Concat("/Portal/", portal.Name, "/", landingPage.Name);
        //        }
        //    }
        //    if (pageUrl.IsNotNullAndNotEmpty())
        //    {
        //        portal.LandingPage = pageUrl;
        //        var myUri = new Uri(string.Concat("http://localhost?", pageUrl));
        //        string pageType = HttpUtility.ParseQueryString(myUri.Query).Get("pageType");
        //        ViewBag.PageType = pageType;
        //    }
        //    page.Portal = portal;
        //    page.RunningMode = runningMode;
        //    page.RequestSource = requestSource;

        //    ViewBag.Menus = menus.Where(x => x.IsHidden == false).ToList();
        //    ViewBag.Title = string.Concat(page.Title, " - ", portal.DisplayName);
        //    ViewBag.Portal = portal;

        //    return View("cms", page);
        //}

        private async Task<PageTemplateViewModel> PageView(PageViewModel page)
        {
            var model = new PageTemplateViewModel();
            return model;

        }

        [HttpPost]
        public async Task<IActionResult> ManageForm(FormTemplateViewModel model)
        {

            var result = await _cmsBusiness.ManageForm(model);
            return Redirect($"~/Portal/{model.PortalName}");
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);
            tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();
            tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);

            var jsonResult = JObject.Parse(model.Json);
            foreach (var item in tabledata.ColumnMetadataView)
            {
                var valObj = jsonResult.SelectToken(item.Name);
                if (valObj != null)
                {
                    item.Value = valObj;

                }
            }

        }
        [HttpPost]
        public async Task<IActionResult> ManageNote(NoteTemplateViewModel model)
        {
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.ManageForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);
            tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();
            tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);



            var result = JObject.Parse(model.Json);
            foreach (var item in tabledata.ColumnMetadataView)
            {
                var valObj = result.SelectToken(item.Name);
                if (valObj != null)
                {
                    item.Value = valObj;

                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> ManageTask(TaskTemplateViewModel model)
        {
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.EditForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);
            tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();
            tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);



            var result = JObject.Parse(model.Json);
            foreach (var item in tabledata.ColumnMetadataView)
            {
                var valObj = result.SelectToken(item.Name);
                if (valObj != null)
                {
                    item.Value = valObj;

                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> ManageService(ServiceTemplateViewModel model)
        {
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.EditForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);
            tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();
            tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);



            var result = JObject.Parse(model.Json);
            foreach (var item in tabledata.ColumnMetadataView)
            {
                var valObj = result.SelectToken(item.Name);
                if (valObj != null)
                {
                    item.Value = valObj;

                }
            }

        }
        private Task<IActionResult> FormReadonlyView(PageViewModel page)
        {
            throw new NotImplementedException();
        }
        private async Task<NoteTemplateViewModel> GetNoteViewModel(PageViewModel page)
        {
            var model = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);
            model.Page = page;
            model.PageId = page.Id;
            model.DataAction = page.DataAction;
            model.RecordId = page.RecordId;
            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;
        }
        private async Task<TaskTemplateViewModel> GetTaskViewModel(PageViewModel page)
        {
            var model = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);
            model.Page = page;
            model.PageId = page.Id;
            model.DataAction = page.DataAction;
            model.RecordId = page.RecordId;
            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;
        }
        private async Task<ServiceTemplateViewModel> GetServiceViewModel(PageViewModel page)
        {
            var model = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);
            model.Page = page;
            model.PageId = page.Id;
            model.DataAction = page.DataAction;
            model.RecordId = page.RecordId;
            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;
        }
        private async Task<IActionResult> GetCustomViewModel(PageViewModel page)
        {
            throw new NotImplementedException();
        }

        private async Task<IActionResult> ValidateRequest(PortalViewModel portal, string pageName, string id)
        {
            var isAuth = Request.HttpContext.User.Identity.IsAuthenticated;
            if (!isAuth)
            {
                //  var loginurl = $"/Identity/Account/LogIn?returnUrl={Request.Path}&theme={portal.Theme.ToString()}";
                return RedirectToAction("Login", "Account", new { portalId = portal.Id, returnUrl = Request.Path });
            }
            return null;
        }

        //public async Task<IActionResult> cms([FromRoute] string portal , [FromRoute] string page,string id)
        //{
        //    if (!portal.IsNullOrEmpty())
        //    {

        //    }
        //    return RedirectToAction("index", "content", new { @area = "cms" });
        //}

        public async Task<IActionResult> Privacy()
        {
            var k = await _cmsBusiness.Test();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            try
            {
                //var ex = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                //if (ex != null && IsApplicationPath(ex.Path))
                //{
                //    //_logger.LogError(ex.ToString());
                //    var uc = _sp.GetService<IUserContext>();
                //    var exception = ex.Error;
                //    var pathString = ex.Path;
                //    var appError = new ApplicationError
                //    {
                //        Exception = exception.ToString(),
                //        ErrorMessage = exception.Message,
                //        Url = pathString,
                //        UserId = uc.UserId,
                //        Email = uc.Email,
                //        UserName = uc.Email
                //    };
                //    await _pageBusiness.Create<ApplicationError, ApplicationError>(appError);

                //}
            }
            catch (Exception)
            {

            }


            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool IsApplicationPath(string path)
        {
            var ret = path.Contains("/css/")
                 || path.Contains("/js/")
                 || path.Contains("/img/")
                 || path.Contains(".css")
                 || path.Contains(".js")
                 || path.Contains(".jpg")
                 || path.Contains(".png")
                 ;
            return !ret;
        }

        public async Task<IActionResult> PopupSuccess(string msg, bool erp = false, bool ebb = false, string bbUrl = "", string bbText = "", bool ecb = false, string cbUrl = "", string cbText = "")
        {
            return View(new SuccessViewModel
            {
                Message = msg,
                EnableReloadParent = erp,
                EnableBackButton = ebb,
                BackButtonUrl = bbUrl,
                BackButtonText = bbText,
                EnableCreateNewButton = ecb,
                CreateNewButtonUrl = cbUrl,
                CreateNewButtonText = cbText
            });
        }

        [HttpGet]
        public IActionResult GetEnumIdNameList(string enumType, string exculdeItem1 = "", string exculdeItem2 = "")
        {
            var list = new List<IdNameViewModel>();
            var t = string.Concat("Synergy.App.Common.", enumType, ", Synergy.App.Common");
            Type type = Type.GetType(t);
            if (type.IsEnum)
            {
                list = Enum.GetValues(type)
                    .Cast<Enum>()
                     .Where(i => i.ToString() != exculdeItem1 && i.ToString() != exculdeItem2)
                    .Select(e => new IdNameViewModel()
                    {
                        Name = e.Description(),
                        EnumId = Convert.ToInt32(e),
                        Id = Enum.GetName(type, e)
                    })
                    .ToList();
            }

            return Json(list);
        }
        public JsonResult GetEnumAsTreeList(string id, string enumType)
        {

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = "Root",
                    Name = "All",
                    DisplayName = "All",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"

                });
            }
            if (id == "Root")
            {
                var t = string.Concat("Synergy.App.Common.", enumType, ", Synergy.App.Common");
                Type type = Type.GetType(t);
                if (type.IsEnum)
                {
                    list = Enum.GetValues(type)
                   .Cast<Enum>()
                   //.Where(i => i.ToString() != exculdeItem1 && i.ToString() != exculdeItem2)
                   .Select(e => new TreeViewViewModel()
                   {
                       id = Enum.GetName(type, e),
                       Name = e.Description(),
                       DisplayName = e.Description(),
                       ParentId = id,
                       hasChildren = false,
                       expanded = false,
                       Type = "Child"
                       //Name = e.Description(),
                       //EnumId = Convert.ToInt32(e),
                       //Id = Enum.GetName(type, e)
                   })
                   .ToList();
                    //var enumList = EnumExtension.SelectListFor(typeof(type));
                    //if (enumList != null && enumList.Count() > 0)
                    //{
                    //    list.AddRange(enumList.Select(x => new TreeViewViewModel
                    //    {
                    //        id = x.Value.ToString(),
                    //        Name = x.Text.ToString(),
                    //        DisplayName = x.Text.ToString(),
                    //        ParentId = id,
                    //        hasChildren = false,
                    //        expanded = false,
                    //        Type = "Child"

                    //    }));
                    //}

                }


            }
            return Json(list.ToList());
        }

        public async Task<object> GetEnumAsFancyTreeList(string id, string enumType)
        {

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = "Root",
                    Name = "All",
                    DisplayName = "All",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"

                });
            }
            if (id == "Root")
            {
                var t = string.Concat("Synergy.App.Common.", enumType, ", Synergy.App.Common");
                Type type = Type.GetType(t);
                if (type.IsEnum)
                {
                    list = Enum.GetValues(type)
                   .Cast<Enum>()
                   //.Where(i => i.ToString() != exculdeItem1 && i.ToString() != exculdeItem2)
                   .Select(e => new TreeViewViewModel()
                   {
                       id = Enum.GetName(type, e),
                       Name = e.Description(),
                       DisplayName = e.Description(),
                       ParentId = id,
                       hasChildren = false,
                       expanded = false,
                       Type = "Child"
                       //Name = e.Description(),
                       //EnumId = Convert.ToInt32(e),
                       //Id = Enum.GetName(type, e)
                   })
                   .ToList();
                    //var enumList = EnumExtension.SelectListFor(typeof(type));
                    //if (enumList != null && enumList.Count() > 0)
                    //{
                    //    list.AddRange(enumList.Select(x => new TreeViewViewModel
                    //    {
                    //        id = x.Value.ToString(),
                    //        Name = x.Text.ToString(),
                    //        DisplayName = x.Text.ToString(),
                    //        ParentId = id,
                    //        hasChildren = false,
                    //        expanded = false,
                    //        Type = "Child"

                    //    }));
                    //}

                }


            }
            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(list.ToList().Select(x => new FileExplorerViewModel { key = x.id, title = x.Name, lazy = true }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
            //return Json(list.ToList());
        }

    }
    public interface IScript
    {
        //  bool DecisionScript(TaskTemplateViewModel taskViewModel, ServiceTemplateViewModel serviceViewModel, NoteTemplateViewModel noteViewModel, Dictionary<string, object> udf);
        int Sum(int a, int b);
    }
    public class Params
    {
        //  bool DecisionScript(TaskTemplateViewModel taskViewModel, ServiceTemplateViewModel serviceViewModel, NoteTemplateViewModel noteViewModel, Dictionary<string, object> udf);
        public TaskViewModel input { get; set; }
    }

}

