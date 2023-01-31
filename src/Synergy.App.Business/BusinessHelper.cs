
using AutoMapper;
using Synergy.App.Business.Implementations.BusinessQueryPostgre;
using Synergy.App.Business.Implementations.PortalAdmin;
using Synergy.App.Business.Interface;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Business.Interface.PortalAdmin;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Synergy.App.Business
{
    public class BusinessHelper
    {
        public static void RegisterDependency(IServiceCollection services)
        {


            services.Add(new ServiceDescriptor(typeof(IRepositoryBase<,>), typeof(RepositoryPostgresBase<,>), ServiceLifetime.Scoped));
            //services.Add(new ServiceDescriptor(typeof(IRepositoryQueryBase<>), typeof(RepositoryPostgresQueryBase<>), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRepositoryQueryBase<>), typeof(RepositoryPostgresDapperQueryBase<>), ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(IBusinessBase<,>), typeof(BusinessBase<,>), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessQueryBase<>), typeof(BusinessQueryBase<>), ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(ISettingsBusiness), typeof(SettingsBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IDocumentTypeBusiness), typeof(DocumentTypeBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICompositionBusiness), typeof(CompositionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IEditorTypeBusiness), typeof(EditorTypeBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICmsBusiness), typeof(CmsBusiness), ServiceLifetime.Scoped));



            services.Add(new ServiceDescriptor(typeof(IBusinessAreaBusiness), typeof(BusinessAreaBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBreMetadataBusiness), typeof(BreMetadataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessRuleNodeBusiness), typeof(BusinessRuleNodeBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessRuleConnectorBusiness), typeof(BusinessRuleConnectorBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserContext), typeof(UserContext), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserBusiness), typeof(UserBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICompanyBusiness), typeof(CompanyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICompanySettingBusiness), typeof(CompanySettingBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessRuleModelBusiness), typeof(BusinessRuleModelBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessRuleBusiness), typeof(BusinessRuleBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessDataBusiness), typeof(BusinessDataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessSectionBusiness), typeof(BusinessSectionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessRuleGroupBusiness), typeof(BusinessRuleGroupBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IDocumentBusiness), typeof(DocumentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBreMasterMetadataBusiness), typeof(BreMasterMetadataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBreResultBusiness), typeof(BreResultBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPageBusiness), typeof(PageBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPortalBusiness), typeof(PortalBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IStyleBusiness), typeof(StyleBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPageContentBusiness), typeof(PageContentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IMemberBusiness), typeof(MemberBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IMemberGroupBusiness), typeof(MemberGroupBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPageMemberBusiness), typeof(PageMemberBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPageMemberGroupBusiness), typeof(PageMemberGroupBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPublishPageBusiness), typeof(PublishPageBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPublishPageContentBusiness), typeof(PublishPageContentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPermissionBusiness), typeof(PermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserPermissionBusiness), typeof(UserPermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRolePermissionBusiness), typeof(UserRolePermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRoleBusiness), typeof(UserRoleBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IModuleBusiness), typeof(ModuleBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISubModuleBusiness), typeof(SubModuleBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IMenuGroupBusiness), typeof(MenuGroupBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPageIndexColumnBusiness), typeof(PageIndexColumnBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPageIndexBusiness), typeof(PageIndexBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPageTemplateBusiness), typeof(PageTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IFormTemplateBusiness), typeof(FormTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INoteTemplateBusiness), typeof(NoteTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITaskTemplateBusiness), typeof(TaskTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IServiceTemplateBusiness), typeof(ServiceTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICustomTemplateBusiness), typeof(CustomTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IFormIndexPageTemplateBusiness), typeof(FormIndexPageTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITemplateCategoryBusiness), typeof(TemplateCategoryBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITemplateBusiness), typeof(TemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITableMetadataBusiness), typeof(TableMetadataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IColumnMetadataBusiness), typeof(ColumnMetadataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IFormIndexPageColumnBusiness), typeof(FormIndexPageColumnBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITeamBusiness), typeof(TeamBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRoleUserBusiness), typeof(UserRoleUserBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INotificationTemplateBusiness), typeof(NotificationTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IProcessDesignBusiness), typeof(ProcessDesignBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITeamUserBusiness), typeof(TeamUserBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IProcessDesignVariableBusiness), typeof(ProcessDesignVariableBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IStepTaskComponentBusiness), typeof(StepTaskComponentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IAdhocTaskBusiness), typeof(AdhocTaskBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INoteIndexPageTemplateBusiness), typeof(NoteIndexPageTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INoteIndexPageColumnBusiness), typeof(NoteIndexPageColumnBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITaskIndexPageColumnBusiness), typeof(TaskIndexPageColumnBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IServiceIndexPageColumnBusiness), typeof(ServiceIndexPageColumnBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITaskIndexPageTemplateBusiness), typeof(TaskIndexPageTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IServiceIndexPageTemplateBusiness), typeof(ServiceIndexPageTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IHierarchyMasterBusiness), typeof(HierarchyMasterBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserHierarchyBusiness), typeof(UserHierarchyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IManpowerSummaryCommentBusiness), typeof(ManPowerSummaryCommentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IManpowerRecruitmentSummaryBusiness), typeof(ManpowerRecruitmentSummaryBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationBusiness), typeof(ApplicationBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBatchBusiness), typeof(BatchBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateEducationalBusiness), typeof(CandidateEducationalBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IJobAdvertisementBusiness), typeof(JobAdvertisementBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IJobCriteriaBusiness), typeof(JobCriteriaBusiness), ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(ICandidateProfileBusiness), typeof(CandidateProfileBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IFileBusiness), typeof(FileBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateExperienceBusiness), typeof(CandidateExperienceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateExperienceByCountryBusiness), typeof(CandidateExperienceByCountryBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateExperienceByNatureBusiness), typeof(CandidateExperienceByNatureBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateExperienceBySectorBusiness), typeof(CandidateExperienceBySectorBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IListOfValueBusiness), typeof(ListOfValueBusiness), ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(IJobAdvertisementTrackBusiness), typeof(JobAdvertisementTrackBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateExperienceByJobBusiness), typeof(CandidateExperienceByJobBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateComputerProficiencyBusiness), typeof(CandidateComputerProficiencyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPushNotificationBusiness), typeof(PushNotificationBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRecEmailBusiness), typeof(RecEmailBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateLanguageProficiencyBusiness), typeof(CandidateLanguageProficiencyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateReferencesBusiness), typeof(CandidateReferencesBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateDrivingLicenseBusiness), typeof(CandidateDrivingLicenseBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateProjectBusiness), typeof(CandidateProjectBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IMasterBusiness), typeof(MasterBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationExperienceByNatureBusiness), typeof(ApplicationExperienceByNatureBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationExperienceBusiness), typeof(ApplicationExperienceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationExperienceByCountryBusiness), typeof(ApplicationExperienceByCountryBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationExperienceByJobBusiness), typeof(ApplicationExperienceByJobBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationExperienceBySectorBusiness), typeof(ApplicationExperienceBySectorBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationProjectBusiness), typeof(ApplicationProjectBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationEducationalBusiness), typeof(ApplicationEducationalBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationComputerProficiencyBusiness), typeof(ApplicationComputerProficiencyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationLanguageProficiencyBusiness), typeof(ApplicationLanguageProficiencyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationDrivingLicenseBusiness), typeof(ApplicationDrivingLicenseBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationReferencesBusiness), typeof(ApplicationReferencesBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationBeneficaryBusiness), typeof(ApplicationBeneficaryBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationStateCommentBusiness), typeof(ApplicationStateCommentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateEvaluationBusiness), typeof(CandidateEvaluationBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IAppointmentApprovalRequestBusiness), typeof(AppointmentApprovalRequestBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRecruitmentElementBusiness), typeof(RecruitmentElementBusiness), ServiceLifetime.Scoped));
            //services.Add(new ServiceDescriptor(typeof(ITaskBusiness), typeof(TaskBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICandidateExperienceByOtherBusiness), typeof(CandidateExperienceByOtherBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INotificationBusiness), typeof(NotificationBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IEmailBusiness), typeof(EmailBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationExperienceByOtherBusiness), typeof(ApplicationExperienceByOtherBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRecTaskBusiness), typeof(RecTaskBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRecTaskTemplateBusiness), typeof(RecTaskTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserPortalBusiness), typeof(UserPortalBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRolePortalBusiness), typeof(UserRolePortalBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IHiringManagerBusiness), typeof(HiringManagerBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IAgencyBusiness), typeof(AgencyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationJobCriteriaBusiness), typeof(ApplicationJobCriteriaBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserDataPermissionBusiness), typeof(UserDataPermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRoleDataPermissionBusiness), typeof(UserRoleDataPermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ILOVBusiness), typeof(LOVBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IJobDescriptionBusiness), typeof(JobDescriptionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsBusiness), typeof(NtsBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ILegalEntityBusiness), typeof(LegalEntityBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IDecisionScriptComponentBusiness), typeof(DecisionScriptComponentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IComponentBusiness), typeof(ComponentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IExecutionScriptBusiness), typeof(ExecutionScriptBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserPagePreferenceBusiness), typeof(UserPagePreferenceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IHeadOfDepartmentBusiness), typeof(HeadOfDepartmentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsTaskSharedBusiness), typeof(NtsTaskSharedBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsTaskCommentBusiness), typeof(NtsTaskCommentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsServiceCommentBusiness), typeof(NtsServiceCommentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRoleStageChildBusiness), typeof(UserRoleStageChildBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRoleStageParentBusiness), typeof(UserRoleStageParentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INoteBusiness), typeof(NoteBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITaskBusiness), typeof(TaskBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IServiceBusiness), typeof(ServiceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsServiceSharedBusiness), typeof(NtsServiceSharedBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IProjectManagementBusiness), typeof(ProjectManagementBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsNoteSharedBusiness), typeof(NtsNoteSharedBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsNoteCommentBusiness), typeof(NtsNoteCommentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsNoteUserReactionBusiness), typeof(NtsNoteUserReactionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUdfPermissionBusiness), typeof(UdfPermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsTaskTimeEntryBusiness), typeof(NtsTaskTimeEntryBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsTaskPrecedenceBusiness), typeof(NtsTaskPrecedenceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IComponentResultBusiness), typeof(ComponentResultBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IProcessDesignResultBusiness), typeof(ProcessDesignResultBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IJobDescriptionCriteriaBusiness), typeof(JobDescriptionCriteriaBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IDynamicScriptBusiness), typeof(DynamicScriptBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IHRCoreBusiness), typeof(HRCoreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBreMasterTableMetadataBusiness), typeof(BreMasterTableMetadataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBreMasterColumnMetadataBusiness), typeof(BreMasterColumnMetadataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IProjectEmailSetupBusiness), typeof(ProjectEmailSetupBusiness), ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(IUserGroupBusiness), typeof(UserGroupBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserGroupUserBusiness), typeof(UserGroupUserBusiness), ServiceLifetime.Scoped));



            services.Add(new ServiceDescriptor(typeof(INtsGroupTemplateBusiness), typeof(NtsGroupTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsGroupUserGroupBusiness), typeof(NtsGroupUserGroupBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsGroupBusiness), typeof(NtsGroupBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBusinessDiagramBusiness), typeof(BusinessDiagramBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserSetBusiness), typeof(UserSetBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPayrollBatchBusiness), typeof(PayrollBatchBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPayrollRunBusiness), typeof(PayrollRunBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IGrantAccessBusiness), typeof(GrantAccessBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPerformanceManagementBusiness), typeof(PerformanceManagementBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPayrollTransactionsBusiness), typeof(PayrollTransactionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPayrollRunResultBusiness), typeof(PayrollRunResultBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IResourceLanguageBusiness), typeof(ResourceLanguageBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsTagBusiness), typeof(NtsTagBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IDocumentPermissionBusiness), typeof(DocumentPermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ILeaveBalanceSheetBusiness), typeof(LeaveBalanceSheetBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserHierarchyPermissionBusiness), typeof(UserHierarchyPermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRoleHierarchyPermissionBusiness), typeof(UserRoleHierarchyPermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISalaryEntryBusiness), typeof(SalaryEntryBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPayrollElementBusiness), typeof(PayrollElementBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IAttendanceBusiness), typeof(AttendanceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRosterScheduleBusiness), typeof(RosterScheduleBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserEntityPermissionBusiness), typeof(UserEntityPermissionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISalaryInfoBusiness), typeof(SalaryInfoBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserInfoBusiness), typeof(UserInfoBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPayrollBusiness), typeof(PayrollBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IOrganizationDocumentBusiness), typeof(OrganizationDocumentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IExcelReportBusiness), typeof(ExcelReportBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISuccessionPlanningBusiness), typeof(SuccessionPlanningBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITalentAssessmentBusiness), typeof(TalentAssessmentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITicketAssessmentBusiness), typeof(TicketManagementBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ILearningBusiness), typeof(LearningBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITemplateStageBusiness), typeof(TemplateStageBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IDMSDocumentBusiness), typeof(DMSDocumentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IMenuGroupDetailsBusiness), typeof(MenuGroupDetailsBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPageDetailsBusiness), typeof(PageDetailsBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUdfPermissionSettingsBusiness), typeof(UdfPermissionSettingsBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsNoteCommentUserBusiness), typeof(NtsNoteCommentUserBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsTaskCommentUserBusiness), typeof(NtsTaskCommentUserBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsLogPageColumnBusiness), typeof(NtsLogPageColumnBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPropertyBusiness), typeof(PropertyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICustomIndexPageColumnBusiness), typeof(CustomIndexPageColumnBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICustomIndexPageTemplateBusiness), typeof(CustomIndexPageTemplateBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IEGovernanceBusiness), typeof(EGovernanceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserReportBusiness), typeof(UserReportBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserPromotionBusiness), typeof(UserPromotionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IInventoryManagementBusiness), typeof(InventoryManagementBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationDocumentBusiness), typeof(ApplicationDocumentBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationErrorBusiness), typeof(ApplicationErrorBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IHybridHierarchyBusiness), typeof(HybridHierarchyBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IApplicationAccessBusiness), typeof(ApplicationAccessBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IOCRMappingBusiness), typeof(OCRMappingBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsStagingBusiness), typeof(NtsStagingBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IWorkboardBusiness), typeof(WorkboardBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IProjectWorkboardBusiness), typeof(ProjectWorkboardBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISmartCityBusiness), typeof(SmartCityBusiness), ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(IAssessmentQueryBusiness), typeof(AssessmentQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBreQueryBusiness), typeof(BreQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IClockServerQueryBusiness), typeof(ClockServerQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IEGovernanceQueryBusiness), typeof(EGovernanceQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IInventoryManagementQueryBusiness), typeof(InventoryManagementQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ILearningManagementQueryBusiness), typeof(LearningManagementQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPayRollQueryBusiness), typeof(PayRollQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IProjectManagementQueryBusiness), typeof(ProjectManagementQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISuccessionPlanningQueryBusiness), typeof(SuccessionPlanningQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPropertyQueryBusiness), typeof(PropertyQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IPerformanceManagementQueryBusiness), typeof(PerformanceManagementQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(INtsQueryBusiness), typeof(NtsQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITicketManagementQueryBusiness), typeof(TicketManagementQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITaaQueryBusiness), typeof(TaaQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ITalentAssessmentQueryBusiness), typeof(TalentAssessmentQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IHRCoreQueryBusiness), typeof(HRCoreQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IDocumentManagementQueryBusiness), typeof(DocumentManagementQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICmsQueryBusiness), typeof(CmsQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISalesBusiness), typeof(SalesBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISalesQueryBusiness), typeof(SalesQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRecruitmentTransactionBusiness), typeof(RecruitmentTransactionBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICareerPortalBusiness), typeof(CareerPortalBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRecruitmentQueryBusiness), typeof(RecruitmentQueryPostgreBusiness), ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(IRecQueryBusiness), typeof(RecQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IIipQueryBusiness), typeof(IipQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IIipBusiness), typeof(IipBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICommonServiceBusiness), typeof(CommonServiceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICommonServiceQueryBusiness), typeof(CommonServiceQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBLSBusiness), typeof(BLSBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IBLSQueryBusiness), typeof(BLSQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserPreferenceBusiness), typeof(UserPreferenceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IUserRolePreferenceBusiness), typeof(UserRolePreferenceBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IQRCodeBusiness), typeof(QRCodeBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IStepTaskEscalationBusiness), typeof(StepTaskEscalationBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IStepTaskEscalationDataBusiness), typeof(StepTaskEscalationDataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICaseManagementBusiness), typeof(CaseManagementBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ISmartCityQueryBusiness), typeof(SmartCityQueryPostgreBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRuntimeWorkflowBusiness), typeof(RuntimeWorkflowBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IRuntimeWorkflowDataBusiness), typeof(RuntimeWorkflowDataBusiness), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IHangfireScheduler), typeof(HangfireScheduler), ServiceLifetime.Scoped));
        }

        public static object ConvertToDbValue(object s, bool isSystemColumn, DataColumnTypeEnum dataType)
        {

            if (s == null || Convert.ToString(s) == "")
            {
                return $"null";
            }

            else if (!isSystemColumn)
            {
                var udf = Convert.ToString(s);
                if (udf.Contains("\"storage\":") && udf.Contains("["))
                {
                    udf = udf.Replace("[", "").Replace("]", "");
                    var data = (JObject)JsonConvert.DeserializeObject(udf);                    
                    if (!data["id"].IsNotNull())
                    {
                        //var a = data.First.ToString();
                        //var b = (JObject)JsonConvert.DeserializeObject(data.First.First.ToString());
                        if (data.First.First.IsNotNull())
                        {
                            var griddata = (JObject)JsonConvert.DeserializeObject(data.First.First.ToString());
                            if (griddata["id"].IsNotNull())
                            {
                                string gridfileId = griddata["id"].Value<string>();
                                return @$"'{gridfileId}'";
                            }
                           
                        }
                       
                    }
                    else
                    {
                        string fileId = data["id"].Value<string>();
                        return @$"'{fileId}'";
                    }
                   

                }
                else
                {
                    if (dataType == DataColumnTypeEnum.DateTime && s.ToString().Contains("Invalid date"))
                    {
                        return $"null";
                    }
                    else if (dataType == DataColumnTypeEnum.Text)
                    {
                        return @$"'{udf.Replace("'", "''")}'";
                    }
                    else if (dataType == DataColumnTypeEnum.TextArray)
                    {
                        udf = udf.Replace("\r", "");
                        udf = udf.Replace("\n", "");
                        return $"'{udf}'";
                    }
                    return $"'{s}'";
                }

            }
            switch (dataType)
            {
                case DataColumnTypeEnum.Bool:
                    return @$"{(((bool)s) ? "true" : "false")}";
                case DataColumnTypeEnum.DateTime:
                    return @$"'{((DateTime)s).ToDatabaseDateFormat()}'";
                case DataColumnTypeEnum.TextArray:
                    var array = (string[])s;
                    var text = "";
                    foreach (var item in array)
                    {
                        text = @$"{text}""{item}"",";
                    }
                    text = $"'{{{text.Trim(',')}}}'";
                    return text;
                case DataColumnTypeEnum.Text:
                    var udf = Convert.ToString(s);
                    if (udf.Contains("\"storage\":") && udf.Contains("["))
                    {
                        udf = udf.Replace("[", "").Replace("]", "");
                        var data = (JObject)JsonConvert.DeserializeObject(udf);
                        string fileId = data["id"].Value<string>();
                        return @$"'{fileId}'";

                    }
                    else
                    {

                        return @$"'{s}'";
                    }

                default:
                    return s;
            }
        }
        public static bool IsEmailPublicDomain(string email)
        {

            var domain = email.Split('@');
            if (domain.Length == 2)
            {
                var domainName = domain[1].ToLower();
                if (domainName.Contains("gmail") || domainName.Contains("yahoo") || domainName.Contains("hotmail")
                    || domainName.Contains("redmail") || domainName.Contains("outlook")
                    || domainName.Contains("inbox") || domainName.Contains("icloud")
                    || domainName.Contains("ymail")
                    )
                {
                    return true;
                }
                return false;
            }
            return true;
        }


    }
    public static class BusinessExtension
    {
        public static string GetLanguageValue(this ResourceLanguageViewModel s, string cultureName)
        {
            switch (cultureName)
            {
                case ApplicationConstant.LanguageCode.Arabic:
                    return s.Arabic;
                case ApplicationConstant.LanguageCode.Hindi:
                    return s.Hindi;
                case ApplicationConstant.LanguageCode.English:
                default:
                    return s.English;
            }
        }
        public static ApplicationIdentityUser ToIdentityUser(this IUserContext userContext)
        {
            return new ApplicationIdentityUser
            {
                Id = userContext.UserId,
                UserName = userContext.Name,
                IsSystemAdmin = userContext.IsSystemAdmin,
                Email = userContext.Email,
                UserUniqueId = userContext.Email,
                CompanyId = userContext.CompanyId,
                CompanyCode = userContext.CompanyCode,
                CompanyName = userContext.CompanyName,
                JobTitle = userContext.JobTitle,
                PhotoId = userContext.PhotoId,
                UserRoleCodes = userContext.UserRoleCodes,
                UserRoleIds = userContext.UserRoleIds,
                IsGuestUser = userContext.IsGuestUser,
                UserPortals = userContext.UserPortals,
                PortalTheme = userContext.PortalTheme,
                PortalId = userContext.PortalId,
                LegalEntityId = userContext.LegalEntityId,
                LegalEntityCode = userContext.LegalEntityCode,
                PersonId = userContext.PersonId,
                PositionId = userContext.PositionId,
                DepartmentId = userContext.OrganizationId,
                PortalName = userContext.PortalName,
            };
        }
        public static string ToStyleText(this StyleViewModel s)
        {
            var text = string.Concat(
                s.BackgroundColor.ToHtmlStyle("background-color"),
                s.PaddingDefault.ToHtmlStyle("padding"),
                s.PaddingLeft.ToHtmlStyle("padding-left"),
                s.PaddingTop.ToHtmlStyle("padding-top"),
                s.PaddingRight.ToHtmlStyle("padding-right"),
                s.PaddingBottom.ToHtmlStyle("padding-bottom"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background"),
                s.BackgroundColor.ToHtmlStyle("background")
            );


            return text;
        }
        public static string ToHtmlStyle(this string s, string styleName)
        {
            if (!s.IsNullOrEmpty())
            {
                return string.Concat(styleName, ":", s, ";");
            }
            return string.Empty;
        }

        //public static string GetHybridHierarchyReferenceType(this int levelId)
        //{
        //    if (levelId == 1)
        //    {
        //        return "OrgLevel1";
        //    }
        //    else if (levelId == 2)
        //    {
        //        return "OrgLevel2";
        //    }
        //    else if (levelId == 3)
        //    {
        //        return "OrgLevel3";
        //    }
        //    else if (levelId == 4)
        //    {
        //        return "OrgLevel4";
        //    }
        //    else if (levelId == 5)
        //    {
        //        return "Brand";
        //    }
        //    else if (levelId == 6)
        //    {
        //        return "Market";
        //    }

        //    else if (levelId == 7)
        //    {
        //        return "Province";
        //    }
        //    else if (levelId == 8)
        //    {
        //        return "CareerLevel";
        //    }
        //    else if (levelId == 9)
        //    {
        //        return "Department";
        //    }
        //    else if (levelId == 10)
        //    {
        //        return "Job";
        //    }
        //    else if (levelId == 11)
        //    {
        //        return "Employee";
        //    }
        //    else
        //    {
        //        return "Root";
        //    }
        //}
        public static string GetHybridHierarchyReferenceType(this string refType, string templateCode = null)
        {
            if (templateCode == "NEW_POSITION_CREATE" || templateCode == "HRPosition")
            {
                return "POSITION";
            }
            else if (templateCode == "EMPLOYEE_CREATE" || templateCode == "HRPerson")
            {
                return "EMPLOYEE";
            }
            switch (refType)
            {
                case "ROOT":
                    return "LEVEL1";
                case "LEVEL1":
                    return "LEVEL2";
                case "LEVEL2":
                    return "LEVEL3";
                case "LEVEL3":
                    return "LEVEL4";
                case "LEVEL4":
                    return "BRAND";
                case "BRAND":
                    return "MARKET";
                case "MARKET":
                    return "PROVINCE";
                case "PROVINCE":
                    return "DEPARTMENT";
                case "DEPARTMENT":
                    return "CAREER_LEVEL";
                case "CAREER_LEVEL":
                    return "JOB";
                case "Position":
                    return "POSITION";

                default:
                    return "DEPARTMENT";

            }

        }
    }
}
