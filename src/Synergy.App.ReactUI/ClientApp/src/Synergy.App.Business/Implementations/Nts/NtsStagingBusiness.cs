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
    public class NtsStagingBusiness : BusinessBase<NtsStagingViewModel, NtsStaging>, INtsStagingBusiness
    {
        private readonly IRepositoryQueryBase<NtsStagingViewModel> _queryRepo;
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        public NtsStagingBusiness(IRepositoryBase<NtsStagingViewModel, NtsStaging> repo, IMapper autoMapper,
            IRepositoryQueryBase<NtsStagingViewModel> queryRepo, INtsQueryBusiness ntsQueryBusiness)
            : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _ntsQueryBusiness = ntsQueryBusiness;
        }             
        public async override Task<CommandResult<NtsStagingViewModel>> Create(NtsStagingViewModel model, bool autoCommit = true)
        {
            
            var result = await base.Create(model,autoCommit);           

            return CommandResult<NtsStagingViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async override Task<CommandResult<NtsStagingViewModel>> Edit(NtsStagingViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            return CommandResult<NtsStagingViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<bool> UpdateStagingByBatchId(string batchId)
        {
           var data= await _ntsQueryBusiness.UpdateStagingByBatchIdData(batchId);
            return true;

        }
    }
}
