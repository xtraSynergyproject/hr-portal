using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class UserGroupBusiness : BusinessBase<UserGroupViewModel, UserGroup>, IUserGroupBusiness
    {
        private readonly IRepositoryQueryBase<UserGroupViewModel> _queryRepo;
        private IUserBusiness _userGroupBusiness;
        private IUserGroupUserBusiness _userGroupUserBusiness;
        private readonly IRepositoryQueryBase<TagCategoryViewModel> _querytagcategory;
        private readonly IRepositoryQueryBase<TagViewModel> _querytag;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public UserGroupBusiness(IRepositoryBase<UserGroupViewModel, UserGroup> repo, IMapper autoMapper, IRepositoryQueryBase<UserGroupViewModel> queryRepo, IRepositoryQueryBase<TagCategoryViewModel> querytagcategory, IRepositoryQueryBase<TagViewModel> querytag, IUserBusiness userGroupBusiness, ICmsQueryBusiness cmsQueryBusiness, IUserGroupUserBusiness userGroupUserBusiness) : base(repo, autoMapper)
        {
            _userGroupBusiness = userGroupBusiness;
            _userGroupUserBusiness = userGroupUserBusiness;
            _querytagcategory = querytagcategory;
            _querytag = querytag;
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<UserGroupViewModel>> Create(UserGroupViewModel model, bool autoCommit = true)
        {

           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           var validateName = await IsNameExists(model);
           if (!validateName.IsSuccess)
           {
               return CommandResult<UserGroupViewModel>.Instance(model, false, validateName.Messages);
           }
           var result = await base.Create(model,autoCommit);
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

        public async override Task<CommandResult<UserGroupViewModel>> Edit(UserGroupViewModel model, bool autoCommit = true)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserGroupViewModel>.Instance(model, false, validateName.Messages);
            }

            var result = await base.Edit(model,autoCommit);
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
        public async Task<CommandResult<UserGroupViewModel>> CreateFromPortal(UserGroupViewModel model, bool autoCommit = true)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserGroupViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model,autoCommit);
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

        public async Task<CommandResult<UserGroupViewModel>> EditFromPortal(UserGroupViewModel model, bool autoCommit = true)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<UserGroupViewModel>.Instance(model, false, validateName.Messages);
            }

            var result = await base.Edit(model,autoCommit);
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

            var queryData = await _cmsQueryBusiness.GetAllTagCategory();
            //var list = new List<TagCategoryViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new TagCategoryViewModel { TagCategoryName = x.TagCategoryName, TagCategoryCode = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return queryData;
        }

        public async Task<TagCategoryViewModel> GetTagCategoryDetails(string Id)
        {
            var queryData = await _cmsQueryBusiness.GetTagCategoryDetails(Id);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllSourceID()
        {
            var queryData = await _cmsQueryBusiness.GetAllSourceID();

           var list = new List<IdNameViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return queryData;
        }

        public async Task DeleteTagCategory(string Id)
        {
            await _cmsQueryBusiness.DeleteTagCategory(Id);
        }

        public async Task DeleteTag(string Id)
        {
            await _cmsQueryBusiness.DeleteTag(Id);
        }

        public async Task<TagCategoryViewModel> IsTagCategoryNameExist(string TagCategoryName, string Id)
        {

            var queryData = await _cmsQueryBusiness.IsTagCategoryNameExist(TagCategoryName, Id);
            return queryData;
        }


        public async Task<TagCategoryViewModel> IsTagCategoryCodeExist(string TagCategoryCode, string Id)
        {

            var queryData = await _cmsQueryBusiness.IsTagCategoryCodeExist(TagCategoryCode,Id);
            return queryData;
        }


        public async Task<TagCategoryViewModel> IsParentAssignTosourceTagExist(string ParentId, string TagSourceId,string Id)
        {

            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _cmsQueryBusiness.IsParentAssignTosourceTagExist(ParentId,TagSourceId,Id);
            return queryData;
        }



        public async Task<List<IdNameViewModel>> GetParentTagCategory()
        {

            var queryData = await _cmsQueryBusiness.GetParentTagCategory();

            var list = new List<IdNameViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return queryData;
        }


        //Tag


        public async Task<List<TagViewModel>> GetTagList(string CategoryId)
        {


            var queryData = await _cmsQueryBusiness.GetTagList(CategoryId);
            return queryData;
        }


        public async Task<TagViewModel> GetTagEdit(string NoteId)
        {

            var queryData = await _cmsQueryBusiness.GetTagEdit(NoteId);
            return queryData;
        }


        public async Task<TagViewModel> GetNoteId(string Id)
        {


            var queryData = await _cmsQueryBusiness.GetNoteId(Id);
            return queryData;
        }

        public async Task<TagViewModel> IsTagNameExist(string Parentid, string TagName, string Id)
        {

            var queryData = await _cmsQueryBusiness.IsTagNameExist(Parentid, TagName, Id);
            return queryData;
        }
        public async Task<List<UserGroupViewModel>> GetTeamWithPortalIds()
        {

            var list = await _cmsQueryBusiness.GetTeamWithPortalIds();
            return list;

        }


    }
}
