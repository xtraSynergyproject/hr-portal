using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class PayrollBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollBusiness
    {
        
        private readonly IRepositoryQueryBase<CalendarViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<CalendarHolidayViewModel> _queryRepo;
        

        public PayrollBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper            
            , IRepositoryQueryBase<CalendarViewModel> queryRepo1,
            IRepositoryQueryBase<CalendarHolidayViewModel> queryRepo) : base(repo, autoMapper)
        {            
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            
        }

        public async override Task<CommandResult<NoteViewModel>> Create(NoteViewModel model)
        {

            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<NoteViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteViewModel>> Edit(NoteViewModel model)
        {

            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<NoteViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<CalendarViewModel> GetCalendarDetailsById(string id)
        {
            string query = $@"select *, ""NtsNoteId"" as NoteId from cms.""N_PayrollHR_PayrollCalendar"" where ""Id""='{id}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo1.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<List<CalendarViewModel>> GetCalendarListData()
        {
            var query = $@"SELECT * FROM cms.""N_PayrollHR_PayrollCalendar"" where ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<List<CalendarHolidayViewModel>> GetCalendarHolidayData(string calendarId)
        {
           var query = $@"SELECT *,ch.""NtsNoteId"" as NoteId FROM  public.""NtsNote"" N
                            inner join cms.""N_PayrollHR_CalendarHoliday"" ch on N.""Id"" =ch.""NtsNoteId"" and ch.""CalendarId""='{calendarId}' and ch.""IsDeleted""=false and ch.""CompanyId""='{_repo.UserContext.CompanyId}'
                              where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'" ;
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }


        public async Task<List<CalendarHolidayViewModel>> GetCalendarHolidayDatawithmonthYear(string calendarId,int Year,int month)
        {
            var query = $@"SELECT *	FROM cms.""N_PayrollHR_CalendarHoliday"" where  extract(year from CAST(""ToDate"" AS DATE))={Year}
    and extract(month from CAST(""ToDate"" AS DATE))= {month} and ""CalendarId"" = '{calendarId}'

    and ""IsDeleted"" = false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<CalendarHolidayViewModel> GetCalendarHolidayDetailsById(string calHolidayId)
        {
            string query = $@"select * from cms.""N_PayrollHR_CalendarHoliday"" where ""Id""='{calHolidayId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<bool> DeleteCalendarHoliday(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                var query = $@"update  cms.""N_PayrollHR_CalendarHoliday"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);

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
            string query = @$"SELECT * FROM cms.""N_PayrollHR_CalendarHoliday"" where ""HolidayName""='{holName}' and ""CalendarId""='{calId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQueryList<CalendarHolidayViewModel>(query, null);
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
            string query = @$"SELECT * FROM cms.""N_PayrollHR_PayrollCalendar"" where ""LegalEntityId""='{legalEId}' and (""Name""='{name}' or ""Code""='{code}') and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo1.ExecuteQueryList<CalendarViewModel>(query, null);
            return result;
        }

    }
}
