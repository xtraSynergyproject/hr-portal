using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class FormIndexPageColumnViewModel : FormIndexPageColumn
    {
        public string ColumnName { get; set; }
        public string ColumnHeaderName
        {
            get
            {
                return HeaderName.IsNullOrEmpty() ? ColumnName : HeaderName;
            }

        }
        public string AdvanceSetting { get; set; }
        public bool Select { get; set; }

    }

}
