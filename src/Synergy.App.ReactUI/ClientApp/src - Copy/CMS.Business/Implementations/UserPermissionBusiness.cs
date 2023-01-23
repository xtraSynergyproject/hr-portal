using AutoMapper;
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
    public class UserPermissionBusiness : BusinessBase<UserPermissionViewModel, UserPermission>, IUserPermissionBusiness
    {
        private readonly IUserPortalBusiness _userPortalBusiness;
        public UserPermissionBusiness(IRepositoryBase<UserPermissionViewModel, UserPermission> repo, IMapper autoMapper,IUserPortalBusiness userPortalBusiness) : base(repo, autoMapper)
        {
            _userPortalBusiness = userPortalBusiness;
        }

        public async override Task<CommandResult<UserPermissionViewModel>> Create(UserPermissionViewModel model)
        {
           
            var permission = new List<string>();
            
            if(model.Check1)            
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
            //    return CommandResult<UserPermissionViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model);

            return CommandResult<UserPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserPermissionViewModel>> Edit(UserPermissionViewModel data)
        {

           // var data = _autoMapper.Map<UserPermissionViewModel>(model);
            var permission = new List<string>();
            if (data.Check1)
                permission.Add(data.Permission1);
            if (data.Check2)
                permission.Add(data.Permission2);
            if (data.Check3)
                permission.Add(data.Permission3);
            if (data.Check4)
                permission.Add(data.Permission4);
            if (data.Check5)
                permission.Add(data.Permission5);
            if (data.Check6)
                permission.Add(data.Permission6);
            if (data.Check7)
                permission.Add(data.Permission7);
            if (data.Check8)
                permission.Add(data.Permission8);
            if (data.Check9)
                permission.Add(data.Permission9);
            if (data.Check10)
                permission.Add(data.Permission10);

            data.Permissions = permission.ToArray();
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<UserPermissionViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(data);

            return CommandResult<UserPermissionViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }
        
        
        public async Task<CommandResult<UserPermissionViewModel>> EditUserPermission(UserPermissionViewModel data)
        {
            var result = await base.Edit(data);

            return CommandResult<UserPermissionViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }
        
        public async Task<CommandResult<UserPermissionViewModel>> CreateUserPermission(UserPermissionViewModel data)
        {
            var result = await base.Create(data);

            return CommandResult<UserPermissionViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<UserPermissionViewModel>> IsNameExists(UserPermissionViewModel viewModel)
        {
            
            return CommandResult<UserPermissionViewModel>.Instance();
        }

        public async Task<List<UserPermissionViewModel>> GetUserPermission(string pageId,TemplateTypeEnum pageType,string portalId)
        {
            
            var user = await _repo.GetList<UserViewModel, User>();
            if (portalId.IsNotNullAndNotEmpty()) 
            {
                user = await _userPortalBusiness.GetUserByPortal(portalId);
            }           
             var userPerm = await GetList(x=>x.PageId==pageId);
            var list = new List<UserPermissionViewModel>();
            var permission = await _repo.GetList<PermissionViewModel, Permission>(x => x.PageTypes.Contains(pageType.ToString()) && x.UserPermissionTypes.Contains(UserPermissionTypeEnum.User.ToString()));
            //var permission = await _repo.GetList<PermissionViewModel, Permission>();
            permission = permission.OrderBy(x => x.Name).ToList();
            foreach(var item in user)
            {                
                var userPermission = new UserPermissionViewModel { UserId = item.Id,UserName=item.Name,DataAction=DataActionEnum.Read ,Status=StatusEnum.Active,PageId= pageId };
                var up = userPerm.Where(x => x.UserId == item.Id).FirstOrDefault();
                int i = 1;
                if(up!=null)
                {
                    userPermission.Id = up.Id;
                }
                foreach (var item3 in permission)
                {                    
                    var Col1 = string.Concat("Permission", i);
                    var Col2 = string.Concat("Check", i);
                    ApplicationExtension.SetPropertyValue(userPermission, Col1, item3.Code);

                    if(up!=null && up.Permissions.Contains(item3.Code))
                    ApplicationExtension.SetPropertyValue(userPermission, Col2, true);
                    else
                    {
                        ApplicationExtension.SetPropertyValue(userPermission, Col2, false);
                    }

                    i++;
                }
                list.Add(userPermission);
            }
            return list;
        }
        public async Task<CommandResult<UserPermissionViewModel>> SaveUserPermission(IList<UserPermissionViewModel> userPermissions)
        {
            var userpermission = userPermissions.FirstOrDefault();
            if (userpermission != null)
            {
                var list = await _repo.GetList(x => x.PageId == userpermission.PageId);
                foreach (var userPermission in userPermissions)
                {
                    if (list.Any(x => x.UserId == userPermission.UserId))
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


            return CommandResult<UserPermissionViewModel>.Instance();
        }

    }
}
