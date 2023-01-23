using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class CustomIndexPageColumnViewModel : CustomIndexPageColumn
    {
        public string AdvanceSetting { get; set; }
        public string TableName { get; set; }
        public bool Select { get; set; }
    }

}
