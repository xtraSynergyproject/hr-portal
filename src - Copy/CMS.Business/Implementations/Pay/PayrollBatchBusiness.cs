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
    public class PayrollBatchBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollBatchBusiness
    {
        private readonly IRepositoryQueryBase<SalaryInfoViewModel> _salaryInfo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _queryPayBatch;

        public PayrollBatchBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch) : base(repo, autoMapper)
        {
            _salaryInfo = salaryInfo;
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            _queryPayBatch = queryPayBatch;
        }

        public async override Task<CommandResult<NoteViewModel>> Create(NoteViewModel model)
        {

            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<NoteViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteViewModel>> Edit(NoteViewModel model)
        {

            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<NoteViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<List<IdNameViewModel>> GetPayGroupList()
        {
            string query = $@"select p.""Id"" as Id ,p.""Name"" as Name
                            from cms.""N_PayrollHR_PayrollGroup"" as p  
                           where p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'" ;

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<IdNameViewModel>> GetPayCalenderList()
        {
            string query = $@"select p.""Id"" as Id ,p.""Name"" as Name
                            from cms.""N_PayrollHR_PayrollCalendar"" as p                        
                             where p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<IdNameViewModel>> GetPayBankBranchList()
        {
            string query = $@"select p.""Id"" as Id ,p.""BranchName"" as Name
                            from cms.""N_PayrollHR_BankBranch"" as p                        
                            where p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<IdNameViewModel>> GetSalaryElementIdName()
        {
            var query = $@"SELECT B.""ElementName"" as Name, B.""Id"" as Id
    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_PayrollElement"" B on  N.""Id"" =B.""NtsNoteId"" and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
             where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'" ;

            var queryData = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
            public async Task<List<SalaryInfoViewModel>> GetSalaryInfoDetails(string salaryInfoId)
        {
            var query = "";
            if (salaryInfoId.IsNotNullAndNotEmpty())
            {
                query = $@"SELECT B.*, LOV.""Name"" as StatusName,p.""Id"" as PersonId ,CONCAT( p.""FirstName"",' ',p.""LastName"") as PersonName,
p.""PersonNo"" as PersonNo,p.""IqamahNoNationalId"" as SponsorshipNo,pay.""Name"" as PayGroupName,N.""Id"" as NoteId,pay.""Id"" as PayGroupId

    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_SalaryInfo"" B on  N.""Id"" =B.""NtsNoteId""  and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_PayrollHR_PayrollGroup"" as pay on pay.""Id""=B.""PayGroupId"" and  pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=B.""PersonId"" and  p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
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
            
            var queryData = await _salaryInfo.ExecuteQueryList<SalaryInfoViewModel>(query, null);

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
        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoDetails(string elementId,string salaryInfoId)
        {
            var query = "";
            if (elementId.IsNotNullAndNotEmpty())
            {
                query = $@"SELECT B.*,N.""Id"" as NoteId

    FROM  public.""NtsNote"" N
    inner join cms.""N_PayrollHR_SalaryElementInfo"" B on  N.""Id"" =B.""NtsNoteId"" and B.""IsDeleted""=false and B.""CompanyId""='{_repo.UserContext.CompanyId}'
 where B.""NtsNoteId""='{elementId}' and N.""ParentNoteId""='{salaryInfoId}' and N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}'"; 
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

            var queryData = await _salaryInfo.ExecuteQueryList<SalaryElementInfoViewModel>(query, null);

           
            return queryData;


        }
        public async Task<bool> DeleteSalaryElement(string NoteId)
        {
            var note = await _repo.GetSingleById(NoteId);
            if (note!=null)
            {
                var query = $@"update  cms.""N_PayrollHR_SalaryElementInfo"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);
                
                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<List<IdNameViewModel>> GetPayrollGroupList()
        {
            var query = $@"select ""Id"", ""Name"" from cms.""N_PayrollHR_PayrollGroup"" where ""IsDeleted""=false ";

            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<PayrollGroupViewModel> GetPayrollGroupById(string payGroupId)
        {
            var query = $@"select * from cms.""N_PayrollHR_PayrollGroup"" where ""Id""='{payGroupId}'  and ""IsDeleted""=false  and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryRepo1.ExecuteQuerySingle<PayrollGroupViewModel>(query, null);
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

            var result = await _queryPayBatch.ExecuteQueryList(query, null);

            return result;
        }
        public async Task<PayrollBatchViewModel> GetSingleById(string payrollBatchId)
        {
            var query = @$"Select * from cms.""N_PayrollHR_PayrollBatch"" where ""Id""='{payrollBatchId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryPayBatch.ExecuteQuerySingle<PayrollBatchViewModel>(query, null);
            return result;
        }
        public async Task<PayrollBatchViewModel> IsPayrollExist(string payGroupId, string yearmonth)
        {
            var query = @$"Select * from cms.""N_PayrollHR_PayrollBatch"" where ""PayrollGroupId""='{payGroupId}' and ""YearMonth""='{yearmonth}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var result = await _queryPayBatch.ExecuteQuerySingle<PayrollBatchViewModel>(query, null);
            return result;
        }


    }
}
