﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.EGOV;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IEGovernanceBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<EGovCommunityHallViewModel>> GetCommunityHallBookingList(string hallId);
        Task<IList<ServiceTemplateViewModel>> GetMyRequestList(bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string searchtext = null, DateTime? From = null, DateTime? To = null, string statusCodes = null, string templateIds = null);
        Task<IList<ServiceTemplateViewModel>> GetServiceList(string statusCodes, string templateCode);
        Task<List<MapMarkerViewModel>> GetMapMarkerDeatils();
        Task<List<EgovGISLocationViewModel>> GetGISLocationDetails();
        Task<List<EGOVBannerViewModel>> GetEGovSliderBannerData();
        Task<List<EGOVBannerViewModel>> GetEGovProjectMasterData();
        Task<List<EGOVBannerViewModel>> GetEGovLatestNewsData();
        Task<List<EGOVBannerViewModel>> GetEGovNotificationMasterData();
        Task<List<EGOVBannerViewModel>> GetEGovSSCNotificationData();
        Task<List<EGOVBannerViewModel>> GetEGovSSCTenderData();
        Task<List<EGOVBannerViewModel>> GetEGovSSCOrderData();
        Task<List<EGOVBannerViewModel>> GetEGovSSCCircularData();
        Task<List<EGOVBannerViewModel>> GetEGovSSCDownloadsData();
        Task<List<EGOVBannerViewModel>> GetEGovSSCOrderCircularData();
        Task<List<EGOVBannerViewModel>> GetEGovSSCNewsData();
        Task<List<EGOVBannerViewModel>> GetEGovSSCPublicationData();
        Task<List<EGovCorporationWardViewModel>> GetAdministrativeWardsList();
        Task<EGovCorporationWardViewModel> GetWardConstituencyMappingList(string wardIds);
        Task<List<EGOVBannerViewModel>> GetEGovSSCWardMapsData();
        Task<EGOVBannerViewModel> GetSrinagarSettingData(string code);
        Task<List<EGOVBannerViewModel>> GetSSCActnByeLawsData(string val);
        Task<List<EGOVBannerViewModel>> GetEGovSSCCorporatorsData();
        Task<EGOVCommitteeMasterViewModel> GetEGovSSCCommitteeMasterData(string committeeCode);
        Task<List<EGOVBannerViewModel>> GetEGovSSCCommitteeMemberData(string committeeCode);
        Task<List<EGOVBannerViewModel>> GetEGovCorporatePhotoData();
        Task<List<EGOVBannerViewModel>> GetEGovTenderMasterData();
        Task<List<EGOVBannerViewModel>> GetEGovOtherWebsiteData();
        Task<double> GetEGovDebrisRateData();
        Task<double> GetEGovPoultryCostData();
        Task<double> GetEGovSepticTankCostData();
        Task<double> GetEGovBinBookingCostData(string binSizeId);
        Task<List<IdNameViewModel>> GetEGovAdminConstituencyIdNameList();
        Task<List<EGovCorporationWardViewModel>> GetCorporatorList(string wardNo = null, string wardName = null, string councillorName = null, string address = null, string phone = null);
        Task<List<EGovCorporationWardViewModel>> GetAdminWardList(string wardNo = null, string wardName = null, string electoralWardName = null, string location = null, string constituencyName = null, string latitude = null, string longitude = null);
        Task<List<EGovCorporationWardViewModel>> GetAdminOfficersWardList(string wardNo = null, string wardName = null, string officerName = null, string location = null, string phone = null);
        Task<EGovBinBookingViewModel> GetExistingBinBookingDetails(string consumerNo);
        Task<bool> UpdateBinBookingDetails(dynamic udf);
        Task<EGovSewerageViewModel> GetExistingSewerageDetails(string consumerNo);
        Task<List<ServiceTemplateViewModel>> GetResidentialTaxService();
        Task<List<ServiceTemplateViewModel>> GetCommercialTaxService();
        Task<IList<TaskViewModel>> GetTaskList(string portalId);
        Task<List<ServiceTemplateViewModel>> GetTradeTaxService();
        Task<List<IdNameViewModel>> GetPropertyList(string wardId, string rentingType);
        Task<EGovRentalViewModel> GetAgreementDetails(string agreementNo);
        Task<bool> UpdateRenewalEndDate(dynamic udf);
        Task<bool> UpdateRentalStatus(TaskTemplateViewModel viewModel, dynamic udf);
        Task<CommandResult<OnlinePaymentViewModel>> UpdateOnlinePaymentDetails(OnlinePaymentViewModel model);
        Task UpdateOnlinePayment(OnlinePaymentViewModel responseViewModel);
        Task<OnlinePaymentViewModel> GetOnlinePayment(string id);
        Task<List<EGovProjectViewModel>> GetUpcomingProjectList(string categoryId, string wardId);
        Task<bool> DeleteUpcomingProject(string projectId);
        Task<List<EGovCommunityHallViewModel>> GetCommunityHallList();
        Task<List<NewDashboardViewModel>> GetDashboardList();
        Task<List<EGovDashboardViewModel>> GetProposalProjectsList(string type, string userId);
        Task<List<EGovDashboardViewModel>> GetAllProposalProjectsList();
        Task<EGovProjectProposalResponseViewModel> GetProposalLikesData(string proposalId, ProjectPropsalResponseEnum type, string userId);
        Task UpdateProjectProposalLikes(string proposalId, ProjectPropsalResponseEnum? type, string userId, DataActionEnum action);
        Task<List<EGovDashboardViewModel>> GetProposalProjectsCount(string type=null);
        Task<EGovDashboardViewModel> ViewProjectsUnderTaken(string serviceId);
        Task<FacilityViewModel> GetFacilityDetails(string facilityCode);
        Task<IList<FacilityViewModel>> GetFacilityList(string userId = null);
        Task<IList<FacilityLocationViewModel>> GetFacilityLocationList();
        Task<EGovDashboardViewModel> GetWardData(string UserId);
        Task<List<EGovDashboardViewModel>> GetNWTimeLineData();
        Task<FacilityViewModel> GetPreviousFacilityStatus(string facilityCode);
        Task<bool> ValidateNeedsWantsTimelineFromDateandToDate(FormTemplateViewModel viewModel);
        Task<IList<NtsTaskIndexPageViewModel>> GetNeedsAndWantsTaskCount(string categoryCodes, string portalId, bool showAllTaskForAdmin = true);
        Task<IList<EGovProjectProposal>> GetNeedsAndWantsTaskList(string categoryCodes, string status, string portalId, bool showAllTaskForAdmin);
        Task<IList<EGovProjectProposal>> ProjectsByLatLong();
        Task<bool> UpdateProjectProposalStatus(dynamic udf, string tsCode);
        Task<IList<ServiceTemplateViewModel>> GetDraftedServiceList(string statusCodes, string templateCode, string portalId);
        Task<JSCAssetConsumerViewModel> ReadJSCAssetConsumerData(string consumerId);
        Task<IList<JSCAssetConsumerViewModel>> GetJSCAssetsDataByConsumer(string userId);
        Task<List<IdNameViewModel>> GetWardListFromMaster();
        Task<List<IdNameViewModel>> GetJammuCollectorList();
        Task<List<CollectorWardAssignmentViewModel>> GetAssignedWardCollectorList();
        Task<CollectorWardAssignmentViewModel> GetWardCollectorById(string id);
        Task DeleteWardCollector(string id);
        Task<bool> ValidateAssetFeeTimelineFromDateandToDate(FormTemplateViewModel viewModel);
        Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentReport(string source, string assetType, string ward, DateTime? From, DateTime? To);
        Task<List<IdNameViewModel>> GeCollectorListForJammu();
        Task<List<IdNameViewModel>> GetAssetTypeListForJammu();
        Task<List<UserViewModel>> GetUnallocationUserList();
        Task<List<IdNameViewModel>> GetUnAllocatedAssetFilterByFromnToDate(string fromDate, string toDate);
        Task<List<IdNameViewModel>> GetUnAllocatedWard();
        Task<List<EGOVBannerViewModel>> GetVideoGallery(string videoTypeCode);
        Task<List<EGovDashboardViewModel>> GetJSCProposalProjectsList(string type, string userId);
        Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentReport2(string source, string assetType, string ward, DateTime? From, DateTime? To);
        Task<bool> GetEmailAndNameEnforcement(string Email,string Name,string Id);
        Task<bool> CreateUserEntry(EGOVUserDetailsViewModel model);

    }
}