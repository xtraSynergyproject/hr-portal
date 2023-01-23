using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBLSBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<IdNameViewModel>> getBLSLocationList(string userId = null);
        Task<List<IdNameViewModel>> GetVisaTypes();
        Task<BLSVisaAppointmentViewModel> GetAppointmentDetails(string serviceId, string serviceType);
        Task<BLSVisaAppointmentViewModel> GetVisaApplicationDetails(string serviceId);
        Task<List<BLSTimeSlotViewModel>> GetTimeSlotList(string location);
        Task<List<Holiday>> GetHolidays(string location);
        Task<List<Holiday>> GetAppointmentDate();
        Task<List<BLSApplicantViewModel>> GetAppointmentSlotById(string appointmentId);
        Task<List<BLSVisaAppointmentViewModel>> GetAppointmentDetailsByServiceId(string serviceId);
        Task<BLSVisaAppointmentViewModel> GetAppointmentDetailsByServiceNo(string serviceNo);
        Task<BLSApplicantViewModel> GetPassportDetail(string passportNo);
        Task<BLSAPiViewModel> IntegratePassportDetail(string country);
        Task<List<IdNameViewModel>> GetSlotValues(DateTime date, string loc, string serviceType, string visaType, string category);
        Task<BLSVisaApplicationSettingsViewModel> GetSettingsData();
        Task<BLSVisaAppointmentViewModel> CheckEmailandServiceNo(string applicantEmail, string serviceNo);
        Task<BLSVisaAppointmentViewModel> GetDataById(string id);
        Task<BLSVisaAppointmentViewModel> GetSchengenVisaApplicationDetailsById(string id);
        Task<CommandResult<OnlinePaymentViewModel>> UpdateOnlinePaymentDetails(OnlinePaymentViewModel model);
        Task UpdateOnlinePayment(OnlinePaymentViewModel responseViewModel);
        Task UpdateApplicationStatus(string Id, string status);

        Task<OnlinePaymentViewModel> GetOnlinePayment(string id);
        Task<BLSVisaAppointmentViewModel> GetVisaAppointmentByParams(string applicantEmail, string applicationNo);
        Task<VisaTypeViewModel> GetVisaTypeDetails(string id);
        Task<List<IdNameViewModel>> GetAppointmentCategoryList(string userId = null, string Id = null);
        Task<BLSVisaAppointmentViewModel> GetAppointmentDetailsById(string id);
        Task<List<BLSVisaAppointmentViewModel>> GetVisaApplicationDetailsByAppId(string appId);
        Task<List<ValueAddedServicesViewModel>> GetSelectedVAS(string appId);
        Task<List<ValueAddedServicesViewModel>> GetValueAddedServices();
        Task<BLSTimeSlotViewModel> GetTimeSlotById(string Id);
        Task<List<BLSTimeSlotViewModel>> GetAllTimeSlotList();
        Task<List<TimeSlot>> GetTimeSlotByParentId(string Id);
        Task<List<BLSApplicantViewModel>> GetApplicantsList(string parentId);
        Task<List<BLSVisaAppointmentViewModel>> GetMyAppointmentsList(string serviceId=null);
        Task<List<BLSApplicantViewModel>> GetAppointmentDetailsWithApplicants(string parentId = null);
        Task<BLSVisaApplicationViewModel> GetVisaApplicationDetailsByServiceNo(string serviceNo = null);

    }
}
