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
    public class UserRolePermissionBusiness : BusinessBase<UserRolePermissionViewModel, UserRolePermission>, IUserRolePermissionBusiness
    {
        private readonly IUserRolePortalBusiness _userRolePortalbusiness;
        public UserRolePermissionBusiness(IRepositoryBase<UserRolePermissionViewModel, UserRolePermission> repo, IMapper autoMapper, IUserRolePortalBusiness userRolePortalbusiness) : base(repo, autoMapper)
        {
            _userRolePortalbusiness = userRolePortalbusiness;
        }

        public async override Task<CommandResult<UserRolePermissionViewModel>> Create(UserRolePermissionViewModel model)
        {

            //var data = _autoMapper.Map<UserRolePermissionViewModel>(model);
            var permission = new List<string>();
            if (model.Check1)
                permission.Add(model.Permission1);
            if (model.Check2)
                permission.Add(model.Permission2);
            if (model.Check3)
                permission.Add(model.Permission3);
            if (model.Check4)
                permission.Add(model.Permission4);
            if (model.Check5)
                permission.Add(model.Permission5);
            if (model.Check6)
                permission.Add(model.Permission6);
            if (model.Check7)
                permission.Add(model.Permission7);
            if (model.Check8)
                permission.Add(model.Permission8);
            if (model.Check9)
                permission.Add(model.Permission9);
            if (model.Check10)
                permission.Add(model.Permission10);

            model.Permissions = permission.ToArray();
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<UserRolePermissionViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model);

            return CommandResult<UserRolePermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRolePermissionViewModel>> Edit(UserRolePermissionViewModel model)
        {

            //var data = _autoMapper.Map<UserRolePermissionViewModel>(model);
            var permission = new List<string>();
            if (model.Check1)
                permission.Add(model.Permission1);
            if (model.Check2)
                permission.Add(model.Permission2);
            if (model.Check3)
                permission.Add(model.Permission3);
            if (model.Check4)
                permission.Add(model.Permission4);
            if (model.Check5)
                permission.Add(model.Permission5);
            if (model.Check6)
                permission.Add(model.Permission6);
            if (model.Check7)
                permission.Add(model.Permission7);
            if (model.Check8)
                permission.Add(model.Permission8);
            if (model.Check9)
                permission.Add(model.Permission9);
            if (model.Check10)
                permission.Add(model.Permission10);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<UserRolePermissionViewModel>.Instance(model, false, validateName.Messages);
            //}
            model.Permissions = permission.ToArray();
            var result = await base.Edit(model);

            return CommandResult<UserRolePermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<UserRolePermissionViewModel>> IsNameExists(UserRolePermissionViewModel viewModel)
        {
            
            return CommandResult<UserRolePermissionViewModel>.Instance();
        }

        public async Task<List<UserRolePermissionViewModel>> GetUserRolePermission(string pageId, TemplateTypeEnum pageType, string portalId)
        {
            var user = await _repo.GetList<UserRoleViewModel, UserRole>();
            if (portalId.IsNotNullAndNotEmpty())
            {
                user = await _userRolePortalbusiness.GetUserRoleByPortal(portalId);
            }
            var userPerm = await GetList(x => x.PageId == pageId);
            var list = new List<UserRolePermissionViewModel>();
            var permission = await _repo.GetList<PermissionViewModel, Permission>(x => x.PageTypes.Contains(pageType.ToString()) && x.UserPermissionTypes.Contains(UserPermissionTypeEnum.UserRole.ToString()));
            //var permission = await _repo.GetList<PermissionViewModel, Permission>();
            permission = permission.OrderBy(x => x.Name).ToList();
            foreach (var item in user)
            {
                var userPermission = new UserRolePermissionViewModel { UserRoleId = item.Id, UserRoleName=item.Name,DataAction=DataActionEnum.Read,Status=StatusEnum.Active, PageId = pageId };
                var up = userPerm.Where(x => x.UserRoleId == item.Id).FirstOrDefault();
                if (up != null)
                {
                    userPermission.Id = up.Id;
                }
                int i = 1;
                foreach (var item3 in permission)
                {                   
                    var Col1 = string.Concat("Permission", i);
                    var Col2 = string.Concat("Check", i);
                    ApplicationExtension.SetPropertyValue(userPermission, Col1, item3.Code);

                    if (up != null && up.Permissions.Contains(item3.Code))
                        ApplicationExtension.SetPropertyValue(userPermission, Col2, true);

                    i++;
                }
                list.Add(userPermission);
            }
            return list;
        }
        public async Task<CommandResult<UserRolePermissionViewModel>> SaveUserRolePermission(IList<UserRolePermissionViewModel> userPermissions)
        {
            var userpermission = userPermissions.FirstOrDefault();
            if (userpermission!=null) 
            {
                var list=await _repo.GetList(x => x.PageId == userpermission.PageId);
                foreach (var userPermission in userPermissions)
                {                    
                    if (list.Any(x => x.UserRoleId == userPermission.UserRoleId))
                    {
                        // Edit
                        userPermission.DataAction = DataActionEnum.Edit;
                        await Edit(userPermission);
                    }
                    else
                    {
                        // create
                        userPermission.DataAction = DataActionEnum.Create;
                        await Create(userPermission);
                    }
                   

                }
            }
           
         
            return CommandResult<UserRolePermissionViewModel>.Instance();
        }

        public async Task<CommandResult<UserRolePermissionViewModel>> CreateUserRolePermission(UserRolePermissionViewModel model)
        {

            var result = await base.Create(model);

            return CommandResult<UserRolePermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<UserRolePermissionViewModel>> EditUserRolePermission(UserRolePermissionViewModel model)
        {           
            var result = await base.Edit(model);
            return CommandResult<UserRolePermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

    }
}
