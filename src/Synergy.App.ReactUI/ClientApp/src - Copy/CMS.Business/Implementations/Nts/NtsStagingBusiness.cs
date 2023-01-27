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
    public class NtsStagingBusiness : BusinessBase<NtsStagingViewModel, NtsStaging>, INtsStagingBusiness
    {
        private readonly IRepositoryQueryBase<NtsStagingViewModel> _queryRepo;
        public NtsStagingBusiness(IRepositoryBase<NtsStagingViewModel, NtsStaging> repo, IMapper autoMapper,
            IRepositoryQueryBase<NtsStagingViewModel> queryRepo)
            : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }             
        public async override Task<CommandResult<NtsStagingViewModel>> Create(NtsStagingViewModel model)
        {
            
            var result = await base.Create(model);           

            return CommandResult<NtsStagingViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async override Task<CommandResult<NtsStagingViewModel>> Edit(NtsStagingViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<NtsStagingViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<bool> UpdateStagingByBatchId(string batchId)
        {
            var query = @$" update public.""NtsStaging"" set ""StageStatus""=1
                        where ""BatchId""='{batchId}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
    }
}
