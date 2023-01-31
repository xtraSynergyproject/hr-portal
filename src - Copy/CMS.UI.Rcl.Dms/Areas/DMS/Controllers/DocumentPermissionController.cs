using CMS.Business;
using CMS.Business.Interface;
using CMS.Common;
using CMS.Data.Model;
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

namespace CMS.UI.Web.Areas.DMS.Controllers
{
    [Area("DMS")]
    public class DocumentPermissionController : ApplicationController
    {
        private IDocumentPermissionBusiness _DocumentPermissionBusiness;
        private readonly IDMSDocumentBusiness _documentBusiness;
        private INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private ITemplateBusiness _TemplateBusiness;
        private IEmailBusiness _EmailBusiness;
        private ITableMetadataBusiness _tableMetadataBusiness;
        private ILOVBusiness _lovBusiness;
        public DocumentPermissionController(IDocumentPermissionBusiness DocumentPermissionBusiness, IDMSDocumentBusiness documentBusiness, INoteBusiness NoteBusiness, IUserContext userContext, Microsoft.Extensions.Configuration.IConfiguration configuration, ITemplateBusiness TemplateBusiness, IEmailBusiness EmailBusiness,
            ITableMetadataBusiness tableMetadataBusiness
            , ILOVBusiness lovBusiness)
        {
            _DocumentPermissionBusiness = DocumentPermissionBusiness;
            _documentBusiness = documentBusiness;
            _noteBusiness = NoteBusiness;
            _userContext = userContext;
            _configuration = configuration;
            _TemplateBusiness = TemplateBusiness;
            _EmailBusiness = EmailBusiness;
             _tableMetadataBusiness=  tableMetadataBusiness;
            _lovBusiness = lovBusiness;
        }
        public async Task<IActionResult> Index(string noteId, string workspaceId, string parentId,bool isSelfWorkspace, FolderTypeEnum folderType,string fileId)
        {
            var note = await _noteBusiness.GetSingleById(noteId);
            
            var model = new DocumentPermissionViewModel();
            if (note != null)
            {
                //bool IsSelftWorkspace = false;
                //var template = await _TemplateBusiness.GetSingleById(note.TemplateId);
                //var category = await _noteBusiness.GetSingleById<TemplateCategoryViewModel, TemplateCategory>(template.TemplateCategoryId);
                //var FolderType = category.Code == "GENERAL_DOCUMENT" ? FolderTypeEnum.Document : template.Code == "GENERAL_FOLDER" ? FolderTypeEnum.Folder : FolderTypeEnum.Workspace;
                //var permission = await _DocumentPermissionBusiness.GetNotePermissionDataForUser(note.Id);
                //if (FolderType== FolderTypeEnum.Workspace)
                //{
                //    var rowdata = await _tableMetadataBusiness.GetTableDataByColumn(template.Code, null, "NtsNoteId", note.Id);
                //    var lov = await _lovBusiness.GetSingle(x => x.Id == rowdata["TypeId"].ToString());
                //    if (lov.IsNotNull())
                //    {
                //        IsSelftWorkspace = lov.Code == "MY_WORKSPACE" ? true : false;
                //    }
                //}
                model = new DocumentPermissionViewModel
                {
                    WorkspaceId = workspaceId,
                    NoteId = noteId,
                    ParentId = parentId,
                    DisablePermissionInheritance = note.DisablePermissionInheritance,
                    //IsDocument = note.TemplateCode == "GENERAL_DOCUMENT" ? true : false,
                    IsDocument = folderType == FolderTypeEnum.Document || folderType == FolderTypeEnum.File ? true : false,
                    DocumentName = note.NoteSubject,
                    FolderType = folderType,
                    IsWorkspaceAdmin = false,
                    //IsSelfWorkspace= FolderType == FolderTypeEnum.Workspace ? IsSelftWorkspace : false,
                    IsSelfWorkspace= isSelfWorkspace,
                    //PermissionType= permission.IsNotNull() && permission.Any(x=>x.PermissionType==DmsPermissionTypeEnum.Allow)? DmsPermissionTypeEnum.Allow: DmsPermissionTypeEnum.Deny,
                    //Access= permission.IsNotNull() &&  permission.Any(x => x.Access == DmsAccessEnum.FullAccess) ? DmsAccessEnum.FullAccess : permission.Any(x => x.Access == DmsAccessEnum.Modify)? DmsAccessEnum.Modify: DmsAccessEnum.ReadOnly
                    FileId=fileId
                };
            }            
            return View(model);
        }
        public async Task<IActionResult> ViewPermission(string noteId, string workspaceId, string parentId, string Type)
        {
            // var model = new DocumentPermissionViewModel{NoteId=noteId };
            var note = await _noteBusiness.GetSingleById(noteId);
            //var note1 = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { NoteId = noteId });
            var model = new DocumentPermissionViewModel
            {
                WorkspaceId = workspaceId,
                NoteId = noteId,
                ParentId = parentId,
                DisablePermissionInheritance = note.DisablePermissionInheritance,
                //NoteType = Type,
                //IsDocument = note1.TemplateCategoryCode == "GENERAL_DOCUMENT",
                //ExpiryDate = note1.ExpiryDate,
                // DocumentName = note.Subject
            };
            return View(model);
        }
        public async Task<IActionResult> ManagePermission(string Id, string NodeId)
        {
            var model = new DocumentPermissionViewModel();
            model.NoteId = NodeId;
            model.LegalEntityId = _userContext.LegalEntityId;
            if (Id.IsNullOrEmpty())
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = await _DocumentPermissionBusiness.GetSingle(x => x.Id == Id);
                model.DataAction = DataActionEnum.Edit;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DisableParentPermission(string id, string InheritanceStatus)
        {
            await _DocumentPermissionBusiness.DisableParentPermissions(id, InheritanceStatus);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Save(DocumentPermissionViewModel model)
        {
            var note = await _noteBusiness.GetSingleById(model.NoteId);
            if (model.DataAction == DataActionEnum.Create)
            {
                if (model.PermittedUserId == null && model.PermittedUserGroupId == null)
                {
                    return Json(new { success = false, error = "Please Select User or Permission group from the list" });
                }
                if (model.PermittedUserId != null)
                {
                    var cnt = await _DocumentPermissionBusiness.GetSingle(x => x.PermittedUserId == model.PermittedUserId && x.PermissionType==model.PermissionType && x.Access == model.Access && x.AppliesTo == model.AppliesTo && x.NoteId == model.NoteId && x.IsInherited==false &&x.IsInheritedFromChild==false);
                    if (cnt != null)
                    {
                        return Json(new { success = false, error = "Selected Permission Is Already Exist!" });
                    }                   
                }
               
                if (model.PermittedUserGroupId != null)
                {
                    var cnt = await _DocumentPermissionBusiness.GetSingle(x => x.PermittedUserGroupId == model.PermittedUserGroupId && x.PermissionType == model.PermissionType && x.Access == model.Access && x.AppliesTo == model.AppliesTo && x.NoteId == model.NoteId && x.IsInherited == false && x.IsInheritedFromChild == false);
                    if (cnt != null)
                    {
                        return Json(new { success = false, error = "Selected User Group Permission Is Already Exist" });
                    }
                }
                
                if ((note.TemplateCode !="GENERAL_FOLDER" && note.TemplateCode != "WORKSPACE_GENERAL") && model.AppliesTo!=DmsAppliesToEnum.OnlyThisDocument)
                {
                    return Json(new { success = false, error = "Change Applies To OnlyThisDocument" });
                }
                else if ((note.TemplateCode == "GENERAL_FOLDER" || note.TemplateCode == "WORKSPACE_GENERAL") && model.AppliesTo == DmsAppliesToEnum.OnlyThisDocument)
                {
                    return Json(new { success = false, error = "OnlyThisDocument Is Not Applicable To Folder and Workspace" });
                }
                var result = await _DocumentPermissionBusiness.Create(model);
                if (result.IsSuccess)
                {
                    await _DocumentPermissionBusiness.ManageChildPermissions(model.NoteId);
                    await _DocumentPermissionBusiness.ManageParentPermissions(model.NoteId,result.Item.Id);
                    return Json(new { success = true });

                }
                else
                {
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                if (model.PermittedUserId == null && model.PermittedUserGroupId == null)
                {
                    return Json(new { success = false, error = "Please Select User or Permission group from the list" });
                }
                if (model.PermittedUserId != null)
                {
                    var cnt = await _DocumentPermissionBusiness.GetSingle(x => x.PermittedUserId == model.PermittedUserId && x.Id != model.Id && x.PermissionType == model.PermissionType && x.Access == model.Access && x.AppliesTo == model.AppliesTo && x.NoteId == model.NoteId && x.IsInherited == false && x.IsInheritedFromChild == false);
                    if (cnt != null)
                    {
                        return Json(new { success = false, error = "Selected Permission Is Already Exist!" });
                    }
                }

                if (model.PermittedUserGroupId != null)
                {
                    var cnt = await _DocumentPermissionBusiness.GetSingle(x => x.PermittedUserGroupId == model.PermittedUserGroupId && x.Id != model.Id && x.PermissionType == model.PermissionType && x.Access == model.Access && x.AppliesTo == model.AppliesTo && x.NoteId == model.NoteId && x.IsInherited == false && x.IsInheritedFromChild == false);
                    if (cnt != null)
                    {
                        return Json(new { success = false, error = "Selected User Group Permission Is Already Exist" });
                    }
                }
                if ((note.TemplateCode != "GENERAL_FOLDER" && note.TemplateCode != "WORKSPACE_GENERAL") && model.AppliesTo != DmsAppliesToEnum.OnlyThisDocument)
                {
                    return Json(new { success = false, error = "Change Applies To OnlyThisDocument" });
                }
                else if ((note.TemplateCode == "GENERAL_FOLDER" || note.TemplateCode == "WORKSPACE_GENERAL") && model.AppliesTo == DmsAppliesToEnum.OnlyThisDocument)
                {
                    return Json(new { success = false, error = "OnlyThisDocument Is Not Applicable To Folder and Workspace" });
                }
                var result = await _DocumentPermissionBusiness.Edit(model);


                if (result.IsSuccess)
                {
                    await _DocumentPermissionBusiness.ManageChildPermissionsOnEdit(model.NoteId, result.Item.Id);
                    await _DocumentPermissionBusiness.ManageParentPermissionsOnEdit(model.NoteId, result.Item.Id);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }

            return Json(new { success = true });
        }



        public async Task<ActionResult> GetPermissionDetails([DataSourceRequest] DataSourceRequest request, string noteId)
        {
            var model = await _DocumentPermissionBusiness.GetPermissionList(noteId);

            //foreach (var item in model)
            //{
            //    item.Access= GetCategoryName(Convert.ToInt32(Convert.ToInt32( item.Access)));
            //}

            var j = Json(model.ToDataSourceResult(request));
            return j;

            //return Json(model.ToDataSourceResult(request));
        }


        public async Task<ActionResult> GetPermissionDetails1(string noteId)
        {                    
            var model = await _DocumentPermissionBusiness.GetPermissionList(noteId);            
            return Json(model);                       
        }

        public static string GetCategoryName(int Index)
        {
            TagCategoryTypeEnum Categorytype = (TagCategoryTypeEnum)Index;
            return Categorytype.ToString();
        }

        public async Task<IActionResult> DeletePermission(string Id)
        {
            var permission = await _DocumentPermissionBusiness.GetSingleById(Id);                       
            await _DocumentPermissionBusiness.DeleteChildPermissions(permission.NoteId, new List<DocumentPermissionViewModel>(), Id);
            await _DocumentPermissionBusiness.DeleteParentPermissions(permission.NoteId, Id);
            await _DocumentPermissionBusiness.Delete(Id);
            return Json(true);
        }

        public IActionResult DMSTemplate(string parentId)
        {
            var model = new TemplateViewModel { Id = parentId };
            return View(model);
        }
        public async Task<IActionResult> ReadTemplate(string parentId)
        {
            var result = await _DocumentPermissionBusiness.GetTemplateList(parentId);
            if (result.Count > 0)
            {
                result = result.OrderBy(x => x.DisplayName).ToList();
                var j = Json(result);
                return j;
            }
            else
            {
                var j = Json(result);
                return j;
                //return Json(new { success = false, error = "Document type is not configured in workspace for this document" });
            }
        }

        public ActionResult ViewPermissionData([DataSourceRequest] DataSourceRequest request, string NoteId)
        {
            var model = _DocumentPermissionBusiness.ViewPermissionList(NoteId);
            var data = model.Result.ToList();

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ViewPermissionDataGrid(string NoteId)
        { 
//var model =await _DocumentPermissionBusiness.ViewPermissionList(NoteId);
                var model =await _DocumentPermissionBusiness.GetPermissionList(NoteId);
              //  var data = model.Result.ToList();

              //  var dsResult = data.ToDataSourceResult(request);
                return Json(model);
        }

        public async Task<ActionResult> GetDocumentShareDetails([DataSourceRequest] DataSourceRequest request, string noteId)
        {
            var model = await _DocumentPermissionBusiness.GetDocumentLinksData(noteId);

            //foreach (var item in model)
            //{
            //    item.Access= GetCategoryName(Convert.ToInt32(Convert.ToInt32( item.Access)));
            //}



            var j = Json(model.ToDataSourceResult(request));
            return j;

            //return Json(model.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDocumentShareDetailsGrid( string noteId)
        {
            var model = await _DocumentPermissionBusiness.GetDocumentLinksData(noteId);

            //foreach (var item in model)
            //{
            //    item.Access= GetCategoryName(Convert.ToInt32(Convert.ToInt32( item.Access)));
            //}



            var j = Json(model);
            return j;

            //return Json(model.ToDataSourceResult(request));
        }

        public IActionResult ExternalShare(string noteId, string workspaceId, string parentId, string documentName,string fileId)
        {

            var model = new DocumentPermissionViewModel { NoteId = noteId, ParentId = parentId, WorkspaceId = workspaceId, DocumentName = documentName,FileId=fileId };
            return View(model);

            
        }

        [HttpPost]
        public async Task<ActionResult> CreateLink(string noteId,string fileId, DateTime? expirydate)
        {

            if (expirydate >= DateTime.Today)
            {
                //var file = await _DocumentPermissionBusiness.GetFileId(noteId);

                var model = new NoteLinkShareViewModel
                {
                    Key = new Random().Next().ToString(),
                };

                var url = string.Empty;
                var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
                var urlnew = "FileDownload/Index?key=" + model.Key;
                url = baseurl + urlnew;
                model.Link = url;


                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "DOCUMENT_SHARE_LINK";
                noteTempModel.ExpiryDate = expirydate;


                noteTempModel.OwnerUserId = _userContext.UserId;
                noteTempModel.ParentNoteId = noteId;


                //noteTempModel.pa = model.ParentNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                notemodel.ExpiryDate = expirydate;
                notemodel.OwnerUserId = _userContext.UserId;
                notemodel.StartDate = DateTime.Today;
                notemodel.ParentNoteId = noteId;



                //notemodel.ReferenceId = file.DocumentId;
                notemodel.ReferenceId = fileId;
                notemodel.ReferenceType = ReferenceTypeEnum.File;



                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, Url = url, referenceId = result.Item.NoteId });

                }
                return Json(new { success = false, htmlerror = result.HtmlError });
            }
            else {
                return Json(new { sucecess = false, htmlerror = "Please select date greater then or Equal to todays date" });
                    }
        }


        [HttpPost]
        public async  Task<ActionResult> DeleteLink(string noteId)
        {
            await _DocumentPermissionBusiness.DeleteLink(noteId);

            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<ActionResult> UpdateLink(string noteId, string linkId, DateTime? expirydate, string emailTo, string link)
        {

            var data = await _noteBusiness.GetSingleById(linkId);
            string[] linkcode = link.Split('=');


            if (data.IsNotNull())
            {
                var model = new NoteLinkShareViewModel
                {
                    Link = link,
                    EmailTo = emailTo,
                    Key = linkcode[1].ToString(),
                    
                   
                };

               // var url = string.Empty;
               // var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
               // var urlnew = "FileDownload/Index?key=" + model.Key;
               // url = baseurl + urlnew;
               // model.Link = url;


                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.NoteId = linkId;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "DOCUMENT_SHARE_LINK";
                noteTempModel.ExpiryDate = expirydate;
                noteTempModel.OwnerUserId = _userContext.UserId;
                noteTempModel.ParentNoteId = data.ParentNoteId;
                


                //noteTempModel.pa = model.ParentNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                notemodel.Json = JsonConvert.SerializeObject(model);
                
                notemodel.ExpiryDate = expirydate;
                
                notemodel.OwnerUserId = _userContext.UserId;
                notemodel.ParentNoteId = data.ParentNoteId;
                notemodel.ReferenceId = data.ReferenceId;
                notemodel.ReferenceType = data.ReferenceType;



                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, Url = link, referenceId = result.Item.NoteId });

                }
                return Json(new { success = false });

            }
            return Json(new { success = false });

        }




        [HttpPost]
        public async Task<ActionResult> SendLink(string noteId, string linkId, string emailTo, string link, DateTime? expirydate)
        {
            var item = await _TemplateBusiness.GetSingle(x => x.Code == "DOCUMENT_SHARE_LINK");
            if (item != null)
            {
                var Email = new EmailViewModel
                {
                    From = _userContext.Email,
                    SenderName = _userContext.Name,
                    Operation = DataOperation.Create,
                    To = emailTo,
                    Subject = "Document Link Shared",
                    Body = link

                };
                var result =await _EmailBusiness.SendMailAsync(Email);
                if (result.IsSuccess)
                {
                  await this.UpdateLink(noteId, linkId, expirydate, emailTo, link);
                }
                //_business.CreateLogOfDocumentSharedViaEmail(emailTo, expirydate);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public async Task<JsonResult> GetPermittedTemplateList(string parentId)
        {
            var result = await _DocumentPermissionBusiness.GetTemplateList(parentId);
            if (result.Count > 0)
            {
                result = result.Where(x=>x.CategoryCode== "GENERAL_DOCUMENT").OrderBy(x => x.DisplayName).ToList();
                var j = Json(result);
                return j;
            }
            else
            {
                var j = Json(result);
                return j;                
            }
        }


    }
}
