using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class LeaveBalanceSheetBusiness : BusinessBase<NoteViewModel, NtsNote>, ILeaveBalanceSheetBusiness
    {
        private readonly IRepositoryQueryBase<SalaryInfoViewModel> _salaryInfo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<LeaveViewModel> _queryLeaveRepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _queryPayBatch;
        private readonly IRepositoryQueryBase<LeaveDetailViewModel> _querylevdetail;
        INoteBusiness _noteBusiness;
        IServiceBusiness _serviceBusiness;
        IHRCoreBusiness _hRCoreBusiness;
        ILegalEntityBusiness _legalEntityBusiness;
        ITemplateCategoryBusiness _templateCategoryBusiness;
        ITemplateBusiness _templateBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        IServiceProvider _sp;
        
        private readonly IRepositoryQueryBase<CalendarViewModel> _calrepo;
        private readonly IRepositoryQueryBase<CalendarHolidayViewModel> _calholrepo;
        private readonly IRepositoryQueryBase<LeaveBalanceSheetViewModel> _leaveBalSheet;
        private readonly IRepositoryQueryBase<TeamLeaveRequestViewModel> _queryLeaveReq;
        private readonly ITaaQueryBusiness _taaQueryBusiness;
        public LeaveBalanceSheetBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper, IServiceBusiness serviceBusiness,
            IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo, IRepositoryQueryBase<TeamLeaveRequestViewModel> queryLeaveReq
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IRepositoryQueryBase<LeaveDetailViewModel> querylevdetail,
            INoteBusiness noteBusiness, IHRCoreBusiness hRCoreBusiness, IServiceProvider sp,
             ILegalEntityBusiness legalEntityBusiness, ITemplateCategoryBusiness templateCategoryBusiness,
             IRepositoryQueryBase<CalendarViewModel> calrepo, ITemplateBusiness templateBusiness,
             ITaaQueryBusiness taaQueryBusiness,
            IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch,
             IRepositoryQueryBase<LeaveBalanceSheetViewModel> leaveBalSheet, ITableMetadataBusiness tableMetadataBusiness,
             IRepositoryQueryBase<LeaveViewModel> queryLeaveRepo, IRepositoryQueryBase<CalendarHolidayViewModel> calholrepo) : base(repo, autoMapper)
        {
            _salaryInfo = salaryInfo;
            _sp = sp;
            _serviceBusiness = serviceBusiness;
            _queryRepo1 = queryRepo1;
            _queryLeaveReq = queryLeaveReq;
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _queryPayBatch = queryPayBatch;
            _legalEntityBusiness = legalEntityBusiness;
            _calrepo = calrepo;
            _leaveBalSheet = leaveBalSheet;
            _queryLeaveRepo = queryLeaveRepo;
            _hRCoreBusiness = hRCoreBusiness;
            _calholrepo = calholrepo;
            _templateCategoryBusiness = templateCategoryBusiness;
            _templateBusiness = templateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _querylevdetail = querylevdetail;
            _taaQueryBusiness = taaQueryBusiness;
        }

        public Task DeleteAnnualLeaveAccrual(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
        public async Task<TimeSpan?> getUnderTimeHours(string id)
        {
            var hours = await _taaQueryBusiness.getUnderTimeHours(id);
            return TimeSpan.Parse(hours);
        }
        public async Task<CommandResult<LeaveBalanceSheetViewModel>> UpdateLeaveBalance(DateTime date, string leaveTypeCode, string userId, double? leaveEntitlement = null, DateTime? dateOfJoin = null)
        {
            leaveTypeCode = "ANNUAL_LEAVE";
            var year = await _legalEntityBusiness.GetFinancialYear(date);

            var model = await _taaQueryBusiness.GetLeaveBalanceData(userId,leaveTypeCode,year);

            if ((model!=null && !model.OpeningBalance.IsNotNull()) || (model != null && model.OpeningBalance==0))
            {
               var person=await _hRCoreBusiness.GetPersonDetailByUserId(userId);
                if (person!=null) 
                {
                    var contract = await _hRCoreBusiness.GetContractDetail(person.Id);
                    if (contract.IsNotNull()) {
                        model.OpeningBalance = contract.AnnualLeaveEntitlement.IsNotNull() ? Convert.ToDouble(contract.AnnualLeaveEntitlement) : 0;
                    }
                }
            }
            if (model != null)
            {
                var leaves = await GetAllAnnualLeaveTransactions(userId);
                double leavesConsumed = 0;
                foreach (var item in leaves)
                {
                    switch (item.LeaveStatus)
                    {
                        case "SERVICE_STATUS_INPROGRESS":
                        case "SERVICE_STATUS_COMPLETE":
                        case "SERVICE_STATUS_CLOSE":
                        case "SERVICE_STATUS_OVERDUE":
                        case "SERVICE_STATUS_NOTSTARTED":
                            if (item.LeaveTypeCode == "LEAVE_ADJUSTMENT" || item.LeaveTypeCode == "LEAVE_ACCRUAL")
                            {
                                if (item.AddDeduct == "ADD")
                                {
                                    leavesConsumed += (item.Adjustment.Value * -1);
                                }
                                else
                                {
                                    leavesConsumed += item.Adjustment.Value;
                                }
                            }
                            else if (item.LeaveTypeCode == "ANNUAL_LEAVE_ENCASHMENT")
                            {
                                leavesConsumed += item.Adjustment.Value;
                            }
                            else
                            {
                               // item.LeaveStartDate = item.LeaveStartDate.HasValue ? item.LeaveStartDate.Value : DateTime.Now;
                                //item.LeaveEndDate = item.LeaveEndDate.HasValue ? item.LeaveEndDate.Value : DateTime.Now;
                                //var duration = await GetLeaveDuration(userId, item.LeaveStartDate.Value.Date, item.LeaveEndDate.Value.Date, item.LeaveTypeCode == "ANNUAL_LEAVE_HD");
                                if(item.LeaveStartDate.IsNotNull()&& item.LeaveEndDate.IsNotNull())
                                {
                                    var duration = await GetLeaveDuration(userId, item.LeaveStartDate.Value.Date, item.LeaveEndDate.Value.Date, item.LeaveTypeCode == "ANNUAL_LEAVE_HD");
                                    leavesConsumed += duration ?? 0;
                                }
                               

                            }


                            break;
                        default:
                            break;
                    }
                }
                model.ClosingBalance = Math.Round(model.OpeningBalance - leavesConsumed, 2);
                return await CorrectLeaveBalanceSheet(model);
            }
            else
            {
                var leave = await GetLeaveTypeByCode(leaveTypeCode);
                double balance = 0;
                var balanceSheetModel = new LeaveBalanceSheetViewModel
                {
                    Year = year,
                    OpeningBalance = balance,
                    ClosingBalance = balance,
                    UserId = userId,
                    IsDeleted = false,
                    CompanyId = _repo.UserContext.CompanyId,
                    Status = StatusEnum.Active,
                    DataAction = DataActionEnum.Create,
                    CreatedBy = _repo.UserContext.UserId,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = _repo.UserContext.UserId,
                    LastUpdatedDate = DateTime.Now,
                    LeaveTypeId = leave.Id

                };
                await CreateLeaveBalanceSheet(balanceSheetModel);
                await UpdateLeaveBalance(date, leaveTypeCode, userId, leaveEntitlement, dateOfJoin);
            }
            return null;
        }
        public async Task<IdNameViewModel> GetLeaveTypeByCode(string code)
        {
            var result = await _taaQueryBusiness.GetLeaveTypeByCode(code);
            return result;
        }
        public async Task<IList<LeaveViewModel>> GetAllAnnualLeaveTransactions(string userId)
        {
            var model = await _taaQueryBusiness.GetAllAnnualLeaveTransactions(userId);
            return model;
        }
        public async Task<double?> GetLeaveDuration(string userId, DateTime startDate, DateTime endDate, bool isHalfDay = false, bool includePublicHolidays = true)
        {
            var totalDays = (endDate - startDate).TotalDays + 1;
            var holidays = await GetTotalHolidays(userId, startDate, endDate, includePublicHolidays);
            if (holidays == null)
            {
                holidays = 0;
            }
            var duration = totalDays - holidays ?? 0;
            if (isHalfDay)
            {
                if (duration > 0)
                {
                    return 0.5;
                }
                else
                {
                    return 0.0;
                }
            }
            return duration;
        }
        public async Task<double?> GetTotalHolidays(string userId, DateTime startDate, DateTime endDate, bool includePublicHolidays = true)
        {
            double count = 0;

            var payCalendar = await _taaQueryBusiness.GetCalendarDetails(userId);
            if (payCalendar == null)
            {
                return null;
            }
            else
            {
                var payCalendarHolidays = await _taaQueryBusiness.GetHolidayDetails(payCalendar);
                var isWeekEnd = false;
                while (startDate <= endDate)
                {
                    switch (startDate.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            isWeekEnd = payCalendar.IsSundayWeekEnd;
                            break;
                        case DayOfWeek.Monday:
                            isWeekEnd = payCalendar.IsMondayWeekEnd;
                            break;
                        case DayOfWeek.Tuesday:
                            isWeekEnd = payCalendar.IsTuesdayWeekEnd;
                            break;
                        case DayOfWeek.Wednesday:
                            isWeekEnd = payCalendar.IsWednesdayWeekEnd;
                            break;
                        case DayOfWeek.Thursday:
                            isWeekEnd = payCalendar.IsThursdayWeekEnd;
                            break;
                        case DayOfWeek.Friday:
                            isWeekEnd = payCalendar.IsFridayWeekEnd;
                            break;
                        case DayOfWeek.Saturday:
                            isWeekEnd = payCalendar.IsSaturdayWeekEnd;
                            break;
                        default:
                            break;
                    }
                    if (isWeekEnd)
                    {
                        count++;
                    }
                    else if (includePublicHolidays && payCalendarHolidays.Any(x => x.FromDate <= startDate && startDate <= x.ToDate))
                    {
                        count++;
                    }
                    startDate = startDate.AddDays(1);
                }
            }
            return count;
        }
        public async Task<CommandResult<LeaveBalanceSheetViewModel>> CreateLeaveBalanceSheet(LeaveBalanceSheetViewModel viewModel, bool doCommit = true)
        {
            //var data = BusinessHelper.MapModel<LeaveBalanceSheetViewModel, TAA_LeaveBalanceSheet>(viewModel);
            if (!viewModel.LeaveTypeId.IsNotNullAndNotEmpty())
            {
                var leaveType = await _taaQueryBusiness.GetLeaveTypeByCode(viewModel);
                if (leaveType != null)
                {
                    viewModel.LeaveTypeId = leaveType.Id;
                }
            }
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _repo.UserContext.UserId;
            noteTempModel.TemplateCode = "LeaveBalanceSheet";
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
            var result = await _noteBusiness.ManageNote(notemodel);
            return CommandResult<LeaveBalanceSheetViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<LeaveBalanceSheetViewModel>> CorrectLeaveBalanceSheet(LeaveBalanceSheetViewModel viewModel, bool doCommit = true)
        {
            //var data = BusinessHelper.MapModel<LeaveBalanceSheetViewModel, TAA_LeaveBalanceSheet>(viewModel);
            //var errorList = new List<KeyValuePair<string, string>>();
            //var validateName = IsCodeExists(viewModel);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<LeaveBalanceSheetViewModel>.Instance(viewModel, false, validateName.Messages);
            //}
            // var query = $@"select * from cms.""N_TAA_LeaveBalanceSheet"" as lbs where ""Id""='{viewModel.Id}'";
            // var data = await _calrepo.ExecuteQuerySingle<LeaveBalanceSheetViewModel>(query, null);
            if (!viewModel.LeaveTypeId.IsNotNullAndNotEmpty())
            {
                var leaveType = await _taaQueryBusiness.GetLeaveTypeByCode(viewModel);
                if (leaveType != null)
                {
                    viewModel.LeaveTypeId = leaveType.Id;
                }
            }
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _repo.UserContext.UserId;
            noteTempModel.NoteId = viewModel.NtsNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            // var model = _autoMapper.Map<LeaveBalanceSheetViewModel, LeaveBalanceSheetViewModel>(data, viewModel);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel);
            var result = await _noteBusiness.ManageNote(notemodel);

            return CommandResult<LeaveBalanceSheetViewModel>.Instance(viewModel);
        }
       

        public async Task<List<LeaveDetailViewModel>> GetAllLeaveDuration(string userId, DateTime startDate, DateTime endDate)
        {            
            //if (userId == 1801)
            //{
            //    var k = 123;
            //}
            //var prms = new Dictionary<string, object> { { "UserId", userId } };
            //var cypher = string.Concat(@"match (u:ADM_User{ IsDeleted:0,Status:'Active',Id:{UserId}})
            //match (u)<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0})
            //match (s)-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //match (t)-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //match (tr)-[:R_TemplateMaster_TemplateCategory]
            //->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //match(s)-[:R_Service_Status_ListOfValue]->(lv:GEN_ListOfValue{IsDeleted:0})           
            //where not s.TemplateAction in ['Draft','Cancel']
            //and not tr.Code in ['LEAVE_ADJUSTMENT','LEAVE_ACCRUAL','LEAVE_CANCEL','RETURN_TO_WORK','ANNUAL_LEAVE_ENCASHMENT_KSA']
            //match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //match (nfv)-[:R_ServiceFieldValue_TemplateField]->(tf:NTS_TemplateField{FieldName:'startDate'})
            //match(s)<-[:R_ServiceFieldValue_Service]-(nfv1: NTS_ServiceFieldValue)
            //match (nfv1)-[:R_ServiceFieldValue_TemplateField]->(tf1:NTS_TemplateField{FieldName:'endDate'})
            //match(s)<-[:R_ServiceFieldValue_Service]-(nfv2: NTS_ServiceFieldValue)
            //match (nfv2)-[:R_ServiceFieldValue_TemplateField]->(tf2:NTS_TemplateField{FieldName:'leaveDuration'})
            //return s.ServiceNo as ServiceNo,u.Id as UserId,nfv.Code as StartDate,nfv1.Code as EndDate,tr.Code as LeaveTypeCode,tr.Name as LeaveType,nfv2.Code as Duration");

            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Leave" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);

            var querydata = await _taaQueryBusiness.LeaveDetails(templateList, userId);

            var list = querydata.Where(x => x.EndDate >= startDate && x.StartDate <= endDate).ToList();
            var returnList = new List<LeaveDetailViewModel>();
            foreach (var item in list)
            {

                var duration = item.Duration;
                var start = item.StartDate < startDate ? startDate : item.StartDate;
                var end = item.EndDate > endDate ? endDate : item.EndDate;

                var datedDuration = await GetLeaveDuration(userId, start.Value, end.Value, IsHalfDay(item.LeaveTypeCode), IsUnpaidLeave(item.LeaveTypeCode));
                var datedAllDuration = (end - start).Value.TotalDays + 1;
                var existing = returnList.FirstOrDefault(x => x.LeaveTypeCode == item.LeaveTypeCode);
                if (existing == null)
                {
                    returnList.Add(new LeaveDetailViewModel
                    {
                        ServiceNo = item.ServiceNo,
                        LeaveType = item.LeaveType,
                        LeaveTypeCode = item.LeaveTypeCode,
                        Duration = duration,
                        DatedDuration = datedDuration.Value,
                        DatedAllDuration = datedAllDuration
                    });
                }
                else
                {
                    existing.Duration += duration;
                    existing.DatedDuration += datedDuration.Value;
                    existing.DatedAllDuration += datedAllDuration;
                }
            }
            return returnList;
        }

        private bool IsHalfDay(string leaveTypeCode)
        {
            return leaveTypeCode == "ANNUAL_LEAVE_HD" || leaveTypeCode == "ANNUAL_LEAVE_HD_UAE" || leaveTypeCode == "ANNUAL_LEAVE_HD_AH";
        }
        private bool IsUnpaidLeave(string leaveTypeCode)
        {
            return leaveTypeCode == "UNPAID_L" || leaveTypeCode == "UNA_ABSENT" || leaveTypeCode == "UNPAID_L_UAE"
                || leaveTypeCode == "UNA_ABSENT_UAE" || leaveTypeCode == "UNPAID_L_AH" || leaveTypeCode == "UNA_ABSENT_AH"
                 || leaveTypeCode == "PLANNED_UNPAID_L" || leaveTypeCode == "PLANNED_UNPAID_L_UAE" || leaveTypeCode == "PLANNED_UNPAID_L_AH";
        }
        public async Task<List<LeaveDetailViewModel>> GetAllLeaveEncashmentDuration(DateTime startDate, DateTime endDate)
        {
            var querydata = await _taaQueryBusiness.GetAllLeaveEncashmentDuration(startDate, endDate);
            return querydata;
        }

        //public Task<List<LeaveDetailViewModel>> GetAllLeaves(DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<LeaveDetailViewModel>> GetAllLeavesWithDuration(DateTime startDate, DateTime endDate)
        {
            //throw new NotImplementedException();
            //var prms = new Dictionary<string, object> { { "SD", startDate }, { "ED", endDate } };
            //var cypher = string.Concat(@"match (u:ADM_User{ IsDeleted:0,Status:'Active'})
            //-[:R_User_PersonRoot]-(pr:HRS_PersonRoot{ IsDeleted: 0,Status:'Active'})
            //match (u)<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0})
            //match (s)-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //match (t)-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //match (tr)-[:R_TemplateMaster_TemplateCategory]
            //->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //match(s)-[:R_Service_Status_ListOfValue]->(lv:GEN_ListOfValue{IsDeleted:0})           
            //where not s.TemplateAction in ['Draft','Cancel']
            //and not tr.Code in ['LEAVE_ADJUSTMENT','LEAVE_ACCRUAL','LEAVE_CANCEL','RETURN_TO_WORK','ANNUAL_LEAVE_ENCASHMENT_KSA']
            //match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //match (nfv)-[:R_ServiceFieldValue_TemplateField]->(tf:NTS_TemplateField{FieldName:'startDate'})
            //match(s)<-[:R_ServiceFieldValue_Service]-(nfv1: NTS_ServiceFieldValue)
            //match (nfv1)-[:R_ServiceFieldValue_TemplateField]->(tf1:NTS_TemplateField{FieldName:'endDate'})
            //match(s)<-[:R_ServiceFieldValue_Service]-(nfv2: NTS_ServiceFieldValue)
            //match (nfv2)-[:R_ServiceFieldValue_TemplateField]->(tf2:NTS_TemplateField{FieldName:'leaveDuration'})
            //return pr.Id as PersonId,u.Id as UserId,nfv.Code as StartDate,nfv1.Code as EndDate,tr.Name as LeaveType,nfv2.Code as Duration,tr.Code as LeaveTypeCode,s.TemplateAction as TemplateAction");
            //var list = ExecuteCypherList<LeaveDetailViewModel>(cypher, prms)
            //     .Where(x => x.EndDate >= startDate && x.StartDate <= endDate).ToList();

            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Leave" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);

            var querydata = await _taaQueryBusiness.GetAllLeavesWithDuration(templateList);

            var list = querydata.Where(x => x.EndDate >= startDate && x.StartDate <= endDate).ToList();
            var returnList = new List<LeaveDetailViewModel>();
            foreach (var item in list)
            {
                var duration = item.Duration;
                var start = item.StartDate < startDate ? startDate : item.StartDate;
                var end = item.EndDate > endDate ? endDate : item.EndDate;
                var datedDuration = await GetLeaveDuration(item.UserId, start.Value, end.Value, IsHalfDay(item.LeaveTypeCode), IsUnpaidLeave(item.LeaveTypeCode));
                var datedAllDuration = (end - start).Value.TotalDays + 1;
                item.Duration += duration;
                item.DatedDuration += datedDuration;
                item.DatedAllDuration += datedAllDuration;
            }
            return list;
        }

        public async Task<List<LeaveDetailViewModel>> GetAllSickLeaveDuration(string userId, DateTime startDate, DateTime endDate)
        {
            //var prms = new Dictionary<string, object> { { "UserId", userId }, { "EED", endDate } };


            //var cypher = string.Concat(@"match (u:ADM_User{ IsDeleted:0,Status:'Active',Id:{UserId}})
            //match (u)<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0})
            //match (s)-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //match (t)-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //match (tr)-[:R_TemplateMaster_TemplateCategory]
            //->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //match(s)-[:R_Service_Status_ListOfValue]->(lv:GEN_ListOfValue{IsDeleted:0})           
            //where not s.TemplateAction in ['Draft','Cancel']
            //and   tr.Code in ['SICK_LEAVE','SICK_L_K','SICK_L_U','SICK_L_AH']
            //match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //match (nfv)-[:R_ServiceFieldValue_TemplateField]->(tf:NTS_TemplateField{FieldName:'startDate'})
            //match(s)<-[:R_ServiceFieldValue_Service]-(nfv1: NTS_ServiceFieldValue)
            //match (nfv1)-[:R_ServiceFieldValue_TemplateField]->(tf1:NTS_TemplateField{FieldName:'endDate'})
            //match(s)<-[:R_ServiceFieldValue_Service]-(nfv2: NTS_ServiceFieldValue)
            //match (nfv2)-[:R_ServiceFieldValue_TemplateField]->(tf2:NTS_TemplateField{FieldName:'leaveDuration'})
            //return u.Id as UserId,nfv.Code as StartDate,nfv1.Code as EndDate,tr.Code as LeaveType,nfv2.Code as Duration");
        //   var list = ExecuteCypherList<LeaveDetailViewModel>(cypher, prms)
              //  .Where(x => x.EndDate >= startDate && x.StartDate <= endDate).ToList();
            var returnList = new List<LeaveDetailViewModel>();
            var list = new List<LeaveDetailViewModel>();

            list = await _taaQueryBusiness.GetAllSickLeaveDuration(userId);

            foreach (var item in list)
            {
                var duration = item.Duration;
                var start = item.StartDate < startDate ? startDate : item.StartDate;
                var end = item.EndDate > endDate ? endDate : item.EndDate;
                var datedDuration =await GetLeaveDuration(userId, start.Value, end.Value);
                var datedAllDuration = (end - start).Value.TotalDays + 1;
                var existing = returnList.FirstOrDefault(x => x.LeaveType == item.LeaveType);
                if (existing == null)
                {
                    returnList.Add(new LeaveDetailViewModel { LeaveType = item.LeaveType, Duration = duration, DatedDuration = datedDuration, DatedAllDuration = datedAllDuration });
                }
                else
                {
                    existing.Duration += duration;
                    existing.DatedDuration += datedDuration;
                    existing.DatedAllDuration += datedAllDuration;
                }
            }
            return returnList;
        }

        public async Task<List<LeaveDetailViewModel>> GetAllUnpaidLeaveDuration(string userId, DateTime startDate, DateTime endDate)
        {
           // var prms = new Dictionary<string, object> { { "UserId", userId } };
           // var cypher = string.Concat(@"match (u:ADM_User{ IsDeleted:0,Status:'Active',Id:{UserId}})
           // match (u)<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0})
           // match (s)-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
           // match (t)-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
           // match (tr)-[:R_TemplateMaster_TemplateCategory]
           // ->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
           // match(s)-[:R_Service_Status_ListOfValue]->(lv:GEN_ListOfValue{IsDeleted:0})           
             
           // where not s.TemplateAction in ['Draft','Cancel']
           // and   tr.Code in ['UNPAID_L','UNA_ABSENT']
           // match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
           // match (nfv)-[:R_ServiceFieldValue_TemplateField]->(tf:NTS_TemplateField{FieldName:'startDate'})
           // match(s)<-[:R_ServiceFieldValue_Service]-(nfv1: NTS_ServiceFieldValue)
           // match (nfv1)-[:R_ServiceFieldValue_TemplateField]->(tf1:NTS_TemplateField{FieldName:'endDate'})
           // match(s)<-[:R_ServiceFieldValue_Service]-(nfv2: NTS_ServiceFieldValue)
           // match (nfv2)-[:R_ServiceFieldValue_TemplateField]->(tf2:NTS_TemplateField{FieldName:'leaveDuration'})
           //return u.Id as UserId,nfv.Code as StartDate,nfv1.Code as EndDate,tr.Code as LeaveType,tr.Code as LeaveTypeCode,nfv2.Code as Duration");

            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Leave" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);

            
            
            var returnList = new List<LeaveDetailViewModel>();

            var querydata = await _taaQueryBusiness.GetAllUnpaidLeaveDuration(userId, templateList);

            if (querydata.IsNotNull())
            {
                
                var list = querydata.Where(x => x.EndDate >= startDate && x.StartDate <= endDate).ToList();
               
                foreach (var item in list)
                {
                    var duration = item.Duration;
                    var start = item.StartDate < startDate ? startDate : item.StartDate;
                    var end = item.EndDate > endDate ? endDate : item.EndDate;
                    var datedDuration = await GetLeaveDuration(userId, start.Value, end.Value, false, false);
                    var datedAllDuration = (end - start).Value.TotalDays + 1;
                    var existing = returnList.FirstOrDefault(x => x.LeaveType == item.LeaveType);
                    if (existing == null)
                    {
                        returnList.Add(new LeaveDetailViewModel { LeaveType = item.LeaveType, LeaveTypeCode = item.LeaveTypeCode, Duration = duration, DatedDuration = datedDuration, DatedAllDuration = datedAllDuration });
                    }
                    else
                    {
                        existing.Duration += duration;
                        existing.DatedDuration += datedDuration;
                        existing.DatedAllDuration += datedAllDuration;
                    }
                }
            }
            
            return returnList;
        }

        public async Task<List<LeaveDetailViewModel>> GetAllUnpaidLeaveDurationIncludingPlannedUnpaidLeave(string userId, DateTime startDate, DateTime endDate)
        {
            // throw new NotImplementedException();
            var list = await _taaQueryBusiness.GetAllUnpaidLeaveDurationIncludingPlannedUnpaidLeave(userId);
            list=list.Where(x => x.EndDate >= startDate && x.StartDate <= endDate).ToList();
            var returnList = new List<LeaveDetailViewModel>();
            foreach (var item in list)
            {
                var duration = item.Duration;
                var start = item.StartDate < startDate ? startDate : item.StartDate;
                var end = item.EndDate > endDate ? endDate : item.EndDate;
                var datedDuration =await GetLeaveDuration(userId, start.Value, end.Value, false, false);
                var datedAllDuration = (end - start).Value.TotalDays + 1;
                var existing = returnList.FirstOrDefault(x => x.LeaveType == item.LeaveType);
                if (existing == null)
                {
                    returnList.Add(new LeaveDetailViewModel { LeaveType = item.LeaveType, Duration = duration, DatedDuration = datedDuration, DatedAllDuration = datedAllDuration });
                }
                else
                {
                    existing.Duration += duration;
                    existing.DatedDuration += datedDuration;
                    existing.DatedAllDuration += datedAllDuration;
                }
            }
            return returnList;


        }

        public async Task<double> GetAnnualLeaveDatedDurationForAccrual(string userId, DateTime startDate, DateTime endDate)
        {
            //var prms = new Dictionary<string, object> { { "UserId", userId } };
            //var cypher = string.Concat(@"match (u:ADM_User{Id:{UserId},IsDeleted:0,Status:'Active'})
            //<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0,Status:'Active'})
            //-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //-[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //optional match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //-[:R_ServiceFieldValue_TemplateField]->(ttf:NTS_TemplateField)
            //with s,tr,u,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf
            //where not s.TemplateAction in ['Draft','Cancel']
            //and tr.Code in ['LEAVE_ADJUSTMENT','ANNUAL_LEAVE','ANNUAL_LEAVE_HD','ANNUAL_LEAVE_ADV','UNDERTIME_REQUEST','ANNUAL_LEAVE_ENCASHMENT_KSA']
            //return s.ServiceNo,u.Id as UserId,tr.Code as LeaveTypeCode,tr.Name as LeaveType
            //,case when tr.Code='ANNUAL_LEAVE_ENCASHMENT_KSA' then udf.effectiveDate else udf.startDate end as LeaveStartDate
            //,case when tr.Code='ANNUAL_LEAVE_ENCASHMENT_KSA' then udf.effectiveDate else   udf.endDate end as LeaveEndDate
            //,s.TemplateAction as LeaveStatus,udf.addDeduct as AddDeduct,udf.adjustment as Adjustment");
            //var list = ExecuteCypherList<LeaveViewModel>(cypher, prms).Where(x => x.LeaveEndDate >= startDate && x.LeaveStartDate <= endDate).ToList();
            double duration = 0;
            var templateCodes = new string[] { "LEAVE_ADJUSTMENT", "ANNUAL_LEAVE", "ANNUAL_LEAVE_HD", "ANNUAL_LEAVE_ADV", "UNDERTIME_REQUEST", "ANNUAL_LEAVE_ENCASHMENT_KSA" };
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Leave" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id && templateCodes.Any(y=>y==x.Code));
            
            var list =new List<LeaveViewModel>();

            list = await _taaQueryBusiness.GetAnnualLeaveDatedDurationForAccrual(userId, templateList);


            foreach (var item in list)
            {
                if (item.LeaveTypeCode == "LEAVE_ADJUSTMENT")
                {
                    if (item.AddDeduct == "Add")
                    {
                        duration += (item.Adjustment.Value * -1);
                    }
                    else
                    {
                        duration += item.Adjustment.Value;
                    }
                }
                else if (item.LeaveTypeCode == "ANNUAL_LEAVE_ENCASHMENT_KSA")
                {
                    duration += item.Adjustment.Value;
                }
                else
                {
                    var start = item.LeaveStartDate < startDate ? startDate : item.LeaveStartDate;
                    var end = item.LeaveEndDate > endDate ? endDate : item.LeaveEndDate;
                    var dur = await GetLeaveDuration(userId, start.Value, end.Value);
                    var datedDuration = dur ?? 0;
                    var datedAllDuration = (end - start).Value.Days + 1;
                    duration += datedDuration;

                }
            }
            return duration;
        }

        public Task<LeaveViewModel> GetAnyExistingLeave(int year, DateTime startDate, DateTime endDate, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<double> GetEntitlement(string leaveTypeCode, string userId)
        {
            var value = await _taaQueryBusiness.GetEntitlement(leaveTypeCode, userId);
            return value;
            // throw new NotImplementedException();
        }

        public async Task<CalendarViewModel> GetHolidaysAndWeekend(string userId, DateTime startDate, DateTime endDate, bool includePublicHolidays = true)
        {            
            int holidaycount = 0;
            int weekencount = 0;
            var calendar = new CalendarViewModel();

            //var cypher = @"match(c:PAY_Calendar{Status:'Active'})
            //<-[:R_SalaryInfo_PayCalendar]-(si:PAY_SalaryInfo{IsDeleted:0})
            //where  si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate  
            //match(si)-[:R_SalaryInfoRoot]->(sir:PAY_SalaryInfoRoot{ IsDeleted:0})
            //-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[:R_User_PersonRoot]-(u:ADM_User{Id:{UserId}})
            //return c";

            //var query = $@"Select c.* From cms.""N_PayrollHR_PayrollCalendar"" as c
            //                Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PayCalendarId"" = c.""Id"" and si.""CompanyId""='{_repo.UserContext.CompanyId}' and si.""IsDeleted""=false
            //                Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""CompanyId""='{_repo.UserContext.CompanyId}' and p.""IsDeleted""=false
            //                Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false";

            var prms = new Dictionary<string, object>
            {
                { "UserId",userId},
                { "ESD",DateTime.Today}
            };
            var payCalendar = await _taaQueryBusiness.GetCalendarDetails(userId);
            if (payCalendar == null)
            {
                return null;
            }
            else
            {
                //cypher = @"match(c:PAY_Calendar{Id:{Id}})<-[:R_CalendarHoliday_Calendar]-(h:PAY_CalendarHoliday{Status:'Active'}) return h";

                //var queryhol = $@"Select h.* from cms.""N_PayrollHR_PayrollCalendar"" as c
                //                Join cms.""N_PayrollHR_CalendarHoliday"" as h on h.""CalendarId""=c.""Id"" and h.""CompanyId""='{_repo.UserContext.CompanyId}' and h.""IsDeleted""=false
                //                where c.""Id""='{payCalendar.Id}' and c.""CompanyId""='{_repo.UserContext.CompanyId}' and c.""IsDeleted""=false";

                prms = new Dictionary<string, object> { { "Id", payCalendar.Id } };
                var payCalendarHolidays = await _taaQueryBusiness.GetHolidayDetails(payCalendar);
                var isWeekEnd = false;
                while (startDate <= endDate)
                {
                    switch (startDate.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            isWeekEnd = payCalendar.IsSundayWeekEnd;
                            break;
                        case DayOfWeek.Monday:
                            isWeekEnd = payCalendar.IsMondayWeekEnd;
                            break;
                        case DayOfWeek.Tuesday:
                            isWeekEnd = payCalendar.IsTuesdayWeekEnd;
                            break;
                        case DayOfWeek.Wednesday:
                            isWeekEnd = payCalendar.IsWednesdayWeekEnd;
                            break;
                        case DayOfWeek.Thursday:
                            isWeekEnd = payCalendar.IsThursdayWeekEnd;
                            break;
                        case DayOfWeek.Friday:
                            isWeekEnd = payCalendar.IsFridayWeekEnd;
                            break;
                        case DayOfWeek.Saturday:
                            isWeekEnd = payCalendar.IsSaturdayWeekEnd;
                            break;
                        default:
                            break;
                    }
                    if (isWeekEnd)
                    {
                        weekencount++;
                    }
                    else if (includePublicHolidays && payCalendarHolidays.Any(x => x.FromDate <= startDate && startDate <= x.ToDate))
                    {
                        holidaycount++;
                    }
                    startDate = startDate.AddDays(1);
                }
            }
            calendar.HolidayCount = holidaycount;
            calendar.WeekendCount = weekencount;

            return calendar;
        }

        public async Task<double> GetLeaveAccrualPerMonth(string userId, DateTime startDate, DateTime endDate, double? leaveEntitlement = null)
        {

            var workingDays = 30.0;
            var employeeWorkingDays = 30.0;
            var empWorkingStartDate = startDate;
            var empWorkingEndDate = endDate;
            var cm = await _taaQueryBusiness.GetContractDetails(userId);
            if (cm != null)
            {
                leaveEntitlement = cm.AnnualLeaveEntitlement ?? leaveEntitlement ?? 22.0;
                if (cm.DateOfJoin.HasValue && cm.DateOfJoin.Value > startDate
                    && cm.EffectiveEndDate.HasValue && cm.EffectiveEndDate.Value < endDate)
                {
                    empWorkingStartDate = cm.DateOfJoin.Value.Date;
                    empWorkingEndDate = cm.EffectiveEndDate.Value.Date;
                    employeeWorkingDays = (empWorkingEndDate - empWorkingStartDate).TotalDays;
                }
                else if (cm.DateOfJoin.HasValue && cm.DateOfJoin.Value.Date > startDate)
                {
                    empWorkingStartDate = cm.DateOfJoin.Value.Date;
                    employeeWorkingDays = (empWorkingEndDate - empWorkingStartDate).TotalDays;
                }
                else if (cm.EffectiveEndDate.HasValue && cm.EffectiveEndDate.Value.Date < endDate)
                {
                    empWorkingEndDate = cm.EffectiveEndDate.Value.Date;
                    employeeWorkingDays = (empWorkingEndDate - empWorkingStartDate).TotalDays;
                }

            }

            var monthlyAccrual = (leaveEntitlement / 12.0).RoundPayrollAmount();
            var unpaidLeaves =await GetAllUnpaidLeaveDurationIncludingPlannedUnpaidLeave(userId, startDate, endDate);
            if (unpaidLeaves != null && unpaidLeaves.Count > 0)
            {
                employeeWorkingDays = employeeWorkingDays - unpaidLeaves.Sum(x => x.DatedAllDuration.Value);
            }
            if (employeeWorkingDays > workingDays)
            {
                employeeWorkingDays = workingDays;
            }
            if (employeeWorkingDays < 0)
            {
                employeeWorkingDays = 0;
            }
            return ((monthlyAccrual / workingDays) * employeeWorkingDays).RoundToTwoDecimal();
        }
        public double GetLeaveAccrualPerMonth(double leaveEntitlement)
        {
            return 1.83;
            // throw new NotImplementedException();
        }

        public Task<double> GetLeaveAccruals(string userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<double> GetLeaveBalance(DateTime date, string leaveTypeCode, string userId)
        {
            leaveTypeCode = leaveTypeCode ?? "ANNUAL_LEAVE";
            var year = await _legalEntityBusiness.GetFinancialYear(date);

            var val = await _taaQueryBusiness.GetLeaveBalance(userId, year, leaveTypeCode);
            return val;
        }

       

        public Task<List<TeamLeaveRequestViewModel>> GetLeaveCalendar(long organizationId, DateTime? date = null)
        {
            throw new NotImplementedException();
        }

        

        public Task<double> GetLeaveEncashmentAmount(string userId, double encashleave)
        {
            throw new NotImplementedException();
        }

        public Task<double> GetRemainingLeaveAccruals(string userId, DateTime asofDate)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveDetailViewModel> GetSickLeaveBalance(string userId, DateTime asofDate)
        {
            var result = new LeaveDetailViewModel
            {
                Duration = 0,
                DatedAllDuration = 0,
                DatedDuration = 0,
                HalfDayDuration = 0,
                HalfDayDatedAllDuration = 0,
                HalfDayDatedDuration = 0,
                ThreeFourthDuration = 0,
                ThreeFourthDatedDuration = 0,
                ThreeFourthDatedAllDuration = 0,
                NoPayDuration = 0,
                NoPayDatedDuration = 0,
                NoPayDatedAllDuration = 0,
            };
           // var pb = BusinessHelper.GetInstance<IPersonBusiness>();
            var startDate = await GetCurrentAnniversaryStartDateByUserId(userId) ?? DateTime.Today;
            var leavesTaken =await GetTotalSickLeaveDuration(userId, startDate, asofDate);
            var probationEndDate = await GetProbationEndDateByUserId(userId);

            var sickLeavesTaken = leavesTaken.DatedDuration ?? 0;
            if (sickLeavesTaken <= 22)
            {
                result.DatedDuration = 22 - sickLeavesTaken;
                result.ThreeFourthDatedDuration = 44;
                result.NoPayDatedDuration = 22;
            }
            else if (sickLeavesTaken <= 22 + 44)
            {
                result.DatedDuration = 0;
                result.ThreeFourthDatedDuration = 44 - (sickLeavesTaken - 22);
                if (result.ThreeFourthDatedDuration < 0)
                {
                    result.ThreeFourthDatedDuration = 0;
                }
                result.NoPayDatedDuration = 22;
            }
            else
            {
                result.DatedDuration = 0;
                result.ThreeFourthDatedDuration = 0;
                result.NoPayDatedDuration = 66 - (sickLeavesTaken - 22);
                if (result.NoPayDatedDuration < 0)
                {
                    result.NoPayDatedDuration = 0;
                }
            }
            return result;

        }

        public async Task<DateTime> GetProbationEndDateByUserId(string userId)
        {
            //var parameters = new Dictionary<string, object> { { "UserId", userId } };
            //var cypher = @"match (u:ADM_User{IsDeleted: 0,Id:{UserId}})-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot)<-[:R_AssignmentRoot]-(a:HRS_Assignment{IsLatest:true})
            //return a";
            var assignment = await _taaQueryBusiness.GetAssignmentDetails(userId);

            if (assignment == null || assignment.DateOfJoin == null)
            {
                return DateTime.Today;
            }
            if (assignment.ProbationEndDate.HasValue)
            {
                return assignment.ProbationEndDate.Value;
            }
            if (assignment.ProbationPeriod == null)
            {
                return assignment.DTDateOfJoin.AddDays(90);
            }
            switch (assignment.ProbationPeriod)
            {
                case ProbationPeriodEnum.No:
                    return assignment.DTDateOfJoin;
                case ProbationPeriodEnum.OneMonth:
                    return assignment.DTDateOfJoin.AddDays(30);
                case ProbationPeriodEnum.TwoMonths:
                    return assignment.DTDateOfJoin.AddDays(60);
                case ProbationPeriodEnum.ThreeMonths:
                    return assignment.DTDateOfJoin.AddDays(90);
                case ProbationPeriodEnum.SixMonths:
                    return assignment.DTDateOfJoin.AddDays(180);
                default:
                    return assignment.DTDateOfJoin.AddDays(90);
            }
        }

        public async Task<DateTime?> GetCurrentAnniversaryStartDateByUserId(string userId)
        {
            var parameters = new Dictionary<string, object> { { "UserId", userId } };
            var assignment = await _taaQueryBusiness.GetCurrentAnniversaryStartDateByUserId(userId);
            if (assignment == null || assignment.DateOfJoin == null)
            {
                return null;
            }
            var anniversaryDate = new DateTime(DateTime.Today.Year, assignment.DTDateOfJoin.Month, assignment.DTDateOfJoin.Day);
            if (anniversaryDate > DateTime.Today)
            {
                anniversaryDate = anniversaryDate.AddYears(-1);
            }
            return anniversaryDate;
        }


        public async Task<ContractViewModel> GetContractByUser(string userId)
        {
            //var match = string.Concat(@"match(u:ADM_User{ IsDeleted: 0,CompanyId: { CompanyId},Id:{UserId} })-[:R_User_PersonRoot]->(pr: HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //    match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot)
            //    match(cr)<-[:R_ContractRoot]-(c:HRS_Contract)
            //    where c.EffectiveStartDate <= { ESD} <= c.EffectiveEndDate
            //    return c.EffectiveEndDate as EffectiveEndDate");

            var result = await _taaQueryBusiness.GetContractByUser(userId);

            return result;
        }
        public async Task<double> GetTicketAccrualPerMonth(string userId, DateTime startDate, DateTime endDate, double yearlyTicketCost)
        {
            // throw new NotImplementedException();
            var yearDays = 360.0;
            var cm = await _taaQueryBusiness.GetTicketDetails(userId);
            if (cm != null)
            {
                if (cm.AirTicketInterval == 0)
                {
                    cm.AirTicketInterval = 1;
                }
            }
            yearDays = yearDays * cm.AirTicketInterval;

            var dailyAccrual = (yearlyTicketCost / yearDays);
            var monthDays = 30.0;
            var deductionDays = 0.0;
            var unpaidLeaves =await GetAllUnpaidLeaveDurationIncludingPlannedUnpaidLeave(userId, startDate, endDate);
            if (unpaidLeaves != null && unpaidLeaves.Count > 0)
            {
                deductionDays = unpaidLeaves.Sum(x => x.DatedAllDuration.Value);
            }
            var contract =await GetContractByUser(userId);
            if (contract != null && contract.EffectiveStartDate.HasValue && contract.EffectiveEndDate.HasValue)
            {
                if (startDate < contract.EffectiveStartDate.Value)
                {
                    deductionDays += (contract.EffectiveStartDate.Value - endDate.FirstDateOfMonth()).Days;
                }
                if (endDate > contract.EffectiveEndDate.Value)
                {
                    deductionDays += (endDate.LastDateOfMonth() - contract.EffectiveEndDate.Value).Days;
                }

            }
            var intervalDays = (endDate - startDate).Days + 1;
            if (deductionDays >= monthDays || deductionDays >= intervalDays)
            {
                monthDays = 0;
            }
            else
            {
                monthDays -= deductionDays;
            }
            return (dailyAccrual * monthDays).RoundToTwoDecimal();
        }

        
        public async Task<LeaveDetailViewModel> GetTotalSickLeaveDuration(string userId, DateTime startDate, DateTime endDate)
        {
            var prms = new Dictionary<string, object> { { "UserId", userId }, { "EED", endDate } };
            var result = new LeaveDetailViewModel { DatedAllDuration = 0, Duration = 0, DatedDuration = 0 };

            var notInSystem = await _taaQueryBusiness.GetSickLeaveDetails(userId);

            var allUnpaidLeaves =await GetAllSickLeaveDuration(userId, startDate, endDate);

            foreach (var itemleave in allUnpaidLeaves)
            {
                switch (itemleave.LeaveTypeCode)
                {
                    case "SICK_LEAVE":
                    case "SICK_L_K":
                    case "SICK_L_U":
                    case "SICK_L_AH":
                        result.DatedAllDuration += itemleave.DatedAllDuration ?? 0;
                        result.DatedDuration += itemleave.DatedDuration ?? 0;
                        result.Duration += itemleave.Duration ?? 0;
                        break;
                }
            }
            result.DatedAllDuration += notInSystem;
            result.DatedDuration += notInSystem;
            result.Duration += notInSystem;
            return result;
        }

        public async Task<LeaveDetailViewModel> GetTotalUnpaidLeaveDuration(string userId, DateTime startDate, DateTime endDate)
        {
            //var cypher = @"match(u:ADM_User{Id:{UserId},IsDeleted: 0})-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[:R_SalaryInfoRoot_PersonRoot]-(salr:PAY_SalaryInfoRoot)
            //match(salr)<-[:R_SalaryInfoRoot]-(sal:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //where sal.EffectiveStartDate <= {EED} <= sal.EffectiveEndDate
            //return sum(coalesce(sal.UnpaidLeavesNotInSystem, 0.0)) as UnpaidLeavesNotInSystem";

            
            var result = new LeaveDetailViewModel { DatedAllDuration = 0, DatedDuration = 0, Duration = 0 };

            var notInSystem = await _taaQueryBusiness.GetUnpaidLeaveDetails(userId);

            var allUnpaidLeaves = await GetAllUnpaidLeaveDuration(userId, startDate, endDate);

            foreach (var itemleave in allUnpaidLeaves)
            {
                switch (itemleave.LeaveTypeCode)
                {
                    case "UNPAID_L":
                    case "AUTH_LEAVE_WITHOUT_PAY":
                    case "UNPAID_L_UAE":
                    case "UNA_ABSENT_UAE":
                    case "UNPAID_L_AH":
                    case "UNA_ABSENT_AH":
                        result.DatedAllDuration += (itemleave.DatedAllDuration ?? 0);
                        result.DatedDuration += (itemleave.DatedDuration ?? 0);
                        result.Duration += (itemleave.Duration ?? 0);
                        break;
                }
            }
            result.DatedAllDuration += notInSystem;
            result.DatedDuration += notInSystem;
            result.Duration += notInSystem;
            return result;
        }

        public async Task<double> GetTotalWorkingDays(string userId, DateTime startDate, DateTime endDate, bool publicHolidayAsWorkingDay = false)
        {
            double count = 0;
            //var cypher = @"match(c:PAY_Calendar{Status:'Active'})
            //<-[:R_SalaryInfo_PayCalendar]-(si:PAY_SalaryInfo{IsDeleted:0})
            //where  si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate  
            //match(si)-[:R_SalaryInfoRoot]->(sir:PAY_SalaryInfoRoot{ IsDeleted:0})
            //-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[:R_User_PersonRoot]-(u:ADM_User{Id:{UserId}})
            //return c";



            var payCalendar = await _taaQueryBusiness.GetCalendarDetails(userId);
            if (payCalendar == null)
            {
                return count;
            }
            else
            {
                //cypher = @"match(c:PAY_Calendar{Id:{Id}})<-[:R_CalendarHoliday_Calendar]-(h:PAY_CalendarHoliday{Status:'Active'}) return h";
                //var query = $@"Select h.* From cms.""N_PayrollHR_CalendarHoliday"" as h
                //           Join cms.""N_PayrollHR_PayrollCalendar"" as c on c.""Id""=h.""CalendarId"" and c.""CompanyId""='{_repo.UserContext.CompanyId}' and c.""IsDeleted""=false
                //            where c.""Id""='{payCalendar.Id}' and h.""CompanyId""='{_repo.UserContext.CompanyId}' and h.""IsDeleted""=false";

                var payCalendarHolidays = await _taaQueryBusiness.GetHolidayDetails(payCalendar);
                var isWeekEnd = false;
                while (startDate <= endDate)
                {
                    switch (startDate.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            isWeekEnd = payCalendar.IsSundayWeekEnd;
                            break;
                        case DayOfWeek.Monday:
                            isWeekEnd = payCalendar.IsMondayWeekEnd;
                            break;
                        case DayOfWeek.Tuesday:
                            isWeekEnd = payCalendar.IsTuesdayWeekEnd;
                            break;
                        case DayOfWeek.Wednesday:
                            isWeekEnd = payCalendar.IsWednesdayWeekEnd;
                            break;
                        case DayOfWeek.Thursday:
                            isWeekEnd = payCalendar.IsThursdayWeekEnd;
                            break;
                        case DayOfWeek.Friday:
                            isWeekEnd = payCalendar.IsFridayWeekEnd;
                            break;
                        case DayOfWeek.Saturday:
                            isWeekEnd = payCalendar.IsSaturdayWeekEnd;
                            break;
                        default:
                            break;
                    }
                    if (!isWeekEnd)
                    {
                        if (publicHolidayAsWorkingDay)
                        {
                            count++;
                        }
                        else if (!payCalendarHolidays.Any(x => x.FromDate <= startDate && startDate <= x.ToDate))
                        {
                            count++;
                        }

                    }
                    startDate = startDate.AddDays(1);
                }
            }
            return count;
        }

        public Task<bool> IsInProbation(string userId, DateTime dateOfJoin, DateTime asofDate, DateTime? probationEndDate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsOnLeave(string userId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsOnPaidLeave(string userId, DateTime date)
        {
            var result = await _taaQueryBusiness.IsOnPaidLeave(userId, date);
            return result;
        }

        private async Task<ServiceViewModel> LeaveAccrualServiceExists(string userId, DateTime startDate, DateTime endDate, IServiceBusiness serviceBusiness=null)
        {
            var res = await _taaQueryBusiness.LeaveAccrualServiceExists(userId, startDate, endDate, serviceBusiness);
            return res;
        }

        public async Task ManageAnnualLeaveAccrual(DateTime startDate, DateTime endDate, DateTime accrualDate)
        {
            //throw new NotImplementedException();
            try
            {
                var personBusiness = _sp.GetService<IHRCoreBusiness>();
               // var serviceBusiness = BusinessHelper.GetInstance<IServiceBusiness>();
                var persons = await personBusiness.GetPersonDetailsForLOV("");
                persons = persons.Where(x => x.UserId != null && x.GradeName != null).ToList();
                foreach (var person in persons)
                {
                    try
                    {
                        double annualLeaveEntitlement = 22;
                        if (person.AnnualLeaveEntitlement.HasValue)
                        {
                            annualLeaveEntitlement = person.AnnualLeaveEntitlement.Value;
                        }
                        double accrual =await GetLeaveAccrualPerMonth(person.UserId, startDate, endDate, annualLeaveEntitlement);
                        var exist =await LeaveAccrualServiceExists(person.UserId, startDate, endDate);
                        if (exist != null)
                        {
                            //var cypher = @"match (s:NTS_Service{Id:{ServiceId}})
                            //<-[:R_ServiceFieldValue_Service]-(nfv3: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(tf3)
                            //where tf3.FieldName = 'adjustment'
                            //return nfv3";
                            //var parameters = new Dictionary<string, object> {
                            //{ "ServiceId",exist.Id }
                            //};
                            //var adjValue = serviceBusiness.ExecuteCypher<NTS_ServiceFieldValue>(cypher, parameters);
                            //if (adjValue != null)
                            //{
                            //    adjValue.Code = adjValue.Value = accrual.ToString();
                            //    _repository.Edit<NTS_ServiceFieldValue>(adjValue);
                            //    UpdateLeaveBalance(DateTime.Today, "ANNUAL_LEAVE", person.UserId);
                            //}

                            var adjValue = await _tableMetadataBusiness.GetTableDataByColumn("LeaveAccrual", null, "Id", exist.UdfNoteTableId);
                            if (adjValue != null)
                            {
                                var updatecol = new Dictionary<string, object>();
                                updatecol.Add("Adjustment", accrual.ToString());
                                var noteid = Convert.ToString(adjValue["NtsNoteId"]);
                                await _tableMetadataBusiness.EditTableDataByHeaderId("LeaveAccrual", null, noteid, updatecol);
                            }
                        }
                        else
                        {
                            var service = new ServiceTemplateViewModel();
                            service.ActiveUserId = _repo.UserContext.UserId;
                            service.TemplateCode = "LeaveAccrual";
                            var leaveacc = await _serviceBusiness.GetServiceDetails(service);
                            leaveacc.OwnerUserId = person.UserId;
                            leaveacc.DataAction = DataActionEnum.Create;
                            leaveacc.AllowPastStartDate = true;
                            leaveacc.StartDate = accrualDate;
                            leaveacc.SLA = new TimeSpan(0, 00, 00, 00);
                            leaveacc.DueDate = accrualDate;
                            leaveacc.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                            leaveacc.ServiceSubject = leaveacc.ServiceDescription = string.Concat("Annual leave accrual for ", accrualDate.ToMMM_YYYY());
                            dynamic exo = new System.Dynamic.ExpandoObject();

                            ((IDictionary<String, Object>)exo).Add("AddOrDeduct", AddDeductEnum.Add.ToString());
                            ((IDictionary<String, Object>)exo).Add("Adjustment", accrual.ToString());
                            ((IDictionary<String, Object>)exo).Add("LeaveType", "Annual Leave");
                            leaveacc.Json =Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                            var stagecreate = await _serviceBusiness.ManageService(leaveacc);
                            //var service = serviceBusiness.GetServiceDetails(new ServiceViewModel
                            //{
                            //    TemplateMasterCode = "LEAVE_ACCRUAL",
                            //    ActiveUserId = 1,
                            //    RequestedByUserId = 1,
                            //    OwnerUserId = person.UserId,
                            //    Operation = DataOperation.Create,
                            //    StartDate = accrualDate,
                            //    SLA = new TimeSpan(0, 00, 00, 00),
                            //    HideDateAndSLA = false
                            //});
                            //service.Subject = service.Description = string.Concat("Annual leave accrual for ", accrualDate.ToMMM_YYYY());
                            //service.TemplateAction = NtsActionEnum.Submit;
                            //service.StartDate = accrualDate;
                            //service.SLA = new TimeSpan(0, 00, 00, 00);
                            //service.DueDate = accrualDate;
                            //service.IsSystemAutoService = true;
                            //service.Operation = DataOperation.Create;
                            //service.CreatedDate = service.LastUpdatedDate = DateTime.Now;
                            //var addOrDeduct = service.Controls.FirstOrDefault(x => x.FieldName == "addDeduct");
                            //if (addOrDeduct != null)
                            //{
                            //    addOrDeduct.Code = addOrDeduct.Value = AddDeductEnum.Add.ToString();
                            //}
                            //var adjustment = service.Controls.FirstOrDefault(x => x.FieldName == "adjustment");
                            //if (adjustment != null)
                            //{
                            //    adjustment.Code = adjustment.Value = accrual.ToString();
                            //}
                            //var leaveType = service.Controls.FirstOrDefault(x => x.FieldName == "leaveType");
                            //if (leaveType != null)
                            //{
                            //    leaveType.Code = "ANNUAL_LEAVE";
                            //    leaveType.Value = "Annual Leave";
                            //}
                            //service.IsValidated = true;
                            //serviceBusiness.Manage(service);

                            //service = serviceBusiness.GetServiceDetails(new ServiceViewModel
                            //{
                            //    Id = service.Id,
                            //    ActiveUserId = 1,
                            //    RequestedByUserId = 1,
                            //    OwnerUserId = person.UserId,
                            //    Operation = DataOperation.Correct
                            //});
                            //service.TemplateAction = NtsActionEnum.Complete;
                            //service.Operation = DataOperation.Correct;
                            //service.CreatedDate = service.LastUpdatedDate = DateTime.Now;
                            //service.CreatedBy = service.LastUpdatedBy = service.CreatedBy;
                            //service.IsSystemAutoService = true;
                            //serviceBusiness.Manage(service);
                            await UpdateLeaveBalance(DateTime.Today, "ANNUAL_LEAVE", person.UserId);
                        }

                    }
                    catch (Exception ex)
                    {

                      //  Log.Instance.Error(ex, "ManageAnnualLeaveAccrual Error");
                    }

                }
                try
                {

                    //var prms = new Dictionary<string, object>
                    //{
                    //    { "Status", StatusEnum.Active },
                    //    { "LegalEntityId",LegalEntityId },
                    //    { "SD",startDate },
                    //    { "ED",endDate },
                    //};
                    // var vm = _repository.ExecuteCypher<PayrollRunViewModel>(cypher, prms);
                    var vm = await _taaQueryBusiness.GetPayrollRunDetails(startDate, endDate);
                    if (vm != null)
                    {
                        var prb =_sp.GetService<IPayrollRunBusiness>();
                        await prb.LoadVacationAccrualForCurrentMonth(vm);
                    }
                }
                catch (Exception ex2)
                {

                  //  Log.Instance.Error(ex2, "LoadVacationAccrualForCurrentMonth Error");
                }

            }
            catch (Exception ex)
            {

              //  Log.Instance.Error(ex, "ManageAnnualLeaveAccrual Error");
            }
        }

       
        public async Task<bool> IsDayOff(string personId, DateTime dayToCheck)
        {
            //var IsDayQueryCheck = "pc.Is" + dayToCheck.DayOfWeek.ToString() + "WeekEnd";
            //var cypher = @"match (pr:HRS_PersonRoot{Id: {personId}})<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot)
            //match (psr)<-[:R_SalaryInfoRoot]-(ps:PAY_SalaryInfo) where datetime(ps.EffectiveStartDate) <= datetime() <= datetime(ps.EffectiveEndDate)
            //match (ps)-[:R_SalaryInfo_PayCalendar]->(pc:PAY_Calendar)
            //optional match (pc)<-[:R_CalendarHoliday_Calendar]-(pch:PAY_CalendarHoliday)
            //where date(datetime(pch.FromDate)) <= date(datetime()) <= date(datetime(pch.ToDate))
            //return CASE WHEN (count(pch)>0 or " + IsDayQueryCheck + @")  THEN true         
            //ELSE false END as IsDayOff limit 1";
            //var prms = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "Status", StatusEnum.Active.ToString() },
            //    { "personId", personId },
            //};
            //var result = await _queryRepo.ExecuteScalar<bool>(cypher, null);

            var IsDayQueryCheck = "Is" + dayToCheck.DayOfWeek.ToString() + "WeekEnd";
            DateTime todayDate = DateTime.Now;

            var result = await _taaQueryBusiness.IsDayOff(personId, todayDate, IsDayQueryCheck);
            return result;
        }
        public async Task<double> GetActualWorkingdays(string calendarId, DateTime startDate, DateTime endDate)
        {

            //var cypher = @"match (c:PAY_Calendar{IsDeleted:0,Id:{CalendarId}}) return c";
            ////var prms = new Dictionary<string, object>
            ////{
            ////    { "CalendarId", calendarId }
            ////};
            //var calendarVM = await _queryRepo.ExecuteQuerySingle<CalendarViewModel>(cypher, null);
            //cypher = @"match (ch:PAY_CalendarHoliday{IsDeleted:0})-[:R_CalendarHoliday_Calendar]
            //->(c:PAY_Calendar{IsDeleted:0,Id:{CalendarId}}) return ch";
            //var holidayVM = await _queryRepo.ExecuteQueryList<CalendarHolidayViewModel>(cypher, null);


            var calendarVM = await _taaQueryBusiness.GetCalendarDetailsByCalendarId(calendarId);

            //var query1 = $@"select h.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
            //join cms.""N_PayrollHR_CalendarHoliday"" as h on h.""CalendarId""=pc.""Id"" and pc.""Id""='{ calendarVM.Id}' and h.""IsDeleted""=false and h.""CompanyId""='{_repo.UserContext.CompanyId}'
            //  where pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var holidayVM = await _taaQueryBusiness.GetHolidayDetails(calendarVM);

            var totalDays = 0.0;
            if (calendarVM != null)
            {
                var sd = startDate.Date;
                var isHoliday = false;
                while (sd <= endDate)
                {
                    switch (sd.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            isHoliday = calendarVM.IsSundayWeekEnd;
                            break;
                        case DayOfWeek.Monday:
                            isHoliday = calendarVM.IsMondayWeekEnd;
                            break;
                        case DayOfWeek.Tuesday:
                            isHoliday = calendarVM.IsTuesdayWeekEnd;
                            break;
                        case DayOfWeek.Wednesday:
                            isHoliday = calendarVM.IsWednesdayWeekEnd;
                            break;
                        case DayOfWeek.Thursday:
                            isHoliday = calendarVM.IsThursdayWeekEnd;
                            break;
                        case DayOfWeek.Friday:
                            isHoliday = calendarVM.IsFridayWeekEnd;
                            break;
                        case DayOfWeek.Saturday:
                            isHoliday = calendarVM.IsSaturdayWeekEnd;
                            break;
                        default:
                            break;
                    }
                    if (!isHoliday)
                    {
                        isHoliday = holidayVM.Any(x => x.FromDate <= sd && x.ToDate >= sd);
                    }
                    if (!isHoliday)
                    {
                        totalDays++;
                    }
                    sd = sd.AddDays(1);
                }
            }
            return totalDays;
        }


        public async Task<List<CalendarHolidayViewModel>> GetHolidays(DateTime startDate, DateTime endDate)
        {            
            //var prms = new Dictionary<string, object>
            //{
            //    { "ESD", DateTime.Today }
            //};
            //var cypher = @"match (pr:HRS_PersonRoot)<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot)
            //match (pr)<-[:R_User_PersonRoot]-(u:ADM_User)             
            //match (psr)<-[:R_SalaryInfoRoot]-(ps:PAY_SalaryInfo) 
            //where  ps.EffectiveStartDate <= {ESD} <=  ps.EffectiveEndDate
            //match (ps)-[:R_SalaryInfo_PayCalendar]->(pc:PAY_Calendar)
            //<-[:R_CalendarHoliday_Calendar]-(ch:PAY_CalendarHoliday{IsDeleted:0})
            //return ch,pr.Id as PersonId,u.Id as UserId";


            var querydata = await _taaQueryBusiness.GetHolidays();
            var result = new List<CalendarHolidayViewModel>();
            if (querydata.Count > 0)
            {
                 result = querydata.Where(x => (x.FromDate <= startDate && x.ToDate >= startDate) || (x.FromDate <= endDate && x.ToDate >= endDate)
               || (x.FromDate >= startDate && x.FromDate <= endDate) || (x.ToDate >= startDate && x.ToDate <= endDate)).Distinct().ToList();
            }
            return result;
        }

   

        //Task<double> ILeaveBalanceSheetBusiness.GetLeaveBalance(DateTime date, string leaveTypeCode, string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double> ILeaveBalanceSheetBusiness.GetEntitlement(string leaveTypeCode, string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //Task ILeaveBalanceSheetBusiness.ManageAnnualLeaveAccrual(DateTime startDate, DateTime endDate, DateTime accrualDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<IList<LeaveViewModel>> ILeaveBalanceSheetBusiness.GetAllAnnualLeaveTransactions(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<LeaveViewModel> ILeaveBalanceSheetBusiness.GetAnyExistingLeave(int year, DateTime startDate, DateTime endDate, string userId)
        //{
        //    throw new NotImplementedException();
        //}

        async Task<double> ILeaveBalanceSheetBusiness.GetLeaveBalanceWithFutureEntitlement(DateTime date, string leaveTypeCode, string userId)
        {
            var legal =await _hRCoreBusiness.GetCompanyOrganization(userId);
            if (legal != null && legal.Name.ToLower().Contains("uae"))
            {
                return await GetLeaveBalanceWithFutureEntitlementUAE(date, leaveTypeCode, userId);
            }
            else
            {
                return await GetLeaveBalanceWithFutureEntitlementKSA(date, leaveTypeCode, userId);
            }
        }

        public async Task<double> GetLeaveBalanceWithFutureEntitlementKSA(DateTime date, string leaveTypeCode, string userId)
        {
            var year = await _legalEntityBusiness.GetFinancialYear(date);

            var leaveBalance = await _taaQueryBusiness.GetLeaveBalanceWithFutureEntitlementKSA(year, leaveTypeCode, userId);


            var numberofMonths = date.Month - DateTime.Now.Month;
            if (numberofMonths > 0)
            {
                var entitlement =await GetEntitlement(leaveTypeCode, userId);
                var value = GetLeaveAccrualPerMonth(entitlement);
                leaveBalance += Math.Round(value * numberofMonths, 2);
            }
            return leaveBalance;
        }

        public async Task<double> GetLeaveBalanceWithFutureEntitlementUAE(DateTime date, string leaveTypeCode, string userId)
        {
            //leaveTypeCode = "ANNUAL_LEAVE_UAE";
            var year = date.Year;

            //var cypher = @"MATCH (l:TAA_LeaveBalanceSheet{IsDeleted:0,Year:{Year}}) 
            //            match(l)-[:R_LeaveBalanceSheet_User]-(a:ADM_User{IsDeleted:0,Id:{UserId}})
            //            match(l)-[:R_LeaveBalanceSheet_LeaveType]-(lt:TAA_LeaveType{IsDeleted:0,Code:{LeaveTypeCode}})
            //            return l.ClosingBalance as ClosingBalance";

            var leaveBalance = await _taaQueryBusiness.GetLeaveBalanceWithFutureEntitlementUAE(year, leaveTypeCode, userId);
            var numberofMonths = date.Month - DateTime.Now.Month;
            if (numberofMonths > 0)
            {
                var entitlement = await GetEntitlement(leaveTypeCode, userId);
                var value = GetLeaveAccrualPerMonth(entitlement);
                leaveBalance += Math.Round(value * numberofMonths, 2);
            }
            return leaveBalance;
            // throw new NotImplementedException();

        }

        async Task<List<TeamLeaveRequestViewModel>> ILeaveBalanceSheetBusiness.GetLeaveCalendar(string organizationId, DateTime? date)
        {

            //var list = await _queryLeaveReq.ExecuteQueryList(query, null);
            var list = await _taaQueryBusiness.GetTeamLeaveDetails(organizationId, date);

            return list;
        }

        //Task<CommandResult<LeaveBalanceSheetViewModel>> ILeaveBalanceSheetBusiness.UpdateLeaveBalance(DateTime date, string leaveTypeCode, string userId, double? leaveEntitlement, DateTime? dateOfJoin)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<bool> ILeaveBalanceSheetBusiness.IsInProbation(string userId, DateTime dateOfJoin, DateTime asofDate, DateTime? probationEndDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double?> ILeaveBalanceSheetBusiness.GetTotalHolidays(string userId, DateTime startDate, DateTime endDate, bool includePublicHolidays)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double> ILeaveBalanceSheetBusiness.GetRemainingLeaveAccruals(string userId, DateTime asofDate)
        //{
        //    throw new NotImplementedException();
        //}

        
        //Task<bool> ILeaveBalanceSheetBusiness.IsOnLeave(string userId, DateTime date)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<bool> ILeaveBalanceSheetBusiness.IsOnPaidLeave(string userId, DateTime date)
        //{
        //    throw new NotImplementedException();
        //}

        //Task ILeaveBalanceSheetBusiness.DeleteAnnualLeaveAccrual(DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<LeaveDetailViewModel>> GetAllLeaves(DateTime startDate, DateTime endDate)
        {            
            //var cypher = string.Concat(@"match (u:ADM_User{ IsDeleted:0,Status:'Active'})
            //match (u)<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0})
            //match (s)-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //match (t)-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //match (tr)-[:R_TemplateMaster_TemplateCategory]
            //->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //match(s)-[:R_Service_Status_ListOfValue]->(lv:GEN_ListOfValue{IsDeleted:0})           
            //where not s.TemplateAction in ['Draft','Cancel']
            //and not tr.Code in ['LEAVE_ADJUSTMENT','LEAVE_ACCRUAL','LEAVE_CANCEL','RETURN_TO_WORK','ANNUAL_LEAVE_ENCASHMENT_KSA']
            //match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //match (nfv)-[:R_ServiceFieldValue_TemplateField]->(tf:NTS_TemplateField{FieldName:'startDate'})
            //match(s)<-[:R_ServiceFieldValue_Service]-(nfv1: NTS_ServiceFieldValue)
            //match (nfv1)-[:R_ServiceFieldValue_TemplateField]->(tf1:NTS_TemplateField{FieldName:'endDate'})
            //match(s)<-[:R_ServiceFieldValue_Service]-(nfv2: NTS_ServiceFieldValue)
            //match (nfv2)-[:R_ServiceFieldValue_TemplateField]->(tf2:NTS_TemplateField{FieldName:'leaveDuration'})
            //return u.Id as UserId,nfv.Code as StartDate,nfv1.Code as EndDate,tr.Name as LeaveType,nfv2.Code as Duration,tr.Code as LeaveTypeCode,s.TemplateAction as TemplateAction");

            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Leave" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);

            var querydata = await _taaQueryBusiness.GetAllLeaves(templateList);
            var list = querydata.Where(x => x.EndDate >= startDate && x.StartDate <= endDate).ToList();
            
            return list;
        }

        //Task<List<LeaveDetailViewModel>> ILeaveBalanceSheetBusiness.GetAllUnpaidLeaveDurationIncludingPlannedUnpaidLeave(string userId, DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double> ILeaveBalanceSheetBusiness.GetLeaveAccrualPerMonth(string userId, DateTime startDate, DateTime endDate, double? leaveEntitlement)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<CalendarViewModel> ILeaveBalanceSheetBusiness.GetHolidaysAndWeekend(string userId, DateTime startDate, DateTime endDate, bool includePublicHolidays)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double> ILeaveBalanceSheetBusiness.GetTicketAccrualPerMonth(string userId, DateTime startDate, DateTime endDate, double yearlyTicketCost)
        //{
        //    throw new NotImplementedException();
        //}
        

        //Task<double> ILeaveBalanceSheetBusiness.GetLeaveAccruals(string userId, DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double> ILeaveBalanceSheetBusiness.GetLeaveEncashmentAmount(string userId, double encashleave)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<List<LeaveDetailViewModel>> ILeaveBalanceSheetBusiness.GetAllUnpaidLeaveDuration(string userId, DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<List<LeaveDetailViewModel>> ILeaveBalanceSheetBusiness.GetAllLeavesWithDuration(DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double> ILeaveBalanceSheetBusiness.GetAnnualLeaveDatedDurationForAccrual(string userId, DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

//        async Task<LeaveDetailViewModel> ILeaveBalanceSheetBusiness.GetTotalUnpaidLeaveDuration(string userId, DateTime startDate, DateTime endDate)
//        {
//           // throw new NotImplementedException();
//            var query = $@"select sum(coalesce(si.""UnpaidLeavesNotInSystem"",'0')::int) as UnpaidLeavesNotInSystem
//from public.""User"" as u
//Join cms.""N_CoreHR_HRPerson"" as p on u.""Id""=p.""UserId"" and p.""IsDeleted""=false
//Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=p.""Id"" and si.""IsDeleted""=false
//where u.""Id""='{userId}' and u.""IsDeleted""=false ";

//            var result = new LeaveDetailViewModel();
//            var notInSystem = await _querylevdetail.ExecuteScalar<double?>(query, null);
//            var allUnpaidLeaves =await GetAllUnpaidLeaveDuration(userId, startDate, endDate);

//            foreach (var itemleave in allUnpaidLeaves)
//            {
//                switch (itemleave.LeaveTypeCode)
//                {
//                    case "UNPAID_L":
//                    case "AUTH_LEAVE_WITHOUT_PAY":
//                    case "UNPAID_L_UAE":
//                    case "UNA_ABSENT_UAE":
//                    case "UNPAID_L_AH":
//                    case "UNA_ABSENT_AH":
//                        result.DatedAllDuration += itemleave.DatedAllDuration ?? 0;
//                        result.DatedDuration += itemleave.DatedDuration ?? 0;
//                        result.Duration += itemleave.Duration ?? 0;
//                        break;
//                }
//            }
//            result.DatedAllDuration += notInSystem.HasValue? notInSystem.Value :0;
//            result.DatedDuration += notInSystem.HasValue ? notInSystem.Value : 0;
//            result.Duration += notInSystem.HasValue ? notInSystem.Value : 0;
//            return result;
//        }

        //Task<LeaveDetailViewModel> ILeaveBalanceSheetBusiness.GetSickLeaveBalance(string userId, DateTime asofDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<LeaveDetailViewModel> ILeaveBalanceSheetBusiness.GetTotalSickLeaveDuration(string userId, DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<List<LeaveDetailViewModel>> ILeaveBalanceSheetBusiness.GetAllSickLeaveDuration(string userId, DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<List<LeaveDetailViewModel>> ILeaveBalanceSheetBusiness.GetAllLeaveEncashmentDuration(DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double?> ILeaveBalanceSheetBusiness.GetLeaveDuration(string userId, DateTime startDate, DateTime endDate, bool isHalfDay, bool includePublicHolidays)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<TimeSpan?> ILeaveBalanceSheetBusiness.getUnderTimeHours(string id)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<bool> ILeaveBalanceSheetBusiness.IsDayOff(string personId, DateTime dayToCheck)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<double> ILeaveBalanceSheetBusiness.GetActualWorkingdays(string calendarId, DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<List<CalendarHolidayViewModel>> ILeaveBalanceSheetBusiness.GetHolidays(DateTime startDate, DateTime endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<IdNameViewModel> ILeaveBalanceSheetBusiness.GetLeaveTypeByCode(string code)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
