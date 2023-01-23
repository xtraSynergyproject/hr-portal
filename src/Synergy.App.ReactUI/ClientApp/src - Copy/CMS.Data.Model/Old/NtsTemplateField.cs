using CMS.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Model
{
    public class NtsTemplateField : DataModelBase
    {
        public long? TemplateId { get; set; }
        public long TemplateFieldId { get; set; }
        public string FieldName { get; set; }
        public string LabelDisplayName { get; set; }
        public string FieldPartialViewName { get; set; }
    }
}
