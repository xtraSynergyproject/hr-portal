using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
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

namespace CMS.Business
{
    public class EGovernanceBusiness : BusinessBase<NoteViewModel, NtsNote>, IEGovernanceBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _querydata;
        private readonly IRepositoryQueryBase<EGovCommunityHallViewModel> _querych;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<ServiceTemplateViewModel> _query;
        private readonly INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryRepo1;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserBusiness _userBusiness;
        public EGovernanceBusiness(IRepositoryQueryBase<IdNameViewModel> querydata,
            IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<EGovCommunityHallViewModel> querych
            , IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<TaskViewModel> queryRepo1,
            IRepositoryQueryBase<ServiceTemplateViewModel> query, INoteBusiness noteBusiness,
            ILOVBusiness lOVBusiness, IServiceBusiness serviceBusiness, IUserBusiness userBusiness) : base(repo, autoMapper)
        {
            _querych = querych;
            _queryRepo = queryRepo;
            _query = query;
            _noteBusiness = noteBusiness;
            _queryRepo1 = queryRepo1;
            _lOVBusiness = lOVBusiness;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _querydata = querydata;

        }


        public async Task<List<EGovCommunityHallViewModel>> GetCommunityHallBookingList(string hallId)
        {

            var Query = $@"Select ch.""CommunityHallName"",c.""BookingFromDate""::TIMESTAMP::DATE,c.""BookingToDate""::TIMESTAMP::DATE,lov.""Name"" as Status 
from cms.""N_PublicServices_CommunityHallBooking"" as c
join cms.""N_EGOV_MASTER_DATA_CommunityHallName"" as ch on c.""CommunityHallNameId""=ch.""Id"" and ch.""IsDeleted""=false
join public.""NtsNote"" as n on c.""NtsNoteId""=n.""Id""
join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId""
join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""Code"" not in ('SERVICE_STATUS_CANCEL','SERVICE_STATUS_REJECT')
where c.""CommunityHallNameId""='{hallId}' ";

            var result = await _querych.ExecuteQueryList<EGovCommunityHallViewModel>(Query, null);
            return result;

        }

        public async Task<List<EGovCorporationWardViewModel>> GetCorporatorList(string wardNo = null, string wardName = null, string councillorName = null, string address = null, string phone = null)
        {

            var Query = $@" SELECT * FROM cms.""N_E_GOV_CorporationWard"" where ""IsDeleted""=false 
                        ";

            var result = await _querych.ExecuteQueryList<EGovCorporationWardViewModel>(Query, null);
            if (wardNo.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.WardNo.Contains(wardNo)).ToList();
            }

            if (wardName.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.WardName.Contains(wardName)).ToList();
            }

            if (councillorName.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.CouncillorName.Contains(councillorName)).ToList();
            }

            if (address.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Address.Contains(address)).ToList();

            }

            if (phone.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Phone.Contains(phone)).ToList();
            }



            return result;

        }

        public async Task<List<EGovCorporationWardViewModel>> GetAdminWardList(string wardNo = null, string wardName = null, string electoralWardName = null, string location = null, string constituencyName = null, string latitude = null, string longitude = null)
        {

            var Query = $@"  SELECT aw.""AdministrativeWardNo"" as WardNo,aw.""AdministrativeWardName"" as WardName,
trim(both '[ ]' from aw.""ElectoralWardId"") as ElectoralWardId,c.""ConstituencyName"" as ConstituencyName,aw.""Location"" as Location,aw.""Longitude"" as Longitude,
aw.""Latitude"" as Latitude
FROM cms.""N_E_GOV_AdminWard"" aw
      left join   cms.""N_E_GOV_CorporationWard"" cw on cw.""Id""= aw.""ElectoralWardId"" and cw.""IsDeleted""=false   
	 left join cms.""N_E_GOV_Constituency"" c on c.""Id""= aw.""ConstituencyId"" and c.""IsDeleted""=false   
	  where aw.""IsDeleted""=false 
                        ";

            var result = await _querych.ExecuteQueryList<EGovCorporationWardViewModel>(Query, null);
            foreach (var item in result)
            {
                var corporateWardIds = item.ElectoralWardId.Split(",");
                //var s= string.Join("','", item.ElectoralWardId);
                var s = item.ElectoralWardId.Replace("\"", "'");
                var wardquery = $@"SELECT string_agg(concat(cw.""WardNo""::text,' ',cw.""WardName""::text), ',') as ""WardName"" 

          FROM cms.""N_E_GOV_CorporationWard"" cw
         where cw.""IsDeleted"" = false and cw.""Id"" in (" + s + ")";
                var corpwardname = await _querych.ExecuteQuerySingle<EGovCorporationWardViewModel>(wardquery, null);
                if (corpwardname.IsNotNull())
                {
                    item.ElectoralWardName = corpwardname.WardName;
                }

            }
            if (wardNo.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.WardNo.Contains(wardNo)).ToList();
            }

            if (wardName.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.WardName.Contains(wardName)).ToList();
            }

            if (electoralWardName.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.ElectoralWardName.Contains(electoralWardName)).ToList();
            }
            if (constituencyName.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.ConstituencyName.Contains(constituencyName)).ToList();
            }

            if (location.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Location.Contains(location)).ToList();

            }

            if (latitude.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Latitude.Contains(latitude)).ToList();
            }
            if (longitude.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Longitude.Contains(longitude)).ToList();
            }



            return result;

        }

        public async Task<List<EGovCorporationWardViewModel>> GetAdminOfficersWardList(string wardNo = null, string wardName = null, string officerName = null, string location = null, string phone = null)
        {

            var Query = $@"  SELECT cw.""AdministrativeWardNo"" as WardNo,
aw.""OfficerName"" as OffficerName,aw.""Phone"" as Phone
FROM cms.""N_E_GOV_AdminWardOfficer"" aw
      left join   cms.""N_E_GOV_AdminWard"" cw on cw.""Id""= aw.""AdministrativeWardId"" and cw.""IsDeleted""=false    
	  where aw.""IsDeleted""=false 
                        ";

            var result = await _querych.ExecuteQueryList<EGovCorporationWardViewModel>(Query, null);
            if (wardNo.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.WardNo.Contains(wardNo)).ToList();
            }

            if (wardName.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.WardName.Contains(wardName)).ToList();
            }

            if (officerName.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.OffficerName.Contains(officerName)).ToList();
            }

            if (phone.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Phone.Contains(phone)).ToList();
            }

            if (location.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Location.Contains(location)).ToList();

            }

            return result;

        }


        public async Task<IList<ServiceTemplateViewModel>> GetMyRequestList(bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string searchtext = null, DateTime? From = null, DateTime? To = null, string statusIds = null, string templateIds = null)
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
        public async Task<List<MapMarkerViewModel>> GetMapMarkerDeatils()
        {
            var query = $@"select n.*,m.""areaName"" as AreaName, m.""circleAndMarkCoo"" as CircleAndMarkCoo, m.""polyCoord"" as PolyCoord
                            , m.""markType"" as MarkType, m.""circleRadius"" as CircleRadius, lov.""Name"" as MarkTypeName
                            from cms.""N_E_GOV_MapMarker"" as m
                            join public.""NtsNote"" as n on n.""Id"" = m.""NtsNoteId"" and n.""IsDeleted""=false
                            left join public.""LOV"" as lov on lov.""Id""=m.""markType"" and lov.""IsDeleted""=false    
                            where m.""IsDeleted""=false ";
            var list = await _query.ExecuteQueryList<MapMarkerViewModel>(query, null);
            return list.ToList();
        }

        public async Task<List<EGOVBannerViewModel>> GetEGovSliderBannerData()
        {
            var query = $@" select sb.""SliderImageId"" as BannerImageId, sb.""SliderContent"" as BannerContent,sb.""SequenceNo"" as SequenceNo 
                        from cms.""N_EGOV_MASTER_DATA_SliderBanner"" as sb
                        where sb.""IsDeleted""=false ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovProjectMasterData()
        {
            var query = $@" select pm.""ProjectBannerId"" as BannerImageId, pm.""ProjectContent"" as BannerContent,pm.""ProjectStartDate"" as StartDate,  pm.""ProjectEndDate"" as EndDate,pm.""SequenceNo"" as SequenceNo
                        from cms.""N_EGOV_MASTER_DATA_EGovProjectMaster"" as pm
                        where pm.""IsDeleted""=false ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovLatestNewsData()
        {
            var query = $@" select n.""NewsImageId"" as BannerImageId, n.""NewsContent"" as BannerContent,n.""NewsDate"" as StartDate, n.""NewsExpiryDate"" as EndDate,n.""SequenceNo"" as SequenceNo
                        from cms.""N_EGOV_MASTER_DATA_EGovLatestNews"" as n
                        where n.""IsDeleted""=false ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovNotificationMasterData()
        {
            var query = $@" select n.""Details"" as BannerContent,n.""StartDate"" as StartDate, n.""ExpiryDate"" as EndDate,n.""SequenceNo"" as SequenceNo,n.""UrlLink"" as UrlLink
                        from cms.""N_EGOV_MASTER_DATA_EGovNotification"" as n
                        where n.""IsDeleted""=false ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovCorporatePhotoData()
        {
            var query = $@" select n.""Title"" as BannerContent,n.""PhotoId"" as BannerImageId,n.""SequenceNo"" as SequenceNo,n.""UrlLink"" as UrlLink
                        from cms.""N_EGOV_MASTER_DATA_EGovCorporatePhoto"" as n
                        where n.""IsDeleted""=false ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovTenderMasterData()
        {
            var query = $@" select n.""TenderDetails"" as BannerContent,n.""TenderDate"" as StartDate,n.""SequenceNo"" as SequenceNo,n.""UrlLink"" as UrlLink
                        from cms.""N_EGOV_MASTER_DATA_EGovTender"" as n
                        where n.""IsDeleted""=false ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovOtherWebsiteData()
        {
            var query = $@" select n.""Title"" as BannerContent,n.""LogoId"" as BannerImageId,n.""SequenceNo"" as SequenceNo,n.""UrlLink"" as UrlLink
                        from cms.""N_EGOV_MASTER_DATA_EGovOtherWebsite"" as n
                        where n.""IsDeleted""=false ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<IList<ServiceTemplateViewModel>> GetServiceList(string statusCodes, string templateCode)
        {
            var query = $@"Select  s.""Id"" as ServiceId, s.""ServiceNo"" as ServiceNo, t.""DisplayName"" as TemplateDisplayName,t.""Code"" as TemplateCode,   
ss.""Name"" as ServiceStatusName,ss.""Code"" as ServiceStatusCode, s.""CreatedDate"" as CreatedDate
from public.""NtsService"" as s
join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false 
join public.""Template"" as t on t.""Id"" =s.""TemplateId"" and t.""IsDeleted""=false #TEMPCODEWHERE#
join public.""TemplateCategory"" as tc on tc.""Id"" =t.""TemplateCategoryId"" and tc.""IsDeleted""=false
join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false #STATUSWHERE#
left join public.""Module"" as m on m.""Id"" =t.""ModuleId"" and m.""IsDeleted""=false 
where s.""IsDeleted""=false and (s.""RequestedByUserId""='{_repo.UserContext.UserId}' or s.""OwnerUserId""='{_repo.UserContext.UserId}') 
and s.""PortalId""='{_repo.UserContext.PortalId}' order by s.""CreatedDate"" desc ";

            var temcode = "";
            if (templateCode.IsNotNullAndNotEmpty())
            {
                temcode = $@" and t.""Code""='{templateCode}' ";
            }
            query = query.Replace("#TEMPCODEWHERE#", temcode);

            var status = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                status = $@" and ss.""Code"" in ('{statusCodes.Replace(",", "','")}')";
            }
            query = query.Replace("#STATUSWHERE#", status);


            var result = await _query.ExecuteQueryList(query, null);

            return result;
        }

        public async Task<EGovBinBookingViewModel> GetExistingBinBookingDetails(string consumerNo)
        {
            var query = $@"Select b.*,s.""Id"" as ServiceId from cms.""N_EGOV_SANITATIONS_BinBookingForm"" as b
join public.""NtsNote"" as n on b.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false
where b.""BinBookingConsumerNo""='{consumerNo}' and b.""IsDeleted""=false ";

            var result = await _query.ExecuteQuerySingle<EGovBinBookingViewModel>(query, null);
            return result;
        }

        public async Task<bool> UpdateBinBookingDetails(dynamic udf)
        {
            if (udf != null)
            {

                var query = $@"Update cms.""N_EGOV_SANITATIONS_BinBookingForm"" set ""ContactNumber""='{udf.ContactNumber}', ""Email""='{udf.Email}',
""Address""='{udf.Address}',""NumberOfBins""='{udf.NumberOfBins}',""BookingFromDate""='{udf.BookingFromDate}',""BookingToDate""='{udf.BookingToDate}',""BinSizeId""='{udf.BinSizeId}' where ""BinBookingConsumerNo""='{udf.BinBookingConsumerNo}' ";

                await _query.ExecuteCommand(query, null);
                return true;
            }
            return false;
        }

        public async Task<EGovSewerageViewModel> GetExistingSewerageDetails(string consumerNo)
        {
            var query = $@"Select sc.*,sc.""sewerageConsumerNumber"" as SewerageConsumerNumber,s.""Id"" as ServiceId from cms.""N_EGOV_WATER_NewSewerageConnection"" as sc
join public.""NtsNote"" as n on sc.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false
where sc.""sewerageConsumerNumber""='{consumerNo}' and sc.""IsDeleted""=false ";

            var result = await _query.ExecuteQuerySingle<EGovSewerageViewModel>(query, null);
            return result;
        }
        public async Task<List<ServiceTemplateViewModel>> GetCommercialTaxService()
        {

            var Query = $@" select  n.""ServiceNo"" as ""ServiceNo"",n.""Id"" as Id,tr.""Code"" as ""TemplateCode"", 
 ns.""Name"" as ""ServiceStatusName"",n.""ServiceSubject"" as ""ServiceSubject"",n.""StartDate"" as ""StartDate"",n.""DueDate"" as ""DueDate"",
 sc.""BillAmount"" as ""BillAmount""


from public.""NtsService"" as n
  join public.""Template"" as tr on tr.""Id""=n.""TemplateId"" and tr.""IsDeleted""=false  
 join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
left join cms.""N_SNC_EGOV_SANITATIONS_SanitationTaxPaymentCommercialUsers"" as sc on sc.""Id""=n.""UdfNoteTableId""
 where n.""IsDeleted""=false and tr.""Code""='SanitationTaxPaymentCommercialUsers' 
                        ";

            var result = await _query.ExecuteQueryList<ServiceTemplateViewModel>(Query, null);
            return result;

        }
        public async Task<List<ServiceTemplateViewModel>> GetResidentialTaxService()
        {

            var Query = $@" select  n.""ServiceNo"" as ""ServiceNo"",n.""Id"" as Id,tr.""Code"" as ""TemplateCode"", 
 ns.""Name"" as ""ServiceStatusName"",n.""ServiceSubject"" as ""ServiceSubject"",n.""StartDate"" as ""StartDate"",n.""DueDate"" as ""DueDate"",
 sr.""BillAmount""


from public.""NtsService"" as n
  join public.""Template"" as tr on tr.""Id""=n.""TemplateId"" and tr.""IsDeleted""=false  
 join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
left join cms.""N_SNC_EGOV_SANITATIONS_SanitationTaxPaymentResidentialUsers"" as sr on sr.""Id""=n.""UdfNoteTableId""
 where n.""IsDeleted""=false and tr.""Code""='SanitationTaxPaymentResidentialUsers' 
                        ";

            var result = await _query.ExecuteQueryList<ServiceTemplateViewModel>(Query, null);
            return result;
        }

        public async Task<List<ServiceTemplateViewModel>> GetTradeTaxService()
        {
            var Query = $@" select  n.""ServiceNo"" as ""ServiceNo"",n.""Id"" as Id,tr.""Code"" as ""TemplateCode"", 
ns.""Name"" as ""ServiceStatusName"",sr.""TradeName"" as ""ServiceSubject"",n.""StartDate"" as ""StartDate"",sr.""BillDueDate"" as ""DueDate"",
sr.""BillAmount""
from public.""NtsService"" as n
join public.""Template"" as tr on tr.""Id""=n.""TemplateId"" and tr.""IsDeleted""=false  
join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
left join cms.""N_SNC_EGOV_SANITATIONS_SanitationTaxPaymentCommercialUsers"" as sr on sr.""Id""=n.""UdfNoteTableId""
where n.""IsDeleted""=false and tr.""Code""='SanitationTaxPaymentCommercialUsers' and sr.""serviceName""='RegisterForTradeTax' ";

            var result = await _query.ExecuteQueryList<ServiceTemplateViewModel>(Query, null);
            return result;
        }

        public async Task<IList<TaskViewModel>> GetTaskList(string portalId)
        {
            var list = new List<TaskViewModel>();
            string query = @$"Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""AverageCostDifferAsTheRequest"" as ""ServiceCost"",b.""BinBookingConsumerNo"" as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"",ps.""Name"" as ""PaymentStatus"",ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                           
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_EGOV_SANITATIONS_BinBookingForm"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                            Left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false  
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('BIN_BOOKING_PAYMENT','CitizenCommunityHallBooking','MakePayment','MakePaymentSewerageRequest','UPDATE_BIN_PAYMENT','CD_WASTE_PAYMENT','POULTRY_PAYMENT','SEPTIC_TANK_PAYMENT','Citizen can Make Payment') and t.""IsDeleted""=false 
                        UNION
                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""TotalCharges"" as ""ServiceCost"",'' as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"",ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false
                            
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_PublicServices_CommunityHallBooking"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                             Left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('BIN_BOOKING_PAYMENT','CitizenCommunityHallBooking','MakePayment','MakePaymentSewerageRequest','UPDATE_BIN_PAYMENT','CD_WASTE_PAYMENT','POULTRY_PAYMENT','SEPTIC_TANK_PAYMENT','Citizen can Make Payment') and t.""IsDeleted""=false
                              UNION
                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""AverageCostDifferAsTheRequest"" as ""ServiceCost"",b.""sewerageConsumerNumber"" as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                            
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_EGOV_WATER_NewSewerageConnection"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                             left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('BIN_BOOKING_PAYMENT','CitizenCommunityHallBooking','MakePayment','MakePaymentSewerageRequest', 'UPDATE_BIN_PAYMENT','CD_WASTE_PAYMENT','POULTRY_PAYMENT','SEPTIC_TANK_PAYMENT','Citizen can Make Payment') and t.""IsDeleted""=false
                                     UNION
                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""AverageCostDifferAsTheRequest"" as ""ServiceCost"",'' as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                           
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_EGOV_WATER_NewWaterConnectionRequest"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                              Left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('BIN_BOOKING_PAYMENT','CitizenCommunityHallBooking','MakePayment','MakePaymentSewerageRequest','UPDATE_BIN_PAYMENT','CD_WASTE_PAYMENT','POULTRY_PAYMENT','SEPTIC_TANK_PAYMENT','Citizen can Make Payment') and t.""IsDeleted""=false
                          UNION
                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""AverageCostDifferAsTheRequest"" as ""ServiceCost"",b.""BinBookingConsumerNo"" as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                           
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_SNC_EGOV_SANITATIONS_ExistingBinBooking"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                           left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('BIN_BOOKING_PAYMENT','CitizenCommunityHallBooking','MakePayment','MakePaymentSewerageRequest','UPDATE_BIN_PAYMENT','CD_WASTE_PAYMENT','POULTRY_PAYMENT','SEPTIC_TANK_PAYMENT','Citizen can Make Payment') and t.""IsDeleted""=false
                             UNION
                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""AverageCostDifferAsTheRequest"" as ""ServiceCost"",b.""sewerageConsumerNumber"" as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                              
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_SNC_EGOV_SANITATIONS_ExistingSewerageConnection"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                            Left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('BIN_BOOKING_PAYMENT','CitizenCommunityHallBooking','MakePayment','MakePaymentSewerageRequest','UPDATE_BIN_PAYMENT','CD_WASTE_PAYMENT','POULTRY_PAYMENT','SEPTIC_TANK_PAYMENT','Citizen can Make Payment') and t.""IsDeleted""=false
                            UNION
                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""AverageCostDifferAsTheRequest"" as ""ServiceCost"",b.""PoultryWasteServiceNumber"" as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                             
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_EGOV_SANITATIONS_PoultryWasteForm"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('BIN_BOOKING_PAYMENT','CitizenCommunityHallBooking','MakePayment','MakePaymentSewerageRequest','UPDATE_BIN_PAYMENT','CD_WASTE_PAYMENT','POULTRY_PAYMENT','SEPTIC_TANK_PAYMENT','Citizen can Make Payment') and t.""IsDeleted""=false
                      UNION
                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""AverageCostDifferAsTheRequest"" as ""ServiceCost"",'' as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                            
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_EGOV_SANITATIONS_SepticTankClearanceForm"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                                left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('BIN_BOOKING_PAYMENT','CitizenCommunityHallBooking','MakePayment','MakePaymentSewerageRequest','UPDATE_BIN_PAYMENT','CD_WASTE_PAYMENT','POULTRY_PAYMENT','SEPTIC_TANK_PAYMENT','Citizen can Make Payment') and t.""IsDeleted""=false
                    UNION
  Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""BillAmount"" as ""ServiceCost"",'' as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                            
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_EGOV_SANITATIONS_ConstructionandDebrisWasteClearanceForm"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('CITIZEN_PAYMENT_POST','Citizen can Make Payment') and t.""IsDeleted""=false
   UNION
  Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""BillAmount"" as ""ServiceCost"",'' as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                            
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_SNC_EGOV_SANITATIONS_BinBooking"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" ='Payment - Bin' and t.""IsDeleted""=false  
UNION
  Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""TotalCharges"" as ""ServiceCost"",'' as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                            
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_PublicServices_CommunityHallBooking"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" ='COM_HALL_PAYMENT' and t.""IsDeleted""=false 
";
            var result = await _queryRepo1.ExecuteQueryList(query, null);
            //foreach (var i in result)
            //{
            //    i.DisplayDueDate = i.DueDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            //    i.DisplayStartDate = i.StartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            //}
            return result;
        }

        public async Task<List<IdNameViewModel>> GetPropertyList(string wardId, string rentingType)
        {
            //            var query = $@"Select p.""Id"",p.""PropertyName"" as Name,rm.""EndDate""
            //from cms.""N_EGOV_MASTER_DATA_RentalProperty"" as p
            //left join cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" as rm on p.""Id"" = rm.""PropertyId"" and rm.""IsDeleted"" = false
            //where p.""WardId"" = '{wardId}' and p.""PropertyRentingTypeId"" = '{rentingType}'
            //and ""EndDate"" is null
            //union
            //Select p.""Id"",p.""PropertyName"" as Name,rm.""EndDate""
            //from cms.""N_EGOV_MASTER_DATA_RentalProperty"" as p
            //left join cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" as rm on p.""Id"" = rm.""PropertyId"" and rm.""IsDeleted"" = false
            //join public.""NtsService"" as s on rm.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
            //join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
            //where p.""WardId""='{wardId}' and p.""PropertyRentingTypeId""='{rentingType}'
            //and (rm.""EndDate""::TIMESTAMP::DATE < '{DateTime.Now}'::TIMESTAMP::DATE and ss.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_INPROGRESS'))
            //union
            //Select p.""Id"",p.""PropertyName"" as Name,rm.""EndDate""
            //from cms.""N_EGOV_MASTER_DATA_RentalProperty"" as p
            //left join cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" as rm on p.""Id"" = rm.""PropertyId"" and rm.""IsDeleted"" = false
            //join public.""NtsService"" as s on rm.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
            //join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
            //where p.""WardId""='{wardId}' and p.""PropertyRentingTypeId""='{rentingType}'
            //and (rm.""EndDate""::TIMESTAMP::DATE > '{DateTime.Now}'::TIMESTAMP::DATE and ss.""Code"" in ('SERVICE_STATUS_REJECT','SERVICE_STATUS_CANCEL'))
            // ";

            var query = $@"Select ""Id"",""PropertyName"" as Name
from cms.""N_EGOV_MASTER_DATA_RentalProperty""
where ""WardId"" = '{wardId}' and ""PropertyRentingTypeId"" = '{rentingType}' 
and ""Id"" not in (select rm.""PropertyId"" from cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" as rm   
join public.""LOV"" as rps on rm.""RentalPropertyStatus""=rps.""Id"" and rps.""IsDeleted""=false
				  where rps.""Code"" in ('RENTAL_PROP_PENDING','RENTAL_PROP_APPROVED'))";

            var result = await _query.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<EGovRentalViewModel> GetAgreementDetails(string agreementNo)
        {
            var query = $@"Select s.""Id"" as ServiceId, rp.""PropertyId"",rp.""ApplicantsFullName"" as ApplicantName,rp.""RentStartDate"" as AgreementStartDate,rp.""EndDate"" as AgreementEndDate,
rp.""TotalRentAfterConcession"" as RentalAmount, rp.""RentFrequency"",rp.""Tenure"",rp.""TradeBusinessName"",rp.""RentalAgreementNumber"",rp.""PropertyRentingType"",rp.""WardId"",rp.""BuildingNumber"",
rp.""Street"",rp.""LocalitySpecificLocation""
From cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" as rp
Join public.""NtsService"" as s on rp.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where rp.""RentalAgreementNumber""='{agreementNo}' ";

            var result = await _query.ExecuteQuerySingle<EGovRentalViewModel>(query, null);
            return result;
        }
        public async Task<bool> UpdateRenewalEndDate(dynamic udf)
        {
            if (udf != null)
            {
                var query = $@"Update cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" set ""EndDate""='{udf.RenewalEndDate}'
where ""RentalAgreementNumber""='{udf.RentalAgreementNumber}' ";

                await _query.ExecuteCommand(query, null);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateRentalStatus(TaskTemplateViewModel viewModel, dynamic udf)
        {
            var service = new ServiceViewModel();
            var rentalStatusCode = "";

            if (viewModel.ParentServiceId.IsNotNullAndNotEmpty())
            {
                service = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);

                if (service.TemplateCode == "NEW_RENTAL_PROPERTY")
                {
                    if (viewModel.TemplateCode == "DOCUMENT_VERIFIER_RENTAL_PROPERTY" && viewModel.TaskStatusCode == "TASK_STATUS_REJECT")
                    {
                        rentalStatusCode = "RENTAL_PROP_REJECTED";
                    }
                    else if (viewModel.TemplateCode == "PAYMENT_RENT_MODULE" && viewModel.TaskStatusCode == "TASK_STATUS_REJECT")
                    {
                        rentalStatusCode = "RENTAL_PROP_REJECTED";
                    }
                    else
                    {
                        rentalStatusCode = "RENTAL_PROP_APPROVED";
                    }
                }

                if (udf != null)
                {
                    var rentalstatus = await _lOVBusiness.GetSingle(x => x.Code == rentalStatusCode);

                    var query = $@"Update cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" set ""RentalPropertyStatus""='{rentalstatus.Id}'
where ""RentalAgreementNumber""='{udf.RentalAgreementNumber}' ";

                    await _query.ExecuteCommand(query, null);
                    return true;
                }
            }

            if (viewModel.TemplateCode == "NEW_RENTAL_PROPERTY")
            {
                if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
                {
                    rentalStatusCode = "RENTAL_PROP_PENDING";
                }
                else if (viewModel.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
                {
                    rentalStatusCode = "RENTAL_PROP_CANCELLED";
                }

                if (udf != null)
                {
                    var rentalstatus = await _lOVBusiness.GetSingle(x => x.Code == rentalStatusCode);

                    var query = $@"Update cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" set ""RentalPropertyStatus""='{rentalstatus.Id}'
where ""Id""='{udf.UdfNoteTableId}' ";

                    await _query.ExecuteCommand(query, null);
                    return true;
                }
            }

            return false;
        }

        public async Task<CommandResult<OnlinePaymentViewModel>> UpdateOnlinePaymentDetails(OnlinePaymentViewModel model)
        {
            var existquery = $@"Select * from cms.""F_GENERAL_OnlinePayment"" where ""NtsId""='{model.NtsId}' and ""NtsType""='{(int)model.NtsType}' and ""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQuerySingle<OnlinePaymentViewModel>(existquery, null);

            if (result.IsNotNull() && result.PaymentStatusId.IsNotNullAndNotEmpty())
            {
                return CommandResult<OnlinePaymentViewModel>.Instance(model, false, "Your payment has been initiated already");
            }



            var userdetail = await _userBusiness.GetSingleById(model.UserId);
            if (userdetail == null)
            {
                return CommandResult<OnlinePaymentViewModel>.Instance(model, false, "User details is invalid. Please check with administrator");
            }
            var companySettings = await _repo.GetList<CompanySettingViewModel, CompanySetting>();
            //create viewmodel for all params and return this
            if (result != null)
            {
                model.Id = result.Id;
            }
            else
            {
                model.Id = Guid.NewGuid().ToString();
            }

            var date = DateTime.Now.ToDatabaseDateFormat();

            model.EmailId = userdetail.Email;
            model.MobileNumber = userdetail.Mobile;
            if (model.MobileNumber.IsNullOrEmpty())
            {
                model.MobileNumber = "NA";
            }
            if (companySettings != null && companySettings.Any())
            {
                model.MerchantID = companySettings.FirstOrDefault(x => x.Code == "PGWY_MERCHANT_ID")?.Value;
                model.CurrencyType = companySettings.FirstOrDefault(x => x.Code == "PGWY_CURRENCY_TYPE")?.Value;
                model.TypeField1 = companySettings.FirstOrDefault(x => x.Code == "PGWY_TYPE_FIELD_1")?.Value;
                model.SecurityID = companySettings.FirstOrDefault(x => x.Code == "PGWY_SECURITY_ID")?.Value;
                model.ChecksumKey = companySettings.FirstOrDefault(x => x.Code == "PGWY_CHECKSUM_KEY")?.Value;
                model.PaymentGatewayUrl = companySettings.FirstOrDefault(x => x.Code == "PGWY_GATEWAY_URL")?.Value;
                model.PaymentGatewayReturnUrl = companySettings.FirstOrDefault(x => x.Code == "PGWY_GATEWAY_RETURN_URL")?.Value;
               // model.PaymentGatewayReturnUrl = "https://localhost:44389/egov/egovernment/paymentresponse";
            }
            model.TypeField2 = "NA";
            model.Filler1 = "NA";
            model.AdditionalInfo1 = "NA";
            model.AdditionalInfo2 = "NA";
            model.AdditionalInfo3 = "NA";
            model.AdditionalInfo4 = "NA";
            model.AdditionalInfo5 = "NA";
            model.AdditionalInfo6 = "NA";
            model.AdditionalInfo7 = "NA";
            model.Message = String.Concat(model.MerchantID, "|", model.Id, "|", model.Filler1, "|", model.Amount.ToString("#.00"), "|NA|NA|NA|", model.CurrencyType, "|NA|", model.TypeField1, "|", model.SecurityID, "|NA|NA|F|", model.MobileNumber, "|", model.EmailId, "|NA|NA|NA|NA|NA|", model.PaymentGatewayReturnUrl);

            model.ChecksumValue = await GenerateCheckSum(model.ChecksumKey, model.Message);

            model.RequestUrl = String.Concat(model.PaymentGatewayUrl, "?msg=", model.Message, "|", model.ChecksumValue);
            if (result.IsNotNull())
            {
                await UpdateOnlinePayment(model);
            }
            else
            {
                var query = @$"Insert into cms.""F_GENERAL_OnlinePayment"" (""Id"",""CreatedDate"",""CreatedBy"",""LastUpdatedDate"",""LastUpdatedBy"",""CompanyId"",
                ""SequenceOrder"",""VersionNo"",""IsDeleted"",""LegalEntityId"",""Status"",""NtsId"",""NtsType"",""UdfTableId"",""Amount"",""RequestUrl"",""EmailId"",""MobileNumber"",""Message"",""ChecksumValue"",""PaymentGatewayReturnUrl"",""ReturnUrl"")
                Values('{model.Id}','{date}','{model.UserId}','{date}','{model.UserId}','{_repo.UserContext.CompanyId}','1','1',false,'{_repo.UserContext.LegalEntityId}','{(int)StatusEnum.Active}',
                '{model.NtsId}','{(int)model.NtsType}','{model.UdfTableId}','{model.Amount}','{model.RequestUrl}','{model.EmailId}','{model.MobileNumber}','{model.Message}','{model.ChecksumValue}','{model.PaymentGatewayReturnUrl}','{model.ReturnUrl}' )";
                await _query.ExecuteCommand(query, null);
            }



            // return commandresult - if paymentstatus is having value then return message with payment initiated and status
            return CommandResult<OnlinePaymentViewModel>.Instance(model);

        }
        public async Task UpdateOnlinePayment(OnlinePaymentViewModel responseViewModel)
        {
            var query = $@"Update cms.""F_GENERAL_OnlinePayment"" set ""LastUpdatedDate""='{DateTime.Now}',
            ""LastUpdatedBy""='{_repo.UserContext.UserId}', ""NtsId""='{responseViewModel.NtsId}',""NtsType""='{responseViewModel.NtsType}',
            ""UdfTableId""='{responseViewModel.UdfTableId}',""Amount""='{responseViewModel.Amount}',""PaymentStatusId""='{responseViewModel.PaymentStatusId}',""ChecksumValue""='{responseViewModel.ChecksumValue}',
            ""RequestUrl""='{responseViewModel.RequestUrl}',""ResponseUrl""='{responseViewModel.ResponseUrl}',""ChecksumKey""='{responseViewModel.ChecksumKey}',""ReturnUrl""='{responseViewModel.ReturnUrl}',""Message""='{responseViewModel.Message}',
            ""PaymentGatewayURL""='{responseViewModel.PaymentGatewayUrl}',""EmailId""='{responseViewModel.EmailId}',""MobileNumber""='{responseViewModel.MobileNumber}',""PaymentReferenceNo""='{responseViewModel.PaymentReferenceNo}',
            ""PaymentGatewayReturnUrl""='{responseViewModel.PaymentGatewayReturnUrl}',""ResponseMessage""='{responseViewModel.ResponseMessage}',""ResponseChecksumValue""='{responseViewModel.ResponseChecksumValue}',""ResponseErrorCode""='{responseViewModel.ResponseErrorCode}',""ResponseError""='{responseViewModel.ResponseError}',
            ""AuthStatus""='{responseViewModel.AuthStatus}'
            where ""Id""='{responseViewModel.Id}'";
            await _queryRepo1.ExecuteCommand(query, null);
        }
        private async Task<string> GenerateCheckSum(string key, string text)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            var hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex.ToUpper();
        }

        public async Task<OnlinePaymentViewModel> GetOnlinePayment(string id)
        {
            var existquery = $@"Select * from cms.""F_GENERAL_OnlinePayment"" where ""Id""='{id}' and ""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQuerySingle<OnlinePaymentViewModel>(existquery, null);
            return result;
        }
        public async Task<List<EGovCommunityHallViewModel>> GetCommunityHallList()
        {

            var Query = $@"Select ch.""CommunityHallName"",ch.""ChargesLeviedPerDay"",ch.""WardIds"" as Ward
from cms.""N_EGOV_MASTER_DATA_CommunityHallName"" as ch
where ch.""IsDeleted""=false
--left join public.""LOV"" as lov on ch.""WardIds""=lov.""Id"" and lov.""IsDeleted""=false ";

            var result = await _querych.ExecuteQueryList<EGovCommunityHallViewModel>(Query, null);
            
            if (result.IsNotNull())
            {
                foreach (var item in result)
                {
                    if (item.Ward.IsNotNullAndNotEmpty())
                    {
                        var wardids = item.Ward.Trim('[', ']');
                        wardids = wardids.Replace("\"", "\'");
                        var query1 = $@" select string_agg(lov.""Name""::text, ', ') as ""WardName""
                                    from public.""LOV"" as lov
                                    where lov.""IsDeleted"" = false and lov.""Id"" IN ({wardids})
                                    ";
                        var data = await _querydata.ExecuteScalar<string>(query1, null);
                        if (data.IsNotNullAndNotEmpty())
                        {
                            item.WardName = data;
                        }
                    }
                }
            }
            return result;

 //           foreach (var i in result)
            
 //           {
 //               var wids = i.Ward.Split(",");
 //            var s = i.Ward.Replace("[", "").Replace("]", "").Replace("/", "").Replace("\", "");
 //               //var ts = JsonConvert.DeserializeObject(i.Ward);
 //               //var wids = String.Join(",", i.Ward);
 //               //var wids = string.Join("','", i.Ward);

 //               var wardquery = $@"SELECT lov.""Id"" as Id, lov.""Name"" as WardName FROM public.""LOV"" as lov

 //where lov.""Id"" in (" + s + ")";

 //               var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(wardquery, null);
 //               var wardid = queryData.Select(x => x.Name);
 //               var wid = string.Join("','", wardid);
 //               //sids += $"{i},";
 //               i.WardName = wid;
 //           }
           

          
 //           return result;

        }
    }
}
