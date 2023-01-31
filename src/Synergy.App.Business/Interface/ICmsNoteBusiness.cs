using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{

    public interface ICmsNoteBusiness : IBusinessBase<TemplateViewModel, Template>
    {
        Task ManageTable(string tableMetadataId);
        Task<DataTable> GetIndexPageData(string indexPageTemplateId, NtsNoteOwnerTypeEnum ownerType, DataSourceRequest request);
        Task<NoteIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page);
        Task<string> GetDataById(TemplateTypeEnum viewName, PageViewModel page, string recordId);
        // Task CreateCmsRecord(string data, string pageId);
        Task<CommandResult<NoteTemplateViewModel>> Manage(NoteTemplateViewModel model);
        //Task EditForm(string recordId, string data, string pageId);

    }
}
