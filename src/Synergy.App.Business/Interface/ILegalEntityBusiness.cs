using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ILegalEntityBusiness : IBusinessBase<LegalEntityViewModel,LegalEntity>
    {
        Task<List<LegalEntityViewModel>> GetData();
        Task<int> GetFinancialYear(DateTime date);
        Task<List<LegalEntityViewModel>> GetLegalEntityByLocationData();
    }
}
