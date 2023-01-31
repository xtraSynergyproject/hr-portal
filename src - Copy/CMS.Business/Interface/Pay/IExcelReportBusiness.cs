using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IExcelReportBusiness: IBusinessBase<NoteViewModel, NtsNote>
    {

        Task<MemoryStream> GetFlightAccrualDetails(List<PayrollReportViewModel> list);
        Task<MemoryStream> GetLoanAccrualDetails(List<PayrollReportViewModel> list);
        Task<MemoryStream> GetEndOfServiceAccrualDetails(List<PayrollReportViewModel> list);
        Task<MemoryStream> GetVacationAccuralDetails(List<PayrollReportViewModel> list);
    }
}
