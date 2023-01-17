using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class PageIndexViewModel : PageIndex
    {
        public string CreateStyle { get; set; }   
        public string EditButtonIcon { get; set; }
        public string DeleteButtonIcon { get; set; }
        public string RowData { get; set; }
        public List<PageIndexColumnViewModel> SelectedTableRows { get; set; }
       
    }
    
}
