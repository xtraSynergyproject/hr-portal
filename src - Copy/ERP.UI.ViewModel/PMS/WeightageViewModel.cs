using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class WeightageViewModel : ViewModelBase
    {
        public int Weightage { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public long ServiceId { get; set; }
    }
}
