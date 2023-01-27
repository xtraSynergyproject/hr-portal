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
    public class UserGroupBusiness : BusinessBase<UserGroupViewModel, UserGroup>, IUserGroupBusiness
    {
        private readonly IRepositoryQueryBase<UserGroupViewModel> _queryRepo;
        private IUserBusiness _userGroupBusiness;
        private IUserGroupUserBusiness _userGroupUserBusiness;
        private readonly IRepositoryQueryBase<TagCategoryViewModel> _querytagcategory;
        private readonly IRepositoryQueryBase<TagViewModel> _querytag;

        public UserGroupBusiness(IRepositoryBase<UserGroupViewModel, UserGroup> repo, IMapper autoMapper, IRepositoryQueryBase<UserGroupViewModel> queryRepo, IRepositoryQueryBase<TagCategoryViewModel> querytagcategory, IRepositoryQueryBase<TagViewModel> querytag, IUserBusiness userGroupBusiness, IUserGroupUserBusiness userGroupUserBusiness) : base(repo, autoMapper)
        {
            _userGroupBusiness = userGroupBusiness;
            _userGroupUserBusiness = userGroupUserBusiness;
            _querytagcategory = querytagcategory;
            _querytag = querytag;
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<UserGroupViewModel>> Create(UserGroupViewModel model)
        {

           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           var validateName = await IsNameExists(model);
           if (!validateName.IsSuccess)
           {
               return CommandResult<UserGroupViewModel>.Instance(model, false, validateName.Messages);
           }
           var result = await base.Create(model);
           if (model.UserIds != null && model.UserIds.Count() > 0)
           {
               foreach (var id in model.UserIds)
               {
                   var user = new UserGroupUserViewModel();
                   user.UserGroupId = result.Item.Id;
                   user.UserId = id;
                   await _userGroupUserBusiness.Create(user);
       
               }
           }

            return CommandResult<UserGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserGroupViewModel>> Edit(UserGroupViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserGroupViewModel>.Instance(model, false, validateName.Messages);
            }

            var result = await base.Edit(model);
            var pagecol = await _userGroupUserBusiness.GetList(x => x.UserGroupId == model.Id);
            var existingIds = pagecol.Select(x => x.UserId);
            var newIds = model.UserIds;
            var ToDelete = new List<string>();
            var ToAdd = new List<string>();
            if (existingIds.IsNotNull() && existingIds.Count() > 0)
            {
                ToDelete = existingIds.Except(newIds).ToList();
            }
            if (newIds.IsNotNull() && newIds.Count()>0)
            {
                ToAdd = newIds.Except(existingIds).ToList();
            }
           
            // Add
            foreach (var id in ToAdd)
            {
                var user = new UserGroupUserViewModel();
                user.UserGroupId = result.Item.Id;
                user.UserId = id;
                await _userGroupUserBusiness.Create(user);
            }
            // Delete
            foreach (var id in ToDelete)
            {
                var role = await _userGroupUserBusiness.GetSingle(x => x.UserGroupId == model.Id && x.UserId == id);
                await _userGroupUserBusiness.Delete(role.Id);
            }

            return CommandResult<UserGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<CommandResult<UserGroupViewModel>> CreateFromPortal(UserGroupViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserGroupViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);
            if (model.UserIds != null && model.UserIds.Count() > 0)
            {
                foreach (var id in model.UserIds)
                {
                    var user = new UserGroupUserViewModel();
                    user.UserGroupId = result.Item.Id;
                    user.UserId = id;
                    user.PortalId = _repo.UserContext.PortalId;
                    user.LegalEntityId = _repo.UserContext.LegalEntityId;
                    await _userGroupUserBusiness.Create(user);

                }
            }

            return CommandResult<UserGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<UserGroupViewModel>> EditFromPortal(UserGroupViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserGroupViewModel>.Instance(model, false, validateName.Messages);
            }

            var result = await base.Edit(model);
            var pagecol = await _userGroupUserBusiness.GetList(x => x.UserGroupId == model.Id);
            var existingIds = pagecol.Select(x => x.UserId);
            var newIds = model.UserIds;
            var ToDelete = existingIds.Except(newIds).ToList();
            var ToAdd = newIds.Except(existingIds).ToList();
            // Add
            foreach (var id in ToAdd)
            {
                var user = new UserGroupUserViewModel();
                user.UserGroupId = result.Item.Id;
                user.UserId = id;
                user.PortalId = _repo.UserContext.PortalId;
                user.LegalEntityId = _repo.UserContext.LegalEntityId;
                await _userGroupUserBusiness.Create(user);
            }
            // Delete
            foreach (var id in ToDelete)
            {
                var role = await _userGroupUserBusiness.GetSingle(x => x.UserGroupId == model.Id && x.UserId == id);
                await _userGroupUserBusiness.Delete(role.Id);
            }

            return CommandResult<UserGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<UserGroupViewModel>> IsNameExists(UserGroupViewModel viewModel)
        {

            Dictionary<string, string> obj = new Dictionary<string, string>();
            if (viewModel.Name.IsNullOrEmpty())
            {
                obj.Add("Name", "User Group Name is required.");
            }
            var pagelist = await GetList(x => x.Name == viewModel.Name);
            if (pagelist.Exists(x => x.Id != viewModel.Id && x.Name.ToLower() == viewModel.Name.ToLower()))
            {

                obj.Add("NameExist", "The user group already exists. Please choose another user group");

            }
            var Codeexist = await GetList(x => x.Code == viewModel.Code);
            if (Codeexist.Exists(x => x.Id != viewModel.Id && x.Code.ToLower() == viewModel.Code.ToLower()))
            {
                obj.Add("CodeExist", "The user group code already exists. Please choose another user group code");
            }

            if (obj.Count > 0)
            {
                return CommandResult<UserGroupViewModel>.Instance(viewModel, false, obj);
            }
            return CommandResult<UserGroupViewModel>.Instance();
        }

        public async Task<List<TagCategoryViewModel>> GetAllTagCategory()
        {
            var query = $@"SELECT Ns.""TagCategoryType"", Ns.""TagCategoryCode"", Ns.""TagCategoryName"", 
         Ns.""EnableAutoTag"", Ns""TagSourceId"", N.""Id""

    FROM cms.""N_General_TagCategory"" as Ns   inner join public.""NtsNote"" N on Ns.""NtsNoteId""=N.""Id"" and NS.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""CompanyId""='{_repo.UserContext.CompanyId}' and NS.""IsDeleted""=false and N.""IsDeleted""=false";

            var queryData = await _querytagcategory.ExecuteQueryList<TagCategoryViewModel>(query, null);




            //var list = new List<TagCategoryViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new TagCategoryViewModel { TagCategoryName = x.TagCategoryName, TagCategoryCode = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return queryData;
        }

        public async Task<TagCategoryViewModel> GetTagCategoryDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_General_TagCategory"" as TC inner join public.""NtsNote"" as N on TC.""NtsNoteId""=N.""Id"" and TC.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""CompanyId""='{_repo.UserContext.CompanyId}' where TC.""Id""='{Id}' and  TC.""IsDeleted""=false and  N.""IsDeleted""=false ";

            var queryData = await _querytagcategory.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllSourceID()
        {
            var query = $@"SELECT ""Id"",  ""DisplayName"" as Name  FROM public.""TableMetadata"" where  ""IsDeleted""='false' and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _querytagcategory.ExecuteQueryList<IdNameViewModel>(query, null);

           var list = new List<IdNameViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return queryData;
        }

        public async Task DeleteTagCategory(string Id)
        {
            var query = $@"Update cms.""N_General_TagCategory"" set ""IsDeleted""='True' where ""Id""='{Id}'";
            await _querytagcategory.ExecuteCommand(query, null);
        }

        public async Task DeleteTag(string Id)
        {
            var query = $@"Update cms.""N_General_Tag"" set ""IsDeleted""='True' where ""Id""='{Id}'";
            await _querytagcategory.ExecuteCommand(query, null);
        }

        public async Task<TagCategoryViewModel> IsTagCategoryNameExist(string TagCategoryName, string Id)
        {
            var query = $@"Select * from cms.""N_General_TagCategory"" where ""TagCategoryName""='{TagCategoryName}' and ""IsDeleted""='false' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and ""NtsNoteId"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _querytagcategory.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<TagCategoryViewModel> IsTagCategoryCodeExist(string TagCategoryCode, string Id)
        {
            var query = $@"Select * from cms.""N_General_TagCategory"" where ""TagCategoryCode""='{TagCategoryCode}' and ""IsDeleted""='false' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and ""NtsNoteId"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _querytagcategory.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<TagCategoryViewModel> IsParentAssignTosourceTagExist(string ParentId, string TagSourceId,string Id)
        {
            var query = $@"select * from cms.""N_General_TagCategory"" As TC
                           inner join public.""NtsNote"" as N on TC.""NtsNoteId""=N.""Id"" and N .""IsDeleted""='false'
	         where N.""ParentNoteId""='{Id}' and TC.""NtsNoteId""='{ParentId}'  and TC .""IsDeleted""='false'";

           // var where = "";
           // if (Id.IsNotNullAndNotEmpty())
           // {
           //     where = $@" and N.""Id"" !='{Id}' ";
           // }
           // query = query.Replace("#IdWhere#", where);
            var queryData = await _querytagcategory.ExecuteQuerySingle(query, null);
            return queryData;
        }



        public async Task<List<IdNameViewModel>> GetParentTagCategory()
        {
            var query = $@"select ""NtsNoteId"" as Id,""TagCategoryName"" as Name from   cms.""N_General_TagCategory"" where  ""IsDeleted""='false' and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _querytagcategory.ExecuteQueryList<IdNameViewModel>(query, null);

            var list = new List<IdNameViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return queryData;
        }


        //Tag


        public async Task<List<TagViewModel>> GetTagList(string CategoryId)
        {
            var query = $@"Select  T.*,T.""Id"" as Id,N.""NoteSubject"" from cms.""N_General_Tag"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and T.""CompanyId""='{_repo.UserContext.CompanyId}'
            where N.""ParentNoteId"" = '{CategoryId}'  and T.""IsDeleted"" = false and N.""IsDeleted"" = false";

            var queryData = await _querytag.ExecuteQueryList<TagViewModel>(query, null);
            return queryData;
        }


        public async Task<TagViewModel> GetTagEdit(string NoteId)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_General_Tag"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and T.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""CompanyId""='{_repo.UserContext.CompanyId}'
            where T.""Id"" = '{NoteId}' and T.""IsDeleted"" = 'false'";

            var queryData = await _querytag.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<TagViewModel> GetNoteId(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_General_TagCategory"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" 
            where T.""Id"" = '{Id}' and T.""IsDeleted"" = 'false'";

            var queryData = await _querytag.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<TagViewModel> IsTagNameExist(string Parentid, string TagName, string Id)
        {
            var query = $@"Select  T.""Id"" as Id,N.""NoteSubject"" from cms.""N_General_Tag"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" 
            where N.""ParentNoteId"" = '{Parentid}' and  N.""NoteSubject""='{TagName}'  and T.""IsDeleted"" = 'false' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and ""NtsNoteId"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _querytag.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<List<UserGroupViewModel>> GetTeamWithPortalIds()
        {

            var Query = $@"SELECT distinct u.* FROM public.""UserGroup"" as u 
            join public.""Company"" as c on c.""Id""='{_repo.UserContext.CompanyId}' and array[u.""AllowedPortalIds""] <@ array[c.""LicensedPortalIds""] and c.""IsDeleted""=false
            where u.""IsDeleted""=false and u.""AllowedPortalIds"" is not null";
            var list = await _queryRepo.ExecuteQueryList<UserGroupViewModel>(Query, null);
            return list;

        }


    }
}
