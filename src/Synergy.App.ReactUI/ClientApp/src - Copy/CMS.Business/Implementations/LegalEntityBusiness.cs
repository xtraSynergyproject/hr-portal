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
    public class LegalEntityBusiness : BusinessBase<LegalEntityViewModel, LegalEntity>, ILegalEntityBusiness
    {
        private readonly IRepositoryQueryBase<LegalEntityViewModel> _queryRepo;
        public LegalEntityBusiness(IRepositoryBase<LegalEntityViewModel, LegalEntity> repo, IRepositoryQueryBase<LegalEntityViewModel> queryRepo, IMapper autoMapper) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<LegalEntityViewModel>> Create(LegalEntityViewModel model)
        {
            var exist = await IsExists(model);
            if (!exist.IsSuccess)
            {
                return CommandResult<LegalEntityViewModel>.Instance(model, false, exist.Messages);
            }
            var result = await base.Create(model);
            return CommandResult<LegalEntityViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<LegalEntityViewModel>> Edit(LegalEntityViewModel model)
        {
            var exist = await IsExists(model);
            if (!exist.IsSuccess)
            {
                return CommandResult<LegalEntityViewModel>.Instance(model, false, exist.Messages);
            }
            var result = await base.Edit(model);
            return CommandResult<LegalEntityViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

      
        public async Task<List<LegalEntityViewModel>> GetData()
        {

            string query = @$"SELECT c.*, co.""Name"" as CountryName
                            FROM public.""LegalEntity"" as c                           
                            LEFT JOIN cms.""Country"" as co ON co.""Id"" = c.""CountryId"" and co.""IsDeleted""=false
                            where c.""IsDeleted"" = false";
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }

        private async Task<CommandResult<LegalEntityViewModel>> IsExists(LegalEntityViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Name is required.");
            }
            else
            {
                var name = await _repo.GetSingle(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");
                }
            }
            if (model.Code.IsNullOrEmpty())
            {
                errorList.Add("Code", "Code is required.");
            }
            else
            {
                var code = await _repo.GetSingle(x => x.Code.ToLower() == model.Code.ToLower() && x.Id != model.Id);
                if (code != null)
                {
                    errorList.Add("Code", "Code already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<LegalEntityViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<LegalEntityViewModel>.Instance();
        }
        public async Task<int> GetFinancialYear(DateTime date)
        {
            var financialYear = date.Year;
            var cypher = $@"SELECT l.* FROM public.""LegalEntity"" as l
            join public.""User"" as u on u.""LegalEntityId"" = l.""Id""  and u.""IsDeleted""=false
            where u.""Id""='{_repo.UserContext.UserId}' and l.""IsDeleted""=false";
           
            var legalEntity = await _queryRepo.ExecuteQuerySingle(cypher,null);
            if (legalEntity != null)
            {
                if (legalEntity.FiscalYearType != null)
                {
                    if (legalEntity.FiscalYearType.Value == FiscalYearTypeEnum.StartsPreviousYear && date.Month >= (int)legalEntity.FiscalYearStartMonth)
                    {
                        financialYear++;
                    }
                    else if (legalEntity.FiscalYearType.Value == FiscalYearTypeEnum.EndsNextYear && date.Month <= (int)legalEntity.FiscalYearEndMonth)
                    {
                        financialYear--;
                    }
                }

            }
            return financialYear;
        }

    }
}
