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
    public class CommonServiceQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, ICommonServiceQueryBusiness
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
        public CommonServiceQueryPostgreBusiness(IRepositoryQueryBase<IdNameViewModel> querydata,
            IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<EGovCommunityHallViewModel> querych
            , IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<TaskViewModel> queryRepo1,
            IRepositoryQueryBase<ServiceTemplateViewModel> query, INoteBusiness noteBusiness, IUserContext userContext,
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
            _userContext = userContext;

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
       

        public async Task<OnlinePaymentViewModel> GetOnlinePayment(string id)
        {
            var existquery = $@"Select * from cms.""F_GENERAL_OnlinePayment"" where ""Id""='{id}' and ""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQuerySingle<OnlinePaymentViewModel>(existquery, null);
            return result;
        }

        public async Task<CSCReportViewModel> GetCSCBirthCertificateData(string serviceId)
        {
            var query = $@"select s.""Id"" as ""ServiceId"",s.""ServiceNo"",c.""PermanentLocalArea"" as ""LocalAreaName"",c.""PermanentNameOfTownCityVillage"" as ""TehasilBlockName"",pd.""Name"" as ""DistrictName""
                        ,psm.""Name"" as ""StateName"", c.""NameOfChild"" as ""Name"",g.""Name"" as ""GenderName"",c.""DateOfBirth"" as ""BirthDate"",pb.""Name"" as ""BirthPlace""
                        ,c.""NameofMother"" as ""MotherName"", c.""FatherFullName"" as ""FatherName"", c.""Address"" as ""CurrentAddress"",concat(c.""LocalArea"",', ',c.""NameOfTownCityVillage"") as ""CurrentAddress2""
                        ,concat(ca3.""Name"",', ',csa3.""Name"",' - ',c.""Pincode"") as ""CurrentAddress3"", c.""PermanentAddress"" as ""PermanentAddress"",concat(c.""PermanentLocalArea"",', ',c.""PermanentNameOfTownCityVillage"") as ""PermanentAddress2"",concat(pd.""Name"",', ',psm.""Name"",' - ',c.""PermanentPincode"") as ""PermanentAddress3""
                        , s.""ServiceNo"" as ""RegistrationNo""
                        ,s.""CreatedDate"" as ""RegistrationDate"", c.""ServiceCenterAgentRemark"" as ""Remarks"", s.""CompletedDate"" as ""IssueDate"", '' as ""AuthoritySignature"", '' as ""AuthorityAddress""
                        from public.""NtsService"" as s
                        join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='BIRTH_CERT_REQUEST' and t.""IsDeleted""=false
                        join cms.""N_SNC_CSC_BirthCertificateRequest"" as c on c.""NtsNoteId""=s.""UdfNoteId"" and c.""IsDeleted""=false
                        left join public.""LOV"" as g on g.""Id""=c.""GenderId"" and g.""IsDeleted""=false
                        left join public.""LOV"" as pb on pb.""Id""=c.""PlaceOfBirth"" and pb.""IsDeleted""=false
                        left join cms.""N_EGOV_MASTER_DATA_District"" as ca3 on ca3.""Id""=c.""DistrictId"" and ca3.""IsDeleted""=false
                        left join cms.""N_EGOV_MASTER_DATA_StateMaster"" as csa3 on csa3.""Id""=c.""StateId"" and csa3.""IsDeleted""=false
                        left join cms.""N_EGOV_MASTER_DATA_District"" as pd on pd.""Id""=c.""PermanentDistrictId"" and pd.""IsDeleted""=false
                        left join cms.""N_EGOV_MASTER_DATA_StateMaster"" as psm on psm.""Id""=c.""PermanentStateId"" and psm.""IsDeleted""=false
                        where s.""IsDeleted""=false and s.""Id""='{serviceId}'
                        ";
            var querydata = await _queryRepo.ExecuteQuerySingle<CSCReportViewModel>(query, null);
            return querydata;
        }
        public async Task<CSCReportMarriageCertificateViewModel> GetCSCMarriageCertificateData(string serviceId)
        {
            var query = $@"select s.""Id"" as ServiceId, s.""ServiceNo"" as ApplicationReferenceNumber, s.""CreatedDate"" as ElectronicApplicationDate,'' as ChoiceCenter
                        ,'5' as NetAmount, s.""ServiceNo"" as RegistrationNumber, s.""CompletedDate"" as ApprovedDate, s.""RequestedByUserId"" as RequestedByUserId
                        , c.""NameofGroominEnglish"" as GroomName, c.""FatherNameinEnglish"" as GroomFatherName, c.""groomage"" as GroomAge, c.""PermanentAddressofGroominEnglish"" as GroomAddress, c.""NameofGroominLocalLanguage"" as GroomNameHi, c.""FatherNameinlocallanguage"" as GroomFatherNameHi, c.""PermanentAddressofGroominLocalLanguage"" as GroomAddressHi, c.""uploadPhotographOfGroom"" as GroomImageId
                        , c.""NameofBrideinEnglish"" as BrideName, c.""FatherNameinEnglish3"" as BrideFatherName, c.""Brideage"" as BrideAge, c.""PermanentAddressofBrideinEnglish"" as BrideAddress, c.""NameofBrideinLocalLanguage"" as BrideNameHi, c.""FatherNameinlocallanguage3"" as BrideFatherNameHi, c.""PermanentAddressofBrideinLocalLanguage"" as BrideAddressHi, c.""uploadPhotographOfBride"" as BrideImageId
                        , c.""dateOfMarriage"" as MarriageDate,dist.""DistrictName"" as DistrictName,resdist.""Hindi"" as DistrictNameHi, oft.""OfficeName"" as MunicipalOfficeName, resoft.""Hindi"" as MunicipalOfficeNameHi, ut2.""Name"" as AuthorityName
                        from public.""NtsService"" as s
                        join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='CSC_MARRIAGE_CERTIFICATE' and t.""IsDeleted""=false
                        join cms.""N_SNC_CSC_SERVICES_Csc_Marriage_Certificate"" as c on c.""NtsNoteId""=s.""UdfNoteId"" and c.""IsDeleted""=false
                        left join public.""LOV"" as sst on sst.""Id"" = s.""ServiceStatusId"" and  sst.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_District"" as dist on dist.""Id""=c.""district"" and dist.""IsDeleted""=false
                        left join public.""FormResourceLanguage"" as resdist on resdist.""FormTableId""=dist.""Id"" and resdist.""IsDeleted""=false
                        left join cms.""F_CSC_SETTINGS_OfficeType"" as oft on oft.""Id""=c.""officeType"" and oft.""IsDeleted""=false
                        left join public.""FormResourceLanguage"" as resoft on resoft.""FormTableId""=oft.""Id"" and resoft.""IsDeleted""=false
                        left join public.""NtsTask"" as t1 on t1.""ParentServiceId"" =s.""Id"" and t1.""TemplateCode""='CscMarriageCerVerifier' and t1.""IsDeleted""=false
                        left join public.""User"" as ut1 on ut1.""Id""=t1.""AssignedToUserId"" and ut1.""IsDeleted""=false
                        left join public.""NtsTask"" as t2 on t2.""ParentServiceId"" =s.""Id"" and t2.""TemplateCode""='APPROVER' and t2.""IsDeleted""=false
                        left join public.""User"" as ut2 on ut2.""Id""=t2.""AssignedToUserId"" and ut2.""IsDeleted""=false
                        where s.""IsDeleted""=false and s.""Id""='{serviceId}'
                        ";
            var querydata = await _queryRepo.ExecuteQuerySingle<CSCReportMarriageCertificateViewModel>(query, null);
            if (querydata.IsNotNull())
            {
                var queryrole = $@"SELECT r.* FROM public.""User"" as u 
                                    join public.""UserRoleUser"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                                    join public.""UserRole"" as r on up.""UserRoleId""=r.""Id"" and r.""IsDeleted""=false
                                    where u.""IsDeleted""=false  and u.""Id""='{querydata.RequestedByUserId}' ";
                var queryroledata = await _queryRepo.ExecuteQueryList<UserRoleViewModel>(queryrole, null);
                if (queryroledata.IsNotNull())
                {
                    var roles = queryroledata.Select(x => x.Code).ToList();
                    if (roles.Contains("CSC_AGENT"))
                    {
                        querydata.ChoiceCenter = "लोक सेवा केंद्र";
                    }
                    else
                    {
                        querydata.ChoiceCenter = "चॉइस";
                    }
                }
            }
            //var querydata = new CSCReportMarriageCertificateViewModel
            //{
            //    ElectronicApplicationDate = new DateTime(2022, 06, 28),
            //    ApplicationReferenceNumber = "2303012226000258",
            //    ChoiceCenter = "लोकसेवा केंद्र",
            //    NetAmount = 5.00,
            //    RegistrationNumber = "2022-26-000261",
            //    GroomName = "Groom Name",
            //    GroomFatherName = "GroomFatherName",
            //    GroomAge = 25,
            //    GroomAddress = "GroomAddress",
            //    GroomNameHi = "दूल्हे का नाम",
            //    GroomFatherNameHi = "दूल्हे के पिता का नाम",
            //    GroomAddressHi = "दूल्हे का पता",
            //    BrideName = "BrideName",
            //    BrideFatherName = "BrideFatherName",
            //    BrideAge = 25,
            //    BrideAddress = "BrideAddress",
            //    BrideNameHi = "दुल्हन का नाम",
            //    BrideFatherNameHi = "दुल्हन के पिता का नाम",
            //    BrideAddressHi = "दुल्हन का पता",
            //    MarriageDate = new DateTime(2022, 05, 20),
            //    ApprovedDate = new DateTime(2022, 06, 30),
            //    DistrictName = "KONDAGAON",
            //    DistrictNameHi = "कोंडागांव",
            //    MunicipalOfficeName = "KONDAGAON MUNICIPAL COUNCIL",
            //    MunicipalOfficeNameHi = "कोण्डागांव नगर पालिका परिषद",
            //    AuthorityName = "XYZ"
            //};
            return querydata;
        }
        public async Task<CSCReportOBCCertificateViewModel> GetCSCOBCCertificateData(string serviceId)
        {
            var query = $@"select s.""Id"" as ServiceId, s.""ServiceNo"" as ApplicationReferenceNumber, s.""CreatedDate"" as ElectronicApplicationDate,c.""ChoiceCenter"" as ChoiceCenter
                        ,'5' as NetAmount,s.""ServiceNo"" as RegistrationNumber,c.""ApplicantName"" as ApplicantNameHi, c.""ApplicantName"" as ApplicantName
                        , c.""BeneficiaryGuardianName"" as ApplicantFatherNameHi,c.""BeneficiaryGuardianNameEnglish"" as ApplicantFatherName
                        , c.""PermanentAddress"" as ApplicantResidentHi, c.""PermanentAddress"" as ApplicantResident, sdist.""SubDistrictName"" as ApplicantTehasilHi, sdist.""SubDistrictName"" as ApplicantTehasil
                        , dist.""DistrictName"" as ApplicantDistrictHi,dist.""DistrictName"" as ApplicantDistrict, cas.""CastName"" as ApplicantCasteHi, cas.""CastName"" as ApplicantCaste
                        , '' as AuthorityName, s.""CompletedDate"" as IssueDate, upt.""Name"" as ProvisionalAuthorityName, uft.""Name"" as FinalAuthorityName
                        from public.""NtsService"" as s
                        join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='OBC_CERTIFICATE' and t.""IsDeleted""=false
                        join cms.""N_SNC_CSC_SERVICES_OBCCertificate"" as c on c.""NtsNoteId""=s.""UdfNoteId"" and c.""IsDeleted""=false
                        left join public.""LOV"" as sst on sst.""Id"" = s.""ServiceStatusId"" and  sst.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_SubDistrict"" as sdist on sdist.""Id""=c.""SubDistrictRevenue"" and sdist.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_District"" as dist on dist.""Id""=c.""PermanentDistrict"" and dist.""IsDeleted""=false
                        left join cms.""F_CSC_CASTE_CasteMaster"" as cas on cas.""Id""=c.""CasteOBC"" and cas.""IsDeleted""=false
                        left join public.""NtsTask"" as pt on pt.""ParentServiceId"" =s.""Id"" and pt.""TemplateCode""='OBCCertificateApproval2' and pt.""IsDeleted""=false
                        left join public.""User"" as upt on upt.""Id""=pt.""AssignedToUserId"" and upt.""IsDeleted""=false
                        left join public.""NtsTask"" as ft on ft.""ParentServiceId"" =s.""Id"" and ft.""TemplateCode""='OBCCertificateApproval4' and ft.""IsDeleted""=false
                        left join public.""User"" as uft on uft.""Id""=ft.""AssignedToUserId"" and uft.""IsDeleted""=false
                        where s.""IsDeleted""=false and s.""Id""='{serviceId}'
                        ";
            var querydata = await _queryRepo.ExecuteQuerySingle<CSCReportOBCCertificateViewModel>(query, null);
            return querydata;
        }
        public async Task<CSCReportAcknowledgementViewModel> GetCSCAcknowledgementData(string serviceId)
        {
            var serviceDetails = await _serviceBusiness.GetSingleById(serviceId);
            var query = string.Empty;
            if (serviceDetails.TemplateCode== "OBC_CERTIFICATE")
            {
                query = $@"select s.""Id"" as ServiceId, s.""ServiceNo"" as ApplicationReferenceNumber,c.""ApplicationDate"" as DateOfApplication
                        ,t.""DisplayName"" as ApplicationServiceName, c.""Amount"" as TotalFeesPaid, c.""PaymentReferenceNo"" as PaymentDetails
                        ,s.""DueDate"" as DeliveryDateOfService,'Certificate' as DeliverableDetails,ofty.""OfficeName"" as OfficeType, dist.""DistrictName"" as Distict
                        , sdist.""SubDistrictName"" as Tehsil, rev.""RevenueVillageName"" as RevenueVillage,c.""ApplicantName"" as ApplicantName, c.""PermanentAddress"" as ApplicantAddress
                        , pdist.""DistrictName"" as ApplicantDistict,  sta.""StateName"" as ApplicantState, c.""Email"" as ApplicantEmail, c.""MobileNo"" as ApplicantMobile
                        , uf.""Name"" as OfficerName, uf.""Address"" as OfficerAddress,uf.""Mobile"" as OfficerMobile
                        from public.""NtsService"" as s
                        join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='OBC_CERTIFICATE' and t.""IsDeleted""=false
                        join cms.""N_SNC_CSC_SERVICES_OBCCertificate"" as c on c.""NtsNoteId""=s.""UdfNoteId"" and c.""IsDeleted""=false
                        left join public.""LOV"" as sst on sst.""Id"" = s.""ServiceStatusId"" and  sst.""IsDeleted""=false
                        left join cms.""F_CSC_SETTINGS_OfficeType"" as ofty on ofty.""Id""=c.""OfficeType"" and ofty.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_District"" as dist on dist.""Id""=c.""District"" and dist.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_SubDistrict"" as sdist on sdist.""Id""=c.""SubDistrictRevenue"" and sdist.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_RevenueVillage"" as rev on rev.""Id""=c.""RevenueVillage"" and rev.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_District"" as pdist on pdist.""Id""=c.""PresentAddressDistrict"" and pdist.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_State"" as sta on sta.""Id""=pdist.""StateId"" and sta.""IsDeleted""=false
                        left join cms.""F_CSC_CASTE_CasteMaster"" as cas on cas.""Id""=c.""CasteOBC"" and cas.""IsDeleted""=false
                        left join public.""User"" as uf on uf.""Id""=s.""RequestedByUserId"" and uf.""IsDeleted""=false    
                        where s.""IsDeleted""=false and s.""Id""='{serviceId}'
                        ";
            }else if (serviceDetails.TemplateCode == "CSC_MARRIAGE_CERTIFICATE")
            {
                query = $@"select s.""Id"" as ServiceId, s.""ServiceNo"" as ApplicationReferenceNumber,c.""CreatedDate"" as DateOfApplication
                        ,t.""DisplayName"" as ApplicationServiceName, c.""Amount"" as TotalFeesPaid, c.""PaymentReferenceNo"" as PaymentDetails
                        ,s.""DueDate"" as DeliveryDateOfService,'Certificate' as DeliverableDetails,ofty.""OfficeName"" as OfficeType, dist.""DistrictName"" as Distict
                        , '' as Tehsil, '' as RevenueVillage, np.""NagarPanchayatName"" as NagarPanchayatName, mn.""MunicipalityName"" as MunicipalityName, zn.""ZoneName"" as ZoneName
                        , c.""nameOfApplicant"" as ApplicantName, c.""addressOfApplicant"" as ApplicantAddress
                        , dist.""DistrictName"" as ApplicantDistict,  sta.""StateName"" as ApplicantState, c.""email"" as ApplicantEmail, c.""mobileNo"" as ApplicantMobile
                        , uf.""Name"" as OfficerName, uf.""Address"" as OfficerAddress,uf.""Mobile"" as OfficerMobile
                        from public.""NtsService"" as s
                        join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='CSC_MARRIAGE_CERTIFICATE' and t.""IsDeleted""=false
                        join cms.""N_SNC_CSC_SERVICES_Csc_Marriage_Certificate"" as c on c.""NtsNoteId""=s.""UdfNoteId"" and c.""IsDeleted""=false
                        left join public.""LOV"" as sst on sst.""Id"" = s.""ServiceStatusId"" and  sst.""IsDeleted""=false
                        left join cms.""F_CSC_SETTINGS_OfficeType"" as ofty on ofty.""Id""=c.""officeType"" and ofty.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_District"" as dist on dist.""Id""=c.""district"" and dist.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_State"" as sta on sta.""Id""=dist.""StateId"" and sta.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_NagarPanchayat"" as np on np.""Id""=c.""NagarPanchayat"" and np.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_Municipality"" as mn on mn.""Id""=c.""Muncipality"" and mn.""IsDeleted""=false
                        left join cms.""F_CSC_GEOGRAPHIC_Zone"" as zn on zn.""Id""=c.""Zone"" and zn.""IsDeleted""=false
                        --left join cms.""F_CSC_GEOGRAPHIC_SubDistrict"" as sdist on sdist.""Id""=c.""SubDistrictRevenue"" and sdist.""IsDeleted""=false
                        --left join cms.""F_CSC_GEOGRAPHIC_RevenueVillage"" as rev on rev.""Id""=c.""RevenueVillage"" and rev.""IsDeleted""=false
                        --left join cms.""F_CSC_GEOGRAPHIC_District"" as pdist on pdist.""Id""=c.""PresentAddressDistrict"" and pdist.""IsDeleted""=false
                        --left join cms.""F_CSC_CASTE_CasteMaster"" as cas on cas.""Id""=c.""CasteOBC"" and cas.""IsDeleted""=false
                        left join public.""User"" as uf on uf.""Id""=s.""RequestedByUserId"" and uf.""IsDeleted""=false    
                        where s.""IsDeleted""=false and s.""Id""='{serviceId}'
                        ";
            }

            var querydata = await _queryRepo.ExecuteQuerySingle<CSCReportAcknowledgementViewModel>(query, null);
            return querydata;
        }
        public async Task<List<ServiceChargeViewModel>> GetServiceChargeData(string serviceId)
        {
            string query = $@"select cm.""ChargeName"",cm.""ChargeCode"", sc.""Amount"",sc.""FeeExcemptionQuantity""--,udf.*
                                from public.""NtsService"" as s
                                join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false
                                --join cms.""N_SNC_CSC_SERVICES_OBCCertificate"" as udf on s.""UdfNoteTableId""=udf.""Id"" and udf.""IsDeleted""=false
                                left join cms.""F_CSC_CHARGES_ServiceCharge"" as sc on t.""Id""=sc.""TemplateId"" and sc.""IsDeleted""=false
                                left join cms.""F_CSC_CHARGES_ChargeMaster"" as cm on sc.""ChargeId""=cm.""Id"" and cm.""IsDeleted""=false
                                where s.""Id""='{serviceId}' and s.""IsDeleted""=false ";
            var querydata = await _queryRepo1.ExecuteQueryList<ServiceChargeViewModel>(query, null);
            return querydata;
        }
        public async Task<List<CSCTrackApplicationViewModel>> GetTrackApplicationList(string applicationNo)
        {
            var querydata = new List<CSCTrackApplicationViewModel>();
            //querydata.Add(new CSCTrackApplicationViewModel
            //{
            //    ApplicationServiceName = "अन्य पिछड़ा वर्ग प्रमाण पत्र / OBC Certificate",
            //    ApplicationReferenceNumber = "0705012211002792",
            //    ApplicantName = "विजय दास महंत",
            //    DateOfApplication = new DateTime(2022, 05, 27, 10, 45, 13),
            //    DeliveryDateOfService = new DateTime(2022, 07, 25, 12, 56, 34),
            //    IssueDate = new DateTime(2022, 06, 16, 17, 44, 27),
            //    ServiceStatus = "अनुमोदित",
            //    Remarks = "NA",
            //    Acknowledgement = true,
            //    ProvisionalCertificate =true,
            //    Certificate = true
            //});

            var query = $@"select s.""Id"" as ServiceId, s.""ServiceNo"" as ApplicationReferenceNumber,t.""DisplayName"" as ApplicationServiceName,s.""TemplateCode"" as ServiceTemplateCode
                        ,c.""ApplicantName"" as ApplicantName, c.""ApplicationDate"" as DateOfApplication, s.""DueDate"" as DeliveryDateOfService,s.""CompletedDate"" as IssueDate
                        , sst.""Name"" as ServiceStatus, '' as Remarks,c.""Acknowledgment"" as Acknowledgement, c.""ProvisionalCertificate"" as ProvisionalCertificate,c.""Certificate"" as Certificate
                        , '' as CertificateFileId
                        from public.""NtsService"" as s
                        join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='OBC_CERTIFICATE' and t.""IsDeleted""=false
                        join cms.""N_SNC_CSC_SERVICES_OBCCertificate"" as c on c.""NtsNoteId""=s.""UdfNoteId"" and c.""IsDeleted""=false
                        left join public.""LOV"" as sst on sst.""Id"" = s.""ServiceStatusId"" and  sst.""IsDeleted""=false
                        where s.""IsDeleted""=false and s.""ServiceNo""='{applicationNo}'
                        union
                        select s.""Id"" as ServiceId, s.""ServiceNo"" as ApplicationReferenceNumber, t.""DisplayName"" as ApplicationServiceName,s.""TemplateCode"" as ServiceTemplateCode
                        , c.""nameOfApplicant"" as ApplicantName, c.""CreatedDate""::Text as DateOfApplication, s.""DueDate"" as DeliveryDateOfService, s.""CompletedDate"" as IssueDate
                        , sst.""Name"" as ServiceStatus, '' as Remarks, c.""Acknowledgment"" as Acknowledgement, 'false' as ProvisionalCertificate, c.""Certificate"" as Certificate
                        , c.""marriageCertificateFileId"" as CertificateFileId
                        from public.""NtsService"" as s
                        join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='CSC_MARRIAGE_CERTIFICATE' and t.""IsDeleted""=false
                        join cms.""N_SNC_CSC_SERVICES_Csc_Marriage_Certificate"" as c on c.""NtsNoteId""=s.""UdfNoteId"" and c.""IsDeleted""=false
                        left join public.""LOV"" as sst on sst.""Id"" = s.""ServiceStatusId"" and  sst.""IsDeleted""=false
                        where s.""IsDeleted""=false and s.""ServiceNo""='{applicationNo}'
                        ";
            querydata = await _queryRepo.ExecuteQueryList<CSCTrackApplicationViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<TaskViewModel>> GetCSCTaskList(string portalId)
        {
            
            string query = @$"Select t.""Id"",t.""TaskNo"",s.""ServiceNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",b.""Id"" as UdfNoteTableId, b.""ApplicationFee"" as ""ServiceCost"",
                            ts.""GroupCode"" as ""StatusGroupCode"",
                            ps.""Name"" as ""PaymentStatus"",ps.""Code"" as ""PaymentStatusCode"",
                            b.""PaymentReferenceNo"" as ""ReferenceNumber""
                            From public.""NtsTask"" as t
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false                             
                            Join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false
                            Join cms.""N_SNC_CSC_BirthCertificateRequest"" as b on b.""Id"" = s.""UdfNoteTableId"" and b.""IsDeleted""=false
                            Left Join public.""LOV"" as ps on b.""PaymentStatusId""=ps.""Id"" and ps.""IsDeleted""=false  
                            where t.""PortalId"" in ('{portalId}') -- and t.""RequestedByUserId""='{_repo.UserContext.UserId}' 
                      and t.""TemplateCode"" in ('PayBirthCertificateApplicationFee') and t.""IsDeleted""=false 
                     
            ";

            var result = await _queryRepo1.ExecuteQueryList(query, null);
            
            return result;
        }

        public async Task<IList<ServiceChargeViewModel>> GetServiceChargeDetails(string serviceId)
        {
            var userroleIds = _repo.UserContext.UserRoleIds.Replace(",","','");

            string query = $@"select cm.""ChargeName"",cm.""ChargeCode"", sc.""Amount"",sc.""FeeExcemptionQuantity""--,udf.*
                                from public.""NtsService"" as s
                                join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false
                                --join cms.""N_SNC_CSC_SERVICES_OBCCertificate"" as udf on s.""UdfNoteTableId""=udf.""Id"" and udf.""IsDeleted""=false
                                left join cms.""F_CSC_CHARGES_ServiceCharge"" as sc on t.""Id""=sc.""TemplateId"" and sc.""IsDeleted""=false
                                left join cms.""F_CSC_CHARGES_ChargeMaster"" as cm on sc.""ChargeId""=cm.""Id"" and cm.""IsDeleted""=false
                                where s.""Id""='{serviceId}' and s.""IsDeleted""=false and sc.""UserRoleId"" in ('{userroleIds}') ";

            var result = await _queryRepo1.ExecuteQueryList<ServiceChargeViewModel>(query, null);

            return result;
        }

        public async Task<long> GetDocumentsCount(string udfnotetableId)
        {
            string query = $@"select column_name as Name from information_schema.columns 
                  where table_name='N_SNC_CSC_SERVICES_OBCCertificate' and column_name ilike 'Document%' and column_name ilike '%id' ";

            var columns = await _querydata.ExecuteQueryList(query, null);

            string select="";
            var i = 1;
            foreach (var col in columns)
            {                              
                var str = i == columns.Count ? " Is Not Null then 1 else 0 end as Count " : " Is Not Null then 1 else 0 end + ";
                select = string.Concat(select, $@"Case when ""{col.Name}"" " , str);
                i++;
            }

            string query1 = $@"select {select} from cms.""N_SNC_CSC_SERVICES_OBCCertificate"" where ""Id""='{udfnotetableId}' ";

            var res = await _querydata.ExecuteQuerySingle(query1, null);            

            return res.Count;

        }


    }
}
