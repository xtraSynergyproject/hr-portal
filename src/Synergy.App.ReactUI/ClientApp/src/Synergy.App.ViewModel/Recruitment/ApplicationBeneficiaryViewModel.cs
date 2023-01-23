using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ApplicationBeneficiaryViewModel : ApplicationBeneficiary
    {
        public string ApplicationId { get; set; }
      
        public string Name { get; set; }
        public string address { get; set; }
        public string Relationship { get; set; }
        public double? Ratio { get; set; }
        public string ReferenceId { get; set; }
        public string NoteId { get; set; }
        public string Json { get; set; }

    }
}
