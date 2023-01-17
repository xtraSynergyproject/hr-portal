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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class RosterScheduleBusiness : BusinessBase<NoteViewModel, NtsNote>, IRosterScheduleBusiness
    {
        INoteBusiness _noteBusiness;
        IUserContext _userContext;
        ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<RosterScheduleViewModel> _queryRoster;
        private readonly IRepositoryQueryBase<RosterDutyTemplateViewModel> _queryRosterDuty;
        private readonly IRepositoryQueryBase<RosterTimeLineViewModel> _queryRosterTime;
        private readonly ITaaQueryBusiness _taaQueryBusiness;

        public RosterScheduleBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            INoteBusiness noteBusiness, IUserContext userContext, ITableMetadataBusiness tableMetadataBusiness
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IRepositoryQueryBase<RosterDutyTemplateViewModel> queryRosterDuty,
            IRepositoryQueryBase<RosterTimeLineViewModel> queryRosterTime,
            ITaaQueryBusiness taaQueryBusiness,
            IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<RosterScheduleViewModel> queryRoster) : base(repo, autoMapper)
        {
            _noteBusiness = noteBusiness;
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            _queryRoster = queryRoster;
            _userContext = userContext;
             _queryRosterDuty = queryRosterDuty;
            _tableMetadataBusiness = tableMetadataBusiness;
            _queryRosterTime = queryRosterTime;
            _taaQueryBusiness = taaQueryBusiness;
        }

        public async Task<CommandResult<RosterScheduleViewModel>> CreateRosterSchedule(RosterScheduleViewModel viewModel)
        {
            var LegalEntityCode = _userContext.LegalEntityCode;
            var timeDiff = LegalEntityCode.ServerToLocalTimeDiff();
            TimeSpan ts = new TimeSpan(-timeDiff, 0, 0);

            var dst1 = viewModel.DraftDuty1StartTime;
            if (dst1 != null)
            {
                viewModel.DraftDuty1StartTime = dst1.Value.Add(ts);
            }
            var det1 = viewModel.DraftDuty1EndTime;
            if (det1 != null)
            {
                viewModel.DraftDuty1EndTime = det1.Value.Add(ts);
            }
            var dst2 = viewModel.DraftDuty2StartTime;
            if (dst2 != null)
            {
                viewModel.DraftDuty2StartTime = dst2.Value.Add(ts);
            }
            var det2 = viewModel.DraftDuty2EndTime;
            if (det2 != null)
            {
                viewModel.DraftDuty2EndTime = det2.Value.Add(ts);
            }
            var dst3 = viewModel.DraftDuty3StartTime;
            if (dst3 != null)
            {
                viewModel.DraftDuty3StartTime = dst3.Value.Add(ts);
            }
            var det3 = viewModel.DraftDuty3EndTime;
            if (det3 != null)
            {
                viewModel.DraftDuty3EndTime = det3.Value.Add(ts);
            }

            viewModel.IsAttendanceCalculated = false;
            var errorList = new Dictionary<string, string>();
            var validateName =await IsValidationExists(viewModel);
            if (!validateName.IsSuccess)
            {
                return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false, validateName.Message);
            }
           

            TimeSpan total = new TimeSpan(00, 00, 00);

            if (viewModel.DraftDuty1StartTime != null && viewModel.DraftDuty1EndTime != null)
            {
                if (viewModel.DraftDuty1FallsNextDay)
                {

                    DateTime date = DateTime.Now.Date;

                    DateTime st = date.Add(viewModel.DraftDuty1StartTime.Value);
                    DateTime et = date.AddDays(1).Add(viewModel.DraftDuty1EndTime.Value);

                    TimeSpan duration = et - st;

                    total = total.Add(duration);
                }
                else
                {
                    TimeSpan duration = DateTime.Parse(viewModel.DraftDuty1EndTime.ToString()).Subtract(DateTime.Parse(viewModel.DraftDuty1StartTime.ToString()));
                    total = duration;
                }


                viewModel.DraftDuty1Enabled = true;
            }
            if (viewModel.DraftDuty2StartTime != null && viewModel.DraftDuty2EndTime != null)
            {
                if (viewModel.DraftDuty2FallsNextDay)
                {
                    DateTime date = DateTime.Now.Date;

                    DateTime st = date.Add(viewModel.DraftDuty2StartTime.Value);
                    DateTime et = date.AddDays(1).Add(viewModel.DraftDuty2EndTime.Value);

                    TimeSpan duration = et - st;

                    total = total.Add(duration);
                }
                else
                {
                    TimeSpan duration = DateTime.Parse(viewModel.DraftDuty2EndTime.ToString()).Subtract(DateTime.Parse(viewModel.DraftDuty2StartTime.ToString()));
                    total = total.Add(duration);
                }
                viewModel.DraftDuty2Enabled = true;
            }
            if (viewModel.DraftDuty3StartTime != null && viewModel.DraftDuty3EndTime != null)
            {
                if (viewModel.DraftDuty3FallsNextDay)
                {
                    DateTime date = DateTime.Now.Date;

                    DateTime st = date.Add(viewModel.DraftDuty3StartTime.Value);
                    DateTime et = date.AddDays(1).Add(viewModel.DraftDuty3EndTime.Value);

                    TimeSpan duration = et - st;

                    total = total.Add(duration);
                }
                else
                {
                    TimeSpan duration = DateTime.Parse(viewModel.DraftDuty3EndTime.ToString()).Subtract(DateTime.Parse(viewModel.DraftDuty3StartTime.ToString()));
                    total = total.Add(duration);
                }

                viewModel.DraftDuty3Enabled = true;
            }


            viewModel.DraftTotalHours = total;



            var users = viewModel.UserIds.Split(",");
            var dates = viewModel.RosterDates.Split(",");
            dates = dates.SkipLast(1).ToArray();
            users = users.SkipLast(1).ToArray();
            foreach(var user in users)
            {
               foreach(var date in dates)
                {
                    var exist = await GetExistingShiftPattern(user, date);
                    
                    if (exist==null)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "RosterSchedule";
                        viewModel.RosterDate = Convert.ToDateTime(date);
                        viewModel.UserId = user;
                        viewModel.IsDraft = true;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        notemodel.StartDate = DateTime.Now;
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.OwnerUserId = user;
                        notemodel.Json = JsonConvert.SerializeObject(viewModel);
                        var result = await _noteBusiness.ManageNote(notemodel);
                        if (!result.IsSuccess)
                        {
                            return CommandResult<RosterScheduleViewModel>.Instance(viewModel,false,result.Messages);
                        }
                    }
                    else
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                                     
                        noteTempModel.TemplateCode = "RosterSchedule";
                        noteTempModel.NoteId = exist.NoteId;                        
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        notemodel.StartDate = DateTime.Now;
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.OwnerUserId = user;
                        notemodel.DataAction = DataActionEnum.Edit;
                        viewModel.RosterDate = Convert.ToDateTime(date);
                        viewModel.UserId = user;
                        viewModel.RosterDutyType = exist.RosterDutyType;
                        viewModel.Duty1Enabled = exist.Duty1Enabled;
                        viewModel.Duty1StartTime = exist.Duty1StartTime;
                        viewModel.Duty1EndTime = exist.Duty1EndTime;
                        viewModel.Duty1FallsNextDay = exist.Duty1FallsNextDay;
                        viewModel.Duty2Enabled = exist.Duty2Enabled;
                        viewModel.Duty2StartTime = exist.Duty2StartTime;
                        viewModel.Duty2EndTime = exist.Duty2EndTime;
                        viewModel.Duty2FallsNextDay = exist.Duty2FallsNextDay;
                        viewModel.Duty3Enabled = exist.Duty3Enabled;
                        viewModel.Duty3StartTime = exist.Duty3StartTime;
                        viewModel.Duty3EndTime = exist.Duty3EndTime;
                        viewModel.Duty3FallsNextDay = exist.Duty3FallsNextDay;
                        viewModel.TotalHours = exist.TotalHours;
                        viewModel.PublishDate = exist.PublishDate;
                        viewModel.IsDraft = true;
                        notemodel.Json = JsonConvert.SerializeObject(viewModel);
                        var result = await _noteBusiness.ManageNote(notemodel);
                        if (!result.IsSuccess)
                        {
                            return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false, result.Messages);
                        }
                    }
                }
          
            }



            return CommandResult<RosterScheduleViewModel>.Instance(viewModel);
            
        }

        private async Task<CommandResult<RosterScheduleViewModel>> IsValidationExists(RosterScheduleViewModel viewModel)
        {
            if ((viewModel.DraftDuty1StartTime == null || viewModel.DraftDuty1EndTime == null) && viewModel.DraftRosterDutyType != RosterDutyTypeEnum.DayOff && viewModel.DraftRosterDutyType != RosterDutyTypeEnum.PublicHoliday)
            {
                return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false,"Please Enter Duty 1 Details");
            }
            else if (viewModel.DraftDuty1StartTime > viewModel.DraftDuty1EndTime && viewModel.DraftDuty1FallsNextDay != true)
            {
                return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false,"Please Check Duty 1 Fall On Next Day");
            }
            else if (viewModel.DraftDuty1StartTime < viewModel.DraftDuty1EndTime && viewModel.DraftDuty1FallsNextDay == true)
            {
                return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false, "Please Un Check Duty 1 Fall On Next Day");
            }

            if (viewModel.DraftDuty2StartTime != null && viewModel.DraftDuty2EndTime != null)
            {

                if (viewModel.DraftDuty1FallsNextDay)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false, "You can not enter the Duty 2 because Duty 1 fall on next day");
                }
                else if (viewModel.DraftDuty2StartTime < viewModel.DraftDuty1EndTime)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false,"Duty 2 Start Time Should be greater than  Duty 1 End Time");
                }
                else if (viewModel.DraftDuty2StartTime > viewModel.DraftDuty2EndTime && viewModel.DraftDuty2FallsNextDay != true)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false,  "Please Check Duty 2 Fall On Next Day");
                }
                else if (viewModel.DraftDuty2StartTime < viewModel.DraftDuty2EndTime && viewModel.DraftDuty2FallsNextDay == true)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false,  "Please Un Check Duty 2 Fall On Next Day");
                }

            }

            if (viewModel.DraftDuty3StartTime != null && viewModel.DraftDuty3EndTime != null)
            {
                if (viewModel.DraftDuty2FallsNextDay)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false, "You can not enter the Duty 3 because Duty 2 fall on next day");
                }
                else if (viewModel.DraftDuty3StartTime < viewModel.DraftDuty2EndTime)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false,  "Duty 3 Start Time Should be greater than  Duty 2 End Time");
                }
                else if (viewModel.DraftDuty3StartTime > viewModel.DraftDuty3EndTime && viewModel.DraftDuty3FallsNextDay != true)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false,  "Please Check Duty 3 Fall On Next Day");
                }
                else if (viewModel.DraftDuty3StartTime < viewModel.DraftDuty3EndTime && viewModel.DraftDuty3FallsNextDay == true)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false,  "Please Un Check Duty 3 Fall On Next Day");
                }
            }

            return CommandResult<RosterScheduleViewModel>.Instance();
        }

        public async Task<List<IdNameViewModel>> GetDepartmentList()
        {
            var result = await _taaQueryBusiness.GetDepartmentList();
            return result;
        }

        public async Task<List<IdNameViewModel>> GetShiftPatternList()
        {
            var result = await _taaQueryBusiness.GetShiftPatternList();
            return result;
        }

        public async Task<RosterScheduleViewModel> GetExistingShiftPattern(string userId,string rostarDate)
        {
            var result = await _taaQueryBusiness.GetExistingShiftPattern(userId,rostarDate);
            return result;
        }

        public async Task<RosterDutyTemplateViewModel> GetRosterDutyTemplateById(string Id)
        {
            var result = await _taaQueryBusiness.GetRosterDutyTemplateById(Id);
            return result;
        }

        public async Task<List<RosterScheduleViewModel>> GetRosterSchedulerList(string orgId, DateTime? date = null)
        {
            date = date ?? DateTime.Today;
            var LegalEntityId = _userContext.LegalEntityId;
            var LegalEntityCode = _userContext.LegalEntityCode;
            //orgId = orgId == null  ? LegalEntityId : orgId;
            var firstDayOfweek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var dayOfDate = (int)date.Value.DayOfWeek;
            var dateAdd = 0;
            if (firstDayOfweek > dayOfDate)
            {
                dateAdd = firstDayOfweek - dayOfDate - 7;
            }
            else
            {
                dateAdd = firstDayOfweek - dayOfDate;
            }

            var sunday = date.Value.AddDays(dateAdd);

            var sun = sunday;
            var mon = sunday.AddDays(1);
            var tue = sunday.AddDays(2);
            var wed = sunday.AddDays(3);
            var thu = sunday.AddDays(4);
            var fri = sunday.AddDays(5);
            var sat = sunday.AddDays(6);

            //var timeDiff = LegalEntityCode.ServerToLocalTimeDiff();

            var result = await _taaQueryBusiness.GetRosterSchedulerList(orgId,sun,mon,tue,wed,thu,fri,sat, LegalEntityCode);



            foreach (var item in result)
            {
                TimeSpan total = new TimeSpan(00, 00, 00);
                if (item.SundayTotalHours != null)
                {
                    total = total.Add(item.SundayTotalHours.Value);
                    item.SundayHours = string.Concat((item.SundayTotalHours.Value.Days * 24) + item.SundayTotalHours.Value.Hours, ":", item.SundayTotalHours.Value.Minutes);

                }
                if (item.MondayTotalHours != null)
                {
                    total = total.Add(item.MondayTotalHours.Value);
                    item.MondayHours = string.Concat((item.MondayTotalHours.Value.Days * 24) + item.MondayTotalHours.Value.Hours, ":", item.MondayTotalHours.Value.Minutes);
                }
                if (item.TuesdayTotalHours != null)
                {
                    total = total.Add(item.TuesdayTotalHours.Value);
                    item.TuesdayHours = string.Concat((item.TuesdayTotalHours.Value.Days * 24) + item.TuesdayTotalHours.Value.Hours, ":", item.TuesdayTotalHours.Value.Minutes);
                }
                if (item.WednesdayTotalHours != null)
                {
                    total = total.Add(item.WednesdayTotalHours.Value);
                    item.WednesdayHours = string.Concat((item.WednesdayTotalHours.Value.Days * 24) + item.WednesdayTotalHours.Value.Hours, ":", item.WednesdayTotalHours.Value.Minutes);
                }
                if (item.ThursdayTotalHours != null)
                {
                    total = total.Add(item.ThursdayTotalHours.Value);
                    item.ThursdayHours = string.Concat((item.ThursdayTotalHours.Value.Days * 24) + item.ThursdayTotalHours.Value.Hours, ":", item.ThursdayTotalHours.Value.Minutes);
                }
                if (item.FridayTotalHours != null)
                {
                    total = total.Add(item.FridayTotalHours.Value);
                    item.FridayHours = string.Concat((item.FridayTotalHours.Value.Days * 24) + item.FridayTotalHours.Value.Hours, ":", item.FridayTotalHours.Value.Minutes);
                }
                if (item.SaturdayTotalHours != null)
                {
                    total = total.Add(item.SaturdayTotalHours.Value);
                    item.SaturdayHours = string.Concat((item.SaturdayTotalHours.Value.Days * 24) + item.SaturdayTotalHours.Value.Hours, ":", item.SaturdayTotalHours.Value.Minutes);
                }
                item.SumOfWeekHours = string.Concat((total.Days * 24) + total.Hours, ":", total.Minutes);

            }


            return result;

        }

        public async Task<List<RosterScheduleViewModel>> GetRosterScheduleByRosterDate(string orgId, DateTime start,DateTime end)
        {
            var queryData = await _taaQueryBusiness.GetRosterScheduleByRosterDate(orgId,start,end);
            return queryData;
        }

        public async Task<CommandResult<RosterScheduleViewModel>> PublishRoster(string orgId, DateTime? date)
        {
            date = date ?? DateTime.Today;

            var firstDayOfweek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var dayOfDate = (int)date.Value.DayOfWeek;
            var dateAdd = 0;
            if (firstDayOfweek > dayOfDate)
            {
                dateAdd = firstDayOfweek - dayOfDate - 7;
            }
            else
            {
                dateAdd = firstDayOfweek - dayOfDate;
            }

            var start = date.Value.AddDays(dateAdd);
            var end = start.AddDays(6);


            var data = await GetRosterScheduleByRosterDate(orgId, start, end);

            foreach (var item in data)
            {
             
               
                item.PublishDate = DateTime.Now.ApplicationNow().Date;

                var noteTempModel = new NoteTemplateViewModel();

                noteTempModel.TemplateCode = "RosterSchedule";
                noteTempModel.NoteId = item.NoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.StartDate = DateTime.Now;
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.DataAction = DataActionEnum.Edit;
                item.PublishStatus = DocumentStatusEnum.Published;
                item.RosterDutyType = item.DraftRosterDutyType;
                item.Duty1Enabled = item.DraftDuty1Enabled;
                item.Duty1StartTime = item.DraftDuty1StartTime;
                item.Duty1EndTime = item.DraftDuty1EndTime;
                item.Duty1FallsNextDay = item.DraftDuty1FallsNextDay;
                item.Duty2Enabled = item.DraftDuty2Enabled;
                item.Duty2StartTime = item.DraftDuty2StartTime;
                item.Duty2EndTime = item.DraftDuty2EndTime;
                item.Duty2FallsNextDay = item.DraftDuty2FallsNextDay;
                item.Duty3Enabled = item.DraftDuty3Enabled;
                item.Duty3StartTime = item.DraftDuty3StartTime;
                item.Duty3EndTime = item.DraftDuty3EndTime;
                item.Duty3FallsNextDay = item.DraftDuty3FallsNextDay;
                item.TotalHours = item.DraftTotalHours;
                item.IsAttendanceCalculated = false;
                item.IsDraft = false;
                notemodel.Json = JsonConvert.SerializeObject(item);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (!result.IsSuccess)
                {
                    return CommandResult<RosterScheduleViewModel>.Instance(item, false, result.Messages);
                }

            }
            return CommandResult<RosterScheduleViewModel>.Instance();

        }

        public async Task<RosterScheduleViewModel> GetPublishedDate(string orgId, DateTime? date)
        {
            date = date ?? DateTime.Today;

            var firstDayOfweek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var dayOfDate = (int)date.Value.DayOfWeek;
            var dateAdd = 0;
            if (firstDayOfweek > dayOfDate)
            {
                dateAdd = firstDayOfweek - dayOfDate - 7;
            }
            else
            {
                dateAdd = firstDayOfweek - dayOfDate;
            }

            var start = date.Value.AddDays(dateAdd);
            var end = start.AddDays(6);

            var result = await GetRosterScheduleByRosterDate(orgId, start, end);


            return result.FirstOrDefault();

        }

        public async Task<CommandResult<RosterScheduleViewModel>> CopyRoster(RosterScheduleViewModel viewModel)
        {
            if (viewModel.CopyToWeeks == null || viewModel.CopyToWeeks.Count == 0)
            {
                return CommandResult<RosterScheduleViewModel>.Instance(viewModel, false, "Please select atleast one week to copy to.");
            }

            var userIds = viewModel.UserIds.Trim(',').Split(',').Distinct().ToList();
            var rosterDates = viewModel.RosterDates.Trim(',').Split(',').Distinct().Select(x => Convert.ToDateTime(x)).ToList();

            foreach (var item in viewModel.CopyToWeeks)
            {
                var weekDate =item.Split('_').FirstOrDefault();
                var week = Convert.ToDateTime(weekDate).Date;
                //if (weekDate < DateTime.Today)
                //{
                //    continue;
                //}
                foreach (var userId in userIds)
                {
                    foreach (var date in rosterDates)
                    {

                        var rosterDate = week.AddDays((int)date.DayOfWeek);
                        var copydate = date.ToShortDateString();
                        //var userDetails = _userBusiness.GetCompleteUserDetails(userId, rosterDate);
                        //if (userDetails != null && userDetails.ContractEffectiveStartDate <= rosterDate
                        //    && userDetails.AssignmentEffectiveStartDate <= rosterDate)
                       // {
                            var oldRoster = await GetExistingShiftPattern(userId, copydate);
                            if (oldRoster != null)
                            {
                                var data = new RosterScheduleViewModel
                                {
                                    RosterDate = rosterDate,                                    
                                    DraftRosterDutyType = oldRoster.DraftRosterDutyType,
                                    DraftDuty1Enabled = oldRoster.DraftDuty1Enabled,
                                    DraftDuty1StartTime = oldRoster.DraftDuty1StartTime,
                                    DraftDuty1EndTime = oldRoster.DraftDuty1EndTime,
                                    DraftDuty1FallsNextDay = oldRoster.DraftDuty1FallsNextDay,
                                    DraftDuty2Enabled = oldRoster.DraftDuty2Enabled,
                                    DraftDuty2StartTime = oldRoster.DraftDuty2StartTime,
                                    DraftDuty2EndTime = oldRoster.Duty2EndTime,
                                    DraftDuty2FallsNextDay = oldRoster.DraftDuty2FallsNextDay,
                                    DraftDuty3Enabled = oldRoster.DraftDuty3Enabled,
                                    DraftDuty3StartTime = oldRoster.DraftDuty3StartTime,
                                    DraftDuty3EndTime = oldRoster.DraftDuty3EndTime,
                                    DraftDuty3FallsNextDay = oldRoster.DraftDuty3FallsNextDay,
                                    DraftTotalHours = oldRoster.DraftTotalHours,
                                    PublishStatus = DocumentStatusEnum.Draft,
                                    ShiftPatternName = oldRoster.ShiftPatternName,
                                    IsAttendanceCalculated = false,
                                    IsDraft = true,
                                    UserId=userId
                                };
                                // if (viewModel.PublishWhileCopying && oldRoster.PublishStatus == DocumentStatusEnum.Published)
                                if (viewModel.PublishWhileCopying)
                                {
                                    data.RosterDutyType = oldRoster.DraftRosterDutyType;
                                    data.Duty1Enabled = oldRoster.DraftDuty1Enabled;
                                    data.Duty1StartTime = oldRoster.DraftDuty1StartTime;
                                    data.Duty1EndTime = oldRoster.DraftDuty1EndTime;
                                    data.Duty1FallsNextDay = oldRoster.DraftDuty1FallsNextDay;
                                    data.Duty2Enabled = oldRoster.DraftDuty2Enabled;
                                    data.Duty2StartTime = oldRoster.DraftDuty2StartTime;
                                    data.Duty2EndTime = oldRoster.Duty2EndTime;
                                    data.Duty2FallsNextDay = oldRoster.DraftDuty2FallsNextDay;
                                    data.Duty3Enabled = oldRoster.DraftDuty3Enabled;
                                    data.Duty3StartTime = oldRoster.DraftDuty3StartTime;
                                    data.Duty3EndTime = oldRoster.DraftDuty3EndTime;
                                    data.Duty3FallsNextDay = oldRoster.DraftDuty3FallsNextDay;
                                    data.TotalHours = oldRoster.DraftTotalHours;
                                    data.PublishStatus = DocumentStatusEnum.Published;
                                    data.IsAttendanceCalculated = false;
                                    data.IsDraft = false;
                                }                             
                            var newRoster = await GetExistingShiftPattern(userId, rosterDate.ToShortDateString());

                            if (newRoster == null)
                            {
                                var noteTempModel = new NoteTemplateViewModel();
                                noteTempModel.DataAction = DataActionEnum.Create;
                                noteTempModel.ActiveUserId = _userContext.UserId;
                                noteTempModel.TemplateCode = "RosterSchedule";                               
                                viewModel.IsDraft = true;
                                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                                notemodel.StartDate = DateTime.Now;
                                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                notemodel.OwnerUserId = userId;
                                notemodel.Json = JsonConvert.SerializeObject(data);
                                var result = await _noteBusiness.ManageNote(notemodel);
                                if (!result.IsSuccess)
                                {
                                    return CommandResult<RosterScheduleViewModel>.Instance(data, false, result.Messages);
                                }
                            }
                            else
                            {
                                var noteTempModel = new NoteTemplateViewModel();

                                noteTempModel.TemplateCode = "RosterSchedule";
                                noteTempModel.NoteId = newRoster.NoteId;
                                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                                notemodel.StartDate = DateTime.Now;
                                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                notemodel.OwnerUserId = userId;
                                notemodel.DataAction = DataActionEnum.Edit;                               
                                notemodel.Json = JsonConvert.SerializeObject(data);
                                var result = await _noteBusiness.ManageNote(notemodel);
                                if (!result.IsSuccess)
                                {
                                    return CommandResult<RosterScheduleViewModel>.Instance(data, false, result.Messages);
                                }
                            }
                        }
                        //}
                    }
                }
            }
            return CommandResult<RosterScheduleViewModel>.Instance(viewModel);
        }

        public async Task<bool> DeleteRoster(string users, string dates)
        {

            var userIds = users.Split(",");
            var rosterdates = dates.Split(",");
            rosterdates = rosterdates.SkipLast(1).ToArray();
            userIds = userIds.SkipLast(1).ToArray();
            foreach (var user in userIds)
            {
                foreach (var date in rosterdates)
                {
                    var exist = await GetExistingShiftPattern(user, date);
                    var del = await _tableMetadataBusiness.DeleteTableDataByHeaderId("RosterSchedule", "", exist.NoteId);
                }
            }

            return true;   
            
        }

        public async Task<IList<RosterTimeLineViewModel>> GetRosterTimeList(string orgId, DateTime? date = null)
        {
            date = date ?? DateTime.Today;

            var firstDayOfweek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var dayOfDate = (int)date.Value.DayOfWeek;
            var dateAdd = 0;
            if (firstDayOfweek > dayOfDate)
            {
                dateAdd = firstDayOfweek - dayOfDate - 7;
            }
            else
            {
                dateAdd = firstDayOfweek - dayOfDate;
            }

            var sunday = date.Value.AddDays(dateAdd);

            var sun = sunday.FirstDateOfMonth();
            var days = DateTime.DaysInMonth(sunday.Year, sunday.Month);
            var last= new DateTime(sunday.Year, sunday.Month, days, sunday.Hour, sunday.Minute, sunday.Second);
            var sat = last.AddDays(7);

            
            var list = await _taaQueryBusiness.GetRosterTimeList(orgId,sun,sat);

            foreach (var item in list)
            {
                if (item.RosterDate != null)
                {
                    DateTime date1 = item.RosterDate.Value.Date;
                    DateTime date2 = item.RosterDate.Value.Date;

                    if (item.Duty1StartTime != null && item.Duty1EndTime != null)
                    {
                        item.Start = date1.Add(item.Duty1StartTime.Value);
                        if (item.Duty1StartTime > item.Duty1EndTime)
                        {
                            item.End = date2.AddDays(1).Add(item.Duty1EndTime.Value);
                        }
                        else
                        {
                            item.End = date2.Add(item.Duty1EndTime.Value);
                        }
                    }
                }
            }

            return list;
        }

        public async Task<List<IdNameViewModel>> GetPersonListByOrganizationHerarchy(string orgId)
        {
            var result = await _taaQueryBusiness.GetPersonListByOrganizationHerarchy(orgId);
            return result;

        }

        public async Task<IList<DateTime>> GetDistinctNotcalculatedRosterDateList(DateTime rosterDate)
        {
            var list = await _taaQueryBusiness.GetDistinctNotcalculatedRosterDateList(rosterDate);
            return list.Select(x => x.RosterDate).Distinct().OrderBy(x => x).ToList();
        }
        public async Task<List<RosterScheduleViewModel>> GetPublishedRostersList(DateTime rosterDate)
        {
            var result = await _taaQueryBusiness.GetPublishedRostersList(rosterDate);
            return result;
        }
        public async Task<IList<RosterScheduleViewModel>>  GetPublishedRostersForAttendance(DateTime rosterDate)
        {
            var result = await _taaQueryBusiness.GetPublishedRostersForAttendance(rosterDate);
            return result;
        }
        public async Task<CommandResult<RosterScheduleViewModel>> Correct(RosterScheduleViewModel viewModel)
        {
            try
            {
                //var data = BusinessHelper.MapModel<RosterScheduleViewModel, TAA_RosterSchedule>(viewModel);
                // _repository.Edit(data);
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = viewModel.NoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel);
                var result = await _noteBusiness.ManageNote(notemodel);
                return CommandResult<RosterScheduleViewModel>.Instance(viewModel, result.Messages);
               
            }
            catch (Exception ex)
            {
                //Log.Instance.Error(ex, "Error on CorrectAttendanceFromBiometrics");
                throw;
            }
        }
    }
}
