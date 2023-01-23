using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface INtsTaskPrecedenceBusiness : IBusinessBase<NtsTaskPrecedenceViewModel, NtsTaskPrecedence>
    {
        Task<List<NtsTaskPrecedenceViewModel>> GetSearchResult(string taskId);
        Task<List<NtsTaskPrecedenceViewModel>> GetTaskSuccessor(string taskId);
        Task<List<NtsTaskPrecedenceViewModel>> GetTaskPredecessor(string taskId);

    }
}
