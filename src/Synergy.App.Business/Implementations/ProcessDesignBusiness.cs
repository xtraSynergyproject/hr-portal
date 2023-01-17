using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ProcessDesignBusiness : BusinessBase<ProcessDesignViewModel, ProcessDesign>, IProcessDesignBusiness
    {
        IComponentBusiness _componentBusiness;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public ProcessDesignBusiness(IRepositoryBase<ProcessDesignViewModel, ProcessDesign> repo, IMapper autoMapper,
            IComponentBusiness componentBusiness, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _componentBusiness = componentBusiness;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<ProcessDesignViewModel>> Create(ProcessDesignViewModel model, bool autoCommit = true)
        {            
            var result = await base.Create(model,autoCommit);
            return CommandResult<ProcessDesignViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ProcessDesignViewModel>> Edit(ProcessDesignViewModel model, bool autoCommit = true)
        {         
            var result = await base.Edit(model,autoCommit);
            return CommandResult<ProcessDesignViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task RemoveProcessDesign(string templateId) 
        {
            var process = await GetSingle(x => x.TemplateId == templateId);
            await _componentBusiness.RemoveComponentsByProcessDesignId(process.Id);
        }

        public async Task<IList<IdNameViewModel>> GetTriggeringStepTaskComponentList(string templateId)
        {
            var data = await _cmsQueryBusiness.GetTriggeringStepTaskComponentList(templateId);
            return data;
        }
    }
}
