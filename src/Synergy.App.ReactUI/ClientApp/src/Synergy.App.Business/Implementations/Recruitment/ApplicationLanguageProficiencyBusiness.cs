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
    public class ApplicationLanguageProficiencyBusiness : BusinessBase<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency>, IApplicationLanguageProficiencyBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationLanguageProficiencyViewModel> _queryRepo;
        public ApplicationLanguageProficiencyBusiness(IRepositoryBase<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationLanguageProficiencyViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ApplicationLanguageProficiencyViewModel>> Create(ApplicationLanguageProficiencyViewModel model, bool autoCommit = true)
        {
            var validateSequenceOrder = await IsExists(model);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<ApplicationLanguageProficiencyViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var result = await base.Create(model,autoCommit);

            return CommandResult<ApplicationLanguageProficiencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationLanguageProficiencyViewModel>> Edit(ApplicationLanguageProficiencyViewModel model, bool autoCommit = true)
        {
            var validateSequenceOrder = await IsExists(model);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<ApplicationLanguageProficiencyViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var result = await base.Edit(model,autoCommit);

            return CommandResult<ApplicationLanguageProficiencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationLanguageProficiencyViewModel>> IsExists(ApplicationLanguageProficiencyViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.SequenceOrder == null)
            {
                errorList.Add("SlNo", "Sl No is required.");
            }
            else
            {
                var slno = await _repo.GetSingle(x => x.SequenceOrder == model.SequenceOrder && x.Id != model.Id && x.ApplicationId == model.ApplicationId);
                if (slno != null)
                {
                    errorList.Add("SlNo", "Sl No already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<ApplicationLanguageProficiencyViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<ApplicationLanguageProficiencyViewModel>.Instance();
        }
        public async Task<IList<ApplicationLanguageProficiencyViewModel>> GetListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                            l.""Name"" as LanguageName,
                              p.""Name"" as ProficiencyLevelName	
                                FROM rec.""ApplicationLanguageProficiency"" as c
                                LEFT JOIN rec.""ListOfValue"" as l ON l.""Id"" = c.""Language""
                                LEFT JOIN rec.""ListOfValue"" as p ON p.""Id"" = c.""ProficiencyLevel""
                                WHERE c.""ApplicationId""='{ candidateProfileId }' AND c.""IsDeleted""=false order by c.""SequenceOrder"" ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}
