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
    public class BusinessAreaBusiness : BusinessBase<BusinessAreaViewModel, BusinessArea>, IBusinessAreaBusiness
    {

        public BusinessAreaBusiness(IRepositoryBase<BusinessAreaViewModel, BusinessArea> repo, IMapper _autoMapper) : base(repo, _autoMapper)
        {

        }

        public async override Task<CommandResult<BusinessAreaViewModel>> Create(BusinessAreaViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<BusinessAreaViewModel>(model);
            var result = await base.Create(data, autoCommit);
            return CommandResult<BusinessAreaViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
