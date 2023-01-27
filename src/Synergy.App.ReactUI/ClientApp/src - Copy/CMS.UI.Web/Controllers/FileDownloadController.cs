using CMS.Business;
using CMS.Business.Interface;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Controllers
{
    public class FileDownloadController : ApplicationController
    {
        private IDocumentPermissionBusiness _DocumentPermissionBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IFileBusiness _fileBusiness;
        public FileDownloadController(IDocumentPermissionBusiness DocumentPermissionBusiness, INoteBusiness noteBusiness, IFileBusiness fileBusiness)
        {
            _DocumentPermissionBusiness = DocumentPermissionBusiness;
            _noteBusiness = noteBusiness;
            _fileBusiness = fileBusiness;
        }
        public  async Task<IActionResult> Index(string key,string Portal)
        {

            //var model = new FileViewModel();

            ViewBag.Portald = Portal;
            var data = await _DocumentPermissionBusiness.GetNoteDocumentByKey(key);
            if (data != null)
            {
                var note = await _noteBusiness.GetSingleById(data.ParentNoteId);
                if (note != null)
                {
                    note.ExpiryDate = data.ExpiryDate;
                    note.ReferenceId = data.ReferenceId;
                    note.ReferenceType = data.ReferenceType;
                    note.OwnerUserName = data.From;
                    ViewBag.TempCode = note.TemplateCode;
                    ViewBag.LogoId = data.LogoId;
                    return View(note);
                }
            }
           
                var model = new NoteViewModel();
                return View(model);
            
        }


        
        public async  Task<IActionResult> Download(string fileId,ReferenceTypeEnum Type)
        {

            if (fileId.IsNotNull())
            {
                var doc = await _fileBusiness.GetFile(fileId);
                if (doc != null)
                {
                 //   var isPhysical = doc.IsInPhysicalPath ?? false;
                    switch (Type)
                    {
                        case ReferenceTypeEnum.NTS_Note:
                            return RedirectToAction("Manage", "Note", new { area = "Nts", noteId = doc.LinkId });
                        case ReferenceTypeEnum.NTS_Task:
                            return RedirectToAction("Manage", "Task", new { area = "Nts", taskId = doc.LinkId });
                        case ReferenceTypeEnum.NTS_Service:
                            return RedirectToAction("Manage", "Service", new { area = "Nts", serviceId = doc.LinkId });
                        case ReferenceTypeEnum.File:
                        default:
                            if (doc.ContentBase64 != null)
                            {
                                return File(Convert.FromBase64String(doc.ContentBase64), "application/oc-stream", doc.FileName);
                            }
                            else if (doc.MongoFileId.IsNotNullAndNotEmpty() )
                            {
                                return File( await _fileBusiness.DownloadMongoFileByte(doc.MongoFileId), "application/oc-stream", doc.FileName);
                            }
                            else
                            {
                                return File( await _fileBusiness.GetFileByte(doc.FileName), "application/oc-stream", doc.FileName);
                            }
                    }


                }
            }


            return null;
        }
    }
}
