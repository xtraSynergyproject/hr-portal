using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class TableMetadataViewModel : TableMetadata
    {
        public List<ColumnMetadataViewModel> ColumnMetadatas { get; set; }
        public List<ColumnMetadataViewModel> ColumnMetadataView { get; set; }
        public string ModuleName { get; set; }
        public string ColumnMetaDetails { get; set; }
        public string Type { get; set; }
        public string OldName { get; set; }
        public string OldSchema { get; set; }
        public bool IgnorePermission { get; set; }
        public string TemplateId { get; set; }
        public string Json { get; set; }
        public List<TableMetadataViewModel> ChildTable { get; set; }
        public bool IsChildTable { get; set; }
    }
}
