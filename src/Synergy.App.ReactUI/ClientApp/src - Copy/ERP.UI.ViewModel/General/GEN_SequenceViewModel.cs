using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class GEN_SequenceViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public long NextId { get; set; }
    }
}
