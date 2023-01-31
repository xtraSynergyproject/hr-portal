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
    public class CandidateComputerProficiencyBusiness : BusinessBase<CandidateComputerProficiencyViewModel, CandidateComputerProficiency>, ICandidateComputerProficiencyBusiness
    {
        private readonly IRepositoryQueryBase<CandidateComputerProficiencyViewModel> _queryRepo;
        public CandidateComputerProficiencyBusiness(IRepositoryBase<CandidateComputerProficiencyViewModel, CandidateComputerProficiency> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateComputerProficiencyViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<CandidateComputerProficiencyViewModel>> Create(CandidateComputerProficiencyViewModel model)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CandidateComputerProficiencyViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);

            return CommandResult<CandidateComputerProficiencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateComputerProficiencyViewModel>> Edit(CandidateComputerProficiencyViewModel model)
        {
            var data = _autoMapper.Map<CandidateComputerProficiencyViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CandidateComputerProficiencyViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<CandidateComputerProficiencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<CandidateComputerProficiencyViewModel>> IsNameExists(CandidateComputerProficiencyViewModel model)
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
                return CommandResult<CandidateComputerProficiencyViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<CandidateComputerProficiencyViewModel>.Instance();
        }

        public async Task<IList<CandidateComputerProficiencyViewModel>> GetListByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT c.*, lov.""Name"" as ProficiencyLevelName 
                            FROM rec.""CandidateComputerProficiency"" as c
                            LEFT JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = c.""ProficiencyLevel""
                            WHERE c.""CandidateProfileId"" = '{candidateProfileId}' AND c.""IsDeleted""=false order by c.""SequenceOrder"" ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}
