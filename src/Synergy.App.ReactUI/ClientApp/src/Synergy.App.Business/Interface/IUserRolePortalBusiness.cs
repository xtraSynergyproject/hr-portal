using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IUserRolePortalBusiness : IBusinessBase<UserRolePortalViewModel, UserRolePortal>
    {
        Task<List<UserRolePortalViewModel>> GetPortalByUserRole(string userRoleId);
        Task<List<UserRoleViewModel>> GetUserRoleByPortal(string portalId);
    }
}
