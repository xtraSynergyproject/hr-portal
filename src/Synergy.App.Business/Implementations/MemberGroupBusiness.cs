using AutoMapper;
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
    public class MemberGroupBusiness : BusinessBase<MemberGroupViewModel, MemberGroup>, IMemberGroupBusiness
    {
        public MemberGroupBusiness(IRepositoryBase<MemberGroupViewModel, MemberGroup> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<MemberGroupViewModel>> Create(MemberGroupViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<MemberGroupViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<MemberGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<MemberGroupViewModel>> Edit(MemberGroupViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<MemberGroupViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<MemberGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

      
    }
}
