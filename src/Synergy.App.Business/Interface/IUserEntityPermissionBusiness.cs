using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IUserEntityPermissionBusiness : IBusinessBase<UserEntityPermissionViewModel, UserEntityPermission>
    {
        //Task<IList<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userId);
        //Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userId);

    }
}
