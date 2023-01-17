using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.General
{
    public class HomeViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public List<long> ParentIds { get; set; }
        public string Border { get; set; }

        public List<TaskViewModel> Items { get; set; }
    }

}
