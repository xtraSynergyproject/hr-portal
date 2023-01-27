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

namespace Synergy.App.Business
{
    public class EGovernanceQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IEGovernanceQueryBusiness
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
        private readonly IUserContext _userContext;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        public EGovernanceQueryPostgreBusiness(IRepositoryQueryBase<IdNameViewModel> querydata,
            IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<EGovCommunityHallViewModel> querych
            , IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<TaskViewModel> queryRepo1,
            IRepositoryQueryBase<ServiceTemplateViewModel> query, INoteBusiness noteBusiness, IUserContext userContext,
            ILOVBusiness lOVBusiness, IServiceBusiness serviceBusiness, IUserBusiness userBusiness,
            ITemplateCategoryBusiness templateCategoryBusiness) : base(repo, autoMapper)
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
            _userContext = userContext;
            _templateCategoryBusiness = templateCategoryBusiness;

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

            return result;

        }
        public async Task<List<IdNameViewModel>> GetEGovAdminConstituencyIdNameList()
        {
            var query = $@" select n.""Id"",concat(n.""ConstituencyNo"",'-',n.""ConstituencyName"") as Name  
                            from cms.""F_EGOVERNANCE_AdminConstituency"" as n
                            where n.""IsDeleted""=false";

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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
            return result;
        }
        public async Task<EGovCorporationWardViewModel> GetWardListQuery(string s)
        {
            var wardquery = $@"SELECT string_agg(concat(cw.""WardNo""::text,' ',cw.""WardName""::text), ',') as ""WardName"" 

          FROM cms.""N_E_GOV_CorporationWard"" cw
         where cw.""IsDeleted"" = false and cw.""Id"" in (" + s + ")";
            var corpwardname = await _querych.ExecuteQuerySingle<EGovCorporationWardViewModel>(wardquery, null);
            return corpwardname;
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

        public async Task<List<EgovGISLocationViewModel>> GetGISLocationDetails()
        {
            var query = $@"SELECT l.""Id"", l.""Name"",l.""Address"",l.""Latitude"",l.""Longitude"" 
                            FROM cms.""N_E_GOV_EgovGISLocation"" as l";
            var list = await _query.ExecuteQueryList<EgovGISLocationViewModel>(query, null);
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
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCNotificationData()
        {
            DateTime curDate = System.DateTime.Today.Date;
            var query = $@" select n.""NotificationSubject"" as BannerContent,n.""StartDate"" as StartDate, n.""EndDate"" as EndDate,n.""SequenceNo"" as SequenceNo,n.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_Notifications"" as n
                        where n.""IsDeleted""=false and n.""NotificationStatus""='{(int)StatusEnum.Active}' and n.""StartDate""::Date<='{curDate}'::Date and n.""EndDate""::Date>='{curDate}'::Date
                        Order by n.""SequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCTenderData()
        {
            DateTime curDate = System.DateTime.Today.Date;
            var query = $@" select n.""TenderSubject"" as BannerContent,n.""StartDate"" as StartDate, n.""EndDate"" as EndDate,n.""SequenceNo"" as SequenceNo,n.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_Tenders"" as n
                        where n.""IsDeleted""=false and n.""TenderStatus""='{(int)StatusEnum.Active}' and n.""StartDate""::Date<='{curDate}'::Date and n.""EndDate""::Date>='{curDate}'::Date
                        Order by n.""SequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCOrderData()
        {
            DateTime curDate = System.DateTime.Today.Date;
            var query = $@" select n.""OrderSubject"" as BannerContent,n.""StartDate"" as StartDate, n.""EndDate"" as EndDate,n.""SequenceNo"" as SequenceNo,n.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_Orders"" as n
                        where n.""IsDeleted""=false and n.""OrderStatus""='{(int)StatusEnum.Active}' and n.""StartDate""::Date<='{curDate}'::Date and n.""EndDate""::Date>='{curDate}'::Date
                        Order by n.""SequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCCircularData()
        {
            DateTime curDate = System.DateTime.Today.Date;
            var query = $@" select n.""CircularSubject"" as BannerContent,n.""StartDate"" as StartDate, n.""EndDate"" as EndDate,n.""SequenceNo"" as SequenceNo,n.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_Circulars"" as n
                        where n.""IsDeleted""=false and n.""CircularStatus""='{(int)StatusEnum.Active}' and n.""StartDate""::Date<='{curDate}'::Date and n.""EndDate""::Date>='{curDate}'::Date
                        Order by n.""SequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }

        public async Task<List<EGOVBannerViewModel>> GetEGovSSCDownloadsData()
        {
            DateTime curDate = System.DateTime.Today.Date;
            var query = $@" select n.""DownloadSubject"" as BannerContent,n.""StartDate"" as StartDate, n.""EndDate"" as EndDate,n.""SequenceNo"" as SequenceNo,n.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_Downloads"" as n
                        where n.""IsDeleted""=false and n.""DownloadStatus""='{(int)StatusEnum.Active}' and n.""StartDate""::Date<='{curDate}'::Date and n.""EndDate""::Date>='{curDate}'::Date
                        Order by n.""SequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }

        public async Task<List<EGOVBannerViewModel>> GetEGovSSCOrderCircularData()
        {
            DateTime curDate = System.DateTime.Today.Date;
            var query = $@" 
                        select 'SSC_ORDERS' as BannerType,o.""OrderSubject"" as BannerContent,o.""StartDate"" as StartDate, o.""EndDate"" as EndDate,o.""SequenceNo"" as SequenceNo,o.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_Orders"" as o
                        where o.""IsDeleted""=false and o.""OrderStatus""='{(int)StatusEnum.Active}' and o.""StartDate""::Date<='{curDate}'::Date and o.""EndDate""::Date>='{curDate}'::Date
                        --Order by o.""SequenceNo""
                        union
                        select 'SSC_CIRCULARS' as BannerType,c.""CircularSubject"" as BannerContent,c.""StartDate"" as StartDate, c.""EndDate"" as EndDate,c.""SequenceNo"" as SequenceNo,c.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_Circulars"" as c
                        where c.""IsDeleted""=false and c.""CircularStatus""='{(int)StatusEnum.Active}' and c.""StartDate""::Date<='{curDate}'::Date and c.""EndDate""::Date>='{curDate}'::Date
                        --Order by c.""SequenceNo"" 
                        ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            if (list.Count>0)
            {
                list = list.OrderBy(x=>x.StartDate).ToList();
            }
            return list.ToList(); 
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCNewsData()
        {
            DateTime curDate = System.DateTime.Today.Date;
            var query = $@" select n.""NewsSubject"" as BannerContent,n.""StartDate"" as StartDate, n.""EndDate"" as EndDate,n.""SequenceNo"" as SequenceNo,n.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_NewsBoard"" as n
                        where n.""IsDeleted""=false and n.""NewsStatus""='{(int)StatusEnum.Active}' and n.""StartDate""::Date<='{curDate}'::Date and n.""EndDate""::Date>='{curDate}'::Date
                        Order by n.""SequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCPublicationData()
        {
            DateTime curDate = System.DateTime.Today.Date;
            var query = $@" select n.""PublicationSubject"" as BannerContent,n.""StartDate"" as StartDate, n.""EndDate"" as EndDate,n.""SequenceNo"" as SequenceNo,n.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_Publications"" as n
                        where n.""IsDeleted""=false and n.""PublicationStatus""='{(int)StatusEnum.Active}' and n.""StartDate""::Date<='{curDate}'::Date and n.""EndDate""::Date>='{curDate}'::Date
                        Order by n.""SequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<List<EGovCorporationWardViewModel>> GetAdministrativeWardsList()
        {

            var Query = $@" SELECT aw.""AdminWardNo"" as WardNo,concat(aw.""AdminWardNo"",'-',aw.""AdminWardName"") as WardName,
                            trim(both '[ ]' from aw.""ElectoralWardId"") as ElectoralWardId
                            ,aw.""Location"" as Location,aw.""Longitude"" as Longitude,aw.""Latitude"" as Latitude
                            ,aw.""AdminWardOfficerName"" as OffficerName,aw.""AdminWardOfficerPhoneNo"" as Mobile
                            FROM cms.""F_EGOVERNANCE_AdministrativeWards"" aw
                    	    where aw.""IsDeleted""=false
                            order by aw.""AdminWardNo""::int
                        ";

            var result = await _querych.ExecuteQueryList<EGovCorporationWardViewModel>(Query, null);
            if (result.IsNotNull() && result.Count>0) {
                foreach (var item in result)
                {
                    var corporateWardIds = item.ElectoralWardId.Split(",");
                    var s = item.ElectoralWardId.Replace("\"", "'");
                    var mapwardconcname = await GetWardConstituencyMappingList(s);
                    if (mapwardconcname.IsNotNull())
                    {
                        item.ElectoralWardName = mapwardconcname.WardName;
                    }

                }
            }
            return result;
        }
        public async Task<EGovCorporationWardViewModel> GetWardConstituencyMappingList(string wardIds)
        {
            var wardquery = $@"SELECT string_agg(concat(w.""Name""::text,' (',con.""ConstituencyNo""::text,'-',con.""ConstituencyName"",')'), ', ') as ""WardName"" 
                            FROM public.""LOV"" as w
                            left join cms.""F_EGOVERNANCE_ElectoralWardConstituencyMapping"" as map on map.""ElectoralWardId""=w.""Id"" and map.""IsDeleted""=false
                            left join cms.""F_EGOVERNANCE_AdminConstituency"" as con on con.""Id""=map.""ConstituencyId"" and con.""IsDeleted""=false
         where w.""IsDeleted"" = false and w.""Id"" in (" + wardIds + ")";
            var corpwardname = await _querych.ExecuteQuerySingle<EGovCorporationWardViewModel>(wardquery, null);
            return corpwardname;
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCWardMapsList()
        {
            var query = $@"select w.""Id"" as WardId,w.""Name"" as WardName, n.""WardMapId"" as FileId
                        , w.""SequenceOrder"" as  SequenceNo
                        from public.""LOV"" as w
                        left join cms.""F_EGOVERNANCE_Corporators"" as n on n.""WardId""=w.""Id"" and n.""IsDeleted""=false
                        where w.""IsDeleted""=false and w.""LOVType""='EGOV_ELECTORAL_WARD'
                        order by  w.""SequenceOrder""  ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<EGOVBannerViewModel> GetSrinagarSettingData(string code)
        {
            var query = $@"select n.""Name"", n.""Code"", n.""Value"", n.""FileId""
                        from cms.""F_EGOVERNANCE_SrinagarSetting"" as n
                        where n.""IsDeleted""=false and n.""Code""='{code}'
                        ";
            var result = await _query.ExecuteQuerySingle<EGOVBannerViewModel>(query, null);
            return result;
        }
        public async Task<List<EGOVBannerViewModel>> GetSSCActnByeLawsData(string val)
        {
            var query = $@"select n.""Name"", n.""Code"", n.""Value"", n.""AttachmentId"" as FileId
                        from cms.""F_EGOVERNANCE_ActnByeLawSettings"" as n
                        where n.""IsDeleted""=false and n.""Value""='{val}'
                        ";
            var result = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return result;
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCCorporatorsData()
        {
            var query = $@"select n.""WardId"" as WardId,w.""Name"" as WardName, n.""Designation"" as Designation, n.""CouncillorName"" as Name
                        , n.""CouncillorFatherName"" as FHName, n.""Address"" as Address, n.""PhoneNo"" as PhoneNo, n.""PhotoId"" as FileId
                        , n.""SequenceNo"" as  SequenceNo
                        from cms.""F_EGOVERNANCE_Corporators"" as n
                        left join public.""LOV"" as w on w.""Id""=n.""WardId"" and w.""IsDeleted""=false
                        where n.""IsDeleted""=false and n.""CorporatorStatus""='{(int)StatusEnum.Active}'
                        order by  n.""SequenceNo""::int  ";
            var list = await _query.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return list.ToList();
        }
        public async Task<EGOVCommitteeMasterViewModel> GetEGovSSCCommitteeMasterData(string committeeCode)
        {
            var query = $@" select n.""Id"" as CommitteeId, n.""CommitteeTitle"", n.""CommitteeCode"", n.""CommitteeName"", n.""ConstitutedUnder""
                            , n.""SSCCommitteeMembers"", n.""SSCCommitteeFunctions""
                        from cms.""F_EGOVERNANCE_CommitteeMaster"" as n
                        where n.""IsDeleted""=false and n.""CommitteeCode""='{committeeCode}'
                        ";
            var result = await _query.ExecuteQuerySingle<EGOVCommitteeMasterViewModel>(query, null);
            return result;
        }
        public async Task<List<EGOVCommitteeMemberViewModel>> GetEGovSSCCommitteeMemberListByCommitteeId(string committeeId)
        {
            var query = $@" select n.""MemberName"", n.""MemberDesignation"", n.""MemberSequenceNo""
                            from cms.""F_EGOVERNANCE_SSCCommitteeMembers"" as n
                            where n.""IsDeleted""=false and n.""ParentId""='{committeeId}' 
                            order by n.""MemberSequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVCommitteeMemberViewModel>(query, null);
            return list;
            
        }
        public async Task<List<EGOVCommitteeFunctionViewModel>> GetEGovSSCCommitteeFunctionListByCommitteeId(string committeeId)
        {
            var query = $@" select n.""CommitteeFunction"", n.""FunctionSequenceNo""
                            from cms.""F_EGOVERNANCE_SSCCommitteeFunctions"" as n
                            where n.""IsDeleted""=false and n.""ParentId""='{committeeId}' 
                            order by n.""FunctionSequenceNo"" ";
            var list = await _query.ExecuteQueryList<EGOVCommitteeFunctionViewModel>(query, null);
            return list;

        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCCommitteeMemberData(string committeeCode)
        {
            var query = $@" select n.""MemberName"" as Name,n.""MemberDesignation"" as Designation, n.""SequenceNo"" as  SequenceNo
                        from cms.""F_EGOVERNANCE_CommitteeMember"" as n
                        left join public.""LOV"" as c on c.""Id""=n.""CommitteeId"" and c.""IsDeleted""=false
                        where n.""IsDeleted""=false and n.""MemberStatus""='{(int)StatusEnum.Active}'
                        and c.""Code""='{committeeCode}'
                        order by n.""SequenceNo""
                        ";
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
        public async Task<double> GetEGovDebrisRateData()
        {
            var today = DateTime.Today;
            var query = $@"select n.""DebrisRate"" as amount from cms.""F_EGOVERNANCE_DEBRISRATE"" as n
                        where n.""IsDeleted""=false and n.""EffectiveDate""::Date<='{today}'::Date
                        order by n.""EffectiveDate"" desc limit 1 ";
            var data = await _query.ExecuteScalar<double>(query,null);
            return data;
        }
        public async Task<double> GetEGovPoultryCostData()
        {
            var today = DateTime.Today;
            var query = $@"select n.""AverageCost"" as amount from cms.""F_EGOVERNANCE_POULTRYCOST"" as n
                        where n.""IsDeleted""=false and n.""EffectiveDate""::Date<='{today}'::Date
                        order by n.""EffectiveDate"" desc limit 1 ";
            var data = await _query.ExecuteScalar<double>(query, null);
            return data;
        }
        public async Task<double> GetEGovSepticTankCostData()
        {
            var today = DateTime.Today;
            var query = $@"select n.""AverageCost"" as amount from cms.""F_EGOVERNANCE_SEPTICTANKCOST"" as n
                        where n.""IsDeleted""=false and n.""EffectiveDate""::Date<='{today}'::Date
                        order by n.""EffectiveDate"" desc limit 1 ";
            var data = await _query.ExecuteScalar<double>(query, null);
            return data;
        }
        public async Task<double> GetEGovBinBookingCostData(string binSizeId)
        {
            var today = DateTime.Today;
            var query = $@"select n.""AverageCost"" as amount from cms.""F_EGOVERNANCE_BINBOOKINGCOST"" as n
                        join public.""LOV"" as bs on bs.""Id""=n.""BinSize"" and bs.""IsDeleted""=false
                        where n.""IsDeleted""=false and n.""BinSize""='{binSizeId}' and n.""EffectiveDate""::Date<='{today}'::Date
                        order by n.""EffectiveDate"" desc limit 1 ";
            var data = await _query.ExecuteScalar<double>(query, null);
            return data;
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
        public async Task<IList<ServiceTemplateViewModel>> GetDraftedServiceList(string statusCodes, string templateCode, string categoryCode)
        {
            var query = $@"Select  s.""Id"" as ServiceId, s.""ServiceNo"" as ServiceNo, t.""DisplayName"" as TemplateDisplayName,t.""Code"" as TemplateCode,   
ss.""Name"" as ServiceStatusName,ss.""Code"" as ServiceStatusCode, s.""CreatedDate"" as CreatedDate
from public.""NtsService"" as s
join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false 
join public.""Template"" as t on t.""Id"" =s.""TemplateId"" and t.""IsDeleted""=false #TEMPCODEWHERE#
join public.""TemplateCategory"" as tc on tc.""Id"" =t.""TemplateCategoryId"" and tc.""IsDeleted""=false #CATCODEWHERE#
join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false #STATUSWHERE#
left join public.""Module"" as m on m.""Id"" =t.""ModuleId"" and m.""IsDeleted""=false 
where s.""IsDeleted""=false and (s.""RequestedByUserId""='{_repo.UserContext.UserId}' or s.""OwnerUserId""='{_repo.UserContext.UserId}') 
 order by s.""CreatedDate"" desc ";

            var temcode = "";
            if (templateCode.IsNotNullAndNotEmpty())
            {
                temcode = $@" and t.""Code""='{templateCode}' ";
            }
            query = query.Replace("#TEMPCODEWHERE#", temcode);
            var catcode = "";
            if (categoryCode.IsNotNullAndNotEmpty())
            {
                catcode = $@" and tc.""Code""in ('{categoryCode.Replace(",", "','")}')";
            }
            query = query.Replace("#CATCODEWHERE#", catcode);

            var status = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                status = $@" and ss.""Code"" in ('{statusCodes.Replace(",", "','")}')";
            }
            query = query.Replace("#STATUSWHERE#", status);
            //var portal = "";
            //if (portalId.IsNotNullAndNotEmpty())
            //{
            //    portal = $@" and s.""PortalId"" in ('{portalId.Replace(",", "','")}')";
            //}
            //query = query.Replace("#PORTALWHERE#", portal);


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
UNION
  Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""Amount"" as ""ServiceCost"",'' as NoteNo
                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                            
                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                             Join cms.""N_JSC_REV_AssetPayment"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" ='JSC_AssetPayment_AssetBillPaymentGenerate' and t.""IsDeleted""=false 
--UNION
--  Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
--                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""Amount"" as ""ServiceCost"",'' as NoteNo
--                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
--                            From public.""NtsTask"" as t
--                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
--                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                            
--                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
--                             Join cms.""N_SNC_JSC_BOOKING_PERMISSION_JSCHoardingBooking"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
--                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
--                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
--                      and t.""TemplateCode"" ='HOARDING_BOOKING_PAYMENT' and t.""IsDeleted""=false
--UNION
--  Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
--                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""Amount"" as ""ServiceCost"",'' as NoteNo
--                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
--                            From public.""NtsTask"" as t
--                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
--                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                            
--                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
--                             Join cms.""N_SNC_SANITATION_SERVICE_BIN_BOOKING_JSC"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
--                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
--                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
--                      and t.""TemplateCode"" ='JSC_PAYMENT_BIN_BOOKING' and t.""IsDeleted""=false

-- UNION
--                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
--                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""Amount"" as ""ServiceCost"",b.""PoultryWasteServiceNumber"" as NoteNo
--                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
--                            From public.""NtsTask"" as t
--                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
--                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                             
--                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
--                             Join cms.""N_SNC_SANITATION_SERVICE_JSC_PoultryWasteForm"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
--                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
--                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
--                      and t.""TemplateCode""='JSC_POULTRY_WASTE_PAYMENT' and t.""IsDeleted""=false

--UNION
--                       Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
--                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""Amount"" as ""ServiceCost"",'' as NoteNo
--                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
--                            From public.""NtsTask"" as t
--                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
--                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                            
--                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
--                             Join cms.""N_SNC_SANITATION_SERVICE_JSC_SepticTankClearanceForm"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
--                                left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
--                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
--                      and t.""TemplateCode"" = 'JSC_PAYMENT_SEPTIC_CLEARANCE' and t.""IsDeleted""=false

--UNION
--  Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
--                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""Amount"" as ""ServiceCost"",'' as NoteNo
--                            ,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
--                            From public.""NtsTask"" as t
--                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
--                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
                            
--                              Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
--                             Join cms.""N_SNC_JSC_SANITATION_SERVICE_JSC_ConstructionandDebrisWasteClearanceForm"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
--                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
--                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
--                      and t.""TemplateCode"" in ('JSC_CND_PAYMENT','JSC_CND_CITIZEN_POST_PAYMENT') and t.""IsDeleted""=false
--UNION
--  Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
--                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""Amount"" as ""ServiceCost"",'' as NoteNo
--,ts.""GroupCode"" as ""StatusGroupCode"" ,ps.""Name"" as ""PaymentStatus"" ,ps.""Code"" as ""PaymentStatusCode"",b.""PaymentReferenceNo"" as ""ReferenceNumber""
--                            From public.""NtsTask"" as t
--                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
--                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false  
--                            Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
--                            Join cms.""N_SNC_JSC_BOOKING_PERMISSION_JSCCommunityHallBooking"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
--                              left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false
--                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
--                      and t.""TemplateCode"" in ('PaymentJSC1','JSC_PAYMENT') and t.""IsDeleted""=false 

";
            var result = await _queryRepo1.ExecuteQueryList(query, null);
            //foreach (var i in result)
            //{
            //    i.DisplayDueDate = i.DueDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            //    i.DisplayStartDate = i.StartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            //}
            if (!_repo.UserContext.IsSystemAdmin)
            {
                if (result != null && result.Count > 0)
                {
                    result = result.Where(x => x.RequestedByUserId == _repo.UserContext.UserId).ToList();
                }
            }
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

        public async Task<bool> UpdateRentalStatus(string rentalStatusId, string id)
        {

            var query = $@"Update cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" set ""RentalPropertyStatus""='{rentalStatusId}'
            where ""RentalAgreementNumber""='{id}' ";
            await _query.ExecuteCommand(query, null);
            return true;


        }

        public async Task<bool> RentalStatus(string rentalStatusId, string id)
        {
                                   var query = $@"Update cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" set ""RentalPropertyStatus""='{rentalStatusId}'
                                   where ""Id""='{id}' ";

                           await _query.ExecuteCommand(query, null);
                            return true;
        }

        public async Task<OnlinePaymentViewModel> UpdateOnlinePaymentDetails(OnlinePaymentViewModel model)
        {
            var existquery = $@"Select * from cms.""F_GENERAL_OnlinePayment"" where ""NtsId""='{model.NtsId}' and ""NtsType""='{(int)model.NtsType}' and ""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQuerySingle<OnlinePaymentViewModel>(existquery, null);
            return result;
        }

        public async Task UpdateOnlinePaymentDetailsData(OnlinePaymentViewModel model, string date)
        {
            var query = @$"Insert into cms.""F_GENERAL_OnlinePayment"" (""Id"",""CreatedDate"",""CreatedBy"",""LastUpdatedDate"",""LastUpdatedBy"",""CompanyId"",
                    ""SequenceOrder"",""VersionNo"",""IsDeleted"",""LegalEntityId"",""Status"",""NtsId"",""NtsType"",""UdfTableId"",""Amount"",""RequestUrl"",""EmailId"",""MobileNumber"",""Message"",""ChecksumValue"",""PaymentGatewayReturnUrl"",""ReturnUrl"")
                    Values('{model.Id}','{date}','{model.UserId}','{date}','{model.UserId}','{_repo.UserContext.CompanyId}','1','1',false,'{_repo.UserContext.LegalEntityId}','{(int)StatusEnum.Active}',
                    '{model.NtsId}','{(int)model.NtsType}','{model.UdfTableId}','{model.Amount}','{model.RequestUrl}','{model.EmailId}','{model.MobileNumber}','{model.Message}','{model.ChecksumValue}','{model.PaymentGatewayReturnUrl}','{model.ReturnUrl}' )";
            await _query.ExecuteCommand(query, null);
            
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
        public async Task<List<EGovProjectViewModel>> GetUpcomingProjectList(string categoryId, string wardId)
        {
            var query = $@"select proj.*,s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""CreatedDate"" as RequestedDate,u.""Name"" as RequestedBy
                            ,plov.""Name"" as Status,slov.""Name"" as ServiceStatus,clov.""Name"" as ProjectCategoryName,wlov.""Name"" as ProjectWardName
                            ,proj.""IsProposedByCitizen"" as ""IsProposedByCitizen""
                            FROM public.""NtsService"" as s
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""Code""='EGOV_PROJECT_PROPOSAL' and  tt.""IsDeleted""=false 
				            join cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as proj on proj.""NtsNoteId""=s.""UdfNoteId"" and proj.""IsDeleted""=false
                            left join public.""User"" as u on u.""Id""=s.""CreatedBy"" and u.""IsDeleted""=false
				            left join public.""LOV"" as slov on slov.""Id""=s.""ServiceStatusId"" and slov.""IsDeleted""=false
				            left join public.""LOV"" as plov on plov.""Id""=proj.""ProjectStatus"" and plov.""IsDeleted""=false
				            left join public.""LOV"" as clov on clov.""Id""=proj.""ProjectCategory"" and clov.""IsDeleted""=false
				            left join public.""LOV"" as wlov on wlov.""Id""=proj.""ProjectWard"" and wlov.""IsDeleted""=false
                            where s.""IsDeleted""=false #WHERECATEGORY# #WHEREWARD#  order by s.""CreatedDate"" desc ";
            var wherecategory = "";
            if (categoryId.IsNotNullAndNotEmpty())
            {
                wherecategory = $@" and proj.""ProjectCategory""='{categoryId}' ";
            }
            var whereward = "";
            if (wardId.IsNotNullAndNotEmpty())
            {
                whereward = $@" and proj.""ProjectWard""='{wardId}' ";
            }
            query = query.Replace("#WHERECATEGORY#", wherecategory);
            query = query.Replace("#WHEREWARD#", whereward);
            var result = await _querych.ExecuteQueryList<EGovProjectViewModel>(query, null);
            return result;
        }
        public async Task<bool> DeleteUpcomingProject(string projectId)
        {
            var query = $@"update cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal""
                            set ""IsDeleted""=true
                        where ""Id""='{projectId}' ";
            await _querych.ExecuteCommand(query,null);
            return true;
        }
        public async Task<List<EGovCommunityHallViewModel>> GetCommunityHallList(){
	var Query = $@"Select ch.""CommunityHallName"",ch.""ChargesLeviedPerDay"",ch.""WardIds"" as Ward, ch.""PhotoId"" 
from cms.""N_EGOV_MASTER_DATA_CommunityHallName"" as ch
where ch.""IsDeleted""=false order by ch.""CommunityHallName""
--left join public.""LOV"" as lov on ch.""WardIds""=lov.""Id"" and lov.""IsDeleted""=false ";

            var result = await _querych.ExecuteQueryList<EGovCommunityHallViewModel>(Query, null);
			return result;
 }
 
 public async Task<string> GetWardList(string wardids)
        {
	var query1 = $@" select string_agg(lov.""Name""::text, ', ') as ""WardName""
                                    from public.""LOV"" as lov
                                    where lov.""IsDeleted"" = false and lov.""Id"" IN ({wardids})
                                    ";
                        var data = await _querydata.ExecuteScalar<string>(query1, null);
						return data;
 }
        public async Task<List<NewDashboardViewModel>> GetDashboardList()
        {

            var Query = $@"select * from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" where ""IsDeleted""=false";

            var result = await _querych.ExecuteQueryList<NewDashboardViewModel>(Query, null);
            return result;

        }

        public async Task<List<EGovDashboardViewModel>> GetProposalProjectsList(string type,string userId)
        {

            //var Query = $@"Select nw.*,pr.""ResponseType"" from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as nw 
            //            join public.""NtsService"" as s on nw.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
            //            join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
            //            left join cms.""F_EGOVERNANCE_ProjectProposalResponse"" as pr on nw.""Id""=pr.""ProjectProposalId"" and pr.""ResponseByUserId""='{userId}' and pr.""IsDeleted""=false
            //            where nw.""IsDeleted""=false #TYPE#";

            var Query = $@"select distinct nw.*,pc.""Name"" as ProjectCategoryName,pr.ResponseType,s.""Id"" as ServiceId,s.""TemplateCode"" as TemplateCodes,s.""OwnerUserId"" as UserId,
                            s.""ServiceNo"",ss.""Name"" as ""ProjectStatus"",s.""CreatedDate""::TIMESTAMP::DATE as ""RequestedDate"",s.""CreatedDate"", #COLS#
                            COALESCE(lc.LikesCount,0) as LikesCount,COALESCE(dc.DislikesCount,0) as DislikesCount,sc.CommentsCount,
                            coalesce(s.""WorkflowStatus"",serst.""Name"") as WorkflowStatus,
                            case when ss.""Code""='EGOV_PRO_STATUS_PROPOSED' then 'border-info'
                            when ss.""Code""='EGOV_PRO_STATUS_UNDERTAKEN' then 'border-primary'
                            when ss.""Code""='EGOV_PRO_STATUS_REJECTED' then 'border-danger'
                            when ss.""Code""='EGOV_PRO_STATUS_ON_HOLD' then 'border-warning'
                            when ss.""Code""='EGOV_PRO_STATUS_COMPLETED' then 'border-success'
                            end as BorderCSS,ss.""Code"" as ProjectStatusCode
                            from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as nw
                            join public.""NtsService"" as s on nw.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                            join public.""LOV"" as serst on s.""ServiceStatusId""=serst.""Id"" and serst.""IsDeleted""=false
                            join public.""LOV"" as ss on nw.""ProjectStatus""=ss.""Id"" and ss.""IsDeleted""=false
                            join public.""LOV"" as pc on nw.""ProjectCategory""=pc.""Id"" and pc.""IsDeleted""=false
                            #JOINTYPE# join cms.""F_EGOVERNANCE_UserDetails"" as ud on ud.""WardId""=nw.""ProjectWard"" --and ud.""UserId""='{_userContext.UserId}' and ud.""IsDeleted""=false

                            left join(select ""ProjectProposalId"",""ResponseByUserId"", case when ""ResponseType""='1' then '1'
                                        when ""ResponseType""='2' then '2' end as ResponseType
	                                    from cms.""F_EGOVERNANCE_ProjectProposalResponse""
                                  ) as pr on nw.""Id""=pr.""ProjectProposalId"" #USERWHERE#
                            left join(select ""ProjectProposalId"", count(""Id"") as LikesCount 
		                        from cms.""F_EGOVERNANCE_ProjectProposalResponse""
		                        where ""ResponseType""='1' group by ""ProjectProposalId"") as lc on lc.""ProjectProposalId""=nw.""Id""
                            left join(select ""ProjectProposalId"", count(""Id"") as DislikesCount 
		                        from cms.""F_EGOVERNANCE_ProjectProposalResponse""
		                        where ""ResponseType""='2' group by ""ProjectProposalId"") as dc on dc.""ProjectProposalId""=nw.""Id""
                            left join(select ""NtsServiceId"",count(""Id"") as CommentsCount
		                        from public.""NtsServiceComment"" where ""IsDeleted""=false 
		                        group by ""NtsServiceId"") as sc on s.""Id""=sc.""NtsServiceId""
                            where nw.""IsDeleted""=false #TYPE# order by s.""CreatedDate"" desc ";

            string typewhere = "";
            string joinType = "";
            string cols = "";
            if (type == "PROPOSEDBYME")
            {
                typewhere = $@" and nw.""IsProposedByCitizen""='true' and s.""OwnerUserId""='{userId}'  ";
                joinType = $@" left ";
                cols = $@" case when nw.""Level"" = 'C' then 'City' else 'Ward' end as Message,";
            }
            else if(type == "PROPOSEDBYOTHER")
            {
                typewhere = $@"  and serst.""Code""='SERVICE_STATUS_COMPLETE' and nw.""IsProposedByCitizen""='true' and s.""OwnerUserId""!='{userId}'  ";
            }
            else if (type == "PROPOSEDBYCITIZEN")
            {
                typewhere = $@" and nw.""IsProposedByCitizen""='true' ";
            }
            else if (type == "PROJECTSUNDERTAKEN")
            {
                typewhere = $@" and nw.""IsProposedByCitizen""='false' ";
            }
            else if(type == "PROJECTSUNDERTAKENCITIZEN")
            {
                typewhere = $@" and nw.""IsProposedByCitizen""='false' and s.""OwnerUserId""!='{userId}'  ";
            }
            Query = Query.Replace("#TYPE#", typewhere);
            Query = Query.Replace("#JOINTYPE#", joinType);
            Query = Query.Replace("#COLS#", cols);

            string userwhere = userId.IsNotNullAndNotEmpty() ? $@" and pr.""ResponseByUserId""='{userId}'" : "";
            Query = Query.Replace("#USERWHERE#", userwhere);

            var result = await _querych.ExecuteQueryList<EGovDashboardViewModel>(Query, null);
           
            return result;

        }



        public async Task<EGovProjectProposalResponseViewModel> GetProposalLikesData(string proposalId, ProjectPropsalResponseEnum type, string userId)
        {
            var query = $@"Select * from cms.""F_EGOVERNANCE_ProjectProposalResponse""
                          where ""ProjectProposalId""='{proposalId}' and ""ResponseByUserId""='{userId}' and ""IsDeleted""=false  ";

            return await _queryRepo.ExecuteQuerySingle<EGovProjectProposalResponseViewModel>(query, null);

        }

        public async Task UpdateProjectProposalLikes(string proposalId, ProjectPropsalResponseEnum? type, string userId, DataActionEnum action)
        {
            var query = "";

            if(action == DataActionEnum.Create)
            {
                query = $@"Insert into cms.""F_EGOVERNANCE_ProjectProposalResponse""(""Id"",""IsDeleted"",""CompanyId"",""LegalEntityId"",""Status"",""VersionNo"",""CreatedDate"",""CreatedBy"",""LastUpdatedDate"",""LastUpdatedBy"",""ProjectProposalId"",""ResponseByUserId"",""ResponseType"")
                        values('{Guid.NewGuid()}','false','{_userContext.CompanyId}','{_userContext.LegalEntityId}',0,1,'{DateTime.Now}','{userId}','{DateTime.Now}','{userId}','{proposalId}','{userId}','{(int)type}') ";

            }else if(action == DataActionEnum.Edit)
            {   
                query = $@"Update cms.""F_EGOVERNANCE_ProjectProposalResponse""
                            set ""ResponseType""='{(type.HasValue ? (int)type : null)}', ""LastUpdatedDate""='{DateTime.Now}' 
                            where ""ProjectProposalId""='{proposalId}' and ""ResponseByUserId""='{userId}' ";
            }

            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<List<EGovDashboardViewModel>> GetProposalProjectsCount(string type=null)
        {           

            var Query = $@"select count(nw.""Id"") as ProjectsCount #SELECT#
                            from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as nw                            
                            left join public.""LOV"" as ss on nw.""ProjectStatus""=ss.""Id"" and ss.""IsDeleted""=false
                            left join public.""LOV"" as pc on nw.""ProjectCategory""=pc.""Id"" and pc.""IsDeleted""=false
                            left join public.""LOV"" as pl on nw.""ProjectWard""=pl.""Id"" and pl.""IsDeleted""=false
                            where nw.""IsDeleted""=false #GROUP# 
                            ";            

            string select;
            string where;

            if (type == "Location")
            {
                select = $@" ,pl.""Name"" as LocationName ";
                where = $@" and nw.""ProjectWard"" is not null group by nw.""ProjectWard"",pl.""Name"" ";
            }
            else if (type == "Category")
            {
                select = $@" ,pc.""Name"" as ProjectCategoryName ";
                where = $@" and pc.""Name"" is not null group by nw.""ProjectCategory"",pc.""Name"" ";
            }
            else if (type == "undertaken")
            {
                select = $@" ,pc.""Name"" as ProjectCategoryName ";
                //where = $@" and ss.""Code""='EGOV_PRO_STATUS_UNDERTAKEN' group by nw.""ProjectCategory"",pc.""Name"" ";
                where = $@" and nw.""IsProposedByCitizen""='false' group by nw.""ProjectCategory"",pc.""Name"" ";
            }
            else
            {
                select = $@" , ss.""Code"" as ProjectStatusCode,ss.""Name"" as ProjectStatus,
case when ss.""Code""='EGOV_PRO_STATUS_PROPOSED' then '#17a2b8'
when ss.""Code""='EGOV_PRO_STATUS_COMPLETED' then '#13b713'
when ss.""Code""='EGOV_PRO_STATUS_UNDERTAKEN' then '#007bff'
when ss.""Code""='EGOV_PRO_STATUS_ON_HOLD' then '#ffc107!'
when ss.""Code""='EGOV_PRO_STATUS_REJECTED' then '#f10b0b'
end as StatusColor ";
                where = $@" group by nw.""ProjectStatus"", ss.""Code"",ss.""Name"" order by ss.""Name"" ";
            }
            Query = Query.Replace("#SELECT#", select);
            Query = Query.Replace("#GROUP# ", where);
            
            var result = await _querych.ExecuteQueryList<EGovDashboardViewModel>(Query, null);
            return result;

        }
        public async Task<EGovDashboardViewModel> ViewProjectsUnderTaken(string serviceId)
        {
            var query = $@"Select nw.*,s.""ServiceNo"",pc.""Name"" as ProjectCategoryName,ss.""Name"" as ""ProjectStatus""
,s.""CreatedDate""::TIMESTAMP::DATE as ""RequestedDate"",sc.""Comment"" as ""Comment""
                           from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as nw
                          join public.""NtsService"" as s on nw.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                             left join public.""LOV"" as ss on nw.""ProjectStatus""=ss.""Id"" and ss.""IsDeleted""=false
                            left join public.""LOV"" as pc on nw.""ProjectCategory""=pc.""Id"" and pc.""IsDeleted""=false
                           left join public.""NtsServiceComment"" as sc on sc.""NtsServiceId""=s.""Id""
                          where s.""Id""='{serviceId}' and nw.""IsDeleted""=false  ";

            return await _queryRepo.ExecuteQuerySingle<EGovDashboardViewModel>(query, null);

        }
        public async Task<EGovDashboardViewModel> GetWardData(string UserId)
        {
            //var query = $@"Select nw.*, nw.""ProjectWard"" as ""WardId"",w.""Name"" as ""WardName""
            //               from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as nw
            //              --join public.""NtsService"" as s on nw.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
            //              join cms.""F_EGOVERNANCE_UserDetails"" as ud on ud.""WardId""=nw.""ProjectWard"" and ud.""UserId""='{_userContext.UserId}'
            //              join public.""LOV"" as w on w.""Id""=ud.""WardId"" and w.""IsDeleted""=false

            //              where ud.""UserId""='{UserId}' and nw.""IsDeleted""=false  ";

            var query = $@"Select ud.""WardId"",w.""Name"" as ""WardName""
                          from cms.""F_EGOVERNANCE_UserDetails"" as ud
                          join public.""LOV"" as w on w.""Id""=ud.""WardId"" and w.""IsDeleted""=false                            
                          where ud.""UserId""='{UserId}' and ud.""IsDeleted""=false  ";

            return await _queryRepo.ExecuteQuerySingle<EGovDashboardViewModel>(query, null);

        }
        public async Task<List<EGovDashboardViewModel>> GetNWTimeLineData()
        {
            var query = $@"Select nwt.*, nwt.""fromDate"" as ""FromDate"", nwt.""ToDate"" as ""ToDate""
                           ,nwt.""Message"" as ""Message"",nwt.""TimeLineStatus"" as ""TimeLineStatus""
                           from cms.""F_EGOVERNANCE_NeedsandWantsTimeline"" as nwt
                          where nwt.""IsDeleted""=false  ";

            var data= await _queryRepo.ExecuteQueryList<EGovDashboardViewModel>(query, null);
            return data;

        }

        public async Task<FacilityViewModel> GetFacilityDetails(string facilityCode)
        {
            string query = $@"Select f.*,fl.""LocationName"" as FacilityLocationName,fl.""YathraLocationId"",yl.""Name"" as YatraLocationName,ft.""Name"" as FacilityTypeName
from cms.""F_SwacchSanjy_SWACHH_SANJY_FACILITY_FORM"" as f
join cms.""F_SwacchSanjy_SSFacilityLocation"" as fl on f.""FacilityLocationId""=fl.""Id"" and fl.""IsDeleted""=false
join public.""LOV"" as ft on f.""FacilityTypeId""=ft.""Id"" and ft.""IsDeleted""=false 
join public.""LOV"" as yl on fl.""YathraLocationId""=yl.""Id"" and yl.""IsDeleted""=false  
where f.""IsDeleted""=false and f.""FacilityCode""='{facilityCode}' ";

            return await _queryRepo.ExecuteQuerySingle<FacilityViewModel>(query, null);
        }

        public async Task<IList<FacilityViewModel>> GetFacilityList(string userId=null)
        {
            string query = $@"Select f.*,fl.""LocationName"" as FacilityLocationName,fl.""YathraLocationId"",yl.""Name"" as YatraLocationName,ft.""Name"" as FacilityTypeName
from cms.""F_SwacchSanjy_SWACHH_SANJY_FACILITY_FORM"" as f
join cms.""F_SwacchSanjy_SSFacilityLocation"" as fl on f.""FacilityLocationId""=fl.""Id"" and fl.""IsDeleted""=false
join public.""LOV"" as ft on f.""FacilityTypeId""=ft.""Id"" and ft.""IsDeleted""=false 
join public.""LOV"" as yl on fl.""YathraLocationId""=yl.""Id"" and yl.""IsDeleted""=false 
where f.""IsDeleted""=false #USERWHERE# ";

            string userwhere = userId.IsNotNullAndNotEmpty() ? $@" and f.""InspectedBy""='{userId}' " : "";
            query = query.Replace("#USERWHERE#", userwhere);            

            return await _queryRepo.ExecuteQueryList<FacilityViewModel>(query, null);
        }

        public async Task<IList<FacilityLocationViewModel>> GetFacilityLocationList()
        {
            string query = $@"select fl.*,yl.""Name"" as YatraLocationName
from cms.""F_SwacchSanjy_SSFacilityLocation"" as fl
join public.""LOV"" as yl on fl.""YathraLocationId""=yl.""Id"" and yl.""IsDeleted""=false  
where fl.""IsDeleted""=false ";

            return await _queryRepo.ExecuteQueryList<FacilityLocationViewModel>(query, null);
        }

        public async Task<FacilityViewModel> GetPreviousFacilityStatus(string facilityCode)
        {
            string query = $@"Select f.*,fh.""Id"" as HealthStatusId,hs.""Name"" as HealthStatusName 
from cms.""F_SwacchSanjy_SWACHH_SANJY_FACILITY_FORM"" as f
join cms.""F_SwacchSanjy_SWACHH_SANJY_FACILITY_HEALTH_FORM"" as fh on f.""Id""=fh.""FacilityId"" and fh.""IsDeleted""=false
join public.""LOV"" as hs on fh.""HealthStatusId""=hs.""Id"" and hs.""IsDeleted""=false and hs.""LOVType""='SS_HEALTH_STATUS'
where f.""FacilityCode""='{facilityCode}' and f.""IsDeleted""=false
order by fh.""CreatedDate"" desc limit 1 ";

            return await _queryRepo.ExecuteQuerySingle<FacilityViewModel>(query, null);
        }

        public async Task<IList<NtsTaskIndexPageViewModel>> GetNeedsAndWantsTaskCount(string categoryCodes, string portalId, bool showAllTaskForAdmin = true)
        {
            if (categoryCodes.IsNullOrEmpty())
            {
                var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
                string[] codes = data.Select(x => x.Code).ToArray();
                categoryCodes = String.Join(",", codes);
            }

            var query = "";
            if (_repo.UserContext.IsSystemAdmin && showAllTaskForAdmin)
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
count(case when l.""Code"" = 'TASK_STATUS_INPROGRESS' or l.""Code"" = 'TASK_STATUS_OVERDUE' then 1 end) as AssignedToMeInProgreessOverDueCount,
count(case when l.""Code"" = 'TASK_STATUS_COMPLETE' then 1 end) as AssignedToMeCompleteCount,
count(case when l.""Code"" = 'TASK_STATUS_REJECT' or l.""Code"" = 'TASK_STATUS_CANCEL' then 1 end) as AssignedToMeRejectCancelCloseCount,
count(t.""Id"") as TotalCount

from public.""NtsTask"" as t
join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE#
group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }
            else
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
count(case when l.""Code"" = 'TASK_STATUS_INPROGRESS' or l.""Code"" = 'TASK_STATUS_OVERDUE' then 1 end) as AssignedToMeInProgreessOverDueCount,
count(case when l.""Code"" = 'TASK_STATUS_COMPLETE' then 1 end) as AssignedToMeCompleteCount,
count(case when l.""Code"" = 'TASK_STATUS_REJECT' or l.""Code"" = 'TASK_STATUS_CANCEL' then 1 end) as AssignedToMeRejectCancelCloseCount,
count(t.""Id"") as TotalCount

from public.""NtsTask"" as t
join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where t.""AssignedToUserId""='{_repo.UserContext.UserId}' and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE#
group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }

            var catCodeWhere = $@" and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}') ";

            //if (categoryCodes.IsNotNullAndNotEmpty())
            //{
            //    categoryCodes = categoryCodes.Replace(",", "','");
            //    //serTempCodes = String.Concat("'", serTempCodes, "'");
            //    catCodeWhere = $@" and stc.""Code"" in ('{categoryCodes}') ";
            //}
            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var result = await _queryRepo.ExecuteQueryList<NtsTaskIndexPageViewModel>(query, null);

            return result;
        }
        public async Task<IList<EGovProjectProposal>> GetNeedsAndWantsTaskList(string categoryCodes, string status, string portalId, bool showAllTaskForAdmin)
        {
            if (categoryCodes.IsNullOrEmpty())
            {
                var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
                string[] codes = data.Select(x => x.Code).ToArray();
                categoryCodes = String.Join(",", codes);
            }

            var query = "";
            if (_repo.UserContext.IsSystemAdmin && showAllTaskForAdmin)
            {
                query = $@"select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate"",st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode,
                t.""Id"" as TaskActionId, so.""Name"" as ServiceOwner,l.""Name"" as TaskStatusName,u.""Name"" as AssigneeUserName
                ,t.""TaskNo"" as TaskNo,coalesce(s.""WorkflowStatus"",sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName
                ,t.""TemplateCode"" as TemplateMasterCode,t.""TaskSubject"",pp.""ProjectName"",pc.""Name"" as ProjectCategory,psc.""Name"" as ProjectSubCategory,pw.""Name"" as ProjectWard
                from public.""NtsService"" as s
                join cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as pp on s.""UdfNoteTableId""=pp.""Id"" and pp.""IsDeleted""=false and pp.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as pc on pp.""ProjectCategory""=pc.""Id"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as psc on pp.""ProjectSubCategoryId""=psc.""Id"" and psc.""IsDeleted""=false and psc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as pw on pp.""ProjectWard""=pw.""Id"" and pw.""IsDeleted""=false and pw.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'            
                join public.""User"" as so on s.""RequestedByUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'          
                join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'            
			    join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                where t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' --and t.""AssignedToUserId""='{_repo.UserContext.UserId}' 
                #CODEWHERE# #STATUSWHERE# order by s.""CreatedDate"" desc, t.""CreatedDate"" desc ";
            }
            else
            {
                query = $@"select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate"",st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode,
                t.""Id"" as TaskActionId, so.""Name"" as ServiceOwner,l.""Name"" as TaskStatusName,u.""Name"" as AssigneeUserName
                ,t.""TaskNo"" as TaskNo,coalesce(s.""WorkflowStatus"",sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName
                ,t.""TemplateCode"" as TemplateMasterCode,t.""TaskSubject"",pp.""ProjectName"",pc.""Name"" as ProjectCategory,psc.""Name"" as ProjectSubCategory,pw.""Name"" as ProjectWard
                from public.""NtsService"" as s
                join cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as pp on s.""UdfNoteTableId""=pp.""Id"" and pp.""IsDeleted""=false and pp.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as pc on pp.""ProjectCategory""=pc.""Id"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as psc on pp.""ProjectSubCategoryId""=psc.""Id"" and psc.""IsDeleted""=false and psc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as pw on pp.""ProjectWard""=pw.""Id"" and pw.""IsDeleted""=false and pw.""CompanyId""='{_repo.UserContext.CompanyId}'
                
                join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""User"" as so on s.""RequestedByUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'          
                join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'            
			    join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""Id""='{_repo.UserContext.UserId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                where t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""AssignedToUserId""='{_repo.UserContext.UserId}' 
                #CODEWHERE# #STATUSWHERE# order by s.""CreatedDate"" desc, t.""CreatedDate"" desc  ";
            }


            var catCodeWhere = $@"and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}')";
            //if (categoryCodes.IsNotNullAndNotEmpty())
            //{
            //    categoryCodes = categoryCodes.Replace(",", "','");
            //    //serTempCodes = String.Concat("'", serTempCodes, "'");

            //    catCodeWhere = $@"and stc.""Code"" in ('{categoryCodes}')";
            //}
            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var statuswhere = "";
            if (status.IsNotNullAndNotEmpty())
            {
                status = status.Replace(",", "','");
                statuswhere = $@" and l.""Code"" in ('{status}') ";
            }
            query = query.Replace("#STATUSWHERE#", statuswhere);

            var result = await _queryRepo.ExecuteQueryList<EGovProjectProposal>(query, null);
            return result;
        }

        public async Task<IList<EGovProjectProposal>> ProjectsByLatLong()
        {
            string query = $@"select nw.""ProjectName"",nw.""ProjectDescription"",nw.""Latitude"",nw.""Longitude"",pc.""Name"" as ProjectCategory,ss.""Name"" as ProjectStatusName
                            from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as nw                            
                            left join public.""LOV"" as ss on nw.""ProjectStatus""=ss.""Id"" and ss.""IsDeleted""=false
                            left join public.""LOV"" as pc on nw.""ProjectCategory""=pc.""Id"" and pc.""IsDeleted""=false
                            left join public.""LOV"" as pl on nw.""ProjectWard""=pl.""Id"" and pl.""IsDeleted""=false
                            where nw.""IsDeleted""=false and nw.""Latitude"" is not null and nw.""Longitude"" is not null
                            ";

            var result = await _queryRepo.ExecuteQueryList<EGovProjectProposal>(query, null);
            return result;
        }

        public async Task<bool> UpdateProjectProposalStatus(dynamic udf, string ppsid)
        {
            var query = $@"Update cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" set ""ProjectStatus""='{ppsid}'
                                   where ""Id""='{udf.UdfNoteTableId}' ";

            await _query.ExecuteCommand(query, null);
            return true;
        }

        public async Task<JSCAssetConsumerViewModel> ReadJSCAssetConsumerData(string consumerId)
        {
            var query = $@"select cons.*,u.""Name"" as UserName 
                            from cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as cons
                            left join public.""User"" as u on u.""Id""=cons.""UserId"" and u.""IsDeleted""=false
                            where cons.""IsDeleted""=false and cons.""Id""='{consumerId}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<JSCAssetConsumerViewModel>(query,null);
            return querydata;
        }
        public async Task<IList<JSCAssetConsumerViewModel>> GetJSCAssetsDataByConsumer(string userId)
        {
            var query = $@"Select a.""WardId"",a.""AssetName"",a.""AssetDescription"",at.""Name"" as AssetTypeName,a.""AssetPhotoId"",aa.""AllotmentFromDate"",aa.""AllotmentToDate"",a.""specificLocation"" as SpecificLocation
                            ,a.""AssetTypeId"",ap.""Amount"",ap.""DueDate"",aps.""Name"" as PaymentStatusName,t.""Id"" as TaskId,ap.""Id"" as UdfNoteTableId,t.""AssignedToUserId"",w.""Name"" as WardName
                            ,a.""Latitude"" as Latitude,a.""Longitude"" as Longitude,'' as MapArea
                            from cms.""N_JSC_REV_Asset"" as a
                            join cms.""N_JSC_REV_AssetAllotment"" as aa on a.""Id""=aa.""AssetId"" and aa.""IsDeleted""=false
                            join cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as ac on aa.""ConsumerId""=ac.""Id"" and ac.""IsDeleted""=false
                            join public.""User"" as u on ac.""UserId""=u.""Id"" and u.""IsDeleted""=false
                            left join cms.""F_JSC_REV_WARD"" as w on a.""WardId""=w.""Id"" and w.""IsDeleted""=false
                            left join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on a.""AssetTypeId""=at.""Id"" and at.""IsDeleted""=false
                            left join cms.""N_JSC_REV_AssetPayment"" as ap on ac.""Id""=ap.""ConsumerId"" and a.""Id""=ap.""AssetId"" and ap.""IsDeleted""=false
                            left join public.""LOV"" as aps on ap.""PaymentStatusId""=aps.""Id"" and aps.""IsDeleted""=false
                            left Join public.""NtsService"" as s on s.""UdfNoteTableId""=ap.""Id"" and s.""IsDeleted""=false
                            left join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false
                            where a.""IsDeleted""=false and u.""Id""='{userId}' ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCAssetConsumerViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetWardListFromMaster()
        {
            var query = $@" select ""Name"",""Id"" from cms.""F_JSC_REV_WARD"" where ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        
        public async Task<List<IdNameViewModel>> GeCollectorListForJammu()
        {
            var query = $@" select ""Name"",""Id"" from cms.""F_JSC_REV_JSC_COLLECTOR"" where ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetAssetTypeListForJammu()
        {
            var query = $@" select ""Name"",""Id"" from cms.""F_JSC_REV_JSC_ASSET_TYPE"" where ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetJammuCollectorList()
        {
            var query = $@" select ""Name"",""Id"" from cms.""F_JSC_REV_JSC_COLLECTOR"" where ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<CollectorWardAssignmentViewModel>> GetAssignedWardCollectorList()
        {
            var query = $@" SELECT c.""Id"" as CollectorId, c.""Name"" as CollectorName, ARRAY_TO_STRING(ARRAY_AGG(w.""Name""), ' ,  ') as WardName
                            FROM cms.""F_JSC_REV_WardDetails"" as wd
                            left join cms.""F_JSC_REV_WARD"" as w on w.""Id"" = wd.""WardId"" and w.""IsDeleted"" = false
                            left join cms.""F_JSC_REV_JSC_COLLECTOR"" as c on c.""Id"" = wd.""ParentId"" and c.""IsDeleted"" = false
                            where wd.""IsDeleted"" = false
                            GROUP BY c.""Id"",c.""Name"" 
                            ";
            var queryData = await _queryRepo.ExecuteQueryList<CollectorWardAssignmentViewModel>(query, null);
            return queryData;
        }

        public async Task<CollectorWardAssignmentViewModel> GetWardCollectorById(string id)
        {
            var query = $@" select * from cms.""F_JSC_REV_JSC_CollectorWardAssigment"" where ""Id"" = '{id}' and ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CollectorWardAssignmentViewModel>(query, null);
            return queryData;
        }

        public async Task DeleteWardCollector(string id)
        {
            var query = $@" update cms.""F_JSC_REV_JSC_CollectorWardAssigment"" set ""IsDeleted"" = true where ""Id"" = '{id}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentReport(string source,string assetType,string ward,DateTime? From,DateTime? To)
        {
            var query = $@" select ac.""Name"" as Consumer,a.""AssetName"" as AssetName,pay.""DueDate"",pay.""BillDate"",at.""Name"" as AssetTypeName,pay.""Amount"",aa.""Name"" as WardName
                            from cms.""N_JSC_REV_AssetPayment"" as pay 
                            join public.""LOV"" as l on l.""LOVType"" = 'PAYMENT_STATUS' and pay.""PaymentStatusId"" = l.""Id"" and l.""IsDeleted"" = false and l.""Code"" != 'SUCCESS'
                            join cms.""N_JSC_REV_Asset"" as a on a.""Id"" = pay.""AssetId"" and a.""IsDeleted"" = false
                            join cms.""F_JSC_REV_WARD"" as aa on aa.""Id"" = a.""WardId"" and aa.""IsDeleted"" = false
							join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted"" = false
							left join cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as ac on ac.""Id"" = pay.""ConsumerId"" and ac.""IsDeleted"" = false

                            where pay.""IsDeleted"" = false and pay.""DueDate""::TIMESTAMP::DATE<CURRENT_DATE  #WARD# #ASSETTYPE#
                            #DATEWHERE#
                        ";
            if(source=="REVENUE")
            {
                query = $@" select ac.""Name"" as Consumer,a.""AssetName"" as AssetName,pay.""DueDate"",pay.""BillDate"",at.""Name"" as AssetTypeName,pay.""Amount"",aa.""Name"" as WardName
                            from cms.""N_JSC_REV_AssetPayment"" as pay 
                            join public.""LOV"" as l on l.""LOVType"" = 'PAYMENT_STATUS' and pay.""PaymentStatusId"" = l.""Id"" and l.""IsDeleted"" = false and l.""Code"" = 'SUCCESS'
                            join cms.""N_JSC_REV_Asset"" as a on a.""Id"" = pay.""AssetId"" and a.""IsDeleted"" = false
                            join cms.""F_JSC_REV_WARD"" as aa on aa.""Id"" = a.""WardId"" and aa.""IsDeleted"" = false
							join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Id"" = a.""AssetTypeId"" and at.""IsDeleted"" = false
							left join cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as ac on ac.""Id"" = pay.""ConsumerId"" and ac.""IsDeleted"" = false

                            where pay.""IsDeleted"" = false and pay.""DueDate""::TIMESTAMP::DATE<CURRENT_DATE  #WARD# #ASSETTYPE#
                            #DATEWHERE# ";
            }
            var wardwhere = ward.IsNotNullAndNotEmpty() ? $@" and a.""WardId""='{ward}' " : "";
            query = query.Replace("#WARD#", wardwhere);

            var assetTypewhere = assetType.IsNotNullAndNotEmpty() ? $@" and at.""Id""='{assetType}' " : "";
            query = query.Replace("#ASSETTYPE#", assetTypewhere);

            var datesearch = "";
            if (From.HasValue)
            {
                if (To.HasValue)
                {
                    datesearch = $@" and pay.""DueDate""::TIMESTAMP::DATE>='{From}'::TIMESTAMP::DATE and pay.""DueDate""::TIMESTAMP::DATE<='{To}'::TIMESTAMP::DATE ";
                }
                else
                {
                    datesearch = $@" and pay.""DueDate""::TIMESTAMP::DATE>='{From}'::TIMESTAMP::DATE ";
                }
            }
            else if (To.HasValue)
            {
                datesearch = $@" and pay.""DueDate""::TIMESTAMP::DATE<='{To}'::TIMESTAMP::DATE ";
            }
            query = query.Replace("#DATEWHERE#", datesearch);

            var queryData = await _queryRepo.ExecuteQueryList<JMCAssetPaymentViewModel>(query, null);
            return queryData;

        }
        public async Task<List<UserViewModel>> GetUnallocationUserList()
        {
            var query = $@" select u.* from public.""User"" as u 
                        where u.""Id"" not in(select c.""UserId"" from cms.""F_JSC_REV_JSC_ASSET_CONSUMER"" as c where c.""IsDeleted""=false)
                        and u.""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetUnAllocatedAssetFilterByFromnToDate(string fromDate, string toDate)
        {
            var query = $@" select a.""Id"" as Id, a.""AssetName"" as Name
                            from cms.""N_JSC_REV_Asset"" as a
                            where a.""Id"" not in (select aa.""AssetId"" 
					   	                            from cms.""N_JSC_REV_AssetAllotment"" as aa 
						                            where 
                                                    aa.""AllotmentFromDate""::date<='{fromDate.ToSafeDateTime().ToDatabaseDateFormat()}' 
                                                    and aa.""AllotmentToDate""::date>='{toDate.ToSafeDateTime().ToDatabaseDateFormat()}' 
                                                    and aa.""IsDeleted""=false
					                               )
                            and a.""IsDeleted""=false ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetUnAllocatedWard()
        {
            var query = $@" select w.""Id"" as Id, w.""Name"" as Name from cms.""F_JSC_REV_WARD"" as w 
                            where w.""Id"" not in (select wd.""WardId"" from cms.""F_JSC_REV_WardDetails"" as wd where wd.""IsDeleted"" = false )
                            and w.""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<List<EGOVBannerViewModel>> GetVideoGallery(string videoTypeCode)
        {
            var query = $@" select v.""VideoTitle"" as BannerContent, v.""VideoUrl"" as UrlLink
                            from cms.""F_EGOVERNANCE_VideoGallery"" as v
                            left join public.""LOV"" as vt on vt.""Id""=v.""VideoTypeId"" and vt.""IsDeleted""=false
                            where v.""IsDeleted""=false and vt.""Code""='{videoTypeCode}' order by v.""SequenceNo"" ";
            var queryData = await _queryRepo.ExecuteQueryList<EGOVBannerViewModel>(query, null);
            return queryData;
        }
        public async Task<List<EGovDashboardViewModel>> GetJSCProposalProjectsList(string type, string userId)
        {

            //var Query = $@"Select nw.*,pr.""ResponseType"" from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as nw 
            //            join public.""NtsService"" as s on nw.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
            //            join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
            //            left join cms.""F_EGOVERNANCE_ProjectProposalResponse"" as pr on nw.""Id""=pr.""ProjectProposalId"" and pr.""ResponseByUserId""='{userId}' and pr.""IsDeleted""=false
            //            where nw.""IsDeleted""=false #TYPE#";

            var Query = $@"select distinct nw.*,pc.""Name"" as ProjectCategoryName,pr.ResponseType,s.""Id"" as ServiceId,s.""TemplateCode"" as TemplateCodes,s.""OwnerUserId"" as UserId,
                            s.""ServiceNo"",ss.""Name"" as ""ProjectStatus"",s.""CreatedDate""::TIMESTAMP::DATE as ""RequestedDate"",s.""CreatedDate"",
                            COALESCE(lc.LikesCount,0) as LikesCount,COALESCE(dc.DislikesCount,0) as DislikesCount,sc.CommentsCount,
                            case when ss.""Code""='EGOV_PRO_STATUS_PROPOSED' then 'border-info'
                            when ss.""Code""='EGOV_PRO_STATUS_UNDERTAKEN' then 'border-primary'
                            when ss.""Code""='EGOV_PRO_STATUS_REJECTED' then 'border-danger'
                            when ss.""Code""='EGOV_PRO_STATUS_ON_HOLD' then 'border-warning'
                            when ss.""Code""='EGOV_PRO_STATUS_COMPLETED' then 'border-success'
                            end as BorderCSS,ss.""Code"" as ProjectStatusCode
                            from cms.""N_SNC_JSC_NEEDS_AND_WANTS_JSCProjectProposal"" as nw
                            join public.""NtsService"" as s on nw.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                            join public.""LOV"" as serst on s.""ServiceStatusId""=serst.""Id"" and serst.""IsDeleted""=false
                            join public.""LOV"" as ss on nw.""ProjectStatusId""=ss.""Id"" and ss.""IsDeleted""=false
                            join public.""LOV"" as pc on nw.""ProjectCategoryId""=pc.""Id"" and pc.""IsDeleted""=false
                            left join cms.""F_EGOVERNANCE_UserDetails"" as ud on ud.""WardId""=nw.""ProjectWardId"" --and ud.""UserId""='{_userContext.UserId}' and ud.""IsDeleted""=false

                            left join(select ""ProjectProposalId"",""ResponseByUserId"", case when ""ResponseType""='1' then '1'
                                        when ""ResponseType""='2' then '2' end as ResponseType
	                                    from cms.""F_EGOVERNANCE_ProjectProposalResponse""
                                  ) as pr on nw.""Id""=pr.""ProjectProposalId"" #USERWHERE#
                            left join(select ""ProjectProposalId"", count(""Id"") as LikesCount 
		                        from cms.""F_EGOVERNANCE_ProjectProposalResponse""
		                        where ""ResponseType""='1' group by ""ProjectProposalId"") as lc on lc.""ProjectProposalId""=nw.""Id""
                            left join(select ""ProjectProposalId"", count(""Id"") as DislikesCount 
		                        from cms.""F_EGOVERNANCE_ProjectProposalResponse""
		                        where ""ResponseType""='2' group by ""ProjectProposalId"") as dc on dc.""ProjectProposalId""=nw.""Id""
                            left join(select ""NtsServiceId"",count(""Id"") as CommentsCount
		                        from public.""NtsServiceComment"" where ""IsDeleted""=false 
		                        group by ""NtsServiceId"") as sc on s.""Id""=sc.""NtsServiceId""
                            where nw.""IsDeleted""=false #TYPE# order by s.""CreatedDate"" desc ";

            string typewhere = "";
            if (type == "PROPOSEDBYME")
            {
                typewhere = $@" and nw.""IsProposedByCitizen""='true' and s.""OwnerUserId""='{userId}'  ";
            }
            else if (type == "PROPOSEDBYOTHER")
            {
                typewhere = $@"  and serst.""Code""='SERVICE_STATUS_COMPLETE' and nw.""IsProposedByCitizen""='true' and s.""OwnerUserId""!='{userId}'  ";
            }
            else if (type == "PROPOSEDBYCITIZEN")
            {
                typewhere = $@" and nw.""IsProposedByCitizen""='true' ";
            }
            else if (type == "PROJECTSUNDERTAKEN")
            {
                typewhere = $@" and nw.""IsProposedByCitizen""='false' ";
            }
            else if (type == "PROJECTSUNDERTAKENCITIZEN")
            {
                typewhere = $@" and nw.""IsProposedByCitizen""='false' and s.""OwnerUserId""!='{userId}'  ";
            }
            Query = Query.Replace("#TYPE#", typewhere);

            string userwhere = userId.IsNotNullAndNotEmpty() ? $@" and pr.""ResponseByUserId""='{userId}'" : "";
            Query = Query.Replace("#USERWHERE#", userwhere);

            var result = await _querych.ExecuteQueryList<EGovDashboardViewModel>(Query, null);

            return result;

        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentReport2(string source, string assetType, string ward, DateTime? From, DateTime? To)
        {
            var query = $@" select 
                            u.""Name"" as Consumer
                            ,p.""prop_id"" as PropertyId --AssetName
                            ,pay.""DueDate"",pay.""PaymentDate"" as BillDate,pay.""Amount""
                            ,at.""Name"" as AssetTypeName
                            ,w.""wrd_name"" as WardName
                            from cms.""N_JAMMU_SMART_CITY_JSCPayment"" as pay 
                            join public.""LOV"" as l on l.""LOVType"" = 'JSC_PAYMENT_STATUS' and pay.""PaymentStatus"" = l.""Id"" and l.""IsDeleted"" = false and l.""Code"" != 'JSC_SUCCESS'
                            join public.""User"" as u on u.""Id"" = pay.""NoteOwnerUserId"" and u.""IsDeleted"" = false
                            join public.""parcel"" as p on p.""gid""::text = pay.""SourceReferenceId"" 
                            join public.""ward"" as w on w.""wrd_no"" = p.""ward_no""
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Code"" = p.""usg_cat_gf"" and at.""IsDeleted"" = false
                            where pay.""IsDeleted"" = false and pay.""DueDate""::TIMESTAMP::DATE<CURRENT_DATE  #WARD# #ASSETTYPE#
                            #DATEWHERE#
                        ";
            if (source == "REVENUE")
            {
                query = $@" select 
                            u.""Name"" as Consumer
                            ,p.""prop_id"" as PropertyId --AssetName
                            ,pay.""DueDate"",pay.""PaymentDate"" as BillDate,pay.""Amount""
                            ,at.""Name"" as AssetTypeName
                            ,w.""wrd_name"" as WardName
                            from cms.""N_JAMMU_SMART_CITY_JSCPayment"" as pay 
                            join public.""LOV"" as l on l.""LOVType"" = 'JSC_PAYMENT_STATUS' and pay.""PaymentStatus"" = l.""Id"" and l.""IsDeleted"" = false and l.""Code"" = 'JSC_SUCCESS'
                            join public.""User"" as u on u.""Id"" = pay.""NoteOwnerUserId"" and u.""IsDeleted"" = false
                            join public.""parcel"" as p on p.""gid""::text = pay.""SourceReferenceId"" 
                            join public.""ward"" as w on w.""wrd_no"" = p.""ward_no""
                            join cms.""F_JSC_REV_JSC_ASSET_TYPE"" as at on at.""Code"" = p.""usg_cat_gf"" and at.""IsDeleted"" = false
                            where pay.""IsDeleted"" = false #WARD# #ASSETTYPE#
                            #DATEWHERE# ";
            }
            var wardwhere = ward.IsNotNullAndNotEmpty() ? $@" and p.""ward_no""='{ward}' " : "";
            query = query.Replace("#WARD#", wardwhere);

            var assetTypewhere = assetType.IsNotNullAndNotEmpty() ? $@" and at.""Id""='{assetType}' " : "";
            query = query.Replace("#ASSETTYPE#", assetTypewhere);

            var datesearch = "";
            if (From.HasValue)
            {
                if (To.HasValue)
                {
                    datesearch = $@" and pay.""DueDate""::TIMESTAMP::DATE>='{From}'::TIMESTAMP::DATE and pay.""DueDate""::TIMESTAMP::DATE<='{To}'::TIMESTAMP::DATE ";
                }
                else
                {
                    datesearch = $@" and pay.""DueDate""::TIMESTAMP::DATE>='{From}'::TIMESTAMP::DATE ";
                }
            }
            else if (To.HasValue)
            {
                datesearch = $@" and pay.""DueDate""::TIMESTAMP::DATE<='{To}'::TIMESTAMP::DATE ";
            }
            query = query.Replace("#DATEWHERE#", datesearch);

            var queryData = await _queryRepo.ExecuteQueryList<JMCAssetPaymentViewModel>(query, null);
            return queryData;

        }

        public async Task<List<EGovDashboardViewModel>> GetAllProposalProjectsList()
        {
            var Query = $@"select distinct nw.*,pc.""Name"" as ProjectCategoryName,pr.ResponseType,s.""Id"" as ServiceId,s.""TemplateCode"" as TemplateCodes,s.""OwnerUserId"" as UserId,
                            s.""ServiceNo"",ss.""Name"" as ""ProjectStatus"",s.""CreatedDate""::TIMESTAMP::DATE as ""RequestedDate"",s.""CreatedDate"",--wr.""Name"" as WardName,
                            COALESCE(lc.LikesCount,0) as LikesCount,COALESCE(dc.DislikesCount,0) as DislikesCount,sc.CommentsCount,
                            coalesce(s.""WorkflowStatus"",serst.""Name"") as WorkflowStatus,
                            case when ss.""Code""='EGOV_PRO_STATUS_PROPOSED' then 'border-info'
                            when ss.""Code""='EGOV_PRO_STATUS_UNDERTAKEN' then 'border-primary'
                            when ss.""Code""='EGOV_PRO_STATUS_REJECTED' then 'border-danger'
                            when ss.""Code""='EGOV_PRO_STATUS_ON_HOLD' then 'border-warning'
                            when ss.""Code""='EGOV_PRO_STATUS_COMPLETED' then 'border-success'
                            end as BorderCSS,ss.""Code"" as ProjectStatusCode
                            from cms.""N_SNC_EGOV_NEED_WANT_ProjectProposal"" as nw
                            join public.""NtsService"" as s on nw.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                            join public.""LOV"" as serst on s.""ServiceStatusId""=serst.""Id"" and serst.""IsDeleted""=false
                            join public.""LOV"" as ss on nw.""ProjectStatus""=ss.""Id"" and ss.""IsDeleted""=false
                            join public.""LOV"" as pc on nw.""ProjectCategory""=pc.""Id"" and pc.""IsDeleted""=false
                            --join public.""LOV"" as wr on wr.""Id"" = nw.""ProjectWard"" and wr.""IsDeleted""=false and wr.""LOVType"" = 'EGOV_ELECTORAL_WARD'
                            --join cms.""F_EGOVERNANCE_UserDetails"" as ud on ud.""WardId""=nw.""ProjectWard"" --and ud.""UserId""='{_userContext.UserId}' and ud.""IsDeleted""=false

                            left join(select ""ProjectProposalId"",""ResponseByUserId"", case when ""ResponseType""='1' then '1'
                                        when ""ResponseType""='2' then '2' end as ResponseType
	                                    from cms.""F_EGOVERNANCE_ProjectProposalResponse""
                                  ) as pr on nw.""Id""=pr.""ProjectProposalId"" 
                            left join(select ""ProjectProposalId"", count(""Id"") as LikesCount 
		                        from cms.""F_EGOVERNANCE_ProjectProposalResponse""
		                        where ""ResponseType""='1' group by ""ProjectProposalId"") as lc on lc.""ProjectProposalId""=nw.""Id""
                            left join(select ""ProjectProposalId"", count(""Id"") as DislikesCount 
		                        from cms.""F_EGOVERNANCE_ProjectProposalResponse""
		                        where ""ResponseType""='2' group by ""ProjectProposalId"") as dc on dc.""ProjectProposalId""=nw.""Id""
                            left join(select ""NtsServiceId"",count(""Id"") as CommentsCount
		                        from public.""NtsServiceComment"" where ""IsDeleted""=false 
		                        group by ""NtsServiceId"") as sc on s.""Id""=sc.""NtsServiceId""
                            where nw.""IsDeleted""=false and serst.""Code""='SERVICE_STATUS_COMPLETE' and nw.""Level"" = 'C'  order by s.""CreatedDate"" desc ";


            var result = await _querych.ExecuteQueryList<EGovDashboardViewModel>(Query, null);

            return result;

        }

    }
}
