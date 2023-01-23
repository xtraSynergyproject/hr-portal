using Synergy.App.Common;
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
    public interface ICommonServiceQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {      

        Task<OnlinePaymentViewModel> UpdateOnlinePaymentDetails(OnlinePaymentViewModel model);
        Task UpdateOnlinePaymentDetailsData(OnlinePaymentViewModel model,string date);
        Task UpdateOnlinePayment(OnlinePaymentViewModel responseViewModel);
        Task<OnlinePaymentViewModel> GetOnlinePayment(string id);
        Task<CSCReportViewModel> GetCSCBirthCertificateData(string serviceId);
        Task<CSCReportMarriageCertificateViewModel> GetCSCMarriageCertificateData(string serviceId);
        Task<CSCReportOBCCertificateViewModel> GetCSCOBCCertificateData(string serviceId);
        Task<CSCReportAcknowledgementViewModel> GetCSCAcknowledgementData(string serviceId);
        Task<List<ServiceChargeViewModel>> GetServiceChargeData(string serviceId);
        Task<List<CSCTrackApplicationViewModel>> GetTrackApplicationList(string applicationNo);
        Task<IList<TaskViewModel>> GetCSCTaskList(string portalId);
        Task<IList<ServiceChargeViewModel>> GetServiceChargeDetails(string serviceId);
        Task<long> GetDocumentsCount(string udfnotetableId);

    }
}
