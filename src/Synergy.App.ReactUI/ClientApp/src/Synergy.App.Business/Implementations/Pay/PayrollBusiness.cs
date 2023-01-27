using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class PayrollBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollBusiness
    {
        
        private readonly IRepositoryQueryBase<CalendarViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<CalendarHolidayViewModel> _queryRepo;
        private readonly IPayRollQueryBusiness _payRollQueryBusiness;


        public PayrollBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper            
            , IRepositoryQueryBase<CalendarViewModel> queryRepo1
            , IPayRollQueryBusiness payRollQueryBusiness
            , IRepositoryQueryBase<CalendarHolidayViewModel> queryRepo) : base(repo, autoMapper)
        {            
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            _payRollQueryBusiness = payRollQueryBusiness;
        }

        public async override Task<CommandResult<NoteViewModel>> Create(NoteViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<NoteViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteViewModel>> Edit(NoteViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<NoteViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<CalendarViewModel> GetCalendarDetailsById(string id)
        {
            var queryData = await _payRollQueryBusiness.GetCalendarDetailsById(id);
            return queryData;
        }

        public async Task<List<CalendarViewModel>> GetCalendarListData()
        {
            var queryData = await _payRollQueryBusiness.GetCalendarListData();
            return queryData;
        }

        public async Task<List<CalendarHolidayViewModel>> GetCalendarHolidayData(string calendarId)
        {
            var queryData = await _payRollQueryBusiness.GetCalendarHolidayData(calendarId);
            return queryData;
        }


        public async Task<List<CalendarHolidayViewModel>> GetCalendarHolidayDatawithmonthYear(string calendarId,int Year,int month)
        {
            var queryData = await _payRollQueryBusiness.GetCalendarHolidayDatawithmonthYear(calendarId, Year, month);
            return queryData;
        }

        public async Task<CalendarHolidayViewModel> GetCalendarHolidayDetailsById(string calHolidayId)
        {
            var queryData = await _payRollQueryBusiness.GetCalendarHolidayDetailsById(calHolidayId);
            return queryData;
        }

        public async Task<bool> DeleteCalendarHoliday(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                await _payRollQueryBusiness.DeleteCalendarHoliday(NoteId);

                await Delete(NoteId);
                return true;
            }

            return false;
        }

        public async Task<bool> ValidateHolidayName(NoteTemplateViewModel viewModel)
        {
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var calId = Convert.ToString(rowData.GetValueOrDefault("CalendarId"));
            var holName = Convert.ToString(rowData.GetValueOrDefault("HolidayName"));
            var exisiting = await CheckHolidayNameWithCalendar(calId, holName);
            if (exisiting.Count > 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<List<CalendarHolidayViewModel>> CheckHolidayNameWithCalendar(string calId, string holName)
        {
            var result = await _payRollQueryBusiness.CheckHolidayNameWithCalendar(calId, holName);
            return result;
        }

        public async Task<bool> ValidateCalendar(NoteTemplateViewModel viewModel)
        {
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                 
            var exisiting = await CheckCalendarWithLegalEntityId(viewModel.LegalEntityId, rowData["Name"].ToString(), rowData["Code"].ToString());
            if (exisiting.Count > 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<List<CalendarViewModel>> CheckCalendarWithLegalEntityId(string legalEId,string name,string code)
        {
            var result = await _payRollQueryBusiness.CheckCalendarWithLegalEntityId(legalEId, name, code);
            return result;
        }

    }
}
