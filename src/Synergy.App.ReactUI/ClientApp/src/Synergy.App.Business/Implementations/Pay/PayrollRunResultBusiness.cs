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
    public class PayrollRunResultBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollRunResultBusiness
    {
        private readonly IRepositoryQueryBase<PayrollRunResultViewModel> _payrunrepo;
        private readonly IRepositoryQueryBase<ElementViewModel> _elemetRepo;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;
        private readonly IPayrollRunBusiness _payrollRunBusiness;
        private readonly IRepositoryQueryBase<PayrollSalaryElementViewModel> _paysalelerepo;
        private readonly IRepositoryQueryBase<SalaryElementEntryViewModel> _repsalEleEntry;
        INoteBusiness _noteBusiness;
        private readonly IPayRollQueryBusiness _payRollQueryBusiness;

        public PayrollRunResultBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IRepositoryQueryBase<PayrollRunResultViewModel> payrunrepo,
            IMapper autoMapper, IRepositoryQueryBase<ElementViewModel> elemetRepo, IPayrollBatchBusiness payrollBatchBusiness,
            IPayrollRunBusiness payrollRunBusiness, IRepositoryQueryBase<PayrollSalaryElementViewModel> paysalelerepo,
            IRepositoryQueryBase<SalaryElementEntryViewModel> repsalEleEntry, IPayRollQueryBusiness payRollQueryBusiness,
            INoteBusiness noteBusiness) : base(repo, autoMapper)
        {
            _payrunrepo = payrunrepo;
            _elemetRepo = elemetRepo;
            _payrollBatchBusiness = payrollBatchBusiness;
            _payrollRunBusiness = payrollRunBusiness;
            _paysalelerepo = paysalelerepo;
            _repsalEleEntry = repsalEleEntry;
            _noteBusiness = noteBusiness;
            _payRollQueryBusiness = payRollQueryBusiness;
        }

        public async override Task<CommandResult<NoteViewModel>> Create(NoteViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<NoteViewModel>(model);                       
           
            var result = await base.Create(data, autoCommit);

            return CommandResult<NoteViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteViewModel>> Edit(NoteViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<NoteViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        public async Task<string[]> GetDistinctElement(string payrollRunId, ElementCategoryEnum? elementCategory)
        {
            var result = await _payRollQueryBusiness.GetDistinctElement(payrollRunId, elementCategory);
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



            var employees = await _payRollQueryBusiness.GetPayrollSalaryElements(payrollRunId);

            var persons = employees.Select(x => x.PersonId);
            var pers = string.Join("','", persons.ToArray());



            var result1 = await _payRollQueryBusiness.GetPersonSalEntryDetails(pers, payrollBatch);


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




            var salaryElementList = await _payRollQueryBusiness.GetPayrollRunData(payrollRunId, elementCategory);


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

            var queryresult = await _payRollQueryBusiness.GetDistinctElementDisplayName(payrollRunId, elementCategory);
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





            var result = await _payRollQueryBusiness.GetpayRollDetails(payrollRunId, yearMonth, elementCategory);



            var dailyResult = await _payRollQueryBusiness.GetPayrollDailyResult(payrollRunId, yearMonth);

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
            var executionStatus = (int)ExecutionStatusEnum.Success;
            var result = await _payRollQueryBusiness.GetSuccessfulPayrollRunResult(viewModel, executionStatus);
            return result;
        }



        //public Task<List<PaySalaryElementViewModel>> GetPaySalaryElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<string[]> GetDistinctElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory, ElementClassificationEnum? elementType)
        {
            var result = await _payRollQueryBusiness.GetDistinctElementDetails(payrollRunId, elementCategory, elementType);
            return result.ToArray();
        }

        public async Task<string[]> GetDistinctElementEarning(string payrollRunId, ElementCategoryEnum? elementCategory)
        {
            var result = await _payRollQueryBusiness.GetDistinctElementEarning(payrollRunId, elementCategory);
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

            var result = await _payRollQueryBusiness.GetDistinctElementDeduction(payrollRunId, elementCategory);
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

            var result = await _payRollQueryBusiness.GetSuccessfulElementRunResult(viewModel, executionstatus);
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
