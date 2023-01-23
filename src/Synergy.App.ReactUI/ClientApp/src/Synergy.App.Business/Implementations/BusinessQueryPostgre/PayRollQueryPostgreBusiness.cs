using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class PayRollQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayRollQueryBusiness
    {
        IUserContext _uc;
        public readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        public PayRollQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
        }

        public async Task<List<IdNameViewModel>> GetPayGroupList()
        {
            string query = $@"select p.""Id"" as Id ,p.""Name"" as Name
                            from cms.""N_PayrollHR_PayrollGroup"" as p  
                           where p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<IdNameViewModel>> GetPayCalenderList()
        {
            string query = $@"select p.""Id"" as Id ,p.""Name"" as Name
                            from cms.""N_PayrollHR_PayrollCalendar"" as p                        
                             where p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<IdNameViewModel>> GetPayBankBranchList()
        {
            string query = $@"select p.""Id"" as Id ,p.""BranchName"" as Name
                            from cms.""N_PayrollHR_BankBranch"" as p                        
                            where p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<IdNameViewModel>> GetSalaryElementIdName()
        {
            var query = $@"SELECT B.""ElementName"" as Name, B.""Id"" as Id
    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_PayrollElement"" B on  N.""Id"" =B.""NtsNoteId"" and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
             where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<List<SalaryInfoViewModel>> GetSalaryInfoDetails(string salaryInfoId)
        {
            var query = "";
            if (salaryInfoId.IsNotNullAndNotEmpty())
            {
                query = $@"SELECT B.*, LOV.""Name"" as StatusName,p.""Id"" as PersonId ,CONCAT( p.""FirstName"",' ',p.""LastName"") as PersonName,
p.""PersonNo"" as PersonNo,p.""IqamahNoNationalId"" as SponsorshipNo,pay.""Name"" as PayGroupName,N.""Id"" as NoteId,pay.""Id"" as PayGroupId
,pc.""Name"" as PayrollCalendarName,pb.""BranchName"" as PayrollBankBranchName
    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_SalaryInfo"" B on  N.""Id"" =B.""NtsNoteId""  and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_PayrollHR_PayrollGroup"" as pay on pay.""Id""=B.""PayGroupId"" and  pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=B.""PersonId"" and  p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_PayrollCalendar"" as pc on b.""PayCalendarId""=pc.""Id"" and pc.""IsDeleted""=false
left join cms.""N_PayrollHR_BankBranch"" as pb on b.""BankBranchId"" = pb.""Id"" and pb.""IsDeleted"" = false
  --inner join public.""LOV"" as payLOV on B.""PaymentModeId""=payLOV.""Id""
inner join public.""LOV"" as LOV on N.""NoteStatusId""=LOV.""Id"" and  LOV.""IsDeleted""=false  where B.""Id""='{salaryInfoId}' and  N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'";
            }
            else
            {
                query = $@"SELECT B.*, LOV.""Name"" as StatusName,p.""Id"" as PersonId ,CONCAT( p.""FirstName"",' ',p.""LastName"") as PersonName,
p.""PersonNo"" as PersonNo,p.""IqamahNoNationalId"" as SponsorshipNo,pay.""Name"" as PayGroupName,N.""Id"" as NoteId
    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_SalaryInfo"" B on  N.""Id"" =B.""NtsNoteId""  and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_PayrollHR_PayrollGroup"" as pay on pay.""Id""=B.""PayGroupId"" and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=B.""PersonId""  and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""LOV"" as LOV on N.""NoteStatusId""=LOV.""Id"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_repo.UserContext.CompanyId}'
 where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'";
            }

            var queryData = await _queryRepo.ExecuteQueryList<SalaryInfoViewModel>(query, null);

            //var list = new List<SalaryInfoViewModel>();

            //list = queryData.Select(x => new SalaryInfoViewModel
            //{ 
            //    Id = x.Id, 
            //    PersonId=x.PersonId,
            //    PersonName = x.PersonName,
            //    SponsorshipNo = x.SponsorshipNo,
            //    BankAccountNumber = x.BankAccountNumber,
            //    BankIBanNumber = x.BankIBanNumber,
            //    PayGroupName = x.PayGroupName,
            //    StatusName = x.StatusName,
            //    NoteId = x.NoteId,
            //    PayCalendarId = x.PayCalendarId,
            //    PayGroupId = x.PayGroupId,
            //    PayGroupId = x.PayGroupId,
            //    PayGroupId = x.PayGroupId,
            //    PayGroupId = x.PayGroupId,
            //    PayGroupId = x.PayGroupId,
            //}).ToList();
            return queryData;


        }
        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoDetails(string elementId, string salaryInfoId,string salaryElementId=null)
        {
            var query = "";
            if (elementId.IsNotNullAndNotEmpty())
            {
                query = $@"SELECT B.*,N.""Id"" as NoteId

    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_SalaryElementInfo"" B on  N.""Id"" =B.""NtsNoteId"" and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
 where B.""NtsNoteId""='{elementId}' and N.""ParentNoteId""='{salaryInfoId}' and N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'";
            }
            if (salaryElementId.IsNotNullAndNotEmpty())
            {
                query = $@"SELECT B.*,N.""Id"" as NoteId

    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_SalaryElementInfo"" B on  N.""Id"" =B.""NtsNoteId"" and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
 where B.""NtsNoteId""='{elementId}' and N.""ParentNoteId""='{salaryInfoId}' and B.""Id""='{salaryElementId}' and N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'";
            }
            else
            {
                query = $@"SELECT B.*,E.""ElementName"" as ElementName, N.""Id"" as NoteId
    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_SalaryElementInfo"" B on  N.""Id"" =B.""NtsNoteId"" and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_PayrollHR_PayrollElement"" E on  B.""ElementId"" =E.""Id"" and E.""IsDeleted""=false and E.""CompanyId""='{_repo.UserContext.CompanyId}'
where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}' #PARENTNOTEWHERE#
";
                var parentnotewhere = "";
                if (salaryInfoId.IsNotNullAndNotEmpty())
                {
                    parentnotewhere = $@" and N.""ParentNoteId""='{salaryInfoId}' ";
                }
                query = query.Replace("#PARENTNOTEWHERE#", parentnotewhere);
            }

            var queryData = await _queryRepo.ExecuteQueryList<SalaryElementInfoViewModel>(query, null);


            return queryData;


        }
        public async Task DeleteSalaryElement(string NoteId)
        {
            var query = $@"update  cms.""N_PayrollHR_SalaryElementInfo"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<List<IdNameViewModel>> GetPayrollGroupList()
        {
            var query = $@"select ""Id"", ""Name"" from cms.""N_PayrollHR_PayrollGroup"" where ""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<PayrollGroupViewModel> GetPayrollGroupById(string payGroupId)
        {
            var query = $@"select * from cms.""N_PayrollHR_PayrollGroup"" where ""Id""='{payGroupId}'  and ""IsDeleted""=false  and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryRepo.ExecuteQuerySingle<PayrollGroupViewModel>(query, null);
            return result;
        }
        public async Task<List<PayrollBatchViewModel>> ViewModelList(string PayrollBatchId)
        {

            //var match = string.Concat(
            //    @"match (pr:PAY_Payroll{IsDeleted: 0,CompanyId: { CompanyId} })
            //    match(pr)-[:R_Payroll_PayrollGroup]->(pg:PAY_PayrollGroup{ IsDeleted: 0,CompanyId: { CompanyId} }) 
            //    match(pr)-[:R_Payroll_ExecutedBy_User]->(u:ADM_User{IsDeleted: 0,CompanyId: { CompanyId} })               
            //    optional match (pr)<-[:R_PayrollRun_Payroll]-(prr:PAY_PayrollRun{ IsDeleted: 0,CompanyId: { CompanyId}}) 
            //    optional match(pr)-[:R_Payroll_LegalEntity_OrganizationRoot]-(prler:HRS_OrganizationRoot{Id:{LegalEntityId}})
            //    <-[:R_OrganizationRoot]-(prle:HRS_Organization{IsLatest:true})
            //    with pr,pg,u,prler,prle,prr");

            var query = $@"select pr.*, pg.""Id"" as PayrollGroupId, pg.""Name"" as PayrollGroupName, prr.""Id"" as PayrollRunId,
                            substring(pr.""YearMonth"",1,4) as Year, substring(pr.""YearMonth"",5,2) as Month
                            from cms.""N_PayrollHR_PayrollBatch"" as pr
                            join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=pr.""PayrollGroupId"" and  pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""User"" as u on u.""Id""= pr.""ExecutedBy""  and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            left join cms.""N_PayrollHR_PayrollRun"" as prr on pr.""Id""=prr.""PayrollBatchId"" and prr.""IsDeleted""=false  and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pr.""Id""='{PayrollBatchId}' and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var result = await _queryRepo.ExecuteQueryList<PayrollBatchViewModel>(query, null);

            return result;
        }
        public async Task<PayrollBatchViewModel> GetSingleById(string payrollBatchId)
        {
            var query = @$"Select * from cms.""N_PayrollHR_PayrollBatch"" where ""Id""='{payrollBatchId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryRepo.ExecuteQuerySingle<PayrollBatchViewModel>(query, null);
            return result;
        }
        public async Task<PayrollBatchViewModel> IsPayrollExist(string payGroupId, string yearmonth)
        {
            var query = @$"Select * from cms.""N_PayrollHR_PayrollBatch"" where ""PayrollGroupId""='{payGroupId}' and ""YearMonth""='{yearmonth}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var result = await _queryRepo.ExecuteQuerySingle<PayrollBatchViewModel>(query, null);
            return result;
        }


        public async Task<CalendarViewModel> GetCalendarDetailsById(string id)
        {
            string query = $@"select *, ""NtsNoteId"" as NoteId from cms.""N_PayrollHR_PayrollCalendar"" where ""Id""='{id}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQuerySingle<CalendarViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CalendarViewModel>> GetCalendarListData()
        {
            var query = $@"SELECT * FROM cms.""N_PayrollHR_PayrollCalendar"" where ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<CalendarViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CalendarHolidayViewModel>> GetCalendarHolidayData(string calendarId)
        {
            var query = $@"SELECT *,ch.""NtsNoteId"" as NoteId FROM  public.""NtsNote"" N
                            inner join cms.""N_PayrollHR_CalendarHoliday"" ch on N.""Id"" =ch.""NtsNoteId"" and ch.""CalendarId""='{calendarId}' and ch.""IsDeleted""=false and ch.""CompanyId""='{_repo.UserContext.CompanyId}'
                              where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<CalendarHolidayViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CalendarHolidayViewModel>> GetCalendarHolidayDatawithmonthYear(string calendarId, int Year, int month)
        {
            var query = $@"SELECT *	FROM cms.""N_PayrollHR_CalendarHoliday"" where  extract(year from CAST(""ToDate"" AS DATE))={Year}
    and extract(month from CAST(""ToDate"" AS DATE))= {month} and ""CalendarId"" = '{calendarId}'

    and ""IsDeleted"" = false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<CalendarHolidayViewModel>(query, null);
            return queryData;
        }
        public async Task<CalendarHolidayViewModel> GetCalendarHolidayDetailsById(string calHolidayId)
        {
            string query = $@"select * from cms.""N_PayrollHR_CalendarHoliday"" where ""Id""='{calHolidayId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQuerySingle<CalendarHolidayViewModel>(query, null);
            return queryData;
        }
        public async Task DeleteCalendarHoliday(string NoteId)
        {
            var query = $@"update  cms.""N_PayrollHR_CalendarHoliday"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<List<CalendarHolidayViewModel>> CheckHolidayNameWithCalendar(string calId, string holName)
        {
            string query = @$"SELECT * FROM cms.""N_PayrollHR_CalendarHoliday"" where ""HolidayName""='{holName}' and ""CalendarId""='{calId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQueryList<CalendarHolidayViewModel>(query, null);
            return result;
        }
        public async Task<List<CalendarViewModel>> CheckCalendarWithLegalEntityId(string legalEId, string name, string code)
        {
            string query = @$"SELECT * FROM cms.""N_PayrollHR_PayrollCalendar"" where ""LegalEntityId""='{legalEId}' and (""Name""='{name}' or ""Code""='{code}') and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQueryList<CalendarViewModel>(query, null);
            return result;
        }


        public async Task<LegalEntityViewModel> GetPersonDetails(string personId)
        {
            var cypher = string.Concat($@"select le.* from cms.""N_CoreHR_HRPerson"" as p join
cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRDepartment"" as or on or.""Id""=a.""DepartmentId"" and or.""IsDeleted""=false and or.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LegalEntity"" as le on le.""Id""=or.""LegalEntityId"" as le.""IsDeleted""=false and le.""CompanyId""='{_repo.UserContext.CompanyId}'
where p.""Id""='{personId} and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}''
");

            var result = await _queryRepo.ExecuteQuerySingle<LegalEntityViewModel>(cypher, null);
            return result;
        }
        public async Task DeleteSalaryInfo(string id)
        {
            var query = $@"update  cms.""N_PayrollHR_SalaryInfo"" set ""IsDeleted""=false where ""NtsNoteId""='{id}'";
            await _queryRepo.ExecuteCommand(query, null);
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
            return list;
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
            var totalSalary = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return totalSalary;
        }
        public async Task<double> GetBasicSalary(string userId, DateTime? asofDate = null)
        {

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
            var basicSalary = await _queryRepo.ExecuteScalar<double>(cypher, null);
            return basicSalary;
        }
        public async Task<SalaryElementInfoViewModel> GetUserSalary(string userId, DateTime? asofDate = null)
        {

            var query = $@"Select coalesce(SUM(CAST(sei.""Amount"" as Double PRECISION)),0.0) as Amount From cms.""N_CoreHR_HRPerson"" as pr
                            Join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and  si.""IsDeleted""=false  and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""NtsNote"" as n on n.""ParentNoteId""=si.""NtsNoteId"" and  n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=n.""Id"" and sei.""EffectiveStartDate""::TIMESTAMP::DATE <='{asofDate}'::TIMESTAMP::DATE
                            and sei.""EffectiveEndDate""::TIMESTAMP::DATE>='{asofDate}'::TIMESTAMP::DATE
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" 
                            and e.""ElementCode"" not in ('GOSI_EMP_KSA','GOSI_COMP_KSA','GOSI_COMP_NON_KSA') and  e.""IsDeleted""=false  and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pr.""UserId""='{userId}' and  pr.""IsDeleted""=false  and pr.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var totalSalary = await _queryRepo.ExecuteQuerySingle<SalaryElementInfoViewModel>(query, null);
            return totalSalary;
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
            var cypher = $@"Select e.*,e.""Id"" as ElementId
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

        public async Task<SalaryInfoViewModel> GetEligiblityForTickets(string userId)
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
            return list;
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

        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByUser(string userId, DateTime asofDate)
        {
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
        public async Task<SalaryElementInfoViewModel> GetSalaryElementInfoListByUserAndELement(string personId, string elementId)
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



        // PayrollRunBusiness Queries
        public async Task<PayrollRunViewModel> GetPayrollSingleDataById(string payrollRunId)
        {
            var query = @$"Select *,""NtsNoteId"" as NoteId from cms.""N_PayrollHR_PayrollRun"" 
                            where ""Id""='{payrollRunId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";


            var result = await _queryRepo.ExecuteQuerySingle<PayrollRunViewModel>(query, null);
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
            var result = await _queryRepo.ExecuteQueryList<MandatoryDeductionElementViewModel>(full_query, null);
            return result;

        }

        public async Task<List<IdNameViewModel>> GetPayRollElementName()
        {
            //edit
            var query = $@"Select ""Id"" as Id, ""ElementName"" as Name from cms.""N_PayrollHR_PayrollElement""
                            where ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<MandatoryDeductionSlabViewModel> GetSlabForMandatoryDeduction(string mandatoryDeductionId, double amount, DateTime asofDate)
        {
            var query = @$"Select * from cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB""
                            where ""MandatoryDeductionId""='{mandatoryDeductionId}' and ""SlabFrom""::DECIMAL<='{amount}'::DECIMAL and""SlabTo""::DECIMAL>='{amount}'::DECIMAL  and ""IsDeleted""=false and ""ESD""::DATE<='{asofDate}'::DATE and ""EED""::DATE>='{asofDate}'::DATE";
            var result = await _queryRepo.ExecuteQuerySingle<MandatoryDeductionSlabViewModel>(query, null);
            return result;
        }

        public async Task<MandatoryDeductionElementViewModel> GetSingleElementEntryById(string id)
        {
            //edit
            var query = $@"Select * from cms.""F_PAY_HR_HR_MANDATORY_DEDUCTION_ELEMENT""
                            where ""Id""='{id}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}' ";
            var result = await _queryRepo.ExecuteQuerySingle<MandatoryDeductionElementViewModel>(query, null);
            return result;
        }

        public async Task DeleteMandatoryDeductionElement(string Id)
        {
            var query = $@"Update cms.""F_PAY_HR_HR_MANDATORY_DEDUCTION_ELEMENT"" set ""IsDeleted""=true where ""Id""='{Id}' and ""CompanyId""='{_repo.UserContext.CompanyId}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task UpdatePayrollById(string payrollRunId, DateTime payrollRunDate, int exeStatus)
        {
            var query = $@"Update cms.""N_PayrollHR_PayrollRun"" set ""PayrollRunDate""='{payrollRunDate}', ""ExecutionStatus""='{exeStatus}'
                        where ""Id""='{payrollRunId}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<PayrollRunViewModel> GetNextPayroll(string payrollRunId)
        {
            var cypher = $@"select pr.*,p.""Id"" as PayrollId,p.""PayrollStartDate"" as PayrollStartDate
                ,p.""PayrollEndDate"" as PayrollEndDate,p.""AttendanceStartDate"" as AttendanceStartDate,p.""AttendanceEndDate"" as AttendanceEndDate
                ,p.""YearMonth"" as YearMonth,p.""RunType"" as RunType
                ,pg.""Id"" as PayrollGroupId ,le.""Id"" as LegalEntityId 
                from cms.""N_PayrollHR_PayrollRun"" as pr
                join cms.""N_PayrollHR_PayrollBatch"" as p on p.""Id""=pr.""PayrollBatchId"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=p.""PayrollGroupId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LegalEntity"" as le on le.""Id""=p.""LegalEntityId""  and le.""IsDeleted""=false and le.""CompanyId""='{_repo.UserContext.CompanyId}'
                where pr.""IsDeleted""=false and pr.""Id""='{payrollRunId}' and pr.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            var queryData = await _queryRepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
            return queryData;
        }

        public async Task SetPayrollExeStatusnState(int stateEnd, int exeStatus, PayrollRunViewModel viewModel)
        {
            var query = $@"Update cms.""N_PayrollHR_PayrollRun"" set ""PayrollStateStart""='{stateEnd}', ""PayrollStateEnd""='{stateEnd}',
                            ""ExecutionStatus""='{exeStatus}' where ""Id""='{viewModel.Id}' ";

            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task SetBatchStatus(int payrollstatus, PayrollBatchViewModel payroll)
        {
            var queryBatch = $@"Update cms.""N_PayrollHR_PayrollBatch"" set ""PayrollStatus""='{payrollstatus}' 
                                    where ""Id""='{payroll.Id}' ";

            await _queryRepo.ExecuteCommand(queryBatch, null);
        }

        public async Task SetPayrollRunStatus(PayrollRunViewModel viewModel)
        {
            var cypher = $@"update cms.""N_PayrollHR_PayrollRun"" set ""ExecutionStatus""='{PayrollExecutionStatusEnum.InProgress}' where ""Id""='{viewModel.Id}'";
            await _queryRepo.ExecuteCommand(cypher, null);
        }

        public async Task<List<PayrollElementRunResultViewModel>> GetStandardElementListForPayrollRunData(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth, string personIds = null)
        {

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

            var result = await _queryRepo.ExecuteQueryList<PayrollElementRunResultViewModel>(match1, null);
            return result;
        }

        public async Task<IList<PayrollRunViewModel>> GetPayrollBatchList()
        {
            var query = @$"Select pr.*, pb.""PayrollStartDate"", pb.""PayrollEndDate"",pb.""PayrollGroupId"",pg.""Name"" as PayrollGroupName
                            from cms.""N_PayrollHR_PayrollRun"" as pr
                            join cms.""N_PayrollHR_PayrollBatch"" as pb on pb.""Id""=pr.""PayrollBatchId"" and pb.""IsDeleted""=false and pb.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_PayrollHR_PayrollGroup"" as pg on pb.""PayrollGroupId""=pg.""Id"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' order by pr.""CreatedDate"" desc ";

            var result = await _queryRepo.ExecuteQueryList<PayrollRunViewModel>(query, null);
            return result;
        }

        public async Task<List<PayrollRunViewModel>> PayrollRunViewModelList(string PayrollRunId)
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

            var result = await _queryRepo.ExecuteQueryList<PayrollRunViewModel>(query, null);

            return result;

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
            return await _queryRepo.ExecuteQueryList<PayrollRunViewModel>(cypher, null);

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

            var querydata = await _queryRepo.ExecuteQueryList<PayrollSalaryElementViewModel>(query, null);
            return querydata;

        }

        public async Task<List<PayrollSalaryElementViewModel>> GetEmployeeListForPayroll(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth)
        {
            var query = $@"Select Distinct p.""PersonFullName"" as PersonName,p.""Id"" as PersonId, d.""DepartmentName"" as OrganizationName, payd.""DepartmentName"" as PayrollOrganizationName, j.""JobTitle"" as JobName,
                        a.""DateOfJoin"" as DateOfJoin, g.""GradeName"" as GradeName, p.""PersonNo"" as PersonNo, prp.""Id"",pg.""Id"" as PayrollGroupId, pg.""Name"" as PayrollGroupName
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
            var querydata = await _queryRepo.ExecuteQueryList<PayrollSalaryElementViewModel>(query, null);
            return querydata;

        }

        public async Task<List<PayrollElementRunResultViewModel>> GetStandardElementListForPayroll(PayrollBatchViewModel payrollVM, string payrollGroupId, string payrollId, string payrollRunId, int yearMonth, string personIds = null)
        {
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

            var querydata = await _queryRepo.ExecuteQueryList<PayrollElementRunResultViewModel>(query, null);
            return querydata;

        }

        public async Task SetPayrollRunStatusnError(PayrollExecutionStatusEnum exceutionStatus, string error, string payrollRunId)
        {
            var cypher1 = $@"update cms.""N_PayrollHR_PayrollRun"" set ""ExecutionStatus""='{exceutionStatus}',""ExecutePayrollError""='{error}' where ""Id""='{payrollRunId}'";
            await _queryRepo.ExecuteCommand(cypher1, null);
        }

        public async Task<List<ElementViewModel>> GetElementsForPayrollRun(DateTime payrollEndDate)
        {
            var cypher = $@"Select * from cms.""N_PayrollHR_PayrollElement"" where e.""EffectiveStartDate""::Date<='{DateTime.Now}'<=e.""EffectiveEndDate""::Date and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            //var cypher = @"match(er:PAY_ElementRoot)
            //         optional match(er)<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: {CompanyId} })
            //         where e.EffectiveStartDate<={ESD}<=e.EffectiveEndDate
            //         return e,er.Id as ElementId";
            return await _queryRepo.ExecuteQueryList<ElementViewModel>(cypher, null);
        }

        public async Task<List<PayrollPersonViewModel>> GetSelectedPersonsForPayrollRun(PayrollRunViewModel viewModel)
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
            , si.""BankAccountNumber"" as BankAccountNo, si.""BankIBanNumber"" as BankIBanNo, si.""PaymentMode"" as PaymentMode, a.""DateOfJoin"" as DateOfJoin,a.""LastWorkingDate"" as LastWorkingDate
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
            var list = await _queryRepo.ExecuteQueryList<PayrollPersonViewModel>(cypher, null);
            return list;
        }

        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoForPayrollRun2(PayrollRunViewModel viewModel)
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


            var list = await _queryRepo.ExecuteQueryList<SalaryElementInfoViewModel>(cypher, null);
            return list;
        }

        public async Task DeleteMandatoryDeductionSlab(string Id)
        {
            var query = $@"Update cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB"" set ""IsDeleted""=true where ""Id""='{Id}' and ""CompanyId""='{_repo.UserContext.CompanyId}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<List<PayrollTransactionViewModel>> GetLoanAccrualDetails(DateTime payrollStartDate, DateTime payrollEndDate, string payRollRunId)
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(cypher, null);
            return result;
        }

        public async Task<List<PayrollTransactionViewModel>> GetPendingLoanAccrualDetails(DateTime payrollStartDate, DateTime payrollEndDate, string payRollRunId, List<string> exculdePersons, string excludePersonText, DateTime LOANDATE)
        {

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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(cypher, null);
            return result;
        }

        public async Task<List<TicketAccrualViewModel>> GetSickLeaveAccrualPersonList(DateTime payrollDate, string payRollRunId)
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
            var result = await _queryRepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);
            //var result = ExecuteCypherList<TicketAccrualViewModel>(cypher, prms);
            return result;
        }
        public async Task<PayrollTransactionViewModel> ManageClosedTransaction(string personId, string elementCode, DateTime payrollEndDate)
        {
            var cypher = string.Concat($@"
            Select pt.* from 
cms.""N_PayrollHR_PayrollTransaction"" as pt 
join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=pt.""PersonId"" and p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=pt.""ElementId"" and e.""IsDeleted""=false and e.""ElementCode""='{elementCode}' and e.""CompanyId""='{_repo.UserContext.CompanyId}'
where pt.""IsTransactionClosed"" ='true' --and e.""EffectiveStartDate"" <= '{DateTime.Now}' and '{DateTime.Now}' <= e.""EffectiveEndDate""
and pt.""EffectiveDate"" = '{payrollEndDate}' and pt.""IsDeleted"" = false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'");
            var closedTransaction = await _queryRepo.ExecuteQuerySingle<PayrollTransactionViewModel>(cypher, null);
            return closedTransaction;
        }

        public async Task<PayrollTransactionViewModel> GetElementDetails(string elementCode)
        {
            var cypher = string.Concat($@"
           SELECT * FROM cms.""N_PayrollHR_PayrollElement""
where ""ElementCode"" = '{elementCode}' and ""IsDeleted"" = false and ""CompanyId""='{_repo.UserContext.CompanyId}'");
            var data = await _queryRepo.ExecuteQuerySingle<PayrollTransactionViewModel>(cypher, null);
            return data;
        }
        public async Task<List<MandatoryDeductionSlabViewModel>> GetSingleSlabById(string mandatoryDeductionId)
        {
            //index
            var query = @$"Select * from cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB""
                            where ""MandatoryDeductionId""='{mandatoryDeductionId}' and ""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQueryList<MandatoryDeductionSlabViewModel>(query, null);
            return result;
        }

        public async Task<MandatoryDeductionSlabViewModel> GetSingleSlabEntryById(string id)
        {
            //edit
            var query = $@"Select * from cms.""F_PAY_HR_MANDATORY_DEDUCTION_SLAB""
                            where ""Id""='{id}' and ""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQuerySingle<MandatoryDeductionSlabViewModel>(query, null);
            return result;
        }
        public async Task<List<MandatoryDeductionSlabViewModel>> ValidateMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model)
        {
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
            var result = await _queryRepo.ExecuteQueryList<MandatoryDeductionSlabViewModel>(query, null);
            return result;
        }

        public async Task<CalendarViewModel> GetCalendarDetails(string calendarId)
        {
            var query = $@"select pc.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
                        where pc.""Id""='{calendarId}' and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var calendarVM = await _queryRepo.ExecuteQuerySingle<CalendarViewModel>(query, null);
            return calendarVM;
        }

        public async Task<List<CalendarHolidayViewModel>> GetHolidayDetails(CalendarViewModel calendarVM)
        {
            var query1 = $@"select h.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
            join cms.""N_PayrollHR_CalendarHoliday"" as h on h.""CalendarId""=pc.""Id"" and pc.""Id""='{ calendarVM.Id}' and h.""IsDeleted""=false and h.""CompanyId""='{_repo.UserContext.CompanyId}'
              where pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'";


            var holidayVM = await _queryRepo.ExecuteQueryList<CalendarHolidayViewModel>(query1, null);
            return holidayVM;
        }

        public async Task<TicketAccrualViewModel> GetEndOfService(string userId)
        {
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

            var result = await _queryRepo.ExecuteQuerySingle<TicketAccrualViewModel>(cypher, null);
            return result;
        }

        public async Task SetPayrollStateEnd(PayrollRunViewModel payroll)
        {
            var cypher1 = $@"update cms.""N_PayrollHR_PayrollRun"" set ""PayrollStateEnd""='{PayrollStateEnum.ExecutePayroll}' where ""Id""='{payroll.Id}'";
            //var cypher = @"match (s:NTS_Service{Id:{serviceId}}-[:R_Service_Reference{ReferenceTypeCode:'PAY_PayrollRun'}]->(pr:PAY_PayrollRun) set pr.PayrollStateEnd={status},pr.PayrollStateEnd={status}";
            await _queryRepo.ExecuteCommand(cypher1, null);
        }

        public async Task<PayrollRunViewModel> GetPayrollRunDataByServiceId(string serviceId)
        {
            var cypher = $@"Select pr.* from 
cms.""N_PayrollHR_PayrollRun"" as pr 
join public.""NtsNote"" as n on n.""Id""=pr.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as s on s.""Id""=n.""ParentServiceId""and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""Id""='{serviceId}'  and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            var payroll = await _queryRepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
            return payroll;
        }

        public async Task<List<string>> GetDistinctRun(int yearMonth)
        {

            var match1 = string.Concat($@" Select distinct pay.""PayRollNo"" as PayRollNo
from cms.""N_PayrollHR_PayrollRun""  as pay where pay.""YearMonth""='{yearMonth}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
                       order by pay.""PayRollNo""");

            var result = await _queryRepo.ExecuteScalarList<string>(match1, null);
            return result;
        }


        public async Task<PayrollRunViewModel> GetPayrollRunByNoteId(string noteId)
        {

            var cypher = $@"Select * from 
cms.""N_PayrollHR_PayrollRun"" where ""NtsNoteId""='{noteId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'
";
            return await _queryRepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
        }
        public async Task<PayrollRunViewModel> GetPayrollRunById(string Id)
        {

            var cypher = $@"Select * from 
cms.""N_PayrollHR_PayrollRun"" where ""Id""='{Id} and ""IsDeleted""=false' and ""CompanyId""='{_repo.UserContext.CompanyId}'
";
            return await _queryRepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
        }

        public async Task<ServiceViewModel> GetPayrollRunService(string payrollRunId)
        {

            var cypher = $@"Select s.* from 
cms.""N_PayrollHR_PayrollRun"" as pr 
join public.""NtsNote"" as n on n.""Id""=pr.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as s on s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
where pr.""Id""='{payrollRunId}' and s.""ServiceStatusCode""<>'SERVICE_STATUS_CANCEL' and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            return await _queryRepo.ExecuteQuerySingle<ServiceViewModel>(cypher, null);
        }

        public async Task<List<TicketAccrualViewModel>> GetTicketEligibleEmployeeDetails(DateTime payrollDate, string payRollRunId)
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


            var result = await _queryRepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);
            return result;
        }

        public async Task<List<PayrollSummaryViewModel>> GetPayrollSummary1()
        {
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

            var result = await _queryRepo.ExecuteQueryList<PayrollSummaryViewModel>(cypher, null);
            return result;
        }

        public async Task<List<PayrollSummaryViewModel>> GetPayrollSummary2()
        {
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

            var result1 = await _queryRepo.ExecuteQueryList<PayrollSummaryViewModel>(cypher1, null);
            return result1;
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
            var list = await _queryRepo.ExecuteQueryList<PayrollBatchViewModel>(cypher, null);
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
            var list = await _queryRepo.ExecuteQueryList<PayrollBatchViewModel>(cypher, null);
            return list;
        }

        public async Task<List<PayrollBatchViewModel>> GetNotPostedPreviousPayrollEmployeeList(string payrollGroupId)
        {

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
            var list = await _queryRepo.ExecuteQueryList<PayrollBatchViewModel>(cypher, null);
            return list;
        }

        public async Task<double?> GetElementSalaryByPerson(DateTime attendanceDate, string PersonId, string SalaryCode)
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
            var result = await _queryRepo.ExecuteScalar<double?>(cypher, null);

            return result;
        }

        public async Task<PayrollRunViewModel> GetNextPayroll(int submi, int inpro, int erro)
        {
            var cypher = $@" Select  r.*,p.""Id"" as PayrollId
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
            var result = await _queryRepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
            return result;
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
            var result = await _queryRepo.ExecuteQueryList<UserListOfValue>(cypher, null);
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
            var result = await _queryRepo.ExecuteQueryList<UserListOfValue>(cypher, null);

            return result;
        }

        public async Task<List<TicketAccrualViewModel>> GetTicketAccrualDetails(DateTime payrollDate, string payRollRunId)
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

            var result = await _queryRepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);

            return result;
        }


        public async Task<List<TicketAccrualViewModel>> GetEOSAccrualDetails(DateTime payrollDate, string payRollRunId, DateTime attendanceStartDate, DateTime attendanceEndDate)
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

            var result = await _queryRepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);
            return result;
        }


        public async Task<List<TicketAccrualViewModel>> GetVacationAccrualDetails(DateTime payrollDate, string payRollRunId)
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
            var result = await _queryRepo.ExecuteQueryList<TicketAccrualViewModel>(cypher, null);

            return result;
        }

        public async Task UpdatePayrollTransaction(LOVViewModel lov, LOVViewModel dlov, PayrollRunViewModel viewModel,/* PayrollRunViewModel payrollRun*/ string personId)
        {
            var cypher = $@"update cms.""N_PayrollHR_PayrollTransaction"" 
                            set ""ProcessStatusId""='{lov.Id}',""PayrollBatchId""='{viewModel.PayrollBatchId}',""PayrollRunId""='{viewModel.Id}'
                            where ""IsDeleted"" = false and ""PersonId"" in ('{personId}')
                            and ""EffectiveDate""::Date<='{viewModel.PayrollEndDate}'::Date and ""ProcessStatusId""='{dlov.Id}'
                        ";
            await _queryRepo.ExecuteCommand(cypher, null);
        }

        public async Task<List<PayrollRunViewModel>> GetPayrollSingleData(PayrollRunViewModel viewModel)
        {
            var query = $@"select pr.*, prp.""PersonId"" as PayrollPersonId from cms.""N_PayrollHR_PayrollRun"" as pr
                            left join cms.""N_PayrollHR_PayrollRunPerson"" as prp on prp.""PayrollRunId"" = pr.""Id"" and prp.""IsDeleted"" = false and prp.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pr.""IsDeleted"" = false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' and pr.""Id"" = '{viewModel.Id}' ";
            var payrollRun = await _queryRepo.ExecuteQueryList<PayrollRunViewModel>(query, null);
            return payrollRun;
        }

        public async Task<List<double>> GetSickLeavesList(LeaveDetailViewModel model, DateTime date, DateTime lastAnniversaryDate)
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

            var sickLeavesList = await _queryRepo.ExecuteScalarList<double>(cypher, null);
            return sickLeavesList;
        }

        public async Task UpdateSalaryEntry(int publish, PayrollRunViewModel viewModel)
        {
            var cypher = $@"update cms.""N_PayrollHR_SalaryEntry"" set ""PublishStatus""='{publish}' where ""PayrollRunId""='{viewModel.Id}'";
            await _queryRepo.ExecuteCommand(cypher, null);
        }

        public async Task UpdateSalaryElementEntry(int publish, PayrollRunViewModel viewModel)
        {
            var cypher = $@"update cms.""N_PayrollHR_SalaryElementEntry"" set ""PublishStatus""='{publish}' where ""PayrollRunId""='{viewModel.Id}'";
            await _queryRepo.ExecuteCommand(cypher, null);
        }

        public async Task UpdatePayrollTransactionByPayrollIdnPersonId(LOVViewModel lov, PayrollRunViewModel viewModel, PayrollRunViewModel payrollRun)
        {
            var cypher = $@"update cms.""N_PayrollHR_PayrollTransaction""  set  ""ProcessStatusId""='{lov.Id}',""ProcessedDate""=null
where ""PayrollRunId""='{viewModel.Id}' and ""PersonId""='{payrollRun.PayrollPersonId}'
                ";
            await _queryRepo.ExecuteCommand(cypher, null);
        }

        public async Task UpdatePayrollBatch(PayrollRunViewModel viewModel, int payrollStatus)
        {
            var cypher = $@"update cms.""N_PayrollHR_PayrollBatch"" set ""PayrollStatus""='{payrollStatus}' where ""Id""='{viewModel.PayrollId}'";
            await _queryRepo.ExecuteCommand(cypher, null);
        }

        public async Task UpdateIsDeleteofPayrollTransaction(PayrollRunViewModel viewModel)
        {
            var cypher = string.Concat($@"update cms.""N_PayrollHR_PayrollTransaction"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}' and ""PostedSource""='3'");
            await _queryRepo.ExecuteScalar<long>(cypher, null);
        }

        public async Task UpdateIdsinPayrollTransaction(LOVViewModel lov, PayrollRunViewModel viewModel)
        {
            var cypher = string.Concat($@"update cms.""N_PayrollHR_PayrollTransaction""
                                    set ""ProcessStatusId""='{lov.Id}', ""PayrollBatchId""=null,""PayrollRunId""=null
                                    where ""PayrollRunId""='{ viewModel.Id}'");
            await _queryRepo.ExecuteCommand(cypher, null);
        }

        public async Task DeleteFromSalaryElementEntry(PayrollRunViewModel viewModel)
        {
            await _queryRepo.ExecuteCommand($@"update cms.""N_PayrollHR_SalaryElementEntry"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
        }

        public async Task DeleteFromSalaryEntry(PayrollRunViewModel viewModel)
        {
            await _queryRepo.ExecuteCommand($@"update cms.""N_PayrollHR_SalaryEntry"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
        }

        public async Task DeleteFromBankLetterDetail(PayrollRunViewModel viewModel)
        {
            await _queryRepo.ExecuteCommand($@"update cms.""N_PayrollHR_BankLetterDetail"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
        }

        public async Task DeleteFromBankLetter(PayrollRunViewModel viewModel)
        {
            await _queryRepo.ExecuteCommand($@"update cms.""N_PayrollHR_BankLetter"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
        }

        public async Task DeleteFromPayrollElementDailyRunResult(PayrollRunViewModel viewModel)
        {
            await _queryRepo.ExecuteCommand($@"update cms.""N_PayrollHR_PayrollElementDailyRunResult"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
        }

        public async Task DeleteFromPayrollElementRunResult(PayrollRunViewModel viewModel)
        {
            await _queryRepo.ExecuteCommand($@"update cms.""N_PayrollHR_PayrollElementRunResult"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
        }

        public async Task DeleteFromPayrollRunResult(PayrollRunViewModel viewModel)
        {
            await _queryRepo.ExecuteCommand($@"update cms.""N_PayrollHR_PayrollRunResult"" set ""IsDeleted""=true where ""PayrollRunId""='{viewModel.Id}'", null);
        }

        // SalaryInfoBusiness Queries
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

            var result = await _queryRepo.ExecuteScalarList<VM>(cypher, null);
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
        
        public async Task<List<SalaryInfoViewModel>> GetUnAssignedSalaryInfoList(string excludePersonId, string query)
        {
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
            var result = await _queryRepo.ExecuteQueryList<SalaryInfoViewModel>(query1, null);
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

            var result = await _queryRepo.ExecuteScalar<string>(query, null);
            return result;
        }

        public async Task<List<PayrollReportViewModel>> GetSalData(PayrollReportViewModel searchModel)
        {
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

            var result1 = await _queryRepo.ExecuteQueryList<PayrollReportViewModel>(query, null);
            return result1;
        }

        public async Task<List<PayrollReportViewModel>> GetPayrollReport(PayrollReportViewModel searchModel)
        {
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

            var result = await _queryRepo.ExecuteQueryList<PayrollReportViewModel>(query, null);
            return result;
        }

        public async Task<List<PayrollReportViewModel>> GetSalDataForDates(PayrollReportViewModel searchModel, PayrollReportViewModel p)
        {
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

            var querydata = await _queryRepo.ExecuteQueryList<PayrollReportViewModel>(query1, null);
            return querydata;
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

            var result = await _queryRepo.ExecuteQueryList<PayrollReportViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<PayrollReportViewModel>(query, null);
            return result;
        }

        public async Task<List<ElementViewModel>> GetDistinctElement(DateTime value)
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

            var querydata = await _queryRepo.ExecuteQueryList<ElementViewModel>(query, null);
            return querydata;
        }

        public async Task<List<ElementViewModel>> GetDistinctElement()
        {
            //var match = string.Concat(@"match (sei:PAY_SalaryElementInfo{IsDeleted:0,IsLatest:true})           
            //match (sei)-[:R_SalaryElementInfo_ElementRoot]->(er:PAY_ElementRoot)<-[:R_ElementRoot]-(e:PAY_Element{ElementCategory:'Standard',IsLatest:true})           
            //return distinct e.Name order by e.Name");

            var query = $@"Select distinct e.""ElementName"" as Name from cms.""N_PayrollHR_PayrollElement"" as e
                            join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""ElementId"" = e.""Id"" and sei.""IsDeleted""=false  and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where e.""ElementCategory"" = 'Standard' and e.""IsDeleted"" = false and e.""CompanyId""='{_repo.UserContext.CompanyId}' order by e.""ElementName"" ";

            var querydata = await _queryRepo.ExecuteQueryList<ElementViewModel>(query, null);
            return querydata;
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
            var result = await _queryRepo.ExecuteQueryList<PayrollReportViewModel>(query, null);
            return result;
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
            var result = await _queryRepo.ExecuteQueryList<PayrollReportViewModel>(query, null);
            return result;
        }



        // SalaryEntryBusiness Queries
        public async Task<List<SalaryElementEntryViewModel>> GetElementsForPayslipPdf(string id)
        {
            var query = $@"Select e.""ElementType"", e.""ElementClassification"", e.""ElementCategory"", se.""Id"", COALESCE(se.""Name"",e.""ElementName"") as Name,
                            se.""Amount"",se.""EarningAmount"",se.""DeductionAmount""
                            From cms.""N_PayrollHR_SalaryEntry"" as ps
                            Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}' and se.""PublishStatus""='{(int)DocumentStatusEnum.Published}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=se.""PayrollElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}' and e.""ElementType""='Cash'
                            where ps.""Id""='{id}' and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}' order by COALESCE(se.""Name"",e.""ElementName"") ";

            var allList = await _queryRepo.ExecuteQueryList<SalaryElementEntryViewModel>(query, null);
            return allList;
        }

        public async Task<List<SalaryElementEntryViewModel>> GetSalDistinctElement(string payrollRunId)
        {
            var query = $@"Select distict e.""ElementName"" From cms.""N_PayrollHR_PayrollRun"" as pay
                            Join cms.""N_PayrollHR_SalaryEntry"" as ps on ps.""PayrollRunId""=pay.""Id"" and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=se.""PayrollElementId"" and e.""EffectiveStartDate""::TIMESTAMP::DATE<='{DateTime.Now}'::TIMESTAMP::DATE
                            and e.""EffectiveEndDate""::TIMESTAMP::DATE>='{DateTime.Now}'::TIMESTAMP::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pay.""Id""='{payrollRunId}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}' order by e.""ElementName"" ";

            var result = await _queryRepo.ExecuteQueryList<SalaryElementEntryViewModel>(query, null);
            return result;
        }

        public async Task<List<SalaryEntryViewModel>> GetSuccessfulSalaryEntryList(PayrollRunViewModel viewModel)
        {
            var query = $@"Select se.*, p.""Id"" as PersonId, p.""UserId"" as UserId
                                From cms.""N_PayrollHR_SalaryEntry"" as se 
                                 Join cms.""N_PayrollHR_PayrollRun"" as prr on prr.""Id""=se.""PayrollRunId"" and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
                                 Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=se.""PersonId"" and p.""IsDeleted""=false   and p.""CompanyId""='{_repo.UserContext.CompanyId}'                            
                                 where se.""PayrollRunId""='{viewModel.Id}'  and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var querydata = await _queryRepo.ExecuteQueryList<SalaryEntryViewModel>(query, null);
            return querydata;

        }

        public async Task<List<SalaryEntryViewModel>> GetSalaryElementDetails(string id)
        {

            var query = string.Concat($@"Select se.""Id"", se.""Name"", se.""Amount"", ", Helper.PersonFullNameWithSponsorshipNo("p", " as PersonName"),
                             $@" From cms.""N_PayrollHR_SalaryEntry"" as ps 
                                 Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}'
                                 Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId""  and p.""IsDeleted""=false   and p.""CompanyId""='{_repo.UserContext.CompanyId}'                              
                                 where ps.""Id""='{id}'  and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'");

            var querydata = await _queryRepo.ExecuteQueryList<SalaryEntryViewModel>(query, null);
            return querydata;
        }

        public async Task<List<SalaryEntryViewModel>> GetSalaryDetails(int publishStatus)
        {
            

            var query = string.Concat($@"Select ps.*, p.""PersonNo"", p.""SponsorshipNo"", ps.""NetAmount"",", Helper.PersonDisplayNameWithSponsorshipNo("p", " as PersonFullName"),
                             $@" From cms.""N_PayrollHR_SalaryEntry"" as ps 
                                 Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                                 Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{_uc.UserId}'  and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                                 where ps.""PublishStatus""={publishStatus}  and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}' order by ps.""PayrollStartDate"" desc ");
            var querydata = await _queryRepo.ExecuteQueryList<SalaryEntryViewModel>(query, null);
            return querydata;
        }

        public async Task<SalaryEntryViewModel> GetPaySlipHeaderDetails(string id)
        {
            //Helper.PersonFullName("pr", " as PersonFullName")
            var query = string.Concat($@"Select ps.*, u.""Id"" as UserId,a.""DateOfJoin"", legal.""CurrencyName"" as CurrencyCode, sp.""SponsorName"" as CompanyNameBasedOnLegalEntity,
                                        pr.""SponsorshipNo"", pr.""PersonNo"",hd.""DepartmentName"" as OrganizationName,hj.""JobTitle"" as JobName, ", $@" CONCAT(pr.""FirstName"",' ',pr.""LastName"") as PersonFullName ",
                                        $@" From cms.""N_PayrollHR_SalaryEntry"" as ps
                                        Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=ps.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=pr.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        join public.""User"" as u on u.""Id""=pr.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=pr.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        left join public.""LegalEntity"" as legal on legal.""Id""=pr.""PersonLegalEntityId""  and legal.""IsDeleted""=false and legal.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=a.""DepartmentId""  and hd.""IsDeleted""=false and hd.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=a.""JobId""  and hj.""IsDeleted""=false and hj.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        where ps.""Id""='{id}'  and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}' and ps.""PublishStatus""='{(int)DocumentStatusEnum.Published}' ");


            var data = await _queryRepo.ExecuteQuerySingle<SalaryEntryViewModel>(query, null);
            
            return data;
        }

        public async Task<List<PayrollSalaryElementViewModel>> GetPaySalarySummaryDetails(int YearMonth)
        {
            //     var match = string.Concat(Helper.OrganizationMapping(UserId, CompanyId, LegalEntityId),
            //         @"match(or: HRS_OrganizationRoot) <-[:R_OrganizationRoot] 
            //         -(o: HRS_Organization{ IsDeleted: 0,CompanyId: { CompanyId} }) where o.EffectiveStartDate <= {ESD} <= o.EffectiveEndDate and or.Id = AllowedOrganizationIds
            //         match(or)<-[:R_Assignment_OrganizationRoot]-(a:HRS_Assignment{IsLatest:true,IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         match(a)-[:R_AssignmentRoot]->(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         match(ar)-[:R_AssignmentRoot_PersonRoot]->(pr:HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //         match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsLatest:true,IsDeleted: 0,CompanyId: { CompanyId} })	
            //         match(pr)<-[:R_SalaryEntry_PersonRoot]-(ps:PAY_SalaryEntry{YearMonth:{YearMonth},ExecutionStatus:'Success',IsDeleted: 0,CompanyId: {CompanyId}})
            //         optional match(a)-[:R_Assignment_PositionRoot]->(por:HRS_PositionRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //optional match(por)<-[:R_PositionRoot]-(po:HRS_Position{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //         where po.EffectiveStartDate <= {ESD} <= po.EffectiveEndDate 
            //         optional match(o)-[:R_Organization_ResponsibilityCenter] -> (rc: HRS_ResponsibilityCenter{ IsDeleted: 0,CompanyId: { CompanyId} })
            //         optional match(o)-[:R_Organization_CostCenter] -> (cc: HRS_CostCenter{ IsDeleted: 0,CompanyId: { CompanyId} })
            //         optional match(po)-[:R_Position_NAVSection]->(ns:HRS_NAVSection{ IsDeleted: 0,CompanyId: { CompanyId} })
            //         with ps,p,pr,o,rc,ns,cc 
            //         return distinct rc.Code as ResponsibilityCenter,cc.Code as CostCenter,
            //         ns.Name as NavSection,ns.Code as NavSectionCode,sum(ps.NetAmount) as NetAmount order by ResponsibilityCenter");

            var query = $@"Select distinct rc.""ResponsibilityCenterCode"" as ResponsibilityCenter, cc.""CostCenterCode"" as CostCenter, ns.""Name"" as NavSection, ns.""Code"" as NavSectionCode, SUM(ps.""NetAmount"") as NetAmount
                            From cms.""N_CoreHR_HRDepartment"" as o
                            Join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=o.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=a.""EmployeeId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryEntry"" as ps on ps.""PersonId""=p.""Id"" and ps.""YearMonth""='{YearMonth}' and ps.""ExecutionStatus""=0 and ps.""IsDeleted""=false
                            Left Join cms.""N_CoreHR_HRPosition"" as po on po.""Id""=a.""PositionId"" and po.""IsDeleted""=false and po.""EffectiveStartDate""::TIMESTAMP::DATE<='{DateTime.Now}'::TIMESTAMP::DATE and po.""EffectiveEndDate""::TIMESTAMP::DATE>='{DateTime.Now}'::TIMESTAMP::DATE
                            Left Join cms.""N_CoreHR_HRResponsibilityCenter"" as rc on rc.""Id""=o.""ResponsibilityCenterId"" and rc.""IsDeleted""=false and rc.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRCostCenter"" as cc on cc.""Id""=o.""CostCenterId"" and cc.""IsDeleted""=false and cc.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRNavSection"" as ns on ns.""Id""=po.""NavSectionId""  and ns.""IsDeleted""=false and ns.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where  o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}' order by rc.""ResponsibilityCenterCode"" ";

            var result = await _queryRepo.ExecuteQueryList<PayrollSalaryElementViewModel>(query, null);

            return result;
        }

        public async Task<List<SalaryEntryViewModel>> GetPaySalaryDetails(SalaryEntryViewModel search, string legalid)
        {
            
            var query = string.Concat($@"Select ps.*, p.""PersonNo"", p.""SponsorshipNo"", ps.""NetAmount"", concat(p.""FirstName"", coalesce('' , p.""LastName"",''), coalesce('-' , p.""PersonNo"",'')) as PersonFullName
                                        From cms.""N_PayrollHR_SalaryEntry"" as ps
                                        Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        --and p.""PersonLegalEntityId""='{legalid}'
                                        join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        where ps.""YearMonth""='{search.YearMonth}' and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}' #PersonWhere# order by ps.""PayrollStartDate"" desc ");
            var personwhere = "";
            if (search.PersonId.IsNotNullAndNotEmpty())
            {
                personwhere = $@" and ps.""PersonId""='{search.PersonId}' ";
            }
            query = query.Replace("#PersonWhere#", personwhere);
            var result = await _queryRepo.ExecuteQueryList<SalaryEntryViewModel>(query, null);
            return result;
        }

        public async Task<List<PayrollSalaryElementViewModel>> GetPaySalaryElementDetailsQ1(string payrollRunId)
        {

            var query = string.Concat($@"Select ps.""Id"", ps.""NetAmount"", pr.""Id"" as PersonId, ps.""PayrollStartDate"", ps.""PayrollEndDate"",
                                        o.""Name"" as OrganizationName, cc.""Code"" as CostCenter, rc.""ResponsibilityCenterCode"" as ResponsibilityCenter,
                                        ns.""Name"" as NavSection, nc.""Code"" as NavSectionCode, ", Helper.PersonFullNameWithSponsorshipNo("p", " as PersonName"),
                                        $@" From cms.""N_PayrollHR_PayrollRun"" as pay
                                            Join cms.""N_PayrollHR_SalaryEntry"" as ps on ps.""PayrollRunId""=pay.""Id"" and ps.""ExecutionStatus""=0 and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
                                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                                            Left Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}' and a.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= a.""EffectiveEndDate""::Date 
                                            Left Join cms.""N_CoreHR_HRPosition"" as po on po.""Id""=a.""PositionId"" and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}' and po.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= po.""EffectiveEndDate""::Date
                                            Left Join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}' and o.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= o.""EffectiveEndDate""::Date
                                            Left Join cms.""N_CoreHR_HRResponsibilityCenter"" as rc on rc.""Id""=o.""ResponsibilityCenter"" and rc.""IsDeleted""=false and rc.""CompanyId""='{_repo.UserContext.CompanyId}'
                                            Left Join cms.""N_CoreHR_HRCostCenter"" as cc on cc.""Id""=o.""CostCenterId"" and cc.""IsDeleted""=false and cc.""CompanyId""='{_repo.UserContext.CompanyId}'
                                            Left Join cms.""N_CoreHR_HRNavSection"" as ns on ns.""Id""=po.""NavSectionId"" and ns.""IsDeleted""=false and ns.""CompanyId""='{_repo.UserContext.CompanyId}'
                                            where pay.""Id""='{payrollRunId}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}' order by ", Helper.PersonFullNameWithSponsorshipNo("p", ""));

            var result = await _queryRepo.ExecuteQueryList<PayrollSalaryElementViewModel>(query, null);
            return result;
        }

        public async Task<List<SalaryElementEntryViewModel>> GetPaySalaryElementDetailsQ2(string payrollRunId, ElementCategoryEnum? elementCategory)
        {

            var query1 = string.Concat($@"Select se.*, pr.""Id"" as PersonId, e.""ElementName"", er.""Id"" as ElementId, ps.""Id"" as SalaryEntryId
                            From cms.""N_PayrollHR_PayrollRun"" as pay
                            Join cms.""N_PayrollHR_SalaryEntry"" as ps on ps.""PayrollRunId""=pay.""Id"" and ps.""ExecutionStatus""=0 and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""ExecutionStatus""=0 and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=se.""PayrollElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}' and e.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= e.""EffectiveEndDate""::Date",
                             elementCategory == null ? "" : $@" and e.""ElementCategory""='{elementCategory}' ",
                             $@" where pay.""Id""='{payrollRunId}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}' order by e.""ElementName"" ");

            var result = await _queryRepo.ExecuteQueryList<SalaryElementEntryViewModel>(query1, null);
            return result;
        }


        // payrollTransactionBusiness Queries

        public async Task DeletePayrollTransaction(string NoteId)
        {
            var query = $@"update  cms.""N_PayrollHR_PayrollTransaction"" set ""IsDeleted""=true where ""Id""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);
                
        }

        public async Task<PayrollTransactionViewModel> GetPayrollTransactionDetails(string transactionId)
        {
            var query = "";
            if (transactionId.IsNotNullAndNotEmpty())
            {
                query = $@"select * from cms.""N_PayrollHR_PayrollTransaction""  as pt where pt.""Id"" = '{transactionId}' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            }
            var queryData = await _queryRepo.ExecuteQuerySingle<PayrollTransactionViewModel>(query, null);
            return queryData;

        }
        public async Task<IdNameViewModel> GetPayrollElementByCode(string code)
        {
            

            var query = $@"select ""ElementName"" as Name,""Id"" as Id from cms.""N_PayrollHR_PayrollElement""  as pt where pt.""ElementCode"" = '{code}' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
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
            var list = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
            return list;
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


            var result1 = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
            return result1;
        }

        public async Task UpdatePayrollTxn(PayrollRunViewModel viewModel, string payTransIds, DateTime lastUpdateDate)
        {
            var query1 = $@"Update cms.""N_PayrollHR_PayrollTransaction""
                            set ""PayrollBatchId""='{viewModel.PayrollBatchId}',""PayrollRunId""='{viewModel.Id}', ""LastUpdatedDate""='{lastUpdateDate}'
                        where ""Id"" IN ({payTransIds}) and ""IsDeleted""=false";
            await _queryRepo.ExecuteCommand(query1, null);
        }

        public async Task UpdatePayrollTxnforIds(DateTime lastUpdateDate, string notProcessedIds)
        {
            var query = $@"Update cms.""N_PayrollHR_PayrollTransaction""
                            set ""PayrollBatchId""=null,""PayrollRunId""=null, ""LastUpdatedDate""='{lastUpdateDate}'
                        where ""Id"" IN ({notProcessedIds}) ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task UpdatePayrollTxnDetailsforIds(string processedIds, string payrollId, string payrollRunId, LOVViewModel processStatus, DateTime lastUpdateDate)
        {
            var query1 = $@"Update cms.""N_PayrollHR_PayrollTransaction""
                            set ""PayrollBatchId""='{payrollId}',""PayrollRunId""='{payrollRunId}',""ProcessStatusId""='{processStatus.Id}', ""LastUpdatedDate""='{lastUpdateDate}'
                        where ""Id"" IN ({processedIds}) ";
            await _queryRepo.ExecuteCommand(query1, null);
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

        public async Task<List<PayrollTransactionViewModel>> IsTransactionExists(string personId, string elementCode, DateTime date, double amount)
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

            var list = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
            return list;
        }

        public async Task<MandatoryDeductionViewModel> GetExemption(MandatoryDeductionViewModel model, string financialYearId, string personId, DateTime asofDate)
        {
            var query = $@"select Sum(id.""TotalAmount""::DECIMAL) as TotalAmount,de.""SeniorCitizenAmount"" as SeniorCitizenAmount,de.""WomanAmount"" as WomanAmount,de.""DefaultAmount"" as EmployeeAmount from cms.""N_PayrollHR_MandatoryDeduction"" as md 
join cms.""N_PayrollHR_DEDUCTION_EXEMPTION"" as de on de.""MandatoryDeductionId""=md.""Id"" and de.""IsDeleted""=false
join cms.""F_PAY_HR_InvestmentType"" as it on it.""DeductionExemptionId""=de.""Id"" and it.""IsDeleted""=false
join cms.""N_SNC_CHR_InvestmentDeclaration"" as id on id.""InvestmentTypeId""=it.""Id"" and id.""FinancialYearId""='{financialYearId}' and id.""IsDeleted""=false 
join public.""NtsService"" as s on s.""UdfNoteTableId""=id.""Id"" and s.""IsDeleted""=false
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false and lv.""Code""='SERVICE_STATUS_COMPLETE'
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=s.""OwnerUserId"" and p.""IsDeleted""=false and p.""Id""='{personId}'
and md.""IsDeleted""=false where de.""ESDDateTime""::DATE<='{asofDate}'::DATE and de.""EEDDateTime""::DATE>='{asofDate}'::DATE group by id.""Id"",id.""TotalAmount"",de.""SeniorCitizenAmount"",de.""WomanAmount"",de.""DefaultAmount""";
            var data = await _queryRepo.ExecuteQuerySingle<MandatoryDeductionViewModel>(query, null);
            return data;
        }
        public async Task<List<MandatoryDeductionViewModel>> GetFinancialYear(DateTime asOfDate)
        {
            var query = $@"Select *,""FinancialYearName"" as DeductionName,""StartDate"" as EffectiveStartDate,""EndDate"" as EffectiveEndDate from cms.""F_PAY_HR_FinancialYearName"" where ""StartDate""::DATE<='{asOfDate}'::DATE and ""EndDate""::DATE>='{asOfDate}'::DATE  and ""IsDeleted"" = false";
            var result = await _queryRepo.ExecuteQueryList<MandatoryDeductionViewModel>(query, null);
            return result;
        }
        public async Task<double> GetNoOfMonthsForEmploeePayroll(DateTime? dateOfJoin,DateTime? lastWorkingDate,string financialYearId)
        {
            var date1 =DateTime.Now;
            var date2 = DateTime.Now;
            var query = $@"Select *,""FinancialYearName"" as DeductionName,""StartDate"" as EffectiveStartDate,""EndDate"" as EffectiveEndDate from cms.""F_PAY_HR_FinancialYearName"" where ""Id"" ='{financialYearId}' and ""IsDeleted"" = false";
            var result = await _queryRepo.ExecuteQuerySingle<MandatoryDeductionViewModel>(query, null);

            if (dateOfJoin < result.EffectiveStartDate)
            {
                date1 = result.EffectiveStartDate.Value.Date;
            }
            else
            {
                date1 = dateOfJoin.Value.Date;
            }

            if (lastWorkingDate < result.EffectiveEndDate)
            {
                date2 = lastWorkingDate.Value.Date;
            }
            else 
            {
                date2 = result.EffectiveEndDate.Value.Date;
            }
            var TotalMonthBetweenDays = ((date2.Year - date1.Year) * 12) + date2.Month - date1.Month;
            return 0.0;
        }
        public async Task<List<MandatoryDeductionViewModel>> GetMandatoryDeductionOfFinancialYear(DateTime asOfDate)
        {
            var query = $@"Select *,""NtsNoteId"" as NoteId from cms.""N_PayrollHR_MandatoryDeduction"" where ""EffectiveStartDate""::DATE<='{asOfDate}'::DATE and ""EffectiveEndDate""::DATE>='{asOfDate}'::DATE and ""IsDeleted""=false";
            var result = await _queryRepo.ExecuteQueryList<MandatoryDeductionViewModel>(query, null);
            return result;
        }
        public async Task<List<PayrollTransactionViewModel>> GetSalaryTransactionList(DateTime startDate, DateTime endDate, string payrollRunId = null)
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
            var list = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
            return list;
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

            var list = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
            return list;
        }
        public async Task<List<ElementViewModel>> GetPayrollElement(string elementId)
        {
            var query = $@"Select e.""Id"" as ""ElementId""
                            from cms.""N_PayrollHR_PayrollElement"" as e
                            where e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}' and e.""Id""='{elementId}'";
            var percentageElementRootIds = await _queryRepo.ExecuteQueryList<ElementViewModel>(query, null);
            return percentageElementRootIds;
        }

        public async Task<bool?> UpdatePayrollTxnByNoteId(PayrollTransactionViewModel trans)
        {
            var result1 = await _queryRepo.ExecuteScalar<bool?>($@" update cms.""N_PayrollHR_PayrollTransaction"" set ""IsDeleted""=false where ""NtsNoteId""='{trans.NtsNoteId}'", null);
            return result1;
        }

        public async Task<List<PayrollTransactionViewModel>> GetPayrollTxnDatabyDate(ServiceTemplateViewModel viewModel, DateTime? anniversaryStartDate)
        {
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
            return result;
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQuerySingle<PayrollTransactionViewModel>(query, null);
            return result;
        }
        public async Task<PayrollTransactionViewModel> GetPayrollTransationDataByReferenceId(string referenceId)
        {
            var query = "";
            if (referenceId.IsNotNullAndNotEmpty())
            {
                query = $@"select * from cms.""N_PayrollHR_PayrollTransaction""  as pt where pt.""ReferenceId"" = '{referenceId}' and pt.""IsDeleted""=false and pt.""CompanyId""='{_repo.UserContext.CompanyId}'";
            }
            var queryData = await _queryRepo.ExecuteQuerySingle<PayrollTransactionViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
            return result;
        }

        public async Task<IList<PayrollTransactionViewModel>> GetPayrollTransactionListByDates(PayrollTransactionViewModel search, string personId)
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
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

            var list = await _queryRepo.ExecuteQuerySingle<PayrollTransactionViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<PayrollTransactionViewModel>(query, null);
            return result;
        }

        public async Task<bool> CloseTransaction(string Id, bool isClosed)
        {
            //var cypher = @"match (pt:PAY_PayrollTransaction{IsDeleted:0,Id:{Id}}) 
            //             set pt.IsTransactionClosed={isClosed} return 1";

            var query = $@"Update cms.""N_PayrollHR_PayrollTransaction"" set ""IsTransactionClosed""={isClosed} where ""Id""='{Id}' ";

            await _queryRepo.ExecuteCommand(query, null);
            return true;

        }


        // PayrollRunResultBusiness Queries
        public async Task<List<PayrollElementRunResultViewModel>> GetSuccessfulElementRunResult(PayrollRunViewModel viewModel, int executionstatus)
        {
            
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
            var result = await _queryRepo.ExecuteQueryList<PayrollElementRunResultViewModel>(cypher, null);
            return result;
        }

        public async Task<List<ElementViewModel>> GetDistinctElementDeduction(string payrollRunId, ElementCategoryEnum? elementCategory)
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
            var result = await _queryRepo.ExecuteQueryList<ElementViewModel>(query, null);
            return result;
        }

        public async Task<List<ElementViewModel>> GetDistinctElementEarning(string payrollRunId, ElementCategoryEnum? elementCategory)
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
            var result = await _queryRepo.ExecuteQueryList<ElementViewModel>(query, null);
            return result;
        }

        public async Task<List<string>> GetDistinctElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory, ElementClassificationEnum? elementType)
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

            var result = await _queryRepo.ExecuteScalarList<string>(query, null);
            return result;
        }

        public async Task<List<PayrollRunResultViewModel>> GetSuccessfulPayrollRunResult(PayrollRunViewModel viewModel, int executionStatus)
        {

            //var prms = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active },
            //    { "CompanyId", CompanyId },
            //    { "PayrollRunId", viewModel.Id },
            //    { "ESD", DateTime.Today},
            //    { "LegalEntityId",LegalEntityId },
            //};
            
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
            var result = await _queryRepo.ExecuteQueryList<PayrollRunResultViewModel>(cypher, null);
            return result;
        }

        public async Task<List<ElementViewModel>> GetDistinctElement(string payrollRunId, ElementCategoryEnum? elementCategory)
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

            var result = await _queryRepo.ExecuteQueryList<ElementViewModel>(query, null);
            return result;
        }

        public async Task<List<PayrollSalaryElementViewModel>> GetPayrollSalaryElements(string payrollRunId)
        {
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

            var employees = await _queryRepo.ExecuteQueryList<PayrollSalaryElementViewModel>(match, null);
            return employees;
        }

        public async Task<List<SalaryElementEntryViewModel>> GetPersonSalEntryDetails(string pers, PayrollBatchViewModel payrollBatch)
        {
            var match1 = $@"Select pr.""Id"" as PersonId, e.""ElementName"", e.""ElementDisplayName"", sei.""Amount""
                           From cms.""N_CoreHR_HRPerson"" as pr
                           join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=pr.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
                           join public.""NtsNote"" as nts on nts.""ParentNoteId""=si.""NtsNoteId""  and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
                           join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=nts.""Id"" and sei.""IsDeleted""=false and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                           join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                           --and e.""EffectiveStartDate""::TIMESTAMP::DATE<='{payrollBatch.PayrollEndDate}'::TIMESTAMP::DATE and e.""EffectiveEndDate""::TIMESTAMP::DATE >='{payrollBatch.PayrollEndDate}'::TIMESTAMP::DATE
                           where pr.""Id"" in ('{pers}') and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}' order by pr.""Id"", e.""ElementName"" ";

            var result1 = await _queryRepo.ExecuteQueryList<SalaryElementEntryViewModel>(match1, null);
            return result1;
        }

        public async Task<List<PayrollElementRunResultViewModel>> GetPayrollRunData(string payrollRunId, ElementCategoryEnum? elementCategory)
        {
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

            var salaryElementList = await _queryRepo.ExecuteQueryList<PayrollElementRunResultViewModel>(match2, null);
            return salaryElementList;
        }

        public async Task<List<PayrollSalaryElementViewModel>> GetDistinctElementDisplayName(string payrollRunId, ElementCategoryEnum? elementCategory)
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

            var queryresult = await _queryRepo.ExecuteQueryList<PayrollSalaryElementViewModel>(query, null);
            return queryresult;
        }

        public async Task<List<PayrollDetailViewModel>> GetpayRollDetails(string payrollRunId, int? yearMonth, ElementCategoryEnum? elementCategory)
        {
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
join cms.""N_PayrollHR_PayrollRunResult"" as par on par.""PayrollRunId"" = pay.""Id"" and pay.""Id""='{payrollRunId}' and par.""IsDeleted""=false and par.""CompanyId""='{_repo.UserContext.CompanyId}'";
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
            var result = await _queryRepo.ExecuteQueryList<PayrollDetailViewModel>(cypher, null);
            return result;
        }

        public async Task<List<PayrollElementDailyRunResultViewModel>> GetPayrollDailyResult(string payrollRunId, int? yearMonth)
        {
            var search = "";
            if (payrollRunId.IsNullOrEmptyOrWhiteSpace())
            {
                search = string.Concat(
               $@"from cms.""N_PayrollHR_PayrollBatch"" as pr
join cms.""N_PayrollHR_PayrollRun"" as pay on pay.""PayrollBatchId""=pr.""Id"" and pay.""YearMonth""='{yearMonth}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_PayrollRunResult"" as par on par.""PayrollRunId""=pay.""Id"" and par.""IsDeleted""=false and par.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LegalEntity"" as or on or.""Id""=pr.""LegalEntityId"" and or.""IsDeleted""=false and or.""CompanyId""='{_repo.UserContext.CompanyId}' and or.""Id""='{_repo.UserContext.LegalEntityId}'");
                
                
            }
            else
            {
                search = $@"from cms.""N_PayrollHR_PayrollRun"" as pay
join cms.""N_PayrollHR_PayrollRunResult"" as par on par.""PayrollRunId"" = pay.""Id"" and pay.""Id""='{payrollRunId}' and par.""IsDeleted""=false and par.""CompanyId""='{_repo.UserContext.CompanyId}'";
               
            }


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
            var dailyResult = await _queryRepo.ExecuteQueryList<PayrollElementDailyRunResultViewModel>(cypher1, null);
            return dailyResult;
        }



    }
}
