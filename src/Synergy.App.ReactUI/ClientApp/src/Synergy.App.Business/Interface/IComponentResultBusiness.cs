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
    public interface IComponentResultBusiness : IBusinessBase<ComponentResultViewModel, ComponentResult>
    {
        Task InitiateAllComponentsByProcessDesign(string processDesignId);
        Task ProcessAllOpenComponents();
        Task ExecuteComponent(string componentResultId);
        Task ExecuteDynamicStepTaskComponent(string stepcomponentId, string serviceId);
        Task ManageStepTaskComponent(TaskTemplateViewModel task);
        Task EscalateTask();
        Task<List<ComponentResultViewModel>> GetComponentResultList(string ServiceId);
        Task<List<TaskViewModel>> GetStepTaskList(string serviceId);
        Task<List<AssignmentViewModel>> GetUserListOnNtsBasis(string id, NtsTypeEnum type);
        Task<List<TemplateViewModel>> GetStepTaskTemplateList(string serviceTemplateId);
        Task ExecuteStepTaskComponent(ComponentResultViewModel componentResult);
        Task<List<NtsEmailViewModel>> GetNtsEmailTree(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, string departmentId = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<List<NtsEmailViewModel>> GetNtsEmailList(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, string categoryId = null, string templateId = null, EmailTypeEnum? emailType = null, EmailInboxTypeEnum? inboxStatus = null, string departmentId = null, DateTime? fromDate = null, DateTime? toDate = null, NtsEmailTargetTypeEnum? targetType1 = null, NtsEmailTargetTypeEnum? targetType2 = null, string wfStatus = null, SLATypeEnum? slaType = null);
        Task<List<NtsEmailViewModel>> GetNtsEmailDetails(string serviceId, string targetId, NtsEmailTargetTypeEnum? targetType);
    }
}
