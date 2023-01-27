using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;
namespace CMS.Business
{
    public interface IJobAdvertisementBusiness : IBusinessBase<JobAdvertisementViewModel, JobAdvertisement>
    {
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameList();
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameListForSelection();
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameListByOrg(string organizationId);
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameByOrgIdList(string OrgId);
        Task<JobAdvertisementViewModel> GetNameById(string jobAdvId);
        Task<IdNameViewModel> GetJobStatus(string jobId);
        Task<IList<JobAdvertisementViewModel>> GetJobAdvertisementList(string keyWord, string categoryId, string locationId, string manpowerTypeId,string agencyId);
        Task<IList<ListOfValueViewModel>> GetJobAdvertisementListWithCount(string agencyId);
        Task<JobAdvertisementViewModel> GetJobIdNameListByJobAdvertisement(string jobAdvertisementId);
        Task<IList<JobCriteriaViewModel>> GetJobCriteriaList(string type, string jobadvtid);
        Task<IdNameViewModel> GetJobManpowerType(string Id);
        Task<IList<JobAdvertisementViewModel>> GetJobAdvertisement(string jobid, string rolid, StatusEnum status);
        Task<JobAdvertisementViewModel> GetCalculatedData(string id);
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameDashboardList();
        Task UpdateJobAdvertisementStatus();
        Task<List<ApplicationViewModel>> GetBookmarksJobList(string jobIds);
        Task<List<ApplicationViewModel>> GetJobAdvertisementByAgency();
    }
}
