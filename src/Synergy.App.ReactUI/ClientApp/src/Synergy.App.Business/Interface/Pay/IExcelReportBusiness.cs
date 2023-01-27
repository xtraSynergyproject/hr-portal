using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IExcelReportBusiness: IBusinessBase<NoteViewModel, NtsNote>
    {

        Task<MemoryStream> GetFlightAccrualDetails(List<PayrollReportViewModel> list);
        Task<MemoryStream> GetLoanAccrualDetails(List<PayrollReportViewModel> list);
        Task<MemoryStream> GetEndOfServiceAccrualDetails(List<PayrollReportViewModel> list);
        Task<MemoryStream> GetVacationAccuralDetails(List<PayrollReportViewModel> list);
    }
}
