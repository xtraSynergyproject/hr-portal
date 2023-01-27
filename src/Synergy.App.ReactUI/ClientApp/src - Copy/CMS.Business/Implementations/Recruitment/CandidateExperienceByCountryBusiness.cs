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
    public class CandidateExperienceByCountryBusiness : BusinessBase<CandidateExperienceByCountryViewModel, CandidateExperienceByCountry>, ICandidateExperienceByCountryBusiness
    {
        private readonly IRepositoryQueryBase<CandidateExperienceByCountryViewModel> _queryRepo;
        public CandidateExperienceByCountryBusiness(IRepositoryBase<CandidateExperienceByCountryViewModel, CandidateExperienceByCountry> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateExperienceByCountryViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<CandidateExperienceByCountryViewModel>> Create(CandidateExperienceByCountryViewModel model)
        {
            var data = _autoMapper.Map<CandidateExperienceByCountryViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceByCountryViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            //var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            //if (res.Count == 0)
            //{
            //    data.IsLatest = true;
            //}
            var result = await base.Create(data);

            return CommandResult<CandidateExperienceByCountryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateExperienceByCountryViewModel>> Edit(CandidateExperienceByCountryViewModel model)
        {
            var data = _autoMapper.Map<CandidateExperienceByCountryViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceByCountryViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<CandidateExperienceByCountryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<CandidateExperienceByCountryViewModel>> IsExists(CandidateExperienceByCountryViewModel model)
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
                return CommandResult<CandidateExperienceByCountryViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<CandidateExperienceByCountryViewModel>.Instance();
        }

      

        public async Task<IList<CandidateExperienceByCountryViewModel>> GetListByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                                ct.""Name"" as CountryName	
                                FROM rec.""CandidateExperienceByCountry"" as c
                                LEFT JOIN cms.""Country"" as ct ON ct.""Id"" = c.""CountryId""
                                 WHERE c.""CandidateProfileId"" = '{candidateProfileId}' and c.""IsDeleted"" = false order by c.""SequenceOrder""";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}
