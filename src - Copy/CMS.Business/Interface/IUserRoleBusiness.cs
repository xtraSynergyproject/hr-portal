using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IUserRoleBusiness : IBusinessBase<UserRoleViewModel,UserRole>
    {
        Task<List<UserViewModel>> GetUserRoleForUser(string id);
        Task<List<UserRoleViewModel>> GetUserRolesWithPortalIds();

    }
}
