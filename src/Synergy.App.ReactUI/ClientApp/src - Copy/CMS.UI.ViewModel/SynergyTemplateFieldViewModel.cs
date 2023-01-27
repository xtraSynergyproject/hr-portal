using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class SynergyTemplateFieldViewModel 
    {
        public string Name { get; set; }
        public long TemplateId { get; set; }
        public string ParentId { get; set; }
        public string Id { get; set; }
        public long? SequenceOrder { get; set; }
        public PageContentTypeEnum PageContentType { get; set; }
        public PageRowTypeEnum PageRowType { get; set; }
        public ComponentTypeEnum? ComponentType { get; set; }
        public DataActionEnum DataAction { get; set; }

        public string FieldName { get; set; }
        public string LabelDisplayName { get; set; }
        public string FieldPartialViewName { get; set; }


    }
}
