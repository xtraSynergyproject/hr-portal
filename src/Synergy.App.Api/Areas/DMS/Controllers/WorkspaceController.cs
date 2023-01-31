using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Api.Areas.DMS.Models;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
//using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.DMS.Controllers
{
    [Route("dms/workspace")]
    [ApiController]
    public class WorkspaceController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private IDocumentPermissionBusiness _documentPermissionBusiness;
        private ITemplateBusiness _templateBusiness;
        private ITemplateCategoryBusiness _templateCategoryBusiness;
        private IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IDMSDocumentBusiness _documentBusiness;
        private readonly INtsTagBusiness _tagBusiness;
        private IMapper _autoMapper;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public WorkspaceController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, ITemplateBusiness templateBusiness, ITemplateCategoryBusiness templateCategoryBusiness,
            IUserContext userContext, INoteBusiness noteBusiness, IDocumentPermissionBusiness documentPermissionBusiness,
            ITableMetadataBusiness tableMetadataBusiness, IDMSDocumentBusiness documentBusiness, INtsTagBusiness tagBusiness, IMapper autoMapper) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _templateBusiness = templateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _documentPermissionBusiness = documentPermissionBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _documentBusiness = documentBusiness;
            _tagBusiness = tagBusiness;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        [Route("ReadDataGrid")]
        public async Task<ActionResult> ReadDataGrid(string userId,string portalName)
        {
            await Authenticate(userId,portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _documentPermissionBusiness.GetWorkspaceList();
            var data = model.ToList();
            return Ok(data);
        }

        [HttpGet]
        [Route("CreateWorkspace")]
        public async Task<IActionResult> Create(string userId,string portalName,string workspaceId, string id, string master, string parentId, string legalentityId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var model = new WorkspaceViewModel();
            model.LegalEntityId = legalentityId;
            model.PortalId = _userContext.PortalId;
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
                model.LegalEntityId = _userContext.LegalEntityId;
                model.ParentNoteId = parentId;
                model.NoteId = id;
                if (parentId.IsNotNullAndNotEmpty())
                {
                    var legal = await _documentPermissionBusiness.GetLegalEntity(parentId);
                    model.LegalEntityId = legal.LegalEntityId;

                }

            }

            return Ok( model);
        }
       
        [HttpPost]
        [Route("ManageWorkspace")]
        public async Task<IActionResult> Manage(WorkspaceViewModel model)
        {
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
                        noteTempModel.ActiveUserId = _userContext.UserId;
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
                                    noteTempModel1.ActiveUserId = _userContext.UserId;
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
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "WORKSPACE_GENERAL";

                        //noteTempModel.ParentNoteId = model.ParentNoteId;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var oldParentNoteId = notemodel.ParentNoteId;
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
                        var parents = await _documentBusiness.GetAllParentByNoteId(model.Id);
                        var parentids = parents.Select(x => x.Id).ToList();
                        var parentnoteIds = string.Join(",", parentids);
                        var parentnotePermissions = await _documentBusiness.GetAllNotePermissionByParentId(parentnoteIds);
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
                                    noteTempModel1.ActiveUserId = _userContext.UserId;
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
                            if (model.ParentNoteId.IsNotNullAndNotEmpty() && oldParentNoteId != model.ParentNoteId)
                            {
                                var docTags = await _tagBusiness.GetList(x => x.NtsId == model.NoteId && x.TagId == null);
                                foreach (var docTag in docTags)
                                {
                                    var tags = await _tagBusiness.GetList(x => x.TagSourceReferenceId == docTag.TagSourceReferenceId && x.TagId == null);
                                    if (tags.Count > 0)
                                    {
                                        var tagsIds = tags.Select(x => x.Id);
                                        var ids = "'" + String.Join("','", tagsIds) + "'";
                                        var res = await _documentBusiness.UpdateTagsByDocumentIds(ids);
                                    }
                                    var parentlist = await _documentBusiness.GetAllParentByNoteId(docTag.TagSourceReferenceId);
                                    foreach (var parent in parentlist)
                                    {
                                        var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = docTag.TagSourceReferenceId };
                                        await _tagBusiness.Create(tag);
                                    }
                                }
                            }
                            else if (model.ParentNoteId.IsNullOrEmpty() && oldParentNoteId.IsNotNullAndNotEmpty())
                            {
                                var docTags = await _tagBusiness.GetList(x => x.NtsId == model.NoteId && x.TagId == null);
                                foreach (var docTag in docTags)
                                {
                                    var tags = await _tagBusiness.GetList(x => x.TagSourceReferenceId == docTag.TagSourceReferenceId && x.NtsId == oldParentNoteId && x.TagId == null);
                                    if (tags.Count > 0)
                                    {
                                        var tagsIds = tags.Select(x => x.Id);
                                        var ids = "'" + String.Join("','", tagsIds) + "'";
                                        var res = await _documentBusiness.UpdateTagsByDocumentIds(ids);
                                    }
                                }
                            }
                            if (oldParentNoteId != model.ParentNoteId)
                            {
                                if (oldParentNoteId.IsNotNull())
                                {
                                    await _documentPermissionBusiness.DeleteChildPermissions(model.NoteId, parentnotePermissions);
                                }
                                if (model.ParentNoteId.IsNotNull())
                                {
                                    await _documentPermissionBusiness.ManageInheritedPermission(model.NoteId, model.ParentNoteId);
                                    await _documentPermissionBusiness.ManageChildPermissions(model.NoteId);
                                    await _documentPermissionBusiness.ManageParentPermissions(model.NoteId);
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
        [Route("GetDocumentTemplate")]
        public async Task<ActionResult> GetDocumentTemplateIdNameListByUser(string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();

            var tempCategoryId = await _templateCategoryBusiness.GetSingle(x => x.Code == "GENERAL_DOCUMENT");
            var templatlist = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategoryId.Id && x.Code != "GENERAL" && x.PortalId == _userContext.PortalId);
            return Ok(templatlist);
        }

        [HttpGet]
        [Route("GetParentWorkspace")]
        public async Task<ActionResult> ReadParentWorkspaceIdNameList(long? legalEntity, string id, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();

            var pagedata = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL" && x.PortalId == _userContext.PortalId);
            var result = pagedata.Where(x => id.IsNotNull() ? x.ParentNoteId == id : x.ParentNoteId == null)
              .Select(item => new
              {
                  id = item.Id,
                  Name = item.NoteSubject,
                  hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false
              });
            return Ok(result);
        }

        [HttpGet]
        [Route("DeleteWorkspace")]
        public async Task<ActionResult> DeleteWorkspace(string NoteId)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            //var deleteworkspace = await _tableMetadataBusiness.DeleteTableDataByHeaderId("WORKSPACE_GENERAL", null, NoteId);
            //if (deleteworkspace.IsNotNull())
            //{
            //    return Json(new { success = true });
            //}
            //return Json(new { success = false });

            var result = await _noteBusiness.DeleteWorkspace(NoteId);
            return Ok(new { success = result });
        }



        //[HttpGet]
        //[Route("ReadParentWorkspaceIdNameList")]
        //public async Task<ActionResult> ReadParentWorkspaceIdNameList(long? legalEntity, string id)
        //{
        //    var result3 = new List<object>();
        //    var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
        //    var pagedata = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL");
        //    var result = pagedata.Where(x => id.IsNotNull() ? x.ParentNoteId == id : x.ParentNoteId == null)
        //      .Select(item => new
        //      {
        //          id = item.Id,
        //          Name = item.NoteSubject,
        //          hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
        //          children = pagedata.Where(x => x.ParentNoteId == item.Id).Select(item => new
        //          {
        //              id = item.Id,
        //              Name = item.NoteSubject,
        //              hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
        //              children = pagedata.Where(x => x.ParentNoteId == item.Id).Select(item => new
        //              {
        //                  id = item.Id,
        //                  Name = item.NoteSubject,
        //                  hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
        //                  children = pagedata.Where(x => x.ParentNoteId == item.Id).Select(item => new
        //                  {
        //                      id = item.Id,
        //                      Name = item.NoteSubject,
        //                      hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
        //                      children = pagedata.Where(x => x.ParentNoteId == item.Id).Select(item => new
        //                      {
        //                          id = item.Id,
        //                          Name = item.NoteSubject,
        //                          hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
        //                          children = pagedata.Where(x => x.ParentNoteId == item.Id).ToList()
        //                      }).ToList()
        //                  }).ToList()
        //              }).ToList()
        //          }).ToList()
        //      }).ToList();


        //    return Ok(result);
        //}

        //[HttpGet]
        //[Route("GetDocumentTemplateIdNameListByUser")]
        //public async Task<ActionResult> GetDocumentTemplateIdNameListByUser()
        //{
        //    var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
        //    var _templateCategoryBusiness = _serviceProvider.GetService<ITemplateCategoryBusiness>();
        //    var tempCategoryId = await _templateCategoryBusiness.GetSingle(x => x.Code == "GENERAL_DOCUMENT");
        //    var templatlist = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategoryId.Id && x.Code != "GENERAL");
        //    return Ok(templatlist);
        //}

    }
}
