using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class UserController : ApplicationController
    {
        private IUserBusiness _userBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        private IUserRolePermissionBusiness _userRolePermissionBusiness;
        private IPageBusiness _pageBusiness;
        private ITeamBusiness _teamBusiness;
        private ITeamUserBusiness _teamUserBusiness;
        private ICandidateProfileBusiness _candidateProfileBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private IPortalBusiness _portalBusiness;
        private IUserPortalBusiness _userPortalBusiness;
        private IUserPermissionBusiness _userPermissionBusiness;
        private IPermissionBusiness _permissionBusiness;
        private static IServiceProvider _services;
        private readonly IConfiguration _configuration;
        private IUserHierarchyPermissionBusiness _userHierarchyPermissionBusiness;
        private IUserEntityPermissionBusiness _userEntityPermissionBusiness;
        private ILegalEntityBusiness _legalEntityBusiness;
        private IUserContext _userContext;
        private readonly IHttpContextAccessor _accessor;
        private IUserRoleBusiness _userRoleBusiness;
        private IUserPreferenceBusiness _userPreferenceBusiness;
        public UserController(IUserContext userContext, IUserBusiness userBusiness, ITeamBusiness teamBusiness, ITeamUserBusiness teamUserBusiness,
            ICandidateProfileBusiness candidateProfileBusiness, ILegalEntityBusiness legalEntityBusiness,
                      AuthSignInManager<ApplicationIdentityUser> customUserManager, IUserRolePermissionBusiness userRolePermissionBusiness
            , IPageBusiness pageBusiness, IUserRoleUserBusiness userRoleUserBusiness,
                      IPortalBusiness portalBusiness, IUserPortalBusiness userPortalBusiness,
            IUserPermissionBusiness userPermissionBusiness, IPermissionBusiness permissionBusiness
            , IServiceProvider services, IConfiguration configuration, IUserHierarchyPermissionBusiness userHierarchyPermissionBusiness
            , IUserEntityPermissionBusiness userEntityPermissionBusiness, IHttpContextAccessor accessor, IUserRoleBusiness userRoleBusiness
            , IUserPreferenceBusiness userPreferenceBusiness)
        {
            _accessor = accessor;
            _userContext = userContext;
            _userBusiness = userBusiness;
            _userRolePermissionBusiness = userRolePermissionBusiness;
            _teamBusiness = teamBusiness;
            _teamUserBusiness = teamUserBusiness;
            _candidateProfileBusiness = candidateProfileBusiness;
            _customUserManager = customUserManager;
            _pageBusiness = pageBusiness;
            _portalBusiness = portalBusiness;
            _userPortalBusiness = userPortalBusiness;
            _userPermissionBusiness = userPermissionBusiness;
            _permissionBusiness = permissionBusiness;
            _services = services;
            _configuration = configuration;
            _userHierarchyPermissionBusiness = userHierarchyPermissionBusiness;
            _userEntityPermissionBusiness = userEntityPermissionBusiness;
            _legalEntityBusiness = legalEntityBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
            _userRoleBusiness = userRoleBusiness;
            _userPreferenceBusiness = userPreferenceBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserDropDown()
        {
            return View(new UserViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> UserDropDown(UserViewModel model)
        {


            return View("UserDropDown", model);
        }




        public async Task<ActionResult> ReadData()
        {
            var model = await _userBusiness.GetList();
            return Json(model);
        }

        public IActionResult CreateUser()
        {
            return View("ManageUser", new UserViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //Password="123",
                // ConfirmPassword="123"
            });
        }

        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userBusiness.GetSingleById(Id);
            if (user != null)
            {
                var candidatelist = await _candidateProfileBusiness.GetList(x => x.Email == user.Email);
                if (candidatelist != null && candidatelist.Count() > 0)
                {
                    foreach (var candidate in candidatelist)
                    {
                        await _candidateProfileBusiness.Delete(candidate.Id);
                    }
                }
            }
            await _userBusiness.Delete(Id);

            return Json(true);
        }

        public async Task<IActionResult> EditUser(string Id)
        {
            var member = await _userBusiness.GetSingleById(Id);

            var userPortals = await _userPortalBusiness.GetPortalByUser(Id);
            member.Portal = new List<string>();
            if (userPortals.IsNotNull())
            {
                foreach (var up in userPortals)
                {
                    member.Portal.Add(up.PortalId);
                }
            }
            var userRoleUsers = await _userRoleUserBusiness.GetUserRoleByUser(Id);
            member.UserRole = new List<string>();
            if (userRoleUsers.IsNotNull())
            {
                foreach (var uru in userRoleUsers)
                {
                    member.UserRole.Add(uru.UserRoleId);
                }
            }

            if (member != null)
            {
                member.ConfirmPassword = member.Password;
                member.DataAction = DataActionEnum.Edit;
                return View("ManageUser", member);
            }
            return View("ManageUser", new UserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserViewModel model)
        {
            var result = await _userBusiness.Create(model);
            if (result.IsSuccess)
            {
                ViewBag.Success = true;
                //Add UserPortal data
                if (model.Portal.IsNotNull())
                {
                    foreach (var p in model.Portal)
                    {
                        var res = await _userPortalBusiness.Create(new UserPortalViewModel
                        {
                            UserId = result.Item.Id,
                            PortalId = p,
                        });
                    }
                }
                return Json(new { success = true, message = result.Message });
            }
            else
            {
                return Json(new { success = false, error = result.HtmlError });
                //ModelState.AddModelErrors(result.Messages);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //Add UserPortal data
                        if (model.Portal.IsNotNull())
                        {
                            foreach (var p in model.Portal)
                            {
                                var res = await _userPortalBusiness.Create(new UserPortalViewModel
                                {
                                    UserId = result.Item.Id,
                                    PortalId = p,
                                });
                            }
                        }
                        if (model.UserRole.IsNotNull())
                        {
                            //var UserRoleUser = model.UserRoles.Join(",");
                            foreach (var ur in model.UserRole)
                            {
                                var res = await _userRoleUserBusiness.Create(new UserRoleUserViewModel
                                {
                                    UserId = result.Item.Id,
                                    UserRoleId = ur,
                                });
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var existingItem = await _userBusiness.GetSingleById(model.Id);
                    existingItem.Name = model.Name;
                    existingItem.JobTitle = model.JobTitle;
                    existingItem.Email = model.Email;
                    // existingItem.Password = model.Password;
                    // existingItem.ConfirmPassword = model.ConfirmPassword;
                    model.Password = existingItem.Password;
                    model.ConfirmPassword = existingItem.Password;
                    var result = await _userBusiness.Edit(model);
                    if (result.IsSuccess)
                    {

                        var portals = await _userPortalBusiness.GetPortalByUser(model.Id);

                        if (portals.Count() > 0)
                        {
                            foreach (var p in portals)
                            {
                                if (model.Portal == null || model.Portal.Contains(p.PortalId) == false)
                                {
                                    await _userPortalBusiness.Delete(p.Id);
                                }
                            }
                        }
                        if (model.Portal.IsNotNull())
                        {
                            foreach (var p in model.Portal)
                            {
                                if (!portals.Where(x => x.PortalId == p).Any())
                                {
                                    var res = await _userPortalBusiness.Create(new UserPortalViewModel
                                    {
                                        UserId = result.Item.Id,
                                        PortalId = p,
                                    });
                                }
                            }
                        }
                        var userRoles = await _userRoleUserBusiness.GetUserRoleByUser(model.Id);

                        if (userRoles.Count() > 0)
                        {
                            foreach (var ur in userRoles)
                            {
                                if (model.UserRole == null || model.UserRole.Contains(ur.UserRoleId) == false)
                                {
                                    await _userRoleUserBusiness.Delete(ur.Id);
                                }
                            }
                        }
                        if (model.UserRole.IsNotNull())
                        {
                            foreach (var ur in model.UserRole)
                            {
                                if (!userRoles.Where(x => x.UserRoleId == ur).Any())
                                {
                                    var res = await _userRoleUserBusiness.Create(new UserRoleUserViewModel
                                    {
                                        UserId = result.Item.Id,
                                        UserRoleId = ur,
                                    });
                                }
                            }
                        }

                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageUser", model);
        }
        [HttpGet]
        public async Task<ActionResult> GetUserIdNameList(string viewData = null, string portalId = null)
        {
            var data = await _userBusiness.GetUserIdNameList();
            if (portalId.IsNotNullAndNotEmpty())
            {
                var data1 = await _userPortalBusiness.GetUserByPortal(portalId);
                data = data1.Select(x => new IdNameViewModel() { Id = x.Id, Name = x.Name }).ToList();
            }
            if (viewData != null)
            {
                ViewData[viewData] = data;
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetUser(string userId)
        {
            var data = await _userBusiness.GetSingleById(userId);
            return Json(data);
        }
        public async Task<ActionResult> GetUsersListView(string selectedValues)
        {
            List<UserViewModel> list = new List<UserViewModel>();
            if (selectedValues.IsNotNullAndNotEmpty())
            {
                var selectedValueslist = selectedValues.Split(",");
                if (selectedValueslist.Length > 0)
                {
                    foreach (var id in selectedValueslist)
                    {
                        var user = await _userBusiness.GetSingleById(id);
                        list.Add(user);
                    }
                }
            }
            return Json(list);
        }
        public async Task<ActionResult> GetUsersList([DataSourceRequest] DataSourceRequest request, string userRoleId)
        {
            List<UserRoleUserViewModel> list = new List<UserRoleUserViewModel>();
            if (userRoleId.IsNotNullAndNotEmpty())
            {
                list = await _userRoleUserBusiness.GetList(x => x.UserRoleId == userRoleId);
                if (list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        var user = await _userBusiness.GetSingleById(item.UserId);
                        item.UserName = user.Name;
                    }
                }
            }
            return Json(list);
            // return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetUsersList1(string userRoleId)
        {
            List<UserRoleUserViewModel> list = new List<UserRoleUserViewModel>();
            if (userRoleId.IsNotNullAndNotEmpty())
            {
                list = await _userRoleUserBusiness.GetList(x => x.UserRoleId == userRoleId);
                if (list.Count() > 0)
                {
                    foreach (var item in list)
                    {

                        var user = await _userBusiness.GetSingleById(item.UserId);
                        if (user.IsNotNull())
                        {
                            item.UserName = user.Name;
                        }
                    }
                }
            }
            var data = list;
            return Json(data);
            // return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetUsersListOnTypeBasis([DataSourceRequest] DataSourceRequest request, string assigneeType, string teamId, string portalId)
        {
            var list = new List<UserViewModel>();
            if (assigneeType == "TASK_ASSIGN_TO_USER" || assigneeType == "1")
            {
                list = await _userBusiness.GetList();
                if (portalId.IsNotNullAndNotEmpty())
                {
                    list = await _userPortalBusiness.GetUserByPortal(portalId);

                }
            }
            else
            {
                if (teamId.IsNotNullAndNotEmpty())
                {
                    var team = await _teamBusiness.GetSingleById(teamId);
                    var teamUsers = await _teamUserBusiness.GetList(x => x.TeamId == teamId);
                    foreach (var id in teamUsers)
                    {
                        var user = await _userBusiness.GetSingleById(id.UserId);
                        if (portalId.IsNotNullAndNotEmpty())
                        {
                            var data = await _userPortalBusiness.GetSingle(x => x.PortalId == portalId && x.UserId == id.UserId);
                            if (data.IsNotNull())
                            {
                                list.Add(user);
                            }
                        }
                        else
                        {
                            list.Add(user);
                        }

                    }
                }
            }

            list = list.Where(x=>x.Name.IsNotNullAndNotEmpty()).OrderBy(x => x.Name).ToList();
            return Json(list);
            //return Json(list.ToDataSourceResult(request).Data);
        }
        public async Task<ActionResult> GetUsersIdNameList()
        {
            List<UserViewModel> list = new List<UserViewModel>();
            list = await _userBusiness.GetList();
            return Json(list);
        }
        [HttpGet]
        public async Task<JsonResult> GetPortalForUser(string id)
        {
            //var data = await _portalBusiness.GetPortalForUser(id);
            var data = await _portalBusiness.GetPortals(id);
            return Json(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetTeamOwnerUser(string id)
        {
            var teamUsers = await _teamUserBusiness.GetList(x => x.TeamId == id);
            var data = teamUsers.Where(x => x.IsTeamOwner == true).FirstOrDefault();
            return Json(new { success = true, userId = data.UserId });
        }

        public async Task<IActionResult> ManagePermissionForUser(string id, string userId)
        {
            ViewBag.UserId = userId;
            var userDetails = await _userBusiness.GetSingleById(userId);
            ViewBag.UserName = userDetails.Name;
            return View();
        }

        public async Task<IActionResult> GetPortalTreeList(string id, string userId)
        {
            var result = await _pageBusiness.GetPortalTreeList(id, userId);
            var model = result.ToList();
            return Json(model);
        }


        public async Task<object> GetPortalFancyTreeList(string id, string userId)
        {
            var result = await _pageBusiness.GetPortalTreeList(id, userId);
            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel
            {
                key = x.id,
                title = x.Name,
                lazy = true,
                FieldDataType = x.FieldDataType.ToString(),
                ParentId = id,
                checkbox = x.HideCheckbox,
                selected = x.Checked
            }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
        }



        public async Task<IActionResult> GetPortalRoleTreeList(string id, string userId)
        {
            var result = await _pageBusiness.GetPortalRoleTreeList(id, userId);
            return Json(result);
        }

        public async Task<object> GetPortalRoleFancyTreeList(string id, string userId)
        {
            var result = await _pageBusiness.GetPortalRoleTreeList(id, userId);
            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel
            {
                key = x.id,
                title = x.Name,
                lazy = true,
                FieldDataType = x.FieldDataType.ToString(),
                ParentId = id,
                checkbox = x.HideCheckbox,
                selected = x.Checked,

            }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
        }


        public async Task<IActionResult> ManageUserRolePermission(string userRoleId, string pageId, string[] permissionId, bool isChecked)
        {
            var userPermission = await _userRolePermissionBusiness.GetSingle(x => x.UserRoleId == userRoleId && x.PageId == pageId);
            // var permission = await _permissionBusiness.GetSingleById(permissionId);
            if (userPermission.IsNotNull() /*&& permission.IsNotNull()*/)
            {
                //if (isChecked)
                //{
                //  userPermission.Permissions = AddPermission(userPermission.Permissions, permission.Code);
                //}
                //else
                //{

                //    userPermission.Permissions = userPermission.Permissions.RemoveItemFromArray(permission.Code);
                //}
                userPermission.Permissions = permissionId;
                userPermission.PortalId = _userContext.PortalId;
                userPermission.LegalEntityId = _userContext.LegalEntityId;
                var res = await _userRolePermissionBusiness.EditUserRolePermission(userPermission);
                if (res.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            else
            {
                var model = new UserRolePermissionViewModel
                {
                    UserRoleId = userRoleId,
                    PageId = pageId
                };
                string[] array = Array.Empty<string>();
                //model.Permissions = AddPermission(array, permission.Code);
                model.Permissions = permissionId;//AddPermission(array, permission.Code);
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;
                var res = await _userRolePermissionBusiness.CreateUserRolePermission(model);
                if (res.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserPermission(string userId, string pageId, string[] permissionId, bool isChecked)
        {
            var userPermission = await _userPermissionBusiness.GetSingle(x => x.UserId == userId && x.PageId == pageId);
            //var permission = await _permissionBusiness.GetSingleById(permissionId);
            if (userPermission.IsNotNull() /*&& permission.IsNotNull()*/)
            {
                //if (isChecked)
                //{
                //    userPermission.Permissions = AddPermission(userPermission.Permissions, permission.Code);
                //}
                //else
                //{
                //    userPermission.Permissions = userPermission.Permissions.RemoveItemFromArray(permission.Code);
                // }
                userPermission.Permissions = permissionId;
                userPermission.PortalId = _userContext.PortalId;
                userPermission.LegalEntityId = _userContext.LegalEntityId;
                var res = await _userPermissionBusiness.EditUserPermission(userPermission);
                if (res.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            else
            {
                var model = new UserPermissionViewModel
                {
                    UserId = userId,
                    PageId = pageId
                };
                string[] array = Array.Empty<string>();
                model.Permissions = permissionId;
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;
                //model.Permissions = AddPermission(array, permission.Code);
                var res = await _userPermissionBusiness.CreateUserPermission(model);
                if (res.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }
        string[] AddPermission(string[] array, string newValue)
        {
            int newLength = array.Length + 1;

            string[] result = new string[newLength];

            for (int i = 0; i < array.Length; i++)
                result[i] = array[i];

            result[newLength - 1] = newValue;

            return result;
        }

        string[] RemoveAtPermission(string[] array, int index)
        {
            int newLength = array.Length - 1;

            if (array.Length < 1)
            {
                return array;
            }

            string[] result = new string[newLength];
            int newCounter = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (i == index)
                {
                    continue;
                }
                result[newCounter] = array[i];
                newCounter++;
            }

            return result;
        }


        public async Task<ActionResult> ReadUserData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _userBusiness.GetUserList();
            var data = model.ToList();

            // var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }

        public async Task<ActionResult> ReadUserTeamData([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await _userBusiness.GetUserTeamList(Id);
            var data = model.ToList();

            // var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }
        public async Task<IActionResult> SendWelcomeEmail(string userId)
        {
            var user = await _userBusiness.GetSingleById(userId);
            if (user != null)
            {
                await _userBusiness.SendWelcomeEmail(user);
            }
            return Json(true);
        }
        public async Task<IActionResult> SendSummaryEmail()
        {
            //HangfireScheduler hf = new HangfireScheduler(_services, _configuration, _customUserManager, _userContext, _accessor);
            //HangfireScheduler hf = new HangfireScheduler();
           // await hf.SendEmailSummary();
            return Json(true);
        }
        public async Task<IActionResult> UpdateNtsStatus()
        {
            //var hf = new HangfireScheduler(_services, _configuration, _customUserManager, _userContext, _accessor);
           // await hf.UpdateNtsStatus();
            return Json(true);
        }
        public async Task<IActionResult> DailyJob()
        {
            //var hf = new HangfireScheduler(_services, _configuration, _customUserManager, _userContext, _accessor);
            //await hf.DailyJob();
            return Json(true);
        }
        private async Task<IList<UserViewModel>> GetUserList(DataSourceRequest request = null)
        {
            dynamic searchParam = null;
            if (request != null && request.Filters.Count > 0)
            {
                //searchParam = Convert.ToString(((Kendo.Mvc.FilterDescriptor)request.Filters.FirstOrDefault()).ConvertedValue);
                searchParam = request.Filters.FirstOrDefault();
                searchParam = searchParam.ConvertedValue;
            }
            var data = await _userBusiness.GetActiveUserList();
            return data;
        }
        public async Task<ActionResult> GetUserListVirtualData([DataSourceRequest] DataSourceRequest request)
        {
            var data = await GetUserList(request);
            request.Filters.Clear();
            return Json(data);
            //return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetUserListVirtualDataNew()
        {
            var data = await GetUserList(null);
            // request.Filters.Clear();
            return Json(data);
        }

        public async Task<ActionResult> GetAllowedPortalList([DataSourceRequest] DataSourceRequest request)
        {
            var list = await _userBusiness.GetAllowedPortalList(_userContext.UserId);
            return Json(list);
        }
        public async Task<ActionResult> GetUserValueMapper(string value)
        {
            var dataItemIndex = -1;
            if (value != null)
            {
                var data = await GetUserList();
                var item = data.FirstOrDefault(x => x.Id == value);
                dataItemIndex = data.IndexOf(item);
            }
            return Json(dataItemIndex);
        }
        public async Task<IActionResult> UserHierarchyPermission(UserHierarchyPermissionViewModel model)
        {


            return View("UserHierarchyPermission", model);
        }

        public async Task<IActionResult> ManageUserHierarchyPermission(string userId, string pId)
        {
            if (pId.IsNotNullAndNotEmpty())
            {

                var member = await _userHierarchyPermissionBusiness.GetSingleById(pId);



                member.DataAction = DataActionEnum.Edit;
                return View("ManageUserHierarchyPermission", member);
            }
            else
            {
                var model = new UserHierarchyPermissionViewModel();
                model.UserId = userId;
                model.DataAction = DataActionEnum.Create;
                return View("ManageUserHierarchyPermission", model);
            }

        }
        [HttpPost]
        public async Task<ActionResult> ManageUserHierarchyPermission(UserHierarchyPermissionViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userHierarchyPermissionBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _userHierarchyPermissionBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageUserHierarchyPermission", model);
        }
        public ActionResult ReadUserHierarchyPermission(string userId)
        {

            //var list = await _userHierarchyPermissionBusiness.GetUserPermissionHierarchy(userId);
            //var json = Json(list.ToDataSourceResult(request));
            //return json;
            var model = _userHierarchyPermissionBusiness.GetUserPermissionHierarchy(userId);
            var data = model.Result;
            return Json(data);
        }
        public ActionResult GetHierarchyData(HierarchyTypeEnum type)
        {
            var model = _userHierarchyPermissionBusiness.GetHierarchyData(type);
            var data = model.Result;
            return Json(data);
        }

        public async Task<IActionResult> DeleteUserHierarchyPermission(string Id)
        {
            await _userHierarchyPermissionBusiness.Delete(Id);

            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> GetHierarchyIdNameList()
        {
            var dataList = await _userHierarchyPermissionBusiness.GetListGlobal<HierarchyMasterViewModel, HierarchyMaster>();
            return Json(dataList.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.HierarchyType.ToString() }).ToList());
        }
        //public async Task<IActionResult> UserEntityPermission(UserEntityPermissionViewModel model)
        //{


        //    return View("UserHierarchyPermission", model);
        //}
        public async Task<IActionResult> ManageUserEntityPermission(string type, string id, string pId)
        {
            var model = new UserEntityPermissionViewModel();


            if (type == "user")
            {

                model.UserEntityType = UserEntityTypeEnum.User;
                var userrole = await _userRoleUserBusiness.GetList(x => x.UserId == id);
                var ids = userrole.Select(x => x.Id);
                var idu = string.Join("','", ids);
                var result = await _userPermissionBusiness.GetUserPermissionList(id, idu, "");
                var mod = result.DistinctBy(x => x.PortalId).Select(x => new UserEntityPermissionViewModel { PortalId = x.PortalId, Portal = x.Portal }).ToList();
                var listview = "";
                foreach (var m in mod)
                {
                    var port = result.DistinctBy(x => x.PageId).Where(x => x.PortalId == m.PortalId).Select(x => new UserEntityPermissionViewModel { PageId = x.PageId, Page = x.Page }).ToList();
                    listview = string.Concat(listview, "&nbsp;<span class='hr-div-item' style='color:#781C68'>", m.Portal, "</span></br>");

                    foreach (var pg in port)
                    {
                        var page = result.Where(x => x.PageId == pg.PageId && x.Permissions.Any()).Select(x => new UserEntityPermissionViewModel { Permissions = x.Permissions }).ToList();
                        listview = string.Concat(listview, "&nbsp;<span class='hr-div-item' style='color:#319DA0'>", pg.Page, "</span></br>");
                        foreach (var ac in page)
                        {
                            foreach (var pr in ac.Permissions)
                            {
                                if (pr.IsNotNullAndNotEmpty())
                                {
                                    listview = string.Concat(listview, "&nbsp;<span class='hr-div-item'>", string.Join(",", pr), "</span></br>");
                                }
                            }
                        }
                    }

                }
                model.UserRolepermission = listview;
                model.UserEntityType = UserEntityTypeEnum.User;
            }
            else
            {
                model.UserEntityType = UserEntityTypeEnum.UserRole;
                var result = await _userRolePermissionBusiness.GetUserRolePermissionList(id);
                var mod = result.DistinctBy(x => x.PortalId).Select(x => new UserEntityPermissionViewModel { PortalId = x.PortalId, Portal = x.Portal }).ToList();
                var listview = "";
                foreach (var m in mod)
                {
                    var port = result.DistinctBy(x => x.PageId).Where(x => x.PortalId == m.PortalId).Select(x => new UserEntityPermissionViewModel { PageId = x.PageId, Page = x.Page }).ToList();

                    listview = string.Concat(listview, "&nbsp;<span class='hr-div-item' style='color:#781C68'>", m.Portal, "</span></br>");
                    foreach (var pg in port)
                    {
                        var page = result.Where(x => x.PageId == pg.PageId && x.Permissions.Any()).Select(x => new UserEntityPermissionViewModel { Permissions = x.Permissions }).ToList();
                        listview = string.Concat(listview, "&nbsp;<span class='hr-div-item' style='color:#319DA0'>", pg.Page, "</span></br>");
                        foreach (var ac in page)
                        {
                            foreach (var pr in ac.Permissions)
                            {
                                if (pr.IsNotNullAndNotEmpty())
                                {
                                    listview = string.Concat(listview, "&nbsp;<span class='hr-div-item'>", string.Join(",", pr), "</span></br>");
                                }
                            }
                        }
                    }

                }
                model.UserRolepermission = listview;
            }
            model.UserEntityId = id;
            var EList = await _userEntityPermissionBusiness.GetList(x => x.UserEntityId == id && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.LegalEntity);
            var orglist = await _userEntityPermissionBusiness.GetList(x => x.UserEntityId == id && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Organization);
            var tlist = await _userEntityPermissionBusiness.GetList(x => x.UserEntityId == id && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Template);
            if (EList.IsNotNull() && orglist.IsNotNull() && tlist.IsNotNull())
            {


                model.LegalEntityId1 = EList.Select(x => x.EntityModelId).ToArray();
                model.OrganisationId = orglist.Select(x => x.EntityModelId).ToArray();
                model.TemplateId = tlist.Select(x => x.EntityModelId).ToArray();


                model.DataAction = DataActionEnum.Edit;
                return View("UserEntityPermission", model);
            }
            else
            {


                model.DataAction = DataActionEnum.Create;
                return View("UserEntityPermission", model);
            }

        }
        [HttpPost]
        public async Task<ActionResult> ManageUserEntityPermission(UserEntityPermissionViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.PermissionIds.IsNotNullAndNotEmpty())
                {
                    var permids = model.PermissionIds.Split(",");
                    var list = new List<UserEntityPermissionViewModel>();
                    if (model.UserEntityType == UserEntityTypeEnum.User)
                    {
                        foreach (var pid in permids)
                        {
                            if (pid.IsNotNullAndNotEmpty())
                            {
                                var ids = pid.ToString().Split("_");
                                var permissionId = ids[0];
                                var pageId = ids[1];
                                //bool ischecked = ids[3].ToSafeBool();
                                if (permissionId != "Portal" && permissionId != "Page")
                                {
                                    var page = new UserEntityPermissionViewModel { PageId = pageId, PermissionIds = permissionId };
                                    list.Add(page);
                                    //await ManageUserPermission(model.UserEntityId, pageId, permissionId, true);
                                }
                            }
                        }
                        if (list.Count > 0)
                        {
                            var pages = list.DistinctBy(x => x.PageId).Select(x => x.PageId);
                            foreach (var pg in pages)
                            {
                                var pgPer = list.Where(x => x.PageId == pg).Select(x => x.PermissionIds);
                                var perpg = await _permissionBusiness.GetList();
                                var persss = perpg.Where(x => pgPer.Contains(x.Id)).Select(x => x.Code);
                                var per = persss.ToArray();
                                await ManageUserPermission(model.UserEntityId, pg, per, true);
                            }
                        }

                    }
                    else if (model.UserEntityType == UserEntityTypeEnum.UserRole)
                    {
                        foreach (var pid in permids)
                        {
                            if (pid.IsNotNullAndNotEmpty())
                            {
                                var ids = pid.ToString().Split("_");
                                var permissionId = ids[0];
                                var pageId = ids[1];
                                //bool ischecked = ids[3].ToSafeBool();
                                if (permissionId != "Portal" && permissionId != "Page")
                                {
                                    var page = new UserEntityPermissionViewModel { PageId = pageId, PermissionIds = permissionId };
                                    list.Add(page);
                                    //await ManageUserRolePermission(model.UserEntityId, pageId, permissionId, true);
                                }
                            }
                        }
                        if (list.Count > 0)
                        {
                            var pages = list.DistinctBy(x => x.PageId).Select(x => x.PageId);
                            foreach (var pg in pages)
                            {
                                var pgPer = list.Where(x => x.PageId == pg).Select(x => x.PermissionIds);
                                var perpg = await _permissionBusiness.GetList();
                                var persss = perpg.Where(x => pgPer.Contains(x.Id)).Select(x => x.Code);
                                var per = persss.ToArray();
                                await ManageUserRolePermission(model.UserEntityId, pg, per, true);
                            }
                        }
                    }
                }
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userEntityPermissionBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _userEntityPermissionBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("UserEntityPermission", model);
        }

        [HttpGet]
        public async Task<ActionResult> GetLegalEntitiesList(string userId)
        {
            var data = new List<LegalEntityViewModel>();
            if (userId.IsNotNullAndNotEmpty())
            {
                var userdetails = await _userBusiness.GetSingleById(userId);
                var legalentity = "'" + string.Join("','", userdetails.LegalEntityIds) + "'";
                data = await _userBusiness.GetEntityByIds(legalentity);
            }
            else
            {
                data = await _legalEntityBusiness.GetList();
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetPortalList(string userId)
        {
            var data = await _userBusiness.GetAllowedPortalList(userId);
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetPortalListByEmail(string email)
        {
            var userid = await _userBusiness.GetSingle(x => x.Email == email.Trim());
            var data = await _userBusiness.GetAllowedPortalList(userid.Id);
            return Json(data);
        }
        public async Task<ActionResult> GetTeamUsersList([DataSourceRequest] DataSourceRequest request, string selectedValues)
        {

            List<UserViewModel> list = new List<UserViewModel>();
            if (selectedValues.IsNotNullAndNotEmpty())
            {
                var selectedValueslist = selectedValues.Split(",");
                if (selectedValueslist.Length > 0)
                {
                    foreach (var id in selectedValueslist)
                    {
                        var user = await _userBusiness.GetSingleById(id);
                        list.Add(user);
                    }
                }
            }
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }
        public async Task<IActionResult> ViewUserPermission(string id, string userId)
        {
            ViewBag.UserId = userId;
            var userDetails = await _userBusiness.GetSingleById(userId);
            ViewBag.UserName = userDetails.Name;
            return View();
        }
        public async Task<ActionResult> ReadUserPermissionData([DataSourceRequest] DataSourceRequest request, string userId)
        {
            var data = await _userBusiness.ViewUserPermissions(userId);

            var dsResult = data;
            //var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterFaceDecriptors(IdNameViewModel model)
        {
            var userDetails = await _userBusiness.GetSingleById(model.Id);
            if (userDetails.IsNotNull())
            {
                userDetails.FaceDetectionDescriptors = model.Code;
                userDetails.Password = userDetails.Password;
                userDetails.ConfirmPassword = userDetails.Password;
                var data = await _userBusiness.Edit(userDetails);
                return Json(data);
            }
            return Json(model);
        }

        public async Task<IActionResult> GetFaceDataModel()
        {
            var model = await _userBusiness.GetList(x => x.FaceDetectionDescriptors != null);
            if (model.IsNotNull())
            {
                var result = model.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.FaceDetectionDescriptors });
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }
        public async Task<IActionResult> GetPermissionlist(string userId, string pageName, string perName)
        {
            var list = new List<IdNameViewModel>();
            var result = await _userPermissionBusiness.GetList(x => x.UserId == userId);
            foreach (var item in result)
            {
                var page = await _pageBusiness.GetSingleById(item.PageId);
                foreach (var per in item.Permissions)
                {
                    if (page.IsNotNull())
                    {
                        var permission = await _permissionBusiness.GetSingle(x => x.Code == per);
                        list.Add(new IdNameViewModel { Name = page.Name + "-" + per, Id = "chk_" + permission.Id + "_" + page.Id });
                    }
                }
            }
            if (pageName.IsNotNullAndNotEmpty() && perName.IsNotNullAndNotEmpty())
            {
                list.Add(new IdNameViewModel { Name = pageName + "-" + perName });
            }
            // var model = result.ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetUserRoleList(string id)
        {
            var list = await _userRoleBusiness.GetUserRoleForUser(id);
            //foreach (var i in list)
            //{
            //    i.UserRole = String.Join(",", i.UserRoles);
            //}

            return Json(list);
        }

        [HttpGet]
        public async Task<ActionResult> ReadUserPreferencePageData(string userId)
        {
            var userdata = await _userBusiness.GetSingleById(userId);
            var userPortals = await _userPortalBusiness.GetPortalByUser(userId);
            var portallist = await _portalBusiness.GetPortals(null);
            var userpreflist = await _userPreferenceBusiness.GetList(x => x.UserId == userId);
            if (userPortals.IsNotNull())
            {
                foreach (var up in userPortals)
                {
                    //member.Portal.Add(up.PortalId);
                    if (userpreflist.Any(x => x.PreferencePortalId == up.PortalId))
                    {

                    }
                    else
                    {
                        var portal = portallist.Where(x => x.Id == up.PortalId);
                        userpreflist.Add(new UserPreferenceViewModel { UserId = userId, UserName = userdata.Name, PreferencePortalId = up.PortalId, PreferencePortalName = portal.FirstOrDefault().Name });
                    }
                }
            }
            foreach (var pref in userpreflist)
            {
                var pagelist = await _pageBusiness.GetList(x => x.PortalId == pref.PreferencePortalId);
                pref.PageList = pagelist;
            }



            //var model = new List<UserPreferenceViewModel>();
            //model.Add(new UserPreferenceViewModel { PreferencePortalName = "Test01" });
            //model.Add(new UserPreferenceViewModel { PreferencePortalName = "Test02" });

            return Json(userpreflist);
        }


        [HttpGet]
        public async Task<ActionResult> ReadPortalByCompanyIdOld()
        {
            var portals = await _userBusiness.GetSingle<CompanyViewModel, Company>(x => x.Id == _userContext.CompanyId);
            var licensedPortalIds =  portals.LicensedPortalIds.ToList();
            var portalIds = await _portalBusiness.GetList(x => licensedPortalIds.Any(y => y == x.Id));
            var userpreflist =new List<UserPreferenceViewModel>();
            foreach (var pref in portalIds.Select((value, i) => new { i, value }))
            {
               // userpreflist.Add(new UserPreferenceViewModel {  PreferencePortalId = pref.value.Id, PreferencePortalName = pref.value.Name });
                var pagelist = await _pageBusiness.GetList(x => x.PortalId == pref.value.Id);
               // pref.PageList = pagelist;
                userpreflist.Add(new UserPreferenceViewModel { PreferencePortalId = pref.value.Id, PreferencePortalName = pref.value.Name ,PageList=pagelist});
            }


            return Json(userpreflist);
        }


        public JsonResult GetScheduleList()
        {
            var ScheduleList = new List<ScheduleInfo>();
            var schedule = new ScheduleInfo();

            schedule.id = "aksdjsk-jdsnjsdn-sjndjsnj-sndnsb";
            schedule.calendarId = "abcd";

            schedule.title = "dksjdkjksjd";
            schedule.body = "kjfkjd";
            schedule.isReadOnly = false;

            schedule.isPrivate = false;
            schedule.location = "Bhopal";
            schedule.state = "Free";
            schedule.color = "#fffffff";
            schedule.bgColor = "#fffffff";
            schedule.dragBgColor = "#fffffff";
            schedule.borderColor = "#fffffff";

            if (schedule.category == "milestone")
            {
                schedule.color = schedule.bgColor;
                schedule.bgColor = "transparent";
                schedule.dragBgColor = "transparent";
                schedule.borderColor = "transparent";
            }

            schedule.raw.memo = "AJsj ndjjsd hdjsd";
            schedule.raw.creator.name = "Sumbul";
            schedule.raw.creator.avatar = "dskjdk";
            schedule.raw.creator.company = "company";
            schedule.raw.creator.email = "email@gmail.com";
            schedule.raw.creator.phone = "k9e8r98989";



            var startDate = new DateTime();
            var endDate = new DateTime().AddDays(30);
            schedule.isAllday = true;
            schedule.category = "allday";
            schedule.dueDateClass = "morning";
            schedule.start = DateTime.Now.ToString();
            schedule.end = DateTime.Now.AddDays(2).ToString();
            ScheduleList.Add(schedule);
            return Json(ScheduleList);
        }

        [HttpGet]
        public async Task<ActionResult> ReadPortalByCompanyId()
        {
            var portals = await _userBusiness.GetSingle<CompanyViewModel, Company>(x => x.Id == _userContext.CompanyId);
            var licensedPortalIds = portals.LicensedPortalIds.ToList();
            var portalIds = await _portalBusiness.GetList(x => licensedPortalIds.Any(y => y == x.Id));

            return Json(portalIds);
        }

        [HttpGet]
        public async Task<ActionResult> ReadPageByPortalId(string portalId)
        {

            var pages = await _pageBusiness.GetList(x => x.PortalId == portalId);
            return Json(pages);
        }

        public async Task<ActionResult> GetUserPreferences(string userId)
        {
            var model = await _userBusiness.GetUserPreferences(userId);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserPreferences(string userId, string portalId, string pageId, DataActionEnum mode)
        {
            if (mode == DataActionEnum.Create)
            {
                var exist = await _userPreferenceBusiness.GetSingle(x => x.UserId == userId && x.PreferencePortalId == portalId);
                if (exist.IsNotNull())
                {
                    return Json(new { success = false, msg = "User Preference for the Portal already Exist" });
                }
                else
                {
                    var model = new UserPreferenceViewModel()
                    {
                        UserId = userId,
                        PreferencePortalId = portalId,
                        DefaultLandingPageId = pageId,
                        DataAction = DataActionEnum.Create,
                    };
                    var result = await _userPreferenceBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false });
                }
            }


            else
            {
                var exist = await _userPreferenceBusiness.GetSingle(x => x.UserId == userId && x.PreferencePortalId == portalId);
                exist.DefaultLandingPageId = pageId;
                exist.DataAction = DataActionEnum.Edit;
                var result = await _userPreferenceBusiness.Edit(exist);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }

        }

        public async Task<IActionResult> DeleteUserPreference(string Id)
        {
            await _userPreferenceBusiness.Delete(Id);
            return Json(true);
        }

    }
}
