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
    public interface IUserPermissionBusiness : IBusinessBase<UserPermissionViewModel, UserPermission>
    {
        Task<List<UserPermissionViewModel>> GetUserPermission(string pageId, TemplateTypeEnum pageType, string portalId);
        Task<CommandResult<UserPermissionViewModel>> SaveUserPermission(IList<UserPermissionViewModel> userPermissions);
        Task<CommandResult<UserPermissionViewModel>> EditUserPermission(UserPermissionViewModel data, bool autoCommit = true);
        Task<CommandResult<UserPermissionViewModel>> CreateUserPermission(UserPermissionViewModel data, bool autoCommit = true);
        Task<IList<UserEntityPermissionViewModel>> GetUserPermissionList(string userId, string userrole, string permission);
    }
}
