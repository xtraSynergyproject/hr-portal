using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IComponentResultBusiness : IBusinessBase<ComponentResultViewModel, ComponentResult>
    {
        Task InitiateAllComponentsByProcessDesign(string processDesignId);
        Task ProcessAllOpenComponents();
        Task ExecuteComponent(string componentResultId);
        Task ExecuteDynamicStepTaskComponent(string stepcomponentId,string serviceId);
        Task ManageStepTaskComponent(TaskTemplateViewModel task);
        Task<List<ComponentResultViewModel>> GetComponentResultList(string ServiceId);
        Task<List<TaskViewModel>> GetStepTaskList(string serviceId);
        Task<List<AssignmentViewModel>> GetUserListOnNtsBasis(string id, NtsTypeEnum type);
        Task<List<TemplateViewModel>> GetStepTaskTemplateList(string serviceTemplateId);
    }
}
