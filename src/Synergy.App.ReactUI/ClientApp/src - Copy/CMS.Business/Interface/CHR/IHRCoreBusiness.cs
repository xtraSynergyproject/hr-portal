using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IHRCoreBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<OrganizationChartIndexViewModel> GetOrgHierarchyParentId(string employeeId);
        Task<List<OrganizationChartViewModel>> GetOrgHierarchy(string parentId, int levelUpto);
        Task<PositionChartIndexViewModel> GetPostionHierarchyParentId(string employeeId);
        Task<List<double>> GetPositionNodeLevel(string positiionId);
        Task<List<double>> GetDepartmentNodeLevel(string deptId);
        Task<List<PositionChartViewModel>> GetPositionHierarchy(string parentId, int levelUpto);
        Task<IdNameViewModel> GetJobNameById(string Id);
        Task<List<IdNameViewModel>> GetAllJobs();
        Task<List<IdNameViewModel>> GetCountryList();
        Task<List<UserListOfValue>> GetPersonDetailsForLOV(string searchParam, string includeId = null, bool includeIdOnly = false);
        Task<List<IdNameViewModel>> GetPersonListByLegalEntity();
        Task<List<IdNameViewModel>> GetAllOrganisation();
        Task<List<IdNameViewModel>> GetAllPosition();
        Task<IdNameViewModel> GetOrgNameById(string Id);
        Task<List<IdNameViewModel>> GetJobByDepartment(string DepartmentId);
        Task<IdNameViewModel> GetCompanyOrganization(string userId);
        Task<List<BannerViewModel>> GetSliderBannerData();
        Task<string> GenerateNextPositionName(string Name);
        Task<IList<IdNameViewModel>> GenerateNextPositionNo(string Name);
        Task<List<IdNameViewModel>> GetPositionHierarchyUsers(string parentId, int levelUpto);
        Task<List<AssignmentViewModel>> GetAssignmentDetails(string personId,string userId,string legalEntityId=null);
        Task<List<IdNameViewModel>> GetPersonList();
        Task<IList<IdNameViewModel>> GetUnAssignedPersonList(string personId);
        Task<IList<IdNameViewModel>> GetUnAssignedPositionList(string positionId, string departmentId, string jobId);
        Task<IList<IdNameViewModel>> GetUnAssignedContractPersionList(string personId);
        Task<IList<IdNameViewModel>> GetUnAssignedChildPositionList(string parentId,string positionId);
        Task<IList<IdNameViewModel>> GetUnAssignedChildDepartmentList(string parentId,string departmentId);
        Task<IList<IdNameViewModel>> GetUnAssignedUserList(string userId);
        Task<List<TeamViewModel>> GetUserTeamDetail(string userId);
        //Task<IList<PersonIDDocumentViewModel>> GetPersonIDDocuments(long personId);
        //Task<IList<PersonPassportDocumentViewModel>> GetPersonPassportDocuments(long personId);
        //Task<IList<PersonVisaDocumentViewModel>> GetPersonVisaDocuments(long personId);
        //Task<IList<PersonEducationDocumentViewModel>> GetPersonEducationDocuments(long personId);
        //Task<IList<PersonExperienceDocumentViewModel>> GetPersonExperienceDocuments(long personId);
        //Task<IList<PersonTrainingDocumentViewModel>> GetPersonTrainingDocuments(long personId);
        Task<AssignmentViewModel> GetUserFullInfo(string serviceId);
        Task<beneficiaryViewModel> GetBeneficiaryDEt(string Id);
        Task<AssignmentViewModel> GetPersonDetail(string userId);
        Task<IdNameViewModel> GetPersonDetailByUserId(string userId);
        Task<PersonViewModel> GetPersonDetailsById(string personId);
        Task<IList<PersonDocumentViewModel>> GetPersonRequestDocumentList(string userId);
        Task<AssignmentViewModel> GetLeaveBalance(string userId);
        Task<AssignmentViewModel> GetContractDetail(string personId);
        Task<IList<LeaveDetailViewModel>> GetLeaveDetail(string userId);
        Task<IList<PersonDocumentViewModel>> GetDependentDocumentList(string dependentId);
        Task<IList<DependentViewModel>> GetDependentList(string personId, string status);
        Task<IList<PersonDocumentViewModel>> GetDependentRequestDocumentList(string userId);
        Task<List<BusinessTripViewModel>> GetAllEmployee();
        Task<AccessLogViewModel> GetAccessLogData(string userId, PunchingTypeEnum punchingType);
        Task<List<BusinessTripViewModel>> GetBusinessTripbyOwneruserId(string Id);
        Task<List<MisconductViewModel>> GetMisconductDetails(string Id);
        Task<AccessLogViewModel> UpdateAccessLogDetail(string UserId, DateTime punchTime, PunchingTypeEnum punchingType, DeviceTypeEnum deviceType, string locationId);
        Task<IList<ResignationTerminationViewModel>> GetResignationTerminationList(string EmpId);
        Task<JobDetViewModel> GetJobDescription(string organization, string JobId);
        Task<AssignmentViewModel> GetUserLineManagerFromPerformanceHierarchy(string userId);
        Task<List<AssignmentViewModel>> GetUserPerformanceDocumentInfo(string userId,string PerformanceId,string MasterStageId);
        Task<List<AssignmentViewModel>> GetUserLetterTemplateDetails(string userId, string PerformanceId, string MasterStageId);
        Task<List<AssignmentViewModel>> GetUsersInfo(string deptId=null);
        Task<DateTime?> GetContractEndDateByUser(string userId);
        Task CreatePositionHierarchy(NoteTemplateViewModel viewModel);
        Task CreateDepartmentHierarchy(NoteTemplateViewModel viewModel);
        Task<IdNameViewModel> GetOrgByHierarchyAndOrg(string orgId, string hierarchyId);
        Task<IdNameViewModel> GetUserByHierarchyAndUser(string userId, string hierarchyId);
        Task<IdNameViewModel> GetPositionByHierarchyAndPosition(string positionId, string hierarchyId);
        Task<List<IdNameViewModel>> GetNonExistingDepartment(string hierarchyId, string deptId);
        Task<List<IdNameViewModel>> GetDepartmentWithParent(string hierarchyId, string deptId);
        Task<List<IdNameViewModel>> GetNonExistingPosition(string hierarchyId, string positionId);
        Task<List<IdNameViewModel>> GetPositionWithParent(string hierarchyId, string positionId);
        Task<PersonProfileViewModel> GetEmployeeProfile(string personId);
        Task<string> GetIdDialCodeById(string Id);
        Task TriggerEndOfService(ServiceTemplateViewModel viewModel);
        Task<List<IdNameViewModel>> GetAllLocation();
        Task TriggerClearanceForm(ServiceTemplateViewModel viewModel);
        Task<bool> ValidatePostMsgSequenceNo(PostMessageViewModel model);
        Task<ResignationTerminationViewModel> GetTerminationResignationDetailsbyid(string Notid, string Name);

        Task<DateTime?> GetCurrentAnniversaryStartDateByUserId(string userId);
        Task<bool> ValidateUserMappingToPerson(NoteTemplateViewModel viewModel);
        Task<bool> ValidateUniqueDepartment(NoteTemplateViewModel viewModel);
        Task<List<DepartmentViewModel>> GetDepartmentByName(string departmentName);
        Task<bool> ValidateLeaveStartDateandEndDate(ServiceTemplateViewModel viewModel);
        Task<IList<TimePermisssionViewModel>> GetTimePermissionDetailsList(string EmpId);      
        Task<JobDesriptionViewModel> GetHRJobDesciption(string JobId);

        Task<IdNameViewModel> GetParentOrgByOrg(string orgId);
        Task<List<HRJobCriteriaViewModel>> GetJobCriteriabyParentID(string ParentNoteId);


        Task<List<HRJobCriteriaViewModel>> GetJobSkillabyParentID(string ParentNoteId);
        Task<List<HRJobCriteriaViewModel>> GetJobOthernformationParentID(string ParentNoteId);

        void DeleteCriteria(String ParentNoteId, List<string> IDs = null);
        void DeleteOtherInformation(String ParentNoteId, List<string> IDs = null);

        void DeleteSkill(String ParentNoteId, List<string> IDs = null);


        Task<PunchingViewModel> GetPunchingDetails(SigninSignoutTypeEnum Type, String UserId, DateTime Attendancedate, String Hours);

        public void Updatesigninsingnout(SigninSignoutTypeEnum type, string Id, string Hours);
        Task<double> GetAirTicketCostByUser(string userId);
       // Task<IList<AccessLogViewModel>> GetAccessLogList(string Id);
        Task<IList<PositionViewModel>> GetPositionByJobId(string JobId);
        Task<IList<PositionViewModel>> GetPositionByDepartmentId(string DepartmentId);
         Task<IdNameViewModel> GetDepartmentNameById(string DepartmentId);
        Task UpdatePositionName(string name, string id);
        Task<IList<AccessLogViewModel>> GetAccessLogList(string Id,string UserId, string userIds = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<AccessLogViewModel>> GetAllAccessLogList(DateTime? startDate = null, DateTime? dueDate = null, string userId = null);
        
            Task<List<string>> GetParentOrganizationReportingList(string orgId, List<string> ids);
        Task<IList<NoteViewModel>> GetAnnouncements(List<string> orgList);
        Task<IList<NoteViewModel>> GetGroupMessage(EndlessScrollingRequest searchModel);
        Task<List<IdNameViewModel>> GetAllHierarchyUsers(string Userid, string positionid);

        Task<List<AttendanceViewModel>> GetAttendanceListByDate(string personId, DateTime? searchFromDate, DateTime? searchToDate,string UserId);
        Task<IdNameViewModel> GetPositionID(string Userid);
        Task<IList<NoteViewModel>> GetOrgGroupMessage(EndlessScrollingRequest searchModel);
        Task<string> GetAnnouncementTemplateMasterId();
        Task<AnnouncementViewModel> GetAnnouncementByNoteId(string Id);
        Task<PostMessageViewModel> GetGroupMessageByNoteId(string Id);
        Task DeleteGroupPost(string Id);
        Task UnlikePost(string Id, string userId);
        Task<List<RemoteSignInSignOutViewModel>> GetRemotesigninsignoutDetails(string Id);
        Task<IList<NoteViewModel>> GetCompanyGroupMessage(EndlessScrollingRequest searchModel);
        Task<long> GetLikeCount(string ParentNoteId);
        Task<long> GetILikeCount(string ParentNoteId, string userId);
        Task<List<WorkStructureViewModel>> GetWorkStructureDiagram(string personId);
        Task<List<AssignmentViewModel>> GetPersoneWorkStructureDetails(string personId);

        //Task<IList<NoteViewModel>> GetNoteSummaryDetail(NoteSearchViewModel searchModel);
        Task<IList<ManpowerLeaveSummaryViewModel>> GetLeaveRequestDetails(ManpowerLeaveSummaryViewModel searchModel);
        Task<TerminatePersonViewModel> GetPersonInfoForTermination(string personId);
        Task<CommandResult<TerminatePersonViewModel>> UpdatePersonForTermination(TerminatePersonViewModel person);
        Task<List<PersonViewModel>> GetAllPersonOnRoleBasisList(string typeId);
        Task<List<PersonViewModel>> GetAllPersonList(string typeId);
        Task<List<PersonViewModel>> GetAllPersonAgeList(string typeId);
        Task<List<PersonViewModel>> GetAllPersonByCountryList(string typeId);
        Task<List<PersonViewModel>> GetAllPersonSalaryList(string typeId);
        Task<List<IdNameViewModel>> GetPersonListByOrgId(string orgId);

        Task<IList<IdNameViewModel>> GetAllTaskOwnerList(string Category, string Template);

        Task<IList<IdNameViewModel>> GetAllTaskAssigneeList(string Category, string Template);

        Task<IList<IdNameViewModel>> GetCategory();

        Task<IList<TaskDetailsViewModel>> GetTaskDetailList(TaskDetailsViewModel Model);
        Task<IList<IdNameViewModel>> GetOwnerNameList(string Category, string Template);

        Task<IList<TaskDetailsViewModel>> GetServiceDetailsList(TaskDetailsViewModel Model);

        Task<IList<IdNameViewModel>> GetTemplatecategoryWiseList(string Category);
        Task<IList<IdNameViewModel>> GetTemplatecategoryWiseListService(string Category);

        Task<List<LeaveSummaryViewModel>> GetLeaveSummary(string userId);

        Task<List<IdNameViewModel>> GetSponsorList();
        Task<AssignmentViewModel> GetAssignmentByPerson(string personId);
        Task<IList<IdNameViewModel>> GetAssignedPersonList(string personId);
        Task<IList<IdNameViewModel>> GetDepartmentByPerson(string deptId, string personId);
        Task<IList<IdNameViewModel>> GetJobByPerson(string jobId, string personId);
        Task<IdNameViewModel> GetPersonLocationId(string PersonId);
        Task<List<UserHierarchyChartViewModel>> GetUserHierarchy(string parentId, int levelUpto,string hierarchyId);
        Task<List<double>> GetUserNodeLevel(string userHierarchyId, string hierarchyId);
        Task CreateUserHierarchy(NoteTemplateViewModel viewModel, string hierarchyId);
        Task<List<IdNameViewModel>> GetNonExistingUser(string hierarchyId, string userId);
         Task<bool> DeleteUserHierarchy(string NoteId);
        Task<List<IdNameViewModel>> GetUserWithParent(string hierarchyId, string parentuserId, string userId);
        Task<IList<IdNameViewModel>> GetDepartmentByType(string type, string level);
        Task<List<UserHierarchyChartViewModel>> GetBusinessHierarchy(string parentId, int levelUpto);
        Task<PersonViewModel> GetPersonDetailsByPersonId(string personId);
        Task<List<UserHierarchyChartViewModel>> GetObjectHierarchy(string parentId, int levelUpto, string hierarchyId, string permissions);

        Task BulkUploadEmployeeData(string attachId, string noteId);
        Task BulkUploadJobData(string attachId, string noteId);
        Task BulkUploadDepartmentData(string attachId, string noteId);
        Task BulkUploadAssignmentData(string attachId, string noteId);
        Task BulkUploadEmployeeAssignmentData(string attachId, string noteId);

        Task BulkUploadBusinessHierarchyData(string attachId, string noteId);

        Task<IList<DataUploadViewModel>> GetUploadData();
        Task<List<IdNameViewModel>> GetHRMasterList(string templateCode);

        Task<List<IdNameViewModel>> GetAllActivePersonList();
        Task<bool> UpdateDepartmentName(dynamic udf);
        Task<bool> UpdateDepartmentName(Dictionary<string, object> udf);
        Task<bool> UpdateJobName(dynamic udf);
        Task<bool> UpdateJobName(Dictionary<string, object> udf);
        Task<PositionViewModel> CreatePosition(string departmentId, string jobId);
        Task<CommandResult<DataUploadViewModel>> ValidateTemplate(DataUploadViewModel model);
        Task<ReimbursementRequestViewModel> GetReimbursementRequestItemData(string id);
        Task<List<ReimbursementRequestViewModel>> GetReimbursementRequestItemList(string reimbursementRequestItem);
        Task<bool> DeleteReimbursementRequestItem(string id);
        Task<ReimbursementRequestViewModel> CreateReimbursementRequestItem(ReimbursementRequestViewModel model);
        Task<ReimbursementRequestViewModel> UpdateReimbursementRequestItem(ReimbursementRequestViewModel model);
        Task<bool> UpdatePersonType(dynamic udf);
        Task<bool> UpdateAssignmentStatus(dynamic udf);
        Task<bool> UpdateUserStatus(dynamic udf);
        Task<bool> UpdateContract(dynamic udf);
        Task<bool> UpdatePersonDetails(dynamic udf);
        Task<bool> UpdateAssignmentDetails(dynamic udf);
        Task<bool> UpdateLineManager(dynamic udf);
        Task<PositionViewModel> GetPositionDetailsById(string Id);
        Task<bool> ValidateFinancialYearStartDateandEndDate(FormTemplateViewModel viewModel);
        Task CreateDepartment(NoteTemplateViewModel viewModel);
        Task CreateNewCareerLevel(NoteTemplateViewModel viewModel);
        Task CreateNewJob(NoteTemplateViewModel viewModel);
        Task CreateNewPosition(NoteTemplateViewModel viewModel);
        Task UpdatePersonJob(NoteTemplateViewModel viewModel);
        Task UpdatePersonDepartment(NoteTemplateViewModel viewModel);
        Task<CommandResult<NoteTemplateViewModel>> CreateNewPerson(NoteTemplateViewModel viewModel);
        Task<List<BusinessHierarchyAORViewModel>> GetBusinessPartnerMappingList();
        Task<List<BusinessHierarchyAORViewModel>> GetAllAORBusinessHierarchyList();
       
    }
}
