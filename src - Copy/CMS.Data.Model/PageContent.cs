using CMS.Common;
using System;

namespace CMS.Data.Model
{
    public class PageContent : DataModelBase
    {
        public string Title { get; set; }
        public string PageId { get; set; }
        public string ParentId { get; set; }
        public string Content { get; set; }
        public string Style { get; set; }
        public string CssClass { get; set; }

        public PageContentTypeEnum PageContentType { get; set; }
        public PageRowTypeEnum PageRowType { get; set; }
        public ComponentTypeEnum? ComponentType { get; set; }
    }

    public class PageContentPublished : PageContent
    {
        public string PageContentId { get; set; }
        public string PublishedBy { get; set; }
        public string PublishedDate { get; set; }
        public int VersionNo { get; set; }
        public bool IsLatest { get; set; }
    }
}
