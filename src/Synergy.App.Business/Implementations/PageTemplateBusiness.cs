using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class PageTemplateBusiness : BusinessBase<PageTemplateViewModel, PageTemplate>, IPageTemplateBusiness
    {
        public PageTemplateBusiness(IRepositoryBase<PageTemplateViewModel, PageTemplate> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<PageTemplateViewModel>> Create(PageTemplateViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<PageTemplateViewModel>(model);
            var result = await base.Create(data, autoCommit);
            if (result.IsSuccess)
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null)
                {
                    template.Json = model.Json;
                    await _repo.Edit<TemplateViewModel, Template>(template);
                }
            }
            return CommandResult<PageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PageTemplateViewModel>> Edit(PageTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            if (result.IsSuccess)
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null)
                {
                    template.Json = model.Json;
                    await _repo.Edit<TemplateViewModel, Template>(template);
                }
            }
            return CommandResult<PageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<PageTemplateViewModel>> CopyPageTemplate(string tempJson,PageTemplateViewModel oldModel, string newTempId)
        {
            
            var newModel = await _repo.GetSingle(x => x.TemplateId == newTempId);

            newModel.Json = tempJson;
            newModel.SequenceOrder = oldModel.SequenceOrder;

            var pageResult = await Edit(newModel);
            return pageResult;
        }

    }
}
