using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IPayrollElementBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoForPayrollRun(PayrollRunViewModel viewModel);
        Task<List<SalaryElementInfoViewModel>> GetAllSalaryElementInfo();
        Task CalculateSalaryElement(string personId, string salaryInfoId, double total);
        Task<SalaryInfoViewModel> GetEligiblityForTickets(string userId);
        Task<double> GetUserSalary(string userId, DateTime? asofDate = null);
        Task<double> GetBasicSalary(string userId, DateTime? asofDate = null);
        Task<SalaryInfoViewModel> GetEligiblityForEOS(string userId);
        Task<List<IdNameViewModel>> GetAllUserSalary();
        Task<double> GetUserOneDaySalary(string userId, DateTime? asofDate = null);

        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByNodeId(string nodeId);
        Task DeleteSalaryInfo(string id);
        Task<List<IdNameViewModel>> GetPayrollDeductionElement();
        Task<ElementViewModel> GetPayrollElementById(string Id);
        Task<SalaryElementInfoViewModel> GetSalaryElementInfoListByUserAndELement(string personId, string elementId);
        Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByUser(string userId);
        Task<List<ElementViewModel>> GetElementListForPayrollRun(DateTime asofDate);
    }
}
