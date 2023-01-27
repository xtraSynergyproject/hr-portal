using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class TeamBusiness : BusinessBase<TeamViewModel, Team>, ITeamBusiness
    {
        private ITeamUserBusiness _teamuserBusiness;
        private readonly IRepositoryQueryBase<TeamViewModel> _queryRepo;
        // private ITeamBusiness _teamBusiness;
        public TeamBusiness(IRepositoryBase<TeamViewModel, Team> repo, IMapper autoMapper, ITeamUserBusiness teamuserBusiness
            , IRepositoryQueryBase<TeamViewModel> queryRepo) : base(repo, autoMapper)
        {
            _teamuserBusiness = teamuserBusiness;
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<TeamViewModel>> Create(TeamViewModel model)
        {

            var data = _autoMapper.Map<TeamViewModel>(model);
            
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TeamViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data);
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

        public async override Task<CommandResult<TeamViewModel>> Edit(TeamViewModel model)
        {

            var data = _autoMapper.Map<TeamViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TeamViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);
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
            var cypher = $@" select t.""Id"" as Id,t.""Name"" as Name  from 
public.""User"" as u 
join public.""TeamUser"" as tu on tu.""UserId""=u.""Id"" and tu.""IsTeamOwner""=true and tu.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Team"" as t on t.""Id""=tu.""TeamId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""Id""='{userId}' and  u.""IsDeleted""=false

               ";
            //            var cypher = @" match (u:ADM_User{Id:{UserId},IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})-[r:R_Team_User]-(t:ADM_Team{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //optional match(t1:ADM_Team{IsDeleted:0,Status:'Active',CompanyId:{CompanyId}})-[R_Team_User]-(u1:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}}) where t1.Id = t.Id

            //                with t.Id as Id,t.Name as Name, u.Id as UserId, t1, u1, r     where   r.IsTeamOwner=true         
            //return Id as Id, Name as Name, count(u1.Id) as TeamMemberCount, r.IsTeamOwner as IsTeamOwner
            //               ";
            
            var result = await _queryRepo.ExecuteQueryList(cypher, null);
            return result;
        }
        public async Task<IList<TeamViewModel>> GetTeamByUser(string userId)
        {
            var cypher = $@" select t.""Id"" as Id,t.""Name"" as Name  from 
public.""User"" as u 
join public.""TeamUser"" as tu on tu.""UserId""=u.""Id""  and tu.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Team"" as t on t.""Id""=tu.""TeamId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""Id""='{userId}'

               ";
          
            var result = await _queryRepo.ExecuteQueryList(cypher, null);
            return result;
        }
        public async Task<IList<TeamViewModel>> GetTeamMemberList(string teamId)
        {
            var cypher = $@" select u.""Id"" as Id, u.""Name"" as Name,tu.""IsTeamOwner"" as IsTeamOwner
from public.""Team"" as t
join public.""TeamUser"" as tu on tu.""TeamId""=t.""Id"" and t.""Id""='{teamId}' and t.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' where t.""IsDeleted""=false and tu.""IsDeleted""=false and u.""IsDeleted""=false
               ";
            var result = await _queryRepo.ExecuteQueryList(cypher, null);
            return result;
        }
        public async Task<TeamViewModel> GetTeamOwner(string teamId)
        {
            var cypher = $@" select u.""Id"" as Id, u.""Name"" as Name
from public.""Team"" as t
join public.""TeamUser"" as tu on tu.""TeamId""=t.""Id"" and t.""Id""='{teamId}' and t.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' where t.""IsDeleted""=false and tu.""IsDeleted""=false and u.""IsDeleted""=false and tu.""IsTeamOwner""=true
               ";
            var result = await _queryRepo.ExecuteQuerySingle(cypher, null);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetTeamByGroupCode(string groupCode)
        {
            //var result = new List<IdNameViewModel>();
            var query = $@" select t.""Id"" as Id,t.""Name"" as Name  from 
                            public.""Team"" as t 
                            where t.""IsDeleted""=false and t.""GroupCode""='{groupCode}' ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<IList<IdNameViewModel>> GetTeamUsersByCode(string Code)
        {
            //var result = new List<IdNameViewModel>();
            var query = $@" select u.""Id"" as Id,u.""Name"" as Name  from 
                            public.""Team"" as t 
join public.""TeamUser"" as tu on tu.""TeamId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
                            where t.""IsDeleted""=false and t.""Code""='{Code}' ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<TeamViewModel>> GetTeamWithPortalIds()
        {

            var Query = $@"SELECT distinct u.* FROM public.""Team"" as u 
            join public.""Company"" as c on c.""Id""='{_repo.UserContext.CompanyId}' and array[u.""AllowedPortalIds""] <@ array[c.""LicensedPortalIds""] and c.""IsDeleted""=false
            where u.""IsDeleted""=false and u.""AllowedPortalIds"" is not null";
            var list = await _queryRepo.ExecuteQueryList<TeamViewModel>(Query, null);
            return list;

        }

    }
}
