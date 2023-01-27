using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IPortalBusiness : IBusinessBase<PortalViewModel, Portal>
    {
        Task<List<MenuViewModel>> GetMenuItems(Portal portal, string userId, string companyId);
        Task<List<MenuViewModel>> GetPortalDiagramData(PortalViewModel portal, string userId, string companyId);
        Task<PortalViewModel> GetPortalByDomain(string domain);
        Task<List<IdNameViewModel>> GetPortalForUser(string id);
        Task<List<PageViewModel>> GetMenuGroupOfPortal(string portalIds);
        Task<List<TreeViewViewModel>> GetNtsTemplateTreeList(string id, string[] portalIds);
        Task<List<IdNameViewModel>> GetPortals(string id);
        Task<List<IdNameViewModel>> GetAllowedPortals();
    }
}
