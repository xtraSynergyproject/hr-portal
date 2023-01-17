using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.Pay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class PayrollTransactionBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollTransactionsBusiness
    {
        private IServiceProvider _serviceProvider;
        private INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollTransactionViewModel> _payrollTransaction;
        private readonly IRepositoryQueryBase<ElementViewModel> _queryRepoElement;
        private readonly IPayRollQueryBusiness _payRollQueryBusiness;
        private IPayrollElementBusiness _payrollElementBusiness;
        IUserContext _userContext;
        private readonly ILOVBusiness _lovBusiness;
        public PayrollTransactionBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper, INoteBusiness noteBusiness,
            IRepositoryQueryBase<NoteViewModel> queryRepo,
            IRepositoryQueryBase<PayrollTransactionViewModel> payrollTransaction,
            IServiceProvider serviceProvider
            , IPayrollElementBusiness payrollElementBusiness
            , IRepositoryQueryBase<ElementViewModel> queryRepoElement
            , IPayRollQueryBusiness payRollQueryBusiness
            , IUserContext userContext
            , ILOVBusiness lovBusiness) : base(repo, autoMapper)
        {
            _noteBusiness = noteBusiness;
            _queryRepo = queryRepo;
            _payrollTransaction = payrollTransaction;
            _serviceProvider = serviceProvider;
            _payrollElementBusiness = payrollElementBusiness;
            _queryRepoElement = queryRepoElement;
            _userContext = userContext;
            _lovBusiness = lovBusiness;
            _payRollQueryBusiness = payRollQueryBusiness;
        }

        public async Task<bool> ManagePayrollTransaction(PayrollTransactionViewModel model)
        {
            try
            {
                var note = new NoteTemplateViewModel
                {
                    ActiveUserId = _repo.UserContext.UserId,
                    TemplateCode = "PayrollTransaction",
                };
                var noteModel = await _noteBusiness.GetNoteDetails(note);
                noteModel.NoteSubject = "Payroll Trnasaction";
                noteModel.OwnerUserId = _repo.UserContext.UserId;
                noteModel.StartDate = DateTime.Now;
                noteModel.CreatedDate = DateTime.Now;
                if (model.Id.IsNullOrEmpty())
                {
                    noteModel.DataAction = DataActionEnum.Create;
                }
                else
                {
                    noteModel.DataAction = DataActionEnum.Edit;
                }
                model.CreatedBy = _repo.UserContext.UserId;
                model.CompanyId = _repo.UserContext.CompanyId;
                model.CreatedDate = DateTime.Now;
                model.LastUpdatedBy = _repo.UserContext.UserId;
                model.LastUpdatedDate = DateTime.Now;
                noteModel.Json = JsonConvert.SerializeObject(model); //model.Json;
                var result = await _noteBusiness.ManageNote(noteModel);
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }


        public async Task<bool> DeletePayrollTransaction(string NoteId)
        {
            if (NoteId.IsNotNull())
            {
                await _payRollQueryBusiness.DeletePayrollTransaction(NoteId);
                return true;
            }
            return false;
        }

        public async Task<PayrollTransactionViewModel> GetPayrollTransactionDetails(string transactionId)
        {
            var queryData = await _payRollQueryBusiness.GetPayrollTransactionDetails(transactionId);
            return queryData;

        }
        public async Task<IdNameViewModel> GetPayrollElementByCode(string code)
        {
            var queryData = await _payRollQueryBusiness.GetPayrollElementByCode(code);
            return queryData;

        }

        public async Task<List<PayrollTransactionViewModel>> GetAllUnProcessedTransactionsForPayrollRun(PayrollRunViewModel viewModel)
        {
            var list = await _payRollQueryBusiness.GetAllUnProcessedTransactionsForPayrollRun(viewModel);
            if (list.Count > 0)
            {
                var payTransactons = list.Select(x => x.Id).ToList();
                var payTransIds = "";
                foreach (var i in payTransactons)
                {
                    payTransIds += $"'{i}',";
                }
                payTransIds = payTransIds.Trim(',');
                DateTime lastUpdateDate = DateTime.Now;

                await _payRollQueryBusiness.UpdatePayrollTxn(viewModel, payTransIds, lastUpdateDate);
            }
            return list.ToList();
        }

        public async Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionList(PayrollTransactionViewModel search)
        {

            var result1 = await _payRollQueryBusiness.GetPayrollTransactionList(search);

            if (search.OrganizationId != null)
            {
                result1 = result1.Where(x => x.PayrollOrganizationId == search.OrganizationId).ToList();
            }

            //var date = DateTime.Today;

            var first = new DateTime(search.Year.Value, search.Month.Value, 1);

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

            var result = result1.GroupBy(x => new { x.PersonId, x.ElementId }).Select(x => x.FirstOrDefault());
            //var result = result1.DistinctBy(x => new { x.PersonId, x.ElementId });
            var paylist = new List<PayrollTransactionViewModel>();

            foreach (var item in result)
            {
                double total = 0;
                foreach (var item1 in result1)
                {
                    if (item1.EffectiveDate == d1 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day1Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day1Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d2 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day2Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day2Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d3 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day3Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day3Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d4 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day4Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day4Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d5 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day5Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day5Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d6 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day6Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day6Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d7 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day7Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day7Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d8 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day8Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day8Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d9 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day9Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day9Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d10 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day10Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day10Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d11 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day11Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day11Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d12 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day12Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day12Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d13 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day13Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day13Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d14 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day14Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day14Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d15 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day15Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day15Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d16 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day16Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day16Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d17 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day17Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day17Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d18 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day18Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day18Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d19 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day19Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day19Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d20 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day20Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day20Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d21 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day21Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day21Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d22 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day22Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day22Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d23 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day23Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day23Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d24 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day24Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day24Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d25 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day25Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day25Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d26 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day26Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day26Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d27 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day27Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day27Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d28 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day28Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day28Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d29 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day29Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day29Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d30 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day30Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day30Status = item1.ProcessStatus;
                    }
                    if (item1.EffectiveDate == d31 && item.ElementId == item1.ElementId && item.PersonId == item1.PersonId)
                    {
                        item.Day31Amount = item1.Amount;
                        total = total + item1.Amount;
                        item.Day31Status = item1.ProcessStatus;
                    }
                }
                item.TotalAmount = total.RoundPayrollSummaryAmount();
                paylist.Add(item);
            }

            paylist = paylist.ToList();

            //if (search.OrganizationId.IsNotNull())
            //{
            //    paylist = paylist.Where(x => x.OrganizationId == search.OrganizationId).ToList();
            //}
            //if (search.Month.IsNotNull() && search.Year.IsNotNull())
            //{
            //    paylist = paylist.Where(x => DateTime.Parse(x.EffectiveDate.ToString()).Month == search.Month && DateTime.Parse(x.EffectiveDate.ToString()).Year == search.Year).ToList();
            //}

            return paylist;
        }

        public async Task<List<PayrollTransactionViewModel>> BulkUpdateForPayroll(List<PayrollTransactionViewModel> viewModelList, string payrollId, string payrollRunId, bool doCommit = true)
        {
            //throw new NotImplementedException();
            //Log.Instance.Info(DelimeterEnum.Space, "PayrollTransactionViewModel Count: ", viewModelList.Count);
            var processStatus = await _lovBusiness.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "PROCESSED");

            if (viewModelList.Count <= 0)
            {
                return viewModelList;
            }
            //var notProcessed = viewModelList.Where(x => x.ProcessStatus != PayrollProcessStatusEnum.Processed).Select(x => x.Id).ToList();
            //Log.Instance.Info(DelimeterEnum.Space, "PayrollTransactionViewModel Not Processed Count: ", notProcessed.Count);
            //var notProcessedChunks = Helper.ToChunks<long>(notProcessed, 100);
            //var script = "";
            //foreach (var chunk in notProcessedChunks)
            //{
            //    if (chunk.Count > 0)
            //    {
            //        script = string.Concat("match (n:PAY_PayrollTransaction) where n.Id in[", string.Join(",", chunk),
            //        "] set n.PayrollId=null,n.PayrollRunId=null,n.LastUpdatedDate=localdatetime()");
            //        _repository.ExecuteCypherWithoutResult(script);
            //    }
            //}

            var notProcessed = viewModelList.Where(x => x.ProcessStatusId != processStatus.Id).Select(x => x.Id).ToList();
            var notProcessedIds = "";
            foreach (var i in notProcessed)
            {
                notProcessedIds += $"'{i}',";
            }
            var lastUpdateDate = DateTime.Now;
            if (notProcessedIds.IsNotNullAndNotEmpty())
            {
                notProcessedIds = notProcessedIds.Trim(',');

                await _payRollQueryBusiness.UpdatePayrollTxnforIds(lastUpdateDate, notProcessedIds);
            }


            //var processed = viewModelList.Where(x => x.ProcessStatus == PayrollProcessStatusEnum.Processed).Select(x => x.Id).ToList();
            //Log.Instance.Info(DelimeterEnum.Space, "PayrollTransactionViewModel Processed Count: ", processed.Count);
            //var processedChunks = Helper.ToChunks<long>(processed, 100);
            //var prms = new Dictionary<string, object>
            //{
            //    { "PayrollId",payrollId},
            //    { "PayrollRunId",payrollRunId},
            //};
            //foreach (var chunk in processedChunks)
            //{
            //    if (chunk.Count > 0)
            //    {
            //        script = string.Concat("match (n:PAY_PayrollTransaction) where n.Id in[", string.Join(",", chunk),
            //        "] set n.PayrollId={PayrollId},n.PayrollRunId={PayrollRunId},n.ProcessStatus='Processed',n.LastUpdatedDate=localdatetime()");
            //        _repository.ExecuteCypherWithoutResult(script, prms);
            //    }
            //}

            var processed = viewModelList.Where(x => x.ProcessStatusId == processStatus.Id).Select(x => x.Id).ToList();
            var processedIds = "";

            foreach (var i in processed)
            {
                processedIds += $"'{i}',";
            }
            if (processedIds.IsNotNullAndNotEmpty())
            {
                processedIds = processedIds.Trim(',');

                await _payRollQueryBusiness.UpdatePayrollTxnDetailsforIds(processedIds, payrollId, payrollRunId, processStatus, lastUpdateDate);
            }


            return viewModelList;
        }

        public async Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionBasedonElement(string personId, DateTime EffectiveDate, string ElementCode)
        {


            var result = await _payRollQueryBusiness.GetPayrollTransactionBasedonElement(personId, EffectiveDate, ElementCode);
            return result;
        }

        public async Task<bool> IsTransactionExists(string personId, string elementCode, DateTime date, double amount)
        {
            //         var cypher = string.Concat(@"
            //         match (pt:PAY_PayrollTransaction{IsDeleted:0,Status:'Active',EffectiveDate:{EffectiveDate},Amount:{Amount}})  
            //match(pt)-[:R_PayrollTransaction_ElementRoot]->(er:PAY_ElementRoot{IsDeleted:0,Status:'Active'})
            //match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:'Active',Code:{Code}})
            //         where e.EffectiveStartDate<=pt.EffectiveDate<=e.EffectiveEndDate 
            //   match(pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0,Status:'Active',Id:{PersonId}})
            //         return pt limit 1");

            var list = await _payRollQueryBusiness.IsTransactionExists(personId, elementCode, date, amount);
            return list != null;
        }

        public async Task BulkUpdate(List<PayrollTransactionViewModel> viewModel, bool doCommit = true)
        {
            //throw new NotImplementedException();
            var count = viewModel.Count;
            if (count <= 0)
            {
                return;
            }
            foreach (var item in viewModel)
            {
                if (item.NtsNoteId.IsNotNullAndNotEmpty())
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = item.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "PayrollTransaction";
                    noteTempModel.NoteId = item.NtsNoteId;
                    var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

                    noteModel.Json = JsonConvert.SerializeObject(item);
                    noteModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
            }
        }

        public async Task GeneratePayrollSalaryTransactions(PayrollRunViewModel viewModel)
        {
            //throw new NotImplementedException();
            //Log.Instance.Info("Start GeneratePayrollSalaryTransactions");
            var tvm = new PayrollTransactionViewModel
            {                
                EmployeeList = viewModel.EmployeeList,
                ElementList = viewModel.ElementList,
                SalaryTransactionList = await GetSalaryTransactionList(viewModel.PayrollStartDate, viewModel.PayrollEndDate),
                EmployeeSalaryElementInfoList = await _payrollElementBusiness.GetAllSalaryElementInfo() // _salaryElementInfoBusiness.GetAllSalaryElementInfo()
            };

            var asofDate = viewModel.PayrollStartDate;
            while (asofDate <= viewModel.PayrollEndDate)
            {
                foreach (var person in tvm.EmployeeList)
                {

                    var salaryElementInfoList = tvm.EmployeeSalaryElementInfoList.Where(x => x.PersonId == person.PersonId &&
                    x.EffectiveStartDate <= asofDate && asofDate <= x.EffectiveEndDate).ToList();
                    foreach (var salaryElementInfo in salaryElementInfoList)
                    {
                        var element = tvm.ElementList.FirstOrDefault(x => x.ElementId == salaryElementInfo.ElementId);
                        if (element != null)
                        {
                            var amount = salaryElementInfo.Amount.RoundToTwoDecimal();
                            if (element.ValueType == ElementValueTypeEnum.Percentage)
                            {
                                amount = await GetPercentageAmount(asofDate, tvm, element, person, salaryElementInfo);
                            }
                            if (element.ElementEntryType == ElementEntryTypeEnum.SingleEntry)
                            {
                                if (asofDate.IsLastDayOfMonth())
                                {
                                    await AddDailyTransaction(asofDate, amount, person, salaryElementInfo, element, tvm);
                                }
                            }
                            else
                            {
                                amount = salaryElementInfo.Amount.PayrollDailyAmount(asofDate);
                                if (asofDate.IsLastDayOfMonth())
                                {
                                    amount += salaryElementInfo.Amount.RoundToTwoDecimal() - (amount * asofDate.DaysInMonth());
                                }

                                await AddDailyTransaction(asofDate, amount, person, salaryElementInfo, element, tvm);
                            }

                        }
                    }
                    if (asofDate == viewModel.PayrollEndDate) 
                    {
                        await ManageMandatoryDeduction(person, asofDate, tvm);
                    }

                }

                asofDate = asofDate.AddDays(1);
            }
            //Log.Instance.Info("End GeneratePayrollSalaryTransactions");
            var salaryTransactionListCreate = tvm.SalaryTransactionList.Where(x => x.DataAction == DataActionEnum.Create).ToList();
            await BulkInsert(salaryTransactionListCreate, false);
            var salaryTransactionListEdit = tvm.SalaryTransactionList.Where(x => x.DataAction == DataActionEnum.Edit).ToList();
            await BulkUpdate(salaryTransactionListEdit);
            //Log.Instance.Info("End Bulk insert GeneratePayrollSalaryTransactions");
        }
        public async Task<MandatoryDeductionViewModel> GetExemption(MandatoryDeductionViewModel model,string financialYearId,string personId,DateTime asofDate) 
        {
            var data = await _payRollQueryBusiness.GetExemption(model, financialYearId, personId, asofDate);
            return data;
        }
        public async Task<double> GetNoOfMonthsForEmploeePayroll(string financialYearId, DateTime? lastWorkingDate,DateTime? dateofJoin)
        {
            var data = await _payRollQueryBusiness.GetNoOfMonthsForEmploeePayroll(dateofJoin,lastWorkingDate, financialYearId);
            return data;
        }
        public async Task ManageMandatoryDeduction(PayrollPersonViewModel person,DateTime asOfDate,PayrollTransactionViewModel tvm)
        {
            var _paryollRunBusiness = _serviceProvider.GetService<IPayrollRunBusiness>();
            var _hrCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var _lovBusiness= _serviceProvider.GetService<ILOVBusiness>();
            var _payrollElementBusiness = _serviceProvider.GetService<IPayrollElementBusiness>();
            var FY = await GetFinancialYear(asOfDate);
            var FinancialYear = FY.FirstOrDefault();
            if (FinancialYear.IsNotNull())
            { 
             var mandatoryDeductions=await GetMandatoryDeductionOfFinancialYear(asOfDate);
             foreach (var deduction in mandatoryDeductions) 
            {
                IList<SalaryElementInfoViewModel> elementList = new List<SalaryElementInfoViewModel>();
                var DeductionElements = await _paryollRunBusiness.GetSingleElementById(deduction.NoteId);
                foreach (var deductionElement in DeductionElements) 
                {
                        var salaryElement = await _payrollElementBusiness.GetSalaryElementInfoListByUserAndELement(person.PersonId, deductionElement.PayRollElementId);
                        if (salaryElement.IsNotNull())
                        {
                            elementList.Add(salaryElement);
                        }
                }
                if (elementList.Count() > 0)
                {
                    var Amount = elementList.Sum(x => x.Amount);
                    var SlabAmount = Amount;
                    var Exemption =await GetExemption(deduction, FinancialYear.Id, person.PersonId,asOfDate); //return a double value 
                        var Person = await _hrCoreBusiness.GetPersonDetailsById(person.PersonId);
                        string PersonType = string.Empty;
                        double ExemptionAmount = 0.0;
                        double noOfMonths = await GetNoOfMonthsForEmploeePayroll(FinancialYear.Id,person.LastWorkingDate,person.DateOfJoin);
                        if (Person.IsNotNull()) 
                        {
                            
                                if (DateTime.Now.Year - Person.DateOfBirth.Year > 60)
                                {
                                    PersonType = "SeniorCitizen";
                                if (Exemption != null)
                                {
                                    if (Exemption.SeniorCitizenAmount > Exemption.TotalAmount)
                                    {
                                        ExemptionAmount = Exemption.TotalAmount;
                                    }
                                    else
                                    {
                                        ExemptionAmount = Exemption.SeniorCitizenAmount;
                                    }
                                }
                                }
                                else
                                {
                                    var Gender = await _lovBusiness.GetSingleById(Person.GenderId);
                                    if (Gender.Code == "FEMALE")
                                    {
                                        PersonType = "Woman";
                                    if (Exemption != null)
                                    {
                                        if (Exemption.WomanAmount > Exemption.TotalAmount)
                                        {
                                            ExemptionAmount = Exemption.TotalAmount;
                                        }
                                        else
                                        {
                                            ExemptionAmount = Exemption.WomanAmount;
                                        }
                                    }
                                    }
                                    else
                                    {
                                        PersonType = "Employee";
                                    if (Exemption != null)
                                    {
                                        if (Exemption.IsNotNull())
                                        {
                                            if (Exemption.EmployeeAmount > Exemption.TotalAmount)
                                            {
                                                ExemptionAmount = Exemption.TotalAmount;
                                            }
                                            else
                                            {
                                                ExemptionAmount = Exemption.EmployeeAmount;
                                            }
                                        }
                                    }

                                    }

                                }
                            
                            
                        }
                        var slabType = await _lovBusiness.GetSingleById(deduction.DeductionSlabType);
                        if (slabType.Code == "ANNUALLY")
                        {
                            SlabAmount = (SlabAmount * 12) - ExemptionAmount;
                        }
                        else
                        {
                            ExemptionAmount = ExemptionAmount / 12;
                            SlabAmount = SlabAmount - ExemptionAmount;
                        }
                        var slab = await _paryollRunBusiness.GetSlabForMandatoryDeduction(deduction.NoteId, SlabAmount,asOfDate);
                    if (slab.IsNotNull()) 
                    {
                        double deductionAmount = 0.0;
                            if (PersonType == "SeniorCitizen")
                            {
                                if (slab.EmployeeSeniorCitizenValue.IsNotNull())
                                {
                                    deductionAmount = Convert.ToDouble(slab.EmployeeSeniorCitizenValue);

                                }
                            }
                            else if (PersonType == "Woman")
                            {
                                if (slab.EmployeeWomanValue.IsNotNull())
                                {
                                    deductionAmount = Convert.ToDouble(slab.EmployeeWomanValue);

                                }
                            }
                            else if (PersonType == "Employee")
                            {  
                                if (slab.EmployeeDefaultValue.IsNotNull())
                                {
                                    deductionAmount = Convert.ToDouble(slab.EmployeeDefaultValue);

                                }
                            }    
                        var valueType = await _lovBusiness.GetSingleById(deduction.DeductionValueType);
                        if (valueType.Code == "PERCENTAGE_VALUE")
                        {
                            deductionAmount = (SlabAmount * deductionAmount) / 100;
                                if (slabType.Code == "ANNUALLY")
                                {
                                    deductionAmount = deductionAmount / 12;
                                }
                            }
                        else 
                        {
                            if (slabType.Code == "ANNUALLY")
                            {
                                deductionAmount =  deductionAmount / 12;
                            }
                        }
                        var calType = await _lovBusiness.GetSingleById(deduction.DeductionCalculationType);
                        if (calType.Code == "ANNUALLY")
                        {
                                if (FinancialYear.EffectiveEndDate.Value.Month==asOfDate.Month) 
                                { 
                                    deductionAmount = deductionAmount * 12; 
                                }
                                else { deductionAmount = 0; }
                            
                        }                        
                        var payrollElement = await _payrollElementBusiness.GetPayrollElementById(deduction.PayrollElementId);
                            if (deductionAmount>0) 
                            {
                                await AddTransaction(asOfDate, deductionAmount, person, payrollElement, tvm);
                            }
                    }
                }      
            }
            }
        }
        public async Task<List<MandatoryDeductionViewModel>> GetFinancialYear(DateTime asOfDate)
        {
            var result = await _payRollQueryBusiness.GetFinancialYear(asOfDate);
            return result;
        }
        public async Task<List<MandatoryDeductionViewModel>> GetMandatoryDeductionOfFinancialYear(DateTime asOfDate) 
        {
            var result = await _payRollQueryBusiness.GetMandatoryDeductionOfFinancialYear(asOfDate);
            return result;
        }
        private async Task<List<PayrollTransactionViewModel>> GetSalaryTransactionList(DateTime startDate, DateTime endDate, string payrollRunId = null)
        {
            var list = await _payRollQueryBusiness.GetSalaryTransactionList(startDate, endDate, payrollRunId);
            return list;
        }
        private async Task<double> GetPercentageAmount(DateTime asofDate, PayrollTransactionViewModel viewModel, ElementViewModel element,
        PayrollPersonViewModel employee, SalaryElementInfoViewModel salaryElementInfo)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("Status", StatusEnum.Active);
            //prms.AddIfNotExists("CompanyId", CompanyId);
            //prms.AddIfNotExists("Id", element.Id);
            //prms.AddIfNotExists("ESD", DateTime.Now.ApplicationNow().Date);
            //var cypher = @"match(e:PAY_Element{ IsDeleted: 0,Id:{Id} })-[:R_Element_Percentage_ElementRoot]->
            //    (er:PAY_ElementRoot{ IsDeleted: 0,CompanyId: { CompanyId} }) return er.Id as Id";
            //var percentageElementRootIds = ExecuteCypherList<PAY_ElementRoot>(cypher, prms);

            var elementId = element.ElementId;
            
            var percentageElementRootIds = await _payRollQueryBusiness.GetPayrollElement(elementId);

            var totalElementValue = viewModel.EmployeeSalaryElementInfoList
                .Where(x => x.PersonId == employee.PersonId
                && x.EffectiveStartDate <= asofDate && x.EffectiveEndDate >= asofDate
                && percentageElementRootIds.Any(y => y.ElementId == x.ElementId)).Sum(x => x.Amount);

            var percentageValue = (totalElementValue / 100.00) * element.PercentageValue.Value;
            return percentageValue.RoundToTwoDecimal();
        }

        private async Task AddTransaction(DateTime asofDate, double amount, PayrollPersonViewModel person,
         ElementViewModel element, PayrollTransactionViewModel viewModel)
        {
            try
            {
                if (person.DateOfJoin == null /*|| person.ContractEndDate == null*/)
                {
                    return;
                }
                if (person.DateOfJoin > asofDate /*|| person.ContractEndDate < asofDate*/)
                {
                    return;
                }

                var transactionViewModel = viewModel.SalaryTransactionList.FirstOrDefault(x => x.PersonId == person.PersonId
                   && x.ElementId == element.ElementId && x.EffectiveDate == asofDate
                   && x.PostedSource == PayrollPostedSourceEnum.Payroll);

                if (transactionViewModel == null)
                {
                    var lov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "DRAFT");
                    transactionViewModel = new PayrollTransactionViewModel
                    {
                        ElementId = element.ElementId,
                        DataAction = DataActionEnum.Create,
                        PostedSource = PayrollPostedSourceEnum.Payroll,
                        ProcessStatus = PayrollProcessStatusEnum.Draft,
                        ProcessStatusId = lov.Id,
                        PersonId = person.PersonId,
                        PostedUserId = ApplicationConstant.WindowsServiceUserId,
                        Amount = amount.RoundToTwoDecimal(),
                        EarningAmount = 0,
                        DeductionAmount = 0

                    };
                }
                else
                {
                    var lov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "DRAFT");
                    transactionViewModel.ElementId = element.ElementId;
                    transactionViewModel.DataAction = DataActionEnum.Edit;
                    transactionViewModel.PostedSource = PayrollPostedSourceEnum.Payroll;
                    transactionViewModel.ProcessStatus = PayrollProcessStatusEnum.Draft;
                    transactionViewModel.ProcessStatusId = lov.Id;
                    transactionViewModel.PersonId = person.PersonId;
                    transactionViewModel.PostedUserId = ApplicationConstant.WindowsServiceUserId;
                    transactionViewModel.Amount = amount.RoundToTwoDecimal();
                    transactionViewModel.EarningAmount = 0;
                    transactionViewModel.DeductionAmount = 0;
                }

                if (element.ElementClassification == ElementClassificationEnum.Earning)
                {
                    transactionViewModel.EarningAmount = transactionViewModel.Amount;
                }
                else if (element.ElementClassification == ElementClassificationEnum.Deduction)
                {
                    transactionViewModel.DeductionAmount = transactionViewModel.Amount;
                }
                transactionViewModel.Amount = transactionViewModel.EarningAmount - transactionViewModel.DeductionAmount;


                transactionViewModel.Name = element.Name;
                transactionViewModel.EffectiveDate = asofDate;
                transactionViewModel.PostedDate = DateTime.Now;
                transactionViewModel.ElementType = element.ElementType;
                transactionViewModel.ElementCategory = element.ElementCategory;
                transactionViewModel.ElementClassification = element.ElementClassification;
                transactionViewModel.PayrollRunId = viewModel.PayrollRunId;
                viewModel.SalaryTransactionList.Add(transactionViewModel);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task AddDailyTransaction(DateTime asofDate, double amount, PayrollPersonViewModel person,
         SalaryElementInfoViewModel salaryElementInfo, ElementViewModel element, PayrollTransactionViewModel viewModel)
        {
            try
            {
                if (person.DateOfJoin == null /*|| person.ContractEndDate == null*/)
                {
                    return;
                }
                if (person.DateOfJoin > asofDate /*|| person.ContractEndDate < asofDate*/)
                {
                    return;
                }
                if (person.LastWorkingDate!=null && person.LastWorkingDate < asofDate /*|| person.ContractEndDate < asofDate*/)
                {
                    return;
                }
                var transactionViewModel = viewModel.SalaryTransactionList.FirstOrDefault(x => x.PersonId == person.PersonId
                   && x.ElementId == salaryElementInfo.ElementId && x.EffectiveDate == asofDate
                   && x.PostedSource == PayrollPostedSourceEnum.Payroll);

                if (transactionViewModel == null)
                {
                    var lov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "DRAFT");
                    transactionViewModel = new PayrollTransactionViewModel
                    {
                        ElementId = element.ElementId,
                        DataAction = DataActionEnum.Create,
                        PostedSource = PayrollPostedSourceEnum.Payroll,
                        ProcessStatus = PayrollProcessStatusEnum.Draft,
                        ProcessStatusId= lov.Id,
                        PersonId = person.PersonId,
                        PostedUserId = ApplicationConstant.WindowsServiceUserId,
                        Amount = amount.RoundToTwoDecimal(),
                        EarningAmount = 0,
                        DeductionAmount = 0

                    };
                }
                else
                {
                    var lov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "DRAFT");
                    transactionViewModel.ElementId = element.ElementId;
                    transactionViewModel.DataAction = DataActionEnum.Edit;
                    transactionViewModel.PostedSource = PayrollPostedSourceEnum.Payroll;
                    transactionViewModel.ProcessStatus = PayrollProcessStatusEnum.Draft;
                    transactionViewModel.ProcessStatusId = lov.Id;
                    transactionViewModel.PersonId = person.PersonId;
                    transactionViewModel.PostedUserId = ApplicationConstant.WindowsServiceUserId;
                    transactionViewModel.Amount = amount.RoundToTwoDecimal();
                    transactionViewModel.EarningAmount = 0;
                    transactionViewModel.DeductionAmount = 0;
                }

                if (element.ElementClassification == ElementClassificationEnum.Earning)
                {
                    transactionViewModel.EarningAmount = transactionViewModel.Amount;
                }
                else if (element.ElementClassification == ElementClassificationEnum.Deduction)
                {
                    transactionViewModel.DeductionAmount = transactionViewModel.Amount;
                }
                transactionViewModel.Amount = transactionViewModel.EarningAmount - transactionViewModel.DeductionAmount;


                transactionViewModel.Name = element.Name;
                transactionViewModel.EffectiveDate = asofDate;
                transactionViewModel.PostedDate = DateTime.Now;
                transactionViewModel.ElementType = element.ElementType;
                transactionViewModel.ElementCategory = element.ElementCategory;
                transactionViewModel.ElementClassification = element.ElementClassification;
                transactionViewModel.PayrollRunId = viewModel.PayrollRunId;
                viewModel.SalaryTransactionList.Add(transactionViewModel);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<PayrollTransactionViewModel>> GetBasicTransportAndFoodTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            var list = await _payRollQueryBusiness.GetBasicTransportAndFoodTransactions(minAttendanceDate, maxAttendanceDate);
            return list;
        }

        public async Task<List<PayrollTransactionViewModel>> BulkInsert(List<PayrollTransactionViewModel> viewModelList, bool idGenerated = true, bool doCommit = true)
        {
            //throw new NotImplementedException();

            List<long> transactionIds = null;
            var count = viewModelList.Count;
            if (count <= 0)
            {
                return viewModelList;
            }
            foreach (var item in viewModelList)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = item.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "PayrollTransaction";
                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

                noteModel.Json = JsonConvert.SerializeObject(item);
                noteModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                var result = await _noteBusiness.ManageNote(noteModel);
            }
            return viewModelList;
        }

        public async Task<IList<PayrollTransactionViewModel>> IsPayrollTransactionBasedonElementExist(string personId, DateTime EffectiveDate, string ElementCode)
        {
            var result = await _payRollQueryBusiness.IsPayrollTransactionBasedonElementExist(personId, EffectiveDate, ElementCode);
            return result;
        }

        public async Task<PayrollTransactionViewModel> GetPayrollTransationDataById(string id)
        {
            var result = await _payRollQueryBusiness.GetPayrollTransationDataById(id);
            return result;
        }
        public async Task<PayrollTransactionViewModel> GetPayrollTransationDataByReferenceId(string referenceId)
        {
            var queryData = await _payRollQueryBusiness.GetPayrollTransationDataByReferenceId(referenceId);
            return queryData;
        }

        public async Task<List<PayrollTransactionViewModel>> GetAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            var result = await _payRollQueryBusiness.GetAccrualTransactionTransactions(minAttendanceDate, maxAttendanceDate);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetEosAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            var result = await _payRollQueryBusiness.GetEosAccrualTransactionTransactions(minAttendanceDate, maxAttendanceDate);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetVacationAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            var result = await _payRollQueryBusiness.GetVacationAccrualTransactionTransactions(minAttendanceDate, maxAttendanceDate);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetTicketEarningTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            var result = await _payRollQueryBusiness.GetTicketEarningTransactions(minAttendanceDate, maxAttendanceDate);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetOtandDedcutionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            var result = await _payRollQueryBusiness.GetOtandDedcutionTransactions(minAttendanceDate, maxAttendanceDate);
            return result;
        }

        public async Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionListByDates(PayrollTransactionViewModel search)
        {
            

            string personId = null;

            if (search.PersonId.IsNotNull())
            {
                personId = search.PersonId;
            }
            var result = await _payRollQueryBusiness.GetPayrollTransactionListByDates(search, personId);
            return result;
        }

        public async Task<PayrollTransactionViewModel> IsTransactionExists(string personId, string elementCode, DateTime date)
        {
            var list = await _payRollQueryBusiness.IsTransactionExists(personId, elementCode, date);
            return list;
        }

        public async Task<List<PayrollTransactionViewModel>> GetLoanAccrualTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            var result = await _payRollQueryBusiness.GetLoanAccrualTransactions(minAttendanceDate, maxAttendanceDate);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetSickLeaveAccrualTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            var result = await _payRollQueryBusiness.GetSickLeaveAccrualTransactions(minAttendanceDate, maxAttendanceDate);
            return result;
        }

        public async Task<bool> CloseTransaction(string Id, bool isClosed)
        {


            await _payRollQueryBusiness.CloseTransaction(Id, isClosed);
            return true;

        }
        public async Task DeleteIfAnyNotProcessedTrnasaction(ServiceTemplateViewModel viewModel)
        {
            try
            {
                if (viewModel.IsNotNull())
                {
                    //var personBusiness = BusinessHelper.GetInstance<IPersonBusiness>();
                    var hrCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                    var anniversaryStartDate = await hrCoreBusiness.GetCurrentAnniversaryStartDateByUserId(viewModel.OwnerUserId);



                    //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
                    //                            where pt.ReferenceId ={RefId} and pt.ProcessStatus = 'NotProcessed' and date(datetime(pt.EffectiveDate)) > date(datetime({date}))
                    //                            return pt";

                    //var prms = new Dictionary<string, object>
                    //{
                    //    { "RefId", viewModel.Id },
                    //    { "date", anniversaryStartDate },
                    //};

                    var result = await _payRollQueryBusiness.GetPayrollTxnDatabyDate(viewModel, anniversaryStartDate);
                    if (result.IsNotNull())
                    {
                        foreach (var trans in result)
                        {
                            await _noteBusiness.Delete(trans.NtsNoteId);
                            var result1 = await _payRollQueryBusiness.UpdatePayrollTxnByNoteId(trans);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
