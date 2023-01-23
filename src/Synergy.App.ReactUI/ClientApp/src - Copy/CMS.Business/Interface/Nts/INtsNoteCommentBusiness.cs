using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface INtsNoteCommentBusiness : IBusinessBase<NtsNoteCommentViewModel,NtsNoteComment>
    {
        Task<List<NtsNoteCommentViewModel>> GetSearchResult(string TaskId);
        Task<List<NtsNoteCommentViewModel>> GetCommentTree(string NoteId, string Id = null);
        Task<List<NtsNoteCommentViewModel>> GetAllCommentTree(string NoteId);

    }
}
