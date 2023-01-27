using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CMS.Common;
using CMS.Data.Model;

namespace CMS.UI.ViewModel
{
    public class MenuGroupDetailsViewModel:MenuGroupDetails
    {
        public string IconFileId { get; set; }
        public bool ExpandHelpPanel { get; set; }
    }
}
