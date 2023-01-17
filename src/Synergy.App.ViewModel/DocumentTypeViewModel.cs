using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class DocumentTypeViewModel : DocumentType
    {
        public DataActionEnum DataAction { get; set; }
        public List<EditorTypeViewModel> EditorTypeList { get; set; }
        public string PortalParentId { get; set; }
    }
}
