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
    public class UserRoleBusiness : BusinessBase<UserRoleViewModel, UserRole>, IUserRoleBusiness
    {
        private readonly IRepositoryQueryBase<UserViewModel> _queryRepo;
        private IUserBusiness _userBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        public UserRoleBusiness(IRepositoryBase<UserRoleViewModel, UserRole> repo, IMapper autoMapper, IRepositoryQueryBase<UserViewModel> queryRepo, IUserBusiness userBusiness, IUserRoleUserBusiness userRoleUserBusiness) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
            _queryRepo = queryRepo;

        }

        public async override Task<CommandResult<UserRoleViewModel>> Create(UserRoleViewModel model)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserRoleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);
            //if (model.UserIds!=null && model.UserIds.Count()>0) 
            //{
            //    foreach (var id in model.UserIds)
            //    {
            //        var user = new UserRoleUserViewModel();
            //        user.UserRoleId = result.Item.Id;
            //        user.UserId = id;
            //        await _userRoleUserBusiness.Create(user);

            //    }
            //}
            
            return CommandResult<UserRoleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRoleViewModel>> Edit(UserRoleViewModel model)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserRoleViewModel>.Instance(model, false, validateName.Messages);
            }

            var result = await base.Edit(model);
            //var pagecol = await _userRoleUserBusiness.GetList(x => x.UserRoleId == model.Id);
            //var existingIds = pagecol.Select(x => x.UserId);
            //var newIds = model.UserIds;
            //var ToDelete = existingIds.Except(newIds).ToList(); 
            //var ToAdd = newIds.Except(existingIds).ToList();     
            // Add
            //foreach (var id in ToAdd)
            //{
            //    var user = new UserRoleUserViewModel();
            //    user.UserRoleId = result.Item.Id;
            //    user.UserId = id;
            //    await _userRoleUserBusiness.Create(user);
            //}
            //// Delete
            //foreach (var id in ToDelete)
            //{
            //    var role = await _userRoleUserBusiness.GetSingle(x => x.UserRoleId == model.Id && x.UserId== id);
            //    await _userRoleUserBusiness.Delete(role.Id);
            //}

            return CommandResult<UserRoleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<UserRoleViewModel>> IsNameExists(UserRoleViewModel viewModel)
        {

            Dictionary<string, string> obj = new Dictionary<string, string>();
            if (viewModel.Name.IsNullOrEmpty())
            {
                obj.Add("Name", "User Role Name is required.");
            }
            var pagelist = await GetList(x => x.Name == viewModel.Name);
            if (pagelist.Exists(x => x.Id != viewModel.Id && x.Name.ToLower() == viewModel.Name.ToLower()))
            {
                
                obj.Add("NameExist", "The User Role already exists. Please choose another user Role");
                
            }
            if (obj.Count > 0)
            {
                return CommandResult<UserRoleViewModel>.Instance(viewModel, false, obj);
            }
           return CommandResult<UserRoleViewModel>.Instance();
        }
        public async Task<List<UserViewModel>> GetUserRoleForUser(string id)
        {
            var query = @"SELECT ur.""Id"" as Id, ur.""Name"" as Name FROM public.""UserRole"" as ur";
            var queryData = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            if (id.IsNotNullAndNotEmpty())
            {
                queryData = queryData.Where(x => x.Id == id).ToList();
            }
            return queryData;
        }
        public async Task<List<UserRoleViewModel>> GetUserRolesWithPortalIds()
        {

            var Query = $@"SELECT distinct u.* FROM public.""UserRole"" as u 
 join public.""UserRolePortal"" as up on up.""UserRoleId""=u.""Id"" and up.""IsDeleted""=false
join public.""Company"" as c on c.""Id""='{_repo.UserContext.CompanyId}' and up.""PortalId""= Any(c.""LicensedPortalIds"") and c.""IsDeleted""=false
  where u.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<UserRoleViewModel>(Query, null);
            return list;

        }

    }
}
