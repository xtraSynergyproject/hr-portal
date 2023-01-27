using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.ViewModel;
using System.Net.Http.Headers;
using System.IO;
using Synergy.App.Common;
using Hangfire;
using CMS.Web;
using Microsoft.AspNetCore.Hosting;
using Synergy.App.DataModel;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Controllers
{
    [Area("Cms")]
    public class DocumentController : ApplicationController
    {
        #region "Declarations"        
        //IUserBusiness _business;
        IDocumentBusiness _documentBusiness;
        IFileBusiness _fileBusiness;
        IPushNotificationBusiness _notiificationBusiness;
        private readonly IUserContext _userContext;
        private readonly IWebHostEnvironment _HostEnvironment;

        #endregion

        #region "Constructor"
        public DocumentController(IDocumentBusiness documentBusiness
            , IFileBusiness fileBusiness, IPushNotificationBusiness notiificationBusiness, IUserContext userContext,
            IWebHostEnvironment HostEnvironment)

        {
            _documentBusiness = documentBusiness;
            _fileBusiness = fileBusiness;
            _notiificationBusiness = notiificationBusiness;
            _userContext = userContext;
            _HostEnvironment = HostEnvironment;
        }
        #endregion

        //[HttpPost]
        //public async Task<IActionResult> Save(IList<IFormFile> files)
        //{
        //    try
        //    {
        //        foreach (var file in files)
        //        {
        //            var ms = new MemoryStream();
        //            file.OpenReadStream().CopyTo(ms);
        //            var result = await _documentBusiness.Create(new DocumentViewModel
        //            {
        //                Content = ms.ToArray(),
        //                ContentType = file.ContentType,
        //                Length = file.Length,
        //                Name = file.FileName
        //            }
        //            );
        //            if (result.IsSuccess)
        //            {
        //                Response.Headers.Add("fileId", result.Item.Id);
        //                Response.Headers.Add("fileName", result.Item.Name);
        //                return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.Name });
        //            }
        //            else
        //            {
        //                Response.Headers.Add("status", "false");
        //                return Content("");
        //            }



        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Content("");
        //}

        [HttpPost]
        public async Task<IActionResult> SaveS(IList<IFormFile> AttachmentValue1)
        {
            try
            {
                foreach (var file in AttachmentValue1)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _documentBusiness.Create(new DocumentViewModel
                    {
                        Content = ms.ToArray(),
                        ContentType = file.ContentType,
                        Length = file.Length,
                        Name = file.FileName
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.Name);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.Name });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }


        [HttpPost]
        public async Task<IActionResult> Save1(IList<IFormFile> AttachmentValue1)
        {
            try
            {
                foreach (var file in AttachmentValue1)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save2(IList<IFormFile> AttachmentValue2)
        {
            try
            {
                foreach (var file in AttachmentValue2)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save3(IList<IFormFile> AttachmentValue3)
        {
            try
            {
                foreach (var file in AttachmentValue3)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save4(IList<IFormFile> AttachmentValue4)
        {
            try
            {
                foreach (var file in AttachmentValue4)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save5(IList<IFormFile> AttachmentValue5)
        {
            try
            {
                foreach (var file in AttachmentValue5)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save6(IList<IFormFile> AttachmentValue6)
        {
            try
            {
                foreach (var file in AttachmentValue6)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save7(IList<IFormFile> AttachmentValue7)
        {
            try
            {
                foreach (var file in AttachmentValue7)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save8(IList<IFormFile> AttachmentValue8)
        {
            try
            {
                foreach (var file in AttachmentValue8)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save9(IList<IFormFile> AttachmentValue9)
        {
            try
            {
                foreach (var file in AttachmentValue9)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> Save10(IList<IFormFile> AttachmentValue10)
        {
            try
            {
                foreach (var file in AttachmentValue10)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }
        [HttpPost]
        public async Task<IActionResult> SaveFile(IList<IFormFile> file)
        {
            try
            {
                //var notifyModel = new NotificationViewModel
                //{
                //    Subject = "Test Email",
                //    Body = "Test Body",
                //    From = "Compnay@gmail.com",
                //    ToUserId = _userContext.UserId,
                //    To = "shafi@extranet.ae;noorulhuthamh@gmail.com;mthamil107@gmail.com"

                //};
                //var result = await _notiificationBusiness.Create(notifyModel);
                //if (result.IsSuccess)
                //{
                //    BackgroundJob.Enqueue<HScheduler>(x => x.SendEmailUsingHangfire(result.Item.EmailUniqueId)).
                //}
                foreach (var f in file)
                {
                    var ms = new MemoryStream();
                    f.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = f.ContentType,
                        ContentLength = f.Length,
                        FileName = f.FileName,
                        FileExtension = Path.GetExtension(f.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }
        [HttpPost]
        public async Task<IActionResult> SaveFileWithReference(IList<IFormFile> file, ReferenceTypeEnum referenceType, string referenceId)
        {
            try
            {
                foreach (var f in file)
                {
                    var ms = new MemoryStream();
                    f.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = f.ContentType,
                        ContentLength = f.Length,
                        FileName = f.FileName,
                        FileExtension = Path.GetExtension(f.FileName),
                        ReferenceTypeCode = referenceType,
                        ReferenceTypeId = referenceId
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        public async Task<IActionResult> GetImage(string id)
        {

            var image = await _documentBusiness.GetSingleById(id);
            if (image != null)
            {
                return File(image.Content, "image/jpeg");
            }
            else
            {
                string webRootPath = _HostEnvironment.WebRootPath;
                string contentRootPath = _HostEnvironment.ContentRootPath;

                string path = "";
                path = Path.Combine(webRootPath, "images\\profile.jpg");

                byte[] imageByteArray = null;
                FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    imageByteArray = new byte[reader.BaseStream.Length];
                    for (int i = 0; i < reader.BaseStream.Length; i++)
                        imageByteArray[i] = reader.ReadByte();
                }
                if (imageByteArray.IsNotNull())
                {
                    return File(imageByteArray, "image/jpeg");
                }
            }
            return new EmptyResult();
        }
        public async Task<IActionResult> GetFile(string fileId, string returnUrl = "")
        {
            var doc = await _documentBusiness.GetSingleById(fileId);

            if (doc != null)
            {
                if (doc.Content != null && doc.Content.Length > 0)
                {
                    //return File(await _fileBusiness.DownloadMongoFile(doc.MongoFileId), "application/oc-stream", doc.FileName);
                    return File(doc.Content, "application/oc-stream", doc.Name);
                }
            }

            return new EmptyResult();
        }

        public async Task<IActionResult> GetImageMongo(string id)
        {

            var image = await _fileBusiness.GetFileByte(id);
            if (image != null)
            {
                return File(image, "image/jpeg");
            }
            return new EmptyResult();
        }
        public async Task<IActionResult> GetUserPhotoByUserId(string userId)
        {
            var user = await _fileBusiness.GetSingleById<UserViewModel, User>(userId);
            if (user != null)
            {
                var image = await _fileBusiness.GetFileByte(user.PhotoId);
                if (image != null)
                {
                    return File(image, "image/jpeg");
                }
            }

            return new EmptyResult();
        }
        public async Task<IActionResult> GetPageImageByName(string pageName)
        {
            var page = await _fileBusiness.GetSingle<PageViewModel, Synergy.App.DataModel.Page>(x => x.PortalId == _userContext.PortalId && x.Name == pageName);
            if (page != null)
            {
                var image = await _fileBusiness.GetFileByte(page.IconFileId);
                if (image != null)
                {
                    return File(image, "image/jpeg");
                }
            }

            return new EmptyResult();
        }
        public async Task<IActionResult> GetCategoryImageByCode(string code)
        {
            var category = await _fileBusiness.GetSingle<TemplateCategoryViewModel, TemplateCategory>(x => x.PortalId == _userContext.PortalId && x.Code == code);
            if (category != null)
            {
                var image = await _fileBusiness.GetFileByte(category.IconFileId);
                if (image != null)
                {
                    return File(image, "image/jpeg");
                }
            }

            return new EmptyResult();
        }

        public async Task<IActionResult> GetSnapMongo(string id)
        {

            var image = await _fileBusiness.GetSnapFileByte(id);
            if (image != null)
            {
                return File(image, "image/jpeg");
            }
            return new EmptyResult();
        }
        public async Task<IActionResult> GetFileMongo(string fileId, string returnUrl = "")
        {
            var doc = await _fileBusiness.GetFileByte(fileId);
            var detail = await _fileBusiness.GetSingleById(fileId);
            if (doc != null)
            {
                return File(doc, "application/oc-stream", detail.FileName);
            }
            return new EmptyResult();
        }
        public async Task<IActionResult> GetApplicationDocument(string documentCode)
        {
            var appDoc = await _fileBusiness.GetSingle<ApplicationDocumentViewModel, ApplicationDocument>(x => x.Code == documentCode);
            if (appDoc != null)
            {
                var doc = await _fileBusiness.GetFileByte(appDoc.DocumentId);
                var detail = await _fileBusiness.GetSingleById(appDoc.DocumentId);
                if (doc != null)
                {
                    return File(doc, "application/oc-stream", detail.FileName);
                }

            }
            return new EmptyResult();
        }
        public async Task<IActionResult> GetFilePreviewMongoId(string fileId, string returnUrl = "")
        {
            var doc = await _fileBusiness.GetFilePreviewByte(fileId);
            var detail = await _fileBusiness.GetSingleById(fileId);
            var filename = "";
            if (detail.IsNotNull() && detail.MongoPreviewFileId.IsNotNullAndNotEmpty())
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(detail.FileName) + ".pdf";
            }
            else
            {
                filename = detail.FileName;
            }
            if (doc != null)
            {
                return File(doc, "application/oc-stream", filename);
            }
            return new EmptyResult();
        }
        [HttpPost]
        public async Task<IActionResult> SaveImageBase64StringWithReference(FileViewModel file)
        {
            try
            {
                var result = await _fileBusiness.Create(new FileViewModel
                {
                    ContentByte = Convert.FromBase64String(file.ContentBase64.Replace("data:image/png;base64,", string.Empty)),
                    ContentType = "image/png",
                    ContentLength = 0,
                    FileName = "Image_" + System.DateTime.Now.ToString() + ".png",
                    FileExtension = Path.GetExtension(".png"),
                    ReferenceTypeCode = file.ReferenceTypeCode,
                    ReferenceTypeId = file.ReferenceTypeId
                }
                );
                if (result.IsSuccess)
                {
                    Response.Headers.Add("fileId", result.Item.Id);
                    Response.Headers.Add("fileName", result.Item.FileName);
                    return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                }
                else
                {
                    Response.Headers.Add("status", "false");
                    return Content("");
                }

            }
            catch (Exception ex)
            {

            }
            return Content("");
        }
        public async Task<JsonResult> GetExtractedData(string fileId, int pagenum, float x, float y)
        {
            var bytes = await _fileBusiness.GetFilePreviewByte(fileId);
            //string filePath = "C:\\bill.pdf";
            using (var document = PdfDocument.Open(bytes))
            {
                foreach (var page in document.GetPages().Where(x => x.Number == pagenum))
                {
                    // 0. Preprocessing
                    var letters = page.Letters; // no preprocessing

                    // 1. Extract words
                    var wordExtractor = NearestNeighbourWordExtractor.Instance;
                    var wordExtractorOptions = new NearestNeighbourWordExtractor.NearestNeighbourWordExtractorOptions()
                    {
                        Filter = (pivot, candidate) =>
                        {
                            // check if white space (default implementation of 'Filter')
                            if (string.IsNullOrWhiteSpace(candidate.Value))
                            {
                                // pivot and candidate letters cannot belong to the same word 
                                // if candidate letter is null or white space.
                                // ('FilterPivot' already checks if the pivot is null or white space by default)
                                return false;
                            }

                            // check for height difference
                            var maxHeight = Math.Max(pivot.PointSize, candidate.PointSize);
                            var minHeight = Math.Min(pivot.PointSize, candidate.PointSize);
                            if (minHeight != 0 && maxHeight / minHeight > 2.0)
                            {
                                // pivot and candidate letters cannot belong to the same word 
                                // if one letter is more than twice the size of the other.
                                return false;
                            }

                            // check for colour difference
                            var pivotRgb = pivot.Color.ToRGBValues();
                            var candidateRgb = candidate.Color.ToRGBValues();
                            if (!pivotRgb.Equals(candidateRgb))
                            {
                                // pivot and candidate letters cannot belong to the same word 
                                // if they don't have the same colour.
                                return false;
                            }

                            return true;
                        }
                    };

                    var words = wordExtractor.GetWords(letters, wordExtractorOptions);
                    var query = new System.Drawing.PointF { X = x, Y = Convert.ToSingle(page.Height - y) };
                    // 2. Segment page
                    var pageSegmenter = DocstrumBoundingBoxes.Instance;
                    var pageSegmenterOptions = new DocstrumBoundingBoxes.DocstrumBoundingBoxesOptions()
                    {

                    };

                    var textBlocks = pageSegmenter.GetBlocks(words, pageSegmenterOptions);

                    // 3. Postprocessing
                    var readingOrder = UnsupervisedReadingOrderDetector.Instance;
                    var orderedTextBlocks = readingOrder.Get(textBlocks);
                    var block = GetClosestTextBlockPoint(orderedTextBlocks.ToList(), query);
                    //// 4. Extract text
                    if (block.TextLines.Count > 1)
                    {
                        var line = GetClosestTextLinePoint(block.TextLines.ToList(), query);
                        return Json(line.Text);
                    }
                    else
                    {
                        //var word = GetClosestWordPoint(block.TextLines[0].Words.Where(x => x.Text != " ").ToList(), query);                        
                        //return Json(word.Text);
                        return Json(block.Text);
                    }


                }
            }

            return null;

        }
        public static Word GetClosestWordPoint(List<Word> points, System.Drawing.PointF query)
        {
            return points.OrderBy(x => WordDistance(query, x)).First();
        }

        public static double WordDistance(System.Drawing.PointF pt1, Word pt2)
        {
            return Math.Sqrt((pt2.BoundingBox.Top - pt1.Y) * (pt2.BoundingBox.Top - pt1.Y) + (pt2.BoundingBox.Left - pt1.X) * (pt2.BoundingBox.Left - pt1.X));

        }
        public static TextBlock GetClosestTextBlockPoint(List<TextBlock> points, System.Drawing.PointF query)
        {
            return points.OrderBy(x => TextBlockDistance(query, x)).First();
        }

        public static double TextBlockDistance(System.Drawing.PointF pt1, TextBlock pt2)
        {
            return Math.Sqrt((pt2.BoundingBox.Top - pt1.Y) * (pt2.BoundingBox.Top - pt1.Y) + (pt2.BoundingBox.Left - pt1.X) * (pt2.BoundingBox.Left - pt1.X));

        }
        public static TextLine GetClosestTextLinePoint(List<TextLine> points, System.Drawing.PointF query)
        {
            return points.OrderBy(x => TextLineDistance(query, x)).First();
        }
        public static double TextLineDistance(System.Drawing.PointF pt1, TextLine pt2)
        {
            return Math.Sqrt((pt2.BoundingBox.Top - pt1.Y) * (pt2.BoundingBox.Top - pt1.Y) + (pt2.BoundingBox.Left - pt1.X) * (pt2.BoundingBox.Left - pt1.X));

        }
    }
}
