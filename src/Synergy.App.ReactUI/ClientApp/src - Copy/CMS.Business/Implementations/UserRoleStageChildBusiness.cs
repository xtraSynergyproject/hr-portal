﻿using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class UserRoleStageChildBusiness : BusinessBase<UserRoleStageChildViewModel, UserRoleStageChild>, IUserRoleStageChildBusiness
    {
        private IUserBusiness _userBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        public UserRoleStageChildBusiness(IRepositoryBase<UserRoleStageChildViewModel, UserRoleStageChild> repo, IMapper autoMapper, IUserBusiness userBusiness, IUserRoleUserBusiness userRoleUserBusiness) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
        }

        //public async override Task<CommandResult<UserRoleViewModel>> Create(UserRoleViewModel model)
        //{
           
        //   // var data = _autoMapper.Map<UserRoleViewModel>(model);
        //    var validateName = await IsNameExists(model);
        //    if (!validateName.IsSuccess)
        //    {
        //        return CommandResult<UserRoleViewModel>.Instance(model, false, validateName.Messages);
        //    }
        //    var result = await base.Create(model);
        //    if (model.UserIds!=null && model.UserIds.Count()>0) 
        //    {
        //        foreach (var id in model.UserIds)
        //        {
        //            var user = new UserRoleUserViewModel();
        //            user.UserRoleId = result.Item.Id;
        //            user.UserId = id;
        //            await _userRoleUserBusiness.Create(user);

        //        }
        //    }
            
        //    return CommandResult<UserRoleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        //}

        //public async override Task<CommandResult<UserRoleViewModel>> Edit(UserRoleViewModel model)
        //{
           
        //   // var data = _autoMapper.Map<UserRoleViewModel>(model);
        //    var validateName = await IsNameExists(model);
        //    if (!validateName.IsSuccess)
        //    {
        //        return CommandResult<UserRoleViewModel>.Instance(model, false, validateName.Messages);
        //    }

        //    var result = await base.Edit(model);
        //    var pagecol = await _userRoleUserBusiness.GetList(x => x.UserRoleId == model.Id);
        //    var existingIds = pagecol.Select(x => x.UserId);
        //    var newIds = model.UserIds;
        //    var ToDelete = existingIds.Except(newIds).ToList(); 
        //    var ToAdd = newIds.Except(existingIds).ToList();     
        //    // Add
        //    foreach (var id in ToAdd)
        //    {
        //        var user = new UserRoleUserViewModel();
        //        user.UserRoleId = result.Item.Id;
        //        user.UserId = id;
        //        await _userRoleUserBusiness.Create(user);
        //    }
        //    // Delete
        //    foreach (var id in ToDelete)
        //    {
        //        var role = await _userRoleUserBusiness.GetSingle(x => x.UserRoleId == model.Id && x.UserId== id);
        //        await _userRoleUserBusiness.Delete(role.Id);
        //    }

        //    return CommandResult<UserRoleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        //}
        //private async Task<CommandResult<UserRoleViewModel>> IsNameExists(UserRoleViewModel viewModel)
        //{

        //    Dictionary<string, string> obj = new Dictionary<string, string>();
        //    if (viewModel.Name.IsNullOrEmpty())
        //    {
        //        obj.Add("Name", "User Role Name is required.");
        //    }
        //    var pagelist = await GetList(x => x.Name == viewModel.Name);
        //    if (pagelist.Exists(x => x.Id != viewModel.Id && x.Name.ToLower() == viewModel.Name.ToLower()))
        //    {
                
        //        obj.Add("NameExist", "The User Role already exists. Please choose another user Role");
                
        //    }
        //    if (obj.Count > 0)
        //    {
        //        return CommandResult<UserRoleViewModel>.Instance(viewModel, false, obj);
        //    }
        //   return CommandResult<UserRoleViewModel>.Instance();
        //}

    }
}
