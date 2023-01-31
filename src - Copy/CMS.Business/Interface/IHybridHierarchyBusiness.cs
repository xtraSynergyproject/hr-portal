using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IHybridHierarchyBusiness : IBusinessBase<HybridHierarchyViewModel, HybridHierarchy>
    {
        Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyChildList(string parentId, int level, int levelupto, bool enableAOR, string bulkRequestId, bool includeParent);
        Task RemoveFromBusinessHierarchy(string id);
        Task<IList<TaskViewModel>> GetBHServiceData(bool showAllService);
        Task<IList<TaskViewModel>> GetBHTaskData();
        Task<List<BusinessHierarchyPermissionViewModel>> GetBusinessHierarchyPermissionData(string groupCode);
        Task<bool> DeleteBusinessHierarchyPermission(BusinessHierarchyPermissionViewModel model);
        Task<MemoryStream> DownloadHybridHierarchy();
        Task<List<HybridHierarchyViewModel>> GetHourReportProjectData();

        Task<MemoryStream> DownloadAORdata(List<BusinessHierarchyAORViewModel> aorList);
        Task<MemoryStream> DownloadBusinessPartnerMappingExcel(List<BusinessHierarchyAORViewModel> mappingList);
        Task<List<string>> GetHierarchyPath(string hierarchyItemId);
        Task UpdateHierarchyPath(HybridHierarchyViewModel hybridmodel);
        Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyList(string referenceType = null, string searckKey = null, bool bindPath = false);
        //Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyDetails();
        Task<List<UserHierarchyPermissionViewModel>> GetBusinessHierarchyRootPermission(string UserId = null, string PermissionId = null);
        Task MoveItemToNewParent(string cureNodeId, string newParentId);
        Task<List<HybridHierarchyViewModel>> GetHierarchyParentDetails(string hierarchyItemId);

        Task<List<BusinessHierarchyAORViewModel>> GetAllAORBusinessHierarchyList();
    }
}
