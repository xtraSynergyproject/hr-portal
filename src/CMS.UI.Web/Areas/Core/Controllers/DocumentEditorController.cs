using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.EJ2.DocumentEditor;
using Telerik.Web.Spreadsheet;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Controllers
{
    public class DocumentEditorController : ApplicationController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        //Initialize the memory cache object.
        public IMemoryCache _cache;
        private readonly IFileBusiness _fileBusiness;
        private readonly IServiceProvider _sp;

        public DocumentEditorController(IHostingEnvironment hostingEnvironment, IMemoryCache cache, IFileBusiness fileBusiness, IServiceProvider sp)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
            _fileBusiness = fileBusiness;
            _sp = sp;
            Console.WriteLine("PdfViewerController initialized");
        }

        //public PdfViewerController(IFileBusiness fileBusiness)
        //{
        //    _fileBusiness = fileBusiness;
        //    Console.WriteLine("PdfViewerController initialized");
        //}

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [AcceptVerbs("Get")]
        [HttpPost]
        [Route("api/[controller]/Import")]
        public string Import(string fileId, string versionNo)
        {
            var file = _fileBusiness.GetFileLogsDetailsByFileIdAndVersion(fileId, versionNo.ToSafeInt()).Result;
            var bytes = _fileBusiness.DownloadMongoFileByte(file.MongoFileId).Result;
            var stream = new MemoryStream(bytes);
            stream.Position = 0;

            WordDocument document = WordDocument.Load(stream, GetFormatType(file.FileExtension));
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(document);
            document.Dispose();
            return json;
        }

        [HttpPost]
        [Route("api/[controller]/ExportSFDT")]
        public void ExportSFDT([FromBody] SaveParameter data)
        {
            var doc = _fileBusiness.GetSingleById(data.fileid).Result;
            if (doc != null)
            {
                var document = WordDocument.Save(data.content, GetFormatType(doc.FileExtension));
                byte[] bytes;
                MemoryStream memoryStream = new MemoryStream();
                document.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();       
                doc.ContentByte = bytes;
                doc.LastUpdatedDate = DateTime.Now;
                doc.DataAction = DataActionEnum.Edit;
                var result = _fileBusiness.Edit(doc).Result;
                document.Close();
                memoryStream.Dispose();
            }          

        }
        public ActionResult LoadExcel(string id, string versionNo)
        {            
            var fileType = _fileBusiness.GetFileLogsDetailsByFileIdAndVersion(id, versionNo.ToSafeInt()).Result;
            var bytes1 = _fileBusiness.DownloadMongoFileByte(fileType.MongoFileId).Result;           
            using (MemoryStream ms = new MemoryStream(bytes1))
            {
                var workbook = Workbook.Load(ms, Path.GetExtension(fileType.FileName));
                return Content(workbook.ToJson(), MimeTypes.JSON);
            }
            return Content("");

        }
        [HttpPost]
        [Route("api/[controller]/SaveExcel")]
        public void SaveExcel([FromBody] SaveParameter data)
        {
            var doc = _fileBusiness.GetSingleById(data.fileid).Result;
            if (doc != null)
            {
                var workbook = Workbook.FromJson(data.content);                
                byte[] bytes;
                MemoryStream memoryStream = new MemoryStream();               
                workbook.Save(memoryStream, doc.FileExtension);
                bytes = memoryStream.ToArray();
                doc.ContentByte = bytes;
                doc.LastUpdatedDate = DateTime.Now;
                doc.DataAction = DataActionEnum.Edit;
                var result = _fileBusiness.Edit(doc).Result;
                memoryStream.Dispose();
            }

        }
        [HttpPost]
        [Route("api/[controller]/RevertFileVersion")]
        public void RevertFileVersion([FromBody] SaveParameter data)
        {
            var fileType = _fileBusiness.GetFileLogsDetailsByFileIdAndVersion(data.fileid, data.content.ToSafeInt()).Result;
            //var bytes = _fileBusiness.DownloadMongoFileByte(fileType.MongoFileId).Result;
            var doc = _fileBusiness.GetSingleById(data.fileid).Result;
            if (doc != null)
            {                
                //doc.ContentByte = bytes;
                doc.LastUpdatedDate = DateTime.Now;
                doc.DataAction = DataActionEnum.Edit;
                doc.MongoFileId = fileType.MongoFileId;
                var result = _fileBusiness.Edit(doc).Result;
               
            }

        }
        public class SaveParameter
        {
            public string content { get; set; }
            public string fileid { get; set; }
        }


        internal static FormatType GetFormatType(string format)
        {
            switch (format.ToLower())
            {
                case ".dotx":
                case ".docx":
                case ".docm":
                case ".dotm":
                    return FormatType.Docx;
                case ".dot":
                case ".doc":
                    return FormatType.Doc;
                case ".rtf":
                    return FormatType.Rtf;
                case ".txt":
                    return FormatType.Txt;
                case ".xml":
                    return FormatType.WordML;
                default:
                    return FormatType.Txt;
            }
        }


    }
}