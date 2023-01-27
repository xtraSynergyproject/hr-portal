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
    public interface IUserHierarchyPermissionBusiness : IBusinessBase<UserHierarchyPermissionViewModel, UserHierarchyPermission>
    {
        //Task<IList<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userId);
        Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userId);
        Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchyForPortal(string userId);
        Task<List<IdNameViewModel>> GetUserNotInPermissionHierarchy(string UserId);
        Task<List<IdNameViewModel>> GetHierarchyData(HierarchyTypeEnum userId);

    }
}
