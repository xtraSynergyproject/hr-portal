using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class TemplateBusiness : BusinessBase<TemplateViewModel, Template>, ITemplateBusiness
    {
        ITableMetadataBusiness _tableBusiness;
        IFormTemplateBusiness _formTemplateBusiness;
        INoteTemplateBusiness _noteTemplateBusiness;
        ITaskTemplateBusiness _taskTemplateBusiness;
        IServiceTemplateBusiness _serviceTemplateBusiness;
        IPageTemplateBusiness _pageTemplateBusiness;
        ICustomTemplateBusiness _customTemplateBusiness;
        ITemplateCategoryBusiness _templateCategoryBusiness;
        IProcessDesignBusiness _processDesignBusiness;
        INoteIndexPageTemplateBusiness _noteIndexPageTemplateBusiness;
        IFormIndexPageTemplateBusiness _formIndexPageTemplateBusiness;
        ITaskIndexPageTemplateBusiness _taskIndexPageTemplateBusiness;
        IServiceIndexPageTemplateBusiness _serviceIndexPageTemplateBusiness;
        IComponentBusiness _componentBusiness;
        IStepTaskComponentBusiness _stepTaskComponentBusiness;
        IDecisionScriptComponentBusiness _decisionScriptBusiness;
        IExecutionScriptBusiness _executionScriptBusiness;
        IColumnMetadataBusiness _columnMetadataBusiness;
        private readonly IRepositoryQueryBase<TemplateViewModel> _queryRepo;
        public TemplateBusiness(IRepositoryBase<TemplateViewModel, Template> repo, IMapper autoMapper,
            ITableMetadataBusiness tableBusiness,
            INoteTemplateBusiness noteTemplateBusiness,
            ITaskTemplateBusiness taskTemplateBusiness,
            IServiceTemplateBusiness serviceTemplateBusiness,
            IFormTemplateBusiness formTemplateBusiness,
            IPageTemplateBusiness pageTemplateBusiness,
               ICustomTemplateBusiness customTemplateBusiness, ITemplateCategoryBusiness templateCategoryBusiness,
               IProcessDesignBusiness processDesignBusiness,
               INoteIndexPageTemplateBusiness noteIndexPageTemplateBusiness,
               ITaskIndexPageTemplateBusiness taskIndexPageTemplateBusiness,
               IComponentBusiness componentBusiness,
               IServiceIndexPageTemplateBusiness serviceIndexPageTemplateBusiness,
                IFormIndexPageTemplateBusiness formIndexPageTemplateBusiness,
                IStepTaskComponentBusiness stepTaskComponentBusiness,
                 IDecisionScriptComponentBusiness decisionScriptBusiness,
                 IExecutionScriptBusiness executionScriptBusiness
            , IColumnMetadataBusiness columnMetadataBusiness, IRepositoryQueryBase<TemplateViewModel> queryRepo) : base(repo, autoMapper)

        {
            _tableBusiness = tableBusiness;
            _noteTemplateBusiness = noteTemplateBusiness;
            _taskTemplateBusiness = taskTemplateBusiness;
            _serviceTemplateBusiness = serviceTemplateBusiness;
            _formTemplateBusiness = formTemplateBusiness;
            _pageTemplateBusiness = pageTemplateBusiness;
            _customTemplateBusiness = customTemplateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _processDesignBusiness = processDesignBusiness;
            _noteIndexPageTemplateBusiness = noteIndexPageTemplateBusiness;
            _taskIndexPageTemplateBusiness = taskIndexPageTemplateBusiness;
            _componentBusiness = componentBusiness;
            _serviceIndexPageTemplateBusiness = serviceIndexPageTemplateBusiness;
            _formIndexPageTemplateBusiness = formIndexPageTemplateBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _queryRepo = queryRepo;
            _stepTaskComponentBusiness = stepTaskComponentBusiness;
            _decisionScriptBusiness = decisionScriptBusiness;
            _executionScriptBusiness = executionScriptBusiness;
        }
        private async Task<CommandResult<TemplateViewModel>> IsNameExists(TemplateViewModel viewModel)
        {

            var givenName = await _queryRepo.ExecuteScalar<TemplateViewModel>(@$"select ""Id"" from public.""Template"" 
            where ""IsDeleted""=false and ""Id""<>'{viewModel.Id}' and LOWER(""Name"")=LOWER('{viewModel.Name}')", null);
            if (givenName != null)
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The given Name already exists. Please enter another Name");
                return CommandResult<TemplateViewModel>.Instance(viewModel, false, obj);
            }
            var TemplateByCategorylist = await GetList(x => x.TemplateCategoryId == viewModel.TemplateCategoryId);
            if (TemplateByCategorylist.Exists(x => x.Id != viewModel.Id && viewModel.DisplayName.Equals(x.DisplayName, StringComparison.InvariantCultureIgnoreCase)))
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The given Display Name already exists. Please enter another Display Name");
                return CommandResult<TemplateViewModel>.Instance(viewModel, false, obj);
            }
            var code = await _queryRepo.ExecuteScalar<TemplateViewModel>(@$"select ""Id"" from public.""Template"" 
            where ""IsDeleted""=false and ""Id""<>'{viewModel.Id}' and LOWER(""Code"")=LOWER('{viewModel.Code}')", null);
            if (code != null)
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The given Code exists. Please enter another Code");
                return CommandResult<TemplateViewModel>.Instance(viewModel, false, obj);
            }
            var tableId = viewModel.TableMetadataId.Coalesce(viewModel.UdfTableMetadataId);
            var dispname = await _tableBusiness.GetSingle(x => x.Name == viewModel.DisplayName && x.Id != tableId);
            if (dispname != null)
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("DisplayName", "Display name already exist.");
                return CommandResult<TemplateViewModel>.Instance(viewModel, false, obj);
            }
            var tablename = await _tableBusiness.GetSingle(x => x.Name == viewModel.Name && x.Id != tableId);
            if (tablename != null)
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("Name", "Name already exist.");
                return CommandResult<TemplateViewModel>.Instance(viewModel, false, obj);
            }
            var tablealias = await _tableBusiness.GetSingle(x => x.Alias == viewModel.Code && x.Id != tableId);
            if (tablealias != null)
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("Alias", "Alias/Short name already exist.");
                return CommandResult<TemplateViewModel>.Instance(viewModel, false, obj);
            }
            return CommandResult<TemplateViewModel>.Instance();
        }

        public async override Task<CommandResult<TemplateViewModel>> Create(TemplateViewModel model)
        {

            if (model.Name.IsNullOrEmpty())
            {
                return CommandResult<TemplateViewModel>.Instance(model, x => x.Name, "Name is required.");
            }
            var modelname = model.Name;
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                model.Name = model.Name.Replace(c.ToString(), "_");
            }
            model.Name = model.Name.Replace(" ", "_");
            var category = await _templateCategoryBusiness.GetSingleById(model.TemplateCategoryId);
            if (category != null)
            {
                if (category.TemplateType == TemplateTypeEnum.Page)
                {
                    model.Name = "P_" + category.Code + "_" + model.Name;
                }
                else if (category.TemplateType == TemplateTypeEnum.Form)
                {
                    model.Name = "F_" + category.Code + "_" + model.Name;
                }
                else if (category.TemplateType == TemplateTypeEnum.Note)
                {
                    model.Name = "N_" + category.Code + "_" + model.Name;
                }
                else if (category.TemplateType == TemplateTypeEnum.Task)
                {
                    model.Name = "T_" + category.Code + "_" + model.Name;
                }
                else if (category.TemplateType == TemplateTypeEnum.Service)
                {
                    model.Name = "S_" + category.Code + "_" + model.Name;
                }
                else if (category.TemplateType == TemplateTypeEnum.Custom)
                {
                    model.Name = "C_" + category.Code + "_" + model.Name;
                }
                else if (category.TemplateType == TemplateTypeEnum.ProcessDesign)
                {
                    model.Name = "PD_" + category.Code + "_" + model.Name;
                }
            }

            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TemplateViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);
            if (result.IsSuccess)
            {
                switch (model.TemplateType)
                {
                    case TemplateTypeEnum.FormIndexPage:
                        break;
                    case TemplateTypeEnum.Page:
                        var pageResult = await _pageTemplateBusiness.Create(new PageTemplateViewModel { TemplateId = result.Item.Id });
                        if (!pageResult.IsSuccess)
                        {
                            pageResult.Messages.Add("TemplateName", "" + model.Name + " : Created.");
                            return CommandResult<TemplateViewModel>.Instance(model, pageResult.IsSuccess, pageResult.Messages);
                        }
                        break;
                    case TemplateTypeEnum.Form:
                        var formResult = await _formTemplateBusiness.Create(new FormTemplateViewModel { TemplateId = result.Item.Id });
                        if (!formResult.IsSuccess)
                        {
                            formResult.Messages.Add("TemplateName", "" + model.Name + " : Created.");
                            return CommandResult<TemplateViewModel>.Instance(model, formResult.IsSuccess, formResult.Messages);
                        }
                        var formIndexResult = await _formIndexPageTemplateBusiness.Create(new FormIndexPageTemplateViewModel { TemplateId = result.Item.Id, CreateButtonText = "Create", EditButtonText = "Edit", DeleteButtonText = "Delete", EnableCreateButton = true, EnableEditButton = true, EnableDeleteButton = true, EnableDeleteConfirmation = true, DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation, SelectedTableRows = new List<FormIndexPageColumnViewModel>() });
                        break;
                    case TemplateTypeEnum.Note:
                        var noteresult = await _noteTemplateBusiness.Create(new NoteTemplateViewModel { TemplateId = result.Item.Id, TemplateViewModel = result.Item });

                        if (!noteresult.IsSuccess)
                        {
                            noteresult.Messages.Add("TemplateName", "" + model.Name + " : Created.");
                            return CommandResult<TemplateViewModel>.Instance(model, noteresult.IsSuccess, noteresult.Messages);
                        }
                        var noteIndexResult = await _noteIndexPageTemplateBusiness.Create(new NoteIndexPageTemplateViewModel { TemplateId = result.Item.Id, CreateButtonText = "Create", EditButtonText = "Edit", DeleteButtonText = "Delete", EnableCreateButton = true, EnableEditButton = true, EnableDeleteButton = true, EnableDeleteConfirmation = true, DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation, SelectedTableRows = new List<NoteIndexPageColumnViewModel>() });
                        break;
                    case TemplateTypeEnum.Task:
                        var taskResult = await _taskTemplateBusiness.Create(new TaskTemplateViewModel { TemplateId = result.Item.Id, TaskTemplateType = model.TaskType });
                        if (!taskResult.IsSuccess)
                        {
                            taskResult.Messages.Add("TemplateName", "" + model.Name + " : Created.");
                            return CommandResult<TemplateViewModel>.Instance(model, taskResult.IsSuccess, taskResult.Messages);
                        }
                        else
                        {
                            //if (model.TaskType == TaskTypeEnum.StepTask)
                            //{
                            //    var ttmodel = await _taskTemplateBusiness.GetSingleById(taskResult.Item.Id);
                            //    ttmodel.TaskTemplateType = TaskTypeEnum.StepTask;
                            //    await _taskTemplateBusiness.ManageEdit(ttmodel);
                            //}
                            if (model.UdfTemplateId.IsNullOrEmpty())
                            {
                                var templatecategory = await _repo.GetSingleById<TemplateCategoryViewModel, TemplateCategory>(result.Item.TemplateCategoryId);
                                if (templatecategory != null)
                                {

                                    var existtemplatecategory = await _repo.GetSingle<TemplateCategoryViewModel, TemplateCategory>(x => x.Code == templatecategory.Code && x.TemplateType == TemplateTypeEnum.Note);
                                    if (existtemplatecategory != null)
                                    {
                                        templatecategory = existtemplatecategory;
                                    }
                                    else
                                    {
                                        var newtemplatecategory = new TemplateCategoryViewModel { TemplateType = TemplateTypeEnum.Note, Code = "TNC_" + templatecategory.Code, Name = templatecategory.Name, Description = templatecategory.Description };
                                        var newtemplatecategoryresult = await _repo.Create<TemplateCategoryViewModel, TemplateCategory>(newtemplatecategory);
                                        templatecategory = newtemplatecategoryresult;

                                    }
                                }
                                if (model.TaskType != TaskTypeEnum.StepTask)
                                {
                                    var newmodel = new TemplateViewModel();
                                    newmodel.TemplateType = TemplateTypeEnum.Note;
                                    newmodel.Id = Guid.NewGuid().ToString();
                                    newmodel.TemplateCategoryId = templatecategory.Id;
                                    newmodel.Code = "TN_" + result.Item.Code;
                                    newmodel.Name = modelname;
                                    newmodel.DisplayName = result.Item.DisplayName;
                                    newmodel.Description = result.Item.Description;
                                    var result2 = await Create(newmodel);
                                    if (result2.IsSuccess)
                                    {
                                        var t = await _repo.GetSingleById(result.Item.Id);
                                        if (t != null)
                                        {
                                            t.UdfTemplateId = result2.Item.Id;
                                            t.UdfTableMetadataId = result2.Item.TableMetadataId;
                                            var tresult = await _repo.Edit(t);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                var temp = await _repo.GetSingleById(model.UdfTemplateId);
                                var t = await _repo.GetSingleById(result.Item.Id);
                                t.UdfTableMetadataId = temp.TableMetadataId;
                                var tresult = await _repo.Edit(t);
                            }
                            var taskIndexResult = await _taskIndexPageTemplateBusiness.Create(new TaskIndexPageTemplateViewModel { TemplateId = result.Item.Id, CreateButtonText = "Create", EditButtonText = "Edit", DeleteButtonText = "Delete", EnableCreateButton = true, EnableEditButton = true, EnableDeleteButton = true, EnableDeleteConfirmation = true, DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation, SelectedTableRows = new List<TaskIndexPageColumnViewModel>() });
                        }
                        break;
                    case TemplateTypeEnum.Service:
                        var processDesign = await _processDesignBusiness.Create(new ProcessDesignViewModel { TemplateId = result.Item.Id });
                        var serviceresult1 = await _serviceTemplateBusiness.Create(new ServiceTemplateViewModel { TemplateId = result.Item.Id });
                        if (!serviceresult1.IsSuccess)
                        {
                            serviceresult1.Messages.Add("TemplateName", "" + model.Name + " : Created.");
                            return CommandResult<TemplateViewModel>.Instance(model, serviceresult1.IsSuccess, serviceresult1.Messages);
                        }
                        else
                        {
                            if (model.UdfTemplateId.IsNullOrEmpty())
                            {
                                var templatecategory = await _repo.GetSingleById<TemplateCategoryViewModel, TemplateCategory>(result.Item.TemplateCategoryId);
                                if (templatecategory != null)
                                {
                                    var existtemplatecategory = await _repo.GetSingle<TemplateCategoryViewModel, TemplateCategory>(x => x.Code == templatecategory.Code && x.TemplateType == TemplateTypeEnum.Note);
                                    if (existtemplatecategory != null)
                                    {
                                        templatecategory = existtemplatecategory;
                                    }
                                    else
                                    {
                                        var newtemplatecategory = new TemplateCategoryViewModel { TemplateType = TemplateTypeEnum.Note, Code = "SNC_" + templatecategory.Code, Name = templatecategory.Name, Description = templatecategory.Description };
                                        var newtemplatecategoryresult = await _repo.Create<TemplateCategoryViewModel, TemplateCategory>(newtemplatecategory);
                                        templatecategory = newtemplatecategoryresult;

                                    }
                                }
                                var newmodel = new TemplateViewModel();
                                newmodel.TemplateType = TemplateTypeEnum.Note;
                                newmodel.Id = Guid.NewGuid().ToString();
                                newmodel.TemplateCategoryId = templatecategory.Id;
                                newmodel.Code = "SN_" + result.Item.Code;
                                newmodel.Name = modelname;
                                newmodel.DisplayName = result.Item.DisplayName;
                                newmodel.Description = result.Item.Description;
                                var result2 = await Create(newmodel);
                                if (result2.IsSuccess)
                                {
                                    var t = await _repo.GetSingleById(result.Item.Id);
                                    if (t != null)
                                    {
                                        t.UdfTemplateId = result2.Item.Id;
                                        t.UdfTableMetadataId = result2.Item.TableMetadataId;
                                        var tresult = await _repo.Edit(t);
                                    }
                                }
                            }
                            else
                            {
                                var temp = await _repo.GetSingleById(model.UdfTemplateId);
                                var t = await _repo.GetSingleById(result.Item.Id);
                                t.UdfTableMetadataId = temp.TableMetadataId;
                                var tresult = await _repo.Edit(t);
                            }
                            var serviceIndexResult = await _serviceIndexPageTemplateBusiness.Create(new ServiceIndexPageTemplateViewModel { TemplateId = result.Item.Id, CreateButtonText = "Create", EditButtonText = "Edit", DeleteButtonText = "Delete", EnableCreateButton = true, EnableEditButton = true, EnableDeleteButton = true, EnableDeleteConfirmation = true, DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation, SelectedTableRows = new List<ServiceIndexPageColumnViewModel>() });
                        }
                        break;
                    case TemplateTypeEnum.Custom:
                        var customResult = await _customTemplateBusiness.Create(new CustomTemplateViewModel { TemplateId = result.Item.Id });
                        if (!customResult.IsSuccess)
                        {
                            customResult.Messages.Add("TemplateName", "" + model.Name + " : Created.");
                            return CommandResult<TemplateViewModel>.Instance(model, customResult.IsSuccess, customResult.Messages);
                        }
                        break;
                    case TemplateTypeEnum.ProcessDesign:
                        var serviceresult = await _serviceTemplateBusiness.Create(new ServiceTemplateViewModel { TemplateId = result.Item.Id });
                        if (!serviceresult.IsSuccess)
                        {
                            serviceresult.Messages.Add("TemplateName", "" + model.Name + " : Created.");
                            return CommandResult<TemplateViewModel>.Instance(model, serviceresult.IsSuccess, serviceresult.Messages);
                        }
                        break;
                }
            }
            return CommandResult<TemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TemplateViewModel>> Edit(TemplateViewModel model)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TemplateViewModel>.Instance(model, false, validateName.Messages);
            }
            var template = await GetSingleById(model.Id);
            model.Json = template.Json;
            model.TableMetadataId = template.TableMetadataId;
            var result = await base.Edit(model);
            return CommandResult<TemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private List<string> FindNonExist(JArray comps, List<string> efield)
        {
            foreach (JObject jcomp in comps)
            {
                var typeObj = jcomp.SelectToken("type");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    if (type == "textfield" || type == "textarea" || type == "number" || type == "password"
                        || type == "selectboxes" || type == "checkbox" || type == "select" || type == "radio"
                        || type == "email" || type == "url" || type == "phoneNumber" || type == "tags"
                        || type == "dateTime" || type == "day" || type == "time" || type == "currency"
                        || type == "signature")
                    {
                        var tempObj = jcomp.SelectToken("columnMetadataId");
                        if (tempObj.IsNotNull())
                        {
                            efield.Add((string)tempObj);
                        }
                    }
                    else if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                FindNonExist(rows, efield);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                FindNonExist(rows, efield);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            FindNonExist(rows, efield);
                    }
                }
            }
            return efield;
        }
        public async Task<List<TemplateViewModel>> GetTemplateByType(TemplateTypeEnum type, string portalId)
        {
            var result = await _repo.GetList(x => x.IsDeleted == false && x.TemplateCategory.TemplateType == type, x => x.TemplateCategory);
            if (portalId.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.PortalId == portalId).ToList();
            }
            return result;
        }
        public async Task<ExportTemplateViewModel> ExportTemplate(ExportTemplateViewModel model)
        {
            model.TemplateCategory = await _templateCategoryBusiness.GetSingleById(model.Template.TemplateCategoryId);
            if (model.Template.TemplateType == TemplateTypeEnum.Custom)
            {
                if (model.Template != null)
                {
                    model.CustomTemplate = await _customTemplateBusiness.GetSingle(x => x.TemplateId == model.Template.Id);
                    //  model.Template.TableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.Template.TableMetadataId);

                }
            }
            if (model.Template.TemplateType == TemplateTypeEnum.Page)
            {
                if (model.Template != null)
                {
                    model.PageTemplate = await _pageTemplateBusiness.GetSingle(x => x.TemplateId == model.Template.Id);
                    model.Template.TableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.Template.TableMetadataId);

                }
            }
            if (model.Template.TemplateType == TemplateTypeEnum.Form)
            {

                if (model.Template != null)
                {
                    model.FormTemplate = await _formTemplateBusiness.GetSingle(x => x.TemplateId == model.Template.Id);
                    model.Template.TableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.Template.TableMetadataId);
                    if (model.FormTemplate != null)
                    {
                        //  model.FormTemplate.NoteNotificationTemplate = await _repo.GetSingle<NoteNotificationTemplateViewModel, NoteNotificationTemplate>(x => x.NoteTemplateId == model.NoteTemplate.Id);
                        model.FormTemplate.IndexPageTemplateDetails = await _repo.GetSingle<FormIndexPageTemplateViewModel, FormIndexPageTemplate>(x => x.TemplateId == model.Template.Id);
                        if (model.FormTemplate.IndexPageTemplateDetails != null)
                        {
                            model.FormTemplate.IndexPageTemplateDetails.SelectedTableRows = await _repo.GetList<FormIndexPageColumnViewModel, FormIndexPageColumn>(x => x.FormIndexPageTemplateId == model.FormTemplate.IndexPageTemplateId);
                        }
                    }
                }
            }
            if (model.Template.TemplateType == TemplateTypeEnum.Note)
            {

                if (model.Template != null)
                {
                    model.NoteTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == model.Template.Id);
                    model.Template.TableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.Template.TableMetadataId);
                    if (model.NoteTemplate != null)
                    {
                        model.NoteTemplate.NotificationTemplate = await _repo.GetSingle<NotificationTemplateViewModel, NotificationTemplate>(x => x.TemplateId == model.Template.Id);
                        model.NoteTemplate.NoteIndexPageTemplateDetails = await _repo.GetSingle<NoteIndexPageTemplateViewModel, NoteIndexPageTemplate>(x => x.TemplateId == model.Template.Id);
                        if (model.NoteTemplate.NoteIndexPageTemplateDetails != null)
                        {
                            model.NoteTemplate.NoteIndexPageTemplateDetails.SelectedTableRows = await _repo.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == model.NoteTemplate.NoteIndexPageTemplateId);
                        }
                    }
                }
            }
            if (model.Template.TemplateType == TemplateTypeEnum.Task)
            {

                if (model.Template != null)
                {
                    model.TaskTemplate = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == model.Template.Id);
                    //model.Template.TableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.Template.TableMetadataId);
                    if (model.TaskTemplate != null)
                    {
                        var NoteUdfTemplate = await GetSingle(x => x.Id == model.Template.UdfTemplateId);
                        ExportTemplateViewModel newmodel = new ExportTemplateViewModel();
                        newmodel.Template = NoteUdfTemplate;
                        model.TaskTemplate.NoteUdfTemplate = await ExportTemplate(newmodel);
                        model.TaskTemplate.UdfTableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.Template.UdfTableMetadataId);
                        model.TaskTemplate.NotificationTemplate = await _repo.GetSingle<NotificationTemplateViewModel, NotificationTemplate>(x => x.TemplateId == model.Template.Id);
                        model.TaskTemplate.TaskIndexPageTemplateDetails = await _repo.GetSingle<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate>(x => x.TemplateId == model.Template.Id);
                        if (model.TaskTemplate.TaskIndexPageTemplateDetails != null)
                        {
                            model.TaskTemplate.TaskIndexPageTemplateDetails.SelectedTableRows = await _repo.GetList<TaskIndexPageColumnViewModel, TaskIndexPageColumn>(x => x.TaskIndexPageTemplateId == model.TaskTemplate.TaskIndexPageTemplateId);
                        }
                    }
                }
            }
            if (model.Template.TemplateType == TemplateTypeEnum.Service)
            {

                if (model.Template != null)
                {
                    model.ServiceTemplate = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == model.Template.Id);

                    if (model.ServiceTemplate != null)
                    {
                        var NoteUdfTemplate = await GetSingle(x => x.Id == model.Template.UdfTemplateId);
                        ExportTemplateViewModel newmodel = new ExportTemplateViewModel();
                        newmodel.Template = NoteUdfTemplate;
                        model.ServiceTemplate.NoteUdfTemplate = await ExportTemplate(newmodel);
                        model.ServiceTemplate.UdfTableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.Template.UdfTableMetadataId);
                        model.ServiceTemplate.NotificationTemplate = await _repo.GetSingle<NotificationTemplateViewModel, NotificationTemplate>(x => x.TemplateId == model.Template.Id);
                        model.ServiceTemplate.ServiceIndexPageTemplateDetails = await _repo.GetSingle<ServiceIndexPageTemplateViewModel, ServiceIndexPageTemplate>(x => x.TemplateId == model.Template.Id);
                        if (model.ServiceTemplate.ServiceIndexPageTemplateDetails != null)
                        {
                            model.ServiceTemplate.ServiceIndexPageTemplateDetails.SelectedTableRows = await _repo.GetList<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(x => x.ServiceIndexPageTemplateId == model.ServiceTemplate.ServiceIndexPageTemplateId);
                        }
                        model.ServiceTemplate.ProcessDesign = await _repo.GetSingle<ProcessDesignViewModel, ProcessDesign>(x => x.TemplateId == model.Template.Id);
                        if (model.ServiceTemplate.ProcessDesign != null)
                        {
                            model.ServiceTemplate.ProcessDesign.Components = await _repo.GetList<ComponentViewModel, Component>(x => x.ProcessDesignId == model.ServiceTemplate.ProcessDesign.Id);
                            if (model.ServiceTemplate.ProcessDesign.Components != null)
                            {
                                foreach (var comp in model.ServiceTemplate.ProcessDesign.Components)
                                {
                                    if (comp.ComponentsType == ProcessDesignComponentTypeEnum.StepTask)
                                    {
                                        var prop = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.ComponentId == comp.Id);
                                        comp.Properties = JsonConvert.SerializeObject(prop);
                                    }
                                    if (comp.ComponentsType == ProcessDesignComponentTypeEnum.DecisionScript)
                                    {
                                        var prop = await _repo.GetSingle<DecisionScriptComponentViewModel, DecisionScriptComponent>(x => x.ComponentId == comp.Id);
                                        comp.Properties = JsonConvert.SerializeObject(prop);
                                    }
                                    if (comp.ComponentsType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                    {
                                        var prop = await _repo.GetSingle<ExecutionScriptComponentViewModel, ExecutionScriptComponent>(x => x.ComponentId == comp.Id);
                                        comp.Properties = JsonConvert.SerializeObject(prop);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return model;
        }
        public async Task<CommandResult<ExportTemplateViewModel>> ImportTemplate(ExportTemplateViewModel model)
        {
            if (model != null)
            {
                if (model.Template != null)
                {
                    TemplateViewModel Newtemplate = new TemplateViewModel();
                    model.Template.Id = null;
                    Newtemplate = _autoMapper.Map(model.Template, Newtemplate);// model.Template;
                    Newtemplate.Json = null;
                    Newtemplate.UdfTableMetadataId = null;
                    Newtemplate.UdfTemplateId = null;
                    Newtemplate.TableMetadataId = null;
                    Newtemplate.TableMetadata = null;
                    var result = await Create(Newtemplate);
                    Newtemplate = await GetSingleById(result.Item.Id);
                    if (result.IsSuccess)
                    {
                        switch (result.Item.TemplateType)
                        {
                            case TemplateTypeEnum.FormIndexPage:
                                break;
                            case TemplateTypeEnum.Page:
                                if (model.PageTemplate != null)
                                {
                                    var PageTemplate = await _pageTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                    model.PageTemplate.Id = PageTemplate.Id;
                                    model.PageTemplate.TemplateId = result.Item.Id;
                                    PageTemplate = _autoMapper.Map(model.PageTemplate, PageTemplate);
                                    //PageTemplate.Id = PageTemplate.Id;
                                    PageTemplate.TemplateId = result.Item.Id;
                                    if (model.Template.Json != null)
                                    {
                                        var jsondata = JObject.Parse(model.Template.Json);
                                        JArray rows = (JArray)jsondata.SelectToken("components");
                                        foreach (JObject item in rows)
                                        {
                                            item.Remove("columnMetadataId");
                                        }
                                        model.Template.Json = JsonConvert.SerializeObject(jsondata);
                                        PageTemplate.Json = model.Template.Json;
                                    }
                                    var pageresult = await _pageTemplateBusiness.Edit(PageTemplate);
                                    if (!pageresult.IsSuccess)
                                    {
                                        return CommandResult<ExportTemplateViewModel>.Instance(model, pageresult.IsSuccess, pageresult.Messages);
                                    }
                                }
                                break;
                            case TemplateTypeEnum.Form:
                                if (model.FormTemplate != null)
                                {
                                    var FormTemplate = await _formTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                    model.FormTemplate.Id = FormTemplate.Id;
                                    model.FormTemplate.TemplateId = result.Item.Id;
                                    model.FormTemplate.IndexPageTemplateId = FormTemplate.IndexPageTemplateId;
                                    FormTemplate = _autoMapper.Map(model.FormTemplate, FormTemplate);
                                    //FormTemplate.Id = FormTemplate.Id;
                                    FormTemplate.TemplateId = result.Item.Id;
                                    if (model.Template.Json != null)
                                    {
                                        var jsondata = JObject.Parse(model.Template.Json);
                                        JArray rows = (JArray)jsondata.SelectToken("components");
                                        foreach (JObject item in rows)
                                        {
                                            item.Remove("columnMetadataId");
                                        }
                                        model.Template.Json = JsonConvert.SerializeObject(jsondata);
                                        FormTemplate.Json = model.Template.Json;
                                    }
                                    var formresult = await _formTemplateBusiness.Edit(FormTemplate);
                                    if (!formresult.IsSuccess)
                                    {
                                        return CommandResult<ExportTemplateViewModel>.Instance(model, formresult.IsSuccess, formresult.Messages);
                                    }
                                    else
                                    {
                                        if (model.FormTemplate.IndexPageTemplateDetails != null)
                                        {
                                            var FormIndexTemplate = await _formIndexPageTemplateBusiness.GetSingleById(FormTemplate.IndexPageTemplateId);
                                            if (FormIndexTemplate != null)
                                            {
                                                model.FormTemplate.IndexPageTemplateDetails.Id = FormIndexTemplate.Id;
                                                model.FormTemplate.IndexPageTemplateDetails.TemplateId = FormIndexTemplate.TemplateId;
                                                FormIndexTemplate = _autoMapper.Map(model.FormTemplate.IndexPageTemplateDetails, FormIndexTemplate);
                                                FormIndexTemplate.SelectedTableRows = model.FormTemplate.IndexPageTemplateDetails.SelectedTableRows;
                                                var indexPageResult = await _formIndexPageTemplateBusiness.Edit(FormIndexTemplate);
                                            }
                                            else
                                            {
                                                model.FormTemplate.IndexPageTemplateDetails.Id = null;
                                                model.FormTemplate.IndexPageTemplateDetails.TemplateId = result.Item.Id;
                                                var indexPageResult = await _formIndexPageTemplateBusiness.Create(model.FormTemplate.IndexPageTemplateDetails);
                                            }
                                        }

                                    }
                                }
                                break;
                            case TemplateTypeEnum.Note:
                                if (model.NoteTemplate != null)
                                {
                                    var NoteTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                    model.NoteTemplate.Id = NoteTemplate.Id;
                                    model.NoteTemplate.TemplateId = result.Item.Id;
                                    model.NoteTemplate.NoteIndexPageTemplateId = NoteTemplate.NoteIndexPageTemplateId;
                                    NoteTemplate = _autoMapper.Map(model.NoteTemplate, NoteTemplate);
                                    // NoteTemplate.Id = NoteTemplate.Id;
                                    NoteTemplate.TemplateId = result.Item.Id;
                                    //NoteTemplate.NoteIndexPageTemplateId = null;
                                    if (model.Template.Json != null)
                                    {
                                        var jsondata = JObject.Parse(model.Template.Json);
                                        JArray rows = (JArray)jsondata.SelectToken("components");
                                        foreach (JObject item in rows)
                                        {
                                            item.Remove("columnMetadataId");
                                        }
                                        model.Template.Json = JsonConvert.SerializeObject(jsondata);
                                        NoteTemplate.Json = model.Template.Json;
                                    }
                                    var noteresult = await _noteTemplateBusiness.Edit(NoteTemplate);
                                    if (noteresult.IsSuccess)
                                    {
                                        if (model.NoteTemplate.NoteIndexPageTemplateDetails != null)
                                        {
                                            var NoteIndexTemplate = await _noteIndexPageTemplateBusiness.GetSingleById(NoteTemplate.NoteIndexPageTemplateId);
                                            if (NoteIndexTemplate != null)
                                            {
                                                model.NoteTemplate.NoteIndexPageTemplateDetails.Id = NoteIndexTemplate.Id;
                                                model.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = NoteIndexTemplate.TemplateId;
                                                NoteIndexTemplate = _autoMapper.Map(model.NoteTemplate.NoteIndexPageTemplateDetails, NoteIndexTemplate);
                                                var indexPageResult = await _noteIndexPageTemplateBusiness.Edit(NoteIndexTemplate);
                                            }
                                            else
                                            {
                                                model.NoteTemplate.NoteIndexPageTemplateDetails.Id = null;
                                                model.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                                var indexPageResult = await _noteIndexPageTemplateBusiness.Create(model.NoteTemplate.NoteIndexPageTemplateDetails);
                                            }
                                        }
                                    }
                                }
                                break;
                            case TemplateTypeEnum.Task:
                                var TaskTemplate = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                model.TaskTemplate.UdfTemplateId = Newtemplate.UdfTemplateId;
                                model.TaskTemplate.Id = TaskTemplate.Id;
                                model.TaskTemplate.TemplateId = result.Item.Id;
                                model.TaskTemplate.TaskIndexPageTemplateId = TaskTemplate.TaskIndexPageTemplateId;
                                //model.TaskTemplate.TaskIndexPageTemplateId = null;
                                TaskTemplate = _autoMapper.Map(model.TaskTemplate, TaskTemplate);
                                if (model.TaskTemplate.NoteUdfTemplate.Template.Json != null)
                                {
                                    var jsondata = JObject.Parse(model.TaskTemplate.NoteUdfTemplate.Template.Json);
                                    JArray rows = (JArray)jsondata.SelectToken("components");
                                    foreach (JObject item in rows)
                                    {
                                        item.Remove("columnMetadataId");
                                    }
                                    model.TaskTemplate.NoteUdfTemplate.Template.Json = JsonConvert.SerializeObject(jsondata);
                                    TaskTemplate.Json = model.TaskTemplate.NoteUdfTemplate.Template.Json;
                                }
                                var taskResult = await _taskTemplateBusiness.Edit(TaskTemplate);
                                if (!taskResult.IsSuccess)
                                {
                                    taskResult.Messages.Add("TemplateName", "" + model.Template.Name + " : Created.");
                                    return CommandResult<ExportTemplateViewModel>.Instance(model, taskResult.IsSuccess, taskResult.Messages);
                                }
                                else
                                {
                                    if (model.TaskTemplate.TaskIndexPageTemplateDetails != null)
                                    {
                                        var TaskIndexTemplate = await _taskIndexPageTemplateBusiness.GetSingleById(TaskTemplate.TaskIndexPageTemplateId);
                                        if (TaskIndexTemplate != null)
                                        {
                                            model.TaskTemplate.TaskIndexPageTemplateDetails.Id = TaskIndexTemplate.Id;
                                            model.TaskTemplate.TaskIndexPageTemplateDetails.TemplateId = TaskIndexTemplate.TemplateId;
                                            TaskIndexTemplate = _autoMapper.Map(model.TaskTemplate.TaskIndexPageTemplateDetails, TaskIndexTemplate);
                                            var indexPageResult = await _taskIndexPageTemplateBusiness.Edit(TaskIndexTemplate);
                                        }
                                        else
                                        {
                                            model.TaskTemplate.TaskIndexPageTemplateDetails.Id = null;
                                            model.TaskTemplate.TaskIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                            var indexPageResult = await _taskIndexPageTemplateBusiness.Create(model.TaskTemplate.TaskIndexPageTemplateDetails);
                                        }
                                    }
                                    var Template = await GetSingle(x => x.Id == Newtemplate.UdfTemplateId);
                                    var TaskNoteTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == Newtemplate.UdfTemplateId);
                                    #region Commented Udf Template Edit
                                    //model.TaskTemplate.NoteUdfTemplate.Template.TableMetadataId = Template.TableMetadataId;
                                    //model.TaskTemplate.NoteUdfTemplate.Template.Json = Template.Json;
                                    //model.TaskTemplate.NoteUdfTemplate.Template.Id = Template.Id;
                                    //var UDFTemplate = _autoMapper.Map(model.TaskTemplate.NoteUdfTemplate.Template, Template);                                   
                                    //var res = await Edit(UDFTemplate);
                                    #endregion
                                    //model.TaskTemplate.NoteUdfTemplate.NoteTemplate.Json = Template.Json;
                                    model.TaskTemplate.NoteUdfTemplate.NoteTemplate.TemplateId = Template.Id;
                                    model.TaskTemplate.NoteUdfTemplate.NoteTemplate.Id = TaskNoteTemplate.Id;
                                    model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateId = null;
                                    var UDFNoteTemplateData = _autoMapper.Map(model.TaskTemplate.NoteUdfTemplate.NoteTemplate, TaskNoteTemplate);
                                    UDFNoteTemplateData.Json = Template.Json;
                                    var res1 = await _noteTemplateBusiness.Edit(UDFNoteTemplateData);
                                    //if (res.IsSuccess)
                                    //{
                                    if (model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails != null)
                                    {
                                        var NoteIndexTemplate = await _noteIndexPageTemplateBusiness.GetSingleById(model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateId);
                                        if (NoteIndexTemplate != null)
                                        {
                                            model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.Id = NoteIndexTemplate.Id;
                                            model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = NoteIndexTemplate.TemplateId;
                                            NoteIndexTemplate = _autoMapper.Map(model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails, NoteIndexTemplate);
                                            var indexPageResult = await _noteIndexPageTemplateBusiness.Edit(NoteIndexTemplate);
                                        }
                                        else
                                        {
                                            model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.Id = null;
                                            model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = Template.Id;
                                            var indexPageResult = await _noteIndexPageTemplateBusiness.Create(model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails);
                                        }
                                    }
                                    //}
                                }
                                break;
                            case TemplateTypeEnum.Service:
                                var ServiceTemplate = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == Newtemplate.Id);
                                model.ServiceTemplate.UdfTemplateId = Newtemplate.UdfTemplateId;
                                model.ServiceTemplate.Id = ServiceTemplate.Id;
                                model.ServiceTemplate.TemplateId = result.Item.Id;
                                model.ServiceTemplate.ServiceIndexPageTemplateId = ServiceTemplate.ServiceIndexPageTemplateId;
                                //model.ServiceTemplate.ServiceIndexPageTemplateId = null;
                                ServiceTemplate = _autoMapper.Map(model.ServiceTemplate, ServiceTemplate);
                                if (model.ServiceTemplate.NoteUdfTemplate.Template.Json != null)
                                {
                                    var jsondata = JObject.Parse(model.ServiceTemplate.NoteUdfTemplate.Template.Json);
                                    JArray rows = (JArray)jsondata.SelectToken("components");
                                    foreach (JObject item in rows)
                                    {
                                        item.Remove("columnMetadataId");
                                    }
                                    model.ServiceTemplate.NoteUdfTemplate.Template.Json = JsonConvert.SerializeObject(jsondata);
                                    ServiceTemplate.Json = model.ServiceTemplate.NoteUdfTemplate.Template.Json;
                                }
                                var serviceresult1 = await _serviceTemplateBusiness.Edit(ServiceTemplate);
                                if (!serviceresult1.IsSuccess)
                                {
                                    serviceresult1.Messages.Add("TemplateName", "" + model.Template.Name + " : Created.");
                                    return CommandResult<ExportTemplateViewModel>.Instance(model, serviceresult1.IsSuccess, serviceresult1.Messages);
                                }
                                else
                                {
                                    if (model.ServiceTemplate.ServiceIndexPageTemplateDetails != null)
                                    {
                                        var ServiceIndexTemplate = await _serviceIndexPageTemplateBusiness.GetSingleById(ServiceTemplate.ServiceIndexPageTemplateId);
                                        if (ServiceIndexTemplate != null)
                                        {
                                            model.ServiceTemplate.ServiceIndexPageTemplateDetails.Id = ServiceIndexTemplate.Id;
                                            model.ServiceTemplate.ServiceIndexPageTemplateDetails.TemplateId = ServiceIndexTemplate.TemplateId;
                                            ServiceIndexTemplate = _autoMapper.Map(model.ServiceTemplate.ServiceIndexPageTemplateDetails, ServiceIndexTemplate);
                                            var indexPageResult = await _serviceIndexPageTemplateBusiness.Edit(ServiceIndexTemplate);
                                        }
                                        else
                                        {
                                            model.ServiceTemplate.ServiceIndexPageTemplateDetails.Id = null;
                                            model.ServiceTemplate.ServiceIndexPageTemplateDetails.TemplateId = Newtemplate.Id;
                                            var indexPageResult = await _serviceIndexPageTemplateBusiness.Create(model.ServiceTemplate.ServiceIndexPageTemplateDetails);
                                        }
                                    }
                                    var Template = await GetSingle(x => x.Id == Newtemplate.UdfTemplateId);
                                    var ServiceNoteTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == Newtemplate.UdfTemplateId);
                                    #region Commented udf Template Edit
                                    //model.ServiceTemplate.NoteUdfTemplate.Template.TableMetadataId = Template.TableMetadataId;
                                    //model.ServiceTemplate.NoteUdfTemplate.Template.Json = Template.Json;
                                    //model.ServiceTemplate.NoteUdfTemplate.Template.Id = Template.Id;                                  
                                    //var UDFTemplate = _autoMapper.Map(model.ServiceTemplate.NoteUdfTemplate.Template, Template);                                   
                                    //var res = await Edit(UDFTemplate);
                                    #endregion

                                    model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.TemplateId = Template.Id;
                                    model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.Id = ServiceNoteTemplate.Id;
                                    model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateId = null;
                                    var UDFNoteTemplateData = _autoMapper.Map(model.ServiceTemplate.NoteUdfTemplate.NoteTemplate, ServiceNoteTemplate);
                                    UDFNoteTemplateData.Json = Template.Json;
                                    var res1 = await _noteTemplateBusiness.Edit(UDFNoteTemplateData);

                                    if (model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails != null)
                                    {
                                        var NoteIndexTemplate = await _noteIndexPageTemplateBusiness.GetSingleById(model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateId);
                                        if (NoteIndexTemplate != null)
                                        {
                                            model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.Id = NoteIndexTemplate.Id;
                                            model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = NoteIndexTemplate.TemplateId;
                                            NoteIndexTemplate = _autoMapper.Map(model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails, NoteIndexTemplate);
                                            var indexPageResult = await _noteIndexPageTemplateBusiness.Edit(NoteIndexTemplate);
                                        }
                                        else
                                        {
                                            model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.Id = null;
                                            model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = Template.Id;
                                            var indexPageResult = await _noteIndexPageTemplateBusiness.Create(model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails);
                                        }
                                    }

                                    // Copy Process Design its component and its properties
                                    var processDesign = await _repo.GetSingle<ProcessDesignViewModel, ProcessDesign>(x => x.TemplateId == result.Item.Id);
                                    if (model.ServiceTemplate.ProcessDesign != null)
                                    {
                                        if (model.ServiceTemplate.ProcessDesign.Components != null)
                                        {
                                            foreach (var comp in model.ServiceTemplate.ProcessDesign.Components)
                                            {
                                                ComponentViewModel newComp = new ComponentViewModel();
                                                newComp = _autoMapper.Map(comp, newComp);
                                                newComp.Id = null;
                                                newComp.ProcessDesignId = processDesign.Id;
                                                if (comp.ComponentsType == ProcessDesignComponentTypeEnum.Start)
                                                {
                                                    newComp.Id = processDesign.Id;
                                                }
                                                var compres = await _componentBusiness.Create(newComp);
                                                //var compres = await _repo.Create<ComponentViewModel, Component>(newComp);
                                                if (compres.IsSuccess)
                                                {
                                                    comp.NewId = compres.Item.Id;
                                                    var comparents = await _componentBusiness.GetComponentParent(comp.Id);
                                                    if (comparents != null)
                                                    {
                                                        foreach (var parent in comparents)
                                                        {
                                                            ComponentParentViewModel newcomp = new ComponentParentViewModel();
                                                            newcomp.ComponentId = compres.Item.Id;
                                                            var oldparent = model.ServiceTemplate.ProcessDesign.Components.Where(x => x.Id == parent.ParentId).FirstOrDefault();
                                                            newcomp.ParentId = oldparent.NewId;
                                                            var comparentres = await _repo.Create<ComponentParentViewModel, ComponentParent>(newcomp);
                                                        }
                                                    }
                                                    if (comp.ComponentsType == ProcessDesignComponentTypeEnum.StepTask)
                                                    {
                                                        var prop = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(comp.Properties);
                                                        prop.Id = null;
                                                        prop.ComponentId = compres.Item.Id;
                                                        var propres = await _repo.Create<StepTaskComponentViewModel, StepTaskComponent>(prop);
                                                    }
                                                    if (comp.ComponentsType == ProcessDesignComponentTypeEnum.DecisionScript)
                                                    {
                                                        var prop = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(comp.Properties);
                                                        prop.Id = null;
                                                        prop.ComponentId = compres.Item.Id;
                                                        var propres = await _repo.Create<DecisionScriptComponentViewModel, DecisionScriptComponent>(prop);
                                                    }
                                                    if (comp.ComponentsType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                                    {
                                                        var prop = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(comp.Properties);
                                                        prop.Id = null;
                                                        prop.ComponentId = compres.Item.Id;
                                                        var propres = await _repo.Create<ExecutionScriptComponentViewModel, ExecutionScriptComponent>(prop);
                                                    }
                                                }
                                            }
                                            var Newexistinglist = await _componentBusiness.GetList(x => x.ProcessDesignId == processDesign.Id);
                                            if (Newexistinglist != null && Newexistinglist.Count() > 0)
                                            {
                                                foreach (var comp in Newexistinglist)
                                                {
                                                    var oldCompId = model.ServiceTemplate.ProcessDesign.Components.Where(x => x.NewId == comp.Id).FirstOrDefault();
                                                    var comparents = await _componentBusiness.GetComponentParent(oldCompId.Id);
                                                    if (comparents != null && comparents.Count() > 0)
                                                    {
                                                        foreach (var parent in comparents)
                                                        {
                                                            ComponentParentViewModel newcomp = new ComponentParentViewModel();
                                                            newcomp.ComponentId = comp.Id;
                                                            var oldparent = model.ServiceTemplate.ProcessDesign.Components.Where(x => x.Id == parent.ParentId).FirstOrDefault();
                                                            newcomp.ParentId = oldparent.NewId;
                                                            var comparentres = await _repo.Create<ComponentParentViewModel, ComponentParent>(newcomp);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case TemplateTypeEnum.Custom:
                                if (model.CustomTemplate != null)
                                {
                                    var CustomTemplate = await _customTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                    model.CustomTemplate.Id = CustomTemplate.Id;
                                    model.CustomTemplate.TemplateId = result.Item.Id;
                                    CustomTemplate = _autoMapper.Map(model.CustomTemplate, CustomTemplate);
                                    //CustomTemplate.Id = CustomTemplate.Id;
                                    CustomTemplate.TemplateId = result.Item.Id;
                                    var pageresult = await _customTemplateBusiness.Edit(CustomTemplate);
                                    if (!pageresult.IsSuccess)
                                    {
                                        return CommandResult<ExportTemplateViewModel>.Instance(model, pageresult.IsSuccess, pageresult.Messages);
                                    }
                                }
                                break;
                            case TemplateTypeEnum.ProcessDesign:

                                break;
                        }
                    }
                    else
                    {
                        return CommandResult<ExportTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
                    }
                }
                return CommandResult<ExportTemplateViewModel>.Instance(model);
            }
            return CommandResult<ExportTemplateViewModel>.Instance(model);
        }
        public async Task<CommandResult<ExportTemplateViewModel>> OverwriteTemplate(ExportTemplateViewModel model)
        {
            if (model != null)
            {
                if (model.Template != null)
                {
                    //TemplateViewModel Newtemplate = new TemplateViewModel();
                    // model.Template.Id = null;
                    var existingTemp = await GetSingleById(model.Template.Id);
                    var Newtemplate = _autoMapper.Map(model.Template, existingTemp);// model.Template;
                    //Newtemplate.Json = null;
                    //Newtemplate.UdfTableMetadataId = null;
                    //Newtemplate.UdfTemplateId = null;
                    //Newtemplate.TableMetadataId = null;
                    //Newtemplate.TableMetadata = null;
                    var result = await Edit(Newtemplate);
                    Newtemplate = await GetSingleById(result.Item.Id);
                    if (result.IsSuccess)
                    {
                        switch (result.Item.TemplateType)
                        {
                            case TemplateTypeEnum.FormIndexPage:
                                break;
                            case TemplateTypeEnum.Page:
                                if (model.PageTemplate != null)
                                {
                                    var PageTemplate = await _pageTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                    PageTemplate = _autoMapper.Map(model.PageTemplate, PageTemplate);
                                    PageTemplate.Json = model.Template.Json;
                                    var pageresult = await _pageTemplateBusiness.Edit(PageTemplate);
                                    if (!pageresult.IsSuccess)
                                    {
                                        return CommandResult<ExportTemplateViewModel>.Instance(model, pageresult.IsSuccess, pageresult.Messages);
                                    }
                                }
                                break;
                            case TemplateTypeEnum.Form:
                                if (model.FormTemplate != null)
                                {
                                    var FormTemplate = await _formTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                    FormTemplate = _autoMapper.Map(model.FormTemplate, FormTemplate);
                                    FormTemplate.Json = model.Template.Json;
                                    var formresult = await _formTemplateBusiness.Edit(FormTemplate);
                                    if (!formresult.IsSuccess)
                                    {
                                        return CommandResult<ExportTemplateViewModel>.Instance(model, formresult.IsSuccess, formresult.Messages);
                                    }
                                    else
                                    {
                                        if (model.FormTemplate.IndexPageTemplateDetails != null)
                                        {
                                            var FormIndexTemplate = await _formIndexPageTemplateBusiness.GetSingle(x => x.Id == model.FormTemplate.IndexPageTemplateDetails.Id);
                                            if (FormIndexTemplate != null)
                                            {
                                                FormIndexTemplate = _autoMapper.Map(model.FormTemplate.IndexPageTemplateDetails, FormIndexTemplate);
                                                model.FormTemplate.IndexPageTemplateDetails.TemplateId = result.Item.Id;
                                                FormIndexTemplate.SelectedTableRows = model.FormTemplate.IndexPageTemplateDetails.SelectedTableRows;
                                                var indexPageResult = await _formIndexPageTemplateBusiness.Edit(model.FormTemplate.IndexPageTemplateDetails);
                                            }
                                            else
                                            {
                                                model.FormTemplate.IndexPageTemplateDetails.Id = null;
                                                model.FormTemplate.IndexPageTemplateDetails.TemplateId = result.Item.Id;
                                                var indexPageResult = await _formIndexPageTemplateBusiness.Create(model.FormTemplate.IndexPageTemplateDetails);
                                            }
                                        }
                                    }
                                }
                                break;
                            case TemplateTypeEnum.Note:
                                if (model.NoteTemplate != null)
                                {
                                    var NoteTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                    NoteTemplate = _autoMapper.Map(model.NoteTemplate, NoteTemplate);
                                    NoteTemplate.Json = model.Template.Json;
                                    var noteresult = await _noteTemplateBusiness.Edit(NoteTemplate);
                                    if (noteresult.IsSuccess)
                                    {
                                        if (model.NoteTemplate.NoteIndexPageTemplateDetails != null)
                                        {
                                            var NoteIndexTemplate = await _noteIndexPageTemplateBusiness.GetSingle(x => x.Id == model.NoteTemplate.NoteIndexPageTemplateDetails.Id);
                                            if (NoteIndexTemplate != null)
                                            {
                                                NoteIndexTemplate = _autoMapper.Map(model.NoteTemplate.NoteIndexPageTemplateDetails, NoteIndexTemplate);
                                                model.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                                NoteIndexTemplate.SelectedTableRows = model.NoteTemplate.NoteIndexPageTemplateDetails.SelectedTableRows;
                                                var indexPageResult = await _noteIndexPageTemplateBusiness.Edit(model.NoteTemplate.NoteIndexPageTemplateDetails);
                                            }
                                            else
                                            {
                                                model.NoteTemplate.NoteIndexPageTemplateDetails.Id = null;
                                                model.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                                var indexPageResult = await _noteIndexPageTemplateBusiness.Create(model.NoteTemplate.NoteIndexPageTemplateDetails);
                                            }
                                        }
                                    }
                                }
                                break;
                            case TemplateTypeEnum.Task:
                                var TaskTemplate = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                model.TaskTemplate.UdfTemplateId = Newtemplate.UdfTemplateId;
                                model.TaskTemplate.Id = TaskTemplate.Id;
                                model.TaskTemplate.TemplateId = result.Item.Id;
                                TaskTemplate = _autoMapper.Map(model.TaskTemplate, TaskTemplate);
                                TaskTemplate.Json = model.Template.Json;
                                var taskResult = await _taskTemplateBusiness.Edit(TaskTemplate);
                                if (!taskResult.IsSuccess)
                                {
                                    taskResult.Messages.Add("TemplateName", "" + model.Template.Name + " : Created.");
                                    return CommandResult<ExportTemplateViewModel>.Instance(model, taskResult.IsSuccess, taskResult.Messages);
                                }
                                else
                                {
                                    if (model.TaskTemplate.TaskIndexPageTemplateDetails != null)
                                    {
                                        var TaskIndexTemplate = await _taskIndexPageTemplateBusiness.GetSingle(x => x.Id == model.TaskTemplate.TaskIndexPageTemplateDetails.Id);
                                        if (TaskIndexTemplate != null)
                                        {
                                            TaskIndexTemplate = _autoMapper.Map(model.TaskTemplate.TaskIndexPageTemplateDetails, TaskIndexTemplate);
                                            model.TaskTemplate.TaskIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                            TaskIndexTemplate.SelectedTableRows = model.TaskTemplate.TaskIndexPageTemplateDetails.SelectedTableRows;
                                            var indexPageResult = await _taskIndexPageTemplateBusiness.Edit(model.TaskTemplate.TaskIndexPageTemplateDetails);
                                        }
                                        else
                                        {
                                            model.TaskTemplate.TaskIndexPageTemplateDetails.Id = null;
                                            model.TaskTemplate.TaskIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                            var indexPageResult = await _taskIndexPageTemplateBusiness.Create(model.TaskTemplate.TaskIndexPageTemplateDetails);
                                        }
                                    }
                                    var Template = await GetSingle(x => x.Id == Newtemplate.UdfTemplateId);
                                    var TaskNoteTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == Newtemplate.UdfTemplateId);
                                    model.TaskTemplate.NoteUdfTemplate.NoteTemplate.TemplateId = Template.Id;
                                    model.TaskTemplate.NoteUdfTemplate.NoteTemplate.Id = TaskNoteTemplate.Id;
                                    model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateId = null;
                                    var UDFNoteTemplateData = _autoMapper.Map(model.TaskTemplate.NoteUdfTemplate.NoteTemplate, TaskNoteTemplate);
                                    UDFNoteTemplateData.Json = Template.Json;
                                    var res1 = await _noteTemplateBusiness.Edit(UDFNoteTemplateData);
                                    //if (res.IsSuccess)
                                    //{
                                    if (model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails != null)
                                    {
                                        var NoteIndexTemplate = await _noteIndexPageTemplateBusiness.GetSingle(x => x.Id == model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.Id);
                                        if (NoteIndexTemplate != null)
                                        {
                                            NoteIndexTemplate = _autoMapper.Map(model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails, NoteIndexTemplate);
                                            model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                            NoteIndexTemplate.SelectedTableRows = model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.SelectedTableRows;
                                            var indexPageResult = await _noteIndexPageTemplateBusiness.Edit(model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails);
                                        }
                                        else
                                        {
                                            model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.Id = null;
                                            model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = Template.Id;
                                            var indexPageResult = await _noteIndexPageTemplateBusiness.Create(model.TaskTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails);
                                        }
                                    }
                                    //}
                                }
                                break;
                            case TemplateTypeEnum.Service:
                                var ServiceTemplate = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == Newtemplate.Id);
                                model.ServiceTemplate.UdfTemplateId = Newtemplate.UdfTemplateId;
                                model.ServiceTemplate.Id = ServiceTemplate.Id;
                                model.ServiceTemplate.TemplateId = result.Item.Id;
                                ServiceTemplate = _autoMapper.Map(model.ServiceTemplate, ServiceTemplate);
                                ServiceTemplate.Json = model.Template.Json;
                                var serviceresult1 = await _serviceTemplateBusiness.Edit(ServiceTemplate);
                                if (!serviceresult1.IsSuccess)
                                {
                                    serviceresult1.Messages.Add("TemplateName", "" + model.Template.Name + " : Created.");
                                    return CommandResult<ExportTemplateViewModel>.Instance(model, serviceresult1.IsSuccess, serviceresult1.Messages);
                                }
                                else
                                {
                                    if (model.ServiceTemplate.ServiceIndexPageTemplateDetails != null)
                                    {
                                        var ServiceIndexTemplate = await _serviceIndexPageTemplateBusiness.GetSingle(x => x.Id == model.ServiceTemplate.ServiceIndexPageTemplateDetails.Id);
                                        if (ServiceIndexTemplate != null)
                                        {
                                            ServiceIndexTemplate = _autoMapper.Map(model.ServiceTemplate.ServiceIndexPageTemplateDetails, ServiceIndexTemplate);
                                            model.ServiceTemplate.ServiceIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                            ServiceIndexTemplate.SelectedTableRows = model.ServiceTemplate.ServiceIndexPageTemplateDetails.SelectedTableRows;
                                            var indexPageResult = await _serviceIndexPageTemplateBusiness.Edit(model.ServiceTemplate.ServiceIndexPageTemplateDetails);
                                        }
                                        else
                                        {
                                            model.ServiceTemplate.ServiceIndexPageTemplateDetails.Id = null;
                                            model.ServiceTemplate.ServiceIndexPageTemplateDetails.TemplateId = Newtemplate.Id;
                                            var indexPageResult = await _serviceIndexPageTemplateBusiness.Create(model.ServiceTemplate.ServiceIndexPageTemplateDetails);
                                        }
                                    }
                                    var Template = await GetSingle(x => x.Id == Newtemplate.UdfTemplateId);
                                    var ServiceNoteTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == Newtemplate.UdfTemplateId);


                                    model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.TemplateId = Template.Id;
                                    model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.Id = ServiceNoteTemplate.Id;
                                    model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateId = null;
                                    var UDFNoteTemplateData = _autoMapper.Map(model.ServiceTemplate.NoteUdfTemplate.NoteTemplate, ServiceNoteTemplate);
                                    UDFNoteTemplateData.Json = Template.Json;
                                    var res1 = await _noteTemplateBusiness.Edit(UDFNoteTemplateData);
                                    if (model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails != null)
                                    {
                                        var NoteIndexTemplate = await _noteIndexPageTemplateBusiness.GetSingle(x => x.Id == model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.Id);
                                        if (NoteIndexTemplate != null)
                                        {
                                            NoteIndexTemplate = _autoMapper.Map(model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails, NoteIndexTemplate);
                                            model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = result.Item.Id;
                                            NoteIndexTemplate.SelectedTableRows = model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.SelectedTableRows;
                                            var indexPageResult = await _noteIndexPageTemplateBusiness.Edit(model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails);
                                        }
                                        else
                                        {
                                            model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.Id = null;
                                            model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails.TemplateId = Template.Id;
                                            var indexPageResult = await _noteIndexPageTemplateBusiness.Create(model.ServiceTemplate.NoteUdfTemplate.NoteTemplate.NoteIndexPageTemplateDetails);
                                        }
                                    }
                                    // Copy Process Design its component and its properties
                                    var processDesign = await _repo.GetSingle<ProcessDesignViewModel, ProcessDesign>(x => x.TemplateId == result.Item.Id);
                                    if (model.ServiceTemplate.ProcessDesign != null)
                                    {
                                        var existinglist = await _componentBusiness.GetList(x => x.ProcessDesignId == processDesign.Id);
                                        if (existinglist != null)
                                        {
                                            foreach (var item in existinglist)
                                            {
                                                var type = await _componentBusiness.GetSingleById(item.Id);
                                                if (type.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                                                {
                                                    var steptaskcomplist = await _stepTaskComponentBusiness.GetList(x => x.ComponentId == item.Id);
                                                    if (steptaskcomplist != null)
                                                    {
                                                        foreach (var stc in steptaskcomplist)
                                                        {
                                                            await _stepTaskComponentBusiness.Delete(stc.Id);
                                                        }
                                                    }
                                                }
                                                if (type.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                                                {
                                                    var descscriptcomplist = await _decisionScriptBusiness.GetList(x => x.ComponentId == item.Id);
                                                    if (descscriptcomplist != null)
                                                    {
                                                        foreach (var dsc in descscriptcomplist)
                                                        {
                                                            await _decisionScriptBusiness.Delete(dsc.Id);
                                                        }
                                                    }
                                                }
                                                if (type.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                                {
                                                    var exescriptcomplist = await _executionScriptBusiness.GetList(x => x.ComponentId == item.Id);
                                                    if (exescriptcomplist != null)
                                                    {
                                                        foreach (var exe in exescriptcomplist)
                                                        {
                                                            await _executionScriptBusiness.Delete(exe.Id);
                                                        }
                                                    }
                                                }
                                                if (type.ComponentType != ProcessDesignComponentTypeEnum.Start)
                                                {
                                                    await _componentBusiness.Delete(item.Id);
                                                }
                                            }
                                        }
                                        if (model.ServiceTemplate.ProcessDesign.Components != null)
                                        {
                                            foreach (var comp in model.ServiceTemplate.ProcessDesign.Components)
                                            {
                                                ComponentViewModel newComp = new ComponentViewModel();
                                                newComp = _autoMapper.Map(comp, newComp);
                                                newComp.Id = null;
                                                newComp.ProcessDesignId = processDesign.Id;
                                                if (comp.ComponentType == ProcessDesignComponentTypeEnum.Start)
                                                {
                                                    newComp.Id = processDesign.Id;
                                                    comp.NewId = comp.Id;
                                                }
                                                else
                                                {
                                                    var compres = await _componentBusiness.Create(newComp); //_repo.Create<ComponentViewModel, Component>(newComp);
                                                    if (compres.IsSuccess)
                                                    {
                                                        comp.NewId = compres.Item.Id;
                                                        if (comp.ComponentsType == ProcessDesignComponentTypeEnum.StepTask)
                                                        {
                                                            var prop = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(comp.Properties);
                                                            prop.Id = null;
                                                            prop.ComponentId = compres.Item.Id;
                                                            var propres = await _repo.Create<StepTaskComponentViewModel, StepTaskComponent>(prop);
                                                        }
                                                        if (comp.ComponentsType == ProcessDesignComponentTypeEnum.DecisionScript)
                                                        {
                                                            var prop = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(comp.Properties);
                                                            prop.Id = null;
                                                            prop.ComponentId = compres.Item.Id;
                                                            var propres = await _repo.Create<DecisionScriptComponentViewModel, DecisionScriptComponent>(prop);
                                                        }
                                                        if (comp.ComponentsType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                                        {
                                                            var prop = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(comp.Properties);
                                                            prop.Id = null;
                                                            prop.ComponentId = compres.Item.Id;
                                                            var propres = await _repo.Create<ExecutionScriptComponentViewModel, ExecutionScriptComponent>(prop);
                                                        }
                                                    }
                                                }

                                            }


                                            var Newexistinglist = await _componentBusiness.GetList(x => x.ProcessDesignId == processDesign.Id);
                                            if (Newexistinglist != null && Newexistinglist.Count() > 0)
                                            {
                                                foreach (var comp in Newexistinglist)
                                                {
                                                    var oldCompId = model.ServiceTemplate.ProcessDesign.Components.Where(x => x.NewId == comp.Id).FirstOrDefault();
                                                    var comparents = await _componentBusiness.GetComponentParent(oldCompId.Id);
                                                    if (comparents != null)
                                                    {
                                                        foreach (var parent in comparents)
                                                        {
                                                            ComponentParentViewModel newcomp = new ComponentParentViewModel();
                                                            newcomp.ComponentId = comp.Id;
                                                            var oldparent = model.ServiceTemplate.ProcessDesign.Components.Where(x => x.Id == parent.ParentId).FirstOrDefault();
                                                            newcomp.ParentId = oldparent.NewId;
                                                            var comparentres = await _repo.Create<ComponentParentViewModel, ComponentParent>(newcomp);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case TemplateTypeEnum.Custom:
                                if (model.CustomTemplate != null)
                                {
                                    var CustomTemplate = await _customTemplateBusiness.GetSingle(x => x.TemplateId == result.Item.Id);
                                    model.CustomTemplate.Id = CustomTemplate.Id;
                                    model.CustomTemplate.TemplateId = result.Item.Id;
                                    CustomTemplate = _autoMapper.Map(model.CustomTemplate, CustomTemplate);
                                    CustomTemplate.Id = CustomTemplate.Id;
                                    CustomTemplate.TemplateId = result.Item.Id;
                                    var pageresult = await _customTemplateBusiness.Edit(CustomTemplate);
                                    if (!pageresult.IsSuccess)
                                    {
                                        return CommandResult<ExportTemplateViewModel>.Instance(model, pageresult.IsSuccess, pageresult.Messages);
                                    }
                                }
                                break;
                            case TemplateTypeEnum.ProcessDesign:

                                break;
                        }
                    }
                    else
                    {
                        return CommandResult<ExportTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
                    }
                }
                return CommandResult<ExportTemplateViewModel>.Instance(model);
            }
            return CommandResult<ExportTemplateViewModel>.Instance(model);
        }
        public async Task<string> GenerateJsonDynamicallyFromTableMetaData(TemplateViewModel model)
        {
            var columnMetaData = await _columnMetadataBusiness.GetViewableColumnMetadataList(model.TableMetadataId, model.TemplateType);
            columnMetaData = columnMetaData.Where(x => x.IsUdfColumn == true).ToList();
            dynamic expandoForm = new ExpandoObject();
            expandoForm.display = "Form";
            var list = new List<ExpandoObject>();
            foreach (var item in columnMetaData)
            {
                expandoForm.components = new List<ExpandoObject>();
                dynamic expandoComp = new ExpandoObject();
                expandoComp.label = item.LabelName;
                expandoComp.key = item.Name;
                switch (item.DataType)
                {
                    case DataColumnTypeEnum.Text:
                        if (item.ForeignKeyTableId.IsNotNullAndNotEmpty())
                        {
                            expandoComp.type = "select";
                            expandoComp.widget = "choicesjs";
                            expandoComp.tableView = true;
                        }
                        else
                        {
                            expandoComp.type = "textfield";
                        }
                        break;
                    case DataColumnTypeEnum.Bool:
                        expandoComp.type = "checkbox";
                        break;
                    case DataColumnTypeEnum.DateTime:
                        expandoComp.type = "datetime";
                        break;
                    case DataColumnTypeEnum.Integer:
                    case DataColumnTypeEnum.Long:
                    case DataColumnTypeEnum.Double:
                        expandoComp.type = "number";
                        break;

                }
                expandoComp.input = true;

                list.Add(expandoComp);


            }
            expandoForm.components = list;



            var json = JsonConvert.SerializeObject(expandoForm);

            return json;
        }
        public async Task<List<TemplateViewModel>> GetNoteTemplateList(string tCode, string tcCode, string mCodes, string templateIds = null, string categoryIds = null, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard,string portalNames=null)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId""
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""TemplateCategoryType""={(int)categoryType} and tc.""IsDeleted""=false 
                        join public.""NoteTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""=false 
left join public.""Module"" as m on m.""Id""=tc.""ModuleId"" and m.""IsDeleted""=false  
join public.""Portal"" as p on t.""PortalId""=p.""Id"" and p.""IsDeleted""=false #PORTALWHERE#
where t.""TemplateType"" =4 and t.""IsDeleted""=false #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# #categoryIdSearch#";


            var portalWhere = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalWhere = @$" and p.""Name"" in ('{portalNames.Replace(",", "','")}') ";
            }
            else
            {
                portalWhere = @$" and p.""Id""='{_repo.UserContext.PortalId}'";
            }
            query = query.Replace("#PORTALWHERE#", portalWhere);

            var templatecodeSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = @" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);
            var modulecodeSearch = "";
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);
            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);
            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }
            query = query.Replace("#categoryIdSearch#", categoryIdSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }

        public async Task<List<TemplateViewModel>> GetTemplateList(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames=null)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId"", tt.""TaskTemplateType"" as TaskType
                        from public.""Template"" as t                        
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""TemplateCategoryType""={(int)categoryType}
                        left join public.""Module"" as m on m.""Id""=tc.""ModuleId"" and m.""IsDeleted""=false 
                        join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""=false and tt.""TaskTemplateType"" != 2
                        join public.""Portal"" as p on t.""PortalId""=p.""Id"" and p.""IsDeleted""=false #PORTALWHERE#
                        where t.""TemplateType"" = 5 and t.""IsDeleted""=false #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# #categoryIdSearch#";

            var portalWhere="";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalWhere = @$" and p.""Name"" in ('{portalNames.Replace(",","','")}') ";
            }
            else
            {
                portalWhere = @$" and p.""Id""='{_repo.UserContext.PortalId}'";
            }
            query = query.Replace("#PORTALWHERE#", portalWhere);

            var templatecodeSearch = "";
            var modulecodeSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = @" and t.""Code"" in (" + tCode + ") ";
            }
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);
            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);
            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }
            query = query.Replace("#categoryIdSearch#", categoryIdSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }
        public async Task<List<TemplateViewModel>> GetAdhocTemplateList(string tCode, string tcCode, string moduleId)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId"", tt.""TaskTemplateType"" as TaskType
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                        join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""=false 
                        where tt.""TaskTemplateType"" = 4 and t.""PortalId""='{_repo.UserContext.PortalId}' and t.""TemplateType"" = 5 and t.""IsDeleted""=false #templatecodesearch# #categorycodesearch# #module# ";

            var templatecodeSearch = "";
            var moduleSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = @" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);

            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            if (!moduleId.IsNullOrEmpty())
            {
                moduleSearch = @" and t.""ModuleId"" = '" + moduleId + "'";
            }
            query = query.Replace("#module#", moduleSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }
        public async Task DeleteTemplate(string TemplateId)
        {
            var page = await GetSingleById(TemplateId);
            if (page != null)
            {
                if (page.TemplateType == TemplateTypeEnum.Page)
                {
                    var PageTemplate = await _pageTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                    // var tableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.TableMetadataId);
                    if (PageTemplate != null)
                    {
                        await _pageTemplateBusiness.Delete(PageTemplate.Id);
                    }
                }
                else if (page.TemplateType == TemplateTypeEnum.Form)
                {
                    var FormTemplate = await _formTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                    var tableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.TableMetadataId);
                    if (tableMetadata != null)
                    {
                        await _repo.Delete<TableMetadataViewModel, TableMetadata>(tableMetadata.Id);
                    }
                    if (FormTemplate != null)
                    {
                        var FormIndexTemplate = await _formIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                        if (FormIndexTemplate != null)
                        {
                            var FormColumnTemplate = await _repo.GetList<FormIndexPageColumnViewModel, FormIndexPageColumn>(x => x.FormIndexPageTemplateId == FormIndexTemplate.Id);
                            foreach (var col in FormColumnTemplate)
                            {
                                await _repo.Delete<FormIndexPageColumnViewModel, FormIndexPageColumn>(col.Id);
                            }
                            await _formIndexPageTemplateBusiness.Delete(FormIndexTemplate.Id);
                        }
                        await _formTemplateBusiness.Delete(FormTemplate.Id);
                    }
                }
                else if (page.TemplateType == TemplateTypeEnum.Note)
                {
                    var NoteTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                    var tableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.TableMetadataId);
                    if (tableMetadata != null)
                    {
                        await _repo.Delete<TableMetadataViewModel, TableMetadata>(tableMetadata.Id);
                    }
                    if (NoteTemplate != null)
                    {
                        var NoteIndexTemplate = await _noteIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                        if (NoteIndexTemplate != null)
                        {
                            var NoteColumnTemplate = await _repo.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == NoteIndexTemplate.Id);
                            foreach (var col in NoteColumnTemplate)
                            {
                                await _repo.Delete<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(col.Id);
                            }
                            await _noteIndexPageTemplateBusiness.Delete(NoteIndexTemplate.Id);
                        }
                        await _noteTemplateBusiness.Delete(NoteTemplate.Id);
                    }
                }
                else if (page.TemplateType == TemplateTypeEnum.Task)
                {
                    var TaskTemplate = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                    var NoteTemplate = await GetSingleById(page.UdfTemplateId);
                    if (NoteTemplate != null)
                    {
                        await DeleteTemplate(NoteTemplate.Id);
                    }
                    if (TaskTemplate != null)
                    {
                        var TaskIndexTemplate = await _taskIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                        if (TaskIndexTemplate != null)
                        {
                            var TaskColumnTemplate = await _repo.GetList<TaskIndexPageColumnViewModel, TaskIndexPageColumn>(x => x.TaskIndexPageTemplateId == TaskIndexTemplate.Id);
                            foreach (var col in TaskColumnTemplate)
                            {
                                await _repo.Delete<TaskIndexPageColumnViewModel, TaskIndexPageColumn>(col.Id);
                            }
                            await _taskIndexPageTemplateBusiness.Delete(TaskIndexTemplate.Id);
                        }
                        await _taskTemplateBusiness.Delete(TaskTemplate.Id);
                    }
                }
                else if (page.TemplateType == TemplateTypeEnum.Service)
                {
                    var ServiceTemplate = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                    var NoteTemplate = await GetSingleById(page.UdfTemplateId);
                    if (NoteTemplate != null)
                    {
                        await DeleteTemplate(NoteTemplate.Id);
                    }
                    if (ServiceTemplate != null)
                    {
                        var ServiceIndexTemplate = await _serviceIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                        if (ServiceIndexTemplate != null)
                        {
                            var ServiceColumnTemplate = await _repo.GetList<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(x => x.ServiceIndexPageTemplateId == ServiceIndexTemplate.Id);
                            foreach (var col in ServiceColumnTemplate)
                            {
                                await _repo.Delete<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(col.Id);
                            }
                            await _serviceIndexPageTemplateBusiness.Delete(ServiceIndexTemplate.Id);
                        }
                        await _serviceTemplateBusiness.Delete(ServiceTemplate.Id);
                    }
                }
                if (page.TemplateType == TemplateTypeEnum.Custom)
                {
                    var CustomTemplate = await _customTemplateBusiness.GetSingle(x => x.TemplateId == TemplateId);
                    // var tableMetadata = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.TableMetadataId);
                    if (CustomTemplate != null)
                    {
                        await _pageTemplateBusiness.Delete(CustomTemplate.Id);
                    }
                }
                await _repo.Delete(TemplateId);

            }
        }
        public async Task<List<TemplateViewModel>> GetTemplateServiceList(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null, ServiceTypeEnum serviceType = ServiceTypeEnum.StandardService)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"",t.""SequenceOrder"", tc.""Code"" as CategoryCode, st.""IconFileId"", ct.""IconFileId"" as ""CustomIcon"", t.""Code"",
                        ct.""ActionName"",ct.""ControllerName"",ct.""AreaName"",ct.""Parameter"",t.""ViewType"" as ViewType,st.""ServiceTemplateType""
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""TemplateCategoryType""={(int)categoryType}
                        and tc.""IsDeleted""=false 
                        left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false 
                        left join public.""CustomTemplate"" as ct on ct.""TemplateId""=t.""Id"" and ct.""IsDeleted""=false
                        left join public.""Module"" as m on m.""Id""=tc.""ModuleId"" and m.""IsDeleted""=false
                        join public.""Portal"" as p on t.""PortalId""=p.""Id"" and p.""IsDeleted""=false #PORTALWHERE#
                        where t.""IsDeleted""=false and  (t.""TemplateType"" =6 or t.""TemplateType"" =7) 
                        #servicetypewhere#
                        --and (""ServiceTemplateType""=1 or ""ServiceTemplateType"" is null)
                        --and t.""PortalId""='{_repo.UserContext.PortalId}' 
                        #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# #categoryIdSearch#";

            var serTypeWhere = "";
            if(serviceType == ServiceTypeEnum.StandardService)
            {
                serTypeWhere = $@"and (""ServiceTemplateType""=1 or ""ServiceTemplateType"" is null)";
            }
            else
            {
                serTypeWhere = $@"and (""ServiceTemplateType""={(int)serviceType})";
            }
            query = query.Replace("#servicetypewhere#", serTypeWhere);

            var portalwhere = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalwhere = $@" and p.""Name"" in ('{portalNames.Replace(",","','")}') ";
            }
            else
            {
                portalwhere = $@" and p.""Id""='{_repo.UserContext.PortalId}'";
            }
            query = query.Replace("#PORTALWHERE#", portalwhere);

            var templatecodeSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = @" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);
            var modulecodeSearch = "";
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);

            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);
            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }

            query = query.Replace("#categoryIdSearch#", categoryIdSearch);


            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            if (allBooks)
            {
                list = list.Where(x => x.ViewType == NtsViewTypeEnum.Book).ToList();
            }
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }
        public async Task<List<TemplateViewModel>> GetTemplateListByTaskTemplate(string taskTemplateId)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId"", tt.""TaskTemplateType"" as TaskType
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                        join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""=false 
                        #WHERE# ";
            var where = @$" WHERE t.""IsDeleted""=false ";
            if (taskTemplateId.IsNotNullAndNotEmpty())
            {
                var adhocItems = taskTemplateId.Split(',');
                var adhocText = "";
                foreach (var item in adhocItems)
                {
                    adhocText = $"{adhocText},'{item}'";
                }
                var adhocId = $@" and t.""Id"" IN ({adhocText.Trim(',')}) ";
                where = where + adhocId;
            }
            else
            {
                var adhocId = $@" and tt.""Id"" IN ('') ";
                where = where + adhocId;
            }
            query = query.Replace("#WHERE#", where);
            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return list;
        }

        public async Task<List<BusinessDiagramNodeViewModel>> GetTemplateBusinessDiagram(string templateId)
        {
            var query = @$"select ""T"".""Id"" as ""Id"",""T"".""Id"" as ""ReferenceId"",null as ""ParentIds"",""T"".""DisplayName"" as ""Title""
            ,""T"".""Description"" as ""Description"",""T"".""TemplateType"" as ""TemplateType"",'TEMPLATE' as ""Type"",3 as ""NodeShape""
            from public.""Template"" as ""T"" 
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false
            union
            select ""CM"".""Id"" as ""Id"",""CM"".""Id"" as ""ReferenceId"",'UDF_ROOT' as ""ParentIds"",""CM"".""LabelName"" as ""Title""
            ,""CM"".""LabelName"" as ""Description"",""T"".""TemplateType"" as ""TemplateType"",'UDF' as ""Type""
            ,1 as ""NodeShape""
            from public.""Template"" as ""T"" 
            join public.""TableMetadata"" as ""TM"" on coalesce(""T"".""TableMetadataId"",""T"".""UdfTableMetadataId"")=""TM"".""Id""
            join public.""ColumnMetadata"" as ""CM"" on ""TM"".""Id""=""CM"".""TableMetadataId"" and ""CM"".""IsUdfColumn""=true
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""TM"".""IsDeleted""=false and ""CM"".""IsDeleted""=false
            union
            select 'PRE_'||""ST"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId"",'PRE_ACTION_ROOT' as ""ParentIds"",coalesce(""ST"".""Description"",""ST"".""Name"") as ""Title""
            ,""ST"".""Description"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,'PRE_ACTION' as ""Type"",1 as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS' 
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""BR"".""BusinessLogicExecutionType""=0 and ""BR"".""TemplateId""=""T"".""Id""
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false and ""BR"".""IsDeleted""=false
            union
            select 'POST_'||""ST"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId"",'POST_ACTION_ROOT' as ""ParentIds"",coalesce(""ST"".""Description"",""ST"".""Name"") as ""Title""
            ,""ST"".""Description"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,'POST_ACTION' as ""Type"",1 as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS'
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""BR"".""BusinessLogicExecutionType""=1 and ""BR"".""TemplateId""=""T"".""Id""
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false and ""BR"".""IsDeleted""=false
            union
            select  ""BN"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId""
            ,case when ""BN"".""IsStarter""=true then  'PRE_'||""ST"".""Id"" else ""BC"".""SourceId"" end as ""ParentIds""
            ,""BN"".""Name"" as ""Title""
            ,""BN"".""Name"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,case when ""BN"".""Type""='0' and ""BN"".""IsStarter""=true then 'BR_START' 
            when ""BN"".""Type""='0' and ""BN"".""IsStarter""=false then 'BR_STOP' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=true  then 'BR_TRUE' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=false  then 'BR_FALSE' 
            when ""BN"".""Type""='2' then 'BR_DECISION' when ""BN"".""Type""='1' then 'BR_PROCESS' end as ""Type""
            ,case when ""BN"".""Type""='3' then 0 when ""BN"".""Type""='0' then 4
            else ""BN"".""Type"" end as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS'
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""T"".""Id""=""BR"".""TemplateId"" and ""BR"".""BusinessLogicExecutionType""=0
            join public.""BusinessRuleNode"" as ""BN"" on ""BR"".""Id""=""BN"".""BusinessRuleId""  
            left join public.""BusinessRuleConnector"" as ""BC"" on ""BN"".""Id""=""BC"".""TargetId"" 
            and ""BR"".""Id""=""BC"".""BusinessRuleId"" and ""BC"".""IsDeleted""=false
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false
            and ""BR"".""IsDeleted""=false and ""BN"".""IsDeleted""=false
            union
            select  ""BN"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId""
            ,case when ""BN"".""IsStarter""=true then  'POST_'||""ST"".""Id"" else ""BC"".""SourceId"" end as ""ParentIds""
            ,""BN"".""Name"" as ""Title""
            ,""BN"".""Name"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,case when ""BN"".""Type""='0' and ""BN"".""IsStarter""=true then 'BR_START' 
            when ""BN"".""Type""='0' and ""BN"".""IsStarter""=false then 'BR_STOP' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=true  then 'BR_TRUE' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=false  then 'BR_FALSE'
            when ""BN"".""Type""='2' then 'BR_DECISION' when ""BN"".""Type""='1' then 'BR_PROCESS' end as ""Type""
            ,case when ""BN"".""Type""='3' then 0 when ""BN"".""Type""='0' then 4
            else ""BN"".""Type"" end as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS'
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""T"".""Id""=""BR"".""TemplateId"" and ""BR"".""BusinessLogicExecutionType""=1
            join public.""BusinessRuleNode"" as ""BN"" on ""BR"".""Id""=""BN"".""BusinessRuleId""  
            left join public.""BusinessRuleConnector"" as ""BC"" on ""BN"".""Id""=""BC"".""TargetId"" 
            and ""BR"".""Id""=""BC"".""BusinessRuleId"" and ""BC"".""IsDeleted""=false
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false
            and ""BR"".""IsDeleted""=false and ""BN"".""IsDeleted""=false
            union
            select ""C"".""Id"" as ""Id"",""STC"".""TemplateId"" as ""ReferenceId""
             ,case when ""C"".""ComponentType""='1' then 'WF_ROOT' when ""C"".""ComponentType""='4' and ""C"".""ParentId"" <> ""cp"".""ParentId""
            then CONCAT(""C"".""ParentId"",',' ,""cp"".""ParentId"")
            else ""C"".""ParentId"" end as ""ParentIds""

            ,""C"".""Name"" as ""Title"",""C"".""Name"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,case when ""C"".""ComponentType""='1' then 'WF_START' when ""C"".""ComponentType""='2' then 'WF_STOP'
            when ""C"".""ComponentType""='8' then 'WF_TRUE' when ""C"".""ComponentType""='9' then 'WF_FALSE' 
            when ""C"".""ComponentType""='4' then 'WF_STEP_TASK' when ""C"".""ComponentType""='6' then 'WF_DECISION' end as ""Type""
            ,case when ""C"".""ComponentType""='1' then 4 when ""C"".""ComponentType""='2' then 4 
            when ""C"".""ComponentType""='8' then 0 when ""C"".""ComponentType""='9' then 0 
            when ""C"".""ComponentType""='4' then 3 when ""C"".""ComponentType""='6' then 2 end as ""NodeShape""
            from public.""Template"" as ""T""  
            join public.""ProcessDesign"" as ""PD"" on ""T"".""Id""=""PD"".""TemplateId""
            join public.""Component"" as ""C"" on ""PD"".""Id""=""C"".""ProcessDesignId""
            left join public.""ComponentParent"" as cp on ""C"".""Id""=cp.""ComponentId""
            left join public.""StepTaskComponent"" as ""STC"" on  ""STC"".""ComponentId"" = ""C"".""Id"" and ""STC"".""IsDeleted""=false
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""PD"".""IsDeleted""=false and ""C"".""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<BusinessDiagramNodeViewModel>(query, null);
            if (list != null && list.Any())
            {
                var template = list.FirstOrDefault(x => x.Id == templateId);

                if (template.TemplateType != TemplateTypeEnum.Page)
                {
                    list.Add(new BusinessDiagramNodeViewModel
                    {
                        Id = "UDF_ROOT",
                        Title = "Udfs",
                        Description = "User Defined Fields",
                        ReferenceId = "UDF_ROOT",
                        ParentIds = templateId,
                        Type = "UDF_ROOT",
                        TemplateType = template.TemplateType,
                        NodeShape = NodeShapeEnum.SquareWithHeader
                    });

                    list.Add(new BusinessDiagramNodeViewModel
                    {
                        Id = "ACTION_ROOT",
                        Title = "Actions",
                        Description = "Actions",
                        ReferenceId = "ACTION_ROOT",
                        ParentIds = templateId,
                        Type = "ACTION_ROOT",
                        TemplateType = template.TemplateType,
                        NodeShape = NodeShapeEnum.SquareWithHeader
                    });
                    list.Add(new BusinessDiagramNodeViewModel
                    {
                        Id = "PRE_ACTION_ROOT",
                        Title = "Pre Submit",
                        Description = "Pre Submit",
                        ReferenceId = "PRE_ACTION_ROOT",
                        ParentIds = "ACTION_ROOT",
                        Type = "PRE_ACTION_ROOT",
                        TemplateType = template.TemplateType,
                        NodeShape = NodeShapeEnum.SquareWithHeader
                    });
                    list.Add(new BusinessDiagramNodeViewModel
                    {
                        Id = "POST_ACTION_ROOT",
                        Title = "Post Submit",
                        Description = "Post Submit",
                        ReferenceId = "POST_ACTION_ROOT",
                        ParentIds = "ACTION_ROOT",
                        Type = "POST_ACTION_ROOT",
                        TemplateType = template.TemplateType,
                        NodeShape = NodeShapeEnum.SquareWithHeader
                    });
                }

                if (template.TemplateType == TemplateTypeEnum.Service)
                {
                    list.Add(new BusinessDiagramNodeViewModel
                    {
                        Id = "WF_ROOT",
                        Title = "Workflow",
                        Description = "Workflow",
                        ReferenceId = "WF_ROOT",
                        ParentIds = templateId,
                        Type = "WF_ROOT",
                        TemplateType = template.TemplateType,
                        NodeShape = NodeShapeEnum.SquareWithHeader
                    });
                }
            }
            list.ForEach(x => x.HasChild = list.Any(y => y.ParentIdList != null && y.ParentIdList.Contains(x.Id)));
            return list.DistinctBy(x => x.Id).ToList();
        }

        public async Task<List<IdNameViewModel>> GetComponentsList(string templateId)
        {
            var query = $@"Select c.""Id"", case when c.""ComponentType"" = '4' then stc.""Subject"" when c.""ComponentType"" = '6' then c.""Name"" end as ""Name""
            from public.""Template"" as t  
            join public.""ProcessDesign"" as pd on t.""Id""=pd.""TemplateId"" and pd.""IsDeleted""=false
            join public.""Component"" as c on pd.""Id""=c.""ProcessDesignId"" and c.""IsDeleted""=false
            left join public.""StepTaskComponent"" as stc on  stc.""ComponentId"" = c.""Id"" and stc.""IsDeleted""=false
            where t.""Id""='{templateId}' and(c.""ComponentType""=4 or c.""ComponentType""=6) and t.""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<TemplateViewModel>> GetAllowedTemplateList(string categoryCode, string userId, TemplateTypeEnum? templateType, TaskTypeEnum? taskType, string portalCode = null)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType""
            , tc.""Code"" as CategoryCode,coalesce(nt.""IconFileId"",tt.""IconFileId"",st.""IconFileId"") as ""IconFileId""
            ,coalesce(nt.""TemplateColor"",tt.""TemplateColor"",st.""TemplateColor"",'#87CEEB') as ""TemplateColor""
            ,tt.""TaskTemplateType"" as ""TaskType"",tc.""Name"" as ""TemplateCategoryName""
            from public.""Template"" as t
            join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId""  
            left join public.""NoteTemplate"" as nt on nt.""TemplateId""=t.""Id"" and nt.""IsDeleted"" = false
            left join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted"" = false
            left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted"" = false
            where t.""IsDeleted"" = false and tc.""IsDeleted"" = false ";

            if (templateType != null)
            {
                query = $@"{query} and t.""TemplateType""={(int)templateType} ";
            }
            if (categoryCode.IsNotNullAndNotEmpty())
            {
                categoryCode = categoryCode.Replace(",", "','");
                categoryCode = String.Concat("'", categoryCode, "'");
                query = $@"{query} and tc.""Code"" in (" + categoryCode + ") ";
                //query = $@"{query} and tc.""Code""='{categoryCode}' ";
            }
            if (taskType.IsNotNull())
            {
                query = $@"{query} and tt.""TaskTemplateType""={(int)taskType} ";
            }
            return await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
        }

        public async Task<List<WorkflowViewModel>> GetWorkFlowDiagramDetails(string id)
        {
            string query = @$"select distinct s.""Id"" as Id,s.""ServiceSubject"" as Subject, t.""Code"" as TemplateCode, s.""TemplateId"" as TemplateId,
                                    t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
                                ts1.""Name"" as StepName, s.""ServiceStatusId"" as StatusId, lov.""Name"" as StatusName,
                                u.""Id"" as AssignedToUserId, u.""Name"" as AssignedToUserName,
                                s.""StartDate"" as StartDate,s.""DueDate"" as DueDate, null as ParentId, null as ComponentId, 'Service' as Type
                                from public.""NtsService"" as s  
                                left join public.""LOV"" as lov on lov.""Id"" = s.""ServiceStatusId"" and lov.""IsDeleted"" = false
                                left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted"" = false
                                left join public.""Template"" as t on t.""Id"" = s.""TemplateId"" and t.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
                                where s.""Id"" ='{id}' and s.""IsDeleted"" = false";
            var list = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);

            query = @$"select distinct case when cr.""NtsTaskId"" is not null then cr.""NtsTaskId"" else t.""Id"" end as Id,stc.""Subject"" as Subject,t.""Code"" as TemplateCode, stc.""TaskTemplateId"" as TemplateId,
                                t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
                                ts1.""Name"" as StepName, task.""TaskStatusId"" as StatusId, lov.""Name"" as StatusName,
                                u.""Id"" as AssignedToUserId, u.""Name"" as AssignedToUserName,
                                task.""StartDate"" as StartDate,task.""DueDate"" as DueDate, c.""ParentId"" as ParentId, c.""Id"" as ComponentId, 'Task' as Type
                               , case when ""c"".""ParentId"" <> ""cp"".""ParentId""
                                then CONCAT(""c"".""ParentId"",',' ,""cp"".""ParentId"")
                                else ""c"".""ParentId"" end as ""ParentIds""  , stc.""SequenceOrder"" as SequenceOrder
                                from public.""ComponentResult"" as cr  
                                left join public.""Component"" as c on c.""Id"" = cr.""ComponentId"" and c.""IsDeleted"" = false
                                left join public.""ComponentParent"" as cp on ""c"".""Id""=cp.""ComponentId""                                
                                left join public.""NtsTask"" as task on task.""Id"" = cr.""NtsTaskId"" and task.""IsDeleted"" = false
                                left join public.""LOV"" as lov on lov.""Id"" = task.""TaskStatusId"" and lov.""IsDeleted"" = false
                                left join public.""User"" as u on u.""Id"" = task.""AssignedToUserId"" and u.""IsDeleted"" = false
                                left join public.""StepTaskComponent"" as stc on stc.""ComponentId"" = c.""Id"" and stc.""IsDeleted"" = false
                                left join public.""Template"" as t on t.""Id"" = stc.""TemplateId"" and t.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
                                where  c.""ComponentType"" = 4 and cr.""NtsServiceId"" ='{id}' and cr.""IsDeleted"" = false order by stc.""SequenceOrder""";

            var step = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);

            //if(step.Count() == 0 || step ==  null)
            //{
            //    query = @$"select distinct stc.""Id"" as Id,t.""DisplayName"" as Subject,t.""Code"" as TemplateCode, stc.""TaskTemplateId"" as TemplateId,
            //                    t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
            //                    ts1.""Name"" as StepName, null as StatusId, null as StatusName,
            //                    null as AssignedToUserId, null as AssignedToUserName,
            //                    '' as StartDate,'' as DueDate, c.""ParentId"" as ParentId, c.""Id"" as ComponentId, 'Task' as Type
            //                    ,case when ""c"".""ParentId"" <> ""cp"".""ParentId""
            //                    then CONCAT(""c"".""ParentId"",',' ,""cp"".""ParentId"")
            //                    else ""c"".""ParentId"" end as ""ParentIds""  , stc.""SequenceOrder"" as SequenceOrder
            //                    from public.""ComponentResult"" as cr  
            //                    left join public.""Component"" as c on c.""Id"" = cr.""ComponentId"" and c.""IsDeleted"" = false
            //                    left join public.""ComponentParent"" as cp on ""c"".""Id""=cp.""ComponentId""                                                                
            //                    left join public.""ProcessDesign"" as pd on pd.""Id"" = c.""ProcessDesignId"" and pd.""IsDeleted"" = false
            //                    left join public.""Template"" as st on st.""Id"" = pd.""TemplateId"" and st.""IsDeleted"" = false
            //                    left join public.""NtsTask"" as task on task.""Id"" = cr.""NtsTaskId"" and task.""IsDeleted"" = false
            //                    left join public.""LOV"" as lov on lov.""Id"" = task.""TaskStatusId"" and lov.""IsDeleted"" = false
            //                    left join public.""User"" as u on u.""Id"" = task.""AssignedToUserId"" and u.""IsDeleted"" = false
            //                     join public.""StepTaskComponent"" as stc on stc.""ComponentId"" = c.""Id"" and stc.""IsDeleted"" = false
            //                     join public.""Template"" as t on t.""Id"" = stc.""TemplateId"" and t.""IsDeleted"" = false
            //                    left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
            //                    left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
            //                    where  c.""ComponentType"" = 4 and st.""Id"" ='{id}' and cr.""IsDeleted""= false order by stc.""SequenceOrder""";
            //    step = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);
            //}

            list.AddRange(step);
            return list;
        }

        public async Task<List<WorkflowViewModel>> GetWorkFlowDiagramDetailsByTemplate(string id)
        {
            string query = @$"select distinct t.""Id"" as Id,t.""DisplayName"" as Subject, t.""Code"" as TemplateCode, t.""Id"" as TemplateId,
                                    t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
                                ts1.""Name"" as StepName, null as StatusId, null as StatusName,
                                null as AssignedToUserId, null as AssignedToUserName,
                                '' as StartDate,'' as DueDate, null as ParentId, null as ComponentId, 'Service' as Type
                                from  public.""Template"" as t --on t.""Id"" = s.""TemplateId""
                                left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
                                where t.""Id"" ='{id}' and t.""IsDeleted"" = false";
            var list = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);
            query = @$"select distinct stc.""Id"" as Id,t.""DisplayName"" as Subject,t.""Code"" as TemplateCode, stc.""TaskTemplateId"" as TemplateId,
                                t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
                                ts1.""Name"" as StepName, null as StatusId, null as StatusName,
                                null as AssignedToUserId, null as AssignedToUserName,
                                '' as StartDate,'' as DueDate, cp.""ParentId"" as ParentId, c.""Id"" as ComponentId, 'Task' as Type
                                --,case when ""c"".""ParentId"" <> ""cp"".""ParentId""
                                --then CONCAT(""c"".""ParentId"",',' ,""cp"".""ParentId"")
                                --else ""c"".""ParentId"" end as ""ParentIds""  
, stc.""SequenceOrder"" as SequenceOrder
from public.""Template"" as st
 left join public.""ProcessDesign"" as pd on  st.""Id"" = pd.""TemplateId"" and pd.""IsDeleted"" = false
  left join public.""Component"" as c on pd.""Id"" = c.""ProcessDesignId"" and c.""IsDeleted"" = false
left join public.""ComponentParent"" as cp on c.""Id""=cp.""ComponentId"" and cp.""IsDeleted"" = false                      
 left join public.""ComponentResult"" as cr  on c.""Id""=cr.""ComponentId""   and cr.""IsDeleted"" = false   
                                left join public.""NtsTask"" as task on task.""Id"" = cr.""NtsTaskId"" and task.""IsDeleted"" = false
                                left join public.""LOV"" as lov on lov.""Id"" = task.""TaskStatusId"" and lov.""IsDeleted"" = false
                                left join public.""User"" as u on u.""Id"" = task.""AssignedToUserId"" and u.""IsDeleted"" = false
                                join public.""StepTaskComponent"" as stc on stc.""ComponentId"" = c.""Id"" and stc.""IsDeleted"" = false
                                 join public.""Template"" as t on t.""Id"" = stc.""TemplateId"" and t.""IsDeleted"" = false
                               left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
                                where  c.""ComponentType"" = 4 and st.""Id"" ='{id}' and st.""IsDeleted""= false 
								order by stc.""SequenceOrder""";
            //query = @$"select distinct stc.""Id"" as Id,t.""DisplayName"" as Subject,t.""Code"" as TemplateCode, stc.""TaskTemplateId"" as TemplateId,
            //                    t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
            //                    ts1.""Name"" as StepName, null as StatusId, null as StatusName,
            //                    null as AssignedToUserId, null as AssignedToUserName,
            //                    '' as StartDate,'' as DueDate, c.""ParentId"" as ParentId, c.""Id"" as ComponentId, 'Task' as Type
            //                    ,case when ""c"".""ParentId"" <> ""cp"".""ParentId""
            //                    then CONCAT(""c"".""ParentId"",',' ,""cp"".""ParentId"")
            //                    else ""c"".""ParentId"" end as ""ParentIds""  , stc.""SequenceOrder"" as SequenceOrder
            //                    from public.""ComponentResult"" as cr  
            //                    left join public.""Component"" as c on c.""Id"" = cr.""ComponentId"" and c.""IsDeleted"" = false
            //                    left join public.""ComponentParent"" as cp on ""c"".""Id""=cp.""ComponentId""                                                                
            //                    left join public.""ProcessDesign"" as pd on pd.""Id"" = c.""ProcessDesignId"" and pd.""IsDeleted"" = false
            //                    left join public.""Template"" as st on st.""Id"" = pd.""TemplateId"" and st.""IsDeleted"" = false
            //                    left join public.""NtsTask"" as task on task.""Id"" = cr.""NtsTaskId"" and task.""IsDeleted"" = false
            //                    left join public.""LOV"" as lov on lov.""Id"" = task.""TaskStatusId"" and lov.""IsDeleted"" = false
            //                    left join public.""User"" as u on u.""Id"" = task.""AssignedToUserId"" and u.""IsDeleted"" = false
            //                     join public.""StepTaskComponent"" as stc on stc.""ComponentId"" = c.""Id"" and stc.""IsDeleted"" = false
            //                     join public.""Template"" as t on t.""Id"" = stc.""TemplateId"" and t.""IsDeleted"" = false
            //                    left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
            //                    left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
            //                    where  c.""ComponentType"" = 4 and st.""Id"" ='{id}' and cr.""IsDeleted""= false order by stc.""SequenceOrder""";

            var step = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);
            list.AddRange(step);
            return list/*.DistinctBy(x => x.Id).ToList()*/;
        }
        public async Task<List<TemplateViewModel>> GetTemplateServiceListbyTeam(string tCode, string teamId)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""Code""='{tCode}'
                        join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""DefaultOwnerTeamId""='{teamId}'
                        where t.""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return list;
        }
        public async Task DeleteTemplateData(string templateIds)
        {
            templateIds = templateIds.TrimEnd(',');
            var ids = templateIds.Split(",");
            List<string> queryList = new List<string>();
            foreach (var item in ids)
            {

                var template = await GetSingleById(item);
                if ((template.TemplateType == TemplateTypeEnum.Service) || template.TemplateType != TemplateTypeEnum.Service)
                {
                    queryList.Add(@$"delete from log.""CustomTemplateLog"" where ""TemplateId""='{item}'");
                    // await _queryRepo.ExecuteCommand(@$"delete from log.""CustomTemplateLog"" where ""TemplateId""='{item}' ", null);
                    queryList.Add(@$"delete from log.""FormIndexPageTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""FormTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""NoteIndexPageTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""NoteTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""NotificationTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""NtsNoteLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""NtsServiceLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""NtsTaskLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""PageTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""ProcessDesignLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""ProcessDesignVariableLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""ServiceIndexPageTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""ServiceTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""StepTaskComponentLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""TaskIndexPageTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""TaskTemplateLog"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from log.""UdfPermissionLog"" where ""TemplateId""='{item}' ");

                    queryList.Add(@$"delete from public.""BusinessRule"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from public.""CustomTemplate"" where ""TemplateId""='{item}' ");

                    var formindex = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""FormIndexPageTemplate"" where ""TemplateId""='{item}'", null);

                    foreach (var form in formindex)
                    {
                        queryList.Add(@$"delete from log.""FormIndexPageColumnLog"" where ""FormIndexPageTemplateId""='{form.Id}' ");
                        queryList.Add(@$"delete from public.""FormIndexPageColumn"" where ""FormIndexPageTemplateId""='{form.Id}' ");
                    }
                    queryList.Add(@$"delete from public.""FormTemplate"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from public.""FormIndexPageTemplate"" where ""TemplateId""='{item}' ");







                    queryList.Add(@$"delete from public.""NotificationTemplate"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from public.""NtsGroupTemplate"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from public.""PageTemplate"" where ""TemplateId""='{item}' ");

                    queryList.Add(@$"delete from public.""StepTaskComponent"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from public.""UdfPermission"" where ""TemplateId""='{item}' ");

                    var processDesign = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""ProcessDesign"" where ""TemplateId""='{item}'", null);

                    foreach (var process in processDesign)
                    {

                        queryList.Add(@$"delete from log.""ComponentLog"" where ""ProcessDesignId""='{process.Id}' ");
                        queryList.Add(@$"delete from log.""ComponentResultLog"" where ""ProcessDesignId""='{process.Id}' ");
                        queryList.Add(@$"delete from log.""ProcessDesignComponentLog"" where ""ProcessDesignId""='{process.Id}' ");
                        queryList.Add(@$"delete from log.""ProcessDesignResultLog"" where ""ProcessDesignId""='{process.Id}' ");
                        queryList.Add(@$"delete from log.""ProcessDesignVariableLog"" where ""ProcessDesignId""='{process.Id}' ");

                        queryList.Add(@$"delete from public.""ComponentResult"" where ""ProcessDesignId""='{process.Id}' ");
                        var component = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""Component"" where ""ProcessDesignId""='{process.Id}' ", null);

                        foreach (var comp in component)
                        {
                            queryList.Add(@$"delete from log.""AdhocTaskComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""BusinessExecutionComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""BusinessLogicComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""CompleteEventComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""ComponentParentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""ComponentResultLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""DecisionScriptComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""EmailComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""ExecutionScriptComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""FalseComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""ProcessDesignComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""StartEventComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""StepTaskComponentLog"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from log.""TrueComponentLog"" where ""ComponentId""='{comp.Id}' ");


                            queryList.Add(@$"delete from public.""AdhocTaskComponent"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from public.""ComponentParent"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from public.""ComponentResult"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from public.""DecisionScriptComponent"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from public.""ExecutionScriptComponent"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from public.""FalseComponent"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from public.""StepTaskComponent"" where ""ComponentId""='{comp.Id}' ");
                            queryList.Add(@$"delete from public.""TrueComponent"" where ""ComponentId""='{comp.Id}' ");
                        }
                        queryList.Add(@$"delete from public.""Component"" where ""ProcessDesignId""='{process.Id}' ");
                        var processDesignR = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""ProcessDesignResult"" where ""ProcessDesignId""='{process.Id}' ", null);

                        foreach (var designR in processDesignR)
                        {
                            queryList.Add(@$"delete from log.""ComponentResultLog"" where ""ProcessDesignResultId""='{designR.Id}' ");
                            queryList.Add(@$"delete from public.""ComponentResult"" where ""ProcessDesignResultId""='{designR.Id}' ");
                        }
                        queryList.Add(@$"delete from public.""ProcessDesignResult"" where ""ProcessDesignId""='{process.Id}' ");
                        queryList.Add(@$"delete from public.""ProcessDesignVariable"" where ""ProcessDesignId""='{process.Id}' ");

                    }
                    queryList.Add(@$"delete from public.""ProcessDesignVariable"" where ""TemplateId""='{item}' ");
                    queryList.Add(@$"delete from public.""ProcessDesign"" where ""TemplateId""='{item}' ");



                    var pages = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""Page"" where ""TemplateId""='{item}' ", null);

                    foreach (var page in pages)
                    {
                        queryList.Add(@$"delete from log.""PageDetailsLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""PageIndexLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""PageNoteLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""PermissionLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""UdfPermissionHeaderLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""UserDataPermissionLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""UserPagePreferenceLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""UserPermissionLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""UserRoleDataPermissionLog"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from log.""UserRolePermissionLog"" where ""PageId""='{page.Id}' ");


                        queryList.Add(@$"delete from public.""PageDetails"" where ""PageId""='{page.Id}' ");
                        var pageIndex = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""PageIndex"" where ""PageId""='{page.Id}' ", null);

                        foreach (var index in pageIndex)
                        {
                            queryList.Add(@$"delete from log.""PageIndexColumnLog"" where ""PageIndexId""='{index.Id}' ");
                            queryList.Add(@$"delete from public.""PageIndexColumn"" where ""PageIndexId""='{index.Id}' ");
                        }
                        queryList.Add(@$"delete from public.""PageIndex"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from public.""Permission"" where ""PageId""='{page.Id}' ");
                        var servceIndex = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""UdfPermissionHeader"" where ""PageId""='{page.Id}' ", null);

                        foreach (var index in servceIndex)
                        {
                            queryList.Add(@$"delete from log.""ServiceIndexPageColumnLog"" where ""PageIndexId""='{index.Id}' ");
                            queryList.Add(@$"delete from public.""ServiceIndexPageColumn"" where ""PageIndexId""='{index.Id}' ");
                        }
                        queryList.Add(@$"delete from public.""UdfPermissionHeader"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from public.""UserDataPermission"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from public.""UserPagePreference"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from public.""UserPermission"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from public.""UserRoleDataPermission"" where ""PageId""='{page.Id}' ");
                        queryList.Add(@$"delete from public.""UserRolePermission"" where ""PageId""='{page.Id}' ");
                    }

                    queryList.Add(@$"delete from public.""Page"" where ""TemplateId""='{item}' ");

                    if (template.TemplateType == TemplateTypeEnum.Note)
                    {
                        var noteindex = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NoteIndexPageTemplate"" where ""TemplateId""='{item}'", null);

                        foreach (var note in noteindex)
                        {

                            queryList.Add(@$"delete from log.""NoteIndexPageColumnLog"" where ""NoteIndexPageTemplateId""='{note.Id}' ");
                            queryList.Add(@$"delete from log.""NoteTemplateLog"" where ""NoteIndexPageTemplateId""='{note.Id}' ");
                            queryList.Add(@$"delete from public.""NoteIndexPageColumn"" where ""NoteIndexPageTemplateId""='{note.Id}' ");

                        }
                        var notetemp = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NoteTemplate"" where ""TemplateId""='{item}'", null);

                        foreach (var nt in notetemp)
                        {
                            queryList.Add(@$"delete from log.""NtsNoteLog"" where ""NoteTemplateId""='{nt.Id}' ");
                            var notes = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsNote"" where ""NoteTemplateId""='{nt.Id}'", null);
                            foreach (var n in notes)
                            {
                                queryList.Add(@$"delete from log.""DocumentPermissionLog"" where ""NoteId""='{n.Id}' ");
                                queryList.Add(@$"delete from log.""NtsNotePrecedenceLog"" where ""NoteId""='{n.Id}' ");
                                queryList.Add(@$"delete from public.""DocumentPermission"" where ""NoteId""='{n.Id}' ");
                                queryList.Add(@$"delete from public.""NtsNotePrecedence"" where ""NoteId""='{n.Id}' ");

                                queryList.Add(@$"delete from log.""NtsNoteCommentLog"" where ""NtsNoteId""='{n.Id}' ");
                                queryList.Add(@$"delete from log.""NtsNoteSharedLog"" where ""NtsNoteId""='{n.Id}' ");
                                queryList.Add(@$"delete from log.""NtsNoteStatusTrackLog"" where ""NtsNoteId""='{n.Id}' ");

                                var notecomment = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsNoteComment"" where ""NtsNoteId""='{n.Id}'", null);
                                foreach (var nc in notecomment)
                                {
                                    queryList.Add(@$"delete from public.""NtsNoteCommentUser"" where ""NtsNoteCommentId""='{nc.Id}' ");
                                }
                                queryList.Add(@$"delete from public.""NtsNoteComment"" where ""NtsNoteId""='{n.Id}' ");
                                queryList.Add(@$"delete from public.""NtsNoteShared"" where ""NtsNoteId""='{n.Id}' ");
                                queryList.Add(@$"delete from public.""NtsNoteStatusTrack"" where ""NtsNoteId""='{n.Id}' ");

                            }
                            var table = await _tableBusiness.GetSingleById(template.TableMetadataId);
                            if (table != null)
                            {
                                try
                                {
                                    await _queryRepo.ExecuteCommand(@$"drop table log.""{table.Name}log"" ", null);
                                }
                                catch (Exception ex)
                                {
                                }
                                try
                                {
                                    await _queryRepo.ExecuteCommand(@$"drop table cms.""{table.Name}"" ", null);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            queryList.Add(@$"delete from public.""NtsNote"" where ""NoteTemplateId""='{nt.Id}'");

                        }

                        queryList.Add(@$"delete from public.""NoteTemplate"" where ""TemplateId""='{item}'");
                        queryList.Add(@$"delete from public.""NoteIndexPageTemplate"" where ""TemplateId""='{item}' ");
                        queryList.Add(@$"delete from log.""TemplateLog"" where ""RecordId""='{item}' ");
                        queryList.Add(@$"delete from public.""Template"" where ""Id""='{item}' ");

                    }
                    else if (template.TemplateType == TemplateTypeEnum.Task)
                    {
                        var taskindex = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""TaskIndexPageTemplate"" where ""TemplateId""='{item}'", null);

                        foreach (var task in taskindex)
                        {

                            queryList.Add(@$"delete from log.""TaskIndexPageColumnLog"" where ""TaskIndexPageTemplateId""='{task.Id}' ");
                            queryList.Add(@$"delete from log.""TaskTemplateLog"" where ""TaskIndexPageTemplateId""='{task.Id}' ");
                            queryList.Add(@$"delete from public.""TaskIndexPageColumn"" where ""TaskIndexPageTemplateId""='{task.Id}' ");
                        }
                        var tasktemp = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""TaskTemplate"" where ""TemplateId""='{item}'", null);

                        foreach (var tt in tasktemp)
                        {
                            queryList.Add(@$"delete from log.""NtsTaskLog"" where ""TaskTemplateId""='{tt.Id}' ");
                            queryList.Add(@$"delete from log.""StepTaskComponentLog"" where ""TaskTemplateId""='{tt.Id}' ");
                            queryList.Add(@$"delete from public.""StepTaskComponent"" where ""TaskTemplateId""='{tt.Id}' ");

                            var tasks = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsTask"" where ""TaskTemplateId""='{tt.Id}'", null);
                            foreach (var t in tasks)
                            {
                                queryList.Add(@$"delete from log.""AdhocTaskComponentLog"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""ComponentResultLog"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""NtsTaskAttachmentLog"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""NtsTaskCommentLog"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""NtsTaskPrecedenceLog"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""NtsTaskStatusTrackLog"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""NtsTaskTimeEntryLog"" where ""NtsTaskId""='{t.Id}' ");

                                queryList.Add(@$"delete from public.""AdhocTaskComponent"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from public.""ComponentResult"" where ""NtsTaskId""='{t.Id}' ");

                                var notecomment = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsTaskComment"" where ""NtsTaskId""='{t.Id}'", null);
                                foreach (var nc in notecomment)
                                {
                                    queryList.Add(@$"delete from public.""NtsTaskCommentUser"" where ""NtsTaskCommentId""='{nc.Id}' ");
                                }
                                queryList.Add(@$"delete from public.""NtsTaskComment"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from public.""NtsTaskPrecedence"" where ""NtsTaskId""='{t.Id}' ");
                                queryList.Add(@$"delete from public.""NtsTaskTimeEntry"" where ""NtsTaskId""='{t.Id}' ");

                            }
                            queryList.Add(@$"delete from public.""NtsTask"" where ""TaskTemplateId""='{tt.Id}'");
                            queryList.Add(@$"delete from public.""TaskTemplate"" where ""TemplateId""='{item}' ");
                            queryList.Add(@$"delete from public.""TaskIndexPageTemplate"" where ""TemplateId""='{item}' ");
                            queryList.Add(@$"delete from log.""TemplateLog"" where ""RecordId""='{item}' ");
                            queryList.Add(@$"delete from public.""Template"" where ""UdfTemplateId""='{item}' ");
                            queryList.Add(@$"delete from public.""Template"" where ""Id""='{item}' ");

                            var table = await _tableBusiness.GetSingleById(template.UdfTableMetadataId);
                            if (table == null)
                            {
                                table = await _tableBusiness.GetSingleById(template.TableMetadataId);
                            }
                            if (table != null)
                            {
                                try
                                {
                                    await _queryRepo.ExecuteCommand(@$"drop table log.""{table.Name}log"" ", null);
                                }
                                catch (Exception ex)
                                {
                                }
                                try
                                {
                                    await _queryRepo.ExecuteCommand(@$"drop table cms.""{table.Name}"" ", null);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            if (template.UdfTemplateId.IsNotNullAndNotEmpty())
                            {
                                var noteindex = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NoteIndexPageTemplate"" where ""TemplateId""='{template.UdfTemplateId}'", null);

                                foreach (var note in noteindex)
                                {

                                    queryList.Add(@$"delete from log.""NoteIndexPageColumnLog"" where ""NoteIndexPageTemplateId""='{note.Id}' ");
                                    queryList.Add(@$"delete from log.""NoteTemplateLog"" where ""NoteIndexPageTemplateId""='{note.Id}' ");
                                    queryList.Add(@$"delete from public.""NoteIndexPageColumn"" where ""NoteIndexPageTemplateId""='{note.Id}' ");

                                }
                                var notetemp = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NoteTemplate"" where ""TemplateId""='{template.UdfTemplateId}'", null);

                                foreach (var nt in notetemp)
                                {
                                    queryList.Add(@$"delete from log.""NtsNoteLog"" where ""NoteTemplateId""='{nt.Id}' ");
                                    var notes = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsNote"" where ""NoteTemplateId""='{nt.Id}'", null);
                                    foreach (var n in notes)
                                    {
                                        queryList.Add(@$"delete from log.""DocumentPermissionLog"" where ""NoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from log.""NtsNotePrecedenceLog"" where ""NoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from public.""DocumentPermission"" where ""NoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from public.""NtsNotePrecedence"" where ""NoteId""='{n.Id}' ");

                                        queryList.Add(@$"delete from log.""NtsNoteCommentLog"" where ""NtsNoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from log.""NtsNoteSharedLog"" where ""NtsNoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from log.""NtsNoteStatusTrackLog"" where ""NtsNoteId""='{n.Id}' ");

                                        var notecomment = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsNoteComment"" where ""NtsNoteId""='{n.Id}'", null);
                                        foreach (var nc in notecomment)
                                        {
                                            queryList.Add(@$"delete from public.""NtsNoteCommentUser"" where ""NtsNoteCommentId""='{nc.Id}' ");
                                        }
                                        queryList.Add(@$"delete from public.""NtsNoteComment"" where ""NtsNoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from public.""NtsNoteShared"" where ""NtsNoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from public.""NtsNoteStatusTrack"" where ""NtsNoteId""='{n.Id}' ");

                                    }

                                    queryList.Add(@$"delete from public.""NtsNote"" where ""NoteTemplateId""='{nt.Id}'");

                                }
                                queryList.Add(@$"delete from log.""NoteTemplateLog"" where ""TemplateId""='{template.UdfTemplateId}'");
                                queryList.Add(@$"delete from public.""NoteTemplate"" where ""TemplateId""='{template.UdfTemplateId}'");
                                queryList.Add(@$"delete from log.""NoteIndexPageTemplateLog"" where ""TemplateId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from public.""NoteIndexPageTemplate"" where ""TemplateId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from log.""NotificationTemplateLog"" where ""TemplateId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from public.""NotificationTemplate"" where ""TemplateId""='{template.UdfTemplateId}' ");

                                queryList.Add(@$"delete from log.""TemplateLog"" where ""RecordId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from log.""TemplateLog"" where ""UdfTemplateId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from public.""Template"" where ""Id""='{template.UdfTemplateId}' ");

                            }


                        }




                    }
                    else if (template.TemplateType == TemplateTypeEnum.Service)
                    {
                        var serviceindex = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""ServiceIndexPageTemplate"" where ""TemplateId""='{item}'", null);

                        foreach (var service in serviceindex)
                        {

                            queryList.Add(@$"delete from log.""ServiceIndexPageColumnLog"" where ""ServiceIndexPageTemplateId""='{service.Id}' ");
                            queryList.Add(@$"delete from log.""ServiceTemplateLog"" where ""ServiceIndexPageTemplateId""='{service.Id}' ");
                            queryList.Add(@$"delete from public.""ServiceIndexPageColumn"" where ""ServiceIndexPageTemplateId""='{service.Id}' ");
                        }
                        var servicetemp = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""ServiceTemplate"" where ""TemplateId""='{item}'", null);

                        foreach (var st in servicetemp)
                        {
                            queryList.Add(@$"delete from log.""NtsServiceLog"" where ""ServiceTemplateId""='{st.Id}' ");

                            var tasks = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsService"" where ""ServiceTemplateId""='{st.Id}'", null);
                            foreach (var t in tasks)
                            {
                                queryList.Add(@$"delete from log.""ComponentResultLog"" where ""NtsServiceId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""NtsServiceCommentLog"" where ""NtsServiceId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""NtsServiceSharedLog"" where ""NtsServiceId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""NtsServiceStatusTrackLog"" where ""NtsServiceId""='{t.Id}' ");
                                queryList.Add(@$"delete from log.""ProcessDesignResultLog"" where ""NtsServiceId""='{t.Id}' ");

                                queryList.Add(@$"delete from public.""ComponentResult"" where ""NtsTaskId""='{t.Id}' ");

                                var notecomment = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsServiceComment"" where ""NtsServiceId""='{t.Id}'", null);
                                foreach (var nc in notecomment)
                                {
                                    queryList.Add(@$"delete from public.""NtsServiceCommentUser"" where ""NtsServiceCommentId""='{nc.Id}' ");
                                }
                                queryList.Add(@$"delete from public.""NtsServiceComment"" where ""NtsServiceId""='{t.Id}' ");
                                queryList.Add(@$"delete from public.""NtsServiceShared"" where ""NtsServiceId""='{t.Id}' ");
                                queryList.Add(@$"delete from public.""ProcessDesignResult"" where ""NtsServiceId""='{t.Id}' ");

                            }
                            queryList.Add(@$"delete from public.""NtsService"" where ""ServiceTemplateId""='{st.Id}'");
                            queryList.Add(@$"delete from public.""ServiceTemplate"" where ""TemplateId""='{item}' ");
                            queryList.Add(@$"delete from public.""ServiceIndexPageTemplate"" where ""TemplateId""='{item}' ");
                            queryList.Add(@$"delete from log.""TemplateLog"" where ""RecordId""='{item}' ");
                            queryList.Add(@$"delete from public.""Template"" where ""UdfTemplateId""='{item}' ");
                            queryList.Add(@$"delete from public.""Template"" where ""Id""='{item}' ");

                            var table = await _tableBusiness.GetSingleById(template.UdfTableMetadataId);
                            if (table != null)
                            {
                                try
                                {
                                    await _queryRepo.ExecuteCommand(@$"drop table log.""{table.Name}log"" ", null);
                                }
                                catch (Exception ex)
                                {
                                }
                                try
                                {
                                    await _queryRepo.ExecuteCommand(@$"drop table cms.""{table.Name}"" ", null);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            if (template.UdfTemplateId.IsNotNullAndNotEmpty())
                            {
                                var noteindex = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NoteIndexPageTemplate"" where ""TemplateId""='{template.UdfTemplateId}'", null);

                                foreach (var note in noteindex)
                                {

                                    queryList.Add(@$"delete from log.""NoteIndexPageColumnLog"" where ""NoteIndexPageTemplateId""='{note.Id}' ");
                                    queryList.Add(@$"delete from log.""NoteTemplateLog"" where ""NoteIndexPageTemplateId""='{note.Id}' ");
                                    queryList.Add(@$"delete from public.""NoteIndexPageColumn"" where ""NoteIndexPageTemplateId""='{note.Id}' ");

                                }
                                var notetemp = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NoteTemplate"" where ""TemplateId""='{template.UdfTemplateId}'", null);

                                foreach (var nt in notetemp)
                                {
                                    queryList.Add(@$"delete from log.""NtsNoteLog"" where ""NoteTemplateId""='{nt.Id}' ");
                                    var notes = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsNote"" where ""NoteTemplateId""='{nt.Id}'", null);
                                    foreach (var n in notes)
                                    {
                                        queryList.Add(@$"delete from log.""DocumentPermissionLog"" where ""NoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from log.""NtsNotePrecedenceLog"" where ""NoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from public.""DocumentPermission"" where ""NoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from public.""NtsNotePrecedence"" where ""NoteId""='{n.Id}' ");

                                        queryList.Add(@$"delete from log.""NtsNoteCommentLog"" where ""NtsNoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from log.""NtsNoteSharedLog"" where ""NtsNoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from log.""NtsNoteStatusTrackLog"" where ""NtsNoteId""='{n.Id}' ");

                                        var notecomment = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""NtsNoteComment"" where ""NtsNoteId""='{n.Id}'", null);
                                        foreach (var nc in notecomment)
                                        {
                                            queryList.Add(@$"delete from public.""NtsNoteCommentUser"" where ""NtsNoteCommentId""='{nc.Id}' ");
                                        }
                                        queryList.Add(@$"delete from public.""NtsNoteComment"" where ""NtsNoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from public.""NtsNoteShared"" where ""NtsNoteId""='{n.Id}' ");
                                        queryList.Add(@$"delete from public.""NtsNoteStatusTrack"" where ""NtsNoteId""='{n.Id}' ");

                                    }

                                    queryList.Add(@$"delete from public.""NtsNote"" where ""NoteTemplateId""='{nt.Id}'");

                                }
                                queryList.Add(@$"delete from log.""NoteTemplateLog"" where ""TemplateId""='{template.UdfTemplateId}'");
                                queryList.Add(@$"delete from public.""NoteTemplate"" where ""TemplateId""='{template.UdfTemplateId}'");
                                queryList.Add(@$"delete from log.""NoteIndexPageTemplateLog"" where ""TemplateId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from public.""NoteIndexPageTemplate"" where ""TemplateId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from log.""NotificationTemplateLog"" where ""TemplateId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from public.""NotificationTemplate"" where ""TemplateId""='{template.UdfTemplateId}' ");

                                queryList.Add(@$"delete from log.""TemplateLog"" where ""RecordId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from log.""TemplateLog"" where ""UdfTemplateId""='{template.UdfTemplateId}' ");
                                queryList.Add(@$"delete from public.""Template"" where ""Id""='{template.UdfTemplateId}' ");


                            }
                        }



                        //var table = await _tableBusiness.GetSingleById(template.TableMetadataId);
                        //if(table!=null)
                        //{
                        //    try
                        //    {
                        //        await _queryRepo.ExecuteCommand(@$"drop table log.""{table.Name}log"" ", null);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //    }
                        //    await _queryRepo.ExecuteCommand(@$"drop table cms.""{table.Name}"" ", null);
                        //}
                        //await _queryRepo.ExecuteCommand(@$"delete from public.""NtsNote"" where ""TemplateId""='{item}' ", null);
                        //await _queryRepo.ExecuteCommand(@$"delete from public.""NtsService"" where ""TemplateId""='{item}' ", null);
                        //await _queryRepo.ExecuteCommand(@$"delete from public.""NtsTask"" where ""TemplateId""='{item}' ", null);
                    }
                    else
                    {
                        queryList.Add(@$"delete from log.""TemplateLog"" where ""RecordId""='{item}' ");
                        queryList.Add(@$"delete from public.""Template"" where ""Id""='{item}' ");
                    }
                    if (template.TemplateType == TemplateTypeEnum.Note || template.TemplateType == TemplateTypeEnum.Task || template.TemplateType == TemplateTypeEnum.Service)
                    {
                        if (template.TableMetadataId != null || template.UdfTableMetadataId != null)
                        {
                            var columnMeta = await _queryRepo.ExecuteQueryList<IdNameViewModel>(@$"select ""Id"" from public.""ColumnMetadata"" where ""TableMetadataId""='{template.TableMetadataId}'", null);
                            foreach (var column in columnMeta)
                            {
                                queryList.Add(@$"delete from log.""FormIndexPageColumnLog"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from log.""NoteIndexPageColumnLog"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from log.""PageIndexColumnLog"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from log.""ServiceIndexPageColumnLog"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from log.""TaskIndexPageColumnLog"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from log.""UserDataPermissionLog"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from log.""UserRoleDataPermissionLog"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from public.""FormIndexPageColumn"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from public.""NoteIndexPageColumn"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from public.""PageIndexColumn"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from public.""ServiceIndexPageColumn"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from public.""TaskIndexPageColumn"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from public.""UserDataPermission"" where ""ColumnMetadataId""='{column.Id}' ");
                                queryList.Add(@$"delete from public.""UserRoleDataPermission"" where ""ColumnMetadataId""='{column.Id}' ");
                            }
                            if (template.TemplateType == TemplateTypeEnum.Note)
                            {
                                queryList.Add(@$"delete from log.""ColumnMetadataLog"" where ""TableMetadataId""='{template.TableMetadataId}' ");
                                queryList.Add(@$"delete from public.""ColumnMetadata"" where ""TableMetadataId""='{template.TableMetadataId}' ");
                                queryList.Add(@$"delete from log.""TableMetadataLog"" where ""RecordId""='{template.TableMetadataId}' ");
                                queryList.Add(@$"delete from public.""TableMetadata"" where ""Id""='{template.TableMetadataId}' ");
                            }
                            else if (template.TemplateType == TemplateTypeEnum.Service)
                            {
                                queryList.Add(@$"delete from log.""ColumnMetadataLog"" where ""TableMetadataId""='{template.UdfTableMetadataId}' ");
                                queryList.Add(@$"delete from public.""ColumnMetadata"" where ""TableMetadataId""='{template.UdfTableMetadataId}' ");
                                queryList.Add(@$"delete from log.""TableMetadataLog"" where ""RecordId""='{template.UdfTableMetadataId}' ");
                                queryList.Add(@$"delete from public.""TableMetadata"" where ""Id""='{template.UdfTableMetadataId}' ");
                            }
                        }
                    }
                }
            }
            if (queryList.Count > 0)
            {
                var query = String.Join(";", queryList);
                await _queryRepo.ExecuteCommand(query, null);
            }
        }
        public async Task<List<TemplateViewModel>> GetTemplateDeleteList()
        {
            var query = @$"select t.*
                        from public.""Template"" as t
                        where t.""Id"" not in (select ""UdfTemplateId"" from public.""Template"" where ""UdfTemplateId"" is not null ) ";

            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return list;
        }
        public async Task<bool> SetOcrTemplateFileId(string templateId,string fileId)
        {
            var query = @$"update public.""NoteTemplate"" 
                        set ""OcrTemplateFileId"" = '{fileId}'
                        where ""TemplateId"" = '{templateId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
    }
}
