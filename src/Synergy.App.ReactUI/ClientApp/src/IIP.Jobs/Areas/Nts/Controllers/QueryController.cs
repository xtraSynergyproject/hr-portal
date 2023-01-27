using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Api.Areas.Nts.Controllers
{
    [Route("nts/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        [HttpGet]
        [Route("GetAllowedTempaltes")]
        public async Task<IActionResult> GetAllowedTempaltes(string categoryCode, string userId, TemplateTypeEnum? templateType = null, TaskTypeEnum? taskType = null,string portalCode=null)
        {
            try
            {
                var _pageBusiness = _serviceProvider.GetService<ITemplateBusiness>();
                var page = await _pageBusiness.GetAllowedTemplateList(categoryCode, userId, templateType, taskType, portalCode);
                return Ok(page);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetServiceDetails")]
        public async Task<IActionResult> GetServiceDetails(string templateCode, string userId, string serviceId, DataActionEnum dataAction = DataActionEnum.Read)
        {
            try
            {
                var _business = _serviceProvider.GetService<IServiceBusiness>();
                var result = await _business.GetServiceDetails(new ServiceTemplateViewModel
                {
                    TemplateCode = templateCode,
                    ActiveUserId = userId,
                    ServiceId = serviceId,
                    DataAction = dataAction,
                    SetUdfValue = true

                });
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }        
        [HttpGet]
        [Route("GetServiceDetailsByServiceNo")]
        public async Task<IActionResult> GetServiceDetailsByServiceNo(string email, string serviceNo, DataActionEnum dataAction = DataActionEnum.Read)
        {
            try
            {
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _business = _serviceProvider.GetService<IServiceBusiness>();
                if (email.IsNullOrEmpty() || serviceNo.IsNullOrEmpty())
                {
                    return Ok("please provide email & service no");
                }                
                var user = await _userBusiness.GetSingle(x => x.Email == email);
                if(user == null)
                {
                    return Ok("email does not exist");
                }
                var service = await _business.GetSingle(x => x.ServiceNo == serviceNo);
                if (service == null)
                {
                    return Ok("service no does not exist");
                }
                var result = await _business.GetServiceDetails(new ServiceTemplateViewModel
                {
                    TemplateCode = service.TemplateCode,
                    ActiveUserId = user.Id,
                    ServiceId = service.Id,
                    DataAction = dataAction,
                    SetUdfValue = true

                });
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("CreateService")]
        public async Task<IActionResult> CreateService(string email, string templateName, string portalName, string subject,string desciption,string prms/*, DataActionEnum dataAction = DataActionEnum.Create*/)
        {
            try
            {
                
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _business = _serviceProvider.GetService<IServiceBusiness>();
                var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
                var _portalBusiness = _serviceProvider.GetService<IPortalBusiness>();
                if (email.IsNullOrEmpty() || templateName.IsNullOrEmpty() || portalName.IsNullOrEmpty())
                {
                    return Ok("please provide Email , template Name & portal name");
                }
                var user = await _userBusiness.GetSingle(x => x.Email == email);
                if (user == null)
                {
                    return Ok("email does not exist");
                }
                var template = await _templateBusiness.GetSingle(x => x.DisplayName == templateName);
                if (template == null)
                {
                    return Ok("template name does not exist");
                }
                var portal = await _portalBusiness.GetSingle(x => x.Name == portalName);
                if (portal == null)
                {
                    return Ok("portal name does not exist");
                }
                var udfslist = new Dictionary<string, string>();
                if (prms.IsNotNullAndNotEmpty())
                {
                    
                    var spl = prms.Split(',');
                    for (int i = 0; i < spl.Count(); i++)
                    {
                        var tt = spl[i].Split('=');
                        udfslist.Add(tt[0], tt[1]);
                    }
                }
                var model = await _business.GetServiceDetails(new ServiceTemplateViewModel
                {
                    TemplateCode = template.Code,
                    ActiveUserId = user.Id,
                    //ServiceId = service.Id,
                    DataAction = DataActionEnum.Create,
                    SetUdfValue = true,
                    Udfs = udfslist

                }); 
                if(model != null)
                {
                    model.ServiceSubject = subject;
                    model.ServiceDescription = desciption;
                    model.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    model.DataAction = DataActionEnum.Create;
                    model.PortalId = portal.Id;
                    model.OwnerUserId = user.Id;
                    model.RequestedByUserId = user.Id;
                    model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(udfslist);
                }
                var result = await _business.ManageService(model);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetAllServiceStatusByTemplate")]
        public async Task<IActionResult> GetAllServiceStatusByTemplate(string email, string templateName/*,string portalName*/)
        {
            try
            {
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _business = _serviceProvider.GetService<IServiceBusiness>();
                var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
                var _portalBusiness = _serviceProvider.GetService<IPortalBusiness>();
                var _componentResultBusiness = _serviceProvider.GetService<IComponentResultBusiness>();
                if (email.IsNullOrEmpty() || templateName.IsNullOrEmpty() /*|| portalName.IsNullOrEmpty()*/)
                {
                    return Ok("please provide email and template name");
                }
                var user = await _userBusiness.GetSingle(x => x.Email == email);
                if (user == null)
                {
                    return Ok("email does not exist");
                }                
                var template = await _templateBusiness.GetSingle(x => x.DisplayName == templateName);
                if (template == null)
                {
                    return Ok("template name does not exist");
                }
                //var portal = await _portalBusiness.GetSingle(x => x.Name == portalName);
                //if (portal == null)
                //{
                //    return Ok("portal name does not exist");
                //}
                //var viewModel = await _business.GetServiceStatusByTemplateCode(template.Code, user.Id,portal.Id);
                //var newlist = new ProjectDashboardChartViewModel
                //{
                //    ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                //    ItemValueSeries = viewModel.Select(x => x.Value).ToList(),
                //    ItemStatusColor = viewModel.Select(x => x.StatusColor).ToList(),
                //    Code = viewModel.Select(x => x.Code).FirstOrDefault(),
                //};
                //return Ok(newlist);
                var viewModel = await _componentResultBusiness.GetNtsEmailList(null, null, user.Id, null, template.Code);

                long dcount = viewModel.Where(x => x.InboxStatus == EmailInboxTypeEnum.Drafted).Count();
                long pcount = viewModel.Where(x => x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                long ccount = viewModel.Where(x => x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                var newlist = new ProjectDashboardChartViewModel
                {
                    ItemValueLabel = new List<string> { "Draft", "Pending", "Completed" },
                    ItemValueSeries = new List<long> { dcount, pcount, ccount },
                    ItemStatusColor = new List<string> { "#17a2b8", "#007bff", "#13b713" },
                    Code = template.Code,
                };
                return Ok(newlist);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        [HttpGet]
        [Route("GetTaskDetails")]
        public async Task<IActionResult> GetTaskDetails(string templateCode, string userId, string taskId, DataActionEnum dataAction = DataActionEnum.Read)
        {
            try
            {
                var _business = _serviceProvider.GetService<ITaskBusiness>();
                var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
                var _taskTemplateBusiness = _serviceProvider.GetService<ITaskTemplateBusiness>();

                var taskTemplateModel = new TaskTemplateViewModel
                {
                    TemplateCode = templateCode,
                    ActiveUserId = userId,
                    TaskId = taskId,
                    DataAction = dataAction,
                    SetUdfValue = true
                };
                if (taskId.IsNotNullAndNotEmpty())
                {
                    var taskData = await _business.GetSingleById(taskId);
                    if (taskData != null)
                    {
                        var templateData = await _templateBusiness.GetSingleById(taskData.TemplateId);
                        if (templateData != null)
                        {
                            var taskTemplateData = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == templateData.Id);
                            if (taskTemplateData != null)
                            {
                                taskTemplateModel.TaskTemplateType = taskTemplateData.TaskTemplateType;
                            }
                        }
                    }
                }
                var result = await _business.GetTaskDetails(taskTemplateModel);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetNoteDetails")]
        public async Task<IActionResult> GetNoteDetails(string templateCode, string userId, string noteId, DataActionEnum dataAction = DataActionEnum.Read)
        {
            try
            {
                var _business = _serviceProvider.GetService<INoteBusiness>();
                var result = await _business.GetNoteDetails(new NoteTemplateViewModel
                {
                    TemplateCode = templateCode,
                    ActiveUserId = userId,
                    NoteId = noteId,
                    DataAction = dataAction,
                    SetUdfValue = true
                });
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }



        [HttpGet]
        [Route("ReadTaskCommentData")]
        public async Task<IActionResult> ReadTaskCommentData( string taskId)
        {
            var _business = _serviceProvider.GetService<INtsTaskCommentBusiness>();
            var model = await _business.GetSearchResult(taskId);

            return Ok(model);
        }

        [HttpGet]
        [Route("ReadServiceCommentData")]
        public async Task<IActionResult> ReadServiceCommentData( string serviceId)
        {
            var _business = _serviceProvider.GetService<INtsServiceCommentBusiness>();
            var model = await _business.GetSearchResult(serviceId);

            return Ok(model);
        }

        [HttpGet]
        [Route("ReadNoteCommentData")]
        public async Task<IActionResult> ReadNoteCommentData( string noteId)
        {
            var _business = _serviceProvider.GetService<INtsNoteCommentBusiness>();
            var model = await _business.GetSearchResult(noteId);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetServiceCommentCount")]
        public async Task<IActionResult> GetServiceCommentCount(string serviceId)
        {
            var _business = _serviceProvider.GetService<INtsServiceCommentBusiness>();
            var list = await _business.GetSearchResult(serviceId);
            return Ok(list.Count());
        }

        [HttpGet]
        [Route("GetNoteCommentCount")]
        public async Task<IActionResult> GetNoteCommentCount(string noteId)
        {
            var _business = _serviceProvider.GetService<INtsNoteCommentBusiness>();
            var list = await _business.GetSearchResult(noteId);

            return Ok(list.Count());
        }

        [HttpGet]
        [Route("GetTaskCommentCount")]
        public async Task<IActionResult> GetTaskCommentCount(string taskId)
        {
            var _business = _serviceProvider.GetService<INtsTaskCommentBusiness>();
            var list = await _business.GetSearchResult(taskId);

            return Ok(list.Count());
        }

        [HttpGet]
        [Route("GetServiceChartByStatus")]
        public async Task<ActionResult> GetServiceChartByStatus(string userId)
        {
            var _business = _serviceProvider.GetService<IServiceBusiness>();
            //var _context = _serviceProvider.GetService<IUserContext>();
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = userId;
            var viewModel = await _business.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.ServiceStatusCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.ServiceStatusName).FirstOrDefault(), Id = group.Select(x => x.ServiceStatusId).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("GetServiceChartByUserType")]
        public async Task<ActionResult> GetServiceChartByUserType(string userId)
        {
            var _business = _serviceProvider.GetService<IServiceBusiness>();
            //var _context = _serviceProvider.GetService<IUserContext>();
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = userId;
            var viewModel = await _business.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.TemplateUserType).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TemplateUserType).FirstOrDefault(), Id = group.Select(x => Convert.ToInt32(x.TemplateUserType)).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadServiceDashBoardGridData")]
        public async Task<IActionResult> ReadServiceDashBoardGridData(string userId,string text,string serviceStatusIds,string userType, string searchText)//, string templateCategoryCode, string filterUserId, string moduleId, string mode, string serviceNo, string serviceStatus, string subject, DateTime? startDate, DateTime? dueDate, DateTime? completionDate, string templateMasterCode)
        {
            await Authenticate(userId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _business = _serviceProvider.GetService<IServiceBusiness>();
            //var _context = _serviceProvider.GetService<IUserContext>();
            //if (userId.IsNullOrEmpty())
            //{
            //    userId = _context.UserId;
            //}
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = userId;
            var result = await _business.GetSearchResult(search);
            if (text == "Today")
            {
                var res = result.Where(x => x.DueDate <= DateTime.Now && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT");
                return Ok(res);
            }
            else if (text == "Week")
            {
                var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT")).ToList();
                return Ok(res);
            }
            else if (serviceStatusIds.IsNotNull() && serviceStatusIds.Count() > 0)
            {
                var status = string.Join(",", serviceStatusIds);
                var res = result.Where(x => status.Contains(x.ServiceStatusId)).ToList();
                return Ok(res);
            }
            else if (userType.IsNotNull() && userType.Count() > 0)
            {
                var UserType = string.Join(",", userType);
                var res = result.Where(x => UserType.Contains(x.TemplateUserType.ToString())).ToList();
                return Ok(res);
            }
            if (searchText.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => searchText.Contains(x.ServiceNo) || searchText.Contains(x.ServiceSubject ?? "") || searchText.Contains(x.TemplateUserType.ToString())).ToList();// || searchText.Contains(x.OwnerUserUserName)).ToList();
                //result = result.Where(x => searchText.Contains(x.TaskSubject)).ToList();
                //result = result.Where(x => searchText.Contains(x.AssigneeDisplayName)).ToList();
                //result = result.Where(x => searchText.Contains(x.OwnerUserName)).ToList();
                //result = result.Where(x => searchText.Contains(x.TemplateUserType.ToString())).ToList();
            }
            if (result.Count() > 1000)
            {
                return Ok(result.Take(1000));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("ReadDatewiseServiceSLA")]
        public async Task<ActionResult> ReadDatewiseServiceSLA(string userId, DateTime? startDate, DateTime? dueDate)
        {
            var _business = _serviceProvider.GetService<IServiceBusiness>();
            var _context = _serviceProvider.GetService<IUserContext>();
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = userId;
            search.StartDate = startDate;
            search.DueDate = dueDate;
            if (search.StartDate != null && search.DueDate != null)
            {
                var viewModel = await _business.GetDatewiseServiceSLA(search);
                return Ok(viewModel);
            }
            else { return Ok(""); }
        }

        [HttpGet]
        [Route("GetTaskChartByStatus")]
        public async Task<ActionResult> GetTaskChartByStatus(string userId)
        {
            var _business = _serviceProvider.GetService<ITaskBusiness>();
            //var _context = _serviceProvider.GetService<IUserContext>();
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = userId;
            var viewModel = await _business.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.TaskStatusCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TaskStatusName).FirstOrDefault(), Id = group.Select(x => x.TaskStatusId).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("GetTaskChartByUserType")]
        public async Task<ActionResult> GetTaskChartByUserType(string userId)
        {
            var _business = _serviceProvider.GetService<ITaskBusiness>();
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = userId;
            var viewModel = await _business.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.TemplateUserType).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TemplateUserType).FirstOrDefault(), Id = group.Select(x => Convert.ToInt32(x.TemplateUserType)).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadTaskDashBoardGridData")]
        public async Task<IActionResult> ReadTaskDashBoardGridData(string userId,string TaskStatusIds,string TaskAssigneeIds, string TaskOwnerIds ,string searchText)
        {
            var _business = _serviceProvider.GetService<ITaskBusiness>();
            
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = userId;
            if (TaskStatusIds.IsNotNullAndNotEmpty())
            {
                search.TaskStatusIds = new List<string>() { TaskStatusIds };
            }
            if (TaskAssigneeIds.IsNotNullAndNotEmpty())
            {
                search.TaskAssigneeIds = new List<string>() { TaskAssigneeIds };
            }
            if (TaskOwnerIds.IsNotNullAndNotEmpty())
            {
                search.TaskOwnerIds = new List<string>() { TaskOwnerIds };
            }
            var result = await _business.GetSearchResult(search);
            if (search.IsNotNull() && search.TaskOwnerIds.IsNotNull())
            {
                var status = string.Join(",", search.TaskOwnerIds);
                result = result.Where(x => status.Contains(x.OwnerUserId)).ToList();
            }
            if (search.IsNotNull() && search.TaskAssigneeIds.IsNotNull())
            {
                var status = string.Join(",", search.TaskAssigneeIds);
                result = result.Where(x => status.Contains(x.AssignedToUserId)).ToList();
            }
            if (search.IsNotNull() && search.TaskStatusIds.IsNotNull())
            {
                var status = string.Join(",", search.TaskStatusIds);
                result = result.Where(x => status.Contains(x.TaskStatusId)).ToList();
            }
            if (searchText.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => searchText.Contains(x.TaskNo)|| searchText.Contains(x.TaskSubject??"") || searchText.Contains(x.AssigneeDisplayName) || searchText.Contains(x.OwnerUserName) || searchText.Contains(x.TemplateUserType.ToString())).ToList();
                //result = result.Where(x => searchText.Contains(x.TaskSubject)).ToList();
                //result = result.Where(x => searchText.Contains(x.AssigneeDisplayName)).ToList();
                //result = result.Where(x => searchText.Contains(x.OwnerUserName)).ToList();
                //result = result.Where(x => searchText.Contains(x.TemplateUserType.ToString())).ToList();
            }

            if (result.Count() > 1000)
            {
                return Ok(result.Take(1000));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("ReadDatewiseTaskSLA")]
        public async Task<ActionResult> ReadDatewiseTaskSLA(string userId, DateTime? startDate, DateTime? dueDate)
        {
            var _business = _serviceProvider.GetService<ITaskBusiness>();
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = userId;
            search.StartDate = startDate;
            search.DueDate = dueDate;
            if (search.StartDate != null && search.DueDate != null)
            {
                var viewModel = await _business.GetDatewiseTaskSLA(search);
                return Ok(viewModel);
            }
            else { return Ok(""); }
        }

        [HttpGet]
        [Route("GetNoteChartByStatus")]
        public async Task<ActionResult> GetNoteChartByStatus(string userId)
        {
            var _business = _serviceProvider.GetService<INoteBusiness>();
            NoteSearchViewModel search = new NoteSearchViewModel();
            search.UserId = userId;
            var viewModel = await _business.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.NoteStatusCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.NoteStatusName).FirstOrDefault(), Id = group.Select(x => x.NoteStatusId).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            //var list = new List<ProjectDashboardChartViewModel>();

            return Ok(list);
        }

        [HttpGet]
        [Route("GetNoteChartByUserType")]
        public async Task<ActionResult> GetNoteChartByUserType(string userId)
        {
            var _business = _serviceProvider.GetService<INoteBusiness>();
            //NoteSearchViewModel search = new NoteSearchViewModel();
            //search.UserId = _userContext.UserId;
            //var viewModel = await _noteBusiness.GetSearchResult(search);
            //var list1 = viewModel.GroupBy(x => x.TemplateUserType).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TemplateUserType).FirstOrDefault(), Id = group.Select(x => Convert.ToInt32(x.TemplateUserType)).FirstOrDefault() }).ToList();
            //var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            var list = new List<ProjectDashboardChartViewModel>();

            return Ok(list);
        }

        [HttpGet]
        [Route("ReadNoteDashBoardGridData")]
        public async Task<IActionResult> ReadNoteDashBoardGridData(string userId)
        {
            var _business = _serviceProvider.GetService<INoteBusiness>();
            NoteSearchViewModel search = new NoteSearchViewModel();
            search.UserId = userId;
            
            var result = await _business.GetSearchResult(search);
            //if (search?.Text == "Today")
            //{
            //    var res = result.Where(x => x.DueDate <= DateTime.Now && x.NoteStatusCode != "NOTE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT");
            //    return Json(res.ToDataSourceResult(request));
            //}
            //else if (search?.Text == "Week")
            //{
            //    var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT")).ToList();
            //    return Json(res.ToDataSourceResult(request));
            //}
            //else if (search.ServiceStatusIds.IsNotNull() && search.ServiceStatusIds.Count() > 0)
            //{
            //    var status = string.Join(",", search.ServiceStatusIds);
            //    var res = result.Where(x => status.Contains(x.ServiceStatusId)).ToList();
            //    return Json(res.ToDataSourceResult(request));
            //}
            //else if (search.UserType.IsNotNull() && search.UserType.Count() > 0)
            //{
            //    var UserType = string.Join(",", search.UserType);
            //    var res = result.Where(x => UserType.Contains(x.TemplateUserType.ToString())).ToList();
            //    return Json(res.ToDataSourceResult(request));
            //}
            return Ok(result);
        }
        [HttpGet]
        [Route("GetTaskTemplateTreeList")]
        public async Task<ActionResult> GetTaskTemplateTreeList(string id, string parentId)
        {
            var _categoryBusiness = _serviceProvider.GetService<ITemplateCategoryBusiness>();
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            //IList<TreeViewViewModel> list= new List<TreeViewViewModel>(); ;
            var list = new List<TreeViewViewModel>();
            var hierarchyList = new List<TreeViewModelHierarchy>();
            if (id.IsNullOrEmpty())
            {

                //list.Add(new TreeViewViewModel
                //{
                //    id = TemplateTypeEnum.Task.ToString(),
                //    Name = TemplateTypeEnum.Task.ToString(),
                //    DisplayName = TemplateTypeEnum.Task.ToString(),
                //    ParentId = null,
                //    hasChildren = true,
                //    expanded = true,
                //    Type = "Root"

                //});
                hierarchyList.Add(new TreeViewModelHierarchy
                {
                    id = TemplateTypeEnum.Task.ToString(),
                    Name = TemplateTypeEnum.Task.ToString(),
                    DisplayName = TemplateTypeEnum.Task.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"
                });

            }
            if (id == TemplateTypeEnum.Note.ToString()
                || id == TemplateTypeEnum.Task.ToString() || id == TemplateTypeEnum.Service.ToString()
                || id == TemplateTypeEnum.Form.ToString() || id == TemplateTypeEnum.Page.ToString()
                || id == TemplateTypeEnum.FormIndexPage.ToString() || id == TemplateTypeEnum.Custom.ToString() || id == TemplateTypeEnum.ProcessDesign.ToString())
            {
                TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
                var category = await _categoryBusiness.GetList(x => x.TemplateType == type);
                // category = category.Where().ToList();
                //list.AddRange(category.Select(x => new TreeViewViewModel
                //{
                //    id = x.Id.ToString(),
                //    Name = x.Name,
                //    DisplayName = x.Name,
                //    ParentId = id,
                //    hasChildren = true,
                //    expanded = false,
                //    Type = "Category"

                //}));
                hierarchyList.AddRange(category.Select(x => new TreeViewModelHierarchy
                {
                    id = x.Id.ToString(),
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "Category"
                }));
                for (int i = 0; i <= hierarchyList.Count - 1; i++)
                {
                    var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == id);
                    hierarchyList[i].treeViewModelChildren = new List<TreeViewModelHierarchy>();
                    hierarchyList[i].treeViewModelChildren.AddRange(templates.Select(x => new TreeViewModelHierarchy
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName.Coalesce(x.Name),
                        ParentId = id,
                        hasChildren = false,
                        expanded = false,
                        Type = "Template",
                        TemplateType = x.TemplateType
                    }));
                }
            }
            //else
            //{
            //    var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == id);
            //    list.AddRange(templates.Select(x => new TreeViewViewModel
            //    {
            //        id = x.Id,
            //        Name = x.Name,
            //        DisplayName = x.DisplayName.Coalesce(x.Name),
            //        ParentId = id,
            //        hasChildren = false,
            //        expanded = false,
            //        Type = "Template",
            //        TemplateType = x.TemplateType
            //    }));
            //}



            return Ok(hierarchyList.ToList());
        }
        [HttpGet]
        [Route("GetNoteTemplateTreeList")]
        public async Task<ActionResult> GetNoteTemplateTreeList(string id, string parentId)
        {
            var _categoryBusiness = _serviceProvider.GetService<ITemplateCategoryBusiness>();
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var list = new List<TreeViewViewModel>();
            var hierarchyList = new List<TreeViewModelHierarchy>();
            if (id.IsNullOrEmpty())
            {

                //list.Add(new TreeViewViewModel
                //{
                //    id = TemplateTypeEnum.Note.ToString(),
                //    Name = TemplateTypeEnum.Note.ToString(),
                //    DisplayName = TemplateTypeEnum.Note.ToString(),
                //    ParentId = null,
                //    hasChildren = true,
                //    expanded = true,
                //    Type = "Root"

                //});
                hierarchyList.Add(new TreeViewModelHierarchy
                {
                    id = TemplateTypeEnum.Note.ToString(),
                    Name = TemplateTypeEnum.Note.ToString(),
                    DisplayName = TemplateTypeEnum.Note.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"
                });

            }
            if (id == TemplateTypeEnum.Note.ToString()
                || id == TemplateTypeEnum.Task.ToString() || id == TemplateTypeEnum.Service.ToString()
                || id == TemplateTypeEnum.Form.ToString() || id == TemplateTypeEnum.Page.ToString()
                || id == TemplateTypeEnum.FormIndexPage.ToString() || id == TemplateTypeEnum.Custom.ToString() || id == TemplateTypeEnum.ProcessDesign.ToString())
            {
                TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
                var category = await _categoryBusiness.GetList(x => x.TemplateType == type);
                // category = category.Where().ToList();
                //list.AddRange(category.Select(x => new TreeViewViewModel
                //{
                //    id = x.Id.ToString(),
                //    Name = x.Name,
                //    DisplayName = x.Name,
                //    ParentId = id,
                //    hasChildren = true,
                //    expanded = false,
                //    Type = "Category"

                //}));
                hierarchyList.AddRange(category.Select(x => new TreeViewModelHierarchy
                {
                    id = x.Id.ToString(),
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "Category"
                }));
                for (int i = 0; i <= hierarchyList.Count - 1; i++)
                {
                    var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == hierarchyList[i].id);
                    hierarchyList[i].treeViewModelChildren = new List<TreeViewModelHierarchy>();
                    hierarchyList[i].treeViewModelChildren.AddRange(templates.Select(x => new TreeViewModelHierarchy
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName.Coalesce(x.Name),
                        ParentId = id,
                        hasChildren = false,
                        expanded = false,
                        Type = "Template",
                        TemplateType = x.TemplateType
                    }));
                }
            }
            //else
            //{
            //    var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == id);
            //    list.AddRange(templates.Select(x => new TreeViewViewModel
            //    {
            //        id = x.Id,
            //        Name = x.Name,
            //        DisplayName = x.DisplayName.Coalesce(x.Name),
            //        ParentId = id,
            //        hasChildren = false,
            //        expanded = false,
            //        Type = "Template",
            //        TemplateType = x.TemplateType
            //    }));
            //}



            return Ok(hierarchyList.ToList());
        }

        [HttpGet]
        [Route("GetServiceTemplateTreeList")]
        public async Task<ActionResult> GetServiceTemplateTreeList(string id, string parentId)
        {
            var _categoryBusiness = _serviceProvider.GetService<ITemplateCategoryBusiness>();
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var list = new List<TreeViewViewModel>();
            var hierarchyList = new List<TreeViewModelHierarchy>();
            if (id.IsNullOrEmpty())
            {

                //list.Add(new TreeViewViewModel
                //{
                //    id = TemplateTypeEnum.Service.ToString(),
                //    Name = TemplateTypeEnum.Service.ToString(),
                //    DisplayName = TemplateTypeEnum.Service.ToString(),
                //    ParentId = null,
                //    hasChildren = true,
                //    expanded = true,
                //    Type = "Root"
                //});
                hierarchyList.Add( new TreeViewModelHierarchy {
                    id = TemplateTypeEnum.Service.ToString(),
                    Name = TemplateTypeEnum.Service.ToString(),
                    DisplayName = TemplateTypeEnum.Service.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"
                });

            }
            if (id == TemplateTypeEnum.Note.ToString()
                || id == TemplateTypeEnum.Task.ToString() || id == TemplateTypeEnum.Service.ToString()
                || id == TemplateTypeEnum.Form.ToString() || id == TemplateTypeEnum.Page.ToString()
                || id == TemplateTypeEnum.FormIndexPage.ToString() || id == TemplateTypeEnum.Custom.ToString() || id == TemplateTypeEnum.ProcessDesign.ToString())
            {
                TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
                var category = await _categoryBusiness.GetList(x => x.TemplateType == type);
                // category = category.Where().ToList();
                //list.AddRange(category.Select(x => new TreeViewViewModel
                //{
                //    id = x.Id.ToString(),
                //    Name = x.Name,
                //    DisplayName = x.Name,
                //    ParentId = id,
                //    hasChildren = true,
                //    expanded = false,
                //    Type = "Category"

                //}));
                hierarchyList.AddRange(category.Select(x => new TreeViewModelHierarchy
                {
                    id = x.Id.ToString(),
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "Category"
                }));
                for (int i = 0; i <= hierarchyList.Count-1; i++)
                {
                    var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == hierarchyList[i].id);
                    hierarchyList[i].treeViewModelChildren = new List<TreeViewModelHierarchy>();
                    hierarchyList[i].treeViewModelChildren.AddRange(templates.Select(x => new TreeViewModelHierarchy
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName.Coalesce(x.Name),
                        ParentId = id,
                        hasChildren = false,
                        expanded = false,
                        Type = "Template",
                        TemplateType = x.TemplateType
                    }));
                }
            }
            //else
            //{
            //    var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == id);
            //    list.AddRange(templates.Select(x => new TreeViewViewModel
            //    {
            //        id = x.Id,
            //        Name = x.Name,
            //        DisplayName = x.DisplayName.Coalesce(x.Name),
            //        ParentId = id,
            //        hasChildren = false,
            //        expanded = false,
            //        Type = "Template",
            //        TemplateType = x.TemplateType
            //    }));
            //    hierarchyList.AddRange(templates.Select(x => new TreeViewModelHierarchy
            //    {
            //        id = TemplateTypeEnum.Service.ToString(),
            //        Name = TemplateTypeEnum.Service.ToString(),
            //        DisplayName = TemplateTypeEnum.Service.ToString(),
            //        ParentId = null,
            //        hasChildren = true,
            //        expanded = true,
            //        Type = "Root"
            //    }));
            //}



            return Ok(hierarchyList.ToList());
        }

        [HttpGet]
        [Route("WorklistDashboardCount")]
        public async Task<ActionResult> WorklistDashboardCount(string moduleCodes, string userId, string taskTemplateIds, string serviceTemplateIds)
        {
            await Authenticate(userId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _business = _serviceProvider.GetService<IServiceBusiness>();
            if (userId.IsNullOrEmpty())
            {
                userId = _context.UserId;
            }
            var count = await _business.GetWorklistDashboardCount(userId, moduleCodes, null, taskTemplateIds, serviceTemplateIds);
            // var j = Json(count);
            return Ok(count);
        }

        [HttpGet]
        [Route("WorklistDashboardNotesCount")]
        public async Task<ActionResult> WorklistDashboardNotesCount(string moduleCodes, string userId, string noteTemplateIds)
        {
            await Authenticate(userId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _business = _serviceProvider.GetService<INoteBusiness>();
            if (userId.IsNullOrEmpty())
            {
                userId = _context.UserId;
            }
            var count = await _business.NotesDashboardCount(userId, null, moduleCodes, noteTemplateIds);
            return Ok(count);
        }

        //[HttpGet]
        //[Route("ReadTaskDataInProgress")]
        //public async Task<IActionResult> ReadTaskDataInProgress(string userId, string moduleCodes)
        //{
        //    var _business = _serviceProvider.GetService<IRecTaskBusiness>();
        //    var result = await _business.GetActiveListByUserId(userId);
        //    var j = Ok(result.Where(x => x.TaskStatusCode == "INPROGRESS" && x.DueDate >= DateTime.Now).OrderByDescending(x => x.StartDate));
        //    return j;
        //}

        //[HttpGet]
        //[Route("ReadTaskDataOverdue")]
        //public async Task<IActionResult> ReadTaskDataOverdue(string userId)
        //{
        //    var _business = _serviceProvider.GetService<IRecTaskBusiness>();
        //    var result = await _business.GetActiveListByUserId(userId);
        //    var j = Ok(result.Where(x => x.TaskStatusCode == "OVERDUE").OrderByDescending(x => x.StartDate));
        //    return j;
        //}

        //[HttpGet]
        //[Route("ReadTaskDataCompleted")]
        //public async Task<IActionResult> ReadTaskDataCompleted(string userId)
        //{
        //    var _business = _serviceProvider.GetService<IRecTaskBusiness>();
        //    var result = await _business.GetActiveListByUserId(userId);
        //    var j = Ok(result.Where(x => x.TaskStatusCode == "COMPLETED").OrderByDescending(x => x.StartDate));
        //    return j;
        //}

        [HttpGet]
        [Route("ReadTaskDataInProgress")]
        public async Task<ActionResult> ReadTaskDataInProgress(string userId, string moduleCodes, string portalName, string templateCodes, string categoryCodes,string requestby)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
            var result = await _taskBusiness.GetTaskList(_context.PortalId, moduleCodes, templateCodes, categoryCodes);
            if (requestby.IsNotNullAndNotEmpty())
            {
                if (requestby == "RequestedByMe")
                {
                    result = result.Where(x => x.RequestedByUserId == _context.UserId).ToList();
                }
                else if (requestby == "AssignedToMe")
                {
                    result = result.Where(x => x.AssignedToUserId == _context.UserId).ToList();
                }
            }
            if (templateCodes == "DMS_SUPPORT_TICKET")
            {
                var j = Ok(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_DRAFT").OrderByDescending(x => x.StartDate));
                return j;
            }
            else
            {
                var j = Ok(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").OrderByDescending(x => x.StartDate));
                return j;
            }
        }

        [HttpGet]
        [Route("ReadTaskDataOverdue")]
        public async Task<ActionResult> ReadTaskDataOverdue(string userId, string moduleCodes, string portalName, string templateCodes, string categoryCodes,string requestby)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
            var result = await _taskBusiness.GetTaskList(_context.PortalId, moduleCodes, templateCodes, categoryCodes);
            {
                if (requestby == "RequestedByMe")
                {
                    result = result.Where(x => x.RequestedByUserId == _context.UserId).ToList();
                }
                else if (requestby == "AssignedToMe")
                {
                    result = result.Where(x => x.AssignedToUserId == _context.UserId).ToList();
                }
            }
            var j = Ok(result.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").OrderByDescending(x => x.StartDate));
            return j;
        }

        [HttpGet]
        [Route("ReadTaskDataCompleted")]
        public async Task<ActionResult> ReadTaskDataCompleted(string userId, string moduleCodes, string portalName, string templateCodes, string categoryCodes, string requestby)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
            var result = await _taskBusiness.GetTaskList(_context.PortalId, moduleCodes, templateCodes, categoryCodes);
            if (requestby.IsNotNullAndNotEmpty())
            {
                if (requestby == "RequestedByMe")
                {
                    result = result.Where(x => x.RequestedByUserId == _context.UserId).ToList();
                }
                else if (requestby == "AssignedToMe")
                {
                    result = result.Where(x => x.AssignedToUserId == _context.UserId).ToList();
                }
            }
            var j = Ok(result.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").OrderByDescending(x => x.StartDate));
            return j;
        }
        [HttpGet]
        [Route("ReadMonitoringDashboarGridData")]
        public async Task<IActionResult> ReadMonitoringDashboarGridData(string email, string templateCodes,string pageType, string portalName=null)
        {
            try
            {
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _business = _serviceProvider.GetService<IServiceBusiness>();
                var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
                var _portalBusiness = _serviceProvider.GetService<IPortalBusiness>();
                var _componentResultBusiness = _serviceProvider.GetService<IComponentResultBusiness>();
                if (email.IsNullOrEmpty() || templateCodes.IsNullOrEmpty()  || pageType.IsNullOrEmpty())
                {
                    return Ok("please provide email , template Code and pageType");
                }
                var user = await _userBusiness.GetSingle(x => x.Email == email);
                if (user == null)
                {
                    return Ok("email does not exist");
                }
                //var template = await _templateBusiness.GetSingle(x => x.DisplayName == templateName);
                //if (template == null)
                //{
                //    return Ok("template name does not exist");
                //}
                //var portal = await _portalBusiness.GetSingle(x => x.Name == portalName);
                //if (portal == null)
                //{
                //    return Ok("portal name does not exist");
                //}
                var emailList = await _componentResultBusiness.GetNtsEmailList(null, null, user.Id, portalName, templateCodes, null, null, null, null, null, null, null, null, null);

                var templates = emailList.GroupBy(x => x.TemplateId).Select(x => x.Key).ToArray();

                var emailgridlist = new List<NtsEmailViewModel>();

                if (pageType == "Monitoring")
                {
                    foreach (var temp in templates)
                    {
                        var pcount = emailList.Where(x => x.TemplateId == temp && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                        var ccount = emailList.Where(x => x.TemplateId == temp && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                        var data = new NtsEmailViewModel()
                        {
                            TemplateId = temp,
                            TemplateName = emailList.Where(x => x.TemplateId == temp).Select(x => x.TemplateName).FirstOrDefault(),
                            PendingCount = pcount,
                            CompletedCount = ccount
                        };
                        emailgridlist.Add(data);
                    }
                }
                else if (pageType == "Stage")
                {
                    foreach (var temp in templates)
                    {
                        var pcount = emailList.Where(x => x.TemplateId == temp && (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                        var ccount = emailList.Where(x => x.TemplateId == temp && (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                        var data = new NtsEmailViewModel()
                        {
                            TemplateId = temp,
                            TemplateName = emailList.Where(x => x.TemplateId == temp).Select(x => x.TemplateName).FirstOrDefault(),
                            PendingCount = pcount,
                            CompletedCount = ccount
                        };
                        emailgridlist.Add(data);
                    }
                }
                else if (pageType == "Communication")
                {
                    foreach (var temp in templates)
                    {
                        var pcount = emailList.Where(x => x.TemplateId == temp && (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                        var ccount = emailList.Where(x => x.TemplateId == temp && (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                        var data = new NtsEmailViewModel()
                        {
                            TemplateId = temp,
                            TemplateName = emailList.Where(x => x.TemplateId == temp).Select(x => x.TemplateName).FirstOrDefault(),
                            PendingCount = pcount,
                            CompletedCount = ccount
                        };
                        emailgridlist.Add(data);
                    }
                }
                return Ok(emailgridlist);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}

public class TreeViewModelHierarchy {

    public string id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string ParentId { get; set; }
    public bool hasChildren { get; set; }
    public bool expanded { get; set; }
    public string Type { get; set; }
    public string PortalId { get; set; }
    public long? RootId { get; set; }
    public int ItemLevel { get; set; }
    public bool Checked { get; set; }
    public string Url { get; set; }
    public string UserRoleId { get; set; }
    public string[] StatusCode { get; set; }
    public string StatusCodeStr
    {
        get
        {
            var str = "";
            if (StatusCode != null)
            {
                str = string.Join(",", StatusCode);
            }
            return str;
        }
    }

    public TemplateTypeEnum? TemplateType { get; set; }
    public string TemplateTypeText
    {
        get
        {
            return Convert.ToString(TemplateType);
        }
    }
    public DataColumnTypeEnum? FieldDataType { get; set; }

    public List<TreeViewModelHierarchy> treeViewModelChildren { get; set; }
}
