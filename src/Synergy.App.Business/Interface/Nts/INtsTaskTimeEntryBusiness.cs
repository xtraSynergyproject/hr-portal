using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface INtsTaskTimeEntryBusiness : IBusinessBase<TaskTimeEntryViewModel,NtsTaskTimeEntry>
    {
        Task<List<TaskTimeEntryViewModel>> GetSearchResult(string taskId);
        Task<List<TaskTimeEntryViewModel>> GetTimeEntriesData(string serviceId, DateTime timelog, string userId= null);
    }
}
