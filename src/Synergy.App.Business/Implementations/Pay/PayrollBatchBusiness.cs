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
    public class PayrollBatchBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollBatchBusiness
    {
        private readonly IRepositoryQueryBase<SalaryInfoViewModel> _salaryInfo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _queryPayBatch;
        private readonly IPayRollQueryBusiness _payRollQueryBusiness;

        public PayrollBatchBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1
            , IPayRollQueryBusiness payRollQueryBusiness
            , IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch) : base(repo, autoMapper)
        {
            _salaryInfo = salaryInfo;
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            _queryPayBatch = queryPayBatch;
            _payRollQueryBusiness = payRollQueryBusiness;
        }

        public async override Task<CommandResult<NoteViewModel>> Create(NoteViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<NoteViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteViewModel>> Edit(NoteViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<NoteViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<NoteViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<List<IdNameViewModel>> GetPayGroupList()
        {
            var queryData = await _payRollQueryBusiness.GetPayGroupList();
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetPayCalenderList()
        {
            var queryData = await _payRollQueryBusiness.GetPayCalenderList();
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetPayBankBranchList()
        {
            var queryData = await _payRollQueryBusiness.GetPayBankBranchList();
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetSalaryElementIdName()
        {
            var queryData = await _payRollQueryBusiness.GetSalaryElementIdName();
            return queryData;
        }
        public async Task<List<SalaryInfoViewModel>> GetSalaryInfoDetails(string salaryInfoId)
        {
            var queryData = await _payRollQueryBusiness.GetSalaryInfoDetails(salaryInfoId);
            return queryData;

        }
        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoDetails(string elementId,string salaryInfoId, string salaryElementId = null)
        {
            var queryData = await _payRollQueryBusiness.GetSalaryElementInfoDetails(elementId, salaryInfoId, salaryElementId);

           
            return queryData;


        }
        public async Task<bool> DeleteSalaryElement(string NoteId)
        {
            var note = await _repo.GetSingleById(NoteId);
            if (note!=null)
            {
                await _payRollQueryBusiness.DeleteSalaryElement(NoteId);
                
                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<List<IdNameViewModel>> GetPayrollGroupList()
        {
            var result = await _payRollQueryBusiness.GetPayrollGroupList();
            return result;
        }
        public async Task<PayrollGroupViewModel> GetPayrollGroupById(string payGroupId)
        {
            var result = await _payRollQueryBusiness.GetPayrollGroupById(payGroupId);
            return result;
        }

        public async Task<List<PayrollBatchViewModel>> ViewModelList(string PayrollBatchId)
        {
            var result = await _payRollQueryBusiness.ViewModelList(PayrollBatchId);

            return result;
        }
        public async Task<PayrollBatchViewModel> GetSingleById(string payrollBatchId)
        {
            var result = await _payRollQueryBusiness.GetSingleById(payrollBatchId);
            return result;
        }
        public async Task<PayrollBatchViewModel> IsPayrollExist(string payGroupId, string yearmonth)
        {
            var result = await _payRollQueryBusiness.IsPayrollExist(payGroupId, yearmonth);
            return result;
        }


    }
}
