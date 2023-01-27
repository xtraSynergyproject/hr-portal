using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IUserRolePortalBusiness : IBusinessBase<UserRolePortalViewModel, UserRolePortal>
    {
        Task<List<UserRolePortalViewModel>> GetPortalByUserRole(string userRoleId);
        Task<List<UserRoleViewModel>> GetUserRoleByPortal(string portalId);
    }
}
