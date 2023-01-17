using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class UserReportBusiness : BusinessBase<TASUserReportViewModel, TASUserReport>, IUserReportBusiness
    {
        private readonly IRepositoryQueryBase<UserAssessmentResultViewModel> _queryRepo;
        public UserReportBusiness(IRepositoryBase<TASUserReportViewModel, TASUserReport> repo, IMapper autoMapper
            , IRepositoryQueryBase<UserAssessmentResultViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<TASUserReportViewModel>> Create(TASUserReportViewModel model)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TASUserReportViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);

            return CommandResult<TASUserReportViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TASUserReportViewModel>> Edit(TASUserReportViewModel model)
        {
            //var data = _autoMapper.Map<ListOfValueViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TASUserReportViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<TASUserReportViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<TASUserReportViewModel>> IsNameExists(TASUserReportViewModel viewModel)
        {
            var list = await GetList();
           
            return CommandResult<TASUserReportViewModel>.Instance();
        }

        public async Task<List<UserAssessmentResultViewModel>> GetAssessmentResultList()
        {
            var query = $@"select ur.""Id"" as Id, ur.""Email"" as Email,
                            ur.""TotalScore"" as TotalScore, ur.""ResultOfTechnicalAssessment"" as TechnicalAssessment10Percent,
                            ur.""TechnicalScore"" as TechnicalAssessmentScore, ur.""ResultOfCaseStudy"" as CaseStudy10Percent, 
                            ur.""CaseStudyScore"" as CaseStudyScore, ur.""ResultOfTechnicalInterview"" as TechnicalInterview15Percent,
                            ur.""InterviewScore"" as TechnicalInterviewScore, ur.""ILM1"" as ILM1, ur.""ILM2"" as ILM2, ur.""ILM3"" as ILM3, 
                            ur.""ILM4"" as ILM4, ur.""ILM5"" as ILM5, ur.""ILM6"" as ILM6, ur.""ILM7"" as ILM7, ur.""ILM8"" as ILM8, ur.""ILM9"" as ILM9,
                            ur.""Control"" as Control, ur.""Commitment"" as Commitment, ur.""TheChallenge"" as TheChallenge, ur.""Altafah"" as Altafah,
                            ur.""LeadershipScore"" as ILMScore, ur.""MentalAbilityScore"" as MTQScore from public.""TASUserReport"" as ur order by ur.""Email""";

            //return ExecuteCypherList<UserAssessmentResultViewModel>(cypher).ToList();
            var res = await _queryRepo.ExecuteQueryList<UserAssessmentResultViewModel>(query, null);
            return res;
        }
        public async Task<TASUserReportViewModel> GetUserReportData(string userId)
        {

            var cypher = $@"select * from public.""TASUserReport"" as ur where ur.""UserId""='{userId}' ";
           
            var report = await _queryRepo.ExecuteQuerySingle<TASUserReportViewModel>(cypher, null);
            if (report.LastChangeInPosition.IsNotNullAndNotEmpty())
            {
                string mystr = " " + Regex.Replace(report.LastChangeInPosition, @"\d", "") + " ";
                string mynumber = " " + Regex.Replace(report.LastChangeInPosition, @"\D", "") + " ";
                report.LastChangeInPosition = mystr;
                report.Position = mynumber;
            }
            //if (mynumber.IsNotNull())
            //{                
            //    var res = string.Concat(" ",mystr," ",mynumber," ");

            //}
        


            return report;
        }

    }
}
