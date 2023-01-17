using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IRecTaskBusiness : IBusinessBase<RecTaskViewModel,RecTask>
    {        
        Task<RecTaskViewModel> GetTaskDetails(RecTaskViewModel model);
        Task<List<RecTaskViewModel>> GetTaskDetailsList(string ids, string templateCode, string userId);
        Task<IList<RecTaskViewModel>> GetActiveListByUserId(string userId);
        Task<IList<RecTaskViewModel>> GetTaskByTemplateCode(string tempCode);
        Task<IList<RecTaskViewModel>> GetActiveServiceListByUserId(string userId);
        Task<string> GetTaskIdsByUserId(string userId,string templateCode, string status, string batch);
        Task<IList<RecTaskViewModel>> GetActiveStepTaskListByService(string serviceId,string versionNo="");        
        Task<IList<RecTaskViewModel>> GetStepTaskListByService(string serviceId);
        Task<string> GetServiceId(string steptaskId);
        Task<IList<RecTaskViewModel>> GetStepTaskId(string serviceId);
        Task<CommandResult<RecTaskViewModel>> AssignTaskForJobAdvertisement(string referenceId,string jobname,DateTime createddate, string assignTo);
        Task<string> UpdateOverdueTaskAndServiceStatus(DateTime currentDate);
        Task<string> UpdateTaskBatchId(string taskid, string batchId);
        Task<string> UpdateTaskCandidateId(string taskid, string candidateId);
        Task<string> UpdateServiceReference(string serviceId, string appId);
        Task<CommandResult<RecTaskViewModel>> Createversion(RecTaskViewModel model);
        Task<IList<IdNameViewModel>> GetVersionList(string id, long? versionId = null);
        Task<IList<System.Dynamic.ExpandoObject>> GetPendingTaskDetailsForUser(string userId, string orgId, string userRoleCodes);
        Task<double> GetPendingTaskCount(string userId, string userRoleCodes);
        Task<RecTaskViewModel> GetTemplateDetails(string templateCode);
        Task<IList<TreeViewViewModel>> GetBulkApprovalMenuItem(string id, string type, string parentId,string userRoleId, string userId,string userRoleIds,string stageName,string stageId,string batchId, string expandingList,string userroleCode);
        Task<TaskEmailSummaryViewModel> GetTaskSummaryCountByUserId(string userId);
        Task<List<RecTaskViewModel>> GetTasksTemplateCodeByUserId(string userId);
        Task<List<RecTaskViewModel>> GetTaskDetailsSummaryList(string ids, string templateCode, string userId);
    }
}
