using Synergy.App.Business;
using Synergy.App.Common;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System.Data;
using Hangfire;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Pms")]
    public class PerformanceRatingController : ApplicationController
    {

        private readonly IUserGroupBusiness _userGroupBusiness;
        private readonly IPerformanceManagementBusiness _PerformanceManagementBusiness;
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;

        ICmsBusiness _cmsBusiness;
        IServiceProvider _serviceProvider;
        // private readonly IHangfireScheduler _hangfireScheduler;

        public PerformanceRatingController(IPerformanceManagementBusiness pmtBusiness,
         IUserContext userContext, IUserBusiness userBusiness, ICmsBusiness cmsBusiness,
         INoteBusiness noteBusiness
            , IServiceProvider serviceProvider
            // ,IHangfireScheduler hangfireScheduler
            )
        {
            _PerformanceManagementBusiness = pmtBusiness;
            _userContext = userContext;
            _cmsBusiness = cmsBusiness;
            _noteBusiness = noteBusiness;
            _serviceProvider = serviceProvider;
            //_hangfireScheduler = hangfireScheduler;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> PerformanceRating(string Id)
        {
            var model = new PerformaceRatingViewModel();
            if (Id.IsNotNullAndNotEmpty())
            {
                model = await _PerformanceManagementBusiness.GetPerformanceRatingDetails(Id);
                model.DataAction = DataActionEnum.Edit;
                if (model.ParentNoteId.IsNullOrEmpty())
                {
                    model.ParentNoteId = "0";
                }

            }
            else
            {
                model.DataAction = DataActionEnum.Create;

                model.ParentNoteId = "0";
            }
            return View("ManagePerformanceRating", model);
        }




        [HttpPost]
        public async Task<IActionResult> ManagePerformanceRating(TagCategoryViewModel model)
        {

            var exist = await _PerformanceManagementBusiness.IsRatingNameExist(model.Name, model.Id);
            if (exist != null)
            {
                return Json(new { success = false, error = "The given performance rating name already exist" });
            }


            if (model.DataAction == DataActionEnum.Create)
            {

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "PERFORMANCE_RATING";
                noteTempModel.ParentNoteId = model.ParentNoteId;

                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Status = model.Status;
                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";


                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    //if (model.TagCategoryType == TagCategoryTypeEnum.Master)
                    //{
                    //    //await _ntsTagBusiness.GenerateTagsForCategory(result.Item.NoteId);
                    //    BackgroundJob.Enqueue<HangfireScheduler>(x => x.GenerateTagsForCategory(result.Item.NoteId));
                    //}
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {

                //if (model.ParentNoteId.IsNotNullAndNotEmpty())
                //{
                //    var exist3 = await _userGroupBusiness.IsParentAssignTosourceTagExist(model.ParentNoteId, model.TagSourceId, model.Id);
                //    if (exist3 != null)
                //    {
                //        return Json(new { success = false, error = "The parent tag category already assign to tag source" });
                //    }
                //}



                // var exist = await _pmtBusiness.IsDocNameExist(model.Name, model.Id);
                // if (exist != null)
                // {
                //     return Json(new { success = false, error = "The given name already exist" });
                // }
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;


                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Status = model.Status;
                notemodel.Json = JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    if (model.TagCategoryType == TagCategoryTypeEnum.Master)
                    {
                        //await _ntsTagBusiness.GenerateTagsForCategory(result.Item.NoteId);
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.GenerateTagsForCategory(result.Item.NoteId, _userContext.ToIdentityUser()));
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }



        public async Task<ActionResult> ReadPerformanceRatingData()
        {
            //          var model = await _cmsBusiness.GetDataListByTemplate("PERFORMANCE_RATING", "")
            var model = await _PerformanceManagementBusiness.GetPerformanceRatingList();





            return Json(model);
        }


        public async Task<JsonResult> Delete(string Id)
        {
            await _PerformanceManagementBusiness.DeletePerformanceRating(Id);
            return Json("");
        }

        public IActionResult PerformanceRatingItem(string NoteId)
        {
            var model = new PerformanceRatingItemViewModel();
            model.ParentNoteId = NoteId;

            return View(model);
        }

        public async Task<IActionResult> CreateItem(string Id, string ParentNodeId)
        {
            var model = new PerformanceRatingItemViewModel();
            if (Id.IsNotNullAndNotEmpty())
            {
                model = await _PerformanceManagementBusiness.GetPerformanceRatingItemDetails(Id);
                model.DataAction = DataActionEnum.Edit;
                if (model.ParentNoteId.IsNullOrEmpty())
                {
                    model.ParentNoteId = ParentNodeId;
                }

            }
            else
            {
                model.DataAction = DataActionEnum.Create;

                model.ParentNoteId = ParentNodeId;
            }
            return View("ManageItem", model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageItem(PerformanceRatingItemViewModel model)
        {


            var exist = await _PerformanceManagementBusiness.IsRatingItemExist(model.ParentNoteId, model.Name, model.Id);
            if (exist != null)
            {
                return Json(new { success = false, error = "The given performance rating item name already exist" });
            }



            var exist1 = await _PerformanceManagementBusiness.IsRatingItemCodeExist(model.ParentNoteId, model.code.ToString(), model.Id);
            if (exist1 != null)
            {
                return Json(new { success = false, error = "The given performance rating item code already exist" });
            }


            if (model.DataAction == DataActionEnum.Create)
            {

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "PERFORMANCE_RATING_ITEM";
                noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.Status = model.Status;
                //noteTempModel.NoteSubject = model.NoteSubject;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                notemodel.Status = model.Status;
                //notemodel.NoteSubject = model.NoteSubject;

                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {




                // var exist = await _pmtBusiness.IsDocNameExist(model.Name, model.Id);
                // if (exist != null)
                // {
                //     return Json(new { success = false, error = "The given name already exist" });
                // }
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                //noteTempModel.NoteSubject = model.NoteSubject;


                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteSubject = model.NoteSubject;
                notemodel.Status = model.Status;
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        public async Task<ActionResult> ReadItemData(string ParentNoteId)
        {
            var model = await _PerformanceManagementBusiness.GetPerformanceRatingItemList(ParentNoteId);
            var j = Json(model);
            return j;

            //return Json(model.ToDataSourceResult(request));
        }

        public async Task<JsonResult> Deleteitem(string Id)
        {
            await _PerformanceManagementBusiness.DeletePerformanceRatingItem(Id);
            return Json("");
        }


        public IActionResult RatingSummaryChart()

        {
            return View();
        }

        public async Task<IActionResult> GetAllPerformanceDocument()
        {

            var model = await _PerformanceManagementBusiness.GetAllPerformanceDocument();
            return Json(model);
        }


        public async Task<IActionResult> GetAllDepartment()
        {
            var model = await _PerformanceManagementBusiness.GetAllDepartment();
            return Json(model);
        }
    }
}
