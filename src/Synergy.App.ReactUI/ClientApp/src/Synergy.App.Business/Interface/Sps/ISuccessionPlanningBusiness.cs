using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ISuccessionPlanningBusiness: IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<SuccessionPlaningViewModel>> GetSuccessionPlanings(string Module, string Employee, string Department, int? Month, int? year);

        Task<List<SuccessionPlaningViewModel>> GetAssessmentListofUser(int Month, int Year);
        Task<List<SuccessionPlaningViewModel>> GetAssessmenSettListofUser(int Month, int Year);
        Task<SuccessionPlanningAssessmentViewModel> GetAssessmenSetByDateuserid(string UserId, DateTime date);
        Task<SuccessionPlanningAssessmentViewModel> GetAssessmentByDateuserid(string UserId, DateTime date);

        Task<List<CompetencyFeedbackUserViewModel>> GetCompetencyTopName(string Subordinateid);

        Task<List<CompetencyFeedbackUserViewModel>> GetTopFeedbackUser(string Subordinateid);

        Task<List<CompetencyFeedbackUserViewModel>> GetAreDevelopmentCompetencyTopName(string Subordinateid);
        Task<List<CompetencyFeedbackUserViewModel>> GetChartList(string Subordinateid);



    }
}
