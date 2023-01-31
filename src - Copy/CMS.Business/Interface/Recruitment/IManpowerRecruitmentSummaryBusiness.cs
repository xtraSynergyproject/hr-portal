using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IManpowerRecruitmentSummaryBusiness : IBusinessBase<ManpowerRecruitmentSummaryViewModel,ManpowerRecruitmentSummary>
    {
        Task<IList<ManpowerRecruitmentSummaryVersionViewModel>> GetManpowerRecruitmentSummaryVersionData(string id);
        Task<IList<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentSummaryData();
        Task<IList<ManpowerSummaryCommentViewModel>> GetManpowerSummaryCommentData(string id,string userRoleCode);
        Task<IList<IdNameViewModel>> GetJobIdNameList();
        Task<IList<IdNameViewModel>> GetOrganizationIdNameList();
        Task<ManpowerRecruitmentSummaryViewModel> GetManpowerRecruitmentSummaryCalculatedData(string id);
        Task<JobAdvertisementViewModel> GetRecruitmentDashobardCount(string orgId);
        Task<RecruitmentDashboardViewModel> GetManpowerRecruitmentSummaryByOrgJob(string organizationId, string jobId);

        Task<IList<JobAdvertisementViewModel>> GetManpowerUniqueJobData();

        Task<JobAdvertisementViewModel> GetState(string Id);
        Task UpdateManpowerRecruitmentSummaryForAvailable(string applicationId);

        Task<DataTable> GetTaskByOrgUnit(string userId,string userRoleId);
        Task<DataTable> GetJobByOrgUnit(string userId);

        Task<IList<RecTaskViewModel>> GetJobDescriptionTaskList(string manpowerId);
    }
}
