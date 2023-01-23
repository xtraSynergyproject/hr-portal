using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class AdhocTaskBusiness : BusinessBase<AdhocTaskComponentViewModel, AdhocTaskComponent>, IAdhocTaskBusiness
    {
        IColumnMetadataBusiness _columnMetadataBusiness;
        public AdhocTaskBusiness(IRepositoryBase<AdhocTaskComponentViewModel, AdhocTaskComponent> repo, IMapper autoMapper,
            IColumnMetadataBusiness columnMetadataBusiness) : base(repo, autoMapper)
        {
            _columnMetadataBusiness = columnMetadataBusiness;
        }

        public async override Task<CommandResult<AdhocTaskComponentViewModel>> Create(AdhocTaskComponentViewModel model, bool autoCommit = true)
        {

            var result = await base.Create(model, autoCommit);
            return CommandResult<AdhocTaskComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<AdhocTaskComponentViewModel>> Edit(AdhocTaskComponentViewModel model, bool autoCommit = true)
        {

            var result = await base.Edit(model, autoCommit);
            return CommandResult<AdhocTaskComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
