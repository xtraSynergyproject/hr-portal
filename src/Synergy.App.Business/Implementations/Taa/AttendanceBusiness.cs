﻿using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class AttendanceBusiness : BusinessBase<NoteViewModel, NtsNote>, IAttendanceBusiness
    {
        INoteBusiness _noteBusiness;
        IUserContext _userContext;
        IServiceProvider _serviceProvider;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _queryPayBatch;
        private readonly IServiceBusiness _serviceBusiness;

        private readonly IRepositoryQueryBase<TimeinTimeoutDetailsViewModel> _queryTimeinTimeOutDetail;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;
        private readonly IRepositoryQueryBase<AttendanceToPayrollViewModel> _queryAttendanceToPayroll;
        private readonly IRepositoryQueryBase<TimePermissionAttendanceViewModel> _queryTimePer;
        private readonly IRepositoryQueryBase<AttendanceViewModel> _queryAttendance;
        private readonly IRepositoryQueryBase<EmployeeServiceViewModel> _queryEmpSer;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IRosterScheduleBusiness _rosterScheduleBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly IRepositoryQueryBase<SalaryInfoViewModel> _querySalInfo;
        private readonly IRepositoryQueryBase<AccessLogViewModel> _queryAccessLog;
        private readonly IServiceProvider _sp;
        private double _otEndTime = 4;
        private double _otStart = 3;
        private List<AttendanceViewModel> TodaysAttendanceList;
        private readonly ITaaQueryBusiness _taaQueryBusiness;
        public AttendanceBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            INoteBusiness noteBusiness, IUserContext userContext
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch
            , IServiceBusiness serviceBusiness, IRepositoryQueryBase<TimeinTimeoutDetailsViewModel> queryTimeinTimeOutDetail
            , IPayrollBatchBusiness payrollBatchBusiness, IRepositoryQueryBase<AttendanceViewModel> queryAttendance, IRepositoryQueryBase<EmployeeServiceViewModel> queryEmpSer
            , IRepositoryQueryBase<AttendanceToPayrollViewModel> queryAttendanceToPayroll, IRepositoryQueryBase<TimePermissionAttendanceViewModel> queryTimePer
            , ITemplateCategoryBusiness templateCategoryBusiness, ITemplateBusiness templateBusiness, ITableMetadataBusiness tableMetadataBusiness
            , IRosterScheduleBusiness rosterScheduleBusiness
            , IRepositoryQueryBase<SalaryInfoViewModel> querySalInfo
            , ITaaQueryBusiness taaQueryBusiness
            , IRepositoryQueryBase<AccessLogViewModel> queryAccessLog
            , IServiceProvider sp , IServiceProvider serviceProvider
            , ILOVBusiness lovBusiness) : base(repo, autoMapper)
        {
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _queryRepo1 = queryRepo1;
            _serviceProvider = serviceProvider;
            _queryRepo = queryRepo;
            _queryPayBatch = queryPayBatch;
            _serviceBusiness = serviceBusiness;
            _queryTimeinTimeOutDetail = queryTimeinTimeOutDetail;
            _payrollBatchBusiness = payrollBatchBusiness;
            _queryAttendanceToPayroll = queryAttendanceToPayroll;
            _queryAttendance = queryAttendance;
            _templateCategoryBusiness = templateCategoryBusiness;
            _templateBusiness = templateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _queryTimePer = queryTimePer;
            _queryEmpSer = queryEmpSer;
            _rosterScheduleBusiness = rosterScheduleBusiness;
            _querySalInfo = querySalInfo;
            _queryAccessLog = queryAccessLog;
            _sp = sp;
            _lovBusiness = lovBusiness;
            _taaQueryBusiness = taaQueryBusiness;
        }

        public async Task<CommandResult<AttendanceViewModel>> CreateAttendance(AttendanceViewModel viewModel)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _repo.UserContext.UserId;
            noteTempModel.TemplateCode = "Attendance";
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
            var result = await _noteBusiness.ManageNote(notemodel);
            return CommandResult<AttendanceViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);

        }
        public async Task<CommandResult<AttendanceViewModel>> CorrectAttendance(AttendanceViewModel viewModel)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _repo.UserContext.UserId;
            noteTempModel.NoteId = viewModel.NtsNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel);
            var result = await _noteBusiness.ManageNote(notemodel);

            return CommandResult<AttendanceViewModel>.Instance(viewModel);


        }
        public async Task<AttendanceViewModel> GetAttendanceSingleForPersonandDate(string personId, DateTime date)
        {
            var result = await _taaQueryBusiness.GetAttendanceSingleForPersonandDate(personId, date);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetDepartmentIdNameList()
        {
            var querydata = await _taaQueryBusiness.GetDepartmentIdNameList();
            return querydata;
        }
        public async Task<List<AttendanceViewModel>> GetAttendanceList(string orgId, DateTime? date)
        {
            var result = await _taaQueryBusiness.GetAttendanceList(orgId, date);

            if (result.Count > 0)
            {
                string users = null;
                var Ids = result.Select(x => x.UserId).ToList();
                foreach (var i in Ids)
                {
                    users += $"'{i}',";
                }
                if (users.IsNotNullAndNotEmpty())
                {
                    users = users.Trim(',');
                }

                var SD = date.Value.AddDays(-1);
                var ED = date.Value.AddDays(2);

                var userLogs = await _taaQueryBusiness.GetAccessLogs(SD,ED,users);

                foreach (var item in result)
                {
                    SetRosterStartAndEndDate(item, date ?? DateTime.Today);
                    var userAccessLog = userLogs.Where(x => x.UserId == item.UserId && x.PunchingTime >= item.RosterStartDate
                    && x.PunchingTime <= item.RosterEndDate).OrderBy(x => x.PunchingTime).Select(x => x.PunchingTime.ToDD_MMM_YYYY_HHMMSS()).ToList();
                    if (userAccessLog.Any())
                    {
                        item.AccessLogText = string.Join("<br/>", userAccessLog);
                    }
                    else
                    {
                        item.AccessLogText = string.Empty;
                    }

                }
            }

            return result;
        }
        private void SetRosterStartAndEndDate(AttendanceViewModel item, DateTime date)
        {

            if (item.RosterDuty1Enabled.IsTrue() && item.RosterDuty1StartTime.HasValue)
            {
                item.RosterStartDate = date.Add(item.RosterDuty1StartTime.Value).AddHours(_otStart * -1);
            }
            if (item.RosterDuty3Enabled.IsTrue() && item.RosterDuty3EndTime.HasValue)
            {
                item.RosterEndDate = date.Add(item.RosterDuty3EndTime.Value).AddHours(_otEndTime);
                if (item.RosterDuty3FallsNextDay.IsTrue())
                {
                    item.RosterEndDate = item.RosterEndDate.Value.AddDays(1);
                }
            }
            else if (item.RosterDuty2Enabled.IsTrue() && item.RosterDuty2EndTime.HasValue)
            {
                item.RosterEndDate = date.Add(item.RosterDuty2EndTime.Value).AddHours(_otEndTime);
                if (item.RosterDuty2FallsNextDay.IsTrue())
                {
                    item.RosterEndDate = item.RosterEndDate.Value.AddDays(1);
                }
            }
            else if (item.RosterDuty1Enabled.IsTrue() && item.RosterDuty1EndTime.HasValue)
            {
                item.RosterEndDate = date.Add(item.RosterDuty1EndTime.Value).AddHours(_otEndTime);
                if (item.RosterDuty1FallsNextDay.IsTrue())
                {
                    item.RosterEndDate = item.RosterEndDate.Value.AddDays(1);
                }
            }
            else
            {
                item.RosterStartDate = date;
                item.RosterEndDate = date.AddDays(1);
            }
        }
        public async Task<AttendanceViewModel> GetAttendanceDetailsById(string attendanceId)
        {
            var result = await _taaQueryBusiness.GetAttendanceDetailsById(attendanceId);
            return result;
        }
        public async Task<CommandResult<AttendanceViewModel>> CreateOverrideAttendance(AttendanceViewModel viewModel, bool doCommit = true)
        {
            //var data = BusinessHelper.MapModel<AttendanceViewModel, TAA_Attendance>(viewModel);
            //var errorList = new List<KeyValuePair<string, string>>();
            //var validateName = IsCodeExists(viewModel);
            //if (!validateName.IsSuccess)
            //{
            //    errorList.AddRange(validateName.Messages);
            //}
            //if (errorList.Count() > 0)
            //{
            //    return CommandResult<AttendanceViewModel>.Instance(viewModel, false, errorList);
            //}
            //data.IsOverridden = true;
            //_repository.Create(data, false);
            //viewModel.Id = data.Id;

            //_repository.CreateOneToOneRelationship<TAA_Attendance, R_Attendance_User, ADM_User>(data.Id, new R_Attendance_User(), viewModel.UserId, false);
            //_repository.Commit();

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _repo.UserContext.UserId;
            noteTempModel.TemplateCode = "Attendance";
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            //notemodel.OwnerUserId = user;
            notemodel.StartDate = DateTime.Now;
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
            notemodel.Json = JsonConvert.SerializeObject(viewModel);
            var result = await _noteBusiness.ManageNote(notemodel);

            if (viewModel.Mode == "Override")
            {
                var serviceId = await CreateOverrideOTService(viewModel);
                viewModel.ServiceId = serviceId;
            }
            return CommandResult<AttendanceViewModel>.Instance(viewModel);

        }
        private async Task<string> CreateOverrideOTService(AttendanceViewModel model)
        {
            var serviceTemplate = new ServiceTemplateViewModel();
            serviceTemplate.ActiveUserId = _repo.UserContext.UserId;
            serviceTemplate.TemplateCode = "OVERRIDE_OT_HOURS";
            var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);
            service.ServiceSubject = string.Concat("Override Attedance/OT/Deduction ");
            service.ServiceDescription = string.Concat("Override Attedance/OT/Deduction ");
            service.OwnerUserId = model.UserId;
            service.StartDate = System.DateTime.Today;
            service.DueDate = System.DateTime.Today.AddDays(2);
            service.RequestedByUserId = _repo.UserContext.UserId;
            service.DataAction = DataActionEnum.Create;
            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
            //service.Json = "{}";
            //var overrideAttendanceLOV = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "AttendanceType" && x.Name == model.OverrideAttendance.ToString());
            //model.OverrideAttendanceId = overrideAttendanceLOV.Id;

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("UserId", model.UserId);
            ((IDictionary<String, Object>)exo).Add("OverrideAttendanceDate", model.AttendanceDate);
            ((IDictionary<String, Object>)exo).Add("SystemTotalHours", model.TotalHours);
            ((IDictionary<String, Object>)exo).Add("AttendanceTypeId", model.SystemAttendance);
            ((IDictionary<String, Object>)exo).Add("SystemOTHours", model.SystemOTHours);
            ((IDictionary<String, Object>)exo).Add("SystemDeductionHours", model.SystemDeductionHours);
            ((IDictionary<String, Object>)exo).Add("OverrideAttendanceId", model.OverrideAttendanceId);
            ((IDictionary<String, Object>)exo).Add("OverrideOTHours", model.OverrideOTHours);
            ((IDictionary<String, Object>)exo).Add("OverrideDeductionHours", model.OverrideDeductionHours);
            ((IDictionary<String, Object>)exo).Add("OverrideComments", model.OverrideComments);
            ((IDictionary<String, Object>)exo).Add("ParentId", model.Id);

            service.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            var result = await _serviceBusiness.ManageService(service);
            if (result.IsSuccess)
            {
                return result.Item.ServiceId;
            }
            return string.Empty;
        }
        public async Task<CommandResult<AttendanceViewModel>> CorrectOverrideAttendance(AttendanceViewModel viewModel, bool doCommit = true)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _repo.UserContext.UserId;
            noteTempModel.NoteId = viewModel.NtsNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            //var overrideAttendanceLOV = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "AttendanceType" && x.Name == viewModel.OverrideAttendance.ToString());
            //viewModel.OverrideAttendanceId = overrideAttendanceLOV.Id;
            notemodel.Json = JsonConvert.SerializeObject(viewModel);
            var noteresult = await _noteBusiness.ManageNote(notemodel);

            if (viewModel.Mode == "Override")
            {
                var serviceId = await CreateOverrideOTService(viewModel);
                viewModel.ServiceId = serviceId;
            }
            return CommandResult<AttendanceViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<AttendanceViewModel>> Delete(AttendanceViewModel viewModel, bool doCommit = true)
        {
            return CommandResult<AttendanceViewModel>.Instance(viewModel, false, "Error");
        }
        public async Task<List<AttendanceToPayrollViewModel>> GetAtendanceListForPostPayroll(string payrollGroupId, int startMonth, int year, string orgId)
        {
            //var pay = _payrollGroupBusiness.GetSingleById(payrollGroupId);
            var pay = await _payrollBatchBusiness.GetPayrollGroupById(payrollGroupId);

            var start = pay.CutOffStartDay;
            var end = pay.CutOffEndDay;
            var endmonth = startMonth;

            if (pay.IsCutOffStartDayPreviousMonth)
            {
                startMonth = startMonth - 1;
            }

            //var date = DateTime.Now.ApplicationNow().Date;


            var first = new DateTime(year, startMonth, start);
            var last = new DateTime(year, endmonth, end);

            var d1 = first;
            var d2 = first.AddDays(1);
            var d3 = first.AddDays(2);
            var d4 = first.AddDays(3);
            var d5 = first.AddDays(4);
            var d6 = first.AddDays(5);
            var d7 = first.AddDays(6);
            var d8 = first.AddDays(7);
            var d9 = first.AddDays(8);
            var d10 = first.AddDays(9);
            var d11 = first.AddDays(10);
            var d12 = first.AddDays(11);
            var d13 = first.AddDays(12);
            var d14 = first.AddDays(13);
            var d15 = first.AddDays(14);
            var d16 = first.AddDays(15);
            var d17 = first.AddDays(16);
            var d18 = first.AddDays(17);
            var d19 = first.AddDays(18);
            var d20 = first.AddDays(19);
            var d21 = first.AddDays(20);
            var d22 = first.AddDays(21);
            var d23 = first.AddDays(22);
            var d24 = first.AddDays(23);
            var d25 = first.AddDays(24);
            var d26 = first.AddDays(25);
            var d27 = first.AddDays(26);
            var d28 = first.AddDays(27);
            var d29 = first.AddDays(28);
            var d30 = first.AddDays(29);
            var d31 = first.AddDays(30);

            var lastDay = DateTime.DaysInMonth(year, startMonth);
            var condition = "";
            var condition1 = "";
            if (lastDay == 29)
            {
                condition = " or rs29.PayrollPostedStatus ";
                condition1 = " or rs29.Id is not null ";
            }
            else if (lastDay == 30)
            {
                condition = " or rs29.PayrollPostedStatus is null or rs30.PayrollPostedStatus is null ";
                condition1 = " or rs29.Id is not null or rs30.Id is not null ";
            }
            else if (lastDay == 31)
            {
                condition = " or rs29.PayrollPostedStatus is null or rs30.PayrollPostedStatus is null or rs31.PayrollPostedStatus is null ";
                condition1 = " or rs29.Id is not null or rs30.Id is not null or rs31.Id is not null ";
            }

            //cypher = cypher.Replace("#CONDITION#", condition);
            //cypher = cypher.Replace("#CONDITION1#", condition1);


            var result = await _taaQueryBusiness.GetUserDetailsList(orgId);


            var result1 = await _taaQueryBusiness.GetAttendanceDetails(orgId, first, last);


            //var users = string.Join(",", result.Select(x => x.UserId));

            string users = null;
            var userIds = result.Select(x => x.UserId).Distinct().ToList();
            foreach (var j in userIds)
            {
                users += $"'{j}',";
            }
            if (users.IsNotNullAndNotEmpty())
            {
                users = users.Trim(',');
            }
            

            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Leave" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);

            var leave = await _taaQueryBusiness.LeaveQuery(templateList, users);

            foreach (var item in result)
            {
                var attendance = result1.Where(x => x.UserId == item.UserId).ToList();
                var leaves = leave.Where(x => x.UserId == item.UserId).ToList();
                var checkflag = false;
                TimeSpan total = new TimeSpan(00, 00, 00);
                TimeSpan dedtotal = new TimeSpan(00, 00, 00);
                var present = 0;
                var absent = 0;

                foreach (var item1 in attendance)
                {
                    if (item1.AttendanceDate == d1)
                    {
                        item.Day1 = item1.Day1;
                        item.P1 = item1.P1;
                        item.Id1 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d2)
                    {
                        item.Day2 = item1.Day1;
                        item.P2 = item1.P1;
                        item.Id2 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d3)
                    {
                        item.Day3 = item1.Day1;
                        item.P3 = item1.P1;
                        item.Id3 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d4)
                    {
                        item.Day4 = item1.Day1;
                        item.P4 = item1.P1;
                        item.Id4 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d5)
                    {
                        item.Day5 = item1.Day1;
                        item.P5 = item1.P1;
                        item.Id5 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d6)
                    {
                        item.Day6 = item1.Day1;
                        item.P6 = item1.P1;
                        item.Id6 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d7)
                    {
                        item.Day7 = item1.Day1;
                        item.P7 = item1.P1;
                        item.Id7 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d8)
                    {
                        item.Day8 = item1.Day1;
                        item.P8 = item1.P1;
                        item.Id8 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d9)
                    {
                        item.Day9 = item1.Day1;
                        item.P9 = item1.P1;
                        item.Id9 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d10)
                    {
                        item.Day10 = item1.Day1;
                        item.P10 = item1.P1;
                        item.Id10 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d11)
                    {
                        item.Day11 = item1.Day1;
                        item.P11 = item1.P1;
                        item.Id11 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d12)
                    {
                        item.Day12 = item1.Day1;
                        item.P12 = item1.P1;
                        item.Id12 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d13)
                    {
                        item.Day13 = item1.Day1;
                        item.P13 = item1.P1;
                        item.Id13 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d14)
                    {
                        item.Day14 = item1.Day1;
                        item.P14 = item1.P1;
                        item.Id14 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d15)
                    {
                        item.Day15 = item1.Day1;
                        item.P15 = item1.P1;
                        item.Id16 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d16)
                    {
                        item.Day16 = item1.Day1;
                        item.P16 = item1.P1;
                        item.Id16 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d17)
                    {
                        item.Day17 = item1.Day1;
                        item.P17 = item1.P1;
                        item.Id17 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d18)
                    {
                        item.Day18 = item1.Day1;
                        item.P18 = item1.P1;
                        item.Id18 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d19)
                    {
                        item.Day19 = item1.Day1;
                        item.P19 = item1.P1;
                        item.Id19 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d20)
                    {
                        item.Day20 = item1.Day1;
                        item.P20 = item1.P1;
                        item.Id20 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d21)
                    {
                        item.Day21 = item1.Day1;
                        item.P21 = item1.P1;
                        item.Id21 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d22)
                    {
                        item.Day22 = item1.Day1;
                        item.P22 = item1.P1;
                        item.Id22 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d23)
                    {
                        item.Day23 = item1.Day1;
                        item.P23 = item1.P1;
                        item.Id23 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d24)
                    {
                        item.Day24 = item1.Day1;
                        item.P24 = item1.P1;
                        item.Id24 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d25)
                    {
                        item.Day25 = item1.Day1;
                        item.P25 = item1.P1;
                        item.Id25 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d26)
                    {
                        item.Day26 = item1.Day1;
                        item.P26 = item1.P1;
                        item.Id26 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d27)
                    {
                        item.Day27 = item1.Day1;
                        item.P27 = item1.P1;
                        item.Id27 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d28)
                    {
                        item.Day28 = item1.Day1;
                        item.P28 = item1.P1;
                        item.Id28 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d29)
                    {
                        item.Day29 = item1.Day1;
                        item.P29 = item1.P1;
                        item.Id29 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d30)
                    {
                        item.Day30 = item1.Day1;
                        item.P30 = item1.P1;
                        item.Id30 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                    if (item1.AttendanceDate == d31)
                    {
                        item.Day31 = item1.Day1;
                        item.P31 = item1.P1;
                        item.Id31 = item1.Id;
                        if (item1.PayrollPostedStatus == null)
                            checkflag = true;
                        if (item1.Day1.StartsWith("P"))
                            present = present + 1;
                        if (item1.Day1.StartsWith("A"))
                            absent = absent + 1;
                        if (item1.OT1 != null)
                            total = total.Add(item1.OT1.Value);
                        if (item1.D1 != null)
                            dedtotal = dedtotal.Add(item1.D1.Value);
                    }
                }

                foreach (var lev in leaves)
                {
                    if (d1 >= lev.StartDate && d1 <= lev.EndDate)
                    {
                        item.Day1 = string.Concat(item.Day1, "-", lev.Leave);
                    }

                    if (d2 >= lev.StartDate && d2 <= lev.EndDate)
                    {
                        item.Day2 = string.Concat(item.Day2, "-", lev.Leave);
                    }

                    if (d3 >= lev.StartDate && d3 <= lev.EndDate)
                    {
                        item.Day3 = string.Concat(item.Day3, "-", lev.Leave);
                    }

                    if (d4 >= lev.StartDate && d4 <= lev.EndDate)
                    {
                        item.Day4 = string.Concat(item.Day4, "-", lev.Leave);
                    }

                    if (d5 >= lev.StartDate && d5 <= lev.EndDate)
                    {
                        item.Day5 = string.Concat(item.Day5, "-", lev.Leave);
                    }

                    if (d6 >= lev.StartDate && d6 <= lev.EndDate)
                    {
                        item.Day6 = string.Concat(item.Day6, "-", lev.Leave);
                    }

                    if (d7 >= lev.StartDate && d7 <= lev.EndDate)
                    {
                        item.Day7 = string.Concat(item.Day7, "-", lev.Leave);
                    }

                    if (d8 >= lev.StartDate && d8 <= lev.EndDate)
                    {
                        item.Day8 = string.Concat(item.Day8, "-", lev.Leave);
                    }

                    if (d9 >= lev.StartDate && d9 <= lev.EndDate)
                    {
                        item.Day9 = string.Concat(item.Day9, "-", lev.Leave);
                    }

                    if (d10 >= lev.StartDate && d10 <= lev.EndDate)
                    {
                        item.Day10 = string.Concat(item.Day10, "-", lev.Leave);
                    }

                    if (d11 >= lev.StartDate && d11 <= lev.EndDate)
                    {
                        item.Day11 = string.Concat(item.Day11, "-", lev.Leave);
                    }

                    if (d12 >= lev.StartDate && d12 <= lev.EndDate)
                    {
                        item.Day12 = string.Concat(item.Day12, "-", lev.Leave);
                    }

                    if (d13 >= lev.StartDate && d13 <= lev.EndDate)
                    {
                        item.Day13 = string.Concat(item.Day13, "-", lev.Leave);
                    }

                    if (d14 >= lev.StartDate & d14 <= lev.EndDate)
                    {
                        item.Day14 = string.Concat(item.Day14, "-", lev.Leave);
                    }

                    if (d15 >= lev.StartDate && d15 <= lev.EndDate)
                    {
                        item.Day15 = string.Concat(item.Day15, "-", lev.Leave);
                    }

                    if (d16 >= lev.StartDate && d16 <= lev.EndDate)
                    {
                        item.Day16 = string.Concat(item.Day16, "-", lev.Leave);
                    }

                    if (d17 >= lev.StartDate && d17 <= lev.EndDate)
                    {
                        item.Day17 = string.Concat(item.Day17, "-", lev.Leave);
                    }

                    if (d18 >= lev.StartDate && d18 <= lev.EndDate)
                    {
                        item.Day18 = string.Concat(item.Day18, "-", lev.Leave);
                    }

                    if (d19 >= lev.StartDate && d19 <= lev.EndDate)
                    {
                        item.Day19 = string.Concat(item.Day19, "-", lev.Leave);
                    }

                    if (d20 >= lev.StartDate && d20 <= lev.EndDate)
                    {
                        item.Day20 = string.Concat(item.Day20, "-", lev.Leave);
                    }
                    if (d21 >= lev.StartDate && d21 <= lev.EndDate)
                    {
                        item.Day21 = string.Concat(item.Day21, "-", lev.Leave);
                    }

                    if (d22 >= lev.StartDate && d22 <= lev.EndDate)
                    {
                        item.Day22 = string.Concat(item.Day22, "-", lev.Leave);
                    }

                    if (d23 >= lev.StartDate && d23 <= lev.EndDate)
                    {
                        item.Day23 = string.Concat(item.Day23, "-", lev.Leave);
                    }
                    if (d24 >= lev.StartDate && d24 <= lev.EndDate)
                    {
                        item.Day24 = string.Concat(item.Day24, "-", lev.Leave);
                    }
                    if (d25 >= lev.StartDate && d25 <= lev.EndDate)
                    {
                        item.Day25 = string.Concat(item.Day25, "-", lev.Leave);
                    }
                    if (d26 >= lev.StartDate && d26 <= lev.EndDate)
                    {
                        item.Day26 = string.Concat(item.Day26, "-", lev.Leave);
                    }
                    if (d27 >= lev.StartDate && d27 <= lev.EndDate)
                    {
                        item.Day27 = string.Concat(item.Day27, "-", lev.Leave);
                    }
                    if (d28 >= lev.StartDate && d28 <= lev.EndDate)
                    {
                        item.Day28 = string.Concat(item.Day28, "-", lev.Leave);
                    }
                    if (d29 >= lev.StartDate && d29 <= lev.EndDate)
                    {
                        item.Day29 = string.Concat(item.Day29, "-", lev.Leave);
                    }
                    if (d30 >= lev.StartDate && d30 <= lev.EndDate)
                    {
                        item.Day30 = string.Concat(item.Day30, "-", lev.Leave);
                    }
                    if (d31 >= lev.StartDate && d31 <= lev.EndDate)
                    {
                        item.Day31 = string.Concat(item.Day31, "-", lev.Leave);
                    }
                }
                item.TotalPresent = string.Concat("P :", present);
                item.TotalAbsent = string.Concat("A :", absent);
                item.TotalOT = string.Concat("OT :", (total.Days * 24) + total.Hours, ":", total.Minutes);
                item.TotalDED = string.Concat("Ded :", (dedtotal.Days * 24) + dedtotal.Hours, ":", dedtotal.Minutes);

                item.CheckFlag = checkflag;

                item.A1 = d1;
                item.A2 = d2;
                item.A3 = d3;
                item.A4 = d4;
                item.A5 = d5;
                item.A6 = d6;
                item.A7 = d7;
                item.A8 = d8;
                item.A9 = d9;
                item.A10 = d10;
                item.A11 = d11;
                item.A12 = d12;
                item.A13 = d13;
                item.A14 = d14;
                item.A15 = d15;
                item.A16 = d16;
                item.A17 = d17;
                item.A18 = d18;
                item.A19 = d19;
                item.A20 = d20;
                item.A21 = d21;
                item.A22 = d22;
                item.A23 = d23;
                item.A24 = d24;
                item.A25 = d25;
                item.A26 = d26;
                item.A27 = d27;
                item.A28 = d28;
                item.A29 = d29;
                item.A30 = d30;
                item.A31 = d31;
            }
            return result;
        }
        public async Task UpdateOTPayTransToProcessed(DateTime payRollDate, string payRollRunId)
        {
            //var cypher = @"match (n:TAA_Attendance) where datetime(n.AttendanceDate) <= datetime({date}) and n.PayrollPostedStatus = 'Submitted' with n match (n)-[:R_Attendance_User]-(u:ADM_User)
            //match(u)-[:R_User_PersonRoot]-(pr:HRS_PersonRoot{ IsDeleted: 0,Status: {Status},CompanyId: {CompanyId}})-[:R_PersonRoot]-(p:HRS_Person)-[:R_Person_Nationality]->(nation:HRS_Nationality) where datetime(p.EffectiveStartDate) <= datetime({date}) <= datetime(p.EffectiveEndDate)
            //match(pr)<-[:R_PayrollRun_PersonRoot]-(payR:PAY_PayrollRun{Id:{payRollRunId}})
            //optional match (n)<-[:R_Service_Reference{ReferenceTypeCode:'TAA_Attendance'}]-(s:NTS_Service)-[:R_Service_Status_ListOfValue]->(g:GEN_ListOfValue)
            //with n,u, g, s, pr, nation
            //where (g.Code in ['COMPLETED', 'CANCELED'] or g.Code is null)
            //with n,u,g,s,pr, nation
            //match (pr)<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot)
            // match(psr)<-[psrr:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo) where datetime(ps.EffectiveStartDate) <= datetime({date}) <= datetime(ps.EffectiveEndDate)
            // match(ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{ IsDeleted: 0,CompanyId: 1 })<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: 1 })
            //where e.Code = 'BASIC' and datetime(e.EffectiveStartDate) <= datetime(n.AttendanceDate) <= datetime(e.EffectiveEndDate)
            //set n.PayrollPostedStatus = 'Posted' return 1";
            //await _queryRepo.ExecuteCommand(cypher, null);


            var querydata = await _taaQueryBusiness.GetAttendancebyIdandDate(payRollDate,payRollRunId);
            if (querydata!=null)
            {
                DateTime todayDate = DateTime.Now;
                var payrollPostedStatus = await _lovBusiness.GetSingle(x => x.LOVType == "PayrollPostedStatus" && x.Code == "POSTED");
                
                var querydataId = querydata.Id;
                var payrollPostedStatusId = payrollPostedStatus.Id;
                await _taaQueryBusiness.UpdateAttendance(querydataId, payrollPostedStatusId);
            }

        }

        public Task<CommandResult<AttendanceViewModel>> UpdateComment(AttendanceViewModel viewModel, bool doCommit = true)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<AttendanceViewModel>> UpdateApproved(AttendanceViewModel viewModel, bool doCommit = true)
        {
            throw new NotImplementedException();
        }

        public async Task<CommandResult<AttendanceViewModel>> CreateAttendanceFromBiometrics(AttendanceViewModel viewModel, bool doCommit = true)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = viewModel.NtsNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel);
            var result = await _noteBusiness.ManageNote(notemodel);
            return CommandResult<AttendanceViewModel>.Instance(viewModel, result.Messages);
        }

        public async Task<CommandResult<AttendanceViewModel>> CorrectAttendanceFromBiometrics(AttendanceViewModel viewModel, bool doCommit = true)
        {
            try
            {
                //var data = BusinessHelper.MapModel<AttendanceViewModel, TAA_Attendance>(viewModel);
                //data.Duty1StartTime = data.Duty1StartTime.IgnoreSeconds();
                //data.Duty1EndTime = data.Duty1EndTime.IgnoreSeconds();
                //data.Duty2StartTime = data.Duty2StartTime.IgnoreSeconds();
                //data.Duty2EndTime = data.Duty2EndTime.IgnoreSeconds();
                //data.Duty3StartTime = data.Duty3StartTime.IgnoreSeconds();
                //data.Duty3EndTime = data.Duty3EndTime.IgnoreSeconds();

                //_repository.Edit(data);
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = viewModel.NtsNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);              
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel);
                var result = await _noteBusiness.ManageNote(notemodel);
                return CommandResult<AttendanceViewModel>.Instance(viewModel,result.Messages);
            }
            catch (Exception ex)
            {
                //Log.Instance.Error(ex, "Error on CorrectAttendanceFromBiometrics");
                throw;
            }
        }

        public Task<List<AttendanceViewModel>> GetOTPayTransactionList(DateTime attendanceStartDate, DateTime attendanceEndDate, string payRollRunId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPersonAbsentForMonth(string personId, DateTime payroll)
        {
            throw new NotImplementedException();
        }

        public Task<long> NoOfDaysAbsentForPayMonth(string personId, DateTime payroll)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<AttendanceViewModel>> CreateAttendanceFromService(AttendanceViewModel viewModel, bool doCommit = true)
        {
            throw new NotImplementedException();
        }

        public async Task<string> PostAttendanceToPayroll(string personIds, DateTime startDate, DateTime endDate)
        {
            try
            {
                var anyPending = await CheckAnyPendingAttendanceApproval(personIds, startDate, endDate);
                if (anyPending != "")
                {
                    return anyPending;
                }
                else
                {
                    var result = await _taaQueryBusiness.GetPostAttendanceToPayrollDetails(personIds, startDate, endDate);
                    if (result!=null)
                    {
                        var attdIds = result.Select(x => x.Id).Distinct().ToList();
                        string attendanceIds = null;
                        foreach (var j in attdIds)
                        {
                            if (j.IsNotNullAndNotEmpty())
                            {
                                attendanceIds += $"'{j}',";
                            }
                        }
                        attendanceIds = attendanceIds.Trim(',');

                        var updateresult = await _taaQueryBusiness.UpdateAttendanceAfterApproval(attendanceIds);
                        return "Success";
                    }

                }
            }
            catch (Exception e)
            {
                return "Error occured during Payroll Posting, Kindly contact administrator";
            }
            return "Success";
        }
        private async Task<string> CheckAnyPendingAttendanceApproval(string personIds, DateTime startDate, DateTime endDate)
        {
            string users = null;
            var userIds = personIds.Split(","); // result.Select(x => x.UserId).Distinct().ToList();
            foreach (var j in userIds)
            {
                if (j.IsNotNullAndNotEmpty())
                {
                    users += $"'{j}',";
                }
            }
            users = users.Trim(',');

            var result = await _taaQueryBusiness.CheckAnyPendingAttendanceApproval(personIds, startDate, endDate, users);
            return result.Any() ? string.Join("<br/>", result) : "";
        } 
        public async Task<List<AttendanceViewModel>> GetAttendanceListByDate(DateTime attendanceDate)
        {
            var result = await _taaQueryBusiness.GetAttendanceListByDate(attendanceDate);
            return result;
        }

        public async Task<AttendanceViewModel> GetTotalOtAndDeduction(string userId, DateTime startDate, DateTime endDate)
        {
            //throw new NotImplementedException();
            //var cypher = @"match(n:TAA_Attendance{IsCalculated :true,PayrollPostedStatus:'Posted'}) 
            //-[R_Attendance_User]->(u:ADM_User{Id:{UserId}})    
            //where  {StartDate}<=n.AttendanceDate<={EndDate} 
            //return coalesce(n.OverrideOTHours,n.SystemOTHours) as OTHours,coalesce(n.OverrideDeductionHours,n.SystemDeductionHours) as DeductionHours";
            //var prms = new Dictionary<string, object>
            //{
            //    { "UserId", userId },
            //    { "StartDate", startDate },
            //    { "EndDate", endDate }
            //};
            //var list = ExecuteCypherList<AttendanceViewModel>(cypher, prms);

            var list = await _taaQueryBusiness.GetTotalOtAndDeduction(userId,startDate,endDate);

            var ot = TimeSpan.Zero;
            var deduction = TimeSpan.Zero;
            foreach (var item in list)
            {
                if (item.OTHours != null)
                {
                    ot += item.OTHours.Value;
                }
                if (item.DeductionHours != null)
                {
                    deduction += item.DeductionHours.Value;
                }
            }
            return new AttendanceViewModel { OTHours = ot, DeductionHours = deduction };
        }

        public Task<TimeSpan?> getUnderTimeHours(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAttendanceTable(DateTime date, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<AttendanceViewModel>> ManageAttendance(AttendanceViewModel viewModel, bool doCommit = true)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<AttendanceViewModel>> ManageAttendanceDelete(AttendanceViewModel viewModel, bool doCommit = true)
        {
            throw new NotImplementedException();
        }


        

        public async Task<IList<TimeinTimeoutDetailsViewModel>> GetTimeinTimeOutDetails(string Empid, DateTime? Datefrom, DateTime? DateTo, DateTime MonthSearch, string Type)
        {
            var queryData = await _taaQueryBusiness.GetTimeinTimeOutDetails(Empid,Datefrom,DateTo,MonthSearch,Type);
            return queryData;
        }


        public async Task<IList<TimePermissionAttendanceViewModel>> GetReportTimePermissionList()
        {   
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "TimePermission" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);


            var querydata = await _taaQueryBusiness.GetReportTimePermissionList(templateList);

            return querydata;
        }
        public async  Task<IList<EmployeeServiceViewModel>> GetReportBusinessTripList()
        {
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Employee_Service" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);

            var querydata = await _taaQueryBusiness.GetReportBusinessTripList(templateList);
            return querydata;
        }
        public async Task UpdateAttendanceTable(DateTime? date)
        {
           
            try
            {
                var endTime = DateTime.Today;
                var distinctRosterList =await _rosterScheduleBusiness.GetDistinctNotcalculatedRosterDateList(endTime);
                //var distinctRosterList = new List<DateTime> { date.Value };
                while (distinctRosterList.Any())
                {
                    var startDate = distinctRosterList.FirstOrDefault();
                     TodaysAttendanceList = await GetAttendanceListByDate(startDate);
                    //Log.Instance.Info(DelimeterEnum.Space, "Starting update Roster Attendance", startDate.ToDefaultDateFormat());
                    await UpdateAttendanceWithRoster(startDate);
                    //Log.Instance.Info(DelimeterEnum.Space, "Ending update Roster Attendance", startDate.ToDefaultDateFormat());
                    distinctRosterList.Remove(startDate);
                }
                var lasttSuccessfulTime = date ?? DateTime.Today;
                await CloseOldRosters(lasttSuccessfulTime);


                lasttSuccessfulTime = lasttSuccessfulTime.AddDays(-2).Date;
                var employeeList =await GetAllEmployeesForAttendance();
                while (endTime >= lasttSuccessfulTime)
                {
                   // Log.Instance.Info(DelimeterEnum.Space, "Update Attendance", endTime.ToDefaultDateFormat());
                    TodaysAttendanceList =await GetAttendanceListByDate(endTime);
                    await UpdateAttendanceWithoutRoster(endTime, employeeList);
                    endTime = endTime.AddDays(-1);
                }
            }
            catch (Exception ex)
            {
               // Log.Instance.Error(ex, "UpdateAttendanceTable Error");
                throw;
            }
        }
        private async Task UpdateAttendanceWithRoster(DateTime rosterDate, string userId = null, bool allowFutureDated = false)
        {
            var accessStart = rosterDate.AddDays(-3);
            var accessEnd = rosterDate.AddDays(3);
            //var accessLogEntries = _repoAccessLog.GetList(x => x.PunchingTime >= accessStart && x.PunchingTime <= accessEnd).ToList();
            var accessLogEntries = await GetAccessLogDetail(accessEnd, accessStart);
            var rosterList = await _rosterScheduleBusiness.GetPublishedRostersForAttendance(rosterDate);
            if (userId.IsNotNullAndNotEmpty())
            {
                rosterList = rosterList.Where(x => x.UserId == userId).ToList();
            }

            foreach (var roster in rosterList)
            {
                if (IsRosterRunning(roster) && !allowFutureDated)
                {
                    continue;
                }
                var userAccessLogList = accessLogEntries.Where(x => x.BiometricId == roster.BiometricId).ToList();

                if (!userAccessLogList.Any(x => x.PunchingTime >= rosterDate && x.PunchingTime < rosterDate.AddDays(1)))
                {
                    await MarkAbsentWithRoster(roster, userAccessLogList);
                }
                else
                {
                   await MarkPresentWithRoster(roster, userAccessLogList);
                }
            }
        }
        private async Task UpdateAttendanceWithoutRoster(DateTime attendanceDate, List<SalaryInfoViewModel> employeeList)
        {
            var yestreday = attendanceDate.AddDays(-1);
            var accessStart = attendanceDate.AddDays(-3);
            var accessEnd = attendanceDate.AddDays(3);
            // var accessLogEntries = _repoAccessLog.GetList(x => x.UserId != null && x.PunchingTime >= accessStart && x.PunchingTime <= accessEnd).ToList();
            var accessLogEntries = await GetAccessLogDetail(accessEnd, accessStart);

             var rosterList =await _rosterScheduleBusiness.GetPublishedRostersList(attendanceDate);
            var yesterdaysRosterList =await _rosterScheduleBusiness.GetPublishedRostersList(yestreday);
            foreach (var employee in employeeList)
            {
                string userId = employee.UserId;
                if (rosterList.Any(x => x.UserId == userId))
                {
                    continue;
                }
                var yesterdaysRoster = yesterdaysRosterList.FirstOrDefault(x => x.UserId == userId);
                DateTime? yesterdaysOverlappingRosterEndTime = null;
                var userLogEntries = accessLogEntries.Where(x => x.UserId == userId).ToList();
                if (userLogEntries.Count == 0)
                {
                    if (employee.TakeAttendanceFromTAA)
                    {
                        await MarkAbsentWithoutRoster(employee.UserId, attendanceDate);
                    }
                }
                else
                {
                   await SaveFullDayAttendanceWithoutRoster(attendanceDate, userId, userLogEntries, yesterdaysOverlappingRosterEndTime);
                }

            }


        }
        private bool IsRosterRunning(RosterScheduleViewModel roster)
        {            
        var datetTime = DateTime.Now;
            if (roster.Duty1Enabled && roster.Duty1StartTime.HasValue)
            {
                var startDate = roster.RosterDate.Add(roster.Duty1StartTime.Value);
                var isStarted = startDate.AddHours(_otStart * -1) > datetTime;
                if (isStarted)
                {
                    return isStarted;
                }
            }
            if (roster.Duty3Enabled && roster.Duty3EndTime.HasValue)
            {
                var endDate = roster.RosterDate.Add(roster.Duty3EndTime.Value);
                if (roster.Duty3FallsNextDay)
                {
                    endDate = endDate.AddDays(1);
                }
                return endDate.AddHours(_otEndTime) > datetTime;
            }
            if (roster.Duty2Enabled && roster.Duty2EndTime.HasValue)
            {
                var endDate = roster.RosterDate.Add(roster.Duty2EndTime.Value);
                if (roster.Duty2FallsNextDay)
                {
                    endDate = endDate.AddDays(1);
                }
                return endDate.AddHours(_otEndTime) > datetTime;

            }
            if (roster.Duty1Enabled && roster.Duty1EndTime.HasValue)
            {
                var endDate = roster.RosterDate.Add(roster.Duty1EndTime.Value);
                if (roster.Duty1FallsNextDay)
                {
                    endDate = endDate.AddDays(1);
                }
                return endDate.AddHours(_otEndTime) > datetTime;
            }
            return false;
        }
        private async Task CloseOldRosters(DateTime date)
        {
            //date = date.AddDays(-10);
            //var cypher = $@"update cms.""N_TAA_RosterSchedule"" as r set r.""IsAttendanceCalculated""=true where r.""RosterDate""<'{date}'  ";
            
            //var cypher = @"match (r:TAA_RosterSchedule) where r.RosterDate<{Date}  set r.IsAttendanceCalculated=true";
            //ExecuteCypherWithoutResult(cypher, new Dictionary<string, object> { { "Date", date } });

            await _taaQueryBusiness.CloseOldRosters(date);
        }
        private async Task<List<SalaryInfoViewModel>> GetAllEmployeesForAttendance()
        {
            return await _taaQueryBusiness.GetAllEmployeesForAttendance();
        }

        private async Task<List<AccessLogViewModel>> GetAccessLogDetail(DateTime accessEnd,DateTime accessStart)
        {
            return await _taaQueryBusiness.GetAccessLogDetail(accessEnd, accessStart);
        }
        private async Task MarkAbsentWithRoster(RosterScheduleViewModel roster, List<AccessLogViewModel> accessLogEntries)
        {
            //var yesterdaysOverlappingRosterEndTime = UpdateYesterdaysOverlappingAttendance(roster.UserId.Value, roster.RosterDate, accessLogEntries);
            var todaysAttendance = GetTodaysAttendance(roster.UserId, roster.RosterDate);
            if (todaysAttendance.PayrollPostedStatus != null)
            {
                return;
            }
            todaysAttendance.SystemAttendance = AttendanceTypeEnum.Absent;
            todaysAttendance.OverrideAttendance = AttendanceTypeEnum.Absent;
            if (roster.RosterDutyType == RosterDutyTypeEnum.DayOff || await IsEmployeeOnPaidLeave(roster.UserId, roster.RosterDate))
            {
                todaysAttendance.SystemAttendance = AttendanceTypeEnum.Present;
                todaysAttendance.OverrideAttendance = AttendanceTypeEnum.Present;
            }
            if (todaysAttendance.DataAction == DataActionEnum.Create)
            {
                todaysAttendance.UserId = roster.UserId;
                todaysAttendance.AttendanceDate = roster.RosterDate;
                todaysAttendance.IsCalculated = true;
                await CreateAttendanceFromBiometrics(todaysAttendance);
            }
            else
            {
                todaysAttendance.SystemOTHours = null;
                todaysAttendance.SystemDeductionHours = null;
                todaysAttendance.Duty1Enabled = true;
                todaysAttendance.Duty1StartTime = null;
                todaysAttendance.Duty1EndTime = null;
                todaysAttendance.Duty1FallsNextDay = false;
                todaysAttendance.Duty1FallsPreviousDay = false;

                todaysAttendance.Duty2Enabled = false;
                todaysAttendance.Duty2StartTime = null;
                todaysAttendance.Duty2EndTime = null;
                todaysAttendance.Duty2FallsNextDay = false;
                todaysAttendance.Duty2FallsPreviousDay = false;

                todaysAttendance.Duty3Enabled = false;
                todaysAttendance.Duty3StartTime = null;
                todaysAttendance.Duty3EndTime = null;
                todaysAttendance.Duty3FallsNextDay = false;
                todaysAttendance.Duty3FallsPreviousDay = false;
                todaysAttendance.IsCalculated = true;
                todaysAttendance.TotalHours = null;
                await CorrectAttendanceFromBiometrics(todaysAttendance);
            }
            roster.IsAttendanceCalculated = true;
            await _noteBusiness.ManageNote(roster);
        }

        private async Task MarkAbsentWithoutRoster(string userId, DateTime date)
        {
            //var yesterdaysOverlappingRosterEndTime = UpdateYesterdaysOverlappingAttendance(roster.UserId.Value, roster.RosterDate, accessLogEntries);
            var todaysAttendance = GetTodaysAttendance(userId, date);
            if (todaysAttendance.PayrollPostedStatus != null)
            {
                return;
            }
            todaysAttendance.SystemAttendance = AttendanceTypeEnum.Absent;

            if (todaysAttendance.AttendanceDate == DateTime.Today)
            {
                todaysAttendance.SystemAttendance = null;
                todaysAttendance.SystemOTHours = null;
                todaysAttendance.SystemDeductionHours = null;
            }           
            if (await IsEmployeeOnPaidLeave(userId, date))
            {
                todaysAttendance.SystemAttendance = AttendanceTypeEnum.Present;
            }
            if (todaysAttendance.DataAction == DataActionEnum.Create)
            {
                todaysAttendance.UserId = userId;
                todaysAttendance.AttendanceDate = date;
                todaysAttendance.IsCalculated = true;
                await CreateAttendanceFromBiometrics(todaysAttendance);
            }
            else
            {
                todaysAttendance.SystemOTHours = null;
                todaysAttendance.Duty1Enabled = true;
                todaysAttendance.Duty1StartTime = null;
                todaysAttendance.Duty1EndTime = null;
                todaysAttendance.Duty1FallsNextDay = false;

                todaysAttendance.Duty2Enabled = false;
                todaysAttendance.Duty2StartTime = null;
                todaysAttendance.Duty2EndTime = null;
                todaysAttendance.Duty2FallsNextDay = false;

                todaysAttendance.Duty3Enabled = false;
                todaysAttendance.Duty3StartTime = null;
                todaysAttendance.Duty3EndTime = null;
                todaysAttendance.Duty3FallsNextDay = false;
                todaysAttendance.IsCalculated = true;
                await CorrectAttendanceFromBiometrics(todaysAttendance);
            }

            
        }
        private AttendanceViewModel GetTodaysAttendance(string userId, DateTime attendanceDate)
        {
            var attendance = TodaysAttendanceList.FirstOrDefault(x => x.UserId == userId);
            if (attendance == null)
            {
                return new AttendanceViewModel
                {
                    UserId = userId,
                    UserIds = userId.ToString(),
                    AttendanceDate = attendanceDate,
                    DataAction = DataActionEnum.Create,
                    IsDeleted = false,
                    CompanyId = _repo.UserContext.CompanyId,
                    Status = StatusEnum.Active,
                    CreatedBy = _repo.UserContext.UserId,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = _repo.UserContext.UserId,
                    LastUpdatedDate = DateTime.Now

                };
            }
            else
            {
                attendance.DataAction = DataActionEnum.Edit;
                attendance.LastUpdatedBy = _repo.UserContext.UserId;
                attendance.LastUpdatedDate = DateTime.Now;
                return attendance;
            }

        }
        private async Task<bool> IsEmployeeOnPaidLeave(string userId, DateTime? asOfDate)
        {
            if (userId.IsNotNullAndNotEmpty() && asOfDate.HasValue)
            {
                var _lbb = _sp.GetService<ILeaveBalanceSheetBusiness>();
                return await _lbb.IsOnPaidLeave(userId, asOfDate.Value);
            }
            return false;
        }
        private async Task MarkPresentWithRoster(RosterScheduleViewModel roster, List<AccessLogViewModel> accessLogEntries)
        {
            string userId = roster.UserId;
            DateTime date = roster.RosterDate;
            //   var yesterdaysOverlappingRosterEndTime = UpdateYesterdaysOverlappingAttendance(userId, date, accessLogEntries);
            if (roster.RosterDutyType == RosterDutyTypeEnum.DayOff)
            {
                await SaveDutyOffAttendanceWithRoster(roster, accessLogEntries, null);
            }
            else if (roster.Duty1Enabled && !roster.Duty1FallsNextDay && !roster.Duty2Enabled && !roster.Duty3Enabled)
            {

               await SaveFullDayAttendanceWithRoster(roster, accessLogEntries, null);
            }

            else
            {
               await SaveSplitAttendanceWithRoster(roster, accessLogEntries, null);
            }

        }
        private async Task SaveSplitAttendanceWithRoster(RosterScheduleViewModel roster, List<AccessLogViewModel> accessLogEntries, DateTime? minimumStartTime)
        {
            var todaysAttendance = GetTodaysAttendance(roster.UserId, roster.RosterDate);
            if (todaysAttendance.PayrollPostedStatus != null)
            {
                return;
            }
            todaysAttendance.Duty1Enabled = false;
            todaysAttendance.Duty1StartTime = null;
            todaysAttendance.Duty1EndTime = null;
            todaysAttendance.Duty1FallsPreviousDay = false;
            todaysAttendance.Duty1FallsNextDay = false;

            todaysAttendance.Duty2Enabled = false;
            todaysAttendance.Duty2StartTime = null;
            todaysAttendance.Duty2EndTime = null;
            todaysAttendance.Duty2FallsPreviousDay = false;
            todaysAttendance.Duty2FallsNextDay = false;

            todaysAttendance.Duty3Enabled = false;
            todaysAttendance.Duty3StartTime = null;
            todaysAttendance.Duty3EndTime = null;
            todaysAttendance.Duty3FallsPreviousDay = false;
            todaysAttendance.Duty3FallsNextDay = false;

            if (roster.Duty1Enabled && roster.Duty1StartTime.HasValue && roster.Duty1EndTime.HasValue)
            {
                var roster1Start = roster.RosterDate.Add(roster.Duty1StartTime.Value);
                var roster1End = roster.RosterDate.Add(roster.Duty1EndTime.Value);
                if (roster.Duty1FallsNextDay)
                {
                    roster1End = roster1End.AddDays(1);
                }
                minimumStartTime = minimumStartTime ?? roster1Start;
                var attendance1 = GetFirstCheckinLastCheckOut(minimumStartTime.Value, roster1End, accessLogEntries, roster, 1);
                if (attendance1 != null)
                {
                    todaysAttendance.Duty1Enabled = true;
                    todaysAttendance.Duty1StartTime = attendance1.Item1.TimeOfDay;
                    todaysAttendance.Duty1EndTime = attendance1.Item2.TimeOfDay;
                    //if (attendance1.Item1.Date < attendance1.Item2.Date)
                    //{
                    //    todaysAttendance.Duty1FallsNextDay = true;
                    //}
                    if (todaysAttendance.AttendanceDate > attendance1.Item1)
                    {
                        todaysAttendance.Duty1FallsPreviousDay = true;
                    }
                    else
                    {
                        todaysAttendance.Duty1FallsPreviousDay = false;
                    }
                    if (todaysAttendance.AttendanceDate.AddDays(1) <= attendance1.Item2)
                    {
                        todaysAttendance.Duty1FallsNextDay = true;
                    }
                    else
                    {
                        todaysAttendance.Duty1FallsNextDay = false;
                    }
                }

            }
            if (roster.Duty2Enabled && roster.Duty2StartTime.HasValue && roster.Duty2EndTime.HasValue)
            {
                var roster2Start = roster.RosterDate.Add(roster.Duty2StartTime.Value);
                var roster2End = roster.RosterDate.Add(roster.Duty2EndTime.Value);
                if (roster.Duty2FallsNextDay)
                {
                    roster2End = roster2End.AddDays(1);
                }
                var attendance2 = GetFirstCheckinLastCheckOut(roster2Start, roster2End, accessLogEntries, roster, 2);
                if (attendance2 != null)
                {
                    todaysAttendance.Duty2Enabled = true;
                    todaysAttendance.Duty2StartTime = attendance2.Item1.TimeOfDay;
                    todaysAttendance.Duty2EndTime = attendance2.Item2.TimeOfDay;

                    if (todaysAttendance.AttendanceDate > attendance2.Item1)
                    {
                        todaysAttendance.Duty2FallsPreviousDay = true;
                    }
                    else
                    {
                        todaysAttendance.Duty2FallsPreviousDay = false;
                    }

                    if (todaysAttendance.AttendanceDate.AddDays(1) <= attendance2.Item2)
                    {
                        todaysAttendance.Duty2FallsNextDay = true;
                    }
                    else
                    {
                        todaysAttendance.Duty2FallsNextDay = false;
                    }
                }

            }
            if (roster.Duty3Enabled && roster.Duty3StartTime.HasValue && roster.Duty3EndTime.HasValue)
            {
                var roster3Start = roster.RosterDate.Add(roster.Duty3StartTime.Value);
                var roster3End = roster.RosterDate.Add(roster.Duty3EndTime.Value);
                if (roster.Duty3FallsNextDay)
                {
                    roster3End = roster3End.AddDays(1);
                }
                var attendance3 = GetFirstCheckinLastCheckOut(roster3Start, roster3End, accessLogEntries);
                if (attendance3 != null)
                {
                    todaysAttendance.Duty3Enabled = true;
                    todaysAttendance.Duty3StartTime = attendance3.Item1.TimeOfDay;
                    todaysAttendance.Duty3EndTime = attendance3.Item2.TimeOfDay;
                    if (todaysAttendance.AttendanceDate > attendance3.Item1)
                    {
                        todaysAttendance.Duty3FallsPreviousDay = true;
                    }
                    else
                    {
                        todaysAttendance.Duty3FallsPreviousDay = false;
                    }
                    if (todaysAttendance.AttendanceDate.AddDays(1) <= attendance3.Item2)
                    {
                        todaysAttendance.Duty3FallsNextDay = true;
                    }
                    else
                    {
                        todaysAttendance.Duty3FallsNextDay = false;
                    }
                }
            }
            todaysAttendance.Duty1StartTime = todaysAttendance.Duty1StartTime/*.IgnoreSeconds()*/;
            todaysAttendance.Duty1EndTime = todaysAttendance.Duty1EndTime/*.IgnoreSeconds()*/;
            todaysAttendance.Duty2StartTime = todaysAttendance.Duty2StartTime/*.IgnoreSeconds()*/;
            todaysAttendance.Duty2EndTime = todaysAttendance.Duty2EndTime/*.IgnoreSeconds()*/;
            todaysAttendance.Duty3StartTime = todaysAttendance.Duty3StartTime/*.IgnoreSeconds()*/;
            todaysAttendance.Duty3EndTime = todaysAttendance.Duty3EndTime/*.IgnoreSeconds()*/;

            await CalculateTotalHoursAndOTWithRoster(todaysAttendance, roster);
            todaysAttendance.IsCalculated = true;
            if (todaysAttendance.DataAction == DataActionEnum.Create)
            {
                await CreateAttendanceFromBiometrics(todaysAttendance);
            }
            else
            {
                await CorrectAttendanceFromBiometrics(todaysAttendance);
            }
            roster.IsAttendanceCalculated = true;
            await _rosterScheduleBusiness.Correct(roster);
        }
        private async Task SaveFullDayAttendanceWithoutRoster(DateTime attendanceDate, string userId, List<AccessLogViewModel> accessLogEntries, DateTime? minimumStartTime)
        {
            var attendanceStartDate = GetAttendanceStartDate(accessLogEntries, attendanceDate);
            var attendanceEndDate = GetAttendanceEndDate(accessLogEntries, attendanceDate);
            minimumStartTime = minimumStartTime ?? attendanceStartDate;
            var todaysAttendance = GetTodaysAttendance(userId, attendanceDate);
            if (todaysAttendance.PayrollPostedStatus != null)
            {
                return;
            }

            if (minimumStartTime != null && attendanceEndDate != null)
            {
                // var dayEvent = GetFirstCheckinLastCheckOut(minimumStartTime.Value, attendanceEndDate.Value, accessLogEntries);
                todaysAttendance.Duty1Enabled = true;
                todaysAttendance.Duty1StartTime = minimumStartTime.Value.TimeOfDay;
                todaysAttendance.Duty1EndTime = attendanceEndDate.Value.TimeOfDay;

            }
            todaysAttendance.Duty1FallsNextDay = false;
            todaysAttendance.Duty2Enabled = false;
            todaysAttendance.Duty2StartTime = null;
            todaysAttendance.Duty2EndTime = null;
            todaysAttendance.Duty2FallsNextDay = false;

            todaysAttendance.Duty3Enabled = false;
            todaysAttendance.Duty3StartTime = null;
            todaysAttendance.Duty3EndTime = null;
            todaysAttendance.Duty3FallsNextDay = false;
            todaysAttendance.IsCalculated = true;
            if (todaysAttendance.AttendanceDate == DateTime.Today)
            {
                todaysAttendance.SystemAttendance = null;
                todaysAttendance.SystemOTHours = null;
                todaysAttendance.SystemDeductionHours = null;
            }
            else
            {
                await CalculateTotalHoursAndOTWithoutRoster(todaysAttendance);
            }

            if (todaysAttendance.DataAction == DataActionEnum.Create)
            {
                await CreateAttendanceFromBiometrics(todaysAttendance);
            }
            else
            {
                await CorrectAttendanceFromBiometrics(todaysAttendance);
            }
        }
        private async Task SaveFullDayAttendanceWithRoster(RosterScheduleViewModel roster, List<AccessLogViewModel> accessLogEntries, DateTime? minimumStartTime)
        {
            //  minimumStartTime = minimumStartTime ?? roster.Duty1StartTime;
            var rosterStartDate = GetRosterStartDate(roster);
            var rosterEndDate = GetRosterEndDate(roster);
            minimumStartTime = minimumStartTime ?? rosterStartDate;
            var dayEvent = GetFirstCheckinLastCheckOut(minimumStartTime.Value, rosterEndDate, accessLogEntries);

            var todaysAttendance = GetTodaysAttendance(roster.UserId, roster.RosterDate);

            if (todaysAttendance.PayrollPostedStatus != null)
            {
                return;
            }
            if (dayEvent != null)
            {
                todaysAttendance.Duty1Enabled = true;
                todaysAttendance.Duty1StartTime = dayEvent.Item1.TimeOfDay;
                todaysAttendance.Duty1EndTime = dayEvent.Item2.TimeOfDay;
                if (todaysAttendance.AttendanceDate > dayEvent.Item1)
                {
                    todaysAttendance.Duty1FallsPreviousDay = true;
                }
                else
                {
                    todaysAttendance.Duty1FallsPreviousDay = false;
                }
                if (todaysAttendance.AttendanceDate.AddDays(1) <= dayEvent.Item2)
                {
                    todaysAttendance.Duty1FallsNextDay = true;
                }
                else
                {
                    todaysAttendance.Duty1FallsNextDay = false;
                }
            }

            todaysAttendance.Duty2Enabled = false;
            todaysAttendance.Duty2StartTime = null;
            todaysAttendance.Duty2EndTime = null;
            todaysAttendance.Duty2FallsPreviousDay = false;
            todaysAttendance.Duty2FallsNextDay = false;

            todaysAttendance.Duty3Enabled = false;
            todaysAttendance.Duty3StartTime = null;
            todaysAttendance.Duty3EndTime = null;
            todaysAttendance.Duty3FallsPreviousDay = false;
            todaysAttendance.Duty3FallsNextDay = false;
            todaysAttendance.IsCalculated = true;

            todaysAttendance.Duty1StartTime = todaysAttendance.Duty1StartTime/*.IgnoreSeconds()*/;
            todaysAttendance.Duty1EndTime = todaysAttendance.Duty1EndTime/*.IgnoreSeconds()*/;
            await CalculateTotalHoursAndOTWithRoster(todaysAttendance, roster);
            if (todaysAttendance.DataAction == DataActionEnum.Create)
            {
                await CreateAttendanceFromBiometrics(todaysAttendance);
            }
            else
            {
                await CorrectAttendanceFromBiometrics(todaysAttendance);
            }
            roster.IsAttendanceCalculated = true;
            await _rosterScheduleBusiness.Correct(roster);
        }

        private async Task SaveDutyOffAttendanceWithRoster(RosterScheduleViewModel roster, List<AccessLogViewModel> accessLogEntries, DateTime? minimumStartTime)
        {
            var attendanceStartDate = GetAttendanceStartDate(accessLogEntries, roster.RosterDate);
            var attendanceEndDate = GetAttendanceEndDate(accessLogEntries, roster.RosterDate);
            minimumStartTime = minimumStartTime ?? attendanceStartDate;
            var todaysAttendance = GetTodaysAttendance(roster.UserId, roster.RosterDate);
            if (todaysAttendance.PayrollPostedStatus != null)
            {
                return;
            }

            if (minimumStartTime != null && attendanceEndDate != null)
            {
                var dayEvent = new Tuple<DateTime, DateTime>(minimumStartTime.Value, attendanceEndDate.Value);
                todaysAttendance.Duty1Enabled = true;
                todaysAttendance.Duty1StartTime = dayEvent.Item1.TimeOfDay;
                todaysAttendance.Duty1EndTime = dayEvent.Item2.TimeOfDay;
                if (todaysAttendance.AttendanceDate > dayEvent.Item1)
                {
                    todaysAttendance.Duty1FallsPreviousDay = true;
                }
                else
                {
                    todaysAttendance.Duty1FallsPreviousDay = false;
                }
                if (todaysAttendance.AttendanceDate.AddDays(1) <= dayEvent.Item2)
                {
                    todaysAttendance.Duty1FallsNextDay = true;
                }
                else
                {
                    todaysAttendance.Duty1FallsNextDay = false;
                }
            }

            todaysAttendance.Duty2Enabled = false;
            todaysAttendance.Duty2StartTime = null;
            todaysAttendance.Duty2EndTime = null;
            todaysAttendance.Duty2FallsPreviousDay = false;
            todaysAttendance.Duty2FallsNextDay = false;

            todaysAttendance.Duty3Enabled = false;
            todaysAttendance.Duty3StartTime = null;
            todaysAttendance.Duty3EndTime = null;
            todaysAttendance.Duty3FallsPreviousDay = false;
            todaysAttendance.Duty3FallsNextDay = false;
            todaysAttendance.IsCalculated = true;

            todaysAttendance.Duty1StartTime = todaysAttendance.Duty1StartTime/*.IgnoreSeconds()*/;
            todaysAttendance.Duty1EndTime = todaysAttendance.Duty1EndTime/*.IgnoreSeconds()*/;
            await CalculateTotalHoursAndOTWithRoster(todaysAttendance, roster);
            if (todaysAttendance.DataAction == DataActionEnum.Create)
            {
                await CreateAttendanceFromBiometrics(todaysAttendance);
            }
            else
            {
                await CorrectAttendanceFromBiometrics(todaysAttendance);
            }
            roster.IsAttendanceCalculated = true;
            await _rosterScheduleBusiness.Correct(roster);
        }
        private DateTime GetRosterStartDate(RosterScheduleViewModel roster)
        {
            return roster.RosterDate.Add(roster.Duty1StartTime.Value);
        }
        private DateTime? GetAttendanceStartDate(List<AccessLogViewModel> accessLogEntries, DateTime attendanceDate)
        {
            var min = accessLogEntries.Where(x => x.PunchingTime >= attendanceDate && x.PunchingTime < attendanceDate.AddDays(1))
                .OrderBy(x => x.PunchingTime).FirstOrDefault();
            if (min != null)
            {
                return min.PunchingTime;
            }
            return null;
        }
        private DateTime? GetAttendanceEndDate(List<AccessLogViewModel> accessLogEntries, DateTime attendanceDate)
        {
            var max = accessLogEntries.Where(x => x.PunchingTime >= attendanceDate && x.PunchingTime < attendanceDate.AddDays(1))
                .OrderByDescending(x => x.PunchingTime).FirstOrDefault();
            if (max != null)
            {
                return max.PunchingTime;
            }
            return null;
        }
        private DateTime GetRosterEndDate(RosterScheduleViewModel roster)
        {
            if (roster.Duty3Enabled)
            {
                if (roster.Duty3FallsNextDay)
                {
                    return roster.RosterDate.AddDays(1).Add(roster.Duty3EndTime.Value);
                }
                return roster.RosterDate.Add(roster.Duty3EndTime.Value);
            }
            if (roster.Duty2Enabled)
            {
                if (roster.Duty2FallsNextDay)
                {
                    return roster.RosterDate.AddDays(1).Add(roster.Duty2EndTime.Value);
                }
                return roster.RosterDate.Add(roster.Duty2EndTime.Value);
            }
            if (roster.Duty1FallsNextDay)
            {
                return roster.RosterDate.AddDays(1).Add(roster.Duty1EndTime.Value);
            }
            return roster.RosterDate.Add(roster.Duty1EndTime.Value);
        }
        private async Task CalculateTotalHoursAndOTWithRoster(AttendanceViewModel todaysAttendance, RosterScheduleViewModel roster)
        {
            var totalHours = new TimeSpan();
            if (todaysAttendance.Duty1Enabled && todaysAttendance.Duty1StartTime.HasValue && todaysAttendance.Duty1EndTime.HasValue)
            {
                var start = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty1StartTime.Value);
                var end = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty1EndTime.Value);
                if (start != end)
                {
                    if (todaysAttendance.Duty1FallsPreviousDay.IsTrue())
                    {
                        start = start.AddDays(-1);
                    }
                    if (todaysAttendance.Duty1FallsNextDay)
                    {
                        end = end.AddDays(1);
                    }
                }
                //if (todaysAttendance.Duty1FallsNextDay)
                //{
                //    end = end.AddDays(1);
                //}

                totalHours = totalHours.Add(end - start);
            }
            if (todaysAttendance.Duty2Enabled && todaysAttendance.Duty2StartTime.HasValue && todaysAttendance.Duty2EndTime.HasValue)
            {
                var start = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty2StartTime.Value);
                var end = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty2EndTime.Value);
                if (start != end)
                {
                    if (todaysAttendance.Duty2FallsPreviousDay.IsTrue())
                    {
                        start = start.AddDays(-1);
                    }

                    if (todaysAttendance.Duty2FallsNextDay)
                    {
                        end = end.AddDays(1);
                    }
                }

                totalHours = totalHours.Add(end - start);
            }
            if (todaysAttendance.Duty3Enabled && todaysAttendance.Duty3StartTime.HasValue && todaysAttendance.Duty3EndTime.HasValue)
            {
                var start = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty3StartTime.Value);
                var end = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty3EndTime.Value);
                if (start != end)
                {
                    if (todaysAttendance.Duty3FallsPreviousDay.IsTrue())
                    {
                        start = start.AddDays(-1);
                    }

                    if (todaysAttendance.Duty3FallsNextDay)
                    {
                        end = end.AddDays(1);
                    }
                }

                totalHours = totalHours.Add(end - start);
            }
            todaysAttendance.TotalHours = totalHours;
            var isEmployeeOnPaidLeave = false;
            if (totalHours.TotalMinutes > 0 || roster.RosterDutyType == RosterDutyTypeEnum.DayOff)
            {
                todaysAttendance.SystemAttendance = AttendanceTypeEnum.Present;
            }
            else
            {
                isEmployeeOnPaidLeave =await IsEmployeeOnPaidLeave(roster.UserId, roster.RosterDate);
                if (isEmployeeOnPaidLeave)
                {
                    todaysAttendance.SystemAttendance = AttendanceTypeEnum.Present;
                }
                else
                {
                    todaysAttendance.SystemAttendance = AttendanceTypeEnum.Absent;
                }

            }
            if (totalHours.TotalMinutes > 0)
            {
                CalculateOTWithRoster(todaysAttendance, roster, isEmployeeOnPaidLeave);
                CalculateDeductionWithRoster(todaysAttendance, roster, isEmployeeOnPaidLeave);
            }
            else
            {
                todaysAttendance.SystemOTHours = TimeSpan.Zero;
                todaysAttendance.SystemDeductionHours = TimeSpan.Zero;
            }
        }
        private void CalculateOTWithRoster(AttendanceViewModel todaysAttendance, RosterScheduleViewModel roster, bool isEmployeeOnPaidLeave)
        {
            if (roster != null)
            {

                if (roster.RosterDutyType == RosterDutyTypeEnum.DayOff || isEmployeeOnPaidLeave)
                {
                    todaysAttendance.SystemOTHours = todaysAttendance.TotalHours;
                    return;
                }
                var ot = todaysAttendance.TotalHours - roster.TotalHours;
                if (roster.LegalEntityCode == "CAYAN_UAE")
                {
                    if (ot != null && ot.Value.TotalMinutes > 15)
                    {
                        ot = TimeSpan.Zero;
                        var startDiff = roster.Duty1StartTime - todaysAttendance.Duty1StartTime;
                        var endDiff = todaysAttendance.Duty1EndTime - roster.Duty1EndTime;
                        if (startDiff.HasValue && startDiff.Value.TotalMinutes > 15)
                        {
                            ot = ot.Value.Add(startDiff.Value);
                        }
                        if (endDiff.HasValue && endDiff.Value.TotalMinutes > 15)
                        {
                            ot = ot.Value.Add(endDiff.Value);
                        }
                        




                        var totalMinutes = ot.Value.TotalMinutes;
                      
                        todaysAttendance.SystemOTHours = TimeSpan.FromMinutes(totalMinutes);

                    }
                    else
                    {
                        todaysAttendance.SystemOTHours = TimeSpan.Zero;
                    }
                }
                else
                {
                    if (ot != null && ot.Value.TotalMinutes > 15)
                    {
                        var totalMinutes = ot.Value.TotalMinutes;
                        
                        todaysAttendance.SystemOTHours = TimeSpan.FromMinutes(totalMinutes);

                    }
                    else
                    {
                        todaysAttendance.SystemOTHours = TimeSpan.Zero;
                    }
                }
            }
        }
        private void CalculateDeductionWithRoster(AttendanceViewModel todaysAttendance, RosterScheduleViewModel roster, bool isEmployeeOnPaidLeave)
        {
            if (roster != null)
            {
                if (roster.RosterDutyType == RosterDutyTypeEnum.DayOff || todaysAttendance.SystemAttendance == AttendanceTypeEnum.Absent || isEmployeeOnPaidLeave)
                {
                    return;
                }
                var ded = GetTotalDeductionWithRoster(todaysAttendance, roster);
                if (roster.LegalEntityCode == "CAYAN_UAE")
                {
                    if (ded.TotalMinutes > 15)
                    {
                        ded = TimeSpan.Zero;
                        var startDiff = todaysAttendance.Duty1StartTime - roster.Duty1StartTime;
                        var endDiff = roster.Duty1EndTime - todaysAttendance.Duty1EndTime;
                        if (startDiff.HasValue && startDiff.Value.TotalMinutes > 15)
                        {
                            ded = ded.Add(startDiff.Value);
                        }
                        if (endDiff.HasValue && endDiff.Value.TotalMinutes > 15)
                        {
                            ded = ded.Add(endDiff.Value);
                        }

                        var totalMinutes = ded.TotalMinutes;
                        todaysAttendance.SystemDeductionHours = TimeSpan.FromMinutes(totalMinutes);


                    }
                    else
                    {
                        todaysAttendance.SystemDeductionHours = TimeSpan.Zero;
                    }
                }
                else
                {
                    if (ded.TotalMinutes > 15)
                    {
                        var totalMinutes = ded.TotalMinutes;
                       
                        todaysAttendance.SystemDeductionHours = TimeSpan.FromMinutes(totalMinutes);
                    }
                    else
                    {
                        todaysAttendance.SystemDeductionHours = TimeSpan.Zero;
                    }
                }
               
            }
        }
        private TimeSpan GetTotalDeductionWithRoster(AttendanceViewModel todaysAttendance, RosterScheduleViewModel roster)
        {
            if (todaysAttendance == null || roster == null)
            {
                return TimeSpan.Zero;
            }
            double totalDeduction = 0;
            if (roster.Duty1Enabled && roster.Duty1StartTime.HasValue && roster.Duty1EndTime.HasValue
                && todaysAttendance.Duty1StartTime.HasValue && todaysAttendance.Duty1EndTime.HasValue)
            {
                var rosterStart = roster.RosterDate.Add(roster.Duty1StartTime.Value);
                var rosterEnd = roster.RosterDate.Add(roster.Duty1EndTime.Value);
                if (roster.Duty1FallsNextDay)
                {
                    rosterEnd = rosterEnd.AddDays(1);
                }

                var attendanceStart = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty1StartTime.Value);
                var attendanceEnd = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty1EndTime.Value);
                if (todaysAttendance.Duty1StartTime != todaysAttendance.Duty1EndTime)
                {
                    if (todaysAttendance.Duty1FallsPreviousDay.IsTrue())
                    {
                        attendanceStart = attendanceStart.AddDays(-1);
                    }
                    if (todaysAttendance.Duty1FallsNextDay)
                    {
                        attendanceEnd = attendanceEnd.AddDays(1);
                    }
                }
                var startingDifference = (attendanceStart - rosterStart).TotalMinutes;

                var totalDifference = (rosterEnd - rosterStart).TotalMinutes - (attendanceEnd - attendanceStart).TotalMinutes;
                if (totalDifference < 0)
                {
                    totalDifference = 0;
                }
                if (startingDifference > totalDifference)
                {
                    totalDeduction += startingDifference;
                }
                else
                {
                    totalDeduction += totalDifference;
                }
            }
            if (roster.Duty2Enabled && roster.Duty2StartTime.HasValue && roster.Duty2EndTime.HasValue
            && todaysAttendance.Duty2StartTime.HasValue && todaysAttendance.Duty2EndTime.HasValue)
            {
                var rosterStart = roster.RosterDate.Add(roster.Duty2StartTime.Value);
                var rosterEnd = roster.RosterDate.Add(roster.Duty2EndTime.Value);
                if (roster.Duty2FallsNextDay)
                {
                    rosterEnd = rosterEnd.AddDays(1);
                }

                var attendanceStart = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty2StartTime.Value);
                var attendanceEnd = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty2EndTime.Value);
                if (todaysAttendance.Duty2StartTime != todaysAttendance.Duty2EndTime)
                {
                    if (todaysAttendance.Duty2FallsPreviousDay.IsTrue())
                    {
                        attendanceStart = attendanceStart.AddDays(-1);
                    }
                    if (todaysAttendance.Duty2FallsNextDay)
                    {
                        attendanceEnd = attendanceEnd.AddDays(1);
                    }
                }
                var startingDifference = (attendanceStart - rosterStart).TotalMinutes;
                //if (startingDifference < 0)
                //{
                //    startingDifference = 0;
                //}
                var totalDifference = (rosterEnd - rosterStart).TotalMinutes - (attendanceEnd - attendanceStart).TotalMinutes;
                if (totalDifference < 0)
                {
                    totalDifference = 0;
                }
                if (startingDifference > totalDifference)
                {
                    totalDeduction += startingDifference;
                }
                else
                {
                    totalDeduction += totalDifference;
                }
            }
            if (roster.Duty3Enabled && roster.Duty3StartTime.HasValue && roster.Duty3EndTime.HasValue
           && todaysAttendance.Duty3StartTime.HasValue && todaysAttendance.Duty3EndTime.HasValue)
            {
                var rosterStart = roster.RosterDate.Add(roster.Duty3StartTime.Value);
                var rosterEnd = roster.RosterDate.Add(roster.Duty3EndTime.Value);
                if (roster.Duty3FallsNextDay)
                {
                    rosterEnd = rosterEnd.AddDays(1);
                }

                var attendanceStart = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty3StartTime.Value);
                var attendanceEnd = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty3EndTime.Value);
                if (todaysAttendance.Duty3StartTime != todaysAttendance.Duty3EndTime)
                {
                    if (todaysAttendance.Duty3FallsPreviousDay.IsTrue())
                    {
                        attendanceStart = attendanceStart.AddDays(-1);
                    }
                    if (todaysAttendance.Duty3FallsNextDay)
                    {
                        attendanceEnd = attendanceEnd.AddDays(1);
                    }
                }
                var startingDifference = (attendanceStart - rosterStart).TotalMinutes;

                var totalDifference = (rosterEnd - rosterStart).TotalMinutes - (attendanceEnd - attendanceStart).TotalMinutes;
                if (totalDifference < 0)
                {
                    totalDifference = 0;
                }
                if (startingDifference > totalDifference)
                {
                    totalDeduction += startingDifference;
                }
                else
                {
                    totalDeduction += totalDifference;
                }
            }
            return TimeSpan.FromMinutes(totalDeduction);
        }
        private Tuple<DateTime, DateTime> GetFirstCheckinLastCheckOut(DateTime rosterStartDate, DateTime rosterEndDate, List<AccessLogViewModel> accessLogEntries, RosterScheduleViewModel roster = null, int dutyNo = 1)
        {
            var checkInMin = rosterStartDate.AddHours(-1 * _otStart);
            var checkoutMax = rosterEndDate.AddHours(_otEndTime);
            if (roster != null)
            {
                if (dutyNo == 1 && roster.Duty2Enabled && roster.Duty2StartTime.HasValue)
                {
                    var duty2Start = roster.RosterDate.Add(roster.Duty2StartTime.Value).AddHours(-1);
                    if (roster.Duty1FallsNextDay)
                    {
                        duty2Start = duty2Start.AddDays(1);
                    }
                    if (checkoutMax > duty2Start)
                    {
                        checkoutMax = duty2Start;
                    }
                }
                else if (dutyNo == 2)
                {
                    checkInMin = rosterStartDate.AddHours(-1);
                    if (roster.Duty3Enabled && roster.Duty3StartTime.HasValue)
                    {
                        var duty3Start = roster.RosterDate.Add(roster.Duty3StartTime.Value).AddHours(-1);
                        if (roster.Duty2FallsNextDay)
                        {
                            duty3Start = duty3Start.AddDays(1);
                        }
                        if (checkoutMax > duty3Start)
                        {
                            checkoutMax = duty3Start;
                        }
                    }

                }
            }
            var firstCheckinEvent = accessLogEntries.Where(x => x.PunchingTime >= checkInMin && x.PunchingTime <= rosterEndDate)
                .OrderBy(x => x.PunchingTime).FirstOrDefault();

            if (firstCheckinEvent == null)
            {
                return null;
            }


            var lastCheckoutEvent = accessLogEntries.Where(x => x.PunchingTime <= checkoutMax && x.PunchingTime > checkInMin)
                .OrderByDescending(x => x.PunchingTime).FirstOrDefault();

            if (lastCheckoutEvent == null)
            {
                return new Tuple<DateTime, DateTime>(firstCheckinEvent.PunchingTime, firstCheckinEvent.PunchingTime);
            }
            return new Tuple<DateTime, DateTime>(firstCheckinEvent.PunchingTime, lastCheckoutEvent.PunchingTime);
        }
        private async Task CalculateTotalHoursAndOTWithoutRoster(AttendanceViewModel todaysAttendance)
        {
            var totalHours = new TimeSpan();
            if (todaysAttendance.Duty1Enabled && todaysAttendance.Duty1StartTime.HasValue && todaysAttendance.Duty1EndTime.HasValue)
            {
                var start = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty1StartTime.Value);
                var end = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty1EndTime.Value);
                if (todaysAttendance.Duty1FallsNextDay)
                {
                    end = todaysAttendance.AttendanceDate.AddDays(1).Add(todaysAttendance.Duty1EndTime.Value);
                }
                totalHours = totalHours.Add(end - start);
            }
            if (todaysAttendance.Duty2Enabled && todaysAttendance.Duty2StartTime.HasValue && todaysAttendance.Duty2EndTime.HasValue)
            {
                var start = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty2StartTime.Value);
                var end = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty2EndTime.Value);
                if (todaysAttendance.Duty2FallsNextDay)
                {
                    end = todaysAttendance.AttendanceDate.AddDays(1).Add(todaysAttendance.Duty2EndTime.Value);
                }
                totalHours = totalHours.Add(end - start);
            }
            if (todaysAttendance.Duty3Enabled && todaysAttendance.Duty3StartTime.HasValue && todaysAttendance.Duty3EndTime.HasValue)
            {
                var start = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty3StartTime.Value);
                var end = todaysAttendance.AttendanceDate.Add(todaysAttendance.Duty3EndTime.Value);
                if (todaysAttendance.Duty3FallsNextDay)
                {
                    end = todaysAttendance.AttendanceDate.AddDays(1).Add(todaysAttendance.Duty3EndTime.Value);
                }
                totalHours = totalHours.Add(end - start);
            }
            todaysAttendance.TotalHours = totalHours;
            todaysAttendance.SystemDeductionHours = null;
            todaysAttendance.SystemOTHours = null;
            if (totalHours.TotalMinutes > 0 || await IsEmployeeOnPaidLeave(todaysAttendance.UserId, todaysAttendance.AttendanceDate))
            {
                todaysAttendance.SystemAttendance = AttendanceTypeEnum.Present;
            }
            else
            {
                todaysAttendance.SystemAttendance = AttendanceTypeEnum.Absent;
            }

        }


        public async Task<List<AttendanceViewModel>> GetAttendanceListByDate(List<string> orgId, List<string> personId, DateTime? fromdate, DateTime? todate, List<string> empStatus, string payrollRunId = null)
        {
            try
            {
                if (payrollRunId.IsNotNull())
                {
                    var payrollRun = await _payrollBatchBusiness.GetSingleById(payrollRunId);
                    if (payrollRun != null)
                    {
                        fromdate = payrollRun.AttendanceStartDate;
                        todate = payrollRun.AttendanceEndDate;
                    }
                }
                //if (orgId == LegalEntityId)
                //  {
                //      orgId = null;
                //   }
                // var timeDiff = LegalEntityCode.ServerToLocalTimeDiff();

                var result = await _taaQueryBusiness.GetAttendanceListByDate(orgId, personId, fromdate, todate, empStatus, payrollRunId);

                if (result.Count > 0)
                {
                    var lbb = _serviceProvider.GetService<ILeaveBalanceSheetBusiness>();
                    var allLeaves = await lbb.GetAllLeaves(fromdate.Value, todate.Value);
                    var holidays = await lbb.GetHolidays(fromdate.Value, todate.Value);
                    //var timePersmissions = sb.GetAllTimePermissionList();
                    var timePersmissions = await GetReportTimePermissionList();
                    var businessTrips = await GetReportBusinessTripList();

                    foreach (var item in result)
                    {
                        try
                        {


                            if (item.RosterText.ToLower() == "dayoff")
                            {
                                item.LeaveTypeCode = "DAY_OFF";
                                item.SystemAttendanceText = item.RosterText;
                            }
                            else
                            {
                                var uhs = holidays.FirstOrDefault(x => x.UserId == item.UserId && x.FromDate <= item.AttDate.ToSafeDateTime() && x.ToDate >= item.AttDate.ToSafeDateTime());
                                var utp = timePersmissions.FirstOrDefault(x => x.UserId == item.UserId && x.Date == item.AttDate.ToSafeDateTime());
                                var ubt = businessTrips.FirstOrDefault(x => x.UserId == item.UserId && x.StartDate <= item.AttDate.ToSafeDateTime() && x.EndDate >= item.AttDate.ToSafeDateTime());
                                var uls = allLeaves.FirstOrDefault(x => x.UserId == item.UserId && x.StartDate <= item.AttDate.ToSafeDateTime() && x.EndDate >= item.AttDate.ToSafeDateTime());

                                item.SystemOTHours = item.SystemOTHours ?? new TimeSpan();
                                item.PermittedOTHours = new TimeSpan();
                                item.CalculatedOTHours = new TimeSpan();

                                item.SystemDeductionHours = item.SystemDeductionHours ?? new TimeSpan();
                                item.PermittedDeductionHours = new TimeSpan();

                                if (utp != null && utp.Hours.IsNotNullAndNotEmpty())
                                {
                                    var hrs = utp.Hours.Split('.');
                                    var hours = 0;
                                    var minutes = 0;
                                    if (hrs.Length > 0)
                                    {
                                        hours = hrs[0].ToSafeInt();
                                    }
                                    if (hrs.Length > 1)
                                    {
                                        minutes = hrs[1].ToSafeInt();
                                    }
                                    item.PermittedDeductionHours = new TimeSpan(hours, minutes, 0);
                                }
                                var calcDeddHours = item.SystemDeductionHours - item.PermittedDeductionHours;
                                item.CalculatedDeductionHours = item.SystemDeductionHours;
                                if (calcDeddHours.HasValue)
                                {
                                    if (calcDeddHours.Value.TotalMinutes >= 0)
                                    {
                                        item.CalculatedDeductionHours = calcDeddHours;
                                    }
                                    else
                                    {
                                        item.CalculatedDeductionHours = TimeSpan.Zero;
                                    }

                                }

                                if (uhs != null)
                                {
                                    item.LeaveTypeCode = "HOLIDAY";
                                    item.SystemAttendanceText = uhs.HolidayName;
                                }
                                else if (utp != null)
                                {
                                    item.LeaveTypeCode = utp.LeaveTypeCode;

                                    if (utp.TemplateAction == "Complete")
                                    {
                                        var reason = "";
                                        if (utp.TimePermissionType.HasValue)
                                        {
                                            reason = utp.TimePermissionType.Value.ToString();
                                        }
                                        else
                                        {
                                            reason = utp.TimePermissionReason;
                                        }
                                        if (reason.IsNullOrEmpty())
                                        {
                                            if (item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_UAE" || item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_KSA" || item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_AH")
                                            {
                                                reason = "Permission - Personal";
                                            }
                                            else if (item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_UAE" || item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_KSA" || item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_AH")
                                            {
                                                reason = "Permission - Business";
                                            }
                                        }
                                        item.SystemAttendanceText = string.Concat("Approved ", reason);
                                    }
                                    else
                                    {
                                        var reason = "";
                                        if (utp.TimePermissionType.HasValue)
                                        {
                                            reason = utp.TimePermissionType.Value.ToString();
                                        }
                                        else
                                        {
                                            reason = utp.TimePermissionReason;
                                        }
                                        if (reason.IsNullOrEmpty())
                                        {
                                            if (item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_UAE" || item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_KSA" || item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_AH")
                                            {
                                                reason = "Permission - Personal";
                                            }
                                            else if (item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_UAE" || item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_KSA" || item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_AH")
                                            {
                                                reason = "Permission - Business";
                                            }
                                        }
                                        item.SystemAttendanceText = string.Concat("Pending ", reason);
                                    }

                                }
                                else if (uls != null)
                                {
                                    item.LeaveTypeCode = uls.LeaveTypeCode;
                                    if (uls.TemplateAction == "Complete")
                                    {
                                        item.SystemAttendanceText = string.Concat("Approved ", uls.LeaveType);
                                    }
                                    else
                                    {
                                        item.SystemAttendanceText = string.Concat("Pending ", uls.LeaveType);
                                    }

                                }
                                else if (ubt != null)
                                {
                                    item.LeaveTypeCode = ubt.LeaveTypeCode;
                                    if (ubt.TemplateAction == "Complete")
                                    {
                                        item.SystemAttendanceText = string.Concat("Approved ", ubt.Name);
                                    }
                                    else
                                    {
                                        item.SystemAttendanceText = string.Concat("Pending ", ubt.Name);
                                    }


                                }
                                else
                                {
                                    if (item.CalculatedDeductionHours.HasValue && item.CalculatedDeductionHours.Value.TotalMinutes > 0)
                                    {
                                        item.LeaveTypeCode = "SHORT_TIME";
                                        item.SystemAttendanceText = "Late In/Early Out";
                                    }
                                    else if (item.Duty1StartTime.HasValue && item.Duty1EndTime.HasValue && item.Duty1StartTime.Value == item.Duty1EndTime.Value)
                                    {
                                        item.LeaveTypeCode = "LOG_MISSING";
                                        item.SystemAttendanceText = "Sign In/Out Missing";
                                    }                              
                                    else
                                    {
                                        item.LeaveTypeCode = item.SystemAttendance.ToString();
                                        item.SystemAttendanceText = item.SystemAttendance == AttendanceTypeEnum.Present ? "" : item.SystemAttendance.ToString();
                                    }
                                }
                            }

                        }
                        catch (Exception e2)
                        {

                            throw;
                        }


                    }
                }

                return result;
            }
            catch (Exception e)
            {

                throw;
            }


        
        }


       
    }
}