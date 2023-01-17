using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.Extensions.Configuration;
using Synergy.App.Api.Areas.EGov.Models;

namespace Synergy.App.Api.Areas.EGov.Controllers
{
    [Route("egov/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IConfiguration _iConfiguration;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IEGovernanceBusiness _eGovernanceBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ITemplateCategoryBusiness _categoryBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly ILOVBusiness _LOVBusiness;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider, ICmsBusiness cmsBusiness, ITaskBusiness taskBusiness, IEGovernanceBusiness eGovernanceBusiness, ITemplateBusiness templateBusiness,
        ITemplateCategoryBusiness categoryBusiness, IServiceBusiness serviceBusiness, IPortalBusiness portalBusiness, ILOVBusiness LOVBusiness, IConfiguration iConfiguration) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _iConfiguration = iConfiguration;
            _cmsBusiness = cmsBusiness;
            _taskBusiness = taskBusiness;
            _eGovernanceBusiness = eGovernanceBusiness;
            _categoryBusiness = categoryBusiness;
            _templateBusiness = templateBusiness;
            _serviceBusiness = serviceBusiness;
            _portalBusiness = portalBusiness;
            _LOVBusiness = LOVBusiness;
        }
        [HttpGet]
        [Route("GetMapMarkerDetails")]
        public async Task<ActionResult> GetMapMarkerDetails()
        {
            var _egovBusiness = _serviceProvider.GetService<IEGovernanceBusiness>();
            var result = await _egovBusiness.GetMapMarkerDeatils();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetGISLocationDetails")]
        public async Task<ActionResult> GetGISLocationDetails()
        {
            var _egovBusiness = _serviceProvider.GetService<IEGovernanceBusiness>();
            var result = await _egovBusiness.GetGISLocationDetails();
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadCommunityHallList")]
        public async Task<IActionResult> ReadCommunityHallList(string wardId)
        {
            var where = "";
            if (wardId.IsNotNullAndNotEmpty())

            {
                var wardIds = string.Join("','", wardId);
                where = $@" and ""N_EGOV_MASTER_DATA_CommunityHallName"".""WardIds"" like ('%{wardId}%') ";
                var data = await _cmsBusiness.GetDataListByTemplate("EGOV_COMMUNITY_HALL_NAME", "", where);
                return Ok(data);
            }
            else
            {
                var data = await _cmsBusiness.GetDataListByTemplate("EGOV_COMMUNITY_HALL_NAME", "", "");
                return Ok(data);
            }

        }
        [HttpGet]
        [Route("ReadTaskListCount")]
        public async Task<ActionResult> ReadTaskListCount(string categoryCodes, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var result = await _taskBusiness.GetTaskCountByServiceTemplateCodes(categoryCodes, _context.PortalId);
            var j = Ok(result);
            return j;
        }

        [HttpGet]
        [Route("ReadTaskData")]
        public async Task<ActionResult> ReadTaskData(int pageNumber, int pageSize, string categoryCodes, string taskStatus, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            //var source = await _taskBusiness.GetTaskListByServiceCategoryCodes(categoryCodes, taskStatus,_context.PortalId);
            var source = new List<TaskViewModel>();// await _taskBusiness.GetTaskListByServiceCategoryCodes(categoryCodes, taskStatus,_context.PortalId);
            // Get's No of Rows Count   
            int count = source.Count;

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage,
                items
            };

            // Setting Header  
            //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            // Returing List of Customers Collections  
            return Ok(paginationMetadata);
            //var paginatedItem = GenericPagingFn(source, pageNumber, pageSize);
            //return Ok(paginatedItem);
        }


        [HttpGet]
        [Route("GetMyRequestList")]
        public async Task<ActionResult> GetMyRequestList(int pageNumber, int pageSize, string userId, string portalName, bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string search = null, DateTime? From = null, DateTime? To = null, string statusIds = null, string templateIds = null)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var source = await _eGovernanceBusiness.GetMyRequestList(showAllOwnersService, moduleCodes, templateCodes, categoryCodes, search, From, To, statusIds, templateIds);
            // Get's No of Rows Count   
            int count = source.Count;

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage,
                items
            };

            // Setting Header  
            //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            // Returing List of Customers Collections  
            return Ok(paginationMetadata);
            //var paginatedItem=GenericPagingFn(source, pageNumber, pageSize);
            //return Ok(paginatedItem);
            //return Ok(dt.Take(200));
        }

        [HttpGet]
        [Route("ReadTemplateListByCategoryCodes")]
        public async Task<ActionResult> ReadTemplateListByCategoryCodes(string categoryCodes)
        {
            var codes = categoryCodes.Split(",").ToArray();
            var catList = await _categoryBusiness.GetList(x => codes.Contains(x.Code));
            var idList = catList.Select(x => x.Id).ToArray();

            var model = await _templateBusiness.GetList(x => idList.Contains(x.TemplateCategoryId) && x.TemplateType == TemplateTypeEnum.Service);
            return Ok(model);
        }

        [HttpGet]
        [Route("MyRequest")]
        public async Task<ActionResult> MyRequest(string userId, string portalName, string moduleCodes, string templateCodes, string categoryCodes, string requestby, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var result = await _serviceBusiness.GetServiceList(_context.PortalId, null, null, categoryCodes, requestby, false);

            List<customIndexPageTemplateViewModel> list = new List<customIndexPageTemplateViewModel>();

            foreach (var item in result.GroupBy(x => x.TemplateCode))
            {
                list.Add(new customIndexPageTemplateViewModel
                {
                    ServiceName = item.Select(x => x.TemplateDisplayName).FirstOrDefault(),
                    TemplateCode = item.Select(x => x.TemplateCode).FirstOrDefault(),
                    InProgressCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE"),
                    CompletedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE"),
                    RejectedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
                });
            }



            var model = new CustomIndexPageTemplateViewModel()
            {
                ServiceList = list,
                TemplateCodes = templateCodes,
                ModuleCodes = moduleCodes,
                CategoryCodes = categoryCodes,
                IsDisableCreate = isDisableCreate,
                ShowAllOwnersService = showAllOwnersService,
                PortalId = _context.PortalId,
            };

            return Ok(model);
        }

        [HttpGet]
        [Route("ReadServiceList")]
        public async Task<ActionResult> ReadServiceList(string userId, string portalName, string statusCodes, string templateCode)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var dt = await _eGovernanceBusiness.GetServiceList(statusCodes, templateCode);
            return Ok(dt);
        }


        [HttpGet]
        [Route("GetExistingDetails")]
        public async Task<ActionResult> GetExistingDetails(string consumerNo, string type)
        {
            if (type == "BinBooking")
            {
                var model = await _eGovernanceBusiness.GetExistingBinBookingDetails(consumerNo);
                if (model != null)
                {
                    return Ok(new { success = true, data = model });
                }
            }
            else if (type == "Sewerage")
            {
                var model = await _eGovernanceBusiness.GetExistingSewerageDetails(consumerNo);
                if (model != null)
                {
                    return Ok(new { success = true, data = model });
                }
            }

            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("ReadTaskDataInProgress")]
        public async Task<ActionResult> ReadTaskDataInProgress(string userId, string portalNames = null)
        {
            await Authenticate(userId, portalNames);
            var _context = _serviceProvider.GetService<IUserContext>();
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _context.PortalId;
            }
            var result = await _eGovernanceBusiness.GetTaskList(ids);
            var j = Ok(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").OrderByDescending(x => x.StartDate));
            return j;


        }

        [HttpGet]
        [Route("ReadTaskDataOverdue")]
        public async Task<ActionResult> ReadTaskDataOverdue(string userId, string portalNames = null)
        {
            await Authenticate(userId, portalNames);
            var _context = _serviceProvider.GetService<IUserContext>();
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _context.PortalId;
            }
            var result = await _eGovernanceBusiness.GetTaskList(ids);
            var j = Ok(result.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").OrderByDescending(x => x.StartDate));
            return j;
        }

        [HttpGet]
        [Route("ReadTaskDataCompleted")]
        public async Task<ActionResult> ReadTaskDataCompleted(string userId, string portalNames = null)
        {
            await Authenticate(userId, portalNames);
            var _context = _serviceProvider.GetService<IUserContext>();
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _context.PortalId;
            }
            var result = await _eGovernanceBusiness.GetTaskList(ids);

            var j = Ok(result.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").OrderByDescending(x => x.StartDate));
            return j;
        }


        [HttpGet]
        [Route("GetLOVIdNameList")]
        public async Task<ActionResult> GetLOVIdNameList(string lovType)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            var model = await _LOVBusiness.GetList(x => x.LOVType == lovType);

            list = model.OrderBy(x => x.SequenceOrder).ThenBy(x => x.Name).Select(x => new IdNameViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code
            }).ToList();

            return Ok(list);
        }

        [HttpGet]
        [Route("GetPropertyList")]
        public async Task<ActionResult> GetPropertyList(string wardId, string rentingType)
        {
            var data = await _eGovernanceBusiness.GetPropertyList(wardId, rentingType);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetAgreementDetails")]
        public async Task<ActionResult> GetAgreementDetails(string agreementNo)
        {
            var result = await _eGovernanceBusiness.GetAgreementDetails(agreementNo);

            return Ok(new { success = true, data = result });
        }


        [HttpGet]
        [Route("LockTask")]
        public async Task<ActionResult> LockTask(string taskId, string userId)
        {
            try
            {
                await Authenticate(userId);
                var _context = _serviceProvider.GetService<IUserContext>();
                if (userId.IsNullOrEmpty())
                {
                    userId = _context.UserId;
                }
                var result = await _taskBusiness.ChangeAssignee(taskId, userId);
                return Ok("Success");

            }
            catch (Exception e)
            {
                //return Json("Problem in Locking the Task, Please contact administrator", JsonRequestBehavior.AllowGet);
                return Ok("Error");
            }
        }
        [HttpGet]
        [Route("ReleaseTask")]
        public async Task<ActionResult> ReleaseTask(string taskId)
        {
            try
            {
                var result = await _taskBusiness.ChangeLockStatus(taskId);
                return Ok("Success");

            }
            catch (Exception e)
            {
                //return Json("Problem in Locking the Task, Please contact administrator", JsonRequestBehavior.AllowGet);
                return Ok("Error");
            }
        }
        [HttpGet]
        [Route("StartTask")]
        public async Task<ActionResult> StartTask(string taskId)
        {
            try
            {
                var result = await _taskBusiness.UpdateActualStartDate(taskId);
                return Ok("Success");

            }
            catch (Exception e)
            {

                return Ok("Error");
            }
        }

        //public async Task<ActionResult> ReAssignTerminatedEmployeeServices(string services, string userId)
        //{
        //    List<string> serviceIds = new List<string>();
        //    var Str = services.Trim(',');
        //    var ids = Str.Split(',').Distinct();
        //    foreach (var id in ids)
        //    {
        //        serviceIds.Add(id);
        //    }
        //    await _serviceBusiness.ReAssignTerminatedEmployeeServices(userId, serviceIds);
        //    return Ok(new { success = false });
        //}

        [HttpGet]
        [Route("OnlinePayment")]
        public async Task<ActionResult> OnlinePayment(string ntsId, string noteTableId, long amount, NtsTypeEnum ntsType, string assigneeUserId, string returnUrl)
        {
            var model = new OnlinePaymentViewModel()
            {
                NtsId = ntsId,
                UdfTableId = noteTableId,
                Amount = amount,
                NtsType = ntsType,
                UserId = assigneeUserId,
                ReturnUrl = $@"{returnUrl}"
            };

            var result = await _eGovernanceBusiness.UpdateOnlinePaymentDetails(model);
            if (result.IsSuccess)
            {
                return Ok(new { success = true, data = result });
            }
            return Ok(new { success = false, error = result.Messages.ToHtmlError() });
        }

        protected dynamic GenericPagingFn(dynamic source1, int pageNumber, int pageSize)
        {

            var source = source1;
            // Get's No of Rows Count   
            int count = source.Count;

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage,
                items
            };

            // Setting Header  
            //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            // Returing List of Customers Collections  
            return Ok(paginationMetadata);
        }

        [HttpGet]
        [Route("UpdateProjectProposalLikes")]
        public async Task<IActionResult> UpdateProjectProposalLikes(string proposalId, ProjectPropsalResponseEnum type, string userId)
        {
            var exist = await _eGovernanceBusiness.GetProposalLikesData(proposalId, type, userId);

            if (exist.IsNotNull() && exist.ResponseType == type)
            {
                await _eGovernanceBusiness.UpdateProjectProposalLikes(proposalId, null, userId, DataActionEnum.Edit);
            }
            else if (exist.IsNotNull() && exist.ResponseType != type)
            {
                await _eGovernanceBusiness.UpdateProjectProposalLikes(proposalId, type, userId, DataActionEnum.Edit);
            }
            else
            {
                await _eGovernanceBusiness.UpdateProjectProposalLikes(proposalId, type, userId, DataActionEnum.Create);
            }

            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("CitizenProjectsHomeList")]
        public async Task<IActionResult> CititzenProjectsHomeList(string type, string userId,string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var model = await _eGovernanceBusiness.GetProposalProjectsList(type, userId);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetWardData")]
        public async Task<IActionResult> GetWardData(string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _usercContext = _serviceProvider.GetService<IUserContext>();
            var data = await _eGovernanceBusiness.GetWardData(userId);
            return Ok(new {wardName= data.IsNotNull() ? data.WardName : null,wardId= data.IsNotNull() ? data.WardId : null });
        }
        #region Needs and wnats chart

        [HttpGet]
        [Route("GetChartByProjectCategory")]
        public async Task<IActionResult> GetChartByProjectCategory()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount("Category");
            list = list.OrderByDescending(x => x.ProjectsCount).Take(10).ToList();

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.ProjectCategoryName).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList()
            };
            return Ok(chartdata);
        }

        [HttpGet]
        [Route("GetChartByPopularProjects")]
        public async Task<IActionResult> GetChartByPopularProjects()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsList(null, null);

            var data = list.GroupBy(x => x.ProjectCategory).Select(g => new EGovDashboardViewModel { ProjectCategory = g.Key, LikesCount = g.Sum(x => x.LikesCount), ProjectCategoryName = g.FirstOrDefault().ProjectCategoryName }).ToList();

            data = data.Where(x => x.LikesCount > 0).ToList();
            data = data.OrderByDescending(x => x.LikesCount).Take(10).ToList();

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = data.Select(x => x.ProjectCategoryName).ToList(),
                ItemValueSeries = data.Select(x => x.LikesCount).ToList()
            };
            return Ok(chartdata);
        }
        
        [HttpGet]
        [Route("GetChartByProjectsUndertaken")]
        public async Task<IActionResult> GetChartByProjectsUndertaken()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount("undertaken");
            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.ProjectCategoryName).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList()
            };
            return Ok(chartdata);
        }

        [HttpGet]
        [Route("GetChartByProjectsStatus")]
        public async Task<IActionResult> GetChartByProjectsStatus()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount();

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.ProjectStatus).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList(),
                ItemStatusColor = list.Select(x => x.StatusColor).ToList()
            };
            return Ok(chartdata);
        }

        [HttpGet]
        [Route("GetChartByProjectLocation")]
        public async Task<IActionResult> GetChartByProjectLocation()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount("Location");

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.LocationName).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList()
            };
            return Ok(chartdata);
        }

        [HttpGet]
        [Route("GetChartByProjectCategories")]
        public async Task<IActionResult> GetChartByProjectCategories()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount("Category");

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.ProjectCategoryName).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList()
            };
            return Ok(chartdata);
        }


        [HttpGet]
        [Route("GetProjectsByLatLong")]
        public async Task<IActionResult> GetProjectsByLatLong()
        {
            var list = await _eGovernanceBusiness.ProjectsByLatLong();
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadNeedsAndWantsTaskData")]
        public async Task<IActionResult> ReadNeedsAndWantsTaskData(string categoryCodes, string userId, string portalName, string taskStatus, bool showAllTaskForAdmin = true)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var list = await _eGovernanceBusiness.GetNeedsAndWantsTaskList(categoryCodes, taskStatus, _userContext.PortalId, showAllTaskForAdmin);

            var j = Ok(list);
            return j;
        }

        [HttpGet]
        [Route("ReadNeedsAndWantsTaskListCount")]
        public async Task<IActionResult> ReadNeedsAndWantsTaskListCount(string categoryCodes, string userId, string portalName, bool showAllTaskForAdmin = true)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = await _eGovernanceBusiness.GetNeedsAndWantsTaskCount(categoryCodes, _userContext.PortalId, showAllTaskForAdmin);
            var j = Ok(result);
            return j;
        }

        #endregion

        #region upcoming projects

        [HttpGet]
        [Route("ReadUpcomingProjectDepartmentData")]
        public async Task<IActionResult> ReadUpcomingProjectDepartmentData(string categoryId, string wardId)
        {
            var data = new List<EGovProjectViewModel>();
            data = await _eGovernanceBusiness.GetUpcomingProjectList(categoryId, wardId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadUpcomingProjectCitizenData")]
        public async Task<IActionResult> ReadUpcomingProjectCitizenData(string categoryId, string wardId)
        {
            var data = new List<EGovProjectViewModel>();
            data = await _eGovernanceBusiness.GetUpcomingProjectList(categoryId, wardId);
            return Ok(data);
        }

        [HttpGet]
        [Route("DeleteUpcomingProject")]
        public async Task<IActionResult> DeleteUpcomingProject(string id, string serviceId)
        {
            var data = await _eGovernanceBusiness.DeleteUpcomingProject(id);
            await _serviceBusiness.Delete(serviceId);
            return Ok(new { success = true });
        }
        #endregion

        #region Swachh sanjay
        [HttpGet]
        [Route("GetFacilityDetails")]
        public async Task<ActionResult> GetFacilityDetails(string portalId, string facilityCode)
        {
            var result = await _eGovernanceBusiness.GetFacilityDetails(facilityCode);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetFacilityList")]
        public async Task<ActionResult> GetFacilityList(string portalId)
        {
            var result = await _eGovernanceBusiness.GetFacilityList();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetFacilityLocationList")]
        public async Task<ActionResult> GetFacilityLocationList(string portalId)
        {
            var result = await _eGovernanceBusiness.GetFacilityLocationList();
            return Ok(result);
        }


        [HttpGet]
        [Route("GetPreviousFacilityStatus")]
        public async Task<ActionResult> GetPreviousFacilityStatus(string portalId, string facilityCode)
        {
            var result = await _eGovernanceBusiness.GetPreviousFacilityStatus(facilityCode);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetFacilityListByUser")]
        public async Task<ActionResult> GetFacilityListByUser(string portalId,string userId)
        {
            var result = await _eGovernanceBusiness.GetFacilityList(userId);
            return Ok(result);
        }

        #endregion
    } 
}


