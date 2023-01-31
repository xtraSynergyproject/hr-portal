using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class CompetencyFeedbackDetailsViewModel
    {
        public string Userid { get; set; }
        public string Id { get; set; }

        public List<CompetencyFeedbackUserViewModel> ToppositiveCompetency;
        public List<CompetencyFeedbackUserViewModel> AreaofDevelopment;

    }
}
