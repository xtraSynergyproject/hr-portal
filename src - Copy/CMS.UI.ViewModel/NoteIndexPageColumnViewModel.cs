using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NoteIndexPageColumnViewModel : NoteIndexPageColumn
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
