using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IAssessmentQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<UserAssessmentResultViewModel>> GetAssessmentResultListData();
        Task<TASUserReportViewModel> GetUserReportData(string userId);
    }
}
