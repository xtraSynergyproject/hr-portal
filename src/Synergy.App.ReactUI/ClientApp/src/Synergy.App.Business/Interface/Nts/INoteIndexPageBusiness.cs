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
    public interface INoteIndexPageTemplateBusiness : IBusinessBase<NoteIndexPageTemplateViewModel, NoteIndexPageTemplate>
    {
        Task<CommandResult<NoteIndexPageTemplateViewModel>> CopyNoteTemplateIndexPageData(NoteIndexPageTemplateViewModel model, string newTempId, CopyTemplateViewModel copyModel = null);
    }
}
