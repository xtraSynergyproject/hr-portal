using Microsoft.AspNetCore.Mvc;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Synergy.App.WebUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Controllers
{
    [Area("Core")]
    public class SecurityController : ApplicationController
    {

        private readonly IUserBusiness _userBusiness;
        private readonly IServiceProvider _sp;
        private readonly IUserContext _userContext;
        private List<string> Styles { get; set; }
        private List<string> StyleNames { get; set; }
        private int DummyDisplayList { get; set; }


        public SecurityController(

             IUserBusiness userBusiness
            , IServiceProvider sp
            , IUserContext userContext)
        {

            _userBusiness = userBusiness;
            _sp = sp;
            _userContext = userContext;
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitCaptcha(GenerateCaptchaViewModel model)
        {
            if (_userContext.IsGuestUser || _userContext.UserId.IsNullOrEmpty())
            {
                return Json(new { success = false, exceeded = true, error = "Not authorized/Session expired. Please resubmit the appointment again" });
            }
            if (ModelState.IsValid)
            {
                string[] displayImages;
                string[] actualImages;
                var vm = await _userBusiness.GetSingleById<CaptchaViewModel, Captcha>(model.CaptchaId);
                if (vm == null)
                {
                    return Json(new { success = false, exceeded = true, error = "Invalid selection" });
                }
                else
                {
                    if (vm.SubmitCount > 3)
                    {
                        return Json(new { success = false, exceeded = true, error = "You have exceeded maximum number of attempts to verify the appointment. Please resubmit the appointment again" });
                    }
                    displayImages = Convert.ToString(vm.DisplayImages).Split(",");
                    actualImages = Convert.ToString(vm.ActualImages).Split(",");
                }
                if (displayImages == null || displayImages.Length <= 0 || actualImages == null || actualImages.Length <= 0)
                {
                    return Json(new { success = false, exceeded = true, error = "Invalid selection" });
                }
                var selectedImages = model.SelectedImages.Split(",");

                foreach (var item in selectedImages)
                {
                    if (!displayImages.Contains(item))
                    {
                        return Json(new { success = false, exceeded = true, error = "Invalid selection" });
                    }
                }
                if (selectedImages.Length != actualImages.Length)
                {
                    vm.RetryCount++;
                    vm.SubmitCount++;
                    vm.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                    await _userBusiness.Edit<CaptchaViewModel, Captcha>(vm);
                    return Json(new { success = false, error = "Invalid selection" });
                }
                foreach (var item in selectedImages)
                {
                    if (!actualImages.Contains(item))
                    {
                        vm.RetryCount++;
                        vm.SubmitCount++;
                        vm.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                        await _userBusiness.Edit<CaptchaViewModel, Captcha>(vm);
                        return Json(new { success = false, error = "Invalid selection" });
                    }
                }
                vm.RetryCount++;
                vm.SubmitCount++;
                vm.IsVerified = true;
                vm.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var random = new Random();
                vm.CaptchaText = Convert.ToString(random.Next(100000, 1000000));
                await _userBusiness.Edit<CaptchaViewModel, Captcha>(vm);
                return Json(new { success = true, captcha = vm.CaptchaText });
            }
            else
            {
                return Json(new { success = false, error = "Invalid Data" });
            }
        }
        public async Task<IActionResult> GenerateCaptcha(string data)
        {
            if (_userContext.IsGuestUser || _userContext.UserId.IsNullOrEmpty())
            {
                return Unauthorized();
            }
            data = Synergy.App.Common.Helper.Decrypt(data);
            var values = data.Split(",");
            var captchaId = "";
            var referenceId = "";
            var referenceType = "";
            CaptchaViewModel vm = null;
            if (values.Length == 1)
            {
                captchaId = values[0];
                vm = await _userBusiness.GetSingleById<CaptchaViewModel, Captcha>(captchaId);
                if (vm == null)
                {
                    return BadRequest();
                }
            }
            else if (values.Length == 2)
            {
                referenceId = values[0];
                referenceType = values[1];
                vm = await _userBusiness.GetSingle<CaptchaViewModel, Captcha>(x => x.ReferenceId == referenceId && x.ReferenceType == referenceType);
                if (vm == null)
                {
                    return BadRequest();
                }
                else
                {
                    vm.RetryCount++;
                    vm.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                    await _userBusiness.Edit<CaptchaViewModel, Captcha>(vm);
                }

            }
            else
            {
                return BadRequest();
            }
            Styles = new List<string>();
            StyleNames = new List<string>();
            var model = new GenerateCaptchaViewModel();
            model.CaptchaId = captchaId;
            var random = new Random();

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

                //while (allImages.Contains(id))
                //{
                //    id = GenerateRandomString(random);
                //}

                model.Images.Add(new CaptchaImages
                {
                    ClassList = new List<string>(),
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
                //while (allImages.Contains(id))
                //{
                //    id = GenerateRandomString(random);
                //}
                model.Images.Add(new CaptchaImages
                {
                    ClassList = new List<string>(),
                    Id = id,
                    Number = randomNum,
                    Base64 = GenerateImage(random, 150, 80, Convert.ToString(randomNum))
                });
                displayImages.Add(id);
                allImages.Add(id);
            }



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
                    //while (allImages.Contains(id))
                    //{
                    //    id = GenerateRandomString(random);
                    //}
                    allImages.Add(id);
                    dummyImages.Add(id);

                    model.Images.Add(new CaptchaImages
                    {
                        ClassList = new List<string>(),
                        Id = id,
                        Number = randomNum,
                        Base64 = GenerateImage(random, 150, 80, Convert.ToString(randomNum))
                    });
                }
            }
            while (allImages.Count() < 54)
            {
                var lmt = random.Next(1, 6);
                randomNum = random.Next(100, 1000);
                allNumbers.Add(randomNum);
                for (int i = 0; i < lmt; i++)
                {
                    if (allImages.Count() >= 54)
                    {
                        break;
                    }
                    var id = GenerateRandomString(random);
                    //while (allImages.Contains(id))
                    //{
                    //    id = GenerateRandomString(random);
                    //}
                    allImages.Add(id);
                    dummyImages.Add(id);

                    model.Images.Add(new CaptchaImages
                    {
                        ClassList = new List<string>(),
                        Id = id,
                        Number = randomNum,
                        Base64 = GenerateImage(random, 150, 80, Convert.ToString(randomNum))
                    });
                }
            }

            GenerateDummyStyles(random);

            var allfns = GenerateScripts(random, allImages, displayImages, dummyImages, model);
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
            var posStyles = GeneratePositionStyle(random);
            var cnt = 0;
            var next = 0;
            foreach (var item in model.Images)
            {
                if (displayImages.Contains(item.Id))
                {
                    item.ClassList.Add(posStyles[cnt++]);
                }
                else
                {
                    next = random.Next(0, 9);
                    item.ClassList.Add(posStyles[next]);
                }
                Shuffle<string>(random, item.ClassList);
                if (item.ClassList.Any())
                {
                    item.ClassName = string.Join(" ", item.ClassList);
                }
            }
            Shuffle<string>(random, Styles);
            model.Style = string.Join("", Styles);
            vm.DisplayImages = string.Join(",", displayImages);
            vm.ActualImages = string.Join(",", actualImages);
            vm.RetryCount++;
            vm.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _userBusiness.Edit<CaptchaViewModel, Captcha>(vm);
            return View(model);
        }

        private List<string> GeneratePositionStyle(Random random)
        {
            var lst = new List<string>();
            var str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:0px;top:0px;}"));
            lst.Add(str);
            str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:0px;top:110px;}"));
            lst.Add(str);
            str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:0px;top:220px;}"));
            lst.Add(str);
            str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:110px;top:0px;}"));
            lst.Add(str);
            str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:110px;top:110px;}"));
            lst.Add(str);
            str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:110px;top:220px;}"));
            lst.Add(str);
            str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:220px;top:0px;}"));
            lst.Add(str);
            str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:220px;top:110px;}"));
            lst.Add(str);
            str = GenerateRandomString(random);
            Styles.Add(string.Concat(".", str, "{position:absolute;left:220px;top:220px;}"));
            lst.Add(str);
            for (int i = 0; i < 90; i++)
            {
                if (i % 9 == 0)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:0px;top:0px;}"));
                }
                else if (i % 9 == 1)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:0px;top:110px;}"));
                }
                else if (i % 9 == 2)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:0px;top:220px;}"));
                }
                else if (i % 9 == 3)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:110px;top:0px;}"));
                }
                else if (i % 9 == 4)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:110px;top:110px;}"));
                }
                else if (i % 9 == 5)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:110px;top:220px;}"));
                }
                else if (i % 9 == 6)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:110px;top:220px;}"));
                }
                else if (i % 9 == 7)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:220px;top:110px;}"));
                }
                else if (i % 9 == 8)
                {
                    str = GenerateRandomString(random);
                    Styles.Add(string.Concat(".", str, "{position:absolute;left:220px;top:220px;}"));
                }
            }
            return lst;
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

        private Tuple<List<string>, List<string>> GenerateScripts(Random random, List<string> allImages, List<string> displayImages, List<string> dummyImages, GenerateCaptchaViewModel model)
        {
            var fns = new List<string>();
            var fnContent = new List<string>();
            var next = random.Next(5, 20);
            for (int i = 0; i < next; i++)
            {
                var fn = GenerateReturnScript(random, allImages, model);
                fns.Add(fn.Item1);
                fnContent.Add(fn.Item2);
            }
            next = random.Next(5, 20);
            for (int i = 0; i < next; i++)
            {
                var fn = GenerateTryCatchScript(random, allImages, displayImages, model);
                fns.Add(fn.Item1);
                fnContent.Add(fn.Item2);
            }
            for (int i = 0; i < 54; i++)
            {
                GenerateActualScript(random, allImages, displayImages, dummyImages, i, fns, fnContent, model);
            }


            return new Tuple<List<string>, List<string>>(fns, fnContent);
        }

        private void GenerateActualScript(Random random, List<string> allImages, List<string> displayImages, List<string> dummyImages, int index, List<string> fns, List<string> fnContent, GenerateCaptchaViewModel model)
        {
            var img = allImages[index];
            if (displayImages.Contains(img))
            {
                var fn = GenerateShowActtualScript(random, allImages, displayImages, dummyImages, img, model);
                if (fn != null && fn.Item1.IsNotNullAndNotEmpty())
                {
                    fns.Add(fn.Item1);
                    fnContent.Add(fn.Item2);
                }

            }
            else
            {
                var fn = GenerateHideDummyScript(random, allImages, displayImages, dummyImages, img, model);
                if (fn != null && fn.Item1.IsNotNullAndNotEmpty())
                {
                    fns.Add(fn.Item1);
                    fnContent.Add(fn.Item2);
                }

            }
        }

        private Tuple<string, string> GenerateShowActtualScript(Random random, List<string> allImages, List<string> displayImages, List<string> dummyImages, string img, GenerateCaptchaViewModel model)
        {
            var imgObj = model.Images.FirstOrDefault(x => x.Id == img);
            if (imgObj != null)
            {
                var cName = GenerateRandomString(random);
                var st = string.Concat(".", cName, "{z-index:", random.Next(2000, 5000), ";}");
                Styles.Add(st);
                imgObj.ClassList.Add(cName);

                var next = random.Next(10, 30);
                for (int i = 0; i < next; i++)
                {
                    cName = GenerateRandomString(random);
                    st = string.Concat(".", cName, "{}");
                    Styles.Add(st);
                    imgObj.ClassList.Add(cName);
                }
            }
            return null;

        }
        private Tuple<string, string> GenerateHideDummyScript(Random random, List<string> allImages, List<string> displayImages, List<string> dummyImages, string img, GenerateCaptchaViewModel model)
        {
            Tuple<string, string> ret = null;
            var imgObj = model.Images.FirstOrDefault(x => x.Id == img);
            var next = random.Next(1, 3);
            if (next == 1)
            {

                if (imgObj != null)
                {
                    var cName = GenerateRandomString(random);
                    var st = string.Concat(".", cName, "{z-index:", random.Next(1000, 1500), ";}");
                    Styles.Add(st);
                    imgObj.ClassList.Add(cName);
                    next = random.Next(10, 30);
                    ret = AddDummyDisplayList(random, imgObj);
                    for (int i = 0; i < next; i++)
                    {
                        cName = GenerateRandomString(random);
                        imgObj.ClassList.Add(cName);
                        var n = random.Next(1, 4);
                        if (n == 1)
                        {
                            st = string.Concat(".", cName, "{z-index:", random.Next(1000, 1500), ";}");
                        }
                        else if (n == 2)
                        {
                            st = string.Concat(".", cName, "{display:none;}");
                        }
                        else
                        {
                            st = string.Concat(".", cName, "{display:inline;}");
                        }

                        Styles.Add(st);
                    }
                }
            }
            else
            {
                var cName = GenerateRandomString(random);
                var st = string.Concat(".", cName, "{display:none;}");
                Styles.Add(st);
                imgObj.ClassList.Add(cName);
                next = random.Next(10, 30);
                for (int i = 0; i < next; i++)
                {
                    cName = GenerateRandomString(random);
                    st = string.Concat(".", cName, "{z-index:", random.Next(1000, 9000), ";}");
                    Styles.Add(st);
                    imgObj.ClassList.Add(cName);
                }
            }

            return ret;
        }

        private Tuple<string, string> AddDummyDisplayList(Random random, CaptchaImages imgObj)
        {
            if (DummyDisplayList > 8)
            {
                return null;
            }
            var fnName = GenerateRandomString(random);
            //while (allImages.Contains(dummyName))
            //{
            //    dummyName = GenerateRandomString(random);
            //}
            var str = $"function {fnName}(){{try{{";
            str = string.Concat(str, $"$('#'+document.getElementById('", imgObj.Id, "').id).show();");

            str = string.Concat(str, "}catch(er){console.log(er);}}");
            return new Tuple<string, string>(fnName, str);
        }

        private Tuple<string, string> GenerateTryCatchScript(Random random, List<string> allImages, List<string> displayImages, GenerateCaptchaViewModel model)
        {
            var fnName = GenerateRandomString(random);
            var dummyName = GenerateRandomString(random);
            //while (allImages.Contains(dummyName))
            //{
            //    dummyName = GenerateRandomString(random);
            //}
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

        private Tuple<string, string> GenerateReturnScript(Random random, List<string> allImages, GenerateCaptchaViewModel model)
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

        //private string GetRandomCssClass(List<string> dummyStyles, Random random)
        //{
        //    Shuffle<string>(random, dummyStyles);
        //    var next = random.Next(15, dummyStyles.Count());
        //    var className = "";
        //    for (int i = next; i > next - 10; i--)
        //    {
        //        className = string.Concat(className, " ", dummyStyles[i]);
        //    }
        //    return className;
        //}

        //private Tuple<string, string> GetHiddenStyle(Random random)
        //{
        //    var cName = GenerateRandomString(random);
        //    var st = string.Concat(".", cName, "{display:none;}");
        //    Styles.Add(st);
        //    return new Tuple<string, string>(cName, st);
        //}

        //private Tuple<string, string> GetDisplayStyle(Random random)
        //{
        //    var cName = GenerateRandomString(random);
        //    var st = string.Concat(".", cName, "{display:inline;}");
        //    Styles.Add(st);
        //    return new Tuple<string, string>(cName, st);
        //}

        private void GenerateDummyStyles(Random random)
        {
            var cName = "";
            for (int i = 0; i < 150; i++)
            {
                cName = GenerateRandomString(random);
                var rd = random.Next(1, 6);
                if (rd == 1)
                {
                    Styles.Add(string.Concat(".", cName, "{display:inline;}"));
                }
                else if (rd == 2)
                {
                    Styles.Add(string.Concat(".", cName, "{position:absolute;}"));
                }
                else if (rd == 3)
                {
                    Styles.Add(string.Concat(".", cName, "{position:absolute;display:none;}"));
                }
                else if (rd == 4)
                {
                    Styles.Add(string.Concat(".", cName, "{position:absolute;top:10px;left:100px;}"));
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
            var str = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            while (StyleNames.Contains(str))
            {
                str = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            StyleNames.Add(str);
            return str;
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


    }

}

