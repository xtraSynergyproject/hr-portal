using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class TableViewModel : DataModelBase
    {
        public string column_name { get; set; }
        public string data_type { get; set; }
        public string is_nullable { get; set; }
    }
}
