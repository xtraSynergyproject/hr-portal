using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class FormIndexPageTemplateViewModel : FormIndexPageTemplate
    {
        public string RowData { get; set; }
        public IList<FormIndexPageColumnViewModel> SelectedTableRows { get; set; }
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string TableMetadataId { get; set; }
        public string TemplateCode { get; set; }
        public string PageTitle
        {
            get
            {
                if (Page != null)
                {
                    return Page.Title.Coalesce(Page.Name);
                }
                if (Page.Template != null)
                {
                    return Template.DisplayName.Coalesce(Template.Name);
                }
                return "";
            }
        }
    }
}
