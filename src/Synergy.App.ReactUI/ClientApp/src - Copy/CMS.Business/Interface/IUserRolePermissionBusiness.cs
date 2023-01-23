using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IUserRolePermissionBusiness : IBusinessBase<UserRolePermissionViewModel,UserRolePermission>
    {
        Task<List<UserRolePermissionViewModel>> GetUserRolePermission(string pageId, TemplateTypeEnum pageType, string portalId);
        Task<CommandResult<UserRolePermissionViewModel>> SaveUserRolePermission(IList<UserRolePermissionViewModel> list);
        Task<CommandResult<UserRolePermissionViewModel>> CreateUserRolePermission(UserRolePermissionViewModel model);
        Task<CommandResult<UserRolePermissionViewModel>> EditUserRolePermission(UserRolePermissionViewModel model);

    }
}
