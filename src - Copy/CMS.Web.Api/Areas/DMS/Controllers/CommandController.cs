using AutoMapper;
using Cms.UI.ViewModel;
using CMS.Business;
using CMS.Business.Interface.DMS;
using CMS.Common;
using CMS.UI.ViewModel;
using CMS.Web.Api.Controllers;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.DMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public CommandController(AuthSignInManager<ApplicationIdentityUser> customUserManager, Microsoft.Extensions.Configuration.IConfiguration configuration, 
         IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("CreateWorkspace")]
        public async Task<IActionResult> CreateWorkspace(string workspaceId, string id,  string parentId)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();

            var model = new WorkspaceViewModel();
            if (workspaceId.IsNotNullAndNotEmpty())
            {
                model = await _documentPermissionBusiness.GetWorkspaceEdit(workspaceId);

                model.DataAction = DataActionEnum.Edit;
                var templist = await _documentPermissionBusiness.DocumentTemplateList(workspaceId);
                var DocumentTypeIds = templist.Select(x => x.DocumentTypeIds);
                model.DocumentTypeId = DocumentTypeIds.ToArray();
                model.PreviousParentId = model.ParentNoteId;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.ParentNoteId = parentId;
                model.NoteId = id;
            }

            return Ok( model);
        }


        [HttpPost]
        [Route("ManageWorkspace")]
        public async Task<ActionResult> ManageWorkspace(WorkspaceViewModel model)
        {
            await Authenticate(model.OwnerUserId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _tableMetadataBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
            var _autoMapper = _serviceProvider.GetService<IMapper>();
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            //var _userContext = _serviceProvider.GetService<IUserContext>();
            var exist = await _documentPermissionBusiness.ValidateWorkspace(model);
            if (!exist.IsSuccess)
            {
                return Ok(new { success = false, error = exist.HtmlError });
            }
            else
            {
                var seqexist = await _documentPermissionBusiness.ValidateSequenceOrder(model);
                if (!seqexist.IsSuccess)
                {
                    return Ok(new { success = false, error = seqexist.HtmlError });
                }
                else
                {
                    if (model.DataAction == DataActionEnum.Create)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = model.DataAction;
                        noteTempModel.ActiveUserId = model.ActiveUserId;
                        noteTempModel.TemplateCode = "WORKSPACE_GENERAL";
                        //noteTempModel.ParentNoteId = model.ParentNoteId;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                        var workspaceModel = new WorkspaceViewModel()
                        {
                            DocumentTypeId = model.DocumentTypeId,
                            SequenceOrder = model.SequenceOrder,
                            LegalEntityId = model.LegalEntityId,
                            // ParentNoteId=model.ParentNoteId
                        };

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workspaceModel);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.NoteSubject = model.WorkspaceName;
                        notemodel.LegalEntityId = model.LegalEntityId;
                        notemodel.ParentNoteId = model.ParentNoteId;
                        notemodel.SequenceOrder = model.SequenceOrder;
                        model = _autoMapper.Map<NoteTemplateViewModel, WorkspaceViewModel>(notemodel, model);
                        var result = await _noteBusiness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            var noteId = result.Item.NoteId;
                            if (model.DocumentTypeId != null)
                            {
                                // var DocumentTypeIds = model.DocumentTypeId.Split(",");
                                foreach (var DTid in model.DocumentTypeId)
                                {
                                    var noteTempModel1 = new NoteTemplateViewModel();
                                    noteTempModel1.DataAction = model.DataAction;
                                    noteTempModel1.ActiveUserId = model.ActiveUserId;
                                    noteTempModel1.TemplateCode = "WORKSPACE_DOC_TYPE";
                                    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                                    var workspaceDocTypeModel = new WorkspaceDocTypeViewModel()
                                    {
                                        DocumentTypeId = DTid.ToString(),
                                        // ParentNoteId = noteId,
                                        //WorkspaceId = model.WorkspaceId
                                    };

                                    notemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workspaceDocTypeModel);
                                    notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                    notemodel1.ParentNoteId = noteId;
                                    //notemodel1.NoteSubject = model.WorkspaceName;
                                    // model = _autoMapper.Map<NoteTemplateViewModel, WorkspaceViewModel>(notemodel, model);

                                    var result1 = await _noteBusiness.ManageNote(notemodel1);

                                    if (!result1.IsSuccess)
                                    {
                                        return Ok(new { success = true });
                                    }
                                }
                            }
                            return Ok(new { success = true });
                        }
                    }

                    else
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = model.DataAction;
                        noteTempModel.NoteId = model.NoteId;
                        noteTempModel.ActiveUserId = model.ActiveUserId;
                        noteTempModel.TemplateCode = "WORKSPACE_GENERAL";

                        //noteTempModel.ParentNoteId = model.ParentNoteId;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                        var workspaceModel = new WorkspaceViewModel()
                        {
                            DocumentTypeId = model.DocumentTypeId,
                            SequenceOrder = model.SequenceOrder,
                            LegalEntityId = model.LegalEntityId,
                            // ParentNoteId=model.ParentNoteId
                        };

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workspaceModel);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.NoteSubject = model.WorkspaceName;
                        notemodel.LegalEntityId = model.LegalEntityId;
                        notemodel.ParentNoteId = model.ParentNoteId;
                        notemodel.SequenceOrder = model.SequenceOrder;
                        //model = _autoMapper.Map<NoteTemplateViewModel, WorkspaceViewModel>(notemodel, model);
                        var result = await _noteBusiness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            if (model.DocumentTypeId != null)
                            {
                                var noteId = result.Item.NoteId;
                                var templist = await _documentPermissionBusiness.DocumentTemplateList(model.WorkspaceId);
                                var existing = templist.Select(x => x.DocumentTypeIds);
                                var newids = model.DocumentTypeId;
                                var ToDelete = existing.Except(newids).ToList();
                                var ToAdd = newids.Except(existing).ToList();


                                // var DocumentTypeIds = model.DocumentTypeId.Split(",");
                                foreach (var DTid in ToAdd)
                                {
                                    var noteTempModel1 = new NoteTemplateViewModel();
                                    noteTempModel1.DataAction = DataActionEnum.Create;
                                    noteTempModel1.ActiveUserId = model.ActiveUserId;
                                    noteTempModel1.TemplateCode = "WORKSPACE_DOC_TYPE";
                                    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                                    var workspaceDocTypeModel = new WorkspaceDocTypeViewModel()
                                    {
                                        DocumentTypeId = DTid.ToString(),
                                        // ParentNoteId = noteId,
                                        //WorkspaceId = model.WorkspaceId
                                    };

                                    notemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workspaceDocTypeModel);
                                    notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                    notemodel1.ParentNoteId = noteId;
                                    //notemodel1.NoteSubject = model.WorkspaceName;

                                    var result1 = await _noteBusiness.ManageNote(notemodel1);

                                    if (!result1.IsSuccess)
                                    {
                                        return Ok(new { success = false });
                                    }

                                }

                                foreach (var Dtid in ToDelete)
                                {
                                    //var data = await _tableMetadataBusiness.GetTableDataByColumn("WORKSPACE_DOC_TYPE", "", "DocumentTypeNoteId", Dtid);
                                    var data = templist.FirstOrDefault(x => x.DocumentTypeIds == Dtid);
                                    var exnoteId = data.DocumentTypeNoteId;
                                    var deleteworkspace = await _tableMetadataBusiness.DeleteTableDataByHeaderId("WORKSPACE_DOC_TYPE", null, exnoteId);
                                }


                            }
                            return Ok(new { success = true });
                        }
                    }
                }


            }

            return Ok(new { success = false, error = ModelState });

        }

        [HttpGet]
        [Route("CreateFolder")]
        public async Task<IActionResult> ManageFolder(string id, string parentId)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            if (id.IsNotNullAndNotEmpty())
            {
                var note = await _noteBusiness.GetSingle(x => x.Id == id);
                note.DataAction = DataActionEnum.Edit;
                return Ok( note);
            }
            else
            {

                return Ok( new NoteViewModel { ParentNoteId = parentId, DataAction = DataActionEnum.Create });
            }

        }

        [HttpPost]
        [Route("ManageNewFolder")]
        public async Task<ActionResult> ManageNewFolder(NoteViewModel model)
        {
            //await Authenticate(model.UserId);
            //var _tableMetadataBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
            //var _autoMapper = _serviceProvider.GetService<IMapper>();
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            //var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            //var _userContext = _serviceProvider.GetService<IUserContext>();
            if (model.DataAction == DataActionEnum.Create)
            {

                var folderName = model.NoteSubject;
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = model.ActiveUserId;
                templateModel.DataAction = DataActionEnum.Create;
                templateModel.TemplateCode = "GENERAL_FOLDER";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = folderName;
                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel.ParentNoteId = model.ParentNoteId;
                newmodel.SequenceOrder = model.SequenceOrder;
                newmodel.OwnerUserId = model.OwnerUserId;
                var result = await _noteBusiness.ManageNote(newmodel);
                if (!result.IsSuccess)
                {
                    result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                    return Ok(new { success = false, errors = ModelState });
                }



            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var folderName = model.NoteSubject;
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = model.ActiveUserId;
                templateModel.DataAction = DataActionEnum.Edit;
                templateModel.NoteId = model.Id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = folderName;
                newmodel.SequenceOrder = model.SequenceOrder;
                newmodel.DataAction = DataActionEnum.Edit;
                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel.OwnerUserId = model.OwnerUserId;
                var result = await _noteBusiness.ManageNote(newmodel);
            }

            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("UploadFolderAndFiles")]
        public async Task<ActionResult> UploadFolderAndFiles(IEnumerable<IFormFile> files, string metaData)
        {
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            if (metaData == null)
            {
                //return Directory_Upload_Save(files);
            }

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData));
            var serializer = new DataContractJsonSerializer(typeof(ChunkMetaData1));
            ChunkMetaData1 chunkData = serializer.ReadObject(ms) as ChunkMetaData1;
            string path = String.Empty;
            // The Name of the Upload component is "files"
            CommandResult<FileViewModel> result = null;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(chunkData.FileName);
                    var divs = fileName.Split('.').ToList();
                    var ext = Path.GetExtension(chunkData.FileName);
                    MemoryStream ms1 = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms1);

                    //byte[] bytes;
                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    file.InputStream.CopyTo(memoryStream);
                    //    bytes = memoryStream.ToArray();
                    //}
                    //result = _businessFile.Create(new ViewModel.FileViewModel
                    //{
                    //    AttachmentType = AttachmentTypeEnum.File,
                    //    FileName = fileName,
                    //    FileExtension = ext,
                    //    ContentType = chunkData.ContentType,
                    //    ContentLength = file.ContentLength,
                    //    ContentByte = bytes,
                    //    // AttachmentDescription = desc
                    //}
                    result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms1.ToArray(),
                        ContentType = chunkData.ContentType,
                        ContentLength = file.Length,
                        FileName = fileName,
                        FileExtension = ext
                    }

                      );
                }
            }

            UploadedFileResult fileBlob = new UploadedFileResult();
            fileBlob.FileId = result.Item.Id.ToString();
            fileBlob.uploaded = chunkData.TotalChunks - 1 <= chunkData.ChunkIndex;
            fileBlob.fileUid = chunkData.UploadUid;
            var RelativePath = chunkData.RelativePath.Remove(chunkData.RelativePath.LastIndexOf('/') + 1);
            var folderData = RelativePath.Split('/');
            fileBlob.FolderName = folderData[folderData.Length - 2];
            var parentindexposition = Array.IndexOf(folderData, fileBlob.FolderName) - 1;
            if (parentindexposition >= 0)
            {
                fileBlob.ParentFolderName = folderData[parentindexposition];
            }
            fileBlob.RelativePath = chunkData.RelativePath;
            //fileBlob.RelativePath = RelativePath;
            return Ok(fileBlob);

        }

        //public ActionResult Directory_Upload_Save(IEnumerable<IFormFile> files)
        //{
        //    // The Name of the Upload component is "files"
        //    if (files != null)
        //    {
        //        foreach (var file in files)
        //        {
        //            // Some browsers send file names with full path.
        //            // We are only interested in the file name.
        //            var fileName = Path.GetFileName(file.FileName);
        //            var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
        //            var physicalPath = Path.Combine(baseurl, "App_Data", fileName);
        //            // The files are not actually saved in this demo
        //            // file.SaveAs(physicalPath);
        //        }
        //    }

        //    // Return an empty string to signify success
        //    return Content("");
        //}

        [HttpPost]
        [Route("ManageUploadedFile")]
        public async Task<ActionResult> ManageUploadedFile(NoteTemplateViewModel model)
        {
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            if (model.UploadedContent != null && model.UploadedContent != "")
            {
                var result = await _documentBusiness.ManageUploadedFiles(model);
                if (!result.IsSuccess)
                {
                    result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                    return Ok(new { success = false, errors = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return Ok(new { success = true, dataAction = model.DataAction.ToString(), id = model.Id });
        }

        [HttpPost]
        [Route("SavePermission")]
        public async Task<IActionResult> SavePermission(DocumentPermissionViewModel model)
        {


            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            if (model.DataAction == DataActionEnum.Create)
            {
                if (model.PermittedUserId != null)
                {
                    var cnt = await _documentPermissionBusiness.GetSingle(x => x.PermittedUserId == model.PermittedUserId && x.NoteId == model.NoteId);
                    if (cnt != null)
                    {
                        return Ok(new { success = false, error = "Selected User Already Assigned Permission" });
                    }
                }

                if (model.PermittedUserGroupId != null)
                {
                    var cnt = await _documentPermissionBusiness.GetSingle(x => x.PermittedUserGroupId == model.PermittedUserGroupId && x.NoteId == model.NoteId);
                    if (cnt != null)
                    {
                        return Ok(new { success = false, error = "Selected User Group Already Assigned Permission" });
                    }
                }

                var result = await _documentPermissionBusiness.Create(model);
                if (result.IsSuccess)
                {
                    await _documentPermissionBusiness.ManageChildPermissions(model.NoteId);
                    return Ok(new { success = true });

                }
                else
                {
                    return Ok(new { success = false, error = ModelState });
                }
            }
            else if (model.DataAction == DataActionEnum.Edit)
            {


                if (model.PermittedUserId != null)
                {
                    var cnt = await _documentPermissionBusiness.GetSingle(x => x.PermittedUserId == model.PermittedUserId && x.NoteId == model.NoteId && x.Id != model.Id);
                    if (cnt != null)
                    {
                        return Ok(new { success = false, error = "Selected User Already Assigned Permission" });
                    }
                }

                if (model.PermittedUserGroupId != null)
                {
                    var cnt = await _documentPermissionBusiness.GetSingle(x => x.PermittedUserGroupId == model.PermittedUserGroupId && x.NoteId == model.NoteId && x.Id != model.Id);
                    if (cnt != null)
                    {
                        return Ok(new { success = false, error = "Selected User Group Already Assigned Permission" });
                    }
                }
                var result = await _documentPermissionBusiness.Edit(model);


                if (result.IsSuccess)
                {
                    await _documentPermissionBusiness.ManageChildPermissionsOnEdit(model.NoteId, result.Item.Id);
                    return Ok(new { success = true });
                }
                else
                {
                    return Ok(new { success = false, error = ModelState });
                }
            }


            return Ok(new { success = true });
        }


    }
}
