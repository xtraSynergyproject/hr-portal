using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using CMS.UI.ViewModel.Pay;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class PayrollRunBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollRunBusiness
    {
        private readonly IRepositoryQueryBase<PayrollRunViewModel> _payrunrepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _paybatchrepo;
        private readonly IServiceProvider _sp;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly IPayrollTransactionsBusiness _payrollTransactionBusiness;
        private readonly IRepositoryQueryBase<PayrollSalaryElementViewModel> _paysalelerepo;
        private readonly IRepositoryQueryBase<PayrollElementRunResultViewModel> _payelerunresrepo;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ILeaveBalanceSheetBusiness _leaveBalanceBusiness;
        private readonly IRepositoryQueryBase<CalendarViewModel> _calRepo;
        private readonly IRepositoryQueryBase<CalendarHolidayViewModel> _holidayrepo;
        private readonly IRepositoryQueryBase<SalaryEntryViewModel> _salaryEntryRepo;
        private readonly IRepositoryQueryBase<MandatoryDeductionElementViewModel> _mandatoryDeductionElementRepo;
        private readonly IRepositoryQueryBase<MandatoryDeductionSlabViewModel> _mandatoryDeductionSlabRepo;
        private readonly IRepositoryQueryBase<SalaryElementEntryViewModel> _salaryElementEntryRepo;
        public PayrollRunBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IRepositoryQueryBase<PayrollRunViewModel> payrunrepo, IRepositoryQueryBase<MandatoryDeductionElementViewModel> mandatoryDeductionElementRepo,
            IPayrollBatchBusiness payrollBatchBusiness, IRepositoryQueryBase<PayrollSalaryElementViewModel> paysalelerepo,
            IRepositoryQueryBase<PayrollElementRunResultViewModel> payelerunresrepo,
            INoteBusiness noteBusiness, IUserContext userContext, IRepositoryQueryBase<PayrollBatchViewModel> paybatchrepo,
            IMapper autoMapper, IPayrollTransactionsBusiness payrollTransactionBusiness
            , IServiceProvider sp, ITableMetadataBusiness tableMetadataBusiness,
            ILeaveBalanceSheetBusiness leaveBalanceBusiness,
              IRepositoryQueryBase<CalendarViewModel> calRepo,
            IRepositoryQueryBase<CalendarHolidayViewModel> holidayrepo
            , ILOVBusiness lovBusiness
            , IRepositoryQueryBase<SalaryEntryViewModel> salaryEntryRepo,
            IRepositoryQueryBase<MandatoryDeductionSlabViewModel> mandatoryDeductionSlabRepo
            , IRepositoryQueryBase<SalaryElementEntryViewModel> salaryElementEntryRepo) : base(repo, autoMapper)
        {
            _payrunrepo = payrunrepo;
            _paysalelerepo = paysalelerepo;
            _payelerunresrepo = payelerunresrepo;
            _payrollBatchBusiness = payrollBatchBusiness;
            _payrollTransactionBusiness = payrollTransactionBusiness;
            _sp = sp;
            _tableMetadataBusiness = tableMetadataBusiness;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _paybatchrepo = paybatchrepo;
            _lovBusiness = lovBusiness;
            _leaveBalanceBusiness = leaveBalanceBusiness;
            _calRepo = calRepo;
            _holidayrepo = holidayrepo;
            _salaryEntryRepo = salaryEntryRepo;
            _salaryElementEntryRepo = salaryElementEntryRepo;
            _mandatoryDeductionElementRepo = mandatoryDeductionElementRepo;
            _mandatoryDeductionSlabRepo = mandatoryDeductionSlabRepo;
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

        public async Task<PayrollRunViewModel> GetSingleById(string payrollRunId)
        {
            var query = @$"Select *,""NtsNoteId"" as NoteId from cms.""N_PayrollHR_PayrollRun"" 
                            where ""Id""='{payrollRunId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>(query, null);
            return result;
        }

        public async Task<List<MandatoryDeductionElementViewModel>> GetSingleElementById(string mandatoryDeductionId)
        {
            

            
                //index
                //var query = @$"Select * from cms.""F_PAY_HR_HR_MANDATORY_DEDUCTION_ELEMENT""
                //            where ""MandatoryDeductionId""='{mandatoryDeductionId}' and ""IsDeleted""=false ";

                var full_query = @$"Select mde.*, pe.""ElementName"" as Name 
                                    from cms.""F_PAY_HR_HR_MANDATORY_DEDUCTION_ELEMENT"" as mde
                                    inner join cms.""N_PayrollHR_PayrollElement"" as pe on pe.""Id""=mde.""PayRollElementId"" and pe.""IsDeleted""=false and pe.""CompanyId""='{_repo.UserContext.CompanyId}'
                                    where mde.""MandatoryDeductionId""='{mandatoryDeductionId}' and mde.""IsDeleted""=false and mde.""CompanyId"" = '{_repo.UserContext.CompanyId}'";
            var result = await _mandatoryDeductionElementRepo.ExecuteQueryList<MandatoryDeductionElementViewModel>(full_query, null);
                return result;
           
        }

        public async Task<List<IdNameViewModel>> GetPayRollElementName()
        {
            //edit
            var query = $@"Select ""Id"" as Id, ""ElementName"" as Name from cms.""N_PayrollHR_PayrollElement""
                            where ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _mandatoryDeductionElementRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<MandatoryDeductionSlabViewModel> GetSlabForMandatoryDeduction(string mandatoryDeductionId, double amount,DateTime asofDate)
        {
            var query = @$"Select * from cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB""
                            where ""MandatoryDeductionId""='{mandatoryDeductionId}' and ""SlabFrom""::DECIMAL<='{amount}'::DECIMAL and""SlabTo""::DECIMAL>='{amount}'::DECIMAL  and ""IsDeleted""=false and ""ESD""::DATE<='{asofDate}'::DATE and ""EED""::DATE>='{asofDate}'::DATE";
            var result = await _mandatoryDeductionSlabRepo.ExecuteQuerySingle<MandatoryDeductionSlabViewModel>(query, null);
            return result;
        }

        public async Task<MandatoryDeductionElementViewModel> GetSingleElementEntryById(string id)
        {
            //edit
            var query = $@"Select * from cms.""F_PAY_HR_HR_MANDATORY_DEDUCTION_ELEMENT""
                            where ""Id""='{id}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}' ";
            var result = await _mandatoryDeductionElementRepo.ExecuteQuerySingle<MandatoryDeductionElementViewModel>(query, null);
            return result;
        }

        public async Task CreateMandatoryDeductionElement(MandatoryDeductionElementViewModel model)
        {
            //create
            var _cmsBusiness = _sp.GetService<ICmsBusiness>();
            dynamic exo = new System.Dynamic.ExpandoObject();
            ((IDictionary<String, Object>)exo).Add("Id", model.Id);
            ((IDictionary<String, Object>)exo).Add("MandatoryDeductionId", model.MandatoryDeductionId);
            ((IDictionary<String, Object>)exo).Add("PayRollElementId", model.PayRollElementId);
            ((IDictionary<String, Object>)exo).Add("ESD", model.ESD);
            ((IDictionary<String, Object>)exo).Add("EED", model.EED);
            //((IDictionary<String, Object>)exo).Add("LegalEntityId", model.LegalEntityId);
            var Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            var create = await _cmsBusiness.CreateForm(Json, "", "MANDATORY_DEDUCTION_ELEMENT");

        }

        public async Task EditMandatoryDeductionElement(MandatoryDeductionElementViewModel model)
        {
            //edit
            var _cmsBusiness = _sp.GetService<ICmsBusiness>();
            dynamic exo = new System.Dynamic.ExpandoObject();
            //((IDictionary<String, Object>)exo).Add("Id", model.Id);
            //((IDictionary<String, Object>)exo).Add("MandatoryDeductionId", model.MandatoryDeductionId);
            //((IDictionary<String, Object>)exo).Add("PayRollElementId", model.PayRollElementId);
            //((IDictionary<String, Object>)exo).Add("ESD", model.ESD);
            //((IDictionary<String, Object>)exo).Add("EED", model.EED);
            var Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var edit = await _cmsBusiness.EditForm(model.Id, Json, "", "MANDATORY_DEDUCTION_ELEMENT");

        }

        public async Task DeleteMandatoryDeductionElement(string Id)
        {
            var query = $@"Update cms.""F_PAY_HR_HR_MANDATORY_DEDUCTION_ELEMENT"" set ""IsDeleted""=true where ""Id""='{Id}' and ""CompanyId""='{_repo.UserContext.CompanyId}' ";
            await _mandatoryDeductionElementRepo.ExecuteCommand(query, null);
        }

        private async Task<PayrollRunViewModel> GetNextPayroll(string payrollRunId)
        {
            //cypher = @"match(r:PAY_PayrollRun{IsDeleted: 0,ExecutionStatus:'Submitted'}) 
            //match (r)-[:R_PayrollRun_Payroll]->(p:PAY_Payroll)  
            //match (p)-[:R_Payroll_LegalEntity_OrganizationRoot]->(or:HRS_OrganizationRoot)  
            //match (p)-[:R_Payroll_PayrollGroup]->(pg:PAY_PayrollGroup)  
            //with r,p,or,pg order by r.CreatedDate limit 1  with r,p,or,pg 
            //set r.ExecutionStatus='InProgress',r.PayrollRunDate={PayrollRunDate} 
            //return  r,p.Id as PayrollId
            //,or.Id as LegalEntityId,pg.Id as PayrollGroupId,p.PayrollStartDate as PayrollStartDate,p.PayrollEndDate as PayrollEndDate
            //,p.AttendanceStartDate as AttendanceStartDate,p.AttendanceEndDate as AttendanceEndDate
            //,p.YearMonth as YearMonth,p.RunType as RunType";
            //// Log.Instance.Info("End GetNextPayroll");
            //return repository.ExecuteCypher<PayrollRunViewModel>(cypher, prms);

            int exeStatus = (int)PayrollExecutionStatusEnum.InProgress;
            DateTime payrollRunDate = DateTime.Now;
            var query = $@"Update cms.""N_PayrollHR_PayrollRun"" set ""PayrollRunDate""='{payrollRunDate}', ""ExecutionStatus""='{exeStatus}'
                        where ""Id""='{payrollRunId}' ";
            await _payrunrepo.ExecuteCommand(query, null);

            var cypher = $@"select pr.*,p.""Id"" as PayrollId,p.""PayrollStartDate"" as PayrollStartDate
                ,p.""PayrollEndDate"" as PayrollEndDate,p.""AttendanceStartDate"" as AttendanceStartDate,p.""AttendanceEndDate"" as AttendanceEndDate
                ,p.""YearMonth"" as YearMonth,p.""RunType"" as RunType
                ,pg.""Id"" as PayrollGroupId ,le.""Id"" as LegalEntityId 
                from cms.""N_PayrollHR_PayrollRun"" as pr
                join cms.""N_PayrollHR_PayrollBatch"" as p on p.""Id""=pr.""PayrollBatchId"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=p.""PayrollGroupId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LegalEntity"" as le on le.""Id""=p.""LegalEntityId""  and le.""IsDeleted""=false and le.""CompanyId""='{_repo.UserContext.CompanyId}'
                where pr.""IsDeleted""=false and pr.""Id""='{payrollRunId}' and pr.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            var queryData = await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
            return queryData;

        }
        private CommandResult<PayrollRunViewModel> ValidatePayrollRun(PayrollRunViewModel viewModel)
        {
            var errorList = new Dictionary<string, string>();

            //if ((viewModel.PayrollStateEnd == PayrollStateEnum.NotStarted || viewModel.PayrollStateEnd == PayrollStateEnum.ExecutePayroll) &&
            //    viewModel.PersonsNotInList.IsNullOrEmpty() && viewModel.PersonsInList.IsNullOrEmpty())
            //{
            //    errorList.Add("Person", "Please select atleast one person from the list");
            //}
            //if (errorList.Count > 0)
            //{
            //    return CommandResult<PayrollRunViewModel>.Instance(viewModel, false, errorList);
            //}

            return CommandResult<PayrollRunViewModel>.Instance();
        }

        //public async Task<IList<PayrollRunViewModel>> GetPayrollRunList()
        //{            
        //    //var cypher = @"match (p:PAY_Payroll)<-[:R_PayrollRun_Payroll]-(pr:PAY_PayrollRun{ IsDeleted: 0}) 
        //    //    match(p)-[:R_Payroll_LegalEntity_OrganizationRoot]->(le:HRS_OrganizationRoot{ Id:{LegalEntityId}}) 
        //    //    match(p)-[:R_Payroll_PayrollGroup]->(pg:PAY_PayrollGroup{ IsDeleted: 0})     
        //    //      return pr,p.Id as PayrollId,p.PayrollStartDate as PayrollStartDate
        //    //    ,p.PayrollEndDate as PayrollEndDate,pg.Id as PayrollGroupId ,le.Id as LegalEntityId 
        //    //    order by p.PayrollStartDate desc,p.LastUpdatedDate desc";

        //    var query = $@"Select pr.*, pb.""Id"" as PayrollId, pb.""PayrollStartDate"", pb.""PayrollEndDate"", pg.""Id"" as PayrollGroupId
        //                    From cms.""N_PayrollHR_PayrollRun"" as pr
        //                    Join cms.""N_PayrollHR_PayrollBatch"" as pb on pb.""Id""=pr.""PayrollBatchId""
        //                    Join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=pb.""PayrollGroupId""
        //                    order by pb.""PayrollStartDate"" desc, pb.""LastUpdatedDate"" desc";

        //    var result = await _payrunrepo.ExecuteQueryList(query, null);
        //    return result;
        //}

        public async Task<CommandResult<PayrollRunViewModel>> Correct(PayrollRunViewModel viewModel)
        {
            var errorList = new Dictionary<string, string>();

            var existingPayrollRun = await GetSingleById(viewModel.Id);
            if (existingPayrollRun != null && (existingPayrollRun.ExecutionStatus == PayrollExecutionStatusEnum.Submitted || existingPayrollRun.ExecutionStatus == PayrollExecutionStatusEnum.InProgress))
            {
                errorList.Add("ExecutionStatus", "The payroll is already submitted and still executing in the background. Please wait for completion of execution");
                return CommandResult<PayrollRunViewModel>.Instance(viewModel, false, errorList);
            }
            if (viewModel.PayrollStateEnd == PayrollStateEnum.AddToPayroll)
            {
                await AddPersonToPayrollRun(viewModel.Id, viewModel.PersonsNotInList, viewModel.NoteId);
                return CommandResult<PayrollRunViewModel>.Instance();
            }
            viewModel.ExecutionStatus = PayrollExecutionStatusEnum.Submitted;

            var payrollModel = await _payrollBatchBusiness.ViewModelList(viewModel.PayrollBatchId);
            var payroll = payrollModel.FirstOrDefault();


            var validateName = ValidatePayrollRun(viewModel);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PayrollRunViewModel>.Instance(viewModel, false, errorList);
            }
            if (errorList.Count() > 0)
            {
                return CommandResult<PayrollRunViewModel>.Instance(viewModel, false, errorList);
            }

            viewModel.PayrollStateStart = viewModel.PayrollStateEnd;

            //if (viewModel.PayrollStateEnd == PayrollStateEnum.InitiateService)
            //{
            //    InitiatePayrollService(viewModel);
            //}

            if (viewModel.PayrollStateEnd == PayrollStateEnum.FreezePayroll
                || viewModel.PayrollStateEnd == PayrollStateEnum.ClosePayroll
                || viewModel.PayrollStateEnd == PayrollStateEnum.InitiateService)
            {
                viewModel.ExecutionStatus = PayrollExecutionStatusEnum.Completed;
            }

            int stateEnd = (int)viewModel.PayrollStateEnd;
            int exeStatus = (int)viewModel.ExecutionStatus;
            var query = $@"Update cms.""N_PayrollHR_PayrollRun"" set ""PayrollStateStart""='{stateEnd}', ""PayrollStateEnd""='{stateEnd}',
                            ""ExecutionStatus""='{exeStatus}' where ""Id""='{viewModel.Id}' ";

            await _payrunrepo.ExecuteCommand(query, null);

            if (viewModel.PayrollStateEnd == PayrollStateEnum.ExecutePayroll || viewModel.PayrollStateEnd == PayrollStateEnum.NotStarted)
            {

                if (!viewModel.PersonsInList.IsNullOrEmpty())
                {
                    var existPersons = await GetEmployeeListForPayrollRunData(null, viewModel.PayrollGroupId, viewModel.PayrollBatchId, viewModel.Id, null);
                    var existingids = existPersons.Select(x => x.PersonId).ToArray();

                    var perStr = viewModel.PersonsInList;
                    string[] pers = perStr.Split(',').Distinct().ToArray();

                    var newids = pers.Except(existingids);
                    var str = "";
                    foreach (var id in newids)
                    {
                        str = string.Join(",", id);
                    }

                    await AddPersonToPayrollRun(viewModel.Id, str, viewModel.NoteId);
                }

                if (!viewModel.PersonsNotInList.IsNullOrEmpty())
                {
                    await AddPersonToPayrollRun(viewModel.Id, viewModel.PersonsNotInList, viewModel.NoteId);
                }
            }
            if (payroll != null)
            {
                payroll.Name = viewModel.Name;
                payroll.Description = viewModel.Description;
                if (viewModel.PayrollStateEnd == PayrollStateEnum.ClosePayroll)
                {
                    payroll.PayrollStatus = PayrollStatusEnum.Completed;
                }
                int payrollstatus = (int)payroll.PayrollStatus;
                var queryBatch = $@"Update cms.""N_PayrollHR_PayrollBatch"" set ""PayrollStatus""='{payrollstatus}' 
                                    where ""Id""='{payroll.Id}' ";

                await _paybatchrepo.ExecuteCommand(queryBatch, null);
            }
            if ((viewModel.PayrollStateEnd == PayrollStateEnum.RollBack || viewModel.PayrollStateEnd == PayrollStateEnum.ExecutePayroll) && viewModel.ExecutionStatus == PayrollExecutionStatusEnum.Submitted)
            {
                // Status Changes to inprogress
                var cypher = $@"update cms.""N_PayrollHR_PayrollRun"" set ""ExecutionStatus""='{PayrollExecutionStatusEnum.InProgress}' where ""Id""='{viewModel.Id}'";
                await _salaryEntryRepo.ExecuteCommand(cypher, null);
                await ExecutePayroll(viewModel.Id);

            }
            return CommandResult<PayrollRunViewModel>.Instance(viewModel);
        }


        public async Task<List<PayrollElementRunResultViewModel>> GetStandardElementListForPayrollRunData(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth, string personIds = null)
        {
            if (personIds.IsNullOrEmpty())
            {
                var employees = GetEmployeeListForPayrollRunData(payrollVM, payrollGroupId, payrollId, payrollRunId, yearMonth);
                personIds = string.Join("','", employees.Result.Select(x => x.PersonId).ToArray());
                //personIds = personIds.Replace(",", "','");
            }
            //var match1 = string.Concat(@"match(pr:HRS_PersonRoot{IsDeleted: 0})<-[:R_SalaryInfoRoot_PersonRoot]-(sir:PAY_SalaryInfoRoot{IsDeleted: 0})
            //    where pr.Id in [" + personIds + @"]
            //    match(sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo{IsDeleted:0,Status:'Active'}) 
            //     where si.EffectiveStartDate<={PED} and si.EffectiveEndDate>={PSD}
            //    match (sir)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(sei:PAY_SalaryElementInfo{IsDeleted: 0,Status:'Active'})
            //    -[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot{IsDeleted: 0,Status:'Active'})
            //    <-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:'Active',ElementCategory:'Standard'})
            //   where e.EffectiveStartDate<={PED} and e.EffectiveEndDate>={PSD}
            //   return   pr.Id as PersonId,e.Name as Name,coalesce(e.DisplayName,e.Name) as DisplayName,sei.Amount as Amount order by pr.Id,e.ElementClassification desc,e.SequenceNo,e.Name");

            var match1 = $@"Select pr.""Id"" as PersonId, e.""ElementName"" as Name, coalesce(e.""ElementDisplayName"",e.""ElementName"") as DisplayName,e.""ElementClassification"", sei.""Amount""
                            From cms.""N_CoreHR_HRPerson"" as pr
                            join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId""  and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" and e.""ElementCategory""='Standard' and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            --and e.""EffectiveStartDate""::TIMESTAMP::DATE<='{payrollVM.PayrollEndDate}'::TIMESTAMP::DATE and e.""EffectiveEndDate""::TIMESTAMP::DATE >='{payrollVM.PayrollStartDate}'::TIMESTAMP::DATE
                            where pr.""Id"" in ('{personIds}') and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' order by pr.""Id"",e.""ElementClassification"" desc, e.""ElementName"" ";

            var result = await _payelerunresrepo.ExecuteQueryList(match1, null);
            return result;
        }

        public async Task<IList<PayrollRunViewModel>> GetPayrollBatchList()
        {
            var query = @$"Select pr.*, pb.""PayrollStartDate"", pb.""PayrollEndDate"" from cms.""N_PayrollHR_PayrollRun"" as pr
                            join cms.""N_PayrollHR_PayrollBatch"" as pb on pb.""Id""=pr.""PayrollBatchId"" and pb.""IsDeleted""=false and pb.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' order by pr.""CreatedDate"" desc ";

            var result = await _payrunrepo.ExecuteQueryList<PayrollRunViewModel>(query, null);
            return result;
        }

        public async Task<List<PayrollRunViewModel>> ViewModelList(string PayrollRunId)
        {

            //var match = @"match (p:PAY_Payroll)<-[:R_PayrollRun_Payroll]-(pr:PAY_PayrollRun{ IsDeleted: 0,CompanyId: { CompanyId}}) 
            //    match(pr)-[:R_PayrollRun_ExecutedBy_User]->(u:ADM_User{ IsDeleted: 0,CompanyId: { CompanyId} })               
            //    match(p)-[:R_Payroll_PayrollGroup]->(pg:PAY_PayrollGroup{ IsDeleted: 0,CompanyId: { CompanyId} })               
            //    with pr,p,pg";

            var query = $@"select pr.*,pr.""NtsNoteId"" as NoteId, pb.""Id"" as PayrollBatchId, pb.""PayrollStartDate""::TIMESTAMP::DATE, pb.""AttendanceStartDate"",pb.""AttendanceEndDate"",
                           pb.""PayrollEndDate""::TIMESTAMP::DATE, pg.""Id"" as PayrollGroupId
                           from cms.""N_PayrollHR_PayrollRun"" as pr
                           join cms.""N_PayrollHR_PayrollBatch"" as pb on pb.""Id""=pr.""PayrollBatchId"" and pb.""IsDeleted""=false and pb.""CompanyId""='{_repo.UserContext.CompanyId}'
                           join public.""User"" as u on u.""Id"" = pr.""ExecutedBy""  and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                           join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=pb.""PayrollGroupId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
                           where pr.""Id""='{PayrollRunId}' and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' order by pb.""PayrollStartDate"" desc ";

            var result = await _payrunrepo.ExecuteQueryList(query, null);

            return result;

        }

        //public IList<PaySalaryElementViewModel> GetPayrollRunData(long payrollGroupId, long payrollId, long payrollRunId, DateTime asofDate, int yearMonth)
        //{

        //    // var pay = _payrollGroupBusiness.GetSingleById(payrollGroupId);
        //    var param = new Dictionary<string, object>
        //    {
        //        { "Id", payrollId }
        //    };
        //    var payroll = _payrollBusiness.ViewModelList<PayrollBatchViewModel>("pr.Id={Id}", param, "").FirstOrDefault();
        //    //var payroll = _payrollBusiness.GetSingleById(payrollId);
        //    //var date = payroll.PayrollStartDate.Value;
        //    //var endMonth = date;
        //    //// var edmonth = date.Month;
        //    //if (pay.IsCutOffStartDayPreviousMonth)
        //    //    endMonth = endMonth.AddMonths(-1);

        //    //payroll.AttendanceStartDate = new DateTime(endMonth.Year, endMonth.Month, pay.CutOffStartDay);
        //    //payroll.AttendanceEndDate = new DateTime(date.Year, date.Month, pay.CutOffEndDay);

        //    var employees = GetEmployeeListForPayrollRunData(payroll, payrollGroupId, payrollId, payrollRunId, yearMonth);
        //    var personIds = string.Join(",", employees.Select(x => x.PersonId).ToArray());

        //    var elementList = GetStandardElementListForPayroll(payroll, payrollGroupId, payrollId, payrollRunId, yearMonth, personIds);
        //    var distinctList = elementList.Select(x => x.Name).Distinct().ToList();
        //    foreach (var item in employees)
        //    {
        //        int i = 1;
        //        double gross = 0.0;
        //        foreach (var element in distinctList)
        //        {
        //            var col = string.Concat("Element", i);
        //            var e = elementList.FirstOrDefault(x => x.PersonId == item.PersonId && x.Name == element);
        //            if (e != null)
        //            {
        //                GeneralExtension.SetPropertyValue(item, col, e.Amount);
        //                gross += e.Amount;
        //            }
        //            else
        //            {
        //                GeneralExtension.SetPropertyValue(item, col, 0);
        //            }
        //            i++;
        //        }
        //        item.GrossSalary = gross.RoundPayrollSummaryAmount();
        //    }
        //    return employees;
        //}

        //private List<PaySalaryElementViewModel> GetEmployeeListForPayrollRunData(PayrollBatchViewModel payrollVM, long payrollGroupId, long payrollId, long payrollRunId, int yearMonth)
        //{
        //    var prms = new Dictionary<string, object>
        //    {
        //        { "Status", StatusEnum.Active },
        //        { "CompanyId", CompanyId },
        //        { "PayrollRunId", payrollRunId },
        //        { "PayrollId", payrollId },
        //        { "payrollGroupId", payrollGroupId },
        //        { "yearMonth", yearMonth},
        //        { "startDate", payrollVM.AttendanceStartDate },
        //        { "endDate", payrollVM.AttendanceEndDate},
        //        { "PSD", payrollVM.PayrollStartDate},
        //        { "PED", payrollVM.PayrollEndDate},
        //    };
        //    var match = string.Concat(@"match(prr:PAY_PayrollRun{Id:{PayrollRunId},IsDeleted: 0,CompanyId: { CompanyId}})-[R_PayrollRun_PersonRoot]
        //        ->(pr:HRS_PersonRoot{IsDeleted: 0})<-[:R_PersonRoot]-(p:HRS_Person)
        //        where p.EffectiveStartDate<={PED} and p.EffectiveEndDate>={PSD}
        //        match(pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]-(leor:HRS_OrganizationRoot{ IsDeleted:0,Status:'Active'})
        //        <-[:R_OrganizationRoot]-(leo:HRS_Organization{ IsDeleted:0,Status:'Active'})
        //        where leo.EffectiveStartDate<={PED} and leo.EffectiveEndDate>={PSD}
        //        match (pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0,Status:'Active'})
        //        match(cr)<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0})
        //        where c.EffectiveStartDate<={PED} and c.EffectiveEndDate>={PSD}
        //        match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{IsDeleted:0,Status:'Active'})
        //        match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0})
        //        where a.EffectiveStartDate<={PED} and a.EffectiveEndDate>={PSD}          
        //        match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,Status:'Active'})
        //        match(jr)<-[:R_JobRoot]-(j:HRS_Job{IsDeleted:0,Status:'Active'})
        //        where j.EffectiveStartDate<={PED} and j.EffectiveEndDate>={PSD}          
        //        match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{IsDeleted:0,Status:'Active'})
        //        match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,Status:'Active'})
        //        where o.EffectiveStartDate<={PED} and o.EffectiveEndDate>={PSD}
        //        match(a)-[:R_Assignment_GradeRoot]->(gr:HRS_GradeRoot{ IsDeleted:0,Status:'Active'})
        //        match(gr)<-[:R_GradeRoot]-(g:HRS_Grade{ IsDeleted:0,Status:'Active'})
        //        where g.EffectiveStartDate<={PED} and g.EffectiveEndDate>={PSD}
        //        match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,CompanyId:{CompanyId}}) 
        //        optional match(o)-[:R_Organization_Payroll_OrganizationRoot]-(payor:HRS_OrganizationRoot{ IsDeleted:0,Status:'Active'})
        //        <-[:R_OrganizationRoot]-(payo:HRS_Organization{ IsDeleted:0,Status:'Active'})
        //        where payo.EffectiveStartDate<={PED} and payo.EffectiveEndDate>={PSD}
        //        with pr,p,ar,a,jr,j,orr,o,prr,payor,payo,g,leo
        //        return distinct p,o.Name as OrganizationName,payo.Name as PayrollOrganizationName,j.Name as JobName,a.DateOfJoin as DateOfJoin
        //        ,leo.Name as LegalEntityName, p.PersonNo as PersonNo, p.SponsorshipNo as SponsorshipNo
        //        ,g.Name as GradeName,pr.Id as PersonId,", Helper.PersonDisplayName("p", " as PersonName")
        //        , " order by a.DateOfJoin");

        //    var query = $@"Select Distinct p.*, o.""Name"" as OrganizationName, payo.""Name"" as PayrollOrganizationName, j.""Name"" as JobName,
        //                    a.""DateOfJoin"" as DateOfJoin, leo.""Name"" as LegalEntityName, p.""PersonNo"" as PersonNo, p.""SponsorshipNo"" as SponsorshipNo,
        //                    g.""Name"" as GradeName, pr.""Id"" as PersonId
        //                    From cms.""N_PayrollHR_PayrollRun"" as prr ";

        //    return ExecuteCypherList<PaySalaryElementViewModel>(match, prms).ToList();

        //}
        public async Task<IList<PayrollSalaryElementViewModel>> GetPayrollRunData(string payrollGroupId, string payrollId, string payrollRunId, DateTime? asofDate, int? yearMonth)
        {
            // var pay = _payrollGroupBusiness.GetSingleById(payrollGroupId);
            var param = new Dictionary<string, object>
                 {
                     { "Id", payrollId }
                 };
            var payrollmodel = await _payrollBatchBusiness.ViewModelList(payrollId);
            var payroll = payrollmodel.FirstOrDefault();

            var employees = await GetEmployeeListForPayrollRunData(payroll, payrollGroupId, payrollId, payrollRunId, yearMonth);

            //var personIds = string.Join(",", employees.Select(x => x.PersonId).ToArray());

            //var elementList = GetStandardElementListForPayroll(payroll, payrollGroupId, payrollId, payrollRunId, yearMonth, personIds);
            //var distinctList = elementList.Select(x => x.Name).Distinct().ToList();
            //foreach (var item in employees)
            //{
            //    int i = 1;
            //    double gross = 0.0;
            //    foreach (var element in distinctList)
            //    {
            //        var col = string.Concat("Element", i);
            //        var e = elementList.FirstOrDefault(x => x.PersonId == item.PersonId && x.Name == element);
            //        if (e != null)
            //        {
            //            GeneralExtension.SetPropertyValue(item, col, e.Amount);
            //            gross += e.Amount;
            //        }
            //        else
            //        {
            //            GeneralExtension.SetPropertyValue(item, col, 0);
            //        }
            //        i++;
            //    }
            //    item.GrossSalary = gross.RoundPayrollSummaryAmount();
            //}
            return employees;
        }
        public async Task<IList<PayrollRunViewModel>> GetPayrollRunList()
        {
            //var prms = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "LegalEntityId", LegalEntityId }
            //};
            //var cypher = @"match (p:PAY_Payroll)<-[:R_PayrollRun_Payroll]-(pr:PAY_PayrollRun{ IsDeleted: 0}) 
            //    match(p)-[:R_Payroll_LegalEntity_OrganizationRoot]->(le:HRS_OrganizationRoot{ Id:{LegalEntityId}}) 
            //    match(p)-[:R_Payroll_PayrollGroup]->(pg:PAY_PayrollGroup{ IsDeleted: 0})     
            //      return pr,p.Id as PayrollId,p.PayrollStartDate as PayrollStartDate
            //    ,p.PayrollEndDate as PayrollEndDate,pg.Id as PayrollGroupId ,le.Id as LegalEntityId 
            //    order by p.PayrollStartDate desc,p.LastUpdatedDate desc";
            var cypher = $@"select pr.*,p.""Id"" as PayrollId,p.""PayrollStartDate"" as PayrollStartDate
                ,p.""PayrollEndDate"" as PayrollEndDate,pg.""Id"" as PayrollGroupId ,le.""Id"" as LegalEntityId 
                from cms.""N_PayrollHR_PayrollRun"" as pr
                join cms.""N_PayrollHR_PayrollBatch"" as p on p.""Id""=pr.""PayrollBatchId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=p.""PayrollGroupId"" and pg.""IsDeleted""=false  and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
                 join public.""LegalEntity"" as le on le.""Id""=p.""LegalEntityId""  and le.""IsDeleted""=false and le.""Id""='{_repo.UserContext.LegalEntityId}' and le.""CompanyId""='{_repo.UserContext.CompanyId}'
where pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' order by p.""PayrollStartDate"" desc,p.""LastUpdatedDate"" desc";
            return await _payrunrepo.ExecuteQueryList<PayrollRunViewModel>(cypher, null);

        }
        public async Task<List<PayrollSalaryElementViewModel>> GetEmployeeListForPayrollRunData(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int? yearMonth)
        {
            var query = $@"Select Distinct p.*,p.""PersonFullName"" as PersonName, d.""DepartmentName"" as OrganizationName, payd.""DepartmentName"" as PayrollOrganizationName, j.""JobTitle"" as JobName,
                            a.""DateOfJoin"" as DateOfJoin, p.""PersonNo"" as PersonNo,
                            g.""GradeName"" as GradeName, p.""Id"" as PersonId
                            From cms.""N_PayrollHR_PayrollRun"" as prr
                            join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PayrollRunId""=prr.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=prp.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            
                            join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id""  and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false and g.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join cms.""N_CoreHR_HRDepartment"" as payd on payd.""PayrollDepartmentId""=d.""Id""  and payd.""IsDeleted""=false and payd.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where prr.""Id""='{payrollRunId}' and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}' order by a.""DateOfJoin"" ";

            var querydata = await _paysalelerepo.ExecuteQueryList(query, null);
            return querydata;

        }

        public async Task<IList<PayrollSalaryElementViewModel>> PayrollRunPersonData(string payrollGroupId, string payrollId, string payrollRunId, DateTime asofDate, int yearMonth)
        {

            var payrollmodel = await _payrollBatchBusiness.ViewModelList(payrollId);
            var payroll = payrollmodel.FirstOrDefault();

            //------
            //var date = payroll.PayrollStartDate.Value;
            //var endMonth = date;
            //// var edmonth = date.Month;
            //if (pay.IsCutOffStartDayPreviousMonth)
            //    endMonth = endMonth.AddMonths(-1);

            //payroll.AttendanceStartDate = new DateTime(endMonth.Year, endMonth.Month, pay.CutOffStartDay);
            //payroll.AttendanceEndDate = new DateTime(date.Year, date.Month, pay.CutOffEndDay);
            //--------------

            var employees = await GetEmployeeListForPayroll(payroll, payrollGroupId, payrollId, payrollRunId, yearMonth);
            //var personIds = string.Join(",", employees.Select(x => x.PersonId).ToArray());

            //var elementList = await GetStandardElementListForPayroll(payroll, payrollGroupId, payrollId, payrollRunId, yearMonth, personIds);
            //var distinctList = elementList.Select(x => x.Name).Distinct().ToList();
            //foreach (var item in employees)
            //{
            //    int i = 1;
            //    var netAmount = 0.0;
            //    foreach (var element in distinctList)
            //    {
            //        var col = string.Concat("Element", i);
            //        var e = elementList.FirstOrDefault(x => x.PersonId == item.PersonId && x.Name == element);
            //        if (e != null)
            //        {
            //            ApplicationExtension.SetPropertyValue(item, col, e.Amount);
            //            netAmount += item.Amount;
            //        }
            //        else
            //        {
            //            ApplicationExtension.SetPropertyValue(item, col, 0);
            //        }
            //        i++;
            //    }

            //    item.GrossSalary = netAmount.RoundPayrollSummaryAmount();
            //}
            return employees;
        }


        public async Task<List<PayrollSalaryElementViewModel>> GetEmployeeListForPayroll(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth)
        {
            var query = $@"Select Distinct p.""PersonFullName"" as PersonName,p.""Id"" as PersonId, d.""DepartmentName"" as OrganizationName, payd.""DepartmentName"" as PayrollOrganizationName, j.""JobTitle"" as JobName,
                        a.""DateOfJoin"" as DateOfJoin, g.""GradeName"" as GradeName, p.""PersonNo"" as PersonNo, prp.""Id""
                        From cms.""N_PayrollHR_PayrollGroup"" as pg
                        join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PayGroupId""=pg.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false and g.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_CoreHR_HRDepartment"" as payd on payd.""PayrollDepartmentId""=d.""Id"" and payd.""IsDeleted""=false and payd.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=p.""Id"" and prp.""PayrollRunId""='{payrollRunId}' and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where pg.""Id""='{payrollGroupId}' and prp.""Id"" is null and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}' order by a.""DateOfJoin"" ";
            //return ExecuteCypherList<PayrollSalaryElementViewModel>(match, prms).ToList();
            var querydata = await _paysalelerepo.ExecuteQueryList(query, null);
            return querydata;

        }

        public async Task<PayrollRunPersonViewModel> AddPersonToPayrollRun(string payrollRunId, string persons, string payRunNoteId)
        {
            var noteTemplate = new NoteTemplateViewModel();
            var model = new PayrollRunPersonViewModel { PayrollRunId = payrollRunId };

            noteTemplate.ActiveUserId = _repo.UserContext.UserId;
            noteTemplate.TemplateCode = "PAYROLL_RUN_PERSON";
            noteTemplate.DataAction = DataActionEnum.Create;
            noteTemplate.ParentNoteId = payRunNoteId;



            if (!persons.IsNullOrEmpty())
            {
                var perStr = persons.Trim(',');
                var pers = perStr.Split(',').Distinct();
                foreach (var per in pers)
                {
                    model.PersonId = per;
                    var note = await _noteBusiness.GetNoteDetails(noteTemplate);
                    note.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(note);
                }
            }
            return model;
        }

        public async Task<List<PayrollElementRunResultViewModel>> GetStandardElementListForPayroll(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth, string personIds = null)
        {
            if (personIds.IsNullOrEmpty())
            {
                var employees = await GetEmployeeListForPayroll(payrollVM, payrollGroupId, payrollId, payrollRunId, yearMonth);
                personIds = string.Join(",", employees.Select(x => x.PersonId).ToArray());
            }
            //var match1 = string.Concat(@"match(pr:HRS_PersonRoot{IsDeleted: 0})<-[:R_SalaryInfoRoot_PersonRoot]-(sir:PAY_SalaryInfoRoot{IsDeleted: 0})
            //    where pr.Id in [" + personIds + @"]
            //    match(sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo{IsDeleted:0,Status:'Active'}) 
            //    where si.EffectiveStartDate<={PED} and si.EffectiveEndDate>={PSD}
            //    match (sir)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(sei:PAY_SalaryElementInfo{IsDeleted:0,Status:'Active'})
            //    -[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot{IsDeleted: 0,Status:'Active'})
            //    <-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:'Active',ElementCategory:'Standard'})
            //   where e.EffectiveStartDate<={PED} and e.EffectiveEndDate>={PSD}
            //   return   pr.Id as PersonId,e.Name as Name,sei.Amount as Amount order by pr.Id,e.Name");
            //var prms1 = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "PSD", payrollVM.PayrollStartDate},
            //    { "PED", payrollVM.PayrollEndDate},
            //};
            //return ExecuteCypherList<PayrollElementRunResultViewModel>(match1, prms1).ToList();

            var query = $@"Select pr.""Id"" as PersonId, pe.""ElementName"" as Name, sei.""Amount"" as Amount
                        From cms.""N_CoreHR_HRPerson"" as pr
                        Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and  si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join public.""NtsNote"" as N on N.""ParentNoteId""=si.""NtsNoteId"" and N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=N.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_PayrollHR_PayrollElement"" as pe.""Id""=sei.""ElementId"" and pe.""IsDeleted""=false and pe.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Where pr.""Id"" in ('{personIds}') --and pe.""EffectiveStartDate"" <= '{payrollVM.PayrollEndDate}' and pe.""EffectiveEndDate"">='{payrollVM.PayrollStartDate}' 
and  pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var querydata = await _payelerunresrepo.ExecuteQueryList(query, null);
            return querydata;

        }


        ///------------------


        public async Task<PayrollRunViewModel> ExecutePayroll(string payrollRunId)
        {
            var exceutionStatus = PayrollExecutionStatusEnum.Completed;
            string error = string.Empty;
            var viewModel = new PayrollRunViewModel();
            try
            {
                if (payrollRunId.IsNotNullAndNotEmpty())
                {
                    //viewModel = await GetSingleById(payrollRunId);
                    viewModel = await GetNextPayroll(payrollRunId);
                }
                // viewModel.RunAccrual = true;
                if (viewModel.RunAccrual)
                {
                    await ManageAccruals(viewModel);
                    // Log.Instance.Info(DelimeterEnum.Space, "End ExecutePayrollRun Accrual");
                    return viewModel;
                }
                await LoadGeneralData(viewModel);
                switch (viewModel.PayrollStateEnd)
                {
                    case PayrollStateEnum.NotStarted:
                        break;
                    case PayrollStateEnum.ExecutePayroll:
                        await RunPayroll(viewModel);
                        await GenerateSalary(viewModel);
                        await PrepareBankLetter(viewModel);
                        await PublishPayroll(viewModel);
                        break;
                    case PayrollStateEnum.RollBack:
                        await RollbackPayroll(viewModel);
                        break;
                    case PayrollStateEnum.InitiateService:
                        break;
                    case PayrollStateEnum.FreezePayroll:
                        break;
                    case PayrollStateEnum.GeneratePayslip:
                        break;
                    case PayrollStateEnum.PrepareBankLetter:
                        break;
                    case PayrollStateEnum.PublishPayroll:
                        await GenerateSalary(viewModel);
                        await PrepareBankLetter(viewModel);
                        await PublishPayroll(viewModel);
                        break;
                    case PayrollStateEnum.PostToAccounting:
                        break;
                    case PayrollStateEnum.ClosePayroll:
                        await ClosePayroll(viewModel);
                        break;
                    case PayrollStateEnum.AddToPayroll:
                        break;
                    default:
                        break;
                }

            }
            catch (Exception e)
            {
                exceutionStatus = PayrollExecutionStatusEnum.Error;
                error = e.ToString();
            }
            finally
            {
                // CHange to Complete
                var cypher1 = $@"update cms.""N_PayrollHR_PayrollRun"" set ""ExecutionStatus""='{exceutionStatus}',""ExecutePayrollError""='{error}' where ""Id""='{payrollRunId}'";
                await _salaryEntryRepo.ExecuteCommand(cypher1, null);
            }
            return viewModel;
        }
        private async Task LoadGeneralData(PayrollRunViewModel viewModel)
        {
            var eb = _sp.GetService<IPayrollElementBusiness>();
            viewModel.EmployeeList = await GetSelectedPersonsForPayrollRun(viewModel);
            viewModel.ElementList = await eb.GetElementListForPayrollRun(viewModel.PayrollEndDate);
        }

        private async Task<List<ElementViewModel>> GetElementListForPayrollRun(DateTime payrollEndDate)
        {
            var cypher = $@"Select * from cms.""N_PayrollHR_PayrollElement"" where e.""EffectiveStartDate""::Date<='{DateTime.Now}'<=e.""EffectiveEndDate""::Date and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            //var cypher = @"match(er:PAY_ElementRoot)
            //         optional match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: {CompanyId} })
            //         where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate
            //         return e,er.Id as ElementId";
            return await _payrunrepo.ExecuteQueryList<ElementViewModel>(cypher, null);
        }

        private async Task<List<PayrollPersonViewModel>> GetSelectedPersonsForPayrollRun(PayrollRunViewModel viewModel)
        {
            // var cypher = string.Concat(@"			
            //      match (r:PAY_PayrollRun{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{Id}})
            //match (r)-[:R_PayrollRun_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      <-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where  p.EffectiveStartDate <= {ESD} <= p.EffectiveEndDate  
            //      match (pr)<-[R_User_PersonRoot]-(u:ADM_User{IsDeleted:0})
            //match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where a.EffectiveStartDate <= {ESD} <= a.EffectiveEndDate
            //      match(a)-[:R_Assignment_OrganizationRoot]->(or:HRS_OrganizationRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(or)<-[:R_OrganizationRoot]-(o:HRS_Organization{IsDeleted:0,CompanyId:{CompanyId}}) 
            //      where o.EffectiveStartDate <= {ESD} <= o.EffectiveEndDate
            //      match(pr)<-[:R_SalaryInfoRoot_PersonRoot]-(sir:PAY_SalaryInfoRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where  si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate  
            //      match(si)-[:R_SalaryInfo_PayrollGroup]->(pg:PAY_PayrollGroup{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(si)-[:R_SalaryInfo_PayCalendar]->(pc:PAY_Calendar{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(p)-[:R_Person_Nationality]->(n:HRS_Nationality{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //   match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{ IsDeleted:0,CompanyId:{CompanyId}})
            //match(cr)<-[:R_ContractRoot]-(c:HRS_Contract{ IsDeleted:0,CompanyId:{CompanyId}})
            //      where c.EffectiveStartDate <= {ESD} <= c.EffectiveEndDate 
            //      optional match(si)-[:R_SalaryInfo_BankBranch]->(bb:PAY_BankBranch{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      optional match(bb)-[:R_BankBranch_Bank]->(b:PAY_Bank{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      optional match(c)-[:R_Contract_Sponsor]->(sp:HRS_Sponsor{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      optional match(a)-[:R_Assignment_PositionRoot]->(por:HRS_PositionRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      optional match(por)<-[:R_PositionRoot]-(po:HRS_Position{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where po.EffectiveStartDate <= {ESD}<= po.EffectiveEndDate    
            //      optional match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      optional match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where j.EffectiveStartDate <= {ESD} <= j.EffectiveEndDate              
            //      optional match(a)-[:R_Assignment_GradeRoot]->(gr:HRS_GradeRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      optional match(gr)<-[:R_GradeRoot]-(g:HRS_Grade{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where g.EffectiveStartDate <= {ESD} <= g.EffectiveEndDate 
            //      optional match(a)-[:R_Assignment_Location]->(l:HRS_Location{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      return p,pr.Id as PersonId,ar.Id as AssignmentRootId,a.Id as AssignmentId,a.EffectiveStartDate as AssignmentStartDate
            //      ,a.EffectiveEndDate as AssignmentEndDate,si.Status as SalaryInfoStatus
            //      ,o.Id as OrganizationId,o.Name as OrganizationName,sp.Id as SponsorId
            //      ,po.Id as PositionId,po.Name as PositionName
            //      ,j.Id as JobId,j.Name as JobName,b.Name as BankName,bb.Name as BankBranchName,bb.Code as BankCode
            //      ,g.Id as GradeId,g.Name as GradeName
            //      ,l.Id as LocationId,l.Name as LocationName,u.Id as UserId
            //      ,cr.Id as ContractRootId,c.Id as ContractId,c.EffectiveStartDate as ContractStartDate,c.EffectiveEndDate as ContractEndDate
            //      ,sir.Id as SalaryInfoRootId,si.Id as SalaryInfoId,si.EffectiveStartDate as SalaryInfoStartDate,si.EffectiveEndDate as SalaryInfoEndDate
            //      ,si.BankAccountNo as BankAccountNo,si.BankIBanNo as BankIBanNo,si.PaymentMode as PaymentMode,a.DateOfJoin as DateOfJoin
            //      ,si.TakeAttendanceFromTAA as TakeAttendanceFromTAA,si.IsEligibleForOT as IsEligibleForOT,si.OTPaymentType as OTPaymentType
            //      ,pg.Id as PayrollGroupId,pc.Id as PayrollCalendarId
            //      ,n.Id as NationalityId,true as IsPayrollActive,",
            // Helper.PersonDisplayNameWithSponsorshipNo("p", " as Name"));

            var cypher = string.Concat($@" Select p.*,p.""Id"" as PersonId,a.""Id"" as AssignmentId,a.""EffectiveStartDate"" as AssignmentStartDate
            , a.""EffectiveEndDate"" as AssignmentEndDate, si.""Status"" as SalaryInfoStatus
            , d.""Id"" as OrganizationId, d.""DepartmentName"" as OrganizationName, sp.""Id"" as SponsorId
            , po.""Id"" as PositionId, po.""PositionName"" as PositionName
            , j.""Id"" as JobId, j.""JobTitle"" as JobName, b.""BankName"" as BankName, bb.""BranchName"" as BankBranchName, bb.""BranchCode"" as BankCode
            , g.""Id"" as GradeId, g.""GradeName"" as GradeName
            , l.""Id"" as LocationId, l.""LocationName"" as LocationName, u.""Id"" as UserId
            , c.""Id"" as ContractId, c.""EffectiveStartDate"" as ContractStartDate, c.""EffectiveEndDate"" as ContractEndDate
            , si.""Id"" as SalaryInfoId
            , si.""BankAccountNumber"" as BankAccountNo, si.""BankIBanNumber"" as BankIBanNo, si.""PaymentMode"" as PaymentMode, a.""DateOfJoin"" as DateOfJoin
            , si.""UseTimeAndAttendanceModule"" as TakeAttendanceFromTAA, si.""IsEmployeeEligibleForOvertime"" as IsEligibleForOT, si.""OvertimePaymentType"" as OTPaymentType
            , pg.""Id"" as PayrollGroupId, pc.""Id"" as PayrollCalendarId
            , n.""Id"" as NationalityId, true as IsPayrollActive, p.""PersonFullName"" as Name
            from cms.""N_PayrollHR_PayrollRun"" as r
            join cms.""N_PayrollHR_PayrollRunPerson"" as pr on pr.""PayrollRunId"" = r.""Id"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRPerson"" as p on p.""Id"" = pr.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id""  and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
 
            join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false  and d.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and a.""EffectiveStartDate"" <= '{viewModel.PayrollEndDate}' and '{viewModel.PayrollEndDate}' <= a.""EffectiveEndDate""
            --join public.""LegalEntityId"" as o on o.""Id""=p.""PersonLegalEntityId""    
            join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=p.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'

            join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=si.""PayGroupId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_PayrollHR_PayrollCalendar"" as pc on si.""PayCalendarId""=pc.""Id"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRNationality"" as n on p.""NationalityId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and c.""EffectiveStartDate"" <= '{viewModel.PayrollEndDate}' and '{viewModel.PayrollEndDate}' <= c.""EffectiveEndDate"" 
            left join cms.""N_CoreHR_HRPosition"" as po on po.""Id""=a.""PositionId"" and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and po.""EffectiveStartDate"" <= '{viewModel.PayrollEndDate}' and '{viewModel.PayrollEndDate}' <= po.""EffectiveEndDate""    
            left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and j.""EffectiveStartDate"" <= '{viewModel.PayrollEndDate}' and '{viewModel.PayrollEndDate}' <= j.""EffectiveEndDate""   
            left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false and g.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and g.""EffectiveStartDate"" <= '{viewModel.PayrollEndDate}' and '{viewModel.PayrollEndDate}' <= g.""EffectiveEndDate""   
            left join cms.""N_CoreHR_HRLocation"" as l on l.""Id""=a.""LocationId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and l.""EffectiveStartDate"" <= '{viewModel.PayrollEndDate}' and '{viewModel.PayrollEndDate}' <= l.""EffectiveEndDate""   
            left join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join cms.""N_PayrollHR_BankBranch"" as bb on bb.""Id""=si.""BankBranchId"" and bb.""IsDeleted""=false and bb.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join cms.""N_PayrollHR_PayrollBank"" as b on bb.""BankId""=b.""Id"" and  b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'
            where r.""Id""='{viewModel.Id}' and r.""IsDeleted""=false and r.""CompanyId""='{_repo.UserContext.CompanyId}'");
            var list = await _payrunrepo.ExecuteQueryList<PayrollPersonViewModel>(cypher, null);
            return list;
        }

        private async Task RunPayroll(PayrollRunViewModel viewModel)
        {
            await PreExecutePayroll(viewModel);
            await ChangeDraftedTransactionsToNotProcessed(viewModel);
            await LoadDataForExecutePayroll(viewModel);
            await GenerateRunResult(viewModel);
            await ExecuteUnProcessedTransactions(viewModel);
            await SummarizeExecutePayrollEntries(viewModel);
            await UpdateExecutePayroll(viewModel);
        }
        private async Task LoadDataForExecutePayroll(PayrollRunViewModel viewModel)
        {
            viewModel.EmployeeSalaryElementInfoList = await _sp.GetService<IPayrollElementBusiness>().GetSalaryElementInfoForPayrollRun(viewModel);
            viewModel.UnProcessedTransactionList = await _sp.GetService<IPayrollTransactionsBusiness>().GetAllUnProcessedTransactionsForPayrollRun(viewModel);
            viewModel.EmployeePayrollRunResult = new List<PayrollRunResultViewModel>();
            viewModel.EmployeePayrollElementRunResult = new List<PayrollElementRunResultViewModel>();
            viewModel.EmployeePayrollElementDailyRunResult = new List<PayrollElementDailyRunResultViewModel>();
        }
        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoForPayrollRun(PayrollRunViewModel viewModel)
        {

            var cypher = string.Concat($@"select sei.*,pr.""Id"" as PersonId,e.""Id"" as ElementId,e"".ElementType"" as ElementType,e.""ElementEntryType"" as ElementEntryType
                 ,e.""ElementCategory"" as ElementCategory,e.""ElementClassification"" as ElementClassification
from cms.""N_PayrollHR_PayrollRun"" as r 
 join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PayrollRunId""=r.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=prp.""PersonId"" and pr.""IsDeleted""=false  and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as pn on pn.""Id""=si.""NoteId""  and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on pn.""Id""=n.""ParentNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
and sei.""EffectiveStartDate""::Date <= '{DateTime.Now}'::Date <= sei.""EffectiveEndDate""::Date
join cms.""N_PayrollHR_PayrollElement"" as e on sei.""ElementId""=e.""Id"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
--and e.""EffectiveStartDate""::Date <= '{DateTime.Now}'::Date <= e.""EffectiveEndDate""::Date
where r.""Id""='{viewModel.Id} and r.""IsDeleted""=false and r.""CompanyId""='{_repo.UserContext.CompanyId}'
");

            // var cypher = string.Concat(@"match (r:PAY_PayrollRun{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{Id}})
            //match (r)-[:R_PayrollRun_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})		    
            //      match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where  p.EffectiveStartDate <= {ESD} <= p.EffectiveEndDate  
            //match(pr)<-[:R_SalaryInfoRoot_PersonRoot]-(sir:PAY_SalaryInfoRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where  si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate  
            //      match(sir)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(sei:PAY_SalaryElementInfo{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where  sei.EffectiveStartDate <= {ESD} <= sei.EffectiveEndDate   
            //      match(sei)-[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      where  e.EffectiveStartDate <= {ESD} <= e.EffectiveEndDate   
            //      return sei,pr.Id as PersonId,er.Id as ElementId,e.ElementType as ElementType,e.ElementEntryType as ElementEntryType
            //      ,e.ElementCategory as ElementCategory,e.ElementClassification as ElementClassification");


            var list = await _payrunrepo.ExecuteQueryList<SalaryElementInfoViewModel>(cypher, null);
            return list;
        }
        private async Task PreExecutePayroll(PayrollRunViewModel viewModel)
        {

            await DetachAllTransactionsFromPayrollRun(viewModel);
            await DeletePayrollRunData(viewModel);
            await DeleteSalaryData(viewModel);
            await DeleteBankLetterData(viewModel);
            var transactionBusiness = _sp.GetService<IPayrollTransactionsBusiness>();
            await transactionBusiness.GeneratePayrollSalaryTransactions(viewModel);
            await LoadLeaveTransactions(viewModel);
        }

        public async Task LoadLeaveTransactions(PayrollRunViewModel viewModel)
        {
            var output = "success";
            var lbb = _sp.GetService<ILeaveBalanceSheetBusiness>();
            var attendanceOtModel = await lbb.GetAllLeavesWithDuration(viewModel.AttendanceStartDate, viewModel.AttendanceEndDate);
            var startDate = viewModel.AttendanceStartDate;
            var transactionList = new List<PayrollTransactionViewModel>();
            if (attendanceOtModel.Count > 0)
            {

                while (startDate <= viewModel.AttendanceEndDate)
                {
                    var items = attendanceOtModel.Where(x => x.StartDate <= startDate && x.EndDate >= startDate).ToList();
                    foreach (var item in items)
                    {
                        var isDayOff = await lbb.IsDayOff(item.PersonId, startDate);
                        if (!isDayOff)
                        {
                            switch (item.LeaveTypeCode)
                            {

                                case "ANNUAL_LEAVE":
                                case "ANNUAL_LEAVE_ADV":
                                    await PostAnnualLeaveTransaction(item, transactionList, startDate, viewModel);
                                    break;
                                case "ANNUAL_LEAVE_HD":
                                    await PostHalfDayAnnualLeaveTransaction(item, transactionList, startDate, viewModel);
                                    break;
                                case "SICK_L_K":
                                case "SICK_LEAVE":
                                    await PostSickLeaveTransaction(item, transactionList, startDate);
                                    break;
                                case "UNPAID_L":
                                case "AUTH_LEAVE_WITHOUT_PAY":
                                case "PLANNED_UNPAID_L":
                                    await PostUnpaidLeaveTransaction(item, transactionList, startDate, viewModel);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    startDate = startDate.AddDays(1);
                }

            }
            var leaveEncashments = await lbb.GetAllLeaveEncashmentDuration(viewModel.AttendanceStartDate, viewModel.AttendanceEndDate);
            foreach (var item in leaveEncashments)
            {
                await PostLeaveEncashmentTransaction(item, transactionList, startDate, viewModel);
            }
            var ptb = _sp.GetService<IPayrollTransactionsBusiness>();
            if (transactionList.Count > 0)
            {
                UpdateElementDetail(transactionList);
                var bulkInsertList = transactionList.Where(x => x.DataAction == DataActionEnum.Create).ToList();
                await ptb.BulkInsert(bulkInsertList, false);
                var bulkUpdateList = transactionList.Where(x => x.DataAction == DataActionEnum.Edit).ToList();
                await ptb.BulkUpdate(bulkUpdateList, false);
            }
            var ab = _sp.GetService<IAttendanceBusiness>();
            await ab.UpdateOTPayTransToProcessed(viewModel.PayrollEndDate, viewModel.Id);

        }
        private async Task PostLeaveEncashmentTransaction(LeaveDetailViewModel model, List<PayrollTransactionViewModel> transactionList, DateTime date, PayrollRunViewModel viewModel)
        {
            var peb = _sp.GetService<IPayrollElementBusiness>();
            var amount = await peb.GetUserOneDaySalary(model.UserId, viewModel.PayrollEndDate) * (model.Adjustment ?? 0);
            await PostPayrollTransaction(model.PersonId, "LEAVE_ENCASHMENT", date, amount, transactionList);
        }
        private async Task PostUnpaidLeaveTransaction(LeaveDetailViewModel model, List<PayrollTransactionViewModel> transactionList, DateTime date, PayrollRunViewModel viewModel)
        {
            var peb = _sp.GetService<IPayrollElementBusiness>();
            var amount = await peb.GetUserOneDaySalary(model.UserId, viewModel.PayrollEndDate);
            await PostPayrollTransaction(model.PersonId, "UNPAID_LEAVE", date, amount, transactionList);
        }
        private async Task PostSickLeaveTransaction(LeaveDetailViewModel model, List<PayrollTransactionViewModel> transactionList, DateTime date)
        {
            //return;
            var leaveduration = 1;
            var personBusiness = _sp.GetService<IHRCoreBusiness>();
            //TODO
            //var calendarBusiness = BusinessHelper.GetInstance<ICalendarBusiness>();
            //var payTransBusiness = BusinessHelper.GetInstance<IPayrollTransactionBusiness>();
            //var salaryElementInfoBusiness = BusinessHelper.GetInstance<ISalaryElementInfoBusiness>();
            //var serviceRepo = BusinessHelper.GetInstance<IServiceBusiness>();
            //var assignmentBusiness = BusinessHelper.GetInstance<IAssignmentBusiness>();
            var person = new PersonProfileViewModel();// personBusiness.GetPersonByUser(model.UserId);
            // var assignment = assignmentBusiness.GetActiveAssignmentByUser(model.UserId);
            var doj = DateTime.Now;// assignment.DateOfJoin.Value;
            var yearDiff = date.Year - doj.Year;
            var lastAnniversaryDate = doj.AddYears(yearDiff);
            var nextAnniversaryDate = lastAnniversaryDate.AddYears(1);
            if (lastAnniversaryDate > date)
            {
                nextAnniversaryDate = lastAnniversaryDate;
                lastAnniversaryDate = lastAnniversaryDate.AddYears(-1);
            }
            if (date >= lastAnniversaryDate)
            {
                var cypher = $@"Select DATE_PART('day', sl.""LeaveEndDate""::timestamp-sl.""LeaveStartDate""::timestamp) as duration
                                from public.""Template"" as t 
join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and t.""Code""='SICK_L_U' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=s.""OwnerUserId""and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=s.""ServiceStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_SickLeave"" as sl on sl.""NtsNoteId""=s.""UdfNoteId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'
and '{lastAnniversaryDate}'<= sl.""LeaveStartDate"" and sl.""LeaveStartDate"" <='{date}' and '{lastAnniversaryDate}'<= sl.""LeaveEndDate"" and sl.""LeaveEndDate"" <='{date}'
where l.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_INPROGRESS')  and u.""Id""='{model.UserId} and t.""IsDeleted""=false' and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";
                //var cypher = @"match (tr:NTS_TemplateMaster{IsDeleted:0,Status: { Status}})  
                //                where tr.Code='SICK_L_U'
                //                match (tr)<-[:R_TemplateRoot]-(t:NTS_Template{IsDeleted:0,Status: { Status}})  
                //                match(t)<-[:R_Service_Template]-(s:NTS_Service{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId}})
                //                match(s)-[:R_Service_Owner_User]->(u: ADM_User{ Id:{UserId},IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
                //                where s.TemplateAction IN ['Complete','Submit']
                //                match (s)<-[:R_ServiceFieldValue_Service]-(nfv1:NTS_ServiceFieldValue{IsDeleted:0})
                //                -[:R_ServiceFieldValue_TemplateField]->(tf1{FieldName:'startDate',IsDeleted:0})
                //                match (s)<-[:R_ServiceFieldValue_Service]-(nfv2:NTS_ServiceFieldValue{IsDeleted:0})
                //                -[:R_ServiceFieldValue_TemplateField]->(tf2{FieldName:'endDate',IsDeleted:0})
                //                where {StartDate}<= nfv1.Code <={EndDate} and {StartDate}<= nfv2.Code <={EndDate}
                //                RETURN  duration.indays(datetime(nfv1.Code),datetime(nfv2.Code)).days as duration
                //                //return nfv1.Code as startDate,nfv2.Code as endDate
                //                ";
                //var prms = new Dictionary<string, object>
                //    {
                //        { "UserId", model.UserId },
                //        { "Status", StatusEnum.Active },
                //        { "CompanyId",1},
                //       { "StartDate",lastAnniversaryDate},
                //       { "EndDate", date}
                //    };
                //                var cypher = $@" Select  duration.indays(datetime(nfv1.Code),datetime(nfv2.Code)).days as duration
                //from public.""Template"" as tr 
                //join public.""NtsService"" as s on s.""TemplateId""=tr.""Id"" and tr.""Code""='SICK_L_U' and tr.""IsDeleted""=false
                //join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 

                //where s.""ServiceStatusCode"" IN ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_INPROGRESS') and u.""Id""='{model.UserId}'
                //                                ";

                var sickLeavesList = await _payrunrepo.ExecuteScalarList<double>(cypher, null);
                var existingSickLeaves = 0.0;
                foreach (var item in sickLeavesList)
                {
                    if (item.IsNotNull())
                    {
                        existingSickLeaves += item + 1;
                        //if(lastAnniversaryDate<=item[0].ToSafeDateTime() && leaveStartDate>= item[0].ToSafeDateTime() && lastAnniversaryDate<= item[1].ToSafeDateTime() && leaveStartDate>= item[1].ToSafeDateTime())
                        //{
                        //    existingSickLeaves += 1;
                        //}
                    }
                }
                var seb = _sp.GetService<IPayrollElementBusiness>();
                var ptb = _sp.GetService<IPayrollTransactionsBusiness>();
                var lbb = _sp.GetService<ILeaveBalanceSheetBusiness>();
                var leaveStartDate = date;
                var totalleaves = existingSickLeaves + leaveduration;
                var onedaysalary = await seb.GetUserSalary(model.UserId) / 22;
                if (existingSickLeaves < 15)
                {
                    if (totalleaves > 15)
                    {
                        var noDeductionLeaves = 15 - existingSickLeaves;
                        var halfDeductionLeaves = leaveduration - noDeductionLeaves;
                        if (halfDeductionLeaves > 0 && halfDeductionLeaves < 30)
                        {
                            //half deduction
                            leaveStartDate = leaveStartDate.AddDays(noDeductionLeaves);
                            for (int i = 0; i < halfDeductionLeaves; i++)
                            {
                                if (i != 0)
                                    leaveStartDate = leaveStartDate.AddDays(1);
                                var isDayOff = await lbb.IsDayOff(person.Id, leaveStartDate);
                                if (!isDayOff)
                                {
                                    var existTrans = await ptb.GetPayrollTransactionBasedonElement(model.UserId, leaveStartDate, "SICK_LEAVE_DED");
                                    if (!existTrans.Any())
                                    {
                                        await PostPayrollTransaction(model.PersonId, "SICK_LEAVE_DED", date, (onedaysalary / 2.0).RoundToTwoDecimal(), transactionList);
                                    }
                                }


                            }
                        }
                        else
                        {
                            var deductionLeaves = 30;
                            //half deduction for 30days
                            leaveStartDate = leaveStartDate.AddDays(noDeductionLeaves);
                            for (int i = 0; i < deductionLeaves; i++)
                            {
                                if (i != 0)
                                    leaveStartDate = leaveStartDate.AddDays(1);
                                var isDayOff = await lbb.IsDayOff(person.Id, leaveStartDate);
                                if (!isDayOff)
                                {
                                    var existTrans = await ptb.GetPayrollTransactionBasedonElement(model.UserId, leaveStartDate, "SICK_LEAVE_DED");
                                    if (!existTrans.Any())
                                    {
                                        await PostPayrollTransaction(model.PersonId, "SICK_LEAVE_DED", date, (onedaysalary / 2.0).RoundToTwoDecimal(), transactionList);
                                    }
                                }
                            }
                            var fullDeductionLeaves = halfDeductionLeaves - deductionLeaves;
                            if (fullDeductionLeaves > 0)
                            {
                                //full dedcution
                                leaveStartDate = leaveStartDate.AddDays(deductionLeaves);
                                for (int i = 0; i < fullDeductionLeaves; i++)
                                {
                                    if (i != 0)
                                        leaveStartDate = leaveStartDate.AddDays(1);
                                    var isDayOff = await lbb.IsDayOff(person.Id, leaveStartDate);
                                    if (!isDayOff)
                                    {
                                        var existTrans = await ptb.GetPayrollTransactionBasedonElement(model.UserId, leaveStartDate, "SICK_LEAVE_DED");
                                        if (!existTrans.Any())
                                        {
                                            await PostPayrollTransaction(model.PersonId, "SICK_LEAVE_DED", date, onedaysalary.RoundToTwoDecimal(), transactionList);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                else if (existingSickLeaves >= 15 && existingSickLeaves < 45)
                {
                    if (totalleaves > 45)
                    {
                        var halfDeductionLeaves = 45 - existingSickLeaves;
                        //half dedcution                         
                        for (int i = 0; i < halfDeductionLeaves; i++)
                        {
                            if (i != 0)
                                leaveStartDate = leaveStartDate.AddDays(1);
                            var isDayOff = await lbb.IsDayOff(person.Id, leaveStartDate);
                            if (!isDayOff)
                            {
                                var existTrans = await ptb.GetPayrollTransactionBasedonElement(model.UserId, leaveStartDate, "SICK_LEAVE_DED");
                                if (!existTrans.Any())
                                {
                                    await PostPayrollTransaction(model.PersonId, "SICK_LEAVE_DED", date, (onedaysalary / 2.0).RoundToTwoDecimal(), transactionList);
                                }
                            }
                        }
                        var fullDeductionLeaves = leaveduration - halfDeductionLeaves;
                        if (fullDeductionLeaves > 0)
                        {
                            //full deduction 
                            leaveStartDate = leaveStartDate.AddDays(halfDeductionLeaves);
                            for (int i = 0; i < fullDeductionLeaves; i++)
                            {
                                if (i != 0)
                                    leaveStartDate = leaveStartDate.AddDays(1);
                                var isDayOff = await lbb.IsDayOff(person.Id, leaveStartDate);
                                if (!isDayOff)
                                {
                                    var existTrans = await ptb.GetPayrollTransactionBasedonElement(model.UserId, leaveStartDate, "SICK_LEAVE_DED");
                                    if (!existTrans.Any())
                                    {
                                        await PostPayrollTransaction(model.PersonId, "SICK_LEAVE_DED", date, (onedaysalary / 2.0).RoundToTwoDecimal(), transactionList);

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //half deduction
                        for (int i = 0; i < leaveduration; i++)
                        {
                            if (i != 0)
                                leaveStartDate = leaveStartDate.AddDays(1);
                            var isDayOff = await lbb.IsDayOff(person.Id, leaveStartDate);
                            if (!isDayOff)
                            {
                                var existTrans = await ptb.GetPayrollTransactionBasedonElement(model.UserId, leaveStartDate, "SICK_LEAVE_DED");
                                if (!existTrans.Any())
                                {
                                    await PostPayrollTransaction(model.PersonId, "SICK_LEAVE_DED", date, (onedaysalary / 2.0).RoundToTwoDecimal(), transactionList);

                                }
                            }
                        }
                    }
                }
                else if (existingSickLeaves >= 45)
                {
                    //full deduction
                    for (int i = 0; i < leaveduration; i++)
                    {
                        if (i != 0)
                            leaveStartDate = leaveStartDate.AddDays(1);
                        var isDayOff = await lbb.IsDayOff(person.Id, leaveStartDate);
                        if (!isDayOff)
                        {
                            var existTrans = await ptb.GetPayrollTransactionBasedonElement(model.UserId, leaveStartDate, "SICK_LEAVE_DED");
                            if (!existTrans.Any())
                            {
                                await PostPayrollTransaction(model.PersonId, "SICK_LEAVE_DED", date, (onedaysalary).RoundToTwoDecimal(), transactionList);

                            }
                        }
                    }
                }
            }


        }

        private async Task PostHalfDayAnnualLeaveTransaction(LeaveDetailViewModel model, List<PayrollTransactionViewModel> transactionList, DateTime date, PayrollRunViewModel viewModel)
        {
            var peb = _sp.GetService<IPayrollElementBusiness>();
            var amount = await peb.GetUserOneDaySalary(model.UserId, viewModel.PayrollEndDate) / 2.0;
            await PostPayrollTransaction(model.PersonId, "VACATION_ENCASHMENT", date, amount, transactionList);
            await PostPayrollTransaction(model.PersonId, "VACATION_ENCASHMENT_REV", date, amount, transactionList);
        }
        private async Task PostAnnualLeaveTransaction(LeaveDetailViewModel model, List<PayrollTransactionViewModel> transactionList, DateTime date, PayrollRunViewModel viewModel)
        {
            var peb = _sp.GetService<IPayrollElementBusiness>();
            var amount = await peb.GetUserOneDaySalary(model.UserId, viewModel.PayrollEndDate);
            await PostPayrollTransaction(model.PersonId, "VACATION_ENCASHMENT", date, amount, transactionList);
            await PostPayrollTransaction(model.PersonId, "VACATION_ENCASHMENT_REV", date, amount, transactionList);
        }
        private async Task PostPayrollTransaction(string personId, string elementCode, DateTime effectiveDate, double amount, List<PayrollTransactionViewModel> transactionList)
        {
            var ptb = _sp.GetService<IPayrollTransactionsBusiness>();
            var transaction = await ptb.IsTransactionExists(personId, elementCode, effectiveDate);
            if (transaction == null)
            {
                var payModel = await GetTransationVewModel(amount, effectiveDate, elementCode, personId);
                payModel.DataAction = DataActionEnum.Create;// DataOperation.Create;
                transactionList.Add(payModel);
            }
            else if (transaction.ProcessStatusCode == "PROCESS_STATUS_DRAFT")
            {
                transaction.ElementCode = elementCode;
                transaction.Amount = amount;
                transaction.PostedDate = DateTime.Today;
                transaction.DataAction = DataActionEnum.Edit;// DataOperation.Correct;
                transactionList.Add(transaction);
            }


        }
        private async Task<PayrollTransactionViewModel> GetTransationVewModel(double amount, DateTime effectiveDate, string elementCode, string personId, string attendanceId = null)
        {
            var model = new PayrollTransactionViewModel
            {
                Amount = amount.RoundToTwoDecimal(),
                EffectiveDate = effectiveDate,
                ProcessStatusCode = "PROCESS_STATUS_NOT_PROCESSED",// PayrollProcessStatusEnum.NotProcessed,
                PostedSource = PayrollPostedSourceEnum.Payroll,
                PostedDate = effectiveDate,
                ElementCode = elementCode,
                PersonId = personId,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                CreatedBy = _repo.UserContext.UserId,
                LastUpdatedBy = _repo.UserContext.UserId,

            };
            if (attendanceId.IsNotNullAndNotEmpty())
            {
                model.ReferenceNode = NodeEnum.TAA_Attendance;
                model.ReferenceId = attendanceId;
            }
            return model;
        }

        private async Task GenerateSalary(PayrollRunViewModel viewModel)
        {
            await PreExecuteGenerateSalary(viewModel);
            await LoadDataForGenerateSalary(viewModel);
            await ProcessSalaryEntry(viewModel);
            await ProcessSalaryElementEntry(viewModel);
            await SummarizeSalaryEntries(viewModel);
            await UpdateGeneratePayslip(viewModel);
        }
        private async Task UpdateGeneratePayslip(PayrollRunViewModel viewModel)
        {
            var seb = _sp.GetService<ISalaryEntryBusiness>();
            await seb.BulkInsertForPayroll(viewModel.EmployeeSalaryEntryList, true);
            await seb.BulkInsertIntoSalaryElementEntry(viewModel.EmployeeSalaryElementEntryList, false);
        }

        private async Task SummarizeSalaryEntries(PayrollRunViewModel viewModel)
        {
            foreach (var salaryEntry in viewModel.EmployeeSalaryEntryList)
            {
                var salaryElementEntries = viewModel.EmployeeSalaryElementEntryList.Where(x => x.PersonId == salaryEntry.PersonId).ToList();
                if (salaryElementEntries.Count > 0)
                {
                    salaryEntry.TotalEarning = salaryElementEntries.Sum(x => x.EarningAmount.Value).RoundToTwoDecimal();
                    salaryEntry.TotalDeduction = salaryElementEntries.Sum(x => x.DeductionAmount.Value).RoundToTwoDecimal();
                    salaryEntry.NetAmount = (salaryEntry.TotalEarning - salaryEntry.TotalDeduction).RoundPayrollSummaryAmount();
                }
            }
        }
        private async Task ProcessSalaryElementEntry(PayrollRunViewModel viewModel)
        {
            var count = viewModel.EmployeePayrollElementRunResult.Count;
            if (count <= 0)
            {
                return;
            }
            var salaryEntries = viewModel.EmployeePayrollElementRunResult.Where(x => x.ElementType == ElementTypeEnum.Cash).ToList();
            foreach (var runResult in salaryEntries)
            {
                var isGosi = await IsGosiEmployerContribution(viewModel, runResult.ElementId);
                if (isGosi)
                {
                    continue;
                }
                var existingEntry = viewModel.EmployeeSalaryElementEntryList
                        .FirstOrDefault(x => x.PersonId == runResult.PersonId
                        && x.ElementId == runResult.ElementId
                        && x.YearMonth == runResult.YearMonth);
                if (existingEntry != null)
                {
                    existingEntry.EarningAmount += runResult.EarningAmount;
                    existingEntry.DeductionAmount += runResult.DeductionAmount;
                    existingEntry.Amount += runResult.Amount;
                }
                else
                {
                    var see = _autoMapper.Map<PayrollElementRunResultViewModel, SalaryElementEntryViewModel>(runResult);
                    var salaryEntry = viewModel.EmployeeSalaryEntryList.FirstOrDefault(x => x.PersonId == runResult.PersonId);
                    see.Id = null;
                    see.PayrollId = viewModel.PayrollId;
                    see.PayrollRunId = viewModel.Id;
                    see.Status = StatusEnum.Active;
                    see.CompanyId = _repo.UserContext.CompanyId;
                    see.SalaryEntryId = salaryEntry.Id;
                    see.CreatedDate = DateTime.Now;
                    see.CreatedBy = viewModel.CreatedBy;
                    see.LastUpdatedDate = DateTime.Now;
                    see.LastUpdatedBy = viewModel.CreatedBy;
                    see.DataAction = DataActionEnum.Create;
                    see.ExecutionStatus = ExecutionStatusEnum.Success;
                    see.PublishStatus = DocumentStatusEnum.Draft;
                    viewModel.EmployeeSalaryElementEntryList.Add(see);

                }

            }
        }
        private async Task<bool> IsGosiEmployerContribution(PayrollRunViewModel viewModel, string elementId)
        {
            var element = viewModel.ElementList.FirstOrDefault(x => x.ElementId == elementId);
            if (element != null)
            {
                return element.Code == "GOSI_COMP_KSA" || element.Code == "GOSI_COMP_KSA_REV"
                    || element.Code == "GOSI_COMP_NON_KSA" || element.Code == "GOSI_COMP_NON_KSA_REV";
            }
            return false;
        }
        private async Task ProcessSalaryEntry(PayrollRunViewModel viewModel)
        {
            var seb = _sp.GetService<ISalaryEntryBusiness>();
            var count = viewModel.EmployeePayrollRunResult.Count;
            if (count <= 0)
            {
                return;
            }
            //var salaryEntryIds = await seb.GetNextIdList(count);
            //long salaryEntryId = salaryEntryIds.NextItem();
            //long maxSalaryEntryId = salaryEntryIds.Max();
            foreach (var runResult in viewModel.EmployeePayrollRunResult)
            {
                var salaryEntry = _autoMapper.Map<PayrollRunResultViewModel, SalaryEntryViewModel>(runResult);
                salaryEntry.Id = Guid.NewGuid().ToString();
                salaryEntry.DataAction = DataActionEnum.Create;
                salaryEntry.PayrollId = viewModel.PayrollId;
                salaryEntry.PayrollRunId = viewModel.Id;
                salaryEntry.SalaryName = GetSalaryName(viewModel);
                salaryEntry.Status = StatusEnum.Active;
                salaryEntry.CompanyId = _repo.UserContext.CompanyId;
                salaryEntry.CreatedDate = DateTime.Now;
                salaryEntry.CreatedBy = viewModel.CreatedBy;
                salaryEntry.LastUpdatedDate = DateTime.Now;
                salaryEntry.LastUpdatedBy = viewModel.CreatedBy;
                salaryEntry.DataAction = DataActionEnum.Create;
                salaryEntry.ExecutionStatus = ExecutionStatusEnum.Success;
                salaryEntry.PublishStatus = DocumentStatusEnum.Draft;

                var employee = viewModel.EmployeeList.FirstOrDefault(x => x.PersonId == runResult.PersonId);
                if (employee != null)
                {
                    salaryEntry.JobId = employee.JobId;
                    salaryEntry.JobName = employee.JobName;
                    salaryEntry.OrganizationId = employee.OrganizationId;
                    salaryEntry.OrganizationName = employee.OrganizationName;
                    salaryEntry.PositionId = employee.PositionId;
                    salaryEntry.PositionName = employee.PositionName;
                    salaryEntry.GradeId = employee.GradeId;
                    salaryEntry.GradeName = employee.GradeName;
                    salaryEntry.LocationId = employee.LocationId;
                    salaryEntry.LocationName = employee.LocationName;

                }
                viewModel.EmployeeSalaryEntryList.Add(salaryEntry);
            }
        }

        private async Task PreExecuteGenerateSalary(PayrollRunViewModel viewModel)
        {
            await DeleteSalaryData(viewModel);
            await DeleteBankLetterData(viewModel);
        }
        private async Task LoadDataForGenerateSalary(PayrollRunViewModel viewModel)
        {
            var rrb = _sp.GetService<IPayrollRunResultBusiness>();
            viewModel.EmployeePayrollRunResult = await rrb.GetSuccessfulPayrollRunResult(viewModel);
            viewModel.EmployeePayrollElementRunResult = await rrb.GetSuccessfulElementRunResult(viewModel);
            viewModel.EmployeeSalaryEntryList = new List<SalaryEntryViewModel>();
            viewModel.EmployeeSalaryElementEntryList = new List<SalaryElementEntryViewModel>();
        }

        private async Task PrepareBankLetter(PayrollRunViewModel viewModel)
        {
            var seb = _sp.GetService<ISalaryEntryBusiness>();
            await PreExecutePrepareBankLetter(viewModel);
            viewModel.EmployeeSalaryEntryList = await seb.GetSuccessfulSalaryEntryList(viewModel);
            viewModel.BankLetterDetails = new List<BankLetterDetailViewModel>();
            await ProcessBankLetterDetail(viewModel);
        }
        private async Task ProcessBankLetterDetail(PayrollRunViewModel viewModel)
        {
            //throw new NotImplementedException();
            //var companySetup = _companySetupBusiness.ViewModelList("or.Id={OrganizationId}",
            //    new Dictionary<string, object> { { "ESD", DateTime.Now.Date }, { "OrganizationId", viewModel.OrganizationId } })
            //    .FirstOrDefault();

            var bankLetter = new BankLetterViewModel
            {
                Id = Guid.NewGuid().ToString(),
                DataAction = DataActionEnum.Create,
                CompanyId = _repo.UserContext.CompanyId,
                IsDeleted = false,
                Status = StatusEnum.Active,
                ExecutionStatus = ExecutionStatusEnum.Success,
                PayrollId = viewModel.PayrollId,
                PayrollRunId = viewModel.Id,
                PayrollStartDate = viewModel.PayrollStartDate,
                PayrollEndDate = viewModel.PayrollEndDate,
            };
            if (/*companySetup == null*/ false)
            {
                bankLetter.ExecutionStatus = ExecutionStatusEnum.Error;
                bankLetter.Error = "Company setup is not available";
                //_bankLetterBusiness.Create(bankLetter);
                await CreateBankLetter(bankLetter);
            }
            else
            {
                bankLetter.BankName = "Synergy Bank"; //companySetup.BankName;
                bankLetter.BankAccountNo = "9811223344556677"; //companySetup.BankAccountNo;
                bankLetter.BankSwiftCode = "SWIFT1111"; //companySetup.BankSwiftCode;
                bankLetter.BranchName = "Synergy";// companySetup.BankBranchName;
                //bankLetter.OrganizationId = companySetup.OrganizationId;
                bankLetter.PayrollStartDate = viewModel.PayrollStartDate;
                bankLetter.PayrollEndDate = viewModel.PayrollEndDate;
                bankLetter.YearMonth = viewModel.YearMonth;
                var salaryEntriesForBankTransfer = viewModel.EmployeeSalaryEntryList
                .Where(x => x.ExecutionStatus == ExecutionStatusEnum.Success
                && x.PaymentMode == PaymentModeEnum.BankTransfer).ToList();
                foreach (var item in salaryEntriesForBankTransfer)
                {
                    viewModel.BankLetterDetails.Add(new BankLetterDetailViewModel
                    {
                        BankLetterId = bankLetter.Id,
                        BankAccountNo = item.BankAccountNo,
                        BankName = item.BankName,
                        BankIBanNo = item.BankIBanNo,
                        PersonName = item.PersonFullName,
                        SponsorshipNo = item.SponsorshipNo,
                        NetAmount = item.NetAmount,
                        DataAction = DataActionEnum.Create,
                        CreatedBy = viewModel.CreatedBy,
                        CreatedDate = viewModel.CreatedDate,
                        LastUpdatedBy = viewModel.LastUpdatedBy,
                        LastUpdatedDate = viewModel.LastUpdatedDate,
                        IsDeleted = false,
                        Status = StatusEnum.Active,
                        CompanyId = _repo.UserContext.CompanyId,
                        SalaryEntryId = item.Id,
                        PayrollId = viewModel.PayrollId,
                        PayrollRunId = viewModel.Id

                    });
                }
                bankLetter.NetAmount = viewModel.BankLetterDetails.Sum(x => x.NetAmount).RoundPayrollSummaryAmount();
                //_bankLetterBusiness.Create(bankLetter);
                //_bankLetterDetailBusiness.BulkInsertForPayroll(viewModel.BankLetterDetails, false);
                await CreateBankLetter(bankLetter);
                await BankLetterDetailBulkInsertForPayroll(viewModel.BankLetterDetails, false);
            }

        }
        private async Task CreateBankLetter(BankLetterViewModel viewModel)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = viewModel.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "BankLetter";
            var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

            noteModel.Json = JsonConvert.SerializeObject(viewModel);
            noteModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
            var result = await _noteBusiness.ManageNote(noteModel);
        }
        private async Task<List<BankLetterDetailViewModel>> BankLetterDetailBulkInsertForPayroll(List<BankLetterDetailViewModel> viewModelList, bool idGenerated = true, bool doCommit = true)
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
                noteTempModel.TemplateCode = "BankLetterDetail";
                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

                noteModel.Json = JsonConvert.SerializeObject(item);
                noteModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                var result = await _noteBusiness.ManageNote(noteModel);
            }
            return viewModelList;
        }
        private async Task PreExecutePrepareBankLetter(PayrollRunViewModel viewModel)
        {
            await DeleteBankLetterData(viewModel);
        }
        private async Task PublishPayroll(PayrollRunViewModel viewModel)
        {
            //var prms = new Dictionary<string, object>
            //     {
            //         { "PayrollRunId", viewModel.Id }
            //     };
            //var cypher = @"match(n:PAY_SalaryEntry{PayrollRunId:{PayrollRunId}}) set n.PublishStatus='Published'";
            var publish = (int)DocumentStatusEnum.Published;
            var cypher = $@"update cms.""N_PayrollHR_SalaryEntry"" set ""PublishStatus""='{publish}' where ""PayrollRunId""='{viewModel.Id}'";
            await _salaryEntryRepo.ExecuteCommand(cypher, null);
            //cypher = @"match(n:PAY_SalaryElementEntry{PayrollRunId:{PayrollRunId}}) set n.PublishStatus='Published'";
            cypher = $@"update cms.""N_PayrollHR_SalaryElementEntry"" set ""PublishStatus""='{publish}' where ""PayrollRunId""='{viewModel.Id}'";
            await _salaryElementEntryRepo.ExecuteCommand(cypher, null);
        }

        private async Task RollbackPayroll(PayrollRunViewModel viewModel)
        {
            await RollBackTransactions(viewModel);
            await DetachAllTransactionsFromPayrollRun(viewModel);
            await DeletePayrollRunData(viewModel);
            await DeleteSalaryData(viewModel);
            await DeleteBankLetterData(viewModel);
        }
        private async Task RollBackTransactions(PayrollRunViewModel viewModel)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("Status", StatusEnum.Active);
            //prms.AddIfNotExists("CompanyId", CompanyId);
            //prms.AddIfNotExists("PayrollId", viewModel.PayrollId);
            //prms.AddIfNotExists("Id", viewModel.Id);
            //prms.AddIfNotExists("EndDate", viewModel.PayrollEndDate);

            //var cypher = @"match(r:PAY_PayrollRun{Id:{Id}})-[:R_PayrollRun_PersonRoot]->(pr:HRS_PersonRoot)
            //    match (pr)<-[:R_PayrollTransaction_PersonRoot]-(pt:PAY_PayrollTransaction{ IsDeleted: 0,PayrollRunId:{Id}})
            //    set pt.ProcessStatus='NotProcessed',pt.ProcessedDate=null";
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "NOT_PROCESSED");
            var payrollRun = await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>($@"select pr.*,p.""PersonId"" as PayrollPersonId  from cms.""N_PayrollHR_PayrollRun"" as pr  join cms.""N_PayrollHR_PayrollRunPerson"" as p on p.""PayrollRunId""=pr.""Id"" where pr.""Id""='{viewModel.Id}'", null);
            var cypher = $@"update cms.""N_PayrollHR_PayrollTransaction""  set  ""ProcessStatusId""='{lov.Id}',""ProcessedDate""=null
where ""PayrollRunId""='{viewModel.Id}' and ""PersonId""='{payrollRun.PayrollPersonId}'
                ";
            await _payrunrepo.ExecuteCommand(cypher, null);
        }
        private async Task ClosePayroll(PayrollRunViewModel viewModel)
        {
            //var prms = new Dictionary<string, object>
            //     {
            //         { "PayrollId", viewModel.PayrollId },
            //     };
            //var cypher = @"match(n:PAY_Payroll{Id:{PayrollId}}) set n.PayrollStatus='Completed'";
            var payrollStatus = (int)PayrollStatusEnum.Completed;
            var cypher = $@"update cms.""N_PayrollHR_PayrollBatch"" set ""PayrollStatus""='{payrollStatus}' where ""Id""='{viewModel.PayrollId}'";
            await _payrunrepo.ExecuteCommand(cypher, null);
        }
        private async Task DetachAllTransactionsFromPayrollRun(PayrollRunViewModel viewModel)
        {

            //var prms = new Dictionary<string, object>
            //     {
            //         { "PayrollRunId", viewModel.Id },
            //     };
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "DRAFT");
            //var cypher = string.Concat(@"match (pt:PAY_PayrollTransaction{PayrollRunId:{PayrollRunId},PostedSource:'Payroll'}) detach delete pt return 1");
            var cypher = string.Concat($@"update cms.""N_PayrollHR_PayrollTransaction"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}' and ""PostedSource""='3'");
            await _payrunrepo.ExecuteScalar<long>(cypher, null);

            //cypher = string.Concat(@"match (pt:PAY_PayrollTransaction{PayrollRunId:{PayrollRunId}}) set pt.ProcessStatus='Draft', pt.PayrollId=null,pt.PayrollRunId=null");
            cypher = string.Concat($@"update cms.""N_PayrollHR_PayrollTransaction""
                                    set ""ProcessStatusId""='{lov.Id}', ""PayrollBatchId""=null,""PayrollRunId""=null
                                    where ""PayrollRunId""='{ viewModel.Id}'");
            await _payrunrepo.ExecuteCommand(cypher, null);
        }

        private async Task DeletePayrollRunData(PayrollRunViewModel viewModel)
        {
            //var prms = new Dictionary<string, object>
            //     {
            //         { "PayrollRunId", viewModel.Id }
            //     };
            //await _payrunrepo.ExecuteCommand("match(n: PAY_PayrollElementDailyRunResult{ PayrollRunId: { PayrollRunId} }) detach delete n", prms);
            await _payrunrepo.ExecuteCommand($@"update cms.""N_PayrollHR_PayrollElementDailyRunResult"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
            //await _payrunrepo.ExecuteCommand("match(n: PAY_PayrollElementRunResult{ PayrollRunId: { PayrollRunId} }) detach delete n", prms);
            await _payrunrepo.ExecuteCommand($@"update cms.""N_PayrollHR_PayrollElementRunResult"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
            //await _payrunrepo.ExecuteCommand("match(n: PAY_PayrollRunResult{ PayrollRunId: { PayrollRunId} }) detach delete n", prms);
            await _payrunrepo.ExecuteCommand($@"update cms.""N_PayrollHR_PayrollRunResult"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
        }

        private async Task DeleteSalaryData(PayrollRunViewModel viewModel)
        {
            //var prms = new Dictionary<string, object>
            //     {
            //         { "PayrollRunId", viewModel.Id }
            //     };
            //await _payrunrepo.ExecuteCommand("match(n: PAY_SalaryElementEntry{ PayrollRunId: { PayrollRunId} }) detach delete n", prms);
            await _payrunrepo.ExecuteCommand($@"update cms.""N_PayrollHR_SalaryElementEntry"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
            //await _payrunrepo.ExecuteCommand("match(n: PAY_SalaryEntry{ PayrollRunId: { PayrollRunId} }) detach delete n", prms);
            await _payrunrepo.ExecuteCommand($@"update cms.""N_PayrollHR_SalaryEntry"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);

        }
        private async Task DeleteBankLetterData(PayrollRunViewModel viewModel)
        {
            //var prms = new Dictionary<string, object>
            //     {
            //         { "PayrollRunId", viewModel.Id }
            //     };
            //await _payrunrepo.ExecuteCommand("match(n: PAY_BankLetterDetail{ PayrollRunId: { PayrollRunId} }) detach delete n", prms);
            await _payrunrepo.ExecuteCommand($@"update cms.""N_PayrollHR_BankLetterDetail"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
            //await _payrunrepo.ExecuteCommand("match(n: PAY_BankLetter{ PayrollRunId: { PayrollRunId} }) detach delete n", prms);
            await _payrunrepo.ExecuteCommand($@"update cms.""N_PayrollHR_BankLetter"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);

        }

        private async Task ChangeDraftedTransactionsToNotProcessed(PayrollRunViewModel viewModel)
        {

            //var cypher = @"match(r:PAY_PayrollRun{Id:{Id}})-[:R_PayrollRun_PersonRoot]->(pr:HRS_PersonRoot)
            //         match (pr)<-[:R_PayrollTransaction_PersonRoot]-(pt:PAY_PayrollTransaction{ IsDeleted: 0,ProcessStatus:'Draft'})
            //         where  pt.EffectiveDate<={EndDate}
            //         set pt.ProcessStatus='NotProcessed',pt.PayrollId={PayrollId},pt.PayrollRunId=r.Id";
            var dlov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "DRAFT");
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "NOT_PROCESSED");
            //var payrollRun = await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>($@"select * from cms.""N_PayrollHR_PayrollRun"" where ""Id""='{viewModel.Id}'", null);

            var payrollRun = await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>($@"select pr.*, prp.""PersonId"" as PayrollPersonId from cms.""N_PayrollHR_PayrollRun"" as pr
                            left join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PayrollRunId"" = pr.""Id"" and prp.""IsDeleted"" = false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pr.""IsDeleted"" = false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' and pr.""Id"" = '{viewModel.Id}' ", null);

            var cypher = $@"update cms.""N_PayrollHR_PayrollTransaction"" 
                            set ""ProcessStatusId""='{lov.Id}',""PayrollBatchId""='{viewModel.PayrollBatchId}',""PayrollRunId""='{viewModel.Id}'
                            where ""IsDeleted"" = false and ""PersonId""='{payrollRun.PayrollPersonId}' 
                            and ""EffectiveDate""::Date<='{viewModel.PayrollEndDate}'::Date and ""ProcessStatusId""='{dlov.Id}'
                        ";
            await _payrunrepo.ExecuteCommand(cypher, null);
        }

        private async Task GenerateRunResult(PayrollRunViewModel viewModel)
        {
            if (viewModel.EmployeeList.Count <= 0)
            {
                return;
            }
            var runResultBusiness = _sp.GetService<IPayrollRunResultBusiness>();

            foreach (var employee in viewModel.EmployeeList)
            {
                employee.IsPayrollActive = true;
                var model = new PayrollRunResultViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    BankAccountNo = employee.BankAccountNo,
                    BankIBanNo = employee.BankIBanNo,
                    BankName = employee.BankName,
                    BankCode = employee.BankCode,
                    BankBranchName = employee.BankBranchName,
                    PaymentMode = employee.PaymentMode,
                    PayrollInterval = viewModel.PayrollInterval,
                    PublishStatus = DocumentStatusEnum.Draft,
                    Name = GetRunResultName(viewModel, employee),
                    PayrollStartDate = viewModel.PayrollStartDate,
                    PayrollEndDate = viewModel.PayrollEndDate,
                    YearMonth = viewModel.YearMonth,
                    PayrollRunDate = viewModel.PayrollRunDate,
                    PayrollRunId = viewModel.Id,
                    PayrollId = viewModel.PayrollId,
                    DataAction = DataActionEnum.Create,
                    CreatedBy = viewModel.CreatedBy,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = viewModel.CreatedBy,
                    LastUpdatedDate = DateTime.Now,
                    Status = StatusEnum.Active,
                    IsDeleted = false,
                    CompanyId = _repo.UserContext.CompanyId,
                    PayrollGroupId = viewModel.PayrollGroupId,
                    CalendarId = employee.PayrollCalendarId,
                    PayrollCalendarId = employee.PayrollCalendarId,
                    PersonId = employee.PersonId,
                    ExecutionStatus = ExecutionStatusEnum.Success,
                    UserId = employee.UserId
                };
                if (employee.DateOfJoin == null)
                {
                    model.Error = string.Concat(model.Error, "Date of join is not available in Assignment<br/>");
                }
                if (employee.ContractId == null)
                {
                    model.Error = string.Concat(model.Error, "Contract details not available<br/>");
                }
                //if (employee.ContractEndDate == null)
                //{
                //    model.Error = string.Concat(model.Error, "Employee Contract end date is null<br/>");
                //}
                //if (employee.ContractEndDate != null && employee.ContractEndDate < viewModel.PayrollStartDate)
                //{
                //    model.Error = string.Concat(model.Error, "Contract is expired<br/>");
                //}
                if (employee.NationalityId == null)
                {
                    model.Error = string.Concat(model.Error, "Nationality is not updated in Person details<br/>");
                }
                if (employee.SalaryInfoId == null)
                {
                    model.Error = string.Concat(model.Error, "Salary info is not added for the person<br/>");
                }
                if (employee.SponsorId == null)
                {
                    model.Error = string.Concat(model.Error, "Sponsor name is required in Contract<br/>");
                }
                if (employee.SalaryInfoId == null)
                {
                    model.Error = string.Concat(model.Error, "No active salary info for the person<br/>");
                }
                if (employee.SalaryInfoStatus == null || employee.SalaryInfoStatus.Value == StatusEnum.Inactive)
                {
                    model.Error = string.Concat(model.Error, "Person salary information is inactive<br/>");
                }
                if (model.Error.IsNotNullAndNotEmpty())
                {
                    model.ExecutionStatus = ExecutionStatusEnum.Error;
                    employee.IsPayrollActive = false;
                }
                viewModel.EmployeePayrollRunResult.Add(model);

            }
            viewModel.PayrollActiveEmployeeList = viewModel.EmployeeList.Where(x => x.IsPayrollActive).ToList();
        }
        private async Task UpdateExecutePayroll(PayrollRunViewModel viewModel)
        {
            var rrb = _sp.GetService<IPayrollRunResultBusiness>();
            var ptb = _sp.GetService<IPayrollTransactionsBusiness>();
            await rrb.BulkInsertForPayroll(viewModel, true);
            //await rrb.BulkInsertIntoElementRunResult(viewModel.EmployeePayrollElementRunResult, true);
           // await rrb.BulkInsertIntoElementDailyRunResult(viewModel.EmployeePayrollElementDailyRunResult, false);
            //await ptb.BulkUpdateForPayroll(viewModel.UnProcessedTransactionList, viewModel.PayrollId, viewModel.Id);
            await ptb.BulkUpdateForPayroll(viewModel.UnProcessedTransactionList, viewModel.PayrollBatchId, viewModel.Id);
        }

        private async Task SummarizeExecutePayrollEntries(PayrollRunViewModel viewModel)
        {

            foreach (var runResult in viewModel.EmployeePayrollRunResult)
            {
                var earnings = viewModel.EmployeePayrollElementRunResult.Where(x => x.PersonId == runResult.PersonId
                 && x.ElementType == ElementTypeEnum.Cash
                 && x.ElementClassification == ElementClassificationEnum.Earning).ToList();
                runResult.TotalEarning = earnings.Sum(x => x.EarningAmount).RoundToTwoDecimal();

                var deductions = viewModel.EmployeePayrollElementRunResult.Where(x => x.PersonId == runResult.PersonId
                 && x.ElementType == ElementTypeEnum.Cash
                 && x.ElementClassification == ElementClassificationEnum.Deduction).ToList();
                runResult.TotalDeduction = deductions.Sum(x => x.DeductionAmount).RoundToTwoDecimal();

                runResult.NetAmount = (runResult.TotalEarning - runResult.TotalDeduction).RoundPayrollSummaryAmount();
                runResult.PublishStatus = DocumentStatusEnum.Published;

                await ManageSummaryData(runResult, viewModel);
            }


            var payrollEarnings = viewModel.EmployeePayrollElementRunResult.Where(x => x.ElementType == ElementTypeEnum.Cash
           && x.ElementClassification == ElementClassificationEnum.Earning).ToList();
            if (payrollEarnings.Count > 0)
            {
                viewModel.TotalEarning = payrollEarnings.Sum(x => x.EarningAmount).RoundPayrollSummaryAmount(); ;
            }
            var payrollDeductions = viewModel.EmployeePayrollElementRunResult.Where(x => x.ElementType == ElementTypeEnum.Cash
            && x.ElementClassification == ElementClassificationEnum.Deduction).ToList();

            if (payrollDeductions.Count > 0)
            {
                viewModel.TotalDeduction = payrollDeductions.Sum(x => x.DeductionAmount).RoundPayrollSummaryAmount(); ;

            }
            viewModel.NetAmount = viewModel.TotalEarning - viewModel.TotalDeduction;

            viewModel.TotalProcessed = viewModel.EmployeeList.Count();
            viewModel.TotalSucceeded = viewModel.EmployeeList.Where(x => x.IsPayrollActive).Count();
        }

        private async Task ManageSummaryData(PayrollRunResultViewModel runResult, PayrollRunViewModel viewModel)
        {
            var lbb = _sp.GetService<ILeaveBalanceSheetBusiness>();
            var atb = _sp.GetService<IAttendanceBusiness>();
            runResult.AnnualLeaveBalance = await lbb.GetLeaveBalance(viewModel.PayrollEndDate, "ANNUAL_LEAVE", runResult.UserId);
            runResult.ActualWorkingDays = await lbb.GetActualWorkingdays(runResult.PayrollCalendarId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate);
            var allLeaves = await lbb.GetAllLeaveDuration(runResult.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate);
            var annualLeaves = 0.0;
            var sickLeaves = 0.0;
            var otherLeaves = 0.0;
            var unpaidLeaves = 0.0;
            var plannedUnpaidLeaves = 0.0;
            foreach (var item in allLeaves)
            {
                switch (item.LeaveTypeCode)
                {
                    case "AnnualLeave":
                    case "ANNUAL_LEAVE":
                    case "ANNUAL_LEAVE_HD":
                    case "ANNUAL_LEAVE_ADV":
                        annualLeaves += item.DatedDuration ?? 0;
                        break;
                    case "SickLeave":
                    case "SICK_L_K":
                    case "SICK_LEAVE":
                        sickLeaves += item.DatedDuration ?? 0;
                        break;
                    case "UNPAID_L":
                    case "AUTH_LEAVE_WITHOUT_PAY":
                        unpaidLeaves += item.DatedDuration ?? 0;
                        break;
                    case "PLANNED_UNPAID_L":
                        plannedUnpaidLeaves += item.DatedDuration ?? 0;
                        break;
                    default:
                        otherLeaves += item.DatedDuration ?? 0;
                        break;
                }
            }
            runResult.AnnualLeaveDays = annualLeaves;
            runResult.SickLeaveDays = sickLeaves;
            runResult.UnpaidLeaveDays = unpaidLeaves + plannedUnpaidLeaves;
            runResult.PlannedUnpaidLeaveDays = plannedUnpaidLeaves;
            runResult.OtherLeaveDays = otherLeaves;

            runResult.EmployeeWorkingDays = runResult.ActualWorkingDays - (annualLeaves + sickLeaves + unpaidLeaves + plannedUnpaidLeaves + otherLeaves);
            var otAndDeduction = await atb.GetTotalOtAndDeduction(runResult.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate);
            if (otAndDeduction != null)
            {
                runResult.OverTime = otAndDeduction.OTHours;
                runResult.UnderTime = otAndDeduction.DeductionHours;
            }

        }

        private string GetRunResultName(PayrollRunViewModel viewModel, PayrollPersonViewModel employee)
        {
            var name = "";
            if (viewModel.RunType == PayrollRunTypeEnum.Salary)
            {
                name = string.Concat("Payroll run result for the month of ", viewModel.PayrollStartDate.ToMMM_YYYY());
            }
            else
            {
                name = string.Concat("Adhoc payroll run result for the month of ", viewModel.PayrollStartDate.ToMMM_YYYY());
            }
            return name;
        }

        private string GetSalaryName(PayrollRunViewModel viewModel)
        {
            var name = "";
            if (viewModel.RunType == PayrollRunTypeEnum.Salary)
            {
                name = string.Concat("Payslip for the month of ", viewModel.PayrollStartDate.ToMMM_YYYY());
            }
            else
            {
                name = string.Concat("Adhoc payslip for the month of ", viewModel.PayrollStartDate.ToMMM_YYYY());
            }
            return name;
        }


        private async Task ExecuteUnProcessedTransactions(PayrollRunViewModel viewModel)
        {
            if (viewModel.UnProcessedTransactionList.Count <= 0)
            {
                return;
            }
            var transactionElementGroup =
                 (from val in viewModel.UnProcessedTransactionList
                  group val by new
                  {
                      val.PersonId,
                      val.ElementId
                  } into elements
                  select new PayrollElementRunResultViewModel
                  {
                      PersonId = elements.Key.PersonId,
                      ElementId = elements.Key.ElementId,
                      PayrollElementId= elements.Key.ElementId,
                      Amount = elements.Sum(x => x.Amount),
                      EarningAmount = elements.Sum(x => x.EarningAmount),
                      DeductionAmount = elements.Sum(x => x.DeductionAmount),
                      OpeningBalance = elements.Sum(x => x.OpeningBalance),
                      ClosingBalance = elements.Sum(x => x.ClosingBalance),
                      Quantity = elements.Sum(x => x.Quantity),
                      EarningQuantity = elements.Sum(x => x.EarningQuantity),
                      DeductionQuantity = elements.Sum(x => x.DeductionQuantity),
                      OpeningQuantity = elements.Sum(x => x.OpeningQuantity),
                      ClosingQuantity = elements.Sum(x => x.ClosingQuantity)
                  }).ToList();

            var processStatus = await _lovBusiness.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "PROCESSED");
            foreach (var employee in viewModel.PayrollActiveEmployeeList)
            {
                var runResultViewModel = viewModel.EmployeePayrollRunResult.FirstOrDefault(x => x.PersonId == employee.PersonId
                && x.ExecutionStatus == ExecutionStatusEnum.Success);
                if (runResultViewModel == null)
                {
                    continue;
                }
                var transactions = viewModel.UnProcessedTransactionList.Where(x => x.PersonId == employee.PersonId).ToList();
               
                foreach (var transaction in transactions)
                {
                    var exists = AddElementRunResult(viewModel, runResultViewModel, transaction, Guid.NewGuid().ToString());
                    transaction.ProcessStatusId = processStatus.Id;
                }
            }
            foreach (var item in viewModel.EmployeePayrollElementRunResult)
            {
                item.Amount = item.Amount.RoundToTwoDecimal();
                item.EarningAmount = item.EarningAmount.RoundToTwoDecimal();
                item.DeductionAmount = item.DeductionAmount.RoundToTwoDecimal();
                item.OpeningBalance = item.OpeningBalance.RoundPayrollAmount();
                item.ClosingBalance = item.ClosingBalance.RoundPayrollAmount();

                item.Quantity = item.Quantity.RoundPayrollAmount();
                item.EarningQuantity = item.EarningQuantity.RoundPayrollAmount();
                item.DeductionQuantity = item.DeductionQuantity.RoundPayrollAmount();
                item.OpeningQuantity = item.OpeningQuantity.RoundPayrollAmount();
                item.ClosingQuantity = item.ClosingQuantity.RoundPayrollAmount();
            }

        }
        private bool AddElementRunResult(PayrollRunViewModel viewModel, PayrollRunResultViewModel runResultViewModel
            , PayrollTransactionViewModel transaction, string payrollElementRunResultId)
        {
            var exists = viewModel.EmployeePayrollElementRunResult.FirstOrDefault(x => x.PersonId == runResultViewModel.PersonId
              && x.ElementId == transaction.ElementId);
            if (exists != null)
            {

                exists.Amount += transaction.Amount;
                exists.EarningAmount += transaction.EarningAmount;
                exists.DeductionAmount += transaction.DeductionAmount;
                exists.OpeningBalance += transaction.OpeningBalance;
                exists.ClosingBalance += transaction.ClosingBalance;

                exists.Quantity += transaction.Quantity;
                exists.EarningQuantity += transaction.EarningQuantity;
                exists.DeductionQuantity += transaction.DeductionQuantity;
                exists.OpeningQuantity += transaction.OpeningQuantity;
                exists.ClosingQuantity += transaction.ClosingQuantity;
                exists.DataAction = DataActionEnum.Edit;
                AddElementDailyRunResult(viewModel, transaction, exists.Id);
                return true;
            }
            else
            {
                var elementRunRsult = new PayrollElementRunResultViewModel
                {
                    Id = payrollElementRunResultId,
                    PayrollRunId = viewModel.Id,
                    PayrollId = viewModel.PayrollId,
                    PayrollRunResultId = runResultViewModel.Id,
                    ExecutionStatus = ExecutionStatusEnum.Success,
                    PayrollStartDate = viewModel.PayrollStartDate,
                    PayrollEndDate = viewModel.PayrollEndDate,
                    YearMonth = viewModel.YearMonth,


                    Amount = transaction.Amount,
                    EarningAmount = transaction.EarningAmount,
                    DeductionAmount = transaction.DeductionAmount,
                    OpeningBalance = transaction.OpeningBalance,
                    ClosingBalance = transaction.ClosingBalance,

                    Quantity = transaction.Quantity,
                    EarningQuantity = transaction.EarningQuantity,
                    DeductionQuantity = transaction.DeductionQuantity,
                    OpeningQuantity = transaction.OpeningQuantity,
                    ClosingQuantity = transaction.ClosingQuantity,


                    Name = transaction.Name,
                    ElementType = transaction.ElementType,
                    ElementCategory = transaction.ElementCategory,
                    ElementClassification = transaction.ElementClassification,
                    ElementId = transaction.ElementId,
                    PayrollElementId = transaction.ElementId,
                    PersonId = transaction.PersonId,
                    DataAction = DataActionEnum.Create
                };
                viewModel.EmployeePayrollElementRunResult.Add(elementRunRsult);
                AddElementDailyRunResult(viewModel, transaction, elementRunRsult.Id);
                return false;

            }


        }

        private void AddElementDailyRunResult(PayrollRunViewModel viewModel, PayrollTransactionViewModel transaction, string elementRunRsultId)
        {
            var elementDailyRunResult = new PayrollElementDailyRunResultViewModel
            {
                PayrollId = viewModel.PayrollId,
                PayrollRunId = viewModel.Id,
                ElementId = transaction.ElementId,
                PersonId = transaction.PersonId,
                PayrollElementRunResultId = elementRunRsultId,
                Amount = transaction.Amount,
                EarningAmount = transaction.EarningAmount,
                DeductionAmount = transaction.DeductionAmount,
                OpeningBalance = transaction.OpeningBalance,
                ClosingBalance = transaction.ClosingBalance,
                Quantity = transaction.Quantity,
                EarningQuantity = transaction.EarningQuantity,
                DeductionQuantity = transaction.DeductionQuantity,
                OpeningQuantity = transaction.OpeningQuantity,
                ClosingQuantity = transaction.ClosingQuantity,

                Name = transaction.Name,
                PayrollStartDate = viewModel.PayrollStartDate,
                PayrollEndDate = viewModel.PayrollEndDate,
                Date = transaction.EffectiveDate,
                YearMonth = viewModel.YearMonth,
                ElementType = transaction.ElementType,
                ElementCategory = transaction.ElementCategory,
                ElementClassification = transaction.ElementClassification,
                ExecutionStatus = ExecutionStatusEnum.Success,
                PayrollTransactionId = transaction.Id,
                DataAction = DataActionEnum.Create
            };

            transaction.ProcessStatus = PayrollProcessStatusEnum.Processed;
            transaction.ProcessedDate = DateTime.Now;
            var exists = viewModel.EmployeePayrollElementDailyRunResult.FirstOrDefault(x => x.PersonId == transaction.PersonId
               && x.ElementId == transaction.ElementId && x.Date == transaction.EffectiveDate);
            if (exists != null)
            {
                exists.Amount += transaction.Amount;
                exists.EarningAmount += transaction.EarningAmount;
                exists.DeductionAmount += transaction.DeductionAmount;
                exists.OpeningBalance += transaction.OpeningBalance;
                exists.ClosingBalance += transaction.ClosingBalance;
                exists.Quantity += transaction.Quantity;
                exists.EarningQuantity += transaction.EarningQuantity;
                exists.DeductionQuantity += transaction.DeductionQuantity;
                exists.OpeningQuantity += transaction.OpeningQuantity;
                exists.ClosingQuantity += transaction.ClosingQuantity;
            }
            else
            {
                viewModel.EmployeePayrollElementDailyRunResult.Add(elementDailyRunResult);
            }

        }

        private async Task<string> LoadOTAndDeduction(DateTime payrollDate, string payRollRunId, DateTime attendanceStartDate, DateTime attendanceEndDate)
        {
            var atb = _sp.GetService<IAttendanceBusiness>();
            var ptb = _sp.GetService<IPayrollTransactionsBusiness>();
            var peb = _sp.GetService<IPayrollElementBusiness>();
            var lbb = _sp.GetService<ILeaveBalanceSheetBusiness>();
            var output = "success";
            var attendanceOtModel = await atb.GetOTPayTransactionList(attendanceStartDate, attendanceEndDate, payRollRunId);
            if (attendanceOtModel.Count > 0)
            {
                var transactionList = new List<PayrollTransactionViewModel>();
                var minAttendanceDate = attendanceOtModel.Min(x => x.AttendanceDate);
                var maxAttendanceDate = attendanceOtModel.Max(x => x.AttendanceDate);
                var existingOTandDeductionTransactions = await ptb.GetOtandDedcutionTransactions(payrollDate.AddDays(-31), payrollDate);

                var userSalaryList = await peb.GetAllUserSalary();
                //  Log.Instance.Info(DelimeterEnum.Space, "basicTransportAndFoddTransactions Count:", basicTransportAndFoddTransactions.Count);
                foreach (AttendanceViewModel model in attendanceOtModel)
                {

                    var hour = (model.SponsorCode == "CAYAN") ? ((model.NationalityCode ?? "") == "SA" ? 8 : 9) : 8;
                    var ot = model.OTHours ?? TimeSpan.Zero;
                    var dedcution = model.DeductionHours ?? TimeSpan.Zero;
                    var attendance = model.AttendanceFlag ?? "Present";
                    var bs = model.BasicSalary ?? 0;
                    var hourlySalary = (bs / 30) / hour;
                    if (ot != TimeSpan.Zero)
                    {

                    }
                    if (dedcution != TimeSpan.Zero)
                    {
                        var existingLateDedcutionTransactions = existingOTandDeductionTransactions.Where(x => x.ElementCode == "LATE_COMING_DEDUCTION" && x.PersonId == model.PersonId && x.EffectiveDate == model.AttendanceDate).ToList();

                        if (existingLateDedcutionTransactions == null || existingLateDedcutionTransactions?.Count() == 0)
                        {
                            var amt = dedcution.TotalHours * (hourlySalary);

                            amt = 0;//Added for Feb payroll
                            var payModel = await GetTransationVewModel(amt.RoundToTwoDecimal(), model.AttendanceDate, "LATE_COMING_DEDUCTION", model.PersonId, model.Id);
                            // payModel.DeductionTime = dedcution;
                            payModel.Operation = DataOperation.Create;
                            transactionList.Add(payModel);
                        }
                        else if (existingLateDedcutionTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                        {
                            var amt = dedcution.TotalHours * (hourlySalary);
                            amt = 0;//Added for Feb payroll
                            var existingModel = existingLateDedcutionTransactions.FirstOrDefault();
                            existingModel.Amount = amt.RoundToTwoDecimal();
                            existingModel.PostedDate = payrollDate;
                            existingModel.Operation = DataOperation.Correct;
                            transactionList.Add(existingModel);
                        }
                    }
                    if (attendance == "Absent")
                    {
                        var isDayOff = await lbb.IsDayOff(model.PersonId, model.AttendanceDate);
                        if (!isDayOff)
                        {
                            var existingAbsentTransactions = existingOTandDeductionTransactions.Where(x => x.ElementCode == "UNPAID_LEAVE" && x.PersonId == model.PersonId && x.EffectiveDate == model.AttendanceDate).ToList();
                            if (model.SponsorCode == "CAYAN")
                            {
                                double deductionAmount = 0;
                                var salary = userSalaryList.Where(x => x.Id == model.UserId)?.FirstOrDefault()?.Code;
                                var salaryDouble = double.Parse(salary != null ? salary : "0");
                                deductionAmount += salaryDouble.PayrollDailyAmountAsPerWorkingDays();

                                if (deductionAmount > 0)
                                {
                                    if (existingAbsentTransactions == null || existingAbsentTransactions?.Count() == 0)
                                    {
                                        var payModel = await GetTransationVewModel(deductionAmount, model.AttendanceDate, "UNPAID_LEAVE", model.PersonId, model.Id);
                                        payModel.Operation = DataOperation.Create;
                                        transactionList.Add(payModel);
                                    }
                                    else if (existingAbsentTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                                    {
                                        var existingModel = existingAbsentTransactions.FirstOrDefault();
                                        existingModel.Amount = deductionAmount.RoundToTwoDecimal();
                                        existingModel.PostedDate = payrollDate;
                                        existingModel.Operation = DataOperation.Correct;
                                        transactionList.Add(existingModel);
                                    }
                                }

                            }
                        }
                    }
                    // }
                }
                UpdateElementDetail(transactionList);
                var bulkInsertList = transactionList.Where(x => x.Operation == DataOperation.Create).ToList();

                await ptb.BulkInsert(bulkInsertList, false);
                var bulkUpdateList = transactionList.Where(x => x.Operation == DataOperation.Correct).ToList();
                await ptb.BulkUpdate(bulkUpdateList, false);


            }
            await atb.UpdateOTPayTransToProcessed(payrollDate, payRollRunId);

            return output;
        }


        public async Task LoadTicketAmountPostingOnTenthMonth(DateTime payrollDate, string payRollRunId)
        {
            var ptb = _sp.GetService<IPayrollTransactionsBusiness>();
            var ticketEligibleDetails = await GetTicketEligibleEmployeeDetails(payrollDate, payRollRunId);
            var earningTransactions = await ptb.GetTicketEarningTransactions(payrollDate.AddDays(-31), payrollDate);

            if (ticketEligibleDetails.Count > 0)
            {
                var transactionList = new List<PayrollTransactionViewModel>();

                foreach (var item in ticketEligibleDetails)
                {
                    if (item.IsEligibleForTicketClaim)
                    {
                        if (item.IsEligibleForAirTicketForSelf)
                        {
                            var existingSelfTransactions = earningTransactions.Where(x => x.ElementCode == "ANNUAL_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == payrollDate.Month.ToString()).ToList();

                            if (existingSelfTransactions == null || existingSelfTransactions.Count == 0)
                            {
                                var ticketCost = item.TravelClass == TravelClassEnum.Business ? item.AverageBusinessTicketCost : item.AverageEconomyTicketCost;
                                double amount = ticketCost;
                                var transModel = GetTicketTransationModel(amount.RoundToTwoDecimal(), payrollDate, "ANNUAL_TICKET_EARNING", item.PersonId, 0, 0);
                                transModel.Operation = DataOperation.Create;
                                transactionList.Add(transModel);
                            }
                            else if (existingSelfTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                            {

                                var ticketCost = item.TravelClass == TravelClassEnum.Business ? item.AverageBusinessTicketCost : item.AverageEconomyTicketCost;
                                double amount = ticketCost;

                                var existModel = existingSelfTransactions.FirstOrDefault();
                                existModel.Operation = DataOperation.Correct;
                                existModel.Amount = amount.RoundToTwoDecimal();
                                existModel.EffectiveDate = payrollDate;
                                existModel.PostedDate = payrollDate;
                                transactionList.Add(existModel);
                            }
                        }

                        if (item.IsEligibleForAirTicketForDependant)
                        {
                            var ticketCost = item.TravelClass == TravelClassEnum.Business ? item.AverageBusinessTicketCost : item.AverageEconomyTicketCost;
                            var infantAmount = (item.InfantCount * ticketCost);
                            var adultAmount = (item.AdultCount * ticketCost);
                            var kidAmount = (item.KidsCount * ticketCost);
                            // double amount = infantAmount + adultAmount + kidAmount;

                            if (infantAmount != 0)
                            {
                                var existingInfantTransactions = earningTransactions.Where(x => x.ElementCode == "ANNUAL_INFANT_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == payrollDate.Month.ToString()).ToList();
                                if (existingInfantTransactions == null || existingInfantTransactions.Count == 0)
                                {
                                    var transModel = GetTicketTransationModel(infantAmount.RoundToTwoDecimal(), payrollDate, "ANNUAL_INFANT_TICKET_EARNING", item.PersonId, 0, 0);
                                    transModel.Operation = DataOperation.Create;
                                    transactionList.Add(transModel);
                                }
                                else if (existingInfantTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                                {
                                    var existModel = existingInfantTransactions.FirstOrDefault();
                                    existModel.Operation = DataOperation.Correct;
                                    existModel.Amount = infantAmount.RoundToTwoDecimal();
                                    existModel.EffectiveDate = payrollDate;
                                    existModel.PostedDate = payrollDate;
                                    transactionList.Add(existModel);
                                }
                            }
                            if (kidAmount != 0)
                            {
                                var existingChildTransaction = earningTransactions.Where(x => x.ElementCode == "ANNUAL_CHILD_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == payrollDate.Month.ToString()).ToList();
                                if (existingChildTransaction == null || existingChildTransaction.Count == 0)
                                {
                                    var transModel = GetTicketTransationModel(kidAmount.RoundToTwoDecimal(), payrollDate, "ANNUAL_CHILD_TICKET_EARNING", item.PersonId, 0, 0);
                                    transModel.Operation = DataOperation.Create;
                                    transactionList.Add(transModel);
                                }
                                else if (existingChildTransaction.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                                {
                                    var existModel = existingChildTransaction.FirstOrDefault();
                                    existModel.Operation = DataOperation.Correct;
                                    existModel.Amount = kidAmount.RoundToTwoDecimal();
                                    existModel.EffectiveDate = payrollDate;
                                    existModel.PostedDate = payrollDate;
                                    transactionList.Add(existModel);
                                }
                            }
                            if (adultAmount != 0)
                            {
                                var existingAdultTransactions = earningTransactions.Where(x => x.ElementCode == "ANNUAL_DEPENDENT_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == payrollDate.Month.ToString()).ToList();
                                if (existingAdultTransactions == null || existingAdultTransactions.Count == 0)
                                {
                                    var transModel = GetTicketTransationModel(adultAmount.RoundToTwoDecimal(), payrollDate, "ANNUAL_DEPENDENT_TICKET_EARNING", item.PersonId, 0, 0);
                                    transModel.Operation = DataOperation.Create;
                                    transactionList.Add(transModel);
                                }
                                else if (existingAdultTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                                {
                                    var existModel = existingAdultTransactions.FirstOrDefault();
                                    existModel.Operation = DataOperation.Correct;
                                    existModel.Amount = adultAmount.RoundToTwoDecimal();
                                    existModel.EffectiveDate = payrollDate;
                                    existModel.PostedDate = payrollDate;
                                    transactionList.Add(existModel);
                                }
                            }

                        }
                    }


                }
                var bulkInsertList = transactionList.Where(x => x.Operation == DataOperation.Create).ToList();
                UpdateElementDetail(bulkInsertList);
                await ptb.BulkInsert(bulkInsertList, false);

                var bulkUpdateList = transactionList.Where(x => x.Operation == DataOperation.Correct).ToList();
                await ptb.BulkUpdate(bulkUpdateList, false);
            }


        }


        //private void UpdateAccuralsElementDetail(List<PayrollTransactionViewModel> transactionList)
        //{
        //    var elements = _elementBusiness.ViewModelList("", new Dictionary<string, object>());
        //    foreach (var data in transactionList)
        //    {
        //        var element = elements.FirstOrDefault(x => x.Code == data.ElementCode);
        //        if (element != null)
        //        {
        //            data.ElementId = element.ElementId;
        //            data.ElementType = element.ElementType;
        //            data.ElementClassification = element.ElementClassification;
        //            data.ElementCategory = element.ElementCategory;
        //        }
        //        else
        //        {
        //            //Log.Instance.Info(DelimeterEnum.Space, "Element not exists code: ", data.ElementCode);
        //            //Log.Instance.Error(string.Concat("Element not exists code: ", data.ElementCode));
        //        }
        //    }
        //}


        private async Task<List<TicketAccrualViewModel>> GetTicketAccrualDetails(DateTime payrollDate, string payRollRunId)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("EED", payrollDate);
            //prms.AddIfNotExists("payRollRunId", payRollRunId);
            //var cypher = @"match(pr:HRS_PersonRoot)<-[:R_PersonRoot]-(per:HRS_Person{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //     where per.EffectiveStartDate <= {EED} <= per.EffectiveEndDate
            //     match (pr)<-[:R_User_PersonRoot]-(u:ADM_User{ IsDeleted: 0,CompanyId: 1, Status: 'Active' })
            //     match(pr)<-[:R_PayrollRun_PersonRoot]-(payR:PAY_PayrollRun{Id:{payRollRunId}})
            //     optional match(per)-[:R_Person_Nationality]->(n:HRS_Nationality{ IsDeleted: 0,CompanyId: 1, Status: 'Active' })
            //     optional match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot)
            //     <-[:R_AssignmentRoot]-(ass: HRS_Assignment{IsDeleted: 0,CompanyId: 1, Status: 'Active' }) 
            //     where ass.EffectiveStartDate <= {EED} <= ass.EffectiveEndDate
            //     optional match(ass)-[:R_Assignment_GradeRoot]->(gr:HRS_GradeRoot{ IsDeleted:0,CompanyId:1})
            //     optional match(gr)<-[:R_GradeRoot]-(g:HRS_Grade{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //     where g.EffectiveStartDate <= {EED} <= g.EffectiveEndDate

            //     optional match(pr)<-[:R_SalaryInfoRoot_PersonRoot]-(salr:PAY_SalaryInfoRoot)
            //     optional match(salr)<-[:R_SalaryInfoRoot]-(sal{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //     where sal.EffectiveStartDate <= {EED} <= sal.EffectiveEndDate

            //     optional match(pr)<-[:R_DependentRoot_PersonRoot]-(dr:HRS_DependentRoot{IsDeleted: 0,CompanyId: 1, Status: 'Active'})

            //     optional match(:NTS_TemplateMaster{Code:'N_VISA_DEPENDENT'})<-[:R_TemplateRoot]-(:NTS_Template)<-[:R_Note_Template]-(nn:NTS_Note)-[:R_Note_Reference]->(dr)<-[:R_DependentRoot]-(d:HRS_Dependent{IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //     where d.EffectiveStartDate <= { EED} <= d.EffectiveEndDate and
            //     datetime({ EED}) <= datetime(coalesce(nn.ExpiryDate, datetime())) and(duration.inDays(datetime(d.DateOfBirth),
            //     datetime({ EED})).days * 1.0)/ 365 <= 2


            //     optional match(:NTS_TemplateMaster{ Code: ' '})< -[:R_TemplateRoot] - (: NTS_Template) < -[:R_Note_Template] - (not1: NTS_Note) -[:R_Note_Reference]->(dr) < -[:R_DependentRoot] - (d1: HRS_Dependent{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //     where d1.EffectiveStartDate <= { EED} <= d1.EffectiveEndDate and datetime({ EED}) <= datetime(not1.ExpiryDate) and 2 < (duration.inDays(datetime(d1.DateOfBirth), datetime({ EED})).days * 1.0)/ 365 <= 12


            //     optional match(:NTS_TemplateMaster{ Code: 'N_VISA_DEPENDENT'})< -[:R_TemplateRoot] - (: NTS_Template) < -[:R_Note_Template] - (not2: NTS_Note) -[:R_Note_Reference]->(dr) < -[:R_DependentRoot] - (d2: HRS_Dependent{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //     where d2.EffectiveStartDate <= { EED} <= d2.EffectiveEndDate and datetime({ EED}) <= datetime(not2.ExpiryDate) and(duration.inDays(datetime(d2.DateOfBirth), datetime()).days * 1.0) / 365 > 12


            //     with per, n, g, sal, Count(d.Id) as InfantCount, Count(d1.Id) as KidsCount, Count(d2.Id) as AdultCount, pr,u

            //     return u.Id as UserId,pr.Id as PersonId, per.FirstName, per.MiddleName, per.LastName,  n.Name, coalesce(n.AverageEconomyTicketCost, 0) as AverageEconomyTicketCost, 
            //     coalesce(n.AverageBusinessTicketCost, 0) as AverageBusinessTicketCost, g.Name as Grade, 
            //     coalesce(g.TravelClass, 'Economy') as TravelClass, coalesce(sal.IsEligibleForAirTicketForSelf, false) as IsEligibleForAirTicketForSelf, 
            //     coalesce(sal.IsEligibleForAirTicketForDependant, false) as IsEligibleForAirTicketForDependant,  InfantCount, KidsCount, AdultCount";
            var cypher = $@"Select u.""Id"" as UserId,per.""Id"" as PersonId, per.""FirstName"", per.""MiddleName"", per.""LastName"",  n.""NationalityName"", coalesce(n.""AverageEconomyTicketCost"", '0') as AverageEconomyTicketCost, 
                coalesce(n.""AverageBusinessTicketCost"", '0') as AverageBusinessTicketCost, g.""GradeName"" as Grade, 
                 coalesce(g.""TravelClass"", 'Economy') as TravelClass, coalesce(sal.""IsEmployeeEligibleForFlightTicketsForSelf"", 'false') as IsEligibleForAirTicketForSelf, 
                 coalesce(sal.""IsEmployeeEligibleForFlightTicketsForDependants"", 'false') as IsEligibleForAirTicketForDependant, Count(d.""Id"") as  InfantCount, Count(d1.""Id"") as KidsCount, Count(d2.""Id"") as AdultCount

from cms.""N_CoreHR_HRPerson"" as per 
 join public.""User"" as u on per.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
 join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=per.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRun"" as prr on prr.""Id""=prp.""PayrollRunId"" and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as ass on per.""Id""=ass.""EmployeeId"" and ass.""IsDeleted""=false and ass.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id""=per.""NationalityId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=ass.""AssignmentGradeId""  and g.""IsDeleted""=false and g.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryInfo"" as sal on sal.""PersonId""=per.""Id"" and sal.""IsDeleted""=false and sal.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join public.""NtsNote"" as pn on pn.""Id""=si.""NoteId""  and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join public.""NtsNote"" as n on pn.""Id""=n.""ParentNoteId""  and n.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join cms.""N_PayrollHR_SalaryElementInfo"" as ps on ps.""NtsNoteId""=n.""Id"" and ps.""IsDeleted""=false
-- and ps.""EffectiveStartDate""::Date <= '{payrollDate}'::Date  and '{payrollDate}'::Date<= ps.""EffectiveEndDate""::Date
--left join cms.""N_PayrollHR_PayrollElement"" as e on sei.""ElementId""=e.""Id"" and e.""IsDeleted""=false
--and  e.""EffectiveStartDate""::Date <= '{payrollDate}'::Date <= e.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRDependant"" as dr on dr.""EmployeeId""=per.""Id"" and dr.""IsDeleted""=false and dr.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_DependentDocuments_VisaDependent"" as d on d.""DependentId""=dr.""Id"" and d.""IsDeleted""=false  and d.""CompanyId""='{_repo.UserContext.CompanyId}'
and -- d.""EffectiveStartDate""::Date <= '{ payrollDate}'::Date and  '{ payrollDate}'::Date<= d.""EffectiveEndDate""::Date and
'{ payrollDate}'::Date <= coalesce(d.""ExpireDate""::Date,Now())::Date and DATE_PART('day','{ payrollDate}'::timestamp-dr.""DateOfBirth""::timestamp) * 1.0/ 365 <= 2
left join cms.""N_DependentDocuments_VisaDependent"" as d1 on d1.""DependentId""=dr.""Id""  and d1.""IsDeleted""=false and d1.""CompanyId""='{_repo.UserContext.CompanyId}'
--and  d1.""EffectiveStartDate"" <= '{ payrollDate}' and  '{ payrollDate}'<= d1.""EffectiveEndDate"" 
and '{ payrollDate}'::Date <= d1.""ExpireDate""::Date and 2 < DATE_PART('day', '{payrollDate}'::timestamp-dr.""DateOfBirth""::timestamp) * 1.0/ 365 and DATE_PART('day', '{payrollDate}'::timestamp-dr.""DateOfBirth""::timestamp) * 1.0/ 365<= 12
left join cms.""N_DependentDocuments_VisaDependent"" as d2 on d2.""DependentId""=dr.""Id"" and d2.""IsDeleted""=false and d2.""CompanyId""='{_repo.UserContext.CompanyId}'
--and  d2.""EffectiveStartDate"" <= '{ payrollDate}' and  '{ payrollDate}'<= d2.""EffectiveEndDate"" 
and '{ payrollDate}'::Date <= d2.""ExpireDate""::Date and  DATE_PART('day', '{payrollDate}'::timestamp-dr.""DateOfBirth""::timestamp) * 1.0/ 365 > 12
where prr.""Id""='{payRollRunId}' and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
group by per.""Id"",per.""FirstName"",per.""LastName"",n.""NationalityName"",n.""AverageEconomyTicketCost"",n.""AverageBusinessTicketCost"",g.""GradeName"",g.""TravelClass""
,sal.""IsEmployeeEligibleForFlightTicketsForSelf"",sal.""IsEmployeeEligibleForFlightTicketsForDependants"" ,d.""Id"",d2.""Id"",d1.""Id"",u.""Id""

";

            var result = await _payrunrepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);

            return result;
        }


        private async Task<List<TicketAccrualViewModel>> GetEOSAccrualDetails(DateTime payrollDate, string payRollRunId, DateTime attendanceStartDate, DateTime attendanceEndDate)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("EED", payrollDate);
            //prms.AddIfNotExists("startDate", attendanceStartDate);
            //prms.AddIfNotExists("endDate", attendanceEndDate);
            //prms.AddIfNotExists("payRollRunId", payRollRunId);

            //var cypher = @"match(pr)<-[:R_PersonRoot]-(per:HRS_Person{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where per.EffectiveStartDate <= {EED} <= per.EffectiveEndDate
            //         match(pr)<-[:R_PayrollRun_PersonRoot]-(payR:PAY_PayrollRun{Id:{payRollRunId}})
            //         match(pr)<-[:R_User_PersonRoot]-(user)
            //         optional match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot)<-[:R_AssignmentRoot]-(ass: HRS_Assignment{IsDeleted: 0,CompanyId: 1, Status: 'Active' }) 
            //         where ass.EffectiveStartDate <= {EED} <= ass.EffectiveEndDate
            //         optional match(pr)<-[:R_SalaryInfoRoot_PersonRoot]-(salr:PAY_SalaryInfoRoot)
            //         optional match(salr)<-[:R_SalaryInfoRoot]-(sal{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where sal.EffectiveStartDate <= {EED} <= sal.EffectiveEndDate

            //         optional match(salr)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(pssalr:PAY_SalaryElementInfo{ IsDeleted: 0 })-[:R_SalaryElementInfo_ElementRoot]->(pepssalr:PAY_ElementRoot{ IsDeleted: 0 })<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: 1, Status: 'Active'}) 
            //     where e.EffectiveStartDate <= {EED} <= e.EffectiveEndDate and  e.Code in ['BASIC','HOUSING','TRANSPORT']

            //         optional match (pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0,Status:'Active'})
            //         optional match(cr)<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0, IsLatest:true, Status:'Active'})

            //      with per, sal, SUM(coalesce(pssalr.Amount,0.0)) as TotalSalary, ass, user, pr, c

            //       optional match(user)<-[:R_Attendance_User]-(att:TAA_Attendance) where datetime({startDate}) <= datetime(att.AttendanceDate) <= datetime({endDate}) and (att.SystemAttendance is not null or att.OverrideAttendance is not null) 
            //     and coalesce(att.OverrideAttendance, att.SystemAttendance) = 'Absent' and att.AttendanceLeaveType = 'UnpaidLeave'

            //      with per, sal, TotalSalary, ass, count(att.Id) as TotalDuration, user, pr, c
            //         return pr.Id as PersonId,user.Id as UserId, per.FirstName, per.MiddleName, per.LastName, TotalSalary, ass.DateOfJoin as DateOfJoin, TotalDuration as UnpaidLeaveDays, coalesce(sal.UnpaidLeavesNotInSystem, 0) as UnpaidLeavesNotInSystem, c.EffectiveEndDate as ContractEndDate";
            var cypher = $@"Select  per.""Id"" as PersonId,u.""Id"" as UserId, per.""FirstName"", per.""MiddleName"", per.""LastName"", SUM(coalesce(CAST (pssalr.""Amount"" AS Double PRECISION),0.0)) as TotalSalary, ass.""DateOfJoin"" as DateOfJoin,  count(att.""Id"") as UnpaidLeaveDays, coalesce(CAST (sal.""UnpaidLeavesNotInSystem"" as Integer), 0) as UnpaidLeavesNotInSystem, c.""EffectiveEndDate"" as ContractEndDate
from cms.""N_CoreHR_HRPerson"" as per
 join public.""User"" as u on per.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
 join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=per.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRun"" as prr on prr.""Id""=prp.""PayrollRunId"" and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as ass on per.""Id""=ass.""EmployeeId"" and ass.""IsDeleted""=false and ass.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join public.""N_CoreHR_HRNationality"" as n on n.""Id""=per.""NationalityId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=ass.""AssignmentGradeId"" and hg.""IsDeleted""=false and hg.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryInfo"" as sal on sal.""PersonId""=per.""Id"" and sal.""IsDeleted""=false and sal.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as pn on pn.""Id""=sal.""NtsNoteId""  and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as n on pn.""Id""=n.""ParentNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryElementInfo"" as pssalr on pssalr.""NtsNoteId""=n.""Id"" and pssalr.""IsDeleted""=false and pssalr.""CompanyId""='{_repo.UserContext.CompanyId}'
 --and pssalr.""EffectiveStartDate""::Date <= '{payrollDate}'::Date <= pssalr.""EffectiveEndDate""::Date
left join cms.""N_PayrollHR_PayrollElement"" as e on pssalr.""ElementId""=e.""Id"" and e.""IsDeleted""=false and   e.""CompanyId""='{_repo.UserContext.CompanyId}' and
--and  e.""EffectiveStartDate""::Date <= '{payrollDate}'::Date and '{payrollDate}'::Date <= e.""EffectiveEndDate""::Date and 
e.""ElementCode"" in ('BASIC','HOUSING','TRANSPORT')
left join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=per.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_Attendance"" as att on att.""UserId""=u.""Id"" and att.""IsDeleted""=false and att.""CompanyId""='{_repo.UserContext.CompanyId}'
--where  '{attendanceStartDate}'::Date < att.""AttendanceDate""::Date < '{attendanceEndDate}'::Date and (att.""SystemAttendance"" is not null or att.""OverrideAttendance"" is not null) 
 --and coalesce(att.""OverrideAttendance"", att.""SystemAttendance"") = 'Absent' and att.""AttendanceLeaveType"" = 'UnpaidLeave'
where per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
group by per.""Id"",u.""Id"",per.""FirstName"",per.""LastName"",per.""MiddleName"",ass.""DateOfJoin"",
att.""Id"",pssalr.""Amount"",sal.""UnpaidLeavesNotInSystem"",c.""EffectiveEndDate""
";

            var result = await _payrunrepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);
            return result;
        }


        private async Task<List<TicketAccrualViewModel>> GetVacationAccrualDetails(DateTime payrollDate, string payRollRunId)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("EED", payrollDate);
            //prms.AddIfNotExists("payRollRunId", payRollRunId);

            //var cypher = @"match(pr)<-[:R_PersonRoot]-(per:HRS_Person{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where per.EffectiveStartDate <= {EED} <= per.EffectiveEndDate
            //         match(pr)<-[:R_PayrollRun_PersonRoot]-(payR:PAY_PayrollRun{Id:{payRollRunId}})
            //         optional match(pr)<-[:R_User_PersonRoot]-(user)
            //         optional match(user)<-[:R_LeaveBalanceSheet_User]-(leave:TAA_LeaveBalanceSheet{IsDeleted: 0,CompanyId: 1, Status: 'Active'})-[:R_LeaveBalanceSheet_LeaveType]->(t:TAA_LeaveType{Code:'ANNUAL_LEAVE'})
            //         optional match (pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0,Status:'Active'})
            //         optional match(cr)<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0})
            //         where c.EffectiveStartDate<= {EED} <=c.EffectiveEndDate
            //         with pr, sum(leave.ClosingBalance) as leaveBalance, c, per,user
            //         return pr.Id as PersonId,user.Id as UserId,
            //         coalesce(leaveBalance,0) - (coalesce(c.AnnualLeaveEntitlement,0)/12 * 1.0) as OpeningBalance,coalesce(c.AnnualLeaveEntitlement,0)/12 * 1.0 as MonthlyAccrualDays, coalesce(leaveBalance, 0) as ClosingBalance,  coalesce(c.AnnualLeaveEntitlement, 0) as AnnualLeaveEntitlement";

            var cypher = $@"Select per.""Id"" as PersonId,u.""Id"" as UserId,
                     coalesce(sum(CAST (leave.""ClosingBalance"" AS INTEGER)),0) - (coalesce(cast(c.""AnnualLeaveEntitlement"" as Integer))/12 * 1.0) as OpeningBalance,
coalesce((cast(c.""AnnualLeaveEntitlement"" as Integer))/12 * 1.0,0) as MonthlyAccrualDays,
coalesce(sum(CAST (leave.""ClosingBalance"" AS INTEGER)), 0) as ClosingBalance,  
coalesce((cast(c.""AnnualLeaveEntitlement"" as Integer)), 0) as AnnualLeaveEntitlement
from cms.""N_CoreHR_HRPerson"" as per
join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=per.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRun"" as prr on prr.""Id""=prp.""PayrollRunId"" and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""User"" as u on per.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_LeaveBalanceSheet"" as leave on u.""Id""=leave.""UserId"" and leave.""IsDeleted""=false and leave.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=per.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
where per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
group by per.""Id"",u.""Id"",leave.""ClosingBalance"",c.""AnnualLeaveEntitlement"" 
";
            var result = await _payrunrepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);

            return result;
        }

        private void UpdateElementDetail(List<PayrollTransactionViewModel> transactionList)
        {
            // TODO
            var elements = new List<ElementViewModel>();// _elementBusiness.ViewModelList("", new Dictionary<string, object>());
            foreach (var data in transactionList)
            {
                var element = elements.FirstOrDefault(x => x.Code == data.ElementCode);
                if (element != null)
                {
                    data.ElementId = element.ElementId;
                    data.DeductionAmount = element.ElementClassification == ElementClassificationEnum.Deduction ? data.Amount : 0.0;
                    data.EarningAmount = element.ElementClassification == ElementClassificationEnum.Earning ? data.Amount : 0.0;
                    data.ElementType = element.ElementType;
                    data.ElementClassification = element.ElementClassification;
                    data.ElementCategory = element.ElementCategory;
                    if (element.ElementClassification == ElementClassificationEnum.Deduction)
                    {
                        data.Amount = data.DeductionAmount * -1;
                    }
                }
                else
                {
                    //Log.Instance.Info(DelimeterEnum.Space, "Element not exists code: ", data.ElementCode);
                    //Log.Instance.Error(string.Concat("Element not exists code: ", data.ElementCode));
                }
            }
            // Log.Instance.Info(DelimeterEnum.Space, "End UpdateElementDetail");
        }

        public async Task<List<UserListOfValue>> GetIncludePersonList(string payrollId)
        {


            var cypher = $@"Select pr.""Id"" as PersonId,o.""Name"" as OrganizationName,j.""Name"" as JobName,p.""PersonFullName"" as PersonFullName
from cms.""N_CoreHR_HRPerson"" as per
join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=per.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRun"" as prr on prr.""Id""=prp.""PayrollRunId"" and prr.""Id""='{payrollId}'  and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""User"" as u on per.""UserId""=u.""Id"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_LeaveBalanceSheet"" as leave on u.""Id""=leave.""UserId"" and leave.""IsDeleted""=false and leave.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as o on a.""DepartmentId""=o.""Id"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
where per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            var result = await _payrunrepo.ExecuteQueryList<UserListOfValue>(cypher, null);
            return result;
        }

        public async Task<List<UserListOfValue>> GetExcludePersonList(string payrollGroupId, string payrollRunId, string searchParam, string payrollId, string orgId)
        {
            //var pay = _payrollGroupBusiness.GetSingleById(payrollGroupId);
            //var payroll = _payrollBusiness.GetSingleById(payrollId);
            //var date = payroll.PayrollStartDate.Value;
            //var endMonth = date;
            //if (pay.IsCutOffStartDayPreviousMonth)
            //    endMonth = endMonth.AddMonths(-1);

            //var Start = new DateTime(endMonth.Year, endMonth.Month, pay.CutOffStartDay);
            //var End = new DateTime(date.Year, date.Month, pay.CutOffEndDay);

            var cypher = string.Concat(@"match(pg:PAY_PayrollGroup{ Id:{payrollGroupId},IsDeleted: 0,CompanyId: { CompanyId},Status:{Status} })<-[:R_SalaryInfo_PayrollGroup]-(ps:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId} })
                     match(ps)-[:R_SalaryInfoRoot]->(psr:PAY_SalaryInfoRoot)
                     match(psr)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
                     match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                     where p.EffectiveStartDate <= {ESD} <= p.EffectiveEndDate 
                     match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                     match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                     where a.EffectiveStartDate <= {ESD} <= a.EffectiveEndDate                 

                     match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                     match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                     where j.EffectiveStartDate <= {ESD} <= j.EffectiveEndDate 
                     match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                     match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                     where o.EffectiveStartDate <= {ESD} <= o.EffectiveEndDate 

                     match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}}) 
                     optional match(u)<-[:R_Attendance_User]-(a1: TAA_Attendance) where { startDate} <= a1.AttendanceDate <= { endDate} and a1.PayrollPostedStatus is not null
                     optional match(o)-[:R_Organization_Payroll_OrganizationRoot]->(por:HRS_OrganizationRoot)<-[:R_OrganizationRoot]-(po:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                     where po.EffectiveStartDate <= {ESD} <= po.EffectiveEndDate 
                     optional match(pr)<-[R_PayrollRun_PersonRoot]-(prr:PAY_PayrollRun{ Id:{payrollId},IsDeleted: 0,CompanyId: { CompanyId}})               

                     with pr,p,ar,a,jr,j,orr,o,po,ps
                     where prr.Id is null and (o.IsPayrollOrganization=true or por.Id is not null)
                     and (ps.TakeAttendanceFromTAA=false or (ps.TakeAttendanceFromTAA=true and a1.Id is not null)) #SEARCH#
                     return distinct pr.Id as PersonId,o.Name as OrganizationName,j.Name as JobName,"
               , Helper.PersonFullNameWithSponsorshipNo("p", " as Name")
               , " order by ", Helper.PersonFullNameWithSponsorshipNo("p", "")
               );
            //  match(u) < -[:R_Attendance_User] - (a1: TAA_Attendance) where { startDate} <= a1.AttendanceDate <= { endDate}  and a1.PayrollPostedStatus = 'Submitted'
            //var prms = new Dictionary<string, object>
            //     {
            //         { "Status", StatusEnum.Active },
            //         { "CompanyId", CompanyId },
            //         { "ESD", DateTime.Now.ApplicationNow().Date },
            //         { "payrollId", payrollId },
            //         { "payrollGroupId", payrollGroupId },
            //         { "startDate", Start },
            //         { "endDate", End }
            //     };


            var search = "";
            if (searchParam.IsNotNullAndNotEmpty())
            {
                //search = string.Concat(@" and (
                //     p.FirstName ", searchParam.ToCaseInsensitiveContains(), @" or
                //     p.MiddleName ", searchParam.ToCaseInsensitiveContains(), @" or
                //     p.LastName ", searchParam.ToCaseInsensitiveContains(), @" or
                //     p.SponsorshipNo ", searchParam.ToCaseInsensitiveContains(), @" or
                //     p.Mobile ", searchParam.ToCaseInsensitiveContains(), @" or               
                //     o.IsPayrollOrganization=true and o.Name ", searchParam.ToCaseInsensitiveContains(), @" or
                //     por.Id is not null and po.Name ", searchParam.ToCaseInsensitiveContains(), @" or
                //     o.Name ", searchParam.ToCaseInsensitiveContains(), @" or
                //     j.Name ", searchParam.ToCaseInsensitiveContains(), @" 
                //     )");
                search = string.Concat(@" and (
                     p.FirstName ", searchParam, @" or
                     p.MiddleName ", searchParam, @" or
                     p.LastName ", searchParam, @" or
                     p.SponsorshipNo ", searchParam, @" or
                     p.Mobile ", searchParam, @" or               
                     o.IsPayrollOrganization=true and o.Name ", searchParam, @" or
                     por.Id is not null and po.Name ", searchParam, @" or
                     o.Name ", searchParam, @" or
                     j.Name ", searchParam, @" 
                     )");
            }
            cypher = cypher.Replace("#SEARCH#", search);
            // var result = ExecuteCypherList<UserListOfValue>(cypher, prms);
            var result = await _payrunrepo.ExecuteQueryList<UserListOfValue>(cypher, null);

            return result;
        }

        public async Task<PayrollRunViewModel> GetNextPayroll(DateTime startDate)
        {
            //var prms = new Dictionary<string, object>
            //     {
            //         { "Status", StatusEnum.Active },
            //         { "CompanyId", _repo.UserContext.CompanyId },
            //         { "ESD", DateTime.Now.ApplicationNow().Date },
            //         { "PayrollRunDate", DateTime.Now }
            //     };

            //var cypher = @"match(r:PAY_PayrollRun{ IsDeleted: 0,ExecutionStatus:'InProgress' }) return r limit 1";
            var inpro = (int)PayrollExecutionStatusEnum.InProgress;
            var submi = (int)PayrollExecutionStatusEnum.Submitted;
            var erro = (int)PayrollExecutionStatusEnum.Error;

            var cypher = $@"Select r from cms.""N_PayrollHR_PayrollRun"" as r where r.""IsDeleted""=false and r.""ExecutionStatus""='{inpro}' and r.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            var running = await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);

            //cypher = @"match(r:PAY_PayrollRun{IsDeleted: 0,Id:61 }) 
            //     where r.ExecutionStatus in ['Submitted','InProgress','Error']
            //     match (r)-[:R_PayrollRun_Payroll]->(p:PAY_Payroll)  
            //     match (p)-[:R_Payroll_LegalEntity_OrganizationRoot]->(or:HRS_OrganizationRoot)  
            //     match (p)-[:R_Payroll_PayrollGroup]->(pg:PAY_PayrollGroup)  
            //     with r,p,or,pg order by r.CreatedDate limit 1  with r,p,or,pg 
            //     set r.ExecutionStatus='InProgress',r.PayrollRunDate={PayrollRunDate} 
            //     return  r,p.Id as PayrollId
            //     ,or.Id as LegalEntityId,pg.Id as PayrollGroupId,p.PayrollStartDate as PayrollStartDate,p.PayrollEndDate as PayrollEndDate
            //     ,p.AttendanceStartDate as AttendanceStartDate,p.AttendanceEndDate as AttendanceEndDate
            //     ,p.YearMonth as YearMonth,p.RunType as RunType";
            cypher = $@" Select  r.*,p.""Id"" as PayrollId
                 ,or.""Id"" as LegalEntityId,pg.""Id"" as PayrollGroupId,p.""PayrollStartDate"" as PayrollStartDate,p.""PayrollEndDate"" as PayrollEndDate
                 ,p.""AttendanceStartDate"" as AttendanceStartDate,p.""AttendanceEndDate"" as AttendanceEndDate
                 ,p.""YearMonth"" as YearMonth,p.""RunType"" as RunType
                 from 
                cms.""N_PayrollHR_PayrollRun"" as r
                join cms.""N_PayrollHR_PayrollBatch"" as p on p.""Id""=r.""PayrollBatchId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=p.""PayrollGroupId"" and pg.""IsDeleted""=false  and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LegalEntity"" as or on or.""Id""=p.""LegalEntityId""  and or.""IsDeleted""=false and or.""CompanyId""='{_repo.UserContext.CompanyId}'
                where r.""ExecutionStatus"" in ('{submi}','{inpro}','{erro}') and r.""IsDeleted""=false and r.""CompanyId""='{_repo.UserContext.CompanyId}'
                ";
            var result = await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
            if (result != null)
            {
                var query = $@"update cms.""N_PayrollHR_PayrollRun"" set ""ExecutionStatus""='{inpro}',""PayrollRunDate""='{DateTime.Now}'";
                await _payrunrepo.ExecuteCommand(query, null);
            }
            return result;
        }


        private async Task<double> GetElementSalaryByPerson(DateTime attendanceDate, string PersonId, string SalaryCode)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("PersonId", PersonId);
            //prms.AddIfNotExists("SalaryCode", SalaryCode);
            //prms.AddIfNotExists("attendanceDate", attendanceDate);

            //var cypher = @"match (pr:HRS_PersonRoot{Id:{PersonId}})<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot)
            //      match(psr)<-[psrr:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo) where datetime(ps.EffectiveStartDate) <= datetime({attendanceDate}) <= datetime(ps.EffectiveEndDate)
            //      match(ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{ IsDeleted: 0,CompanyId: 1 })<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: 1 })
            //      where e.Code = {SalaryCode} and datetime(e.EffectiveStartDate) <= datetime({attendanceDate}) <= datetime(e.EffectiveEndDate)
            //      return ps.Amount as Salary";
            var cypher = string.Concat($@"select ps.""Amount"" as Salary
from cms.""N_CoreHR_HRPerson"" as pr
join cms.""N_PayrollHR_SalryInfo"" as si on si.""PersonId""=pr.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as pn on pn.""Id""=si.""NoteId""  and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on pn.""Id""=n.""ParentNoteId""  and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalryElementInfo"" as ps on ps.""NtsNoteId""=n.""Id"" and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
and ps.""EffectiveStartDate""::Date <= '{attendanceDate}'::Date and '{attendanceDate}'::Date <= ps.""EffectiveEndDate""::Date
join cms.""N_PayrollHR_PayrollElement"" as e on sei.""ElementId""=e.""Id"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
where  --e.""EffectiveStartDate""::Date <= '{attendanceDate}'::Date and '{attendanceDate}'::Date <= e.""EffectiveEndDate""::Date and
pr.""Id""='{PersonId}' and e.""ElementCode""='{SalaryCode}' and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'

");

            //var result = ExecuteCypherScalar<double?>(cypher, prms);
            var result = await _payrunrepo.ExecuteScalar<double?>(cypher, null);

            return result ?? 0;
        }



        public string GenerateNextPayNo(bool doCommit = true, string id = null)
        {
            var date = DateTime.Now.ApplicationNow().Date;
            return string.Concat("PAY-", String.Format("{0:dd.MM.yyyy}", date), "-", id);
        }

        public async Task<List<PayrollBatchViewModel>> GetPostedPayrollEmployeeList(string payrollGroupId, string payrollRunId, string payrollId)
        {
            //var pay = _payrollGroupBusiness.GetSingleById(payrollGroupId);
            //var payroll = _payrollBusiness.GetSingleById(payrollId);
            //var date = payroll.PayrollStartDate.Value;
            //var stmonth = date.Month;
            //var edmonth = date.Month;
            //if (pay.IsCutOffStartDayPreviousMonth)
            //    stmonth = stmonth - 1;

            //var Start = new DateTime(date.Year, stmonth, pay.CutOffStartDay);
            //var End = new DateTime(date.Year, edmonth, pay.CutOffEndDay);


            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("Status", StatusEnum.Active);
            //prms.AddIfNotExists("CompanyId", CompanyId);
            //prms.AddIfNotExists("ESD", DateTime.Now.ApplicationNow().Date);
            //prms.AddIfNotExists("payrollGroupId", payrollGroupId);
            //prms.AddIfNotExists("payrollRunId", payrollRunId);
            //prms.AddIfNotExists("startDate", Start);
            //prms.AddIfNotExists("endDate", End);


            var cypher = @"match(pg:PAY_PayrollGroup{ Id:{payrollGroupId},IsDeleted: 0,CompanyId: { CompanyId},Status:{Status} })<-[:R_SalaryInfo_PayrollGroup]-(ps:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId} })
                 match(ps)-[:R_SalaryInfoRoot]->(psr:PAY_SalaryInfoRoot)
                 match(psr)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
                 match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})

                 match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
        where a.EffectiveStartDate <= {ESD} <= a.EffectiveEndDate 

        match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot)
                 match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where o.EffectiveStartDate <= {ESD} <= o.EffectiveEndDate

        match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
        match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where j.EffectiveStartDate <= {ESD} <= j.EffectiveEndDate	

        match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where  p.EffectiveStartDate <= {ESD} <= p.EffectiveEndDate
                 match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}}) 

                 optional match(u)<-[R_Attendance_User]-(a1:TAA_Attendance) where {startDate} <= a1.AttendanceDate <= {endDate} and a1.PayrollPostedStatus is not null
                 optional match(o)-[:R_Organization_Payroll_OrganizationRoot]->(por:HRS_OrganizationRoot)<-[:R_OrganizationRoot]-(po:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where po.EffectiveStartDate <= {ESD} <= po.EffectiveEndDate 
                 with distinct pr,u,o,orr,a1,po,ps,por

                 optional match(pr)<-[R_PayrollRun_PersonRoot]-(prr:PAY_PayrollRun{ Id:{payrollRunId},IsDeleted: 0,CompanyId: { CompanyId}})
                 with u,pr,prr,o,orr,a1,po,ps,por
                 where (o.IsPayrollOrganization=true or por.Id is not null)
                 and (ps.TakeAttendanceFromTAA=false or (ps.TakeAttendanceFromTAA=true and a1.Id is not null))
                 with distinct pr.Id as PersonId,case when o.IsPayrollOrganization=true then orr.Id else por.Id end as OrganizationId,case when o.IsPayrollOrganization=true then o.Name else po.Name end as OrganizationName,prr
                 return OrganizationId,OrganizationName,count(case when prr.Id is not null then 1 end) as NoOfEmpInPay,count(case when prr.Id is null then 1 end) as NoOfEmpNotInPay
                 ";
            var list = await _payrunrepo.ExecuteQueryList<PayrollBatchViewModel>(cypher, null);
            return list;
        }

        public async Task<List<PayrollBatchViewModel>> GetNotPostedPayrollEmployeeList(string payrollGroupId, string payrollRunId, string payrollId)
        {
            //var pay = _payrollGroupBusiness.GetSingleById(payrollGroupId);
            //var payroll = _payrollBusiness.GetSingleById(payrollId);
            //var date = payroll.PayrollStartDate.Value;
            //var stmonth = date.Month;
            //var edmonth = date.Month;
            //if (pay.IsCutOffStartDayPreviousMonth)
            //    stmonth = stmonth - 1;

            //var Start = new DateTime(date.Year, stmonth, pay.CutOffStartDay);
            //var End = new DateTime(date.Year, edmonth, pay.CutOffEndDay);


            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("Status", StatusEnum.Active);
            //prms.AddIfNotExists("CompanyId", CompanyId);
            //prms.AddIfNotExists("ESD", DateTime.Now.ApplicationNow().Date);
            //prms.AddIfNotExists("payrollGroupId", payrollGroupId);
            //prms.AddIfNotExists("payrollRunId", payrollRunId);
            //prms.AddIfNotExists("startDate", Start);
            //prms.AddIfNotExists("endDate", End);


            var cypher = @"match(pg:PAY_PayrollGroup{ Id:{payrollGroupId},IsDeleted: 0,CompanyId: { CompanyId},Status:{Status} })
                 <-[:R_SalaryInfo_PayrollGroup]-(ps:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId} })
                 match(ps)-[:R_SalaryInfoRoot]->(psr:PAY_SalaryInfoRoot)
                 match(psr)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
                 match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})

                 match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
        where a.EffectiveStartDate <= {ESD} <= a.EffectiveEndDate 

        match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot)
                 match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where o.EffectiveStartDate <= {ESD} <= o.EffectiveEndDate

        match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
        match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where j.EffectiveStartDate <= {ESD} <= j.EffectiveEndDate	

        match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where  p.EffectiveStartDate <= {ESD} <= p.EffectiveEndDate
                 match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}}) 

                 match(u)<-[R_Attendance_User]-(a1:TAA_Attendance) where {startDate} <= a1.AttendanceDate <= {endDate} and a1.PayrollPostedStatus is null and ps.TakeAttendanceFromTAA=true
                 optional match(o)-[:R_Organization_Payroll_OrganizationRoot]->(por:HRS_OrganizationRoot)<-[:R_OrganizationRoot]-(po:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where po.EffectiveStartDate <= {ESD} <= po.EffectiveEndDate 
                 with distinct pr,o,orr,po,por            

                 with pr,o,orr,po,por
                 where (o.IsPayrollOrganization=true or por.Id is not null)

                 with distinct pr.Id as PersonId,case when o.IsPayrollOrganization=true then orr.Id else por.Id end as OrganizationId,case when o.IsPayrollOrganization=true then o.Name else po.Name end as OrganizationName
                 return OrganizationId,OrganizationName,count(PersonId) as NoOfEmpNotInPay
                 ";
            var list = await _payrunrepo.ExecuteQueryList<PayrollBatchViewModel>(cypher, null);
            return list;
        }

        public async Task<List<PayrollBatchViewModel>> GetNotPostedPreviousPayrollEmployeeList(string payrollGroupId)
        {
            var date = DateTime.Now.ApplicationNow().Date;
            var Start = new DateTime(date.Year, date.Month, 1);

            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("Status", StatusEnum.Active);
            //prms.AddIfNotExists("CompanyId", CompanyId);
            //prms.AddIfNotExists("ESD", DateTime.Now.ApplicationNow().Date);
            //prms.AddIfNotExists("payrollGroupId", payrollGroupId);
            //prms.AddIfNotExists("Start", Start);

            var cypher = @"match(pg:PAY_PayrollGroup{ Id:{payrollGroupId},IsDeleted: 0,CompanyId: { CompanyId},Status:{Status} })<-[:R_SalaryInfo_PayrollGroup]-(ps:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: { CompanyId} })
                 match(ps)-[:R_SalaryInfoRoot]->(psr:PAY_SalaryInfoRoot)
                 match(psr)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
                 match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})

                 match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
        where a.EffectiveStartDate <= {ESD} <= a.EffectiveEndDate 

        match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot)
                 match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where o.EffectiveStartDate <= {ESD} <= o.EffectiveEndDate

        match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
        match(jr)<-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where j.EffectiveStartDate <= {ESD} <= j.EffectiveEndDate	

        match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where  p.EffectiveStartDate <= {ESD} <= p.EffectiveEndDate
                 match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}}) 

                 optional match(o)-[:R_Organization_Payroll_OrganizationRoot]->(por:HRS_OrganizationRoot)<-[:R_OrganizationRoot]-(po:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
                 where po.EffectiveStartDate <= {ESD} <= po.EffectiveEndDate 

                 with u,pr,o,orr,po,ps,por  

                 optional match(u)<-[R_Attendance_User]-(a1:TAA_Attendance) where a1.AttendanceDate<{Start} and a1.PayrollPostedStatus='Submitted' and ps.TakeAttendanceFromTAA=true
                 with distinct u,pr,o,orr,po,por,a1.PayrollPostedStatus as PayrollPostedStatus

                 optional match(pr)<-[:R_PayrollTransaction_PersonRoot]-(pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId}}) where pt.EffectiveDate<{Start} and pt.ProcessStatus='Draft'

                 with distinct pr,o,orr,po,por,count(case when PayrollPostedStatus='Submitted' then 1 end) as NoOfEmpNotInPay,pt.ProcessStatus as ProcessStatus 
                 where (o.IsPayrollOrganization=true or por.Id is not null)
                 with distinct pr.Id as PersonId,case when o.IsPayrollOrganization=true then orr.Id else por.Id end as OrganizationId,case when o.IsPayrollOrganization=true then o.Name else po.Name end as OrganizationName,NoOfEmpNotInPay,ProcessStatus
                 return OrganizationId,OrganizationName,NoOfEmpNotInPay,count(case when ProcessStatus='Draft' then 1 end) as NoOfEmpInTransaction
                 ";
            var list = await _payrunrepo.ExecuteQueryList<PayrollBatchViewModel>(cypher, null);
            return list;
        }
        public async Task<List<PayrollSummaryViewModel>> GetPayrollSummary(PayrollSummaryViewModel search)
        {
            //var param = new Dictionary<string, object>
            //     {
            //         { "Status", StatusEnum.Active },
            //         { "CompanyId", CompanyId },
            //         { "ESD", DateTime.Now.ApplicationNow().Date },
            //         {"YearMonth",search.YearMonth }
            //     };

            var cypher = string.Concat(@"
                 match(porr)<-[:R_OrganizationRoot]-(po: HRS_Organization{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
                 where po.IsPayrollOrganization=true and po.EffectiveStartDate <= { ESD} <= po.EffectiveEndDate 

                 optional match(porr)<-[:R_Organization_Payroll_OrganizationRoot]-(ppor: HRS_Organization{IsDeleted: 0 }) where ppor.EffectiveStartDate<={ESD}<= ppor.EffectiveEndDate 

                 WITH po,collect(porr.Id)+collect(ppor.RootId) as orgs unwind orgs as PayOrganizationIds
                 with distinct  PayOrganizationIds,po

                 match(orr)<-[:R_OrganizationRoot]-(o: HRS_Organization{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
                 where orr.Id = PayOrganizationIds and o.EffectiveStartDate <= { ESD} <= o.EffectiveEndDate

                 match(orr)<-[:R_Assignment_OrganizationRoot]-(a: HRS_Assignment{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
                 where a.EffectiveStartDate <= { ESD} <= a.EffectiveEndDate

                 match(a)-[:R_AssignmentRoot]->(ar: HRS_AssignmentRoot{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })

                 match(ar)-[:R_AssignmentRoot_PersonRoot]->(pr: HRS_PersonRoot{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })

                 match(pr)<-[:R_PersonRoot]-(p: HRS_Person{IsDeleted: 0,Status:{Status},CompanyId: { CompanyId} })
                 where p.EffectiveStartDate <= { ESD} <= p.EffectiveEndDate 

                 match(pr)<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot{IsDeleted: 0,CompanyId: { CompanyId} })
                 match(psr)<-[:R_SalaryInfoRoot]-(ps:PAY_SalaryInfo)

                 optional match(pr)<-[:R_PayrollTransaction_PersonRoot]-(pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId}}) where pt.ProcessStatus='Draft'          

                 with distinct pr,po,count(distinct(case when pt.Id is not null then pr.Id end)) as PendingTransaction

                 optional match(pr)< -[R_PayrollRun_PersonRoot]-(prr: PAY_PayrollRun{IsDeleted: 0,CompanyId: { CompanyId} })
                 where prr.YearMonth = {YearMonth}

        	with distinct pr,po,prr,PendingTransaction  

                 return  count(case when PendingTransaction<>0 then 1 end) as PendingEmpTransaction,count(case when prr.Id is not null then prr.Id end) as IncludedPersonsCount,count(case when prr.Id is null then 1 end) as ExcludedPersonsCount,count(pr.Id) as TotalPersonsCount, po.Name as PayrollOrganization,po.RootId as PayrollOrganizationId");

            var result = await _payrunrepo.ExecuteQueryList<PayrollSummaryViewModel>(cypher, null);


            //var param1 = new Dictionary<string, object>
            //     {
            //         { "Status", StatusEnum.Active },
            //         { "CompanyId", CompanyId },
            //         { "ESD", DateTime.Now.ApplicationNow().Date },
            //         {"YearMonth",search.YearMonth }
            //     };

            var cypher1 = string.Concat(@"
                 match(porr)<-[:R_OrganizationRoot]-(po: HRS_Organization{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
                 where po.IsPayrollOrganization=true and po.EffectiveStartDate <= { ESD} <= po.EffectiveEndDate 

                 optional match(porr)<-[:R_Organization_Payroll_OrganizationRoot]-(ppor: HRS_Organization{IsDeleted: 0 }) where ppor.EffectiveStartDate<={ESD}<= ppor.EffectiveEndDate 

                 WITH po,collect(porr.Id)+collect(ppor.RootId) as orgs unwind orgs as PayOrganizationIds
                 with distinct  PayOrganizationIds,po

                 match(orr)<-[:R_OrganizationRoot]-(o: HRS_Organization{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
                 where orr.Id = PayOrganizationIds and o.EffectiveStartDate <= { ESD} <= o.EffectiveEndDate

                 match(orr)<-[:R_Assignment_OrganizationRoot]-(a: HRS_Assignment{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
                 where a.EffectiveStartDate <= { ESD} <= a.EffectiveEndDate

                 match(a)-[:R_AssignmentRoot]->(ar: HRS_AssignmentRoot{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })

                 match(ar)-[:R_AssignmentRoot_PersonRoot]->(pr: HRS_PersonRoot{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })

                 match(pr) < -[:R_PersonRoot] - (p: HRS_Person{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
                 where p.EffectiveStartDate <= { ESD} <= p.EffectiveEndDate   

                 match(pr)<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot{IsDeleted: 0,CompanyId: { CompanyId} })
                 match(psr)<-[:R_SalaryInfoRoot]-(ps:PAY_SalaryInfo)

                 match(pr)<-[R_PayrollRun_PersonRoot]-(prr: PAY_PayrollRun{IsDeleted: 0,CompanyId: { CompanyId} }) 

                 with distinct pr,prr,po            

                 return prr.PayRollNo as PayrollNo,count(case when prr.YearMonth={YearMonth} then 1 end) as CurrentMonthCount,count(case when prr.YearMonth<>{YearMonth} then 1 end) as PreviousMonthCount, po.Name as PayrollOrganization,po.RootId as PayrollOrganizationId");

            var result1 = await _payrunrepo.ExecuteQueryList<PayrollSummaryViewModel>(cypher1, null);

            var distinctRun = await GetDistinctRun(search.YearMonth.Value);

            foreach (var item in result)
            {
                var payrollorg = result1.Where(x => x.PayrollOrganizationId == item.PayrollOrganizationId);

                foreach (var item3 in payrollorg)
                {
                    int i = 1;
                    var Col1 = string.Concat("PayrollNo", i);
                    var Col2 = string.Concat("CurrentMonthCount", i);
                    var Col3 = string.Concat("PreviousMonthCount", i);

                    foreach (var item1 in distinctRun)
                    {
                        if (item3.PayrollNo == item1)
                        {
                            item.SetPropertyValue(Col1, item3.PayrollNo);
                            item.SetPropertyValue(Col2, item3.CurrentMonthCount);
                            item.SetPropertyValue(Col3, item3.PreviousMonthCount);
                        }
                    }
                    i++;
                }
            }

            return result;
        }
        public async Task<string[]> GetDistinctRun(int yearMonth)
        {

            var match1 = string.Concat($@" Select distinct pay.""PayRollNo"" as PayRollNo
from cms.""N_PayrollHR_PayrollRun""  as pay where pay.""YearMonth""='{yearMonth}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
                       order by pay.""PayRollNo""");

            var result = await _payrunrepo.ExecuteScalarList<string>(match1, null);
            return result.ToArray();
        }


        public async Task<PayrollRunViewModel> GetPayrollRunByNoteId(string noteId)
        {

            var cypher = $@"Select * from 
cms.""N_PayrollHR_PayrollRun"" where ""NtsNoteId""='{noteId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'
";
            return await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
        }
        public async Task<PayrollRunViewModel> GetPayrollRunById(string Id)
        {

            var cypher = $@"Select * from 
cms.""N_PayrollHR_PayrollRun"" where ""Id""='{Id} and ""IsDeleted""=false' and ""CompanyId""='{_repo.UserContext.CompanyId}'
";
            return await _payrunrepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
        }

        public async Task<ServiceViewModel> GetPayrollRunService(string payrollRunId)
        {

            var cypher = $@"Select s.* from 
cms.""N_PayrollHR_PayrollRun"" as pr 
join public.""NtsNote"" as n on n.""Id""=pr.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as s on s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
where pr.""Id""='{payrollRunId}' and s.""ServiceStatusCode""<>'SERVICE_STATUS_CANCEL' and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            return await _payrunrepo.ExecuteQuerySingle<ServiceViewModel>(cypher, null);
        }

        private async Task<List<TicketAccrualViewModel>> GetTicketEligibleEmployeeDetails(DateTime payrollDate, string payRollRunId)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("EED", payrollDate);
            //prms.AddIfNotExists("payRollRunId", payRollRunId);

            //var cypher = @"match(pr)<-[:R_PersonRoot]-(per:HRS_Person{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where per.EffectiveStartDate <= {EED} <= per.EffectiveEndDate
            //         match(pr)<-[:R_PayrollRun_PersonRoot]-(payR:PAY_PayrollRun{Id:{payRollRunId}})
            //         optional match(per)-[:R_Person_Nationality]->(n:HRS_Nationality{ IsDeleted: 0,CompanyId: 1, Status: 'Active' })
            //         optional match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot)<-[:R_AssignmentRoot]-(ass: HRS_Assignment{IsDeleted: 0,CompanyId: 1, Status: 'Active' }) 
            //         where ass.EffectiveStartDate <= {EED} <= ass.EffectiveEndDate
            //         optional match(ass)-[:R_Assignment_GradeRoot]->(gr:HRS_GradeRoot{ IsDeleted:0,CompanyId:1})
            //         optional match(gr)<-[:R_GradeRoot]-(g:HRS_Grade{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where g.EffectiveStartDate <= {EED} <= g.EffectiveEndDate

            //        optional match(pr)<-[:R_SalaryInfoRoot_PersonRoot]-(salr:PAY_SalaryInfoRoot)
            //         optional match(salr)<-[:R_SalaryInfoRoot]-(sal{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where sal.EffectiveStartDate <= {EED} <= sal.EffectiveEndDate

            //         optional match(pr)<-[:R_DependentRoot_PersonRoot]-(dr:HRS_DependentRoot{IsDeleted: 0,CompanyId: 1, Status: 'Active'})

            //         optional match(:NTS_TemplateMaster{Code:'N_VISA_DEPENDENT'})<-[:R_TemplateRoot]-(:NTS_Template)<-[:R_Note_Template]-(nn:NTS_Note)-[:R_Note_Reference]->(dr)<-[:R_DependentRoot]-(d:HRS_Dependent{IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where d.EffectiveStartDate <= { EED} <= d.EffectiveEndDate and
            //          datetime({ EED}) <= datetime(coalesce(nn.ExpiryDate, datetime())) and(duration.inDays(datetime(d.DateOfBirth),
            //           datetime({ EED})).days * 1.0)/ 365 <= 2


            //         optional match(:NTS_TemplateMaster{ Code: 'N_VISA_DEPENDENT'})< -[:R_TemplateRoot] - (: NTS_Template) < -[:R_Note_Template] - (not1: NTS_Note) -[:R_Note_Reference]->(dr) < -[:R_DependentRoot] - (d1: HRS_Dependent{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where d1.EffectiveStartDate <= { EED} <= d1.EffectiveEndDate and datetime({ EED}) <= datetime(not1.ExpiryDate) and 2 < (duration.inDays(datetime(d1.DateOfBirth), datetime({ EED})).days * 1.0)/ 365 <= 12


            //         optional match(:NTS_TemplateMaster{ Code: 'N_VISA_DEPENDENT'})< -[:R_TemplateRoot] - (: NTS_Template) < -[:R_Note_Template] - (not2: NTS_Note) -[:R_Note_Reference]->(dr) < -[:R_DependentRoot] - (d2: HRS_Dependent{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //         where d2.EffectiveStartDate <= { EED} <= d2.EffectiveEndDate and datetime({ EED}) <= datetime(not2.ExpiryDate) and(duration.inDays(datetime(d2.DateOfBirth), datetime()).days * 1.0) / 365 > 12


            //      with per, n, g, sal, Count(d.Id) as InfantCount, Count(d1.Id) as KidsCount, Count(d2.Id) as AdultCount, pr, (datetime(coalesce(ass.DateOfJoin, {EED}) ) + duration({months:10})).month as anniversaryticket 

            //         return pr.Id as PersonId, per.FirstName, per.MiddleName, per.LastName,  n.Name, coalesce(n.AverageEconomyTicketCost, 0) as AverageEconomyTicketCost, 
            //     coalesce(n.AverageBusinessTicketCost, 0) as AverageBusinessTicketCost, g.Name as Grade, 
            //     coalesce(g.TravelClass, 'Economy') as TravelClass, coalesce(sal.IsEligibleForAirTicketForSelf, false) as IsEligibleForAirTicketForSelf, 
            //     coalesce(sal.IsEligibleForAirTicketForDependant, false) as IsEligibleForAirTicketForDependant,  InfantCount, KidsCount, AdultCount,  CASE WHEN anniversaryticket = datetime({EED}).month  THEN true 

            //     ELSE false END as IsEligibleForTicketClaim";
            var cypher = $@"select per.""Id"" as PersonId, per.""FirstName"", per.""MiddleName"", per.""LastName"",  n.""NationalityName"", coalesce(cast(n.""AverageEconomyTicketCost"" as integer), 0) as AverageEconomyTicketCost, 
                coalesce(cast(n.""AverageBusinessTicketCost"" as integer), 0) as AverageBusinessTicketCost, g.""GradeName"" as Grade, 
                 coalesce(g.""TravelClass"", 'Economy') as TravelClass, coalesce(cast(sal.""IsEmployeeEligibleForFlightTicketsForSelf"" as boolean), false) as IsEligibleForAirTicketForSelf, 
                 coalesce(cast(sal.""IsEmployeeEligibleForFlightTicketForDependants"" as boolean), false) as IsEmployeeEligibleForFlightTicketsForDependant,Count(d.""Id"") as  InfantCount,Count(d1.""Id"") as KidsCount,Count(d2.""Id"") as AdultCount  
,CASE WHEN anniversaryticket =  EXTRACT(MONTH from TIMESTAMP  '{payrollDate}')  THEN true  ELSE false END as IsEligibleForTicketClaim

from cms.""N_CoreHR_HRPerson"" as per
 join public.""User"" as u on per.""UserId""=u.""Id""
 join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=per.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRun"" as prr on prr.""Id""=prp.""PayrollRunId"" and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as ass on per.""Id""=ass.""EmployeeId"" and ass.""IsDeleted""=false and ass.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""N_CoreHR_HRNationality"" as n on n.""Id""=per.""NationalityId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=ass.""AssignmentGradeId"" and g.""IsDeleted""=false and g.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryInfo"" as sal on sal.""PersonId""=per.""Id"" and sal.""IsDeleted""=false and sal.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join public.""NtsNote"" as pn on pn.""Id""=sal.""NtsNoteId""  and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join public.""NtsNote"" as note on pn.""Id""=note.""ParentNoteId""  and note.""IsDeleted""=false and note.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join cms.""N_PayrollHR_SalaryElementInfo"" as ps on ps.""NtsNoteId""=note.""Id"" and sal.""IsDeleted""=false and sal.""CompanyId""='{_repo.UserContext.CompanyId}'
-- and ps.""EffectiveStartDate""::Date <= '{payrollDate}'::Date  and '{payrollDate}'::Date<= ps.""EffectiveEndDate""::Date
--left join cms.""N_PayrollHR_PayrollElement"" as e on sei.""ElementId""=e.""Id"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
--and  e.""EffectiveStartDate""::Date <= '{payrollDate}'::Date and  '{ payrollDate}'::Date <= e.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRDependant"" as dr n dr.""EmployeeId""=per.""Id"" and dr.""IsDeleted""=false and drr.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_DependentDocuments_VisaDependent"" as d on d.""DependentId""=dr.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
and -- d.""EffectiveStartDate""::Date <= '{ payrollDate}'::Date and  '{ payrollDate}'::Date<= d.""EffectiveEndDate""::Date and
'{ payrollDate}'::Date <= coalesce(Cast(d.""ExpireDate"" as DateTime),Now())::Date and DATE_PART('day','{ payrollDate}'::timestamp-dr.""DateOfBirth""::timestamp) * 1.0/ 365 <= 2
left join cms.""N_DependentDocuments_VisaDependent"" as d1 on d1.""DependentId""=dr.""Id""  and d1.""IsDeleted""=false and d1.""CompanyId""='{_repo.UserContext.CompanyId}'
--and  d1.""EffectiveStartDate"" <= '{ payrollDate}' and  '{ payrollDate}'<= d1.""EffectiveEndDate"" 
and '{ payrollDate}'::Date <= d1.""ExpireDate""::Date and 2 < DATE_PART('day', '{payrollDate}'::timestamp-dr.""DateOfBirth""::timestamp) * 1.0/ 365 and DATE_PART('day', '{payrollDate}'::timestamp-dr.""DateOfBirth""::timestamp) * 1.0/ 365<= 12
left join cms.""N_DependentDocuments_VisaDependent"" as d2 on d2.""DependentId""=dr.""Id"" and d2.""IsDeleted""=false and d2.""CompanyId""='{_repo.UserContext.CompanyId}'
--and  d2.""EffectiveStartDate"" <= '{ payrollDate}' and  '{ payrollDate}'<= d2.""EffectiveEndDate"" 
and '{ payrollDate}'::Date <= d2.""ExpireDate""::Date and  DATE_PART('day', '{payrollDate}'::timestamp-dr.""DateOfBirth""::timestamp) * 1.0/ 365 > 12
where prr.""Id""='{payRollRunId}' and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
group by per.""Id"",per.""FirstName"",per.""LastName"",n.""NationalityName"",n.""AverageEconomyTicketCost"",n.""AverageBusinessTicketCost"",g.""GradeName"",g.""TravelClass""
,sal.""IsEmployeeEligibleForFlightTicketsForSelf"",sal.""IsEmployeeEligibleForFlightTicketsForDependants"" ,d.""Id"",d2.""Id"",d1.""Id"",(coalesce(ass.""DateOfJoin"", '{payrollDate}') ) + (duration * '10 month'::INTERVAL) as anniversaryticket 


";


            var result = await _payrunrepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);
            return result;
        }

        private PayrollTransactionViewModel GetTicketTransationModel(double amount, DateTime effectiveDate, string elementCode, string personId, double? openingBal, double closingBal, double deductionAmt = 0, PayrollPostedSourceEnum postedSource = PayrollPostedSourceEnum.Payroll)
        {
            var model = new PayrollTransactionViewModel
            {
                OpeningBalance = openingBal.RoundPayrollAmount(),
                ClosingBalance = closingBal.RoundToTwoDecimal(),
                DeductionAmount = deductionAmt.RoundToTwoDecimal(),
                EarningAmount = amount.RoundToTwoDecimal(),
                Amount = (amount.RoundToTwoDecimal() - deductionAmt).RoundToTwoDecimal(),
                EffectiveDate = effectiveDate,
                ProcessStatus = PayrollProcessStatusEnum.NotProcessed,
                PostedSource = postedSource,
                PostedDate = effectiveDate,
                ElementCode = elementCode,
                PersonId = personId,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                CreatedBy = _repo.UserContext.UserId,
                LastUpdatedBy = _repo.UserContext.UserId,
                ReferenceNode = NodeEnum.HRS_PersonRoot,
                ReferenceId = personId,
            };
            return model;
        }

        private PayrollTransactionViewModel GetQuantityTransationModel(double amount, DateTime effectiveDate, string elementCode, string personId, double? openingBal, double closingBal)
        {
            var eq = amount;
            var dq = ((openingBal ?? 0) + amount.RoundToTwoDecimal()) - closingBal;
            var qty = eq - dq;
            var model = new PayrollTransactionViewModel
            {
                OpeningQuantity = openingBal.RoundPayrollAmount(),
                ClosingQuantity = closingBal.RoundToTwoDecimal(),
                Quantity = qty.RoundToTwoDecimal(),// amount.RoundPayrollAmount() - (Math.Abs(closingBal - ((openingBal ?? 0) + amount.RoundPayrollAmount())).RoundPayrollAmount()),
                EarningQuantity = eq.RoundToTwoDecimal(),// amount.RoundPayrollAmount(),
                DeductionQuantity = dq.RoundToTwoDecimal(),
                EffectiveDate = effectiveDate,
                ProcessStatus = PayrollProcessStatusEnum.NotProcessed,
                PostedSource = PayrollPostedSourceEnum.Payroll,
                PostedDate = effectiveDate,
                ElementCode = elementCode,
                PersonId = personId,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                CreatedBy = _repo.UserContext.UserId,
                LastUpdatedBy = _repo.UserContext.UserId,
                ReferenceNode = NodeEnum.HRS_PersonRoot,
                ReferenceId = personId,
            };
            return model;
        }

        public async Task UpdatePayrollRunStatus(string serviceId)
        {
            //var prms = new Dictionary<string, object>
            //     {
            //         { "status",PayrollStateEnum.ExecutePayroll },
            //         { "serviceId",serviceId },
            //     };
            var cypher = $@"Select pr.* from 
cms.""N_PayrollHR_PayrollRun"" as pr 
join public.""NtsNote"" as n on n.""Id""=pr.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as s on s.""Id""=n.""ParentServiceId""and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""Id""='{serviceId}'  and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            var payroll = await _payrunrepo.ExecuteQuerySingle(cypher, null);
            var cypher1 = $@"update cms.""N_PayrollHR_PayrollRun"" set ""PayrollStateEnd""='{PayrollStateEnum.ExecutePayroll}' where ""Id""='{payroll.Id}'";
            //var cypher = @"match (s:NTS_Service{Id:{serviceId}}-[:R_Service_Reference{ReferenceTypeCode:'PAY_PayrollRun'}]->(pr:PAY_PayrollRun) set pr.PayrollStateEnd={status},pr.PayrollStateEnd={status}";
            await _payrunrepo.ExecuteCommand(cypher1, null);
        }


        public async Task<CommandResult<PayrollRunViewModel>> EditAccrual(PayrollRunViewModel model)
        {
            var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = model.NoteId,
                // ActiveUserId = _repo.UserContext.UserId
            });
            var payrollRun = await GetPayrollRunByNoteId(model.NoteId);
            if (noteModel != null)
            {
                if (model.IsEOSAccrual)
                {
                    payrollRun.EOSAccrual = PayrollExecutionStatusEnum.Submitted;
                }
                if (model.IsFlightTicketAccrual)
                {
                    payrollRun.FlightTicketAccrual = PayrollExecutionStatusEnum.Submitted;
                }
                if (model.IsVacationAccrual)
                {
                    payrollRun.VacationAccrual = PayrollExecutionStatusEnum.Submitted;
                }
                if (model.IsSickLeaveAccrual)
                {
                    payrollRun.SickLeaveAccrual = PayrollExecutionStatusEnum.Submitted;
                }
                if (model.IsLoanAccrual)
                {
                    payrollRun.LoanAccrual = PayrollExecutionStatusEnum.Submitted;
                }
                model.LastUpdatedBy = _repo.UserContext.UserId;
                model.LastUpdatedDate = DateTime.Now;
                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(payrollRun);
                var result = await _noteBusiness.ManageNote(noteModel);
                // var result = _repository.Edit(payrollRun);
                //  var exe = await ExecutePayroll(payrollRun.Id);
                return CommandResult<PayrollRunViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PayrollRunViewModel>.Instance(model, false, "Payroll not found");
        }
        public async Task<double> GetEndOfService(EosTypeEnum eosType, string userId, DateTime asofDate)
        {
            //  throw new NotImplementedException();
            var eos = 0.0;
            var cypher = string.Concat($@"select u.""Name"" as UserName,pr.""Id"" as PersonId,pr.""PersonNo"" as PersonNo
                ,u.""Id"" as UserId,sum(coalesce(pssalr.""Amount""::int,0.0)) as TotalSalary, ass.""DateOfJoin"" as DateOfJoin
                 ,coalesce(sal.""UnpaidLeavesNotInSystem""::int, 0) as UnpaidLeavesNotInSystem
from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as pr on pr.""UserId""=u.""Id"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as ass on ass.""EmployeeId""=pr.""Id"" and ass.""IsDeleted""=false and ass.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as sal on sal.""PersonId""=pr.""Id"" and sal.""IsDeleted""=false and sal.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as pn on pn.""Id""=sal.""NtsNoteId""  and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on pn.""Id""=n.""ParentNoteId""  and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryElementInfo"" as pssalr on pssalr.""NtsNoteId""=n.""Id"" and pssalr.""IsDeleted""=false and pssalr.""CompanyId""='{_repo.UserContext.CompanyId}'
and pssalr.""EffectiveStartDate""::Date <= '{DateTime.Now}'::Date and '{DateTime.Now}'::Date <= pssalr.""EffectiveEndDate""::Date
join cms.""N_PayrollHR_PayrollElement"" as e on pssalr.""ElementId""=e.""Id"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
--and e.""EffectiveStartDate""::Date <= '{DateTime.Now}'::Date and '{DateTime.Now}'::Date <= e.""EffectiveEndDate""::Date 
and not e.""ElementCode"" in('GOSI_EMP_KSA','GOSI_COMP_KSA','GOSI_COMP_NON_KSA')
where u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
group by u.""Name"",pr.""Id"",pr.""PersonNo""
                ,u.""Id"", ass.""DateOfJoin"",sal.""UnpaidLeavesNotInSystem""
");
            //var cypher = @"match(u:ADM_User{Id:{Id},IsDeleted: 0})-[:R_User_PersonRoot]->(pr)
            //     <-[:R_PersonRoot]-(per:HRS_Person{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //     where per.EffectiveStartDate <= {EED} <= per.EffectiveEndDate
            //     match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot)<-[:R_AssignmentRoot]
            //     -(ass: HRS_Assignment{IsDeleted: 0,CompanyId: 1, Status: 'Active' }) 
            //     where ass.EffectiveStartDate <= {EED} <= ass.EffectiveEndDate
            //     match(pr)<-[:R_SalaryInfoRoot_PersonRoot]-(salr:PAY_SalaryInfoRoot)
            //     match(salr)<-[:R_SalaryInfoRoot]-(sal:PAY_SalaryInfo{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //     where sal.EffectiveStartDate <= {EED} <= sal.EffectiveEndDate
            //     match(salr)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(pssalr:PAY_SalaryElementInfo{ IsDeleted: 0 })
            //     where pssalr.EffectiveStartDate <= {EED} <= pssalr.EffectiveEndDate
            //     match(pssalr)-[:R_SalaryElementInfo_ElementRoot]->(pepssalr:PAY_ElementRoot{ IsDeleted: 0 })
            //     <-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: 1, Status: 'Active'}) 
            //     where e.EffectiveStartDate <= {EED} <= e.EffectiveEndDate 
            //     and not e.Code in['GOSI_EMP_KSA','GOSI_COMP_KSA','GOSI_COMP_NON_KSA']
            //     return u.UserName as UserName,pr.Id as PersonId,pr.PersonNo as PersonNo
            //     ,u.Id as UserId,sum(coalesce(pssalr.Amount,0.0)) as TotalSalary, ass.DateOfJoin as DateOfJoin
            //     ,coalesce(sal.UnpaidLeavesNotInSystem, 0) as UnpaidLeavesNotInSystem";

            var result = await _paysalelerepo.ExecuteQuerySingle<TicketAccrualViewModel>(cypher, null);
            if (result != null && result.DateOfJoin.HasValue)
            {
                var totalUnpaidLeaves = result.UnpaidLeavesNotInSystem;
                var unpaidLeaves = await _leaveBalanceBusiness.GetAllUnpaidLeaveDuration(userId, result.DateOfJoin.Value, asofDate);
                if (unpaidLeaves != null && unpaidLeaves.Count > 0)
                {
                    totalUnpaidLeaves += unpaidLeaves.Sum(x => x.DatedAllDuration.Value);

                }
                var totalDays = (asofDate.Date - result.DateOfJoin.Value.Date).TotalDays + 1 - totalUnpaidLeaves;
                var totalYears = (totalDays / 365.0);
                var eosDays = 0.0;
                var dailyEosLessthan5 = (result.TotalSalary / 730.0);
                var dailyEosGreaterThan5 = (result.TotalSalary / 365);
                if (totalDays <= 1825)
                {
                    eos = dailyEosLessthan5 * totalDays;
                }
                else
                {
                    eos = (dailyEosLessthan5 * 1825) + ((totalDays - 1825) * dailyEosGreaterThan5);
                }

                if (eosType == EosTypeEnum.Resignation)
                {
                    if (totalYears <= 2)
                    {
                        eos = 0;
                    }
                    else if (totalYears <= 5)
                    {
                        eos = eos / 3.0;
                    }
                    else if (totalYears <= 10)
                    {
                        eos = (eos / 3.0) * 2;
                    }
                }

            }
            return eos;
        }

        public async Task<double> GetActualWorkingdays(string calendarId, DateTime startDate, DateTime endDate)
        {


            var query = $@"select pc.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
                        where pc.""Id""='{calendarId}' and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var calendarVM = await _calRepo.ExecuteQuerySingle<CalendarViewModel>(query, null);

            var query1 = $@"select h.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
            join cms.""N_PayrollHR_CalendarHoliday"" as h on h.""CalendarId""=pc.""Id"" and pc.""Id""='{ calendarVM.Id}' and h.""IsDeleted""=false and h.""CompanyId""='{_repo.UserContext.CompanyId}'
              where pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'";


            var holidayVM = await _holidayrepo.ExecuteQueryList<CalendarHolidayViewModel>(query1, null);


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

        private async Task ManageAccruals(PayrollRunViewModel viewModel)
        {
            if (viewModel.EOSAccrual == PayrollExecutionStatusEnum.Submitted)
            {
                var prv = await GetSingleById(viewModel.Id);
                prv.EOSAccrual = PayrollExecutionStatusEnum.InProgress;
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    NoteId = prv.NoteId,
                    ActiveUserId = _repo.UserContext.UserId
                });
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                try
                {
                    // Log.Instance.Info("LoadEOSAccrualForCurrentMonth");
                    await LoadEOSAccrualForCurrentMonth(viewModel);
                    prv.EOSAccrual = PayrollExecutionStatusEnum.Completed;
                }
                catch (Exception)
                {
                    prv.EOSAccrual = PayrollExecutionStatusEnum.Error;
                }
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                //var noteModel1 = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                //{
                //    NoteId = prv.NoteId,
                //    ActiveUserId = _repo.UserContext.UserId
                //});
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                // _repository.Edit(prv);
            }
            if (viewModel.FlightTicketAccrual == PayrollExecutionStatusEnum.Submitted)
            {
                var prv = await GetSingleById(viewModel.Id);
                prv.FlightTicketAccrual = PayrollExecutionStatusEnum.InProgress;
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    NoteId = prv.NoteId,
                    ActiveUserId = _repo.UserContext.UserId
                });
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                // _repository.Edit(prv);
                try
                {
                    // Log.Instance.Info("LoadFlightTicketAccrualForCurrentMonth");
                    await LoadFlightTicketAccrualForCurrentMonth(viewModel);
                    prv.FlightTicketAccrual = PayrollExecutionStatusEnum.Completed;
                }
                catch (Exception)
                {
                    prv.FlightTicketAccrual = PayrollExecutionStatusEnum.Error;
                }
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                //var noteModel1 = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                //{
                //    NoteId = prv.NoteId,
                //    ActiveUserId = _repo.UserContext.UserId
                //});
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                // _repository.Edit(prv);
            }
            if (viewModel.VacationAccrual == PayrollExecutionStatusEnum.Submitted)
            {
                var prv = await GetSingleById(viewModel.Id);
                prv.VacationAccrual = PayrollExecutionStatusEnum.InProgress;
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                // _repository.Edit(prv);
                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    NoteId = prv.NoteId,
                    ActiveUserId = _repo.UserContext.UserId
                });
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                try
                {
                    //  Log.Instance.Info("LoadVacationAccrualForCurrentMonth");
                    await _leaveBalanceBusiness.ManageAnnualLeaveAccrual(viewModel.PayrollStartDate, viewModel.PayrollEndDate, viewModel.AttendanceEndDate);
                    await LoadVacationAccrualForCurrentMonth(viewModel);
                    prv.VacationAccrual = PayrollExecutionStatusEnum.Completed;
                }
                catch (Exception)
                {
                    prv.VacationAccrual = PayrollExecutionStatusEnum.Error;
                }
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                // _repository.Edit(prv);
            }

            if (viewModel.SickLeaveAccrual == PayrollExecutionStatusEnum.Submitted)
            {
                var prv = await GetSingleById(viewModel.Id);
                prv.SickLeaveAccrual = PayrollExecutionStatusEnum.InProgress;
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                //  _repository.Edit(prv);
                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    NoteId = prv.NoteId,
                    ActiveUserId = _repo.UserContext.UserId
                });
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                try
                {
                    // Log.Instance.Info("LoadSickLeaveAccrualForCurrentMonth");
                    await LoadSickLeaveAccrualForCurrentMonth(viewModel);
                    prv.SickLeaveAccrual = PayrollExecutionStatusEnum.Completed;
                }
                catch (Exception)
                {
                    prv.SickLeaveAccrual = PayrollExecutionStatusEnum.Error;
                }
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                // _repository.Edit(prv);
            }
            if (viewModel.LoanAccrual == PayrollExecutionStatusEnum.Submitted)
            {
                var prv = await GetSingleById(viewModel.Id);
                prv.LoanAccrual = PayrollExecutionStatusEnum.InProgress;
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                // _repository.Edit(prv);
                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    NoteId = prv.NoteId,
                    ActiveUserId = _repo.UserContext.UserId
                });
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                try
                {
                    // Log.Instance.Info("LoadLoanAccrualForCurrentMonth");
                    await LoadLoanAccrualForCurrentMonth(viewModel);
                    prv.LoanAccrual = PayrollExecutionStatusEnum.Completed;
                }
                catch (Exception)
                {
                    prv.LoanAccrual = PayrollExecutionStatusEnum.Error;
                }
                prv.LastUpdatedBy = _userContext.UserId;
                prv.LastUpdatedDate = DateTime.Now;
                if (noteModel != null)
                {
                    noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(prv);
                    var result = await _noteBusiness.ManageNote(noteModel);
                }
                //  _repository.Edit(prv);
            }

        }


        private async Task<string> LoadEOSAccrualForCurrentMonth(PayrollRunViewModel viewModel)
        {
            // Log.Instance.Info(DelimeterEnum.Space, "Start LoadEOSAccrualForCurrentMonth");
            var output = "success";

            var eosDetails = await this.GetEOSAccrualDetails(viewModel.AttendanceEndDate, viewModel.Id, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate);
            // Log.Instance.Info(DelimeterEnum.Space, "eosDetails Count:", eosDetails.Count);
            var accrualTransactions = await _payrollTransactionBusiness.GetEosAccrualTransactionTransactions(viewModel.PayrollEndDate.AddDays(-50), viewModel.PayrollEndDate);
            if (eosDetails.Count > 0)
            {

                var transactionList = new List<PayrollTransactionViewModel>();

                foreach (var item in eosDetails)
                {
                    var closedtransaction = await ManageClosedTransaction(item.PersonId, "MONTHLY_EOS_ACCRUAL", viewModel.AttendanceEndDate);
                    if (!closedtransaction)
                    {
                        continue;
                    }

                    var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_EOS_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());

                    if (existingTransactions == null)
                    {
                        var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_EOS_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                        var ob = pastExistingTransactions?.ClosingBalance ?? 0;
                        var cb = await GetEndOfService(EosTypeEnum.Termination, item.UserId, viewModel.AttendanceEndDate);
                        var amt = (cb - ob).RoundToTwoDecimal();

                        var transModel = GetTicketTransationModel(amt, viewModel.AttendanceEndDate, "MONTHLY_EOS_ACCRUAL", item.PersonId, ob, cb, 0, PayrollPostedSourceEnum.Accrual);
                        // transModel.Operation = DataOperation.Create;
                        // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                        var element = await GetElementDetails(transModel.ElementCode);

                        if (element.IsNotNull())
                        {
                            transModel.ElementId = element.Id;
                            // transModel.ElementType =element["Id"].ToSafeInt();
                            transModel.ElementType = element.ElementType;
                            transModel.ElementClassification = element.ElementClassification;
                            transModel.ElementCategory = element.ElementCategory;


                        }
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Create
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transModel);
                        var result = await _noteBusiness.ManageNote(noteModel);
                        // transactionList.Add(transModel);
                    }
                    else
                    {
                        var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_EOS_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                        double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                        var cb = await GetEndOfService(EosTypeEnum.Termination, item.UserId, viewModel.AttendanceEndDate);
                        var amt = (cb - ob).RoundToTwoDecimal();
                        existingTransactions.Operation = DataOperation.Correct;
                        existingTransactions.Amount = amt.RoundToTwoDecimal();
                        existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                        existingTransactions.PostedDate = DateTime.Now;
                        existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                        existingTransactions.OpeningBalance = ob.RoundToTwoDecimal();
                        existingTransactions.ClosingBalance = cb.RoundToTwoDecimal();
                        existingTransactions.EarningAmount = amt.RoundToTwoDecimal();
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            NoteId = existingTransactions.NtsNoteId,
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Edit
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                        var result = await _noteBusiness.ManageNote(noteModel);
                        // transactionList.Add(existingTransactions);
                    }


                }
                //  Log.Instance.Info(DelimeterEnum.Space, "ticket accrual transactionList count: ", transactionList.Count);

                //var bulkInsertList = transactionList.Where(x => x.Operation == DataOperation.Create).ToList();
                //  await UpdateAccuralsElementDetail(bulkInsertList);
                //  Log.Instance.Info(DelimeterEnum.Space, "bulkInsertList count: ", bulkInsertList.Count);
                // await _payrollTransactionBusiness.BulkInsert(bulkInsertList, false);
                // var bulkUpdatetList = transactionList.Where(x => x.Operation == DataActionEnum.Edit).ToList();

                // Log.Instance.Info(DelimeterEnum.Space, "bulkUpdatetList count: ", bulkUpdatetList.Count);
                // await _payrollTransactionBusiness.BulkUpdate(bulkUpdatetList, false);


            }
            // Log.Instance.Info(DelimeterEnum.Space, "End LoadTicketAccrualForCurrentMonth");

            return output;
        }

        private async Task<string> LoadFlightTicketAccrualForCurrentMonth(PayrollRunViewModel viewModel)
        {
            // Log.Instance.Info(DelimeterEnum.Space, "Start LoadTicketAccrualForCurrentMonth");
            var output = "success";
            // try
            // {
            var ticketDetails = await this.GetTicketAccrualDetails(viewModel.PayrollEndDate, viewModel.Id);
            //Log.Instance.Info(DelimeterEnum.Space, "ticketDetails Count:", ticketDetails.Count);
            var accrualTransactions = await _payrollTransactionBusiness.GetAccrualTransactionTransactions(viewModel.PayrollEndDate.AddDays(-50), viewModel.PayrollEndDate);
            if (ticketDetails.Count > 0)
            {
                var transactionList = new List<PayrollTransactionViewModel>();

                foreach (var item in ticketDetails)
                {
                    var closedtran = await ManageClosedTransaction(item.PersonId, "MONTHLY_SELF_TICKET_ACCRUAL", viewModel.AttendanceEndDate);
                    if (item.IsEligibleForAirTicketForSelf && closedtran)
                    {
                        var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_SELF_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                        var existingYearlyTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "ANNUAL_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());


                        if (existingTransactions == null)
                        {
                            var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_SELF_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                            var ticketCost = item.TravelClass == TravelClassEnum.Business ? item.AverageBusinessTicketCost : item.AverageEconomyTicketCost;

                            double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                            double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, ticketCost);
                            var da = existingYearlyTransactions?.Amount ?? 0;
                            double cb = (ob + ea) - da;
                            var transModel = GetTicketTransationModel(ea.RoundToTwoDecimal(), viewModel.AttendanceEndDate, "MONTHLY_SELF_TICKET_ACCRUAL", item.PersonId, ob.RoundToTwoDecimal(), cb.RoundToTwoDecimal(), da.RoundToTwoDecimal(), PayrollPostedSourceEnum.Accrual);
                            // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                            var element = await GetElementDetails(transModel.ElementCode);

                            if (element.IsNotNull())
                            {
                                transModel.ElementId = element.Id;
                                // transModel.ElementType =element["Id"].ToSafeInt();
                                transModel.ElementType = element.ElementType;
                                transModel.ElementClassification = element.ElementClassification;
                                transModel.ElementCategory = element.ElementCategory;


                            }
                            var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                            {
                                TemplateCode = "PayrollTransaction",
                                ActiveUserId = _repo.UserContext.UserId,
                                DataAction = DataActionEnum.Create
                            });

                            noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transModel);
                            var result = await _noteBusiness.ManageNote(noteModel);
                            //  transModel.Operation = DataOperation.Create;
                            //  transactionList.Add(transModel);
                        }
                        else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                        {
                            var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_SELF_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                            var ticketCost = item.TravelClass == TravelClassEnum.Business ? item.AverageBusinessTicketCost : item.AverageEconomyTicketCost;
                            //var yealyTicketEarning = existingYearlyTransactions != null && existingYearlyTransactions.Any() ? existingYearlyTransactions.FirstOrDefault().Amount : 0;
                            double ob = pastExistingTransactions?.ClosingBalance.RoundPayrollAmount() ?? 0;
                            double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, ticketCost);
                            var da = existingYearlyTransactions?.Amount.RoundToTwoDecimal() ?? 0;
                            double cb = ((ob + ea) - da).RoundToTwoDecimal();
                            var am = (ea - da).RoundToTwoDecimal();
                            // var transModel = GetTicketTransationModel(amount.RoundPayrollAmount(), payrollDate, "MONTHLY_SELF_TICKET_ACCRUAL", item.PersonId, openingBalance, closingBalance, yealyTicketEarning);
                            // transModel.Operation = DataOperation.Create;
                            existingTransactions.Amount = am;
                            existingTransactions.EarningAmount = ea;
                            existingTransactions.OpeningBalance = ob;
                            existingTransactions.ClosingBalance = cb;
                            existingTransactions.DeductionAmount = da;
                            existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                            existingTransactions.PostedDate = DateTime.Now;
                            existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                            var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                            {
                                TemplateCode = "PayrollTransaction",
                                NoteId = existingTransactions.NtsNoteId,
                                ActiveUserId = _repo.UserContext.UserId,
                                DataAction = DataActionEnum.Edit
                            });

                            noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                            var result = await _noteBusiness.ManageNote(noteModel);
                            // existingTransactions.Operation = DataOperation.Correct;
                            //transactionList.Add(existingTransactions);
                        }
                    }

                    if (item.IsEligibleForAirTicketForDependant)
                    {
                        var ticketCost = item.TravelClass == TravelClassEnum.Business ? item.AverageBusinessTicketCost : item.AverageEconomyTicketCost;
                        var infantAmount = (item.InfantCount * ticketCost * 0.10);
                        var adultAmount = (item.AdultCount * ticketCost);
                        var wifeAmount = (item.WifeCount * ticketCost);
                        var husbandAmount = (item.HusbandCount * ticketCost);
                        var kidAmount = (item.KidsCount * ticketCost * 0.75);
                        // double amount = infantAmount + adultAmount + kidAmount;
                        var closrtran = await ManageClosedTransaction(item.PersonId, "MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL", viewModel.AttendanceEndDate);
                        if (infantAmount != 0 && closrtran)
                        {
                            var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                            var existingYearlyTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "ANNUAL_INFANT_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                            if (existingTransactions == null)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());

                                //var yealyTicketEarning = existingDepYearlyTransactions != null && existingDepYearlyTransactions.Any() ? existingDepYearlyTransactions.FirstOrDefault().Amount : 0;
                                //double infantOpeningBalance = pastInfantExistingTransactions1?.SingleOrDefault()?.ClosingBalance ?? 0;
                                //double infantClosingBalance = infantOpeningBalance + infantAmount - yealyTicketEarning;

                                //var transModel = GetTicketTransationModel(infantAmount.RoundPayrollAmount(), payrollDate, "MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL", item.PersonId, infantOpeningBalance, infantClosingBalance, yealyTicketEarning);


                                double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, infantAmount);
                                var da = existingYearlyTransactions?.Amount ?? 0;
                                double cb = (ob + ea) - da;
                                var transModel = GetTicketTransationModel(ea.RoundToTwoDecimal(), viewModel.AttendanceEndDate, "MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL", item.PersonId, ob.RoundToTwoDecimal(), cb.RoundToTwoDecimal(), da.RoundToTwoDecimal(), PayrollPostedSourceEnum.Accrual);

                                // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                                var element = await GetElementDetails(transModel.ElementCode);

                                if (element.IsNotNull())
                                {
                                    transModel.ElementId = element.Id;
                                    // transModel.ElementType =element["Id"].ToSafeInt();
                                    transModel.ElementType = element.ElementType;
                                    transModel.ElementClassification = element.ElementClassification;
                                    transModel.ElementCategory = element.ElementCategory;


                                }
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Create
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transModel);
                                var result = await _noteBusiness.ManageNote(noteModel);

                                //transModel.Operation = DataOperation.Create;
                                //  transactionList.Add(transModel);
                            }
                            else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                                double ob = pastExistingTransactions?.ClosingBalance.RoundPayrollAmount() ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, infantAmount);
                                var da = existingYearlyTransactions?.Amount.RoundToTwoDecimal() ?? 0;
                                double cb = ((ob + ea) - da).RoundToTwoDecimal();
                                var am = (ea - da).RoundToTwoDecimal();

                                existingTransactions.Operation = DataOperation.Correct;
                                existingTransactions.Amount = am;
                                existingTransactions.EarningAmount = ea;
                                existingTransactions.OpeningBalance = ob;
                                existingTransactions.ClosingBalance = cb;
                                existingTransactions.DeductionAmount = da;
                                existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                                existingTransactions.PostedDate = DateTime.Now;
                                existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                                // transactionList.Add(existingTransactions);
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    NoteId = existingTransactions.NtsNoteId,
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Edit
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                                var result = await _noteBusiness.ManageNote(noteModel);


                            }
                        }
                        var closedtrs = await ManageClosedTransaction(item.PersonId, "MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL", viewModel.AttendanceEndDate);
                        if (kidAmount != 0 && closedtrs)
                        {
                            var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                            var existingYearlyTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "ANNUAL_CHILD_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                            if (existingTransactions == null)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());

                                double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, kidAmount);
                                var da = existingYearlyTransactions?.Amount ?? 0;
                                double cb = (ob + ea) - da;
                                var transModel = GetTicketTransationModel(ea.RoundToTwoDecimal(), viewModel.AttendanceEndDate, "MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL", item.PersonId, ob.RoundToTwoDecimal(), cb.RoundToTwoDecimal(), da.RoundToTwoDecimal(), PayrollPostedSourceEnum.Accrual);
                                // transModel.Operation = DataOperation.Create;
                                //transactionList.Add(transModel);
                                // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                                var element = await GetElementDetails(transModel.ElementCode);

                                if (element.IsNotNull())
                                {
                                    transModel.ElementId = element.Id;
                                    // transModel.ElementType =element["Id"].ToSafeInt();
                                    transModel.ElementType = element.ElementType;
                                    transModel.ElementClassification = element.ElementClassification;
                                    transModel.ElementCategory = element.ElementCategory;


                                }
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Create
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transModel);
                                var result = await _noteBusiness.ManageNote(noteModel);
                            }
                            else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                                double ob = pastExistingTransactions?.ClosingBalance.RoundPayrollAmount() ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, kidAmount);
                                var da = existingYearlyTransactions?.Amount.RoundToTwoDecimal() ?? 0;
                                double cb = ((ob + ea) - da).RoundToTwoDecimal();
                                var am = (ea - da).RoundToTwoDecimal();
                                existingTransactions.Operation = DataOperation.Correct;
                                existingTransactions.Amount = am;
                                existingTransactions.EarningAmount = ea;
                                existingTransactions.OpeningBalance = ob;
                                existingTransactions.ClosingBalance = cb;
                                existingTransactions.DeductionAmount = da;
                                existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                                existingTransactions.PostedDate = DateTime.Now;
                                existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    NoteId = existingTransactions.NtsNoteId,
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Edit
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                                var result = await _noteBusiness.ManageNote(noteModel);
                                // transactionList.Add(existingTransactions);
                            }
                        }
                        var closed = await ManageClosedTransaction(item.PersonId, "MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL", viewModel.AttendanceEndDate);
                        if (adultAmount != 0 && closed)
                        {
                            var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                            var existingYearlyTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "ANNUAL_DEPENDENT_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());

                            if (existingTransactions == null)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());

                                double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, adultAmount);
                                var da = existingYearlyTransactions?.Amount ?? 0;
                                double cb = (ob + ea) - da;
                                var transModel = GetTicketTransationModel(ea.RoundToTwoDecimal(), viewModel.AttendanceEndDate, "MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL", item.PersonId, ob.RoundToTwoDecimal(), cb.RoundToTwoDecimal(), da.RoundToTwoDecimal(), PayrollPostedSourceEnum.Accrual);
                                // transModel.Operation = DataOperation.Create;
                                // transactionList.Add(transModel);
                                // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                                var element = await GetElementDetails(transModel.ElementCode);

                                if (element.IsNotNull())
                                {
                                    transModel.ElementId = element.Id;
                                    // transModel.ElementType =element["Id"].ToSafeInt();
                                    transModel.ElementType = element.ElementType;
                                    transModel.ElementClassification = element.ElementClassification;
                                    transModel.ElementCategory = element.ElementCategory;


                                }
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Create
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transModel);
                                var result = await _noteBusiness.ManageNote(noteModel);
                            }
                            else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                                double ob = pastExistingTransactions?.ClosingBalance.RoundPayrollAmount() ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, adultAmount);
                                var da = existingYearlyTransactions?.Amount.RoundToTwoDecimal() ?? 0;
                                double cb = ((ob + ea) - da).RoundToTwoDecimal();
                                var am = (ea - da).RoundToTwoDecimal();
                                existingTransactions.Operation = DataOperation.Correct;
                                existingTransactions.Amount = am;
                                existingTransactions.EarningAmount = ea;
                                existingTransactions.OpeningBalance = ob;
                                existingTransactions.ClosingBalance = cb;
                                existingTransactions.DeductionAmount = da;
                                existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                                existingTransactions.PostedDate = DateTime.Now;
                                existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    NoteId = existingTransactions.NtsNoteId,
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Edit
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                                var result = await _noteBusiness.ManageNote(noteModel);
                                //transactionList.Add(existingTransactions);
                            }
                        }
                        var closedtr = await ManageClosedTransaction(item.PersonId, "MONTHLY_WIFE_TICKET_ACCRUAL", viewModel.AttendanceEndDate);
                        if (wifeAmount != 0 && closedtr)
                        {
                            var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_WIFE_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                            var existingYearlyTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "ANNUAL_WIFE_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());

                            if (existingTransactions == null)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_WIFE_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());

                                double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, wifeAmount);
                                var da = existingYearlyTransactions?.Amount ?? 0;
                                double cb = (ob + ea) - da;
                                var transModel = GetTicketTransationModel(ea.RoundToTwoDecimal(), viewModel.AttendanceEndDate, "MONTHLY_WIFE_TICKET_ACCRUAL", item.PersonId, ob.RoundToTwoDecimal(), cb.RoundToTwoDecimal(), da.RoundToTwoDecimal(), PayrollPostedSourceEnum.Accrual);
                                //  transModel.Operation = DataOperation.Create;
                                // transactionList.Add(transModel);
                                // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                                var element = await GetElementDetails(transModel.ElementCode);

                                if (element.IsNotNull())
                                {
                                    transModel.ElementId = element.Id;
                                    // transModel.ElementType =element["Id"].ToSafeInt();
                                    transModel.ElementType = element.ElementType;
                                    transModel.ElementClassification = element.ElementClassification;
                                    transModel.ElementCategory = element.ElementCategory;


                                }
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Create
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transModel);
                                var result = await _noteBusiness.ManageNote(noteModel);
                            }
                            else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_WIFE_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                                double ob = pastExistingTransactions?.ClosingBalance.RoundPayrollAmount() ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, wifeAmount);
                                var da = existingYearlyTransactions?.Amount.RoundToTwoDecimal() ?? 0;
                                double cb = ((ob + ea) - da).RoundToTwoDecimal();
                                var am = (ea - da).RoundToTwoDecimal();

                                existingTransactions.Operation = DataOperation.Correct;
                                existingTransactions.Amount = am;
                                existingTransactions.EarningAmount = ea;
                                existingTransactions.OpeningBalance = ob;
                                existingTransactions.ClosingBalance = cb;
                                existingTransactions.DeductionAmount = da;
                                existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                                existingTransactions.PostedDate = DateTime.Now;
                                existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    NoteId = existingTransactions.NtsNoteId,
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Edit
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                                var result = await _noteBusiness.ManageNote(noteModel);
                                //   transactionList.Add(existingTransactions);
                            }
                        }
                        var clostr = await ManageClosedTransaction(item.PersonId, "MONTHLY_HUSBAND_TICKET_ACCRUAL", viewModel.AttendanceEndDate);
                        if (husbandAmount != 0 && clostr)
                        {
                            var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_HUSBAND_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                            var existingYearlyTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "ANNUAL_HUSBAND_TICKET_EARNING" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());

                            if (existingTransactions == null)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_HUSBAND_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());

                                double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, husbandAmount);
                                var da = existingYearlyTransactions?.Amount ?? 0;
                                double cb = (ob + ea) - da;
                                var transModel = GetTicketTransationModel(ea.RoundToTwoDecimal(), viewModel.AttendanceEndDate, "MONTHLY_HUSBAND_TICKET_ACCRUAL", item.PersonId, ob.RoundToTwoDecimal(), cb.RoundToTwoDecimal(), da.RoundToTwoDecimal(), PayrollPostedSourceEnum.Accrual);
                                // transModel.Operation = DataOperation.Create;
                                // transactionList.Add(transModel);
                                var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", transModel.ElementCode);
                                if (element.IsNotNull())
                                {
                                    transModel.ElementId = Convert.ToString(element["Id"]);
                                    // transModel.ElementType =element["Id"].ToSafeInt();
                                    transModel.ElementType = (ElementTypeEnum)Enum.Parse(typeof(ElementTypeEnum), ((int)element["ElementType"]).ToString());
                                    transModel.ElementClassification = (ElementClassificationEnum)Enum.Parse(typeof(ElementClassificationEnum), ((int)element["ElementClassification"]).ToString());
                                    transModel.ElementCategory = (ElementCategoryEnum)Enum.Parse(typeof(ElementCategoryEnum), ((int)element["ElementType"]).ToString());


                                }
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Create
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transModel);
                                var result = await _noteBusiness.ManageNote(noteModel);
                            }
                            else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                            {
                                var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_HUSBAND_TICKET_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                                double ob = pastExistingTransactions?.ClosingBalance.RoundPayrollAmount() ?? 0;
                                double ea = await _leaveBalanceBusiness.GetTicketAccrualPerMonth(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate, husbandAmount);
                                var da = existingYearlyTransactions?.Amount.RoundToTwoDecimal() ?? 0;
                                double cb = ((ob + ea) - da).RoundToTwoDecimal();
                                var am = (ea - da).RoundToTwoDecimal();
                                existingTransactions.Operation = DataOperation.Correct;
                                existingTransactions.Amount = am;
                                existingTransactions.EarningAmount = ea;
                                existingTransactions.OpeningBalance = ob;
                                existingTransactions.ClosingBalance = cb;
                                existingTransactions.DeductionAmount = da;
                                existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                                existingTransactions.PostedDate = DateTime.Now;
                                existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                                var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                                {
                                    TemplateCode = "PayrollTransaction",
                                    NoteId = existingTransactions.NtsNoteId,
                                    ActiveUserId = _repo.UserContext.UserId,
                                    DataAction = DataActionEnum.Edit
                                });

                                noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                                var result = await _noteBusiness.ManageNote(noteModel);
                                // transactionList.Add(existingTransactions);
                            }
                        }
                    }

                }
                //Log.Instance.Info(DelimeterEnum.Space, "ticket accrual transactionList count: ", transactionList.Count);
                ////  UpdateElementDetail(transactionList);
                //var bulkInsertList = transactionList.Where(x => x.Operation == DataOperation.Create).ToList();
                //UpdateAccuralsElementDetail(bulkInsertList);
                //Log.Instance.Info(DelimeterEnum.Space, "bulkInsertList count: ", bulkInsertList.Count);
                //_payrollTransactionBusiness.BulkInsert(bulkInsertList, false);

                //var bulkUpdateList = transactionList.Where(x => x.Operation == DataOperation.Correct).ToList();
                //Log.Instance.Info(DelimeterEnum.Space, "bulkUpdateList count: ", bulkUpdateList.Count);
                //_payrollTransactionBusiness.BulkUpdate(bulkUpdateList, false);


            }
            // Log.Instance.Info(DelimeterEnum.Space, "End LoadTicketAccrualForCurrentMonth");

            return output;
        }

        public async Task<string> LoadVacationAccrualForCurrentMonth(PayrollRunViewModel viewModel)
        {
            //Log.Instance.Info(DelimeterEnum.Space, "Start LoadVacationAccrualForCurrentMonth");
            var output = "success";
            var salaryelement = _sp.GetService<IPayrollElementBusiness>();
            var vacationDetails = await this.GetVacationAccrualDetails(viewModel.PayrollEndDate, viewModel.Id);
            // Log.Instance.Info(DelimeterEnum.Space, "eosDetails Count:", vacationDetails.Count);
            var accrualTransactions = await _payrollTransactionBusiness.GetVacationAccrualTransactionTransactions(viewModel.PayrollEndDate.AddDays(-50), viewModel.PayrollEndDate);
            if (vacationDetails.Count > 0)
            {
                var transactionList = new List<PayrollTransactionViewModel>();

                foreach (var item in vacationDetails)
                {
                    var closedtrans = await ManageClosedTransaction(item.PersonId, "MONTHLY_VACATION_ACCRUAL", viewModel.AttendanceEndDate);
                    if (!closedtrans)
                    {
                        continue;
                    }
                    var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_VACATION_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());

                    if (existingTransactions == null)
                    {
                        var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_VACATION_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                        double oq = pastExistingTransactions?.ClosingQuantity ?? 0;
                        var eq = await _leaveBalanceBusiness.GetLeaveAccrualPerMonth(item.UserId, viewModel.PayrollStartDate, viewModel.PayrollEndDate);
                        var dq = await _leaveBalanceBusiness.GetAnnualLeaveDatedDurationForAccrual(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate);

                        var cq = (oq + eq) - dq;

                        var dailySalary = await salaryelement.GetUserOneDaySalary(item.UserId, viewModel.PayrollEndDate);
                        var ob = oq * dailySalary;
                        var ea = eq * dailySalary;
                        var da = dq * dailySalary;
                        var cb = cq * dailySalary;
                        var a = (eq - dq) * dailySalary;



                        var model = new PayrollTransactionViewModel
                        {
                            OpeningQuantity = oq.RoundToTwoDecimal(),
                            ClosingQuantity = cq.RoundToTwoDecimal(),
                            Quantity = (eq - dq).RoundToTwoDecimal(),
                            EarningQuantity = eq.RoundToTwoDecimal(),
                            DeductionQuantity = dq.RoundToTwoDecimal(),

                            OpeningBalance = ob.RoundToTwoDecimal(),
                            ClosingBalance = cb.RoundToTwoDecimal(),
                            Amount = a.RoundToTwoDecimal(),
                            EarningAmount = ea.RoundToTwoDecimal(),
                            DeductionAmount = da.RoundToTwoDecimal(),

                            EffectiveDate = viewModel.AttendanceEndDate,
                            ProcessStatus = PayrollProcessStatusEnum.NotProcessed,
                            PostedSource = PayrollPostedSourceEnum.Accrual,
                            PostedDate = DateTime.Now,
                            ElementCode = "MONTHLY_VACATION_ACCRUAL",
                            PersonId = item.PersonId,
                            ReferenceNode = NodeEnum.HRS_PersonRoot,
                            ReferenceId = item.PersonId,
                        };
                        // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                        var element = await GetElementDetails(model.ElementCode);

                        if (element.IsNotNull())
                        {
                            model.ElementId = element.Id;
                            // transModel.ElementType =element["Id"].ToSafeInt();
                            model.ElementType = element.ElementType;
                            model.ElementClassification = element.ElementClassification;
                            model.ElementCategory = element.ElementCategory;


                        }
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Create
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        var result = await _noteBusiness.ManageNote(noteModel);
                        transactionList.Add(model);
                    }
                    else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                    {
                        var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_VACATION_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                        double oq = pastExistingTransactions?.ClosingQuantity ?? 0;
                        var eq = await _leaveBalanceBusiness.GetLeaveAccrualPerMonth(item.UserId, viewModel.PayrollStartDate, viewModel.PayrollEndDate);
                        var dq = await _leaveBalanceBusiness.GetAnnualLeaveDatedDurationForAccrual(item.UserId, viewModel.AttendanceStartDate, viewModel.AttendanceEndDate);

                        var cq = (oq + eq) - dq;

                        var dailySalary = await salaryelement.GetUserOneDaySalary(item.UserId, viewModel.PayrollEndDate);
                        var ob = oq * dailySalary;
                        var ea = eq * dailySalary;
                        var da = dq * dailySalary;
                        var cb = cq * dailySalary;
                        var a = (eq - dq) * dailySalary;

                        existingTransactions.OpeningQuantity = oq.RoundToTwoDecimal();
                        existingTransactions.EarningQuantity = eq.RoundToTwoDecimal();
                        existingTransactions.DeductionQuantity = dq.RoundToTwoDecimal();
                        existingTransactions.Quantity = (eq - dq).RoundToTwoDecimal();
                        existingTransactions.ClosingQuantity = cq.RoundToTwoDecimal();
                        existingTransactions.OpeningBalance = ob.RoundToTwoDecimal();
                        existingTransactions.ClosingBalance = cb.RoundToTwoDecimal();
                        existingTransactions.Amount = a.RoundToTwoDecimal();
                        existingTransactions.EarningAmount = ea.RoundToTwoDecimal();
                        existingTransactions.DeductionAmount = da.RoundToTwoDecimal();

                        // existingTransactions.Operation = DataOperation.Correct;
                        existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                        existingTransactions.PostedDate = DateTime.Now;
                        existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            NoteId = existingTransactions.NtsNoteId,
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Edit
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                        var result = await _noteBusiness.ManageNote(noteModel);
                        transactionList.Add(existingTransactions);
                    }

                }
                //  Log.Instance.Info(DelimeterEnum.Space, "ticket accrual transactionList count: ", transactionList.Count);
                //  var bulkInsertList = transactionList.Where(x => x.Operation == DataOperation.Create).ToList();
                // UpdateAccuralsElementDetail(bulkInsertList);
                // Log.Instance.Info(DelimeterEnum.Space, "bulkInsertList count: ", bulkInsertList.Count);
                // _payrollTransactionBusiness.BulkInsert(bulkInsertList, false);

                //  var bulkUpdteList = transactionList.Where(x => x.Operation == DataOperation.Correct).ToList();
                //  Log.Instance.Info(DelimeterEnum.Space, "bulkUpdteList count: ", bulkUpdteList.Count);
                //  _payrollTransactionBusiness.BulkUpdate(bulkUpdteList, false);


            }
            //   Log.Instance.Info(DelimeterEnum.Space, "End LoadVacationAccrualForCurrentMonth");

            return output;
        }

        private async Task<string> LoadSickLeaveAccrualForCurrentMonth(PayrollRunViewModel viewModel)
        {
            // Log.Instance.Info(DelimeterEnum.Space, "Start LoadSickLeaveAccrualForCurrentMonth");
            var output = "success";
            var salaryelement = _sp.GetService<IPayrollElementBusiness>();
            var vacationDetails = await this.GetSickLeaveAccrualPersonList(viewModel.PayrollEndDate, viewModel.Id);
            var accrualTransactions = await _payrollTransactionBusiness.GetSickLeaveAccrualTransactions(viewModel.PayrollEndDate.AddDays(-50), viewModel.PayrollEndDate);
            if (vacationDetails.Count > 0)
            {
                var transactionList = new List<PayrollTransactionViewModel>();

                foreach (var item in vacationDetails)
                {
                    var closedtran = await ManageClosedTransaction(item.PersonId, "MONTHLY_SICK_LEAVE_ACCRUAL", viewModel.AttendanceEndDate);
                    if (!closedtran)
                    {
                        continue;
                    }
                    var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_SICK_LEAVE_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());

                    if (existingTransactions == null)
                    {
                        var oq = 0.0;
                        var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_SICK_LEAVE_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                        if (pastExistingTransactions == null)
                        {
                            var sickLeave = await _leaveBalanceBusiness.GetSickLeaveBalance(item.UserId, viewModel.AttendanceStartDate.AddDays(-1));
                            oq = (sickLeave.DatedDuration ?? 0) + (sickLeave.HalfDayDatedDuration ?? 0) + (sickLeave.ThreeFourthDatedDuration ?? 0) + (sickLeave.NoPayDatedDuration ?? 0);
                        }
                        else
                        {
                            oq = pastExistingTransactions.ClosingQuantity ?? 0;
                        }
                        var sickLeave2 = await _leaveBalanceBusiness.GetSickLeaveBalance(item.UserId, viewModel.AttendanceEndDate);
                        var cq = (sickLeave2.DatedDuration ?? 0) + (sickLeave2.HalfDayDatedDuration ?? 0) + (sickLeave2.ThreeFourthDatedDuration ?? 0) + (sickLeave2.NoPayDatedDuration ?? 0);
                        // Log.Instance.Info(DelimeterEnum.Space, "oq", oq);
                        var eq = 0.0;
                        var dq = cq - oq;

                        var dailySalary = await salaryelement.GetUserOneDaySalary(item.UserId, viewModel.PayrollEndDate);
                        var ob = oq * dailySalary;
                        var ea = eq * dailySalary;
                        var da = dq * dailySalary;
                        var cb = cq * dailySalary;
                        var a = (eq - dq) * dailySalary;

                        // Log.Instance.Info(DelimeterEnum.Space, "oq", oq);
                        //  Log.Instance.Info(DelimeterEnum.Space, "cq", cq);

                        var model = new PayrollTransactionViewModel
                        {
                            OpeningQuantity = oq.RoundToTwoDecimal(),
                            ClosingQuantity = cq.RoundToTwoDecimal(),
                            Quantity = (eq - dq).RoundToTwoDecimal(),
                            EarningQuantity = eq.RoundToTwoDecimal(),
                            DeductionQuantity = dq.RoundToTwoDecimal(),

                            OpeningBalance = ob.RoundToTwoDecimal(),
                            ClosingBalance = cb.RoundToTwoDecimal(),
                            Amount = a.RoundToTwoDecimal(),
                            EarningAmount = ea.RoundToTwoDecimal(),
                            DeductionAmount = da.RoundToTwoDecimal(),

                            EffectiveDate = viewModel.AttendanceEndDate,
                            ProcessStatus = PayrollProcessStatusEnum.NotProcessed,
                            PostedSource = PayrollPostedSourceEnum.Accrual,
                            PostedDate = DateTime.Now,
                            ElementCode = "MONTHLY_SICK_LEAVE_ACCRUAL",
                            PersonId = item.PersonId,
                            //CreatedDate = DateTime.Now,
                            //LastUpdatedDate = DateTime.Now,
                            //CreatedBy = UserId,
                            //LastUpdatedBy = UserId,
                            ReferenceNode = NodeEnum.HRS_PersonRoot,
                            ReferenceId = item.PersonId,
                            // Operation = DataOperation.Create
                        };
                        // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                        var element = await GetElementDetails(model.ElementCode);

                        if (element.IsNotNull())
                        {
                            model.ElementId = element.Id;
                            // transModel.ElementType =element["Id"].ToSafeInt();
                            model.ElementType = element.ElementType;
                            model.ElementClassification = element.ElementClassification;
                            model.ElementCategory = element.ElementCategory;


                        }
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Create
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        var result = await _noteBusiness.ManageNote(noteModel);
                        // transactionList.Add(model);
                    }
                    else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                    {
                        var oq = 0.0;
                        var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_SICK_LEAVE_ACCRUAL" && x.PersonId == item.PersonId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                        if (pastExistingTransactions == null)
                        {
                            var sickLeave = await _leaveBalanceBusiness.GetSickLeaveBalance(item.UserId, viewModel.AttendanceStartDate.AddDays(-1));
                            oq = (sickLeave.DatedDuration ?? 0) + (sickLeave.HalfDayDatedDuration ?? 0) + (sickLeave.ThreeFourthDatedDuration ?? 0) + (sickLeave.NoPayDatedDuration ?? 0);
                        }
                        else
                        {
                            oq = pastExistingTransactions?.ClosingQuantity ?? 0;
                        }
                        var sickLeave2 = await _leaveBalanceBusiness.GetSickLeaveBalance(item.UserId, viewModel.AttendanceEndDate);
                        var cq = (sickLeave2.DatedDuration ?? 0) + (sickLeave2.HalfDayDatedDuration ?? 0) + (sickLeave2.ThreeFourthDatedDuration ?? 0) + (sickLeave2.NoPayDatedDuration ?? 0);

                        var eq = 0.0;
                        var dq = cq - oq;

                        var dailySalary = await salaryelement.GetUserOneDaySalary(item.UserId, viewModel.PayrollEndDate);
                        var ob = oq * dailySalary;
                        var ea = eq * dailySalary;
                        var da = dq * dailySalary;
                        var cb = cq * dailySalary;
                        var a = (eq - dq) * dailySalary;

                        existingTransactions.OpeningQuantity = oq.RoundToTwoDecimal();
                        existingTransactions.EarningQuantity = eq.RoundToTwoDecimal();
                        existingTransactions.DeductionQuantity = dq.RoundToTwoDecimal();
                        existingTransactions.Quantity = (eq - dq).RoundToTwoDecimal();
                        existingTransactions.ClosingQuantity = cq.RoundToTwoDecimal();

                        existingTransactions.OpeningBalance = ob.RoundToTwoDecimal();
                        existingTransactions.ClosingBalance = cb.RoundToTwoDecimal();
                        existingTransactions.Amount = a.RoundToTwoDecimal();
                        existingTransactions.EarningAmount = ea.RoundToTwoDecimal();
                        existingTransactions.DeductionAmount = da.RoundToTwoDecimal();

                        existingTransactions.Operation = DataOperation.Correct;
                        existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                        existingTransactions.PostedDate = DateTime.Now;
                        existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            NoteId = existingTransactions.NtsNoteId,
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Edit
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                        var result = await _noteBusiness.ManageNote(noteModel);
                        // transactionList.Add((PayrollTransactionViewModel)existingTransactions);
                    }

                }
                //Log.Instance.Info(DelimeterEnum.Space, "Sick leave accrual transactionList count: ", transactionList.Count);
                //var bulkInsertList = transactionList.Where(x => x.Operation == DataOperation.Create).ToList();
                //UpdateAccuralsElementDetail(bulkInsertList);
                //Log.Instance.Info(DelimeterEnum.Space, "bulkInsertList count: ", bulkInsertList.Count);
                //_payrollTransactionBusiness.BulkInsert(bulkInsertList, false);

                //var bulkUpdteList = transactionList.Where(x => x.Operation == DataOperation.Correct).ToList();
                //Log.Instance.Info(DelimeterEnum.Space, "bulkUpdteList count: ", bulkUpdteList.Count);
                //_payrollTransactionBusiness.BulkUpdate(bulkUpdteList, false);


            }
            //  Log.Instance.Info(DelimeterEnum.Space, "End LoadSickLeaveAccrualForCurrentMonth");

            return output;
        }

        private async Task<string> LoadLoanAccrualForCurrentMonth(PayrollRunViewModel viewModel)
        {
            // Log.Instance.Info(DelimeterEnum.Space, "Start LoadLoanAccrualForCurrentMonth");
            var output = "success";

            var loanDetails = await this.GetLoanAccrualDetails(viewModel.PayrollStartDate, viewModel.PayrollEndDate, viewModel.Id);
            // Log.Instance.Info(DelimeterEnum.Space, "Current month Loan Count:", loanDetails.Count);
            var accrualTransactions = await _payrollTransactionBusiness.GetLoanAccrualTransactions(viewModel.PayrollEndDate.AddDays(-50), viewModel.PayrollEndDate);
            List<string> personList = new List<string>();
            var transactionList = new List<PayrollTransactionViewModel>();
            if (loanDetails.Count > 0)
            {
                //personList=loanDetails.Select()
                personList = loanDetails.Select(x => x.PersonId).ToList();
                foreach (var personId in personList)
                {
                    var closedtrans = await ManageClosedTransaction(personId, "MONTHLY_LOAN_ACCRUAL", viewModel.AttendanceEndDate);
                    if (!closedtrans)
                    {
                        continue;
                    }
                    var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_LOAN_ACCRUAL" && x.PersonId == personId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());

                    if (existingTransactions == null)
                    {
                        var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_LOAN_ACCRUAL" && x.PersonId == personId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                        double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                        var loans = loanDetails.Where(x => x.ElementCode == "LOAN" && x.PersonId == personId).ToList();
                        var ea = 0.0;
                        if (loans != null && loans.Count > 0)
                        {
                            ea = loans.Sum(x => x.EarningAmount);
                        }

                        var da = 0.0;
                        var loanInstallments = loanDetails.Where(x => x.ElementCode == "LOAN_DEDUCTION" && x.PersonId == personId).ToList();
                        if (loanInstallments != null && loanInstallments.Count > 0)
                        {
                            da = loanInstallments.Sum(x => x.DeductionAmount);
                        }
                        var cq = (ob + ea) - da;

                        var model = new PayrollTransactionViewModel
                        {
                            OpeningBalance = ob.RoundToTwoDecimal(),
                            ClosingBalance = cq.RoundToTwoDecimal(),
                            Amount = (ea - da).RoundToTwoDecimal(),
                            EarningAmount = ea.RoundToTwoDecimal(),
                            DeductionAmount = da.RoundToTwoDecimal(),
                            EffectiveDate = viewModel.AttendanceEndDate,
                            ProcessStatus = PayrollProcessStatusEnum.NotProcessed,
                            PostedSource = PayrollPostedSourceEnum.Accrual,
                            PostedDate = DateTime.Now,
                            ElementCode = "MONTHLY_LOAN_ACCRUAL",
                            PersonId = personId,
                            //CreatedDate = DateTime.Now,
                            //LastUpdatedDate = DateTime.Now,
                            //CreatedBy = UserId,
                            //LastUpdatedBy = UserId,
                            ReferenceNode = NodeEnum.HRS_PersonRoot,
                            ReferenceId = personId,
                            // Operation = DataOperation.Create
                        };
                        // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                        var element = await GetElementDetails(model.ElementCode);

                        if (element.IsNotNull())
                        {
                            model.ElementId = element.Id;
                            // transModel.ElementType =element["Id"].ToSafeInt();
                            model.ElementType = element.ElementType;
                            model.ElementClassification = element.ElementClassification;
                            model.ElementCategory = element.ElementCategory;


                        }
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Create
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        var result = await _noteBusiness.ManageNote(noteModel);

                        // transactionList.Add(model);
                    }
                    else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                    {
                        var pastExistingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_LOAN_ACCRUAL" && x.PersonId == personId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.AddDays(-40).Month.ToString());
                        double ob = pastExistingTransactions?.ClosingBalance ?? 0;
                        var loans = loanDetails.Where(x => x.ElementCode == "LOAN" && x.PersonId == personId).ToList();
                        var ea = 0.0;
                        if (loans != null && loans.Count > 0)
                        {
                            ea = loans.Sum(x => x.EarningAmount);
                        }

                        var da = 0.0;
                        var loanInstallments = loanDetails.Where(x => x.ElementCode == "LOAN_DEDUCTION" && x.PersonId == personId).ToList();
                        if (loanInstallments != null && loanInstallments.Count > 0)
                        {
                            da = loanInstallments.Sum(x => x.DeductionAmount);
                        }
                        var cq = (ob + ea) - da;

                        existingTransactions.OpeningBalance = ob.RoundToTwoDecimal();
                        existingTransactions.EarningAmount = ea.RoundToTwoDecimal();
                        existingTransactions.DeductionAmount = da.RoundToTwoDecimal();
                        existingTransactions.Amount = (ea - da).RoundToTwoDecimal();
                        existingTransactions.ClosingBalance = cq.RoundToTwoDecimal();
                        existingTransactions.Operation = DataOperation.Correct;
                        existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                        existingTransactions.PostedDate = DateTime.Now;
                        existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            NoteId = existingTransactions.NtsNoteId,
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Edit
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                        var result = await _noteBusiness.ManageNote(noteModel);
                        // transactionList.Add(existingTransactions);
                    }

                }

            }
            var pendingLoans = await this.GetPendingLoanAccrualDetails(viewModel.PayrollStartDate, viewModel.PayrollEndDate, viewModel.Id, personList);
            if (pendingLoans.Count > 0)
            {
                personList = pendingLoans.Select(x => x.PersonId).Distinct().ToList();
                foreach (var personId in personList)
                {
                    var closedtrans = await ManageClosedTransaction(personId, "MONTHLY_LOAN_ACCRUAL", viewModel.AttendanceEndDate);
                    if (!closedtrans)
                    {
                        continue;
                    }
                    var existingTransactions = accrualTransactions.FirstOrDefault(x => x.ElementCode == "MONTHLY_LOAN_ACCRUAL" && x.PersonId == personId && x.EffectiveDate.Month.ToString() == viewModel.PayrollEndDate.Month.ToString());
                    if (existingTransactions == null)
                    {
                        var pastExistingTransactions = pendingLoans.FirstOrDefault(x => x.ElementCode == "MONTHLY_LOAN_ACCRUAL" && x.PersonId == personId);
                        if (pastExistingTransactions != null)
                        {
                            var model = new PayrollTransactionViewModel
                            {
                                OpeningBalance = pastExistingTransactions.ClosingBalance,
                                ClosingBalance = pastExistingTransactions.ClosingBalance,
                                Amount = 0.0,
                                EarningAmount = 0.0,
                                DeductionAmount = 0.0,
                                EffectiveDate = viewModel.AttendanceEndDate,
                                ProcessStatus = PayrollProcessStatusEnum.NotProcessed,
                                PostedSource = PayrollPostedSourceEnum.Accrual,
                                PostedDate = DateTime.Now,
                                ElementCode = "MONTHLY_LOAN_ACCRUAL",
                                PersonId = personId,
                                //CreatedDate = DateTime.Now,
                                //LastUpdatedDate = DateTime.Now,
                                //CreatedBy = UserId,
                                //LastUpdatedBy = UserId,
                                ReferenceNode = NodeEnum.HRS_PersonRoot,
                                ReferenceId = personId,
                                // = DataOperation.Create
                            };
                            // var element = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", model.ElementCode);
                            var element = await GetElementDetails(model.ElementCode);

                            if (element.IsNotNull())
                            {
                                model.ElementId = element.Id;
                                // transModel.ElementType =element["Id"].ToSafeInt();
                                model.ElementType = element.ElementType;
                                model.ElementClassification = element.ElementClassification;
                                model.ElementCategory = element.ElementCategory;


                            }
                            var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                            {
                                TemplateCode = "PayrollTransaction",
                                ActiveUserId = _repo.UserContext.UserId,
                                DataAction = DataActionEnum.Create
                            });

                            noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                            var result = await _noteBusiness.ManageNote(noteModel);
                            // transactionList.Add(model);
                        }

                    }
                    else //if (existingTransactions.FirstOrDefault().ProcessStatus == PayrollProcessStatusEnum.Draft)
                    {
                        var pastExistingTransactions = pendingLoans.FirstOrDefault(x => x.ElementCode == "MONTHLY_LOAN_ACCRUAL" && x.PersonId == personId);
                        existingTransactions.OpeningBalance = pastExistingTransactions?.ClosingBalance ?? 0;
                        existingTransactions.EarningAmount = 0.0;
                        existingTransactions.DeductionAmount = 0.0;
                        existingTransactions.Amount = 0.0;
                        existingTransactions.ClosingBalance = pastExistingTransactions?.ClosingBalance ?? 0;
                        existingTransactions.Operation = DataOperation.Correct;
                        existingTransactions.EffectiveDate = viewModel.AttendanceEndDate;
                        existingTransactions.PostedDate = DateTime.Now;
                        existingTransactions.PostedSource = PayrollPostedSourceEnum.Accrual;
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            TemplateCode = "PayrollTransaction",
                            NoteId = existingTransactions.NtsNoteId,
                            ActiveUserId = _repo.UserContext.UserId,
                            DataAction = DataActionEnum.Edit
                        });

                        noteModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existingTransactions);
                        var result = await _noteBusiness.ManageNote(noteModel);
                        // transactionList.Add(existingTransactions);
                    }

                }
            }
            //if (transactionList.Count > 0)
            //{
            //    Log.Instance.Info(DelimeterEnum.Space, "loan accrual transactionList count: ", transactionList.Count);
            //    var bulkInsertList = transactionList.Where(x => x.Operation == DataOperation.Create).ToList();
            //    UpdateAccuralsElementDetail(bulkInsertList);
            //    Log.Instance.Info(DelimeterEnum.Space, "bulkInsertList count: ", bulkInsertList.Count);
            //    _payrollTransactionBusiness.BulkInsert(bulkInsertList, false);

            //    var bulkUpdteList = transactionList.Where(x => x.Operation == DataOperation.Correct).ToList();
            //    Log.Instance.Info(DelimeterEnum.Space, "bulkUpdteList count: ", bulkUpdteList.Count);
            //    _payrollTransactionBusiness.BulkUpdate(bulkUpdteList, false);
            //}


            //Log.Instance.Info(DelimeterEnum.Space, "End LoadVacationAccrualForCurrentMonth");

            return output;
        }

        private async Task<List<PayrollTransactionViewModel>> GetLoanAccrualDetails(DateTime payrollStartDate, DateTime payrollEndDate, string payRollRunId)
        {
            //var prms = new Dictionary<string, object>
            //{
            //    { "ESD", payrollStartDate },
            //    { "EED", payrollEndDate },
            //    { "PayRollRunId", payRollRunId }
            //};

            var cypher = $@"select pt.*,per.""Id"" as PersonId,u.""Id"" as UserId,pe.""Id"" as ElementId,pe.""ElementCode"" as ElementCode
from cms.""N_CoreHR_HRPerson"" as per
join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=per.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRun"" as pay on pay.""Id""=prp.""PayrollRunId"" and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollTransaction"" as pt on pt.""PersonId""=per.""Id"" and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollElement"" as pe on pe.""Id""=pt.""ElementId"" and pe.""IsDeleted""=false and pe.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=per.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
where pay.""Id""='{payRollRunId}' and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}' --and pe.""EffectiveStartDate"" <={payrollEndDate} and {payrollEndDate} <= pe.""EffectiveEndDate"" 
     -- and pt.""EffectiveStartDate"" <={payrollStartDate} and {payrollEndDate} <= pt.""EffectiveEndDate""  
and pe.""ElementCode"" in ('LOAN','LOAN_DEDUCTION') ";

            var result = await _payrunrepo.ExecuteQueryList<PayrollTransactionViewModel>(cypher, null);
            return result;
        }

        private async Task<List<PayrollTransactionViewModel>> GetPendingLoanAccrualDetails(DateTime payrollStartDate, DateTime payrollEndDate, string payRollRunId, List<string> exculdePersons)
        {
            var prms = new Dictionary<string, object>
            {
                { "ESD", payrollStartDate },
                { "EED", payrollEndDate },
                { "LOANDATE", payrollStartDate.AddDays(-20) },
                { "PayRollRunId", payRollRunId }
            };
            var excludePersonText = "'" + string.Join("','", exculdePersons) + "'";
            var LOANDATE = payrollStartDate.AddDays(-20);

            //var cypher = string.Concat(@"match(pr:HRS_PersonRoot)<-[:R_PersonRoot]-(per:HRS_Person{ IsDeleted: 0,CompanyId: 1, Status: 'Active'})
            //    where  per.EffectiveStartDate <= {EED} <= per.EffectiveEndDate and not pr.Id in [", excludePersonText, @"]
            //    match(pr)<-[:R_User_PersonRoot]-(user:ADM_User)
            //    match (pr)<-[R_PayrollRun_PersonRoot]-(prr:PAY_PayrollRun{Id:{PayRollRunId}})
            //    match(pr)<-[:R_PayrollTransaction_PersonRoot]-(pt:PAY_PayrollTransaction)
            //    match (pt)-[:R_PayrollTransaction_ElementRoot]->(er:PAY_ElementRoot)
            //    <-[:R_ElementRoot]-(e:PAY_Element{Code:'MONTHLY_LOAN_ACCRUAL'})
            //    where e.EffectiveStartDate <= {EED} <= e.EffectiveEndDate 
            //    and {LOANDATE}<=pt.EffectiveDate<={ESD}  and pt.ClosingBalance<>0
            //    return pt,pr.Id as PersonId,user.Id as UserId,er.Id as ElementId,e.Code as ElementCode");

            //var result = ExecuteCypherList<PayrollTransactionViewModel>(cypher, prms);

            var cypher = $@"select pt.*,per.""Id"" as PersonId,u.""Id"" as UserId,pe.""Id"" as ElementId,pe.""ElementCode"" as ElementCode
from cms.""N_CoreHR_HRPerson"" as per
join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=per.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRun"" as pay on pay.""Id""=prp.""PayrollRunId"" and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollTransaction"" as pt on pt.""PersonId""=per.""Id"" and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollElement"" as pe on pe.""Id""=pt.""ElementId"" and pe.""IsDeleted""=false and pe.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=per.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
where pay.""Id""='{payRollRunId}' and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}' and per.""Id"" not in ({excludePersonText})  --and pe.""EffectiveStartDate"" <={payrollEndDate} and {payrollEndDate} <= pe.""EffectiveEndDate"" 
 and pt.""ClosingBalance""<>'0' --and pt.""EffectiveStartDate"" <={LOANDATE} and {payrollEndDate} <= pt.""EffectiveEndDate""  
and pe.""ElementCode"" in ('MONTHLY_LOAN_ACCRUAL') ";

            var result = await _payrunrepo.ExecuteQueryList<PayrollTransactionViewModel>(cypher, null);
            return result;
        }

        private async Task<List<TicketAccrualViewModel>> GetSickLeaveAccrualPersonList(DateTime payrollDate, string payRollRunId)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("EED", payrollDate);
            //prms.AddIfNotExists("payRollRunId", payRollRunId);

            var cypher = $@"select per.""Id"" as PersonId,u.""Id"" as UserId
from cms.""N_CoreHR_HRPerson"" as per
join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=per.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRun"" as pay on pay.""Id""=prp.""PayrollRunId"" and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=per.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
where pay.""Id""='{payRollRunId}' and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _payrunrepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);
            //var result = ExecuteCypherList<TicketAccrualViewModel>(cypher, prms);
            return result;
        }
        private async Task<bool> ManageClosedTransaction(string personId, string elementCode, DateTime payrollEndDate)
        {
            var cypher = string.Concat($@"
            Select pt.* from 
cms.""N_PayrollHR_PayrollTransaction"" as pt 
join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=pt.""PersonId"" and p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""IsDeleted""=false and e.""ElementCode""='{elementCode}' and e.""CompanyId""='{_repo.UserContext.CompanyId}'
where pt.""IsTransactionClosed"" ='true' --and e.""EffectiveStartDate"" <= '{DateTime.Now}' and '{DateTime.Now}' <= e.""EffectiveEndDate""
and pt.""EffectiveDate"" = '{payrollEndDate}' and pt.""IsDeleted"" = false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'");
            var closedTransaction = await _payrunrepo.ExecuteQuerySingle<PayrollTransactionViewModel>(cypher, null);
            if (closedTransaction.IsNotNull())
            {
                var res = await _payrollTransactionBusiness.DeletePayrollTransaction(closedTransaction.Id);
                return false;
            }
            return true;
        }

        private async Task<PayrollTransactionViewModel> GetElementDetails(string elementCode)
        {
            var cypher = string.Concat($@"
           SELECT * FROM cms.""N_PayrollHR_PayrollElement""
where ""ElementCode"" = '{elementCode}' and ""IsDeleted"" = false and ""CompanyId""='{_repo.UserContext.CompanyId}'");
            var data = await _payrunrepo.ExecuteQuerySingle<PayrollTransactionViewModel>(cypher, null);
            return data;
        }
        public async Task<List<MandatoryDeductionSlabViewModel>> GetSingleSlabById(string mandatoryDeductionId)
        {
                //index
                var query = @$"Select * from cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB""
                            where ""MandatoryDeductionId""='{mandatoryDeductionId}' and ""IsDeleted""=false ";
                var result = await _mandatoryDeductionSlabRepo.ExecuteQueryList<MandatoryDeductionSlabViewModel>(query, null);
                return result;
        }

        public async Task<MandatoryDeductionSlabViewModel> GetSingleSlabEntryById(string id)
        {
            //edit
            var query = $@"Select * from cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB""
                            where ""Id""='{id}' and ""IsDeleted""=false ";

            var result = await _mandatoryDeductionSlabRepo.ExecuteQuerySingle<MandatoryDeductionSlabViewModel>(query, null);
            return result;
        }
        public async Task<bool> ValidateMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model)
        {
            bool flag = false;
            var query = $@"select * from cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB""
                        where ""IsDeleted""=false 
                        and ""MandatoryDeductionId"" = '{model.MandatoryDeductionId}'
                        and (((cast(""SlabFrom"" as decimal) <=cast('{model.SlabFrom}' as decimal) and cast('{model.SlabFrom}' as decimal) <= cast(""SlabTo"" as decimal))
                                or (cast(""SlabFrom"" as decimal) <= cast('{model.SlabTo}' as decimal) and cast('{model.SlabTo}' as decimal) <= cast(""SlabTo"" as decimal)))
	                        )
                        and ((""ESD""::Date <='{model.ESD}'::Date and '{model.ESD}'::Date <= ""EED""::Date) 
                            or (""ESD""::Date <= '{model.EED}'::Date and '{model.EED}'::Date <= ""EED""::Date))
                        #WHERE#
                        ";
            var where = "";
            if (model.DataAction == DataActionEnum.Edit)
            {
                if (model.Id.IsNotNullAndNotEmpty())
                {
                    where = $@" and ""Id""!='{model.Id}' ";
                }
            }
            query = query.Replace("#WHERE#", where);
            var result = await _mandatoryDeductionSlabRepo.ExecuteQueryList<MandatoryDeductionSlabViewModel>(query, null);
            if (result!=null && result.Count>0)
            {
                flag = true;
            }
            return flag;
        }
        public async Task CreateMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model)

        {
            //create
            var _cmsBusiness = _sp.GetService<ICmsBusiness>();
            var Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var create = await _cmsBusiness.CreateForm(Json, "", "MANDATORY_DEDUCTION_SLAB");

        }

        public async Task EditMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model)
        {
            //edit
            var _cmsBusiness = _sp.GetService<ICmsBusiness>();
            var Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var edit = await _cmsBusiness.EditForm(model.Id, Json, "", "MANDATORY_DEDUCTION_SLAB");

        }

        public async Task DeleteMandatoryDeductionSlab(string Id)
        {
            var query = $@"Update cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB"" set ""IsDeleted""=true where ""Id""='{Id}' and ""CompanyId""='{_repo.UserContext.CompanyId}' ";
            await _mandatoryDeductionSlabRepo.ExecuteCommand(query, null);
        }
    }
}
