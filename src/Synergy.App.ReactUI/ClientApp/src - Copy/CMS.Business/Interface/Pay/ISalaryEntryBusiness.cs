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
    public interface ISalaryEntryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<SalaryEntryViewModel>> GetSalaryDetails();
        Task<List<SalaryEntryViewModel>> GetSalaryElementDetails(string id);
        Task<List<SalaryEntryViewModel>> BulkInsertForPayroll(List<SalaryEntryViewModel> viewModelList, bool idGenerated = true, bool doCommit = true);
        Task<List<SalaryEntryViewModel>> GetSuccessfulSalaryEntryList(PayrollRunViewModel viewModel);
        Task<List<PayrollSalaryElementViewModel>> GetPaySalaryElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<List<SalaryElementEntryViewModel>> GetEarningElementForPayslipPdf(string id);
        Task<List<SalaryElementEntryViewModel>> GetDeductionElementForPayslipPdf(string id);
        Task<SalaryEntryViewModel> GetPaySlipHeaderDetails(string id);
        Task<List<SalaryEntryViewModel>> GetPaySalaryDetails(SalaryEntryViewModel search);
        Task<string[]> GetDistinctElement(string payrollRunId);
        Task<List<PayrollSalaryElementViewModel>> GetPaySalarySummaryDetails(int YearMonth);

        Task<List<SalaryElementEntryViewModel>> BulkInsertIntoSalaryElementEntry(List<SalaryElementEntryViewModel> viewModelList, bool idGenerated = true, bool doCommit = true);
        Task<List<SalaryElementEntryViewModel>> GetSalaryElementEntries(string salaryEntryId);
    }
}
