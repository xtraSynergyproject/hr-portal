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
    public class BreMasterTableMetadataBusiness : BusinessBase<BreMasterTableMetadataViewModel, BreMasterTableMetadata>, IBreMasterTableMetadataBusiness
    {
        private readonly IServiceProvider _serviceProvider;
        public BreMasterTableMetadataBusiness(IRepositoryBase<BreMasterTableMetadataViewModel, BreMasterTableMetadata> repo
            , IMapper autoMapper
            , IServiceProvider serviceProvider) : base(repo, autoMapper)
        {
            _serviceProvider = serviceProvider;
        }

        public async override Task<CommandResult<BreMasterTableMetadataViewModel>> Create(BreMasterTableMetadataViewModel model)
        {

            var result = await base.Create(model);
            if (result.IsSuccess)
            {
                var brnBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
                await brnBusiness.ManageOperationValue(null, null, result.Item.Id);
            }
            return result;
        }

        public async override Task<CommandResult<BreMasterTableMetadataViewModel>> Edit(BreMasterTableMetadataViewModel model)
        {

            var result = await base.Edit(model);
            if (result.IsSuccess)
            {
                var brnBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
                await brnBusiness.ManageOperationValue(null, null, result.Item.Id);
            }
            return result;
        }


    }
}
