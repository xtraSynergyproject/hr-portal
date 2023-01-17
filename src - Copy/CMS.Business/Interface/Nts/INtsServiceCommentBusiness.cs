using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface INtsServiceCommentBusiness : IBusinessBase<NtsServiceCommentViewModel,NtsServiceComment>
    {
        Task<List<NtsServiceCommentViewModel>> GetSearchResult(string ServiceId);
        Task<List<NtsServiceCommentViewModel>> GetCommentTree(string ServiceId, string Id = null);
        Task<List<NtsServiceCommentViewModel>> GetAllCommentTree(string serviceId);
    }
}
