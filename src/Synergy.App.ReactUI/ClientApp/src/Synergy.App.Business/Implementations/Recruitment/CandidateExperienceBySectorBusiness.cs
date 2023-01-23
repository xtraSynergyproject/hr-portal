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
    public class CandidateExperienceBySectorBusiness : BusinessBase<CandidateExperienceBySectorViewModel, CandidateExperienceBySector>, ICandidateExperienceBySectorBusiness
    {
        private readonly IRepositoryQueryBase<CandidateExperienceBySectorViewModel> _queryRepo;
        public CandidateExperienceBySectorBusiness(IRepositoryBase<CandidateExperienceBySectorViewModel, CandidateExperienceBySector> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateExperienceBySectorViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<CandidateExperienceBySectorViewModel>> Create(CandidateExperienceBySectorViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateExperienceBySectorViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceBySectorViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            //var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            //if (res.Count == 0)
            //{
            //    data.IsLatest = true;
            //}
            var result = await base.Create(data, autoCommit);

            return CommandResult<CandidateExperienceBySectorViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateExperienceBySectorViewModel>> Edit(CandidateExperienceBySectorViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateExperienceBySectorViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceBySectorViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var result = await base.Edit(model,autoCommit);

            return CommandResult<CandidateExperienceBySectorViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<CandidateExperienceBySectorViewModel>> IsExists(CandidateExperienceBySectorViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.SequenceOrder == null)
            {
                errorList.Add("SlNo", "Sl No is required.");
            }
            else
            {
                var slno = await _repo.GetSingle(x => x.SequenceOrder == model.SequenceOrder && x.Id != model.Id && x.CandidateProfileId == model.CandidateProfileId && x.IsDeleted==false);
                if (slno != null)
                {
                    errorList.Add("SlNo", "Sl No already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<CandidateExperienceBySectorViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<CandidateExperienceBySectorViewModel>.Instance();
        }

        public async Task<IList<CandidateExperienceBySectorViewModel>> GetListByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT l.*,
                            s.""Name"" as SectorName, i.""Name"" as IndustryName, c.""Name"" as CategoryName
                                FROM rec.""CandidateExperienceBySector"" as l
                                LEFT JOIN rec.""ListOfValue"" as s ON s.""Id"" = l.""Sector""
                                LEFT JOIN rec.""ListOfValue"" as i ON i.""Id"" = l.""Industry""
                                LEFT JOIN rec.""ListOfValue"" as c ON c.""Id"" = l.""Category""
                                WHERE l.""CandidateProfileId"" = '{candidateProfileId}' and l.""IsDeleted"" = false order by l.""SequenceOrder""";


            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
    }
}
