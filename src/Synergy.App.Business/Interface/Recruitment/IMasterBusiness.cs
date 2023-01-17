using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IMasterBusiness 
    {
        Task<IList<IdNameViewModel>> GetIdNameList(string Type);
        Task<IdNameViewModel> GetJobNameById(string Id);
        Task<IdNameViewModel> GetOrgNameById(string Id);
        Task<List<IdNameViewModel>> GetOrgByJobAddId(string JobAddId);
        Task<IdNameViewModel> GetNationalityIdByName();
        Task<IdNameViewModel> GetOrgNameByBatchId(string Id);
    }
}
