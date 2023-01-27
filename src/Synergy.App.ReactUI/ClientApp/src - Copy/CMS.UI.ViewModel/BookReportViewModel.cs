using CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class BookReportViewModel
    {
        public string Logo { get; set; }
        public DateTime CurrentDate { get; set; }
        public string LoggedInUserName { get; set; }

        public List<NtsViewModel> NtsItems { get; set; }
    }
}
