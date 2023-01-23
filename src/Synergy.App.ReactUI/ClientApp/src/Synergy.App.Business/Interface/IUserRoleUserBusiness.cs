using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IUserRoleUserBusiness : IBusinessBase<UserRoleUserViewModel,UserRoleUser>
    {
        Task<List<UserRoleUserViewModel>> GetUserRoleByUser(string userId);

    }
}
