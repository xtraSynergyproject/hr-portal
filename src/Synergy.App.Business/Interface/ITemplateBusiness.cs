using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ITemplateBusiness : IBusinessBase<TemplateViewModel, Template>
    {
        //Task<CommandResult<TableMetadataViewModel>> CreateTemplateTable(TemplateViewModel model);
        // Task<CommandResult<TableMetadataViewModel>> EditTemplateTable(TemplateViewModel model);
        Task<List<TemplateViewModel>> GetTemplateByType(TemplateTypeEnum type, string portalId);
        Task<ExportTemplateViewModel> ExportTemplate(ExportTemplateViewModel model);
        Task<CommandResult<ExportTemplateViewModel>> ImportTemplate(ExportTemplateViewModel model);
        Task<CommandResult<ExportTemplateViewModel>> OverwriteTemplate(ExportTemplateViewModel model);
        Task<string> GenerateJsonDynamicallyFromTableMetaData(TemplateViewModel model);
        Task<List<TemplateViewModel>> GetTemplateList(string tCode, string tcCode,string mCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames=null);
        Task<List<TemplateViewModel>> GetAllowedTemplateList(string categoryCode, string userId, TemplateTypeEnum? templateType,TaskTypeEnum? taskType, string portalCode);
        Task<List<TemplateViewModel>> GetAdhocTemplateList(string tCode, string tcCode, string moduleId);
        Task<List<TemplateViewModel>> GetNoteTemplateList(string tCode, string tcCode,string moduleCodes, string templateIds=null, string categoryIds = null, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames = null);
        Task DeleteTemplate(string TemplateId);
        Task<List<TemplateViewModel>> GetTemplateServiceList( string tCode, string tcCode,string moduleCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks=false, string portalNames = null,ServiceTypeEnum? serviceType=null, string groupCodes = null);
        Task<List<TemplateViewModel>> GetTemplateListByTaskTemplate(string taskTemplateId);
        Task<List<BusinessDiagramNodeViewModel>> GetTemplateBusinessDiagram(string templateId);
        Task<List<WorkflowViewModel>> GetWorkFlowDiagramDetails(string id);
        Task<List<WorkflowViewModel>> GetWorkFlowDiagramDetailsByTemplate(string id);
        Task<List<TemplateViewModel>> GetTemplateServiceListbyTeam(string tCode, string teamId);
        Task<List<IdNameViewModel>> GetComponentsList(string templateId);
        Task DeleteTemplateData(string templateIds);
        Task<List<TemplateViewModel>> GetTemplateDeleteList();
        Task<bool> SetOcrTemplateFileId(string templateId, string fileId);
        Task<TemplateViewModel> CheckTemplate(TemplateViewModel model);
        Task<List<TemplateViewModel>> GetMasterList(string groupCode);
        Task<List<TemplateViewModel>> GetTemplatesList(string portalId, TemplateTypeEnum templateType);
        Task<string> GetForeignKeyId(ColumnMetadataViewModel col, string val);
    }
}
