using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class BusinessSectionBusiness : BusinessBase<BusinessSectionViewModel, BusinessSection>, IBusinessSectionBusiness
    {
        public BusinessSectionBusiness(IRepositoryBase<BusinessSectionViewModel, BusinessSection> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<CommandResult<BusinessSectionViewModel>> Create(BusinessSectionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<BusinessSectionViewModel>(model);
            var result = await base.Create(data, autoCommit);
            return CommandResult<BusinessSectionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
