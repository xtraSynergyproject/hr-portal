using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface INtsTagBusiness : IBusinessBase<NtsTagViewModel, NtsTag>
    {
        Task<List<TagCategoryViewModel>> GetNtsTagData(NtsTypeEnum ntstype, string ntsId);
        Task<TagCategoryViewModel> GetTagCategoryDataByNoteId(string NoteId);
        Task<TagViewModel> GetTagByNoteId(string NoteId);
        Task GenerateTagsForCategory(string categoryId);
    }
}
