using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class FormTemplateBusiness : BusinessBase<FormTemplateViewModel, FormTemplate>, IFormTemplateBusiness
    {
        ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IServiceProvider _serviceProvider;
        public FormTemplateBusiness(IServiceProvider serviceProvider,IRepositoryBase<FormTemplateViewModel, FormTemplate> repo, IMapper autoMapper, ITableMetadataBusiness tableMetadataBusiness) : base(repo, autoMapper)
        {
            _tableMetadataBusiness = tableMetadataBusiness;
            _serviceProvider = serviceProvider;
        }

        public async override Task<CommandResult<FormTemplateViewModel>> Create(FormTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model,autoCommit);
            if (result.IsSuccess)
            {

                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null)
                {
                    template.IsChildTable = model.IsChildTable;
                    var tableResult = await _tableMetadataBusiness.ManageTemplateTable(template,false,model.ParentTemplateId);
                    if (!tableResult.IsSuccess)
                    {
                        return CommandResult<FormTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                    }
                }

            }
            return CommandResult<FormTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<FormTemplateViewModel>> Edit(FormTemplateViewModel model, bool autoCommit = true)
        {
            if (!model.EnableIndexPage)
            {
                model.IndexPageTemplateId = null;
            }          
            var result = await base.Edit(model,autoCommit);
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
        public async Task<CommandResult<FormTemplateViewModel>> CopyFormTemplate(FormTemplateViewModel oldModel, string newTempId, CopyTemplateViewModel copyModel = null, bool devImport = false)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _formTemplateBusiness = _serviceProvider.GetService<IFormTemplateBusiness>();

            var oldTempData = new TemplateViewModel();
            if (devImport)
            {
                oldTempData = copyModel.Template;
            }
            else
            {
                oldTempData = await _templateBusiness.GetSingleById(oldModel.TemplateId);
            }
            var newModel = await _formTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);
            FormTemplateViewModel model = new();

            model = newModel;
            newModel = oldModel;

            newModel.Id = model.Id;
            newModel.TemplateId = model.TemplateId;
            newModel.IndexPageTemplateId = model.IndexPageTemplateId;
            newModel.CreatedDate = model.CreatedDate;
            newModel.CreatedBy = model.CreatedBy;
            newModel.LastUpdatedDate = model.LastUpdatedDate;
            newModel.LastUpdatedBy = model.LastUpdatedBy;
            newModel.CompanyId = model.CompanyId;
            newModel.LegalEntityId = model.LegalEntityId;

            var json = Helper.ReplaceJsonProperty(oldTempData.Json, "columnMetadataId");

            newModel.Json = json;

            var formresult = await _formTemplateBusiness.Edit(newModel);
            return formresult;
        }

    }
}
