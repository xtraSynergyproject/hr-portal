using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class UserReportBusiness : BusinessBase<TASUserReportViewModel, TASUserReport>, IUserReportBusiness
    {
        private readonly IRepositoryQueryBase<UserAssessmentResultViewModel> _queryRepo;
        private readonly IAssessmentQueryBusiness _assessmentQueryBusiness;
        public UserReportBusiness(IRepositoryBase<TASUserReportViewModel, TASUserReport> repo, IMapper autoMapper
            , IRepositoryQueryBase<UserAssessmentResultViewModel> queryRepo, IAssessmentQueryBusiness assessmentQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _assessmentQueryBusiness = assessmentQueryBusiness;
        }

        public async override Task<CommandResult<TASUserReportViewModel>> Create(TASUserReportViewModel model, bool autoCommit = true)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TASUserReportViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model, autoCommit);

            return CommandResult<TASUserReportViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TASUserReportViewModel>> Edit(TASUserReportViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<ListOfValueViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<TASUserReportViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model, autoCommit);

            return CommandResult<TASUserReportViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<TASUserReportViewModel>> IsNameExists(TASUserReportViewModel viewModel)
        {
            var list = await GetList();

            return CommandResult<TASUserReportViewModel>.Instance();
        }

        public async Task<List<UserAssessmentResultViewModel>> GetAssessmentResultList()
        {
            var result = await _assessmentQueryBusiness.GetAssessmentResultListData();
            return result;
        }
        public async Task<TASUserReportViewModel> GetUserReportData(string userId)
        {
            var report = await _assessmentQueryBusiness.GetUserReportData(userId);

            if (report.LastChangeInPosition.IsNotNullAndNotEmpty())
            {
                string mystr = " " + Regex.Replace(report.LastChangeInPosition, @"\d", "") + " ";
                string mynumber = " " + Regex.Replace(report.LastChangeInPosition, @"\D", "") + " ";
                report.LastChangeInPosition = mystr;
                report.Position = mynumber;
            }

            return report;
        }

    }
}
