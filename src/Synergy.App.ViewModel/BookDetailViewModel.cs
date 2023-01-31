using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class BookDetailViewModel
    {
        public BookViewModel BookDetails { get; set; }
        public List<IdNameViewModel> BookAllPages { get; set; }
        public BookViewModel BookPageDetails { get; set; }
    }
}
