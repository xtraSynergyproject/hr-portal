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

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class UserController : ApplicationController
    {
        private IUserBusiness _userBusiness;
        private IPageBusiness _pageBusiness;
        private ITeamBusiness _teamBusiness;
        private ITeamUserBusiness _teamUserBusiness;
        private ICandidateProfileBusiness _candidateProfileBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private IPortalBusiness _portalBusiness;
        private IUserPortalBusiness _userPortalBusiness;
        private IUserPermissionBusiness _userPermissionBusiness;
        private IUserRolePermissionBusiness _userRolePermissionBusiness;
        private IPermissionBusiness _permissionBusiness;
        private static IServiceProvider _services;
        private static IUserContext _userContext;
        private readonly IConfiguration _configuration;
        private IUserHierarchyPermissionBusiness _userHierarchyPermissionBusiness;
        private IUserEntityPermissionBusiness _userEntityPermissionBusiness;
        private ILegalEntityBusiness _legalEntityBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        private ICmsBusiness _cmsBusiness;
        private IUserGroupBusiness _userGroupBusiness;
        private IUserGroupUserBusiness _userGroupUserBusiness;
        private readonly IHttpContextAccessor _accessor;
        private readonly ICompanyBusiness _companyBusiness;
        public UserController(IUserBusiness userBusiness, ITeamBusiness teamBusiness, ITeamUserBusiness teamUserBusiness,
            ICandidateProfileBusiness candidateProfileBusiness, ILegalEntityBusiness legalEntityBusiness, IUserGroupBusiness userGroupBusiness,
                      AuthSignInManager<ApplicationIdentityUser> customUserManager, ICmsBusiness cmsBusiness
            , IPageBusiness pageBusiness, IUserRoleUserBusiness userRoleUserBusiness, IUserGroupUserBusiness userGroupUserBusiness,
                      IPortalBusiness portalBusiness, IUserPortalBusiness userPortalBusiness,
            IUserPermissionBusiness userPermissionBusiness, IPermissionBusiness permissionBusiness, IUserRolePermissionBusiness userRolePermissionBusiness
            , IServiceProvider services, IConfiguration configuration, IUserHierarchyPermissionBusiness userHierarchyPermissionBusiness
            , IUserEntityPermissionBusiness userEntityPermissionBusiness, IUserContext userContext, IHttpContextAccessor accessor,
            ICompanyBusiness companyBusiness)
        {
            _accessor = accessor;
            _userBusiness = userBusiness;
            _userRolePermissionBusiness = userRolePermissionBusiness;
            _cmsBusiness = cmsBusiness;
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
            _userContext = userContext;
            _userGroupBusiness = userGroupBusiness;
            _userGroupUserBusiness = userGroupUserBusiness;
            _companyBusiness = companyBusiness;
        }
        public IActionResult Index(string portalId)
        {
            var model = new UserViewModel { PortalId = portalId };

            return View(model);
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

            // var model = await _userBusiness.GetAllUsersWithPortalId(_userContext.PortalId);
            var model = await _userBusiness.GetUsersWithPortalIds();
            return Json(model);
        }

        public async Task<IActionResult> CreateUser(string portalId)
        {

            var portal = await _userBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            // string[] LegalEntityIds = { _userContext.LegalEntityId };

            var model = new UserViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                // Password = "123",
                // ConfirmPassword = "123",
                LegalEntityId = _userContext.LegalEntityId,
                PortalId = _userContext.PortalId,

                Portal = new List<string> { _userContext.PortalId },

                PortalName = portal.IsNotNull() ? portal.Name : ""
                //LegalEntityIds = LegalEntityIds
            };


            return View("ManageUser", model);


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

        public async Task<IActionResult> EditUser(string Id, string portalId)
        {
            var portal = await _userBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            var member = await _userBusiness.GetSingleById(Id);
            //if (member.IsSystemAdmin)
            //{
            var userPortals = await _userPortalBusiness.GetPortalByUser(Id);
            member.Portal = new List<string>();
            if (userPortals.IsNotNull())
            {
                foreach (var up in userPortals)
                {
                    member.Portal.Add(up.PortalId);
                }
            }
            //}
            //else { member.Portal = new List<string> { portalId }; }

            if (member != null)
            {
                member.ConfirmPassword = member.Password;
                member.DataAction = DataActionEnum.Edit;
                member.PortalName = portal.IsNotNull() ? portal.Name : "";
                return View("ManageUser", member);
            }
            return View("ManageUser", new UserViewModel());
        }



        [HttpPost]
        public async Task<IActionResult> ManagePortalUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //  model.PortalId = _userContext.PortalId;
                //  model.LegalEntityId = _userContext.LegalEntityId;
                if (model.DataAction == DataActionEnum.Create)
                {
                    string[] LegalEntityIds = { _userContext.LegalEntityId };
                    model.LegalEntityIds = LegalEntityIds;
                    //model.Portal = new List<string>() { model.PortalId };
                    var result = await _userBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //Add UserPortal data
                        if (model.Portals.IsNotNull())
                        {
                            foreach (var p in model.Portals)
                            {
                                var res = await _userPortalBusiness.Create(new UserPortalViewModel
                                {
                                    UserId = result.Item.Id,
                                    PortalId = p,
                                });
                            }
                        }
                        return Json(new { success = true, result = result.Item });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var existingItem = await _userBusiness.GetSingleById(model.Id);
                    existingItem.Name = model.Name;
                    existingItem.JobTitle = model.JobTitle;
                    existingItem.Email = model.Email;
                    //existingItem.Password = model.Password;
                    // existingItem.ConfirmPassword = model.ConfirmPassword;
                    model.Password = existingItem.Password;
                    model.ConfirmPassword = existingItem.Password;
                    if (!existingItem.LegalEntityIds.Contains(_userContext.LegalEntityId))
                    {
                        string[] LegalEntityIds = { _userContext.LegalEntityId };
                        model.LegalEntityIds = LegalEntityIds;
                    }

                    var result = await _userBusiness.Edit(model);
                    if (result.IsSuccess)
                    {

                        var portals = await _userPortalBusiness.GetPortalByUser(model.Id);
                        //var list = portals.Select(x => x.PortalId).ToList();
                        //model.Portal = new List<string>();
                        //model.Portal.AddRange(list);
                        if (portals.IsNotNull())
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
                        return Json(new { success = true, result = result.Item });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpGet]
        public async Task<IActionResult> GetUserIdNameList(string viewData = null)
        {
            var data1 = await _userBusiness.GetUsersWithPortalIds();
            //var data = await _userBusiness.GetUserIdNameList();
            var data = data1.Select(x => new IdNameViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
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
            //return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetUsersListOnTypeBasis([DataSourceRequest] DataSourceRequest request, string assigneeType, string teamId)
        {
            var list = new List<UserViewModel>();
            if (assigneeType == "TASK_ASSIGN_TO_USER" || assigneeType == "1")
            {
                list = await _userBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            }
            else
            {
                if (teamId.IsNotNullAndNotEmpty())
                {
                    var team = await _teamBusiness.GetSingleById(teamId);
                    var teamUsers = await _teamUserBusiness.GetList(x => x.TeamId == teamId && x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
                    foreach (var id in teamUsers)
                    {
                        var user = await _userBusiness.GetSingleById(id.UserId);
                        list.Add(user);
                    }
                }
            }
            return Json(list);
        }
        public async Task<ActionResult> GetUsersIdNameList()
        {
            List<UserViewModel> list = new List<UserViewModel>();
            //   list = await _userBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);

            list = await _userBusiness.GetDMSPermiisionusersList(_userContext.PortalId, _userContext.LegalEntityId);
            return Json(list);
        }
        [HttpGet]
        public async Task<JsonResult> GetPortalForUser(string id)
        {
            var data = await _portalBusiness.GetPortalForUser(id);
            return Json(data);
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
            var company = await _companyBusiness.GetSingleById(_userContext.CompanyId);
            string[] portalIds = company.LicensedPortalIds.ToArray();
            var model = result.Where(x => portalIds.Contains(x.PortalId)).ToList();
            //var model = result.Where(x => x.PortalId == _userContext.PortalId).ToList();
            return Json(model);
        }

        public async Task<object> GetPortalFancyTreeList(string id, string userId)
        {
            var result = await _pageBusiness.GetPortalTreeList(id, userId);
            var company = await _companyBusiness.GetSingleById(_userContext.CompanyId);
            string[] portalIds = company.LicensedPortalIds.ToArray();
            result = result.Where(x => portalIds.Contains(x.PortalId)).ToList();

            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel
            {
                key = x.id,
                title = x.Name,
                lazy = true,
                FieldDataType = x.FieldDataType.ToString(),
                ParentId = id,
                checkbox = x.HideCheckbox,
            }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
        }

        public async Task<IActionResult> GetPortalRoleTreeList(string id, string userId)
        {
            var result = await _pageBusiness.GetPortalRoleTreeList(id, userId);
            var model = result.Where(x => x.PortalId == _userContext.PortalId).ToList();
            return Json(model);
        }

        public async Task<object> GetPortalRoleFancyTreeList(string id, string userId)
        {
            var result = await _pageBusiness.GetPortalRoleTreeList(id, userId);
            var company = await _companyBusiness.GetSingleById(_userContext.CompanyId);
            string[] portalIds = company.LicensedPortalIds.ToArray();
            result = result.Where(x => portalIds.Contains(x.PortalId)).ToList();

            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel
            {
                key = x.id,
                title = x.Name,
                lazy = true,
                FieldDataType = x.FieldDataType.ToString(),
                ParentId = id,
                checkbox = x.HideCheckbox,
            }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
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
                    var permission = await _permissionBusiness.GetSingle(x => x.Code == per);
                    list.Add(new IdNameViewModel { Name = page.Name + "-" + per, Id = "chk_" + permission.Id + "_" + page.Id });
                }
            }
            if (pageName.IsNotNullAndNotEmpty() && perName.IsNotNullAndNotEmpty())
            {
                list.Add(new IdNameViewModel { Name = pageName + "-" + perName });
            }
            // var model = result.ToList();
            return Json(list);
        }
        [HttpPost]
        public async Task<JsonResult> GetNewPermissionlist(string userId,string permission)
        {
            var list = new List<UserEntityPermissionViewModel>();
            var userrole =await _userRoleUserBusiness.GetList(x=>x.UserId==userId);
            var ids = userrole.Select(x => x.Id);
            var id = string.Join("','", ids);
            var userper = await _userPermissionBusiness.GetUserPermissionList(userId, id, "permission");
            var listview = "";
            if (permission.IsNotNullAndNotEmpty())
            {
                permission = permission.Replace("[", "");
                permission = permission.Replace("]", "");
                permission = permission.Replace("\"", "");
                var permids = permission.Split("#");
                var pages = await _pageBusiness.PortalDetails();
                var permiss = await _permissionBusiness.GetList();
                foreach (var pid in permids)
                {
                    if (pid.IsNotNullAndNotEmpty())
                    {
                        var pids = pid.ToString().Split("_");
                        var permissionId = pids[0];
                        var pageId = pids[1];
                        if (permissionId != "Portal" && permissionId != "Page")
                        {
                            var p = pages.Where(x => x.Id == pageId).FirstOrDefault();
                            var perm = permiss.Where(x => x.Id == permissionId).FirstOrDefault();
                            var data = new UserEntityPermissionViewModel
                            {
                                PortalId = p.PortalId,
                                Portal = p.PortalName,
                                Page = p.Title,
                                PageId = p.Id,
                                Permissions = new string[] { perm.Name }
                            };
                            list.Add(data);
                        }
                    }


                }
            }
            var result = (from l1 in userper
                                  select l1)
                      .Union(list).ToList();

            var mod = result.DistinctBy(x => x.PortalId).Select(x => new UserEntityPermissionViewModel { PortalId = x.PortalId, Portal = x.Portal }).ToList();

           
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


            return Json(listview);
        }
        [HttpPost]
        public async Task<JsonResult> GetNewRolePermissionlist(string permission,string userRoleId)
        {
            var list = new List<UserEntityPermissionViewModel>();
            var listview = "";
            var existPerm = await _userRolePermissionBusiness.GetUserRolePermissionList(userRoleId);
            if (permission.IsNotNullAndNotEmpty())
            {
                permission = permission.Replace("[", "");
                permission = permission.Replace("]", "");
                permission = permission.Replace("\"", "");
                var permids = permission.Split("#");
                var pages = await _pageBusiness.PortalDetails();
                var permiss = await _permissionBusiness.GetList();
                foreach (var pid in permids)
                {
                    if (pid.IsNotNullAndNotEmpty())
                    {
                        var pids = pid.ToString().Split("_");
                        var permissionId = pids[0];
                        var pageId = pids[1];
                        if (permissionId != "Portal" && permissionId != "Page")
                        {
                            var p = pages.Where(x => x.Id == pageId).FirstOrDefault();
                            var perm = permiss.Where(x => x.Id == permissionId).FirstOrDefault();
                            var data = new UserEntityPermissionViewModel
                            {
                                PortalId = p.PortalId,
                                Portal = p.PortalName,
                                Page = p.Title,
                                PageId = p.Id,
                                Permissions = new string[] { perm.Name }
                            };
                            list.Add(data);
                        }
                    }


                }
            }
            var result = (from l1 in existPerm
                          select l1)
                     .Union(list).ToList();
            var mod = result.DistinctBy(x => x.PortalId).Select(x => new UserEntityPermissionViewModel { PortalId = x.PortalId, Portal = x.Portal }).ToList();


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


            return Json(listview);
        }

        public async Task<IActionResult> ManageUserRolePermission(string userRoleId, string pageId, string[] permissionId, bool isChecked)
        {
            var userPermission = await _userRolePermissionBusiness.GetSingle(x => x.UserRoleId == userRoleId && x.PageId == pageId);
            //var permission = await _permissionBusiness.GetSingleById(permissionId);
            if (userPermission.IsNotNull() /*&& permission.IsNotNull()*/)
            {
                //if (isChecked)
                //{
                userPermission.Permissions = permissionId;//AddPermission(userPermission.Permissions, permission.Code);
                //}
                //else
                //{
                //    userPermission.Permissions = userPermission.Permissions.RemoveItemFromArray(permission.Code);
                //}
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
            if (userPermission.IsNotNull())
            {
                //if (isChecked)
                //{
                    userPermission.Permissions = permissionId;//AddPermission(userPermission.Permissions, permission.Code);
                //}
                //else
                //{
                //    userPermission.Permissions = userPermission.Permissions.RemoveItemFromArray(permission.Code);
                //}
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
                model.Permissions = permissionId;//AddPermission(array, permission.Code);
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;
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
            var model = await _userBusiness.GetUserListForPortal();
            var data = model.ToList();

            // var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }

        public async Task<ActionResult> ReadUserTeamData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _userBusiness.GetUserTeamListForPortal();
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
        private async Task<IList<UserViewModel>> GetUserList(DataSourceRequest request = null)
        {
            dynamic searchParam = null;
            if (request != null && request.Filters.Count > 0)
            {
                //searchParam = Convert.ToString(((Kendo.Mvc.FilterDescriptor)request.Filters.FirstOrDefault()).ConvertedValue);
                searchParam = request.Filters.FirstOrDefault();
                searchParam = searchParam.ConvertedValue;
            }
            var data = await _userBusiness.GetActiveUserListForPortal();
            return data;
        }
        public async Task<ActionResult> GetUserListVirtualData([DataSourceRequest] DataSourceRequest request)
        {
            var data = await GetUserList(request);
            request.Filters.Clear();
            return Json(data);
            //return Json(data.ToDataSourceResult(request));
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
                member.PortalId = _userContext.PortalId;
                member.LegalEntityId = _userContext.LegalEntityId;

                member.DataAction = DataActionEnum.Edit;
                return View("ManageUserHierarchyPermission", member);
            }
            else
            {
                var model = new UserHierarchyPermissionViewModel();
                model.UserId = userId;
                model.DataAction = DataActionEnum.Create;
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;
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
        public async Task<ActionResult> ReadUserHierarchyPermission(string userId)
        {

            var list = await _userHierarchyPermissionBusiness.GetUserPermissionHierarchyForPortal(userId);
            var json = Json(list);
            return json;
        }


        public async Task<IActionResult> DeleteUserHierarchyPermission(string Id)
        {
            await _userHierarchyPermissionBusiness.Delete(Id);

            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> GetHierarchyIdNameList()
        {
            var dataList = await _userHierarchyPermissionBusiness.GetListGlobal<HierarchyMasterViewModel, HierarchyMaster>(x => x.PortalId == _userContext.PortalId && x.LegalEntityId == _userContext.LegalEntityId);
            return Json(dataList.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList());
        }
        //public async Task<IActionResult> UserEntityPermission(UserEntityPermissionViewModel model)
        //{


        //    return View("UserHierarchyPermission", model);
        //}

        public async Task<ActionResult> ReadWorkspaceList()
        {
            //var list = await _pmtBusiness.GetGradeList();
            var list = await _userGroupBusiness.GetList(x => x.PortalId == _userContext.PortalId);
            return Json(list);
        }

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
            model.LegalEntityId = _userContext.LegalEntityId;
            model.PortalId = _userContext.PortalId;
            //var portal = await _userBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            //if (portal.IsNotNull())
            //{
            //    ViewBag.Portal = portal.Name;
            //}
            var EList = await _userEntityPermissionBusiness.GetList(x => x.UserEntityId == id && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.LegalEntity && x.PortalId == _userContext.PortalId && x.LegalEntityId == _userContext.LegalEntityId);
            var orglist = await _userEntityPermissionBusiness.GetList(x => x.UserEntityId == id && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Organization /*&& x.PortalId == _userContext.PortalId && x.LegalEntityId == _userContext.LegalEntityId*/);
            var tlist = await _userEntityPermissionBusiness.GetList(x => x.UserEntityId == id && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Template /*&& x.PortalId == _userContext.PortalId && x.LegalEntityId == _userContext.LegalEntityId*/);
            var usergroupuser = await _userGroupUserBusiness.GetList(x => x.UserId == id && x.PortalId == _userContext.PortalId && x.IsDeleted == false);
            var usergroup = await _userGroupBusiness.GetList(x => x.IsDeleted == false && x.PortalId == _userContext.PortalId);
            var wlist = from a in usergroupuser
                        join b in usergroup
                        on a.UserGroupId equals b.Id
                        select new IdNameViewModel
                        {
                            Id = b.Id,
                            Name = b.Name
                        };
            if (EList.IsNotNull() && orglist.IsNotNull() && tlist.IsNotNull())
            {

                if (EList.Count() == 0)
                {
                    // model.LegalEntityId1[0] = (_userContext.LegalEntityId);
                    string[] LegalEntityIds = new string[1];
                    LegalEntityIds[0] = _userContext.LegalEntityId;
                    model.LegalEntityId1 = LegalEntityIds;
                }
                else
                {
                    model.LegalEntityId1 = EList.Select(x => x.EntityModelId).ToArray();
                }

                model.OrganisationId = orglist.Select(x => x.EntityModelId).ToArray();
                model.TemplateId = tlist.Select(x => x.EntityModelId).ToArray();
                if (wlist.IsNotNull())
                {
                    model.WorkspaceId = wlist.Select(x => x.Id).ToArray();
                }


                model.DataAction = DataActionEnum.Edit;
                return View("UserEntityPermission", model);
            }
            else
            {

                // model.LegalEntityId1[0] = (_userContext.LegalEntityId);
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
                else
                {
                    if (model.UserEntityType == UserEntityTypeEnum.User)
                    {
                        var userPermission = await _userPermissionBusiness.GetList(x => x.UserId == model.UserEntityId);
                        foreach (var item in userPermission)
                        {
                            await _userPermissionBusiness.Delete(item.Id);
                        }
                    }
                    else if (model.UserEntityType == UserEntityTypeEnum.UserRole)
                    {
                        var userPermission = await _userRolePermissionBusiness.GetList(x => x.UserRoleId == model.UserEntityId);
                        foreach (var item in userPermission)
                        {
                            await _userRolePermissionBusiness.Delete(item.Id);
                        }
                    }
                }
                if (model.UserEntityType == UserEntityTypeEnum.User && model.WorkspaceId.IsNotNull())
                {
                    var existinglist = await _userGroupUserBusiness.GetList(x => x.UserId == model.UserEntityId);
                    if (existinglist.Count > 0)
                    {
                        var existing = existinglist.Select(x => x.UserGroupId);
                        var newids = model.WorkspaceId;
                        var ToDelete = existing.Except(newids).ToList();
                        var ToAdd = newids.Except(existing).ToList();
                        foreach (var wid in ToAdd)
                        {
                            var usergroupuser = new UserGroupUserViewModel
                            {
                                UserId = model.UserEntityId,
                                UserGroupId = wid
                            };

                            var res = await _userGroupUserBusiness.Create(usergroupuser);
                        }
                        foreach (var wid in ToDelete)
                        {
                            var data = await _userGroupUserBusiness.GetSingle(x => x.UserGroupId == wid && x.UserId == model.UserEntityId);
                            await _userGroupUserBusiness.Delete(data.Id);
                        }
                    }
                    else
                    {
                        foreach (var wid in model.WorkspaceId)
                        {
                            var usergroupuser = new UserGroupUserViewModel
                            {
                                UserId = model.UserEntityId,
                                UserGroupId = wid
                            };

                            var res = await _userGroupUserBusiness.Create(usergroupuser);
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
        public async Task<IActionResult> ViewUserPermission(string id, string userId)
        {
            ViewBag.UserId = userId;
            var userDetails = await _userBusiness.GetSingleById(userId);
            ViewBag.UserName = userDetails.Name;
            return View();
        }
        public async Task<ActionResult> ReadUserPermissionData(string userId)
        {
            var data = await _userBusiness.ViewUserPermissions(userId);


            return Json(data);
        }

    }
}
