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
    public interface ILeaveBalanceSheetBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<double> GetLeaveBalance(DateTime date, string leaveTypeCode, string userId);
        Task<double> GetEntitlement(string leaveTypeCode, string userId);
        Task ManageAnnualLeaveAccrual(DateTime startDate, DateTime endDate, DateTime accrualDate);
        Task<IList<LeaveViewModel>> GetAllAnnualLeaveTransactions(string userId);
        Task<LeaveViewModel> GetAnyExistingLeave(int year, DateTime startDate, DateTime endDate, string userId);
        Task<double> GetLeaveBalanceWithFutureEntitlement(DateTime date, string leaveTypeCode, string userId);
        Task<List<TeamLeaveRequestViewModel>> GetLeaveCalendar(string organizationId, DateTime? date = null);
        Task<CommandResult<LeaveBalanceSheetViewModel>> UpdateLeaveBalance(DateTime date, string leaveTypeCode, string userId, double? leaveEntitlement = null, DateTime? dateOfJoin = null);
        Task<bool> IsInProbation(string userId, DateTime dateOfJoin, DateTime asofDate, DateTime? probationEndDate);
        Task<double?> GetTotalHolidays(string userId, DateTime startDate, DateTime endDate, bool includePublicHolidays = true);
        Task<double> GetRemainingLeaveAccruals(string userId, DateTime asofDate);
        Task<List<LeaveDetailViewModel>> GetAllLeaveDuration(string userId, DateTime startDate, DateTime endDate);
        Task<bool> IsOnLeave(string userId, DateTime date);
        Task<bool> IsOnPaidLeave(string userId, DateTime date);
        Task DeleteAnnualLeaveAccrual(DateTime startDate, DateTime endDate);
        Task<List<LeaveDetailViewModel>> GetAllLeaves(DateTime startDate, DateTime endDate);
        Task<List<LeaveDetailViewModel>> GetAllUnpaidLeaveDurationIncludingPlannedUnpaidLeave(string userId, DateTime startDate, DateTime endDate);
        Task<double> GetLeaveAccrualPerMonth(string userId, DateTime startDate, DateTime endDate, double? leaveEntitlement = null);
        Task<CalendarViewModel> GetHolidaysAndWeekend(string userId, DateTime startDate, DateTime endDate, bool includePublicHolidays = true);
        Task<double> GetTicketAccrualPerMonth(string userId, DateTime startDate, DateTime endDate, double yearlyTicketCost);
        Task<double> GetTotalWorkingDays(string userId, DateTime startDate, DateTime endDate, bool publicHolidayAsWorkingDay = false);
        Task<double> GetLeaveAccruals(string userId, DateTime startDate, DateTime endDate);
        Task<double> GetLeaveEncashmentAmount(string userId, double encashleave);
        Task<List<LeaveDetailViewModel>> GetAllUnpaidLeaveDuration(string userId, DateTime startDate, DateTime endDate);
        Task<List<LeaveDetailViewModel>> GetAllLeavesWithDuration(DateTime startDate, DateTime endDate);
        Task<double> GetAnnualLeaveDatedDurationForAccrual(string userId, DateTime startDate, DateTime endDate);
        Task<LeaveDetailViewModel> GetTotalUnpaidLeaveDuration(string userId, DateTime startDate, DateTime endDate);
        Task<LeaveDetailViewModel> GetSickLeaveBalance(string userId, DateTime asofDate);
        Task<LeaveDetailViewModel> GetTotalSickLeaveDuration(string userId, DateTime startDate, DateTime endDate);
        Task<List<LeaveDetailViewModel>> GetAllSickLeaveDuration(string userId, DateTime startDate, DateTime endDate);
        Task<List<LeaveDetailViewModel>> GetAllLeaveEncashmentDuration(DateTime startDate, DateTime endDate);
        Task<double?> GetLeaveDuration(string userId, DateTime startDate, DateTime endDate, bool isHalfDay = false, bool includePublicHolidays = true);


        Task<TimeSpan?> getUnderTimeHours(string id);
        Task<bool> IsDayOff(string personId, DateTime dayToCheck);
        Task<double> GetActualWorkingdays(string calendarId, DateTime startDate, DateTime endDate);
        Task<List<CalendarHolidayViewModel>> GetHolidays(DateTime startDate, DateTime endDate);
        Task<IdNameViewModel> GetLeaveTypeByCode(string code);
    }
}
