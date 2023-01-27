using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ApplicationExperienceByOtherBusiness : BusinessBase<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther>, IApplicationExperienceByOtherBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationExperienceByOtherViewModel> _queryRepoId;
        public ApplicationExperienceByOtherBusiness(IRepositoryBase<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationExperienceByOtherViewModel> queryRepoId) : base(repo, autoMapper)
        {
            _queryRepoId = queryRepoId;
        }

        public async override Task<CommandResult<ApplicationExperienceByOtherViewModel>> Create(ApplicationExperienceByOtherViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<ApplicationExperienceByOtherViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
           
            var result = await base.Create(data, autoCommit);

            return CommandResult<ApplicationExperienceByOtherViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationExperienceByOtherViewModel>> Edit(ApplicationExperienceByOtherViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<CandidateExperienceByOtherViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateExperienceByOtherViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<ApplicationExperienceByOtherViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationExperienceByOtherViewModel>> IsNameExists(ApplicationExperienceByOtherViewModel model)
        {
                        
            return CommandResult<ApplicationExperienceByOtherViewModel>.Instance();
        }

        public async Task<IList<ApplicationExperienceByOtherViewModel>> GetListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT l.*,
                              o.""Name"" as OtherTypeName
                                FROM rec.""ApplicationExperienceByOther"" as l
                                LEFT JOIN rec.""ListOfValue"" as o ON o.""Id"" = l.""OtherTypeId""
                                WHERE l.""ApplicationId"" = '{candidateProfileId}' and l.""IsDeleted"" = false";


            var queryData = await _queryRepoId.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
    }
}
