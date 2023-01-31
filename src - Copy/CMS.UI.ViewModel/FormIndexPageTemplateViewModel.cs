using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class FormIndexPageTemplateViewModel : FormIndexPageTemplate
    {
       public string RowData { get; set; }
        public IList<FormIndexPageColumnViewModel> SelectedTableRows { get; set; }
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string TableMetadataId { get; set; }
    }
}
