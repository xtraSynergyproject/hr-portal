using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IWorkboardBusiness
    {
        Task<List<WorkBoardViewModel>> GetWorkboardList(WorkBoardstatusEnum status,string id = null);
        Task<bool> UpdateWorkBoardStatus(string id, WorkBoardstatusEnum status);
        Task UpdateWorkBoardJson(WorkBoardViewModel data);
        Task DeleteItem(string itemId);
        Task DeleteSection(string itemId);
        Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemByParentId(string parentId);
        Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemBySectionId(string sectionId);
        Task<WorkBoardItemViewModel> GetWorkBoardItemDetails(string itemId);
        Task UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data);
        Task UpdateWorkBoardSectionSequenceOrder(WorkBoardSectionViewModel data);
        Task UpdateWorkBoardItemSequenceOrder(WorkBoardItemViewModel data);
        Task UpdateWorkBoardItemDetails(WorkBoardItemViewModel data);
        Task<WorkBoardSectionViewModel> GetWorkBoardSectionDetails(string sectionId);
        Task<WorkBoardViewModel> GetWorkBoardDetails(string workBoradId);
        Task<List<WorkBoardInviteDetailsViewModel>> GetWorkBoardInvites(string workBoradId);
        Task<List<WorkBoardTemplateViewModel>> GetTemplateList();
        Task<List<LOVViewModel>> GetTemplateCategoryList();
        Task<string> GetJsonContent(string templateTypeId, string workBoardId, DateTime? date = null, string templateTypeCode = null, string workboardItemId = null, WorkBoardOwnerTypeEnum? ownerType=null);
       
        Task<List<WorkBoardSectionViewModel>> GetSectionContent(string templateTypeId, string workBoardId, DateTime? date = null, string templateTypeCode = null, string workboardItemId = null, WorkBoardOwnerTypeEnum? ownerType=null);

        Task<WorkBoardTemplateViewModel> GetWorkBoardTemplateById(string templateTypeId);
        Task<List<WorkBoardTemplateViewModel>> GetSearchResults(string[] values);
        //Task<List<WorkBoardViewModel>> GetOtherWorkboardList(WorkBoardstatusEnum status, string id);
        Task<List<WorkBoardSectionViewModel>> GetWorkboardSectionList(string id);
        Task<WorkBoardItemViewModel> GetWorkBoardItemByNtsNoteId(string ntsNoteId);
        Task<WorkBoardItemViewModel> GetWorkboardItemById(string id);
        Task<bool> UpdateWorkboardItem(string workboardId, string sectionId, string workboardItemId);
        Task<List<WorkBoardSectionViewModel>> GenerateDummyWorkBoardSections(int NoOfItems);
        Task<List<WorkBoardViewModel>> GetWorkboardTaskList();

        Task<IList<UserViewModel>> GetUserList(string noteId);

        Task<List<WorkBoardViewModel>> GetSharedWorkboardList(WorkBoardstatusEnum status, string sharedWithUserId);

        Task<WorkBoardViewModel> GetWorkBoardDetailsByIdKey(string workBoardUniqueId, string shareKey);

        Task<string> GetWorkBoardSectionForIndex(WorkBoardItemViewModel item);
        Task<WorkBoardItemViewModel> GetWorkboardItemByNoteId(string id);

        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionListByWorkbBoardId(string workboardId);

        Task<List<WorkBoardItemViewModel>> GetItemBySectionId(string sectionId);
        Task<WorkBoardViewModel> GetWorkBoardDetail(string workBoradId);


    }
}
