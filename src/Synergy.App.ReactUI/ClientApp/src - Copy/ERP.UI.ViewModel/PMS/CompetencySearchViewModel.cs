using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CompetencySearchViewModel : ViewModelBase    {

        public IList<CompetencyViewModel> Competencies { get; set; }
        public long? CompetencyServiceTemplateId { get; set; }
        public long? DocumentOwnerUserId { get; set; }
        public long? PerformanceDocumentId { get; set; }
    }
}
