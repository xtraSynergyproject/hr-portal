using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IHRCoreQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<OrganizationChartIndexViewModel> GetOrgHierarchyParentId(string personId);
        Task<List<OrganizationChartViewModel>> GetOrgHierarchy(string parentId, int levelUpto);
        Task<PositionChartIndexViewModel> GetPostionHierarchyParentId(string personId);
        Task<List<PositionChartViewModel>> GetPositionHierarchy(string parentId, int levelUpto);
        Task<List<AssignmentViewModel>> GetEmployeeDirectoryData();
        Task<List<EmployeeTransferItemViewModel>> GetEmployeeTransferItemList(string employeeId);
        Task<List<EmployeeTransferItemViewModel>> GeEmployeeFormTransferItemList(string employeeId, string transferId);
    }
}
