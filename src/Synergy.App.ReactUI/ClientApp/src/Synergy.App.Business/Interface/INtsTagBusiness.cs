using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface INtsTagBusiness : IBusinessBase<NtsTagViewModel, NtsTag>
    {
        Task<List<TagCategoryViewModel>> GetNtsTagData(NtsTypeEnum ntstype, string ntsId);
        Task<TagCategoryViewModel> GetTagCategoryDataByNoteId(string NoteId);
        Task<TagViewModel> GetTagByNoteId(string NoteId);
        Task GenerateTagsForCategory(string categoryId);
    }
}
