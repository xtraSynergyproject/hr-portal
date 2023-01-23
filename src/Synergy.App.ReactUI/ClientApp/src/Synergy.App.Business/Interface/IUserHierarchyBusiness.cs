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
    public interface IUserHierarchyBusiness : IBusinessBase<UserHierarchyViewModel, UserHierarchy>
    {
        Task<IList<UserHierarchyViewModel>> GetHierarchyList(string HierarchyId,string userId);
        Task<CommandResult<UserHierarchyViewModel>> CreateUserHierarchy(UserHierarchyViewModel viewModel);
        Task<CommandResult<UserHierarchyViewModel>> CreateUserHierarchyForPortal(UserHierarchyViewModel viewModel);
        Task<IList<UserHierarchyViewModel>> GetLeaveApprovalHierarchyDetailsOfUser(string userId);
        Task<CommandResult<UserHierarchyViewModel>> UpdateHierarchyLevelForPortal(string hierarchyId, string userId, string levelUserId, int levelNo, int optionNo);
        Task<IList<UserHierarchyViewModel>> GetHierarchyListForPortal(string HierarchyId, string userId);
        Task<IList<UserHierarchyViewModel>> GetHierarchyListForAllPortals(string HierarchyId);
        Task<UserHierarchyViewModel> GetLeaveApprovalHierarchyUser(string userId, string hierarchyId);
        Task<UserHierarchyViewModel> GetHierarchyUser(string HierarchyId, string userId);
        Task<List<UserViewModel>> GetHierarchyUsers(string hierarchyCode, string parentUserId, int level, int option);
        Task<List<UserHierarchyViewModel>> GetPerformanceHierarchyUsers(string parentUserId);
        Task<List<IdNameViewModel>> GetNonExistingUser(string hierarchyId, string userId);
        Task<List<UserHierarchyViewModel>> GetUserHierarchyByCode(string code, string parentUserId);
    }
}
