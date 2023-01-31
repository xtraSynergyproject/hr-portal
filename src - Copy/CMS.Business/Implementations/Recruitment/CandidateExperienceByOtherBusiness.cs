using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class CandidateExperienceByOtherBusiness : BusinessBase<CandidateExperienceByOtherViewModel, CandidateExperienceByOther>, ICandidateExperienceByOtherBusiness
    {
        private readonly IRepositoryQueryBase<CandidateExperienceByOtherViewModel> _queryRepoId;
        public CandidateExperienceByOtherBusiness(IRepositoryBase<CandidateExperienceByOtherViewModel, CandidateExperienceByOther> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateExperienceByOtherViewModel> queryRepoId) : base(repo, autoMapper)
        {
            _queryRepoId = queryRepoId;
        }

      
       public async override Task<CommandResult<CandidateExperienceByOtherViewModel>> Create(CandidateExperienceByOtherViewModel model)
        {
            var data = _autoMapper.Map<CandidateExperienceByOtherViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceByOtherViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            if (res.Count == 0)
            {
                data.IsLatest = true;
            }
            var result = await base.Create(data);

            return CommandResult<CandidateExperienceByOtherViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateExperienceByOtherViewModel>> Edit(CandidateExperienceByOtherViewModel model)
        {
            var data = _autoMapper.Map<CandidateExperienceByOtherViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceByOtherViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<CandidateExperienceByOtherViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<CandidateExperienceByOtherViewModel>> IsExists(CandidateExperienceByOtherViewModel model)
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
                return CommandResult<CandidateExperienceByOtherViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<CandidateExperienceByOtherViewModel>.Instance();
        }



        public async Task<IList<CandidateExperienceByOtherViewModel>> GetNameByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT l.*,
                              o.""Name"" as OtherTypeName
                                FROM rec.""CandidateExperienceByOther"" as l
                                LEFT JOIN rec.""ListOfValue"" as o ON o.""Id"" = l.""OtherTypeId""
                                 WHERE l.""CandidateProfileId"" = '{candidateProfileId}' and l.""IsDeleted"" = false order by l.""SequenceOrder""";


            var queryData = await _queryRepoId.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
    }
}
