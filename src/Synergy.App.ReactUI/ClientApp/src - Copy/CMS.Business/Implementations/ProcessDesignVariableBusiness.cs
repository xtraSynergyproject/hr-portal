using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ProcessDesignVariableBusiness : BusinessBase<ProcessDesignVariableViewModel, ProcessDesignVariable>, IProcessDesignVariableBusiness
    {
        IColumnMetadataBusiness _columnMetadataBusiness;
        public ProcessDesignVariableBusiness(IRepositoryBase<ProcessDesignVariableViewModel, ProcessDesignVariable> repo, IMapper autoMapper, 
            IColumnMetadataBusiness columnMetadataBusiness) : base(repo, autoMapper)
        {
            _columnMetadataBusiness = columnMetadataBusiness;
        }

        public async override Task<CommandResult<ProcessDesignVariableViewModel>> Create(ProcessDesignVariableViewModel model)
        {
            model.DisplayName = model.Name;
            var result = await base.Create(model);
            return CommandResult<ProcessDesignVariableViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ProcessDesignVariableViewModel>> Edit(ProcessDesignVariableViewModel model)
        {
            model.DisplayName = model.Name;
            var result = await base.Edit(model);
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
