using Synergy.App.Common;
using System;
using System.Collections.Generic;

namespace Synergy.App.DataModel
{
    public class DocumentType : SettingsBase
    {
        public  DocumentGroup[] Groups { get; set; }
    }
    public class DocumentGroup : DataModelBase
    {
        public string Name { get; set; }
        public DocumentGroupProperty[] Properties { get; set; }
    }
    public class DocumentGroupProperty : DataModelBase
    {
        public string EditorId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string IsMandatory { get; set; }
        public ValidationTypeEnum ValidationType { get; set; }
        public string CustomValidationMessage { get; set; }
    }
}
