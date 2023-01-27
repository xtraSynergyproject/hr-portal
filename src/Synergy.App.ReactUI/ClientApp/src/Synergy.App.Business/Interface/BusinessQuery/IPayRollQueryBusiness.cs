using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IPayRollQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<IdNameViewModel>> GetPayGroupList();
        Task<List<IdNameViewModel>> GetPayCalenderList();
        Task<List<IdNameViewModel>> GetPayBankBranchList();
        Task<List<IdNameViewModel>> GetSalaryElementIdName();
        Task<List<SalaryInfoViewModel>> GetSalaryInfoDetails(string salaryInfoId);
        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoDetails(string elementId, string salaryInfoId, string salaryElementId = null);
        Task DeleteSalaryElement(string NoteId);
        Task<List<IdNameViewModel>> GetPayrollGroupList();
        Task<PayrollGroupViewModel> GetPayrollGroupById(string payGroupId);
        Task<List<PayrollBatchViewModel>> ViewModelList(string PayrollBatchId);
        Task<PayrollBatchViewModel> GetSingleById(string payrollBatchId);
        Task<PayrollBatchViewModel> IsPayrollExist(string payGroupId, string yearmonth);


        Task<CalendarViewModel> GetCalendarDetailsById(string id);
        Task<List<CalendarViewModel>> GetCalendarListData();
        Task<List<CalendarHolidayViewModel>> GetCalendarHolidayData(string calendarId);
        Task<List<CalendarHolidayViewModel>> GetCalendarHolidayDatawithmonthYear(string calendarId, int Year, int month);
        Task<CalendarHolidayViewModel> GetCalendarHolidayDetailsById(string calHolidayId);
        Task DeleteCalendarHoliday(string NoteId);
        Task<List<CalendarHolidayViewModel>> CheckHolidayNameWithCalendar(string calId, string holName);
        Task<List<CalendarViewModel>> CheckCalendarWithLegalEntityId(string legalEId, string name, string code);


        Task<LegalEntityViewModel> GetPersonDetails(string personId);
        Task DeleteSalaryInfo(string id);
        Task<List<SalaryElementInfoViewModel>> GetAllSalaryElementInfo();
        Task<List<IdNameViewModel>> GetAllUserSalary();
        Task<double> GetBasicSalary(string userId, DateTime? asofDate = null);
        Task<SalaryElementInfoViewModel> GetUserSalary(string userId, DateTime? asofDate = null);
        Task<List<ElementViewModel>> GetElementListForPayrollRun(DateTime asofDate);
        Task<ElementViewModel> GetPayrollElementById(string Id);
        Task<List<IdNameViewModel>> GetPayrollDeductionElement();
        Task<SalaryInfoViewModel> GetEligiblityForEOS(string userId);
        Task<SalaryInfoViewModel> GetEligiblityForTickets(string userId);
        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoForPayrollRun(PayrollRunViewModel viewModel);
        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByNodeId(string nodeId);
        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByUser(string userId, DateTime asofDate);
        Task<SalaryElementInfoViewModel> GetSalaryElementInfoListByUserAndELement(string personId, string elementId);



        Task<PayrollRunViewModel> GetPayrollSingleDataById(string payrollRunId);
        Task<List<MandatoryDeductionElementViewModel>> GetSingleElementById(string mandatoryDeductionId);
        Task<List<IdNameViewModel>> GetPayRollElementName();
        Task<MandatoryDeductionSlabViewModel> GetSlabForMandatoryDeduction(string mandatoryDeductionId, double amount, DateTime asofDate);
        Task<MandatoryDeductionElementViewModel> GetSingleElementEntryById(string id);
        Task DeleteMandatoryDeductionElement(string Id);
        Task UpdatePayrollById(string payrollRunId, DateTime payrollRunDate, int exeStatus);
        Task<PayrollRunViewModel> GetNextPayroll(string payrollRunId);
        Task SetPayrollExeStatusnState(int stateEnd, int exeStatus, PayrollRunViewModel viewModel);
        Task SetBatchStatus(int payrollstatus, PayrollBatchViewModel payroll);
        Task SetPayrollRunStatus(PayrollRunViewModel viewModel);
        Task<List<PayrollElementRunResultViewModel>> GetStandardElementListForPayrollRunData(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth, string personIds = null);
        Task<IList<PayrollRunViewModel>> GetPayrollBatchList();
        Task<List<PayrollRunViewModel>> PayrollRunViewModelList(string PayrollRunId);
        Task<IList<PayrollRunViewModel>> GetPayrollRunList();
        Task<List<PayrollSalaryElementViewModel>> GetEmployeeListForPayrollRunData(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int? yearMonth);
        Task<List<PayrollSalaryElementViewModel>> GetEmployeeListForPayroll(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth);
        Task<List<PayrollElementRunResultViewModel>> GetStandardElementListForPayroll(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth, string personIds = null);
        Task SetPayrollRunStatusnError(PayrollExecutionStatusEnum exceutionStatus, string error, string payrollRunId);
        Task<List<ElementViewModel>> GetElementsForPayrollRun(DateTime payrollEndDate);
        Task<List<PayrollPersonViewModel>> GetSelectedPersonsForPayrollRun(PayrollRunViewModel viewModel);
        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoForPayrollRun2(PayrollRunViewModel viewModel);
        Task DeleteMandatoryDeductionSlab(string Id);
        Task<List<MandatoryDeductionSlabViewModel>> ValidateMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model);
        Task<MandatoryDeductionSlabViewModel> GetSingleSlabEntryById(string id);
        Task<List<MandatoryDeductionSlabViewModel>> GetSingleSlabById(string mandatoryDeductionId);
        Task<PayrollTransactionViewModel> GetElementDetails(string elementCode);
        Task<PayrollTransactionViewModel> ManageClosedTransaction(string personId, string elementCode, DateTime payrollEndDate);
        Task<List<TicketAccrualViewModel>> GetSickLeaveAccrualPersonList(DateTime payrollDate, string payRollRunId);
        Task<List<PayrollTransactionViewModel>> GetPendingLoanAccrualDetails(DateTime payrollStartDate, DateTime payrollEndDate, string payRollRunId, List<string> exculdePersons, string excludePersonText, DateTime LOANDATE);
        Task<List<PayrollTransactionViewModel>> GetLoanAccrualDetails(DateTime payrollStartDate, DateTime payrollEndDate, string payRollRunId);
        Task<CalendarViewModel> GetCalendarDetails(string calendarId);
        Task<List<CalendarHolidayViewModel>> GetHolidayDetails(CalendarViewModel calendarVM);
        Task<TicketAccrualViewModel> GetEndOfService(string userId);
        Task SetPayrollStateEnd(PayrollRunViewModel payroll);
        Task<PayrollRunViewModel> GetPayrollRunDataByServiceId(string serviceId);
        Task<List<TicketAccrualViewModel>> GetTicketEligibleEmployeeDetails(DateTime payrollDate, string payRollRunId);
        Task<ServiceViewModel> GetPayrollRunService(string payrollRunId);
        Task<PayrollRunViewModel> GetPayrollRunById(string Id);
        Task<PayrollRunViewModel> GetPayrollRunByNoteId(string noteId);
        Task<List<string>> GetDistinctRun(int yearMonth);
        Task<List<PayrollSummaryViewModel>> GetPayrollSummary1();
        Task<List<PayrollSummaryViewModel>> GetPayrollSummary2();
        Task<List<PayrollBatchViewModel>> GetPostedPayrollEmployeeList(string payrollGroupId, string payrollRunId, string payrollId);
        Task<List<PayrollBatchViewModel>> GetNotPostedPayrollEmployeeList(string payrollGroupId, string payrollRunId, string payrollId);
        Task<List<PayrollBatchViewModel>> GetNotPostedPreviousPayrollEmployeeList(string payrollGroupId);
        Task<double?> GetElementSalaryByPerson(DateTime attendanceDate, string PersonId, string SalaryCode);
        Task<PayrollRunViewModel> GetNextPayroll(int submi, int inpro, int erro);
        Task<List<UserListOfValue>> GetExcludePersonList(string payrollGroupId, string payrollRunId, string searchParam, string payrollId, string orgId);
        Task<List<UserListOfValue>> GetIncludePersonList(string payrollId);
        Task<List<TicketAccrualViewModel>> GetTicketAccrualDetails(DateTime payrollDate, string payRollRunId);
        Task<List<TicketAccrualViewModel>> GetEOSAccrualDetails(DateTime payrollDate, string payRollRunId, DateTime attendanceStartDate, DateTime attendanceEndDate);
        Task<List<TicketAccrualViewModel>> GetVacationAccrualDetails(DateTime payrollDate, string payRollRunId);
        Task UpdatePayrollTransaction(LOVViewModel lov, LOVViewModel dlov, PayrollRunViewModel viewModel, /*PayrollRunViewModel payrollRun*/ string personIds);
        Task<List<PayrollRunViewModel>> GetPayrollSingleData(PayrollRunViewModel viewModel);
        Task<List<double>> GetSickLeavesList(LeaveDetailViewModel model, DateTime date, DateTime lastAnniversaryDate);
        Task UpdateSalaryEntry(int publish, PayrollRunViewModel viewModel);
        Task UpdateSalaryElementEntry(int publish, PayrollRunViewModel viewModel);
        Task UpdatePayrollTransactionByPayrollIdnPersonId(LOVViewModel lov, PayrollRunViewModel viewModel, PayrollRunViewModel payrollRun);
        Task UpdatePayrollBatch(PayrollRunViewModel viewModel, int payrollStatus);
        Task UpdateIsDeleteofPayrollTransaction(PayrollRunViewModel viewModel);
        Task UpdateIdsinPayrollTransaction(LOVViewModel lov, PayrollRunViewModel viewModel);
        Task DeleteFromSalaryElementEntry(PayrollRunViewModel viewModel);
        Task DeleteFromSalaryEntry(PayrollRunViewModel viewModel);
        Task DeleteFromBankLetterDetail(PayrollRunViewModel viewModel);
        Task DeleteFromBankLetter(PayrollRunViewModel viewModel);
        Task DeleteFromPayrollElementDailyRunResult(PayrollRunViewModel viewModel);
        Task DeleteFromPayrollElementRunResult(PayrollRunViewModel viewModel);
        Task DeleteFromPayrollRunResult(PayrollRunViewModel viewModel);




        Task<IList<VM>> ViewModelList<VM>(string cypherWhere = "", Dictionary<string, object> parameters = null, string returnValues = "");
        Task<List<SalaryInfoViewModel>> GetUnAssignedSalaryInfoList(string excludePersonId, string query);
        Task<string> GetSalaryInfoIdByPersonRootId(string personId);
        Task<List<PayrollReportViewModel>> GetSalData(PayrollReportViewModel searchModel);
        Task<List<PayrollReportViewModel>> GetPayrollReport(PayrollReportViewModel searchModel);
        Task<List<PayrollReportViewModel>> GetSalDataForDates(PayrollReportViewModel searchModel, PayrollReportViewModel p);
        Task<IList<PayrollReportViewModel>> GetAccuralDetailsExcel(string personId, int? Year, MonthEnum? month = null);
        Task<IList<PayrollReportViewModel>> GetBankDetails(PayrollReportViewModel searchModel);
        Task<IList<PayrollReportViewModel>> GetLoanAccuralDetails(PayrollReportViewModel searchModel);
        Task<List<ElementViewModel>> GetDistinctElement(DateTime value);
        Task<List<ElementViewModel>> GetDistinctElement();
        Task<IList<PayrollReportViewModel>> GetAccuralDetails(PayrollReportViewModel searchModel);



        Task<List<SalaryElementEntryViewModel>> GetElementsForPayslipPdf(string id);
        Task<List<SalaryElementEntryViewModel>> GetSalDistinctElement(string payrollRunId);
        Task<List<SalaryEntryViewModel>> GetSuccessfulSalaryEntryList(PayrollRunViewModel viewModel);
        Task<List<SalaryEntryViewModel>> GetSalaryElementDetails(string id);
        Task<List<SalaryEntryViewModel>> GetSalaryDetails(int publishStatus);
        Task<SalaryEntryViewModel> GetPaySlipHeaderDetails(string id);
        Task<List<PayrollSalaryElementViewModel>> GetPaySalarySummaryDetails(int YearMonth);
        Task<List<SalaryEntryViewModel>> GetPaySalaryDetails(SalaryEntryViewModel search, string legalid);
        Task<List<PayrollSalaryElementViewModel>> GetPaySalaryElementDetailsQ1(string payrollRunId);
        Task<List<SalaryElementEntryViewModel>> GetPaySalaryElementDetailsQ2(string payrollRunId, ElementCategoryEnum? elementCategory);



        Task DeletePayrollTransaction(string NoteId);
        Task<PayrollTransactionViewModel> GetPayrollTransactionDetails(string transactionId);
        Task<IdNameViewModel> GetPayrollElementByCode(string code);
        Task<List<PayrollTransactionViewModel>> GetAllUnProcessedTransactionsForPayrollRun(PayrollRunViewModel viewModel);
        Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionList(PayrollTransactionViewModel search);
        Task UpdatePayrollTxn(PayrollRunViewModel viewModel, string payTransIds, DateTime lastUpdateDate);
        Task UpdatePayrollTxnforIds(DateTime lastUpdateDate, string notProcessedIds);
        Task UpdatePayrollTxnDetailsforIds(string processedIds, string payrollId, string payrollRunId, LOVViewModel processStatus, DateTime lastUpdateDate);
        Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionBasedonElement(string personId, DateTime EffectiveDate, string ElementCode);
        Task<List<PayrollTransactionViewModel>> IsTransactionExists(string personId, string elementCode, DateTime date, double amount);
        Task<MandatoryDeductionViewModel> GetExemption(MandatoryDeductionViewModel model, string financialYearId, string personId, DateTime asofDate);
        Task<List<MandatoryDeductionViewModel>> GetFinancialYear(DateTime asOfDate);
        Task<double> GetNoOfMonthsForEmploeePayroll(DateTime? dateofJoin,DateTime? lastWorkingDate, string financialYearId);
        Task<List<MandatoryDeductionViewModel>> GetMandatoryDeductionOfFinancialYear(DateTime asOfDate);
        Task<List<PayrollTransactionViewModel>> GetSalaryTransactionList(DateTime startDate, DateTime endDate, string payrollRunId = null);
        Task<List<PayrollTransactionViewModel>> GetBasicTransportAndFoodTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<ElementViewModel>> GetPayrollElement(string elementId);
        Task<bool?> UpdatePayrollTxnByNoteId(PayrollTransactionViewModel trans);
        Task<List<PayrollTransactionViewModel>> GetPayrollTxnDatabyDate(ServiceTemplateViewModel viewModel, DateTime? anniversaryStartDate);
        Task<bool> CloseTransaction(string Id, bool isClosed);
        Task<List<PayrollTransactionViewModel>> GetSickLeaveAccrualTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetLoanAccrualTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<PayrollTransactionViewModel> IsTransactionExists(string personId, string elementCode, DateTime date);
        Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionListByDates(PayrollTransactionViewModel search, string personId);
        Task<List<PayrollTransactionViewModel>> GetOtandDedcutionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetTicketEarningTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetVacationAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetEosAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<List<PayrollTransactionViewModel>> GetAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate);
        Task<PayrollTransactionViewModel> GetPayrollTransationDataByReferenceId(string referenceId);
        Task<PayrollTransactionViewModel> GetPayrollTransationDataById(string id);
        Task<IList<PayrollTransactionViewModel>> IsPayrollTransactionBasedonElementExist(string personId, DateTime EffectiveDate, string ElementCode);




        Task<List<PayrollElementRunResultViewModel>> GetSuccessfulElementRunResult(PayrollRunViewModel viewModel, int executionstatus);
        Task<List<ElementViewModel>> GetDistinctElementDeduction(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<List<ElementViewModel>> GetDistinctElementEarning(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<List<string>> GetDistinctElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory, ElementClassificationEnum? elementType);
        Task<List<PayrollRunResultViewModel>> GetSuccessfulPayrollRunResult(PayrollRunViewModel viewModel, int executionStatus);
        Task<List<ElementViewModel>> GetDistinctElement(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<List<PayrollSalaryElementViewModel>> GetPayrollSalaryElements(string payrollRunId);
        Task<List<SalaryElementEntryViewModel>> GetPersonSalEntryDetails(string pers, PayrollBatchViewModel payrollBatch);
        Task<List<PayrollElementRunResultViewModel>> GetPayrollRunData(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<List<PayrollSalaryElementViewModel>> GetDistinctElementDisplayName(string payrollRunId, ElementCategoryEnum? elementCategory);
        Task<List<PayrollDetailViewModel>> GetpayRollDetails(string payrollRunId, int? yearMonth, ElementCategoryEnum? elementCategory);
        Task<List<PayrollElementDailyRunResultViewModel>> GetPayrollDailyResult(string payrollRunId, int? yearMonth);



















    }
}
