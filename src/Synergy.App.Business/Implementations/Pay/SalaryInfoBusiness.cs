using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetLight;
using System.IO;

namespace Synergy.App.Business
{
    public class SalaryInfoBusiness : BusinessBase<NoteViewModel, NtsNote>, ISalaryInfoBusiness
    {
        private readonly IRepositoryQueryBase<SalaryInfoViewModel> _salaryInfo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _queryPayBatch;
        private readonly IRepositoryQueryBase<ElementViewModel> _queryEle;
        private readonly IRepositoryQueryBase<PayrollReportViewModel> _queryPayReport;
        private readonly IRepositoryQueryBase<PayrollPersonViewModel> _queryPayPerson;
        private readonly ILeaveBalanceSheetBusiness _leaveBalanceSheetBusiness;
        private readonly IPayrollElementBusiness _payrollElementBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        private readonly IPayRollQueryBusiness _payRollQueryBusiness;
        

        public SalaryInfoBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
        IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo, IUserBusiness userBusiness, IHRCoreBusiness hRCoreBusiness
        , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IPayrollElementBusiness payrollElementBusiness,
        IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch,
        IPayRollQueryBusiness payRollQueryBusiness,
        IRepositoryQueryBase<ElementViewModel> queryEle, IRepositoryQueryBase<PayrollReportViewModel> queryPayReport,
        IRepositoryQueryBase<PayrollPersonViewModel> queryPayPerson, ILeaveBalanceSheetBusiness leaveBalanceSheetBusiness) : base(repo, autoMapper)
        {
            _salaryInfo = salaryInfo;
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            _queryPayBatch = queryPayBatch;
            _queryEle = queryEle;
            _queryPayReport = queryPayReport;
            _queryPayPerson = queryPayPerson;
            _leaveBalanceSheetBusiness = leaveBalanceSheetBusiness;
            _payrollElementBusiness = payrollElementBusiness;
            _userBusiness = userBusiness;
            _hRCoreBusiness = hRCoreBusiness;
            _payRollQueryBusiness = payRollQueryBusiness;
        }

        public async Task<IList<PayrollReportViewModel>> GetAccuralDetails(PayrollReportViewModel searchModel)
        {
            var result = await _payRollQueryBusiness.GetAccuralDetails(searchModel);

            if (searchModel.ElementCode.Contains("'MONTHLY_VACATION_ACCRUAL'") || searchModel.ElementCode.Contains("'MONTHLY_EOS_ACCRUAL'")
                 || searchModel.ElementCode.Contains("'MONTHLY_SICK_LEAVE_ACCRUAL'"))
            {
                foreach (var item in result)
                {
                    if (item.UserId.IsNotNull() && searchModel.AttendanceStartDate.IsNotNull() && searchModel.AttendanceEndDate.IsNotNull())
                    {
                        var calendar = await _leaveBalanceSheetBusiness.GetHolidaysAndWeekend(item.UserId, searchModel.AttendanceStartDate.Value, searchModel.AttendanceEndDate.Value);
                        if (calendar.IsNotNull())
                        {
                            item.HolidayMonthlyDueDays = calendar.HolidayCount;
                            item.DayOffMonthlyDueDays = calendar.WeekendCount;
                        }
                      

                        var allLeaves = await _leaveBalanceSheetBusiness.GetAllLeaveDuration(item.UserId, searchModel.AttendanceStartDate.Value, searchModel.AttendanceEndDate.Value);

                        var onedayamount = await _payrollElementBusiness.GetUserOneDaySalary(item.UserId);
                        //var joiningDate = _userBusiness.GetPersonDateOfJoining(item.UserId.Value).ToSafeDateTime();
                        var joiningDate = await _userBusiness.GetPersonDateOfJoining(item.UserId);
                        var contractEndDate = await _hRCoreBusiness.GetContractEndDateByUser(item.UserId);
                        var annualLeaves = 0.0;
                        var sickLeaves = 0.0;
                        var otherLeaves = 0.0;
                        var unpaidLeaves = 0.0;
                        var plannedUnpaidLeaves = 0.0;
                        var nonworkingdays = 0.0;
                        if (joiningDate.IsNotNullAndNotEmpty() && searchModel.AttendanceStartDate.Value <= joiningDate.ToSafeDateTime() && searchModel.AttendanceEndDate.Value >= joiningDate.ToSafeDateTime())
                        {
                            //nonworkingdays = (searchModel.AttendanceStartDate.Value - joiningDate.ToSafeDateTime()).TotalDays;
                            nonworkingdays = await _leaveBalanceSheetBusiness.GetTotalWorkingDays(item.UserId, searchModel.AttendanceStartDate.Value, joiningDate.ToSafeDateTime());
                        }
                        else if (searchModel.AttendanceStartDate.Value <= contractEndDate && searchModel.AttendanceEndDate.Value >= contractEndDate)
                        {
                            //nonworkingdays = (contractEndDate - searchModel.AttendanceEndDate.Value).TotalDays;
                            nonworkingdays = await _leaveBalanceSheetBusiness.GetTotalWorkingDays(item.UserId, contractEndDate.Value, searchModel.AttendanceEndDate.Value);
                        }
                        foreach (var itemleave in allLeaves)
                        {
                            switch (itemleave.LeaveTypeCode)
                            {
                                case "ANNUAL_LEAVE":
                                case "ANNUAL_LEAVE_HD":
                                case "ANNUAL_LEAVE_ADV":
                                case "ANNUAL_LEAVE_UAE":
                                case "ANNUAL_LEAVE_HD_UAE":
                                case "ANNUAL_LEAVE_ADV_UAE":
                                case "ANNUAL_LEAVE_AH":
                                case "ANNUAL_LEAVE_HD_AH":
                                case "ANNUAL_LEAVE_ADV_AH":
                                    annualLeaves += itemleave.DatedDuration ?? 0;
                                    break;
                                case "SICK_L_K":
                                case "SICK_LEAVE":
                                case "SICK_L_U":
                                case "SICK_L_AH":
                                    sickLeaves += itemleave.DatedDuration ?? 0;
                                    break;
                                case "UNPAID_L":
                                case "AUTH_LEAVE_WITHOUT_PAY":
                                case "UNPAID_L_UAE":
                                case "UNA_ABSENT_UAE":
                                case "UNPAID_L_AH":
                                case "UNA_ABSENT_AH":
                                    unpaidLeaves += itemleave.DatedDuration ?? 0;
                                    break;
                                case "PLANNED_UNPAID_L":
                                case "PLANNED_UNPAID_L_UAE":
                                case "PLANNED_UNPAID_L_AH":
                                    plannedUnpaidLeaves += itemleave.DatedDuration ?? 0;
                                    break;
                                default:
                                    otherLeaves += itemleave.DatedDuration ?? 0;
                                    break;

                            }
                        }

                        var totalUnpaidLeaves = await _leaveBalanceSheetBusiness.GetTotalUnpaidLeaveDuration(item.UserId, joiningDate.ToSafeDateTime(), searchModel.AttendanceEndDate.Value);
                        item.AnnualLeaveDays = annualLeaves;
                        item.AnnualLeaveAmount = annualLeaves * onedayamount;
                        item.SickLeaveDays = sickLeaves;
                        item.SickLeaveAmount = sickLeaves * onedayamount;
                        item.UnpaidLeaveDays = unpaidLeaves + plannedUnpaidLeaves;
                        item.TotalUnpaidLeaveDays = totalUnpaidLeaves.DatedAllDuration;
                        //Log.Instance.Info(DelimeterEnum.Space, "TotalUnpaidLeaveDays ", item.UserId, joiningDate.ToSafeDateTime(), searchModel.AttendanceEndDate.Value, item.TotalUnpaidLeaveDays);
                        item.UnpaidLeaveAmount = (unpaidLeaves + plannedUnpaidLeaves) * onedayamount;
                        item.PlannedUnpaidLeaveDays = plannedUnpaidLeaves;
                        item.PlannedUnpaidLeaveAmount = plannedUnpaidLeaves * onedayamount;
                        item.OtherLeaveAmount = otherLeaves * onedayamount;
                        item.NonWorkingDays = nonworkingdays;
                        item.NonWorkingAmount = nonworkingdays * onedayamount;
                    }
                }
            }

            return result;
        }

        public async Task<IList<PayrollReportViewModel>> GetAccuralDetailsExcel(string personId, int? Year, MonthEnum? month = null)
        {
            var result = await _payRollQueryBusiness.GetAccuralDetailsExcel(personId, Year, month);
            return result;
        }

        public async Task<IList<PayrollReportViewModel>> GetBankDetails(PayrollReportViewModel searchModel)
        {
            var result = await _payRollQueryBusiness.GetBankDetails(searchModel);
            return result;
        }

        public async Task<string[]> GetDistinctElement(DateTime value)
        {
            //var match = string.Concat(@"match (sei:PAY_SalaryElementInfo{IsDeleted:0})
            //where sei.EffectiveStartDate<={ESD}<=sei.EffectiveEndDate
            //match (sei)-[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot)<-[:R_ElementRoot]-(e:PAY_Element{ElementCategory:'Standard'})
            //where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate
            //return distinct e.Name order by e.Name");

            var querydata = await _payRollQueryBusiness.GetDistinctElement(value);
            string[] result = querydata.Select(x => x.Name).ToArray();
            return result;
        }

        public async Task<string[]> GetDistinctElement()
        {
            //var match = string.Concat(@"match (sei:PAY_SalaryElementInfo{IsDeleted:0,IsLatest:true})           
            //match (sei)-[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot)<-[:R_ElementRoot]-(e:PAY_Element{ElementCategory:'Standard',IsLatest:true})           
            //return distinct e.Name order by e.Name");

            var querydata = await _payRollQueryBusiness.GetDistinctElement();
            string[] result = querydata.Select(x => x.Name).ToArray();
            return result;
        }

        public async Task<IList<PayrollReportViewModel>> GetLoanAccuralDetails(PayrollReportViewModel searchModel)
        {
            var result = await _payRollQueryBusiness.GetLoanAccuralDetails(searchModel);
            return result;
        }

        public async Task<IList<PayrollReportViewModel>> GetSalaryChangeInfoDetails(PayrollReportViewModel searchModel)
        {
            //var parameters = new Dictionary<string, object>
            //{
            //    { "PersonId", searchModel.PersonId },
            //    {"Status", StatusEnum.Active },
            //    {"CompanyId", CompanyId },
            //    {"LegalEntity",LegalEntityId},
            //    {"FromDate",searchModel.FromDate },
            //    { "ToDate",searchModel.ToDate}
            //};

            //     var cypher = @"
            //         match(sir:PAY_SalaryInfoRoot)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //         match (pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(orrr:HRS_OrganizationRoot{Id:{LegalEntity} ,IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})            
            //         match(sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId},IsLatest:true })
            //         match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsLatest:true,IsDeleted: 0,CompanyId: { CompanyId}})
            //         match(sir)<-[psrr:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo{ IsDeleted: 0,CompanyId: { CompanyId} })
            //         where {FromDate}<=ps.EffectiveStartDate<={ToDate}
            //         match(ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{ IsDeleted: 0,CompanyId: { CompanyId} })<-[:R_ElementRoot]-(e:PAY_Element{IsLatest:true,ElementCategory:'Standard', IsDeleted: 0,CompanyId: { CompanyId} })
            //         optional match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //         optional match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,CompanyId:{CompanyId},IsLatest:true})
            //         optional match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //optional match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
            //         optional match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //optional match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
            //         with si,p,o,j,pr,ps,e,pe
            //         where pr.Id={PersonId} or {PersonId} is null
            //         return ps,o.Name as OrganizationName,j.Name as JobName,ps.Amount as Amount,e.Name as ElementName,pe.Id as ElementId,pr.Id as PersonId,p.SponsorshipNo as SponsorshipNo, p.PersonNo as PersonNo,
            //         p.FirstName + coalesce(''+ p.MiddleName,'') + coalesce('' + p.LastName,'') as PersonName order by PersonId,ElementId ";

            var result = await _payRollQueryBusiness.GetPayrollReport(searchModel);
            foreach (var p in result)
            {
                //var parameters2 = new Dictionary<string, object>
                //{
                //    {"PersonId", p.PersonId},
                //    {"LegalEntity",_repo.UserContext.LegalEntityId},
                //    {"ElementId",p.ElementId },
                //    {"ToDate",p.EffectiveDate.Value.AddDays(-1)}
                //};
                //         var cypher2 = @"match (pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(orrr:HRS_OrganizationRoot{Id:{LegalEntity} ,IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})            
                //         match(sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId},IsLatest:true })
                //         match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsLatest:true,IsDeleted: 0,CompanyId: { CompanyId}})
                //         match(sir)<-[psrr:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo{ IsDeleted: 0,CompanyId: { CompanyId} })
                //         where ps.EffectiveEndDate={ToDate}
                //         match(ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{ IsDeleted: 0,CompanyId: { CompanyId} })<-[:R_ElementRoot]-(e:PAY_Element{IsLatest:true,ElementCategory:'Standard', IsDeleted: 0,CompanyId: { CompanyId} })
                //         where pe.Id={ElementId}
                //         optional match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,CompanyId:{CompanyId}})
                //         optional match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,CompanyId:{CompanyId},IsLatest:true})
                //         optional match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{ IsDeleted:0,CompanyId:{CompanyId}})
                //optional match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
                //         optional match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,CompanyId:{CompanyId}})
                //optional match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
                //         with si,p,o,j,pr,ps,e,pe
                //         where pr.Id={PersonId} or {PersonId} is null
                //         return ps,o.Name as OrganizationName,j.Name as JobName,ps.Amount as Amount,e.Name as ElementName,pe.Id as ElementId,pr.Id as PersonId,p.SponsorshipNo as SponsorshipNo, p.PersonNo as PersonNo,
                //         p.FirstName + coalesce(''+ p.MiddleName,'') + coalesce('' + p.LastName,'') as PersonName order by PersonId,ElementId";

                var querydata = await _payRollQueryBusiness.GetSalDataForDates(searchModel, p);
                var result2 = querydata.FirstOrDefault();
                if (result2 != null)
                {
                    p.AmountOld = result2.Amount;
                }
            }
            return result.OrderBy(o => o.PersonId).ThenBy(o => o.ElementId).ToList();
        }

        public async Task<IList<PayrollReportViewModel>> GetSalaryInfoDetails(PayrollReportViewModel searchModel)
        {
            //     var match = string.Concat(@"
            //         match(sir:PAY_SalaryInfoRoot)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //         match (pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(orrr:HRS_OrganizationRoot{Id:{LegalEntity} ,IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})            
            //         match(sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId},IsLatest:true })
            //         match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsLatest:true,IsDeleted: 0,CompanyId: { CompanyId}})
            //         match(sir)<-[psrr:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo{ IsDeleted: 0,CompanyId: { CompanyId} })
            //         match(ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{ IsDeleted: 0,CompanyId: { CompanyId} })<-[:R_ElementRoot]-(e:PAY_Element{IsLatest:true,ElementCategory:'Standard', IsDeleted: 0,CompanyId: { CompanyId} })
            //         optional match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //         optional match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,CompanyId:{CompanyId},IsLatest:true})
            //         optional match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //optional match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
            //         optional match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //optional match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
            //         with si,p,o,j,pr,ps,e,pe
            //         where pr.Id={PersonId} or {PersonId} is null
            //         return si,o.Name as OrganizationName,j.Name as JobName,ps.Amount as Amount,e.Name as ElementName,pe.Id as ElementId,pr.Id as PersonId,p.SponsorshipNo as SponsorshipNo, p.PersonNo as PersonNo,", Helper.PersonDisplayName("p", " as PersonName "));

            //     var parameters = new Dictionary<string, object>
            //     {
            //        { "PersonId", searchModel.PersonId },
            //         {"Status", StatusEnum.Active },
            //         {"CompanyId", CompanyId },
            //         {"LegalEntity",LegalEntityId}
            //        // {"ESD", DateTime.Now.ApplicationNow().Date },
            //        // {"EED", DateTime.Now.ApplicationNow().Date }
            //     };

            var result1 = await _payRollQueryBusiness.GetSalData(searchModel);
           
            //var result = result1.DistinctBy(x => new { x.PersonId }).ToList();
            var result = result1.GroupBy(x => new { x.PersonId }).Select(x => x.FirstOrDefault()).ToList();

            //var distinctElement = result1.DistinctBy(x => x.ElementId).OrderBy(x => x.ElementId);
            var distinctElement = result1.GroupBy(x => x.ElementId).Select(x => x.FirstOrDefault()).OrderBy(x => x.ElementId);
            foreach (var item in result)
            {
                var elementresult = result1.Where(x => x.PersonId == item.PersonId);

                int i = 1;
                foreach (var item3 in distinctElement)
                {
                    var Col = string.Concat("Element", i);
                    foreach (var item1 in elementresult)
                    {
                        if (item3.ElementId == item1.ElementId)
                           ApplicationExtension.SetPropertyValue(item, Col, item1.Amount);
                    }
                    i++;
                }
            }
            return result;
        }

        public async Task<string> GetSalaryInfoIdByPersonRootId(string personId)
        {
            var result = await _payRollQueryBusiness.GetSalaryInfoIdByPersonRootId(personId);
            return result;
        }

        public async Task<IList<SalaryInfoViewModel>> GetSearchResult(SalaryInfoViewModel searchModel)
        {
            var list = await ViewModelList<SalaryInfoViewModel>(@" ps.IsLatest=true", new Dictionary<string, object>(), "");
            return list;
        }

        public async Task<IList<SalaryInfoViewModel>> GetUnAssignedSalaryInfoList(string excludePersonId)
        {
            //var match = string.Concat("" + string.Concat(Helper.OrganizationMapping(_repo.UserContext.UserId, _repo.UserContext.CompanyId, _repo.UserContext.LegalEntityId),
            //    @"match(ar:HRS_AssignmentRoot)<-[:R_AssignmentRoot] - (a: HRS_Assignment{ IsDeleted: 0,CompanyId: { CompanyId} })
            //    where a.EffectiveStartDate <= { ESD} and a.EffectiveEndDate >= { EED}
            //    match(a)-[:R_Assignment_OrganizationRoot] -> (orr: HRS_OrganizationRoot{ IsDeleted: 0,CompanyId: { CompanyId} }) 
            //    where orr.Id= AllowedOrganizationIds
            //    match(ar)-[:R_AssignmentRoot_PersonRoot]->(pr: HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //    MATCH(pr)<-[:R_PersonRoot] - (po: HRS_Person{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
            //    WHERE NOT()-[:R_SalaryInfoRoot_PersonRoot]->(pr) and (po.EffectiveStartDate <= { ESD} and po.EffectiveEndDate >= { EED})
            //    return pr.Id as Id,", Helper.PersonDisplayNameWithSponsorshipNo("po", " as Name ")));

            var query = Helper.OrganizationMapping(_repo.UserContext.UserId, _repo.UserContext.CompanyId, _repo.UserContext.LegalEntityId);

            var result = await _payRollQueryBusiness.GetUnAssignedSalaryInfoList(excludePersonId, query);


            return result.ToList();

        }

        public async Task<IList<VM>> ViewModelList<VM>(string cypherWhere = "", Dictionary<string, object> parameters = null, string returnValues = "")
        {

            var result = await _payRollQueryBusiness.ViewModelList<VM>(cypherWhere, parameters, returnValues);
            return result;
            
        }
        public async Task<SalaryInfoViewModel> GetEligiblityForTickets(string UserId)
        {

            return await _payRollQueryBusiness.GetEligiblityForTickets(UserId);
        }




        
    }
}
