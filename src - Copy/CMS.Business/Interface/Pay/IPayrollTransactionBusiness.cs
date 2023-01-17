using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IPayrollTransactionsBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<bool> ManagePayrollTransaction(PayrollTransactionViewModel model);
        Task<bool> DeletePayrollTransaction(string NoteId);
        Task<PayrollTransactionViewModel> GetPayrollTransactionDetails(string transactionId);

        Task<IdNameViewModel> GetPayrollElementByCode(string code);
        Task<List<PayrollTransactionViewModel>> GetAllUnProcessedTransactionsForPayrollRun(PayrollRunViewModel viewModel);
        Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionList(PayrollTransactionViewModel search);
        Task<List<PayrollTransactionViewModel>> BulkUpdateForPayroll(List<PayrollTransactionViewModel> viewModelList, string payrollId, string payrollRunId, bool doCommit = true);
        Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionBasedonElement(string personId, DateTime EffectiveDate, string ElementCode);
        Task<bool> IsTransactionExists(string personId, string elementCode, DateTime date, double amount);
        Task BulkUpdate(List<PayrollTransactionViewModel> viewModel, bool doCommit = true);
        Task GeneratePayrollSalaryTransactions(PayrollRunViewModel viewModel);
        Task<List<PayrollTransactionViewModel>> GetBasicTransportAndFoodTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> BulkInsert(List<PayrollTransactionViewModel> viewModelList, bool idGenerated = true, bool doCommit = true);
        Task<IList<PayrollTransactionViewModel>> IsPayrollTransactionBasedonElementExist(string personId, DateTime EffectiveDate, string ElementCode);
        Task<PayrollTransactionViewModel> GetPayrollTransationDataById(string id);
        Task<PayrollTransactionViewModel> GetPayrollTransationDataByReferenceId(string referenceId);
        Task<List<PayrollTransactionViewModel>> GetAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetEosAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetVacationAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetTicketEarningTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetOtandDedcutionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionListByDates(PayrollTransactionViewModel search);
        Task<PayrollTransactionViewModel> IsTransactionExists(string personId, string elementCode, DateTime date);
        Task<List<PayrollTransactionViewModel>> GetLoanAccrualTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetSickLeaveAccrualTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<bool> CloseTransaction(string Id, bool isClosed);
        Task DeleteIfAnyNotProcessedTrnasaction(ServiceTemplateViewModel viewModel);
    }
}
