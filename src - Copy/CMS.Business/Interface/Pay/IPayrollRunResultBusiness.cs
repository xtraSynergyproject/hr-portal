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
    public interface IPayrollRunResultBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<string[]> GetDistinctElement(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<List<PayrollDetailViewModel>> GetPayollDetailList(string payrollRunId, ElementCategoryEnum? elementCategory, int? yearMonth);
        Task<List<PayrollRunResultViewModel>> BulkInsertForPayroll(PayrollRunViewModel viewModel, bool idGenerated = true, bool doCommit = true);
        Task<List<PayrollRunResultViewModel>> GetSuccessfulPayrollRunResult(PayrollRunViewModel viewModel);
        //Task<string[]> GetDistinctElement(string payrollRunId, ElementCategoryEnum? elementCategory);
        
        Task<string[]> GetDistinctElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory, ElementClassificationEnum? elementType);
        Task<string[]> GetDistinctElementEarning(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<string[]> GetDistinctElementDeduction(string payrollRunId, ElementCategoryEnum? elementCategory);
     

        Task<List<PayrollElementRunResultViewModel>> BulkInsertIntoElementRunResult(PayrollRunViewModel viewModel,List<PayrollElementRunResultViewModel> viewModelList, string payrollRunResultId, bool idGenerated = true, bool doCommit = true);
        Task<List<PayrollElementRunResultViewModel>> GetSuccessfulElementRunResult(PayrollRunViewModel viewModel);
        Task<List<PayrollElementDailyRunResultViewModel>> BulkInsertIntoElementDailyRunResult(List<PayrollElementDailyRunResultViewModel> viewModelList,string elementRunResultId, bool idGenerated = true, bool doCommit = true);
        Task<IList<PayrollSalaryElementViewModel>> GetPaySalaryElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<string[]> GetDistinctElementDisplayName(string payrollRunId, ElementCategoryEnum? elementCategory);
    }
}
