using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface INtsTaskTimeEntryBusiness : IBusinessBase<TaskTimeEntryViewModel,NtsTaskTimeEntry>
    {
        Task<List<TaskTimeEntryViewModel>> GetSearchResult(string taskId);
        Task<List<TaskTimeEntryViewModel>> GetTimeEntriesData(string serviceId, DateTime timelog, string userId= null);
    }
}
