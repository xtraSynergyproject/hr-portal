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
    public class BusinessAreaBusiness : BusinessBase<BusinessAreaViewModel, BusinessArea>, IBusinessAreaBusiness
    {

        public BusinessAreaBusiness(IRepositoryBase<BusinessAreaViewModel, BusinessArea> repo, IMapper _autoMapper) : base(repo, _autoMapper)
        {

        }

        public async override Task<CommandResult<BusinessAreaViewModel>> Create(BusinessAreaViewModel model)
        {
            var data = _autoMapper.Map<BusinessAreaViewModel>(model);
            var result = await base.Create(data);
            return CommandResult<BusinessAreaViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
