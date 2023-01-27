using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IStepTaskEscalationDataBusiness : IBusinessBase<StepTaskEscalationDataViewModel, StepTaskEscalationData>
    {
        Task<List<StepTaskEscalationDataViewModel>> GetPortalTaskListWithEscalationData(string portal, string escalatUser);
        Task<List<StepTaskEscalationDataViewModel>> GetMyTasksEscalatedDataList(string portal, string assigneeUser);
        Task<List<StepTaskEscalationDataViewModel>> AllEscalatedTasks(string portalids);
        Task<MemoryStream> GetExcelForTemplateData(string portalids);
    }
}
