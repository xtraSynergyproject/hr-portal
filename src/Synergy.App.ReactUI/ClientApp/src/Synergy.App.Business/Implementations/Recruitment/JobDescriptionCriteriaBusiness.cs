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
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class JobDescriptionCriteriaBusiness : BusinessBase<JobDescriptionCriteriaViewModel, JobDescriptionCriteria>, IJobDescriptionCriteriaBusiness
    {
        private readonly IRepositoryQueryBase<JobDescriptionCriteriaViewModel> _queryRepo;
        public JobDescriptionCriteriaBusiness(IRepositoryBase<JobDescriptionCriteriaViewModel, JobDescriptionCriteria> repo, IMapper autoMapper
            , IRepositoryQueryBase<JobDescriptionCriteriaViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<JobDescriptionCriteriaViewModel>> Create(JobDescriptionCriteriaViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<JobDescriptionCriteriaViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobDescriptionCriteriaViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model,autoCommit);

            return CommandResult<JobDescriptionCriteriaViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<JobDescriptionCriteriaViewModel>> Edit(JobDescriptionCriteriaViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<JobDescriptionCriteriaViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobDescriptionCriteriaViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<JobDescriptionCriteriaViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<JobDescriptionCriteriaViewModel>> IsNameExists(JobDescriptionCriteriaViewModel model)
        {
                        
            return CommandResult<JobDescriptionCriteriaViewModel>.Instance();
        }
        //public async Task<IList<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByJobAndType(string JobAdvertisementId,string type)
        //{

        //    string query1 = @$"SELECT j.""Id"" as Id,j.""Criteria"" as Criteria,j.""CriteriaType"" as CriteriaType,g.""Code"" as CriteriaTypeCode,j.""Type"" as Type,j.""Weightage"" as Weightage              
        //                    FROM rec.""JobCriteria"" as j
        //                    LEFT JOIN rec.""ListOfValue"" as g ON g.""Id"" = j.""CriteriaType""
        //                    where j.""JobAdvertisementId""='{JobAdvertisementId}' and j.""Type""='{type}'
        //                    ";
        //    var list = await _queryRepo.ExecuteQueryList<ApplicationJobCriteriaViewModel>(query1, null);
        //    return list;
        //}
        //public async Task<IList<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByApplicationIdAndType(string ApplicationId, string type)
        //{

        //    string query1 = @$"SELECT a.*,g.""Code"" as CriteriaTypeCode            
        //                    FROM rec.""ApplicationJobCriteria"" as a
        //                    LEFT JOIN rec.""ListOfValue"" as g ON g.""Id"" = a.""CriteriaType""
        //                    where a.""ApplicationId""='{ApplicationId}' and a.""Type""='{type}'
        //                    ";
        //    var list = await _queryRepo.ExecuteQueryList<ApplicationJobCriteriaViewModel>(query1, null);
        //    return list;
        //}
    }
}
