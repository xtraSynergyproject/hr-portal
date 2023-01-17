using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IEGovernanceBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<EGovCommunityHallViewModel>> GetCommunityHallBookingList(string hallId);
        Task<IList<ServiceTemplateViewModel>> GetMyRequestList(bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string searchtext = null, DateTime? From = null, DateTime? To = null, string statusCodes = null, string templateIds = null);
        Task<IList<ServiceTemplateViewModel>> GetServiceList(string statusCodes, string templateCode);
        Task<List<MapMarkerViewModel>> GetMapMarkerDeatils();
        Task<List<EGOVBannerViewModel>> GetEGovSliderBannerData();
        Task<List<EGOVBannerViewModel>> GetEGovProjectMasterData();
        Task<List<EGOVBannerViewModel>> GetEGovLatestNewsData();
        Task<List<EGOVBannerViewModel>> GetEGovNotificationMasterData();
        Task<List<EGOVBannerViewModel>> GetEGovCorporatePhotoData();
        Task<List<EGOVBannerViewModel>> GetEGovTenderMasterData();
        Task<List<EGOVBannerViewModel>> GetEGovOtherWebsiteData();
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
        Task<List<EGovCommunityHallViewModel>> GetCommunityHallList();

    }
}
