using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetLight;
using System.IO;

namespace CMS.Business
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
        public SalaryInfoBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
        IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo, IUserBusiness userBusiness, IHRCoreBusiness hRCoreBusiness
        , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IPayrollElementBusiness payrollElementBusiness,
        IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch,
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
        }

        public async Task<IList<PayrollReportViewModel>> GetAccuralDetails(PayrollReportViewModel searchModel)
        {

            //var query = string.Concat($@"Select t.*, p.""SponsorshipNo"", e.""ElementName"", e.""ElementCode"", u.""Id"" as UserId,
            //                                p.""Id"" as PersonId, p.""PersonNo"", d.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName,
            //                                sp.""Id"" as SponsorId, a.""DateOfJoin"" as EnrollDate, e.""ElementType"", ", Helper.PersonDisplayName("p", " as PersonName "),
            //                                $@" From cms.""N_CoreHR_HRPerson"" as p 
            //                                Join public.""LegalEntity"" as le on le.""Id""=p.""LegalEntityId"" and le.""Id""='{_repo.UserContext.LegalEntityId}'
            //                                Join cms.""N_PayrollHR_PayrollTransaction"" as t on t.""PersonId""=p.""Id""
            //                                Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=t.""ElementId"" and e.""ElementCode"" in ({searchModel.ElementCode})
            //                                Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id""
            //                                Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId""
            //                                Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //                                Join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id""
            //                                Join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId""
            //                                Join public.""User"" as u on u.""Id""=p.""UserId""
            //                                where p.""Id""='{searchModel.PersonId}' and t.""EffectiveDate""::DATE>='{searchModel.StartDate}'::DATE and t.""EffectiveDate""::DATE<='{searchModel.EndDate}'::DATE
            //                                order by a.""DateOfJoin"" ");

            var query = $@"Select t.*, p.""SponsorshipNo"", e.""ElementName"", e.""ElementCode"", u.""Id"" as UserId,pr.""YearMonth"" as YearMonth,
                       p.""Id"" as PersonId, p.""PersonNo"", d.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName,
                       sp.""Id"" as SponsorId, a.""DateOfJoin"" as EnrollDate, e.""ElementType"",p.""PersonFullName"" as PersonName
                       From cms.""N_CoreHR_HRPerson"" as p 
                       --Join public.""LegalEntity"" as le on le.""Id""=p.""LegalEntityId"" and le.""Id""='{_repo.UserContext.LegalEntityId}' and le.""IsDeleted""=false
                       Join cms.""N_PayrollHR_PayrollTransaction"" as t on t.""PersonId""=p.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                       Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=t.""ElementId"" and e.""ElementCode"" in ({searchModel.ElementCode}) and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                       Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                       Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                       Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                      left Join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                       left Join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'
                       Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_PayrollHR_PayrollRun"" as pr on t.""PayrollRunId""=pr.""Id"" and pr.""IsDeleted""=false  and pr.""CompanyId""='{_repo.UserContext.CompanyId}'                                          
                        where t.""EffectiveDate""::DATE>='{searchModel.StartDate}'::DATE and t.""EffectiveDate""::DATE<='{searchModel.EndDate}'::DATE 
                                  and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'     #WHERE#     order by a.""DateOfJoin"" ";

            var where = "";
            if (searchModel.PersonId.IsNotNullAndNotEmpty())
            {
                where = $@" and p.""Id""='{searchModel.PersonId}'";
            }
            query = query.Replace("#WHERE#", where);
            var result = await _queryPayReport.ExecuteQueryList(query, null);

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
            //var match = string.Concat(@"match(pel:PAY_PayrollElementRunResult)-[:R_PayrollElementRunResult_PayrollRunResult]->(prr:PAY_PayrollRunResult{IsDeleted:0,CompanyId:{CompanyId}})
            //match (prr)-[:R_PayrollRunResult_PersonRoot]->(per:HRS_PersonRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match (depr:HRS_DependentRoot{CompanyId:{CompanyId}})-[r:R_DependentRoot_PersonRoot]->(per)
            //match (dep:HRS_Dependent{CompanyId:{CompanyId}})-[:R_DependentRoot]->(depr)
            //match(ur:ADM_User{CompanyId:{CompanyId}})-[:R_User_PersonRoot]->(per)
            //match (per)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(orrr:HRS_OrganizationRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})            
            //match(per)<-[:R_PersonRoot]-(p:HRS_Person{ IsLatest:true,IsDeleted: 0,CompanyId: {CompanyId}})
            //match(pel)-[:R_PayrollElementRunResult_ElementRoot]->(elr:PAY_ElementRoot)
            //match(elr)<-[:R_ElementRoot]-(el:PAY_Element{IsLatest:true,IsDeleted:0,CompanyId:{CompanyId}})
           
            //optional match(per)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //optional match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,CompanyId:{CompanyId},IsLatest:true})
            //optional match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //optional match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
            //optional match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //optional match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
            //where (per.Id={PersonId} or {PersonId} is null) and pel.Month = {Month} and pel.Year = {Year} 
            //with pel, el, per, p, o, j,ur,dep
       
            //return pel, p.SponsorshipNo as SponsorshipNo,el.Name as ElementName,
            //o.Name as OrganizationName, j.Name as JobName,p.PersonNo as PersonNo,dep.Gender as Gender,dep.RelationshipType as RelationshipType,
            //el.ElementType As ElementType,", Helper.PersonDisplayName("p", " as PersonName "));

            var query = string.Concat($@"Select prr.*, p.""SponsorshipNo"", e.""ElementName"", d.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName,
                            p.""PersonNo"", glov.""Name"" as Gender, rlov.""Name"" as RelationshipType, e.""ElementType"", ", Helper.PersonDisplayName("p", " as PersonName "),
                            $@" From cms.""N_PayrollHR_PayrollRunResult"" as prr
                                Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=prr.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_CoreHR_HRDependant"" as dep on dep.""EmployeeId""=p.""Id"" and dep.""IsDeleted""=false and dep.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join public.""LOV"" as glov on glov.""Id""=dep.""GenderId"" and glov.""IsDeleted""=false and glov.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join public.""LOV"" as rlov on rlov.""Id""=dep.""RelationshipTypeId"" rlov.""IsDeleted""=false and rlov.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pel.""PayrollElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Left Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Left Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Left Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                                where prr.""PersonId""='{personId}' and prr.""Month""='{month}' and prr.""Year""='{Year}' and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'");

            var result = await _queryPayReport.ExecuteQueryList(query, null);           
            return result;
        }

        public async Task<IList<PayrollReportViewModel>> GetBankDetails(PayrollReportViewModel searchModel)
        {
            //throw new NotImplementedException();
       //     var match = string.Concat(@"
       //         match(sir:PAY_SalaryInfoRoot)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
       //         match (pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(orrr:HRS_OrganizationRoot{Id:{LegalEntity} ,IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})            
       //         match(sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId},IsLatest:true })
       //         match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsLatest:true,IsDeleted: 0,CompanyId: { CompanyId}})
       //         match(si)-[r:R_SalaryInfo_BankBranch]->(pb:PAY_BankBranch{IsDeleted:0, CompanyId: { CompanyId}})
       //         optional match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,CompanyId:{CompanyId}})
       //         optional match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,CompanyId:{CompanyId},IsLatest:true})
       //         optional match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{ IsDeleted:0,CompanyId:{CompanyId}})
			    //optional match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})

       //         optional match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,CompanyId:{CompanyId}})
			    //optional match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
       //         with si,p,o,j,pr, pb
       //         where pr.Id={PersonId} or {PersonId} is null
       //         return p.SponsorshipNo as SponsorshipNo, p.PersonNo as PersonNo, o.Name as OrganizationName, j.Name as JobName, pb.Name as BankName, 
       //         si.BankAccountNo as BankAccountNo, si.BankIBanNo as BankIBanNo, si.PaymentMode as PaymentMode, si.SalaryTransferLetterProvided as SalaryTransferLetterProvided,"
       //         , Helper.PersonDisplayName("p", " as PersonName "));

            var query = string.Concat($@"Select p.""SponsorshipNo"", p.""PersonNo"", d.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, pb.""BankName"",
                            si.""BankAccountNumber"" as BankAccountNo, si.""BankIBanNumber"" as BankIBanNo, si.""PaymentMode"" as PaymentMode, si.""IsEligibleForSalaryTransferLetter"" as SalaryTransferLetterProvided, ",
                            Helper.PersonDisplayName("p", " as PersonName "),
                            $@" From cms.""N_CoreHR_HRPerson"" as p
                                Join public.""LegalEntity"" as le on le.""Id""=p.""PersonLegalEntityId"" and le.""Id""='{_repo.UserContext.LegalEntityId}' and le.""IsDeleted""=false and le.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=p.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_PayrollHR_BankBranch"" as br on br.""Id""=si.""BankBranchId"" and br.""IsDeleted""=false and br.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_PayrollHR_PayrollBank"" as pb on pb.""Id""=br.""BankId"" and pb.""IsDeleted""=false and pb.""CompanyId""='{_repo.UserContext.CompanyId}'
                                --Join public.""LOV"" as pmlov on pmlov.""Id""=si.""PaymentModeId"" and pmlov.""IsDeleted""=false and pmlov.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Left Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Left Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Left Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                                where p.""Id""='{searchModel.PersonId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'");

            var result = await _queryPayReport.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<string[]> GetDistinctElement(DateTime value)
        {            
            //var match = string.Concat(@"match (sei:PAY_SalaryElementInfo{IsDeleted:0})
            //where sei.EffectiveStartDate<={ESD}<=sei.EffectiveEndDate
            //match (sei)-[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot)<-[:R_ElementRoot]-(e:PAY_Element{ElementCategory:'Standard'})
            //where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate
            //return distinct e.Name order by e.Name");

            var query = $@"Select distinct e.""ElementName"" as Name from cms.""N_PayrollHR_PayrollElement"" as e
                            join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""ElementId"" = e.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                            and sei.""EffectiveStartDate""::DATE<='{value}'::DATE and '{value}'::DATE<=sei.""EffectiveEndDate""::DATE
                            where e.""ElementCategory"" = 'Standard' and e.""IsDeleted"" = false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            and e.""EffectiveStartDate""::DATE<='{value}'::DATE and '{value}'::DATE<=e.""EffectiveEndDate""::DATE order by e.""ElementName"" ";

            var querydata = await _queryEle.ExecuteQueryList(query, null);
            string[] result = querydata.Select(x => x.Name).ToArray();
            return result;
        }

        public async Task<string[]> GetDistinctElement()
        {            
            //var match = string.Concat(@"match (sei:PAY_SalaryElementInfo{IsDeleted:0,IsLatest:true})           
            //match (sei)-[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot)<-[:R_ElementRoot]-(e:PAY_Element{ElementCategory:'Standard',IsLatest:true})           
            //return distinct e.Name order by e.Name");

            var query = $@"Select distinct e.""ElementName"" as Name from cms.""N_PayrollHR_PayrollElement"" as e
                            join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""ElementId"" = e.""Id"" and sei.""IsDeleted""=false  and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where e.""ElementCategory"" = 'Standard' and e.""IsDeleted"" = false and e.""CompanyId""='{_repo.UserContext.CompanyId}' order by e.""ElementName"" ";

            var querydata = await _queryEle.ExecuteQueryList(query, null);
            string[] result = querydata.Select(x => x.Name).ToArray();
            return result;
        }

        public async Task<IList<PayrollReportViewModel>> GetLoanAccuralDetails(PayrollReportViewModel searchModel)
        {            
            //var match = string.Concat(@"match (per:HRS_PersonRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match (per)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(orrr:HRS_OrganizationRoot{Id:{LegalEntity} ,IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})            
            //match(per)<-[:R_PersonRoot]-(p:HRS_Person{ IsLatest:true,IsDeleted: 0,CompanyId: {CompanyId}})   
            //match(per)<-[:R_PayrollTransaction_PersonRoot]-(t:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId}}) 
            //-[:R_PayrollTransaction_ElementRoot]->(elr:PAY_ElementRoot)            
            //match(elr)<-[:R_ElementRoot]-(el:PAY_Element{IsLatest:true,IsDeleted:0,CompanyId:{CompanyId}})
            //where el.Code in [", searchModel.ElementCode, @"]
            //match(per)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,CompanyId:{CompanyId},IsLatest:true})
            //-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
            //match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,IsLatest:true,CompanyId:{CompanyId}})
            //match (per)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0})
            //<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0,IsLatest:true})
            //-[:R_Contract_Sponsor]->(sp:HRS_Sponsor{ IsDeleted: 0,CompanyId: { CompanyId} })  
            //match(per)<-[:R_User_PersonRoot]-(u:ADM_User)
            //optional match(per)<-[:R_PayrollTransaction_PersonRoot]-(lt:PAY_PayrollTransaction)
            //-[:R_PayrollTransaction_ElementRoot]->(ler:PAY_ElementRoot)<-[:R_ElementRoot]-(le:PAY_Element{Code:'LOAN'})
            //with t, el, per, p, o, j,sp,a,u,max(lt.EffectiveDate) as EnrollDate
            //where (per.Id={PersonId} or {PersonId} is null) and {SD}<=t.EffectiveDate<={ED}
            //return t, p.SponsorshipNo as SponsorshipNo,el.Name as ElementName,el.Code as ElementCode,u.Id as UserId,
            //o.Name as OrganizationName, j.Name as JobName,p.PersonNo as PersonNo,sp.Id as SponsorId, EnrollDate,
            //el.ElementType As ElementType,", Helper.PersonDisplayName("p", " as PersonName "), " order by a.DateOfJoin");

            var query = $@"Select 
t.""NtsNoteId"", t.""EffectiveDate"",t.""DeductionAmount"", t.""ClosingBalance"", t.""DeductionQuantity"", t.""ClosingQuantity"",
t.""EarningAmount"", t.""OpeningBalance"", t.""EarningQuantity"", t.""OpeningQuantity"", t.""Description"",
t.""ProcessStatusId"", t.""Quantity"",t.""IsTransactionClosed"",pr.""YearMonth"" as YearMonth,

p.""SponsorshipNo"", e.""ElementName"", e.""ElementCode"", u.""Id"" as UserId, d.""DepartmentName"" as OrganizationName,
                            j.""JobTitle"" as JobName, p.""PersonNo"", sp.""Id"" as SponsorId, max(pt.""EffectiveDate"") as EnrollDate, e.""ElementType"", p.""PersonFullName""  as PersonName  
                                From cms.""N_CoreHR_HRPerson"" as p
                               -- Join public.""LegalEntity"" as le on le.""Id""=p.""PersonLegalEntityId"" and le.""Id""='{_repo.UserContext.LegalEntityId}' and le.""IsDeleted""=false
                                Join cms.""N_PayrollHR_PayrollTransaction"" as t on t.""PersonId""=p.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=t.""ElementId"" and e.""ElementCode"" in ({searchModel.ElementCode}) and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                                left Join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id""  and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                                left Join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Left Join cms.""N_PayrollHR_PayrollTransaction"" as pt on pt.""PersonId""=p.""Id"" and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Left Join cms.""N_PayrollHR_PayrollElement"" as el on el.""Id""=t.""ElementId"" and el.""ElementCode"" in ('LOAN') and el.""IsDeleted""=false
left join cms.""N_PayrollHR_PayrollRun"" as pr on t.""PayrollRunId""=pr.""Id"" and pr.""IsDeleted""=false   and pr.""CompanyId""='{_repo.UserContext.CompanyId}'                                    
where  '{searchModel.StartDate}'::DATE<=t.""EffectiveDate""::DATE and t.""EffectiveDate""::DATE<='{searchModel.EndDate}'
                                 and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'        #WHERE#
group by 
t.""NtsNoteId"", t.""EffectiveDate"",t.""DeductionAmount"", t.""ClosingBalance"", t.""DeductionQuantity"", t.""ClosingQuantity"",
t.""EarningAmount"", t.""OpeningBalance"", t.""EarningQuantity"", t.""OpeningQuantity"", t.""Description"",
t.""ProcessStatusId"", t.""Quantity"",t.""IsTransactionClosed"",
p.""SponsorshipNo"", e.""ElementName"", e.""ElementCode"", u.""Id"", d.""DepartmentName"",
                            j.""JobTitle"", p.""PersonNo"", sp.""Id"", 
							e.""ElementType"", p.""PersonFullName"" ,pr.""YearMonth""

";

            var where = "";
            if (searchModel.PersonId.IsNotNullAndNotEmpty())
            {
                where = $@" and p.""Id""='{searchModel.PersonId}'";
            }
            query = query.Replace("#WHERE#", where);
            var result = await _queryPayReport.ExecuteQueryList(query, null);
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

            var query = $@"Select sei.*, d.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, ps.""Amount"", e.""ElementName"", e.""Id"" as ElementId,
                            p.""Id"" as PersonId, p.""SponsorshipNo"", p.""PersonNo"", p.""PersonFullName"" as PersonName
                            From cms.""N_PayrollHR_SalaryInfo"" as si
                            Join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                            and sei.""EffectiveStartDate""::DATE>='{searchModel.FromDate}'::DATE and se.""EffectiveStartDate""::DATE<='{searchModel.ToDate}'::DATE
                            Join cms.""N_CoreHR_HRPerson"" as p p.""Id""=si.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""LegalEntity"" as le on le.""Id""=p.""PersonLegalEntityId"" and le.""Id""='{_repo.UserContext.LegalEntityId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" and e.""ElementCategory""='Standard' and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where si.""PersonId""='{searchModel.PersonId}' and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}' order by si.""PersonId"", e.""Id"" ";

            var result = await _queryPayReport.ExecuteQueryList(query, null);
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

                var query1 = $@"Select sei.*, d.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, ps.""Amount"", e.""ElementName"", e.""Id"" as ElementId,
                            p.""Id"" as PersonId, p.""SponsorshipNo"", p.""PersonNo"", p.""PersonFullName"" as PersonName
                            From cms.""N_PayrollHR_SalaryInfo"" as si
                            Join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""EffectiveEndDate""='{p.EffectiveDate.Value.AddDays(-1)}' and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""LegalEntity"" as le on le.""Id""=p.""PersonLegalEntityId"" and le.""Id""='{_repo.UserContext.LegalEntityId}' and le.""IsDeleted""=false and le.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" and e.""ElementCategory""='Standard' and e.""Id""='{p.ElementId}' and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and  j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where si.""PersonId""='{searchModel.PersonId}' and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}' order by si.""PersonId"", e.""Id"" ";

                var querydata = await _queryPayReport.ExecuteQueryList(query1, null);
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

            string query = $@"Select si.*, d.""DepartmentName"" as OrganizationName, J.""JobTitle"" as JobName, sei.""Amount"", e.""ElementName"", pe.""Id"" as ElementId,
                            p.""Id"" as PersonId, p.""SponsorshipNo"", p.""PersonNo"", p.""PersonFullName"" as PersonName
                            From cms.""N_PayrollHR_SalaryInfo"" as si
                            Join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" and e.""ElementCategory""='Standard' and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""LegalEntity"" as le on le.""Id""=p.""PersonLegalEntityId"" and le.""Id""='{_repo.UserContext.LegalEntityId}' and le.""IsDeleted""=false and le.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where si.""PersonId""='{searchModel.PersonId}' and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}' "; 

            var result1 = await _queryPayReport.ExecuteQueryList(query, null);
           
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
            //var match = string.Concat(@"match(or: HRS_OrganizationRoot{Id:{LegalEntity}})<-[:R_PayrollGroup_LegalEntity_OrganizationRoot]
            //    -(pg:PAY_PayrollGroup{ IsDeleted: 0,CompanyId: { CompanyId} }) 
            //    match(pg)<-[:R_SalaryInfo_PayrollGroup]-(ps:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId},Status:{Status} })                  
            //    where ps.EffectiveStartDate<={ESD}<=ps.EffectiveEndDate
            //    match(ps)-[:R_SalaryInfoRoot]->(psr:PAY_SalaryInfoRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //    match(psr)-[:R_SalaryInfoRoot_PersonRoot] ->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //      match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted: 0,CompanyId: { CompanyId}})
            //    where p.IsLatest=true     and      pr.Id={PersonId}                             
            //    return ps.Id");

            var query = $@"Select ps.""Id"" from cms.""N_PayrollHR_SalaryInfo"" as ps
                            join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id"" = ps.""PayGroupId"" and pg.""LegalEntityId"" = '{_repo.UserContext.LegalEntityId}' and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRPerson"" as p on p.""Id"" = ps.""PersonId"" and p.""Id"" = '{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where ps.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and '{DateTime.Today}'::DATE<=ps.""EffectiveEndDate""::DATE  and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var result = await _salaryInfo.ExecuteScalar<string>(query, null);            
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

            var query1 = string.Concat($@"{query} Select p.""Id"",si.""PersonId"", ", Helper.PersonDisplayNameWithSponsorshipNo("po", " as PersonName "), 
                                        $@" From cms.""N_CoreHR_HRAssignment"" as a
                                            Join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""Id""=""Department"".""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=a.""EmployeeId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                                            left Join cms.""N_PayrollHR_SalaryInfo"" as si on p.""Id""=si.""PersonId"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                                            where si.""PersonId"" is null and a.""EffectiveStartDate""::TIMESTAMP::DATE <='{DateTime.Today.Date}'::TIMESTAMP::DATE and a.""EffectiveEndDate""::TIMESTAMP::DATE>='{DateTime.Today.Date}'::TIMESTAMP::DATE  and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'");
                                    
            if (excludePersonId.IsNotNullAndNotEmpty())
            {
                //match = string.Concat(match, " Union MATCH(p: HRS_PersonRoot)<-[:R_PersonRoot]-(po: HRS_Person{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId}" +
                //    " }) WHERE ()-[:R_SalaryInfoRoot_PersonRoot]->(p) and p.Id ={PersonId} and (po.EffectiveStartDate <= { ESD} and po.EffectiveEndDate >= { EED}) " +
                //    "return p.Id as Id,", Helper.PersonDisplayNameWithSponsorshipNo("po", " as PersonName "));

                query1 = string.Concat(query1, $@" Union Select p.""Id"", si.""PersonId"", ", Helper.PersonDisplayNameWithSponsorshipNo("po", " as PersonName "),
                                                $@" From cms.""N_CoreHR_HRPerson"" as p
                                                    Left Join cms.""N_PayrollHR_SalaryInfo"" as si on p.""Id""=si.""PersonId"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                                                    where p.""Id""='{excludePersonId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'");
            }
            var result = await _salaryInfo.ExecuteQueryList(query1, null);
            
            return result.ToList();

        }

        public async Task<IList<VM>> ViewModelList<VM>(string cypherWhere = "", Dictionary<string, object> parameters = null, string returnValues = "")
        {
            //throw new NotImplementedException();
            //parameters.AddIfNotExists("Status", StatusEnum.Active);
            //parameters.AddIfNotExists("CompanyId", CompanyId);
            //parameters.AddIfNotExists("ESD", DateTime.Today);
            //parameters.Add("LegalEntityId", _repo.UserContext.LegalEntityId);
            //var match = string.Concat(@"match(or: HRS_OrganizationRoot{Id:{LegalEntityId}})<-[:R_PayrollGroup_LegalEntity_OrganizationRoot]
            //    -(pg:PAY_PayrollGroup{ IsDeleted: 0,CompanyId: { CompanyId} }) 
            //    match(pg)<-[:R_SalaryInfo_PayrollGroup]-(ps:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId},Status:{Status} })                  
            //    match(ps)-[:R_SalaryInfoRoot]->(psr:PAY_SalaryInfoRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //    match(psr)-[:R_SalaryInfoRoot_PersonRoot] ->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })

            //    optional match(ps)-[:R_SalaryInfo_PayCalendar]->(pc:PAY_Calendar{ IsDeleted: 0,CompanyId: { CompanyId},Status:{Status} })  
            //    optional match(ps)-[:R_SalaryInfo_BankBranch]->(pb:PAY_BankBranch{ IsDeleted: 0,CompanyId: { CompanyId},Status:{Status} })  
            //    optional match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted: 0,CompanyId: { CompanyId} })
            //    where p.IsLatest=true            
            //    with psr,ps,pr,pb,p,pg,pc");

            var query = $@" From cms.""N_PayrollHR_PayrollGroup"" as pg
                            Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PayGroupId""=pg.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""Id""=si.""PayCalendarId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_PayrollHR_BankBranch"" as br on br.""Id""=si.""BankBranchId"" and br.""IsDeleted""=false and br.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pg.""LegalEntityId""='{_repo.UserContext.LegalEntityId}' and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            if (cypherWhere.IsNotNullAndNotEmpty())
            {
                cypherWhere = string.Concat(@" where ", cypherWhere);
            }
            if (returnValues.IsNullOrEmpty())
            {
                //returnValues = @" return c,cr.Id as PersonSalaryInfoId,pr.Id as PersonId,(p.FirstName + p.LastName + p.SponsorshipNo) as PersonName,sp.Id as SponsorId,sp.Name as SponsorName";
                returnValues = string.Concat(@" Select si.*,si.""Id"" as SalaryInfoId,p.""Id"" as PersonId, p.""PersonNo"", p.""SponsorshipNo"" as SponsorshipNo, ", Helper.PersonDisplayName("p", " as PersonName,"), $@" br.""Id"" as BankBranchId, br.""BranchName"" as BankBranchName, pg.""Id"" as PayGroupId, pc.""Id"" as PayCalendarId ");
            }
            var cypher = string.Concat(returnValues, query, cypherWhere);

            var result = await _queryPayPerson.ExecuteScalarList<VM>(cypher, null);
            return result;
            //if (parameters == null)
            //{
            //    return ExecuteCypherList<VM>(cypher);
            //}
            //else
            //{
            //    return ExecuteCypherList<VM>(cypher, parameters);
            //}
        }
        public async Task<SalaryInfoViewModel> GetEligiblityForTickets(string UserId)
        {
            //var cypher = string.Concat(@"
            //match(si:PAY_SalaryInfo{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})-
            //[:R_SalaryInfoRoot]->(sir:PAY_SalaryInfoRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //where  si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate
            //match (sir)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{Id}})
            //return si");
            var cypher = string.Concat($@"select si.* from
cms.""N_PayrollHR_SalaryInfo"" as si
join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=si.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=pr.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{UserId}' and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
");

            //    var prms = new Dictionary<string, object>
            //    {
            //        { "CompanyId", CompanyId },
            //        { "Status", StatusEnum.Active.ToString() },
            //        { "Id", UserId },
            //        { "ESD", DateTime.Now.ApplicationNow().Date },
            //        { "EED", DateTime.Now.ApplicationNow().Date }
            //};
            //return ExecuteCypher<SalaryInfoViewModel>(cypher, prms);
            return await _salaryInfo.ExecuteQuerySingle<SalaryInfoViewModel>(cypher, null);
        }




        
    }
}
