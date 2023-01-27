using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class NtsEmailPageViewModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string ParentId { get; set; }
        public List<NtsEmailPageViewModel> Children { get; set; }

    }
}
