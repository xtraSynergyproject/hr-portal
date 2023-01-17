using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.ViewModel;
using Synergy.App.DataModel;
using Synergy.App.Common;
using Synergy.App.Repository;
using AutoMapper;

namespace Synergy.App.Business
{
    public class BLSQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IBLSQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        
        public BLSQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
        }

        public async Task<List<IdNameViewModel>> getBLSLocationList(string userId = null)
        {
            var query = "";
            if (userId.IsNotNull())
            {
                query = $@" select loc.""Name"" as Name, loc.""Id"" as Id 
                            from public.""LegalEntity"" as loc 
                            join cms.""F_BLS_MASTER_BLS_CUSTOMER"" as cus on cus.""LegalEntityId"" = loc.""Id"" and cus.""IsDeleted"" = false
                            where loc.""IsDeleted"" = false and cus.""UserId"" = '{userId}'
                        ";
            }
            else
            {
                query = $@" select ""Name"" as Name, ""Id"" as Id 
                            from public.""LegalEntity"" where ""IsDeleted"" = false 
                        ";
            }

            var res = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return res;
        }

        public async Task<List<IdNameViewModel>> GetVisaTypes()
        {
            var query = $@" select ""VisaType"" as Name, ""Id"" as Id from cms.""F_BLS_MASTER_VisaType"" where ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return res;
        }
        public async Task<BLSVisaAppointmentViewModel> GetAppointmentDetails(string serviceId, string serviceType)
        {
            var query = $@"select va.*,s.""Id"" as ServiceId,s.""ServiceNo"",l.""Name"" as LocationName,st.""Name"" as ServiceTypeName,
                            vt.""VisaType"" as VisaTypeName,ast.""Name"" as AppointmentStatusName,apc.""CategoryName"" as AppointmentCategoryName
                            from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va
                            join public.""NtsService"" as s on s.""UdfNoteTableId"" = va.""Id"" and s.""IsDeleted""=false
                            left join public.""LOV"" as st on st.""Id""=va.""ServiceTypeId"" and st.""IsDeleted""=false                            
                            left join public.""LegalEntity"" as l on l.""Id""=va.""LegalLocationId"" and l.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_VisaType"" as vt on vt.""Id""=va.""VisaTypeId"" and vt.""IsDeleted""=false
                            left join public.""LOV"" as ast on ast.""Id""=va.""AppointmentStatusId"" and ast.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_AppointmentCategory"" as apc on va.""AppointmentCategoryId""=apc.""Id"" and apc.""IsDeleted""=false 
                            where va.""IsDeleted"" = false and s.""Id"" = '{serviceId}' and st.""Code""='{serviceType}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<BLSVisaAppointmentViewModel>(query, null);
            
            return querydata;
        }
        public async Task<BLSVisaAppointmentViewModel> GetVisaApplicationDetails(string serviceId)
        {
            var query = $@"select va.*
                            from  public.""NtsService"" as s
                            join cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" as va on s.""UdfNoteTableId"" = va.""Id"" and va.""IsDeleted""=false
                            where s.""IsDeleted""=false and s.""Id"" = '{serviceId}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<BLSVisaAppointmentViewModel>(query, null);
            //var querydata = new BLSVisaAppointmentViewModel();
            return querydata;
        }

        public async Task<List<BLSVisaAppointmentViewModel>> GetVisaApplicationDetailsByAppId(string appId)
        {
            var query = $@"select vap.*,vaps.""Id"" as ApplicationServiceId ,aps.""Code"" as ApplicationStatusCode
                        from cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" as vap
                        join public.""NtsService"" as vaps on vap.""Id""=vaps.""UdfNoteTableId"" and vaps.""IsDeleted""=false
                        join public.""LOV"" as aps on vap.""ApplicationStatusId""=aps.""Id"" and aps.""IsDeleted""=false
                        where vap.""IsDeleted""=false and vap.""AppointmentId"" = '{appId}' order by ""CreatedDate"" ";

            var querydata = await _queryRepo.ExecuteQueryList<BLSVisaAppointmentViewModel>(query, null);
            
            return querydata;
        }

        public async Task<List<ValueAddedServicesViewModel>> GetSelectedVAS(string appId)
        {
            string query = $@"select vas.* 
from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as app
join cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" as v on app.""Id""=v.""AppointmentId"" and v.""IsDeleted""=false
join cms.""N_SNC_BLS_BLSApplicationVAS"" as vas on v.""Id""=vas.""ParentId"" and vas.""IsDeleted""=false
where app.""IsDeleted""=false and app.""Id""='{appId}' ";

            var querydata = await _queryRepo.ExecuteQueryList<ValueAddedServicesViewModel>(query, null);            
            return querydata;
        }
        public async Task<List<BLSVisaAppointmentViewModel>> GetAppointmentDetailsByServiceId(string serviceId)
        {
            var query = $@"select va.*,s.""Id"" as ServiceId,s.""ServiceNo"",l.""Name"" as LocationName,st.""Name"" as ServiceTypeName,
                            vt.""VisaType"" as VisaTypeName,ast.""Name"" as AppointmentStatusName,ast.""Code"" as AppointmentStatusCode,l.""Latitude"" as LocationLatitude,l.""Longitude"" as LocationLongitude
                            ,concat_ws(', ',l.""Address"",l.""Street"",city.""CityName"",stat.""StateName"",l.""PinCode"") as AppointmentAddress
                            ,app.""FirstName"" as GivenName, app.""PassportNo"" as PassportNumber,app.""AppointmentSlot"",
                            --vs.""Id"" as ApplicationId,vss.""Id"" as ApplicationServiceId,
                            va.""ImageId"" as PhotoId,app.""DateOfBirth"" as DateOfBirth,app.""LastName"" as SurName,app.""IssueDate"",app.""ExpiryDate"",app.""IssuePlace"",apc.""CategoryName"" as AppointmentCategoryName
                            from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va
                            join cms.""N_SNC_BLS_APPOINTMENT_BLSAPPLICANTDETAILS"" as app on app.""ParentId"" = va.""Id"" and app.""IsDeleted""=false
                            join public.""NtsService"" as s on s.""UdfNoteTableId"" = va.""Id"" and s.""IsDeleted""=false
                            left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
                            left join public.""LOV"" as st on st.""Id""=va.""ServiceTypeId"" and st.""IsDeleted""=false                            
                            left join public.""LegalEntity"" as l on l.""Id""=va.""LegalLocationId"" and l.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_VisaType"" as vt on vt.""Id""=va.""VisaTypeId"" and vt.""IsDeleted""=false
                            left join public.""LOV"" as ast on ast.""Id""=va.""AppointmentStatusId"" and ast.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_BLS_City"" as city on city.""Id""=l.""CityName"" and city.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_BLS_State"" as stat on stat.""Id"" = l.""StateName"" and stat.""IsDeleted"" = false
                            --left join cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" as vs on vs.""AppointmentId""=va.""Id"" and vs.""IsDeleted""=false 
                left join cms.""F_BLS_MASTER_AppointmentCategory"" as apc on va.""AppointmentCategoryId""=apc.""Id"" and apc.""IsDeleted""=false 
                            --left join public.""NtsService"" as vss on vss.""UdfNoteTableId"" = vs.""Id"" and vss.""IsDeleted""=false
                            where va.""IsDeleted"" = false and s.""Id"" = '{serviceId}' --limit 1";
            var querydata = await _queryRepo.ExecuteQueryList<BLSVisaAppointmentViewModel>(query, null);
            if (querydata.IsNotNull())
            {
                foreach(var data in querydata)
                {
                    if (data.ImageId.IsNotNullAndNotEmpty())
                    {
                        data.PhotoId = data.ImageId;
                    }
                }               
            }
            //var querydata = new BLSVisaAppointmentViewModel();
            return querydata;
        }
         public async Task<BLSVisaAppointmentViewModel> GetAppointmentDetailsByServiceNo(string serviceNo)
        {
            var query = $@"select va.*,s.""Id"" as ServiceId,s.""ServiceNo"",l.""Name"" as LocationName,st.""Name"" as ServiceTypeName,
                            vt.""VisaType"" as VisaTypeName,ast.""Name"" as AppointmentStatusName,ast.""Code"" as AppointmentStatusCode,l.""Latitude"" as LocationLatitude,l.""Longitude"" as LocationLongitude
                            ,concat_ws(', ',l.""Address"",l.""Street"",city.""CityName"",stat.""StateName"",l.""PinCode"") as AppointmentAddress
                            ,app.""FirstName"" as GivenName, app.""PassportNo"" as PassportNumber,vs.""Id"" as ApplicationId,vss.""Id"" as ApplicationServiceId
                            , va.""ImageId"" as PhotoId,app.""DateOfBirth"" as DateOfBirth,app.""LastName"" as SurName,app.""IssueDate"",app.""ExpiryDate"",app.""IssuePlace"",pt.""Name"" as PassportType,apc.""CategoryName"" as AppointmentCategoryName
                            from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va
                            join cms.""N_SNC_BLS_APPOINTMENT_BLSAPPLICANTDETAILS"" as app on app.""ParentId"" = va.""Id"" and app.""IsDeleted""=false
                            join public.""NtsService"" as s on s.""UdfNoteTableId"" = va.""Id"" and s.""IsDeleted""=false
                            left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
                            left join public.""LOV"" as st on st.""Id""=va.""ServiceTypeId"" and st.""IsDeleted""=false                            
                            left join public.""LOV"" as pt on pt.""Id""=app.""PassportTypeId"" and pt.""IsDeleted""=false                            
                            left join public.""LegalEntity"" as l on l.""Id""=va.""LegalLocationId"" and l.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_VisaType"" as vt on vt.""Id""=va.""VisaTypeId"" and vt.""IsDeleted""=false
                            left join public.""LOV"" as ast on ast.""Id""=va.""AppointmentStatusId"" and ast.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_BLS_City"" as city on city.""Id""=l.""CityName"" and city.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_BLS_State"" as stat on stat.""Id"" = l.""StateName"" and stat.""IsDeleted"" = false
                            left join cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" as vs on vs.""AppointmentId""=va.""Id"" and vs.""IsDeleted""=false 
                            left join cms.""F_BLS_MASTER_AppointmentCategory"" as apc on va.""AppointmentCategoryId""=apc.""Id"" and apc.""IsDeleted""=false 
                            left join public.""NtsService"" as vss on vss.""UdfNoteTableId"" = vs.""Id"" and vss.""IsDeleted""=false
                            where va.""IsDeleted"" = false and s.""ServiceNo"" = '{serviceNo}' limit 1";
            var querydata = await _queryRepo.ExecuteQuerySingle<BLSVisaAppointmentViewModel>(query, null);
            if (querydata.IsNotNull())
            {
                if (querydata.ImageId.IsNotNullAndNotEmpty())
                {
                    querydata.PhotoId = querydata.ImageId;
                }
            }
            //var querydata = new BLSVisaAppointmentViewModel();
            return querydata;
        }
        public async Task<BLSVisaApplicationSettingsViewModel> GetSettingsData()
        {
            var query = $@" select ""AppointmentInstruction"" , ""AppointmentEmailInstruction"" ,""MaximumAllowedDays""
                            from cms.""F_BLS_SETTINGS_BLS_VISA_SETTINGS"" 
                            where ""IsDeleted"" = false 
                            order by ""LastUpdatedDate"" desc limit 1 ";
            var res = await _queryRepo.ExecuteQuerySingle<BLSVisaApplicationSettingsViewModel>(query, null);
            return res;
        }
        public async Task<BLSApplicantViewModel> GetPassportDetail(string passportNo)
        {
            var query = $@" select *
                            from cms.""F_BLS_MASTER_PASSPORT_DETAIL"" 
                            where ""IsDeleted"" = false and ""PassportNo""='{passportNo}'";
            var res = await _queryRepo.ExecuteQuerySingle<BLSApplicantViewModel>(query, null);
            return res;
        }
        public async Task<BLSAPiViewModel> IntegratePassportDetail(string countryId)
        {
            var query = $@" select * 
                            from cms.""F_BLS_SETTINGS_PASSPORT_MASTER_API"" 
                            where ""IsDeleted"" = false and ""CountryId""='{countryId}' limit 1 ";
            var res = await _queryRepo.ExecuteQuerySingle<BLSAPiViewModel>(query, null);
            return res;
        }

        public async Task<BLSVisaAppointmentViewModel> CheckEmailandServiceNo(string applicantEmail, string serviceNo)
        {
            var query = $@" select va.*,ns.""Id"" as ServiceId from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va
                            --join public.""NtsNote"" as nn on nn.""Id"" = va.""NtsNoteId""
                            join public.""NtsService"" as ns on ns.""UdfNoteTableId"" = va.""Id""
                            where va.""IsDeleted"" = false and ns.""ServiceNo"" = '{serviceNo}' and va.""ApplicantEmail"" = '{applicantEmail}'
                            ";
            var res = await _queryRepo.ExecuteQuerySingle<BLSVisaAppointmentViewModel>(query, null);
            return res;
        }

        public async Task<BLSVisaAppointmentViewModel> GetDataById(string id)
        {
            var query = $@" select *, ""NtsNoteId"" as NtsNoteId
                            from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" 
                            where ""Id"" = '{id}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQuerySingle<BLSVisaAppointmentViewModel>(query, null);
            return res;
        }

        public async Task<List<BLSVisaAppointmentViewModel>> GetAppointmentSlotByDate(string date,string location)
        {
            var query = $@" select ad.""AppointmentSlot"" as AppointmentSlot,ad.""Id"" as AppointmentSlotId from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va
 join cms.""N_SNC_BLS_APPOINTMENT_BLSAPPLICANTDETAILS"" as ad on ad.""ParentId""=va.""Id"" and ad.""IsDeleted"" = false
where va.""LegalLocationId""='{location}' and va.""AppointmentDate"" LIKE '{date}%' and va.""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<BLSVisaAppointmentViewModel>(query, null);
            return res;
        }
        public async Task<List<BLSApplicantViewModel>> GetAppointmentSlotById(string appointmentId)
        {
            var query = $@" select ad.""AppointmentSlot"" as AppointmentSlot,ad.""Id"" as Id from 
            cms.""N_SNC_BLS_APPOINTMENT_BLSAPPLICANTDETAILS"" as ad 
            where ad.""ParentId""='{appointmentId}' and ad.""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<BLSApplicantViewModel>(query, null);
            return res;
        }
        public async Task<BLSTimeSlotViewModel> GetTimeSlotByLocation(DateTime date,string location,string category)
        {
            var query = $@" select * from cms.""F_BLS_SETTINGS_BLS_VISA_TIMESLOT_SETTINGS"" where ""LegalEntityId""='{location}' 
            and ""Day""=extract(dow from date('{date}'))::text and ""SlotCategoryId""='{category}' ";
            var res = await _queryRepo.ExecuteQuerySingle<BLSTimeSlotViewModel>(query, null);
            return res;
        }
        public async Task<List<BLSTimeSlotViewModel>> GetTimeSlotList(string location)
        {
            var query = $@" select * from cms.""F_BLS_SETTINGS_BLS_VISA_TIMESLOT_SETTINGS"" where ""LegalEntityId""='{location}' ";
            var res = await _queryRepo.ExecuteQueryList<BLSTimeSlotViewModel>(query, null);
            return res;
        } 
        public async Task<List<Holiday>> GetAppointmentDate()
        {
            var data = await GetSettingsData();
            var query = $@"SELECT day::date as StartDate
            FROM   generate_series(CURRENT_DATE, CURRENT_DATE+{data.MaximumAllowedDays}, '1 day') day; ";
            var res = await _queryRepo.ExecuteQueryList<Holiday>(query, null);
            return res;
        }
        public async Task<List<Holiday>> GetHolidays(string location)
        {
            var data = await GetSettingsData();

            var query = $@" select * from cms.""F_BLS_MASTER_HOLIDAY"" where ""LegalEntityId""='{location}' 
                        and (date(""StartDate"")<=CURRENT_DATE and date(""EndDate"")>=CURRENT_DATE)
                        or date(""StartDate"")>=CURRENT_DATE and date(""StartDate"")<=CURRENT_DATE+{data.MaximumAllowedDays+3} ";
            var res = await _queryRepo.ExecuteQueryList<Holiday>(query, null);
            return res;
        }

        public async Task<BLSVisaAppointmentViewModel> GetSchengenVisaApplicationDetailsById(string id)
        {
            var query = $@" select ""FirstName"",""Surname"" as SurName, ""SurnameatBirth"" as SurnameAtBirth,""FileNumber"",""DateOfBirth"",c.""Name"" as CountryOfBirth,
n.""Name"" as NationalityName,u.""PhotoId""
from cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" as sv
left join cms.""F_BLS_MASTER_BLS_Country"" as n on sv.""CurrentNationalityId""=n.""Id"" and n.""IsDeleted""=false
left join cms.""F_BLS_MASTER_BLS_Country"" as c on sv.""CountryOfBirthId""=c.""Id"" and c.""IsDeleted""=false
join public.""NtsService"" as s on sv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false
where sv.""Id"" = '{id}' and sv.""IsDeleted"" = false ";

            var res = await _queryRepo.ExecuteQuerySingle<BLSVisaAppointmentViewModel>(query, null);
            return res;
        }

        public async Task<BLSVisaAppointmentViewModel> GetVisaAppointmentByParams(string applicantEmail, string applicationNo)
        {
            //var query = $@" select va.*,s.""Id"" as ServiceId, s.""ServiceNo"" as ServiceNo
            //                from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va
            //                join public.""NtsService"" as s on s.""UdfNoteTableId"" = va.""Id"" and s.""IsDeleted"" = false
            //                join cms.""N_SNC_BLS_APPOINTMENT_BLSAPPLICANTDETAILS"" as app on app.""ParentId"" = va.""Id""
            //                where va.""IsDeleted"" = false and va.""AppointmentFor"" = '{appointmentFor}'
            //                and ( '{applicationNo}' = s.""ServiceNo"" or '{applicationNo}' = app.""PassportNo"")
            //            ";

            var query = $@" select va.*,s.""Id"" as ServiceId, s.""ServiceNo"" as ServiceNo
                            from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va
                            join public.""NtsService"" as s on s.""UdfNoteTableId"" = va.""Id"" and s.""IsDeleted"" = false
                            join cms.""N_SNC_BLS_APPOINTMENT_BLSAPPLICANTDETAILS"" as app on app.""ParentId"" = va.""Id""
                            where va.""IsDeleted"" = false and va.""ApplicantEmail"" = '{applicantEmail}'
                            and '{applicationNo}' = s.""ServiceNo""
                        ";

            var res = await _queryRepo.ExecuteQuerySingle<BLSVisaAppointmentViewModel>(query, null);
            return res;
        }

        public async Task<OnlinePaymentViewModel> GetOnlinePaymentDetails(OnlinePaymentViewModel model)
        {
            var existquery = $@"Select * from cms.""F_GENERAL_OnlinePayment"" where ""NtsId""='{model.NtsId}' and ""NtsType""='{(int)model.NtsType}' and ""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQuerySingle<OnlinePaymentViewModel>(existquery, null);
            return result;
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

            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task UpdateOnlinePaymentDetailsData(OnlinePaymentViewModel model, string date)
        {
            var query = @$"Insert into cms.""F_GENERAL_OnlinePayment"" (""Id"",""CreatedDate"",""CreatedBy"",""LastUpdatedDate"",""LastUpdatedBy"",""CompanyId"",
                    ""SequenceOrder"",""VersionNo"",""IsDeleted"",""LegalEntityId"",""Status"",""NtsId"",""NtsType"",""UdfTableId"",""Amount"",""RequestUrl"",""EmailId"",""MobileNumber"",""Message"",""ChecksumValue"",""PaymentGatewayReturnUrl"",""ReturnUrl"")
                    Values('{model.Id}','{date}','{model.UserId}','{date}','{model.UserId}','{_repo.UserContext.CompanyId}','1','1',false,'{_repo.UserContext.LegalEntityId}','{(int)StatusEnum.Active}',
                    '{model.NtsId}','{(int)model.NtsType}','{model.UdfTableId}','{model.Amount}','{model.RequestUrl}','{model.EmailId}','{model.MobileNumber}','{model.Message}','{model.ChecksumValue}','{model.PaymentGatewayReturnUrl}','{model.ReturnUrl}' )";
            
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateApplicationStatus(string Id, string status)
        {
            var query = $@"Update cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" set ""LastUpdatedDate""='{DateTime.Now}',
            ""LastUpdatedBy""='{_repo.UserContext.UserId}', 
            ""ApplicationStatusId""='{status}'
            where ""Id""='{Id}'";

            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<OnlinePaymentViewModel> GetOnlinePayment(string id)
        {
            var existquery = $@"Select * from cms.""F_GENERAL_OnlinePayment"" where ""Id""='{id}' and ""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQuerySingle<OnlinePaymentViewModel>(existquery, null);
            return result;
        }
        public async Task<VisaTypeViewModel> GetVisaTypeDetails(string id)
        {
            var query = $@" select * from cms.""F_BLS_MASTER_VisaType"" where ""Id"" = '{id}' ";
            var res = await _queryRepo.ExecuteQuerySingle<VisaTypeViewModel>(query, null);
            return res;
        }
        public async Task<List<IdNameViewModel>> GetAppointmentCategoryList(string userId=null,string Id=null)
        {
            var query = $@" select ""Id"",""CategoryName"" as ""Name"",""CategoryCode"" as Code from cms.""F_BLS_MASTER_AppointmentCategory"" where ""IsDeleted"" = false <<WHERE>> ";
            var where = Id.IsNotNullAndNotEmpty() ? $@" and ""Id""='{Id}' " : "";
            query = query.Replace("<<WHERE>>",where);
            var res = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return res;
        }
        public async Task<BLSVisaAppointmentViewModel> GetAppointmentDetailsById(string Id)
        {
            var query = $@"select va.*, s.""Id"" as ServiceId, va.""NtsNoteId"" as ""NtsNoteId""
                           
                            from cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va
                            join public.""NtsService"" as s on s.""UdfNoteTableId"" = va.""Id"" and s.""IsDeleted"" = false
                           
                            where va.""IsDeleted"" = false and va.""Id"" = '{Id}'";
            var querydata = await _queryRepo.ExecuteQuerySingle<BLSVisaAppointmentViewModel>(query, null);
            //var querydata = new BLSVisaAppointmentViewModel();
            return querydata;
        }

        public async Task<List<ValueAddedServicesViewModel>> GetValueAddedServices()
        {
            var query = $@" select *,""ServiceCharge"" as ServiceCharges from cms.""F_BLS_MASTER_ValueAddedServices"" where ""IsDeleted"" = false
                        ";
            var res = await _queryRepo.ExecuteQueryList<ValueAddedServicesViewModel>(query, null);
            return res;
        }
        public async Task<BLSTimeSlotViewModel> GetTimeSlotById(string Id)
        {
            var query = $@" select * from cms.""F_BLS_SETTINGS_BLS_VISA_TIMESLOT_SETTINGS"" where ""Id""='{Id}'";
            var res = await _queryRepo.ExecuteQuerySingle<BLSTimeSlotViewModel>(query, null);
            return res;
        }
        public async Task<List<BLSTimeSlotViewModel>> GetAllTimeSlotList()
        {
            var query = $@" select vt.*,lg.""Name"" as LocationName from cms.""F_BLS_SETTINGS_BLS_VISA_TIMESLOT_SETTINGS"" as vt 
                        join public.""LegalEntity"" as lg on lg.""Id""=vt.""LegalEntityId""
                        where vt.""IsDeleted""=false and vt.""LegalEntityId""='{_uc.LegalEntityId}' ";
            var res = await _queryRepo.ExecuteQueryList<BLSTimeSlotViewModel>(query, null);
            return res;
        }
        public async Task<List<TimeSlot>> GetTimeSlotByParentId(string Id)
        {
            var query = $@" select * from cms.""F_BLS_SETTINGS_BLS_TIME_SLOT"" where ""ParentId""='{Id}' and ""IsDeleted""=false";
            var res = await _queryRepo.ExecuteQueryList<TimeSlot>(query, null);
            return res;
        }

        public async Task<List<BLSApplicantViewModel>> GetApplicantsList(string parentId)
        {
            var query = $@" select * from cms.""N_SNC_BLS_APPOINTMENT_BLSAPPLICANTDETAILS"" where ""ParentId""='{parentId}' and ""IsDeleted""=false";
            var res = await _queryRepo.ExecuteQueryList<BLSApplicantViewModel>(query, null);
            return res;
        }

        public async Task<List<BLSVisaAppointmentViewModel>> GetMyAppointmentsList(string serviceId=null)
        {
            var query = $@"Select va.*,le.""Name"" as LocationName,vt.""VisaType"" as VisaTypeName,aps.""Name"" as AppointmentStatusName,aps.""Code"" as AppointmentStatusCode,
apc.""CategoryName"" as AppointmentCategoryName
--,vaps.""Name"" as VisaApplicationStatusName,vaps.""Code"" as ApplicationStatusCode,
--vaser.""Id"" as VisaApplicationServiceId,vaser.""TemplateCode"" as VisaApplicationServiceTemplateCode
,s.""Id"" as VisaAppointmentServiceId,s.""ServiceNo""
from public.""NtsService"" as s
join cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va on s.""UdfNoteTableId""=va.""Id"" and va.""IsDeleted""=false
join public.""LegalEntity"" as le on va.""LegalLocationId""=le.""Id"" and le.""IsDeleted""=false
join cms.""F_BLS_MASTER_VisaType"" as vt on va.""VisaTypeId""=vt.""Id"" and vt.""IsDeleted""=false
left join public.""LOV"" as aps on va.""AppointmentStatusId""=aps.""Id"" and aps.""IsDeleted""=false
left join cms.""F_BLS_MASTER_AppointmentCategory"" as apc on va.""AppointmentCategoryId""=apc.""Id"" and apc.""IsDeleted""=false
--left join cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" as vap on va.""Id""=vap.""AppointmentId"" and vap.""IsDeleted""=false
--left join public.""LOV"" as vaps on vap.""ApplicationStatusId""=vaps.""Id"" and vaps.""IsDeleted""=false
--left join public.""NtsService"" as vaser on vap.""Id""=vaser.""UdfNoteTableId"" and vaser.""IsDeleted""=false
where s.""IsDeleted""=false and s.""OwnerUserId""='{_uc.UserId}' <<WHERE>> order by va.""AppointmentDate"" desc ";

            string where = serviceId.IsNotNullAndNotEmpty() ? $@" and s.""Id""='{serviceId}' " : "" ;
            query = query.Replace("<<WHERE>>", where);

            var res = await _queryRepo.ExecuteQueryList<BLSVisaAppointmentViewModel>(query, null);
            return res;
        }

        public async Task<List<BLSApplicantViewModel>> GetAppointmentDetailsWithApplicants(string parentId = null)
        {
            var query = $@"select apd.*, va.""Id"" as AppointmentId,va.""ApplicantEmail"",va.""CurrentContactNumber"",va.""AppointmentDate""
from cms.""N_SNC_BLS_APPOINTMENT_BLSAPPLICANTDETAILS"" as apd
join cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va on apd.""ParentId""=va.""Id"" and va.""IsDeleted""=false
join public.""NtsService"" as s on va.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where apd.""IsDeleted""=false and apd.""ParentId""='{parentId}'
order by apd.""CreatedDate"" ";                       

            var res = await _queryRepo.ExecuteQueryList<BLSApplicantViewModel>(query, null);
            return res;
        }

        public async Task<BLSVisaApplicationViewModel> GetVisaApplicationDetailsByServiceNo(string serviceNo = null)
        {
            var query = $@"Select vap.*,vaps.""Id"" as ApplicationServiceId
from public.""NtsService"" as s
join cms.""N_SNC_BLS_APPOINTMENT_BLS_VisaAppointment"" as va on s.""UdfNoteTableId""=va.""Id"" and va.""IsDeleted""=false
join cms.""N_SNC_BLS_BLS_VISA_SCHENGEN"" as vap on va.""Id""=vap.""AppointmentId"" and vap.""IsDeleted""=false
join public.""NtsService"" as vaps on vap.""Id""=vaps.""UdfNoteTableId"" and vaps.""IsDeleted""=false
where s.""IsDeleted""=false and s.""ServiceNo""='{serviceNo}' ";            

            var res = await _queryRepo.ExecuteQuerySingle<BLSVisaApplicationViewModel>(query, null);
            return res;
        }
    }
}
