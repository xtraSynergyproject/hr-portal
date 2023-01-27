using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ITicketAssessmentBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        public Task<List<HelpDeskViewModel>> GetRequestCounts(string type);
        public Task<List<HelpDeskViewModel>> GetChartData(string type);
        public Task<List<HelpDeskViewModel>> GetPendingTaskCountWithAssignee();
        Task<List<HelpDeskViewModel>> GetServiceSLAVoilationCompletedIn20Days();
        Task<List<HelpDeskViewModel>> GetServiceSLAVoilationInLast20Days();
        Task<List<HelpDeskViewModel>> GetRequestForServiceChart(string type);
        Task<List<HelpDeskViewModel>> GetOverDueRequest();
        Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolation(string type);
    }
}
