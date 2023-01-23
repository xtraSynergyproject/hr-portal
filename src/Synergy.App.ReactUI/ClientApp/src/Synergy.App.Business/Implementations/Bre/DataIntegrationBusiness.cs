using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class DataIntegrationBusiness : BusinessBase<DataIntegration, DataIntegration>, IDataIntegrationBusiness
    {

        public DataIntegrationBusiness(IRepositoryBase<DataIntegration, DataIntegration> repo, IMapper _autoMapper) : base(repo, _autoMapper)
        {

        }

        public async override Task<CommandResult<DataIntegration>> Create(DataIntegration model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<SynergyEmployeeViewModel>(model);
            var result = await base.Create(model, autoCommit);
            return CommandResult<DataIntegration>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
