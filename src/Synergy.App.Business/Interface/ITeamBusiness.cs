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
    public interface ITeamBusiness : IBusinessBase<TeamViewModel, Team>
    {
        Task<IList<TeamViewModel>> GetTeamByOwner(string userId);
        Task<IList<TeamViewModel>> GetTeamMemberList(string teamId);
        Task<IList<TeamViewModel>> GetTeamByUser(string userId);
        Task<IList<IdNameViewModel>> GetTeamByGroupCode(string groupCode);
        Task<TeamViewModel> GetTeamOwner(string teamId);
        Task<IList<IdNameViewModel>> GetTeamUsersByCode(string Code);
        Task<List<TeamViewModel>> GetTeamWithPortalIds();
        Task<List<TeamViewModel>> GetTeamsBasedOnAllowedPortals(string portalId);
    }
}
