using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ITicketManagementQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<HelpDeskViewModel>> GetRequestCountsByCategory();
        Task<List<HelpDeskViewModel>> GetRequestCountsByService();
        Task<List<HelpDeskViewModel>> GetRequestCountsByPriority();
        Task<List<HelpDeskViewModel>> GetRequestCountsByOwner();
        Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolationByCategory();
        Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolationByService();
        Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolationByPriority();
        Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolationByOwner();
        Task<List<HelpDeskViewModel>> GetChartDataThisWeek();
        Task<List<HelpDeskViewModel>> GetChartDataLastWeek();
        Task<List<HelpDeskViewModel>> GetChartDataThisMonth();
        Task<List<HelpDeskViewModel>> GetChartDataLastMonth();
        Task<List<HelpDeskViewModel>> GetPendingTaskCountWithAssignee();
        Task<List<HelpDeskViewModel>> GetServiceSLAVoilationInLast20Days();
        Task<List<HelpDeskViewModel>> GetServiceSLAVoilationCompletedIn20Days();
        Task<List<HelpDeskViewModel>> GetRequestForServiceChart(string type);
        Task<List<HelpDeskViewModel>> GetOverDueRequest();

    }
}
