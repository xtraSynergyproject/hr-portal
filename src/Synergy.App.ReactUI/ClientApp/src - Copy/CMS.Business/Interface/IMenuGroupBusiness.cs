using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IMenuGroupBusiness : IBusinessBase<MenuGroupViewModel,MenuGroup>
    {
        Task<List<MenuGroupViewModel>> GetMenuGroupList();
        Task<List<MenuGroupViewModel>> GetMenuGroupListPortalAdmin(string PortalId, string LegalEntityId);
        Task<List<MenuGroupViewModel>> GetMenuGroupListparentPortalAdmin(string PortalId, string LegalEntityId, string Id);
    }
}
