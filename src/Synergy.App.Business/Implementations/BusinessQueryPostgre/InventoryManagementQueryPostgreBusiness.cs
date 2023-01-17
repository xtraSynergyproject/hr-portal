using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.IMS;
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
    public class InventoryManagementQueryPostgreBusiness : BusinessBase<ServiceViewModel, NtsService>, IInventoryManagementQueryBusiness
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
        public InventoryManagementQueryPostgreBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IRepositoryQueryBase<DirectSalesViewModel> queryRepo,
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
        public async Task<DirectSalesViewModel> GetDirectSalesData(string serviceId)
        {
            var query = $@"select ds.*
from cms.""N_IMS_IMS_DIRECT_SALES"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
where s.""Id""='{serviceId}'";

            return await _queryRepo.ExecuteQuerySingle<DirectSalesViewModel>(query, null);
        }
        public async Task<RequisitionViewModel> GetRequisitionData(string serviceId)
        {
            var query = $@"select ds.*
from cms.""N_IMS_Requisition"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
where s.""Id""='{serviceId}'";

            return await _queryRepo.ExecuteQuerySingle<RequisitionViewModel>(query, null);
        }
        public async Task<IList<DirectSalesViewModel>> GetDirectSalesData(DirectSalesSearchViewModel search)
        {
            var query = $@"select s.""Id"" as ServiceId,ds.""ProposalDate"" as ProposalDate,Sum(i.""Amount""::DECIMAL) as ProposalValue,
c.""CustomerName"" as CustomerName,s.""WorkflowStatus"" as WorkflowStatus,s.""ServiceNo"" as ServiceNo,lv.""Code"" as ServiceStatusCode
from cms.""N_IMS_IMS_DIRECT_SALES"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=ds.""Customer"" and c.""IsDeleted""=false 
left join cms.""N_IMS_DirectSaleItem"" as i on i.""DirectSalesId""=s.""Id"" and i.""IsDeleted""=false 
where 1=1 and ds.""IsDeleted""=false 
and ds.""ProposalDate""::DATE < '{search.ToDate}'::DATE and ds.""ProposalDate""::DATE > '{search.FromDate}'::DATE #WHERE#  group by 
ds.""Id"" ,s.""Id"" ,ds.""ProposalDate"" ,
c.""CustomerName"" ,s.""WorkflowStatus"" ,s.""ServiceNo"",lv.""Code"" ";
            var replace = $@"";
            if (search.Customer.IsNotNullAndNotEmpty())
            {
                replace = $@"and c.""Id""='{search.Customer}'";
            }
            if (search.Customer.IsNotNullAndNotEmpty())
            {
                replace = $@"and c.""Id""='{search.Customer}'";
            }

            //if (search.WorkflowStatus.IsNotNullAndNotEmpty())
            //{
            //    replace = $@"and s.""WorkflowStatus""='{search.WorkflowStatus}'";
            //}
            if (search.ProposalSource.IsNotNullAndNotEmpty())
            {
                replace = $@"and ds.""ProposalSource""='{search.ProposalSource}'";
            }
            query = query.Replace("#WHERE#", replace);
            return await _queryRepo.ExecuteQueryList<DirectSalesViewModel>(query, null);
        }
        public async Task<IList<ItemsViewModel>> GetDirectSaleItemsData(string directSalesId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,
n.""NoteNo"" as NoteNo,sri.""ReturnQuantity"",sri.""ReturnReason"",sri.""ReturnTypeId"",sri.""NtsNoteId"",
case when sri.""Id"" is not null then true else false end as CheckFlag
from cms.""N_IMS_DirectSaleItem"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false
join public.""NtsService"" as s on s.""Id""=dsi.""DirectSalesId"" and s.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""Item"" and i.""IsDeleted""=false 
left join cms.""N_IMS_SALES_RETURN_ITEM"" as sri on dsi.""Id""=sri.""DirectSaleItemId"" and sri.""IsDeleted""=false
--left join cms.""N_IMS_IMS_DIRECT_SALES"" as ds on s.""UdfNoteTableId""=ds.""Id"" and ds.""IsDeleted""=false
--left join cms.""N_IMS_SALES_RETURN"" sr on ds.""Id"" = sr.""DirectSaleId"" and sr.""IsDeleted"" = false
--left join cms.""N_IMS_SALES_RETURN_ITEM"" as sri on sr.""Id"" = sri.""SalesReturnId"" and sri.""IsDeleted"" = false
where s.""Id""='{directSalesId}' and dsi.""IsDeleted""=false order by dsi.""CreatedDate"" desc ";

            return await _queryRepo.ExecuteQueryList<ItemsViewModel>(query, null);
        }

        public async Task<IList<ItemsViewModel>> GetItemsListData(ItemsSearchViewModel search)
        {
            var query = $@"select i.*,n.""Id"" as NoteId, lovns.""Name"" as ItemStatus,lovns.""Code"" as NoteStatusCode,iuom.""FullName"" as ItemUnitName, lov.""Name"" as ItemTypeName
                            ,ic.""Name"" as ItemCategoryName, isc.""Name"" as ItemSubCategoryName
                            from cms.""N_IMS_IMS_ITEM_MASTER"" as i
                            left join public.""NtsNote"" as n on n.""Id""=i.""NtsNoteId"" and n.""IsDeleted""=false
                            left join public.""LOV"" as lovns on lovns.""Id""=n.""NoteStatusId"" and lovns.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_ITEM_UOM"" as iuom on iuom.""Id""=i.""ItemUnit""  and iuom.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on isc.""Id"" = i.""ItemSubCategory"" and isc.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_CATEGORY"" as ic on ic.""Id""=isc.""ItemCategory"" and ic.""IsDeleted""=false
                            left join public.""LOV"" as lov on lov.""Id""=ic.""ItemType"" and lov.""IsDeleted""=false

                            WHERE i.""IsDeleted""=false #WHERE# order by i.""CreatedDate"" desc
                        ";
            var replace = $@"";
            if (search.ItemTypeId.IsNotNullAndNotEmpty())
            {
                replace = String.Concat(replace, $@" and lov.""Id""='{search.ItemTypeId}' ");
            }
            if (search.ItemCategoryId.IsNotNullAndNotEmpty())
            {
                replace = String.Concat(replace + $@" and ic.""Id""='{search.ItemCategoryId}' ");
            }
            if (search.ItemSubCategoryId.IsNotNullAndNotEmpty())
            {
                replace = String.Concat(replace + $@" and isc.""Id""='{search.ItemSubCategoryId}'");
            }
            if (search.ItemStatusId.IsNotNullAndNotEmpty())
            {
                replace = String.Concat(replace, $@" and lovns.""Id""='{search.ItemStatusId}'");
            }
            query = query.Replace("#WHERE#", replace);
            return await _queryRepo.ExecuteQueryList<ItemsViewModel>(query, null);
        }
        public async Task<ItemsViewModel> GetItemsDetails(string id)
        {
            var query = $@"select i.*,n.""Id"" as NoteId,lovns.""Name"" as ItemStatus,lovns.""Code"" as NoteStatusCode,iuom.""FullName"" as ItemUnitName,lov.""Id"" as ItemType, lov.""Name"" as ItemTypeName
                            ,ic.""Id"" as ItemCategory,ic.""Name"" as ItemCategoryName, isc.""Id"" as ItemSubCategory, isc.""Name"" as ItemSubCategoryName
                            from cms.""N_IMS_IMS_ITEM_MASTER"" as i
                            left join public.""NtsNote"" as n on n.""Id""=i.""NtsNoteId"" and n.""IsDeleted""=false
                            left join public.""LOV"" as lovns on lovns.""Id""=n.""NoteStatusId"" and lovns.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_ITEM_UOM"" as iuom on iuom.""Id""=i.""ItemUnit""  and iuom.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on isc.""Id"" = i.""ItemSubCategory"" and isc.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_CATEGORY"" as ic on ic.""Id""=isc.""ItemCategory"" and ic.""IsDeleted""=false
                            left join public.""LOV"" as lov on lov.""Id""=ic.""ItemType"" and lov.""IsDeleted""=false
                            WHERE i.""IsDeleted""=false and i.""Id""='{id}'
                        ";
            var result = await _queryRepo.ExecuteQuerySingle<ItemsViewModel>(query, null);
            return result;
        }
        public async Task<IList<RequisitionViewModel>> GetRequisitionDataByItemHead(string itemHead, string Customer, string status, string From, string To, string requisitionIds = null)
        {
            var query = $@"select ds.""Id"" as Id,s.""Id"" as ServiceId,ds.""RequisitionDate"" as RequisitionDate,ds.""RequisitionValue"" as RequisitionValue,
c.""CustomerName"" as CustomerName,s.""WorkflowStatus"" as WorkflowStatus,s.""ServiceNo"" as ServiceNo,lv.""Code"" as ServiceStatusCode,lv.""Name"" as ServiceStatusName,ds.""RequisitionParticular"" as RequisitionParticular,u.""Name"" as CreatedBy
from cms.""N_IMS_Requisition"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=ds.""Customer"" and c.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=ds.""CreatedBy"" and u.""IsDeleted""=false

where 1=1 and ds.""IsDeleted""=false #WHERE#  
 ";
            var search = "";
            if (itemHead.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and ds.""ItemHead""='{itemHead}'");
            }
            if (Customer.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and c.""Id""='{Customer}'");
            }
            if (status.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and s.""ServiceStatusId""='{status}'");
            }
            if (From.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and ds.""RequisitionDate""::DATE>='{From}'::DATE");
            }
            if (To.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and ds.""RequisitionDate""::DATE<='{To}'::DATE");
            }
            if (requisitionIds.IsNotNullAndNotEmpty())
            {
                requisitionIds = requisitionIds.Replace(",", "','");
                search = string.Concat(search, $@" and ds.""Id"" in ('{requisitionIds}') ");
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<RequisitionViewModel>(query, null);
        }
        public async Task<IList<RequisitionViewModel>> GetIssueRequisitionData(string itemHead, string From, string To)
        {
            var query = $@"select s.""Id"" as ServiceId,ds.""RequisitionDate"" as RequisitionDate,ds.""RequisitionValue"" as RequisitionValue,lv.""Name"" as ServiceStatusName,
c.""CustomerName"" as CustomerName,s.""WorkflowStatus"" as WorkflowStatus,s.""ServiceNo"" as ServiceNo,lv.""Code"" as ServiceStatusCode,ds.""RequisitionParticular"" as RequisitionParticular,u.""Name"" as CreatedBy,
ds.""Id"" as Id
from cms.""N_IMS_Requisition"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=ds.""Customer"" and c.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=ds.""CreatedBy"" and u.""IsDeleted""=false
--left join cms.""N_IMS_DirectSaleItem"" as i on i.""DirectSalesId""=s.""Id"" and i.""IsDeleted""=false 
where 1=1 and ds.""IsDeleted""=false and lv.""Code""='SERVICE_STATUS_COMPLETE' and (ds.""Issued""='False' or ds.""Issued"" is null)  #WHERE#  
--  group by 
--ds.""Id"" ,s.""Id"" ,ds.""RequisitionDate"" ,ds.""RequisitionParticular"",
--c.""CustomerName"" ,s.""WorkflowStatus"" ,s.""ServiceNo"",lv.""Code"" ";
            var search = "";
            if (itemHead.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and ds.""ItemHead""='{itemHead}'");
            }

            if (From.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and ds.""RequisitionDate""::DATE>='{From}'::DATE");
            }
            if (To.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and ds.""RequisitionDate""::DATE <='{To}'::DATE");
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<RequisitionViewModel>(query, null);
        }

        public async Task<RequisitionViewModel> GetRequisitionDataByServiceId(string serviceId)
        {
            var query = $@"select ds.*,lv.""Name"" as ItemHead,s.""ServiceNo""
from cms.""N_IMS_Requisition"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=ds.""ItemHead"" and lv.""IsDeleted""=false
where s.""Id""='{serviceId}'";

            return await _queryRepo.ExecuteQuerySingle<RequisitionViewModel>(query, null);
        }
        public async Task<RequisitionViewModel> GetRequisitionDataById(string Id)
        {
            var query = $@"select ds.*,lv.""Id"" as ItemHead,s.""ServiceNo"",s.""Id"" as ServiceId
from cms.""N_IMS_Requisition"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=ds.""ItemHead"" and lv.""IsDeleted""=false
where ds.""Id""='{Id}'";

            return await _queryRepo.ExecuteQuerySingle<RequisitionViewModel>(query, null);
        }
        public async Task<List<ItemsViewModel>> GetCRPFRequisitionItemList(string serviceId)
        {
            var query = $@"select i.""ItemName"" as ItemName,ds.""RequiredQuantity"" as ItemQuantity,i.""ClosingQuantity"" as ClosingQuantity
from cms.""F_CRPFInventory_CRPFInventoryRequisitionItems"" as ds
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=ds.""ItemId"" and i.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=ds.""ItemHead"" and lv.""IsDeleted""=false
where ds.""RequisitionId""='{serviceId}'";

            return await _queryRepo.ExecuteQueryList<ItemsViewModel>(query, null);
        }

        public async Task<ItemsViewModel> GetItemsUnitDetailsByItemId(string itemId)
        {
            var query = $@"select u.""FullName"" as ItemUnitName

from cms.""N_IMS_IMS_ITEM_UOM"" as u
join public.""NtsNote"" as n on n.""Id""=u.""NtsNoteId"" and n.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""ItemUnit""=u.""Id"" and i.""IsDeleted""=false where i.""Id""='{itemId}' and u.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQuerySingle<ItemsViewModel>(query, null);
        }

        public async Task<IList<IdNameViewModel>> GetItemCategoryByItemTypeId(string itemTypeId)
        {
            var query = $@"select ic.""Id"" as Id, ic.""Name"" as Name,ic.""Code"" as Code
                            from cms.""N_IMS_ITEM_CATEGORY"" as ic 
                            where ic.""IsDeleted""=false and ic.""ItemType""='{itemTypeId}' ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetItemSubCategoryByItemCategoryId(string itemCategoryId)
        {
            var query = $@"select isc.""Id"" as Id, isc.""Name"" as Name,isc.""Code"" as Code
                            from cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc 
                            where isc.""IsDeleted""=false and isc.""ItemCategory""='{itemCategoryId}' ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetItemByItemSubCategoryId(string itemSubCategoryId)
        {
            var query = $@"select i.""Id"" as Id, i.""ItemName"" as Name
                            from cms.""N_IMS_IMS_ITEM_MASTER"" as i 
                            where i.""IsDeleted""=false and i.""ItemSubCategory""='{itemSubCategoryId}' ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<IList<ItemsViewModel>> GetRequisistionItemsData(string requisitionId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,
n.""NoteNo"" as NoteNo,dsi.""ItemQuantity"",dsi.""IssuedQuantity"",dsi.""ApprovedQuantity""
from cms.""N_IMS_RequisitionItems"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_IMS_Requisition"" as ds on ds.""Id""=dsi.""RequisitionId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""Item"" and i.""IsDeleted""=false where ds.""Id""='{requisitionId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<ItemsViewModel>(query, null);
        }

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionItemsToIssue(string requisitionId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""Id"" as RequisitionItemId,
ds.""Id"" as RequisitionId,dsi.""ItemQuantity"" as ItemQuantity,
case when dsi.""IssuedQuantity"" is not null then dsi.""IssuedQuantity"" else '0' end as IssuedQuantity,
(dsi.""ItemQuantity""::DECIMAL - (case when dsi.""IssuedQuantity"" is not null then dsi.""IssuedQuantity"" else '0' end)::DECIMAL) as BalanceQuantity,
i.""InventoryQuantity"" as InventoryQuantity,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionItems"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join public.""NtsService"" as ds on ds.""Id""=dsi.""RequisitionId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""Item"" and i.""IsDeleted""=false where ds.""Id""='{requisitionId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
        }

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItems(string issueServiceId, ImsIssueTypeEnum issueType)
        {
            var query = "";
            if (issueType==ImsIssueTypeEnum.Requisition) 
            {
                 query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""ReferenceHeaderItemId"" as ReferenceHeaderItemId,
dsi.""ReferenceHeaderId"" as ReferenceHeaderId,ri.""ItemQuantity"" as ItemQuantity,dsi.""IssuedQuantity"" as IssuedQuantity,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionIssueItem"" as dsi
join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=dsi.""ReferenceHeaderItemId"" and ri.""IsDeleted""=false 
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_IMS_RequisitionIssue"" as ds on ds.""Id""=dsi.""RequisitionIssueId"" and ds.""IsDeleted""=false and ds.""IssueReferenceType""='0'
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false where ds.""Id""='{issueServiceId}' and dsi.""IsDeleted""=false 
";
            }

            else if(issueType == ImsIssueTypeEnum.StockAdjustment || issueType == ImsIssueTypeEnum.StockTransfer)
            {
                query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""IssuedQuantity"" as IssuedQuantity,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionIssueItem"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_IMS_RequisitionIssue"" as ds on ds.""Id""=dsi.""RequisitionIssueId"" and ds.""IsDeleted""=false and ds.""IssueReferenceType""='{(int)((ImsIssueTypeEnum)Enum.Parse(typeof(ImsIssueTypeEnum), issueType.ToString()))}'
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false where ds.""Id""='{issueServiceId}' and dsi.""IsDeleted""=false 
";
            }
            return await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
        }

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItemsByRequisitionId(string requisitionId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""ReferenceHeaderItemId"" as ReferenceHeaderItemId,
ds.""Id"" as RequisitionIssueId,ri.""ItemQuantity"" as ItemQuantity,dsi.""IssuedQuantity"" as IssuedQuantity,u.""Name"" as CreatedBy,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionIssueItem"" as dsi
join public.""User"" as u on u.""Id""=dsi.""CreatedBy"" and u.""IsDeleted""=false 
join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=dsi.""ReferenceHeaderItemId"" and ri.""IsDeleted""=false 
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_IMS_RequisitionIssue"" as ds on ds.""Id""=dsi.""RequisitionIssueId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false where ri.""RequisitionId""='{requisitionId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
        }

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisitionIssueItemsByIssueRefId(string issueRefId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""ReferenceHeaderItemId"" as ReferenceHeaderItemId,
ds.""Id"" as RequisitionIssueId,dsi.""IssuedQuantity"" as IssuedQuantity,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionIssueItem"" as dsi 
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_IMS_RequisitionIssue"" as ds on ds.""Id""=dsi.""RequisitionIssueId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false 
where ds.""IssueReferenceId""='{issueRefId}' and dsi.""IsDeleted""=false ";

            return await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
        }



        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItemsToDeliver(string requisitionId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""ReferenceHeaderItemId"" as ReferenceHeaderItemId,
ds.""Id"" as RequisitionIssueId,ri.""ItemQuantity"" as ItemQuantity,dsi.""IssuedQuantity"" as IssuedQuantity,u.""Name"" as CreatedBy,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionIssueItem"" as dsi
join public.""User"" as u on u.""Id""=dsi.""CreatedBy"" and u.""IsDeleted""=false 
join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=dsi.""ReferenceHeaderItemId"" and ri.""IsDeleted""=false 
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_IMS_RequisitionIssue"" as ds on ds.""Id""=dsi.""RequisitionIssueId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false where ri.""RequisitionId""='{requisitionId}' and dsi.""IsDeleted""=false 
and COALESCE(cast(ri.""IssuedQuantity"" as Integer),0) > COALESCE(cast(ri.""DeliveredQuantity"" as Integer),0)
";

            return await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
        }



        public async Task<RequisitionIssueItemsViewModel> GetRequisistionIssueItemsById(string requisitionIssueItemId)
        {
            var query = $@"select * from  cms.""N_IMS_RequisitionIssueItem"" where ""Id""='{requisitionIssueItemId}'";
            return await _queryRepo.ExecuteQuerySingle<RequisitionIssueItemsViewModel>(query, null);
        }
        public async Task<StockAdjustmentViewModel> GetStockAdjustmentById(string stockAdjustmentId)
        {
            var query = $@"select *,""NtsNoteId"" as NoteId from  cms.""N_SNC_IMS_INVENTORY_StockAdjustment"" where ""Id""='{stockAdjustmentId}'";
            return await _queryRepo.ExecuteQuerySingle<StockAdjustmentViewModel>(query, null);
        }
        public async Task<IList<IssueRequisitionViewModel>> GetRequisistionIssue(string referenceId, ImsIssueTypeEnum issuetype)
        {
            var query = "";
            if (issuetype == ImsIssueTypeEnum.StockAdjustment || issuetype==ImsIssueTypeEnum.StockTransfer)
            {
                 query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId,lv.""Name"" as IssueType,c.""CustomerName"" as IssueTo,s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo
from cms.""N_IMS_RequisitionIssue"" as dsi

join public.""NtsService"" as s on s.""UdfNoteTableId""=dsi.""Id"" and s.""IsDeleted""=false 
left join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=dsi.""IssueTo"" and c.""IsDeleted""=false 
left join public.""LOV"" as lv on lv.""Id""=dsi.""IssueType"" and lv.""IsDeleted""=false 
 where dsi.""IssueReferenceId""='{referenceId}' and dsi.""IsDeleted""=false and dsi.""IssueReferenceType""='{(int)((ImsIssueTypeEnum)Enum.Parse(typeof(ImsIssueTypeEnum), issuetype.ToString()))}'
";
            }
            else if (issuetype == ImsIssueTypeEnum.Requisition)
            {
                 query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId,lv.""Name"" as IssueType,c.""CustomerName"" as IssueTo,s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo
                            from cms.""N_IMS_RequisitionIssue"" as dsi
                            join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=dsi.""IssueTo"" and c.""IsDeleted""=false 
                            join public.""LOV"" as lv on lv.""Id""=dsi.""IssueType"" and lv.""IsDeleted""=false 
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=dsi.""Id"" and s.""IsDeleted""=false 
                            where dsi.""IssueReferenceId""='{referenceId}' and dsi.""IsDeleted""=false and dsi.""IssueReferenceType""='0'
                            union
                            select dsi.*,dsi.""NtsNoteId"" as NoteId,lv.""Name"" as IssueType,c.""Name"" as IssueTo,s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo
                            from cms.""N_IMS_RequisitionIssue"" as dsi
                            join public.""User"" as c on c.""Id""=dsi.""IssueTo"" and c.""IsDeleted""=false 
                            join public.""LOV"" as lv on lv.""Id""=dsi.""IssueType"" and lv.""IsDeleted""=false 
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=dsi.""Id"" and s.""IsDeleted""=false 
                            where dsi.""IssueReferenceId""='{referenceId}' and dsi.""IsDeleted""=false and dsi.""IssueReferenceType""='0'
                            ";
            }
           

            return await _queryRepo.ExecuteQueryList<IssueRequisitionViewModel>(query, null);
        }
        public async Task<IList<DeliveryNoteViewModel>> GetDeliveryNoteData(string itemHead, string From, string To)
        {
            var query = $@"select ds.*,s.""ServiceNo"" as ServiceNo,u.""Name""  as CreatedBy,lv.""Name"" as ServiceStatusName,lv.""Code"" as ServiceStatusCode,s.""Id"" as ServiceId
from cms.""N_IMS_IMS_DELIVERY_NOTE"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
join cms.""N_IMS_Requisition"" as r on ds.""RequisitionId""=r.""Id"" and r.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=ds.""CreatedBy"" and u.""IsDeleted""=false  #WHERE#  
 ";
            var search = "";
            if (itemHead.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and r.""ItemHead""='{itemHead}'");
            }

            if (From.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and ds.""DeliveryOn""::DATE>='{From}'::DATE");
            }
            if (To.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and ds.""DeliveryOn""::DATE <='{To}'::DATE");
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<DeliveryNoteViewModel>(query, null);
        }
        public async Task<DeliveryNoteReportViewModel> GetDeliveryNoteReportData(string deliveryNoteId)
        {
            var query = $@"select s.""ServiceNo"" as ServiceNo,u.""Name"" as CreatedBy,
                            dn.""NameScopeOfWork"" as NameScopeOfWork,dn.""GSTIN"" as GSTIN,dn.""DeliveryOn"" as DeliveryOn,
                            country.""Name"" as CountryName,state.""Name"" as StateName,city.""Name"" as CityName,
                            dn.""VehicleNo"" as VehicleNo, dn.""PIN"" as PIN, dn.""ShippingAddress"" as ShippingAddress
                            ,c.""CustomerName"" as CustomerName, c.""Address"" as CustomerAddress
                            ,rs.""ServiceNo"" as RequisitionNo, req.""RequisitionDate"" as RequisitionDate
                            ,le.""Name"" as LegalEntityName, le.""Address"" as LegalEntityAddress, le.""Email"" as LegalEntityEmail
                            from cms.""N_IMS_IMS_DELIVERY_NOTE"" as dn
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=dn.""Id"" and s.""IsDeleted""=false
                            join public.""User"" as u on u.""Id""=dn.""CreatedBy"" and u.""IsDeleted""=false
                            left join public.""NtsService"" as rs on rs.""Id""=dn.""RequisitionId"" and rs.""IsDeleted""=false 
                            left join cms.""N_IMS_Requisition"" as req on req.""Id""=rs.""UdfNoteTableId"" and req.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=req.""Customer"" and c.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_Country"" as country on country.""Id""= dn.""CountryId"" and country.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_States"" as state on state.""Id""= dn.""StateId"" and state.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_City"" as city on city.""Id""= dn.""CityId"" and city.""IsDeleted""=false
                            left join public.""LegalEntity"" as le on le.""Id""= dn.""LegalEntityId"" and le.""IsDeleted""=false
                            where dn.""Id""='{deliveryNoteId}' and dn.""IsDeleted""=false
                            ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DeliveryNoteReportViewModel>(query, null);
            return querydata;
        }
        public async Task<PurchaseOrderReportViewModel> GetPurchaseOrderReportData(string purchaseOrderId)
        {
            var query = $@"select po.""PoDate"" as PoDate,po.""PoDate"" as PurchaseOrderDate, po.""BillToUnit"" as BillToUnit,po.""VendorId"" as VendorId,po.""ContactPersonId"" as ContactPersonId
                            , po.""ContactNo"" as ContactNo, po.""ShipTo"" as ShipTo, po.""PhoneNo"" as PhoneNo, po.""CountryId"" as CountryId, po.""StateId"" as StateId, po.""CityId"" as CityId
                            , po.""ShippingAddress"" as ShippingAddress, po.""GstNo"" as GstNo, po.""Remark"" as Remark, po.""POValue"" as POValue
                            , s.""ServiceNo"" as ServiceNo, s.""ServiceNo"" as PurchaseOrderNo
                            , uc.""Name"" as CreatedBy, up.""Name"" as PreparedBy
                            , v.""VendorName"" as VendorName, v.""Address"" as VendorAddress, v.""GstNo"" as VendorGstNo, v.""PanNo"" as VendorPanNo
                            , vc.""ContactPersonName"" as ContactPersonName
                            , country.""Name"" as CountryName, state.""Name"" as StateName, city.""Name"" as CityName
                            , bu.""BusinessUnitName"" as BillToUnitName, bu.""Address"" as BillToUnitAddress, bu.""PinCode"" as BillToUnitPinCode, bu.""PANNo"" as BillToUnitPANNo, bu.""TANNo"" as BillToUnitTANNo
                            , bu.""GSTNo"" as BillToUnitGSTNo, bu.""PhoneNo"" as BillToUnitPhoneNo, bu.""Mobile"" as BillToUnitMobile
                            , bucountry.""Name"" as BillToUnitCountry, bustate.""Name"" as BillToUnitState, bucity.""Name"" as BillToUnitCity
                            from cms.""N_IMS_IMS_PO"" as po
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false
                            left join public.""User"" as uc on uc.""Id""=po.""CreatedBy"" and uc.""IsDeleted""=false
                            left join public.""User"" as up on up.""Id""=s.""OwnerUserId"" and up.""IsDeleted""=false
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
                            left join cms.""N_IMS_VendorContact"" as vc on po.""ContactPersonId""=vc.""Id"" and vc.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_Country"" as country on country.""Id""= po.""CountryId"" and country.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_States"" as state on state.""Id""= po.""StateId"" and state.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_City"" as city on city.""Id"" = po.""CityId"" and city.""IsDeleted"" = false
                            left join cms.""N_IMS_BusinessUnit"" as bu on bu.""Id""=po.""BillToUnit"" and bu.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_Country"" as bucountry on bucountry.""Id""= bu.""CountryId"" and bucountry.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_States"" as bustate on bustate.""Id""= bu.""StateId"" and bustate.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_City"" as bucity on bucity.""Id"" = bu.""CityId"" and bucity.""IsDeleted"" = false
                            where po.""Id""='{purchaseOrderId}' and po.""IsDeleted""=false
                            ";
            var querydata = await _queryRepo.ExecuteQuerySingle<PurchaseOrderReportViewModel>(query, null);
            return querydata;
        }
        public async Task<ReceivedNoteReportViewModel> GetReceivedNoteReportData(string receivedNoteId)
        {
            var query = $@" select gr.""ChallanDate"" as ChallanDate, gr.""ChallonNo"" as ChallanNo,gr.""InvoiceDate"" as InvoiceDate, gr.""InvoiceNo"" as InvoiceNo, gr.""ReceiveDate"" as ReceivedOn
                            , v.""VendorName"" as VendorName
                            ,s.""ServiceNo"" as ServiceNo,u.""Name"" as CreatedBy
                            from cms.""N_IMS_GOODS_RECEIPT"" as gr
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=gr.""Id"" and s.""IsDeleted""=false 
                            left join public.""User"" as u on u.""Id""=gr.""CreatedBy"" and u.""IsDeleted""=false 
                            left join cms.""N_IMS_IMS_PO"" as po on po.""Id""=gr.""POId"" and po.""IsDeleted""=false
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
                            where gr.""Id""='{receivedNoteId}' and gr.""IsDeleted""=false ";

            var querydata = await _queryRepo.ExecuteQuerySingle<ReceivedNoteReportViewModel>(query, null);

            return querydata;
        }
        public async Task<PurchaseInvoiceReportViewModel> GetPurchaseInvoiceReportData(string purchaseInvoiceId)
        {
            var query = $@" select pi.""InvoiceNo"" as InvoiceNo, pi.""InvoiceDate"" as InvoiceDate
                            , v.""VendorName"" as VendorName
                            from cms.""N_IMS_PO_INVOICE"" as pi
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=pi.""Id"" and s.""IsDeleted""=false
                            left join public.""User"" as u on u.""Id""=pi.""CreatedBy"" and u.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_PO"" as po on po.""Id""=pi.""PoId"" and po.""IsDeleted""=false
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
                            where pi.""Id""='{purchaseInvoiceId}' and pi.""IsDeleted""=false
                            ";
            var querydata = await _queryRepo.ExecuteQuerySingle<PurchaseInvoiceReportViewModel>(query, null);
            return querydata;
        }
        public async Task<List<InvoiceItemViewModel>> GetPurchaseInvoiceItemsList(string purchaseInvoiceId)
        {
            var query = $@" select item.""ItemName"" as ItemName, poi.""ReceivedQuantity"" as ReceivedQuantity,poi.""TotalAmount"" as TotalAmount
                        ,reqi.""PurchaseRate"" as PurchaseRate
                        from cms.""N_IMS_PO_INVOICE_ITEM"" as pii
                        join cms.""N_IMS_PO_INVOICE"" as pi on pi.""Id""=pii.""POInvoiceId"" and pi.""IsDeleted""=false
                        left join cms.""N_IMS_IMS_PO_ITEM"" as poi on poi.""Id""=pii.""POItemId"" and poi.""IsDeleted""=false
                        left join cms.""N_IMS_IMS_ITEM_MASTER"" as item on item.""Id""=pii.""ItemId"" and item.""IsDeleted""=false
                        left join cms.""N_IMS_RequisitionItems"" as reqi on reqi.""Id""=poi.""RequisitionItemId"" and reqi.""IsDeleted""=false
                        where pii.""POInvoiceId"" = '{purchaseInvoiceId}' and pii.""IsDeleted""=false 
                        ";

            var list = await _queryRepo.ExecuteQueryList<InvoiceItemViewModel>(query, null);
            var sno = 1;
            foreach (var item in list)
            {
                item.SNo = sno;
                sno++;
            }
            return list;
        }
        public async Task<List<GoodsReceiptItemViewModel>> GetGoodReceiptItemsList(string receiptId)
        {
            var query = $@" select gri.""ItemQuantity"",gri.""Id"", gri.""ReferenceHeaderItemId"", reqItem.""POQuantity"", reqItem.""PurchaseRate"", i.""Id"" as ItemId, i.""ItemName"",gri.""WarehouseId"" as WarehouseId
                        from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as gri 
                        join cms.""N_IMS_GOODS_RECEIPT"" as gr on gr.""Id""=gri.""GoodReceiptId"" and gr.""IsDeleted""=false
                        left join cms.""N_IMS_IMS_PO_ITEM"" as poItem on poItem.""Id"" = gri.""ReferenceHeaderItemId"" and poItem.""IsDeleted""=false
                        left join cms.""N_IMS_RequisitionItems"" as reqItem on reqItem.""Id"" = poItem.""RequisitionItemId"" and reqItem.""IsDeleted""=false
                        left join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = reqItem.""Item"" and i.""IsDeleted""=false
                        where gri.""GoodReceiptId"" = '{receiptId}' and gri.""IsDeleted""=false ";

            var list = await _queryRepo.ExecuteQueryList<GoodsReceiptItemViewModel>(query, null);
            var sno = 1;
            foreach (var item in list)
            {
                item.SNo = sno;
                sno++;
            }
            return list;
        }
        public async Task<List<DeliveryItemViewModel>> GetDeliveryItemsList(string deliveryNoteId)
        {
            var query = $@" select di.*,di.""NtsNoteId"" as NoteId, di.""DeliveredQuantity"" as DeliveredQuantity, i.""ItemName"" as ItemName, u.""ShortName"" as ItemUOM
                        from cms.""N_IMS_DELIVERY_ITEMS"" as di 
                        join cms.""N_IMS_IMS_DELIVERY_NOTE"" as dn on dn.""Id""=di.""DeliveryNoteId"" and dn.""IsDeleted""=false
                        left join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = di.""IssuedItemsId"" and i.""IsDeleted""=false
                        left join cms.""N_IMS_IMS_ITEM_UOM"" as u on u.""Id""=i.""ItemUnit"" and u.""IsDeleted""=false
                        where di.""DeliveryNoteId"" = '{deliveryNoteId}' and di.""IsDeleted""=false ";

            var list = await _queryRepo.ExecuteQueryList<DeliveryItemViewModel>(query, null);
            var sno = 1;
            foreach (var item in list)
            {
                item.SNo = sno;
                sno++;
            }
            return list;
        }
        public async Task<List<IdNameViewModel>> GetInvertoryFinancialYearIdNameList()
        {
            var query = $@" select fy.""Id"" as Id, fy.""FinancialYearName"" as Name
                            from cms.""N_IMS_FINANCIAL_YEAR"" as fy
                            where fy.""IsDeleted""=false
                        ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<PurchaseInvoiceReportViewModel>> GetReportInvoiceDetailsData(DateTime fromDate, DateTime toDate)
        {
            var query = $@" select pi.""InvoiceNo"" as InvoiceNo, pi.""InvoiceDate""::Date as InvoiceDate
                            , po.""POValue""::Decimal as InvoiceAmount
                            , v.""VendorName"" as VendorName,v.""GstNo"" as  VendorGSTNo
                            from cms.""N_IMS_PO_INVOICE"" as pi
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=pi.""Id"" and s.""IsDeleted""=false
                            left join public.""User"" as u on u.""Id""=pi.""CreatedBy"" and u.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_PO"" as po on po.""Id""=pi.""PoId"" and po.""IsDeleted""=false
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
                            where pi.""IsDeleted""=false and pi.""InvoiceDate""::Date>='{fromDate}'::Date and pi.""InvoiceDate""::Date<='{toDate}'::Date
                            ";
            var querydata = await _queryRepo.ExecuteQueryList<PurchaseInvoiceReportViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<RequisitionIssueItemsViewModel>> GetReportIssueTypeData(DateTime fromDate, DateTime toDate, string issueTypeId, string departmentId, string employeeId, string issueToTypeId)
        {
            var query = $@" select rii.""CreatedDate"",i.""ItemName"",rii.""IssuedQuantity"",rs.""ServiceNo"" as ChallanNo
                            , itype.""Name"" as IssueTypeName, c.""CustomerName"" as IssueToTypeName
                            from cms.""N_IMS_RequisitionIssueItem"" as rii
                            left join cms.""N_IMS_RequisitionIssue"" as ri on ri.""Id""=rii.""RequisitionIssueId"" and ri.""IsDeleted"" = false
                            left join cms.""N_IMS_Requisition"" as r on r.""Id"" = ri.""RequisitionId"" and r.""IsDeleted"" = false
                            left join public.""NtsService"" as rs on rs.""UdfNoteTableId""=ri.""Id"" and rs.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = rii.""ItemId"" and i.""IsDeleted"" = false
                            left join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on w.""Id""=rii.""WarehouseId"" and w.""IsDeleted"" = false
                            left join public.""LOV"" as itype on itype.""Id""=ri.""IssueType"" and itype.""IsDeleted"" = false
                            left join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=ri.""IssueTo"" and c.""IsDeleted"" = false
                            where rii.""IsDeleted"" = false and rii.""CreatedDate""::Date >= '{fromDate}'::Date and rii.""CreatedDate""::Date <= '{toDate}'::Date
                            #WHEREISSUETYPE# #WHEREDEPARTMENT# #WHEREEMPLOYEE# #WHEREISSUETOTYPE#
                            ";
            var searchIssueType = "";
            var searchDepartment = "";
            var searchEmployee = "";
            var searchIssueToType = "";
            if (issueTypeId.IsNotNullAndNotEmpty())
            {
                searchIssueType = $@" and itype.""Id""='{issueTypeId}' ";
            }
            if (departmentId.IsNotNullAndNotEmpty())
            {

            }
            if (employeeId.IsNotNullAndNotEmpty())
            {

            }
            if (issueToTypeId.IsNotNullAndNotEmpty())
            {
                searchIssueToType = $@" and c.""Id""='{issueToTypeId}' ";
            }
            query = query.Replace("#WHEREISSUETYPE#", searchIssueType);
            query = query.Replace("#WHEREDEPARTMENT#", searchDepartment);
            query = query.Replace("#WHEREEMPLOYEE#", searchEmployee);
            query = query.Replace("#WHEREISSUETOTYPE#", searchIssueToType);
            var querydata = await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<ItemStockViewModel>> GetReportItemHistoryData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            var query = $@" select 'RECEIPT' as InOutType,gr.""ReceiveDate"" as TransactionDate, i.""ItemName"",u.""Name"" as VerifiedBy,gri.""CreatedDate"" as VerifiedDate
                            ,gri.""ItemQuantity"" as ItemQuantity
                            from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as gri
                            join cms.""N_IMS_GOODS_RECEIPT"" as gr on gr.""Id""=gri.""GoodReceiptId"" and gr.""IsDeleted""=false
                            join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=gri.""ItemId"" and i.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on isc.""Id"" = i.""ItemSubCategory"" and isc.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_CATEGORY"" as ic on ic.""Id"" = isc.""ItemCategory"" and ic.""IsDeleted"" = false
                            left join public.""LOV"" as itemtype on itemtype.""LOVType""='IMS_ITEM_TYPE' and itemtype.""Id""=ic.""ItemType"" and itemtype.""IsDeleted"" = false
                            left join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on w.""Id""=gri.""WarehouseId"" and w.""IsDeleted"" = false
                            left join public.""User"" as u on u.""Id""=gri.""CreatedBy"" and u.""IsDeleted""=false
                            where gri.""IsDeleted""=false and gr.""ReceiveDate""::Date >= '{fromDate}'::Date and gr.""ReceiveDate""::Date <= '{toDate}'::Date
                            #WHEREWAREHOUSE# #WHEREITEMTYPE# #WHEREITEMCATEGORY# #WHEREITEMSUBCATEGORY# #WHEREITEM#

                            union
                            select 'ISSUE' as InOutType, ri.""IssuedOn"" as TransactionDate, i.""ItemName"",u.""Name"" as VerifiedBy,rii.""CreatedDate"" as VerifiedDate
                            ,rii.""IssuedQuantity"" as ItemQuantity
                            from cms.""N_IMS_RequisitionIssueItem"" as rii
                            join cms.""N_IMS_RequisitionIssue"" as ri on ri.""Id""=rii.""RequisitionIssueId"" and ri.""IsDeleted""=false
                            join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=rii.""ItemId"" and i.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on isc.""Id"" = i.""ItemSubCategory"" and isc.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_CATEGORY"" as ic on ic.""Id"" = isc.""ItemCategory"" and ic.""IsDeleted"" = false
                            left join public.""LOV"" as itemtype on itemtype.""LOVType""='IMS_ITEM_TYPE' and itemtype.""Id""=ic.""ItemType"" and itemtype.""IsDeleted"" = false
                            left join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on w.""Id""=rii.""WarehouseId"" and w.""IsDeleted"" = false                            
                            join public.""User"" as u on u.""Id""=rii.""CreatedBy"" and u.""IsDeleted""=false
                            where rii.""IsDeleted""=false and ri.""IssuedOn""::Date >= '{fromDate}'::Date and ri.""IssuedOn""::Date <= '{toDate}'::Date
                            #WHEREWAREHOUSE# #WHEREITEMTYPE# #WHEREITEMCATEGORY# #WHEREITEMSUBCATEGORY# #WHEREITEM#
                            ";

            var searchWarehouse = "";
            var searchItemType = "";
            var searchItemCategory = "";
            var searchItemSubCategory = "";
            var searchItem = "";
            if (warehouseId.IsNotNullAndNotEmpty())
            {
                searchWarehouse = $@" and w.""Id""='{warehouseId}' ";
            }
            if (itemTypeId.IsNotNullAndNotEmpty())
            {
                searchItemType = $@" and itemtype.""Id""='{itemTypeId}' ";
            }
            if (itemCategoryId.IsNotNullAndNotEmpty())
            {
                searchItemCategory = $@" and ic.""Id""='{itemCategoryId}' ";
            }
            if (itemSubCategoryId.IsNotNullAndNotEmpty())
            {
                searchItemSubCategory = $@" and isc.""Id""='{itemSubCategoryId}' ";
            }
            if (itemId.IsNotNullAndNotEmpty())
            {
                searchItem = $@" and i.""Id""='{itemId}' ";
            }
            query = query.Replace("#WHEREWAREHOUSE#", searchWarehouse);
            query = query.Replace("#WHEREITEMTYPE#", searchItemType);
            query = query.Replace("#WHEREITEMCATEGORY#", searchItemCategory);
            query = query.Replace("#WHEREITEMSUBCATEGORY#", searchItemSubCategory);
            query = query.Replace("#WHEREITEM#", searchItem);

            var querydata = await _queryRepo.ExecuteQueryList<ItemStockViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<StockTransferViewModel>> GetReportItemTransferData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            var query = $@" select s.""ServiceNo"",st.""TransferDate"",st.""ChallanNo"",i.""ItemName"",fw.""WarehouseName"" as FromWareHouse
                            ,tw.""WarehouseName"" as ToWareHouse,sti.""TransferQuantity"",sti.""IssuedQuantity"",st.""TransferReason""
                            from cms.""N_IMS_STOCK_TRANSFER"" as st
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=st.""Id"" and s.""IsDeleted""=false
                            join cms.""N_IMS_STOCK_TRANSFER_ITEM"" as sti on sti.""StockTransferId""=st.""Id"" and sti.""IsDeleted""=false
                            join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=sti.""ItemId"" and i.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on isc.""Id"" = i.""ItemSubCategory"" and isc.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_CATEGORY"" as ic on ic.""Id"" = isc.""ItemCategory"" and ic.""IsDeleted"" = false
                            left join public.""LOV"" as itemtype on itemtype.""LOVType""='IMS_ITEM_TYPE' and itemtype.""Id""=ic.""ItemType"" and itemtype.""IsDeleted"" = false
                            left join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as fw on fw.""Id""=st.""FromWarehouseId"" and fw.""IsDeleted"" = false                            
                            left join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as tw on tw.""Id""=st.""ToWarehouseId"" and tw.""IsDeleted"" = false                            
                            where st.""IsDeleted""=false and st.""TransferDate""::Date >= '{fromDate}'::Date and st.""TransferDate""::Date <= '{toDate}'::Date
                            #WHEREWAREHOUSE# #WHEREITEMTYPE# #WHEREITEMCATEGORY# #WHEREITEMSUBCATEGORY# #WHEREITEM#
                            ";

            var searchWarehouse = "";
            var searchItemType = "";
            var searchItemCategory = "";
            var searchItemSubCategory = "";
            var searchItem = "";
            if (warehouseId.IsNotNullAndNotEmpty())
            {
                searchWarehouse = $@" and ( fw.""Id""='{warehouseId}' or tw.""Id""='{warehouseId}' ) ";
            }
            if (itemTypeId.IsNotNullAndNotEmpty())
            {
                searchItemType = $@" and itemtype.""Id""='{itemTypeId}' ";
            }
            if (itemCategoryId.IsNotNullAndNotEmpty())
            {
                searchItemCategory = $@" and ic.""Id""='{itemCategoryId}' ";
            }
            if (itemSubCategoryId.IsNotNullAndNotEmpty())
            {
                searchItemSubCategory = $@" and isc.""Id""='{itemSubCategoryId}' ";
            }
            if (itemId.IsNotNullAndNotEmpty())
            {
                searchItem = $@" and i.""Id""='{itemId}' ";
            }
            query = query.Replace("#WHEREWAREHOUSE#", searchWarehouse);
            query = query.Replace("#WHEREITEMTYPE#", searchItemType);
            query = query.Replace("#WHEREITEMCATEGORY#", searchItemCategory);
            query = query.Replace("#WHEREITEMSUBCATEGORY#", searchItemSubCategory);
            query = query.Replace("#WHEREITEM#", searchItem);

            var querydata = await _queryRepo.ExecuteQueryList<StockTransferViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<PurchaseOrderViewModel>> GetReportPurchaseOrderStatusData(DateTime fromDate, DateTime toDate, string statusId)
        {
            var query = $@" select s.""ServiceNo"", v.""VendorName"", po.""PoDate"", po.""POValue"",lv.""Name"" as POStatus,itemtype.""Name"" as ItemHeadName
                            from cms.""N_IMS_IMS_PO"" as po
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false
                            left join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
                            left join public.""LOV"" as itemtype on itemtype.""Id""=po.""ItemHeadId"" and itemtype.""IsDeleted""=false
                            where po.""IsDeleted""=false and po.""PoDate""::Date >= '{fromDate}'::Date and po.""PoDate""::Date <= '{toDate}'::Date
                            #WHERESTATUS#
                        ";
            var searchStatus = "";
            if (statusId.IsNotNullAndNotEmpty())
            {
                searchStatus = $@" and lv.""Id""='{statusId}' ";
            }
            query = query.Replace("#WHERESTATUS#",searchStatus);
            var querydata = await _queryRepo.ExecuteQueryList<PurchaseOrderViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<PurchaseOrderViewModel>> GetReportOrderBookData()
        {
            var query = $@" select s.""ServiceNo"", v.""VendorName"",city.""Name"" as CityName,po.""Remark"", po.""PoDate"", po.""POValue"",lv.""Name"" as POStatus,itemtype.""Name"" as ItemHeadName
                            from cms.""N_IMS_IMS_PO"" as po
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false
                            left join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
                            left join public.""LOV"" as itemtype on itemtype.""Id""=po.""ItemHeadId"" and itemtype.""IsDeleted""=false
                            left join cms.""N_IMS_MASTERDATA_City"" as city on city.""Id"" = v.""CityId"" and city.""IsDeleted"" = false
                            where po.""IsDeleted""=false order by po.""CreatedDate""
                        ";

            var querydata = await _queryRepo.ExecuteQueryList<PurchaseOrderViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<OrderSummaryViewModel>> GetReportOrderStatusData(string financialYearId)
        {
            var list = new List<OrderSummaryViewModel>();
            if (financialYearId.IsNotNullAndNotEmpty())
            {
                var fyquery = $@" select fy.* 
                            from cms.""N_IMS_FINANCIAL_YEAR"" as fy 
                            where fy.""IsDeleted""=false and fy.""Id""='{financialYearId}'
                            ";
                var fyqueryData = await _queryRepo.ExecuteQuerySingle<InventoryFinancialYearViewModel>(fyquery, null);

                var query = $@" select po.*,s.""ServiceNo"", v.""VendorName"",lv.""Name"" as POStatus                           
							from cms.""N_IMS_IMS_PO"" as po
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false
                            left join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
                            where po.""IsDeleted""=false
                            --and po.""PoDate""::Date>=(select ""FinancialYearStartDate""::Date from cms.""N_IMS_FINANCIAL_YEAR"" as fy where fy.""IsDeleted""=false and fy.""Id""='{financialYearId}' )
                            --and po.""PoDate""::Date<=(select ""FinancialYearEndDate""::Date from cms.""N_IMS_FINANCIAL_YEAR"" as fy where fy.""IsDeleted""=false and fy.""Id""='{financialYearId}' )
                            and po.""PoDate""::Date>='{fyqueryData.FinancialYearStartDate}'::Date and po.""PoDate""::Date<='{fyqueryData.FinancialYearEndDate}'::Date
                        ";

                var querydata = await _queryRepo.ExecuteQueryList<PurchaseOrderViewModel>(query, null);

                decimal closingBalance = 0;
                int j = 1;
                string quarter = "Q1";
                if (querydata != null)
                {
                    int i = 4;
                    while (i <= 12)
                    {
                        var monthYear = querydata.Where(x => x.PoDate.Month == i).Select(x => ((MonthEnum)x.PoDate.Date.Month).ToString() + " - " + x.PoDate.Date.Year.ToString()).FirstOrDefault();
                        if (monthYear.IsNullOrEmpty())
                        {
                            if (i >= 4)
                            {
                                monthYear = ((MonthEnum)i).ToString() + " - " + fyqueryData.FinancialYearStartDate.Date.Year.ToString();
                            }
                            else
                            {
                                monthYear = ((MonthEnum)i).ToString() + " - " + fyqueryData.FinancialYearEndDate.Date.Year.ToString();
                            }

                        }
                        var monthAmount = querydata.Where(x => x.PoDate.Month == i).Sum(x => Convert.ToDecimal(x.POValue));

                        list.Add(new OrderSummaryViewModel
                        {
                            MonthYear = monthYear,
                            PreviousYearBalance = closingBalance,
                            NewOrdersValue = monthAmount,
                            BilledOrdersValue = 0,
                            ClosingBalance = closingBalance + monthAmount,
                            SequenceOrder = j,
                            Quarter = quarter
                        });
                        closingBalance = closingBalance + monthAmount;
                        if (i == 3)
                            break;
                        i++;
                        j++;
                        if (i == 13)
                        {
                            i = 1;
                        }
                        if (i == 7)
                        {
                            var q1Amt = list.Where(x => x.Quarter == "Q1").Sum(x => x.NewOrdersValue);
                            list.Add(new OrderSummaryViewModel
                            {
                                MonthYear = "TOTAL",
                                NewOrdersValue = q1Amt,
                                Quarter = quarter
                            });

                            quarter = "Q2";
                        }
                        else if (i == 10)
                        {
                            var q2Amt = list.Where(x => x.Quarter == "Q2").Sum(x => x.NewOrdersValue);
                            list.Add(new OrderSummaryViewModel
                            {
                                MonthYear = "TOTAL",
                                NewOrdersValue = q2Amt,
                                Quarter = quarter
                            });
                            quarter = "Q3";
                        }
                        else if (i == 1)
                        {
                            var q3Amt = list.Where(x => x.Quarter == "Q3").Sum(x => x.NewOrdersValue);
                            list.Add(new OrderSummaryViewModel
                            {
                                MonthYear = "TOTAL",
                                NewOrdersValue = q3Amt,
                                Quarter = quarter
                            });
                            quarter = "Q4";
                        }
                    }
                    var q4Amt = list.Where(x => x.Quarter == "Q4").Sum(x => x.NewOrdersValue);
                    list.Add(new OrderSummaryViewModel
                    {
                        MonthYear = "TOTAL",
                        NewOrdersValue = q4Amt,
                        Quarter = quarter
                    });

                    var allAmt = querydata.Sum(x => Convert.ToDecimal(x.POValue));
                    list.Add(new OrderSummaryViewModel
                    {
                        MonthYear = "GRAND TOTAL",
                        NewOrdersValue = allAmt,
                        Quarter = quarter
                    });
                }
            }
            

            //list.Add(new OrderSummaryViewModel {MonthYear= "April-2022",PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "May-2022", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "June-2022", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "July-2022", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "August-2022", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "September-2022", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "October-2022", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "November-2022", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "December-2022", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "January-2023", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "February-2023", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            //list.Add(new OrderSummaryViewModel {MonthYear= "March-2023", PreviousYearBalance=0,NewOrdersValue=1200,BilledOrdersValue=0,ClosingBalance=1200 });
            return list;
        }
        public async Task<List<RequisitionViewModel>> GetReportRequisitionByStatusData(DateTime fromDate,DateTime toDate,string typeId,string customerId,string statusId)
        {
            //var data = new List<RequisitionViewModel>();
            var query = $@"select rs.""ServiceNo"",r.""RequisitionDate"",itemtype.""Name"" as ItemHeadName,c.""CustomerName"",r.""RequisitionValue"",r.""RequisitionParticular""
                            ,lv.""Name"" as RequisitionStatus
                            from cms.""N_IMS_Requisition"" as r
                            join public.""NtsService"" as rs on rs.""UdfNoteTableId""=r.""Id"" and rs.""IsDeleted""=false
                            join public.""LOV"" as lv on lv.""Id""=rs.""ServiceStatusId"" and lv.""IsDeleted""=false
                            left join public.""LOV"" as itemtype on itemtype.""Id""=r.""ItemHead"" and itemtype.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=r.""Customer"" and c.""IsDeleted"" = false
                            where r.""IsDeleted""=false and r.""RequisitionDate""::Date >= '{fromDate}'::Date and r.""RequisitionDate""::Date <= '{toDate}'::Date 
                            #WEHRETYPE# #WEHRECUSTOMER# #WEHREStatus#
                        ";
            var searchType = "";
            var searchCustomer = "";
            var searchStatus = "";
            if (typeId.IsNotNullAndNotEmpty())
            {
                searchType = $@" and itemtype.""Id""='{typeId}' ";
            }
            if (customerId.IsNotNullAndNotEmpty())
            {
                searchCustomer = $@" and c.""Id""='{customerId}' ";
            }
            if (statusId.IsNotNullAndNotEmpty())
            {
                searchStatus = $@" and lv.""Id""='{statusId}' ";
            }
            query = query.Replace("#WEHRETYPE#",searchType);
            query = query.Replace("#WEHRECUSTOMER#",searchCustomer);
            query = query.Replace("#WEHREStatus#",searchStatus);
            var querydata = await _queryRepo.ExecuteQueryList<RequisitionViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<RequisitionViewModel>> GetReportRequisitionByDetailsData(DateTime fromDate, DateTime toDate, string typeId, string customerId, string statusId)
        {
            //var data = new List<RequisitionViewModel>();
            var query = $@"select rs.""ServiceNo"",r.""RequisitionDate"",itemtype.""Name"" as ItemHeadName,c.""CustomerName"",r.""RequisitionValue"",r.""RequisitionParticular""
                            ,lv.""Name"" as RequisitionStatus,i.""ItemName"",ri.""ItemQuantity"" as RequisitionQuantity, ri.""ApprovedQuantity""
                            from cms.""N_IMS_Requisition"" as r
                            join public.""NtsService"" as rs on rs.""UdfNoteTableId""=r.""Id"" and rs.""IsDeleted""=false
                            join public.""LOV"" as lv on lv.""Id""=rs.""ServiceStatusId"" and lv.""IsDeleted""=false
							left join cms.""N_IMS_RequisitionItems"" as ri on ri.""RequisitionId""=r.""Id"" and ri.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = ri.""Item"" and i.""IsDeleted"" = false
                            left join public.""LOV"" as itemtype on itemtype.""Id""=r.""ItemHead"" and itemtype.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=r.""Customer"" and c.""IsDeleted"" = false
                            where r.""IsDeleted""=false and r.""RequisitionDate""::Date >= '{fromDate}'::Date and r.""RequisitionDate""::Date <= '{toDate}'::Date 
                            #WEHRETYPE# #WEHRECUSTOMER# #WEHREStatus#
                        ";
            var searchType = "";
            var searchCustomer = "";
            var searchStatus = "";
            if (typeId.IsNotNullAndNotEmpty())
            {
                searchType = $@" and itemtype.""Id""='{typeId}' ";
            }
            if (customerId.IsNotNullAndNotEmpty())
            {
                searchCustomer = $@" and c.""Id""='{customerId}' ";
            }
            if (statusId.IsNotNullAndNotEmpty())
            {
                searchStatus = $@" and lv.""Id""='{statusId}' ";
            }
            query = query.Replace("#WEHRETYPE#", searchType);
            query = query.Replace("#WEHRECUSTOMER#", searchCustomer);
            query = query.Replace("#WEHREStatus#", searchStatus);
            var querydata = await _queryRepo.ExecuteQueryList<RequisitionViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<GoodsReceiptViewModel>> GetReportReceivedFromPOData(DateTime fromDate, DateTime toDate, string vendorId)
        {
            //var data = new List<RequisitionViewModel>();
            var query = $@"select pos.""ServiceNo"" as PONo, i.""ItemName"",poi.""ReceivedQuantity"", reqi.""POQuantity"" , gr.""ReceiveDate"",gr.""ChallonNo"",gr.""ChallanDate"",v.""VendorName""
                            from cms.""N_IMS_GOODS_RECEIPT"" as gr
                            join cms.""N_IMS_GOODS_RECEIPT_ITEM"" as gri on gri.""GoodReceiptId""=gr.""Id"" and gri.""IsDeleted""=false
                            join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = gri.""ItemId"" and i.""IsDeleted"" = false
                            join cms.""N_IMS_IMS_PO"" as po on po.""Id""=gr.""GoodsReceiptReferenceId"" and po.""IsDeleted""=false
                            join public.""NtsService"" as pos on pos.""UdfNoteTableId""=po.""Id"" and pos.""IsDeleted""=false
                            join cms.""N_IMS_IMS_PO_ITEM"" as poi on poi.""POId""=po.""Id"" and poi.""IsDeleted""=false
                            join cms.""N_IMS_RequisitionItems"" as reqi on reqi.""Item""=i.""Id"" and reqi.""Id""=poi.""RequisitionItemId"" and poi.""IsDeleted""=false
                            join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
                            where gr.""IsDeleted""=false and gr.""ReceiptType""='0' and gr.""ReceiveDate""::Date >= '{fromDate}'::Date and gr.""ReceiveDate""::Date <= '{toDate}'::Date 
                            #WEHREVENDOR#
                        ";
            var searchVendor = "";
            if (vendorId.IsNotNullAndNotEmpty())
            {
                searchVendor = $@" and v.""Id""='{vendorId}' ";
            }
            query = query.Replace("#WEHREVENDOR#", searchVendor);

            var querydata = await _queryRepo.ExecuteQueryList<GoodsReceiptViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<VendorCategoryViewModel>> GetReportVendorCategoryData(string vendorId, string categoryId, string subCategoryId)
        {
            var query = $@"select distinct vc.""Id"", v.""VendorName"",c.""Name"" as ItemCategoryName
                            from cms.""N_IMS_IMS_VENDOR_CATEGORY_MAPPING"" as vc
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=vc.""VendorId"" and v.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_CATEGORY"" as c on c.""Id""=vc.""CategoryId"" and c.""IsDeleted""=false
                            left join cms.""N_IMS_ITEM_SUB_CATEGORY"" as sc on sc.""ItemCategory""=c.""Id"" and c.""IsDeleted""=false
                            where vc.""IsDeleted""=false
                            #WEHREVENDOR# #WHERECATEGORY# #WHERESUBCATEGORY#
                        ";
            var searchVendor = "";
            var searchCategory = "";
            var searchSubCategory = "";
            if (vendorId.IsNotNullAndNotEmpty())
            {
                searchVendor = $@" and v.""Id""='{vendorId}' ";
            }
            if (categoryId.IsNotNullAndNotEmpty())
            {
                searchCategory = $@" and c.""Id""='{categoryId}' ";
            }
            if (subCategoryId.IsNotNullAndNotEmpty())
            {
                searchSubCategory = $@" and sc.""Id""='{subCategoryId}' ";
            }
            query = query.Replace("#WEHREVENDOR#", searchVendor);
            query = query.Replace("#WHERECATEGORY#", searchCategory);
            query = query.Replace("#WHERESUBCATEGORY#", searchSubCategory);

            var querydata = await _queryRepo.ExecuteQueryList<VendorCategoryViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<PurchaseReturnViewModel>> GetReportReturnToVendorData(DateTime fromDate, DateTime toDate, string vendorId)
        {
            var query = $@"select pr.""ReturnDate"", v.""VendorName"",pri.""PurchaseQuantity"",pri.""ReturnQuantity"",pri.""ReturnComment""
                            ,pos.""ServiceNo"" as PurchaseOrderServiceNo,i.""ItemName""
                            from cms.""N_SNC_IMS_PURCHASE_PurchaseReturn"" as pr
                            join cms.""N_IMS_PurchaseReturnItem"" as pri on pri.""PurchaseReturnId""=pr.""Id"" and pri.""IsDeleted""=false
                            left join cms.""N_IMS_VENDOR"" as v on v.""Id""=pr.""VendorId"" and v.""IsDeleted""=false
                            left join cms.""N_IMS_IMS_PO"" as po on po.""Id""=pr.""POId"" and po.""IsDeleted""=false
                            left join public.""NtsService"" as pos on pos.""UdfNoteTableId""=po.""Id"" and pos.""IsDeleted""=false
                            join cms.""N_IMS_IMS_PO_ITEM"" as poi on poi.""Id""=pri.""POItemId"" and poi.""IsDeleted""=false
                            join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=poi.""RequisitionItemId"" and ri.""IsDeleted""=false
                            join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=ri.""Item"" and i.""IsDeleted""=false
                            where pr.""IsDeleted""=false and pr.""ReturnDate""::Date >= '{fromDate}'::Date and pr.""ReturnDate""::Date <= '{toDate}'::Date 
                            #WEHREVENDOR#
                        ";
            var searchVendor = "";
            if (vendorId.IsNotNullAndNotEmpty())
            {
                searchVendor = $@" and v.""Id""='{vendorId}' ";
            }
            query = query.Replace("#WEHREVENDOR#", searchVendor);

            var querydata = await _queryRepo.ExecuteQueryList<PurchaseReturnViewModel>(query, null);
            var sno = 1;
            foreach (var item in querydata)
            {
                item.SNo = sno;
                sno++;
            }
            return querydata;
        }
        public async Task<List<POItemsViewModel>> GetRequisistionItemsByRequisitionId(string requisitionIds,bool isValidate=false)
        {
            requisitionIds = requisitionIds.Replace(",", "','");
            List<POItemsViewModel> list = new List<POItemsViewModel>();
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId,i.""ItemName"" as ItemName  
from cms.""N_IMS_RequisitionItems"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 

join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""Item"" and i.""IsDeleted""=false 
where dsi.""RequisitionId"" in ('{requisitionIds}') and dsi.""IsDeleted""=false #Validate# ";
            
            string validate = isValidate? $@"and dsi.""ApprovedQuantity""::decimal <= dsi.""POQuantity""::decimal" : $@"and dsi.""ApprovedQuantity""::decimal > dsi.""POQuantity""::decimal";
            query = query.Replace("#Validate#", validate);
            
            var data = await _queryRepo.ExecuteQueryList<ItemsViewModel>(query, null);
            if (data.Count>0)
            {
                list = data.Select(x => new POItemsViewModel()
                {
                    RequisitionItemId = x.Id,
                    PurchaseRate = x.PurchaseRate,
                    ItemQuantity = x.ItemQuantity,
                    ApprovedQuantity = x.ApprovedQuantity,
                    SalesRate = x.SaleRate,
                    ItemName = x.ItemName,
                    ExistingPOQuantity=x.POQuantity,
                    DeliveredQuantity=x.DeliveredQuantity
                }).ToList();
            }
            return list;
        }
        public async Task<IList<VendorViewModel>> GetVendorList(string countryId, string stateId, string cityId, string name)
        {
            var query = $@"select dsi.*,c.""Name"" as CityName, dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_VENDOR"" as dsi
join cms.""N_IMS_MASTERDATA_City"" as c on c.""Id""=dsi.""CityId"" and c.""IsDeleted"" =false where 1=1 #COUNTRYWHERE# #STATEWHERE# #CITYWHERE# #NAMEWHERE#
";
            var search1 = "";
            var search2 = "";
            var search3 = "";
            var search4 = "";
            if (countryId.IsNotNullAndNotEmpty())
            {
                search1 = $@" and dsi.""CountryId""='{countryId}'";

            }
            if (stateId.IsNotNullAndNotEmpty())
            {
                search2 = $@" and dsi.""StateId""='{stateId}'";

            }
            if (cityId.IsNotNullAndNotEmpty())
            {
                search3 = $@" and dsi.""CityId""='{cityId}'";

            }
            if (name.IsNotNullAndNotEmpty())
            {
                search4 = $@" and dsi.""VendorName"" like '%{name}%' COLLATE ""tr-TR-x-icu""";

            }
            query = query.Replace("#COUNTRYWHERE#", search1);
            query = query.Replace("#STATEWHERE#", search2);
            query = query.Replace("#CITYWHERE#", search3);
            query = query.Replace("#NAMEWHERE#", search4);
            return await _queryRepo.ExecuteQueryList<VendorViewModel>(query, null);
        }
        public async Task<VendorViewModel> GetVendorDetailsById(string vendorId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_VENDOR"" as dsi
where dsi.""IsDeleted""=false and dsi.""Id""='{vendorId}'
";
            return await _queryRepo.ExecuteQuerySingle<VendorViewModel>(query, null);
        }
        public async Task<ItemShelfViewModel> GetItemShelfDetailsById(string itemShelfId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_ITEM_SHELF"" as dsi
where dsi.""IsDeleted""=false and dsi.""Id""='{itemShelfId}'
";
            return await _queryRepo.ExecuteQuerySingle<ItemShelfViewModel>(query, null);
        }
        public async Task<List<ContactsViewModel>> ReadVendorContactsData(string vendorId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_VendorContact"" as dsi
where dsi.""IsDeleted""=false #WHERE# 
";
            var search = "";
            if (vendorId.IsNotNullAndNotEmpty()) 
            {
                search = $@" and dsi.""VendorId""='{vendorId}'";
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<ContactsViewModel>(query, null);
        }
        public async Task<List<VendorCategoryViewModel>> ReadVendorCategoryData(string vendorId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId,v.""VendorName"" as VendorName,i.""Name"" as ItemCategoryName
from cms.""N_IMS_IMS_VENDOR_CATEGORY_MAPPING"" as dsi
join cms.""N_IMS_VENDOR"" as v on v.""Id""=dsi.""VendorId""
join cms.""N_IMS_ITEM_CATEGORY"" as i on i.""Id""=dsi.""CategoryId""
where dsi.""IsDeleted""=false and dsi.""VendorId""='{vendorId}'
";
            return await _queryRepo.ExecuteQueryList<VendorCategoryViewModel>(query, null);
        }
        public async Task<List<ItemShelfViewModel>> ReadItemShelfCategoryData(string itemShelfId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId,v.""ShelfNo"" as ShelfNo,i.""Name"" as ItemCategoryName
from cms.""N_IMS_IMS_ITEM_SHELF_CATEGORY"" as dsi
join cms.""N_IMS_ITEM_SHELF"" as v on v.""Id""=dsi.""ShelfId""
join cms.""N_IMS_ITEM_CATEGORY"" as i on i.""Id""=dsi.""CategoryId""
where dsi.""IsDeleted""=false and dsi.""ShelfId""='{itemShelfId}'
";
            return await _queryRepo.ExecuteQueryList<ItemShelfViewModel>(query, null);
        }


        //        public async Task<List<VendorCategoryViewModel>> ReadCategoryNotInVendorCategoryData(string vendorId)
        //        {
        //            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId,v.""ShelfNo"" as Shelf,i.""Name"" as ItemCategoryName
        //from cms.""N_IMS_IMS_VENDOR_CATEGORY_MAPPING"" as dsi
        //join cms.""N_IMS_VENDOR"" as v on v.""Id""=dsi.""VendorId""
        //join cms.""N_IMS_ITEM_CATEGORY"" as i on i.""Id""<> dsi.""CategoryId""
        //where dsi.""IsDeleted""=false and dsi.""VendorId""='{vendorId}'
        //";
        //            return await _queryRepo.ExecuteQueryList<VendorCategoryViewModel>(query, null);
        //        }
        public async Task<List<ItemShelfViewModel>> ReadCategoryNotInItemShelfCategoryData(string itemShelfId)
        {
            var query = $@"select i.""Id"",i.""Name"" as ""ItemCategoryName"",i.""NtsNoteId"" as ""NoteId""
from cms.""N_IMS_ITEM_CATEGORY"" as i
where i.""Id"" not in (select dsi.""CategoryId"" from
                    cms.""N_IMS_IMS_ITEM_SHELF_CATEGORY"" as dsi
                    join cms.""N_IMS_ITEM_SHELF"" as v on v.""Id"" = dsi.""ShelfId"" and v.""IsDeleted""=false
                    where dsi.""ShelfId"" = '{itemShelfId}' and dsi.""IsDeleted""=false) 
and i.""IsDeleted""=false
";
            return await _queryRepo.ExecuteQueryList<ItemShelfViewModel>(query, null);
        }
        public async Task<List<VendorCategoryViewModel>> ReadCategoryNotInVendorCategoryData(string vendorId)
        {
            var query = $@"select i.""Id"",i.""Name"" as ""ItemCategoryName"",i.""NtsNoteId"" as ""NoteId""
from cms.""N_IMS_ITEM_CATEGORY"" as i
where i.""Id"" not in (select dsi.""CategoryId"" from
                    cms.""N_IMS_IMS_VENDOR_CATEGORY_MAPPING"" as dsi
                                             join cms.""N_IMS_VENDOR"" as v on v.""Id""=dsi.""VendorId""

                    where dsi.""VendorId"" = '{vendorId}')
";
            return await _queryRepo.ExecuteQueryList<VendorCategoryViewModel>(query, null);
        }
        public async Task<List<ItemShelfViewModel>> ReadShelfList()
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_ITEM_SHELF"" as dsi

where dsi.""IsDeleted""=false 
";
            return await _queryRepo.ExecuteQueryList<ItemShelfViewModel>(query, null);
        }
        public async Task<ItemShelfViewModel> GetItemShelfDetail(string noteId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_ITEM_SHELF"" as dsi

where dsi.""IsDeleted""=false  and dsi.""NtsNoteId""='{noteId}'
";
            return await _queryRepo.ExecuteQuerySingle<ItemShelfViewModel>(query, null);
        }
        public async Task DeleteContacts(string Id)
        {
            var query = $@"Update cms.""N_IMS_VendorContact"" 
set ""IsDeleted""=true where ""Id""='{Id}'
";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<IList<CustomerViewModel>> GetCustomerList(string countryId, string stateId, string cityId, string name)
        {
            var query = $@"select dsi.*,c.""Name"" as CityName,lov.""Name"" as ""CustomerCategoryName""
,lov1.""Name"" as ""CustomerVerticalName"",dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_IMS_CUSTOMER"" as dsi
join cms.""N_IMS_MASTERDATA_City"" as c on c.""Id""=dsi.""City"" and c.""IsDeleted"" =false 
join public.""LOV"" as lov on lov.""Id""=dsi.""CustomerCategory""
join public.""LOV"" as lov1 on lov1.""Id""=dsi.""CustomerVertical""
where 1=1 #COUNTRYWHERE# #STATEWHERE# #CITYWHERE# #NAMEWHERE#
";
            var search1 = "";
            var search2 = "";
            var search3 = "";
            var search4 = "";
            if (countryId.IsNotNullAndNotEmpty())
            {
                search1 = $@" and dsi.""Country""='{countryId}'";

            }
            if (stateId.IsNotNullAndNotEmpty())
            {
                search2 = $@" and dsi.""State""='{stateId}'";

            }
            if (cityId.IsNotNullAndNotEmpty())
            {
                search3 = $@" and dsi.""City""='{cityId}'";

            }
            if (name.IsNotNullAndNotEmpty())
            {
                search4 = $@" and dsi.""CustomerName"" like '%{name}%' COLLATE ""tr-TR-x-icu""";

            }
            query = query.Replace("#COUNTRYWHERE#", search1);
            query = query.Replace("#STATEWHERE#", search2);
            query = query.Replace("#CITYWHERE#", search3);
            query = query.Replace("#NAMEWHERE#", search4);
            return await _queryRepo.ExecuteQueryList<CustomerViewModel>(query, null);
        }
        public async Task<CustomerViewModel> GetCustomerDetailsById(string customerId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_IMS_CUSTOMER"" as dsi
where dsi.""IsDeleted""=false and dsi.""Id""='{customerId}'
";
            return await _queryRepo.ExecuteQuerySingle<CustomerViewModel>(query, null);
        }
        public async Task<List<ItemStockViewModel>> ReadItemStockData(string itemTypeId, string itemCategory, string itemSubCategory, string warehouseId)
        {
            var query = $@"select s.*,dsi.""ItemName"",dsi.""Id"" as ItemId,dsi.""NtsNoteId"" as NoteId,u.""FullName"" as ""ItemUnit"",  s.""BalanceQuantity"" :: INTEGER, s.""ClosingQuantity"" :: INTEGER, ss.""Code"" as NoteStatusCode, ss.""Name"" as ItemStatus 
,dsi.""IsSerializable"" as IsSerializable,w.""WarehouseName"" as WarehouseName
from cms.""N_IMS_IMS_ITEM_MASTER"" as dsi 
join cms.""N_IMS_IMS_ITEM_UOM"" as u on u.""Id"" = dsi.""ItemUnit""
join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on dsi.""ItemSubCategory"" =isc.""Id""
join cms.""N_IMS_ITEM_CATEGORY"" as ic on isc.""ItemCategory"" = ic.""Id""
join public.""LOV"" as lov on ic.""ItemType"" =lov.""Id""
left join cms.""N_IMS_ItemStock"" as s on s.""ItemId"" = dsi.""Id"" and s.""WarehouseId""='{warehouseId}'
left join public.""NtsNote"" as n on n.""Id"" = s.""NtsNoteId"" 
left join public.""LOV"" as ss on n.""NoteStatusId"" = ss.""Id""
left join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on w.""Id""=s.""WarehouseId"" 
where dsi.""IsDeleted"" = false
 #ITEMTYPEIDWHERE# #ITEMCATEGORYWHERE# #ITEMSUBCATEGORYWHERE# #WAREHOUSEIDWHERE#
";
            var search1 = "";
            var search2 = "";
            var search3 = "";
            var search4 = "";
            if (itemTypeId.IsNotNullAndNotEmpty())
            {
                search1 = $@" and lov.""Id""='{itemTypeId}'";

            }
            if (itemCategory.IsNotNullAndNotEmpty())
            {
                search2 = $@" and ic.""Id""='{itemCategory}'";

            }
            if (itemSubCategory.IsNotNullAndNotEmpty())
            {
                search3 = $@" and isc.""Id""='{itemSubCategory}'";

            }
 
            query = query.Replace("#ITEMTYPEIDWHERE#", search1);
            query = query.Replace("#ITEMCATEGORYWHERE#", search2);
            query = query.Replace("#ITEMSUBCATEGORYWHERE#", search3);
            query = query.Replace("#WAREHOUSEIDWHERE#", search4);
            return await _queryRepo.ExecuteQueryList<ItemStockViewModel>(query, null);


        }
        public async Task<List<ItemStockViewModel>> ReadItemListByStock(string itemTypeId, string itemCategory, string itemSubCategory, string warehouseId)
        {
            var query = $@"select dsi.""ItemName"",dsi.""Id"" as ItemId,u.""FullName"" as ""ItemUnit"",  s.""BalanceQuantity"" :: INTEGER 
from cms.""N_IMS_IMS_ITEM_MASTER"" as dsi 
join cms.""N_IMS_IMS_ITEM_UOM"" as u on u.""Id"" = dsi.""ItemUnit""
join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on dsi.""ItemSubCategory"" =isc.""Id""
join cms.""N_IMS_ITEM_CATEGORY"" as ic on isc.""ItemCategory"" = ic.""Id""
join public.""LOV"" as lov on ic.""ItemType"" =lov.""Id""
join cms.""N_IMS_ItemStock"" as s on s.""ItemId"" = dsi.""Id""
join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on w.""Id""=s.""WarehouseId""
where dsi.""IsDeleted"" = false
 #ITEMTYPEIDWHERE# #ITEMCATEGORYWHERE# #ITEMSUBCATEGORYWHERE# #WAREHOUSEIDWHERE#
";
            var search1 = "";
            var search2 = "";
            var search3 = "";
            var search4 = "";
            if (itemTypeId.IsNotNullAndNotEmpty())
            {
                search1 = $@" and lov.""Id""='{itemTypeId}'";
            }
            if (itemCategory.IsNotNullAndNotEmpty())
            {
                search2 = $@" and ic.""Id""='{itemCategory}'";
            }
            if (itemSubCategory.IsNotNullAndNotEmpty())
            {
                search3 = $@" and isc.""Id""='{itemSubCategory}'";
            }
            if (warehouseId.IsNotNullAndNotEmpty())
            {
                search4 = $@" and w.""Id""='{warehouseId}'";
            }
            query = query.Replace("#ITEMTYPEIDWHERE#", search1);
            query = query.Replace("#ITEMCATEGORYWHERE#", search2);
            query = query.Replace("#ITEMSUBCATEGORYWHERE#", search3);
            query = query.Replace("#WAREHOUSEIDWHERE#", search4);
            return await _queryRepo.ExecuteQueryList<ItemStockViewModel>(query, null);

        }

        public async Task<List<ItemStockViewModel>> ReadItemCurrentStockData(string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            var query = $@"select s.*,i.""ItemName"",i.""Id"" as ItemId,i.""NtsNoteId"" as NoteId,u.""FullName"" as ""ItemUnit""
                            from cms.""N_IMS_IMS_ITEM_MASTER"" as i 
                            join cms.""N_IMS_IMS_ITEM_UOM"" as u on u.""Id"" = i.""ItemUnit"" and u.""IsDeleted""=false
                            join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on isc.""Id""=i.""ItemSubCategory"" and isc.""IsDeleted"" = false
                            join cms.""N_IMS_ITEM_CATEGORY"" as ic on ic.""Id""=isc.""ItemCategory"" and ic.""IsDeleted"" = false 
                            join public.""LOV"" as lov on lov.""Id""=ic.""ItemType"" and lov.""IsDeleted"" = false
                            left join cms.""N_IMS_ItemStock"" as s on s.""ItemId"" = i.""Id"" and s.""IsDeleted"" = false
                            left join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on w.""Id""=s.""WarehouseId"" and w.""IsDeleted"" = false
                            where i.""IsDeleted"" = false
                             #WAREHOUSEIDWHERE# #ITEMTYPEIDWHERE# #ITEMCATEGORYWHERE# #ITEMSUBCATEGORYWHERE# #ITEMWHERE# 
                            ";
            var searchWarehouse = "";
            var searchItemType = "";
            var searchItemCategory = "";
            var searchItemSubCateggory = "";
            var searchItem = "";
            if (warehouseId.IsNotNullAndNotEmpty())
            {
                searchWarehouse = $@" and w.""Id""='{warehouseId}' ";
            }
            if (itemTypeId.IsNotNullAndNotEmpty())
            {
                searchItemType = $@" and lov.""Id""='{itemTypeId}' ";
            }
            if (itemCategoryId.IsNotNullAndNotEmpty())
            {
                searchItemCategory = $@" and ic.""Id""='{itemCategoryId}' ";
            }
            if (itemSubCategoryId.IsNotNullAndNotEmpty())
            {
                searchItemSubCateggory = $@" and isc.""Id""='{itemSubCategoryId}' ";
            }
            if (itemId.IsNotNullAndNotEmpty())
            {
                searchItem = $@" and i.""Id""='{itemId}' ";
            }

            query = query.Replace("#WAREHOUSEIDWHERE#", searchWarehouse);
            query = query.Replace("#ITEMTYPEIDWHERE#", searchItemType);
            query = query.Replace("#ITEMCATEGORYWHERE#", searchItemCategory);
            query = query.Replace("#ITEMSUBCATEGORYWHERE#", searchItemSubCateggory);
            query = query.Replace("#ITEMWHERE#", searchItem);

            var currentStockList = await _queryRepo.ExecuteQueryList<ItemStockViewModel>(query, null);
            var sno = 1;
            foreach (var item in currentStockList)
            {
                item.SNo = sno;
                sno++;
            }
            return currentStockList;
        }

        public async Task<ItemStockViewModel> GetUnitItemData(string Id)
        {
            var query = $@"select s.*,dsi.""ItemName"",s.""NtsNoteId"" as NoteId, ss.""Code"" as NoteStatusCode, ss.""Name"" as ItemStatus
 from cms.""N_IMS_IMS_ITEM_MASTER"" as dsi
 left join cms.""N_IMS_ItemStock"" as s on s.""ItemId"" = dsi.""Id""
left join public.""NtsNote"" as n on n.""Id"" = s.""NtsNoteId"" 
left join public.""LOV"" as ss on n.""NoteStatusId"" = ss.""Id""
 where dsi.""Id"" =  '{Id}'
";
            return await _queryRepo.ExecuteQuerySingle<ItemStockViewModel>(query, null);
        }

        public async Task<ItemStockViewModel> CheckItemStockExists(string itemId,string warehouseId)
        {
            var query = $@" select * from cms.""N_IMS_ItemStock"" where ""ItemId"" = '{itemId}' and ""WarehouseId""='{warehouseId}'";
            var queryData = await _queryRepo.ExecuteQuerySingle<ItemStockViewModel>(query, null);
            return queryData;
        }

        public async Task<ItemStockViewModel> GetItemHeaderData(string Id,string warehouseId)
        {
            var query = $@"select s.*,dsi.""ItemName"",lov.""Name"" as ""ItemTypeName"",iu.""FullName"" as ItemUnit,dsi.""Id"" as ItemId,w.""WarehouseName"" as WarehouseName
 from cms.""N_IMS_IMS_ITEM_MASTER"" as dsi
left join cms.""N_IMS_ItemStock"" as s on s.""ItemId"" = dsi.""Id""
left join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on w.""Id""=s.""WarehouseId"" and w.""IsDeleted"" = false
left join cms.""N_IMS_IMS_ITEM_UOM"" as iu on dsi.""ItemUnit""=iu.""Id"" and iu.""IsDeleted""=false
join cms.""N_IMS_ITEM_SUB_CATEGORY"" as isc on dsi.""ItemSubCategory"" =isc.""Id""
join cms.""N_IMS_ITEM_CATEGORY"" as ic on isc.""ItemCategory"" = ic.""Id""
join public.""LOV"" as lov on ic.""ItemType"" =lov.""Id""
 where dsi.""Id"" =  '{Id}' and s.""WarehouseId"" ='{warehouseId}'
";
            return await _queryRepo.ExecuteQuerySingle<ItemStockViewModel>(query, null);
        }

        public async Task<List<IdNameViewModel>> GetItemCodeMappingList()
        {
            var query = $@"select dsi.""Id"" as Id,CONCAT( dsi.""Code"",'-',dsi.""ItemDescription"") as Name,dsi.""TaxDescription"" as Code
from cms.""N_IMS_ItemCodeMapping"" as dsi
where dsi.""IsDeleted""=false 
";
            return await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
        }
        public async Task<List<ContactsViewModel>> ReadCustomerContactsData(string customerId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId
from cms.""N_IMS_CustomerContact"" as dsi
where dsi.""IsDeleted""=false #Where# ";
            var where = "";
            if (customerId.IsNotNullAndNotEmpty())
            {
              where = $@" and dsi.""CustomerId""='{customerId}' ";
            }

            //var where = customerId.IsNotNullAndNotEmpty() ? $@" and dsi.""CustomerId""='{customerId}' ";
            query = query.Replace("#Where#", where);
            return await _queryRepo.ExecuteQueryList<ContactsViewModel>(query, null);
        }
        public async Task DeleteCustomerContacts(string Id)
        {
            var query = $@"Update cms.""N_IMS_CustomerContact"" 
set ""IsDeleted""=true where ""Id""='{Id}'
";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task DeleteVendorCategories(string Id)
        {
            var query = $@"Update cms.""N_IMS_IMS_VENDOR_CATEGORY_MAPPING"" 
set ""IsDeleted""=true where ""Id""='{Id}'
";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task DeleteItemShelfCategories(string Id)
        {
            var query = $@"Update cms.""N_IMS_IMS_ITEM_SHELF_CATEGORY"" 
set ""IsDeleted""=true where ""Id""='{Id}'
";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<List<PurchaseOrderViewModel>> GetVendorPOList(string itemHead = null, string vendorId = null, string statusId = null, string From = null, string To = null)
        {
            var query = $@"Select s.""Id"" as ServiceId, s.""ServiceNo"" as PurchaseOrderNo,v.""VendorName"",po.""Id"" as POID,po.""VendorId"",po.""PoDate""::DATE as PurchaseOrderDate,po.""POValue"",
ss.""Name"" as POStatus,ss.""Code"" as ServiceStatusCode, u.""Name"" as PreparedBy,vc.""ContactPersonName"",po.""ContactNo"",v.""Address"" as VendorAddress
From public.""NtsService"" as s
join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_IMS_IMS_PO"" as po on n.""Id""=po.""NtsNoteId"" and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_IMS_VENDOR"" as v on po.""VendorId""=v.""Id"" and v.""IsDeleted""=false and v.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_IMS_VendorContact"" as vc on po.""ContactPersonId""=vc.""Id"" and vc.""IsDeleted""=false and vc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' #WHERE# order by s.""CreatedDate"" desc ";

            var search = "";
            if (itemHead.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""ItemHeadId""='{itemHead}'");
            }
            if (vendorId.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""VendorId""='{vendorId}'");
            }
            if (statusId.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and s.""ServiceStatusId""='{statusId}'");
            }
            if (From.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""PoDate""::DATE>='{From}'::DATE");
            }
            if (To.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""PoDate""::DATE<='{To}'::DATE");
            }
            query = query.Replace("#WHERE#", search);

            return await _queryRepo.ExecuteQueryList<PurchaseOrderViewModel>(query, null);
        }

        public async Task<double> GetPOValueByPOId(string POId)
        {
            var query = $@"select sum(poi.""TotalAmount""::decimal) from cms.""N_IMS_IMS_PO"" as po
join cms.""N_IMS_IMS_PO_ITEM"" as poi on poi.""POId""=po.""Id"" and poi.""IsDeleted""=false 
where po.""IsDeleted""=false and po.""Id""='{POId}'  group by poi.""POId""";
            return await _queryRepo.ExecuteScalar<double>(query, null);
        }

        public async Task UpdatePOValueInPO(string POId, double POValue)
        {
            var query = $@"Update  cms.""N_IMS_IMS_PO"" set ""POValue""='{POValue}',""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
where ""Id""='{POId}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<List<PurchaseOrderViewModel>> ReadPOData(string ItemHead, string Vendor, string From, string To)
        {
            var query = $@"select po.*,po.""NtsNoteId"" as NoteId,s.""Id"" as ServiceId,lv.""Code"" as ServiceStatusCode,lv.""Name"" as ServiceStatusName,
s.""ServiceNo"" as ServiceNo,v.""VendorName"" as VendorName,u.""Name"" as CreatedBy
from cms.""N_IMS_IMS_PO"" as po
join cms.""N_IMS_VENDOR"" as v on v.""Id""=po.""VendorId"" and v.""IsDeleted""=false
join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=po.""CreatedBy"" and u.""IsDeleted""=false 
where po.""IsDeleted""=false and lv.""Code""='SERVICE_STATUS_COMPLETE' #WHERE#";
            var search = "";
            if (ItemHead.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""ItemHeadId""='{ItemHead}'");
            }
            if (Vendor.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""VendorId""='{Vendor}'");
            }

            if (From.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""PoDate""::DATE>='{From}'::DATE");
            }
            if (To.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""PoDate""::DATE<='{To}'::DATE");
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<PurchaseOrderViewModel>(query, null);
        }
        public async Task<List<POItemsViewModel>> ReadPOItemsData(string poId)
        {
            var query = $@"select ri.""POQuantity"",poi.""PurcahseRate"" as PurchaseRate,poi.""TotalAmount"",
poi.""NtsNoteId"" as NoteId, im.""ItemName"",s.""ServiceNo"" as ""ReqCode"",poi.""RequisitionItemId"" as RequisitionItemId
, iu.""ShortName"" as ItemUOM, poi.""ItemQuantity"",ri.""PurchaseRate"" as ItemPurchaseRate 
from cms.""N_IMS_IMS_PO_ITEM"" as poi
join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id"" = poi.""RequisitionItemId""
join cms.""N_IMS_IMS_ITEM_MASTER"" as im on im.""Id"" = ri.""Item""
 join cms.""N_IMS_Requisition"" as r on r.""Id"" = ri.""RequisitionId""
 join public.""NtsService"" as s on s.""UdfNoteTableId""=r.""Id""
left Join cms.""N_IMS_IMS_ITEM_UOM"" as iu on iu.""Id""=im.""ItemUnit"" 
 where poi.""POId""='{poId}'
";
            var queryData = await _queryRepo.ExecuteQueryList<POItemsViewModel>(query, null);
            var sno = 1;
            foreach (var item in queryData)
            {
                item.SNo = sno;
                sno++;
            }
            return queryData;
        }
        public async Task DeletePOItem(string poId)
        {
            var query = $@"Update cms.""N_IMS_IMS_PO_ITEM"" 
set ""IsDeleted""=true where ""Id""='{poId}'
";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<ItemsViewModel> GetRequisitionItemById(string Id)
        {
            var query = $@"select ds.*,ds.""NtsNoteId"" as NoteId
from cms.""N_IMS_RequisitionItems"" as ds
where ds.""Id""='{Id}'";
            return await _queryRepo.ExecuteQuerySingle<ItemsViewModel>(query, null);
        }
        public async Task<POItemsViewModel> GetPOItemById(string Id)
        {
            var query = $@"select ds.*,ds.""NtsNoteId"" as NoteId
from cms.""N_IMS_IMS_PO_ITEM"" as ds
where ds.""Id""='{Id}'";
            return await _queryRepo.ExecuteQuerySingle<POItemsViewModel>(query, null);
        }
        public async Task<IList<GoodsReceiptItemViewModel>> GetPOItemsByPOId(string poId)
        {
            var query = $@"select dsi.""Id"" as ReferenceHeaderItemId,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,ise.""Code"" as IsSerializable,
n.""NoteNo"" as NoteNo,dsi.""ReceivedQuantity"" as ReceivedQuantity,dsi.""ItemQuantity"" as POQuantity
from cms.""N_IMS_IMS_PO_ITEM"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_IMS_IMS_PO"" as ds on ds.""Id""=dsi.""POId"" and ds.""IsDeleted""=false
join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=dsi.""RequisitionItemId"" and ri.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=ri.""Item"" and i.""IsDeleted""=false  
left join public.""LOV"" as ise on ise.""Id""=i.""IsSerializable"" and ise.""IsDeleted""=false where ds.""Id""='{poId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<GoodsReceiptItemViewModel>(query, null);
        }

        public async Task<IList<GoodsReceiptItemViewModel>> GetGoodReceiptItemsByReceiptId(string receiptId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,i.""IsSerializable"" as IsSerializable,
n.""NoteNo"" as NoteNo,dsi.""ItemQuantity"" as ItemQuantity
from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_IMS_GOODS_RECEIPT"" as ds on ds.""Id""=dsi.""GoodReceiptId"" and ds.""IsDeleted""=false

join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false  
--left join public.""LOV"" as ise on ise.""Id""=i.""IsSerializable"" and ise.""IsDeleted""=false
where ds.""Id""='{receiptId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<GoodsReceiptItemViewModel>(query, null);
        }
        public async Task<IList<PurchaseOrderViewModel>> GetPOItemsData(string poId)
        {
            var query = $@"select po.*
from cms.""N_IMS_IMS_PO"" as po
join cms.""N_IMS_IMS_PO_ITEM"" as poi on po.""Id""=poi.""POId"" and poi.""IsDeleted""=false and poi.""CompanyId""='{_repo.UserContext.CompanyId}'
where po.""Id""='{poId}' and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            return await _queryRepo.ExecuteQueryList<PurchaseOrderViewModel>(query, null);
        }

        public async Task<IList<GoodsReceiptViewModel>> ReadDeliveryChallanData(string ItemHead, string Vendor, string From, string To, string poId, ImsReceiptTypeEnum? receiptType)
        {
            var query = $@"select po.*,po.""NtsNoteId"" as NoteId,s.""Id"" as ServiceId,lv.""Code"" as ServiceStatusCode,lv.""Name"" as ServiceStatusName,
s.""ServiceNo"" as ServiceNo,u.""Name"" as CreatedBy
from cms.""N_IMS_GOODS_RECEIPT"" as po

join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=po.""CreatedBy"" and u.""IsDeleted""=false 
where po.""IsDeleted""=false and po.""GoodsReceiptReferenceId""='{poId}' and po.""ReceiptType""='0'  #WHERE#";
            var search = "";
            if (ItemHead.IsNotNullAndNotEmpty())
            {
                //search = string.Concat(search, $@" and po.""ItemHeadId""='{ItemHead}'");
            }
            //if (poId.IsNotNullAndNotEmpty())
            //{
            //    search = string.Concat(search, $@" ");
            //}

            if (From.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""ReceiveDate""::DATE>='{From}'::DATE");
            }
            if (To.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and po.""ReceiveDate""::DATE<='{To}'::DATE");
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<GoodsReceiptViewModel>(query, null);
        }

        public async Task<PurchaseOrderViewModel> GetPOData(string serviceId)
        {
            var query = $@"select po.*
from cms.""N_IMS_IMS_PO"" as po
join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false
where s.""Id""='{serviceId}'";

            return await _queryRepo.ExecuteQuerySingle<PurchaseOrderViewModel>(query, null);
        }

        public async Task<List<POTermsAndConditionsViewModel>> ReadPOTermsData(string poId)
        {
            var query = $@"select potc.*,potc.""Id"" as POTCID,potc.""NtsNoteId"" as NoteId
from cms.""N_IMS_PO_TANDC"" as potc
 where potc.""POID""='{poId}' and potc.""IsDeleted""=false order by potc.""CreatedDate"" desc ";

            return await _queryRepo.ExecuteQueryList<POTermsAndConditionsViewModel>(query, null);
        }

        public async Task<IList<DirectSalesViewModel>> FilterDirectSalesData(DirectSalesSearchViewModel search)
        {
            var query = $@"select s.""Id"" as ServiceId,ds.""ProposalDate"" as ProposalDate,Sum(i.""Amount""::DECIMAL) as ProposalValue,
c.""CustomerName"" as CustomerName,s.""WorkflowStatus"" as WorkflowStatus,s.""ServiceNo"" as ServiceNo,lv.""Code"" as ServiceStatusCode
from cms.""N_IMS_IMS_DIRECT_SALES"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=ds.""Customer"" and c.""IsDeleted""=false 
left join cms.""N_IMS_DirectSaleItem"" as i on i.""DirectSalesId""=s.""Id"" and i.""IsDeleted""=false 
where 1=1 and ds.""IsDeleted""=false 
and ds.""ProposalDate""::DATE <= '{search.ToDate}'::DATE and ds.""ProposalDate""::DATE >= '{search.FromDate}'::DATE #WHERE#  group by 
ds.""Id"" ,s.""Id"" ,ds.""ProposalDate"" ,
c.""CustomerName"" ,s.""WorkflowStatus"" ,s.""ServiceNo"",lv.""Code"" ";
            var replace = $@"";
            if (search.Customer.IsNotNullAndNotEmpty())
            {
                replace = $@"and c.""Id""='{search.Customer}'";
            }


            if (search.WorkflowStatus.IsNotNullAndNotEmpty())
            {
                //if (search.WorkflowStatus == "All")
                //{
                //    //replace = $@"and lv.""Code"" IN ('Workflow Started','Workflow Drafted')";
                //}
                if (search.WorkflowStatus == "Won")
                {
                    replace = $@"and lv.""Code""='SERVICE_STATUS_COMPLETE'";
                }
                else if(search.WorkflowStatus == "Draft")
                {
                    replace = $@"and lv.""Code""='SERVICE_STATUS_DRAFT'";
                }
                else if(search.WorkflowStatus == "Pending")
                {
                    replace = $@"and lv.""Code""='SERVICE_STATUS_INPROGRESS'";
                }
                else
                {

                }

            }
            if (search.ProposalSource.IsNotNullAndNotEmpty())
            {
                replace = $@"and ds.""ProposalSource""='{search.ProposalSource}'";
            }
            query = query.Replace("#WHERE#", replace);
            return await _queryRepo.ExecuteQueryList<DirectSalesViewModel>(query, null);
        }
        public async Task<double> GetClosingBalance(string itemId, string warehouseId)
        {
            var query = $@"with cte as(
            select ri.""BalanceQuantity""::decimal as Quantity from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as ri
            join cms.""N_IMS_GOODS_RECEIPT"" as r on ri.""GoodReceiptId""=r.""Id"" and r.""IsDeleted""=false 
            join public.""NtsService"" as rs on rs.""UdfNoteTableId""=r.""Id"" and rs.""IsDeleted""=false 
            join public.""LOV"" as lv on lv.""Id""=rs.""ServiceStatusId"" and lv.""IsDeleted""=false 
            where ri.""IsDeleted""=false and ri.""ItemId""='{itemId}' 
            and lv.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') #WHERE#
            union all
            select st.""BalanceQuantity""::decimal as Quantity from cms.""N_IMS_ItemStock"" as st
            join public.""NtsNote"" as n on st.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false 
            join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false 
            where st.""IsDeleted""=false and st.""ItemId""='{itemId}' 
           and lv.""Code""='NOTE_STATUS_INPROGRESS'  #WHERE2# 
            )select coalesce(sum(Quantity),0) from cte
            ";
            var replace1 = "";
            var replace = "";
            if (warehouseId.IsNotNullAndNotEmpty())
            {
                replace = $@"and ri.""WarehouseId""='{warehouseId}'";
                replace1 = $@"and st.""WarehouseId""='{warehouseId}'";

            }
            query = query.Replace("#WHERE#", replace);
            query = query.Replace("#WHERE2#", replace1);
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }
        public async Task UpdateStockClosingBalance(string itemId, string warehouseId,double closingBalance)
        {
            var query = $@"Update  cms.""N_IMS_ItemStock"" set ""ClosingQuantity""='{closingBalance}',""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
where ""ItemId""='{itemId}' and ""WarehouseId""='{warehouseId}' ";
            await _queryRepo.ExecuteCommand(query, null);
            var closingQty = await GetClosingBalance(itemId, null);
            await UpdateItemMasterClosingBalance(itemId, closingQty);
        }
        private async Task UpdateItemMasterClosingBalance(string itemId, double closingBalance)
        {
            var query = $@"Update  cms.""N_IMS_IMS_ITEM_MASTER"" set ""ClosingQuantity""='{closingBalance}',""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
where ""Id""='{itemId}'  ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<List<RequisitionIssueItemsViewModel>> GetGoodReceiptItemsToIssue(string requisitionItemId, string itemId, string warehouseId, ImsReceiptTypeEnum receiptType)
        {
            var query = "";
            if (receiptType == ImsReceiptTypeEnum.PurchaseOrder)
            {
                query = $@"select s.""AdditionalInfo"",s.""Id"",s.""Id"" as ReferenceHeaderId,s.""OpeningQuantity"" as TransactionQuantity,
s.""BalanceQuantity"" as BalanceQuantity,s.""IssuedQuantity"" as AlreadyIssuedQuantity,s.""ItemId"" as ItemId,i.""IsSerializable"" as IsSerializable
from cms.""N_IMS_ItemStock"" as s
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=s.""ItemId"" and i.""IsDeleted""=false
where s.""ItemId""='{itemId}' and s.""WarehouseId""='{warehouseId}' and s.""IsDeleted""=false
union 
select s.""AdditionalInfo"",s.""Id"",s.""GoodReceiptId"" as ReferenceHeaderId,s.""ItemQuantity"" as TransactionQuantity ,
s.""BalanceQuantity"" as BalanceQuantity,s.""IssuedQuantity"" as AlreadyIssuedQuantity,i.""Id"" as ItemId,i.""IsSerializable"" as IsSerializable
from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as s
join cms.""N_IMS_GOODS_RECEIPT"" as gr on gr.""Id""=s.""GoodReceiptId"" and gr.""IsDeleted""=false
join cms.""N_IMS_IMS_PO"" as po on po.""Id""=gr.""GoodsReceiptReferenceId"" and po.""IsDeleted""=false
join cms.""N_IMS_IMS_PO_ITEM"" as poi on poi.""POId""=po.""Id"" and poi.""IsDeleted""=false
join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=poi.""RequisitionItemId"" and ri.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=ri.""Item"" and i.""IsDeleted""=false
where s.""IsDeleted""=false and i.""Id""='{itemId}' and ri.""Id""='{requisitionItemId}'
and s.""BalanceQuantity""::decimal>0.0



";
            }
            else if(receiptType == ImsReceiptTypeEnum.StockAdjustment)
            {
                 query = $@"select s.""AdditionalInfo"",s.""Id"",s.""Id"" as ReferenceHeaderId,s.""OpeningQuantity"" as TransactionQuantity,
s.""BalanceQuantity"" as BalanceQuantity,s.""IssuedQuantity"" as AlreadyIssuedQuantity,s.""ItemId"" as ItemId,i.""IsSerializable"" as IsSerializable
from cms.""N_IMS_ItemStock"" as s
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=s.""ItemId"" and i.""IsDeleted""=false
where s.""ItemId""='{itemId}' and s.""WarehouseId""='{warehouseId}' and s.""IsDeleted""=false
union 
select s.""AdditionalInfo"",s.""Id"",s.""GoodReceiptId"" as ReferenceHeaderId,s.""ItemQuantity"" as TransactionQuantity ,
s.""BalanceQuantity"" as BalanceQuantity,s.""IssuedQuantity"" as AlreadyIssuedQuantity,i.""Id"" as ItemId,i.""IsSerializable"" as IsSerializable
from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as s
join cms.""N_IMS_GOODS_RECEIPT"" as gr on gr.""Id""=s.""GoodReceiptId"" and gr.""IsDeleted""=false
join cms.""N_SNC_IMS_INVENTORY_StockAdjustment"" as po on po.""Id""=gr.""GoodsReceiptReferenceId"" and po.""IsDeleted""=false
join cms.""N_IMS_StockAdjustmentItem"" as poi on poi.""StockAdjustmentId""=po.""Id"" and poi.""IsDeleted""=false

join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=poi.""ItemId"" and i.""IsDeleted""=false
where s.""IsDeleted""=false and i.""Id""='{itemId}' and poi.""Id""='{requisitionItemId}'
and s.""BalanceQuantity""::decimal>0.0



";
            }
            else if (receiptType == ImsReceiptTypeEnum.StockTransfer)
            {
                query = $@"select s.""AdditionalInfo"",s.""Id"",s.""Id"" as ReferenceHeaderId,s.""OpeningQuantity"" as TransactionQuantity,
s.""BalanceQuantity"" as BalanceQuantity,s.""IssuedQuantity"" as AlreadyIssuedQuantity,s.""ItemId"" as ItemId,i.""IsSerializable"" as IsSerializable
from cms.""N_IMS_ItemStock"" as s
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=s.""ItemId"" and i.""IsDeleted""=false
where s.""ItemId""='{itemId}' and s.""WarehouseId""='{warehouseId}' and s.""IsDeleted""=false
union 
select s.""AdditionalInfo"",s.""Id"",s.""GoodReceiptId"" as ReferenceHeaderId,s.""ItemQuantity"" as TransactionQuantity ,
s.""BalanceQuantity"" as BalanceQuantity,s.""IssuedQuantity"" as AlreadyIssuedQuantity,i.""Id"" as ItemId,i.""IsSerializable"" as IsSerializable
from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as s
join cms.""N_IMS_GOODS_RECEIPT"" as gr on gr.""Id""=s.""GoodReceiptId"" and gr.""IsDeleted""=false
join public.""NtsService"" as ser on ser.""UdfNoteTableId""=gr.""Id"" and ser.""IsDeleted""=false  
join public.""LOV"" as lv on lv.""Id""=ser.""ServiceStatusId"" and lv.""IsDeleted""=false and lv.""Code""='SERVICE_STATUS_COMPLETE' 
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=s.""ItemId"" and i.""IsDeleted""=false
where s.""IsDeleted""=false and i.""Id""='{itemId}' 
and s.""BalanceQuantity""::decimal>0.0



";
            }

            return await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
        }

        public async Task<IList<GoodsReceiptViewModel>> GetChallanDetailsbyPOId(string poId)
        {
            var query = $@" select po.*,po.""NtsNoteId"" as NoteId,s.""Id"" as ServiceId,
                            s.""ServiceNo"" as PONo
                            from cms.""N_IMS_GOODS_RECEIPT"" as po
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false 
                            join public.""User"" as u on u.""Id""=po.""CreatedBy"" and u.""IsDeleted""=false 
                            where po.""IsDeleted""=false and po.""GoodsReceiptReferenceId""='{poId}' ";

            var list = await _queryRepo.ExecuteQueryList<GoodsReceiptViewModel>(query, null);
            return list;
        }

        public async Task<GoodsReceiptItemViewModel> GetGoodReceiptItemDetails(string receiptId)
        {
            var query = $@" select gri.""ItemQuantity"", gri.""ReferenceHeaderItemId"", reqItem.""POQuantity"", reqItem.""PurchaseRate"", i.""Id"" as ItemId, i.""ItemName""
                        from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as gri
                        join cms.""N_IMS_IMS_PO_ITEM"" as poItem on poItem.""Id"" = gri.""ReferenceHeaderItemId"" and poItem.""IsDeleted""=false
                        join cms.""N_IMS_RequisitionItems"" as reqItem on reqItem.""Id"" = poItem.""RequisitionItemId"" and reqItem.""IsDeleted""=false
                        join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = reqItem.""Item"" and i.""IsDeleted""=false
                        where gri.""GoodReceiptId"" = '{receiptId}' and gri.""IsDeleted""=false ";

            var list = await _queryRepo.ExecuteQuerySingle<GoodsReceiptItemViewModel>(query, null);
            return list;
        }
        public async Task<GoodsReceiptItemViewModel> GetGoodReceiptItemById(string id)
        {
            var query = $@" select gri.""ItemQuantity"", gri.""ReferenceHeaderItemId"", reqItem.""POQuantity"", reqItem.""PurchaseRate"", i.""Id"" as ItemId, i.""ItemName""
                        from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as gri
                        left join cms.""N_IMS_IMS_PO_ITEM"" as poItem on poItem.""Id"" = gri.""ReferenceHeaderItemId"" and poItem.""IsDeleted""=false
                        left join cms.""N_IMS_RequisitionItems"" as reqItem on reqItem.""Id"" = poItem.""RequisitionItemId"" and reqItem.""IsDeleted""=false
                        join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = gri.""ItemId"" and i.""IsDeleted""=false
                        where gri.""Id"" = '{id}' and gri.""IsDeleted""=false ";

            var list = await _queryRepo.ExecuteQuerySingle<GoodsReceiptItemViewModel>(query, null);
            return list;
        }

        public async Task UpdateInvoiceNoinGR(string Id, string invoiceNo)
        {
            var query = $@" UPDATE cms.""N_IMS_GOODS_RECEIPT"" SET ""InvoiceNo"" = '{invoiceNo}' where ""Id"" = '{Id}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<string> GetGoodReceiptItemIdByPoItemId(string id)
        {
            var query = $@" select ""Id"" from cms.""N_IMS_GOODS_RECEIPT_ITEM"" where ""ReferenceHeaderItemId"" = '{id}' ";
            var res = await _queryRepo.ExecuteQuerySingle<GoodsReceiptViewModel>(query, null);
            return res.GoodsReceiptReferenceId;
        }
        public async Task<List<ScheduleInvoiceViewModel>> GetRequisitiononFilters(string ItemHead, string From, string To)
        {
            var query = $@"select r.*,s.""ServiceNo""
from cms.""N_IMS_Requisition"" as r
join public.""NtsService"" as s on s.""UdfNoteTableId""=r.""Id"" and s.""IsDeleted""=false 
where r.""IsDeleted""=false  #WHERE#";
            var search = "";
            if (ItemHead.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and r.""ItemHead""='{ItemHead}'");
            }
             if (From.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and r.""RequisitionDate""::DATE>='{From}'::DATE");
            }
            if (To.IsNotNullAndNotEmpty())
            {
                search = string.Concat(search, $@" and r.""RequisitionDate""::DATE<='{To}'::DATE");
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<ScheduleInvoiceViewModel>(query, null);
        }

        public async Task<IList<StockTransferViewModel>> GetItemTransferredList(string from, string to, string challanNo)
        {
            var query = $@"select s.""Id"" as ServiceId, s.""ServiceNo"",st.""Id"" as UdfNoteTableId,st.""ChallanNo"",st.""WareHouseLegalEntityId"",st.""FromWarehouseId"",st.""ToWarehouseId"",
st.""TransferDate"",st.""TransferReason"",st.""BusinessUnitId"",st.""ReceiptId"",st.""IssueId"",st.""AllIssued"",s.""WorkflowStatus"",
fwh.""WarehouseName"" as FromWareHouse,twh.""WarehouseName"" as ToWareHouse,
--i.""ItemName"",sti.""TransferQuantity"",
case when st.""TransferDate"" is not null then u.""Name"" else null end as TransferredBy,
case when st.""TransferDate"" is not null then st.""TransferDate"" else null end as TransferDate,ss.""Code"" as ServiceStatusCode
--,riss.""Code"" as RequisitionIssueServiceStatusCode,ri.""Id"" as RequisitionIssueId
,grss.""Code"" as GoodsReceiptServiceStatusCode,gr.""Id"" as GoodsReceiptId,
grs.""Id"" as GoodsReceiptServiceId
from public.""NtsService"" as s
join cms.""N_IMS_STOCK_TRANSFER"" as st on s.""UdfNoteTableId""=st.""Id"" and st.""IsDeleted""=false
--join cms.""N_IMS_STOCK_TRANSFER_ITEM"" as sti on st.""Id""=sti.""StockTransferId"" and sti.""IsDeleted""=false
--join cms.""N_IMS_IMS_ITEM_MASTER"" as i on sti.""ItemId""=i.""Id"" and i.""IsDeleted""=false
join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as fwh on st.""FromWarehouseId""=fwh.""Id"" and fwh.""IsDeleted""=false
join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as twh on st.""ToWarehouseId""=twh.""Id"" and twh.""IsDeleted""=false
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
join public.""User"" as u on s.""LastUpdatedBy""=u.""Id"" and u.""IsDeleted""=false
--left join cms.""N_IMS_RequisitionIssue"" as ri on st.""Id""=ri.""IssueReferenceId"" and ri.""IsDeleted""=false
--left join public.""NtsService"" as ris on ri.""Id""=ris.""UdfNoteTableId"" and ris.""IsDeleted""=false
--left join public.""LOV"" as riss on ris.""ServiceStatusId""=riss.""Id"" and riss.""IsDeleted""=false
left join cms.""N_IMS_GOODS_RECEIPT"" as gr on st.""Id""=gr.""GoodsReceiptReferenceId"" and gr.""IsDeleted""=false
left join public.""NtsService"" as grs on gr.""Id""=grs.""UdfNoteTableId"" and grs.""IsDeleted""=false
left join public.""LOV"" as grss on grs.""ServiceStatusId""=grss.""Id"" and grss.""IsDeleted""=false
where s.""IsDeleted""=false #DATEWHERE# #WHERE# order by s.""CreatedDate"" desc ";

            var datewhere = "";
            if(from.IsNotNullAndNotEmpty() && to.IsNotNullAndNotEmpty())
            {
                datewhere = $@" and (st.""TransferDate"" is null or st.""TransferDate""::DATE <= '{to}'::DATE and st.""TransferDate""::DATE >= '{from}'::DATE) ";
            }
            query = query.Replace("#DATEWHERE#", datewhere);
            var where = "";
            if (challanNo.IsNotNullAndNotEmpty())
            {
                where = $@" and st.""ChallanNo""='{challanNo}' ";
            }
            query = query.Replace("#WHERE#", where);

            return await _queryRepo.ExecuteQueryList<StockTransferViewModel>(query, null);
        }

        public async Task<IList<StockTransferViewModel>> GetTransferItemsList(string stockTransferId)
        {
            var query = $@"select sti.""Id"" as StockTransferItemId,sti.""ItemId"",i.""ItemName"",sti.""TransferQuantity"",sti.""IssuedQuantity"",sti.""NtsNoteId"",i.""IsSerializable"" as IsSerializable,sti.""Issued"" as Issued
from cms.""N_IMS_STOCK_TRANSFER"" as st 
join cms.""N_IMS_STOCK_TRANSFER_ITEM"" as sti on st.""Id""=sti.""StockTransferId"" and sti.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on sti.""ItemId""=i.""Id"" and i.""IsDeleted""=false
where st.""Id""='{stockTransferId}' and st.""IsDeleted""=false ";           

            return await _queryRepo.ExecuteQueryList<StockTransferViewModel>(query, null);
        }
        public async Task<StockTransferViewModel> GetTransferById(string stockTransferId)
        {
            var query = $@"select *,""NtsNoteId"" as NoteId
from cms.""N_IMS_STOCK_TRANSFER"" 
where ""Id""='{stockTransferId}' and ""IsDeleted""=false ";

            return await _queryRepo.ExecuteQuerySingle<StockTransferViewModel>(query, null);
        }
        public async Task<List<POInvoiceViewModel>> GetPOInvoiceDetailsList(string poId)
        {
            var query = $@" select pi.""Id"" as POInvoiceId ,pi.""InvoiceNo"",pi.""InvoiceDate"",pi.""PoId"",
                        pii.""POItemId"",
                        u.""Name"" as CreatedBy, s.""CreatedDate""::Date,
                        SUM(CAST(pii.""ReceivedQuantity"" as decimal) * CAST(r.""PurchaseRate"" as decimal)) as InvoiceAmount

                        from cms.""N_IMS_PO_INVOICE"" as pi
                        join cms.""N_IMS_PO_INVOICE_ITEM"" as pii on pii.""POInvoiceId"" = pi.""Id"" and pii.""IsDeleted"" = false
                        join cms.""N_IMS_IMS_PO_ITEM"" as poi on poi.""Id"" = pii.""POItemId"" and poi.""IsDeleted"" = false
                        join cms.""N_IMS_RequisitionItems"" as r on r.""Id"" = poi.""RequisitionItemId"" and r.""IsDeleted"" = false
                        join public.""NtsService"" as s on s.""UdfNoteTableId""=pi.""Id"" and s.""IsDeleted"" = false 
                        join public.""User"" as u on u.""Id""=pi.""CreatedBy"" and u.""IsDeleted"" = false
                        where pi.""IsDeleted""=false and pi.""PoId"" = '{poId}' 
                        group by pi.""Id"", pi.""InvoiceNo"", pi.""InvoiceDate"", pi.""PoId"", pii.""POItemId"", r.""PurchaseRate"", u.""Name"", s.""CreatedDate""
                        ";
            var res = await _queryRepo.ExecuteQueryList<POInvoiceViewModel>(query, null);
            return res;

        }

        public async Task<POInvoiceViewModel> InvoiceNoExists(string invoiceNo)
        {
            var query = $@" select * from cms.""N_IMS_PO_INVOICE"" where ""InvoiceNo"" = '{invoiceNo}' ";
            var res = await _queryRepo.ExecuteQuerySingle<POInvoiceViewModel>(query, null);
            return res;
        }
        public async Task<List<SerialNoViewModel>> GetSerailNoByHeaderIdandReferenceId(string referenceId,string hearderId)
        {
            var query = $@" select *,""NtsNoteId"" as NoteId from cms.""N_IMS_ITEMS_SERIALNO"" where ""ReferenceId"" = '{referenceId}'  and ""ReferenceHeaderId""='{hearderId}' and ""Issued"" is null or ""Issued""= 'false'";
            var res = await _queryRepo.ExecuteQueryList<SerialNoViewModel>(query, null);
            return res;
        }

        public async Task updateSerialNosToIssued(string serialNoIds)
        {
            var query = $@" Update cms.""N_IMS_ITEMS_SERIALNO"" set  ""Issued""=true  where ""Id"" = '{serialNoIds}'  ";
            await _queryRepo.ExecuteCommand(query, null);
           
        }

        public async Task<double> GetTotalQtyInHandCount()
        {
            var query = $@"select SUM(Cast(itms.""ClosingQuantity"" as INTEGER)) from cms.""N_IMS_ItemStock"" as itms
                        join public.""NtsNote"" as nitem on itms.""NtsNoteId"" = nitem.""Id"" and nitem.""IsDeleted"" = false
                        join public.""LOV"" as l on l.""Id"" = nitem.""NoteStatusId"" and l.""IsDeleted"" = false
                        and l.""Code"" = 'NOTE_STATUS_INPROGRESS'
                        where itms.""ClosingQuantity"" is not null and itms.""IsDeleted"" = false";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }

        public async Task<double> GetTotalQtyTobeRecievedCount()
        {
            var query = $@"Select Sum(cast(poIt.""ItemQuantity"" as INTEGER)-cast(poIt.""ReceivedQuantity"" as INTEGER))
                            From public.""NtsService"" as s
                            join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false
                            join cms.""N_IMS_IMS_PO"" as po on n.""Id""=po.""NtsNoteId"" and po.""IsDeleted""=false
                            join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
                            join cms.""N_IMS_IMS_PO_ITEM"" as poIt on poIt.""POId"" = po.""Id"" and poIt.""IsDeleted""=false
                            and ss.""Code"" = 'SERVICE_STATUS_COMPLETE' 
                            and poIt.""ReceivedQuantity"" is not null
                            and poIt.""ItemQuantity"" is not null
                            and s.""IsDeleted""=false";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }

        public async Task<double> GetTotalAllItem()
        {
            var query = $@"select Count(itmm.*) from cms.""N_IMS_IMS_ITEM_MASTER"" as itmm
                            join public.""NtsNote"" as nitem on itmm.""NtsNoteId"" = nitem.""Id"" and nitem.""IsDeleted"" = false
                            join public.""LOV"" as l on l.""Id"" = nitem.""NoteStatusId"" and l.""IsDeleted"" = false
                            and l.""Code"" = 'NOTE_STATUS_INPROGRESS'";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }

        public async Task<double> GetTotalLowStockItems()
        {
            var query = $@"select Count(itm.*) from cms.""N_IMS_IMS_ITEM_MASTER""as itm
                                join public.""NtsNote"" as nitem on itm.""NtsNoteId"" = nitem.""Id""  and nitem.""IsDeleted"" = false
                                join  cms.""N_IMS_ItemStock"" as itms on itm.""Id"" = itms.""ItemId""
                                join public.""NtsNote"" as nitems on itms.""NtsNoteId"" = nitems.""Id"" and nitems.""IsDeleted"" = false
                                where Cast(itms.""MinimumQuantity"" as INTEGER) > 0 
                                and Cast(itms.""ClosingQuantity"" as INTEGER) < Cast(itms.""MinimumQuantity"" as INTEGER)
                                and itm.""IsDeleted"" = false and itms.""IsDeleted"" = false";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }

        public async Task<double> GetTotalAllItemGroupItems()
        {
            var query = $@"select count(*) from (select Count(itmc.*) from cms.""N_IMS_IMS_ITEM_MASTER""as itm
join cms.""N_IMS_ITEM_SUB_CATEGORY"" as itmc on itmc.""Id""=itm.""ItemSubCategory"" and itmc.""IsDeleted"" = false
                                join public.""NtsNote"" as nitem on itm.""NtsNoteId"" = nitem.""Id""  and nitem.""IsDeleted"" = false
                                join  cms.""N_IMS_ItemStock"" as itms on itm.""Id"" = itms.""ItemId""
                                join public.""NtsNote"" as nitems on itms.""NtsNoteId"" = nitems.""Id"" and nitems.""IsDeleted"" = false
                                where Cast(itms.""MinimumQuantity"" as INTEGER) > 0 
                                and Cast(itms.""ClosingQuantity"" as INTEGER) < Cast(itms.""MinimumQuantity"" as INTEGER)
                                and itm.""IsDeleted"" = false and itms.""IsDeleted"" = false group by itmc.""Name"") as a
                            ";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }
        public async Task<double> GetTotalItemsInHand()
        {
            var query = $@"select Sum(itms.""ClosingQuantity""::int) from cms.""N_IMS_IMS_ITEM_MASTER""as itm

                                join public.""NtsNote"" as nitem on itm.""NtsNoteId"" = nitem.""Id""  and nitem.""IsDeleted"" = false
                                join  cms.""N_IMS_ItemStock"" as itms on itm.""Id"" = itms.""ItemId""
                                join public.""NtsNote"" as nitems on itms.""NtsNoteId"" = nitems.""Id"" and nitems.""IsDeleted"" = false";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }
        public async Task<double> GetTotalItemsToReceive()
        {
            var query = $@"select Sum(itm.""ItemQuantity""::int- case when itm.""ReceivedQuantity"" is null then 0 else itm.""ReceivedQuantity""::int end)
 from cms.""N_IMS_IMS_PO_ITEM"" as itm
                                join public.""NtsNote"" as nitem on itm.""NtsNoteId"" = nitem.""Id""  and nitem.""IsDeleted"" = false
								join cms.""N_IMS_IMS_PO"" as po on po.""Id"" = itm.""POId"" and po.""IsDeleted"" = false
                                join  public.""NtsService"" as s on s.""UdfNoteId"" = po.""NtsNoteId"" and s.""IsDeleted"" = false
								join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""Code""='SERVICE_STATUS_COMPLETE' ";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }

        public async Task<IList<IdNameViewModel>> GetTopSellingsItem(DateTime startDate, DateTime endDate)
        {
            var query = $@"SELECT Sum(cast(ds.""ItemQuantity"" as INTEGER)) as Code,
                            case when i.""ItemName"" is null then 'NA' else i.""ItemName"" end as Name
                            FROM cms.""N_IMS_DirectSaleItem"" as ds
                            join cms.""N_IMS_IMS_ITEM_MASTER"" as i on ds.""Item"" = i.""Id""
 join public.""NtsService"" as s on s.""Id"" = ds.""DirectSalesId""
                            join cms.""N_IMS_IMS_DIRECT_SALES"" as ids on ids.""Id"" = s.""UdfNoteTableId""
                           
                            join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
                            where ss.""Code"" = 'SERVICE_STATUS_COMPLETE' and 
                            ids.""OrderDate"" >= '{startDate}'
                            and ids.""OrderDate"" < '{endDate}'
                            Group by ds.""Item"",  i.""ItemName""
                            order by Code  desc  limit 5";
            var data = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return data;
        }

        public async Task<double> GetPurchaseOrderQtyOrdered(DateTime startDate, DateTime endDate)
        {
            var query = $@"SELECT Sum(cast(""ItemQuantity"" as Integer)) FROM cms.""N_IMS_IMS_PO_ITEM""
                            where ""IsDeleted"" = false
                            and 
                            ""CreatedDate"" >= '{startDate}'
                            and ""CreatedDate"" <  '{endDate}'";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }

        public async Task<double> GetPurchaseOrderTotaCost(DateTime startDate, DateTime endDate)
        {
            var query = $@"SELECT Sum(cast(""TotalAmount"" as Integer)) FROM cms.""N_IMS_IMS_PO_ITEM""
                        where ""IsDeleted"" = false and 
                            ""CreatedDate"" >= '{startDate}'
                            and ""CreatedDate"" <  '{endDate}'";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }

        public async Task<IList<SalesOrder>> GetSalesOrderSummaryChart(DateTime startDate, DateTime endDate, InventoryDataFilterEnum filter)
        {
            var query = "";
            if (filter == InventoryDataFilterEnum.ThisWeek || filter == InventoryDataFilterEnum.PreviousWeek)
            {
                query = $@"With CTE as (SELECT Sum(cast(ds.""Amount"" as INTEGER)) as ""Amount"", Cast(ids.""OrderDate"" as TIMESTAMP) as ""OrderDate"" ,			  
                        case when i.""ItemName"" is null then 'NA' else i.""ItemName"" end as ""ItemName""
                        FROM cms.""N_IMS_DirectSaleItem"" as ds
                        join cms.""N_IMS_IMS_ITEM_MASTER"" as i on ds.""Item"" = i.""Id""
                     
                        join public.""NtsService"" as s on s.""Id"" = ds.""DirectSalesId""
   join cms.""N_IMS_IMS_DIRECT_SALES"" as ids on ids.""Id"" = s.""UdfNoteTableId""
                        join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
		                         where ss.""Code"" = 'SERVICE_STATUS_COMPLETE'  and 
                                             ids.""OrderDate"" >= '{startDate}'
                                                   and ids.""OrderDate"" <  '{endDate}'
			                         Group By ""ItemName"", ""OrderDate""
			                         limit 5
                        )
                        select ""ItemName"", ""Amount"", ""OrderDate"",
                        to_char(""OrderDate"", 'Day') AS ""Range"" ,
                        EXTRACT(ISODOW  FROM  ""OrderDate"") as ""RangeNumber""
                        --to_char(""OrderDate"", 'Day') AS ""DayName"",
                        --EXTRACT(DAY FROM  ""OrderDate"") as ""Day"",
                        --EXTRACT(YEAR FROM  ""OrderDate"") as ""Year"",
                        --EXTRACT(QUARTER FROM  ""OrderDate"") as ""QUARTER""
                        from cte 
                        Group by cte.""OrderDate"",  cte.""ItemName"", cte.""Amount"", ""Range"", ""RangeNumber"" --, ""DayName"", ""Day"",""Year"", ""QUARTER""
                        order by ""RangeNumber""  desc";
            }
            if (filter == InventoryDataFilterEnum.ThisMonth || filter == InventoryDataFilterEnum.PreviousMonth)
            {
                query = $@"With CTE as (SELECT Sum(cast(ds.""Amount"" as INTEGER)) as ""Amount"", Cast(ids.""OrderDate"" as TIMESTAMP) as ""OrderDate"" ,			  
                        case when i.""ItemName"" is null then 'NA' else i.""ItemName"" end as ""ItemName""
                        FROM cms.""N_IMS_DirectSaleItem"" as ds
                        join cms.""N_IMS_IMS_ITEM_MASTER"" as i on ds.""Item"" = i.""Id""
                        
                        join public.""NtsService"" as s on s.""Id"" =  ds.""DirectSalesId""
join cms.""N_IMS_IMS_DIRECT_SALES"" as ids on ids.""Id"" = s.""UdfNoteTableId""
                        join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
		                         where ss.""Code"" = 'SERVICE_STATUS_COMPLETE'  and 
                                             ids.""OrderDate"" >= '{startDate}'
                                                   and ids.""OrderDate"" <  '{endDate}'
			                         Group By ""ItemName"", ""OrderDate""
			                         limit 5
                        )
                        select ""ItemName"", ""Amount"", ""OrderDate"",
                        EXTRACT(Day  FROM  ""OrderDate"") AS ""Range"" ,
                        EXTRACT(Day  FROM  ""OrderDate"") as ""RangeNumber""
                        --to_char(""OrderDate"", 'Day') AS ""DayName"",
                        --EXTRACT(DAY FROM  ""OrderDate"") as ""Day"",
                        --EXTRACT(YEAR FROM  ""OrderDate"") as ""Year"",
                        --EXTRACT(QUARTER FROM  ""OrderDate"") as ""QUARTER""
                        from cte 
                        Group by cte.""OrderDate"",  cte.""ItemName"", cte.""Amount"", ""Range"", ""RangeNumber"" --, ""DayName"", ""Day"",""Year"", ""QUARTER""
                        order by ""RangeNumber""  desc";
            }
            if (filter == InventoryDataFilterEnum.ThisYear || filter == InventoryDataFilterEnum.PreviousYear)
            {
                query = $@"With CTE as (SELECT Sum(cast(ds.""Amount"" as INTEGER)) as ""Amount"", Cast(ids.""OrderDate"" as TIMESTAMP) as ""OrderDate"" ,			  
                        case when i.""ItemName"" is null then 'NA' else i.""ItemName"" end as ""ItemName""
                        FROM cms.""N_IMS_DirectSaleItem"" as ds
                        join cms.""N_IMS_IMS_ITEM_MASTER"" as i on ds.""Item"" = i.""Id""
                        
                        join public.""NtsService"" as s on s.""Id"" = ds.""DirectSalesId""
join cms.""N_IMS_IMS_DIRECT_SALES"" as ids on ids.""Id"" = s.""UdfNoteTableId""
                        join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
		                         where ss.""Code"" = 'SERVICE_STATUS_COMPLETE'  and 
                                             ids.""OrderDate"" >= '{startDate}'
                                                   and ids.""OrderDate"" <  '{endDate}'
			                         Group By ""ItemName"", ""OrderDate""
			                         limit 5
                        )
                        select ""ItemName"", ""Amount"", ""OrderDate"",
                        to_char(""OrderDate"", 'Month') AS ""Range"" ,
                        EXTRACT(Month  FROM  ""OrderDate"") as ""RangeNumber""
                        --to_char(""OrderDate"", 'Day') AS ""DayName"",
                        --EXTRACT(DAY FROM  ""OrderDate"") as ""Day"",
                        --EXTRACT(YEAR FROM  ""OrderDate"") as ""Year"",
                        --EXTRACT(QUARTER FROM  ""OrderDate"") as ""QUARTER""
                        from cte 
                        Group by cte.""OrderDate"",  cte.""ItemName"", cte.""Amount"", ""Range"", ""RangeNumber"" --, ""DayName"", ""Day"",""Year"", ""QUARTER""
                        order by ""RangeNumber""  desc";
            }
            if (filter == InventoryDataFilterEnum.ThisQuarter || filter == InventoryDataFilterEnum.PreviousQuarter)
            {
                query = $@"With CTE as (SELECT Sum(cast(ds.""Amount"" as INTEGER)) as ""Amount"", Cast(ids.""OrderDate"" as TIMESTAMP) as ""OrderDate"" ,			  
                        case when i.""ItemName"" is null then 'NA' else i.""ItemName"" end as ""ItemName""
                        FROM cms.""N_IMS_DirectSaleItem"" as ds
                        join cms.""N_IMS_IMS_ITEM_MASTER"" as i on ds.""Item"" = i.""Id""
                        
                        join public.""NtsService"" as s on s.""Id"" =ds.""DirectSalesId""
join cms.""N_IMS_IMS_DIRECT_SALES"" as ids on ids.""Id"" = s.""UdfNoteTableId""
                        join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
		                         where ss.""Code"" = 'SERVICE_STATUS_COMPLETE'  and 
                                             ids.""OrderDate"" >= '{startDate}'
                                                   and ids.""OrderDate"" <  '{endDate}'
			                         Group By ""ItemName"", ""OrderDate""
			                         limit 5
                        )
                        select ""ItemName"", ""Amount"", ""OrderDate"",
                       Concat('Quarter ', EXTRACT(QUARTER  FROM  ""OrderDate"")) AS ""Range"" ,
                        EXTRACT(QUARTER  FROM  ""OrderDate"") as ""RangeNumber""
                        --to_char(""OrderDate"", 'Day') AS ""DayName"",
                        --EXTRACT(DAY FROM  ""OrderDate"") as ""Day"",
                        --EXTRACT(YEAR FROM  ""OrderDate"") as ""Year"",
                        --EXTRACT(QUARTER FROM  ""OrderDate"") as ""QUARTER""
                        from cte 
                        Group by cte.""OrderDate"",  cte.""ItemName"", cte.""Amount"", ""Range"", ""RangeNumber"" --, ""DayName"", ""Day"",""Year"", ""QUARTER""
                        order by ""RangeNumber""  desc";
            }
            var model = new List<SalesOrder>();

            var data = await _queryRepo.ExecuteQueryList<SalesOrder>(query, null);
            if (data.IsNotNull())
            {
                var nameList = data.Select(x => x.ItemName).Distinct();
                var categories = data.Select(x => x.Range).Distinct();
                foreach (var name in nameList)
                {
                    var amount = data.Where(x => x.ItemName == name).Select(x => x.Amount.ToSafeInt());
                    model.Add(new SalesOrder { name = name, data = amount.ToList(),
                    categories = categories.ToList()});
                }
            }

            return model;
        }

        public async Task<double> GetDirectSalesAmountSummaryChart(object startDate, object endDate, InventoryDataFilterEnum filter)
        {
            var query = $@"SELECT Coalesce(Sum(cast(ds.""Amount"" as INTEGER)), 0)
                            FROM cms.""N_IMS_DirectSaleItem"" as ds
                            join cms.""N_IMS_IMS_ITEM_MASTER"" as i on ds.""Item"" = i.""Id""
                           
                            join public.""NtsService"" as s on s.""Id"" =ds.""DirectSalesId""
 join cms.""N_IMS_IMS_DIRECT_SALES"" as ids on ids.""Id"" = s.""UdfNoteTableId""
                            join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
		                            where ss.""Code"" = 'SERVICE_STATUS_COMPLETE'  and 
                                              ids.""OrderDate"" >= '{startDate}'
                                              and ids.""OrderDate"" <  '{endDate}'";
            var data = await _queryRepo.ExecuteScalar<double>(query, null);
            return data;
        }

        public async Task<List<ItemStockViewModel>> ReadStockEntriesData(string itemId,string warehouseId,DateTime? FromDate,DateTime? ToDate)
        {
            var query = $@"select s.""AdditionalInfo"",s.""Id"",s.""OpeningQuantity"" as TransactionQuantity,u.""Name"" as CreatedBy,s.""TransactionDate""::Date,
s.""Id"" as ReferenceId,s.""Id"" as ReferenceHeaderId,
s.""ItemId"" as ItemId
from cms.""N_IMS_ItemStock"" as s
join public.""User"" as u on u.""Id""=s.""CreatedBy"" 
where s.""ItemId""='{itemId}' and s.""WarehouseId""='{warehouseId}' and s.""IsDeleted""=false #WHERE#
union 
select s.""AdditionalInfo"",s.""Id"",s.""ItemQuantity"" as TransactionQuantity ,u.""Name"" as CreatedBy,s.""CreatedDate""::Date as TransactionDate,
s.""Id"" as ReferenceId,s.""GoodReceiptId"" as ReferenceHeaderId,
i.""Id"" as ItemId
from cms.""N_IMS_GOODS_RECEIPT_ITEM"" as s
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=s.""ItemId"" and i.""IsDeleted""=false
join public.""User"" as u on u.""Id""=s.""CreatedBy""
where s.""IsDeleted""=false and i.""Id""='{itemId}' #WHERE# 
union 
select s.""AdditionalInfo"",s.""Id"",s.""IssuedQuantity"" as TransactionQuantity,u.""Name"" as CreatedBy,s.""CreatedDate"",
s.""Id"" as ReferenceId,s.""RequisitionIssueId"" as ReferenceHeaderId,
i.""Id"" as ItemId
from cms.""N_IMS_RequisitionIssueItem"" as s
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=s.""ItemId"" and i.""IsDeleted""=false
join public.""User"" as u on u.""Id""=s.""CreatedBy"" 
where s.""IsDeleted""=false and i.""Id""='{itemId}' #WHERE#";
            var search = "";
            if (FromDate.IsNotNull())
            {
                search = string.Concat(search, $@" and s.""CreatedDate""::DATE>='{FromDate.Value}'::DATE");
            }
            if (ToDate.IsNotNull())
            {
                search = string.Concat(search, $@" and s.""CreatedDate""::DATE<='{ToDate.Value}'::DATE");
            }
            
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<ItemStockViewModel>(query,null);
        }

        public async Task<ItemStockViewModel> GetStockDetailsById(string stockId)
        {
            var query = $@" select s.*
                from cms.""N_IMS_ItemStock""  as s
                
                where s.""Id"" = '{stockId}' ";
            var queryData = await _queryRepo.ExecuteQuerySingle<ItemStockViewModel>(query, null);
            return queryData;
        }

        public async Task<ItemStockViewModel> GetStockDataByNoteId(string stockNoteId)
        {
            var query = $@" select * from cms.""N_IMS_ItemStock"" where ""NtsNoteId"" = '{stockNoteId}' ";
            var data = await _queryRepo.ExecuteQuerySingle<ItemStockViewModel>(query, null);
            return data;
        }

        public async Task<List<ItemsViewModel>> GetActiveItemsFilterBySubCategory(string subCategoryId)
        {
            var query = $@" select i.*, i.""NtsNoteId"" as NoteId, ss.""Name"" as ItemStatus, ss.""Code"" as ItemStatusCode,st.""ClosingQuantity"" as CurrentBalance
                            from cms.""N_IMS_IMS_ITEM_MASTER"" as i
                            left join public.""NtsNote"" as n on n.""Id"" = i.""NtsNoteId"" and n.""IsDeleted""=false
                            left join public.""LOV"" as ss on n.""NoteStatusId"" = ss.""Id"" and ss.""IsDeleted""=false
 left join cms.""N_IMS_ItemStock"" as st on i.""Id"" = st.""ItemId"" and st.""IsDeleted""=false
                            where ss.""Code"" = 'NOTE_STATUS_INPROGRESS' and i.""Status"" = '1' and i.""IsDeleted"" = false #WHERE#
                        ";
            var where = "";
            if (subCategoryId.IsNotNullAndNotEmpty())
            {
                where = $@" and i.""ItemSubCategory""='{subCategoryId}' ";
            }
            query = query.Replace("#WHERE#", where);

            var data = await _queryRepo.ExecuteQueryList<ItemsViewModel>(query, null);
            return data;
        }
        public async Task<IList<DirectSalesViewModel>> GetDirectSalesList()
        {
            var query = $@"Select distinct s.""ServiceNo"",s.""Id""
From public.""NtsService"" as s
join cms.""N_IMS_IMS_DIRECT_SALES"" as ds on s.""UdfNoteTableId""=ds.""Id"" and ds.""IsDeleted""=false
join cms.""N_IMS_DirectSaleItem"" as i on i.""DirectSalesId""=s.""Id"" and i.""IsDeleted""=false
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false --and ss.""Code""='SERVICE_STATUS_COMPLETE'
where s.""TemplateCode""='IMS_DIRECT_SALES' and s.""IsDeleted""=false ";

            var data = await _queryRepo.ExecuteQueryList<DirectSalesViewModel>(query, null);
            return data;
        }

        public async Task<IList<SalesReturnViewModel>> GetSalesReturnList(string cusId, string From, string To,string serNo)
        {
            var query = $@"Select s.""ServiceNo"",s.""Id"" as ServiceId,c.""CustomerName"",sr.""ReturnDate"",
s.""WorkflowStatus"" as ServiceStatusName,ss.""Code"" as ServiceStatusCode,cc.""ContactPersonName"",gr.""Id"" as GoodsReceiptId,
gr.""GoodsReceiptReferenceId"",grss.""Code"" as GoodsReceiptStatus,grs.""Id"" as GoodsReceiptServiceId,gr.""WarehouseId""
From public.""NtsService"" as s
join cms.""N_IMS_SALES_RETURN"" as sr on s.""UdfNoteTableId""=sr.""Id"" and sr.""IsDeleted""=false
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
join cms.""N_IMS_IMS_CUSTOMER"" as c on sr.""CustomerId""=c.""Id"" and c.""IsDeleted""=false
join cms.""N_IMS_CustomerContact"" as cc on sr.""CustomerContactId""=cc.""Id"" and cc.""IsDeleted""=false
left join cms.""N_IMS_GOODS_RECEIPT"" as gr on sr.""Id""=gr.""GoodsReceiptReferenceId"" and gr.""IsDeleted""=false
left join public.""NtsService"" as grs on gr.""Id""=grs.""UdfNoteTableId"" and grs.""IsDeleted""=false
left join public.""LOV"" as grss on grs.""ServiceStatusId""=grss.""Id"" and grss.""IsDeleted""=false
where s.""IsDeleted""=false and (sr.""ReturnDate""::Date >= '{From}'::Date and sr.""ReturnDate""::Date <= '{To}'::Date) #Where# order by s.""CreatedDate"" desc ";

            var where = cusId.IsNotNullAndNotEmpty() ? $@" and sr.""CustomerId""='{cusId}' " : "";
            where = serNo.IsNotNullAndNotEmpty() ? String.Concat(where, $@" and s.""ServiceNo""='{serNo}' ") : where;

            query = query.Replace("#Where#", where);

            var data = await _queryRepo.ExecuteQueryList<SalesReturnViewModel>(query, null);
            return data;
        }

        public async Task<SalesReturnViewModel> GetSalesReturnData(string serId)
        {
            var query = $@"Select sr.*,sr.""Id"" as SalesReturnId, s.""Id"" as ServiceId,ds.""Designation"",ds.""MobileNo"",ds.""ContactNo"",ds.""EmailId"" as Email, dss.""Id"" as DirectSaleServiceId,ss.""Code"" as ServiceStatusCode
from cms.""N_IMS_SALES_RETURN"" as sr
join public.""NtsService"" as s on sr.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
left join cms.""N_IMS_IMS_DIRECT_SALES"" as ds on sr.""DirectSaleId""=ds.""Id"" and ds.""IsDeleted""=false
left join public.""NtsService"" as dss on ds.""Id""=dss.""UdfNoteTableId"" and dss.""IsDeleted""=false
where sr.""IsDeleted""=false and s.""Id""='{serId}' ";

            var data = await _queryRepo.ExecuteQuerySingle<SalesReturnViewModel>(query, null);
            return data;
        }

        public async Task<IList<SalesReturnViewModel>> GetSalesReturnItemsList(string salesReturnId)
        {
            var query = $@"Select * from cms.""N_IMS_SALES_RETURN_ITEM"" where ""SalesReturnId""='{salesReturnId}' 
and ""IsDeleted""=false ";

            return await _queryRepo.ExecuteQueryList<SalesReturnViewModel>(query, null);
        }

        public async Task<IList<PurchaseReturnViewModel>> GetPurchaseReturnList(string cusId, string From, string To, string serNo)
        {
            var query = $@"Select s.""ServiceNo"",s.""Id"" as ServiceId,v.""VendorName"" as VendorName,pr.""ReturnDate"",
s.""WorkflowStatus"" as ServiceStatusName,ss.""Code"" as ServiceStatusCode,vc.""ContactPersonName"" as VendorContactPersonName
,gr.""Id"" as GoodsReceiptId,
gr.""GoodsReceiptReferenceId"",grss.""Code"" as GoodsReceiptStatus,grs.""Id"" as GoodsReceiptServiceId,gr.""WarehouseId""
From public.""NtsService"" as s
join cms.""N_SNC_IMS_PURCHASE_PurchaseReturn"" as pr on s.""UdfNoteTableId""=pr.""Id"" and pr.""IsDeleted""=false
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
join cms.""N_IMS_VENDOR"" as v on pr.""VendorId""=v.""Id"" and v.""IsDeleted""=false
join cms.""N_IMS_VendorContact"" as vc on pr.""VendorContactId""=vc.""Id"" and vc.""IsDeleted""=false
left join cms.""N_IMS_GOODS_RECEIPT"" as gr on pr.""Id""=gr.""GoodsReceiptReferenceId"" and gr.""IsDeleted""=false
left join public.""NtsService"" as grs on gr.""Id""=grs.""UdfNoteTableId"" and grs.""IsDeleted""=false
left join public.""LOV"" as grss on grs.""ServiceStatusId""=grss.""Id"" and grss.""IsDeleted""=false
where s.""IsDeleted""=false and (pr.""ReturnDate""::Date >= '{From}'::Date and pr.""ReturnDate""::Date <= '{To}'::Date) #Where# ";

            var where = cusId.IsNotNullAndNotEmpty() ? $@" and pr.""VendorId""='{cusId}' " : "";
            where = serNo.IsNotNullAndNotEmpty() ? String.Concat(where, $@" and s.""ServiceNo""='{serNo}' ") : where;

            query = query.Replace("#Where#", where);

            var data = await _queryRepo.ExecuteQueryList<PurchaseReturnViewModel>(query, null);
            return data;
        }

        public async Task<PurchaseReturnViewModel> GetPurchaseReturnData(string serId)
        {
//            var query = $@"Select sr.* from cms.""N_SNC_IMS_PURCHASE_PurchaseReturn"" as sr
//join public.""NtsService"" as s on sr.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
//where sr.""IsDeleted""=false and s.""Id""='{serId}' ";


            var query = $@"Select sr.*,dss.""Id"" as POId,sr.""Id"" as PurchaseReturnId, s.""Id"" as ServiceId,sr.""VendorId"",sr.""VendorContactId"",ss.""Code"" as ServiceStatusCode,ds.""ContactNo"",ds.""PhoneNo""
from cms.""N_SNC_IMS_PURCHASE_PurchaseReturn"" as sr
join public.""NtsService"" as s on sr.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
--join cms.""N_IMS_VENDOR"" as v on sr.""VendorId""=v.""Id"" and v.""IsDeleted""=false
--join cms.""N_IMS_VendorContact"" as vc on sr.""VendorContactId""=vc.""Id"" and vc.""IsDeleted""=false
left join cms.""N_IMS_IMS_PO"" as ds on sr.""POId""=ds.""Id"" and ds.""IsDeleted""=false
left join public.""NtsService"" as dss on ds.""Id""=dss.""UdfNoteTableId"" and dss.""IsDeleted""=false
where sr.""IsDeleted""=false and s.""Id""='{serId}' ";

            var data = await _queryRepo.ExecuteQuerySingle<PurchaseReturnViewModel>(query, null);
            return data;
        }
        public async Task<IList<StockAdjustmentViewModel>> GetStockAdjustmentList()
        {
            var query = $@"Select ds.*, s.""ServiceNo"",s.""Id"" as ServiceId,w.""WarehouseName"" as WarehouseName,ss.""Name"" as ServiceStatusName,u.""Name"" as CreatedBy,ss.""Code"" as ServiceStatusCode
From public.""NtsService"" as s
join cms.""N_SNC_IMS_INVENTORY_StockAdjustment"" as ds on s.""UdfNoteTableId""=ds.""Id"" and ds.""IsDeleted""=false
join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on ds.""WarehouseId""=w.""Id"" and w.""IsDeleted""=false
join public.""User"" as u on u.""Id""=ds.""CreatedBy"" and u.""IsDeleted""=false 
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false 
where  s.""IsDeleted""=false ";

            var data = await _queryRepo.ExecuteQueryList<StockAdjustmentViewModel>(query, null);
            return data;
        }
        public async Task<IList<StockAdjustmentItemViewModel>> GetStockAdjustmentItemsData(string stockAdjustmentId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_StockAdjustmentItem"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_SNC_IMS_INVENTORY_StockAdjustment"" as ds on ds.""Id""=dsi.""StockAdjustmentId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false where ds.""Id""='{stockAdjustmentId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<StockAdjustmentItemViewModel>(query, null);
        }
        public async Task<IList<SalesReturnViewModel>> GetPurchaseReturnItemsData(string serviceId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_PurchaseReturnItem"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join cms.""N_SNC_IMS_PURCHASE_PurchaseReturn"" as ds on ds.""Id""=dsi.""PurchaseReturnId"" and ds.""IsDeleted""=false
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false where s.""Id""='{serviceId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<SalesReturnViewModel>(query, null);
        }

        public async Task<List<SalesOrder>> GetItemValueByCategory()
        {
            var query = $@"select a.""Name"" as ItemName,Sum(a.""itemvalue"") as ItemValue from ( select  c.""Name"", 
	                        case when cast(itms.""ClosingQuantity"" as INTEGER) is not null and   cast(itms.""UnitRate"" as integer) is not null
	                        then Sum(cast(itms.""ClosingQuantity"" as INTEGER)) * cast(itms.""UnitRate"" as integer)  else 0 end as ItemValue
	                        from cms.""N_IMS_ItemStock"" as itms
                        join public.""NtsNote"" as nitem on itms.""NtsNoteId"" = nitem.""Id"" and nitem.""IsDeleted"" = false
                        join public.""LOV"" as l on l.""Id"" = nitem.""NoteStatusId"" and l.""IsDeleted"" = false and l.""Code"" = 'NOTE_STATUS_INPROGRESS'
                        join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = itms.""ItemId"" and i.""IsDeleted"" = false
                        join cms.""N_IMS_ITEM_SUB_CATEGORY"" as ic on ic.""Id"" = i.""ItemSubCategory"" and ic.""IsDeleted"" = false
                        join cms.""N_IMS_ITEM_CATEGORY"" as c on c.""Id"" = ic.""ItemCategory"" and c.""IsDeleted"" = false
                        where i.""Status"" = 1
                        Group by  c.""Name"", itms.""UnitRate"",itms.""ClosingQuantity"") as a group by a.""Name""";
            var data = await _queryRepo.ExecuteQueryList<SalesOrder>(query, null);
            return data;
        }
        
        public async Task<List<SalesOrder>> GetItemValueByWarehouse()
        {
            //var query = $@"select  i.""ItemName"" as ItemName, itms.""Id"" as Id, itms.""WarehouseId"",
            //             case when cast(itms.""ClosingQuantity"" as INTEGER) is not null and   cast(itms.""UnitRate"" as integer) is not null
            //             then Sum(cast(itms.""ClosingQuantity"" as INTEGER)) * cast(itms.""UnitRate"" as integer)  else 0 end as ItemValue
            //             from cms.""N_IMS_ItemStock"" as itms
            //            join public.""NtsNote"" as nitem on itms.""NtsNoteId"" = nitem.""Id"" and nitem.""IsDeleted"" = false
            //            join public.""LOV"" as l on l.""Id"" = nitem.""NoteStatusId"" and l.""IsDeleted"" = false and l.""Code"" = 'NOTE_STATUS_INPROGRESS'
            //            join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = itms.""ItemId"" and i.""IsDeleted"" = false
            //            join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as w on w.""Id"" = itms.""WarehouseId"" and w.""IsDeleted"" = false
            //            where i.""Status"" = 1
            //            Group by itms.""Id"", itms.""UnitRate"", i.""ItemName"", w.""Id""";
            var query = $@"select a.""WarehouseName"" as ItemName,Sum(a.""itemvalue"") as ItemValue from (select  c.""WarehouseName"",
	                        case when cast(itms.""ClosingQuantity"" as INTEGER) is not null and   cast(itms.""UnitRate"" as integer) is not null
	                        then Sum(cast(itms.""ClosingQuantity"" as INTEGER)) * cast(itms.""UnitRate"" as integer)  else 0 end as ItemValue
	                        from cms.""N_IMS_ItemStock"" as itms
                        join public.""NtsNote"" as nitem on itms.""NtsNoteId"" = nitem.""Id"" and nitem.""IsDeleted"" = false
                        join public.""LOV"" as l on l.""Id"" = nitem.""NoteStatusId"" and l.""IsDeleted"" = false and l.""Code"" = 'NOTE_STATUS_INPROGRESS'
                        join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id"" = itms.""ItemId"" and i.""IsDeleted"" = false
                        --join cms.""N_IMS_ITEM_SUB_CATEGORY"" as ic on ic.""Id"" = i.""ItemSubCategory"" and ic.""IsDeleted"" = false
                        join cms.""N_IMS_MASTERDATA_WareHouseMaster"" as c on c.""Id"" = itms.""WarehouseId"" and c.""IsDeleted"" = false
                        where i.""Status"" = 1
                        Group by  c.""WarehouseName"", itms.""UnitRate"",itms.""ClosingQuantity"") as a group by a.""WarehouseName""";
            var data = await _queryRepo.ExecuteQueryList<SalesOrder>(query, null);
            return data;
        }

        public async Task<IList<GoodsReceiptViewModel>> ReadGoodsReceiptData(string GoodsReceiptReferenceId, ImsReceiptTypeEnum receiptType)
        {
            var query = "";
            if (receiptType== ImsReceiptTypeEnum.StockAdjustment) 
            {
                query = $@"select po.*,po.""NtsNoteId"" as NoteId,s.""Id"" as ServiceId,lv.""Code"" as ServiceStatusCode,lv.""Name"" as ServiceStatusName,
s.""ServiceNo"" as ServiceNo,u.""Name"" as CreatedBy
from cms.""N_IMS_GOODS_RECEIPT"" as po

join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=po.""CreatedBy"" and u.""IsDeleted""=false 
where po.""IsDeleted""=false and po.""GoodsReceiptReferenceId""='{GoodsReceiptReferenceId}' and po.""ReceiptType""='2' ";
            }
            return await _queryRepo.ExecuteQueryList<GoodsReceiptViewModel>(query, null);
        }

        public async Task<List<PurchaseOrderViewModel>> GetPurchaseOrderList()
        {
            var query = $@"Select s.""ServiceNo"",s.""Id""
                            From public.""NtsService"" as s
                            join cms.""N_IMS_IMS_PO"" as ds on s.""UdfNoteTableId""=ds.""Id"" and ds.""IsDeleted""=false
                            join cms.""N_IMS_IMS_PO_ITEM"" as i on i.""POId""=ds.""Id"" and i.""IsDeleted""=false
                            join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false --and ss.""Code""='SERVICE_STATUS_COMPLETE'
                            where s.""TemplateCode""='IMS_PO' and s.""IsDeleted""=false ";
            var data = await _queryRepo.ExecuteQueryList<PurchaseOrderViewModel>(query, null);
            return data;
        }

        public async Task<PurchaseOrderViewModel> GetPurchaseOrderData(string serviceId)
        {
            var query = $@"select ds.*
                    from cms.""N_IMS_IMS_PO"" as ds
                    join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
                    where s.""Id""='{serviceId}'";

            return await _queryRepo.ExecuteQuerySingle<PurchaseOrderViewModel>(query, null);
        }

        public async Task<List<ItemsViewModel>> GetPurchaseOrderItemsList(string purchaseId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,
                n.""NoteNo"" as NoteNo,sri.""ReturnQuantity"",sri.""ReturnComment"" as ReturnReason,sri.""ReturnType"" as ReturnTypeId,sri.""NtsNoteId"",
                case when sri.""Id"" is not null then true else false end as CheckFlag
                from cms.""N_IMS_IMS_PO_ITEM"" as dsi
                join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false
                join cms.""N_IMS_IMS_PO"" as po on po.""Id"" = dsi.""POId""
                join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false
                join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id"" = dsi.""RequisitionItemId""
                join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=ri.""Item"" and i.""IsDeleted""=false 
                left join cms.""N_IMS_PurchaseReturnItem"" as sri on dsi.""Id""=sri.""POItemId"" and sri.""IsDeleted""=false
                --left join cms.""N_IMS_IMS_DIRECT_SALES"" as ds on s.""UdfNoteTableId""=ds.""Id"" and ds.""IsDeleted""=false
                --left join cms.""N_IMS_SALES_RETURN"" sr on ds.""Id"" = sr.""DirectSaleId"" and sr.""IsDeleted"" = false
                --left join cms.""N_IMS_SALES_RETURN_ITEM"" as sri on sr.""Id"" = sri.""SalesReturnId"" and sri.""IsDeleted"" = false
                where s.""Id""='{purchaseId}' and dsi.""IsDeleted""=false order by dsi.""CreatedDate"" desc ";

            return await _queryRepo.ExecuteQueryList<ItemsViewModel>(query, null);
        }

        public async Task<List<PurchaseReturnViewModel>> GetPurchaseReturnItemsList(string purchaseReturnId)
        {
            var query = $@"Select * from cms.""N_IMS_PurchaseReturnItem"" where ""PurchaseReturnId""='{purchaseReturnId}' 
            and ""IsDeleted""=false ";

            return await _queryRepo.ExecuteQueryList<PurchaseReturnViewModel>(query, null);
        }

        public async Task<GoodsReceiptViewModel> GetGoodsReceiptDataBySerId(string goodsReceiptServiceId)
        {
            var query = $@"select po.*
from cms.""N_IMS_GOODS_RECEIPT"" as po
join public.""NtsService"" as s on s.""UdfNoteTableId""=po.""Id"" and s.""IsDeleted""=false 
where po.""IsDeleted""=false and s.""Id""='{goodsReceiptServiceId}' ";
            
            return await _queryRepo.ExecuteQuerySingle<GoodsReceiptViewModel>(query, null);
        }
        public async Task<GoodsReceiptViewModel> GetGoodsReceiptById(string id)
        {
            var query = $@"select ds.*
from cms.""N_IMS_GOODS_RECEIPT"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
where ds.""Id""='{id}'";

            return await _queryRepo.ExecuteQuerySingle<GoodsReceiptViewModel>(query, null);
        }

        public async Task UpdateDeliveryNoteAcknowledgement(string deliveryNoteId, string fileId)
        {
            var query = $@"Update  cms.""N_IMS_IMS_DELIVERY_NOTE"" set ""AccknowledgeFileId""='{fileId}',""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
where ""Id""='{deliveryNoteId}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<DeliveryNoteViewModel> GetDeliveryNoteById(string deliveryNoteId) 
        {
            var query = $@"Select * from  cms.""N_IMS_IMS_DELIVERY_NOTE"" where ""Id""='{deliveryNoteId}' and ""IsDeleted""=false";
           return await _queryRepo.ExecuteQuerySingle<DeliveryNoteViewModel>(query, null);
        }

        public async Task<List<ScheduleInvoiceViewModel>> ReadScheduleInvoice(string customerId)
        {
            var query = $@"Select iss.* ,lv.""Name"" as AmountBase,s.""ServiceNo"" as RequisitionCode
from  cms.""N_IMS_InvoiceSchedule"" iss 
join cms.""N_IMS_Requisition"" as r on r.""Id""=iss.""RequisitionId"" and r.""IsDeleted""=false
join public.""NtsService"" as s on s.""UdfNoteTableId""=r.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=iss.""AmountBase"" and lv.""IsDeleted""=false
 where iss.""CustomerId""='{customerId}' and iss.""IsDeleted""=false";
            return await _queryRepo.ExecuteQueryList<ScheduleInvoiceViewModel>(query, null);
        }
    }
}
