using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CMS.Business;
using Microsoft.AspNetCore.Mvc;
using CMS.UI.ViewModel;
using CMS.Common;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Kendo.Mvc.UI;
using CMS.Data.Model;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.CMS.Controllers


{
    //[Authorize]
    [Area("Cms")]

    public class ContentController : ApplicationController
    {
        private readonly IPageBusiness _pageBusiness;
        private readonly IPageContentBusiness _pageContentBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IStyleBusiness _styleBusiness;
        private readonly IDocumentBusiness _documentBusiness;
        private readonly IUserPermissionBusiness _userPermissionBusiness;
        private readonly IUserRolePermissionBusiness _userRolePermissionBusiness;
        private readonly IPermissionBusiness _permissionBusiness;
        private readonly IPageIndexBusiness _pageIndexBusiness;
        private readonly IUserContext _userContext;
        private readonly IUserDataPermissionBusiness _userDataPermissionBusiness;
        private readonly IUserRoleDataPermissionBusiness _userRoleDataPermissionBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IMenuGroupBusiness _menuGroupBusiness;
        private readonly ISubModuleBusiness _subModuleBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IColumnMetadataBusiness _columnMetadataBusiness;
        private readonly IPageDetailsBusiness _PageDetailsBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public ContentController(IPageBusiness pageBusiness, IPortalBusiness portalBusiness, IStyleBusiness styleBusiness
            , IDocumentBusiness documentBusiness, IPageContentBusiness pageContentBusiness, IUserPermissionBusiness userPermissionBusiness
            , IUserRolePermissionBusiness userRolePermissionBusiness
            , IPermissionBusiness permissionBusiness, IPageIndexBusiness pageIndexBusiness
            , IUserContext userContext, IUserDataPermissionBusiness userDataPermissionBusiness,
            IUserRoleDataPermissionBusiness userRoleDataPermissionBusiness, IUserBusiness userBusiness, IUserRoleBusiness userRoleBusiness,
            IMenuGroupBusiness menuGroupBusiness, ISubModuleBusiness subModuleBusiness, ITableMetadataBusiness tableMetadataBusiness
            , IColumnMetadataBusiness columnMetadataBusiness, ICmsBusiness cmsBusiness, IPageDetailsBusiness PageDetailsBusiness
            , AuthSignInManager<ApplicationIdentityUser> customUserManager)
        {
            _customUserManager = customUserManager;
            _pageBusiness = pageBusiness;
            _portalBusiness = portalBusiness;
            _styleBusiness = styleBusiness;
            _documentBusiness = documentBusiness;
            _pageContentBusiness = pageContentBusiness;
            _userPermissionBusiness = userPermissionBusiness;
            _userRolePermissionBusiness = userRolePermissionBusiness;
            _permissionBusiness = permissionBusiness;
            _pageIndexBusiness = pageIndexBusiness;
            _userContext = userContext;
            _userDataPermissionBusiness = userDataPermissionBusiness;
            _userRoleDataPermissionBusiness = userRoleDataPermissionBusiness;
            _userBusiness = userBusiness;
            _userRoleBusiness = userRoleBusiness;
            _menuGroupBusiness = menuGroupBusiness;
            _subModuleBusiness = subModuleBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _cmsBusiness = cmsBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _PageDetailsBusiness = PageDetailsBusiness;
        }
        [Authorize]
        [Authorize(Policy = nameof(AuthorizeCMS))]
        public async Task<IActionResult> Index()
        {
            await CleanContext();
            var model = new DocumentTypeViewModel();
            return View(model);
        }
        public async Task CleanContext()
        {

            var id = new ApplicationIdentityUser
            {
                Id = _userContext.UserId,
                UserName = _userContext.Name,
                IsSystemAdmin = _userContext.IsSystemAdmin,
                Email = _userContext.Email,
                UserUniqueId = _userContext.Email,
                CompanyId = _userContext.CompanyId,
                CompanyCode = _userContext.CompanyCode,
                CompanyName = _userContext.CompanyName,
                JobTitle = _userContext.JobTitle,
                PhotoId = _userContext.PhotoId,
                UserRoleCodes = _userContext.UserRoleCodes,
                UserRoleIds = _userContext.UserRoleIds,
                IsGuestUser = _userContext.IsGuestUser,
                UserPortals = _userContext.UserPortals,
                PortalTheme = _userContext.PortalTheme,
                PortalId = null,
                LegalEntityId = _userContext.LegalEntityId,
                LegalEntityCode = _userContext.LegalEntityCode,
                PersonId = _userContext.PersonId,
                PositionId = _userContext.PositionId,
                DepartmentId = _userContext.OrganizationId,
                PortalName = _userContext.PortalName,
            };
            id.MapClaims();
            await _customUserManager.SignInAsync(id, true);

        }
        public async Task<IActionResult> ManageActionPermissions(string pageId, TemplateTypeEnum pageType, string portalId)
        {
            var model = new UserPermissionViewModel { PageId = pageId, PageType = pageType, PortalId = portalId };
            var permission = await _permissionBusiness.GetList(x => x.PageTypes.Contains(pageType.ToString()) && x.UserPermissionTypes.Contains(UserPermissionTypeEnum.User.ToString()));
            //var permission = await _permissionBusiness.GetList();
            permission = permission.OrderBy(x => x.Name).ToList();
            var grp = new List<string>();
            var cols = new List<string>();
            foreach (var item in permission)
            {
                if (item.GroupName.IsNotNullAndNotEmpty())
                    grp.Add(item.GroupName);

                cols.Add(item.Name);
            }
            // model.Groups = grp.ToArray();
            model.Columns = cols.ToArray();

            //user role
            //var userrolepermission = await _permissionBusiness.GetList();
            var userrolepermission = await _permissionBusiness.GetList(x => x.PageTypes.Contains(pageType.ToString()) && x.UserPermissionTypes.Contains(UserPermissionTypeEnum.UserRole.ToString()));
            userrolepermission = userrolepermission.OrderBy(x => x.Name).ToList();

            var userrole = new List<string>();
            foreach (var item in userrolepermission)
            {
                userrole.Add(item.Name);
            }
            model.UserRoleColumns = userrole.ToArray();
            return View(model);
        }

        public async Task<IActionResult> ManagePermissions(string pageId, TemplateTypeEnum pageType, string portalId)
        {
            var model = new PermissionViewModel { PageId = pageId, PortalId = portalId };

            return View(model);
        }

        public async Task<IActionResult> AddPermissions(string pageId, string portalId)
        {
            var model = new PermissionViewModel { PageId = pageId, PortalId = portalId };
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }

        public async Task<IActionResult> EditPermission(string pageId, string id)
        {
            var model = await _permissionBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("AddPermissions", model);
        }
        [HttpPost]
        public async Task<ActionResult> DeletePermission(string id)
        {
            if (id.IsNotNull())
            {
                //create
                await _permissionBusiness.Delete(id);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> ManageAddPermissions(PermissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.permissiontype = "Permission";
                if (model.DataAction == DataActionEnum.Create)
                {

                    var result = await _permissionBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("Index");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _permissionBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            //return Json(new { success = false, errors = ModelState.SerializeErrors() });
            return View("AddPermissions", model);
        }

        public async Task<ActionResult> ReadPermission([DataSourceRequest] DataSourceRequest request, string pageId, string portalId)
        {
            var data = await _permissionBusiness.GetList(x => x.PageId == pageId);
            if (portalId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.PortalId == portalId).ToList();
            }
            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadPermissionList(string pageId, string portalId)
        {
            var data = await _permissionBusiness.GetList(x => x.PageId == pageId);
            if (portalId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.PortalId == portalId).ToList();
            }
           
            return Json(data);
        }

        public async Task<IActionResult> ManageUserDataPermissions(string pageId, TemplateTypeEnum pageType, string portalId)
        {
            var model = new UserPermissionViewModel { PageId = pageId, PageType = pageType, PortalId = portalId };
            var permission = await _permissionBusiness.GetList(x => x.PageTypes.Contains(pageType.ToString()) && x.UserPermissionTypes.Contains(UserPermissionTypeEnum.User.ToString()));
            //var permission = await _permissionBusiness.GetList();
            permission = permission.OrderBy(x => x.Name).ToList();
            var grp = new List<string>();
            var cols = new List<string>();
            foreach (var item in permission)
            {
                if (item.GroupName.IsNotNullAndNotEmpty())
                    grp.Add(item.GroupName);

                cols.Add(item.Name);
            }
            // model.Groups = grp.ToArray();
            model.Columns = cols.ToArray();

            //user role
            var userrolepermission = await _permissionBusiness.GetList();
            userrolepermission = userrolepermission.OrderBy(x => x.Name).ToList();

            var userrole = new List<string>();
            foreach (var item in userrolepermission)
            {
                userrole.Add(item.Name);
            }
            model.UserRoleColumns = userrole.ToArray();
            return View(model);
        }
        public async Task<IActionResult> CreatePage(string portalId, string parentId, string menugroupId, bool isPopup = false, string source = null, string pagePortalId = null)
        {
            if (isPopup)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = null;
            }
            PageViewModel model = new PageViewModel();
            model.DataAction = DataActionEnum.Create;
            model.PortalId = portalId;
            model.PagePortalId = pagePortalId;
            model.ParentId = parentId;
            model.Id = Guid.NewGuid().ToString();
            if (menugroupId.IsNotNullAndNotEmpty())
            {
                model.MenuGroupId = menugroupId;
                var menugroup = await _menuGroupBusiness.GetSingleById(menugroupId);
                if (menugroup != null)
                {
                    model.SubModuleId = menugroup.SubModuleId;
                    var subModule = await _subModuleBusiness.GetSingle(x => x.Id == menugroup.SubModuleId);
                    if (subModule != null)
                    {
                        model.ModuleId = subModule.ModuleId;
                    }
                }
            }
            model.RequestUrl = source;
            return View("_Page", model);
        }

        public async Task<IActionResult> EditPage(string pageId, bool isPopup = false, string source = null, string pagePortalId = null)
        {
            if (isPopup)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = null;
            }
            var pagedata = await _pageBusiness.GetPageData(pageId);
            pagedata.PagePortalId = pagePortalId;
            if (pagedata != null)
            {
                pagedata.DataAction = DataActionEnum.Edit;
                pagedata.RequestUrl = source;
                return View("_Page", pagedata);
            }
            return View("_Page", new PageViewModel());
        }

        public async Task<IActionResult> GetContentTreeList(string id)
        {
            var result = await _pageBusiness.GetContentTreeList(id);
            var model = result.OrderBy(x=>x.Name).ToList();
            return Json(model);
        }
        public IActionResult SelectPageIcon(string pageId, string iconCss, string iconColor)
        {
            var model = new PageViewModel();
            model.DataAction = DataActionEnum.Create;
            model.IconCss = iconCss;
            model.IconColor = iconColor;
            return View("ManagePageIcon", model);
        }

        public async Task<IActionResult> ManageIndex(string pageId, PageTypeEnum pageType)
        {
            var model = new PageIndexViewModel();
            model.PageId = pageId;
            model.DataAction = DataActionEnum.Create;
            return View("ManageIndex", model);
        }
        public async Task<IActionResult> ManageDetailPage(string pageId, PageTypeEnum pageType)
        {
            var model = new PageViewModel();
            model.Id = pageId;
            model.DataAction = DataActionEnum.Create;
            return View("ManageDetailPage", model);
        }
        public async Task<IActionResult> ManageGeneralTab()
        {
            var model = new PageViewModel();
            model.DataAction = DataActionEnum.Create;
            return View("ManageGeneralTab", model);
        }

        public IActionResult CreatePortal(string parentId)
        {
            var model = new PortalViewModel();
            model.DataAction = DataActionEnum.Create;
            model.BannerHeight = 65;
            model.ParentId = parentId;
            return View("ManagePortal", model);
        }

        public async Task<IActionResult> EditPortal(string id)
        {
            var model = await _portalBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            if (model.Theme != null)
            {
                model.ThemeData = model.Theme.ToString();
            }
            if (model.PortalStatus != null)
            {
                model.PortalStatusData = model.PortalStatus.ToString();
            }
            return View("ManagePortal", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManagePortal(PortalViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _portalBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("Index");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _portalBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            //return Json(new { success = false, errors = ModelState.SerializeErrors() });
            return View("ManagePortal", model);
        }

        //public IActionResult CreateStyle(string sourceId, PageContentTypeEnum? sourceType)
        //{
        //    var model = new StyleViewModel();
        //    model.DataAction = DataActionEnum.Create;
        //    model.SourceId = sourceId;
        //    model.SourceType = sourceType;
        //    return View("ManageStyle", model);
        //}

        public async Task<IActionResult> ManageStyle(string sourceId, PageContentTypeEnum sourceType, string pageId)
        {
            var model = await _styleBusiness.GetStyleBySourceId(sourceId, sourceType);
            model.SourceId = sourceId;
            model.SourceType = sourceType;
            model.PageId = pageId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageStyle(StyleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _styleBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("Index");
                        //return PopupRedirect("Style created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _styleBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("Index");
                        //return PopupRedirect("Style edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            //return Json(new { success = false, errors = ModelState.SerializeErrors() });
            return View("ManageStyle", model);
        }
        public async Task<IActionResult> GetBackgroundImage(string id)
        {

            var image = await _documentBusiness.GetSingleById(id);
            if (image != null)
            {
                return File(image.Content, "image/jpeg");
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> GetPageJsonForm(string pageId)
        {
            var pagedata = await _pageBusiness.GetSingleById(pageId);
            return Json(pagedata.JsonForm);
        }
        //[HttpPost]
        //public async Task<IActionResult> ManagePage(string data, string state)
        //{
        //    try
        //    {
        //        var model = JsonConvert.DeserializeObject<PageViewModel>(data);
        //        model.state = state;
        //        if (model.DataAction == DataActionEnum.Create)
        //        {
        //            var result = await _pageBusiness.Create(model);
        //            if (result.IsSuccess)
        //            {
        //                // return RedirectToAction("Index");
        //                result.Item.Portal = await _portalBusiness.GetSingleById(result.Item.PortalId);
        //                return Json(new { success = true, result = result.Item });
        //            }
        //            else
        //            {
        //                ModelState.AddModelErrors(result.Messages);
        //                return Json(new { success = false, errors = ModelState.SerializeErrors() });
        //            }
        //        }
        //        else if (model.DataAction == DataActionEnum.Edit)
        //        {
        //            var result = await _pageBusiness.Edit(model);
        //            if (result.IsSuccess)
        //            {
        //                //return RedirectToAction("Index");
        //                result.Item.Portal = await _portalBusiness.GetSingleById(result.Item.PortalId);
        //                return Json(new { success = true, result = result.Item });
        //            }
        //            else
        //            {
        //                ModelState.AddModelErrors(result.Messages);
        //                return Json(new { success = false, errors = ModelState.SerializeErrors() });
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public async Task<IActionResult> ManageGeneralTabData(PageViewModel model)
        {
            try
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _pageBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        // return RedirectToAction("Index");
                        //result.Item.Portal = await _portalBusiness.GetSingleById(result.Item.PortalId);
                        return Json(new { success = true, page = result.Item });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        //  return RedirectToAction("Index");
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _pageBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        //return RedirectToAction("Index");
                        // result.Item.Portal = await _portalBusiness.GetSingleById(result.Item.PortalId);
                        return Json(new { success = true, page = result.Item });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        //return RedirectToAction("Index");
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                //var model = JsonConvert.DeserializeObject<PageViewModel>(data);
                // model.state = state;

            }
            catch (Exception e)
            {
                // return RedirectToAction("Index");
                return Json(new { success = false, error = e.Message });
            }
            //return RedirectToAction("Index");
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> CreatePageStyle(string sourceId, PageContentTypeEnum? sourceType)
        {
            //var pagemodel = await _pageBusiness.GetSingleById(sourceId);
            var model = new StyleViewModel();
            //if (pagemodel != null)
            //{
            //    model.PageName = pagemodel.Name;
            //    model.PageTitle = pagemodel.Title;
            //    model.PageMenuName = pagemodel.MenuName;
            //    model.PageIsRootPage = pagemodel.IsRootPage;
            //    model.PageHideInMenu = pagemodel.HideInMenu;
            //}
            model.DataAction = DataActionEnum.Create;
            model.SourceId = sourceId;
            model.SourceType = sourceType;
            return View("ManagePageStyle", model);
        }

        public async Task<IActionResult> ManageSettings(string sourceId, PageContentTypeEnum sourceType)
        {
            //sourceId = "5fe8aaa0ca30881a161ee18d"; 
            var model = await _styleBusiness.GetStyleBySourceId(sourceId, sourceType);
            var pagemodel = await _pageBusiness.GetSingleById(sourceId);
            if (pagemodel != null)
            {
                model.PageName = pagemodel.Name;
                model.PageTitle = pagemodel.Title;
                model.PageMenuName = pagemodel.MenuName;
                model.PageIsRootPage = pagemodel.IsRootPage;
                model.PageHideInMenu = pagemodel.HideInMenu;
            }
            model.SourceId = sourceId;
            model.SourceType = sourceType;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageSettings(StyleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool flag = true;
                var pagemodel = await _pageBusiness.GetSingleById(model.SourceId);
                if (pagemodel != null)
                {
                    pagemodel.Name = model.PageName;
                    pagemodel.Title = model.PageTitle;
                    pagemodel.MenuName = model.PageMenuName;
                    pagemodel.IsRootPage = model.PageIsRootPage;
                    pagemodel.HideInMenu = model.PageHideInMenu;
                    pagemodel.DataAction = DataActionEnum.Edit;
                    var pageresult = await _pageBusiness.Edit(pagemodel);
                    if (!pageresult.IsSuccess)
                    {
                        ModelState.AddModelErrors(pageresult.Messages);
                        flag = false;
                    }
                }
                if (flag && model.DataAction == DataActionEnum.Create)
                {
                    var result = await _styleBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return RedirectToAction("Index");
                        //return PopupRedirect("Style created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (flag && model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _styleBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return RedirectToAction("Index");
                        //return PopupRedirect("Style edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            //return Json(new { success = false, errors = ModelState.SerializeErrors() });
            return View("ManageSettings", model);
        }

        public async Task<IActionResult> DeleteContent(string id, PageContentTypeEnum type)
        {
            var group = await _pageContentBusiness.GetSingleById(id);
            if (group != null)
            {
                var childs = await _pageBusiness.GetContentChildList(id, type);
                foreach (var child in childs)
                {
                    await _pageContentBusiness.Delete(child.Id);
                }
                await _pageContentBusiness.Delete(id);
                return Json(new { success = true });
            }
            return Json(new { success = true });
        }
        public async Task<IActionResult> DeletePage(string id)
        {
            var page = await _pageBusiness.GetSingleById(id);
            if (page != null)
            {
                await _pageBusiness.Delete(id);
                return Json(new { success = true });
            }
            return Json(new { success = true });
        }
        public async Task<ActionResult> ReadUserPermissionData(string pageId, TemplateTypeEnum pageType, string portalId)
        {
            var data = await _userPermissionBusiness.GetUserPermission(pageId, pageType, portalId);

            var dsResult = data/*.ToDataSourceResult(request)*/;
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadUserPermissionData1([DataSourceRequest] DataSourceRequest request,string pageId, TemplateTypeEnum pageType, string portalId)
        {
            var data = await _userPermissionBusiness.GetUserPermission(pageId, pageType, portalId);

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserPermissionData1([DataSourceRequest] DataSourceRequest request,
          [Bind(Prefix = "models")] IEnumerable<UserPermissionViewModel> userPermissions)
        {
            if (userPermissions != null && ModelState.IsValid)
            {
                await _userPermissionBusiness.SaveUserPermission(userPermissions.ToList());
                //foreach (var userPermission in userPermissions)
                //{
                //    if (userPermission.Id.IsNotNullAndNotEmpty())
                //    {
                //        userPermission.DataAction = DataActionEnum.Edit;
                //        await _userPermissionBusiness.Edit(userPermission);
                //    }
                //    else
                //    {
                //        userPermission.DataAction = DataActionEnum.Create;
                //        await _userPermissionBusiness.Create(userPermission);
                //    }
                //}
            }

            return Json(userPermissions.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserPermissionData(UserPermissionViewModel data)
        {
            List<UserPermissionViewModel> userPermissions = new List<UserPermissionViewModel>();
            
            if (data != null && ModelState.IsValid)
            {
                userPermissions.Add(data);
                await _userPermissionBusiness.SaveUserPermission(userPermissions.ToList());
                //foreach (var userPermission in userPermissions)
                //{
                //    if (userPermission.Id.IsNotNullAndNotEmpty())
                //    {
                //        userPermission.DataAction = DataActionEnum.Edit;
                //        await _userPermissionBusiness.Edit(userPermission);
                //    }
                //    else
                //    {
                //        userPermission.DataAction = DataActionEnum.Create;
                //        await _userPermissionBusiness.Create(userPermission);
                //    }
                //}
            }

            return Json(userPermissions);
        }

        [HttpPost]
        public ActionResult CreateUserPermissionData([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<UserPermissionViewModel> userPermissions)
        {
            var results = new List<UserPermissionViewModel>();
            if (userPermissions != null && ModelState.IsValid)
            {
                foreach (var userPermission in userPermissions)
                {
                    userPermission.DataAction = DataActionEnum.Create;
                    _userPermissionBusiness.Create(userPermission);
                    results.Add(userPermission);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserPermissionData([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<UserPermissionViewModel> userPermissions)
        {
            foreach (var userPermission in userPermissions)
            {
                await _userPermissionBusiness.Delete(userPermission.Id);
            }

            return Json(userPermissions.ToDataSourceResult(request, ModelState));
        }


        //User Role Permission
        //public async Task<ActionResult> ReadUserRolePermissionData([DataSourceRequest] DataSourceRequest request, string pageId, TemplateTypeEnum pageType, string portalId)
        //{
        //    var data = await _userRolePermissionBusiness.GetUserRolePermission(pageId, pageType, portalId);

        //    var dsResult = data.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}
        public async Task<ActionResult> ReadUserRolePermissionData( string pageId, TemplateTypeEnum pageType, string portalId)
        {
            var data = await _userRolePermissionBusiness.GetUserRolePermission(pageId, pageType, portalId);

           
            return Json(data);
        }
        //[HttpPost]
        //public async Task<IActionResult> UpdateUserRolePermissionData([DataSourceRequest] DataSourceRequest request,
        // [Bind(Prefix = "models")] IEnumerable<UserRolePermissionViewModel> userPermissions)
        //{
        //    if (userPermissions != null && ModelState.IsValid)
        //    {
        //        await _userRolePermissionBusiness.SaveUserRolePermission(userPermissions.ToList());
        //        //foreach (var userPermission in userPermissions)
        //        //{
        //        //    if (userPermission.Id.IsNotNullAndNotEmpty())
        //        //    {
        //        //        userPermission.DataAction = DataActionEnum.Edit;
        //        //         await _userRolePermissionBusiness.Edit(userPermission);
        //        //    }
        //        //    else
        //        //    {
        //        //        userPermission.DataAction = DataActionEnum.Create;
        //        //        await _userRolePermissionBusiness.Create(userPermission);
        //        //    }

        //        //}
        //    }

        //    return Json(userPermissions.ToDataSourceResult(request, ModelState));
        //}
        [HttpPost]
        public async Task<IActionResult> UpdateUserRolePermissionData(UserRolePermissionViewModel data)
        {
            List<UserRolePermissionViewModel> userPermissions = new List<UserRolePermissionViewModel>();
            if (data != null && ModelState.IsValid)
            {
                userPermissions.Add(data);
                await _userRolePermissionBusiness.SaveUserRolePermission(userPermissions.ToList());
               
            }

            return Json(userPermissions);
        }

        [HttpPost]
        public ActionResult CreateUserRolePermissionData([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<UserRolePermissionViewModel> userPermissions)
        {
            var results = new List<UserRolePermissionViewModel>();
            if (userPermissions != null && ModelState.IsValid)
            {
                foreach (var userPermission in userPermissions)
                {
                    _userRolePermissionBusiness.Create(userPermission);
                    results.Add(userPermission);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserRolePermissionData([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<UserRolePermissionViewModel> userPermissions)
        {
            foreach (var userPermission in userPermissions)
            {
                await _userRolePermissionBusiness.Delete(userPermission.Id);
            }

            return Json(userPermissions.ToDataSourceResult(request, ModelState));
        }
        public ActionResult ReadTableProperties([DataSourceRequest] DataSourceRequest request)
        {
            List<PageIndexColumnViewModel> model = new List<PageIndexColumnViewModel>();
            model.Add(new PageIndexColumnViewModel()
            {
                Id = "1",
                ColumnName = "Column 1",
                HeaderName = "Header 1",
                EnableFiltering = true,
                EnableSorting = true,
                AdvanceSetting = "abc1"
            });
            model.Add(new PageIndexColumnViewModel()
            {
                Id = "2",
                ColumnName = "Column 2",
                HeaderName = "Header 2",
                EnableFiltering = true,
                EnableSorting = true,
                AdvanceSetting = "abc2"
            });
            model.Add(new PageIndexColumnViewModel()
            {
                Id = "3",
                ColumnName = "Column 3",
                HeaderName = "Header 3",
                EnableFiltering = true,
                EnableSorting = true,
                AdvanceSetting = "abc3"
            });
            return Json(model.ToDataSourceResult(request));
        }
        [HttpPost]
        public async Task<ActionResult> ManageTableData(PageIndexViewModel model)
        {
            model.SelectedTableRows = JsonConvert.DeserializeObject<List<PageIndexColumnViewModel>>(model.RowData);
            var result = await _pageIndexBusiness.Create(model);
            if (result.IsSuccess)
            {

            }
            return View("ManageIndex");
        }

        public async Task<ActionResult> ManageUserPermissionData(string pageId, string id, string portalId)
        {
            var model = new UserDataPermissionViewModel();
            model.PortalId = portalId;
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _userDataPermissionBusiness.GetSingleById(id);
                if (model.IsNotNull())
                {
                    var user = await _userBusiness.GetSingleById(model.UserId);
                    if (user.IsNotNull())
                    {
                        model.UserName = user.Name;
                        model.UserIds = new List<string>();
                        model.UserIds.Add(model.UserId);
                    }
                }
            }
            model.PageId = pageId;
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetColunmMetadataListByPage(string pageId)
        {
            var data = await _userDataPermissionBusiness.GetColunmMetadataListByPage(pageId);
            return Json(data);
        }


        public async Task<ActionResult> ManageUserRolePermissionData(string pageId, string id, string portalId)
        {
            var model = new UserRoleDataPermissionViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _userRoleDataPermissionBusiness.GetSingleById(id);
                if (model.IsNotNull())
                {
                    var role = await _userRoleBusiness.GetSingleById(model.UserRoleId);
                    if (role.IsNotNull())
                    {
                        model.UserRoleName = role.Name;
                        model.PortalId = portalId;
                        model.UserRoleIds = new List<string>();
                        model.UserRoleIds.Add(model.UserRoleId);

                    }
                }
            }
            model.PageId = pageId;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ManageUserDataPermission(UserDataPermissionViewModel model)
        {
            if (model.IsNotNull())
            {
                if (model.Id.IsNullOrEmpty())
                {
                    //create
                    if (model.UserIds.IsNotNull())
                    {
                        foreach (var r in model.UserIds)
                        {
                            model.UserId = r;
                            var res = await _userDataPermissionBusiness.Create(model);
                        }
                    }
                    return Json(new { success = true });
                }
                else
                {
                    if (model.UserId.IsNullOrEmpty())
                    {

                        model.UserId = model.UserId;
                    }
                    //edit
                    var res = await _userDataPermissionBusiness.Edit(model);
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<ActionResult> ManageUserRolesDataPermission(UserRoleDataPermissionViewModel model)
        {
            if (model.IsNotNull())
            {
                if (model.Id.IsNullOrEmpty())
                {
                    //create
                    if (model.UserRoleIds.IsNotNull())
                    {
                        foreach (var r in model.UserRoleIds)
                        {
                            model.UserRoleId = r;
                            var res = await _userRoleDataPermissionBusiness.Create(model);
                        }
                    }
                    return Json(new { success = true });
                }
                else
                {
                    if (model.UserRoleId.IsNullOrEmpty())
                    {

                        model.UserRoleId = model.UserRoleId;
                    }
                    //edit
                    var res = await _userRoleDataPermissionBusiness.Edit(model);
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUserRolesDataPermission(string id)
        {
            if (id.IsNotNull())
            {
                //create
                await _userRoleDataPermissionBusiness.Delete(id);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUserDataPermission(string id)
        {
            if (id.IsNotNull())
            {
                //create
                await _userDataPermissionBusiness.Delete(id);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<ActionResult> ReadUserDataPermission([DataSourceRequest] DataSourceRequest request, string pageId, string portalId)
        {
            var data = await _userDataPermissionBusiness.GetUserDataPermissionByPageId(pageId, portalId);
            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadUserDataPermissionList( string pageId, string portalId)
        {
            var data = await _userDataPermissionBusiness.GetUserDataPermissionByPageId(pageId, portalId);
           
            return Json(data);
        }

        //public async Task<ActionResult> ReadUserRoleDataPermission([DataSourceRequest] DataSourceRequest request, string pageId, string portalId)
        //{
        //    var data = await _userRoleDataPermissionBusiness.GetUserRoleDataPermissionByPageId(pageId, portalId);
        //    var dsResult = data.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}
        public async Task<ActionResult> ReadUserRoleDataPermission(string pageId, string portalId)
        {
            var data = await _userRoleDataPermissionBusiness.GetUserRoleDataPermissionByPageId(pageId, portalId);
            
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetPortalList(string portalId)
        {
            var data = await _portalBusiness.GetList();
            if (portalId.IsNotNullAndNotEmpty())
            {
                return Json(data.Where(x => x.Id == portalId));
            }
            return Json(data);
        }

        public async Task<ActionResult> PortalDiagram()
        {
            var portalDetails = await _portalBusiness.GetSingle(x => x.Name == "CMS");
            ViewBag.PortalId = "";
            if (portalDetails.IsNotNull())
            {
                ViewBag.PortalId = portalDetails.Id;
            }
            return View();
        }

        public async Task<ActionResult> PageDiagram(string pageId, bool isPopup = false)
        {
            var portalDetails = await _portalBusiness.GetSingle(x => x.Name == "CMS");
            var pageDetails = await _pageBusiness.GetSingleById(pageId);
            if (pageDetails.IsNotNull())
            {
                ViewBag.TemplateId = pageDetails.TemplateId;
            }
            ViewBag.PortalId = "";
            if (portalDetails.IsNotNull())
            {
                ViewBag.PortalId = portalDetails.Id;
            }
            if (isPopup)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = "/Areas/Cms/Views/Shared/_LayoutCms.cshtml";
            }
            return View();
        }

        public async Task<IActionResult> GetPortalDiagramData(string id)
        {
            ViewBag.PortalId = id;//"4610639e-2e3c-437c-a7e8-65f412787941";
            var portalId = ViewBag.PortalId;

            var model = new List<MenuViewModel>();
            var portal = await _portalBusiness.GetSingleById(portalId);
            if (portal != null)
            {
                model = await _portalBusiness.GetPortalDiagramData(portal, _userContext.UserId, _userContext.CompanyId);
            }
            return Json(model.ToList());
        }

        [Authorize]
        public async Task<IActionResult> TableIdNameList()
        {
            try
            {
                var tables = await _tableMetadataBusiness.GetList();
                return Ok(tables.Select(x => new { Id = x.Id, Name = x.Schema + "." + x.Name }));
            }
            catch (Exception)
            {

                throw;
            }

        }
        [Authorize]
        public async Task<IActionResult> GetData(string tableName, string columns = null, string filter = null, string orderbyColumns = null, string orderBy = null)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var data = await _cmsBusiness.GetData(split[0], split[1]);
                //var j = JsonConvert.SerializeObject(data);
                //var result = JsonConvert.DeserializeObject(j);
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        public async Task<IActionResult> ColumnIdNameList(string tableName)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var columns = await _columnMetadataBusiness.GetViewableColumnMetadataList(split[0], split[1], true);
                return Ok(columns.Select(x => new { Id = x.Id, Name = x.Name, DataType = x.DataType.ToString() }));
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Authorize]
        public async Task<IActionResult> ColumnList(string tableName)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var columns = await _columnMetadataBusiness.GetViewableColumnMetadataList(split[0], split[1], true);
                return Ok(columns);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IActionResult> GetPortalIdNameList()
        {
            var list = await _pageBusiness.GetPortalList();
            return Json(list);
        }


        public async Task<IActionResult> PageDetailsIndex(string Id)
        {
            var model = new PageDetailsViewModel { PageId = Id };

            if (Id.IsNotNull())
            {
                var modelM = await _pageBusiness.GetSingleById(Id);
                if (modelM.IsNotNull())
                {
                    model.IconFileId = modelM.IconFileId;
                    model.ExpandHelpPanel = modelM.ExpandHelpPanel;
                }
            }


            return View(model);
        }



        public async Task<JsonResult> ReadPageDetailsData([DataSourceRequest] DataSourceRequest request, string PageId)
        {
            var list = await _PageDetailsBusiness.GetList(x => x.PageId == PageId);
            var dsResult = list.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<JsonResult> ReadPageDetailsDataList(string PageId)
        {
            var list = await _PageDetailsBusiness.GetList(x => x.PageId == PageId);
           // var dsResult = list.ToDataSourceResult(request);
            return Json(list);
        }


        public IActionResult CreatePageDetails(string PageId)
        {
            return View("ManagePageDetails", new PageDetailsViewModel
            {
                DataAction = DataActionEnum.Create,
                PageId = PageId,

            }); ;
        }

        public async Task<IActionResult> EditPageDetails(string Id)
        {
            var member = await _PageDetailsBusiness.GetSingleById(Id);

            if (member != null)
            {
                member.DataAction = DataActionEnum.Edit;
                return View("ManagePageDetails", member);
            }
            return View("ManagePageDetails", new PageDetailsViewModel());
        }
        public async Task<IActionResult> DeletePageDetails(string id)
        {
            await _PageDetailsBusiness.Delete(id);
            //return View("Index", new MenuGroupViewModel());
            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> ManagePageDetails(PageDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _PageDetailsBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true });
                        //return PopupRedirect("Menu Group created successfully", true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _PageDetailsBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true });
                        //return PopupRedirect("Menu Group edited successfully", true);                        
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return Json(new { success = true });
        }


        [HttpPost]

        public async Task<IActionResult> Saveimg(string Id, string IconId, bool ExpanndeHelp)
        {
            var model = await _pageBusiness.GetSingleById(Id);
            if (model.IsNotNull())
            {
                model.IconFileId = IconId;
                model.ExpandHelpPanel = ExpanndeHelp;
                var Result = await _pageBusiness.Edit(model);
                if (Result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }

            }
            else
            {
                return Json(new { success = false });
            }



        }

    }
}
