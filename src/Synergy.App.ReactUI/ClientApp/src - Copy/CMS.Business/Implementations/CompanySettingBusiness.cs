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
    public class CompanySettingBusiness : BusinessBase<CompanySettingViewModel, CompanySetting>, ICompanySettingBusiness
    {
        public CompanySettingBusiness(IRepositoryBase<CompanySettingViewModel, CompanySetting> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<CompanySettingViewModel>> Create(CompanySettingViewModel model)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CompanySettingViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);

            return CommandResult<CompanySettingViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CompanySettingViewModel>> Edit(CompanySettingViewModel model)
        {
            //var data = _autoMapper.Map<ListOfValueViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CompanySettingViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<CompanySettingViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<CompanySettingViewModel>> IsNameExists(CompanySettingViewModel viewModel)
        {
            var list = await GetList();
            if (list.Exists(x => x.Id != viewModel.Id && viewModel.Code.Equals(x.Code, StringComparison.InvariantCultureIgnoreCase)))
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The given Code exists. Please enter another Code");
                return CommandResult<CompanySettingViewModel>.Instance(viewModel, false, obj);
            }
            return CommandResult<CompanySettingViewModel>.Instance();
        }
    }
}
