using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
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
    public class SalaryEntryBusiness : BusinessBase<NoteViewModel, NtsNote>, ISalaryEntryBusiness
    {
        private readonly IRepositoryQueryBase<SalaryInfoViewModel> _salaryInfo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _queryPayBatch;
        private readonly IRepositoryQueryBase<SalaryEntryViewModel> _salEntry;
        private readonly IRepositoryQueryBase<SalaryElementEntryViewModel> _saleleEntry;
        private readonly IRepositoryQueryBase<PayrollSalaryElementViewModel> _paysalele;
        private readonly ILeaveBalanceSheetBusiness _lbs;
        private readonly IPayrollElementBusiness _payelebus;
        IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;

        public SalaryEntryBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo, IRepositoryQueryBase<PayrollSalaryElementViewModel> paysalele
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IPayrollElementBusiness payelebus, IRepositoryQueryBase<SalaryElementEntryViewModel> saleleEntry,
            IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch,
            IUserContext userContext, IRepositoryQueryBase<SalaryEntryViewModel> salEntry, ILeaveBalanceSheetBusiness lbs
            , INoteBusiness noteBusiness) : base(repo, autoMapper)
        {
            _salaryInfo = salaryInfo;
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            _queryPayBatch = queryPayBatch;
            _userContext = userContext;
            _salEntry = salEntry;
            _lbs = lbs;
            _payelebus = payelebus;
            _saleleEntry = saleleEntry;
            _paysalele = paysalele;
            _noteBusiness = noteBusiness;
        }

        public async Task<List<SalaryEntryViewModel>> BulkInsertForPayroll(List<SalaryEntryViewModel> viewModelList, bool idGenerated = true, bool doCommit = true)
        {
            //throw new NotImplementedException();
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
                noteTempModel.TemplateCode = "SalaryEntry";
                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

                noteModel.Json = JsonConvert.SerializeObject(item);
                noteModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                var result = await _noteBusiness.ManageNote(noteModel);
            }
            return viewModelList;
        }

        public async Task<List<SalaryElementEntryViewModel>> BulkInsertIntoSalaryElementEntry(List<SalaryElementEntryViewModel> viewModelList, bool idGenerated = true, bool doCommit = true)
        {
            //throw new NotImplementedException();
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
                noteTempModel.TemplateCode = "SALARY_ELEMENT_ENTRY";
                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

                noteModel.Json = JsonConvert.SerializeObject(item);
                noteModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                var result = await _noteBusiness.ManageNote(noteModel);
            }
            return viewModelList;

        }

        public async Task<List<SalaryElementEntryViewModel>> GetDeductionElementForPayslipPdf(string id)
        {
            var query = $@"Select e.""ElementType"", e.""ElementClassification"", e.""ElementCategory"", se.""Id"", COALESCE(se.""Name"",e.""ElementName"") as Name,
                            se.""Amount"",se.""EarningAmount"",se.""DeductionAmount""
                            From cms.""N_PayrollHR_SalaryEntry"" as ps
                            Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}' and se.""PublishStatus""='{(int)DocumentStatusEnum.Published}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=se.""PayrollElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}' and e.""ElementType""='Cash'
                            where ps.""Id""='{id}' and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}' order by COALESCE(se.""Name"",e.""ElementName"") ";

            var allList = await _saleleEntry.ExecuteQueryList(query, null);

            //Test Dummy data
            if (allList.Count()==0)
            {
                allList.Add(
                    new SalaryElementEntryViewModel
                    {
                        ElementType = ElementTypeEnum.Cash,
                        ElementClassification = ElementClassificationEnum.Earning,
                        ElementCategory = ElementCategoryEnum.Standard,
                        Id = "072198e6-59c6-464d-a518-509cab789cfdev01",
                        Name = "Basic Salary",
                        Amount = 4000,
                        EarningAmount = 4000,
                        DeductionAmount = 0
                    });

                allList.Add(
                    new SalaryElementEntryViewModel
                    {
                        ElementType = ElementTypeEnum.Cash,
                        ElementClassification = ElementClassificationEnum.Earning,
                        ElementCategory = ElementCategoryEnum.Standard,
                        Id = "072198e6-59c6-464d-a518-509cab789cfdev02",
                        Name = "HRA",
                        Amount = 1000,
                        EarningAmount = 1000,
                        DeductionAmount = 0
                    });

                allList.Add(
                    new SalaryElementEntryViewModel
                    {
                        ElementType = ElementTypeEnum.Cash,
                        ElementClassification = ElementClassificationEnum.Deduction,
                        ElementCategory = ElementCategoryEnum.Standard,
                        Id = "072198e6-59c6-464d-a518-509cab789cfdev03",
                        Name = "Other Allowance Deduction",
                        Amount = 550,
                        EarningAmount = 0,
                        DeductionAmount = 550
                    });
            }
            //Test Dummy data Closed

            var earningList = allList.Where(x => x.ElementClassification == ElementClassificationEnum.Earning && x.EarningAmount != null && x.EarningAmount != 0).ToList();
            var deductionList = allList.Where(x => x.ElementClassification == ElementClassificationEnum.Deduction && x.DeductionAmount != null && x.DeductionAmount != 0).ToList();
            var diff = earningList.Count() - deductionList.Count();
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    deductionList.Add(new SalaryElementEntryViewModel());
                }
            }
            return deductionList;
        }

        public async Task<string[]> GetDistinctElement(string payrollRunId)
        {

            var query = $@"Select distict e.""ElementName"" From cms.""N_PayrollHR_PayrollRun"" as pay
                            Join cms.""N_PayrollHR_SalaryEntry"" as ps on ps.""PayrollRunId""=pay.""Id"" and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=se.""PayrollElementId"" and e.""EffectiveStartDate""::TIMESTAMP::DATE<='{DateTime.Now}'::TIMESTAMP::DATE
                            and e.""EffectiveEndDate""::TIMESTAMP::DATE>='{DateTime.Now}'::TIMESTAMP::DATE and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where pay.""Id""='{payrollRunId}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}' order by e.""ElementName"" ";

            var result = await _saleleEntry.ExecuteQueryList(query, null);
            string[] data = result.Select(x => x.ElementName).ToArray();
            return data;
        }

        public async Task<List<SalaryElementEntryViewModel>> GetEarningElementForPayslipPdf(string id)
        {

            var query = $@"Select e.""ElementType"", e.""ElementClassification"", e.""ElementCategory"", se.""Id"", COALESCE(se.""Name"",e.""ElementName"") as Name,
                            se.""Amount"",se.""EarningAmount"",se.""DeductionAmount""
                            From cms.""N_PayrollHR_SalaryEntry"" as ps
                            Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}' and se.""PublishStatus""='{(int)DocumentStatusEnum.Published}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=se.""PayrollElementId"" and e.""ElementType""='Cash' and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where ps.""Id""='{id}' and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}' order by COALESCE(se.""Name"",e.""ElementName"") ";

            var allList = await _saleleEntry.ExecuteQueryList(query, null);
            
            //Test Dummy data
            if (allList.Count()==0)
            {
                allList.Add(
                    new SalaryElementEntryViewModel
                    {
                        ElementType = ElementTypeEnum.Cash,
                        ElementClassification = ElementClassificationEnum.Earning,
                        ElementCategory = ElementCategoryEnum.Standard,
                        Id = "072198e6-59c6-464d-a518-509cab789cfdev01",
                        Name = "Basic Salary",
                        Amount = 4000,
                        EarningAmount = 4000,
                        DeductionAmount = 0
                    });

                allList.Add(
                    new SalaryElementEntryViewModel
                    {
                    ElementType = ElementTypeEnum.Cash,
                    ElementClassification = ElementClassificationEnum.Earning,
                    ElementCategory = ElementCategoryEnum.Standard,
                    Id = "072198e6-59c6-464d-a518-509cab789cfdev02",
                    Name = "HRA",
                    Amount = 1000,
                    EarningAmount = 1000,
                    DeductionAmount = 0
                    });

                allList.Add(
                    new SalaryElementEntryViewModel
                    {
                    ElementType = ElementTypeEnum.Cash,
                    ElementClassification = ElementClassificationEnum.Deduction,
                    ElementCategory = ElementCategoryEnum.Standard,
                    Id = "072198e6-59c6-464d-a518-509cab789cfdev03",
                    Name = "Other Allowance Deduction",
                    Amount = 550,
                    EarningAmount = 0,
                    DeductionAmount = 550
                    });
            }
            //Test Dummy data Closed

            var earningList = allList.Where(x => x.ElementClassification == ElementClassificationEnum.Earning && x.EarningAmount != null && x.EarningAmount != 0).ToList();
            var deductionList = allList.Where(x => x.ElementClassification == ElementClassificationEnum.Deduction && x.DeductionAmount != null && x.DeductionAmount != 0).ToList();
            var diff = deductionList.Count() - earningList.Count();
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    earningList.Add(new SalaryElementEntryViewModel());
                }
            }
            return earningList;
        }

        public async Task<List<SalaryEntryViewModel>> GetPaySalaryDetails(SalaryEntryViewModel search)
        {
            var legalid = _repo.UserContext.LegalEntityId;
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
            var result = await _salEntry.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<PayrollSalaryElementViewModel>> GetPaySalaryElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory)
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

            var result = await _paysalele.ExecuteQueryList(query, null);


            var query1 = string.Concat($@"Select se.*, pr.""Id"" as PersonId, e.""ElementName"", er.""Id"" as ElementId, ps.""Id"" as SalaryEntryId
                            From cms.""N_PayrollHR_PayrollRun"" as pay
                            Join cms.""N_PayrollHR_SalaryEntry"" as ps on ps.""PayrollRunId""=pay.""Id"" and ps.""ExecutionStatus""=0 and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""ExecutionStatus""=0 and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=se.""PayrollElementId"" and e.""IsDeleted""=false and e.""CompanyId""='{_repo.UserContext.CompanyId}' and e.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date <= e.""EffectiveEndDate""::Date",
                            elementCategory == null ? "" : $@" and e.""ElementCategory""='{elementCategory}' ",
                            $@" where pay.""Id""='{payrollRunId}' and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}' order by e.""ElementName"" ");

            var result1 = await _saleleEntry.ExecuteQueryList(query1, null);

            var distinctElement = await GetDistinctElement(payrollRunId);//result1.DistinctBy(x=>x.ElementId);
            foreach (var item in result)
            {
                var elementresult = result1.Where(x => x.SalaryEntryId == item.Id && x.PersonId == item.PersonId);

                int i = 1;
                //double netAmount = 0;
                foreach (var item3 in distinctElement)
                {
                    var Col = string.Concat("Element", i);
                    foreach (var item1 in elementresult)
                    {
                        if (item3 == item1.ElementName)
                            ApplicationExtension.SetPropertyValue(item, Col, item1.Amount);
                        //netAmount += item1.Amount;
                    }
                    i++;
                }
                // item.NetAmount = netAmount;
            }
            return result;
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

            var result = await _paysalele.ExecuteQueryList(query, null);

            return result;
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


            var data = await _salEntry.ExecuteQuerySingle(query, null);
            if (data!=null)
            {
                if (data.UserId.IsNotNull())
                {
                    data.GrossSalary = await _payelebus.GetUserSalary(data.UserId);
                    data.VacationBalance = await _lbs.GetLeaveBalance(DateTime.Today, "ANNUAL_LEAVE", data.UserId);
                }
            }
            

            return data;
        }

        public async Task<List<SalaryEntryViewModel>> GetSalaryDetails()
        {
            int publishStatus = (int)DocumentStatusEnum.Published;

            var query = string.Concat($@"Select ps.*, p.""PersonNo"", p.""SponsorshipNo"", ps.""NetAmount"",", Helper.PersonDisplayNameWithSponsorshipNo("p", " as PersonFullName"),
                             $@" From cms.""N_PayrollHR_SalaryEntry"" as ps 
                                 Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                                 Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{_userContext.UserId}'  and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                                 where ps.""PublishStatus""={publishStatus}  and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}' order by ps.""PayrollStartDate"" desc ");
            var querydata = await _salEntry.ExecuteQueryList(query, null);
            return querydata;
        }

        public async Task<List<SalaryEntryViewModel>> GetSalaryElementDetails(string id)
        {

            var query = string.Concat($@"Select se.""Id"", se.""Name"", se.""Amount"", ", Helper.PersonFullNameWithSponsorshipNo("p", " as PersonName"),
                             $@" From cms.""N_PayrollHR_SalaryEntry"" as ps 
                                 Join cms.""N_PayrollHR_SalaryElementEntry"" as se on se.""SalaryEntryId""=ps.""Id"" and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}'
                                 Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=ps.""PersonId""  and p.""IsDeleted""=false   and p.""CompanyId""='{_repo.UserContext.CompanyId}'                              
                                 where ps.""Id""='{id}'  and ps.""IsDeleted""=false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'") ;

            var querydata = await _salEntry.ExecuteQueryList(query, null);
            return querydata;
        }

        public Task<List<SalaryElementEntryViewModel>> GetSalaryElementEntries(string salaryEntryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SalaryEntryViewModel>> GetSuccessfulSalaryEntryList(PayrollRunViewModel viewModel)
        {
            var query = $@"Select se.*, p.""Id"" as PersonId, p.""UserId"" as UserId
                                From cms.""N_PayrollHR_SalaryEntry"" as se 
                                 Join cms.""N_PayrollHR_PayrollRun"" as prr on prr.""Id""=se.""PayrollRunId"" and prr.""IsDeleted""=false and prr.""CompanyId""='{_repo.UserContext.CompanyId}'
                                 Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=se.""PersonId"" and p.""IsDeleted""=false   and p.""CompanyId""='{_repo.UserContext.CompanyId}'                            
                                 where se.""PayrollRunId""='{viewModel.Id}'  and se.""IsDeleted""=false and se.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var querydata = await _salEntry.ExecuteQueryList(query, null);
            return querydata;

        }
    }
}
