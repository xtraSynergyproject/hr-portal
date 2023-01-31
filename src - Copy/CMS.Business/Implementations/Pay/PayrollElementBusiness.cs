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
    public class PayrollElementBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollElementBusiness
    {
        private readonly IRepositoryQueryBase<SalaryInfoViewModel> _salaryInfo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _queryPayBatch;
        private readonly IRepositoryQueryBase<SalaryElementInfoViewModel> _salEleInfo;
        private readonly IServiceProvider _sp;
      
        public PayrollElementBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo, IServiceProvider sp
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IRepositoryQueryBase<SalaryElementInfoViewModel> salEleInfo,
            IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch) : base(repo, autoMapper)
        {
            _salaryInfo = salaryInfo;
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            _queryPayBatch = queryPayBatch;
            _salEleInfo = salEleInfo;
            _sp = sp;
        }

        public async Task CalculateSalaryElement(string personId, string salaryInfoId, double total)
        {
            //var cypher = string.Concat(@"
            //match (pr:HRS_PersonRoot {Id:{PersonId},IsDeleted:0})-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(or:HRS_OrganizationRoot)         
            //match (or)<-[:R_LegalEntity_OrganizationRoot]-(le:ADM_LegalEntity)
            //return le");
            var cypher = string.Concat($@"select le.* from cms.""N_CoreHR_HRPerson"" as p join
cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRDepartment"" as or on or.""Id""=a.""DepartmentId"" and or.""IsDeleted""=false and or.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LegalEntity"" as le on le.""Id""=or.""LegalEntityId"" as le.""IsDeleted""=false and le.""CompanyId""='{_repo.UserContext.CompanyId}'
where p.""Id""='{personId} and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}''
");
            //var prms = new Dictionary<string, object>
            //{
            //    { "PersonId", personId }
            //};
            var salDetail = await _queryRepo.ExecuteQuerySingle<LegalEntityViewModel>(cypher, null);

            var salarystartDate = DateTime.Today;
            var ab = _sp.GetService<IHRCoreBusiness>();
            var assignment = await ab.GetAssignmentByPerson(personId);
            if (assignment != null && assignment.DateOfJoin.IsNotNullAndNotEmpty())
            {
                salarystartDate = assignment.DateOfJoin.ToSafeDateTime();
            }

            if (salDetail != null)
            {
                var basicCode = "BASIC";
                var houseCode = "HOUSING";
                var transCode = "TRANSPORT";
                double totalSal = 0.0;

                if (salDetail.BasicSalaryPercentage != 0 && salDetail.HousingAllowancePercentage != 0 && salDetail.TransportAllowancePercentage != 0)
                {
                    var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
                    var _noteBusiness = _sp.GetService<INoteBusiness>();
                    var pay = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", houseCode);
                    var sal = await _tableMetadataBusiness.GetTableDataByColumn("SalaryInfo", "", "Id", salaryInfoId);
                    var houseSal = total * (salDetail.HousingAllowancePercentage / 100.00);
                    houseSal = Math.Round(houseSal);
                    if (pay.IsNotNull())
                    {                       
                        var housemodel = new SalaryElementInfoViewModel
                        {
                            ElementId = pay["Id"].ToString(),
                            SalaryInfoId = sal["NtsNoteId"].ToString(),
                            Amount = houseSal,
                            EffectiveStartDate = salarystartDate,
                            EffectiveEndDate = DateTime.MaxValue,
                        };
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                        noteTempModel.TemplateCode = "SalaryElementInfo";
                        noteTempModel.ParentNoteId = sal["NtsNoteId"].ToString();
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);                       
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(housemodel);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";                       
                        var result = await _noteBusiness.ManageNote(notemodel);

                    }
                    //var house = _elementBusiness.GetActiveSingle(x => x.Code == houseCode);                   
                    //Create(housemodel);

                    var trans = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", transCode);
                    // var trans = _elementBusiness.GetActiveSingle(x => x.Code == transCode);
                    var transSal = total * (salDetail.TransportAllowancePercentage / 100.00);
                    transSal = Math.Round(transSal);
                    if (trans.IsNotNull())
                    {                       
                        var transmodel = new SalaryElementInfoViewModel
                        {
                            ElementId = trans["Id"].ToString(),
                            SalaryInfoId = sal["NtsNoteId"].ToString(),
                            Amount = transSal,
                            EffectiveStartDate = salarystartDate,
                            EffectiveEndDate = DateTime.MaxValue,
                        };
                        // Create(transmodel);
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                        noteTempModel.TemplateCode = "SalaryElementInfo";
                        noteTempModel.ParentNoteId = sal["NtsNoteId"].ToString();
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transmodel);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusiness.ManageNote(notemodel);
                    }
                    // var basic = _elementBusiness.GetActiveSingle(x => x.Code == basicCode);
                    var basic = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", basicCode);
                    // var trans = _elementBusiness.GetActiveSingle(x => x.Code == transCode);
                    if (basic.IsNotNull())
                    {
                        var basicSal = total * (salDetail.BasicSalaryPercentage / 100.00);
                        basicSal = Math.Round(basicSal);
                        totalSal = basicSal + houseSal + transSal;

                        var different = total - totalSal;

                        var basicmodel = new SalaryElementInfoViewModel
                        {
                            ElementId = basic["Id"].ToString(),
                            SalaryInfoId = sal["NtsNoteId"].ToString(),
                            Amount = basicSal + different,
                            EffectiveStartDate = salarystartDate,
                            EffectiveEndDate = DateTime.MaxValue,
                        };
                        // Create(basicmodel);
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                        noteTempModel.TemplateCode = "SalaryElementInfo";
                        noteTempModel.ParentNoteId = sal["NtsNoteId"].ToString();
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(basicmodel);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusiness.ManageNote(notemodel);
                    }
                        
                }
            }
        }

        public async Task DeleteSalaryInfo(string id)
        {
            var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();           
            var sal = await _tableMetadataBusiness.GetTableDataByColumn("SalaryInfo", "", "Id", id);
            if (sal!=null) 
            {
                var query = $@"update  cms.""N_PayrollHR_SalaryInfo"" set ""IsDeleted""=false where ""NtsNoteId""='{id}'";
                await _queryRepo.ExecuteCommand(query, null);
                await _noteBusiness.Delete(sal["NtsNoteId"].ToString());
            }
        }

        public async Task<List<SalaryElementInfoViewModel>> GetAllSalaryElementInfo()
        {
            //      var cypher = string.Concat(@"
            //match(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      <-[:R_SalaryInfoRoot_PersonRoot]-(sir:PAY_SalaryInfoRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(sir)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(sei:PAY_SalaryElementInfo{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(sei)-[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //      return sei,pr.Id as PersonId,er.Id as ElementId,e.ElementType as ElementType,e.ElementEntryType as ElementEntryType
            //      ,e.ElementCategory as ElementCategory,e.ElementClassification as ElementClassification
            //      ,e.EffectiveStartDate as ElementEffectiveStartDate,e.EffectiveEndDate as ElementEffectiveEndDate");

            var cypher = string.Concat($@"Select sei.*,p.""Id"" as PersonId,e.""Id"" as ElementId,e.""ElementType"" as ElementType,e.""ElementEntryType"" as ElementEntryType
            ,e.""ElementCategory"" as ElementCategory,e.""ElementClassification"" as ElementClassification
            ,e.""EffectiveStartDate"" as ElementEffectiveStartDate,e.""EffectiveEndDate"" as ElementEffectiveEndDate
 
From cms.""N_CoreHR_HRPerson"" as p
                            Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=p.""Id"" and si.""IsDeleted""=false and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
");
            var list = await _queryRepo.ExecuteQueryList<SalaryElementInfoViewModel>(cypher, null);
            return list.ToList();
        }

        public async Task<List<IdNameViewModel>> GetAllUserSalary()
        {
            //      var cypher = string.Concat(@"
            //match(psr:PAY_SalaryInfoRoot)<-[psrr:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo{ IsDeleted: 0,CompanyId: {CompanyId} })
            //      match(ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{ IsDeleted: 0,CompanyId:{CompanyId} })<-[:R_ElementRoot]-(e:PAY_Element{IsLatest:true, IsDeleted: 0,CompanyId: {CompanyId} })
            //      match(psr)-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo)
            //      match(psr)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot)
            //      match(pr)<-[:R_User_PersonRoot]-(u:ADM_User)
            //      where not e.Code in['GOSI_EMP_KSA','GOSI_COMP_KSA','GOSI_COMP_NON_KSA']
            //      return u.Id as Id, toFloat(SUM(ps.Amount)) as Code");

            //var prms = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "Status", StatusEnum.Active.ToString() },
            //};
            var query = $@"Select u.""Id"" as Id,coalesce(SUM(CAST(sei.""Amount"" as Double PRECISION)),0.0) as Code 
From cms.""N_CoreHR_HRPerson"" as pr
                            Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and pr.""IsDeleted""=false  and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId"" and n.""IsDeleted""=false and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id""   and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'                   
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId""  and e.""IsDeleted""=false  and e.""CompanyId""='{_repo.UserContext.CompanyId}'  
                             and e.""ElementCode"" not in ('GOSI_EMP_KSA','GOSI_COMP_KSA','GOSI_COMP_NON_KSA')
 Join public.""User"" as u on u.""Id""=pr.""UserId""  and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  group by u.""Id"",sei.""Amount""   
                             ";
            var totalSalary = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return totalSalary.ToList();
        }

        public async Task<double> GetBasicSalary(string userId, DateTime? asofDate = null)
        {
            asofDate = asofDate ?? DateTime.Today;
            //var cypher = string.Concat(@"match(u:ADM_User{Id:{UserId},IsDeleted: 0,Status:'Active'})-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo)
            //where si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate
            //match (psr)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo{IsDeleted: 0,Status:'Active'})
            //where ps.EffectiveStartDate <= {ESD} <= ps.EffectiveEndDate
            //match (ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{IsDeleted: 0,Status:'Active'})
            //<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0,Status:'Active'})
            //where e.EffectiveStartDate <= {ESD} <= e.EffectiveEndDate and e.Code in['BASIC']
            //return toFloat(SUM(ps.Amount))");

            //var prms = new Dictionary<string, object>
            //{
            //    { "UserId", userId },
            //    { "ESD", asofDate },
            //};
            var cypher = string.Concat($@"select sum(ps.""Amount"":: Double Precision)
from public.""User"" as u 
join cms.""N_CoreHR_HRPerson"" as pr on pr.""UserId""=u.""Id"" and u.""IsDeleted""=false and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as pn on pn.""Id""=si.""NtsNoteId""  and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on pn.""Id""=n.""ParentNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryElementInfo"" as ps on ps.""NtsNoteId""=n.""Id"" and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
and ps.""EffectiveStartDate""::Date <= '{asofDate}'::Date and '{asofDate}'::Date <= ps.""EffectiveEndDate""::Date
join cms.""N_PayrollHR_PayrollElement"" as e on ps.""ElementId""=e.""Id"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
and e.""EffectiveStartDate""::Date <= '{asofDate}'::Date  and '{asofDate}'::Date<= e.""EffectiveEndDate""::Date
where u.""Id""='{userId} and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
");
            var basicSalary =await _queryRepo.ExecuteScalar<double>(cypher, null);
            return basicSalary;
        }

        public async Task<List<ElementViewModel>> GetElementListForPayrollRun(DateTime asofDate)
        {
            //var prms = new Dictionary<string, object>();
            //prms.AddIfNotExists("Status", StatusEnum.Active);
            //prms.AddIfNotExists("CompanyId", CompanyId);
            //prms.AddIfNotExists("ESD", asofDate);
            //var cypher = @"match(er:PAY_ElementRoot)
            //    optional match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: {CompanyId} })
            //    where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate
            //    return e,er.Id as ElementId";
             var cypher =$@"Select e.*,e.""Id"" as ElementId
              from cms.""N_PayrollHR_PayrollElement"" as e where   e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo.ExecuteQueryList<ElementViewModel>(cypher, null);
        }
        public async Task<ElementViewModel> GetPayrollElementById(string Id)
        {
           
            var cypher = $@"Select e.*,e.""Id"" as ElementId
              from cms.""N_PayrollHR_PayrollElement"" as e where e.""Id""='{Id}' and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo.ExecuteQuerySingle<ElementViewModel>(cypher, null);
        }
        public async Task<List<IdNameViewModel>> GetPayrollDeductionElement()
        {

            var cypher = $@"Select e.""ElementName"" as Name,e.""Id"" as Id
              from cms.""N_PayrollHR_PayrollElement"" as e where e.""ElementClassification""='Deduction' and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo.ExecuteQueryList<IdNameViewModel>(cypher, null);
        }
        public async Task<SalaryInfoViewModel> GetEligiblityForEOS(string userId)
        {
            var cypher = string.Concat($@"Select coalesce(si.""IsEmployeeEligibleForAirTicketsForSelf"",'false') as IsEligibleForAirTicketForSelf,
            coalesce(si.""IsEmployeeEligibleForAirTicketsForDependants"",'false') as IsEligibleForAirTicketForDependant		
from cms.""N_PayrollHR_SalaryInfo"" as si join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""IsDeleted""=false and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}' and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
");
            //var cypher = string.Concat(@"
            //match(si:PAY_SalaryInfo{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})-
            //[:R_SalaryInfoRoot]->(sir:PAY_SalaryInfoRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //where  si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate
            //match(sir)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{Id}})
            //return coalesce(si.IsEligibleForAirTicketForSelf,false) as IsEligibleForAirTicketForSelf,
            //coalesce(si.IsEligibleForAirTicketForDependant,false) as IsEligibleForAirTicketForDependant		   
            //");

            //var prms = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "Status", StatusEnum.Active.ToString() },
            //    { "Id", userId },
            //     { "ESD", DateTime.Now.ApplicationNow().Date}
            //};
            return await _queryRepo.ExecuteQuerySingle<SalaryInfoViewModel>(cypher, null);
        }

        public async  Task<SalaryInfoViewModel> GetEligiblityForTickets(string userId)
        {
            var cypher = string.Concat($@"Select si.* from
   cms.""N_PayrollHR_SalaryInfo"" as si join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=si.""PersonId"" and p.""IsDeleted""=false and si.""IsDeleted""=false
join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}' and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
");
            //var cypher = string.Concat(@"
            //match(si:PAY_SalaryInfo{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})-
            //[:R_SalaryInfoRoot]->(sir:PAY_SalaryInfoRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //where  si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate
            //match (sir)-[:R_SalaryInfoRoot_PersonRoot]->(pr:HRS_PersonRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{Id}})
            //return si");

            //    var prms = new Dictionary<string, object>
            //    {
            //        { "CompanyId", CompanyId },
            //        { "Status", StatusEnum.Active.ToString() },
            //        { "Id", UserId },
            //        { "ESD", DateTime.Now.ApplicationNow().Date },
            //        { "EED", DateTime.Now.ApplicationNow().Date }
            //};
            return await _queryRepo.ExecuteQuerySingle<SalaryInfoViewModel>(cypher, null);
        }

        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoForPayrollRun(PayrollRunViewModel viewModel)
        {
            //      var cypher = string.Concat(@"match (r:PAY_PayrollRun{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{Id}})
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

            //var prms = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "Status", StatusEnum.Active.ToString() },
            //    { "ESD", viewModel.PayrollEndDate },
            //    { "Id", viewModel.Id },
            //};
            var cypher = string.Concat($@"select sei.*,pr.""Id"" as PersonId,e.""Id"" as ElementId,e.""ElementType"" as ElementType,e.""ElementEntryType"" as ElementEntryType
                 ,e.""ElementCategory"" as ElementCategory,e.""ElementClassification"" as ElementClassification
from cms.""N_PayrollHR_PayrollRun"" as r 
 join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PayrollRunId""=r.""Id"" and prp.""IsDeleted""=false    and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=prp.""PersonId"" and pr.""IsDeleted""=false  and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as pn on pn.""Id""=si.""NtsNoteId""  and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on pn.""Id""=n.""ParentNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
and sei.""EffectiveStartDate""::Date <= '{DateTime.Now}'::Date and '{DateTime.Now}'::Date <= sei.""EffectiveEndDate""::Date
join cms.""N_PayrollHR_PayrollElement"" as e on sei.""ElementId""=e.""Id"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
--and e.""EffectiveStartDate""::Date <= '{DateTime.Now}'::Date and '{DateTime.Now}'::Date <= e.""EffectiveEndDate""::Date
where r.""Id""='{viewModel.Id}' and r.""IsDeleted""=false and r.""CompanyId""='{_repo.UserContext.CompanyId}'
");
            var list = await _queryRepo.ExecuteQueryList<SalaryElementInfoViewModel>(cypher, null);
            return list.ToList();
        }

        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByNodeId(string nodeId)
        {

            //var match = @"match(n:PAY_SalaryElementInfo{Id:{Id},IsDeleted: 0})-[:R_SalaryElementInfo_SalaryInfoRoot]-> 
            //(sir:PAY_SalaryInfoRoot)
            //match (sir)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo{ IsDeleted: 0 })
            //-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{ IsDeleted: 0})
            //match(pe)<-[:R_SalaryElementInfo_ElementRoot]-(n)
            //return ps";
            var match = $@"select ps.* 
from cms.""N_PayrollHR_SalaryElementInfo"" as n
join public.""NtsNote"" as note on n.""NtsNoteId""=note.""Id""  and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
 join cms.""N_PayrollHR_SalaryInfo"" as sir on sir.""NtsNoteId""=note.""Id"" and sir.""IsDeleted""=false and sir.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n1 on n1.""ParentNoteId""=sir.""NtsNoteId""  and n1.""IsDeleted""=false and n1.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryElementInfo"" as ps on ps.""NtsNoteId""=n1.""Id"" and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=ps.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
where n.""Id""='{nodeId}' and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            return await _queryRepo.ExecuteQueryList<SalaryElementInfoViewModel>(match, null);
        }

        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByUser(string userId)
        {
            var asofDate = DateTime.Today;
            var cypher = string.Concat($@"Select ps.*,e.""Id"" as ElementId,e.""Code"" as ElementCode
from public.""User"" as u 
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'
  join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId""  and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryElementInfo"" as ps on ps.""NtsNoteId""=n.""Id"" and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=ps.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
and e.""EffectiveStartDate""::TIMESTAMP::DATE<='{asofDate}'::TIMESTAMP::DATE and e.""EffectiveEndDate""::TIMESTAMP::DATE >='{asofDate}'::TIMESTAMP::DATE
");
            //var cypher = string.Concat(@"match(u:ADM_User{Id:{UserId},IsDeleted: 0,Status:'Active'})-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo)
            //where si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate
            //match (psr)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo{IsDeleted: 0,Status:'Active'})
            //where ps.EffectiveStartDate <= {ESD} <= ps.EffectiveEndDate
            //match (ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{IsDeleted: 0,Status:'Active'})
            //<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0,Status:'Active'})
            //where e.EffectiveStartDate <= {ESD} <= e.EffectiveEndDate
            //return ps,e.Id as ElementId,e.Code as ElementCode");

            //var prms = new Dictionary<string, object>
            //{
            //    { "UserId", userId },
            //    { "ESD", asofDate },
            //};
            var result = await _queryRepo.ExecuteQueryList<SalaryElementInfoViewModel>(cypher, null);
            return result;
        }
        public async Task<SalaryElementInfoViewModel> GetSalaryElementInfoListByUserAndELement(string personId,string elementId)
        {
           
            var cypher = string.Concat($@"Select ps.*,e.""Id"" as ElementId,e.""ElementCode"" as ElementCode
from 
cms.""N_CoreHR_HRPerson"" as p 
  join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=p.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId""  and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryElementInfo"" as ps on ps.""NtsNoteId""=n.""Id"" and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=ps.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
where e.""Id""='{elementId}' and p.""Id""='{personId}' and p.""IsDeleted""=false and  p.""CompanyId""='{_repo.UserContext.CompanyId}'
");
            var result = await _queryRepo.ExecuteQuerySingle<SalaryElementInfoViewModel>(cypher, null);
            return result;
        }
        public async Task<double> GetUserOneDaySalary(string userId, DateTime? asofDate = null)
        {
            // throw new NotImplementedException();
            var res = await GetUserSalary(userId, asofDate ?? DateTime.Today);
            var list = res.PayrollDailyAmountAsPerWorkingDays();
            return list;
        }

   

        public async Task<double> GetUserSalary(string userId, DateTime? asofDate = null)
        {
            asofDate = asofDate ?? DateTime.Today;           

            var query = $@"Select coalesce(SUM(CAST(sei.""Amount"" as Double PRECISION)),0.0) as Amount From cms.""N_CoreHR_HRPerson"" as pr
                            Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and  si.""IsDeleted""=false  and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId"" and  n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""EffectiveStartDate""::TIMESTAMP::DATE <='{asofDate}'::TIMESTAMP::DATE
                            and sei.""EffectiveEndDate""::TIMESTAMP::DATE>='{asofDate}'::TIMESTAMP::DATE
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" 
                            and e.""ElementCode"" not in ('GOSI_EMP_KSA','GOSI_COMP_KSA','GOSI_COMP_NON_KSA') and  e.""IsDeleted""=false  and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pr.""UserId""='{userId}' and  pr.""IsDeleted""=false  and pr.""CompanyId""='{_repo.UserContext.CompanyId}'";
            
            var totalSalary =  await _salEleInfo.ExecuteQuerySingle(query, null);
            if (totalSalary!=null) 
            {
                return totalSalary.Amount;
            }
            return 0.0;
        }
    }
}
