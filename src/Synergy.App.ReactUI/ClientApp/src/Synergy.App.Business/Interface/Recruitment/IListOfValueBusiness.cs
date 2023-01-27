using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IListOfValueBusiness : IBusinessBase<ListOfValueViewModel, ListOfValue>
    {
        Task<List<ListOfValueViewModel>> GetTreeList(string id);
        Task<List<ListOfValueViewModel>> GetListOfValueByParentAndValue(string type, string value);
    }
}
