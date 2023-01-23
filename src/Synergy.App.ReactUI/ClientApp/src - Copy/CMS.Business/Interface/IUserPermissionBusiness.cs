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
    public interface IUserPermissionBusiness : IBusinessBase<UserPermissionViewModel, UserPermission>
    {
        Task<List<UserPermissionViewModel>> GetUserPermission(string pageId, TemplateTypeEnum pageType, string portalId);
        Task<CommandResult<UserPermissionViewModel>> SaveUserPermission(IList<UserPermissionViewModel> userPermissions);
        Task<CommandResult<UserPermissionViewModel>> EditUserPermission(UserPermissionViewModel data);
        Task<CommandResult<UserPermissionViewModel>> CreateUserPermission(UserPermissionViewModel data);
    }
}
