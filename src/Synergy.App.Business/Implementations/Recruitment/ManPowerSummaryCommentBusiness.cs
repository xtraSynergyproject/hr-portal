﻿using AutoMapper;
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
    public class ManPowerSummaryCommentBusiness : BusinessBase<ManpowerSummaryCommentViewModel, ManpowerSummaryComment>, IManpowerSummaryCommentBusiness
    {
        private IUserBusiness _userBusiness;


        public ManPowerSummaryCommentBusiness(IRepositoryBase<ManpowerSummaryCommentViewModel, ManpowerSummaryComment> repo, IMapper autoMapper, IUserBusiness userBusiness) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;

        }

        public async override Task<CommandResult<ManpowerSummaryCommentViewModel>> Create(ManpowerSummaryCommentViewModel model, bool autoCommit = true)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);

            var result = await base.Create(model,autoCommit);
            return CommandResult<ManpowerSummaryCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ManpowerSummaryCommentViewModel>> Edit(ManpowerSummaryCommentViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            return CommandResult<ManpowerSummaryCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}