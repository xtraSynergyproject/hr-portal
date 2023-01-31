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
    public class PayrollRunResultBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollRunResultBusiness
    {
        private readonly IRepositoryQueryBase<PayrollRunResultViewModel> _payrunrepo;
        private readonly IRepositoryQueryBase<ElementViewModel> _elemetRepo;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;
        private readonly IPayrollRunBusiness _payrollRunBusiness;
        private readonly IRepositoryQueryBase<PayrollSalaryElementViewModel> _paysalelerepo;
        private readonly IRepositoryQueryBase<SalaryElementEntryViewModel> _repsalEleEntry;
        INoteBusiness _noteBusiness;
        public PayrollRunResultBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IRepositoryQueryBase<PayrollRunResultViewModel> payrunrepo,
            IMapper autoMapper, IRepositoryQueryBase<ElementViewModel> elemetRepo, IPayrollBatchBusiness payrollBatchBusiness,
            IPayrollRunBusiness payrollRunBusiness, IRepositoryQueryBase<PayrollSalaryElementViewModel> paysalelerepo,
            IRepositoryQueryBase<SalaryElementEntryViewModel> repsalEleEntry,
            INoteBusiness noteBusiness) : base(repo, autoMapper)
        {
            _payrunrepo = payrunrepo;
            _elemetRepo = elemetRepo;
            _payrollBatchBusiness = payrollBatchBusiness;
            _payrollRunBusiness = payrollRunBusiness;
            _paysalelerepo = paysalelerepo;
            _repsalEleEntry = repsalEleEntry;
            _noteBusiness = noteBusiness;
        }

        public async override Task<CommandResult<NoteViewModel>> Create(NoteViewModel model)
        {
            var data = _autoMapper.Map<NoteViewModel>(model);                       
           
            var result = await base.Create(data);

            return CommandResult<NoteViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteViewModel>> Edit(NoteViewModel model)
        {
            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<NoteViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        public async Task<string[]> GetDistinctElement(string payrollRunId, ElementCategoryEnum? elementCategory)
        {
            //var match1 = string.Concat(@" match (pay:PAY_PayrollRun{Id:{payrollRunId}})<-[:R_PayrollRunResult_PayrollRun]
            //    -(par:PAY_PayrollRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //  match (par)<-[:R_PayrollElementRunResult_PayrollRunResult]-(prer:PAY_PayrollElementRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})  
            //   match (prer)-[:R_PayrollElementRunResult_ElementRoot]->(er:PAY_ElementRoot)
            //<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: {CompanyId} })
            //   where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate and e.ElementType<>'Accrual'

            //    with par,prer,e,er ", elementCategory == null ? "" : " where e.ElementCategory={ElementCategory} ", @"
            //    with distinct e.Name as Name,e.ElementClassification as ElementClassification,e.SequenceNo as SequenceNo
            //    return Name order by ElementClassification desc,SequenceNo,Name");

            var query = $@"select distinct pe.""ElementName"" as ""Name"" from cms.""N_PayrollHR_PayrollRun"" as pr
                            join cms.""N_PayrollHR_PayrollRunResult"" as pres on pres.""PayrollRunId"" = pr.""Id"" and pres.""IsDeleted""=false and pres.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElementRunResult"" as per on per.""PayrollRunResultId"" = pres.""Id"" and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElement"" as pe on pe.""Id"" = per.""PayrollElementId"" and pe.""IsDeleted""=false and pe.""CompanyId""='{_repo.UserContext.CompanyId}' and pe.""ElementType""!='2'
                            where pres.""PayrollRunId""='{payrollRunId}'  and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            #ElementWhere#
                        ";

            var where = "";
            if (elementCategory.IsNotNull())
            {
                int ec = (int)elementCategory;
                where = $@"and pe.""ElementCategory""='{ec}'";
            }            
            query = query.Replace("#ElementWhere#", where);

            var result =  await _elemetRepo.ExecuteQueryList(query, null);
            string[] stringarr = result.Select(x=>x.Name).ToArray();
            return stringarr;
        }
        public async Task<IList<PayrollSalaryElementViewModel>> GetPaySalaryElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory)
        {              

            var payrollRunViewModel = await _payrollRunBusiness.ViewModelList(payrollRunId);
            var payrollRunModel = payrollRunViewModel.FirstOrDefault();
            
            var payrollBatch = await _payrollBatchBusiness.GetSingleById(payrollRunModel.PayrollBatchId);

            payrollBatch.AttendanceStartDate = payrollRunModel.AttendanceStartDate;
            payrollBatch.AttendanceEndDate = payrollRunModel.AttendanceEndDate;

            //var prms = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "PayrollRunId", payrollRunId },
            //    { "PayrollId", payrollRunViewModel.PayrollId },
            //    { "payrollGroupId", payrollRunViewModel.PayrollGroupId },
            //    { "ESD", payroll.PayrollEndDate},
            //    { "yearMonth", payrollRunViewModel.YearMonth},
            //    { "startDate",  payroll.AttendanceStartDate },
            //    { "endDate", payroll.AttendanceEndDate},
            //};

            //var match = string.Concat(@"match(payr:PAY_PayrollRun{Id:{PayrollRunId},IsDeleted: 0})
            //    <-[:R_PayrollRunResult_PayrollRun]-(prr:PAY_PayrollRunResult{IsDeleted: 0})
            //    -[:R_PayrollRunResult_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0 })<-[:R_PersonRoot]-(p:HRS_Person)
            //    where p.EffectiveStartDate<={ESD}<=p.EffectiveEndDate

            //    match(pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]-(leor:HRS_OrganizationRoot{ IsDeleted:0,Status:'Active'})
            //    <-[:R_OrganizationRoot]-(leo:HRS_Organization{ IsDeleted:0,Status:'Active'})
            //    where leo.EffectiveStartDate <= {ESD} <= leo.EffectiveEndDate 

            //    match (pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0,Status:'Active'})
            //    match(cr)<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0})
            //    where c.EffectiveStartDate<={ESD}<=c.EffectiveEndDate
            //    match(c)-[:R_Contract_Sponsor]->(s:HRS_Sponsor{IsDeleted:0})
            //    match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{IsDeleted:0,Status:'Active'})
            //    match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0})
            //    where a.EffectiveStartDate <= {ESD} <=a.EffectiveEndDate           
            //    match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,Status:'Active'})
            //    match(jr)<-[:R_JobRoot]-(j:HRS_Job{IsDeleted:0,Status:'Active'})
            //    where j.EffectiveStartDate <= {ESD} <= j.EffectiveEndDate              
            //    match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{IsDeleted:0,Status:'Active'})
            //    match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,Status:'Active'})
            //    where o.EffectiveStartDate <= {ESD} <= o.EffectiveEndDate 
            //    match(a)-[:R_Assignment_GradeRoot]->(gr:HRS_GradeRoot{ IsDeleted:0,Status:'Active'})
            //    match(gr)<-[:R_GradeRoot]-(g:HRS_Grade{ IsDeleted:0,Status:'Active'})
            //    where g.EffectiveStartDate <= {ESD} <= g.EffectiveEndDate 
            //    match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,CompanyId:{CompanyId}})
            //    optional match(o)-[:R_Organization_Payroll_OrganizationRoot]-(payor:HRS_OrganizationRoot{ IsDeleted:0,Status:'Active'})
            //    <-[:R_OrganizationRoot]-(payo:HRS_Organization{ IsDeleted:0,Status:'Active'})
            //    where payo.EffectiveStartDate <= {ESD} <= payo.EffectiveEndDate 
            //    with prr,pr,p,ar,a,jr,j,orr,o,payor,payo,g,leo,u,s
            //    return prr,o.Name as OrganizationName,payo.Name as PayrollOrganizationName,j.Name as JobName,a.DateOfJoin as DateOfJoin,s.Id as SponsorId,s.Name as SponsorName
            //    ,leo.Name as LegalEntityName,u.Id as UserId, p.PersonNo as PersonNo, p.SponsorshipNo as SponsorshipNo
            //    ,g.Name as GradeName,pr.Id as PersonId,", Helper.PersonDisplayName("p", " as PersonName")
            //    , " order by p.PersonNo");
            //var employees = ExecuteCypherList<PaySalaryElementViewModel>(match, prms);

            var match = $@"Select prr.*, d.""DepartmentName"" as OrganizationName, payd.""DepartmentName"" as PayrollOrganizationName, j.""JobTitle"" as JobName,
                            a.""DateOfJoin"", s.""Id"" as SponsorId, s.""SponsorName"" as SponsorName, u.""Id"" as UserId,
                            p.""PersonNo"", g.""GradeName"", p.""Id"" as PersonId, p.""PersonFullName"" as PersonName
                            From cms.""N_PayrollHR_PayrollRun"" as payr
                            Join cms.""N_PayrollHR_PayrollRunResult"" as prr on prr.""PayrollRunId""=payr.""Id"" and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=prr.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            
                            Join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRSponsor"" as s on s.""Id""=c.""SponsorId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false and g.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join cms.""N_CoreHR_HRDepartment"" as payd on payd.""Id""=d.""PayrollDepartmentId"" and payd.""IsDeleted""=false and payd.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where payr.""Id""='{payrollRunId}' and payr.""IsDeleted""=false and payr.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var employees = await _paysalelerepo.ExecuteQueryList(match, null);

            var persons = employees.Select(x => x.PersonId);
            var pers = string.Join("','", persons.ToArray());
            

            var match1 = $@"Select pr.""Id"" as PersonId, e.""ElementName"", e.""ElementDisplayName"", sei.""Amount""
                           From cms.""N_CoreHR_HRPerson"" as pr
                           join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                           join public.""NtsNote"" as nts on nts.""ParentNoteId""=si.""NtsNoteId""  and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
                           join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=nts.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                           join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                           --and e.""EffectiveStartDate""::TIMESTAMP::DATE<='{payrollBatch.PayrollEndDate}'::TIMESTAMP::DATE and e.""EffectiveEndDate""::TIMESTAMP::DATE >='{payrollBatch.PayrollEndDate}'::TIMESTAMP::DATE
                           where pr.""Id"" in ('{pers}') and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' order by pr.""Id"", e.""ElementName"" ";

            var result1 = await _repsalEleEntry.ExecuteQueryList(match1, null);


            var runElementList = await _payrollRunBusiness.GetStandardElementListForPayrollRunData
                (payrollBatch, payrollRunModel.PayrollGroupId, payrollRunModel.PayrollBatchId, payrollRunId, payrollRunModel.YearMonth.Value);

            var distinctElement = runElementList.Select(x => x.Name).Distinct();// _salaryInfoBusiness.GetDistinctElement(DateTime.Now);//result1.DistinctBy(x=>x.ElementId);
            foreach (var item in employees)
            {
                int i = 1;
                var netAmount = 0.0;
                foreach (var item3 in distinctElement)
                {
                    var e = result1.FirstOrDefault(x => x.PersonId == item.PersonId && x.ElementName == item3);
                    var Col = string.Concat("Element", i);
                    if (e != null)
                    {
                        ApplicationExtension.SetPropertyValue(item, Col, e.Amount);
                        netAmount += e.Amount.Value;
                    }
                    else
                    {
                        ApplicationExtension.SetPropertyValue(item, Col, 0);
                    }
                    i++;
                }
                item.GrossSalary = netAmount.RoundPayrollSummaryAmount();
            }
            

            var match2 = $@"Select prer.*, e.""ElementCode"" as ElementCode, pr.""Id"" as PersonId, e.""ElementName"" as Name, coalesce(e.""ElementDisplayName"",e.""ElementName"") as DisplayName,
                            e.""Id"" as ElementId, par.""Id"" as PayrollRunResultId
                            From cms.""N_PayrollHR_PayrollRun"" as pay
                            join cms.""N_PayrollHR_PayrollRunResult"" as par on par.""PayrollRunId""=pay.""Id"" and par.""IsDeleted""=false and par.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=par.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElementRunResult"" as prer on prer.""PayrollRunResultId""=par.""Id"" and prer.""IsDeleted""=false and prer.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=prer.""PayrollElementId"" and e.""IsDeleted""=false and e.""ElementType"" <> 'Accrual'  and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            --and e.""EffectiveStartDate""::TIMESTAMP::DATE<='{DateTime.Now.Date}' and e.""EffectiveEndDate""::TIMESTAMP::DATE>='{DateTime.Now.Date}' 
#ELECATWHERE#
                            where pay.""Id""='{payrollRunId}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}' order by pr.""Id"", e.""ElementClassification"" desc ,e.""ElementName"" ";

            var elecatwhere = "";
            if (elementCategory.IsNotNull())
            {
                elecatwhere = $@" and e.""ElementCategory""='{elementCategory}' ";
            }
            match2 = match2.Replace("#ELECATWHERE#", elecatwhere);

            var salaryElementList = await _paysalelerepo.ExecuteQueryList<PayrollElementRunResultViewModel>(match2, null);


            var distinctSalaryElement = await GetDistinctElementDisplayName(payrollRunId, elementCategory);//result1.DistinctBy(x => x.ElementId);
            foreach (var item in employees)
            {
                int i = 1;
                double netAmount = 0;
                double netPayable = 0;

                foreach (var item3 in distinctSalaryElement)
                {
                    var Col = string.Concat("SalaryElement", i);
                    var e = salaryElementList.Where(x => x.PersonId == item.PersonId && x.DisplayName == item3);

                    if (e != null && e.Any())
                    {
                        var amount = e.Sum(x => x.Amount);
                        ApplicationExtension.SetPropertyValue(item, Col, amount);
                        netAmount += amount;
                        if (item3 != "GOSI Company Contribution")
                        {
                            netPayable += amount;
                        }
                    }
                    else
                    {
                        ApplicationExtension.SetPropertyValue(item, Col, 0);
                    }
                    i++;
                }
                item.NetAmount = netAmount.RoundPayrollSummaryAmount();
                item.NetPayableAmount = netPayable.RoundPayrollSummaryAmount();
            }

            return employees;
        }


        public async Task<string[]> GetDistinctElementDisplayName(string payrollRunId, ElementCategoryEnum? elementCategory)
        {
            //var match1 = string.Concat(@" match (pay:PAY_PayrollRun{Id:{payrollRunId}})<-[:R_PayrollRunResult_PayrollRun]
            //    -(par:PAY_PayrollRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //  match (par)<-[:R_PayrollElementRunResult_PayrollRunResult]-(prer:PAY_PayrollElementRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})  
            //   match (prer)-[:R_PayrollElementRunResult_ElementRoot]->(er:PAY_ElementRoot)
            //<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: {CompanyId} })
            //   where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate and e.ElementType<>'Accrual'
                
            //    with par,prer,e,er ", elementCategory == null ? "" : " where e.ElementCategory={ElementCategory} ", @"
            //    with distinct coalesce(e.DisplayName,e.Name) as Name,e.ElementClassification as ElementClassification,e.SequenceNo as SequenceNo
            //    return Name order by ElementClassification desc,SequenceNo,Name");

            //var prms1 = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "payrollRunId", payrollRunId },
            //    { "ESD", DateTime.Now},
            //    { "ElementCategory", elementCategory}
            //};

            var query = $@"Select Distinct coalesce(e.""ElementDisplayName"",e.""ElementName"") as ElementName, e.""ElementClassification""
                            From cms.""N_PayrollHR_PayrollRunResult"" as par
                            join cms.""N_PayrollHR_PayrollElementRunResult"" as prer on prer.""PayrollRunResultId""=par.""Id"" and prer.""IsDeleted""=false and prer.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=prer.""PayrollElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}' and e.""ElementType""<>'Accrual' 
                           -- and e.""EffectiveStartDate""<='{DateTime.Now}' and e.""EffectiveEndDate"">='{DateTime.Now}'
#ELECATWHERE#
                            where par.""PayrollRunId""='{payrollRunId}' and par.""IsDeleted""=false and par.""CompanyId""='{_repo.UserContext.CompanyId}' order by e.""ElementClassification"" desc ";

            var elecatwhere = "";
            if (elementCategory.IsNotNull())
            {
                elecatwhere = $@" and e.""ElementCategory""='{elementCategory}' ";
            }
            query = query.Replace("#ELECATWHERE#", elecatwhere);

            var queryresult = await _paysalelerepo.ExecuteQueryList(query, null);
            string[] result = queryresult.Select(x=>x.ElementName).ToArray();
            return result;
        }

        public async Task<List<PayrollDetailViewModel>> GetPayollDetailList(string payrollRunId, ElementCategoryEnum? elementCategory, int? yearMonth)
        {
            var d1 = 1;
            var d2 = 2;
            var d3 = 3;
            var d4 = 4;
            var d5 = 5;
            var d6 = 6;
            var d7 = 7;
            var d8 = 8;
            var d9 = 9;
            var d10 = 10;
            var d11 = 11;
            var d12 = 12;
            var d13 = 13;
            var d14 = 14;
            var d15 = 15;
            var d16 = 16;
            var d17 = 17;
            var d18 = 18;
            var d19 = 19;
            var d20 = 20;
            var d21 = 21;
            var d22 = 22;
            var d23 = 23;
            var d24 = 24;
            var d25 = 25;
            var d26 = 26;
            var d27 = 27;
            var d28 = 28;
            var d29 = 29;
            var d30 = 30;
            var d31 = 31;

            var search = "";
            var where = "";
            if (payrollRunId.IsNullOrEmptyOrWhiteSpace())
            {
                search = string.Concat(
               $@"from cms.""N_PayrollHR_PayrollBatch"" as pr
join cms.""N_PayrollHR_PayrollRun"" as pay on pay.""PayrollBatchId""=pr.""Id"" and pay.""YearMonth""='{yearMonth}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRunResult"" as par on par.""PayrollRunId""=pay.""Id"" and par.""IsDeleted""=false and par.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LegalEntity"" as or on or.""Id""=pr.""LegalEntityId"" and or.""IsDeleted""=false and or.""CompanyId""='{_repo.UserContext.CompanyId}' and or.""Id""='{_repo.UserContext.LegalEntityId}'");
                //search = string.Concat(
                //@"match(or: HRS_OrganizationRoot{Id:{LegalEntityId}})<-[:R_Payroll_LegalEntity_OrganizationRoot]
                //-(pr:PAY_Payroll{IsDeleted: 0,CompanyId: { CompanyId} })
                //match(pr)<-[:R_PayrollRun_Payroll]-(pay:PAY_PayrollRun {YearMonth:{yearMonth}})
                //match (pay)<-[:R_PayrollRunResult_PayrollRun]-(par:PAY_PayrollRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})");
                where = $@" pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'";
            }
            else
            {
                search = $@"from cms.""N_PayrollHR_PayrollRun"" as pay
join cms.""N_PayrollHR_PayrollRunResult"" as par on par.""PayrollRunId"" = pay.""Id"" and pay.""Id""='{payrollRunId}' and par.""IsDeleted""=false and par.""CompanyId""='{_repo.UserContext.CompanyId}'" ; 
                where = $@" pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'";
            }

            var cypher = string.Concat($@" Select par.""Id"" as Id,e.""Id"" as ElementId,e.""Name"" as ElementName,u.""Id"" as UserId,p.""Id"" as PersonId,p.""SponsorshipNo"" as SponsorshipNo,p.""Gender"" as Gender,a.""DateOfJoin"" as DateOfJoin,
            p.""PersonNo"" as PersonNo,na.""Name"" as Nationality,o.""Name"" as OrganizationName, cc.""Name"" as CostCenter, j.""Name"" as JobName,         
            c.""EffectiveEndDate"" as ContractEndDate,c.""ContractRenewable"" as ContractRenewable,sp.""Name"" as Sponsor
            ,case when sc.""Id"" is null THEN 0 ELSE sc.""Id"" END as SectionId,case when sc.""Name"" is null THEN '' ELSE sc.""Name"" END as SectionName
            ,a.""Id"" as AssignmentId,pg.""Name"" as PayGroup,pc.""Name"" as PayCalendar,prer.""Amount"" as NetAmount,prer.""YearMonth"" as YearMonth,par.""Error"" as Error,par.""ExecutionStatus"" as ExecutionStatus
            ,par.""TotalEarning"" as TotalEarning,par.""TotalDeduction"" as TotalDeduction,prer.""Id"" as PayrollElementRunResultId,u.""Name"" as UserNameWithEmail,p.""PersonFullName"" as EmployeeName
            #SEARCH
            join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=par.""PersonId"" and p.""IsDeleted""=false
left join cms.""N_PayrollHR_PayrollElementRunResult"" as prer on prer.""PayrollRunResultId""=par.""Id"" and prer.""IsDeleted""=false and prer.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=prer.""PayrollElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}' --and e.EffectiveStartDate::Date<='{DateTime.Now.ApplicationNow().Date}'::Date<=e.EffectiveEndDate::Date
left join cms.""N_PayrollHR_PayrollGroup"" as pg on  pg.""Id""=par.""PayrollGroupId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""Id""=par.""CalendarId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as a on a.""PersonId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}' and a.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= a.""EffectiveEndDate"" ::Date
left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= j.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}' and o.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= o.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRCostCenter"" as cc on cc.""Id""=o.""CostCenterId""  and cc.""IsDeleted""=false and cc.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Nationality"" as na on na.""Id""=p.""NationalityId"" and na.""IsDeleted""=false and na.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRSection"" as sc on sc.""Id""=p.""SectionId"" and sc.""IsDeleted""=false and sc.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}' and c.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= c.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false  and sp.""CompanyId""='{_repo.UserContext.CompanyId}'"

, elementCategory == null ? "where #WHERE#" : $@" where #WHERE# and e.""ElementCategory""='{elementCategory}' ");

            //var prms = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "Status", StatusEnum.Active.ToString() },
            //    { "ESD", DateTime.Now.ApplicationNow().Date },
            //    { "payrollRunId", payrollRunId },
            //    { "ElementCategory", elementCategory },
            //    { "yearMonth", yearMonth },
            //    { "LegalEntityId", LegalEntityId },
            //};
            cypher = cypher.Replace("#WHERE#", where);
            cypher = cypher.Replace("#SEARCH", search);
            var result =await _payrunrepo.ExecuteQueryList<PayrollDetailViewModel>(cypher, null);

            var cypher1 = string.Concat(@"select per.""Amount"" as Amount,per.""Date"" as Date,prer.""Id"" as PayrollElementRunResultId,prer.""YearMonth"" as YearMonth 
#SEARCH
from 
             join cms.""N_PayrollHR_PayrollElementRunResult"" as prer on prer.""PayrollRunResultId""=par.""Id"" and prer.""IsDeleted""=false and prer.""CompanyId""='{_repo.UserContext.CompanyId}'
             join cms.""N_PayrollHR_PayrollElementDailyRunResult"" as per on per.""PayrollElementRunResultId""=prer.""Id""  and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
           
            ");
            //var cypher1 = string.Concat(@" #SEARCH
            //match (par)<-[:R_PayrollElementRunResult_PayrollRunResult]-(prer:PAY_PayrollElementRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})  
            //match (prer)<-[:R_PayrollElementDailyRunResult_PayrollElementRunResult]-(per:PAY_PayrollElementDailyRunResult{IsDeleted: 0,CompanyId: {CompanyId}})
            //return per.Amount as Amount,per.Date as Date,prer.Id as PayrollElementRunResultId,prer.YearMonth as YearMonth");

            //var prms1 = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "Status", StatusEnum.Active.ToString() },
            //    { "ESD", DateTime.Now.ApplicationNow().Date },
            //    { "payrollRunId", payrollRunId },
            //    { "yearMonth", yearMonth },
            //     { "LegalEntityId", LegalEntityId },
            //};
            cypher1 = cypher1.Replace("#SEARCH", search);
            var dailyResult = await _payrunrepo.ExecuteQueryList<PayrollElementDailyRunResultViewModel>(cypher1, null);
            foreach (var item in result)
            {
                foreach (var item1 in dailyResult)
                {
                    if (item1.Date.Value.Day == d1 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day1Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d2 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day2Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d3 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day3Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d4 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day4Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d5 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day5Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d6 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day6Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d7 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day7Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d8 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day8Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d9 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day9Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d10 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day10Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d11 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day11Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d12 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day12Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d13 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day13Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d14 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day14Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d15 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day15Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d16 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day16Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d17 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day17Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d18 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day18Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d19 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day19Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d20 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day20Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d21 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day21Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d22 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day22Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d23 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day23Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d24 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day24Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d25 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day25Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d26 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day26Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d27 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day27Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d28 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day28Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d29 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day29Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d30 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day30Amount = item1.Amount;
                    }
                    if (item1.Date.Value.Day == d31 && item1.YearMonth == item.YearMonth && item.PayrollElementRunResultId == item1.PayrollElementRunResultId)
                    {
                        item.Day31Amount = item1.Amount;
                    }
                }
            }

            return result;
        }

        public async Task<List<PayrollRunResultViewModel>> BulkInsertForPayroll(PayrollRunViewModel viewModel, bool idGenerated = true, bool doCommit = true)
        {
            var count = viewModel.EmployeePayrollRunResult.Count;
            if (count <= 0)
            {
                return viewModel.EmployeePayrollRunResult.ToList();
            }
            foreach (var item in viewModel.EmployeePayrollRunResult)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                noteTempModel.TemplateCode = "PayrollRunResult";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(notemodel);

                var elementRunResult = viewModel.EmployeePayrollElementRunResult.Where(x=>x.PayrollRunResultId==item.Id).ToList();
                await BulkInsertIntoElementRunResult(viewModel, elementRunResult,result.Item.UdfNoteTableId);
            }

            return viewModel.EmployeePayrollRunResult;

        }

        public async Task<List<PayrollRunResultViewModel>> GetSuccessfulPayrollRunResult(PayrollRunViewModel viewModel)
        {

            //var prms = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "PayrollRunId", viewModel.Id },
            //    { "ESD", DateTime.Today},
            //    { "LegalEntityId",LegalEntityId },
            //};
            var executionStatus = (int)ExecutionStatusEnum.Success;
            var cypher = $@"select prr.*,leg.""Id"" as LegalEntityId,pg.""Id"" as PayrollGroupId,pr.""Id"" as PersonId
            ,pc.""Id"" as PayrollCalendarId,p.""RunType"" as RunType,u.""Id"" as UserId
from cms.""N_PayrollHR_PayrollRun"" as r 
join cms.""N_PayrollHR_PayrollBatch"" as p on p.""Id""=r.""PayrollBatchId""  and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LegalEntity"" as leg on leg.""Id""=p.""LegalEntityId"" and leg.""Id""='{_repo.UserContext.LegalEntityId}' and leg.""IsDeleted""=false and leg.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=p.""PayrollGroupId"" and pg.""IsDeleted""=false  and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRunResult"" as prr on prr.""PayrollRunId"" = r.""Id"" and prr.""ExecutionStatus""='{executionStatus}' and prr.""IsDeleted""=false  and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=prr.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=pr.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""Id""=prr.""CalendarId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
where r.""Id""='{viewModel.Id}' and r.""IsDeleted""=false and r.""CompanyId""='{_repo.UserContext.CompanyId}'
            ";
            //var cypher = @"match(r:PAY_PayrollRun{IsDeleted: 0,Id:{PayrollRunId}}) 
            //match (r)-[:R_PayrollRun_Payroll]->(p:PAY_Payroll{ IsDeleted: 0 })  
            //match (p)-[:R_Payroll_LegalEntity_OrganizationRoot]->(or:HRS_OrganizationRoot{ Id:{LegalEntityId}})  
            //match (p)-[:R_Payroll_PayrollGroup]->(pg:PAY_PayrollGroup{ IsDeleted: 0 })  
            //match (r)<-[:R_PayrollRunResult_PayrollRun]-(prr:PAY_PayrollRunResult{IsDeleted: 0 ,ExecutionStatus:'Success'})  
            //match (prr)-[:R_PayrollRunResult_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0 })  
            //match (pr)<-[:R_User_PersonRoot]->(u:ADM_User{ IsDeleted: 0 })  
            //match (prr)-[:R_PayrollRunResult_PayCalendar]->(pc:PAY_Calendar{ IsDeleted: 0 })  
            //return prr,or.Id as LegalEntityId,pg.Id as PayrollGroupId,pr.Id as PersonId
            //,pc.Id as PayrollCalendarId,p.RunType as RunType,u.Id as UserId";           
            var result = await _payrunrepo.ExecuteQueryList<PayrollRunResultViewModel>(cypher, null);
            return result;
        }



        //public Task<List<PaySalaryElementViewModel>> GetPaySalaryElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<string[]> GetDistinctElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory, ElementClassificationEnum? elementType)
        {
            var query = $@"select distinct pe.""ElementName"" as ""Name"" 
from cms.""N_PayrollHR_PayrollRun"" as pr
                            join cms.""N_PayrollHR_PayrollRunResult"" as pres on pres.""PayrollRunId"" = pr.""Id"" and pres.""IsDeleted""=false and pres.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElementRunResult"" as per on per.""PayrollRunResultId"" = pres.""Id"" and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElement"" as pe on pe.""Id"" = per.""PayrollElementId"" and pe.""IsDeleted""=false and pe.""CompanyId""='{_repo.UserContext.CompanyId}' and pe.""ElementType""!='2' and pe.""ElementClassification""='{elementType}'
                            where pres.""PayrollRunId""='{payrollRunId}' and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            #ElementWhere#
                        ";

            var where = "";
            if (elementCategory.IsNotNull())
            {
                int ec = (int)elementCategory;
                where = $@"and pe.""ElementCategory""='{ec}'";
            }
            query = query.Replace("#ElementWhere#", where);
            //var match1 = string.Concat(@" match (pay:PAY_PayrollRun{Id:{payrollRunId}})<-[:R_PayrollRunResult_PayrollRun]
            //    -(par:PAY_PayrollRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //  match (par)<-[:R_PayrollElementRunResult_PayrollRunResult]-(prer:PAY_PayrollElementRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})  
            //   match (prer)-[:R_PayrollElementRunResult_ElementRoot]->(er:PAY_ElementRoot)
            //<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: {CompanyId} })
            //   where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate and e.ElementType<>'Accrual' and e.ElementClassification={ElementClassification}
                
            //    with par,prer,e,er ", elementCategory == null ? "" : " where e.ElementCategory={ElementCategory} ", @"
            //    return distinct e.Name order by e.Name");

            //var prms1 = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "payrollRunId", payrollRunId },
            //    { "ESD", DateTime.Now},
            //    { "ElementCategory", elementCategory},
            //    { "ElementClassification", elementType}
            //};

            var result = await _payrunrepo.ExecuteScalarList<string>(query, null);
            return result.ToArray();
        }

        public async Task<string[]> GetDistinctElementEarning(string payrollRunId, ElementCategoryEnum? elementCategory)
        {
            var query = $@"select distinct pe.""ElementName"" as ""Name"" 
from cms.""N_PayrollHR_PayrollRun"" as pr
                            join cms.""N_PayrollHR_PayrollRunResult"" as pres on pres.""PayrollRunId"" = pr.""Id"" and pres.""IsDeleted""=false and pres.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElementRunResult"" as per on per.""PayrollRunResultId"" = pres.""Id"" and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElement"" as pe on pe.""Id"" = per.""PayrollElementId"" and pe.""IsDeleted""=false and pe.""CompanyId""='{_repo.UserContext.CompanyId}' and pe.""ElementType""!='2' and pe.""ElementClassification""='{ElementClassificationEnum.Earning}'
                            where pres.""PayrollRunId""='{payrollRunId}' and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            #ElementWhere#
                        ";

            var where = "";
            if (elementCategory.IsNotNull())
            {
                int ec = (int)elementCategory;
                where = $@"and pe.""ElementCategory""='{ec}'";
            }
            query = query.Replace("#ElementWhere#", where);
            var result = await _elemetRepo.ExecuteQueryList(query, null);
            string[] stringarr = result.Select(x => x.Name).ToArray();
            return stringarr;
            //var match1 = string.Concat(@" match (pay:PAY_PayrollRun{Id:{payrollRunId}})<-[:R_PayrollRunResult_PayrollRun]
            //    -(par:PAY_PayrollRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //  match (par)<-[:R_PayrollElementRunResult_PayrollRunResult]-(prer:PAY_PayrollElementRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})  
            //   match (prer)-[:R_PayrollElementRunResult_ElementRoot]->(er:PAY_ElementRoot)
            //<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: {CompanyId} })
            //   where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate and e.ElementClassification={ElementClassification} and e.ElementType<>'Accrual'

            //    with par,prer,e,er ", elementCategory == null ? "" : " where e.ElementCategory={ElementCategory} ", @"
            //    with distinct coalesce(e.DisplayName,e.Name)  as Name,e.ElementClassification as ElementClassification,e.SequenceNo as SequenceNo
            //    return Name order by ElementClassification desc,SequenceNo,Name");

            //var prms1 = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "payrollRunId", payrollRunId },
            //    { "ESD", DateTime.Now},
            //    { "ElementCategory", elementCategory},
            //    { "ElementClassification", ElementClassificationEnum.Earning}
            //};

        }

        public async Task<string[]> GetDistinctElementDeduction(string payrollRunId, ElementCategoryEnum? elementCategory)
        {
            var query = $@"select distinct pe.""ElementName"" as ""Name"" 
from cms.""N_PayrollHR_PayrollRun"" as pr
                            join cms.""N_PayrollHR_PayrollRunResult"" as pres on pres.""PayrollRunId"" = pr.""Id"" and pres.""IsDeleted""=false and pres.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElementRunResult"" as per on per.""PayrollRunResultId"" = pres.""Id"" and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElement"" as pe on pe.""Id"" = per.""PayrollElementId"" and pe.""IsDeleted""=false and pe.""CompanyId""='{_repo.UserContext.CompanyId}' and pe.""ElementType""!='2' and pe.""ElementClassification""='{ElementClassificationEnum.Deduction}'
                            where pres.""PayrollRunId""='{payrollRunId}' and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            #ElementWhere#
                        ";

            var where = "";
            if (elementCategory.IsNotNull())
            {
                int ec = (int)elementCategory;
                where = $@"and pe.""ElementCategory""='{ec}'";
            }
            query = query.Replace("#ElementWhere#", where);
            var result = await _elemetRepo.ExecuteQueryList(query, null);
            string[] stringarr = result.Select(x => x.Name).ToArray();
            return stringarr;
            //var match1 = string.Concat(@" match (pay:PAY_PayrollRun{Id:{payrollRunId}})<-[:R_PayrollRunResult_PayrollRun]
            //    -(par:PAY_PayrollRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //  match (par)<-[:R_PayrollElementRunResult_PayrollRunResult]-(prer:PAY_PayrollElementRunResult{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})  
            //   match (prer)-[:R_PayrollElementRunResult_ElementRoot]->(er:PAY_ElementRoot)
            //<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: {CompanyId} })
            //   where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate and e.ElementClassification={ElementClassification} and e.ElementType<>'Accrual'

            //    with par,prer,e,er ", elementCategory == null ? "" : " where e.ElementCategory={ElementCategory} ", @"
            //    with distinct e.Name as Name,e.ElementClassification as ElementClassification,e.SequenceNo as SequenceNo
            //    return Name order by ElementClassification desc,SequenceNo,Name");

            //var prms1 = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "payrollRunId", payrollRunId },
            //    { "ESD", DateTime.Now},
            //    { "ElementCategory", elementCategory},
            //    { "ElementClassification", ElementClassificationEnum.Deduction}
            //};

            //var result = ExecuteCypherScalarList<string>(match1, prms1).ToArray();
            //return result;
        }

        //public Task<string[]> GetDistinctElementDisplayName(string payrollRunId, ElementCategoryEnum? elementCategory)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<PayrollElementRunResultViewModel>> BulkInsertIntoElementRunResult(PayrollRunViewModel viewModel,List<PayrollElementRunResultViewModel> viewModelList,string payrollRunResultId, bool idGenerated = true, bool doCommit = true)
        {
           
            var count = viewModelList.Count;
            if (count <= 0)
            {
                return viewModelList.ToList();
            }
            foreach (var item in viewModelList)
            {
                item.PayrollRunResultId = payrollRunResultId;
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                noteTempModel.TemplateCode = "PayrollElementRunResult";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(notemodel);
                var dailyRunResult = viewModel.EmployeePayrollElementDailyRunResult.Where(x => x.PayrollElementRunResultId == item.Id).ToList();
                await BulkInsertIntoElementDailyRunResult(dailyRunResult,result.Item.UdfNoteTableId);
            }

            return viewModelList;
        }

        public async Task<List<PayrollElementRunResultViewModel>> GetSuccessfulElementRunResult(PayrollRunViewModel viewModel)
        {
            var executionstatus = (int)ExecutionStatusEnum.Success;
            var cypher = $@"select prer.*,leg.""Id"" as LegalEntityId,pg.""Id"" as PayrollGroupId,pr.""Id"" as PersonId,prr.""Id"" as PayrollRunResultId
            ,pc.""Id"" as PayrollCalendarId,p.""RunType"" as RunType,er.""Id"" as ElementId
            from cms.""N_PayrollHR_PayrollRun"" as r 
            join cms.""N_PayrollHR_PayrollBatch"" as p on p.""Id""=r.""PayrollBatchId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=p.""PayrollGroupId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LegalEntity"" as leg on leg.""Id""=p.""LegalEntityId"" and leg.""IsDeleted""=false and leg.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_PayrollHR_PayrollRunResult"" as prr on prr.""PayrollRunId"" = r.""Id"" and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""Id""=prr.""CalendarId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=prr.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_PayrollHR_PayrollElementRunResult"" as prer on prer.""PayrollRunResultId"" = prr.""Id"" and prer.""IsDeleted""=false and prer.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_PayrollHR_PayrollElement"" as er on er.""Id"" = prer.""PayrollElementId"" and er.""IsDeleted""=false and er.""CompanyId""='{_repo.UserContext.CompanyId}'
            where prer.""PayrollRunId""='{viewModel.Id}' and r.""IsDeleted""=false  and r.""CompanyId""='{_repo.UserContext.CompanyId}' and prer.""ExecutionStatus""='{executionstatus}' ";
            //var prms = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "PayrollRunId", viewModel.Id },
            //    { "ESD", DateTime.Today },
            //    { "LegalEntityId", LegalEntityId }
            //};

            //var cypher = @"match(r:PAY_PayrollRun{IsDeleted: 0,Id:{PayrollRunId}}) 
            //match (r)-[:R_PayrollRun_Payroll]->(p:PAY_Payroll)  
            //match (p)-[:R_Payroll_LegalEntity_OrganizationRoot]->(or:HRS_OrganizationRoot{Id:{LegalEntityId}})  
            //match (p)-[:R_Payroll_PayrollGroup]->(pg:PAY_PayrollGroup)  
            //match (r)<-[:R_PayrollRunResult_PayrollRun]-(prr:PAY_PayrollRunResult{ExecutionStatus:'Success'})  
            //match (prr)-[:R_PayrollRunResult_PersonRoot]->(pr:HRS_PersonRoot) 
            //match (prr)-[:R_PayrollRunResult_PayCalendar]->(pc:PAY_Calendar) 
            //match (prr)<-[:R_PayrollElementRunResult_PayrollRunResult]-(prer:PAY_PayrollElementRunResult{ExecutionStatus:'Success'})  
            //match (prer)-[:R_PayrollElementRunResult_ElementRoot]->(er:PAY_ElementRoot)  
            //return prer,or.Id as LegalEntityId,pg.Id as PayrollGroupId,pr.Id as PersonId,prr.Id as PayrollRunResultId
            //,pc.Id as PayrollCalendarId,p.RunType as RunType,er.Id as ElementId";
            var result = await _elemetRepo.ExecuteQueryList<PayrollElementRunResultViewModel>(cypher, null);
            return result;
        }

        public async  Task<List<PayrollElementDailyRunResultViewModel>> BulkInsertIntoElementDailyRunResult(List<PayrollElementDailyRunResultViewModel> viewModelList,string elementRunResultId, bool idGenerated = true, bool doCommit = true)
        {
            var count = viewModelList.Count;
            if (count <= 0)
            {
                return  viewModelList.ToList();
            }
       
            foreach (var item in viewModelList)
            {
                item.PayrollElementRunResultId = elementRunResultId;
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                noteTempModel.TemplateCode = "PayrollElementDailyRunResult";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(notemodel);
            }
            return viewModelList;

        }
    }
}
