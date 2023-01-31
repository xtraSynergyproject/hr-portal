using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IPageBusiness : IBusinessBase<PageViewModel, Page>
    {
        Task<List<TreeViewViewModel>> GetContentTreeList(string id);
        Task<PageViewModel> GetPageData(string pageId);

        Task<PageViewModel> GetDefaultPageDataByPortal(PortalViewModel portal, string userId, RunningModeEnum runningMode);
        Task<PageViewModel> GetPageDataForExecution(string portalId, string pageName, RunningModeEnum runningMode);
        Task<PageViewModel> GetPageDataForExecution(string pageId);
        Task<List<PageContentViewModel>> GetContentChildList(string id, PageContentTypeEnum type);
        Task<PageViewModel> GetUserPagePermission(string portalId, string pageId);

        Task<PageViewModel> GetPageForExecution(string pageId);
        Task<PageViewModel> GetPageForExecution(string portalId, string pageName);
        Task<List<BreadcrumbViewModel>> GetBreadcrumbs(PageViewModel page);
        Task<List<TreeViewViewModel>> GetPortalTreeList(string id, string userId);
        Task<List<TreeViewViewModel>> GetProcessDiagramTreeList(string id);
        Task<List<PageViewModel>> GetPageOnMenuGroupAndPortalId(string menuGroupId);
        Task<PageViewModel> GetPageDetailsByTemplate(string portalId, string templateCode);
        Task<PageViewModel> GetDefaultPageByTemplate(string portalId, string templateCodes);
        Task<PageViewModel> GetDefaultPageByCategory(string portalId, string categoryCodes);
        Task<List<IdNameViewModel>> GetPortalList();
        Task<List<TreeViewViewModel>> GetPortalRoleTreeList(string id, string userRoleId);
        Task<List<PageViewModel>> GetMenuGroupPageList(string portalId);
        Task<List<PageViewModel>> PortalDetails();
        Task<List<PageViewModel>> GetMenuGroupPageList2(string portalId);
    }
}
