using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class FormTemplateBusiness : BusinessBase<FormTemplateViewModel, FormTemplate>, IFormTemplateBusiness
    {
        ITableMetadataBusiness _tableMetadataBusiness;
        public FormTemplateBusiness(IRepositoryBase<FormTemplateViewModel, FormTemplate> repo, IMapper autoMapper, ITableMetadataBusiness tableMetadataBusiness) : base(repo, autoMapper)
        {
            _tableMetadataBusiness = tableMetadataBusiness;
        }

        public async override Task<CommandResult<FormTemplateViewModel>> Create(FormTemplateViewModel model)
        {
            var result = await base.Create(model);
            if (result.IsSuccess)
            {

                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null)
                {
                    var tableResult = await _tableMetadataBusiness.ManageTemplateTable(template,false,model.ParentTemplateId);
                    if (!tableResult.IsSuccess)
                    {
                        return CommandResult<FormTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                    }
                }

            }
            return CommandResult<FormTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<FormTemplateViewModel>> Edit(FormTemplateViewModel model)
        {
            if (!model.EnableIndexPage)
            {
                model.IndexPageTemplateId = null;
            }          
            var result = await base.Edit(model);
            if (result.IsSuccess)
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null)
                {
                    template.Json = model.Json;
                    await _repo.Edit<TemplateViewModel, Template>(template);
                    var tableResult = await _tableMetadataBusiness.ManageTemplateTable(template,false,model.ParentTemplateId);
                    if (!tableResult.IsSuccess)
                    {
                        return CommandResult<FormTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                    }
                }
            }
            return CommandResult<FormTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
