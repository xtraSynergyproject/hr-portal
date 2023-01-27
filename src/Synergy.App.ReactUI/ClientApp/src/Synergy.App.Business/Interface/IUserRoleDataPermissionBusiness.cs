using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IUserRoleDataPermissionBusiness : IBusinessBase<UserRoleDataPermissionViewModel, UserRoleDataPermission>
    {
        Task<IList<IdNameViewModel>> GetColunmMetadataListByPageForUserRole(string pageId);
        Task<IList<UserRoleDataPermissionViewModel>> GetUserRoleDataPermissionByPageId(string pageId,string portalId);

    }
}
