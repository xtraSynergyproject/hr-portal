using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class AssessmentQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IAssessmentQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        public AssessmentQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
        }

        public async Task<List<UserAssessmentResultViewModel>> GetAssessmentResultListData()
        {
            var query = $@"select ur.""Id"" as Id, ur.""Email"" as Email,
                            ur.""TotalScore"" as TotalScore, ur.""ResultOfTechnicalAssessment"" as TechnicalAssessment10Percent,
                            ur.""TechnicalScore"" as TechnicalAssessmentScore, ur.""ResultOfCaseStudy"" as CaseStudy10Percent, 
                            ur.""CaseStudyScore"" as CaseStudyScore, ur.""ResultOfTechnicalInterview"" as TechnicalInterview15Percent,
                            ur.""InterviewScore"" as TechnicalInterviewScore, ur.""ILM1"" as ILM1, ur.""ILM2"" as ILM2, ur.""ILM3"" as ILM3, 
                            ur.""ILM4"" as ILM4, ur.""ILM5"" as ILM5, ur.""ILM6"" as ILM6, ur.""ILM7"" as ILM7, ur.""ILM8"" as ILM8, ur.""ILM9"" as ILM9,
                            ur.""Control"" as Control, ur.""Commitment"" as Commitment, ur.""TheChallenge"" as TheChallenge, ur.""Altafah"" as Altafah,
                            ur.""LeadershipScore"" as ILMScore, ur.""MentalAbilityScore"" as MTQScore from public.""TASUserReport"" as ur order by ur.""Email""";

            var res = await _queryRepo.ExecuteQueryList<UserAssessmentResultViewModel>(query, null);
            return res;
        }

        public async Task<TASUserReportViewModel> GetUserReportData(string userId)
        {

            var cypher = $@"select * from public.""TASUserReport"" as ur where ur.""UserId""='{userId}' ";

            var report = await _queryRepo.ExecuteQuerySingle<TASUserReportViewModel>(cypher, null);
            return report;
        }
    }
}
