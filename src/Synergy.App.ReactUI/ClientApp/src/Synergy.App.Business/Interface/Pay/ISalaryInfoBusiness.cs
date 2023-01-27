using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ISalaryInfoBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<IList<SalaryInfoViewModel>> GetSearchResult(SalaryInfoViewModel searchModel);
        Task<IList<SalaryInfoViewModel>> GetUnAssignedSalaryInfoList(string excludePersonId);
        Task<IList<VM>> ViewModelList<VM>(string cypherWhere = "", Dictionary<string, object> parameters = null, string returnValues = "");
        Task<string[]> GetDistinctElement(DateTime value);
        Task<string[]> GetDistinctElement();
        Task<IList<PayrollReportViewModel>> GetSalaryInfoDetails(PayrollReportViewModel searchModel);
        Task<IList<PayrollReportViewModel>> GetBankDetails(PayrollReportViewModel searchModel);
        Task<IList<PayrollReportViewModel>> GetAccuralDetails(PayrollReportViewModel searchModel);
        Task<string> GetSalaryInfoIdByPersonRootId(string personId);
        Task<IList<PayrollReportViewModel>> GetAccuralDetailsExcel(string personId, int? Year, MonthEnum? month = null);
        Task<IList<PayrollReportViewModel>> GetLoanAccuralDetails(PayrollReportViewModel searchModel);
        Task<IList<PayrollReportViewModel>> GetSalaryChangeInfoDetails(PayrollReportViewModel searchModel);
        Task<SalaryInfoViewModel> GetEligiblityForTickets(string UserId);
    }
}
