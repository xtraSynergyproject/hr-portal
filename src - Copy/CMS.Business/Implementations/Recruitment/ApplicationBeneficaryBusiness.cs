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
    public class ApplicationBeneficaryBusiness : BusinessBase<ApplicationBeneficiaryViewModel, ApplicationBeneficiary>, IApplicationBeneficaryBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationBeneficiaryViewModel> _queryRepo;
        public ApplicationBeneficaryBusiness(IRepositoryBase<ApplicationBeneficiaryViewModel, ApplicationBeneficiary> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationBeneficiaryViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ApplicationBeneficiaryViewModel>> Create(ApplicationBeneficiaryViewModel model)
        {
            var data = _autoMapper.Map<ApplicationBeneficiaryViewModel>(model);

            var result = await base.Create(data);

            return CommandResult<ApplicationBeneficiaryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationBeneficiaryViewModel>> Edit(ApplicationBeneficiaryViewModel model)
        {
            var data = _autoMapper.Map<ApplicationBeneficiaryViewModel>(model);
 
            var result = await base.Edit(data);

            return CommandResult<ApplicationBeneficiaryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationBeneficiaryViewModel>> IsNameExists(ApplicationBeneficiaryViewModel model)
        {                        
            return CommandResult<ApplicationBeneficiaryViewModel>.Instance();
        }
    
    }
}
