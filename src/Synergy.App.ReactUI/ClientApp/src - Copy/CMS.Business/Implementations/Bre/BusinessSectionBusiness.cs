using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class BusinessSectionBusiness : BusinessBase<BusinessSectionViewModel, BusinessSection>, IBusinessSectionBusiness
    {
        public BusinessSectionBusiness(IRepositoryBase<BusinessSectionViewModel, BusinessSection> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<CommandResult<BusinessSectionViewModel>> Create(BusinessSectionViewModel model)
        {
            var data = _autoMapper.Map<BusinessSectionViewModel>(model);
            var result = await base.Create(data);
            return CommandResult<BusinessSectionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
