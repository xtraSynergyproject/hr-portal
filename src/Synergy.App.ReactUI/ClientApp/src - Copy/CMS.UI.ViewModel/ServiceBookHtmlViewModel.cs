using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class ServiceBookHtmlViewModel
    {
        public List<ServiceBookHtmlItemViewModel> ServiceBookHtmlItem { get;set; }

        public List<NtsBookItemViewModel> BookItems { get; set; }
        public List<NtsViewModel> NtsItems { get; set; }

    }
}
