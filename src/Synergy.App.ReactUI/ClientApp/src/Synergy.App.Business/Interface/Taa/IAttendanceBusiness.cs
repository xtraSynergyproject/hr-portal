using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IAttendanceBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<CommandResult<AttendanceViewModel>> CorrectAttendance(AttendanceViewModel viewModel);
        Task<CommandResult<AttendanceViewModel>> CreateAttendance(AttendanceViewModel viewModel);
        Task<List<IdNameViewModel>> GetDepartmentIdNameList();
        Task<CommandResult<AttendanceViewModel>> Delete(AttendanceViewModel viewModel, bool doCommit = true);
        Task<List<AttendanceViewModel>> GetAttendanceList(string orgId, DateTime? date);
        //Task<List<EmployeeAttendanceViewModel>> GetEmployeeAtendanceList(EmployeeAttendanceViewModel model);
        Task<AttendanceViewModel> GetAttendanceDetailsById(string attendanceId);
        Task<CommandResult<AttendanceViewModel>> UpdateComment(AttendanceViewModel viewModel, bool doCommit = true);
        Task<CommandResult<AttendanceViewModel>> UpdateApproved(AttendanceViewModel viewModel, bool doCommit = true);
        Task<CommandResult<AttendanceViewModel>> CreateAttendanceFromBiometrics(AttendanceViewModel viewModel, bool doCommit = true);
        Task<CommandResult<AttendanceViewModel>> CorrectAttendanceFromBiometrics(AttendanceViewModel viewModel, bool doCommit = true);
        //Task<List<OverrideAttendanceServiceViewModel>> GetOverrideApprovalList(string orgId, DateTime? start, DateTime? end);
        //Task ApproveRejectTask(OverrideAttendanceServiceViewModel model);
        Task<List<AttendanceViewModel>> GetOTPayTransactionList(DateTime attendanceStartDate, DateTime attendanceEndDate, string payRollRunId);
        Task<bool> IsPersonAbsentForMonth(string personId, DateTime payroll);
        Task UpdateOTPayTransToProcessed(DateTime payRollDate, string payRollRunId);
        Task<long> NoOfDaysAbsentForPayMonth(string personId, DateTime payroll);
        Task<CommandResult<AttendanceViewModel>> CreateAttendanceFromService(AttendanceViewModel viewModel, bool doCommit = true);
        Task<AttendanceViewModel> GetAttendanceSingleForPersonandDate(string personId, DateTime date);
        Task<string> PostAttendanceToPayroll(string personIds, DateTime startDate, DateTime endDate);
        Task<List<AttendanceToPayrollViewModel>> GetAtendanceListForPostPayroll(string payrollGroupId, int startMonth, int year, string orgId);
        Task UpdateAttendanceTable(DateTime? date);
        Task<CommandResult<AttendanceViewModel>> CreateOverrideAttendance(AttendanceViewModel viewModel, bool doCommit = true);
        Task<CommandResult<AttendanceViewModel>> CorrectOverrideAttendance(AttendanceViewModel viewModel, bool doCommit = true);
        Task<List<AttendanceViewModel>> GetAttendanceListByDate(DateTime attendanceDate);
        Task<AttendanceViewModel> GetTotalOtAndDeduction(string userId, DateTime startDate, DateTime endDate);
        Task<TimeSpan?> getUnderTimeHours(string id);
        Task UpdateAttendanceTable(DateTime date, string userId);
        Task<CommandResult<AttendanceViewModel>> ManageAttendance(AttendanceViewModel viewModel, bool doCommit = true);
        Task<CommandResult<AttendanceViewModel>> ManageAttendanceDelete(AttendanceViewModel viewModel, bool doCommit = true);

         Task<IList<TimeinTimeoutDetailsViewModel>> GetTimeinTimeOutDetails(string Empid, DateTime? Datefrom, DateTime? DateTo, DateTime MonthSearch, string Type);

       Task<IList<TimePermissionAttendanceViewModel>> GetReportTimePermissionList();

       Task<IList<EmployeeServiceViewModel>> GetReportBusinessTripList();

        Task<List<AttendanceViewModel>> GetAttendanceListByDate(List<string> orgId, List<string> personId, DateTime? fromdate, DateTime? todate, List<string> empStatus, string payrollRunId = null);
    }
}
