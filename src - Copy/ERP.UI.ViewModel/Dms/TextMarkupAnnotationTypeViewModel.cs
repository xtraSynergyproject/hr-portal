using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class TextMarkupAnnotationTypeViewModel
    {
        public string textMarkupAnnotationType { get; set; }
        public string author { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string subject { get; set; }
        public string note { get; set; }
        public string annotName { get; set; }
        public string shapeAnnotationType { get; set; }
        public long? pageNumber { get; set; }
        public List<CommentsViewModel> comments { get; set; }

    }
}
