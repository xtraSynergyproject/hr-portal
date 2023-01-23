using AutoMapper;
using AutoMapper.Data;
using CMS.Business;
using CMS.Business.Interface;
using CMS.Business.Interface.DMS;
using CMS.Common;
//using CMS.Common;
using CMS.Data.Repository;
using CMS.Web.Scheduler.Data;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using CMS.Business.Interface.PortalAdmin;
using CMS.Business.Implementations.PortalAdmin;
//using Telerik.Reporting.Cache.File;
//using Telerik.Reporting.Services;
//using Telerik.Reporting.Services.AspNetCore;
//using Telerik.WebReportDesigner.Services;

namespace CMS.Web.Scheduler
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private void RegisterDependency(IServiceCollection services)
        {


            services.Add(new ServiceDescriptor(typeof(IRepositoryBase<,>), typeof(RepositoryPostgresBaseMigration<,>), ServiceLifetime.Scoped));
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
            services.Add(new ServiceDescriptor(typeof(IWorkboardBusiness), typeof(WorkboardBusiness), ServiceLifetime.Scoped));
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IWebHelper, WebHelper>();
            //services.AddSingleton<DmsMigration, DmsMigration>();
            services.AddScoped<DmsMigration, DmsMigration>();
            services.AddScoped<AssessmentMigration, AssessmentMigration>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddKendo();
            services.AddMvc(options => options.EnableEndpointRouting = false).AddRazorRuntimeCompilation();




            services.AddSignalR();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddEntityFrameworkNpgsql().AddDbContext<PostgreDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));

            services.AddDefaultIdentity<ApplicationIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddSignInManager<AuthSignInManager<ApplicationIdentityUser>>();
            services.AddControllersWithViews();


            var configuration = new MapperConfiguration(cfg =>
            {
                ConfigureMapper(cfg);
            });


            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);


            services.AddMvcCore().AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizeCMS", policy => policy.Requirements.Add(new AuthorizeCMS()));
            });

            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/LogIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(2);
                options.SlidingExpiration = true;
            });

            RegisterDependency(services);


            //services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfirePostgreConnection")));
            //services.AddHangfireServer();
        }
        protected virtual void ConfigureMapper(IMapperConfigurationExpression cfg)
        {
            cfg.AddDataReaderMapping(false);
            var profile = new MappingProfile();
            profile.AddDataRecordMember();
            cfg.AddProfile(profile);

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDE4MzQ5QDMxMzgyZTM0MmUzMGdla0UydThJVnAxeUxlQ2pPcFNLcStzemZoR0ZjQjNCWE05cFI0RDJiVWs9;NDE4MzUwQDMxMzgyZTM0MmUzMG8xZDJORWxpbFdtejZuOHBwVCtrem9BNFhyRTNlZDlwcmd1MVNjQ09vNFE9;NDE4MzUxQDMxMzgyZTM0MmUzMEJuZ2pGM2FJbCtoNmUyR0dCWjdqejh2ek1mc2ROekdQUTkraUM2eVFxUEk9;NDE4MzUyQDMxMzgyZTM0MmUzMGt6VzJGVWVaelJnYkh2dG9qL0xqNzE4OFJ1MVZjWUJrK28xVEpLZy85Mjg9;NDE4MzUzQDMxMzgyZTM0MmUzMERQeStlSkdwc3lucVNGbkdwelVWQzJSaVZjd2ZXWWh1dG1GSy9WY0Z5SXM9;NDE4MzU0QDMxMzgyZTM0MmUzME1jQlB5MUI3QkwyUE1hSStVV1F5aTBzRDk0SnYxemZEU2pWTSs0UHNEVG89;NDE4MzU1QDMxMzgyZTM0MmUzMGpTcE5sd21lOXBFTUNGWkIrd0lDUE5jem9WdVpJK2xzRmNQbkZFWktBcXc9;NDE4MzU2QDMxMzgyZTM0MmUzMEVwRTRQUlZqdW5iL2JwWXZvVGdrM3hOd1YvUlJ3UXQzR3BSV08rZTBSczQ9;NDE4MzU3QDMxMzgyZTM0MmUzMG5OZnlmN2dxeDdOZkltTlN2ZnZhYlI4dUc3c25tVjRjdWFJditINkpKbXM9;NDE4MzU4QDMxMzgyZTM0MmUzMFFXUVhQMlorVWYyV2VhVjlqSzlKV0kyREp2K2VDN1NJeVNvM3ozWUJOWWs9");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new HangfireAuthorizationFilter() }
            //});





            app.UseEndpoints(endpoints =>
            {
                //  endpoints.MapHangfireDashboard();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            // app.UseHangfireDashboard();

            try
            {
                RecurringJob.AddOrUpdate<CMS.Business.HangfireScheduler>(x => x.UpdateJobAdvertisementStatus(), Cron.Daily);
            }
            catch (Exception ex)
            {

            }

            try
            {
                RecurringJob.AddOrUpdate<CMS.Business.HangfireScheduler>(x => x.UpdateNtsStatus(), "0 */4 * * *");
            }
            catch (Exception ex)
            {

            }
            //try
            //{
            //    RecurringJob.AddOrUpdate<CMS.Business.HangfireScheduler>(x => x.SendEmailSummary(), Cron.Daily(4));
            //}
            //catch (Exception ex)
            //{

            //}
            try
            {
                RecurringJob.AddOrUpdate<CMS.Business.HangfireScheduler>(x => x.UpdateReceiveEmailForProjectManagement(), Cron.MinuteInterval(5));
            }
            catch (Exception ex)
            {

            }

        }
    }
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.Identity.IsAuthenticated;
        }
    }

}
