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
    public class ApplicationExperienceByJobBusiness : BusinessBase<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob>, IApplicationExperienceByJobBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationExperienceByJobViewModel> _queryRepo;
        public ApplicationExperienceByJobBusiness(IRepositoryBase<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationExperienceByJobViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

    

    public async override Task<CommandResult<ApplicationExperienceByJobViewModel>> Create(ApplicationExperienceByJobViewModel model)
        {           
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateExperienceByJobViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model);

            return CommandResult<ApplicationExperienceByJobViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationExperienceByJobViewModel>> Edit(ApplicationExperienceByJobViewModel model)
        {
            //var data = _autoMapper.Map<CandidateExperienceByJobViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateExperienceByJobViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model);

            return CommandResult<ApplicationExperienceByJobViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationExperienceByJobViewModel>> IsNameExists(ApplicationExperienceByJobViewModel model)
        {
                        
            return CommandResult<ApplicationExperienceByJobViewModel>.Instance();
        }
        public async Task<IList<ApplicationExperienceByJobViewModel>> GetListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                                ct.""Name"" as JobName	
                                FROM rec.""ApplicationExperienceByJob"" as c
                                LEFT JOIN cms.""Job"" as ct ON ct.""Id"" = c.""JobId""
                                WHERE c.""IsDeleted""=false AND c.""ApplicationId""='" + candidateProfileId + "'";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}
