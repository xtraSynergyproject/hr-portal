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
    public class CandidateExperienceByJobBusiness : BusinessBase<CandidateExperienceByJobViewModel, CandidateExperienceByJob>, ICandidateExperienceByJobBusiness
    {
        private readonly IRepositoryQueryBase<CandidateExperienceByJobViewModel> _queryRepo;
        public CandidateExperienceByJobBusiness(IRepositoryBase<CandidateExperienceByJobViewModel, CandidateExperienceByJob> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateExperienceByJobViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }



        public async override Task<CommandResult<CandidateExperienceByJobViewModel>> Create(CandidateExperienceByJobViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateExperienceByJobViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceByJobViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            if (res.Count == 0)
            {
                data.IsLatest = true;
            }
            var result = await base.Create(data, autoCommit);

            return CommandResult<CandidateExperienceByJobViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateExperienceByJobViewModel>> Edit(CandidateExperienceByJobViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateExperienceByJobViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceByJobViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var result = await base.Edit(model,autoCommit);

            return CommandResult<CandidateExperienceByJobViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<CandidateExperienceByJobViewModel>> IsExists(CandidateExperienceByJobViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.SequenceOrder == null)
            {
                errorList.Add("SlNo", "Sl No is required.");
            }
            else
            {
                var slno = await _repo.GetSingle(x => x.SequenceOrder == model.SequenceOrder && x.Id != model.Id && x.CandidateProfileId == model.CandidateProfileId && x.IsDeleted == false);
                if (slno != null)
                {
                    errorList.Add("SlNo", "Sl No already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<CandidateExperienceByJobViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<CandidateExperienceByJobViewModel>.Instance();
        }

       
        public async Task<IList<CandidateExperienceByJobViewModel>> GetListByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                                ct.""Name"" as JobName	
                                FROM rec.""CandidateExperienceByJob"" as c
                                LEFT JOIN cms.""Job"" as ct ON ct.""Id"" = c.""JobId""
                                 WHERE c.""CandidateProfileId"" = '{candidateProfileId}' and c.""IsDeleted"" = false order by c.""SequenceOrder""";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}
