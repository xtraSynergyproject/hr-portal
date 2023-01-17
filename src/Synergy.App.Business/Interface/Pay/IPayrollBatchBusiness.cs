using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IPayrollBatchBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<SalaryInfoViewModel>> GetSalaryInfoDetails(string NoteId);
        Task<List<IdNameViewModel>> GetPayGroupList();
        Task<List<IdNameViewModel>> GetPayCalenderList();
        Task<List<IdNameViewModel>> GetPayBankBranchList();
        Task<List<IdNameViewModel>> GetSalaryElementIdName();
        Task<List<IdNameViewModel>> GetPayrollGroupList();
        Task<PayrollGroupViewModel> GetPayrollGroupById(string payGroupId);
        Task<bool> DeleteSalaryElement(string NoteId);
        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoDetails(string elementId, string salaryInfoId, string salaryElementId = null);
        Task<List<PayrollBatchViewModel>> ViewModelList(string PayrollBatchId);
        Task<PayrollBatchViewModel> GetSingleById(string payrollBatchId);
        Task<PayrollBatchViewModel> IsPayrollExist(string payGroupId, string yearmonth);


    }
}
