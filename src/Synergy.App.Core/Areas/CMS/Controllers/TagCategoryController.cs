using Synergy.App.Business;
using Synergy.App.Common;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
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
    [Area("Cms")]
    public class TagCategoryController : ApplicationController
    {

        private readonly IUserGroupBusiness _userGroupBusiness;
        private readonly IPerformanceManagementBusiness _PerformanceManagementBusiness;
        private IUserGroupUserBusiness _userGroupUserBusiness;
        private readonly IPerformanceManagementBusiness _pmtBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserContext _userContext;
        private readonly IPushNotificationBusiness _notificationBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;
        ICmsBusiness _cmsBusiness;
        INtsTagBusiness _ntsTagBusiness;
        IServiceProvider _serviceProvider;

        //private readonly IHangfireScheduler _hangfireScheduler;

        public TagCategoryController(IEmailBusiness emailBusiness, IPushNotificationBusiness notificationBusiness, IPerformanceManagementBusiness pmtBusiness, IUserBusiness _userBusiness,
     IUserContext userContext, IServiceBusiness serviceBusiness, IUserBusiness userBusiness,
     ITaskBusiness taskBusiness, IUserGroupBusiness UserGroupBusiness
     , INoteBusiness noteBusiness, ICmsBusiness cmsBusiness, INtsTagBusiness ntsTagBusiness
            , IServiceProvider serviceProvider
            // , IHangfireScheduler hangfireScheduler
            )
        {
            _pmtBusiness = pmtBusiness;
            _userContext = userContext;
            _notificationBusiness = notificationBusiness;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _emailBusiness = emailBusiness;
            _cmsBusiness = cmsBusiness;
            _userGroupBusiness = UserGroupBusiness;
            _ntsTagBusiness = ntsTagBusiness;
            _serviceProvider = serviceProvider;
            // _hangfireScheduler = hangfireScheduler;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TagCategoryByTemplateType(NtsTypeEnum TemplateType, string TemplateId, string Id)
        {
            var model = new TagCategoryViewModel();
            model.TemplateId = TemplateId;
            model.NtsType = TemplateType;
            model.NtsId = Id;
            return View(model);
        }
        public IActionResult AddTag(string TemplateId, string NtsId, NtsTypeEnum NtsType)
        {
            var model = new TagCategoryViewModel();
            model.TemplateId = TemplateId;
            model.NtsId = NtsId;
            model.NtsType = NtsType;
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }

        public async Task<ActionResult> GetGridData([DataSourceRequest] DataSourceRequest request, string Id = null)
        {
            var model = await _userGroupBusiness.GetAllTagCategory();

            var j = Json(model);
            //var j = Json(model.ToDataSourceResult(request));
            return j;

        }

        public async Task<JsonResult> ReadTagCategoriesWithTag(string id, string TemplateId)
        {
            var data = new List<TagCategoryViewModel>();

            var categoriesList = await _noteBusiness.GetTagCategoryList(TemplateId);
            foreach (var category in categoriesList)
            {
                TagCategoryViewModel model = new TagCategoryViewModel();
                model.Id = category.Id;
                model.Name = category.Name;
                var tagslist = await _noteBusiness.GetTagListByCategoryId(category.Id);
                model.Tags = new List<TagCategoryViewModel>();
                foreach (var tag in tagslist)
                {
                    TagCategoryViewModel tagmodel = new TagCategoryViewModel();
                    tagmodel.Id = tag.Id;
                    tagmodel.Name = tag.Name;
                    tagmodel.ParentNoteId = category.Id;
                    tagmodel.HasChildren = false;
                    data.Add(tagmodel);
                }
                model.HasChildren = tagslist.Count() > 0 ? true : false;
                data.Add(model);
            }


            var result = data.Where(x => id.IsNotNullAndNotEmpty() ? x.ParentNoteId == id : x.ParentNoteId == null)
           .Select(item => new
           {
               id = item.Id,
               Name = item.Name,
               ParentId = item.ParentNoteId,
               hasChildren = item.HasChildren
           });
            return Json(result);
        }
        public async Task<IActionResult> TagCategory(string TagId)
        {
            var model = new TagCategoryViewModel();
            if (TagId.IsNotNullAndNotEmpty())
            {
                model = await _userGroupBusiness.GetTagCategoryDetails(TagId);
                model.DataAction = DataActionEnum.Edit;
                if (model.ParentNoteId.IsNullOrEmpty())
                {
                    model.ParentNoteId = "0";
                }

            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.TagSourceId = "0";
                model.ParentNoteId = "0";
            }
            return View("ManageTagCategory", model);
        }

        [HttpPost]
        public async Task<ActionResult> AddTag(NtsTagViewModel model)
        {
            if (model.Tags != null && model.Tags != null && model.Tags != null)
            {
                var tagArr = model.Tags.Split(',');
                var existinglist = await _ntsTagBusiness.GetNtsTagData(model.NtsType, model.NtsId);
                var existingTagsList = existinglist.Select(x => x.TagId).ToArray();
                if (existingTagsList.Any((item) => tagArr.Contains(item)))
                {
                    return Json(new { success = false, error = "At least one already present. Please select again." });
                }

                foreach (var item in tagArr)
                {
                    var tag = await _ntsTagBusiness.GetTagByNoteId(item);
                    model.TagId = item;
                    if (tag.IsNotNull())
                    {
                        model.TagSourceReferenceId = tag.TagSourceReferenceId;
                    }
                    var tC = await _noteBusiness.GetCategoryByTagId(item);
                    model.TagCategoryId = tC.Id;
                    model.Id = null;
                    if (DataActionEnum.Create == model.DataAction)
                    {
                        var result = await _ntsTagBusiness.Create(model);

                        //if (!result.IsSuccess)
                        //{
                        //    result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                        //    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                        //}
                        //else
                        //{
                        //    return Json(new { success = true, operation = "" });
                        //}
                    }
                    else
                    {
                        var result = await _ntsTagBusiness.Edit(model);

                        //if (!result.IsSuccess)
                        //{
                        //    result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                        //    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                        //}
                        //else
                        //{
                        //    return Json(new { success = true });
                        //}
                    }
                }
            }
            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> ManageTagCategory(TagCategoryViewModel model)
        {
            if (model.TextQueryCode.IsNotNullAndNotEmpty())
            {
                if (!model.TextQueryCode.Contains("select", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Json(new { success = false, error = "The given query must be a select query" });
                }
            }
            var exist = await _userGroupBusiness.IsTagCategoryNameExist(model.TagCategoryName, model.Id);
            if (exist != null)
            {
                return Json(new { success = false, error = "The given tag category name already exist" });
            }
            var exist2 = await _userGroupBusiness.IsTagCategoryCodeExist(model.TagCategoryCode, model.Id);
            if (exist2 != null)
            {
                return Json(new { success = false, error = "The given tag category code already exist" });
            }

            if (model.DataAction == DataActionEnum.Create)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "TAG_CATEGORY";
                noteTempModel.ParentNoteId = model.ParentNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

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
            else
            {
                if (model.ParentNoteId.IsNotNullAndNotEmpty())
                {
                    var exist3 = await _userGroupBusiness.IsParentAssignTosourceTagExist(model.ParentNoteId, model.TagSourceId, model.Id);
                    if (exist3 != null)
                    {
                        return Json(new { success = false, error = "The parent tag category already assign to tag source" });
                    }
                }

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

        public async Task<ActionResult> ReadTagCategoryData(PerformanceDocumentViewModel search = null)
        {
            var model = await _cmsBusiness.GetDataListByTemplate("TAG_CATEGORY", "");

            for (int i = 0; i < model.Rows.Count; i++)
            {
                string name = model.Rows[i][0].ToString();
                model.Rows[i][0] = GetCategoryName(Convert.ToInt32(name));
            }
            return Json(model);
        }

        public async Task<ActionResult> ReadNtsTagData(NtsTypeEnum NtsType, string NtsId)
        {
            var model = await _ntsTagBusiness.GetNtsTagData(NtsType, NtsId);
            return Json(model);
        }

        public static string GetCategoryName(int Index)
        {
            TagCategoryTypeEnum Categorytype = (TagCategoryTypeEnum)Index;
            return Categorytype.ToString();
        }
        public async Task<ActionResult> GetAllsourceid()
        {
            var model = await _userGroupBusiness.GetAllSourceID();
            return Json(model);
        }

        public async Task<ActionResult> GetParentTagCategory()
        {
            var model = await _userGroupBusiness.GetParentTagCategory();
            return Json(model);
        }
        [HttpGet]
        public async Task<JsonResult> GetTagCategoryList(string portalId)
        {
            var data = await _noteBusiness.GetTagIdNameList(portalId);
            return Json(data);
        }

        public async Task<JsonResult> Delete(string Id)
        {
            await _userGroupBusiness.DeleteTagCategory(Id);
            return Json(new { success = true });
        }
        public async Task<JsonResult> SyncTags(string CategoryId)
        {
            //await _ntsTagBusiness.GenerateTagsForCategory(CategoryId);
            var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
            await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.GenerateTagsForCategory(CategoryId, _userContext.ToIdentityUser()));
            return Json(new { success = true });
        }

        ///  Tag Code
        ///  

        public IActionResult Tag(string TagCategoryId)
        {
            var model = new TagViewModel();
            model.NoteId = TagCategoryId;
            ViewBag.TagCategoryId = TagCategoryId;
            return View(model);
        }

        public IActionResult TagMaster(string TagCategoryId)
        {
            var model = new TagViewModel();
            model.NoteId = TagCategoryId;
            ViewBag.TagCategoryId = TagCategoryId;
            return View(model);
        }

        public async Task<IActionResult> TagCreate(string TagId, string TagCategoryId)
        {
            var model = new TagViewModel();
            if (TagId.IsNotNullAndNotEmpty())
            {
                model = await _userGroupBusiness.GetTagEdit(TagId);
                model.DataAction = DataActionEnum.Edit;
                if (model.ParentNoteId.IsNullOrEmpty())
                {
                    model.ParentNoteId = TagCategoryId;
                }
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.ParentNoteId = TagCategoryId;
            }
            return View("ManageTag", model);
        }

        public async Task<ActionResult> ReadTagData(string TagCategoryId)
        {
            var model = await _userGroupBusiness.GetTagList(TagCategoryId);
            var j = Json(model);
            return j;
        }


        [HttpPost]
        public async Task<IActionResult> ManageTag(TagViewModel model)
        {
            var exist = await _userGroupBusiness.IsTagNameExist(model.ParentNoteId, model.NoteSubject, model.Id);
            if (exist != null)
            {
                return Json(new { success = false, error = "The given tag name already exist" });
            }

            if (model.DataAction == DataActionEnum.Create)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "TAG";
                noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.NoteSubject = model.NoteSubject;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                notemodel.NoteSubject = model.NoteSubject;

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
                noteTempModel.NoteSubject = model.NoteSubject;

                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteSubject = model.NoteSubject;
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }

        public async Task<JsonResult> DeleteTags(string Id)
        {
            await _userGroupBusiness.DeleteTag(Id);
            return Json("");
        }

        public async Task<JsonResult> DeleteTag(string Id)
        {
            await _ntsTagBusiness.Delete(Id);
            return Json("");
        }

    }
}
