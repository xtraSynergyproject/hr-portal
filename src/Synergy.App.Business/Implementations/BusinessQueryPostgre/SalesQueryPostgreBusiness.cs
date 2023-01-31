using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Hangfire;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Synergy.App.Business
{
    public class SalesQueryPostgreBusiness : BusinessBase<ServiceViewModel, NtsService>, ISalesQueryBusiness
    {
        private readonly IRepositoryQueryBase<DirectSalesViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<ProgramDashboardViewModel> _queryPDRepo;
        private readonly IRepositoryQueryBase<ProjectGanttTaskViewModel> _queryGantt;
        private readonly IRepositoryQueryBase<TeamWorkloadViewModel> _queryTWRepo;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryDCRepo;
        private readonly IRepositoryQueryBase<PerformanceDashboardViewModel> _queryProjDashRepo;
        private readonly IRepositoryQueryBase<ProjectDashboardChartViewModel> _queryProjDashChartRepo;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryTaskRepo;
        private readonly IRepositoryQueryBase<MailViewModel> _queryMailTaskRepo;
        private readonly IRepositoryQueryBase<PerformanceDocumentViewModel> _queryPerDoc;
        private readonly IRepositoryQueryBase<PerformanceDocumentStageViewModel> _queryPerDocStage;
        private readonly IRepositoryQueryBase<GoalViewModel> _queryGoal;
        private readonly IRepositoryQueryBase<NoteTemplateViewModel> _queryNoteTemplate;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IUserContext _userContext;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IStepTaskComponentBusiness _stepCompBusiness;
        private readonly IRepositoryQueryBase<PerformaceRatingViewModel> _queryPerformanceRatingRepo;
        private readonly IRepositoryQueryBase<PerformanceRatingItemViewModel> _queryPerformanceRatingitemRepo;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IRepositoryQueryBase<CompetencyViewModel> _queryComp;

        private readonly IRepositoryQueryBase<CompetencyCategoryViewModel> _queryCompeencyCategory;
        public SalesQueryPostgreBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IRepositoryQueryBase<DirectSalesViewModel> queryRepo,
            IRepositoryQueryBase<ProgramDashboardViewModel> queryPDRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<DashboardCalendarViewModel> queryDCRepo,
            IRepositoryQueryBase<ProjectGanttTaskViewModel> queryGantt,
             IRepositoryQueryBase<TeamWorkloadViewModel> queryTWRepo,
             IRepositoryQueryBase<PerformanceDashboardViewModel> queryProjDashRepo,
             IRepositoryQueryBase<ProjectDashboardChartViewModel> queryProjDashChartRepo,
             IRepositoryQueryBase<TaskViewModel> queryTaskRepo, INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness, ITableMetadataBusiness tableMetadataBusiness,
            IMapper autoMapper, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, IServiceBusiness serviceBusiness, IRepositoryQueryBase<MailViewModel> queryMailTaskRepo,
            IRepositoryQueryBase<PerformanceDocumentViewModel> queryPerDoc, IRepositoryQueryBase<GoalViewModel> queryGoal, IRepositoryQueryBase<PerformanceDocumentStageViewModel> queryPerDocStage
            , IHRCoreBusiness hrCoreBusiness, IUserContext userContext, IRepositoryQueryBase<NoteTemplateViewModel> queryNoteTemplate, IComponentResultBusiness componentResultBusiness, ILOVBusiness lovBusiness
            , ITemplateBusiness templateBusiness, IStepTaskComponentBusiness stepCompBusiness, IRepositoryQueryBase<PerformaceRatingViewModel> queryPerformaceRating,
            IRepositoryQueryBase<PerformanceRatingItemViewModel> queryPerformaceRatingitem, IRepositoryQueryBase<CompetencyViewModel> queryComp, ICmsBusiness cmsBusiness, IRepositoryQueryBase<CompetencyCategoryViewModel> queryComptetencyCategory) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _queryPDRepo = queryPDRepo;
            _queryDCRepo = queryDCRepo;
            _queryGantt = queryGantt;
            _queryTWRepo = queryTWRepo;
            _queryProjDashRepo = queryProjDashRepo;
            _queryProjDashChartRepo = queryProjDashChartRepo;
            _queryTaskRepo = queryTaskRepo;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _queryMailTaskRepo = queryMailTaskRepo;
            _queryPerDoc = queryPerDoc;
            _queryPerDocStage = queryPerDocStage;
            _hrCoreBusiness = hrCoreBusiness;
            _componentResultBusiness = componentResultBusiness;
            _queryGoal = queryGoal;
            _userContext = userContext;
            _queryNoteTemplate = queryNoteTemplate;
            _lovBusiness = lovBusiness;
            _templateBusiness = templateBusiness;
            _stepCompBusiness = stepCompBusiness;
            _queryComp = queryComp;
            _queryPerformanceRatingRepo = queryPerformaceRating;
            _queryPerformanceRatingitemRepo = queryPerformaceRatingitem;
            _cmsBusiness = cmsBusiness;
            _queryCompeencyCategory = queryComptetencyCategory;
        }
        public async override Task<CommandResult<ServiceViewModel>> Create(ServiceViewModel model, bool autoCommit = true)
        {

            return CommandResult<ServiceViewModel>.Instance();
        }
        public async override Task<CommandResult<ServiceViewModel>> Edit(ServiceViewModel model, bool autoCommit = true)
        {
            return CommandResult<ServiceViewModel>.Instance();
        }

        public async Task<LicenseViewModel> GenerateLicense(string CustomerId,string ProductId,string MachineName)
        {
            //var cypher = @"match (n:NTS_Note)<-[:R_NoteFieldValue_Note]-(fvc: NTS_NoteFieldValue{ IsDeleted: 0,Code:{Customer}})
            //-[:R_NoteFieldValue_TemplateField]->(tfc:NTS_TemplateField{ IsDeleted: 0,FieldName:'customer'})
            //match (n)<-[:R_NoteFieldValue_Note]-(fvp: NTS_NoteFieldValue{ IsDeleted: 0,Code:{Product}})
            //-[:R_NoteFieldValue_TemplateField]->(tfp:NTS_TemplateField{ IsDeleted: 0,FieldName:'product'})
            //match (n)<-[:R_NoteFieldValue_Note]-(fvm: NTS_NoteFieldValue{ IsDeleted: 0,Code:{MachineName}})
            //-[:R_NoteFieldValue_TemplateField]->(tfm:NTS_TemplateField{ IsDeleted: 0,FieldName:'machineName'})
            //return n limit 1";
            var query = $@"select sale.*,sale.""NtsNoteId"" as NoteId
                from public.""NtsNote"" as n
                join cms.""N_SYN_SALE_ApplicationLicense"" as sale on sale.""NtsNoteId""=n.""Id"" and sale.""IsDeleted""=false
                where n.""IsDeleted""=false and sale.""CustomerId""='{CustomerId}' and sale.""ProductId""='{ProductId}' and sale.""MachineName""='{MachineName}'";
            return await _queryRepo.ExecuteQuerySingle<LicenseViewModel>(query, null );
        }

        public async Task<LicenseViewModel> GetLicenseNote(string licensePrivateKey)
        {
            var query = $@"Select * from 
public.""NtsNote"" as n 
join cms.""N_SYN_SALE_ApplicationLicense"" as udf on udf.""NtsNoteId""=n.""Id""  and 
udf.""IsDeleted""=false where n.""IsDeleted""=false and udf.""PrivateKey""='{licensePrivateKey}'";
            return await _queryRepo.ExecuteQuerySingle<LicenseViewModel>(query,null);
        }

    }
}
