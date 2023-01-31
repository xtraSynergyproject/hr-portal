using Synergy.App.Business;
using Synergy.App.Business.Interface;
using Synergy.App.Common;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.DMS.Controllers
{
    [Area("DMS")]
    public class DmsAdminController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        public DmsAdminController(IUserContext userContext, INoteBusiness noteBusiness, 
           ITemplateBusiness templateBusiness, ITemplateCategoryBusiness templateCategoryBusiness)
        {

            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _templateBusiness = templateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;

          }

        public IActionResult Index()
        {
            var model = new WorkspacePermissionGroupViewModel();
            return View("WorkSpacePermissionGroup", model);
        }
        public async Task<JsonResult> ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _noteBusiness.GetList(x => x.TemplateCode == "DMS_PERMISSION_GROUP");
            var result = model;
            //var result = model.ToDataSourceResult(request);
            return Json(result);
        }

        public async Task<JsonResult> ReadDataGrid()
        {
            var model = await _noteBusiness.GetList(x => x.TemplateCode == "DMS_PERMISSION_GROUP");
          //  var result = model.ToDataSourceResult(request);
            return Json(model);
        }

        //    if (id.IsNotNullAndNotEmpty())
        //    {
        //        var list = await _noteBusiness.GetList(x => x.TemplateCode == "DMS_PERMISSION_GROUP");
        //        var result = list.Where(x => x.ParentNoteId == id).Select(item => new
        //        {
        //            id = item.Id,
        //            Name = item.NoteSubject,
        //            ParentId = item.ParentNoteId,
        //           // hasChildren = list.Where(x => x.ParentNoteId == item.Id).Count() > 0 ? true : false,
        //        });

        //        return Json(result.ToList());
        //    }
        //    else
        //    {
        //        var list = await _noteBusiness.GetList(x => x.TemplateCode == "DMS_PERMISSION_GROUP");
        //        var result = list.Where(x => x.ParentNoteId == null).Select(item => new
        //        {
        //            id = item.Id,
        //            Name = item.NoteSubject,
        //            ParentId = item.ParentNoteId,
        //           // hasChildren = list.Where(x => x.ParentNoteId == item.Id).Count() > 0 ? true : false,
        //        });

        //        return Json(result.ToList());
        //    }

        //}


        public async Task<IActionResult> CreateWorkSpacePermissionGroup(DataActionEnum dataAction, string id)
        {
            return View("ManageWorkSpacePermissionGroup", new WorkspacePermissionGroupViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),

            });
        }

            //model.ActiveUserId = _userContext.UserId;
            //if (dataAction == DataActionEnum.Create)
            //{

            //    var templateModel = new NoteTemplateViewModel();
            //    templateModel.ActiveUserId = _userContext.UserId;
            //    templateModel.DataAction = dataAction;
            //    templateModel.TemplateCode = "DMS_PERMISSION_GROUP";
            //    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            //    model.ParentNoteId = parentId;
            //    model.NoteSubject = newmodel.NoteSubject;
            //    model.NoteDescription = newmodel.NoteDescription;
            //    model.CreatedBy = newmodel.CreatedBy;
            //    model.CreatedDate = System.DateTime.Now;
            //    model.LastUpdatedBy = newmodel.LastUpdatedBy;
            //    model.LastUpdatedDate = System.DateTime.Now;
            //    model.CompanyId = newmodel.CompanyId;
            //    model.DataAction = dataAction;
            //    return View("ManageWorkSpacePermissionGroup", );
            //}
        public async Task<IActionResult> EditWorkSpacePermissionGroup(DataActionEnum dataAction, string Id)
        {
            var member = await _noteBusiness.GetSingleById(Id);

            if (member != null)
            {
                member.DataAction = DataActionEnum.Edit;
                return View("ManageWorkSpacePermissionGroup", member);
            }
            return View("ManageWorkSpacePermissionGroup", new WorkspacePermissionGroupViewModel());
        }
        //    var templateModel = new NoteTemplateViewModel();
        //templateModel.ActiveUserId = _userContext.UserId;
        //templateModel.DataAction = dataAction;
        //templateModel.NoteId = id;
        //var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
        //model.NoteSubject = newmodel.NoteSubject;
        //model.NoteDescription = newmodel.NoteDescription;
        //model.NoteId = id;
        //model.CreatedBy = newmodel.CreatedBy;
        //model.LastUpdatedBy = newmodel.LastUpdatedBy;
        //model.LastUpdatedDate = System.DateTime.Now;
        //model.CompanyId = newmodel.CompanyId;
        //model.DataAction = dataAction;


    
           
        [HttpPost]
        public async Task<IActionResult> ManageWorkspacePermissionGroup(WorkspacePermissionGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "DMS_PERMISSION_GROUP";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    //newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                else
                {
                    var templateModel = new NoteTemplateViewModel();
                   templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    //newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }

    }
}
