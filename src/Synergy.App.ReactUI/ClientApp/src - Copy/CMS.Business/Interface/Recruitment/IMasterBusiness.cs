using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
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
