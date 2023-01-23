using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class TaskIndexPageColumnViewModel : TaskIndexPageColumn
    {
        public string ColumnHeaderName
        {
            get
            {
                return HeaderName.IsNullOrEmpty() ? ColumnName : HeaderName;
            }

        }
        public string AdvanceSetting { get; set; }
        public string TableName { get; set; }
        public bool Select { get; set; }
    }
    
}
