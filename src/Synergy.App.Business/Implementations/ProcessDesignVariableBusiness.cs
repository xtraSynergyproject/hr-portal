using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ProcessDesignVariableBusiness : BusinessBase<ProcessDesignVariableViewModel, ProcessDesignVariable>, IProcessDesignVariableBusiness
    {
        IColumnMetadataBusiness _columnMetadataBusiness;
        public ProcessDesignVariableBusiness(IRepositoryBase<ProcessDesignVariableViewModel, ProcessDesignVariable> repo, IMapper autoMapper, 
            IColumnMetadataBusiness columnMetadataBusiness) : base(repo, autoMapper)
        {
            _columnMetadataBusiness = columnMetadataBusiness;
        }

        public async override Task<CommandResult<ProcessDesignVariableViewModel>> Create(ProcessDesignVariableViewModel model, bool autoCommit = true)
        {
            model.DisplayName = model.Name;
            var result = await base.Create(model,autoCommit);
            return CommandResult<ProcessDesignVariableViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ProcessDesignVariableViewModel>> Edit(ProcessDesignVariableViewModel model, bool autoCommit = true)
        {
            model.DisplayName = model.Name;
            var result = await base.Edit(model,autoCommit);
            return CommandResult<ProcessDesignVariableViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<ProcessDesignVariableViewModel>> GetListByTemplate(string templateId)
        {
            var model = await _repo.GetSingle(x => x.TemplateId == templateId, x => x.Template);
            var datalist = new List<ProcessDesignVariableViewModel>();

            if (model != null)
            {                
                var colmodel = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == model.Template.TableMetadataId);

                foreach (var col in colmodel)
                {
                   datalist.Add(new ProcessDesignVariableViewModel { Name = col.Name, DataType = col.DataType });
                }                
            }
            return datalist;
        }        
    }
}
