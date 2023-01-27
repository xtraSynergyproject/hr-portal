using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
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

namespace CMS.Business
{
    public class InventoryManagementBusiness : BusinessBase<ServiceViewModel, NtsService>, IInventoryManagementBusiness
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
        public InventoryManagementBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IRepositoryQueryBase<DirectSalesViewModel> queryRepo,
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
        public async override Task<CommandResult<ServiceViewModel>> Create(ServiceViewModel model)
        {

            return CommandResult<ServiceViewModel>.Instance();
        }
        public async override Task<CommandResult<ServiceViewModel>> Edit(ServiceViewModel model)
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
n.""NoteNo"" as NoteNo
from cms.""N_IMS_DirectSaleItem"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false
join public.""NtsService"" as ds on ds.""Id""=dsi.""DirectSalesId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""Item"" and i.""IsDeleted""=false where ds.""Id""='{directSalesId}' and dsi.""IsDeleted""=false 
";
           
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

                            WHERE i.""IsDeleted""=false #WHERE#
                        ";
            var replace = $@"";
            if (search.ItemTypeId.IsNotNullAndNotEmpty())
            {
                replace = replace + $@" and lov.""Id""='{search.ItemTypeId}' ";
            }
            if (search.ItemCategoryId.IsNotNullAndNotEmpty())
            {
                replace = replace + $@" and ic.""Id""='{search.ItemCategoryId}' ";
            }
            if (search.ItemSubCategoryId.IsNotNullAndNotEmpty())
            {
                replace = replace + $@" and isc.""Id""='{search.ItemSubCategoryId}'";
            }
            if (search.ItemStatusId.IsNotNullAndNotEmpty())
            {
                replace = $@"and lovns.""Id""='{search.ItemStatusId}'";
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
        public async Task<IList<RequisitionViewModel>> GetRequisitionDataByItemHead(string itemHead,string Customer,string status,string From,string To)
        {
            var query = $@"select s.""Id"" as ServiceId,ds.""RequisitionDate"" as RequisitionDate,ds.""RequisitionValue"" as RequisitionValue,
c.""CustomerName"" as CustomerName,s.""WorkflowStatus"" as WorkflowStatus,s.""ServiceNo"" as ServiceNo,lv.""Code"" as ServiceStatusCode,lv.""Name"" as ServiceStatusName,ds.""RequisitionParticular"" as RequisitionParticular,ds.""CreatedBy"" as CreatedBy
from cms.""N_IMS_Requisition"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=ds.""Customer"" and c.""IsDeleted""=false 
--left join cms.""N_IMS_DirectSaleItem"" as i on i.""DirectSalesId""=s.""Id"" and i.""IsDeleted""=false 
where 1=1 and ds.""IsDeleted""=false #WHERE#  
--  group by 
--ds.""Id"" ,s.""Id"" ,ds.""RequisitionDate"" ,ds.""RequisitionParticular"",
--c.""CustomerName"" ,s.""WorkflowStatus"" ,s.""ServiceNo"",lv.""Code"" ";
            var search = "";
            if (itemHead.IsNotNullAndNotEmpty()) 
            {
                search =string.Concat(search, $@" and ds.""ItemHead""='{itemHead}'");
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
                search = string.Concat(search, $@" and ds.""RequisitionDate""::DATE<'{To}'::DATE");
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<RequisitionViewModel>(query, null);
        }
        public async Task<IList<RequisitionViewModel>> GetIssueRequisitionData(string itemHead, string From, string To)
        {
            var query = $@"select s.""Id"" as ServiceId,ds.""RequisitionDate"" as RequisitionDate,ds.""RequisitionValue"" as RequisitionValue,lv.""Name"" as ServiceStatusName,
c.""CustomerName"" as CustomerName,s.""WorkflowStatus"" as WorkflowStatus,s.""ServiceNo"" as ServiceNo,lv.""Code"" as ServiceStatusCode,ds.""RequisitionParticular"" as RequisitionParticular,u.""Name"" as CreatedBy
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
                search = string.Concat(search, $@" and ds.""RequisitionDate""::DATE <'{To}'::DATE");
            }
            query = query.Replace("#WHERE#", search);
            return await _queryRepo.ExecuteQueryList<RequisitionViewModel>(query, null);
        }

        public async Task<RequisitionViewModel> GetRequisitionDataByServiceId(string serviceId)
        {
            var query = $@"select ds.*,lv.""Name"" as ItemHead
from cms.""N_IMS_Requisition"" as ds
join public.""NtsService"" as s on s.""UdfNoteTableId""=ds.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=ds.""ItemHead"" and lv.""IsDeleted""=false
where s.""Id""='{serviceId}'";

            return await _queryRepo.ExecuteQuerySingle<RequisitionViewModel>(query, null);
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
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query,null);
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
        public async Task<IList<ItemsViewModel>> GetRequisistionItemsData(string requisitionId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionItems"" as dsi
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join public.""NtsService"" as ds on ds.""Id""=dsi.""RequisitionId"" and ds.""IsDeleted""=false
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

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItems(string issueServiceId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""RequisitionItemId"" as RequisitionItemId,
dsi.""RequisitionId"" as RequisitionId,ri.""ItemQuantity"" as ItemQuantity,dsi.""IssuedQuantity"" as IssuedQuantity,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionIssueItem"" as dsi
join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=dsi.""RequisitionItemId"" and ri.""IsDeleted""=false 
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join public.""NtsService"" as ds on ds.""Id""=dsi.""RequisitionIssueId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false where ds.""Id""='{issueServiceId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
        }

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItemsByRequisitionId(string requisitionServiceId)
        {
            var query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""RequisitionItemId"" as RequisitionItemId,
ds.""Id"" as RequisitionId,ri.""ItemQuantity"" as ItemQuantity,ri.""IssuedQuantity"" as IssuedQuantity,
n.""NoteNo"" as NoteNo
from cms.""N_IMS_RequisitionIssueItem"" as dsi
join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=dsi.""RequisitionItemId"" and ri.""IsDeleted""=false 
join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
join public.""NtsService"" as ds on ds.""Id""=dsi.""RequisitionIssueId"" and ds.""IsDeleted""=false
join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false where ri.""RequisitionId""='{requisitionServiceId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<RequisitionIssueItemsViewModel>(query, null);
        }
        public async Task UpdateRequisitionServiceToIssued(string requisitionId)
        {
            
            var noteTempModel = new ServiceTemplateViewModel();
            //noteTempModel.TemplateCode = note.TemplateCode;
            noteTempModel.ServiceId = requisitionId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            if (notemodel.IsNotNull()) 
            {
                var noteTempModel1 = new NoteTemplateViewModel();
                noteTempModel1.SetUdfValue = true;
                noteTempModel1.NoteId = notemodel.UdfNoteId;
                var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                if (rowData.ContainsKey("Issued"))
                {
                    rowData["Issued"] = "True";
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
            }
        }
        public async Task<IList<IssueRequisitionViewModel>> GetRequisistionIssue(string requisitionServiceId)
        {
            var query = $@"select dsi.*,dsi.""NtsNoteId"" as NoteId,lv.""Name"" as IssueType,c.""CustomerName"" as IssueTo,s.""Id"" as ServiceId
from cms.""N_IMS_RequisitionIssue"" as dsi
join cms.""N_IMS_IMS_CUSTOMER"" as c on c.""Id""=dsi.""IssueTo"" and c.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=dsi.""IssueType"" and lv.""IsDeleted""=false 
join public.""NtsService"" as s on s.""UdfNoteTableId""=dsi.""Id"" and s.""IsDeleted""=false 
 where dsi.""RequisitionId""='{requisitionServiceId}' and dsi.""IsDeleted""=false 
";

            return await _queryRepo.ExecuteQueryList<IssueRequisitionViewModel>(query, null);
        }
        public async Task<CommandResult<InventoryItemViewModel>> InsertInventory(InventoryItemViewModel model)
        {            
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "IMS_ITEM_INVENTORY";
            
            var note = await _noteBusiness.GetNoteDetails(noteTempModel);

            note.StartDate = DateTime.Now;

            model.InventoryQuantity = (model.InventoryAction == AddDeductEnum.Add) ? model.ItemQuantity : (model.ItemQuantity-1);

            note.Json = JsonConvert.SerializeObject(model);
            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            var result = await _noteBusiness.ManageNote(note);            
            
            return CommandResult<InventoryItemViewModel>.Instance(model);
        }
    }
}
