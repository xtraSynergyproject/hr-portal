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
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ApplicationJobCriteriaBusiness : BusinessBase<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>, IApplicationJobCriteriaBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationJobCriteriaViewModel> _queryRepo;
        public ApplicationJobCriteriaBusiness(IRepositoryBase<ApplicationJobCriteriaViewModel, ApplicationJobCriteria> repo, IMapper autoMapper, IRepositoryQueryBase<ApplicationJobCriteriaViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ApplicationJobCriteriaViewModel>> Create(ApplicationJobCriteriaViewModel model)
        {
            //var data = _autoMapper.Map<JobCriteriaViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobCriteriaViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model);

            return CommandResult<ApplicationJobCriteriaViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationJobCriteriaViewModel>> Edit(ApplicationJobCriteriaViewModel model)
        {
            //var data = _autoMapper.Map<JobCriteriaViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobCriteriaViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model);

            return CommandResult<ApplicationJobCriteriaViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationJobCriteriaViewModel>> IsNameExists(ApplicationJobCriteriaViewModel model)
        {
                        
            return CommandResult<ApplicationJobCriteriaViewModel>.Instance();
        }

        public async Task<IList<ApplicationJobCriteriaViewModel>> GetCriteriaData(string applicationId, string type)
        {
            string query = @$"select jc.*, lv.""Name"" as CriteriaValue,lv1.""Name"" as ListOfValueType,lv.""Description"" as Description from rec.""ApplicationJobCriteria"" as jc
                                left join rec.""ListOfValue"" as lv on lv.""Id"" = jc.""Value""
                                 left join rec.""ListOfValue"" as lv1 on lv1.""Id"" = jc.""ListOfValueTypeId""
                                where jc.""ApplicationId"" = '{applicationId}' and jc.""Type"" = '{type}' and jc.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);            
            return queryData;
        }
    }
}
