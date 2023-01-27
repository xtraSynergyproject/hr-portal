using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ITaaQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<IdNameViewModel>> GetDepartmentList();
        Task<List<IdNameViewModel>> GetShiftPatternList();
        Task<RosterScheduleViewModel> GetExistingShiftPattern(string userId, string rostarDate);
        Task<RosterDutyTemplateViewModel> GetRosterDutyTemplateById(string Id);
        Task<List<RosterScheduleViewModel>> GetRosterSchedulerList(string orgId, DateTime sun, DateTime mon, DateTime tue, DateTime wed, DateTime thu, DateTime fri, DateTime sat,string LegalEntityCode);
        Task<List<RosterScheduleViewModel>> GetRosterScheduleByRosterDate(string orgId, DateTime start, DateTime end);

        Task<List<RosterTimeLineViewModel>> GetRosterTimeList(string orgId, DateTime sun, DateTime sat);
        Task<List<IdNameViewModel>> GetPersonListByOrganizationHerarchy(string orgId);
        Task<List<RosterScheduleViewModel>> GetDistinctNotcalculatedRosterDateList(DateTime rosterDate);
        Task<List<RosterScheduleViewModel>> GetPublishedRostersList(DateTime rosterDate);
        Task<IList<RosterScheduleViewModel>> GetPublishedRostersForAttendance(DateTime rosterDate);

        Task CloseOldRosters(DateTime date);
        Task<AttendanceViewModel> GetAttendanceSingleForPersonandDate(string personId, DateTime date);
        Task<List<IdNameViewModel>> GetDepartmentIdNameList();
        Task<List<AttendanceViewModel>> GetAttendanceList(string orgId, DateTime? date);
        Task<List<AccessLogViewModel>> GetAccessLogs(DateTime startDate, DateTime endDate, string users);
        Task<AttendanceViewModel> GetAttendanceDetailsById(string attendanceId);
        Task<List<AttendanceToPayrollViewModel>> GetUserDetailsList(string orgId);
        Task<List<AttendanceToPayrollViewModel>> GetAttendanceDetails(string orgId, DateTime first, DateTime last);
        Task<List<AttendanceToPayrollViewModel>> LeaveQuery(List<TemplateViewModel> templateList, string users);
        Task<AttendanceViewModel> GetAttendancebyIdandDate(DateTime payRollDate, string payRollRunId);
        Task UpdateAttendance(string id, string payrollPostedStatusId);
        Task<List<AttendanceViewModel>> GetPostAttendanceToPayrollDetails(string personIds, DateTime startDate, DateTime endDate);
        Task<bool?> UpdateAttendanceAfterApproval(string attendanceIds);
        Task<List<string>> CheckAnyPendingAttendanceApproval(string personIds, DateTime startDate, DateTime endDate, string users);
        Task<List<AttendanceViewModel>> GetAttendanceListByDate(DateTime attendanceDate);
        Task<List<AttendanceViewModel>> GetTotalOtAndDeduction(string userId, DateTime startDate, DateTime endDate);
        Task<IList<TimeinTimeoutDetailsViewModel>> GetTimeinTimeOutDetails(string Empid, DateTime? Datefrom, DateTime? DateTo, DateTime MonthSearch, string Type);
        Task<List<TimePermissionAttendanceViewModel>> GetReportTimePermissionList(List<TemplateViewModel> templateList);
        Task<List<EmployeeServiceViewModel>> GetReportBusinessTripList(List<TemplateViewModel> templateList);
        Task<List<SalaryInfoViewModel>> GetAllEmployeesForAttendance();
        Task<List<AccessLogViewModel>> GetAccessLogDetail(DateTime accessEnd, DateTime accessStart);
        Task<List<AttendanceViewModel>> GetAttendanceListByDate(List<string> orgId, List<string> personId, DateTime? fromdate, DateTime? todate, List<string> empStatus, string payrollRunId = null);

        Task<string> getUnderTimeHours(string id);
        Task<LeaveBalanceSheetViewModel> GetLeaveBalanceData(string userId, string leaveTypeCode, int year);
        Task<bool> IsOnPaidLeave(string userId, DateTime date);
        Task<IdNameViewModel> GetLeaveTypeByCode(string code);
        Task<IList<LeaveViewModel>> GetAllAnnualLeaveTransactions(string userId);
        Task<CalendarViewModel> GetCalendarDetails(string userId);
        Task<List<CalendarHolidayViewModel>> GetHolidayDetails(CalendarViewModel payCalendar);
        Task<List<LeaveDetailViewModel>> LeaveDetails(List<TemplateViewModel> templateList, string userId);
        Task<List<LeaveDetailViewModel>> GetAllLeaveEncashmentDuration(DateTime startDate, DateTime endDate);
        Task<List<LeaveDetailViewModel>> GetAllLeavesWithDuration(List<TemplateViewModel> templateList);
        Task<List<LeaveDetailViewModel>> GetAllSickLeaveDuration(string userId);
        Task<List<LeaveDetailViewModel>> GetAllUnpaidLeaveDuration(string userId, List<TemplateViewModel> templateList);
        Task<List<LeaveDetailViewModel>> GetAllUnpaidLeaveDurationIncludingPlannedUnpaidLeave(string userId);
        Task<List<LeaveViewModel>> GetAnnualLeaveDatedDurationForAccrual(string userId, List<TemplateViewModel> templateList);
        Task<double> GetEntitlement(string leaveTypeCode, string userId);
        Task<ContractViewModel> GetContractDetails(string userId);
        Task<double> GetLeaveBalance(string userId, int year, string leaveTypeCode);
        Task<AssignmentViewModel> GetAssignmentDetails(string userId);
        Task<AssignmentViewModel> GetCurrentAnniversaryStartDateByUserId(string userId);
        Task<ContractViewModel> GetContractByUser(string userId);
        Task<ContractViewModel> GetTicketDetails(string userId);
        Task<long> GetSickLeaveDetails(string userId);
        Task<double> GetUnpaidLeaveDetails(string userId);
        Task<ServiceViewModel> LeaveAccrualServiceExists(string userId, DateTime startDate, DateTime endDate, IServiceBusiness serviceBusiness = null);
        Task<List<LeaveDetailViewModel>> GetAllLeaves(List<TemplateViewModel> templateList);
        Task<PayrollRunViewModel> GetPayrollRunDetails(DateTime startDate, DateTime endDate);
        Task<bool> IsDayOff(string personId, DateTime todayDate, string IsDayQueryCheck);
        Task<CalendarViewModel> GetCalendarDetailsByCalendarId(string calendarId);
        Task<List<CalendarHolidayViewModel>> GetHolidays();
        Task<double> GetLeaveBalanceWithFutureEntitlementKSA(int year, string leaveTypeCode, string userId);
        Task<double> GetLeaveBalanceWithFutureEntitlementUAE(int year, string leaveTypeCode, string userId);
        Task<List<TeamLeaveRequestViewModel>> GetTeamLeaveDetails(string organizationId, DateTime? date);


        Task<LeaveTypeViewModel> GetLeaveTypeByCode(LeaveBalanceSheetViewModel viewModel);


    }
}
