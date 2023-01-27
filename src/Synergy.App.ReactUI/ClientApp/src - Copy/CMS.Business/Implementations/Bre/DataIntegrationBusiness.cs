using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class DataIntegrationBusiness : BusinessBase<DataIntegration, DataIntegration>, IDataIntegrationBusiness
    {

        public DataIntegrationBusiness(IRepositoryBase<DataIntegration, DataIntegration> repo, IMapper _autoMapper) : base(repo, _autoMapper)
        {

        }

        public async override Task<CommandResult<DataIntegration>> Create(DataIntegration model)
        {
            //var data = _autoMapper.Map<SynergyEmployeeViewModel>(model);
            var result = await base.Create(model);
            return CommandResult<DataIntegration>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
