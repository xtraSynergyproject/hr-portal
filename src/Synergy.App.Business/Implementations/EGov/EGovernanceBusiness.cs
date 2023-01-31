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
        private readonly IEGovernanceQueryBusiness _EGovernanceQueryBusiness;
        private readonly IUserContext _userContext;
        private readonly ICmsBusiness _cmsBusiness;
        public EGovernanceBusiness(IRepositoryQueryBase<IdNameViewModel> querydata,
            IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<EGovCommunityHallViewModel> querych
            , IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<TaskViewModel> queryRepo1,
            IRepositoryQueryBase<ServiceTemplateViewModel> query, INoteBusiness noteBusiness, ICmsBusiness cmsBusiness,
            IEGovernanceQueryBusiness eGovernanceQueryBusiness,
            ILOVBusiness lOVBusiness, IServiceBusiness serviceBusiness, IUserBusiness userBusiness, IUserContext userContext) : base(repo, autoMapper)
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
            _EGovernanceQueryBusiness = eGovernanceQueryBusiness;
            _userContext = userContext;
            _cmsBusiness = cmsBusiness;

        }


        public async Task<List<EGovCommunityHallViewModel>> GetCommunityHallBookingList(string hallId)
        {
            var result = await _EGovernanceQueryBusiness.GetCommunityHallBookingList(hallId);
            return result;

        }

        public async Task<List<EGovCorporationWardViewModel>> GetCorporatorList(string wardNo = null, string wardName = null, string councillorName = null, string address = null, string phone = null)
        {

            var result = await _EGovernanceQueryBusiness.GetCorporatorList(wardNo, wardName, councillorName, address, phone);
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
        public async Task<List<IdNameViewModel>> GetEGovAdminConstituencyIdNameList()
        {
            var result = await _EGovernanceQueryBusiness.GetEGovAdminConstituencyIdNameList();
            return result;
        }
        public async Task<List<EGovCorporationWardViewModel>> GetAdminWardList(string wardNo = null, string wardName = null, string electoralWardName = null, string location = null, string constituencyName = null, string latitude = null, string longitude = null)
        {
            var result = await _EGovernanceQueryBusiness.GetAdminWardList(wardNo, wardName, electoralWardName, location, constituencyName, latitude, longitude);
            foreach (var item in result)
            {
                var corporateWardIds = item.ElectoralWardId.Split(",");
                //var s= string.Join("','", item.ElectoralWardId);
                var s = item.ElectoralWardId.Replace("\"", "'");
                //       var wardquery = $@"SELECT string_agg(concat(cw.""WardNo""::text,' ',cw.""WardName""::text), ',') as ""WardName"" 

                // FROM cms.""N_E_GOV_CorporationWard"" cw
                //where cw.""IsDeleted"" = false and cw.""Id"" in (" + s + ")";
                var corpwardname = await _EGovernanceQueryBusiness.GetWardListQuery(s);
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
            var result = await _EGovernanceQueryBusiness.GetAdminOfficersWardList(wardNo, wardName, officerName, location, phone);
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

            var result = await _EGovernanceQueryBusiness.GetMyRequestList(showAllOwnersService, moduleCodes, templateCodes, categoryCodes, searchtext, From, To, statusIds, templateIds);
            return result;
        }
        public async Task<List<MapMarkerViewModel>> GetMapMarkerDeatils()
        {
            var list = await _EGovernanceQueryBusiness.GetMapMarkerDeatils();
            return list.ToList();
        }

        public async Task<List<EgovGISLocationViewModel>> GetGISLocationDetails()
        {
            var list = await _EGovernanceQueryBusiness.GetGISLocationDetails();
            return list.ToList();
        }

        public async Task<List<EGOVBannerViewModel>> GetEGovSliderBannerData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSliderBannerData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovProjectMasterData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovProjectMasterData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovLatestNewsData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovLatestNewsData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovNotificationMasterData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovNotificationMasterData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCNotificationData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCNotificationData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCTenderData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCTenderData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCOrderData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCOrderData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCCircularData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCCircularData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCDownloadsData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCDownloadsData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCOrderCircularData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCOrderCircularData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCNewsData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCNewsData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCPublicationData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCPublicationData();
            return list.ToList();
        }
        public async Task<List<EGovCorporationWardViewModel>> GetAdministrativeWardsList()
        {
            var result = await _EGovernanceQueryBusiness.GetAdministrativeWardsList();
            return result;

        }
        public async Task<EGovCorporationWardViewModel> GetWardConstituencyMappingList(string wardIds)
        {
            var result = await _EGovernanceQueryBusiness.GetWardConstituencyMappingList(wardIds);
            return result;
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCWardMapsData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCWardMapsList();
            return list.ToList();
        }
        public async Task<EGOVBannerViewModel> GetSrinagarSettingData(string code)
        {
            var data = await _EGovernanceQueryBusiness.GetSrinagarSettingData(code);
            return data;
        }
        public async Task<List<EGOVBannerViewModel>> GetSSCActnByeLawsData(string val)
        {
            var data = await _EGovernanceQueryBusiness.GetSSCActnByeLawsData(val);
            return data;
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCCorporatorsData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCCorporatorsData();
            return list.ToList();
        }
        public async Task<EGOVCommitteeMasterViewModel> GetEGovSSCCommitteeMasterData(string committeeCode)
        {
            var result = await _EGovernanceQueryBusiness.GetEGovSSCCommitteeMasterData(committeeCode);
            if (result.IsNotNull())
            {
                result.CommitteeMemberList = await _EGovernanceQueryBusiness.GetEGovSSCCommitteeMemberListByCommitteeId(result.CommitteeId);
                result.CommitteeFunctionList = await _EGovernanceQueryBusiness.GetEGovSSCCommitteeFunctionListByCommitteeId(result.CommitteeId);
            }
            return result;
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovSSCCommitteeMemberData(string committeeCode)
        {
            var list = await _EGovernanceQueryBusiness.GetEGovSSCCommitteeMemberData(committeeCode);
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovCorporatePhotoData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovCorporatePhotoData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovTenderMasterData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovTenderMasterData();
            return list.ToList();
        }
        public async Task<List<EGOVBannerViewModel>> GetEGovOtherWebsiteData()
        {
            var list = await _EGovernanceQueryBusiness.GetEGovOtherWebsiteData();
            return list.ToList();
        }
        public async Task<double> GetEGovDebrisRateData()
        {
            var data = await _EGovernanceQueryBusiness.GetEGovDebrisRateData();
            return data;
        }
        public async Task<double> GetEGovPoultryCostData()
        {
            var data = await _EGovernanceQueryBusiness.GetEGovPoultryCostData();
            return data;
        }
        public async Task<double> GetEGovSepticTankCostData()
        {
            var data = await _EGovernanceQueryBusiness.GetEGovSepticTankCostData();
            return data;
        }
        public async Task<double> GetEGovBinBookingCostData(string binSizeId)
        {
            var data = await _EGovernanceQueryBusiness.GetEGovBinBookingCostData(binSizeId);
            return data;
        }
        public async Task<IList<ServiceTemplateViewModel>> GetServiceList(string statusCodes, string templateCode)
        {
            var result = await _EGovernanceQueryBusiness.GetServiceList(statusCodes, templateCode);

            return result;
        }

        public async Task<EGovBinBookingViewModel> GetExistingBinBookingDetails(string consumerNo)
        {
            var result = await _EGovernanceQueryBusiness.GetExistingBinBookingDetails(consumerNo);
            return result;
        }

        public async Task<bool> UpdateBinBookingDetails(dynamic udf)
        {
            if (udf != null)
            {
                await _EGovernanceQueryBusiness.UpdateBinBookingDetails(udf);
                return true;
            }
            return false;
        }

        public async Task<EGovSewerageViewModel> GetExistingSewerageDetails(string consumerNo)
        {
            var result = await _EGovernanceQueryBusiness.GetExistingSewerageDetails(consumerNo);
            return result;
        }
        public async Task<List<ServiceTemplateViewModel>> GetCommercialTaxService()
        {
            var result = await _EGovernanceQueryBusiness.GetCommercialTaxService();
            return result;

        }
        public async Task<List<ServiceTemplateViewModel>> GetResidentialTaxService()
        {
            var result = await _EGovernanceQueryBusiness.GetResidentialTaxService();
            return result;
        }

        public async Task<List<ServiceTemplateViewModel>> GetTradeTaxService()
        {
            var result = await _EGovernanceQueryBusiness.GetTradeTaxService();
            return result;
        }

        public async Task<IList<TaskViewModel>> GetTaskList(string portalId)
        {
            var result = await _EGovernanceQueryBusiness.GetTaskList(portalId);
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

            var result = await _EGovernanceQueryBusiness.GetPropertyList(wardId, rentingType);
            return result;
        }

        public async Task<EGovRentalViewModel> GetAgreementDetails(string agreementNo)
        {
            var result = await _EGovernanceQueryBusiness.GetAgreementDetails(agreementNo);
            return result;
        }
        public async Task<bool> UpdateRenewalEndDate(dynamic udf)
        {
            if (udf != null)
            {
                await _EGovernanceQueryBusiness.UpdateRenewalEndDate(udf);
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
                    await _EGovernanceQueryBusiness.UpdateRentalStatus(rentalstatus.Id, udf.RentalAgreementNumber);
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

                    await _EGovernanceQueryBusiness.RentalStatus(rentalstatus.Id, udf.UdfNoteTableId);
                    return true;
                }
            }

            return false;
        }

        public async Task<CommandResult<OnlinePaymentViewModel>> UpdateOnlinePaymentDetails(OnlinePaymentViewModel model)
        {
            var result = await _EGovernanceQueryBusiness.UpdateOnlinePaymentDetails(model);

            //var result = await _EGovernanceQueryBusiness.UpdateOnlinePaymentDetails(model);

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
                await _EGovernanceQueryBusiness.UpdateOnlinePaymentDetailsData(model, date);
            }



            // return commandresult - if paymentstatus is having value then return message with payment initiated and status
            return CommandResult<OnlinePaymentViewModel>.Instance(model);

        }
        public async Task UpdateOnlinePayment(OnlinePaymentViewModel responseViewModel)
        {
            await _EGovernanceQueryBusiness.UpdateOnlinePayment(responseViewModel);
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
            var result = await _EGovernanceQueryBusiness.GetOnlinePayment(id);
            return result;
        }
        public async Task<List<EGovProjectViewModel>> GetUpcomingProjectList(string categoryId, string wardId)
        {
            var result = await _EGovernanceQueryBusiness.GetUpcomingProjectList(categoryId, wardId);
            return result;
        }
        public async Task<bool> DeleteUpcomingProject(string projectId)
        {
            var data = await _EGovernanceQueryBusiness.DeleteUpcomingProject(projectId);
            return data;
        }
        public async Task<List<EGovCommunityHallViewModel>> GetCommunityHallList()
        {
            var result = await _EGovernanceQueryBusiness.GetCommunityHallList();
            var wardList = await _lOVBusiness.GetList(x => x.LOVType == "EGOV_ELECTORAL_WARD");

            if (result.IsNotNull())
            {
                foreach (var item in result)
                {
                    if (item.Ward.IsNotNullAndNotEmpty())
                    {
                        var wardids = item.Ward.Trim('[', ']');
                        var wardIdList = wardids.Replace("\"", "").Replace("\\r", "").Replace("\\n", "").Split(",");
                        var wards = wardList.Where(x => wardIdList.Any(y => y.Trim() == x.Id)).ToList();
                        if (wards.Any())
                        {
                            item.WardName = string.Join(", ", wards.Select(x => x.Name));
                        }

                        //var data = await _EGovernanceQueryBusiness.GetWardList(wardids);
                        //if (data.IsNotNullAndNotEmpty())
                        //{
                        //    item.WardName = data;
                        //}
                    }
                }
            }
            return result;


        }
        public async Task<List<NewDashboardViewModel>> GetDashboardList()
        {
            var result = await _EGovernanceQueryBusiness.GetDashboardList();
            return result;
        }

        public async Task<List<EGovDashboardViewModel>> GetProposalProjectsList(string type, string userId)
        {
            var result = await _EGovernanceQueryBusiness.GetProposalProjectsList(type, userId);
            return result;
        }

        public async Task<List<EGovDashboardViewModel>> GetAllProposalProjectsList()
        {
            var res = await _EGovernanceQueryBusiness.GetAllProposalProjectsList();
            return res;
        }

        public async Task<List<EGovDashboardViewModel>> GetJSCProposalProjectsList(string type, string userId)
        {
            var result = await _EGovernanceQueryBusiness.GetJSCProposalProjectsList(type, userId);
            return result;
        }

        public async Task<EGovProjectProposalResponseViewModel> GetProposalLikesData(string proposalId, ProjectPropsalResponseEnum type, string userId)
        {
            var result = await _EGovernanceQueryBusiness.GetProposalLikesData(proposalId, type, userId);
            return result;
        }

        public async Task UpdateProjectProposalLikes(string proposalId, ProjectPropsalResponseEnum? type, string userId, DataActionEnum action)
        {
            await _EGovernanceQueryBusiness.UpdateProjectProposalLikes(proposalId, type, userId, action);
        }

        public async Task<List<EGovDashboardViewModel>> GetProposalProjectsCount(string type = null)
        {
            return await _EGovernanceQueryBusiness.GetProposalProjectsCount(type);
        }
        public async Task<EGovDashboardViewModel> ViewProjectsUnderTaken(string serviceId)
        {
            var result = await _EGovernanceQueryBusiness.ViewProjectsUnderTaken(serviceId);
            return result;
        }
        public async Task<EGovDashboardViewModel> GetWardData(string UserId)
        {
            var result = await _EGovernanceQueryBusiness.GetWardData(UserId);
            return result;
        }
        public async Task<List<EGovDashboardViewModel>> GetNWTimeLineData()
        {
            var result = await _EGovernanceQueryBusiness.GetNWTimeLineData();
            return result;
        }

        public async Task<FacilityViewModel> GetFacilityDetails(string facilityCode)
        {
            var result = await _EGovernanceQueryBusiness.GetFacilityDetails(facilityCode);
            return result;
        }

        public async Task<IList<FacilityViewModel>> GetFacilityList(string userId = null)
        {
            var result = await _EGovernanceQueryBusiness.GetFacilityList(userId);
            return result;
        }

        public async Task<IList<FacilityLocationViewModel>> GetFacilityLocationList()
        {
            var result = await _EGovernanceQueryBusiness.GetFacilityLocationList();
            return result;
        }

        public async Task<FacilityViewModel> GetPreviousFacilityStatus(string facilityCode)
        {
            var result = await _EGovernanceQueryBusiness.GetPreviousFacilityStatus(facilityCode);
            return result;
        }
        public async Task<bool> ValidateNeedsWantsTimelineFromDateandToDate(FormTemplateViewModel viewModel)
        {

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var fromDate = Convert.ToString(rowData.GetValueOrDefault("fromDate"));
            var toDate = Convert.ToString(rowData.GetValueOrDefault("ToDate"));
            var newId = viewModel.Id;
            var query =
                $@" select ""Id""
                    from cms.""F_EGOVERNANCE_NeedsandWantsTimeline"" 
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""fromDate"">='{fromDate}' and ""fromDate""<='{toDate}')
                                                    or
                                                         (""ToDate"">='{fromDate}' and ""ToDate""<='{toDate}')) and ""Id""!='{newId}'";

            var exisiting = await _queryRepo1.ExecuteQueryList(query, null);
            if (exisiting.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<IList<NtsTaskIndexPageViewModel>> GetNeedsAndWantsTaskCount(string categoryCodes, string portalId, bool showAllTaskForAdmin = true)
        {
            var result = await _EGovernanceQueryBusiness.GetNeedsAndWantsTaskCount(categoryCodes, portalId, showAllTaskForAdmin);

            return result;
        }

        public async Task<IList<EGovProjectProposal>> GetNeedsAndWantsTaskList(string categoryCodes, string status, string portalId, bool showAllTaskForAdmin)
        {
            var result = await _EGovernanceQueryBusiness.GetNeedsAndWantsTaskList(categoryCodes, status, portalId, showAllTaskForAdmin);
            return result;
        }

        public async Task<IList<EGovProjectProposal>> ProjectsByLatLong()
        {
            var result = await _EGovernanceQueryBusiness.ProjectsByLatLong();
            return result;
        }

        public async Task<bool> UpdateProjectProposalStatus(dynamic udf, string tsCode)
        {
            if (udf != null)
            {
                var ppcode = tsCode == "TASK_STATUS_COMPLETE" ? "EGOV_PRO_STATUS_COMPLETED" : "EGOV_PRO_STATUS_REJECTED";

                var pps = await _lOVBusiness.GetSingle(x => x.Code == ppcode);
                await _EGovernanceQueryBusiness.UpdateProjectProposalStatus(udf, pps.Id);
                return true;
            }
            return false;
        }
        public async Task<IList<ServiceTemplateViewModel>> GetDraftedServiceList(string statusCodes, string templateCode, string categoryCode)
        {
            var result = await _EGovernanceQueryBusiness.GetDraftedServiceList(statusCodes, templateCode, categoryCode);

            return result;
        }

        public async Task<JSCAssetConsumerViewModel> ReadJSCAssetConsumerData(string consumerId)
        {
            var result = await _EGovernanceQueryBusiness.ReadJSCAssetConsumerData(consumerId);
            return result;
        }

        public async Task<IList<JSCAssetConsumerViewModel>> GetJSCAssetsDataByConsumer(string userId)
        {
            var result = await _EGovernanceQueryBusiness.GetJSCAssetsDataByConsumer(userId);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetWardListFromMaster()
        {
            var res = await _EGovernanceQueryBusiness.GetWardListFromMaster();
            return res;
        }
        
        public async Task<List<IdNameViewModel>> GeCollectorListForJammu()
        {
            var res = await _EGovernanceQueryBusiness.GeCollectorListForJammu();
            return res;
        }
        
        public async Task<List<IdNameViewModel>> GetAssetTypeListForJammu()
        {
            var res = await _EGovernanceQueryBusiness.GetAssetTypeListForJammu();
            return res;
        }

        public async Task<List<IdNameViewModel>> GetJammuCollectorList()
        {
            var res = await _EGovernanceQueryBusiness.GetJammuCollectorList();
            return res;
        }

        public async Task<List<CollectorWardAssignmentViewModel>> GetAssignedWardCollectorList()
        {
            var res = await _EGovernanceQueryBusiness.GetAssignedWardCollectorList();
            return res;
        }

        public async Task<CollectorWardAssignmentViewModel> GetWardCollectorById(string id)
        {
            var res = await _EGovernanceQueryBusiness.GetWardCollectorById(id);
            return res;
        }

        public async Task DeleteWardCollector(string id)
        {
            await _EGovernanceQueryBusiness.DeleteWardCollector(id);
        }
        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentReport(string source, string assetType, string ward, DateTime? From, DateTime? To)
        {
            var res = await _EGovernanceQueryBusiness.GetAssetPaymentReport(source,assetType,ward, From, To);
            return res;
        }
        public async Task<bool> ValidateAssetFeeTimelineFromDateandToDate(FormTemplateViewModel viewModel)
        {

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var startDate = Convert.ToString(rowData.GetValueOrDefault("FeeStartDate"));
            var endDate = Convert.ToString(rowData.GetValueOrDefault("FeeEndDate"));
            var assetId = Convert.ToString(rowData.GetValueOrDefault("AssetId"));
            var newId = viewModel.Id;
            var query =
                $@" select ""Id""
                    from cms.""F_JSC_REV_JSC_ASSET_FEE"" 
                        where  ""IsDeleted""='false' and ((""FeeStartDate"">='{startDate}' and ""FeeEndDate""<='{endDate}')
                                                    or
                                                         (""FeeEndDate"">='{endDate}' and ""FeeStartDate""<='{startDate}')) and ""Id""!='{newId}' and  ""AssetId""='{assetId}' ";

            var exisiting = await _queryRepo1.ExecuteQueryList(query, null);    
            if (exisiting.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<List<UserViewModel>> GetUnallocationUserList()
        {
            var res = await _EGovernanceQueryBusiness.GetUnallocationUserList();
            return res;
        }

        public async Task<List<IdNameViewModel>> GetUnAllocatedAssetFilterByFromnToDate(string fromDate, string toDate)
        {
            var queryData = await _EGovernanceQueryBusiness.GetUnAllocatedAssetFilterByFromnToDate(fromDate, toDate);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetUnAllocatedWard()
        {
            var queryData = await _EGovernanceQueryBusiness.GetUnAllocatedWard();
            return queryData;
        }
        public async Task<List<EGOVBannerViewModel>> GetVideoGallery(string videoTypeCode)
        {
            var queryData = await _EGovernanceQueryBusiness.GetVideoGallery(videoTypeCode);
            return queryData;
        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentReport2(string source, string assetType, string ward, DateTime? From, DateTime? To)
        {
            var res = await _EGovernanceQueryBusiness.GetAssetPaymentReport2(source, assetType, ward, From, To);
            return res;
        }
        public async Task<bool> GetEmailAndNameEnforcement(string Email, string Name,string Id)
        {
            var query = $@"select ""SubName"" as Name,""Id"" as Code from cms.""F_JSC_ENFORCEMENT_SubLogins""
                        where ""IsDeleted""='false' and (""SubUserName"" = '{Name}' or ""SubEmail"" = '{Email}') #IdFilter";
            var idFilter = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                idFilter=$@"and ""Id""<>'{Id}'";
            }
            query = query.Replace("#IdFilter", idFilter);
            var querydata = await _queryRepo.ExecuteQuerySingle<SynergyIdNameViewModel>(query, null);
            if (querydata.IsNotNull())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CreateUserEntry(EGOVUserDetailsViewModel model)
        {
            var wardDetails = await _lOVBusiness.GetSingle(x => x.Name == model.WardName && x.LOVType == "EGOV_ELECTORAL_WARD" && x.IsDeleted == false);
            model.WardId = wardDetails.Id;
            var formTempModel = new FormTemplateViewModel();
            formTempModel.DataAction = DataActionEnum.Create;
            formTempModel.TemplateCode = "EGOV_USER_DETAILS";
            var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
            formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var res = await _cmsBusiness.ManageForm(formmodel);
            if (res.IsSuccess)
            {
                return true;
            }
            return false;
        }

    }
}
