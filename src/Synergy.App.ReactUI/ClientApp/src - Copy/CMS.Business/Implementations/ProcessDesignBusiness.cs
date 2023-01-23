using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ProcessDesignBusiness : BusinessBase<ProcessDesignViewModel, ProcessDesign>, IProcessDesignBusiness
    {
        IComponentBusiness _componentBusiness;
        public ProcessDesignBusiness(IRepositoryBase<ProcessDesignViewModel, ProcessDesign> repo, IMapper autoMapper,
            IComponentBusiness componentBusiness) : base(repo, autoMapper)
        {
            _componentBusiness = componentBusiness;
        }

        public async override Task<CommandResult<ProcessDesignViewModel>> Create(ProcessDesignViewModel model)
        {            
            var result = await base.Create(model);
            return CommandResult<ProcessDesignViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ProcessDesignViewModel>> Edit(ProcessDesignViewModel model)
        {         
            var result = await base.Edit(model);
            return CommandResult<ProcessDesignViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task RemoveProcessDesign(string templateId) 
        {
            var process = await GetSingle(x => x.TemplateId == templateId);
            await _componentBusiness.RemoveComponentsByProcessDesignId(process.Id);
        }
    }
}
