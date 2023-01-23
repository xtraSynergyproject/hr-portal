using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using CMS.UI.ViewModel.Pay;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IPayrollRunBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<IList<PayrollRunViewModel>> GetPayrollBatchList();
        Task<PayrollRunViewModel> GetSingleById(string payrollRunId);
        Task<List<MandatoryDeductionElementViewModel>> GetSingleElementById(string mandatoryDeductionId);
        Task<MandatoryDeductionElementViewModel> GetSingleElementEntryById(string id);

        Task CreateMandatoryDeductionElement(MandatoryDeductionElementViewModel model);

        Task EditMandatoryDeductionElement(MandatoryDeductionElementViewModel model);
        Task<List<IdNameViewModel>> GetPayRollElementName();
        Task<MandatoryDeductionSlabViewModel> GetSlabForMandatoryDeduction(string mandatoryDeductionId, double amount,DateTime asOfDate);
        Task DeleteMandatoryDeductionElement(string Id);
        Task<List<PayrollRunViewModel>> ViewModelList(string PayrollRunId);
        Task<List<PayrollSalaryElementViewModel>> GetEmployeeListForPayroll(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth);
        Task<List<PayrollElementRunResultViewModel>> GetStandardElementListForPayroll(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth, string personIds = null);
        Task<IList<PayrollSalaryElementViewModel>> PayrollRunPersonData(string payrollGroupId, string payrollId, string payrollRunId, DateTime asofDate, int yearMonth);
        Task<PayrollRunPersonViewModel> AddPersonToPayrollRun(string payrollRunId, string persons, string payRunNoteId);
        Task<IList<PayrollSalaryElementViewModel>> GetPayrollRunData(string payrollGroupId, string payrollId, string payrollRunId, DateTime? asofDate, int? yearMonth);
        Task<List<PayrollSalaryElementViewModel>> GetEmployeeListForPayrollRunData(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int? yearMonth);
        Task<CommandResult<PayrollRunViewModel>> Correct(PayrollRunViewModel viewModel);
        Task<PayrollRunViewModel> ExecutePayroll(string payrollRunId);
        Task<IList<PayrollRunViewModel>> GetPayrollRunList();
        Task<CommandResult<PayrollRunViewModel>> EditAccrual(PayrollRunViewModel model);
        Task<PayrollRunViewModel> GetPayrollRunById(string Id);
        Task<string> LoadVacationAccrualForCurrentMonth(PayrollRunViewModel viewModel);
        Task<List<PayrollElementRunResultViewModel>> GetStandardElementListForPayrollRunData(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth, string personIds = null);
        Task<List<MandatoryDeductionSlabViewModel>> GetSingleSlabById(string mandatoryDeductionId);
        Task<MandatoryDeductionSlabViewModel> GetSingleSlabEntryById(string id);
        Task<bool> ValidateMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model);
        Task CreateMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model);

        Task EditMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model);
        Task DeleteMandatoryDeductionSlab(string id);
    }
}
