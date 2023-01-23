using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;

namespace Synergy.App.Business
{
    public interface IBLSQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<IdNameViewModel>> getBLSLocationList(string userId = null);
        Task<List<IdNameViewModel>> GetVisaTypes();
        Task<BLSVisaAppointmentViewModel> GetAppointmentDetails(string serviceId, string serviceType);
        Task<List<BLSVisaAppointmentViewModel>> GetAppointmentDetailsByServiceId(string serviceId);
        Task<BLSVisaAppointmentViewModel> GetAppointmentDetailsByServiceNo(string serviceNo);
        Task<BLSVisaApplicationSettingsViewModel> GetSettingsData();
        Task<BLSVisaAppointmentViewModel> CheckEmailandServiceNo(string applicantEmail, string serviceNo);
        Task<BLSVisaAppointmentViewModel> GetDataById(string id);
        Task<BLSVisaAppointmentViewModel> GetSchengenVisaApplicationDetailsById(string id);
        Task<OnlinePaymentViewModel> GetOnlinePaymentDetails(OnlinePaymentViewModel model);
        Task UpdateOnlinePayment(OnlinePaymentViewModel responseViewModel);
        Task UpdateOnlinePaymentDetailsData(OnlinePaymentViewModel model, string date);
        Task UpdateApplicationStatus(string Id, string status);
        Task<OnlinePaymentViewModel> GetOnlinePayment(string id);
        Task<BLSVisaAppointmentViewModel> GetVisaAppointmentByParams(string applicantEmail, string applicationNo);
        Task<VisaTypeViewModel> GetVisaTypeDetails(string id);
        Task<List<IdNameViewModel>> GetAppointmentCategoryList(string userId = null, string Id = null);
        Task<BLSVisaAppointmentViewModel> GetVisaApplicationDetails(string serviceId);
        Task<BLSVisaAppointmentViewModel> GetAppointmentDetailsById(string Id);
        Task<List<BLSVisaAppointmentViewModel>> GetVisaApplicationDetailsByAppId(string appId);
        Task<List<ValueAddedServicesViewModel>> GetSelectedVAS(string appId);
        Task<List<BLSVisaAppointmentViewModel>> GetAppointmentSlotByDate(string date,string location);
        Task<List<BLSApplicantViewModel>> GetAppointmentSlotById(string appointmentId);
        Task<BLSTimeSlotViewModel> GetTimeSlotByLocation(DateTime date,string location, string category);
        Task<List<BLSTimeSlotViewModel>> GetTimeSlotList(string location);
        Task<List<Holiday>> GetAppointmentDate();
        Task<List<Holiday>> GetHolidays(string location);
        Task<List<ValueAddedServicesViewModel>> GetValueAddedServices();
        Task<BLSTimeSlotViewModel> GetTimeSlotById(string Id);
        Task<List<BLSTimeSlotViewModel>> GetAllTimeSlotList();
        Task<List<TimeSlot>> GetTimeSlotByParentId(string Id);
        Task<List<BLSApplicantViewModel>> GetApplicantsList(string parentId);
        Task<List<BLSVisaAppointmentViewModel>> GetMyAppointmentsList(string serviceId = null);
        Task<BLSApplicantViewModel> GetPassportDetail(string passportNo);
        Task<BLSAPiViewModel> IntegratePassportDetail(string countryId);
        Task<List<BLSApplicantViewModel>> GetAppointmentDetailsWithApplicants(string parentId = null);
        Task<BLSVisaApplicationViewModel> GetVisaApplicationDetailsByServiceNo(string serviceNo = null);
    }
}
