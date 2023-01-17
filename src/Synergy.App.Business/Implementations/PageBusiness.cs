using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Synergy.App.Business
{
    public class PageBusiness : BusinessBase<PageViewModel, Page>, IPageBusiness
    {
        private readonly IPageContentBusiness _pageContentBusiness;
        private readonly IPublishPageBusiness _publishPageBusiness;
        private readonly IPublishPageContentBusiness _publishPageContentBusiness;
        private readonly IMenuGroupBusiness _menuGroupBusiness;
        private readonly ISubModuleBusiness _subModuleBusiness;
        private readonly IPermissionBusiness _permissionBusiness;
        private readonly IUserPermissionBusiness _userPermissionBusiness;
        private IResourceLanguageBusiness _resourceLanguageBusiness;
        private readonly IUserRolePermissionBusiness _userRolePermissionBusiness;
        private readonly IRepositoryQueryBase<UserPortalViewModel> _repoQuery;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        private readonly IServiceProvider _sp;
        public PageBusiness(IRepositoryBase<PageViewModel, Page> repo
            , IPublishPageContentBusiness publishPageContentBusiness
            , IPublishPageBusiness publishPageBusiness
            , IMapper autoMapper
            , IUserRolePermissionBusiness userRolePermissionBusiness
            , IPageContentBusiness pageContentBusiness
            , ISubModuleBusiness subModuleBusiness
            , IMenuGroupBusiness menuGroupBusiness
            , IPermissionBusiness permissionBusiness
            , IUserPermissionBusiness userPermissionBusiness
            , ICmsQueryBusiness cmsQueryBusiness
            , IResourceLanguageBusiness resourceLanguageBusiness
            , IRepositoryQueryBase<UserPortalViewModel> repoQuery
            , IServiceProvider sp)
            : base(repo, autoMapper)
        {
            _pageContentBusiness = pageContentBusiness;
            _menuGroupBusiness = menuGroupBusiness;
            _publishPageBusiness = publishPageBusiness;
            _subModuleBusiness = subModuleBusiness;
            _publishPageContentBusiness = publishPageContentBusiness;
            _repoQuery = repoQuery;
            _permissionBusiness = permissionBusiness;
            _userPermissionBusiness = userPermissionBusiness;
            _userRolePermissionBusiness = userRolePermissionBusiness;
            _resourceLanguageBusiness = resourceLanguageBusiness;
            _cmsQueryBusiness = cmsQueryBusiness;
            _sp = sp;
        }

        public async Task<List<TreeViewViewModel>> GetContentTreeList(string id)
        {
            var list = new List<TreeViewViewModel>();
            var data = await GetList();

            if (id.IsNullOrEmpty())
            {
                var portals = await _repo.GetList<PortalViewModel, Portal>();
                if (portals != null && portals.Any())
                {
                    //portals = portals.Where(x => x.Name != "CMS").OrderBy(x => x.SequenceOrder).ToList();
                    list.AddRange(portals.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = x.ParentId,
                        hasChildren = true,
                        Type = ContentTypeEnum.Portal.ToString(),
                        IconCss = x.PortalStatus.ToStatusIcon(),
                        IconTitle = x.PortalStatus.ToIconTitle(),
                        children = true,
                        text = x.DisplayName,
                        parent = "#",
                        a_attr = new { data_id = x.Id, data_name = x.Name, data_type = ContentTypeEnum.Portal.ToString() },
                    }));
                }
            }

            else
            {
                var menugroup = await _menuGroupBusiness.GetSingleById(id);
                if (menugroup == null)
                {
                    var page = await _repo.GetList<PageViewModel, Page>(x => x.PortalId == id, y => y.MenuGroup);
                    var groupedPages = page.GroupBy(x => x.MenuGroupId).Select(y => y.FirstOrDefault()).Select(p => p.MenuGroup).Distinct();
                    list.AddRange(groupedPages.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = id,
                        PortalId = x.PortalId,
                        hasChildren = true,
                        Type = ContentTypeEnum.MenuGroup.ToString(),
                        IconCss = "",
                        IconTitle = "",
                        parent = id,
                        text = x.Name,
                        children = true,
                        a_attr = new { data_id = x.Id, data_name = x.Name, data_type = ContentTypeEnum.MenuGroup.ToString(), data_portalid = x.PortalId },
                    }));
                }
                else
                {
                    var pages = await _repo.GetList<PageViewModel, Page>(x => x.MenuGroupId == id);
                    if (pages != null && pages.Any())
                    {
                        pages = pages.OrderBy(x => x.SequenceOrder).ToList();
                        list.AddRange(pages.Select(x => new TreeViewViewModel
                        {
                            id = x.Id,
                            Name = x.Name,
                            DisplayName = x.Title.Coalesce(x.Name),
                            //ParentId = x.ParentId,
                            ParentId = id,
                            PortalId = x.PortalId,
                            ItemLevel = x.Level,
                            hasChildren = false,
                            Type = ContentTypeEnum.ContentPage.ToString(),
                            parent = id,
                            text = x.Name,
                            a_attr = new { data_id = x.Id, data_name = x.Name, data_type = ContentTypeEnum.ContentPage.ToString(), data_portalid = x.PortalId },
                        }));
                        //list.ForEach(x => x.hasChildren = data.Any(y => y.ParentId == x.id));
                    }
                }

            }
            return list;
        }

        public async Task<PageViewModel> GetPageData(string pageId)
        {
            var page = await GetSingleById(pageId);
            //if (page != null)
            //{
            //    var menugroup = await _menuGroupBusiness.GetSingleById(page.MenuGroupId);
            //    if (menugroup != null)
            //    {
            //        page.SubModuleId = menugroup.SubModuleId;
            //        var subModule = await _subModuleBusiness.GetSingleById(menugroup.SubModuleId);
            //        if (subModule != null)
            //        {
            //            page.ModuleId = subModule.ModuleId;
            //        }
            //    }

            //}

            //{
            //    page.Groups = await _pageContentBusiness.GetList(x => x.PageId == page.Id && x.PageContentType == PageContentTypeEnum.Group);
            //    foreach (var group in page.Groups)
            //    {
            //        group.DataAction = DataActionEnum.Edit;
            //    }
            //    page.Cells = await _pageContentBusiness.GetList(x => x.PageId == page.Id && x.PageContentType == PageContentTypeEnum.Cell);
            //    foreach (var Cell in page.Cells)
            //    {
            //        Cell.DataAction = DataActionEnum.Edit;
            //    }
            //    page.Columns = await _pageContentBusiness.GetList(x => x.PageId == page.Id && x.PageContentType == PageContentTypeEnum.Column);
            //    foreach (var Column in page.Columns)
            //    {
            //        Column.DataAction = DataActionEnum.Edit;
            //    }
            //    page.Components = await _pageContentBusiness.GetList(x => x.PageId == page.Id && x.PageContentType == PageContentTypeEnum.Component);
            //    foreach (var Component in page.Components)
            //    {
            //        Component.DataAction = DataActionEnum.Edit;
            //    }
            //}
            return page;
        }

        public async override Task<CommandResult<PageViewModel>> Create(PageViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<PageViewModel>(model);
            data.Description = HttpUtility.HtmlDecode(data.Description);
            if (model.Title.IsNullOrEmpty())
            {
                return CommandResult<PageViewModel>.Instance(model, x => x.Title, "Page Title is required.");
            }

            if (data.Name.IsNullOrEmpty())
            {
                data.Name = data.Title;
                string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

                foreach (char c in invalid)
                {
                    data.Name = data.Name.Replace(c.ToString(), "-");
                }
                data.Name = data.Name.Replace(" ", "-");
            }
            // Check if name is unique
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PageViewModel>.Instance(model, false, validateName.Messages);
            }

            //if (data.ParentId == data.PortalId)
            //{
            //    data.Level = 1;
            //}
            //else
            //{
            //    var parentpage = await GetSingleById(data.ParentId);
            //    data.Level = parentpage.Level + 1;
            //}
            if (data.IsRootPage)
            {
                var existingpagelist = await GetList(x => x.PortalId == data.PortalId);
                if (existingpagelist.Any(x => x.IsRootPage == true))
                {
                    var rootpage = existingpagelist.Where(x => x.IsRootPage == true).FirstOrDefault();
                    rootpage.IsRootPage = false;
                    await base.Edit(rootpage);
                }
            }
            var result = await base.Create(data, autoCommit);
            if (result.IsSuccess)
            {
                if (model.MenuName.IsNotNullAndNotEmpty())
                {
                    var Ldata = new ResourceLanguageViewModel
                    {
                        TemplateId = result.Item.Id,
                        TemplateType = TemplateTypeEnum.PortalPage,
                        English = result.Item.MenuName,
                        Code = "PortalPageMenuName"
                    };
                    await base.Edit<ResourceLanguageViewModel, ResourceLanguage>(Ldata);
                }
                if (model.Title.IsNotNullAndNotEmpty())
                {
                    var Ldata = new ResourceLanguageViewModel
                    {
                        TemplateId = result.Item.Id,
                        TemplateType = TemplateTypeEnum.PortalPage,
                        English = result.Item.Title,
                        Code = "PortalPageTitle"
                    };
                    await base.Create<ResourceLanguageViewModel, ResourceLanguage>(Ldata);
                }
                //await Task.WhenAll(SavePageContent(model.Groups), SavePageContent(model.Columns), SavePageContent(model.Cells), SavePageContent(model.Components));
                //if (model.state == "Publish")
                //{
                //    await PublishPage(result.Item.Id);
                //}
            }
            return CommandResult<PageViewModel>.Instance(result.Item, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PageViewModel>> Edit(PageViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<PageViewModel>(model);
            data.Description = HttpUtility.HtmlDecode(data.Description);
            if (model.Title.IsNullOrEmpty())
            {
                return CommandResult<PageViewModel>.Instance(model, x => x.Title, "Page Title is required.");
            }
            //var style = await _styleBusiness.GetStyleBySourceId(model.Id);
            //if (style != null)
            //{
            //    data.Style = style.ToStyleText();
            //}
            // Check if name is unique
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PageViewModel>.Instance(model, false, validateName.Messages);
            }
            if (data.IsRootPage)
            {
                var existingpagelist = await GetList(x => x.PortalId == data.PortalId);
                if (existingpagelist.Any(x => x.IsRootPage == true))
                {
                    var rootpage = existingpagelist.Where(x => x.IsRootPage == true).FirstOrDefault();
                    rootpage.IsRootPage = false;
                    await base.Edit(rootpage);
                }
            }
            var result = await base.Edit(data, autoCommit);
            if (result.IsSuccess)
            {
                if (model.MenuName.IsNotNullAndNotEmpty())
                {
                    var MenuName = await _repo.GetSingle(x => x.MenuName.ToLower() != model.MenuName.ToLower() && x.Id == model.Id);
                    if (MenuName == null)
                    {
                        var Ldata = new ResourceLanguageViewModel
                        {
                            TemplateId = result.Item.Id,
                            TemplateType = TemplateTypeEnum.PortalPage,
                            English = result.Item.MenuName,
                            Code = "PortalPageMenuName"
                        };
                        var record = await _resourceLanguageBusiness.GetExistingResourceLanguage(Ldata);
                        if (record != null)
                        {
                            record.English = result.Item.MenuName;
                            await _resourceLanguageBusiness.Edit(record);
                        }
                        else
                        {
                            await base.Create<ResourceLanguageViewModel, ResourceLanguage>(Ldata);
                        }

                    }
                }

                if (model.Title.IsNotNullAndNotEmpty())
                {
                    var Title = await _repo.GetSingle(x => x.Title.ToLower() != model.Title.ToLower() && x.Id == model.Id);
                    if (Title == null)
                    {
                        var Ldata = new ResourceLanguageViewModel
                        {
                            TemplateId = result.Item.Id,
                            TemplateType = TemplateTypeEnum.PortalPage,
                            English = result.Item.Title,
                            Code = "PortalPageTitle"
                        };
                        var record = await _resourceLanguageBusiness.GetExistingResourceLanguage(Ldata);
                        if (record != null)
                        {
                            record.English = result.Item.Title;
                            await _resourceLanguageBusiness.Edit(record);
                        }
                        else
                        {
                            await base.Create<ResourceLanguageViewModel, ResourceLanguage>(Ldata);
                        }
                    }
                }



                //await Task.WhenAll(SavePageContent(model.Groups), SavePageContent(model.Columns), SavePageContent(model.Cells), SavePageContent(model.Components));
                //if (model.state == "Publish")
                //{
                //    await PublishPage(result.Item.Id);
                //}
            }
            return CommandResult<PageViewModel>.Instance(model);
        }
        public async Task SavePageContent(List<PageContentViewModel> model)
        {
            foreach (var content in model)
            {
                if (content.DataAction == DataActionEnum.Create)
                {
                    var result1 = await _pageContentBusiness.Create(content);
                }
                else if (content.DataAction == DataActionEnum.Edit)
                {
                    var contentdata = await _pageContentBusiness.GetSingleById(content.Id);
                    if (contentdata != null)
                    {
                        contentdata.Title = content.Title;
                        contentdata.PageId = content.PageId;
                        contentdata.ParentId = content.ParentId;
                        contentdata.Content = content.Content;
                        contentdata.Style = content.Style;
                        contentdata.CssClass = content.CssClass;
                        contentdata.PageContentType = content.PageContentType;
                        contentdata.ComponentType = content.ComponentType;
                        contentdata.PageRowType = content.PageRowType;
                        var result1 = await _pageContentBusiness.Edit(contentdata);
                    }
                }
            }
        }


        public async Task<PageViewModel> GetDefaultPageDataByPortal(PortalViewModel portal, string userId, RunningModeEnum runningMode)
        {
            PageViewModel page = null;
            if (userId.IsNotNullAndNotEmpty())
            {
                var ub = _sp.GetService<IUserBusiness>();
                page = await ub.GetUserLandingPage(userId, portal.Id);
            }

            if (page == null)
            {
                page = await _repo.GetSingleGlobal<PageViewModel, Page>(x => x.PortalId == portal.Id && x.IsRootPage == true);
            }
            if (page == null)
            {
                var pages = await _repo.GetListGlobal<PageViewModel, Page>(x => x.PortalId == portal.Id);
                page = pages.OrderBy(x => x.SequenceOrder).FirstOrDefault();
            }
            //else
            //{
            //    page = await GetPageDataForExecution(portal.Id, page.Name, runningMode);
            //}
            return page;
        }

        public async Task<PageViewModel> GetPageDataForExecution(string portalId, string pageName, RunningModeEnum runningMode)
        {

            if (runningMode == RunningModeEnum.Live)
            {
                return await GetPageDataForLive(portalId, pageName, runningMode);
            }
            else
            {
                return await GetPageDataForPreview(portalId, pageName, runningMode);
            }

        }

        private async Task<PageViewModel> GetPageDataForPreview(string portalId, string pageName, RunningModeEnum runningMode)
        {
            //var page = await GetSingleGlobal(x => x.PortalId == portalId && x.Name == pageName);
            var page = await GetListGlobal(x => x.PortalId == portalId);
            return page.FirstOrDefault(x => x.Name.ToLower() == pageName.ToLower());
        }

        private async Task<PageViewModel> GetPageDataForLive(string portalId, string pageName, RunningModeEnum runningMode)
        {
            var page = await _repo.GetSingleGlobal<PageViewModel, Page>(x => x.Name == pageName && x.PortalId == portalId);
            return page;
        }

        public async Task<List<PageContentViewModel>> GetContentChildList(string id, PageContentTypeEnum type)
        {
            List<PageContentViewModel> list = new List<PageContentViewModel>();
            if (type == PageContentTypeEnum.Group)
            {
                var columns = await _pageContentBusiness.GetList(x => x.PageContentType == PageContentTypeEnum.Column && x.ParentId == id);
                if (columns != null)
                {
                    list.AddRange(columns);
                }
                var cells = await _pageContentBusiness.GetList(x => x.PageContentType == PageContentTypeEnum.Cell);
                if (cells != null)
                {
                    foreach (var cell in cells)
                    {
                        if (columns.Exists(y => y.Id == cell.ParentId))
                        {
                            list.AddRange(cells);
                        }
                    }
                }
                var components = await _pageContentBusiness.GetList(x => x.PageContentType == PageContentTypeEnum.Column);
                if (components != null)
                {
                    foreach (var component in components)
                    {
                        if (cells.Exists(y => y.Id == component.ParentId))
                        {
                            list.AddRange(components);
                        }
                    }
                }
            }
            else if (type == PageContentTypeEnum.Cell)
            {
                var components = await _pageContentBusiness.GetList(x => x.PageContentType == PageContentTypeEnum.Column && x.ParentId == id);
                if (components != null)
                {
                    list.AddRange(components);
                }
            }
            return list;
        }
        private async Task<CommandResult<PageViewModel>> IsNameExists(PageViewModel viewModel)
        {
            var pagelist = await GetList(x =>/* x.ParentId == viewModel.ParentId && */x.PortalId == viewModel.PortalId);
            if (pagelist.Exists(x => x.Id != viewModel.Id && x.Name == viewModel.Name))
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The given name already exists. Please enter another name");
                return CommandResult<PageViewModel>.Instance(viewModel, false, obj);
            }
            return CommandResult<PageViewModel>.Instance();
        }

        public async Task PublishPage(string id)
        {
            // Publish Page
            var page = await GetSingleById(id);
            var data = _autoMapper.Map<PublishPageViewModel>(page);
            if (page != null)
            {
                var publishedPagesList = await _publishPageBusiness.GetList(x => x.PageId == id);
                if (publishedPagesList.Count() > 0)
                {
                    var version = 0;
                    foreach (var publishpage in publishedPagesList)
                    {
                        if (publishpage.IsLatest == true)
                        {
                            publishpage.IsLatest = false;
                            version = publishpage.VersionNo;
                            var result1 = await _publishPageBusiness.Edit(publishpage);
                        }
                    }
                    data.PublishedBy = _repo.UserContext.UserId;
                    data.PublishedDate = DateTime.Now.ToString();
                    data.PageId = id;
                    data.Id = null;
                    data.VersionNo = version + 1;
                    data.IsLatest = true;
                    var result = await _publishPageBusiness.Create(data);
                }
                else
                {
                    data.PublishedBy = _repo.UserContext.UserId;
                    data.PublishedDate = DateTime.Now.ToString();
                    data.PageId = id;
                    data.VersionNo = 0;
                    data.Id = null;
                    data.IsLatest = true;
                    var result = await _publishPageBusiness.Create(data);
                }
            }

            // get all groups , columns, cells, components
            var contentList = await _pageContentBusiness.GetList(x => x.PageId == id);
            if (contentList != null && contentList.Count() > 0)
            {
                var contentIds = contentList.Select(x => x.Id).ToList();
                // publish pageContent
                await PublishPageContent(contentIds);
            }
        }
        public async Task PublishPageContent(List<string> item)
        {
            foreach (var id in item)
            {
                // Publish Page Content
                var page = await _pageContentBusiness.GetSingleById(id);
                var data = _autoMapper.Map<PublishPageContentViewModel>(page);
                if (page != null)
                {
                    var publishedPagesContentList = await _publishPageContentBusiness.GetList(x => x.PageContentId == id);
                    if (publishedPagesContentList.Count() > 0)
                    {
                        var version = 0;
                        foreach (var publishpage in publishedPagesContentList)
                        {
                            if (publishpage.IsLatest == true)
                            {
                                publishpage.IsLatest = false;
                                version = publishpage.VersionNo;
                                var result1 = await _publishPageContentBusiness.Edit(publishpage);
                            }
                        }
                        data.PublishedBy = _repo.UserContext.UserId;
                        data.PublishedDate = DateTime.Now.ToString();
                        data.PageContentId = id;
                        data.Id = null;
                        data.VersionNo = version + 1;
                        data.IsLatest = true;
                        var result = await _publishPageContentBusiness.Create(data);
                    }
                    else
                    {
                        data.PublishedBy = _repo.UserContext.UserId;
                        data.PublishedDate = DateTime.Now.ToString();
                        data.PageContentId = id;
                        data.VersionNo = 0;
                        data.Id = null;
                        data.IsLatest = true;
                        var result = await _publishPageContentBusiness.Create(data);
                    }
                }
            }
        }

        public async Task<PageViewModel> GetUserPagePermission(string portalId, string pageId)
        {
            var result = await _cmsQueryBusiness.GetUserPagePermission(portalId, pageId);

            return result;
        }

        public async Task<PageViewModel> GetPageForExecution(string pageId)
        {
            var result = await _repo.GetSingleGlobal<PageViewModel, Page>(x => x.Id == pageId, x => x.Template, x => x.Portal);
            if (result != null)
            {
                var languages = await _repo.GetList<ResourceLanguageViewModel, ResourceLanguage>(x => x.TemplateType == TemplateTypeEnum.PortalPage && x.TemplateId == pageId);
                if (languages != null)
                {
                    var title = languages.FirstOrDefault(x => x.Code == "PortalPageTitle");
                    if (title != null)
                    {
                        result.Title = title.GetLanguageValue(_repo.UserContext.CultureName);
                    }
                    var menuName = languages.FirstOrDefault(x => x.Code == "PortalPageMenuName");
                    if (menuName != null)
                    {
                        result.MenuName = menuName.GetLanguageValue(_repo.UserContext.CultureName);
                    }
                }
            }
            return result.ToViewModel<PageViewModel, Page>(_autoMapper);
        }

        public async Task<PageViewModel> GetPageDataForExecution(string pageId)
        {
            var page = await GetSingleGlobal(x => x.Id == pageId);
            return page;
        }

        public async Task<PageViewModel> GetPageForExecution(string portalId, string pageName)
        {
            var result = await _repo.GetSingleGlobal(x => x.PortalId == portalId && x.Name == pageName, x => x.Portal, x => x.Template);
            if (result != null)
            {
                var languages = await _repo.GetList<ResourceLanguageViewModel, ResourceLanguage>(x => x.TemplateType == TemplateTypeEnum.PortalPage && x.TemplateId == result.Id);
                if (languages != null)
                {
                    var title = languages.FirstOrDefault(x => x.Code == "PortalPageTitle");
                    if (title != null)
                    {
                        result.Title = title.GetLanguageValue(_repo.UserContext.CultureName);
                    }
                    var menuName = languages.FirstOrDefault(x => x.Code == "PortalPageMenuName");
                    if (menuName != null)
                    {
                        result.MenuName = menuName.GetLanguageValue(_repo.UserContext.CultureName);
                    }
                }
            }
            return result;
        }

        public async Task<List<BreadcrumbViewModel>> GetBreadcrumbs(PageViewModel page)
        {
            var list = new List<BreadcrumbViewModel>();
            var id = "0";
            if (!page.ShowOutsideMenuGroup)
            {
                var language = Helper.GetLanguage(_repo.UserContext.CultureName);
                //  var menuGroup = await _repo.GetSingleById<MenuGroupViewModel, MenuGroup>(page.MenuGroupId);
                var menuGroup = await _cmsQueryBusiness.GetMenuGroup(language, page);
                if (menuGroup != null)
                {
                    id = menuGroup.Id;
                    list.Add(new BreadcrumbViewModel
                    {
                        Id = menuGroup.Id,
                        ParentId = "0",
                        Text = menuGroup.Name,
                        Name = menuGroup.Name,
                        IsNotClickable = true,
                        IsClickDisabled = true,
                        IconCss = menuGroup.IconCss.Coalesce("fa fa-user-circle")

                    });
                }
            }

            if (page.DataAction == DataActionEnum.Create)
            {
                list.Add(new BreadcrumbViewModel
                {
                    Id = page.Id,
                    ParentId = id,
                    Text = page.Title,
                    Name = page.Name,
                    PageId = page.Id,
                    PageType = page.PageType.ToString(),
                    DataAction = DataActionEnum.None.ToString(),
                    RequestSource = RequestSourceEnum.Main.ToString(),
                    RecordId = page.RecordId,
                    IconCss = page.IconCss.Coalesce("fa fa-user-circle"),
                    IsNotClickable = false,
                    IsClickDisabled = false
                });
                list.Add(new BreadcrumbViewModel
                {
                    Id = "Create",
                    ParentId = page.Id,
                    Text = "Create",
                    Name = $"Create_{page.Name}",
                    PageId = page.Id,
                    PageType = page.PageType.ToString(),
                    DataAction = page.DataAction.ToString(),
                    RequestSource = page.RequestSource.ToString(),
                    RecordId = page.RecordId,
                    IconCss = page.IconCss.Coalesce("fa fa-user-circle"),
                    IsNotClickable = false,
                    IsClickDisabled = true
                });
            }
            else if (page.DataAction == DataActionEnum.Edit)
            {
                list.Add(new BreadcrumbViewModel
                {
                    Id = page.Id,
                    ParentId = id,
                    Text = page.Title,
                    Name = page.Name,
                    PageId = page.Id,
                    PageType = page.PageType.ToString(),
                    DataAction = DataActionEnum.None.ToString(),
                    RequestSource = RequestSourceEnum.Main.ToString(),
                    RecordId = page.RecordId,
                    IconCss = page.IconCss.Coalesce("fa fa-user-circle"),
                    IsNotClickable = false,
                    IsClickDisabled = false
                });
                list.Add(new BreadcrumbViewModel
                {
                    Id = "Edit",
                    ParentId = page.Id,
                    Text = "Edit",
                    Name = $"Edit_{page.Name}",
                    PageId = page.Id,
                    PageType = page.PageType.ToString(),
                    DataAction = page.DataAction.ToString(),
                    RequestSource = page.RequestSource.ToString(),
                    RecordId = page.RecordId,
                    IconCss = page.IconCss.Coalesce("fa fa-user-circle"),
                    IsNotClickable = false,
                    IsClickDisabled = true
                });
            }
            else
            {
                list.Add(new BreadcrumbViewModel
                {
                    Id = page.Id,
                    ParentId = id,
                    Text = page.Title,
                    Name = page.Name,
                    PageId = page.Id,
                    PageType = page.PageType.ToString(),
                    DataAction = page.DataAction.ToString(),
                    RequestSource = page.RequestSource.ToString(),
                    RecordId = page.RecordId,
                    IconCss = page.IconCss.Coalesce("fa fa-user-circle"),
                    IsNotClickable = false,
                    IsClickDisabled = true
                });
            }
            return await Task.FromResult(list);
        }

        public async Task<List<TreeViewViewModel>> GetPortalTreeList(string id, string userId)
        {
            var list = new List<TreeViewViewModel>();
            var data = await GetList();
            if (id.IsNullOrEmpty())
            {
                var portals = await _repo.GetList<PortalViewModel, Portal>();
                if (portals != null && portals.Any())
                {
                    portals = portals.OrderBy(x => x.SequenceOrder).ToList();
                    list.AddRange(portals.Select(x => new TreeViewViewModel
                    {
                        id = string.Concat("Portal_", x.Id),
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = null,
                        hasChildren = true,
                        PortalId = x.Id,
                        Type = ContentTypeEnum.Portal.ToString(),
                        IconCss = x.PortalStatus.ToStatusIcon(),
                        IconTitle = x.PortalStatus.ToIconTitle(),
                        children = true,
                        text = x.Name,
                        parent = "#",
                        HideCheckbox = true,
                        a_attr = new { data_id = x.Id, data_name = x.Name, data_type = ContentTypeEnum.Portal.ToString() },
                    }));
                }

            }

            else
            {
                var ids = id.Split('_');
                id = ids[1];
                var portal = await _repo.GetSingle<PortalViewModel, Portal>(x => x.Id == id);
                if (portal.IsNotNull())
                {
                    var pages = await _repo.GetList<PageViewModel, Page>(x => x.PortalId == id);
                    var permission = await _permissionBusiness.GetList();
                    if (pages != null && pages.Any())
                    {
                        pages = pages.OrderBy(x => x.SequenceOrder).ToList();
                        list.AddRange(pages.Select(x => new TreeViewViewModel
                        {
                            id = string.Concat("Page_", x.Id),
                            Name = x.Name,
                            DisplayName = x.Title.Coalesce(x.Name),
                            ParentId = id,
                            PortalId = x.PortalId,
                            ItemLevel = x.Level,
                            hasChildren = true,
                            Type = x.PageType.ToString(),
                            children = true,
                            text = x.Name,
                            parent = id,
                            HideCheckbox = true,
                            a_attr = new { data_id = x.Id, data_name = x.Name, data_type = x.PageType.ToString() },
                        }));

                        list.ForEach(x => x.children = permission.Any(y => y.PageTypes.Contains(x.Type)));
                    }
                }
                else if (ids[0] == "Page")
                {
                    var page = await _repo.GetSingle<PageViewModel, Page>(x => x.Id == id);

                    var model = new UserPermissionViewModel { PageId = id, PageType = page.PageType };

                    var permission = await _permissionBusiness.GetList(x => x.PageTypes.Contains(page.PageType.ToString()) &&
                    x.UserPermissionTypes.Contains(UserPermissionTypeEnum.User.ToString()));


                    permission = permission.OrderBy(x => x.SequenceOrder).OrderBy(x => x.Name).ToList();

                    foreach (var per in permission)
                    {
                        var userPermission = await _userPermissionBusiness.GetSingle(x => x.UserId == userId && x.PageId == page.Id);
                        bool isChecked = userPermission.IsNotNull() && userPermission.Permissions.Contains(per.Code);
                        list.Add(new TreeViewViewModel
                        {
                            id = string.Concat(per.Id, "_", id),
                            Name = per.Name,
                            DisplayName = per.Name,
                            ParentId = id,
                            PageName = page.Name,
                            PortalId = page.PortalId,
                            hasChildren = false,
                            ItemLevel = 0,
                            Type = ContentTypeEnum.Permission.ToString(),
                            Checked = isChecked,
                            text = per.Name,
                            parent = id,
                            HideCheckbox = true,
                            a_attr = new { data_id = per.Id, data_name = per.Name, data_type = ContentTypeEnum.Permission.ToString() },
                        });
                    }
                }
            }
            return list;
        }

        public async Task<List<TreeViewViewModel>> GetPortalRoleTreeList(string id, string userRoleId)
        {
            var list = new List<TreeViewViewModel>();
            var data = await GetList();
            if (id.IsNullOrEmpty())
            {
                var portals = await _repo.GetList<PortalViewModel, Portal>();
                if (portals != null && portals.Any())
                {
                    portals = portals.OrderBy(x => x.SequenceOrder).ToList();
                    list.AddRange(portals.Select(x => new TreeViewViewModel
                    {
                        id = string.Concat("Portal_", x.Id),
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = null,
                        hasChildren = true,
                        PortalId = x.Id,
                        Type = ContentTypeEnum.Portal.ToString(),
                        IconCss = x.PortalStatus.ToStatusIcon(),
                        IconTitle = x.PortalStatus.ToIconTitle(),
                        children = true,
                        text = x.Name,
                        parent = "#",
                        HideCheckbox = true,
                        a_attr = new { data_id = x.Id, data_name = x.Name, data_type = "Portal" },
                    }));
                }

            }

            else
            {
                var ids = id.Split('_');
                id = ids[1];
                var portal = await _repo.GetSingle<PortalViewModel, Portal>(x => x.Id == id);
                if (portal.IsNotNull())
                {
                    var pages = await _repo.GetList<PageViewModel, Page>(x => x.PortalId == id);
                    var permission = await _permissionBusiness.GetList();
                    if (pages != null && pages.Any())
                    {
                        pages = pages.OrderBy(x => x.SequenceOrder).ToList();
                        list.AddRange(pages.Select(x => new TreeViewViewModel
                        {
                            id = string.Concat("Page_", x.Id),
                            Name = x.Name,
                            DisplayName = x.Title.Coalesce(x.Name),
                            ParentId = id,
                            PortalId = x.PortalId,
                            ItemLevel = x.Level,
                            hasChildren = true,
                            Type = x.PageType.ToString(),
                            children = true,
                            text = x.Name,
                            parent = id,
                            HideCheckbox = true,
                            a_attr = new { data_id = x.Id, data_name = x.Name, data_type = x.PageType.ToString() },
                        }));

                        list.ForEach(x => x.hasChildren = permission.Any(y => y.PageTypes.Contains(x.Type)));
                    }
                }
                else if (ids[0] == "Page")
                {
                    var page = await _repo.GetSingle<PageViewModel, Page>(x => x.Id == id);

                    var model = new UserRolePermissionViewModel { PageId = id, PortalPageType = page.PageType };

                    var permission = await _permissionBusiness.GetList(x => (x.PageTypes.Contains(page.PageType.ToString()) &&
                    x.UserPermissionTypes.Contains(UserPermissionTypeEnum.UserRole.ToString())) || x.PageId == id);


                    permission = permission.OrderBy(x => x.SequenceOrder).OrderBy(x => x.Name).ToList();

                    foreach (var per in permission)
                    {
                        var userPermission = await _userRolePermissionBusiness.GetSingle(x => x.UserRoleId == userRoleId && x.PageId == page.Id);
                        bool isChecked = userPermission.IsNotNull() && userPermission.Permissions.Contains(per.Code);
                        list.Add(new TreeViewViewModel
                        {
                            id = string.Concat(per.Id, "_", id),
                            Name = per.Name,
                            DisplayName = per.Name,
                            ParentId = id,
                            PortalId = page.PortalId,
                            hasChildren = false,
                            ItemLevel = 0,
                            Type = ContentTypeEnum.Permission.ToString(),
                            Checked = isChecked,
                            children = true,
                            text = per.Name,
                            parent = id,
                            HideCheckbox = true,
                            a_attr = new { data_id = per.Id, data_name = per.Name, data_type = ContentTypeEnum.Permission.ToString() },
                        });
                    }
                }
            }
            return list;
        }
        public async Task<List<TreeViewViewModel>> GetProcessDiagramTreeList(string id)
        {
            var list = new List<TreeViewViewModel>();
            var data = await GetList();

            if (id.IsNullOrEmpty())
            {
                var portals = await _repo.GetList<PortalViewModel, Portal>();
                if (portals != null && portals.Any())
                {
                    portals = portals.Where(x => x.Name != "CMS").OrderBy(x => x.SequenceOrder).ToList();
                    list.AddRange(portals.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = x.ParentId,
                        hasChildren = true,
                        Type = ContentTypeEnum.Portal.ToString(),
                        IconCss = x.PortalStatus.ToStatusIcon(),
                        IconTitle = x.PortalStatus.ToIconTitle(),
                    }));
                }
            }
            else
            {
                var menugroup = await _menuGroupBusiness.GetSingleById(id);
                if (menugroup == null)
                {
                    var page = await _repo.GetList<PageViewModel, Page>(x => x.PortalId == id, y => y.MenuGroup);
                    var groupedPages = page.GroupBy(x => x.MenuGroupId).Select(y => y.FirstOrDefault()).Select(p => p.MenuGroup).Distinct();
                    list.AddRange(groupedPages.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = id,
                        PortalId = x.PortalId,
                        hasChildren = true,
                        Type = ContentTypeEnum.MenuGroup.ToString(),
                        IconCss = "",
                        IconTitle = "",
                    }));
                }
                else
                {

                }

            }
            return list;
        }

        public async Task<List<PageViewModel>> GetPageOnMenuGroupAndPortalId(string menuGroupId)
        {
            var pages = await _repo.GetList<PageViewModel, Page>(x => x.MenuGroupId == menuGroupId);
            if (pages != null && pages.Any())
            {
                pages = pages.OrderBy(x => x.SequenceOrder).ToList();
                //list.AddRange(pages.Select(x => new TreeViewViewModel
                //{
                //    id = x.Id,
                //    Name = x.Name,
                //    DisplayName = x.Title.Coalesce(x.Name),
                //    //ParentId = x.ParentId,
                //    ParentId = id,
                //    PortalId = x.PortalId,
                //    ItemLevel = x.Level,
                //    hasChildren = false,
                //    Type = ContentTypeEnum.ContentPage.ToString()
                //}));
                //list.ForEach(x => x.hasChildren = data.Any(y => y.ParentId == x.id));
            }
            return pages.ToList();
        }

        public async Task<PageViewModel> GetPageDetailsByTemplate(string portalId, string templateCode)
        {
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == templateCode);
            if (template == null)
            {
                return null;
            }
            var result = await _repo.GetSingle<PageViewModel, Page>(x => x.TemplateId == template.Id && x.PortalId == portalId, x => x.Portal);
            return result;
        }
        public async Task<PageViewModel> GetDefaultPageByTemplate(string portalId, string templateCodes)
        {
            var templates = templateCodes.Split(",");
            if (templates == null || !templates.Any())
            {
                return null;
            }
            var templateCode = templates[0];
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == templateCode);
            if (template == null)
            {
                return null;
            }
            var pageCode = "";
            switch (template.TemplateType)
            {
                case TemplateTypeEnum.Page:
                    pageCode = "GENERAL_PAGE";
                    break;
                case TemplateTypeEnum.Form:
                    pageCode = "GENERAL_FORM";
                    break;
                case TemplateTypeEnum.Note:
                    pageCode = "GENERAL_NOTE";
                    break;
                case TemplateTypeEnum.Task:
                    pageCode = "GENERAL_TASK";
                    break;
                case TemplateTypeEnum.Service:
                    pageCode = "GENERAL_SERVICE";
                    break;
                default:
                    break;
            }
            var result = await _repo.GetSingle<PageViewModel, Page>(x => x.Name == pageCode && x.PortalId == portalId, x => x.Portal);
            if (result != null)
            {
                result.Template = template;
                var languages = await _repo.GetList<ResourceLanguageViewModel, ResourceLanguage>(x => x.TemplateType == TemplateTypeEnum.PortalPage && x.TemplateId == result.Id);
                if (languages != null)
                {
                    var title = languages.FirstOrDefault(x => x.Code == "PortalPageTitle");
                    if (title != null)
                    {
                        result.Title = title.GetLanguageValue(_repo.UserContext.CultureName);
                    }
                    var menuName = languages.FirstOrDefault(x => x.Code == "PortalPageMenuName");
                    if (menuName != null)
                    {
                        result.MenuName = menuName.GetLanguageValue(_repo.UserContext.CultureName);
                    }
                }
                result.TemplateId = template.Id;
            }
            return result;
        }

        public Task<PageViewModel> GetDefaultPageByCategory(string portalId, string categoryCodes)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IdNameViewModel>> GetPortalList()
        {
            var list = new List<IdNameViewModel>();
            var portals = await _repo.GetList<PortalViewModel, Portal>();
            if (portals != null && portals.Any())
            {
                //portals = portals.Where(x => x.Name != "CMS").OrderBy(x => x.SequenceOrder).ToList();
                list.AddRange(portals.Select(x => new IdNameViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).OrderBy(s => s.Name));
            }
            return list.ToList();
        }
        public async Task<List<PageViewModel>> GetMenuGroupPageList(string portalId)
        {
            var data = await _repo.GetList<PageViewModel, Page>(x => x.PortalId == portalId, y => y.MenuGroup);
            var result = new List<PageViewModel>();

            foreach (var cat in data.GroupBy(x => x.MenuGroupId))
            {
                var model = new List<PageViewModel>();

                foreach (var temp in data)
                {
                    if (temp.Id.IsNotNull() && cat.Any(x => x.MenuGroupId == temp.MenuGroupId))
                    {
                        model.Add(new PageViewModel()
                        {
                            Title = temp.Title,
                            Name = temp.Name,
                            TemplateType = temp.TemplateType,
                            Id = temp.Id
                        }
                        );
                    }
                }

                result.Add(new PageViewModel()
                {
                    MenuGroupId = cat.Key,
                    MenuGroupName = cat.FirstOrDefault().MenuGroupName,
                    PagesList = model,
                }
                );
            }

            return result;
        }

        public async Task<List<PageViewModel>> PortalDetails()
        {
            var res = await _cmsQueryBusiness.PortalDetails();
            return res;



        }

        public async Task<List<PageViewModel>> GetMenuGroupPageList2(string portalId)
        {
            //var data = await _repo.GetList<PageViewModel, Page>(x => x.PortalId == portalId, y => y.MenuGroup);
            var result = new List<PageViewModel>();
            var menuGroup = await _repo.GetList<MenuGroupViewModel, MenuGroup>(x => x.PortalId == portalId && x.Status == StatusEnum.Active && x.IsDeleted == false);

            foreach (var x in menuGroup)
            {
                var model = new List<PageViewModel>();
                var data1 = await _repo.GetList<PageViewModel, Page>(y => y.PortalId == portalId && y.MenuGroupId == x.Id && y.IsDeleted == false);
                foreach (var page in data1)
                {
                    model.Add(new PageViewModel()
                    {
                        Title = page.Title,
                        Name = page.Name,
                        TemplateType = page.TemplateType,
                        Id = page.Id,
                        MenuGroupId = page.Id,
                    });
                }

                result.Add(new PageViewModel()
                {
                    MenuGroupId = x.Id,
                    MenuGroupName = x.Name,
                    PagesList = model,
                    PortalId = portalId,
                }
                );

            }

            return result;
        }
    }
}
