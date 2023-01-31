using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class EmailTaskController : ApplicationController
    {
        private readonly IProjectManagementBusiness _projectManagementBusiness;
        private readonly IUserContext _userContext;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IProjectEmailSetupBusiness _projectEmailBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly IMapper _autoMapper;
        private readonly IUserBusiness _userBusiness;
        private readonly INoteBusiness _noteBusiness;

        public EmailTaskController(IProjectManagementBusiness projectManagementBusiness, IUserContext userContext,
            ITaskBusiness taskBusiness,IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness,
            IEmailBusiness emailBusiness, IProjectEmailSetupBusiness projectEmailBusiness, ILOVBusiness lovBusiness,
            IMapper autoMapper, IUserBusiness userBusiness, INoteBusiness noteBusiness)
        {
            _projectManagementBusiness = projectManagementBusiness;
            _userContext = userContext;
            _userRoleBusiness = userRoleBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _emailBusiness = emailBusiness;
            _projectEmailBusiness = projectEmailBusiness;
            _lovBusiness = lovBusiness;
            _autoMapper = autoMapper;
            _userBusiness = userBusiness;
            _noteBusiness = noteBusiness;
        }

        public ActionResult EmailTaskList(string Id)
        {
            if (Id.IsNotNullAndNotEmpty())
            {
                ViewBag.Id = Id;
            }
            return View();
        }

        public ActionResult SelfEmails(string Id)
        {
            if (Id.IsNotNullAndNotEmpty())
            {
                ViewBag.Id = Id;
            }
            return View();
        }

        public ActionResult CompanyEmails(string Id)
        {
            if (Id.IsNotNullAndNotEmpty())
            {
                ViewBag.Id = Id;
            }
            return View();
        }

        public ActionResult ProjectEmails(string Id)
        {
            if (Id.IsNotNullAndNotEmpty())
            {
                ViewBag.Id = Id;
            }
            return View();
        }

        public ActionResult EmailInbox(string module,string Prms,string NtsType,string Templatecode,string cbm)
        {
            ViewBag.Module = module;
            ViewBag.Prms = Prms;
            ViewBag.NtsType = "Note";
            ViewBag.TemplateCode = Templatecode;
            ViewBag.cbm = cbm;
            if (_userContext.IsSystemAdmin)
            {
                ViewBag.Admin = true;
            }
            else
            {
                ViewBag.Admin = false;
            }
            return View();
        }
        public async Task<JsonResult> ReadEmailTasks([DataSourceRequest] DataSourceRequest request, string id,string search)
        {
            // var result = new List<MailViewModel>();
            //var result =await _projectManagementBusiness.ReadEmailTaskData(_userContext.UserId);
            if (search.IsNotNullAndNotEmpty())
            {
                var result = await _emailBusiness.SearchEmailInbox(search);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                var Skip = request.PageSize * (request.Page - 1);
                var Take = request.PageSize;
                if (id.IsNullOrEmptyOrWhiteSpace())
                {
                    id = "INBOX";
                }
                var result = await _emailBusiness.ReceiveEmailInbox(id, Skip, Take);

                var result1 = new DataSourceResult()
                {
                    Data = result,
                    Total = result.Count == 0 ? 0 : result.FirstOrDefault().Total,
                };
                return Json(result1);
            }
            //return Json(result1, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ReadEmailTasks1(string id, string search)
        {
            // var result = new List<MailViewModel>();
            //var result =await _projectManagementBusiness.ReadEmailTaskData(_userContext.UserId);
            if (search.IsNotNullAndNotEmpty())
            {
                var result = await _emailBusiness.SearchEmailInbox(search);
                return Json(result);
            }
            else
            {                
                if (id.IsNullOrEmptyOrWhiteSpace())
                {
                    id = "INBOX";
                }
                var result = await _emailBusiness.ReceiveEmailInbox(id,0,1000000);
                
                return Json(result);
            }
            //return Json(result1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveEmailToNtsType(string NtsType,string Id,string templateCode, string prms)
        {
            var result = await _emailBusiness.ReceiveEmailById(Id);
            var TypeId = "";
            var TemplateCode = "";
            prms = prms.Replace("&amp;", "&");

            if (NtsType == "Note")
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.AttachmentIds = result.AttachmentIds;

                noteTempModel.Prms = Helper.QueryStringToDictionary(prms);

                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = templateCode;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                notemodel.NoteDescription = result.Body;
                notemodel.NoteSubject = result.Subject;              

                var result1 = await _noteBusiness.ManageNote(notemodel);
                if (result1.IsSuccess)
                {

                    TypeId = result1.Item.NoteId;
                }
            }
            else if (NtsType == "Service")
            {
                ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                serviceModel.ActiveUserId = _userContext.UserId;
                serviceModel.TemplateCode = templateCode;
                serviceModel.Prms = Helper.QueryStringToDictionary(prms);

                var service = await _serviceBusiness.GetServiceDetails(serviceModel);
                service.LastUpdatedBy = _userContext.UserId;
                service.DataAction = DataActionEnum.Create;
                service.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                
                //service.Json = JsonConvert.SerializeObject(assResultModel);

                var res = await _serviceBusiness.ManageService(service);
                if (res.IsSuccess)
                {
                    TypeId = res.Item.ServiceId;
                }
            }else if (NtsType=="Task")
            {
                var taskTempModel = new TaskTemplateViewModel();
                
                taskTempModel.TemplateCode =templateCode;
                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                // for line manager line.ManagerUserId
                //stepmodel.AssignedToUserId = line.ManagerUserId;
                stepmodel.OwnerUserId = _userContext.UserId;
                stepmodel.StartDate = DateTime.Now.Date;
                stepmodel.DueDate = DateTime.Now.AddDays(10);
                stepmodel.DataAction = DataActionEnum.Create;
                
                stepmodel.TaskStatusCode = "TASK_STATUS_DRAFT";
                //stepmodel.Json = "{}";
             //   dynamic exo = new System.Dynamic.ExpandoObject();

                //((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);

//                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var resss=  await _taskBusiness.ManageTask(stepmodel);
                if (resss.IsSuccess)
                {
                    TypeId = resss.Item.Id;
                }
            }
            return Json(new { success = true,Id=TypeId,TemplateCode=templateCode,Attachmentids=result.AttachmentIds});
        }

        public async Task<JsonResult> ReadCompanyEmail(string id)
        {
            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                id = "INBOX";
            }
            var result = await _emailBusiness.ReceiveEmailCompanyInbox(id);
            return Json(result);
        }
        public async Task<JsonResult> ReadProjectEmail(string id,string projectid)
        {
            var result = new List<MessageEmailViewModel>();
            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                id = "INBOX";
            }
            if (projectid.IsNotNullAndNotEmpty())
            {
                result = await _emailBusiness.ReceiveEmailProjectInbox(id, projectid);
            }
            return Json(result);
        }

        public async Task<JsonResult> ReadProjectEmailSetups([DataSourceRequest] DataSourceRequest request)
        {
 
             var projects = await _projectManagementBusiness.GetProjectsList(_userContext.UserId,true);
            var result = new List<IdNameViewModel>();
            foreach(var ids in projects)
            {
                var emailsetup = await _projectEmailBusiness.GetSingle(x => x.ServiceId == ids.Id);
                if (emailsetup.IsNotNull())
                {
                    result.Add(new IdNameViewModel()
                    {
                        Id = emailsetup.ServiceId,
                        Name = ids.Name
                    });
                }
   
            }
            return Json(result);
        }
        public ActionResult ViewProjectWithWBSItem(string id)
        {
            ViewBag.EmailTaskId = id;
            return View();
        }
        public async Task<ActionResult> ReadWBSGridData([DataSourceRequest] DataSourceRequest request, string projectId, string emailTaskId)
        {
            var result =await _projectManagementBusiness.ReadProjectTaskForEmailList(projectId);
            // _pmtBusiness.GetWbsGridData(projectId.Value, wbsItemtemplateId.Value);
            //var filtered = new List<WBSViewModel>();            
            var filtered = result.Where(x=>x.Id != emailTaskId);            
            return Json(filtered.ToDataSourceResult(request));

        }

        public async Task<object> ReadWBSGridDataTree(string id, string projectId, string emailTaskId)
        {
            var result = await _projectManagementBusiness.ReadProjectTaskForEmailList(projectId);
            // _pmtBusiness.GetWbsGridData(projectId.Value, wbsItemtemplateId.Value);
            //var filtered = new List<WBSViewModel>();            
            var filtered = result.Where(x => x.Id != emailTaskId);
            if (id.IsNotNullAndNotEmpty())
            {
                filtered = filtered.Where(x => x.ParentId == id);
            }
            else
            {
                filtered = filtered.Where(x => x.ParentId == null);
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(filtered);
            return json;

        }
        [HttpPost]
        public async Task<JsonResult> ManageEmailTask(string id,string taskId,string parentId,string type)
        {
           
            var model = await _taskBusiness.GetSingleById(id);
            if (model.IsNotNull())
            {
                if (type == "Task")
                {
                    model.ReferenceId = taskId;
                    model.ParentServiceId = parentId;
                    model.ReferenceType = ReferenceTypeEnum.NTS_Task;
                }
                else
                {                    
                    model.ReferenceId = taskId;
                    model.ParentTaskId = null;
                    model.ReferenceType = ReferenceTypeEnum.NTS_Service;
                }
                var result = await _taskBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });

                }
                else
                {
                    //ModelState.AddModelErrors(result.Messages);
                   return Json(new { success = false });
                }
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<JsonResult> ManageGeneralEmailTask(string id, string refId, ReferenceTypeEnum type)
        {

            var model = await _taskBusiness.GetSingleById(id);
            if (model.IsNotNull())
            {
                model.ReferenceId = refId;
                model.ReferenceType = type;

                var result = await _taskBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });

                }
                else
                {
                    //ModelState.AddModelErrors(result.Messages);
                    return Json(new { success = false });
                }
            }

            return Json(new { success = false });
        }

        public async Task<IActionResult> GetInboxTreeviewList(string id,string config,string projectid)
        {
            var vm = new TreeViewViewModel();
            if(config=="project")
            {

              //  if(projectid.IsNotNullAndNotEmpty())
               // {
                  var model = await _emailBusiness.GetInboxMenuItemProject(id, projectid);
                    return Json(model);
               // }
              //  return Json(vm);
                
            }
            else if (config == "company")
            {
                var model = await _emailBusiness.GetInboxMenuItemCompany(id);
                return Json(model);
            }
            else
            {
                var model = await _emailBusiness.GetInboxMenuItem(id);
                return Json(model);
            }
           
           
        }

    }
}