using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface INtsTaskCommentBusiness : IBusinessBase<NtsTaskCommentViewModel,NtsTaskComment>
    {
        Task<List<NtsTaskCommentViewModel>> GetSearchResult(string TaskId);
        Task<List<IdNameViewModel>> GetTakCommentUserList(string TaskId);
        Task<List<NtsTaskCommentViewModel>> GetCommentTree(string TaskId, string Id = null);
        Task<List<NtsTaskCommentViewModel>> GetAllCommentTree(string taskId);
    }
}
