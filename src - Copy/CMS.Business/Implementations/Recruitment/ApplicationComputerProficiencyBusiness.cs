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
    public class ApplicationComputerProficiencyBusiness : BusinessBase<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency>, IApplicationComputerProficiencyBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationComputerProficiencyViewModel> _queryRepo;
        public ApplicationComputerProficiencyBusiness(IRepositoryBase<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationComputerProficiencyViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ApplicationComputerProficiencyViewModel>> Create(ApplicationComputerProficiencyViewModel model)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ApplicationComputerProficiencyViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);

            return CommandResult<ApplicationComputerProficiencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationComputerProficiencyViewModel>> Edit(ApplicationComputerProficiencyViewModel model)
        {
            var data = _autoMapper.Map<ApplicationComputerProficiencyViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ApplicationComputerProficiencyViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<ApplicationComputerProficiencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationComputerProficiencyViewModel>> IsNameExists(ApplicationComputerProficiencyViewModel model)
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
                return CommandResult<ApplicationComputerProficiencyViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<ApplicationComputerProficiencyViewModel>.Instance();
        }

        public async Task<IList<ApplicationComputerProficiencyViewModel>> GetListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT c.*, lov.""Name"" as ProficiencyLevelName 
                            FROM rec.""ApplicationComputerProficiency"" as c
                            LEFT JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = c.""ProficiencyLevel""
                            WHERE c.""ApplicationId"" = '{candidateProfileId}' AND c.""IsDeleted""=false order by c.""SequenceOrder"" ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}
