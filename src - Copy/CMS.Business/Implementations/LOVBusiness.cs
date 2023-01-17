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
    public class LOVBusiness : BusinessBase<LOVViewModel, LOV>, ILOVBusiness
    {
        public LOVBusiness(IRepositoryBase<LOVViewModel, LOV> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<LOVViewModel>> Create(LOVViewModel model)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<LOVViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);

            return CommandResult<LOVViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<LOVViewModel>> Edit(LOVViewModel model)
        {
            //var data = _autoMapper.Map<ListOfValueViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<LOVViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<LOVViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<LOVViewModel>> IsNameExists(LOVViewModel viewModel)
        {
            var list = await GetList();
            if (list.Exists(x => x.Id != viewModel.Id && viewModel.Code.Equals(x.Code, StringComparison.InvariantCultureIgnoreCase)))
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The given Code exists. Please enter another Code");
                return CommandResult<LOVViewModel>.Instance(viewModel, false, obj);
            }
            return CommandResult<LOVViewModel>.Instance();
        }
    }
}
