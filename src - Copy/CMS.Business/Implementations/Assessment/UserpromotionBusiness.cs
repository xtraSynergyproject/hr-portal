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
    public class UserPromotionBusiness : BusinessBase<UserPromotionViewModel, UserPromotion>, IUserPromotionBusiness
    {
        public UserPromotionBusiness(IRepositoryBase<UserPromotionViewModel, UserPromotion> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<UserPromotionViewModel>> Create(UserPromotionViewModel model)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserPromotionViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);

            return CommandResult<UserPromotionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserPromotionViewModel>> Edit(UserPromotionViewModel model)
        {
            //var data = _autoMapper.Map<ListOfValueViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserPromotionViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<UserPromotionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<UserPromotionViewModel>> IsNameExists(UserPromotionViewModel viewModel)
        {
            var list = await GetList();
         
            return CommandResult<UserPromotionViewModel>.Instance();
        }
    }
}
