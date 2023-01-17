using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;


namespace CMS.UI.ViewModel
{
  public  class PageDetailsViewModel:PageDetails
    {
        public string IconFileId { get; set; }
        public bool ExpandHelpPanel { get; set; }
    }
}
