using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.EGOV;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Nest;
using DocumentFormat.OpenXml.Office2010.Excel;
using static Nest.JoinField;
using Org.BouncyCastle.Asn1.X500;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using ProtoBuf.Meta;
using System.Runtime.Intrinsics.Arm;
using UglyToad.PdfPig.Content;
using DocumentFormat.OpenXml.Wordprocessing;
using Syncfusion.EJ2.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
using Ubiety.Dns.Core.Records;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using SixLabors.ImageSharp.ColorSpaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.VariantTypes;
using System.Collections;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Syncfusion.EJ2.Grids;

namespace Synergy.App.Business
{
    public class SmartCityQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, ISmartCityQueryBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _querydata;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<ServiceTemplateViewModel> _query;
        private readonly INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryRepo1;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly IServiceProvider _serviceProvider;
        public SmartCityQueryPostgreBusiness(IRepositoryQueryBase<IdNameViewModel> querydata,
            IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper
            , IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<TaskViewModel> queryRepo1,
            IRepositoryQueryBase<ServiceTemplateViewModel> query, INoteBusiness noteBusiness, IUserContext userContext,
            ILOVBusiness lOVBusiness, IServiceBusiness serviceBusiness, IUserBusiness userBusiness,
            ITemplateCategoryBusiness templateCategoryBusiness, IServiceProvider serviceProvider) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _query = query;
            _noteBusiness = noteBusiness;
            _queryRepo1 = queryRepo1;
            _lOVBusiness = lOVBusiness;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _querydata = querydata;
            _userContext = userContext;
            _templateCategoryBusiness = templateCategoryBusiness;
            _serviceProvider = serviceProvider;
        }

        public async Task<JSCAssetConsumerViewModel> ReadJSCAssetConsumerData(string consumerId)
        {
            var query = $@"select cons.*,u.""Name"" as UserName 
                            from cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as cons
                            left join public.""User"" as u on u.""Id""=cons.""UserId"" and u.""IsDeleted""=false
                            where cons.""IsDeleted""=false and cons.""Id""='{consumerId}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCAssetConsumerViewModel>(query, null);
            return querydata;
        }

        public async Task<PropertyTaxPaymentViewModel> GetPropertyTaxReportData(string ddno, string year)
        {
            var query = $@"select p.""own_name"" as OwnerName,pr.""Year"" as BillingYear,pr.""Date"" as BillDate,
                             p.""ward_no"" as WardNo,p.""mmi_id"" as TenamentNo,pr.""ReceiptNumber"" as BillNo,pr.""ReceiptAmount"" as TotalAmount,
                             a.""TotalArea"" as TotalArea,a.""RateOfTax"" as RateOfTax,l.""Name"" as PropertyType,a.""LocationFactorRate"",
                             a.""UsageTypeRate"",
                             concat(p.""st_rd_name"",',',p.""locality"",',',p.""sector"",',',p.""permt_add"",',',p.""pin_code"") as Address
                             from public.""parcel"" as p
                             left join cms.""F_JSC_PROP_MGMNT_JSC_PropertyTax_Payment_Receipt"" as pr on pr.""DdnNo""=p.""mmi_id"" and pr.""IsDeleted""=false
                             left join cms.""F_JSC_PROP_MGMNT_SELF_ASSESSMENT_FORM"" as a on a.""DdnNo""=p.""mmi_id""  and a.""IsDeleted""=false
                             join public.""LOV"" as l on l.""Id"" = a.""PropertyType"" and l.""IsDeleted"" = false
                             where p.""mmi_id"" = '{ddno}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<PropertyTaxPaymentViewModel>(query, null);
            return querydata;
        }

        public async Task<List<PropertyTaxPaymentViewModel>> GetPropertyTaxReportFloorData(string ddno, string year)
        {
            var query = $@"select distinct a.""Id"",  f.""BuildingAge"" as AgeFact,af.""Factor"" as FloorRate,f.""Area"" as FloorArea
                            from public.""parcel"" as p
                            -- join cms.""F_JSC_PROP_MGMNT_JSC_PropertyTax_Payment_Receipt"" as pr on pr.""DdnNo""=p.""mmi_id"" and pr.""IsDeleted""=false
                            join cms.""F_JSC_PROP_MGMNT_SELF_ASSESSMENT_FORM"" as a on a.""DdnNo""=p.""mmi_id""  and a.""IsDeleted""=false
                            join cms.""F_JSC_PROP_MGMNT_PROPERTY_FLOOR_DETAIL"" as f on f.""ParentId""=a.""Id""  and f.""IsDeleted""=false
                            join cms.""F_JSC_MASTER_AgeFactor"" as af on f.""BuildingAge"">=af.""RangeFrom"" and f.""BuildingAge""<=af.""RangeTo""  and af.""IsDeleted""=false
                            where p.""mmi_id"" = '{ddno}' ";
            var querydata = await _queryRepo.ExecuteQueryList<PropertyTaxPaymentViewModel>(query, null);
            return querydata;
        }
        public async Task<IdNameViewModel> GetBuildingCategory(string buildingType)
        {
            var query = $@"select uf.""Name"" as Name,uf.""Id"" as Id
                            from cms.""F_JSC_PROP_MGMNT_UsageFactorCommercialProperty"" as p
                            join public.""LOV"" as uf on uf.""Id""=p.""CategoryId"" 
                            where p.""BuilldingTypeId"" like '%{buildingType}%'
                            ";
            var querydata = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<JSCPropertySelfAssessmentViewModel> GetSelfAssessmentData(string assessmentId)
        {
            var query = $@"select a.* ,lf.""Name"" as PropertyName,uf.""Name"" as BuildingTypeName,of.""Name"" as OccupancyName
,of.""Factor"" as OccupancyRate,usf.""Rate"" as BuildingRate
from cms.""F_JSC_PROP_MGMNT_SELF_ASSESSMENT_FORM"" as a
left join public.""LOV"" as lf on lf.""Id""=a.""PropertyType"" 
left join public.""LOV"" as uf on uf.""Id""=a.""BuildingType"" 
left join cms.""F_JSC_PROP_MGMNT_OccupancyFactor"" as of on of.""Id""=a.""OccupancyId"" 
left join cms.""F_JSC_PROP_MGMNT_UsageFactorCommercialProperty"" as usf on usf.""CategoryId"" = a.""BuildingCategory""

                        where a.""Id"" = '{assessmentId}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCPropertySelfAssessmentViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCPropertyFloorViewModel>> GetAssessmentFloorData(string assessmentId)
        {
            var query = $@"select f.*,lf.""Code"" as FloorType,lf.""Name"" as Description
            from cms.""F_JSC_PROP_MGMNT_PROPERTY_FLOOR_DETAIL"" as f
            left join  public.""LOV"" as lf on lf.""Id""=f.""FloorId""
            where f.""ParentId"" = '{assessmentId}' ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCPropertyFloorViewModel>(query, null);
            return querydata;
        }

        public async Task<JSCParcelViewModel> IsParcelIdValid(string parcelId)
        {
            var query = $@"select p.*  
                            from public.""parcel"" as p
                            where p.""mmi_id""='{parcelId}' limit 1 ";
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCParcelViewModel>(query, null);
            return querydata;
        }
        public async Task<List<TreeViewViewModel>> GetJSCMapViewTreeList()
        {
            var query = $@"select 'WARD' as ParentId,w.""Id"" as id,w.""Name"" as Name,w.""Latitude"" as Latitude,w.""Longitude"" as Longitude,w.""WardArea"" as MapArea
                            from cms.""F_JSC_REV_WARD"" as w 
                            where w.""IsDeleted""=false
                            union
                            select 'LANDMARK' as ParentId,lm.""Id"" as id,lm.""LandmarkDescription"" as Name,lm.""Latitude"" as Latitude,lm.""Longitude"" as Longitude,'' as MapArea
                            from cms.""F_JSC_REV_LandMark"" as lm 
                            where lm.""IsDeleted""=false
                            union
                            select 'LOCALITY' as ParentId,lc.""Id"" as id,lc.""LocalityName"" as Name,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,lc.""LocalityArea"" as MapArea
                            from cms.""F_JSC_REV_JSC_LOCALITY"" as lc 
                            where lc.""IsDeleted""=false
                            ";
            var querydata = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCGrievanceWorkflow>> GetGrievanceWorkflowList()
        {
            var query = $@"select w.*,d.""Name"" as DepartmentName,lv.""Name"" as WorkflowLevel--,wd.""wrd_name"" as WardName
                            from cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as w 
                            join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id""=w.""DepartmentId""
                            --join public.""ward"" as wd on wd.""wrd_no""=w.""WardId""
                            join public.""LOV"" as lv on lv.""Id""=w.""WorkflowLevelId""
                            where w.""IsDeleted""=false
                            
                            ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCGrievanceWorkflow>(query, null);
            return querydata;
        }
        public async Task<IdNameViewModel> GetDepartmentById(string id)
        {
            var query = $@"select w.*
                            from cms.""F_JSC_GRIEVANCE_MGMT_Department"" as w                             
                            where w.""IsDeleted""=false and w.""Id""='{id}'
                            
                            ";
            var querydata = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<TreeViewViewModel>> GetAssetMapViewTreeList()
        {
            var query = $@"select 'WARD' as ParentId,w.""Id"" as id,w.""Name"" as Name,w.""Latitude"" as Latitude,w.""Longitude"" as Longitude,w.""WardArea"" as MapArea
                            from cms.""F_JSC_REV_WARD"" as w 
                            where w.""IsDeleted""=false
                            union
                            select lm.""WardId"" as ParentId,lm.""Id"" as id,lm.""AssetName"" as Name,lm.""Latitude"" as Latitude,lm.""Longitude"" as Longitude,'' as MapArea
                            from cms.""N_JSC_REV_Asset"" as lm 
                            where lm.""IsDeleted""=false
                            ";
            var querydata = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<JSCAssetConsumerViewModel>> GetAssetCountByWard(string wardId = null, string collectorId = null, string revType = null)
        {
            var query = $@"select count(a.""Id"") as AssetCount,w.""Name"" as WardName
                            from cms.""N_JSC_REV_Asset"" as a
                            join cms.""F_JSC_REV_WARD"" as w on a.""WardId""=w.""Id"" and w.""IsDeleted""=false
                            <<collectorQuery>>
                            where a.""IsDeleted""=false #WARD# -- #COLLECTOR# #REVTYPE#
                            group by a.""WardId"",w.""Name""  
                         ";

            var wardwhere = wardId.IsNotNull() ? $@" and a.""WardId""='{wardId}' " : "";
            query = query.Replace("#WARD#", wardwhere);

            //var collectorwhere = wardId.IsNotNull() ? $@" and a.""CollectorId""='{collectorId}' " : "";
            //query = query.Replace("#COLLECTOR#", collectorwhere);

            //var revtypewhere = wardId.IsNotNull() ? $@" and a.""RevenueTypeId""='{revType}' " : "";
            //query = query.Replace("#REVTYPE#", revtypewhere);

            var collectorQuery = "";
            if (collectorId.IsNotNullAndNotEmpty())
            {
                collectorQuery = $@" join cms.""F_JSC_REV_WardDetails"" as wd on wd.""WardId"" = a.""WardId"" and wd.""IsDeleted"" = false
                                        join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = wd.""ParentId"" and c.""Id"" = '{collectorId}' and c.""IsDeleted"" = false 
                                  ";
            }
            query = query.Replace("<<collectorQuery>>", collectorQuery);

            var queryData = await _queryRepo.ExecuteQueryList<JSCAssetConsumerViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<JSCAssetConsumerViewModel>> GetAssetAllotmentCountByWard(string wardId = null, string collectorId = null, string revType = null)
        {
            var query = $@"select count(aa.""Id"") as AssetCount,w.""Name"" as WardName, at.""Id"" as AssetTypeId, at.""Name"" as AssetTypeName
                            from cms.""N_JSC_REV_Asset"" as a
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted"" = false
                            join cms.""F_JSC_REV_WARD"" as w on a.""WardId""=w.""Id"" and w.""IsDeleted""=false
                            join cms.""N_JSC_REV_AssetAllotment"" as aa on a.""Id""=aa.""AssetId"" and aa.""IsDeleted""=false
                            <<collectorQuery>>
                            where a.""IsDeleted""=false #WARD# -- #COLLECTOR# #REVTYPE#
                            group by a.""WardId"",w.""Name"", at.""Id"", at.""Name"" 
                        ";

            var wardwhere = wardId.IsNotNull() ? $@" and a.""WardId""='{wardId}' " : "";
            query = query.Replace("#WARD#", wardwhere);

            //var collectorwhere = wardId.IsNotNull() ? $@" and a.""CollectorId""='{collectorId}' " : "";
            //query = query.Replace("#COLLECTOR#", collectorwhere);

            //var revtypewhere = wardId.IsNotNull() ? $@" and a.""RevenueTypeId""='{revType}' " : "";
            //query = query.Replace("#REVTYPE#", revtypewhere);

            var collectorQuery = "";
            if (collectorId.IsNotNullAndNotEmpty())
            {
                collectorQuery = $@" join cms.""F_JSC_REV_WardDetails"" as wd on wd.""WardId"" = a.""WardId"" and wd.""IsDeleted"" = false
                                        join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = wd.""ParentId"" and c.""Id"" = '{collectorId}' and c.""IsDeleted"" = false 
                                  ";
            }
            query = query.Replace("<<collectorQuery>>", collectorQuery);

            var queryData = await _queryRepo.ExecuteQueryList<JSCAssetConsumerViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByWard(string wardId = null, string collectorId = null)
        {
            var query = $@" select SUM(pay.""Amount""::int) as Amount, w.""Name"" as WardName, w.""Id""
                            from cms.""N_JSC_REV_AssetPayment"" as pay 
                            join public.""LOV"" as l on l.""LOVType"" = 'PAYMENT_STATUS' and pay.""PaymentStatusId"" = l.""Id"" and l.""IsDeleted"" = false and l.""Code"" = 'SUCCESS'
                            join cms.""N_JSC_REV_Asset"" as a on a.""Id"" = pay.""AssetId"" and a.""IsDeleted"" = false
                            join cms.""F_JSC_REV_WARD"" as w on w.""Id"" = a.""WardId"" and w.""IsDeleted"" = false
                            <<collectorQuery>>
                            where pay.""IsDeleted"" = false #WARD# 
                            group by w.""Id"",w.""Name"" 
                        ";

            var wardwhere = wardId.IsNotNull() ? $@" and a.""WardId""='{wardId}' " : "";
            query = query.Replace("#WARD#", wardwhere);

            var collectorQuery = "";
            if (collectorId.IsNotNullAndNotEmpty())
            {
                collectorQuery = $@" join cms.""F_JSC_REV_WardDetails"" as wd on wd.""WardId"" = a.""WardId"" and wd.""IsDeleted"" = false
                                        join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = wd.""ParentId"" and c.""Id"" = '{collectorId}' and c.""IsDeleted"" = false 
                                  ";
            }
            query = query.Replace("<<collectorQuery>>", collectorQuery);

            var queryData = await _queryRepo.ExecuteQueryList<JMCAssetPaymentViewModel>(query, null);
            return queryData;

        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByAssetType(string wardId = null, string collectorId = null)
        {
            var query = $@" select SUM(pay.""Amount""::int) as Amount, aa.""Name"" as AssetTypeName, aa.""Id""
                            from cms.""N_JSC_REV_AssetPayment"" as pay 
                            join public.""LOV"" as l on l.""LOVType"" = 'PAYMENT_STATUS' and pay.""PaymentStatusId"" = l.""Id"" and l.""IsDeleted"" = false and l.""Code"" = 'SUCCESS'
                            join cms.""N_JSC_REV_Asset"" as a on a.""Id"" = pay.""AssetId"" and a.""IsDeleted"" = false
                            <<collectorQuery>>
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as aa on aa.""Id"" = a.""AssetTypeId"" and aa.""IsDeleted"" = false
                            where pay.""IsDeleted"" = false #WARD# 
                            group by aa.""Id"",aa.""Name"" 
                        ";


            var wardwhere = wardId.IsNotNull() ? $@" and a.""WardId""='{wardId}' " : "";
            query = query.Replace("#WARD#", wardwhere);

            var collectorQuery = "";
            if (collectorId.IsNotNullAndNotEmpty())
            {
                collectorQuery = $@" join cms.""F_JSC_REV_WardDetails"" as wd on wd.""WardId"" = a.""WardId"" and wd.""IsDeleted"" = false
                                        join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = wd.""ParentId"" and c.""Id"" = '{collectorId}' and c.""IsDeleted"" = false 
                                  ";
            }
            query = query.Replace("<<collectorQuery>>", collectorQuery);

            var queryData = await _queryRepo.ExecuteQueryList<JMCAssetPaymentViewModel>(query, null);
            return queryData;

        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByCollector(string wardId = null, string collectorId = null)
        {
            var query = $@" select SUM(pay.""Amount""::int) as Amount, c.""Name"" as CollectorName, c.""Id""
                            from cms.""N_JSC_REV_AssetPayment"" as pay 
                            join public.""LOV"" as l on l.""LOVType"" = 'PAYMENT_STATUS' and pay.""PaymentStatusId"" = l.""Id"" and l.""IsDeleted"" = false and l.""Code"" = 'SUCCESS'
                            join cms.""N_JSC_REV_Asset"" as a on a.""Id"" = pay.""AssetId"" and a.""IsDeleted"" = false
                            join cms.""F_JSC_REV_WardDetails"" as aa on aa.""WardId"" = a.""WardId"" and aa.""IsDeleted"" = false
							join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = aa.""ParentId"" and c.""IsDeleted"" = false
                            where pay.""IsDeleted"" = false #WARD# #COLLECTOR#
                            group by c.""Id"",c.""Name""
                        ";

            var wardwhere = wardId.IsNotNull() ? $@" and a.""WardId""='{wardId}' " : "";
            query = query.Replace("#WARD#", wardwhere);

            var collectorwhere = collectorId.IsNotNull() ? $@" and c.""Id""='{collectorId}' " : "";
            query = query.Replace("#COLLECTOR#", collectorwhere);

            var queryData = await _queryRepo.ExecuteQueryList<JMCAssetPaymentViewModel>(query, null);
            return queryData;

        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByPaymentStatus(string wardId = null, string collectorId = null)
        {
            var query = $@" select Count(pay.""Id"") as PaymentCount, l.""Name"" as PaymentStatusName, l.""Id""
                            from cms.""N_JSC_REV_AssetPayment"" as pay 
                            join cms.""N_JSC_REV_Asset"" as a on a.""Id"" = pay.""AssetId"" and a.""IsDeleted"" = false
                            <<collectorQuery>>
                            join public.""LOV"" as l on l.""LOVType"" = 'PAYMENT_STATUS' and pay.""PaymentStatusId"" = l.""Id"" and l.""IsDeleted"" = false
                            where pay.""IsDeleted"" = false #WARD# 
                            group by l.""Id"",l.""Name"" 
                        ";

            var wardwhere = wardId.IsNotNull() ? $@" and a.""WardId""='{wardId}' " : "";
            query = query.Replace("#WARD#", wardwhere);

            var collectorQuery = "";
            if (collectorId.IsNotNullAndNotEmpty())
            {
                collectorQuery = $@" join cms.""F_JSC_REV_WardDetails"" as wd on wd.""WardId"" = a.""WardId"" and wd.""IsDeleted"" = false
                                        join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = wd.""ParentId"" and c.""Id"" = '{collectorId}' and c.""IsDeleted"" = false 
                                  ";
            }
            query = query.Replace("<<collectorQuery>>", collectorQuery);

            var queryData = await _queryRepo.ExecuteQueryList<JMCAssetPaymentViewModel>(query, null);
            return queryData;

        }


        public async Task<IList<JSCAssestBillPaymentViewModel>> GetAssetAllotmentDetails(DateTime billDate)
        {
            var query = $@"select distinct aa.*, af.""FeeAmount"" as FeeAmount, af.""FeeStartDate"" as FeeStartDate, af.""FeeEndDate"" as FeeEndDate,
                            l.""Code"" as PaymentTypeCode
                            from cms.""N_JSC_REV_AssetAllotment"" as aa
                            join cms.""F_JSC_REV_JSC_FEE_TYPE"" as ft on ft.""Id"" = aa.""FeeTypeId""
                            join cms.""F_JSC_REV_JSC_ASSET_FEE"" as af on af.""AssetId"" = aa.""AssetId""
                            join public.""LOV"" as l on l.""Id"" = ft.""PaymentType""
                            Where aa.""IsDeleted"" = 'false'and af.""IsDeleted"" = 'false' and ft.""IsDeleted"" = 'false'
                            and l.""IsDeleted"" = 'false'
                            and aa.""AllotmentFromDate""::date<='{billDate.ToDatabaseDateFormat()}'
                            and aa.""AllotmentToDate""::date>='{billDate.ToDatabaseDateFormat()}'
                            and af.""FeeStartDate""::date<='{billDate.ToDatabaseDateFormat()}'
                            and af.""FeeEndDate""::date>='{billDate.ToDatabaseDateFormat()}'
                            and (aa.""BillGenerationDay"")::int={billDate.Day}";
            var querydata = await _queryRepo.ExecuteQueryList<JSCAssestBillPaymentViewModel>(query, null);
            return querydata;
        }

        public async Task<int> CheckIfPaymentDataAlreadyProcessed(string assetId, string consumerId, DateTime billDate)
        {
            var query = $@"select distinct Count(ap.""Id"") from cms.""N_JSC_REV_AssetPayment"" as ap
                            where ap.""IsDeleted""=false and ap.""ConsumerId"" = '{consumerId}'
                            and ap.""AssetId"" = '{assetId}'
                            and ap.""BillDate""::date = '{billDate.ToDatabaseDateFormat()}'::date";
            var querydata = await _queryRepo.ExecuteScalar<int>(query, null);
            return querydata;
        }
        public async Task<IdNameViewModel> GetAssetTypeForJammuById(string assetTypeId)
        {
            var query = $@" select ""Name"",""Id"" 
                            from cms.""F_JSC_REV_JSC_ASSET_TYPE"" 
                            where ""IsDeleted"" = false and ""Id""='{assetTypeId}' ";
            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<IdNameViewModel> GetWardForJammuById(string wardId)
        {
            var query = $@" select ""Name"",""Id"" 
                            from cms.""F_JSC_REV_WARD"" 
                            where ""IsDeleted"" = false and ""Id""='{wardId}' ";
            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<JSCAssetViewModel> GetJSCAssetDetailsById(string assetId)
        {
            var query = $@" select a.*,at.""Name"" as AssetTypeName,w.""Name"" as WardName 
                            from cms.""N_JSC_REV_Asset"" as a
                            left join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_WARD"" as w on w.""Id"" = a.""WardId"" and w.""IsDeleted"" = false
                            where a.""IsDeleted"" = false and a.""Id""='{assetId}' ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JSCAssetViewModel>(query, null);
            return queryData;
        }
        public async Task<JSCBillPaymentReportViewModel> GetJSCBillPaymentDetails(string serviceId)
        {
            //     var query = $@" Select s.""Id"" as ""ServiceId"", s.""ServiceNo"",o.""Name"" as OwnerName
            //                     ,t.""DueDate"", b.""Amount"" as ""BillAmount"",ps.""Name"" as ""PaymentStatus""
            //,b.""PaymentReferenceNo"" as ""ReferenceNo"",ast.""AssetName""
            //                     From public.""NtsTask"" as t
            //                     Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
            //                     Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
            //                     Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
            //                     Join cms.""N_JSC_REV_AssetPayment"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
            //                     left join cms.""N_JSC_REV_Asset"" as ast on ast.""Id""=b.""AssetId"" and ast.""IsDeleted""=false
            //                     left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
            //                     where t.""IsDeleted""=false and s.""Id""='{serviceId}' ";

            var query = $@" Select s.""Id"" as ""ServiceId"", s.""ServiceNo"",o.""Name"" as OwnerName
                                 ,t.""DueDate"", b.""Amount"" as ""BillAmount"",ps.""Name"" as ""PaymentStatus""
                                ,b.""ReferenceNo"" as ""ReferenceNo"" --,ast.""AssetName""
                                 From public.""NtsTask"" as t
                                 Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                                 Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                                 Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                                 Join cms.""N_JAMMU_SMART_CITY_JSCPayment"" as b on b.""TaskId"" = t.""Id"" and b.""IsDeleted""=false
                                 --left join cms.""N_JSC_REV_Asset"" as ast on ast.""Id""=b.""AssetId"" and ast.""IsDeleted""=false
                                 left Join public.""LOV"" as ps on b.""PaymentStatus""=ps.""Id"" and ps.""IsDeleted""=false
                                 where t.""IsDeleted""=false and s.""Id""='{serviceId}' ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JSCBillPaymentReportViewModel>(query, null);
            return queryData;
        }
        public async Task<string> GetJSCConsumerUserId(string consumerId)
        {
            var query = $@"select distinct u.""Id"" from  cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as ac 
                            join public.""User"" as u on u.""Id"" = ac.""UserId""
                            where ac.""IsDeleted""=false and u.""IsDeleted""=false and ac.""Id"" = '{consumerId}'";
            var querydata = await _queryRepo.ExecuteScalar<string>(query, null);
            return querydata;
        }

        public async Task<List<JSCColonyViewModel>> GetJSCColonyMapViewList()
        {
            var query = $@"
                                        select
                                            'Feature' as ""type"",
                                            ST_AsGeoJSON(ST_Transform(""geom"", 4326), 6) :: text as ""geometry"",
                                            ""col_id"",
                                                        ""col_name"",
                                                        ""col_area"",
                                            'COLONY' as ParentId
                                        from ""public"".""colony"" --where ""col_id"" = '1'";
            var querydata = await _queryRepo.ExecuteQueryList<JSCColonyViewModel>(query, null);
            return querydata;
        }


        public async Task<JSCAssetConsumerViewModel> GetConsumerByConsumerNo(string consumerNo)
        {
            var query = $@" select * from cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" 
                            where ""ConsumerNo""='{consumerNo}' and ""IsDeleted""=false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JSCAssetConsumerViewModel>(query, null);
            return queryData;
        }

        public async Task<JSCAssetConsumerViewModel> GetConsumerById(string id)
        {
            var query = $@" select * from cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" 
                            where ""Id""='{id}' and ""IsDeleted""=false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JSCAssetConsumerViewModel>(query, null);
            return queryData;
        }

        public async Task<JMCAssetViewModel> GetAssetByServiceNo(string serviceNo)
        {
            var query = $@" select a.*,s.""ServiceNo"" 
                            from cms.""N_JSC_REV_Asset"" as a
                            join public.""NtsService"" as s on s.""UdfNoteTableId"" = a.""Id"" and s.""IsDeleted""=false
                            where s.""ServiceNo""='{serviceNo}' and a.""IsDeleted""=false 
                        ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JMCAssetViewModel>(query, null);
            return queryData;
        }

        public async Task<JMCAssetViewModel> GetAssetById(string id)
        {
            var query = $@" select a.*,s.""ServiceNo"",at.""Name"" as AssetTypeName,w.""Name"" as WardName
                            from cms.""N_JSC_REV_Asset"" as a
                            join cms.""F_JSC_REV_WARD"" as w on w.""Id"" = a.""WardId"" and w.""IsDeleted""=false
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted""=false
                            join public.""NtsService"" as s on s.""UdfNoteTableId"" = a.""Id"" and s.""IsDeleted""=false
                            where a.""Id""='{id}' and a.""IsDeleted""=false 
                        ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JMCAssetViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JSCAssetConsumerViewModel>> GetAssetConsumerData(string assetId)
        {
            var query = $@"  select c.""Id"" as ConsumerId,a.""AssetName"",c.""ConsumerNo"",c.""Email"",c.""Name"" as ConsumerName,c.""Mobile"",c.""Address""
                            , aa.""AllotmentToDate"", aa.""AllotmentFromDate"", aa.""AllotmentDate"",at.""Name"" as AssetTypeName
                            from cms.""N_JSC_REV_Asset"" as a
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted""=false
                            join cms.""N_JSC_REV_AssetAllotment"" as aa on aa.""AssetId"" = a.""Id"" and aa.""IsDeleted""=false
                            join cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as c on c.""Id"" = aa.""ConsumerId"" and c.""IsDeleted""=false
                            where a.""Id"" = '{assetId}' and a.""IsDeleted""=false
                        ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCAssetConsumerViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JSCAssetConsumerViewModel>> GetAssetPaymentData(string assetId)
        {
            var query = $@"  select a.""AssetName"",c.""ConsumerNo"" as ConsumerNo,c.""Name"" as ConsumerName,at.""Name"" as AssetTypeName
                            ,pay.""Amount"",pay.""BillDate"", pay.""DueDate""
                            ,l.""Name"" as PaymentStatus, pay.""PaymentReferenceNo"",pay.""PaymentModeId""
                            --, lov.""Name"" as PaymentMode
                            from cms.""N_JSC_REV_Asset"" as a
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted""=false
                            join cms.""N_JSC_REV_AssetPayment"" as pay on pay.""AssetId"" = a.""Id"" and pay.""IsDeleted""=false
                             join cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as c on c.""Id"" = pay.""ConsumerId"" and c.""IsDeleted""=false
                            join public.""LOV"" as l on l.""Id"" = pay.""PaymentStatusId"" and l.""LOVType"" = 'PAYMENT_STATUS' and l.""IsDeleted""=false
                            -- join public.""LOV"" as lov on lov.""Id"" = pay.""PaymentModeId""  and lov.""LOVType"" = 'JSC_REV_PAYMENT_MODE' and lov.""IsDeleted""=false
                             where a.""Id"" = '{assetId}' and a.""IsDeleted""=false
                        ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCAssetConsumerViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JSCAssetConsumerViewModel>> GetConsumerPaymentData(string consumerId)
        {
            var query = $@" select a.""AssetName"",at.""Name"" as AssetTypeName,pay.""Amount"",pay.""BillDate"",pay.""DueDate"",pay.""PaymentReferenceNo""
                            ,l.""Name"" as PaymentStatus
                            from cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as c
                            join cms.""N_JSC_REV_AssetPayment"" as pay on pay.""ConsumerId"" = c.""Id"" and pay.""IsDeleted""=false
                            join cms.""N_JSC_REV_Asset"" as a on a.""Id"" = pay.""AssetId"" and a.""IsDeleted""=false
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted""=false
                            join public.""LOV"" as l on l.""Id"" = pay.""PaymentStatusId"" and l.""LOVType"" = 'PAYMENT_STATUS' and l.""IsDeleted""=false
                            -- join public.""LOV"" as lov on lov.""Id"" = pay.""PaymentModeId""  and lov.""LOVType"" = 'JSC_REV_PAYMENT_MODE' and lov.""IsDeleted""=false
                            where c.""IsDeleted""=false and c.""Id"" = '{consumerId}'
                        ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCAssetConsumerViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JMCAssetViewModel>> GetConsumerAssetData(string consumerId)
        {
            var query = $@" select a.""Id"" as AssetId,a.""AssetName"" ,a.""AssetDescription"",w.""Name"" as WardName,a.""specificLocation"",a.""Latitude"",a.""Longitude"",
                            at.""Name"" as AssetTypeName,aa.""AllotmentDate"", aa.""AllotmentFromDate"", aa.""AllotmentToDate""
                            from cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as c 
                            join cms.""N_JSC_REV_AssetAllotment"" as aa on aa.""ConsumerId"" = c.""Id"" and aa.""IsDeleted""=false
                            join cms.""N_JSC_REV_Asset"" as a on a.""Id"" = aa.""AssetId"" and a.""IsDeleted""=false
                            join cms.""F_JSC_REV_WARD"" as w on w.""Id"" = a.""WardId"" and w.""IsDeleted""=false
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted""=false
                            where c.""IsDeleted""=false and c.""Id"" = '{consumerId}'
                        ";
            var queryData = await _queryRepo.ExecuteQueryList<JMCAssetViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JSCParcelViewModel>> GetJSCParcelMapViewList(string colName, string colText)
        {
            var query = $@"SELECT 
                            'Feature' as ""type"",
                            ST_AsGeoJSON(ST_Transform(""geom"", 4326), 6) :: text as ""geometry"",
                            ""prop_id"", ""res_stat"", ""road_desc"", ""road_type"", ""own_dtls"", ""own_name"", ""tel_no"",
                            ""aadhar"",
                            'COLONY' as ParentId
                            FROM public.""parcel"" #where# ";
            if (colName.IsNotNullAndNotEmpty())
            {
                query = query.Replace("#where#", $@" where ""{colName}"" = '{colText}'");
            }
            else
            {
                query = query.Replace("#where#", "");
            }

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetParcelColumnList()
        {

            var query = $@"SELECT p.""ColName"" as Id, p.""ColText"" as Name
                            FROM cms.""F_EGOVERNANCE_ParcelColumns"" as p";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetColonyList()
        {

            var query = $@"SELECT p.""col_name"" as Name, p.""col_id"" as Id
                            FROM public.""colony"" as p";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetParcelTypeList()
        {
            var query = $@"SELECT distinct  p.""usg_cat_gf""
                             as Name, p.""usg_cat_gf"" as Id
                            FROM public.""parcel""  as p ";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetWardList()
        {
            var query = $@"SELECT distinct concat(p.""wrd_no"", '-', p.""wrd_name"")
                             as Name, p.""wrd_no"" as Id
                            FROM public.""ward""  as p
                            order by p.""wrd_no"" ";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetJSCZoneList()
        {
            var query = $@"SELECT z.""Id"" as Id, z.""Name"" as Name
                            FROM cms.""F_JSC_GRIEVANCE_MGMT_Zone"" as z
                            WHERE z.""IsDeleted""=false
                            order by z.""Name"" ";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetJSCZoneListByDepartment(string departmentId)
        {
            var query = $@"SELECT z.""Id"" as Id, z.""Name"" as Name
                            FROM cms.""F_JSC_GRIEVANCE_MGMT_Zone"" as z
                            join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id""=z.""DepartmentId"" and d.""IsDeleted""=false
                            WHERE z.""IsDeleted""=false and d.""Id""='{departmentId}'
                            order by z.""Name"" ";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetParcelIdNameList()
        {
            var query = $@"select ""mmi_id"" as Id, Concat(""prop_id"",'_', ""mmi_id"", '_', ""ward_no"", '_', ""own_name"") as Name
                            from public.""parcel"" ";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetBinCollectorNameList()
        {
            var query = $@"select ""UserId"" as Id, ""Name"" as Name
                            from cms.""F_JSC_REV_JSC_COLLECTOR"" where ""UserId"" is not null and ""IsDeleted""=False";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<JSCCollectorViewModel> GetBinCollectorMobile(string userId)
        {
            var query = $@"select c.""PhoneNumber"", auto.""AutoNumber""
                            from cms.""F_JSC_REV_JSC_COLLECTOR"" as c
                            join cms.""F_JSC_REV_AllocationWardToCollector"" as a on a.""Collector"" = c.""Id"" and a.""IsDeleted"" = false
                            join cms.""F_JSC_PROP_MGMNT_AutoMaster"" as auto on auto.""Id"" = a.""Auto"" and auto.""IsDeleted"" = false
                            where c.""IsDeleted""=false and c.""UserId""='{userId}' and c.""UserId"" is not null";

            var querydata = await _queryRepo.ExecuteQuerySingle<JSCCollectorViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetJSCOwnerList()
        {
            var query = $@"SELECT distinct  p.""own_name""
                             as Name, p.""own_name"" as Id
                            FROM public.""parcel""  as p ";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCParcelViewModel>> GetParcelSearchByWardandType(string ward, string parcelType)
        {
            var query = $@"SELECT 
                            'Feature' as ""type"",
                            ST_AsGeoJSON(ST_Transform(""geom"", 4326), 6) :: text as ""geometry"",
                            ""prop_id"", ""res_stat"", ""road_desc"", ""road_type"", ""own_dtls"", ""own_name"", ""tel_no"",
                            ""aadhar"",
                            'COLONY' as ParentId
                            FROM public.""parcel"" where  ""usg_cat_gf"" = '{parcelType}' and ""ward_no"" = '{ward}'";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetParcelSearchByWard(string ward)
        {
            var query = $@"SELECT ""mmi_id"" as Id, ""mmi_id"" as Name
                            FROM public.""parcel"" where   ""ward_no"" = '{ward}'";


            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }


        public async Task<List<IdNameViewModel>> GetViolationData()
        {
            var query = $@"select cons.* 
                            from cms.""F_JSC_ENFORCEMENT_Violations"" as cons";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetAuthorizationList()
        {
            //var query = $@"select cons.* 
            //                from cms.""N_DMS_JMC_OBPS"" as cons";
            var query = $@"select cons.""Ward"" as WardNo,cons.""AuthorizationId"" as AuthorizationId,
                        cons.""OrderNo"" as OrderNo,cons.""Name"" as Name,cons.""Mobile"" as MobileNo,
	                    cons.""DateOfApproval"" as ApprovalDate,cons.""ApprovalDetails"" as ApprovalDetails,
						cons.""fileAttachment"" as Attachments
                            from cms.""N_DMS_JMC_OBPS"" as cons";
            var querydata = await _queryRepo.ExecuteQueryList<JSCEnforcementUnAuthorizationViewModel>(query, null);
            return querydata;
        }


        public async Task<List<JSCParcelViewModel>> GetJSCWardMapViewList(string wardNo)
        {
            var query = $@"SELECT 
                            'Feature' as ""type"",
                            ST_AsGeoJSON(ST_Transform(""geom"", 4326), 6) :: text as ""geometry"",
                            ""prop_id"", ""res_stat"", ""road_desc"", ""road_type"", ""own_dtls"", ""own_name"", ""tel_no"",
                            ""aadhar"",
                            'COLONY' as ParentId
                            FROM public.""parcel"" where ""ward_no""='{wardNo}' ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCParcelViewModel>> GenerateRevenueCollectionBillForJammu()
        {
            var query = $@" select p.*,u.""Id"" as ""UserId"",f.""Code"" as FeeType,r.""Code"" as RevenueType,ar.""Amount"" as Amount,ar.""RevenueTypeId"" as RevenueTypeId from public.""parcel"" as p 
            join public.""User"" as u on(u.""Mobile""=p.""tel_no"" or u.""NationalId""=p.""aadhar"")
			join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Code""=p.""usg_cat_gf""
			join cms.""F_JSC_REV_ASSET_TYPE_REVENUE_TYPE"" as ar on ar.""AssetTypeId""=at.""Id""
			join cms.""F_JSC_REV_REVENUE_TYPE"" as r on r.""Id""=ar.""RevenueTypeId""
			join cms.""F_JSC_REV_JSC_FEE_TYPE"" as f on f.""Id""=ar.""FeeTypeId""
            where p.""ward_no""='62'";
            var queryData = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return queryData;

        }

        public async Task<List<JSCParcelViewModel>> GetJSCParcelListByUser(string userId)
        {
            var query = $@" SELECT 
                                'Feature' as ""type"",p.""gid"",
                                p.""prop_id""
                                ,p.""pcl_id""
                                , p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                                p.""aadhar"",
                                w.""wrd_name"",p.""ward_no"",p.""usg_cat_gf"",ast.""Id"" as AssetTypeId,
                                ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry""
                                FROM public.""User"" as u 
                                JOIN public.""parcel"" as p on p.""tel_no""=u.""Mobile""
                                JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no""
                                left join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as ast on ast.""Code""=p.""usg_cat_gf"" and ast.""IsDeleted""=false
                                where u.""Id"" = '{userId}' and u.""IsDeleted""=false
                            ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCParcelViewModel>> GetJSCAssetParcelListByUser(string userId)
        {
            var query = $@" SELECT 
                                'Feature' as ""type"",p.""gid"",
                                ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                                p.""prop_id""
                                ,p.""pcl_id""
                                , p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                                p.""aadhar"",
                                w.""wrd_name"",p.""ward_no"",p.""usg_cat_gf""
                                FROM public.""User"" as u 
                                JOIN public.""parcel"" as p on p.""tel_no""=u.""Mobile""
                                JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                                where u.""Id"" = '{userId}' and u.""IsDeleted""=false
                            ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task<JSCParcelViewModel> GetParcelDataByPclId(string id)
        {
            var query = $@" SELECT 
                                'Feature' as ""type"",
                                ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                                p.""prop_id""
                                ,p.""pcl_id""
                                , p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                                p.""aadhar"",
                                w.""wrd_name"",
                                'COLONY' as ParentId,p.""usg_cat_gf""
                                FROM public.""parcel"" as p 
                                JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                                where P.""pcl_id"" = '{id}'
                        ";
            var result = await _queryRepo.ExecuteQuerySingle<JSCParcelViewModel>(query, null);
            return result;
        }

        public async Task<IList<JSCPaymentViewModel>> GetJSCPaymentsList(string portalIds = null, string userId = null)
        {
            userId = userId.IsNotNull() ? userId : _repo.UserContext.UserId;

            string query = $@"Select p.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"" as PaymentSubject,t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,
 ts.""Code"" as TaskStatusCode,t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"" as NoteOwnerUserId,p.""Id"" as UdfNoteTableId, 
 p.""Amount"",ps.""Name"" as ""PaymentStatus"",p.""NtsNoteId"" as NoteId,t.""Id"" as TaskId,p.""SourceReferenceId"",
 ps.""Code"" as ""PaymentStatusCode"",p.""ReferenceNo""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                            left Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
							join cms.""N_JAMMU_SMART_CITY_JSCPayment"" as p on t.""Id""=p.""TaskId"" and p.""IsDeleted""=false
                            left Join public.""LOV"" as ps on p.""PaymentStatus""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""IsDeleted""=false and t.""AssignedToUserId""='{userId}' #PORTALWHERE# ";

            var portalwhere = portalIds.IsNotNullAndNotEmpty() ? $@" and t.""PortalId"" in ('{portalIds}') " : "";

            query = query.Replace("#PORTALWHERE#", portalwhere);

            var result = await _queryRepo.ExecuteQueryList<JSCPaymentViewModel>(query, null);

            return result;
        }

        public async Task<IList<IdNameViewModel>> GetJSCLocalityList(string wardNo)
        {
            string query = $@"select distinct ""locality"" as Id, ""locality"" as Name  from public.""parcel"" where ""ward_no"" = '{wardNo}' and ""locality"" <> 'null'";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);

            return result;
        }

        public async Task<IList<IdNameViewModel>> GetJSCSubLocalityList(string wardNo, string loc)
        {
            string query = $@"select distinct  ""sub_loc"" as Id, ""sub_loc"" as Name from public.""parcel"" where ""locality"" = '{loc}' and ""ward_no"" = '{wardNo}' and ""sub_loc"" <> 'null'";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);

            return result;
        }
        public async Task<IList<IdNameViewModel>> GetJSCSubLocalityIdNameList()
        {
            string query = $@"select distinct  ""sub_loc"" as Id, ""sub_loc"" as Name from public.""parcel"" where ""sub_loc"" <> 'null'";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<List<JSCParcelViewModel>> GetGrievanceReportGISBasedData(DateTime fromDate, DateTime toDate)
        {
            var query = $@"select distinct s.""ServiceNo"",s.""Id"" as ServiceId, p.""mmi_id"",'Feature' as ""type"", 
                        ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                        count(s.""ServiceNo""), p.""prop_id"", p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                        p.""aadhar"",p.""locality"", p.""sub_loc""
                        from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc
                        join public.""NtsService"" as s on lc.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                        join public.""parcel"" as p on p.""mmi_id"" = lc.""DDN""
                        where lc.""IsDeleted"" = false
                        and (lc.""CreatedDate""::Date>='{fromDate}'::Date and lc.""CreatedDate""::Date<='{toDate}'::Date)
                        group By s.""ServiceNo"",s.""Id"",p.""mmi_id"",p.""geom"",p.""prop_id"", p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                        p.""aadhar"",p.""locality"", p.""sub_loc""
                        ";


            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            foreach (var item in querydata)
            {
                var query1 = $@"select Count(lc.""DDN"" ) 
                from public.""NtsService"" as s
                Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
                Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
                Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" = lc.""Id""  
                where s.""IsDeleted""=false and lc.""DDN""='{item.mmi_id}' 
                group by lc.""DDN"" ";

                var querydata1 = await _queryRepo.ExecuteScalar<long>(query1, null);
                if (querydata1 > 0)
                {
                    item.Count = querydata1;
                }
            }
            return querydata.OrderBy(x => x.gid).ToList();
        }

        public async Task<List<JSCParcelViewModel>> GetGrievanceReportWardHeatMapData(DateTime fromDate, DateTime toDate, string departmentId)
        {
            var query = $@" select  p.""wrd_no"" as ""ward_no"",'Feature' as ""type"", 
                        ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry""
                        from public.""ward"" as p
                        ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);

            foreach (var item in querydata)
            {
                var query1 = $@" SELECT l.""Ward"" as Ward,Count(l.*) as ComplaintCount,  string_agg(case when ls.""Code"" is null then 'GRV_PENDING' else ls.""Code"" end, ',') AS StatusList
                             from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                             --join public.""ward"" as w on w.""wrd_no"" = l.""Ward""
                            left join public.""LOV"" as ls on ls.""Id""=l.""GrvStatus"" and ls.""IsDeleted""=false
                             where l.""IsDeleted"" = false and l.""Ward""='{item.ward_no}' #WHEREDEPT# and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                            Group by  l.""Ward"" order by l.""Ward""
                            ";
                var wheredept = "";
                if (departmentId.IsNotNullAndNotEmpty())
                {
                    wheredept = $@" and l.""Department""='{departmentId}' ";
                }
                query1 = query1.Replace("#WHEREDEPT#", wheredept);
                var querydata1 = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query1, null);
                if (querydata1.Count>0)
                {
                    if (querydata1.FirstOrDefault().StatusList.IsNotNullAndNotEmpty())
                    {
                        var statusList = querydata1.FirstOrDefault().StatusList.Split(",").ToList();

                        var pendingCnt = statusList.Where(x => x == "GRV_PENDING").Count();
                        var inProgressCnt = statusList.Where(x => x == "GRV_IN_PROGRESS").Count();
                        var notPertainingCnt = statusList.Where(x => x == "GRV_NOT_PERTAINING").Count();
                        var disposedCnt = statusList.Where(x => x == "GRV_DISPOSED").Count();
                        
                        item.PendingCount = pendingCnt;
                        item.InProgressCount = inProgressCnt;
                        item.NotPertainingCount = notPertainingCnt;
                        item.DisposedCount = disposedCnt;
                    }
                    item.Count = querydata1.Select(x => x.ComplaintCount).FirstOrDefault();
                }
            }
            return querydata.OrderBy(x => x.gid).ToList();
        }

        public async Task<List<JSCParcelViewModel>> GetJSCParcelDataForGarbageCollection(string wardId = null, string locality = null, string ddn = null, string autoId = null, DateTime? date = null)
        {
            var todayDate = DateTime.Today.ToDatabaseDateFormat();
            if (date.IsNotNull())
            {
                todayDate = date.Value.ToDatabaseDateFormat();
            }
            var where1 = "";
            var where2 = "";
            var where3 = "";
            var where4 = "";
            var query = $@"select distinct p.""mmi_id"" as mmi_id,
                        'Feature' as ""type"",
                        ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                        p.""prop_id""
                        ,p.""pcl_id""
                        , p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                        p.""aadhar"",
                        w.""wrd_name"",
                        p.""ward_no"" ,
                        col.""GarbageTypeName"",       
                        'COLONY' as ParentId,p.""locality"", p.""sub_loc"",
                        case when col.""CollectedDate""=date('{todayDate}') then true else false end ""IsGarbageCollected""
                        FROM public.""parcel"" as p 
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = p.""mmi_id"" and gcp.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
                        #WHERE1# #WHERE3#
                        union
                        select  p.""mmi_id"" as mmi_id,
                        'Feature' as ""type"",
                        ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                        p.""prop_id""
                        ,p.""pcl_id""
                        , p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                        p.""aadhar"",
                        w.""wrd_name"",
                        p.""ward_no"" ,
                        col.""GarbageTypeName"",
                        'COLONY' as ParentId,p.""locality"", p.""sub_loc"",
                        case when col.""CollectedDate""=date('{todayDate}') then true else false end ""IsGarbageCollected""
                        from cms.""N_SNC_SANITATION_SERVICE_BIN_BOOKING_JSC"" b
                        join public.""NtsService"" s on b.""Id""=s.""UdfNoteTableId""	 and s.""IsDeleted""=false  
			            Join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false 
			            join public.""parcel"" p on p.""mmi_id""::text=b.""ParcelId""
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = p.""mmi_id"" and gcp.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
			            where b.""IsDeleted""=false 
                        #WHERE2# #WHERE4#";

            where1 = $@" WHERE p.""ward_no"" = '{wardId}' ";
            where2 = $@" and  p.""ward_no"" = '{wardId}' ";
            where3 = $@" WHERE p.""mmi_id"" = '{ddn}' ";
            where4 = $@" and  p.""mmi_id"" = '{ddn}' ";
            if (wardId.IsNotNullAndNotEmpty())
            {
                where3 = $@" and p.""mmi_id"" = '{ddn}' ";
                query = query.Replace("#WHERE1#", where1);
                query = query.Replace("#WHERE2#", where2);
            }
            else
            {
                query = query.Replace("#WHERE1#", "");
                query = query.Replace("#WHERE2#", "");
            }

            if (ddn.IsNotNullAndNotEmpty())
            {
                query = query.Replace("#WHERE3#", where3);
                query = query.Replace("#WHERE4#", where4);
            }
            else
            {
                query = query.Replace("#WHERE3#", "");
                query = query.Replace("#WHERE4#", "");
            }


            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            if (ddn.IsNotNullAndNotEmpty())
            {
                querydata = querydata.Where(x => x.mmi_id == ddn).ToList();
            }
            return querydata.OrderBy(x => x.gid).ToList();
        }

        public async Task<List<JSCParcelViewModel>> GetGISDataByAutoWise(string autoId = null, DateTime? date = null)
        {
            var todayDate = DateTime.Today.ToDatabaseDateFormat();
            if (date.IsNotNull())
            {
                todayDate = date.Value.ToDatabaseDateFormat();
            }
            var query = $@"select distinct aa.""AutoNumber"", gcp.""ParcelId"",
                 p.""mmi_id"" as mmi_id,
                 'Feature' as ""type"",
                 ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                 p.""prop_id""
                 ,p.""pcl_id""
                 , p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                 p.""aadhar"",
                 ac.""WardNo"",
                 'COLONY' as ParentId,p.""locality"", p.""sub_loc"",
                 case when col.""CollectedDate""=date('{todayDate}') then true else false end ""IsGarbageCollected""
                 from cms.""F_JSC_PROP_MGMNT_AutoMaster"" as aa    
                 join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Auto"" = aa.""Id"" and ac.""IsDeleted"" = false
                 join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = ac.""Collector"" and gcp.""IsDeleted"" = false
                 left join public.""parcel"" as p on p.""mmi_id"" = gcp.""ParcelId""
                  left join
                 (
                  select ""ParcelId"",date(max(""CollectionDateTime"")) ""CollectedDate"" from cms.""F_JSC_REV_GarbageCollection""
                     where ""IsDeleted"" = false group by ""ParcelId""
                 ) col on p.""mmi_id""::text = col.""ParcelId""
                 where aa.""IsDeleted"" = false  
                  #auto# ";

            var auto = $@"and aa.""Id"" = '{autoId}'";

            if (autoId.IsNotNullAndNotEmpty())
            {
                query = query.Replace("#auto#", auto);
            }
            else
            {
                query = query.Replace("#auto#", "");
            }
            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata.OrderBy(x => x.gid).ToList();
        }

        public async Task<List<JSCParcelViewModel>> GetJSCParcelDataForGarbageCollectionByWard(string wardId = null)
        {
            var query = $@"select  col.""CollectedByUserId"" as CollectorId, p.""mmi_id"" as mmi_id,
                        'Feature' as ""type"",
                        ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                        p.""prop_id""
                        ,p.""pcl_id""
                        , p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                        p.""aadhar"",
                        w.""wrd_name"",
                        col.""GarbageTypeName"",       
                        'COLONY' as ParentId,p.""locality"", p.""sub_loc"",
                        case when col.""CollectedDate""=date('{DateTime.Now.ToDatabaseDateFormat()}') then true else false end ""IsGarbageCollected""
                        FROM public.""parcel"" as p 
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = p.""mmi_id"" and gcp.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
                        WHERE p.""ward_no"" = '{wardId}' 
                        union
                        select col.""CollectedByUserId"" as CollectorId, p.""mmi_id"" as mmi_id,
                        'Feature' as ""type"",
                        ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                        p.""prop_id""
                        ,p.""pcl_id""
                        , p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                        p.""aadhar"",
                        w.""wrd_name"",
                        col.""GarbageTypeName"",       
                        'COLONY' as ParentId,p.""locality"", p.""sub_loc"",
                        case when col.""CollectedDate""=date('{DateTime.Now.ToDatabaseDateFormat()}') then true else false end ""IsGarbageCollected""
                        from cms.""N_SNC_SANITATION_SERVICE_BIN_BOOKING_JSC"" b
                        join public.""NtsService"" s on b.""Id""=s.""UdfNoteTableId""	 and s.""IsDeleted""=false  
			            Join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false 
			            join public.""parcel"" p on p.""mmi_id""::text=b.""ParcelId""
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""   
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = p.""mmi_id"" and gcp.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
			            where ss.""Code""='SERVICE_STATUS_COMPLETE' and b.""IsDeleted""=false 
                        and  p.""ward_no"" = '{wardId}'  ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata.OrderBy(x => x.gid).ToList();
        }
        public async Task<double> GetJSCBinFeeAmount(DateTime bookingFromDate, DateTime bookingToDate, string binTypeId, string binSizeId, long binNumber)
        {
            double result = 0;
            var days = (bookingToDate - bookingFromDate).TotalDays + 1;
            var query = $@"select f.""DailyAmount"" as amount 
                            from cms.""F_JSC_REV_BinFee"" as f
                            where f.""IsDeleted""=false and f.""BinTypeId""='{binTypeId}' and f.""BinSizeId""='{binSizeId}' ";
            var querydata = await _queryRepo.ExecuteScalar<double>(query, null);
            if (querydata.IsNotNull())
            {
                result = Convert.ToDouble(days) * Convert.ToDouble(querydata) * Convert.ToDouble(binNumber);
            }
            return result;
        }
        public async Task<OnlinePaymentViewModel> GetOnlinePaymentDetailsJSC(OnlinePaymentViewModel model)
        {
            var existquery = $@"Select * from cms.""F_GENERAL_OnlinePayment"" where ""UdfTableId""='{model.UdfTableId}' and ""NtsType""='{(int)model.NtsType}' and ""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQuerySingle<OnlinePaymentViewModel>(existquery, null);
            return result;
        }

        public async Task InsertOnlinePaymentDetailsDataJSC(OnlinePaymentViewModel model, string date)
        {
            var query = @$"Insert into cms.""F_GENERAL_OnlinePayment"" (""Id"",""CreatedDate"",""CreatedBy"",""LastUpdatedDate"",""LastUpdatedBy"",""CompanyId"",
                    ""SequenceOrder"",""VersionNo"",""IsDeleted"",""LegalEntityId"",""Status"",""NtsId"",""NtsType"",""UdfTableId"",""Amount"",""RequestUrl"",""EmailId"",""MobileNumber"",""Message"",""ChecksumValue"",""PaymentGatewayReturnUrl"",""ReturnUrl"",""PaymentIds"")
                    Values('{model.Id}','{date}','{model.UserId}','{date}','{model.UserId}','{_repo.UserContext.CompanyId}','1','1',false,'{_repo.UserContext.LegalEntityId}','{(int)StatusEnum.Active}',
                    '{model.NtsId}','{(int)model.NtsType}','{model.UdfTableId}','{model.Amount}','{model.RequestUrl}','{model.EmailId}','{model.MobileNumber}','{model.Message}','{model.ChecksumValue}','{model.PaymentGatewayReturnUrl}','{model.ReturnUrl}','{model.PaymentIds}' )";
            await _query.ExecuteCommand(query, null);

        }
        public async Task UpdateOnlinePaymentJSC(OnlinePaymentViewModel responseViewModel)
        {
            var query = $@"Update cms.""F_GENERAL_OnlinePayment"" set ""LastUpdatedDate""='{DateTime.Now}',
            ""LastUpdatedBy""='{_repo.UserContext.UserId}', ""NtsId""='{responseViewModel.NtsId}',""NtsType""='{responseViewModel.NtsType}',
            ""UdfTableId""='{responseViewModel.UdfTableId}',""Amount""='{responseViewModel.Amount}',""PaymentStatusId""='{responseViewModel.PaymentStatusId}',""ChecksumValue""='{responseViewModel.ChecksumValue}',
            ""RequestUrl""='{responseViewModel.RequestUrl}',""ResponseUrl""='{responseViewModel.ResponseUrl}',""ChecksumKey""='{responseViewModel.ChecksumKey}',""ReturnUrl""='{responseViewModel.ReturnUrl}',""Message""='{responseViewModel.Message}',
            ""PaymentGatewayURL""='{responseViewModel.PaymentGatewayUrl}',""EmailId""='{responseViewModel.EmailId}',""MobileNumber""='{responseViewModel.MobileNumber}',""PaymentReferenceNo""='{responseViewModel.PaymentReferenceNo}',
            ""PaymentGatewayReturnUrl""='{responseViewModel.PaymentGatewayReturnUrl}',""ResponseMessage""='{responseViewModel.ResponseMessage}',""ResponseChecksumValue""='{responseViewModel.ResponseChecksumValue}',""ResponseErrorCode""='{responseViewModel.ResponseErrorCode}',""ResponseError""='{responseViewModel.ResponseError}',
            ""AuthStatus""='{responseViewModel.AuthStatus}',""PaymentIds""='{responseViewModel.PaymentIds}'
            where ""Id""='{responseViewModel.Id}'";
            await _queryRepo1.ExecuteCommand(query, null);
        }

        public async Task<OnlinePaymentViewModel> GetOnlinePaymentJSC(string id)
        {
            var existquery = $@"Select * from cms.""F_GENERAL_OnlinePayment"" where ""Id""='{id}' and ""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQuerySingle<OnlinePaymentViewModel>(existquery, null);
            return result;
        }


        public async Task<JSCParcelViewModel> GetPropertyById(string propertyId)
        {
            var query = $@"SELECT 
                            'Feature' as ""type"",
                            ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",p.""prop_id"",
                            p.""ward_no"", p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                            p.""aadhar"", p.""sub_loc"", p.""locality"", p.""sector"", p.""post_off"", p.""pin_code"", p.""bu_type"",
                            p.""usg_cat_gf"", p.""building"", p.""pcl_id"", p.""gid"",
                            'COLONY' as ParentId
                            FROM public.""parcel"" as p
                            where p.""gid""='{propertyId}' 
                        ";

            var querydata = await _queryRepo.ExecuteQuerySingle<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCPaymentViewModel>> GetPaymentDetailsByPropertyId(string gid)
        {
            var query = $@" select p.*, l.""Name""  as PaymentStatusName, u.""Name"" as OwnerName, lov.""Name"" as PaymentModeName
                            from cms.""N_JAMMU_SMART_CITY_JSCPayment"" as p
                            join public.""LOV"" as l on l.""Id"" = p.""PaymentStatus"" and l.""IsDeleted""=false
                            join public.""User"" as u on u.""Id"" = p.""NoteOwnerUserId"" and p.""IsDeleted""=false
                            join public.""LOV"" as lov on lov.""Id"" = p.""PaymentMode"" and lov.""IsDeleted""=false
                            where p.""SourceReferenceId"" = '{gid}'
                        ";
            var res = await _queryRepo.ExecuteQueryList<JSCPaymentViewModel>(query, null);
            return res;
        }
        public async Task<List<JSCPaymentViewModel>> GetPaymentDetailsByServiceId(string serviceId)
        {
            var query = $@" select p.*, l.""Name""  as PaymentStatusName, u.""Name"" as OwnerName, lov.""Name"" as PaymentModeName
                            from cms.""N_JAMMU_SMART_CITY_JSCPayment"" as p
                            join public.""LOV"" as l on l.""Id"" = p.""PaymentStatus"" and l.""IsDeleted""=false
                            join public.""User"" as u on u.""Id"" = p.""NoteOwnerUserId"" and p.""IsDeleted""=false
                            join public.""LOV"" as lov on lov.""Id"" = p.""PaymentMode"" and lov.""IsDeleted""=false
                            where p.""IsDeleted""=false and p.""ServiceId"" = '{serviceId}' 
                        ";
            var res = await _queryRepo.ExecuteQueryList<JSCPaymentViewModel>(query, null);
            return res;
        }
        public async Task UpdatePropertyTax(string paymentStatus,string reference,string id)
        {
            var query = $@"Update cms.""F_JSC_PROP_MGMNT_PROPERTY_INSTALLMENT"" set ""PaymentDate""='{DateTime.Now}',
            ""PaymentStatusId""='{paymentStatus}', ""PaymentReferenceNo""='{reference}'
            where ""Id""='{id}'";
            await _queryRepo1.ExecuteCommand(query, null);
        }
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByPropertyId(string gid)
        {
            var query = $@" select g.*, u.""Name"" as CollectedByUserName
                            from cms.""F_JSC_REV_GarbageCollection"" as g
                            join public.""User"" as u on u.""Id"" = g.""CollectedByUserId""
                            where g.""ParcelId"" = '{gid}'
                        ";
            var res = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return res;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetTotalRevenue(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null)
        {
            var where = "";
            if (year != null)
            {
                where = $@"{where} and date_part('year',pay.""CreatedDate"")={year}";
            }
            if (months.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and date_part('month',pay.""CreatedDate"") in ({months})";
            }
            if (wards.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and p.""ward_no"" in ('{wards.Replace(",", "','")}')";
            }
            if (assetTypes.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and a.""Id"" in ('{assetTypes.Replace(",", "','")}')";
            }
            if (revenueTypes.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and r.""Id"" in ('{revenueTypes.Replace(",", "','")}')";
            }
            var query = $@"select 'Total Revenue' as ""GroupName"",
            sum(pay.""Amount""::int) ""Expected"",
            sum(case when lov.""Code"" = 'JSC_SUCCESS' then pay.""Amount""::int else 0 end) ""Actual""
            from public.""parcel"" p
            join cms.""N_JAMMU_SMART_CITY_JSCPayment"" pay on p.""gid""::text=pay.""SourceReferenceId""
            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" a on p.""usg_cat_gf""=a.""Code"" and a.""IsDeleted""=false
            join cms.""F_JSC_REV_REVENUE_TYPE"" r on pay.""RevenueTypeId""=r.""Id"" and r.""IsDeleted""=false
            join public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
            left join public.""LOV"" lov on pay.""PaymentStatus""=lov.""Id"" and pay.""IsDeleted""=false
            where pay.""IsDeleted""=false <<where>>";
            query = query.Replace("<<where>>", where);
            var res = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return res;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetRevenueByWard(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null)
        {
            var where = "";
            if (year != null)
            {
                where = $@"{where} and date_part('year',pay.""CreatedDate"")={year}";
            }
            if (months.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and date_part('month',pay.""CreatedDate"") in ({months})";
            }
            if (wards.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and p.""ward_no"" in ('{wards.Replace(",", "','")}')";
            }
            if (assetTypes.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and a.""Id"" in ('{assetTypes.Replace(",", "','")}')";
            }
            if (revenueTypes.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and r.""Id"" in ('{revenueTypes.Replace(",", "','")}')";
            }
            var query = $@"select w.""wrd_name"" as ""GroupName"",
            sum(pay.""Amount""::int) ""Expected"",
            sum(case when lov.""Code"" = 'JSC_SUCCESS' then pay.""Amount""::int else 0 end) ""Actual""
            from public.""parcel"" p
            join cms.""N_JAMMU_SMART_CITY_JSCPayment"" pay on p.""gid""::text=pay.""SourceReferenceId""
            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" a on p.""usg_cat_gf""=a.""Code"" and a.""IsDeleted""=false
            join cms.""F_JSC_REV_REVENUE_TYPE"" r on pay.""RevenueTypeId""=r.""Id"" and r.""IsDeleted""=false
            join public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
            left join public.""LOV"" lov on pay.""PaymentStatus""=lov.""Id"" and pay.""IsDeleted""=false
            where pay.""IsDeleted""=false <<where>> group by w.""wrd_name""";
            query = query.Replace("<<where>>", where);
            var res = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return res;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetRevenueByAssetType(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null)
        {
            var where = "";
            if (year != null)
            {
                where = $@"{where} and date_part('year',pay.""CreatedDate"")={year}";
            }
            if (months.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and date_part('month',pay.""CreatedDate"") in ({months})";
            }
            if (wards.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and p.""ward_no"" in ('{wards.Replace(",", "','")}')";
            }
            if (assetTypes.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and a.""Id"" in ('{assetTypes.Replace(",", "','")}')";
            }
            if (revenueTypes.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and r.""Id"" in ('{revenueTypes.Replace(",", "','")}')";
            }
            var query = $@"select a.""Name"" as ""GroupName"",
            sum(pay.""Amount""::int) ""Expected"",
            sum(case when lov.""Code"" = 'JSC_SUCCESS' then pay.""Amount""::int else 0 end) ""Actual""
            from public.""parcel"" p
            join cms.""N_JAMMU_SMART_CITY_JSCPayment"" pay on p.""gid""::text=pay.""SourceReferenceId""
            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" a on p.""usg_cat_gf""=a.""Code"" and a.""IsDeleted""=false
            join cms.""F_JSC_REV_REVENUE_TYPE"" r on pay.""RevenueTypeId""=r.""Id"" and r.""IsDeleted""=false
            join public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
            left join public.""LOV"" lov on pay.""PaymentStatus""=lov.""Id"" and pay.""IsDeleted""=false
            where pay.""IsDeleted""=false <<where>> group by a.""Name""";
            query = query.Replace("<<where>>", where);
            var res = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return res;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetRevenueByRevenueType(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null)
        {
            var where = "";
            if (year != null)
            {
                where = $@"{where} and date_part('year',pay.""CreatedDate"")={year}";
            }
            if (months.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and date_part('month',pay.""CreatedDate"") in ({months})";
            }
            if (wards.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and p.""ward_no"" in ('{wards.Replace(",", "','")}')";
            }
            if (assetTypes.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and a.""Id"" in ('{assetTypes.Replace(",", "','")}')";
            }
            if (revenueTypes.IsNotNullAndNotEmpty())
            {
                where = $@"{where} and r.""Id"" in ('{revenueTypes.Replace(",", "','")}')";
            }
            var query = $@"select r.""Name"" as ""GroupName"",
            sum(pay.""Amount""::int) ""Expected"",
            sum(case when lov.""Code"" = 'JSC_SUCCESS' then pay.""Amount""::int else 0 end) ""Actual""
            from public.""parcel"" p
            join cms.""N_JAMMU_SMART_CITY_JSCPayment"" pay on p.""gid""::text=pay.""SourceReferenceId""
            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" a on p.""usg_cat_gf""=a.""Code"" and a.""IsDeleted""=false
            join cms.""F_JSC_REV_REVENUE_TYPE"" r on pay.""RevenueTypeId""=r.""Id"" and r.""IsDeleted""=false
            join public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
            left join public.""LOV"" lov on pay.""PaymentStatus""=lov.""Id"" and pay.""IsDeleted""=false
            where pay.""IsDeleted""=false <<where>> group by r.""Name""";
            query = query.Replace("<<where>>", where);
            var res = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return res;
        }

        public async Task<List<JSCPaymentViewModel>> GetPaymentDetailsForConsumer(string mobileNo, string aadhar)
        {
            var query = $@" select p.*, l.""Name""  as PaymentStatusName, u.""Name"" as OwnerName, lov.""Name"" as PaymentModeName
                            , par.""pcl_id"", par.""prop_id"", r.""Name"" as RevenueTypeName
                            from cms.""N_JAMMU_SMART_CITY_JSCPayment"" as p
                            join public.""LOV"" as l on l.""Id"" = p.""PaymentStatus"" and l.""IsDeleted""=false
                            join public.""User"" as u on u.""Id"" = p.""NoteOwnerUserId"" and p.""IsDeleted""=false
                            join public.""LOV"" as lov on lov.""Id"" = p.""PaymentMode"" and lov.""IsDeleted""=false
                            join public.""parcel"" as par on par.""gid""::text = p.""SourceReferenceId""
                            join cms.""F_JSC_REV_REVENUE_TYPE"" as r on r.""Id"" = p.""RevenueTypeId"" and r.""IsDeleted""=false
                            where u.""NationalId"" = '{aadhar}' or u.""Mobile"" = '{mobileNo}'
                        ";
            var res = await _queryRepo.ExecuteQueryList<JSCPaymentViewModel>(query, null);
            return res;
        }

        public async Task<List<JSCParcelViewModel>> GetPropertyDetailsForConsumer(string mobileNo, string aadhar)
        {
            var query = $@"SELECT 
                            p.""ward_no"", p.""res_stat"", p.""road_desc"", p.""road_type"", p.""own_dtls"", p.""own_name"", p.""tel_no"",
                            p.""aadhar"", p.""sub_loc"", p.""locality"", p.""sector"", p.""post_off"", p.""pin_code"", p.""bu_type"",
                            p.""usg_cat_gf"", p.""building"", p.""pcl_id"", p.""gid"",p.""prop_id""
                            FROM public.""parcel"" as p
                            where p.""tel_no""='{mobileNo}' or p.""aadhar"" = '{aadhar}'
                        ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task<JSCParcelViewModel> GetParcelByMobileOrAadhar(string value)
        {
            var query = $@" select p.* 
                            from public.""parcel"" as p 
                            where p.""tel_no""='{value}' or p.""aadhar""='{value}'
                        ";
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task<JSCParcelViewModel> GetParcelByPropertyId(string mmid, string userId = null)
        {
            var query = $@" select p.* , 'Feature' as ""type"",
                            ST_AsGeoJSON(ST_Transform(p.""geom"", 4326), 6) :: text as ""geometry"",
                            case when gc.""CollectionDateTime"" is not null then true else false end IsGarbageCollected,
                            lp.""Name"" as PropertyTypeName
                            from public.""parcel"" as p 
                            join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = p.""mmi_id"" and gcp.""IsDeleted"" = false
                            join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
                            left join cms.""F_JSC_REV_GarbageCollection"" as gc on p.""mmi_id""=gc.""ParcelId"" and gc.""IsDeleted""=false                            
                            and gc.""CollectionDateTime""::Date ='{DateTime.Now.Date.ToDatabaseDateFormat()}'::Date <<WHERE>>
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = gc.""CollectedByUserId"" and c.""IsDeleted"" = false
                            where p.""mmi_id"" = '{mmid}' ";

            string where = userId.IsNotNullAndNotEmpty() ? $@"and gc.""CollectedByUserId""='{userId}'" : "";
            query = query.Replace("<<WHERE>>", where);

            var querydata = await _queryRepo.ExecuteQuerySingle<JSCParcelViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCLandRateViewModel>> GetFormulaFactor(string year)
        {
            var query = $@" select p.*,lv.""Code"" as FormulaType
                            from cms.""F_JSC_PROP_MGMNT_FormulaFactors"" as p 
                            join public.""LOV"" as lv on lv.""Id""=p.""FormulaType""
                            where p.""FinancialYear"" = '{year}'                            
                            
                        ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<JSCFormulaViewModel> GetFormula(string type)
        {
            var query = $@" select p.*,lv.""Code"" as FormulaTypeCode
                            from cms.""F_JSC_PROP_MGMNT_FORMULA_SETTINGS"" as p 
                            left join public.""LOV"" as lv on lv.""Id""=p.""FormulaType""
                            where p.""IsDeleted""=false  and p.""FormulaType""='{type}'
                            --and date(p.""EffectiveFromDate"")<=CURRENT_DATE and date(p.""EffectiveToDate"")>= CURRENT_DATE
                        ";
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCFormulaViewModel>(query, null);
            return querydata;
        }
        public async Task<JSCLandRateViewModel> GetLandRate(string id, string type)
        {
            var query = "";
            if (type == "LOCATION")
            {
                query = $@" select p.""Code"" as Code,p.""Factor"" as Rate
                            from cms.""F_JSC_PROP_MGMNT_LocationFactor"" as p 
                            where p.""Id"" = '{id}'     ";
            }
            else if (type == "USAGE")
            {
                query = $@" select p.""Rate"" as Rate
                            from cms.""F_JSC_PROP_MGMNT_UsageFactorCommercialProperty"" as p 
                            where p.""CategoryId"" = '{id}'    ";
            }
            else if (type == "OCCUPANCY")
            {
                query = $@" select p.""Code"" as Code,p.""Factor"" as Rate
                            from cms.""F_JSC_PROP_MGMNT_OccupancyFactor"" as p 
                            where p.""Id"" = '{id}'       ";
            }
            else if (type == "ROOF")
            {
                query = $@" select p.""Factor"" as Rate
                            from cms.""F_JSC_PROP_MGMNT_NatureOfCunstruction"" as p 
                            where p.""Id"" = '{id}'      ";
            }
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<JSCLandRateViewModel> GetLandValueRate(string ward, string property,string type)
        {
            var queryCircle = $@" select p.*
                            from cms.""F_JSC_PROP_MGMNT_CircleRate"" as p 
                            where p.""WardNumberAndName"" = '{ward}'   ";

            var circleData = await _queryRepo.ExecuteQuerySingle<JSCLandRateViewModel>(queryCircle, null);

            var query = $@" select p.""Factor"" as Rate
                            from cms.""F_JSC_PROP_MGMNT_LocationFactor"" as p 
                            where p.""propertyType"" = '{property}'  and  
                            ({circleData.ValueOfResidential_InLacs}>=p.""RangeFrom""::bigint  and {circleData.ValueOfResidential_InLacs}<=p.""RangeTo""::bigint)
                            ";
            if(type != "JSC_RESIDENTIAL")
            {
                query = $@" select p.""Factor"" as Rate
                            from cms.""F_JSC_PROP_MGMNT_LocationFactor"" as p 
                            where p.""propertyType"" = '{property}'  and  
                            ({circleData.ValueOfCommercial_InLacs}>=p.""RangeFrom""::bigint  and {circleData.ValueOfCommercial_InLacs}<=p.""RangeTo""::bigint)
                            ";
            }
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCLandRateViewModel>(query, null);

            return querydata;
        }
        public async Task<JSCLandRateViewModel> GetPropertyTaxRate()
        {
            var query = $@" select p.*
                            from cms.""F_JSC_PROP_MGMNT_PROPERTY_TAX_RATE"" as p 
                            where p.""IsDeleted"" = false                  
                           
                        ";
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCLandRateViewModel>> GetUnitArea(string year)
        {
            var query = $@" select p.""Code"" as Code,p.""Rate"" as Rate
                            from cms.""F_JSC_REV_UnitAreaValue"" as p 
                            where p.""FinancialYear"" = '{year}'                      
                           
                        ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCLandRateViewModel>> GetLocationFactor(string year)
        {
            var query = $@" select p.""Code"" as Code,p.""Factor"" as Rate
                            from cms.""F_JSC_PROP_MGMNT_LocationFactor"" as p 
                            where p.""FinancialYear"" = '{year}'                      
                           
                        ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCLandRateViewModel>> GetAgeFactor(string year)
        {
            var query = $@" select p.""Code"" as Code,p.""Factor"" as Rate,p.""RangeFrom"",p.""RangeTo""
                            from cms.""F_JSC_MASTER_AgeFactor"" as p 
                            where ""IsDeleted"" =false                   
                           
                        ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCLandRateViewModel>> GetOccupancyFactor(string year)
        {
            var query = $@" select p.""Code"" as Code,p.""Factor"" as Rate
                            from cms.""F_JSC_PROP_MGMNT_OccupancyFactor"" as p 
                            where p.""FinancialYear"" = '{year}'                      
                           
                        ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCLandRateViewModel>> GetUsageFactor(string year)
        {
            var query = $@" select p.""Code"" as Code,p.""Rate"" as Rate,p.""Name"" as Name
                            from cms.""F_JSC_PROP_MGMNT_UsageFactorCommercialProperty"" as p 
                            where p.""FinancialYear"" = '{year}'                      
                           
                        ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCLandRateViewModel>> GetRoofFactor(string year)
        {
            var query = $@"  select p.""Factor"" as Rate,p.""Id""
                            from cms.""F_JSC_PROP_MGMNT_NatureOfCunstruction"" as p 
                            where p.""IsDeleted"" = false                     
                           
                        ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCLandRateViewModel>(query, null);
            return querydata;
        }
        public async Task<JSCParcelViewModel> CheckPropertyTaxExist(string propId, string year)
        {
            var query = $@" select p.* 
                            from cms.""N_TNC_JSC_PROPERTY_MANAGEMENT_PropertyTax"" as p 
                            join public.""NtsTask"" as t on t.""UdfNoteTableId"" = p.""Id"" and t.""IsDeleted"" = false
                            join public.""LOV"" as ts on ts.""Id""=t.""TaskStatusId""
                            where p.""IsDeleted"" = false and p.""ParcelId""='{propId}' and p.""FinancialYear""='{year}' and ts.""Code""!='TASK_STATUS_REJECT' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCParcelViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<ServiceTemplateViewModel>> GetMyAllRequestList(bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string searchtext = null, DateTime? From = null, DateTime? To = null, string statusIds = null, string templateIds = null)
        {
            var query = $@"Select  s.""Id"" as ServiceId, s.""ServiceNo"" as ServiceNo, t.""DisplayName"" as TemplateDisplayName,t.""Code"" as TemplateCode,   
ss.""Name"" as ServiceStatusName,ss.""Code"" as ServiceStatusCode, s.""CreatedDate"" as CreatedDate,s.""ServiceSubject"" as ServiceSubject
from public.""NtsService"" as s
join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false #USERWHERE#
join public.""Template"" as t on t.""Id"" =s.""TemplateId"" and t.""IsDeleted""=false
join public.""TemplateCategory"" as tc on tc.""Id"" =t.""TemplateCategoryId"" and tc.""IsDeleted""=false #TEMCATCODEWHERE#
join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false 
left join public.""Module"" as m on m.""Id"" =t.""ModuleId"" and m.""IsDeleted""=false #MCODEWHERE#
where s.""IsDeleted""=false and s.""PortalId""='{_repo.UserContext.PortalId}' #WHERE# #STATUSWHERE# #TEMPWHERE# #DATEWHERE# order by s.""CreatedDate"" desc ";

            var user = "";
            if (!showAllOwnersService)
            {
                user = $@" and u.""Id""='{_repo.UserContext.UserId}'";
            }
            query = query.Replace("#USERWHERE#", user);

            var catcode = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                catcode = $@" and tc.""Code"" in ('{categoryCodes.Replace(",", "','")}')";
            }
            query = query.Replace("#TEMCATCODEWHERE#", catcode);

            var temcode = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                temcode = $@" and t.""Code"" in ('{templateCodes.Replace(",", "','")}')";
            }
            query = query.Replace("#TEMCATCODEWHERE#", temcode);

            var mcode = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                mcode = $@" and m.""Code"" in ('{moduleCodes.Replace(",", "','")}')";
            }
            query = query.Replace("#MCODEWHERE#", mcode);

            var status = "";
            if (statusIds.IsNotNullAndNotEmpty())
            {
                status = $@" and s.""ServiceStatusId"" in ('{statusIds.Replace(",", "','")}')";
            }
            query = query.Replace("#STATUSWHERE#", status);

            var temp = "";
            if (templateIds.IsNotNullAndNotEmpty())
            {
                temp = $@" and t.""Id"" in ('{templateIds.Replace(",", "','")}')";
            }
            query = query.Replace("#TEMPWHERE#", temp);

            var where = "";
            if (searchtext.IsNotNullAndNotEmpty())
            {
                where = $@" and lower(s.""ServiceNo"") like '%{searchtext}%' COLLATE ""tr-TR-x-icu"" ";
            }
            query = query.Replace("#WHERE#", where);

            var datesearch = "";
            if (From.HasValue)
            {
                if (To.HasValue)
                {
                    datesearch = $@" and s.""CreatedDate""::TIMESTAMP::DATE>='{From}'::TIMESTAMP::DATE and s.""CreatedDate""::TIMESTAMP::DATE<='{To}'::TIMESTAMP::DATE ";
                }
                else
                {
                    datesearch = $@" and s.""CreatedDate""::TIMESTAMP::DATE>='{From}'::TIMESTAMP::DATE ";
                }
            }
            else if (To.HasValue)
            {
                datesearch = $@" and s.""CreatedDate""::TIMESTAMP::DATE<='{To}'::TIMESTAMP::DATE ";
            }
            query = query.Replace("#DATEWHERE#", datesearch);

            var result = await _query.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<IList<IdNameViewModel>> GetAssetTypeIdNameList()
        {
            string query = $@"select distinct ""bu_type"" as Id, ""bu_type"" as Name from public.""parcel"" ";

            var result = await _querydata.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<PropertyTaxPaymentViewModel>> GetPropertyTaxPaymentDetails(string propNo = null, string taskNo = null)
        {
            var query = $@" select t.""TaskNo"",f.""FinancialYearName"",p.* 
                            from cms.""N_TNC_JSC_PROPERTY_MANAGEMENT_PropertyTax"" as p 
                            join public.""NtsTask"" as t on t.""UdfNoteTableId"" = p.""Id"" and t.""IsDeleted"" = false
                            join cms.""F_JSC_PROP_MGMNT_JSCFinancialYear"" as f on f.""Id"" = p.""FinancialYear"" and f.""IsDeleted"" = false
                            where p.""IsDeleted"" = false #propNo# #taskNo#
                        ";
            var propertyVal = "";
            var taskNoVal = "";
            if (propNo.IsNotNullAndNotEmpty())
            {
                propertyVal = $@" and p.""ParcelId"" = '{propNo}' ";
            }
            query = query.Replace("#propNo#", propertyVal);
            if (taskNo.IsNotNullAndNotEmpty())
            {
                taskNoVal = $@" and t.""TaskNo"" = '{taskNo}' ";
            }
            query = query.Replace("#taskNo#", taskNoVal);

            var result = await _queryRepo.ExecuteQueryList<PropertyTaxPaymentViewModel>(query, null);
            return result;

        }

        public async Task<IdNameViewModel> GetFinancialYearDetailsById(string year)
        {
            string query = $@"select ""FinancialYearName"" as Name, ""Id"" as Id from cms.""F_JSC_PROP_MGMNT_JSCFinancialYear""
                            where ""FinancialYearName"" = '{year}'";
            var result = await _querydata.ExecuteScalar<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetTransferStationList()
        {
            string query = $@"select ""Name"" as Name, ""Id"" as Id, ""UserId"" as Code  from cms.""F_JSC_REV_TransferStation""";
            var result = await _querydata.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;

        }

        public async Task<JSCCollectorViewModel> GetCollectorDetailsByUserId(string userId)
        {
            string query = $@"select c.*, string_agg(DISTINCT w.""wrd_name"",',') as WardNames,string_agg(DISTINCT w.""wrd_no"",',') as WardNo 
from cms.""F_JSC_REV_JSC_COLLECTOR"" as c
join cms.""F_JSC_REV_AllocationWardToCollector"" as awc on c.""Id""=awc.""Collector"" and awc.""IsDeleted""=false
join public.""ward"" as w on awc.""WardNo""=w.""wrd_no""
where c.""IsDeleted""=false and c.""UserId""='{userId}'
group by c.""Id"" ";

            var result = await _querydata.ExecuteQuerySingle<JSCCollectorViewModel>(query, null);
            return result;
        }
        public async Task<JSCGrievanceWorkflow> GetGrievanceWorkflowById(string id)
        {
            var query = $@" select * from cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" where ""Id"" = '{id}' and ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JSCGrievanceWorkflow>(query, null);
            return queryData;
        }
        public async Task<JSCFormulaViewModel> GetFormulaById(string id)
        {
            var query = $@" select * from cms.""F_JSC_PROP_MGMNT_FORMULA_SETTINGS"" where ""Id"" = '{id}' and ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JSCFormulaViewModel>(query, null);
            return queryData;
        }
        public async Task<List<JSCFormulaViewModel>> GetFormulaList(string type)
        {
            var query = $@" select f.* from cms.""F_JSC_PROP_MGMNT_FORMULA_SETTINGS"" as f
                join public.""LOV"" as lv on lv.""Id""=f.""FormulaType"" where f.""IsDeleted"" = false 
                and lv.""Code""='{type}' ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCFormulaViewModel>(query, null);
            return queryData;
        }
        public async Task<JSCFormulaViewModel> GetFormulaFactor()
        {
            var query = $@" select * from cms.""F_JSC_PROP_MGMNT_FormulaFactors"" where ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JSCFormulaViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetFormulaType()
        {
            var query = $@" select * from cms.""F_JSC_PROP_MGMNT_FACTOR_TYPE"" where ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<JSCFormulaViewModel> GetLatestFormula()
        {
            var query = $@" select * from cms.""F_JSC_PROP_MGMNT_FORMULA_SETTINGS"" where ""Status"" = 1 and ""IsDeleted"" = false
                order by ""CreatedDate"" desc limit 1 ";
            var queryData = await _queryRepo.ExecuteQuerySingle<JSCFormulaViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JSCFormulaFactorViewModel>> GetFormulaTypeByCode(string year, string code)
        {
            var query = $@" select ff.*,ft.""PropertyColumnType"" as PropertyColumnType,ft.""PropertyColumn"" as PropertyColumnType
            from cms.""F_JSC_PROP_MGMNT_FormulaFactors"" as ff
            join cms.""F_JSC_PROP_MGMNT_FACTOR_TYPE"" as ft on ft.""Id""=ff.""FormulaTypeId""
            
            where ff.""IsDeleted"" = false and ft.""IsDeleted"" = false and ft.""Code""='{code}' and ff.""FinancialYear""='{year}' ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCFormulaFactorViewModel>(query, null);
            return queryData;
        }
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByUserId(string userId, DateTime? date = null, string mobileNo = null, string userName = null, string ddnNo = null)
        {
            //            string query = $@"Select c.""Name"" as CollectedByUserName,gc.""CollectionDateTime"",Concat(p.""prop_id"",'_', p.""mmi_id"", '_', p.""ward_no"", '_', p.""own_name"") as PropertyName,
            //case when gc.""CollectionDateTime"" is not null then true else false end ""IsGarbageCollected"",p.""ward_no"" as WardNo,w.""wrd_name"" as WardName
            //from cms.""F_JSC_REV_JSC_COLLECTOR"" as c
            //join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on c.""Id""=gcp.""CollectorId"" and gcp.""IsDeleted""=false
            //join public.""parcel"" as p on ""ParcelId""=p.""mmi_id""
            //join public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
            //left join cms.""F_JSC_REV_GarbageCollection"" as gc on p.""mmi_id""=gc.""ParcelId"" and gc.""IsDeleted""=false and gc.""CollectionDateTime""::Date='{date.Value.Date}'
            //where c.""UserId""='{userId}' and c.""IsDeleted""=false <<MOBWHERE>> <<USERNAMEWHERE>> <<PROPERTYWHERE>> --and gc.""CollectedByUserId""='{userId}' ";

            string query = $@"Select distinct c.""Name"" as CollectedByUserName,gc.""CollectionDateTime"",--Concat(p.""prop_id"",'_', p.""mmi_id"", '_', p.""ward_no"", '_', p.""own_name"") as PropertyName,
                                p.""own_name"" as PropertyName,p.""mmi_id"" as DDN,
                                case when gc.""CollectionDateTime"" is not null then true else false end ""IsGarbageCollected"",p.""ward_no"" as WardNo,w.""wrd_name"" as WardName,
                                s.""ServiceNo"" as ComplaintNo,s.""Id"" as ComplaintId,u.""Name"",u.""Mobile"", gc.""ComplaintCount"" as ComplaintCount,gc.""Id"" as CollectionId
                                from cms.""F_JSC_REV_JSC_COLLECTOR"" as c
                                join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on c.""Id""=gcp.""CollectorId"" and gcp.""IsDeleted""=false
                                join public.""parcel"" as p on gcp.""ParcelId""=p.""mmi_id"" <<PROPERTYWHERE>>
                                join public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                                left join cms.""F_JSC_REV_GarbageCollection"" as gc on p.""mmi_id""=gc.""ParcelId"" and gc.""IsDeleted""=false and gc.""CollectionDateTime""::Date='{date.Value.Date.ToDatabaseDateFormat()}'
                                left join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on p.""mmi_id""=lc.""DDN"" and lc.""EventDate""::Date='{date.Value.Date.ToDatabaseDateFormat()}'::Date and lc.""IsDeleted""=false
                                left join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department"" and d.""Code"" = 'SA'
                                left join public.""NtsService"" as s on lc.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                                left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE')
                                left join public.""User"" as u on p.""tel_no""=u.""Mobile"" and u.""IsDeleted""=false <<MOBWHERE>> <<USERNAMEWHERE>>
                                where c.""UserId""='{userId}' and c.""IsDeleted""=false ";

            var mobwhere = mobileNo.IsNotNullAndNotEmpty() ? $@" and u.""Mobile""='{mobileNo}' " : "";
            query = query.Replace("<<MOBWHERE>>", mobwhere);

            var usernamewhere = userName.IsNotNullAndNotEmpty() ? $@" and u.""Name"" like '%{userName}%' COLLATE ""tr-TR-x-icu"" " : "";
            query = query.Replace("<<USERNAMEWHERE>>", usernamewhere);

            var propertywhere = ddnNo.IsNotNullAndNotEmpty() ? $@" and p.""mmi_id""='{ddnNo}' " : "";
            query = query.Replace("<<PROPERTYWHERE>>", propertywhere);

            var result = await _querydata.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);

            var bwgq = $@"select distinct c.""nameOfTheRouteCoordinator"" as CollectedByUserName, 
                            c.""CreatedDate"" as CollectionDateTime,
                            p.""mmi_id"" as DDN,
                            true as IsGarbageCollected
                            ,p.""ward_no"" as WardNo
                            from  cms.""F_JSC_REV_BWGsWasteCollectionFormat"" as c
                            join public.""parcel"" as p on c.""ParcelId""=p.""mmi_id""
                            left join public.""User"" as u on c.""CreatedBy""=u.""Id"" and u.""IsDeleted""=false
                            where c.""CreatedBy"" = '{userId}'";

            result.AddRange( await _querydata.ExecuteQueryList<JSCGarbageCollectionViewModel>(bwgq, null));


            return result;

        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByCitizen(string userId, DateTime? date = null, string ddnNo = null)
        {
            string query = $@"select distinct c.""Name"" as CollectedByUserName,gc.""CollectionDateTime"",--Concat(p.""prop_id"",'_', p.""mmi_id"", '_', p.""ward_no"", '_', p.""own_name"") as PropertyName,
p.""own_name"" as PropertyName,p.""mmi_id"" as DDN,
case when gc.""CollectionDateTime"" is not null then true else false end ""IsGarbageCollected"",p.""ward_no"" as WardNo,w.""wrd_name"" as WardName,p.""mmi_id"" as DDN,
s.""ServiceNo"" as ComplaintNo,s.""Id"" as ComplaintId, gc.""ComplaintCount"" as ComplaintCount, gc.""Id"" as CollectionId
from public.""User"" as u
join public.""parcel"" as p on u.""Mobile""=p.""tel_no""
join public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on p.""mmi_id""=gcp.""ParcelId"" and gcp.""IsDeleted""=false
left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on gcp.""CollectorId""=c.""Id"" and c.""IsDeleted""=false
left join cms.""F_JSC_REV_GarbageCollection"" as gc on p.""mmi_id""=gc.""ParcelId"" and gc.""IsDeleted""=false and gc.""CollectionDateTime""::Date='{date.Value.Date.ToDatabaseDateFormat()}'::Date
left join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on p.""mmi_id""=lc.""DDN"" and lc.""EventDate""::Date='{date.Value.Date.ToDatabaseDateFormat()}'::Date and lc.""IsDeleted""=false
left join public.""NtsService"" as s on lc.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE')
where u.""IsDeleted""=false <<USERWHERE>> <<PROPERTYWHERE>>  ";

            var userwhere = userId.IsNotNullAndNotEmpty() ? $@" and u.""Id""='{userId}' " : "";
            query = query.Replace("<<USERWHERE>>", userwhere);

            var propertywhere = ddnNo.IsNotNullAndNotEmpty() ? $@" and p.""mmi_id""='{ddnNo}' " : "";
            query = query.Replace("<<PROPERTYWHERE>>", propertywhere);

            var result = await _querydata.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            var startDate = date.Value.AddDays(-2);

            foreach (var res in result)
            {
                var querycnt = $@"select Count(gc.*)
                                    from cms.""F_JSC_REV_CitizenGarbageCollectionComplaint"" as gc 
                                    where gc.""DDN"" = '{res.DDN}' and (gc.""Date""::date >= '{startDate.ToDatabaseDateFormat()}'::date and gc.""Date""::date <= '{date.Value.Date.ToDatabaseDateFormat()}'::date) ";

                var complaintCount = await _queryRepo.ExecuteScalar<int>(querycnt, null);
                res.ComplaintCount = complaintCount.IsNotNull() ? complaintCount : 0;
            }

            return result;

        }

        public async Task UpdateCollectorUserId(string id, string userId)
        {
            var query = $@" update cms.""F_JSC_REV_JSC_COLLECTOR"" set ""UserId"" = '{userId}' where ""Id"" = '{id}'
                        ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task UpdateDriverUserId(string id, string userId)
        {
            var query = $@" update cms.""F_TypeOfUsers_DriverMaster"" set ""UserId"" = '{userId}' where ""Id"" = '{id}'
                        ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task UpdateComplaintOperatorUserId(string id, string userId)
        {
            var query = $@" update cms.""F_JSC_PROP_MGMNT_ComplaintOperator"" set ""UserId"" = '{userId}' where ""Id"" = '{id}'
                        ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task UpdateTransferStationUserId(string id, string userId)
        {
            var query = $@" update cms.""F_JSC_REV_TransferStation"" set ""UserId"" = '{userId}' where ""Id"" = '{id}'
                        ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task UpdateWTPUserId(string id, string userId)
        {
            var query = $@" update cms.""F_JSC_REV_JSC_WTP"" set ""UserId"" = '{userId}' where ""Id"" = '{id}'
                        ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task UpdateSubLoginUserId(string id, string userId)
        {
            var query = $@" update cms.""F_JSC_ENFORCEMENT_SubLogins"" set ""SubloginUserId"" = '{userId}' where ""Id"" = '{id}'
                        ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetAllGarbageCollectionData(string autoNo, string wardNo, string collector, DateTime? collectionDate)
        {
            var query = $@" select distinct g.* ,p.""ward_no"" as WardNo, c.""Name"" as CollectedByUserName, aa.""AutoNumber"",g.""ParcelId"" as ParcelId,
                            g.""CollectionDateTime"" as CollectionDateTime
                            from cms.""F_JSC_REV_GarbageCollection"" as g
                            left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false
                            left join cms.""F_JSC_PROP_MGMNT_AutoMaster"" as aa on aa.""Id"" = ac.""Auto"" and aa.""IsDeleted"" = false
                            where g.""IsDeleted""=false  #autoNo# #ward# #collector# #collectionDate#
                            order by  g.""CollectionDateTime"" desc
                        ";

            var autoVal = "";
            var wardVal = "";
            var collectorVal = "";
            var collectionDateVal = "";

            if (autoNo.IsNotNullAndNotEmpty())
            {
                autoVal = $@" and aa.""Id"" = '{autoNo}' ";
            }
            query = query.Replace("#autoNo#", autoVal);

            if (wardNo.IsNotNullAndNotEmpty())
            {
                wardVal = $@" and p.""ward_no"" = '{wardNo}' ";
            }
            query = query.Replace("#ward#", wardVal);

            if (collector.IsNotNullAndNotEmpty())
            {
                collectorVal = $@" and c.""Name"" = '{collector}' ";
            }
            query = query.Replace("#collector#", collectorVal);

            if (collectionDate.IsNotNull())
            {
                var date = Convert.ToString(collectionDate);
                var dateVal = date.Split(' ')[0];
                collectionDateVal = $@" and g.""CollectionDateTime""::date = '{dateVal}'::date ";

            }
            query = query.Replace("#collectionDate#", collectionDateVal);

            var queryData = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return queryData;
        }


        public async Task<List<IdNameViewModel>> GetGrievanceTypeByDepartment(string department)
        {
            string query = $@"select * from cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gc where ""Department"" = '{department}' ";

            var result = await _querydata.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<string> GetPropertyIdsByPhoneNo(string userId)
        {
            string query = $@"select u.""Mobile"" from public.""parcel""  as p
                           left  join public.""User"" as u on u.""Mobile"" = p.""tel_no""
                            where u.""Id""='{userId}'";

            var result = await _querydata.ExecuteScalar<string>(query, null);
            return result;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetUserDoorToDoorGarbageCollectionData(string userId, string garbageType, DateTime? collectionDate = null)
        {
            var date = Convert.ToString(collectionDate);
            var dateVal = date.Split(' ')[0];
            var query = "";

            var telephone = await GetPropertyIdsByPhoneNo(_userContext.UserId);
            if (garbageType == "Res_Com")
            {
                query = $@" select g.* ,p.""ward_no"" as WardNo, c.""Name"" as CollectedByUserName, aa.""AutoNumber""
                            from cms.""F_JSC_REV_GarbageCollection"" as g
                            left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
							left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = c.""Id"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as l on l.""Id"" = gcp.""GarbageTypeId"" and l.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false
                            left join cms.""F_JSC_PROP_MGMNT_AutoMaster"" as aa on aa.""Id"" = ac.""Auto"" and aa.""IsDeleted"" = false
                            where g.""IsDeleted""=false and p.""tel_no"" = '{telephone}' and l.""Code"" = 'JSC_GARBAGE_COMMERCIAL' and g.""CollectionDateTime"" LIKE '{dateVal}%'
                            Union
                            select g.* ,p.""ward_no"" as WardNo, c.""Name"" as CollectedByUserName, aa.""AutoNumber""
                            from cms.""F_JSC_REV_GarbageCollection"" as g
                            left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
							left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = c.""Id"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as l on l.""Id"" = gcp.""GarbageTypeId"" and l.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false
                            left join cms.""F_JSC_PROP_MGMNT_AutoMaster"" as aa on aa.""Id"" = ac.""Auto"" and aa.""IsDeleted"" = false
                            where g.""IsDeleted""=false and p.""tel_no"" = '{telephone}' and l.""Code"" = 'JSC_GARBAGE_RESIDENTIAL' and g.""CollectionDateTime"" LIKE '{dateVal}%'
                        ";
            }
            else
            {
                query = $@"  select g.* ,p.""ward_no"" as WardNo, c.""Name"" as CollectedByUserName, aa.""AutoNumber""
                            from cms.""F_JSC_REV_GarbageCollection"" as g
                            left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
							left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = c.""Id"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as l on l.""Id"" = gcp.""GarbageTypeId"" and l.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false
                            left join cms.""F_JSC_PROP_MGMNT_AutoMaster"" as aa on aa.""Id"" = ac.""Auto"" and aa.""IsDeleted"" = false
                            where g.""IsDeleted""=false and p.""tel_no"" = '{telephone}' and l.""Code"" = 'JSC_BWG' and g.""CollectionDateTime"" LIKE '{dateVal}%'
                        ";
            }
            var queryData = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return queryData;
        }

        public async Task<JSCParcelViewModel> GetParcelByDDNNO(string ddnNo)
        {
            var query = $@" select p.*,                                         
                            ST_AsGeoJSON(ST_Transform(""geom"", 4326), 6) :: text as ""geometry""
                             from public.""parcel"" as p where p.""mmi_id"" = '{ddnNo}' ";

            var querydata = await _queryRepo.ExecuteQuerySingle<JSCParcelViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCParcelViewModel>> GetPropertiesByDDNNO(string ddnNo)
        {
            var query = $@" select p.*,                                         
                            ST_AsGeoJSON(ST_Transform(""geom"", 4326), 6) :: text as ""geometry""
                             from public.""parcel"" as p where p.""mmi_id"" = '{ddnNo}' ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task<JSCGrievanceWorkflow> GetGrievanceWorkflow(string ward, string dept)
        {
            string query = $@"Select * from cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW""
                            where ""DepartmentId""='{dept}' and ""WardId"" like '%{ward}%' and ""IsDeleted""=false ";

            var res = await _queryRepo.ExecuteQuerySingle<JSCGrievanceWorkflow>(query, null);
            return res;
        }

        public async Task<List<JSCParcelViewModel>> GetJSCRegisteredAssetsList(string userId)
        {
            string query = $@"select p.*,w.""wrd_name"",ra.""RegisteredDate""
from cms.""N_SNC_JSC_RegisterAsset"" as ra
join public.""NtsService"" as s on ra.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""Code""='SERVICE_STATUS_COMPLETE'
join public.""parcel"" as p on cast(ra.""ParcelId"" as integer)=p.""gid""
JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no""
where ra.""IsDeleted""=false and ra.""UserId""='{userId}' ";

            var res = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return res;
        }

        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportData(GrievanceDatefilters datefilters, string startDate, string endDate)
        {
            string query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false order by s.""CreatedDate""";

            var res = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            if (datefilters != GrievanceDatefilters.AllTime)
            {
                var dates = GetStartEndDateByFilterType(datefilters, startDate, endDate);
                res = res.Where(x => x.CreatedDate.Date >= dates.StartDate.Date && x.CreatedDate <= dates.EndDate.Date).ToList();
            }
            return res;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportTurnaroundTimeData(string department, DateTime fromDate, DateTime toDate)
        {
            string query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                ,replace((s.""CompletedDate""::date - s.""CreatedDate""::date)::varchar ,'-', '')::int as ""NoOfDaysDisposed""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and (ls.""Code"" = 'GRV_DISPOSED') #WHEREDEPT#
                and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date)
                order by s.""CreatedDate"" ";

            var wheredept = "";
            if (department.IsNotNullAndNotEmpty())
            {
                wheredept = $@" and d.""Name""='{department}' ";
            }
            query = query.Replace("#WHEREDEPT#", wheredept);
            var res = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return res;
        }
        private DateFilter GetStartEndDateByFilterType(GrievanceDatefilters dateFilterType, string startDate, string endDate)
        {
            if (dateFilterType == GrievanceDatefilters.AllTime)
            {
                return new DateFilter();
            }
            else if (dateFilterType == GrievanceDatefilters.Custom)
            {
                return new DateFilter
                {
                    StartDate = startDate.ToSafeDateTime(),
                    EndDate = endDate.ToSafeDateTime(),
                };
            }
            else if (dateFilterType == GrievanceDatefilters.Last30Days)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddDays(-30),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == GrievanceDatefilters.Last7Days)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == GrievanceDatefilters.LastMonth)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddMonths(-1),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == GrievanceDatefilters.LastWeek)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == GrievanceDatefilters.LastYear)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddYears(-1),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == GrievanceDatefilters.ThisMonth)
            {
                return new DateFilter
                {
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == GrievanceDatefilters.ThisWeek)
            {
                int i = DateTime.Today.DayOfWeek - DayOfWeek.Monday;
                if (i == -1) i = 6;
                TimeSpan ts = new TimeSpan(i, 0, 0, 0);
                var dt = DateTime.Today.Subtract(ts);
                return new DateFilter
                {
                    StartDate = dt,
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == GrievanceDatefilters.ThisYear)
            {
                return new DateFilter
                {
                    StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == GrievanceDatefilters.Today)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1).AddTicks(-1),
                };
            }
            else if (dateFilterType == GrievanceDatefilters.Yesterday)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today.AddTicks(-1),
                };
            }
            else return new DateFilter();
        }

        public async Task<List<JSCComplaintViewModel>> GetJSCMyComplaint()
        {
            string query = $@"select distinct s.""OwnerUserId"", s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""MarkFlag"" as MarkFlag
            ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false      
                    left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false  and s.""OwnerUserId"" = '{_userContext.UserId}' order by s.""CreatedDate"" desc
				";

            var res = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return res;
        }

        public async Task<JSCComplaintViewModel> GetJSCMyComplaintById(string serviceId)
        {
            string query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,f.""FileName""  as FileName,	
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag,
                lc.""PhotoFile"" as PhotoFile,
                lc.""PhotoFileAfterReopen"" as PhotoFileAfterReopen,
                w.""Level1AssignedToUserId"" as Level1User,
                w.""Level2AssignedToUserId"" as Level2User,
                w.""Level3AssignedToUserId"" as Level3User,
                w.""Level4AssignedToUserId"" as Level4User
                , ls.""Code"" as GrvStatusCode
                from public.""NtsService"" as s
				Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and lc.""IsDeleted""=false   
                left join public.""File"" as f on f.""Id""=lc.""DocumentId"" and f.""IsDeleted""=false
                join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as w 
				on w.""DepartmentId"" = lc.""Department"" and w.""WardId"" LIKE concat('%', lc.""Ward"" ,'%') COLLATE ""tr-TR-x-icu""
				where s.""Id"" = '{serviceId}' order by s.""CreatedDate"" desc
				";

            var res = await _queryRepo.ExecuteQuerySingle<JSCComplaintViewModel>(query, null);
            return res;
        }
        public async Task<List<JSCComplaintViewModel>> GetJSCComplaintByDDN(string ddn)
        {
            //        var query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
            //            s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
            //            lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
            //            lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
            //            ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
            //            ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
            //            from public.""NtsService"" as s
            // Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
            // Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
            // Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
            //            left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
            //           left  JOIN (
            //               select count(""Id"") as reopenCount, ""ParentId""
            //               from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
            //              group by ""ParentId""
            //               ) as cnt on cnt.""ParentId"" = lc.""Id""  
            //            where s.""IsDeleted""=false and lc.""DDN""='{ddn}' order by s.""CreatedDate"" desc
            //";

            var query = $@"select  s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" = lc.""Id""  
                where s.""IsDeleted""=false and lc.""DDN""='{ddn}' order by s.""CreatedDate"" desc ";

            var res = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return res;
        }

        public async Task<List<JSCComplaintViewModel>> GetJSCComplaintForResolver(bool isAdmin, bool isUpperLevel)
        {
            string query = "";


            if ((_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("COMPLAINT_OPERATOR")) || _userContext.IsSystemAdmin)
            {

                query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false order by s.""CreatedDate"" desc
				";
            }
            else
            {
                query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id"" 
                join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as w 
				on w.""DepartmentId"" = lc.""Department"" and w.""WardId"" LIKE concat('%', lc.""Ward"" ,'%') COLLATE ""tr-TR-x-icu"" and (w.""Level1AssignedToUserId"" ='{_userContext.UserId}' or 
																					w.""Level2AssignedToUserId"" ='{_userContext.UserId}' or 
																					w.""Level3AssignedToUserId"" ='{_userContext.UserId}' or
																					w.""Level4AssignedToUserId"" ='{_userContext.UserId}')
                where s.""IsDeleted""=false  order by s.""CreatedDate"" desc
				";
            }


            var res = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return res;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportComplaintListData(string wardId, string departmentId, string complaintTypeId, string statusCode, DateTime fromDate, DateTime toDate, string complaintNo)
        {
            var query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
                left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date)
                #WHERE# 
                order by s.""CreatedDate"" desc
				";
            var where = $@"";
            if (wardId.IsNotNullAndNotEmpty())
            {
                where = where + $@" and lc.""Ward""='{wardId}' ";
            }
            if (departmentId.IsNotNullAndNotEmpty())
            {
                where = where + $@" and lc.""Department""='{departmentId}' ";
            }
            if (complaintTypeId.IsNotNullAndNotEmpty())
            {
                where = where + $@" and lc.""GrievanceType""='{complaintTypeId}' ";
            }
            if (statusCode.IsNotNullAndNotEmpty())
            {
                if (statusCode == "GRV_PENDING")
                {
                    where = where + $@" and (ls.""Code""='{statusCode}' or lc.""GrvStatus"" is null ) ";
                }
                else
                {
                    where = where + $@" and (ls.""Code""='{statusCode}') ";
                }
            }
            //if (complaintDate.IsNotNull())
            //{
            //    where = where + $@" and s.""CreatedDate""::Date='{complaintDate}'::Date ";
            //}
            if (complaintNo.IsNotNullAndNotEmpty())
            {
                where = where + $@" and s.""ServiceNo"" LIKE concat('%', '{complaintNo}' ,'%') COLLATE ""tr-TR-x-icu"" ";

            }
            query = query.Replace("#WHERE#", where);
            var res = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return res;
        }
        public async Task<IList<JSCComplaintViewModel>> GetComplaintslist(string templateCodes, string userId)
        {

            var query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and s.""OwnerUserId"" ='{_userContext.UserId}' or s.""RequestedByUserId""='{userId}' order by s.""CreatedDate"" desc";

            var result = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return result;
        }
        public async Task<List<JSCCommunityHallViewModel>> GetJSCCommunityHallIdNameList(string wardId)
        {
            string query = $@"select ch.""Id"" as CommunityHallId, ch.* 
                            from cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHall"" as ch 
                            where ch.""IsDeleted""=false #WHEREWARD# ";
            var whereward = "";
            if (wardId.IsNotNullAndNotEmpty())
            {
                whereward = $@" and ch.""Ward""='{wardId}' ";
            }
            query = query.Replace("#WHEREWARD#",whereward);

            var result = await _queryRepo.ExecuteQueryList<JSCCommunityHallViewModel>(query, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetJSCFunctionTypeIdNameList()
        {
            string query = $@"select f.""Id"", f.""Name"", f.""Code"" 
                                from cms.""F_JSC_COMMUNITY_HALL_SERVICE_TypeofFunction"" as f 
                                where f.""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<JSCCommunityHallViewModel> GetJSCCommunityHallDetailsById(string communityHallId)
        {
            DateTime today = DateTime.Today;
            string query = $@"select ch.""Id"" as CommunityHallId,sp.""SpecialRate"",typ.""Name"" as CommunityTypeName,concat(w.""wrd_no"",'-',w.""wrd_name"") as WardName
                            ,case when ch.""Images""='[]' then null else ch.""Images"" end as PhotoId, ch.* 
                            from cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHall"" as ch 
                            left join cms.""F_JSC_COMMUNITY_HALL_SERVICE_TypeofCommunityHall"" as typ on typ.""Id""=ch.""CommunityType"" and typ.""IsDeleted""=false
                            left join public.""ward"" as w on w.""wrd_no""=ch.""Ward""
                            left join cms.""F_JSC_COMMUNITY_HALL_SERVICE_SpecialRateGrid"" as sp on sp.""ParentId""=ch.""Id"" and sp.""IsDeleted""=false and (sp.""StartDate""::Date<='{today}'::Date and sp.""EndDate""::Date>='{today}'::Date)
                            where ch.""IsDeleted""=false and ch.""Id""='{communityHallId}' ";

            var result = await _queryRepo.ExecuteQuerySingle<JSCCommunityHallViewModel>(query, null);
            if (result!=null)
            {
                var photolist = await GetJSCCommunityHallPhotoIdList(communityHallId);
                if (photolist.Count>0)
                {
                    result.CommunityHallPhotoId = photolist.Select(x=>x.Id).ToList();
                }
                else
                {
                    result.CommunityHallPhotoId = new List<string>();
                }
                
            }
            return result;
        }
        public async Task<List<IdNameViewModel>> GetJSCCommunityHallPhotoIdList(string communityHallId)
        {
            string query = $@"select chp.""ImageId"" as Id
                            from cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHallPhotos"" as chp 
                            where chp.""IsDeleted""=false and chp.""ParentId""='{communityHallId}' ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<JSCCommunityHallViewModel> GetJSCCommunityHallPhotos(string communityHallId)
        {
            var result = new JSCCommunityHallViewModel();
            //string query = $@"select chp.""ImageId"" as Id
            //                from cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHallPhotos"" as chp 
            //                where chp.""IsDeleted""=false and chp.""ParentId""='{communityHallId}' ";
            //var result1 = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            var result1 = await GetJSCCommunityHallPhotoIdList(communityHallId);
            if (result1.Count>0)
            {
                result.CommunityHallPhotoId = result1.Select(x => x.Id).ToList();
            }
            return result;
        }
        public async Task<List<JSCCommunityHallViewModel>> GetJSCCommunityHallServiceChildData(string parentId)
        {
            string query = $@"select ch.*, ch.""Id"" as CommunityHallId, h.""Name""
                            from cms.""N_SNC_JSC_BOOKING_PERMISSION_JSC_CommunityHallBooking"" as ch    
                            join cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHall"" as h on h.""Id""=ch.""CommunityHallId"" and h.""IsDeleted""=false 
                            where ch.""IsDeleted""=false and ch.""ParentId""='{parentId}' ";

            var result = await _queryRepo.ExecuteQueryList<JSCCommunityHallViewModel>(query, null);
            return result;
        }
        public async Task<List<JSCCommunityHallViewModel>> SearchJSCCommunityHallList(string communityHallId, string wardId)
        {
            DateTime today = DateTime.Today;
            string query = $@" select h.*,h.""Id"" as CommunityHallId,sp.""SpecialRate"", case when h.""Images""='[]' then null else h.""Images"" end as PhotoId
                                ,concat(w.""wrd_no"",'-',w.""wrd_name"") as WardName
                                from cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHall"" as h
                                left join public.""ward"" as w on w.""wrd_no""=h.""Ward""
                                left join cms.""F_JSC_COMMUNITY_HALL_SERVICE_SpecialRateGrid"" as sp on sp.""ParentId""=h.""Id"" and sp.""IsDeleted""=false and (sp.""StartDate""::Date<='{today}'::Date and sp.""EndDate""::Date>='{today}'::Date)
                                where h.""IsDeleted""=false #WEREHALL# #WEREWARD#
                                order by h.""Name""
                                ";
            var wherehall = "";
            var whereward = "";
            if (communityHallId.IsNotNullAndNotEmpty())
            {
                wherehall = $@" and h.""Id""='{communityHallId}' ";
            }
            if (wardId.IsNotNullAndNotEmpty())
            {
                whereward = $@" and h.""Ward""='{wardId}' ";
            }
            query = query.Replace("#WEREHALL#", wherehall);
            query = query.Replace("#WEREWARD#", whereward);
            var result = await _queryRepo.ExecuteQueryList<JSCCommunityHallViewModel>(query, null);
            return result;
        }
        public async Task<List<JSCCommunityHallViewModel>> GetCommunityHallList(string type, DateTime? st = null, DateTime? en = null, string[] dates = null)
        {

            string query = $@" select h.*,h.""Id"" as CommunityHallId, case when h.""Images""='[]' then null else h.""Images"" end as PhotoId
                                ,concat(w.""wrd_no"",'-',w.""wrd_name"") as WardName 
                                from cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHall"" as h
                                left join public.""ward"" as w on w.""wrd_no""=h.""Ward""
                                where h.""IsDeleted""=false and h.""Id"" NOT IN (
                                select ch.""Id"" as CommunityHallId --, ch.""Name"", c.""CommunityBookingFromDate""::Date, c.""CommunityBookingToDate""::Date  
                                from cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHall"" as ch
                                join cms.""N_SNC_JSC_BOOKING_PERMISSION_JSC_CommunityHallBooking"" as c on ch.""Id""=c.""CommunityHallId"" and c.""IsDeleted""=false
                                <<WHERE>> 
                                join cms.""N_SNC_JSC_BOOKING_PERMISSION_CommunityHallService"" as par on c.""ParentId""=par.""Id"" and par.""IsDeleted""=false
                                join public.""NtsService"" as s on par.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                                join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE','SERVICE_STATUS_COMPLETE')
                                where ch.""IsDeleted""=false
                                )
                                order by h.""Name""
                                ";
            string where = "";
            if (type == "DateRange")
            {
                //where = $@" and c.""CommunityBookingFromDate""::Date not in ('{st}'::Date, '{en}'::Date) 
                //or c.""CommunityBookingToDate""::Date not in ('{st}'::Date ,'{en}'::Date)";

                where = $@" and ((c.""CommunityBookingFromDate""::Date between '{st}'::Date and '{en}'::Date) 
                                or (c.""CommunityBookingToDate""::Date between '{st}'::Date and '{en}'::Date)) ";
            }
            else
            {
                var datesin = String.Join("','", dates[0].Split(",").ToArray());

                //where = $@" and c.""CommunityBookingFromDate""::Date not in ('{datesin}') 
                //        or c.""CommunityBookingToDate""::Date not in ('{datesin}')";

                where = $@" and ( c.""CommunityBookingFromDate""::Date in ('{datesin}') 
									or c.""CommunityBookingToDate""::Date in ('{datesin}')) ";
            }

            query = query.Replace("<<WHERE>>", where);

            var result = await _queryRepo.ExecuteQueryList<JSCCommunityHallViewModel>(query, null);

            DateTime today = DateTime.Today;
            string query1 = $@"select h.*, h.""Id"" as CommunityHallId, sp.""SpecialRate"", typ.""Name"" as CommunityTypeName, case when h.""Images""='[]' then null else h.""Images"" end as PhotoId
                                ,concat(w.""wrd_no"",'-',w.""wrd_name"") as WardName 
                                from cms.""F_JSC_COMMUNITY_HALL_SERVICE_CommunityHall"" as h
                                left join public.""ward"" as w on w.""wrd_no""=h.""Ward""
                                left join cms.""F_JSC_COMMUNITY_HALL_SERVICE_TypeofCommunityHall"" as typ on typ.""Id""=h.""CommunityType"" and typ.""IsDeleted""=false
                                left join cms.""F_JSC_COMMUNITY_HALL_SERVICE_SpecialRateGrid"" as sp on sp.""ParentId""=h.""Id"" and sp.""IsDeleted""=false and (sp.""StartDate""::Date<='{today}'::Date and sp.""EndDate""::Date>='{today}'::Date)
                                where h.""IsDeleted""=false
                                order by h.""Name""
                                ";
            var result1 = await _queryRepo.ExecuteQueryList<JSCCommunityHallViewModel>(query1, null);

            foreach (var item in result1)
            {
                if (result.Any(x=>x.CommunityHallId == item.CommunityHallId))
                {
                    item.IsAvailable = true;
                }
                else
                {
                    item.IsAvailable = false;
                }
            }
            return result1;
        }

        public async Task<List<BBPSRegisterViewModel>> GetBBPSRegisterList(string serviceType)
        {
            var query = $@" select p.*,case when p,""PhotoId"" is null then lv.""ImageId"" else p.""PhotoId"" end as PhotoId from cms.""N_SNC_BBPS_REGISTER"" as p 
            join public.""LOV"" as lv on lv.""Id""=p.""ServiceTypeId""
            where p.""IsDeleted"" = false and lv.""Code""='{serviceType}' ";

            var querydata = await _queryRepo.ExecuteQueryList<BBPSRegisterViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetJSCDepartmentList()
        {
            var query = $@" select ""Name"",""Id"" 
                            from cms.""F_JSC_GRIEVANCE_MGMT_Department"" 
                            where ""IsDeleted"" = false order By ""Name"" ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<IdNameViewModel>> GetJSCRevenueTypeList()
        {
            var query = $@" select ""Name"",""Id"",""Code"" 
                            from cms.""F_JSC_REV_REVENUE_TYPE"" 
                            where ""IsDeleted"" = false order By ""Name"" ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetJSCGrievanceTypeList()
        {
            var query = $@" select ""Name"",""Id"" 
                            from cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" 
                            where ""IsDeleted"" = false order By ""Name"" ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<JSCComplaintViewModel>> UpdateResolverInput(string id, string status, string documentId)
        {
            var query = $@"Update cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" set ""LastUpdatedDate""='{DateTime.Now}',
            ""LastUpdatedBy""='{_repo.UserContext.UserId}', ""GrvStatus""='{status}',""PhotoFile""='{documentId}'
            where ""Id""='{id}'";
            await _queryRepo1.ExecuteCommand(query, null);
            return new List<JSCComplaintViewModel>();
        }

        public async Task<IList<JSCComplaintViewModel>> ComplaintMarkFlag(string parentId)
        {
            //var query = $@"Update cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" set ""LastUpdatedDate""='{DateTime.Now}',
            //""LastUpdatedBy""='{_repo.UserContext.UserId}',""CreatedDate""='{DateTime.Now}',
            //""CreatedBy""='{_repo.UserContext.UserId}', ""MarkFlag""= true
            //where ""Id""='{id}'";

            var id = Guid.NewGuid().ToString();
            var query = $@"Insert into cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintFlagDetails""  (""Id"", ""LastUpdatedDate"",
            ""LastUpdatedBy"",""CreatedDate"",""CreatedBy"", ""MarkFlag"",""FlagDateTime""
             ,""ParentId"", ""IsDeleted"", ""CompanyId"", ""Status"", ""VersionNo"")
			 values
            ('{id}','{DateTime.Now}','{_userContext.UserId}', '{DateTime.Now}', '{_userContext.UserId}', true,
                '{DateTime.Now}', '{parentId}', false,'{_userContext.CompanyId}',{(int)StatusEnum.Active}, 1)";
            await _queryRepo1.ExecuteCommand(query, null);
            return new List<JSCComplaintViewModel>();
        }

        public async Task<IList<JSCComplaintViewModel>> GetFlagComplaintDetails(string parentId)
        {
            var query = $@"Select a.""Id"" as FlagId, a.""FlagDateTime"" as FlagDateTime, '{parentId}' as ComplaintId,
            a.""CreatedBy"" as CreatedBy, u.""Name"" as OwnerUserName, u.""JobTitle"" as LevelJobTitle, ur.""Code"" as LevelUserRole
            from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintFlagDetails"" as a
            left join public.""User"" as u on u.""Id"" = a.""CreatedBy"" and u.""IsDeleted""=false
            left join public.""UserRoleUser"" as uru on uru.""UserId"" = u.""Id"" and uru.""IsDeleted""=false
            left join public.""UserRole"" as ur on ur.""Id"" = uru.""UserRoleId"" and ur.""IsDeleted""=false
            where a.""ParentId""='{parentId}' and ur.""Code"" <> 'COMPLAINT_RESOLVER'";
            var list = await _queryRepo1.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return list;
        }

        public async Task<IList<JSCComplaintViewModel>> ReopenComplaint(string parentId, string documents)
        {
            var id = Guid.NewGuid().ToString();
            var query = $@"Insert into cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails""  (""Id"", ""LastUpdatedDate"",
            ""LastUpdatedBy"",""CreatedDate"",""CreatedBy"", ""IsReopen"",""ReopenDateTime""
             ,""ParentId"", ""IsDeleted"", ""CompanyId"", ""Status"", ""VersionNo"")
			 values
            ('{id}','{DateTime.Now}','{_userContext.UserId}', '{DateTime.Now}', '{_userContext.UserId}', true,
                '{DateTime.Now}', '{parentId}', false,'{_userContext.CompanyId}',{(int)StatusEnum.Active}, 1)";
            await _queryRepo1.ExecuteCommand(query, null);

            query = $@"Update cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" set ""LastUpdatedDate""='{DateTime.Now}',
            ""LastUpdatedBy""='{_repo.UserContext.UserId}',""GrvStatus""= null, ""PhotoFileAfterReopen"" = '{documents}'
            where ""Id""='{parentId}'";
            await _queryRepo1.ExecuteCommand(query, null);
            return new List<JSCComplaintViewModel>();
        }

        public async Task<IList<JSCComplaintViewModel>> GetReopenComplaintDetails(string parentId)
        {
            var query = $@"Select c.""Id"" as ReopenId, c.""ReopenDateTime"" as ReopenDateTime, '{parentId}' as ComplaintId,
            ut.""Name"" as ReopenByName, ut.""Id"" as ReopenById, ut.""JobTitle"" as ReopenByJobTitle, ut.""Email"" as ReopenByEmail
            from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails""  as c
            join public.""User"" as ut ON ut.""Id"" = c.""CreatedBy"" and ut.""IsDeleted""=false
            where ""ParentId""='{parentId}' and c.""IsDeleted""=false";
            var list = await _queryRepo1.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return list;
        }

        public async Task<List<NtsServiceCommentViewModel>> GetAllGrievanceComment(string serviceId)
        {
            var query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
                            n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
                            case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
                            null as ParentId,true as hasChildren,true as expanded, uc.""JobTitle"" as JobTitle
                                            from public.""NtsServiceComment"" as n
                            join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                            left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                             left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
                            left join public.""NtsServiceComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
                            left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                             left join public.""User"" as uc ON uc.""Id"" = n.""CommentedByUserId"" and uc.""IsDeleted""=false
                             where n.""NtsServiceId""='{serviceId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
                            or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
                             group by n.""Id"",ub.""Name"",ub.""PhotoId"",
                            n.""CommentedDate"",n.""Comment"",f.""FileName"", uc.""JobTitle"" order by n.""CommentedDate"" desc  ";
            var result = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            return result;
        }

        public async Task<List<NtsServiceCommentViewModel>> GetAllGrievanceComment1(string p)
        {
            var query = $@" with recursive cmn as(
                        select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
                        n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
                        ,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
                        n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	                        n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			                           from public.""NtsServiceComment"" as n
                        join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                        left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                         left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	                        left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                         where  n.""IsDeleted""= false and n.""ParentCommentId""='{p}'
                        and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	                        union
	
	                        select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
                        n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
                        ,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
                        n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	                        n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			                           from public.""NtsServiceComment"" as n
                        join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	                        join cmn as p on p.""Id""=n.""ParentCommentId""
                        left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                         left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	                        left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                         where  n.""IsDeleted""= false
                         and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


                        )select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
                        from cmn
                        group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
                        ,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



                        ";
            var result1 = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            return result1;
        }

        public async Task<JSCParcelViewModel> CheckIfDDNExist(string ddn)
        {
            var query = $@"select case when count(p.*) > 0 then true else false end  as IsDDNExist , p.* from public.""parcel"" as p
                            where p.""mmi_id"" = '{ddn}' group by p.""gid"" limit 1";
            var queryData = await _queryRepo.ExecuteScalar<JSCParcelViewModel>(query, null);
            return queryData;
        }

        public async Task<bool> UpdateDepartmentByOperator(string id, string departmentId, string grievanceTypeId)
        {
            var query = $@"Update cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" set ""LastUpdatedDate""='{DateTime.Now}',
            ""LastUpdatedBy""='{_repo.UserContext.UserId}',""Department""= '{departmentId}', ""GrievanceType"" = '{grievanceTypeId}'
            where ""Id""='{id}'";
            await _queryRepo1.ExecuteCommand(query, null);

            query = $@"Update cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" set ""LastUpdatedDate""='{DateTime.Now}',
            ""LastUpdatedBy""='{_repo.UserContext.UserId}',""GrvStatus""= null
            where ""Id""='{id}'";
            await _queryRepo1.ExecuteCommand(query, null);

            return true;
        }

        public async Task<bool> MarkDisposedByOperator(string id)
        {
            var disposedId = "";
            var lovDetails = await _lOVBusiness.GetSingle(x => x.Code == "GRV_DISPOSED");
            if (lovDetails.IsNotNull())
            {
                disposedId = lovDetails.Id;
            }
            var query = $@"Update cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" set ""LastUpdatedDate""='{DateTime.Now}',
            ""LastUpdatedBy""='{_repo.UserContext.UserId}',""GrvStatus""= '{disposedId}'
            where ""Id""='{id}'";
            await _queryRepo1.ExecuteCommand(query, null);
            return true;
        }

        public async Task<JSCSanitationReportViewModel> GetMSWAutoDetails(string id)
        {

            var query = $@"select distinct a.""Id"" as AutoId, a.""AutoNumber"" as AutoNumber, ac.""WardNo"" as WardNo,
                            a.""WetWasteCapacityKgs"", a.""DryWasteCapacityKgs"", a.""TransferStation"" as TransferStationId
                            from cms.""F_JSC_PROP_MGMNT_AutoMaster"" as a
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Auto"" = a.""Id""
                            left join public.""ward"" as wd on wd.""wrd_no""=ac.""WardNo""
                            where a.""Id"" = '{id}'";

            var queryData = await _queryRepo.ExecuteScalar<JSCSanitationReportViewModel>(query, null);
            if (queryData.IsNotNull())
            {

                var noOfTripsQ = $@"select count(gc.*) from cms.""F_JSC_REV_AllocationWardToCollector"" as ac	
                    join cms.""F_JSC_REV_DailyMSWCollection"" as  gc on gc.""AutoId"" = ac.""Auto"" and gc.""IsDeleted"" = false
                    where ac.""Auto""  = '{id}'
                    and gc.""Date""::date ='{DateTime.Now}'::date ";

                var noOfTrips = await _queryRepo.ExecuteScalar<long>(noOfTripsQ, null);

                queryData.NoOfTrips = noOfTrips + 1;

                var queryWorkStartTime = "";
                var workStartTime = "";
                if (queryData.NoOfTrips == 1)
                {
                    queryWorkStartTime = $@"select min(gc.""CollectionDateTime""::time)::text
                                    from cms.""F_JSC_REV_GarbageCollection"" as gc 
                                    join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" =  gc.""CollectedByUserId""
                                    join cms.""F_JSC_REV_AllocationWardToCollector"" as w on w.""Auto"" = '{id}'
                                    where  gc.""CollectionDateTime""::date ='{DateTime.Now}'::date 
                                                            group by gc.""CollectionDateTime""::date, w.""Auto""";
                    workStartTime = await _queryRepo.ExecuteScalar<string>(queryWorkStartTime, null);

                }
                else
                {

                    queryWorkStartTime = $@"select max(msw.""WorkEndTime""::time)::text as ""WorkEndDate""
                        --msw.""Date""::date as ""Date"", msw.""AutoId"" as ""AutoId""
						from cms.""F_JSC_REV_DailyMSWCollection"" as msw
						where msw.""AutoId"" = '{id}'
                        and  msw.""Date""::date ='{DateTime.Now}'::date 
                        group by msw.""Date""::date, msw.""AutoId""";
                    workStartTime = await _queryRepo.ExecuteScalar<string>(queryWorkStartTime, null);

                }

                queryData.WorkStartTime = workStartTime;
                queryData.WorkEndTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            }
            return queryData;
        }

        public async Task<List<JSCSanitationReportViewModel>> GetMSWReportDetails(string autoId, DateTime startDate, DateTime endDate, string transferStationId)
        {
            var query = $@"";

            var noOfHouseAssignedQ = $@"Select distinct Count(gcp.""ParcelId"" )
                                        from cms.""F_JSC_REV_JSC_COLLECTOR"" as c 
                                         join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = c.""Id"" and gcp.""IsDeleted"" = false
                                         join public.""LOV"" as l on l.""Id"" = gcp.""GarbageTypeId"" and l.""IsDeleted"" = false
                                         join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false
                                        where ac.""Auto"" = '{autoId}'
                                        and l.""Code"" = 'JSC_GARBAGE_RESIDENTIAL'";
            var noOfHouseAssigned = await _queryRepo.ExecuteScalar<long>(noOfHouseAssignedQ, null);


            var noOfCommercialAssignedQ = $@"Select distinct Count(gcp.""ParcelId"" )
                                            from cms.""F_JSC_REV_JSC_COLLECTOR"" as c 
                                             join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = c.""Id"" and gcp.""IsDeleted"" = false
                                             join public.""LOV"" as l on l.""Id"" = gcp.""GarbageTypeId"" and l.""IsDeleted"" = false
                                             join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false
                                            where ac.""Auto""   = '{autoId}'
                                            and l.""Code"" = 'JSC_GARBAGE_COMMERCIAL'";

            var noOfCommercialAssigned = await _queryRepo.ExecuteScalar<long>(noOfCommercialAssignedQ, null);

            query = $@"Select distinct am.""AutoNumber"" as AutoNumber, ts.""Name"" as TransferStationName,
                     c.""Name"" as CollectorName,awc.""WardNo"" as WardNo , at.""Name"" as AutoType,
                     am.""WetWasteCapacityKgs"" as WetWasteCapacityKgs, am.""DryWasteCapacityKgs"" as DryWasteCapacityKgs,
                        am.""WetWasteCapacityKgs""::decimal + am.""DryWasteCapacityKgs""::decimal as TotalWasteCapacityKgs
						,gc.""CollectionDateTime""::date as Date,
						min(gc.""CollectionDateTime""::time)::text as WorkStartTime
						,msw.""WorkEndDate""::text as WorkEndTime
                        from cms.""F_JSC_PROP_MGMNT_AutoMaster"" as am 
                        join cms.""F_JSC_REV_TransferStation"" as ts on ts.""Id"" = am.""TransferStation""
                        join cms.""F_JSC_REV_AllocationWardToCollector"" as awc on awc.""Auto"" = am.""Id""
                        join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = awc.""Collector""
						join cms.""F_JSC_REV_GarbageCollection"" as gc on gc.""CollectedByUserId"" = c.""UserId"" and gc.""IsDeleted"" = false
                        join cms.""F_TypeMasters_AutoTypeMaster"" as at on at.""Id"" = am.""AutoTypeId""
						left join (select max(msw.""WorkEndTime""::time)::text as ""WorkEndDate"",msw.""Date""::date as ""Date"", msw.""AutoId"" as ""AutoId""
						from cms.""F_JSC_REV_DailyMSWCollection"" as msw
						where msw.""AutoId"" = '{autoId}' group by msw.""Date""::date, msw.""AutoId"") as msw
						on msw.""AutoId"" = am.""Id"" and gc.""CollectionDateTime""::date = msw.""Date""::date
                        where am.""Id"" = '{autoId}'
                        and am.""IsDeleted"" = false 
                        and gc.""CollectionDateTime""::date >= '{startDate}'::date
                        and gc.""CollectionDateTime""::date <= '{endDate}'::date
						Group By am.""AutoNumber"" ,ts.""Name"",
						am.""WetWasteCapacityKgs"",am.""DryWasteCapacityKgs"",gc.""CollectionDateTime""::date,
						msw.""WorkEndDate""::time, c.""Name"",awc.""WardNo"",at.""Name"",msw.""WorkEndDate""
"
                        ;
            var queryData = await _queryRepo.ExecuteQueryList<JSCSanitationReportViewModel>(query, null);


            foreach (var report in queryData)
            {
                var res = $@"select   sum(msw.""WetWaste""::int) as ""WetWaste"", sum(msw.""DryWaste""::int) as ""DryWaste"", 
                               sum( msw.""DryWaste1""::int) as ""DryWaste1"",sum(msw.""WetWaste1""::int) as ""WetWaste1""
                        , string_agg(  msw.""Remarks"", ',') as Remarks, Sum(msw.""NoOfHousesFromWhere70OrAboveSegregatedWasteReceived""::int) as NoOfHousesFromWhere70OrAboveSegregatedWasteReceived
						from cms.""F_JSC_REV_DailyMSWCollection"" as msw
						where msw.""AutoId"" = '{autoId}' 
						and msw.""Date""::date = '{report.Date}'::date";
                var resData = await _queryRepo.ExecuteScalar<JSCSanitationReportViewModel>(res, null);

                var noOfCommercialUnitCoveredQ = $@"select count(gc.*) from cms.""F_JSC_REV_AllocationWardToCollector"" as ac	
                    join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = ac.""Collector"" and ac.""IsDeleted"" = false
                    join cms.""F_JSC_REV_GarbageCollection"" as  gc on gc.""CollectedByUserId"" = c.""UserId""
                    join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = gc.""ParcelId"" and gcp.""IsDeleted"" = false
                    join public.""LOV"" as l on l.""Id"" = gcp.""GarbageTypeId"" and l.""IsDeleted"" = false
                    where ac.""Auto""  = '{autoId}' and  l.""Code"" = 'JSC_GARBAGE_COMMERCIAL'
                    and gc.""CollectionDateTime""::date ='{report.Date}'::date ";
                var noOfCommercialUnitCovered = await _queryRepo.ExecuteScalar<long>(noOfCommercialUnitCoveredQ, null);


                var noOfHouseHoldUnitCoveredQ = $@"select count(gc.*) from cms.""F_JSC_REV_AllocationWardToCollector"" as ac	
                    join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = ac.""Collector"" and ac.""IsDeleted"" = false
                    join cms.""F_JSC_REV_GarbageCollection"" as  gc on gc.""CollectedByUserId"" = c.""UserId""
                    join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = gc.""ParcelId"" and gcp.""IsDeleted"" = false
                    join public.""LOV"" as l on l.""Id"" = gcp.""GarbageTypeId"" and l.""IsDeleted"" = false
                    where ac.""Auto""  = '{autoId}' and  l.""Code"" = 'JSC_GARBAGE_RESIDENTIAL'
                    and gc.""CollectionDateTime""::date ='{report.Date}'::date ";

                var noOfHouseHoldUnitCovered = await _queryRepo.ExecuteScalar<long>(noOfHouseHoldUnitCoveredQ, null);


                var noOfTripsQ = $@"select count(gc.*) from cms.""F_JSC_REV_AllocationWardToCollector"" as ac	
                    join cms.""F_JSC_REV_DailyMSWCollection"" as  gc on gc.""AutoId"" = ac.""Auto"" and gc.""IsDeleted"" = false
                    where ac.""Auto""  = '{autoId}'
                    and gc.""Date""::date ='{report.Date}'::date ";

                var noOfTrips = await _queryRepo.ExecuteScalar<long>(noOfTripsQ, null);
                report.Date = report.Date.ToSafeDateTime().ToString("dd-MM-yyyy");
                report.NoOfHouseHold = noOfHouseAssigned;
                report.NoOfCommercial = noOfCommercialAssigned;
                report.NoOfHouseHoldCovered = noOfHouseHoldUnitCovered;
                report.NoOfCommercialCovered = noOfCommercialUnitCovered;
                report.WetWaste = resData.IsNotNull() ? resData.WetWaste : "0";
                report.WetWaste1 = resData.IsNotNull() ? resData.WetWaste1 : "0";
                report.DryWaste = resData.IsNotNull() ? resData.DryWaste : "0";
                report.DryWaste1 = resData.IsNotNull() ? resData.DryWaste1 : "0";
                report.Remarks = resData.IsNotNull() ? resData.Remarks : "";
                report.NoOfTrips = noOfTrips;
                report.WorkStartTime = report.WorkStartTime.Split(".")[0];
                report.NoOfHousesFromWhere70OrAboveSegregatedWasteReceived = resData.IsNotNull() ? resData.NoOfHousesFromWhere70OrAboveSegregatedWasteReceived : "";

            }
            return queryData;
        }
        public async Task<JSCSanitationReportViewModel> GetBWGAutoDetails(string id)
        {
            var query = $@"select distinct a.""Id"" as AutoId, a.""AutoNumber"" as AutoNumber, ac.""Date"",ac.""nameOfTheRouteCoordinator""
                            from cms.""F_JSC_PROP_MGMNT_AutoMaster"" as a
           left join cms.""F_JSC_REV_BWGsWasteCollectionFormat"" as ac on ac.""AutoId"" = a.""Id""
                            where a.""Id"" = '{id}'";
            var queryData = await _queryRepo.ExecuteScalar<JSCSanitationReportViewModel>(query, null);
            return queryData;
        }
        public async Task<List<JSCSanitationReportViewModel>> GetBWGReportDetails(string autoId, DateTime startDate, DateTime endDate, string transferStationId)
        {
            var startDates = startDate.ToString("yyyy-MM-dd");
            var endDates = endDate.ToString("yyyy-MM-dd");
            var query = $@"select  (row_number() OVER (ORDER BY c.""Id"") - 1) % 4 + 1 as SlNo,
                            c.""NameOfBulkWasteGeneratorBwg"" as NameOfBulkWasteGenerator, c.""ArrivalTime"" as ArrivalTime, c.""DepartureTime"" as DepartureTime
                            ,c.""WetWasteInKgs"" as WetWaste, c.""DryWaste"" as DryWaste, c.""SignatureOfBwGsRepresentative"" as SignatureOfBWGRepresentative
                            ,c.""Date"" AS Date,c.""nameOfTheRouteCoordinator"" AS nameOfTheRouteCoordinator
                            from  cms.""F_JSC_REV_BWGsWasteCollectionFormat"" as c
                            where c.""AutoId"" = '{autoId}' and 
                            c.""Date"":: date >= '{startDates}'::date 
                            and c.""Date"":: date <= '{endDates}'::date ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCSanitationReportViewModel>(query, null);
            return queryData;
        }
        public async Task<List<JSCSanitationReportViewModel>> GetBWGDetailsByUserId(string userId)
        {
            var query = $@"SELECT * FROM cms.""F_JSC_REV_BWGsWasteCollectionFormat"" as bwg
                            where bwg.""CreatedBy"" = '{userId}'
                            and bwg.""Date"":: date = '{DateTime.Today.ToDatabaseDateFormat()}'::date
                            ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCSanitationReportViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetJSCAutoList()
        {
            var query = $@"SELECT distinct a.""AutoNumber""
                             as Name, a.""Id"" as Id
                            FROM cms.""F_JSC_PROP_MGMNT_AutoMaster""  as a where a.""IsDeleted"" = false";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetJSCAutoListByTransferStation(string transferStationId)
        {
            var query = $@"select a.""AutoNumber"" as Name, a.""Id"" as Id  from cms.""F_JSC_PROP_MGMNT_AutoMaster"" AS a
                            where a.""TransferStation"" = '{transferStationId}' and a.""IsDeleted"" = false 
                            Group By a.""AutoNumber"",a.""Id""";
            if (_userContext.IsSystemAdmin)
            {

                query = $@"select a.""AutoNumber"" as Name, a.""Id"" as Id  from cms.""F_JSC_PROP_MGMNT_AutoMaster"" AS a
                            where a.""IsDeleted"" = false 
                            Group By a.""AutoNumber"",a.""Id""";
            }

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCAutoViewModel>> GetAutoListByUserId(string userId)
        {
            var query = $@"Select am.*,c.""Id"" as CollectorId, c.""Name"" as CollectorName, ac.""WardNo"" as WardNo, 
ac.""Auto"" as AutoId, wcf.""nameOfTheRouteCoordinator"" as RouteCoordinatorName
from public.""User"" as u
join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on u.""Id""=c.""UserId"" and c.""IsDeleted""=false
join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false
join cms.""F_JSC_PROP_MGMNT_AutoMaster"" as am on am.""Id"" = ac.""Auto"" and am.""IsDeleted"" = false
join cms.""F_TypeOfUsers_DriverMaster"" as dm on am.""DriverId""=dm.""Id"" and dm.""IsDeleted""=false
join cms.""F_JSC_REV_TransferStation"" as ts on am.""TransferStation""=ts.""Id"" and ts.""IsDeleted""=false
left join cms.""F_JSC_REV_BWGsWasteCollectionFormat"" as wcf on am.""Id""=wcf.""AutoId"" and wcf.""IsDeleted""=false
where u.""Id""= '{userId}' and u.""IsDeleted""=false ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCAutoViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCComplaintViewModel>> GetComplaintByWardAndDepartmentWithStatus()
        {
            var query = $@"SELECT l.""Ward"" as Ward, l.""Department"" as DepartmentId,d.""Name"" as Department,
                            string_agg(case when ls.""Code"" is null then 'GRV_PENDING' else ls.""Code"" end, ',') AS StatusList
                             from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                            Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = l.""Department""
                            left join public.""LOV"" as ls on ls.""Id""=l.""GrvStatus"" and ls.""IsDeleted""=false      
                            Group by l.""Ward"", l.""Department""	, d.""Name""					";

            var querydata = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportDeptTurnaroundTimeData(string typeCode, string departmentId, string wardId, DateTime fromDate, DateTime toDate)
        {
            var query = $@"select distinct  replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending"",
                replace((s.""CompletedDate""::date - s.""CreatedDate""::date)::varchar ,'-', '')::int as ""NoOfDaysDisposed"",
                s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                ,u.""Name"" as OwnerUserName, ul.""Name"" as Level1User, ul.""Id"" as Level1UserId
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
				left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
                left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and (ls.""Code"" = 'GRV_DISPOSED') #WHEREDEPT# #WHEREWARD#
                and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date)
				order by ""NoOfDaysPending"" desc";
            var wheredept = "";
            if (departmentId.IsNotNullAndNotEmpty())
            {
                wheredept = $@" and lc.""Department""='{departmentId}' ";
            }
            var whereward = "";
            if (wardId.IsNotNullAndNotEmpty())
            {
                whereward = $@" and lc.""Ward""='{wardId}' ";
            }
            query = query.Replace("#WHEREDEPT#", wheredept);
            query = query.Replace("#WHEREWARD#", whereward);
            var querydata = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportAgingData(string typeCode, DateTime fromDate, DateTime toDate)
        {
            var query = $@"select distinct  replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending"",
                s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                ,u.""Name"" as OwnerUserName, ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
				left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
                left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and (ls.""Code"" = 'GRV_PENDING' or ls.""Code"" is null) 
				order by ""NoOfDaysPending"" desc";

            var querydata = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportDepartmentWiseData(string typeCode, DateTime fromDate, DateTime toDate)
        {
            var query = "";



            if (typeCode == "DEPARTMENT_WISE")
            {
                query = $@"SELECT d.""Id"" as DepartmentId, d.""Name"" as Department, Count(l.*) as ComplaintCount
                     from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                    Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = l.""Department""
                             where l.""IsDeleted"" = false and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                    Group by d.""Id"",d.""Name""	
                    ";
            }
            else if (typeCode == "DEPARTMENT_STATUS_WISE")
            {
                query = $@"SELECT d.""Id"" as DepartmentId, d.""Name"" as Department, string_agg(case when ls.""Code"" is null then 'GRV_PENDING' else ls.""Code"" end, ',') AS StatusList
                     from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                    Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = l.""Department""
                    left join public.""LOV"" as ls on ls.""Id""=l.""GrvStatus"" and ls.""IsDeleted""=false
                    where l.""IsDeleted"" = false and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                    Group by d.""Id"",d.""Name""	
                    ";
            }
            else if (typeCode == "WARD_WISE")
            {
                query = $@"SELECT l.""Ward"" as Ward, Count(l.*) as ComplaintCount
                             from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                             --join public.""ward"" as w on w.""wrd_no"" = l.""Ward""
                             where l.""IsDeleted"" = false and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                            Group by  l.""Ward"" order by l.""Ward"" ";
            }
            else if (typeCode == "WARD_STATUS_WISE")
            {
                query = $@"SELECT l.""Ward"" as Ward,  string_agg(case when ls.""Code"" is null then 'GRV_PENDING' else ls.""Code"" end, ',') AS StatusList
                             from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                             --join public.""ward"" as w on w.""wrd_no"" = l.""Ward""
                            left join public.""LOV"" as ls on ls.""Id""=l.""GrvStatus"" and ls.""IsDeleted""=false
                             where l.""IsDeleted"" = false and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                            Group by  l.""Ward"" order by l.""Ward"" ";
            }
            else if (typeCode == "ZONE_WISE")
            {
                query = $@"select z.""Id"" as ZoneId,z.""Name"" as ZoneName,z.""DepartmentId"" as DepartmentId,d.""Name"" as Department, z.""Ward"" as WardIds 
                                    from cms.""F_JSC_GRIEVANCE_MGMT_Zone"" as z
                                    join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id""=z.""DepartmentId"" and d.""IsDeleted""=false
                                    where z.""IsDeleted""=false 
                                    order by z.""Name"" ";

            }
            else if (typeCode == "COMPLAINTTYPE_WISE")
            {
                query = $@"SELECT d.""Id"" as GrievanceTypeId, d.""Name"" as GrievanceType, Count(l.*) as ComplaintCount
                         from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                        Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as d on d.""Id"" = l.""GrievanceType""
                             where l.""IsDeleted"" = false and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                        Group by d.""Id"", d.""Name""	";
            }
            else if (typeCode == "STATUS_WISE")
            {
                query = $@"SELECT lv.""Id"" as GrvStatusId,lv.""Name"" as GrvStatus, Count(l.*) as ComplaintCount
                             from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
							 join public.""LOV"" as lv on lv.""Id"" = l.""GrvStatus""
                             where l.""IsDeleted"" = false and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                            Group by lv.""Id"", lv.""Name"" ";
            }
            else if (typeCode == "EMPLOYEE_WISE")
            {
                query = $@"SELECT d.""Name"" as Department, Count(l.*) as ComplaintCount
                     from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                    Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = l.""Department""
                    Group by d.""Name""	
                    ";
            }

            var querydata = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            if (querydata != null)
            {
                if (typeCode == "STATUS_WISE")
                {
                    var query1 = $@"SELECT Count(l.*) as ComplaintCount
                             from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                             where l.""IsDeleted"" = false and (l.""GrvStatus"" is null) and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                                    ";
                    long stcount = await _queryRepo.ExecuteScalar<long>(query1, null);
                    if (stcount > 0)
                    {
                        foreach (var i in querydata)
                        {
                            if (i.GrvStatus == "Pending")
                            {
                                i.ComplaintCount = i.ComplaintCount + stcount;
                            }
                        }
                    }
                }
                else if (typeCode == "ZONE_WISE")
                {
                    foreach (var item in querydata)
                    {
                        var ward = item.WardIds.Replace("[", "").Replace("]", "");
                        var wards = ward.Split(",");
                        foreach (var w in wards)
                        {
                            var wrd = w.Trim();
                            if (wrd.IsNotNullAndNotEmpty())
                            {
                                var query1 = $@"SELECT l.""Ward"" as Ward,  string_agg(case when ls.""Code"" is null then 'GRV_PENDING' else ls.""Code"" end, ',') AS StatusList
                             from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
                             --join public.""ward"" as w on w.""wrd_no"" = l.""Ward""
                            left join public.""LOV"" as ls on ls.""Id""=l.""GrvStatus"" and ls.""IsDeleted""=false
                             where l.""IsDeleted"" = false and l.""Ward""='{wrd}' and (l.""CreatedDate""::Date>='{fromDate}'::Date and l.""CreatedDate""::Date<='{toDate}'::Date)
                            Group by  l.""Ward"" order by l.""Ward"" ";
                                var querydata1 = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query1, null);
                                if (querydata1.Count > 0)
                                {
                                    item.StatusList = item.StatusList + "," + querydata1.FirstOrDefault().StatusList;
                                    item.StatusList = item.StatusList.Trim(',');
                                }
                            }
                        }
                    }


                }
            }
            return querydata;
        }
        public async Task<List<JSCComplaintViewModel>> GetComplaintZoneStatusData(string zone, string status, DateTime fromDate, DateTime toDate)
        {
            var ward = "";
            var queryzone = $@"select z.""Id"" as ZoneId,z.""Name"" as ZoneName,z.""DepartmentId"" as DepartmentId,d.""Name"" as Department, z.""Ward"" as WardIds 
                                    from cms.""F_JSC_GRIEVANCE_MGMT_Zone"" as z
                                  left join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id""=z.""DepartmentId"" and d.""IsDeleted""=false
                                    where z.""IsDeleted""=false and z.""Name""='{zone}' 
                                    order by z.""Name"" ";
            var querydatazone = await _queryRepo.ExecuteQuerySingle<JSCComplaintViewModel>(queryzone, null);
            if (querydatazone.IsNotNull())
            {
                var wards = querydatazone.WardIds.Replace("[", "").Replace("]", "");
                var wardlist = wards.Split(",");
                foreach (var w in wardlist)
                {
                    var wrd = w.Trim();
                    if (wrd.IsNotNullAndNotEmpty())
                    {
                        ward = ward + $@"'{wrd}',";
                    }
                }
                if (ward.IsNotNullAndNotEmpty())
                {
                    ward = ward.Trim(',');
                }
            }

            var query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date) #WHEREWARD# #WHEREDEPT# #WHERESTATUS# order by s.""CreatedDate"" ";

            var whereward = "";
            if (ward.IsNotNullAndNotEmpty())
            {
                whereward = $@" and lc.""Ward"" IN ({ward}) ";
            }
            var wheredept = "";
            //if (dpt.IsNotNullAndNotEmpty())
            //{
            //    wheredept = $@" and d.""Name""='{dpt}' ";
            //}
            var wherestatus = $@" and ls.""Name""='{status}' ";
            if (status == "Pending")
            {
                wherestatus = $@" and (ls.""Name""='{status}' or ls.""Name"" is null) ";
            }
            query = query.Replace("#WHEREWARD#", whereward);
            query = query.Replace("#WHEREDEPT#", wheredept);
            query = query.Replace("#WHERESTATUS#", wherestatus);
            var querydata = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCComplaintViewModel>> GetComplaintWardDepartmentStatusData(string warddept, string status, DateTime fromDate, DateTime toDate)
        {
            var ward = warddept.Split("-")[0];
            var dpt = warddept.Split("-")[1];
            var query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date) #WHEREWARD# #WHEREDEPT# #WHERESTATUS# order by s.""CreatedDate"" ";

            var whereward = "";
            if (ward.IsNotNullAndNotEmpty())
            {
                whereward = $@" and lc.""Ward""='{ward}' ";
            }
            var wheredept = "";
            if (dpt.IsNotNullAndNotEmpty())
            {
                wheredept = $@" and d.""Name""='{dpt}' ";
            }
            var wherestatus = $@" and ls.""Name""='{status}' ";
            if (status == "Pending")
            {
                wherestatus = $@" and (ls.""Name""='{status}' or ls.""Name"" is null) ";
            }
            query = query.Replace("#WHEREWARD#", whereward);
            query = query.Replace("#WHEREDEPT#", wheredept);
            query = query.Replace("#WHERESTATUS#", wherestatus);
            var querydata = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return querydata;
        }
        public async Task<List<JSCComplaintViewModel>> GetComplaintByWardAndDepartmentWithStatusDetails(string department, string status)
        {
            var ward = department.Split("-")[0];
            var dpt = department.Split("-")[1];
            var query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and lc.""Ward"" = '{ward}' and d.""Name"" = '{dpt}' #WHERESTATUS# order by s.""CreatedDate"" ";

            var wherestatus = $@" and ls.""Name"" = '{status}' ";
            if (status == "Pending")
            {
                wherestatus = $@" and (ls.""Name"" = '{status}' or ls.""Name"" is null) ";
            }
            query = query.Replace("#WHERESTATUS#", wherestatus);
            var querydata = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return querydata;
        }


        public async Task<List<JSCComplaintViewModel>> GetComplaintReportData(string name, string reportType, DateTime fromDate, DateTime toDate)
        {
            var query = "";
            if (reportType == "DEPARTMENT_WISE")
            {

                query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                 left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false  and d.""Name"" = '{name}' and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date)  order by s.""CreatedDate"" ";

            }
            else if (reportType == "WARD_WISE")
            {
                var ward = name.Split("-")[0];

                query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and lc.""Ward"" = '{ward}' and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date)  order by s.""CreatedDate"" ";

            }
            else if (reportType == "COMPLAINTTYPE_WISE")
            {
                query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false and gt.""Name"" = '{name}' and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date) order by s.""CreatedDate"" ";

            }
            else if (reportType == "STATUS_WISE")
            {
                query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false #WHERESTATUS#  and (s.""CreatedDate""::Date>='{fromDate}'::Date and s.""CreatedDate""::Date<='{toDate}'::Date) order by s.""CreatedDate"" ";

                var wherestatus = $@" and ls.""Name"" = '{name}' ";
                if (name == "Pending")
                {
                    wherestatus = $@" and (ls.""Name"" = '{name}' or ls.""Name"" is null) ";
                }
                query = query.Replace("#WHERESTATUS#", wherestatus);

            }
            else if (reportType == "EMPLOYEE_WISE")
            {
                query = $@"select distinct s.""Id"", s.""ServiceNo"" as ServiceNo, d.""Name"" as Department, gt.""Name"" as GrievanceType,
                s.""CreatedDate"", ls.""Name"" as TaskStatusName, lc.""Ward"" as Ward, 
                lc.""Name"" as Name, lc.""Details"" as Details, lc.""Option"" as Option, lc.""DocumentId"" as DocumentId,
                lc.""Address"" as Address,lc.""DDN"" as DDN, lc.""MapArea"" as MapArea,lc.""Latitude"" as Latitude,lc.""Longitude"" as Longitude,
                ls.""Name"" as GrvStatus, lc.""Id"" as ComplaintId, lc.""GrvStatus"" as GrvStatusId, lc.""Department"" as DepartmentId, lc.""MarkFlag"" as IsFlag
                ,cnt.reopenCount as ReopenCount, ls.""Code"" as GrvStatusCode
                , u.""Name"" as OwnerUserName, replace((s.""CreatedDate""::date - NOW()::date)::varchar ,'-', '')::int as ""NoOfDaysPending""
                , ul.""Name"" as Level1User
                from public.""NtsService"" as s
				 Join cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as lc on lc.""Id"" = s.""UdfNoteTableId""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_Department"" as d on d.""Id"" = lc.""Department""
				 Join cms.""F_JSC_GRIEVANCE_MGMT_GrievanceType"" as gt on gt.""Id"" = lc.""GrievanceType""
                left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
				left join cms.""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"" as gw on gw.""DepartmentId""=lc.""Department"" and gw.""IsDeleted""=false
				left join public.""User"" as ul on ul.""Id""=gw.""Level1AssignedToUserId"" and ul.""IsDeleted""=false
                left join public.""LOV"" as ls on ls.""Id""=lc.""GrvStatus"" and ls.""IsDeleted""=false       
               left  JOIN (
                   select count(""Id"") as reopenCount, ""ParentId""
                   from cms.""N_SNC_GRIEVANCE_SERVICE_ComplaintReopenDetails"" e2 
                  group by ""ParentId""
                   ) as cnt on cnt.""ParentId"" =     lc.""Id""  
                where s.""IsDeleted""=false order by s.""CreatedDate"" ";

            }


            var querydata = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCCollectorViewModel>> GetCollectorWithWardByCollectorId(string collectorId)
        {
            string query = $@"Select * from cms.""F_JSC_REV_AllocationWardToCollector"" where ""Collector""='{collectorId}' and ""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCCollectorViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetCollectorListByWard(string wardId)
        {
            string query = $@"
                        select c.""Name"" as Name, c.""UserId"" as Id from cms.""F_JSC_REV_AllocationWardToCollector"" as ac
                        join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = ac.""Collector""
                        #ward#";
            if (wardId.IsNotNull())
            {
                var wardWhere = $@"where ac.""WardNo"" = '{wardId}'";
                query = query.Replace("#ward#", wardWhere);
            }
            else
            {
                query = query.Replace("#ward#", "");
            }

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectedAndNotCollectedList()
        {
            var query = $@"select distinct p.""mmi_id"" as mmi_id,
                        case when col.""CollectedDate""=date('{DateTime.Today}') then true else false end ""IsGarbageCollected""
                        FROM public.""parcel"" as p 
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = ac.""Collector"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
                         
                        union
                        select  p.""mmi_id"" as mmi_id,                        
                        case when col.""CollectedDate""=date('{DateTime.Today}') then true else false end ""IsGarbageCollected""
                        from cms.""N_SNC_SANITATION_SERVICE_BIN_BOOKING_JSC"" b
                        join public.""NtsService"" s on b.""Id""=s.""UdfNoteTableId""	 and s.""IsDeleted""=false  
			            Join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false 
			            join public.""parcel"" p on p.""mmi_id""::text=b.""ParcelId""
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = ac.""Collector"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
			            where b.""IsDeleted""=false 
                         ";
            var data = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectedAndNotCollectedListByWard(string wardId)
        {
            var query = $@"select distinct p.""mmi_id"" as mmi_id,
                        case when col.""CollectedDate""=date('{DateTime.Today}') then true else false end ""IsGarbageCollected""
                        FROM public.""parcel"" as p 
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = ac.""Collector"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
                         where p.""ward_no"" ='{wardId}'
                        union
                        select  p.""mmi_id"" as mmi_id,                        
                        case when col.""CollectedDate""=date('{DateTime.Today}') then true else false end ""IsGarbageCollected""
                        from cms.""N_SNC_SANITATION_SERVICE_BIN_BOOKING_JSC"" b
                        join public.""NtsService"" s on b.""Id""=s.""UdfNoteTableId""	 and s.""IsDeleted""=false  
			            Join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false 
			            join public.""parcel"" p on p.""mmi_id""::text=b.""ParcelId""
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = ac.""Collector"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
			            where b.""IsDeleted""=false  and 
                          p.""ward_no"" ='{wardId}'
                         ";
            var data = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByPropertyType()
        {
            var query = $@"select distinct p.""mmi_id"" as mmi_id,col.""GarbageTypeName"",
                        case when col.""CollectedDate""=date('{DateTime.Today}') then true else false end ""IsGarbageCollected""
                        FROM public.""parcel"" as p 
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = ac.""Collector"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
                        union
                        select  p.""mmi_id"" as mmi_id,            col.""GarbageTypeName"",            
                        case when col.""CollectedDate""=date('{DateTime.Today}') then true else false end ""IsGarbageCollected""
                        from cms.""N_SNC_SANITATION_SERVICE_BIN_BOOKING_JSC"" b
                        join public.""NtsService"" s on b.""Id""=s.""UdfNoteTableId""	 and s.""IsDeleted""=false  
			            Join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false 
			            join public.""parcel"" p on p.""mmi_id""::text=b.""ParcelId""
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" from cms.""F_JSC_REV_GarbageCollection"" as g
							left join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""UserId"" = g.""CollectedByUserId"" and c.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""CollectorId"" = ac.""Collector"" and gcp.""IsDeleted"" = false
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
			            where b.""IsDeleted""=false  
                         ";
            var data = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByPropertyTypeAndWard(string wardId)
        {
            var query = $@"select distinct p.""mmi_id"" as mmi_id,col.""GarbageTypeName"",p.""ward_no"" ,
                        case when col.""CollectedDate""=date('{DateTime.Today}') then true else false end ""IsGarbageCollected""
                        FROM public.""parcel"" as p 
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" 
							from  public.""parcel"" as p 
							left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = p.""mmi_id"" and gcp.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = gcp.""CollectorId"" and c.""IsDeleted"" = false
							left join cms.""F_JSC_REV_GarbageCollection"" as g on g.""ParcelId"" = p.""mmi_id""
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
                        where p.""ward_no"" = '{wardId}'
                        union
                        select  p.""mmi_id"" as mmi_id,            col.""GarbageTypeName"",  p.""ward_no"" ,          
                        case when col.""CollectedDate""=date('{DateTime.Today}') then true else false end ""IsGarbageCollected""
                        from cms.""N_SNC_SANITATION_SERVICE_BIN_BOOKING_JSC"" b
                        join public.""NtsService"" s on b.""Id""=s.""UdfNoteTableId""	 and s.""IsDeleted""=false  
			            Join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false 
			            join public.""parcel"" p on p.""mmi_id""::text=b.""ParcelId""
                        JOIN public.""ward"" as w on w.""wrd_no"" = p.""ward_no"" 
                        left join
						(
							select g.""ParcelId"",date(max(g.""CollectionDateTime"")) ""CollectedDate"",g.""CollectedByUserId"", lp.""Name"" as ""GarbageTypeName"" 
							from  public.""parcel"" as p 
							left join cms.""F_JSC_REV_GarbageCollectorProperty"" as gcp on gcp.""ParcelId"" = p.""mmi_id"" and gcp.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = gcp.""CollectorId"" and c.""IsDeleted"" = false
							left join cms.""F_JSC_REV_GarbageCollection"" as g on g.""ParcelId"" = p.""mmi_id""
                            left join cms.""F_JSC_REV_AllocationWardToCollector"" as ac on ac.""Collector"" = c.""Id"" and ac.""IsDeleted"" = false                    
                            left join public.""LOV"" as lp on lp.""Id"" = gcp.""GarbageTypeId""
							where g.""IsDeleted"" = false group by g.""ParcelId"",g.""CollectedByUserId"", lp.""Name"" 
						) col on p.""mmi_id""::text = col.""ParcelId""
			            where b.""IsDeleted""=false  and p.""ward_no"" = '{wardId}'
                         ";
            var data = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }
        public async Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgs()
        {
            var query = $@"select sum(COALESCE(""WetWaste""::int,0) + COALESCE(""WetWaste1""::int,0)) as WetWasteInKgs ,
                            sum(COALESCE(""DryWaste""::int,0) + COALESCE(""DryWaste1""::int,0)) as DryWasteInKgs
                            from cms.""F_JSC_REV_DailyMSWCollection""
        no                    where ""Date""::date = '{DateTime.Today}'::date";
            var data = await _queryRepo.ExecuteScalar<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }
        public async Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByWard(string wardId)
        {
            var query = $@"select sum(COALESCE(""WetWaste""::int,0) + COALESCE(""WetWaste1""::int,0)) as WetWasteInKgs ,
                        sum(COALESCE(""DryWaste""::int,0) + COALESCE(""DryWaste1""::int,0)) as DryWasteInKgs 
                        from cms.""F_JSC_REV_DailyMSWCollection""
                        where ""Date""::date = '{DateTime.Today}'::date and ""WardNo"" = '{wardId}'";
            var data = await _queryRepo.ExecuteScalar<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }
        public async Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByPropertyType()
        {
            var query = $@"select sum(COALESCE(""WetWaste""::int,0) + COALESCE(""DryWaste""::int,0)) as WasteResidential ,
                        sum(COALESCE(""DryWaste1""::int,0) + COALESCE(""WetWaste1""::int,0)) as WasteCommercial 
                        from cms.""F_JSC_REV_DailyMSWCollection""
                        where ""Date""::date = '{DateTime.Today}'::date";
            var data = await _queryRepo.ExecuteScalar<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }

        public async Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByPropertyTypeByWard(string wardId)
        {
            var query = $@"select sum(COALESCE(""WetWaste""::int,0) + COALESCE(""DryWaste""::int,0)) as WasteResidential ,
                        sum(COALESCE(""DryWaste1""::int,0) + COALESCE(""WetWaste1""::int,0)) as WasteCommercial 
                        from cms.""F_JSC_REV_DailyMSWCollection""
                        where ""Date""::date = '{DateTime.Today}'::date  and ""WardNo"" = '{wardId}'";
            var data = await _queryRepo.ExecuteScalar<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }


        public async Task<List<JSCPropertyRegistrationViewModel>> GetPropertyRegistrationStatusWise()
        {

            var query = $@"select row_number() over (ORDER BY Pr.""Id"") as SrNo, Pr.""NewHoldingNumber"" as CustomerId, Pr.""HouseNumberName"" as PropertyName,Pr.""OwnerName"" as CustomerName,
									Pr.""Address"" as CustomerAddress, Pr.""CreatedDate"" as CreatedDateText, Pr.""Status"" as GrvStatus
								from cms.""N_SNC_JSC_PROPERTY_TAX_Property_Registration"" as Pr 
								order by Pr.""Id"";";


            var querydata = await _queryRepo.ExecuteQueryList<JSCPropertyRegistrationViewModel>(query, null);
            return querydata;
        }

        public async Task<bool> GetGarbageCollectionCompaintCountandUpdate(string ddn, DateTime collectionDate)
        {
            var startDate = collectionDate.AddDays(-2);

            var query = $@"select gc.* --case when gc.""ComplaintCount"" is null then '0' else gc.""ComplaintCount"" end as ComplaintCount
                                    from cms.""F_JSC_REV_CitizenGarbageCollectionComplaint"" as gc 
                                    where gc.""DDN"" = '{ddn}' and (gc.""Date""::date >= '{startDate.ToDatabaseDateFormat()}'::date and gc.""Date""::date <= '{collectionDate.ToDatabaseDateFormat()}'::date) ";

            var list = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            var isExist = list.IsNotNull() && list.Where(x => x.CreatedDate.Date == DateTime.Today.ToString("yyyy-dd-MM").ToSafeDateTime()).Any();
            var complaintLodgeQ = $@"SELECT lv.""Id"" as GrvStatusId,lv.""Code"" as GrvStatusCode, l.*
                             from cms.""N_SNC_GRIEVANCE_SERVICE_LodgeComplaint"" as l
							 left join public.""LOV"" as lv on lv.""Id"" = l.""GrvStatus""
                             where l.""IsDeleted"" = false and (l.""CreatedDate""::Date='{DateTime.Now.AddDays(-1).ToDatabaseDateFormat()}'::Date or
                            l.""CreatedDate""::Date='{DateTime.Now.ToDatabaseDateFormat()}'::Date) and  l.""DDN"" = '{ddn}' limit 1";

            var complaint = await _queryRepo.ExecuteScalar<JSCComplaintViewModel>(complaintLodgeQ, null);
            if (complaint.IsNotNull())
            {
                if (list.Count <= 3 && !isExist && complaint.GrvStatusCode == "GRV_DISPOSED")
                {
                    var id = Guid.NewGuid().ToString();
                    var queryI = $@"Insert into cms.""F_JSC_REV_CitizenGarbageCollectionComplaint""  (""Id"", ""LastUpdatedDate"",
                        ""LastUpdatedBy"",""CreatedDate"",""CreatedBy"", ""DDN"",""Date"", ""IsDeleted"", ""CompanyId"", ""Status"", ""VersionNo"")
			             values
                        ('{id}','{DateTime.Now.ToDatabaseDateFormat()}','{_userContext.UserId}', '{DateTime.Now.ToDatabaseDateFormat()}', '{_userContext.UserId}', '{ddn}',
                            '{DateTime.Now.ToDatabaseDateFormat()}', false,'{_userContext.CompanyId}',{(int)StatusEnum.Active}, 1)";
                    await _queryRepo1.ExecuteCommand(queryI, null);
                }
            }
            else if (list.Count < 3 && !isExist)
            {
                var id = Guid.NewGuid().ToString();
                var queryI = $@"Insert into cms.""F_JSC_REV_CitizenGarbageCollectionComplaint""  (""Id"", ""LastUpdatedDate"",
                        ""LastUpdatedBy"",""CreatedDate"",""CreatedBy"", ""DDN"",""Date"", ""IsDeleted"", ""CompanyId"", ""Status"", ""VersionNo"")
			             values
                        ('{id}','{DateTime.Now.ToDatabaseDateFormat()}','{_userContext.UserId}', '{DateTime.Now.ToDatabaseDateFormat()}', '{_userContext.UserId}', '{ddn}',
                            '{DateTime.Now.ToDatabaseDateFormat()}', false,'{_userContext.CompanyId}',{(int)StatusEnum.Active}, 1)";
                await _queryRepo1.ExecuteCommand(queryI, null);
            }


            list = await _queryRepo.ExecuteQueryList<JSCComplaintViewModel>(query, null);
            var count = list.Count;
            if (complaint.IsNotNull() && complaint.GrvStatusCode != "GRV_DISPOSED") // complaint already in progress
            {
                return false;// by default complaint count changes to 4 and no need to raise the complaint
            }

            if (count == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<JSCParcelViewModel>> GetParcelForPropertyTaxCal()
        {
            var query = $@"Select p.""own_name"", p.""tel_no"", p.""res_stat"", p.""ward_no"", p.""bld_age"", p.""occ_selft"" as occ_st_ff from public.""parcel"" as p;";
            var queryData = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return queryData;
        }
        public async Task<List<JSCAssessmentViewModel>> GetViewAssessmentByDDNNO()
        {
            // var query = $@" select A.* from cms.""N_SNC_JSC_PROPERTY_TAX_SELF_ASSESSMENT"" as A where A.""DdnNo"" = '{ddnNo}' ";
            var query = $@"select A.*, lp.""Name"" as PropertyType, lb.""Name"" as BuildingCategory,
                        lt.""Name"" as BuildingType, o.""Name"" as Occupancy
                        from cms.""F_JSC_PROP_MGMNT_SELF_ASSESSMENT_FORM"" as A 
                        join cms.""F_JSC_PROP_MGMNT_UserPropertyMap"" as U on U.""DdnNo"" =A.""DdnNo""
                        
                        left join public.""LOV"" as lp
                        on lp.""Id"" = A.""PropertyType""
                        left join public.""LOV"" as lb
                        on lb.""Id"" = A.""BuildingCategory""
                        left join public.""LOV"" as lt
                        on lt.""Id"" = A.""BuildingType""
                        left join cms.""F_JSC_PROP_MGMNT_OccupancyFactor"" as o
                        on o.""Id"" = A.""OccupancyId""
                            where U.""userId""='{_repo.UserContext.UserId}' ";
            var querydata = await _queryRepo.ExecuteQueryList<JSCAssessmentViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetPointsListByVehicle(string vehicleId)
        {
            string query = $@"select p.""Id"",p.""PointName"" as Name
from cms.""F_JSC_REV_PointAndVehicleMapping"" as pvm 
join cms.""F_JSC_REV_GVP_Add_Point"" as p on pvm.""points""=p.""Id"" and p.""IsDeleted""=false
where pvm.""IsDeleted""=false and pvm.""vehicle""='
            {vehicleId}' ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<string> GetVehicleIdForLoggedInUser(string userId)
        {
            string query = $@"select ""Id"" from cms.""F_JSC_REV_Vehicle""
                            where ""UserId""  ='{userId}' ";
            var queryData = await _queryRepo.ExecuteScalar<string>(query, null);
            return queryData;
        }
        public async Task<string> GetVehicleTypeForLoggedInUser(string userId)
        {
            string query = $@"select l.""Code"" from cms.""F_JSC_REV_Vehicle"" as v
                            join cms.""F_TypeMasters_VehicleTypeMaster"" as l on l.""Id"" = v.""VehicalType""
                            where v.""UserId""  ='{userId}' ";
            var queryData = await _queryRepo.ExecuteScalar<string>(query, null);
            return queryData;
        }


        public async Task<List<IdNameViewModel>> GetOutwardVehicleList(DateTime date)
        {
            string query = $@"Select v.""VehicleNumber"" as Name,
                v.""Id"" as Id
                from cms.""F_JSC_REV_TransferPointOutwardRegister"" as ot
                join cms.""F_JSC_REV_Vehicle"" as v on v.""Id"" = ot.""VehicleNumber""
                where ot.""CreatedDate""::date = '{date}'::date";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetJSCVehicleDetails(string vehicleId, DateTime? startDate, DateTime? endDate)
        {
            var startDateString = "";
            var endDateString = "";
            var result = new JSCGarbageCollectionViewModel();
            //var query = $@"select (row_number() OVER (ORDER BY v.""Id"") - 1) + 1 as SlNo,
            //                v.""Id"" as VehicleId, v.""VehicleNumber"" as VehicleNumber,
            //                l.""Name"" as VehicleTypeName , v.""VehicalType"" as VehicleTypeId,
            //                v.""DryWasteCapacityKgs""::decimal+ v.""WetWasteCapacityKgs""::decimal as VehicleCapacity,
            //                case when o.""VehicleReachingTime"" is null then cast(NOW() as time)::text else o.""VehicleReachingTime""  end as VehicleReachingTime,
            //                o.""VehicleDepartureTime"" as VehicleDepartureTime,
            //                o.""WetWaste"" as WetWaste, o.""DryWaste"" as DryWaste,
            //                o.""Remark"" as Remarks
            //                from cms.""F_JSC_REV_Vehicle"" as v
            //                left join cms.""F_TypeMasters_VehicleTypeMaster"" as l on l.""Id"" = v.""VehicalType""
            //                left join cms.""F_JSC_REV_TransferPointOutwardRegister"" as o on o.""VehicleNumber"" = v.""Id""
            //                #DATE# #VEHICLE# 
            //                ";

            var query = $@"select (row_number() OVER (ORDER BY v.""Id"") - 1) + 1 as SlNo,
                            v.""Id"" as VehicleId, v.""VehicleNumber"" as VehicleNumber,
                            l.""Name"" as VehicleTypeName , v.""VehicalType"" as VehicleTypeId,
                            v.""DryWasteCapacityKgs""::decimal+ v.""WetWasteCapacityKgs""::decimal as VehicleCapacity,
                            case when o.""VehicleReachingTime"" is null then cast(NOW() as time)::text else o.""VehicleReachingTime""  end as VehicleReachingTime,
                            o.""VehicleDepartureTime"" as VehicleDepartureTime,
                            o.""WetWaste"" as WetWaste, o.""DryWaste"" as DryWaste,
                            o.""Remark"" as Remarks
                            from cms.""F_JSC_REV_Vehicle"" as v
                            left join cms.""F_TypeMasters_VehicleTypeMaster"" as l on l.""Id"" = v.""VehicalType""
                            left join cms.""F_JSC_REV_TransferPointOutwardRegister"" as o on o.""VehicleNumber"" = v.""Id""
                             where v.""Id"" = '{vehicleId}' 
                            ";

            var res = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);


            var checkVehicleType = $@" select l.""Code"" from cms.""F_JSC_REV_Vehicle"" as v
                            left join cms.""F_TypeMasters_VehicleTypeMaster"" as l on l.""Id"" = v.""VehicalType"" where v.""Id"" = '{vehicleId}'";

            var vehicleType = await _queryRepo.ExecuteScalar<string>(checkVehicleType, null);

            if (vehicleType.IsNotNullAndNotEmpty() && vehicleType == "TRUCK")
            {
                if (!startDate.IsNotNull())
                {
                    startDateString = DateTime.Now.ToDatabaseDateFormat();
                    endDateString = DateTime.Now.ToDatabaseDateFormat();
                }

                query = $@"select 
                            case when o.""VehicleReachingTime"" is null then cast(NOW() as time)::text else o.""VehicleReachingTime""  end as VehicleReachingTime,
                            o.""VehicleDepartureTime"" as VehicleDepartureTime,
                            o.""WetWaste"" as WetWaste, o.""DryWaste"" as DryWaste,
                            o.""Remark"" as Remarks
                            from cms.""F_JSC_REV_Vehicle"" as v
                            left join cms.""F_TypeMasters_VehicleTypeMaster"" as l on l.""Id"" = v.""VehicalType""
                            left join cms.""F_JSC_REV_TransferPointOutwardRegister"" as o on o.""VehicleNumber"" = v.""Id""
                             where v.""Id"" = '{vehicleId}' 
                                and o.""CreatedDate""::date >= '{startDateString}'::date and o.""CreatedDate""::date <= '{endDateString}'::date
                                ";
                result = await _queryRepo.ExecuteScalar<JSCGarbageCollectionViewModel>(query, null);

                query = $@"select count(o.*)
                            from cms.""F_JSC_REV_Vehicle"" as v
                            left join cms.""F_TypeMasters_VehicleTypeMaster"" as l on l.""Id"" = v.""VehicalType""
                            left join cms.""F_JSC_REV_TransferPointSecondaryPointInwardRegister"" as o on o.""VehicleNumber"" = v.""Id""
                             where v.""Id"" = '{vehicleId}' 
                                and o.""CreatedDate""::date >= '{startDateString}'::date and o.""CreatedDate""::date <= '{endDateString}'::date
                                ";
                var inwardCount = await _queryRepo.ExecuteScalar<int>(query, null);

                query = $@"select count(o.*)
                            from cms.""F_JSC_REV_Vehicle"" as v
                            left join cms.""F_TypeMasters_VehicleTypeMaster"" as l on l.""Id"" = v.""VehicalType""
                            left join cms.""F_JSC_REV_TransferPointOutwardRegister"" as o on o.""VehicleNumber"" = v.""Id""
                             where v.""Id"" = '{vehicleId}' 
                                and o.""CreatedDate""::date >= '{startDateString}'::date and o.""CreatedDate""::date <= '{endDateString}'::date
                                ";
                var outwardCount = await _queryRepo.ExecuteScalar<int>(query, null);



                if (result.IsNotNull() && inwardCount < outwardCount)
                {
                    foreach (var a in res)
                    {
                        a.VehicleReachingTime = result.VehicleReachingTime.IsNotNullAndNotEmpty() ? result.VehicleReachingTime.Split(".")[0] : "";
                        a.VehicleDepartureTime = result.VehicleDepartureTime.IsNotNullAndNotEmpty() ? result.VehicleDepartureTime.Split(".")[0] : "";
                        a.WetWaste = result.WetWaste.IsNotNullAndNotEmpty() ? result.WetWaste : "";
                        a.DryWaste = result.DryWaste.IsNotNullAndNotEmpty() ? result.DryWaste : "";
                        a.Remarks = result.Remarks.IsNotNullAndNotEmpty() ? result.Remarks : "";
                    }
                } else
                {
                    foreach (var a in res)
                    {
                        a.WetWaste = "";
                        a.DryWaste = "";
                        a.Remarks = "";
                    }
                }
            } else
            {
                foreach (var a in res)
                {
                    a.WetWaste = "";
                    a.DryWaste = "";
                    a.Remarks = "";
                }
            }
            
            return res;
        }

        public async Task<List<PropertyTaxPaymentReceiptViewModel>> GetPropertyTaxPaymentReceiptByDDN(string DDNNO)
        {
            var query = $@"Select r.""Id"", r.""Date""::DATE,  Y.""FinancialYearName"" as FinancialYear, r.""ReceiptNumber"", r.""ReceiptAmount"", l.""Name"" as PaymentMode,
                           r.""DdnNo"" FROM CMS.""F_JSC_PROP_MGMNT_JSC_PropertyTax_Payment_Receipt"" as r
                           join public.""LOV"" as l on l.""Id""=r.""PaymentMode""
                           JOIN cms.""F_JSC_PROP_MGMNT_JSCFinancialYear"" as Y on Y.""Id"" = r.""Year"";";
            var queryData = await _queryRepo.ExecuteQueryList<PropertyTaxPaymentReceiptViewModel>(query, null);
            return queryData;
        }
        public async Task<PropertyTaxPaymentReceiptViewModel> GetPropertyTaxPaymentReceiptByReceiptId(string ReceiptId)
        {
            var query = $@"Select r.""Id"", r.""Date""::date, Y.""FinancialYearName"" as FinancialYear, r.""ReceiptNumber"", r.""ReceiptAmount"", l.""Name"" as PaymentMode,
                           r.""DdnNo"" FROM CMS.""F_JSC_PROP_MGMNT_JSC_PropertyTax_Payment_Receipt"" as r
                           left join public.""LOV"" as l on l.""Id""=r.""PaymentMode""
                           JOIN cms.""F_JSC_PROP_MGMNT_JSCFinancialYear"" as Y on Y.""Id"" = r.""Year""
                           WHERE r.""Id""='{ReceiptId}';";
            var queryData = await _queryRepo.ExecuteQuerySingle<PropertyTaxPaymentReceiptViewModel>(query, null);
            return queryData;
        }
        public async Task<List<JSCParcelViewModel>> GetViewPrpertyMapByDdnNoAndUser()
        {
            string query = $@"select distinct P.* from public.""parcel"" as P 
                        join cms.""F_JSC_PROP_MGMNT_UserPropertyMap"" as U on U.""DdnNo"" =P.""mmi_id""
                        Join cms.""F_JSC_PROP_MGMNT_SELF_ASSESSMENT_FORM"" as A on A.""DdnNo"" = P.""mmi_id""
                        join cms.""F_JSC_PROP_MGMNT_JSCFinancialYear"" as Y on Y.""Id"" = A.""Year""
                        where U.""userId""='{_repo.UserContext.UserId}'";
            var queryData = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return queryData;
        }
        public async Task<List<JSCParcelViewModel>> GetViewPrpertyForSelfAssessment()
        {
            string query = $@"select distinct P.* from public.""parcel"" as P 
                        join cms.""F_JSC_PROP_MGMNT_UserPropertyMap"" as U on U.""DdnNo"" =P.""mmi_id""
                        Join cms.""F_JSC_PROP_MGMNT_SELF_ASSESSMENT_FORM"" as A on A.""DdnNo"" = P.""mmi_id""
                        join cms.""F_JSC_PROP_MGMNT_JSCFinancialYear"" as Y on Y.""Id"" = A.""Year""
                        where U.""userId""='{_repo.UserContext.UserId}'
                        and TO_DATE(Y.""StartDate"", 'YYYY-MM-DD') = '2022-04-01 00:00:00'  
                        and TO_DATE(Y.""EndDate"", 'YYYY-MM-DD') = '2023-03-31 01:30:00.000' ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JSCParcelViewModel>> GetAddPropertyExist(string ddnNo)
        {
            string query = $@"select U.*
                            from cms.""F_JSC_PROP_MGMNT_UserPropertyMap"" as U 
                            where U.""DdnNo"" = '{ddnNo}' ";
            var queryData = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return queryData;
        }
        public async Task<List<JSCDailyBasedActivityViewModel>> GetPointAndVehicleMappingData()
        {
            string query = $@"select vm.*,v.""VehicleNumber""
                            from cms.""F_JSC_REV_PointAndVehicleMapping"" as vm
                            join cms.""F_JSC_REV_Vehicle"" as v on vm.""VehicleId""=v.""Id"" and v.""IsDeleted""=false
                            where vm.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<JSCDailyBasedActivityViewModel>(query, null);
            return queryData;
        }

        public async Task<JSCGarbageCollectionViewModel> GetBWGCollection()
        {
            var query = $@"select SUM(""WetWasteInKgs""::int) as WetWasteInKgs ,
                            SUM(""DryWaste""::int) as DryWasteInKgs
                            from cms.""F_JSC_REV_BWGsWasteCollectionFormat""
                            where ""Date""::date = '{DateTime.Today}'::date";
            var data = await _queryRepo.ExecuteScalar<JSCGarbageCollectionViewModel>(query, null);
            return data;

        }

        public async Task<List<JSCInwardOutwardReportViewModel>> GetJSCOutwardReport(DateTime? date)
        {
            var query = $@"Select  (row_number() OVER (ORDER BY ot.""Id"") - 1)+ 1 as SlNo,  v.""VehicleNumber"" as VehicleNumber,
                            ot.""VehicleType"" as VehicleType,
                            v.""DryWasteCapacityKgs""::decimal+ v.""WetWasteCapacityKgs""::decimal as VehicleCapacity,
                           ot.""VehicleReachingTime"" as VehicleReachingTime,
                            ot.""VehicleDepartureTime"" as VehicleDepartureTime,
                            ot.""WetWaste"" as WetWaste,
                            ot.""DryWaste"" as DryWaste,
                            ot.""Remark"" as Remarks
                            from cms.""F_JSC_REV_TransferPointOutwardRegister"" as ot
                            join cms.""F_JSC_REV_Vehicle"" as v on v.""Id"" = ot.""VehicleNumber""
                            where ot.""CreatedDate""::date = '{date}'::date
                           
                            ";

            var data = await _queryRepo.ExecuteQueryList<JSCInwardOutwardReportViewModel>(query, null);
            return data;
        }

        public async Task<List<JSCInwardOutwardReportViewModel>> GetJSCInwardReport(DateTime? date)
        {
            var query = $@"Select  (row_number() OVER (ORDER BY ot.""Id"") - 1)+ 1 as SlNo, v.""VehicleNumber"" as VehicleNumber,
                                ot.""VehicleType"" as VehicleType,
                                v.""DryWasteCapacityKgs""::decimal+ v.""WetWasteCapacityKgs""::decimal as VehicleCapacity,
                               ot.""VehicleReachingTime"" as VehicleReachingTime,
                                ot.""VehicleDepartureTime"" as VehicleDepartureTime,
                                ot.""WetWaste"" as WetWaste,
                                ot.""DryWaste"" as DryWaste,
                                ot.""Remark"" as Remarks
                                from cms.""F_JSC_REV_TransferPointSecondaryPointInwardRegister"" as ot
                                join cms.""F_JSC_REV_Vehicle"" as v on v.""Id"" = ot.""VehicleNumber""
                                where ot.""CreatedDate""::date = '{date}'::date
                                ";

            var data = await _queryRepo.ExecuteQueryList<JSCInwardOutwardReportViewModel>(query, null);
            return data;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByDate(DateTime? date)
        {

            string query = $@"select distinct 'Garbage Collected' as ""Name"", Count(*) as ""Count"" 
                        from cms.""F_JSC_REV_GarbageCollection""
                        where ""CollectionDateTime""::Date = '{date}'::Date
                        union 
                        select  distinct 'Garbage Not Collected' as ""Name"", Count(g.*) as ""Count"" 
                        from public.""parcel"" as g
                        where g.""mmi_id"" not in ( select ""ParcelId""  from cms.""F_JSC_REV_GarbageCollection""
                        where ""CollectionDateTime""::Date = '{date}'::Date)
                        union
                        select distinct 'BWG' as ""Name"", Count(*) as ""Count""
                        from cms.""F_JSC_REV_BWGsWasteCollectionFormat"" as g
                        where g.""Date""::date =  '{date}'::Date
                        union
                        select distinct 'Residential' as ""Name"", Count(c.*) as ""Count"" 
                        from cms.""F_JSC_REV_GarbageCollection"" as  c
                        join cms.""F_JSC_REV_GarbageCollectorProperty"" as cp on cp.""ParcelId"" = c.""ParcelId""
                        join public.""LOV"" as lo on lo.""Id"" = cp.""GarbageTypeId""
                        where ""CollectionDateTime""::Date = '{date}'::Date
                        and lo.""Code"" = 'JSC_GARBAGE_RESIDENTIAL'
                        union
                        select distinct 'Commercial' as ""Name"", Count(c.*) as ""Count"" 
                        from cms.""F_JSC_REV_GarbageCollection"" as  c
                        join cms.""F_JSC_REV_GarbageCollectorProperty"" as cp on cp.""ParcelId"" = c.""ParcelId""
                        join public.""LOV"" as lo on lo.""Id"" = cp.""GarbageTypeId""
                        where ""CollectionDateTime""::Date = '{date}'::Date
                        and lo.""Code"" = 'JSC_GARBAGE_COMMERCIAL'
                        union
                        select distinct 'MSW Wet' as ""Name"", Sum(m.""WetWaste""::int + m.""WetWaste1""::int) as ""Count""
                        from cms.""F_JSC_REV_DailyMSWCollection"" as m
                        where m.""Date""::date = '{date}'::Date
                        union
                        select distinct 'MSW Dry' as ""Name"", Sum(m.""DryWaste""::int + m.""DryWaste1""::int) as ""Count""
                        from cms.""F_JSC_REV_DailyMSWCollection"" as m
                        where m.""Date""::date = '{date}'::Date
                        union
                        select distinct 'Outward Dry' as ""Name"", Sum(m.""DryWaste""::int) as ""Count""
                        from cms.""F_JSC_REV_TransferPointOutwardRegister"" as m
                        where m.""CreatedDate""::date = '{date}'::Date
                        union
                        select distinct 'Outward Wet' as ""Name"", Sum(m.""WetWaste""::int) as ""Count""
                        from cms.""F_JSC_REV_TransferPointOutwardRegister"" as m
                        where m.""CreatedDate""::date = '{date}'::Date
                        union
                        select distinct 'Inward Wet' as ""Name"", Sum(m.""WetWaste""::int) as ""Count""
                        from cms.""F_JSC_REV_TransferPointSecondaryPointInwardRegister"" as m
                        where m.""CreatedDate""::date = '{date}'::Date
                        union
                        select distinct 'Inward Dry' as ""Name"", Sum(m.""DryWaste""::int) as ""Count""
                        from cms.""F_JSC_REV_TransferPointSecondaryPointInwardRegister"" as m
                        where m.""CreatedDate""::date = '{date}'::Date";
            var queryData = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDataByWardAndDate(DateTime? date, string ward)
        {

            string query = $@"select 'BWG' as ""Name"", Count(*) as ""Count""
                                from cms.""F_JSC_REV_BWGsWasteCollectionFormat"" as g
                                join public.""parcel"" as p on p.""mmi_id"" = g.""ParcelId""
                                where p.""ward_no"" =  '{ward}' and g.""Date""::date =  '{date}'::Date
                                union
                                select 'Residential' as ""Name"", Count(c.*) as ""Count"" 
                                from cms.""F_JSC_REV_GarbageCollection"" as  c
                                join public.""parcel"" as p on p.""mmi_id"" = c.""ParcelId""
                                join cms.""F_JSC_REV_GarbageCollectorProperty"" as cp on cp.""ParcelId"" = p.""mmi_id""
                                join public.""LOV"" as lo on lo.""Id"" = cp.""GarbageTypeId""
                                where p.""ward_no"" = '{ward}'
                                and lo.""Code"" = 'JSC_GARBAGE_RESIDENTIAL' and c.""CollectionDateTime""::Date = '{date}'::Date
                                union
                                select 'Commercial' as ""Name"", Count(c.*) as ""Count"" 
                                from cms.""F_JSC_REV_GarbageCollection"" as  c
                                join public.""parcel"" as p on p.""mmi_id"" = c.""ParcelId""
                                join cms.""F_JSC_REV_GarbageCollectorProperty"" as cp on cp.""ParcelId"" = p.""mmi_id""
                                join public.""LOV"" as lo on lo.""Id"" = cp.""GarbageTypeId""
                                where p.""ward_no"" = '{ward}'
                                and lo.""Code"" = 'JSC_GARBAGE_COMMERCIAL' and c.""CollectionDateTime""::Date = '{date}'::Date";
            var queryData = await _queryRepo.ExecuteQueryList<JSCGarbageCollectionViewModel>(query, null);
            return queryData;
        }

        public async Task InsertEnforcementUnAuthorization(JSCEnforcementUnAuthorizationViewModel model)
        {
            var query = $@" Select Count(""Id"") as SlNo From cms.""F_JSC_ENFORCEMENT_Enforcement_UnAuthorization""";
            int SNo = await _queryRepo.ExecuteScalar<int>(query, null);
            //var Ward = $@"SELECT ""wrd_no"" FROM public.ward WHERE ""wrd_no""='{model.WardNo}'";
            //int WardNo = await _queryRepo.ExecuteScalar<int>(Ward, null);
            string WardNo = model.WardNo;
            string SerialNo = "00001";
            if (SNo == 0)
            {
                SerialNo = "00001";
            }
            else if (SNo < 9)
            {
                SerialNo = "0000" + SNo;
            }
            else
            {
                SerialNo = "000" + SNo;
            }
            var unauthoid = Guid.NewGuid().ToString();
            var date = DateTime.Now.ToDatabaseDateFormat();
            var CaseId = "UNAUJMC" + WardNo + SerialNo;
            var query1 = @$"INSERT INTO cms.""F_JSC_ENFORCEMENT_Enforcement_UnAuthorization""(
                            ""Id"", ""CreatedDate"", ""IsDeleted"", ""CreatedBy"", ""LastUpdatedDate"", ""LastUpdatedBy"", 
                            ""CompanyId"", ""LegalEntityId"", ""SequenceOrder"", ""Status"", ""VersionNo"", ""WardNo"", 
                            ""TypesofViolation"", ""CaseId"", ""Name"", ""MobileNo"", ""Address"",""DetailsofViolation"",""Attachment"",""Latitude"",""Longitude"")
	                        VALUES ('{unauthoid}','{date}',false,'{_userContext.UserId}','{date}','{_userContext.UserId}',
                                '{_userContext.CompanyId}','{_userContext.LegalEntityId}','1','{(int)StatusEnum.Active}','1',
                                    '{model.WardNo}','{model.TypesOfViolation}','{CaseId}','{model.Name}','{model.MobileNo}','{model.Address}','{model.DetailsofViolation}','{model.Attachments}','{model.Latitude}','{model.Longitude}')";
            await _query.ExecuteCommand(query1, null);
            String[] attachmentlist = model.Attachments.Split(",");

            foreach (var item in attachmentlist)
            {
                var unauthoAttachId = Guid.NewGuid().ToString();
                var query2 = @$"INSERT INTO cms.""F_JSC_ENFORCEMENT_Attachment""(
	                            ""Id"", ""UnauthorizeAttachment"", ""ParentId"", ""CreatedDate"", 
                                ""LastUpdatedBy"", ""IsDeleted"", ""CompanyId"", ""LegalEntityId"", 
                                ""SequenceOrder"", ""Status"", ""VersionNo"", ""CreatedBy"", 
                                ""LastUpdatedDate"")
	                            VALUES ('{unauthoAttachId}', '{item}', '{unauthoid}', '{date}', '{_userContext.UserId}',
                                        false, '{_userContext.CompanyId}', '{_userContext.LegalEntityId}', '1',
                                        '{(int)StatusEnum.Active}','1', '{_userContext.UserId}', '{date}');";
                await _query.ExecuteCommand(query2, null);
            }

        }

        public async Task<List<IdNameViewModel>> GetUnauthorizedCaseList()
        {
            string query = $@"select ""CaseId"" as Name, ""Id"" as Id from cms.""F_JSC_ENFORCEMENT_Enforcement_UnAuthorization""";
            var result = await _querydata.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetEnforcementViolations()
        {
            string query = $@"select ""Name"" as Name, ""Id"" as Id from cms.""F_JSC_ENFORCEMENT_Violations""
                             where ""IsDeleted""=false  ";
            var result = await _querydata.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCUnauthorizedViolationsDetail(DateTime? date, string ward, string userId)
        {
            var query = $@"SELECT un.""Id"", un.""WardNo"", un.""CreatedDate"", un.""Status"", un.""CaseId"", un.""Name"", 
                            un.""Address"",un.""MobileNo"", un.""Address"", un.""TypesofViolation"",
		                    un.""DetailsofViolation"", un.""Attachment"" as Attachments ,un.""CreatedBy"",TO_CHAR(un.""CreatedDate""::date, 'dd/mm/yyyy') as ""ReportedDateTime""
							,u.""Name""
                            FROM cms.""F_JSC_ENFORCEMENT_Enforcement_UnAuthorization"" as un
                            left join public.""User"" u ON  u.""Id"" = un.""CreatedBy""
                            where un.""IsDeleted""=false #DATE# #Ward#";

            var datefilter = "";
            var wardfilter = "";
            if (date.IsNotNull())
            {
                datefilter = $@" and un.""CreatedDate""::date = '{date}'::date";
            }
            if (ward.IsNotNull())
            {
                wardfilter = $@" and un.""WardNo"" = '{ward}'";
            }

            query = query.Replace("#DATE#", datefilter);
            query = query.Replace("#Ward#", wardfilter);

            var data = await _queryRepo.ExecuteQueryList<JSCEnforcementUnAuthorizationViewModel>(query, null);
            return data;
        }

        public async Task InsertEnforcementAuthorization(JSCEnforcementUnAuthorizationViewModel model)
        {
            var query = $@" Select Count(""Id"") as SlNo From cms.""F_JSC_ENFORCEMENT_Enforcement_Authorization""";
            int SNo = await _queryRepo.ExecuteScalar<int>(query, null);
            string WardNo = model.WardNo;
            string SerialNo = "00001";
            if (SNo == 0)
            {
                SerialNo = "00001";
            }
            else if (SNo < 9)
            {
                SerialNo = "0000" + SNo;
            }
            else
            {
                SerialNo = "000" + SNo;
            }
            var authoid = Guid.NewGuid().ToString();
            var date = DateTime.Now.ToDatabaseDateFormat();
            var CaseId = "AUJMC" + WardNo + SerialNo;
            var query1 = @$"INSERT INTO cms.""F_JSC_ENFORCEMENT_Enforcement_Authorization""(
	                                    ""Id"", ""CreatedDate"", ""CreatedBy"", ""LastUpdatedDate"", ""LastUpdatedBy"", 
                                        ""IsDeleted"", ""CompanyId"", ""LegalEntityId"", ""SequenceOrder"", ""Status"", 
                                        ""VersionNo"", ""TypesofViolation"", ""CaseId"", ""Name"", ""MobileNo"", ""Address"",
                                        ""WardNo"",""Latitude"",""Longtitude"")
	                        VALUES  ('{authoid}','{date}','{_userContext.UserId}','{date}','{_userContext.UserId}','false',
                                    '{_userContext.CompanyId}','{_userContext.LegalEntityId}','1','{(int)StatusEnum.Active}','1',
                                    '{model.TypesOfViolation}','{CaseId}','{model.Name}','{model.MobileNo}','{model.Address}','{model.WardNo}',
                                    '{model.Latitude}','{model.Longitude}')";
            await _query.ExecuteCommand(query1, null);
        }
        public async Task<List<IdNameViewModel>> GetAuthorizedCaseList()
        {
            string query = $@"select ""CaseId"" as Name, ""Id"" as Id from cms.""F_JSC_ENFORCEMENT_Enforcement_Authorization""";
            var result = await _querydata.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCAuthorizedViolationsDetail(DateTime? date, string ward,string userId)
        {
            var query = $@"SELECT un.""Id"", un.""WardNo"", un.""CreatedDate"", un.""Status"", un.""CaseId"", un.""Name"", 
                            un.""Address"",un.""MobileNo"", un.""Address"", un.""TypesofViolation"",
		                   un.""CreatedBy"",TO_CHAR(un.""CreatedDate""::date, 'dd/mm/yyyy') as ""ReportedDateTime""
							,u.""Name""
                            FROM cms.""F_JSC_ENFORCEMENT_Enforcement_Authorization"" as un
                            left join public.""User"" u ON  u.""Id"" = un.""CreatedBy""
                            where un.""IsDeleted""=false #DATE# #Ward#";

            var datefilter = "";
            var wardfilter = "";
            if (date.IsNotNull())
            {
                datefilter = $@" and un.""CreatedDate""::date = '{date}'::date";
            }
            if (ward.IsNotNull())
            {
                wardfilter = $@" and un.""WardNo"" = '{ward}'";
            }

            query = query.Replace("#DATE#", datefilter);
            query = query.Replace("#Ward#", wardfilter);

            var data = await _queryRepo.ExecuteQueryList<JSCEnforcementUnAuthorizationViewModel>(query, null);
            return data;
        }

        public async Task<List<JSCEnforcementSubLoginViewModel>> GetSubLogin(string loginType)
        {
            var query = $@" select * from cms.""F_JSC_ENFORCEMENT_SubLogins""  where ""IsDeleted""=false and ""SubloginType"" = '{loginType}'
                        ";
            var data = await _queryRepo.ExecuteQueryList<JSCEnforcementSubLoginViewModel>(query, null);
            return data;
        }

        public async Task<IList<UserViewModel>> GetUserListForSubLogin()
        {
            
                string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""PortalId""='{_repo.UserContext.PortalId}' and ""LegalEntityId""='{_repo.UserContext.LegalEntityId}' and ""CompanyId""='{_repo.UserContext.CompanyId}'
                            and ""Id"" NOT IN(Select ""SubloginUserId"" from cms.""F_JSC_ENFORCEMENT_SubLogins"")";
                var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
                return list;
            
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCOBPSAuthorizedDetail(DateTime? date, string ward)
        {
            var query = $@"SELECT ob.""Id"", ob.""CreatedDate"", ob.""CreatedBy"",  ob.""OrderNo"", ob.""Ward"" as WardNo, 
                            ob.""fileAttachment"" as Attachments, ob.""LatLong"", ob.""Name"", ob.""Mobile"", ob.""DateOfApproval"" as ReportedDateTime, ob.""ApprovalDetails"",
                            ob.""AuthorizationId"",ob.""documentType""
                            FROM cms.""N_DMS_JMC_OBPS"" ob
                            left join public.""User"" u ON  u.""Id"" = ob.""CreatedBy""
                            where ob.""IsDeleted""=false  And ob.""AuthorizationId"" IS NOT NULL #DATE# #Ward#";

            var datefilter = "";
            var wardfilter = "";
            if (date.IsNotNull())
            {
                datefilter = $@" and ob.""CreatedDate""::date = '{date}'::date";
            }
            if (ward.IsNotNull())
            {
                wardfilter = $@" and ob.""Ward"" = '{ward}'";
            }

            query = query.Replace("#DATE#", datefilter);
            query = query.Replace("#Ward#", wardfilter);

            var data = await _queryRepo.ExecuteQueryList<JSCEnforcementUnAuthorizationViewModel>(query, null);
            return data;
        }

        public async Task<List<PropertyTaxPaymentReceiptViewModel>> GetPropertyPaymentDetails()
        {
            var query = $@"select I.* , Y.""FinancialYearName"" as Year from cms.""F_JSC_PROP_MGMNT_PROPERTY_INSTALLMENT"" as I 
                                join cms.""F_JSC_PROP_MGMNT_JSCFinancialYear"" as Y on Y.""Id"" = I.""Year"" ";
            var data = await _queryRepo.ExecuteQueryList<PropertyTaxPaymentReceiptViewModel>(query, null);
            return data;
        } 
        public async Task<JSCPropertyTaxInstallmentViewModel> GetPropertyPaymentDetailById(string id)
        {
            var query = $@" select * from cms.""F_JSC_PROP_MGMNT_PROPERTY_INSTALLMENT"" where ""Id""='{id}' ";
            var data = await _queryRepo.ExecuteQuerySingle<JSCPropertyTaxInstallmentViewModel>(query, null);
            return data;
        }
        public async Task<string> GetNextPropertyReceiptNumber()
        {
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
            var query = $@"update cms.""F_JSC_PROP_MGMNT_RECEIPTNUMBER_SEQUENCE"" SET ""NextId"" = ""NextId""::int+1,
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            
            RETURNING ""NextId""::int-1";

            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            if (nextId == null)
            {
                nextId = 1;
                var formTempModel = new FormTemplateViewModel();
                formTempModel.DataAction = DataActionEnum.Create;
                formTempModel.TemplateCode = "RECEIPTNUMBER_SEQUENCE";
                var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("NextId", nextId);
                formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _cmsBusiness.ManageForm(formmodel);
            }

            return Convert.ToString(nextId);
        }

        public async Task<string> GetWardListByUser(string userId)
        {
            var query = $@" select ""Ward"" from cms.""F_JSC_ENFORCEMENT_SubLogins""  where ""IsDeleted""=false and ""SubloginUserId"" = '{userId}'
                        ";
            var data = await _queryRepo.ExecuteScalar<string>(query, null);
            return data;
        }


        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementAuthorization(string userId)
        {
            string query = @$"select * from cms.""F_JSC_ENFORCEMENT_Enforcement_Authorization"" where ""IsDeleted""=false
                            and ""CreatedBy"" = '{userId}'";
            var list = await _queryRepo.ExecuteQueryList<JSCEnforcementUnAuthorizationViewModel>(query, null);
            return list;

        }
        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementUnAuthorization(string userId)
        {

            string query = @$"select * from cms.""F_JSC_ENFORCEMENT_Enforcement_UnAuthorization"" where ""IsDeleted""=false
                            and ""CreatedBy"" = '{userId}'";
            var list = await _queryRepo.ExecuteQueryList<JSCEnforcementUnAuthorizationViewModel>(query, null);
            return list;

        }

        public async Task InsertEnforcementAuthorizedWeeklyReport(JSCEnforcementUnAuthorizationViewModel model)
        {
            var authoid = Guid.NewGuid().ToString();
            var date = DateTime.Now.ToDatabaseDateFormat();
            var query = @$"INSERT INTO cms.""F_JSC_ENFORCEMENT_AuthorizedWeeklyReport""(
	                        ""Id"", ""CreatedDate"", ""CreatedBy"", ""LastUpdatedDate"", 
                            ""LastUpdatedBy"", ""IsDeleted"", ""CompanyId"", ""LegalEntityId"", 
                            ""SequenceOrder"", ""Status"", ""VersionNo"", ""AuthorizationID"", 
                            ""Attachments"", ""Latitude"", ""Longitude"", ""Remarks"")
	                        VALUES  ('{authoid}','{date}','{_userContext.UserId}','{date}','{_userContext.UserId}','false',
                                    '{_userContext.CompanyId}','{_userContext.LegalEntityId}','1','{(int)StatusEnum.Active}','1',
                                    '{model.AuthorizationId}','{model.Attachments}','{model.Latitude}','{model.Longitude}','{model.Remarks}')";
            await _query.ExecuteCommand(query, null);
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementAuthorizationWeeklyReport()
        {

            string query = @$"select * from cms.""F_JSC_ENFORCEMENT_AuthorizedWeeklyReport"" where ""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<JSCEnforcementUnAuthorizationViewModel>(query, null);
            return list;

        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementSubloginMappinglist()
        {
            string query = @$"SELECT sub.""Id"", sub.""CreatedDate"", sub.""CreatedBy"", sub.""LastUpdatedBy"", sub.""LastUpdatedDate"", sub.""IsDeleted"", sub.""CompanyId"", sub.""LegalEntityId"", sub.""SequenceOrder"", sub.""Status"", sub.""VersionNo"", ""SubloginType"", ""SubloginUserId"", ""Ward"" as WardNo
	                        ,u.""Name"",l.""Name"" as SubLoginType
	                FROM cms.""F_JSC_ENFORCEMENT_SubLogins"" sub
                    join public.""User"" u ON  u.""Id"" = sub.""SubloginUserId""
                    join public.""LOV"" l ON  l.""Id"" = sub.""SubloginType""";
            var list = await _queryRepo.ExecuteQueryList<JSCEnforcementUnAuthorizationViewModel>(query, null);
            return list;
        }
    }
}
