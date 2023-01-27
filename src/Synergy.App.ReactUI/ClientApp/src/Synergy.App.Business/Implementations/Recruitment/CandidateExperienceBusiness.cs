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
    public class CandidateExperienceBusiness : BusinessBase<CandidateExperienceViewModel, CandidateExperience>, ICandidateExperienceBusiness
    {
        private readonly IRepositoryQueryBase<CandidateExperienceViewModel> _queryRepo;
        public CandidateExperienceBusiness(IRepositoryBase<CandidateExperienceViewModel, CandidateExperience> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateExperienceViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<CandidateExperienceViewModel>> Create(CandidateExperienceViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateExperienceViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
            //var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            //if(res.Count == 0)
            //{
            //    data.IsLatest = true;
            //}
            var result = await base.Create(data, autoCommit);

            return CommandResult<CandidateExperienceViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateExperienceViewModel>> Edit(CandidateExperienceViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateExperienceViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(data,autoCommit);

            return CommandResult<CandidateExperienceViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<CandidateExperienceViewModel>> IsNameExists(CandidateExperienceViewModel model)
        {                        
            return CommandResult<CandidateExperienceViewModel>.Instance();
        }

        public async Task<IList<CandidateExperienceViewModel>> GetListByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT c.*, f.""FileName"" as AttachmentName	
                                FROM rec.""CandidateExperience"" as c                                
                                LEFT JOIN public.""File"" as f ON f.""Id"" = c.""AttachmentId""
                                where c.""CandidateProfileId""='{candidateProfileId}' and c.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            //var list = new List<CandidateEducationalViewModel>();

            return queryData;
        }
        public async Task<CandidateExperienceViewModel> GetCandidateExperienceDuration(string candidateProfileId)
        {
            string query = @$"SELECT sum(c.""Duration"") as TotalDuration	
                                FROM rec.""CandidateExperience"" as c                                
                                where c.""CandidateProfileId""='{candidateProfileId}' and c.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            //var list = new List<CandidateEducationalViewModel>();

            return queryData;
        }
    }
}
