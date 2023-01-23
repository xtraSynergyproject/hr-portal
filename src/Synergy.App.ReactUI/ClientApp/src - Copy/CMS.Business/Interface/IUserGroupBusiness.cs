using System;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;

namespace CMS.Business
{
      public  interface IUserGroupBusiness : IBusinessBase<UserGroupViewModel, UserGroup>
    {
          Task<List<TagCategoryViewModel>> GetAllTagCategory();
        Task<TagCategoryViewModel> GetTagCategoryDetails(string Id);

         Task<List<IdNameViewModel>> GetAllSourceID();
        Task DeleteTagCategory(string Id);
        Task DeleteTag(string Id);
        Task<TagCategoryViewModel> IsTagCategoryNameExist(string TagCategoryName, string Id);
        Task<TagCategoryViewModel> IsTagCategoryCodeExist(string TagCategoryCode, string Id);
        Task <List<IdNameViewModel>> GetParentTagCategory();

        Task<TagCategoryViewModel> IsParentAssignTosourceTagExist(string ParentId, string TagSourceId, string Id);

        Task<CommandResult<UserGroupViewModel>> CreateFromPortal(UserGroupViewModel model);
        Task<CommandResult<UserGroupViewModel>> EditFromPortal(UserGroupViewModel model);
        Task<List<TagViewModel>> GetTagList(string CategoryId);

        Task<TagViewModel> GetTagEdit(string NoteId);
        Task<List<UserGroupViewModel>> GetTeamWithPortalIds();
        Task<TagViewModel> IsTagNameExist(string Parentid, string TagName, string Id);

    }
}
