using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
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
        private readonly IPayRollQueryBusiness _payRollQueryBusiness;


        public SalaryEntryBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IPayRollQueryBusiness payRollQueryBusiness,
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
            _payRollQueryBusiness = payRollQueryBusiness;
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

            var allList = await _payRollQueryBusiness.GetElementsForPayslipPdf(id);

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

            var result = await _payRollQueryBusiness.GetSalDistinctElement(payrollRunId);
            string[] data = result.Select(x => x.ElementName).ToArray();
            return data;
        }

        public async Task<List<SalaryElementEntryViewModel>> GetEarningElementForPayslipPdf(string id)
        {

            
            var allList = await _payRollQueryBusiness.GetElementsForPayslipPdf(id);

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
            var result = await _payRollQueryBusiness.GetPaySalaryDetails(search, legalid);
            return result;
        }

        public async Task<List<PayrollSalaryElementViewModel>> GetPaySalaryElementDetails(string payrollRunId, ElementCategoryEnum? elementCategory)
        {

            
            var result = await _payRollQueryBusiness.GetPaySalaryElementDetailsQ1(payrollRunId);



            var result1 = await _payRollQueryBusiness.GetPaySalaryElementDetailsQ2(payrollRunId, elementCategory);

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
            var result = await _payRollQueryBusiness.GetPaySalarySummaryDetails(YearMonth);

            return result;
        }

        public async Task<SalaryEntryViewModel> GetPaySlipHeaderDetails(string id)
        {
            
            var data = await _payRollQueryBusiness.GetPaySlipHeaderDetails(id);
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

            var querydata = await _payRollQueryBusiness.GetSalaryDetails(publishStatus);
            return querydata;
        }

        public async Task<List<SalaryEntryViewModel>> GetSalaryElementDetails(string id)
        {

            var querydata = await _payRollQueryBusiness.GetSalaryElementDetails(id);
            return querydata;
        }

        public Task<List<SalaryElementEntryViewModel>> GetSalaryElementEntries(string salaryEntryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SalaryEntryViewModel>> GetSuccessfulSalaryEntryList(PayrollRunViewModel viewModel)
        {
            var querydata = await _payRollQueryBusiness.GetSuccessfulSalaryEntryList(viewModel);
            return querydata;

        }
    }
}
