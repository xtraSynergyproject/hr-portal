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
    public class LegalEntityBusiness : BusinessBase<LegalEntityViewModel, LegalEntity>, ILegalEntityBusiness
    {
        private readonly IRepositoryQueryBase<LegalEntityViewModel> _queryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public LegalEntityBusiness(IRepositoryBase<LegalEntityViewModel, LegalEntity> repo, IRepositoryQueryBase<LegalEntityViewModel> queryRepo, IMapper autoMapper
            , ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness; 
        }

        public async override Task<CommandResult<LegalEntityViewModel>> Create(LegalEntityViewModel model, bool autoCommit = true)
        {
            var exist = await IsExists(model);
            if (!exist.IsSuccess)
            {
                return CommandResult<LegalEntityViewModel>.Instance(model, false, exist.Messages);
            }
            var result = await base.Create(model,autoCommit);
            return CommandResult<LegalEntityViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<LegalEntityViewModel>> Edit(LegalEntityViewModel model, bool autoCommit = true)
        {
            var exist = await IsExists(model);
            if (!exist.IsSuccess)
            {
                return CommandResult<LegalEntityViewModel>.Instance(model, false, exist.Messages);
            }
            var result = await base.Edit(model,autoCommit);
            return CommandResult<LegalEntityViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

      
        public async Task<List<LegalEntityViewModel>> GetData()
        {
            var list = await _cmsQueryBusiness.GetData();
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
          var legalEntity = await _cmsQueryBusiness.GetFinancialYearData();
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

        public async Task<List<LegalEntityViewModel>> GetLegalEntityByLocationData()
        {
            var list = await _cmsQueryBusiness.GetLegalEntityByLocationData();
            return list;
        }

    }
}
