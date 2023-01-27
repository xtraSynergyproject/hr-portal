using AutoMapper;
using Synergy.App.Business.Interface;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class NtsGroupUserGroupBusiness : BusinessBase<NtsGroupUserGroupViewModel, NtsGroupUserGroup>, INtsGroupUserGroupBusiness
    {

        public NtsGroupUserGroupBusiness(IRepositoryBase<NtsGroupUserGroupViewModel, NtsGroupUserGroup> repo, IMapper autoMapper) : base(repo, autoMapper)
        {


        }

        public async override Task<CommandResult<NtsGroupUserGroupViewModel>> Create(NtsGroupUserGroupViewModel model, bool autoCommit = true)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);

            var result = await base.Create(model,autoCommit);
            return CommandResult<NtsGroupUserGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsGroupUserGroupViewModel>> Edit(NtsGroupUserGroupViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            return CommandResult<NtsGroupUserGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
