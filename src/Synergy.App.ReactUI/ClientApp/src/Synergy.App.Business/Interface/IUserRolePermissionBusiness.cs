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
    public interface IUserRolePermissionBusiness : IBusinessBase<UserRolePermissionViewModel, UserRolePermission>
    {
        Task<List<UserRolePermissionViewModel>> GetUserRolePermission(string pageId, TemplateTypeEnum pageType, string portalId);
        Task<CommandResult<UserRolePermissionViewModel>> SaveUserRolePermission(IList<UserRolePermissionViewModel> list);
        Task<CommandResult<UserRolePermissionViewModel>> CreateUserRolePermission(UserRolePermissionViewModel model, bool autoCommit = true);
        Task<CommandResult<UserRolePermissionViewModel>> EditUserRolePermission(UserRolePermissionViewModel model, bool autoCommit = true);
        Task<IList<UserEntityPermissionViewModel>> GetUserRolePermissionList(string userRole);

    }
}
