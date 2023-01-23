using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class TeamBusiness : BusinessBase<TeamViewModel, Team>, ITeamBusiness
    {
        private ITeamUserBusiness _teamuserBusiness;
        private readonly IRepositoryQueryBase<TeamViewModel> _queryRepo;
        // private ITeamBusiness _teamBusiness;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        private readonly IUserContext _userContext;
        public TeamBusiness(IRepositoryBase<TeamViewModel, Team> repo, IMapper autoMapper, ITeamUserBusiness teamuserBusiness
            , IRepositoryQueryBase<TeamViewModel> queryRepo, ICmsQueryBusiness cmsQueryBusiness,
            IUserContext userContext) : base(repo, autoMapper)
        {
            _teamuserBusiness = teamuserBusiness;
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
            _userContext = userContext;
        }

        public async override Task<CommandResult<TeamViewModel>> Create(TeamViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<TeamViewModel>(model);
            
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TeamViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data, autoCommit);
            if (model.UserIds != null)
            {
                foreach (var id in model.UserIds)
                {

                    var team = new TeamUserViewModel();
                    if (model.TeamOwnerId == id)
                    { team.IsTeamOwner = true; }
                    else { team.IsTeamOwner = false; }
                    team.TeamId = result.Item.Id;
                    team.UserId = id;
                    await _teamuserBusiness.Create(team);

                }
            }

            return CommandResult<TeamViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TeamViewModel>> Edit(TeamViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<TeamViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TeamViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model,autoCommit);
            var pagecol = await _teamuserBusiness.GetList(x => x.TeamId == model.Id);
            var existingTeamOwner = pagecol.Where(x => x.IsTeamOwner == true);
            
            var existingIds = pagecol.Select(x => x.UserId);
            var newIds = model.UserIds;
            //var ToDelete = existingIds.Except(newIds).ToList();
            //var ToAdd = newIds.Except(existingIds).ToList();
            var ToDelete = new List<string>();
            var ToAdd = new List<string>();
            if (existingIds.IsNotNull() && existingIds.Count()>0)
            {
                ToDelete = existingIds.Except(newIds).ToList();
            }
            if (newIds.IsNotNull() && newIds.Count()>0)
            {
                ToAdd = newIds.Except(existingIds).ToList();
            }
            // Add
            if (ToAdd.Count>0)
            {
                foreach (var id in ToAdd)
                {

                    var team = new TeamUserViewModel();
                    if (model.TeamOwnerId == id)
                    { team.IsTeamOwner = true; }
                    else { team.IsTeamOwner = false; }
                    team.TeamId = result.Item.Id;
                    team.UserId = id;
                    await _teamuserBusiness.Create(team);
                }
            }
            if(existingTeamOwner.FirstOrDefault().UserId != model.TeamOwnerId)
            {
                var teamuser = await _teamuserBusiness.GetSingle(x=>x.UserId==existingTeamOwner.FirstOrDefault().UserId && x.TeamId==model.Id);
                teamuser.IsTeamOwner = false;
                await _teamuserBusiness.Edit(teamuser);

                var newOwner = await _teamuserBusiness.GetSingle(x=>x.UserId==model.TeamOwnerId && x.TeamId==model.Id);
                newOwner.IsTeamOwner = true;
                await _teamuserBusiness.Edit(newOwner);
            }

            // Delete
            foreach (var id in ToDelete)
            {
                var role = await _teamuserBusiness.GetSingle(x => x.TeamId == model.Id && x.UserId == id);
                await _teamuserBusiness.Delete(role.Id);
            }
           
           



            return CommandResult<TeamViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<TeamViewModel>> IsNameExists(TeamViewModel model)
        {

            var errorList = new Dictionary<string, string>();
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Name is required.");
            }
            else
            {
                var name = await _repo.GetSingle(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");
                }
            }
            if (!model.Code.IsNullOrEmpty())
            
               
            {
                var name = await _repo.GetSingle(x => x.Code.ToLower() == model.Code.ToLower() && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Code", "Code already exist.");
                }
            }

            if (errorList.Count > 0)
            {
                return CommandResult<TeamViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<TeamViewModel>.Instance();
        }
        public async Task<IList<TeamViewModel>> GetTeamByOwner(string userId)
        {
            var result = await _cmsQueryBusiness.GetTeamByOwnerData(userId);
            return result;
        }
        public async Task<IList<TeamViewModel>> GetTeamByUser(string userId)
        {
            var result = await _cmsQueryBusiness.GetTeamByUserData(userId);
            return result;
        }
        public async Task<IList<TeamViewModel>> GetTeamMemberList(string teamId)
        {
            var result = await _cmsQueryBusiness.GetTeamMemberListData(teamId);            
            return result;
        }
        public async Task<TeamViewModel> GetTeamOwner(string teamId)
        {
            var result = await _cmsQueryBusiness.GetTeamOwnerData(teamId);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetTeamByGroupCode(string groupCode)
        {
            var result = await _cmsQueryBusiness.GetTeamByGroupCodeData(groupCode);
            return result;
        }

        public async Task<IList<IdNameViewModel>> GetTeamUsersByCode(string Code)
        {
            var result = await _cmsQueryBusiness.GetTeamUsersByCodeData(Code);
            return result;
        }

        public async Task<List<TeamViewModel>> GetTeamWithPortalIds()
        {
            var list = await _cmsQueryBusiness.GetTeamWithPortalIdsData();
            return list;

        }
        public async Task<List<TeamViewModel>> GetTeamsBasedOnAllowedPortals(string portalId)
        {
            var list = await _cmsQueryBusiness.GetTeamsBasedOnAllowedPortalsData(portalId);
            return list;

        }

    }
}
