using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IPayrollBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<CalendarViewModel> GetCalendarDetailsById(string id);
        Task<List<CalendarHolidayViewModel>> GetCalendarHolidayData(string calendarId);
        Task<CalendarHolidayViewModel> GetCalendarHolidayDetailsById(string calHolidayId);
        Task<bool> DeleteCalendarHoliday(string NoteId);
        Task<List<CalendarViewModel>> GetCalendarListData();
        Task<bool> ValidateHolidayName(NoteTemplateViewModel viewModel);
        Task<List<CalendarHolidayViewModel>> CheckHolidayNameWithCalendar(string calId, string holName);
        Task<bool> ValidateCalendar(NoteTemplateViewModel viewModel);
        Task<List<CalendarViewModel>> CheckCalendarWithLegalEntityId(string legalEId, string name, string code);
        Task<List<CalendarHolidayViewModel>> GetCalendarHolidayDatawithmonthYear(string calendarId, int Year, int month);
    }
}
