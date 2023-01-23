using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IUserRoleBusiness : IBusinessBase<UserRoleViewModel,UserRole>
    {
        Task<List<UserViewModel>> GetUserRoleForUser(string id);
        Task<List<UserRoleViewModel>> GetUserRolesWithPortalIds();
        Task<List<UserRolePreferenceViewModel>> GetUserRolePreferences(string userRoleId);

    }
}
