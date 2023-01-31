using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Cms.Controllers
{
    [Route("api/cms")]
    [ApiController]
    public class CmsController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly INtsTagBusiness _ntsTagBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private INtsServiceCommentBusiness _ntsServiceCommentBusiness;
        private readonly IPageBusiness _pageBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        IFormTemplateBusiness _formTemplateBusiness;

        public CmsController(AuthSignInManager<ApplicationIdentityUser> customUserManager
                , IServiceProvider serviceProvider, IPageBusiness pageBusiness,
        INtsServiceCommentBusiness ntsServiceCommentBusiness, ITemplateBusiness templateBusiness, IFormTemplateBusiness formTemplateBusiness, ICmsBusiness cmsBusiness, IServiceBusiness serviceBusiness, INtsTagBusiness ntsTagBusiness, INoteBusiness noteBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cmsBusiness = cmsBusiness;
            _ntsTagBusiness = ntsTagBusiness;
            _noteBusiness = noteBusiness;
            _serviceBusiness = serviceBusiness;
            _ntsServiceCommentBusiness= ntsServiceCommentBusiness;
            _pageBusiness = pageBusiness;
            _formTemplateBusiness=formTemplateBusiness;
            _templateBusiness = templateBusiness;
        }


        [HttpGet]
        [Route("AddTaskTimeEntry")]
        public IActionResult AddTaskTimeEntry(string taskId, string assignTo)
        {
            var model = new TaskTimeEntryViewModel();
            model.DataAction = DataActionEnum.Create;
            model.NtsTaskId = taskId;
            model.UserId = assignTo;
            model.StartDate = System.DateTime.Now;
            model.EndDate = model.StartDate.AddDays(1);
            model.Duration = (model.EndDate - model.StartDate);
            return Ok(model);
        }


        [HttpGet]
        [Route("ReadEmailData")]
        public async Task<IActionResult> ReadEmailData(string id, string refId, ReferenceTypeEnum refType)
        {
            var model = await _cmsBusiness.ReadEmailTaskData(refId, refType);
            if (id.IsNotNullAndNotEmpty())
            {
                model = model.Where(x => x.ParentTaskId == id).ToList();
            }
            else
            {
                model = model.Where(x => x.ParentTaskId == null).ToList();
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            return Ok(json);
            //  return Json(model.ToDataSourceResult(request));
        }

        [HttpGet]
        [Route("AddTag")]
        public IActionResult AddTag(string TemplateId, string NtsId, NtsTypeEnum NtsType)
        {
            var model = new TagCategoryViewModel();
            model.TemplateId = TemplateId;
            model.NtsId = NtsId;
            model.NtsType = NtsType;
            model.DataAction = DataActionEnum.Create;
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddTagPost(NtsTagViewModel model)
        {
            if (model.Tags != null && model.Tags != null && model.Tags != null)
            {
                var tagArr = model.Tags.Split(',');
                var existinglist = await _ntsTagBusiness.GetNtsTagData(model.NtsType, model.NtsId);
                var existingTagsList = existinglist.Select(x => x.TagId).ToArray();
                if (existingTagsList.Any((item) => tagArr.Contains(item)))
                {
                    return Ok(new { success = false, error = "At least one already present. Please select again." });
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
            return Ok(new { success = true });
        }




        [HttpGet]
        [Route("DeleteServiceBookItems")]
        public async Task<ActionResult> DeleteServiceBookItems(string serviceId, string parentId, NtsTypeEnum parentType)
        {
            await _serviceBusiness.DeleteServiceBookItem(serviceId, parentId, parentType);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("DeleteNoteBookItems")]
        public async Task<ActionResult> DeleteNoteBookItems(string noteId, string parentId, NtsTypeEnum parentType)
        {
            await _noteBusiness.DeleteServiceBookItem(noteId, parentId, parentType);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("ReadServiceCommentDataList")]
        public async Task<ActionResult> ReadServiceCommentDataList(string serviceId)
        {
            var model = await _ntsServiceCommentBusiness.GetAllCommentTree(serviceId);

            return Ok(model);
        }

        [HttpGet]
        [Route("LoadFormIndexPageGrid")]
        public async Task<IActionResult> LoadFormIndexPageGrid(string indexPageTemplateId)
        {
            var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, null);
            return Ok(dt);
        }

        [HttpGet]
        [Route("LoadServiceIndexPageGrid")]
        public async Task<IActionResult> LoadServiceIndexPageGrid(NtsActiveUserTypeEnum ownerType, string indexPageTemplateId, string serviceStatusCode)
        {
            var dt = await _serviceBusiness.GetServiceIndexPageGridData(null, indexPageTemplateId, ownerType, serviceStatusCode);
            return Ok(dt);
        }

        [HttpGet]
        [Route("CreateForm")]
        public async Task<IActionResult> CreateForm(string templateId)
        {
            var model = new FormTemplateViewModel();
            model.TemplateId = templateId;
            var temp = await _formTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp == null)
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = temp;
                model.DataAction = DataActionEnum.Edit;
            }
            return Ok( model);
        }

        [HttpGet]
        [Route("CreateFormByCode")]
        public async Task<IActionResult> CreateFormByCode(string templateCode)
        {
            var model = new FormTemplateViewModel();
            var template = await _templateBusiness.GetSingle(x => x.Code == templateCode);
            //model.TemplateId = templateId;
            var temp = await _formTemplateBusiness.GetSingle(x => x.TemplateId == template.Id);
            if (temp != null)
            {
                model.DataAction = DataActionEnum.Create;
            }
            
            return Ok(model);
        }


        [HttpPost]
        [Route("ManageForm")]
        public async Task<IActionResult> ManageForm(FormTemplateViewModel model)
        {
            await Authenticate(model.UserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var page1 = await _pageBusiness.GetPageDetailsByTemplate(_userContext.PortalId, model.TemplateCode);
            var page = await _pageBusiness.GetPageForExecution(page1.Id);
            if (page != null)
            {
                page.RecordId = model.RecordId;
                page.DataAction = model.DataAction;
                page.RequestSource = RequestSourceEnum.Post;
                model.Page = page;
                return await GetForm(model);
            }
            return Ok(new { success = false, error = "Page not found" });

        }



        protected async Task<IActionResult> GetForm(FormTemplateViewModel formTemplate)
        {
            var viewModel = await GetFormViewModel(formTemplate);

            if (formTemplate.DataAction == DataActionEnum.Create)
            {
                var create = await _cmsBusiness.ManageForm(formTemplate);
                if (create.IsSuccess)
                {
                    var msg = "Item created successfully.";
                    return Ok(new { success = true, message = msg });
                }
                else
                {
                    return Ok(new { success = false, error = create.HtmlError });
                }
            }
            else if (formTemplate.DataAction == DataActionEnum.Edit)
            {
                var edit = await _cmsBusiness.ManageForm(formTemplate);
                if (edit.IsSuccess)
                {
                    var msg = "Item updated successfully.";
                    return Ok(new { success = true, message = msg });

                }
                else
                {
                    return Ok(new { success = false, error = edit.HtmlError });
                }
            }
            else if (formTemplate.DataAction == DataActionEnum.Delete)
            {
                var delete = await _cmsBusiness.ManageForm(new FormTemplateViewModel
                {
                    DataAction = DataActionEnum.Delete,
                    PageId = formTemplate.Page.Id,
                    RecordId = formTemplate.Id

                });


                var msg = "Selected item deleted successfully.";
                return Ok(new { success = true, message = msg });
            }
            else
            {
                return Ok(new { success = false, error = "Invalid action" });
            }
        }

        protected async Task<FormTemplateViewModel> GetFormViewModel(FormTemplateViewModel formTemplate)
        {
            var model = formTemplate;
            model.RecordId = formTemplate.Page.RecordId;
            if (model.Page.RequestSource != RequestSourceEnum.Main
                && (model.Page.RequestSource != RequestSourceEnum.Post || model.Page.DataAction == DataActionEnum.Delete))
            {
                await _cmsBusiness.GetFormDetails(model);
            }

            model.Page = formTemplate.Page;
            model.PageId = formTemplate.Page.Id;
            model.DataAction = formTemplate.Page.DataAction;
            model.RecordId = formTemplate.Page.RecordId;
            model.PortalName = formTemplate.Page.Portal.Name;

            //model.CustomUrl = formTemplate.Page.CustomUrl;
            //model.ReturnUrl = formTemplate.Page.ReturnUrl;
            model.TemplateCode = formTemplate.TemplateCode;
            //model.LayoutMode = formTemplate.Page.LayoutMode;
            //model.PopupCallbackMethod = formTemplate.Page.PopupCallbackMethod;
            return model;
        }

        protected async Task<FormIndexPageTemplateViewModel> GetFormIndexPageViewModel(PageViewModel page)
        {
            var model = await _cmsBusiness.GetFormIndexPageViewModel(page);
            model.Page = page;
            model.PageId = page.Id;
            model.TemplateCode = page.TemplateCodes;
            return model;
        }
    }
}
