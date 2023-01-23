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
    public class MemberBusiness : BusinessBase<MemberViewModel, Member>, IMemberBusiness
    {
        public MemberBusiness(IRepositoryBase<MemberViewModel, Member> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<MemberViewModel>> Create(MemberViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<MemberViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<MemberViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data, autoCommit);

            return CommandResult<MemberViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<MemberViewModel>> Edit(MemberViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<MemberViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<MemberViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data,autoCommit);

            return CommandResult<MemberViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<MemberViewModel>> IsNameExists(MemberViewModel viewModel)
        {
            var pagelist = await GetList(x => x.SynergyUserId == viewModel.SynergyUserId);
            if (pagelist.Exists(x => x.Id != viewModel.Id && x.SynergyUserId == viewModel.SynergyUserId))
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The User already exists. Please choose another user");
                return CommandResult<MemberViewModel>.Instance(viewModel, false, obj);
            }
            return CommandResult<MemberViewModel>.Instance();
        }

    }
}
