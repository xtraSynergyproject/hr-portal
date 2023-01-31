using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
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
