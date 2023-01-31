using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IPortalBusiness : IBusinessBase<PortalViewModel, Portal>
    {
        Task<List<MenuViewModel>> GetMenuItems(Portal portal, string userId, string companyId,string groupCode=null);
        Task<List<MenuViewModel>> GetPortalDiagramData(PortalViewModel portal, string userId, string companyId);
        Task<PortalViewModel> GetPortalByDomain(string domain);
        Task<List<IdNameViewModel>> GetPortalForUser(string id);
        Task<List<PageViewModel>> GetMenuGroupOfPortal(string portalIds);
        Task<List<TreeViewViewModel>> GetNtsTemplateTreeList(string id, string[] portalIds);
        Task<List<IdNameViewModel>> GetPortals(string id);
        Task<List<PortalViewModel>> GetPortalListByPortalNames(string portalNames);
        Task<List<IdNameViewModel>> GetAllowedPortals();
        Task<List<TeamViewModel>> GetTeamWithUsersByPortalId(string portalId, string teamIds);
    }
}
