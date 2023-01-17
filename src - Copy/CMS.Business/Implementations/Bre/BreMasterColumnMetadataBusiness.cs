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
    public class BreMasterColumnMetadataBusiness : BusinessBase<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata>, IBreMasterColumnMetadataBusiness
    {
        public BreMasterColumnMetadataBusiness(IRepositoryBase<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<BreMasterColumnMetadataViewModel>> Create(BreMasterColumnMetadataViewModel model)
        {

            var data = _autoMapper.Map<BreMasterColumnMetadataViewModel>(model);           
            var result = await base.Create(data);

            return CommandResult<BreMasterColumnMetadataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<BreMasterColumnMetadataViewModel>> Edit(BreMasterColumnMetadataViewModel model)
        {

            var data = _autoMapper.Map<BreMasterColumnMetadataViewModel>(model);            
            var result = await base.Edit(data);

            return CommandResult<BreMasterColumnMetadataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
       

    }
}
