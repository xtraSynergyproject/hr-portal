using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.UI.ViewModel;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using CMS.Common;
using CMS.Business;
using Newtonsoft.Json;
using AutoMapper;
using CMS.Data.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class HRDirectController : ApplicationController
    {
        private readonly IHRCoreBusiness _hRCoreBusiness;
        private readonly IUserContext _userContext;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly INoteIndexPageTemplateBusiness _noteIndexPageTemplateBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly ITeamBusiness _teamBusiness;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;
        private readonly IHierarchyMasterBusiness _hierarchyMasterBusiness;
        private readonly IUserBusiness _userBusiness;
        public HRDirectController(IHRCoreBusiness hRCoreBusiness, IUserContext userContext, ITemplateCategoryBusiness templateCategoryBusiness
            , INoteIndexPageTemplateBusiness noteIndexPageTemplateBusiness, ITemplateBusiness templateBusiness
            , INoteBusiness noteBusiness, IServiceBusiness serviceBusiness, ITaskBusiness taskBusiness
            , IUserHierarchyBusiness userHierarchyBusiness, IHierarchyMasterBusiness hierarchyMasterBusiness,
              IUserBusiness userBusiness)
        {
            _hRCoreBusiness = hRCoreBusiness;
            _userContext = userContext;
            _templateCategoryBusiness = templateCategoryBusiness;
            _noteIndexPageTemplateBusiness = noteIndexPageTemplateBusiness;
            _templateBusiness = templateBusiness;
            _noteBusiness = noteBusiness;
            _serviceBusiness = serviceBusiness;
            _taskBusiness = taskBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _userBusiness = userBusiness;
        }

        public IActionResult Index()
        {
            var model = new HRDirectViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageHRDirect(HRDirectViewModel model)
        {

            return Json(new { success = true });
        }

        public IActionResult HRDirectPage()
        {
            var model = new AssignmentViewModel();
            return View(model);
        }

        public async Task<IActionResult> HomePage(string userId, string personId, string postitionid, string tab, DataActionEnum DataAction, string jsMethod, string jsParam)
        {
            ViewBag.JavascriptMethod = jsMethod;
            ViewBag.JavascriptParam = jsParam;
            var model = new AssignmentViewModel();
            var userrole = _userContext.UserRoleCodes.IsNullOrEmpty()? new string[] { } : _userContext.UserRoleCodes.Split(",");
            if (personId.IsNotNullAndNotEmpty())
            {
                var persondetail = await _hRCoreBusiness.GetAssignmentDetails(personId, null);
                model = persondetail.FirstOrDefault();
                model.PersonId = personId;
            }
            //else if (!userrole.Contains("HR"))
            else
            {
                var personuserdetail = await _hRCoreBusiness.GetAssignmentDetails(null, _userContext.UserId);
                model = personuserdetail.FirstOrDefault();
            }
            if (model == null || DataAction == DataActionEnum.Create)
            {
                var newmodel = new AssignmentViewModel();
                newmodel.UserRoleCodes = userrole;
                newmodel.ActiveTab = tab;
                return View(newmodel);
            }
            model.UserRoleCodes = userrole;
            model.ActiveTab = tab;
          
            return View(model);
        }

        public async Task<IActionResult> ManageBenefits(string userId, string personId, string postitionid)
        {


            ViewBag.Title = "Vacation Accrual";

            var year = DateTime.Today.Year;
            var month = (MonthEnum)DateTime.Today.Month;
            var yearmonth = string.Concat(year, DateTime.Today.Month.ToString().PadLeft(2, '0')).ToSafeInt();
            var startDate = DateTime.Today.FirstDateOfMonth();
            DateTime lastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
            DateTime endDate = lastDate;


            var model = new PayrollReportViewModel { Year = year, Month = month, YearMonth = yearmonth, PersonId = personId, StartDate = startDate, EndDate = endDate };
            return View(model);
        }

        public async Task<JsonResult> GetPersonList()
        {
            var result = await _hRCoreBusiness.GetPersonList();

            return Json(result);
        }
        public async Task<JsonResult> GetYearList(string value)
        {
            var currentYear = DateTime.Today.Year;
            var list = new List<SelectListItem>();
            while (currentYear >= ApplicationConstant.ApplicationStartYear)
            {
                list.Add(new SelectListItem { Text = currentYear.ToString(), Value = currentYear.ToString(), Selected = currentYear.ToString() == value });
                currentYear--;
            }
            //return list;

            return Json(list);
        }
        public async Task<JsonResult> GetPersonListWithDetails(string legalEntityId)
        {
            var result = await _hRCoreBusiness.GetAssignmentDetails(null, null, legalEntityId);

            return Json(result);
        }


        public async Task<IActionResult> Team(string userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        public async Task<ActionResult> ReadTeamData( string userId)
        {
            var model = await _hRCoreBusiness.GetUserTeamDetail(userId);
            var data = model;

            //var dsResult = data;
            return Json(data);
        }

        public async Task<ActionResult> EmployeeDocument(string userId,LayoutModeEnum? lo)
        {
            var userRole = "";
            var role = _userContext.UserRoleCodes.IsNullOrEmpty() ? "" : _userContext.UserRoleCodes;
            if (role.Contains("HR"))
            {
                userRole = "HR";
            }

            var person = new PersonProfileViewModel();
            var category = await _templateCategoryBusiness.GetSingle(x => x.Code == "PersonDocuments");
            var templatelist = await _templateBusiness.GetList(x => x.TemplateCategoryId == category.Id);
            person.NoteTableRows = new List<NoteIndexPageTemplateViewModel>();
            foreach (var template in templatelist)
            {
                var noteIndex = await _noteIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == template.Id);
                if (noteIndex != null)
                {
                    noteIndex.SelectedTableRows = await _noteIndexPageTemplateBusiness.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == noteIndex.Id);
                    noteIndex.TemplateName = template.DisplayName;
                    person.NoteTableRows.Add(noteIndex);
                }
            }
            person.UserId = userId;
            person.UserRole = userRole;
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
                ViewBag.lo = LayoutModeEnum.Popup;
            }
            return PartialView(person);
        }
        public async Task<IActionResult> LoadNoteIndexPageGrid( string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string noteStatusCode, string userId)
        {
            var dt = await _noteBusiness.GetNoteIndexPageGridData(null, indexPageTemplateId, ownerType, noteStatusCode, userId);

            return Json(dt);
        }
        public async Task<JsonResult> ReadPersonDocumentRequestList(string userId)
        {
            var depenList = await _hRCoreBusiness.GetPersonRequestDocumentList(userId);
           // var dsResult = depenList.ToDataSourceResult(request);
            return Json(depenList);
        }
        //public async Task<IActionResult> ReadPersonIDDocuments([DataSourceRequest] DataSourceRequest request, long PersonId)
        //{
        //    var model = await _hRCoreBusiness.GetPersonIDDocuments(PersonId);

        //    return Json(model.ToDataSourceResult(request));
        //}

        //public async Task<IActionResult> ReadPersonPassportDocuments([DataSourceRequest] DataSourceRequest request, long PersonId)
        //{
        //    var model = await _hRCoreBusiness.GetPersonPassportDocuments(PersonId);

        //    return Json(model.ToDataSourceResult(request));
        //}

        //public async Task<IActionResult> ReadPersonVisaDocuments([DataSourceRequest] DataSourceRequest request, long PersonId)
        //{
        //    var model = await _hRCoreBusiness.GetPersonVisaDocuments(PersonId);

        //    return Json(model.ToDataSourceResult(request));
        //}

        //public async Task<IActionResult> ReadPersonEducationDocuments([DataSourceRequest] DataSourceRequest request, long PersonId)
        //{
        //    var model = await _hRCoreBusiness.GetPersonEducationDocuments(PersonId);

        //    return Json(model.ToDataSourceResult(request));
        //}

        //public async Task<IActionResult> ReadPersonExperienceDocuments([DataSourceRequest] DataSourceRequest request, long PersonId)
        //{
        //    var model =await _hRCoreBusiness.GetPersonExperienceDocuments(PersonId);

        //    return Json(model.ToDataSourceResult(request));
        //}

        //public async Task<IActionResult> ReadPersonTrainingDocuments([DataSourceRequest] DataSourceRequest request, long PersonId)
        //{
        //    var model = await _hRCoreBusiness.GetPersonTrainingDocuments(PersonId);

        //    return Json(model.ToDataSourceResult(request));
        //}


        public async Task<IActionResult> Dependant(string userId, string personId)
        {
            var userRole = "";
            var role = _userContext.UserRoleCodes.IsNullOrEmpty() ? "" : _userContext.UserRoleCodes;
            if (role.Contains("HR"))
            {
                userRole = "HR";
            }

            if (userId.IsNullOrEmpty())
            {
                var person = await _hRCoreBusiness.GetPersonDetailsById(personId);
                var personuserid = string.Empty;
                if (person!=null && person.UserId.IsNullOrEmpty())
                {
                    personuserid = person.UserId;
                }
                var model = new DependentViewModel()
                {
                    PersonId = personId,
                    //UserId = person.UserId,
                    UserId = personuserid,
                    UserRole = userRole
                };
                return View(model);
            }
            else
            {
                var user = await _hRCoreBusiness.GetPersonDetail(userId);
                var model = new DependentViewModel()
                {
                    PersonId = user.PersonId,
                    UserId = userId,
                    UserRole = userRole
                };
                return View(model);
            }
        }


        public async Task<JsonResult> ReadDependantList(/*[DataSourceRequest] DataSourceRequest request,*/ string personId, string ntsstatus)
        {
            var depenList = await _hRCoreBusiness.GetDependentList(personId, ntsstatus);
            var dsResult = depenList/*.ToDataSourceResult(request)*/;
            return Json(dsResult);
        }

        public async Task<IActionResult> DependantDocumentList(string dependentId)
        {
            var userRole = "";
            var role = _userContext.UserRoleCodes;
            if (role.Contains("HR"))
            {
                userRole = "HR";
            }

            var model = new DependentViewModel()
            {
                DependentId=dependentId,
                UserRole = userRole
            };
            return View(model);
        }

        public async Task<JsonResult> ReadDependentDocumentList(/*[DataSourceRequest] DataSourceRequest request,*/ string dependentId)
        {

            var depenList = await _hRCoreBusiness.GetDependentDocumentList(dependentId);
            var dsResult = depenList/*.ToDataSourceResult(request)*/;
            return Json(dsResult);
        }
        public async Task<JsonResult> GetTemplateCodeByNoteId(string ntsnoteId)
        {
            var result = await _noteBusiness.GetSingleById(ntsnoteId);
            return Json(result.TemplateCode);
        }


        public async Task<JsonResult> ReadDependantDocumentRequestList(string userId)
        {
            var depenList = await _hRCoreBusiness.GetDependentRequestDocumentList(userId);
            var dsResult = depenList;
            return Json(dsResult);
        }

        public async Task<JsonResult> ReadServiceList(string userId)
        {
            var search = new ServiceSearchViewModel { UserId = userId.IsNotNull() ? userId : _userContext.UserId, FilterUserId = userId.IsNotNull() ? userId : _userContext.UserId, TemplateCategoryType = TemplateCategoryTypeEnum.Standard };

            var result = await _serviceBusiness.GetSearchResult(search);
             return Json(result);
        }
        public async Task<JsonResult> ReadTaskList(string userId)
        {
            var search = new TaskSearchViewModel { UserId = userId.IsNotNull() ? userId : _userContext.UserId, Mode = "ASSIGN_TO" };

            var result = await _taskBusiness.GetSearchResult(search);
            
            return Json(result);
        }
        public IActionResult Service(string userId)
        {
            var person = new PersonProfileViewModel { UserId = userId };
            return View(person);
        }

        public IActionResult Task(string userId)
        {
            var person = new PersonProfileViewModel { UserId = userId };
            return View(person);
        }
        public IActionResult Misconduct(string EmpId)
        {


            var model = new MisconductViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            return View(model);
        }
        public async Task<ActionResult> GetMisconductGridData(string Id = null, string UserId = null)

        {
            var userid = "";


            if (!string.IsNullOrEmpty(Id))
            {
                userid = UserId;
            }
            else { userid = _userContext.UserId; }


            var model = await _hRCoreBusiness.GetMisconductDetails(userid);

            var j = Json(model);
            return j;
            /// return Json(model);




        }
        public IActionResult AccessLog(string EmpId, string UserId, bool hideUser = false)
        {

            ViewBag.HideUser = hideUser;
            var model = new AccessLogViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.PersonId = spl[0];
                model.UserId = spl[1];
            }


            return View(model);
        }
        public async Task<ActionResult> GetAccessLogGridData(string Id = null, string UserId = null, string userIds = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            if (Id.IsNullOrEmpty())
            {
                var model = await _hRCoreBusiness.GetAllAccessLogList(startDate, dueDate);
                var j = Json(model);
                return j;
            }
            else
            {
                var model = await _hRCoreBusiness.GetAccessLogList(Id, UserId, userIds, startDate, dueDate);

                var j = Json(model);
                return j;
                /// return Json(model);

            }
        }


        [HttpGet]
        public async Task<IActionResult> GetJobDesc(string Departmentid, string JobId)
        {
            if (Departmentid.IsNotNullAndNotEmpty())
            {
                {
                    string UserRole = "";
                    var Roles = _userContext.UserRoleCodes;
                    string[] roleArry = Roles.Split(',');
                    for (int i = 0; i < roleArry.Length; i++)
                    {
                        if (roleArry[i].ToLower() == "hr" || roleArry[i].ToLower() == "admin" || roleArry[i].ToLower() == "hr_head" || roleArry[i].ToLower() == "project_manager")
                        {
                            UserRole = "HR";
                            break;
                        }
                        else if (roleArry[i].ToLower() == "user")
                        {
                            UserRole = "User";
                        }
                        else { UserRole = "User"; }
                    }
                    var data = await _hRCoreBusiness.GetJobDescription(Departmentid, JobId);
                    string Id = null;
                    if (data.IsNotNull())
                    {
                        Id = data.Id;
                    }
                    return Json(new { success = true, result = Id, userRole = UserRole });
                }
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> LeaveApproveHierarchy(string hierarchyId)
        {
            var userId = _userContext.UserId;
            ViewBag.Title = "Approval Hierarchy";

            if (hierarchyId.IsNullOrEmpty())
            {
                var model = await _hierarchyMasterBusiness.GetList(x => x.HierarchyType == HierarchyTypeEnum.User);
                var hierarchy = model.FirstOrDefault();
                hierarchyId = hierarchy.Id;
            }

            var viewmodel = await _userHierarchyBusiness.GetLeaveApprovalHierarchyUser(userId, hierarchyId);
            viewmodel.HierarchyId = hierarchyId;
            return View(viewmodel);
        }

        public async Task<IActionResult> LeaveApproveHierarchyByUserId(string userId, string hierarchyId)
        {
            var list = new List<HierarchyMasterViewModel>();
            if (hierarchyId.IsNotNullAndNotEmpty())
            {
                list = await _hierarchyMasterBusiness.GetList(x => x.Id == hierarchyId);
            }
            else
            {
                list = await _hierarchyMasterBusiness.GetList();

            }
            ViewBag.UserId = userId;
            ViewBag.hierarchyId = hierarchyId;

            return View(list.FirstOrDefault());
        }
        public async Task<IActionResult> EmployeeProfile(string personId,LayoutModeEnum? lo)
        {
             var model = new PersonProfileViewModel();
            //model.PersonId = personId;
            var person = await _hRCoreBusiness.GetEmployeeProfile(personId);
            // model.PersonId = person.PersonId;
            if (lo==LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            if (person!=null) 
            {
                return View("EmployeeProfile", person);
            }
            return View("EmployeeProfile", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationList()
        {
            var list = await _hRCoreBusiness.GetAllLocation();
            return Json(list);
        }


        public IActionResult RemoteSignInSignOut(string EmpId)
        {
            var model = new RemoteSignInSignOutViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetIdDialCodeById(string id)
        {
            var model = await _hRCoreBusiness.GetIdDialCodeById(id);
            return Json(new { success = true, code = model });
        }

        public async Task<ActionResult> GetRemoteSignInSingOutGridData(string Id = null, string UserId = null)
        {
            var userid = "";

            if (!string.IsNullOrEmpty(Id))
            {
                userid = UserId;
            }
            else { userid = _userContext.UserId; }

            var model = await _hRCoreBusiness.GetRemotesigninsignoutDetails(userid);

            var j = Json(model);
            return j;
            /// return Json(model);

        }
        
        public async Task<ActionResult>  TerminatePerson(string personId = null, string layoutMode = null)
        {
            ViewBag.Title = "Terminate Person";
            var vm = new TerminatePersonViewModel();
            if (personId != null)
            {
                vm =await  _hRCoreBusiness.GetPersonInfoForTermination(personId);
                vm.PersonTerminateDate = DateTime.Now.ApplicationNow().Date;
                vm.DataAction = DataActionEnum.Edit;
            }
           // vm.LayoutMode = layoutMode.IsNullOrEmpty() ? LayoutModeEnum.Main : layoutMode.ToEnum<LayoutModeEnum>();
            //if (vm.LayoutMode == LayoutModeEnum.Iframe)
            //{
            //    ViewBag.Layout = "~/Views/Shared/_LayoutPopup.cshtml";
            //}
            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateTerminatePerson(TerminatePersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _hRCoreBusiness.UpdatePersonForTermination(model);
                if (!result.IsSuccess)
                {
                    result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                }
                else
                {
                    return Json(new { success = true, operation = model.DataAction.ToString() });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        public async Task<ActionResult> GetAssignToListVirtualData([DataSourceRequest] DataSourceRequest request, AssignToTypeEnum? assignToType, string teamId)
        {
            var data =await GetAssignToList(request, assignToType, teamId);
            request.Filters.Clear();
            return Json(data.ToDataSourceResult(request));
        }

      

        public async Task<ActionResult> GetAssignToValueMapper(string value, AssignToTypeEnum? assignToType, string teamId)
        {
            var dataItemIndex = -1;

            if (value.IsNotNullAndNotEmpty())
            {
                var data =await GetAssignToList(null, assignToType, teamId);
                var item = data.FirstOrDefault(x => x.Id == value);
                dataItemIndex = data.IndexOf(item);
            }
            return Json(dataItemIndex);
        }
       
        private async Task<IList<UserListOfValue>> GetAssignToList(DataSourceRequest request = null, AssignToTypeEnum? assignToType = null, string teamId = null)
        {
            var searchParam = "";
            if (request != null && request.Filters.Count > 0)
            {
                searchParam = Convert.ToString(((Kendo.Mvc.FilterDescriptor)request.Filters.FirstOrDefault()).ConvertedValue);
            }
            var data = new List<UserListOfValue>();
            switch (assignToType)
            {
                case AssignToTypeEnum.User:
                    var userlist = await _hRCoreBusiness.GetAssignmentDetails(null,null,null) ;
                    data = userlist.Select(x=>new UserListOfValue()
                    { 
                        Id=x.UserId,
                        UserId=x.UserId,
                        PositionName=x.PositionName,
                        JobName=x.JobName,
                        OrganizationName=x.DepartmentName,
                        PersonNo=x.PersonNo,
                        DisplayName=x.PersonFullName,
                        Name=x.PersonFullName,
                        LocationName=x.LocationName,
                        AssignmentId=x.AssignmentId,
                        Email=x.Email,
                        GradeName=x.GradeName,
                        PhotoId=x.PhotoId,
                        SponsorshipNo=x.SponsorshipNo,
                        PersonStatus=x.PersonStatus
                    }).ToList();
                    break;
                case AssignToTypeEnum.Team:
                    //data = _teamBusiness.GetTeamMemberList(teamId, searchParam).ToList();
                    break;
                case AssignToTypeEnum.Query:
                    break;
                default:
                    break;
            }
            // var data = _business.GetList(x => x.IsActive());
            return data;
        }


        public ActionResult TaskDetails()
        {

            var model = new TaskDetailsViewModel();
            return View(model);
        }


        public async Task<IActionResult> GetCategory()
        {

            var Category = await _hRCoreBusiness.GetCategory();
            return Json(Category);
        }

        public async Task<IActionResult> GetTemplates(string Category)
        {
         //   if (Category.Length > 0)
            {

                //var roles = db.Roles.Where(r => listOfRoleId.Contains(r.RoleId));
                //  var Template = await _templateBusiness.GetList(x => x.TemplateCategoryId == Category);

                var Template = await _hRCoreBusiness.GetTemplatecategoryWiseList(Category);
                return Json(Template);
            }
            
        }

        public async Task<IActionResult> GetTemplatesService(string Category)
        {
            //   if (Category.Length > 0)
            {

                //var roles = db.Roles.Where(r => listOfRoleId.Contains(r.RoleId));
                //  var Template = await _templateBusiness.GetList(x => x.TemplateCategoryId == Category);

                var Template = await _hRCoreBusiness.GetTemplatecategoryWiseListService(Category);
                return Json(Template);
            }

        }

        public async Task<IActionResult> Assignee(string Category,string Template)
        {

            var Users = await _hRCoreBusiness.GetAllTaskAssigneeList(Category, Template);
            return Json(Users);
        }
        public async Task<ActionResult> GetTaskDetailsData( TaskDetailsViewModel Model)
        {

            var model = await _hRCoreBusiness.GetTaskDetailList(Model);
           
            var j = Json(model);
            return j;
        }



        public ActionResult GetServiceDetails()
        {
            var model = new TaskDetailsViewModel();
            return View(model);
            
        }

        public async Task<IActionResult> GetCategoryservice()
        {

            var Category = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Service);
            return Json(Category);
        }

        public async Task<IActionResult> OwnerName(string Category, string Template)
        {

            var Users = await _hRCoreBusiness.GetOwnerNameList(Category, Template);
            return Json(Users);
        }


        public async Task<ActionResult> GetServiceDetailsData(TaskDetailsViewModel Model)
        {

            var model = await _hRCoreBusiness.GetServiceDetailsList(Model);

            var j = Json(model);
            return j;
        }
    }
}
