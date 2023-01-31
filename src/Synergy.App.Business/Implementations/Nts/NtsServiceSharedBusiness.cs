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
    public class NtsServiceSharedBusiness : BusinessBase<NtsServiceSharedViewModel, NtsServiceShared>, INtsServiceSharedBusiness
    {
        private readonly IRepositoryQueryBase<NtsServiceSharedViewModel> _queryRepo;
        private readonly INtsQueryBusiness _ntsQueryBusiness;

        public NtsServiceSharedBusiness(IRepositoryBase<NtsServiceSharedViewModel, NtsServiceShared> repo, IMapper autoMapper, IRepositoryQueryBase<NtsServiceSharedViewModel> queryRepo
            , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _ntsQueryBusiness = ntsQueryBusiness;


        }

        public async override Task<CommandResult<NtsServiceSharedViewModel>> Create(NtsServiceSharedViewModel model, bool autoCommit = true)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);

            var result = await base.Create(model,autoCommit);
            return CommandResult<NtsServiceSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsServiceSharedViewModel>> Edit(NtsServiceSharedViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            return CommandResult<NtsServiceSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


        public async Task<List<NtsServiceSharedViewModel>> GetSearchResult(string ServiceId)
        {

            var list = await _ntsQueryBusiness.GetSearchSharedResult(ServiceId);
            return list;
        }



    }
}
