using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class DocumentTypeViewModel : DocumentType
    {
        public DataActionEnum DataAction { get; set; }
        public List<EditorTypeViewModel> EditorTypeList { get; set; }
        public string PortalParentId { get; set; }
    }
}
