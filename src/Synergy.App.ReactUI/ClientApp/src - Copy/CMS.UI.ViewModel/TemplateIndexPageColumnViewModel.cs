using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TemplateIndexPageColumnViewModel : DataModelBase
    {
        public string HeaderName { get; set; }
        public string ColumnMetadataId { get; set; }
        public bool EnableSorting { get; set; }
        public bool EnableFiltering { get; set; }
        public bool IsForeignKeyTableColumn { get; set; }

        public string ForeignKeyTableAliasName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnHeaderName
        {
            get
            {
                return HeaderName.IsNullOrEmpty() ? ColumnName : HeaderName;
            }

        }
        public string AdvanceSetting { get; set; }
    }

}
