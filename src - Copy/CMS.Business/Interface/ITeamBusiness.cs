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
    public interface ITeamBusiness : IBusinessBase<TeamViewModel, Team>
    {
        Task<IList<TeamViewModel>> GetTeamByOwner(string userId);
        Task<IList<TeamViewModel>> GetTeamMemberList(string teamId);
        Task<IList<TeamViewModel>> GetTeamByUser(string userId);
        Task<IList<IdNameViewModel>> GetTeamByGroupCode(string groupCode);
        Task<TeamViewModel> GetTeamOwner(string teamId);
        Task<IList<IdNameViewModel>> GetTeamUsersByCode(string Code);
        Task<List<TeamViewModel>> GetTeamWithPortalIds();
    }
}
