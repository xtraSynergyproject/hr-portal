using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class TablePropertiesViewModel 
    {
        public string Id { get; set; }
        public string ColumnName { get; set; }
        public string HeaderName { get; set; }
        public bool EnableSorting { get; set; }
        public bool EnableFiltering { get; set; }
        public int SequenceOrder { get; set; }
       public string AdvanceSetting { get; set; }
    }
    
}
