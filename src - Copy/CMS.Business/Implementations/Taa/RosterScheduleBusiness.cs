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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
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

        public RosterScheduleBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            INoteBusiness noteBusiness, IUserContext userContext, ITableMetadataBusiness tableMetadataBusiness
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IRepositoryQueryBase<RosterDutyTemplateViewModel> queryRosterDuty,
            IRepositoryQueryBase<RosterTimeLineViewModel> queryRosterTime,
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
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);
            string query = $@"{companyquery} select d.""Id"" as Id ,d.""DepartmentName"" as Name
                            from cms.""N_CoreHR_HRDepartment"" as d 
                            join  ""Department"" as dept on dept.""DepartmentId""=d.""Id""
                            where d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                            ";
            //query = query.Replace("#comp#", companyquery);
            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<IdNameViewModel>> GetShiftPatternList()
        {
            
            var query =
                $@"SELECT rd.""Id"" as Id ,rd.""Name"" as Name FROM cms.""N_TAA_RosterDutyTemplate"" as rd
                    where rd.""IsDeleted""=false and rd.""CompanyId""='{_repo.UserContext.CompanyId}'
                        ";
            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }

        public async Task<RosterScheduleViewModel> GetExistingShiftPattern(string userId,string rostarDate)
        {

            var query =
                $@"SELECT rd.""NtsNoteId"" as NoteId ,rd.* FROM cms.""N_TAA_RosterSchedule"" as rd
                    where rd.""IsDeleted""=false and rd.""UserId""='{userId}' and substring(rd.""RosterDate"",0,11)='{rostarDate}' and rd.""CompanyId""='{_repo.UserContext.CompanyId}'
                        ";
            var queryData = await _queryRoster.ExecuteQuerySingle(query, null);
            var list = queryData;
            return list;
        }

        public async Task<RosterDutyTemplateViewModel> GetRosterDutyTemplateById(string Id)
        {
            var query =
                $@"SELECT rd.* FROM cms.""N_TAA_RosterDutyTemplate"" as rd
                      where rd.""Id""='{Id}' and rd.""IsDeleted""=false and rd.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            
            var queryData = await _queryRosterDuty.ExecuteQuerySingle(query, null);
           
            return queryData;
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

             var timeDiff = LegalEntityCode.ServerToLocalTimeDiff();

            var query = $@"select u.""Id"" as UserId,p.""Id"" as PersonId,p.""IqamahNoNationalId"" as SponsorshipNo,p.""PersonNo"" as PersonNo
,j.""JobTitle"" as JobName,n.""NationalityName"" as Nationality,d.""DepartmentName"" as OrganizationName
,c.""EffectiveEndDate"" as ContractEndDate,c.""ContractRenewable"" as ContractRenewable,sp.""Code"" as Sponsor
,a.""Id"" as AssignmentId,d.""Id"",p.""PersonFullName"" as EmployeeName
,case when count(case when nts.""LeaveStartDate"" <= '{sat}' and '{sat}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Sunday,
            case when count(case when nts.""LeaveStartDate"" <= '{sun}' and '{sun}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Monday,
            case when count(case when nts.""LeaveStartDate"" <= '{mon}' and '{mon}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Tuesday,
            case when count(case when nts.""LeaveStartDate"" <= '{tue}' and '{tue}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Wednesday,
            case when count(case when nts.""LeaveStartDate"" <= '{wed}' and '{wed}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Thursday,
            case when count(case when nts.""LeaveStartDate"" <= '{thu}' and '{thu}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Friday,
            case when count(case when nts.""LeaveStartDate"" <= '{fri}' and '{fri}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Saturday,
			            
coalesce(substring(rs1.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs1.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs1.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs1.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs1.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs1.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs1.""DraftDuty1FallsNextDay""='true' or rs1.""DraftDuty2FallsOnNextDay""='true' or rs1.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as SundayText,
coalesce(substring(rs2.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs2.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs2.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs2.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs2.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs2.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs2.""DraftDuty1FallsNextDay""='true' or rs2.""DraftDuty2FallsOnNextDay""='true' or rs2.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as MondayText,   
coalesce(substring(rs3.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs3.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs3.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs3.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs3.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs3.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs3.""DraftDuty1FallsNextDay""='true' or rs3.""DraftDuty2FallsOnNextDay""='true' or rs3.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as TuesdayText,  
coalesce(substring(rs4.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs4.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs4.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs4.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs4.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs4.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs4.""DraftDuty1FallsNextDay""='true' or rs4.""DraftDuty2FallsOnNextDay""='true' or rs4.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as WednesdayText,
coalesce(substring(rs5.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs5.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs5.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs5.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs5.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs5.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs5.""DraftDuty1FallsNextDay""='true' or rs5.""DraftDuty2FallsOnNextDay""='true' or rs5.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as ThursdayText, 
coalesce(substring(rs6.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs6.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs6.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs6.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs6.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs6.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs6.""DraftDuty1FallsNextDay""='true' or rs6.""DraftDuty2FallsOnNextDay""='true' or rs6.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as FridayText,   
coalesce(substring(rs7.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs7.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs7.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs7.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs7.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs7.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs7.""DraftDuty1FallsNextDay""='true' or rs7.""DraftDuty2FallsOnNextDay""='true' or rs7.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as SaturdayText, 


rs1.""DraftTotalHours"" as SundayTotalHours,
            rs2.""DraftTotalHours"" as MondayTotalHours,
            rs3.""DraftTotalHours"" as TuesdayTotalHours,
            rs4.""DraftTotalHours"" as WednesdayTotalHours,
            rs5.""DraftTotalHours"" as ThursdayTotalHours,
            rs6.""DraftTotalHours"" as FridayTotalHours,
            rs7.""DraftTotalHours"" as SaturdayTotalHours,

            rs1.""DraftRosterDutyType"" as DayOff1,
            rs2.""DraftRosterDutyType"" as DayOff2,
            rs3.""DraftRosterDutyType"" as DayOff3,
            rs4.""DraftRosterDutyType"" as DayOff4,
            rs5.""DraftRosterDutyType"" as DayOff5,
            rs6.""DraftRosterDutyType"" as DayOff6,
            rs7.""DraftRosterDutyType"" as DayOff7,
             
            rs1.""PublishStatus"" as RostarStatus1,
            rs2.""PublishStatus"" as RostarStatus2,
            rs3.""PublishStatus"" as RostarStatus3,
            rs4.""PublishStatus"" as RostarStatus4,
            rs5.""PublishStatus"" as RostarStatus5,
            rs6.""PublishStatus"" as RostarStatus6,
            rs7.""PublishStatus"" as RostarStatus7,

            rs1.""IsDraft"" as Draft1,
            rs2.""IsDraft"" as Draft2,
            rs3.""IsDraft"" as Draft3,
            rs4.""IsDraft"" as Draft4,
            rs5.""IsDraft"" as Draft5,
            rs6.""IsDraft"" as Draft6,
            rs7.""IsDraft"" as Draft7,

            case when rs1.""Duty1StartTime"" is not null then 1 else 0 end as Count1,
            case when rs2.""Duty1StartTime"" is not null then 1 else 0 end as Count2,
            case when rs3.""Duty1StartTime"" is not null then 1 else 0 end as Count3,
            case when rs4.""Duty1StartTime"" is not null then 1 else 0 end as Count4,
            case when rs5.""Duty1StartTime"" is not null then 1 else 0 end as Count5,
            case when rs6.""Duty1StartTime"" is not null then 1 else 0 end as Count6,
            case when rs7.""Duty1StartTime"" is not null then 1 else 0 end as Count7	

from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id""=p.""NationalityId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join cms.""N_CoreHR_HRSection"" as s on s.""Id""=p.""NationalityId"" and n.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'

left join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false and rs1.""RosterDate""='{sun}' and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs2 on u.""Id""=rs2.""UserId"" and rs2.""IsDeleted""=false and rs2.""RosterDate""='{mon}' and rs2.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs3 on u.""Id""=rs3.""UserId"" and rs3.""IsDeleted""=false and rs3.""RosterDate""='{tue}' and rs3.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs4 on u.""Id""=rs4.""UserId"" and rs4.""IsDeleted""=false and rs4.""RosterDate""='{wed}' and rs4.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs5 on u.""Id""=rs5.""UserId"" and rs5.""IsDeleted""=false and rs5.""RosterDate""='{thu}' and rs5.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs6 on u.""Id""=rs6.""UserId"" and rs6.""IsDeleted""=false and rs6.""RosterDate""='{fri}' and rs6.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs7 on u.""Id""=rs7.""UserId"" and rs7.""IsDeleted""=false and rs7.""RosterDate""='{sat}' and rs7.""CompanyId""='{_repo.UserContext.CompanyId}'
left join (
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts
join cms.""N_Leave_AnnualLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
	  
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts
join cms.""N_Leave_MaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	  
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_PaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false	and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	  
union 	  
 select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_CompassionatelyLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false	and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	   
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_HajjLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	  

	  
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_LeaveExamination"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	  
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_MarriageLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false

union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_PlannedUnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL')  and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false

union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_SickLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL')  and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false

union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_UndertimeLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false

union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_UnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}') 
               as nts on nts.""OwnerUserId""=u.""Id""
where d.""IsDeleted""=false #Org#			   
group by 	u.""Id"",p.""Id"",p.""IqamahNoNationalId"",p.""PersonNo""
,j.""JobTitle"",n.""NationalityName"",d.""DepartmentName""
,c.""EffectiveEndDate"",c.""ContractRenewable"",sp.""Code""
,a.""Id"",d.""Id"",p.""PersonFullName"",u.""Name"",rs1.""IsDraft""
,rs2.""IsDraft""
,rs3.""IsDraft""
,rs4.""IsDraft""
,rs5.""IsDraft""
,rs6.""IsDraft""
,rs7.""IsDraft""
,rs1.""DraftRosterDutyType""
,rs2.""DraftRosterDutyType""
,rs3.""DraftRosterDutyType""
,rs4.""DraftRosterDutyType""
,rs5.""DraftRosterDutyType""
,rs6.""DraftRosterDutyType""
,rs7.""DraftRosterDutyType""
,rs1.""DraftTotalHours""
,rs2.""DraftTotalHours""
,rs3.""DraftTotalHours""
,rs4.""DraftTotalHours""
,rs5.""DraftTotalHours""
,rs6.""DraftTotalHours""
,rs7.""DraftTotalHours""
,rs1.""Duty1StartTime""
,rs2.""Duty1StartTime""
,rs3.""Duty1StartTime""
,rs4.""Duty1StartTime""
,rs5.""Duty1StartTime""
,rs6.""Duty1StartTime""
,rs7.""Duty1StartTime""
,rs1.""DraftDuty1StartTime"",rs1.""DraftDuty2StartTime"",rs1.""DraftDuty3StartTime"",rs1.""DraftDuty1EndTime"",rs1.""DraftDuty2EndTime"",rs1.""DraftDuty3EndTime""
,rs2.""DraftDuty1StartTime"",rs2.""DraftDuty2StartTime"",rs2.""DraftDuty3StartTime"",rs2.""DraftDuty1EndTime"",rs2.""DraftDuty2EndTime"",rs2.""DraftDuty3EndTime""
,rs3.""DraftDuty1StartTime"",rs3.""DraftDuty2StartTime"",rs3.""DraftDuty3StartTime"",rs3.""DraftDuty1EndTime"",rs3.""DraftDuty2EndTime"",rs3.""DraftDuty3EndTime""
,rs4.""DraftDuty1StartTime"",rs4.""DraftDuty2StartTime"",rs4.""DraftDuty3StartTime"",rs4.""DraftDuty1EndTime"",rs4.""DraftDuty2EndTime"",rs4.""DraftDuty3EndTime""
,rs5.""DraftDuty1StartTime"",rs5.""DraftDuty2StartTime"",rs5.""DraftDuty3StartTime"",rs5.""DraftDuty1EndTime"",rs5.""DraftDuty2EndTime"",rs5.""DraftDuty3EndTime""
,rs6.""DraftDuty1StartTime"",rs6.""DraftDuty2StartTime"",rs6.""DraftDuty3StartTime"",rs6.""DraftDuty1EndTime"",rs6.""DraftDuty2EndTime"",rs6.""DraftDuty3EndTime""
,rs7.""DraftDuty1StartTime"",rs7.""DraftDuty2StartTime"",rs7.""DraftDuty3StartTime"",rs7.""DraftDuty1EndTime"",rs7.""DraftDuty2EndTime"",rs7.""DraftDuty3EndTime""

,rs1.""DraftDuty1FallsNextDay"",rs1.""DraftDuty2FallsOnNextDay"",rs1.""DraftDuty3FallsNextDay""
,rs2.""DraftDuty1FallsNextDay"",rs2.""DraftDuty2FallsOnNextDay"",rs2.""DraftDuty3FallsNextDay""
,rs3.""DraftDuty1FallsNextDay"",rs3.""DraftDuty2FallsOnNextDay"",rs3.""DraftDuty3FallsNextDay""
,rs4.""DraftDuty1FallsNextDay"",rs4.""DraftDuty2FallsOnNextDay"",rs4.""DraftDuty3FallsNextDay""
,rs5.""DraftDuty1FallsNextDay"",rs5.""DraftDuty2FallsOnNextDay"",rs5.""DraftDuty3FallsNextDay""
,rs6.""DraftDuty1FallsNextDay"",rs6.""DraftDuty2FallsOnNextDay"",rs6.""DraftDuty3FallsNextDay""
,rs7.""DraftDuty1FallsNextDay"",rs7.""DraftDuty2FallsOnNextDay"",rs7.""DraftDuty3FallsNextDay""
,rs1.""PublishStatus"" 
, rs2.""PublishStatus""
 ,rs3.""PublishStatus""
 ,rs4.""PublishStatus""
 ,rs5.""PublishStatus""
 ,rs6.""PublishStatus""
 ,rs7.""PublishStatus""
";



            var whereorg = "";
            if (orgId.IsNotNullAndNotEmpty()&&orgId!="0")
            {
                whereorg = $@" and d.""Id""='{orgId}'";
            }
            else
            {
                var legalorgids = await GetDepartmentList();
                var orgids1 = string.Join("','", legalorgids.Select(x => x.Id));
                var orgids = "'" + string.Join("','", legalorgids.Select(x => x.Id)) + "'";
                whereorg = $@" and d.""Id"" in ({orgids})";
            }
            query = query.Replace("#Org#", whereorg);
            var result = await _queryRoster.ExecuteQueryList(query, null);



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
            var query = $@"
            select rs1.*,rs1.""NtsNoteId"" as NoteId
            from cms.""N_CoreHR_HRDepartment"" as d
            join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
            where rs1.""RosterDate"" >='{start}' and rs1.""RosterDate""<='{end}' and d.""CompanyId""='{_repo.UserContext.CompanyId}' #ORG# ";

            var whereorg = "";
            if (orgId.IsNotNullAndNotEmpty() && orgId != "All")
            {
                whereorg = $@" and d.""Id""='{orgId}'";
            }
            query = query.Replace("#ORG#", whereorg);
            var queryData = await _queryRoster.ExecuteQueryList(query, null);

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

            var query = $@"
            select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""RosterDate"" as RosterDate,
            rs1.""Duty1StartTime"" as Duty1StartTime,rs1.""Duty1EndTime"" as Duty1EndTime,'R' as Type, 1 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""RosterDate"" is not null and rs1.""Duty1StartTime"" is not null and rs1.""Duty1EndTime"" is not null and '{sun}'<=rs1.""RosterDate"" and rs1.""RosterDate""<='{sat}'
 and rs1.""PublishStatus""='1' and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    
union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""RosterDate"" as RosterDate,
            rs1.""Duty2StartTime"" as Duty1StartTime,rs1.""Duty2EndTime"" as Duty1EndTime,'R' as Type, 1 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""RosterDate"" is not null and rs1.""Duty2StartTime"" is not null and rs1.""Duty2EndTime"" is not null and '{sun}'<=rs1.""RosterDate"" and rs1.""RosterDate""<='{sat}'
and rs1.""PublishStatus""='1' and rs1.""PublishStatus""='1' and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    
union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""RosterDate"" as RosterDate,
            rs1.""Duty3StartTime"" as Duty1StartTime,rs1.""Duty3EndTime"" as Duty1EndTime,'R' as Type, 1 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""RosterDate"" is not null and rs1.""Duty3StartTime"" is not null and rs1.""Duty3EndTime"" is not null and '{sun}'<=rs1.""RosterDate"" and rs1.""RosterDate""<='{sat}'
and rs1.""PublishStatus""='1' and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    

union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""AttendanceDate"" as RosterDate,
            rs1.""Duty1StartTime"" as Duty1StartTime,rs1.""Duty1EndTime"" as Duty1EndTime,'A' as Type, 2 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_Attendance"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false and rs1.""CompanyId""='{_repo.UserContext.CompanyId}' 
where rs1.""AttendanceDate"" is not null and rs1.""Duty1StartTime"" is not null and rs1.""Duty1EndTime"" is not null and '{sun}'<=rs1.""AttendanceDate"" and rs1.""AttendanceDate""<='{sat}'
 and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    

union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""AttendanceDate"" as RosterDate,
            rs1.""Duty2StartTime"" as Duty1StartTime,rs1.""Duty2EndTime"" as Duty1EndTime,'A' as Type, 2 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_Attendance"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""AttendanceDate"" is not null and rs1.""Duty2StartTime"" is not null and rs1.""Duty2EndTime"" is not null and '{sun}'<=rs1.""AttendanceDate"" and rs1.""AttendanceDate""<='{sat}'
 and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    

union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""AttendanceDate"" as RosterDate,
            rs1.""Duty3StartTime"" as Duty1StartTime,rs1.""Duty3EndTime"" as Duty1EndTime,'A' as Type, 2 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_Attendance"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""AttendanceDate"" is not null and rs1.""Duty3StartTime"" is not null and rs1.""Duty3EndTime"" is not null and '{sun}'<=rs1.""AttendanceDate"" and rs1.""AttendanceDate""<='{sat}'
 and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false  #Org#         ";

            var whereorg = "";
            if (orgId.IsNotNullAndNotEmpty() && orgId != "All")
            {
                whereorg = $@" and d.""Id""='{orgId}'";
            }
            query = query.Replace("#Org#", whereorg);
            var list = await _queryRosterTime.ExecuteQueryList(query, null);

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
            var query = $@"select distinct p.""Id"",concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name
from cms.""N_CoreHR_HRDepartment"" as d
left join(
WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""IsDeleted""=false #Org#


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false
                                join Department ns on h.""ParentDepartmentId"" = ns.""Id""
                                where  h.""IsDeleted""=false
                             )
                            select ""ParentId"" from Department  where Type = 'Child'

) as cdept on cdept.""ParentId""=d.""Id""
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
  where d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}' #Org#";
            var whereorg = "";
            if (orgId.IsNotNullAndNotEmpty() && orgId != "All")
            {
                whereorg = $@"and d.""Id""='{orgId}'";
            }
            query = query.Replace("#Org#", whereorg);
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;


        }

        public async Task<IList<DateTime>> GetDistinctNotcalculatedRosterDateList(DateTime rosterDate)
        {
            var match = $@"Select r.* from 
cms.""N_TAA_RosterSchedule"" as r 
join public.""User"" as u on u.""Id""=r.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
 where r.""RosterDate""::Date<='{rosterDate}'::Date and p.""BiometricId"" is not null   
                and (r.""IsAttendanceCalculated"" is  null or r.""IsAttendanceCalculated""='false') and r.""CompanyId""='{_repo.UserContext.CompanyId}'  and r.""IsDeleted""=false
";
            var list =await _queryRoster.ExecuteQueryList<RosterScheduleViewModel>(match,null);
            //var match = @"MATCH (r:TAA_RosterSchedule{IsDeleted:0,Status:'Active'})
            //            -[:R_RosterSchedule_User]->(u:ADM_User{IsDeleted:0,Status:'Active'})
            //            match (u)-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //            match (pr)<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0,Status:'Active',IsLatest:true})
            //            where r.RosterDate<={RosterDate} and p.BiometricId is not null  
            //            and (r.IsAttendanceCalculated is  null or r.IsAttendanceCalculated=false) 
            //            return r";
            //var list = ExecuteCypherList<TAA_RosterSchedule>(match, new Dictionary<string, object> { { "RosterDate", rosterDate } });
            return list.Select(x => x.RosterDate).Distinct().OrderBy(x => x).ToList();
        }
        public async Task<List<RosterScheduleViewModel>> GetPublishedRostersList(DateTime rosterDate)
        {
            //var cypher = @"MATCH (r:TAA_RosterSchedule{IsDeleted:0,Status:'Active',PublishStatus:'Published',RosterDate:{RosterDate}})
            //            -[:R_RosterSchedule_User]->(u:ADM_User{IsDeleted:0,Status:'Active'})
            //            -[:R_User_PersonRoot]->(pr:HRS_PersonRoot)<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0,Status:'Active',IsLatest:true})
            //            match (pr)<-[:R_UserInfo_PersonRoot]-(ui:CLK_UserInfo{IsDeleted:0,Status:'Active'})
            //            where  p.BiometricId is not null 
            //            return r,u.Id as UserId,p.BiometricId as BiometricId";
            var cypher = $@"select r.*,u.""Id"" as UserId,p.""BiometricId"" as BiometricId from
cms.""N_TAA_RosterSchedule"" as r 
join public.""User"" as u on u.""Id""=r.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CLK_UserInfo"" as ui on ui.""PersonId""=p.""Id"" and ui.""IsDeleted""=false and ui.""CompanyId""='{_repo.UserContext.CompanyId}'
where p.""BiometricId"" is not null  and r.""RosterDate""='{rosterDate}' and r.""CompanyId""='{_repo.UserContext.CompanyId}' and r.""IsDeleted""=false 
";
            return await _queryRoster.ExecuteQueryList<RosterScheduleViewModel>(cypher, null);
        }
        public async Task<IList<RosterScheduleViewModel>>  GetPublishedRostersForAttendance(DateTime rosterDate)
        {
            //var cypher = @"MATCH (r:TAA_RosterSchedule{IsDeleted:0,Status:'Active'})
            //            -[:R_RosterSchedule_User]->(u:ADM_User{IsDeleted:0,Status:'Active'})
            //            -[:R_User_PersonRoot]->(pr:HRS_PersonRoot)<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0,Status:'Active',IsLatest:true})
            //            match (pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(or:HRS_OrganizationRoot)
            //            <-[:R_LegalEntity_OrganizationRoot]-(le:ADM_LegalEntity)
            //            where r.PublishStatus='Published' and r.RosterDate={RosterDate} and p.BiometricId is not null  
            //           and (p.IsAttendanceCalculated is  null or p.IsAttendanceCalculated=false)
            //            return r,u.Id as UserId,p.BiometricId as BiometricId,le.LegalEntityCode as LegalEntityCode ";
            var cypher = $@"select r.*,u.""Id"" as UserId,p.""BiometricId"" as BiometricId,le.""Code"" as LegalEntityCode 
ms.""N_TAA_RosterSchedule"" as r 
join public.""User"" as u on u.""Id""=r.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LegalEntity"" as le on le.""Id""=p.""LegalEntityId"" and le.""IsDeleted""=false  and le.""CompanyId""='{_repo.UserContext.CompanyId}'
  where r.""PublishStatus""='1' and r.""RosterDate""='{rosterDate}' and p.""BiometricId"" is not null  
                 and (p.""IsAttendanceCalculated"" is  null or p.""IsAttendanceCalculated""=false)  and r.""CompanyId""='{_repo.UserContext.CompanyId}' and r.""IsDeleted""=false 
";
            return await _queryRoster.ExecuteQueryList<RosterScheduleViewModel>(cypher, null);
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
