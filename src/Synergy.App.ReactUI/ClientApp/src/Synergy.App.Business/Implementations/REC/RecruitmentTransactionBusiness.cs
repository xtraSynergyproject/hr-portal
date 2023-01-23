using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Synergy.App.Business
{
    public class RecruitmentTransactionBusiness : BusinessBase<ServiceViewModel, NtsService>, IRecruitmentTransactionBusiness
    {
        private readonly IRepositoryQueryBase<ServiceViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<JobAdvertisementViewModel> _queryRepoJob;
        private readonly IRecruitmentQueryBusiness _recruitmentQueryBusiness;
        private readonly IRecQueryBusiness _recQueryBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ILOVBusiness _lovBusiness;
        
        IUserContext _userContext;
        public RecruitmentTransactionBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IMapper autoMapper,
            IRepositoryQueryBase<ServiceViewModel> queryRepo,
            IRepositoryQueryBase<JobAdvertisementViewModel> queryRepoJob,
            IRecQueryBusiness recQueryBusiness, INoteBusiness noteBusiness,
            IRecruitmentQueryBusiness recruitmentQueryBusiness, IServiceBusiness serviceBusiness,
            ILOVBusiness lovBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepoJob = queryRepoJob;
            _recruitmentQueryBusiness = recruitmentQueryBusiness;
            _noteBusiness = noteBusiness;
            _recQueryBusiness = recQueryBusiness;
            _serviceBusiness = serviceBusiness;
            _lovBusiness = lovBusiness;
           
        }
        public async Task<IList<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userroleCode)
        {
            return await _recQueryBusiness.GetInboxTreeviewList(id, type, parentId, userRoleId, userId, userRoleIds, stageName, stageId, batchId, expandingList, userroleCode);
        }

        public async Task<IList<JobAdvertisementViewModel>> GetManpowerUniqueJobData()
        {
            return await _recQueryBusiness.GetManpowerUniqueJobData();
        }

        public async Task<IList<RecTaskViewModel>> GetPendingTaskListByUserId(string userId)
        {
            return await _recQueryBusiness.GetPendingTaskListByUserId(userId);
        }
        public async Task<DataTable> GetJobByOrgUnit(string userId)
        {
            return await _recQueryBusiness.GetJobByOrgUnit(userId);
        }
        public async Task<IList<IdNameViewModel>> GetIdNameList(string type)
        {
            return await _recQueryBusiness.GetIdNameList(type);
        }
        public async Task<IList<IdNameViewModel>> GetCountryIdNameList()
        {
            return await _recQueryBusiness.GetCountryIdNameList();
        }
        public async Task<DataTable> GetTaskByOrgUnit(string userId, string userroleId)
        {
            return await _recQueryBusiness.GetTaskByOrgUnit(userId, userroleId);
        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameDashboardList()
        {
            return await _recQueryBusiness.GetJobIdNameDashboardList();
        }
        public async Task<List<IdNameViewModel>> GetOrgByJobAddId(string JobId)
        {
            return await _recQueryBusiness.GetOrgByJobAddId(JobId);
        }
        public async Task<IdNameViewModel> GetHmOrg(string userId)
        {
            return await _recQueryBusiness.GetHmOrg(userId);
        }
        public async Task<IdNameViewModel> GetHODOrg(string userId)
        {
            return await _recQueryBusiness.GetHODOrg(userId);
        }
        public async Task<JobAdvertisementViewModel> GetRecruitmentDashobardCount(string orgId)
        {
            return await _recQueryBusiness.GetRecruitmentDashobardCount(orgId);
        }
        public async Task<RecruitmentDashboardViewModel> GetManpowerRecruitmentSummaryByOrgJob(string organizationId, string jobId, string permission = "")
        {
            return await _recQueryBusiness.GetManpowerRecruitmentSummaryByOrgJob(organizationId, jobId, permission);
        }
        public async Task<IList<ApplicationViewModel>> GetApplicationPendingTask(string userId)
        {
            return await _recQueryBusiness.GetApplicationPendingTask(userId);
        }
        public async Task<IList<ApplicationViewModel>> GetApplicationWorkerPoolNotUnderApproval()
        {
            return await _recQueryBusiness.GetApplicationWorkerPoolNotUnderApproval();
        }
        public async Task<IdNameViewModel> GetJobNameById(string jobId)
        {
            return await _recQueryBusiness.GetJobNameById(jobId);
        }
        public async Task<List<IdNameViewModel>> GetJobIdNameDataList()
        {
            var list = await _recQueryBusiness.GetJobIdNameDataList();
            return list;
        }
        public async Task<IdNameViewModel> GetApplicationStateByCode(string stateCode)
        {
            return await _recQueryBusiness.GetApplicationStateByCode(stateCode);
        }
        public async Task<IList<System.Dynamic.ExpandoObject>> GetPendingTaskDetailsForUser(string userId, string orgId, string userRoleCodes)
        {
            return await _recQueryBusiness.GetPendingTaskDetailsForUser(userId, orgId, userRoleCodes);
        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobAdvertisement(string jobid, string rolid, StatusEnum status)
        {
            return await _recruitmentQueryBusiness.GetJobAdvertisement(jobid, rolid, status);
        }

        public async Task<IList<JobDescriptionCriteriaViewModel>> GetJobDescCriteriaList(string type, string jobdescid)
        {
            return await _recQueryBusiness.GetJobDescCriteriaList(type, jobdescid);
        }

        public async Task<JobDescriptionViewModel> GetJobDescriptionData(string jobId)
        {
            return await _recQueryBusiness.GetJobDescriptionData(jobId);
        }
        public async Task<IList<JobCriteriaViewModel>> GetJobCriteriaList(string type, string jobadvtid)
        {
            return await _recQueryBusiness.GetJobCriteriaList(type, jobadvtid);
        }

        public async Task<JobAdvertisementViewModel> GetJobIdNameListByJobAdvertisement(string JobId)
        {
            return await _recQueryBusiness.GetJobIdNameListByJobAdvertisement(JobId);
        }

        public async Task<JobAdvertisementViewModel> GetJobAdvertisementData(string id)
        {
            return await _recQueryBusiness.GetJobAdvertisementData(id);
        }
        public async Task<List<RecBatchViewModel>> GetBatchData(string jobid, BatchTypeEnum type, string orgId)
        {
            return await _recQueryBusiness.GetBatchData(jobid, type, orgId);
        }
        public async Task<RecBatchViewModel> GetBatchDataById(string batchId)
        {
            return await _recQueryBusiness.GetBatchDataById(batchId);
        }
        public async Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search)
        {
            return await _recQueryBusiness.GetCandiadteShortListDataByHR(search);
        }
        public async Task<List<ApplicationStateCommentViewModel>> GetApplicationStateCommentData(string appId, string appStateId)
        {
            return await _recQueryBusiness.GetApplicationStateCommentData(appId,appStateId);
        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameListForSelection()
        {
            return await _recQueryBusiness.GetJobIdNameListForSelection();
        }
        public async Task<IList<IdNameViewModel>> GetBatchIdNameList(string JobAddId, BatchTypeEnum batchType, string orgId)
        {
            return await _recQueryBusiness.GetBatchIdNameList(JobAddId, batchType, orgId);
        }
        public async Task<List<ManpowerRecruitmentSummaryViewModel>> GetBatchDataByJobId(string jobId)
        {
            return await _recQueryBusiness.GetBatchDataByJobId(jobId);
        }
        public async Task<List<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentList(string jobId, string orgId)
        {
            return await _recQueryBusiness.GetManpowerRecruitmentList(jobId, orgId);
        }
        public async Task<string> GenerateNextBatchNameUsingOrg(string Name)
        {
            var id = 0;
            var date = DateTime.Now.Date;
            var batchList = await _recQueryBusiness.GenerateNextBatchNo(Name);
            if (batchList.Count > 0)
            {
                for (var i = 0; i < batchList.Count; i++)
                {
                    var lastNo = batchList[i].BatchName.Split('_')[^1];
                    lastNo = lastNo.Replace("-", string.Empty);
                    var lastNo1 = Convert.ToInt32(lastNo);
                    if (id < lastNo1)
                    {
                        id = lastNo1;
                    }
                }
                return string.Concat(Name, "-", ++id);
            }
            else
            {
                return string.Concat(Name, "-", id);
            }
        }

        public async Task<IList<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentSummaryData()
        {
            return await _recQueryBusiness.GetManpowerRecruitmentSummaryData();
        }
        public async Task<JobAdvertisementViewModel> GetState(string serId)
        {
            return await _recQueryBusiness.GetState(serId);
        }
        public async Task<IList<ApplicationViewModel>> GetViewApplicationDetails(string jobId, string orgId)
        {
            return await _recQueryBusiness.GetViewApplicationDetails(jobId, orgId);
        }

        public async Task<List<RecHeadOfDepartmentViewModel>> GetHODListByOrgId(string orgId)
        {
            return await _recQueryBusiness.GetHODListByOrgId(orgId);
        }
        public async Task<List<RecHiringManagerViewModel>> GetHMListByOrgId(string orgId)
        {
            return await _recQueryBusiness.GetHMListByOrgId(orgId);
        }
        public async Task UpdateApplicationtStatus(string users, string type)
        {
            await _recQueryBusiness.UpdateApplicationtStatus(users, type);
        }
        public async Task<RecBatchViewModel> GetBatchApplicantCount(string Id)
        {
           return await _recQueryBusiness.GetBatchApplicantCount(Id);
        }
        public async Task UpdateStatus(string batchId, string code)
        {
            await _recQueryBusiness.UpdateStatus(batchId,code);
        }

        public async Task<IList<IdNameViewModel>> GetGradeIdNameList(string code=null)
        {
            return await _recQueryBusiness.GetGradeIdNameList(code);
        }

        public async Task<IList<RecCandidateElementInfoViewModel>> GetElementData(string appid)
        {
            return await _recQueryBusiness.GetElementData(appid);
        }

        public async Task<IList<RecCandidatePayElementViewModel>> GetElementPayData(string appid)
        {
            return await _recQueryBusiness.GetPayElementData(appid);
        }

        public async Task<RecApplicationViewModel> GetApplicationDetailsById(string appId)
        {
            var data = await _recQueryBusiness.GetApplicationDetailsById(appId);
            return data;
        }

        public async Task<RecApplicationViewModel> GetAppointmentDetailsById(string appId)
        {
            var data = await _recQueryBusiness.GetAppointmentDetailsById(appId);
            return data;
        }

        public async Task<CandidateProfileViewModel> GetApplicationforOfferById(string appId)
        {
            return await _recQueryBusiness.GetApplicationforOfferById(appId);
        }

        public async Task<long> GenerateFinalOfferSeq()
        {
            return await _recQueryBusiness.GenerateFinalOfferSeq();
        }

        public async Task<IList<RecApplicationViewModel>> GetTotalApplication(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode)
        {
            return await _recQueryBusiness.GetTotalApplication(jobid, orgId, jobadvtstate, tempcode, nexttempcode, visatempcode);
        }

        public async Task<RecDashboardViewModel> GetManpowerRequirement(string jobId, string orgId)
        {
            return await _recQueryBusiness.GetManpowerRequirement(jobId, orgId);
        }

        public async Task<IList<RecApplicationViewModel>> GetJobAdvertismentState(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode, string status)
        {
            return await _recQueryBusiness.GetJobAdvertismentState(jobid, orgId, jobadvtstate, tempcode, nexttempcode, visatempcode, status);
        }

        public async Task<IList<RecApplicationViewModel>> GetDirictHiringData(string jobid, string orgId)
        {
            return await _recQueryBusiness.GetDirictHiringData(jobid, orgId);
        }

        public async Task<IList<RecApplicationViewModel>> GetJobAdvertismentApplication(string jobadvtid, string orgId, string jobadvtstate, string templateCode, string templateCodeOther)
        {
            return await _recQueryBusiness.GetJobAdvertismentApplication(jobadvtid, orgId, jobadvtstate, templateCode, templateCodeOther);
        }
        public async Task<IdNameViewModel> GetNationalitybyId(string id)
        {
            return await _recQueryBusiness.GetNationalitybyId(id);
        }
        public async Task<IdNameViewModel> GetTitlebyId(string id)
        {
            return await _recQueryBusiness.GetTitlebyId(id);
        }
        public async Task<RecApplicationViewModel> GetApplicationDeclarationData(string applicationId)
        {
            return await _recQueryBusiness.GetApplicationDeclarationData(applicationId);
        }
        public async Task<RecApplicationViewModel> GetConfidentialAgreementDetails(string applicationId)
        {
            return await _recQueryBusiness.GetConfidentialAgreementDetails(applicationId);
        }
        public async Task<RecApplicationViewModel> GetCompetenceMatrixDetails(string applicationId)
        {
            return await _recQueryBusiness.GetCompetenceMatrixDetails(applicationId);
        }
        public async Task<IdNameViewModel> GetUserSign()
        {
            return await _recQueryBusiness.GetUserSign();
        }
        public async Task<IdNameViewModel> GetJobManpowerType(string Id)
        {
            return await _recQueryBusiness.GetJobManpowerType(Id);
        }
        public async Task<RecApplicationViewModel> GetStaffJoiningDetails(string applicationId)
        {
            return await _recQueryBusiness.GetStaffJoiningDetails(applicationId);
        }
        public async Task<IList<ApplicationStateTrackDetailViewModel>> GetAppStateTrackDetailsByCand(string applicationId)
        {
            var states = await _recQueryBusiness.GetAppStateTrackDetailsByCand(applicationId);

            var comments = await _recQueryBusiness.GetTaskCommentsList(applicationId);

            var list = GetAllState();
            var applied = states.FirstOrDefault(x => x.StateCode == "UnReviewed");
            if (applied != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 1.0);
                if (item != null)
                {
                    item.ActionStatus = "Applied";
                    item.ChangedDate = applied.ChangedDate;
                    item.TaskId = applied.TaskId;
                    item.TaskAssignedToUserId = applied.TaskAssignedToUserId;
                }
            }

            var shortListByHr = states.FirstOrDefault(x => x.StateCode == "ShortListByHr"
            && x.ApplicationStatusCode == null);
            if (shortListByHr != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 2.0);
                if (item != null)
                {
                    item.ActionStatus = shortListByHr.StatusName.Coalesce(shortListByHr.StatusCode);
                    item.ChangedDate = shortListByHr.ChangedDate;
                    item.TaskId = shortListByHr.TaskId;
                    item.TaskAssignedToUserId = shortListByHr.TaskAssignedToUserId;
                }
            }

            var shortListBatch = states.FirstOrDefault(x => x.StateCode == "ShortListByHr"
          && x.ApplicationStatusCode == "SL_BATCH");
            if (shortListBatch != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 2.1);
                if (item != null)
                {
                    item.ActionStatus = shortListBatch.StatusName.Coalesce(shortListBatch.ApplicationStatusCode);
                    item.ChangedDate = shortListBatch.ChangedDate;
                    item.TaskId = shortListBatch.TaskId;
                    item.TaskAssignedToUserId = shortListBatch.TaskAssignedToUserId;
                }
            }

            var shortListBatchSend = states.FirstOrDefault(x => x.StateCode == "ShortListByHr"
            && x.ApplicationStatusCode == "SL_BATCH_SEND");
            if (shortListBatchSend != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 2.2);
                if (item != null)
                {
                    item.ActionStatus = shortListBatchSend.StatusName.Coalesce(shortListBatchSend.ApplicationStatusCode);
                    item.ChangedDate = shortListBatchSend.ChangedDate;
                    item.TaskId = shortListBatchSend.TaskId;
                    item.TaskAssignedToUserId = shortListBatchSend.TaskAssignedToUserId;
                }
            }

            //var shortListHM = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
            //&& x.ApplicationStatusCode== "SL_HM");
            //if (shortListHM != null)
            //{
            //    var item = list.FirstOrDefault(x => x.UniqueNumber == 3.0);
            //    if (item != null)
            //    {
            //        item.ActionStatus = shortListHM.StatusName.Coalesce(shortListHM.StatusCode);
            //        item.ChangedDate = shortListHM.ChangedDate;
            //    }
            //}

            var no3_01 = states.FirstOrDefault(x => x.StateCode == "DirectHiring"
           && x.ApplicationStatusCode == null);
            if (no3_01 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.01);
                if (item != null)
                {
                    item.ActionName = no3_01.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ChangedDate = no3_01.ChangedDate;
                    item.TaskId = no3_01.TaskId;
                    item.TaskAssignedToUserId = no3_01.TaskAssignedToUserId;
                }
            }

            var no3_01_1 = states.FirstOrDefault(x => x.StateCode == "DirectHiring"
             && x.ApplicationStatusCode == "TASK_DIRECT_HIRING");
            if (no3_01_1 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.011);
                if (item != null)
                {
                    item.ActionName = no3_01_1.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_01_1.ActionStatus;
                    item.ActionSLAActual = no3_01_1.ActionSLAActual;
                    item.ActionSLAPlanned = no3_01_1.ActionSLAPlanned;
                    item.DaysElapsed = no3_01_1.DaysElapsed;
                    item.ChangedDate = no3_01_1.ChangedDate;
                    item.AssigneeName = no3_01_1.AssigneeName;
                    item.TextValue1 = no3_01_1.TextValue2;
                    item.TaskId = no3_01_1.TaskId;
                    item.TaskAssignedToUserId = no3_01_1.TaskAssignedToUserId;
                }
            }
            var no3_011_2 = states.FirstOrDefault(x => x.StateCode == "DirectHiring"
             && x.ApplicationStatusCode == "DIRECTHIRING_EVALUATIONFORM");
            if (no3_011_2 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.012);
                if (item != null)
                {
                    item.ActionName = no3_011_2.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_011_2.ActionStatus;
                    item.ActionSLAActual = no3_011_2.ActionSLAActual;
                    item.ActionSLAPlanned = no3_011_2.ActionSLAPlanned;
                    item.DaysElapsed = no3_011_2.DaysElapsed;
                    item.ChangedDate = no3_011_2.ChangedDate;
                    item.AssigneeName = no3_011_2.AssigneeName;
                    item.TextValue1 = no3_011_2.TextValue2;
                    item.TaskId = no3_011_2.TaskId;
                    item.TaskAssignedToUserId = no3_011_2.TaskAssignedToUserId;
                }
            }
            var no3_01_2 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
            && x.ApplicationStatusCode == "SCHEDULE_INTERVIEW_CANDIDATE");
            if (no3_01_2 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 3.22);
                if (item != null)
                {
                    item.ActionName = no3_01_2.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_01_2.ActionStatus;
                    item.ActionSLAActual = no3_01_2.ActionSLAActual;
                    item.ActionSLAPlanned = no3_01_2.ActionSLAPlanned;
                    item.DaysElapsed = no3_01_2.DaysElapsed;
                    item.ChangedDate = no3_01_2.ChangedDate;
                    item.AssigneeName = no3_01_2.AssigneeName;
                    item.TextValue1 = no3_01_2.TextValue3;
                    item.TaskId = no3_01_2.TaskId;
                    item.TaskAssignedToUserId = no3_01_2.TaskAssignedToUserId;
                }
            }


            var no3_1 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
             && x.ApplicationStatusCode == "SL_HM");
            if (no3_1 != null && no3_1.StatusCode == null)
            {
                no3_1 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm" && x.ApplicationStatusCode == "NotShortlisted");
                if (no3_1 != null)
                {
                    var item = list.FirstOrDefault(x => x.UniqueNumber == 3.1);
                    if (item != null)
                    {
                        item.ActionName = no3_1.TaskTemplateSubject.Coalesce(item.ActionName);
                        item.ActionStatus = no3_1.ApplicationStatusName;
                        item.ChangedDate = no3_1.ChangedDate;
                        item.TaskId = no3_1.TaskId;
                        item.TaskAssignedToUserId = no3_1.TaskAssignedToUserId;
                        item.TextValue1 = no3_1.ShortlistByHMComment;
                    }
                }
                else
                {
                    no3_1 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm" && x.ApplicationStatusCode == "ShortlistedHM");
                    if (no3_1 != null)
                    {
                        var item = list.FirstOrDefault(x => x.UniqueNumber == 3.1);
                        if (item != null)
                        {
                            item.ActionName = no3_1.TaskTemplateSubject.Coalesce(item.ActionName);
                            item.ActionStatus = no3_1.StatusName;
                            item.ChangedDate = no3_1.ChangedDate;
                            item.TaskId = no3_1.TaskId;
                            item.TaskAssignedToUserId = no3_1.TaskAssignedToUserId;
                            item.TextValue1 = no3_1.ShortlistByHMComment;
                        }
                    }
                    else
                    {
                        var item = list.FirstOrDefault(x => x.UniqueNumber == 3.1);
                        if (item != null)
                        {
                            item.ActionName = no3_1.TaskTemplateSubject.Coalesce(item.ActionName);
                            item.ActionStatus = no3_1.ApplicationStatusName;
                            item.ChangedDate = no3_1.ChangedDate;
                            item.TaskId = no3_1.TaskId;
                            item.TaskAssignedToUserId = no3_1.TaskAssignedToUserId;
                            item.TextValue1 = no3_1.ShortlistByHMComment;
                        }
                    }
                }
            }
            else
            {
                if (no3_1 != null)
                {
                    var item = list.FirstOrDefault(x => x.UniqueNumber == 3.1);
                    if (item != null)
                    {
                        item.ActionName = no3_1.TaskTemplateSubject.Coalesce(item.ActionName);
                        item.ActionStatus = no3_1.StatusName;
                        item.ChangedDate = no3_1.ChangedDate;
                        item.TaskId = no3_1.TaskId;
                        item.TaskAssignedToUserId = no3_1.TaskAssignedToUserId;
                        item.TextValue1 = no3_1.ShortlistByHMComment;
                    }
                }
            }

            var no3_2_1 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
             && x.ApplicationStatusCode == "SCHEDULE_INTERVIEW_RECRUITER");
            if (no3_2_1 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 3.21);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "SCHEDULE_INTERVIEW");
                    item.ActionName = no3_2_1.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_2_1.ActionStatus;
                    item.ActionSLAActual = no3_2_1.ActionSLAActual;
                    item.ActionSLAPlanned = no3_2_1.ActionSLAPlanned;
                    item.DaysElapsed = no3_2_1.DaysElapsed;
                    item.ChangedDate = no3_2_1.ChangedDate;
                    item.AssigneeName = no3_2_1.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no3_2_1.TextValue2;
                    item.TaskId = no3_2_1.TaskId;
                    item.TaskAssignedToUserId = no3_2_1.TaskAssignedToUserId;
                }
            }
            //var no3_2_2 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
            //&& x.ApplicationStatusCode == "SCHEDULE_INTERVIEW_CANDIDATE");
            //if (no3_2_2 != null)
            //{
            //    var item = list.FirstOrDefault(x => x.UniqueNumber == 3.22);
            //    if (item != null)
            //    {
            //        item.ActionName = no3_2_2.TaskTemplateSubject.Coalesce(item.ActionName);
            //        item.ActionStatus = no3_2_2.ActionStatus;
            //        item.ActionSLAActual = no3_2_2.ActionSLAActual;
            //        item.ActionSLAPlanned = no3_2_2.ActionSLAPlanned;
            //        item.DaysElapsed = no3_2_2.DaysElapsed;
            //        item.ChangedDate = no3_2_2.ChangedDate;
            //        item.AssigneeName = no3_2_2.AssigneeName;
            //        item.TextValue1 = no3_2_2.TextValue3;
            //    }
            //}


            var no3_2_2 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
          && x.ApplicationStatusCode == "INTERVIEW_EVALUATION_HOD");
            if (no3_2_2 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 3.22);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "SCHEDULE_INTERVIEW");
                    item.ActionName = no3_2_2.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_2_2.ActionStatus;
                    item.ActionSLAActual = no3_2_2.ActionSLAActual;
                    item.ActionSLAPlanned = no3_2_2.ActionSLAPlanned;
                    item.DaysElapsed = no3_2_2.DaysElapsed;
                    item.ChangedDate = no3_2_2.ChangedDate;
                    item.AssigneeName = no3_2_2.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no3_2_2.TextValue3;
                    item.TaskId = no3_2_2.TaskId;
                    item.TaskAssignedToUserId = no3_2_2.TaskAssignedToUserId;
                }
            }

            var no3_3 = states.FirstOrDefault(x => x.ApplicationStatusCode == "INTERVIEW_EVALUATION_HM");
            if (no3_3 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 3.3);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "SCHEDULE_INTERVIEW");
                    item.ActionName = no3_3.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_3.ActionStatus;
                    item.ActionSLAActual = no3_3.ActionSLAActual;
                    item.ActionSLAPlanned = no3_3.ActionSLAPlanned;
                    item.DaysElapsed = no3_3.DaysElapsed;
                    item.ChangedDate = no3_3.ChangedDate;
                    item.AssigneeName = no3_3.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no3_3.TextValue2;
                    item.TaskId = no3_3.TaskId;
                    item.TaskAssignedToUserId = no3_3.TaskAssignedToUserId;
                }
            }
            //var no3_3 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            // && x.ApplicationStatusCode == "INTERVIEW_EVALUATION_HM");
            //if (no3_3 != null)
            //{
            //    var item = list.FirstOrDefault(x => x.UniqueNumber == 3.3);
            //    if (item != null)
            //    {
            //        item.ActionStatus = no3_3.StatusName.Coalesce(no3_3.ApplicationStatusCode);
            //        item.ChangedDate = no3_3.ChangedDate;
            //    }
            //}

            var no4_11 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
             && x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL1");
            if (no4_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.11);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "EMPLOYEE_APPOINTMENT");
                    item.ActionName = no4_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_11.ActionStatus;
                    item.ActionSLAActual = no4_11.ActionSLAActual;
                    item.ActionSLAPlanned = no4_11.ActionSLAPlanned;
                    item.DaysElapsed = no4_11.DaysElapsed;
                    item.ChangedDate = no4_11.ChangedDate;
                    item.AssigneeName = no4_11.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no4_11.TextValue4;
                    item.TaskId = no4_11.TaskId;
                    item.TaskAssignedToUserId = no4_11.TaskAssignedToUserId;
                }
            }

            var no4_11_1 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            && x.ApplicationStatusCode == "REVISING_INTENT_TO_OFFER_HOD");
            if (no4_11_1 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.111);
                if (item != null)
                {
                    item.ActionName = no4_11_1.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_11_1.ActionStatus;
                    item.ActionSLAActual = no4_11_1.ActionSLAActual;
                    item.ActionSLAPlanned = no4_11_1.ActionSLAPlanned;
                    item.DaysElapsed = no4_11_1.DaysElapsed;
                    item.ChangedDate = no4_11_1.ChangedDate;
                    item.AssigneeName = no4_11_1.AssigneeName;
                    item.TextValue1 = no4_11_1.TextValue4;
                    item.TaskId = no4_11_1.TaskId;
                    item.TaskAssignedToUserId = no4_11_1.TaskAssignedToUserId;
                }
            }

            var no4_12 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
             && x.ApplicationStatusCode == "INTENT_TO_OFFER");
            if (no4_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.12);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "EMPLOYEE_APPOINTMENT");
                    item.ActionName = no4_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_12.ActionStatus;
                    item.ActionSLAActual = no4_12.ActionSLAActual;
                    item.ActionSLAPlanned = no4_12.ActionSLAPlanned;
                    item.DaysElapsed = no4_12.DaysElapsed;
                    item.ChangedDate = no4_12.ChangedDate;
                    item.AssigneeName = no4_12.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no4_12.TextValue2;
                    item.TaskId = no4_12.TaskId;
                    item.TaskAssignedToUserId = no4_12.TaskAssignedToUserId;
                }
            }
            //New step

            var no4_13 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            && x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL2");
            if (no4_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.13);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "EMPLOYEE_APPOINTMENT");
                    item.ActionName = no4_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_13.ActionStatus;
                    item.ActionSLAActual = no4_13.ActionSLAActual;
                    item.ActionSLAPlanned = no4_13.ActionSLAPlanned;
                    item.DaysElapsed = no4_13.DaysElapsed;
                    item.ChangedDate = no4_13.ChangedDate;
                    item.AssigneeName = no4_13.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no4_13.TextValue2;
                    item.TaskId = no4_13.TaskId;
                    item.TaskAssignedToUserId = no4_13.TaskAssignedToUserId;
                }
            }
            var no4_131 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            && x.ApplicationStatusCode == "REVIEWED_INTENT_OFFER");
            if (no4_131 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.131);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "EMPLOYEE_APPOINTMENT");
                    item.ActionName = no4_131.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_131.ActionStatus;
                    item.ActionSLAActual = no4_131.ActionSLAActual;
                    item.ActionSLAPlanned = no4_131.ActionSLAPlanned;
                    item.DaysElapsed = no4_131.DaysElapsed;
                    item.ChangedDate = no4_131.ChangedDate;
                    item.AssigneeName = no4_131.AssigneeName;
                    item.TextValue1 = comment.TextValue4; //no4_131.TextValue3;
                    item.TaskId = no4_131.TaskId;
                    item.TaskAssignedToUserId = no4_131.TaskAssignedToUserId;
                }
            }
            var no4_14 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
           && x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL3");
            if (no4_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.14);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "EMPLOYEE_APPOINTMENT");
                    item.ActionName = no4_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_14.ActionStatus;
                    item.ActionSLAActual = no4_14.ActionSLAActual;
                    item.ActionSLAPlanned = no4_14.ActionSLAPlanned;
                    item.DaysElapsed = no4_14.DaysElapsed;
                    item.ChangedDate = no4_14.ChangedDate;
                    item.AssigneeName = no4_14.AssigneeName;
                    item.TextValue1 = comment.TextValue5; //no4_14.TextValue2;
                    item.TaskId = no4_14.TaskId;
                    item.TaskAssignedToUserId = no4_14.TaskAssignedToUserId;
                }
            }
            // var no4_15 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            //&& x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL4");
            // if (no4_15 != null)
            // {
            //     var item = list.FirstOrDefault(x => x.UniqueNumber == 4.15);
            //     if (item != null)
            //     {
            //         item.ActionName = no4_15.TaskTemplateSubject.Coalesce(item.ActionName);
            //         item.ActionStatus = no4_15.ActionStatus;
            //         item.ActionSLAActual = no4_15.ActionSLAActual;
            //         item.ActionSLAPlanned = no4_15.ActionSLAPlanned;
            //         item.DaysElapsed = no4_15.DaysElapsed;
            //         item.ChangedDate = no4_15.ChangedDate;
            //         item.AssigneeName = no4_15.AssigneeName;
            //         item.TextValue1 = no4_15.TextValue2;
            //     }
            // }
            var no4_16 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            && x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL5");
            if (no4_16 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.15);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "EMPLOYEE_APPOINTMENT");
                    item.ActionName = no4_16.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_16.ActionStatus;
                    item.ActionSLAActual = no4_16.ActionSLAActual;
                    item.ActionSLAPlanned = no4_16.ActionSLAPlanned;
                    item.DaysElapsed = no4_16.DaysElapsed;
                    item.ChangedDate = no4_16.ChangedDate;
                    item.AssigneeName = no4_16.AssigneeName;
                    item.TextValue1 = comment.TextValue6; //no4_16.TextValue2;
                    item.TaskId = no4_16.TaskId;
                    item.TaskAssignedToUserId = no4_16.TaskAssignedToUserId;
                }
            }
            var no5_11 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_POOL_HOD");
            if (no5_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.11);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_POOL_REQUEST");
                    item.ActionName = no5_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_11.ActionStatus;
                    item.ActionSLAActual = no5_11.ActionSLAActual;
                    item.ActionSLAPlanned = no5_11.ActionSLAPlanned;
                    item.DaysElapsed = no5_11.DaysElapsed;
                    item.ChangedDate = no5_11.ChangedDate;
                    item.AssigneeName = no5_11.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no5_11.TextValue4;
                    item.TaskId = no5_11.TaskId;
                    item.TaskAssignedToUserId = no5_11.TaskAssignedToUserId;
                }
            }

            var no5_12 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "REVIEW_WORKER_HR");
            if (no5_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.12);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_POOL_REQUEST");
                    item.ActionName = no5_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_12.ActionStatus;
                    item.ActionSLAActual = no5_12.ActionSLAActual;
                    item.ActionSLAPlanned = no5_12.ActionSLAPlanned;
                    item.DaysElapsed = no5_12.DaysElapsed;
                    item.ChangedDate = no5_12.ChangedDate;
                    item.AssigneeName = no5_12.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no5_12.TextValue1;
                    item.TaskId = no5_12.TaskId;
                    item.TaskAssignedToUserId = no5_12.TaskAssignedToUserId;
                }
            }
            var no5_13 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_POOL_HR_HEAD");
            if (no5_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.13);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_POOL_REQUEST");
                    item.ActionName = no5_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_13.ActionStatus;
                    item.ActionSLAActual = no5_13.ActionSLAActual;
                    item.ActionSLAPlanned = no5_13.ActionSLAPlanned;
                    item.DaysElapsed = no5_13.DaysElapsed;
                    item.ChangedDate = no5_13.ChangedDate;
                    item.AssigneeName = no5_13.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no5_13.TextValue4;
                    item.TaskId = no5_13.TaskId;
                    item.TaskAssignedToUserId = no5_13.TaskAssignedToUserId;
                }
            }
            var no5_14 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_POOL_ED");
            if (no5_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.14);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_POOL_REQUEST");
                    item.ActionName = no5_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_14.ActionStatus;
                    item.ActionSLAActual = no5_14.ActionSLAActual;
                    item.ActionSLAPlanned = no5_14.ActionSLAPlanned;
                    item.DaysElapsed = no5_14.DaysElapsed;
                    item.ChangedDate = no5_14.ChangedDate;
                    item.AssigneeName = no5_14.AssigneeName;
                    item.TextValue1 = comment.TextValue4; //no5_14.TextValue4;
                    item.TaskId = no5_14.TaskId;
                    item.TaskAssignedToUserId = no5_14.TaskAssignedToUserId;
                }
            }
            var no5_15 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_POOL_HR");
            if (no5_15 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.15);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_POOL_REQUEST");
                    item.ActionName = no5_15.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_15.ActionStatus;
                    item.ActionSLAActual = no5_15.ActionSLAActual;
                    item.ActionSLAPlanned = no5_15.ActionSLAPlanned;
                    item.DaysElapsed = no5_15.DaysElapsed;
                    item.ChangedDate = no5_15.ChangedDate;
                    item.AssigneeName = no5_15.AssigneeName;
                    item.TextValue1 = comment.TextValue5; //no5_15.TextValue4;
                    item.TaskId = no5_15.TaskId;
                    item.TaskAssignedToUserId = no5_15.TaskAssignedToUserId;
                }
            }
            var no5_16 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_SALARY_AGENCY");
            if (no5_16 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.16);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_POOL_REQUEST");
                    item.ActionName = no5_16.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_16.ActionStatus;
                    item.ActionSLAActual = no5_16.ActionSLAActual;
                    item.ActionSLAPlanned = no5_16.ActionSLAPlanned;
                    item.DaysElapsed = no5_16.DaysElapsed;
                    item.ChangedDate = no5_16.ChangedDate;
                    item.AssigneeName = no5_16.AssigneeName;
                    item.TextValue1 = comment.TextValue6; //no5_16.TextValue2;
                    item.TaskId = no5_16.TaskId;
                    item.TaskAssignedToUserId = no5_16.TaskAssignedToUserId;
                }
            }

            var no6_11 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
            && x.ApplicationStatusCode == "FINAL_OFFER_APPROVAL_RECRUITER");
            if (no6_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.11);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "REC_WF_FINAL_OFFER");
                    item.ActionName = no6_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_11.ActionStatus;
                    item.ActionSLAActual = no6_11.ActionSLAActual;
                    item.ActionSLAPlanned = no6_11.ActionSLAPlanned;
                    item.DaysElapsed = no6_11.DaysElapsed;
                    item.ChangedDate = no6_11.ChangedDate;
                    item.AssigneeName = no6_11.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no6_11.TextValue6;
                    item.TaskId = no6_11.TaskId;
                    item.TaskAssignedToUserId = no6_11.TaskAssignedToUserId;
                }
            }
            var no6_12 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
           && x.ApplicationStatusCode == "FINAL_OFFER_APPROVAL_HR_HEAD");
            if (no6_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.12);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "REC_WF_FINAL_OFFER");
                    item.ActionName = no6_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_12.ActionStatus;
                    item.ActionSLAActual = no6_12.ActionSLAActual;
                    item.ActionSLAPlanned = no6_12.ActionSLAPlanned;
                    item.DaysElapsed = no6_12.DaysElapsed;
                    item.ChangedDate = no6_12.ChangedDate;
                    item.AssigneeName = no6_12.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no6_12.TextValue2;
                    item.TaskId = no6_12.TaskId;
                    item.TaskAssignedToUserId = no6_12.TaskAssignedToUserId;
                }
            }
            var no6_13 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
          && x.ApplicationStatusCode == "FINAL_OFFER_APPROVAL_ED");
            if (no6_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.13);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "REC_WF_FINAL_OFFER");
                    item.ActionName = no6_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_13.ActionStatus;
                    item.ActionSLAActual = no6_13.ActionSLAActual;
                    item.ActionSLAPlanned = no6_13.ActionSLAPlanned;
                    item.DaysElapsed = no6_13.DaysElapsed;
                    item.ChangedDate = no6_13.ChangedDate;
                    item.AssigneeName = no6_13.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no6_13.TextValue2;
                    item.TaskId = no6_13.TaskId;
                    item.TaskAssignedToUserId = no6_13.TaskAssignedToUserId;
                }
            }
            var no6_14 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
            && x.ApplicationStatusCode == "FINAL_OFFER_APPROVAL_CANDIDATE");
            if (no6_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.14);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "REC_WF_FINAL_OFFER");
                    item.ActionName = no6_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_14.ActionStatus;
                    item.ActionSLAActual = no6_14.ActionSLAActual;
                    item.ActionSLAPlanned = no6_14.ActionSLAPlanned;
                    item.DaysElapsed = no6_14.DaysElapsed;
                    item.ChangedDate = no6_14.ChangedDate;
                    item.AssigneeName = no6_14.AssigneeName;
                    item.TextValue1 = comment.TextValue4; //no6_14.TextValue2;
                    item.TaskId = no6_14.TaskId;
                    item.TaskAssignedToUserId = no6_14.TaskAssignedToUserId;
                }
            }

            var no6_15 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
           && x.ApplicationStatusCode == "REVIEW_FINAL_OFFER_HR");
            if (no6_15 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.15);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "REC_WF_FINAL_OFFER");
                    item.ActionName = no6_15.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_15.ActionStatus;
                    item.ActionSLAActual = no6_15.ActionSLAActual;
                    item.ActionSLAPlanned = no6_15.ActionSLAPlanned;
                    item.DaysElapsed = no6_15.DaysElapsed;
                    item.ChangedDate = no6_15.ChangedDate;
                    item.AssigneeName = no6_15.AssigneeName;
                    item.TextValue1 = comment.TextValue5; //no6_15.TextValue2;
                    item.TaskId = no6_15.TaskId;
                    item.TaskAssignedToUserId = no6_15.TaskAssignedToUserId;
                }
            }


            var no7_11 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "INFORM_CANDIDATE_FOR_MEDICAL");
            if (no7_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.11);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "BUSINESS_VISA_MEDICAL");
                    item.ActionName = no7_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_11.ActionStatus;
                    item.ActionSLAActual = no7_11.ActionSLAActual;
                    item.ActionSLAPlanned = no7_11.ActionSLAPlanned;
                    item.DaysElapsed = no7_11.DaysElapsed;
                    item.ChangedDate = no7_11.ChangedDate;
                    item.AssigneeName = no7_11.AssigneeName;
                    item.TaskId = no7_11.TaskId;
                    item.TaskAssignedToUserId = no7_11.TaskAssignedToUserId;
                    item.TextValue1 = comment.TextValue1;
                }
            }
            var no7_12 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
           && x.ApplicationStatusCode == "CHECK_MEDICAL_REPORT_INFORM_PRO");
            if (no7_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.12);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "BUSINESS_VISA_MEDICAL");
                    item.ActionName = no7_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_12.ActionStatus;
                    item.ActionSLAActual = no7_12.ActionSLAActual;
                    item.ActionSLAPlanned = no7_12.ActionSLAPlanned;
                    item.DaysElapsed = no7_12.DaysElapsed;
                    item.ChangedDate = no7_12.ChangedDate;
                    item.AssigneeName = no7_12.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no7_12.TextValue2;
                    item.TaskId = no7_12.TaskId;
                    item.TaskAssignedToUserId = no7_12.TaskAssignedToUserId;
                }
            }
            var no7_13 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
           && x.ApplicationStatusCode == "OBTAIN_BUSINESS_VISA");
            if (no7_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.13);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "BUSINESS_VISA_MEDICAL");
                    item.ActionName = no7_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_13.ActionStatus;
                    item.ActionSLAActual = no7_13.ActionSLAActual;
                    item.ActionSLAPlanned = no7_13.ActionSLAPlanned;
                    item.DaysElapsed = no7_13.DaysElapsed;
                    item.ChangedDate = no7_13.ChangedDate;
                    item.AssigneeName = no7_13.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no7_13.TextValue6;
                    item.TaskId = no7_13.TaskId;
                    item.TaskAssignedToUserId = no7_13.TaskAssignedToUserId;
                }
            }
            var no7_14 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE");
            if (no7_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.14);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "BUSINESS_VISA_MEDICAL");
                    item.ActionName = no7_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_14.ActionStatus;
                    item.ActionSLAActual = no7_14.ActionSLAActual;
                    item.ActionSLAPlanned = no7_14.ActionSLAPlanned;
                    item.DaysElapsed = no7_14.DaysElapsed;
                    item.ChangedDate = no7_14.ChangedDate;
                    item.AssigneeName = no7_14.AssigneeName;
                    item.TextValue1 = comment.TextValue4; //no7_14.TextValue6;
                    item.TaskId = no7_14.TaskId;
                    item.TaskAssignedToUserId = no7_14.TaskAssignedToUserId;
                }
            }
            var no7_151 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "CONFIRM_TRAVELING_DATE");
            if (no7_151 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.151);
                if (item != null)
                {
                    item.ActionName = no7_151.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_151.ActionStatus;
                    item.ActionSLAActual = no7_151.ActionSLAActual;
                    item.ActionSLAPlanned = no7_151.ActionSLAPlanned;
                    item.DaysElapsed = no7_151.DaysElapsed;
                    item.ChangedDate = no7_151.ChangedDate;
                    item.AssigneeName = no7_151.AssigneeName;
                    item.TextValue1 = no7_151.TextValue4;
                    item.TaskId = no7_151.TaskId;
                    item.TaskAssignedToUserId = no7_151.TaskAssignedToUserId;
                }
            }
            var no7_152 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "BOOK_TICKET_ATTACH");
            if (no7_152 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.152);
                if (item != null)
                {
                    item.ActionName = no7_152.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_152.ActionStatus;
                    item.ActionSLAActual = no7_152.ActionSLAActual;
                    item.ActionSLAPlanned = no7_152.ActionSLAPlanned;
                    item.DaysElapsed = no7_152.DaysElapsed;
                    item.ChangedDate = no7_152.ChangedDate;
                    item.AssigneeName = no7_152.AssigneeName;
                    item.TextValue1 = no7_152.TextValue3;
                    item.TaskId = no7_152.TaskId;
                    item.TaskAssignedToUserId = no7_152.TaskAssignedToUserId;
                }
            }
            var no7_153 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "TICKET_ATTACH");
            if (no7_153 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.153);
                if (item != null)
                {
                    item.ActionName = no7_153.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_153.ActionStatus;
                    item.ActionSLAActual = no7_153.ActionSLAActual;
                    item.ActionSLAPlanned = no7_153.ActionSLAPlanned;
                    item.DaysElapsed = no7_153.DaysElapsed;
                    item.ChangedDate = no7_153.ChangedDate;
                    item.AssigneeName = no7_153.AssigneeName;
                    item.TextValue1 = no7_153.TextValue2;
                    item.TaskId = no7_153.TaskId;
                    item.TaskAssignedToUserId = no7_153.TaskAssignedToUserId;
                }
            }
            var no7_154 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
           && x.ApplicationStatusCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL");
            if (no7_154 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.154);
                if (item != null)
                {
                    item.ActionName = no7_154.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_154.ActionStatus;
                    item.ActionSLAActual = no7_154.ActionSLAActual;
                    item.ActionSLAPlanned = no7_154.ActionSLAPlanned;
                    item.DaysElapsed = no7_154.DaysElapsed;
                    item.ChangedDate = no7_154.ChangedDate;
                    item.AssigneeName = no7_154.AssigneeName;
                    item.TextValue1 = no7_154.TextValue2;
                    item.TaskId = no7_154.TaskId;
                    item.TaskAssignedToUserId = no7_154.TaskAssignedToUserId;
                }
            }
            var no7_155 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
          && x.ApplicationStatusCode == "ARRANGE_ACCOMMODATION");
            if (no7_155 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.155);
                if (item != null)
                {
                    item.ActionName = no7_155.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_155.ActionStatus;
                    item.ActionSLAActual = no7_155.ActionSLAActual;
                    item.ActionSLAPlanned = no7_155.ActionSLAPlanned;
                    item.DaysElapsed = no7_155.DaysElapsed;
                    item.ChangedDate = no7_155.ChangedDate;
                    item.AssigneeName = no7_155.AssigneeName;
                    item.TextValue1 = no7_155.TextValue2;
                    item.TaskId = no7_155.TaskId;
                    item.TaskAssignedToUserId = no7_155.TaskAssignedToUserId;
                }
            }
            var no7_156 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "ARRANGE_VEHICLE_PICKUP");
            if (no7_156 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.156);
                if (item != null)
                {
                    item.ActionName = no7_156.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_156.ActionStatus;
                    item.ActionSLAActual = no7_156.ActionSLAActual;
                    item.ActionSLAPlanned = no7_156.ActionSLAPlanned;
                    item.DaysElapsed = no7_156.DaysElapsed;
                    item.ChangedDate = no7_156.ChangedDate;
                    item.AssigneeName = no7_156.AssigneeName;
                    item.TextValue1 = no7_156.TextValue2;
                    item.TaskId = no7_156.TaskId;
                    item.TaskAssignedToUserId = no7_156.TaskAssignedToUserId;
                }
            }

            var no7_21 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "APPLY_WORK_VISA_THROUGH_MOL");
            if (no7_21 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.21);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_VISA_MEDICAL");
                    item.ActionName = no7_21.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_21.ActionStatus;
                    item.ActionSLAActual = no7_21.ActionSLAActual;
                    item.ActionSLAPlanned = no7_21.ActionSLAPlanned;
                    item.DaysElapsed = no7_21.DaysElapsed;
                    item.ChangedDate = no7_21.ChangedDate;
                    item.AssigneeName = no7_21.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no7_21.TextValue4;
                    item.TaskId = no7_21.TaskId;
                    item.TaskAssignedToUserId = no7_21.TaskAssignedToUserId;
                }
            }
            var no7_22 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "BOOK_QVC_APPOINTMENT");
            if (no7_22 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.22);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_VISA_MEDICAL");
                    item.ActionName = no7_22.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_22.ActionStatus;
                    item.ActionSLAActual = no7_22.ActionSLAActual;
                    item.ActionSLAPlanned = no7_22.ActionSLAPlanned;
                    item.DaysElapsed = no7_22.DaysElapsed;
                    item.ChangedDate = no7_22.ChangedDate;
                    item.AssigneeName = no7_22.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no7_22.TextValue3;
                    item.TaskId = no7_22.TaskId;
                    item.TaskAssignedToUserId = no7_22.TaskAssignedToUserId;
                }
            }
            var no7_23 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "CONDUCT_MEDICAL_FINGER_PRINT");
            if (no7_23 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.23);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_VISA_MEDICAL");
                    item.ActionName = no7_23.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_23.ActionStatus;
                    item.ActionSLAActual = no7_23.ActionSLAActual;
                    item.ActionSLAPlanned = no7_23.ActionSLAPlanned;
                    item.DaysElapsed = no7_23.DaysElapsed;
                    item.ChangedDate = no7_23.ChangedDate;
                    item.AssigneeName = no7_23.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no7_23.TextValue3;
                    item.TaskId = no7_23.TaskId;
                    item.TaskAssignedToUserId = no7_23.TaskAssignedToUserId;
                }
            }

            var no7_24 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "FIT_UNFIT_ATTACH_VISA_COPY");
            if (no7_24 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.24);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_VISA_MEDICAL");
                    item.ActionName = no7_24.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_24.ActionStatus;
                    item.ActionSLAActual = no7_24.ActionSLAActual;
                    item.ActionSLAPlanned = no7_24.ActionSLAPlanned;
                    item.DaysElapsed = no7_24.DaysElapsed;
                    item.ChangedDate = no7_24.ChangedDate;
                    item.AssigneeName = no7_24.AssigneeName;
                    item.TextValue1 = comment.TextValue4; //no7_24.TextValue5;
                    item.TaskId = no7_24.TaskId;
                    item.TaskAssignedToUserId = no7_24.TaskAssignedToUserId;
                }
            }

            var no7_25 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE");
            if (no7_25 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.25);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_VISA_MEDICAL");
                    item.ActionName = no7_25.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_25.ActionStatus;
                    item.ActionSLAActual = no7_25.ActionSLAActual;
                    item.ActionSLAPlanned = no7_25.ActionSLAPlanned;
                    item.DaysElapsed = no7_25.DaysElapsed;
                    item.ChangedDate = no7_25.ChangedDate;
                    item.AssigneeName = no7_25.AssigneeName;
                    item.TextValue1 = comment.TextValue5; //no7_25.TextValue6;
                    item.TaskId = no7_25.TaskId;
                    item.TaskAssignedToUserId = no7_25.TaskAssignedToUserId;
                }
            }

            var no7_251 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "CONFIRM_TRAVELING_DATE");
            if (no7_251 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.251);
                if (item != null)
                {
                    item.ActionName = no7_251.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_251.ActionStatus;
                    item.ActionSLAActual = no7_251.ActionSLAActual;
                    item.ActionSLAPlanned = no7_251.ActionSLAPlanned;
                    item.DaysElapsed = no7_251.DaysElapsed;
                    item.ChangedDate = no7_251.ChangedDate;
                    item.AssigneeName = no7_251.AssigneeName;
                    item.TextValue1 = no7_251.TextValue4;
                    item.TaskId = no7_251.TaskId;
                    item.TaskAssignedToUserId = no7_251.TaskAssignedToUserId;
                }
            }

            var no7_252 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "BOOK_TICKET_ATTACH");
            if (no7_252 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.252);
                if (item != null)
                {
                    item.ActionName = no7_252.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_252.ActionStatus;
                    item.ActionSLAActual = no7_252.ActionSLAActual;
                    item.ActionSLAPlanned = no7_252.ActionSLAPlanned;
                    item.DaysElapsed = no7_252.DaysElapsed;
                    item.ChangedDate = no7_252.ChangedDate;
                    item.AssigneeName = no7_252.AssigneeName;
                    item.TextValue1 = no7_252.TextValue3;
                    item.TaskId = no7_252.TaskId;
                    item.TaskAssignedToUserId = no7_252.TaskAssignedToUserId;
                }
            }
            var no7_253 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "TICKET_ATTACH");
            if (no7_253 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.253);
                if (item != null)
                {
                    item.ActionName = no7_253.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_253.ActionStatus;
                    item.ActionSLAActual = no7_253.ActionSLAActual;
                    item.ActionSLAPlanned = no7_253.ActionSLAPlanned;
                    item.DaysElapsed = no7_253.DaysElapsed;
                    item.ChangedDate = no7_253.ChangedDate;
                    item.AssigneeName = no7_253.AssigneeName;
                    item.TextValue1 = no7_253.TextValue2;
                    item.TaskId = no7_253.TaskId;
                    item.TaskAssignedToUserId = no7_253.TaskAssignedToUserId;
                }
            }
            var no7_254 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL");
            if (no7_254 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.254);
                if (item != null)
                {
                    item.ActionName = no7_254.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_254.ActionStatus;
                    item.ActionSLAActual = no7_254.ActionSLAActual;
                    item.ActionSLAPlanned = no7_254.ActionSLAPlanned;
                    item.DaysElapsed = no7_254.DaysElapsed;
                    item.ChangedDate = no7_254.ChangedDate;
                    item.AssigneeName = no7_254.AssigneeName;
                    item.TextValue1 = no7_254.TextValue2;
                    item.TaskId = no7_254.TaskId;
                    item.TaskAssignedToUserId = no7_254.TaskAssignedToUserId;
                }
            }

            var no7_255 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "ARRANGE_ACCOMMODATION");
            if (no7_255 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.255);
                if (item != null)
                {
                    item.ActionName = no7_255.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_255.ActionStatus;
                    item.ActionSLAActual = no7_255.ActionSLAActual;
                    item.ActionSLAPlanned = no7_255.ActionSLAPlanned;
                    item.DaysElapsed = no7_255.DaysElapsed;
                    item.ChangedDate = no7_255.ChangedDate;
                    item.AssigneeName = no7_255.AssigneeName;
                    item.TextValue1 = no7_255.TextValue2;
                    item.TaskId = no7_255.TaskId;
                    item.TaskAssignedToUserId = no7_255.TaskAssignedToUserId;
                }
            }

            var no7_256 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "ARRANGE_VEHICLE_PICKUP");
            if (no7_256 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.256);
                if (item != null)
                {
                    item.ActionName = no7_256.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_256.ActionStatus;
                    item.ActionSLAActual = no7_256.ActionSLAActual;
                    item.ActionSLAPlanned = no7_256.ActionSLAPlanned;
                    item.DaysElapsed = no7_256.DaysElapsed;
                    item.ChangedDate = no7_256.ChangedDate;
                    item.AssigneeName = no7_256.AssigneeName;
                    item.TextValue1 = no7_256.TextValue2;
                    item.TaskId = no7_256.TaskId;
                    item.TaskAssignedToUserId = no7_256.TaskAssignedToUserId;
                }
            }

            var no7_31 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
           && x.ApplicationStatusCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS");
            if (no7_31 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.31);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "VISA_TRANSFER");
                    item.ActionName = no7_31.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_31.ActionStatus;
                    item.ActionSLAActual = no7_31.ActionSLAActual;
                    item.ActionSLAPlanned = no7_31.ActionSLAPlanned;
                    item.DaysElapsed = no7_31.DaysElapsed;
                    item.ChangedDate = no7_31.ChangedDate;
                    item.AssigneeName = no7_31.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no7_31.TextValue6;
                    item.TaskId = no7_31.TaskId;
                    item.TaskAssignedToUserId = no7_31.TaskAssignedToUserId;
                }
            }

            var no7_32 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
          && x.ApplicationStatusCode == "VERIFY_DOCUMENTS");
            if (no7_32 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.32);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "VISA_TRANSFER");
                    item.ActionName = no7_32.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_32.ActionStatus;
                    item.ActionSLAActual = no7_32.ActionSLAActual;
                    item.ActionSLAPlanned = no7_32.ActionSLAPlanned;
                    item.DaysElapsed = no7_32.DaysElapsed;
                    item.ChangedDate = no7_32.ChangedDate;
                    item.AssigneeName = no7_32.AssigneeName;
                    //item.TextValue1 = comment.TextValue2; //no7_32.TextValue6;
                    item.TaskId = no7_32.TaskId;
                    item.TaskAssignedToUserId = no7_32.TaskAssignedToUserId;
                }
            }

            var no7_33 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
         && x.ApplicationStatusCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP");
            if (no7_33 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.33);
                if (item != null)
                {
                    item.ActionName = no7_33.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_33.ActionStatus;
                    item.ActionSLAActual = no7_33.ActionSLAActual;
                    item.ActionSLAPlanned = no7_33.ActionSLAPlanned;
                    item.DaysElapsed = no7_33.DaysElapsed;
                    item.ChangedDate = no7_33.ChangedDate;
                    item.AssigneeName = no7_33.AssigneeName;
                    item.TextValue1 = no7_33.TextValue6;
                    item.TaskId = no7_33.TaskId;
                    item.TaskAssignedToUserId = no7_33.TaskAssignedToUserId;
                }
            }

            var no7_34 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
         && x.ApplicationStatusCode == "VERIFY_VISA_TRANSFER_COMPLETED");
            if (no7_34 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.34);
                if (item != null)
                {
                    item.ActionName = no7_34.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_34.ActionStatus;
                    item.ActionSLAActual = no7_34.ActionSLAActual;
                    item.ActionSLAPlanned = no7_34.ActionSLAPlanned;
                    item.DaysElapsed = no7_34.DaysElapsed;
                    item.ChangedDate = no7_34.ChangedDate;
                    item.AssigneeName = no7_34.AssigneeName;
                    item.TextValue1 = no7_34.TextValue6;
                    item.TaskId = no7_34.TaskId;
                    item.TaskAssignedToUserId = no7_34.TaskAssignedToUserId;
                }
            }

            var no7_35 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
        && x.ApplicationStatusCode == "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE");
            if (no7_35 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.35);
                if (item != null)
                {
                    item.ActionName = no7_35.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_35.ActionStatus;
                    item.ActionSLAActual = no7_35.ActionSLAActual;
                    item.ActionSLAPlanned = no7_35.ActionSLAPlanned;
                    item.DaysElapsed = no7_35.DaysElapsed;
                    item.ChangedDate = no7_35.ChangedDate;
                    item.AssigneeName = no7_35.AssigneeName;
                    item.TextValue1 = no7_35.TextValue6;
                    item.TaskId = no7_35.TaskId;
                    item.TaskAssignedToUserId = no7_35.TaskAssignedToUserId;
                }
            }

            var no7_41 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
            && x.ApplicationStatusCode == "SUBMIT_WORK_PERMIT_DOCUMENTS");
            if (no7_41 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.41);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "LOCAL_WORK_PERMIT");
                    item.ActionName = no7_41.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_41.ActionStatus;
                    item.ActionSLAActual = no7_41.ActionSLAActual;
                    item.ActionSLAPlanned = no7_41.ActionSLAPlanned;
                    item.DaysElapsed = no7_41.DaysElapsed;
                    item.ChangedDate = no7_41.ChangedDate;
                    item.AssigneeName = no7_41.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no7_41.TextValue7;
                    item.TaskId = no7_41.TaskId;
                    item.TaskAssignedToUserId = no7_41.TaskAssignedToUserId;
                }
            }
            var no7_42 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
        && x.ApplicationStatusCode == "VERIFY_WORK_PERMIT_DOCUMENTS");
            if (no7_42 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.42);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "LOCAL_WORK_PERMIT");
                    item.ActionName = no7_42.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_42.ActionStatus;
                    item.ActionSLAActual = no7_42.ActionSLAActual;
                    item.ActionSLAPlanned = no7_42.ActionSLAPlanned;
                    item.DaysElapsed = no7_42.DaysElapsed;
                    item.ChangedDate = no7_42.ChangedDate;
                    item.AssigneeName = no7_42.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no7_42.TextValue7;
                    item.TaskId = no7_42.TaskId;
                    item.TaskAssignedToUserId = no7_42.TaskAssignedToUserId;
                }
            }
            var no7_43 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
        && x.ApplicationStatusCode == "OBTAIN_WORK_PERMIT_ATTACH");
            if (no7_43 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.43);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "LOCAL_WORK_PERMIT");
                    item.ActionName = no7_43.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_43.ActionStatus;
                    item.ActionSLAActual = no7_43.ActionSLAActual;
                    item.ActionSLAPlanned = no7_43.ActionSLAPlanned;
                    item.DaysElapsed = no7_43.DaysElapsed;
                    item.ChangedDate = no7_43.ChangedDate;
                    item.AssigneeName = no7_43.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no7_43.TextValue8;
                    item.TaskId = no7_43.TaskId;
                    item.TaskAssignedToUserId = no7_43.TaskAssignedToUserId;
                }
            }
            var no7_44 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
            && x.ApplicationStatusCode == "VERIFY_WORK_PERMIT_OBTAINED");
            if (no7_44 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.44);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "LOCAL_WORK_PERMIT");
                    item.ActionName = no7_44.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_44.ActionStatus;
                    item.ActionSLAActual = no7_44.ActionSLAActual;
                    item.ActionSLAPlanned = no7_44.ActionSLAPlanned;
                    item.DaysElapsed = no7_44.DaysElapsed;
                    item.ChangedDate = no7_44.ChangedDate;
                    item.AssigneeName = no7_44.AssigneeName;
                    item.TextValue1 = comment.TextValue4; //no7_44.TextValue5;
                    item.TaskId = no7_44.TaskId;
                    item.TaskAssignedToUserId = no7_44.TaskAssignedToUserId;
                }
            }
            var no7_45 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
            && x.ApplicationStatusCode == "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE");
            if (no7_45 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.45);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "LOCAL_WORK_PERMIT");
                    item.ActionName = no7_45.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_45.ActionStatus;
                    item.ActionSLAActual = no7_45.ActionSLAActual;
                    item.ActionSLAPlanned = no7_45.ActionSLAPlanned;
                    item.DaysElapsed = no7_45.DaysElapsed;
                    item.ChangedDate = no7_45.ChangedDate;
                    item.AssigneeName = no7_45.AssigneeName;
                    item.TextValue1 = comment.TextValue5; //no7_45.TextValue5;s
                    item.TaskId = no7_45.TaskId;
                    item.TaskAssignedToUserId = no7_45.TaskAssignedToUserId;
                }
            }
            //var no7_46 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
            //&& x.ApplicationStatusCode == "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE");
            //if (no7_45 != null)
            //{
            //    var item = list.FirstOrDefault(x => x.UniqueNumber == 7.45);
            //    if (item != null)
            //    {
            //        item.ActionName = no7_45.TaskTemplateSubject.Coalesce(item.ActionName);
            //        item.ActionStatus = no7_45.ActionStatus;
            //        item.ActionSLAActual = no7_45.ActionSLAActual;
            //        item.ActionSLAPlanned = no7_45.ActionSLAPlanned;
            //        item.DaysElapsed = no7_45.DaysElapsed;
            //        item.ChangedDate = no7_45.ChangedDate;
            //        item.AssigneeName = no7_45.AssigneeName;
            //        item.TextValue1 = no7_45.TextValue5;
            //        item.TaskId = no7_45.TaskId;
            //        item.TaskAssignedToUserId = no7_45.TaskAssignedToUserId;
            //    }
            //}

            //var no8_11 = states.FirstOrDefault(x => x.StateCode == "StaffJoined"
            //&& x.ApplicationStatusCode == "FILL_STAFF_DETAILS");
            //if (no8_11 != null)
            //{
            //    var item = list.FirstOrDefault(x => x.UniqueNumber == 8.11);
            //    if (item != null)
            //    {
            //        item.ActionName = no8_11.TaskTemplateSubject.Coalesce(item.ActionName);
            //        item.ActionStatus = no8_11.ActionStatus;
            //        item.ActionSLAActual = no8_11.ActionSLAActual;
            //        item.ActionSLAPlanned = no8_11.ActionSLAPlanned;
            //        item.DaysElapsed = no8_11.DaysElapsed;
            //        item.ChangedDate = no8_11.ChangedDate;
            //        item.AssigneeName = no8_11.AssigneeName;
            //        item.TextValue1 = no8_11.TextValue9;
            //    }
            // }

            var no9_11 = states.FirstOrDefault(x => x.StateCode == "StaffJoined"
           && x.ApplicationStatusCode == "FILL_STAFF_DETAILS");
            if (no9_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 9.11);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "STAFF_JOINING_FORMALITIES");
                    item.ActionName = no9_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no9_11.ActionStatus;
                    item.ActionSLAActual = no9_11.ActionSLAActual;
                    item.ActionSLAPlanned = no9_11.ActionSLAPlanned;
                    item.DaysElapsed = no9_11.DaysElapsed;
                    item.ChangedDate = no9_11.ChangedDate;
                    item.AssigneeName = no9_11.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no9_11.TextValue9;
                    item.TaskId = no9_11.TaskId;
                    item.TaskAssignedToUserId = no9_11.TaskAssignedToUserId;
                }
            }

            var no10_11 = states.FirstOrDefault(x => x.StateCode == "WorkerJoined"
           && x.ApplicationStatusCode == "FILL_WORKER_DETAILS");
            if (no10_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 10.02);
                if (item != null)
                {
                    item.ActionName = no10_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no10_11.ActionStatus;
                    item.ActionSLAActual = no10_11.ActionSLAActual;
                    item.ActionSLAPlanned = no10_11.ActionSLAPlanned;
                    item.DaysElapsed = no10_11.DaysElapsed;
                    item.ChangedDate = no10_11.ChangedDate;
                    item.AssigneeName = no10_11.AssigneeName;
                    item.TextValue1 = no10_11.TextValue9;
                    item.TaskId = no10_11.TaskId;
                    item.TaskAssignedToUserId = no10_11.TaskAssignedToUserId;
                }
            }

            var no8_12 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "PROVIDE_CASH_VOUCHER");
            if (no8_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 12.1);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "STAFF_JOINING_FORMALITIES");
                    item.ActionName = no8_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_12.ActionStatus;
                    item.ActionSLAActual = no8_12.ActionSLAActual;
                    item.ActionSLAPlanned = no8_12.ActionSLAPlanned;
                    item.DaysElapsed = no8_12.DaysElapsed;
                    item.ChangedDate = no8_12.ChangedDate;
                    item.AssigneeName = no8_12.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no8_12.TextValue2;
                    item.TaskId = no8_12.TaskId;
                    item.TaskAssignedToUserId = no8_12.TaskAssignedToUserId;
                }
            }
            var no8_13 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
           && x.ApplicationStatusCode == "SEND_INTIMATION_EMAIL_ORG_UNIT_HOD_COPYING_IT");
            if (no8_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 12.2);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "STAFF_JOINING_FORMALITIES");
                    item.ActionName = no8_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_13.ActionStatus;
                    item.ActionSLAActual = no8_13.ActionSLAActual;
                    item.ActionSLAPlanned = no8_13.ActionSLAPlanned;
                    item.DaysElapsed = no8_13.DaysElapsed;
                    item.ChangedDate = no8_13.ChangedDate;
                    item.AssigneeName = no8_13.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no8_13.TextValue3;
                    item.TaskId = no8_13.TaskId;
                    item.TaskAssignedToUserId = no8_13.TaskAssignedToUserId;
                }
            }

            var no8_14 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "SEND_FRA_HRA_REQUEST_FINANCE");
            if (no8_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 12.3);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "STAFF_JOINING_FORMALITIES");
                    item.ActionName = no8_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_14.ActionStatus;
                    item.ActionSLAActual = no8_14.ActionSLAActual;
                    item.ActionSLAPlanned = no8_14.ActionSLAPlanned;
                    item.DaysElapsed = no8_14.DaysElapsed;
                    item.ChangedDate = no8_14.ChangedDate;
                    item.AssigneeName = no8_14.AssigneeName;
                    item.TextValue1 = comment.TextValue4; //no8_14.TextValue3;
                    item.TaskId = no8_14.TaskId;
                    item.TaskAssignedToUserId = no8_14.TaskAssignedToUserId;
                }
            }
            var no8_15 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "UPLOAD_PASSPORT_VISA_QATARID");
            if (no8_15 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 12.4);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "STAFF_JOINING_FORMALITIES");
                    item.ActionName = no8_15.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_15.ActionStatus;
                    item.ActionSLAActual = no8_15.ActionSLAActual;
                    item.ActionSLAPlanned = no8_15.ActionSLAPlanned;
                    item.DaysElapsed = no8_15.DaysElapsed;
                    item.ChangedDate = no8_15.ChangedDate;
                    item.AssigneeName = no8_15.AssigneeName;
                    item.TextValue1 = comment.TextValue5; //no8_15.TextValue6;
                    item.TaskId = no8_15.TaskId;
                    item.TaskAssignedToUserId = no8_15.TaskAssignedToUserId;
                }
            }

            var no8_16 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "UPDATE_EMPLOYEE_FILE_IN_SAP");
            if (no8_16 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 12.5);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "STAFF_JOINING_FORMALITIES");
                    item.ActionName = no8_16.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_16.ActionStatus;
                    item.ActionSLAActual = no8_16.ActionSLAActual;
                    item.ActionSLAPlanned = no8_16.ActionSLAPlanned;
                    item.DaysElapsed = no8_16.DaysElapsed;
                    item.ChangedDate = no8_16.ChangedDate;
                    item.AssigneeName = no8_16.AssigneeName;
                    item.TextValue1 = comment.TextValue6; //no8_16.TextValue1;
                    item.TaskId = no8_16.TaskId;
                    item.TaskAssignedToUserId = no8_16.TaskAssignedToUserId;
                }
            }


            var no8_17 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "CONFIRM_INDUCTION_DATE_TO_CANDIDATE");
            if (no8_17 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 12.6);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "STAFF_JOINING_FORMALITIES");
                    item.ActionName = no8_17.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_17.ActionStatus;
                    item.ActionSLAActual = no8_17.ActionSLAActual;
                    item.ActionSLAPlanned = no8_17.ActionSLAPlanned;
                    item.DaysElapsed = no8_17.DaysElapsed;
                    item.ChangedDate = no8_17.ChangedDate;
                    item.AssigneeName = no8_17.AssigneeName;
                    item.TextValue1 = comment.TextValue7; //no8_17.TextValue2;
                    item.TaskId = no8_17.TaskId;
                    item.TaskAssignedToUserId = no8_17.TaskAssignedToUserId;
                }
            }

            var no8_18 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HM");
            if (no8_18 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 12.7);
                if (item != null)
                {
                    item.ActionName = no8_18.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_18.ActionStatus;
                    item.ActionSLAActual = no8_18.ActionSLAActual;
                    item.ActionSLAPlanned = no8_18.ActionSLAPlanned;
                    item.DaysElapsed = no8_18.DaysElapsed;
                    item.ChangedDate = no8_18.ChangedDate;
                    item.AssigneeName = no8_18.AssigneeName;
                    item.TextValue1 = no8_18.TextValue1;
                    item.TaskId = no8_18.TaskId;
                    item.TaskAssignedToUserId = no8_18.TaskAssignedToUserId;
                }
            }
            var no8_19 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HR");
            if (no8_19 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 12.8);
                if (item != null)
                {
                    item.ActionName = no8_19.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_19.ActionStatus;
                    item.ActionSLAActual = no8_19.ActionSLAActual;
                    item.ActionSLAPlanned = no8_19.ActionSLAPlanned;
                    item.DaysElapsed = no8_19.DaysElapsed;
                    item.ChangedDate = no8_19.ChangedDate;
                    item.AssigneeName = no8_19.AssigneeName;
                    item.TaskId = no8_19.TaskId;
                    item.TaskAssignedToUserId = no8_19.TaskAssignedToUserId;
                }
            }
            //var no8_21 = states.FirstOrDefault(x => x.StateCode == "WorkerJoined"
            //&& x.ApplicationStatusCode == "FILL_WORKER_DETAILS");
            //if (no8_21 != null)
            //{
            //    var item = list.FirstOrDefault(x => x.UniqueNumber == 8.21);
            //    if (item != null)
            //    {
            //        item.ActionName = no8_21.TaskTemplateSubject.Coalesce(item.ActionName);
            //        item.ActionStatus = no8_21.ActionStatus;
            //        item.ActionSLAActual = no8_21.ActionSLAActual;
            //        item.ActionSLAPlanned = no8_21.ActionSLAPlanned;
            //        item.DaysElapsed = no8_21.DaysElapsed;
            //        item.ChangedDate = no8_21.ChangedDate;
            //        item.AssigneeName = no8_21.AssigneeName;
            //        item.TextValue1 = no8_21.TextValue9;
            //    }
            //}
            var no8_22 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
           && x.ApplicationStatusCode == "PROVIDE_CASH_VOUCHER_WORKER");
            if (no8_22 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 13.1);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_JOINING_FORMALITIES");
                    item.ActionName = no8_22.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_22.ActionStatus;
                    item.ActionSLAActual = no8_22.ActionSLAActual;
                    item.ActionSLAPlanned = no8_22.ActionSLAPlanned;
                    item.DaysElapsed = no8_22.DaysElapsed;
                    item.ChangedDate = no8_22.ChangedDate;
                    item.AssigneeName = no8_22.AssigneeName;
                    item.TextValue1 = comment.TextValue1; //no8_22.TextValue2;
                    item.TaskId = no8_22.TaskId;
                    item.TaskAssignedToUserId = no8_22.TaskAssignedToUserId;
                }
            }
            var no8_23 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
            && x.ApplicationStatusCode == "UPLOAD_PASSPORT_VISA_QATARID_WORKER");
            if (no8_23 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 13.2);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_JOINING_FORMALITIES");
                    item.ActionName = no8_23.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_23.ActionStatus;
                    item.ActionSLAActual = no8_23.ActionSLAActual;
                    item.ActionSLAPlanned = no8_23.ActionSLAPlanned;
                    item.DaysElapsed = no8_23.DaysElapsed;
                    item.ChangedDate = no8_23.ChangedDate;
                    item.AssigneeName = no8_23.AssigneeName;
                    item.TextValue1 = comment.TextValue2; //no8_23.TextValue6;
                    item.TaskId = no8_23.TaskId;
                    item.TaskAssignedToUserId = no8_23.TaskAssignedToUserId;
                }
            }
            var no8_24 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
           && x.ApplicationStatusCode == "UPDATE_EMPLOYEE_FILE_IN_SAP_WORKER");
            if (no8_24 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 13.3);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_JOINING_FORMALITIES");
                    item.ActionName = no8_24.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_24.ActionStatus;
                    item.ActionSLAActual = no8_24.ActionSLAActual;
                    item.ActionSLAPlanned = no8_24.ActionSLAPlanned;
                    item.DaysElapsed = no8_24.DaysElapsed;
                    item.ChangedDate = no8_24.ChangedDate;
                    item.AssigneeName = no8_24.AssigneeName;
                    item.TextValue1 = comment.TextValue3; //no8_24.TextValue1;
                    item.TaskId = no8_24.TaskId;
                    item.TaskAssignedToUserId = no8_24.TaskAssignedToUserId;
                }
            }

            var no8_25 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
          && x.ApplicationStatusCode == "CONDUCT_INDUCTION");
            if (no8_25 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 13.4);
                if (item != null)
                {
                    var comment = comments.FirstOrDefault(x => x.TemplateCode == "WORKER_JOINING_FORMALITIES");
                    item.ActionName = no8_25.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_25.ActionStatus;
                    item.ActionSLAActual = no8_25.ActionSLAActual;
                    item.ActionSLAPlanned = no8_25.ActionSLAPlanned;
                    item.DaysElapsed = no8_25.DaysElapsed;
                    item.ChangedDate = no8_25.ChangedDate;
                    item.AssigneeName = no8_25.AssigneeName;
                    item.TextValue1 = comment.TextValue4; //no8_25.TextValue1;
                    item.TaskId = no8_25.TaskId;
                    item.TaskAssignedToUserId = no8_25.TaskAssignedToUserId;
                }
            }
            var no8_26 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
          && x.ApplicationStatusCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HM");
            if (no8_26 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 13.5);
                if (item != null)
                {
                    item.ActionName = no8_26.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_26.ActionStatus;
                    item.ActionSLAActual = no8_26.ActionSLAActual;
                    item.ActionSLAPlanned = no8_26.ActionSLAPlanned;
                    item.DaysElapsed = no8_26.DaysElapsed;
                    item.ChangedDate = no8_26.ChangedDate;
                    item.AssigneeName = no8_26.AssigneeName;
                    item.TextValue1 = no8_26.TextValue2;
                    item.TaskId = no8_26.TaskId;
                    item.TaskAssignedToUserId = no8_26.TaskAssignedToUserId;
                }
            }
            var no8_27 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
          && x.ApplicationStatusCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HR");
            if (no8_27 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 13.6);
                if (item != null)
                {
                    item.ActionName = no8_27.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_27.ActionStatus;
                    item.ActionSLAActual = no8_27.ActionSLAActual;
                    item.ActionSLAPlanned = no8_27.ActionSLAPlanned;
                    item.DaysElapsed = no8_27.DaysElapsed;
                    item.ChangedDate = no8_27.ChangedDate;
                    item.AssigneeName = no8_27.AssigneeName;
                    item.TextValue1 = no8_27.TextValue2;
                    item.TaskId = no8_27.TaskId;
                    item.TaskAssignedToUserId = no8_27.TaskAssignedToUserId;
                }
            }

            return list;
        }

        private List<ApplicationStateTrackDetailViewModel> GetAllState()
        {
            var list = new List<ApplicationStateTrackDetailViewModel>();
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 1.0,
                Number = "1",
                Stage = "Applied for job",
                ActionName = "Submit Application"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 2.0,
                Number = "2",
                Stage = "Shortlist by HR",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 2.1,
                Number = "",
                SubNumber = "2.1",
                Stage = "",
                ActionName = "Shortlist to Batch – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 2.2,
                Number = "",
                SubNumber = "2.2",
                Stage = "",
                ActionName = "Batch sent to Hiring Manager - Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.0,
                Number = "3",
                SubNumber = "",
                Stage = "Shortlist by HM",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.1,
                Number = "",
                SubNumber = "3.1",
                Stage = "",
                ActionName = "Shortlist for interview – Hiring Manager"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.21,
                Number = "",
                SubNumber = "3.2.1",
                Stage = "",
                ActionName = "Arrange for interview – Recruiter"
            });
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 3.22,
            //    Number = "",
            //    SubNumber = "3.2.2",
            //    Stage = "",
            //    ActionName = "Accept interview date/time – Candidate/Agency"
            //});
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.3,
                Number = "",
                //SubNumber = "3.2.3",
                SubNumber = "3.2.2",
                Stage = "",
                ActionName = "Evaluation – Hiring Manager"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.22,
                Number = "",
                SubNumber = "3.2.3",
                Stage = "",
                ActionName = "Evaluation Approval – Task to HOD"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.01,
                Number = "4",
                SubNumber = "",
                Stage = "Direct Hiring",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.011,
                Number = "",
                SubNumber = "4.1",
                Stage = "Collect Applicant Information",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.012,
                Number = "",
                SubNumber = "4.2",
                Stage = "Fill Evaluation Form",
                ActionName = ""
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.0,
                Number = "5",
                SubNumber = "",
                Stage = "Intent to Offer [Staff Only]",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.11,
                Number = "",
                SubNumber = "5.1.1",
                Stage = "",
                ActionName = "Prepare intent to offer – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.111,
                Number = "",
                SubNumber = "5.1.1.1",
                Stage = "",
                ActionName = "Revising intent to offer – HOD"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.12,
                Number = "",
                SubNumber = "5.1.2",
                Stage = "",
                ActionName = "Accept intent to offer – Candidate"
            });
            //New step

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.13,
                Number = "",
                SubNumber = "5.1.3",
                Stage = "",
                ActionName = "Approval – HOD"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.131,
                Number = "",
                SubNumber = "5.1.3.1",
                Stage = "",
                ActionName = "Reviewed intent to offer – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.14,
                Number = "",
                SubNumber = "5.1.4",
                Stage = "",
                ActionName = "Approval – HR Head"
            });
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 4.15,
            //    Number = "",
            //    SubNumber = "4.1.5",
            //    Stage = "",
            //    ActionName = "Approval – Planning Head"
            //});
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.15,
                Number = "",
                SubNumber = "5.1.5",
                Stage = "",
                ActionName = "Approval – ED"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.0,
                Number = "6",
                SubNumber = "",
                Stage = "Worker Appointment request",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.11,
                Number = "",
                SubNumber = "6.1.1",
                Stage = "",
                ActionName = "Approval – HOD"
            });
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 5.22,
            //    Number = "",
            //    SubNumber = "5.2.2",
            //    Stage = "",
            //    ActionName = "Approval – Planning Head"
            //});
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.12,
                Number = "",
                SubNumber = "6.1.2",
                Stage = "",
                ActionName = "Review Worker – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.13,
                Number = "",
                SubNumber = "6.1.3",
                Stage = "",
                ActionName = "Approval – HR Head"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.14,
                Number = "",
                SubNumber = "6.1.4",
                Stage = "",
                ActionName = "Approval – ED"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.15,
                Number = "",
                SubNumber = "6.1.5",
                Stage = "",
                ActionName = "Confirm Salary – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.16,
                Number = "",
                SubNumber = "6.1.6",
                Stage = "",
                ActionName = "Worker Salary Acceptance – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.0,
                Number = "7",
                SubNumber = "",
                Stage = "Final Offer",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.11,
                Number = "",
                SubNumber = "7.1.1",
                Stage = "",
                ActionName = "Prepare Final Offer - Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.12,
                Number = "",
                SubNumber = "7.1.2",
                Stage = "",
                ActionName = "Approval – HR Head"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.13,
                Number = "",
                SubNumber = "7.1.3",
                Stage = "",
                ActionName = "Approval – ED"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.14,
                Number = "",
                SubNumber = "7.1.4",
                Stage = "",
                ActionName = "Final Offer Acceptance - Candidate"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.15,
                Number = "",
                SubNumber = "7.1.5",
                Stage = "",
                ActionName = "Review Final Offer Acceptance - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 70,
                Number = "8",
                SubNumber = "",
                Stage = "Visa",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.1,
                Number = "8.1",
                SubNumber = "",
                Stage = "Overseas Business Visa",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.11,
                Number = "",
                SubNumber = "8.1.1",
                Stage = "",
                ActionName = "Submit Medical Report - Candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.12,
                Number = "",
                SubNumber = "8.1.2",
                Stage = "",
                ActionName = "Check Medical Report & Inform PRO – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.13,
                Number = "",
                SubNumber = "8.1.3",
                Stage = "",
                ActionName = "Obtain Business Visa & Attach – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.14,
                Number = "",
                SubNumber = "8.1.4",
                Stage = "",
                ActionName = "Receive Visa Copy & Advise traveling date – Candidate"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.151,
                Number = "",
                SubNumber = "8.1.5.1",
                Stage = "",
                ActionName = "Confirm traveling date – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.152,
                Number = "",
                SubNumber = "8.1.5.2",
                Stage = "",
                ActionName = "Send email for ticket booking – Ticketing Team"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.153,
                Number = "",
                SubNumber = "8.1.5.3",
                Stage = "",
                ActionName = "Attach Ticket & Hotel quarantine booking – Ticketing Team"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.154,
                Number = "",
                SubNumber = "8.1.5.4",
                Stage = "",
                ActionName = "Confirm receipt of ticket & date of travel – Candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.155,
                Number = "",
                SubNumber = "8.1.5.5",
                Stage = "",
                ActionName = "Arrange Accommodation – Admin"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.156,
                Number = "",
                SubNumber = "8.1.5.6",
                Stage = "",
                ActionName = "Arrange Airport Pickup – Plant"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.2,
                Number = "8.2",
                SubNumber = "",
                Stage = "Overseas Worker Visa",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.21,
                Number = "",
                SubNumber = "8.2.1",
                Stage = "",
                ActionName = "Apply work visa through MOL – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.22,
                Number = "",
                SubNumber = "8.2.2",
                Stage = "",
                ActionName = "Book QVC Appointment – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.23,
                Number = "",
                SubNumber = "8.2.3",
                Stage = "",
                ActionName = "Conduct Medical/Fingerprint – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.24,
                Number = "",
                SubNumber = "8.2.4",
                Stage = "",
                ActionName = "Attach visa copy if fit – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.25,
                Number = "",
                SubNumber = "8.2.5",
                Stage = "",
                ActionName = "Receive Visa Copy & Advise traveling date – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.251,
                Number = "",
                SubNumber = "8.2.5.1",
                Stage = "",
                ActionName = "Confirm traveling date – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.252,
                Number = "",
                SubNumber = "8.2.5.2",
                Stage = "",
                ActionName = "Send email for ticket booking – Ticketing Team"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.253,
                Number = "",
                SubNumber = "8.2.5.3",
                Stage = "",
                ActionName = "Attach Ticket & Hotel quarantine booking – Ticketing Team"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.254,
                Number = "",
                SubNumber = "8.2.5.4",
                Stage = "",
                ActionName = "Confirm receipt of ticket & date of travel – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.255,
                Number = "",
                SubNumber = "8.2.5.5",
                Stage = "",
                ActionName = "Arrange Accommodation – Admin"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.256,
                Number = "",
                SubNumber = "8.2.5.6",
                Stage = "",
                ActionName = "Arrange Airport Pickup – Plant"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.3,
                Number = "8.3",
                SubNumber = "",
                Stage = "Local - Visa Transfer",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.31,
                Number = "",
                SubNumber = "8.3.1",
                Stage = "",
                ActionName = "Submit Visa transfer document – candidate"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.32,
                Number = "",
                SubNumber = "8.3.2",
                Stage = "",
                ActionName = "Verify documents – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.33,
                Number = "",
                SubNumber = "8.3.3",
                Stage = "",
                ActionName = "Submit visa transfer & confirm sponsorship change – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.34,
                Number = "",
                SubNumber = "8.3.4",
                Stage = "",
                ActionName = "Verify visa transfer completed – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.35,
                Number = "",
                SubNumber = "8.3.5",
                Stage = "",
                ActionName = "Receive visa transfer confirmation & inform joining date - Candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.4,
                Number = "8.4",
                SubNumber = "",
                Stage = "Local – Work Permit",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.41,
                Number = "",
                SubNumber = "8.4.1",
                Stage = "",
                ActionName = "Submit Work Permit documents – candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.42,
                Number = "",
                SubNumber = "8.4.2",
                Stage = "",
                ActionName = "Verify documents – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.43,
                Number = "",
                SubNumber = "8.4.3",
                Stage = "",
                ActionName = "Obtain Work Permit & Attach – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.44,
                Number = "",
                SubNumber = "8.4.4",
                Stage = "",
                ActionName = "Verify Work Permit Obtained– Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.45,
                Number = "",
                SubNumber = "8.4.5",
                Stage = "",
                ActionName = "Receive Work Permit & inform joining date - Candidate"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 9.0,
                Number = "9",
                SubNumber = "",
                Stage = "Staff Joining – Recruiter Tasks",
                ActionName = ""
            });


            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 9.1,
                Number = "9.1",
                SubNumber = "",
                Stage = "Staff Joining formalities",
                ActionName = ""
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 9.11,
                Number = "",
                SubNumber = "9.1.1",
                Stage = "",
                ActionName = "Fill [Joining Report, Personal Data Form, Competency Matrix, MOL Form] & print forms [Nomination Form, employment contract, NDA], get signature & file - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 10.0,
                Number = "10",
                SubNumber = "",
                Stage = "Worker Joining – Recruiter Tasks",
                ActionName = ""
            });


            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 10.01,
                Number = "10.1",
                SubNumber = "",
                Stage = "Worker Joining formalities",
                ActionName = ""
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 10.02,
                Number = "",
                SubNumber = "10.1.1",
                Stage = "",
                ActionName = "Sign Nomination Form, employment contract - Candidate"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 11.0,
                Number = "11",
                SubNumber = "",
                Stage = "Candidate Joined",
                ActionName = ""
            });


            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12,
                Number = "12",
                SubNumber = "",
                Stage = "Post Joining – Candidate",
                ActionName = ""
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12.1,
                Number = "",
                SubNumber = "12.1",
                Stage = "",
                ActionName = "Provide cash Voucher [Amount] - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12.2,
                Number = "",
                SubNumber = "12.2",
                Stage = "",
                ActionName = "Send intimation email to Org Unit HOD copying IT - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12.3,
                Number = "",
                SubNumber = "12.3",
                Stage = "",
                ActionName = "Send FRA/HRA request to Finance via email - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12.4,
                Number = "",
                SubNumber = "12.4",
                Stage = "",
                ActionName = "Upload Passport and Visa/Qatar ID in system - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12.5,
                Number = "",
                SubNumber = "12.5",
                Stage = "",
                ActionName = "Update employee file in SAP & Timesheet - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12.6,
                Number = "",
                SubNumber = "12.6",
                Stage = "",
                ActionName = "Confirm Induction Date to candidate - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12.7,
                Number = "",
                SubNumber = "12.7",
                Stage = "",
                ActionName = "Confirm / Reject / Extend probation Date - HM"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 12.8,
                Number = "",
                SubNumber = "12.8",
                Stage = "",
                ActionName = "Confirm / Reject / Extend probation Date - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 13,
                Number = "13",
                SubNumber = "",
                Stage = "Post Joining – Worker",
                ActionName = ""
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 13.1,
                Number = "",
                SubNumber = "13.1",
                Stage = "",
                ActionName = "Provide cash Voucher [Amount] - HR"
            });



            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 13.2,
                Number = "",
                SubNumber = "13.2",
                Stage = "",
                ActionName = "Upload Passport and Visa/Qatar ID in system - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 13.3,
                Number = "",
                SubNumber = "13.3",
                Stage = "",
                ActionName = "Update employee file in SAP & Timesheet - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 13.4,
                Number = "",
                SubNumber = "13.4",
                Stage = "",
                ActionName = "Conduct Induction - HR"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 13.5,
                Number = "",
                SubNumber = "13.5",
                Stage = "",
                ActionName = "Confirm / Reject / Extend probation Date - HM"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 13.6,
                Number = "",
                SubNumber = "13.6",
                Stage = "",
                ActionName = "Confirm / Reject / Extend probation Date - HR"
            });

            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.0,
            //    Number = "8",
            //    SubNumber = "",
            //    Stage = "Candidate Joining",
            //    ActionName = ""
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.1,
            //    Number = "8.1",
            //    SubNumber = "",
            //    Stage = "Staff Joining",
            //    ActionName = ""
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.11,
            //    Number = "",
            //    SubNumber = "8.1.1",
            //    Stage = "",
            //    ActionName = "Fill Forms, Get signature from Candidate – HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.12,
            //    Number = "",
            //    SubNumber = "8.1.2",
            //    Stage = "",
            //    ActionName = "Provide Cash Voucher – HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.13,
            //    Number = "",
            //    SubNumber = "8.1.3",
            //    Stage = "",
            //    ActionName = "Send intimation email to Org Unit HOD copying IT – HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.14,
            //    Number = "",
            //    SubNumber = "8.1.4",
            //    Stage = "",
            //    ActionName = "Send FRA/HRA request to Finance via email – HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.15,
            //    Number = "",
            //    SubNumber = "8.1.5",
            //    Stage = "",
            //    ActionName = "Upload Passport and Visa/Qatar ID in system - HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.16,
            //    Number = "",
            //    SubNumber = "8.1.6",
            //    Stage = "",
            //    ActionName = "Update employee file in SAP & Timesheet – HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.17,
            //    Number = "",
            //    SubNumber = "8.1.7",
            //    Stage = "",
            //    ActionName = "Confirm Induction - HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.18,
            //    Number = "",
            //    SubNumber = "8.1.8",
            //    Stage = "",
            //    ActionName = "Confirm/Reject/Extend Probation Date - HM"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.19,
            //    Number = "",
            //    SubNumber = "8.1.9",
            //    Stage = "",
            //    ActionName = "Confirm Probation Date - HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.2,
            //    Number = "8.2",
            //    SubNumber = "",
            //    Stage = "Worker Joining",
            //    ActionName = ""
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.21,
            //    Number = "",
            //    SubNumber = "8.2.1",
            //    Stage = "",
            //    ActionName = "Fill Forms, Get signature from Candidate – HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.22,
            //    Number = "",
            //    SubNumber = "8.2.2",
            //    Stage = "",
            //    ActionName = "Provide Cash Voucher – HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.23,
            //    Number = "",
            //    SubNumber = "8.2.3",
            //    Stage = "",
            //    ActionName = "Upload Passport and Visa/Qatar ID in system - HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.24,
            //    Number = "",
            //    SubNumber = "8.2.4",
            //    Stage = "",
            //    ActionName = "Update employee file in SAP & Timesheet  – HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.25,
            //    Number = "",
            //    SubNumber = "8.2.5",
            //    Stage = "",
            //    ActionName = "Conduct Induction - HR"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.26,
            //    Number = "",
            //    SubNumber = "8.2.6",
            //    Stage = "",
            //    ActionName = "Confirm/Reject/Extend Probation Date - HM"
            //});
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 8.27,
            //    Number = "",
            //    SubNumber = "8.2.7",
            //    Stage = "",
            //    ActionName = "Confirm Probation Date - HR"
            //});
            return list;
        }
        public async Task<List<HiringManagerViewModel>> GetHiringManagersList()
        {
            var list = await _recQueryBusiness.GetHiringManagersList();
            return list;
        }
        public async Task<RecApplicationViewModel> GetApplicationEvaluationDetails(string applicationId)
        {
            var data = await _recQueryBusiness.GetApplicationEvaluationDetails(applicationId);
            return data;
        }
        public async Task<List<RecCandidateEvaluationViewModel>> GetCandidateEvaluationDetails(string applicationId)
        {
            var data = await _recQueryBusiness.GetCandidateEvaluationDetails(applicationId);
            return data;
        }
        public async Task<List<RecCandidateEvaluationViewModel>> GetCandidateEvaluationTemplateData()
        {
            var data = await _recQueryBusiness.GetCandidateEvaluationTemplateData();
            return data;
        }
        public async Task<IList<RecApplicationViewModel>> GetCandiadteShortListApplicationData(ApplicationSearchViewModel search)
        {
            try
            {
                var allList = await _recQueryBusiness.GetCandiadteShortListApplicationData();

                IList<RecApplicationViewModel> Newlist = new List<RecApplicationViewModel>();

                Newlist = allList;

                if (search.ApplicationStateCode.IsNotNullAndNotEmpty())
                {
                    Newlist = Newlist.Where(x => x.ApplicationStateCode == search.ApplicationStateCode).ToList();                    
                }
                if (search.ApplicationStatusCode.IsNotNullAndNotEmpty())
                {
                    Newlist = Newlist.Where(x => x.ApplicationStatusCode == search.ApplicationStatusCode).ToList();
                }
                if (search.JobAdvertisementId.IsNotNullAndNotEmpty())
                {
                    Newlist = Newlist.Where(x => x.JobId == search.JobAdvertisementId).ToList();
                }
                if (search.BatchId.IsNotNullAndNotEmpty())
                {
                    Newlist = Newlist.Where(x => x.BatchId == search.BatchId).ToList();
                }
                else
                {
                    Newlist = new List<RecApplicationViewModel>();
                }

                if (search.BatchStatusCode.IsNotNullAndNotEmpty())
                {
                    Newlist = Newlist.Where(x => x.BatchStatusCode == search.BatchStatusCode).ToList();
                }
                
                return Newlist;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task TriggerIntentToOffer(ServiceTemplateViewModel viewModel,dynamic udf)
        {
                var serviceTempModel = new ServiceTemplateViewModel();

                serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
                serviceTempModel.TemplateCode = "EMPLOYEE_APPOINTMENT";
                var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                
                servicemodel.OwnerUserId = viewModel.OwnerUserId;
                servicemodel.DataAction = DataActionEnum.Create;
                servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                dynamic exo = new System.Dynamic.ExpandoObject();
                
                ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
                var status =  await _lovBusiness.GetSingle(x => x.Code == "IntentToOffer");
                ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

                servicemodel.Json = JsonConvert.SerializeObject(exo);
                await _serviceBusiness.ManageService(servicemodel);
                await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "IntentToOffer");
        } 
        public async Task TriggerWorkerAppointment(ServiceTemplateViewModel viewModel,dynamic udf)
        {
                var serviceTempModel = new ServiceTemplateViewModel();

                serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
                serviceTempModel.TemplateCode = "WORKER_POOL_REQUEST";
                var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                
                servicemodel.OwnerUserId = viewModel.OwnerUserId;
                servicemodel.DataAction = DataActionEnum.Create;
                servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                dynamic exo = new System.Dynamic.ExpandoObject();
                
                ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
                var status = await _lovBusiness.GetSingle(x => x.Code == "WorkerPool");
                ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

                servicemodel.Json = JsonConvert.SerializeObject(exo);
                await _serviceBusiness.ManageService(servicemodel);
                await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "WorkerPool");
        }
        public async Task TriggerFinalOffer(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "PREPARE_FINAL_OFFER";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
            var status = await _lovBusiness.GetSingle(x => x.Code == "FinalOffer");
            ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);
            await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "FinalOffer");
        }
        public async Task TriggerOverseasBusinessVisa(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "BUSINESS_VISA_MEDICAL";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
            var status = await _lovBusiness.GetSingle(x => x.Code == "BusinessVisa");
            ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);
            await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "BusinessVisa");
        }
        public async Task TriggerOverseasWorkVisa(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "WORKER_VISA_MEDICAL";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
            var status = await _lovBusiness.GetSingle(x => x.Code == "WorkVisa");
            ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);
            await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "WorkVisa");
        }
        public async Task TriggerTravelling(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "TRAVEL";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);            
            ((IDictionary<String, Object>)exo).Add("ApplicationStateId", udf.ApplicationStateId);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);
            
        }
        public async Task TriggerVisaTransfer(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "VISA_TRANSFER";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
            var status = await _lovBusiness.GetSingle(x => x.Code == "VisaTransfer");
            ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);
            await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "VisaTransfer");
        }
        public async Task TriggerWorkPermit(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "LOCAL_WORK_PERMIT";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
            var status = await _lovBusiness.GetSingle(x => x.Code == "WorkPermit");
            ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);
            await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "WorkPermit");
        }
        public async Task TriggerStaffJoining(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "STAFF_JOINING_FORMALITIES";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
            var status = await _lovBusiness.GetSingle(x => x.Code == "StaffJoined");
            ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);
            await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "StaffJoined");
        }
        public async Task TriggerWorkerJoining(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "WORKER_JOINING_FORMALITIES";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);
            var status = await _lovBusiness.GetSingle(x => x.Code == "WorkerJoined");
            ((IDictionary<String, Object>)exo).Add("ApplicationStateId", status.Id);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);
            await _recQueryBusiness.UpdateApplicationState(udf.ApplicationId, "WorkerJoined");
        }

        public async Task<IList<RecTaskViewModel>> GetRecTaskList(string search)
        {
            return await _recQueryBusiness.GetRecTaskList(search);
        }
        public async Task<List<RecBatchViewModel>> GetBatchHmData(string jobid, string orgId, string HmId, BatchTypeEnum type, string batchId)
        {
            var list = await _recQueryBusiness.GetBatchHmData(jobid, orgId, HmId, type, batchId);
            foreach (var item in list)
            {
                item.Evaluated = item.NoOfApplication - (item.NotShortlistByHM + item.ShortlistByHM + item.ConfirmInterview);
            }
            return list;
        }

        public async Task<string> CreateApplicationStatusTrack(string applicationId, string statusCode = null, string taskReferenceId = null)
        {
            try
            {
                var application = await _recQueryBusiness.GetAppDetailsById(applicationId);
                var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                ApplicationStateTrack.ApplicationId = application.Id;
                ApplicationStateTrack.ApplicationStateId = application.ApplicationState;
                if (statusCode.IsNullOrEmpty())
                {
                    var appStatus = await _repo.GetSingle<IdNameViewModel, LOV>(x => x.Id == application.ApplicationStatus);
                    if (appStatus != null)
                    {
                        ApplicationStateTrack.ApplicationStatusCode = appStatus.Code;
                    }
                    ApplicationStateTrack.ApplicationStatusId = application.ApplicationStatus;
                }
                else
                {
                    var appStatus = await _repo.GetSingle<IdNameViewModel, LOV>(x => x.Code == statusCode);
                    if (appStatus != null)
                    {
                        ApplicationStateTrack.ApplicationStatusId = appStatus.Id;
                    }
                    ApplicationStateTrack.ApplicationStatusCode = statusCode;
                }
                ApplicationStateTrack.TaskReferenceId = taskReferenceId;
                ApplicationStateTrack.ChangedBy = _repo.UserContext.UserId;
                ApplicationStateTrack.ChangedDate = DateTime.Now;

                var noteTemp = new NoteTemplateViewModel();
                noteTemp.ActiveUserId = _repo.UserContext.UserId;
                noteTemp.TemplateCode = "REC_APPLICATION_STATE_TRACK";
                noteTemp.DataAction = DataActionEnum.Create;

                var notemodel = await _noteBusiness.GetNoteDetails(noteTemp);

                notemodel.Json = JsonConvert.SerializeObject(ApplicationStateTrack);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";                
                var result = await _noteBusiness.ManageNote(notemodel);

                //var result2 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                if (result.IsSuccess)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
                
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        public async Task<IList<RecApplicationViewModel>> GetBatchData(string batchid)
        {
            return await _recQueryBusiness.GetBatchData(batchid);
        }

        public async Task<IList<RecApplicationViewModel>> GetWorkerPoolBatchData(string batchid)
        {
            return await _recQueryBusiness.GetWorkerPoolBatchData(batchid);
        }
        public async Task<IdNameViewModel> GetCountrybyId(string id)
        {
            return await _recQueryBusiness.GetCountrybyId(id);
        }

        public async Task<RecApplicationViewModel> GetApplicationForJobAdv(string applicationId)
        {
            return await _recQueryBusiness.GetApplicationForJobAdv(applicationId);
        }
        public async Task<List<JobAdvertisementViewModel>> GetJobAdvertisementListByJobId(string jobId)
        {
            return await _recQueryBusiness.GetJobAdvertisementListByJobId(jobId);
        }
        public async Task<IList<JobAdvertisementViewModel>> GetSelectedJobAdvertisement(string vacancyId)
        {
            return await _recruitmentQueryBusiness.GetSelectedJobAdvertisement(vacancyId);
        }
        
        public async Task<IList<JobAdvertisementViewModel>> GetJobAdvList()
        {
            return await _recruitmentQueryBusiness.GetJobAdvList();
        }
        public async Task TriggerCRPFAppointment(ServiceTemplateViewModel viewModel, dynamic udf)
        {
            var serviceTempModel = new ServiceTemplateViewModel();

            serviceTempModel.ActiveUserId = viewModel.OwnerUserId;
            serviceTempModel.TemplateCode = "REC_CRPF_Appointment";
            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            servicemodel.OwnerUserId = viewModel.OwnerUserId;
            servicemodel.DataAction = DataActionEnum.Create;
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ApplicationId", udf.ApplicationId);

            servicemodel.Json = JsonConvert.SerializeObject(exo);
            await _serviceBusiness.ManageService(servicemodel);

        }
        public async Task<IList<RecApplicationViewModel>> GetJobApplicationList()
        {
            return await _recQueryBusiness.GetJobApplicationList();
        }
        public async Task<IList<IdNameViewModel>> GetExamCenter()
        {
            return await _recQueryBusiness.GetExamCenter();
        }
        public async Task<MedFitCertificateViewModel> GetMedicalFitnessData(string appId)
        {
            return await _recQueryBusiness.GetMedicalFitnessData(appId);
        }
        public async Task<IList<ApplicationViewModel>> GetResultData(string eId)
        {
            return await _recQueryBusiness.GetResultData(eId);
        }
    }
}
