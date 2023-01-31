using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class AdhocTaskBusiness : BusinessBase<AdhocTaskComponentViewModel, AdhocTaskComponent>, IAdhocTaskBusiness
    {
        IColumnMetadataBusiness _columnMetadataBusiness;
        public AdhocTaskBusiness(IRepositoryBase<AdhocTaskComponentViewModel, AdhocTaskComponent> repo, IMapper autoMapper, 
            IColumnMetadataBusiness columnMetadataBusiness) : base(repo, autoMapper)
        {
            _columnMetadataBusiness = columnMetadataBusiness;
        }

        public async override Task<CommandResult<AdhocTaskComponentViewModel>> Create(AdhocTaskComponentViewModel model)
        {
           
            var result = await base.Create(model);
            return CommandResult<AdhocTaskComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<AdhocTaskComponentViewModel>> Edit(AdhocTaskComponentViewModel model)
        {
           
            var result = await base.Edit(model);
            return CommandResult<AdhocTaskComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
