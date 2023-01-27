using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
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
        public LeaveBalanceSheetBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper, IServiceBusiness serviceBusiness,
            IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo, IRepositoryQueryBase<TeamLeaveRequestViewModel> queryLeaveReq
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IRepositoryQueryBase<LeaveDetailViewModel> querylevdetail,
            INoteBusiness noteBusiness, IHRCoreBusiness hRCoreBusiness, IServiceProvider sp,
             ILegalEntityBusiness legalEntityBusiness, ITemplateCategoryBusiness templateCategoryBusiness,
             IRepositoryQueryBase<CalendarViewModel> calrepo, ITemplateBusiness templateBusiness,
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
        }

        public Task DeleteAnnualLeaveAccrual(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
        public async Task<TimeSpan?> getUnderTimeHours(string id)
        {
            var query = $@"select udf.""Hours""
from public.""User"" as u 
join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""Id""='{id}' and s.""IsDeleted""=false and u.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""Code"" in ('UndertimeLeave','UNDERTIME_REQUEST_AH') and tr.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_UndertimeLeave"" as udf on udf.""NtsNoteId""=s.""UdfNoteId"" and udf.""CompanyId""='{_repo.UserContext.CompanyId}' and udf.""IsDeleted""=false
where u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
";
            //var cypher = @"match(a:ADM_User{IsDeleted:0})<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0, Id:{ServiceId}})
            //            -[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //              where tr.Code in ['UNDERTIME_REQUEST','UNDERTIME_REQUEST_AH']
            //             match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(tf{FieldName:'hours',IsDeleted:0})
            //            return nfv.Code";
            var hours = await _queryRepo.ExecuteScalar<string>(query, null);
            return TimeSpan.Parse(hours);
        }
        public async Task<CommandResult<LeaveBalanceSheetViewModel>> UpdateLeaveBalance(DateTime date, string leaveTypeCode, string userId, double? leaveEntitlement = null, DateTime? dateOfJoin = null)
        {
            leaveTypeCode = "ANNUAL_LEAVE";
            var year = await _legalEntityBusiness.GetFinancialYear(date);
            var Query = $@"SELECT lbs.*,u.""Id"" as UserId,lt.""Id"" as LeaveTypeId,lt.""Code"" as LeaveTypeCode 
FROM cms.""N_TAA_LeaveBalanceSheet"" as lbs
join public.""User"" as u on u.""Id""=lbs.""UserId"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
join cms.""N_TAA_LeaveType"" as lt on lbs.""LeaveTypeId""=lt.""Id"" and lt.""Code""='{leaveTypeCode}' and lt.""CompanyId""='{_repo.UserContext.CompanyId}'  and lt.""IsDeleted""=false
where lbs.""Year""='{year}'  and lbs.""CompanyId""='{_repo.UserContext.CompanyId}'  and lbs.""IsDeleted""=false";

            //var cypher = @"MATCH (l:TAA_LeaveBalanceSheet{IsDeleted:0,Year:{Year}}) 
            //match(l)-[:R_LeaveBalanceSheet_User]->(u:ADM_User{IsDeleted:0,Id:{UserId}})
            //match(l)-[:R_LeaveBalanceSheet_LeaveType]->(lt:TAA_LeaveType{IsDeleted:0,Code:{LeaveTypeCode}})
            //return l,u.Id as UserId,lt.Id as LeaveTypeId,lt.Code as LeaveTypeCode";
           
            var model = await _queryRepo.ExecuteQuerySingle<LeaveBalanceSheetViewModel>(Query, null);// ExecuteCypher<LeaveBalanceSheetViewModel>(cypher, parameters);
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
            var query = $@"select ""Id"",""Name"" from cms.""N_TAA_LeaveType"" where ""Code""='{code}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<IList<LeaveViewModel>> GetAllAnnualLeaveTransactions(string userId)
        {
            var cypher = $@"select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"",u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, null as LeaveStartDate
            ,null as LeaveEndDate,lov.""Code"" as LeaveStatus
            ,lovs.""Code"" as AddDeduct,coalesce(cast(la.""AdjustmentWorkingDays"" as DOUBLE PRECISION),0.0) as Adjustment
			from public.""User"" as u 
			join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""CompanyId""='{_repo.UserContext.CompanyId}'  and s.""IsDeleted""=false
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
			join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""CompanyId""='{_repo.UserContext.CompanyId}'  and tr.""IsDeleted""=false
			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave'  and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""IsDeleted""=false
			left join cms.""N_Leave_LeaveAdjustment"" as la on la.""Id""=s.""UdfNoteTableId""  and la.""CompanyId""='{_repo.UserContext.CompanyId}'  and la.""IsDeleted""=false
left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId"" and lovs.""CompanyId""='{_repo.UserContext.CompanyId}'  and lovs.""IsDeleted""=false
		    where tr.""Code"" ='LeaveAdjustment'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
and u.""Id""='{userId}'
union
--select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
--            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
--            ,'' as AddDeduct,0.0 as Adjustment
--
--            from public.""User"" as u
--            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id""
--join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
--			join public.""Template"" as tr on tr.""Id""=s.""TemplateId""
--			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave'
--
--            left join cms.""N_Leave_AccuralLeave"" as la on la.""Id""=s.""UdfNoteTableId"" 
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
--		    where tr.""Code"" ='AccuralLeave'
--and u.""Id""='{userId}'
--union 
select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
            ,'' as AddDeduct,0.0 as Adjustment

            from public.""User"" as u
            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""CompanyId""='{_repo.UserContext.CompanyId}'  and s.""IsDeleted""=false
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
			join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""CompanyId""='{_repo.UserContext.CompanyId}'  and tr.""IsDeleted""=false
			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""IsDeleted""=false

            left join cms.""N_Leave_AnnualLeave"" as la on la.""Id""=s.""UdfNoteTableId""  and la.""CompanyId""='{_repo.UserContext.CompanyId}'  and la.""IsDeleted""=false

		    where tr.""Code"" ='AnnualLeave' and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
and u.""Id""='{userId}'
union 
select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, null as LeaveStartDate
            ,null as LeaveEndDate,lov.""Code"" as LeaveStatus
            ,'' as AddDeduct,0.0 as Adjustment

            from public.""User"" as u
            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""CompanyId""='{_repo.UserContext.CompanyId}'  and s.""IsDeleted""=false
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
			join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""CompanyId""='{_repo.UserContext.CompanyId}'  and tr.""IsDeleted""=false
			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""IsDeleted""=false

            left join cms.""N_Leave_AnnualLeaveEncashment"" as la on la.""Id""=s.""UdfNoteTableId""   and la.""CompanyId""='{_repo.UserContext.CompanyId}'  and la.""IsDeleted""=false
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
		    where tr.""Code"" ='AnnualLeaveEncashment'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
and u.""Id""='{userId}'
union 
select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
            ,'' as AddDeduct,0.0 as Adjustment

            from public.""User"" as u
            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""CompanyId""='{_repo.UserContext.CompanyId}'  and s.""IsDeleted""=false
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
			join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""CompanyId""='{_repo.UserContext.CompanyId}'  and tr.""IsDeleted""=false
			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""IsDeleted""=false

            left join cms.""N_Leave_UndertimeLeave"" as la on la.""Id""=s.""UdfNoteTableId""   and la.""CompanyId""='{_repo.UserContext.CompanyId}'  and la.""IsDeleted""=false
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
		    where tr.""Code"" ='UndertimeLeave'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
and u.""Id""='{userId}'
--union 
--select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
--            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
--            ,'' as AddDeduct,0.0 as Adjustment
--
--            from public.""User"" as u
--            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id""
--join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
--			join public.""Template"" as tr on tr.""Id""=s.""TemplateId""
--			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave'
--
--            left join cms.""N_Leave_AnnualLeaveADV"" as la on la.""Id""=s.""UdfNoteTableId"" 
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
--		    where tr.""Code"" ='ANNUAL_LEAVE_ADV'
--and u.""Id""='{userId}'
--union 
--select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
--            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
--            ,'' as AddDeduct,0.0 as Adjustment
--
--            from public.""User"" as u
--            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id""
--join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
--			join public.""Template"" as tr on tr.""Id""=s.""TemplateId""
--			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave'
--
--            left join cms.""N_Leave_AnnualLeaveHD"" as la on la.""Id""=s.""UdfNoteTableId"" 
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
--		    where tr.""Code"" ='ANNUAL_LEAVE_HD'
--and u.""Id""='{userId}'
";
            //var cypher = @"match (u:ADM_User{Id:{UserId},IsDeleted:0,Status:'Active'})
            //<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0,Status:'Active'})
            //-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //-[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //optional match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //-[:R_ServiceFieldValue_TemplateField]->(ttf:NTS_TemplateField)
            //with s,tr,u,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf
            //where tr.Code in ['LEAVE_ADJUSTMENT','LEAVE_ACCRUAL','ANNUAL_LEAVE','ANNUAL_LEAVE_HD','ANNUAL_LEAVE_ADV','UNDERTIME_REQUEST','ANNUAL_LEAVE_ENCASHMENT_KSA']
            //return s,u.Id as UserId,tr.Code as LeaveTypeCode,tr.Name as LeaveType, udf.startDate as LeaveStartDate
            //,udf.endDate as LeaveEndDate,s.TemplateAction as LeaveStatus
            //,udf.addDeduct as AddDeduct,udf.adjustment as Adjustment";
            //var parameters = new Dictionary<string, object> {

            //    { "UserId",userId },

            //    { "Status",StatusEnum.Active },

            //};

            var model = await _queryLeaveRepo.ExecuteQueryList<LeaveViewModel>(cypher, null); /*ExecuteCypherList<LeaveViewModel>(cypher, parameters).ToList();*/
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
            var query = $@"SELECT pc.* FROM  cms.""N_PayrollHR_PayrollCalendar"" as pc 
join cms.""N_PayrollHR_SalaryInfo"" as psi on psi.""PayCalendarId"" = pc.""Id"" and psi.""IsDeleted""=false and psi.""CompanyId""='{_repo.UserContext.CompanyId}'  
join cms.""N_CoreHR_HRPerson"" as p on psi.""PersonId"" = p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
where  pc.""CompanyId""='{_repo.UserContext.CompanyId}'  and pc.""IsDeleted""=false
";



            var payCalendar = await _calrepo.ExecuteQuerySingle(query, null);
            if (payCalendar == null)
            {
                return null;
            }
            else
            {
                query = $@"select h.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
join cms.""N_PayrollHR_CalendarHoliday"" as h on h.""CalendarId""=pc.""Id"" and pc.""Id""='{ payCalendar.Id}' and h.""CompanyId""='{_repo.UserContext.CompanyId}'  and h.""IsDeleted""=false
where  pc.""CompanyId""='{_repo.UserContext.CompanyId}' and pc.""IsDeleted""=false";
                //cypher = @"match(c:PAY_Calendar{Id:{Id}})<-[:R_CalendarHoliday_Calendar]-(h:PAY_CalendarHoliday{Status:'Active'}) return h";
                //prms = new Dictionary<string, object> { { "Id", payCalendar.Id } };
                //var payCalendarHolidays = ExecuteCypherList<PAY_CalendarHoliday>(cypher, prms);
                var payCalendarHolidays = await _calrepo.ExecuteQueryList<CalendarHolidayViewModel>(query, null);
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
                var leaveType = await _queryRepo.ExecuteQuerySingle<LeaveTypeViewModel>($@"select * from cms.""N_TAA_LeaveType"" where ""Code""='{viewModel.LeaveTypeCode}'", null);
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
                var leaveType = await _calrepo.ExecuteQuerySingle<LeaveTypeViewModel>($@"select * from cms.""N_TAA_LeaveType"" where ""Code""='{viewModel.LeaveTypeCode}'", null);
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

            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (item.Code == "LeaveAdjustment" || item.Code == "LeaveAccrual" || item.Code == "LEAVE_CANCEL" || item.Code == "RETURN_TO_WORK" || item.Code == "AnnualLeaveEncashment")
                {

                }
                else
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }
                    if(item.Code== "UndertimeLeave")
                    {
                        selectQry = @$" {selectQry} Select s.""ServiceNo"", u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate,  udf.""LeaveEndDate""::TIMESTAMP::DATE  as EndDate, 
                            t.""DisplayName"" as LeaveType, null as CalendarDuration, null as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf 
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where udf.""IsDeleted""=false and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";

                    }
                    else if (item.Code == "LeaveHandoverService")
                    {
                        selectQry = @$" {selectQry} Select s.""ServiceNo"", u.""Id"" as UserId, null as StartDate,  null  as EndDate, 
                            t.""DisplayName"" as LeaveType, null as CalendarDuration, null as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where udf.""IsDeleted""=false and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";

                    }
                    else
                    {
                        selectQry = @$" {selectQry} Select s.""ServiceNo"", u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate,  udf.""LeaveEndDate""::TIMESTAMP::DATE  as EndDate, 
                            t.""DisplayName"" as LeaveType, udf.""LeaveDurationCalendarDays"" as CalendarDuration, udf.""LeaveDurationWorkingDays"" as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""IsDeleted""=false
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false 
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where udf.""IsDeleted""=false and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";

                    }
                    i++;
                }
                
            }

            var querydata = await _querylevdetail.ExecuteQueryList(selectQry, null);
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
            //and tr.Code in ['ANNUAL_LEAVE_ENCASHMENT_KSA']
            //match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //-[:R_ServiceFieldValue_TemplateField]->(ttf:NTS_TemplateField)
            //with s,pr,u,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf
            //where {SD}<=udf.effectiveDate<={ED}
            //return s.Id as ServiceId,pr.Id as PersonId,u.Id as UserId
            //,sum(coalesce(toFloat(udf.adjustment),0.0)) as Adjustment");
            //return ExecuteCypherList<LeaveDetailViewModel>(cypher, prms).ToList();

            var selectQry = @$" Select s.""Id"" as ServiceId, p.""Id"" as PersonId,u.""Id"" as UserId,sum(coalesce(0.0,0.0)) as Adjustment
                            From cms.""N_Leave_AnnualLeaveEncashment"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and  n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false  and  s.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and  t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false 
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and  UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and  st.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and  u.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and  p.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL')   and  lov.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            where udf.""IsDeleted""=false and  udf.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            group by s.""Id"",p.""Id"",u.""Id""
                            ";

            var querydata = await _querylevdetail.ExecuteQueryList(selectQry, null);
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

            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);
                if (item.Code == "LeaveAdjustment" || item.Code == "LeaveAccrual" || item.Code == "LEAVE_CANCEL" || item.Code == "RETURN_TO_WORK"
                    || item.Code == "AnnualLeaveEncashment" || item.Code == "LeaveHandoverService" || item.Code == "UndertimeLeave")
                { 
                }
                else
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }
                    selectQry = @$" {selectQry} Select p.""Id"" as PersonId,u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate, udf.""LeaveEndDate""::TIMESTAMP::DATE as EndDate
                                , t.""DisplayName"" as LeaveType, udf.""LeaveDurationCalendarDays"" as CalendarDuration, udf.""LeaveDurationCalendarDays"" as Duration, udf.""LeaveDurationWorkingDays"" as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
where  udf.""CompanyId""='{_repo.UserContext.CompanyId}'  and udf.""IsDeleted""=false";
                    i++;
                }
            }
            var querydata = await _querylevdetail.ExecuteQueryList(selectQry, null);
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
            var query = $@"select 
u.""Id"" as UserId ,nts.""TemplateCode"" as LeaveType
  ,al.""LeaveStartDate"" as StartDate,al.""LeaveEndDate"" as EndDate,al.""LeaveDurationWorkingDays"" as Duration
    from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_SickLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var list = new List<LeaveDetailViewModel>();
            list = await _querylevdetail.ExecuteQueryList<LeaveDetailViewModel>(query, null);
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

            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (item.Code == "UNPAID_L" || item.Code == "UNA_ABSENT")
                {                
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }
                    selectQry = @$" {selectQry} Select u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate, udf.""LeaveEndDate""::TIMESTAMP::DATE as EndDate, t.""DisplayName"" as LeaveType, udf.""LeaveDurationCalendarDays"" as CalendarDuration, udf.""LeaveDurationWorkingDays"" as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and udf.""CompanyId""='{_repo.UserContext.CompanyId}' and udf.""IsDeleted""=false ";
                }
                i++;
            }
            var returnList = new List<LeaveDetailViewModel>();
            if (selectQry.IsNotNullAndNotEmpty())
            {
                var querydata = await _querylevdetail.ExecuteQueryList(selectQry, null);
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
            var query = $@"select u.""Id"" as UserId,l.""LeaveStartDate"" as StartDate,
l.""LeaveEndDate"" as EndDate,'UNPAID_L' as LeaveType,l.""LeaveDurationWorkingDays""::int as Duration
from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on nts.""ServiceStatusId""=lv.""Id"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_UnpaidLeave"" as l on l.""Id""=nts.""UdfNoteTableId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}' and u.""IsDeleted""=false and lv.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and u.""CompanyId""='{_repo.UserContext.CompanyId}'

union

select u.""Id"" as UserId,l.""LeaveStartDate"" as StartDate,
l.""LeaveEndDate"" as EndDate,'PLANNED_UNPAID_L' as LeaveType,l.""LeaveDurationWorkingDays""::int as Duration
from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on nts.""ServiceStatusId""=lv.""Id"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_PlannedUnpaidLeave"" as l on l.""Id""=nts.""UdfNoteTableId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}' and u.""IsDeleted""=false and lv.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and u.""CompanyId""='{_repo.UserContext.CompanyId}'

union

select u.""Id"" as UserId,l.""LeaveStartDate"" as StartDate,
l.""LeaveEndDate"" as EndDate,'UNA_ABSENT' as LeaveType,0 as Duration
from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on nts.""ServiceStatusId""=lv.""Id"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_UNAAbsent"" as l on l.""Id""=nts.""UdfNoteTableId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}' and u.""IsDeleted""=false and lv.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and u.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var list = await _querylevdetail.ExecuteQueryList(query, null);
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
            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }

                    selectQry = @$" {selectQry} Select s.ServiceNo,u.Id as UserId,t.Code as LeaveTypeCode,t.Name as LeaveType
            ,case when t.Code='ANNUAL_LEAVE_ENCASHMENT_KSA' then udf.EffectiveDate else s.StartDate end as LeaveStartDate
            ,case when tr.Code='ANNUAL_LEAVE_ENCASHMENT_KSA' then udf.EffectiveDate else   s.DueDate end as LeaveEndDate
            ,udf.*,t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}' and lov.""IsDeleted""=false
                             where udf.""IsDeleted""=false and u.""Id""='{userId}' and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";
                    i++;

            }

            var list =new List<LeaveViewModel>();
            if (selectQry != "")
            {
                list = await _querylevdetail.ExecuteQueryList<LeaveViewModel>(selectQry, null);
            }
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
            var query = $@"select cr.""AnnualLeaveEntitlement""
            from cms.""N_CoreHR_HRPerson"" as p 
            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}' and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRContract"" as cr on cr.""EmployeeId""=p.""Id"" and  cr.""IsDeleted""=false and cr.""CompanyId""='{_repo.UserContext.CompanyId}'
            where  p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
";

            //var cypher = @"MATCH (u:ADM_User{IsDeleted:0,Id:{UserId}}) 
            //            match(u)-[:R_User_PersonRoot]->(pr:HRS_PersonRoot{IsDeleted:0})
            //            match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0})
            //            match(cr)<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0,IsLatest:true})
            //            return c.AnnualLeaveEntitlement as AnnualLeaveEntitlement";
            //var parameters = new Dictionary<string, object> {
            //    { "UserId",userId }
            //};
            var value = await _queryPayBatch.ExecuteScalar<double>(query, null);
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
            var query = $@"Select c.* From cms.""N_PayrollHR_PayrollCalendar"" as c
                            Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PayCalendarId"" = c.""Id"" and si.""CompanyId""='{_repo.UserContext.CompanyId}' and si.""IsDeleted""=false
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""CompanyId""='{_repo.UserContext.CompanyId}' and p.""IsDeleted""=false
                            Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false";

            var prms = new Dictionary<string, object>
            {
                { "UserId",userId},
                { "ESD",DateTime.Today}
            };
            var payCalendar = await _calrepo.ExecuteQuerySingle(query, null);
            if (payCalendar == null)
            {
                return null;
            }
            else
            {
                //cypher = @"match(c:PAY_Calendar{Id:{Id}})<-[:R_CalendarHoliday_Calendar]-(h:PAY_CalendarHoliday{Status:'Active'}) return h";
                var queryhol = $@"Select h.* from cms.""N_PayrollHR_PayrollCalendar"" as c
                                Join cms.""N_PayrollHR_CalendarHoliday"" as h on h.""CalendarId""=c.""Id"" and h.""CompanyId""='{_repo.UserContext.CompanyId}' and h.""IsDeleted""=false
                                where c.""Id""='{payCalendar.Id}' and c.""CompanyId""='{_repo.UserContext.CompanyId}' and c.""IsDeleted""=false";

                prms = new Dictionary<string, object> { { "Id", payCalendar.Id } };
                var payCalendarHolidays = await _calholrepo.ExecuteQueryList(queryhol, null);
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
            var cypher = string.Concat(
            $@"select c.*,a.""DateOfJoin"" as DateOfJoin
from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' ");
            var cm = await _queryLeaveRepo.ExecuteQuerySingle<ContractViewModel>(cypher, null);
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
            
            var query = $@"Select l.""ClosingBalance"" from cms.""N_TAA_LeaveBalanceSheet"" as l
                        Join cms.""N_TAA_LeaveType"" as lt on l.""LeaveTypeId""=lt.""Id"" and lt.""Code""='{leaveTypeCode}' and lt.""CompanyId""='{_repo.UserContext.CompanyId}'  and lt.""IsDeleted""=false 
                        Where l.""Year""='{year}' and l.""UserId""='{userId}' and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'";
                        
            var val = await _leaveBalSheet.ExecuteScalar<double>(query,null);
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
            var query = $@"select a.*,a.""DateOfJoin""::date as DTDateOfJoin from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var assignment = await _queryLeaveRepo.ExecuteQuerySingle<AssignmentViewModel>(query, null);

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
            var cypher = $@"select a.*,a.""DateOfJoin""::date as DTDateOfJoin from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'
           ";
            var assignment = await _queryLeaveRepo.ExecuteQuerySingle<AssignmentViewModel>(cypher,null);
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

            var query = $@"Select c.""EffectiveEndDate""::date From cms.""N_CoreHR_HRContract"" as c
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=c.""EmployeeId""  and p.""CompanyId""='{_repo.UserContext.CompanyId}' and p.""IsDeleted""=false
                            Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
where c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryRepo.ExecuteQuerySingle<ContractViewModel>(query, null);

            return result;
        }
        public async Task<double> GetTicketAccrualPerMonth(string userId, DateTime startDate, DateTime endDate, double yearlyTicketCost)
        {
            // throw new NotImplementedException();
            var yearDays = 360.0;
            var cypher = $@"select c.*,a.""DateOfJoin"" as DateOfJoin,s.""FlightTicketFrequency""::int as AirTicketInterval
from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRContract"" as c on p.""Id""=c.""EmployeeId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as s on p.""Id""=s.""PersonId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var cm = await _queryRepo.ExecuteQuerySingle<ContractViewModel>(cypher, null);
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
            var cypher = $@"select sum(coalesce(sal.""UnpaidLeavesNotInSystem""::int, 0)) as UnpaidLeavesNotInSystem
from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and  p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as sal on sal.""PersonId""=p.""Id"" and sal.""IsDeleted""=false and  sal.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""Id""='{userId}'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var prms = new Dictionary<string, object> { { "UserId", userId }, { "EED", endDate } };
            var result = new LeaveDetailViewModel { DatedAllDuration = 0, Duration = 0, DatedDuration = 0 };
            var notInSystem =await _queryLeaveRepo.ExecuteScalar<long>(cypher,null);
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

            var query = $@"Select sum(Cast(sal.""UnpaidLeavesNotInSystem"" as float)) as UnpaidLeavesNotInSystem
                            From cms.""N_PayrollHR_SalaryInfo"" as sal
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=sal.""PersonId"" and p.""IsDeleted""=false  and  p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""Id""='{userId}'  and  u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where sal.""IsDeleted""=false and  sal.""CompanyId""='{_repo.UserContext.CompanyId}' ";
           
            var result = new LeaveDetailViewModel { DatedAllDuration = 0, DatedDuration = 0, Duration = 0 };
            var notInSystem = await _querylevdetail.ExecuteScalar<double>(query, null);
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

            var query = $@"Select c.* From cms.""N_PayrollHR_PayrollCalendar"" as c 
                            Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PayCalendarId""=c.""Id""  and si.""CompanyId""='{_repo.UserContext.CompanyId}' and si.""IsDeleted""=false
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""CompanyId""='{_repo.UserContext.CompanyId}' and p.""IsDeleted""=false
                            Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
where  c.""CompanyId""='{_repo.UserContext.CompanyId}' and c.""IsDeleted""=false";

            var payCalendar = await _calrepo.ExecuteQuerySingle(query, null);
            if (payCalendar == null)
            {
                return count;
            }
            else
            {
                //cypher = @"match(c:PAY_Calendar{Id:{Id}})<-[:R_CalendarHoliday_Calendar]-(h:PAY_CalendarHoliday{Status:'Active'}) return h";
                query = $@"Select h.* From cms.""N_PayrollHR_CalendarHoliday"" as h
                           Join cms.""N_PayrollHR_PayrollCalendar"" as c on c.""Id""=h.""CalendarId"" and c.""CompanyId""='{_repo.UserContext.CompanyId}' and c.""IsDeleted""=false
                            where c.""Id""='{payCalendar.Id}' and h.""CompanyId""='{_repo.UserContext.CompanyId}' and h.""IsDeleted""=false";

                var payCalendarHolidays = await _calholrepo.ExecuteQueryList(query, null);
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

        public Task<bool> IsOnPaidLeave(string userId, DateTime date)
        {
            throw new NotImplementedException();
        }

        private async Task<ServiceViewModel> LeaveAccrualServiceExists(string userId, DateTime startDate, DateTime endDate, IServiceBusiness serviceBusiness=null)
        {
            var cypher = $@"select nts.*
from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_LeaveAccrual"" as l on l.""Id""=nts.""UdfNoteTableId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}'  and nts.""StartDate"">='{startDate}' and nts.""StartDate""<'{endDate}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' limit 1";
            //var parameters = new Dictionary<string, object> {
            //    { "UserId",userId },
            //    { "Status",StatusEnum.Active },
            //    { "CompanyId",1 },
            //    { "SD",startDate },
            //    { "ED",endDate },
            //};
            var res = await _queryLeaveRepo.ExecuteQuerySingle<ServiceViewModel>(cypher,null);
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
                    var cypher = $@"select pay.*,pb.""Id"" as PayrollId
                    ,leg.""Id"" as LegalEntityId,pg.""Id"" as PayrollGroupId,
					pb.""PayrollStartDate"" as PayrollStartDate,pb.""PayrollEndDate"" as PayrollEndDate
                    ,pb.""AttendanceStartDate"" as AttendanceStartDate,pb.""AttendanceEndDate"" as AttendanceEndDate
                    ,pb.""YearMonth"" as YearMonth,pb.""RunType"" as RunType
from cms.""N_PayrollHR_PayrollRun"" as pay
join cms.""N_PayrollHR_PayrollBatch"" as pb on pb.""Id""=pay.""PayrollBatchId"" and pb.""IsDeleted""=false and pb.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LegalEntity"" as leg on leg.""Id""=pb.""LegalEntityId"" and leg.""IsDeleted""=false and leg.""Id""='{_repo.UserContext.LegalEntityId}' and leg.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as d on d.""LegalEntityId""=leg.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=pb.""PayrollGroupId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
where pb.""PayrollStartDate""::date='{startDate}' and pb.""PayrollEndDate""='{endDate}'
and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'";
                    //var prms = new Dictionary<string, object>
                    //{
                    //    { "Status", StatusEnum.Active },
                    //    { "LegalEntityId",LegalEntityId },
                    //    { "SD",startDate },
                    //    { "ED",endDate },
                    //};
                   // var vm = _repository.ExecuteCypher<PayrollRunViewModel>(cypher, prms);
                    var vm =await _queryRepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
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

            var query = $@"select case when((count(pch.""Id"") > 0) or cast(pc.""{IsDayQueryCheck}"" as boolean)) then true else false end as IsDayOff
                        from cms.""N_CoreHR_HRPerson"" as pr
                        join cms.""N_PayrollHR_SalaryInfo"" as ps on ps.""PersonId"" = pr.""Id"" and ps.""IsDeleted"" = false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""Id"" = ps.""PayCalendarId""  and pc.""IsDeleted"" = false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_PayrollHR_CalendarHoliday"" as pch on pch.""CalendarId"" = pc.""Id"" and pch.""IsDeleted"" = false
                        and pch.""FromDate""::Date <= '{todayDate}'::Date and '{todayDate}'::Date <= pch.""ToDate""::Date and pch.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where pr.""IsDeleted"" = false and pr.""Id"" = '{personId}'  and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                        group by pch.""Id"",pc.""{IsDayQueryCheck}""
                        limit 1 ";
            var result = await _queryRepo.ExecuteScalar<bool>(query,null);
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

            var query = $@"select pc.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
                        where pc.""Id""='{calendarId}' and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var calendarVM = await _calrepo.ExecuteQuerySingle(query, null);

            var query1 = $@"select h.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
            join cms.""N_PayrollHR_CalendarHoliday"" as h on h.""CalendarId""=pc.""Id"" and pc.""Id""='{ calendarVM.Id}' and h.""IsDeleted""=false and h.""CompanyId""='{_repo.UserContext.CompanyId}'
              where pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var holidayVM = await _calholrepo.ExecuteQueryList(query1, null);

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

            var query = $@"Select ch.*, pr.""Id"" as PersonId, u.""Id"" as UserId
                        From cms.""N_PayrollHR_SalaryInfo"" as ps
                        Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=ps.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join public.""User"" as u on u.""Id""=pr.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""Id""=ps.""PayCalendarId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_PayrollHR_CalendarHoliday"" as ch on ch.""CalendarId""=pc.""Id"" and ch.""CompanyId""='{_repo.UserContext.CompanyId}'  and ch.""IsDeleted""=false
where ps.""CompanyId""='{_repo.UserContext.CompanyId}'  and ps.""IsDeleted""=false";

            var querydata = await _calholrepo.ExecuteQueryList(query, null);
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
            var query = $@"SELECT lbs.""ClosingBalance""
                        FROM cms.""N_TAA_LeaveBalanceSheet"" as lbs
                        join public.""User"" as u on u.""Id""=lbs.""UserId"" and u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_TAA_LeaveType"" as lt on lbs.""LeaveTypeId""=lt.""Id"" and lt.""Code""='{leaveTypeCode}' and lt.""IsDeleted""=false and lt.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where lbs.""Year""='{year}' and lbs.""CompanyId""='{_repo.UserContext.CompanyId}' and lbs.""IsDeleted""=false";

            var leaveBalance = await _queryPayBatch.ExecuteScalar<double>(query, null);


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
            var query = $@"SELECT lbs.""ClosingBalance""
                        FROM cms.""N_TAA_LeaveBalanceSheet"" as lbs 
                        join public.""User"" as u on u.""Id""=lbs.""UserId"" and u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_TAA_LeaveType"" as lt on lbs.""LeaveTypeId""=lt.""Id"" and lt.""Code""='{leaveTypeCode}' and lt.""IsDeleted""=false and lt.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where lbs.""Year""='{year}' and lbs.""CompanyId""='{_repo.UserContext.CompanyId}' and lbs.""IsDeleted""=false";
            //var cypher = @"MATCH (l:TAA_LeaveBalanceSheet{IsDeleted:0,Year:{Year}}) 
            //            match(l)-[:R_LeaveBalanceSheet_User]-(a:ADM_User{IsDeleted:0,Id:{UserId}})
            //            match(l)-[:R_LeaveBalanceSheet_LeaveType]-(lt:TAA_LeaveType{IsDeleted:0,Code:{LeaveTypeCode}})
            //            return l.ClosingBalance as ClosingBalance";

            var leaveBalance = await _queryPayBatch.ExecuteScalar<double>(query, null);
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
            var query = $@"

select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_AnnualLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_MaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_PaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_CompassionatelyLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	   
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_HajjLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false	and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  

	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_LeaveExamination"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false	and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_MarriageLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#

union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_PlannedUnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false  and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#

union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_SickLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and al.""CompanyId""='{_repo.UserContext.CompanyId}'	 
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#

union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_UndertimeLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'	 
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false  and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#

union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_UnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false	 #Org#
 	";
            var whereorg = "";
            if (organizationId.IsNotNullAndNotEmpty() && organizationId != "All")
            {
                whereorg = $@"and d.""Id""='{organizationId}'";
            }
            query = query.Replace("#Org#", whereorg);

            var list = await _queryLeaveReq.ExecuteQueryList(query, null);

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

            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (item.Code == "LeaveAdjustment" || item.Code == "LEAVE_ACCRUAL" || item.Code == "LEAVE_CANCEL" || item.Code == "LeaveHandoverService" || item.Code == "AnnualLeaveEncashment" || item.Code== "UndertimeLeave" ||  item.Code == "RETURN_TO_WORK")

                {

                }
                else
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }

                    selectQry = @$" {selectQry} Select u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate,  udf.""LeaveEndDate""::TIMESTAMP::DATE  as EndDate, t.""DisplayName"" as LeaveType, udf.""LeaveDurationCalendarDays"" as CalendarDuration, udf.""LeaveDurationWorkingDays"" as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where  udf.""CompanyId""='{_repo.UserContext.CompanyId}' and udf.""IsDeleted""=false";
                    i++;
                }
               
            }

            var querydata = await _querylevdetail.ExecuteQueryList(selectQry, null);
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
