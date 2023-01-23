using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using CMS.UI.ViewModel.Pay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class PayrollTransactionBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollTransactionsBusiness
    {
        private IServiceProvider _serviceProvider;
        private INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollTransactionViewModel> _payrollTransaction;
        private readonly IRepositoryQueryBase<ElementViewModel> _queryRepoElement;
        private IPayrollElementBusiness _payrollElementBusiness;
        IUserContext _userContext;
        private readonly ILOVBusiness _lovBusiness;
        public PayrollTransactionBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper, INoteBusiness noteBusiness,
            IRepositoryQueryBase<NoteViewModel> queryRepo,
            IRepositoryQueryBase<PayrollTransactionViewModel> payrollTransaction,
            IServiceProvider serviceProvider
            , IPayrollElementBusiness payrollElementBusiness
            , IRepositoryQueryBase<ElementViewModel> queryRepoElement
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
                var query = $@"update  cms.""N_PayrollHR_PayrollTransaction"" set ""IsDeleted""=true where ""Id""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);
                return true;
            }
            return false;
        }

        public async Task<PayrollTransactionViewModel> GetPayrollTransactionDetails(string transactionId)
        {
            var query = "";
            if (transactionId.IsNotNullAndNotEmpty())
            {
                query = $@"select * from cms.""N_PayrollHR_PayrollTransaction""  as pt where pt.""Id"" = '{transactionId}' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            }
            var queryData = await _payrollTransaction.ExecuteQuerySingle<PayrollTransactionViewModel>(query, null);
            return queryData;

        }
        public async Task<IdNameViewModel> GetPayrollElementByCode(string code)
        {
            var query = "";

            query = $@"select ""ElementName"" as Name,""Id"" as Id from cms.""N_PayrollHR_PayrollElement""  as pt where pt.""ElementCode"" = '{code}' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _payrollTransaction.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;

        }

        public async Task<List<PayrollTransactionViewModel>> GetAllUnProcessedTransactionsForPayrollRun(PayrollRunViewModel viewModel)
        {
            //throw new NotImplementedException();
            //var cypher = string.Concat(@"match (r:PAY_PayrollRun{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{PayrollRunId}})
            //   match (r)-[:R_PayrollRun_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         match(pr)<-[:R_PayrollTransaction_PersonRoot]-(pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId},ProcessStatus:'NotProcessed'}) 
            //         where pt.EffectiveDate<={PED}
            //match(pt)-[:R_PayrollTransaction_ElementRoot]->(er:PAY_ElementRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         where  e.EffectiveStartDate <= pt.EffectiveDate <= e.EffectiveEndDate 
            //         with r,pr,pt,er,e
            //         set pt.PayrollId={PayrollId},pt.PayrollRunId={PayrollRunId}
            //         return pt,er.Id as ElementId,pr.Id as PersonId");
            //var list = ExecuteCypherList<PayrollTransactionViewModel>(cypher, prms);

            var query = $@"Select distinct pt.*, e.""Id"" as ElementId, pr.""Id"" as PersonId
                                     From cms.""N_PayrollHR_PayrollRun"" as r
                                     Join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PayrollRunId""=r.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
                                     Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=prp.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                                     Join cms.""N_PayrollHR_PayrollTransaction"" as pt on pt.""PersonId""=pr.""Id"" and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'  and pt.""EffectiveDate""::Date <= '{viewModel.PayrollEndDate}'::Date
                                     join public.""LOV"" as lov on lov.""Id""=pt.""ProcessStatusId"" and lov.""Code""='NOT_PROCESSED' and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                                     Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""IsDeleted""=false 
                                     where r.""IsDeleted""=false and r.""CompanyId""='{_repo.UserContext.CompanyId}'  ";
            var list = await _payrollTransaction.ExecuteQueryList(query, null);
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
                var query1 = $@"Update cms.""N_PayrollHR_PayrollTransaction""
                            set ""PayrollBatchId""='{viewModel.PayrollBatchId}',""PayrollRunId""='{viewModel.Id}', ""LastUpdatedDate""='{lastUpdateDate}'
                        where ""Id"" IN ({payTransIds}) and ""IsDeleted""=false";
                await _payrollTransaction.ExecuteCommand(query1, null);
            }
            return list.ToList();
        }

        public async Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionList(PayrollTransactionViewModel search)
        {

            var query = string.Concat($@"Select pt.*, per.""Id"" as ElementId, e.""ElementName"", u.""Id"" as UserId, p.""Id"" as PersonId, p.""SponsorshipNo"" as EmployeeNo,
                                                 lov.""Name"" as Gender, a.""DateOfJoin"", p.""PersonNo"", na.""Name"" as Nationality, o.""Name"" as OrganizationName, orr.""Id"" as OrganizationId,
                                                 cc.""Name"" as CostCenter,j.""JobTitle"" as JobName, c.""EffectiveEndDate"" as ContractEndDate,c.""ContractRenewable"" as ContractRenewable,sp.""Name"" as Sponsor,
                                                 CASE WHEN sc.""Id"" is null THEN '' ELSE sc.""Id"" END as SectionId, CASE WHEN sc.""Name"" is null THEN '' ELSE sc.""Name"" END as SectionName,
                                                 a.""Id"" as AssignmentId, CASE WHEN o.""IsPayrollOrganization""=true THEN orr.""Id"" ELSE por.""Id"" END as PayrollOrganizationId, ",
                                        Helper.UserDisplayName("u", " as UserNameWithEmail,"),
                                        Helper.PersonDisplayName("p", " as EmployeeName order by EmployeeName"),
                                        $@" From cms.""PayrollHR_PayrollGroup"" as pg
                                        Join cms.""N_PayrollHR_SalaryInfo"" as ps on ps.""PayGroupId""=pg.""Id"" and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId"" and p.""EffectiveStartDate"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        Join cms.""N_PayrollHR_PayrollTransaction"" as pt on pt.""PersonId""=p.""Id"" and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        and datetime(pt.EffectiveDate).month={search.Month} and datetime(pt.EffectiveDate).year={search.Year} and pt.""IsDeleted""=false
                                        Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE
                                        
                                        Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        and a.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and a.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE
                                        
                                        Join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        and o.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and o.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE
                                        
                                        Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
                                        Left Join cms.""N_CoreHR_HRDepartment"" as po on po.""Id""=o.""PayrollDepartmentId"" and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        and po.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and po.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE
                                        
                                        Left Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        and j.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and j.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE
                                        
                                        Left Join cms.""N_CoreHR_HRCostCenter"" as cc on cc.""Id""=o.""CostCenterId"" and cc.""IsDeleted""=false and cc.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        Left Join cms.""N_CoreHR_HRNationality"" as na on na.""Id""=p.""NationalityId"" and na.""IsDeleted""=false and na.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        Left Join cms.""N_CoreHR_HRSection"" as sc on sc.""Id""=p.""SectionId"" and sc.""IsDeleted""=false and sc.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        Left Join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        and c.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and c.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE
                                        
                                        Left join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        where pg.""Id""='{search.PayrollGroupId}' and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'");


            var result1 = await _payrollTransaction.ExecuteQueryList(query, null);

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

                var query = $@"Update cms.""N_PayrollHR_PayrollTransaction""
                            set ""PayrollBatchId""=null,""PayrollRunId""=null, ""LastUpdatedDate""='{lastUpdateDate}'
                        where ""Id"" IN ({notProcessedIds}) ";
                await _payrollTransaction.ExecuteCommand(query, null);
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
                var query1 = $@"Update cms.""N_PayrollHR_PayrollTransaction""
                            set ""PayrollBatchId""='{payrollId}',""PayrollRunId""='{payrollRunId}',""ProcessStatusId""='{processStatus.Id}', ""LastUpdatedDate""='{lastUpdateDate}'
                        where ""Id"" IN ({processedIds}) ";
                await _payrollTransaction.ExecuteCommand(query1, null);
            }


            return viewModelList;
        }

        public async Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionBasedonElement(string personId, DateTime EffectiveDate, string ElementCode)
        {
            var cypher = $@"Select pt.* from 
cms.""N_PayrollHr_PayrollTransaction"" as pt 
join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=pt.""PersonId"" and p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHr_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
where e.""EffectiveStartDate""<='{DateTime.Now}'<=e.""EffectiveEndDate"" and pt.""EffectiveDate""='{EffectiveDate}' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId}}) 
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{Id:{personId}, IsDeleted:0,CompanyId:{CompanyId}})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0,CompanyId: {CompanyId}})<-[:R_ElementRoot]-(e:PAY_Element{Code:{ElementCode}, IsDeleted: 0,CompanyId: {CompanyId} })
            //where datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate)
            //and date(datetime(pt.EffectiveDate)) = date(datetime({date}))
            //return pt";

            //var prms = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "ElementCode", ElementCode },
            //    { "personId", personId },
            //    { "date", EffectiveDate },
            //};

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(cypher, null);
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

            var query = $@"Select pt.* From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""ElementCode""='{elementCode}' 
                            and e.""EffectiveStartDate""::DATE<=pt.""EffectiveDate""::DATE<=e.""EffectiveEndDate""::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=pt.""PersonId"" and p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where  pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}' and pt.""EffectiveDate""::DATE='{date}'::DATE and pt.""Amount""={amount} limit 1 ";

            var list = await _payrollTransaction.ExecuteQueryList(query, null);
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
        public async Task<MandatoryDeductionViewModel> GetExemption(MandatoryDeductionViewModel model,string financialYearId,string personId) 
        {
            var query = $@"select Sum(id.""TotalAmount""::DECIMAL) as TotalAmount,de.""SeniorCitizenAmount"" as SeniorCitizenAmount,de.""WomanAmount"" as WomanAmount,de.""DefaultAmount"" as EmployeeAmount from cms.""N_PayrollHR_MandatoryDeduction"" as md 
join cms.""N_PayrollHR_DEDUCTION_EXEMPTION"" as de on de.""MandatoryDeductionId""=md.""Id"" and de.""IsDeleted""=false
join cms.""F_PAY_HR_InvestmentType"" as it on it.""DeductionExemptionId""=de.""Id"" and it.""IsDeleted""=false
join cms.""N_SNC_CHR_InvestmentDeclaration"" as id on id.""InvestmentTypeId""=it.""Id"" and id.""FinancialYearId""='{financialYearId}' and id.""IsDeleted""=false 
join public.""NtsService"" as s on s.""UdfNoteTableId""=id.""Id"" and s.""IsDeleted""=false
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=s.""OwnerUserId"" and p.""IsDeleted""=false and p.""Id""='{personId}'
and md.""IsDeleted""=false group by id.""Id"",id.""TotalAmount"",de.""SeniorCitizenAmount"",de.""WomanAmount"",de.""DefaultAmount""";
            var data = await _queryRepo.ExecuteQuerySingle<MandatoryDeductionViewModel>(query,null);
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
                    var Exemption =await GetExemption(deduction, FinancialYear.Id, person.PersonId); //return a double value 
                        var Person = await _hrCoreBusiness.GetPersonDetailsById(person.PersonId);
                        string PersonType = string.Empty;
                        double ExemptionAmount = 0.0;
                        if (Person.IsNotNull()) 
                        {
                            if (DateTime.Now.Year-Person.DateOfBirth.Year > 60)
                            {
                                PersonType = "SeniorCitizen";
                                if (Exemption.SeniorCitizenAmount > Exemption.TotalAmount)
                                {
                                    ExemptionAmount =  Exemption.TotalAmount;
                                }
                                else
                                {
                                    ExemptionAmount= Exemption.SeniorCitizenAmount;
                                }
                            }
                            else
                            {
                                var Gender = await _lovBusiness.GetSingleById(Person.GenderId);
                                if (Gender.Code=="FEMALE")
                                {
                                    PersonType = "Woman";
                                    if (Exemption.WomanAmount > Exemption.TotalAmount)
                                    {
                                        ExemptionAmount = Exemption.TotalAmount;
                                    }
                                    else
                                    {
                                        ExemptionAmount = Exemption.WomanAmount;
                                    }
                                }
                                else 
                                {
                                    PersonType = "Employee";
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
            var query = $@"Select *,""FinancialYearName"" as DeductionName,""StartDate"" as EffectiveStartDate,""EndDate"" as EffectiveEndDate from cms.""F_PAY_HR_FinancialYearName"" where ""StartDate""::DATE<='{asOfDate}'::DATE and ""EndDate""::DATE>='{asOfDate}'::DATE  and ""IsDeleted"" = false";
            var result = await _queryRepoElement.ExecuteQueryList<MandatoryDeductionViewModel>(query, null);
            return result;
        }
        public async Task<List<MandatoryDeductionViewModel>> GetMandatoryDeductionOfFinancialYear(DateTime asOfDate) 
        {
            var query = $@"Select *,""NtsNoteId"" as NoteId from cms.""N_PayrollHR_MandatoryDeduction"" where ""EffectiveStartDate""::DATE<='{asOfDate}'::DATE and ""EffectiveEndDate""::DATE>='{asOfDate}'::DATE and ""IsDeleted""=false";
            var result = await _queryRepoElement.ExecuteQueryList<MandatoryDeductionViewModel>(query,null);
            return result;
        }
        private async Task<List<PayrollTransactionViewModel>> GetSalaryTransactionList(DateTime startDate, DateTime endDate, string payrollRunId = null)
        {
            //         var cypher = string.Concat(@"
            //         match (pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId},PostedSource:'Payroll'}) 
            //         where {StartDate}<=pt.EffectiveDate<={EndDate}
            //match(pt)-[:R_PayrollTransaction_ElementRoot]->(er:PAY_ElementRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         where e.EffectiveStartDate<=pt.EffectiveDate<=e.EffectiveEndDate
            //   match(pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         return pt,er.Id as ElementRootId,er.Id as ElementId,pr.Id as PersonId
            //         ,e.EffectiveStartDate as ElementEffectiveStartDate,e.EffectiveEndDate as ElementEffectiveEndDate");
            //         if (payrollRunId.IsNotNullAndNotEmpty())
            //         {
            //             cypher = string.Concat(@"
            //             match (pt:PAY_PayrollRun{IsDeleted:0,CompanyId:{CompanyId},Id:{PayrollRunId}})-[:R_PayrollRun_PersonRoot]
            //             ->(pr:HRS_PersonRoot)<-[:R_PayrollTransaction_PersonRoot]
            //             -(pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId},PostedSource:'Payroll'}) 
            //             where {StartDate}<=pt.EffectiveDate<={EndDate}
            //    match(pt)-[:R_PayrollTransaction_ElementRoot]->(er:PAY_ElementRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //             where e.EffectiveStartDate<=pt.EffectiveDate<=e.EffectiveEndDate
            //       match(pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //             return pt,er.Id as ElementRootId,er.Id as ElementId,pr.Id as PersonId
            //             ,e.EffectiveStartDate as ElementEffectiveStartDate,e.EffectiveEndDate as ElementEffectiveEndDate");
            //         }

            //         var prms = new Dictionary<string, object>
            //         {
            //             { "CompanyId", CompanyId },
            //             { "Status", StatusEnum.Active.ToString() },
            //             { "StartDate", startDate },
            //             { "EndDate", endDate},
            //             { "PayrollRunId", payrollRunId}

            //         };
            //         var list = ExecuteCypherList<PayrollTransactionViewModel>(cypher, prms);
            //         return list.ToList();
            var postedSource = (int)PayrollPostedSourceEnum.Payroll;
            var query = $@"select pt.*,e.""Id"" as ElementId,pr.""Id"" as PersonId
                            ,e.""EffectiveStartDate"" as ElementEffectiveStartDate,e.""EffectiveEndDate"" as ElementEffectiveEndDate
                            from cms.""N_PayrollHR_PayrollTransaction"" as pt
                            join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id"" = pt.""ElementId"" and e.""IsDeleted"" = false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                                --and e.""EffectiveStartDate""::Date <= pt.""EffectiveDate""::Date and e.""EffectiveEndDate""::Date >= pt.""EffectiveDate""::Date
                            join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id"" = pt.""PersonId"" and pr.""IsDeleted"" = false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pt.""IsDeleted"" = false and pt.""CompanyId""='{_repo.UserContext.CompanyId}' and pt.""PostedSource""='{postedSource}'
                            and pt.""EffectiveDate""::Date >= '{startDate}'::Date and pt.""EffectiveDate""::Date <= '{endDate}'::Date
                        ";
            if (payrollRunId.IsNotNullAndNotEmpty())
            {
                query = $@"select pt.*,e.""Id"" as ElementId,pr.""Id"" as PersonId
                            ,e.""EffectiveStartDate"" as ElementEffectiveStartDate,e.""EffectiveEndDate"" as ElementEffectiveEndDate
                            from cms.""N_PayrollHR_PayrollTransaction"" as pt
                            join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id"" = pt.""ElementId"" and e.""IsDeleted"" = false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                                --and e.""EffectiveStartDate""::Date <= pt.""EffectiveDate""::Date and e.""EffectiveEndDate""::Date >= pt.""EffectiveDate""::Date
                            join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id"" = pt.""PersonId"" and pr.""IsDeleted"" = false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PersonId""=pr.""Id"" and prp.""IsDeleted""=false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollRun"" as prr on prr.""Id"" = prp.""PayrollRunId"" and prr.""IsDeleted"" = false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pt.""IsDeleted"" = false and pt.""CompanyId""='{_repo.UserContext.CompanyId}' and pt.""PostedSource""='{postedSource}' and prr.""Id""='{payrollRunId}'
                            and pt.""EffectiveDate""::Date >= '{startDate}'::Date and pt.""EffectiveDate""::Date <= '{endDate}'::Date
                        ";
            }
            var list = await _payrollTransaction.ExecuteQueryList(query, null);
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
            var query = $@"Select e.""Id"" as ""ElementId""
                            from cms.""N_PayrollHR_PayrollElement"" as e
                            where e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}' and e.""Id""='{elementId}'";
            var percentageElementRootIds = await _queryRepoElement.ExecuteQueryList(query, null);

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
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
            //where  {MinDate}<=pt.EffectiveDate<={MaxDate}
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0})<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0})
            //where e.Code in ['BASIC','TRANSPORT','FOOD'] and datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate) 
            //return pt,pr.Id as PersonId,e.Code as ElementCode";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""ElementCode""
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt 
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId""  and e.""ElementCode"" in ('BASIC','TRANSPORT','FOOD') 
                            and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where '{minAttendanceDate}'::DATE <= pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE <= '{maxAttendanceDate}'::DATE and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var list = await _payrollTransaction.ExecuteQueryList(query, null);
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
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId}}) 
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{Id:{personId}, IsDeleted:0,CompanyId:{CompanyId}})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0,CompanyId: {CompanyId}})<-[:R_ElementRoot]-(e:PAY_Element{Code:{ElementCode}, IsDeleted: 0,CompanyId: {CompanyId} })
            //where date(datetime(pt.EffectiveDate)).month = date(datetime({date})).month and date(datetime(pt.EffectiveDate)).year = date(datetime({date})).year
            //return pt";

            var query = $@"Select pt.* From cms.""N_PayrollHR_PayrollTransaction"" as pt 
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' and pr.""Id""='{personId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""IsDeleted""=false and e.""ElementCode""='{ElementCode}'
                            where EXTRACT(MONTH FROM pt.""EffectiveDate""::TIMESTAMP)=EXTRACT(MONTH FROM '{EffectiveDate}'::TIMESTAMP)
                            and EXTRACT(YEAR FROM pt.""EffectiveDate""::TIMESTAMP)=EXTRACT(YEAR FROM '{EffectiveDate}'::TIMESTAMP) and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<PayrollTransactionViewModel> GetPayrollTransationDataById(string id)
        {
            //var cypher = @"match(pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId}, Id: {Id}})
            //            match(pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot)
            //            match(pt)-[:R_PayrollTransaction_ElementRoot]->(er:PAY_ElementRoot)
            //            optional match(pt)<-[:R_Attachment_Reference{ReferenceTypeCode:'PAY_PayrollTransaction'}]-(at:GEN_Attachment)
            //            return pt, pr.Id as PersonId,er.Id as ElementId,at.FileId as AttachmentId";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""Id"" as ElementId, pt.""AttachmentId"" as AttachmentId
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pt.""Id""='{id}' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _payrollTransaction.ExecuteQuerySingle(query, null);
            return result;
        }
        public async Task<PayrollTransactionViewModel> GetPayrollTransationDataByReferenceId(string referenceId)
        {
            var query = "";
            if (referenceId.IsNotNullAndNotEmpty())
            {
                query = $@"select * from cms.""N_PayrollHR_PayrollTransaction""  as pt where pt.""ReferenceId"" = '{referenceId}' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";
            }
            var queryData = await _payrollTransaction.ExecuteQuerySingle<PayrollTransactionViewModel>(query, null);
            return queryData;
        }

        public async Task<List<PayrollTransactionViewModel>> GetAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
            //where  {MinDate}<=pt.EffectiveDate<={MaxDate}
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0})<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0})
            //where e.Code in ['MONTHLY_SELF_TICKET_ACCRUAL','MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL', 'MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL', 'MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL', 'ANNUAL_INFANT_TICKET_EARNING', 'ANNUAL_TICKET_EARNING', 'ANNUAL_CHILD_TICKET_EARNING', 'ANNUAL_DEPENDENT_TICKET_EARNING'] 
            //    and datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate) 
            //return pt,pr.Id as PersonId,e.Code as ElementCode";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""ElementCode""
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId""and e.""CompanyId""='{_repo.UserContext.CompanyId}' and e.""IsDeleted""=false
                            and e.""ElementCode"" in ('MONTHLY_SELF_TICKET_ACCRUAL','MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL', 'MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL', 'MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL', 'ANNUAL_INFANT_TICKET_EARNING', 'ANNUAL_TICKET_EARNING', 'ANNUAL_CHILD_TICKET_EARNING', 'ANNUAL_DEPENDENT_TICKET_EARNING')
                            --and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE and e.""IsDeleted""=false
                            --where '{minAttendanceDate}'::DATE <= pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE <= '{maxAttendanceDate}'::DATE and pt.""IsDeleted""=false ";

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetEosAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
            //where  {MinDate}<=pt.EffectiveDate<={MaxDate}
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0})<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0})
            //where e.Code in ['MONTHLY_EOS_ACCRUAL'] and datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate) 
            //return pt,pr.Id as PersonId,e.Code as ElementCode";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""ElementCode""
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" 
                            and e.""ElementCode"" in ('MONTHLY_EOS_ACCRUAL') and e.""CompanyId""='{_repo.UserContext.CompanyId}' and  e.""IsDeleted""=false
                            --and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE and e.""IsDeleted""=false
                            --where '{minAttendanceDate}'::DATE <= pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE <= '{maxAttendanceDate}'::DATE and pt.""IsDeleted""=false";

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetVacationAccrualTransactionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
            //where  {MinDate}<=pt.EffectiveDate<={MaxDate}
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0})<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0})
            //where e.Code in ['MONTHLY_VACATION_ACCRUAL'] and datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate) 
            //return pt,pr.Id as PersonId,e.Code as ElementCode";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""ElementCode""
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" 
                            and e.""ElementCode"" in ('MONTHLY_VACATION_ACCRUAL')
                            and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where '{minAttendanceDate}'::DATE <= pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE <= '{maxAttendanceDate}'::DATE and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetTicketEarningTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
            //where  {MinDate}<=pt.EffectiveDate<={MaxDate}
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0})<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0})
            //where e.Code in ['ANNUAL_INFANT_TICKET_EARNING', 'ANNUAL_TICKET_EARNING', 'ANNUAL_CHILD_TICKET_EARNING', 'ANNUAL_DEPENDENT_TICKET_EARNING'] and datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate) 
            //return pt,pr.Id as PersonId,e.Code as ElementCode";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""ElementCode""
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" 
                            and e.""ElementCode"" in ('ANNUAL_INFANT_TICKET_EARNING', 'ANNUAL_TICKET_EARNING', 'ANNUAL_CHILD_TICKET_EARNING', 'ANNUAL_DEPENDENT_TICKET_EARNING')
                            and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where '{minAttendanceDate}'::DATE <= pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE <= '{maxAttendanceDate}'::DATE and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetOtandDedcutionTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
            //where  {MinDate}<=pt.EffectiveDate<={MaxDate}
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0})<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0})
            //where e.Code in ['OT', 'LATE_COMING_DEDUCTION', 'UNPAID_LEAVE'] and datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate) 
            //return pt,pr.Id as PersonId,e.Code as ElementCode";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""ElementCode""
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" 
                            and e.""ElementCode"" in ('OT', 'LATE_COMING_DEDUCTION', 'UNPAID_LEAVE')
                            and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where '{minAttendanceDate}'::DATE <= pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE <= '{maxAttendanceDate}'::DATE and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionListByDates(PayrollTransactionViewModel search)
        {
            //         var cypher = string.Concat(@"match(pr:HRS_PersonRoot{IsDeleted: 0,CompanyId: { CompanyId} }) where (pr.Id={PersonId} or {PersonId} is null)
            //         match(pr)<-[:R_PersonRoot]-(p:HRS_Person{IsLatest:true,IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         match(pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(le:HRS_OrganizationRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}, Id: {LegalEntity}}) 
            //         match (pr)<-[:R_PayrollTransaction_PersonRoot]-(pt:PAY_PayrollTransaction{IsDeleted:0,CompanyId:{CompanyId}})
            //         where {StartDate}<=pt.EffectiveDate<={EndDate}
            //         match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0,CompanyId: {CompanyId}})
            //         <-[:R_ElementRoot]-(e:PAY_Element{IsLatest:true, IsDeleted: 0,CompanyId: {CompanyId} })
            //      match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{IsLatest:true, IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(a)-[:R_Assignment_OrganizationRoot]->(orr:HRS_OrganizationRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}}) where (orr.Id={OrgId} or {OrgId} is null)	
            //         match(orr)<-[:R_OrganizationRoot]-(o:HRS_Organization{IsLatest:true, IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}}) 
            //         match(cr)<-[:R_ContractRoot]-(c:HRS_Contract{IsLatest:true, IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(jr)<-[:R_JobRoot]-(j:HRS_Job{IsLatest:true, IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         optional match(pt)<-[:R_Attachment_Reference{ReferenceTypeCode:'PAY_PayrollTransaction'}]-(at:GEN_Attachment)            
            //         return pt,per.Id as ElementId,e.Name as ElementName,u.Id as UserId,pr.Id as PersonId,p.SponsorshipNo as EmployeeNo,p.Gender as Gender,a.DateOfJoin as DateOfJoin,
            //         p.PersonNo as PersonNo,o.Name as OrganizationName,orr.Id as OrganizationId,j.Name as JobName,p.SponsorshipNo as SponsorshipNo,
            //         c.EffectiveEndDate as ContractEndDate,c.ContractRenewable as ContractRenewable
            //         ,a.Id as AssignmentId,at.FileId as AttachmentId,",
            //        Helper.UserDisplayName("u", " as UserNameWithEmail,"),
            //        Helper.PersonDisplayName("p", " as EmployeeName order by EmployeeName")
            //        );

            string personId = null;

            if (search.PersonId.IsNotNull())
            {
                personId = search.PersonId;
            }

            var query = string.Concat($@"Select pt.*, per.""Id"" as ElementId, e.""ElementName"", u.""Id"" as UserId, p.""Id"" as PersonId, p.""SponsorshipNo"" as EmployeeNo,
                        lov.""Name"" as Gender, a.""DateOfJoin"", p.""PersonNo"", o.""Name"" as OrganizationName, orr.""Id"" as OrganizationId, j.""Name"" as JobName, p.""SponsorshipNo"",
                        c.""EffectiveEndDate"" as ContractEndDate, c.""ContractRenewable"" as ContractRenewable, a.""Id"" as AssignmentId, pt.""AttachmentId""
                        From cms.""N_CoreHR_HRPerson"" as p
                        Join public.""LegalEntity"" as le on le.""Id""=p.""PersonLegalEntityId"" and le.""Id""='{_repo.UserContext.LegalEntityId}' and le.""IsDeleted""=false and le.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_PayrollHR_PayrollTransaction"" as pt on pt.""PersonId""=p.""Id"" and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}' and '{search.StartDate}'::Date<=pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE<='{search.EndDate}'::DATE
                        Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""Id""='{search.OrganizationId}' and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join public.""LOV"" as lov on lov.""Id""=p.""GenderId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'",
                        Helper.UserDisplayName("u", " as UserNameWithEmail,"),
                        Helper.PersonDisplayName("p", " as EmployeeName order by EmployeeName"));

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<PayrollTransactionViewModel> IsTransactionExists(string personId, string elementCode, DateTime date)
        {
            //         var cypher = string.Concat(@"
            //         match (pt:PAY_PayrollTransaction{IsDeleted:0,Status:'Active',EffectiveDate:{EffectiveDate}})  
            //match(pt)-[:R_PayrollTransaction_ElementRoot]->(er:PAY_ElementRoot{IsDeleted:0,Status:'Active'})
            //match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:'Active',Code:{Code}})
            //         where e.EffectiveStartDate<=pt.EffectiveDate<=e.EffectiveEndDate 
            //   match(pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0,Status:'Active',Id:{PersonId}})
            //         return pt limit 1");

            var query = $@"Select pt.* From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""ElementCode""='{elementCode}'
                            and e.""EffectiveStartDate""::DATE<=pt.""EffectiveDate""::DATE and e.""EffectiveEndDate""::DATE<=pt.""EffectiveDate""::DATE and e.""IsDeleted""=false  and e.""CompanyId""='{_repo.UserContext.CompanyId}'                          
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=pt.""PersonId"" and p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where  pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}' and pt.""EffectiveDate""::DATE='{date}'::DATE limit 1";

            var list = await _payrollTransaction.ExecuteQuerySingle(query, null);
            return list;
        }

        public async Task<List<PayrollTransactionViewModel>> GetLoanAccrualTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
            //where  {MinDate}<=pt.EffectiveDate<={MaxDate}
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0})<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0})
            //where e.Code in ['MONTHLY_LOAN_ACCRUAL'] and datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate) 
            //return pt,pr.Id as PersonId,e.Code as ElementCode";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""ElementCode""
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" 
                            and e.""ElementCode"" in ('MONTHLY_LOAN_ACCRUAL')
                            and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where '{minAttendanceDate}'::DATE <= pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE <= '{maxAttendanceDate}'::DATE and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetSickLeaveAccrualTransactions(DateTime minAttendanceDate, DateTime maxAttendanceDate)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
            //where  {MinDate}<=pt.EffectiveDate<={MaxDate}
            //match (pt)-[:R_PayrollTransaction_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted:0})
            //match (pt)-[:R_PayrollTransaction_ElementRoot]->(per:PAY_ElementRoot{IsDeleted: 0})<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0})
            //where e.Code in ['MONTHLY_SICK_LEAVE_ACCRUAL'] and datetime(e.EffectiveStartDate)<=datetime()<=datetime(e.EffectiveEndDate) 
            //return pt,pr.Id as PersonId,e.Code as ElementCode";

            var query = $@"Select pt.*, pr.""Id"" as PersonId, e.""ElementCode""
                            From cms.""N_PayrollHR_PayrollTransaction"" as pt
                            Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=pt.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" 
                            and e.""ElementCode"" in ('MONTHLY_SICK_LEAVE_ACCRUAL')
                            and e.""EffectiveStartDate""::DATE<='{DateTime.Today}'::DATE and e.""EffectiveEndDate""::DATE<='{DateTime.Today}'::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where '{minAttendanceDate}'::DATE <= pt.""EffectiveDate""::DATE and pt.""EffectiveDate""::DATE <= '{maxAttendanceDate}'::DATE and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _payrollTransaction.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<bool> CloseTransaction(string Id, bool isClosed)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0,Id:{Id}}) 
            //             set pt.IsTransactionClosed={isClosed} return 1";

            var query = $@"Update cms.""N_PayrollHR_PayrollTransaction"" set ""IsTransactionClosed""={isClosed} where ""Id""='{Id}' ";

            await _payrollTransaction.ExecuteCommand(query, null);
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
                    var cypher = $@"select pt.* from cms.""N_PayrollHR_PayrollTransaction"" as pt 
join public.""LOV"" as lov on lov.""Id""=pt.""ProcessStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where pt.""ReferenceId""='{viewModel.ServiceId }' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'
and lov.""Code"" = 'NotProcessed' and pt.""EffectiveDate""::TIMESTAMP > '{anniversaryStartDate}'
";


                    //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0}) 
                    //                            where pt.ReferenceId ={RefId} and pt.ProcessStatus = 'NotProcessed' and date(datetime(pt.EffectiveDate)) > date(datetime({date}))
                    //                            return pt";

                    //var prms = new Dictionary<string, object>
                    //{
                    //    { "RefId", viewModel.Id },
                    //    { "date", anniversaryStartDate },
                    //};

                    var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(cypher, null);
                    if (result.IsNotNull())
                    {
                        foreach (var trans in result)
                        {
                            await _noteBusiness.Delete(trans.NtsNoteId);
                            var result1 = await _queryRepo.ExecuteScalar<bool?>($@" update cms.""N_PayrollHR_PayrollTransaction"" set ""IsDeleted""=false where ""NtsNoteId""='{trans.NtsNoteId}'", null);
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
