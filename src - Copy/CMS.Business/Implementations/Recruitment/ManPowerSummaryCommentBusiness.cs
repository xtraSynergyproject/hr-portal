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
    public class ManPowerSummaryCommentBusiness : BusinessBase<ManpowerSummaryCommentViewModel, ManpowerSummaryComment>, IManpowerSummaryCommentBusiness
    {
        private IUserBusiness _userBusiness;


        public ManPowerSummaryCommentBusiness(IRepositoryBase<ManpowerSummaryCommentViewModel, ManpowerSummaryComment> repo, IMapper autoMapper, IUserBusiness userBusiness) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;

        }

        public async override Task<CommandResult<ManpowerSummaryCommentViewModel>> Create(ManpowerSummaryCommentViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);

            var result = await base.Create(model);
            return CommandResult<ManpowerSummaryCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ManpowerSummaryCommentViewModel>> Edit(ManpowerSummaryCommentViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<ManpowerSummaryCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}