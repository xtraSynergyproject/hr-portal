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
    public class NtsTaskSharedBusiness : BusinessBase<NtsTaskSharedViewModel, NtsTaskShared>, INtsTaskSharedBusiness
    {
        private readonly IRepositoryQueryBase<NtsTaskSharedViewModel> _queryRepo;
        private readonly INtsQueryBusiness _ntsQueryBusiness;

        public NtsTaskSharedBusiness(IRepositoryBase<NtsTaskSharedViewModel, NtsTaskShared> repo, IMapper autoMapper, IRepositoryQueryBase<NtsTaskSharedViewModel> queryRepo
            , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _ntsQueryBusiness = ntsQueryBusiness;




        }

        public async override Task<CommandResult<NtsTaskSharedViewModel>> Create(NtsTaskSharedViewModel model, bool autoCommit = true)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           
            var result = await base.Create(model,autoCommit);           
            return CommandResult<NtsTaskSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsTaskSharedViewModel>> Edit(NtsTaskSharedViewModel model, bool autoCommit = true)
        {  
            var result = await base.Edit(model,autoCommit);           
            return CommandResult<NtsTaskSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<NtsTaskSharedViewModel>> GetSearchResult(string TaskId)
        {
            var list = await _ntsQueryBusiness.GetSearchTaskCommentResult(TaskId);
            return list;
        }
    


    }
}
