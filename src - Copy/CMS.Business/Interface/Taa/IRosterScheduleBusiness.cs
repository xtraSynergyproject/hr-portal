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
    public interface IRosterScheduleBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<IdNameViewModel>> GetDepartmentList();
        Task<List<IdNameViewModel>> GetShiftPatternList();
        Task<CommandResult<RosterScheduleViewModel>> CreateRosterSchedule(RosterScheduleViewModel viewModel);
        Task<RosterDutyTemplateViewModel> GetRosterDutyTemplateById(string Id);
        Task<List<RosterScheduleViewModel>> GetRosterSchedulerList(string orgId, DateTime? date = null);
        Task<CommandResult<RosterScheduleViewModel>> PublishRoster(string orgId, DateTime? date);
        Task<RosterScheduleViewModel> GetPublishedDate(string orgId, DateTime? date);
        Task<CommandResult<RosterScheduleViewModel>> CopyRoster(RosterScheduleViewModel viewModel);
        Task<bool> DeleteRoster(string users, string dates);
        Task<IList<RosterTimeLineViewModel>> GetRosterTimeList(string orgId, DateTime? date = null);
        Task<List<IdNameViewModel>> GetPersonListByOrganizationHerarchy(string orgId);
        Task<IList<DateTime>> GetDistinctNotcalculatedRosterDateList(DateTime rosterDate);
        Task<List<RosterScheduleViewModel>> GetPublishedRostersList(DateTime rosterDate);
        Task<IList<RosterScheduleViewModel>> GetPublishedRostersForAttendance(DateTime rosterDate);
        Task<CommandResult<RosterScheduleViewModel>> Correct(RosterScheduleViewModel viewModel);
    }
}
