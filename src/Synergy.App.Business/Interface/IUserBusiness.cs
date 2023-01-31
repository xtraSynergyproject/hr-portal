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
    public interface IUserBusiness : IBusinessBase<UserViewModel, User>
    {
        Task<UserViewModel> ValidateLogin(string email, string secret);
        Task<CommandResult<ChangePassowrdViewModel>> ChangePassword(ChangePassowrdViewModel model);
        Task ChangeUserProfilePhoto(string photoId, string userId);
        Task<List<IdNameViewModel>> GetUserIdNameList();
        Task<List<IdNameViewModel>> GetUserIdNameListForPortal();
        Task<UserViewModel> GetGuestUser(string companyId);
        Task<UserViewModel> ValidateUser(string email, string legalEntityId = null);
        //Task<CommandResult<UserViewModel>> CreateUser(UserViewModel model);
        Task<CommandResult<UserViewModel>> EditUser(UserViewModel model, bool autoCommit = true);
        Task<UserViewModel> ConfirmEmailOTP(string email);
        Task<UserViewModel> ConfirmPasswordChange(string email, string password);
        Task ChangeUserSignature(string signId, string userId);
        Task<IList<UserViewModel>> GetUserList();
        Task<IList<UserViewModel>> GetUserListForPortal();
        Task<IList<UserViewModel>> GetActiveUserList();
        Task<IList<UserViewModel>> GetActiveUserListForSwitchProfile();
        Task<IList<UserViewModel>> GetActiveUserListForPortal();
        Task<IList<UserViewModel>> GetUserTeamList(string id);
        Task<IList<UserViewModel>> GetUserTeamListForPortal();
        Task<IList<UserViewModel>> GetUserListForEmailSummary();
        Task<IList<UserViewModel>> GetSwitchToUserList(string userId, string loggedinAsByUserId, bool hasLoggedInAsPermission, string searchParam);
        Task SendWelcomeEmail(UserViewModel model);
        Task<IdNameViewModel> GetPersonWithSponsor(string userId);
        Task<IList<UserListOfValue>> ViewModelList(string userId);
        Task<Tuple<string, string, string>> GetHierarchyRootId(string userId, string hierarchyCode, string userHierarchyId, DateTime? asofDate = null);
        Task<List<double>> GetUserNodeLevel(string userId);
        Task<List<PositionChartViewModel>> GetUserHierarchy(string parentId, int levelUpto, string hierarchyId, int level = 1, int option = 1);
        Task<string> GetPersonDateOfJoining(string userId);
        Task<List<LegalEntityViewModel>> GetEntityByIds(string legalEntity);
        Task<UserViewModel> ValidateUserById(string id, string legalEntityId = null);
        Task<UserViewModel> ValidateUserByUserId(string userId, string legalEntityId = null);
        Task UpdateHierarchyLevel(string hierarchyId, string userId, string levelUserId, int levelNo, int optionNo, DateTime esd, DateTime eed);
        Task<IList<UserViewModel>> GetUserListWithEmailText();
        Task<List<UserPermissionViewModel>> ViewUserPermissions(string userId);
        Task<List<UserViewModel>> GetAllUsersWithPortalId(string PortalId);
        Task<List<UserViewModel>> GetAllCSCUsersList();
        Task<List<UserViewModel>> GetUsersWithPortalIds();
        Task<List<UserViewModel>> GetDMSPermiisionusersList(string PortalId, string LegalEntityId);
        Task<List<PortalViewModel>> GetAllowedPortalList(string userId);
        Task CreateUserWorkSpace(string userId);
        Task<Tuple<string, string, string>> GetUserHierarchyRootId(string userId, string hierarchyCode, string userHierarchyId, DateTime? asofDate = null);
        Task<UserViewModel> TwoFactorAuthOTP(string email);
        Task<PagedList<UserViewModel>> GetSwitchUserList(string userId, string loggedinAsByUserId, bool hasLoggedInAsPermission, string filter, int pageSize, int pageNumber, string indexedItemId = null);
        Task<List<PositionChartViewModel>> GetUserHierarchyChartData(string parentId, int levelUpto, string c, int level = 1, int option = 1);
        Task<PageViewModel> GetUserLandingPage(string userId, string portalId);
        Task<List<UserPreferenceViewModel>> GetUserPreferences(string userId);

    }
}
