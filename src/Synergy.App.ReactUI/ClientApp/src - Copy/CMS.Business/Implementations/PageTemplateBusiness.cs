using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class PageTemplateBusiness : BusinessBase<PageTemplateViewModel, PageTemplate>, IPageTemplateBusiness
    {
        public PageTemplateBusiness(IRepositoryBase<PageTemplateViewModel, PageTemplate> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<PageTemplateViewModel>> Create(PageTemplateViewModel model)
        {
            var data = _autoMapper.Map<PageTemplateViewModel>(model);
            var result = await base.Create(data);
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

        public async override Task<CommandResult<PageTemplateViewModel>> Edit(PageTemplateViewModel model)
        {
            var result = await base.Edit(model);
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


    }
}
