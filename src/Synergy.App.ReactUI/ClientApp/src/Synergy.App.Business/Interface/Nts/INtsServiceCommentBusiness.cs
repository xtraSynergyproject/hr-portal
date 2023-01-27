using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface INtsServiceCommentBusiness : IBusinessBase<NtsServiceCommentViewModel,NtsServiceComment>
    {
        Task<List<NtsServiceCommentViewModel>> GetSearchResult(string ServiceId);
        Task<List<NtsServiceCommentViewModel>> GetCommentTree(string ServiceId, string Id = null);
        Task<List<NtsServiceCommentViewModel>> GetAllCommentTree(string serviceId);
    }
}
