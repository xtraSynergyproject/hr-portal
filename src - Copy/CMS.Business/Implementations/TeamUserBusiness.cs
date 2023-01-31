using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class TeamUserBusiness : BusinessBase<TeamUserViewModel, TeamUser>, ITeamUserBusiness
    {
        public TeamUserBusiness(IRepositoryBase<TeamUserViewModel, TeamUser> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<TeamUserViewModel>> Create(TeamUserViewModel model)
        {

           // var data = _autoMapper.Map<TeamUserViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<TeamUserViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model);

            return CommandResult<TeamUserViewModel>.Instance(model);
        }

        public async override Task<CommandResult<TeamUserViewModel>> Edit(TeamUserViewModel model)
        {

            var data = _autoMapper.Map<TeamUserViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<TeamUserViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(data);

            return CommandResult<TeamUserViewModel>.Instance(model);
        }
        private async Task<CommandResult<TeamUserViewModel>> IsNameExists(TeamUserViewModel model)
        {

            //var errorList = new Dictionary<string, string>();

            //if (model.Name != null || model.Name != "")
            //{
            //    var name = await _repo.GetSingle(x => x.Name == model.Name && x.Id != model.Id);
            //    if (name != null)
            //    {
            //        errorList.Add("Name", "Name already exist.");
            //    }
            //}
            //if (errorList.Count > 0)
            //{
            //    return CommandResult<TeamUserViewModel>.Instance(model, false, errorList);
            //}

            return CommandResult<TeamUserViewModel>.Instance();
        }

    }
}
