using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class BreMasterColumnMetadataBusiness : BusinessBase<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata>, IBreMasterColumnMetadataBusiness
    {
        public BreMasterColumnMetadataBusiness(IRepositoryBase<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<BreMasterColumnMetadataViewModel>> Create(BreMasterColumnMetadataViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<BreMasterColumnMetadataViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<BreMasterColumnMetadataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<BreMasterColumnMetadataViewModel>> Edit(BreMasterColumnMetadataViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<BreMasterColumnMetadataViewModel>(model);
            var result = await base.Edit(data, autoCommit);

            return CommandResult<BreMasterColumnMetadataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
